using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// E-Test Spec 설정 화면(frmEtLimitSetup)을 화면 밖에서 띄워 Spec 표 붙여넣기 -> 조회 -> 수정 / 삭제 -> Setup Chain 으로 찾기 -> Chain 묶기 흐름을 검사한다. (2026-10-12 Parameter 단위로 다시 작성)
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다(화면 -> RO -> BSL -> 로컬 DB).
/// 준비: 샘플 TSVET xlsx 를 EtResultFileParser + BSL 로 FACTORY=PROBE / DEVICE_ID=ET_LIMIT_SMOKE / Slot 4 (RH-HS2615-04) 에 올린다.
/// 인자: [0] E-Test xlsx, [1] 결과 이미지(png), [2] 고객 Spec 표(탭 구분 텍스트), [3] keep(선택: 저장한 파일 / Spec / Chain 묶음을 지우지 않음)
/// 기대값(샘플 + 고객 표): 표 119행(TSVET 37 / COWET 40 / FT 42), TSVET 37개 모두 데이터 있음, Fail 합 6 (#44 의 RES 6항목 718~719 Ohm > 715)
/// </summary>
internal static class EtLimitSetupFormSmoke
{
    private const string Factory = "PROBE";
    private const string Device = "ET_LIMIT_SMOKE";
    private const string ResRN = "RESISTANCE_IFORCE=8.0MA@TSVDC_R_N_FH";
    private const string ResLP = "RESISTANCE_IFORCE=8.0MA@TSVDC_L_P_FH";
    private const string ResBot1P = "RESISTANCE_IFORCE=8.0MA@TSVDC_BOT1_P_FH";
    private const string ResTR = "RESISTANCE_IFORCE=8.0MA@TSVDC_TR_FH";
    private const string ResAsic1P = "RESISTANCE_IFORCE=8.0MA@TSVDC_ASIC1_P_FH";
    private const string LkgAsic1 = "LEAKAGE_SERIAL@TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL";
    private static readonly string[] FailItems = {
        ResRN, ResLP, "RESISTANCE_IFORCE=8.0MA@TSVDC_L_N_FH", ResBot1P, "RESISTANCE_IFORCE=8.0MA@TSVDC_BOT1_N_FH", "RESISTANCE_IFORCE=8.0MA@TSVDC_HBM1_N_FH" };
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
        string specText = File.ReadAllText(args[2]);
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // ---- 0) 준비: 이전 잔여 정리 + 샘플 업로드 (Slot 4) ----
        Cleanup(bsl, null);
        string seq = Upload(bsl, args[0]);
        Console.WriteLine("준비: 파일 번호 " + seq);

        // ---- 1) Spec 표 해석 (화면 없이) ----
        var parsed = DACrux.SEMDMS.ENGUI.EtSpecTable.Parse(specText);
        Check("Spec 표 119행 / 오류 0 / TSVET 37 COWET 40 FT 42", parsed.Count == 119 && parsed.All(r => r.Error == null)
            && parsed.Count(r => r.TestProgram == "TSVET") == 37 && parsed.Count(r => r.TestProgram == "COWET") == 40 && parsed.Count(r => r.TestProgram == "FT") == 42,
            parsed.Count + " / 오류 " + parsed.Count(r => r.Error != null));
        var ftRow = parsed.First(r => r.TestProgram == "FT" && r.ItemName.StartsWith("LEAKAGE_SERIAL@TSVDC_BOT1_N_FL", StringComparison.Ordinal));
        Check("FT '#번호' 제거 / 대문자 / ±50", ftRow.ItemName == "LEAKAGE_SERIAL@TSVDC_BOT1_N_FL:TSVDC_BOT1_P_FH" && ftRow.Lower == "-50" && ftRow.Upper == "50" && ftRow.Unit == "uA",
            ftRow.ItemName + " / " + ftRow.Lower + " / " + ftRow.Upper);
        var resRow = parsed.First(r => r.TestProgram == "TSVET" && r.ItemName == ResTR);
        Check("RES 'If val > 715' -> Upper 715 만", resRow.Lower == "" && resRow.Upper == "715" && resRow.Unit == "Ohm", resRow.Lower + " / " + resRow.Upper);
        string lo, up, err;
        Check("Spec 문장: val < 1 -> Lower 1", DACrux.SEMDMS.ENGUI.EtSpecTable.TryParseSpec("If val < 1, Fail; otherwise, Pass", out lo, out up, out err) && lo == "1" && up == "", lo + "/" + up);
        Check("Spec 문장: >= 는 오류", !DACrux.SEMDMS.ENGUI.EtSpecTable.TryParseSpec("If val >= 715, Fail; otherwise, Pass", out lo, out up, out err), err);
        Check("헤더 정리: 공백 / '#717'", DACrux.SEMDMS.ENGUI.EtResultFileParser.NormalizeHeader("Resistance_iforce=8.0mA@TSVDC_HBM_C_P_FH  #717 ") == "Resistance_iforce=8.0mA@TSVDC_HBM_C_P_FH", "");

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
            var cboProgram = (ComboBox)Get(type, form, "cboTestProgram");
            var grid = (DataGridView)Get(type, form, "dgvLimit");
            var chain = (DataGridView)Get(type, form, "dgvChain");
            var txtFilter = (TextBox)Get(type, form, "txtChainFilter");
            var status = (Label)Get(type, form, "lblStatus");

