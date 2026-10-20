using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// KLARF 업로드 화면(frmKlarfUpload)을 화면 밖에서 실제로 띄워 [파일 선택] -> 읽기 -> [DB 저장] 흐름을 검사한다.
/// Service(Remoting) 대신 RO.KlarfUpload(iKlarfUpload) 생성자로 BSL.KlarfUpload 를 같은 프로세스에 붙인다. 그래서 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다.
/// 확인창(Ask) / 안내창(Notify)은 PreviewFixture 가 정해진 답으로 바꾼다(창을 띄우지 않음). 파일 / 폴더 선택 창 대신 입력란에 값을 넣고 Analyze 를 부른다.
/// 2026-10-08 화면 재배치: [분석] 버튼 / 처리 기록 없음, KLARF Defect Grid / 요약 줄 / 아이콘 버튼 확인을 추가했다.
/// 샘플 Wafer 에 사용자가 올린 데이터가 있으면 읽기 검사만 하고 저장 검사는 건너뛴다(사용자 데이터 보호).
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
        string klarf = Path.GetFullPath(args[1]);
        string imageDir = Path.Combine(Path.GetDirectoryName(klarf), "Defect Images");
        string guard = null;

        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();
            // Remoting 대신 BSL 을 같은 프로세스에서 호출한다
            Set(type, form, "uploadService", new DACrux.SEMDMS.RO.KlarfUpload(new DACrux.SEMDMS.BSL.KlarfUpload()));

            // 사용자 데이터 보호(2026-10-07): 같은 샘플 Wafer 를 사용자가 화면에서 올려 둔 경우 저장 검사의 재등록이 그 데이터를 지우므로 저장 검사는 하지 않는다.
            // (실제로 사용자 업로드 STEP 454 가 Smoke 재등록으로 지워진 적이 있음) STEP COMMENT 의 USER 가 PROBE / SMOKE 인 것만 검사 데이터로 본다.
            guard = CheckUserData(klarf);
            if (guard != null) Console.WriteLine("NOTE: " + guard);

            var txtFile = (TextBox)Get(type, form, "txtFile");
            var txtImageDir = (TextBox)Get(type, form, "txtImageDir");
            var txtProduct = (TextBox)Get(type, form, "txtProduct");
            var grid = (DataGridView)Get(type, form, "dgvWafer");
            var defects = (DataGridView)Get(type, form, "dgvDefect");
            var status = (Label)Get(type, form, "lblStatus");
            var summary = (Label)Get(type, form, "lblSummary");
            var map = (DACrux.Map.DefectMap)Get(type, form, "m_dMap");
            var save = (Button)Get(type, form, "btnSave");
            var browse = (Button)Get(type, form, "btnBrowse");
            var folder = (Button)Get(type, form, "btnImageDir");

            // ---- 1) 화면 구성 (2026-10-08 재배치) ----
            Check("[분석] 버튼 / 처리 기록 없음", !HasField(type, "btnAnalyze") && !HasField(type, "txtLog"), "");
            // POC 버튼 기준: 모든 버튼 16x16 아이콘(99_Resource/ByModule/POC/Button) / 높이 28 (2026-10-16)
            Check("버튼 아이콘(99_Resource/ByModule/POC/Button) 파일 선택 16 / 폴더 선택 16 / DB 저장 16",
                browse.Image != null && browse.Image.Width == 16 && folder.Image != null && folder.Image.Width == 16 && save.Image != null && save.Image.Width == 16,
                Size(browse.Image) + " / " + Size(folder.Image) + " / " + Size(save.Image));
            Check("파일 / 폴더 입력란은 선택 창으로만 지정(읽기 전용)", txtFile.ReadOnly && txtImageDir.ReadOnly, "");
            Check("읽기 전 [DB 저장] 비활성", !save.Enabled, "");
            Type picker = type.Assembly.GetType("DACrux.SEMDMS.ENGUI.FolderPickerDialog", true);
            bool modern = (bool)picker.GetMethod("CanUseModernDialog", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            Check("탐색기 형식 폴더 선택 창 사용 가능(IFileOpenDialog + FOS_PICKFOLDERS)", modern, modern.ToString());

            // ---- 2) 파일 선택 = 바로 읽기 (이미지 폴더 미지정) ----
            txtFile.Text = klarf;
            Call(type, form, "Analyze");
            Console.WriteLine("Status: " + status.Text);
            Console.WriteLine("Summary: " + summary.Text);
            Check("파일 선택 후 바로 읽기 완료(결과만, 사용법 안내 없음)", status.Text.StartsWith("읽기 완료 / Wafer ") && !status.Text.Contains("["), status.Text);
            // 샘플 KLARF 는 사용자 / 다른 작업이 고칠 수 있어(2026-10-08 DeviceID 변경 확인) Device / Step 은 파일에서 읽은 값과 비교한다.
            string fileDevice = KlarfValue(klarf, "DeviceID");
            string fileStep = KlarfValue(klarf, "StepID");
            Check("요약 줄(Lot / Device / Defect / 이미지 0 / 15)", summary.Text.Contains("PFIC_RH-HS2615_C4BUMP") && summary.Text.Contains("Device " + fileDevice) && summary.Text.Contains("Defect 15") && summary.Text.Contains("이미지 0 / 15"), summary.Text);
            Check("Wafer 1행 / 원본·DMS WaferID", grid.Rows.Count == 1 && Cell(grid, "colRawWaferId") == "XD106.02" && Cell(grid, "colWaferId") == "PFIC_RH-HS2615_C4BUMP-02", Cell(grid, "colRawWaferId") + " / " + Cell(grid, "colWaferId"));
            Check("이미지 폴더 미지정 -> 찾음 0 / 15", Cell(grid, "colImages") == "0 / 15", Cell(grid, "colImages"));
            Check("KLARF Defect Grid 15행", defects.Rows.Count == 15, defects.Rows.Count.ToString());
            var d1 = (DACrux.Base.Defect)defects.Rows[0].Tag;
            Check("Defect 1: KLARF Index (-1, 3) / X = Parser 값 / Class 0 / 분류명 Unclassified (ClassLookup = CLASSNUMBER, KLARF 표준) / RoughBin 54 그대로 / 이미지 없음",
                Cell(defects, 0, "colDefectId") == "1" && Cell(defects, 0, "colIndexX") == "-1" && Cell(defects, 0, "colIndexY") == "3"
                && Cell(defects, 0, "colX") == d1.X.ToString("#,0.###", System.Globalization.CultureInfo.InvariantCulture)
                && Math.Abs(d1.X + 23229.42) < 0.01 && Cell(defects, 0, "colRoughBin") == "54" && Cell(defects, 0, "colClass") == "0"
                && Cell(defects, 0, "colClassName") == "Unclassified" && Cell(defects, 0, "colImageFound") == "없음",
                RowText(defects, 0));
            Check("Map 미리보기 Defect 15", map.Defects.Count == 15, map.Defects.Count.ToString());
            Check("저장 값 기본(KLARF Device / Step)", txtProduct.Text == fileDevice && ((TextBox)Get(type, form, "txtStepId")).Text == fileStep, txtProduct.Text + " / " + fileDevice);

            // ---- 3) 폴더 선택 = 같은 파일 다시 읽기 (사용자가 고친 저장 값 유지) ----
            txtProduct.Text = "EDITED_BY_USER";
            txtImageDir.Text = imageDir;
            Call(type, form, "Analyze");
            Check("이미지 폴더 지정 -> 15 / 15 / Grid 이미지 있음", Cell(grid, "colImages") == "15 / 15" && Cell(defects, 0, "colImageFound") == "있음" && summary.Text.Contains("이미지 15 / 15"), Cell(grid, "colImages") + " / " + Cell(defects, 0, "colImageFound"));
            Check("같은 파일 다시 읽어도 고친 Product 유지", txtProduct.Text == "EDITED_BY_USER", txtProduct.Text);
            txtProduct.Text = fileDevice;
            Check("읽은 후 [DB 저장] 활성", save.Enabled, "");

            // ---- 4) Defect Grid 선택은 Map 에 표시하지 않음(2026-10-14 기능 최소화) / 분류 기준 선택 없음, Map 색 = Class (2026-10-15 KLARF 표준) ----
            defects.Rows[0].Selected = true;
            Application.DoEvents();
            Check("Defect 행 선택 -> Map 선택 강조 없음", map.SelectedDefect.Count == 0, map.SelectedDefect.Count.ToString());
            Check("분류 기준 콤보 / 저장 Class 열 없음, Map 색 = CLASS", !HasField(type, "cmbClassSource") && !HasField(type, "colSaveClass") && map.MapType == DACrux.Base.MAP_TYPE.CLASS, map.MapType.ToString());

            // ---- 4-1) Map Setup 에 없는 Device -> [DB 저장] 오류 안내, 저장 안 함 (2026-10-13) ----
            // 서버 미리보기(BeginUpload)에서 INVALID 로 멈추고 작업 폴더를 지우므로 DB 는 바뀌지 않는다(사용자 데이터가 있어도 안전).
            txtProduct.Text = "NO_MAP_SMOKE_DEVICE";
            Call(type, form, "btnSave_Click", null, EventArgs.Empty);
            Check("Map 없는 Device -> 저장 안 함 / 오류 안내", status.Text == "저장 안 함 / 입력 값 확인" && form.LastNotice.Contains("저장할 수 없는 값") && form.LastNotice.Contains("Map 이 없습니다") && Cell(grid, "colState") == "입력 오류",
                status.Text + " / " + form.LastNotice.Replace("\r\n", " "));
            txtProduct.Text = fileDevice;

            if (guard == null)
            {
                // ---- 5) 저장 (이미 있으면 재등록 = 예) ----
                form.Answers.Enqueue(DialogResult.Yes); // 재등록 질문이 나오면 예 (첫 실행이면 질문 없이 저장되고 이 답은 남는다)
                Call(type, form, "btnSave_Click", null, EventArgs.Empty);
                form.Answers.Clear();
                Console.WriteLine("Save1: " + status.Text + " / " + Cell(grid, "colMessage"));
                Check("저장 결과 '저장' / 이미지 15", Cell(grid, "colState") == "저장" && Cell(grid, "colImages") == "저장 15", Cell(grid, "colState") + " / " + Cell(grid, "colImages") + " / " + Cell(grid, "colMessage"));
                long step1;
                Check("STEP_SEQ 표시", long.TryParse(Cell(grid, "colStepSeq"), out step1), Cell(grid, "colStepSeq"));
                Check("완료 안내에 Wafer 결과 + 조회 방법(Lot / 기간)", form.LastNotice.Contains("STEP_SEQ " + step1) && form.LastNotice.Contains("PFIC_RH-HS2615_C4BUMP") && form.LastNotice.Contains("2026-06-05"), form.LastNotice.Replace("\r\n", " "));
                Check("상태 표시줄에 저장 결과", status.Text.StartsWith("저장 1 / 건너뜀 0 / 오류 0"), status.Text);

                // ---- 6) 다시 저장: 등록됨 -> 아니요(건너뜀) / 취소 / 예(재등록) ----
                form.Answers.Enqueue(DialogResult.No);
                Call(type, form, "btnSave_Click", null, EventArgs.Empty);
                Check("다시 저장 시 재등록 질문 표시", form.LastQuestion.Contains("이미 등록된 Wafer") && form.LastQuestion.Contains(step1.ToString()), form.LastQuestion.Split('\r')[0]);
                Check("아니요 -> 건너뜀 / 같은 STEP_SEQ", Cell(grid, "colState") == "건너뜀" && Cell(grid, "colStepSeq") == step1.ToString(), Cell(grid, "colState") + " / " + Cell(grid, "colStepSeq"));
                form.Answers.Enqueue(DialogResult.Cancel);
                Call(type, form, "btnSave_Click", null, EventArgs.Empty);
                Check("취소 -> 저장 취소", status.Text == "저장 취소", status.Text);
                form.Answers.Enqueue(DialogResult.Yes);
                Call(type, form, "btnSave_Click", null, EventArgs.Empty);
                long step2;
                long.TryParse(Cell(grid, "colStepSeq"), out step2);
                Check("예 -> 재등록 새 STEP_SEQ", Cell(grid, "colState") == "저장" && step2 > step1 && Cell(grid, "colMessage").Contains(step1.ToString()), step1 + " -> " + step2 + " / " + Cell(grid, "colMessage"));

                SaveBitmap(form, args[2]);

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
            }
            else
            {
                SaveBitmap(form, args[2]);
                Console.WriteLine("SKIP: 저장 검사(5~7) 건너뜀 - 사용자 데이터 보호");
            }
            form.Close();
        }

        string scope = guard == null ? "" : " (읽기 검사만, 저장 검사는 사용자 데이터 보호로 건너뜀)";
        if (failures == 0) { Console.WriteLine("KLARF_UPLOAD_FORM_SMOKE=PASS" + scope); return 0; }
        Console.WriteLine("KLARF_UPLOAD_FORM_SMOKE=FAIL (" + failures + ")" + scope);
        return 1;
    }

    /// <summary>
    /// 샘플 Wafer 에 검사(PROBE / SMOKE)가 아닌 업로드 데이터가 있으면 설명 문자열, 없으면 null.
    /// BSL BeginUpload(DB 조회만) -> EXISTS 면 STEP COMMENT 확인 -> 작업 폴더는 CancelUpload 로 지운다.
    /// </summary>
    private static string CheckUserData(string klarfPath)
    {
        var bsl = new DACrux.SEMDMS.BSL.KlarfUpload();
        DataTable pv = bsl.BeginUpload(new string[] { "SCK", "SMOKE", Path.GetFileName(klarfPath), "", "", "", "" }, File.ReadAllBytes(klarfPath));
        string key = pv.Rows[0]["UPLOAD_KEY"].ToString();
        try
        {
            foreach (DataRow r in pv.Rows)
            {
                if (r["RESULT"].ToString() != "EXISTS") continue;
                DataTable c = new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null, new string[] { r["EXIST_STEP_SEQ"].ToString(), r["WAFER_SEQ"].ToString() });
                string comment = c.Rows.Count > 0 ? c.Rows[0]["STEP_COMMENT"].ToString() : "";
                if (!comment.Contains("USER=PROBE;") && !comment.Contains("USER=SMOKE;"))
                    return "사용자가 올린 데이터가 있습니다 (STEP_SEQ " + r["EXIST_STEP_SEQ"] + ", " + comment + "). 저장 검사는 재등록으로 이 데이터를 지우므로 실행하지 않습니다.";
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

    /// <summary>KLARF 1.2 헤더 값(예: DeviceID "OMM_Hybrid";)을 따옴표 없이 읽는다.</summary>
    private static string KlarfValue(string path, string key)
    {
        var m = System.Text.RegularExpressions.Regex.Match(File.ReadAllText(path), key + "\\s+\"([^\"]*)\"");
        return m.Success ? m.Groups[1].Value : "";
    }

    private static void SaveBitmap(Form form, string path)
    {
        using (var bitmap = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
            bitmap.Save(path);
        }
    }

    private static void Check(string name, bool ok, string detail)
    {
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static string Size(Image image)
    {
        return image == null ? "(없음)" : image.Width + "x" + image.Height;
    }

    private static string Cell(DataGridView grid, string column)
    {
        return Cell(grid, 0, column);
    }

    private static string Cell(DataGridView grid, int row, string column)
    {
        if (grid.Rows.Count <= row) return "";
        object v = grid.Rows[row].Cells[column].Value;
        return v == null ? "" : v.ToString();
    }

    private static string RowText(DataGridView grid, int row)
    {
        if (grid.Rows.Count <= row) return "(행 없음)";
        var parts = new List<string>();
        foreach (DataGridViewCell c in grid.Rows[row].Cells) parts.Add(grid.Columns[c.ColumnIndex].HeaderText + "=" + c.Value);
        return string.Join(", ", parts.ToArray());
    }

    private static bool HasField(Type type, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) != null;
    }

    private static object Get(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    private static void Set(Type type, object instance, string name, object value)
    {
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, value);
    }

    /// <summary>화면의 private 함수를 부른다(인자 없으면 Analyze 같은 함수, 있으면 버튼 이벤트).</summary>
    private static void Call(Type type, object instance, string method, params object[] args)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, args.Length == 0 ? null : args);
        Application.DoEvents();
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창만 바꾼다. 디자이너 / 컨트롤 / 읽기 / 저장 코드는 그대로 실행한다.</summary>
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
