using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

/// <summary>
/// E-Test Para 등록 화면(frmEtParaRegister)과 E-Test Para 설정 화면(frmEtParaSetup)을 화면 밖에서 띄워 다음 흐름을 검사한다. (2026-10-14)
///  A) 등록: 파일 열기(Factory 만 입력, Device / 공정 / 파일 읽기 전용, 행 머리글 = 열) -> 헤더 행 목록에서 빼기 / 되살리기 -> 직접 체크 + [등록] -> 거르기
///  B) 설정: 등록된 Device / 공정 콤보 고르기 = 바로 조회 -> [Spec 표 붙여넣기](없는 Parameter 자동 등록) -> 샘플 업로드 -> GDS Chain 직접 입력
///     (더블클릭 / Ctrl+V / 오른쪽 목록 복사) / 사용 · 비고 · Lower · Upper · Enable 수정 표시 -> 수정 행만 저장 -> 다시 판정(#44 Fail 6 -> 4) ->
///     Upper 지우기(Spec 삭제) / 원래 값으로 되돌리기 / 거르기 / 필터 콤보(2026-10-15) / 공정 콤보(수정 버림 질문) / 삭제, 화면 아래 진행 표시줄(ProgressStatusBar)
///     (2026-10-15 화면에서 정보 줄 / 우클릭 메뉴(Spec 비우기 / 수정 취소)가 빠져, 수정 행 수 / 주황 칸 수는 표에서 직접 센다)
///  C) 등록 화면에서 파일을 다시 열면 설정 화면에서 넣은 GDS Chain / Spec 이 보임
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다(화면 -> RO -> BSL -> 로컬 DB).
/// 시험 데이터: FACTORY=PROBE / E-Test Device = 파일명 Device(OMM) / 공정 TSVET (Spec 표로 COWET / FT 도 등록) / 업로드 Slot 4 (RH-HS2615-04),
///   GDS Device = E-Test Device 와 같은 이름의 시험 Map (PROBE 시험 Map LAYER_FILTER_PROBE 를 OMM 으로 복사, 시험 GDS Chain Code ZZETU*, 2026-10-15). keep 이 없으면 끝에 모두 지운다.
/// 인자: [0] E-Test xlsx, [1] 결과 이미지(png, 설정 화면), [2] 고객 Spec 표(탭 구분 텍스트), [3] keep(선택)
/// 기대값(샘플 + 고객 표): 표 119행(TSVET 37 / COWET 40 / FT 42), TSVET 37 은 파일에서 먼저 등록, 업로드 값 888, Fail 6 (#44 의 RES 6항목 718~719 Ohm > 715)
/// </summary>
internal static class EtParaSetupFormSmoke
{
    private const string Factory = "PROBE";
    private const string GdsSource = "LAYER_FILTER_PROBE";
    /// <summary>GDS Device = E-Test Device 와 같은 이름의 Map. 준비 단계에서 GdsSource 를 복사해 만든다 (2026-10-15).</summary>
    private static string GdsDevice { get { return etDevice; } }
    private const string Slot = "4";
    private static readonly string ResTR = N("Resistance_iforce=8.0mA@TSVDC_TR_FH");
    private static readonly string ResAsic1P = N("Resistance_iforce=8.0mA@TSVDC_ASIC1_P_FH");
    private static readonly string ResRN = N("Resistance_iforce=8.0mA@TSVDC_R_N_FH");
    private static readonly string ResLP = N("Resistance_iforce=8.0mA@TSVDC_L_P_FH");
    private static readonly string ResBot1P = N("Resistance_iforce=8.0mA@TSVDC_BOT1_P_FH");
    private static int failures;
    private static string etDevice = "OMM";
    private const int WM_CHAR = 0x0102;

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        bool keep = args.Length > 3 && args[3] == "keep";
        string specText = File.ReadAllText(args[2]);
        string imageDir = Path.GetDirectoryName(Path.GetFullPath(args[1]));
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();
        var chainBsl = new DACrux.SEMDMS.BSL.GdsChain();
        var import = new DACrux.SEMDMS.BSL.GdsMapImport();
        etDevice = DACrux.SEMDMS.ENGUI.EtResultFileParser.Parse(args[0]).FileDevice;
        string seq = null;

        // ---- 0) 준비: 잔여 정리 + E-Test Device 이름의 시험 GDS Map(시험 Map 복사) + 시험 GDS Chain 2개 ----
        Cleanup(bsl, chainBsl, import);
        import.CopyMap(Factory, GdsSource, GdsDevice, "SMOKE");
        string seqTrFl = NewGdsChain(chainBsl, "ZZETU1", "TSVDC_TR_FL");
        string seqAsicPFl = NewGdsChain(chainBsl, "ZZETU2", "TSVDC_ASIC1_P_FL");
        Console.WriteLine("준비: E-Test Device " + etDevice + " -> GDS " + GdsDevice + " / 시험 Chain " + seqTrFl + ", " + seqAsicPFl);
        var specNames = new HashSet<string>(DACrux.SEMDMS.ENGUI.EtSpecTable.Parse(specText).Where(r => r.TestProgram == "TSVET").Select(r => N(r.HeaderText)));

