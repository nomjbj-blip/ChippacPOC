using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// GDS x KLARF 연관 분석 화면(화면 A, frmGdsKlarfAnalysis)과 Die 상세 화면(화면 B, NexplantQMS.GdsMap.GdsDieAnalysisForm)을
/// 화면 밖에서 실제로 띄워 Wafer 그리기 -> [분석] -> 구간 색 / 목록 -> Die 선택 -> 상세(도면 + Chain + Defect) 흐름을 검사한다.
/// Service(Remoting) 대신 RO 의 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꿔 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다. DB 는 조회만 한다.
/// 인자: [0] 화면 A 이미지(png), [1] STEP_SEQ, [2] Factory, [3] GDS Device
/// </summary>
internal static class GdsKlarfAnalysisFormSmoke
{
    private static int failures;
    private static int checks;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        string png = args[0];
        long stepSeq = long.Parse(args[1]);
        string factory = args[2];
        string device = args[3];
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";

        var defectBsl = new DACrux.SEMDMS.BSL.DefectMapAnalysis();
        var viewBsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var chainBsl = new DACrux.SEMDMS.BSL.GdsChain();
        var analysisBsl = new DACrux.SEMDMS.BSL.GdsKlarfAnalysis();
        // 기준 = 통합 분석 서버 Analyze 를 화면 기본 조건(Chain 면 BS / KLARF 검사 면 FS 고정 / 크기 + 허용오차 5 µm)으로 직접 부른 결과
        // (2026-10-16, 2026-10-20 Chain 면 기본 BS · KLARF 검사 면 FS 고정 -> 기본 조건은 반전)
        DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest baseRequest = new DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest
        {
            Factory = factory, DeviceId = device, StepSeqs = new[] { stepSeq }, ViewSides = new[] { "FRONT" }, ChainSide = "BACK", DisplayStepSeq = stepSeq,
            AreaMode = DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest.AreaSizeTolerance, ToleranceUm = 5,
            ClassBasis = DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest.ClassBasisRoughBin
        };
        DataSet expected = analysisBsl.Analyze(baseRequest);
        DataRow expectedInfo = expected.Tables["ANL_INFO"].Rows[0];
        int expectedInChain = Convert.ToInt32(expectedInfo["IN_CHAIN_DEFECT_COUNT"]);
        DataRow topDie = expected.Tables["DIE"].Rows[0];
        int topX = Convert.ToInt32(topDie["XINDEX"]), topY = Convert.ToInt32(topDie["YINDEX"]), topN = Convert.ToInt32(topDie["IN_CHAIN_COUNT"]);
        Console.WriteLine("BSL 기준: Defect " + expectedInfo["DEFECT_COUNT"] + " / Chain 영역 " + expectedInChain + " / 첫 Die (" + topX + ", " + topY + ") N=" + topN);

