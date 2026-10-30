using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using DACrux.SEMDMS.ENGUI;
using DACrux.SEMDMS.Interface;

/// <summary>
/// GDS-KLARF 연관 분석 Simple 화면(frmGdsKlarfSimple) Smoke 검사.
/// 설계: 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md 8절
/// 화면을 화면 밖에 실제로 띄워 Wafer 받기 -> 자동 1차 분석 -> Map 색 / 숫자 -> 종합 (Die) -> 색 구간 팝업 -> 설정 바꾸고 [분석]
/// -> Die 선택 -> 이미지 카드 흐름을 검사한다. Service(Remoting) 대신 RO 의 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꿔
/// 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다. DB 는 조회만 한다.
/// 2026-10-22 2단계: Die 더블클릭 상세보기(설계 15절)를 메시지 루프 안에서 실제 경로(MDI 부모 + 상세보기 창 + 공유 GDS 상세 화면)로 검사한다(DetailChecks).
/// 인자: [0] 결과 그림 폴더, [1] STEP_SEQ, [2] Factory, [3] GDS Device (= KLARF Product), [4] "capture" 면 상세보기 창을 잠깐 화면에 띄워 그림을 남긴다(선택)
/// </summary>
internal static class GdsKlarfSimpleFormSmoke
{
    private static int failures;
    private static int checks;
    /// <summary>기대 Die Index(고객 통합 Die#) Map: BSL 로 직접 읽은 Factory + Product 의 TQP_USEMAP 사용 Die. 없으면 null (2026-10-22).</summary>
    private static DieCoordinateMapper indexMap;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        string outDir = args[0];
        long stepSeq = long.Parse(args[1]);
        string factory = args[2];
        string device = args[3];
        Directory.CreateDirectory(outDir);
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";

        var defectBsl = new DACrux.SEMDMS.BSL.DefectMapAnalysis();
        var viewBsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var analysisBsl = new DACrux.SEMDMS.BSL.GdsKlarfAnalysis();
        var chainBsl = new DACrux.SEMDMS.BSL.GdsChain();
        bool capture = args.Length > 4 && args[4] == "capture";
        // E-Test / Wafer Key (2026-10-22 결정 "같은 Wafer 최신 파일 자동"): 화면과 같이 서버 준비 조회 READY_ET 의 SELECTED 파일(없으면 0)과 WAFER_KEY.
        // 아래 기준 요청도 같은 값을 넣어 화면 결과(Die 별 수)와 비교한다
        DataSet readyExpected = analysisBsl.GetWaferReady(factory, stepSeq, device);
        DataRow etExpected = readyExpected.Tables["READY_ET"].Rows.Cast<DataRow>()
            .Where(x => { long v; return long.TryParse(x["SRC_FILE_SEQ"].ToString(), out v) && v > 0; })
            .OrderBy(x => x["SELECTED"].ToString() == "Y" ? 0 : 1).FirstOrDefault();
        long etSeqExpected = etExpected == null ? 0 : Convert.ToInt64(etExpected["SRC_FILE_SEQ"]);
        string keyExpected = readyExpected.Tables["WAFER_KEY"].Rows.Count == 0 ? "" : readyExpected.Tables["WAFER_KEY"].Rows[0]["WAFER_KEY"].ToString();
        // 기준 = 설계 5.1절 요청(허용오차 0 / 5)을 BSL 로 직접 부른 결과
        Func<double, GdsKlarfAnalysisRequest> request = tol => new GdsKlarfAnalysisRequest
        {
            Factory = factory, DeviceId = device, WaferKey = keyExpected, StepSeqs = new[] { stepSeq }, ViewSides = new[] { "FRONT" }, ChainSide = "BACK",
            DisplayStepSeq = stepSeq, AreaMode = GdsKlarfAnalysisRequest.AreaSizeTolerance, ToleranceUm = tol, SquareUm = 10,
            ClassBasis = GdsKlarfAnalysisRequest.ClassBasisRoughBin, EtSrcFileSeq = etSeqExpected
        };
        DataSet expected0 = analysisBsl.Analyze(request(0));
        DataSet expected5 = analysisBsl.Analyze(request(5));
        DataRow info0 = expected0.Tables["ANL_INFO"].Rows[0];
        int in0 = Convert.ToInt32(info0["IN_CHAIN_DEFECT_COUNT"]);
        int in5 = Convert.ToInt32(expected5.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);
        DataTable imagePaths = defectBsl.GetDefectImagePath01(new[] { stepSeq });
        // Die Index: 화면은 GdsDieIndex.Service(같은 프로세스 BSL)로 읽고, 기대값은 BSL 조회 결과로 직접 만든다
        var etBsl = new DACrux.SEMDMS.BSL.GdsEtResult();
        GdsDieIndex.Service = new DACrux.SEMDMS.RO.GdsEtResult(etBsl);
        DataTable mapDies = etBsl.GetEtMapDieList(factory, info0["PRODUCT"].ToString());
        indexMap = mapDies.Rows.Cast<DataRow>().Any(x => x["USECODE"] != DBNull.Value && Convert.ToInt32(x["USECODE"]) == DieCoordinateMapper.UsedDieCode)
            ? DieCoordinateMapper.FromDieTable(mapDies, new DACrux.SEMDMS.Interface.DiePoint(0, 0)) : null;
        Console.WriteLine("BSL 기준: Defect " + info0["DEFECT_COUNT"] + " / Chain 위 (허용오차 0) " + in0 + " / (허용오차 5) " + in5
            + " / Chain " + info0["CHAIN_COUNT"] + " / 이미지 경로 " + imagePaths.Rows.Count
            + " / Index Map " + factory + " / " + info0["PRODUCT"] + " 사용 Die " + (indexMap == null ? 0 : indexMap.DieCount));

