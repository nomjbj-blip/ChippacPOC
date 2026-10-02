using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 찾은 묶음을 목록에서 검토하고 체크한 결과만 현재 Chain의 수동 제외에 반영한다.
    /// 한 번의 확정에서 실제 바뀐 키만 보관하여 다른 수동 편집 기록을 지우지 않고 되돌린다.
    /// </summary>
    public partial class GdsMapForm
    {
        /// <summary>목록의 한 행이 가리키는 검색 결과와 보호 여부를 함께 유지한다.</summary>
        private sealed class ChainSimilarListItem
        {
            public int Number;
            public ChainSimilarGroup Group;
            public override string ToString()
            {
                string state = Group.IsProtected ? "단자 보호"
                    : Group.IsAmbiguous ? "검토 필요" : "일치";
                return Number.ToString("000") + " / " + state + " / "
                    + Group.ElementKeys.Length + " Element";
            }
        }

        private bool _updatingChainSimilarList;
        private bool _chainSimilarApplying;

        /// <summary>Designer의 제외 확정 버튼을 현재 Chain 검증 절차에 연결한다.</summary>
        private void BtnChainApplySimilar_Click(object sender, EventArgs e)
        {
            ApplyCheckedChainSimilarGroups();
        }

        /// <summary>Designer의 되돌리기 버튼을 직전 배치 편집 기록에 연결한다.</summary>
        private void BtnChainUndoSimilar_Click(object sender, EventArgs e)
        {
            UndoLastChainSimilarRemoval();
        }

        /// <summary>높은 일치는 기본 체크하고 단자 보호/모호한 결과는 엔지니어가 검토하게 둔다.</summary>
        private void FillChainSimilarList(IEnumerable<ChainSimilarGroup> groups)
        {
            _updatingChainSimilarList = true;
            try
            {
                _chainSimilarList.Items.Clear();
                int number = 0;
                foreach (ChainSimilarGroup group in groups)
                {
                    var item = new ChainSimilarListItem { Number = ++number, Group = group };
                    _chainSimilarList.Items.Add(item, !group.IsProtected && !group.IsAmbiguous);
                }
            }
            finally { _updatingChainSimilarList = false; }
            UpdateChainSelectionButtons();
        }

        /// <summary>단자 보호 묶음의 체크를 막고 나머지 체크 변경은 확정 버튼에 반영한다.</summary>
        private void ChainSimilarList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingChainSimilarList || e.Index < 0) return;
            var item = (ChainSimilarListItem)_chainSimilarList.Items[e.Index];
            if (item.Group.IsProtected && e.NewValue == CheckState.Checked)
            {
                e.NewValue = CheckState.Unchecked;
                _lblChainSetupStatus.Text = "Input/Output이 포함된 묶음은 제외할 수 없습니다.";
            }
            if (IsHandleCreated)
                BeginInvoke(new Action(UpdateChainSelectionButtons));
        }

        /// <summary>결과 행을 고르면 해당 묶음을 강조하고 화면 중심으로 이동한다.</summary>
        private void ChainSimilarList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingChainSimilarList) return;
            if (!(_chainSimilarList.SelectedItem is ChainSimilarListItem item))
            {
                map.SetChainSimilarSelected(null);
                return;
            }
            IList<ChainTraceElement> elements = map.GetChainTraceElements(item.Group.ElementKeys);
            GBox bounds = GBox.Empty;
            foreach (ChainTraceElement element in elements)
                bounds.Include(element.Bounds);
            map.SetChainSimilarSelected(elements);
            map.FocusChainBounds(bounds);
            _lblChainSetupStatus.Text = item.Number + "번 묶음 / "
                + item.Group.ElementKeys.Length + " Element / "
                + (item.Group.IsProtected ? "단자 보호"
                    : item.Group.IsAmbiguous ? "검토 필요" : "일치");
        }

        /// <summary>현재 검색 결과에서 체크된 묶음만 모아 단자/Chain 소속을 다시 검증한다.</summary>
        private HashSet<string> GetCheckedChainSimilarKeys()
        {
            return new HashSet<string>(_chainSimilarList.CheckedItems
                .Cast<ChainSimilarListItem>()
                .SelectMany(item => item.Group.ElementKeys), StringComparer.Ordinal);
        }

        /// <summary>
        /// 제외 전후를 같은 Layer 규칙으로 검사한 뒤 새 단절이 있으면 경고한다.
        /// 검증 중에는 Chain을 수정하지 않고, 확인이 끝난 현재 Chain에만 수동 제외를 반영한다.
        /// </summary>
        private async void ApplyCheckedChainSimilarGroups()
        {
            if (!HasSelectedChain || !_activeChain.Visible || !_chainTraceResultShown
                || _chainSimilarSearching || _chainSimilarApplying || _chainSimilarGroups.Count == 0)
                return;
            HashSet<string> removeKeys = GetCheckedChainSimilarKeys();
            if (removeKeys.Count == 0)
            {
                _lblChainSetupStatus.Text = "제외할 묶음을 하나 이상 체크하세요.";
                return;
            }
            var endpoints = new HashSet<string>(_chainInputSelections.Keys
                .Concat(_chainOutputSelections.Keys), StringComparer.Ordinal);
            HashSet<string> beforeKeys = GetEditedChainKeys();
            if (removeKeys.Overlaps(endpoints) || !removeKeys.IsSubsetOf(beforeKeys))
            {
                _lblChainSetupStatus.Text = "단자 또는 현재 Chain 밖의 객체가 포함되어 다시 검색해야 합니다.";
                return;
            }
            ChainTraceElement[] beforeElements = map.GetChainTraceElements(beforeKeys).ToArray();
            if (beforeElements.Length != beforeKeys.Count
                || beforeElements.Any(element => !_activeChain.LayerIds.Contains(element.LayerId)))
            {
                _lblChainSetupStatus.Text = "현재 Layer에서 검사할 수 없는 경로가 있습니다. 후보 경로를 다시 찾으세요.";
                return;
            }
            ChainTraceElement[] afterElements = beforeElements
                .Where(element => !removeKeys.Contains(element.ElementKey)).ToArray();
            string[] inputKeys = _chainInputSelections.Keys.ToArray();
            string[] outputKeys = _chainOutputSelections.Keys.ToArray();
            ChainLayerConnectionRule[] rules = _chainLayerRules.ToArray();
            bool applyRules = _applyChainLayerRules;
            double cellSize = (double)_numChainCellSize.Value;
            ChainDefinition chain = _activeChain;
            int traceGeneration = _chainTraceGeneration;
            int similarGeneration = _chainSimilarGeneration;
            _chainSimilarApplying = true;
            UpdateChainSelectionButtons();
            _lblChainSetupStatus.Text = "체크한 묶음의 제외 전후 연결을 검사하는 중입니다.";
            try
            {
                Tuple<ChainConnectivityResult, ChainConnectivityResult> checks = await Task.Run(() =>
                {
                    var tracer = new ChainCandidateTracer();
                    var beforeRequest = new ChainTraceRequest(beforeElements, inputKeys,
                        outputKeys, rules, cellSize) { ApplyLayerRules = applyRules };
                    var afterRequest = new ChainTraceRequest(afterElements, inputKeys,
                        outputKeys, rules, cellSize) { ApplyLayerRules = applyRules };
                    return Tuple.Create(tracer.CheckSelectionConnectivity(beforeRequest),
                        tracer.CheckSelectionConnectivity(afterRequest));
                });
                if (traceGeneration != _chainTraceGeneration
                    || similarGeneration != _chainSimilarGeneration
                    || !ReferenceEquals(chain, _activeChain)) return;
                int newDisconnected = checks.Item2.DisconnectedKeys
                    .Except(checks.Item1.DisconnectedKeys, StringComparer.Ordinal).Count();
                bool outputLost = checks.Item2.ReachedOutputCount < checks.Item1.ReachedOutputCount;
                if (newDisconnected > 0 || outputLost)
                {
                    string message = "이번 제외로 새 단절 객체 " + newDisconnected
                        + "개 / Output 도달 " + checks.Item1.ReachedOutputCount
                        + " → " + checks.Item2.ReachedOutputCount
                        + "개가 됩니다. 그래도 제외하시겠습니까?";
                    if (MessageBox.Show(this, message, "Chain 연결 변경",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;
                }

                HashSet<string> automatic = GetAutomaticChainKeys();
                chain.LastSimilarUndo = ChainSimilarBatchEdit.Apply(removeKeys, automatic,
                    _chainManualAdded, _chainManualExcluded);
                ClearChainExampleSelection();
                _lblChainSetupStatus.Text = "현재 Chain에서 " + removeKeys.Count
                    + "개 Element 제외 / 연결 검사: 새 단절 " + newDisconnected
                    + "개 / 되돌리기 가능";
                RefreshChainDisplay();
            }
            catch (Exception ex)
            {
                if (ReferenceEquals(chain, _activeChain))
                    _lblChainSetupStatus.Text = "일괄 제외 검사 실패: " + ex.Message;
            }
            finally
            {
                _chainSimilarApplying = false;
                UpdateChainSelectionButtons();
            }
        }

        /// <summary>직전 일괄 제외에서 바뀐 Key만 이전 수동 추가/제외 상태로 복원한다.</summary>
        private void UndoLastChainSimilarRemoval()
        {
            if (!HasSelectedChain || _chainSimilarApplying || _activeChain.LastSimilarUndo == null)
                return;
            ChainSimilarBatchChange undo = _activeChain.LastSimilarUndo;
            undo.Restore(_chainManualAdded, _chainManualExcluded);
            _activeChain.LastSimilarUndo = null;
            _lblChainSetupStatus.Text = "직전 일괄 제외 " + undo.ChangedCount
                + "개 Element를 되돌렸습니다.";
            RefreshChainDisplay();
            UpdateChainSelectionButtons();
        }
    }
}
