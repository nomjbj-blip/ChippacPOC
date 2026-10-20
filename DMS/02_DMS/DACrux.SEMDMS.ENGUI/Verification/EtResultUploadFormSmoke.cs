using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// E-Test 결과 업로드 화면(frmEtResultUpload)과 결과 조회 화면(frmEtResultView)을 화면 밖에서 실제로 띄워 검사한다.
/// (2026-10-07 작성, 2026-10-14 Upload 재설계로 다시 작성, 2026-10-15 Upload / View 분리 + 업로드 파일 하나씩으로 다시 작성)
/// 업로드: [파일 열기] 하나씩 -> 상태(읽기 실패 / 등록 없음 / Map 없음 / Slot 없음) -> 정보 표 더블클릭 수정(공정 / Device / Lot-Wafer No / Lot) + Slot 칸
///         -> 공통 결과 Control(정보 11행 / 1단계 Die / 2단계 정의 Para 값 거르기)
///         -> 저장 확인 문구(Parameter x Die = 값) -> 저장 -> 중복 아니요 / 예(덮어쓰기) -> 등록 변경 후 실패 / [다시 읽기].
/// 조회: Device / 공정 / Lot 조회 -> 파일 목록(추가 Para / Spec 바뀜 / Map) -> 같은 결과 Control 에 저장 결과(업로드 미리보기와 같은 값) -> 삭제.
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다. 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다.
/// 확인창(Ask) / 안내창(Notify) / 파일 고르기는 Fixture 가 정해진 값으로 바꾼다(창을 띄우지 않음).
/// 인자: [0] E-Test xlsx, [1] 업로드 화면 이미지(png, 같은 폴더에 view.png 도 만든다), [2] 작업 폴더(파일명을 바꾼 사본), [3] 고객 Spec 표(탭 구분 텍스트), [4] keep(선택)
/// 시험 데이터: FACTORY=PROBE / Device = 파일명 Device(OMM) / 공정 TSVET 등록 Parameter 37(고객 Spec 표) + Spec. 끝나면 지운다.
///   Device NOMAP(파일명 Device 를 바꾼 사본)에도 같은 Parameter 를 등록한다(Map 없음 오류 확인용).
/// 시험 Map: 업로드는 Map 이 필수(2026-10-15)라 Verify-EtResultUpload.ps1 이 PROBE / OMM 시험 Map(SCK OMM_Hybrid 사용 Die 복사)을 넣고 끝나면 지운다.
/// </summary>
internal static class EtResultUploadFormSmoke
{
    private const string Factory = "PROBE";
    private const string Device = "OMM";
    private const string Lot = "RH-HS2615";
    private const string NoMapDevice = "NOMAP";
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
        string noSlot = Copy(sample, work, name.Replace("RH-HS2615-02.xlsx", "RH-HS2615-XX.xlsx"));  // Slot 없음 -> 입력
        string slot3 = Copy(sample, work, name.Replace("RH-HS2615-02.xlsx", "RH-HS2615-03.xlsx"));   // 다른 Wafer
        string slot5 = Copy(sample, work, name.Replace("RH-HS2615-02.xlsx", "RH-HS2615-05.xlsx"));   // 분석 뒤 등록 변경
        string noMap = Copy(sample, work, name.Replace("_OMM_", "_" + NoMapDevice + "_"));          // 등록 Parameter 는 있고 Map 없음
        var spec = DACrux.SEMDMS.ENGUI.EtSpecTable.Parse(File.ReadAllText(args[3])).Where(r => r.TestProgram == "TSVET").ToList();
        string[,] reg = new string[spec.Count, 4];
        string[,] lim = new string[spec.Count, 7];
        for (int i = 0; i < spec.Count; i++)
        {
            reg[i, 0] = spec[i].HeaderText; reg[i, 1] = spec[i].Unit; reg[i, 2] = ""; reg[i, 3] = "";
            lim[i, 0] = spec[i].HeaderText; lim[i, 1] = spec[i].Lower; lim[i, 2] = spec[i].Upper; lim[i, 3] = "Y"; lim[i, 4] = spec[i].Unit; lim[i, 5] = spec[i].SpecText; lim[i, 6] = "";
        }
        bsl.RegisterEtParameters(new[] { Factory, Device, "TSVET", "SPEC", "" }, reg, "SMOKE");
        bsl.RegisterEtParameters(new[] { Factory, NoMapDevice, "TSVET", "SPEC", "" }, reg, "SMOKE");
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
            var txtSlot = (TextBox)Get(type, form, "txtSlot");
            var btnSave = (Button)Get(type, form, "btnSave");
            var lblState = (Label)Get(type, form, "lblState");
            var lblMessage = (Label)Get(type, form, "lblMessage");
            var result = (DACrux.SEMDMS.ENGUI.EtResultDataControl)Get(type, form, "resultView");
            var info = (DataGridView)Get(ctlType, result, "dgvInfo");
            var infoTitle = (Label)Get(ctlType, result, "lblInfoTitle");
            var dies = (DataGridView)Get(ctlType, result, "dgvDie");
            var values = (DataGridView)Get(ctlType, result, "dgvValue");
            var chkFail = (CheckBox)Get(ctlType, result, "chkFailOnly");
            var chkDash = (CheckBox)Get(ctlType, result, "chkNotTested");
            var txtFind = (TextBox)Get(ctlType, result, "txtFind");
            var valueTitle = (Label)Get(ctlType, result, "lblValueTitle");
            var count = (Label)Get(ctlType, result, "lblCount");
            txtFactory.Text = Factory;
            Check("처음: 안내 문구 / [저장] · [다시 읽기] 꺼짐 / Slot 잠김", lblMessage.Text.StartsWith("[파일 열기]") && !btnSave.Enabled
                && !((Button)Get(type, form, "btnReload")).Enabled && txtSlot.ReadOnly, lblMessage.Text);

