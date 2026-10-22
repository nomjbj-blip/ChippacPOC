using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View U8 (2026-10-20, 설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md).
    /// U8 처럼 같은 형태의 연결 묶음(TSV)이 다이 전체에 반복되는 Device 의 Chain 을 한 번에 만드는 화면이다.
    /// 기존 GDS Map View(GdsMapViewForm)의 U9 Chain 연결 기능(Input / Output / 후보 경로 / 도형 추가·제외 / 유사 묶음)은 넣지 않는다.
    ///
    /// 처리 흐름
    ///  화면 표시 -> Device 목록 -> Device 선택 -> Map 정보 + Layer 목록
    ///  [조회] 체크한 Layer 만 페이지로 받아 도면 표시 + Label 색인
    ///  [예시 지정] 도형 클릭 -> 그 도형과 닿는 도형 덩어리 = 예시 (GdsMapViewU8Form_Template.cs)
    ///  [같은 형태 찾기] 도면 전체 덩어리를 예시와 비교 -> 찾기 결과 (GdsMapViewU8Form_Extract.cs)
    ///  [찾기 결과] 탭: 분류 / 찾기로 거르고 [목록에서 빼기] -> [Chain 생성(DB)] 지금 목록의 일치 덩어리를 일괄 저장
    ///  [저장된 Chain] 탭: 목록 / 선택 표시 / 삭제 (GdsMapViewU8Form_ChainDb.cs)
    /// Map 도형(LAYER / PLACED)은 바꾸지 않는다. Map 복사 / 삭제는 기존 GDS Map View 에서 한다.
    /// </summary>
    public partial class GdsMapViewU8Form : Form
    {
        /// <summary>서비스 한 번에 받는 도형 수(기존 GDS Map View 와 같다).</summary>
        private const int PageRows = 10000;

        private DataTable _deviceTable;
        private CancellationTokenSource _loadCancel;
        private bool _updatingLayerChecks;
        /// <summary>지금 도면에 올린 Layer 키.</summary>
        private readonly HashSet<int> _loadedLayers = new HashSet<int>();
        /// <summary>Layer 키별 DB 도형 수(목록 표시용).</summary>
        private readonly Dictionary<int, long> _layerCounts = new Dictionary<int, long>();
        private string _loadSummary;
        /// <summary>선택한 Device 의 READY Map. 없거나 READY 가 아니면 null.</summary>
        private MapItem _currentMap;
        /// <summary>지금 도면에 그려진 Map. Chain 저장 대상이다.</summary>
        private MapItem _shownMap;
        private string _displaySide = GdsSideTransform.Front;
        private List<MapPlacedElementData> _shownItems;
        private Dictionary<int, Color> _shownColors;
        private bool _changingViewSide;
        /// <summary>로그인 Factory(화면을 열 때 한 번 읽음).</summary>
        private string _factory = string.Empty;

        /// <summary>조회 서비스. null 이면 원격 호출 RO 를 만든다(Smoke 검사는 같은 프로세스 BSL 을 넣은 RO 를 쓴다).</summary>
        public DACrux.SEMDMS.RO.GdsMapView MapViewService { get; set; }

        /// <summary>Chain 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public DACrux.SEMDMS.RO.GdsChain ChainService { get; set; }

        private DACrux.SEMDMS.RO.GdsMapView MapViewRo()
        {
            return MapViewService ?? new DACrux.SEMDMS.RO.GdsMapView();
        }

        private DACrux.SEMDMS.RO.GdsChain ChainRo()
        {
            return ChainService ?? new DACrux.SEMDMS.RO.GdsChain();
        }

        public GdsMapViewU8Form()
        {
            InitializeComponent();
            GridBuffer.Enable(this);
            _changingViewSide = true;
            cboViewSide.SelectedIndex = 0;
            _changingViewSide = false;
            InitializeLabelSearchState();
            InitializeTemplateState();
            InitializeExtractState();
            InitializeChainDbState(false);
        }

        /// <summary>DACrux 호스트(frmGdsMapViewU8)가 쓰는 생성자. Chain DB 저장 / 목록은 호스트 안에서만 쓴다.</summary>
        public GdsMapViewU8Form(bool hostedByDms) : this()
        {
            InitializeChainDbState(hostedByDms);
        }

        /// <summary>콤보에서 고른 보기 기준(0 Front / 1 Back).</summary>
        private string SelectedViewSide
        {
            get { return cboViewSide.SelectedIndex == 1 ? GdsSideTransform.Back : GdsSideTransform.Front; }
        }

        /// <summary>지금 화면이 저장 기준과 다른 기준(반전)으로 그려져 있는지.</summary>
        private bool DisplayMirrored
        {
            get { return _shownMap != null && GdsSideTransform.NeedsMirror(_shownMap.ViewSide, _displaySide); }
        }

        /// <summary>화면 X -> 저장 기준 X(Chain Marker 저장용).</summary>
        private double ToStoredX(double displayX)
        {
            return DisplayMirrored ? GdsSideTransform.MirrorX(displayX) : displayX;
        }

        /// <summary>저장 기준 X -> 화면 X(Label 색인 표시용). 변환이 부호 반전이라 ToStoredX 와 같은 식이다.</summary>
        private double ToDisplayX(double storedX)
        {
            return DisplayMirrored ? GdsSideTransform.MirrorX(storedX) : storedX;
        }

        #region [조회 조건]

        /// <summary>화면이 보인 뒤 로그인 Factory 의 READY Map Device 목록을 읽는다.</summary>
        private async void GdsMapViewU8Form_Shown(object sender, EventArgs e)
        {
            _factory = DmsHostContext.Factory;
            if (_factory.Length == 0)
            {
                SetStatus("Factory 정보가 없습니다. DACrux에 로그인한 상태에서 메뉴로 여세요.");
                return;
            }
            SetStatus("Device 목록 조회 중 (Factory " + _factory + ")");
            try
            {
                using (ViewBusy.Busy())
                    _deviceTable = await Task.Run(() => MapViewRo().GetMapDeviceList());
                FillDeviceList();
                SetStatus(cboDevice.Items.Count == 0 ? "Factory " + _factory + "에 조회 가능한 GDS Map이 없습니다." : "Device " + cboDevice.Items.Count + "개 (Factory " + _factory + ")");
            }
            catch (Exception ex)
            {
                ShowError("Device 목록 조회", ex);
            }
        }

        /// <summary>로그인 Factory 의 Device 만 목록에 넣는다.</summary>
        private void FillDeviceList()
        {
            cboDevice.Items.Clear();
            cboDevice.Text = string.Empty;
            ClearMapInfo();
            if (_deviceTable == null) return;
            foreach (DataRow row in _deviceTable.Rows)
                if (string.Equals(row["FACTORY"].ToString(), _factory, StringComparison.Ordinal))
                    cboDevice.Items.Add(row["DEVICE_ID"].ToString());
            if (cboDevice.Items.Count > 0) cboDevice.SelectedIndex = 0;
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMapInfo();
        }

        /// <summary>목록에 없는 Device 를 직접 입력하고 Enter 를 누르면 Map 정보를 읽는다.</summary>
        private void cboDevice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) LoadMapInfo();
        }

        /// <summary>Map 헤더를 읽고 READY 면 Layer 목록(저장 색상 / 도형 수)을 채운다. 기다리는 동안 선택이 바뀌면 결과를 버린다.</summary>
        private async void LoadMapInfo()
        {
            ClearMapInfo();
            string factory = _factory;
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0) return;
            SetStatus("Map 정보 조회 중");
            try
            {
                using (ViewBusy.Busy())
                {
                    var service = MapViewRo();
                    DataTable info = await Task.Run(() => service.GetMapInfo(factory, device));
                    if (device != cboDevice.Text.Trim()) return;
                    if (info.Rows.Count == 0)
                    {
                        SetStatus("등록된 GDS Map이 없습니다: " + factory + " / " + device);
                        return;
                    }
                    var item = new MapItem(info.Rows[0]);
                    toolTipView.SetToolTip(cboDevice, item.Describe());
                    if (!item.IsReady)
                    {
                        SetStatus("저장이 끝나지 않은 Map (상태 " + item.Status + ")");
                        return;
                    }

                    DataTable layers = await Task.Run(() => service.GetMapLayerList(factory, device));
                    if (device != cboDevice.Text.Trim()) return;
                    _updatingLayerChecks = true;
                    chkLayerItems.BeginUpdate();
                    try
                    {
                        foreach (DataRow row in layers.Rows)
                        {
                            int layerKey = GdsLayerKey.FromRow(row);
                            _layerCounts[layerKey] = Convert.ToInt64(row["PLACED_COUNT"], CultureInfo.InvariantCulture);
                            var color = Color.FromArgb(Convert.ToInt32(row["COLOR_ARGB"], CultureInfo.InvariantCulture));
                            chkLayerItems.Items.Add(new LayerDisplayItem(layerKey, color, true), true);
                        }
                    }
                    finally
                    {
                        chkLayerItems.EndUpdate();
                        _updatingLayerChecks = false;
                    }
                    _currentMap = item;
                    SetStatus("Layer " + chkLayerItems.Items.Count + "개 - 찾을 도형 Layer 와 Label Layer 만 체크하고 [조회]");
                }
            }
            catch (Exception ex)
            {
                ShowError("Map 정보 조회", ex);
            }
        }

        /// <summary>Device 가 바뀌면 이전 도면 / 예시 / 찾기 결과 / 저장 목록을 비운다.</summary>
        private void ClearMapInfo()
        {
            _currentMap = null;
            _shownMap = null;
            _shownItems = null;
            _shownColors = null;
            ClearTemplate();
            ClearElementIndex();
            ClearExtractResults();
            ClearSavedChains();
            ClearLabelIndex();
            toolTipView.SetToolTip(cboDevice, string.Empty);
            ClearLayerList();
            UpdateU8Buttons();
        }

        private void ClearLayerList()
        {
            _updatingLayerChecks = true;
            chkLayerItems.Items.Clear();
            _updatingLayerChecks = false;
            _layerCounts.Clear();
            _loadedLayers.Clear();
        }

        #endregion

        #region [도면 조회]

        /// <summary>
        /// 체크한 Layer 의 도형을 받아 도면을 다시 그린다. 다른 Map 이면 Label 색인도 함께 받는다.
        /// 같은 Map 을 다시 조회해도 도형 목록이 바뀌므로 예시 / 찾기 결과는 비운다(덩어리를 새 도형으로 다시 계산해야 한다).
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            MapItem target = _currentMap;
            if (target == null)
            {
                MessageBox.Show(this, "조회할 수 있는 GDS Map이 있는 Device를 선택하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var layers = chkLayerItems.CheckedItems.Cast<LayerDisplayItem>().ToList();
            if (layers.Count == 0)
            {
                MessageBox.Show(this, "조회할 Layer를 하나 이상 체크하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_extractRunning || _chainDbBusy) return;

            bool sameMap = _shownMap != null && _shownMap.Factory == target.Factory && _shownMap.DeviceId == target.DeviceId;
            GdsLabelIndex labelIndex = null;
            string labelIndexError = null;
            if (!sameMap)
            {
                SetStatus("Label 색인 조회 중");
                btnSearch.Enabled = false;
                try
                {
                    using (ViewBusy.Busy())
                        labelIndex = await Task.Run(() => ReadLabelIndex(MapViewRo(), target.Factory, target.DeviceId));
                }
                catch (Exception ex)
                {
                    labelIndexError = ex.Message;
                }
                finally
                {
                    btnSearch.Enabled = true;
                }
            }

            long total = layers.Sum(l => _layerCounts.ContainsKey(l.LayerKey) ? _layerCounts[l.LayerKey] : 0);
            var colors = layers.ToDictionary(l => l.LayerKey, l => l.Color);
            var progress = new Progress<long>(received => ReportLoadProgress(received, total));
            DACrux.SEMDMS.RO.GdsMapView pageService = MapViewRo();
            _loadCancel = new CancellationTokenSource();
            CancellationToken token = _loadCancel.Token;
            SetLoading(true);
            try
            {
                List<MapPlacedElementData> items = await Task.Run(
                    () => LoadPlacedElements(pageService, target.Factory, target.DeviceId, layers.Select(l => l.LayerKey).ToList(), progress, token));

                string view = SelectedViewSide;
                if (GdsSideTransform.NeedsMirror(target.ViewSide, view))
                    items = GdsSideTransform.MirrorAll(items);

                ClearTemplate();
                ClearExtractResults();
                _showLoadProgress = true;
                map.ShowPlacedElements(target.TopStructure, colors, items);
                _loadedLayers.Clear();
                foreach (var layer in layers) _loadedLayers.Add(layer.LayerKey);
                _shownMap = target;
                _shownItems = items;
                _shownColors = colors;
                _displaySide = view;
                BuildElementIndex();
                if (!sameMap)
                {
                    ReplaceLabelIndex(labelIndex, labelIndexError);
                    ClearSavedChains();
                }
                chkLayerItems.Refresh();
                _loadSummary = "조회 완료: " + target.DeviceId + " / 도형 " + items.Count.ToString("N0")
                    + " - [예시 지정] 후 Chain 한 묶음의 도형을 클릭하세요.";
                SetStatus(_loadSummary);
                if (!sameMap && _chainDbEnabled)
                    await RefreshSavedChainsAsync(false);
            }
            catch (OperationCanceledException)
            {
                SetStatus("조회를 중지했습니다. 화면의 도면은 이전 상태입니다.");
            }
            catch (Exception ex)
            {
                ShowError("도면 조회", ex);
            }
            finally
            {
                SetLoading(false);
                _loadCancel.Dispose();
                _loadCancel = null;
            }
        }

        /// <summary>Layer 키마다 이전 페이지 마지막 ID 다음부터 PageRows 씩 받아 행을 해석한다(백그라운드).</summary>
        private static List<MapPlacedElementData> LoadPlacedElements(DACrux.SEMDMS.RO.GdsMapView service, string factory, string deviceId,
            IList<int> layerKeys, IProgress<long> progress, CancellationToken token)
        {
            var items = new List<MapPlacedElementData>();
            foreach (int layerKey in layerKeys)
            {
                string after = string.Empty;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    int rowCount;
                    using (DataTable page = service.GetPlacedElementPage(factory, deviceId, GdsLayerKey.Format(layerKey), after, PageRows))
                    {
                        foreach (DataRow row in page.Rows)
                            items.Add(GdsMapPlacedRowReader.Read(row));
                        rowCount = page.Rows.Count;
                        if (rowCount > 0)
                            after = page.Rows[rowCount - 1]["PLACED_ELEMENT_ID"].ToString();
                    }
                    progress.Report(items.Count);
                    if (rowCount < PageRows) break;
                }
            }
            return items;
        }

        /// <summary>[중지]: 지금 받는 페이지가 끝나면 멈춘다.</summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_loadCancel != null) _loadCancel.Cancel();
            btnStop.Enabled = false;
            SetStatus("중지 요청: 현재 페이지가 끝나면 멈춥니다.");
        }

        private void ReportLoadProgress(long received, long total)
        {
            int percent = total <= 0 ? 0 : (int)Math.Min(100, received * 100 / total);
            progressLoad.Style = ProgressBarStyle.Continuous;
            progressLoad.Value = percent;
            SetStatus("도면을 불러오는 중입니다.");
        }

        /// <summary>
        /// 보기 기준 변경. 도형을 좌우 반전해 다시 그린다. 덩어리 좌표가 바뀌므로 예시 / 찾기 결과는 비운다
        /// (Chain Marker 는 저장할 때 저장 기준으로 바꾸므로 어느 기준에서 만들어도 같다).
        /// </summary>
        private void cboViewSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_changingViewSide) return;
            string view = SelectedViewSide;
            if (_shownItems == null || _shownMap == null || view == _displaySide)
            {
                SetStatus("보기 기준: " + GdsSideTransform.Text(view) + (_shownItems == null ? " (다음 [조회]에 적용)" : ""));
                return;
            }
            if (_extractRunning || _chainDbBusy)
            {
                _changingViewSide = true;
                cboViewSide.SelectedIndex = _displaySide == GdsSideTransform.Back ? 1 : 0;
                _changingViewSide = false;
                return;
            }
            using (ViewBusy.Busy())
            {
                ClearTemplate();
                ClearExtractResults();
                List<MapPlacedElementData> items = GdsSideTransform.MirrorAll(_shownItems);
                map.ShowPlacedElements(_shownMap.TopStructure, _shownColors, items);
                ApplyLoadedLayerVisibility();
                _shownItems = items;
                _displaySide = view;
                BuildElementIndex();
                RefreshLabelResultsForSide();
                ShowSelectedSavedChains();
            }
            SetStatus("보기 기준: " + GdsSideTransform.Text(view) + " - 예시 / 찾기 결과를 지웠습니다.");
        }

        /// <summary>다시 그린 지도에 Layer 목록의 표시 체크를 다시 적용한다.</summary>
        private void ApplyLoadedLayerVisibility()
        {
            var visibility = new Dictionary<int, bool>();
            foreach (LayerDisplayItem item in chkLayerItems.Items)
                if (_loadedLayers.Contains(item.LayerKey))
                    visibility[item.LayerKey] = chkLayerItems.CheckedItems.Contains(item);
            if (visibility.Count > 0) map.SetLayerVisibility(visibility);
        }

        /// <summary>조회 / 찾기 / 저장 중 표시: 상태 칸 "작업 중..." + 모래시계 커서.</summary>
        private BusyStatus ViewBusy
        {
            get { return _viewBusy ?? (_viewBusy = new BusyStatus(this, lblElapsed)); }
        }
        private BusyStatus _viewBusy;

        /// <summary>조회 중에는 조건 / Layer 체크 / U8 작업을 막는다.</summary>
        private void SetLoading(bool loading)
        {
            if (loading) ViewBusy.Start();
            else ViewBusy.Stop();
            _loading = loading;
            cboDevice.Enabled = !loading;
            btnSearch.Enabled = !loading;
            btnStop.Enabled = loading;
            chkLayerItems.Enabled = btnLayerAll.Enabled = btnLayerNone.Enabled = !loading;
            progressLoad.Value = 0;
            progressLoad.Visible = loading;
            cboViewSide.Enabled = !loading;
            SetLabelSearchEnabled(!loading);
            UpdateU8Buttons();
        }

        private bool _loading;

        #endregion

        #region [Layer 표시]

        /// <summary>Layer 이름 뒤에 DB 도형 수와 조회 여부를 붙인다.</summary>
        private void chkLayerItems_Format(object sender, ListControlConvertEventArgs e)
        {
            var item = e.ListItem as LayerDisplayItem;
            if (item == null) return;
            long count;
            string text = "Layer " + GdsLayerKey.Format(item.LayerKey) + " (" + (_layerCounts.TryGetValue(item.LayerKey, out count) ? count.ToString("N0") : "-") + ")";
            if (_loadedLayers.Count > 0 && !_loadedLayers.Contains(item.LayerKey)) text += " 미조회";
            e.Value = text;
        }

        /// <summary>조회한 Layer 면 바로 표시 / 숨김. 같은 형태 찾기 대상 Layer 는 예시를 지정할 때 체크된 Layer 다.</summary>
        private void chkLayerItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingLayerChecks) return;
            var item = chkLayerItems.Items[e.Index] as LayerDisplayItem;
            if (item == null) return;
            if (_loadedLayers.Contains(item.LayerKey))
                map.SetLayerVisibility(new Dictionary<int, bool> { { item.LayerKey, e.NewValue == CheckState.Checked } });
            else if (_loadedLayers.Count > 0 && e.NewValue == CheckState.Checked)
                SetStatus("Layer " + GdsLayerKey.Format(item.LayerKey) + " 미조회");
        }

        private void btnLayerAll_Click(object sender, EventArgs e)
        {
            SetAllLayerChecked(true);
        }

        private void btnLayerNone_Click(object sender, EventArgs e)
        {
            SetAllLayerChecked(false);
        }

        private void SetAllLayerChecked(bool isChecked)
        {
            _updatingLayerChecks = true;
            chkLayerItems.BeginUpdate();
            try
            {
                for (int i = 0; i < chkLayerItems.Items.Count; i++)
                    chkLayerItems.SetItemChecked(i, isChecked);
            }
            finally
            {
                chkLayerItems.EndUpdate();
                _updatingLayerChecks = false;
            }
            if (_loadedLayers.Count > 0)
                map.SetLayerVisibility(_loadedLayers.ToDictionary(id => id, id => isChecked));
        }

        /// <summary>도면에 올라 있고 지금 체크된(보이는) 도형 Layer 키. TEXT 만 있는 Layer 도 들어 있을 수 있다.</summary>
        private HashSet<int> CheckedLoadedLayers()
        {
            var result = new HashSet<int>();
            foreach (LayerDisplayItem item in chkLayerItems.CheckedItems)
                if (_loadedLayers.Contains(item.LayerKey)) result.Add(item.LayerKey);
            return result;
        }

        #endregion

        #region [상태 표시]

        private bool _showLoadProgress;

        private void map_RenderProgressChanged(object sender, GdsMapRenderProgressChangedEventArgs e)
        {
            if (_showLoadProgress)
                lblStatus.Text = e.IsCompleted ? "도면 표시 완료" : "도면을 그리는 중입니다.";
            if (e.IsCompleted)
            {
                if (!_showLoadProgress && !_extractRunning) progressLoad.Visible = false;
                statusStripView.Refresh();
                return;
            }
            progressLoad.Visible = true;
            progressLoad.Style = e.IsMarquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            if (!e.IsMarquee)
                progressLoad.Value = Math.Max(progressLoad.Minimum, Math.Min(progressLoad.Maximum, e.Percent));
            statusStripView.Refresh();
        }

        private void map_FirstFrameMeasured(object sender, GdsMapLoadMetrics metrics)
        {
            progressLoad.Visible = false;
            if (_showLoadProgress && !string.IsNullOrEmpty(_loadSummary))
                lblStatus.Text = _loadSummary;
            _showLoadProgress = false;
        }

        private int _lastCoordinateTick;

        /// <summary>마우스 월드 좌표를 약 30fps 로 제한해 표시한다.</summary>
        private void map_MouseWorldPositionChanged(object sender, PointF position)
        {
            int now = Environment.TickCount;
            if (unchecked(now - _lastCoordinateTick) < 33) return;
            _lastCoordinateTick = now;
            lblCoordinate.Text = "X: " + position.X.ToString("0.###") + " / Y: " + position.Y.ToString("0.###");
        }

        /// <summary>Esc: 예시 지정 중이면 끝낸다.</summary>
        private void GdsMapViewU8Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape || !_templateSelecting) return;
            SetTemplateSelecting(false);
            SetStatus("예시 지정을 마쳤습니다.");
            e.Handled = true;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        /// <summary>원격 호출 예외는 안쪽 예외에 실제 사유가 있으므로 가장 안쪽 메시지를 보여 준다.</summary>
        private void ShowError(string title, Exception ex)
        {
            Exception cause = ex;
            while (cause.InnerException != null) cause = cause.InnerException;
            SetStatus(title + " 실패: " + cause.Message);
            MessageBox.Show(this, cause.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// U8 작업 버튼 사용 가능 여부를 한 곳에서 정한다.
        /// 조회 / 찾기 / 저장 중에는 막고, 도면이 있으면 예시 지정, 예시가 있으면 찾기, 생성 대상이 있으면 생성.
        /// </summary>
        private void UpdateU8Buttons()
        {
            bool busy = _loading || _extractRunning || _chainDbBusy;
            bool loaded = _shownMap != null && map.Structure != null;
            pnlU8Bar.Visible = loaded;
            btnTemplate.Enabled = loaded && !busy;
            btnTemplate.Text = _templateSelecting ? "지정 끝" : "예시 지정";
            btnFind.Enabled = loaded && !busy && !_templateSelecting && _template != null;
            btnFindStop.Enabled = _extractRunning;
            btnCreate.Enabled = loaded && !busy && _chainDbEnabled && CountCreateTargets() > 0;
            btnResultRemove.Enabled = !busy && dgvResult.SelectedRows.Count > 0;
            cboResultClass.Enabled = txtResultFind.Enabled = !_extractRunning;
            btnSavedRefresh.Enabled = loaded && !busy && _chainDbEnabled;
            btnSavedDelete.Enabled = loaded && !busy && _chainDbEnabled && dgvSaved.SelectedRows.Count > 0;
            btnSavedDeleteShown.Enabled = loaded && !busy && _chainDbEnabled && _savedView.Count > 0;
            btnSavedCheckAll.Enabled = loaded && !busy && _chainDbEnabled && _savedView.Count > 0;
            btnSavedCheckNone.Enabled = loaded && !busy && _chainDbEnabled && _savedRows.Count > 0;
            btnSavedColor.Enabled = loaded && !busy && _chainDbEnabled && dgvSaved.SelectedRows.Count > 0;
        }

        #endregion

        /// <summary>선택한 Device 의 Map 헤더 1행(GetMapInfo)을 보관한다.</summary>
        private sealed class MapItem
        {
            public string Factory { get; private set; }
            public string DeviceId { get; private set; }
            public string TopStructure { get; private set; }
            public string Status { get; private set; }
            /// <summary>DB 좌표의 저장 기준(TQP_GDS_MAP.VIEW_SIDE, 없으면 FRONT).</summary>
            public string ViewSide { get; private set; }
            public bool IsReady { get { return Status == "READY"; } }
            private readonly string _description;

            public MapItem(DataRow row)
            {
                Factory = row["FACTORY"].ToString();
                DeviceId = row["DEVICE_ID"].ToString();
                TopStructure = row["TOP_STRUCTURE"].ToString();
                Status = row["MAP_STATUS"].ToString();
                ViewSide = GdsSideTransform.Normalize(row.Table.Columns.Contains("VIEW_SIDE") ? row["VIEW_SIDE"].ToString() : null);
                _description = "Factory / Device: " + Factory + " / " + DeviceId
                    + "\r\n상태: " + Status
                    + "\r\nGDS 파일: " + row["GDS_FILE_NAME"]
                    + "\r\n도형: " + Convert.ToInt64(row["PLACED_COUNT"], CultureInfo.InvariantCulture).ToString("N0");
            }

            public string Describe()
            {
                return _description;
            }
        }
    }
}
