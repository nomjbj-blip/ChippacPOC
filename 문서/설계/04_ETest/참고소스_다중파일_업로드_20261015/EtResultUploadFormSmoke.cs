using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// E-Test 결과 업로드 화면(frmEtResultUpload)과 결과 조회 화면(frmEtResultView)을 화면 밖에서 실제로 띄워 검사한다.
/// (2026-10-07 작성, 2026-10-14 Upload 재설계로 다시 작성, 2026-10-15 Upload / View 분리로 다시 작성)
/// 업로드: 여러 파일 분석 -> 상태 -> 공통 결과 Control(정보 / 1단계 Die / 2단계 정의 Para 값 거르기) -> 저장 확인 문구(파일별 Parameter x Die = 값 + 합계)
///         -> 저장 -> 중복 건너뛰기 / 덮어쓰기 -> 등록 변경 후 실패 / 다시 분석.
/// 조회: Device / 공정 / Lot 조회 -> 파일 목록(추가 Para / Spec 바뀜 / Map) -> 같은 결과 Control 에 저장 결과(업로드 미리보기와 같은 값) -> Map 좌표 다시 계산 / 삭제.
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다. 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다.
/// 확인창(Ask) / 안내창(Notify) / 파일 고르기는 Fixture 가 정해진 값으로 바꾼다(창을 띄우지 않음). 분석은 순서를 보장하려고 백그라운드를 끈다.
/// 인자: [0] E-Test xlsx, [1] 업로드 화면 이미지(png, 같은 폴더에 view.png 도 만든다), [2] 작업 폴더(파일명을 바꾼 사본), [3] 고객 Spec 표(탭 구분 텍스트), [4] keep(선택)
/// 시험 데이터: FACTORY=PROBE / Device = 파일명 Device(OMM) / 공정 TSVET 등록 Parameter 37(고객 Spec 표) + Spec. 끝나면 지운다.
/// </summary>
internal static class EtResultUploadFormSmoke
{
    private const string Factory = "PROBE";
    private const string Device = "OMM";
    private const string Lot = "RH-HS2615";
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        bool keep = args.Length > 4 && args[4] == "keep";
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        Type type = typeof(DACrux.SEMDMS.ENGUI.frmEtResultUpload);
        Type viewType = typeof(DACrux.SEMDMS.ENGUI.frmEtResultView);
        Type ctlType = typeof(DACrux.SEMDMS.ENGUI.EtResultDataControl);
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // ---- 0) 준비: 잔여 정리 / 파일명을 바꾼 사본 / 등록 Parameter(TSVET 37) + Spec ----
        Cleanup(bsl);
        string sample = args[0];
        string work = args[2];
        Directory.CreateDirectory(work);
        string name = Path.GetFileName(sample);                                    // CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx
        string cowet = Copy(sample, work, name.Replace("_TSVET_", "_COWET_"));    // 등록 Parameter 없음
        string broken = Copy(sample, work, "CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615.xlsx");   // 7칸 -> 읽기 실패
        string twin = Copy(sample, work, name.Replace("20260609161940", "20260610090000"));        // 같은 Wafer + 공정 -> 목록 안 중복
        string noSlot = Copy(sample, work, name.Replace("RH-HS2615-02.xlsx", "RH-HS2615-XX.xlsx"));  // Slot 없음 -> 입력
        string slot3 = Copy(sample, work, name.Replace("RH-HS2615-02.xlsx", "RH-HS2615-03.xlsx"));   // 다른 Wafer
        var spec = DACrux.SEMDMS.ENGUI.EtSpecTable.Parse(File.ReadAllText(args[3])).Where(r => r.TestProgram == "TSVET").ToList();
        string[,] reg = new string[spec.Count, 4];
        string[,] lim = new string[spec.Count, 7];
        for (int i = 0; i < spec.Count; i++)
        {
            reg[i, 0] = spec[i].HeaderText; reg[i, 1] = spec[i].Unit; reg[i, 2] = ""; reg[i, 3] = "";
            lim[i, 0] = spec[i].HeaderText; lim[i, 1] = spec[i].Lower; lim[i, 2] = spec[i].Upper; lim[i, 3] = "Y"; lim[i, 4] = spec[i].Unit; lim[i, 5] = spec[i].SpecText; lim[i, 6] = "";
        }
        bsl.RegisterEtParameters(new[] { Factory, Device, "TSVET", "SPEC", "" }, reg, "SMOKE");
        bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" }, lim, "SMOKE");
        Console.WriteLine("준비: " + Factory + " / " + Device + " / TSVET 등록 Parameter " + spec.Count + " + Spec");