            // ---- 2) 조회: Spec 없음 ----
            txtFactory.Text = Factory;
            Call(type, form, "LoadDeviceList");
            Check("Device 목록에 ET_LIMIT_SMOKE", cboDevice.Items.Contains(Device), cboDevice.Items.Count.ToString());
            cboDevice.Text = Device;
            CallArgs(type, form, "LoadTestProgramList", new object[] { null });
            Check("공정 목록 TSVET (업로드 파일)", cboProgram.Items.Count == 1 && cboProgram.Text == "TSVET", cboProgram.Text);
            Invoke(type, form, "btnSearch_Click");
            Check("Spec 없음 -> 0행 / Chain 1,306", grid.Rows.Count == 0 && chain.Rows.Count == 1306, grid.Rows.Count + " / " + chain.Rows.Count);
            var chainInfo = (Label)Get(type, form, "lblChainInfo");
            Check("Setup Chain 목록 출처 표시", chainInfo.Text.Contains("Setup Chain 목록: Smoke 고정 3개") && chainInfo.Text.Contains("Setup Chain 연결 3"), chainInfo.Text);

            // ---- 3) 붙여넣기: 잘못된 행이 있으면 저장 안 함 ----
            form.Clipboard = "U9\tTSVET\tResistance_iforce=8.0mA@TSVDC_TR_FH\tResistance\tIf val >= 715, Fail; otherwise, Pass\tOhm";
            Invoke(type, form, "btnPaste_Click");
            Check("해석 못 하는 Spec -> 저장 안 함", form.LastNotice.Contains("해석할 수 없는 행이 1개") && bsl.GetEtLimit(Factory, Device, "TSVET").Rows.Count == 0, form.LastNotice.Split('\r')[0]);