        Type t = typeof(frmGdsKlarfSimple);
        BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
        using (var form = new frmGdsKlarfSimple())
        {
            form.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(defectBsl);
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            form.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.ShowErrorDialogs = false;
            form.Show();
            Application.DoEvents();

            var map = (GdsKlarfAnalysisMap)Get(t, form, "map");
            var busyBar = (DACrux.SEMDMS.Control.ProgressStatusBar)Get(t, form, "busyBar");
            var lblInfo = (Label)Get(t, form, "lblInfo");
            var lblChanged = (Label)Get(t, form, "lblChanged");
            var rdoSize = (RadioButton)Get(t, form, "rdoDefectSize");
            var numTol = (NumericUpDown)Get(t, form, "numTolerance");
            var numSquare = (NumericUpDown)Get(t, form, "numSquare");
            var cboSide = (ComboBox)Get(t, form, "cboChainSide");
            var btnAnalyze = (Button)Get(t, form, "btnAnalyze");
            var dgvDie = (DataGridView)Get(t, form, "dgvDie");
            var flowImage = (FlowLayoutPanel)Get(t, form, "flowImage");
            var splitSetting = (SplitContainer)Get(t, form, "splitSetting");
            var lblImageInfo = (Label)Get(t, form, "lblImageInfo");
            var lblDeviceValue = (Label)Get(t, form, "lblDeviceValue");

            // ---- 1) 배치 / 기본값 ----
            // 2026-10-22: 오른쪽 탭 없음(한 페이지) / Chain 목록 · [Wafer 전체] · [Defect 있는 Chain 만] 없음 / 종합 표 = 위, Defect 이미지 = 맨 아래
            string[] gone = { "numOffsetX", "numOffsetY", "dgvReadyEt", "cboEtStep", "chkMarkSpec", "chkMarkFile", "btnFindEt", "dgvReadyGds",
                "lblWaferKey", "btnReadyRefresh", "dgvTolerance", "dgvDefect", "cboColorBasis", "btnClass", "btnChain", "lblTolUnit", "lblSquareUnit",
                "lblMapBasis", "pnlStatus", "lblStatus", "chkImageOut", "tabList", "tabSetting", "tabDie", "dgvChain", "chkChainAll", "chkChainHit",
                "lblChainCount", "pnlChainHead" };
            bool onePage = dgvDie.Parent == splitSetting.Panel1 && flowImage.Parent == splitSetting.Panel2 && splitSetting.Orientation == Orientation.Horizontal
                && splitSetting.Parent != null && splitSetting.Parent.Name == "pnlSetting";
            string[] present = gone.Where(n => t.GetField(n, f) != null).ToArray();
            MethodInfo popupFilter = typeof(GdsKlarfAnalysisMap).GetMethod("ctxmWaferMap_PopupFilter", f);
            popupFilter.Invoke(map, new object[] { null, EventArgs.Empty });
            List<string> simpleMenu = map.VisibleMenuItems();
            List<string> analysisMenu;
            using (var mapA = new GdsKlarfAnalysisMap())
            {
                popupFilter.Invoke(mapA, new object[] { null, EventArgs.Empty });
                analysisMenu = mapA.VisibleMenuItems();
            }
            Check("배치: 오른쪽 탭 없이 한 페이지(종합 표 위 / Defect 이미지 맨 아래) / Chain 목록 · 체크 2개 · E-Test · 좌표 보정 · 준비 표 · 허용오차 탭 · Defect 표 · 단위 Label · Map 위 색 기준 글자 · 아래 상태줄 · Chain 밖 체크 없음"
                + " / [다시 분석] / 진행 표시 = 위 막대(쉬는 동안 숨김) / 우클릭 [Detail Analysis] 보임(2026-10-22 2단계 상세보기 연결, 화면 A Map 과 같음)",
                onePage && present.Length == 0 && map.ShowDetailMenu
                && btnAnalyze.Text == "다시 분석" && busyBar.Parent != null && busyBar.Parent.Name == "pnlTop" && !busyBar.Visible
                && simpleMenu.Contains("Detail Analysis") && simpleMenu.Contains("Free Zoom") && simpleMenu.Contains("Center Cross") && analysisMenu.Contains("Detail Analysis"),
                "한 페이지 " + onePage + " / 남은 컨트롤 " + (present.Length == 0 ? "없음" : String.Join(",", present)) + " / Simple 메뉴 " + String.Join("|", simpleMenu.ToArray())
                + " / 화면 A 메뉴 " + String.Join("|", analysisMenu.ToArray()));
            Check("설정 기본값: Defect 크기 + 허용오차 0 µm / 정사각형 10 µm(꺼짐) / Chain 면 BS",
                rdoSize.Checked && numTol.Value == 0 && numTol.Enabled && numSquare.Value == 10 && !numSquare.Enabled && cboSide.Text == "BS",
                rdoSize.Text + " " + numTol.Value + " / 정사각형 " + numSquare.Value + " / " + cboSide.Text);

            // ---- 2) Wafer 받기 -> [분석] 없이 1차 분석 ----
            var wafer = new DACrux.Base.DPWafer();
            wafer.StepSeq = stepSeq.ToString();
            wafer.WaferID = info0["WAFER_ID"].ToString();
            wafer.LotID = info0["LOT_ID"].ToString();
            wafer.Product = info0["PRODUCT"].ToString();
            var watch = Stopwatch.StartNew();
            form.DrawWafer(new[] { wafer });
            bool analyzed = WaitFor(() => form.AnalysisResult != null, 180);
            double autoSeconds = watch.Elapsed.TotalSeconds;
            Pump(300);
            GdsKlarfAnalysisRequest r = form.LastRequest;
            Check("Wafer 받으면 [분석] 없이 1차 분석 / 요청 = 설계 5.1 (SIZE_TOL · 허용오차 0 · 보정 0 · Chain BS · 검사 FS · Chain / 분류 전체 · 큰 Defect 포함) + E-Test = 같은 Wafer 최신 READY 파일(없으면 0) · Wafer Key",
                analyzed && r != null && r.AreaMode == GdsKlarfAnalysisRequest.AreaSizeTolerance && r.ToleranceUm == 0 && r.OffsetXUm == 0 && r.OffsetYUm == 0
                && r.EtSrcFileSeq == etSeqExpected && r.ChainSide == "BACK" && r.ViewSides.SequenceEqual(new[] { "FRONT" }) && r.StepSeqs.SequenceEqual(new[] { stepSeq })
                && r.DisplayStepSeq == stepSeq && r.WaferKey == keyExpected && r.ChainSeqs == null && r.ClassValues == null && !r.ExcludeLargeDefect
                && r.ClassBasis == GdsKlarfAnalysisRequest.ClassBasisRoughBin && r.DeviceId == device && r.Factory == factory,
                String.Format("{0:0.0}초 / {1}", autoSeconds, r == null ? "요청 없음" : r.AreaMode + " " + r.ToleranceUm + " / 보정 (" + r.OffsetXUm + ", " + r.OffsetYUm
                    + ") / ET " + r.EtSrcFileSeq + " (기대 " + etSeqExpected + ") / Wafer Key " + r.WaferKey + " / " + r.ChainSide + " / " + String.Join(",", r.ViewSides) + " / " + r.DeviceId));
            DataSet result = form.AnalysisResult;
            Check("1차 결과 = BSL 직접 호출(허용오차 0): Chain 위 Defect 수 / Die 별 Defect · Chain 위 · Chain 수", analyzed && SameDies(result, expected0),
                analyzed ? "Chain 위 " + result.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"] + " / Die " + result.Tables["DIE"].Rows.Count : "결과 없음");
            if (!analyzed) return Finish();

            // Map: 채움색 / 숫자 / E-Test 표식
            {
                Type m = typeof(GdsKlarfAnalysisMap);
                var fills = (Dictionary<Point, Color>)Get(m, map, "_dieFill");
                var labels = (Dictionary<Point, int>)Get(m, map, "_dieLabel");
                var spec = (HashSet<Point>)Get(m, map, "_specDies");
                var file = (HashSet<Point>)Get(m, map, "_fileDies");
                List<DataRow> dies = result.Tables["DIE"].Rows.Cast<DataRow>().ToList();
                List<DataRow> defectDies = dies.Where(d => Convert.ToInt32(d["DEFECT_COUNT"]) > 0).ToList();
                bool labelsOk = labels.Count == defectDies.Count && defectDies.All(d => labels[DiePoint(d)] == Convert.ToInt32(d["IN_CHAIN_COUNT"]));
                Check("Map: 채움색 = 결과 Die 전체 / 숫자 = Defect Die 의 Chain 위 Defect 수 / E-Test 삼각형 0",
                    fills.Count == dies.Count && labelsOk && spec.Count == 0 && file.Count == 0 && map.Defects.Count == Convert.ToInt32(info0["DEFECT_COUNT"]),
                    "fill " + fills.Count + " / label " + labels.Count + " (Defect Die " + defectDies.Count + ") / 삼각형 " + (spec.Count + file.Count) + " / Map Defect " + map.Defects.Count);
            }
            // 2026-10-22: 분석 정보는 제목 줄(설정 바뀜 / 알림)만 남기고 Step / GDS / E-Test / 결과 / 기준 5줄은 뺐다
            var pnlInfo = (Panel)Get(t, form, "pnlInfo");
            var lblInfoTitle = (Label)Get(t, form, "lblInfoTitle");
            Check("분석 정보 = 제목 줄만(Step / GDS / E-Test / 결과 / 기준 5줄 없음) / 설정 바뀜 · 알림 없음 / GDS Device / [다시 분석] 사용 가능",
                !lblInfo.Visible && lblInfo.Text.Length == 0 && pnlInfo.Height <= lblInfoTitle.Bottom + lblInfoTitle.Top && !lblChanged.Visible
                && lblDeviceValue.Text == device && btnAnalyze.Enabled,
                "분석 정보 높이 " + pnlInfo.Height + " (제목 아래 " + lblInfoTitle.Bottom + ") / 알림 \"" + lblInfo.Text + "\"");
            Check("아래 상태줄 없음: 진행 표시는 작업(Wafer 조회 + 분석) 뒤 숨김 / 오류 없으면 분석 정보 알림 없음",
                !busyBar.Visible && busyBar.LastResult.StartsWith("Wafer 조회 + 분석") && !lblInfo.Visible,
                "진행 표시 보임 " + busyBar.Visible + " / 마지막 작업 " + busyBar.LastResult);

            // 종합 (Die)
            {
                List<DataRow> defectDies = result.Tables["DIE"].Rows.Cast<DataRow>().Where(d => Convert.ToInt32(d["DEFECT_COUNT"]) > 0).ToList();
                // 2026-10-22: 검사 열은 값을 채우고 숨긴다
                string[] cols = dgvDie.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray();
                bool order = dgvDie.Rows.Count == 0 || (Convert.ToInt32(dgvDie.Rows[0].Cells["X"].Value) == Convert.ToInt32(defectDies[0]["XINDEX"])
                    && Convert.ToInt32(dgvDie.Rows[0].Cells["Y"].Value) == Convert.ToInt32(defectDies[0]["YINDEX"]));
                var lblDieCount = (Label)Get(t, form, "lblDieCount");
                int hitDies = defectDies.Count(d => Convert.ToInt32(d["IN_CHAIN_COUNT"]) > 0);
                Check("종합 (Die, 한 페이지 위쪽 표): 보이는 열 = Index / X / Y / Defect 전체 / Chain 위 / Chain 밖 / Chain 수 (검사 열 숨김), 행 = Defect 있는 Die(서버 순서) / 머리 = Die 수 · Chain 위 Die 수 / 화면에 보임",
                    cols.SequenceEqual(new[] { "Index", "X", "Y", "Defect 전체", "Chain 위", "Chain 밖", "Chain 수" }) && dgvDie.Columns.Contains("검사") && !dgvDie.Columns["검사"].Visible
                    && dgvDie.Rows.Count == defectDies.Count && order && lblDieCount.Text == "Die " + defectDies.Count + " / Chain 위 Defect 있는 Die " + hitDies
                    && dgvDie.Visible && dgvDie.Height > 0,
                    String.Join(",", cols) + " / 행 " + dgvDie.Rows.Count + " (Defect Die " + defectDies.Count + ") / " + lblDieCount.Text + " / 표 높이 " + dgvDie.Height);
                // 2026-10-22: 맨 왼쪽 Index = 고객 통합 Die#(Map 사용 Die 를 위 행부터 · 같은 행 왼쪽부터). 사용 Die 가 아니면 빈 칸
                int numbered = 0;
                bool indexOk = indexMap != null && dgvDie.Rows.Cast<DataGridViewRow>().All(row =>
                {
                    int n = ExpectedIndex(Convert.ToInt32(row.Cells["X"].Value), Convert.ToInt32(row.Cells["Y"].Value));
                    object v = row.Cells["Index"].Value;
                    if (n > 0) numbered++;
                    return n > 0 ? v != null && v != DBNull.Value && Convert.ToInt32(v) == n : v == null || v == DBNull.Value;
                });
                Check("종합 맨 왼쪽 Index = 고객 통합 Die#(BSL Map 사용 Die 로 직접 계산한 값과 같음, 사용 Die 아니면 빈 칸)",
                    indexOk && numbered > 0 && dgvDie.Columns["Index"].DisplayIndex == 0,
                    "Map 사용 Die " + (indexMap == null ? "없음" : indexMap.DieCount.ToString()) + " / 번호 있는 행 " + numbered + " / " + dgvDie.Rows.Count
                    + (dgvDie.Rows.Count == 0 ? "" : " / 첫 행 Index " + dgvDie.Rows[0].Cells["Index"].Value + " (" + dgvDie.Rows[0].Cells["X"].Value + ", " + dgvDie.Rows[0].Cells["Y"].Value + ")"));
            }
            // Wafer Map 머리: 마우스가 Die 위를 지나면 X / Y / Index, Map 밖으로 나가면 지움 (2026-10-22)
            {
                var lblMapDie = (Label)Get(t, form, "lblMapDie");
                var lblMapTitle = (Label)Get(t, form, "lblMapTitle");
                var btnBand = (Button)Get(t, form, "btnBand");
                DataRow top = result.Tables["DIE"].Rows.Cast<DataRow>().First(d => Convert.ToInt32(d["DEFECT_COUNT"]) > 0);
                Point p = DiePoint(top);
                DACrux.Base.Die hoverDie = null;
                foreach (DACrux.Base.Die d in map.Dies) { if (d.IndexX == p.X && d.IndexY == p.Y) { hoverDie = d; break; } }
                // Designer 연결까지 보려고 Map 의 OnChangeCurrentDie 이벤트 목록으로 부른다(필드를 못 찾으면 처리 함수를 직접 부름)
                FieldInfo evField = null;
                for (Type bt = map.GetType(); bt != null && evField == null; bt = bt.BaseType)
                    evField = bt.GetField("OnChangeCurrentDie", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                Delegate ev = evField == null ? null : evField.GetValue(map) as Delegate;
                bool wired = ev != null && ev.GetInvocationList().Any(x => x.Target == form && x.Method.Name == "map_OnChangeCurrentDie");
                string hoverError = "";
                try
                {
                    if (wired) ev.DynamicInvoke(map, hoverDie);
                    else Invoke(t, form, "map_OnChangeCurrentDie", map, hoverDie);
                }
                catch (TargetInvocationException ex) { hoverError = ex.InnerException.GetType().Name + ": " + ex.InnerException.Message; }
                Application.DoEvents();
                int n = ExpectedIndex(p.X, p.Y);
                string expectText = "X " + p.X + "  /  Y " + p.Y + "  /  Index " + (n > 0 ? n.ToString() : "-");
                string hoverText = lblMapDie.Text;
                bool placed = lblMapDie.Visible && lblMapDie.Parent == lblMapTitle.Parent && lblMapDie.Left > lblMapTitle.Right && lblMapDie.Right < btnBand.Left
                    && lblMapDie.Width > 0;
                string position = lblMapDie.Left + "~" + lblMapDie.Right;
                typeof(Control).GetMethod("OnMouseLeave", f).Invoke(map, new object[] { EventArgs.Empty });
                string leaveText = lblMapDie.Text;
                Invoke(t, form, "map_OnChangeCurrentDie", map, hoverDie);
                Invoke(t, form, "map_OnChangeCurrentDie", map, null);
                string nullText = lblMapDie.Text;
                // S1 그림에 머리 표시가 보이게 다시 그 Die 위에 둔다
                Invoke(t, form, "map_OnChangeCurrentDie", map, hoverDie);
                Check("Wafer Map 머리: 마우스 Die = \"X / Y / Index\"(제목과 [색 구간 설정] 사이) / Map 밖 = 지움 / Die 없음(null) = 지움, 예외 없음",
                    !ReferenceEquals(hoverDie, null) && wired && hoverError == "" && hoverText == expectText && n > 0 && placed && leaveText == "" && nullText == "",
                    "\"" + hoverText + "\" (기대 \"" + expectText + "\") / 이벤트 연결 " + wired + (hoverError == "" ? "" : " / " + hoverError)
                    + " / 위치 " + position + " (제목 끝 " + lblMapTitle.Right + ", 버튼 " + btnBand.Left + ") / 나감 \"" + leaveText + "\" / null \"" + nullText + "\"");
            }
            // POC 화면 문구 규칙(2026-10-22 공통 개발 지침): 이름(라벨 제목 / 체크 / 라디오 / 버튼 / 탭 / 표 머리글)에 설명 괄호 없음, 화면 어디에도 단위 없음.
            // 값을 보이는 괄호(개수 / 좌표 / 비율 / 시간, 예: "Die (6, 6)", "(6.7%)")는 쓸 수 있어 정보 칸은 단위만 본다
            {
                var names = new List<string>();
                var all = new List<string>();
                var valueLabels = new HashSet<string> { "lblWaferInfo", "lblDeviceValue", "lblInfo", "lblImageInfo", "lblDieCount" };
                Action<Control> walk = null;
                walk = c =>
                {
                    foreach (Control child in c.Controls)
                    {
                        // Map 내부 / 진행 표시줄 / 이미지 목록(카드)은 값 표시라 뺀다
                        if (child is GdsKlarfAnalysisMap || child is DACrux.SEMDMS.Control.ProgressStatusBar || child is FlowLayoutPanel) continue;
                        if (child is Label || child is ButtonBase || child is TabPage)
                        {
                            all.Add(child.Name + "=" + child.Text);
                            if (!valueLabels.Contains(child.Name)) names.Add(child.Name + "=" + child.Text);
                        }
                        if (child is DataGridView)
                            foreach (DataGridViewColumn col in ((DataGridView)child).Columns) { names.Add(child.Name + "." + col.HeaderText); all.Add(col.HeaderText); }
                        walk(child);
                    }
                };
                walk(form);
                var popupForm = (Form)Get(t, form, "_bandPopup");
                walk(popupForm);
                System.Text.RegularExpressions.Regex unit = new System.Text.RegularExpressions.Regex(@"µm|\bum\b|\bmm\b|\bnm\b");
                string[] parenNames = names.Where(n => n.Contains("(")).ToArray();
                string[] unitTexts = all.Where(s => unit.IsMatch(s)).ToArray();
                Check("문구 규칙: 이름에 설명 괄호 없음 / 화면 문구 · 분석 정보에 단위 없음",
                    parenNames.Length == 0 && unitTexts.Length == 0 && !unit.IsMatch(lblInfo.Text) && names.Count > 20,
                    "이름 " + names.Count + "개 / 괄호 " + (parenNames.Length == 0 ? "없음" : String.Join(", ", parenNames)) + " / 단위 " + (unitTexts.Length == 0 ? "없음" : String.Join(", ", unitTexts)));
            }
            Save(form, Path.Combine(outDir, "S1_자동분석_허용오차0.png"));

            // ---- 3) 색 구간 팝업 ----
            {
                form.ShowBandPopup();
                Application.DoEvents();
                var popup = (GdsKlarfBandPopup)Get(t, form, "_bandPopup");
                Type p = typeof(GdsKlarfBandPopup);
                var dgvBand = (DataGridView)Get(p, popup, "dgvBand");
                var numTh = (NumericUpDown)Get(p, popup, "numThreshold");
                List<DataRow> dies = result.Tables["DIE"].Rows.Cast<DataRow>().ToList();
                int bandDies = dgvBand.Rows.Cast<DataGridViewRow>().Sum(row => Convert.ToInt32(row.Cells[3].Value));
                int maxIn = dies.Select(d => Convert.ToInt32(d["IN_CHAIN_COUNT"])).DefaultIfEmpty(0).Max();
                List<GdsKlarfColorBand> bands = popup.Bands;
                Check("색 구간 팝업: 열림 / 자동 구간 = Auto(1, 최대 Chain 위) / 구간 + 고정 2행(기준 미만 · Defect 없음) / Die 수 합 = 결과 Die 수",
                    popup.Visible && popup.AutoBand && bands.Count == Math.Min(5, Math.Max(1, maxIn)) && bands[0].From == 1 && bands[bands.Count - 1].To == 999
                    && dgvBand.Rows.Count == bands.Count + 2 && Convert.ToString(dgvBand.Rows[bands.Count].Cells[0].Value) == "기준 미만"
                    && Convert.ToString(dgvBand.Rows[bands.Count + 1].Cells[0].Value) == "Defect 없음" && bandDies == dies.Count,
                    "구간 " + String.Join(" / ", bands.Select(b => b.From + "-" + b.To).ToArray()) + " / 행 " + dgvBand.Rows.Count + " / Die 합 " + bandDies + " (" + dies.Count + ")");
                var fills = (Dictionary<Point, Color>)Get(typeof(GdsKlarfAnalysisMap), map, "_dieFill");
                List<DataRow> defectDies = dies.Where(d => Convert.ToInt32(d["DEFECT_COUNT"]) > 0).ToList();
                numTh.Value = maxIn + 1;
                Application.DoEvents();
                bool allBelow = defectDies.All(d => fills[DiePoint(d)].ToArgb() == GdsKlarfColorBands.BelowThresholdColor.ToArgb());
                numTh.Value = 1;
                Application.DoEvents();
                DataRow hit = dies.FirstOrDefault(d => Convert.ToInt32(d["IN_CHAIN_COUNT"]) > 0);
                bool back = hit == null || fills[DiePoint(hit)].ToArgb() != GdsKlarfColorBands.BelowThresholdColor.ToArgb();
                Check("표시 기준 = 최대 + 1 -> Defect Die 모두 연회색(Map 다시 칠함) / 1 로 되돌리면 구간 색", allBelow && back,
                    "Defect Die " + defectDies.Count + " / 최대 Chain 위 " + maxIn + (hit == null ? " / Chain 위 Die 없음" : ""));
                popup.Hide();
            }

            // ---- 4) 설정 바꾸기 -> 설정 바뀜 -> [분석] (허용오차 5) ----
            {
                numTol.Value = 5;
                Application.DoEvents();
                bool dirty = lblChanged.Visible && ReferenceEquals(form.AnalysisResult, result);
                DataSet r5 = form.AnalyzeNow();
                Application.DoEvents();
                Check("허용오차 5 로 바꾸면 '설정 바뀜' 만(서버 안 부름) / [다시 분석] = 허용오차 5 BSL 결과 / Chain 위 수 0 <= 5",
                    dirty && SameDies(r5, expected5) && in0 <= in5 && !lblChanged.Visible && form.LastRequest.ToleranceUm == 5,
                    "설정 바뀜 표시 " + dirty + " / Chain 위 " + in0 + " -> " + r5.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);
                result = r5;
            }

            // ---- 5) Die 선택 -> 종합 행 / 이미지 카드 ----
            {
                Invoke(t, form, "dgvDie_CellClick", dgvDie, new DataGridViewCellEventArgs(0, 0));
                Application.DoEvents();
                Point die = form.SelectedDie ?? new Point(int.MinValue, int.MinValue);
                Point first = dgvDie.Rows.Count == 0 ? die : new Point(Convert.ToInt32(dgvDie.Rows[0].Cells["X"].Value), Convert.ToInt32(dgvDie.Rows[0].Cells["Y"].Value));
                bool rowPicked = dgvDie.SelectedRows.Count == 1 && dgvDie.SelectedRows[0].Index == 0;
                Check("종합 첫 행 클릭 = Die 선택(Map 빨간 테두리 / 종합 행 / 이미지 머리)",
                    form.SelectedDie.HasValue && die == first && rowPicked && map.SelectedDies.Count == 1 && map.SelectedDies[0] == die
                    && lblImageInfo.Text.StartsWith("Die (" + die.X + ", " + die.Y + ")  Defect "),
                    "Die (" + die.X + ", " + die.Y + ") / 종합 행 선택 " + rowPicked + " / " + lblImageInfo.Text);

                bool loaded = WaitFor(() => Get(t, form, "_imagePaths") != null || ((string)Get(t, form, "_imagePathNote")).Length > 0, 60);
                Invoke(t, form, "RefreshImages");
                Application.DoEvents();
                // 이미지 = 그 Die 에 있는 모든 Defect (2026-10-22 레이아웃 정리: Chain 위 / 밖 묶음 · 체크 없음)
                var dieIds = new HashSet<long>(map.Defects.Where(d => d.XINDEX == die.X && d.YINDEX == die.Y).Select(d => (long)d.DEFECTID));
                int expectAll = imagePaths.Rows.Cast<DataRow>().Count(row => dieIds.Contains(Convert.ToInt64(row["DEFECTID"])));
                int cardsAll = flowImage.Controls.OfType<DACrux.SEMDMS.Control.DPUCDefectImageCard>().Count();
                int heads = flowImage.Controls.OfType<Label>().Count();
                string expectInfo = "Die (" + die.X + ", " + die.Y + ")  Defect " + dieIds.Count + " / 이미지 " + expectAll;
                Check("이미지: 그 Die 의 모든 Defect 이미지 카드 = 이미지 경로 수 / 묶음 제목 없음 / 머리 = Die · Defect 수 · 이미지 수",
                    loaded && cardsAll == expectAll && heads == 0 && lblImageInfo.Text == expectInfo,
                    "카드 " + cardsAll + " (기대 " + expectAll + ") / 묶음 제목 " + heads + " / " + lblImageInfo.Text);
                // 카드 이미지 읽기(FTP)는 이 PC 의 DEFECT_FTP 설정 / FTP 서버에 따른다. 검사가 아니라 결과만 적는다
                List<DACrux.SEMDMS.Control.DPUCDefectImageCard> cards = flowImage.Controls.OfType<DACrux.SEMDMS.Control.DPUCDefectImageCard>().ToList();
                WaitFor(() => cards.All(c => c.State != DACrux.SEMDMS.Control.DPUCDefectImageCard.ImageState.Loading), 60);
                Console.WriteLine("참고 / 이미지 카드 읽기: Loaded " + cards.Count(c => c.State == DACrux.SEMDMS.Control.DPUCDefectImageCard.ImageState.Loaded)
                    + " / Failed " + cards.Count(c => c.State == DACrux.SEMDMS.Control.DPUCDefectImageCard.ImageState.Failed)
                    + " / Loading " + cards.Count(c => c.State == DACrux.SEMDMS.Control.DPUCDefectImageCard.ImageState.Loading));
                Save(form, Path.Combine(outDir, "S2_Die선택_허용오차5_이미지.png"));
            }

            // ---- 5b) KLARF 위치 조정 (2026-10-22 설계 2026-10-22_KLARF_위치조정_설계.md): 화살표 = KLARF REL 에 더할 값 ±1, [초기화] = 0,
            //          값만 바꾸고 [다시 분석] 으로 계산(K2), 요청 보정값 = X / Y, Wafer 를 다시 받아도 값 유지(K5) ----
            {
                var numShiftX = (NumericUpDown)Get(t, form, "numShiftX");
                var numShiftY = (NumericUpDown)Get(t, form, "numShiftY");
                var btnLeft = (Button)Get(t, form, "btnShiftLeft");
                var btnUp = (Button)Get(t, form, "btnShiftUp");
                var btnDown = (Button)Get(t, form, "btnShiftDown");
                var btnRight = (Button)Get(t, form, "btnShiftRight");
                var btnReset = (Button)Get(t, form, "btnShiftReset");
                Button[] arrows = { btnLeft, btnUp, btnDown, btnRight };
                Check("KLARF 위치 조정 배치: X / Y 기본 0 / [◀][▲][▼][▶] 아이콘만 / [초기화] / [다시 분석] = 위치 조정 묶음 / 구분선 4개",
                    numShiftX.Value == 0 && numShiftY.Value == 0 && arrows.All(b => b.Text.Length == 0 && b.Image != null)
                    && btnReset.Text == "초기화" && btnReset.Image != null && btnAnalyze.Parent != null && btnAnalyze.Parent.Name == "pnlShift"
                    && new[] { "lineArea", "lineShift", "lineDie", "lineImage" }.All(n => t.GetField(n, f) != null),
                    "X " + numShiftX.Value + " / Y " + numShiftY.Value + " / [다시 분석] 묶음 " + (btnAnalyze.Parent == null ? "-" : btnAnalyze.Parent.Name));

                DataSet noShift = form.AnalysisResult;   // 4) 의 허용오차 5 / 위치 0 결과
                foreach (Button b in new[] { btnRight, btnRight, btnRight, btnLeft, btnUp, btnDown, btnDown })
                    Invoke(t, form, "ShiftButton_Click", b, EventArgs.Empty);
                Application.DoEvents();
                bool moved = numShiftX.Value == 2 && numShiftY.Value == -1;
                bool waiting = lblChanged.Visible && ReferenceEquals(form.AnalysisResult, noShift);
                Check("화살표: ▶ x3 · ◀ x1 = X 2 / ▲ x1 · ▼ x2 = Y -1, 값만 바뀌고 '설정 바뀜'(서버 안 부름, 결과는 이전 그대로)",
                    moved && waiting, "X " + numShiftX.Value + " / Y " + numShiftY.Value + " / 설정 바뀜 " + lblChanged.Visible + " / 결과 그대로 " + ReferenceEquals(form.AnalysisResult, noShift));

                GdsKlarfAnalysisRequest shiftRequest = request(5);
                shiftRequest.OffsetXUm = 2;
                shiftRequest.OffsetYUm = -1;
                DataSet expectedShift = analysisBsl.Analyze(shiftRequest);
                DataSet shifted = form.AnalyzeNow();
                Application.DoEvents();
                DataRow si = shifted.Tables["ANL_INFO"].Rows[0];
                Check("[다시 분석]: 요청 보정 X 2 / Y -1 = 결과 OFFSET_X / Y / BSL 직접 호출(같은 보정)과 Die 별 같음 / 설정 바뀜 꺼짐",
                    form.LastRequest.OffsetXUm == 2 && form.LastRequest.OffsetYUm == -1 && Convert.ToDouble(si["OFFSET_X"]) == 2 && Convert.ToDouble(si["OFFSET_Y"]) == -1
                    && SameDies(shifted, expectedShift) && !lblChanged.Visible,
                    "OFFSET " + si["OFFSET_X"] + " / " + si["OFFSET_Y"] + " / Chain 위 " + noShift.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"] + " -> " + si["IN_CHAIN_DEFECT_COUNT"]);

                // Defect 보기 좌표(GDS_X / GDS_Y) = 조정 전 + (2, -1). 반전(Chain 면 BS)이어도 보기 좌표는 KLARF 방향 그대로라 상세보기 도면에서도 같은 방향이다
                Func<DataSet, Dictionary<long, double[]>> xy = ds => ds.Tables["DEFECT"].Rows.Cast<DataRow>()
                    .GroupBy(row => Convert.ToInt64(row["DEFECTID"])).ToDictionary(g => g.Key, g => new[] { Convert.ToDouble(g.First()["GDS_X"]), Convert.ToDouble(g.First()["GDS_Y"]) });
                Dictionary<long, double[]> a = xy(noShift), bxy = xy(shifted);
                List<long> both = a.Keys.Where(bxy.ContainsKey).ToList();
                int exact = both.Count(id => Math.Abs(bxy[id][0] - a[id][0] - 2) < 0.01 && Math.Abs(bxy[id][1] - a[id][1] + 1) < 0.01);
                Check("Defect 보기 좌표: GDS_X = 조정 전 + 2 / GDS_Y = 조정 전 - 1 (모든 Defect, 반전 Chain 면 BS 에서도 같은 방향)",
                    both.Count > 0 && exact == both.Count, "Defect " + both.Count + " 중 " + exact + " 일치 / 반전 " + si["MIRROR"]);

                // [초기화] -> 0, 다시 분석하면 4) 결과와 같다
                Invoke(t, form, "btnShiftReset_Click", btnReset, EventArgs.Empty);
                Application.DoEvents();
                bool reset = numShiftX.Value == 0 && numShiftY.Value == 0 && lblChanged.Visible;
                DataSet back = form.AnalyzeNow();
                Application.DoEvents();
                Check("[초기화] = X / Y 0 + 설정 바뀜 -> [다시 분석] = 조정 전(허용오차 5) 결과",
                    reset && SameDies(back, expected5), "초기화 " + reset);

                // K5: Wafer 를 다시 받아도 값 유지 -> 자동 1차 분석 요청에도 들어간다
                numShiftX.Value = 1;
                Application.DoEvents();
                DataSet beforeReload = form.AnalysisResult;
                form.DrawWafer(new[] { wafer });
                bool reloaded = WaitFor(() => form.AnalysisResult != null && !ReferenceEquals(form.AnalysisResult, beforeReload), 180);
                Pump(300);
                Check("Wafer 다시 받기: 위치 조정 값 유지(X 1) / 자동 1차 분석 요청 보정 X 1",
                    reloaded && numShiftX.Value == 1 && form.LastRequest.OffsetXUm == 1 && form.LastRequest.OffsetYUm == 0,
                    "X " + numShiftX.Value + " / 요청 " + (form.LastRequest == null ? "-" : form.LastRequest.OffsetXUm + ", " + form.LastRequest.OffsetYUm));
                Save(form, Path.Combine(outDir, "S5_KLARF위치조정_X1.png"));
                numShiftX.Value = 0;
                form.AnalyzeNow();
                Application.DoEvents();

                // 참고(검사 아님): 기본 조건(허용오차 0)에서 위치 조정에 따른 Chain 위 Defect 수
                var shifts = new[] { new[] { 0, 0 }, new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 }, new[] { 5, 0 }, new[] { -5, 0 }, new[] { 0, 5 }, new[] { 0, -5 } };
                var parts = new List<string>();
                foreach (int[] s in shifts)
                {
                    GdsKlarfAnalysisRequest r0 = request(0);
                    r0.OffsetXUm = s[0];
                    r0.OffsetYUm = s[1];
                    parts.Add("(" + s[0] + ", " + s[1] + ") " + analysisBsl.Analyze(r0).Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);
                }
                Console.WriteLine("참고 / 위치 조정별 Chain 위 Defect 수(허용오차 0): " + String.Join(" / ", parts.ToArray()));
            }

            // ---- 6) 안내 / 오류 ----
            {
                var other = new DACrux.Base.DPWafer();
                other.StepSeq = (stepSeq + 100000).ToString();
                other.WaferID = "OTHER";
                DataSet before = form.AnalysisResult;
                form.DrawWafer(new[] { wafer, other });
                Application.DoEvents();
                Check("Wafer 여러 개 -> 오류(화면 그대로, 분석 정보 제목 줄 알림 = 1개 Wafer 만 분석)",
                    lblInfo.Visible && lblInfo.Text.Contains("1개 Wafer 만 분석할 수 있습니다.") && lblInfo.Text.Contains("(선택 2개)") && ReferenceEquals(form.AnalysisResult, before),
                    lblInfo.Text);

                FieldInfo productField = t.GetField("_product", f);
                string product = (string)productField.GetValue(form);
                MethodInfo selectDevice = t.GetMethod("SelectDeviceForProduct", f);
                productField.SetValue(form, "ZZ_NO_SUCH_PRODUCT");
                string note = (string)selectDevice.Invoke(form, null);
                string noneError = "";
                try { form.AnalyzeNow(); } catch (InvalidOperationException ex) { noneError = ex.Message; }
                Check("GDS Map 없는 Product -> Device 없음 / [분석] 꺼짐 / 분석 불가 안내",
                    form.GdsDevice.Length == 0 && lblDeviceValue.Text == "없음 - 분석 안 함" && !btnAnalyze.Enabled && note.EndsWith("READY GDS Map 없음 (분석 안 함)")
                    && noneError.Contains("분석할 수 없습니다"),
                    lblDeviceValue.Text + " / " + note + " / " + noneError.Split('\n')[0]);
                productField.SetValue(form, product);
                selectDevice.Invoke(form, null);
                Application.DoEvents();
            }
            CloseLikeDacrux(form);
        }

        // ---- 7) Die 상세보기 (2026-10-22 2단계): DACrux 처럼 Application.Run 메시지 루프 안에서 실제 경로 ----
        RunInMessageLoop(() => DetailChecks(stepSeq, info0, outDir, capture, defectBsl, viewBsl, chainBsl, analysisBsl));
        return Finish();
    }

    /// <summary>
    /// 7) Die 상세보기 (설계 15절): MDI 부모 + Simple 화면을 띄워 Wafer 받기 -> 자동 분석 -> Die 더블클릭 -> AnalyzeDie -> 상세보기 창(Shown)
    /// -> 공유 GDS 상세 화면 경로를 탄다. 공유 화면은 기본 서비스(DefaultMapService / DefaultChainService)를 같은 프로세스 BSL 로 바꿔 쓴다(화면 A Smoke 와 같음).
    /// </summary>
    private static void DetailChecks(long stepSeq, DataRow info0, string outDir, bool capture, DACrux.SEMDMS.BSL.DefectMapAnalysis defectBsl,
        DACrux.SEMDMS.BSL.GdsMapView viewBsl, DACrux.SEMDMS.BSL.GdsChain chainBsl, DACrux.SEMDMS.BSL.GdsKlarfAnalysis analysisBsl)
    {
        Type t = typeof(frmGdsKlarfSimple);
        Type tb = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
        Type tm = typeof(NexplantQMS.GdsMap.GdsMapControl);
        Func<object, string, object> field = (o, n) => o.GetType().GetField(n).GetValue(o);
        NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
        NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
        NexplantQMS.GdsMap.GdsDieMapCache.Clear();
        frmGdsDieAnalysis.ShowErrorDialogs = false;
        try
        {
            using (var mdi = new Form())
            {
                mdi.IsMdiContainer = true;
                mdi.StartPosition = FormStartPosition.Manual;
                mdi.Location = new Point(-32000, -32000);
                mdi.Size = new Size(1600, 950);
                mdi.ShowInTaskbar = false;
                mdi.Text = "Simple 상세보기 Smoke";
                mdi.Show();
                var s = new frmGdsKlarfSimple();
                s.MdiParent = mdi;
                s.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(defectBsl);
                s.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
                s.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
                s.ShowErrorDialogs = false;
                s.Show();
                Application.DoEvents();
                var wafer = new DACrux.Base.DPWafer();
                wafer.StepSeq = stepSeq.ToString();
                wafer.WaferID = info0["WAFER_ID"].ToString();
                wafer.LotID = info0["LOT_ID"].ToString();
                wafer.Product = info0["PRODUCT"].ToString();
                s.DrawWafer(new[] { wafer });
                if (!WaitFor(() => s.AnalysisResult != null, 180))
                {
                    Check("상세보기: MDI 안 Simple 화면 자동 분석", false, "결과 없음");
                    return;
                }
                // [분석] 뒤 미리 읽기(Preload)가 끝나기를 기다린다
                if (NexplantQMS.GdsMap.GdsDieMapCache.Pending != null) Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                DataSet result = s.AnalysisResult;
                // 서버 순서 첫 Defect Die = Chain 위 Defect 가 가장 많은 Die
                Point d = DiePoint(result.Tables["DIE"].Rows.Cast<DataRow>().First(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0));
                var map = (GdsKlarfAnalysisMap)Get(t, s, "map");

                // ---- 7-1) Die 더블클릭 = 상세보기 창 하나(넘긴 결과에 따로 표시하지 않음, 2026-10-22 두 화면 같은 모양) ----
                var watch = Stopwatch.StartNew();
                Invoke(t, s, "map_DieDoubleClicked", map, new GdsKlarfDieEventArgs(d.X, d.Y));
                bool shown = WaitFor(() => ShowsDie(d.X, d.Y), 180);
                double openSeconds = watch.Elapsed.TotalSeconds;
                var w = frmGdsDieAnalysis.Current;
                var b = frmGdsDieAnalysis.SharedDetailForm as NexplantQMS.GdsMap.GdsDieAnalysisForm;
                Check("Die 더블클릭 = 상세보기 창 하나 \"상세보기 (x, y)\" / 넘긴 결과에 Simple 표시 없음(화면 A 와 같은 결과)",
                    shown && w != null && b != null && w.HasSharedDetail && frmGdsDieAnalysis.OpenWindows.Count == 1 && w.Text == "상세보기 (" + d.X + ", " + d.Y + ")"
                    && !w.Analysis.ExtendedProperties.ContainsKey("DETAIL_VIEW") && tb.GetProperty("SimpleView") == null,
                    String.Format("{0:0.0}초 / Die ({1}, {2}) / 창 {3} / {4}", openSeconds, d.X, d.Y, w == null ? "-" : w.Text, b == null ? "공유 화면 없음" : b.StatusText));
                if (!shown || w == null || b == null) return;
                DataSet dieData = w.Analysis;

                // ---- 7-1b) 왼쪽 Die 목록 (2026-10-22 설계 Die상세_DLL로딩_Die목록 5.2절): Defect Die + E-Test Fail Die, 서버 DIE 표 순서 /
                //      열 No · Index(고객 통합 Die#, 2026-10-22) · X · Y · Defect · Chain 위(E-Test 결과가 있으면 E-Test Fail) / 연 Die 가 골라져 있음 ----
                Func<DataRow, int> etFail = r => r.Table.Columns.Contains("ET_FAIL_CHAIN_COUNT") && !r.IsNull("ET_FAIL_CHAIN_COUNT") ? Convert.ToInt32(r["ET_FAIL_CHAIN_COUNT"]) : 0;
                List<DataRow> targetRows = result.Tables["DIE"].Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 || etFail(r) > 0).ToList();
                bool etLoaded = result.Tables["DIE"].Rows.Cast<DataRow>().Any(r => r.Table.Columns.Contains("ET_FAIL_CHAIN_COUNT") && !r.IsNull("ET_FAIL_CHAIN_COUNT"));
                var dgvDies = (DataGridView)Get(typeof(frmGdsDieAnalysis), w, "dgvDies");
                string[] heads = dgvDies.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray();
                string[] expectHeads = etLoaded ? new[] { "No", "Index", "X", "Y", "Defect", "Chain 위", "E-Test Fail" } : new[] { "No", "Index", "X", "Y", "Defect", "Chain 위" };
                bool valuesOk = dgvDies.Rows.Count == targetRows.Count && dgvDies.Rows.Cast<DataGridViewRow>().All(row =>
                {
                    DataRow r = targetRows[row.Index];
                    int n = ExpectedIndex(DiePoint(r).X, DiePoint(r).Y);
                    string indexText = Convert.ToString(row.Cells["colIndex"].Value);
                    return Convert.ToInt32(row.Cells["colNo"].Value) == row.Index + 1 && indexText == (n > 0 ? n.ToString() : "")
                        && Convert.ToInt32(row.Cells["colX"].Value) == DiePoint(r).X
                        && Convert.ToInt32(row.Cells["colY"].Value) == DiePoint(r).Y && Convert.ToInt32(row.Cells["colDefect"].Value) == Convert.ToInt32(r["DEFECT_COUNT"])
                        && Convert.ToInt32(row.Cells["colInChain"].Value) == Convert.ToInt32(r["IN_CHAIN_COUNT"]);
                });
                Check("상세보기 왼쪽 Die 목록 = Defect / E-Test Fail Die(서버 순서) / 열 No · Index · X · Y · Defect · Chain 위 (+ E-Test Fail) / 값 = DIE 표 · Index = 고객 통합 Die# / 연 Die 선택 / 건수",
                    valuesOk && w.ListedDies.SequenceEqual(targetRows.Select(DiePoint)) && heads.SequenceEqual(expectHeads)
                    && dgvDies.SelectedRows.Count == 1 && (Point)dgvDies.SelectedRows[0].Tag == d
                    && ((Label)Get(typeof(frmGdsDieAnalysis), w, "lblListCount")).Text == targetRows.Count.ToString(),
                    "목록 " + dgvDies.Rows.Count + " (DIE " + targetRows.Count + ") / 열 " + String.Join(",", heads)
                    + (dgvDies.Rows.Count == 0 ? "" : " / 첫 행 Index " + dgvDies.Rows[0].Cells["colIndex"].Value)
                    + " / 선택 " + (dgvDies.SelectedRows.Count == 0 ? "-" : dgvDies.SelectedRows[0].Tag.ToString()));
                List<DataRow> dieDefects = dieData.Tables["DEFECT"].Rows.Cast<DataRow>().Where(r => DiePoint(r) == d).ToList();

                // ---- 7-2) Simple 요청 1 / 2 + 한 패널 요청(두 화면 같은 모양): 위 분석 기준 칸 · 표 위 요약 글자 없음, 기준 묶음 그대로,
                //      표 위 체크 = [걸린 / Fail Chain 만](2026-10-28 기본 켬) · [E-Test 값]. Chain 밖 체크 · 4분류 상태 칸 없음, 위쪽 오른쪽 [다시 분석] (2026-10-22) ----
                string[] sumNames = { "lblSumTitle", "lblSumGds", "lblSumKlarf", "lblSumEt", "lblChainTitle" };
                Func<bool> sumHidden = () => sumNames.All(n => !((Control)Get(tb, b, n)).Visible);
                var pnlChainHead = (Control)Get(tb, b, "pnlChainHead");
                var pnlGdsCriteria = (Control)Get(tb, b, "pnlGdsCriteria");
                var chkHitOnly = (CheckBox)Get(tb, b, "chkHitOnly");
                bool removed = new[] { "chkOutside", "flowClass", "lblClassBoth" }.All(n => tb.GetField(n, BindingFlags.NonPublic | BindingFlags.Instance) == null)
                    && ((Control)Get(tb, b, "btnReanalyze")).Visible;
                Check("상세보기: 위쪽 분석 기준 칸 · 표 위 요약 글자 없음(Wafer · Die 줄 그대로) / GDS / KLARF 기준 그대로 / 표 위 체크 보임 · [걸린 / Fail Chain 만] 켬 / Chain 밖 체크 · 4분류 칸 없음 / [다시 분석] 보임",
                    sumHidden() && ((Control)Get(tb, b, "lblWaferInfo")).Visible && ((Control)Get(tb, b, "lblDieInfo")).Visible
                    && pnlChainHead.Visible && pnlGdsCriteria.Visible && chkHitOnly.Checked && removed,
                    "분석 기준 칸 · 요약 글자 숨김 " + sumHidden() + " / 체크 줄 보임 " + pnlChainHead.Visible + " / 걸린 Chain 만 " + chkHitOnly.Checked + " / 지운 컨트롤 · 다시 분석 " + removed);

                // ---- 7-2b) E-Test 없는 결과(이 Smoke 의 로컬 DB 는 같은 Device E-Test 파일 없음): 비어 있는 분류 / ET / Fail Para 열 · 아래 E-Test 값 표 · E-Test 줄 숨김 ----
                var grid = (DataGridView)Get(tb, b, "dgvChainDefects");
                var splitRight = (SplitContainer)Get(tb, b, "splitRight");
                string[] etColumns = { "colClass", "colEt", "colFailPara" };
                Func<string> shownColumns = () => String.Join(",", grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText).ToArray());
                Func<bool> etPanelHidden = () => splitRight.Panel2Collapsed && !((Control)Get(tb, b, "lblEtText")).Visible
                    && etColumns.All(n => !grid.Columns[n].Visible);
                // [E-Test 값] 체크는 2026-10-23 삭제(E-Test 가 있으면 아래 표 늘 보임)
                Check("E-Test 없는 결과: [분류 / ET / Fail Para] 열 · 아래 표 · E-Test 줄 숨김 / [E-Test 값] 체크 · 가까운 Chain 열 · 오른쪽 탭 없음",
                    etPanelHidden() && !grid.Columns.Contains("colNear") && tb.GetField("tabMain", BindingFlags.NonPublic | BindingFlags.Instance) == null
                    && tb.GetField("chkEtValues", BindingFlags.NonPublic | BindingFlags.Instance) == null,
                    "보이는 열 " + shownColumns() + " / 아래 표 접힘 " + splitRight.Panel2Collapsed);

                // ---- 7-3) 기본(2026-10-28 [걸린 / Fail Chain 만] 켬): 걸린 / E-Test Fail Chain 묶음(둘 다 없는 Die 는 모든 Chain) + Chain 밖 Defect 묶음 ----
                List<object> tags = grid.Rows.Cast<DataGridViewRow>().Select(r => r.Tag).Where(x => x != null).ToList();
                int chainGroups = tags.Count(o => (bool)field(o, "First") && field(o, "ChainSeq") != null);
                bool outsideGroup = tags.Any(o => field(o, "ChainSeq") == null);
                int outside = dieDefects.Count(r => r["IN_CHAIN"].ToString() != "Y");
                int chains = dieData.Tables["CHAIN"].Rows.Count;
                int hitChains = dieData.Tables["DIE_CHAIN"].Rows.Cast<DataRow>()
                    .Where(r => DiePoint(r) == d && (Convert.ToInt32(r["DEFECT_COUNT"]) > 0 || (r.Table.Columns.Contains("ET_STATUS") && r["ET_STATUS"].ToString() == "F")))
                    .Select(r => r["CHAIN_SEQ"].ToString()).Distinct().Count();
                Check("[GDS / KLARF] 표 = 걸린 / Fail Chain 묶음(기본 [걸린 / Fail Chain 만] 켬, 둘 다 없으면 모든 Chain) + Chain 밖 Defect 묶음(있으면)",
                    chainGroups == (hitChains > 0 ? hitChains : chains) && outsideGroup == (outside > 0),
                    "Chain 묶음 " + chainGroups + " / 걸린 · Fail " + hitChains + " / Chain " + chains + " / Chain 밖 Defect " + outside + " / 밖 묶음 " + outsideGroup);

                // ---- 7-4) 요청 3 / 4: Defect 채움(빨강 / 노랑), 흰 점선 없음, 크기 = 분석 Defect 영역(허용오차 0 = KLARF 크기) ----
                var gmap = (NexplantQMS.GdsMap.GdsMapControl)Get(tb, b, "map");
                var markers = (System.Collections.IList)Get(tm, gmap, "_defectMarkers");
                Func<double, double, bool> sizeIs = (add, square) => markers.Count == dieDefects.Count && markers.Cast<object>().All(m =>
                {
                    DataRow r = dieDefects.First(x => Convert.ToInt64(x["DEFECTID"]) == (long)field(m, "Id"));
                    double wExpect = square > 0 ? square : Convert.ToDouble(r["XSIZE"]) + add * 2;
                    double hExpect = square > 0 ? square : Convert.ToDouble(r["YSIZE"]) + add * 2;
                    return Math.Abs((double)field(m, "Width") - wExpect) < 1e-6 && Math.Abs((double)field(m, "Height") - hExpect) < 1e-6;
                });
                Func<string> markerText = () => String.Join(", ", markers.Cast<object>().Take(3).Select(m => "#" + field(m, "Id") + " " + field(m, "Width") + "x" + field(m, "Height")).ToArray());
                Check("도면 Defect = 채움 모양(Chain 안 빨강 / 밖 노랑) / 허용오차 점선 없음 / 크기 = KLARF 크기(허용오차 0) / 수 = 그 Die Defect",
                    gmap.DefectStyle == NexplantQMS.GdsMap.DefectOverlayStyle.Filled && (double)Get(tm, gmap, "_defectMarkerTolerance") == 0 && sizeIs(0, 0),
                    gmap.DefectStyle + " / 점선 " + Get(tm, gmap, "_defectMarkerTolerance") + " / Defect " + markers.Count + " (" + dieDefects.Count + ") / " + markerText());

                // ---- 7-5) 행 선택 = 그 Defect 로 이동 + 고른 표시(번호 글자 없음) ----
                DataGridViewRow pick = grid.Rows.Cast<DataGridViewRow>().Where(r => r.Tag != null && field(r.Tag, "Defect") != null)
                    .OrderBy(r => ((DataRow)field(r.Tag, "Defect"))["IN_CHAIN"].ToString() == "Y" ? 0 : 1).FirstOrDefault();
                if (pick != null)
                {
                    var pickRow = (DataRow)field(pick.Tag, "Defect");
                    long pickId = Convert.ToInt64(pickRow["DEFECTID"]);
                    grid.ClearSelection();
                    grid.CurrentCell = pick.Cells["colDefectId"];
                    pick.Selected = true;
                    Pump(300);
                    double cx, cy, sc, ls;
                    gmap.GetView(out cx, out cy, out sc, out ls);
                    double gx = Convert.ToDouble(pickRow["GDS_X"]), gy = Convert.ToDouble(pickRow["GDS_Y"]);
                    object sel = markers.Cast<object>().FirstOrDefault(m => (bool)field(m, "Selected"));
                    Check("행 선택 = 그 Defect 로 이동(도면 가운데) / 그 Defect 만 고른 표시",
                        sel != null && (long)field(sel, "Id") == pickId && markers.Cast<object>().Count(m => (bool)field(m, "Selected")) == 1
                        && Math.Abs(cx - gx) < 1 && Math.Abs(cy - gy) < 1,
                        String.Format("Defect {0} ({1}) / 도면 가운데 ({2:0.#}, {3:0.#}) / Defect ({4:0.#}, {5:0.#})", pickId,
                            pickRow["IN_CHAIN"].ToString() == "Y" ? "Chain 안" : "Chain 밖", cx, cy, gx, gy));
                }
                if (capture)
                {
                    CaptureScreen(mdi, w, Path.Combine(outDir, "D1_상세보기_Simple_허용오차0.png"));
                    // Chain 밖(노랑) Defect 행도 골라 찍는다
                    DataGridViewRow outRow = grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Tag != null && field(r.Tag, "ChainSeq") == null && field(r.Tag, "Defect") != null);
                    if (outRow != null)
                    {
                        grid.ClearSelection();
                        grid.CurrentCell = outRow.Cells["colDefectId"];
                        outRow.Selected = true;
                        Pump(300);
                        CaptureScreen(mdi, w, Path.Combine(outDir, "D2_상세보기_Simple_Chain밖선택.png"));
                    }
                }

                // ---- 7-6) 분석 설정을 바꿔 다시 분석하면 열린 상세보기도 같이 바뀐다(2026-10-22 요청 "상세도 같이 refresh ... 서로 동기화"):
                //      더블클릭 없이 같은 창 · 같은 Die 를 새 요청으로 다시 계산, Defect 크기 = 새 설정(허용오차 5 = 양쪽 5 / 중심 정사각형 10) ----
                var numTol = (NumericUpDown)Get(t, s, "numTolerance");
                int analyze0 = w.AnalyzeCount;
                double vx0, vy0, vs0, vl0;
                gmap.GetView(out vx0, out vy0, out vs0, out vl0);
                numTol.Value = 5;
                s.AnalyzeNow();
                bool tol5 = WaitFor(() => sizeIs(5, 0) && (double)Get(tm, gmap, "_defectMarkerTolerance") == 0, 60);
                string tol5Text = markerText();
                // 같은 도면이면 보던 확대 위치를 이어 간다
                double vx1, vy1, vs1, vl1;
                gmap.GetView(out vx1, out vy1, out vs1, out vl1);
                bool viewKept = Math.Abs(vx1 - vx0) < 0.01 && Math.Abs(vy1 - vy0) < 0.01 && Math.Abs(vs1 / vs0 - 1) < 1e-6;
                ((RadioButton)Get(t, s, "rdoSquare")).Checked = true;
                ((NumericUpDown)Get(t, s, "numSquare")).Value = 10;
                s.AnalyzeNow();
                bool square10 = WaitFor(() => sizeIs(0, 10), 60);
                int version = (int)t.GetField("_analysisVersion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(s);
                Check("분석 설정을 바꿔 다시 분석 = 열린 상세보기도 더블클릭 없이 새로 계산(창 하나 · 같은 Die · 같은 분석 순번 · 확대 위치 이어짐) / Defect 크기 = 새 설정(허용오차 5 = KLARF 크기 + 양쪽 5, 중심 정사각형 10 = 한 변 10, 점선 없음)",
                    tol5 && square10 && viewKept && frmGdsDieAnalysis.OpenWindows.Count == 1 && frmGdsDieAnalysis.Current == w && w.Text == "상세보기 (" + d.X + ", " + d.Y + ")"
                    && w.AnalyzeCount == analyze0 + 2 && w.AnalysisVersion == version && ShowsDie(d.X, d.Y),
                    "허용오차 5: " + tol5Text + " / 정사각형 10: " + markerText() + " / 창 " + frmGdsDieAnalysis.OpenWindows.Count + " / 다시 계산 " + (w.AnalyzeCount - analyze0)
                    + " / 확대 위치 이어짐 " + viewKept);

                // ---- 7-6b) 상세보기가 열린 채로 다른 Die 더블클릭 = 새 창 없이 그 창 목록에서 그 Die (2026-10-22 요청 "여러 화면이 나오게 하지 않도록") /
                //      목록에서 처음 Die 다시 고르기 = 0.3초 뒤 표시, 기억한 결과라 서버를 다시 부르지 않음 ----
                Point? otherDie = w.ListedDies.Where(p => p != d).Select(p => (Point?)p).FirstOrDefault();
                if (otherDie.HasValue)
                {
                    Point d2 = otherDie.Value;
                    int a0 = w.AnalyzeCount;
                    Invoke(t, s, "map_DieDoubleClicked", map, new GdsKlarfDieEventArgs(d2.X, d2.Y));
                    bool shown2 = WaitFor(() => ShowsDie(d2.X, d2.Y), 60);
                    bool sameWindow = frmGdsDieAnalysis.OpenWindows.Count == 1 && frmGdsDieAnalysis.Current == w && w.Text == "상세보기 (" + d2.X + ", " + d2.Y + ")"
                        && dgvDies.SelectedRows.Count == 1 && (Point)dgvDies.SelectedRows[0].Tag == d2 && w.AnalyzeCount == a0 + 1;
                    DataGridViewRow backRow = dgvDies.Rows.Cast<DataGridViewRow>().First(r => (Point)r.Tag == d);
                    var pick0 = Stopwatch.StartNew();
                    dgvDies.CurrentCell = backRow.Cells["colNo"];
                    bool backShown = WaitFor(() => ShowsDie(d.X, d.Y), 30);
                    double backSeconds = pick0.Elapsed.TotalSeconds;
                    Check("상세보기가 열린 채로 다른 Die 더블클릭 = 새 창 없이 그 창(목록 선택 · 탭 제목 그 Die) / 목록에서 처음 Die 고르기 = 표시 · 서버 다시 안 부름",
                        shown2 && sameWindow && backShown && w.Text == "상세보기 (" + d.X + ", " + d.Y + ")" && w.AnalyzeCount == a0 + 1 && frmGdsDieAnalysis.OpenWindows.Count == 1,
                        "다른 Die (" + d2.X + ", " + d2.Y + ") 창 하나 " + sameWindow + " / 목록 고르기 " + backSeconds.ToString("0.0") + "초 / 다시 계산 " + (w.AnalyzeCount - a0));
                }
                else Console.WriteLine("참고 / Defect Die 가 하나라 다른 Die 더블클릭 검사는 건너뜀");

                // ---- 7-7) E-Test 자동 연결 (2026-10-22 결정 "같은 Wafer 최신 파일 자동"): Wafer 를 받을 때 준비 목록(같은 Wafer + 같은 Device)에서 찾고,
                //      없으면 E-Test 없이 분석한다. 로컬 DB 파일은 Device 이름이 GDS Device 와 달라(OMM / OMMTCB) 준비 목록에 없으므로,
                //      같은 Wafer 파일을 찾아 화면 값에 넣고 [다시 분석] 해서 상세보기가 화면 A 와 같게 E-Test 값을 보이는지 본다.
                //      2026-10-22: 분석 정보 E-Test 줄이 없어져 요청 / 결과의 E-Test 파일 번호로 보고, 조회가 실패하지 않았으면 알림도 없어야 한다 ----
                var lblInfo = (Label)Get(t, s, "lblInfo");
                DataRow etFile = FindSameWaferEtFile(analysisBsl, DACrux.Base.GlobalVariable.Factory, stepSeq, s.GdsDevice);
                if (etFile == null)
                {
                    Check("E-Test 자동 연결: 같은 Device 파일 없으면 E-Test 없이 분석(요청 E-Test 0) / 분석 정보 알림 없음",
                        s.LastRequest.EtSrcFileSeq == 0 && !lblInfo.Visible, "요청 E-Test " + s.LastRequest.EtSrcFileSeq + " / 알림 \"" + lblInfo.Text + "\"");
                    Console.WriteLine("참고 / 같은 Wafer E-Test 파일이 없어 Simple E-Test 연결 상세보기 검사는 건너뜀");
                }
                else
                {
                    BindingFlags pf = BindingFlags.NonPublic | BindingFlags.Instance;
                    ((RadioButton)Get(t, s, "rdoDefectSize")).Checked = true;
                    numTol.Value = 0;
                    t.GetField("_etSrcFileSeq", pf).SetValue(s, Convert.ToInt64(etFile["SRC_FILE_SEQ"]));
                    t.GetField("_waferKey", pf).SetValue(s, etFile["WAFER_ID"].ToString());
                    // 다시 분석해도 보던 Die 의 고른 행은 이어지므로(2026-10-22 동기화) 고른 행을 풀고 E-Test 연결 모양(아래 표 = Die 전체)을 본다
                    grid.ClearSelection();
                    Pump(200);
                    DataSet etResult = s.AnalyzeNow();
                    if (NexplantQMS.GdsMap.GdsDieMapCache.Pending != null) Wait(NexplantQMS.GdsMap.GdsDieMapCache.Pending, 300);
                    string etSeq = etFile["SRC_FILE_SEQ"].ToString();
                    bool etLinked = s.LastRequest.EtSrcFileSeq.ToString() == etSeq && etResult.Tables["ANL_INFO"].Rows[0]["ET_SRC_FILE_SEQ"].ToString() == etSeq;
                    Check("E-Test 연결: 분석 요청 · 결과 E-Test 파일 = 그 파일 / 분석 정보 알림 없음",
                        etLinked && !lblInfo.Visible, "연결 SRC_FILE_SEQ " + etSeq + " (" + etFile["DEVICE_ID"] + ") / 결과 " + etResult.Tables["ANL_INFO"].Rows[0]["ET_SRC_FILE_SEQ"]
                        + " / 알림 \"" + lblInfo.Text + "\"");
                    // E-Test 값이 있는 Die: 일치 Chain -> Fail Chain -> 판정 있는 Die
                    DataRow etDc = etResult.Tables["DIE_CHAIN"].Rows.Cast<DataRow>()
                        .OrderBy(r => r["CLASS4"].ToString() == "BOTH" ? 0 : r["ET_STATUS"].ToString() == "F" ? 1 : r["ET_STATUS"].ToString().Length > 0 ? 2 : 3).FirstOrDefault();
                    Point e = etDc == null ? d : DiePoint(etDc);
                    Invoke(t, s, "OpenDetail", e.X, e.Y);
                    bool etShown = WaitFor(() => ShowsDie(e.X, e.Y) && !splitRight.Panel2Collapsed, 120);
                    var ew = frmGdsDieAnalysis.Current;
                    int etValues = ew == null ? -1 : ew.Analysis.Tables["ET_VALUE"].Rows.Cast<DataRow>().Count(r => DiePoint(r) == e);
                    var etGrid = (DataGridView)Get(tb, b, "dgvEtValues");
                    var etHead = (Label)Get(tb, b, "lblEtCount");
                    // 기준 E-Test 줄은 2026-10-22 부터 E-Test 가 있어도 숨김
                    bool same = etShown && !splitRight.Panel2Collapsed && !((Control)Get(tb, b, "lblEtText")).Visible
                        && etColumns.All(n => grid.Columns[n].Visible) && sumHidden() && pnlChainHead.Visible && gmap.DefectStyle == NexplantQMS.GdsMap.DefectOverlayStyle.Filled
                        && etGrid.Rows.Count == etValues && etHead.Text.StartsWith("E-Test 값 · Die 전체 (" + etValues + ")");
                    Check("Simple E-Test 연결 결과 상세보기 = 화면 A 와 같은 모양: 아래 E-Test 값 표 늘 보임(Die 전체) · 분류 / ET / Fail Para 열 보임 + 분석 기준 칸 · 기준 E-Test 줄 숨김 · Defect 채움",
                        same, "Die (" + e.X + ", " + e.Y + ") / " + etHead.Text + " / 표 " + etGrid.Rows.Count + " (" + etValues + ") / 보이는 열 " + shownColumns());
                    if (capture) CaptureScreen(mdi, w, Path.Combine(outDir, "D3_상세보기_Simple_ETest연결.png"));
                }

                // ---- 7-8) 상세 화면 위쪽 오른쪽 [다시 분석] (2026-10-22): 호스트가 연 Simple 화면의 [다시 분석] 을 부른다(Simple 의 지금 설정)
                //      -> Simple 결과 반영 -> 동기화(SourceAnalyzed)로 같은 창 · 같은 Die 가 새 결과. Simple 에서 허용오차만 바꾸고(설정 바뀜) 상세에서 누른다 ----
                {
                    ((RadioButton)Get(t, s, "rdoDefectSize")).Checked = true;
                    numTol.Value = 3;
                    Application.DoEvents();
                    var lblChanged = (Control)Get(t, s, "lblChanged");
                    bool dirtyBefore = lblChanged.Visible;
                    var win = frmGdsDieAnalysis.Current;
                    Point shownDie = win == null ? d : new Point(win.DieX, win.DieY);
                    int versionBefore = (int)t.GetField("_analysisVersion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(s);
                    Invoke(tb, b, "btnReanalyze_Click", Get(tb, b, "btnReanalyze"), EventArgs.Empty);
                    int versionAfter = (int)t.GetField("_analysisVersion", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(s);
                    bool reShown = WaitFor(() => ShowsDie(shownDie.X, shownDie.Y) && frmGdsDieAnalysis.Current != null && frmGdsDieAnalysis.Current.Analysis != null
                        && Math.Abs(Convert.ToDouble(frmGdsDieAnalysis.Current.Analysis.Tables["ANL_INFO"].Rows[0]["TOLERANCE_UM"]) - 3) < 1e-9, 120);
                    var winAfter = frmGdsDieAnalysis.Current;
                    Check("상세 화면 [다시 분석] = 연 Simple 화면의 [다시 분석](지금 설정 허용오차 3) -> Simple 설정 바뀜 풀림 / 상세보기 같은 창 · 같은 Die · 새 결과(허용오차 3)",
                        dirtyBefore && !lblChanged.Visible && s.LastRequest.ToleranceUm == 3 && versionAfter == versionBefore + 1 && reShown
                        && ReferenceEquals(winAfter, win) && winAfter.AnalysisVersion == versionAfter && winAfter.Text == "상세보기 (" + shownDie.X + ", " + shownDie.Y + ")",
                        "설정 바뀜 " + dirtyBefore + " -> " + lblChanged.Visible + " / 분석 순번 " + versionBefore + " -> " + versionAfter + " / 요청 허용오차 " + s.LastRequest.ToleranceUm
                        + " / 창 " + (winAfter == null ? "-" : winAfter.Text + " 순번 " + winAfter.AnalysisVersion) + " / 표시 " + reShown);
                }

                foreach (Form open in frmGdsDieAnalysis.OpenWindows.ToArray()) open.Dispose();
                CloseLikeDacrux(s);
                var holders = ((System.Collections.IEnumerable)typeof(frmGdsDieAnalysis).GetField("holders", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).Cast<object>().ToList();
                Check("상세보기 창 / Simple 화면을 닫으면 공유 상세 화면 · 미리 읽은 도면 해제(메모리 반환)",
                    frmGdsDieAnalysis.SharedDetailForm == null && b.IsDisposed && holders.Count == 0 && NexplantQMS.GdsMap.GdsDieMapCache.Keys.Length == 0,
                    "열린 창 " + frmGdsDieAnalysis.OpenWindows.Count + " / 붙잡은 화면 " + holders.Count + " / 공유 화면 해제 " + b.IsDisposed
                    + " / 도면 캐시 " + NexplantQMS.GdsMap.GdsDieMapCache.Keys.Length);
                s.Dispose();
            }
        }
        finally
        {
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultMapService = null;
            NexplantQMS.GdsMap.GdsDieAnalysisForm.DefaultChainService = null;
        }
    }

    /// <summary>
    /// Smoke 에는 Remoting 연결이 없어 DACrux 기반 화면의 닫기 처리(사용 기록 원격 호출)에서 끊겨 Close() 가 닫지 못한다.
    /// 화면 A Smoke 와 같이 닫힘 처리(OnFormClosed = 공유 상세 화면 해제)만 직접 부른다.
    /// </summary>
    private static void CloseLikeDacrux(Form form)
    {
        typeof(Form).GetMethod("OnFormClosed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { new FormClosedEventArgs(CloseReason.UserClosing) });
        Application.DoEvents();
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

    /// <summary>공유 상세 화면이 (x, y) Die 를 다 그렸는지(도면 조회 끝 + 위쪽 Die 정보 = 그 Die).</summary>
    /// <summary>
    /// 같은 Wafer 의 READY E-Test 파일(GdsKlarfAnalysisFormSmoke 와 같은 방법). 준비 목록(같은 Device)에 있으면 그 파일,
    /// 없으면 점검 ET_OTHER_DEVICE 문구 "(Device 공정, ...)" 의 Device 로 파일 목록을 찾아 같은 Wafer 파일. 없으면 null.
    /// </summary>
    private static DataRow FindSameWaferEtFile(DACrux.SEMDMS.BSL.GdsKlarfAnalysis analysisBsl, string factory, long stepSeq, string device)
    {
        DataSet ready;
        try { ready = analysisBsl.GetWaferReady(factory, stepSeq, device); }
        catch (Exception ex) { Console.WriteLine("참고 / E-Test 준비 목록 조회 실패: " + ex.Message); return null; }
        DataRow file = ready.Tables["READY_ET"].Rows.Cast<DataRow>()
            .Where(r => { long v; return long.TryParse(r["SRC_FILE_SEQ"].ToString(), out v) && v > 0; })
            .OrderBy(r => r["SELECTED"].ToString() == "Y" ? 0 : 1).FirstOrDefault();
        if (file != null || ready.Tables["WAFER_KEY"].Rows.Count == 0) return file;
        DataRow other = ready.Tables["READY_CHECK"].Rows.Cast<DataRow>().FirstOrDefault(r => r["CODE"].ToString() == "ET_OTHER_DEVICE");
        if (other == null) return null;
        string message = other["MESSAGE"].ToString();
        string waferKey = ready.Tables["WAFER_KEY"].Rows[0]["WAFER_KEY"].ToString();
        var etBsl = new DACrux.SEMDMS.BSL.GdsEtResult();
        foreach (string etDevice in message.Substring(message.LastIndexOf('(') + 1).TrimEnd(')').Split(',').Select(s => s.Trim().Split(' ')[0]).Where(s => s.Length > 0).Distinct())
        {
            try
            {
                file = etBsl.GetEtFileBrowseList(factory, etDevice, "", "").Rows.Cast<DataRow>()
                    .FirstOrDefault(r => String.Equals(r["WAFER_ID"].ToString(), waferKey, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) { Console.WriteLine("참고 / E-Test 파일 목록 조회 실패(" + etDevice + "): " + ex.Message); }
            if (file != null) return file;
        }
        return null;
    }

    private static bool ShowsDie(int x, int y)
    {
        var shared = frmGdsDieAnalysis.SharedDetailForm as NexplantQMS.GdsMap.GdsDieAnalysisForm;
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
            if (w.Elapsed.TotalSeconds > seconds) throw new TimeoutException("상세보기 대기 시간 초과");
            Application.DoEvents();
            System.Threading.Thread.Sleep(20);
        }
        if (task.IsFaulted) throw task.Exception.InnerException;
    }

    /// <summary>
    /// OpenGL 도면은 DrawToBitmap 으로 그려지지 않아 PrintWindow(PW_RENDERFULLCONTENT = 화면 합성 내용 포함)로 MDI 창을 찍는다.
    /// 화면 복사(CopyFromScreen)는 화면 배율 150% 에서 도면 위에 회색 사각형이 덮여 찍혀(2026-10-22) 쓰지 않는다.
    /// 화면 밖(-32000)에서는 내용이 그려지지 않아 찍는 동안만 화면 왼쪽 위로 옮긴다(맨 앞 고정 / 활성화는 하지 않음).
    /// </summary>
    private static void CaptureScreen(Form mdi, Form window, string png)
    {
        Point keep = mdi.Location;
        mdi.Location = new Point(0, 0);
        Pump(1500);
        using (var bmp = new Bitmap(mdi.Width, mdi.Height))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hdc = g.GetHdc();
                try { PrintWindow(mdi.Handle, hdc, 2); }
                finally { g.ReleaseHdc(hdc); }
            }
            bmp.Save(png, ImageFormat.Png);
        }
        mdi.Location = keep;
        Application.DoEvents();
        Console.WriteLine("그림: " + png);
    }

    [DllImport("user32.dll")]
    private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, uint flags);

    private static int Finish()
    {
        Console.WriteLine(String.Format("GDS_KLARF_SIMPLE_FORM_SMOKE={0} ({1}/{2})", failures == 0 ? "PASS" : "FAIL", checks - failures, checks));
        return failures == 0 ? 0 : 1;
    }

    /// <summary>두 결과의 ANL_INFO Chain 위 Defect 수와 Die 별 (Defect 수, Chain 위, 걸린 Chain 수)가 같은지.</summary>
    private static bool SameDies(DataSet a, DataSet b)
    {
        if (a == null || b == null) return false;
        if (Convert.ToInt32(a.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]) != Convert.ToInt32(b.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"])) return false;
        Func<DataSet, Dictionary<Point, string>> dies = ds => ds.Tables["DIE"].Rows.Cast<DataRow>().ToDictionary(DiePoint,
            r => r["DEFECT_COUNT"] + "/" + r["IN_CHAIN_COUNT"] + "/" + r["HIT_CHAIN_COUNT"]);
        Dictionary<Point, string> x = dies(a), y = dies(b);
        return x.Count == y.Count && x.All(kv => y.ContainsKey(kv.Key) && y[kv.Key] == kv.Value);
    }

    /// <summary>화면을 그림으로 남긴다(설계 문서 구현 결과용).</summary>
    private static void Save(Form form, string path)
    {
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(path, ImageFormat.Png);
        }
        Console.WriteLine("그림: " + path);
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

    /// <summary>기대 Die Index (Map 이 없거나 사용 Die 가 아니면 0).</summary>
    private static int ExpectedIndex(int x, int y)
    {
        return indexMap == null ? 0 : indexMap.DieNumberOf(new DACrux.SEMDMS.Interface.DiePoint(x, y));
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

    private static object Get(Type type, object target, string name)
    {
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null) throw new MissingFieldException(type.Name, name);
        return f.GetValue(target);
    }

    /// <summary>비공개 메서드 호출. 같은 이름의 오버로드는 인자 수로 고른다.</summary>
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
