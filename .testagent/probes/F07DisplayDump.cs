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
using System.Windows.Forms;

/// <summary>
/// F07 데이터 검증 2: 연관 분석 화면 A(frmGdsKlarfAnalysis)를 화면 밖에 띄워 [분석] 뒤 화면에 보이는 값을 CSV 로 쓴다(조회만).
/// RO 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꿔 화면 -> RO -> BSL -> 로컬 DB 를 거친다(GdsKlarfAnalysisFormSmoke 와 같은 방식).
/// 비교는 f07data_display_check.py 가 한다.
/// 인자: 출력폴더 STEP_SEQ Factory GdsDevice WaferId LotId Product
/// </summary>
internal static class F07DisplayDump
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
        long stepSeq = long.Parse(args[1]);
        string factory = args[2], device = args[3];
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "PROBE";
        Type typeA = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        Type typeMap = typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap);
        using (var form = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis())
        {
            form.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(new DACrux.SEMDMS.BSL.DefectMapAnalysis());
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(new DACrux.SEMDMS.BSL.GdsMapView());
            form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(new DACrux.SEMDMS.BSL.GdsChain());
            form.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(new DACrux.SEMDMS.BSL.GdsKlarfAnalysis());
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.ShowErrorDialogs = false;
            form.Show();
            Application.DoEvents();

            var wafer = new DACrux.Base.DPWafer();
            wafer.StepSeq = stepSeq.ToString();
            wafer.WaferID = args[4];
            wafer.LotID = args[5];
            wafer.Product = args[6];
            form.DrawWafer(new[] { wafer });
            var map = (DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap)Get(typeA, form, "map");
            Pump(1500);
            Console.WriteLine("Wafer Map Defect " + map.Defects.Count + " / GDS Device " + form.GdsDevice);

            DataSet result = form.AnalyzeNow();
            Pump(500);
            Console.WriteLine("분석 Chain 영역 " + result.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);

            // 1) 글자: 기능 탭 / 결과 하위 탭 / 범례 / 요약 / 상태줄
            var meta = new StringBuilder();
            foreach (string tc in new[] { "tabList", "tabGdsResult", "tabKlarfResult", "tabEtResult" })
                foreach (TabPage p in ((TabControl)Get(typeA, form, tc)).TabPages)
                    meta.AppendLine("TAB\t" + tc + "\t" + p.Name + "\t" + p.Text);
            meta.AppendLine("SELECTED_TAB\t" + ((TabControl)Get(typeA, form, "tabList")).SelectedTab.Text);
            foreach (Control c in ((FlowLayoutPanel)Get(typeA, form, "flowLegend")).Controls)
                meta.AppendLine("LEGEND\t" + c.Text);
            foreach (string n in new[] { "lblSumGds", "lblSumKlarf", "lblSumEt", "lblMapBasis", "lblStatus", "lblWaferInfo", "lblDeviceValue" })
                meta.AppendLine("LABEL\t" + n + "\t" + ((Control)Get(typeA, form, n)).Text.Replace("\r", "").Replace("\n", " | "));
            foreach (string n in new[] { "rdoFailSpec", "rdoFailFile", "chkChainAll", "chkDefectAll" })
            {
                var b = (ButtonBase)Get(typeA, form, n);
                meta.AppendLine("CHECK\t" + n + "\t" + b.Text + "\t" + (b is RadioButton ? ((RadioButton)b).Checked : ((CheckBox)b).Checked));
            }
            meta.AppendLine("COMBO\tcboDefectScope\t" + ((ComboBox)Get(typeA, form, "cboDefectScope")).Text);
            object sel = Get(typeA, form, "_selectedDie");
            meta.AppendLine("SELECTED_DIE\t" + (sel == null ? "" : sel.ToString()));
            foreach (var band in form.ColorBands)
                meta.AppendLine("BAND\t" + band.From + "\t" + band.To + "\t" + ColorTranslator.ToHtml(band.Color));
            File.WriteAllText(Path.Combine(outDir, "ui_meta.tsv"), meta.ToString(), new UTF8Encoding(true));

            // 2) Map: Die 채움색 / 숫자 / 빨간 삼각형
            var fills = (Dictionary<Point, Color>)Get(typeMap, map, "_dieFill");
            var labels = (Dictionary<Point, int>)Get(typeMap, map, "_dieLabel");
            var fails = (HashSet<Point>)Get(typeMap, map, "_failDies");
            var sb = new StringBuilder("X,Y,FILL,LABEL,FAIL_MARK\r\n");
            foreach (Point p in fills.Keys.Union(labels.Keys).Union(fails).OrderBy(p => p.Y).ThenBy(p => p.X))
            {
                Color c;
                int v;
                sb.AppendLine(p.X + "," + p.Y + "," + (fills.TryGetValue(p, out c) ? ColorTranslator.ToHtml(c) : "") + "," + (labels.TryGetValue(p, out v) ? v.ToString() : "") + "," + (fails.Contains(p) ? "Y" : ""));
            }
            File.WriteAllText(Path.Combine(outDir, "ui_map.csv"), sb.ToString(), new UTF8Encoding(true));

            // 3) 표: 구간 / Die / Defect / 허용오차 / E-Test 연결 / E-Test Fail / Chain(지금 상태)
            Grid((DataGridView)Get(typeA, form, "dgvBand"), "ui_band", null, true);
            Grid((DataGridView)Get(typeA, form, "dgvDie"), "ui_die", null, false);
            Grid((DataGridView)Get(typeA, form, "dgvDefect"), "ui_defect", null, false);
            Grid((DataGridView)Get(typeA, form, "dgvTolerance"), "ui_tolerance", null, false);
            Grid((DataGridView)Get(typeA, form, "dgvEtLink"), "ui_etlink", null, false);
            Grid((DataGridView)Get(typeA, form, "dgvEtFail"), "ui_etfail", null, false);
            var dgvChain = (DataGridView)Get(typeA, form, "dgvChain");
            Grid(dgvChain, "ui_chain_now", null, false);

            // 4) Chain 탭 Wafer 전체 모드
            var chkChainAll = (CheckBox)Get(typeA, form, "chkChainAll");
            bool before = chkChainAll.Checked;
            chkChainAll.Checked = true;
            Invoke(typeA, form, "BindChainGrid");
            Grid(dgvChain, "ui_chain_wafer", null, false);
            chkChainAll.Checked = before;

            // 5) Die 마다 선택 -> Chain 탭(그 Die 의 (Die, Chain)) / Defect 탭 행 수
            var dgvDefect = (DataGridView)Get(typeA, form, "dgvDefect");
            var dieChain = new StringBuilder();
            var dieDefect = new StringBuilder("X,Y,DEFECT_ROWS,DEFECT_TAB\r\n");
            bool header = true;
            foreach (DataRow d in result.Tables["DIE"].Rows)
            {
                int x = Convert.ToInt32(d["XINDEX"]), y = Convert.ToInt32(d["YINDEX"]);
                form.SelectDie(x, y);
                Application.DoEvents();
                Grid(dgvChain, null, dieChain, header, x + "," + y);
                header = false;
                dieDefect.AppendLine(x + "," + y + "," + dgvDefect.Rows.Count + "," + Q(((TabPage)Get(typeA, form, "tabDefect")).Text));
            }
            File.WriteAllText(Path.Combine(outDir, "ui_chain_die.csv"), dieChain.ToString(), new UTF8Encoding(true));
            File.WriteAllText(Path.Combine(outDir, "ui_defect_die.csv"), dieDefect.ToString(), new UTF8Encoding(true));

            using (var bmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                bmp.Save(Path.Combine(outDir, "ui_form.png"), System.Drawing.Imaging.ImageFormat.Png);
            }
            form.Close();
        }
        Console.WriteLine("DISPLAY_DUMP=PASS");
        return 0;
    }

    /// <summary>표의 머리글 / 화면에 보이는 문자열(FormattedValue)을 CSV 로 쓴다. key 가 있으면 앞에 X,Y 열.</summary>
    static void Grid(DataGridView g, string name, StringBuilder into, bool header, string key = null)
    {
        var sb = into ?? new StringBuilder();
        var cols = g.Columns.Cast<DataGridViewColumn>().OrderBy(c => c.DisplayIndex).ToList();
        if (header || into == null)
            sb.AppendLine((key != null ? "X,Y," : "") + String.Join(",", cols.Select(c => Q(c.HeaderText + (c.Visible ? "" : "[숨김]"))).ToArray()) + (name == "ui_band" ? ",TAG" : ""));
        foreach (DataGridViewRow r in g.Rows)
            sb.AppendLine((key != null ? key + "," : "") + String.Join(",", cols.Select(c => Q(Convert.ToString(r.Cells[c.Index].FormattedValue))).ToArray())
                + (name == "ui_band" ? "," + Q(Convert.ToString(r.Tag)) : ""));
        if (into == null)
            File.WriteAllText(Path.Combine(outDir, name + ".csv"), sb.ToString(), new UTF8Encoding(true));
    }

    static string Q(string s) { return "\"" + (s ?? "").Replace("\"", "\"\"") + "\""; }

    static T Ro<T>(object bsl) where T : class
    {
        var ro = (T)FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return ro;
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

    static void Invoke(Type type, object target, string name, params object[] args)
    {
        type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == name && m.GetParameters().Length == args.Length).Invoke(target, args);
    }
}
