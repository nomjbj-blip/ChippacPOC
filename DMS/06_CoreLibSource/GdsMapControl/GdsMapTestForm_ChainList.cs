using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 한 GDS 화면의 여러 Chain 초안을 메모리에서 관리한다.
    /// 목록 선택은 편집 대상을, 체크는 지도 표시를 결정하며 서로 영향을 주지 않는다.
    /// </summary>
    public partial class GdsMapForm
    {
        /// <summary>한 Chain의 편집 상태를 다른 Chain과 섞이지 않게 보관하는 현재 세션의 초안이다.</summary>
        private sealed class ChainDefinition
        {
            public Guid Id;
            public string Code;
            public string Name;
            public Color Color;
            public bool Visible = true;
            public Dictionary<string, ChainEndpointPick> Inputs =
                new Dictionary<string, ChainEndpointPick>(StringComparer.Ordinal);
            public Dictionary<string, ChainEndpointPick> Outputs =
                new Dictionary<string, ChainEndpointPick>(StringComparer.Ordinal);
            public HashSet<string> ManualAdded = new HashSet<string>(StringComparer.Ordinal);
            public HashSet<string> ManualExcluded = new HashSet<string>(StringComparer.Ordinal);
            public ChainSimilarBatchChange LastSimilarUndo;
            public HashSet<int> LayerIds = new HashSet<int>();
            public List<ChainLayerConnectionRule> LayerRules = new List<ChainLayerConnectionRule>();
            public bool ApplyLayerRules;
            public decimal CellSize = 10m;
            public ChainTraceResult LastResult;
            public bool TraceShown;
            public string Status = "Input / Output을 지정하세요.";

            public override string ToString()
            {
                int count = LastResult == null ? Inputs.Count + Outputs.Count
                    : LastResult.CandidateElements.Count + ManualAdded.Count - ManualExcluded.Count;
                return Code + " / " + Name + " / " + Math.Max(0, count)
                    + "개 / " + ColorTranslator.ToHtml(Color);
            }
        }

        private readonly List<ChainDefinition> _chains = new List<ChainDefinition>();
        private ChainDefinition _activeChain;
        private bool _switchingChain;
        private bool _chainTraceInProgress;
        private bool _chainVisibilityClick;
        private bool _showingHiddenChainStatus;
        private int _nextChainOrdinal;

        /// <summary>목록에서 선택한 행이 실제 편집 대상 Chain인지 확인해 빈 목록의 잔여 상태를 차단한다.</summary>
        private bool HasSelectedChain => _activeChain != null && _chains.Contains(_activeChain)
            && ReferenceEquals(_chainList.SelectedItem, _activeChain);

        /// <summary>Designer에서 이벤트가 연결된 Chain 목록의 최초 사용 상태를 준비한다.</summary>
        private void InitializeChainListState()
        {
            SetChainListButtonsEnabled(false);
        }

        /// <summary>새 Chain 초안을 만들고 편집 대상으로 선택한다.</summary>
        private void BtnChainNew_Click(object sender, EventArgs e)
        {
            CreateChain();
        }

        /// <summary>현재 선택한 Chain 이름을 수정한다.</summary>
        private void BtnChainRename_Click(object sender, EventArgs e)
        {
            RenameActiveChain();
        }

        /// <summary>현재 선택한 Chain 초안을 삭제한다.</summary>
        private void BtnChainDelete_Click(object sender, EventArgs e)
        {
            DeleteActiveChain();
        }

        /// <summary>체크 영역 클릭과 행 선택을 구분하기 위해 MouseDown 위치를 판정한다.</summary>
        private void ChainList_MouseDown(object sender, MouseEventArgs e)
        {
            int index = _chainList.IndexFromPoint(e.Location);
            _chainVisibilityClick = index >= 0
                && e.X < _chainList.GetItemRectangle(index).Left + 20;
        }

        /// <summary>Chain 표시 체크 처리 후 MouseDown 판정 상태를 초기화한다.</summary>
        private void ChainList_MouseUp(object sender, MouseEventArgs e)
        {
            _chainVisibilityClick = false;
        }

        /// <summary>새 GDS가 로드되면 이전 화면의 SCENE 키와 Chain 초안을 함께 비운다.</summary>
        private void ResetChainWorkspace(bool loaded)
        {
            _switchingChain = true;
            try
            {
                _chains.Clear();
                _activeChain = null;
                _nextChainOrdinal = 0;
                _chainList.Items.Clear();
            }
            finally { _switchingChain = false; }
            ClearActiveChainSelection();
            _lblChainSetupStatus.Text = loaded
                ? "Chain이 없습니다. 새 Chain을 만드세요."
                : "GDS 조회 후 새 Chain을 만드세요.";
        }

        /// <summary>Chain 선택이 없어지면 임시 단자/편집 상태를 버리고 공용 지도에 남은 Marker를 지운다.</summary>
        private void ClearActiveChainSelection()
        {
            ClearChainExampleSelection();
            _switchingChain = true;
            try
            {
                _activeChain = null;
                _chainTraceGeneration++;
                _chainPointCaptureMode = ChainPointCaptureMode.None;
                _chainEditMode = ChainEditMode.None;
                _chainDraftSelections = null;
                _chainInputSelections.Clear();
                _chainOutputSelections.Clear();
                _chainManualAdded.Clear();
                _chainManualExcluded.Clear();
                _chainLastTraceResult = null;
                _chainTraceResultShown = false;
                _chainLayerRules.Clear();
                _applyChainLayerRules = false;
                _numChainCellSize.Value = 10m;
                _showingHiddenChainStatus = false;
            }
            finally { _switchingChain = false; }
            map.Cursor = Cursors.SizeAll;
            _lblChainSetupStatus.Text = "Chain이 없습니다. 새 Chain을 만들거나 목록에서 선택하세요.";
            RefreshChainDisplay();
            UpdateChainSelectionButtons();
        }

        /// <summary>현재 GDS에서 겹치지 않는 새 코드와 내부 ID를 만들고 빈 Chain을 활성화한다.</summary>
        private void CreateChain()
        {
            if (map.Structure == null || _chainPointCaptureMode != ChainPointCaptureMode.None
                || _chainEditMode != ChainEditMode.None || _chainList == null)
                return;
            CaptureActiveChain();
            _nextChainOrdinal++;
            string code = GetChainCode(_nextChainOrdinal);
            var palette = new[] { Color.Gold, Color.DeepSkyBlue, Color.LimeGreen,
                Color.OrangeRed, Color.Violet, Color.Cyan, Color.HotPink, Color.White };
            var chain = new ChainDefinition
            {
                Id = Guid.NewGuid(), Code = code, Name = "Chain " + code,
                Color = palette[(_nextChainOrdinal - 1) % palette.Length],
                LayerIds = GetCheckedChainLayers()
            };
            _chains.Add(chain);
            _switchingChain = true;
            try
            {
                _chainList.Items.Add(chain, true);
                _chainList.SelectedItem = chain;
            }
            finally { _switchingChain = false; }
            RestoreActiveChain(chain);
            _lblChainSetupStatus.Text = code + " 생성 / Input과 Output을 지정하세요.";
        }

        /// <summary>A~Z 다음에는 AA, AB 순서로 사람이 읽기 쉬운 Chain Code를 만든다.</summary>
        private static string GetChainCode(int ordinal)
        {
            string code = string.Empty;
            while (ordinal > 0)
            {
                ordinal--;
                code = (char)('A' + ordinal % 26) + code;
                ordinal /= 26;
            }
            return code;
        }

        /// <summary>행 선택으로 이전 Chain을 저장하고 새 Chain의 편집 상태를 복원한다.</summary>
        private void ChainList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_switchingChain || _chainVisibilityClick) return;
            if (!ReferenceEquals(_chainList.SelectedItem, _activeChain))
                ClearChainExampleSelection();
            if (_chainList.SelectedItem == null)
            {
                CaptureActiveChain();
                ClearActiveChainSelection();
                return;
            }
            if (_chainPointCaptureMode != ChainPointCaptureMode.None
                || _chainEditMode != ChainEditMode.None)
                return;
            var next = (ChainDefinition)_chainList.SelectedItem;
            if (ReferenceEquals(next, _activeChain)) return;
            CaptureActiveChain();
            RestoreActiveChain(next);
        }

        /// <summary>체크 변경은 Chain의 지도 표시만 바꾸고 활성 편집 대상은 바꾸지 않는다.</summary>
        private void ChainList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_switchingChain || e.Index < 0 || e.Index >= _chainList.Items.Count)
                return;
            if (ReferenceEquals(_chainList.Items[e.Index], _activeChain)
                && e.NewValue != CheckState.Checked)
                ClearChainExampleSelection();
            CaptureActiveChain();
            ((ChainDefinition)_chainList.Items[e.Index]).Visible = e.NewValue == CheckState.Checked;
            BeginInvoke(new Action(() =>
            {
                if (!IsDisposed)
                {
                    if (!ReferenceEquals(_chainList.SelectedItem, _activeChain))
                    {
                        _switchingChain = true;
                        try { _chainList.SelectedItem = _activeChain; }
                        finally { _switchingChain = false; }
                    }
                    RefreshChainDisplay();
                    UpdateChainSelectionButtons();
                }
            }));
        }

        /// <summary>현재 Form 필드를 활성 Chain 초안에 복사해 전환 뒤에도 편집 내용이 남게 한다.</summary>
        private void CaptureActiveChain()
        {
            if (_activeChain == null || _switchingChain) return;
            _activeChain.Inputs = new Dictionary<string, ChainEndpointPick>(_chainInputSelections, StringComparer.Ordinal);
            _activeChain.Outputs = new Dictionary<string, ChainEndpointPick>(_chainOutputSelections, StringComparer.Ordinal);
            _activeChain.ManualAdded = new HashSet<string>(_chainManualAdded, StringComparer.Ordinal);
            _activeChain.ManualExcluded = new HashSet<string>(_chainManualExcluded, StringComparer.Ordinal);
            // Layer 체크는 공통 화면 표시다. Chain의 탐색 Layer는 후보 경로를 다시 찾을 때만 갱신한다.
            _activeChain.LayerRules = _chainLayerRules.ToList();
            _activeChain.ApplyLayerRules = _applyChainLayerRules;
            _activeChain.CellSize = _numChainCellSize.Value;
            _activeChain.LastResult = _chainLastTraceResult;
            _activeChain.TraceShown = _chainTraceResultShown;
            if (_activeChain.Visible && !_showingHiddenChainStatus)
                _activeChain.Status = _lblChainSetupStatus.Text;
            _chainList.Invalidate();
        }

        /// <summary>선택한 Chain의 편집값만 Form에 적용한다. 공통 Layer 표시와 다른 Chain 경로는 유지한다.</summary>
        private void RestoreActiveChain(ChainDefinition chain)
        {
            ClearChainExampleSelection();
            _switchingChain = true;
            try
            {
                _activeChain = chain;
                _chainTraceGeneration++;
                _chainPointCaptureMode = ChainPointCaptureMode.None;
                _chainEditMode = ChainEditMode.None;
                _chainDraftSelections = null;
                _chainInputSelections = new Dictionary<string, ChainEndpointPick>(chain.Inputs, StringComparer.Ordinal);
                _chainOutputSelections = new Dictionary<string, ChainEndpointPick>(chain.Outputs, StringComparer.Ordinal);
                _chainManualAdded = new HashSet<string>(chain.ManualAdded, StringComparer.Ordinal);
                _chainManualExcluded = new HashSet<string>(chain.ManualExcluded, StringComparer.Ordinal);
                _chainLayerRules = chain.LayerRules.ToList();
                _chainLastTraceResult = chain.LastResult;
                _chainTraceResultShown = chain.TraceShown;
                _applyChainLayerRules = chain.ApplyLayerRules;
                _numChainCellSize.Value = chain.CellSize;
                _lblChainSetupStatus.Text = chain.Status;
                _showingHiddenChainStatus = false;
                map.Cursor = Cursors.SizeAll;
            }
            finally
            {
                _switchingChain = false;
            }
            RefreshChainDisplay();
            UpdateChainSelectionButtons();
            SetChainListButtonsEnabled(true);
        }

        /// <summary>왼쪽에서 체크한 Layer를 활성 Chain의 탐색 필터 값으로 읽는다.</summary>
        private HashSet<int> GetCheckedChainLayers()
        {
            var layers = new HashSet<int>();
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
                if (chkLayerItems.GetItemChecked(i))
                    layers.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerId);
            return layers;
        }

        /// <summary>체크된 모든 Chain을 같은 규칙의 색 외곽선으로 그리고 활성 Chain의 역할 Marker만 표시한다.</summary>
        private void RefreshChainDisplay()
        {
            if (map.Structure == null) return;
            if (_activeChain == null)
            {
                map.HighlightChainElements(new string[0]);
                map.SetChainEndpointPoints(new GPoint[0], new GPoint[0]);
            }
            else
            {
                if (_activeChain.Visible && _showingHiddenChainStatus)
                {
                    _lblChainSetupStatus.Text = _activeChain.Status;
                    _showingHiddenChainStatus = false;
                }
                CaptureActiveChain();
                // 기존 노란 GPU 선택과 후보 겹침 외곽선을 지워 활성/비활성 표시 차이를 없앤다.
                map.HighlightChainElements(new string[0]);
                if (_activeChain.Visible)
                    map.SetChainEndpointPoints(_chainInputSelections.Values.Select(pick => pick.Point),
                        _chainOutputSelections.Values.Select(pick => pick.Point));
                else
                {
                    map.SetChainEndpointPoints(new GPoint[0], new GPoint[0]);
                    _lblChainSetupStatus.Text = _activeChain.Code + " / 표시 꺼짐 / 체크하면 편집할 수 있습니다.";
                    _showingHiddenChainStatus = true;
                }
            }
            var overlays = new List<ChainVisibleOverlay>();
            foreach (ChainDefinition chain in _chains.Where(item => item.Visible))
            {
                HashSet<string> keys = GetChainKeys(chain);
                overlays.Add(new ChainVisibleOverlay(chain.Code, chain.Color,
                    map.GetChainTraceElements(keys)));
            }
            map.SetVisibleChainOverlays(overlays);
            _chainList.Invalidate();
        }

        /// <summary>저장된 자동 후보와 수동 보정을 합쳐 다른 Visible Chain의 표시 키를 만든다.</summary>
        private static HashSet<string> GetChainKeys(ChainDefinition chain)
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);
            if (chain.LastResult != null && chain.TraceShown)
            {
                IEnumerable<ChainTraceElement> automatic = chain.LastResult.IsConnected
                    ? chain.LastResult.CandidateElements
                    : chain.LastResult.VisitedElements.Concat(chain.LastResult.EndpointElements);
                keys.UnionWith(automatic.Select(element => element.ElementKey));
            }
            keys.UnionWith(chain.ManualAdded);
            keys.ExceptWith(chain.ManualExcluded);
            keys.UnionWith(chain.Inputs.Keys);
            keys.UnionWith(chain.Outputs.Keys);
            return keys;
        }

        /// <summary>활성 Chain 이름만 바꾸며 다른 Chain의 Code와 설정은 유지한다.</summary>
        private void RenameActiveChain()
        {
            if (!HasSelectedChain) return;
            using (var dialog = new Form { Text = "Chain 이름 수정", Width = 350, Height = 145,
                StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog })
            {
                var input = new TextBox { Left = 12, Top = 12, Width = 310, Text = _activeChain.Name };
                var ok = new Button { Text = "적용", Left = 166, Top = 47, Width = 75,
                    DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "취소", Left = 247, Top = 47, Width = 75,
                    DialogResult = DialogResult.Cancel };
                dialog.Controls.Add(input);
                dialog.Controls.Add(ok);
                dialog.Controls.Add(cancel);
                dialog.AcceptButton = ok;
                dialog.CancelButton = cancel;
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                string name = input.Text.Trim();
                if (name.Length == 0) return;
                _activeChain.Name = name;
                _chainList.Invalidate();
                _lblChainSetupStatus.Text = _activeChain.Code + " / 이름을 변경했습니다.";
            }
        }

        /// <summary>현재 메모리 초안 하나만 삭제하고 공용 Element와 다른 Chain은 유지한다.</summary>
        private void DeleteActiveChain()
        {
            if (!HasSelectedChain) return;
            ChainDefinition removed = _activeChain;
            if (MessageBox.Show(this, removed.Code + " / " + removed.Name + "을 현재 세션에서 삭제할까요?",
                "Chain 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            int index = _chainList.SelectedIndex;
            _switchingChain = true;
            try
            {
                _chains.Remove(removed);
                _chainList.Items.Remove(removed);
                _activeChain = null;
                if (_chainList.Items.Count > 0)
                    _chainList.SelectedIndex = Math.Min(index, _chainList.Items.Count - 1);
            }
            finally { _switchingChain = false; }
            if (_chainList.SelectedItem is ChainDefinition next)
                RestoreActiveChain(next);
            else
                ClearActiveChainSelection();
        }

        /// <summary>지도 로드/선택 모드에 맞춰 Chain 목록 편집 명령을 활성화한다.</summary>
        private void SetChainListButtonsEnabled(bool loaded)
        {
            bool free = loaded && _chainPointCaptureMode == ChainPointCaptureMode.None
                && _chainEditMode == ChainEditMode.None && !_chainTraceInProgress
                && !_chainSimilarSearching && !_chainSimilarApplying;
            _chainList.Enabled = free;
            _btnChainNew.Enabled = free && !_chainExampleSelecting;
            _btnChainRename.Enabled = free && !_chainExampleSelecting && HasSelectedChain;
            _btnChainDelete.Enabled = free && !_chainExampleSelecting && HasSelectedChain;
        }
    }
}
