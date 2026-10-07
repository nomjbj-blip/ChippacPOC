using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// KLARF 업로드 화면(frmKlarfUpload)을 화면 밖에서 실제로 띄워 [분석] -> [DB 저장] 전체 흐름을 검사한다. (CLAUDE-031, F08 구현 3단계)
/// Service(Remoting) 대신 RO.KlarfUpload(iKlarfUpload) 생성자로 BSL.KlarfUpload 를 같은 프로세스에 붙인다. 그래서 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다.
/// 확인창(Ask) / 안내창(Notify)은 PreviewFixture 가 정해진 답으로 바꾼다(창을 띄우지 않음).
/// 인자: [0] DACrux.SEMDMS.ENGUI.dll, [1] KLARF 파일, [2] 결과 이미지(png), [3] keep(선택: 마지막 저장 Wafer 를 지우지 않음)
/// 설정: 실행 파일 옆 KlarfUploadSmoke.exe.config (Verify-KlarfUpload.ps1 이 Service config 를 복사해 KLARF_IMAGE_ROOT 등을 넣는다).
/// </summary>
internal static class KlarfUploadFormSmoke
{
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        bool keep = args.Length > 3 && args[3] == "keep";
        DACrux.Base.GlobalVariable.Factory = "SCK";
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        Type type = typeof(DACrux.SEMDMS.ENGUI.frmKlarfUpload);

        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();
            // Remoting 대신 BSL 을 같은 프로세스에서 호출한다
            Set(type, form, "uploadService", new DACrux.SEMDMS.RO.KlarfUpload(new DACrux.SEMDMS.BSL.KlarfUpload()));

            // 사용자 데이터 보호(2026-10-07): 같은 샘플 Wafer 를 사용자가 화면에서 올려 둔 경우 Smoke 의 재등록이 그 데이터를 지우므로 멈춘다.
            // (실제로 사용자 업로드 STEP 454 가 Smoke 재등록으로 지워진 적이 있음) STEP COMMENT 의 USER 가 PROBE / SMOKE 인 것만 검사 데이터로 본다.
            string guard = CheckUserData(args[1]);
            if (guard != null)
            {
                Console.WriteLine("STOP: " + guard);
                Console.WriteLine("KLARF_UPLOAD_FORM_SMOKE=SKIPPED (사용자 데이터 보호)");
                form.Close();
                return 2;
            }

            var txtFile = (TextBox)Get(type, form, "txtFile");
            var txtImageDir = (TextBox)Get(type, form, "txtImageDir");
            var grid = (DataGridView)Get(type, form, "dgvWafer");
            var status = (Label)Get(type, form, "lblStatus");
            var log = (TextBox)Get(type, form, "txtLog");
            var map = (DACrux.Map.DefectMap)Get(type, form, "m_dMap");
            var save = (Button)Get(type, form, "btnSave");