        // ================= A) E-Test Para 등록 화면 =================
        Type rtype = typeof(DACrux.SEMDMS.ENGUI.frmEtParaRegister);
        using (var form = new RegisterFixture())
        {
            form.SamplePath = args[0];
            ShowOffScreen(form);
            Set(rtype, form, "paraService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var txtFactory = (TextBox)Get(rtype, form, "txtFactory");
            var txtDevice = (TextBox)Get(rtype, form, "txtDevice");
            var txtProgram = (TextBox)Get(rtype, form, "txtTestProgram");
            var txtFileName = (TextBox)Get(rtype, form, "txtFileName");
            var mnuDelete = (ToolStripMenuItem)Get(rtype, form, "mnuHeaderDelete");
            // 2026-10-15 화면에서 정보 줄(lblFileInfo)이 빠져 파일 요약(fileInfoText) / 목록에서 뺀 행(removedItems)을 직접 읽는다
            Func<string> fileInfo = () => (string)Get(rtype, form, "fileInfoText");
            Func<int> removed = () => ((HashSet<int>)Get(rtype, form, "removedItems")).Count;
            var header = (DataGridView)Get(rtype, form, "dgvHeader");
            var chkNew = (CheckBox)Get(rtype, form, "chkNewOnly");
            var txtFind = (TextBox)Get(rtype, form, "txtFind");
            var status = (Label)Get(rtype, form, "lblStatus");
            txtFactory.Text = Factory;

            Check("등록 화면 구성: Factory(로그인 Factory, 2026-10-15) · Device · 공정 · 파일 읽기 전용 / 탭 · 편집 표 · 저장 버튼 없음",
                txtFactory.ReadOnly && txtDevice.ReadOnly && txtProgram.ReadOnly && txtFileName.ReadOnly && !HasField(rtype, "tabMain")
                && !HasField(rtype, "dgvPara") && !HasField(rtype, "btnSave") && !HasField(rtype, "btnDelete"), form.Text);
            Invoke(rtype, form, "btnRegister_Click");
            Check("파일 없이 [등록] -> 안내", form.LastNotice.Contains("등록할 신규 헤더를 체크"), form.LastNotice);

            // 파일 열기: 파일명 Device / 공정 / 파일 이름을 읽기 전용 칸에, 헤더 2,755 모두 신규, 행 머리글 = 열 순서
            Invoke(rtype, form, "btnOpenFile_Click");
            int newRows = header.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colHStatus") == "신규");
            Check("파일 열기: Device / 공정 / 파일 이름 / 헤더 2,755 신규 / 파일 요약", txtDevice.Text == etDevice && txtProgram.Text == "TSVET"
                && txtFileName.Text == Path.GetFileName(args[0]) && header.Rows.Count == 2755 && newRows == 2755
                && fileInfo().StartsWith("헤더 2755 / 신규 2755 / 등록됨 0 / 등록 불가 0", StringComparison.Ordinal),
                txtDevice.Text + " / " + txtProgram.Text + " / " + fileInfo());
            Check("행 머리글 '열' = 파일 측정 열 순서", Convert.ToString(header.TopLeftHeaderCell.Value) == "열"
                && Convert.ToString(header.Rows[0].HeaderCell.Value) == "1" && Convert.ToString(header.Rows[2754].HeaderCell.Value) == "2755",
                Convert.ToString(header.Rows[0].HeaderCell.Value) + " ~ " + Convert.ToString(header.Rows[2754].HeaderCell.Value));
            DataGridViewRow trRow = HeaderRow(header, ResTR);
            Check("헤더 행: 원문 / 측정 / 조건 / Chain / Leg / 단위 / GDS Chain · Spec 빈 칸(미등록)", Cell(trRow, "colHHeader") == "Resistance_iforce=8.0mA@TSVDC_TR_FH"
                && Cell(trRow, "colHMeas") == "RESISTANCE" && Cell(trRow, "colHCond") == "IFORCE=8.0MA" && Cell(trRow, "colHChain1") == "TSVDC_TR_FH" && Cell(trRow, "colHLeg1") == "FH"
                && Cell(trRow, "colHUnit") == "Ohm" && Cell(trRow, "colHGds") == "" && Cell(trRow, "colHSpec") == "", Cell(trRow, "colHName"));

            // 헤더 행 목록에서 빼기 + 직접 체크(Spec 표 TSVET 37 행)
            List<DataGridViewRow> plain = header.Rows.Cast<DataGridViewRow>().Where(r => !specNames.Contains(Cell(r, "colHName"))).Take(6).ToList();
            header.ClearSelection();
            foreach (DataGridViewRow r in plain.Take(5)) r.Selected = true;
            trRow.Selected = true;
            RightClick(rtype, form, "dgvHeader_CellMouseDown", header, plain[0].Index, "colHHeader");
            var opening = new System.ComponentModel.CancelEventArgs();
            rtype.GetMethod("ctxHeader_Opening", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { null, opening });
            string menuText = mnuDelete.Text;
            Invoke(rtype, form, "mnuHeaderDelete_Click");
            int listed = header.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            Check("여러 행 위 우클릭 -> 선택 6행 유지 / 메뉴 삭제 -> 2,749행 / 뺀 행 6", menuText == "선택 행 목록에서 삭제 (6)" && !opening.Cancel && listed == 2749 && !trRow.Visible
                && removed() == 6, menuText + " / " + listed);
            foreach (DataGridViewRow r in header.Rows)
                if (r.Visible && specNames.Contains(Cell(r, "colHName"))) r.Cells["colHCheck"].Value = true;
            int checkedVisible = CheckedHeader(rtype, form);
            RightClick(rtype, form, "dgvHeader_CellMouseDown", header, plain[5].Index, "colHHeader");
            int selectedAfterClick = header.SelectedRows.Count;
            rtype.GetMethod("dgvHeader_KeyDown", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { null, new KeyEventArgs(Keys.Delete) });
            Check("직접 체크 37 중 뺀 행 제외 36 / 선택 밖 행 우클릭 -> 그 행만 / Del 키 -> 7행 뺌 / 다른 행 체크 유지", checkedVisible == 36 && selectedAfterClick == 1
                && !plain[5].Visible && removed() == 7 && CheckedHeader(rtype, form) == 36, checkedVisible + " / 뺀 행 " + removed());
            Invoke(rtype, form, "mnuHeaderRestore_Click");
            bool trUnchecked = !Checked(trRow, "colHCheck");
            trRow.Cells["colHCheck"].Value = true;
            Check("되살리기 -> 2,755 / 뺐던 행은 체크가 풀려 있음 -> 다시 체크 37", header.Rows.Cast<DataGridViewRow>().All(r => r.Visible)
                && removed() == 0 && trUnchecked && CheckedHeader(rtype, form) == 37, "체크 " + CheckedHeader(rtype, form));

            // [등록]
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(rtype, form, "btnRegister_Click");
            int done = header.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colHStatus") == "등록됨");
            DataRow reg = bsl.GetEtParameterList(Factory, etDevice, "TSVET").Rows.Cast<DataRow>().FirstOrDefault(r => r["ITEM_NAME"].ToString() == ResTR);
            Check("등록: 질문 / 37 등록 / 헤더 상태 등록됨 37 / DB 출처 FILE + 파일 이름", form.LastQuestion.Contains("Parameter 37개를 등록") && done == 37
                && status.Text.StartsWith("등록 37개 / 이미 있어 건너뜀 0개", StringComparison.Ordinal) && reg != null && reg["REG_SOURCE"].ToString() == "FILE"
                && reg["REG_FILE_NAME"].ToString() == Path.GetFileName(args[0]), status.Text);
            chkNew.Checked = true;
            Application.DoEvents();
            int visibleNew = header.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            txtFind.Text = "tsvdc_tr_f";
            Application.DoEvents();
            int visibleFind = header.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            chkNew.Checked = false;
            txtFind.Text = "";
            Application.DoEvents();
            Check("헤더 거르기: 신규만 2,718 / 글자 찾기", visibleNew == 2718 && visibleFind > 0 && header.Rows.Cast<DataGridViewRow>().All(r => r.Visible), visibleNew + " / " + visibleFind);
            SaveImage(form, Path.Combine(imageDir, "para_register.png"));
            form.Close();
        }

        // ================= B) E-Test Para 설정 화면 =================
        Type type = typeof(DACrux.SEMDMS.ENGUI.frmEtParaSetup);
        using (var form = new SetupFixture())
        {
            ShowOffScreen(form);
            Set(type, form, "paraService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var txtFactory = (TextBox)Get(type, form, "txtFactory");
            var cboDevice = (ComboBox)Get(type, form, "cboDevice");
            var cboProgram = (ComboBox)Get(type, form, "cboTestProgram");
            var para = (DataGridView)Get(type, form, "dgvPara");
            var gds = (DataGridView)Get(type, form, "dgvGds");
            var txtGds = (TextBox)Get(type, form, "txtGdsDevice");
            var lblGdsInfo = (Label)Get(type, form, "lblGdsInfo");
            var txtParaFind = (TextBox)Get(type, form, "txtParaFind");
            var chkSpecOnly = (CheckBox)Get(type, form, "chkSpecOnly");
            var chkNoGds = (CheckBox)Get(type, form, "chkNoGdsOnly");
            var cboFltMeas = (ComboBox)Get(type, form, "cboFltMeas");
            var cboFltCond = (ComboBox)Get(type, form, "cboFltCond");
            var cboFltGds = (ComboBox)Get(type, form, "cboFltGds");
            var cboFltUse = (ComboBox)Get(type, form, "cboFltUse");
            var btnClearFilter = (Button)Get(type, form, "btnClearFilter");
            var status = (Label)Get(type, form, "lblStatus");
            var busyBar = (DACrux.SEMDMS.Control.ProgressStatusBar)Get(type, form, "busyBar");
            txtFactory.Text = Factory;

            Check("설정 화면 구성: 탭 · 파일 열기 · 헤더 표 없음 / Device · 공정 콤보는 목록에서만 고름 / 더블클릭 편집",
                !HasField(type, "tabMain") && !HasField(type, "btnOpenFile") && !HasField(type, "dgvHeader") && !HasField(type, "btnRegister")
                && cboDevice.DropDownStyle == ComboBoxStyle.DropDownList && cboProgram.DropDownStyle == ComboBoxStyle.DropDownList
                && para.EditMode == DataGridViewEditMode.EditProgrammatically, form.Text);
            Invoke(type, form, "btnSearch_Click");
            Check("Device 없이 조회 -> 고르라는 안내(등록 화면 안내 포함)", form.LastNotice.Contains("Factory / Device / 공정을 고르세요") && form.LastNotice.Contains("E-Test Para 등록"),
                form.LastNotice);
            CallRet(type, form, "LoadDeviceCombo");
            Check("Device 목록 = 등록된 Device (" + etDevice + " 있음)", cboDevice.Items.Contains(etDevice), Items(cboDevice));
            cboDevice.SelectedItem = etDevice;
            Application.DoEvents();
            Check("Device 고르기 -> 공정 TSVET 자동 / 바로 조회 37행 / GDS Device(같은 이름 Map) / GDS Setup Chain 2", cboProgram.Text == "TSVET" && para.Rows.Count == 37
                && txtGds.Text == GdsDevice && txtGds.ReadOnly && gds.Rows.Count == 2 && status.Text.StartsWith("조회 완료 / Device " + etDevice + " / 공정 TSVET / 등록 Parameter 37 / Spec 0", StringComparison.Ordinal),
                cboProgram.Text + " / " + para.Rows.Count + " / " + txtGds.Text + " / " + status.Text);
            DataGridViewRow pTr = ParaRow(para, ResTR);
            Check("등록 Parameter 행: 원문 / 출처 / 사용 / 등록 파일 / Spec · GDS 빈 칸", Cell(pTr, "colPHeader") == "Resistance_iforce=8.0mA@TSVDC_TR_FH" && Cell(pTr, "colPSource") == "결과 파일"
                && Checked(pTr, "colPUse") && Cell(pTr, "colPRegFile") == Path.GetFileName(args[0]) && Cell(pTr, "colPUpper") == "" && Cell(pTr, "colPGds") == "",
                Cell(pTr, "colPName"));

            // [Spec 표 붙여넣기]: 잘못된 표는 거부, 고객 표는 COWET / FT 82 새로 등록 + Spec 119
            form.Clipboard = "U9\tTSVET\tResistance_iforce=8.0mA@TSVDC_TR_FH\tResistance\tIf val >= 715, Fail; otherwise, Pass\tOhm";
            Invoke(type, form, "btnPasteSpec_Click");
            Check("해석 못 하는 Spec -> 저장 안 함", form.LastNotice.Contains("해석할 수 없는 행이 1개") && bsl.GetEtLimit(Factory, etDevice, "TSVET").Rows.Count == 0, form.LastNotice.Split('\r')[0]);
            form.Clipboard = specText;
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnPasteSpec_Click");
            Check("표 저장 질문 (공정별 건수 / 표 Device U9 대신 / 새로 등록 82 = COWET 40 / FT 42)", form.LastQuestion.Contains("COWET 40 / FT 42 / TSVET 37")
                && form.LastQuestion.Contains("표의 Device U9 대신") && form.LastQuestion.Contains("등록되지 않은 Parameter 82개"), form.LastQuestion.Replace("\r\n", " "));
            pTr = ParaRow(para, ResTR);
            Check("표 저장 완료 / 공정 TSVET 그대로 / 공정 목록 COWET · FT · TSVET / TR Upper 715 · Enable · Spec 원문", status.Text.StartsWith("Spec 표 저장 완료 / COWET 40 / FT 42 / TSVET 37 / 새로 등록한 Parameter 82")
                && cboProgram.Text == "TSVET" && Items(cboProgram) == "COWET,FT,TSVET" && para.Rows.Count == 37 && Cell(pTr, "colPUpper") == "715" && Cell(pTr, "colPLower") == ""
                && Checked(pTr, "colPEnable") && Cell(pTr, "colPSpecText") == "If val > 715, Fail; otherwise, Pass", status.Text);

            // 업로드 (등록 Parameter 37 만 저장 + 판정) -> 다시 조회: 값 통계 / Fail 6
            seq = Upload(bsl, args[0]);
            DataRow upFile = bsl.GetEtFile(seq).Rows[0];
            Check("업로드: 값 888 / Spec Fail Die 1 (#44)", Convert.ToInt32(upFile["VALUE_COUNT"]) == 888 && Convert.ToInt32(upFile["FAIL_DIE_COUNT"]) == 1, "파일 번호 " + seq);
            Invoke(type, form, "btnSearch_Click");
            DataGridViewRow pRn = ParaRow(para, ResRN);
            DataRow d44Before = bsl.GetEtDieList(seq).Rows.Cast<DataRow>().First(r => Convert.ToInt32(r["DUT_NO"]) == 44);
            Check("업로드 뒤 조회 37행 / 값 통계 열(파일 수 ~ Fail) 없음 / #44 Fail 6 (RES 718~719 > 715)", para.Rows.Count == 37
                && !para.Columns.Contains("colPFiles") && !para.Columns.Contains("colPMax") && !para.Columns.Contains("colPFail")
                && Convert.ToInt32(d44Before["FAIL_COUNT"]) == 6, para.Columns.Count + "열");

            // GDS Chain 직접 입력: 더블클릭 편집(앞뒤 공백 제거, 대소문자 무시 비교) / Ctrl+V / 오른쪽 목록 복사
            pTr = ParaRow(para, ResTR);
            EditCell(type, form, para, pTr, "colPGds", "tsvdc_tr_fl ");
            Check("더블클릭 -> GDS Chain 입력 / 공백 제거 / 노란색 / 행 머리글 '*' / 목록에 있음(주황 아님) / 사용 수 1",
                Cell(pTr, "colPGds") == "tsvdc_tr_fl" && pTr.Cells["colPGds"].Style.BackColor == Color.LightYellow && Convert.ToString(pTr.HeaderCell.Value) == "*"
                && pTr.Cells["colPGds"].Style.ForeColor != Color.DarkOrange && Cell(GdsRow(gds, "TSVDC_TR_FL"), "colGUse") == "1",
                Cell(pTr, "colPGds") + " / 사용 수 " + Cell(GdsRow(gds, "TSVDC_TR_FL"), "colGUse"));
            DataGridViewRow pAsic = ParaRow(para, ResAsic1P);
            para.CurrentCell = pAsic.Cells["colPGds"];
            form.Clipboard = "TSVDC_NOPE\r\n";
            Key(type, form, "dgvPara_KeyDown", Keys.Control | Keys.V);
            bool orange = pAsic.Cells["colPGds"].Style.ForeColor == Color.DarkOrange;
            int unknownAfterPaste = Orange(para);
            InvokeCell(type, form, "dgvGds_CellDoubleClick", gds, GdsRow(gds, "TSVDC_ASIC1_P_FL").Index, gds.Columns["colGName"].Index);
            string copied = form.Copied;
            form.Clipboard = copied;
            para.CurrentCell = pAsic.Cells["colPGds"];
            Key(type, form, "dgvPara_KeyDown", Keys.Control | Keys.V);
            gds.ClearSelection();
            GdsRow(gds, "TSVDC_TR_FL").Selected = true;
            Key(type, form, "dgvGds_KeyDown", Keys.Control | Keys.C);
            Check("Ctrl+V 로 목록에 없는 이름 -> 주황 / 목록 더블클릭 = 이름 복사 -> Ctrl+V -> 주황 아님 / 목록 Ctrl+C = 이름만 복사",
                orange && unknownAfterPaste == 1 && copied == "TSVDC_ASIC1_P_FL" && Cell(pAsic, "colPGds") == "TSVDC_ASIC1_P_FL"
                && pAsic.Cells["colPGds"].Style.ForeColor != Color.DarkOrange && form.Copied == "TSVDC_TR_FL" && GdsCount(para) == 2 && Orange(para) == 0,
                copied + " / " + Cell(pAsic, "colPGds") + " / " + form.Copied);

            // 사용 / 비고 / Spec 수정 표시 + 입력 확인
            para.CurrentCell = pTr.Cells["colPRemark"];
            Key(type, form, "dgvPara_KeyDown", Keys.F2);
            bool f2Edit = para.IsCurrentCellInEditMode;
            para.EditingControl.Text = "Smoke 비고";
            para.EndEdit();
            Application.DoEvents();
            InvokeCell(type, form, "dgvPara_CellContentClick", para, pTr.Index, para.Columns["colPUse"].Index);
            pRn = ParaRow(para, ResRN);
            InvokeCell(type, form, "dgvPara_CellContentClick", para, pRn.Index, para.Columns["colPEnable"].Index);
            DataGridViewRow pLp = ParaRow(para, ResLP);
            EditCell(type, form, para, pLp, "colPUpper", "720");
            Check("F2 편집 / 사용 클릭 -> 해제 + 회색 글자 / Enable 클릭 -> 해제 / Upper 720 노란색 / 수정 4행 (저장 전)", f2Edit && Cell(pTr, "colPRemark") == "Smoke 비고"
                && !Checked(pTr, "colPUse") && pTr.DefaultCellStyle.ForeColor == Color.Gray && !Checked(pRn, "colPEnable") && pLp.Cells["colPUpper"].Style.BackColor == Color.LightYellow
                && Dirty(type, form) == 4, "수정 " + Dirty(type, form) + "행");
            EditCell(type, form, para, pTr, "colPLower", "800");
            bool red = pTr.Cells["colPLower"].Style.BackColor == Color.MistyRose;
            Invoke(type, form, "btnSave_Click");
            Check("Lower > Upper -> 붉은색 / 저장 거부(아무것도 저장 안 함)", red && form.LastNotice.Contains(ResTR) && form.LastNotice.Contains("Lower 가 Upper")
                && SetupRow(bsl, ResTR)["GDS_CHAIN_NAME"].ToString() == "", form.LastNotice.Split('\r')[0]);
            EditCell(type, form, para, pTr, "colPLower", "");
            DataGridViewRow pBot = ParaRow(para, ResBot1P);
            EditCell(type, form, para, pBot, "colPUpper", "999");
            bool botDirty = pBot.HeaderCell.Value != null && Dirty(type, form) == 5;
            EditCell(type, form, para, pBot, "colPUpper", "715.0");
            Check("Upper 999 -> 수정 5행 / 원래 값(715.0 = 715, 숫자로 비교)으로 되돌리면 수정 표시 없음 / 수정 4행", botDirty && pBot.HeaderCell.Value == null
                && pBot.Cells["colPUpper"].Style.BackColor != Color.LightYellow && Dirty(type, form) == 4, Cell(pBot, "colPUpper"));

            // [저장]: 수정 4행만 -> 다시 판정(예) -> #44 Fail 6 -> 4
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            DataRow d44 = bsl.GetEtDieList(seq).Rows.Cast<DataRow>().First(r => Convert.ToInt32(r["DUT_NO"]) == 44);
            Check("저장: 수정 4행만 / Spec 저장 2 (값이 바뀐 R_N · L_P 만) / 다시 판정 질문(예) -> TSVET 1개 / #44 Fail 6 -> 4", status.Text.StartsWith("저장 완료 / 수정 4행 / Spec 저장 2 / Spec 삭제 0")
                && form.LastQuestion.Contains("업로드 파일 1개를 바뀐 Spec 으로 다시 판정") && status.Text.Contains("다시 판정: TSVET 1개")
                && Convert.ToInt32(d44["FAIL_COUNT"]) == 4 && d44["JUDGE_FLAG"].ToString() == "F", status.Text + " / #44 Fail " + d44["FAIL_COUNT"]);
            DataRow sTr = SetupRow(bsl, ResTR), sRn = SetupRow(bsl, ResRN), sLp = SetupRow(bsl, ResLP);
            Check("DB: TR 사용 N / 비고 / GDS 입력 그대로(tsvdc_tr_fl) / Spec 유지, ASIC1_P GDS, R_N Enable N, L_P Upper 720 (Spec 원문 유지)",
                sTr["USE_FLAG"].ToString() == "N" && sTr["REMARK"].ToString() == "Smoke 비고" && sTr["GDS_CHAIN_NAME"].ToString() == "tsvdc_tr_fl" && sTr["HAS_SPEC"].ToString() == "Y"
                && SetupRow(bsl, ResAsic1P)["GDS_CHAIN_NAME"].ToString() == "TSVDC_ASIC1_P_FL" && sRn["ENABLE_FLAG"].ToString() == "N"
                && Convert.ToDouble(sLp["UPPER_LIMIT"], CultureInfo.InvariantCulture) == 720 && sLp["SPEC_TEXT"].ToString() == "If val > 715, Fail; otherwise, Pass",
                sTr["GDS_CHAIN_NAME"] + " / " + sRn["ENABLE_FLAG"] + " / " + sLp["UPPER_LIMIT"]);
            pTr = ParaRow(para, ResTR);
            Check("다시 조회: 수정 표시 없음 / R_N Enable 해제 · L_P Upper 720 그대로", Dirty(type, form) == 0 && !Checked(ParaRow(para, ResRN), "colPEnable")
                && Cell(ParaRow(para, ResLP), "colPUpper") == "720" && pTr.HeaderCell.Value == null, status.Text);

            // Spec 있는 행의 Upper 지우기(Lower 없음) -> Enable 자동 해제 -> 저장 = Spec 삭제(질문) / 다시 판정 아니요, Spec 없는 행에 값 -> Enable 자동
            pBot = ParaRow(para, ResBot1P);
            EditCell(type, form, para, pBot, "colPUpper", "");
            bool cleared = Cell(pBot, "colPUpper") == "" && !Checked(pBot, "colPEnable") && pBot.Cells["colPUpper"].Style.BackColor == Color.LightYellow;
            form.Answers.Enqueue(DialogResult.Yes);
            form.Answers.Enqueue(DialogResult.No);
            form.Questions.Clear();
            Invoke(type, form, "btnSave_Click");
            pBot = ParaRow(para, ResBot1P);
            Check("Upper 지우기 -> 저장 -> Spec 삭제 질문(예) / 다시 판정(아니요) -> Spec 36 / 행은 37 그대로", cleared && form.Questions.Count == 2 && form.Questions[0].Contains("Spec 이 삭제됩니다")
                && status.Text.Contains("Spec 삭제 1") && !status.Text.Contains("다시 판정") && bsl.GetEtLimit(Factory, etDevice, "TSVET").Rows.Count == 36 && para.Rows.Count == 37
                && Cell(pBot, "colPUpper") == "", status.Text);
            EditCell(type, form, para, pBot, "colPUpper", "5");
            bool autoEnable = Checked(pBot, "colPEnable");
            EditCell(type, form, para, pBot, "colPUpper", "");
            Check("Spec 없는 행에 Upper -> Enable 자동 켜짐 / 다시 지우면 꺼지고 수정 표시 없음", autoEnable && !Checked(pBot, "colPEnable") && pBot.HeaderCell.Value == null, autoEnable.ToString());

            // 거르기 / GDS Device 바꾸기
            chkSpecOnly.Checked = true;
            Application.DoEvents();
            int specOnly = Visible(para);
            chkSpecOnly.Checked = false;
            chkNoGds.Checked = true;
            Application.DoEvents();
            int noGds = Visible(para);
            chkNoGds.Checked = false;
            txtParaFind.Text = "asic1_p";
            Application.DoEvents();
            var found = para.Rows.Cast<DataGridViewRow>().Where(r => r.Visible).ToList();
            txtParaFind.Text = "";
            Application.DoEvents();
            Check("거르기: Spec 있는 것만 36 / GDS Chain 없는 것만 35 / 찾기 'asic1_p'", specOnly == 36 && noGds == 35 && found.Count > 1 && found.Count < 37
                && found.All(r => new[] { "colPHeader", "colPName", "colPChain1", "colPChain2", "colPGds" }.Any(c => Cell(r, c).IndexOf("ASIC1_P", StringComparison.OrdinalIgnoreCase) >= 0)),
                specOnly + " / " + noGds + " / " + found.Count);

            // 필터 콤보(2026-10-15): 펼치면 (전체) + 열 값(건수, 다른 콤보 선택을 통과한 행 기준) / 고르면 그 값의 행만 / 여러 콤보 AND / 수정한 행은 보임 / [필터 해제]
            string condRes = Cell(ParaRow(para, ResTR), "colPCond");
            ComboBox[] filterCombos = { cboFltMeas, cboFltCond, (ComboBox)Get(type, form, "cboFltChain1"), cboFltGds, cboFltUse, (ComboBox)Get(type, form, "cboFltEnable") };
            InvokeWith(type, form, "cboFlt_DropDown", cboFltCond);
            InvokeWith(type, form, "cboFlt_DropDown", cboFltUse);
            Check("필터 콤보 6개(목록에서만 고름) 모두 (전체) / [필터 해제] 꺼짐 / 조건 목록 (전체) 37 · " + condRes + " 28 · SERIAL 9 / 사용 목록 N 1 · Y 36",
                filterCombos.All(c => c.DropDownStyle == ComboBoxStyle.DropDownList && c.Text.StartsWith("(전체) (", StringComparison.Ordinal)) && !btnClearFilter.Enabled
                && Items(cboFltCond) == "(전체) (37)," + condRes + " (28),SERIAL (9)" && Items(cboFltUse) == "(전체) (37),N (1),Y (36)",
                Items(cboFltCond) + " / " + Items(cboFltUse));
            SelectItem(cboFltCond, condRes);
            int condVisible = Visible(para);
            InvokeWith(type, form, "cboFlt_DropDown", cboFltGds);
            string gdsItems = Items(cboFltGds);
            SelectItem(cboFltGds, "(빈 값)");
            Check("조건 고름 -> 28 / GDS Chain 목록은 조건을 통과한 행만(이름 2 + (빈 값) 26, (빈 값)은 끝) / (빈 값) 고름 -> 26 / [필터 해제] 켜짐 / 상태 줄",
                condVisible == 28 && gdsItems == "(전체) (28),TSVDC_ASIC1_P_FL (1),tsvdc_tr_fl (1),(빈 값) (26)" && Visible(para) == 26 && btnClearFilter.Enabled
                && status.Text == "필터 조건=" + condRes + ", GDS Chain (Chain 1)=(빈 값) / 표시 26 / 전체 37",
                condVisible + " / " + gdsItems + " / " + status.Text);
            DataGridViewRow pShown = para.Rows.Cast<DataGridViewRow>().First(r => r.Visible);
            EditCell(type, form, para, pShown, "colPGds", "TSVDC_TR_FL");
            CallRet(type, form, "ApplyParaFilter");
            bool dirtyShown = pShown.Visible && Visible(para) == 26;
            InvokeWith(type, form, "cboFlt_DropDown", cboFltGds);
            string gdsWhileEditing = Items(cboFltGds);
            EditCell(type, form, para, pShown, "colPGds", "");
            Check("필터에 맞지 않게 고친 행(GDS 입력)도 수정 중이면 보임 / 펼치면 수정 중인 값으로 다시 셈 -> 되돌리면 수정 없음", dirtyShown && Dirty(type, form) == 0 && pShown.Visible
                && gdsWhileEditing == "(전체) (28),TSVDC_ASIC1_P_FL (1),TSVDC_TR_FL (1),tsvdc_tr_fl (1),(빈 값) (25)" && cboFltGds.Text == "(빈 값) (25)", gdsWhileEditing);
            InvokeWith(type, form, "cboFlt_DropDown", cboFltMeas);
            Check("측정 목록 = 다른 콤보(조건 / GDS Chain)를 통과한 행만", Items(cboFltMeas) == "(전체) (26),RESISTANCE (26)", Items(cboFltMeas));
            Invoke(type, form, "btnClearFilter_Click");
            Check("[필터 해제] -> 모두 (전체) / 37 모두 보임 / 버튼 꺼짐 / 상태 줄", Visible(para) == 37 && filterCombos.All(c => c.SelectedIndex == 0) && !btnClearFilter.Enabled
                && status.Text == "필터 해제 / 표시 37 / 전체 37", status.Text);
            InvokeWith(type, form, "cboFlt_DropDown", cboFltCond);
            SelectItem(cboFltCond, condRes);
            Invoke(type, form, "btnSearch_Click");
            Check("같은 Device / 공정 다시 조회 -> 고른 조건 유지 28 / 상태 줄에 필터 표시", cboFltCond.Text == condRes + " (28)" && Visible(para) == 28 && btnClearFilter.Enabled
                && status.Text.EndsWith("/ 필터 조건=" + condRes + " 표시 28", StringComparison.Ordinal), status.Text);

            // GDS Device 는 고르지 않고 위 E-Test Device 이름으로 조회(2026-10-15): 그 이름의 GDS(시험 Map / Chain)를 지우고 다시 조회하면 빈 목록 그대로
            bool listBefore = gds.Rows.Count == 2 && Cell(GdsRow(gds, "TSVDC_ASIC1_P_FL"), "colGUse") == "1" && Orange(para) == 0;
            foreach (DataRow dr in chainBsl.GetChainList(Factory, GdsDevice).Rows)
                chainBsl.DeleteChain(Factory, GdsDevice, dr["CHAIN_SEQ"].ToString(), "SMOKE");
            import.DeleteMap(Factory, GdsDevice, "SMOKE");
            Invoke(type, form, "btnSearch_Click");
            Check("GDS Device = 위 E-Test Device(읽기 전용) / 그 이름의 GDS 를 지우고 다시 조회 -> 목록 0 그대로 / 표 GDS Chain 주황 2 / 행 37 그대로",
                listBefore && txtGds.Text == etDevice && gds.Rows.Count == 0 && lblGdsInfo.Text == "GDS Setup Chain 0개" && Orange(para) == 2
                && ParaRow(para, ResTR).Cells["colPGds"].Style.ForeColor == Color.DarkOrange && para.Rows.Count == 37, lblGdsInfo.Text + " / 주황 " + Orange(para));

            // 값이 있는 Parameter 삭제 거부 / 저장 안 한 수정이 있으면 조회 전에 묻기
            para.ClearSelection();
            ParaRow(para, ResTR).Cells["colPHeader"].Selected = true;
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnDelete_Click");
            Check("측정값이 저장된 Parameter 삭제 -> 서버 거부 / 37 그대로", form.LastNotice.StartsWith("삭제 실패", StringComparison.Ordinal) && para.Rows.Count == 37, form.LastNotice.Split('\r')[0]);
            pLp = ParaRow(para, ResLP);
            EditCell(type, form, para, pLp, "colPRemark", "저장 안 함");
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSearch_Click");
            Check("수정 1행이 있는데 [조회] -> 버릴지 질문(아니요) -> 수정 유지", form.LastQuestion.Contains("저장하지 않은 수정 1행") && Cell(ParaRow(para, ResLP), "colPRemark") == "저장 안 함",
                form.LastQuestion);
            SaveImage(form, args[1]);

            // 공정 콤보: 수정 버림 질문(아니요 = 되돌림 / 예 = 바로 조회) / 삭제
            form.Answers.Enqueue(DialogResult.No);
            cboProgram.SelectedItem = "COWET";
            Application.DoEvents();
            Check("공정 COWET 고름 -> 수정 버림 질문(아니요) -> 원래 공정 TSVET 로 되돌림 / 필터 조건 그대로 28", cboProgram.Text == "TSVET" && para.Rows.Count == 37
                && form.LastQuestion.Contains("수정 1행") && cboFltCond.Text == condRes + " (28)" && Visible(para) == 28, cboProgram.Text);
            form.Answers.Enqueue(DialogResult.Yes);
            cboProgram.SelectedItem = "COWET";
            Application.DoEvents();
            Check("공정 COWET 고름 -> 버림(예) -> 바로 조회 40행 / Spec 40 / 공정이 바뀌어 필터 콤보 (전체)(40 모두 보임)", cboProgram.Text == "COWET" && para.Rows.Count == 40
                && status.Text.StartsWith("조회 완료 / Device " + etDevice + " / 공정 COWET / 등록 Parameter 40 / Spec 40") && filterCombos.All(c => c.SelectedIndex == 0) && Visible(para) == 40
                && !btnClearFilter.Enabled, status.Text);
            para.ClearSelection();
            para.Rows[0].Cells["colPHeader"].Selected = true;
            para.Rows[1].Cells["colPHeader"].Selected = true;
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnDelete_Click");
            Check("COWET Parameter 2개 삭제(Spec 같이) -> 38 / Spec 38", para.Rows.Count == 38 && status.Text == "삭제 2개" && bsl.GetEtLimit(Factory, etDevice, "COWET").Rows.Count == 38,
                status.Text);
            Check("진행 표시줄: 삭제 끝 문구 '삭제 (2개) 완료'(처리 시간 없음) / 작업 없음 / 모래시계 풀림",
                busyBar.LastResult == "삭제 (2개) 완료" && busyBar.DisplayText == "삭제 (2개) 완료" && !busyBar.IsBusy && !form.UseWaitCursor,
                busyBar.DisplayText);

            // 진행 표시줄: 작업 중 문구 / 모래시계 / 키 입력 막기, 끝나면 입력 받음 / 예외는 그대로 전달
            // Factory 칸은 2026-10-15 부터 읽기 전용(로그인 Factory)이라 키 입력 막기 검사 동안만 입력 가능하게 둔다
            string factoryText = txtFactory.Text;
            bool factoryReadOnly = txtFactory.ReadOnly;
            txtFactory.ReadOnly = false;
            bool sawBusy = false, sawCursor = false;
            string during = string.Empty;
            using (var probe = new System.Windows.Forms.Timer { Interval = 100 })
            {
                probe.Tick += (sender, e) =>
                {
                    if (!busyBar.IsBusy) return;
                    sawBusy = true;
                    sawCursor = form.UseWaitCursor;
                    during = busyBar.DisplayText;
                };
                probe.Start();
                PostMessage(txtFactory.Handle, WM_CHAR, (IntPtr)'Z', IntPtr.Zero);
                int answer = busyBar.Run("시험 작업", () =>
                {
                    System.Threading.Thread.Sleep(800);
                    return 7;
                });
                probe.Stop();
                Application.DoEvents();
                bool blocked = txtFactory.Text == factoryText;
                PostMessage(txtFactory.Handle, WM_CHAR, (IntPtr)'Z', IntPtr.Zero);
                Application.DoEvents();
                bool accepted = txtFactory.Text != factoryText;
                txtFactory.Text = factoryText;
                txtFactory.ReadOnly = factoryReadOnly;
                Check("진행 표시줄: 작업 중 '시험 작업 중...'(처리 시간 없음) / 모래시계 / 그동안 키 입력 막음 -> 끝나면 받음 / 결과 값 그대로",
                    answer == 7 && sawBusy && sawCursor && during == "시험 작업 중..." && blocked && accepted && busyBar.LastResult == "시험 작업 완료",
                    during + " / 막음 " + blocked + " / 받음 " + accepted);
            }
            string error = string.Empty;
            try
            {
                busyBar.Run("실패 시험", () => { throw new InvalidOperationException("시험 오류"); });
            }
            catch (InvalidOperationException ex)
            {
                error = ex.Message;
            }
            Check("진행 표시줄: 작업 예외는 그대로 전달 / '실패 시험 실패' 표시 / 입력 막기 풀림", error == "시험 오류" && busyBar.LastResult == "실패 시험 실패"
                && !busyBar.IsBusy && !form.UseWaitCursor, busyBar.LastResult);
            form.Close();
        }

        // ================= C) 등록 화면 다시 열기: 설정 화면에서 넣은 값이 보임 =================
        using (var form = new RegisterFixture())
        {
            form.SamplePath = args[0];
            ShowOffScreen(form);
            Set(rtype, form, "paraService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            ((TextBox)Get(rtype, form, "txtFactory")).Text = Factory;
            var header = (DataGridView)Get(rtype, form, "dgvHeader");
            Invoke(rtype, form, "btnOpenFile_Click");
            DataGridViewRow trRow = HeaderRow(header, ResTR);
            int done = header.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colHStatus") == "등록됨");
            Check("등록 화면 다시 열기: 등록됨 37 / TR 행 GDS Chain(tsvdc_tr_fl) · Spec(~ 715) 열", done == 37 && Cell(trRow, "colHGds") == "tsvdc_tr_fl"
                && Cell(trRow, "colHSpec").Trim() == "~ 715" && !Checked(trRow, "colHCheck") && trRow.Cells["colHCheck"].ReadOnly, Cell(trRow, "colHGds") + " / " + Cell(trRow, "colHSpec"));
            form.Close();
        }

        if (!keep) Cleanup(bsl, chainBsl, import);
        else Console.WriteLine("KEEP: " + Factory + " / " + etDevice + " 파일 번호 " + seq + " / 등록 Parameter / Spec / 시험 GDS Map · Chain 유지");

        if (failures == 0) { Console.WriteLine("ET_PARA_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_PARA_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>헤더 원문 -> 저장 이름 (화면 / 서버와 같은 규칙)</summary>
    private static string N(string header)
    {
        return EtParameterNameRule.ToParameterName(header);
    }

    /// <summary>
    /// 시험 파일(Slot 4) / 시험 GDS Chain(ZZETU*) / PROBE + E-Test Device 의 모든 공정 등록 Parameter(Spec 같이)를 지우고 시험 GDS Map(E-Test Device 이름)을 지운다.
    /// 값이 저장된 Parameter 는 삭제가 거부되므로 파일을 먼저 지운다.
    /// </summary>
    private static void Cleanup(DACrux.SEMDMS.BSL.GdsEtResult bsl, DACrux.SEMDMS.BSL.GdsChain chainBsl, DACrux.SEMDMS.BSL.GdsMapImport import)
    {
        foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, "RH-HS2615", Slot }).Rows)
            bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
        foreach (DataRow dr in chainBsl.GetChainList(Factory, GdsDevice).Rows)
            if (dr["CHAIN_CODE"].ToString().StartsWith("ZZETU", StringComparison.Ordinal))
                chainBsl.DeleteChain(Factory, GdsDevice, dr["CHAIN_SEQ"].ToString(), "SMOKE");
        foreach (DataRow dr in bsl.GetEtParaProgramList(Factory).Rows)
        {
            if (dr["DEVICE_ID"].ToString() != etDevice) continue;
            string program = dr["TEST_PROGRAM"].ToString();
            string[] names = bsl.GetEtParameterList(Factory, etDevice, program).Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()).ToArray();
            if (names.Length > 0) bsl.DeleteEtParameters(new[] { Factory, etDevice, program }, names, "SMOKE");
        }
        if (import.GetMapInfo(Factory, GdsDevice).Rows.Count > 0) import.DeleteMap(Factory, GdsDevice, "SMOKE");
    }

    /// <summary>업로드 화면과 같은 순서로 샘플을 저장한다(Slot 4, 등록 Parameter 만 + 판정).</summary>
    private static string Upload(DACrux.SEMDMS.BSL.GdsEtResult bsl, string path)
    {
        var f = DACrux.SEMDMS.ENGUI.EtResultFileParser.Parse(path);
        string program = EtParameterNameRule.ToTestProgram(f.TestProgram);
        var plan = DACrux.SEMDMS.ENGUI.EtUploadPlan.Build(f, bsl.GetEtParameterList(Factory, etDevice, program));
        string seq = bsl.BeginEtFile(new[] { Factory, etDevice, f.FileName, f.LotId, Slot, program, f.TestTime, "", f.SheetName,
            f.DieCount.ToString(CultureInfo.InvariantCulture), plan.Matched.Count.ToString(CultureInfo.InvariantCulture), "SMOKE",
            f.Customer, f.FileDevice, f.WaferType, f.WaferRev, f.WaferId, f.FabName,
            f.ItemCount.ToString(CultureInfo.InvariantCulture), plan.RegisteredCount.ToString(CultureInfo.InvariantCulture), plan.Missing.Count.ToString(CultureInfo.InvariantCulture) })
            .Rows[0]["SRC_FILE_SEQ"].ToString();
        bsl.CreateEtDieBatch(seq, f.Dies, "SMOKE");
        foreach (var batch in plan.BuildValueBatches(f, 10000)) bsl.CreateEtValueBatch(seq, batch, "SMOKE");
        string result = bsl.CompleteEtFile(seq, "SMOKE").Rows[0]["RESULT"].ToString();
        if (result != "OK") throw new InvalidOperationException("준비 업로드 실패: " + result);
        return seq;
    }

    /// <summary>시험 GDS Map 의 Layer 1 도형 하나를 Input 으로 하는 Setup Chain 을 만든다(GdsChain 서비스, GdsChainProbe 와 같은 방식).</summary>
    private static string NewGdsChain(DACrux.SEMDMS.BSL.GdsChain chainBsl, string code, string name)
    {
        string id = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL().GetData("SELECT_PLACED_EL_PAGE", null, new[] { Factory, GdsDevice, "1", " ", "1" })
            .Rows[0]["PLACED_ELEMENT_ID"].ToString();
        string seq = chainBsl.BeginChainSave(new[] { Factory, GdsDevice, "", code, name, "-16711936", "0", "10", "SMOKE" }, new[,] { { "1" } }, null).Rows[0]["CHAIN_SEQ"].ToString();
        chainBsl.CreateChainMemberBatch(new[] { Factory, GdsDevice, seq }, new[,] { { id, "INPUT", "ENDPOINT", "0", "0" } }, "SMOKE");
        string result = chainBsl.CompleteChainSave(new[] { Factory, GdsDevice, seq, "1", "0" }, "SMOKE").Rows[0]["RESULT"].ToString();
        if (result != "OK") throw new InvalidOperationException("시험 GDS Chain 저장 실패: " + code + " " + result);
        return seq;
    }

    /// <summary>DB 의 등록 Parameter 목록(GetEtParameterSetupList) 1행</summary>
    private static DataRow SetupRow(DACrux.SEMDMS.BSL.GdsEtResult bsl, string name)
    {
        return bsl.GetEtParameterSetupList(Factory, etDevice, "TSVET").Rows.Cast<DataRow>().First(r => r["ITEM_NAME"].ToString() == name);
    }

    private static void ShowOffScreen(Form form)
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-32000, -32000);
        form.ShowInTaskbar = false;
        form.Show();
        Application.DoEvents();
    }

    private static void SaveImage(Form form, string path)
    {
        form.Size = new Size(1300, 760);
        Application.DoEvents();
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(path);
        }
    }

    private static bool HasField(Type type, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) != null;
    }

    private static int CheckedHeader(Type type, object form)
    {
        return ((List<DataGridViewRow>)CallRet(type, form, "CheckedHeaderRows")).Count;
    }

    private static int Visible(DataGridView grid)
    {
        return grid.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
    }

    /// <summary>저장 안 한 수정 행 수 (화면 DirtyParaRows). 2026-10-15 화면에서 정보 줄이 빠져 직접 센다.</summary>
    private static int Dirty(Type type, object form)
    {
        return ((List<DataGridViewRow>)CallRet(type, form, "DirtyParaRows")).Count;
    }

    /// <summary>GDS Chain 칸이 주황 글자(GDS Setup Chain 목록에 없음)인 행 수</summary>
    private static int Orange(DataGridView grid)
    {
        return grid.Rows.Cast<DataGridViewRow>().Count(r => r.Cells["colPGds"].Style.ForeColor == Color.DarkOrange);
    }

    /// <summary>GDS Chain 칸에 값이 있는 행 수</summary>
    private static int GdsCount(DataGridView grid)
    {
        return grid.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colPGds").Trim().Length > 0);
    }

    /// <summary>화면의 (object, EventArgs) 처리기를 sender 를 넣어 부른다(필터 콤보 펼치기 등).</summary>
    private static void InvokeWith(Type type, object instance, string method, object sender)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { sender, EventArgs.Empty });
        Application.DoEvents();
    }

    /// <summary>콤보에서 글자가 "값 (" 로 시작하는 항목을 고른다(사용자가 고른 것과 같이 SelectedIndexChanged 가 돈다).</summary>
    private static void SelectItem(ComboBox combo, string value)
    {
        for (int i = 0; i < combo.Items.Count; i++)
        {
            if (!combo.Items[i].ToString().StartsWith(value + " (", StringComparison.Ordinal)) continue;
            combo.SelectedIndex = i;
            Application.DoEvents();
            return;
        }
        throw new InvalidOperationException("콤보 항목 없음: " + value + " / " + Items(combo));
    }

    private static string Items(ComboBox combo)
    {
        return string.Join(",", combo.Items.Cast<object>().Select(x => x.ToString()).ToArray());
    }

    private static DataGridViewRow HeaderRow(DataGridView grid, string name)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colHName") == name) return r;
        throw new InvalidOperationException("헤더 행 없음: " + name);
    }

    private static DataGridViewRow ParaRow(DataGridView grid, string name)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colPName") == name) return r;
        throw new InvalidOperationException("Parameter 행 없음: " + name);
    }

    private static DataGridViewRow GdsRow(DataGridView grid, string name)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colGName") == name) return r;
        throw new InvalidOperationException("GDS 행 없음: " + name);
    }

    private static string Cell(DataGridViewRow row, string column)
    {
        return row == null ? "(행 없음)" : Convert.ToString(row.Cells[column].Value, CultureInfo.InvariantCulture);
    }

    private static bool Checked(DataGridViewRow row, string column)
    {
        object v = row.Cells[column].Value;
        return v is bool && (bool)v;
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

    private static void InvokeCell(Type type, object instance, string method, DataGridView grid, int row, int column)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { grid, new DataGridViewCellEventArgs(column, row) });
        Application.DoEvents();
    }

    private static void Key(Type type, object instance, string method, Keys keys)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { null, new KeyEventArgs(keys) });
        Application.DoEvents();
    }

    /// <summary>칸을 더블클릭해 편집을 시작하고 글자를 넣은 뒤 편집을 끝낸다(사용자 입력과 같은 경로).</summary>
    private static void EditCell(Type type, object instance, DataGridView grid, DataGridViewRow row, string column, string text)
    {
        InvokeCell(type, instance, "dgvPara_CellDoubleClick", grid, row.Index, grid.Columns[column].Index);
        if (!grid.IsCurrentCellInEditMode) throw new InvalidOperationException("편집이 시작되지 않음: " + column);
        grid.EditingControl.Text = text;
        grid.EndEdit();
        Application.DoEvents();
    }

    /// <summary>표 행의 칸을 마우스 오른쪽 버튼으로 누른 것처럼 CellMouseDown 처리기를 부른다.</summary>
    private static void RightClick(Type type, object instance, string method, DataGridView grid, int rowIndex, string column)
    {
        var args = new DataGridViewCellMouseEventArgs(grid.Columns[column].Index, rowIndex, 5, 5, new MouseEventArgs(MouseButtons.Right, 1, 5, 5, 0));
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { grid, args });
        Application.DoEvents();
    }

    private static object CallRet(Type type, object instance, string method)
    {
        object result = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(instance, null);
        Application.DoEvents();
        return result;
    }

    /// <summary>등록 화면: 공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창 / 파일 고르기만 바꾼다.</summary>
    private sealed class RegisterFixture : DACrux.SEMDMS.ENGUI.frmEtParaRegister
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public string SamplePath = "";

        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }

        protected override string ChooseFile()
        {
            return SamplePath;
        }

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

    /// <summary>설정 화면: 공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창 / 클립보드만 바꾼다.</summary>
    private sealed class SetupFixture : DACrux.SEMDMS.ENGUI.frmEtParaSetup
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public readonly List<string> Questions = new List<string>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public string Clipboard = "";
        public string Copied = "";

        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }

        protected override string ClipboardText()
        {
            return Clipboard;
        }

        protected override void SetClipboardText(string text)
        {
            Copied = text;
        }

        protected override DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            LastQuestion = text;
            Questions.Add(text);
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