        using (var form = new UploadFixture())
        using (var view = new ViewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();
            Set(type, form, "uploadService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var txtFactory = (TextBox)Get(type, form, "txtFactory");
            var queue = (DataGridView)Get(type, form, "dgvQueue");
            var result = (DACrux.SEMDMS.ENGUI.EtResultDataControl)Get(type, form, "resultView");
            var info = (DataGridView)Get(ctlType, result, "dgvInfo");
            var dies = (DataGridView)Get(ctlType, result, "dgvDie");
            var values = (DataGridView)Get(ctlType, result, "dgvValue");
            var chkFail = (CheckBox)Get(ctlType, result, "chkFailOnly");
            var chkDash = (CheckBox)Get(ctlType, result, "chkNotTested");
            var txtFind = (TextBox)Get(ctlType, result, "txtFind");
            var valueTitle = (Label)Get(ctlType, result, "lblValueTitle");
            var count = (Label)Get(ctlType, result, "lblCount");
            txtFactory.Text = Factory;

            // ---- 1) 파일 5개 추가 -> 분석 / 상태 ----
            form.Files = new[] { sample, cowet, broken, twin, noSlot };
            Invoke(type, form, "btnAddFiles_Click");
            DataGridViewRow ra = QRow(queue, sample), rb = QRow(queue, cowet), rc = QRow(queue, broken), re = QRow(queue, twin), rf = QRow(queue, noSlot);
            Check("파일 5개 분석: 샘플 준비 / COWET 등록 없음 / 7칸 읽기 실패 / 목록 안 중복 / Slot 없음",
                Cell(ra, "colQStatus") == "준비" && Cell(rb, "colQStatus") == "오류" && Cell(rb, "colQMessage").StartsWith("등록 Parameter 없음")
                && Cell(rc, "colQStatus") == "오류" && Cell(rc, "colQMessage").StartsWith("파일 읽기 실패")
                && Cell(re, "colQStatus") == "오류" && Cell(re, "colQMessage").StartsWith("목록 안 중복")
                && Cell(rf, "colQStatus") == "오류" && Cell(rf, "colQMessage").StartsWith("Slot 을 입력"),
                Cell(ra, "colQStatus") + " / " + Cell(rb, "colQStatus") + " / " + Cell(rc, "colQStatus") + " / " + Cell(re, "colQStatus") + " / " + Cell(rf, "colQStatus") + " | " + Cell(ra, "colQMessage"));
            Check("샘플 행: OMM / TSVET / Slot 2 / WAFER_ID / 헤더 2,755 / 등록 37 / 저장 37 / 누락 0 / Spec Fail Die 1 / Map 없음",
                Cell(ra, "colQDevice") == "OMM" && Cell(ra, "colQProgram") == "TSVET" && Cell(ra, "colQSlot") == "2" && Cell(ra, "colQWafer") == "RH-HS2615-02"
                && Cell(ra, "colQHeader") == "2,755" && Cell(ra, "colQRegistered") == "37" && Cell(ra, "colQSaved") == "37" && Cell(ra, "colQMissing") == "0"
                && Cell(ra, "colQFailDie") == "1" && Cell(ra, "colQMap") == "없음", Cell(ra, "colQFailDie") + " / " + Cell(ra, "colQMap"));

            // ---- 2) 공통 결과 Control (업로드 미리보기): 정보 / 1단계 Die / 2단계 정의 Para 값 ----
            queue.ClearSelection();
            ra.Selected = true;
            Application.DoEvents();
            var d44 = DieRow(dies, "#44");
            var d20 = DieRow(dies, "#20");
            Check("미리보기: 제목 / 정보 첫 행 파일명 / Die 24 / #44 Fail 6 (빨간 행) / #20 Pass (DUT Flag Fail 은 참고)",
                ((Label)Get(ctlType, result, "lblInfoTitle")).Text == "업로드 미리보기 (저장 전) - 준비" && Cell(info.Rows[0], "colInfoName") == "파일명"
                && Cell(info.Rows[0], "colInfoValue") == name && dies.Rows.Count == 24 && Cell(d44, "colDJudge") == "Fail" && Cell(d44, "colDFail") == "6"
                && d44.DefaultCellStyle.BackColor == Color.MistyRose && Cell(d20, "colDJudge") == "Pass" && Cell(d20, "colDFlag").StartsWith("Fail"),
                "Die " + dies.Rows.Count + " / #44 " + Cell(d44, "colDJudge") + " " + Cell(d44, "colDFail"));
            Check("2단계 (Die 고르지 않음): 37 x 24 = 888행 / 건수 표시", values.Rows.Count == 888 && count.Text == "표시 888 / 전체 888 (Parameter 37 x Die 24)"
                && valueTitle.Text.EndsWith("(전체 Die)"), values.Rows.Count + " / " + count.Text);
            SelectDies(dies, d44);
            int rows44 = values.Rows.Count;
            string title44 = valueTitle.Text;
            chkFail.Checked = true;
            Application.DoEvents();
            var v0 = (DataRowView)values.Rows[0].DataBoundItem;
            Check("2단계 #44: 37행 -> [Fail 만] 6행 / DUT #44 / 헤더 원문 / 저장 이름 / Chain / 값 > Upper 715 / 판정 Fail / 사유 Upper 초과",
                rows44 == 37 && title44.EndsWith("(DUT 44)") && values.Rows.Count == 6 && v0["DUT"].ToString() == "#44" && v0["HEADER"].ToString().StartsWith("Resistance_iforce=8.0mA@")
                && v0["NAME"].ToString().StartsWith("RESISTANCE_IFORCE_8P0MA_") && v0["CHAIN"].ToString().Length > 0 && v0["UPPER"].ToString() == "715"
                && double.Parse(v0["VALUE"].ToString(), CultureInfo.InvariantCulture) > 715 && v0["JUDGE"].ToString() == "Fail" && v0["REASON"].ToString() == "Upper 초과",
                rows44 + " -> " + values.Rows.Count + " / " + v0["HEADER"] + " = " + v0["VALUE"]);
            SelectDies(dies, d20);
            int fail20 = values.Rows.Count;
            SelectDies(dies);
            int failAll = values.Rows.Count;
            SelectDies(dies, d20, d44);
            int fail2 = values.Rows.Count;
            string title2 = valueTitle.Text;
            Check("Die 고르기 + [Fail 만]: #20 0행 / 전체 Die 6행 / #20 + #44 6행 (Die 2개)", fail20 == 0 && failAll == 6 && fail2 == 6 && title2.EndsWith("(Die 2개)"),
                fail20 + " / " + failAll + " / " + fail2 + " / " + title2);
            chkFail.Checked = false;
            SelectDies(dies);
            chkDash.Checked = true;
            Application.DoEvents();
            string dashInfo = InfoValue(info, "'-' 값 / 빈 칸");
            int dashRows = values.Rows.Count;
            bool dashAll = values.Rows.Cast<DataGridViewRow>().All(r => ((DataRowView)r.DataBoundItem)["VALUE"].ToString() == "-");
            Check("['-' 만]: 행 수 = 정보 '-' 값 / 값은 모두 '-'", dashInfo.StartsWith(dashRows + " / ") && dashAll, dashRows + " / 정보 " + dashInfo);
            chkDash.Checked = false;
            txtFind.Text = "8p0ma";
            Application.DoEvents();
            int findRows = values.Rows.Count;
            bool findAll = values.Rows.Cast<DataGridViewRow>().All(r => ((DataRowView)r.DataBoundItem)["NAME"].ToString().ToUpperInvariant().Contains("8P0MA")
                || ((DataRowView)r.DataBoundItem)["HEADER"].ToString().ToUpperInvariant().Contains("8P0MA"));
            Check("찾기 '8p0ma'(대소문자 무시): 일부 행 / 모두 이름 또는 헤더에 포함", findRows > 0 && findRows < 888 && findAll, findRows + "행");
            txtFind.Text = string.Empty;
            Application.DoEvents();
            DataTable uploadValues = ((DataView)values.DataSource).Table;
            List<string> uploadInfoKeys = InfoKeys(info);

            // ---- 3) Slot 입력 -> 준비, 모두 저장 (확인 문구: 파일별 Parameter x Die = 값 + 합계) ----
            SetSlot(type, form, queue, rf, "4");
            Check("Slot 4 입력 -> 준비 / WAFER_ID RH-HS2615-04", Cell(rf, "colQStatus") == "준비" && Cell(rf, "colQWafer") == "RH-HS2615-04", Cell(rf, "colQStatus") + " / " + Cell(rf, "colQWafer"));
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSaveAll_Click");
            string seqA = Cell(ra, "colQSeq");
            string q = form.LastQuestion;
            Check("저장 확인: 2개 / 파일별 'Parameter 37 x Die 24 = 값 888' 2줄 / 합계 1,776 / 등록 안 된 헤더 안내 / 다시 업로드 안내 / 오류 3개 제외",
                q.StartsWith("E-Test 결과 파일 2개를 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.") && Occurrences(q, "|  TSVET  |  Parameter 37 x Die 24 = 값 888") == 2
                && q.Contains(Path.GetFileName(sample) + "  |") && q.Contains("합계: 값 1,776") && q.Contains("등록되지 않은 헤더는 저장하지 않습니다 (")
                && q.Contains("다시 업로드해야 보입니다") && q.Contains("오류 3개는 저장하지 않습니다"), q.Replace("\r\n", " / "));
            Check("모두 저장: 저장됨 2 / 파일 번호 / 메시지 Spec Fail Die 1 · Map 없음 / 안내에 조회 화면",
                Cell(ra, "colQStatus") == "저장됨" && Cell(rf, "colQStatus") == "저장됨" && seqA.Length > 0 && Cell(ra, "colQMessage").Contains("Spec Fail Die 1")
                && Cell(ra, "colQMessage").Contains("Map 없음") && form.LastNotice.StartsWith("저장 2 / 실패 0 / 건너뜀 0") && form.LastNotice.Contains("E-Test 결과 조회 화면"),
                Cell(ra, "colQMessage"));
            var busyBar = (DACrux.SEMDMS.Control.ProgressStatusBar)Get(type, form, "busyBar");
            Check("진행 표시줄: '저장 (2개) 완료'(처리 시간 없음) / 작업 없음", busyBar.LastResult == "저장 (2개) 완료" && !busyBar.IsBusy, busyBar.DisplayText);
            DataRow saved = bsl.GetEtFile(seqA).Rows[0];
            Check("DB: 저장 37 / 등록 37 / 헤더 2,755 / Pass 23 · Fail 1 / 값 888 / 규칙 3",
                Convert.ToInt32(saved["ITEM_COUNT"]) == 37 && Convert.ToInt32(saved["DEFINED_COUNT"]) == 37 && Convert.ToInt32(saved["HEADER_COUNT"]) == 2755
                && Convert.ToInt32(saved["PASS_DIE_COUNT"]) == 23 && Convert.ToInt32(saved["FAIL_DIE_COUNT"]) == 1 && Convert.ToInt32(saved["VALUE_COUNT"]) == 888
                && Convert.ToInt32(saved["PARSE_RULE_VER"]) == 3, saved["WAFER_ID"] + " / 값 " + saved["VALUE_COUNT"]);
            Check("저장 뒤 미리보기 제목: 저장됨", ((Label)Get(ctlType, result, "lblInfoTitle")).Text == "업로드 미리보기 (저장 전) - 저장됨", ((Label)Get(ctlType, result, "lblInfoTitle")).Text);

            // ---- 4) 조회 화면: 목록 / 같은 결과 Control / Map 좌표 다시 계산 ----
            view.StartPosition = FormStartPosition.Manual;
            view.Location = new Point(-32000, -32000);
            view.ShowInTaskbar = false;
            view.Show();
            Application.DoEvents();
            Set(viewType, view, "viewService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var vFactory = (TextBox)Get(viewType, view, "txtFactory");
            var vDevice = (ComboBox)Get(viewType, view, "cboDevice");
            var vProgram = (ComboBox)Get(viewType, view, "cboProgram");
            var vLot = (TextBox)Get(viewType, view, "txtLot");
            var files = (DataGridView)Get(viewType, view, "dgvFiles");
            var vStatus = (Label)Get(viewType, view, "lblStatus");
            var vBar = (DACrux.SEMDMS.Control.ProgressStatusBar)Get(viewType, view, "busyBar");
            var vResult = (DACrux.SEMDMS.ENGUI.EtResultDataControl)Get(viewType, view, "resultView");
            var vInfo = (DataGridView)Get(ctlType, vResult, "dgvInfo");
            var vDies = (DataGridView)Get(ctlType, vResult, "dgvDie");
            var vValues = (DataGridView)Get(ctlType, vResult, "dgvValue");
            var vChkFail = (CheckBox)Get(ctlType, vResult, "chkFailOnly");
            vFactory.Text = Factory;
            InvokeNoArgs(viewType, view, "LoadDeviceList");
            int deviceIndex = vDevice.Items.IndexOf(Device);
            vDevice.SelectedIndex = deviceIndex;   // 공정 목록 + 조회
            Application.DoEvents();
            Check("조회 화면: Device 목록에 OMM / 고르면 공정 (전체) + TSVET / 파일 2건 / 첫 행 자동 선택 + 결과 표시",
                deviceIndex >= 0 && vProgram.Items.Count == 2 && (string)vProgram.Items[0] == "(전체)" && (string)vProgram.Items[1] == "TSVET" && files.Rows.Count == 2
                && files.SelectedRows.Count == 1 && vDies.Rows.Count == 24, "Device " + deviceIndex + " / 공정 " + vProgram.Items.Count + " / 파일 " + files.Rows.Count);
            var fa = FileRow(files, seqA);
            Check("목록 열: Spec Fail Die 1 / 저장 Para 37 / 값 888 / 추가 Para 0 / Spec 바뀜 N / Map 없음 / 열 제목",
                Convert.ToString(fa.Cells["FAIL_DIE_COUNT"].Value) == "1" && Convert.ToString(fa.Cells["ITEM_COUNT"].Value) == "37" && Convert.ToString(fa.Cells["VALUE_COUNT"].Value) == "888"
                && Convert.ToString(fa.Cells["ADDED_PARA_COUNT"].Value) == "0" && Convert.ToString(fa.Cells["SPEC_CHANGED"].Value) == "N"
                && fa.Cells["MAP_ID"].FormattedValue.ToString() == "없음" && files.Columns["FAIL_DIE_COUNT"].HeaderText == "Spec Fail Die"
                && files.Columns["ADDED_PARA_COUNT"].HeaderText == "추가 Para" && files.Columns["SPEC_CHANGED"].HeaderText == "Spec 바뀜" && !files.Columns["DEVICE_ID"].Visible,
                fa.Cells["ADDED_PARA_COUNT"].Value + " / " + fa.Cells["SPEC_CHANGED"].Value + " / " + fa.Cells["MAP_ID"].FormattedValue);
            SelectFile(files, fa);
            Check("파일 고르기: 제목 저장된 결과 / 파일 번호 / 진행 표시줄 결과 조회 완료",
                ((Label)Get(ctlType, vResult, "lblInfoTitle")).Text == "저장된 결과 - 파일 번호 " + seqA && InfoValue(vInfo, "파일 번호") == seqA
                && vBar.LastResult == "결과 조회 (파일 번호 " + seqA + ") 완료", ((Label)Get(ctlType, vResult, "lblInfoTitle")).Text + " / " + vBar.LastResult);
            List<string> viewInfoKeys = InfoKeys(vInfo);
            Check("정보 항목 순서: 업로드 미리보기와 같음(안내 / 충돌 행 제외)", string.Join("|", uploadInfoKeys.Where(k => k != "안내" && k != "충돌").ToArray())
                == string.Join("|", viewInfoKeys.Where(k => k != "안내" && k != "충돌").ToArray()), string.Join(",", viewInfoKeys.ToArray()));
            Check("정보 값: Die 24 / 저장 Parameter 37 (판정 37) / 저장 값 888 / Spec Pass · Fail 23 / 1 / '-' 값 업로드와 같음",
                InfoValue(vInfo, "Die") == "24" && InfoValue(vInfo, "저장 Parameter") == "37 (판정 37)" && InfoValue(vInfo, "저장 값") == "888 (Parameter 37 x Die 24)"
                && InfoValue(vInfo, "Spec Pass / Fail Die") == "23 / 1" && InfoValue(vInfo, "'-' 값 / 빈 칸") == dashInfo,
                InfoValue(vInfo, "저장 Parameter") + " / " + InfoValue(vInfo, "Spec Pass / Fail Die") + " / " + InfoValue(vInfo, "'-' 값 / 빈 칸"));
            string diff = CompareValues(uploadValues, ((DataView)vValues.DataSource).Table);
            Check("2단계 값: 업로드 미리보기와 조회 888행이 같음(행 순서 / DUT / 저장 이름 / 헤더 / Chain / 값 / Lower / Upper / 판정 / 사유)", diff.Length == 0, diff.Length == 0 ? "888행 같음" : diff);
            SelectDies(vDies, DieRow(vDies, "#44"));
            vChkFail.Checked = true;
            Application.DoEvents();
            Check("조회 #44 + [Fail 만]: 6행 / #44 Die 행 빨간색", vValues.Rows.Count == 6 && DieRow(vDies, "#44").DefaultCellStyle.BackColor == Color.MistyRose, vValues.Rows.Count + "행");
            vChkFail.Checked = false;
            SelectDies(vDies);
            vLot.Text = "hs2615";
            vProgram.SelectedIndex = 1;
            InvokeArgs(viewType, view, "btnSearch_Click", new object[] { null, EventArgs.Empty });
            int lotFound = files.Rows.Count;
            bool kept = files.SelectedRows.Count == 1 && RowSeq(files.SelectedRows[0]) == seqA && vDies.Rows.Count == 24;
            vLot.Text = "zz";
            InvokeArgs(viewType, view, "btnSearch_Click", new object[] { null, EventArgs.Empty });
            Check("조건: Lot 'hs2615'(대소문자 무시) + 공정 TSVET 2건 / 고른 파일 유지 / Lot 'zz' 0건(결과 비움)", lotFound == 2 && kept && files.Rows.Count == 0 && vDies.Rows.Count == 0,
                lotFound + " / " + files.Rows.Count);
            vLot.Text = string.Empty;
            vProgram.SelectedIndex = 0;
            view.Answers.Enqueue(DialogResult.Yes);
            InvokeArgs(viewType, view, "btnRemap_Click", new object[] { null, EventArgs.Empty });
            Check("[Map 좌표 다시 계산]: Map ID OMM 이 없어 안내 / 목록 다시 조회", view.LastNotice.Contains("Map 없음") && view.LastNotice.Contains("파일 2") && files.Rows.Count == 2,
                view.LastNotice);

            // ---- 5) 중복: 건너뛰기 / 덮어쓰기 ----
            Invoke(type, form, "btnClear_Click");
            Check("목록 비우기 -> 결과 Control 비움", dies.Rows.Count == 0 && values.Rows.Count == 0, dies.Rows.Count + " / " + values.Rows.Count);
            form.Files = new[] { sample, slot3 };
            Invoke(type, form, "btnAddFiles_Click");
            ra = QRow(queue, sample);
            var r3 = QRow(queue, slot3);
            Check("다시 추가: 샘플은 중복(DB 파일 번호) / Slot 3 은 준비", Cell(ra, "colQStatus") == "중복" && Cell(ra, "colQMessage").Contains("파일 번호 " + seqA)
                && Cell(r3, "colQStatus") == "준비", Cell(ra, "colQStatus") + " / " + Cell(r3, "colQStatus"));
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSaveAll_Click");
            Check("중복 건너뛰기(아니요): 확인 문구에 2개 + 이미 있는 1개 / 샘플 건너뜀 / Slot 3 저장됨 / 기존 파일 그대로",
                form.LastQuestion.StartsWith("E-Test 결과 파일 2개를 저장합니다.") && form.LastQuestion.Contains("이미 있는 1개") && Cell(ra, "colQStatus") == "건너뜀"
                && Cell(r3, "colQStatus") == "저장됨" && bsl.GetEtFile(seqA).Rows.Count == 1, Cell(ra, "colQStatus") + " / " + Cell(r3, "colQStatus"));
            string seq3 = Cell(r3, "colQSeq");
            Invoke(type, form, "btnClear_Click");
            form.Files = new[] { sample };
            Invoke(type, form, "btnAddFiles_Click");
            ra = QRow(queue, sample);
            form.Answers.Enqueue(DialogResult.Yes);
            queue.ClearSelection();
            ra.Selected = true;
            Application.DoEvents();
            Invoke(type, form, "btnSaveSelected_Click");
            string seqA2 = Cell(ra, "colQSeq");
            Check("중복 덮어쓰기(예, 선택 저장): 새 파일 번호 / 기존 파일 삭제", Cell(ra, "colQStatus") == "저장됨" && seqA2.Length > 0 && seqA2 != seqA && bsl.GetEtFile(seqA).Rows.Count == 0,
                seqA + " -> " + seqA2);

            // ---- 6) 분석 뒤 등록 변경 -> 저장 실패 / 다시 분석하면 누락 경고 ----
            Invoke(type, form, "btnClear_Click");
            form.Files = new[] { slot3.Replace("-03.xlsx", "-05.xlsx") };
            File.Copy(sample, form.Files[0], true);
            Invoke(type, form, "btnAddFiles_Click");
            var r5 = QRow(queue, form.Files[0]);
            Thread.Sleep(1100);   // 등록 시각이 앞서 저장한 파일 등록 시각보다 뒤가 되게(DATE 는 초 단위)
            bsl.RegisterEtParameters(new[] { Factory, Device, "TSVET", "SPEC", "" }, new[,] { { "Resistance_iforce=8.0mA@ZZ_NOT_IN_FILE_FH", "Ohm", "", "" } }, "SMOKE");
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSaveAll_Click");
            Check("분석 뒤 등록 Parameter 추가 -> 저장 실패(다시 분석 안내) / 파일 안 남음", Cell(r5, "colQStatus") == "실패" && Cell(r5, "colQMessage").Contains("다시 분석")
                && bsl.GetEtFileByKey(new[] { Factory, Lot, "5", "TSVET" }).Rows.Count == 0, Cell(r5, "colQMessage"));
            Invoke(type, form, "btnClear_Click");
            Invoke(type, form, "btnAddFiles_Click");
            r5 = QRow(queue, form.Files[0]);
            Check("다시 분석: 등록 38 / 저장 37 / 누락 1 -> 경고", Cell(r5, "colQStatus") == "경고" && Cell(r5, "colQRegistered") == "38" && Cell(r5, "colQSaved") == "37"
                && Cell(r5, "colQMissing") == "1" && Cell(r5, "colQMessage").Contains("누락 1"), Cell(r5, "colQMessage"));
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSaveAll_Click");
            Check("경고 파일 저장 확인 문구: 경고 1개 포함", form.LastQuestion.Contains("경고 1개 포함"), form.LastQuestion.Replace("\r\n", " / "));
            DataRow f5 = bsl.GetEtFileByKey(new[] { Factory, Lot, "5", "TSVET" }).Rows[0];
            string seq5 = f5["SRC_FILE_SEQ"].ToString();
            DataRow f5Detail = bsl.GetEtFile(seq5).Rows[0];
            Check("경고(누락) 파일 저장: 등록 38 / 저장 37 / 누락 1 기록", Cell(r5, "colQStatus") == "저장됨" && Convert.ToInt32(f5Detail["DEFINED_COUNT"]) == 38
                && Convert.ToInt32(f5Detail["ITEM_COUNT"]) == 37 && Convert.ToInt32(f5Detail["MISSING_COUNT"]) == 1, Cell(r5, "colQMessage"));

            // ---- 7) 조회 화면: 추가 Para / Spec 바뀜 / 삭제 ----
            vProgram.SelectedIndex = 0;
            vLot.Text = string.Empty;
            InvokeArgs(viewType, view, "btnSearch_Click", new object[] { null, EventArgs.Empty });
            var fOld = FileRow(files, seq3);
            var fNew = FileRow(files, seq5);
            Check("추가 Para: 등록 전에 올린 파일 1 (다시 업로드해야 보임) / 등록 뒤 올린 파일 0 (업로드 때 누락은 빼고 셈)",
                files.Rows.Count == 4 && Convert.ToString(fOld.Cells["ADDED_PARA_COUNT"].Value) == "1" && Convert.ToString(fNew.Cells["ADDED_PARA_COUNT"].Value) == "0"
                && vStatus.Text.Contains("추가 Para 있는 파일 3"), files.Rows.Count + "건 / " + fOld.Cells["ADDED_PARA_COUNT"].Value + " / " + fNew.Cells["ADDED_PARA_COUNT"].Value + " / " + vStatus.Text);
            int specIndex = spec.FindIndex(s => s.Upper.Length > 0);
            Thread.Sleep(1100);   // Spec 수정 시각이 판정 시각보다 뒤가 되게
            string newUpper = (double.Parse(spec[specIndex].Upper, CultureInfo.InvariantCulture) + 1).ToString("R", CultureInfo.InvariantCulture);
            bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" }, new[,] { { spec[specIndex].HeaderText, spec[specIndex].Lower, newUpper, "Y", spec[specIndex].Unit, spec[specIndex].SpecText, "" } }, "SMOKE");
            InvokeArgs(viewType, view, "btnSearch_Click", new object[] { null, EventArgs.Empty });
            fOld = FileRow(files, seq3);
            SelectFile(files, fOld);
            List<string> notes = InfoKeys(vInfo).Select((k, i) => k == "안내" ? Cell(vInfo.Rows[i], "colInfoValue") : null).Where(t => t != null).ToList();
            Check("Spec 수정 뒤: Spec 바뀜 Y (4건) / 정보 안내 2줄(추가 Para 1개 다시 업로드 / 판정 뒤 Spec 바뀜) / Upper 는 지금 Spec",
                Convert.ToString(fOld.Cells["SPEC_CHANGED"].Value) == "Y" && files.Rows.Cast<DataGridViewRow>().All(r => Convert.ToString(r.Cells["SPEC_CHANGED"].Value) == "Y")
                && notes.Count == 2 && notes[0].StartsWith("추가 Para 1개") && notes[0].Contains("다시 업로드") && notes[1].StartsWith("Spec 이 판정 뒤 바뀌었습니다")
                && ((DataView)vValues.DataSource).Table.Rows.Cast<DataRow>().Any(r => r["HEADER"].ToString() == spec[specIndex].HeaderText && r["UPPER"].ToString() == newUpper),
                fOld.Cells["SPEC_CHANGED"].Value + " / " + string.Join(" | ", notes.ToArray()));
            int before = files.Rows.Count;
            view.Answers.Enqueue(DialogResult.Yes);
            InvokeArgs(viewType, view, "btnDelete_Click", new object[] { null, EventArgs.Empty });
            Check("[선택 파일 삭제]: 확인 문구에 YMS 데이터 / 목록 1건 줄어듦 / 삭제한 파일 없음", view.LastQuestion.Contains("YMS") && files.Rows.Count == before - 1
                && vStatus.Text.StartsWith("Device " + Device) && bsl.GetEtFile(seq3).Rows.Count == 0, before + " -> " + files.Rows.Count);

            // 화면 이미지: 조회 화면(첫 파일, #44 선택) / 업로드 화면(샘플 행 선택)
            view.Size = new Size(1300, 800);
            Application.DoEvents();
            SelectFile(files, files.Rows[0]);
            SelectDies(vDies, DieRow(vDies, "#44"));
            Save(view, Path.Combine(Path.GetDirectoryName(Path.GetFullPath(args[1])), "view.png"));
            view.Close();
            Invoke(type, form, "btnClear_Click");
            form.Files = new[] { sample, cowet };
            Invoke(type, form, "btnAddFiles_Click");
            queue.ClearSelection();
            QRow(queue, sample).Selected = true;
            Application.DoEvents();
            form.Size = new Size(1300, 800);
            Application.DoEvents();
            Save(form, args[1]);
            form.Close();
        }

        if (!keep) Cleanup(bsl);
        else Console.WriteLine("KEEP: " + Factory + " / " + Device + " TSVET 저장 파일 / 등록 Parameter 유지");
        if (failures == 0) { Console.WriteLine("ET_UPLOAD_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_UPLOAD_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>시험 파일(Lot RH-HS2615 Slot 2~5, 모든 공정)과 PROBE / OMM 등록 Parameter(Spec 같이)를 지운다.</summary>
    private static void Cleanup(DACrux.SEMDMS.BSL.GdsEtResult bsl)
    {
        foreach (string slot in new[] { "2", "3", "4", "5" })
            foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, Lot, slot }).Rows)
                bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
        foreach (DataRow dr in bsl.GetEtParaProgramList(Factory).Rows)
        {
            if (dr["DEVICE_ID"].ToString() != Device) continue;
            string program = dr["TEST_PROGRAM"].ToString();
            string[] names = bsl.GetEtParameterList(Factory, Device, program).Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()).ToArray();
            if (names.Length > 0) bsl.DeleteEtParameters(new[] { Factory, Device, program }, names, "SMOKE");
        }
    }

