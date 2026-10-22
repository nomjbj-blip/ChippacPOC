using NexplantQMS.GdsMap.Chain;
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
    /// GDS Map View U8 Chain 일괄 생성 / 저장된 Chain 목록 (2026-10-20, 설계 6 / 7절, D3 ~ D5).
    /// 저장 형식은 기존 GDS Map View 와 같다(KLARF 연관 분석 / Die 분석 / E-Test Para 가 그대로 읽음).
    ///  - CHAIN_CODE = 번호 Label(TSV_2162) / 이름 = Net Label + 번호(없으면 Code)
    ///  - 탐색 Layer = 예시 Layer / Layer 규칙 미적용 / 격자 10
    ///  - 번호 Label 이 든 도형 = INPUT / ENDPOINT, Marker = Label 위치(저장 기준 X). 나머지 = MEMBER / TRACE
    /// 서버 호출: CreateChainBulk(포함 도형 1만 행 이하씩) / GetChainCodeList / DeleteChainBulk / GetChainMembers(선택 Chain 표시).
    /// </summary>
    public partial class GdsMapViewU8Form
    {
        /// <summary>일괄 저장 한 번에 보내는 포함 도형 최대 행 수(서버 제한과 같다).</summary>
        private const int BulkMemberRows = 10000;
        /// <summary>삭제 한 번에 보내는 Chain 수.</summary>
        private const int BulkDeleteChains = 1000;
        /// <summary>목록에서 여러 Chain 을 고르면 지도에 함께 그리는 최대 수(Chain 마다 서버 호출 1번).</summary>
        private const int SavedShowLimit = 20;
        /// <summary>'표시' 체크한 Chain 포함 도형을 CHAIN_SEQ 범위로 묶어 읽을 때 한 번에 묶는 Chain 수(기존 GDS Map View 와 같다).</summary>
        private const int SavedMemberRangeSize = 5000;
        /// <summary>[전체 체크]로 이보다 많이 지도에 그리면 먼저 묻는다(기존 GDS Map View Chain 목록과 같은 기준).</summary>
        private const int SavedShowConfirm = 1000;
        /// <summary>저장 격자 크기(기존 GDS Map View 기본값과 같다). U8 덩어리 계산에는 쓰지 않는다.</summary>
        private const string SavedCellSize = "10";
        /// <summary>이보다 많이 만들면 확인 창에 기존 화면이 느려진다는 안내를 함께 보인다.</summary>
        private const int ManyChainsWarning = 1000;
        /// <summary>색 변경 한 번에 보내는 Chain 수(서버 제한과 같다).</summary>
        private const int BulkColorChains = 10000;

        private bool _chainDbEnabled;
        private bool _chainDbBusy;
        private List<SavedChain> _savedRows = new List<SavedChain>();
        private List<SavedChain> _savedView = new List<SavedChain>();
        private readonly HashSet<string> _dbCodes = new HashSet<string>(StringComparer.Ordinal);
        private bool _updatingSaved;
        private int _savedShowGeneration;
        /// <summary>읽어 둔 Chain 포함 도형 ID(CHAIN_SEQ 별). 목록을 다시 읽거나 Device 를 바꾸면 비운다 (2026-10-21).</summary>
        private readonly Dictionary<string, string[]> _savedMemberKeys = new Dictionary<string, string[]>(StringComparer.Ordinal);

        /// <summary>DB 의 Chain 한 개(GetChainCodeList 행).</summary>
        private sealed class SavedChain
        {
            public string Seq;
            public string Code;
            public string Name;
            public Color Color;
            public string Status;
            /// <summary>목록 '표시' 체크: 고르지 않아도 지도에 계속 그린다 (2026-10-21).</summary>
            public bool Show;
        }

        /// <summary>
        /// DB 저장 / 목록은 DMS 호스트 안에서만 쓴다(단독 실행에는 서비스 연결 정보가 없다).
        /// 단독 실행이면 [Chain 생성(DB)]를 숨기고 [저장된 Chain] 탭 버튼은 막는다.
        /// </summary>
        private void InitializeChainDbState(bool enabled)
        {
            _chainDbEnabled = enabled;
            btnCreate.Visible = enabled;
            lblSavedSummary.Text = enabled ? "Chain 0개" : "DACrux 메뉴로 열었을 때만 DB Chain 을 볼 수 있습니다.";
            UpdateU8Buttons();
        }

        #region [Chain 생성(DB)]

        /// <summary>
        /// [Chain 생성(DB)] (찾기 결과 탭): 지금 목록(분류 / 찾기로 거르고 뺀 뒤 보이는 행) 중 일치 + DB 없음을 Chain 으로 저장한다.
        /// 처리 흐름: 확인 -> 포함 도형 1만 행 이하로 나눠 CreateChainBulk -> 결과(생성 / 건너뜀) -> 저장된 Chain 다시 읽기.
        /// 중간에 실패하면 서버가 그 호출에서 만든 Chain 만 지운다. 앞 호출에서 만든 Chain 은 남고 결과에 "생성"으로 보인다.
        /// </summary>
        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (!_chainDbEnabled || _shownMap == null || _resultExtractor == null || _chainDbBusy) return;
            List<U8Result> targets = CreateTargetsInView();
            if (targets.Count == 0)
            {
                MessageBox.Show(this, "지금 목록에 만들 Chain 이 없습니다. 분류 / 찾기 조건을 확인하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int inDb = _resultView.Count(r => r.IsMatch && r.InDb);
            int review = _resultView.Count(r => !r.IsMatch);
            string layerText = string.Join(", ", _templateLayers.Select(GdsLayerKey.Format));
            string sample = string.Join(", ", targets.Take(3).Select(r => r.Name));
            bool many = targets.Count > ManyChainsWarning;
            string text = "Device " + _shownMap.DeviceId + " 에 Chain " + targets.Count.ToString("N0") + "개를 만듭니다.\r\n\r\n"
                + "대상: 찾기 결과 지금 목록 " + _resultView.Count.ToString("N0") + "행 중 일치 + DB 없음"
                + (inDb > 0 || review > 0 ? " (제외: " + (inDb > 0 ? "DB 있음 " + inDb.ToString("N0") : "")
                    + (inDb > 0 && review > 0 ? " / " : "") + (review > 0 ? "검토 " + review.ToString("N0") : "") + ")" : "") + "\r\n"
                + "이름 예: " + sample + (targets.Count > 3 ? " ..." : "") + "\r\n"
                + "탐색 Layer: " + layerText + "\r\n"
                + (many ? "\r\nChain 이 많으면 기존 GDS Map View 에서 이 Device 를 열거나 E-Test Para 의 GDS Chain 목록을 볼 때 많이 느려질 수 있습니다.\r\n" : "")
                + "\r\n만드시겠습니까?";
            if (MessageBox.Show(this, text, "Chain 생성", MessageBoxButtons.YesNo, many ? MessageBoxIcon.Warning : MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            string done = await CreateChainsAsync(targets);
            if (done != null)
                MessageBox.Show(this, done, "Chain 생성", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>생성 대상을 저장하고 결과 문구를 돌려준다. 실패하면 사유를 보이고 null. 확인 창은 부르는 쪽이 띄운다.</summary>
        private async Task<string> CreateChainsAsync(List<U8Result> targets)
        {
            string factory = _shownMap.Factory, device = _shownMap.DeviceId;
            string user = Environment.UserName;
            string done = null;
            string[,] layers = BuildLayerRows(_templateLayers);
            List<List<U8Result>> chunks = SplitByMembers(targets);
            int created = 0, skipped = 0, sent = 0;
            SetChainDbBusy(true);
            progressLoad.Style = ProgressBarStyle.Continuous;
            progressLoad.Value = 0;
            progressLoad.Visible = true;
            try
            {
                var service = ChainRo();
                // 한 번의 생성(같은 Rule) = 한 색. 이미 저장된 Chain 색 / 지도 Layer 색과 가장 먼 색 하나를 고른다 (2026-10-21).
                Color ruleColor = ChainColorPicker.Pick(_savedRows.Select(c => c.Color).Distinct(),
                    _shownColors == null ? null : _shownColors.Values);
                foreach (List<U8Result> chunk in chunks)
                {
                    string[,] chains;
                    string[,] members;
                    BuildChunkRows(chunk, ruleColor, out chains, out members);
                    string[] key = { factory, device, "0", SavedCellSize };
                    SetStatus("Chain 저장 중: " + sent.ToString("N0") + " / " + targets.Count.ToString("N0"));
                    DataTable result = await Task.Run(() => service.CreateChainBulk(key, chains, layers, members, user));
                    var byCode = chunk.ToDictionary(r => r.Code, StringComparer.Ordinal);
                    foreach (DataRow row in result.Rows)
                    {
                        U8Result r;
                        if (!byCode.TryGetValue(row["CHAIN_CODE"].ToString(), out r)) continue;
                        r.InDb = true;
                        if (string.Equals(row["RESULT"].ToString(), "CREATED", StringComparison.Ordinal)) { r.Created = true; created++; }
                        else skipped++;
                    }
                    sent += chunk.Count;
                    progressLoad.Value = (int)Math.Min(100, sent * 100L / targets.Count);
                }
                done = "Chain 생성 완료: 생성 " + created.ToString("N0") + (skipped > 0 ? " / 건너뜀(같은 Code 있음) " + skipped.ToString("N0") : "");
                SetStatus(done);
            }
            catch (Exception ex)
            {
                ShowError("Chain 생성(DB) (생성 " + created.ToString("N0") + "개까지 저장됨)", ex);
            }
            finally
            {
                progressLoad.Visible = false;
                SetChainDbBusy(false);
            }
            await RefreshSavedChainsAsync(false);
            dgvResult.Invalidate();
            ShowResultPreview();
            UpdateResultSummary();
            return done;
        }

        /// <summary>탐색 Layer 행(LAYER_ID / DATA_TYPE).</summary>
        private static string[,] BuildLayerRows(IList<int> layerKeys)
        {
            var rows = new string[layerKeys.Count, 2];
            for (int i = 0; i < layerKeys.Count; i++)
            {
                rows[i, 0] = GdsLayerKey.Layer(layerKeys[i]).ToString(CultureInfo.InvariantCulture);
                rows[i, 1] = GdsLayerKey.DataType(layerKeys[i]).ToString(CultureInfo.InvariantCulture);
            }
            return rows;
        }

        /// <summary>포함 도형 합계가 BulkMemberRows 이하가 되도록 Chain 을 나눈다.</summary>
        private static List<List<U8Result>> SplitByMembers(IList<U8Result> targets)
        {
            var chunks = new List<List<U8Result>>();
            var current = new List<U8Result>();
            int rows = 0;
            foreach (U8Result r in targets)
            {
                int n = r.Component.Elements.Count;
                if (current.Count > 0 && rows + n > BulkMemberRows)
                {
                    chunks.Add(current);
                    current = new List<U8Result>();
                    rows = 0;
                }
                current.Add(r);
                rows += n;
            }
            if (current.Count > 0) chunks.Add(current);
            return chunks;
        }

        /// <summary>
        /// 한 번에 보낼 Chain / 포함 도형 행을 만든다.
        /// INPUT = 번호 Label 이 든 도형(Label 을 덩어리에 붙일 때와 같은 판정), Marker = Label 위치(저장 기준 X).
        /// 색은 이번 생성 전체에 같은 색(color)을 쓴다 (2026-10-21, 같은 Rule 로 만든 Chain 은 한 색).
        /// </summary>
        private void BuildChunkRows(IList<U8Result> chunk, Color color, out string[,] chains, out string[,] members)
        {
            chains = new string[chunk.Count, 3];
            int memberRows = chunk.Sum(r => r.Component.Elements.Count);
            members = new string[memberRows, 6];
            int m = 0;
            for (int i = 0; i < chunk.Count; i++)
            {
                U8Result r = chunk[i];
                string name = r.Name ?? r.Code;
                if (name.Length > 200) name = name.Substring(0, 200);
                chains[i, 0] = r.Code;
                chains[i, 1] = name;
                chains[i, 2] = color.ToArgb().ToString(CultureInfo.InvariantCulture);

                ChainComponentLabel label = r.Component.Labels.First(l => l.LayerKey == _rule.NumberLabelLayerKey);
                ChainTraceElement input = _resultExtractor.FindElementAt(label.Point);
                if (input == null || !r.Component.Elements.Any(el => el.ElementKey == input.ElementKey))
                    input = r.Component.Elements[0];
                string index = i.ToString(CultureInfo.InvariantCulture);
                foreach (ChainTraceElement element in r.Component.Elements)
                {
                    bool isInput = element.ElementKey == input.ElementKey;
                    members[m, 0] = index;
                    members[m, 1] = element.ElementKey;
                    members[m, 2] = isInput ? "INPUT" : "MEMBER";
                    members[m, 3] = isInput ? "ENDPOINT" : "TRACE";
                    members[m, 4] = isInput ? ToStoredX(label.Point.X).ToString("G17", CultureInfo.InvariantCulture) : string.Empty;
                    members[m, 5] = isInput ? label.Point.Y.ToString("G17", CultureInfo.InvariantCulture) : string.Empty;
                    m++;
                }
            }
        }

        /// <summary>저장 / 삭제 중에는 조회 / 찾기 / 다른 저장을 막는다.</summary>
        private void SetChainDbBusy(bool busy)
        {
            _chainDbBusy = busy;
            if (busy) ViewBusy.Start();
            else ViewBusy.Stop();
            btnSearch.Enabled = !busy && !_loading;
            cboDevice.Enabled = !busy && !_loading;
            cboViewSide.Enabled = !busy && !_loading;
            UpdateU8Buttons();
        }

        #endregion

        #region [저장된 Chain]

        private void ClearSavedChains()
        {
            _savedShowGeneration++;
            _savedRows = new List<SavedChain>();
            _savedView = new List<SavedChain>();
            _dbCodes.Clear();
            _savedMemberKeys.Clear();
            _updatingSaved = true;
            try { dgvSaved.RowCount = 0; }
            finally { _updatingSaved = false; }
            lblSavedSummary.Text = "Chain 0개";
            if (map.Structure != null) map.SetVisibleChainOverlays(null);
            UpdateU8Buttons();
        }

        private async void btnSavedRefresh_Click(object sender, EventArgs e)
        {
            await RefreshSavedChainsAsync(true);
        }

        /// <summary>DB 의 Chain 목록(포함 도형 수 계산 없음)을 다시 읽고 찾기 결과의 "DB 있음"을 맞춘다.</summary>
        private async Task RefreshSavedChainsAsync(bool announce)
        {
            if (!_chainDbEnabled || _shownMap == null) return;
            string factory = _shownMap.Factory, device = _shownMap.DeviceId;
            try
            {
                DataTable table;
                using (ViewBusy.Busy())
                    table = await Task.Run(() => ChainRo().GetChainCodeList(factory, device));
                if (_shownMap == null || _shownMap.DeviceId != device) return;
                // 다시 읽어도 '표시' 체크는 같은 Chain(CHAIN_SEQ)에 남긴다 (2026-10-21)
                var showSeqs = new HashSet<string>(_savedRows.Where(c => c.Show).Select(c => c.Seq), StringComparer.Ordinal);
                var rows = new List<SavedChain>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    int argb;
                    rows.Add(new SavedChain
                    {
                        Seq = row["CHAIN_SEQ"].ToString(),
                        Code = row["CHAIN_CODE"].ToString(),
                        Name = row["CHAIN_NAME"].ToString(),
                        Color = int.TryParse(row["COLOR_ARGB"].ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out argb) ? Color.FromArgb(argb) : Color.Yellow,
                        Status = row["VERSION_STATUS"].ToString(),
                        Show = showSeqs.Contains(row["CHAIN_SEQ"].ToString())
                    });
                }
                _savedRows = rows;
                _savedMemberKeys.Clear();
                _dbCodes.Clear();
                foreach (SavedChain chain in rows) _dbCodes.Add(chain.Code);
                ApplySavedFilter();
                ApplyDbState();
                dgvResult.Invalidate();
                UpdateResultSummary();
                if (announce) SetStatus("저장된 Chain " + rows.Count.ToString("N0") + "개");
            }
            catch (Exception ex)
            {
                ShowError("저장된 Chain 조회", ex);
            }
        }

        private void txtSavedFind_TextChanged(object sender, EventArgs e)
        {
            ApplySavedFilter();
        }

        /// <summary>찾기 글자(이름 / Code 포함)로 목록을 거른다.</summary>
        private void ApplySavedFilter()
        {
            string find = txtSavedFind.Text.Trim();
            _savedView = find.Length == 0 ? _savedRows.ToList()
                : _savedRows.Where(c => c.Name.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0
                    || c.Code.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            _updatingSaved = true;
            try
            {
                dgvSaved.RowCount = 0;
                dgvSaved.RowCount = _savedView.Count;
                dgvSaved.ClearSelection();
            }
            finally
            {
                _updatingSaved = false;
            }
            dgvSaved.Invalidate();
            UpdateSavedSummary();
            ShowSelectedSavedChains();
            UpdateU8Buttons();
        }

        private void dgvSaved_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _savedView.Count) return;
            SavedChain chain = _savedView[e.RowIndex];
            if (e.ColumnIndex == colSavedShow.Index) e.Value = chain.Show;
            else if (e.ColumnIndex == colSavedName.Index) e.Value = chain.Name;
            else if (e.ColumnIndex == colSavedCode.Index) e.Value = chain.Code;
            else if (e.ColumnIndex == colSavedStatus.Index) e.Value = chain.Status == "READY" ? "완료" : "저장 미완료(" + chain.Status + ")";
        }

        /// <summary>'색' 칸은 글자 없이 Chain 색으로 칠한다(선택 행에서도 같은 색) (2026-10-21).</summary>
        private void dgvSaved_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != colSavedColor.Index || e.RowIndex < 0 || e.RowIndex >= _savedView.Count) return;
            Color color = _savedView[e.RowIndex].Color;
            e.CellStyle.BackColor = color;
            e.CellStyle.SelectionBackColor = color;
            e.Value = string.Empty;
            e.FormattingApplied = true;
        }

        private void dgvSaved_SelectionChanged(object sender, EventArgs e)
        {
            if (_updatingSaved) return;
            ShowSelectedSavedChains();
            UpdateU8Buttons();
        }

        /// <summary>
        /// '표시' 칸(체크 칸) 클릭으로 체크를 바꾼다. 체크한 Chain 은 고르지 않아도 지도에 계속 그린다 (2026-10-21).
        /// 체크 칸 더블클릭은 두 번 바뀌어 그대로다(일반 체크 박스와 같음). 두 번째 클릭은 CellContentClick 이 아니라 CellContentDoubleClick 으로 온다.
        /// </summary>
        private void dgvSaved_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ToggleSavedShow(e.ColumnIndex, e.RowIndex);
        }

        private void dgvSaved_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ToggleSavedShow(e.ColumnIndex, e.RowIndex);
        }

        private void ToggleSavedShow(int column, int row)
        {
            if (column != colSavedShow.Index || row < 0 || row >= _savedView.Count || _chainDbBusy || _loading) return;
            SavedChain chain = _savedView[row];
            chain.Show = !chain.Show;
            dgvSaved.InvalidateCell(column, row);
            UpdateSavedSummary();
            ShowSavedChainsOnMap(false);
        }

        /// <summary>[전체 체크]: 목록에 보이는 Chain(찾기로 거른 뒤)을 모두 지도에 표시한다(Layer [전체] 와 같은 방식) (2026-10-21).</summary>
        private void btnSavedCheckAll_Click(object sender, EventArgs e)
        {
            if (_savedView.Count == 0) return;
            int after = _savedRows.Count(c => c.Show) + _savedView.Count(c => !c.Show);
            if (after > SavedShowConfirm
                && MessageBox.Show(this, "Chain " + after.ToString("N0") + "개를 지도에 표시합니다.\r\n\r\n"
                    + "포함 도형을 읽고 외곽선을 그리는 데 시간이 걸리고 메모리를 많이 쓸 수 있습니다. 계속하시겠습니까?",
                    "전체 체크", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            SetAllSavedShow(_savedView, true);
        }

        /// <summary>[전체 미체크]: 모든 Chain 의 지도 표시 체크를 끈다(찾기로 가려진 행 포함). 목록에서 고른 Chain 은 계속 그린다 (2026-10-21).</summary>
        private void btnSavedCheckNone_Click(object sender, EventArgs e)
        {
            SetAllSavedShow(_savedRows, false);
        }

        private void SetAllSavedShow(IEnumerable<SavedChain> chains, bool show)
        {
            foreach (SavedChain chain in chains) chain.Show = show;
            dgvSaved.Invalidate();
            UpdateSavedSummary();
            ShowSavedChainsOnMap(false);
        }

        /// <summary>목록 위 요약: 전체 수 / 찾기로 거른 목록 수 / 지도 표시(체크) 수.</summary>
        private void UpdateSavedSummary()
        {
            int shown = _savedRows.Count(c => c.Show);
            lblSavedSummary.Text = "Chain " + _savedRows.Count.ToString("N0") + "개"
                + (_savedView.Count != _savedRows.Count ? " / 목록 " + _savedView.Count.ToString("N0") + "개" : "")
                + (shown > 0 ? " / 지도 표시 " + shown.ToString("N0") + "개" : "");
        }

        private List<SavedChain> SelectedSavedChains()
        {
            return dgvSaved.SelectedRows.Cast<DataGridViewRow>().Select(row => row.Index)
                .Where(i => i >= 0 && i < _savedView.Count).OrderBy(i => i).Select(i => _savedView[i]).ToList();
        }

        /// <summary>목록에서 고른 Chain(최대 SavedShowLimit 개)을 그리고 그 범위로 이동한다. '표시' 체크한 Chain 도 함께 그린다.</summary>
        private void ShowSelectedSavedChains()
        {
            ShowSavedChainsOnMap(true);
        }

        /// <summary>
        /// 저장된 Chain 을 Chain 색 외곽선으로 그린다: '표시' 체크한 Chain 전체 + 목록에서 고른 Chain(최대 SavedShowLimit 개) (2026-10-21).
        /// 처리 흐름: 포함 도형 ID 를 아직 읽지 않은 Chain 만 서버에서 읽음(ReadSavedMembers, 읽은 것은 _savedMemberKeys 에 보관)
        ///  -> 늦게 끝난 조회는 선택 / 체크가 바뀌었으면 버림 -> 체크한 Chain, 고른 Chain 순서로 외곽선(고른 Chain 이 위)
        ///  -> focusSelection 이면 고른 Chain 범위로 이동. 체크를 바꿀 때(focusSelection = false)는 화면 위치를 그대로 둔다.
        /// </summary>
        private async void ShowSavedChainsOnMap(bool focusSelection)
        {
            int generation = ++_savedShowGeneration;
            if (map.Structure == null || !_chainDbEnabled || _shownMap == null) return;
            List<SavedChain> selected = SelectedSavedChains();
            List<SavedChain> shown = selected.Take(SavedShowLimit).ToList();
            List<SavedChain> checkedChains = _savedRows.Where(c => c.Show).ToList();
            if (shown.Count == 0 && checkedChains.Count == 0)
            {
                map.SetVisibleChainOverlays(null);
                if (!focusSelection) SetStatus("지도에 표시한 저장된 Chain 이 없습니다.");
                return;
            }
            string factory = _shownMap.Factory, device = _shownMap.DeviceId;
            List<SavedChain> unread = checkedChains.Concat(shown).Where(c => !_savedMemberKeys.ContainsKey(c.Seq)).Distinct().ToList();
            try
            {
                if (unread.Count > 0)
                {
                    Dictionary<string, string[]> read;
                    // 많이 읽을 때만 모래시계(목록에서 몇 개 고를 때는 예전처럼 바로 그린다)
                    using (unread.Count > SavedShowLimit ? ViewBusy.Busy() : null)
                        read = await Task.Run(() => ReadSavedMembers(factory, device, unread));
                    if (_shownMap == null || _shownMap.DeviceId != device) return;
                    foreach (KeyValuePair<string, string[]> pair in read) _savedMemberKeys[pair.Key] = pair.Value;
                }
                if (generation != _savedShowGeneration || map.Structure == null) return;
                var selectedSet = new HashSet<SavedChain>(shown);
                var overlays = new List<ChainVisibleOverlay>(checkedChains.Count + shown.Count);
                GBox area = GBox.Empty;
                int missing = 0;
                foreach (SavedChain chain in checkedChains.Where(c => !selectedSet.Contains(c)).Concat(shown))
                {
                    string[] keys;
                    if (!_savedMemberKeys.TryGetValue(chain.Seq, out keys)) keys = new string[0];
                    var elements = new List<ChainTraceElement>(keys.Length);
                    foreach (string key in keys)
                    {
                        ChainTraceElement element;
                        if (_elementByKey.TryGetValue(key, out element)) elements.Add(element);
                        else missing++;
                    }
                    if (selectedSet.Contains(chain))
                        foreach (ChainTraceElement element in elements)
                        {
                            area.Include(element.Bounds.MinX, element.Bounds.MinY);
                            area.Include(element.Bounds.MaxX, element.Bounds.MaxY);
                        }
                    overlays.Add(new ChainVisibleOverlay(chain.Code, chain.Color, elements));
                }
                map.SetVisibleChainOverlays(overlays);
                if (focusSelection && !area.IsEmpty) map.FocusChainBounds(area);
                string status;
                if (focusSelection && shown.Count > 0)
                {
                    string[] firstKeys;
                    if (!_savedMemberKeys.TryGetValue(shown[0].Seq, out firstKeys)) firstKeys = new string[0];
                    status = selected.Count == 1
                        ? shown[0].Name + (shown[0].Code != shown[0].Name ? " / Code " + shown[0].Code : "") + " / 도형 " + firstKeys.Length
                        : "선택 Chain " + selected.Count.ToString("N0") + "개" + (selected.Count > SavedShowLimit ? "(지도에는 앞 " + SavedShowLimit + "개)" : "");
                    if (checkedChains.Count > 0) status += " / 체크 표시 " + checkedChains.Count.ToString("N0") + "개";
                }
                else if (!focusSelection)
                    status = "지도 표시(체크) Chain " + checkedChains.Count.ToString("N0") + "개" + (shown.Count > 0 ? " + 선택 " + shown.Count.ToString("N0") + "개" : "");
                else
                    return; // 고른 Chain 없이 다시 그림(찾기 / 보기 기준 바꿈): 상태 줄은 부른 쪽 문구를 그대로 둔다
                if (missing > 0) status += " / 조회하지 않은 Layer 도형 " + missing.ToString("N0") + "개는 그리지 않음";
                SetStatus(status);
            }
            catch (Exception ex)
            {
                if (generation == _savedShowGeneration) ShowError("Chain 포함 도형 조회", ex);
            }
        }

        /// <summary>
        /// Chain 포함 도형 ID(PLACED_ELEMENT_ID)를 읽는다(작업 스레드). 반환: CHAIN_SEQ -> 도형 ID(없으면 빈 배열).
        /// SavedShowLimit 개 이하면 Chain 마다 읽고(예전 방식), 많으면 CHAIN_SEQ 범위로 묶어 읽어 범위 안 다른 Chain 행은 버린다.
        /// 서버에 범위 조회가 없으면(이전 서버) Chain 마다 읽는 방식으로 돌아간다 (2026-10-21).
        /// </summary>
        private Dictionary<string, string[]> ReadSavedMembers(string factory, string device, List<SavedChain> chains)
        {
            var service = ChainRo();
            var result = new Dictionary<string, string[]>(StringComparer.Ordinal);
            if (chains.Count > SavedShowLimit)
            {
                var wanted = new Dictionary<long, string>();
                foreach (SavedChain chain in chains)
                {
                    long seq;
                    if (long.TryParse(chain.Seq, NumberStyles.Integer, CultureInfo.InvariantCulture, out seq)) wanted[seq] = chain.Seq;
                }
                List<long> seqs = wanted.Keys.OrderBy(s => s).ToList();
                var lists = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                bool rangeAvailable = false;
                try
                {
                    for (int start = 0; start < seqs.Count; start += SavedMemberRangeSize)
                    {
                        int end = Math.Min(start + SavedMemberRangeSize, seqs.Count) - 1;
                        using (DataTable rows = ReadSavedMemberRange(service, factory, device, seqs[start], seqs[end]))
                        {
                            rangeAvailable = true;
                            foreach (DataRow row in rows.Rows)
                            {
                                string seq;
                                if (!wanted.TryGetValue(Convert.ToInt64(row["CHAIN_SEQ"], CultureInfo.InvariantCulture), out seq)) continue;
                                List<string> list;
                                if (!lists.TryGetValue(seq, out list)) lists[seq] = list = new List<string>();
                                list.Add(row["PLACED_ELEMENT_ID"].ToString());
                            }
                        }
                    }
                    foreach (SavedChain chain in chains)
                    {
                        List<string> list;
                        result[chain.Seq] = lists.TryGetValue(chain.Seq, out list) ? list.ToArray() : new string[0];
                    }
                    return result;
                }
                catch (Exception ex) when (!rangeAvailable && (ex is System.Runtime.Remoting.RemotingException || ex is MissingMethodException))
                {
                    // 이전 서버(범위 조회 없음): 아래에서 Chain 마다 읽는다.
                }
            }
            foreach (SavedChain chain in chains)
                using (DataTable t = service.GetChainMembers(factory, device, chain.Seq))
                    result[chain.Seq] = t.Rows.Cast<DataRow>().Select(row => row["PLACED_ELEMENT_ID"].ToString()).ToArray();
            return result;
        }

        /// <summary>범위 조회 1번. 이전 RO(함수 없음)면 이 함수를 부를 때 MissingMethodException 이 나도록 따로 둔다.</summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static DataTable ReadSavedMemberRange(DACrux.SEMDMS.RO.GdsChain service, string factory, string device, long fromSeq, long toSeq)
        {
            return service.GetChainMembersRange(factory, device, fromSeq.ToString(CultureInfo.InvariantCulture), toSeq.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// [색 변경]: 목록에서 고른 Chain(여러 개 가능, Ctrl+A = 목록 전체)의 색을 한 색으로 바꿔 DB 에 바로 저장한다 (2026-10-21).
        /// 처리 흐름: 색 선택 창(처음 색 = 첫 Chain 색, 사용자 정의 색 = 추천 16색) -> 여러 개면 확인 -> 1만 개씩 UpdateChainColorBulk
        ///  -> 목록 '색' 칸 / 지도 외곽선 색을 바꾼다. 포함 도형 / 이름 / 상태는 바꾸지 않는다.
        /// </summary>
        private async void btnSavedColor_Click(object sender, EventArgs e)
        {
            if (!_chainDbEnabled || _shownMap == null || _chainDbBusy) return;
            List<SavedChain> chains = SelectedSavedChains();
            if (chains.Count == 0)
            {
                MessageBox.Show(this, "색을 바꿀 Chain 을 목록에서 고르세요.\r\n여러 개는 Ctrl / Shift + 클릭, 목록 전체는 Ctrl+A 로 고릅니다.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Color color;
            using (var dialog = new ColorDialog { Color = chains[0].Color, FullOpen = true, AnyColor = true })
            {
                // ColorDialog 사용자 정의 색은 0x00BBGGRR 형식이다.
                dialog.CustomColors = ChainColorPicker.SuggestedColors.Select(c => c.R | (c.G << 8) | (c.B << 16)).ToArray();
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                color = Color.FromArgb(255, dialog.Color);
            }
            if (chains.Count > 1
                && MessageBox.Show(this, "고른 Chain " + chains.Count.ToString("N0") + "개의 색을 모두 같은 색으로 바꿔 DB 에 저장합니다.\r\n\r\n"
                    + string.Join(", ", chains.Take(5).Select(c => c.Name)) + (chains.Count > 5 ? " ..." : "") + "\r\n\r\n바꾸시겠습니까?",
                    "Chain 색 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            await ChangeSavedChainColorAsync(chains, color);
        }

        /// <summary>Chain 색을 1만 개씩 DB 에 저장하고 화면 색을 바꾼다. 반환: 바꾼 Chain 수(실패하면 그때까지). 확인 창은 부르는 쪽이 띄운다.</summary>
        private async Task<int> ChangeSavedChainColorAsync(List<SavedChain> chains, Color color)
        {
            string factory = _shownMap.Factory, device = _shownMap.DeviceId, user = Environment.UserName;
            string argb = color.ToArgb().ToString(CultureInfo.InvariantCulture);
            int changed = 0;
            string done = null;
            SetChainDbBusy(true);
            try
            {
                var service = ChainRo();
                for (int start = 0; start < chains.Count; start += BulkColorChains)
                {
                    List<SavedChain> part = chains.Skip(start).Take(BulkColorChains).ToList();
                    string[] seqs = part.Select(c => c.Seq).ToArray();
                    SetStatus("Chain 색 변경 중: " + changed.ToString("N0") + " / " + chains.Count.ToString("N0"));
                    await Task.Run(() => service.UpdateChainColorBulk(factory, device, seqs, argb, user));
                    foreach (SavedChain chain in part) chain.Color = color;
                    changed += part.Count;
                }
                done = "Chain 색 변경 완료: " + changed.ToString("N0") + "개";
            }
            catch (Exception ex)
            {
                ShowError("Chain 색 변경 (" + changed.ToString("N0") + "개까지 바뀜)", ex);
            }
            finally
            {
                SetChainDbBusy(false);
            }
            dgvSaved.Invalidate();
            ShowSavedChainsOnMap(false);
            if (done != null) SetStatus(done);
            return changed;
        }

        private async void btnSavedDelete_Click(object sender, EventArgs e)
        {
            await DeleteSavedChainsAsync(SelectedSavedChains(), "선택한");
        }

        private async void btnSavedDeleteShown_Click(object sender, EventArgs e)
        {
            await DeleteSavedChainsAsync(_savedView.ToList(), txtSavedFind.Text.Trim().Length > 0 ? "목록에 보이는(찾기 '" + txtSavedFind.Text.Trim() + "')" : "목록의 모든");
        }

        /// <summary>
        /// Chain 을 실제로 삭제한다(하위 Layer / Member 포함, 되돌릴 수 없음). 기존 GDS Map View 에서 만든 Chain 도 목록에 있으면 지워진다.
        /// 기본 버튼은 [아니오]다.
        /// </summary>
        private async Task DeleteSavedChainsAsync(List<SavedChain> chains, string what)
        {
            if (!_chainDbEnabled || _shownMap == null || _chainDbBusy || chains.Count == 0) return;
            string sample = string.Join(", ", chains.Take(5).Select(c => c.Name));
            string text = what + " Chain " + chains.Count.ToString("N0") + "개를 DB 에서 삭제합니다.\r\n\r\n"
                + sample + (chains.Count > 5 ? " ..." : "") + "\r\n\r\n"
                + "기존 GDS Map View 에서 만든 Chain 도 이 목록에 있으면 함께 삭제되며 되돌릴 수 없습니다.\r\n\r\n삭제하시겠습니까?";
            if (MessageBox.Show(this, text, "Chain 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            await DeleteChainsAsync(chains);
        }

        /// <summary>Chain 을 1,000개씩 삭제하고 목록을 다시 읽는다. 반환: 지운 Chain 수. 확인 창은 부르는 쪽이 띄운다.</summary>
        private async Task<int> DeleteChainsAsync(List<SavedChain> chains)
        {
            string factory = _shownMap.Factory, device = _shownMap.DeviceId, user = Environment.UserName;
            int deleted = 0;
            SetChainDbBusy(true);
            try
            {
                var service = ChainRo();
                for (int start = 0; start < chains.Count; start += BulkDeleteChains)
                {
                    string[] seqs = chains.Skip(start).Take(BulkDeleteChains).Select(c => c.Seq).ToArray();
                    SetStatus("Chain 삭제 중: " + deleted.ToString("N0") + " / " + chains.Count.ToString("N0"));
                    deleted += await Task.Run(() => service.DeleteChainBulk(factory, device, seqs, user));
                }
                SetStatus("Chain 삭제 완료: " + deleted.ToString("N0") + "개");
            }
            catch (Exception ex)
            {
                ShowError("Chain 삭제", ex);
            }
            finally
            {
                SetChainDbBusy(false);
            }
            foreach (U8Result r in _results) r.Created = false;
            map.SetVisibleChainOverlays(null);
            await RefreshSavedChainsAsync(false);
            ShowResultPreview();
            return deleted;
        }

        #endregion
    }
}
