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
/// 화면 A Wafer Map E-Test 표식(Spec Out 왼쪽 위 빨강 / Fail Die 오른쪽 아래 남색) 확인: 실제 Wafer 로 화면 A 를 화면 밖에 띄워
/// [분석] 뒤 표식 Die 집합 / 체크 상태 / 색 구간 팝업 표 끝 행 / 상태줄 문구를 쓰고, 체크를 끄고 켠 결과도 쓴다(조회만).
/// RO 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꾼다(GdsKlarfAnalysisFormSmoke 와 같은 방식). 비교는 f07data_mark_check.py.
/// 인자: 출력폴더 STEP_SEQ Factory GdsDevice WaferId LotId Product
/// </summary>
internal static class F07MarkCheck
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
        DACrux.Base.GlobalVariable.Factory = args[2];
        DACrux.Base.GlobalVariable.UserID = "PROBE";
        Type t = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        Type tm = typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap);
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
            Pump(1500);
            DataSet result = form.AnalyzeNow();
            Pump(500);
            DataRow info = result.Tables["ANL_INFO"].Rows[0];
            Console.WriteLine("분석: Step " + info["STEP_SEQS"] + " / E-Test " + info["ET_SRC_FILE_SEQ"] + " " + info["ET_TEST_PROGRAM"] + " / Chain In " + info["IN_CHAIN_DEFECT_COUNT"]);

            var sb = new StringBuilder("X,Y,ET_STATE,ET_FILE_FLAG\r\n");
            foreach (DataRow r in result.Tables["DIE"].Rows)
                sb.AppendLine(r["XINDEX"] + "," + r["YINDEX"] + "," + Col(r, "ET_STATE") + "," + Col(r, "ET_FILE_FLAG"));
            File.WriteAllText(Path.Combine(outDir, "m_data_die.csv"), sb.ToString(), new UTF8Encoding(true));

            var map = (DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap)Get(t, form, "map");
            var chkSpec = (CheckBox)Get(t, form, "chkMarkSpec");
            var chkFile = (CheckBox)Get(t, form, "chkMarkFile");
            var meta = new StringBuilder();
            meta.AppendLine("LABEL\tlblFailMark\t" + ((Control)Get(t, form, "lblFailMark")).Text);
            meta.AppendLine("CHECK\tchkMarkSpec\t" + chkSpec.Text + "\t" + chkSpec.Checked);
            meta.AppendLine("CHECK\tchkMarkFile\t" + chkFile.Text + "\t" + chkFile.Checked);
            Snapshot(meta, "BOTH_ON", form, t, map, tm);
            using (var bmp = new Bitmap(map.Width, map.Height))
            {
                map.DrawToBitmap(bmp, new Rectangle(0, 0, map.Width, map.Height));
                bmp.Save(Path.Combine(outDir, "m_map_both.png"), System.Drawing.Imaging.ImageFormat.Png);
            }

            // 상태줄: Spec Out + Fail Die / Fail Die 만 / E-Test 없음 Die 하나씩
            MethodInfo status = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == "UpdateStatus" && m.GetParameters().Length == 2);
            foreach (DataRow r in result.Tables["DIE"].Rows.Cast<DataRow>()
                .GroupBy(r => Col(r, "ET_STATE") + "|" + Col(r, "ET_FILE_FLAG")).Select(g => g.First()))
            {
                int x = Convert.ToInt32(r["XINDEX"]), y = Convert.ToInt32(r["YINDEX"]);
                status.Invoke(form, new object[] { x, y });
                string text = ((Control)Get(t, form, "lblStatus")).Text.Replace("\r", "").Replace("\n", " | ");
                meta.AppendLine("STATUS\t" + x + "\t" + y + "\t" + Col(r, "ET_STATE") + "\t" + Col(r, "ET_FILE_FLAG") + "\t" + text);
            }

            chkFile.Checked = false;
            Application.DoEvents();
            Snapshot(meta, "SPEC_ONLY", form, t, map, tm);
            chkSpec.Checked = false;
            Application.DoEvents();
            Snapshot(meta, "NONE", form, t, map, tm);
            chkFile.Checked = true;
            Application.DoEvents();
            Snapshot(meta, "FILE_ONLY", form, t, map, tm);
            using (var bmp = new Bitmap(map.Width, map.Height))
            {
                map.DrawToBitmap(bmp, new Rectangle(0, 0, map.Width, map.Height));
                bmp.Save(Path.Combine(outDir, "m_map_file_only.png"), System.Drawing.Imaging.ImageFormat.Png);
            }
            chkSpec.Checked = true;
            Application.DoEvents();
            Snapshot(meta, "BOTH_AGAIN", form, t, map, tm);
            File.WriteAllText(Path.Combine(outDir, "m_meta.tsv"), meta.ToString(), new UTF8Encoding(true));
            form.Close();
        }
        Console.WriteLine("MARK_DUMP=PASS");
        return 0;
    }

    /// <summary>지금 상태: 표식 Die 집합 2개 / 색 구간 팝업 표 E-Test 행.</summary>
    static void Snapshot(StringBuilder meta, string tag, Form form, Type t, object map, Type tm)
    {
        var spec = (HashSet<Point>)Get(tm, map, "_specDies");
        var file = (HashSet<Point>)Get(tm, map, "_fileDies");
        meta.AppendLine("MARK\t" + tag + "\tSPEC\t" + String.Join(";", spec.OrderBy(p => p.Y).ThenBy(p => p.X).Select(p => p.X + " " + p.Y).ToArray()));
        meta.AppendLine("MARK\t" + tag + "\tFILE\t" + String.Join(";", file.OrderBy(p => p.Y).ThenBy(p => p.X).Select(p => p.X + " " + p.Y).ToArray()));
        var band = (DataGridView)Get(t, form, "dgvBand");
        foreach (DataGridViewRow r in band.Rows)
            if (Convert.ToString(r.Tag).StartsWith("FAIL_MARK", StringComparison.Ordinal))
                meta.AppendLine("BAND\t" + tag + "\t" + Convert.ToString(r.Tag) + "\t" + Convert.ToString(r.Cells[0].Value) + "\t" + Convert.ToString(r.Cells[3].Value));
    }

    static string Col(DataRow r, string c)
    {
        return r.Table.Columns.Contains(c) && r[c] != DBNull.Value ? r[c].ToString().Trim() : "";
    }

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
}