        DataSet result;
        int dieChains = expected.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY);
        Type typeA = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        using (var form = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis())
        {
            form.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(defectBsl);
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            form.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.ShowErrorDialogs = false;
            form.Show();
            Application.DoEvents();

            // ---- 1) Wafer 받기 (왼쪽 DMSearch [Search] 와 같은 입구). 2026-10-16: Wafer 콤보 없음, 1장만 ----
            var wafer = new DACrux.Base.DPWafer();
            wafer.StepSeq = stepSeq.ToString();
            wafer.WaferID = expectedInfo["WAFER_ID"].ToString();
            wafer.LotID = expectedInfo["LOT_ID"].ToString();
            wafer.Product = expectedInfo["PRODUCT"].ToString();
            form.DrawWafer(new[] { wafer });
            Pump(1500);
            var map = (DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap)Get(typeA, form, "map");
            var lblDeviceValue = (Label)Get(typeA, form, "lblDeviceValue");
            var btnChain = (Button)Get(typeA, form, "btnChain");
            var btnClass = (Button)Get(typeA, form, "btnClass");
            var status = (Label)Get(typeA, form, "lblStatus");
            Check("Wafer Map Defect 수", map.Defects.Count == Convert.ToInt32(expectedInfo["DEFECT_COUNT"]), map.Defects.Count.ToString());
            Check("Map Die 격자", map.Dies.Count > 0, "Die " + map.Dies.Count);
            if (form.GdsDevice != device) throw new InvalidOperationException("Product -> GDS Device 가 " + device + " 가 아님: " + form.GdsDevice);
            Check("GDS Device = Wafer Product(읽기 전용) / Chain 메뉴(READY 전체)", lblDeviceValue.Text == device && btnChain.Text.StartsWith("Chain: 전체("),
                lblDeviceValue.Text + " / " + btnChain.Text);
            Check("분류 메뉴(기본 RoughBin, 전체)", btnClass.Text.StartsWith("RoughBin: 전체("), btnClass.Text);
            var hidden = new[] { "cboClassBasis", "btnClass", "btnChain", "chkDieIndex", "chkLargeDefect", "cboColorBasis" }
                .Where(n => ((Control)Get(typeA, form, n)).Visible).ToArray();
            Check("숨김: 분류 기준 / 분류 / Chain 거르기 / Die Index / 큰 Defect 제외 / 색 기준", hidden.Length == 0 && typeA.GetField("cboWafer", BindingFlags.NonPublic | BindingFlags.Instance) == null
                && typeA.GetField("cboDevice", BindingFlags.NonPublic | BindingFlags.Instance) == null, hidden.Length == 0 ? "콤보 없음" : string.Join(", ", hidden));

            // Wafer 2개(Step 2개) 선택 -> 오류, 지금 화면 그대로
            var other2 = new DACrux.Base.DPWafer();
            other2.StepSeq = (stepSeq + 100000).ToString();
            other2.WaferID = "OTHER";
            form.DrawWafer(new[] { wafer, other2 });
            Application.DoEvents();
            Check("Wafer 여러 개 선택 -> 오류(화면 그대로, 문구 = 1개 Wafer 만 분석)", status.Text.StartsWith("1개 Wafer 만 분석할 수 있습니다.") && status.Text.Contains("(선택 2개)")
                && !status.Text.Contains("[준비]") && map.Defects.Count == Convert.ToInt32(expectedInfo["DEFECT_COUNT"]),
                status.Text);
            form.DrawWafer(new[] { wafer });
            Pump(1500);
            Check("분석 전 상태 줄 = 결과만(사용법 안내 없음)", status.Text.Contains(" / Defect ") && !status.Text.Contains("["), status.Text);
            // [준비] 탭 = 서버 Ready 조회(GetWaferReady) 결과 (2026-10-16)
            DataSet ready = form.ReadyData;
            DataRow readyKey = ready.Tables["WAFER_KEY"].Rows[0];
            bool readyStep = ready.Tables["READY_KLARF"].Rows.Cast<DataRow>().Any(r => r["STEP_SEQ"].ToString() == stepSeq.ToString());
            string[] readyCodes = ready.Tables["READY_CHECK"].Rows.Cast<DataRow>().Select(r => r["CODE"].ToString()).ToArray();
            Check("[준비] 탭 = 서버 Ready 조회(Wafer Key / GDS / 고른 Step / 점검, 서버 실패 행 없음)",
                readyKey["WAFER_KEY"].ToString().Length > 0 && ready.Tables["READY_GDS"].Rows[0]["DEVICE_ID"].ToString() == device && readyStep
                && !readyCodes.Contains("READY_LOCAL") && readyCodes.Contains("RULE_TENTATIVE"),
                readyKey["WAFER_KEY"] + " / " + ready.Tables["READY_GDS"].Rows[0]["DEVICE_ID"] + " / Step " + ready.Tables["READY_KLARF"].Rows.Count + " / " + string.Join(",", readyCodes));
            // [E-Test] 탭 Step 콤보 (2026-10-20): 서버가 같은 Wafer Key + 분석 GDS Device 파일만 주고, 화면은 Step 을 골라 그 Step 파일만 보인다.
            // Device 가 달라 뺀 파일이 있으면 상태줄에 결과로만 알린다. 실제 데이터는 Wafer 마다 파일이 0 ~ 1개라 파일 3개(TSVET 2 / COWET 1) 준비 목록을 넣어 본다
            {
                BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
                var dgvEt = (DataGridView)Get(typeA, form, "dgvReadyEt");
                var colUse = (DataGridViewColumn)Get(typeA, form, "colEtUse");
                var colSeq = (DataGridViewColumn)Get(typeA, form, "colEtSeq");
                var cboEtStep = (ComboBox)Get(typeA, form, "cboEtStep");
                MethodInfo etFile = typeA.GetMethod("ReadyEtFile", f);
                Func<string> shown = () => String.Join(",", dgvEt.Rows.Cast<DataGridViewRow>()
                    .Select(r => r.Cells[colSeq.Index].Value + (true.Equals(r.Cells[colUse.Index].Value) ? "*" : "")).ToArray());
                DataRow otherDevice = ready.Tables["READY_CHECK"].Rows.Cast<DataRow>().FirstOrDefault(r => r["CODE"].ToString() == "ET_OTHER_DEVICE");
                bool deviceOnly = ready.Tables["READY_ET"].Rows.Cast<DataRow>().All(r => String.Equals(r["DEVICE_ID"].ToString(), device, StringComparison.OrdinalIgnoreCase));
                Check("E-Test 목록 = 분석 GDS Device 파일만 / Device 다른 파일은 상태줄 결과(사용법 안내 없음)",
                    deviceOnly && (otherDevice == null || (status.Text.Contains(otherDevice["MESSAGE"].ToString()) && !status.Text.Contains("["))),
                    "E-Test " + ready.Tables["READY_ET"].Rows.Count + "개 / " + (otherDevice == null ? "Device 다른 파일 없음" : status.Text));

                DataSet fake = ready.Copy();
                DataTable fakeEt = fake.Tables["READY_ET"];
                fakeEt.Rows.Clear();
                fakeEt.Rows.Add("9001", "TSVET", "W-01", device, "2026-10-02 16:18:46", "76", "2", "2026-10-19 10:00:00", "N", "4", "", "Y", "RULE", "Y");
                fakeEt.Rows.Add("9002", "COWET", "W-01", device, "2026-10-01 10:00:00", "76", "1", "2026-10-19 10:00:00", "N", "4", "", "Y", "RULE", "N");
                fakeEt.Rows.Add("9003", "TSVET", "W-01", device, "2026-09-30 10:00:00", "76", "0", "2026-10-19 10:00:00", "Y", "4", "", "Y", "RULE", "N");
                form.ShowReady(fake);
                Application.DoEvents();
                string items = String.Join(",", cboEtStep.Items.Cast<object>().Select(i => i.ToString()).ToArray());
                string first = cboEtStep.Text + " [" + shown() + "] " + etFile.Invoke(form, null);
                cboEtStep.SelectedIndex = 1;
                Application.DoEvents();
                string cowet = cboEtStep.Text + " [" + shown() + "] " + etFile.Invoke(form, null);
                cboEtStep.SelectedIndex = 0;
                Application.DoEvents();
                string back = cboEtStep.Text + " [" + shown() + "] " + etFile.Invoke(form, null);
                dgvEt.Rows[1].Cells[colUse.Index].Value = true;
                Application.DoEvents();
                string older = "[" + shown() + "] " + etFile.Invoke(form, null);
                Check("E-Test Step 콤보: Step = TSVET, COWET(최신 순) / 고른 Step 파일만 · 체크 1개(최신) / Step 바꾸면 그 Step 최신 / Step 안 다른 파일 체크 = 1개만",
                    cboEtStep.Enabled && items == "TSVET,COWET" && first == "TSVET [9001*,9003] 9001" && cowet == "COWET [9002*] 9002"
                    && back == "TSVET [9001*,9003] 9001" && older == "[9001,9003*] 9003"
                    && dgvEt.Columns[1].HeaderText == "Step" && ((Label)Get(typeA, form, "lblEtStep")).Text == "Step",
                    items + " | " + first + " | " + cowet + " | " + back + " | " + older + " | 열 " + dgvEt.Columns[1].HeaderText);
                fakeEt.Rows.Clear();
                form.ShowReady(fake);
                Application.DoEvents();
                // 제목(굵게) · Step 라벨 · 콤보 · [직접 찾기]가 한 줄에서 겹치지 않음 (미리보기에서 제목 끝이 Step 라벨에 가려진 것을 보고 추가)
                var lblTitle = (Label)Get(typeA, form, "lblReadyEt");
                var lblStep = (Label)Get(typeA, form, "lblEtStep");
                var btnFind = (Button)Get(typeA, form, "btnFindEt");
                Check("E-Test 머리 줄 배치: 제목 < Step 라벨 < 콤보 < [직접 찾기] (겹침 없음)",
                    lblStep.Left >= lblTitle.Right && cboEtStep.Left >= lblStep.Right && cboEtStep.Right <= btnFind.Left,
                    String.Format("제목 {0}~{1} / Step {2}~{3} / 콤보 {4}~{5} / 찾기 {6}", lblTitle.Left, lblTitle.Right, lblStep.Left, lblStep.Right, cboEtStep.Left, cboEtStep.Right, btnFind.Left));
                string noEt = cboEtStep.Enabled + " / Step " + cboEtStep.Items.Count + " / 행 " + dgvEt.Rows.Count + " / 파일 " + etFile.Invoke(form, null);
                Check("E-Test 파일 없음: Step 콤보 꺼짐 / 표 0행 / 분석 E-Test 없음(0)", noEt == "False / Step 0 / 행 0 / 파일 0", noEt);
                form.ShowReady(ready);
                Application.DoEvents();
            }
            // 로컬 DB 에 이 Wafer 의 실제 E-Test 파일이 있으면(2026-10-19 업로드) [E-Test] 표가 최신 공정을 자동 선택한다.
            // 아래 검사는 E-Test 없는 결과(기준 = baseRequest)를 전제로 하므로 선택을 끈다. E-Test 연결은 BSL/Probes/GdsKlarfEtProbe 가 시험 데이터로 확인한다 (2026-10-20)
            {
                var dgvReadyEt = (DataGridView)Get(typeA, form, "dgvReadyEt");
                var colEtUse = (DataGridViewColumn)Get(typeA, form, "colEtUse");
                int etRows = dgvReadyEt.Rows.Count;
                foreach (DataGridViewRow r in dgvReadyEt.Rows) r.Cells[colEtUse.Index].Value = false;
                Application.DoEvents();
                Console.WriteLine("[E-Test] 표 파일 " + etRows + "개 - 이 Smoke 는 E-Test 선택을 끄고 분석");
            }
            // KLARF 탭 (2026-10-20 Wafer 1개만 분석): Step 표 / KLARF [직접 찾기] 없음, 고른 Step 정보 + Pitch 출처,
            // 검사 면 콤보 없음(KLARF 검사 면 FS 고정, Ready VIEW_SIDE = FRONT), GDS / KLARF / E-Test 탭 위쪽 제목 = "Wafer 1개만 분석"
            {
                BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
                var stepInfo = (Label)Get(typeA, form, "lblStepInfo");
                var pitch = (Label)Get(typeA, form, "lblPitchSource");
                DataRow pickedStep = ready.Tables["READY_KLARF"].Rows.Cast<DataRow>().First(r => r["STEP_SEQ"].ToString() == stepSeq.ToString());
                string[] heads = { ((Label)Get(typeA, form, "lblReadyGds")).Text, ((Label)Get(typeA, form, "lblReadyStep")).Text, ((Label)Get(typeA, form, "lblReadyEt")).Text };
                bool gone = typeA.GetField("dgvReadyStep", f) == null && typeA.GetField("btnFindStep", f) == null && typeA.GetMethod("btnFindStep_Click", f) == null
                    && typeA.GetField("cboStepSide", f) == null && typeA.GetField("lblStepSide", f) == null;
                Check("KLARF 탭: Step 표 · [직접 찾기] · 검사 면 콤보 없음 / 고른 Step 정보 / Ready 검사 면 FRONT / Pitch 출처 / 탭 제목 3개 = Wafer 1개만 분석",
                    gone && stepInfo.Text.StartsWith("Step " + pickedStep["STEP_ID"] + "  /  Lot " + pickedStep["LOT_ID"]) && stepInfo.Text.Contains("Defect " + pickedStep["DEFECTS"])
                    && !stepInfo.Text.Contains("검사 면") && pickedStep["VIEW_SIDE"].ToString() == "FRONT" && pitch.Text == "Pitch 출처: Map Setup"
                    && heads.All(h => h.Contains("Wafer 1개만 분석")) && !heads.Any(h => h.Contains("같은 Wafer")),
                    stepInfo.Text.Replace(Environment.NewLine, " | ") + " / Ready 검사 면 " + pickedStep["VIEW_SIDE"] + " / " + pitch.Text + " / " + String.Join(" | ", heads));
            }
            // 화면 배치 (2026-10-19 안 1): 기능 탭 GDS / KLARF / E-Test / 종합(Die) + 결과 하위 탭, [준비] 탭 · 점검 표 없음(기준은 기능 탭 위쪽),
            // 색 구간 표는 팝업(화면에 없음), Wafer 를 받으면 [KLARF] 탭. "지금 기준" 요약 줄 / Map 범례는 2026-10-20 삭제, Chain 면 기본 BS(서버 Ready 도 BACK)
            var tabMain = (TabControl)Get(typeA, form, "tabList");
            string[] mainTabs = tabMain.TabPages.Cast<TabPage>().Select(p => p.Text).ToArray();
            Func<string, string[]> subTabs = n => ((TabControl)Get(typeA, form, n)).TabPages.Cast<TabPage>().Select(p => p.Name).ToArray();
            var pnlBandHost = (Control)Get(typeA, form, "pnlBand");
            var cboSide = (ComboBox)Get(typeA, form, "cboChainSide");
            BindingFlags hiddenField = BindingFlags.NonPublic | BindingFlags.Instance;
            bool summaryGone = new[] { "lblSumTitle", "lblSumGds", "lblSumKlarf", "lblSumEt", "flowLegend" }.All(n => typeA.GetField(n, hiddenField) == null)
                && typeA.GetMethod("UpdateCriteriaSummary", hiddenField) == null && typeA.GetMethod("RefreshLegend", hiddenField) == null;
            Check("화면 배치: 기능 탭 GDS / KLARF / E-Test / 종합 (Die) · 결과 하위 탭 / 준비 탭 · 점검 표 없음 / 색 구간 표 = 팝업 / 지금 기준 요약 · Map 범례 없음 / Chain 면 기본 BS / Wafer 받으면 KLARF 탭",
                mainTabs.SequenceEqual(new[] { "GDS", "KLARF", "E-Test", "종합 (Die)" })
                && subTabs("tabGdsResult").SequenceEqual(new[] { "tabChain" }) && subTabs("tabKlarfResult").SequenceEqual(new[] { "tabDefect", "tabTolerance", "tabImage" })
                && subTabs("tabEtResult").SequenceEqual(new[] { "tabEtFail", "tabEtLink" })
                && typeA.GetField("tabReady", hiddenField) == null && typeA.GetField("dgvReadyCheck", hiddenField) == null && typeA.GetField("lblReadyCheck", hiddenField) == null
                && !pnlBandHost.Visible && pnlBandHost.Parent == form && tabMain.SelectedTab.Text == "KLARF"
                && summaryGone && cboSide.SelectedIndex == 1 && Convert.ToString(cboSide.SelectedItem) == "BS"
                && ready.Tables["READY_GDS"].Rows[0]["CHAIN_SIDE"].ToString() == "BACK",
                String.Join(",", mainTabs) + " / " + tabMain.SelectedTab.Text + " / 요약 · 범례 없음 " + summaryGone + " / Chain 면 " + cboSide.SelectedItem
                + " (Ready " + ready.Tables["READY_GDS"].Rows[0]["CHAIN_SIDE"] + ")");

            // KLARF Product -> 이름이 같은(대소문자 무시) GDS Device, 없으면 Device 없음 + 분석 불가 (2026-10-16)
            FieldInfo productField = typeA.GetField("_product", BindingFlags.NonPublic | BindingFlags.Instance);
            string product = (string)productField.GetValue(form);
            MethodInfo selectDevice = typeA.GetMethod("SelectDeviceForProduct", BindingFlags.NonPublic | BindingFlags.Instance);
            productField.SetValue(form, device.ToLowerInvariant());
            string sameNote = (string)selectDevice.Invoke(form, null);
            string sameDevice = form.GdsDevice;
            productField.SetValue(form, "ZZ_NO_SUCH_PRODUCT");
            string noneNote = (string)selectDevice.Invoke(form, null);
            string noneDevice = form.GdsDevice, noneLabel = lblDeviceValue.Text;
            string noneError = "";
            try { form.AnalyzeNow(); } catch (InvalidOperationException ex) { noneError = ex.Message; }
            Check("Product 같은 이름(대소문자 무시) -> GDS Device / 없는 Product -> Device 없음 + 분석 불가",
                sameDevice == device && sameNote.Length == 0 && noneDevice.Length == 0 && noneLabel == "없음 - 분석 불가"
                && noneNote.EndsWith("READY GDS Map 없음 (분석 불가)") && noneError.Contains("분석할 수 없습니다"),
                sameDevice + " / " + noneNote + " / " + noneError.Split('\n')[0]);
            productField.SetValue(form, product);
            selectDevice.Invoke(form, null);
            Application.DoEvents();

            // ---- 2) [분석] (Back side / 5 µm / 크기 반영 = 기본값) ----
            result = form.AnalyzeNow();
            DataRow info = result.Tables["ANL_INFO"].Rows[0];
            Check("분석 결과 = BSL 직접 호출 (기본 Chain BS · KLARF 검사 FS 고정 = 반전, 요청 Step 1개)", Convert.ToInt32(info["IN_CHAIN_DEFECT_COUNT"]) == expectedInChain
                && info["VIEW_SIDE"].ToString() == expectedInfo["VIEW_SIDE"].ToString() && info["MIRROR"].ToString() == "Y" && info["CHAIN_SIDE"].ToString() == "BACK"
                && info["STEP_SIDES"].ToString() == "FRONT" && info["STEP_SEQS"].ToString() == stepSeq.ToString()
                && Convert.ToDouble(info["TOLERANCE_UM"]) == 5, "Chain 영역 " + info["IN_CHAIN_DEFECT_COUNT"] + " / " + info["VIEW_SIDE"] + " / 반전 " + info["MIRROR"]
                + " / 검사 " + info["STEP_SIDES"]);
            Dictionary<Point, Color> fills = (Dictionary<Point, Color>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieFill");
            Dictionary<Point, int> labels = (Dictionary<Point, int>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieLabel");
            List<DACrux.SEMDMS.ENGUI.GdsKlarfColorBand> bands = form.ColorBands;
            Color topColor = DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.DieColor(Convert.ToInt32(topDie["DEFECT_COUNT"]), topN, 1, bands);
            Check("자동 구간 = Auto(1, 최대 N)", bands.Count == Math.Min(5, Math.Max(1, topN)) && bands[0].From == 1 && bands[bands.Count - 1].To == 999,
                string.Join(" / ", bands.Select(b => b.From + "-" + b.To).ToArray()));
            Check("Die 채움색 = 검사 Die 전체 / 숫자 = Defect Die", fills.Count == result.Tables["DIE"].Rows.Count && labels.Count == Convert.ToInt32(info["DEFECT_DIE_COUNT"]),
                "fill " + fills.Count + " / label " + labels.Count);
            Check("최다 Die 색 = 마지막 구간 색", fills[new Point(topX, topY)].ToArgb() == bands[bands.Count - 1].Color.ToArgb() && topColor.ToArgb() == bands[bands.Count - 1].Color.ToArgb(),
                ColorTranslator.ToHtml(fills[new Point(topX, topY)]));
            DataRow outOnly = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && Convert.ToInt32(r["IN_CHAIN_COUNT"]) == 0);
            DataRow empty = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) == 0);
            if (outOnly != null)
                Check("Chain 밖 Defect 만 있는 Die = 연회색", fills[DiePoint(outOnly)].ToArgb() == DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.BelowThresholdColor.ToArgb(), DiePoint(outOnly).ToString());
            if (empty != null)
                Check("Defect 없는 Die = 흰색", fills[DiePoint(empty)].ToArgb() == Color.White.ToArgb(), DiePoint(empty).ToString());
            var dgvBand = (DataGridView)Get(typeA, form, "dgvBand");
            var dgvDie = (DataGridView)Get(typeA, form, "dgvDie");
            int bandDies = 0;
            foreach (DataGridViewRow r in dgvBand.Rows) if (!"FAIL_MARK".Equals(r.Tag)) bandDies += Convert.ToInt32(r.Cells[3].Value);
            Check("구간 표 = 구간 + 고정 2행 / Die 수 합 = 결과 Die 수", dgvBand.Rows.Count == bands.Count + 2 && bandDies == result.Tables["DIE"].Rows.Count,
                "rows " + dgvBand.Rows.Count + " / dies " + bandDies);
            // [분석] 뒤 (2026-10-19 결정 L4) 처음 결과면 [종합 (Die)] 탭. Map 범례는 없고(2026-10-20) 색 구간 / Die 수는 [색 구간 설정] 팝업 표에서만 본다
            var mapHost = splitMainPanel(form, typeA);
            bool noLegend = mapHost.Controls.OfType<FlowLayoutPanel>().All(p => !p.Visible);
            Check("[분석] 뒤 종합 (Die) 탭 / Map 범례 없음(색 구간은 팝업 표)", tabMain.SelectedTab.Text == "종합 (Die)" && noLegend && dgvBand.Rows.Count > 0,
                tabMain.SelectedTab.Text + " / Map 위 범례 패널 " + mapHost.Controls.OfType<FlowLayoutPanel>().Count(p => p.Visible) + " / 팝업 표 " + dgvBand.Rows.Count + "행");
            // 기준 접기 / 펴기, 결과 탭 열기(기능 탭 + 하위 탭)
            {
                var btnKlarfFold = (Button)Get(typeA, form, "btnKlarfFold");
                var pnlKlarfCriteria = (Panel)Get(typeA, form, "pnlKlarfCriteria");
                form.ShowResultTab((TabPage)Get(typeA, form, "tabDefect"));
                Application.DoEvents();
                bool openBefore = pnlKlarfCriteria.Visible;
                Invoke(typeA, form, "CriteriaFold_Click", btnKlarfFold, EventArgs.Empty);
                Application.DoEvents();
                bool folded = !pnlKlarfCriteria.Visible && btnKlarfFold.Text == "기준 펴기";
                Invoke(typeA, form, "CriteriaFold_Click", btnKlarfFold, EventArgs.Empty);
                Application.DoEvents();
                bool reopened = pnlKlarfCriteria.Visible && btnKlarfFold.Text == "기준 접기";
                form.ShowResultTab((TabPage)Get(typeA, form, "tabEtFail"));
                bool toEtFail = tabMain.SelectedTab.Text == "E-Test" && ((TabControl)Get(typeA, form, "tabEtResult")).SelectedTab == (TabPage)Get(typeA, form, "tabEtFail");
                Check("KLARF 기준 접기 -> 펴기 / 결과 탭 열기(E-Test Fail -> E-Test 기능 탭)", openBefore && folded && reopened && toEtFail,
                    "펼침 " + openBefore + " / 접힘 " + folded + " / 다시 펼침 " + reopened + " / E-Test Fail " + toEtFail);
                form.ShowResultTab((TabPage)Get(typeA, form, "tabDie"));
            }
            // E-Test 표식 (2026-10-16, 2026-10-20 두 가지): E-Test 판정이 없으면 표식 / 구간 표 행 없음. 있으면 켠 표식마다 구간 표 끝 1행, Die 수 = Map 표식 수.
            // Spec Out = DMS Spec 판정(ET_STATE) F -> 왼쪽 위 빨강, Fail Die = 결과 파일 DUT Flag(ET_FILE_FLAG) F -> 오른쪽 아래 남색. [E-Test] 탭 체크 2개(기본 둘 다 켬).
            // 이 Smoke 데이터는 E-Test 가 없어 Spec Out 2개(Die 0, 1) / 파일 Fail 3개(Die 1, 2, 3)를 넣은 복사본으로 확인한 뒤 원래 결과로 되돌린다.
            {
                var chkSpec = (CheckBox)Get(typeA, form, "chkMarkSpec");
                var chkFile = (CheckBox)Get(typeA, form, "chkMarkFile");
                bool checkOk = ((Label)Get(typeA, form, "lblFailMark")).Text == "Map E-Test 표식" && chkSpec.Text == "Spec Out" && chkFile.Text == "Fail Die (결과 파일)"
                    && chkSpec.Checked && chkFile.Checked && chkSpec.Image != null && chkFile.Image != null;
                bool noMarkRow = dgvBand.Rows.Cast<DataGridViewRow>().All(r => !"FAIL_MARK".Equals(r.Tag));
                bool serverFileFlag = result.Tables["DIE"].Columns.Contains("ET_FILE_FLAG");
                DataSet etCopy = result.Copy();
                DataTable etDieTable = etCopy.Tables["DIE"];
                if (!etDieTable.Columns.Contains("ET_FILE_FLAG")) etDieTable.Columns.Add("ET_FILE_FLAG", typeof(string));
                DataRow[] etDies = etDieTable.Rows.Cast<DataRow>().ToArray();
                for (int i = 0; i < etDies.Length; i++)
                {
                    etDies[i]["ET_STATE"] = i < 2 ? "F" : "P";
                    etDies[i]["ET_FILE_FLAG"] = i >= 1 && i <= 3 ? "F" : "P";
                }
                var specDies = new HashSet<Point>(etDies.Take(2).Select(DiePoint));
                var fileDies = new HashSet<Point>(etDies.Skip(1).Take(3).Select(DiePoint));
                Func<string> markRows = () => String.Join(" | ", dgvBand.Rows.Cast<DataGridViewRow>().Where(r => "FAIL_MARK".Equals(r.Tag))
                    .Select(r => Convert.ToString(r.Cells[0].Value) + " " + Convert.ToString(r.Cells[3].Value)).ToArray());
                var specSet = (HashSet<Point>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_specDies");
                var fileSet = (HashSet<Point>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_fileDies");
                string bothRows = "Spec Out Die " + specDies.Count + " | Fail Die (결과 파일) " + fileDies.Count;
                form.ShowAnalysisResult(etCopy);
                string both = markRows();
                bool bothOk = both == bothRows && specSet.SetEquals(specDies) && fileSet.SetEquals(fileDies);
                int otherDies = dgvBand.Rows.Cast<DataGridViewRow>().Where(r => !"FAIL_MARK".Equals(r.Tag)).Sum(r => Convert.ToInt32(r.Cells[3].Value));
                chkFile.Checked = false;
                Application.DoEvents();
                string specOnly = markRows();
                bool specOnlyOk = specOnly == "Spec Out Die " + specDies.Count && specSet.SetEquals(specDies) && fileSet.Count == 0;
                chkFile.Checked = true;
                chkSpec.Checked = false;
                Application.DoEvents();
                string fileOnly = markRows();
                bool fileOnlyOk = fileOnly == "Fail Die (결과 파일) " + fileDies.Count && specSet.Count == 0 && fileSet.SetEquals(fileDies);
                chkSpec.Checked = true;
                Application.DoEvents();
                string bandPng = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(png), "band_fail_mark.png");
                // 색 구간 표는 [색 구간 설정] 팝업 안에 있다 (2026-10-19)
                form.ShowBandPopup();
                Application.DoEvents();
                Control bandPanel = dgvBand.Parent;
                Check("[색 구간 설정] 팝업 = 색 구간 표(화면 밖 별도 창, 보임)", bandPanel.FindForm() != form && bandPanel.FindForm().Visible && dgvBand.Visible
                    && bandPanel.FindForm().Text == "색 구간 설정", bandPanel.FindForm().Text);
                using (var bandBmp = new Bitmap(bandPanel.Width, bandPanel.Height))
                {
                    bandPanel.DrawToBitmap(bandBmp, new Rectangle(0, 0, bandPanel.Width, bandPanel.Height));
                    bandBmp.Save(bandPng, System.Drawing.Imaging.ImageFormat.Png);
                }
                Console.WriteLine("구간 표 이미지(E-Test 표식 2행): " + bandPng);
                bool backOk = markRows() == bothRows && specSet.SetEquals(specDies) && fileSet.SetEquals(fileDies);
                form.ShowAnalysisResult(result);
                bool restored = map.SpecOutMarkCount == 0 && map.FileFailMarkCount == 0 && dgvBand.Rows.Count == bands.Count + 2;
                Check("E-Test 표식 체크 2개(Spec Out / Fail Die): 판정 없으면 행 없음 / 둘 다 = 끝 2행 · Map 표식 2종 / 하나씩 끄기 / 다시 켜기 / 서버 ET_FILE_FLAG 열 / 원래 결과로 되돌림",
                    checkOk && noMarkRow && serverFileFlag && bothOk && specOnlyOk && fileOnlyOk && backOk && otherDies == etDies.Length && restored,
                    "체크 " + checkOk + " / 둘 다 [" + both + "] / Spec 만 [" + specOnly + "] / Fail 만 [" + fileOnly + "] / 되돌림 " + backOk + " / other " + otherDies + " / 서버 열 " + serverFileFlag + " / restored " + restored);
            }
            // Die 탭 열 (2026-10-16): Die -> X / Y 두 열, Defect 전체 / Chain In / Chain Out, 주요 Chain 열 없음
            string[] dieCols = dgvDie.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray();
            Check("Die 탭 = Defect Die, 첫 행 = Chain In 최다 / 열 X · Y · Defect 전체 · Chain In · Chain Out (Die · 주요 Chain 열 없음)",
                dgvDie.Rows.Count == Convert.ToInt32(info["DEFECT_DIE_COUNT"]) && Convert.ToInt32(dgvDie.Rows[0].Cells["Chain In"].Value) == topN
                && dieCols.Take(5).SequenceEqual(new[] { "X", "Y", "Defect 전체", "Chain In", "Chain Out" })
                && !dieCols.Contains("Die") && !dieCols.Contains("주요 Chain") && !dieCols.Contains("전체")
                && Convert.ToInt32(dgvDie.Rows[0].Cells["Defect 전체"].Value) == Convert.ToInt32(dgvDie.Rows[0].Cells["Chain In"].Value) + Convert.ToInt32(dgvDie.Rows[0].Cells["Chain Out"].Value),
                dgvDie.Rows.Count + " / " + String.Join(",", dieCols) + " / 첫 행 (" + dgvDie.Rows[0].Cells["X"].Value + ", " + dgvDie.Rows[0].Cells["Y"].Value + ")");
            // 검사 면(KLARF FS 고정)은 상태줄에도 보이지 않는다 (2026-10-20)
            Check("상태줄 분석 기준(Chain 면 + 반전, 검사 면 안 보임 / 임시 안내 없음)", status.Text.Contains("Chain In " + expectedInChain) && status.Text.Contains(expectedInfo["VIEW_SIDE"].ToString() + " 보기")
                && status.Text.Contains("(Chain BS -> 반전)") && !status.Text.Contains("검사 FS") && !status.Text.Contains("검사 BS")
                && !status.Text.Contains("Map Step 만") && !status.Text.Contains("연결 전"),
                status.Text.Replace(Environment.NewLine, " | "));
            // 상세 화면에 넘길 결과 = 같은 조건 AnalyzeDie (Die 1개, Member 단위 DEFECT_HIT / E-Test 값 표)
            DataSet detailData = form.DetailData(topX, topY, null);
            Check("상세 데이터 = AnalyzeDie(Die 1행 / Defect = 그 Die 전체 / DEFECT_HIT / ET_VALUE)", detailData.Tables["DIE"].Rows.Count == 1
                && detailData.Tables.Contains("DEFECT_HIT") && detailData.Tables.Contains("ET_VALUE")
                && detailData.Tables["DEFECT"].Rows.Count == Convert.ToInt32(topDie["DEFECT_COUNT"]),
                "Defect " + detailData.Tables["DEFECT"].Rows.Count + " / Member 행 " + detailData.Tables["DEFECT_HIT"].Rows.Count);

            // ---- 3) Die 선택 -> Chain / Defect 탭 거르기 ----
            form.SelectDie(topX, topY);
            Application.DoEvents();
            var dgvChain = (DataGridView)Get(typeA, form, "dgvChain");
            var dgvDefect = (DataGridView)Get(typeA, form, "dgvDefect");
            Check("선택 Die 의 Chain 탭 = DIE_CHAIN", dgvChain.Rows.Count == dieChains, dgvChain.Rows.Count + " / " + dieChains);
            Check("선택 Die 의 Defect 탭(Chain In) = N", dgvDefect.Rows.Count == topN, dgvDefect.Rows.Count.ToString());
            // Chain 코드(A, B, AA ..)는 화면에 쓰지 않는다 (2026-10-16): Chain 탭 첫 열 / Defect 탭 Chain 열 = Chain 이름
            var chainNames = new HashSet<string>(result.Tables["CHAIN"].Rows.Cast<DataRow>().Select(r => r["CHAIN_NAME"].ToString()));
            bool chainColNames = dgvChain.Rows.Cast<DataGridViewRow>().All(r => chainNames.Contains(Convert.ToString(r.Cells["Chain"].Value)))
                && !dgvChain.Columns.Contains("이름");
            bool defectColNames = dgvDefect.Rows.Cast<DataGridViewRow>().All(r => Convert.ToString(r.Cells["Chain"].Value)
                .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).All(n => chainNames.Contains(n)));
            Check("Chain 탭 / Defect 탭의 Chain = 이름(코드 없음, 이름 열 따로 없음)", chainColNames && defectColNames && dgvChain.Rows.Count > 0,
                dgvChain.Rows.Count == 0 ? "-" : Convert.ToString(dgvChain.Rows[0].Cells["Chain"].Value) + " / " + (dgvDefect.Rows.Count == 0 ? "-" : Convert.ToString(dgvDefect.Rows[0].Cells["Chain"].Value)));
            Check("Map 선택 Die 빨간 테두리 목록", map.SelectedDies.Count == 1 && map.SelectedDies[0] == new Point(topX, topY), map.SelectedDies.Count.ToString());

            // ---- 3-1) 마우스가 Die 위를 지날 때(OnChangeCurrentDie) 상태줄. DACrux.Base.Die 의 == 연산자가 null 을 처리하지 않아 예외가 났던 경로 ----
            DACrux.Base.Die hoverDie = null;
            foreach (DACrux.Base.Die d in map.Dies) { if (d.IndexX == topX && d.IndexY == topY) { hoverDie = d; break; } }
            string hoverError = "";
            try { Invoke(typeA, form, "map_OnChangeCurrentDie", map, hoverDie); }
            catch (TargetInvocationException ex) { hoverError = ex.InnerException.GetType().Name + ": " + ex.InnerException.Message; }
            Check("마우스 Die 이동 -> 상태줄(예외 없음, Defect 전체 / Chain In / Chain Out)", hoverError == "" && status.Text.Contains("Die (" + topX + ", " + topY + ") Defect 전체")
                && status.Text.Contains(" / Chain In ") && status.Text.Contains(" / Chain Out "), hoverError == "" ? status.Text.Replace(Environment.NewLine, " | ") : hoverError);

            // ---- 3-2) 우클릭 메뉴(2026-10-12 요청, 2026-10-13 Center Cross 추가): Free Zoom / Fit Size / Defect Select / Die Select / Center Cross / Detail Analysis 만 ----
            var ctxm = (System.Windows.Forms.Menu)Get(typeof(DACrux.Map.WaferMap), map, "ctxmWaferMap");
            typeof(ContextMenu).GetMethod("OnPopup", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ctxm, new object[] { EventArgs.Empty });
            string menuText = string.Join(" / ", map.VisibleMenuItems().ToArray());
            Check("우클릭 메뉴 = Free Zoom / Fit Size / - / Center Cross / - / Detail Analysis (Defect / Die Select 없음, 선택 Chip 있으면 사용 가능)",
                menuText == "Free Zoom / Fit Size / - / Center Cross / - / Detail Analysis" && map.DetailMenuEnabled, menuText + " / enabled=" + map.DetailMenuEnabled);

            // ---- 3-3) 우클릭하면 커서 아래 Chip 을 먼저 선택한다(메뉴는 끄고 확인: 실제 메뉴는 모달이라 자동 검사를 멈춘다) ----
            DataRow rightDie = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            if (rightDie != null)
            {
                Point rp = DiePoint(rightDie);
                DACrux.Base.Die target = null;
                foreach (DACrux.Base.Die d in map.Dies) { if (d.IndexX == rp.X && d.IndexY == rp.Y) { target = d; break; } }
                PointF sp = map.ToScreen(target.DieCood.X + target.DieCood.Width / 2d - map.OriginX, target.DieCood.Y + target.DieCood.Height / 2d - map.OriginY);
                map.PopupMenu = false;
                MethodInfo move = typeof(Control).GetMethod("OnMouseMove", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo down = typeof(Control).GetMethod("OnMouseDown", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo up = typeof(Control).GetMethod("OnMouseUp", BindingFlags.NonPublic | BindingFlags.Instance);
                move.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.None, 0, (int)sp.X, (int)sp.Y, 0) });
                down.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.Right, 1, (int)sp.X, (int)sp.Y, 0) });
                up.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.Right, 1, (int)sp.X, (int)sp.Y, 0) });
                map.PopupMenu = true;
                Application.DoEvents();
                Check("우클릭 -> 커서 아래 Chip 선택", map.SelectedDies.Count == 1 && map.SelectedDies[0] == rp, rp + " -> " + (map.SelectedDies.Count > 0 ? map.SelectedDies[0].ToString() : "없음"));
                form.SelectDie(topX, topY);
                Application.DoEvents();
            }

            // ---- 3-4) 상세보기 창은 Die 마다 하나(2026-10-16 요청: 1,1 다음 1,2 를 열면 하나 더) / 같은 Die 는 그 창 / 탭 제목 = 상세보기 (X, Y) ----
            // (2026-10-12 의 "두 개가 뜨던 버그" = ShowForm 이 다른 어셈블리 사본을 만들던 것. 같은 Die 를 다시 열어도 창이 늘지 않아야 한다)
            // MDI 부모 + 같은 화면 A 를 하나 더 만들어 OpenDetail 을 세 번 부른다. 상세 화면의 Shown(GDS EXE 로드)은 메시지를 돌리기 전에 닫아 실행하지 않는다.
            using (var mdi = new Form())
            {
                mdi.IsMdiContainer = true;
                mdi.StartPosition = FormStartPosition.Manual;
                mdi.Location = new Point(-32000, -32000);
                mdi.ShowInTaskbar = false;
                mdi.Show();
                var a2 = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis();
                a2.MdiParent = mdi;
                typeA.GetField("_result", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a2, result);
                Invoke(typeA, a2, "OpenDetail", topX, topY);
                Form[] details1 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                string title1 = details1.Length > 0 ? details1[0].Text : "";
                Point second = rightDie != null ? DiePoint(rightDie) : new Point(topX, topY);
                Invoke(typeA, a2, "OpenDetail", second.X, second.Y);
                Form[] details2 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                Form secondWindow = details2.FirstOrDefault(f => f != details1.FirstOrDefault());
                string title2 = secondWindow != null ? secondWindow.Text : "";
                Invoke(typeA, a2, "OpenDetail", topX, topY);
                Form[] details3 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                Check("상세보기 Die 마다 창(첫 창 그대로 + 하나 더) / 같은 Die 다시 = 창 늘지 않음 / 탭 제목 상세보기(좌표)",
                    details1.Length == 1 && details2.Length == 2 && details3.Length == 2 && details2.Contains(details1[0]) && details3.SequenceEqual(details2)
                    && title1 == "상세보기 (" + topX + ", " + topY + ")" && title2 == "상세보기 (" + second.X + ", " + second.Y + ")"
                    && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a2, topX, topY) == details1[0],
                    details1.Length + " -> " + details2.Length + " -> " + details3.Length + " / " + title1 + " , " + title2);
                foreach (Form f in details3) f.Dispose();
                a2.Dispose();
            }
            Application.DoEvents();
            // ---- 4) 표시 기준 2 -> N=1 Die 는 연회색 ----
            var numThreshold = (NumericUpDown)Get(typeA, form, "numThreshold");
            numThreshold.Value = 2;
            Application.DoEvents();
            fills = (Dictionary<Point, Color>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieFill");
            DataRow one = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["IN_CHAIN_COUNT"]) == 1);
            if (one != null)
                Check("표시 기준 2: N=1 Die 연회색", fills[DiePoint(one)].ToArgb() == DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.BelowThresholdColor.ToArgb(), DiePoint(one).ToString());
            Check("표시 기준 2: 구간 시작 2", form.ColorBands[0].From == 2, form.ColorBands[0].From.ToString());
            numThreshold.Value = 1;
            Application.DoEvents();

            // ---- 5) 사용자 구간: FROM/TO 수정 -> 자동 해제 ----
            // 실제 조작과 같게: TO 칸 편집 -> 다른 칸을 마우스로 누름(OnMouseDown -> SetCurrentCellAddressCore -> EndEdit -> CellEndEdit).
            // CurrentCell 속성으로 옮기면 처리 순서가 달라 재진입 예외가 재현되지 않는다.
            // 2026-10-12 사용자 실행 중 이 경로에서 Rows.Clear 재진입 예외가 났다(처리 함수 직접 호출로는 재현되지 않음).
            string bandError = "";
            try
            {
                form.ShowBandPopup();   // 색 구간 표는 팝업 안 (2026-10-19)
                Application.DoEvents();
                dgvBand.Focus();
                dgvBand.CurrentCell = dgvBand.Rows[0].Cells[1];
                dgvBand.BeginEdit(true);
                if (dgvBand.EditingControl == null) throw new InvalidOperationException("편집 시작 실패");
                dgvBand.EditingControl.Text = "2";
                dgvBand.NotifyCurrentCellDirty(true);
                ClickCell(dgvBand, 0, 1);
                Pump(300);
            }
            catch (Exception ex) { bandError = ex.GetType().Name + ": " + ex.Message; }
            var chkAuto = (CheckBox)Get(typeA, form, "chkAutoBand");
            Check("구간 TO 칸 편집 후 다른 칸 이동 -> 사용자 구간(예외 없음, 자동 해제)", bandError == "" && !chkAuto.Checked && form.ColorBands[0].To == 2
                && System.Convert.ToString(dgvBand.Rows[0].Cells[1].Value) == "2", bandError == "" ? "auto=" + chkAuto.Checked + " to=" + form.ColorBands[0].To : bandError);
            bandError = "";
            try
            {
                dgvBand.CurrentCell = dgvBand.Rows[0].Cells[0];
                dgvBand.BeginEdit(true);
                dgvBand.EditingControl.Text = "abc";
                dgvBand.NotifyCurrentCellDirty(true);
                ClickCell(dgvBand, 1, 1);
                Pump(300);
            }
            catch (Exception ex) { bandError = ex.GetType().Name + ": " + ex.Message; }
            Check("구간 FROM 잘못된 값 -> 되돌림(예외 없음)", bandError == "" && System.Convert.ToString(dgvBand.Rows[0].Cells[0].Value) == form.ColorBands[0].From.ToString(),
                bandError == "" ? "from=" + dgvBand.Rows[0].Cells[0].Value : bandError);
            chkAuto.Checked = true;
            Application.DoEvents();

            // ---- 6) 조건 변경 -> 경고 -> 다시 분석 ----
            var numTol = (NumericUpDown)Get(typeA, form, "numTolerance");
            numTol.Value = 50;
            Application.DoEvents();
            Check("조건 변경 경고", status.Text.Contains("조건 변경됨"), "");
            DataSet result50 = form.AnalyzeNow();
            int in50 = Convert.ToInt32(result50.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);
            Check("허용오차 50 재분석 >= 5 µm", in50 >= expectedInChain && !status.Text.Contains("조건 변경됨"), expectedInChain + " -> " + in50);
            numTol.Value = 5;
            // ---- 6-0) Defect 영역: 중심 사각형(한 변 10 µm) = 크기 끔 + 허용오차 5 µm (2026-10-16) ----
            var rdoSquare = (RadioButton)Get(typeA, form, "rdoSquare");
            var rdoDefectSize = (RadioButton)Get(typeA, form, "rdoDefectSize");
            var numSquare = (NumericUpDown)Get(typeA, form, "numSquare");
            rdoSquare.Checked = true;
            numSquare.Value = 10;
            DataRow sq = form.AnalyzeNow().Tables["ANL_INFO"].Rows[0];
            string sqStatus = status.Text;
            bool sqInputs = numSquare.Enabled && !numTol.Enabled;
            rdoDefectSize.Checked = true;
            Check("Defect 중심 사각형 한 변 10 -> 크기 끔 + 허용오차 5 / 입력칸 전환 / 상태줄 문구", sq["USE_DEFECT_SIZE"].ToString() == "N" && Convert.ToDouble(sq["TOLERANCE_UM"]) == 5
                && sqInputs && numTol.Enabled && sqStatus.Contains("중심 사각형 한 변 10 µm"), sq["USE_DEFECT_SIZE"] + " / " + sq["TOLERANCE_UM"] + " / " + sqStatus.Split('\n')[0]);
            // ---- 6-1) Chain 면 (2026-10-16): Chain 면 ≠ 검사 면이면 반전. 고르는 것은 GDS Chain 면만, KLARF 검사 면은 FS 고정 (2026-10-20) ----
            var cboChainSide = (ComboBox)Get(typeA, form, "cboChainSide");
            cboChainSide.SelectedIndex = 0;   // Chain 면 FS = 검사 면 FS
            DataRow sameInfo = form.AnalyzeNow().Tables["ANL_INFO"].Rows[0];
            string sameStatus = status.Text.Split('\n')[0];
            cboChainSide.SelectedIndex = 1;   // 기본 Chain 면 BS 로 되돌림
            Check("기본 Chain BS ≠ 검사 FS -> 반전 / Chain FS = 검사 FS -> 반전 없음(보기 면 바뀜, 검사 면은 그대로 FRONT)",
                sameInfo["MIRROR"].ToString() == "N" && sameInfo["VIEW_SIDE"].ToString() != info["VIEW_SIDE"].ToString() && sameInfo["STEP_SIDES"].ToString() == "FRONT"
                && info["MIRROR"].ToString() == "Y" && sameStatus.Contains("(Chain FS)") && !sameStatus.Contains("검사 FS") && !sameStatus.Contains("반전"), sameInfo["VIEW_SIDE"] + " / " + sameStatus);
            result = form.AnalyzeNow();
            form.SelectDie(topX, topY);
            Application.DoEvents();

            // ---- 7) 화면 이미지 ----
            using (var bmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                bmp.Save(png, System.Drawing.Imaging.ImageFormat.Png);
            }
            Console.WriteLine("화면 A 이미지: " + png);
        }

        // ---- 8) 화면 B 계약(ENGUI 호스트가 리플렉션으로 찾는 멤버) ----
        Type typeB = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
        Check("화면 B 계약: 생성자(bool) / ShowDie(DataSet,int,int) / DefectImageRequested(Action<long,long>)",
            typeB.GetConstructor(new[] { typeof(bool) }) != null && typeB.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) }) != null
            && typeB.GetProperty("DefectImageRequested") != null && typeB.GetProperty("DefectImageRequested").PropertyType == typeof(Action<long, long>), "");

        // ---- 9) 화면 B: 도면 + Chain + Defect ----
        using (var detail = new NexplantQMS.GdsMap.GdsDieAnalysisForm(true))
        {
            detail.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            detail.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            detail.StartPosition = FormStartPosition.Manual;
            detail.Location = new Point(-32000, -32000);
            detail.Size = new Size(1400, 900);
            detail.ShowInTaskbar = false;
            detail.ShowErrorDialogs = false;
            detail.Show();
            Application.DoEvents();
            // 콘솔 Smoke 는 Application.Run 이 없어 첫 화면을 닫은 뒤 동기화 문맥이 일반 SynchronizationContext 로 바뀐다(새 화면을 띄워도 그대로).
            // 그러면 await 뒤 처리가 다른 스레드에서 돌아 OpenGL MakeCurrent 가 실패(Error 170)하므로, DMS(메시지 루프 = WinForms 문맥)와 같게 화면을 띄운 뒤 지정한다.
            System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            var watch = Stopwatch.StartNew();
            DataSet dieData = analysisBsl.AnalyzeDie(baseRequest, topX, topY, null);
            Wait(detail.ShowDieAsync(dieData, topX, topY), 600);
            double loadSeconds = watch.Elapsed.TotalSeconds;
            Console.WriteLine("화면 B 첫 표시(도면 + Chain Member 조회): " + loadSeconds.ToString("0.0") + "초 / 상태: " + detail.StatusText);
            // 도면 키 = Factory|Device|보기 기준|배경|Chain 번호 목록|Chain 저장 시각 (2026-10-16 Chain 목록 / 저장 시각 추가)
            Check("화면 B 도면 키", detail.LoadedMapKey != null && detail.LoadedMapKey.StartsWith(factory + "|" + device + "|" + dieData.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"] + "|CHAIN|")
                && detail.LoadedMapKey == NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(dieData), detail.LoadedMapKey);
            int dieDefects = dieData.Tables["DEFECT"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY);
            Check("화면 B Defect 표시 = 선택 Die Defect", detail.DefectMarkerCount == dieDefects, detail.DefectMarkerCount + " / " + dieDefects);
            Check("화면 B Chain 표시(걸린 Chain)", detail.VisibleChainCount == Math.Max(1, dieChains) || (dieChains == 0 && detail.VisibleChainCount > 0), detail.VisibleChainCount + " / " + dieChains);

            // 반전 일치: 걸린 Member 도형(지도에 그린 보기 기준 좌표)이 Defect 사각형 ± 허용오차와 겹쳐야 한다
            Type typeB2 = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
            var gmap = (NexplantQMS.GdsMap.GdsMapControl)Get(typeB2, detail, "map");
            int pairs = 0, overlaps = 0;
            foreach (DataRow hit in dieData.Tables["DEFECT_HIT"].Rows)
            {
                DataRow d = dieData.Tables["DEFECT"].Select("DEFECTID = " + hit["DEFECTID"])[0];
                if (Convert.ToInt32(d["XINDEX"]) != topX || Convert.ToInt32(d["YINDEX"]) != topY) continue;
                pairs++;
                IList<NexplantQMS.GdsMap.Chain.ChainTraceElement> el = gmap.GetChainTraceElements(new[] { hit["PLACED_ELEMENT_ID"].ToString() });
                if (el.Count != 1) continue;
                double x = Convert.ToDouble(d["GDS_X"]), y = Convert.ToDouble(d["GDS_Y"]);
                double hw = Convert.ToDouble(d["XSIZE"]) / 2 + 5.001, hh = Convert.ToDouble(d["YSIZE"]) / 2 + 5.001;
                var b = el[0].Bounds;
                if (b.MinX <= x + hw && b.MaxX >= x - hw && b.MinY <= y + hh && b.MaxY >= y - hh) overlaps++;
            }
            Check("화면 B 반전 일치: 걸린 Member 도형 = Defect 위치(보기 기준)", pairs == 0 || overlaps == pairs, overlaps + " / " + pairs);

            // 상세 화면 = 선택 Chip 1개(2026-10-12 요청): Chip 이동 / Wafer 합산 컨트롤이 없고 위쪽에 Chip 좌표 정보가 나온다
            var lblDieInfo = (Label)Get(typeB2, detail, "lblDieInfo");
            Check("화면 B Chip 좌표 정보 / Chip 이동 컨트롤 없음", lblDieInfo.Text.StartsWith("Die (" + topX + ", " + topY + ")  Defect 전체 " + dieDefects + " / Chain In ")
                && typeB2.GetField("cboDie", BindingFlags.NonPublic | BindingFlags.Instance) == null && typeB2.GetField("rdoWafer", BindingFlags.NonPublic | BindingFlags.Instance) == null,
                lblDieInfo.Text);
            // 화면 B [GDS / KLARF] 표의 Chain 열 = 이름(맨 아래 묶음은 "Chain 밖 Defect"), 이름 열 없음 (2026-10-16, 표 합침 2026-10-20)
            var dgvChainsB = (DataGridView)Get(typeB2, detail, "dgvChainDefects");
            var chainNamesB = new HashSet<string>(result.Tables["CHAIN"].Rows.Cast<DataRow>().Select(r => r["CHAIN_NAME"].ToString()));
            Check("화면 B Chain 열 = 이름(코드 없음, 맨 아래 Chain 밖 Defect) / 이름 열 없음", dgvChainsB.Rows.Count > 0
                && dgvChainsB.Rows.Cast<DataGridViewRow>().All(r => chainNamesB.Contains(Convert.ToString(r.Cells["colCode"].Value)) || Convert.ToString(r.Cells["colCode"].Value) == "Chain 밖 Defect")
                && !dgvChainsB.Columns.Contains("colName"),
                dgvChainsB.Rows.Count == 0 ? "-" : Convert.ToString(dgvChainsB.Rows[0].Cells["colCode"].Value));
            // 화면 B 도면의 Chain 색 = 저장 색(E-Test Fail Chain 도 빨간색으로 덮지 않음), 더블클릭(역추적) / 해제해도 Chain 목록 순서 · 선택 행 그대로 (2026-10-16)
            Dictionary<long, int> stored = result.Tables["CHAIN"].Rows.Cast<DataRow>().ToDictionary(r => Convert.ToInt64(r["CHAIN_SEQ"]), r =>
            {
                int argb = Convert.ToInt32(r["COLOR_ARGB"]);
                return (argb == 0 ? Color.Orange : Color.FromArgb(255, Color.FromArgb(argb))).ToArgb();
            });
            List<string> colorBad = detail.VisibleChainColors.Where(p => !stored.ContainsKey(p.Key) || stored[p.Key] != p.Value.ToArgb()).Select(p => p.Key.ToString()).ToList();
            // 묶음 순서 = 묶음 첫 행의 Chain 이름 순서. 역추적 묶음은 탐색 거리 안 Defect 가 더해질 수 있어 행 수가 아니라 묶음 순서를 본다 (2026-10-20)
            Func<List<string>> groupOrder = () => dgvChainsB.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["colCode"].Value)).Distinct().ToList();
            List<string> orderBefore = groupOrder();
            DataGridViewRow pickHead = dgvChainsB.Rows.Cast<DataGridViewRow>().Last(r => Convert.ToString(r.Cells["colCode"].Value) != "Chain 밖 Defect"
                && (r.Index == 0 || Convert.ToString(dgvChainsB.Rows[r.Index - 1].Cells["colCode"].Value) != Convert.ToString(r.Cells["colCode"].Value)));
            string pickName = Convert.ToString(pickHead.Cells["colCode"].Value);
            Func<bool> onPickHead = () => dgvChainsB.CurrentRow != null && Convert.ToString(dgvChainsB.CurrentRow.Cells["colCode"].Value) == pickName
                && (dgvChainsB.CurrentRow.Index == 0 || Convert.ToString(dgvChainsB.Rows[dgvChainsB.CurrentRow.Index - 1].Cells["colCode"].Value) != pickName);
            MethodInfo dbl = typeB2.GetMethod("dgvChainDefects_CellDoubleClick", BindingFlags.NonPublic | BindingFlags.Instance);
            dbl.Invoke(detail, new object[] { dgvChainsB, new DataGridViewCellEventArgs(dgvChainsB.Columns["colCode"].Index, pickHead.Index) });
            Application.DoEvents();
            bool focusKeep = groupOrder().SequenceEqual(orderBefore) && onPickHead() && detail.VisibleChainCount <= 1;
            typeB2.GetMethod("btnClearFocus_Click", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(detail, new object[] { null, EventArgs.Empty });
            Application.DoEvents();
            bool clearKeep = groupOrder().SequenceEqual(orderBefore) && onPickHead();
            Check("화면 B 도면 Chain 색 = 저장 색 / Chain 칸 더블클릭(역추적) · 해제 뒤 묶음 순서와 고른 Chain 묶음 그대로",
                detail.VisibleChainColors.Count > 0 && colorBad.Count == 0 && focusKeep && clearKeep,
                "그린 Chain " + detail.VisibleChainColors.Count + " / 색 다름 " + colorBad.Count + " / 더블클릭 " + pickName + " -> "
                + (dgvChainsB.CurrentRow == null ? "-" : Convert.ToString(dgvChainsB.CurrentRow.Cells["colCode"].Value)) + " / 역추적 " + focusKeep + " / 해제 " + clearKeep);

            // 화면 B 배치 (2026-10-19, 설계 2026-10-19_화면A_레이아웃_후보.md 10절): 화면 A 와 같은 기능 탭 + 기준 제목 + 분석 기준 요약 3칸 + 용어
            {
                Func<string, Control> b = n => (Control)Get(typeB2, detail, n);
                var tabB = (TabControl)b("tabMain");
                string[] tabsB = tabB.TabPages.Cast<TabPage>().Select(p => p.Text).ToArray();
                bool parents = b("dgvChainDefects").Parent == b("tabChainDefect") && b("dgvEtValues").Parent == b("tabEt")
                    && b("dgvSummary").Parent == b("tabDie") && b("map").Parent == ((SplitContainer)b("splitMain")).Panel1;
                bool titles = b("lblGdsCriteria").Text == "GDS / KLARF 기준" && b("lblGdsText").Text.Contains(Environment.NewLine + "Step ") && b("lblEtCriteria").Text == "E-Test 기준"
                    && b("lblSumTitle").Text == "분석 기준" && b("lblSumGds").Text == "GDS · Chain BS -> 반전" && b("lblGdsText").Text.Contains(" · Chain 면 BS · Chain ") && !b("lblGdsText").Text.Contains("검사 면") && b("lblSumKlarf").Text.StartsWith("KLARF · Step 1개 · 크기 + 허용오차 5 µm · 보정 (0, 0)")
                    && b("lblSumEt").Text.StartsWith("E-Test · ") && b("lblMapSide").Text == "보기 = " + NexplantQMS.GdsMap.Persistence.GdsSideTransform.Text(dieData.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString());
                bool oldGone = new[] { "lblCriteria", "tabRight", "tabGds", "tabKlarf", "dgvChains", "dgvDefects", "cboScope", "lblKlarfCriteria" }
                    .All(n => typeB2.GetField(n, BindingFlags.NonPublic | BindingFlags.Instance) == null);
                Check("화면 B 배치: 기능 탭 GDS / KLARF(합침) / E-Test / 종합 (Die) · 표 위치 · 기준 제목(GDS / KLARF 2줄) · 분석 기준 3칸(화면 A 문구) · 도면 머리 보기 면",
                    tabsB.SequenceEqual(new[] { "GDS / KLARF", "E-Test", "종합 (Die)" }) && parents && titles && oldGone,
                    String.Join(",", tabsB) + " / 표 위치 " + parents + " / 옛 컨트롤 없음 " + oldGone + " / " + b("lblSumGds").Text + " | " + b("lblSumKlarf").Text + " | " + b("lblSumEt").Text + " | " + b("lblMapSide").Text);

                // 용어: Chain 칸(표시 · 색 · Chain · Defect 수 · 분류 · ET · Fail Para · Member) + Defect 칸(Defect ID · Gap · RoughBin · Class ...), 4분류 '-' 없음
                string[] gridHeads = dgvChainsB.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray();
                List<DataGridViewRow> chainRows = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colCode"].Value) != "Chain 밖 Defect").ToList();
                string[] classes = chainRows.Select(r => Convert.ToString(r.Cells["colClass"].Value)).Distinct().ToArray();
                Check("화면 B 용어 = 화면 A: Chain 칸 8열 + Defect 칸 10열(Defect ID / Gap / 가까운 Chain(Gap 바로 뒤, 2026-10-20) / RoughBin / Class / 크기 / GDS X / Y / XREL / YREL), 분류 '-' 없음, 정렬 안 됨",
                    gridHeads.SequenceEqual(new[] { "표시", "색", "Chain", "Defect 수", "분류", "ET", "Fail Para", "Member",
                        "Defect ID", "Gap(µm)", "가까운 Chain", "RoughBin", "Class", "크기(µm)", "GDS X", "GDS Y", "XREL", "YREL" })
                    && classes.All(k => new[] { "일치", "Fail만", "Defect만", "정상", "비교 불가" }.Contains(k))
                    && chainRows.All(r => Convert.ToString(r.Cells["colMember"].Value).Length > 0)
                    && dgvChainsB.Columns.Cast<DataGridViewColumn>().All(c => c.SortMode == DataGridViewColumnSortMode.NotSortable),
                    String.Join(",", gridHeads) + " / 분류 " + String.Join(",", classes));

                // 합친 표 묶음 규칙 (2026-10-20, 설계 2026-10-20_상세보기_GDS_KLARF_합친탭_설계.md 2.1절):
                // Chain 묶음 행 = 그 Chain 에 걸린 Defect(CHAIN_CODES) 수(없으면 1행), 맨 아래 Chain 밖 묶음 = IN_CHAIN N Defect(가까운 거리 순, 거리 없음 뒤),
                // 머리 줄 = Chain n / Defect 전체 / Chain In / Chain Out, [Chain 밖 Defect] 끄면 그 묶음 없음
                {
                    List<DataRow> dieDefectRows = dieData.Tables["DEFECT"].Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY).ToList();
                    Dictionary<string, string> nameToCode = dieData.Tables["CHAIN"].Rows.Cast<DataRow>().ToDictionary(r => r["CHAIN_NAME"].ToString(), r => r["CHAIN_CODE"].ToString());
                    List<string> groups = groupOrder();
                    var groupErrors = new List<string>();
                    foreach (string g in groups)
                    {
                        List<string> ids = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colCode"].Value) == g)
                            .Select(r => Convert.ToString(r.Cells["colDefectId"].Value)).Where(s => s.Length > 0).ToList();
                        List<string> want = g == "Chain 밖 Defect"
                            ? dieDefectRows.Where(r => r["IN_CHAIN"].ToString() != "Y").Select(r => r["DEFECTID"].ToString()).ToList()
                            : dieDefectRows.Where(r => ("," + r["CHAIN_CODES"].ToString().Replace(" ", "") + ",").Contains("," + nameToCode[g] + ",")).Select(r => r["DEFECTID"].ToString()).ToList();
                        if (ids.Count != want.Count || ids.Except(want).Any()) groupErrors.Add(g + " " + ids.Count + "/" + want.Count);
                    }
                    int outCount = dieDefectRows.Count(r => r["IN_CHAIN"].ToString() != "Y");
                    List<double> outGaps = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colCode"].Value) == "Chain 밖 Defect")
                        .Select(r => { double v; return double.TryParse(Convert.ToString(r.Cells["colGap"].Value), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v) ? v : double.MaxValue; }).ToList();
                    bool outLastSorted = outCount == 0 || (groups.Last() == "Chain 밖 Defect" && outGaps.Zip(outGaps.Skip(1), (x, y) => x <= y).All(ok => ok));
                    string title = ((Label)b("lblChainTitle")).Text;
                    int chainGroups = groups.Count(g => g != "Chain 밖 Defect");
                    bool titleOk = title == String.Format("Chain {0} / Defect 전체 {1} / Chain In {2} / Chain Out {3}", chainGroups, dieDefectRows.Count, dieDefectRows.Count - outCount, outCount);
                    var chkOutsideB = (CheckBox)b("chkOutside");
                    chkOutsideB.Checked = false;
                    Application.DoEvents();
                    bool hidden = dgvChainsB.Rows.Cast<DataGridViewRow>().All(r => Convert.ToString(r.Cells["colCode"].Value) != "Chain 밖 Defect");
                    chkOutsideB.Checked = true;
                    Application.DoEvents();
                    bool shownAgain = outCount == 0 || dgvChainsB.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToString(r.Cells["colCode"].Value) == "Chain 밖 Defect") == outCount;
                    Check("화면 B [GDS / KLARF] 묶음: Chain 묶음 Defect = 그 Chain 에 걸린 Defect / 맨 아래 Chain 밖 = Chain Out(가까운 거리 순) / 머리 줄 수 / [Chain 밖 Defect] 끄기 · 켜기",
                        groupErrors.Count == 0 && outLastSorted && titleOk && hidden && shownAgain,
                        "묶음 " + String.Join(",", groups) + " / 어긋남 " + String.Join(",", groupErrors) + " / 순서 " + outLastSorted + " / \"" + title + "\" / 끄기 " + hidden + " 켜기 " + shownAgain);
                }

                // 두 Chain 에 걸린 Defect (2026-10-20): 두 묶음 모두 + Defect ID 굵게 + 바로 옆 "가까운 Chain" = "함께 걸림: 다른 Chain", 다른 Defect 는 굵지 않음.
                // 이 Smoke 데이터에는 두 Chain Defect 가 없어, 걸린 Defect 하나에 Chain 코드를 하나 더 넣은 복사본으로 확인하고 원래 결과로 되돌린다.
                {
                    DataSet twoCopy = dieData.Copy();
                    DataRow hitRow = twoCopy.Tables["DEFECT"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY && r["IN_CHAIN"].ToString() == "Y");
                    DataRow otherChain = hitRow == null ? null : twoCopy.Tables["CHAIN"].Rows.Cast<DataRow>()
                        .FirstOrDefault(r => !("," + hitRow["CHAIN_CODES"].ToString().Replace(" ", "") + ",").Contains("," + r["CHAIN_CODE"] + ","));
                    bool twoOk = false;
                    string twoText = "걸린 Defect 또는 다른 Chain 없음";
                    if (hitRow != null && otherChain != null)
                    {
                        string firstCode = hitRow["CHAIN_CODES"].ToString().Split(',')[0].Trim();
                        string twoFirstName = twoCopy.Tables["CHAIN"].Rows.Cast<DataRow>().First(r => r["CHAIN_CODE"].ToString() == firstCode)["CHAIN_NAME"].ToString();
                        string twoOtherName = otherChain["CHAIN_NAME"].ToString();
                        hitRow["CHAIN_CODES"] = firstCode + "," + otherChain["CHAIN_CODE"];
                        DataTable dcT = twoCopy.Tables["DIE_CHAIN"];
                        DataRow dc = dcT.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY && r["CHAIN_SEQ"].ToString() == otherChain["CHAIN_SEQ"].ToString());
                        if (dc == null)
                        {
                            dc = dcT.NewRow();
                            dc["XINDEX"] = topX;
                            dc["YINDEX"] = topY;
                            dc["CHAIN_SEQ"] = otherChain["CHAIN_SEQ"];
                            dc["CHAIN_CODE"] = otherChain["CHAIN_CODE"];
                            dc["DEFECT_COUNT"] = 0;
                            dcT.Rows.Add(dc);
                        }
                        dc["DEFECT_COUNT"] = Convert.ToInt32(dc["DEFECT_COUNT"]) + 1;
                        Wait(detail.ShowDieAsync(twoCopy, topX, topY), 60);
                        Application.DoEvents();
                        string id = hitRow["DEFECTID"].ToString();
                        List<DataGridViewRow> twoRows = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colDefectId"].Value) == id).ToList();
                        Func<DataGridViewRow, bool> bold = r => r.Cells["colDefectId"].Style.Font != null && r.Cells["colDefectId"].Style.Font.Bold;
                        bool othersPlain = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colDefectId"].Value).Length > 0 && Convert.ToString(r.Cells["colDefectId"].Value) != id).All(r => !bold(r));
                        twoOk = twoRows.Count == 2 && twoRows.All(bold) && othersPlain
                            && twoRows.Any(r => Convert.ToString(r.Cells["colCode"].Value) == twoFirstName && Convert.ToString(r.Cells["colNear"].Value) == "함께 걸림: " + twoOtherName)
                            && twoRows.Any(r => Convert.ToString(r.Cells["colCode"].Value) == twoOtherName && Convert.ToString(r.Cells["colNear"].Value) == "함께 걸림: " + twoFirstName)
                            && dgvChainsB.Columns["colNear"].Index == dgvChainsB.Columns["colGap"].Index + 1;
                        twoText = "Defect " + id + " / " + twoFirstName + " + " + twoOtherName + " / 행 " + twoRows.Count + " / 굵게 " + twoRows.Count(bold) + " / 다른 Defect 굵지 않음 " + othersPlain
                            + " / " + String.Join(" | ", twoRows.Select(r => r.Cells["colCode"].Value + ": " + r.Cells["colNear"].Value).ToArray());
                        Wait(detail.ShowDieAsync(dieData, topX, topY), 60);
                        Application.DoEvents();
                    }
                    Check("화면 B 두 Chain 에 걸린 Defect: 두 묶음 모두 / Defect ID 굵게 / Gap 바로 뒤 가까운 Chain = 함께 걸림 + 다른 Chain (복사본 결과, 되돌림)", twoOk, twoText);
                }

                // [종합 (Die)]: 4분류 수 합 = Chain 수, 표 = 그 Die 의 일치 · Fail만 · Defect만 (Die, Chain), Die 요약 = Die 줄과 같은 수
                int chainCount = dieData.Tables["CHAIN"].Rows.Count;
                int chipSum = new[] { "lblClassBoth", "lblClassFail", "lblClassDefect", "lblClassOk", "lblClassNa" }
                    .Sum(n => int.Parse(b(n).Text.Split(' ').Last()));
                int wantRows = dieData.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY
                    && new[] { "BOTH", "FAIL", "DEFECT" }.Contains(Convert.ToString(r["CLASS4"])));
                var dgvSum = (DataGridView)b("dgvSummary");
                Check("화면 B [종합 (Die)]: 4분류 수 합 = Chain 수 / 표 행 = 일치 · Fail만 · Defect만 (Die, Chain) / Die 요약 Defect 전체 · Chain In",
                    chipSum == chainCount && dgvSum.Rows.Count == wantRows && b("lblDieText").Text.StartsWith("Defect 전체 " + dieDefects + " / Chain In ")
                    && b("lblDieCriteria").Text == "Die (" + topX + ", " + topY + ") 요약",
                    "4분류 합 " + chipSum + " / Chain " + chainCount + " / 표 " + dgvSum.Rows.Count + " / 기대 " + wantRows + " / " + b("lblDieText").Text);

                // 처음 열기 = [종합 (Die)], 역추적으로 열기 = [GDS / KLARF] + 탭 제목 "(역추적)", 해제하면 원래 제목
                string firstName = Convert.ToString(dgvChainsB.Rows[0].Cells["colCode"].Value);
                string focusSeq = dieData.Tables["CHAIN"].Rows.Cast<DataRow>().First(r => r["CHAIN_NAME"].ToString() == firstName)["CHAIN_SEQ"].ToString();
                Wait(detail.ShowDieAsync(dieData, topX, topY, focusSeq), 60);
                Application.DoEvents();
                bool focusTab = tabB.SelectedTab == b("tabChainDefect") && b("tabChainDefect").Text == "GDS / KLARF (역추적)" && b("tabEt").Text == "E-Test (역추적)"
                    && dgvChainsB.CurrentRow != null && Convert.ToString(dgvChainsB.CurrentRow.Cells["colCode"].Value) == firstName;
                Wait(detail.ShowDieAsync(dieData, topX, topY, null), 60);
                Application.DoEvents();
                bool plainTab = tabB.SelectedTab == b("tabDie") && b("tabChainDefect").Text == "GDS / KLARF" && b("tabEt").Text == "E-Test";
                Check("화면 B 첫 탭: Die 전체 = [종합 (Die)] / 역추적 = [GDS / KLARF](그 Chain 묶음) + 탭 제목 (역추적)", focusTab && plainTab,
                    "역추적 " + focusTab + " / 전체 " + plainTab + " / 지금 " + tabB.SelectedTab.Text);
            }

            DataRow other = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            if (other != null)
            {
                watch.Restart();
                Wait(detail.ShowDieAsync(analysisBsl.AnalyzeDie(baseRequest, DiePoint(other).X, DiePoint(other).Y, null), DiePoint(other).X, DiePoint(other).Y), 60);
                Check("화면 B Die 변경 = 도면 다시 읽지 않음", watch.Elapsed.TotalSeconds < 5 && detail.DefectMarkerCount == Convert.ToInt32(other["DEFECT_COUNT"]),
                    watch.Elapsed.TotalSeconds.ToString("0.00") + "초 / Defect " + detail.DefectMarkerCount);
            }
            var dgvDefects = (DataGridView)Get(typeB2, detail, "dgvChainDefects");
            DataGridViewRow defectRow = dgvDefects.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Convert.ToString(r.Cells["colDefectId"].Value).Length > 0);
            if (defectRow != null)
            {
                dgvDefects.ClearSelection();
                defectRow.Selected = true;
                Application.DoEvents();
                Check("화면 B Defect 행 선택 -> 상태줄", ((ToolStripStatusLabel)Get(typeB2, detail, "lblStatus")).Text.StartsWith("Defect #"),
                    ((ToolStripStatusLabel)Get(typeB2, detail, "lblStatus")).Text);
            }
        }

        // ---- 10) 상세보기 여러 창 + GDS 상세 화면(도면) 하나 같이 쓰기 + [분석] 때 미리 읽기 (2026-10-16, 설계 Die상세_여러화면_GDS재사용) ----
        // MDI 부모에 화면 A(띄우지 않음: Load 의 원격 조회를 피함)를 붙이고 실제 경로(OpenDetail -> AnalyzeDie -> 호스트 창 Shown / Activated)를 탄다.
        // 호스트가 만드는 공유 화면은 기본 서비스(DefaultMapService / DefaultChainService)를 같은 프로세스 BSL 로 바꿔 쓴다.
        {
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            NexplantQMS.GdsMap.GdsDieMapCache.Clear();
            DataRow secondDie = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            Point p2 = secondDie != null ? DiePoint(secondDie) : new Point(topX, topY);
            using (var mdi = new Form())
            {
                mdi.IsMdiContainer = true;
                mdi.StartPosition = FormStartPosition.Manual;
                mdi.Location = new Point(-32000, -32000);
                mdi.Size = new Size(1400, 900);
                mdi.ShowInTaskbar = false;
                mdi.Show();
                var a3 = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis();
                a3.MdiParent = mdi;
                a3.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
                a3.ShowErrorDialogs = false;
                typeA.GetField("_result", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a3, result);
                typeA.GetField("_lastRequest", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a3, baseRequest);

                // 미리 읽기: Wafer 결과의 키 = 상세 결과(AnalyzeDie)의 키여야 첫 상세보기에서 DB 를 부르지 않는다
                string keyWafer = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(result);
                string keyDie = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(analysisBsl.AnalyzeDie(baseRequest, topX, topY, null));
                int loads0 = NexplantQMS.GdsMap.GdsDieMapCache.LoadCount;
                var watch = Stopwatch.StartNew();
                DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Preload(a3, result);
                Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                double preloadSeconds = watch.Elapsed.TotalSeconds;
                Check("[분석] 뒤 미리 읽기: Wafer 결과 키 = 상세 결과 키 / DB 읽기 1번", keyWafer != null && keyWafer == keyDie
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && NexplantQMS.GdsMap.GdsDieMapCache.Key == keyWafer,
                    preloadSeconds.ToString("0.0") + "초 / " + keyWafer + (keyWafer == keyDie ? "" : " <> " + keyDie));

                // 첫 상세보기: Shown 에서 공유 화면을 만들고 미리 읽은 데이터로 도면만 만든다
                watch.Restart();
                Invoke(typeA, a3, "OpenDetail", topX, topY);
                var w1 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a3, topX, topY);
                bool shown1 = WaitFor(() => ShowsDie(topX, topY), 180);
                double firstSeconds = watch.Elapsed.TotalSeconds;
                var shared = (NexplantQMS.GdsMap.GdsDieAnalysisForm)DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm;
                Check("첫 상세보기 = 공유 화면 + 미리 읽은 도면(DB 다시 읽지 않음)", shown1 && w1 != null && w1.HasSharedDetail
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && shared.LoadedMapKey == keyWafer,
                    firstSeconds.ToString("0.0") + "초 / " + (shared == null ? "공유 화면 없음" : shared.StatusText));
                if (!shown1 || shared == null) throw new InvalidOperationException("첫 상세보기가 표시되지 않아 이후 검사를 멈춤");
                // 2026-10-20: Defect 범위 콤보 대신 [Chain 밖 Defect] 체크(창마다 보기 상태)
                var chkOutsideB = (CheckBox)Get(typeB, shared, "chkOutside");
                var gmapB = (NexplantQMS.GdsMap.GdsMapControl)Get(typeB, shared, "map");
                IntPtr mapHandle = gmapB.Handle;
                // 창 1 보기 바꾸기: Chain 밖 Defect 끔, 확대 위치 이동
                chkOutsideB.Checked = false;
                double cx, cy, scale, label;
                gmapB.GetView(out cx, out cy, out scale, out label);
                double view1X = cx + 37, view1Y = cy - 21, view1Scale = scale * 1.7;
                gmapB.SetView(view1X, view1Y, view1Scale, label);

                // 두 번째 Die: 창 하나 더, 공유 화면을 옮겨 쓰고 도면은 다시 읽거나 만들지 않는다
                watch.Restart();
                Invoke(typeA, a3, "OpenDetail", p2.X, p2.Y);
                var w2 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a3, p2.X, p2.Y);
                bool shown2 = WaitFor(() => ShowsDie(p2.X, p2.Y), 60);
                double secondSeconds = watch.Elapsed.TotalSeconds;
                Check("두 번째 Die = 새 창(첫 창 그대로) / 공유 화면 옮김(같은 화면 · 같은 도면 핸들) / 첫 창 안내 문구 / 도면 다시 읽지 않음",
                    shown2 && w2 != null && w2 != w1 && !w1.IsDisposed && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 2
                    && w2.HasSharedDetail && !w1.HasSharedDetail && w1.ShowsPlaceholder && !w2.ShowsPlaceholder
                    && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == shared && gmapB.Handle == mapHandle
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && shared.LoadedMapKey == keyWafer && secondSeconds < 5
                    && w1.Text == "상세보기 (" + topX + ", " + topY + ")" && w2.Text == "상세보기 (" + p2.X + ", " + p2.Y + ")",
                    secondSeconds.ToString("0.00") + "초 / 창 " + DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count + " / " + w1.Text + " , " + (w2 == null ? "-" : w2.Text));
                chkOutsideB.Checked = true;   // 창 2 = Chain 밖 Defect 켬
                gmapB.GetView(out cx, out cy, out scale, out label);
                double view2X = cx, view2Y = cy, view2Scale = scale;

                // 같은 Die 다시: 창은 늘지 않고 그 창으로 가며, 그 창에서 보던 Defect 범위 / 확대 위치로 되돌린다
                Invoke(typeA, a3, "OpenDetail", topX, topY);
                bool back1 = WaitFor(() => ShowsDie(topX, topY) && w1.HasSharedDetail, 30);
                gmapB.GetView(out cx, out cy, out scale, out label);
                bool view1Ok = Math.Abs(cx - view1X) < 0.01 && Math.Abs(cy - view1Y) < 0.01 && Math.Abs(scale / view1Scale - 1) < 1e-6;
                bool out1 = chkOutsideB.Checked;
                w2.Activate();
                bool back2 = WaitFor(() => ShowsDie(p2.X, p2.Y) && w2.HasSharedDetail, 30);
                gmapB.GetView(out cx, out cy, out scale, out label);
                bool view2Ok = Math.Abs(cx - view2X) < 0.01 && Math.Abs(cy - view2Y) < 0.01 && Math.Abs(scale / view2Scale - 1) < 1e-6;
                bool out2 = chkOutsideB.Checked;
                Check("같은 Die 다시 = 그 창으로(창 2개 그대로) / 창마다 Chain 밖 Defect · 확대 위치 되돌림 / 도면 다시 읽지 않음",
                    back1 && back2 && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 2 && view1Ok && view2Ok && !out1 && out2
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && gmapB.Handle == mapHandle,
                    "창1 Chain 밖 " + out1 + " 위치 " + view1Ok + " / 창2 Chain 밖 " + out2 + " 위치 " + view2Ok);

                // E-Test Fail 역추적으로 같은 Die 를 열면 창은 그대로, 그 창 역추적 Chain 만 바뀐다
                DataRow focusChain = result.Tables["CHAIN"].Rows[0];
                Invoke(typeA, a3, "OpenDetail", topX, topY, focusChain["CHAIN_SEQ"].ToString());
                var lblFocusB = (Label)Get(typeB, shared, "lblFocus");
                bool focused = WaitFor(() => ShowsDie(topX, topY) && w1.HasSharedDetail && lblFocusB.Visible, 30);
                Check("역추적으로 같은 Die 열기 = 그 창(창 2개 그대로) / 역추적 Chain 표시", focused && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 2
                    && lblFocusB.Text.Contains(focusChain["CHAIN_NAME"].ToString()), lblFocusB.Text);

                // 공유 화면을 가진 창을 닫으면 남은 창이 가져간다(공유 화면 / 도면 핸들 그대로, 해제 안 됨)
                w2.Activate();
                WaitFor(() => w2.HasSharedDetail, 10);
                w2.Dispose();
                bool auto = WaitFor(() => w1.HasSharedDetail, 3);
                if (!auto) w1.Activate();
                bool kept1 = WaitFor(() => ShowsDie(topX, topY) && w1.HasSharedDetail, 30);
                Check("공유 화면을 가진 창 닫기 -> 남은 창이 가져감(공유 화면 · 도면 핸들 그대로)", kept1 && w2.IsDisposed && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 1
                    && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == shared && !shared.IsDisposed && gmapB.Handle == mapHandle,
                    "MDI 자동 활성 " + auto);

                // 창을 모두 닫아도 화면 A 가 열려 있으면 공유 화면 / 미리 읽기 유지(D4) -> 화면 A 를 닫으면 해제(메모리 반환)
                w1.Dispose();
                Application.DoEvents();
                bool keptAll = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == shared && !shared.IsDisposed && NexplantQMS.GdsMap.GdsDieMapCache.Key == keyWafer;
                typeA.GetMethod("OnFormClosed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(a3, new object[] { new FormClosedEventArgs(CloseReason.UserClosing) });
                bool released = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == null && shared.IsDisposed && NexplantQMS.GdsMap.GdsDieMapCache.Key == null;
                Check("상세보기 창을 다 닫아도 화면 A 가 있으면 공유 화면 유지 / 화면 A 닫으면 공유 화면 · 미리 읽기 해제", keptAll && released,
                    "유지 " + keptAll + " / 해제 " + released);
                a3.Dispose();
            }
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = null;
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = null;
        }

        // ---- 11) 메시지 루프 안 검사 (2026-10-16): DACrux 처럼 Application.Run 안에서 상세보기 창끼리 간섭을 본다 ----
        // 콘솔에서 DoEvents 로만 돌리면 도면 키가 다른 창으로 바꿀 때 화면 스레드 문맥이 풀려 OpenGL MakeCurrent 가 실패한다(Error 170, 시험 환경 문제).
        // 기본 결과(Chain BS · 검사 FS = 반전, BACK 보기)와 Chain FS 재분석(반전 없음, FRONT 보기)으로 도면 키가 다른 창을 비교한다 (2026-10-20 KLARF 검사 면 FS 고정)
        DataSet backResult = analysisBsl.Analyze(baseRequest);
        RunInMessageLoop(() => MessageLoopChecks(stepSeq, backResult, baseRequest, analysisBsl, viewBsl, chainBsl));

        Console.WriteLine();
        Console.WriteLine("GDS_KLARF_FORM_SMOKE=" + (failures == 0 ? "PASS" : "FAIL") + " (" + (checks - failures) + "/" + checks + ")");
        return failures == 0 ? 0 : 1;
    }

    /// <summary>body 를 Application.Run 메시지 루프 안에서 돌린다(화면 스레드 문맥 = WindowsFormsSynchronizationContext 유지). 예외는 실패 검사로 남긴다.</summary>
    private static void RunInMessageLoop(Action body)
    {
        Exception error = null;
        var starter = new Form { ShowInTaskbar = false, StartPosition = FormStartPosition.Manual, Location = new Point(-32000, -32000), Size = new Size(10, 10) };
        starter.Shown += (s, e) => starter.BeginInvoke(new Action(() =>
        {
            try { body(); }
            catch (Exception ex) { error = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex; }
            finally { starter.Close(); }
        }));
        Application.Run(starter);
        if (error != null) Check("메시지 루프 안 검사 실행", false, error.GetType().Name + ": " + error.Message);
    }

    /// <summary>
    /// 11절: 상세보기 창끼리 간섭 (2026-10-16 점검 결과 1 ~ 4 수정 확인).
    /// BACK 보기 결과(Chain BS / 검사 FS)와 FRONT 보기 재분석 결과(Chain FS / 검사 FS)로 도면 키가 다른 창을 만든다 (2026-10-20 KLARF 검사 면 FS 고정).
    /// </summary>
    /// <summary>화면 A 의 Map 쪽 패널(splitMain.Panel1).</summary>
    private static System.Windows.Forms.Control splitMainPanel(Form form, Type typeA)
    {
        return ((SplitContainer)Get(typeA, form, "splitMain")).Panel1;
    }

    private static void MessageLoopChecks(long stepSeq, DataSet resBack, DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest reqBack,
        DACrux.SEMDMS.BSL.GdsKlarfAnalysis analysisBsl, DACrux.SEMDMS.BSL.GdsMapView viewBsl, DACrux.SEMDMS.BSL.GdsChain chainBsl)
    {
        Type typeA = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        Type typeB = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
        var reqFront = new DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest
        {
            Factory = reqBack.Factory, DeviceId = reqBack.DeviceId, StepSeqs = reqBack.StepSeqs, ViewSides = reqBack.ViewSides, ChainSide = "FRONT",
            DisplayStepSeq = reqBack.DisplayStepSeq, AreaMode = reqBack.AreaMode, ToleranceUm = reqBack.ToleranceUm, ClassBasis = reqBack.ClassBasis
        };
        DataSet resFront = analysisBsl.Analyze(reqFront);
        List<Point> dies = resBack.Tables["DIE"].Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0).Select(DiePoint).Distinct().Take(3).ToList();
        if (dies.Count < 3)
        {
            Check("메시지 루프 안 검사: Defect Die 3개", false, dies.Count.ToString());
            return;
        }
        Point d1 = dies[0], d2 = dies[1], d3 = dies[2];
        string keyBack = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(resBack);
        string keyFront = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(resFront);
        NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
        NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
        NexplantQMS.GdsMap.GdsDieMapCache.Clear();
        try
        {
            using (var mdi = new Form())
            {
                mdi.IsMdiContainer = true;
                mdi.StartPosition = FormStartPosition.Manual;
                mdi.Location = new Point(-32000, -32000);
                mdi.Size = new Size(1400, 900);
                mdi.ShowInTaskbar = false;
                mdi.Show();
                var a = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis();
                a.MdiParent = mdi;
                a.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
                a.ShowErrorDialogs = false;
                a.ShowAnalysisResult(resBack);   // 분석 순번 1
                typeA.GetField("_lastRequest", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a, reqBack);
                DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Preload(a, resBack);
                Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                int loads0 = NexplantQMS.GdsMap.GdsDieMapCache.LoadCount;

                // ---- 11-1) 새 창은 앞 창의 상태줄 / Chain 고른 행 / 스크롤을 이어받지 않는다. 돌아가면 그 창 값 ----
                Invoke(typeA, a, "OpenDetail", d1.X, d1.Y);
                WaitFor(() => ShowsDie(d1.X, d1.Y), 180);
                var w1 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a, d1.X, d1.Y);
                var b = (NexplantQMS.GdsMap.GdsDieAnalysisForm)DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm;
                // 2026-10-20: Chain 표 + Defect 표 = [GDS / KLARF] 표 하나. 고른 행 = 마지막 Defect 행(Chain 칸 + Defect ID 로 비교)
                var dgvChains = (DataGridView)Get(typeB, b, "dgvChainDefects");
                Func<DataGridViewRow, string> rowText = r => r == null ? "-" : Convert.ToString(r.Cells["colCode"].Value) + "|" + Convert.ToString(r.Cells["colDefectId"].Value);
                DataGridViewRow lastChain = dgvChains.Rows.Cast<DataGridViewRow>().LastOrDefault(r => Convert.ToString(r.Cells["colDefectId"].Value).Length > 0)
                    ?? dgvChains.Rows[dgvChains.Rows.Count - 1];
                dgvChains.ClearSelection();
                dgvChains.CurrentCell = lastChain.Cells["colCode"];
                lastChain.Selected = true;
                string tag1 = rowText(lastChain);
                Application.DoEvents();
                string status1 = b.StatusText;
                Invoke(typeA, a, "OpenDetail", d2.X, d2.Y);
                WaitFor(() => ShowsDie(d2.X, d2.Y), 60);
                var w2 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a, d2.X, d2.Y);
                string status2 = b.StatusText;
                // 2026-10-19: Chain 표는 [GDS / KLARF] 기능 탭에 있어 새 창(처음 탭 = 종합 (Die))에서는 숨어 있다. 숨은 표의 첫 표시 행은 -1 = 스크롤 없음
                int chainSelected = dgvChains.SelectedRows.Count, chainFirst = dgvChains.Rows.Count == 0 ? 0 : dgvChains.FirstDisplayedScrollingRowIndex;
                bool freshList = chainSelected == 0 && chainFirst <= 0;
                w1.Activate();
                bool back1 = WaitFor(() => ShowsDie(d1.X, d1.Y) && w1.HasSharedDetail, 30);
                Check("새 창은 앞 창 상태줄 / Chain 고른 행 · 스크롤 이어받지 않음 / 돌아가면 그 창 상태줄 · Chain 행",
                    status1.StartsWith("Defect #") && status2 != status1 && status2.StartsWith("Die (" + d2.X + ", " + d2.Y + ")") && freshList
                    && back1 && b.StatusText == status1 && dgvChains.CurrentRow != null && rowText(dgvChains.CurrentRow) == tag1,
                    "w1 \"" + status1 + "\" / w2 \"" + status2 + "\" / w2 Chain 새 목록 " + freshList + " (선택 " + chainSelected + " / 첫 행 " + chainFirst + ") / w1 되돌림 \"" + b.StatusText + "\""
                    + " / w1 행 " + rowText(dgvChains.CurrentRow) + " (기대 " + tag1 + ")");

                // ---- 11-2) 재분석(FRONT 보기) -> 이전 결과 창 탭 제목 "(이전 분석)" / 미리 읽기 캐시 2칸 ----
                a.ShowAnalysisResult(resFront);  // 분석 순번 2
                typeA.GetField("_lastRequest", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a, reqFront);
                DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Preload(a, resFront);
                Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                string[] keys = NexplantQMS.GdsMap.GdsDieMapCache.Keys;
                Check("재분석 -> 이전 결과 창 탭 제목 (이전 분석) / 미리 읽기 캐시 2칸(FRONT · BACK)",
                    w1.Text == "상세보기 (" + d1.X + ", " + d1.Y + ") (이전 분석)" && w2.Text == "상세보기 (" + d2.X + ", " + d2.Y + ") (이전 분석)"
                    && keyBack != keyFront && keys.Length == 2 && keys[0] == keyFront && keys[1] == keyBack && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1,
                    w1.Text + " , " + w2.Text + " / 캐시 " + keys.Length);

                // ---- 11-3) 도면 키가 다른 창끼리 오가기: DB 다시 읽지 않음(도면만 다시 만듦) ----
                var watch = Stopwatch.StartNew();
                Invoke(typeA, a, "OpenDetail", d3.X, d3.Y);
                bool shown3 = WaitFor(() => ShowsDie(d3.X, d3.Y), 120);
                var w3 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.FindWindow(a, d3.X, d3.Y);
                double open3 = watch.Elapsed.TotalSeconds;
                watch.Restart();
                w1.Activate();
                bool toBack = WaitFor(() => ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyBack, 120);
                double backSeconds = watch.Elapsed.TotalSeconds;
                watch.Restart();
                w3.Activate();
                bool toFront = WaitFor(() => ShowsDie(d3.X, d3.Y) && b.LoadedMapKey == keyFront, 120);
                double frontSeconds = watch.Elapsed.TotalSeconds;
                Check("도면 키 다른 창끼리 오가기 = DB 다시 읽지 않음(캐시 2칸) / 각 창 자기 도면", shown3 && toBack && toFront && w3.Text == "상세보기 (" + d3.X + ", " + d3.Y + ")"
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1,
                    String.Format("새 창 {0:0.0}초 / BACK 창으로 {1:0.0}초 / FRONT 창으로 {2:0.0}초 / DB 읽기 {3}", open3, backSeconds, frontSeconds, NexplantQMS.GdsMap.GdsDieMapCache.LoadCount - loads0));

                // ---- 11-4) 다른 키 도면을 읽는 중에 지금 도면과 같은 키 창으로 바로 넘어가기: 늦게 끝난 읽기가 덮지 않는다 ----
                w1.Activate();
                WaitFor(() => ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyBack, 120);
                NexplantQMS.GdsMap.GdsDieMapCache.Clear();   // FRONT 를 DB 에서 읽게 한다
                w3.Activate();
                bool started = WaitFor(() => b.IsBusy, 10);
                w2.Activate();
                WaitFor(() => NexplantQMS.GdsMap.GdsDieMapCache.Pending != null && NexplantQMS.GdsMap.GdsDieMapCache.Pending.IsCompleted, 120);
                Pump(1500);
                int w2Defects = w2.Analysis.Tables["DEFECT"].Rows.Cast<DataRow>().Count(r => DiePoint(r) == d2);
                Check("다른 키 도면 읽는 중 같은 키 창으로 전환 -> 늦게 끝난 읽기가 그 창 도면을 덮지 않음",
                    started && w2.HasSharedDetail && ShowsDie(d2.X, d2.Y) && b.LoadedMapKey == keyBack && b.DefectMarkerCount == w2Defects && !b.IsBusy,
                    "읽기 시작 " + started + " / 도면 키 " + (b.LoadedMapKey == keyBack ? "BACK" : b.LoadedMapKey == keyFront ? "FRONT" : "-")
                    + " / Defect 표시 " + b.DefectMarkerCount + " (w2 " + w2Defects + ")");
                int loadsRace = NexplantQMS.GdsMap.GdsDieMapCache.LoadCount;
                w3.Activate();
                bool after = WaitFor(() => ShowsDie(d3.X, d3.Y) && b.LoadedMapKey == keyFront, 120);
                Check("버린 읽기 데이터는 캐시에 남음(그 창으로 가면 DB 다시 읽지 않음)", after && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loadsRace, "");

                // ---- 11-5) 재분석 뒤 같은 Die 다시 열기 = 그 창에 새 결과 + 제목 되돌림, 다른 창은 (이전 분석) 그대로 ----
                Invoke(typeA, a, "OpenDetail", d1.X, d1.Y);
                bool reopened = WaitFor(() => ShowsDie(d1.X, d1.Y) && w1.HasSharedDetail && b.LoadedMapKey == keyFront, 120);
                Check("재분석 뒤 같은 Die 다시 = 그 창 새 결과(FRONT) / 제목 되돌림 / 다른 창 (이전 분석) 그대로",
                    reopened && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 3 && w1.Text == "상세보기 (" + d1.X + ", " + d1.Y + ")"
                    && w1.Analysis.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString() == "FRONT" && w2.Text.EndsWith("(이전 분석)"),
                    w1.Text + " " + w1.Analysis.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"] + " / " + w2.Text);

                foreach (Form f in DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.ToArray()) f.Dispose();
                typeA.GetMethod("OnFormClosed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(a, new object[] { new FormClosedEventArgs(CloseReason.UserClosing) });
                Check("메시지 루프 검사 정리: 창 / 화면 A 닫으면 공유 화면 · 캐시 해제", DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == null && b.IsDisposed
                    && NexplantQMS.GdsMap.GdsDieMapCache.Keys.Length == 0, "");
                a.Dispose();
            }
        }
        finally
        {
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = null;
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = null;
        }
    }

    /// <summary>RO 생성자(원격 URL 읽기)를 거치지 않고 원격 연결 필드만 BSL 로 채운다.</summary>
    private static T Ro<T>(object bsl) where T : class
    {
        var ro = (T)FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return ro;
    }

    private static Point DiePoint(DataRow r)
    {
        return new Point(Convert.ToInt32(r["XINDEX"]), Convert.ToInt32(r["YINDEX"]));
    }

    private static void Pump(int milliseconds)
    {
        var w = Stopwatch.StartNew();
        while (w.ElapsedMilliseconds < milliseconds) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    /// <summary>조건이 맞을 때까지 메시지를 돌리며 기다린다. 시간 안에 맞으면 true.</summary>
    private static bool WaitFor(Func<bool> done, int seconds)
    {
        var w = Stopwatch.StartNew();
        while (!done())
        {
            if (w.Elapsed.TotalSeconds > seconds) return false;
            Application.DoEvents();
            System.Threading.Thread.Sleep(20);
        }
        return true;
    }

    /// <summary>공유 상세 화면이 (x, y) Die 를 다 그렸는지(도면 조회 끝 + 위쪽 Die 정보 = 그 Die).</summary>
    private static bool ShowsDie(int x, int y)
    {
        var shared = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm as NexplantQMS.GdsMap.GdsDieAnalysisForm;
        if (shared == null || shared.IsBusy || shared.LoadedMapKey == null) return false;
        var info = (Label)Get(typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm), shared, "lblDieInfo");
        return info.Text.StartsWith("Die (" + x + ", " + y + ")  Defect 전체");
    }

    /// <summary>UI 스레드 await 가 이어지도록 메시지를 돌리며 기다린다.</summary>
    private static void Wait(Task task, int seconds)
    {
        var w = Stopwatch.StartNew();
        while (!task.IsCompleted)
        {
            if (w.Elapsed.TotalSeconds > seconds) throw new TimeoutException("화면 B 대기 시간 초과");
            Application.DoEvents();
            System.Threading.Thread.Sleep(20);
        }
        if (task.IsFaulted) throw task.Exception.InnerException;
    }

    /// <summary>
    /// 칸 가운데를 마우스 왼쪽 버튼으로 누른다(DataGridView.OnMouseDown / OnMouseUp 을 직접 호출).
    /// 사용자 클릭과 같은 경로(OnCellMouseDown -> SetCurrentCellAddressCore(throughMouseClick))를 탄다.
    /// </summary>
    private static void ClickCell(DataGridView grid, int column, int row)
    {
        Rectangle r = grid.GetCellDisplayRectangle(column, row, false);
        if (r.IsEmpty) throw new InvalidOperationException("칸이 화면에 없음: " + column + ", " + row);
        var e = new MouseEventArgs(MouseButtons.Left, 1, r.X + r.Width / 2, r.Y + r.Height / 2, 0);
        MethodInfo down = typeof(Control).GetMethod("OnMouseDown", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo up = typeof(Control).GetMethod("OnMouseUp", BindingFlags.NonPublic | BindingFlags.Instance);
        try
        {
            down.Invoke(grid, new object[] { e });
            up.Invoke(grid, new object[] { e });
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    private static object Get(Type type, object target, string name)
    {
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null) throw new MissingFieldException(type.Name, name);
        return f.GetValue(target);
    }

    /// <summary>비공개 메서드 호출. 같은 이름의 오버로드(예: OpenDetail(x, y) / OpenDetail(x, y, chain))는 인자 수로 고른다.</summary>
    private static void Invoke(Type type, object target, string name, params object[] args)
    {
        type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == name && m.GetParameters().Length == args.Length).Invoke(target, args);
    }

    private static void Check(string name, bool ok, string detail)
    {
        checks++;
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }
}
