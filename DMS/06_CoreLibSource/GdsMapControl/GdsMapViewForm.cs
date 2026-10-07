using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// DB에 저장된 GDS Map(READY)을 GDS 파일 없이 조회하고, Chain을 설정해 DB에 반영하는 화면이다.
    /// 설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 3절 / 15절, 문서/2026-10-06_GDS_Map_화면역할_재정의_설계.md.
    /// 2026-10-06 사용자 결정(CLAUDE-026): Chain 설정 / Chain DB 반영은 이 화면에서 한다(Chain 동작은 GdsMapViewForm_Chain*.cs).
    /// Map 도형(LAYER / SOURCE / PLACED)은 바꾸지 않고 Chain 테이블만 저장한다.
    /// DACrux 메뉴(frmGdsMapView)가 이 EXE를 같은 프로세스에 올려 표시하므로 DMS Service(RO GdsMapView)를 호출할 수 있다.
    /// 2026-10-02 리비전 관리 제거(CLAUDE-017): Map은 Factory + Device(제품)당 1개이므로 Revision 선택 단계가 없다.
    ///
    /// 처리 흐름
    ///  화면 표시 -> Device 목록 -> Factory / Device 선택 -> Map 정보 + Layer 목록(도형 수)
    ///  [조회] 체크한 Layer만 Layer별 페이지로 받음(백그라운드) -> 행 해석 -> ShowPlacedElements(UI 스레드 1회)
    ///  조회 후 Layer 체크 변경은 다시 조회하지 않고 표시 / 숨김으로 동작한다.
    /// </summary>
    public partial class GdsMapViewForm : Form
    {
        /// <summary>서비스 한 번에 받는 도형 수. 2026-10-02 측정: 10,000행 = Binary 약 2.9MB / 페이지 약 0.4초.</summary>
        private const int PageRows = 10000;

        private DataTable _deviceTable;
        private CancellationTokenSource _loadCancel;
        private bool _updatingLayerChecks;
        /// <summary>현재 도면에 실제로 올린 Layer. 체크 변경 시 이 Layer만 표시 / 숨김을 적용한다.</summary>
        private readonly HashSet<int> _loadedLayers = new HashSet<int>();
        /// <summary>Layer별 DB 도형 수 (목록 표시용).</summary>
        private readonly Dictionary<int, long> _layerCounts = new Dictionary<int, long>();
        private string _loadSummary;
        /// <summary>현재 선택한 Device의 READY Map 정보. 없거나 READY가 아니면 null.</summary>
        private MapItem _currentMap;
        /// <summary>지금 도면에 그려진 Map. Chain 저장 대상이다(Device 선택만 바꾸고 조회하지 않은 Map과 구분).</summary>
        private MapItem _shownMap;
        /// <summary>
        /// Front / Back 보기(2026-10-07 CLAUDE-030): 지금 화면 기준과 그 기준으로 변환된 화면 도형 목록 / 색상.
        /// 보기 기준을 바꾸면 DB를 다시 읽지 않고 이 목록을 좌우 반전(GdsSideTransform)해 다시 그린다.
        /// </summary>
        private string _displaySide = GdsSideTransform.Front;
        private List<MapPlacedElementData> _shownItems;
        private Dictionary<int, Color> _shownColors;
        private bool _changingViewSide;

        public GdsMapViewForm()
        {
            InitializeComponent();
            InitializeChainSetupState();
            InitializeLabelSearchState();
            // 보기 기준 기본값: 원본 Front side(2026-10-07 사용자 결정).
            _changingViewSide = true;
            cboViewSide.SelectedIndex = 0;
            _changingViewSide = false;
        }

        /// <summary>콤보에서 고른 보기 기준(0 Front / 1 Back).</summary>
        private string SelectedViewSide
        {
            get { return cboViewSide.SelectedIndex == 1 ? GdsSideTransform.Back : GdsSideTransform.Front; }
        }

        /// <summary>
        /// 보기 기준 변경. 도면이 없으면 다음 [조회]에 적용한다.
        /// 처리 흐름: 편집 중 Chain 보관 -> 화면 도형 목록 좌우 반전 -> 다시 그리기 -> 탐색 캐시 재연결 -> Chain 좌표 사본 반전 -> Chain 다시 연결.
        /// Chain 소속(도형 ID) / DB 데이터 / 저장 지문은 바뀌지 않는다.
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
            var watch = Stopwatch.StartNew();
            CaptureActiveChain();
            List<MapPlacedElementData> items = GdsSideTransform.MirrorAll(_shownItems);
            map.ShowPlacedElements(_shownMap.TopStructure, _shownColors, items);
            ApplyLoadedLayerVisibility();
            _shownItems = items;
            _displaySide = view;
            map.RebuildChainTraceItems();
            MirrorChainsForSideChange();
            ReattachChainsAfterMapReload();
            // Label 검색 결과도 새 기준 X로 다시 표시한다(CLAUDE-032).
            RefreshLabelResultsForSide();
            UpdateMapInfoText();
            SetStatus("보기 기준: " + GdsSideTransform.Text(view) + " / 전환 " + watch.Elapsed.TotalSeconds.ToString("0.0") + "초 (도형 "
                + items.Count.ToString("N0") + ", DB 다시 읽지 않음)");
        }

        /// <summary>다시 그린 지도에 Layer 목록의 표시 체크를 다시 적용한다(ShowPlacedElements는 모든 Layer를 보이게 만든다).</summary>
        private void ApplyLoadedLayerVisibility()
        {
            var visibility = new Dictionary<int, bool>();
            foreach (LayerDisplayItem item in chkLayerItems.Items)
                if (_loadedLayers.Contains(item.LayerId))
                    visibility[item.LayerId] = chkLayerItems.CheckedItems.Contains(item);
            if (visibility.Count > 0) map.SetLayerVisibility(visibility);
        }

        /// <summary>Map 정보에 저장 기준 / 보기 기준을 함께 보여 준다.</summary>
        private void UpdateMapInfoText()
        {
            MapItem item = _shownMap ?? _currentMap;
            if (item == null) return;
            lblMapInfo.Text = item.Describe() + "\r\n보기 기준: " + GdsSideTransform.Text(_shownMap != null ? _displaySide : SelectedViewSide);
        }

        /// <summary>DACrux 호스트(frmGdsMapView)가 사용하는 생성자. 등록 화면과 같은 형식으로 맞춘다.</summary>
        public GdsMapViewForm(bool hostedByDms) : this()
        {
            // Chain DB 저장은 DMS Service(RO) 연결 정보가 있는 DACrux 안에서만 가능하므로 호스트 모드에서만 보인다.
            InitializeChainDbState(hostedByDms);
        }

        #region [조회 조건]

        /// <summary>
        /// 이 화면의 Factory. 2026-10-07 사용자 지시: 화면에서 고르지 않고 DACrux 로그인 Factory(DACrux.Base.GlobalVariable.Factory)를 쓴다.
        /// 화면을 열 때 한 번 읽는다(열려 있는 동안 Device 목록과 Factory가 어긋나지 않게).
        /// </summary>
        private string _factory = string.Empty;

        /// <summary>화면이 보인 뒤 로그인 Factory의 READY Map Device 목록을 읽는다.</summary>
        private async void GdsMapViewForm_Shown(object sender, EventArgs e)
        {
            HookCloseConfirmation();
            _factory = DmsHostContext.Factory;
            if (_factory.Length == 0)
            {
                SetStatus("Factory 정보가 없습니다. DACrux에 로그인한 상태에서 메뉴로 여세요.");
                return;
            }
            SetStatus("Device 목록 조회 중 (Factory " + _factory + ")");
            try
            {
                _deviceTable = await Task.Run(() => new DACrux.SEMDMS.RO.GdsMapView().GetMapDeviceList());
                FillDeviceList();
                SetStatus(cboDevice.Items.Count == 0 ? "Factory " + _factory + "에 조회 가능한 GDS Map이 없습니다." : "Device를 선택하세요. (Factory " + _factory + ")");
            }
            catch (Exception ex)
            {
                ShowError("Device 목록 조회", ex);
            }
        }

        private Form _closeHookForm;

        /// <summary>
        /// 화면을 닫을 때 저장하지 않은 Chain 변경을 확인한다(2026-10-06 CLAUDE-026 2단계).
        /// DMS에서는 이 Form이 호스트(frmGdsMapView) 안에 TopLevel = false로 붙으므로 자기 FormClosing이 오지 않는다.
        /// 그래서 실제로 닫히는 바깥 Form(호스트, 단독 실행이면 자기 자신)의 FormClosing을 구독한다. DACrux 종료(MDI 닫기)도 여기로 온다.
        /// </summary>
        private void HookCloseConfirmation()
        {
            Form owner = TopLevel ? this : (Parent == null ? null : Parent.FindForm());
            if (owner == null || ReferenceEquals(owner, _closeHookForm)) return;
            if (_closeHookForm != null) _closeHookForm.FormClosing -= CloseHookForm_FormClosing;
            _closeHookForm = owner;
            _closeHookForm.FormClosing += CloseHookForm_FormClosing;
        }

        private void CloseHookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel || IsDisposed) return;
            if (!ConfirmDiscardUnsavedChains("화면을 닫으"))
                e.Cancel = true;
        }

        /// <summary>조회 조건을 되돌리는 동안 콤보 이벤트를 무시한다.</summary>
        private bool _restoringCondition;

        /// <summary>
        /// Device를 바꾸기 전에 저장하지 않은 Chain 변경을 확인한다(2026-10-06 CLAUDE-026 2단계, 설계 4.2절).
        /// [아니오]면 콤보를 지금 도면의 Device로 되돌리고 false를 돌려준다.
        /// [예]면 이후 ClearMapInfo에서 Chain이 비워지므로 같은 변경 중 Device 이벤트가 다시 묻지 않는다.
        /// </summary>
        private bool ConfirmConditionChange()
        {
            if (_restoringCondition) return false;
            if (_shownMap == null || ConfirmDiscardUnsavedChains("다른 Device를 선택")) return true;
            _restoringCondition = true;
            try
            {
                cboDevice.Text = _shownMap.DeviceId;
            }
            finally
            {
                _restoringCondition = false;
            }
            return false;
        }

        /// <summary>로그인 Factory의 Device만 목록에 넣는다.</summary>
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

        /// <summary>Device를 고르면 Map 정보와 Layer 목록을 읽는다.</summary>
        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ConfirmConditionChange()) return;
            LoadMapInfo();
        }

        /// <summary>목록에 없는 Device를 직접 입력하고 Enter를 누르면 Map 정보를 읽는다.</summary>
        private void cboDevice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ConfirmConditionChange()) LoadMapInfo();
        }

        /// <summary>
        /// Factory / Device의 Map 헤더를 읽어 정보를 표시하고, READY면 Layer 목록(저장 색상 / 도형 수)을 채운다.
        /// 응답을 기다리는 동안 선택이 바뀌면 이전 결과는 버린다.
        /// </summary>
        private async void LoadMapInfo()
        {
            ClearMapInfo();
            string factory = _factory;
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0) return;
            SetStatus("Map 정보 조회 중");
            try
            {
                var service = new DACrux.SEMDMS.RO.GdsMapView();
                DataTable info = await Task.Run(() => service.GetMapInfo(factory, device));
                if (device != cboDevice.Text.Trim()) return;
                if (info.Rows.Count == 0)
                {
                    lblMapInfo.Text = "등록된 GDS Map이 없습니다.";
                    SetStatus("등록된 GDS Map이 없습니다: " + factory + " / " + device);
                    return;
                }
                var item = new MapItem(info.Rows[0]);
                lblMapInfo.Text = item.Describe();
                if (!item.IsReady)
                {
                    SetStatus("저장이 끝나지 않은 Map입니다(상태 " + item.Status + "). 등록 화면에서 저장을 마친 뒤 조회하세요.");
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
                        int layerId = Convert.ToInt32(row["LAYER_ID"], CultureInfo.InvariantCulture);
                        _layerCounts[layerId] = Convert.ToInt64(row["PLACED_COUNT"], CultureInfo.InvariantCulture);
                        var color = Color.FromArgb(Convert.ToInt32(row["COLOR_ARGB"], CultureInfo.InvariantCulture));
                        chkLayerItems.Items.Add(new LayerDisplayItem(layerId, color, true), true);
                    }
                }
                finally
                {
                    chkLayerItems.EndUpdate();
                    _updatingLayerChecks = false;
                }
                _currentMap = item;
                SetStatus("체크한 Layer만 조회합니다. [조회]를 누르세요.");
            }
            catch (Exception ex)
            {
                ShowError("Map 정보 조회", ex);
            }
        }

        private void ClearMapInfo()
        {
            _currentMap = null;
            _shownMap = null;
            _shownItems = null;
            _shownColors = null;
            // 다른 Device로 바뀌면 이전 도면의 Chain 편집 상태를 비운다(미저장 변경은 ConfirmConditionChange에서 먼저 확인).
            ResetChainSetupAfterMapLoad(false);
            ClearLabelIndex();
            lblMapInfo.Text = "Device를 선택하세요.";
            ClearLayerList();
        }

        private void ClearLayerList()
        {
            _updatingLayerChecks = true;
            chkLayerItems.Items.Clear();
            _updatingLayerChecks = false;
            _layerCounts.Clear();
            // 목록이 다른 Device로 바뀌면 이전 도면의 Layer 체크로 표시 / 숨김을 바꾸지 않는다.
            _loadedLayers.Clear();
        }

        #endregion

        #region [도면 조회]

        /// <summary>
        /// 체크한 Layer의 도형을 DB에서 받아 도면을 다시 그린다.
        /// 처리 흐름: 입력 확인 -> Layer별 페이지 조회 + 행 해석(백그라운드, 페이지마다 중지 확인) -> ShowPlacedElements(UI 스레드).
        /// 중지하거나 실패하면 현재 화면의 도면은 그대로 둔다.
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

            // Chain(3단계): 다른 Map이면 도면을 그리기 전에 DB Chain을 읽고, 같은 Map이면 화면 Chain을 쓴다.
            // Chain이 쓰는 Layer가 조회 체크에 없으면 함께 조회할지 묻는다(사용자 결정 3).
            bool sameMap = _shownMap != null && _shownMap.Factory == target.Factory && _shownMap.DeviceId == target.DeviceId;
            List<ChainDbData> dbChains = null;
            if (!sameMap && _chainDbEnabled)
            {
                SetStatus("저장된 Chain 조회 중");
                btnSearch.Enabled = false;
                try
                {
                    var chainProgress = new Progress<string>(SetStatus);
                    dbChains = await Task.Run(() => ReadDbChains(target.Factory, target.DeviceId, chainProgress));
                }
                catch (Exception ex)
                {
                    ShowError("저장된 Chain 조회", ex);
                }
                finally
                {
                    btnSearch.Enabled = true;
                }
            }
            // Label 검색 색인(CLAUDE-032 사용자 결정 5): 다른 Map이면 [조회] 때 함께 받는다. 실패해도 도면은 조회하고 검색만 막는다.
            GdsLabelIndex labelIndex = null;
            string labelIndexError = null;
            if (!sameMap)
            {
                SetStatus("Label 색인 조회 중");
                btnSearch.Enabled = false;
                try
                {
                    labelIndex = await Task.Run(() => ReadLabelIndex(target.Factory, target.DeviceId));
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
            bool layersAdded = sameMap
                ? ConfirmChainLayers(MemoryChainLayers(), "화면의 Chain")
                : dbChains != null && dbChains.Count > 0 && ConfirmChainLayers(dbChains.SelectMany(c => c.NeededLayers), "저장된 Chain " + dbChains.Count + "개");
            if (layersAdded)
                layers = chkLayerItems.CheckedItems.Cast<LayerDisplayItem>().ToList();

            long total = layers.Sum(l => _layerCounts.ContainsKey(l.LayerId) ? _layerCounts[l.LayerId] : 0);
            var colors = layers.ToDictionary(l => l.LayerId, l => l.Color);
            var progress = new Progress<long>(received => ReportLoadProgress(received, total));
            // 지도를 다시 그리기 전에 편집 중인 Chain 상태를 Chain 정의에 보관한다.
            CaptureActiveChain();
            _loadCancel = new CancellationTokenSource();
            CancellationToken token = _loadCancel.Token;
            SetLoading(true);
            var watch = Stopwatch.StartNew();
            try
            {
                List<MapPlacedElementData> items = await Task.Run(
                    () => LoadPlacedElements(target.Factory, target.DeviceId, layers.Select(l => l.LayerId).ToList(), progress, token));
                double dbSeconds = watch.Elapsed.TotalSeconds;

                // 보기 기준(CLAUDE-030): DB 좌표는 저장 기준. 고른 보기 기준과 다르면 좌우 반전해 그린다.
                string view = SelectedViewSide;
                if (GdsSideTransform.NeedsMirror(target.ViewSide, view))
                    items = GdsSideTransform.MirrorAll(items);

                watch.Restart();
                map.ShowPlacedElements(target.TopStructure, colors, items);
                double buildSeconds = watch.Elapsed.TotalSeconds;

                _loadedLayers.Clear();
                foreach (var layer in layers) _loadedLayers.Add(layer.LayerId);
                // 같은 Map을 다시 조회(Layer 추가 / 변경)했으면 Chain 목록과 편집 내용을 유지하고 새 지도에 다시 연결한다.
                // 다른 Map이면 Chain 설정을 새로 시작하고 DB Chain을 불러온다. (2026-10-06 CLAUDE-026 2·3단계)
                _shownMap = target;
                _shownItems = items;
                _shownColors = colors;
                _displaySide = view;
                UpdateMapInfoText();
                if (sameMap)
                    ReattachChainsAfterMapReload();
                else
                {
                    ResetChainSetupAfterMapLoad(true);
                    if (dbChains != null && dbChains.Count > 0)
                        LoadChainsFromDb(dbChains);
                    ReplaceLabelIndex(labelIndex, labelIndexError);
                }
                // 목록 문구(미조회 표시)를 새 기준으로 다시 그린다.
                chkLayerItems.Refresh();
                _loadSummary = "조회 완료: " + target.Factory + " / " + target.DeviceId + " / 도형 " + items.Count.ToString("N0")
                    + " / DB 조회 " + dbSeconds.ToString("0.0") + "초 / 도면 구성 " + buildSeconds.ToString("0.0") + "초";
                SetStatus(_loadSummary);
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

        /// <summary>
        /// Layer마다 이전 페이지 마지막 ID 다음부터 PageRows씩 받아 행을 해석한다. 백그라운드 스레드에서 실행한다.
        /// 받은 페이지는 해석 후 바로 해제해 DataTable과 해석 결과를 동시에 오래 들고 있지 않는다.
        /// </summary>
        private static List<MapPlacedElementData> LoadPlacedElements(string factory, string deviceId, IList<int> layerIds,
            IProgress<long> progress, CancellationToken token)
        {
            var service = new DACrux.SEMDMS.RO.GdsMapView();
            var items = new List<MapPlacedElementData>();
            foreach (int layerId in layerIds)
            {
                string after = string.Empty;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    int rowCount;
                    using (DataTable page = service.GetPlacedElementPage(factory, deviceId,
                        layerId.ToString(CultureInfo.InvariantCulture), after, PageRows))
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
            SetStatus("DB 조회 중: " + received.ToString("N0") + " / " + total.ToString("N0") + " 도형");
        }

        /// <summary>조회 중에는 조건 / 옵션 / Layer 체크를 막는다.</summary>
        private void SetLoading(bool loading)
        {
            cboDevice.Enabled = !loading;
            btnSearch.Enabled = !loading;
            btnStop.Enabled = loading;
            chkLayerItems.Enabled = btnLayerAll.Enabled = btnLayerNone.Enabled = !loading;
            progressLoad.Value = 0;
            progressLoad.Visible = loading;
            cboViewSide.Enabled = !loading;
            // Label 검색은 조회 중 막고, 끝나면 색인이 있을 때만 연다(CLAUDE-032).
            SetLabelSearchEnabled(!loading);
            // 조회 중에는 Chain 조작을 막는다. 조회가 끝나면 ResetChainSetupAfterMapLoad 또는 아래에서 다시 연다.
            if (loading)
            {
                SetChainButtonsEnabled(false);
                SetChainListButtonsEnabled(false);
            }
            else if (map.Structure != null)
            {
                SetChainButtonsEnabled(true);
                SetChainListButtonsEnabled(true);
                UpdateChainSelectionButtons();
            }
        }

        #endregion

        #region [Layer 표시]

        /// <summary>Layer 이름 뒤에 DB 도형 수와 조회 여부를 붙인다.</summary>
        private void chkLayerItems_Format(object sender, ListControlConvertEventArgs e)
        {
            var item = e.ListItem as LayerDisplayItem;
            if (item == null) return;
            long count;
            string text = "Layer " + item.LayerId + " (" + (_layerCounts.TryGetValue(item.LayerId, out count) ? count.ToString("N0") : "-") + ")";
            if (_loadedLayers.Count > 0 && !_loadedLayers.Contains(item.LayerId)) text += " 미조회";
            e.Value = text;
        }

        /// <summary>
        /// 조회한 Layer면 즉시 표시 / 숨김을 적용한다. 조회하지 않은 Layer는 다음 [조회] 대상만 바뀐다.
        /// ItemCheck는 상태가 바뀌기 전에 발생하므로 이벤트의 새 값을 사용한다.
        /// </summary>
        private void chkLayerItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingLayerChecks) return;
            var item = chkLayerItems.Items[e.Index] as LayerDisplayItem;
            if (item == null) return;
            if (_loadedLayers.Contains(item.LayerId))
                map.SetLayerVisibility(new Dictionary<int, bool> { { item.LayerId, e.NewValue == CheckState.Checked } });
            else if (_loadedLayers.Count > 0 && e.NewValue == CheckState.Checked)
                SetStatus("Layer " + item.LayerId + "는 조회하지 않은 Layer입니다. 표시하려면 [조회]를 다시 누르세요.");
        }

        private void btnLayerAll_Click(object sender, EventArgs e)
        {
            SetAllLayerChecked(true);
        }

        private void btnLayerNone_Click(object sender, EventArgs e)
        {
            SetAllLayerChecked(false);
        }

        /// <summary>전체 체크 / 해제 후 조회한 Layer의 표시 상태를 한 번에 적용한다.</summary>
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

        #endregion

        #region [상태 표시]

        /// <summary>도면 구성 / GPU 처리 단계의 진행을 StatusStrip에 표시한다(GdsMapForm과 같은 방식).</summary>
        private void map_RenderProgressChanged(object sender, GdsMapRenderProgressChangedEventArgs e)
        {
            lblStatus.Text = e.Message;
            if (e.IsCompleted)
            {
                statusStripView.Refresh();
                return;
            }
            progressLoad.Visible = true;
            progressLoad.Style = e.IsMarquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            if (!e.IsMarquee)
                progressLoad.Value = Math.Max(progressLoad.Minimum, Math.Min(progressLoad.Maximum, e.Percent));
            statusStripView.Refresh();
        }

        /// <summary>첫 화면이 그려지면 조회 요약을 다시 표시하고 단계별 시간은 툴팁으로 보여 준다.</summary>
        private void map_FirstFrameMeasured(object sender, GdsMapLoadMetrics metrics)
        {
            progressLoad.Visible = false;
            if (!string.IsNullOrEmpty(_loadSummary))
                lblStatus.Text = _loadSummary;
            lblStatus.ToolTipText = "도형 생성 " + metrics.FlattenMs.ToString("0") + "ms"
                + " / 정점 생성 " + metrics.VertexBuildMs.ToString("0") + "ms"
                + " / GPU 업로드 " + metrics.GpuUploadMs.ToString("0") + "ms"
                + " / Draw " + metrics.FirstDrawMs.ToString("0") + "ms"
                + " / BOUNDARY " + metrics.BoundaryCount.ToString("N0")
                + " / PATH " + metrics.PathCount.ToString("N0")
                + " / TEXT " + metrics.TextCount.ToString("N0")
                + " / 좌표 " + metrics.SourcePointCount.ToString("N0")
                + " / 정점 " + metrics.VertexCount.ToString("N0");
        }

        private int _lastCoordinateTick;

        /// <summary>마우스 월드 좌표를 약 30fps로 제한해 표시한다.</summary>
        private void map_MouseWorldPositionChanged(object sender, PointF position)
        {
            int now = Environment.TickCount;
            if (unchecked(now - _lastCoordinateTick) < 33) return;
            _lastCoordinateTick = now;
            lblCoordinate.Text = "X: " + position.X.ToString("0.###") + " / Y: " + position.Y.ToString("0.###");
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

        #endregion

        /// <summary>선택한 Device의 Map 헤더 1행(GetMapInfo)을 보관하고 Map 정보 문구를 만든다.</summary>
        private sealed class MapItem
        {
            public string Factory { get; private set; }
            public string DeviceId { get; private set; }
            public string TopStructure { get; private set; }
            public string Status { get; private set; }
            /// <summary>DB PLACED / Chain Marker 좌표의 저장 기준(TQP_GDS_MAP.VIEW_SIDE, 없으면 FRONT).</summary>
            public string ViewSide { get; private set; }
            public bool IsReady { get { return Status == "READY"; } }
            private readonly string _description;

            public MapItem(DataRow row)
            {
                Factory = row["FACTORY"].ToString();
                DeviceId = row["DEVICE_ID"].ToString();
                TopStructure = row["TOP_STRUCTURE"].ToString();
                Status = row["MAP_STATUS"].ToString();
                // 저장 기준(2026-10-06 CLAUDE-027): BACK이면 PLACED가 GDS 원점 기준 좌우 반전 좌표로 저장되어 있다(그대로 그림).
                string side = GdsSideTransform.Normalize(row.Table.Columns.Contains("VIEW_SIDE") ? row["VIEW_SIDE"].ToString() : null);
                ViewSide = side;
                _description = "Factory / Device: " + Factory + " / " + DeviceId
                    + "\r\n상태: " + Status
                    + "\r\n저장 기준: " + GdsSideTransform.Text(side)
                    + "\r\nGDS 파일: " + row["GDS_FILE_NAME"]
                    + "\r\nTop Structure: " + TopStructure
                    + "\r\nSOURCE: " + FormatCount(row["SOURCE_COUNT"])
                    + "\r\nPLACED: " + FormatCount(row["PLACED_COUNT"])
                    + "\r\n좌표: " + FormatCount(row["POINT_COUNT"])
                    + "\r\n등록: " + row["CREATE_USER"] + " / " + row["CREATE_TIME"];
            }

            public string Describe() { return _description; }

            private static string FormatCount(object value)
            {
                return Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString("N0");
            }
        }
    }
}
