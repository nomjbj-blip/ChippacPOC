using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// GDS-KLARF 연관 분석 Simple 화면.
    /// 설계: 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md (결정 S1 ~ S10)
    ///
    /// 연관 분석 화면 A(frmGdsKlarfAnalysis)보다 단순하게 KLARF Defect 가 GDS Chain 위에 올라갔는지만 본다.
    /// 화면 A 와 같은 방식으로 왼쪽 DMSearch(메뉴 RESV_04 = DM)에서 Wafer 1장을 받고, 받으면 바로 기본 설정으로 1차 분석한다.
    ///  - 기본 설정 = Defect 크기(XSIZE × YSIZE) 그대로(허용오차 0 µm), GDS Chain 면 BS, KLARF 검사 면 FS 고정, KLARF 위치 조정 0,
    ///    E-Test = 같은 Wafer 의 READY E-Test 중 Test 시각 최신 1개를 자동 연결(없으면 E-Test 없이, 2026-10-22 결정 S2 바뀜).
    ///  - KLARF 위치 조정(2026-10-22): GDS 는 맞다고 보고 KLARF Defect 의 XREL / YREL 에 X / Y 를 더한다(화살표 1 씩, 서버 보정값 그대로).
    ///  - 서버는 화면 A 와 같은 RO.GdsKlarfAnalysis.Analyze 다(서버 / Interface 변경 없음). E-Test 파일 / Wafer Key 는 화면 A 와 같은 GetWaferReady 로 찾는다(FindEtFile).
    ///  - Map: Die 색 = 색 구간, 가운데 숫자 = Chain 위 Defect 수(DIE.IN_CHAIN_COUNT). E-Test 삼각형은 그리지 않는다.
    ///  - 오른쪽 탭 [설정 / 정보] = 분석 정보 / Defect 영역 설정 + Chain 면 / KLARF 위치 조정 + [다시 분석] / 선택 Die 의 Defect 이미지 / Chain 목록,
    ///    [종합] = Defect 가 있는 Die 표(화면 A Die 표에서 E-Test 열을 뺀 것).
    ///  - 용어는 Chain 위 / Chain 밖(= 화면 A 의 Chain In / Chain Out, 결정 S7).
    ///  - Die 더블클릭 / [종합] 행 더블클릭 / Map 우클릭 [Detail Analysis] = Die 상세보기(2026-10-22 2단계, 화면 A 상세보기를 그대로 씀, OpenDetail).
    ///    두 화면 상세보기는 같은 모양이다(2026-10-22 요청 "두 화면에서 동일하게").
    ///  - 아래 상태줄은 두지 않는다(2026-10-22 레이아웃 정리). 알림은 분석 정보 제목 줄에, 진행 표시는 위 막대 오른쪽에 작업 중에만 보인다.
    ///    분석 정보 5줄(Step / GDS / E-Test / 결과 / 기준)은 2026-10-22 에 뺐다. 설정 바뀜 / 알림만 남는다.
    ///
    /// 처리 흐름
    ///  1) DrawWafer(1장, 여러 장이면 오류) -> LoadSelectedWafer: Wafer 조회(작업 스레드) -> ShowWafer(Map / GDS Device = Product 이름)
    ///     -> GDS Device 가 있으면 AnalyzeCore(작업 스레드에서 Analyze) -> ApplyResult. 이미지 경로는 따로 백그라운드로 읽는다.
    ///  2) 설정을 바꾸면 "설정 바뀜" 만 표시하고, [다시 분석] 을 눌러야 다시 계산한다(숫자를 바꿀 때마다 서버를 부르지 않는다).
    ///  3) Die 클릭(Map / 종합 표) = Die 선택 -> 그 Die 의 Defect 이미지 카드(분석 결과와 상관없이 그 Die 의 모든 Defect) / (Wafer 전체를 끄면) 그 Die 의 Chain 목록.
    /// 서비스는 속성으로 바꿀 수 있다(기본 원격 호출, Smoke 검사는 같은 프로세스 BSL).
    /// </summary>
    public partial class frmGdsKlarfSimple : DACruxUXBasic01, iSEMControl, frmGdsDieAnalysis.IDetailSource
    {
        /// <summary>KLARF 검사 면. FS 고정이고 화면에 보이지 않는다(통합 분석 설계 B28, 화면 A 와 같음).</summary>
        private const string KlarfStepSide = "FRONT";

        private long _stepSeq;
        private string _product = string.Empty;
        private string _device = string.Empty;
        private readonly List<string> _devices = new List<string>();
        private string _stepFactory = string.Empty;
        private DataSet _result;
        /// <summary>마지막으로 결과를 받은 분석 요청. Die 상세(AnalyzeDie)가 같은 조건을 쓴다.</summary>
        private GdsKlarfAnalysisRequest _lastRequest;
        /// <summary>분석 결과 순번(결과를 반영할 때마다 +1). 같은 Die 상세보기 창이 같은 결과로 열렸는지 비교한다(화면 A 와 같음).</summary>
        private int _analysisVersion;
        /// <summary>마지막 분석 실패 문구(분석 정보 알림에 보인다). 성공하면 빈 값.</summary>
        private string _failure = string.Empty;
        private DataTable _imagePaths;
        /// <summary>이미지 경로 조회 실패 문구. 읽는 중이거나 성공이면 빈 값.</summary>
        private string _imagePathNote = string.Empty;
        /// <summary>분석 정보 알림(아래 상태줄 대신, 2026-10-22). Wafer 를 다시 받거나 분석이 끝나면 지운다.</summary>
        private string _notice = string.Empty;
        /// <summary>분석에 연결한 E-Test 파일(SRC_FILE_SEQ, 0 = 없음)과 Wafer Key. Wafer 를 받을 때 FindEtFile 이 정한다 (2026-10-22).</summary>
        private long _etSrcFileSeq;
        private string _waferKey = string.Empty;
        /// <summary>E-Test 파일 조회 실패 문구(분석 정보 알림에 보인다). Wafer 를 받을 때 지운다.</summary>
        private string _etNote = string.Empty;
        private Point? _selectedDie;
        private bool _dirty;
        private bool _loading;
        /// <summary>Wafer 조회 / 분석 중. 작업 스레드를 기다리는 동안 DMSearch 에서 Wafer 가 또 들어오면 받지 않는다.</summary>
        private bool _busy;
        private bool _showErrorDialogs = true;
        private bool _mdiHooked;
        private readonly GdsKlarfBandPopup _bandPopup = new GdsKlarfBandPopup();

        /// <summary>Wafer 그리기 / Defect 이미지 경로 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.DefectMapAnalysis DefectService { get; set; }

        /// <summary>GDS Map Device 목록 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsMapView MapService { get; set; }

        /// <summary>연관 분석 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsKlarfAnalysis AnalysisService { get; set; }

        /// <summary>마지막 분석 결과(없으면 null).</summary>
        public DataSet AnalysisResult { get { return _result; } }

        /// <summary>마지막으로 결과를 받은 분석 요청(없으면 null).</summary>
        public GdsKlarfAnalysisRequest LastRequest { get { return _lastRequest; } }

        /// <summary>분석에 쓸 GDS Device(= KLARF Product 와 같은 이름의 READY Map). 없으면 빈 값이고 분석할 수 없다.</summary>
        public string GdsDevice { get { return _device; } }

        /// <summary>선택 Die(없으면 null).</summary>
        public Point? SelectedDie { get { return _selectedDie; } }

        /// <summary>오류를 메시지 창으로도 알릴지(기본 true). 자동 검사(Smoke)는 false 로 두고 분석 정보 알림 줄만 본다.</summary>
        public bool ShowErrorDialogs
        {
            get { return _showErrorDialogs; }
            set { _showErrorDialogs = value; }
        }

        public frmGdsKlarfSimple()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (화면 A 와 같음)
            _loading = true;
            cboChainSide.SelectedIndex = 1;     // GDS Chain 면 기본 BS (결정 S4, 화면 A 와 같음)
            _loading = false;
            DefectArea_CheckedChanged(this, EventArgs.Empty);
            foreach (DataGridView grid in new[] { dgvDie, dgvChain })
                ConfigureGrid(grid);
            _bandPopup.BandsChanged += (s, e) => ColorMap();
            ShowInfo();
            BindDieGrid();
            BindChainGrid();
            RefreshImages();
        }

        /// <summary>Device 목록을 미리 읽는다. 실패해도 화면은 연다(서비스 미기동 등).</summary>
        private void frmGdsKlarfSimple_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                LoadDeviceList();
            }
            catch (Exception ex)
            {
                ShowNotice("GDS Map 목록 조회 실패: " + ex.Message);
            }
        }

        /// <summary>
        /// 처음 보일 때 [설정 / 정보] 탭의 이미지 / Chain 목록을 반씩 나눈다.
        /// 안쪽 SplitContainer 의 나누기 위치(SplitterDistance)는 화면 배율에 맞춰 줄지 않아 설계 값 그대로면 Chain 목록이 너무 낮다(2026-10-22 Smoke 그림 확인).
        /// 그 뒤 크기가 바뀌면 SplitContainer 가 비율을 유지하고, 사용자가 끌어 바꿀 수 있다.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            int half = (splitSetting.Height - splitSetting.SplitterWidth) / 2;
            if (half > splitSetting.Panel1MinSize && half < splitSetting.Height - splitSetting.Panel2MinSize) splitSetting.SplitterDistance = half;
        }

        /// <summary>
        /// 화면을 닫으면 색 구간 팝업 숨기기 연결을 풀고(팝업 해제는 Dispose) 상세보기 공유 도면을 놓는다
        /// (열린 상세보기 창도 없으면 메모리를 돌려준다, 화면 A 와 같음).
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            frmGdsDieAnalysis.Release(this);
            if (_mdiHooked && MdiParent != null) MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            base.OnFormClosed(e);
        }

        #region [Wafer]

        /// <summary>
        /// 왼쪽 DMSearch [Search] 결과를 받는다(iSEMControl). Wafer(Step) 1장만 분석한다. 여러 개면 오류로 알리고 지금 화면은 그대로 둔다(화면 A 와 같은 문구).
        /// </summary>
        public void DrawWafer(DPWafer[] wafer)
        {
            List<DPWafer> list = wafer == null ? new List<DPWafer>() : wafer.GroupBy(w => w.StepSeq).Select(g => g.First()).ToList();
            if (list.Count == 0)
            {
                ShowNotice("받은 Wafer 가 없습니다.");
                return;
            }
            if (list.Count > 1)
            {
                string message = "1개 Wafer 만 분석할 수 있습니다. Wafer 를 하나만 선택하세요. (선택 " + list.Count + "개)";
                ShowNotice(message);
                if (_showErrorDialogs) MessageBox.Show(this, message, "Wafer 선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_busy)
            {
                ShowNotice("조회 / 분석 중이라 받은 Wafer 를 그리지 않았습니다.");
                return;
            }
            this.DPWaferList = new[] { list[0] };
            LoadSelectedWafer(list[0]);
        }

        /// <summary>
        /// 선택한 Wafer 의 Defect / 기준 정보 / 검사 Die 를 기존 Defect Map Viewer 와 같은 서비스로 읽어 그리고, 바로 1차 분석한다.
        /// 서버 조회와 분석은 위 막대 오른쪽 busyBar 가 작업 스레드에서 돌리고, 한 작업("Wafer 조회 + 분석")으로 묶어 모래시계가 중간에 풀리지 않게 한다.
        /// 오류 창은 묶음이 끝난 뒤 띄운다(작업 중에는 화면 입력을 막기 때문, 화면 A 와 같음).
        /// </summary>
        private void LoadSelectedWafer(DPWafer wafer)
        {
            long step;
            if (!long.TryParse(wafer.StepSeq, NumberStyles.Integer, CultureInfo.InvariantCulture, out step)) return;   // DPWafer 는 구조체

            _busy = true;
            _notice = string.Empty;
            Exception analysisFailure = null;
            try
            {
                RO.DefectMapAnalysis svc = DefectService ?? new RO.DefectMapAnalysis();
                long[] steps = { step };
                using (BeginBusy("Wafer 조회 + 분석"))
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
                    LoadImagePathsAsync(step);
                    // 1차 분석: Wafer 를 받으면 [분석] 없이 기본 설정으로 바로 한다 (요청 2026-10-22). 같은 Wafer 의 최신 E-Test 를 먼저 찾는다
                    if (_device.Length > 0)
                    {
                        FindEtFile(step);
                        analysisFailure = AnalyzeCore();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotice("Wafer 조회 실패: " + ex.Message);
                if (_showErrorDialogs) MessageBox.Show(this, ex.Message, "Wafer 조회", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _busy = false;
            }
            if (analysisFailure != null) ReportFailure(analysisFailure);
        }

        /// <summary>
        /// Wafer Map 을 그린다(분석 전 모습: Defect 점만). 설정 순서는 화면 A / DPUCDefecMapViewer.Draw 와 같다(LeftBottom / Notch 0 / Pitch / Origin -> DieCalculation -> AddDie).
        /// 이전 분석 결과는 지운다. 분석은 하지 않는다(LoadSelectedWafer 가 이어서 부른다). Smoke 검사도 이 함수를 직접 부를 수 있다.
        /// </summary>
        public void ShowWafer(long stepSeq, DefectList defects, DataSet info, DataTable mapTable)
        {
            DataRow stepRow = info.Tables["STEP_INFO"].Rows[0];
            DataRow setup = info.Tables["SETUP_INFO"].Rows[0];
            _stepSeq = stepSeq;
            _product = ColText(stepRow, "PRODUCT");
            _stepFactory = ColText(stepRow, "FACTORY");
            _imagePaths = null;
            _imagePathNote = string.Empty;
            _etSrcFileSeq = 0;
            _waferKey = string.Empty;
            _etNote = string.Empty;
            lblWaferInfo.Text = String.Format("{0}  /  {1}  (Product {2})", ColText(stepRow, "WAFER_ID"), ColText(stepRow, "STEP_ID"), _product);
            // Wafer 문구 길이에 따라 GDS Device 를 바로 뒤에 둔다(겹치지 않게)
            lblDevice.Left = lblWaferInfo.Right + 30;
            lblDeviceValue.Left = lblDevice.Right + 6;

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
            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            map.DieSelectMode();   // 기본 Die 선택 (화면 A 와 같음)

            SelectDeviceForProduct();   // GDS Map 이 없으면 분석 정보 알림에 보인다
            _selectedDie = null;
            _failure = string.Empty;
            ClearResult();
            tabList.SelectedTab = tabSetting;
        }

        /// <summary>Defect 이미지 경로(TQD_IMAGES)를 백그라운드로 읽는다. 실패하면 이미지 머리에 "이미지 경로 조회 실패" 만 보인다.</summary>
        private void LoadImagePathsAsync(long step)
        {
            RO.DefectMapAnalysis svc = DefectService ?? new RO.DefectMapAnalysis();
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                DataTable paths = null;
                string note = string.Empty;
                try
                {
                    paths = svc.GetDefectImagePath01(new[] { step });
                }
                catch (Exception ex)
                {
                    note = "이미지 경로 조회 실패";
                    System.Diagnostics.Trace.TraceWarning("Defect 이미지 경로 조회 실패: {0}", ex.Message);
                }
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(new Action(() =>
                    {
                        if (_stepSeq != step) return;
                        _imagePaths = paths;
                        _imagePathNote = note;
                        RefreshImages();
                    }));
            });
        }

        #endregion

        #region [GDS Device]

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
        /// GDS Device 는 Wafer 의 Product 를 따른다(화면 A 와 같은 규칙). 같은 이름(대소문자 무시)의 READY GDS Map 이 없으면 빈 값이고 분석할 수 없다.
        /// 반환: 안내 문구(" / Product ...: 같은 이름의 READY GDS Map 없음 (분석 안 함)" 또는 빈 값). 화면에는 분석 정보 알림으로 보인다
        /// </summary>
        private string SelectDeviceForProduct()
        {
            if (_devices.Count == 0) TryRun(LoadDeviceList);
            string device = _product.Length == 0 ? null
                : _devices.Contains(_product) ? _product
                : _devices.FirstOrDefault(d => String.Equals(d, _product, StringComparison.OrdinalIgnoreCase));
            _device = device ?? string.Empty;
            lblDeviceValue.Text = _device.Length > 0 ? _device : "없음 - 분석 안 함";
            lblDeviceValue.ForeColor = _device.Length > 0 ? Color.FromArgb(20, 60, 130) : Color.Firebrick;
            btnAnalyze.Enabled = _device.Length > 0;
            return _device.Length > 0 || _product.Length == 0 ? string.Empty : " / Product " + _product + ": 같은 이름의 READY GDS Map 없음 (분석 안 함)";
        }

        #endregion

        #region [분석]

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            string error = CheckAnalyzable();
            if (error != null)
            {
                MessageBox.Show(this, error, "다시 분석", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _busy = true;
            Exception failure;
            try
            {
                using (BeginBusy("다시 분석"))
                    failure = AnalyzeCore();
            }
            finally { _busy = false; }
            if (failure != null) ReportFailure(failure);
        }

        /// <summary>상세보기 [다시 분석] (2026-10-22): [다시 분석] 과 같다. 결과를 반영하면 상세보기 창도 새 결과로 바뀐다(SourceAnalyzed).</summary>
        public void AnalyzeAgain()
        {
            btnAnalyze_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// 진행 표시(busyBar)를 위 막대 오른쪽에 보이고 작업을 시작한다. 끝나면(Dispose) 다시 숨긴다 (2026-10-22 레이아웃 정리).
        /// busyBar 는 작업이 끝난 뒤에도 "… 완료" 글자를 남기므로, 쉬는 동안에는 숨겨 위 막대에 정보 글자가 남지 않게 한다.
        /// 작업 스레드 실행은 busyBar 핸들이 있어야 하므로 보인 뒤에 시작한다(숨김 상태면 화면 스레드에서 실행된다).
        /// </summary>
        private IDisposable BeginBusy(string name)
        {
            busyBar.Visible = true;
            busyBar.Update();
            return new BusyScope(busyBar, busyBar.Begin(name));
        }

        /// <summary>BeginBusy 묶음: 작업을 끝내고 진행 표시를 숨긴다.</summary>
        private sealed class BusyScope : IDisposable
        {
            private readonly System.Windows.Forms.Control _bar;
            private IDisposable _job;

            public BusyScope(System.Windows.Forms.Control bar, IDisposable job)
            {
                _bar = bar;
                _job = job;
            }

            public void Dispose()
            {
                if (_job == null) return;
                _job.Dispose();
                _job = null;
                if (!_bar.IsDisposed) _bar.Visible = false;
            }
        }

        /// <summary>Smoke 검사용: 현재 설정으로 같은 스레드에서 분석하고 화면에 반영한다. 분석할 수 없으면 예외.</summary>
        public DataSet AnalyzeNow()
        {
            string error = CheckAnalyzable();
            if (error != null) throw new InvalidOperationException(error);
            GdsKlarfAnalysisRequest request = BuildRequest();
            DataSet ds = (AnalysisService ?? new RO.GdsKlarfAnalysis()).Analyze(request);
            FinishAnalysis(request, ds);
            return ds;
        }

        private string CheckAnalyzable()
        {
            if (_stepSeq <= 0) return "왼쪽 Wafer 조회에서 Wafer 를 먼저 [Search] 하세요.";
            if (_device.Length == 0)
                return "Product " + _product + " 와 같은 이름의 READY GDS Map 이 없어 분석할 수 없습니다." + Environment.NewLine
                    + "GDS Map 조회 화면 [Map 복사]로 Product 이름의 Map 을 만드세요.";
            return null;
        }

        /// <summary>
        /// 분석 요청(설계 5.1절). 화면 A 와 같은 Analyze 를 쓰고, 화면에서 고르지 않는 값은 화면 A 의 숨김 기본값과 같다.
        ///  - Defect 영역: 크기 + 허용오차(기본 0 µm = Defect 크기 그대로) / 중심 정사각형. 서버는 Defect 좌표 중심 XSIZE × YSIZE 사각형과
        ///    Chain Member 실제 모양의 거리(Gap)가 허용오차 이하면 Chain 위로 본다.
        ///  - KLARF 위치 조정 X / Y = 서버 보정값(OffsetXUm / OffsetYUm). 서버는 반전 전에 XREL / YREL 에 더하므로 KLARF REL 을 옮긴 것과 같다
        ///    (2026-10-22, 문서/설계/05_연관분석/2026-10-22_KLARF_위치조정_설계.md 2.1절).
        ///  - E-Test 파일 / Wafer Key = Wafer 를 받을 때 FindEtFile 이 찾은 값(없으면 0 = E-Test 분석 안 함, 2026-10-22).
        ///  - Chain 전체 / RoughBin 전체 / 큰 Defect 포함 / KLARF 검사 면 FS 고정.
        /// </summary>
        private GdsKlarfAnalysisRequest BuildRequest()
        {
            return new GdsKlarfAnalysisRequest
            {
                Factory = CurrentFactory,
                DeviceId = _device,
                WaferKey = _waferKey,
                StepSeqs = new[] { _stepSeq },
                ViewSides = new[] { KlarfStepSide },
                ChainSide = ChainSide,
                DisplayStepSeq = _stepSeq,
                AreaMode = rdoDefectSize.Checked ? GdsKlarfAnalysisRequest.AreaSizeTolerance : GdsKlarfAnalysisRequest.AreaSquare,
                ToleranceUm = (double)numTolerance.Value,
                SquareUm = (double)numSquare.Value,
                ExcludeLargeDefect = false,
                OffsetXUm = (double)numShiftX.Value,
                OffsetYUm = (double)numShiftY.Value,
                ChainSeqs = null,
                ClassBasis = GdsKlarfAnalysisRequest.ClassBasisRoughBin,
                ClassValues = null,
                EtSrcFileSeq = _etSrcFileSeq
            };
        }

        /// <summary>
        /// 같은 Wafer 의 E-Test 파일을 자동으로 연결한다 (2026-10-22 요청 "두 화면 상세보기 동일하게" + 결정 "같은 Wafer 최신 파일 자동").
        /// 화면 A 와 같은 서버 준비 조회(GetWaferReady)의 READY_ET(같은 Wafer Key + 분석 GDS Device 와 같은 Device 파일) 중 SELECTED(Test 시각 최신 1개)를 쓴다.
        /// 파일이 없거나 조회가 실패하면 E-Test 없이 분석한다(조회 실패는 분석 정보 알림에 보임). 서버 조회는 busyBar 작업 스레드에서 돈다.
        /// </summary>
        private void FindEtFile(long step)
        {
            _etSrcFileSeq = 0;
            _waferKey = string.Empty;
            _etNote = string.Empty;
            RO.GdsKlarfAnalysis svc = AnalysisService ?? new RO.GdsKlarfAnalysis();
            string factory = CurrentFactory, device = _device;
            DataSet ready;
            try
            {
                ready = busyBar.Run("E-Test 파일", () => svc.GetWaferReady(factory, step, device));
            }
            catch (Exception ex)
            {
                _etNote = "E-Test 파일 조회 실패 / E-Test 없이 분석";
                Trace.TraceWarning("E-Test 준비 조회 실패: {0}", ex.Message);
                return;
            }
            DataRow key = Rows(ready, "WAFER_KEY").FirstOrDefault();
            _waferKey = key == null ? string.Empty : ColText(key, "WAFER_KEY");
            DataRow file = Rows(ready, "READY_ET")
                .Where(r => { long seq; return long.TryParse(ColText(r, "SRC_FILE_SEQ"), NumberStyles.Integer, CultureInfo.InvariantCulture, out seq) && seq > 0; })
                .OrderBy(r => ColText(r, "SELECTED") == "Y" ? 0 : 1).FirstOrDefault();
            if (file == null) return;
            _etSrcFileSeq = long.Parse(ColText(file, "SRC_FILE_SEQ"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 지금 Wafer 를 분석한다: 서버 Analyze 는 busyBar 작업 스레드에서 돌리고(그동안 이 화면 입력은 막음) 결과는 화면 스레드에서 반영한다.
        /// 실패하면 결과를 지우고 예외를 돌려준다(오류 창은 부르는 쪽이 작업 묶음이 끝난 뒤 띄운다). 분석 중 Wafer 가 바뀌었으면 결과를 버린다.
        /// </summary>
        private Exception AnalyzeCore()
        {
            GdsKlarfAnalysisRequest request = BuildRequest();
            long step = _stepSeq;
            RO.GdsKlarfAnalysis svc = AnalysisService ?? new RO.GdsKlarfAnalysis();
            btnAnalyze.Enabled = false;
            DataSet ds = null;
            Exception failure = null;
            try { ds = busyBar.Run("분석 (" + request.DeviceId + ")", () => svc.Analyze(request)); }
            catch (Exception ex) { failure = ex; }
            if (IsDisposed) return null;
            btnAnalyze.Enabled = _device.Length > 0;
            if (step != _stepSeq) return null;
            if (failure != null)
            {
                _failure = "분석 실패: " + failure.Message;
                ClearResult();
                return failure;
            }
            // 상세보기 도면 데이터를 백그라운드로 미리 읽는다. 첫 Die 더블클릭도 도면 읽기를 기다리지 않는다.
            // 결과 반영 끝에 열린 상세보기도 새 결과로 바꾸므로 반영보다 먼저 시작한다 (화면 A 와 같음)
            frmGdsDieAnalysis.Preload(this, ds);
            // 결과 반영(Map 색 / 표)도 화면 스레드에서 시간이 걸려 모래시계 커서를 유지한다 (화면 A 와 같음)
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
                FinishAnalysis(request, ds);
            return null;
        }

        private void FinishAnalysis(GdsKlarfAnalysisRequest request, DataSet ds)
        {
            _lastRequest = request;
            _failure = string.Empty;
            _notice = string.Empty;
            ApplyResult(ds);
        }

        /// <summary>분석 실패: 문구는 분석 정보 알림(_failure)에 이미 보이므로 메시지 창만 띄운다.</summary>
        private void ReportFailure(Exception failure)
        {
            if (_showErrorDialogs) MessageBox.Show(this, failure.Message, "분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 분석 결과를 화면에 반영한다: 색 구간(자동이면 다시 만듦) -> Map 색 / 숫자 -> 종합 -> Chain 목록 -> 이미지 -> 분석 정보.
        /// 마지막에 이 화면에서 연 상세보기 창도 새 결과로 바꾼다(2026-10-22 동기화, 화면 A 와 같음).
        /// </summary>
        private void ApplyResult(DataSet ds)
        {
            _result = ds;
            _analysisVersion++;
            _dirty = false;
            if (_selectedDie.HasValue && FindDieRow(_selectedDie.Value) == null)
            {
                _selectedDie = null;
                map.ClearSelectedDie();
            }
            _bandPopup.SetDieValues(Rows(_result, "DIE").Select(r => new[] { ColInt(r, "DEFECT_COUNT"), ColInt(r, "IN_CHAIN_COUNT") }).ToList());
            ColorMap();
            BindDieGrid();
            BindChainGrid();
            RefreshImages();
            ShowInfo();
            frmGdsDieAnalysis.SourceAnalyzed(this, _analysisVersion, _result, _lastRequest, AnalysisService);
        }

        /// <summary>분석 결과 표시를 지운다(Wafer 변경 / 분석 실패). 색 구간 표 Die 수도 비운다.</summary>
        private void ClearResult()
        {
            _result = null;
            _lastRequest = null;
            _dirty = false;
            _bandPopup.SetDieValues(null);
            map.ClearDieDisplay();
            BindDieGrid();
            BindChainGrid();
            RefreshImages();
            ShowInfo();
        }

        /// <summary>GDS Chain 면: FRONT(FS) / BACK(BS). 반전 규칙(Chain 면 ≠ KLARF 검사 면 FS)은 서버가 계산한다.</summary>
        private string ChainSide { get { return cboChainSide.SelectedIndex == 1 ? "BACK" : "FRONT"; } }

        #endregion

        #region [Map 색 / 색 구간]

        /// <summary>
        /// Die 채움색 / 숫자를 Map 에 넣는다. 색 = 색 구간 팝업 기준(Defect 없음 흰색 / 표시 기준 미만 연회색 / 구간 색),
        /// 숫자 = Chain 위 Defect 수(Defect 가 있는 Die 만). E-Test 표식은 넣지 않는다.
        /// </summary>
        private void ColorMap()
        {
            var fills = new Dictionary<Point, Color>();
            var labels = new Dictionary<Point, int>();
            foreach (DataRow row in Rows(_result, "DIE"))
            {
                Point key = DiePointOf(row);
                int total = ColInt(row, "DEFECT_COUNT");
                int inChain = ColInt(row, "IN_CHAIN_COUNT");
                fills[key] = _bandPopup.DieColor(total, inChain);
                if (total > 0) labels[key] = inChain;
            }
            map.SetDieDisplay(fills, labels);
        }

        private void btnBand_Click(object sender, EventArgs e)
        {
            ShowBandPopup();
        }

        /// <summary>[색 구간 설정] 팝업을 버튼 아래에 보인다. 팝업은 DACrux 메인 창 소유라 다른 화면 탭으로 가면 숨긴다.</summary>
        public void ShowBandPopup()
        {
            if (!_mdiHooked && MdiParent != null)
            {
                MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
                _mdiHooked = true;
            }
            _bandPopup.ShowNear(btnBand);
        }

        private void MdiParent_MdiChildActivate(object sender, EventArgs e)
        {
            var parent = sender as Form;
            if (!_bandPopup.IsDisposed && _bandPopup.Visible && parent != null && parent.ActiveMdiChild != this) _bandPopup.Hide();
        }

        #endregion

        #region [종합 / Chain 목록 / 이미지]

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.AutoGenerateColumns = true;
        }

        /// <summary>
        /// [종합] 탭: Defect 가 있는 Die 만, 서버 정렬(Chain 위 많은 순 -> Defect 전체 -> Y -> X) 그대로.
        /// 화면 A Die 표에서 E-Test 열(ET 판정 / Fail Chain / 일치 Chain)을 뺐다. Defect 전체 = Chain 위 + Chain 밖.
        /// </summary>
        private void BindDieGrid()
        {
            var table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("Defect 전체", typeof(int));
            table.Columns.Add("Chain 위", typeof(int));
            table.Columns.Add("Chain 밖", typeof(int));
            table.Columns.Add("Chain 수", typeof(int));
            table.Columns.Add("검사", typeof(string));
            int hitDies = 0;
            foreach (DataRow r in Rows(_result, "DIE"))
            {
                if (ColInt(r, "DEFECT_COUNT") <= 0) continue;
                table.Rows.Add(ColInt(r, "XINDEX"), ColInt(r, "YINDEX"), ColInt(r, "DEFECT_COUNT"), ColInt(r, "IN_CHAIN_COUNT"), ColInt(r, "OUT_CHAIN_COUNT"),
                    ColInt(r, "HIT_CHAIN_COUNT"), ColText(r, "IN_PLAN"));
                if (ColInt(r, "IN_CHAIN_COUNT") > 0) hitDies++;
            }
            dgvDie.DataSource = table;
            lblDieCount.Text = _result == null ? "Die 0" : String.Format("Die {0} / Chain 위 Defect 있는 Die {1}", table.Rows.Count, hitDies);
            SyncDieGridSelection();
        }

        /// <summary>
        /// Chain 목록(결정 S6): [Wafer 전체] 또는 선택 Die 가 없으면 Chain 별 합계(CHAIN: Chain 위 Defect 수 / Die 수 / Member),
        /// 아니면 선택 Die 의 Chain(DIE_CHAIN: Defect 수 / Defect ID / Member). [Defect 있는 Chain 만] 을 켜면 Defect 0 인 Chain 은 뺀다(기본 켬,
        /// U8 처럼 Chain 이 2만 개 넘는 Device 에서 표를 가볍게 둔다). Chain 은 이름으로만 보인다(화면 A 와 같음).
        /// </summary>
        private void BindChainGrid()
        {
            var table = new DataTable();
            bool waferMode = chkChainAll.Checked || !_selectedDie.HasValue;
            bool hitOnly = chkChainHit.Checked;
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("Defect 수", typeof(int));
            if (waferMode) table.Columns.Add("Die 수", typeof(int));
            else table.Columns.Add("Defect ID", typeof(string));
            table.Columns.Add("Member", typeof(int));
            int total = 0;
            if (_result != null)
            {
                List<DataRow> chains = Rows(_result, "CHAIN").ToList();
                total = chains.Count;
                if (waferMode)
                {
                    foreach (DataRow c in chains.Where(c => !hitOnly || ColInt(c, "HIT_DEFECT_COUNT") > 0)
                        .OrderByDescending(c => ColInt(c, "HIT_DEFECT_COUNT")).ThenByDescending(c => ColInt(c, "HIT_DIE_COUNT")).ThenBy(c => ColText(c, "CHAIN_NAME")))
                        table.Rows.Add(ChainName(c), ColInt(c, "HIT_DEFECT_COUNT"), ColInt(c, "HIT_DIE_COUNT"), ColInt(c, "MEMBER_COUNT"));
                }
                else
                {
                    var bySeq = new Dictionary<string, DataRow>(StringComparer.Ordinal);
                    foreach (DataRow c in chains)
                        if (!bySeq.ContainsKey(ColText(c, "CHAIN_SEQ"))) bySeq.Add(ColText(c, "CHAIN_SEQ"), c);
                    Point die = _selectedDie.Value;
                    foreach (DataRow dc in Rows(_result, "DIE_CHAIN").Where(r => DiePointOf(r) == die && (!hitOnly || ColInt(r, "DEFECT_COUNT") > 0))
                        .OrderByDescending(r => ColInt(r, "DEFECT_COUNT")))
                    {
                        DataRow c;
                        bySeq.TryGetValue(ColText(dc, "CHAIN_SEQ"), out c);
                        table.Rows.Add(c == null ? ColText(dc, "CHAIN_CODE") : ChainName(c), ColInt(dc, "DEFECT_COUNT"), ColText(dc, "DEFECT_IDS"),
                            c == null ? 0 : ColInt(c, "MEMBER_COUNT"));
                    }
                }
            }
            dgvChain.DataSource = table;
            lblChainCount.Text = _result == null ? string.Empty : String.Format("{0} / {1}", table.Rows.Count, total);
        }

        private static string ChainName(DataRow chain)
        {
            string name = ColText(chain, "CHAIN_NAME");
            return name.Length > 0 ? name : ColText(chain, "CHAIN_CODE");
        }

        private void ChainFilter_Changed(object sender, EventArgs e)
        {
            if (_loading) return;
            BindChainGrid();
        }

        /// <summary>
        /// Defect 이미지(2026-10-22 레이아웃 정리로 바뀜, 이전 결정 S5 의 Chain 위 / 밖 묶음은 없앰):
        /// 선택 Die 에 있는 모든 Defect 의 이미지 카드를 Defect ID 순으로 보인다. 분석 결과와 상관없다(분석 전 / GDS Map 없음이어도 보인다).
        /// 카드는 Defect Map Viewer / 화면 A Image 탭과 같은 DPUCDefectImageCard(폭 220, FTP 다운로드는 카드 대기열, 더블클릭 = 큰 이미지 창)다.
        /// 경로 = TQD_IMAGES.IMAGE_PATH / IMAGE_FILENAME (키 STEP_SEQ + DEFECTID). 이미지가 없는 Defect 는 카드를 만들지 않고 머리의 "이미지 n" 으로만 안다.
        /// 지난 카드는 Dispose 로 지운다(이미지 / 창 핸들이 남지 않게, 화면 A 와 같음).
        /// </summary>
        private void RefreshImages()
        {
            while (flowImage.Controls.Count > 0) flowImage.Controls[0].Dispose();
            if (!_selectedDie.HasValue)
            {
                lblImageInfo.Text = "선택 Die 없음";
                return;
            }
            Point die = _selectedDie.Value;
            List<Defect> defects = map.Defects.Where(d => d.XINDEX == die.X && d.YINDEX == die.Y).OrderBy(d => d.DEFECTID).ToList();
            Dictionary<long, List<string>> paths = ImagePathsById();
            List<string> list;
            string text = String.Format("Die ({0}, {1})  Defect {2}", die.X, die.Y, defects.Count);
            if (paths == null) text += _imagePathNote.Length > 0 ? " / " + _imagePathNote : " / 이미지 경로 읽는 중";
            else text += " / 이미지 " + defects.Sum(d => paths.TryGetValue((long)d.DEFECTID, out list) ? list.Count : 0);
            lblImageInfo.Text = text;
            if (paths == null) return;

            foreach (Defect d in defects)
            {
                if (!paths.TryGetValue((long)d.DEFECTID, out list)) continue;
                foreach (string path in list)
                {
                    try
                    {
                        flowImage.Controls.Add(new DACrux.SEMDMS.Control.DPUCDefectImageCard(d, path));
                    }
                    catch (Exception ex)
                    {
                        ShowNotice("이미지 표시 실패: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>이미지 경로 표 -> Defect ID 별 경로 목록(IMAGE_ID 순). 아직 읽지 못했으면 null.</summary>
        private Dictionary<long, List<string>> ImagePathsById()
        {
            if (_imagePaths == null) return null;
            var paths = new Dictionary<long, List<string>>();
            foreach (DataRow row in _imagePaths.Rows.Cast<DataRow>().OrderBy(r => ColLong(r, "IMAGE_ID")))
            {
                long id = ColLong(row, "DEFECTID");
                List<string> list;
                if (!paths.TryGetValue(id, out list)) paths.Add(id, list = new List<string>());
                list.Add(String.Format("{0}/{1}", ColText(row, "IMAGE_PATH"), ColText(row, "IMAGE_FILENAME")));
            }
            return paths;
        }

        #endregion

        #region [Die 선택]

        /// <summary>Die 를 고른다: Map 빨간 테두리 / 종합 행 / Chain 목록 / 이미지. 탭은 바꾸지 않는다.</summary>
        public void SelectDie(int x, int y)
        {
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                _selectedDie = new Point(x, y);
                map.SetSelectedDie(x, y);
                map.Redraw();
                SyncDieGridSelection();
                BindChainGrid();
                RefreshImages();
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

        /// <summary>Die 더블클릭 = 그 Die 상세보기 (2026-10-22 2단계, 설계 15절).</summary>
        private void map_DieDoubleClicked(object sender, GdsKlarfDieEventArgs e)
        {
            SelectDie(e.XIndex, e.YIndex);
            OpenDetail(e.XIndex, e.YIndex);
        }

        /// <summary>우클릭 [Detail Analysis] = 그 Die 상세보기(화면 A 와 같은 메뉴, 1단계에서는 숨겼음).</summary>
        private void map_DetailAnalysisRequested(object sender, GdsKlarfDieEventArgs e)
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

        /// <summary>[종합] 행 더블클릭 = 그 Die 상세보기.</summary>
        private void dgvDie_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvDie.Rows[e.RowIndex];
            OpenDetail(System.Convert.ToInt32(row.Cells["X"].Value), System.Convert.ToInt32(row.Cells["Y"].Value));
        }

        #endregion

        #region [Die 상세보기]

        /// <summary>
        /// Die 상세보기(2026-10-22 2단계, 설계 15절): 화면 A 의 상세보기(frmGdsDieAnalysis + GdsMap GdsDieAnalysisForm)를 그대로 쓴다.
        /// 상세보기 창이 마지막 분석 요청(연결한 E-Test 포함)으로 그 Die 만 다시 계산(AnalyzeDie)한다. 두 화면 상세보기는 같은 모양이다(2026-10-22, 따로 표시하지 않음).
        /// 상세보기 창은 하나다: 열려 있으면 그 창 왼쪽 Die 목록에서 그 Die 를 고르고, 이 화면이 다시 분석하면 그 창도 새 결과로 바뀐다
        /// (2026-10-22, 문서/설계/05_연관분석/2026-10-22_Die상세_DLL로딩_Die목록_설계.md 5절, 화면 A 와 같음).
        /// DACrux ShowForm 은 ENGUI 를 다른 어셈블리 사본으로 읽어 창이 겹치므로 쓰지 않고 상세보기 창 Open 으로 직접 만든다(화면 A 2026-10-12 확인).
        /// </summary>
        private void OpenDetail(int x, int y)
        {
            if (_result == null || _lastRequest == null)
            {
                ShowNotice("분석 결과가 없어 상세보기를 열 수 없습니다.");
                return;
            }
            if (_dirty) ShowNotice("설정 바뀜: 상세보기는 마지막 분석 조건");
            // 상세 화면 만들기(GDS DLL 읽기 포함) / 그 Die 계산 동안 모래시계 커서 (화면 A 와 같음)
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                try { frmGdsDieAnalysis.Open(this, _analysisVersion, _result, _lastRequest, AnalysisService, x, y, null); }
                catch (Exception ex)
                {
                    ShowNotice("Die 상세 분석 실패: " + ex.Message);
                    if (_showErrorDialogs) MessageBox.Show(this, ex.Message, "Die 상세", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        #region [분석 정보 / 설정 변경]

        private void Condition_Changed(object sender, EventArgs e)
        {
            if (_loading || _result == null) return;
            _dirty = true;
            ShowInfo();
        }

        /// <summary>Defect 영역 라디오: 고른 쪽 입력칸만 쓸 수 있게 하고 설정 바뀜으로 표시한다.</summary>
        private void DefectArea_CheckedChanged(object sender, EventArgs e)
        {
            numTolerance.Enabled = rdoDefectSize.Checked;
            numSquare.Enabled = rdoSquare.Checked;
            Condition_Changed(sender, e);
        }

        /// <summary>KLARF 위치 조정 화살표 한 번에 옮기는 값(결정 K3: 1 고정).</summary>
        private const decimal ShiftStep = 1m;

        /// <summary>
        /// KLARF 위치 조정 화살표(2026-10-22 설계 3.2절, 결정 K1): KLARF REL 좌표계 기준 ◀ = X − 1 / ▶ = X + 1 / ▲ = Y + 1 / ▼ = Y − 1.
        /// 상세보기 도면도 같은 보기 좌표(DEFECT.GDS_X)로 그리므로 Chain 면 BS(반전)여도 보이는 방향이 같다.
        /// 값만 바꾸고 다시 계산은 [다시 분석] 이 한다(결정 K2).
        /// </summary>
        private void ShiftButton_Click(object sender, EventArgs e)
        {
            if (sender == btnShiftLeft) MoveShift(numShiftX, -ShiftStep);
            else if (sender == btnShiftRight) MoveShift(numShiftX, ShiftStep);
            else if (sender == btnShiftUp) MoveShift(numShiftY, ShiftStep);
            else if (sender == btnShiftDown) MoveShift(numShiftY, -ShiftStep);
        }

        private static void MoveShift(NumericUpDown box, decimal delta)
        {
            box.Value = Math.Max(box.Minimum, Math.Min(box.Maximum, box.Value + delta));
        }

        /// <summary>[초기화]: KLARF 위치 조정 X / Y 를 0 으로 되돌린다(다시 계산은 [다시 분석]).</summary>
        private void btnShiftReset_Click(object sender, EventArgs e)
        {
            numShiftX.Value = 0;
            numShiftY.Value = 0;
        }

        /// <summary>
        /// KLARF 위치 조정 값이 바뀌면 설정 바뀜으로 표시한다(분석 결과가 있을 때만).
        /// 값은 Wafer 를 바꿔도 그대로 둔다(결정 K5).
        /// </summary>
        private void Shift_ValueChanged(object sender, EventArgs e)
        {
            if (_result == null) return;
            Condition_Changed(sender, e);
        }

        /// <summary>
        /// [설정 / 정보] 탭 분석 정보 제목 줄: 설정을 바꾸고 아직 [다시 분석] 하지 않았으면 "설정 바뀜", 그 뒤에 알림을 잇는다.
        /// 알림 = GDS Map 없음 / 분석 실패 / E-Test 파일 조회 실패 / 오류 · 받지 않은 Wafer 등(아래 상태줄 대신, 2026-10-22). 없으면 숨긴다.
        /// 2026-10-22: 제목 아래 5줄(Step / GDS / E-Test / 결과 / 기준)은 화면에서 뺐다.
        /// </summary>
        private void ShowInfo()
        {
            var alerts = new List<string>();
            if (_stepSeq > 0 && _device.Length == 0) alerts.Add("Product " + _product + " 와 같은 이름의 GDS Map 없음 / 분석 안 함");
            if (_failure.Length > 0) alerts.Add(_failure);
            if (_etNote.Length > 0) alerts.Add(_etNote);
            if (_notice.Length > 0) alerts.Add(_notice);
            lblChanged.Visible = _dirty;
            lblInfo.Text = String.Join(" · ", alerts.ToArray());
            lblInfo.Visible = alerts.Count > 0;
            // 설정 바뀜이 보이면 그 뒤에 잇는다
            lblInfo.Left = lblChanged.Visible ? lblChanged.Right + lblChanged.Margin.Right * 3 : lblChanged.Left;
            lblInfo.Width = Math.Max(0, pnlInfo.ClientSize.Width - lblInfo.Left - lblChanged.Margin.Right * 2);
        }

        /// <summary>알림(오류 / 받지 않은 Wafer 등)을 분석 정보 제목 줄에 보인다. 아래 상태줄 대신이다 (2026-10-22).</summary>
        private void ShowNotice(string text)
        {
            _notice = (text ?? string.Empty).Replace(Environment.NewLine, " ");
            ShowInfo();
        }

        #endregion

        #region [Helper]

        private DataRow FindDieRow(Point die)
        {
            return Rows(_result, "DIE").FirstOrDefault(r => DiePointOf(r) == die);
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

        private static long ColLong(DataRow row, string column)
        {
            long value;
            return long.TryParse(ColText(row, column), NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : 0;
        }

        private static Point DiePointOf(DataRow row)
        {
            return new Point(ColInt(row, "XINDEX"), ColInt(row, "YINDEX"));
        }

        private static IEnumerable<DataRow> Rows(DataSet ds, string name)
        {
            return ds != null && ds.Tables.Contains(name) ? ds.Tables[name].Rows.Cast<DataRow>() : Enumerable.Empty<DataRow>();
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
                System.Diagnostics.Trace.TraceWarning("frmGdsKlarfSimple: {0}", ex.Message);
            }
        }

        #endregion
    }
}
