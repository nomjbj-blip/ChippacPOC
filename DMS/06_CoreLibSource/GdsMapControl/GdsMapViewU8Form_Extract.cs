using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View U8 같은 형태 찾기 / 찾기 결과 (2026-10-20, 설계 5절 2번, D5 / D6).
    ///  - 예시 Layer 의 도면 전체 덩어리(Union-Find) -> Label 붙이기 -> 예시와 비교(구성 + 상대 위치, 8 방향).
    ///  - 일치 = 형태 일치(회전·대칭 포함) + 번호 Label 1개. 나머지는 "검토"로 목록에만 보이고 Chain 을 만들지 않는다(D6).
    ///  - Chain 생성 대상 = 지금 목록(분류 / 찾기로 거르고 [목록에서 빼기]로 뺀 뒤 보이는 행) 중 일치 + DB 에 같은 Code 없음(D5).
    ///    E-Test Net 목록 붙여넣기 / 범위 선택은 뺐다 (2026-10-20).
    /// 도형 데이터는 [조회]로 받은 화면 도형을 그대로 쓴다(서버 다시 조회 없음). 백그라운드 + 진행률 + 중지.
    /// 검증: Probes/GdsChainAutoExtractProbe.ps1 (U8 예시 TSV_21063 -> 일치 27,341).
    /// </summary>
    public partial class GdsMapViewU8Form
    {
        /// <summary>찾기 결과 미리보기(흰 외곽선)로 그릴 최대 도형 수. 넘으면 목록에서 고른 덩어리만 그린다.</summary>
        private const int PreviewElementLimit = 50000;

        private readonly ChainNetNameRule _rule = new ChainNetNameRule();
        /// <summary>찾기 결과 전체([목록에서 빼기]로 뺀 행은 없음).</summary>
        private List<U8Result> _results = new List<U8Result>();
        /// <summary>지금 목록에 보이는 행(분류 / 찾기로 거른 결과). Chain 생성 대상은 이 안에서 고른다.</summary>
        private List<U8Result> _resultView = new List<U8Result>();
        /// <summary>이번 찾기 결과에서 [목록에서 빼기]로 뺀 행 수.</summary>
        private int _removedCount;
        /// <summary>찾기 결과를 만든 덩어리 계산기(INPUT 도형 찾기에 쓴다).</summary>
        private ChainComponentExtractor _resultExtractor;
        private bool _extractRunning;
        private CancellationTokenSource _extractCancel;
        private bool _updatingResults;

        private void InitializeExtractState()
        {
            cboResultClass.SelectedIndex = 0;
            dgvResult.RowCount = 0;
        }

        /// <summary>찾기 결과 한 행: 덩어리 + 비교 결과 + 이름 / Code + DB 상태.</summary>
        private sealed class U8Result
        {
            public ChainComponent Component;
            public ChainShapeMatch Match;
            public int NumberLabels;
            public string Code;
            public string Name;
            public bool InDb;
            public bool Created;

            /// <summary>형태 일치(회전·대칭 포함) + 번호 Label 1개.</summary>
            public bool IsMatch
            {
                get { return (Match == ChainShapeMatch.Match || Match == ChainShapeMatch.MatchTransformed) && NumberLabels == 1 && Code != null; }
            }

            public string ClassText
            {
                get
                {
                    if (IsMatch) return ChainShapeTemplate.Text(Match);
                    if (Match == ChainShapeMatch.Match || Match == ChainShapeMatch.MatchTransformed)
                        return NumberLabels == 0 ? "검토: Label 없음" : "검토: Label " + NumberLabels + "개";
                    return "검토: " + ChainShapeTemplate.Text(Match);
                }
            }
        }

        /// <summary>Chain 생성 대상 행: 일치 + DB 에 같은 Code 없음.</summary>
        private bool IsCreateTarget(U8Result r)
        {
            return r.IsMatch && !r.InDb;
        }

        /// <summary>지금 목록(거른 결과)의 생성 대상.</summary>
        private List<U8Result> CreateTargetsInView()
        {
            return _resultView.Where(IsCreateTarget).ToList();
        }

        private int CountCreateTargets()
        {
            return _resultView.Count(IsCreateTarget);
        }

        /// <summary>[같은 형태 찾기]: 예시 Layer 의 도면 전체 덩어리를 예시와 비교한다.</summary>
        private async void btnFind_Click(object sender, EventArgs e)
        {
            if (_template == null || _extractRunning) return;
            List<int> layers = _templateLayers;
            ChainShapeTemplate template = _template;
            ClearExtractResults();
            _extractCancel = new CancellationTokenSource();
            CancellationToken token = _extractCancel.Token;
            _extractRunning = true;
            ViewBusy.Start();
            UpdateU8Buttons();
            progressLoad.Value = 0;
            progressLoad.Style = ProgressBarStyle.Continuous;
            progressLoad.Visible = true;
            SetStatus("같은 형태 찾는 중: 덩어리 계산 준비");
            int generation = _elementGeneration;
            try
            {
                ChainComponentExtractor extractor = await GetExtractorAsync(layers);
                if (extractor == null) return;
                List<ChainComponentLabel> labels = NamingLabels(_rule);
                var progress = new Progress<int>(percent =>
                {
                    if (!_extractRunning) return;
                    progressLoad.Value = Math.Max(0, Math.Min(100, percent));
                    SetStatus("같은 형태 찾는 중: 덩어리 계산 " + percent + "%");
                });
                List<U8Result> results = await Task.Run(() => FindSameShapes(extractor, template, labels, progress, token));
                if (generation != _elementGeneration) return;
                _results = results;
                _removedCount = 0;
                _resultExtractor = extractor;
                ApplyDbState();
                ApplyResultFilter();
                tabRight.SelectedTab = tabResult;
                SetStatus("같은 형태 찾기 완료: " + SummaryLine());
            }
            catch (OperationCanceledException)
            {
                SetStatus("같은 형태 찾기를 중지했습니다.");
            }
            catch (Exception ex)
            {
                ShowError("같은 형태 찾기", ex);
            }
            finally
            {
                _extractRunning = false;
                _extractCancel.Dispose();
                _extractCancel = null;
                progressLoad.Visible = false;
                ViewBusy.Stop();
                UpdateU8Buttons();
            }
        }

        /// <summary>덩어리 계산 + Label + 비교(백그라운드). 결과는 덩어리 순서(아래 -> 위, 왼쪽 -> 오른쪽).</summary>
        private List<U8Result> FindSameShapes(ChainComponentExtractor extractor, ChainShapeTemplate template,
            List<ChainComponentLabel> labels, IProgress<int> progress, CancellationToken token)
        {
            IList<ChainComponent> components = extractor.ExtractAll(progress, token);
            token.ThrowIfCancellationRequested();
            extractor.AttachLabels(components, labels);
            var results = new List<U8Result>(components.Count);
            foreach (ChainComponent component in components)
            {
                int transform;
                results.Add(new U8Result
                {
                    Component = component,
                    Match = template.Compare(component.Elements, out transform),
                    NumberLabels = _rule.NumberLabelCount(component),
                    Code = _rule.CodeOf(component),
                    Name = _rule.NameOf(component),
                });
            }
            return results;
        }

        private void btnFindStop_Click(object sender, EventArgs e)
        {
            if (_extractCancel != null) _extractCancel.Cancel();
            btnFindStop.Enabled = false;
            SetStatus("중지 요청: 지금 계산 단계가 끝나면 멈춥니다.");
        }

        /// <summary>찾기 결과를 비운다(예시 / 도면 / 보기 기준이 바뀔 때).</summary>
        private void ClearExtractResults()
        {
            if (_extractCancel != null) _extractCancel.Cancel();
            _results = new List<U8Result>();
            _resultView = new List<U8Result>();
            _removedCount = 0;
            _resultExtractor = null;
            _updatingResults = true;
            try { dgvResult.RowCount = 0; }
            finally { _updatingResults = false; }
            if (map.Structure != null)
            {
                map.SetChainSimilarPreview(null, null, null);
                map.SetChainSimilarSelected(null);
            }
            lblResultSummary.Text = "예시를 지정한 뒤 [같은 형태 찾기]";
            UpdateU8Buttons();
        }

        /// <summary>DB Code 목록으로 결과의 "DB 있음"을 다시 정한다.</summary>
        private void ApplyDbState()
        {
            foreach (U8Result r in _results)
                r.InDb = r.Code != null && _dbCodes.Contains(r.Code);
        }

        /// <summary>요약 첫 줄: 덩어리 / 일치 / 검토 / 뺀 행.</summary>
        private string SummaryLine()
        {
            int match = _results.Count(r => r.IsMatch);
            int transformed = _results.Count(r => r.IsMatch && r.Match == ChainShapeMatch.MatchTransformed);
            return "덩어리 " + _results.Count.ToString("N0") + " / 일치 " + match.ToString("N0")
                + (transformed > 0 ? "(회전·대칭 " + transformed.ToString("N0") + ")" : "")
                + " / 검토 " + (_results.Count - match).ToString("N0")
                + (_removedCount > 0 ? " / 뺀 행 " + _removedCount.ToString("N0") : "");
        }

        /// <summary>요약 두 줄: 찾기 결과 전체 + 지금 목록의 생성 대상(검토 / DB 있음은 만들지 않음).</summary>
        private void UpdateResultSummary()
        {
            if (_results.Count == 0 && _removedCount == 0)
            {
                lblResultSummary.Text = "예시를 지정한 뒤 [같은 형태 찾기]";
                UpdateU8Buttons();
                return;
            }
            int targets = CountCreateTargets();
            int inDb = _resultView.Count(r => r.IsMatch && r.InDb);
            int review = _resultView.Count(r => !r.IsMatch);
            lblResultSummary.Text = SummaryLine() + "\r\n목록 " + _resultView.Count.ToString("N0") + "행 / 생성 대상 " + targets.ToString("N0")
                + (inDb > 0 ? " / DB 있음 " + inDb.ToString("N0") : "")
                + (review > 0 ? " / 검토 " + review.ToString("N0") + "(생성 안 함)" : "");
            UpdateU8Buttons();
        }

        /// <summary>
        /// 지금 목록의 생성 대상 덩어리를 흰 외곽선으로 미리 보인다. 너무 많으면(PreviewElementLimit) 그리지 않고 목록 선택만 보인다.
        /// </summary>
        private void ShowResultPreview()
        {
            if (map.Structure == null) return;
            List<U8Result> targets = CreateTargetsInView();
            int elements = targets.Sum(r => r.Component.Elements.Count);
            if (targets.Count == 0 || elements > PreviewElementLimit)
                map.SetChainSimilarPreview(null, null, null);
            else
                map.SetChainSimilarPreview(targets.SelectMany(r => r.Component.Elements), null, null);
        }

        #region [찾기 결과 목록]

        private void ResultFilter_Changed(object sender, EventArgs e)
        {
            ApplyResultFilter();
        }

        /// <summary>
        /// 분류(전체 / 일치 / 검토) / 찾기 글자(이름 / Code 포함)로 목록을 거른다. 2.7만 행이라 가상 목록에 행 수만 준다.
        /// 거른 결과가 Chain 생성 대상 범위이므로 미리보기 / 요약도 함께 다시 정한다.
        /// </summary>
        private void ApplyResultFilter()
        {
            string find = txtResultFind.Text.Trim();
            int klass = cboResultClass.SelectedIndex;
            IEnumerable<U8Result> query = _results;
            if (klass == 1) query = query.Where(r => r.IsMatch);
            else if (klass == 2) query = query.Where(r => !r.IsMatch);
            if (find.Length > 0)
                query = query.Where(r => (r.Name != null && r.Name.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (r.Code != null && r.Code.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0));
            _resultView = query.ToList();
            _updatingResults = true;
            try
            {
                dgvResult.RowCount = 0;
                dgvResult.RowCount = _resultView.Count;
                dgvResult.ClearSelection();
            }
            finally
            {
                _updatingResults = false;
            }
            dgvResult.Invalidate();
            if (map.Structure != null) map.SetChainSimilarSelected(null);
            ShowResultPreview();
            UpdateResultSummary();
        }

        private void dgvResult_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _resultView.Count) return;
            U8Result r = _resultView[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0: e.Value = r.Name ?? "(Label 없음)"; break;
                case 1: e.Value = r.ClassText; break;
                case 2: e.Value = r.Created ? "생성" : r.InDb ? "있음" : ""; break;
                case 3:
                    GBox b = r.Component.Bounds;
                    e.Value = "(" + (b.MinX + b.Width / 2).ToString("0.#", CultureInfo.InvariantCulture)
                        + ", " + (b.MinY + b.Height / 2).ToString("0.#", CultureInfo.InvariantCulture) + ")";
                    break;
            }
        }

        /// <summary>행을 고르면 그 덩어리를 Cyan 외곽선으로 보이고 그 위치로 이동한다(여러 행이면 현재 행).</summary>
        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            if (_updatingResults) return;
            UpdateU8Buttons();
            if (dgvResult.CurrentCell == null || dgvResult.SelectedRows.Count == 0) return;
            int index = dgvResult.CurrentCell.RowIndex;
            if (index < 0 || index >= _resultView.Count || map.Structure == null) return;
            U8Result r = _resultView[index];
            map.SetChainSimilarSelected(r.Component.Elements);
            map.FocusChainBounds(r.Component.Bounds);
            SetStatus((r.Name ?? "(Label 없음)") + (r.Code != null && r.Code != r.Name ? " / Code " + r.Code : "")
                + " / " + r.ClassText + " / 도형 " + r.Component.Elements.Count
                + (dgvResult.SelectedRows.Count > 1 ? " (선택 " + dgvResult.SelectedRows.Count.ToString("N0") + "행)" : ""));
        }

        /// <summary>Delete 키 = [목록에서 빼기].</summary>
        private void dgvResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || !btnResultRemove.Enabled) return;
            e.Handled = true;
            RemoveSelectedResults();
        }

        private void btnResultRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedResults();
        }

        /// <summary>
        /// [목록에서 빼기]: 고른 행을 찾기 결과에서 빼서 Chain 생성 대상에서 제외한다. DB 는 바꾸지 않는다.
        /// 다시 [같은 형태 찾기]를 하면 돌아온다. 분류 / 찾기 조건은 그대로 둔다.
        /// </summary>
        private void RemoveSelectedResults()
        {
            if (_extractRunning || _chainDbBusy) return;
            var removed = new HashSet<U8Result>(dgvResult.SelectedRows.Cast<DataGridViewRow>().Select(row => row.Index)
                .Where(i => i >= 0 && i < _resultView.Count).Select(i => _resultView[i]));
            if (removed.Count == 0) return;
            _results.RemoveAll(removed.Contains);
            _removedCount += removed.Count;
            ApplyResultFilter();
            SetStatus("목록에서 " + removed.Count.ToString("N0") + "행을 뺐습니다. (다시 [같은 형태 찾기]를 하면 돌아옵니다)");
        }

        #endregion
    }
}
