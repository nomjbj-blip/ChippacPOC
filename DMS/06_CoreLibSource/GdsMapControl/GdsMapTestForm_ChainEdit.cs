using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 자동 후보를 계산한 다음 엔지니어가 지도에서 Element를 추가하거나 제외하는 화면 동작이다.
    /// GDS 원본과 자동 탐색 결과는 수정하지 않고 현재 설정의 수동 보정 키만 보관한다.
    /// </summary>
    public partial class GdsMapForm
    {
        /// <summary>같은 버튼 또는 Esc로 편집을 끝내며 Input/Output 지정과 동시에 실행되지 않게 한다.</summary>
        private void ToggleChainEditMode(ChainEditMode mode)
        {
            if (_chainPointCaptureMode != ChainPointCaptureMode.None || !_chainTraceResultShown)
                return;
            _chainEditMode = _chainEditMode == mode ? ChainEditMode.None : mode;
            if (_chainEditMode != ChainEditMode.None)
                map.Mode = GdsMapControl.ViewMode.View;
            map.Cursor = _chainEditMode == ChainEditMode.None ? Cursors.SizeAll : Cursors.Cross;
            UpdateChainSelectionButtons();
            _lblChainSetupStatus.Text = _chainEditMode == ChainEditMode.None
                ? FormatEditedConnectivity().TrimStart(' ', '/') + " / 경로 편집 종료"
                    + FormatChainManualCounts()
                : (_chainEditMode == ChainEditMode.Add ? "경로 추가" : "경로 제외")
                    + " 중 / 지도 객체를 클릭하세요. 같은 버튼 또는 Esc로 종료합니다.";
        }

        /// <summary>현재 클릭 위치의 체크된 Layer 객체만 제시하고 겹친 객체는 메뉴에서 구분한다.</summary>
        private void SelectChainEditElement(Point screenPoint)
        {
            var checkedLayers = new HashSet<int>();
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
                if (chkLayerItems.GetItemChecked(i))
                    checkedLayers.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerId);

            List<ChainElementCandidate> candidates = map.GetChainElementCandidates(screenPoint)
                .Where(candidate => checkedLayers.Contains(candidate.LayerId))
                .ToList();
            if (candidates.Count == 0)
            {
                _lblChainSetupStatus.Text = "클릭 위치에 체크된 Layer의 BOUNDARY/PATH가 없습니다.";
                return;
            }
            if (candidates.Count == 1)
            {
                ApplyChainEdit(new[] { candidates[0] });
                return;
            }

            var menu = new ContextMenuStrip();
            var selectedKeys = GetEditedChainKeys();
            foreach (ChainElementCandidate candidate in candidates)
            {
                ChainElementCandidate current = candidate;
                var item = new ToolStripMenuItem("L" + current.LayerId + " / DT" + current.DataType
                    + " / " + current.ElementType + " / " + current.ElementKey)
                {
                    Checked = selectedKeys.Contains(current.ElementKey),
                    ToolTipText = current.Bounds.ToString()
                };
                item.Click += (sender, args) => ApplyChainEdit(new[] { current });
                menu.Items.Add(item);
            }
            menu.Items.Add(new ToolStripSeparator());
            var all = new ToolStripMenuItem(_chainEditMode == ChainEditMode.Add
                ? "이 위치의 객체 모두 추가" : "이 위치의 후보 모두 제외");
            all.Click += (sender, args) => ApplyChainEdit(candidates);
            menu.Items.Add(all);
            menu.Closed += (sender, args) =>
            {
                // WinForms의 메뉴 종료 처리 이후 삭제해야 다시 클릭할 때 예외가 나지 않는다.
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(new Action(() => menu.Dispose()));
            };
            menu.Show(map, screenPoint);
        }

        /// <summary>
        /// 수동 추가는 제외 기록을 취소하고, 수동 제외는 자동 후보의 제외 기록을 남긴다.
        /// Input/Output은 역할 지정 화면에서만 변경하도록 보호한다.
        /// </summary>
        private void ApplyChainEdit(IEnumerable<ChainElementCandidate> candidates)
        {
            if (_chainLastTraceResult == null || _chainEditMode == ChainEditMode.None)
                return;
            ClearChainExampleSelection();
            if (HasSelectedChain) _activeChain.LastSimilarUndo = null;
            var automaticKeys = GetAutomaticChainKeys();
            var endpointKeys = new HashSet<string>(_chainInputSelections.Keys
                .Concat(_chainOutputSelections.Keys), StringComparer.Ordinal);
            int changed = 0;
            int protectedCount = 0;
            foreach (ChainElementCandidate candidate in candidates)
            {
                string key = candidate.ElementKey;
                if (_chainEditMode == ChainEditMode.Remove && endpointKeys.Contains(key))
                {
                    protectedCount++;
                    continue;
                }
                bool wasIncluded = GetEditedChainKeys().Contains(key);
                if (_chainEditMode == ChainEditMode.Add)
                {
                    _chainManualExcluded.Remove(key);
                    if (!automaticKeys.Contains(key)) _chainManualAdded.Add(key);
                }
                else
                {
                    _chainManualAdded.Remove(key);
                    if (automaticKeys.Contains(key)) _chainManualExcluded.Add(key);
                }
                bool isIncluded = _chainEditMode == ChainEditMode.Add;
                if (wasIncluded != isIncluded) changed++;
            }
            ShowEditedChainCandidate();
            _lblChainSetupStatus.Text = FormatEditedConnectivity().TrimStart(' ', '/') + " / "
                + (_chainEditMode == ChainEditMode.Add ? "추가 " : "제외 ")
                + changed + "개 / 단자 보호 " + protectedCount + "개"
                + FormatChainManualCounts();
        }

        /// <summary>현재 자동 추적 결과에서 화면 후보로 표시할 키를 모은다.</summary>
        private HashSet<string> GetAutomaticChainKeys()
        {
            IEnumerable<ChainTraceElement> elements = _chainLastTraceResult.IsConnected
                ? _chainLastTraceResult.CandidateElements
                : _chainLastTraceResult.VisitedElements.Concat(_chainLastTraceResult.EndpointElements);
            return new HashSet<string>(elements.Select(element => element.ElementKey), StringComparer.Ordinal);
        }

        /// <summary>자동 결과에 수동 추가/제외를 적용하되 지정된 Input/Output은 항상 포함한다.</summary>
        private HashSet<string> GetEditedChainKeys()
        {
            var keys = GetAutomaticChainKeys();
            keys.UnionWith(_chainManualAdded);
            keys.ExceptWith(_chainManualExcluded);
            keys.UnionWith(_chainInputSelections.Keys);
            keys.UnionWith(_chainOutputSelections.Keys);
            return keys;
        }

        /// <summary>후보/수동 보정을 Chain 색 외곽선과 역할 Marker에 같은 표시 규칙으로 반영한다.</summary>
        private void ShowEditedChainCandidate()
        {
            RefreshChainDisplay();
        }

        /// <summary>현재 수동 보정 수를 상태 문구에 덧붙인다.</summary>
        private string FormatChainManualCounts()
        {
            return " / 수동 추가 " + _chainManualAdded.Count + "개 / 수동 제외 "
                + _chainManualExcluded.Count + "개";
        }

        /// <summary>
        /// 최종 후보의 실제 도형 연결을 다시 검사해 수동 편집으로 끊긴 경로와 고립 객체를 알린다.
        /// 현재 Layer 밖의 보정 객체가 있으면 성공으로 오인하지 않게 검사 불가를 표시한다.
        /// </summary>
        private string FormatEditedConnectivity()
        {
            if (_chainLastTraceResult == null)
                return string.Empty;
            try
            {
                ChainConnectivityResult check = map.CheckChainSelectionConnectivity(
                    GetEditedChainKeys(), _chainInputSelections.Keys, _chainOutputSelections.Keys,
                    _activeChain.LayerIds, _chainLayerRules, _applyChainLayerRules,
                    (double)_numChainCellSize.Value);
                if (_chainOutputSelections.Count == 0)
                    return check.DisconnectedKeys.Count == 0
                        ? " / Input에서 후보 객체 연결 유지 / Output 미지정"
                        : " / 단절 객체 " + check.DisconnectedKeys.Count + "개 / Output 미지정";
                return check.IsConnected
                    ? " / 도형 연결 유지(전기적 확인 필요)"
                    : " / 연결 검증 필요: Output " + check.ReachedOutputCount + "/"
                        + check.TotalOutputCount + ", 단절 객체 " + check.DisconnectedKeys.Count + "개";
            }
            catch (ArgumentException ex)
            {
                return " / 연결 검사 불가: " + ex.Message;
            }
        }
    }
}