    private static string Copy(string source, string folder, string name)
    {
        string path = Path.Combine(folder, name);
        File.Copy(source, path, true);
        return path;
    }

    private static DataGridViewRow QRow(DataGridView grid, string path)
    {
        string name = Path.GetFileName(path);
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colQFile") == name) return r;
        throw new InvalidOperationException("대기 목록 행 없음: " + name);
    }

    private static DataGridViewRow DieRow(DataGridView grid, string dut)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colDDut") == dut) return r;
        throw new InvalidOperationException("Die 행 없음: " + dut);
    }

    private static DataGridViewRow FileRow(DataGridView grid, string seq)
    {
        foreach (DataGridViewRow r in grid.Rows) if (RowSeq(r) == seq) return r;
        throw new InvalidOperationException("파일 목록 행 없음: " + seq);
    }

    private static string RowSeq(DataGridViewRow row)
    {
        return Convert.ToString(((DataRowView)row.DataBoundItem)["SRC_FILE_SEQ"], CultureInfo.InvariantCulture);
    }

    /// <summary>Die 표에서 주어진 행만 고른다(없으면 선택 해제 = 전체 Die).</summary>
    private static void SelectDies(DataGridView grid, params DataGridViewRow[] rows)
    {
        grid.ClearSelection();
        foreach (DataGridViewRow r in rows) r.Selected = true;
        Application.DoEvents();
    }

    /// <summary>조회 화면 파일 목록에서 한 행을 고른다(선택 변경 처리기가 결과를 읽는다).</summary>
    private static void SelectFile(DataGridView grid, DataGridViewRow row)
    {
        grid.CurrentCell = row.Cells[grid.Columns.Cast<DataGridViewColumn>().First(c => c.Visible).Index];
        grid.ClearSelection();
        row.Selected = true;
        Application.DoEvents();
    }

    private static List<string> InfoKeys(DataGridView info)
    {
        return info.Rows.Cast<DataGridViewRow>().Select(r => Cell(r, "colInfoName")).ToList();
    }

    private static string InfoValue(DataGridView info, string key)
    {
        foreach (DataGridViewRow r in info.Rows) if (Cell(r, "colInfoName") == key) return Cell(r, "colInfoValue");
        return null;
    }

    /// <summary>
    /// 업로드 미리보기와 조회의 2단계 표(모든 Die x Parameter)를 행 순서와 DUT + 저장 이름으로 맞춰 비교한다. 값 / Lower / Upper 는 숫자로 비교(DB 는 NUMBER).
    /// 같으면 빈 문자열, 다르면 첫 차이.
    /// </summary>
    private static string CompareValues(DataTable upload, DataTable view)
    {
        if (upload.Rows.Count != view.Rows.Count) return "행 수 " + upload.Rows.Count + " / " + view.Rows.Count;
        var map = view.Rows.Cast<DataRow>().ToDictionary(r => r["DUT"] + "|" + r["NAME"]);
        for (int r = 0; r < upload.Rows.Count; r++)
        {
            DataRow u = upload.Rows[r], v;
            string key = u["DUT"] + "|" + u["NAME"];
            if (!map.TryGetValue(key, out v)) return "조회에 없음: " + key;
            if (view.Rows[r] != v) return (r + 1) + "행 순서 다름: " + key + " / " + view.Rows[r]["DUT"] + "|" + view.Rows[r]["NAME"];
            foreach (string column in new[] { "XY", "HEADER", "CHAIN", "STATUS", "JUDGE_FLAG", "JUDGE", "REASON" })
                if (u[column].ToString() != v[column].ToString()) return key + " " + column + ": " + u[column] + " / " + v[column];
            foreach (string column in new[] { "VALUE", "LOWER", "UPPER" })
                if (!SameNumber(u[column].ToString(), v[column].ToString())) return key + " " + column + ": " + u[column] + " / " + v[column];
        }
        return string.Empty;
    }

    private static bool SameNumber(string a, string b)
    {
        double x, y;
        if (a == b) return true;
        if (!double.TryParse(a, NumberStyles.Float, CultureInfo.InvariantCulture, out x) || !double.TryParse(b, NumberStyles.Float, CultureInfo.InvariantCulture, out y)) return false;
        return Math.Abs(x - y) <= 1e-12 * Math.Max(1, Math.Abs(x));
    }

    private static int Occurrences(string text, string part)
    {
        int n = 0;
        for (int i = text.IndexOf(part, StringComparison.Ordinal); i >= 0; i = text.IndexOf(part, i + part.Length, StringComparison.Ordinal)) n++;
        return n;
    }

    /// <summary>Slot 칸을 고치고 CellEndEdit 처리기를 부른다(처리기가 BeginInvoke 로 다시 그림).</summary>
    private static void SetSlot(Type type, object form, DataGridView grid, DataGridViewRow row, string value)
    {
        row.Cells["colQSlot"].Value = value;
        InvokeArgs(type, form, "dgvQueue_CellEndEdit", new object[] { grid, new DataGridViewCellEventArgs(grid.Columns["colQSlot"].Index, row.Index) });
        Application.DoEvents();
    }

    private static void Save(Form form, string path)
    {
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(path);
        }
    }

    private static string Cell(DataGridViewRow row, string column)
    {
        return Convert.ToString(row.Cells[column].Value, CultureInfo.InvariantCulture);
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

    private static void InvokeNoArgs(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(instance, null);
        Application.DoEvents();
    }

    private static void InvokeArgs(Type type, object instance, string method, object[] values)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, values);
        Application.DoEvents();
    }

    /// <summary>업로드 화면: 공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창 / 파일 고르기만 바꾼다. 분석은 백그라운드를 끈다.</summary>
    private sealed class UploadFixture : DACrux.SEMDMS.ENGUI.frmEtResultUpload
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public string[] Files = new string[0];

        protected override bool AnalyzeInBackground
        {
            get { return false; }
        }

        protected override string[] ChooseFiles()
        {
            return Files;
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

    /// <summary>조회 화면: 공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창만 바꾼다(Device 목록은 Smoke 가 직접 부른다).</summary>
    private sealed class ViewFixture : DACrux.SEMDMS.ENGUI.frmEtResultView
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
