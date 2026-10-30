using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Die 상세 분석 화면(화면 B). 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5절 / 7.3절
    ///
    /// 연관 분석 화면(DMS ENGUI frmGdsKlarfAnalysis)이 서버 분석 결과 DataSet 과 Die 를 넘기면
    /// GDS Chip 도면 위에 저장된 Chain 색과 KLARF Defect 위치를 함께 보여 준다. 판정은 다시 하지 않는다(같은 결과를 그대로 표시).
    /// ENGUI(.NET 4.0)는 이 프로젝트(.NET 4.8)를 참조하지 않으므로 호스트(frmGdsDieAnalysis)가 EXE 를 실행 시 읽어
    /// ShowDie(DataSet, int, int) 를 리플렉션으로 부르고, 이미지 보기는 DefectImageRequested(Action) 로 호스트에 돌려준다.
    ///
    /// 처리 흐름
    ///  1) ShowDie -> 분석 기준(ANL_INFO) / 선택 Chip 좌표 정보 표시. 상세 화면은 이 Chip 1개만 분석한다(2026-10-12 요청, 설계 14.4절)
    ///  2) 도면 키(Factory, Device, 보기 기준, 전체 도면, Chain 목록, Chain 저장 시각)가 바뀌었을 때만 도면 + Chain Member 를 읽는다(전체 Layer 18.7초 -> 기본은 Chain Layer 만).
    ///     읽기는 GdsDieMapCache 를 거친다: [분석] 때 미리 읽은 것(PreloadMap)이 있으면 DB 를 부르지 않는다(2026-10-16).
    ///  3) 상세보기 창은 Die 마다 따로 열리고 이 화면(도면) 하나를 같이 쓴다(2026-10-16, 문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md).
    ///     호스트가 보이는 창으로 이 화면을 옮기고 그 창의 결과 / 보기 상태(CaptureViewState)로 ShowDie 를 부른다.
    ///     도면 키가 같으면 Defect 표시 / Chain 목록 / Defect 목록만 다시 만든다(서버 호출 없음)
    ///  4) E-Test (2026-10-16, 통합 분석 설계 8.2 / 15.3절): Chain 목록에 4분류 / E-Test 판정 / Fail Para, 도면은 Chain 마다 저장 색,
    ///     E-Test 값 탭. 역추적 기준 Chain(focusChainSeq)을 받으면 그 Chain 만 그리고 Defect 를 그 Chain 까지 거리 순으로 보인다.
    ///     역추적으로 바꿔도 Chain 목록 순서 / 스크롤 위치는 그대로 두고 그 행만 선택한다.
    ///     E-Test 열(DIE_CHAIN.ET_STATUS / CLASS4, ET_VALUE 표)이 없는 결과면 해당 칸을 비워 둔다.
    ///  5) 배치 (2026-10-19, 설계 문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md 10절): 화면 A 와 같은 구조.
    ///     위 = Wafer + 분석 기준 요약(GDS / KLARF / E-Test) / Die 정보, 왼쪽 = GDS 도면, 오른쪽 = 기준(읽기 전용) + 표(8번에서 한 패널로).
    ///     문구 / 용어(Defect 수, Defect ID, RoughBin, 비교 불가, Spec Out / Fail Die)는 화면 A 와 같게.
    ///  6) [GDS / KLARF] 탭 (2026-10-20, 설계 문서/설계/05_연관분석/2026-10-20_상세보기_GDS_KLARF_합친탭_설계.md):
    ///     예전 [GDS] Chain 표와 [KLARF] Defect 표를 한 표로 합친다. 한 행 = (Chain, Defect). 같은 Chain 의 행은 Chain 칸을 한 칸처럼 그린다.
    ///     걸린 Defect 가 없는 Chain 은 빈 1행, 두 Chain 에 걸린 Defect 는 두 묶음에 모두(Defect ID 굵은 글자),
    ///     어느 Chain 에도 안 걸린 Defect 는 맨 아래 "Chain 밖 Defect" 묶음(가까운 거리 순). 서버 결과(AnalyzeDie)는 그대로 쓴다.
    ///  7) 화면 A · Simple 같은 모양 (2026-10-22 요청 "두 화면에서 동일하게", 설계 문서/설계/05_연관분석/2026-10-22_상세보기_오른쪽_한패널_설계.md 12절):
    ///     어느 화면이 열어도 같은 모양이다(예전 Simple 보기 표시 DETAIL_VIEW 는 없앰). Simple 상세 요청과 한 패널 요청을 합친다.
    ///     위쪽 "분석 기준" 칸과 표 위 요약 글자는 숨기고, 표 위 체크는 [걸린 / Fail Chain 만](2026-10-28 기본 켬, 끄면 모든 Chain) / [전체 표시].
    ///     [전체 표시](2026-10-23)는 모든 Chain 의 표시 체크를 한 번에 켜거나 끄고, Chain 하나를 바꾸면 표시 체크를 따라간다(모두 켜짐 = 켬).
    ///     Chain 밖 Defect 묶음은 늘 보인다(체크 없음, 2026-10-22).
    ///     도면 Defect 는 Chain 영역 빨강 / 밖 노랑으로 채우고 흰 점선 · 흰 테두리 · 번호는 그리지 않으며(DefectOverlayStyle.Filled),
    ///     크기는 분석 Defect 영역 그대로(크기 + 허용오차 양쪽 / 중심 정사각형)다.
    ///  8) 오른쪽 한 패널 (2026-10-22, 설계 문서/설계/05_연관분석/2026-10-22_상세보기_오른쪽_한패널_설계.md):
    ///     탭 [GDS / KLARF] [E-Test] [종합] 을 없애고 위 = GDS / KLARF 표, 아래 = 위 표에서 고른 행 Chain 의 이 Die E-Test 값 표
    ///     (고른 행 없음 = Die 전체, Chain 밖 행 = 빈 표). E-Test 가 있는 결과는 아래 표를 늘 보인다(2026-10-23 [E-Test 값] 체크 삭제).
    ///     분류 글자색은 표 분류 칸에, E-Test 기준은 기준 묶음 3째 줄에 둔다. "가까운 Chain" 열 / 4분류 상태 칸 5개는 뺐다.
    ///     E-Test 를 연결하지 않은 결과는 아래 표 / E-Test 줄 / [분류 · ET · Fail Para] 열을 숨긴다(비어 있는 칸).
    ///  9) [다시 분석] (2026-10-22, 설계 13절): 위쪽 오른쪽 버튼. ReanalyzeRequested 로 호스트에 알리고 호스트는 이 화면을 연 분석 화면의
    ///     [다시 분석] / [분석] 을 부른다(그 화면의 지금 설정으로 Wafer 다시 분석 -> 동기화로 이 화면도 새 결과). 기준의 최종 저장 / 분석 시각은 보이지 않는다.
    /// </summary>
    public partial class GdsDieAnalysisForm : Form
    {
        private const int PageRows = 10000;

        private DataSet _analysis;
        private string _factory = string.Empty;
        private string _device = string.Empty;
        private string _viewSide = GdsSideTransform.Back;
        private long _stepSeq;
        private double _tolerance;
        private bool _squareArea;
        private string _loadedKey;
        private int _loadVersion;
        /// <summary>ShowDie 순번. 도면을 기다리는 동안 다른 창(Die)이 들어오면 늦게 끝난 쪽은 표시하지 않는다.</summary>
        private int _showVersion;
        /// <summary>다음 FillChainDefectGrid 는 전 목록의 고른 행 / 스크롤을 이어받지 않는다(ShowDie 로 다른 창 / 새 결과가 들어왔을 때).</summary>
        private bool _freshChainList;
        private bool _busy;
        private bool _updating;
        private Point? _currentDie;
        private string _focusChainSeq;
        private Dictionary<long, IList<string>> _chainMembers = new Dictionary<long, IList<string>>();
        private readonly Dictionary<long, IList<ChainTraceElement>> _chainElements = new Dictionary<long, IList<ChainTraceElement>>();
        /// <summary>기준 E-Test 줄 중 분석 결과(파일) 부분. 빈 값 = E-Test 를 연결하지 않은 분석. Die 판정은 ShowEtCriteria 가 뒤에 붙인다.</summary>
        private string _etCriteria = string.Empty;
        /// <summary>이 결과에 E-Test 가 있는지(ANL_INFO.ET_TEST_PROGRAM 있음). 있으면 아래 E-Test 값 표를 늘 보이고, 없으면 아래 표 / E-Test 줄 / E-Test 열을 숨긴다.</summary>
        private bool _etAvailable;
        /// <summary>아래 E-Test 값 표가 지금 보이는 범위: null = Die 전체, OutsideEtKey = Chain 밖 Defect, 그 밖 = CHAIN_SEQ.</summary>
        private string _etKey;
        private const string OutsideEtKey = "OUT";
        /// <summary>아래 E-Test 값 표 높이를 한 번 맞췄는지(ApplyEtPanel).</summary>
        private bool _etSplitSized;

        /// <summary>GDS 도면 조회 서비스. null 이면 원격 호출 RO 를 만든다(Probe 는 같은 프로세스 BSL 로 바꾼다).</summary>
        public DACrux.SEMDMS.RO.GdsMapView MapService { get; set; }

        /// <summary>Chain Member 조회 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public DACrux.SEMDMS.RO.GdsChain ChainService { get; set; }

        /// <summary>
        /// 화면마다 MapService / ChainService 를 주지 않았을 때 쓰는 서비스(null = 원격 호출 RO). 호스트가 만드는 공유 화면과 미리 읽기가 쓴다.
        /// Smoke 가 같은 프로세스 BSL 로 바꾼다 (2026-10-16).
        /// </summary>
        public static DACrux.SEMDMS.RO.GdsMapView DefaultMapService { get; set; }
        public static DACrux.SEMDMS.RO.GdsChain DefaultChainService { get; set; }

        /// <summary>Defect 이미지 보기 요청(STEP_SEQ, DEFECTID). DMS 호스트가 기존 FTP 이미지 표시 함수로 채운다.</summary>
        public Action<long, long> DefectImageRequested { get; set; }

        /// <summary>[다시 분석] 요청(2026-10-22). DMS 호스트가 이 화면을 연 분석 화면의 다시 분석으로 채운다(없으면 버튼은 상태줄 안내만).</summary>
        public Action ReanalyzeRequested { get; set; }

        /// <summary>오류를 메시지 창으로도 알릴지(기본 true). 자동 검사(Smoke)는 false 로 두고 상태줄만 본다.</summary>
        public bool ShowErrorDialogs { get; set; } = true;

        /// <summary>마지막 상태줄 문구(Smoke 확인용).</summary>
        public string StatusText { get { return lblStatus.Text; } }

        /// <summary>도면 조회 중인지(Probe 대기용).</summary>
        public bool IsBusy { get { return _busy; } }

        /// <summary>지금 그린 도면의 키(Factory|Device|보기 기준|배경|Chain 번호 목록|Chain 저장 시각). Probe 확인용.</summary>
        public string LoadedMapKey { get { return _loadedKey; } }

        /// <summary>지도에 표시 중인 Defect 수 / Chain 수(Probe 확인용).</summary>
        public int DefectMarkerCount { get { return map.DefectMarkerCount; } }

        public int VisibleChainCount { get; private set; }

        public GdsDieAnalysisForm()
        {
            InitializeComponent();
            GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            // 두 화면 같은 모양(2026-10-22, 7번): 위쪽 분석 기준 칸 / 표 위 요약 글자 숨김, [걸린 / Fail Chain 만] 기본 끔(모든 Chain),
            // 도면 Defect = 채움(Chain 안 빨강 / 밖 노랑, 점선 · 번호 없음)
            lblSumTitle.Visible = false;
            lblSumGds.Visible = false;
            lblSumKlarf.Visible = false;
            lblSumEt.Visible = false;
            lblChainTitle.Visible = false;
            // 기준 묶음 E-Test 줄(E-Test 공정 / Wafer / 판정 / 이 Die 판정)은 보이지 않는다(2026-10-22 요청). Die 판정은 위쪽 Die 줄에 있다
            lblEtText.Visible = false;
            map.DefectStyle = DefectOverlayStyle.Filled;
            SetEtSplit();
            // Chain 칸은 묶음의 첫 행(가려지면 화면 첫 행)에만 글자를 그린다. 스크롤하면 새 첫 행을 다시 그리도록 전체를 다시 그린다 (2026-10-20)
            dgvChainDefects.Scroll += (s, e) => dgvChainDefects.Invalidate();
        }

        /// <summary>DACrux 호스트(frmGdsDieAnalysis)가 쓰는 생성자. GDS Map 조회 화면과 같은 형식으로 맞춘다.</summary>
        public GdsDieAnalysisForm(bool hostedByDms) : this()
        {
        }

        #region [분석 결과 받기]

        /// <summary>호스트가 리플렉션으로 부르는 진입점. 결과를 기다리지 않는다.</summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex)
        {
            _ = ShowDieAsync(analysis, xIndex, yIndex, null);
        }

        /// <summary>역추적 기준 Chain 을 함께 받는 진입점(2026-10-16). focusChainSeq = CHAIN_SEQ, null 이면 Die 전체 보기.</summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex, string focusChainSeq)
        {
            _ = ShowDieAsync(analysis, xIndex, yIndex, focusChainSeq, null);
        }

        /// <summary>
        /// 상세보기 창마다 저장한 보기 상태(CaptureViewState 가 돌려준 값)를 함께 받는 진입점(2026-10-16).
        /// viewState 가 있으면 그 창에서 보던 역추적 Chain / 목록 / 확대 위치로 되돌리고, null 이면 처음 여는 것처럼 Defect 범위로 확대한다.
        /// </summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex, string focusChainSeq, object viewState)
        {
            _ = ShowDieAsync(analysis, xIndex, yIndex, focusChainSeq, viewState);
        }

        public Task ShowDieAsync(DataSet analysis, int xIndex, int yIndex)
        {
            return ShowDieAsync(analysis, xIndex, yIndex, null, null);
        }

        public Task ShowDieAsync(DataSet analysis, int xIndex, int yIndex, string focusChainSeq)
        {
            return ShowDieAsync(analysis, xIndex, yIndex, focusChainSeq, null);
        }

        /// <summary>
        /// 분석 결과와 Die 를 받아 화면을 채운다. 도면 키가 바뀌었으면 도면을 읽은 뒤(캐시에 있으면 DB 를 부르지 않음) 표시한다.
        /// 도면을 기다리는 동안 다른 창의 ShowDie 가 들어오면 늦게 끝난 이 호출은 표시하지 않는다(_showVersion).
        /// 지금 도면과 같은 키인데 다른 키 도면을 읽는 중이면 그 읽기를 버린다(늦게 끝나 이 창 도면을 덮지 않게, 읽은 데이터는 캐시에 남음).
        /// 보기 상태가 없으면(처음 여는 창 / 새 결과) 상태줄과 Chain 목록의 고른 행 / 스크롤은 다른 창 것을 이어받지 않는다.
        /// 실패는 상태줄 / 메시지로 알리고 예외를 밖으로 던지지 않는다(호스트가 리플렉션으로 부르므로).
        /// </summary>
        public async Task ShowDieAsync(DataSet analysis, int xIndex, int yIndex, string focusChainSeq, object viewState)
        {
            int show = ++_showVersion;
            var state = viewState as ViewState;
            _focusChainSeq = state != null ? state.FocusChainSeq : String.IsNullOrEmpty(focusChainSeq) ? null : focusChainSeq;
            if (analysis == null || !analysis.Tables.Contains("ANL_INFO") || analysis.Tables["ANL_INFO"].Rows.Count == 0)
            {
                SetStatus("분석 결과 없음");
                return;
            }
            DataRow info = analysis.Tables["ANL_INFO"].Rows[0];
            _analysis = analysis;
            ApplyViewMode();
            _factory = info["FACTORY"].ToString();
            _device = info["DEVICE_ID"].ToString();
            _viewSide = GdsSideTransform.Normalize(info["VIEW_SIDE"].ToString());
            _stepSeq = Convert.ToInt64(info["STEP_SEQ"], CultureInfo.InvariantCulture);
            _tolerance = Convert.ToDouble(info["TOLERANCE_UM"], CultureInfo.InvariantCulture);
            lblWaferInfo.Text = String.Format("{0} / {1}  (GDS {2})", info["WAFER_ID"], info["STEP_ID"], _device);
            // Defect 영역(2026-10-16): 크기 + 허용오차 / 중심 사각형(한 변 = 허용오차 x 2, 크기 끔)
            _squareArea = info["USE_DEFECT_SIZE"].ToString() != "Y";
            ShowCriteria(info);
            // 상세 화면은 이 Chip 1개만 분석한다(2026-10-12 요청, 설계 14.4절). 다른 Chip 은 연관 분석 화면에서 다시 고른다.
            _currentDie = new Point(xIndex, yIndex);
            lblDieInfo.Text = "Die (" + xIndex + ", " + yIndex + ")";

            bool loaded = false;
            if (MapKey() != _loadedKey)
            {
                try
                {
                    await LoadMapAsync();
                    loaded = true;
                }
                catch (Exception ex)
                {
                    if (show == _showVersion) ShowError("GDS 도면 조회", ex);
                    return;
                }
            }
            else if (_busy)
            {
                // 다른 창의 도면(다른 키)을 읽는 중: 그 읽기 결과는 도면에 반영하지 않는다(LoadMapAsync 의 순번 확인)
                _loadVersion++;
                SetBusy(false);
            }
            if (show != _showVersion || MapKey() != _loadedKey) return;
            if (state != null) RestoreListState(state);
            _freshChainList = true;
            ShowCurrent();
            if (state != null) RestoreSelectionAndView(state);
            else if (!loaded) SetStatus(String.Format("Die ({0}, {1}) 표시", xIndex, yIndex));
        }

        /// <summary>
        /// 분석 기준(읽기 전용, 이 결과의 ANL_INFO): 위쪽 요약 3칸은 화면 A "지금 기준" 칸과 같은 문구,
        /// 오른쪽 기준 묶음에 자세한 값(1줄 GDS / 2줄 KLARF / 3줄 E-Test, 2026-10-22 [E-Test] 탭 기준을 3줄로 옮김).
        /// </summary>
        private void ShowCriteria(DataRow info)
        {
            string chainSide = Col(info, "CHAIN_SIDE");
            bool mirror = Col(info, "MIRROR") == "Y";
            // 검사 면(KLARF FS 고정)은 보이지 않고 Chain 면 + 반전만 보인다 (2026-10-20)
            string sides = chainSide.Length == 0 ? "" : String.Format("Chain {0}{1}", SideCode(chainSide), mirror ? " -> 반전" : "");
            string area = _squareArea ? "중심 사각형 한 변 " + (_tolerance * 2).ToString("0.#", CultureInfo.InvariantCulture)
                                      : "크기 + 허용오차 " + _tolerance.ToString("0.#", CultureInfo.InvariantCulture);
            string offset = String.Format("보정 ({0}, {1})", Col(info, "OFFSET_X"), Col(info, "OFFSET_Y"));
            string[] stepIds = Col(info, "STEP_IDS").Length > 0 ? Col(info, "STEP_IDS").Split(',') : new[] { Col(info, "STEP_ID") };
            string program = Col(info, "ET_TEST_PROGRAM");
            bool specChanged = Col(info, "ET_SPEC_CHANGED") == "Y";

            lblSumGds.Text = "GDS · " + (sides.Length > 0 ? sides : GdsSideTransform.Text(_viewSide) + " 보기");
            lblSumKlarf.Text = String.Format("KLARF · Step {0}개 · {1} · {2}", stepIds.Length, area, offset);
            lblSumEt.Text = program.Length == 0 ? "E-Test · 없음" : "E-Test · " + program + (specChanged ? " · Spec 바뀜" : "");
            lblMapSide.Text = "보기 = " + GdsSideTransform.Text(_viewSide);
            // [GDS / KLARF] 기준 2줄 (2026-10-20 탭 합침): 1줄 = GDS, 2줄 = KLARF. Chain 최종 저장 / 분석 시각은 보이지 않는다(2026-10-22 요청)
            lblGdsText.Text = String.Format("보기 {0}{1} · Chain {2}", GdsSideTransform.Text(_viewSide),
                chainSide.Length == 0 ? "" : " · Chain 면 " + SideCode(chainSide), Col(info, "CHAIN_COUNT"))
                + Environment.NewLine
                + String.Format("Step {0} · Defect 영역 {1} · 좌표 {2}", String.Join(", ", stepIds), area, offset);
            _etCriteria = program.Length == 0 ? ""
                : String.Format("E-Test 공정 {0} · Wafer {1} · 판정 {2}{3}", program, Col(info, "ET_WAFER_ID"), Col(info, "ET_JUDGE_TIME"),
                    specChanged ? " · 판정 뒤 Spec 바뀜" : "");
            lblEtText.Text = _etCriteria;
        }

        /// <summary>
        /// 결과마다 보기를 맞춘다(8번): E-Test 를 연결하지 않은 결과는 아래 표 /
        /// [분류 · ET · Fail Para] 열(비어 있음)을 숨긴다(기준 E-Test 줄은 2026-10-22 부터 늘 숨김). 어느 화면이 연 결과든 같은 규칙이다(2026-10-22 두 화면 같은 모양).
        /// 화면 하나를 두 화면의 상세보기 창이 같이 쓰기 때문에 ShowDie 마다 부른다.
        /// </summary>
        private void ApplyViewMode()
        {
            _etAvailable = Col(_analysis.Tables["ANL_INFO"].Rows[0], "ET_TEST_PROGRAM").Length > 0;
            colClass.Visible = _etAvailable;
            colEt.Visible = _etAvailable;
            colFailPara.Visible = _etAvailable;
            pnlGdsCriteria.Height = lblGdsText.Bottom + 4;
            ApplyEtPanel();
        }

        /// <summary>
        /// 아래 E-Test 값 표를 펴거나 접는다(E-Test 있는 결과면 늘 편다, 2026-10-23 [E-Test 값] 체크 삭제).
        /// 표가 다시 보일 때 첫 행을 저절로 고르는 것은 행 선택 처리(Chain 도형 칠하기 / 상태줄)를 돌리지 않고 지운다.
        /// </summary>
        private void ApplyEtPanel()
        {
            bool show = _etAvailable;
            if (splitRight.Panel2Collapsed == show)
            {
                _updating = true;
                splitRight.Panel2Collapsed = !show;
                dgvEtValues.ClearSelection();
                _updating = false;
            }
            // 아래 표를 처음 펼 때(창 크기가 정해진 뒤) 한 번 더 35% 로 맞춘다. 그 뒤 사용자가 옮긴 위치는 그대로 둔다
            if (show && !_etSplitSized) _etSplitSized = SetEtSplit();
        }

        /// <summary>
        /// 아래 E-Test 값 표 = 오른쪽 높이의 35%(2026-10-22). 디자이너를 다른 화면 배율로 저장하면 나누기 위치는 배율대로 줄지 않아
        /// 아래 표가 최소 높이로 눌리거나 칸 밖으로 나가므로 비율로 다시 맞춘다. 높이가 모자라면 그대로 두고 false.
        /// </summary>
        private bool SetEtSplit()
        {
            if (splitRight.Height <= splitRight.Panel1MinSize + splitRight.Panel2MinSize + splitRight.SplitterWidth) return false;
            splitRight.SplitterDistance = Math.Max(splitRight.Panel1MinSize, splitRight.Height * 65 / 100);
            return true;
        }

        /// <summary>[걸린 / Fail Chain 만]. 기본 끔 = 모든 Chain (2026-10-22 두 화면 같은 모양, Simple 요청 "기본으로 전부").</summary>
        private bool HitOnly { get { return chkHitOnly.Checked; } }

        /// <summary>면 문구 FRONT -> FS / BACK -> BS (화면 A 와 같음).</summary>
        private static string SideCode(string side)
        {
            string s = (side ?? "").Trim().ToUpperInvariant();
            return s == "FRONT" ? "FS" : s == "BACK" ? "BS" : s;
        }

        private string MapKey()
        {
            return MapRequest.From(_analysis, chkBackground.Checked).Key;
        }

        /// <summary>
        /// [분석] 결과로 상세 화면 도면 데이터(도형 + Chain Member)를 미리 읽는다(백그라운드, 기다리지 않음, 2026-10-16 설계 5.3절).
        /// 연관 분석 화면이 호스트(frmGdsDieAnalysis)를 거쳐 리플렉션으로 부른다. 기본 보기(전체 도면 끔) 키로 읽는다.
        /// 실패하면 상세 화면을 열 때 다시 읽는다(GdsDieMapCache).
        /// </summary>
        public static void PreloadMap(DataSet analysis)
        {
            if (!HasInfo(analysis)) return;
            MapRequest request = MapRequest.From(analysis, false);
            DACrux.SEMDMS.RO.GdsMapView mapService = DefaultMapService ?? new DACrux.SEMDMS.RO.GdsMapView();
            DACrux.SEMDMS.RO.GdsChain chainService = DefaultChainService ?? new DACrux.SEMDMS.RO.GdsChain();
            GdsDieMapCache.GetOrLoad(request.Key, () => ReadMap(mapService, chainService, request, null));
        }

        /// <summary>결과의 기본 보기 도면 키(없으면 null). 호스트가 공유 화면을 버릴지(다른 Device) 판단할 때 쓴다.</summary>
        public static string MapKeyOf(DataSet analysis)
        {
            return HasInfo(analysis) ? MapRequest.From(analysis, false).Key : null;
        }

        /// <summary>미리 읽은 도면 데이터를 버린다(연관 분석 화면과 상세보기 창을 모두 닫았을 때 호스트가 부른다).</summary>
        public static void ClearMapCache()
        {
            GdsDieMapCache.Clear();
        }

        private static bool HasInfo(DataSet analysis)
        {
            return analysis != null && analysis.Tables.Contains("ANL_INFO") && analysis.Tables["ANL_INFO"].Rows.Count > 0;
        }

        #endregion

        #region [도면 / Chain 조회]

        /// <summary>
        /// GDS 도면과 Chain Member 를 읽어 도면을 만든다. 기본은 Chain Member 가 쓰는 Layer 만 읽고,
        /// [전체 도면] 이면 Map 의 모든 Layer 를 읽는다. 저장 기준과 보기 기준이 다르면 GDS 좌우 반전(GdsSideTransform).
        /// 읽기는 GdsDieMapCache 를 거친다(같은 키를 미리 읽었거나 읽는 중이면 그것을 기다림, 2026-10-16).
        /// 같은 화면에서 다른 요청이 들어오면 이전 조회 결과는 버린다(_loadVersion).
        /// </summary>
        private async Task LoadMapAsync()
        {
            int version = ++_loadVersion;
            MapRequest request = MapRequest.From(_analysis, chkBackground.Checked);
            DACrux.SEMDMS.RO.GdsMapView mapService = MapService ?? DefaultMapService ?? new DACrux.SEMDMS.RO.GdsMapView();
            DACrux.SEMDMS.RO.GdsChain chainService = ChainService ?? DefaultChainService ?? new DACrux.SEMDMS.RO.GdsChain();
            var progress = new Progress<string>(SetStatus);

            SetBusy(true);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                Task<GdsDieMapData> task = GdsDieMapCache.GetOrLoad(request.Key, () => ReadMap(mapService, chainService, request, progress));
                if (!task.IsCompleted) SetStatus("GDS 도면 조회 중");
                GdsDieMapData loaded = await task;
                // 기다리는 동안 다른 창으로 넘어갔으면(순번 바뀜, 또는 지금 창이 원하는 도면이 아님) 도면에 반영하지 않는다
                if (version != _loadVersion || request.Key != MapKey()) return;
                double dbSeconds = watch.Elapsed.TotalSeconds;
                map.SetDefectMarkers(null, 0);
                map.ShowPlacedElements(loaded.TopStructure, loaded.Colors, loaded.Items);
                map.RebuildChainTraceItems();
                _chainMembers = loaded.Members;
                _chainElements.Clear();
                _loadedKey = request.Key;
                SetStatus(String.Format("도면 조회 완료: {0} / 도형 {1:N0}", request.Device, loaded.Items.Count));
                System.Diagnostics.Trace.WriteLine("GdsDieAnalysis DB " + dbSeconds.ToString("0.0") + "s");
            }
            finally
            {
                if (version == _loadVersion) SetBusy(false);
            }
        }

        /// <summary>
        /// 백그라운드: Map 헤더 / Layer / 도형 페이지 / Chain Member 를 읽고 보기 기준으로 맞춘다(반전 전 원본은 버림).
        /// 화면 컨트롤을 만지지 않는다. progress 는 null 일 수 있다(미리 읽기).
        /// </summary>
        private static GdsDieMapData ReadMap(DACrux.SEMDMS.RO.GdsMapView mapService, DACrux.SEMDMS.RO.GdsChain chainService,
            MapRequest request, IProgress<string> progress)
        {
            Report(progress, "GDS Map 정보 조회 중");
            DataTable info = mapService.GetMapInfo(request.Factory, request.Device);
            if (info.Rows.Count == 0 || info.Rows[0]["MAP_STATUS"].ToString() != "READY")
                throw new InvalidOperationException("저장 완료(READY)된 GDS Map 이 없습니다: " + request.Factory + " / " + request.Device);
            var result = new GdsDieMapData
            {
                TopStructure = info.Rows[0]["TOP_STRUCTURE"].ToString(),
                StoredSide = GdsSideTransform.Normalize(info.Rows[0]["VIEW_SIDE"].ToString())
            };
            DataTable layers = mapService.GetMapLayerList(request.Factory, request.Device);
            foreach (DataRow row in layers.Rows)
            {
                // Layer 키 = 번호 + DataType(TEXT 는 TEXTTYPE) (2026-10-20)
                int layerKey = GdsLayerKey.FromRow(row);
                if (request.Background || request.ChainLayers.Count == 0 || request.ChainLayers.Contains(layerKey))
                    result.Colors[layerKey] = Color.FromArgb(Convert.ToInt32(row["COLOR_ARGB"], CultureInfo.InvariantCulture));
            }
            result.Items = LoadPlacedElements(mapService, request.Factory, request.Device, result.Colors.Keys.ToList(), progress);
            if (GdsSideTransform.NeedsMirror(result.StoredSide, request.ViewSide))
                result.Items = GdsSideTransform.MirrorAll(result.Items);
            ReadChainMembers(chainService, request, result.Members, progress);
            return result;
        }

        /// <summary>Chain Member 를 한 번에 읽는 Chain 수(Chain 번호 범위 1개). GDS Map View 의 Chain 상세 범위와 같다.</summary>
        private const int MemberRangeSize = 5000;

        /// <summary>
        /// 결과 CHAIN 표의 Chain Member(도형 ID)를 Chain 번호 범위로 묶어 읽는다 (2026-10-21).
        /// Chain 마다 부르면 U8(Chain 27,342개)은 서버 호출이 2만 7천 번이라 상세보기 열기가 1분 이상 걸렸다.
        /// 범위 안의 다른 Chain(초안 등)은 버린다. 서버에 범위 조회가 없으면(이전 서버) Chain 마다 읽는 예전 방식으로 돌아간다.
        /// 근거: 문서/설계/05_연관분석/2026-10-21_U8_연관분석_영향_확인.md A1
        /// </summary>
        private static void ReadChainMembers(DACrux.SEMDMS.RO.GdsChain chainService, MapRequest request,
            Dictionary<long, IList<string>> members, IProgress<string> progress)
        {
            List<long> seqs = request.ChainSeqs;
            var wanted = new HashSet<long>(seqs);
            bool rangeAvailable = false;
            try
            {
                for (int start = 0; start < seqs.Count; start += MemberRangeSize)
                {
                    int end = Math.Min(start + MemberRangeSize, seqs.Count) - 1;
                    Report(progress, "Chain Member 조회 중 " + (end + 1) + " / " + seqs.Count);
                    DataTable rows = ReadMemberRange(chainService, request, seqs[start], seqs[end]);
                    rangeAvailable = true;
                    foreach (DataRow r in rows.Rows)
                    {
                        long seq = Convert.ToInt64(r["CHAIN_SEQ"], CultureInfo.InvariantCulture);
                        if (!wanted.Contains(seq)) continue;
                        IList<string> list;
                        if (!members.TryGetValue(seq, out list)) members[seq] = list = new List<string>();
                        list.Add(r["PLACED_ELEMENT_ID"].ToString());
                    }
                }
                foreach (long seq in seqs)
                    if (!members.ContainsKey(seq)) members[seq] = new List<string>();
            }
            catch (Exception ex) when (!rangeAvailable && (ex is System.Runtime.Remoting.RemotingException || ex is MissingMethodException))
            {
                members.Clear();
                int no = 0;
                foreach (long seq in seqs)
                {
                    Report(progress, "Chain Member 조회 중 " + (++no) + " / " + seqs.Count);
                    DataTable one = chainService.GetChainMembers(request.Factory, request.Device, seq.ToString(CultureInfo.InvariantCulture));
                    members[seq] = one.Rows.Cast<DataRow>().Select(r => r["PLACED_ELEMENT_ID"].ToString()).ToList();
                }
            }
        }

        /// <summary>범위 조회 1번. 이전 RO(함수 없음)면 이 함수를 부를 때 MissingMethodException 이 나도록 따로 둔다.</summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static DataTable ReadMemberRange(DACrux.SEMDMS.RO.GdsChain chainService, MapRequest request, long fromSeq, long toSeq)
        {
            return chainService.GetChainMembersRange(request.Factory, request.Device,
                fromSeq.ToString(CultureInfo.InvariantCulture), toSeq.ToString(CultureInfo.InvariantCulture));
        }

        private static void Report(IProgress<string> progress, string text)
        {
            if (progress != null) progress.Report(text);
        }

        /// <summary>
        /// Layer 마다 이전 페이지 마지막 ID 다음부터 PageRows 씩 받아 행을 해석한다.
        /// GdsMapViewForm.LoadPlacedElements 와 같은 방식이다(그 함수는 화면 전용 private 이라 여기서 따로 둔다).
        /// </summary>
        private static List<MapPlacedElementData> LoadPlacedElements(DACrux.SEMDMS.RO.GdsMapView service, string factory, string device,
            IList<int> layerIds, IProgress<string> progress)
        {
            var items = new List<MapPlacedElementData>();
            foreach (int layerKey in layerIds)
            {
                string after = string.Empty;
                while (true)
                {
                    int rowCount;
                    using (DataTable page = service.GetPlacedElementPage(factory, device, GdsLayerKey.Format(layerKey), after, PageRows))
                    {
                        foreach (DataRow row in page.Rows)
                            items.Add(GdsMapPlacedRowReader.Read(row));
                        rowCount = page.Rows.Count;
                        if (rowCount > 0) after = page.Rows[rowCount - 1]["PLACED_ELEMENT_ID"].ToString();
                    }
                    Report(progress, "GDS 도면 조회 중 Layer " + GdsLayerKey.Format(layerKey) + " / 도형 " + items.Count.ToString("N0"));
                    if (rowCount < PageRows) break;
                }
            }
            return items;
        }

        #endregion

        #region [Die 표시]

        /// <summary>
        /// 선택 Chip 의 Defect 표시 / Chain 목록 / Defect 목록을 만들고 Defect 가 보이게 확대한다. 서버를 다시 부르지 않는다.
        /// 2026-10-12 요청로 상세 화면은 이 Chip 1개만 분석한다(Die 이동 / Wafer 합산 제거, 설계 14.4절).
        /// </summary>
        private void ShowCurrent()
        {
            if (_analysis == null || _loadedKey == null || !_currentDie.HasValue) return;
            if (_focusChainSeq != null && ChainRow(_focusChainSeq) == null) _focusChainSeq = null;
            List<DataRow> defects = CurrentDefects().ToList();
            // 중심 사각형 방식이면 Defect 를 한 변 = 허용오차 x 2 정사각형으로 그린다(판정과 같은 모양)
            // 크기 + 허용오차는 점선 대신 사각형을 허용오차만큼 양쪽으로 키워 채운다(분석 Defect 영역 그대로, 허용오차 0 = KLARF 크기, 2026-10-22 두 화면 같은 모양)
            double grow = _squareArea ? 0 : _tolerance;
            map.SetDefectMarkers(defects.Select(r => new DefectOverlayMarker
            {
                Id = Convert.ToInt64(r["DEFECTID"], CultureInfo.InvariantCulture),
                X = Convert.ToDouble(r["GDS_X"], CultureInfo.InvariantCulture),
                Y = Convert.ToDouble(r["GDS_Y"], CultureInfo.InvariantCulture),
                Width = _squareArea ? _tolerance * 2 : Convert.ToDouble(r["XSIZE"], CultureInfo.InvariantCulture) + grow * 2,
                Height = _squareArea ? _tolerance * 2 : Convert.ToDouble(r["YSIZE"], CultureInfo.InvariantCulture) + grow * 2,
                InChain = r["IN_CHAIN"].ToString() == "Y"
            }), 0);
            map.HighlightChainElements(null);
            FillChainDefectGrid();
            ApplyChainOverlays();
            // 아래 E-Test 값 = 다시 채운 표에서 고른 행(역추적 Chain / 이어받은 행)의 Chain, 없으면 Die 전체
            _etKey = SelectedEtKey();
            FillEtValueGrid();
            ShowEtCriteria();

            // 역추적이면 그 Chain 전체 범위, 아니면 Defect 범위로 확대한다
            IList<ChainTraceElement> focusElements = _focusChainSeq == null ? null
                : ChainElements(Convert.ToInt64(_focusChainSeq, CultureInfo.InvariantCulture));
            GBox bounds = map.GetDefectMarkerBounds();
            if (focusElements != null && focusElements.Count > 0) map.FocusChainBounds(BoundsOf(focusElements));
            else if (bounds.IsEmpty) map.ZoomToFit();
            else
            {
                // 작은 범위면 주변 배선이 보이도록 최소 400 µm 를 보장한다(FocusChainBounds 는 범위의 4배를 화면에 맞춘다).
                double cx = bounds.MinX + bounds.Width / 2, cy = bounds.MinY + bounds.Height / 2;
                double half = Math.Max(200, Math.Max(bounds.Width, bounds.Height) / 2);
                map.FocusChainBounds(new GBox(cx - half, cy - half, cx + half, cy + half));
            }
            lblDieInfo.Text = DieText(defects);
            ShowFocusHeader();
        }

        /// <summary>
        /// Die 정보 문구 = 화면 A 상태줄 둘째 줄과 같은 항목(2026-10-19): Defect 전체 / Chain In / Chain Out / 걸린 Chain / E-Test 판정(Fail Chain, 일치).
        /// </summary>
        private string DieText(List<DataRow> defects)
        {
            int inChain = defects.Count(r => r["IN_CHAIN"].ToString() == "Y");
            DataRow die = Rows("DIE").FirstOrDefault(IsCurrentDie);
            string et = Col(die, "ET_STATE");
            return String.Format("Die ({0}, {1})  Defect 전체 {2} / Chain In {3} / Chain Out {4} / 걸린 Chain {5}{6}",
                _currentDie.Value.X, _currentDie.Value.Y, defects.Count, inChain, defects.Count - inChain, HitChainCount(),
                et.Length == 0 ? "" : String.Format(" / E-Test {0} (Fail Chain {1}, 일치 {2})", et, Col(die, "ET_FAIL_CHAIN_COUNT"), Col(die, "BOTH_CHAIN_COUNT")));
        }

        /// <summary>이 Die 에서 Defect 가 걸린 Chain 수(DIE_CHAIN.DEFECT_COUNT > 0).</summary>
        private int HitChainCount()
        {
            return Rows("DIE_CHAIN").Where(IsCurrentDie).Count(r => { int n; return int.TryParse(Col(r, "DEFECT_COUNT"), out n) && n > 0; });
        }

        /// <summary>
        /// 역추적 표시(2026-10-16): 기준 Chain 이 있으면 [역추적 해제] 를 보인다(2026-10-22 표 위 체크 줄 아래로 옮기고 Chain 이름 글자는 뺌).
        /// (2026-10-22 탭이 없어져 탭 제목 끝 " (역추적)" 은 뺐다)
        /// </summary>
        private void ShowFocusHeader()
        {
            bool on = _focusChainSeq != null;
            btnClearFocus.Visible = on;
            if (!on) return;
            DataRow chain = ChainRow(_focusChainSeq);
            DataRow dc = DieChainRow(_focusChainSeq);
        }

        private static GBox BoundsOf(IList<ChainTraceElement> elements)
        {
            return new GBox(elements.Min(x => x.Bounds.MinX), elements.Min(x => x.Bounds.MinY),
                elements.Max(x => x.Bounds.MaxX), elements.Max(x => x.Bounds.MaxY));
        }

        /// <summary>선택 Chip 의 Defect.</summary>
        private IEnumerable<DataRow> CurrentDefects()
        {
            Point die = _currentDie.Value;
            return _analysis.Tables["DEFECT"].Rows.Cast<DataRow>()
                .Where(r => Convert.ToInt32(r["XINDEX"], CultureInfo.InvariantCulture) == die.X && Convert.ToInt32(r["YINDEX"], CultureInfo.InvariantCulture) == die.Y);    
        }

        /// <summary>맨 아래 묶음의 Chain 칸 문구.</summary>
        private const string OutsideGroupText = "Chain 밖 Defect";

        /// <summary>Chain 밖 묶음의 색 칸 / Chain 칸 바탕.</summary>
        private static readonly Color OutsideColor = Color.FromArgb(200, 200, 200);
        private static readonly Color OutsideBackColor = Color.FromArgb(244, 244, 244);

        /// <summary>
        /// [GDS / KLARF] 표 한 행 = (Chain, Defect) (2026-10-20). ChainSeq 가 null 이면 맨 아래 Chain 밖 Defect 묶음,
        /// Defect 가 null 이면 걸린 Defect 가 없는 Chain 의 빈 행. Group = 묶음 번호, First / Last = 묶음 첫 / 끝 행(그리기용).
        /// </summary>
        private sealed class GridRowTag
        {
            public long? ChainSeq;
            public DataRow Defect;
            public int Group;
            public bool First;
            public bool Last;
        }

        /// <summary>묶음 안 Defect 1행: Gap(그 Chain 까지, Chain 밖이면 가장 가까운 Chain 까지, 없으면 null) / 허용오차 밖(회색).</summary>
        private sealed class GridDefect
        {
            public DataRow Row;
            public double? Gap;
            public bool Dim;
        }

        /// <summary>
        /// [GDS / KLARF] 표를 채운다 (2026-10-20, 예전 Chain 표 + Defect 표).
        /// Chain 순서 = 일치 -> Fail만 -> Defect만 -> 정상 -> 비교 불가, 같은 분류는 Defect 수 많은 순(예전 Chain 표와 같음).
        /// 역추적 Chain 을 표 순서 맨 위로 옮기지 않는다(2026-10-16, 순서는 분류 순 그대로이고 그 묶음 첫 행으로 스크롤만 한다).
        /// [걸린 / Fail Chain 만] 이면 걸렸거나 E-Test Fail 인 Chain 만 행으로 둔다(둘 다 없는 Die 는 전부, 역추적 Chain 은 늘 둔다). 끄면 모든 Chain.
        /// Chain 이 많은 Device(U8 27,341개 등)도 모든 Chain 을 넣는다(2026-10-28 요청 "다 보여줘라". 10-21 의 그 Die DIE_CHAIN Chain 만 넣던 예외 삭제).
        /// 표시 체크: 역추적이면 그 Chain 만, 아니면 행이 있는 Chain 전부. [전체 표시] 는 채운 뒤 표시 체크에 맞춘다(SyncShowAll).
        /// Chain 마다 걸린 Defect 를 Gap 순(같으면 Defect ID)으로 붙이고, 없으면 빈 1행.
        /// 두 Chain 에 걸린 Defect 는 두 묶음에 모두 넣는다(Defect ID 굵은 글자. "가까운 Chain" 열은 2026-10-22 뺐다).
        /// 역추적 Chain 묶음은 예전 [KLARF] 역추적처럼 탐색 거리 안 Defect 를 거리 순으로 모두 보인다(허용오차 밖은 회색).
        /// 맨 아래에 어느 Chain 에도 안 걸린 Defect 를 가까운 거리 순(거리 없음은 뒤, 같으면 Defect ID)으로 늘 둔다(2026-10-22 [Chain 밖 Defect] 체크 삭제).
        /// 다시 채울 때 스크롤 위치와 고른 행을 이어받는다(역추적이면 그 Chain 묶음 첫 행). 다른 창 / 새 결과(_freshChainList)면 이어받지 않는다.
        /// </summary>
        private void FillChainDefectGrid()
        {
            bool fresh = _freshChainList;
            _freshChainList = false;
            DataGridView grid = dgvChainDefects;
            int firstRow = !fresh && grid.Rows.Count > 0 ? grid.FirstDisplayedScrollingRowIndex : -1;
            string keepKey = fresh || grid.CurrentRow == null ? null : RowKey(grid.CurrentRow.Tag as GridRowTag);
            var counts = new Dictionary<long, int>();
            var dieChains = new Dictionary<long, DataRow>();
            foreach (DataRow dc in Rows("DIE_CHAIN"))
                if (IsCurrentDie(dc))
                {
                    long seq = Convert.ToInt64(dc["CHAIN_SEQ"], CultureInfo.InvariantCulture);
                    counts[seq] = Convert.ToInt32(dc["DEFECT_COUNT"], CultureInfo.InvariantCulture);
                    dieChains[seq] = dc;
                }
            bool anyHit = counts.Values.Any(v => v > 0) || dieChains.Values.Any(r => Col(r, "ET_STATUS") == "F");
            List<DataRow> defects = CurrentDefects().ToList();
            Dictionary<string, double> pairGaps = PairGaps();
            DataRow die = Rows("DIE").FirstOrDefault(IsCurrentDie);

            _updating = true;
            grid.Rows.Clear();
            int group = 0, chainGroups = 0;
            foreach (DataRow c in Rows("CHAIN")
                .OrderBy(r => ClassOrder(ClassOf(r, DieChainOf(dieChains, r), die)))
                .ThenByDescending(r => CountOf(counts, r)).ThenBy(r => r["CHAIN_CODE"].ToString().Length).ThenBy(r => r["CHAIN_CODE"].ToString(), StringComparer.Ordinal))
            {
                long seq = Convert.ToInt64(c["CHAIN_SEQ"], CultureInfo.InvariantCulture);
                string seqText = seq.ToString(CultureInfo.InvariantCulture);
                int n = CountOf(counts, c);
                DataRow dc = DieChainOf(dieChains, c);
                string class4 = ClassOf(c, dc, die);
                // 행이 없는 칸: 서버는 Defect 0 + E-Test P 칸을 보내지 않는다 -> 정상(P), 연결 없음 / 판정 없음 -> 비교 불가 (2026-10-19, 화면 A ClassOf 와 같음)
                string et = dc != null ? Col(dc, "ET_STATUS") : class4 == "OK" ? "P" : "";
                bool focus = seqText == _focusChainSeq;
                if (HitOnly && anyHit && n == 0 && et != "F" && !focus) continue;
                string failPara = Col(dc, "ET_FAIL_COUNT").Length == 0 ? "" : Col(dc, "ET_FAIL_COUNT") + " / " + Col(dc, "ET_PARA_COUNT");
                string code = Col(c, "CHAIN_CODE");
                List<GridDefect> rows = focus && _analysis.Tables.Contains("DEFECT_CHAIN")
                    ? FocusRows(defects)
                    : defects.Where(d => HasCode(d, code)).Select(d => new GridDefect
                    {
                        Row = d,
                        Gap = PairGap(pairGaps, d, seqText) ?? 0
                    }).OrderBy(d => d.Gap).ThenBy(d => DefectIdOf(d.Row)).ToList();
                AddGroup(group++, seq, new object[] { _focusChainSeq == null || focus, "", c["CHAIN_NAME"], n, ClassText(class4), et, failPara, Col(c, "MEMBER_COUNT") },
                    rows, ChainColor(c), n > 0 || et == "F", et == "F", class4);
                chainGroups++;
            }
            List<DataRow> outside = defects.Where(d => d["IN_CHAIN"].ToString() != "Y").ToList();
            if (outside.Count > 0)
            {
                List<GridDefect> rows = outside.Select(d => new GridDefect
                {
                    Row = d,
                    Gap = GapValue(Col(d, "GAP"))
                }).OrderBy(d => d.Gap.HasValue ? 0 : 1).ThenBy(d => d.Gap ?? 0).ThenBy(d => DefectIdOf(d.Row)).ToList();
                AddGroup(group++, null, new object[] { false, "", OutsideGroupText, outside.Count, "", "", "", "" }, rows, OutsideColor, true, false, "");
            }
            lblChainTitle.Text = String.Format("Chain {0} / Defect 전체 {1} / Chain In {2} / Chain Out {3}",
                chainGroups, defects.Count, defects.Count - outside.Count, outside.Count);

            // 순서는 같으므로 스크롤 위치를 되돌리고, 역추적 Chain 묶음 첫 행(없으면 전에 고른 행)을 고른다(선택 처리는 돌리지 않음)
            if (firstRow >= 0 && firstRow < grid.Rows.Count) grid.FirstDisplayedScrollingRowIndex = firstRow;
            DataGridViewRow pick = _focusChainSeq != null
                ? grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r =>
                {
                    var t = r.Tag as GridRowTag;
                    return t != null && t.First && t.ChainSeq.HasValue && t.ChainSeq.Value.ToString(CultureInfo.InvariantCulture) == _focusChainSeq;
                })
                : keepKey == null ? null : grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => RowKey(r.Tag as GridRowTag) == keepKey);
            // 같은 행이 없으면(역추적 해제로 허용오차 밖 행이 빠짐 등) 같은 Chain 묶음의 첫 행
            if (pick == null && keepKey != null)
            {
                string chainPart = keepKey.Substring(0, keepKey.IndexOf('|') + 1);
                pick = grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => { var t = r.Tag as GridRowTag; return t != null && t.First && RowKey(t).StartsWith(chainPart, StringComparison.Ordinal); });
            }
            grid.ClearSelection();
            if (pick != null)
            {
                grid.CurrentCell = pick.Cells[colCode.Index];
                pick.Selected = true;
                if (_focusChainSeq != null) grid.FirstDisplayedScrollingRowIndex = pick.Index;
                else if (firstRow >= 0 && firstRow < grid.Rows.Count) grid.FirstDisplayedScrollingRowIndex = firstRow;
            }
            _updating = false;
            SyncShowAll();
            grid.Invalidate();
        }

        /// <summary>
        /// 묶음 하나를 표에 넣는다. Chain 칸 값은 모든 행에 넣고(행만 보고도 어느 Chain 인지 알게) 글자는 묶음 첫 행에만 그린다(dgvChainDefects_CellPainting).
        /// 표시 체크는 묶음 첫 행만 바꿀 수 있고 Chain 밖 묶음에는 없다.
        /// 두 Chain 이상에 걸린 Defect 는 Defect ID 를 굵게 한다. 분류 칸 글자색 = 예전 [종합] 표 색(일치 빨강 / Fail만 주황 / Defect만 진노랑, 2026-10-22).
        /// </summary>
        private void AddGroup(int group, long? chainSeq, object[] chainCells, IList<GridDefect> defects, Color color, bool bold, bool etFail, string class4)
        {
            Color? classColor = class4 == "BOTH" ? Color.Firebrick : class4 == "FAIL" ? Color.DarkOrange : class4 == "DEFECT" ? Color.FromArgb(150, 110, 0) : (Color?)null;
            DataGridView grid = dgvChainDefects;
            int count = Math.Max(1, defects.Count);
            for (int k = 0; k < count; k++)
            {
                GridDefect d = k < defects.Count ? defects[k] : null;
                var cells = new object[grid.Columns.Count];
                Array.Copy(chainCells, cells, chainCells.Length);
                if (d != null)
                {
                    DataRow r = d.Row;
                    cells[colDefectId.Index] = DefectIdOf(r);
                    cells[colGap.Index] = d.Gap.HasValue ? d.Gap.Value.ToString("0.0", CultureInfo.InvariantCulture) : "";
                    cells[colRoughBin.Index] = Col(r, "ROUGHBINNUMBER");
                    cells[colClassNo.Index] = Col(r, "CLASSNUMBER");
                    cells[colSize.Index] = String.Format(CultureInfo.InvariantCulture, "{0:0.##} x {1:0.##}", r["XSIZE"], r["YSIZE"]);
                    cells[colGdsX.Index] = r["GDS_X"];
                    cells[colGdsY.Index] = r["GDS_Y"];
                    cells[colXRel.Index] = r["XREL"];
                    cells[colYRel.Index] = r["YREL"];
                }
                int i = grid.Rows.Add(cells);
                DataGridViewRow row = grid.Rows[i];
                row.Tag = new GridRowTag { ChainSeq = chainSeq, Defect = d == null ? null : d.Row, Group = group, First = k == 0, Last = k == count - 1 };
                if (k > 0 || !chainSeq.HasValue) row.Cells[colShow.Index].ReadOnly = true;
                for (int col = 0; col <= colMember.Index; col++)
                {
                    DataGridViewCellStyle style = row.Cells[col].Style;
                    if (col == colColor.Index) style.BackColor = style.SelectionBackColor = color;
                    else if (!chainSeq.HasValue) style.BackColor = OutsideBackColor;
                    if (bold) style.Font = GridBoldFont;
                }
                if (etFail) row.Cells[colEt.Index].Style.ForeColor = Color.Firebrick;
                if (classColor.HasValue) row.Cells[colClass.Index].Style.ForeColor = classColor.Value;
                if (!chainSeq.HasValue) row.Cells[colCode.Index].Style.ForeColor = Color.DimGray;
                if (d != null && d.Dim)
                    for (int col = colDefectId.Index; col < grid.Columns.Count; col++) row.Cells[col].Style.ForeColor = Color.Gray;
                if (d != null && IsMultiChain(d.Row)) row.Cells[colDefectId.Index].Style.Font = GridBoldFont;
            }
        }

        /// <summary>두 Chain 이상에 걸린 Defect 인지(DEFECT.CHAIN_CODES 에 코드가 2개 이상).</summary>
        private static bool IsMultiChain(DataRow defect)
        {
            return defect["IN_CHAIN"].ToString() == "Y" && Col(defect, "CHAIN_CODES").Split(',').Count(s => s.Trim().Length > 0) > 1;
        }

        /// <summary>Chain 칸 굵은 글자(걸린 / Fail Chain, Chain 밖 묶음). 한 번 만들어 같이 쓴다.</summary>
        private Font GridBoldFont
        {
            get { return _gridBoldFont ?? (_gridBoldFont = new Font(dgvChainDefects.Font, FontStyle.Bold)); }
        }
        private Font _gridBoldFont;

        /// <summary>
        /// 역추적 Chain 묶음: 그 Chain 까지 거리 순, 탐색 거리 안 Defect 모두. 허용오차 밖은 회색 (예전 [KLARF] 역추적 목록과 같음).
        /// </summary>
        private List<GridDefect> FocusRows(List<DataRow> defects)
        {
            Dictionary<string, double> gaps = FocusGaps();
            return defects.Where(d => gaps.ContainsKey(DefectKey(d))).Select(d =>
            {
                double gap = gaps[DefectKey(d)];
                return new GridDefect { Row = d, Gap = gap, Dim = gap > _tolerance };
            }).OrderBy(d => d.Gap).ThenBy(d => DefectIdOf(d.Row)).ToList();
        }

        /// <summary>DEFECT.CHAIN_CODES("A,B")에 그 Chain 코드가 있는지(걸린 Defect).</summary>
        private static bool HasCode(DataRow defect, string code)
        {
            return defect["IN_CHAIN"].ToString() == "Y" && ("," + Col(defect, "CHAIN_CODES").Replace(" ", "") + ",").Contains("," + code + ",");
        }

        /// <summary>(Defect, Chain) 거리 = DEFECT_CHAIN.GAP. 키 = Defect 키|CHAIN_SEQ. 같은 쌍이 여러 행이면 작은 값.</summary>
        private Dictionary<string, double> PairGaps()
        {
            var gaps = new Dictionary<string, double>();
            foreach (DataRow r in Rows("DEFECT_CHAIN"))
            {
                double? gap = GapValue(Col(r, "GAP"));
                if (!gap.HasValue) continue;
                string key = DefectKey(r) + "|" + Col(r, "CHAIN_SEQ");
                if (!gaps.ContainsKey(key) || gaps[key] > gap.Value) gaps[key] = gap.Value;
            }
            return gaps;
        }

        private static double? PairGap(Dictionary<string, double> gaps, DataRow defect, string chainSeq)
        {
            double gap;
            return gaps.TryGetValue(DefectKey(defect) + "|" + chainSeq, out gap) ? gap : (double?)null;
        }

        private static double? GapValue(string gap)
        {
            double v;
            return double.TryParse(gap, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : (double?)null;
        }

        private static long DefectIdOf(DataRow r)
        {
            return Convert.ToInt64(r["DEFECTID"], CultureInfo.InvariantCulture);
        }

        /// <summary>행 키 = CHAIN_SEQ(Chain 밖 = OUT)|Defect 키. 다시 채운 뒤 / 창 보기 상태에서 같은 행을 찾는다.</summary>
        private static string RowKey(GridRowTag t)
        {
            if (t == null) return null;
            return (t.ChainSeq.HasValue ? t.ChainSeq.Value.ToString(CultureInfo.InvariantCulture) : "OUT") + "|" + (t.Defect == null ? "" : DefectKey(t.Defect));
        }

        private GridRowTag TagAt(int rowIndex)
        {
            return rowIndex < 0 || rowIndex >= dgvChainDefects.Rows.Count ? null : dgvChainDefects.Rows[rowIndex].Tag as GridRowTag;
        }

        /// <summary>표시 체크를 볼 수 있는 행(Chain 묶음 첫 행).</summary>
        private IEnumerable<DataGridViewRow> ChainHeadRows()
        {
            return dgvChainDefects.Rows.Cast<DataGridViewRow>().Where(r => { var t = r.Tag as GridRowTag; return t != null && t.First && t.ChainSeq.HasValue; });
        }

        private static DataRow DieChainOf(Dictionary<long, DataRow> dieChains, DataRow chain)
        {
            DataRow dc;
            return dieChains.TryGetValue(Convert.ToInt64(chain["CHAIN_SEQ"], CultureInfo.InvariantCulture), out dc) ? dc : null;
        }

        /// <summary>4분류 정렬 순서(일치 -> Fail만 -> Defect만 -> 정상 -> 비교 불가).</summary>
        private static int ClassOrder(string class4)
        {
            switch (class4)
            {
                case "BOTH": return 0;
                case "FAIL": return 1;
                case "DEFECT": return 2;
                case "OK": return 3;
                default: return 4;
            }
        }

        /// <summary>
        /// (Die, Chain) 4분류 코드. DIE_CHAIN.CLASS4 가 있으면 그 값, 행이 없으면 E-Test 판정 Die(P / F) + 연결 Chain 이면 OK, 아니면 NA
        /// (화면 A frmGdsKlarfAnalysis.ClassOf 와 같은 규칙, 2026-10-19).
        /// </summary>
        private static string ClassOf(DataRow chain, DataRow dieChain, DataRow die)
        {
            string class4 = Col(dieChain, "CLASS4");
            if (class4.Length > 0) return class4;
            string et = Col(die, "ET_STATE");
            return (et == "P" || et == "F") && Col(chain, "ET_LINK_STATUS") == "LINKED" ? "OK" : "NA";
        }

        /// <summary>(Die, Chain) 4분류 문구(통합 분석 설계 7.1절, 화면 A GdsKlarfColorBands.ClassText 와 같음).</summary>
        private static string ClassText(string class4)
        {
            switch (class4)
            {
                case "BOTH": return "일치";
                case "FAIL": return "Fail만";
                case "DEFECT": return "Defect만";
                case "OK": return "정상";
                default: return "비교 불가";
            }
        }

        private static int CountOf(Dictionary<long, int> counts, DataRow chain)
        {
            int n;
            return counts.TryGetValue(Convert.ToInt64(chain["CHAIN_SEQ"], CultureInfo.InvariantCulture), out n) ? n : 0;
        }

        /// <summary>저장된 Chain 색. 값이 없으면(0) 주황.</summary>
        private static Color ChainColor(DataRow chain)
        {
            int argb = Convert.ToInt32(chain["COLOR_ARGB"], CultureInfo.InvariantCulture);
            return argb == 0 ? Color.Orange : Color.FromArgb(255, Color.FromArgb(argb));
        }

        /// <summary>
        /// 체크한 Chain 만 그린다(GDS Map 조회 화면과 같은 Chain Overlay). 색은 Chain 마다 저장 색(목록 색 칸과 같음).
        /// E-Test Fail Chain 도 Chain 색으로 그리고 Fail 여부는 목록 ET / 분류 열로 본다 (2026-10-16 변경: 빨간색으로 덮으면 어느 Chain 인지 구분되지 않음).
        /// </summary>
        private void ApplyChainOverlays()
        {
            var overlays = new List<ChainVisibleOverlay>();
            var colors = new Dictionary<long, Color>();
            // 표시 체크는 Chain 묶음 첫 행에 있다 (2026-10-20 [GDS / KLARF] 합친 표)
            foreach (DataGridViewRow row in ChainHeadRows())
            {
                if (!(row.Cells[colShow.Index].Value is bool) || !(bool)row.Cells[colShow.Index].Value) continue;
                long seq = ((GridRowTag)row.Tag).ChainSeq.Value;
                IList<ChainTraceElement> elements = ChainElements(seq);
                Color color = row.Cells[colColor.Index].Style.BackColor;
                if (elements.Count == 0) continue;
                overlays.Add(new ChainVisibleOverlay(row.Cells[colCode.Index].Value.ToString(), color, elements));
                colors[seq] = color;
            }
            map.SetVisibleChainOverlays(overlays);
            VisibleChainCount = overlays.Count;
            VisibleChainColors = colors;
        }

        /// <summary>지도에 그린 Chain 번호 -> 그린 색(Smoke 확인용).</summary>
        public IDictionary<long, Color> VisibleChainColors { get; private set; }

        /// <summary>표시 중인 Chain 중 E-Test Fail Chain 수(Smoke 확인용). 색은 다른 Chain 과 같이 저장 색이다.</summary>
        public int FailChainOverlayCount
        {
            get
            {
                return ChainHeadRows().Count(r => r.Cells[colShow.Index].Value is bool && (bool)r.Cells[colShow.Index].Value
                    && Convert.ToString(r.Cells[colEt.Index].Value) == "F");
            }
        }

        /// <summary>Chain Member ID -> 지금 지도의 도형. 한 번 만든 것은 도면을 다시 읽을 때까지 재사용한다.</summary>
        private IList<ChainTraceElement> ChainElements(long chainSeq)
        {
            IList<ChainTraceElement> elements;
            if (_chainElements.TryGetValue(chainSeq, out elements)) return elements;
            IList<string> ids;
            elements = _chainMembers.TryGetValue(chainSeq, out ids) ? map.GetChainTraceElements(ids) : new List<ChainTraceElement>();
            _chainElements[chainSeq] = elements;
            return elements;
        }

        /// <summary>
        /// 역추적 Chain 까지의 Defect 별 거리. 서버 결과 DEFECT_CHAIN(STEP_SEQ, DEFECTID, CHAIN_SEQ, GAP)을 쓰고,
        /// 그 표가 없는 결과(이전 서버)는 Chain 코드가 걸린 Defect 를 거리 0 으로 본다.
        /// </summary>
        private Dictionary<string, double> FocusGaps()
        {
            var gaps = new Dictionary<string, double>();
            if (_analysis.Tables.Contains("DEFECT_CHAIN"))
            {
                foreach (DataRow r in Rows("DEFECT_CHAIN"))
                {
                    double gap;
                    if (r["CHAIN_SEQ"].ToString() != _focusChainSeq) continue;
                    if (!double.TryParse(Col(r, "GAP"), NumberStyles.Float, CultureInfo.InvariantCulture, out gap)) gap = 0;
                    string key = DefectKey(r);
                    if (!gaps.ContainsKey(key) || gaps[key] > gap) gaps[key] = gap;
                }
                return gaps;
            }
            string code = Col(ChainRow(_focusChainSeq), "CHAIN_CODE");
            foreach (DataRow r in CurrentDefects())
                if (r["IN_CHAIN"].ToString() == "Y" && ("," + Col(r, "CHAIN_CODES").Replace(" ", "") + ",").Contains("," + code + ","))
                    gaps[DefectKey(r)] = 0;
            return gaps;
        }

        /// <summary>Defect 키 = STEP_SEQ|DEFECTID (여러 KLARF Step 을 합친 결과에서 DEFECTID 가 겹칠 수 있다).</summary>
        private static string DefectKey(DataRow r)
        {
            return Col(r, "STEP_SEQ") + "|" + r["DEFECTID"];
        }

        /// <summary>
        /// 아래 E-Test 값 표(2026-10-16 [E-Test] 탭, 2026-10-22 한 패널 아래 표): 이 Die 의 GDS Chain 연결 Parameter 값 / Spec / 판정.
        /// 범위 = _etKey: null = Die 전체, OutsideEtKey = Chain 밖 Defect(E-Test 없음, 빈 표), 그 밖 = 그 Chain 것만.
        /// 결과 ET_VALUE 표(Die 상세 서버 결과)가 없으면 빈 표다. Fail 이 위로 오고 빨간 글자.
        /// </summary>
        private void FillEtValueGrid()
        {
            // 판정 / 값을 앞에 두고 긴 헤더 원문은 맨 뒤(오른쪽 칸이 좁아도 판정이 보이게)
            var table = new DataTable();
            table.Columns.Add("판정", typeof(string));
            table.Columns.Add("Parameter", typeof(string));
            table.Columns.Add("값", typeof(string));
            table.Columns.Add("Lower", typeof(string));
            table.Columns.Add("Upper", typeof(string));
            table.Columns.Add("GDS Chain", typeof(string));
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("헤더 원문", typeof(string));
            table.Columns.Add("SEQ", typeof(string));
            if (_etKey != OutsideEtKey)
                foreach (DataRow r in Rows("ET_VALUE").Where(IsCurrentDie)
                    .Where(r => _etKey == null || Col(r, "CHAIN_SEQ") == _etKey)
                    .OrderBy(r => Col(r, "JUDGE_FLAG") == "F" ? 0 : 1).ThenBy(r => Col(r, "GDS_CHAIN_NAME")).ThenBy(r => Col(r, "ITEM_NAME")))
                    table.Rows.Add(Col(r, "JUDGE_FLAG"), Col(r, "ITEM_NAME"), Col(r, "MEAS_VALUE"), Col(r, "LOWER_LIMIT"), Col(r, "UPPER_LIMIT"),
                        Col(r, "GDS_CHAIN_NAME"), Col(r, "CHAIN_TEXT"), Col(r, "HEADER_TEXT"), Col(r, "CHAIN_SEQ"));
            _updating = true;
            dgvEtValues.DataSource = table;
            if (dgvEtValues.Columns.Contains("SEQ")) dgvEtValues.Columns["SEQ"].Visible = false;
            foreach (DataGridViewRow row in dgvEtValues.Rows)
                if (Convert.ToString(row.Cells["판정"].Value) == "F") row.DefaultCellStyle.ForeColor = Color.Firebrick;
            dgvEtValues.ClearSelection();
            _updating = false;
            int fails = table.Rows.Cast<DataRow>().Count(r => Convert.ToString(r["판정"]) == "F");
            lblEtCount.Text = _etKey == OutsideEtKey ? "E-Test 값 · " + OutsideGroupText
                : String.Format("E-Test 값 · {0} ({1}){2}", _etKey == null ? "Die 전체" : "Chain " + Col(ChainRow(_etKey), "CHAIN_NAME"),
                    table.Rows.Count, fails > 0 ? "  Fail " + fails : "");
        }

        /// <summary>위 표 행을 고를 때: 그 행 Chain 이 지금 아래 표 범위와 다르면 아래 표를 다시 채운다(같은 Chain 안에서 행만 바꾸면 그대로).</summary>
        private void ShowEtValuesFor(GridRowTag tag)
        {
            string key = EtKeyOf(tag);
            if (key == _etKey) return;
            _etKey = key;
            FillEtValueGrid();
        }

        /// <summary>위 표에서 지금 고른 행의 아래 표 범위(고른 행 없음 = Die 전체).</summary>
        private string SelectedEtKey()
        {
            return dgvChainDefects.SelectedRows.Count == 0 ? null : EtKeyOf(dgvChainDefects.SelectedRows[0].Tag as GridRowTag);
        }

        /// <summary>행 -> 아래 표 범위: Chain 묶음 = CHAIN_SEQ, Chain 밖 Defect 묶음 = OutsideEtKey.</summary>
        private static string EtKeyOf(GridRowTag tag)
        {
            if (tag == null) return null;
            return tag.ChainSeq.HasValue ? tag.ChainSeq.Value.ToString(CultureInfo.InvariantCulture) : OutsideEtKey;
        }

        /// <summary>
        /// 기준 묶음 E-Test 줄 = 분석 결과 파일 문구 + 이 Die 판정(2026-10-19 [E-Test] 탭 기준, 2026-10-22 한 줄로 옮김). 이름은 화면 A Map 빨간 삼각형 기준과 같다:
        /// Spec 판정(DMS Spec, ET_STATE) F = "Spec Out", 결과 파일 DUT Flag(ET_FILE_FLAG) F = "Fail Die". 줄은 2026-10-22 부터 늘 숨기고 글자만 채운다.
        /// </summary>
        private void ShowEtCriteria()
        {
            if (_etCriteria.Length == 0)
            {
                lblEtText.Text = "";
                return;
            }
            DataRow die = Rows("DIE").FirstOrDefault(IsCurrentDie);
            string spec = Col(die, "ET_STATE");
            string file = Col(die, "ET_FILE_FLAG");
            string judge = spec.Length == 0 && file.Length == 0 ? " · 이 Die E-Test 없음"
                : String.Format(" · 이 Die Spec 판정 {0}{1} · 결과 파일 {2}{3}", spec.Length == 0 ? "-" : spec, spec == "F" ? " Spec Out" : "",
                    file.Length == 0 ? "-" : file, file == "F" ? " Fail Die" : "");
            lblEtText.Text = _etCriteria + judge;
        }

        #endregion

        #region [이벤트]

        private async void chkBackground_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _busy) return;
            int show = ++_showVersion;
            try
            {
                await LoadMapAsync();
                if (show == _showVersion && MapKey() == _loadedKey) ShowCurrent();
            }
            catch (Exception ex)
            {
                ShowError("GDS 도면 조회", ex);
            }
        }

        private void chkOriginCross_CheckedChanged(object sender, EventArgs e)
        {
            map.ShowOriginCross = chkOriginCross.Checked;
        }

        private void chkHitOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _loadedKey == null) return;
            FillChainDefectGrid();
            ApplyChainOverlays();
        }

        /// <summary>
        /// [다시 분석] (2026-10-22): 호스트(DMS 상세보기 창)에 알린다. 호스트는 이 화면을 연 분석 화면의 [다시 분석] / [분석] 을 부르고,
        /// 그 화면이 결과를 반영하면 동기화로 이 화면에 새 결과를 넘긴다. 호스트 없이 연 화면이면 상태줄 안내만.
        /// </summary>
        private void btnReanalyze_Click(object sender, EventArgs e)
        {
            if (ReanalyzeRequested == null)
            {
                SetStatus("다시 분석은 DMS 분석 화면에서 연 상세보기에서만 할 수 있습니다.");
                return;
            }
            try
            {
                ReanalyzeRequested();
            }
            catch (Exception ex)
            {
                ShowError("다시 분석", ex);
            }
        }

        /// <summary>표시 체크(Chain 묶음 첫 행)를 바꾸면 같은 묶음의 다른 행에도 같은 값을 넣고 바로 지도에 반영한다.</summary>
        private void dgvChainDefects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_updating || e.RowIndex < 0 || e.ColumnIndex != colShow.Index) return;
            GridRowTag tag = TagAt(e.RowIndex);
            if (tag == null || !tag.First || !tag.ChainSeq.HasValue) return;
            dgvChainDefects.CommitEdit(DataGridViewDataErrorContexts.Commit);
            object on = dgvChainDefects.Rows[e.RowIndex].Cells[colShow.Index].Value;
            _updating = true;
            foreach (DataGridViewRow row in dgvChainDefects.Rows)
            {
                var t = row.Tag as GridRowTag;
                if (t != null && t.Group == tag.Group && row.Index != e.RowIndex) row.Cells[colShow.Index].Value = on;
            }
            _updating = false;
            ApplyChainOverlays();
            SyncShowAll();
        }

        /// <summary>
        /// [전체 표시] (2026-10-23 요청 "전체 체크 / 미체크 기능"): 모든 Chain 의 표시 체크(묶음 전체 행)를 한 번에 켜거나 끄고 바로 도면에 반영한다.
        /// 역추적 중이어도 그대로 따른다. Chain 밖 Defect 묶음은 표시 체크가 없어 건드리지 않는다.
        /// </summary>
        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _loadedKey == null) return;
            dgvChainDefects.EndEdit();
            bool on = chkShowAll.Checked;
            _updating = true;
            foreach (DataGridViewRow row in dgvChainDefects.Rows)
            {
                var t = row.Tag as GridRowTag;
                if (t != null && t.ChainSeq.HasValue) row.Cells[colShow.Index].Value = on;
            }
            _updating = false;
            ApplyChainOverlays();
        }

        /// <summary>[전체 표시] 를 지금 표시 체크에 맞춘다(Chain 이 있고 모두 켜져 있으면 켬). 이벤트는 돌리지 않는다.</summary>
        private void SyncShowAll()
        {
            List<DataGridViewRow> heads = ChainHeadRows().ToList();
            bool was = _updating;
            _updating = true;
            chkShowAll.Checked = heads.Count > 0 && heads.All(r => r.Cells[colShow.Index].Value is bool && (bool)r.Cells[colShow.Index].Value);
            _updating = was;
        }

        /// <summary>
        /// 오른쪽 클릭 (2026-10-22): Defect 가 있는 행이면 그 행을 고르고(도면이 그 Defect 를 따라감) [Defect 이미지 보기] 메뉴를 연다.
        /// 표 더블클릭은 아무 일도 하지 않는다(Chain 칸 더블클릭 역추적 / Defect 칸 더블클릭 이미지 보기를 뺐다).
        /// 역추적은 분석 화면에서 Chain 을 지정해 열 때만 하고 [역추적 해제] 로 푼다.
        /// </summary>
        private void dgvChainDefects_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            GridRowTag tag = TagAt(e.RowIndex);
            if (tag == null || tag.Defect == null) return;
            DataGridViewRow row = dgvChainDefects.Rows[e.RowIndex];
            if (!row.Selected)
            {
                dgvChainDefects.ClearSelection();
                dgvChainDefects.CurrentCell = row.Cells[colCode.Index];
                row.Selected = true;
            }
            Rectangle cell = dgvChainDefects.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            mnuDefect.Show(dgvChainDefects, cell.Left + e.X, cell.Top + e.Y);
        }

        /// <summary>[Defect 이미지 보기]: 고른 행의 Defect 이미지를 DMS 호스트에 요청한다.</summary>
        private void mnuDefectImage_Click(object sender, EventArgs e)
        {
            GridRowTag tag = dgvChainDefects.SelectedRows.Count == 0 ? null : dgvChainDefects.SelectedRows[0].Tag as GridRowTag;
            if (tag != null && tag.Defect != null) RequestDefectImage(DefectIdOf(tag.Defect));
        }

        /// <summary>[역추적 해제]: Die 전체 보기로 돌아간다.</summary>
        private void btnClearFocus_Click(object sender, EventArgs e)
        {
            _focusChainSeq = null;
            ShowCurrent();
        }

        /// <summary>
        /// E-Test 값 행 선택: 상태줄에 Parameter / GDS Chain / 값 / 판정을 보인다.
        /// 그 GDS Chain 도형을 선택 색(노랑)으로 칠하던 것은 뺐다(2026-10-22 요청, 위 표 행 선택과 같이 Chain 색 그대로).
        /// </summary>
        private void dgvEtValues_SelectionChanged(object sender, EventArgs e)
        {
            if (_updating || dgvEtValues.SelectedRows.Count == 0 || !dgvEtValues.Columns.Contains("SEQ")) return;
            SetStatus(String.Format("{0} / {1} / 값 {2} ({3})", dgvEtValues.SelectedRows[0].Cells["Parameter"].Value,
                dgvEtValues.SelectedRows[0].Cells["GDS Chain"].Value, dgvEtValues.SelectedRows[0].Cells["값"].Value, dgvEtValues.SelectedRows[0].Cells["판정"].Value));
        }

        /// <summary>
        /// 행 선택 (2026-10-20 합친 표): Defect 가 있는 행 = 지도에서 그 Defect 강조(허용오차 점선) + 주변으로 이동.
        /// Chain 도형을 선택 색(노랑)으로 칠하지 않는다(2026-10-20 변경: Defect 만 따라간다). Chain 색은 표시 체크한 Chain 색 그대로 보인다.
        /// Defect 가 없는 Chain 행은 상태줄에 Chain 이름만 보인다.
        /// 아래 E-Test 값 표는 그 행 Chain 의 Parameter 로 바꾼다(Chain 밖 행 = 빈 표, 2026-10-22).
        /// </summary>
        private void dgvChainDefects_SelectionChanged(object sender, EventArgs e)
        {
            if (_updating || dgvChainDefects.SelectedRows.Count == 0) return;
            var tag = dgvChainDefects.SelectedRows[0].Tag as GridRowTag;
            if (tag == null) return;
            if (tag.Defect != null) SelectDefect(tag.Defect);
            else if (tag.ChainSeq.HasValue) ShowChainStatus(tag.ChainSeq.Value);
            ShowEtValuesFor(tag);
        }

        private void SelectDefect(DataRow r)
        {
            long id = DefectIdOf(r);
            string step = Col(r, "STEP_ID");
            map.SelectDefectMarker(id);
            map.HighlightChainElements(null);
            int hitMembers = Rows("DEFECT_HIT")
                .Count(h => DefectIdOf(h) == id && (step.Length == 0 || Col(h, "STEP_ID").Length == 0 || Col(h, "STEP_ID") == step));
            double x = Convert.ToDouble(r["GDS_X"], CultureInfo.InvariantCulture);
            double y = Convert.ToDouble(r["GDS_Y"], CultureInfo.InvariantCulture);
            map.FocusChainBounds(new GBox(x - 100, y - 100, x + 100, y + 100));
            SetStatus(String.Format("Defect #{0} / GDS ({1:0.###}, {2:0.###}) / 걸린 Member {3}", id, x, y, hitMembers));
        }

        private void ShowChainStatus(long chainSeq)
        {
            map.HighlightChainElements(null);
            IList<string> members;
            bool found = _chainMembers.TryGetValue(chainSeq, out members);
            SetStatus(String.Format("Chain {0} / Member {1}", Col(ChainRow(chainSeq.ToString(CultureInfo.InvariantCulture)), "CHAIN_NAME"), found ? members.Count : 0));
        }

        /// <summary>Defect 이미지 보기를 DMS 호스트에 요청한다.</summary>
        private void RequestDefectImage(long id)
        {
            if (DefectImageRequested == null)
            {
                SetStatus("이미지는 DMS 안에서 열었을 때만 볼 수 있습니다.");
                return;
            }
            try
            {
                DefectImageRequested(_stepSeq, id);
            }
            catch (Exception ex)
            {
                ShowError("Defect 이미지", ex);
            }
        }

        /// <summary>
        /// Chain 칸을 한 칸처럼 그린다 (2026-10-20): 묶음 안 가로선을 없애고 글자는 묶음 첫 행(가려졌으면 화면 첫 행)에만.
        /// 색 칸은 모든 행에 칠해 묶음이 이어져 보인다. Chain 밖 묶음은 표시 체크를 그리지 않는다. 묶음 끝에는 진한 가로선.
        /// </summary>
        private void dgvChainDefects_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GridRowTag tag = e.ColumnIndex < 0 ? null : TagAt(e.RowIndex);
            if (tag == null) return;
            bool chainCell = e.ColumnIndex <= colMember.Index;
            if (chainCell && !tag.Last) e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            bool firstShown = tag.First || e.RowIndex == dgvChainDefects.FirstDisplayedScrollingRowIndex;
            bool blank = chainCell && (!firstShown || (e.ColumnIndex == colShow.Index && (!tag.ChainSeq.HasValue || !tag.First)));
            if (blank) e.PaintBackground(e.ClipBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            else e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            if (tag.Last)
                using (var pen = new Pen(Color.FromArgb(150, 150, 150)))
                    e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            e.Handled = true;
        }

        /// <summary>
        /// 지도에서 Defect 를 클릭하면 [GDS / KLARF] 표에서 그 Defect 의 첫 행을 고른다 (2026-10-20). Chain 밖 Defect 묶음은 늘 있다(2026-10-22).
        /// </summary>
        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            long? id = map.FindDefectMarkerAt(e.Location);
            if (!id.HasValue) return;
            DataGridViewRow row = FindDefectRow(id.Value);
            if (row == null) return;
            dgvChainDefects.ClearSelection();
            row.Selected = true;
            dgvChainDefects.FirstDisplayedScrollingRowIndex = row.Index;
        }

        private DataGridViewRow FindDefectRow(long id)
        {
            return dgvChainDefects.Rows.Cast<DataGridViewRow>().FirstOrDefault(r =>
            {
                var t = r.Tag as GridRowTag;
                return t != null && t.Defect != null && DefectIdOf(t.Defect) == id;
            });
        }

        private void map_MouseWorldPositionChanged(object sender, PointF position)
        {
            lblCoordinate.Text = String.Format("X: {0:0.###} / Y: {1:0.###}", position.X, position.Y);
        }

        #endregion

        #region [창마다 보기 상태]

        /// <summary>
        /// 지금 보기 상태(2026-10-16, 설계 5.2절): 역추적 Chain / 걸린 Chain 만 / Chain 표시 체크 / 고른 행 /
        /// 표 스크롤 / 상태줄 / 도면 확대 위치. 호스트가 창을 바꿀 때 저장했다가 ShowDie 로 돌려준다(내용은 보지 않음).
        /// 2026-10-22: 오른쪽 탭이 없어져 탭 번호는 뺐다. 아래 E-Test 값 표는 되돌린 고른 행 Chain 으로 다시 채운다.
        /// 도면이 없거나 읽는 중이면 null(그 창은 처음 여는 것처럼 표시). [전체 도면] 은 창마다가 아니라 화면 하나의 설정이다.
        /// 2026-10-20: Chain 표 + Defect 표가 [GDS / KLARF] 표 하나가 되어 고른 행 / 스크롤도 하나다.
        /// </summary>
        public object CaptureViewState()
        {
            if (_analysis == null || _loadedKey == null || _busy || !_currentDie.HasValue) return null;
            var s = new ViewState
            {
                FocusChainSeq = _focusChainSeq,
                HitOnly = chkHitOnly.Checked,
                RowKey = dgvChainDefects.SelectedRows.Count > 0 ? RowKey(dgvChainDefects.SelectedRows[0].Tag as GridRowTag)
                    : dgvChainDefects.CurrentRow == null ? null : RowKey(dgvChainDefects.CurrentRow.Tag as GridRowTag),
                RowSelected = dgvChainDefects.SelectedRows.Count > 0,
                FirstRow = dgvChainDefects.Rows.Count > 0 ? dgvChainDefects.FirstDisplayedScrollingRowIndex : -1,
                Status = lblStatus.Text
            };
            foreach (DataGridViewRow row in ChainHeadRows())
                s.ChainShow[((GridRowTag)row.Tag).ChainSeq.Value] = row.Cells[colShow.Index].Value is bool && (bool)row.Cells[colShow.Index].Value;
            map.GetView(out s.CenterX, out s.CenterY, out s.Scale, out s.LabelScale);
            return s;
        }

        /// <summary>표를 채우기 전에 되돌릴 값(걸린 Chain 만). 이벤트는 돌리지 않는다.</summary>
        private void RestoreListState(ViewState s)
        {
            _updating = true;
            chkHitOnly.Checked = s.HitOnly;
            _updating = false;
        }

        /// <summary>표를 채운 뒤 되돌릴 값: Chain 표시 체크(묶음 전체), 고른 행(강조 + 아래 E-Test 값), 스크롤, 마지막에 도면 확대 위치.</summary>
        private void RestoreSelectionAndView(ViewState s)
        {
            _updating = true;
            foreach (DataGridViewRow row in dgvChainDefects.Rows)
            {
                var t = row.Tag as GridRowTag;
                bool on;
                if (t != null && t.ChainSeq.HasValue && s.ChainShow.TryGetValue(t.ChainSeq.Value, out on)) row.Cells[colShow.Index].Value = on;
            }
            _updating = false;
            ApplyChainOverlays();
            SyncShowAll();

            DataGridViewRow pick = s.RowKey == null ? null : dgvChainDefects.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => RowKey(r.Tag as GridRowTag) == s.RowKey);
            if (pick != null && s.RowSelected)
            {
                // 행 선택 처리(강조 / 걸린 Member 색 / 상태줄)는 그대로 돌리고, 확대 위치는 아래에서 저장 값으로 되돌린다
                dgvChainDefects.ClearSelection();
                dgvChainDefects.CurrentCell = pick.Cells[colCode.Index];
                pick.Selected = true;
            }
            else
            {
                _updating = true;
                if (pick != null) dgvChainDefects.CurrentCell = pick.Cells[colCode.Index];
                dgvChainDefects.ClearSelection();
                _updating = false;
                // 고른 행이 없던 창 = 아래 E-Test 값은 Die 전체
                ShowEtValuesFor(null);
            }
            if (s.FirstRow >= 0 && s.FirstRow < dgvChainDefects.Rows.Count) dgvChainDefects.FirstDisplayedScrollingRowIndex = s.FirstRow;
            map.SetView(s.CenterX, s.CenterY, s.Scale, s.LabelScale);
            // 상태줄도 그 창에서 마지막으로 보던 문구로(다른 창 문구가 남지 않게)
            if (s.Status != null) SetStatus(s.Status);
        }

        /// <summary>CaptureViewState 결과. 호스트는 object 로만 들고 있다.</summary>
        private sealed class ViewState
        {
            public string FocusChainSeq;
            public bool HitOnly;
            public readonly Dictionary<long, bool> ChainShow = new Dictionary<long, bool>();
            public string RowKey;
            public bool RowSelected;
            public int FirstRow;
            public string Status;
            public double CenterX, CenterY, Scale, LabelScale;
        }

        #endregion

        #region [Helper]

        /// <summary>결과 표 행. 표가 없으면(이전 서버 결과) 빈 목록.</summary>
        private IEnumerable<DataRow> Rows(string table)
        {
            return _analysis != null && _analysis.Tables.Contains(table) ? _analysis.Tables[table].Rows.Cast<DataRow>() : Enumerable.Empty<DataRow>();
        }

        /// <summary>열 값(문자열). 행이나 열이 없으면 빈 값.</summary>
        private static string Col(DataRow row, string column)
        {
            return row != null && row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? row[column].ToString().Trim() : string.Empty;
        }

        private bool IsCurrentDie(DataRow row)
        {
            return _currentDie.HasValue && Col(row, "XINDEX") == _currentDie.Value.X.ToString(CultureInfo.InvariantCulture)
                && Col(row, "YINDEX") == _currentDie.Value.Y.ToString(CultureInfo.InvariantCulture);
        }

        private DataRow ChainRow(string chainSeq)
        {
            return Rows("CHAIN").FirstOrDefault(r => r["CHAIN_SEQ"].ToString() == chainSeq);
        }

        private DataRow DieChainRow(string chainSeq)
        {
            return Rows("DIE_CHAIN").FirstOrDefault(r => r["CHAIN_SEQ"].ToString() == chainSeq && IsCurrentDie(r));
        }

        /// <summary>작업 중 표시 (2026-10-15): 도면 / Defect 읽기 중 상태 칸 "작업 중..." + 모래시계 커서(지도 위 포함). 처리 시간은 보이지 않는다.</summary>
        private BusyStatus BusyIndicator
        {
            get { return _busyIndicator ?? (_busyIndicator = new BusyStatus(this, lblElapsed)); }
        }
        private BusyStatus _busyIndicator;

        private void SetBusy(bool busy)
        {
            _busy = busy;
            if (busy) BusyIndicator.Start();
            else BusyIndicator.Stop();
            progressLoad.Visible = busy;
            progressLoad.Style = ProgressBarStyle.Marquee;
            chkBackground.Enabled = !busy;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void ShowError(string title, Exception ex)
        {
            Exception cause = ex;
            while (cause.InnerException != null) cause = cause.InnerException;
            SetStatus(title + " 실패: " + cause.Message);
            System.Diagnostics.Trace.TraceWarning("GdsDieAnalysisForm {0}: {1}", title, cause);
            if (ShowErrorDialogs) MessageBox.Show(this, cause.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 결과(ANL_INFO / CHAIN)에서 꺼낸 도면 읽기 조건과 키(2026-10-16).
        /// 키 = Factory|Device|보기 기준|ALL(배경) 또는 CHAIN|Chain 번호(정렬)|Chain 최종 저장 시각. 키가 같으면 같은 도형 / Chain Member 다.
        /// </summary>
        private sealed class MapRequest
        {
            public string Factory;
            public string Device;
            public string ViewSide;
            public bool Background;
            public HashSet<int> ChainLayers;
            public List<long> ChainSeqs;
            public string Key;

            public static MapRequest From(DataSet analysis, bool background)
            {
                DataRow info = analysis.Tables["ANL_INFO"].Rows[0];
                List<DataRow> chains = analysis.Tables.Contains("CHAIN") ? analysis.Tables["CHAIN"].Rows.Cast<DataRow>().ToList() : new List<DataRow>();
                var request = new MapRequest
                {
                    Factory = info["FACTORY"].ToString(),
                    Device = info["DEVICE_ID"].ToString(),
                    ViewSide = GdsSideTransform.Normalize(info["VIEW_SIDE"].ToString()),
                    Background = background,
                    ChainSeqs = chains.Select(r => Convert.ToInt64(r["CHAIN_SEQ"], CultureInfo.InvariantCulture)).Distinct().OrderBy(s => s).ToList(),
                    // LAYER_IDS = "번호/DataType" 목록(예 3/0,5/0). 이전 서버의 번호만 있는 값은 DataType 0 (2026-10-20)
                    ChainLayers = new HashSet<int>(chains
                        .SelectMany(r => r["LAYER_IDS"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        .Select(GdsLayerKey.Parse))
                };
                request.Key = request.Factory + "|" + request.Device + "|" + request.ViewSide + "|" + (background ? "ALL" : "CHAIN")
                    + "|" + String.Join(",", request.ChainSeqs.Select(s => s.ToString(CultureInfo.InvariantCulture)).ToArray())
                    + "|" + Col(info, "CHAIN_LAST_SAVED");
                return request;
            }
        }

        #endregion
    }
}
