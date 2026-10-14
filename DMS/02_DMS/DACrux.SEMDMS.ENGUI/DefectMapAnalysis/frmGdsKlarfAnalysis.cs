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
    /// GDS × KLARF 연관 분석 화면(화면 A). 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4절
    ///
    /// 기존 Defect Map Viewer 와 같은 방식으로 DACrux 왼쪽 DMSearch 패널(메뉴 RESV_04 = DM)에서 Wafer 를 받고(iSEMControl.DrawWafer),
    /// 기존 Wafer 그리기 서비스(RO.DefectMapAnalysis)로 Map 을 그린 뒤, [분석] 으로 GDS Chain 영역 비교 결과를 받아
    /// Die 를 "Chain 영역 Defect 수" 구간 색으로 칠한다. Die 를 더블클릭하면 Die 상세 화면(화면 B, frmGdsDieAnalysis)으로 결과를 넘긴다.
    ///
    /// 처리 흐름
    ///  1) DrawWafer -> Wafer 콤보 -> LoadSelectedWafer -> ShowWafer(Map / Class 목록 / Device 자동 선택)
    ///  2) Device 선택 -> Chain 목록(저장 완료 READY 만 선택 가능)
    ///  3) [분석] -> RO.GdsKlarfAnalysis.AnalyzeWafer (백그라운드) -> ApplyResult(구간 / Map 색 / 목록 / 상태줄)
    ///  4) Die 클릭 = 목록 거르기, Die 더블클릭 / [상세 보기] = OpenDetail
    /// 서비스는 속성으로 바꿀 수 있다(기본 원격 호출, Smoke 검사는 같은 프로세스 BSL).
    /// </summary>
    public partial class frmGdsKlarfAnalysis : DACruxUXBasic01, iSEMControl
    {
        /// <summary>
        /// Die 상세 화면 메뉴 키(숨김 메뉴, 설계 결정 A9). 2026-10-12 부터 상세 화면을 여는 데는 쓰지 않는다(설계 14.1절, ShowForm 중복 생성 버그).
        /// 메뉴 행은 상세 화면이 활성화될 때 DACrux 가 왼쪽 DMSearch 패널을 닫는 기준(RESV_04 = DMS)으로만 남긴다.
        /// </summary>
        public const string DetailMenuKey = "MNU_GDS_DIE_ANALYSIS";

        private const string AllItemTag = "ALL";
        private const string NoneItemTag = "NONE";

        private long _stepSeq;
        private string _product = string.Empty;
        private string _stepFactory = string.Empty;
        private string _waferText = string.Empty;
        private DataSet _result;
        private DataTable _imagePaths;
        private bool _dirty;
        private bool _loading;
        private bool _running;
        private Point? _selectedDie;
        private List<GdsKlarfColorBand> _bands = GdsKlarfColorBands.Auto(1, 1);

        /// <summary>Wafer 그리기 / Defect 이미지 경로 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.DefectMapAnalysis DefectService { get; set; }

        /// <summary>GDS Map Device 목록 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsMapView MapService { get; set; }

        /// <summary>Chain 목록 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsChain ChainService { get; set; }

        /// <summary>연관 분석 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public RO.GdsKlarfAnalysis AnalysisService { get; set; }

        /// <summary>마지막 분석 결과(없으면 null). 화면 B 와 Smoke 검사가 읽는다.</summary>
        public DataSet AnalysisResult { get { return _result; } }

        /// <summary>현재 색 구간(복사본).</summary>
        public List<GdsKlarfColorBand> ColorBands { get { return _bands.Select(b => new GdsKlarfColorBand { From = b.From, To = b.To, Color = b.Color }).ToList(); } }

        public frmGdsKlarfAnalysis()
        {
            InitializeComponent();
            _loading = true;
            cboSide.SelectedIndex = 0;          // 설계 결정 A6: Back side 기본
            cboDefectScope.SelectedIndex = 0;
            _loading = false;
            map.OnChangeCurrentDie += map_OnChangeCurrentDie;
            ConfigureGrid(dgvDie);
            ConfigureGrid(dgvChain);
            ConfigureGrid(dgvDefect);
            RefreshBandGrid();
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
        /// 왼쪽 DMSearch [Search] 결과를 받는다(iSEMControl). Wafer 콤보에 넣고 첫 장을 그린다.
        /// 여러 장이 와도 분석은 1장씩 한다(설계 2절).
        /// </summary>
        public void DrawWafer(DPWafer[] wafer)
        {
            this.DPWaferList = wafer;
            _loading = true;
            cboWafer.Items.Clear();
            if (wafer != null)
                foreach (DPWafer w in wafer)
                    cboWafer.Items.Add(new WaferItem(w));
            _loading = false;
            if (cboWafer.Items.Count > 0)
                cboWafer.SelectedIndex = 0;
            else
                SetStatus("받은 Wafer 가 없습니다.");
        }

        private void cboWafer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadSelectedWafer();
        }

        /// <summary>선택한 Wafer 의 Defect / 기준 정보 / 검사 Die 를 기존 Defect Map Viewer 와 같은 서비스로 읽어 그린다.</summary>
        private void LoadSelectedWafer()
        {
            var item = cboWafer.SelectedItem as WaferItem;
            long step;
            if (item == null || !long.TryParse(item.Wafer.StepSeq, NumberStyles.Integer, CultureInfo.InvariantCulture, out step)) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                SetStatus("Wafer 를 조회 중입니다...");
                RO.DefectMapAnalysis svc = DefectService ?? new RO.DefectMapAnalysis();
                long[] steps = { step };
                var defects = new DefectList();
                defects.AddRange(svc.GetDefectMapViewer_DefectArray(steps));
                DataSet info = svc.GetDefectMapViewer_Info(steps);
                if (!info.Tables.Contains("STEP_INFO") || info.Tables["STEP_INFO"].Rows.Count == 0)
                    throw new InvalidOperationException("Wafer 기준 정보(TQD_INSP_INFO)가 없습니다.");
                DataRow stepRow = info.Tables["STEP_INFO"].Rows[0];
                DataTable mapTable = svc.GetDefectMapViewer_Map(steps, new[] { stepRow["SETUP_SEQ"].ToString() }, new[] { stepRow["TEST"].ToString() });
                ShowWafer(step, defects, info, mapTable);
                LoadImagePathsAsync(step);
            }
            catch (Exception ex)
            {
                SetStatus("Wafer 조회 실패: " + ex.Message);
                MessageBox.Show(this, ex.Message, "Wafer 조회", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Wafer Map 을 그린다. 설정 순서는 DPUCDefecMapViewer.Draw(DefectList) 와 같다(LeftBottom / Notch 0 / Pitch / Origin -> DieCalculation -> AddDie).
        /// 이전 분석 결과는 지운다. Smoke 검사도 이 함수를 직접 부른다.
        /// </summary>
        public void ShowWafer(long stepSeq, DefectList defects, DataSet info, DataTable mapTable)
        {
            DataRow stepRow = info.Tables["STEP_INFO"].Rows[0];
            DataRow setup = info.Tables["SETUP_INFO"].Rows[0];
            _stepSeq = stepSeq;
            _product = ColText(stepRow, "PRODUCT");
            _stepFactory = ColText(stepRow, "FACTORY");
            _waferText = ColText(stepRow, "WAFER_ID") + " / " + ColText(stepRow, "STEP_ID");
            _result = null;
            _dirty = false;
            _selectedDie = null;
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
            map.DefectSelectMode();
            map.Redraw();

            FillClassMenu(map.Defects);
            string deviceNote = SelectDeviceForProduct();
            ClearResultViews();
            SetStatus(String.Format("{0} / Defect {1} / 검사 Die {2}{3}. GDS Device / 보기 기준을 확인하고 [분석] 하세요.",
                _waferText, map.Defects.Count, mapTable == null ? 0 : mapTable.Rows.Count, deviceNote));
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

        #region [Device / Chain / Class 선택]

        /// <summary>DACrux 로그인 Factory. 없으면 KLARF Step 의 FACTORY.</summary>
        private string CurrentFactory
        {
            get
            {
                string factory = (GlobalVariable.Factory ?? string.Empty).Trim();
                return factory.Length > 0 ? factory : _stepFactory;
            }
        }

        /// <summary>READY GDS Map 이 있는 Device 를 콤보에 넣는다(로그인 Factory 기준).</summary>
        private void LoadDeviceList()
        {
            DataTable devices = (MapService ?? new RO.GdsMapView()).GetMapDeviceList();
            string factory = CurrentFactory;
            string current = cboDevice.SelectedItem as string;
            _loading = true;
            cboDevice.Items.Clear();
            foreach (DataRow row in devices.Rows)
                if (factory.Length == 0 || String.Equals(row["FACTORY"].ToString(), factory, StringComparison.OrdinalIgnoreCase))
                    cboDevice.Items.Add(row["DEVICE_ID"].ToString());
            _loading = false;
            if (current != null && cboDevice.Items.Contains(current)) cboDevice.SelectedItem = current;
        }

        /// <summary>
        /// KLARF Product 와 같은 이름의 GDS Device 가 있으면 고른다.
        /// 없으면 GDS Map 조회 화면에서 등록한 Sub Device 로 대표 Device 를 찾아 고른다(2026-10-12, 예: OMM_Hybrid -> U9).
        /// 그래도 없으면 이전 선택을 유지한다. 반환: 상태줄에 붙일 안내(" / Sub Device ..." 또는 빈 값)
        /// </summary>
        private string SelectDeviceForProduct()
        {
            if (cboDevice.Items.Count == 0) TryRun(LoadDeviceList);
            if (_product.Length > 0 && cboDevice.Items.Contains(_product))
            {
                cboDevice.SelectedItem = _product;
                return string.Empty;
            }
            string representative = _product.Length == 0 ? null : FindRepresentativeDevice(_product);
            if (representative != null && cboDevice.Items.Contains(representative))
            {
                cboDevice.SelectedItem = representative;
                return " / Sub Device " + _product + " -> GDS Device " + representative;
            }
            if (cboDevice.SelectedIndex < 0 && cboDevice.Items.Count > 0)
                cboDevice.SelectedIndex = 0;
            return _product.Length == 0 ? string.Empty : " / Product " + _product + " 와 연결된 GDS Device 없음(GDS Map 조회 > Sub Device 에서 등록)";
        }

        /// <summary>Product 이름을 Sub Device 로 등록한 대표 GDS Device(READY). 없거나 조회 실패면 null.</summary>
        private string FindRepresentativeDevice(string product)
        {
            try
            {
                DataTable found = (MapService ?? new RO.GdsMapView()).GetMapDeviceBySub(CurrentFactory, product);
                return found.Rows.Count == 0 ? null : found.Rows[0]["DEVICE_ID"].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Sub Device 대표 찾기 실패: {0}", ex.Message);
                return null;
            }
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            TryRun(LoadChainMenu);
            Condition_Changed(sender, e);
        }

        /// <summary>Device 의 Chain 목록을 체크 메뉴로 만든다. 저장 미완료(DRAFT) Chain 은 고를 수 없다.</summary>
        private void LoadChainMenu()
        {
            menuChain.Items.Clear();
            string device = cboDevice.SelectedItem as string;
            if (String.IsNullOrEmpty(device)) { UpdateCheckButton(btnChain, menuChain, "Chain"); return; }
            DataTable chains = (ChainService ?? new RO.GdsChain()).GetChainList(CurrentFactory, device);
            AddAllNoneItems(menuChain);
            foreach (DataRow row in chains.Rows)
            {
                bool ready = row["VERSION_STATUS"].ToString() == "READY";
                var item = new ToolStripMenuItem(String.Format("{0}  {1}  (Member {2}){3}", row["CHAIN_CODE"], row["CHAIN_NAME"], row["MEMBER_COUNT"],
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

        /// <summary>이 Wafer 의 Defect Class 를 체크 메뉴로 만든다(전부 체크). 이름은 Class 코드 충돌(TQD_DEFECT_TYPE) 때문에 번호와 개수만 쓴다.</summary>
        private void FillClassMenu(DefectList defects)
        {
            menuClass.Items.Clear();
            AddAllNoneItems(menuClass);
            foreach (var g in defects.GroupBy(d => d.CLASSNUMBER).OrderBy(g => g.Key))
                menuClass.Items.Add(new ToolStripMenuItem(String.Format("Class {0}  ({1}개)", g.Key, g.Count()))
                {
                    CheckOnClick = true,
                    Checked = true,
                    Tag = g.Key.ToString(CultureInfo.InvariantCulture)
                });
            UpdateCheckButton(btnClass, menuClass, "Class");
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
            UpdateCheckButton(menu == menuClass ? btnClass : btnChain, menu, menu == menuClass ? "Class" : "Chain");
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
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                DataSet ds = null;
                Exception failure = null;
                try { ds = Call(svc, request); }
                catch (Exception ex) { failure = ex; }
                try
                {
                    if (IsHandleCreated && !IsDisposed)
                        BeginInvoke(new Action(() => FinishAnalysis(request, ds, failure)));
                }
                catch (InvalidOperationException)
                {
                    // 화면이 닫히는 중이면 결과를 버린다.
                }
            });
        }

        /// <summary>Smoke 검사용: 현재 조건으로 같은 스레드에서 분석하고 화면에 반영한다. 조건 오류면 예외.</summary>
        public DataSet AnalyzeNow()
        {
            AnalysisRequest request;
            string error = BuildRequest(out request);
            if (error != null) throw new InvalidOperationException(error);
            DataSet ds = Call(AnalysisService ?? new RO.GdsKlarfAnalysis(), request);
            ApplyResult(ds);
            return ds;
        }

        /// <summary>화면 조건을 확인해 요청을 만든다. 문제가 있으면 안내 문구를 돌려준다.</summary>
        private string BuildRequest(out AnalysisRequest request)
        {
            request = null;
            if (_stepSeq <= 0) return "왼쪽 Wafer 조회에서 Wafer 를 먼저 [Search] 하세요.";
            string device = cboDevice.SelectedItem as string;
            if (String.IsNullOrEmpty(device)) return "GDS Device 를 고르세요. (KLARF Product: " + _product + ")";
            string[] chains = CheckedTags(menuChain);
            if (chains != null && chains.Length == 0) return "비교할 Chain 을 1개 이상 고르세요.";
            string[] classes = CheckedTags(menuClass);
            if (classes != null && classes.Length == 0) return "Defect Class 를 1개 이상 고르세요.";
            request = new AnalysisRequest
            {
                Factory = CurrentFactory,
                Device = device,
                StepSeq = _stepSeq,
                Side = cboSide.SelectedIndex == 1 ? "FRONT" : "BACK",
                Tolerance = (double)numTolerance.Value,
                UseSize = chkDefectSize.Checked,
                OffsetX = (double)numOffsetX.Value,
                OffsetY = (double)numOffsetY.Value,
                Chains = chains,
                Classes = classes == null ? null : classes.Select(c => int.Parse(c, CultureInfo.InvariantCulture)).ToArray()
            };
            return null;
        }

        private static DataSet Call(RO.GdsKlarfAnalysis svc, AnalysisRequest r)
        {
            return svc.AnalyzeWafer(r.Factory, r.Device, r.StepSeq, r.Side, r.Tolerance, r.UseSize, r.OffsetX, r.OffsetY, r.Chains, r.Classes);
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
            ApplyResult(ds);
        }

        private void SetRunning(bool running)
        {
            _running = running;
            btnAnalyze.Enabled = !running;
            Cursor = running ? Cursors.WaitCursor : Cursors.Default;
            if (running) SetStatus("분석 중입니다... (Chain Member 영역과 KLARF Defect 비교)");
        }

        /// <summary>분석 결과를 화면에 반영한다: 자동 구간 -> Map 색 -> 구간 표 -> 목록 -> 상태줄.</summary>
        private void ApplyResult(DataSet ds)
        {
            _result = ds;
            _dirty = false;
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxChainCount());
            if (_selectedDie.HasValue && FindDieRow(_selectedDie.Value) == null) _selectedDie = null;
            ApplyMapColors();
            BindDieGrid();
            RefreshLists();
            UpdateStatus();
        }

        private int Threshold { get { return (int)numThreshold.Value; } }

        private int MaxChainCount()
        {
            if (_result == null) return 0;
            return _result.Tables["DIE"].Rows.Cast<DataRow>().Select(r => System.Convert.ToInt32(r["IN_CHAIN_COUNT"])).DefaultIfEmpty(0).Max();
        }

        /// <summary>Die 채움색 / 숫자를 Map 에 넣고 구간 표의 Die 수를 갱신한다.</summary>
        private void ApplyMapColors()
        {
            var fills = new Dictionary<Point, Color>();
            var labels = new Dictionary<Point, int>();
            if (_result != null)
            {
                foreach (DataRow row in _result.Tables["DIE"].Rows)
                {
                    var key = new Point(System.Convert.ToInt32(row["XINDEX"]), System.Convert.ToInt32(row["YINDEX"]));
                    int total = System.Convert.ToInt32(row["DEFECT_COUNT"]);
                    int inChain = System.Convert.ToInt32(row["IN_CHAIN_COUNT"]);
                    fills[key] = GdsKlarfColorBands.DieColor(total, inChain, Threshold, _bands);
                    if (total > 0) labels[key] = inChain;
                }
            }
            map.SetDieDisplay(fills, labels);
            RefreshBandGrid();
        }

        #endregion

        #region [색 구간 표]

        /// <summary>
        /// 구간 표를 다시 채운다. 마지막 두 줄(기준 미만 / Defect 없음)은 고정 안내 행이라 고칠 수 없다.
        /// Die 수는 현재 분석 결과 기준이다.
        /// </summary>
        private void RefreshBandGrid()
        {
            dgvBand.Rows.Clear();
            List<int[]> dies = _result == null ? new List<int[]>() : _result.Tables["DIE"].Rows.Cast<DataRow>()
                .Select(r => new[] { System.Convert.ToInt32(r["DEFECT_COUNT"]), System.Convert.ToInt32(r["IN_CHAIN_COUNT"]) }).ToList();
            foreach (GdsKlarfColorBand band in _bands)
            {
                int n = dies.Count(d => d[0] > 0 && d[1] >= Threshold && band.Contains(d[1]));
                int i = dgvBand.Rows.Add(band.From, band.To == GdsKlarfColorBands.OpenEnd ? "이상" : band.To.ToString(CultureInfo.InvariantCulture), "", n);
                PaintColorCell(dgvBand.Rows[i], band.Color);
            }
            AddFixedBandRow("기준 미만", GdsKlarfColorBands.BelowThresholdColor, dies.Count(d => d[0] > 0 && d[1] < Threshold));
            AddFixedBandRow("Defect 없음", GdsKlarfColorBands.NoDefectColor, dies.Count(d => d[0] == 0));
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
        /// DataGridView 가 현재 칸을 옮기는 도중(SetCurrentCellAddressCore)에 Rows.Clear 를 하면 재진입 예외가 나서(2026-10-12 사용자 실행 중 발견)
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
            _bands = GdsKlarfColorBands.Auto(Threshold, MaxChainCount());
            ApplyMapColors();
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxChainCount());
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

        /// <summary>Die 탭: Defect 가 있는 Die 만, 서버 정렬(Chain 영역 내림차순) 그대로.</summary>
        private void BindDieGrid()
        {
            var table = new DataTable();
            table.Columns.Add("Die", typeof(string));
            table.Columns.Add("전체", typeof(int));
            table.Columns.Add("Chain 영역", typeof(int));
            table.Columns.Add("Chain 밖", typeof(int));
            table.Columns.Add("Chain 수", typeof(int));
            table.Columns.Add("주요 Chain", typeof(string));
            table.Columns.Add("검사", typeof(string));
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            if (_result != null)
                foreach (DataRow r in _result.Tables["DIE"].Rows)
                    if (System.Convert.ToInt32(r["DEFECT_COUNT"]) > 0)
                        table.Rows.Add(DieText(r["XINDEX"], r["YINDEX"]), r["DEFECT_COUNT"], r["IN_CHAIN_COUNT"], r["OUT_CHAIN_COUNT"],
                            r["HIT_CHAIN_COUNT"], r["TOP_CHAINS"], r["IN_PLAN"], r["XINDEX"], r["YINDEX"]);
            dgvDie.DataSource = table;
            if (dgvDie.Columns.Contains("X")) dgvDie.Columns["X"].Visible = false;
            if (dgvDie.Columns.Contains("Y")) dgvDie.Columns["Y"].Visible = false;
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

        /// <summary>Chain 탭: [Wafer 전체] 또는 선택 Die 가 없으면 Chain 별 합계, 아니면 선택 Die 의 (Die, Chain).</summary>
        private void BindChainGrid()
        {
            var table = new DataTable();
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("이름", typeof(string));
            table.Columns.Add("Defect 수", typeof(int));
            table.Columns.Add("Die 수", typeof(int));
            table.Columns.Add("Defect ID", typeof(string));
            table.Columns.Add("Member", typeof(int));
            if (_result != null)
            {
                Dictionary<string, DataRow> chains = _result.Tables["CHAIN"].Rows.Cast<DataRow>().ToDictionary(r => r["CHAIN_SEQ"].ToString());
                if (chkChainAll.Checked || !_selectedDie.HasValue)
                {
                    foreach (DataRow c in chains.Values.OrderByDescending(r => System.Convert.ToInt32(r["HIT_DEFECT_COUNT"])))
                        table.Rows.Add(c["CHAIN_CODE"], c["CHAIN_NAME"], c["HIT_DEFECT_COUNT"], c["HIT_DIE_COUNT"], "", c["MEMBER_COUNT"]);
                }
                else
                {
                    foreach (DataRow dc in DieRows(_result.Tables["DIE_CHAIN"]))
                    {
                        DataRow c = chains[dc["CHAIN_SEQ"].ToString()];
                        table.Rows.Add(dc["CHAIN_CODE"], c["CHAIN_NAME"], dc["DEFECT_COUNT"], 1, dc["DEFECT_IDS"], c["MEMBER_COUNT"]);
                    }
                }
            }
            dgvChain.DataSource = table;
        }

        /// <summary>Defect 탭: [Chain 영역] / [Chain 밖] / [전체], [Wafer 전체] 가 아니면 선택 Die 만.</summary>
        private void BindDefectGrid()
        {
            var table = new DataTable();
            table.Columns.Add("Defect ID", typeof(long));
            table.Columns.Add("Die", typeof(string));
            table.Columns.Add("Class", typeof(int));
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("XSIZE", typeof(double));
            table.Columns.Add("YSIZE", typeof(double));
            table.Columns.Add("XREL", typeof(double));
            table.Columns.Add("YREL", typeof(double));
            table.Columns.Add("GDS X", typeof(double));
            table.Columns.Add("GDS Y", typeof(double));
            if (_result != null)
            {
                IEnumerable<DataRow> rows = chkDefectAll.Checked || !_selectedDie.HasValue
                    ? _result.Tables["DEFECT"].Rows.Cast<DataRow>()
                    : DieRows(_result.Tables["DEFECT"]);
                foreach (DataRow r in rows)
                {
                    bool inChain = r["IN_CHAIN"].ToString() == "Y";
                    if ((cboDefectScope.SelectedIndex == 0 && !inChain) || (cboDefectScope.SelectedIndex == 1 && inChain)) continue;
                    table.Rows.Add(r["DEFECTID"], DieText(r["XINDEX"], r["YINDEX"]), r["CLASSNUMBER"], inChain ? r["CHAIN_CODES"] : "(밖)",
                        r["XSIZE"], r["YSIZE"], r["XREL"], r["YREL"], r["GDS_X"], r["GDS_Y"]);
                }
            }
            dgvDefect.DataSource = table;
            tabDefect.Text = "Defect (" + table.Rows.Count + ")";
        }

        /// <summary>Image 탭: 선택 Die(없으면 비움)의 Defect 이미지를 기존 DPUCDefectInfo 썸네일로 보여 준다(FTP 다운로드는 컨트롤이 한다).</summary>
        private void RefreshImages()
        {
            if (tabList.SelectedTab != tabImage) return;
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
                    flowImage.Controls.Add(new DACrux.SEMDMS.Control.DPUCDefectInfo(d.DEFECTID.ToString(), path, d.XINDEX.ToString(), d.YINDEX.ToString(),
                        d.CLASSNUMBER.ToString(), d.CLUSTERNUMBER.ToString(), d.XREL.ToString(), d.YREL.ToString(), d.XSIZE.ToString(), d.YSIZE.ToString()));
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
            tabList.SelectedTab = tabImage;
            while (flowImage.Controls.Count > 0) flowImage.Controls[0].Dispose();
            ShowImages(map.Defects.Where(d => d.DEFECTID == id).ToDictionary(d => (long)d.DEFECTID));
            if (flowImage.Controls.Count == 0) SetStatus("Defect " + id + " 이미지가 없습니다(또는 이미지 경로 조회 전).");
        }

        private IEnumerable<DataRow> DieRows(DataTable table)
        {
            Point die = _selectedDie.Value;
            return table.Rows.Cast<DataRow>().Where(r => System.Convert.ToInt32(r["XINDEX"]) == die.X && System.Convert.ToInt32(r["YINDEX"]) == die.Y);
        }

        private DataRow FindDieRow(Point die)
        {
            if (_result == null) return null;
            return _result.Tables["DIE"].Rows.Cast<DataRow>()
                .FirstOrDefault(r => System.Convert.ToInt32(r["XINDEX"]) == die.X && System.Convert.ToInt32(r["YINDEX"]) == die.Y);
        }

        /// <summary>Die 를 고른다: Map 빨간 테두리 / Die 탭 행 / 다른 탭 거르기 / 상태줄.</summary>
        public void SelectDie(int x, int y)
        {
            _selectedDie = new Point(x, y);
            map.SetSelectedDie(x, y);
            map.Redraw();
            SyncDieGridSelection();
            RefreshLists();
            UpdateStatus();
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

        /// <summary>
        /// 선택 Chip 의 상세 화면(화면 B)을 연다(설계 7.3 / 14.1절). 상세 화면은 하나만 두고, 다른 Chip 을 고르면 같은 화면에 넘긴다
        /// (GDS 도면을 다시 읽지 않음, 탭 제목 = "상세보기 (X, Y)").
        /// DACrux ShowForm 은 ENGUI DLL 을 Assembly.Load(byte[]) 로 새로 읽어 다른 어셈블리 사본의 화면을 만든다. 그 화면은 형식으로 찾을 수 없어
        /// 상세 화면이 두 개 생겼으므로(2026-10-12 사용자 확인 요청 -> 버그) ShowForm 을 쓰지 않고 이 화면이 직접 만든다.
        /// </summary>
        private void OpenDetail(int x, int y)
        {
            if (_result == null)
            {
                MessageBox.Show(this, "먼저 [분석] 하세요.", "Detail Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dirty) SetStatus("조건이 바뀌었지만 상세 화면에는 마지막 분석 결과를 넘깁니다. 새 조건은 [분석] 후 다시 여세요.");
            Form detail = FindDetail();
            if (detail == null)
            {
                var created = new frmGdsDieAnalysis();
                if (MdiParent != null) created.MdiParent = MdiParent;
                // 탭 제목과 넘길 값을 먼저 정한다. GDS 상세 Form 은 Shown 때 붙고 그때 받은 값을 넘긴다.
                created.ShowDie(_result, x, y);
                created.Show();
                return;
            }
            var typed = detail as frmGdsDieAnalysis;
            if (typed != null)
                typed.ShowDie(_result, x, y);
            else
                detail.GetType().GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) }).Invoke(detail, new object[] { _result, x, y });
            detail.Activate();
        }

        /// <summary>열린 상세 화면. 다른 어셈블리 사본으로 만든 화면도 찾도록 형식 이름으로 비교한다.</summary>
        private Form FindDetail()
        {
            if (MdiParent == null) return null;
            string name = typeof(frmGdsDieAnalysis).FullName;
            return MdiParent.MdiChildren.FirstOrDefault(f => !f.IsDisposed && f.GetType().FullName == name);
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

        private void chkDieIndex_CheckedChanged(object sender, EventArgs e)
        {
            map.ShowDieIndexLabel = chkDieIndex.Checked;
            map.Redraw();
        }

        /// <summary>마우스가 다른 Die 로 가면 그 Die 의 분석 숫자를 상태줄 둘째 줄에 보여 준다(Map 에 툴팁 기능이 없어 기존 화면과 같이 상태줄 사용).</summary>
        private void map_OnChangeCurrentDie(object sender, Die die)
        {
            // DACrux.Base.Die 는 == 연산자를 Equals 로 재정의했고 Equals 가 null 을 처리하지 않는다(die == null 이면 NullReferenceException).
            // 그래서 연산자를 거치지 않는 ReferenceEquals 로 확인한다. (2026-10-12 사용자 실행 중 발견)
            if (ReferenceEquals(die, null) || _result == null) return;
            UpdateStatus(die.IndexX, die.IndexY);
        }

        private void UpdateStatus()
        {
            if (_selectedDie.HasValue) UpdateStatus(_selectedDie.Value.X, _selectedDie.Value.Y);
            else UpdateStatus(int.MinValue, int.MinValue);
        }

        /// <summary>첫 줄 = 분석 기준(설계 7.2 ANL_INFO), 둘째 줄 = Die 정보.</summary>
        private void UpdateStatus(int x, int y)
        {
            if (_result == null) return;
            DataRow i = _result.Tables["ANL_INFO"].Rows[0];
            string line1 = String.Format("{0} / GDS {1}(저장 {2}) -> {3} 보기 / 허용오차 {4} µm{5} / Chain {6}{7} (최종 저장 {8}) / Defect {9} 중 Chain 영역 {10} / {11:0.0}초{12}",
                i["WAFER_ID"], i["DEVICE_ID"], i["MAP_VIEW_SIDE"], i["VIEW_SIDE"], i["TOLERANCE_UM"], i["USE_DEFECT_SIZE"].ToString() == "Y" ? " + 크기" : "",
                i["CHAIN_COUNT"], System.Convert.ToInt32(i["DRAFT_CHAIN_COUNT"]) > 0 ? " (미완료 " + i["DRAFT_CHAIN_COUNT"] + " 제외)" : "",
                i["CHAIN_LAST_SAVED"], i["DEFECT_COUNT"], i["IN_CHAIN_DEFECT_COUNT"], System.Convert.ToInt64(i["ELAPSED_MS"]) / 1000.0,
                _dirty ? "   ※ 조건 변경됨 - [분석] 필요" : "");
            string line2 = "Chip 을 클릭(또는 우클릭)해 선택하면 목록을 거르고, 우클릭 [Detail Analysis] 또는 더블클릭으로 상세 화면을 엽니다.";
            if (x != int.MinValue)
            {
                DataRow die = FindDieRow(new Point(x, y));
                line2 = die == null
                    ? String.Format("Die ({0}, {1}) 검사 계획 밖", x, y)
                    : String.Format("Die ({0}, {1}) 전체 {2} / Chain 영역 {3} / 밖 {4} / {5}", x, y, die["DEFECT_COUNT"], die["IN_CHAIN_COUNT"],
                        die["OUT_CHAIN_COUNT"], die["TOP_CHAINS"].ToString().Length == 0 ? "걸린 Chain 없음" : die["TOP_CHAINS"]);
            }
            lblStatus.Text = line1 + Environment.NewLine + line2;
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
            RefreshBandGrid();
            BindDieGrid();
            RefreshLists();
        }

        #endregion

        #region [Helper]

        private static string DieText(object x, object y)
        {
            return "(" + x + ", " + y + ")";
        }

        private static string ColText(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? row[column].ToString().Trim() : string.Empty;
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

        /// <summary>Wafer 콤보 항목.</summary>
        private sealed class WaferItem
        {
            public WaferItem(DPWafer wafer)
            {
                Wafer = wafer;
            }

            public DPWafer Wafer { get; private set; }

            public override string ToString()
            {
                return String.Format("{0}  ({1} / {2})", Wafer.WaferID, Wafer.LotID, Wafer.Product);
            }
        }

        /// <summary>분석 요청 값(백그라운드 스레드로 넘기기 위해 화면 값을 복사한다).</summary>
        private sealed class AnalysisRequest
        {
            public string Factory;
            public string Device;
            public long StepSeq;
            public string Side;
            public double Tolerance;
            public bool UseSize;
            public double OffsetX;
            public double OffsetY;
            public string[] Chains;
            public int[] Classes;
        }

        #endregion
    }
}
