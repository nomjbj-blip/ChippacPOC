using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// GDS × KLARF × E-Test 연관 분석 화면(화면 A).
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4절,
    ///       문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8.1 / 15 / 16절 (2026-10-16 Wafer 중심 Ready 배치)
    ///
    /// 기존 Defect Map Viewer 와 같은 방식으로 DACrux 왼쪽 DMSearch 패널(메뉴 RESV_04 = DM)에서 Wafer 를 받고(iSEMControl.DrawWafer),
    /// 기존 Wafer 그리기 서비스(RO.DefectMapAnalysis)로 Map 을 그린 뒤, 기능 탭(GDS / KLARF / E-Test) 위쪽에 이 Wafer 의 GDS 상태 / 고른 KLARF Step / E-Test 파일을 보인다(Wafer 1개만 분석, 2026-10-20).
    /// [분석] 으로 GDS Chain 영역 비교 결과를 받아 Die 를 색 기준(Chain Defect 수 / E-Test Fail Chain 수 / 일치 Chain 수 / 선택 Chain 4분류)으로 칠한다.
    /// Die 를 더블클릭하거나 E-Test Fail 행을 더블클릭하면 Die 상세 화면(화면 B, frmGdsDieAnalysis)으로 결과를 넘긴다.
    ///
    /// 처리 흐름
    ///  1) DrawWafer(Wafer 1장, 여러 장이면 오류) -> LoadSelectedWafer -> ShowWafer(Map / 분류 목록 / Device = Product 이름)
    ///     -> 기능 탭 위쪽 기준(서버 Ready 조회 GetWaferReady, 실패하면 고른 Step 하나로 만든 목록)
    ///  2) GDS Chain 면 / E-Test 공정 선택(필요하면 E-Test [직접 찾기]). KLARF 는 고른 Step 1개 + 검사 면 FS 고정 (2026-10-20)
    ///  3) [분석] -> RO.GdsKlarfAnalysis.Analyze(GdsKlarfAnalysisRequest, 백그라운드) -> ApplyResult(구간 / Map 색 / Fail 표식 / 목록 / 상태줄)
    ///  4) Die 클릭 = 목록 거르기, Die 더블클릭 / 우클릭 [Detail Analysis] / E-Test Fail 행 더블클릭 = OpenDetail(AnalyzeDie 로 그 Die 만 다시 계산)
    /// 서비스는 속성으로 바꿀 수 있다(기본 원격 호출, Smoke 검사는 같은 프로세스 BSL).
    ///
    /// 결과 DataSet 은 2026-10-15 통합 분석 설계 10.2절 열을 읽는다. 열이 없으면(이전 서버) 해당 칸을 비워 둔다.
    /// </summary>
    public partial class frmGdsKlarfAnalysis : DACruxUXBasic01, iSEMControl, frmGdsDieAnalysis.IDetailSource
    {
        /// <summary>
        /// Die 상세 화면 메뉴 키(숨김 메뉴, 설계 결정 A9). 2026-10-12 부터 상세 화면을 여는 데는 쓰지 않는다(설계 14.1절, ShowForm 중복 생성 버그).
        /// 메뉴 행은 상세 화면이 활성화될 때 DACrux 가 왼쪽 DMSearch 패널을 닫는 기준(RESV_04 = DMS)으로만 남긴다.
        /// </summary>
        public const string DetailMenuKey = "MNU_GDS_DIE_ANALYSIS";

        private const string AllItemTag = "ALL";
        private const string NoneItemTag = "NONE";
        private const string AddedByRule = "규칙";
        private const string AddedByHand = "직접";

        private long _stepSeq;
        private string _product = string.Empty;
        private string _device = string.Empty;
        private readonly List<string> _devices = new List<string>();
        private bool _showErrorDialogs = true;
        private string _stepFactory = string.Empty;
        private string _waferText = string.Empty;
        private DataRow _stepRow;
        private int _inspectedDies;
        private DataSet _result;
        private DataSet _ready;
        /// <summary>서버 준비 목록 조회 실패 안내(성공이면 빈 값). 점검 표를 뺀 뒤 상태줄로 보인다 (2026-10-16).</summary>
        private string _readyNote = string.Empty;
        /// <summary>[E-Test] 탭 파일 목록(READY_ET 모양, [직접 찾기] 행 포함). 표에는 Step 콤보로 고른 Step 행만 보인다 (2026-10-20).</summary>
        private DataTable _etFiles;
        private DataTable _imagePaths;
        private bool _dirty;
        private bool _loading;
        private bool _running;
        private string _sideText = string.Empty;
        /// <summary>마지막 [분석] 요청(상세 화면 AnalyzeDie 에 같은 조건을 쓴다). 결과를 직접 받은 경우 null.</summary>
        private GdsKlarfAnalysisRequest _lastRequest;
        /// <summary>분석 결과 순번(결과를 반영할 때마다 +1). 같은 Die 상세보기 창이 같은 결과로 열렸는지 비교한다 (2026-10-16).</summary>
        private int _analysisVersion;
        private string _focusChainSeq;
        /// <summary>[색 구간 설정] 팝업(처음 열 때 만든다). 색 구간 표(pnlBand)를 이 창으로 옮겨 보인다 (2026-10-19).</summary>
        private Form _bandPopup;
        /// <summary>기능 탭 머리 글꼴(고른 탭 = 굵게, 2026-10-19).</summary>
        private Font _tabBoldFont;
        private Image _foldCollapseImage;
        private Image _foldExpandImage;
        private Point? _selectedDie;
        private List<GdsKlarfColorBand> _bands = GdsKlarfColorBands.Auto(1, 1);

        /// <summary>Wafer 그리기 / Defect 이미지 경로 / KLARF Step 찾기 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.DefectMapAnalysis DefectService { get; set; }

        /// <summary>GDS Map Device 목록 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsMapView MapService { get; set; }

        /// <summary>Chain 목록 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsChain ChainService { get; set; }

        /// <summary>연관 분석 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsKlarfAnalysis AnalysisService { get; set; }

        /// <summary>E-Test 파일 찾기 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsEtResult EtService { get; set; }

        /// <summary>마지막 분석 결과(없으면 null). 화면 B 와 Smoke 검사가 읽는다.</summary>
        public DataSet AnalysisResult { get { return _result; } }

        /// <summary>지금 [준비] 탭에 보이는 준비 목록(없으면 null).</summary>
        public DataSet ReadyData { get { return _ready; } }

        /// <summary>오류를 메시지 창으로도 알릴지(기본 true). 자동 검사(Smoke)는 false 로 두고 상태줄만 본다.</summary>
        public bool ShowErrorDialogs
        {
            get { return _showErrorDialogs; }
            set { _showErrorDialogs = value; }
        }

        /// <summary>분석에 쓸 GDS Device(= KLARF Product 와 같은 이름의 READY Map). 없으면 빈 값이고 분석할 수 없다.</summary>
        public string GdsDevice { get { return _device; } }

        /// <summary>현재 색 구간(복사본).</summary>
        public List<GdsKlarfColorBand> ColorBands { get { return _bands.Select(b => new GdsKlarfColorBand { From = b.From, To = b.To, Color = b.Color }).ToList(); } }

        public frmGdsKlarfAnalysis()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            _loading = true;
            cboDefectScope.SelectedIndex = 0;
            cboClassBasis.SelectedIndex = 0;    // 결정 B7: 고객 파일은 CLASSNUMBER 가 0 이라 RoughBin 기본 (2026-10-16)
            cboColorBasis.SelectedIndex = 0;
            cboChainSide.SelectedIndex = 1;     // Chain 면 기본 BS (2026-10-20 변경, 이전 FS). 서버 Ready 기본값(READY_GDS.CHAIN_SIDE)도 BACK
            _loading = false;
            map.OnChangeCurrentDie += map_OnChangeCurrentDie;
            DefectArea_CheckedChanged(this, EventArgs.Empty);
            foreach (DataGridView grid in new[] { dgvDie, dgvChain, dgvDefect, dgvTolerance, dgvEtLink, dgvEtFail, dgvReadyGds })
                ConfigureGrid(grid);
            // [준비] 탭 표는 높이가 작아 머리글을 한 줄로 둔다(두 줄로 접히면 GDS 행이 가려진다)
            foreach (DataGridView grid in new[] { dgvReadyGds, dgvReadyEt })
                grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvBand.CellPainting += dgvBand_CellPainting; // E-Test Fail Die 행 색 칸 (2026-10-16)
            // Map E-Test 표식 체크 앞 그림 = Map 표식 모양 (2026-10-20)
            chkMarkSpec.Image = MarkImage(true);
            chkMarkFile.Image = MarkImage(false);
            // 색 구간 표 FROM 칸에 고정 행 이름("Fail Die (결과 파일)")이 들어가므로 다른 칸보다 넓게
            colBandFrom.FillWeight = 170;
            // [E-Test] 탭 Step 라벨 / 콤보는 굵은 제목 바로 뒤에 둔다. 화면 배율에 따라 제목 폭이 달라 고정 위치면 겹친다 (2026-10-20)
            lblReadyEt.SizeChanged += (s, e) => PlaceEtStep();
            lblEtStep.SizeChanged += (s, e) => PlaceEtStep();
            PlaceEtStep();
            // 화면 배치 (2026-10-19 안 1): 기능 탭 머리 글꼴 / 기준 접기 아이콘. "지금 기준" 요약 줄 / Map 범례는 2026-10-20 삭제(색 구간은 [색 구간 설정] 팝업에서만)
            _tabBoldFont = new Font(tabList.Font, FontStyle.Bold);
            _foldCollapseImage = btnGdsFold.Image;
            _foldExpandImage = new System.ComponentModel.ComponentResourceManager(typeof(frmGdsKlarfAnalysis)).GetObject("foldExpand.Image") as Image;
            RefreshBandGrid();
            BindResultTabs();
        }

        /// <summary>Device 목록을 미리 읽는다. 실패해도 화면은 연다(서비스 미기동 등).</summary>
        private void frmGdsKlarfAnalysis_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                LoadDeviceList();
            }
            catch (Exception ex)
            {
                SetStatus("GDS Map 목록 조회 실패: " + ex.Message);
            }
        }

        #region [Wafer]

        /// <summary>
        /// 왼쪽 DMSearch [Search] 결과를 받는다(iSEMControl). 분석은 Wafer(Step) 1장만 한다 (2026-10-16 요청: Wafer 콤보 제거).
        /// 여러 개를 고르면 오류로 알리고 지금 화면은 그대로 둔다. 같은 Wafer 의 다른 Step 을 함께 모으는 것은 2차(설계 B16, 2026-10-20).
        /// </summary>
        public void DrawWafer(DPWafer[] wafer)
        {
            List<DPWafer> list = wafer == null ? new List<DPWafer>() : wafer.GroupBy(w => w.StepSeq).Select(g => g.First()).ToList();
            if (list.Count == 0)
            {
                SetStatus("받은 Wafer 가 없습니다.");
                return;
            }
            if (list.Count > 1)
            {
                // 문구 2026-10-20: Wafer 1개만 분석 (같은 Wafer 다른 Step 모으기는 설계 B16 2차)
                string message = "1개 Wafer 만 분석할 수 있습니다. Wafer 를 하나만 선택하세요. (선택 " + list.Count + "개)";
                SetStatus(message.Replace(Environment.NewLine, " "));
                if (_showErrorDialogs) MessageBox.Show(this, message, "Wafer 선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DPWaferList = new[] { list[0] };
            LoadSelectedWafer(list[0]);
        }

        /// <summary>
        /// 선택한 Wafer 의 Defect / 기준 정보 / 검사 Die 를 기존 Defect Map Viewer 와 같은 서비스로 읽어 그린다.
        /// 진행 표시(2026-10-14): 서버 조회 3개는 화면 아래 busyBar 가 작업 스레드에서 돌리고, Map 그리기는 화면 스레드에서 한다.
        /// 서버 조회부터 Map 그리기(GDS Device / Chain 목록 포함)까지 한 작업으로 묶어 모래시계가 중간에 풀리지 않게 한다 (2026-10-15).
        /// 오류 창은 묶음이 끝난 뒤(catch) 띄운다.
        /// </summary>
        private void LoadSelectedWafer(DPWafer wafer)
        {
            long step;
            if (!long.TryParse(wafer.StepSeq, NumberStyles.Integer, CultureInfo.InvariantCulture, out step)) return;   // DPWafer 는 구조체

            try
            {
                SetStatus("Wafer 를 조회 중입니다...");
                RO.DefectMapAnalysis svc = DefectService ?? new RO.DefectMapAnalysis();
                long[] steps = { step };
                using (busyBar.Begin("Wafer 조회"))
                {
                    object[] data = busyBar.Run("서버 조회", () =>
                    {
                        var found = new DefectList();
                        found.AddRange(svc.GetDefectMapViewer_DefectArray(steps));
                        DataSet stepInfo = svc.GetDefectMapViewer_Info(steps);
                        if (!stepInfo.Tables.Contains("STEP_INFO") || stepInfo.Tables["STEP_INFO"].Rows.Count == 0)
                            throw new InvalidOperationException("Wafer 기준 정보(TQD_INSP_INFO)가 없습니다.");
                        DataRow stepRow = stepInfo.Tables["STEP_INFO"].Rows[0];
                        DataTable dies = svc.GetDefectMapViewer_Map(steps, new[] { stepRow["SETUP_SEQ"].ToString() }, new[] { stepRow["TEST"].ToString() });
                        return new object[] { found, stepInfo, dies };
                    });
                    ShowWafer(step, (DefectList)data[0], (DataSet)data[1], (DataTable)data[2]);
                }
                LoadImagePathsAsync(step);
            }
            catch (Exception ex)
            {
                SetStatus("Wafer 조회 실패: " + ex.Message);
                if (_showErrorDialogs) MessageBox.Show(this, ex.Message, "Wafer 조회", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Wafer Map 을 그린다. 설정 순서는 DPUCDefecMapViewer.Draw(DefectList) 와 같다(LeftBottom / Notch 0 / Pitch / Origin -> DieCalculation -> AddDie).
        /// 이전 분석 결과는 지우고 [준비] 탭을 다시 만든다. Smoke 검사도 이 함수를 직접 부른다.
        /// </summary>
        public void ShowWafer(long stepSeq, DefectList defects, DataSet info, DataTable mapTable)
        {
            DataRow stepRow = info.Tables["STEP_INFO"].Rows[0];
            DataRow setup = info.Tables["SETUP_INFO"].Rows[0];
            _stepSeq = stepSeq;
            _stepRow = stepRow;
            _inspectedDies = mapTable == null ? 0 : mapTable.Rows.Count;
            _product = ColText(stepRow, "PRODUCT");
            _stepFactory = ColText(stepRow, "FACTORY");
            _waferText = ColText(stepRow, "WAFER_ID") + " / " + ColText(stepRow, "STEP_ID");
            lblWaferInfo.Text = String.Format("{0}  /  {1}  (Product {2})", ColText(stepRow, "WAFER_ID"), ColText(stepRow, "STEP_ID"), _product);
            // Wafer 문구 길이에 따라 GDS Device 를 바로 뒤에 둔다(겹치지 않게)
            lblDevice.Left = lblWaferInfo.Right + 30;
            lblDeviceValue.Left = lblDevice.Right + 6;
            _result = null;
            _lastRequest = null;
            _dirty = false;
            _selectedDie = null;
            _focusChainSeq = null;
            _imagePaths = null;

            map.DefectClear();
            map.Defects.AddRange(defects ?? new DefectList());
            map.Rotate(0);
            map.WaferSize = DACrux.Base.Convert.doubleParse(setup["WAFER_SIZE"].ToString());
            map.NotchType = Notch.Notch;
            map.AngleOffSet = 0;
            map.XYDirect = XYDirection.LeftBottom;
            map.NotchAngle = 0;
            map.DieSizeX = DACrux.Base.Convert.doubleParse(setup["DIE_PITCH_X"].ToString());
            map.DieSizeY = DACrux.Base.Convert.doubleParse(setup["DIE_PITCH_Y"].ToString());
            map.OriginIndexX = DACrux.Base.Convert.intParse(setup["DIE_ORIGIN_X"].ToString());
            map.OriginIndexY = DACrux.Base.Convert.intParse(setup["DIE_ORIGIN_Y"].ToString());
            map.OriginX = DACrux.Base.Convert.doubleParse(setup["ORIGIN_X"].ToString());
            map.OriginY = DACrux.Base.Convert.doubleParse(setup["ORIGIN_Y"].ToString());
            map.DieCalculation(true);
            map.DrawDefects = "ALL";
            map.DieClear();
            TryRun(() => DACrux.SEMDMS.Control.UserConfiguration.SetVirtualDie(map));
            if (mapTable != null)
                foreach (DataRow dr in mapTable.Rows)
                    map.AddDie(new Die(DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString()), DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString()),
                        DACrux.Base.Convert.intParse(dr["TEST"].ToString()), 1));
            TryRun(() => DACrux.SEMDMS.Control.UserConfiguration.SetMapColor(map));
            map.ClearSelectedDie();
            map.ClearDieDisplay();
            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            map.DieSelectMode();   // 기본 Die 선택 (2026-10-16 요청: 우클릭 메뉴의 Defect / Die Select 제거)
            map.Redraw();

            FillClassMenu(map.Defects);
            string deviceNote = SelectDeviceForProduct();
            ClearResultViews();
            ShowReady(LoadReady());
            tabList.SelectedTab = tabKlarf;   // 고른 Step 정보 / Defect 영역을 정하는 KLARF 기준부터 보인다 (2026-10-19, Step 표 · 검사 면 콤보 없앰 2026-10-20)
            SetStatus(String.Format("{0} / Defect {1} / 검사 Die {2}{3}{4}",
                _waferText, map.Defects.Count, _inspectedDies, deviceNote, _readyNote.Length == 0 ? "" : " / " + _readyNote));
        }

        /// <summary>Defect 이미지 경로(TQD_IMAGES)를 백그라운드로 읽는다. 실패하면 Image 탭만 비어 있다.</summary>
        private void LoadImagePathsAsync(long step)
        {
            RO.DefectMapAnalysis svc = DefectService ?? new RO.DefectMapAnalysis();
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    DataTable paths = svc.GetDefectImagePath01(new[] { step });
                    if (IsHandleCreated && !IsDisposed)
                        BeginInvoke(new Action(() => { if (_stepSeq == step) { _imagePaths = paths; RefreshImages(); } }));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceWarning("Defect 이미지 경로 조회 실패: {0}", ex.Message);
                }
            });
        }

        #endregion

        #region [준비 탭]

        /// <summary>
        /// 준비 목록을 [준비] 탭에 보인다. 설계 16.4절 표(WAFER_KEY / READY_GDS / READY_KLARF / READY_ET)를 읽는다.
        /// READY_CHECK(점검)는 화면에 보이지 않고(오른쪽 아래 점검 표 삭제, 2026-10-16), 서버 조회 실패 행만 상태줄로 알린다.
        /// 서버 Ready 조회(GetWaferReady) 결과와 화면이 직접 만든 목록(BuildLocalReady)이 같은 모양이다. 분석 결과는 지우지 않고 "조건 변경됨" 으로 표시한다.
        /// </summary>
        public void ShowReady(DataSet ready)
        {
            _ready = ready;
            _loading = true;
            try
            {
                DataRow key = FirstRow(ready, "WAFER_KEY");
                string keyText = key == null ? "-" : ColText(key, "WAFER_KEY");
                bool confirmed = key != null && ColText(key, "CONFIRMED") == "Y";
                lblWaferKey.Text = "Wafer Key: " + (keyText.Length == 0 ? "-" : keyText)
                    + (key == null || confirmed ? "" : "    (규칙 잠정" + (ColText(key, "RULE_NAME").Length > 0 ? ": " + ColText(key, "RULE_NAME") : "") + ")");

                var gds = new DataTable();
                gds.Columns.Add("Device", typeof(string));
                gds.Columns.Add("Map", typeof(string));
                gds.Columns.Add("READY Chain", typeof(string));
                gds.Columns.Add("DRAFT Chain", typeof(string));
                gds.Columns.Add("Chain 저장", typeof(string));
                gds.Columns.Add("Cache", typeof(string));
                foreach (DataRow r in Rows(ready, "READY_GDS"))
                    gds.Rows.Add(ColText(r, "DEVICE_ID"), ColText(r, "MAP_STATUS"), ColText(r, "READY_CHAIN"), ColText(r, "DRAFT_CHAIN"),
                        ColText(r, "CHAIN_LAST_SAVED"), ColText(r, "CACHE_HIT"));
                dgvReadyGds.DataSource = gds;
                // Chain 면: 준비 목록이 기본값을 주면 따르고(서버 Ready), 없으면 엔지니어가 고른 값을 유지한다
                string chainSide = ColText(FirstRow(ready, "READY_GDS"), "CHAIN_SIDE");
                if (chainSide == "FRONT" || chainSide == "BACK") cboChainSide.SelectedIndex = chainSide == "BACK" ? 1 : 0;

                ShowStepInfo(ready);

                // E-Test: 서버가 같은 Wafer Key + 분석 GDS Device 파일만 준다. 화면은 Step 콤보로 Step 을 고르고 그 Step 파일만 보인다 (2026-10-20)
                _etFiles = NewReadySet().Tables["READY_ET"];
                foreach (DataRow r in Rows(ready, "READY_ET"))
                    _etFiles.ImportRow(r);
                DataRow picked = _etFiles.Rows.Cast<DataRow>().FirstOrDefault(r => ColText(r, "SELECTED") == "Y");
                ShowEtSteps(picked == null ? null : ColText(picked, "TEST_PROGRAM"), picked == null ? null : ColText(picked, "SRC_FILE_SEQ"));

                // 점검 표는 화면에서 뺐다(2026-10-16 요청). 서버 준비 목록 조회 실패와 Device 가 달라 뺀 E-Test 파일(2026-10-20)만 상태줄로 알린다
                DataRow failed = Rows(ready, "READY_CHECK").FirstOrDefault(r => ColText(r, "CODE") == "READY_LOCAL")
                    ?? Rows(ready, "READY_CHECK").FirstOrDefault(r => ColText(r, "CODE") == "ET_OTHER_DEVICE");
                _readyNote = failed == null ? string.Empty : ColText(failed, "MESSAGE");
            }
            finally
            {
                _loading = false;
            }
            Condition_Changed(this, EventArgs.Empty);
        }

        /// <summary>
        /// KLARF 탭 Step 정보 (2026-10-20: Step 표 대신 고른 Step 정보 + Pitch 출처, Wafer 1개만 분석).
        /// 준비 목록(READY_KLARF)에서 Map 을 그린 Step 행을 쓰고, 없으면 첫 행. KLARF 검사 면은 FS 고정이라 보이지 않는다(KlarfStepSide).
        /// 서버 Ready 가 같은 Wafer 의 다른 Step 을 함께 돌려줘도 화면은 고른 Step 하나만 분석한다(설계 B16 2차).
        /// </summary>
        private void ShowStepInfo(DataSet ready)
        {
            List<DataRow> rows = Rows(ready, "READY_KLARF").ToList();
            DataRow r = rows.FirstOrDefault(x => ColText(x, "STEP_SEQ") == _stepSeq.ToString(CultureInfo.InvariantCulture)) ?? rows.FirstOrDefault();
            if (r == null)
            {
                lblStepInfo.Text = "-";
                lblPitchSource.Text = "Pitch 출처: -";
                return;
            }
            // Step ID / Lot 이 길면 한 줄에서 뒤쪽(Defect / 검사 Die)이 잘려 두 줄로 나눈다 (2026-10-20)
            lblStepInfo.Text = String.Format("Step {0}  /  Lot {1}" + Environment.NewLine + "검사 {2}  /  Defect {3}  /  검사 Die {4}", ColText(r, "STEP_ID"), ColText(r, "LOT_ID"),
                ColText(r, "RESULTTIMESTAMP"), ColText(r, "DEFECTS"), ColText(r, "INSPECTED_DIE"));
            string pitch = ColText(r, "PITCH_SOURCE");
            lblPitchSource.Text = "Pitch 출처: " + (pitch.Length == 0 ? "-" : PitchText(pitch));
        }

        /// <summary>
        /// Step 콤보 = E-Test 파일 Step(파일 Step ID = TEST_PROGRAM) 종류 (2026-10-20). 목록이 Test 시각 최신 순이라 앞 Step 이 최신이다.
        /// step 이 목록에 없으면 첫 Step. 파일이 없으면 콤보를 끈다. selectSeq = 체크할 파일(없으면 그 Step 의 첫 = 최신 파일).
        /// </summary>
        private void ShowEtSteps(string step, string selectSeq)
        {
            List<string> steps = _etFiles == null ? new List<string>()
                : _etFiles.Rows.Cast<DataRow>().Select(r => ColText(r, "TEST_PROGRAM")).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            bool loading = _loading;
            _loading = true;
            try
            {
                cboEtStep.Items.Clear();
                foreach (string s in steps) cboEtStep.Items.Add(s);
                cboEtStep.Enabled = steps.Count > 0;
                int index = step == null ? -1 : steps.FindIndex(s => String.Equals(s, step, StringComparison.OrdinalIgnoreCase));
                cboEtStep.SelectedIndex = steps.Count == 0 ? -1 : Math.Max(0, index);
            }
            finally
            {
                _loading = loading;
            }
            ShowEtRows(selectSeq);
        }

        /// <summary>고른 Step 의 E-Test 파일만 표에 보인다. 체크는 1개(selectSeq, 없거나 다른 Step 이면 첫 = Test 시각 최신 파일, 결정 B19).</summary>
        private void ShowEtRows(string selectSeq)
        {
            bool loading = _loading;
            _loading = true;
            try
            {
                dgvReadyEt.Rows.Clear();
                if (_etFiles == null || cboEtStep.SelectedIndex < 0) return;
                List<DataRow> rows = _etFiles.Rows.Cast<DataRow>()
                    .Where(r => String.Equals(ColText(r, "TEST_PROGRAM"), cboEtStep.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                if (selectSeq == null || !rows.Any(r => ColText(r, "SRC_FILE_SEQ") == selectSeq))
                    selectSeq = rows.Count == 0 ? null : ColText(rows[0], "SRC_FILE_SEQ");
                foreach (DataRow r in rows)
                    AddReadyEtRow(r, ColText(r, "SRC_FILE_SEQ") == selectSeq);
            }
            finally
            {
                _loading = loading;
            }
        }

        /// <summary>Step 라벨 = 제목 오른쪽(글자 높이 반 만큼 띄움), 콤보 = Step 라벨 오른쪽.</summary>
        private void PlaceEtStep()
        {
            lblEtStep.Left = lblReadyEt.Right + lblReadyEt.Height / 2;
            cboEtStep.Left = lblEtStep.Right + 4;
        }

        /// <summary>Step 을 바꾸면 그 Step 의 최신 파일을 고른다.</summary>
        private void cboEtStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            ShowEtRows(null);
            Condition_Changed(sender, e);
        }

        private void AddReadyEtRow(DataRow r, bool use)
        {
            string spec = ColText(r, "SPEC_CHANGED") == "Y" ? "바뀜" : "그대로";
            string link = ColText(r, "LINKED_CHAIN").Length == 0 ? "" : ColText(r, "LINKED_CHAIN") + " / 미연결 " + (ColText(r, "UNLINKED_NAME").Length == 0 ? "0" : ColText(r, "UNLINKED_NAME"));
            int i = dgvReadyEt.Rows.Add(use, ColText(r, "TEST_PROGRAM"), ColText(r, "WAFER_ID"), ColText(r, "DEVICE_ID"), ColText(r, "TEST_TIME"),
                ColText(r, "DIE_COUNT"), ColText(r, "FAIL_DIE_COUNT"), ColText(r, "JUDGE_TIME"), spec, link, AddedText(ColText(r, "ADDED_BY")), ColText(r, "SRC_FILE_SEQ"));
            if (spec == "바뀜") dgvReadyEt.Rows[i].Cells[colEtSpec.Index].Style.ForeColor = Color.DarkOrange;
        }

        private static string PitchText(string source)
        {
            return source == "KLARF" ? "KLARF" : source == "MAP_SETUP" ? "Map Setup" : source;
        }

        /// <summary>"추가" 열 문구. PREV / NEXT 는 같은 Step ID 를 다시 검사한 다른 행이며 기본 체크하지 않는다(결정 B27, 2026-10-16).</summary>
        private static string AddedText(string addedBy)
        {
            return addedBy == "HAND" ? AddedByHand : addedBy == "RULE" ? AddedByRule
                : addedBy == "PREV" ? "이전 검사" : addedBy == "NEXT" ? "이후 검사" : addedBy;
        }

        /// <summary>
        /// 준비 목록을 서버 Ready 조회(GetWaferReady, 설계 16.4절)로 만든다 (2026-10-16).
        /// 서버 호출이 실패하면(이전 서버 / 연결 끊김) 고른 Step 하나로 만든 목록을 쓰고 실패 사유는 상태줄에 보인다.
        /// </summary>
        private DataSet LoadReady()
        {
            if (_stepSeq <= 0) return BuildLocalReady(null);
            try
            {
                RO.GdsKlarfAnalysis svc = AnalysisService ?? new RO.GdsKlarfAnalysis();
                string factory = CurrentFactory, device = _device;
                long step = _stepSeq;
                return busyBar.RunHere("준비 목록", () => svc.GetWaferReady(factory, step, device));
            }
            catch (Exception ex)
            {
                return BuildLocalReady(ex.Message);
            }
        }

        /// <summary>
        /// DMSearch 에서 고른 Step 하나로 준비 목록을 만든다(서버 Ready 조회를 못 했을 때). GDS 줄은 지금 고른 Device 와 Chain 메뉴로 채운다.
        /// serverError = 서버 Ready 조회 실패 사유(READY_LOCAL 행 -> 상태줄).
        /// </summary>
        private DataSet BuildLocalReady(string serverError)
        {
            DataSet ds = NewReadySet();
            ds.Tables["WAFER_KEY"].Rows.Add("", "", "N", _stepSeq.ToString(CultureInfo.InvariantCulture), CurrentFactory);
            // KLARF 검사 면 FS 고정 / Chain 면은 화면 콤보(기본 BS) -> 기본 조건은 반전 (2026-10-20, 이전 결정 A6 검사 면 BS)
            AddLocalGdsRow(ds);
            if (_stepRow != null)
                ds.Tables["READY_KLARF"].Rows.Add(_stepSeq.ToString(CultureInfo.InvariantCulture), ColText(_stepRow, "STEP_ID"), ColText(_stepRow, "LOT_ID"),
                    ColText(_stepRow, "RESULTTIMESTAMP"), map.Defects.Count.ToString(CultureInfo.InvariantCulture), _inspectedDies.ToString(CultureInfo.InvariantCulture),
                    KlarfStepSide, "", "RULE", "Y");
            DataTable check = ds.Tables["READY_CHECK"];
            if (_device.Length == 0)
                check.Rows.Add("GDS_NO_MAP", "오류", "Product " + _product + " 와 같은 이름의 READY GDS Map 이 없어 분석할 수 없습니다. (GDS Map 조회 화면 [Map 복사])");
            if (serverError != null)
                check.Rows.Add("READY_LOCAL", "주의", "서버 준비 목록 조회 실패(" + serverError.Split('\n')[0].Trim() + ") - 고른 Step 만 보입니다. [다시 조회]로 다시 시도하세요.");
            return ds;
        }

        private void AddLocalGdsRow(DataSet ds)
        {
            string device = _device;
            List<ToolStripMenuItem> chains = menuChain.Items.OfType<ToolStripMenuItem>().Where(i => !AllItemTag.Equals(i.Tag) && !NoneItemTag.Equals(i.Tag)).ToList();
            ds.Tables["READY_GDS"].Rows.Add(device ?? "", String.IsNullOrEmpty(device) ? "없음" : "READY",
                chains.Count(i => i.Enabled).ToString(CultureInfo.InvariantCulture), chains.Count(i => !i.Enabled).ToString(CultureInfo.InvariantCulture), "", "", "");
        }

        /// <summary>준비 목록 DataSet 빈 틀(설계 16.4절 열).</summary>
        public static DataSet NewReadySet()
        {
            var ds = new DataSet("READY");
            AddTable(ds, "WAFER_KEY", "WAFER_KEY", "RULE_NAME", "CONFIRMED", "STEP_SEQ", "FACTORY");
            AddTable(ds, "READY_GDS", "DEVICE_ID", "MAP_STATUS", "READY_CHAIN", "DRAFT_CHAIN", "CHAIN_LAST_SAVED", "CACHE_HIT", "CHAIN_SIDE");
            AddTable(ds, "READY_KLARF", "STEP_SEQ", "STEP_ID", "LOT_ID", "RESULTTIMESTAMP", "DEFECTS", "INSPECTED_DIE", "VIEW_SIDE", "PITCH_SOURCE", "ADDED_BY", "USE_FLAG");
            AddTable(ds, "READY_ET", "SRC_FILE_SEQ", "TEST_PROGRAM", "WAFER_ID", "DEVICE_ID", "TEST_TIME", "DIE_COUNT", "FAIL_DIE_COUNT", "JUDGE_TIME",
                "SPEC_CHANGED", "LINKED_CHAIN", "UNLINKED_NAME", "MAP_SHAPE_SAME", "ADDED_BY", "SELECTED");
            AddTable(ds, "READY_CHECK", "CODE", "LEVEL", "MESSAGE");
            return ds;
        }

        private static void AddTable(DataSet ds, string name, params string[] columns)
        {
            DataTable t = ds.Tables.Add(name);
            foreach (string c in columns) t.Columns.Add(c, typeof(string));
        }

        /// <summary>체크 / 콤보 칸은 고르는 즉시 값을 확정해 CellValueChanged 가 바로 오게 한다.</summary>
        private void ReadyGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.IsCurrentCellDirty && (grid.CurrentCell is DataGridViewCheckBoxCell || grid.CurrentCell is DataGridViewComboBoxCell))
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>E-Test 는 고른 Step 의 파일 1개만 고른다(결정 B19, Step 콤보 2026-10-20). 하나를 체크하면 나머지를 끈다.</summary>
        private void dgvReadyEt_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0 || e.ColumnIndex != colEtUse.Index) return;
            if (true.Equals(dgvReadyEt.Rows[e.RowIndex].Cells[colEtUse.Index].Value))
            {
                _loading = true;
                foreach (DataGridViewRow row in dgvReadyEt.Rows)
                    if (row.Index != e.RowIndex) row.Cells[colEtUse.Index].Value = false;
                _loading = false;
            }
            Condition_Changed(sender, e);
        }

        private void btnReadyRefresh_Click(object sender, EventArgs e)
        {
            if (_stepSeq <= 0) return;
            ShowReady(LoadReady());
            if (_readyNote.Length > 0) SetStatus(_readyNote);
        }

        /// <summary>
        /// 분석할 KLARF Step: Map 을 그린 Step 1개 + KLARF 검사 면 FS (STEP_SEQ, FRONT).
        /// 2026-10-20 Wafer 1개만 분석: Step 표를 없앴다. 서버 Analyze 는 여러 Step 을 받을 수 있지만 화면은 1개만 보낸다(설계 B16 2차).
        /// </summary>
        private List<KeyValuePair<long, string>> ReadySteps()
        {
            var steps = new List<KeyValuePair<long, string>>();
            if (_stepSeq > 0) steps.Add(new KeyValuePair<long, string>(_stepSeq, KlarfStepSide));
            return steps;
        }

        /// <summary>
        /// KLARF 검사 면 = FS 고정, 화면에 보이지 않는다 (2026-10-20). 고르는 것은 GDS Chain 면만(기본 BS) -> 기본 조건 Chain BS · 검사 FS 는 반전.
        /// 서버는 검사 면을 반전에만 쓰고 KLARF / E-Test Die 좌표는 그대로 쓴다.
        /// </summary>
        private const string KlarfStepSide = "FRONT";

        /// <summary>Chain 면(분석에 1개, 2026-10-16): GDS Chain 이 그려진 면. FRONT(FS) / BACK(BS).</summary>
        private string ChainSide { get { return cboChainSide.SelectedIndex == 1 ? "BACK" : "FRONT"; } }

        private void cboChainSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            Condition_Changed(sender, e);
        }

        /// <summary>FRONT -> FS, 그 밖 -> BS (화면 표시). 반전 규칙(Chain 면 ≠ 검사 면)은 서버 Analyze 가 Step 마다 계산한다 (2026-10-16).</summary>
        private static string SideCode(string side)
        {
            return side == "FRONT" ? "FS" : "BS";
        }

        /// <summary>[준비] 탭에서 고른 E-Test 파일(SRC_FILE_SEQ). 없으면 0.</summary>
        private long ReadyEtFile()
        {
            foreach (DataGridViewRow row in dgvReadyEt.Rows)
            {
                long seq;
                if (true.Equals(row.Cells[colEtUse.Index].Value) && long.TryParse(System.Convert.ToString(row.Cells[colEtSeq.Index].Value), out seq)) return seq;
            }
            return 0;
        }

        /// <summary>
        /// [직접 찾기] E-Test: 기존 E-Test 결과 조회 화면 목록 서비스(GetEtFileBrowseList)로 찾는다. 고른 행을 E-Test 목록에 "직접" 으로 더하고,
        /// Step 콤보를 그 파일 Step 으로 바꿔 선택한다. 찾기 Device 기본값 = 분석 GDS Device (2026-10-20).
        /// </summary>
        private void btnFindEt_Click(object sender, EventArgs e)
        {
            if (_stepSeq <= 0) return;
            using (var dlg = new frmGdsKlarfSourceFind())
            {
                dlg.Text = "E-Test 파일 찾기";
                dlg.CompareText = "비교용 KLARF: " + CompareText();
                dlg.ShowDevice = true;
                dlg.ShowProgram = true;
                dlg.SingleSelect = true;
                dlg.DeviceFilter = _device;
                RO.GdsEtResult svc = EtService ?? new RO.GdsEtResult();
                string factory = CurrentFactory;
                dlg.Search = (device, program, lot) => ToEtFindTable(svc.GetEtFileBrowseList(factory, device, program, lot));
                if (dlg.ShowDialog(this) != DialogResult.OK || dlg.SelectedRows.Count == 0) return;
                DataRow r = dlg.SelectedRows[0];
                string seq = r["_SRC_FILE_SEQ"].ToString();
                if (_etFiles == null) _etFiles = NewReadySet().Tables["READY_ET"];
                DataRow file = _etFiles.Rows.Cast<DataRow>().FirstOrDefault(x => ColText(x, "SRC_FILE_SEQ") == seq)
                    ?? _etFiles.Rows.Add(seq, r["Step"], r["WAFER_ID"], r["Device"], r["Test 시각"], r["Die"], r["Fail Die"], r["판정 시각"],
                        r["Spec 바뀜"], "", "", "", "HAND", "N");
                ShowEtSteps(ColText(file, "TEST_PROGRAM"), seq);
                Condition_Changed(sender, e);
            }
        }

        private static DataTable ToEtFindTable(DataTable list)
        {
            var t = new DataTable();
            t.Columns.Add("WAFER_ID", typeof(string));
            t.Columns.Add("Lot", typeof(string));
            t.Columns.Add("Slot", typeof(string));
            t.Columns.Add("Step", typeof(string));
            t.Columns.Add("Device", typeof(string));
            t.Columns.Add("Test 시각", typeof(string));
            t.Columns.Add("Die", typeof(string));
            t.Columns.Add("Fail Die", typeof(string));
            t.Columns.Add("판정 시각", typeof(string));
            t.Columns.Add("Spec 바뀜", typeof(string));
            t.Columns.Add("_SRC_FILE_SEQ", typeof(string));
            if (list != null)
                foreach (DataRow r in list.Rows)
                    t.Rows.Add(ColText(r, "WAFER_ID"), ColText(r, "LOT_ID"), ColText(r, "SLOT_NO"), ColText(r, "TEST_PROGRAM"), ColText(r, "DEVICE_ID"),
                        ColText(r, "TEST_TIME"), ColText(r, "DIE_COUNT"), ColText(r, "FAIL_DIE_COUNT"), ColText(r, "JUDGE_TIME"), ColText(r, "SPEC_CHANGED"),
                        ColText(r, "SRC_FILE_SEQ"));
            return t;
        }

        private string CompareText()
        {
            return _stepRow == null ? "-" : String.Format("Lot {0} / Slot {1} / Wafer {2} / {3}",
                ColText(_stepRow, "LOT_ID"), ColText(_stepRow, "SLOT_ID"), ColText(_stepRow, "WAFER_ID"), ColText(_stepRow, "STEP_ID"));
        }

        #endregion

        #region [Device / Chain / 분류 선택]

        /// <summary>DACrux 로그인 Factory. 없으면 KLARF Step 의 FACTORY.</summary>
        private string CurrentFactory
        {
            get
            {
                string factory = (GlobalVariable.Factory ?? string.Empty).Trim();
                return factory.Length > 0 ? factory : _stepFactory;
            }
        }

        /// <summary>READY GDS Map 이 있는 Device 이름을 읽는다(로그인 Factory 기준).</summary>
        private void LoadDeviceList()
        {
            DataTable devices = busyBar.RunHere("GDS Device 목록", () => (MapService ?? new RO.GdsMapView()).GetMapDeviceList());
            string factory = CurrentFactory;
            _devices.Clear();
            foreach (DataRow row in devices.Rows)
                if (factory.Length == 0 || String.Equals(row["FACTORY"].ToString(), factory, StringComparison.OrdinalIgnoreCase))
                    _devices.Add(row["DEVICE_ID"].ToString());
        }

        /// <summary>
        /// GDS Device 는 Wafer 의 Product 를 따른다(2026-10-16 요청). 같은 이름(대소문자 무시)의 READY GDS Map 이 없으면 빈 값이고 분석할 수 없다.
        /// Sub Device 연결은 2026-10-15 제거했다. 이름이 다르면 GDS Map 조회 화면 [Map 복사]로 Product 이름의 Map 을 만든다.
        /// 반환: 상태줄에 붙일 안내(" / Product ...: 같은 이름의 READY GDS Map 없음 (분석 불가)" 또는 빈 값)
        /// </summary>
        private string SelectDeviceForProduct()
        {
            if (_devices.Count == 0) TryRun(LoadDeviceList);
            string device = _product.Length == 0 ? null
                : _devices.Contains(_product) ? _product
                : _devices.FirstOrDefault(d => String.Equals(d, _product, StringComparison.OrdinalIgnoreCase));
            _device = device ?? string.Empty;
            lblDeviceValue.Text = _device.Length > 0 ? _device : "없음 - 분석 불가";
            lblDeviceValue.ForeColor = _device.Length > 0 ? Color.FromArgb(20, 60, 130) : Color.Firebrick;
            TryRun(LoadChainMenu);
            return _device.Length > 0 || _product.Length == 0 ? string.Empty : " / Product " + _product + ": 같은 이름의 READY GDS Map 없음 (분석 불가)";
        }

        /// <summary>Device 의 Chain 목록을 체크 메뉴로 만든다. 저장 미완료(DRAFT) Chain 은 고를 수 없다.</summary>
        private void LoadChainMenu()
        {
            menuChain.Items.Clear();
            string device = _device;
            if (String.IsNullOrEmpty(device)) { UpdateCheckButton(btnChain, menuChain, "Chain"); return; }
            DataTable chains = busyBar.RunHere("Chain 목록", () => (ChainService ?? new RO.GdsChain()).GetChainList(CurrentFactory, device));
            AddAllNoneItems(menuChain);
            foreach (DataRow row in chains.Rows)
            {
                bool ready = row["VERSION_STATUS"].ToString() == "READY";
                // Chain 코드(A, B, AA ..)는 화면에 보이지 않고 이름만 쓴다 (2026-10-16)
                var item = new ToolStripMenuItem(String.Format("{0}  (Member {1}){2}", row["CHAIN_NAME"], row["MEMBER_COUNT"],
                    ready ? "" : " - 저장 미완료"))
                {
                    CheckOnClick = ready,
                    Checked = ready,
                    Enabled = ready,
                    Tag = row["CHAIN_SEQ"].ToString()
                };
                menuChain.Items.Add(item);
            }
            UpdateCheckButton(btnChain, menuChain, "Chain");
        }

        /// <summary>분류 기준(결정 B7): RoughBin = ROUGHBINNUMBER, Class = CLASSNUMBER. 고객 파일은 실제 분류(RDLopen / RDLshort)가 RoughBin 에 있다.</summary>
        private bool UseRoughBin { get { return cboClassBasis.SelectedIndex != 1; } }

        private string ClassBasisName { get { return UseRoughBin ? "RoughBin" : "Class"; } }

        private void cboClassBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            FillClassMenu(map.Defects);
            Condition_Changed(sender, e);
        }

        /// <summary>이 Wafer 의 Defect 분류를 체크 메뉴로 만든다(전부 체크). 이름은 코드 충돌(TQD_DEFECT_TYPE) 때문에 번호와 개수만 쓴다.</summary>
        private void FillClassMenu(DefectList defects)
        {
            menuClass.Items.Clear();
            AddAllNoneItems(menuClass);
            bool roughBin = UseRoughBin;
            foreach (var g in defects.GroupBy(d => roughBin ? d.ROUGHBINNUMBER : d.CLASSNUMBER).OrderBy(g => g.Key))
                menuClass.Items.Add(new ToolStripMenuItem(String.Format("{0} {1}  ({2}개)", ClassBasisName, g.Key, g.Count()))
                {
                    CheckOnClick = true,
                    Checked = true,
                    Tag = g.Key.ToString(CultureInfo.InvariantCulture)
                });
            UpdateCheckButton(btnClass, menuClass, ClassBasisName);
        }

        private static void AddAllNoneItems(ContextMenuStrip menu)
        {
            menu.Items.Add(new ToolStripMenuItem("전체 선택") { Tag = AllItemTag });
            menu.Items.Add(new ToolStripMenuItem("전체 해제") { Tag = NoneItemTag });
            menu.Items.Add(new ToolStripSeparator());
        }

        private void btnClass_Click(object sender, EventArgs e)
        {
            menuClass.Show(btnClass, new Point(0, btnClass.Height));
        }

        private void btnChain_Click(object sender, EventArgs e)
        {
            menuChain.Show(btnChain, new Point(0, btnChain.Height));
        }

        /// <summary>항목을 눌러도 메뉴를 닫지 않아 여러 개를 연속으로 고를 수 있게 한다. 전체 선택 / 해제도 여기서 처리한다.</summary>
        private void CheckMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            var menu = (ContextMenuStrip)sender;
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
                ToolStripItem clicked = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Selected);
                if (clicked != null && (AllItemTag.Equals(clicked.Tag) || NoneItemTag.Equals(clicked.Tag)))
                    foreach (ToolStripMenuItem item in CheckItems(menu))
                        item.Checked = AllItemTag.Equals(clicked.Tag);
            }
            UpdateCheckButton(menu == menuClass ? btnClass : btnChain, menu, menu == menuClass ? ClassBasisName : "Chain");
            Condition_Changed(sender, e);
        }

        private static IEnumerable<ToolStripMenuItem> CheckItems(ContextMenuStrip menu)
        {
            return menu.Items.OfType<ToolStripMenuItem>().Where(i => i.Enabled && !AllItemTag.Equals(i.Tag) && !NoneItemTag.Equals(i.Tag));
        }

        private static void UpdateCheckButton(Button button, ContextMenuStrip menu, string name)
        {
            List<ToolStripMenuItem> items = CheckItems(menu).ToList();
            int on = items.Count(i => i.Checked);
            button.Text = String.Format("{0}: {1} ▼", name, items.Count == 0 ? "없음" : on == items.Count ? "전체(" + on + ")" : on + " / " + items.Count);
        }

        /// <summary>고른 값. 전부 골랐으면 null(= 서비스에서 전체), 하나도 안 골랐으면 빈 배열.</summary>
        private static string[] CheckedTags(ContextMenuStrip menu)
        {
            List<ToolStripMenuItem> items = CheckItems(menu).ToList();
            if (items.Count > 0 && items.All(i => i.Checked)) return null;
            return items.Where(i => i.Checked).Select(i => (string)i.Tag).ToArray();
        }

        #endregion

        #region [분석]

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (_running) return;
            AnalysisRequest request;
            string error = BuildRequest(out request);
            if (error != null)
            {
                MessageBox.Show(this, error, "분석", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetRunning(true);
            RO.GdsKlarfAnalysis svc = AnalysisService ?? new RO.GdsKlarfAnalysis();
            // 진행 표시(2026-10-14): 서버 분석은 화면 아래 busyBar 가 작업 스레드에서 돌리고 작업 이름을 보인다(그동안 이 화면 입력은 막음)
            DataSet ds = null;
            Exception failure = null;
            try { ds = busyBar.Run("분석 (" + request.Device + ")", () => Call(svc, request)); }
            catch (Exception ex) { failure = ex; }
            if (!IsDisposed) FinishAnalysis(request, ds, failure);
        }

        /// <summary>상세보기 [다시 분석] (2026-10-22): [분석] 과 같다. 결과를 반영하면 상세보기 창도 새 결과로 바뀐다(SourceAnalyzed).</summary>
        public void AnalyzeAgain()
        {
            btnAnalyze_Click(this, EventArgs.Empty);
        }

        /// <summary>Smoke 검사용: 현재 조건으로 같은 스레드에서 분석하고 화면에 반영한다. 조건 오류면 예외.</summary>
        public DataSet AnalyzeNow()
        {
            AnalysisRequest request;
            string error = BuildRequest(out request);
            if (error != null) throw new InvalidOperationException(error);
            DataSet ds = Call(AnalysisService ?? new RO.GdsKlarfAnalysis(), request);
            _sideText = request.SideText;
            _lastRequest = request.Request;
            ApplyResult(ds);
            return ds;
        }

        /// <summary>
        /// 화면 조건을 확인해 통합 분석 요청(GdsKlarfAnalysisRequest, 설계 10.1절)을 만든다. 문제가 있으면 안내 문구를 돌려준다 (2026-10-16 서버 Analyze 연결).
        /// 고른 Step 1개(KLARF 검사 면 FS 고정) / Chain 면 / 고른 E-Test 파일과 조건 줄의 Defect 영역 / 보정 / 분류 / Chain 거르기를 그대로 보낸다.
        /// 서버가 반전 = (Chain 면 ≠ 검사 면) 으로 판정하고 KLARF / E-Test Die 좌표는 그대로 쓴다 (2026-10-20).
        /// </summary>
        private string BuildRequest(out AnalysisRequest request)
        {
            request = null;
            if (_stepSeq <= 0) return "왼쪽 Wafer 조회에서 Wafer 를 먼저 [Search] 하세요.";
            string device = _device;
            if (String.IsNullOrEmpty(device))
                return "Product " + _product + " 와 같은 이름의 READY GDS Map 이 없어 분석할 수 없습니다." + Environment.NewLine
                    + "GDS Map 조회 화면 [Map 복사]로 Product 이름의 Map 을 만드세요.";
            List<KeyValuePair<long, string>> steps = ReadySteps();
            string[] chains = CheckedTags(menuChain);
            if (chains != null && chains.Length == 0) return "비교할 Chain 을 1개 이상 고르세요.";
            string[] classes = CheckedTags(menuClass);
            if (classes != null && classes.Length == 0) return ClassBasisName + " 를 1개 이상 고르세요.";
            string stepSide = steps.First(s => s.Key == _stepSeq).Value;
            string chainSide = ChainSide;
            DataRow key = FirstRow(_ready, "WAFER_KEY");
            request = new AnalysisRequest
            {
                StepSeq = _stepSeq,
                Device = device,
                // 검사 면(KLARF FS 고정)은 보이지 않는다 (2026-10-20)
                SideText = String.Format("Chain {0}{1}", SideCode(chainSide), chainSide != stepSide ? " -> 반전" : ""),
                Request = new GdsKlarfAnalysisRequest
                {
                    Factory = CurrentFactory,
                    DeviceId = device,
                    WaferKey = ColText(key, "WAFER_KEY"),
                    StepSeqs = steps.Select(s => s.Key).ToArray(),
                    ViewSides = steps.Select(s => s.Value).ToArray(),
                    ChainSide = chainSide,
                    DisplayStepSeq = _stepSeq,
                    // Defect 영역(2026-10-16): 크기 + 허용오차 / 중심 정사각형(한 변, 걸림 = Gap 0)
                    AreaMode = rdoDefectSize.Checked ? GdsKlarfAnalysisRequest.AreaSizeTolerance : GdsKlarfAnalysisRequest.AreaSquare,
                    ToleranceUm = (double)numTolerance.Value,
                    SquareUm = (double)numSquare.Value,
                    ExcludeLargeDefect = chkLargeDefect.Checked,
                    OffsetXUm = (double)numOffsetX.Value,
                    OffsetYUm = (double)numOffsetY.Value,
                    ChainSeqs = chains,
                    ClassBasis = UseRoughBin ? GdsKlarfAnalysisRequest.ClassBasisRoughBin : GdsKlarfAnalysisRequest.ClassBasisClass,
                    ClassValues = classes == null ? null : classes.Select(c => int.Parse(c, CultureInfo.InvariantCulture)).ToArray(),
                    EtSrcFileSeq = ReadyEtFile()
                }
            };
            return null;
        }

        private static DataSet Call(RO.GdsKlarfAnalysis svc, AnalysisRequest r)
        {
            return svc.Analyze(r.Request);
        }

        private void FinishAnalysis(AnalysisRequest request, DataSet ds, Exception failure)
        {
            SetRunning(false);
            if (request.StepSeq != _stepSeq) return;   // 분석 중 Wafer 를 바꿨다
            if (failure != null)
            {
                SetStatus("분석 실패: " + failure.Message);
                MessageBox.Show(this, failure.Message, "분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _sideText = request.SideText;
            _lastRequest = request.Request;
            // 상세보기 도면 데이터를 백그라운드로 미리 읽는다. 첫 상세보기도 도면 읽기(약 6초)를 기다리지 않는다 (2026-10-16 설계 5.3절).
            // 결과 반영 끝에 열린 상세보기도 새 결과로 바꾸므로(도면 키가 바뀌면 그 읽기를 기다림) 반영보다 먼저 시작한다 (2026-10-22)
            frmGdsDieAnalysis.Preload(this, ds);
            // 결과 반영(Map 색 / 표)도 화면 스레드에서 시간이 걸려 모래시계 커서를 유지한다 (2026-10-15)
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
                ApplyResult(ds);
        }

        /// <summary>화면을 닫으면 상세보기 공유 도면을 놓는다(열린 상세보기 창도 없으면 메모리를 돌려준다, 2026-10-16 설계 5.4절).</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            frmGdsDieAnalysis.Release(this);
            if (MdiParent != null) MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            base.OnFormClosed(e);
        }

        private void SetRunning(bool running)
        {
            _running = running;
            btnAnalyze.Enabled = !running;
            if (running) SetStatus("분석 중...");
        }

        /// <summary>
        /// 분석 결과를 화면에 그대로 보인다(서버를 부르지 않음). 서버 결과를 다른 곳에서 받았거나 레이아웃을 확인할 때 쓴다.
        /// 결과 표는 설계 10.2절 열을 읽고, 없는 열은 비워 둔다.
        /// </summary>
        public void ShowAnalysisResult(DataSet ds)
        {
            ApplyResult(ds);
        }

        /// <summary>
        /// 분석 결과를 화면에 반영한다: 자동 구간 -> Fail 표식 -> Map 색 -> 구간 표 -> 목록 -> 상태줄. 이 Wafer 의 처음 결과면 [종합 (Die)] 탭을 연다(2026-10-19 결정 L4).
        /// 마지막에 이 화면에서 연 상세보기 창도 새 결과로 바꾼다(2026-10-22 동기화, 상세보기 창 SourceAnalyzed).
        /// </summary>
        private void ApplyResult(DataSet ds)
        {
            bool firstResult = _result == null;
            _result = ds;
            _analysisVersion++;
            _dirty = false;
            if (_focusChainSeq != null && FindChainRow(_focusChainSeq) == null) _focusChainSeq = null;
            if (_focusChainSeq == null) _focusChainSeq = DefaultFocusChain();
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxBasisValue());
            if (_selectedDie.HasValue && FindDieRow(_selectedDie.Value) == null) _selectedDie = null;
            ApplyEtMarks();
            ApplyMapColors();
            BindDieGrid();
            RefreshLists();
            BindResultTabs();
            UpdateStatus();
            if (firstResult) tabList.SelectedTab = tabDie;
            frmGdsDieAnalysis.SourceAnalyzed(this, _analysisVersion, _result, _lastRequest, AnalysisService);
        }

        private int Threshold { get { return (int)numThreshold.Value; } }

        #endregion

        #region [색 기준 / 색 구간 표]

        private GdsKlarfColorBasis ColorBasis { get { return (GdsKlarfColorBasis)Math.Max(0, cboColorBasis.SelectedIndex); } }

        /// <summary>색 기준을 바꾸면 서버를 다시 부르지 않고 같은 결과로 다시 칠한다 (2026-10-16).</summary>
        private void cboColorBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxBasisValue());
            ApplyMapColors();
        }

        /// <summary>
        /// Map E-Test 표식 (2026-10-20, 설계 문서/설계/05_연관분석/2026-10-20_Map_ETest_표식_설계.md): 두 가지를 다른 모서리에 함께 그린다.
        ///  Spec Out = DMS Spec 판정(ET_STATE) F -> 왼쪽 위 빨간 삼각형
        ///  Fail Die = 결과 파일 DUT Pass / Fail Flag(ET_FILE_FLAG) F -> 오른쪽 아래 작은 남색 삼각형
        /// [E-Test] 탭 체크 2개로 따로 끈다(기본 둘 다 켬). 열이 없는 예전 서버 결과면 Fail Die 는 0개다.
        /// </summary>
        private void ApplyEtMarks()
        {
            map.SetEtMarks(chkMarkSpec.Checked ? EtMarkDies(IsSpecOutDie) : null, chkMarkFile.Checked ? EtMarkDies(IsFileFailDie) : null);
        }

        private List<Point> EtMarkDies(Func<DataRow, bool> mark)
        {
            return _result == null ? new List<Point>() : Rows(_result, "DIE").Where(mark).Select(DiePointOf).ToList();
        }

        private static bool IsSpecOutDie(DataRow die)
        {
            return ColText(die, "ET_STATE") == "F";
        }

        private static bool IsFileFailDie(DataRow die)
        {
            return ColText(die, "ET_FILE_FLAG") == "F";
        }

        /// <summary>표식 체크를 바꾸면 서버를 다시 부르지 않고 같은 결과로 표식과 구간 표만 다시 그린다.</summary>
        private void chkEtMark_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            ApplyEtMarks();
            ApplyMapColors();
        }

        /// <summary>체크 앞 / 색 구간 표 칸에 쓰는 표식 그림(흰 Die 칸 + 그 모서리 삼각형).</summary>
        private static Bitmap MarkImage(bool specOut)
        {
            var bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
                DrawMarkBox(g, new Rectangle(1, 1, 13, 13), specOut);
            return bmp;
        }

        /// <summary>box 안에 Map 과 같은 모양의 표식을 그린다: Spec Out = 왼쪽 위 빨강, Fail Die = 오른쪽 아래 남색. 작은 칸이라 Map 비율보다 크게(Spec 80% / Fail 65%).</summary>
        private static void DrawMarkBox(Graphics g, Rectangle box, bool specOut)
        {
            g.FillRectangle(Brushes.White, box);
            if (specOut)
            {
                float m = Math.Max(5f, box.Height * 0.8f);
                using (var brush = new SolidBrush(GdsKlarfAnalysisMap.FailMarkColor))
                    g.FillPolygon(brush, new[] { new PointF(box.Left, box.Top), new PointF(box.Left + m, box.Top), new PointF(box.Left, box.Top + m) });
            }
            else
            {
                float m = Math.Max(4f, box.Height * 0.65f);
                using (var brush = new SolidBrush(GdsKlarfAnalysisMap.FileFailMarkColor))
                    g.FillPolygon(brush, new[] { new PointF(box.Right, box.Bottom), new PointF(box.Right - m, box.Bottom), new PointF(box.Right, box.Bottom - m) });
            }
            g.DrawRectangle(Pens.Gray, box);
        }

        /// <summary>E-Test 판정(P / F)이 있는 Die 인지. N(판정 값 없음) / 빈 값은 비교할 수 없다.</summary>
        private static bool EtJudged(DataRow die)
        {
            string s = ColText(die, "ET_STATE");
            return s == "P" || s == "F";
        }

        /// <summary>색 기준 숫자. ChainDefect = IN_CHAIN_COUNT, EtFailChain = ET_FAIL_CHAIN_COUNT, MatchChain = BOTH_CHAIN_COUNT.</summary>
        private int BasisValue(DataRow die)
        {
            switch (ColorBasis)
            {
                case GdsKlarfColorBasis.EtFailChain: return ColInt(die, "ET_FAIL_CHAIN_COUNT");
                case GdsKlarfColorBasis.MatchChain: return ColInt(die, "BOTH_CHAIN_COUNT");
                default: return ColInt(die, "IN_CHAIN_COUNT");
            }
        }

        private int MaxBasisValue()
        {
            if (_result == null) return 0;
            return Rows(_result, "DIE").Select(BasisValue).DefaultIfEmpty(0).Max();
        }

        /// <summary>
        /// Die 채움색 / 숫자를 Map 에 넣고 구간 표의 Die 수를 갱신한다.
        ///  ChainDefect : Defect 없음 흰색 / 기준 미만 연회색 / 구간 색 (기존)
        ///  EtFailChain / MatchChain : E-Test 판정 없는 Die 는 "비교 불가" 색, 값 0 은 흰색, 그 밖은 구간 색
        ///  ChainClass : Chain 탭에서 고른 Chain 의 4분류 색, 숫자 없음
        /// </summary>
        private void ApplyMapColors()
        {
            var fills = new Dictionary<Point, Color>();
            var labels = new Dictionary<Point, int>();
            if (_result != null)
            {
                GdsKlarfColorBasis basis = ColorBasis;
                foreach (DataRow row in Rows(_result, "DIE"))
                {
                    Point key = DiePointOf(row);
                    if (basis == GdsKlarfColorBasis.ChainClass)
                    {
                        fills[key] = GdsKlarfColorBands.ClassColor(ClassOf(row, _focusChainSeq));
                        continue;
                    }
                    if (basis == GdsKlarfColorBasis.ChainDefect)
                    {
                        int total = ColInt(row, "DEFECT_COUNT");
                        int inChain = ColInt(row, "IN_CHAIN_COUNT");
                        fills[key] = GdsKlarfColorBands.DieColor(total, inChain, Threshold, _bands);
                        if (total > 0) labels[key] = inChain;
                        continue;
                    }
                    if (!EtJudged(row))
                    {
                        fills[key] = GdsKlarfColorBands.NotComparableColor;
                        continue;
                    }
                    int value = BasisValue(row);
                    fills[key] = GdsKlarfColorBands.DieColor(value, value, Threshold, _bands);
                    if (value > 0) labels[key] = value;
                }
            }
            map.SetDieDisplay(fills, labels);
            RefreshBandGrid();
        }

        /// <summary>
        /// (Die, Chain) 4분류 코드(설계 7.1절). DIE_CHAIN.CLASS4 가 있으면 그 값,
        /// 행이 없으면(Defect 0 이고 E-Test P 인 칸은 서버가 보내지 않음) E-Test 판정 Die + 연결 Chain 이면 OK, 아니면 NA.
        /// </summary>
        private string ClassOf(DataRow die, string chainSeq)
        {
            if (chainSeq == null) return "NA";
            Point p = DiePointOf(die);
            DataRow dc = Rows(_result, "DIE_CHAIN").FirstOrDefault(r => ColText(r, "CHAIN_SEQ") == chainSeq && DiePointOf(r) == p);
            if (dc != null && ColText(dc, "CLASS4").Length > 0) return ColText(dc, "CLASS4");
            DataRow chain = FindChainRow(chainSeq);
            return EtJudged(die) && chain != null && ColText(chain, "ET_LINK_STATUS") == "LINKED" ? "OK" : "NA";
        }

        /// <summary>4분류 기본 Chain: 일치 Die 가 가장 많은 Chain, 없으면 Defect 가 가장 많은 Chain.</summary>
        private string DefaultFocusChain()
        {
            DataRow best = Rows(_result, "CHAIN").OrderByDescending(r => ColInt(r, "DIE_BOTH")).ThenByDescending(r => ColInt(r, "HIT_DEFECT_COUNT")).FirstOrDefault();
            return best == null ? null : ColText(best, "CHAIN_SEQ");
        }

        /// <summary>
        /// 구간 표를 다시 채운다. 고정 행(기준 미만 / 없음 / 비교 불가)은 고칠 수 없다. Die 수는 현재 분석 결과와 색 기준 기준이다.
        /// 4분류 기준이면 4분류 5행만 보인다.
        /// E-Test 판정이 있으면 색 기준과 상관없이 끝에 E-Test Fail 표식(Map 왼쪽 위 빨간 삼각형) 행을 더한다 (2026-10-16, 기준 라디오 2026-10-19).
        /// </summary>
        private void RefreshBandGrid()
        {
            dgvBand.Rows.Clear();
            FillBandRows();
            AddFailMarkRow();
        }

        private void FillBandRows()
        {
            GdsKlarfColorBasis basis = ColorBasis;
            lblBandTitle.Text = "색 구간 - " + cboColorBasis.Text + (basis == GdsKlarfColorBasis.ChainClass && _focusChainSeq != null ? " (" + ChainNameOf(_focusChainSeq) + ")" : "");
            lblMapBasis.Text = "색 = " + lblBandTitle.Text.Substring("색 구간 - ".Length);
            List<DataRow> dies = _result == null ? new List<DataRow>() : Rows(_result, "DIE").ToList();
            if (basis == GdsKlarfColorBasis.ChainClass)
            {
                foreach (string code in GdsKlarfColorBands.ClassCodes)
                    AddFixedBandRow(GdsKlarfColorBands.ClassText(code), GdsKlarfColorBands.ClassColor(code), dies.Count(d => ClassOf(d, _focusChainSeq) == code));
                return;
            }
            if (basis == GdsKlarfColorBasis.ChainDefect)
            {
                List<int[]> values = dies.Select(r => new[] { ColInt(r, "DEFECT_COUNT"), ColInt(r, "IN_CHAIN_COUNT") }).ToList();
                foreach (GdsKlarfColorBand band in _bands)
                    AddBandRow(band, values.Count(d => d[0] > 0 && d[1] >= Threshold && band.Contains(d[1])));
                AddFixedBandRow("기준 미만", GdsKlarfColorBands.BelowThresholdColor, values.Count(d => d[0] > 0 && d[1] < Threshold));
                AddFixedBandRow("Defect 없음", GdsKlarfColorBands.NoDefectColor, values.Count(d => d[0] == 0));
                return;
            }
            List<int> judged = dies.Where(EtJudged).Select(BasisValue).ToList();
            foreach (GdsKlarfColorBand band in _bands)
                AddBandRow(band, judged.Count(v => v >= Threshold && band.Contains(v)));
            AddFixedBandRow("기준 미만", GdsKlarfColorBands.BelowThresholdColor, judged.Count(v => v > 0 && v < Threshold));
            AddFixedBandRow(basis == GdsKlarfColorBasis.EtFailChain ? "Fail 없음" : "일치 없음", GdsKlarfColorBands.NoDefectColor, judged.Count(v => v == 0));
            AddFixedBandRow("비교 불가 (E-Test 판정 없음)", GdsKlarfColorBands.NotComparableColor, dies.Count(d => !EtJudged(d)));
        }

        private void AddBandRow(GdsKlarfColorBand band, int count)
        {
            int i = dgvBand.Rows.Add(band.From, band.To == GdsKlarfColorBands.OpenEnd ? "이상" : band.To.ToString(CultureInfo.InvariantCulture), "", count);
            PaintColorCell(dgvBand.Rows[i], band.Color);
        }

        private void AddFixedBandRow(string text, Color color, int count)
        {
            int i = dgvBand.Rows.Add(text, "", "", count);
            DataGridViewRow row = dgvBand.Rows[i];
            row.ReadOnly = true;
            row.Tag = "FIXED";
            row.DefaultCellStyle.ForeColor = Color.DimGray;
            PaintColorCell(row, color);
        }

        private void PaintColorCell(DataGridViewRow row, Color color)
        {
            DataGridViewCell cell = row.Cells[colBandColor.Index];
            cell.Style.BackColor = color;
            cell.Style.SelectionBackColor = color;
        }

        /// <summary>E-Test Fail Die 행 표시(Tag). 고정 행처럼 고칠 수 없다.</summary>
        private const string FailMarkRowTag = "FAIL_MARK";

        /// <summary>색 구간 표 E-Test 표식 행 이름 (2026-10-20).</summary>
        private const string SpecMarkRowText = "Spec Out Die";
        private const string FileMarkRowText = "Fail Die";

        /// <summary>
        /// E-Test 표식 행 (2026-10-16, 2026-10-20 두 행): 켠 표식마다 한 행, Map 표식과 같은 Die 를 센다.
        /// "Spec Out Die n"(왼쪽 위 빨강) / "Fail Die (결과 파일) n"(오른쪽 아래 남색). 두 행 모두 Tag = FAIL_MARK(고칠 수 없는 행).
        /// 표식은 채움색 위에 덧그리므로 한 Die 가 위 구간 행과 이 행에 함께 세어진다. E-Test Die 가 없으면 행을 넣지 않는다.
        /// </summary>
        private void AddFailMarkRow()
        {
            List<DataRow> dies = _result == null ? new List<DataRow>() : Rows(_result, "DIE").ToList();
            if (!dies.Any(d => EtJudged(d) || ColText(d, "ET_FILE_FLAG").Length > 0)) return;
            if (chkMarkSpec.Checked) AddMarkRow(SpecMarkRowText, dies.Count(IsSpecOutDie));
            if (chkMarkFile.Checked) AddMarkRow(FileMarkRowText, dies.Count(IsFileFailDie));
        }

        private void AddMarkRow(string text, int count)
        {
            int i = dgvBand.Rows.Add(text, "", "", count);
            DataGridViewRow row = dgvBand.Rows[i];
            row.ReadOnly = true;
            row.Tag = FailMarkRowTag;
            row.DefaultCellStyle.ForeColor = Color.DimGray;
            PaintColorCell(row, Color.White);
        }

        /// <summary>
        /// E-Test 표식 행의 색 칸은 Map 표식 모양으로 그린다: Spec Out = 흰 칸 왼쪽 위 빨간 삼각형, Fail Die = 오른쪽 아래 남색 삼각형.
        /// 칸 높이가 작아 Map 비율로는 잘 보이지 않으므로 삼각형을 크게 그린다(DrawMarkBox).
        /// </summary>
        private void dgvBand_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colBandColor.Index || !FailMarkRowTag.Equals(dgvBand.Rows[e.RowIndex].Tag)) return;
            e.PaintBackground(e.ClipBounds, false);
            Rectangle box = Rectangle.Inflate(e.CellBounds, -3, -3);
            if (box.Width < 4 || box.Height < 4) { e.Handled = true; return; }
            DrawMarkBox(e.Graphics, box, System.Convert.ToString(dgvBand.Rows[e.RowIndex].Cells[0].Value) != FileMarkRowText);
            e.Handled = true;
        }

        /// <summary>FROM / TO 를 고치면 사용자 구간이 된다. 숫자가 아니거나 FROM &gt; TO 면 되돌린다. "이상"(빈 값)은 끝까지(999).</summary>
        private void dgvBand_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvBand.Rows[e.RowIndex].Tag != null || e.RowIndex >= _bands.Count) return;
            DataGridViewRow row = dgvBand.Rows[e.RowIndex];
            int from, to;
            string toText = System.Convert.ToString(row.Cells[colBandTo.Index].Value).Trim();
            if (!int.TryParse(System.Convert.ToString(row.Cells[colBandFrom.Index].Value), out from) || from < 1)
            {
                BeginInvoke(new Action(RefreshBandGrid));
                return;
            }
            if (toText.Length == 0 || toText == "이상") to = GdsKlarfColorBands.OpenEnd;
            else if (!int.TryParse(toText, out to) || to < from)
            {
                BeginInvoke(new Action(RefreshBandGrid));
                return;
            }
            _bands[e.RowIndex].From = from;
            _bands[e.RowIndex].To = to;
            SetUserBands();
        }

        /// <summary>색 칸을 더블클릭하면 색을 고른다(기존 Defect Count Color 와 같은 방식).</summary>
        private void dgvBand_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colBandColor.Index || dgvBand.Rows[e.RowIndex].Tag != null || e.RowIndex >= _bands.Count) return;
            colorDialog.Color = _bands[e.RowIndex].Color;
            if (colorDialog.ShowDialog(this) != DialogResult.OK) return;
            _bands[e.RowIndex].Color = colorDialog.Color;
            SetUserBands();
        }

        /// <summary>
        /// 사용자 구간으로 바꾼다(자동 해제, Wafer 를 바꿔도 유지).
        /// 구간 표 이벤트(CellEndEdit / CellDoubleClick) 안에서 불리므로 표를 바로 다시 만들지 않는다.
        /// DataGridView 가 현재 칸을 옮기는 도중(SetCurrentCellAddressCore)에 Rows.Clear 를 하면 재진입 예외가 나서(2026-10-12 실행 중 발견)
        /// 이벤트가 끝난 뒤(BeginInvoke) Map 색과 구간 표를 다시 그린다.
        /// </summary>
        private void SetUserBands()
        {
            _loading = true;
            chkAutoBand.Checked = false;
            _loading = false;
            BeginInvoke(new Action(ApplyMapColors));
        }

        private void chkAutoBand_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading || !chkAutoBand.Checked) return;
            _bands = GdsKlarfColorBands.Auto(Threshold, MaxBasisValue());
            ApplyMapColors();
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxBasisValue());
            else if (_bands.Count > 0) _bands[0].From = Math.Min(Threshold, _bands[0].To);
            ApplyMapColors();
        }

        #endregion

        #region [목록 / Die 선택]

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.AutoGenerateColumns = true;
        }

        /// <summary>
        /// Die 탭: Defect 또는 E-Test Fail 이 있는 Die 만, 서버 정렬(Chain In 내림차순) 그대로.
        /// Die 는 X / Y 두 열로 보이고(선택 / 상세 열기도 이 두 열을 읽는다), 주요 Chain 열은 두지 않는다 (2026-10-16).
        /// Defect 전체 = 그 Die 의 Defect 수(분류 / 큰 Defect 거르기 뒤) = Chain In + Chain Out.
        /// </summary>
        private void BindDieGrid()
        {
            var table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("Defect 전체", typeof(int));
            table.Columns.Add("Chain In", typeof(int));
            table.Columns.Add("Chain Out", typeof(int));
            table.Columns.Add("Chain 수", typeof(int));
            table.Columns.Add("ET 판정", typeof(string));
            table.Columns.Add("Fail Chain", typeof(string));
            table.Columns.Add("일치 Chain", typeof(string));
            table.Columns.Add("검사", typeof(string));
            foreach (DataRow r in Rows(_result, "DIE"))
                if (ColInt(r, "DEFECT_COUNT") > 0 || ColText(r, "ET_STATE") == "F")
                    table.Rows.Add(r["XINDEX"], r["YINDEX"], r["DEFECT_COUNT"], r["IN_CHAIN_COUNT"], r["OUT_CHAIN_COUNT"],
                        r["HIT_CHAIN_COUNT"], ColText(r, "ET_STATE"), ColText(r, "ET_FAIL_CHAIN_COUNT"), ColText(r, "BOTH_CHAIN_COUNT"),
                        r["IN_PLAN"]);
            dgvDie.DataSource = table;
            foreach (DataGridViewRow row in dgvDie.Rows)
                if (System.Convert.ToString(row.Cells["ET 판정"].Value) == "F") row.Cells["ET 판정"].Style.ForeColor = Color.Firebrick;
            SyncDieGridSelection();
        }

        /// <summary>선택 Die 기준으로 Chain / Defect / Image 탭을 다시 채운다.</summary>
        private void RefreshLists()
        {
            BindChainGrid();
            BindDefectGrid();
            RefreshImages();
        }

        private void ListFilter_Changed(object sender, EventArgs e)
        {
            if (_loading) return;
            BindChainGrid();
            BindDefectGrid();
        }

        /// <summary>
        /// Chain 탭: [Wafer 전체] 또는 선택 Die 가 없으면 Chain 별 합계(+ E-Test 연결 / Defect · Fail · 일치 Die / Fail률 2개, 설계 7.2절),
        /// 아니면 선택 Die 의 (Die, Chain). 행을 누르면 4분류 기준 Chain 이 된다.
        /// </summary>
        private void BindChainGrid()
        {
            // 첫 열 Chain = Chain 이름(코드는 쓰지 않는다, 2026-10-16), 숫자 열은 그 뒤
            var table = new DataTable();
            bool waferMode = chkChainAll.Checked || !_selectedDie.HasValue;
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("Defect 수", typeof(int));
            if (waferMode)
            {
                table.Columns.Add("Die 수", typeof(int));
                table.Columns.Add("E-Test 연결", typeof(string));
                table.Columns.Add("Defect Die", typeof(string));
                table.Columns.Add("Fail Die", typeof(string));
                table.Columns.Add("일치 Die", typeof(string));
                table.Columns.Add("Defect 있는 Fail률", typeof(string));
                table.Columns.Add("Defect 없는 Fail률", typeof(string));
                table.Columns.Add("Para", typeof(string));
            }
            else
            {
                table.Columns.Add("분류", typeof(string));
                table.Columns.Add("ET", typeof(string));
                table.Columns.Add("가까운 거리", typeof(string));
                table.Columns.Add("Fail Para", typeof(string));
                table.Columns.Add("Defect ID", typeof(string));
            }
            table.Columns.Add("Member", typeof(int));
            table.Columns.Add("SEQ", typeof(string));
            if (_result != null)
            {
                Dictionary<string, DataRow> chains = Rows(_result, "CHAIN").ToDictionary(r => r["CHAIN_SEQ"].ToString());
                if (waferMode)
                {
                    foreach (DataRow c in chains.Values.OrderByDescending(r => ColInt(r, "DIE_BOTH")).ThenByDescending(r => ColInt(r, "HIT_DEFECT_COUNT")))
                        table.Rows.Add(c["CHAIN_NAME"], c["HIT_DEFECT_COUNT"], c["HIT_DIE_COUNT"], LinkText(ColText(c, "ET_LINK_STATUS")),
                            ColText(c, "DIE_DEFECT"), ColText(c, "DIE_FAIL"), ColText(c, "DIE_BOTH"),
                            RateText(c, "FAIL_RATE_DEFECT"), RateText(c, "FAIL_RATE_CLEAN"), ColText(c, "ET_PARA_COUNT"), c["MEMBER_COUNT"], c["CHAIN_SEQ"]);
                }
                else
                {
                    foreach (DataRow dc in DieRows(Table(_result, "DIE_CHAIN")))
                    {
                        DataRow c;
                        if (!chains.TryGetValue(dc["CHAIN_SEQ"].ToString(), out c)) continue;
                        string failPara = ColText(dc, "ET_FAIL_COUNT").Length == 0 ? "" : ColText(dc, "ET_FAIL_COUNT") + " / " + ColText(dc, "ET_PARA_COUNT");
                        table.Rows.Add(c["CHAIN_NAME"], dc["DEFECT_COUNT"], ColText(dc, "CLASS4").Length == 0 ? "" : GdsKlarfColorBands.ClassText(ColText(dc, "CLASS4")),
                            ColText(dc, "ET_STATUS"), GapText(ColText(dc, "MIN_GAP")), failPara, ColText(dc, "DEFECT_IDS"),
                            c["MEMBER_COUNT"], c["CHAIN_SEQ"]);
                    }
                }
            }
            dgvChain.DataSource = table;
            if (dgvChain.Columns.Contains("SEQ")) dgvChain.Columns["SEQ"].Visible = false;
            foreach (DataGridViewRow row in dgvChain.Rows)
                if (System.Convert.ToString(row.Cells["SEQ"].Value) == _focusChainSeq) row.DefaultCellStyle.BackColor = Color.FromArgb(232, 240, 252);
        }

        /// <summary>Chain 행을 누르면 그 Chain 을 4분류 기준으로 삼는다(색 기준이 4분류면 바로 다시 칠한다).</summary>
        private void dgvChain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvChain.Columns.Contains("SEQ")) return;
            _focusChainSeq = System.Convert.ToString(dgvChain.Rows[e.RowIndex].Cells["SEQ"].Value);
            foreach (DataGridViewRow row in dgvChain.Rows)
                row.DefaultCellStyle.BackColor = System.Convert.ToString(row.Cells["SEQ"].Value) == _focusChainSeq ? Color.FromArgb(232, 240, 252) : Color.Empty;
            if (ColorBasis == GdsKlarfColorBasis.ChainClass) ApplyMapColors();
        }

        /// <summary>
        /// Defect 탭: [Chain In] / [Chain Out] / [Defect 전체], [Wafer 전체] 가 아니면 선택 Die 만. 걸린 Chain 은 이름으로 보인다(2026-10-16).
        /// 통합 분석 서버는 Chain 영역 + Chip 밖 Defect 만 보낸다(설계 9.2절). 나머지 Defect 는 Map 에 그려져 있다.
        /// </summary>
        private void BindDefectGrid()
        {
            var table = new DataTable();
            table.Columns.Add("Defect ID", typeof(long));
            table.Columns.Add("Step", typeof(string));
            table.Columns.Add("Die", typeof(string));
            table.Columns.Add("RoughBin", typeof(string));
            table.Columns.Add("Class", typeof(int));
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("Gap", typeof(string));
            table.Columns.Add("XSIZE", typeof(double));
            table.Columns.Add("YSIZE", typeof(double));
            table.Columns.Add("큰 Defect", typeof(string));
            table.Columns.Add("XREL", typeof(double));
            table.Columns.Add("YREL", typeof(double));
            table.Columns.Add("GDS X", typeof(double));
            table.Columns.Add("GDS Y", typeof(double));
            if (_result != null)
            {
                IEnumerable<DataRow> rows = chkDefectAll.Checked || !_selectedDie.HasValue
                    ? Rows(_result, "DEFECT")
                    : DieRows(Table(_result, "DEFECT"));
                foreach (DataRow r in rows)
                {
                    bool inChain = r["IN_CHAIN"].ToString() == "Y";
                    if ((cboDefectScope.SelectedIndex == 0 && !inChain) || (cboDefectScope.SelectedIndex == 1 && inChain)) continue;
                    table.Rows.Add(r["DEFECTID"], ColText(r, "STEP_ID"), DieText(r["XINDEX"], r["YINDEX"]), ColText(r, "ROUGHBINNUMBER"), r["CLASSNUMBER"],
                        inChain ? ChainNamesOf(ColText(r, "CHAIN_CODES")) : "(밖)", GapText(ColText(r, "GAP")), r["XSIZE"], r["YSIZE"], ColText(r, "LARGE") == "Y" ? "Y" : "",
                        r["XREL"], r["YREL"], r["GDS_X"], r["GDS_Y"]);
                }
            }
            dgvDefect.DataSource = table;
            tabDefect.Text = "Defect (" + table.Rows.Count + ")";
        }

        /// <summary>허용오차 / E-Test 연결 / E-Test Fail 탭 (2026-10-16, 설계 5.5 / 6.5 / 15.3절).</summary>
        private void BindResultTabs()
        {
            BindToleranceGrid();
            BindEtLinkGrid();
            BindEtFailGrid();
        }

        /// <summary>허용오차 탭: 한 번 분석으로 단계별(0 / 2 / 5 / 10 / 20 / 50 µm) 센 Chain In Defect 수와 걸린 Die 수. 지금 허용오차 행은 굵게.</summary>
        private void BindToleranceGrid()
        {
            var table = new DataTable();
            table.Columns.Add("허용오차", typeof(string));
            table.Columns.Add("Chain In Defect", typeof(string));
            table.Columns.Add("걸린 Die", typeof(string));
            foreach (DataRow r in Rows(_result, "TOL_CURVE"))
                table.Rows.Add(ColText(r, "TOL_UM"), ColText(r, "IN_CHAIN_DEFECT"), ColText(r, "HIT_DIE"));
            dgvTolerance.DataSource = table;
            string current = ((double)numTolerance.Value).ToString(CultureInfo.InvariantCulture);
            foreach (DataGridViewRow row in dgvTolerance.Rows)
                if (System.Convert.ToString(row.Cells[0].Value) == current) row.DefaultCellStyle.Font = new Font(dgvTolerance.Font, FontStyle.Bold);
        }

        /// <summary>E-Test 연결 탭: E-Test GDS Chain 이름 -> 분석 Device Chain 대응 상태(설계 6.5절).</summary>
        private void BindEtLinkGrid()
        {
            var table = new DataTable();
            table.Columns.Add("E-Test Chain 이름", typeof(string));
            table.Columns.Add("연결 Chain", typeof(string));
            table.Columns.Add("상태", typeof(string));
            table.Columns.Add("Parameter", typeof(string));
            table.Columns.Add("2 Chain 항목", typeof(string));
            // 이름은 E-Test 등록 원문(GDS_CHAIN_NAME), 없으면 비교 키(대문자)
            foreach (DataRow r in Rows(_result, "ET_LINK"))
                table.Rows.Add(ColText(r, "GDS_CHAIN_NAME").Length > 0 ? ColText(r, "GDS_CHAIN_NAME") : ColText(r, "GDS_CHAIN_KEY"),
                    ColText(r, "CHAIN_SEQ").Length == 0 ? "" : ChainNameOf(ColText(r, "CHAIN_SEQ")),
                    LinkText(ColText(r, "STATUS")), ColText(r, "PARA_COUNT"), ColText(r, "CHAIN2_PARA_COUNT"));
            dgvEtLink.DataSource = table;
            foreach (DataGridViewRow row in dgvEtLink.Rows)
                if (System.Convert.ToString(row.Cells["상태"].Value) != "연결") row.Cells["상태"].Style.ForeColor = Color.DarkOrange;
        }

        /// <summary>
        /// E-Test Fail 탭(역추적 목록, 설계 15.3절): E-Test 상태 F 인 (Die, Chain).
        /// 정렬은 일치 먼저, 그 안에서 가장 가까운 Defect 거리 순. 행을 누르면 Die 선택, 더블클릭하면 그 Chain 기준으로 상세 화면을 연다.
        /// </summary>
        private void BindEtFailGrid()
        {
            // Die / Chain(이름, 코드는 쓰지 않는다 2026-10-16) 다음에 판단에 쓰는 열(분류 / 거리 / 걸린 수)
            var table = new DataTable();
            table.Columns.Add("Die", typeof(string));
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("분류", typeof(string));
            table.Columns.Add("가까운 Defect", typeof(string));
            table.Columns.Add("걸린 Defect", typeof(string));
            table.Columns.Add("Fail Para", typeof(string));
            table.Columns.Add("Fail Parameter", typeof(string));
            table.Columns.Add("값", typeof(string));
            table.Columns.Add("Step", typeof(string));
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("SEQ", typeof(string));
            Dictionary<string, DataRow> chains = Rows(_result, "CHAIN").ToDictionary(r => r["CHAIN_SEQ"].ToString());
            IEnumerable<DataRow> fails = Rows(_result, "DIE_CHAIN").Where(r => ColText(r, "ET_STATUS") == "F")
                .OrderBy(r => ColText(r, "CLASS4") == "BOTH" ? 0 : 1).ThenBy(r => GapSortKey(ColText(r, "MIN_GAP")));
            foreach (DataRow r in fails)
            {
                DataRow c;
                chains.TryGetValue(r["CHAIN_SEQ"].ToString(), out c);
                table.Rows.Add(DieText(r["XINDEX"], r["YINDEX"]), c == null ? "" : ColText(c, "CHAIN_NAME"), GdsKlarfColorBands.ClassText(ColText(r, "CLASS4")),
                    GapText(ColText(r, "MIN_GAP")), ColText(r, "DEFECT_COUNT"), ColText(r, "ET_FAIL_COUNT") + " / " + ColText(r, "ET_PARA_COUNT"),
                    ColText(r, "ET_FIRST_FAIL_ITEM"), ColText(r, "ET_FIRST_FAIL_VALUE"), ColText(r, "NEAR_STEP_ID"),
                    ColInt(r, "XINDEX"), ColInt(r, "YINDEX"), ColText(r, "CHAIN_SEQ"));
            }
            dgvEtFail.DataSource = table;
            foreach (string hidden in new[] { "X", "Y", "SEQ" })
                if (dgvEtFail.Columns.Contains(hidden)) dgvEtFail.Columns[hidden].Visible = false;
            foreach (DataGridViewRow row in dgvEtFail.Rows)
                row.Cells["분류"].Style.ForeColor = System.Convert.ToString(row.Cells["분류"].Value) == "일치" ? Color.Firebrick : Color.DarkOrange;
            tabEtFail.Text = "E-Test Fail (" + table.Rows.Count + ")";
        }

        private void dgvEtFail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvEtFail.Rows[e.RowIndex];
            _focusChainSeq = System.Convert.ToString(row.Cells["SEQ"].Value);
            SelectDie(System.Convert.ToInt32(row.Cells["X"].Value), System.Convert.ToInt32(row.Cells["Y"].Value));
            if (ColorBasis == GdsKlarfColorBasis.ChainClass) ApplyMapColors();
        }

        /// <summary>E-Test Fail 행 더블클릭: 그 Die 상세 화면을 그 Chain 역추적으로 연다(설계 15.3절).</summary>
        private void dgvEtFail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvEtFail.Rows[e.RowIndex];
            OpenDetail(System.Convert.ToInt32(row.Cells["X"].Value), System.Convert.ToInt32(row.Cells["Y"].Value), System.Convert.ToString(row.Cells["SEQ"].Value));
        }

        /// <summary>
        /// Image 탭: 선택 Die(없으면 비움)의 Defect 이미지를 이미지 카드(DPUCDefectImageCard, 폭 220)로 보여 준다.
        /// FTP 다운로드는 카드 대기열이 한다. 더블클릭하면 큰 이미지 창 제목 줄에 REL / Cluster 까지 나온다. (2026-10-21 카드 교체)
        /// </summary>
        private void RefreshImages()
        {
            if (!ImageTabShown) return;
            while (flowImage.Controls.Count > 0) flowImage.Controls[0].Dispose();
            if (!_selectedDie.HasValue || _imagePaths == null) return;
            var defects = map.Defects.Where(d => d.XINDEX == _selectedDie.Value.X && d.YINDEX == _selectedDie.Value.Y).ToDictionary(d => (long)d.DEFECTID);
            ShowImages(defects);
        }

        private void ShowImages(Dictionary<long, Defect> defects)
        {
            if (_imagePaths == null) return;
            foreach (DataRow row in _imagePaths.Rows)
            {
                Defect d;
                if (!defects.TryGetValue(System.Convert.ToInt64(row["DEFECTID"]), out d)) continue;
                string path = String.Format("{0}/{1}", row["IMAGE_PATH"].ToString().Trim(), row["IMAGE_FILENAME"].ToString().Trim());
                try
                {
                    flowImage.Controls.Add(new DACrux.SEMDMS.Control.DPUCDefectImageCard(d, path));
                }
                catch (Exception ex)
                {
                    SetStatus("이미지 표시 실패: " + ex.Message);
                }
            }
        }

        private void tabList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshImages();
        }

        /// <summary>Defect 행 더블클릭: 그 Defect 이미지만 Image 탭에 보여 준다.</summary>
        private void dgvDefect_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long id = System.Convert.ToInt64(dgvDefect.Rows[e.RowIndex].Cells["Defect ID"].Value);
            ShowResultTab(tabImage);
            while (flowImage.Controls.Count > 0) flowImage.Controls[0].Dispose();
            ShowImages(map.Defects.Where(d => d.DEFECTID == id).ToDictionary(d => (long)d.DEFECTID));
            if (flowImage.Controls.Count == 0) SetStatus("Defect " + id + " 이미지 없음");
        }

        private IEnumerable<DataRow> DieRows(DataTable table)
        {
            if (table == null) return Enumerable.Empty<DataRow>();
            Point die = _selectedDie.Value;
            return table.Rows.Cast<DataRow>().Where(r => System.Convert.ToInt32(r["XINDEX"]) == die.X && System.Convert.ToInt32(r["YINDEX"]) == die.Y);
        }

        private DataRow FindDieRow(Point die)
        {
            return Rows(_result, "DIE").FirstOrDefault(r => DiePointOf(r) == die);
        }

        private DataRow FindChainRow(string chainSeq)
        {
            DataRow row;
            return chainSeq != null && ChainIndex().BySeq.TryGetValue(chainSeq, out row) ? row : null;
        }

        /// <summary>
        /// 결과 CHAIN 표의 찾기 사전(Chain 번호 -> 행, Chain 코드 -> 이름). 결과(_result)가 바뀌면 다시 만든다 (2026-10-21).
        /// U8 처럼 Chain 이 2만 7천 개면 행마다 CHAIN 표를 훑던 방식이 Defect 탭 199행에 2.4초 걸렸다.
        /// 근거: 문서/설계/05_연관분석/2026-10-21_U8_연관분석_영향_확인.md A3
        /// </summary>
        private ChainLookup ChainIndex()
        {
            if (_chainIndex == null || !ReferenceEquals(_chainIndex.Source, _result)) _chainIndex = new ChainLookup(_result, Rows(_result, "CHAIN"));
            return _chainIndex;
        }

        private ChainLookup _chainIndex;

        private sealed class ChainLookup
        {
            public readonly DataSet Source;
            public readonly Dictionary<string, DataRow> BySeq = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            public readonly Dictionary<string, string> NameByCode = new Dictionary<string, string>(StringComparer.Ordinal);

            public ChainLookup(DataSet source, IEnumerable<DataRow> chains)
            {
                Source = source;
                foreach (DataRow r in chains)
                {
                    string seq = ColText(r, "CHAIN_SEQ");
                    if (!BySeq.ContainsKey(seq)) BySeq.Add(seq, r);
                    string code = ColText(r, "CHAIN_CODE");
                    if (!NameByCode.ContainsKey(code)) NameByCode.Add(code, ColText(r, "CHAIN_NAME"));
                }
            }
        }

        /// <summary>Chain 번호 -> 이름. 화면에는 Chain 코드(A, B, AA ..)를 쓰지 않는다 (2026-10-16).</summary>
        private string ChainNameOf(string chainSeq)
        {
            DataRow c = FindChainRow(chainSeq);
            return c == null ? chainSeq : ColText(c, "CHAIN_NAME");
        }

        /// <summary>
        /// 서버 DEFECT.CHAIN_CODES("A,B") -> Chain 이름 목록("이름1, 이름2"). 같은 결과의 CHAIN 표로 바꾼다(코드는 Device 안에서 한 Chain 에 하나).
        /// 서버는 그대로 두고 표시만 바꾸려는 것이다 (2026-10-16).
        /// </summary>
        private string ChainNamesOf(string codes)
        {
            if (String.IsNullOrEmpty(codes)) return "";
            Dictionary<string, string> names = ChainIndex().NameByCode;
            return String.Join(", ", codes.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0)
                .Select(s => { string n; return names.TryGetValue(s, out n) && n.Length > 0 ? n : s; }).ToArray());
        }

        /// <summary>Die 를 고른다: Map 빨간 테두리 / Die 탭 행 / 다른 탭 거르기 / 상태줄. 표를 다시 묶는 동안 모래시계 커서 (2026-10-15).</summary>
        public void SelectDie(int x, int y)
        {
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                _selectedDie = new Point(x, y);
                map.SetSelectedDie(x, y);
                map.Redraw();
                SyncDieGridSelection();
                RefreshLists();
                UpdateStatus();
            }
        }

        private void SyncDieGridSelection()
        {
            dgvDie.ClearSelection();
            if (!_selectedDie.HasValue || !dgvDie.Columns.Contains("X")) return;
            foreach (DataGridViewRow row in dgvDie.Rows)
                if (System.Convert.ToInt32(row.Cells["X"].Value) == _selectedDie.Value.X && System.Convert.ToInt32(row.Cells["Y"].Value) == _selectedDie.Value.Y)
                {
                    row.Selected = true;
                    dgvDie.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
        }

        private void map_DieClicked(object sender, GdsKlarfDieEventArgs e)
        {
            SelectDie(e.XIndex, e.YIndex);
        }

        private void map_DieDoubleClicked(object sender, GdsKlarfDieEventArgs e)
        {
            SelectDie(e.XIndex, e.YIndex);
            OpenDetail(e.XIndex, e.YIndex);
        }

        private void dgvDie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvDie.Rows[e.RowIndex];
            SelectDie(System.Convert.ToInt32(row.Cells["X"].Value), System.Convert.ToInt32(row.Cells["Y"].Value));
        }

        private void dgvDie_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvDie.Rows[e.RowIndex];
            OpenDetail(System.Convert.ToInt32(row.Cells["X"].Value), System.Convert.ToInt32(row.Cells["Y"].Value));
        }

        /// <summary>
        /// 우클릭 메뉴 [Detail Analysis]: 우클릭으로 선택한 Chip 의 상세 화면을 연다.
        /// 2026-10-12 요청으로 조건 줄 [상세 보기] 버튼을 없애고 "Chip 선택 후 상세 이동" 방식으로 바꿨다(설계 14.2절).
        /// </summary>
        private void map_DetailAnalysisRequested(object sender, GdsKlarfDieEventArgs e)
        {
            SelectDie(e.XIndex, e.YIndex);
            OpenDetail(e.XIndex, e.YIndex);
        }

        private void OpenDetail(int x, int y)
        {
            OpenDetail(x, y, null);
        }

        /// <summary>
        /// 선택 Chip 의 상세 화면(화면 B)을 연다(설계 7.3 / 14.1절). 탭 제목 = "상세보기 (X, Y)". focusChainSeq 가 있으면 그 Chain 역추적으로 연다(설계 15.3절).
        /// 상세보기 창은 하나다 (2026-10-22 요청 "상세 화면이 열려 있는 상태에서 분석화면에서 다른 die 를 더블클릭하면 열려 있는 화면 그대로 조회",
        /// 문서/설계/05_연관분석/2026-10-22_Die상세_DLL로딩_Die목록_설계.md 5절):
        ///  - 창이 없으면 새로 만들고, 있으면 그 창 왼쪽 Die 목록에서 그 Die 를 고른다. 목록은 이 화면의 마지막 분석 결과다.
        ///  - 상세보기 창이 마지막 분석 요청으로 그 Die 만 다시 계산한다(AnalyzeDie, 결과를 직접 받아 요청이 없으면 Wafer 결과 그대로).
        /// DACrux ShowForm 은 ENGUI DLL 을 Assembly.Load(byte[]) 로 새로 읽어 다른 어셈블리 사본의 화면을 만든다. 그 화면은 형식으로 찾을 수 없어
        /// 상세 화면이 두 개 생겼으므로(2026-10-12 확인 -> 버그) ShowForm 을 쓰지 않고 상세보기 창 Open 으로 직접 만든다.
        /// </summary>
        private void OpenDetail(int x, int y, string focusChainSeq)
        {
            if (_result == null)
            {
                MessageBox.Show(this, "먼저 [분석] 하세요.", "Detail Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dirty) SetStatus("조건 변경됨: 상세 화면은 마지막 분석 조건");
            // 상세 화면 만들기(GDS DLL 읽기 포함) / 그 Die 계산 동안 모래시계 커서 (2026-10-15)
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                try { frmGdsDieAnalysis.Open(this, _analysisVersion, _result, _lastRequest, AnalysisService, x, y, focusChainSeq); }
                catch (Exception ex)
                {
                    SetStatus("Die 상세 분석 실패: " + ex.Message);
                    if (_showErrorDialogs) MessageBox.Show(this, ex.Message, "Detail Analysis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        #region [상태줄 / 조건 변경]

        private void Condition_Changed(object sender, EventArgs e)
        {
            if (_loading) return;
            if (_result != null)
            {
                _dirty = true;
                UpdateStatus();
            }
        }

        /// <summary>Defect 영역 라디오: 고른 쪽 입력칸만 쓸 수 있게 하고 조건 변경으로 표시한다.</summary>
        private void DefectArea_CheckedChanged(object sender, EventArgs e)
        {
            numTolerance.Enabled = rdoDefectSize.Checked;
            numSquare.Enabled = rdoSquare.Checked;
            Condition_Changed(sender, e);
        }

        private void chkDieIndex_CheckedChanged(object sender, EventArgs e)
        {
            map.ShowDieIndexLabel = chkDieIndex.Checked;
            map.Redraw();
        }

        /// <summary>마우스가 다른 Die 로 가면 그 Die 의 분석 숫자를 상태줄 둘째 줄에 보여 준다(Map 에 툴팁 기능이 없어 기존 화면과 같이 상태줄 사용).</summary>
        private void map_OnChangeCurrentDie(object sender, Die die)
        {
            // DACrux.Base.Die 는 == 연산자를 Equals 로 재정의했고 Equals 가 null 을 처리하지 않는다(die == null 이면 NullReferenceException).
            // 그래서 연산자를 거치지 않는 ReferenceEquals 로 확인한다. (2026-10-12 실행 중 발견)
            if (ReferenceEquals(die, null) || _result == null) return;
            UpdateStatus(die.IndexX, die.IndexY);
        }

        private void UpdateStatus()
        {
            if (_selectedDie.HasValue) UpdateStatus(_selectedDie.Value.X, _selectedDie.Value.Y);
            else UpdateStatus(int.MinValue, int.MinValue);
        }

        /// <summary>
        /// 첫 줄 = 분석 기준 핵심 값(설계 7.2 ANL_INFO + 통합 설계 10.2: Wafer Key / Step 수 / E-Test 파일 / Spec 바뀜 / Pitch 출처 / Cache),
        /// 둘째 줄 = 고른 Die 정보. 사용법 안내 문구는 두지 않는다 (2026-10-15).
        /// </summary>
        private void UpdateStatus(int x, int y)
        {
            if (_result == null) return;
            DataRow i = _result.Tables["ANL_INFO"].Rows[0];
            string key = ColText(i, "WAFER_KEY");
            string steps = ColText(i, "STEP_SEQS");
            string et = ColText(i, "ET_WAFER_ID").Length == 0 ? "" : String.Format(" / E-Test {0} {1}{2}", ColText(i, "ET_WAFER_ID"), ColText(i, "ET_TEST_PROGRAM"),
                ColText(i, "ET_SPEC_CHANGED") == "Y" ? " (Spec 바뀜)" : "");
            // 면: 통합 분석 서버는 CHAIN_SIDE / STEP_SIDES / MIRROR(Map Step) 를 돌려준다. 없으면(결과를 직접 받은 경우) 요청 때 만든 문구.
            // 검사 면(KLARF FS 고정)은 보이지 않고 Chain 면 + 반전만 보인다 (2026-10-20)
            string sides = ColText(i, "CHAIN_SIDE").Length > 0
                ? String.Format("Chain {0}{1}", SideCode(ColText(i, "CHAIN_SIDE")), ColText(i, "MIRROR") == "Y" ? " -> 반전" : "")
                : _sideText;
            string line1 = String.Format("{0}{1} / GDS {2} / {3} 보기{4} / {5} / Chain {6} / Defect {7} 중 Chain In {8}{9}{10}{11}",
                key.Length == 0 ? i["WAFER_ID"] : "Wafer Key " + key, steps.Length == 0 ? "" : " (Step " + steps.Split(',').Length + "개)",
                i["DEVICE_ID"], i["VIEW_SIDE"], sides.Length == 0 ? "" : " (" + sides + ")", AreaText(i),
                i["CHAIN_COUNT"], i["DEFECT_COUNT"], i["IN_CHAIN_DEFECT_COUNT"], et,
                ColText(i, "PITCH_SOURCE") == "MAP_SETUP" ? " / Pitch = Map Setup" : "",
                _dirty ? " (조건 변경됨)" : "");
            string line2 = string.Empty;
            if (x != int.MinValue)
            {
                DataRow die = FindDieRow(new Point(x, y));
                line2 = die == null
                    ? String.Format("Die ({0}, {1}) 검사 계획 밖", x, y)
                    // 주요 Chain(코드 목록, 서버 TOP_CHAINS)은 보이지 않고 걸린 Chain 수만 (2026-10-16)
                    : String.Format("Die ({0}, {1}) Defect 전체 {2} / Chain In {3} / Chain Out {4} / 걸린 Chain {5}{6}", x, y, die["DEFECT_COUNT"], die["IN_CHAIN_COUNT"],
                        die["OUT_CHAIN_COUNT"], die["HIT_CHAIN_COUNT"],
                        EtStatusText(die));
            }
            lblStatus.Text = line1 + Environment.NewLine + line2;
        }

        /// <summary>
        /// 상태줄 Die 줄의 E-Test 부분(2026-10-20, Map 표식 이름과 같게):
        /// " / E-Test Spec Out (Fail Chain n, 일치 n) · 결과 파일 Fail Die". Spec 판정 P = "Spec 통과", 결과 파일 P = "Pass". E-Test Die 가 없으면 빈 값.
        /// </summary>
        private static string EtStatusText(DataRow die)
        {
            string spec = ColText(die, "ET_STATE");
            string file = ColText(die, "ET_FILE_FLAG");
            if (spec.Length == 0 && file.Length == 0) return "";
            string specText = spec == "F" ? "Spec Out" : spec == "P" ? "Spec 통과" : spec.Length == 0 ? "-" : spec;
            string fileText = file.Length == 0 ? "" : " · 결과 파일 " + (file == "F" ? "Fail Die" : file == "P" ? "Pass" : file);
            return String.Format(" / E-Test {0} (Fail Chain {1}, 일치 {2}){3}", specText, ColText(die, "ET_FAIL_CHAIN_COUNT"), ColText(die, "BOTH_CHAIN_COUNT"), fileText);
        }

        /// <summary>Defect 영역 문구: 크기 + 허용오차 n µm / 중심 사각형 한 변 n µm (ANL_INFO.USE_DEFECT_SIZE / TOLERANCE_UM).</summary>
        private static string AreaText(DataRow info)
        {
            double tol;
            double.TryParse(ColText(info, "TOLERANCE_UM"), NumberStyles.Float, CultureInfo.InvariantCulture, out tol);
            return ColText(info, "USE_DEFECT_SIZE") == "Y"
                ? "크기 + 허용오차 " + tol.ToString("0.#", CultureInfo.InvariantCulture)
                : "중심 사각형 한 변 " + (tol * 2).ToString("0.#", CultureInfo.InvariantCulture);
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void ClearResultViews()
        {
            _loading = true;
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, 1);
            _loading = false;
            map.SetEtMarks(null, null);
            RefreshBandGrid();
            BindDieGrid();
            RefreshLists();
            BindResultTabs();
        }

        #endregion

        #region [화면 배치: 기능 탭 / 색 구간 팝업]

        // 기능 색 (2026-10-19, 설계 문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md 3절): 탭 머리 막대 / 기준 제목 / 요약 칸
        private static readonly Color GdsTabColor = Color.FromArgb(59, 111, 196);
        private static readonly Color KlarfTabColor = Color.FromArgb(63, 155, 75);
        private static readonly Color EtTabColor = Color.FromArgb(224, 123, 0);
        private static readonly Color SummaryTabColor = Color.FromArgb(122, 79, 176);

        /// <summary>
        /// 결과 탭을 연다: 그 탭이 들어 있는 기능 탭을 고른 뒤 하위 탭을 고른다 (2026-10-19).
        /// 예) Image -> [KLARF] + [Image], E-Test Fail -> [E-Test] + [E-Test Fail]. 시안 / Smoke 도 쓴다.
        /// </summary>
        public void ShowResultTab(TabPage page)
        {
            var host = page == null ? null : page.Parent as TabControl;
            if (host == null) return;
            if (host != tabList)
            {
                var owner = host.Parent as TabPage;
                if (owner != null) tabList.SelectedTab = owner;
            }
            host.SelectedTab = page;
        }

        /// <summary>Image 탭이 보이는지([KLARF] 기능 탭의 하위 탭).</summary>
        private bool ImageTabShown
        {
            get { return tabList.SelectedTab == tabKlarf && tabKlarfResult.SelectedTab == tabImage; }
        }

        /// <summary>기능 탭 머리: 위쪽에 기능 색 막대, 고른 탭은 흰 바탕 + 굵은 글자.</summary>
        private void tabList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tabList.TabPages.Count) return;
            TabPage page = tabList.TabPages[e.Index];
            Rectangle r = tabList.GetTabRect(e.Index);
            bool selected = e.Index == tabList.SelectedIndex;
            using (var back = new SolidBrush(selected ? Color.White : Color.FromArgb(232, 232, 232)))
                e.Graphics.FillRectangle(back, r);
            using (var bar = new SolidBrush(TabColor(page)))
                e.Graphics.FillRectangle(bar, r.X, r.Y, r.Width, selected ? 4 : 2);
            TextRenderer.DrawText(e.Graphics, page.Text, selected && _tabBoldFont != null ? _tabBoldFont : tabList.Font, r, Color.FromArgb(30, 30, 30),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        private Color TabColor(TabPage page)
        {
            return page == tabGds ? GdsTabColor : page == tabKlarf ? KlarfTabColor : page == tabEt ? EtTabColor : SummaryTabColor;
        }

        /// <summary>[기준 접기 / 펴기]: 기능 탭 위쪽 기준 묶음을 접어 아래 결과 표를 크게 본다 (2026-10-19).</summary>
        private void CriteriaFold_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Panel criteria = button == btnGdsFold ? pnlGdsCriteria : button == btnKlarfFold ? pnlKlarfCriteria : pnlEtCriteria;
            bool fold = criteria.Visible;
            criteria.Visible = !fold;
            button.Text = fold ? "기준 펴기" : "기준 접기";
            Image image = fold ? _foldExpandImage : _foldCollapseImage;
            if (image != null) button.Image = image;
        }

        private void btnBand_Click(object sender, EventArgs e)
        {
            ShowBandPopup();
        }

        /// <summary>
        /// [색 구간 설정] 팝업 (2026-10-19 결정 L2: 색 구간 설정은 팝업). 색 구간 표(pnlBand: 자동 구간 / 표시 기준 / FROM · TO / 색 / Die 수)를
        /// 작은 창으로 옮겨 [색 구간 설정] 버튼 아래에 보인다. 표 이벤트 / 값은 이 화면 것을 그대로 쓴다.
        /// 닫기(X / Esc)는 숨기기만 하고 화면을 닫을 때 해제한다. Smoke 가 표를 고치기 전에 연다.
        /// </summary>
        public void ShowBandPopup()
        {
            if (_bandPopup == null || _bandPopup.IsDisposed)
            {
                Size size = pnlBand.Size;
                var popup = new Form
                {
                    Text = "색 구간 설정",
                    FormBorderStyle = FormBorderStyle.SizableToolWindow,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    KeyPreview = true,
                    Font = Font,
                    ClientSize = size
                };
                Controls.Remove(pnlBand);
                pnlBand.Dock = DockStyle.Fill;
                pnlBand.Visible = true;
                popup.Controls.Add(pnlBand);
                popup.FormClosing += (s, e) =>
                {
                    if (e.CloseReason != CloseReason.UserClosing) return;
                    e.Cancel = true;
                    popup.Hide();
                };
                popup.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape && !dgvBand.IsCurrentCellInEditMode) popup.Hide(); };
                _bandPopup = popup;
                // 팝업은 DACrux 메인 창 소유라 다른 화면 탭으로 가면 숨긴다(이 화면이 활성 MDI 화면일 때만 보인다)
                if (MdiParent != null) MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
            }
            _bandPopup.Location = btnBand.PointToScreen(new Point(btnBand.Width - _bandPopup.Width, btnBand.Height + 2));
            if (!_bandPopup.Visible) _bandPopup.Show(TopLevelControl as Form ?? this);
            _bandPopup.Activate();
        }

        private void MdiParent_MdiChildActivate(object sender, EventArgs e)
        {
            var parent = sender as Form;
            if (_bandPopup != null && !_bandPopup.IsDisposed && _bandPopup.Visible && parent != null && parent.ActiveMdiChild != this) _bandPopup.Hide();
        }

        #endregion

        #region [Helper]

        private static string DieText(object x, object y)
        {
            return "(" + x + ", " + y + ")";
        }

        private static string ColText(DataRow row, string column)
        {
            return row != null && row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? row[column].ToString().Trim() : string.Empty;
        }

        private static int ColInt(DataRow row, string column)
        {
            int value;
            return int.TryParse(ColText(row, column), NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : 0;
        }

        private static Point DiePointOf(DataRow row)
        {
            return new Point(ColInt(row, "XINDEX"), ColInt(row, "YINDEX"));
        }

        private static DataTable Table(DataSet ds, string name)
        {
            return ds != null && ds.Tables.Contains(name) ? ds.Tables[name] : null;
        }

        private static IEnumerable<DataRow> Rows(DataSet ds, string name)
        {
            DataTable t = Table(ds, name);
            return t == null ? Enumerable.Empty<DataRow>() : t.Rows.Cast<DataRow>();
        }

        private static DataRow FirstRow(DataSet ds, string name)
        {
            return Rows(ds, name).FirstOrDefault();
        }

        /// <summary>E-Test 연결 상태 코드 -> 화면 문구 (서버 ET_LINK.STATUS / CHAIN.ET_LINK_STATUS).</summary>
        private static string LinkText(string status)
        {
            switch (status)
            {
                case "LINKED": return "연결";
                case "NOT_IN_GDS": return "GDS 에 없음";
                case "DUPLICATE": return "이름 중복";
                case "NONE": return "-";
                default: return status;
            }
        }

        private static string RateText(DataRow row, string column)
        {
            double v;
            return double.TryParse(ColText(row, column), NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? Math.Round(v * 100).ToString(CultureInfo.InvariantCulture) + "%" : "";
        }

        private static string GapText(string gap)
        {
            double v;
            return double.TryParse(gap, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v.ToString("0.0", CultureInfo.InvariantCulture) : gap;
        }

        private static double GapSortKey(string gap)
        {
            double v;
            return double.TryParse(gap, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : double.MaxValue;
        }

        /// <summary>사용자 설정 / 가상 Die 처럼 실패해도 화면을 계속 쓸 수 있는 부가 처리.</summary>
        private void TryRun(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("frmGdsKlarfAnalysis: {0}", ex.Message);
            }
        }

        /// <summary>분석 요청 값(백그라운드 스레드로 넘기기 위해 화면 값을 복사한다).</summary>
        /// <summary>[분석] 1회 요청: 서버 요청 + 화면 표시용 값.</summary>
        private sealed class AnalysisRequest
        {
            public GdsKlarfAnalysisRequest Request;
            public long StepSeq;
            public string Device;
            public string SideText;
        }

        #endregion
    }
}