            // ---- 4) 붙여넣기: 고객 표 ----
            form.Clipboard = specText;
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnPaste_Click");
            Check("표 저장 질문 (공정별 건수 / 표 Device U9 대신)", form.LastQuestion.Contains("COWET 40 / FT 42 / TSVET 37") && form.LastQuestion.Contains("표의 Device U9 대신"), form.LastQuestion.Split('\r')[0]);
            Check("표 저장 완료 상태", status.Text.StartsWith("Spec 표 저장 완료 / COWET 40 / FT 42 / TSVET 37"), status.Text);
            Check("공정 목록 COWET / FT / TSVET, 지금 공정 TSVET", string.Join(",", cboProgram.Items.Cast<object>().Select(x => x.ToString()).ToArray()) == "COWET,FT,TSVET" && cboProgram.Text == "TSVET",
                cboProgram.Text);
            Check("TSVET 37행 / 모두 대문자 / 모두 데이터 있음", grid.Rows.Count == 37 && grid.Rows.Cast<DataGridViewRow>().All(r => Cell(r, "colItem") == Cell(r, "colItem").ToUpperInvariant() && Cell(r, "colInData") == "Y"),
                grid.Rows.Count + " / 데이터 있음 " + grid.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colInData") == "Y"));
            var fails = grid.Rows.Cast<DataGridViewRow>().Where(r => Cell(r, "colFail") != "0").Select(r => Cell(r, "colItem")).OrderBy(x => x).ToArray();
            Check("Fail 6 항목 (#44 RES 718~719)", string.Join(",", fails) == string.Join(",", FailItems.OrderBy(x => x).ToArray())
                && grid.Rows.Cast<DataGridViewRow>().Sum(r => int.Parse(Cell(r, "colFail"), NumberStyles.AllowThousands, CultureInfo.InvariantCulture)) == 6, string.Join(",", fails));
            var rn = Row(grid, ResRN);
            Check("R_N: Upper 715 / Lower 없음 / Enable / Ohm / 값 24 / 최대 > 715 / Spec 원문", Cell(rn, "colUpper") == "715" && Cell(rn, "colLower") == "" && Checked(rn)
                && Cell(rn, "colUnit") == "Ohm" && Cell(rn, "colValues") == "24" && double.Parse(Cell(rn, "colMax"), CultureInfo.InvariantCulture) > 715
                && Cell(rn, "colSpec") == "If val > 715, Fail; otherwise, Pass" && Cell(rn, "colFail") == "1",
                Cell(rn, "colUpper") + " / " + Cell(rn, "colValues") + " / " + Cell(rn, "colMax") + " / " + Cell(rn, "colFail"));
            var lk = Row(grid, LkgAsic1);
            Check("Leakage: ±50 / uA / Fail 0", Cell(lk, "colLower") == "-50" && Cell(lk, "colUpper") == "50" && Cell(lk, "colUnit") == "uA" && Cell(lk, "colFail") == "0",
                Cell(lk, "colLower") + " / " + Cell(lk, "colUpper") + " / " + Cell(lk, "colFail"));

            // ---- 5) FT: '#' 제거 / 데이터 없음 ----
            var ft = bsl.GetEtLimit(Factory, Device, "FT");
            Check("FT 42 / '#' 없음", ft.Rows.Count == 42 && ft.Rows.Cast<DataRow>().All(r => r["ITEM_NAME"].ToString().IndexOf('#') < 0)
                && ft.Rows.Cast<DataRow>().Any(r => r["ITEM_NAME"].ToString() == "LEAKAGE_SERIAL@TSVDC_BOT1_N_FL:TSVDC_BOT1_P_FH"), ft.Rows.Count.ToString());
            cboProgram.Text = "FT";
            Invoke(type, form, "btnSearch_Click");
            Check("FT 조회: 42행 / 데이터 없음(N) / 상태 '데이터 없음 42'", grid.Rows.Count == 42 && grid.Rows.Cast<DataGridViewRow>().All(r => Cell(r, "colInData") == "N") && status.Text.Contains("데이터 없음 42"),
                status.Text);
            cboProgram.Text = "TSVET";
            Invoke(type, form, "btnSearch_Click");

            // ---- 6) 수정: Enable 해제 / Upper 완화 / 비고 / 잘못된 값 ----
            rn = Row(grid, ResRN);
            rn.Cells["colEnable"].Value = false;
            Application.DoEvents();
            var lp = Row(grid, ResLP);
            SetCell(lp, "colUpper", "720");
            SetCell(lp, "colRemark", "Smoke 완화");
            Check("수정 행 노란색", lp.Cells["colUpper"].Style.BackColor == Color.LightYellow, lp.Cells["colUpper"].Style.BackColor.Name);
            var tr = Row(grid, ResTR);
            SetCell(tr, "colLower", "800");
            Check("Lower > Upper -> 붉은색", tr.Cells["colLower"].Style.BackColor == Color.MistyRose, tr.Cells["colLower"].Style.BackColor.Name);
            Invoke(type, form, "btnSave_Click");
            Check("잘못된 행이 있으면 저장 안 함", form.LastNotice.Contains(ResTR) && form.LastNotice.Contains("Lower 가 Upper"), form.LastNotice.Split('\r')[0]);
            SetCell(tr, "colLower", "");
            Invoke(type, form, "btnSave_Click");
            Check("저장 2행", status.Text.StartsWith("저장 완료 / 저장 2 / 삭제 0"), status.Text);
            rn = Row(grid, ResRN); lp = Row(grid, ResLP);
            Check("R_N Enable 해제 -> Fail 0", !Checked(rn) && Cell(rn, "colFail") == "0", Cell(rn, "colFail"));
            Check("L_P Upper 720 -> Fail 0 / 비고 / Spec 원문 유지", Cell(lp, "colUpper") == "720" && Cell(lp, "colFail") == "0" && Cell(lp, "colRemark") == "Smoke 완화"
                && Cell(lp, "colSpec") == "If val > 715, Fail; otherwise, Pass", Cell(lp, "colFail") + " / " + Cell(lp, "colSpec"));
            Check("수정자 표시", Cell(lp, "colUpdate").StartsWith("SMOKE "), Cell(lp, "colUpdate"));

            // ---- 7) 삭제: [선택 행 삭제] / 값 모두 지우고 저장 ----
            grid.ClearSelection();
            Row(grid, ResBot1P).Cells["colItem"].Selected = true;
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnDelete_Click");
            Check("[선택 행 삭제] -> 36행", grid.Rows.Count == 36 && status.Text.StartsWith("삭제 완료 / Parameter 1개"), grid.Rows.Count + " / " + status.Text);
            tr = Row(grid, ResTR);
            SetCell(tr, "colUpper", "");
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            Check("값 모두 지움 -> 삭제 질문 -> 35행", form.LastQuestion.Contains(ResTR) && grid.Rows.Count == 35 && status.Text.Contains("삭제 1"), status.Text);

            // ---- 8) 서비스 규칙 ----
            bsl.SaveEtLimit(new[] { Factory, Device, "tsvet" }, new[,] { { " resistance_iforce=8.0mA@TSVDC_TR_FH  #717", "", "700", "y", "", "", "소문자 등록" } }, "SMOKE");
            DataRow saved = bsl.GetEtLimit(Factory, Device, "TSVET").Rows.Cast<DataRow>().FirstOrDefault(r => r["ITEM_NAME"].ToString() == ResTR);
            Check("소문자 / '#717' / 공정 소문자 -> 정리해서 저장, 단위는 측정 항목(Ohm)", saved != null && saved["ENABLE_FLAG"].ToString() == "Y" && saved["UNIT"].ToString() == "Ohm"
                && saved["REMARK"].ToString() == "소문자 등록", saved == null ? "없음" : saved["ITEM_NAME"] + " / " + saved["UNIT"]);
            Check("아직 업로드 안 된 Parameter 도 저장", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device, "COWET" }, new[,] { { "NEW_PARA@NEW_CHAIN_FH", "", "1", "Y", "Ohm", "", "" } }, "SMOKE")) == "", "");
            Check("서비스 거부: '@' 없음", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" }, new[,] { { "RESISTANCE_IFORCE", "", "1", "Y", "", "", "" } }, "SMOKE")).Contains("측정 헤더 전체"), "");
            Check("서비스 거부: Lower / Upper 모두 없음", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" }, new[,] { { ResTR, "", "", "Y", "", "", "" } }, "SMOKE")).Contains("하나는"), "");
            Check("서비스 거부: 숫자 아님", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" }, new[,] { { ResTR, "abc", "", "Y", "", "", "" } }, "SMOKE")).Contains("숫자가 아닙니다"), "");
            Check("서비스 거부: 같은 Parameter 두 번('#' 만 다름)", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device, "TSVET" },
                new[,] { { ResTR + " #1", "", "1", "Y", "", "", "" }, { ResTR + " #2", "", "1", "Y", "", "", "" } }, "SMOKE")).Contains("두 번"), "");
            Check("서비스 거부: 키 2개(이전 방식)", Throws(() => bsl.SaveEtLimit(new[] { Factory, Device }, new[,] { { ResTR, "", "1", "Y", "", "", "" } }, "SMOKE")).Contains("3개 항목"), "");

            // ---- 9) Setup Chain 으로 찾기: 묶기 전 (같은 이름 TSVDC_ASIC1_P_FL 만) ----
            Invoke(type, form, "btnSearch_Click");
            var cboSetup = (ComboBox)Get(type, form, "cboSetupChain");
            var lstSetup = (ListBox)Get(type, form, "lstSetupChain");
            var lookup = (DataGridView)Get(type, form, "dgvLookup");
            var lookupInfo = (Label)Get(type, form, "lblLookupInfo");
            Check("Setup Chain 목록 3개 (Smoke 고정)", lstSetup.Items.Count == 3 && cboSetup.Items.Count == 3, lstSetup.Items.Count.ToString());
            var pChains = new[] { "TSVDC_ASIC1_P_FH", "TSVDC_ASIC1_P_FL" };
            Func<string[], int> definedFor = chains => parsed.Count(r => {
                string tail = r.ItemName.Substring(r.ItemName.IndexOf('@') + 1);
                return tail.Split(':').Any(c => chains.Contains(c));
            });
            cboSetup.Text = "TSVDC_ASIC1_P_FL";
            Invoke(type, form, "btnLookup_Click");
            int expectFl = definedFor(new[] { "TSVDC_ASIC1_P_FL" });
            Check("묶기 전 찾기 TSVDC_ASIC1_P_FL: 같은 이름 1개 / Spec " + expectFl + " / Spec 없는 항목 1 (Leakage_Parallel)",
                lookupInfo.Text.Contains("TSVDC_ASIC1_P_FL (같은 이름)") && !lookupInfo.Text.Contains("TSVDC_ASIC1_P_FH")
                && LookupRows(lookup, "Y") == expectFl && LookupRows(lookup, "N") == 1, lookupInfo.Text.Replace("\r\n", " / "));

            // ---- 10) Chain 묶기 (바로 저장) ----
            txtFilter.Text = "TSVDC_ASIC1_P";
            Application.DoEvents();
            var visible = chain.Rows.Cast<DataGridViewRow>().Where(r => r.Visible).ToList();
            Check("Chain 찾기 'TSVDC_ASIC1_P' -> _FH / _FL 2행", visible.Count == 2 && Cell(visible[0], "colChainName") == "TSVDC_ASIC1_P_FH" && Cell(visible[1], "colChainName") == "TSVDC_ASIC1_P_FL",
                string.Join(",", visible.Select(r => Cell(r, "colChainName")).ToArray()));
            Check("묶기 전 Setup Chain 열: _FL 만 연결", Cell(visible[0], "colChainSetup") == "" && Cell(visible[1], "colChainSetup") == "TSVDC_ASIC1_P_FL",
                Cell(visible[0], "colChainSetup") + " / " + Cell(visible[1], "colChainSetup"));
            chain.ClearSelection();
            foreach (var r in visible) r.Selected = true;
            Application.DoEvents();
            int asked = form.AskCount;
            Invoke(type, form, "btnChainGroup_Click");
            Check("2개 묶기 -> 질문 없이 저장", form.AskCount == asked && status.Text.StartsWith("묶음 완료 / Chain 2개"), status.Text);
            visible = chain.Rows.Cast<DataGridViewRow>().Where(r => r.Visible).ToList();
            Check("묶음 Chain / 묶음 수 2 / 비고 / 수정자", visible.All(r => Cell(r, "colChainGroup") == "TSVDC_ASIC1_P_FH + TSVDC_ASIC1_P_FL" && Cell(r, "colChainGroupCount") == "2"
                && Cell(r, "colChainRemark") == "수동 묶음" && Cell(r, "colChainUpdate").StartsWith("SMOKE ")), Cell(visible[0], "colChainGroup"));
            Check("묶은 뒤 Setup Chain 열: _FH 도 TSVDC_ASIC1_P_FL", visible.All(r => Cell(r, "colChainSetup") == "TSVDC_ASIC1_P_FL"), Cell(visible[0], "colChainSetup"));
            Check("Spec 탭 묶음 / Setup Chain: RES ASIC1_P_FH", Cell(Row(grid, ResAsic1P), "colGroup") == "TSVDC_ASIC1_P_FH + TSVDC_ASIC1_P_FL"
                && Cell(Row(grid, ResAsic1P), "colSetup") == "TSVDC_ASIC1_P_FL", Cell(Row(grid, ResAsic1P), "colGroup") + " / " + Cell(Row(grid, ResAsic1P), "colSetup"));
            Check("Spec 탭 묶음: Leakage ASIC1_N_FH:ASIC1_P_FL -> 'N_FH : P_FH + P_FL'", Cell(Row(grid, LkgAsic1), "colGroup") == "TSVDC_ASIC1_N_FH : TSVDC_ASIC1_P_FH + TSVDC_ASIC1_P_FL",
                Cell(Row(grid, LkgAsic1), "colGroup"));

            // ---- 11) Setup Chain 으로 찾기: 묶은 뒤 (묶인 _FH 의 Spec 까지 전부) ----
            Invoke(type, form, "btnLookup_Click");
            int expectGroup = definedFor(pChains);
            Check("묶은 뒤 찾기 TSVDC_ASIC1_P_FL: 같은 이름 + 묶음 / Spec " + expectGroup + " (3 공정) / Spec 없는 항목 2",
                lookupInfo.Text.Contains("TSVDC_ASIC1_P_FL (같은 이름)") && lookupInfo.Text.Contains("TSVDC_ASIC1_P_FH (묶음)") && lookupInfo.Text.Contains("(COWET, FT, TSVET)")
                && LookupRows(lookup, "Y") == expectGroup && LookupRows(lookup, "N") == 2 && expectGroup > expectFl, lookupInfo.Text.Replace("\r\n", " / "));
            var lkRes = lookup.Rows.Cast<DataGridViewRow>().First(r => Cell(r, "colLkProgram") == "TSVET" && Cell(r, "colLkItem") == ResAsic1P);
            Check("찾은 행: TSVET RES ASIC1_P_FH Upper 715 / 파일 1 / 값 24 / Fail 0 / 찾은 Chain _FH", Cell(lkRes, "colLkUpper") == "715" && Cell(lkRes, "colLkFiles") == "1"
                && Cell(lkRes, "colLkValues") == "24" && Cell(lkRes, "colLkFail") == "0" && Cell(lkRes, "colLkMatch") == "TSVDC_ASIC1_P_FH",
                Cell(lkRes, "colLkUpper") + " / " + Cell(lkRes, "colLkFiles") + " / " + Cell(lkRes, "colLkValues") + " / " + Cell(lkRes, "colLkMatch"));
            var lkFt = lookup.Rows.Cast<DataGridViewRow>().First(r => Cell(r, "colLkProgram") == "FT" && Cell(r, "colLkItem") == "LEAKAGE_SERIAL@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH");
            Check("찾은 행: FT Leakage N_FL:P_FH (묶인 _FH 로 찾음) / 파일 0", Cell(lkFt, "colLkFiles") == "0" && Cell(lkFt, "colLkLower") == "-50", Cell(lkFt, "colLkMatch"));
            var lkUndef = lookup.Rows.Cast<DataGridViewRow>().Where(r => Cell(r, "colLkDefined") == "N").Select(r => Cell(r, "colLkItem")).OrderBy(x => x).ToArray();
            Check("Spec 없는 측정 항목: Connectivity _FH / Leakage_Parallel _FL (원문 이름)", string.Join(",", lkUndef) == "Connectivity_iforce=8.0mA@TSVDC_ASIC1_P_FH,Leakage_Parallel@TSVDC_ASIC1_P_FL",
                string.Join(",", lkUndef));
            int resIndex = lookup.Rows.IndexOf(lkRes);
            lookup.GetType().GetMethod("OnCellDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(lookup, new object[] { new DataGridViewCellEventArgs(1, resIndex) });
            Application.DoEvents();
            var tabMain = (TabControl)Get(type, form, "tabMain");
            Check("찾은 행 더블클릭 -> [Spec] 탭 그 공정(TSVET) / Parameter", tabMain.SelectedTab.Name == "tabSpec" && cboProgram.Text == "TSVET" && grid.CurrentRow != null && Cell(grid.CurrentRow, "colItem") == ResAsic1P,
                tabMain.SelectedTab.Name + " / " + cboProgram.Text + " / " + (grid.CurrentRow == null ? "" : Cell(grid.CurrentRow, "colItem")) + " / idx " + resIndex + " " + Cell(lookup.Rows[resIndex], "colLkProgram"));
            var lkCowet = lookup.Rows.Cast<DataGridViewRow>().First(r => Cell(r, "colLkProgram") == "COWET");
            tabMain.SelectedTab = (TabPage)Get(type, form, "tabLookup");
            lookup.GetType().GetMethod("OnCellDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(lookup, new object[] { new DataGridViewCellEventArgs(1, lookup.Rows.IndexOf(lkCowet)) });
            Application.DoEvents();
            Check("다른 공정(COWET) 행 더블클릭 -> 공정 COWET 로 바꿔 Spec 조회", cboProgram.Text == "COWET" && grid.CurrentRow != null && Cell(grid.CurrentRow, "colItem") == Cell(lkCowet, "colLkItem").ToUpperInvariant()
                && status.Text.Contains("공정 COWET"), cboProgram.Text + " / " + status.Text);

            cboSetup.Text = "PIC_COMB12_M4";
            Invoke(type, form, "btnLookup_Click");
            Check("찾기 PIC_COMB12_M4: 같은 이름 없음 -> _FH / _FL (SUFFIX) / Spec 0", lookupInfo.Text.Contains("PIC_COMB12_M4_FH (이름 + _FH / _FL)") && lookupInfo.Text.Contains("PIC_COMB12_M4_FL (이름 + _FH / _FL)")
                && LookupRows(lookup, "Y") == 0 && LookupRows(lookup, "N") > 0, lookupInfo.Text.Replace("\r\n", " / "));
            cboSetup.Text = "NO_SUCH_CHAIN";
            Invoke(type, form, "btnLookup_Click");
            Check("찾기 NO_SUCH_CHAIN: 찾은 Chain 없음 / 0행", lookupInfo.Text.Contains("찾은 E-Test Chain 없음") && lookup.Rows.Count == 0, lookupInfo.Text);
            DataTable svcInfo = bsl.GetEtChainSpecInfo(Factory, Device, "tsvdc_asic1_p_fl");
            Check("서비스 GetEtChainSpecInfo (소문자 이름) = 화면과 같은 행 수", svcInfo.Rows.Count == expectGroup + 2 && svcInfo.Columns.Contains("MATCH_CHAIN"), svcInfo.Rows.Count.ToString());

            // ---- 12) FH / FL 짝 자동 묶기 / 옮기기 / 해제 ----
            txtFilter.Text = "";
            Application.DoEvents();
            var names = new HashSet<string>(chain.Rows.Cast<DataGridViewRow>().Select(r => Cell(r, "colChainName")));
            int expectPairs = names.Count(n => n.EndsWith("_FH") && names.Contains(n.Substring(0, n.Length - 3) + "_FL")) - 1;
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnChainAuto_Click");
            Check("FH / FL 자동 묶기 " + expectPairs + "쌍 (이미 묶인 ASIC1_P 제외)", form.LastQuestion.Contains(expectPairs + "쌍") && status.Text == "FH / FL 자동 묶음 완료 / " + expectPairs + "쌍", status.Text);
            Check("자동 묶음 예: PIC_COMB12_M4 / 비고", Cell(ChainRow(chain, "PIC_COMB12_M4_FH"), "colChainGroup") == "PIC_COMB12_M4_FH + PIC_COMB12_M4_FL"
                && Cell(ChainRow(chain, "PIC_COMB12_M4_FL"), "colChainRemark") == "FH-FL 자동 묶음" && Cell(ChainRow(chain, "PIC_COMB12_M4_FH"), "colChainSetup") == "PIC_COMB12_M4",
                Cell(ChainRow(chain, "PIC_COMB12_M4_FH"), "colChainGroup"));
            Check("짝 없는 Chain 은 그대로 (TSVDC_L_N_FH 등 _FH 만 있는 Chain)", names.Where(n => n.EndsWith("_FH") && !names.Contains(n.Substring(0, n.Length - 3) + "_FL"))
                .All(n => Cell(ChainRow(chain, n), "colChainGroup") == ""), "");

            chain.ClearSelection();
            ChainRow(chain, "TSVDC_ASIC1_P_FH").Selected = true;
            ChainRow(chain, "PIC_COMB12_M4_FH").Selected = true;
            Application.DoEvents();
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnChainGroup_Click");
            Check("이미 묶인 Chain 옮기기 질문 -> 새 묶음 {PIC_COMB12_M4_FH, TSVDC_ASIC1_P_FH}", form.LastQuestion.Contains("이미 묶인 Chain 2개")
                && Cell(ChainRow(chain, "TSVDC_ASIC1_P_FH"), "colChainGroup") == "PIC_COMB12_M4_FH + TSVDC_ASIC1_P_FH", Cell(ChainRow(chain, "TSVDC_ASIC1_P_FH"), "colChainGroup"));
            Check("1개만 남은 이전 묶음은 풀림 (TSVDC_ASIC1_P_FL / PIC_COMB12_M4_FL)", Cell(ChainRow(chain, "TSVDC_ASIC1_P_FL"), "colChainGroup") == "" && Cell(ChainRow(chain, "PIC_COMB12_M4_FL"), "colChainGroup") == "",
                Cell(ChainRow(chain, "TSVDC_ASIC1_P_FL"), "colChainGroup") + " / " + Cell(ChainRow(chain, "PIC_COMB12_M4_FL"), "colChainGroup"));

            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                tabMain.SelectedTab = (TabPage)Get(type, form, "tabLookup");
                cboSetup.Text = "TSVDC_ASIC1_P_FL";
                Invoke(type, form, "btnLookup_Click");
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[1]);
                tabMain.SelectedTab = (TabPage)Get(type, form, "tabChain");
                Application.DoEvents();
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(Path.ChangeExtension(args[1], null) + "_chain.png");
            }

            chain.ClearSelection();
            ChainRow(chain, "TSVDC_ASIC1_P_FH").Selected = true;
            Application.DoEvents();
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnChainUngroup_Click");
            Check("선택 묶음 해제 -> TSVDC_ASIC1_P_FH / PIC_COMB12_M4_FH 모두 풀림", status.Text == "묶음 해제 완료 / 1개" && Cell(ChainRow(chain, "TSVDC_ASIC1_P_FH"), "colChainGroup") == ""
                && Cell(ChainRow(chain, "PIC_COMB12_M4_FH"), "colChainGroup") == "" && Cell(Row(grid, ResAsic1P), "colGroup") == "", status.Text);
            Check("서비스 거부: 묶음에 Chain 1개", Throws(() => bsl.CreateEtChainGroups(new[] { Factory, Device }, new[,] { { "1", "A_FH", "" } }, "SMOKE")).Contains("2개 이상"), "");
            Check("서비스 거부: Chain 에 ':'", Throws(() => bsl.CreateEtChainGroups(new[] { Factory, Device }, new[,] { { "1", "A_FH:B_FL", "" }, { "1", "B_FL", "" } }, "SMOKE")).Contains("':'"), "");
            Check("서비스 거부: 같은 Chain 두 번(대소문자만 다름)", Throws(() => bsl.CreateEtChainGroups(new[] { Factory, Device }, new[,] { { "1", "a_fh", "" }, { "1", "A_FH", "" } }, "SMOKE")).Contains("두 번"), "");
            Check("서비스: 소문자 Chain -> 대문자 / 측정 항목에 없는 Chain 도 묶음", Throws(() => bsl.CreateEtChainGroups(new[] { Factory, Device }, new[,] { { "x", "smoke_only_fh", "" }, { "x", "smoke_only_fl", "" } }, "SMOKE")) == ""
                && bsl.GetEtChainMatch(Factory, Device, "smoke_only").Rows.Count == 2, "");
            form.Close();
        }

        if (!keep)
        {
            Cleanup(bsl, seq);
        }
        else Console.WriteLine("KEEP: 파일 번호 " + seq + " / Spec / Chain 묶음 유지");

        if (failures == 0) { Console.WriteLine("ET_LIMIT_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_LIMIT_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>Smoke Device 의 파일(Slot 4) / 모든 공정 Spec / Chain 묶음을 지운다.</summary>
    private static void Cleanup(DACrux.SEMDMS.BSL.GdsEtResult bsl, string seq)
    {
        foreach (DataRow dr in bsl.GetEtTestProgramList(Factory, Device).Rows)
        {
            string program = dr["TEST_PROGRAM"].ToString();
            foreach (DataRow limit in bsl.GetEtLimit(Factory, Device, program).Rows)
                bsl.DeleteEtLimit(new[] { Factory, Device, program, limit["ITEM_NAME"].ToString() }, "SMOKE");
        }
        var groups = new HashSet<string>();
        foreach (DataRow dr in bsl.GetEtChainGroupList(Factory, Device).Rows)
            if (dr["GROUP_SEQ"] != DBNull.Value && groups.Add(dr["GROUP_SEQ"].ToString()))
                bsl.DeleteEtChainGroup(new[] { Factory, Device, dr["CHAIN_NAME"].ToString() }, "SMOKE");
        foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, "RH-HS2615", "4" }).Rows)
            bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
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

    private static DataGridViewRow Row(DataGridView grid, string item)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colItem") == item) return r;
        throw new InvalidOperationException("Parameter 행 없음: " + item);
    }

    /// <summary>찾기 표에서 정의(Y) / 정의 없음(N) 행 수</summary>
    private static int LookupRows(DataGridView grid, string defined)
    {
        return grid.Rows.Cast<DataGridViewRow>().Count(r => Cell(r, "colLkDefined") == defined);
    }

    private static DataGridViewRow ChainRow(DataGridView grid, string name)
    {
        foreach (DataGridViewRow r in grid.Rows) if (Cell(r, "colChainName") == name) return r;
        throw new InvalidOperationException("Chain 행 없음: " + name);
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

    private static void CallArgs(Type type, object instance, string method, object[] values)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, values);
        Application.DoEvents();
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 창 / 클립보드만 바꾼다.</summary>
    private sealed class PreviewFixture : DACrux.SEMDMS.ENGUI.frmEtLimitSetup
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public string Clipboard = "";

        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }

        protected override string ClipboardText()
        {
            return Clipboard;
        }

        /// <summary>GDS 데이터 없이 Setup Chain 목록을 고정한다(같은 이름 1개 / _FH·_FL 로 찾는 이름 1개 / 없는 이름 1개).</summary>
        public List<string> SetupChains = new List<string> { "NO_SUCH_CHAIN", "PIC_COMB12_M4", "TSVDC_ASIC1_P_FL" };
        public int AskCount;

        protected override List<string> LoadSetupChains(string factory, string device, out string source)
        {
            source = "Smoke 고정 " + SetupChains.Count + "개";
            return new List<string>(SetupChains);
        }

        protected override DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            AskCount++;
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
