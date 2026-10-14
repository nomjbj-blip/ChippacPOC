using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS 테스트 화면에 Chain Input, Output, Layer 필터와 경로 편집 기능을 제공한다.
    /// 후보 경로 검증이 끝나기 전까지 DB 저장 기능과 확정 Chain 관리는 포함하지 않는다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        private enum ChainPointCaptureMode
        {
            None,
            Input,
            Output
        }

        /// <summary>지도 클릭으로 자동 후보의 포함 여부를 보정하는 모드다.</summary>
        private enum ChainEditMode { None, Add, Remove }

        /// <summary>지도에서 선택한 배치 Element와 역할 Marker를 그릴 클릭 좌표를 함께 보관한다.</summary>
        private sealed class ChainEndpointPick
        {
            public ChainElementCandidate Candidate { get; set; }
            public GPoint Point { get; set; }
        }

        private ChainPointCaptureMode _chainPointCaptureMode;
        private ChainEditMode _chainEditMode;
        private HashSet<string> _chainManualAdded = new HashSet<string>(StringComparer.Ordinal);
        private HashSet<string> _chainManualExcluded = new HashSet<string>(StringComparer.Ordinal);
        private Dictionary<string, ChainEndpointPick> _chainInputSelections =
            new Dictionary<string, ChainEndpointPick>(StringComparer.Ordinal);
        private Dictionary<string, ChainEndpointPick> _chainOutputSelections =
            new Dictionary<string, ChainEndpointPick>(StringComparer.Ordinal);
        private Dictionary<string, ChainEndpointPick> _chainDraftSelections;
        private ChainTraceResult _chainLastTraceResult;
        private Point _chainMouseDownPoint;
        private bool _chainTraceResultShown;
        private int _chainTraceGeneration;
        private List<ChainLayerConnectionRule> _chainLayerRules = new List<ChainLayerConnectionRule>();

        /// <summary>
        /// 화면에서 Layer 규칙 설정을 제거한 뒤에도 하위 추적 API와 Chain 상태 구조를 유지하기 위한 내부 값이다.
        /// 현재 화면에서는 사용자가 규칙을 켤 수 없으므로 항상 false로 초기화한다.
        /// </summary>
        private bool _applyChainLayerRules;

        /// <summary>Designer에서 이벤트가 연결된 Chain 설정 화면의 최초 상태를 준비한다.</summary>
        private void InitializeChainSetupState()
        {
            InitializeChainListState();
            SetChainButtonsEnabled(false);
            UpdateChainSelectionButtons();
        }

        /// <summary>Map에서 Chain Input으로 사용할 점 선택을 시작한다.</summary>
        private void BtnChainInput_Click(object sender, EventArgs e)
        {
            BeginChainPointCapture(ChainPointCaptureMode.Input);
        }

        /// <summary>Map에서 Chain Output으로 사용할 점 선택을 시작한다.</summary>
        private void BtnChainOutput_Click(object sender, EventArgs e)
        {
            BeginChainPointCapture(ChainPointCaptureMode.Output);
        }

        /// <summary>임시 Input/Output 선택 내용을 현재 Chain에 반영한다.</summary>
        private void BtnChainApply_Click(object sender, EventArgs e)
        {
            ApplyChainEndpointSelection();
        }

        /// <summary>진행 중인 Input/Output 선택을 취소한다.</summary>
        private void BtnChainCancel_Click(object sender, EventArgs e)
        {
            CancelChainEndpointSelection();
        }

        /// <summary>지도에서 후보 경로를 추가하는 편집 모드로 전환한다.</summary>
        private void BtnChainAdd_Click(object sender, EventArgs e)
        {
            ToggleChainEditMode(ChainEditMode.Add);
        }

        /// <summary>지도에서 후보 경로를 제외하는 편집 모드로 전환한다.</summary>
        private void BtnChainRemove_Click(object sender, EventArgs e)
        {
            ToggleChainEditMode(ChainEditMode.Remove);
        }

        /// <summary>유사 묶음 검색의 기준으로 사용할 예시 선택을 전환한다.</summary>
        private void BtnChainExample_Click(object sender, EventArgs e)
        {
            ToggleChainExampleSelection();
        }

        /// <summary>선택한 예시와 유사한 Chain 묶음을 검색한다.</summary>
        private void BtnChainFindSimilar_Click(object sender, EventArgs e)
        {
            FindChainSimilarGroups();
        }

        /// <summary>유사 묶음 미리보기만 지우고 예시 선택은 유지한다.</summary>
        private void BtnChainClearSimilar_Click(object sender, EventArgs e)
        {
            ClearChainSimilarPreview();
            SetChainStatus("유사 묶음 미리보기를 취소했습니다.");
        }

        /// <summary>Chain 편집과 클릭을 구분하기 위해 Map MouseDown 위치를 보관한다.</summary>
        private void Map_ChainMouseDown(object sender, MouseEventArgs e)
        {
            _chainMouseDownPoint = e.Location;
        }

        /// <summary>
        /// Input 또는 Output의 확정값을 임시 선택으로 복사하고 연속 클릭 모드를 시작한다.
        /// 적용 전까지 확정값은 변경하지 않아 Esc로 이전 값을 복원할 수 있다.
        /// </summary>
        private void BeginChainPointCapture(ChainPointCaptureMode mode)
        {
            if (map.Structure == null)
            {
                MessageBox.Show(this, "GDS 도면을 먼저 조회하세요.", "Chain Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!EnsureSelectedChainForEndpoint()) return;

            if (_chainPointCaptureMode != ChainPointCaptureMode.None || _chainEditMode != ChainEditMode.None)
                return;

            // 확정한 역할은 이 Chain에서 다시 지정하지 않는다. 다른 단자 쌍은 새 Chain에 만든다.
            var confirmed = mode == ChainPointCaptureMode.Input
                ? _chainInputSelections : _chainOutputSelections;
            if (confirmed.Count > 0)
            {
                ShowChainEndpointLimitWarning(mode == ChainPointCaptureMode.Input ? "Input" : "Output");
                return;
            }

            _chainPointCaptureMode = mode;
            _chainDraftSelections = new Dictionary<string, ChainEndpointPick>(confirmed, StringComparer.Ordinal);
            map.Mode = GdsMapControl.ViewMode.View;
            map.Cursor = Cursors.Cross;
            UpdateChainSelectionButtons();
            ShowChainEndpointSelection();
            SetChainStatus(GetCaptureGuide(mode));
        }

        /// <summary>Input/Output 작업은 목록에서 선택한 Chain에만 귀속되도록 마지막 진입 지점에서도 검사한다.</summary>
        private bool EnsureSelectedChainForEndpoint()
        {
            if (HasSelectedChain && _activeChain.Visible) return true;
            CaptureActiveChain();
            ClearActiveChainSelection();
            const string message = "Chain을 먼저 선택하세요. 목록이 비어 있으면 새 Chain을 만드세요.";
            SetChainStatus(message);
            MessageBox.Show(this, message, "Chain 선택 필요",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>현재 지정할 좌표의 종류에 맞는 화면 안내 문구를 반환한다.</summary>
        private static string GetCaptureGuide(ChainPointCaptureMode mode)
        {
            switch (mode)
            {
                case ChainPointCaptureMode.Input:
                    return "지도에서 Input 도형을 클릭한 뒤 [선택 적용]을 누르세요.";
                case ChainPointCaptureMode.Output:
                    return "지도에서 Output 도형을 클릭한 뒤 [선택 적용]을 누르세요.";
                default:
                    return string.Empty;
            }
        }

        /// <summary>역할별 두 번째 단자 또는 확정 단자 재지정을 막고 새 Chain 생성 방법을 안내한다.</summary>
        private void ShowChainEndpointLimitWarning(string role)
        {
            string message = "한 Chain에는 Input과 Output을 각각 한 개씩만 지정할 수 있습니다. "
                + role + "을 추가하거나 다시 지정하려면 새 Chain을 만드세요.";
            SetChainStatus(message);
            MessageBox.Show(this, message, "Chain 단자 지정 제한",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 지도 클릭으로 역할별 임시 Element를 전환한다. 화면 이동은 클릭으로 취급하지 않는다.
        /// </summary>
        private void Map_ChainMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left
                || (_chainPointCaptureMode == ChainPointCaptureMode.None
                    && _chainEditMode == ChainEditMode.None && !_chainExampleSelecting))
                return;

            if (!HasSelectedChain || !_activeChain.Visible)
            {
                CaptureActiveChain();
                ClearActiveChainSelection();
                return;
            }

            if (Math.Abs(e.X - _chainMouseDownPoint.X) > 4
                || Math.Abs(e.Y - _chainMouseDownPoint.Y) > 4)
                return;

            if (_chainExampleSelecting)
                SelectChainExampleElement(e.Location);
            else if (_chainEditMode != ChainEditMode.None)
                SelectChainEditElement(e.Location);
            else
                SelectChainEndpointElement(e.Location);
        }

        /// <summary>
        /// 후보가 하나면 바로 전환하고, 여러 개면 지도 옆 메뉴에서 마우스로 고르게 한다.
        /// 후보 그리드를 열지 않으며 메뉴에 올린 항목만 임시로 미리 강조한다.
        /// </summary>
        private void SelectChainEndpointElement(Point screenPoint)
        {
            IList<ChainElementCandidate> candidates = map.GetChainElementCandidates(screenPoint);
            if (candidates.Count == 0)
            {
                SetChainStatus("클릭한 위치에 고를 수 있는 도형이 없습니다. 켜져 있는 Layer 의 도형만 고를 수 있습니다.");
                return;
            }

            GPoint point = map.GetWorldPosition(screenPoint);
            if (candidates.Count == 1)
            {
                ToggleChainEndpointCandidate(candidates[0], point);
                return;
            }

            var menu = new ContextMenuStrip();
            foreach (ChainElementCandidate candidate in candidates)
            {
                ChainElementCandidate current = candidate;
                var item = new ToolStripMenuItem("L" + current.LayerId + " / DT" + current.DataType
                    + " / " + current.ElementType + " / " + current.ElementKey)
                {
                    Checked = _chainDraftSelections.ContainsKey(current.ElementKey),
                    ToolTipText = current.Bounds.ToString()
                };
                item.MouseEnter += (sender, args) => ShowChainEndpointSelection(current);
                item.Click += (sender, args) => ToggleChainEndpointCandidate(current, point);
                menu.Items.Add(item);
            }
            menu.Closed += (sender, args) =>
            {
                ShowChainEndpointSelection();
                // WinForms가 Closed 이후에도 메뉴 종료 처리를 계속하므로 다음 UI 메시지에서 삭제한다.
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(new Action(() => menu.Dispose()));
            };
            menu.Show(map, screenPoint);
        }

        /// <summary>한 Element의 임시 선택을 전환하고 역할별 Marker와 개수를 즉시 갱신한다.</summary>
        private void ToggleChainEndpointCandidate(ChainElementCandidate candidate, GPoint point)
        {
            var other = _chainPointCaptureMode == ChainPointCaptureMode.Input
                ? _chainOutputSelections : _chainInputSelections;
            if (other.ContainsKey(candidate.ElementKey))
            {
                SetChainStatus("이 도형은 이미 다른 단자로 지정되어 있습니다.");
                return;
            }

            if (!_chainDraftSelections.ContainsKey(candidate.ElementKey)
                && _chainDraftSelections.Count >= 1)
            {
                ShowChainEndpointLimitWarning(_chainPointCaptureMode == ChainPointCaptureMode.Input
                    ? "Input" : "Output");
                return;
            }

            if (!_chainDraftSelections.Remove(candidate.ElementKey))
                _chainDraftSelections.Add(candidate.ElementKey,
                    new ChainEndpointPick { Candidate = candidate, Point = point });
            ShowChainEndpointSelection();
            SetChainStatus(_chainDraftSelections.Count == 0
                ? GetCaptureGuide(_chainPointCaptureMode)
                : "도형을 골랐습니다. [선택 적용]을 누르세요.");
        }

        /// <summary>역할별 임시/확정 선택을 합쳐 지도에 표시하고 후보 메뉴의 Hover를 추가한다.</summary>
        private void ShowChainEndpointSelection(ChainElementCandidate hovered = null)
        {
            if (_chainPointCaptureMode == ChainPointCaptureMode.None)
            {
                RefreshChainDisplay();
                return;
            }
            var input = _chainPointCaptureMode == ChainPointCaptureMode.Input
                ? _chainDraftSelections : _chainInputSelections;
            var output = _chainPointCaptureMode == ChainPointCaptureMode.Output
                ? _chainDraftSelections : _chainOutputSelections;
            // IN / OUT 원형 표시는 쓰지 않는다. 다시 조회할 때 표시가 남는 문제가 있어 제거 (2026-10-08).
            // 지정한 Input / Output 도형은 아래 노란 선택 강조로 보인다.
            map.SetChainEndpointPoints(new GPoint[0], new GPoint[0]);
            map.HighlightChainElements(input.Keys.Concat(output.Keys).Concat(
                hovered == null ? Enumerable.Empty<string>() : new[] { hovered.ElementKey }));
        }

        /// <summary>임시 선택을 검증한 뒤 해당 역할의 확정 집합에만 반영한다.</summary>
        private void ApplyChainEndpointSelection()
        {
            if (_chainPointCaptureMode == ChainPointCaptureMode.None)
                return;
            if (!EnsureSelectedChainForEndpoint()) return;
            if (_chainDraftSelections.Count == 0)
            {
                SetChainStatus("지도에서 도형을 먼저 클릭하세요.");
                return;
            }
            if (_chainDraftSelections.Count != 1)
            {
                ShowChainEndpointLimitWarning(_chainPointCaptureMode == ChainPointCaptureMode.Input
                    ? "Input" : "Output");
                return;
            }
            var checkedLayers = new HashSet<int>();
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
                if (chkLayerItems.GetItemChecked(i))
                    checkedLayers.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerId);
            if (_chainDraftSelections.Values.Any(pick => !checkedLayers.Contains(pick.Candidate.LayerId)))
            {
                SetChainStatus("고른 도형의 Layer 가 꺼져 있습니다. Layer 를 켜거나 다시 고르세요.");
                return;
            }

            string role = _chainPointCaptureMode == ChainPointCaptureMode.Input ? "Input" : "Output";
            var confirmed = new Dictionary<string, ChainEndpointPick>(_chainDraftSelections, StringComparer.Ordinal);
            if (_chainPointCaptureMode == ChainPointCaptureMode.Input)
                _chainInputSelections = confirmed;
            else
                _chainOutputSelections = confirmed;
            if (HasSelectedChain) _activeChain.LastSimilarUndo = null;
            _chainPointCaptureMode = ChainPointCaptureMode.None;
            _chainDraftSelections = null;
            _chainTraceResultShown = false;
            _chainLastTraceResult = null;
            _chainTraceGeneration++;
            map.Cursor = Cursors.SizeAll;
            UpdateChainSelectionButtons();
            RefreshChainDisplay();
            string nameNote = role == "Input" ? ApplyInputLabelName() : string.Empty;
            SetChainStatus(role + " 지정 완료." + nameNote + " [후보 경로 찾기]를 누르세요.");
        }

        /// <summary>
        /// Input 확정 직후 Input 근처 Label로 Chain 이름을 정한다 (2026-10-08).
        /// 이름이 기본값(IsDefaultChainName)이거나 이전에 자동으로 붙인 이름일 때만 바꾸고, 사용자가 고친 이름은 그대로 둔다.
        /// 1순위 = Input과 연결된 도형 안의 Label, 2순위 = 근처에서 가장 가까운 Label, 없으면 이름을 바꾸지 않는다.
        /// 반환: 상태 줄에 덧붙일 한 문장(정한 이름 / 같은 이름의 Chain 여부 / Label 없음). 찾은 근거(연결 단계 등)는 보이지 않는다 (2026-10-13).
        /// </summary>
        private string ApplyInputLabelName()
        {
            if (!HasSelectedChain) return string.Empty;
            ChainDefinition chain = _activeChain;
            if (!chain.NameAuto && !IsDefaultChainName(chain)) return string.Empty;
            ChainEndpointLabelResult found;
            try
            {
                found = map.FindChainEndpointLabel(_chainInputSelections.Select(pair =>
                    new KeyValuePair<string, GPoint>(pair.Key, pair.Value.Point)));
            }
            catch (Exception ex)
            {
                // 이름 자동 지정은 보조 기능이다. 실패해도 Input 확정은 유지하고 화면에는 알리지 않는다.
                System.Diagnostics.Trace.TraceWarning("Chain 이름 자동 지정 실패: {0}", ex.Message);
                return string.Empty;
            }
            if (found == null || found.Text == null)
                return " 근처 Label 이 없어 이름은 그대로입니다.";
            chain.Name = found.Text;
            chain.NameAuto = true;
            _chainList.Invalidate();
            bool duplicate = _chains.Any(other => !ReferenceEquals(other, chain) && other.Name == chain.Name);
            return " 이름: " + found.Text + (duplicate ? "(같은 이름의 Chain 이 있습니다)" : "") + ".";
        }

        /// <summary>이번 마우스 편집을 버리고 이전 Input/Output 확정값과 경로 표시를 복원한다.</summary>
        private void CancelChainEndpointSelection()
        {
            if (_chainPointCaptureMode == ChainPointCaptureMode.None)
                return;
            _chainPointCaptureMode = ChainPointCaptureMode.None;
            _chainDraftSelections = null;
            map.Cursor = Cursors.SizeAll;
            UpdateChainSelectionButtons();
            RefreshChainDisplay();
            SetChainStatus("선택을 취소했습니다.");
        }

        /// <summary>Esc로 임시 선택을 취소하여 지도 작업을 마우스와 키보드로 종료할 수 있게 한다.</summary>
        private void GdsMapViewForm_ChainKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            if (_chainExampleSelecting)
            {
                ToggleChainExampleSelection();
                ClearChainExampleSelection();
                SetChainStatus("예시 묶음 지정을 취소했습니다.");
            }
            else if (_chainEditMode != ChainEditMode.None)
                ToggleChainEditMode(_chainEditMode);
            else if (_chainPointCaptureMode != ChainPointCaptureMode.None)
                CancelChainEndpointSelection();
            else
                return;
            e.Handled = true;
        }

        /// <summary>편집 중에는 역할 전환과 경로 찾기를 막고 적용/취소만 활성화한다.</summary>
        private void UpdateChainSelectionButtons()
        {
            bool editing = _chainPointCaptureMode != ChainPointCaptureMode.None
                || _chainEditMode != ChainEditMode.None || _chainExampleSelecting
                || _chainSimilarSearching || _chainSimilarApplying;
            bool loaded = map.Structure != null && HasSelectedChain && _activeChain.Visible
                && !_chainTraceInProgress;
            _btnChainInput.Enabled = loaded && !editing;
            _btnChainOutput.Enabled = loaded && !editing;
            _btnChainTrace.Enabled = loaded && !editing;
            _btnChainAdd.Enabled = loaded && _chainTraceResultShown
                && _chainPointCaptureMode == ChainPointCaptureMode.None
                && !_chainExampleSelecting
                && !_chainSimilarSearching && !_chainSimilarApplying
                && (_chainEditMode == ChainEditMode.None || _chainEditMode == ChainEditMode.Add);
            _btnChainRemove.Enabled = loaded && _chainTraceResultShown
                && _chainPointCaptureMode == ChainPointCaptureMode.None
                && !_chainExampleSelecting
                && !_chainSimilarSearching && !_chainSimilarApplying
                && (_chainEditMode == ChainEditMode.None || _chainEditMode == ChainEditMode.Remove);
            _btnChainExample.Enabled = loaded && _chainTraceResultShown
                && (_chainExampleSelecting || !editing);
            _btnChainExample.BackColor = _chainExampleSelecting
                ? Color.Plum : SystemColors.Control;
            _btnChainFindSimilar.Enabled = loaded && _chainTraceResultShown
                && !editing && _chainExampleKeys.Count > 0;
            _btnChainClearSimilar.Enabled = !_chainSimilarApplying
                && (_chainSimilarSearching || _chainSimilarGroups.Count > 0);
            _btnChainApplySimilar.Enabled = loaded && !editing && _chainSimilarGroups.Count > 0
                && _chainSimilarList.CheckedItems.Count > 0;
            _btnChainUndoSimilar.Enabled = loaded && !editing
                && _activeChain.LastSimilarUndo != null;
            _chainSimilarList.Enabled = !_chainSimilarApplying && !_chainSimilarSearching
                && _chainSimilarGroups.Count > 0;
            _btnChainAdd.BackColor = _chainEditMode == ChainEditMode.Add ? Color.LightGoldenrodYellow : SystemColors.Control;
            _btnChainRemove.BackColor = _chainEditMode == ChainEditMode.Remove ? Color.LightGoldenrodYellow : SystemColors.Control;
            _btnChainApply.Enabled = loaded && _chainPointCaptureMode != ChainPointCaptureMode.None;
            _btnChainCancel.Enabled = loaded && _chainPointCaptureMode != ChainPointCaptureMode.None;
            SetChainListButtonsEnabled(map.Structure != null);
            UpdateViewLayout(); // Chain 작업 줄 / 유사 묶음 목록 표시 (2026-10-12)
        }

        /// <summary>확정된 Input/Output과 현재 후보 경로를 지도에 다시 표시한다.</summary>
        private void RestoreChainEndpointHighlight()
        {
            RefreshChainDisplay();
        }

        /// <summary>
        /// Input과 왼쪽 Layer 체크 상태를 검증한 뒤 실제 형상 접촉 경로를 계산한다.
        /// Output이 없으면 Input에서 더 이상 연결되지 않는 곳까지 후보를 찾는다.
        /// 대량 Element 탐색 중 화면이 멈추지 않도록 계산은 백그라운드에서 실행한다.
        /// </summary>
        private async void BtnChainTrace_Click(object sender, EventArgs e)
        {
            if (!TryCreateChainTraceInput(
                out HashSet<int> selectedLayerIds,
                out List<ChainLayerConnectionRule> rules,
                out string validationMessage))
            {
                MessageBox.Show(this, validationMessage, "Chain Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _chainTraceInProgress = true;
            if (HasSelectedChain) _activeChain.LastSimilarUndo = null;
            _btnChainTrace.Enabled = false;
            _btnChainInput.Enabled = false;
            _btnChainOutput.Enabled = false;
            SetChainListButtonsEnabled(false);
            int traceGeneration = _chainTraceGeneration;
            SetChainStatus("후보 경로를 찾는 중입니다.");
            progressLoad.Style = ProgressBarStyle.Marquee;
            progressLoad.Visible = true;
            try
            {
                string[] inputElementKeys = _chainInputSelections.Keys.ToArray();
                string[] outputElementKeys = _chainOutputSelections.Keys.ToArray();
                bool applyLayerRules = _applyChainLayerRules;
                double cellSize = (double)_numChainCellSize.Value;
                ChainTraceResult result = await Task.Run(() => map.TraceChainCandidate(
                    inputElementKeys,
                    outputElementKeys,
                    selectedLayerIds,
                    rules,
                    applyLayerRules,
                    cellSize));

                if (traceGeneration != _chainTraceGeneration)
                    return;
                _chainTraceResultShown = true;
                _chainLastTraceResult = result;
                _activeChain.LayerIds = new HashSet<int>(selectedLayerIds);
                ShowEditedChainCandidate();
                SetChainStatus(FormatChainTraceResult(result) + FormatConnectivityWarning());
            }
            catch (Exception ex)
            {
                if (traceGeneration != _chainTraceGeneration)
                    return;
                SetChainStatus("후보 경로 찾기 실패");
                MessageBox.Show(this, ex.Message, "Chain Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _chainTraceInProgress = false;
                progressLoad.Visible = false;
                UpdateChainSelectionButtons();
            }
        }

        /// <summary>
        /// 후보 경로 찾기 결과를 한 문장으로 알린다.
        /// 겹침 / 분기 수, 경로 시작·끝 Element 키, 지정 단자 수 같은 확인용 계산 값은 보이지 않는다 (2026-10-13).
        /// 연결에 실패하면 지나간 Layer 를 알려 Layer 체크를 고칠 수 있게 한다.
        /// </summary>
        private static string FormatChainTraceResult(ChainTraceResult result)
        {
            if (result.IsOpenEnded)
                return "Input 에서 이어진 도형 " + result.VisitedElements.Count.ToString("N0")
                    + "개를 찾았습니다. 지도에서 확인하고 필요하면 [경로 추가] / [경로 제외]로 고치세요.";
            if (result.IsConnected)
                return "후보 경로 도형 " + result.Path.Count.ToString("N0")
                    + "개를 찾았습니다. 지도에서 확인하고 필요하면 [경로 추가] / [경로 제외]로 고치세요.";

            string visitedLayers = string.Join(", ", result.VisitedElements
                .Select(element => element.LayerId)
                .Distinct()
                .OrderBy(layerId => layerId)
                .Select(layerId => "Layer " + layerId));
            return "Output 까지 이어진 경로를 찾지 못했습니다. Layer 체크를 확인하세요."
                + (string.IsNullOrEmpty(visitedLayers) ? "" : " (지나간 Layer: " + visitedLayers + ")");
        }

        /// <summary>왼쪽 Layer 체크 상태와 선택적 Layer 규칙을 한 번의 추적 요청 값으로 복사한다.</summary>
        private bool TryCreateChainTraceInput(
            out HashSet<int> selectedLayerIds,
            out List<ChainLayerConnectionRule> rules,
            out string message)
        {
            selectedLayerIds = new HashSet<int>();
            rules = new List<ChainLayerConnectionRule>();
            message = string.Empty;

            if (map.Structure == null)
            {
                message = "GDS 도면을 먼저 조회하세요.";
                return false;
            }
            if (!HasSelectedChain || !_activeChain.Visible)
            {
                message = "Chain을 먼저 선택하세요. 목록이 비어 있으면 새 Chain을 만드세요.";
                return false;
            }
            if (_chainInputSelections.Count != 1 || _chainOutputSelections.Count > 1)
            {
                message = "Input Element는 한 개가 필요하며 Output은 한 개 이하로 지정할 수 있습니다.";
                return false;
            }
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
            {
                if (chkLayerItems.GetItemChecked(i))
                    selectedLayerIds.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerId);
            }
            if (selectedLayerIds.Count == 0)
            {
                message = "왼쪽에서 탐색할 Layer를 한 개 이상 체크하세요.";
                return false;
            }
            foreach (ChainEndpointPick pick in _chainInputSelections.Values.Concat(_chainOutputSelections.Values))
            {
                if (!selectedLayerIds.Contains(pick.Candidate.LayerId))
                {
                    message = "Input과 Output의 Layer를 왼쪽에서 모두 체크하세요.";
                    return false;
                }
            }
            if (_applyChainLayerRules && _chainLayerRules.Count == 0)
            {
                message = "Layer 규칙 적용을 사용하려면 규칙을 한 개 이상 추가하세요.";
                return false;
            }

            rules = _chainLayerRules.ToList();
            return true;
        }

        /// <summary>
        /// Layer 체크가 바뀌면 이전 탐색 결과를 해제한다.
        /// 확정된 Input/Output은 유지하여 엔지니어가 새 조건으로 다시 탐색할 수 있게 한다.
        /// </summary>
        private void InvalidateChainTraceResult()
        {
            ClearChainExampleSelection();
            if (HasSelectedChain) _activeChain.LastSimilarUndo = null;
            _chainTraceGeneration++;
            if (!_chainTraceResultShown)
                return;
            _chainTraceResultShown = false;
            _chainLastTraceResult = null;
            _chainEditMode = ChainEditMode.None;
            map.Cursor = Cursors.SizeAll;
            RestoreChainEndpointHighlight();
            UpdateChainSelectionButtons();
            SetChainStatus("Layer 체크가 바뀌었습니다. [후보 경로 찾기]를 다시 누르세요.");
        }

        /// <summary>GDS 조회 상태에 따라 Chain 설정 버튼의 사용 가능 상태를 일괄 변경한다.</summary>
        private void SetChainButtonsEnabled(bool enabled)
        {
            _btnChainInput.Enabled = enabled;
            _btnChainOutput.Enabled = enabled;
            _btnChainTrace.Enabled = enabled;
            _btnChainAdd.Enabled = enabled && _chainTraceResultShown;
            _btnChainRemove.Enabled = enabled && _chainTraceResultShown;
            _btnChainExample.Enabled = enabled && _chainTraceResultShown;
            _btnChainFindSimilar.Enabled = enabled && _chainTraceResultShown
                && _chainExampleKeys.Count > 0;
            _btnChainClearSimilar.Enabled = false;
            _btnChainApplySimilar.Enabled = false;
            _btnChainUndoSimilar.Enabled = false;
            _chainSimilarList.Enabled = false;
            _btnChainApply.Enabled = false;
            _btnChainCancel.Enabled = false;
            UpdateViewLayout(); // Chain 작업 줄 / 유사 묶음 목록 표시 (2026-10-12)
        }

        /// <summary>새 GDS를 조회한 뒤 이전 도면의 Chain 좌표를 제거하고 설정을 시작할 수 있게 한다.</summary>
        private void ResetChainSetupAfterMapLoad(bool loadSucceeded)
        {
            ClearChainExampleSelection();
            _chainPointCaptureMode = ChainPointCaptureMode.None;
            _chainEditMode = ChainEditMode.None;
            _chainDraftSelections = null;
            _chainInputSelections.Clear();
            _chainOutputSelections.Clear();
            _chainLastTraceResult = null;
            _chainTraceResultShown = false;
            _chainManualAdded.Clear();
            _chainManualExcluded.Clear();
            _chainTraceGeneration++;
            _chainLayerRules.Clear();
            _applyChainLayerRules = false;
            map.Cursor = Cursors.SizeAll;
            map.ResetChainSetupOverlay();
            SetChainButtonsEnabled(loadSucceeded);
            // 조회 직후 상태 줄은 조회 결과 한 줄만 보인다. Chain 안내는 Chain 을 고를 때 보인다 (2026-10-13).
            ResetChainWorkspace(loadSucceeded);
        }
    }
}
