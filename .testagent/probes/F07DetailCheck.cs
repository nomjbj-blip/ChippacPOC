using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 상세보기(화면 B, GdsDieAnalysisForm) 기능 탭 배치 확인: 실제 분석(AnalyzeDie)으로 화면을 띄워 탭마다 화면을 찍고, 보이는 값을 CSV 로 쓴다(조회만).
/// RO 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꾼다(GdsKlarfAnalysisFormSmoke 와 같은 방식). OpenGL 도면이라 화면에 잠깐 띄워 화면 복사로 찍는다.
/// 비교는 f07data_detail_check.py 가 한다.
/// 인자: 출력폴더 Factory GdsDevice WaferKey STEP_SEQ ET_SRC_FILE_SEQ DieX DieY
/// </summary>
internal static class F07DetailCheck
{
    static string outDir;

    [STAThread]
    static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        outDir = args[0];
        Directory.CreateDirectory(outDir);
        string factory = args[1], device = args[2], waferKey = args[3];
        long stepSeq = long.Parse(args[4]), etSeq = long.Parse(args[5]);
        int x = int.Parse(args[6]), y = int.Parse(args[7]);
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "PROBE";
        var analysis = new DACrux.SEMDMS.BSL.GdsKlarfAnalysis();
        var request = new DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest
        {
            Factory = factory, DeviceId = device, WaferKey = waferKey, StepSeqs = new[] { stepSeq }, ViewSides = new[] { "BACK" }, ChainSide = "FRONT",
            DisplayStepSeq = stepSeq, AreaMode = DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest.AreaSizeTolerance, ToleranceUm = 5,
            ClassBasis = DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest.ClassBasisRoughBin, EtSrcFileSeq = etSeq
        };
        DataSet die = analysis.AnalyzeDie(request, x, y, null);
        Console.WriteLine("AnalyzeDie: DEFECT " + die.Tables["DEFECT"].Rows.Count + " / DIE_CHAIN " + die.Tables["DIE_CHAIN"].Rows.Count
            + " / ET_VALUE " + (die.Tables.Contains("ET_VALUE") ? die.Tables["ET_VALUE"].Rows.Count : -1));
        Csv(die.Tables["DIE_CHAIN"], "b_data_die_chain");
        Csv(die.Tables["DEFECT"], "b_data_defect");
        if (die.Tables.Contains("ET_VALUE")) Csv(die.Tables["ET_VALUE"], "b_data_et_value");
        if (die.Tables.Contains("DEFECT_CHAIN")) Csv(die.Tables["DEFECT_CHAIN"], "b_data_defect_chain");

