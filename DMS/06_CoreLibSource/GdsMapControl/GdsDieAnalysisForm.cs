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
    /// Die 상세 분석 화면(화면 B). 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5절 / 7.3절 (CLAUDE-053)
    ///
    /// 연관 분석 화면(DMS ENGUI frmGdsKlarfAnalysis)이 서버 분석 결과 DataSet 과 Die 를 넘기면
    /// GDS Chip 도면 위에 저장된 Chain 색과 KLARF Defect 위치를 함께 보여 준다. 판정은 다시 하지 않는다(같은 결과를 그대로 표시).
    /// ENGUI(.NET 4.0)는 이 프로젝트(.NET 4.8)를 참조하지 않으므로 호스트(frmGdsDieAnalysis)가 EXE 를 실행 시 읽어
    /// ShowDie(DataSet, int, int) 를 리플렉션으로 부르고, 이미지 보기는 DefectImageRequested(Action) 로 호스트에 돌려준다.
    ///
    /// 처리 흐름
    ///  1) ShowDie -> 분석 기준(ANL_INFO) / 선택 Chip 좌표 정보 표시. 상세 화면은 이 Chip 1개만 분석한다(2026-10-12 사용자 지시, 설계 14.4절)
    ///  2) (Factory, Device, 보기 기준, 배경 도면) 이 바뀌었을 때만 도면 + Chain Member 를 DB 에서 읽는다(전체 Layer 18.7초 -> 기본은 Chain Layer 만)
    ///  3) 연관 분석 화면에서 다른 Chip 을 고르면 같은 화면에 다시 넘어오고, Defect 표시 / Chain 목록 / Defect 목록만 다시 만든다(서버 호출 없음)
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
        private string _loadedKey;
        private int _loadVersion;
        private bool _busy;
        private bool _updating;
        private Point? _currentDie;
        private Dictionary<long, IList<string>> _chainMembers = new Dictionary<long, IList<string>>();
        private readonly Dictionary<long, IList<ChainTraceElement>> _chainElements = new Dictionary<long, IList<ChainTraceElement>>();

        /// <summary>GDS 도면 조회 서비스. null 이면 원격 호출 RO 를 만든다(Probe 는 같은 프로세스 BSL 로 바꾼다).</summary>
        public DACrux.SEMDMS.RO.GdsMapView MapService { get; set; }

        /// <summary>Chain Member 조회 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public DACrux.SEMDMS.RO.GdsChain ChainService { get; set; }

        /// <summary>Defect 이미지 보기 요청(STEP_SEQ, DEFECTID). DMS 호스트가 기존 FTP 이미지 표시 함수로 채운다.</summary>
        public Action<long, long> DefectImageRequested { get; set; }

        /// <summary>오류를 메시지 창으로도 알릴지(기본 true). 자동 검사(Smoke)는 false 로 두고 상태줄만 본다.</summary>
        public bool ShowErrorDialogs { get; set; } = true;

        /// <summary>마지막 상태줄 문구(Smoke 확인용).</summary>
        public string StatusText { get { return lblStatus.Text; } }

        /// <summary>도면 조회 중인지(Probe 대기용).</summary>
        public bool IsBusy { get { return _busy; } }

        /// <summary>지금 그린 도면의 키(Factory|Device|보기 기준|배경). Probe 확인용.</summary>
        public string LoadedMapKey { get { return _loadedKey; } }

        /// <summary>지도에 표시 중인 Defect 수 / Chain 수(Probe 확인용).</summary>
        public int DefectMarkerCount { get { return map.DefectMarkerCount; } }
        public int VisibleChainCount { get; private set; }

        public GdsDieAnalysisForm()
        {
            InitializeComponent();
            _updating = true;
            cboScope.SelectedIndex = 2;
            chkHitOnly.Checked = true;
            _updating = false;
        }

        /// <summary>DACrux 호스트(frmGdsDieAnalysis)가 쓰는 생성자. GDS Map 조회 화면과 같은 형식으로 맞춘다.</summary>
        public GdsDieAnalysisForm(bool hostedByDms) : this()
        {
        }

        #region [분석 결과 받기]

        /// <summary>호스트가 리플렉션으로 부르는 진입점. 결과를 기다리지 않는다.</summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex)
        {
            _ = ShowDieAsync(analysis, xIndex, yIndex);
        }

        /// <summary>
        /// 분석 결과와 Die 를 받아 화면을 채운다. 도면 키가 바뀌었으면 도면을 다시 읽은 뒤 표시한다.
        /// 실패는 상태줄 / 메시지로 알리고 예외를 밖으로 던지지 않는다(호스트가 리플렉션으로 부르므로).
        /// </summary>
        public async Task ShowDieAsync(DataSet analysis, int xIndex, int yIndex)
        {
            if (analysis == null || !analysis.Tables.Contains("ANL_INFO") || analysis.Tables["ANL_INFO"].Rows.Count == 0)
            {
                SetStatus("분석 결과가 없습니다. 연관 분석 화면에서 [분석] 한 뒤 Die 를 더블클릭하세요.");
                return;
            }
            DataRow info = analysis.Tables["ANL_INFO"].Rows[0];
            _analysis = analysis;
            _factory = info["FACTORY"].ToString();
            _device = info["DEVICE_ID"].ToString();
            _viewSide = GdsSideTransform.Normalize(info["VIEW_SIDE"].ToString());
            _stepSeq = Convert.ToInt64(info["STEP_SEQ"], CultureInfo.InvariantCulture);
            _tolerance = Convert.ToDouble(info["TOLERANCE_UM"], CultureInfo.InvariantCulture);
            lblWaferInfo.Text = String.Format("{0} / {1}  (GDS {2})", info["WAFER_ID"], info["STEP_ID"], _device);
            lblCriteria.Text = String.Format("분석 기준: {0} 보기 / 허용오차 {1} µm{2} / 보정 ({3}, {4}) / Chain {5} (최종 저장 {6}) / 분석 {7}",
                GdsSideTransform.Text(_viewSide), info["TOLERANCE_UM"], info["USE_DEFECT_SIZE"].ToString() == "Y" ? " + Defect 크기" : "",
                info["OFFSET_X"], info["OFFSET_Y"], info["CHAIN_COUNT"], info["CHAIN_LAST_SAVED"], info["ANALYZED_AT"]);
            // 상세 화면은 이 Chip 1개만 분석한다(2026-10-12 사용자 지시, 설계 14.4절). 다른 Chip 은 연관 분석 화면에서 다시 고른다.
            _currentDie = new Point(xIndex, yIndex);
            lblDieInfo.Text = "Die (" + xIndex + ", " + yIndex + ")";

            if (MapKey() != _loadedKey)
            {
                try
                {
                    await LoadMapAsync();
                }
                catch (Exception ex)
                {
                    ShowError("GDS 도면 조회", ex);
                    return;
                }
            }
            ShowCurrent();
        }

        private string MapKey()
        {
            return _factory + "|" + _device + "|" + _viewSide + "|" + (chkBackground.Checked ? "ALL" : "CHAIN");
        }

        #endregion

        #region [도면 / Chain 조회]

        /// <summary>
        /// GDS 도면과 Chain Member 를 DB 에서 읽는다(백그라운드). 기본은 Chain Member 가 쓰는 Layer 만 읽고,
        /// [배경 도면] 이면 Map 의 모든 Layer 를 읽는다. 저장 기준과 보기 기준이 다르면 GDS 좌우 반전(GdsSideTransform).
        /// 같은 화면에서 다른 요청이 들어오면 이전 조회 결과는 버린다(_loadVersion).
        /// </summary>
        private async Task LoadMapAsync()
        {
            int version = ++_loadVersion;
            string factory = _factory, device = _device, viewSide = _viewSide;
            bool background = chkBackground.Checked;
            List<long> chainSeqs = _analysis.Tables["CHAIN"].Rows.Cast<DataRow>().Select(r => Convert.ToInt64(r["CHAIN_SEQ"], CultureInfo.InvariantCulture)).ToList();
            var chainLayers = new HashSet<int>(_analysis.Tables["CHAIN"].Rows.Cast<DataRow>()
                .SelectMany(r => r["LAYER_IDS"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => int.Parse(s, CultureInfo.InvariantCulture)));
            DACrux.SEMDMS.RO.GdsMapView mapService = MapService ?? new DACrux.SEMDMS.RO.GdsMapView();
            DACrux.SEMDMS.RO.GdsChain chainService = ChainService ?? new DACrux.SEMDMS.RO.GdsChain();
            var progress = new Progress<string>(SetStatus);

            SetBusy(true);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                LoadedMap loaded = await Task.Run(() => ReadMap(mapService, chainService, factory, device, background, chainLayers, chainSeqs, progress));
                if (version != _loadVersion) return;
                double dbSeconds = watch.Elapsed.TotalSeconds;
                List<MapPlacedElementData> items = loaded.Items;
                if (GdsSideTransform.NeedsMirror(loaded.StoredSide, viewSide))
                    items = GdsSideTransform.MirrorAll(items);
                map.SetDefectMarkers(null, 0);
                map.ShowPlacedElements(loaded.TopStructure, loaded.Colors, items);
                map.RebuildChainTraceItems();
                _chainMembers = loaded.Members;
                _chainElements.Clear();
                _loadedKey = factory + "|" + device + "|" + viewSide + "|" + (background ? "ALL" : "CHAIN");
                SetStatus(String.Format("도면 조회 완료: {0} / Layer {1} / 도형 {2:N0} / Chain Member {3:N0} / {4:0.0}초 ({5})",
                    device, loaded.Colors.Count, items.Count, loaded.Members.Values.Sum(m => m.Count), watch.Elapsed.TotalSeconds,
                    GdsSideTransform.Text(viewSide)));
                System.Diagnostics.Trace.WriteLine("GdsDieAnalysis DB " + dbSeconds.ToString("0.0") + "s");
            }
            finally
            {
                if (version == _loadVersion) SetBusy(false);
            }
        }

        /// <summary>백그라운드: Map 헤더 / Layer / 도형 페이지 / Chain Member 를 읽는다. 화면 컨트롤을 만지지 않는다.</summary>
        private static LoadedMap ReadMap(DACrux.SEMDMS.RO.GdsMapView mapService, DACrux.SEMDMS.RO.GdsChain chainService,
            string factory, string device, bool background, ISet<int> chainLayers, IList<long> chainSeqs, IProgress<string> progress)
        {
            progress.Report("GDS Map 정보 조회 중");
            DataTable info = mapService.GetMapInfo(factory, device);
            if (info.Rows.Count == 0 || info.Rows[0]["MAP_STATUS"].ToString() != "READY")
                throw new InvalidOperationException("저장 완료(READY)된 GDS Map 이 없습니다: " + factory + " / " + device);
            var result = new LoadedMap
            {
                TopStructure = info.Rows[0]["TOP_STRUCTURE"].ToString(),
                StoredSide = GdsSideTransform.Normalize(info.Rows[0]["VIEW_SIDE"].ToString())
            };
            DataTable layers = mapService.GetMapLayerList(factory, device);
            foreach (DataRow row in layers.Rows)
            {
                int layerId = Convert.ToInt32(row["LAYER_ID"], CultureInfo.InvariantCulture);
                if (background || chainLayers.Count == 0 || chainLayers.Contains(layerId))
                    result.Colors[layerId] = Color.FromArgb(Convert.ToInt32(row["COLOR_ARGB"], CultureInfo.InvariantCulture));
            }
            result.Items = LoadPlacedElements(mapService, factory, device, result.Colors.Keys.ToList(), progress);
            int no = 0;
            foreach (long seq in chainSeqs)
            {
                progress.Report("Chain Member 조회 중 " + (++no) + " / " + chainSeqs.Count);
                DataTable members = chainService.GetChainMembers(factory, device, seq.ToString(CultureInfo.InvariantCulture));
                result.Members[seq] = members.Rows.Cast<DataRow>().Select(r => r["PLACED_ELEMENT_ID"].ToString()).ToList();
            }
            return result;
        }

        /// <summary>
        /// Layer 마다 이전 페이지 마지막 ID 다음부터 PageRows 씩 받아 행을 해석한다.
        /// GdsMapViewForm.LoadPlacedElements 와 같은 방식이다(그 함수는 화면 전용 private 이라 여기서 따로 둔다).
        /// </summary>
        private static List<MapPlacedElementData> LoadPlacedElements(DACrux.SEMDMS.RO.GdsMapView service, string factory, string device,
            IList<int> layerIds, IProgress<string> progress)
        {
            var items = new List<MapPlacedElementData>();
            foreach (int layerId in layerIds)
            {
                string after = string.Empty;
                while (true)
                {
                    int rowCount;
                    using (DataTable page = service.GetPlacedElementPage(factory, device, layerId.ToString(CultureInfo.InvariantCulture), after, PageRows))
                    {
                        foreach (DataRow row in page.Rows)
                            items.Add(GdsMapPlacedRowReader.Read(row));
                        rowCount = page.Rows.Count;
                        if (rowCount > 0) after = page.Rows[rowCount - 1]["PLACED_ELEMENT_ID"].ToString();
                    }
                    progress.Report("GDS 도면 조회 중 Layer " + layerId + " / 도형 " + items.Count.ToString("N0"));
                    if (rowCount < PageRows) break;
                }
            }
            return items;
        }

        #endregion

        #region [Die 표시]

        /// <summary>
        /// 선택 Chip 의 Defect 표시 / Chain 목록 / Defect 목록을 만들고 Defect 가 보이게 확대한다. 서버를 다시 부르지 않는다.
        /// 2026-10-12 사용자 지시로 상세 화면은 이 Chip 1개만 분석한다(Die 이동 / Wafer 합산 제거, 설계 14.4절).
        /// </summary>
        private void ShowCurrent()
        {
            if (_analysis == null || _loadedKey == null || !_currentDie.HasValue) return;
            List<DataRow> defects = CurrentDefects().ToList();
            map.SetDefectMarkers(defects.Select(r => new DefectOverlayMarker
            {
                Id = Convert.ToInt64(r["DEFECTID"], CultureInfo.InvariantCulture),
                X = Convert.ToDouble(r["GDS_X"], CultureInfo.InvariantCulture),
                Y = Convert.ToDouble(r["GDS_Y"], CultureInfo.InvariantCulture),
                Width = Convert.ToDouble(r["XSIZE"], CultureInfo.InvariantCulture),
                Height = Convert.ToDouble(r["YSIZE"], CultureInfo.InvariantCulture),
                InChain = r["IN_CHAIN"].ToString() == "Y"
            }), _tolerance);
            map.HighlightChainElements(null);
            FillChainGrid();
            ApplyChainOverlays();
            FillDefectGrid();

            GBox bounds = map.GetDefectMarkerBounds();
            if (bounds.IsEmpty) map.ZoomToFit();
            else
            {
                // 작은 범위면 주변 배선이 보이도록 최소 400 µm 를 보장한다(FocusChainBounds 는 범위의 4배를 화면에 맞춘다).
                double cx = bounds.MinX + bounds.Width / 2, cy = bounds.MinY + bounds.Height / 2;
                double half = Math.Max(200, Math.Max(bounds.Width, bounds.Height) / 2);
                map.FocusChainBounds(new GBox(cx - half, cy - half, cx + half, cy + half));
            }
            int inChain = defects.Count(r => r["IN_CHAIN"].ToString() == "Y");
            string dieText = String.Format("Die ({0}, {1})  Defect {2} / Chain 영역 {3} / 밖 {4}",
                _currentDie.Value.X, _currentDie.Value.Y, defects.Count, inChain, defects.Count - inChain);
            lblDieInfo.Text = dieText;
            SetStatus(dieText);
        }

        /// <summary>선택 Chip 의 Defect.</summary>
        private IEnumerable<DataRow> CurrentDefects()
        {
            Point die = _currentDie.Value;
            return _analysis.Tables["DEFECT"].Rows.Cast<DataRow>()
                .Where(r => Convert.ToInt32(r["XINDEX"], CultureInfo.InvariantCulture) == die.X && Convert.ToInt32(r["YINDEX"], CultureInfo.InvariantCulture) == die.Y);
        }

        /// <summary>
        /// Chain 목록: 이 Chip 에서 걸린 Defect 수를 붙이고 많은 순으로 정렬한다.
        /// [걸린 Chain 만] 이면 걸린 Chain 만 체크(걸린 Chain 이 없으면 전부 체크해 배치를 볼 수 있게 한다).
        /// </summary>
        private void FillChainGrid()
        {
            var counts = new Dictionary<long, int>();
            foreach (DataRow dc in _analysis.Tables["DIE_CHAIN"].Rows)
                if (Convert.ToInt32(dc["XINDEX"], CultureInfo.InvariantCulture) == _currentDie.Value.X && Convert.ToInt32(dc["YINDEX"], CultureInfo.InvariantCulture) == _currentDie.Value.Y)
                    counts[Convert.ToInt64(dc["CHAIN_SEQ"], CultureInfo.InvariantCulture)] = Convert.ToInt32(dc["DEFECT_COUNT"], CultureInfo.InvariantCulture);
            bool anyHit = counts.Values.Any(v => v > 0);
            _updating = true;
            dgvChains.Rows.Clear();
            foreach (DataRow c in _analysis.Tables["CHAIN"].Rows.Cast<DataRow>()
                .OrderByDescending(r => CountOf(counts, r)).ThenBy(r => r["CHAIN_CODE"].ToString().Length).ThenBy(r => r["CHAIN_CODE"].ToString(), StringComparer.Ordinal))
            {
                int n = CountOf(counts, c);
                bool show = !chkHitOnly.Checked || !anyHit || n > 0;
                int i = dgvChains.Rows.Add(show, "", c["CHAIN_CODE"], n, c["CHAIN_NAME"]);
                DataGridViewRow row = dgvChains.Rows[i];
                row.Tag = Convert.ToInt64(c["CHAIN_SEQ"], CultureInfo.InvariantCulture);
                Color color = ChainColor(c);
                row.Cells[colColor.Index].Style.BackColor = color;
                row.Cells[colColor.Index].Style.SelectionBackColor = color;
                if (n > 0) row.DefaultCellStyle.Font = new Font(dgvChains.Font, FontStyle.Bold);
            }
            _updating = false;
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

        /// <summary>체크한 Chain 만 저장 색 외곽선으로 그린다(GDS Map 조회 화면과 같은 Chain Overlay).</summary>
        private void ApplyChainOverlays()
        {
            var overlays = new List<ChainVisibleOverlay>();
            foreach (DataGridViewRow row in dgvChains.Rows)
            {
                if (!(row.Cells[colShow.Index].Value is bool) || !(bool)row.Cells[colShow.Index].Value) continue;
                IList<ChainTraceElement> elements = ChainElements((long)row.Tag);
                if (elements.Count > 0)
                    overlays.Add(new ChainVisibleOverlay(row.Cells[colCode.Index].Value.ToString(), row.Cells[colColor.Index].Style.BackColor, elements));
            }
            map.SetVisibleChainOverlays(overlays);
            VisibleChainCount = overlays.Count;
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

        /// <summary>Defect 목록: [Chain 영역] / [Chain 밖] / [전체]. 좌표는 보기 기준 GDS µm.</summary>
        private void FillDefectGrid()
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(long));
            table.Columns.Add("Chain", typeof(string));
            table.Columns.Add("Class", typeof(int));
            table.Columns.Add("크기", typeof(string));
            table.Columns.Add("Die", typeof(string));
            table.Columns.Add("GDS X", typeof(double));
            table.Columns.Add("GDS Y", typeof(double));
            table.Columns.Add("XREL", typeof(double));
            table.Columns.Add("YREL", typeof(double));
            int total = 0, inChain = 0;
            foreach (DataRow r in CurrentDefects())
            {
                bool hit = r["IN_CHAIN"].ToString() == "Y";
                total++;
                if (hit) inChain++;
                if ((cboScope.SelectedIndex == 0 && !hit) || (cboScope.SelectedIndex == 1 && hit)) continue;
                table.Rows.Add(r["DEFECTID"], hit ? r["CHAIN_CODES"] : "(밖)", r["CLASSNUMBER"],
                    String.Format(CultureInfo.InvariantCulture, "{0:0.##} x {1:0.##}", r["XSIZE"], r["YSIZE"]),
                    "(" + r["XINDEX"] + ", " + r["YINDEX"] + ")", r["GDS_X"], r["GDS_Y"], r["XREL"], r["YREL"]);
            }
            _updating = true;
            dgvDefects.DataSource = table;
            dgvDefects.ClearSelection();
            _updating = false;
            lblDefectCount.Text = String.Format("Chain 영역 {0} / 밖 {1}", inChain, total - inChain);
        }

        #endregion

        #region [이벤트]

        private async void chkBackground_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _busy) return;
            try
            {
                await LoadMapAsync();
                ShowCurrent();
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

        private void btnFit_Click(object sender, EventArgs e)
        {
            map.ZoomToFit();
        }

        private void chkHitOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _loadedKey == null) return;
            FillChainGrid();
            ApplyChainOverlays();
        }

        /// <summary>표시 체크를 바꾸면 바로 지도에 반영한다.</summary>
        private void dgvChains_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_updating || e.RowIndex < 0 || e.ColumnIndex != colShow.Index) return;
            dgvChains.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ApplyChainOverlays();
        }

        /// <summary>Chain 행 더블클릭: 그 Chain 전체 범위로 이동한다.</summary>
        private void dgvChains_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            IList<ChainTraceElement> elements = ChainElements((long)dgvChains.Rows[e.RowIndex].Tag);
            if (elements.Count == 0) return;
            map.FocusChainBounds(new GBox(elements.Min(x => x.Bounds.MinX), elements.Min(x => x.Bounds.MinY),
                elements.Max(x => x.Bounds.MaxX), elements.Max(x => x.Bounds.MaxY)));
        }

        /// <summary>
        /// Defect 행 선택: 지도에서 그 Defect 를 강조(허용오차 점선)하고, 걸린 Member 도형을 선택 색으로 칠한 뒤 주변으로 이동한다.
        /// </summary>
        private void dgvDefects_SelectionChanged(object sender, EventArgs e)
        {
            if (_updating || dgvDefects.SelectedRows.Count == 0) return;
            long id = Convert.ToInt64(dgvDefects.SelectedRows[0].Cells["ID"].Value, CultureInfo.InvariantCulture);
            map.SelectDefectMarker(id);
            List<string> hitMembers = _analysis.Tables["DEFECT_HIT"].Rows.Cast<DataRow>()
                .Where(r => Convert.ToInt64(r["DEFECTID"], CultureInfo.InvariantCulture) == id)
                .Select(r => r["PLACED_ELEMENT_ID"].ToString()).ToList();
            map.HighlightChainElements(hitMembers);
            double x = Convert.ToDouble(dgvDefects.SelectedRows[0].Cells["GDS X"].Value, CultureInfo.InvariantCulture);
            double y = Convert.ToDouble(dgvDefects.SelectedRows[0].Cells["GDS Y"].Value, CultureInfo.InvariantCulture);
            map.FocusChainBounds(new GBox(x - 100, y - 100, x + 100, y + 100));
            SetStatus(String.Format("Defect #{0} / GDS ({1:0.###}, {2:0.###}) µm / 걸린 Member {3}", id, x, y, hitMembers.Count));
        }

        /// <summary>Defect 행 더블클릭: DMS 호스트에 이미지 보기를 요청한다.</summary>
        private void dgvDefects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long id = Convert.ToInt64(dgvDefects.Rows[e.RowIndex].Cells["ID"].Value, CultureInfo.InvariantCulture);
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

        /// <summary>지도에서 Defect 를 클릭하면 목록 행을 고른다(목록 범위 밖이면 [전체] 로 바꾼다).</summary>
        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            long? id = map.FindDefectMarkerAt(e.Location);
            if (!id.HasValue) return;
            DataGridViewRow row = FindDefectRow(id.Value);
            if (row == null && cboScope.SelectedIndex != 2)
            {
                cboScope.SelectedIndex = 2;
                row = FindDefectRow(id.Value);
            }
            if (row == null) return;
            dgvDefects.ClearSelection();
            row.Selected = true;
            dgvDefects.FirstDisplayedScrollingRowIndex = row.Index;
        }

        private DataGridViewRow FindDefectRow(long id)
        {
            return dgvDefects.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Convert.ToInt64(r.Cells["ID"].Value, CultureInfo.InvariantCulture) == id);
        }

        private void cboScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating || _analysis == null || _loadedKey == null) return;
            FillDefectGrid();
        }

        private void map_MouseWorldPositionChanged(object sender, PointF position)
        {
            lblCoordinate.Text = String.Format("X: {0:0.###} / Y: {1:0.###} µm", position.X, position.Y);
        }

        #endregion

        #region [Helper]

        private void SetBusy(bool busy)
        {
            _busy = busy;
            progressLoad.Visible = busy;
            progressLoad.Style = ProgressBarStyle.Marquee;
            chkBackground.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
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

        /// <summary>DB 에서 읽은 도면 묶음(백그라운드 -> 화면 스레드 전달용).</summary>
        private sealed class LoadedMap
        {
            public string TopStructure;
            public string StoredSide;
            public readonly Dictionary<int, Color> Colors = new Dictionary<int, Color>();
            public List<MapPlacedElementData> Items = new List<MapPlacedElementData>();
            public readonly Dictionary<long, IList<string>> Members = new Dictionary<long, IList<string>>();
        }

        #endregion
    }
}
