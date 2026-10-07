using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// E-Test Para Limit 설정 화면(frmEtLimitSetup)을 화면 밖에서 띄워 조회 -> 입력 -> 저장 -> 범위 밖 수 -> 삭제 흐름을 검사한다. (CLAUDE-029)
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다(화면 -> RO -> BSL -> 로컬 DB).
/// 준비: 샘플 xlsx 를 EtResultFileParser + BSL 로 FACTORY=PROBE / DEVICE_ID=ET_LIMIT_SMOKE / Slot 4 (RH-HS2615-04) 에 올린다.
/// 인자: [0] E-Test xlsx, [1] 결과 이미지(png), [2] keep(선택: 저장한 파일 / Limit 을 지우지 않음)
/// 기대값(샘플): Para 5 / RESISTANCE_IFORCE 값 21,000 최대 6400 / Upper 1000 -> 범위 밖 1 / LEAKAGE_PARALLEL ±50 -> 4 / LEAKAGE_SERIAL ±50 -> 1
/// </summary>
internal static class EtLimitSetupFormSmoke
{
    private const string Factory = "PROBE";
    private const string Device = "ET_LIMIT_SMOKE";
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        bool keep = args.Length > 2 && args[2] == "keep";
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // ---- 0) 준비: 이전 잔여 정리 + 샘플 업로드 (Slot 4) ----
        foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, "RH-HS2615", "4" }).Rows)
            bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
        foreach (DataRow dr in bsl.GetEtLimit(Factory, Device).Rows)
            bsl.DeleteEtLimit(new[] { Factory, Device, dr["PARA_NAME"].ToString() }, "SMOKE");
        string seq = Upload(bsl, args[0]);
        Console.WriteLine("준비: 파일 번호 " + seq);

        Type type = typeof(DACrux.SEMDMS.ENGUI.frmEtLimitSetup);
        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();
            Set(type, form, "limitService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var txtFactory = (TextBox)Get(type, form, "txtFactory");
            var cboDevice = (ComboBox)Get(type, form, "cboDevice");
            var grid = (DataGridView)Get(type, form, "dgvLimit");
            var status = (Label)Get(type, form, "lblStatus");

            // ---- 1) 조회 ----
            txtFactory.Text = Factory;
            Call(type, form, "LoadDeviceList");
            Check("Device 목록에 ET_LIMIT_SMOKE", cboDevice.Items.Contains(Device), cboDevice.Items.Count.ToString());
            cboDevice.Text = Device;
            Invoke(type, form, "btnSearch_Click");
            var paras = new List<string>();
            foreach (DataGridViewRow r in grid.Rows) paras.Add(Cell(r, "colPara"));
            Check("Para 5행 (대문자)", grid.Rows.Count == 5 && paras.Contains("RESISTANCE_IFORCE") && paras.Contains("LEAKAGE_SERIAL") && paras.TrueForAll(p => p == p.ToUpperInvariant()),
                string.Join(",", paras.ToArray()));
            var res = Row(grid, "RESISTANCE_IFORCE");
            Check("RESISTANCE 통계 (단위 Ohm / 항목 875 / 값 21,000 / 최대 6400 / Limit 없음)", Cell(res, "colUnit") == "Ohm" && Cell(res, "colItems") == "875"
                && Cell(res, "colValues") == "21,000" && Cell(res, "colMax") == "6400" && Cell(res, "colUpper") == "" && Cell(res, "colOut") == "",
                Cell(res, "colUnit") + " / " + Cell(res, "colItems") + " / " + Cell(res, "colValues") + " / " + Cell(res, "colMax"));
            Check("LEAKAGE_PARALLEL 미측정 11", Cell(Row(grid, "LEAKAGE_PARALLEL"), "colNotTested") == "11", Cell(Row(grid, "LEAKAGE_PARALLEL"), "colNotTested"));

            // ---- 2) 입력: Upper 만 -> Enable 자동 체크 / 노란색 ----
            SetCell(res, "colUpper", "1000");
            Check("Upper 입력 -> Enable 자동 체크 / 노란색", Checked(res) && res.Cells["colUpper"].Style.BackColor == Color.LightYellow, Checked(res) + " / " + res.Cells["colUpper"].Style.BackColor.Name);
            var lp = Row(grid, "LEAKAGE_PARALLEL");
            SetCell(lp, "colLower", "-50"); SetCell(lp, "colUpper", "50"); SetCell(lp, "colRemark", "파일 Limit 과 같음");
            var ls = Row(grid, "LEAKAGE_SERIAL");
            SetCell(ls, "colLower", "-50"); SetCell(ls, "colUpper", "50");
            ls.Cells["colEnable"].Value = false;
            Application.DoEvents();
            var co = Row(grid, "CONTINUITY_IFORCE");
            SetCell(co, "colLower", "5"); SetCell(co, "colUpper", "1");
            Check("Lower > Upper -> 붉은색", co.Cells["colLower"].Style.BackColor == Color.MistyRose, co.Cells["colLower"].Style.BackColor.Name);
            Invoke(type, form, "btnSave_Click");
            Check("잘못된 행이 있으면 저장 안 함", form.LastNotice.Contains("CONTINUITY_IFORCE") && form.LastNotice.Contains("Lower 가 Upper"), form.LastNotice.Replace("\r\n", " "));
            SetCell(co, "colLower", ""); SetCell(co, "colUpper", "");
            co.Cells["colEnable"].Value = false;
            Application.DoEvents();

            // ---- 3) 저장 -> 다시 조회 / 범위 밖 수 ----
            Invoke(type, form, "btnSave_Click");
            Console.WriteLine("Save: " + status.Text);
            res = Row(grid, "RESISTANCE_IFORCE"); lp = Row(grid, "LEAKAGE_PARALLEL"); ls = Row(grid, "LEAKAGE_SERIAL"); co = Row(grid, "CONTINUITY_IFORCE");
            Check("저장 완료 3행", status.Text.StartsWith("저장 완료 / 저장 3 / 삭제 0"), status.Text);
            Check("RESISTANCE Upper 1000 (Lower 없음) Enable / 범위 밖 1", Cell(res, "colLower") == "" && Cell(res, "colUpper") == "1000" && Checked(res) && Cell(res, "colOut") == "1",
                Cell(res, "colLower") + " / " + Cell(res, "colUpper") + " / " + Cell(res, "colOut"));
            Check("LEAKAGE_PARALLEL ±50 Enable / 범위 밖 4 / 비고", Cell(lp, "colLower") == "-50" && Cell(lp, "colUpper") == "50" && Cell(lp, "colOut") == "4" && Cell(lp, "colRemark") == "파일 Limit 과 같음",
                Cell(lp, "colOut") + " / " + Cell(lp, "colRemark"));
            Check("LEAKAGE_SERIAL Enable 해제 -> 범위 밖 0 (판정 안 함)", !Checked(ls) && Cell(ls, "colOut") == "0", Checked(ls) + " / " + Cell(ls, "colOut"));
            Check("CONTINUITY Limit 없음 유지", Cell(co, "colUpper") == "" && Cell(co, "colOut") == "", Cell(co, "colOut"));
            Check("수정자 표시", Cell(res, "colUpdate").StartsWith("SMOKE "), Cell(res, "colUpdate"));
            Check("변경 없음 -> 노란색 해제", res.Cells["colUpper"].Style.BackColor != Color.LightYellow, res.Cells["colUpper"].Style.BackColor.Name);

            // ---- 4) 서비스: 소문자 Para 등록 -> 대문자 저장 / 거부 ----
            string[,] lower = { { "leakage_serial", "-50", "50", "y", "소문자 등록" } };
            bsl.SaveEtLimit(new[] { Factory, Device }, lower, "SMOKE");
            DataRow saved = null;
            foreach (DataRow dr in bsl.GetEtLimit(Factory, Device).Rows) if (dr["PARA_NAME"].ToString() == "LEAKAGE_SERIAL") saved = dr;
            Check("소문자 Para / Enable y -> LEAKAGE_SERIAL / Y 로 저장", saved != null && saved["ENABLE_FLAG"].ToString() == "Y" && saved["REMARK"].ToString() == "소문자 등록",
                saved == null ? "없음" : saved["PARA_NAME"] + " / " + saved["ENABLE_FLAG"]);
            Invoke(type, form, "btnSearch_Click");
            Check("다시 조회: LEAKAGE_SERIAL 범위 밖 1", Cell(Row(grid, "LEAKAGE_SERIAL"), "colOut") == "1", Cell(Row(grid, "LEAKAGE_SERIAL"), "colOut"));
            Check("서비스 거부: Lower / Upper 모두 없음", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device }, new[,] { { "RESISTANCE_IFORCE", "", "", "Y", "" } }, "SMOKE")).Contains("하나는"), "");
            Check("서비스 거부: 없는 Para", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device }, new[,] { { "NO_SUCH_PARA", "1", "", "Y", "" } }, "SMOKE")).Contains("없는 Para"), "");
            Check("서비스 거부: 숫자 아님", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device }, new[,] { { "RESISTANCE_IFORCE", "abc", "", "Y", "" } }, "SMOKE")).Contains("숫자가 아닙니다"), "");

            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[1]);
            }

            // ---- 5) 삭제: 값 모두 지우고 저장 (질문 예) / [선택 Limit 삭제] ----
            res = Row(grid, "RESISTANCE_IFORCE");
            SetCell(res, "colUpper", "");
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            Check("값 모두 지움 -> 삭제 질문 -> Limit 삭제", form.LastQuestion.Contains("RESISTANCE_IFORCE") && Cell(Row(grid, "RESISTANCE_IFORCE"), "colUpper") == ""
                && Cell(Row(grid, "RESISTANCE_IFORCE"), "colOut") == "" && status.Text.Contains("삭제 1"), status.Text);
            grid.CurrentCell = Row(grid, "LEAKAGE_PARALLEL").Cells["colPara"];
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnDelete_Click");
            Check("[선택 Limit 삭제] -> LEAKAGE_PARALLEL Limit 없음", Cell(Row(grid, "LEAKAGE_PARALLEL"), "colUpper") == "" && status.Text.StartsWith("삭제 완료"), status.Text);
            form.Close();
        }

        if (!keep)
        {
            foreach (DataRow dr in bsl.GetEtLimit(Factory, Device).Rows)
                bsl.DeleteEtLimit(new[] { Factory, Device, dr["PARA_NAME"].ToString() }, "SMOKE");
            bsl.DeleteEtFile(seq, "SMOKE");
        }
        else Console.WriteLine("KEEP: 파일 번호 " + seq + " / Limit 유지");

        if (failures == 0) { Console.WriteLine("ET_LIMIT_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_LIMIT_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>업로드 화면과 같은 순서로 샘플을 저장한다(Slot 4).</summary>
    private static string Upload(DACrux.SEMDMS.BSL.GdsEtResult bsl, string path)
    {
        var f = DACrux.SEMDMS.ENGUI.EtResultFileParser.Parse(path);
        string seq = bsl.BeginEtFile(new[] { Factory, Device, f.FileName, f.LotId, "4", f.TestProgram, f.TestTime, f.FilePrefix, f.SheetName,
            f.DieCount.ToString(CultureInfo.InvariantCulture), f.ItemCount.ToString(CultureInfo.InvariantCulture), "SMOKE" }).Rows[0]["SRC_FILE_SEQ"].ToString();
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (DataRow dr in bsl.MergeEtItem(new[] { Factory, Device }, f.Items, "SMOKE").Rows) map[dr["ITEM_NAME"].ToString()] = dr["ET_ITEM_SEQ"].ToString();
        bsl.CreateEtDieBatch(seq, f.Dies, "SMOKE");
        foreach (var batch in DACrux.SEMDMS.ENGUI.EtResultFileParser.BuildValueBatches(f, map, 10000)) bsl.CreateEtValueBatch(seq, batch, "SMOKE");
        string result = bsl.CompleteEtFile(seq, "SMOKE").Rows[0]["RESULT"].ToString();
        if (result != "OK") throw new InvalidOperationException("준비 업로드 실패: " + result);
        return seq;
    }

    private static DataGridViewRow Row(DataGridView grid, string para)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colPara") == para) return r;
        throw new InvalidOperationException("Para 행 없음: " + para);
    }

    private static string Cell(DataGridViewRow row, string column)
    {
        return Convert.ToString(row.Cells[column].Value, CultureInfo.InvariantCulture);
    }

    private static bool Checked(DataGridViewRow row)
    {
        object v = row.Cells["colEnable"].Value;
        return v is bool && (bool)v;
    }

    private static void SetCell(DataGridViewRow row, string column, string value)
    {
        row.Cells[column].Value = value;
        Application.DoEvents();
    }

    private static string Throws(Action action)
    {
        try { action(); return ""; } catch (Exception ex) { while (ex.InnerException != null) ex = ex.InnerException; return ex.Message; }
    }

    private static void Check(string name, bool ok, string detail)
    {
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static object Get(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    private static void Set(Type type, object instance, string name, object value)
    {
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, value);
    }

    private static void Invoke(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { null, EventArgs.Empty });
        Application.DoEvents();
    }

    private static void Call(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(instance, null);
        Application.DoEvents();
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창만 바꾼다.</summary>
    private sealed class PreviewFixture : DACrux.SEMDMS.ENGUI.frmEtLimitSetup
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";

        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }

        protected override DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            LastQuestion = text;
            DialogResult answer = Answers.Count > 0 ? Answers.Dequeue() : DialogResult.Yes;
            Console.WriteLine("  [질문] " + text.Split('\r')[0] + " -> " + answer);
            return answer;
        }

        protected override void Notify(string text, MessageBoxIcon icon)
        {
            LastNotice = text;
            Console.WriteLine("  [안내] " + text.Replace("\r\n", " / "));
        }
    }
}
