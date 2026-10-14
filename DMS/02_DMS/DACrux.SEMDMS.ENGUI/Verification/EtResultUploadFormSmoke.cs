using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// E-Test 결과 업로드 화면(frmEtResultUpload)을 화면 밖에서 실제로 띄워 [분석] -> [DB 저장] -> 재저장 -> 삭제 흐름을 검사한다. (2026-10-07)
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다. 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다.
/// 확인창(Ask) / 안내창(Notify)은 PreviewFixture 가 정해진 답으로 바꾼다(창을 띄우지 않음). Verify-KlarfUpload.ps1 / KlarfUploadFormSmoke.cs 와 같은 구조.
/// 인자: [0] E-Test xlsx, [1] 결과 이미지(png), [2] 작업 폴더(잘못된 파일명 사본), [3] keep(선택: 마지막 저장 파일을 지우지 않음)
/// 저장 위치: FACTORY=PROBE / DEVICE_ID=ET_SMOKE
/// </summary>
internal static class EtResultUploadFormSmoke
{
    private const string Factory = "PROBE";
    /// <summary>2026-10-12: Device 는 파일명 2칸(샘플 OMM)을 그대로 쓴다.</summary>
    private const string Device = "OMM";
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
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        Type type = typeof(DACrux.SEMDMS.ENGUI.frmEtResultUpload);
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // 이전 실행 잔여 정리 (중복 키 = Factory + WAFER_ID 이므로 Device 와 무관하게 Wafer -02 / -03 을 지운다)
        foreach (string[] lotSlot in new[] { new[] { "RH-HS2615", "2" }, new[] { "RH-HS2615", "3" }, new[] { "U9-HS2633", "1" } })
            foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, lotSlot[0], lotSlot[1] }).Rows)
                bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");

        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();
            Set(type, form, "uploadService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));

            var txtFile = (TextBox)Get(type, form, "txtFile");
            var txtFactory = (TextBox)Get(type, form, "txtFactory");
            var txtDevice = (TextBox)Get(type, form, "txtDevice");
            var txtWarning = (TextBox)Get(type, form, "txtWarning");
            Func<string, string> info = n => ((TextBox)Get(type, form, "txtInfo" + n)).Text;
            var dgvDie = (DataGridView)Get(type, form, "dgvDie");
            var dgvFiles = (DataGridView)Get(type, form, "dgvFiles");
            var status = (Label)Get(type, form, "lblStatus");
            var save = (Button)Get(type, form, "btnSave");
            var txtSlot = (TextBox)Get(type, form, "txtSlot");
            var lblWafer = (Label)Get(type, form, "lblWafer");

            // ---- 1) 분석 ----
            Check("분석 전 [DB 저장] 비활성", !save.Enabled, "");
            txtFile.Text = Path.GetFullPath(args[0]);
            Call(type, form, "AnalyzeFile");
            Console.WriteLine("Status: " + status.Text);
            Check("분석 완료", status.Text.StartsWith("분석 완료"), status.Text);
            Check("Die 미리보기 24행", dgvDie.Rows.Count == 24, dgvDie.Rows.Count.ToString());
            Check("Device = 파일명 Device (OMM, 읽기 전용)", txtDevice.Text == Device && txtDevice.ReadOnly, txtDevice.Text);
            Check("파일 정보 칸: Customer / Wafer Type / Rev / Fab / Test Type / Test Date", info("Customer") == "CAI" && info("WaferType") == "ATV" && info("WaferRev") == "A0"
                && info("Fab") == "(없음)" && info("TestType") == "TSVET" && info("TestDate") == "2026-06-09 16:19:40",
                info("Customer") + " / " + info("WaferType") + " / " + info("WaferRev") + " / " + info("Fab") + " / " + info("TestType") + " / " + info("TestDate"));
            Check("파일 정보 칸: Lot / Lot-Wafer No / Die 24 / Pass 20 / Fail 4 / 시트", info("Lot") == "RH-HS2615" && info("LotWafer") == "RH-HS2615-02" && info("Die") == "24"
                && info("Pass") == "20" && info("Fail") == "4" && info("Sheet") == "DUT Summary", info("Lot") + " / " + info("LotWafer") + " / " + info("Die") + " / " + info("Pass") + " / " + info("Fail"));
            Check("파일 정보 칸: 항목 2,755 / 측정값 66,120 / 미측정 17 / 빈 칸 0 / 경고 없음", info("Items") == "2,755" && info("Values") == "66,120" && info("NotTested") == "17"
                && info("Empty") == "0" && txtWarning.Text == "경고 없음", info("Items") + " / " + info("Values") + " / " + info("NotTested") + " / " + txtWarning.Text);
            Check("Slot No 기본값 2 / WAFER_ID 미리보기", txtSlot.Text == "2" && lblWafer.Text == "WAFER_ID RH-HS2615-02", txtSlot.Text + " / " + lblWafer.Text);
            txtSlot.Text = "7";
            Application.DoEvents();
            Check("Slot 입력 -> 미리보기 갱신", lblWafer.Text == "WAFER_ID RH-HS2615-07", lblWafer.Text);
            Check("Die #20 (7,3) Fail 0x08 미측정 5", DieCell(dgvDie, "#20", "X") == "7" && DieCell(dgvDie, "#20", "Y") == "3"
                && DieCell(dgvDie, "#20", "Pass/Fail") == "Fail" && DieCell(dgvDie, "#20", "Flag") == "0x08" && DieCell(dgvDie, "#20", "'-' 값") == "5",
                DieCell(dgvDie, "#20", "SW Bin") + " / " + DieCell(dgvDie, "#20", "'-' 값"));
            Check("분석 후 [DB 저장] 활성", save.Enabled, "");

            // ---- 2) Factory / Slot 미입력 저장 막음 (Device 는 파일명에서 오므로 입력 칸 없음) ----
            txtFactory.Text = "";
            Invoke(type, form, "btnSave_Click");
            Check("Factory 없으면 저장 안 함", form.LastNotice.StartsWith("Factory 를 입력"), form.LastNotice.Split('\r')[0]);
            txtFactory.Text = Factory;
            txtSlot.Text = "";
            Invoke(type, form, "btnSave_Click");
            Check("Slot No 없으면 저장 안 함", form.LastNotice.Contains("Slot No") && lblWafer.Text.Contains("입력"), form.LastNotice.Split('\r')[0] + " / " + lblWafer.Text);

            // ---- 3) 첫 저장 (Slot 2) ----
            txtSlot.Text = "2";
            Call(type, form, "LoadFileList");
            Check("저장 전 목록 0행", dgvFiles.Rows.Count == 0, dgvFiles.Rows.Count.ToString());
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Invoke(type, form, "btnSave_Click");
            Console.WriteLine("Save1: " + status.Text + " (" + watch.ElapsedMilliseconds + "ms)");
            Check("저장 완료 안내", form.LastNotice.StartsWith("저장이 완료") && form.LastNotice.Contains("RH-HS2615-02"), form.LastNotice.Replace("\r\n", " "));
            Check("목록 1행 READY / 66,120", dgvFiles.Rows.Count == 1 && FileCell(dgvFiles, "LOAD_STATUS") == "READY" && FileCell(dgvFiles, "VALUE_COUNT") == "66120",
                dgvFiles.Rows.Count + " / " + FileCell(dgvFiles, "LOAD_STATUS") + " / " + FileCell(dgvFiles, "VALUE_COUNT"));
            string seq1 = FileCell(dgvFiles, "SRC_FILE_SEQ");
            DataTable values = bsl.GetEtValueList(seq1);
            int notTested = 0;
            foreach (DataRow dr in values.Rows) if (dr["VALUE_STATUS"].ToString() == "N") notTested++;
            Check("DB 측정값 66,120 / 미측정 17 / Die 24", values.Rows.Count == 66120 && notTested == 17 && bsl.GetEtDieList(seq1).Rows.Count == 24,
                values.Rows.Count + " / " + notTested);
            DataRow h1 = bsl.GetEtFile(seq1).Rows[0];
            Check("DB 파일명 칸 (CAI / OMM / ATV / A0 / RH-HS2615-02 / Fab NULL / 접두어 NULL / 규칙 2 / BACK)",
                h1["CUSTOMER"].ToString() == "CAI" && h1["FILE_DEVICE"].ToString() == "OMM" && h1["WAFER_TYPE"].ToString() == "ATV" && h1["WAFER_REV"].ToString() == "A0"
                && h1["LOT_WAFER_NO"].ToString() == "RH-HS2615-02" && h1["FAB_NAME"] == DBNull.Value && h1["FILE_PREFIX"] == DBNull.Value
                && Convert.ToInt32(h1["PARSE_RULE_VER"]) == 2 && h1["VIEW_SIDE"].ToString() == "BACK",
                h1["CUSTOMER"] + "/" + h1["FILE_DEVICE"] + "/" + h1["WAFER_TYPE"] + "/" + h1["WAFER_REV"] + "/" + h1["LOT_WAFER_NO"] + "/" + h1["FAB_NAME"] + "/ver " + h1["PARSE_RULE_VER"] + "/" + h1["VIEW_SIDE"]);
            Check("DB 요약 (Pass 20 / Fail 4 / 미측정 17)", Convert.ToInt32(h1["PASS_DIE_COUNT"]) == 20 && Convert.ToInt32(h1["FAIL_DIE_COUNT"]) == 4 && Convert.ToInt32(h1["NOT_TESTED_COUNT"]) == 17,
                h1["PASS_DIE_COUNT"] + " / " + h1["FAIL_DIE_COUNT"] + " / " + h1["NOT_TESTED_COUNT"]);
            Check("목록 열 제목 (공정 (Test Type) / Fail Die)", dgvFiles.Columns["TEST_PROGRAM"].HeaderText == "공정 (Test Type)" && dgvFiles.Columns["FAIL_DIE_COUNT"].HeaderText == "Fail Die"
                && FileCell(dgvFiles, "FAIL_DIE_COUNT") == "4", dgvFiles.Columns["TEST_PROGRAM"].HeaderText + " / " + FileCell(dgvFiles, "FAIL_DIE_COUNT"));

            // ---- 4) 다시 저장: 아니요 -> 그대로 ----
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSave_Click");
            Check("재저장 질문 표시 (같은 Wafer)", form.LastQuestion.Contains("같은 Wafer") && form.LastQuestion.Contains(seq1), form.LastQuestion.Split('\r')[0]);
            Check("아니요 -> 저장 취소 / 같은 파일 번호", status.Text.StartsWith("저장 취소") && dgvFiles.Rows.Count == 1 && FileCell(dgvFiles, "SRC_FILE_SEQ") == seq1, status.Text);

            // ---- 5) 다시 저장: 예 -> 삭제 후 재등록 ----
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            string seq2 = FileCell(dgvFiles, "SRC_FILE_SEQ");
            Check("예 -> 새 파일 번호 / 1행", dgvFiles.Rows.Count == 1 && long.Parse(seq2) > long.Parse(seq1) && FileCell(dgvFiles, "LOAD_STATUS") == "READY",
                seq1 + " -> " + seq2);

            // (2026-10-12) Device 는 파일명에서 오므로 화면에서 "다른 Device 로 같은 Wafer" 는 만들 수 없다. 서비스 규칙은 GdsEtResultProbe 가 확인한다.
            Check("저장 Device = 파일명 Device", bsl.GetEtFile(seq2).Rows[0]["DEVICE_ID"].ToString() == Device, bsl.GetEtFile(seq2).Rows[0]["DEVICE_ID"].ToString());

            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[1]);
            }

            // ---- 5-1) 목록 더블클릭 -> 업로드 데이터 조회 창 (설계 11절) ----
            type.GetMethod("dgvFiles_CellDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(form, new object[] { null, new DataGridViewCellEventArgs(0, 0) });
            Application.DoEvents();
            var view = form.LastView;
            Check("더블클릭 -> 조회 창 열림", view != null && view.Visible, view == null ? "없음" : view.Text);
            if (view != null)
            {
                Type vt = typeof(DACrux.SEMDMS.ENGUI.frmEtResultView);
                var dgvValue = (DataGridView)Get(vt, view, "dgvValue");
                var dgvViewDie = (DataGridView)Get(vt, view, "dgvDie");
                var lblInfo = (Label)Get(vt, view, "lblInfo");
                var cboPara = (ComboBox)Get(vt, view, "cboPara");
                var txtChain = (TextBox)Get(vt, view, "txtChain");
                var chkNotTested = (CheckBox)Get(vt, view, "chkNotTested");
                int dieCols = 0;
                foreach (DataGridViewColumn c in dgvValue.Columns) if (c.Name.StartsWith("D_") && c.Visible) dieCols++;
                Check("조회 창: 항목 2,755행 / Die 24열 / Die 목록 24", dgvValue.Rows.Count == 2755 && dieCols == 24 && dgvViewDie.Rows.Count == 24,
                    dgvValue.Rows.Count + " / " + dieCols + " / " + dgvViewDie.Rows.Count);
                Check("조회 창: 파일명 칸 / Pass·Fail", lblInfo.Text.Contains("Customer CAI") && lblInfo.Text.Contains("Wafer Rev A0") && lblInfo.Text.Contains("Pass 20 / Fail 4")
                    && lblInfo.Text.Contains("Lot-Wafer No RH-HS2615-02") && lblInfo.Text.Contains("면 BACK"), lblInfo.Text.Replace("\r\n", " / "));
                Check("조회 창: 파일 정보 (WAFER_ID / 미측정 17 / 파일 번호)", lblInfo.Text.Contains("WAFER_ID RH-HS2615-02") && lblInfo.Text.Contains("('-' 17)")
                    && lblInfo.Text.Contains("파일 번호 " + seq2), lblInfo.Text.Replace("\r\n", " / "));
                var col20 = dgvValue.Columns["D_20"];
                Check("조회 창: #20 머리글 (7,3) Fail 색", col20.HeaderText == "#20 (7,3)" && col20.HeaderCell.Style.BackColor == Color.IndianRed, col20.HeaderText);
                Check("조회 창: 첫 항목 Continuity #3 = 0.073", Convert.ToString(dgvValue.Rows[0].Cells["CHAIN_NAME"].Value) == "C4DC_210_3_FH"
                    && Convert.ToString(dgvValue.Rows[0].Cells["PARA_NAME"].Value) == "Continuity_iforce" && Convert.ToString(dgvValue.Rows[0].Cells["D_3"].Value) == "0.073",
                    Convert.ToString(dgvValue.Rows[0].Cells["PARA_NAME"].Value) + " / " + Convert.ToString(dgvValue.Rows[0].Cells["D_3"].Value));
                Check("조회 창: Para 목록 (전체) + 7", cboPara.Items.Count == 8, cboPara.Items.Count.ToString());
                cboPara.SelectedItem = "Leakage_Serial";
                Application.DoEvents();
                int nSerial = dgvValue.Rows.Count;
                txtChain.Text = "pic_comb12_m4";
                Application.DoEvents();
                int nSerialChain = dgvValue.Rows.Count;
                string serial20 = nSerialChain == 1 ? Convert.ToString(dgvValue.Rows[0].Cells["D_20"].Value) : "";
                cboPara.SelectedIndex = 0;
                Application.DoEvents();
                int nChain = dgvValue.Rows.Count;
                txtChain.Text = "";
                chkNotTested.Checked = true;
                Application.DoEvents();
                int nNotTested = dgvValue.Rows.Count;
                Check("조회 창 거르기: Leakage_Serial 156 / + Chain(소문자) 1 / Chain 만 3 / 미측정 있는 항목 6",
                    nSerial == 156 && nSerialChain == 1 && nChain == 3 && nNotTested == 6, nSerial + " / " + nSerialChain + " / " + nChain + " / " + nNotTested);
                Check("조회 창: 미측정 셀 '-' (#20 Leakage_Serial COMB12_M4)", serial20 == "-", serial20);
                chkNotTested.Checked = false;
                Application.DoEvents();
                vt.GetMethod("dgvDie_CellDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view, new object[] { null, new DataGridViewCellEventArgs(0, RowOf(dgvViewDie, "#42")) });
                Application.DoEvents();
                Check("조회 창: Die #42 더블클릭 -> 그 열로 이동", dgvValue.CurrentCell != null && dgvValue.Columns[dgvValue.CurrentCell.ColumnIndex].Name == "D_42",
                    dgvValue.CurrentCell == null ? "없음" : dgvValue.Columns[dgvValue.CurrentCell.ColumnIndex].Name);
                using (var bitmap = new Bitmap(view.Width, view.Height))
                {
                    view.DrawToBitmap(bitmap, new Rectangle(Point.Empty, view.Size));
                    bitmap.Save(Path.Combine(Path.GetDirectoryName(args[1]), "view.png"));
                }
                view.Close();
            }

            // ---- 5-2) 같은 Device 에 다른 Slot(3) -> 새 Wafer 로 저장 (Device 중복 허용) ----
            txtSlot.Text = "3";
            Invoke(type, form, "btnSave_Click");
            Check("같은 Device 다른 Slot -> 새 Wafer 저장 / 목록 2행", form.LastNotice.StartsWith("저장이 완료") && form.LastNotice.Contains("RH-HS2615-03") && dgvFiles.Rows.Count == 2,
                form.LastNotice.Split('\r')[0] + " / 목록 " + dgvFiles.Rows.Count);

            // ---- 5-3) 새 파일명 규칙 (2026-10-12): ...U9-HS2633-01$Silterra$.xlsx (샘플 내용 사본) ----
            Directory.CreateDirectory(args[2]);
            string ruleFile = Path.Combine(args[2], "CAI_OMM_ATV_A0_TSVET_20260902141031_U9-HS2633_U9-HS2633-01$Silterra$.xlsx");
            File.Copy(args[0], ruleFile, true);
            txtFile.Text = ruleFile;
            Call(type, form, "AnalyzeFile");
            Check("새 규칙 이름: Fab Silterra / Lot-Wafer No / Slot 기본값 1 / Test Date", info("Fab") == "Silterra" && info("LotWafer") == "U9-HS2633-01" && info("Lot") == "U9-HS2633"
                && txtSlot.Text == "1" && lblWafer.Text == "WAFER_ID U9-HS2633-01" && info("TestDate") == "2026-09-02 14:10:31" && txtDevice.Text == "OMM",
                info("Fab") + " / " + info("LotWafer") + " / " + txtSlot.Text + " / " + lblWafer.Text + " / " + info("TestDate"));
            Check("새 규칙 이름: 내용 Wafer ID(RH-HS2615) 와 Lot 다름 경고", txtWarning.Text.StartsWith("경고 1건") && txtWarning.Text.Contains("내용의 Wafer ID") && txtWarning.Text.Contains("U9-HS2633"), txtWarning.Text);
            form.Answers.Enqueue(DialogResult.Yes); // 경고가 있어도 저장
            Invoke(type, form, "btnSave_Click");
            DataRow h3 = null;
            foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, "U9-HS2633", "1" }).Rows) h3 = bsl.GetEtFile(dr["SRC_FILE_SEQ"].ToString()).Rows[0];
            Check("새 규칙 저장: FAB_NAME Silterra / LOT U9-HS2633 / WAFER_ID U9-HS2633-01 / Test Date / 규칙 2", h3 != null && h3["FAB_NAME"].ToString() == "Silterra"
                && h3["LOT_ID"].ToString() == "U9-HS2633" && h3["WAFER_ID"].ToString() == "U9-HS2633-01" && h3["TEST_TIME"].ToString() == "20260902141031"
                && Convert.ToInt32(h3["PARSE_RULE_VER"]) == 2 && h3["FILE_NAME"].ToString().EndsWith("$Silterra$.xlsx"),
                h3 == null ? "없음" : h3["FAB_NAME"] + " / " + h3["WAFER_ID"] + " / " + h3["TEST_TIME"] + " / " + h3["FILE_NAME"]);
            string bad7 = Path.Combine(args[2], "CAI_OMM_ATV_TSVET_20260902141031_U9-HS2633_U9-HS2633-01$Silterra$.xlsx");
            File.Copy(args[0], bad7, true);
            txtFile.Text = bad7;
            Call(type, form, "AnalyzeFile");
            Check("7칸 이름 거부 (Wafer Rev 빠짐)", form.LastNotice.Contains("칸이 7개") && !save.Enabled, form.LastNotice.Split('\r')[0]);

            // ---- 5-4) 같은 Wafer 의 다른 공정 (2026-10-12: 키 = Factory + WAFER_ID + 공정) ----
            //      고객 메일 예시 형식: COWET / FT 는 Lot-Wafer No 가 'HS2615-02' (Lot 으로 시작하지 않음), FT 는 $...$ 없음
            foreach (string stepName in new[] { "CAI_OMM_ATV_A0_COWET_20260410134003_RH-HS2615_HS2615-02$Silterra$.xlsx", "CAI_OMM_ATV_A0_FT_20260518152222_RH-HS2615_HS2615-02.xlsx" })
            {
                string stepFile = Path.Combine(args[2], stepName);
                File.Copy(args[0], stepFile, true);
                txtFile.Text = stepFile;
                Call(type, form, "AnalyzeFile");
                string question = form.LastQuestion;
                Invoke(type, form, "btnSave_Click");
                Check("다른 공정 " + info("TestType") + ": Slot 기본값 2 (Lot-Wafer No " + info("LotWafer") + ") / 재등록 질문 없이 저장",
                    txtSlot.Text == "2" && lblWafer.Text == "WAFER_ID RH-HS2615-02" && form.LastQuestion == question && form.LastNotice.StartsWith("저장이 완료"),
                    txtSlot.Text + " / " + lblWafer.Text + " / " + form.LastNotice.Split('\r')[0]);
            }
            var stepRows = bsl.GetEtFileByKey(new[] { Factory, "RH-HS2615", "2" }).Rows;
            var steps = new List<string>();
            foreach (DataRow dr in stepRows) steps.Add(dr["TEST_PROGRAM"] + (dr["FILE_NAME"].ToString().Contains("$") ? "" : "(Fab 없음)"));
            steps.Sort();
            Check("같은 Wafer RH-HS2615-02 에 공정 3개 (COWET / FT / TSVET)", string.Join(",", steps.ToArray()) == "COWET,FT(Fab 없음),TSVET(Fab 없음)", string.Join(",", steps.ToArray()));
            txtFile.Text = Path.Combine(args[2], "CAI_OMM_ATV_A0_COWET_20260410134003_RH-HS2615_HS2615-02$Silterra$.xlsx");
            Call(type, form, "AnalyzeFile");
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSave_Click");
            Check("같은 Wafer + 같은 공정(COWET) 다시 저장 -> 재등록 질문", form.LastQuestion.Contains("같은 공정") && form.LastQuestion.Contains("공정 COWET"), form.LastQuestion.Split('\r')[0]);

            // ---- 6) 잘못된 파일명 -> 분석 실패 / 저장 비활성 ----
            Directory.CreateDirectory(args[2]);
            string bad = Path.Combine(args[2], "ETEST_SAMPLE.xlsx");
            File.Copy(args[0], bad, true);
            txtFile.Text = bad;
            Call(type, form, "AnalyzeFile");
            Check("잘못된 파일명 거부 / 저장 비활성", form.LastNotice.Contains("파일명 형식") && !save.Enabled, form.LastNotice.Replace("\r\n", " "));

            // ---- 7) 삭제 ----
            if (!keep)
            {
                // 잘못된 파일명 분석 뒤라 Device 칸이 비어 있으므로 다시 채우고 목록을 조회한다 (2026-10-12 보정: 빈 목록으로 0행 통과하던 문제)
                txtDevice.Text = Device;
                Call(type, form, "LoadFileList");
                Check("삭제 전 목록 5행 (TSVET -02 / -03 / U9 / COWET / FT)", dgvFiles.Rows.Count == 5, dgvFiles.Rows.Count.ToString());
                for (int n = 0; n < 10 && dgvFiles.Rows.Count > 0; n++)
                {
                    dgvFiles.CurrentCell = dgvFiles.Rows[0].Cells[0];
                    form.Answers.Enqueue(DialogResult.Yes);
                    Invoke(type, form, "btnDelete_Click");
                }
                Check("선택 파일 삭제 -> 목록 0행", dgvFiles.Rows.Count == 0 && status.Text.StartsWith("삭제 완료"), status.Text);
            }
            else
            {
                Console.WriteLine("KEEP: SRC_FILE_SEQ " + seq2);
            }
            form.Close();
        }

        if (failures == 0) { Console.WriteLine("ET_UPLOAD_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_UPLOAD_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    private static void Check(string name, bool ok, string detail)
    {
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static string DieCell(DataGridView grid, string dut, string column)
    {
        foreach (DataGridViewRow row in grid.Rows)
            if (Convert.ToString(row.Cells["DUT"].Value) == dut) return Convert.ToString(row.Cells[column].Value);
        return "";
    }

    private static int RowOf(DataGridView grid, string dut)
    {
        foreach (DataGridViewRow row in grid.Rows)
            if (Convert.ToString(row.Cells["DUT"].Value) == dut) return row.Index;
        return -1;
    }

    private static string FileCell(DataGridView grid, string column)
    {
        if (grid.Rows.Count == 0) return "";
        return Convert.ToString(grid.Rows[0].Cells[column].Value);
    }

    private static object Get(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    private static void Set(Type type, object instance, string name, object value)
    {
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, value);
    }

    /// <summary>버튼 이벤트 처리기 호출 (sender, EventArgs)</summary>
    private static void Invoke(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { null, EventArgs.Empty });
        Application.DoEvents();
    }

    /// <summary>인자 없는 내부 메서드 호출</summary>
    private static void Call(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(instance, null);
        Application.DoEvents();
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창만 바꾼다. 디자이너 / 컨트롤 / 분석 / 저장 코드는 그대로 실행한다.</summary>
    private sealed class PreviewFixture : DACrux.SEMDMS.ENGUI.frmEtResultUpload
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public Form LastView;

        /// <summary>조회 창을 화면 밖에 띄운다.</summary>
        protected override void ShowResultView(Form view)
        {
            view.StartPosition = FormStartPosition.Manual;
            view.Location = new Point(-32000, -32000);
            view.ShowInTaskbar = false;
            LastView = view;
            view.Show(this);
        }

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