            // ---- 1) 파일 하나씩 열기: 오류 4종 -> 정보 표 더블클릭 수정(공정 / Device / Lot-Wafer No / Lot) + Slot 칸 -> 저장 ----
            Open(type, form, broken);
            bool brokenOk = lblState.Text == "오류" && lblMessage.Text.StartsWith("파일 읽기 실패") && dies.Rows.Count == 0 && !btnSave.Enabled;
            Open(type, form, cowet);
            bool cowetOk = lblState.Text == "오류" && lblMessage.Text.StartsWith("등록 Parameter 없음") && !btnSave.Enabled;
            List<string> keys = InfoKeys(info);
            Check("정보 표: 파일 기준 11행(저장 Parameter 아래 없음) / Device · 공정 · Lot · Lot-Wafer No 만 고칠 수 있음",
                string.Join("|", keys.ToArray()) == "파일명|Device|공정|Lot|Lot-Wafer No|Slot / WAFER_ID|Test Date|Customer / Wafer Type / Rev|Die|헤더|등록 (사용 Y)"
                && Editable(info, "Device") && Editable(info, "공정") && Editable(info, "Lot") && Editable(info, "Lot-Wafer No")
                && !Editable(info, "파일명") && !Editable(info, "Slot / WAFER_ID") && !Editable(info, "Test Date"), string.Join(",", keys.ToArray()));
            EditInfo(ctlType, result, info, "공정", "tsvet");
            Check("공정 COWET -> 'tsvet' 수정: 다시 분석 -> 공정 TSVET / 등록 37 / 준비", cowetOk && lblState.Text == "준비" && InfoValue(info, "공정") == "TSVET"
                && InfoValue(info, "등록 (사용 Y)") == "37" && btnSave.Enabled, lblState.Text + " / " + lblMessage.Text);
            Open(type, form, noMap);
            bool noMapOk = lblState.Text == "오류" && lblMessage.Text.StartsWith("Map 없음: File Map Setup 에 Map ID " + NoMapDevice) && !btnSave.Enabled;
            EditInfo(ctlType, result, info, "Device", Device);
            Check("Map 없음 오류(Device NOMAP) -> Device 를 OMM 으로 수정: Map 좌표 DUT #20 = 9,2 / 준비", noMapOk && lblState.Text == "준비" && InfoValue(info, "Device") == Device
                && Cell(DieRow(dies, "#20"), "colDMap") == "9,2", noMapOk + " / " + lblState.Text + " / " + lblMessage.Text);
            Open(type, form, noSlot);
            bool noSlotOk = lblState.Text == "오류" && lblMessage.Text.StartsWith("Slot 없음: Lot-Wafer No RH-HS2615-XX") && txtSlot.Text.Length == 0 && !txtSlot.ReadOnly && !btnSave.Enabled;
            Check("파일 열기 오류: 7칸 읽기 실패(결과 비움) / COWET 등록 Parameter 없음 / Slot 없음 -> [저장] 꺼짐", brokenOk && cowetOk && noSlotOk,
                brokenOk + " / " + cowetOk + " / " + noSlotOk + " | " + lblMessage.Text);
            EditInfo(ctlType, result, info, "Lot-Wafer No", "RH-HS2615-07");
            bool waferOk = lblState.Text == "준비" && txtSlot.Text == "7" && InfoValue(info, "Slot / WAFER_ID") == "7 / RH-HS2615-07";
            txtSlot.Text = "4";
            Invoke(type, form, "txtSlot_Leave");
            bool slotOk = lblState.Text == "준비" && InfoValue(info, "Slot / WAFER_ID") == "4 / RH-HS2615-04" && InfoValue(info, "Lot-Wafer No") == "RH-HS2615-07";
            EditInfo(ctlType, result, info, "Lot", "RH-HSZZ");
            bool lotOk = InfoValue(info, "Slot / WAFER_ID") == "4 / RH-HSZZ-04";
            EditInfo(ctlType, result, info, "Lot", Lot);
            Check("Lot-Wafer No 수정 -> Slot 7 / Slot 칸 4 -> WAFER_ID RH-HS2615-04 / Lot 수정 -> RH-HSZZ-04 (Slot 유지) / Lot 되돌림 / [저장] 켜짐",
                waferOk && slotOk && lotOk && InfoValue(info, "Slot / WAFER_ID") == "4 / RH-HS2615-04" && btnSave.Enabled,
                waferOk + " / " + slotOk + " / " + lotOk + " / " + InfoValue(info, "Slot / WAFER_ID"));
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSave_Click");
            string q = form.LastQuestion;
            string seq4 = SeqOf(lblMessage.Text);
            Check("저장 확인 문구: 파일 / WAFER_ID RH-HS2615-04 · 공정 TSVET / 'Parameter 37 x Die 24 = 값 888' / 등록 안 된 헤더 2,718개 / 다시 업로드 안내",
                q.StartsWith("E-Test 결과 파일을 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.") && q.Contains("파일: " + Path.GetFileName(noSlot))
                && q.Contains("WAFER_ID RH-HS2615-04 / 공정 TSVET") && q.Contains("저장: Parameter 37 x Die 24 = 값 888") && q.Contains("등록되지 않은 헤더는 저장하지 않습니다 (2,718개)")
                && q.Contains("다시 업로드해야 보입니다"), q.Replace("\r\n", " / "));
            Check("저장 뒤: 상태 저장됨 / 메시지 파일 번호 / 제목 '업로드 결과 - 저장됨 (파일 번호)' / [저장] 꺼짐 / Slot · 정보 표 잠김 / 안내에 조회 화면",
                lblState.Text == "저장됨" && lblMessage.Text.StartsWith("저장 완료 / 파일 번호 " + seq4) && infoTitle.Text == "업로드 결과 - 저장됨 (파일 번호 " + seq4 + ")"
                && !btnSave.Enabled && txtSlot.ReadOnly && !Editable(info, "Device") && form.LastNotice.Contains("E-Test 결과 조회 화면") && bsl.GetEtFile(seq4).Rows.Count == 1, lblMessage.Text);
            DataRow s4 = bsl.GetEtFile(seq4).Rows[0];
            Check("DB: 고친 Lot-Wafer No RH-HS2615-07 / Slot 4 / WAFER_ID RH-HS2615-04 / 파일명은 원래 이름(XX) / Map OMM",
                s4["LOT_WAFER_NO"].ToString() == "RH-HS2615-07" && s4["SLOT_NO"].ToString() == "4" && s4["WAFER_ID"].ToString() == "RH-HS2615-04"
                && s4["FILE_NAME"].ToString() == Path.GetFileName(noSlot) && s4["MAP_ID"].ToString() == Device,
                s4["LOT_WAFER_NO"] + " / " + s4["SLOT_NO"] + " / " + s4["WAFER_ID"] + " / " + s4["MAP_ID"]);

