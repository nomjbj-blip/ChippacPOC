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
/// 인자: [0] 화면 A 이미지(png), [1] STEP_SEQ, [2] Factory, [3] GDS Device, [4] capture 면 화면 B 아래 E-Test 값 표 그림(2026-10-22, [0] 폴더 images_STEP)
/// </summary>
internal static class GdsKlarfAnalysisFormSmoke
{
    private static int failures;
    private static int checks;
    /// <summary>화면 B 그림 폴더(capture 인자일 때만, 아니면 null).</summary>
    private static string captureDir;

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
        if (args.Length > 4 && args[4] == "capture")
        {
            captureDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(png), "images_" + stepSeq);
            System.IO.Directory.CreateDirectory(captureDir);
        }
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
            DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.ShowErrorDialogs = false;
            // 상세보기 Die 목록 Index 열(2026-10-22)이 원격 호출 대신 같은 프로세스 BSL 로 Map 사용 Die 를 읽게 한다
            DACrux.SEMDMS.ENGUI.GdsDieIndex.Service = new DACrux.SEMDMS.RO.GdsEtResult(new DACrux.SEMDMS.BSL.GdsEtResult());
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
            // 검사 면 콤보 없음(KLARF 검사 면 FS 고정, Ready VIEW_SIDE = FRONT), GDS / KLARF / E-Test 탭 위쪽 제목에 괄호 설명 없음 (2026-10-22)
            {
                BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
                var stepInfo = (Label)Get(typeA, form, "lblStepInfo");
                var pitch = (Label)Get(typeA, form, "lblPitchSource");
                DataRow pickedStep = ready.Tables["READY_KLARF"].Rows.Cast<DataRow>().First(r => r["STEP_SEQ"].ToString() == stepSeq.ToString());
                string[] heads = { ((Label)Get(typeA, form, "lblReadyGds")).Text, ((Label)Get(typeA, form, "lblReadyStep")).Text, ((Label)Get(typeA, form, "lblReadyEt")).Text };
                bool gone = typeA.GetField("dgvReadyStep", f) == null && typeA.GetField("btnFindStep", f) == null && typeA.GetMethod("btnFindStep_Click", f) == null
                    && typeA.GetField("cboStepSide", f) == null && typeA.GetField("lblStepSide", f) == null;
                Check("KLARF 탭: Step 표 · [직접 찾기] · 검사 면 콤보 없음 / 고른 Step 정보 / Ready 검사 면 FRONT / Pitch 출처 / 탭 제목 3개 괄호 설명 없음",
                    gone && stepInfo.Text.StartsWith("Step " + pickedStep["STEP_ID"] + "  /  Lot " + pickedStep["LOT_ID"]) && stepInfo.Text.Contains("Defect " + pickedStep["DEFECTS"])
                    && !stepInfo.Text.Contains("검사 면") && pickedStep["VIEW_SIDE"].ToString() == "FRONT" && pitch.Text == "Pitch 출처: Map Setup"
                    && heads.All(h => !h.Contains("(")) && !heads.Any(h => h.Contains("같은 Wafer")),
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
            Check("화면 배치: 기능 탭 GDS / KLARF / E-Test / 종합 · 결과 하위 탭 / 준비 탭 · 점검 표 없음 / 색 구간 표 = 팝업 / 지금 기준 요약 · Map 범례 없음 / Chain 면 기본 BS / Wafer 받으면 KLARF 탭",
                mainTabs.SequenceEqual(new[] { "GDS", "KLARF", "E-Test", "종합" })
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
            // [분석] 뒤 (2026-10-19 결정 L4) 처음 결과면 [종합] 탭. Map 범례는 없고(2026-10-20) 색 구간 / Die 수는 [색 구간 설정] 팝업 표에서만 본다
            var mapHost = splitMainPanel(form, typeA);
            bool noLegend = mapHost.Controls.OfType<FlowLayoutPanel>().All(p => !p.Visible);
            Check("[분석] 뒤 종합 탭 / Map 범례 없음(색 구간은 팝업 표)", tabMain.SelectedTab.Text == "종합" && noLegend && dgvBand.Rows.Count > 0,
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
                bool checkOk = ((Label)Get(typeA, form, "lblFailMark")).Text == "Map E-Test 표식" && chkSpec.Text == "Spec Out" && chkFile.Text == "Fail Die"
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
                string bothRows = "Spec Out Die " + specDies.Count + " | Fail Die " + fileDies.Count;
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
                bool fileOnlyOk = fileOnly == "Fail Die " + fileDies.Count && specSet.Count == 0 && fileSet.SetEquals(fileDies);
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
            // 상세 화면이 계산하는 결과 = 화면 마지막 요청으로 AnalyzeDie (Die 1개, Member 단위 DEFECT_HIT / E-Test 값 표). 2026-10-22: 상세보기 창이 직접 부른다
            var lastRequest = (DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest)Get(typeA, form, "_lastRequest");
            DataSet detailData = form.AnalysisService.AnalyzeDie(lastRequest, topX, topY, null);
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

            // ---- 3-4) 상세보기 창은 하나(2026-10-22 요청 "상세 화면이 열려 있는 상태에서 다른 die 를 더블클릭하면 열려 있는 화면 그대로 조회") /
            //      탭 제목 = 상세보기 (X, Y) = 지금 Die / 왼쪽 목록 = Defect Die, 지금 Die 선택 ----
            // (2026-10-12 의 "두 개가 뜨던 버그" = ShowForm 이 다른 어셈블리 사본을 만들던 것. Die 를 바꿔 열어도 창이 늘지 않아야 한다)
            // MDI 부모 + 같은 화면 A 를 하나 더 만들어 OpenDetail 을 세 번 부른다(요청 없음 = Wafer 결과 그대로). 상세 화면의 Shown(GDS DLL 로드)은 메시지를 돌리기 전에 닫아 실행하지 않는다.
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
                string title2 = details2.Length > 0 ? details2[0].Text : "";
                Invoke(typeA, a2, "OpenDetail", topX, topY);
                Form[] details3 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                var dw = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current;
                int defectDies = result.Tables["DIE"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0
                    || (!r.IsNull("ET_FAIL_CHAIN_COUNT") && Convert.ToInt32(r["ET_FAIL_CHAIN_COUNT"]) > 0));
                Check("상세보기 창 하나(다른 Die · 같은 Die 다시 열어도 창 늘지 않음) / 탭 제목 = 상세보기(지금 Die 좌표) / 목록 = Defect · E-Test Fail Die",
                    details1.Length == 1 && details2.Length == 1 && details3.Length == 1 && details2[0] == details1[0] && details3[0] == details1[0] && dw == details1[0]
                    && title1 == "상세보기 (" + topX + ", " + topY + ")" && title2 == "상세보기 (" + second.X + ", " + second.Y + ")" && dw.Text == title1
                    && dw.DieX == topX && dw.DieY == topY && dw.ListedDies.Count == defectDies && dw.AnalyzeCount == 0,
                    details1.Length + " -> " + details2.Length + " -> " + details3.Length + " / " + title1 + " , " + title2 + " , " + (dw == null ? "-" : dw.Text)
                    + " / 목록 " + (dw == null ? -1 : dw.ListedDies.Count) + " (" + defectDies + ")");
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
                && sqInputs && numTol.Enabled && sqStatus.Contains("중심 사각형 한 변 10") && !sqStatus.Contains("µm"), sq["USE_DEFECT_SIZE"] + " / " + sq["TOLERANCE_UM"] + " / " + sqStatus.Split('\n')[0]);
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
            // 2026-10-28 [걸린 / Fail Chain 만] 기본 켬 -> 처음 열면 이 Die 에서 걸린 / E-Test Fail Chain 만 그린다(둘 다 없는 Die 는 모든 Chain)
            int allChains = dieData.Tables["CHAIN"].Rows.Count;
            int hitChains = dieData.Tables["DIE_CHAIN"].Rows.Cast<DataRow>()
                .Where(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY
                    && (Convert.ToInt32(r["DEFECT_COUNT"]) > 0 || (r.Table.Columns.Contains("ET_STATUS") && r["ET_STATUS"].ToString() == "F")))
                .Select(r => r["CHAIN_SEQ"].ToString()).Distinct().Count();
            int startChains = hitChains > 0 ? hitChains : allChains;
            Check("화면 B Chain 표시 = 걸린 / Fail Chain 만([걸린 / Fail Chain 만] 기본 켬, 둘 다 없으면 모든 Chain)", detail.VisibleChainCount == startChains,
                detail.VisibleChainCount + " / 걸린 · Fail " + hitChains + " / 모든 Chain " + allChains + " (Die Chain " + dieChains + ")");
            // [전체 표시](2026-10-23): 처음 = 표의 모든 Chain 표시라 켬 / 끄면 표시 체크 모두 끔 · 도면 0 / 켜면 다시 모두 / Chain 하나를 끄면 꺼지고 다시 켜면 켬
            // (2026-10-28 [걸린 / Fail Chain 만] 기본 켬: "모두" = 표에 있는 Chain 묶음 수)
            {
                Type typeAll = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
                var chkAllB = (CheckBox)Get(typeAll, detail, "chkShowAll");
                var gridAllB = (DataGridView)Get(typeAll, detail, "dgvChainDefects");
                Func<DataGridViewRow, bool> isHead = r => r.Tag != null && (bool)r.Tag.GetType().GetField("First").GetValue(r.Tag)
                    && r.Tag.GetType().GetField("ChainSeq").GetValue(r.Tag) != null;
                Func<int> shownHeads = () => gridAllB.Rows.Cast<DataGridViewRow>().Count(r => isHead(r) && r.Cells["colShow"].Value is bool && (bool)r.Cells["colShow"].Value);
                int headCount = gridAllB.Rows.Cast<DataGridViewRow>().Count(isHead);
                bool startOn = chkAllB.Visible && chkAllB.Checked && chkAllB.Text == "전체 표시" && headCount > 0 && shownHeads() == headCount;
                chkAllB.Checked = false;
                Application.DoEvents();
                bool allOff = shownHeads() == 0 && detail.VisibleChainCount == 0;
                chkAllB.Checked = true;
                Application.DoEvents();
                bool allOn = shownHeads() == headCount && detail.VisibleChainCount == headCount;
                // Chain 하나 끄기 / 켜기 = 표시 칸 클릭과 같은 처리(값 바꾼 뒤 CellContentClick)
                DataGridViewRow oneB = gridAllB.Rows.Cast<DataGridViewRow>().First(isHead);
                int showCol = gridAllB.Columns["colShow"].Index;
                oneB.Cells[showCol].Value = false;
                Invoke(typeAll, detail, "dgvChainDefects_CellContentClick", gridAllB, new DataGridViewCellEventArgs(showCol, oneB.Index));
                bool oneOff = !chkAllB.Checked && detail.VisibleChainCount == headCount - 1;
                oneB.Cells[showCol].Value = true;
                Invoke(typeAll, detail, "dgvChainDefects_CellContentClick", gridAllB, new DataGridViewCellEventArgs(showCol, oneB.Index));
                bool oneBack = chkAllB.Checked && detail.VisibleChainCount == headCount;
                Check("화면 B [전체 표시]: 처음 켬(모든 Chain 표시) / 끄면 표시 체크 모두 끔 · 도면 Chain 0 / 켜면 모두 · 도면 전부 / Chain 하나 끄면 꺼짐 · 다시 켜면 켬",
                    startOn && allOff && allOn && oneOff && oneBack,
                    "Chain " + headCount + " / 처음 " + startOn + " / 끔 " + allOff + " / 켬 " + allOn + " / 하나 끔 " + oneOff + " / 다시 " + oneBack + " / 도면 " + detail.VisibleChainCount);
            }

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
            // 화면 B 도면의 Chain 색 = 저장 색(E-Test Fail Chain 도 빨간색으로 덮지 않음), 역추적(Chain 지정 열기) / 해제해도 Chain 목록 순서 · 선택 행 그대로 (2026-10-16)
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
            // 표 더블클릭은 아무 일도 하지 않는다(2026-10-22 더블클릭 역추적 · 이미지 삭제) -> 역추적은 분석 화면에서 Chain 을 지정해 열 때만
            bool noDoubleClick = typeB2.GetMethod("dgvChainDefects_CellDoubleClick", BindingFlags.NonPublic | BindingFlags.Instance) == null;
            string pickSeq = dieData.Tables["CHAIN"].Rows.Cast<DataRow>().First(r => r["CHAIN_NAME"].ToString() == pickName)["CHAIN_SEQ"].ToString();
            Wait(detail.ShowDieAsync(dieData, topX, topY, pickSeq), 60);
            Application.DoEvents();
            bool focusKeep = groupOrder().SequenceEqual(orderBefore) && onPickHead() && detail.VisibleChainCount <= 1 && noDoubleClick;
            // [역추적 해제](2026-10-22 위쪽 칸 pnlTop 의 [다시 분석] 아래로 옮김): 실행 크기 높이 28 이상 · 위쪽 칸 안(잘리지 않음) · [다시 분석] 아래 오른쪽 끝 맞춤
            // · 위쪽 칸 다른 컨트롤과 겹치지 않음, 해제하면 숨김
            var btnClearB = (Button)Get(typeB2, detail, "btnClearFocus");
            var btnReanalyzeB = (Button)Get(typeB2, detail, "btnReanalyze");
            Control topB = (Control)Get(typeB2, detail, "pnlTop");
            Func<string> clearPlace = () => btnClearB.Width + "x" + btnClearB.Height + " @" + btnClearB.Left + "," + btnClearB.Top
                + " / 위쪽 칸 " + topB.ClientSize.Width + "x" + topB.ClientSize.Height + " / [다시 분석] 오른쪽 끝 " + btnReanalyzeB.Right + " 아래 끝 " + btnReanalyzeB.Bottom;
            Func<bool> clearPlaced = () => btnClearB.Visible && btnClearB.Parent == topB && btnClearB.Height >= 28 && btnClearB.Width >= 100
                && btnClearB.Left >= 0 && btnClearB.Right <= topB.ClientSize.Width && btnClearB.Bottom <= topB.ClientSize.Height
                && btnClearB.Top >= btnReanalyzeB.Bottom && Math.Abs(btnClearB.Right - btnReanalyzeB.Right) <= 12
                && topB.Controls.Cast<Control>().All(sib => sib == btnClearB || !sib.Visible || !sib.Bounds.IntersectsWith(btnClearB.Bounds));
            string clearSize = clearPlace();
            bool clearShown = clearPlaced();
            // 상세보기 호스트 패널 폭은 창 크기마다 다르다: 화면 폭을 줄이거나 늘려도 [다시 분석] 아래 그대로 · 잘림 없음
            Size clearSizeKeep = detail.Size;
            var clearWidths = new List<string>();
            bool clearResize = true;
            foreach (int clearW in new[] { 1100, 1700 })
            {
                detail.Width = clearW;
                Application.DoEvents();
                bool clearOk = clearPlaced();
                clearResize &= clearOk;
                clearWidths.Add(clearW + "px " + (clearOk ? "OK" : "어긋남 " + clearPlace()));
            }
            detail.Size = clearSizeKeep;
            Application.DoEvents();
            typeB2.GetMethod("btnClearFocus_Click", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(detail, new object[] { null, EventArgs.Empty });
            Application.DoEvents();
            bool clearKeep = groupOrder().SequenceEqual(orderBefore) && onPickHead() && !btnClearB.Visible;
            Check("화면 B 도면 Chain 색 = 저장 색 / 표 더블클릭 처리 없음 / Chain 지정 열기(역추적) · 해제 뒤 묶음 순서와 고른 Chain 묶음 그대로 / [역추적 해제] 위쪽 칸 [다시 분석] 아래 · 높이 28 이상 · 잘림 · 겹침 없음",
                detail.VisibleChainColors.Count > 0 && colorBad.Count == 0 && focusKeep && clearKeep && clearShown,
                "그린 Chain " + detail.VisibleChainColors.Count + " / 색 다름 " + colorBad.Count + " / 더블클릭 처리 없음 " + noDoubleClick + " / 역추적 " + pickName + " -> "
                + (dgvChainsB.CurrentRow == null ? "-" : Convert.ToString(dgvChainsB.CurrentRow.Cells["colCode"].Value)) + " / 역추적 " + focusKeep + " / 해제 " + clearKeep
                + " / [역추적 해제] " + clearShown + " " + clearSize);
            Check("화면 B [역추적 해제] 화면 폭을 바꿔도(1100 / 1700) [다시 분석] 아래 오른쪽 끝 맞춤 · 잘림 없음", clearResize, String.Join(" / ", clearWidths.ToArray()));

            // Defect 이미지 보기 = Defect 행 오른쪽 클릭 메뉴(2026-10-22, 예전 Defect 칸 더블클릭 대신): 그 행을 고르고 메뉴 [Defect 이미지 보기],
            // 메뉴를 누르면 호스트에 그 Defect 이미지 요청(호스트 없으면 상태줄 안내). Defect 없는 Chain 행은 메뉴 없음
            {
                var mnuDefectB = (ContextMenuStrip)Get(typeB2, detail, "mnuDefect");
                DataGridViewRow defectRowB = dgvChainsB.Rows.Cast<DataGridViewRow>().Last(r => Convert.ToString(r.Cells["colDefectId"].Value).Length > 0);
                DataGridViewRow plainRowB = dgvChainsB.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Convert.ToString(r.Cells["colDefectId"].Value).Length == 0);
                int idCol = dgvChainsB.Columns["colDefectId"].Index;
                Func<DataGridViewRow, bool> rightClick = row =>
                {
                    Invoke(typeB2, detail, "dgvChainDefects_CellMouseClick", dgvChainsB,
                        new DataGridViewCellMouseEventArgs(idCol, row.Index, 5, 5, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0)));
                    Application.DoEvents();
                    bool menuOpen = mnuDefectB.Visible;
                    mnuDefectB.Close();
                    Application.DoEvents();
                    return menuOpen;
                };
                bool plainNoMenu = plainRowB == null || !rightClick(plainRowB);
                dgvChainsB.ClearSelection();
                bool menuShown = rightClick(defectRowB);
                bool rowPicked = defectRowB.Selected && dgvChainsB.SelectedRows.Count == 1;
                string wantId = Convert.ToString(defectRowB.Cells["colDefectId"].Value);
                long gotId = -1;
                detail.DefectImageRequested = (imageStep, imageId) => gotId = imageId;
                Invoke(typeB2, detail, "mnuDefectImage_Click", mnuDefectB.Items[0], EventArgs.Empty);
                detail.DefectImageRequested = null;
                Invoke(typeB2, detail, "mnuDefectImage_Click", mnuDefectB.Items[0], EventArgs.Empty);
                bool noHostNote = detail.StatusText.StartsWith("이미지는 DMS 안에서 열었을 때만");
                Check("화면 B Defect 행 오른쪽 클릭 = 그 행 고름 + [Defect 이미지 보기] 메뉴 -> 호스트에 그 Defect 이미지 요청(호스트 없으면 안내) / Defect 없는 행은 메뉴 없음",
                    menuShown && rowPicked && mnuDefectB.Items.Count == 1 && mnuDefectB.Items[0].Text == "Defect 이미지 보기" && gotId.ToString() == wantId && noHostNote && plainNoMenu,
                    "메뉴 " + menuShown + " / 행 고름 " + rowPicked + " / 요청 Defect " + gotId + " (행 " + wantId + ") / 호스트 없음 안내 " + noHostNote + " / 빈 행 메뉴 없음 " + plainNoMenu);
            }

            // 화면 B 배치 (2026-10-22, 설계 2026-10-22_상세보기_오른쪽_한패널_설계.md): 오른쪽 탭 없이 한 패널
            // = 기준 묶음(GDS / KLARF 2줄 + E-Test 줄) / 머리 줄(체크 2개) / 가로 나누기 위 GDS · KLARF 표 + 아래 E-Test 값 표.
            // 2026-10-22 후속: 위쪽 오른쪽 [다시 분석], [Chain 밖 Defect] 체크 · 4분류 상태 칸 5개 삭제, 기준의 최종 저장 / 분석 시각 삭제
            // 2026-10-22 후속 2: 기준 E-Test 줄 늘 숨김, 도면 [전체 보기] 삭제 · [배경 도면] -> [전체 도면], 역추적 Chain 글자(lblFocus) 삭제
            {
                Func<string, Control> b = n => (Control)Get(typeB2, detail, n);
                var splitRightB = (SplitContainer)b("splitRight");
                var rightPanel = ((SplitContainer)b("splitMain")).Panel2;
                var btnReanalyze = (Button)b("btnReanalyze");
                bool parents = b("dgvChainDefects").Parent == splitRightB.Panel1 && b("dgvEtValues").Parent == splitRightB.Panel2
                    && splitRightB.Parent == rightPanel && b("pnlChainHead").Parent == rightPanel && b("pnlGdsCriteria").Parent == rightPanel
                    && b("chkShowAll").Parent == b("pnlChainHead") && b("lblEtText").Parent == b("pnlGdsCriteria")
                    && splitRightB.Orientation == Orientation.Horizontal && b("map").Parent == ((SplitContainer)b("splitMain")).Panel1
                    && btnReanalyze.Parent == b("pnlTop") && btnReanalyze.Visible && btnReanalyze.Text == "다시 분석" && btnReanalyze.Image != null
                    && (btnReanalyze.Anchor & AnchorStyles.Right) != 0 && btnReanalyze.Right > b("pnlTop").Width - 40;
                // 호스트 없이 연 상세 화면(이 Smoke)의 [다시 분석] = 상태줄 안내만, 호스트가 채우는 요청 속성 = System.Action
                Invoke(typeB2, detail, "btnReanalyze_Click", btnReanalyze, EventArgs.Empty);
                bool reanalyzeAlone = detail.StatusText.StartsWith("다시 분석은 DMS 분석 화면에서 연 상세보기에서만")
                    && typeB2.GetProperty("ReanalyzeRequested") != null && typeB2.GetProperty("ReanalyzeRequested").PropertyType == typeof(Action);
                bool noTimes = !b("lblGdsText").Text.Contains("최종 저장") && !b("lblGdsText").Text.Contains(" · 분석 ");
                bool titles = b("lblGdsCriteria").Text == "GDS / KLARF 기준" && b("lblGdsText").Text.Contains(Environment.NewLine + "Step ") && noTimes && reanalyzeAlone
                    && b("lblSumTitle").Text == "분석 기준" && b("lblSumGds").Text == "GDS · Chain BS -> 반전" && b("lblGdsText").Text.Contains(" · Chain 면 BS · Chain ") && !b("lblGdsText").Text.Contains("검사 면") && b("lblSumKlarf").Text.StartsWith("KLARF · Step 1개 · 크기 + 허용오차 5 · 보정 (0, 0)")
                    && b("lblSumEt").Text.StartsWith("E-Test · ") && b("lblMapSide").Text == "보기 = " + NexplantQMS.GdsMap.Persistence.GdsSideTransform.Text(dieData.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString());
                bool oldGone = new[] { "lblCriteria", "tabRight", "tabGds", "tabKlarf", "dgvChains", "dgvDefects", "cboScope", "lblKlarfCriteria",
                    "tabMain", "tabChainDefect", "tabEt", "tabDie", "dgvSummary", "lblDieText", "lblDieCriteria", "pnlEtCriteria", "lblEtCriteria", "colNear",
                    "chkOutside", "flowClass", "lblClassBoth", "lblClassFail", "lblClassDefect", "lblClassOk", "lblClassNa", "btnFit", "lblFocus", "chkEtValues" }
                    .All(n => typeB2.GetField(n, BindingFlags.NonPublic | BindingFlags.Instance) == null)
                    && typeB2.GetMethod("btnFit_Click", BindingFlags.NonPublic | BindingFlags.Instance) == null && b("chkBackground").Text == "전체 도면";
                // 이 Smoke 기본 결과는 E-Test 를 연결하지 않는다 -> 아래 표(접힘) 숨김. 기준 E-Test 줄은 늘 숨김([E-Test 값] 체크는 2026-10-23 삭제)
                bool noEtHidden = splitRightB.Panel2Collapsed && !b("lblEtText").Visible && b("pnlChainHead").Visible
                    && b("pnlGdsCriteria").Height == b("lblGdsText").Bottom + 4;
                // 두 화면 같은 모양(2026-10-22): 위쪽 분석 기준 칸 · 표 위 요약 글자 숨김, [걸린 / Fail Chain 만] 켬(2026-10-28 기본 켬), Defect 채움
                bool sameLook = new[] { "lblSumTitle", "lblSumGds", "lblSumKlarf", "lblSumEt", "lblChainTitle" }.All(n => !b(n).Visible)
                    && ((CheckBox)b("chkHitOnly")).Checked && gmap.DefectStyle == NexplantQMS.GdsMap.DefectOverlayStyle.Filled;
                Check("화면 B 배치: 오른쪽 탭 없음 · 한 패널(기준 묶음 / 머리 줄 / 위 GDS · KLARF 표 + 아래 E-Test 값 표) · 위쪽 오른쪽 [다시 분석](호스트 없으면 안내) · Chain 밖 체크 · 4분류 칸 · [전체 보기] 없음 · [전체 도면] · 기준에 최종 저장 / 분석 시각 · E-Test 줄 · [E-Test 값] 체크 없음 · 머리 줄에 [전체 표시] / E-Test 없는 결과 = 아래 표 숨김 / 두 화면 같은 모양",
                    parents && titles && oldGone && noEtHidden && sameLook,
                    "표 위치 · 버튼 " + parents + " / 기준 · 다시 분석 안내 " + titles + " / 옛 컨트롤 없음 " + oldGone + " / E-Test 없음 숨김 " + noEtHidden + " / 같은 모양 " + sameLook
                    + " / " + b("lblGdsText").Text.Replace(Environment.NewLine, " | ") + " / " + detail.StatusText);

                // 도면 머리 줄(2026-10-22): 도면 칸이 좁아도(Die 목록 옆 약 190px) 보기 / 전체 도면 / 원점 표시가 겹치지 않고 다음 줄로 넘어간다.
                // 한 줄 = 모든 항목의 줄 위치(Top - Margin.Top)가 같음([전체 보기] 삭제로 줄 높이가 버튼 기준 32 보다 낮아짐)
                {
                    var splitMainB = (SplitContainer)b("splitMain");
                    Control head = b("pnlMapHead");
                    int keepDistance = splitMainB.SplitterDistance;
                    Func<string> overlap = () =>
                    {
                        List<Control> items = head.Controls.Cast<Control>().Where(c => c.Visible).ToList();
                        var bad = new List<string>();
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].Left < 0 || items[i].Right > head.ClientSize.Width) bad.Add(items[i].Name + " 밖");
                            for (int j = i + 1; j < items.Count; j++)
                                if (items[i].Bounds.IntersectsWith(items[j].Bounds)) bad.Add(items[i].Name + " x " + items[j].Name);
                        }
                        return String.Join(",", bad);
                    };
                    Func<int> headRows = () => head.Controls.Cast<Control>().Where(c => c.Visible).Select(c => c.Top - c.Margin.Top).Distinct().Count();
                    string wideBad = overlap();
                    int wideHeight = head.Height, wideRows = headRows();
                    bool wideOk = wideBad.Length == 0 && wideRows == 1 && b("map").Top == head.Bottom;
                    splitMainB.SplitterDistance = 190;
                    Pump(300);
                    string narrowBad = overlap();
                    int narrowHeight = head.Height, narrowRows = headRows();
                    bool narrowOk = narrowBad.Length == 0 && narrowRows > 1 && narrowHeight > wideHeight && b("map").Top == head.Bottom;
                    CaptureDetail(detail, "B0_상세_도면칸좁음.png");
                    splitMainB.SplitterDistance = keepDistance;
                    Pump(300);
                    bool restored = head.Height == wideHeight && headRows() == 1 && overlap().Length == 0;
                    Check("화면 B 도면 머리 줄: 넓으면 한 줄, 도면 칸 190px 이면 다음 줄로 넘어가 겹침 · 잘림 없음, 도면은 머리 줄 바로 아래, 넓히면 다시 한 줄",
                        wideOk && narrowOk && restored,
                        "넓음 " + head.Parent.Width + "px " + wideRows + "줄 높이 " + wideHeight + (wideBad.Length == 0 ? "" : " 겹침 " + wideBad)
                        + " / 좁음 190px " + narrowRows + "줄 높이 " + narrowHeight + (narrowBad.Length == 0 ? "" : " 겹침 " + narrowBad) + " / 되돌림 " + restored);
                }

                // 용어: Chain 칸(표시 · 색 · Chain · Defect 수) + Defect 칸(Defect ID · RoughBin · Class · 크기 · XREL · YREL), 4분류 '-' 없음.
                // E-Test 없는 결과는 비어 있는 분류 / ET / Fail Para 열을 숨긴다(2026-10-22, E-Test 연결 결과는 EtPanelChecks 에서 열 보임 확인).
                // Member / Gap / GDS X / GDS Y 열은 숨김(2026-10-22 요청, 값은 채워 둠)
                string[] gridHeads = dgvChainsB.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray();
                List<DataGridViewRow> chainRows = dgvChainsB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToString(r.Cells["colCode"].Value) != "Chain 밖 Defect").ToList();
                string[] classes = chainRows.Select(r => Convert.ToString(r.Cells["colClass"].Value)).Distinct().ToArray();
                Check("화면 B 용어 = 화면 A: Chain 칸 4열 + Defect 칸 6열(가까운 Chain · Member · Gap · GDS X / Y 열 없음), E-Test 없는 결과는 분류 / ET / Fail Para 열 숨김, 분류 값 '-' 없음, 정렬 안 됨",
                    gridHeads.SequenceEqual(new[] { "표시", "색", "Chain", "Defect 수",
                        "Defect ID", "RoughBin", "Class", "크기", "XREL", "YREL" })
                    && classes.All(k => new[] { "일치", "Fail만", "Defect만", "정상", "비교 불가" }.Contains(k))
                    && chainRows.All(r => Convert.ToString(r.Cells["colMember"].Value).Length > 0)
                    && dgvChainsB.Columns.Cast<DataGridViewColumn>().All(c => c.SortMode == DataGridViewColumnSortMode.NotSortable),
                    String.Join(",", gridHeads) + " / 분류 " + String.Join(",", classes));

                // 합친 표 묶음 규칙 (2026-10-20, 설계 2026-10-20_상세보기_GDS_KLARF_합친탭_설계.md 2.1절):
                // Chain 묶음 행 = 그 Chain 에 걸린 Defect(CHAIN_CODES) 수(없으면 1행), 맨 아래 Chain 밖 묶음 = IN_CHAIN N Defect(가까운 거리 순, 거리 없음 뒤),
                // 머리 줄 = Chain n / Defect 전체 / Chain In / Chain Out(글자는 숨김). Chain 밖 묶음은 늘 있다(2026-10-22 체크 삭제)
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
                    bool outShown = outCount == 0 || dgvChainsB.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToString(r.Cells["colCode"].Value) == "Chain 밖 Defect") == outCount;
                    Check("화면 B [GDS / KLARF] 묶음: Chain 묶음 Defect = 그 Chain 에 걸린 Defect / 맨 아래 Chain 밖 = Chain Out(가까운 거리 순, 늘 보임) / 머리 줄 수",
                        groupErrors.Count == 0 && outLastSorted && titleOk && outShown,
                        "묶음 " + String.Join(",", groups) + " / 어긋남 " + String.Join(",", groupErrors) + " / 순서 " + outLastSorted + " / \"" + title + "\" / Chain 밖 " + outShown);
                }

                // 두 Chain 에 걸린 Defect (2026-10-20): 두 묶음 모두 + Defect ID 굵게, 다른 Defect 는 굵지 않음("가까운 Chain" 열은 2026-10-22 삭제).
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
                            && twoRows.Any(r => Convert.ToString(r.Cells["colCode"].Value) == twoFirstName)
                            && twoRows.Any(r => Convert.ToString(r.Cells["colCode"].Value) == twoOtherName)
                            && !dgvChainsB.Columns.Contains("colNear");
                        twoText = "Defect " + id + " / " + twoFirstName + " + " + twoOtherName + " / 행 " + twoRows.Count + " / 굵게 " + twoRows.Count(bold) + " / 다른 Defect 굵지 않음 " + othersPlain
                            + " / " + String.Join(" | ", twoRows.Select(r => Convert.ToString(r.Cells["colCode"].Value)).ToArray());
                        Wait(detail.ShowDieAsync(dieData, topX, topY), 60);
                        Application.DoEvents();
                    }
                    Check("화면 B 두 Chain 에 걸린 Defect: 두 묶음 모두 / Defect ID 굵게 / 가까운 Chain 열 없음 (복사본 결과, 되돌림)", twoOk, twoText);
                }

                // Chain 이 많은 Device (2026-10-28): Chain 1,000개 초과여도 [걸린 / Fail Chain 만]을 끄면 [GDS / KLARF] 표는 모든 Chain
                // (10-21 의 그 Die DIE_CHAIN Chain 만 넣던 예외는 2026-10-28 요청 "다 보여줘라"로 삭제). 가짜 Chain 1,000개를 더한 복사본으로 확인하고 원래 결과로 되돌린다(가짜 Chain 은 Member 없음).
                {
                    DataSet bigCopy = dieData.Copy();
                    DataTable chainT = bigCopy.Tables["CHAIN"];
                    DataRow sample = chainT.Rows[0];
                    for (int i = 0; i < 1000; i++)
                    {
                        DataRow fake = chainT.NewRow();
                        fake.ItemArray = sample.ItemArray;
                        fake["CHAIN_SEQ"] = 900000000L + i;
                        fake["CHAIN_CODE"] = "ZZ" + i;
                        fake["CHAIN_NAME"] = "SMOKE_FAKE_" + i;
                        fake["MEMBER_COUNT"] = 0;
                        chainT.Rows.Add(fake);
                    }
                    var hitOnlyB = (CheckBox)b("chkHitOnly");
                    bool hitOnlyBefore = hitOnlyB.Checked;
                    // 도면 키(Chain 목록)가 바뀌어 도면을 다시 읽는다. Application.Run 없이 DoEvents 만 돌리면 동기화 문맥이 풀려
                    // 읽기 뒤 그리기가 다른 스레드에서 실행되므로(OpenGL 오류 170) 부르기 전에 다시 설정한다.
                    System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
                    Wait(detail.ShowDieAsync(bigCopy, topX, topY), 300);
                    Application.DoEvents();
                    hitOnlyB.Checked = false;
                    Application.DoEvents();
                    var allNames = new HashSet<string>(chainT.Rows.Cast<DataRow>().Select(r => r["CHAIN_NAME"].ToString()));
                    var shownNames = new HashSet<string>(dgvChainsB.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["colCode"].Value))
                        .Where(n => n != "Chain 밖 Defect"));
                    string bigTitle = b("lblChainTitle").Text;
                    string bigStatus = detail.StatusText;
                    bool bigOk = shownNames.SetEquals(allNames) && bigTitle.StartsWith("Chain " + chainT.Rows.Count + " /") && !bigTitle.Contains("(이 Die 주변)")
                        && dgvChainsB.Rows.Count >= chainT.Rows.Count && bigStatus.StartsWith("도면 조회 완료");
                    string bigText = "Chain " + chainT.Rows.Count + " / 표 Chain " + shownNames.Count
                        + " / 행 " + dgvChainsB.Rows.Count + " / \"" + bigTitle + "\" / " + bigStatus;
                    hitOnlyB.Checked = hitOnlyBefore;
                    System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
                    Wait(detail.ShowDieAsync(dieData, topX, topY), 300);
                    Application.DoEvents();
                    Check("화면 B Chain 1,000개 초과 Device: [걸린 / Fail Chain 만] 끄면 표 = 모든 Chain / 머리 줄 (이 Die 주변) 없음 (복사본 결과, 되돌림)", bigOk, bigText);
                }

                // 열기 (2026-10-22 탭 없음): 역추적 = 그 Chain 묶음 첫 행을 고르고 아래 E-Test 값 범위 = 그 Chain, Die 전체 = 고른 행 없음 + 아래 = Die 전체
                var lblEtCountB = (Label)b("lblEtCount");
                string firstName = Convert.ToString(dgvChainsB.Rows[0].Cells["colCode"].Value);
                string focusSeq = dieData.Tables["CHAIN"].Rows.Cast<DataRow>().First(r => r["CHAIN_NAME"].ToString() == firstName)["CHAIN_SEQ"].ToString();
                Wait(detail.ShowDieAsync(dieData, topX, topY, focusSeq), 60);
                Application.DoEvents();
                bool focusOpen = dgvChainsB.SelectedRows.Count == 1 && Convert.ToString(dgvChainsB.SelectedRows[0].Cells["colCode"].Value) == firstName
                    && lblEtCountB.Text.StartsWith("E-Test 값 · Chain " + firstName + " (");
                string focusText = lblEtCountB.Text;
                Wait(detail.ShowDieAsync(dieData, topX, topY, null), 60);
                Application.DoEvents();
                bool plainOpen = dgvChainsB.SelectedRows.Count == 0 && lblEtCountB.Text.StartsWith("E-Test 값 · Die 전체 (");
                Check("화면 B 열기: 역추적 = 그 Chain 묶음 첫 행 + 아래 E-Test 값 범위 그 Chain / Die 전체 = 고른 행 없음 + 아래 Die 전체", focusOpen && plainOpen,
                    focusText + " / " + lblEtCountB.Text);

                // 아래 E-Test 값 표 (2026-10-22, 설계 3절): 이 Wafer 의 READY E-Test 파일을 연결한 결과로 확인하고 원래 결과로 되돌린다. 파일이 없으면 참고만 남긴다.
                {
                    DataSet ready = null;
                    try { ready = analysisBsl.GetWaferReady(factory, stepSeq, device); }
                    catch (Exception ex) { Console.WriteLine("참고 / E-Test 준비 목록 조회 실패: " + ex.Message); }
                    DataRow etFile = ready == null ? null : ready.Tables["READY_ET"].Rows.Cast<DataRow>()
                        .Where(r => { long v; return long.TryParse(r["SRC_FILE_SEQ"].ToString(), out v) && v > 0; })
                        .OrderBy(r => r["SELECTED"].ToString() == "Y" ? 0 : 1).FirstOrDefault();
                    // 준비 목록은 GDS Device 와 같은 Device 파일만 둔다. 같은 Wafer 파일이 다른 Device 이름이면(ET_OTHER_DEVICE) 파일 목록에서 같은 Wafer 파일을 쓴다
                    // (서버 판정은 Parameter -> GDS Chain 이름 연결이라 화면 검사에는 쓸 수 있다)
                    DataRow otherDevice = ready == null ? null : ready.Tables["READY_CHECK"].Rows.Cast<DataRow>().FirstOrDefault(r => r["CODE"].ToString() == "ET_OTHER_DEVICE");
                    if (etFile == null && otherDevice != null && ready.Tables["WAFER_KEY"].Rows.Count > 0)
                    {
                        // 점검 문구 "... 목록에서 뺌(OMMTCB TSVET, ...)" 의 괄호 안 = "Device 공정" 목록 -> 첫 단어가 Device
                        string message = otherDevice["MESSAGE"].ToString();
                        string waferKey = ready.Tables["WAFER_KEY"].Rows[0]["WAFER_KEY"].ToString();
                        var etBsl = new DACrux.SEMDMS.BSL.GdsEtResult();
                        foreach (string etDevice in message.Substring(message.LastIndexOf('(') + 1).TrimEnd(')').Split(',')
                            .Select(s => s.Trim().Split(' ')[0]).Where(s => s.Length > 0).Distinct())
                        {
                            try
                            {
                                etFile = etBsl.GetEtFileBrowseList(factory, etDevice, "", "").Rows.Cast<DataRow>()
                                    .FirstOrDefault(r => String.Equals(r["WAFER_ID"].ToString(), waferKey, StringComparison.OrdinalIgnoreCase));
                            }
                            catch (Exception ex) { Console.WriteLine("참고 / E-Test 파일 목록 조회 실패(" + etDevice + "): " + ex.Message); }
                            if (etFile == null) continue;
                            Console.WriteLine("참고 / 같은 Wafer E-Test 파일이 다른 Device(" + etDevice + ")라 파일 목록에서 찾아 씀");
                            break;
                        }
                    }
                    if (etFile == null) Console.WriteLine("참고 / 이 Wafer 에 READY E-Test 파일이 없어 아래 E-Test 값 표 검사는 건너뜀 (Step " + stepSeq + ")");
                    else EtPanelChecks(detail, typeB2, analysisBsl, baseRequest, ready, etFile);
                }
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

        // ---- 10) 상세보기 창 하나 + 왼쪽 Die 목록 + GDS 상세 화면(도면) 하나 + [분석] 때 미리 읽기 + 분석 화면과 동기화
        //      (2026-10-16 설계 Die상세_여러화면_GDS재사용, 2026-10-22 설계 Die상세_DLL로딩_Die목록) ----
        // MDI 부모에 화면 A(띄우지 않음: Load 의 원격 조회를 피함)를 붙이고 실제 경로(OpenDetail -> 상세보기 창 Open -> AnalyzeDie -> 창 Shown / Activated)를 탄다.
        // 호스트가 만드는 공유 화면은 기본 서비스(DefaultMapService / DefaultChainService)를 같은 프로세스 BSL 로 바꿔 쓴다.
        {
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            NexplantQMS.GdsMap.GdsDieMapCache.Clear();
            DataRow secondDie = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            Point p2 = secondDie != null ? DiePoint(secondDie) : new Point(topX, topY);
            Func<int> windows = () => DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count;
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
                var w1 = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current;
                bool shown1 = WaitFor(() => ShowsDie(topX, topY), 180);
                double firstSeconds = watch.Elapsed.TotalSeconds;
                var shared = (NexplantQMS.GdsMap.GdsDieAnalysisForm)DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm;
                Check("첫 상세보기 = 공유 화면 + 미리 읽은 도면(DB 다시 읽지 않음)", shown1 && w1 != null && w1.HasSharedDetail
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && shared.LoadedMapKey == keyWafer,
                    firstSeconds.ToString("0.0") + "초 / " + (shared == null ? "공유 화면 없음" : shared.StatusText));
                if (!shown1 || shared == null) throw new InvalidOperationException("첫 상세보기가 표시되지 않아 이후 검사를 멈춤");
                var dgvDiesW = (DataGridView)Get(typeof(DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis), w1, "dgvDies");
                Func<Point, DataGridViewRow> listRow = p => dgvDiesW.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (Point)r.Tag == p);
                Func<Point?> listPick = () => dgvDiesW.SelectedRows.Count == 1 ? (Point?)(Point)dgvDiesW.SelectedRows[0].Tag : null;
                // Die 마다 보기 상태: [걸린 / Fail Chain 만] 체크(2026-10-22 [Chain 밖 Defect] 체크가 없어져 이 체크로 본다)
                var chkHitOnlyB = (CheckBox)Get(typeB, shared, "chkHitOnly");
                var gmapB = (NexplantQMS.GdsMap.GdsMapControl)Get(typeB, shared, "map");
                IntPtr mapHandle = gmapB.Handle;
                // Die 1 보기 바꾸기: 걸린 / Fail Chain 만 켬, 확대 위치 이동
                chkHitOnlyB.Checked = true;
                double cx, cy, scale, label;
                gmapB.GetView(out cx, out cy, out scale, out label);
                double view1X = cx + 37, view1Y = cy - 21, view1Scale = scale * 1.7;
                gmapB.SetView(view1X, view1Y, view1Scale, label);

                // 열린 채로 두 번째 Die 더블클릭: 같은 창(새 창 없음) / 공유 화면 · 도면 핸들 그대로 / 도면 다시 읽지 않음 / 탭 제목 · 목록 선택 = 그 Die
                watch.Restart();
                Invoke(typeA, a3, "OpenDetail", p2.X, p2.Y);
                bool shown2 = WaitFor(() => ShowsDie(p2.X, p2.Y), 60);
                double secondSeconds = watch.Elapsed.TotalSeconds;
                Check("열린 채로 두 번째 Die 더블클릭 = 같은 창(창 1개, 새 창 없음) / 공유 화면 · 도면 핸들 그대로 / 도면 다시 읽지 않음 / 탭 제목 · 목록 선택 = 그 Die",
                    shown2 && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current == w1 && windows() == 1
                    && w1.HasSharedDetail && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == shared && gmapB.Handle == mapHandle
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && shared.LoadedMapKey == keyWafer && secondSeconds < 5
                    && w1.Text == "상세보기 (" + p2.X + ", " + p2.Y + ")" && listPick() == p2 && w1.AnalyzeCount == 2,
                    secondSeconds.ToString("0.00") + "초 / 창 " + windows() + " / " + w1.Text + " / 목록 선택 " + listPick() + " / 계산 " + w1.AnalyzeCount);
                chkHitOnlyB.Checked = false;   // Die 2 = 걸린 / Fail Chain 만 끔
                gmapB.GetView(out cx, out cy, out scale, out label);
                double view2X = cx, view2Y = cy, view2Scale = scale;

                // 목록에서 Die 오가기(선택 0.3초 뒤 반영): 서버 다시 안 부름 / Die 마다 보던 걸린 / Fail Chain 만 · 확대 위치로 되돌림
                int a0 = w1.AnalyzeCount;
                dgvDiesW.CurrentCell = listRow(new Point(topX, topY)).Cells["colNo"];
                bool back1 = WaitFor(() => ShowsDie(topX, topY), 30);
                gmapB.GetView(out cx, out cy, out scale, out label);
                bool view1Ok = Math.Abs(cx - view1X) < 0.01 && Math.Abs(cy - view1Y) < 0.01 && Math.Abs(scale / view1Scale - 1) < 1e-6;
                bool hit1 = chkHitOnlyB.Checked;
                dgvDiesW.CurrentCell = listRow(p2).Cells["colNo"];
                bool back2 = WaitFor(() => ShowsDie(p2.X, p2.Y), 30);
                gmapB.GetView(out cx, out cy, out scale, out label);
                bool view2Ok = Math.Abs(cx - view2X) < 0.01 && Math.Abs(cy - view2Y) < 0.01 && Math.Abs(scale / view2Scale - 1) < 1e-6;
                bool hit2 = chkHitOnlyB.Checked;
                Check("목록에서 Die 오가기 = 서버 다시 안 부름 / Die 마다 걸린 / Fail Chain 만 · 확대 위치 되돌림 / 도면 다시 읽지 않음 / 창 1개",
                    back1 && back2 && w1.AnalyzeCount == a0 && view1Ok && view2Ok && hit1 && !hit2 && windows() == 1
                    && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1 && gmapB.Handle == mapHandle,
                    "Die1 걸린 Chain 만 " + hit1 + " 위치 " + view1Ok + " / Die2 걸린 Chain 만 " + hit2 + " 위치 " + view2Ok + " / 다시 계산 " + (w1.AnalyzeCount - a0));

                // E-Test Fail 역추적으로 Die 를 열면 같은 창에서 다시 계산해 역추적 Chain 을 보인다
                DataRow focusChain = result.Tables["CHAIN"].Rows[0];
                Invoke(typeA, a3, "OpenDetail", topX, topY, focusChain["CHAIN_SEQ"].ToString());
                // 역추적 표시 = [역추적 해제] 보임 + 역추적 Chain 번호(2026-10-22 Chain 이름 글자 lblFocus 삭제)
                var btnClearShared = (Control)Get(typeB, shared, "btnClearFocus");
                Func<string> sharedFocusSeq = () => Convert.ToString(typeB.GetField("_focusChainSeq", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(shared));
                bool focused = WaitFor(() => ShowsDie(topX, topY) && w1.HasSharedDetail && btnClearShared.Visible, 30);
                Check("역추적으로 Die 열기 = 같은 창(창 1개) / 다시 계산 / 역추적 Chain 표시", focused && windows() == 1 && w1.AnalyzeCount == a0 + 1
                    && sharedFocusSeq() == focusChain["CHAIN_SEQ"].ToString(), "역추적 Chain " + sharedFocusSeq() + " / [역추적 해제] " + btnClearShared.Visible);

                // 분석 화면이 다른 조건(허용오차 + 3)으로 다시 분석 -> 열린 상세보기도 같이 새 결과(2026-10-22 동기화): 같은 창 · 같은 Die · 분석 순번 같음 · 새 허용오차
                var request2 = (DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest)typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(baseRequest, null);
                request2.ToleranceUm = baseRequest.ToleranceUm + 3;
                DataSet result2 = analysisBsl.Analyze(request2);
                int a1 = w1.AnalyzeCount;
                typeA.GetField("_lastRequest", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a3, request2);
                a3.ShowAnalysisResult(result2);
                int version3 = (int)typeA.GetField("_analysisVersion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(a3);
                bool synced = WaitFor(() => ShowsDie(topX, topY), 60);
                double tol2 = Convert.ToDouble(w1.Analysis.Tables["ANL_INFO"].Rows[0]["TOLERANCE_UM"]);
                Check("분석 화면이 다시 분석하면 열린 상세보기도 같이 새로 계산(더블클릭 없이) / 같은 창 · 같은 Die / 분석 순번 같음 / 새 허용오차 / 목록 = 새 결과",
                    synced && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current == w1 && windows() == 1 && w1.AnalysisVersion == version3 && w1.AnalyzeCount == a1 + 1
                    && Math.Abs(tol2 - Convert.ToDouble(request2.ToleranceUm)) < 1e-9 && w1.Text == "상세보기 (" + topX + ", " + topY + ")"
                    && listPick() == new Point(topX, topY) && w1.ListedDies.Count == ListedDieCount(result2),
                    "허용오차 " + tol2 + " (" + request2.ToleranceUm + ") / 순번 " + w1.AnalysisVersion + " (" + version3 + ") / 다시 계산 " + (w1.AnalyzeCount - a1)
                    + " / 목록 " + w1.ListedDies.Count + " (" + ListedDieCount(result2) + ")");

                // 상세보기 창을 닫아도 화면 A 가 열려 있으면 공유 화면 / 미리 읽기 유지(D4) -> 화면 A 를 닫으면 해제(메모리 반환)
                w1.Dispose();
                Application.DoEvents();
                bool keptAll = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == shared && !shared.IsDisposed && NexplantQMS.GdsMap.GdsDieMapCache.Key == keyWafer;
                typeA.GetMethod("OnFormClosed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(a3, new object[] { new FormClosedEventArgs(CloseReason.UserClosing) });
                bool released = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm == null && shared.IsDisposed && NexplantQMS.GdsMap.GdsDieMapCache.Key == null;
                Check("상세보기 창을 닫아도 화면 A 가 있으면 공유 화면 유지 / 화면 A 닫으면 공유 화면 · 미리 읽기 해제", keptAll && released,
                    "유지 " + keptAll + " / 해제 " + released);
                a3.Dispose();
            }
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = null;
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = null;
        }
        // ---- 11) 메시지 루프 안 검사 (2026-10-16, 2026-10-22 창 하나 기준): DACrux 처럼 Application.Run 안에서 상세보기 Die / 결과 바꾸기 간섭을 본다 ----
        // 콘솔에서 DoEvents 로만 돌리면 도면 키가 다른 결과로 바꿀 때 화면 스레드 문맥이 풀려 OpenGL MakeCurrent 가 실패한다(Error 170, 시험 환경 문제).
        // 기본 결과(Chain BS · 검사 FS = 반전, BACK 보기)와 Chain FS 재분석(반전 없음, FRONT 보기)으로 도면 키가 다른 결과를 오간다 (2026-10-20 KLARF 검사 면 FS 고정)
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
    /// 11절: 상세보기 간섭 (2026-10-16 점검 결과 1 ~ 4 수정 확인, 2026-10-22 창 하나 + 목록 + 분석 화면 동기화 기준으로 바꿈).
    /// BACK 보기 결과(Chain BS / 검사 FS)와 FRONT 보기 재분석 결과(Chain FS / 검사 FS)를 화면 A 에 번갈아 넣어 도면 키가 다른 결과를 오간다 (2026-10-20 KLARF 검사 면 FS 고정).
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
        List<Point> dies = resBack.Tables["DIE"].Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0).Select(DiePoint).Distinct().Take(2).ToList();
        if (dies.Count < 2)
        {
            Check("메시지 루프 안 검사: Defect Die 2개", false, dies.Count.ToString());
            return;
        }
        Point d1 = dies[0], d2 = dies[1];
        string keyBack = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(resBack);
        string keyFront = NexplantQMS.GdsMap.GdsDieAnalysisForm.MapKeyOf(resFront);
        FieldInfo lastRequest = typeA.GetField("_lastRequest", BindingFlags.NonPublic | BindingFlags.Instance);
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
                lastRequest.SetValue(a, reqBack);
                a.ShowAnalysisResult(resBack);   // 분석 순번 1 (열린 상세보기 없음)
                DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Preload(a, resBack);
                Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                int loads0 = NexplantQMS.GdsMap.GdsDieMapCache.LoadCount;

                // ---- 11-1) 목록에서 다른 Die 로 가면 앞 Die 의 상태줄 / Chain 고른 행 / 스크롤을 이어받지 않는다. 돌아가면 그 Die 값 ----
                Invoke(typeA, a, "OpenDetail", d1.X, d1.Y);
                WaitFor(() => ShowsDie(d1.X, d1.Y), 180);
                var w = DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current;
                var b = (NexplantQMS.GdsMap.GdsDieAnalysisForm)DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.SharedDetailForm;
                var dgvDiesW = (DataGridView)Get(typeof(DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis), w, "dgvDies");
                Func<Point, DataGridViewRow> listRow = p => dgvDiesW.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (Point)r.Tag == p);
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
                dgvDiesW.CurrentCell = listRow(d2).Cells["colNo"];
                WaitFor(() => ShowsDie(d2.X, d2.Y), 60);
                string status2 = b.StatusText;
                int chainSelected = dgvChains.SelectedRows.Count, chainFirst = dgvChains.Rows.Count == 0 ? 0 : dgvChains.FirstDisplayedScrollingRowIndex;
                bool freshList = chainSelected == 0 && chainFirst <= 0;
                dgvDiesW.CurrentCell = listRow(d1).Cells["colNo"];
                bool back1 = WaitFor(() => ShowsDie(d1.X, d1.Y), 30);
                Check("목록에서 다른 Die = 앞 Die 상태줄 / Chain 고른 행 · 스크롤 이어받지 않음 / 돌아가면 그 Die 상태줄 · Chain 행",
                    status1.StartsWith("Defect #") && status2 != status1 && status2.StartsWith("Die (" + d2.X + ", " + d2.Y + ")") && freshList
                    && back1 && b.StatusText == status1 && dgvChains.CurrentRow != null && rowText(dgvChains.CurrentRow) == tag1,
                    "d1 \"" + status1 + "\" / d2 \"" + status2 + "\" / d2 Chain 새 목록 " + freshList + " (선택 " + chainSelected + " / 첫 행 " + chainFirst + ") / d1 되돌림 \"" + b.StatusText + "\""
                    + " / d1 행 " + rowText(dgvChains.CurrentRow) + " (기대 " + tag1 + ")");

                // ---- 11-2) 분석 화면이 FRONT 보기로 다시 분석 -> 열린 상세보기도 FRONT 결과 · 도면(동기화) / 미리 읽기 캐시 2칸 / DB 읽기 1번 ----
                lastRequest.SetValue(a, reqFront);
                DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Preload(a, resFront);   // 화면 A FinishAnalysis 처럼 반영 전에 미리 읽기 시작
                a.ShowAnalysisResult(resFront);  // 분석 순번 2 -> 상세보기 동기화
                bool toFront = WaitFor(() => ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyFront, 120);
                string[] keys = NexplantQMS.GdsMap.GdsDieMapCache.Keys;
                Check("FRONT 보기로 다시 분석 -> 열린 상세보기도 FRONT 결과 · 도면(같은 창 · 같은 Die) / 미리 읽기 캐시 2칸 / DB 읽기 1번(미리 읽기를 같이 씀)",
                    toFront && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.Current == w && DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis.OpenWindows.Count == 1
                    && w.Analysis.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString() == "FRONT" && w.Text == "상세보기 (" + d1.X + ", " + d1.Y + ")"
                    && keyBack != keyFront && keys.Length == 2 && keys[0] == keyFront && keys[1] == keyBack && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1,
                    w.Text + " " + w.Analysis.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"] + " / 캐시 " + keys.Length + " / DB 읽기 " + (NexplantQMS.GdsMap.GdsDieMapCache.LoadCount - loads0));

                // ---- 11-3) 다시 BACK 보기로 분석 -> BACK 도면, DB 다시 읽지 않음(캐시 2칸) ----
                var watch = Stopwatch.StartNew();
                lastRequest.SetValue(a, reqBack);
                a.ShowAnalysisResult(resBack);   // 분석 순번 3
                bool toBack = WaitFor(() => ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyBack, 120);
                Check("BACK 보기로 다시 분석 -> 열린 상세보기 BACK 도면 / DB 다시 읽지 않음(캐시 2칸)", toBack && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loads0 + 1
                    && w.Analysis.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString() == resBack.Tables["ANL_INFO"].Rows[0]["VIEW_SIDE"].ToString(),
                    String.Format("{0:0.0}초 / DB 읽기 {1}", watch.Elapsed.TotalSeconds, NexplantQMS.GdsMap.GdsDieMapCache.LoadCount - loads0));

                // ---- 11-4) 다른 키 도면을 읽는 중에 지금 도면과 같은 키 결과로 다시 분석: 늦게 끝난 읽기가 덮지 않는다 ----
                NexplantQMS.GdsMap.GdsDieMapCache.Clear();   // FRONT 를 DB 에서 읽게 한다
                lastRequest.SetValue(a, reqFront);
                a.ShowAnalysisResult(resFront);
                bool started = WaitFor(() => b.IsBusy, 10);
                lastRequest.SetValue(a, reqBack);
                a.ShowAnalysisResult(resBack);
                WaitFor(() => NexplantQMS.GdsMap.GdsDieMapCache.Pending != null && NexplantQMS.GdsMap.GdsDieMapCache.Pending.IsCompleted, 120);
                Pump(1500);
                int d1Defects = w.Analysis.Tables["DEFECT"].Rows.Cast<DataRow>().Count(r => DiePoint(r) == d1);
                Check("다른 키 도면 읽는 중 같은 키 결과로 다시 분석 -> 늦게 끝난 읽기가 지금 도면을 덮지 않음",
                    started && ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyBack && b.DefectMarkerCount == d1Defects && !b.IsBusy,
                    "읽기 시작 " + started + " / 도면 키 " + (b.LoadedMapKey == keyBack ? "BACK" : b.LoadedMapKey == keyFront ? "FRONT" : "-")
                    + " / Defect 표시 " + b.DefectMarkerCount + " (" + d1Defects + ")");
                int loadsRace = NexplantQMS.GdsMap.GdsDieMapCache.LoadCount;
                lastRequest.SetValue(a, reqFront);
                a.ShowAnalysisResult(resFront);
                bool after = WaitFor(() => ShowsDie(d1.X, d1.Y) && b.LoadedMapKey == keyFront, 120);
                Check("버린 읽기 데이터는 캐시에 남음(다시 그 결과로 가면 DB 다시 읽지 않음)", after && NexplantQMS.GdsMap.GdsDieMapCache.LoadCount == loadsRace, "");

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

    /// <summary>상세보기 왼쪽 목록에 들어갈 Die 수: Defect 1개 이상 또는 E-Test Fail Chain 1개 이상.</summary>
    private static int ListedDieCount(DataSet ds)
    {
        return ds.Tables["DIE"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0
            || (r.Table.Columns.Contains("ET_FAIL_CHAIN_COUNT") && !r.IsNull("ET_FAIL_CHAIN_COUNT") && Convert.ToInt32(r["ET_FAIL_CHAIN_COUNT"]) > 0));
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

    /// <summary>
    /// 화면 B 아래 E-Test 값 표 (2026-10-22, 설계 2026-10-22_상세보기_오른쪽_한패널_설계.md 3절): 이 Wafer 의 READY E-Test 파일을 연결한 결과(BSL 직접)로
    /// [E-Test 값] 체크 · 아래 표 범위(Die 전체 / 고른 행 Chain / Chain 밖) · 아래 행 고르기 · 접기 · 4분류 칸 · 분류 글자색 · 기준 E-Test 줄을 본다.
    /// 다음 검사가 E-Test 없는 결과로 다시 연다.
    /// </summary>
    private static void EtPanelChecks(NexplantQMS.GdsMap.GdsDieAnalysisForm detail, Type typeB, DACrux.SEMDMS.BSL.GdsKlarfAnalysis analysisBsl,
        DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest baseRequest, DataSet ready, DataRow etFile)
    {
        Func<string, Control> b = n => (Control)Get(typeB, detail, n);
        var request = new DACrux.SEMDMS.Interface.GdsKlarfAnalysisRequest
        {
            Factory = baseRequest.Factory, DeviceId = baseRequest.DeviceId, StepSeqs = baseRequest.StepSeqs, ViewSides = baseRequest.ViewSides,
            ChainSide = baseRequest.ChainSide, DisplayStepSeq = baseRequest.DisplayStepSeq, AreaMode = baseRequest.AreaMode, ToleranceUm = baseRequest.ToleranceUm,
            ClassBasis = baseRequest.ClassBasis, EtSrcFileSeq = long.Parse(etFile["SRC_FILE_SEQ"].ToString()),
            WaferKey = ready.Tables["WAFER_KEY"].Rows.Count > 0 ? ready.Tables["WAFER_KEY"].Rows[0]["WAFER_KEY"].ToString() : null
        };
        DataSet wafer = analysisBsl.Analyze(request);
        // E-Test 값이 있는 Die: 일치 Chain 이 있는 Die -> Fail Chain 이 있는 Die -> E-Test 판정이 있는 Die
        DataRow pickDc = wafer.Tables["DIE_CHAIN"].Rows.Cast<DataRow>()
            .OrderBy(r => r["CLASS4"].ToString() == "BOTH" ? 0 : r["ET_STATUS"].ToString() == "F" ? 1 : r["ET_STATUS"].ToString().Length > 0 ? 2 : 3).FirstOrDefault();
        if (pickDc == null || pickDc["ET_STATUS"].ToString().Length == 0)
        {
            Console.WriteLine("참고 / E-Test 판정이 있는 Die 가 없어 아래 E-Test 값 표 검사는 건너뜀 (SRC_FILE_SEQ " + request.EtSrcFileSeq + ")");
            return;
        }
        int x = Convert.ToInt32(pickDc["XINDEX"]), y = Convert.ToInt32(pickDc["YINDEX"]);
        DataSet die = analysisBsl.AnalyzeDie(request, x, y, null);
        List<DataRow> etRows = die.Tables["ET_VALUE"].Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["XINDEX"]) == x && Convert.ToInt32(r["YINDEX"]) == y).ToList();
        Console.WriteLine("E-Test 연결: SRC_FILE_SEQ " + request.EtSrcFileSeq + " / " + etFile["TEST_PROGRAM"] + " / Die (" + x + ", " + y + ") / E-Test 값 " + etRows.Count);
        System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
        Wait(detail.ShowDieAsync(die, x, y, null), 300);
        Application.DoEvents();
        var split = (SplitContainer)b("splitRight");
        var grid = (DataGridView)b("dgvChainDefects");
        var et = (DataGridView)b("dgvEtValues");
        var head = (Label)b("lblEtCount");
        Func<DataGridViewRow, object> seqOf = r => r.Tag == null ? null : r.Tag.GetType().GetField("ChainSeq").GetValue(r.Tag);

        // 1) 보이기 + 고른 행 없음 = Die 전체
        // 기준 E-Test 줄은 2026-10-22 부터 E-Test 가 있어도 숨김(글자는 채움). 아래 표 높이 = 오른쪽의 35%(1/4 이상이면 통과)
        bool shown = !split.Panel2Collapsed && !b("lblEtText").Visible && b("lblEtText").Text.StartsWith("E-Test 공정 ")
            && new[] { "colClass", "colEt", "colFailPara" }.All(n => grid.Columns[n].Visible) && split.Panel2.Height * 4 >= split.Height;
        bool dieAll = grid.SelectedRows.Count == 0 && et.Rows.Count == etRows.Count && head.Text.StartsWith("E-Test 값 · Die 전체 (" + etRows.Count + ")");
        Check("화면 B E-Test 연결 결과: 아래 표 펼침 · 분류 / ET / Fail Para 열 보임 · 기준 E-Test 줄 숨김 / 고른 행 없음 = 아래 표 Die 전체 Parameter", shown && dieAll,
            "보임 " + shown + " / 아래 표 높이 " + split.Panel2.Height + " / " + split.Height + " / " + head.Text + " / 표 " + et.Rows.Count + " (" + etRows.Count + ") / " + b("lblEtText").Text);

        // 2) 위 표 Chain 행 고르기 = 그 Chain 의 이 Die Parameter 만(Fail 먼저)
        DataGridViewRow chainRow = grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => seqOf(r) != null && etRows.Any(e => e["CHAIN_SEQ"].ToString() == seqOf(r).ToString()));
        if (chainRow != null)
        {
            string seq = seqOf(chainRow).ToString();
            string name = Convert.ToString(chainRow.Cells["colCode"].Value);
            int want = etRows.Count(e => e["CHAIN_SEQ"].ToString() == seq);
            grid.ClearSelection();
            chainRow.Selected = true;
            Application.DoEvents();
            List<string> flags = et.Rows.Cast<DataGridViewRow>().Select(r => Convert.ToString(r.Cells["판정"].Value)).ToList();
            bool scoped = et.Rows.Count == want && et.Rows.Cast<DataGridViewRow>().All(r => Convert.ToString(r.Cells["SEQ"].Value) == seq)
                && head.Text.StartsWith("E-Test 값 · Chain " + name + " (" + want + ")") && flags.SkipWhile(f => f == "F").All(f => f != "F");
            Check("화면 B 위 표 Chain 행 고르기 = 아래 표 = 그 Chain 의 이 Die Parameter(Fail 먼저) / 머리 글자", scoped, head.Text + " / 표 " + et.Rows.Count + " (" + want + ")");
            CaptureDetail(detail, "B1_상세_Chain고름_아래ETest값.png");

            // 3) 아래 행 고르기 = 상태줄 Parameter / GDS Chain / 값 / 판정. 그 GDS Chain 도형을 노랗게 칠하지 않는다(2026-10-22 요청, 도면 선택 도형 0)
            if (et.Rows.Count > 0)
            {
                et.ClearSelection();
                et.Rows[0].Selected = true;
                Application.DoEvents();
                var picked = (System.Collections.ICollection)typeof(NexplantQMS.GdsMap.GdsMapControl)
                    .GetField("_selectedItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(b("map"));
                Check("화면 B 아래 E-Test 값 행 고르기 = 상태줄 Parameter / GDS Chain / 값 / 판정 / 도면 Chain 노랗게 칠하지 않음",
                    detail.StatusText.StartsWith(Convert.ToString(et.Rows[0].Cells["Parameter"].Value) + " / ") && picked.Count == 0,
                    detail.StatusText + " / 칠한 도형 " + picked.Count);
            }
        }
        else Console.WriteLine("참고 / 이 Die 위 표에 E-Test 값이 있는 Chain 행이 없음");

        // 4) Chain 밖 Defect 행 = 빈 표
        DataGridViewRow outRow = grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Tag != null && seqOf(r) == null);
        if (outRow != null)
        {
            grid.ClearSelection();
            outRow.Selected = true;
            Application.DoEvents();
            Check("화면 B Chain 밖 Defect 행 고르기 = 아래 표 빈 표 + 머리 \"E-Test 값 · Chain 밖 Defect\"", et.Rows.Count == 0 && head.Text == "E-Test 값 · Chain 밖 Defect",
                head.Text + " / 표 " + et.Rows.Count);
            CaptureDetail(detail, "B2_상세_Chain밖고름.png");
        }

        // 5) [E-Test 값] 체크 없음(2026-10-23): 같은 결과를 다시 보여도 아래 E-Test 값 표는 늘 펼침
        Type etFormType = detail.GetType();
        Wait(detail.ShowDieAsync(die, x, y, null), 300);
        Application.DoEvents();
        bool noEtCheck = etFormType.GetField("chkEtValues", BindingFlags.NonPublic | BindingFlags.Instance) == null
            && etFormType.GetMethod("chkEtValues_CheckedChanged", BindingFlags.NonPublic | BindingFlags.Instance) == null;
        Check("화면 B [E-Test 값] 체크 없음 / E-Test 연결 결과는 다시 보여도 아래 표 늘 펼침", noEtCheck && !split.Panel2Collapsed && split.Panel2.Height * 4 >= split.Height,
            "체크 없음 " + noEtCheck + " / 아래 표 높이 " + split.Panel2.Height + " / " + split.Height);

        // 6) 분류 칸 글자색 = 예전 [종합] 표 색 (4분류 상태 칸 5개는 2026-10-22 삭제)
        var colors = new Dictionary<string, Color> { { "일치", Color.Firebrick }, { "Fail만", Color.DarkOrange }, { "Defect만", Color.FromArgb(150, 110, 0) } };
        List<DataGridViewRow> classRows = grid.Rows.Cast<DataGridViewRow>().Where(r => colors.ContainsKey(Convert.ToString(r.Cells["colClass"].Value))).ToList();
        bool colorOk = classRows.All(r => r.Cells["colClass"].Style.ForeColor.ToArgb() == colors[Convert.ToString(r.Cells["colClass"].Value)].ToArgb());
        Check("화면 B 분류 칸 글자색(일치 빨강 / Fail만 주황 / Defect만 진노랑)", colorOk,
            "색 행 " + classRows.Count + " (" + String.Join(",", classRows.Select(r => Convert.ToString(r.Cells["colClass"].Value)).Distinct().ToArray()) + ")");
    }

    /// <summary>
    /// capture 인자일 때 화면 B 를 그림으로 남긴다(2026-10-22). OpenGL 도면은 DrawToBitmap 으로 그려지지 않아 PrintWindow(PW_RENDERFULLCONTENT)로 찍고,
    /// 화면 밖(-32000)에서는 내용이 그려지지 않아 찍는 동안만 화면 왼쪽 위로 옮긴다(Simple Smoke 와 같은 방식).
    /// </summary>
    private static void CaptureDetail(Form detail, string name)
    {
        if (captureDir == null) return;
        Point keep = detail.Location;
        detail.Location = new Point(0, 0);
        Pump(1500);
        string file = System.IO.Path.Combine(captureDir, name);
        using (var bmp = new Bitmap(detail.Width, detail.Height))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hdc = g.GetHdc();
                try { PrintWindow(detail.Handle, hdc, 2); }
                finally { g.ReleaseHdc(hdc); }
            }
            bmp.Save(file, System.Drawing.Imaging.ImageFormat.Png);
        }
        detail.Location = keep;
        Application.DoEvents();
        Console.WriteLine("그림: " + file);
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, uint flags);

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