            // ---- 1) 저장 전: 분석 버튼 비활성 / 분석 ----
            Check("분석 전 [DB 저장] 비활성", !save.Enabled, "");
            // 이미지 폴더 선택 창(탐색기 형식): 창은 띄우지 않고 COM 생성 / 폴더 선택 옵션만 확인한다
            Type picker = type.Assembly.GetType("DACrux.SEMDMS.ENGUI.FolderPickerDialog", true);
            bool modern = (bool)picker.GetMethod("CanUseModernDialog", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            Check("탐색기 형식 폴더 선택 창 사용 가능(IFileOpenDialog + FOS_PICKFOLDERS)", modern, modern.ToString());
            txtFile.Text = Path.GetFullPath(args[1]);
            // 2026-10-07 화면 수정(사용자): 이미지 폴더 자동 찾기를 빼고 직접 지정한다 -> 지정 전에는 이미지 0 / 15
            Invoke(type, form, "btnAnalyze_Click");
            Check("이미지 폴더 미지정 -> 찾음 0 / 15", Cell(grid, "colImages") == "0 / 15", Cell(grid, "colImages"));
            txtImageDir.Text = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(args[1])), "Defect Images");
            Invoke(type, form, "btnAnalyze_Click");
            Console.WriteLine("Status: " + status.Text);
            Check("분석 완료", status.Text.StartsWith("분석 완료"), status.Text);
            Check("Wafer 1행", grid.Rows.Count == 1, grid.Rows.Count.ToString());
            Check("원본 / DMS WaferID", Cell(grid, "colRawWaferId") == "XD106.02" && Cell(grid, "colWaferId") == "PFIC_RH-HS2615_C4BUMP-02", Cell(grid, "colRawWaferId") + " / " + Cell(grid, "colWaferId"));
            Check("Defect 15 / 검사 Die 26", Cell(grid, "colDefects") == "15" && Cell(grid, "colDies") == "26", Cell(grid, "colDefects") + " / " + Cell(grid, "colDies"));
            Check("이미지 폴더 지정(Defect Images) -> 15 / 15", txtImageDir.Text.EndsWith("Defect Images") && Cell(grid, "colImages") == "15 / 15", txtImageDir.Text + " / " + Cell(grid, "colImages"));
            Check("Map 미리보기 Defect 15", map.Defects.Count == 15, map.Defects.Count.ToString());
            Check("저장 값 기본(KLARF Device / Step)", ((TextBox)Get(type, form, "txtProduct")).Text == "Hybrid_OMM" && ((TextBox)Get(type, form, "txtStepId")).Text == "OMM_Hybrid_C4Bump_Silterra", "");
            Check("분석 후 [DB 저장] 활성", save.Enabled, "");

            // ---- 2) 저장 (이미 있으면 재등록 = 예) ----
            form.Answers.Enqueue(DialogResult.Yes); // 재등록 질문이 나오면 예 (첫 실행이면 질문 없이 저장되고 이 답은 남는다)
            Invoke(type, form, "btnSave_Click");
            form.Answers.Clear();
            Console.WriteLine("Save1: " + status.Text + " / " + Cell(grid, "colMessage"));
            Check("저장 결과 '저장' / 이미지 15", Cell(grid, "colState") == "저장" && Cell(grid, "colImages") == "저장 15", Cell(grid, "colState") + " / " + Cell(grid, "colImages") + " / " + Cell(grid, "colMessage"));
            long step1;
            Check("STEP_SEQ 표시", long.TryParse(Cell(grid, "colStepSeq"), out step1), Cell(grid, "colStepSeq"));
            Check("완료 안내에 조회 방법(Lot / 기간)", form.LastNotice.Contains("PFIC_RH-HS2615_C4BUMP") && form.LastNotice.Contains("2026-06-05"), form.LastNotice.Replace("\r\n", " "));

            // ---- 3) 다시 저장: 등록됨 -> 아니요(건너뜀) ----
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSave_Click");
            Check("다시 저장 시 재등록 질문 표시", form.LastQuestion.Contains("이미 등록된 Wafer") && form.LastQuestion.Contains(step1.ToString()), form.LastQuestion.Split('\r')[0]);
            Check("아니요 -> 건너뜀 / 같은 STEP_SEQ", Cell(grid, "colState") == "건너뜀" && Cell(grid, "colStepSeq") == step1.ToString(), Cell(grid, "colState") + " / " + Cell(grid, "colStepSeq"));

            // ---- 4) 다시 저장: 취소 -> 저장 안 함 ----
            form.Answers.Enqueue(DialogResult.Cancel);
            Invoke(type, form, "btnSave_Click");
            Check("취소 -> 저장 취소", status.Text == "저장 취소", status.Text);

            // ---- 5) 재등록(예) -> 새 STEP ----
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            long step2;
            long.TryParse(Cell(grid, "colStepSeq"), out step2);
            Check("예 -> 재등록 새 STEP_SEQ", Cell(grid, "colState") == "저장" && step2 > step1 && Cell(grid, "colMessage").Contains(step1.ToString()), step1 + " -> " + step2 + " / " + Cell(grid, "colMessage"));

            // ---- 6) 분류 기준 변경 시 Map 다시 그림 / 그림 저장 ----
            var cmb = (ComboBox)Get(type, form, "cmbClassSource");
            cmb.SelectedIndex = 1;
            Application.DoEvents();
            Check("분류 기준 FineBin -> MapType FINEBIN", map.MapType == DACrux.Base.MAP_TYPE.FINEBIN, map.MapType.ToString());
            cmb.SelectedIndex = 0;
            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[2]);
            }
            Console.WriteLine("--- 처리 기록 ---");
            Console.WriteLine(log.Text);

            // ---- 7) 정리 ----
            if (!keep)
            {
                long waferSeq = WaferSeqOf(step2);
                int deleted = new DACrux.SEMDMS.BSL.KlarfUpload().DeleteUploadedStep(step2.ToString(), waferSeq.ToString(), "SMOKE");
                Check("Smoke 데이터 삭제", deleted == 1, "STEP_SEQ " + step2);
            }
            else
            {
                Console.WriteLine("KEEP: STEP_SEQ " + step2);
            }
            form.Close();
        }

        if (failures == 0) { Console.WriteLine("KLARF_UPLOAD_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("KLARF_UPLOAD_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>
    /// 샘플 Wafer 에 검사(PROBE / SMOKE)가 아닌 업로드 데이터가 있으면 설명 문자열, 없으면 null.
    /// BSL BeginUpload(DB 조회만) -> EXISTS 면 STEP COMMENT 확인 -> 작업 폴더는 CancelUpload 로 지운다.
    /// </summary>
    private static string CheckUserData(string klarfPath)
    {
        var bsl = new DACrux.SEMDMS.BSL.KlarfUpload();
        DataTable pv = bsl.BeginUpload(new string[] { "SCK", "SMOKE", Path.GetFileName(klarfPath), "", "", "", "ROUGHBIN" }, File.ReadAllBytes(klarfPath));
        string key = pv.Rows[0]["UPLOAD_KEY"].ToString();
        try
        {
            foreach (DataRow r in pv.Rows)
            {
                if (r["RESULT"].ToString() != "EXISTS") continue;
                DataTable c = new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null, new string[] { r["EXIST_STEP_SEQ"].ToString(), r["WAFER_SEQ"].ToString() });
                string comment = c.Rows.Count > 0 ? c.Rows[0]["STEP_COMMENT"].ToString() : "";
                if (!comment.Contains("USER=PROBE;") && !comment.Contains("USER=SMOKE;"))
                    return "사용자가 올린 데이터가 있습니다 (STEP_SEQ " + r["EXIST_STEP_SEQ"] + ", " + comment + "). Smoke 는 재등록으로 이 데이터를 지우므로 실행하지 않습니다.";
            }
            return null;
        }
        finally
        {
            bsl.CancelUpload(key);
        }
    }

    /// <summary>STEP_SEQ 로 WAFER_SEQ 를 찾는다(기존 화면 BSL GetDefectData 의 Defect 행).</summary>
    private static long WaferSeqOf(long stepSeq)
    {
        DataTable dt = new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectData(stepSeq);
        return Convert.ToInt64(dt.Rows[0]["WAFER_SEQ"]);
    }

    private static void Check(string name, bool ok, string detail)
    {
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static string Cell(DataGridView grid, string column)
    {
        object v = grid.Rows[0].Cells[column].Value;
        return v == null ? "" : v.ToString();
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

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창만 바꾼다. 디자이너 / 컨트롤 / 분석 / 저장 코드는 그대로 실행한다.</summary>
    private sealed class PreviewFixture : DACrux.SEMDMS.ENGUI.frmKlarfUpload
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