            // ---- 2) 샘플 열기 -> 공통 결과 Control (업로드 미리보기): 정보 / 1단계 Die / 2단계 정의 Para 값 ----
            Open(type, form, sample);
            Check("샘플: 준비(저장할 수 있습니다) / 파일 경로 / Slot 2 / 정보 OMM · TSVET · 2 / RH-HS2615-02 · 헤더 2,755 · 등록 37",
                lblState.Text == "준비" && lblMessage.Text == "저장할 수 있습니다." && ((TextBox)Get(type, form, "txtFile")).Text == sample && txtSlot.Text == "2"
                && InfoValue(info, "Device") == "OMM" && InfoValue(info, "공정") == "TSVET" && InfoValue(info, "Slot / WAFER_ID") == "2 / RH-HS2615-02"
                && InfoValue(info, "헤더") == "2,755" && InfoValue(info, "등록 (사용 Y)") == "37",
                lblState.Text + " / " + lblMessage.Text);

            // ---- 2-1) 단위 다름 (2026-10-16): 파일 Unit 이 등록 단위와 다르면 그 Parameter 는 저장하지 않고 경고. 끝나면 샘플을 다시 연다 ----
            object cur = Get(type, form, "current");
            Type curType = cur.GetType();
            var curFile = (DACrux.SEMDMS.ENGUI.EtResultFile)curType.GetField("File").GetValue(cur);
            var curPlan = (DACrux.SEMDMS.ENGUI.EtUploadPlan)curType.GetField("Plan").GetValue(cur);
            int unitIndex = curPlan.Matched[0].Index;
            string unit0 = curFile.Items[unitIndex, 5];
            curFile.Items[unitIndex, 5] = "n" + unit0;
            var unitPlan = DACrux.SEMDMS.ENGUI.EtUploadPlan.Build(curFile, bsl.GetEtParameterList(Factory, Device, "TSVET"));
            curType.GetField("Plan").SetValue(cur, unitPlan);
            InvokeArgs(type, form, "ComputeStatus", new object[] { cur });
            string unitState = (string)curType.GetField("Status").GetValue(cur);
            string unitMessage = (string)curType.GetField("Message").GetValue(cur);
            curFile.Items[unitIndex, 5] = unit0;
            Check("단위 다름: 파일 n" + unit0 + " / 등록 " + unit0 + " -> 그 Parameter 저장 안 함(36) / 누락 아님 / 상태 경고 '단위 다름 1'",
                unitPlan.Matched.Count == 36 && unitPlan.UnitMismatches.Count == 1 && unitPlan.Missing.Count == 0
                && unitPlan.UnitMismatches[0].EndsWith(": 파일 n" + unit0 + " / 등록 " + unit0) && unitState == "경고" && unitMessage.Contains("단위 다름 1 건너뜀"),
                unitState + " / " + unitMessage);
            Open(type, form, sample);
            var d44 = DieRow(dies, "#44");
            var d20 = DieRow(dies, "#20");
            Check("미리보기: 제목 / 정보 첫 행 파일명 / Die 24 / #44 Fail 6 (빨간 행) / #20 Pass (DUT Flag Fail 은 참고)",
                infoTitle.Text == "업로드 미리보기 (저장 전) - 준비" && Cell(info.Rows[0], "colInfoName") == "파일명"
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
            int dashDie = dies.Rows.Cast<DataGridViewRow>().Sum(r => int.Parse(Cell(r, "colDNotTested"), CultureInfo.InvariantCulture));
            int dashRows = values.Rows.Count;
            bool dashAll = values.Rows.Cast<DataGridViewRow>().All(r => ((DataRowView)r.DataBoundItem)["VALUE"].ToString() == "-");
            Check("['-' 만]: 행 수 = 1단계 '-' 수 합 / 값은 모두 '-'", dashRows == dashDie && dashAll, dashRows + " / 1단계 합 " + dashDie);
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

            // ---- 3) 샘플 저장 ----
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSave_Click");
            string seqA = SeqOf(lblMessage.Text);
            Check("샘플 저장: 확인 문구 WAFER_ID RH-HS2615-02 / 저장됨 / 메시지 Spec Fail Die 1 · 값 888 · Map OMM 좌표 Die 24",
                form.LastQuestion.Contains("WAFER_ID RH-HS2615-02 / 공정 TSVET") && lblState.Text == "저장됨" && lblMessage.Text.Contains("Spec Fail Die 1")
                && lblMessage.Text.Contains("값 888") && lblMessage.Text.Contains("Map OMM 좌표 Die 24"), lblMessage.Text);
            var busyBar = (DACrux.SEMDMS.Control.ProgressStatusBar)Get(type, form, "busyBar");
            Check("진행 표시줄: '저장 완료'(처리 시간 없음) / 작업 없음", busyBar.LastResult == "저장 완료" && !busyBar.IsBusy, busyBar.DisplayText);
            DataRow saved = bsl.GetEtFile(seqA).Rows[0];
            Check("DB: 저장 37 / 등록 37 / 헤더 2,755 / Pass 23 · Fail 1 / 값 888 / 규칙 3",
                Convert.ToInt32(saved["ITEM_COUNT"]) == 37 && Convert.ToInt32(saved["DEFINED_COUNT"]) == 37 && Convert.ToInt32(saved["HEADER_COUNT"]) == 2755
                && Convert.ToInt32(saved["PASS_DIE_COUNT"]) == 23 && Convert.ToInt32(saved["FAIL_DIE_COUNT"]) == 1 && Convert.ToInt32(saved["VALUE_COUNT"]) == 888
                && Convert.ToInt32(saved["PARSE_RULE_VER"]) == 3, saved["WAFER_ID"] + " / 값 " + saved["VALUE_COUNT"]);

            // ---- 4) 조회 화면: 목록 / 같은 결과 Control ----
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
            Check("목록 열: Spec Fail Die 1 / 저장 Para 37 / 값 888 / 추가 Para 0 / Spec 바뀜 N / Map OMM · 좌표 Die 24 / 열 제목",
                Convert.ToString(fa.Cells["FAIL_DIE_COUNT"].Value) == "1" && Convert.ToString(fa.Cells["ITEM_COUNT"].Value) == "37" && Convert.ToString(fa.Cells["VALUE_COUNT"].Value) == "888"
                && Convert.ToString(fa.Cells["ADDED_PARA_COUNT"].Value) == "0" && Convert.ToString(fa.Cells["SPEC_CHANGED"].Value) == "N"
                && fa.Cells["MAP_ID"].FormattedValue.ToString() == Device && Convert.ToString(fa.Cells["MAP_DIE_COUNT"].Value) == "24"
                && files.Columns["FAIL_DIE_COUNT"].HeaderText == "Spec Fail Die"
                && files.Columns["ADDED_PARA_COUNT"].HeaderText == "추가 Para" && files.Columns["SPEC_CHANGED"].HeaderText == "Spec 바뀜" && !files.Columns["DEVICE_ID"].Visible,
                fa.Cells["ADDED_PARA_COUNT"].Value + " / " + fa.Cells["SPEC_CHANGED"].Value + " / " + fa.Cells["MAP_ID"].FormattedValue);
            SelectFile(files, fa);
            Check("파일 고르기: 제목 저장된 결과 / 파일 번호 / 진행 표시줄 결과 조회 완료",
                ((Label)Get(ctlType, vResult, "lblInfoTitle")).Text == "저장된 결과 - 파일 번호 " + seqA
                && vBar.LastResult == "결과 조회 (파일 번호 " + seqA + ") 완료", ((Label)Get(ctlType, vResult, "lblInfoTitle")).Text + " / " + vBar.LastResult);
            List<string> viewInfoKeys = InfoKeys(vInfo);
            Check("정보 항목 순서: 업로드 미리보기와 같음(안내 / 충돌 행 제외)", string.Join("|", uploadInfoKeys.Where(k => k != "안내" && k != "충돌").ToArray())
                == string.Join("|", viewInfoKeys.Where(k => k != "안내" && k != "충돌").ToArray()), string.Join(",", viewInfoKeys.ToArray()));
            Check("정보 값: Die 24 / 등록 37 (저장 당시) / 조회 화면은 고칠 수 있는 칸 없음 / 1단계 #20 Map 9,2",
                InfoValue(vInfo, "Die") == "24" && InfoValue(vInfo, "등록 (사용 Y)") == "37 (저장 당시)" && !Editable(vInfo, "Device") && !Editable(vInfo, "Lot")
                && Cell(DieRow(vDies, "#20"), "colDMap") == "9,2", InfoValue(vInfo, "등록 (사용 Y)") + " / " + Cell(DieRow(vDies, "#20"), "colDMap"));
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
            Check("조회 화면에 [Map 좌표 다시 계산] 없음 (2026-10-15 삭제)", viewType.GetField("btnRemap", BindingFlags.Instance | BindingFlags.NonPublic) == null
                && viewType.GetMethod("btnRemap_Click", BindingFlags.Instance | BindingFlags.NonPublic) == null, string.Empty);

            // ---- 5) 중복: 아니요 = 저장 안 함 / 예 = 덮어쓰기, 다른 Wafer 저장 ----
            Open(type, form, sample);
            Check("샘플 다시 열기: 중복(DB 파일 번호) / [저장] 켜짐", lblState.Text == "중복" && lblMessage.Text.Contains("파일 번호 " + seqA) && btnSave.Enabled, lblMessage.Text);
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSave_Click");
            Check("중복 [아니요]: 확인 문구에 덮어쓰기 안내 / 저장 안 함 / 기존 파일 그대로", form.LastQuestion.Contains("예 = 기존 파일을 지우고 다시 저장(덮어쓰기)")
                && lblState.Text == "중복" && bsl.GetEtFile(seqA).Rows.Count == 1, lblState.Text);
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            string seqA2 = SeqOf(lblMessage.Text);
            Check("중복 [예]: 덮어쓰기 -> 새 파일 번호 / 기존 파일 삭제", lblState.Text == "저장됨" && seqA2 != seqA && bsl.GetEtFile(seqA).Rows.Count == 0
                && bsl.GetEtFile(seqA2).Rows.Count == 1, seqA + " -> " + seqA2);
            Open(type, form, slot3);
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSave_Click");
            string seq3 = SeqOf(lblMessage.Text);
            Check("Slot 3 파일 저장", lblState.Text == "저장됨" && bsl.GetEtFile(seq3).Rows.Count == 1, lblMessage.Text);

            // ---- 6) 분석 뒤 등록 변경 -> 저장 실패 / [다시 읽기] 하면 누락 경고 ----
            Open(type, form, slot5);
            Thread.Sleep(1100);   // 등록 시각이 앞서 저장한 파일 등록 시각보다 뒤가 되게(DATE 는 초 단위)
            bsl.RegisterEtParameters(new[] { Factory, Device, "TSVET", "SPEC", "" }, new[,] { { "Resistance_iforce=8.0mA@ZZ_NOT_IN_FILE_FH", "Ohm", "", "" } }, "SMOKE");
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSave_Click");
            Check("분석 뒤 등록 Parameter 추가 -> 저장 실패(다시 분석 안내) / 파일 안 남음 / [저장] 꺼짐", lblState.Text == "실패" && lblMessage.Text.Contains("다시 분석")
                && !btnSave.Enabled && bsl.GetEtFileByKey(new[] { Factory, Lot, "5", "TSVET" }).Rows.Count == 0, lblMessage.Text);
            Invoke(type, form, "btnReload_Click");
            Check("[다시 읽기]: 경고 / 등록 38 / 메시지 누락 1", lblState.Text == "경고" && InfoValue(info, "등록 (사용 Y)") == "38" && lblMessage.Text.Contains("누락 1"), lblMessage.Text);
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnSave_Click");
            Check("경고 파일 저장 확인 문구: '경고: 누락 1 ...'", form.LastQuestion.Contains("경고: 누락 1"), form.LastQuestion.Replace("\r\n", " / "));
            DataRow f5 = bsl.GetEtFileByKey(new[] { Factory, Lot, "5", "TSVET" }).Rows[0];
            string seq5 = f5["SRC_FILE_SEQ"].ToString();
            DataRow f5Detail = bsl.GetEtFile(seq5).Rows[0];
            Check("경고(누락) 파일 저장: 등록 38 / 저장 37 / 누락 1 기록", lblState.Text == "저장됨" && Convert.ToInt32(f5Detail["DEFINED_COUNT"]) == 38
                && Convert.ToInt32(f5Detail["ITEM_COUNT"]) == 37 && Convert.ToInt32(f5Detail["MISSING_COUNT"]) == 1, lblMessage.Text);

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
            Check("Spec 수정 뒤: 목록 Spec 바뀜 Y (4건) / 2단계 Upper 는 지금 Spec",
                Convert.ToString(fOld.Cells["SPEC_CHANGED"].Value) == "Y" && files.Rows.Cast<DataGridViewRow>().All(r => Convert.ToString(r.Cells["SPEC_CHANGED"].Value) == "Y")
                && ((DataView)vValues.DataSource).Table.Rows.Cast<DataRow>().Any(r => r["HEADER"].ToString() == spec[specIndex].HeaderText && r["UPPER"].ToString() == newUpper),
                Convert.ToString(fOld.Cells["SPEC_CHANGED"].Value));
            int before = files.Rows.Count;
            view.Answers.Enqueue(DialogResult.Yes);
            InvokeArgs(viewType, view, "btnDelete_Click", new object[] { null, EventArgs.Empty });
            Check("[선택 파일 삭제]: 확인 문구에 YMS 데이터 / 목록 1건 줄어듦 / 삭제한 파일 없음", view.LastQuestion.Contains("YMS") && files.Rows.Count == before - 1
                && vStatus.Text.StartsWith("Device " + Device) && bsl.GetEtFile(seq3).Rows.Count == 0, before + " -> " + files.Rows.Count);

            // 화면 이미지: 조회 화면(첫 파일, #44 선택) / 업로드 화면(샘플 다시 열기 = 중복)
            view.Size = new Size(1300, 800);
            Application.DoEvents();
            SelectFile(files, files.Rows[0]);
            SelectDies(vDies, DieRow(vDies, "#44"));
            Save(view, Path.Combine(Path.GetDirectoryName(Path.GetFullPath(args[1])), "view.png"));
            view.Close();
            Open(type, form, sample);
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

    /// <summary>시험 파일(Lot RH-HS2615 Slot 2~5, 모든 공정)과 PROBE / OMM · NOMAP 등록 Parameter(Spec 같이)를 지운다.</summary>
    private static void Cleanup(DACrux.SEMDMS.BSL.GdsEtResult bsl)
    {
        foreach (string slot in new[] { "2", "3", "4", "5" })
            foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, Lot, slot }).Rows)
                bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
        foreach (DataRow dr in bsl.GetEtParaProgramList(Factory).Rows)
        {
            string device = dr["DEVICE_ID"].ToString();
            if (device != Device && device != NoMapDevice) continue;
            string program = dr["TEST_PROGRAM"].ToString();
            string[] names = bsl.GetEtParameterList(Factory, device, program).Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()).ToArray();
            if (names.Length > 0) bsl.DeleteEtParameters(new[] { Factory, device, program }, names, "SMOKE");
        }
    }

    private static string Copy(string source, string folder, string name)
    {
        string path = Path.Combine(folder, name);
        File.Copy(source, path, true);
        return path;
    }

    /// <summary>[파일 열기] 를 눌러 파일 하나를 연다(파일 고르기는 Fixture 가 이 경로를 돌려준다).</summary>
    private static void Open(Type type, UploadFixture form, string path)
    {
        form.NextFile = path;
        Invoke(type, form, "btnOpen_Click");
    }

    /// <summary>정보 표 항목의 값 칸을 더블클릭해 고치고 편집을 끝낸다(편집 끝 알림은 BeginInvoke 라 DoEvents 로 처리된다).</summary>
    private static void EditInfo(Type ctlType, object control, DataGridView info, string key, string value)
    {
        DataGridViewRow row = info.Rows.Cast<DataGridViewRow>().First(r => Cell(r, "colInfoName") == key);
        int column = info.Columns["colInfoValue"].Index;
        InvokeArgs(ctlType, control, "dgvInfo_CellDoubleClick", new object[] { info, new DataGridViewCellEventArgs(column, row.Index) });
        if (!info.IsCurrentCellInEditMode) throw new InvalidOperationException("정보 표 편집이 시작되지 않음: " + key);
        info.EditingControl.Text = value;
        info.EndEdit();
        Application.DoEvents();
        Application.DoEvents();
    }

    /// <summary>정보 표 항목의 값 칸을 고칠 수 있는지</summary>
    private static bool Editable(DataGridView info, string key)
    {
        DataGridViewRow row = info.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Cell(r, "colInfoName") == key);
        return row != null && !row.Cells["colInfoValue"].ReadOnly;
    }

    /// <summary>저장 메시지("저장 완료 / 파일 번호 n / ...")의 파일 번호</summary>
    private static string SeqOf(string message)
    {
        Match m = Regex.Match(message, @"파일 번호 (\d+)");
        return m.Success ? m.Groups[1].Value : string.Empty;
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

    /// <summary>업로드 화면: 공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창 / 파일 고르기만 바꾼다.</summary>
    private sealed class UploadFixture : DACrux.SEMDMS.ENGUI.frmEtResultUpload
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public string NextFile = "";

        protected override string ChooseFile()
        {
            return NextFile;
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
