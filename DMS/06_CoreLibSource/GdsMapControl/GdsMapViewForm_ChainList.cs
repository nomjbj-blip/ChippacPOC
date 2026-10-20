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
    public partial class GdsMapViewForm
    {
        /// <summary>
        /// Chain 탐색 공간 격자 크기(도면 좌표, µm). 주변 도형 후보를 줄이는 색인 칸 크기라서 속도 / 메모리에만 영향을 주고
        /// 연결 판단(실제 형상 비교) 결과는 바꾸지 않는다. 화면 입력(_numChainCellSize)은 숨기고 이 값을 쓴다 (2026-10-06).
        /// 성능 문제로 조정이 필요하면 Designer에서 chainCellSizeLabel / _numChainCellSize의 Visible을 true로 바꿔 다시 꺼낸다.
        /// </summary>
        private const decimal DefaultChainCellSize = 10m;

        /// <summary>한 Chain의 편집 상태를 다른 Chain과 섞이지 않게 보관하는 현재 세션의 초안이다.</summary>
        private sealed class ChainDefinition : IColorListItem
        {
            /// <summary>Chain 목록 행 오른쪽 색 견본 (2026-10-13).</summary>
            Color IColorListItem.ListColor { get { return Color; } }

            public Guid Id;
            public string Code;
            public string Name;
            public Color Color;
            /// <summary>
            /// Input 근처 Label로 이름을 자동으로 붙여도 되는지 (2026-10-08). 새 Chain / 자동 이름이면 true,
            /// [이름 수정]으로 바꾸면 false. DB에서 불러온 Chain은 false지만 이름이 기본값(IsDefaultChainName)이면 자동 지정한다.
            /// </summary>
            public bool NameAuto;
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
            public decimal CellSize = DefaultChainCellSize;
            public ChainTraceResult LastResult;
            public bool TraceShown;
            // DB 저장(GdsMapForm_ChainDb) 상태: 저장된 Chain 번호(없으면 새 Chain)와 마지막 저장 내용 지문.
            public string ChainSeq;
            public string SavedFingerprint;
            // DB에서 불러온 Chain(3단계): Member 도형의 Layer(조회 시 필요한 Layer 계산용)와
            // 필요한 Layer를 조회하지 않아 도형 일부를 복원하지 못한 경우의 안내(저장 제외 사유). 완전하면 null.
            public HashSet<int> MemberLayerIds = new HashSet<int>();
            public string DbIncomplete;

            /// <summary>
            /// Chain 목록 문구: 이름 + 도형 수 + 필요할 때만 저장 상태. Code(A, B, C)는 DB 구분 키로만 쓰고 화면에는 보이지 않는다 (2026-10-12).
            /// 색은 행 오른쪽 색 견본으로 보이고 색 코드(#RRGGBB)는 쓰지 않는다. 저장된 Chain 은 상태를 붙이지 않는다 (2026-10-13).
            /// </summary>
            public override string ToString()
            {
                int count = LastResult == null ? Inputs.Count + Outputs.Count
                    : LastResult.CandidateElements.Count + ManualAdded.Count - ManualExcluded.Count;
                string state = ChainSeq == null ? "  (저장 안 됨)"
                    : DbIncomplete != null ? "  (Layer 미조회)"
                    : SavedFingerprint == null ? "  (저장 미완료)" : "";
                return Name + "  " + Math.Max(0, count).ToString("N0") + "개" + state;
            }
        }

        private readonly List<ChainDefinition> _chains = new List<ChainDefinition>();
        private ChainDefinition _activeChain;
        private bool _switchingChain;
        private bool _chainTraceInProgress;
        /// <summary>더블클릭으로 체크를 바꾸는 중인지. 이때만 목록 체크 변경을 받는다(2026-10-13).</summary>
        private bool _chainCheckByDoubleClick;
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

        /// <summary>Chain 목록 행을 더블클릭하면 지도 표시 체크를 바꾼다. 한 번 클릭은 행 선택(편집 대상)만 한다(2026-10-13).</summary>
        private void ChainList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ToggleCheckByDoubleClick(_chainList, e);
        }

        /// <summary>
        /// 더블클릭한 행의 체크를 뒤집는다. Chain 목록 / 유사 묶음 목록이 함께 쓴다.
        /// CheckedListBox 는 이미 선택된 행을 다시 클릭하거나 Space 키를 누르면 스스로 체크를 바꾸므로,
        /// 각 목록의 ItemCheck 는 이 함수가 바꿀 때(_chainCheckByDoubleClick)만 받고 나머지는 되돌린다.
        /// </summary>
        private void ToggleCheckByDoubleClick(CheckedListBox list, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int index = list.IndexFromPoint(e.Location);
            if (index < 0 || index >= list.Items.Count || !list.GetItemRectangle(index).Contains(e.Location)) return;
            _chainCheckByDoubleClick = true;
            try
            {
                list.SetItemChecked(index, !list.GetItemChecked(index));
            }
            finally
            {
                _chainCheckByDoubleClick = false;
            }
        }

        /// <summary>새 GDS가 로드되면 이전 화면의 SCENE 키와 Chain 초안을 함께 비운다.</summary>
        private void ResetChainWorkspace(bool loaded)
        {
            _switchingChain = true;
            try
            {
                _chains.Clear();
                _deletedDbChains.Clear();
                _activeChain = null;
                _nextChainOrdinal = 0;
                _chainList.Items.Clear();
            }
            finally { _switchingChain = false; }
            ClearActiveChainSelection();
        }

        /// <summary>
        /// 같은 Map을 다른 Layer 조합으로 다시 조회한 뒤 Chain 목록과 편집 내용을 그대로 두고 새 지도에 다시 연결한다.
        /// 도형 ID(PLACED_ELEMENT_ID)가 같으므로 Chain 정의는 유효하다. 진행 중이던 단자 지정 / 수동 편집 / 유사 묶음 미리보기만 취소한다.
        /// (2026-10-06, 설계 4.2절)
        /// </summary>
        private void ReattachChainsAfterMapReload()
        {
            ClearChainExampleSelection();
            _chainPointCaptureMode = ChainPointCaptureMode.None;
            _chainEditMode = ChainEditMode.None;
            _chainDraftSelections = null;
            _chainTraceGeneration++;
            map.Cursor = Cursors.SizeAll;
            map.ResetChainSetupOverlay();
            map.RebuildChainTraceItems();
            SetChainButtonsEnabled(true);
            ChainDefinition active = _activeChain != null && _chains.Contains(_activeChain) ? _activeChain : null;
            if (active != null)
            {
                _activeChain = null;
                RestoreActiveChain(active);
            }
            else
            {
                RefreshChainDisplay();
                UpdateChainSelectionButtons();
                SetChainListButtonsEnabled(true);
            }
            // 다시 조회한 결과는 조회 결과 한 줄로 보인다 (2026-10-13).
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
                _numChainCellSize.Value = DefaultChainCellSize;
                _showingHiddenChainStatus = false;
            }
            finally { _switchingChain = false; }
            map.Cursor = Cursors.SizeAll;
            SetChainStatus(ChainGuide(null));
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
            // 고정 팔레트를 순서대로 돌려 쓰면 DB에서 불러온 Chain 색과 겹칠 수 있다(2026-10-08 사용자 지적).
            // 화면의 모든 Chain 색과 가장 먼 색을 고르고, 지도 Layer 색과도 되도록 다르게 한다.
            var chain = new ChainDefinition
            {
                Id = Guid.NewGuid(), Code = code, Name = DefaultChainName(_nextChainOrdinal), NameAuto = true,
                Color = ChainColorPicker.Pick(_chains.Select(item => item.Color),
                    map.GetLayerDisplayItems().Select(layer => layer.Color)),
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
            SetChainStatus(chain.Name + " 을(를) 만들었습니다.");
        }

        /// <summary>
        /// 새 Chain 기본 이름: "새 Chain " + 순번. Code(A, B, C)는 화면에 보이지 않으므로 이름에도 넣지 않는다 (2026-10-12).
        /// 순번은 Code 순번과 같다(A = 1). Input 근처 Label을 찾으면 그 이름으로 바뀐다.
        /// </summary>
        private static string DefaultChainName(int ordinal)
        {
            return "새 Chain " + ordinal;
        }

        /// <summary>이름이 기본값인지(자동 이름 대상). 예전 기본값 "Chain " + Code 로 저장된 DB Chain도 기본값으로 본다.</summary>
        private static bool IsDefaultChainName(ChainDefinition chain)
        {
            return chain.Name == "Chain " + chain.Code || chain.Name == DefaultChainName(ParseChainCode(chain.Code));
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
            if (_switchingChain) return;
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

        /// <summary>체크 변경은 Chain의 지도 표시만 바꾸고 활성 편집 대상은 바꾸지 않는다. 체크는 더블클릭으로만 바뀐다(2026-10-13).</summary>
        private void ChainList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_switchingChain || e.Index < 0 || e.Index >= _chainList.Items.Count)
                return;
            if (!_chainCheckByDoubleClick)
            {
                e.NewValue = e.CurrentValue;
                return;
            }
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
            // Chain 을 바꿔 고르면 그 Chain 상태로 다음 할 일을 안내한다(Chain 별 문구 보관은 하지 않음, 2026-10-13).
            if (chain.Visible) SetChainStatus(ChainGuide(chain));
        }

        /// <summary>왼쪽에서 체크한 Layer를 활성 Chain의 탐색 필터 값으로 읽는다.</summary>
        private HashSet<int> GetCheckedChainLayers()
        {
            var layers = new HashSet<int>();
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
                if (chkLayerItems.GetItemChecked(i))
                    layers.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerKey);
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
                    _showingHiddenChainStatus = false;
                    SetChainStatus(ChainGuide(_activeChain));
                }
                CaptureActiveChain();
                // 기존 노란 GPU 선택과 후보 겹침 외곽선을 지워 활성/비활성 표시 차이를 없앤다.
                map.HighlightChainElements(new string[0]);
                // IN / OUT 원형 표시는 쓰지 않는다. 다시 조회할 때 표시가 남는 문제가 있어 제거 (2026-10-08).
                // 이전에 남아 있을 수 있는 표시를 지우기 위해 항상 빈 목록을 넘긴다.
                map.SetChainEndpointPoints(new GPoint[0], new GPoint[0]);
                if (!_activeChain.Visible)
                {
                    SetChainStatus(_activeChain.Name + " 은(는) 표시가 꺼져 있습니다.");
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
                // 직접 정한 이름은 Input을 다시 지정해도 자동 이름으로 덮어쓰지 않는다.
                _activeChain.NameAuto = false;
                _chainList.Invalidate();
                SetChainStatus("이름을 " + _activeChain.Name + " (으)로 바꿨습니다.");
            }
        }

        /// <summary>우클릭 메뉴를 연 행 번호. 메뉴 항목 클릭 시 이 행의 Chain 색을 바꾼다.</summary>
        private int _chainMenuIndex = -1;

        /// <summary>
        /// Chain 목록 우클릭 메뉴를 열기 전에 마우스 아래 행을 기억한다. 행이 없거나 다른 작업 중이면 메뉴를 열지 않는다.
        /// 우클릭은 행 선택 / 체크를 바꾸지 않으므로 편집 중인 Chain이 아닌 행의 색도 바꿀 수 있다.
        /// </summary>
        private void ChainListMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _chainMenuIndex = _chainList.IndexFromPoint(_chainList.PointToClient(Cursor.Position));
            if (!_chainList.Enabled || _chainMenuIndex < 0 || _chainMenuIndex >= _chainList.Items.Count)
                e.Cancel = true;
        }

        /// <summary>우클릭 메뉴 "색상 변경…": 그 행의 Chain 색을 바꾼다.</summary>
        private void ChainColorMenu_Click(object sender, EventArgs e)
        {
            if (_chainMenuIndex < 0 || _chainMenuIndex >= _chainList.Items.Count) return;
            ChangeChainColor((ChainDefinition)_chainList.Items[_chainMenuIndex]);
        }

        /// <summary>
        /// 색 선택 창으로 Chain 색을 바꾸고 지도 표시를 다시 그린다.
        /// 색은 DB 저장 지문(GdsMapViewForm_ChainDb)에 포함되므로 [Chain 저장(DB)] 시 변경 Chain으로 저장된다.
        /// 다른 Chain과 거의 같은 색이면 바꾸되 상태 줄에 알린다.
        /// </summary>
        private void ChangeChainColor(ChainDefinition chain)
        {
            if (chain == null || !_chainList.Enabled) return;
            using (var dialog = new ColorDialog { Color = chain.Color, FullOpen = true, AnyColor = true })
            {
                // ColorDialog 사용자 정의 색은 0x00BBGGRR 형식이다.
                dialog.CustomColors = ChainColorPicker.SuggestedColors
                    .Select(color => color.R | (color.G << 8) | (color.B << 16)).ToArray();
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                Color selected = Color.FromArgb(255, dialog.Color);
                if (selected.ToArgb() == chain.Color.ToArgb()) return;
                CaptureActiveChain();
                chain.Color = selected;
                ChainDefinition similar = _chains.FirstOrDefault(other => !ReferenceEquals(other, chain)
                    && ChainColorPicker.Difference(other.Color, selected) < 15);
                RefreshChainDisplay();
                SetChainStatus(chain.Name + " 의 색을 바꿨습니다."
                    + (similar == null ? "" : " " + similar.Name + " 와(과) 색이 비슷합니다."));
            }
        }

        /// <summary>현재 메모리 초안 하나만 삭제하고 공용 Element와 다른 Chain은 유지한다.</summary>
        private void DeleteActiveChain()
        {
            if (!HasSelectedChain) return;
            ChainDefinition removed = _activeChain;
            string question = removed.ChainSeq == null
                ? "'" + removed.Name + "' Chain을 현재 세션에서 삭제할까요?"
                : "'" + removed.Name + "' Chain을 삭제할까요?\r\n\r\nDB에 저장된 Chain입니다. 다음 [Chain 저장] 때 DB에서도 실제로 삭제됩니다.";
            if (MessageBox.Show(this, question, "Chain 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            // DB 삭제는 실제 삭제다. 저장 버튼을 누를 때 한 번에 반영한다.
            if (removed.ChainSeq != null)
                _deletedDbChains.Add(removed);
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
                && !_chainSimilarSearching && !_chainSimilarApplying && !_chainDbSaving;
            _chainList.Enabled = free;
            _btnChainNew.Enabled = free && !_chainExampleSelecting;
            _btnChainRename.Enabled = free && !_chainExampleSelecting && HasSelectedChain;
            _btnChainDelete.Enabled = free && !_chainExampleSelecting && HasSelectedChain;
            _btnChainDbSave.Enabled = free && !_chainExampleSelecting;
        }
    }
}