        Type t = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
        using (var form = new NexplantQMS.GdsMap.GdsDieAnalysisForm(true))
        {
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(new DACrux.SEMDMS.BSL.GdsMapView());
            form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(new DACrux.SEMDMS.BSL.GdsChain());
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(20, 20);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.ShowErrorDialogs = false;
            form.TopMost = true;
            form.Show();
            form.Activate();
            Application.DoEvents();
            System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            Wait(form.ShowDieAsync(die, x, y), 600);
            Pump(1500);
            var tabMain = (TabControl)Get(t, form, "tabMain");
            Meta(form, t, "b_meta_die.tsv");
            Grid((DataGridView)Get(t, form, "dgvChains"), "b_ui_chain");
            Grid((DataGridView)Get(t, form, "dgvDefects"), "b_ui_defect");
            Grid((DataGridView)Get(t, form, "dgvEtValues"), "b_ui_et");
            Grid((DataGridView)Get(t, form, "dgvSummary"), "b_ui_summary");
            // 탭을 처음 열 때 표가 첫 행을 저절로 골라 도면 확대 / 상태줄이 바뀌지 않는지(2026-10-19) 함께 적는다
            var sw = new StringBuilder();
            foreach (string page in new[] { "tabDie", "tabGds", "tabKlarf", "tabEt" })
            {
                string before = form.StatusText;
                tabMain.SelectedTab = (TabPage)Get(t, form, page);
                Pump(900);
                string grid = page == "tabKlarf" ? "dgvDefects" : page == "tabEt" ? "dgvEtValues" : page == "tabDie" ? "dgvSummary" : "dgvChains";
                sw.AppendLine("SWITCH\t" + page + "\t" + ((DataGridView)Get(t, form, grid)).SelectedRows.Count + "\t" + before + "\t" + form.StatusText);
                Capture(form, Path.Combine(outDir, "B_" + page + ".png"));
            }
            File.WriteAllText(Path.Combine(outDir, "b_meta_switch.tsv"), sw.ToString(), new UTF8Encoding(true));

            // 역추적: 일치 Chain(없으면 Fail만, 없으면 첫 Chain) 으로 다시 열기 -> [KLARF] + "(역추적)"
            DataRow focus = die.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToString(r["CLASS4"]) == "BOTH")
                ?? die.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToString(r["ET_STATUS"]) == "F")
                ?? die.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().FirstOrDefault();
            if (focus != null)
            {
                Wait(form.ShowDieAsync(die, x, y, focus["CHAIN_SEQ"].ToString()), 60);
                Pump(1500);
                Meta(form, t, "b_meta_focus.tsv", "FOCUS_SEQ\t" + focus["CHAIN_SEQ"]);
                Grid((DataGridView)Get(t, form, "dgvDefects"), "b_ui_defect_focus");
                Grid((DataGridView)Get(t, form, "dgvEtValues"), "b_ui_et_focus");
                Capture(form, Path.Combine(outDir, "B_focus_tabKlarf.png"));
                tabMain.SelectedTab = (TabPage)Get(t, form, "tabEt");
                Pump(900);
                Capture(form, Path.Combine(outDir, "B_focus_tabEt.png"));
            }
            form.Close();
        }
        Console.WriteLine("DETAIL_DUMP=PASS");
        return 0;
    }

    static void Meta(Form form, Type t, string name, string extra = null)
    {
        var sb = new StringBuilder();
        var tabMain = (TabControl)Get(t, form, "tabMain");
        foreach (TabPage p in tabMain.TabPages) sb.AppendLine("TAB\t" + p.Name + "\t" + p.Text);
        sb.AppendLine("SELECTED_TAB\t" + tabMain.SelectedTab.Name);
        foreach (string n in new[] { "lblWaferInfo", "lblSumTitle", "lblSumGds", "lblSumKlarf", "lblSumEt", "lblDieInfo", "lblFocus", "lblMapTitle", "lblMapSide",
            "lblGdsCriteria", "lblGdsText", "lblChainTitle", "lblKlarfCriteria", "lblKlarfText", "lblDefectCount", "lblEtCriteria", "lblEtText", "lblEtCount",
            "lblDieCriteria", "lblDieText", "lblSummaryTitle", "lblClassBoth", "lblClassFail", "lblClassDefect", "lblClassOk", "lblClassNa" })
        {
            var c = (Control)Get(t, form, n);
            sb.AppendLine("LABEL\t" + n + "\t" + c.Text.Replace("\r", "").Replace("\n", " | ") + "\t" + (c.Visible ? "Y" : "N"));
        }
        sb.AppendLine("CHECK\tchkHitOnly\t" + ((CheckBox)Get(t, form, "chkHitOnly")).Checked);
        sb.AppendLine("COMBO\tcboScope\t" + ((ComboBox)Get(t, form, "cboScope")).Text);
        if (extra != null) sb.AppendLine(extra);
        File.WriteAllText(Path.Combine(outDir, name), sb.ToString(), new UTF8Encoding(true));
    }

    static void Grid(DataGridView g, string name)
    {
        var sb = new StringBuilder();
        var cols = g.Columns.Cast<DataGridViewColumn>().OrderBy(c => c.DisplayIndex).ToList();
        sb.AppendLine("ROW_TAG," + String.Join(",", cols.Select(c => Q(c.HeaderText + (c.Visible ? "" : "[숨김]"))).ToArray()));
        foreach (DataGridViewRow r in g.Rows)
            sb.AppendLine(Q(Convert.ToString(r.Tag)) + "," + String.Join(",", cols.Select(c => Q(Convert.ToString(r.Cells[c.Index].FormattedValue))).ToArray()));
        File.WriteAllText(Path.Combine(outDir, name + ".csv"), sb.ToString(), new UTF8Encoding(true));
    }

    static void Csv(DataTable table, string name)
    {
        var sb = new StringBuilder();
        sb.AppendLine(String.Join(",", table.Columns.Cast<DataColumn>().Select(c => Q(c.ColumnName)).ToArray()));
        foreach (DataRow r in table.Rows)
            sb.AppendLine(String.Join(",", table.Columns.Cast<DataColumn>().Select(c => Q(r[c] == DBNull.Value ? "" : Convert.ToString(r[c], System.Globalization.CultureInfo.InvariantCulture))).ToArray()));
        File.WriteAllText(Path.Combine(outDir, name + ".csv"), sb.ToString(), new UTF8Encoding(true));
    }

    /// <summary>화면 복사. 프로그램이 보는 좌표에 화면 배율(GdsKlarfLayoutPreview 와 같은 계산)을 곱해 실제 화면 영역을 찍는다.</summary>
    static void Capture(Form form, string png)
    {
        form.Activate();
        form.BringToFront();
        Application.DoEvents();
        double scale = ScreenScale();
        Rectangle r = form.Bounds;
        var physical = new Rectangle((int)Math.Round(r.X * scale), (int)Math.Round(r.Y * scale), (int)Math.Round(r.Width * scale), (int)Math.Round(r.Height * scale));
        using (var bmp = new Bitmap(physical.Width, physical.Height))
        {
            using (Graphics g = Graphics.FromImage(bmp)) g.CopyFromScreen(physical.Location, Point.Empty, physical.Size);
            bmp.Save(png, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    static extern int GetDeviceCaps(IntPtr hdc, int index);

    static double ScreenScale()
    {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
            IntPtr hdc = g.GetHdc();
            try
            {
                int logical = GetDeviceCaps(hdc, 8);      // HORZRES
                int physical = GetDeviceCaps(hdc, 118);   // DESKTOPHORZRES
                return logical > 0 ? Math.Max(1.0, physical / (double)logical) : 1.0;
            }
            finally { g.ReleaseHdc(hdc); }
        }
    }

    static string Q(string s) { return "\"" + (s ?? "").Replace("\"", "\"\"") + "\""; }

    static T Ro<T>(object bsl) where T : class
    {
        var ro = (T)FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return ro;
    }

    static void Wait(Task task, int seconds)
    {
        var w = Stopwatch.StartNew();
        while (!task.IsCompleted)
        {
            if (w.Elapsed.TotalSeconds > seconds) throw new TimeoutException("화면 표시 시간 초과");
            Application.DoEvents();
            System.Threading.Thread.Sleep(20);
        }
        task.GetAwaiter().GetResult();
    }

    static void Pump(int milliseconds)
    {
        var w = Stopwatch.StartNew();
        while (w.ElapsedMilliseconds < milliseconds) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    static object Get(Type type, object target, string name)
    {
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null) throw new MissingFieldException(type.Name, name);
        return f.GetValue(target);
    }
}
