using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 유사 묶음 검색에 사용할 한 곳의 예시 Element를 현재 Chain에서 클릭으로 고른다.
    /// 이 파일은 예시만 보관하며 Chain 소속이나 GDS 원본을 변경하지 않는다.
    /// </summary>
    public partial class GdsMapTestForm
    {
        private readonly HashSet<string> _chainExampleKeys = new HashSet<string>(StringComparer.Ordinal);
        private bool _chainExampleSelecting;
        private IList<ChainSimilarGroup> _chainSimilarGroups = new List<ChainSimilarGroup>();
        private CancellationTokenSource _chainSimilarCancellation;
        private int _chainSimilarGeneration;
        private bool _chainSimilarSearching;

        /// <summary>버튼을 다시 누르면 클릭 모드만 끝내고 선택한 예시는 다음 검색을 위해 남긴다.</summary>
        private void ToggleChainExampleSelection()
        {
            if (!_chainExampleSelecting && (map.Structure == null || !HasSelectedChain
                || !_activeChain.Visible || !_chainTraceResultShown || _chainTraceInProgress
                || _chainPointCaptureMode != ChainPointCaptureMode.None
                || _chainEditMode != ChainEditMode.None))
                return;

            _chainExampleSelecting = !_chainExampleSelecting;
            if (_chainExampleSelecting)
                map.Mode = GdsMapControl.ViewMode.View;
            map.Cursor = _chainExampleSelecting ? Cursors.Cross : Cursors.SizeAll;
            UpdateChainSelectionButtons();
            ShowChainExampleStatus();
        }

        /// <summary>클릭 지점에서 현재 Chain 소속이면서 체크된 Layer의 객체만 예시 후보로 보여 준다.</summary>
        private void SelectChainExampleElement(Point screenPoint)
        {
            HashSet<string> chainKeys = GetEditedChainKeys();
            HashSet<int> checkedLayers = GetCheckedChainLayers();
            List<ChainElementCandidate> candidates = map.GetChainElementCandidates(screenPoint)
                .Where(candidate => chainKeys.Contains(candidate.ElementKey)
                    && checkedLayers.Contains(candidate.LayerId)
                    && !_chainInputSelections.ContainsKey(candidate.ElementKey)
                    && !_chainOutputSelections.ContainsKey(candidate.ElementKey))
                .ToList();
            if (candidates.Count == 0)
            {
                _lblChainSetupStatus.Text = "이 위치에 선택 가능한 현재 Chain 객체가 없습니다. Input/Output은 예시에서 보호합니다.";
                return;
            }
            if (candidates.Count == 1)
            {
                ToggleChainExampleElement(candidates[0]);
                return;
            }

            ChainDefinition selectedChain = _activeChain;
            var menu = new ContextMenuStrip();
            foreach (ChainElementCandidate candidate in candidates)
            {
                ChainElementCandidate current = candidate;
                var item = new ToolStripMenuItem("L" + current.LayerId + " / DT" + current.DataType
                    + " / " + current.ElementType + " / " + current.ElementKey)
                {
                    Checked = _chainExampleKeys.Contains(current.ElementKey),
                    ToolTipText = current.Bounds.ToString()
                };
                item.Click += (sender, args) =>
                {
                    if (_chainExampleSelecting && ReferenceEquals(selectedChain, _activeChain))
                        ToggleChainExampleElement(current);
                };
                menu.Items.Add(item);
            }
            menu.Closed += (sender, args) =>
            {
                // WinForms의 메뉴 종료 이벤트가 끝난 다음 삭제하여 재클릭 시 예외를 막는다.
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(new Action(() => menu.Dispose()));
            };
            menu.Show(map, screenPoint);
        }

        /// <summary>선택한 객체를 예시에 넣거나 빼고 현재 예시만 OpenGL 외곽선으로 다시 표시한다.</summary>
        private void ToggleChainExampleElement(ChainElementCandidate candidate)
        {
            ClearChainSimilarPreview();
            if (!_chainExampleKeys.Add(candidate.ElementKey))
                _chainExampleKeys.Remove(candidate.ElementKey);
            map.SetChainExampleElements(map.GetChainTraceElements(_chainExampleKeys));
            UpdateChainSelectionButtons();
            ShowChainExampleStatus();
        }

        /// <summary>예시 지정 진행 상태를 알려 주되 현재 Chain의 경로 결과는 바꾸지 않는다.</summary>
        private void ShowChainExampleStatus()
        {
            _lblChainSetupStatus.Text = "예시 묶음 " + _chainExampleKeys.Count + "개 Element / "
                + (_chainExampleSelecting
                    ? "현재 Chain의 도형을 클릭하세요. 다시 클릭하면 해제됩니다."
                    : "지정 종료. 버튼을 누르면 이어서 수정할 수 있습니다.");
        }

        /// <summary>Chain/Layer/경로가 바뀌면 이전 예시 Key가 새 화면에 섞이지 않도록 초기화한다.</summary>
        private void ClearChainExampleSelection()
        {
            ClearChainSimilarPreview();
            if (!_chainExampleSelecting && _chainExampleKeys.Count == 0)
                return;
            _chainExampleSelecting = false;
            _chainExampleKeys.Clear();
            map.SetChainExampleElements(null);
            map.Cursor = Cursors.SizeAll;
            UpdateChainSelectionButtons();
        }

        /// <summary>
        /// 현재 Chain과 체크된 Layer의 도형만 복사해 백그라운드에서 유사 묶음을 찾는다.
        /// 늦게 끝난 검색은 Chain/Layer/예시가 바뀌었으면 화면에 적용하지 않는다.
        /// </summary>
        private async void FindChainSimilarGroups()
        {
            if (!HasSelectedChain || !_activeChain.Visible || !_chainTraceResultShown
                || _chainExampleSelecting || _chainSimilarSearching || _chainExampleKeys.Count == 0)
                return;
            ClearChainSimilarPreview();
            HashSet<int> checkedLayers = GetCheckedChainLayers();
            ChainTraceElement[] chainElements = map.GetChainTraceElements(GetEditedChainKeys())
                .Where(element => checkedLayers.Contains(element.LayerId)).ToArray();
            string[] exampleKeys = _chainExampleKeys.ToArray();
            string[] protectedKeys = _chainInputSelections.Keys.Concat(_chainOutputSelections.Keys).ToArray();
            ChainDefinition selectedChain = _activeChain;
            var cancellation = new CancellationTokenSource();
            _chainSimilarCancellation = cancellation;
            int generation = ++_chainSimilarGeneration;
            _chainSimilarSearching = true;
            UpdateChainSelectionButtons();
            _lblChainSetupStatus.Text = "현재 Chain에서 유사 묶음을 찾는 중입니다.";
            try
            {
                IList<ChainSimilarGroup> found = await Task.Run(() =>
                    new ChainSimilarGroupFinder().Find(chainElements, exampleKeys,
                        protectedKeys, 0.0001, cancellation.Token));
                if (generation != _chainSimilarGeneration
                    || !ReferenceEquals(selectedChain, _activeChain)) return;
                _chainSimilarGroups = found;
                ShowChainSimilarPreview(chainElements, found);
                FillChainSimilarList(found);
                int protectedCount = found.Count(group => group.IsProtected);
                int ambiguousCount = found.Count(group => group.IsAmbiguous);
                _lblChainSetupStatus.Text = "유사 묶음 " + found.Count + "개 / 보호 "
                    + protectedCount + "개 / 검토 필요 " + ambiguousCount
                    + "개 / 녹색 일반, 주황 단자 보호, 빨강 검토 필요, 자주색 예시";
            }
            catch (OperationCanceledException)
            {
                // Chain/Layer 변경 또는 미리보기 취소가 요청한 검색은 화면에 반영하지 않는다.
            }
            catch (Exception ex)
            {
                if (generation == _chainSimilarGeneration)
                    _lblChainSetupStatus.Text = "유사 묶음 찾기 실패: " + ex.Message;
            }
            finally
            {
                if (generation == _chainSimilarGeneration)
                {
                    _chainSimilarSearching = false;
                    _chainSimilarCancellation = null;
                    UpdateChainSelectionButtons();
                }
                cancellation.Dispose();
            }
        }

        /// <summary>묶음이 공유하는 Element는 검토 필요 색을 우선하여 한 번만 그린다.</summary>
        private void ShowChainSimilarPreview(IEnumerable<ChainTraceElement> elements,
            IEnumerable<ChainSimilarGroup> groups)
        {
            var ambiguous = new HashSet<string>(groups.Where(group => group.IsAmbiguous)
                .SelectMany(group => group.ElementKeys), StringComparer.Ordinal);
            var protectedKeys = new HashSet<string>(groups.Where(group => group.IsProtected)
                .SelectMany(group => group.ElementKeys), StringComparer.Ordinal);
            protectedKeys.ExceptWith(ambiguous);
            var ready = new HashSet<string>(groups.Where(group => !group.IsProtected && !group.IsAmbiguous)
                .SelectMany(group => group.ElementKeys), StringComparer.Ordinal);
            ready.ExceptWith(ambiguous);
            ready.ExceptWith(protectedKeys);
            ChainTraceElement[] snapshot = elements.ToArray();
            map.SetChainSimilarPreview(snapshot.Where(element => ready.Contains(element.ElementKey)),
                snapshot.Where(element => protectedKeys.Contains(element.ElementKey)),
                snapshot.Where(element => ambiguous.Contains(element.ElementKey)));
        }

        /// <summary>검색을 취소하고 색 표시만 지운다. 예시 선택과 Chain 소속은 그대로 둔다.</summary>
        private void ClearChainSimilarPreview()
        {
            if (!_chainSimilarSearching && _chainSimilarGroups.Count == 0) return;
            _chainSimilarGeneration++;
            if (_chainSimilarCancellation != null)
            {
                _chainSimilarCancellation.Cancel();
                _chainSimilarCancellation = null;
            }
            _chainSimilarSearching = false;
            _chainSimilarGroups = new List<ChainSimilarGroup>();
            _updatingChainSimilarList = true;
            try { _chainSimilarList.Items.Clear(); }
            finally { _updatingChainSimilarList = false; }
            map.SetChainSimilarPreview(null, null, null);
            map.SetChainSimilarSelected(null);
            UpdateChainSelectionButtons();
        }
    }
}
