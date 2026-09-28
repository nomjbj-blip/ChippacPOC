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
    /// GDS 테스트 화면에 Chain Input, Output, Layer 필터와 선택적 Layer 규칙을 입력하는 POC 패널을 제공한다.
    /// 후보 경로 검증이 끝나기 전까지 DB 저장 기능과 확정 Chain 관리는 포함하지 않는다.
    /// </summary>
    public partial class GdsMapTestForm
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
        private Button _btnChainInput;
        private Button _btnChainOutput;
        private Button _btnChainApply;
        private Button _btnChainCancel;
        private Button _btnChainTrace;
        private Button _btnChainAdd;
        private Button _btnChainRemove;
        private Button _btnChainRuleEdit;
        private CheckBox _chkChainApplyLayerRules;
        private bool _chainTraceResultShown;
        private int _chainTraceGeneration;
        private TextBox _txtChainLayerRules;
        private List<ChainLayerConnectionRule> _chainLayerRules = new List<ChainLayerConnectionRule>();
        private NumericUpDown _numChainCellSize;
        private Label _lblChainSetupStatus;

        /// <summary>
        /// 기존 Designer를 크게 변경하지 않고 Map 탭 위쪽에 Chain 설정용 컨트롤을 추가한다.
        /// 기존 파일 조회, Layer 목록, Grid 기능은 그대로 유지한다.
        /// </summary>
        private void InitializeChainSetupPanel()
        {
            var panel = new TableLayoutPanel
            {
                Name = "chainSetupPanel",
                Dock = DockStyle.Top,
                Height = 118,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = SystemColors.Control,
                Padding = new Padding(4)
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));

            var firstRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                AutoScroll = true
            };
            var secondRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                AutoScroll = true
            };

            _btnChainInput = CreateChainButton("Input 지정", (sender, args) => BeginChainPointCapture(ChainPointCaptureMode.Input));
            _btnChainOutput = CreateChainButton("Output 지정", (sender, args) => BeginChainPointCapture(ChainPointCaptureMode.Output));
            _btnChainInput.Width = 100;
            _btnChainOutput.Width = 100;
            _btnChainTrace = CreateChainButton("후보 경로 찾기", BtnChainTrace_Click);
            _btnChainTrace.Width = 115;

            _btnChainApply = CreateChainButton("선택 적용", (sender, args) => ApplyChainEndpointSelection());
            _btnChainCancel = CreateChainButton("선택 취소", (sender, args) => CancelChainEndpointSelection());
            _btnChainApply.Width = 80;
            _btnChainCancel.Width = 80;

            firstRow.Controls.Add(_btnChainInput);
            firstRow.Controls.Add(_btnChainOutput);
            firstRow.Controls.Add(_btnChainApply);
            firstRow.Controls.Add(_btnChainCancel);
            firstRow.Controls.Add(_btnChainTrace);
            _btnChainAdd = CreateChainButton("경로 추가", (sender, args) => ToggleChainEditMode(ChainEditMode.Add));
            _btnChainRemove = CreateChainButton("경로 제외", (sender, args) => ToggleChainEditMode(ChainEditMode.Remove));
            _btnChainAdd.Width = 85;
            _btnChainRemove.Width = 85;
            firstRow.Controls.Add(_btnChainAdd);
            firstRow.Controls.Add(_btnChainRemove);

            _chkChainApplyLayerRules = new CheckBox
            {
                Text = "Layer 규칙 적용",
                AutoSize = true,
                Margin = new Padding(8, 7, 3, 0)
            };
            _chkChainApplyLayerRules.CheckedChanged += (sender, args) => InvalidateChainTraceResult();
            secondRow.Controls.Add(_chkChainApplyLayerRules);
            secondRow.Controls.Add(CreateChainLabel("Layer 규칙"));
            _txtChainLayerRules = new TextBox
            {
                Name = "txtChainLayerRules",
                Width = 220,
                ReadOnly = true,
                Text = "규칙 없음"
            };
            secondRow.Controls.Add(_txtChainLayerRules);
            _btnChainRuleEdit = CreateChainButton("Layer 규칙 편집", BtnChainRuleEdit_Click);
            _btnChainRuleEdit.Width = 115;
            secondRow.Controls.Add(_btnChainRuleEdit);
            secondRow.Controls.Add(CreateChainLabel("격자 크기"));
            _numChainCellSize = CreateChainNumber(0.001m, 1000000000, 10, 3);
            secondRow.Controls.Add(_numChainCellSize);
            _lblChainSetupStatus = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 3, 3, 0),
                Text = "GDS 조회 후 Input부터 지정하세요."
            };

            panel.Controls.Add(firstRow, 0, 0);
            panel.Controls.Add(secondRow, 0, 1);
            panel.Controls.Add(_lblChainSetupStatus, 0, 2);
            tabPage1.Controls.Add(panel);
            panel.BringToFront();

            map.MouseClick += Map_ChainMouseClick;
            map.MouseDown += (sender, args) => _chainMouseDownPoint = args.Location;
            KeyPreview = true;
            KeyDown += GdsMapTestForm_ChainKeyDown;
            SetChainButtonsEnabled(false);
            UpdateChainSelectionButtons();
        }

        /// <summary>
        /// 현재 GDS Layer 목록을 규칙 편집 화면에 전달하고 적용한 규칙을 후보 추적 입력으로 보관한다.
        /// 편집 화면에서 취소한 경우 현재 규칙은 그대로 유지한다.
        /// </summary>
        private void BtnChainRuleEdit_Click(object sender, EventArgs e)
        {
            List<int> layerIds = chkLayerItems.Items
                .Cast<LayerDisplayItem>()
                .Select(item => item.LayerId)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            using (var editor = new ChainLayerRuleEditorForm(layerIds, _chainLayerRules))
            {
                if (editor.ShowDialog(this) != DialogResult.OK)
                    return;

                _chainLayerRules = editor.Rules.ToList();
                UpdateChainLayerRuleSummary();
                InvalidateChainTraceResult();
                _lblChainSetupStatus.Text = "Layer 규칙 " + _chainLayerRules.Count + "개를 보관했습니다.";
            }
        }

        /// <summary>현재 Layer 규칙 수와 일부 조합을 읽기 전용 요약 칸에 표시한다.</summary>
        private void UpdateChainLayerRuleSummary()
        {
            if (_chainLayerRules.Count == 0)
            {
                _txtChainLayerRules.Text = "규칙 없음";
                return;
            }

            string preview = string.Join(", ", _chainLayerRules.Take(3).Select(rule =>
                rule.FirstLayerId + "-" + rule.SecondLayerId + ":" + rule.Tolerance.ToString("0.###")));
            _txtChainLayerRules.Text = _chainLayerRules.Count + "개 / " + preview
                + (_chainLayerRules.Count > 3 ? " ..." : string.Empty);
        }

        /// <summary>Chain 패널에서 같은 크기와 여백을 사용하는 버튼을 만든다.</summary>
        private static Button CreateChainButton(string text, EventHandler clickHandler)
        {
            var button = new Button
            {
                Width = 100,
                Height = 30,
                Margin = new Padding(3),
                Text = text,
                UseVisualStyleBackColor = true
            };
            button.Click += clickHandler;
            return button;
        }

        /// <summary>Chain 설정값의 의미를 짧게 표시하는 공통 Label을 만든다.</summary>
        private static Label CreateChainLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Margin = new Padding(8, 7, 3, 0),
                Text = text
            };
        }

        /// <summary>GDS 좌표 단위에 맞춰 소수점 입력이 가능한 공통 NumericUpDown을 만든다.</summary>
        private static NumericUpDown CreateChainNumber(decimal minimum, decimal maximum, decimal value, int decimalPlaces)
        {
            return new NumericUpDown
            {
                Width = 90,
                Minimum = minimum,
                Maximum = maximum,
                Value = value,
                DecimalPlaces = decimalPlaces,
                Increment = decimalPlaces > 0 ? 0.1m : 1m
            };
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

            if (_chainPointCaptureMode != ChainPointCaptureMode.None || _chainEditMode != ChainEditMode.None)
                return;

            _chainPointCaptureMode = mode;
            var confirmed = mode == ChainPointCaptureMode.Input
                ? _chainInputSelections : _chainOutputSelections;
            _chainDraftSelections = new Dictionary<string, ChainEndpointPick>(confirmed, StringComparer.Ordinal);
            map.Mode = GdsMapControl.ViewMode.View;
            map.Cursor = Cursors.Cross;
            UpdateChainSelectionButtons();
            ShowChainEndpointSelection();
            _lblChainSetupStatus.Text = GetCaptureGuide(mode);
        }

        /// <summary>현재 지정할 좌표의 종류에 맞는 화면 안내 문구를 반환한다.</summary>
        private static string GetCaptureGuide(ChainPointCaptureMode mode)
        {
            switch (mode)
            {
                case ChainPointCaptureMode.Input:
                    return "Input 선택 중 / 지도에서 여러 Element를 클릭한 뒤 선택 적용을 누르세요.";
                case ChainPointCaptureMode.Output:
                    return "Output 선택 중 / 지도에서 여러 Element를 클릭한 뒤 선택 적용을 누르세요.";
                default:
                    return "Chain 설정 대기";
            }
        }

        /// <summary>
        /// 지도 클릭으로 역할별 임시 Element를 전환한다. 화면 이동은 클릭으로 취급하지 않는다.
        /// </summary>
        private void Map_ChainMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left
                || (_chainPointCaptureMode == ChainPointCaptureMode.None && _chainEditMode == ChainEditMode.None))
                return;

            if (Math.Abs(e.X - _chainMouseDownPoint.X) > 4
                || Math.Abs(e.Y - _chainMouseDownPoint.Y) > 4)
                return;

            if (_chainEditMode != ChainEditMode.None)
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
                _lblChainSetupStatus.Text = "선택 Layer의 BOUNDARY/PATH 실제 도형이 클릭 위치에 없습니다.";
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
            menu.Items.Add(new ToolStripSeparator());
            var addAll = new ToolStripMenuItem("이 위치의 후보 모두 추가");
            addAll.Click += (sender, args) => AddChainEndpointCandidates(candidates, point);
            menu.Items.Add(addAll);
            menu.Closed += (sender, args) =>
            {
                ShowChainEndpointSelection();
                // WinForms가 Closed 이후에도 메뉴 종료 처리를 계속하므로 다음 UI 메시지에서 삭제한다.
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(new Action(() => menu.Dispose()));
            };
            menu.Show(map, screenPoint);
        }

        /// <summary>겹친 후보를 한 번에 임시 추가하되 다른 역할에 속한 Element는 건너뛴다.</summary>
        private void AddChainEndpointCandidates(IEnumerable<ChainElementCandidate> candidates, GPoint point)
        {
            var other = _chainPointCaptureMode == ChainPointCaptureMode.Input
                ? _chainOutputSelections : _chainInputSelections;
            int added = 0;
            foreach (ChainElementCandidate candidate in candidates)
            {
                if (other.ContainsKey(candidate.ElementKey)
                    || _chainDraftSelections.ContainsKey(candidate.ElementKey))
                    continue;
                _chainDraftSelections.Add(candidate.ElementKey,
                    new ChainEndpointPick { Candidate = candidate, Point = point });
                added++;
            }
            ShowChainEndpointSelection();
            _lblChainSetupStatus.Text = "이 위치에서 " + added + "개 추가 / 임시 "
                + _chainDraftSelections.Count + "개 / 선택 적용 전 확인하세요.";
        }

        /// <summary>한 Element의 임시 선택을 전환하고 역할별 Marker와 개수를 즉시 갱신한다.</summary>
        private void ToggleChainEndpointCandidate(ChainElementCandidate candidate, GPoint point)
        {
            var other = _chainPointCaptureMode == ChainPointCaptureMode.Input
                ? _chainOutputSelections : _chainInputSelections;
            if (other.ContainsKey(candidate.ElementKey))
            {
                _lblChainSetupStatus.Text = "이 Element는 이미 다른 역할로 지정되었습니다.";
                return;
            }

            if (!_chainDraftSelections.Remove(candidate.ElementKey))
                _chainDraftSelections.Add(candidate.ElementKey,
                    new ChainEndpointPick { Candidate = candidate, Point = point });
            ShowChainEndpointSelection();
            _lblChainSetupStatus.Text = GetCaptureGuide(_chainPointCaptureMode)
                + " / 임시 " + _chainDraftSelections.Count + "개";
        }

        /// <summary>역할별 임시/확정 선택을 합쳐 지도에 표시하고 후보 메뉴의 Hover를 추가한다.</summary>
        private void ShowChainEndpointSelection(ChainElementCandidate hovered = null)
        {
            var input = _chainPointCaptureMode == ChainPointCaptureMode.Input
                ? _chainDraftSelections : _chainInputSelections;
            var output = _chainPointCaptureMode == ChainPointCaptureMode.Output
                ? _chainDraftSelections : _chainOutputSelections;
            map.SetChainEndpointPoints(input.Values.Select(pick => pick.Point),
                output.Values.Select(pick => pick.Point));
            if (_chainPointCaptureMode == ChainPointCaptureMode.None
                && _chainTraceResultShown && _chainLastTraceResult != null)
            {
                ShowEditedChainCandidate();
                return;
            }
            map.HighlightChainElements(input.Keys.Concat(output.Keys).Concat(
                hovered == null ? Enumerable.Empty<string>() : new[] { hovered.ElementKey }));
        }

        /// <summary>임시 선택을 검증한 뒤 해당 역할의 확정 집합에만 반영한다.</summary>
        private void ApplyChainEndpointSelection()
        {
            if (_chainPointCaptureMode == ChainPointCaptureMode.None)
                return;
            if (_chainDraftSelections.Count == 0)
            {
                _lblChainSetupStatus.Text = "Element를 한 개 이상 선택해야 적용할 수 있습니다.";
                return;
            }
            var checkedLayers = new HashSet<int>();
            for (int i = 0; i < chkLayerItems.Items.Count; i++)
                if (chkLayerItems.GetItemChecked(i))
                    checkedLayers.Add(((LayerDisplayItem)chkLayerItems.Items[i]).LayerId);
            if (_chainDraftSelections.Values.Any(pick => !checkedLayers.Contains(pick.Candidate.LayerId)))
            {
                _lblChainSetupStatus.Text = "선택한 Element의 Layer가 해제되었습니다. 다시 체크하거나 선택을 제거하세요.";
                return;
            }

            string role = _chainPointCaptureMode == ChainPointCaptureMode.Input ? "Input" : "Output";
            var confirmed = new Dictionary<string, ChainEndpointPick>(_chainDraftSelections, StringComparer.Ordinal);
            if (_chainPointCaptureMode == ChainPointCaptureMode.Input)
                _chainInputSelections = confirmed;
            else
                _chainOutputSelections = confirmed;
            _chainPointCaptureMode = ChainPointCaptureMode.None;
            _chainDraftSelections = null;
            _chainTraceResultShown = false;
            _chainLastTraceResult = null;
            _chainTraceGeneration++;
            map.Cursor = Cursors.SizeAll;
            UpdateChainSelectionButtons();
            ShowChainEndpointSelection();
            _lblChainSetupStatus.Text = role + " " + confirmed.Count + "개 확정 / Input "
                + _chainInputSelections.Count + "개 / Output " + _chainOutputSelections.Count + "개";
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
            ShowChainEndpointSelection();
            _lblChainSetupStatus.Text = "선택을 취소했습니다. Input " + _chainInputSelections.Count
                + "개 / Output " + _chainOutputSelections.Count + "개";
        }

        /// <summary>Esc로 임시 선택을 취소하여 지도 작업을 마우스와 키보드로 종료할 수 있게 한다.</summary>
        private void GdsMapTestForm_ChainKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            if (_chainEditMode != ChainEditMode.None)
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
                || _chainEditMode != ChainEditMode.None;
            bool loaded = map.Structure != null;
            _btnChainInput.Enabled = loaded && !editing;
            _btnChainOutput.Enabled = loaded && !editing;
            _btnChainTrace.Enabled = loaded && !editing;
            _btnChainAdd.Enabled = loaded && _chainTraceResultShown
                && _chainPointCaptureMode == ChainPointCaptureMode.None
                && (_chainEditMode == ChainEditMode.None || _chainEditMode == ChainEditMode.Add);
            _btnChainRemove.Enabled = loaded && _chainTraceResultShown
                && _chainPointCaptureMode == ChainPointCaptureMode.None
                && (_chainEditMode == ChainEditMode.None || _chainEditMode == ChainEditMode.Remove);
            _btnChainAdd.BackColor = _chainEditMode == ChainEditMode.Add ? Color.LightGoldenrodYellow : SystemColors.Control;
            _btnChainRemove.BackColor = _chainEditMode == ChainEditMode.Remove ? Color.LightGoldenrodYellow : SystemColors.Control;
            _btnChainApply.Enabled = loaded && _chainPointCaptureMode != ChainPointCaptureMode.None;
            _btnChainCancel.Enabled = loaded && _chainPointCaptureMode != ChainPointCaptureMode.None;
        }

        /// <summary>확정된 Input/Output과 현재 후보 경로를 지도에 다시 표시한다.</summary>
        private void RestoreChainEndpointHighlight()
        {
            ShowChainEndpointSelection();
        }

        /// <summary>
        /// 확정된 Element와 왼쪽 Layer 체크 상태를 검증한 뒤 실제 형상 접촉 경로를 계산한다.
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

            _btnChainTrace.Enabled = false;
            _btnChainInput.Enabled = false;
            _btnChainOutput.Enabled = false;
            int traceGeneration = _chainTraceGeneration;
            _lblChainSetupStatus.Text = "후보 경로를 찾는 중입니다.";
            progressMapLoad.Style = ProgressBarStyle.Marquee;
            progressMapLoad.Visible = true;
            try
            {
                string[] inputElementKeys = _chainInputSelections.Keys.ToArray();
                string[] outputElementKeys = _chainOutputSelections.Keys.ToArray();
                bool applyLayerRules = _chkChainApplyLayerRules.Checked;
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
                ShowEditedChainCandidate();
                _lblChainSetupStatus.Text = FormatEditedConnectivity().TrimStart(' ', '/')
                    + " / " + FormatChainTraceResult(result) + FormatChainManualCounts();
            }
            catch (Exception ex)
            {
                if (traceGeneration != _chainTraceGeneration)
                    return;
                _lblChainSetupStatus.Text = "후보 경로 조회 실패";
                MessageBox.Show(this, ex.Message, "Chain Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressMapLoad.Visible = false;
                UpdateChainSelectionButtons();
            }
        }

        /// <summary>
        /// 후보 추적 결과를 성공 경로 수 또는 실패 진단 정보로 표시한다.
        /// 실패 시 Input/Output Layer와 실제 방문 Layer를 보여 주어 빠진 연결 규칙을 판단할 수 있게 한다.
        /// </summary>
        private static string FormatChainTraceResult(ChainTraceResult result)
        {
            if (result.IsConnected)
                return "후보 경로 " + result.Path.Count.ToString("N0") + "개 Element / "
                    + "지정 단자 " + result.EndpointElements.Count.ToString("N0")
                    + "개 / 경로 밖 단자 " + result.OffPathEndpointElements.Count.ToString("N0")
                    + "개(연결 확인 필요) / "
                    + "직접 겹침 " + result.OverlappingElements.Count.ToString("N0")
                    + "개 / 그중 분기 " + result.BranchCandidates.Count.ToString("N0") + "개 / "
                    + result.Path[0].ElementKey + " → "
                    + result.Path[result.Path.Count - 1].ElementKey + " / 사용자 확인 필요";

            string inputLayer = result.InputLayerId.HasValue ? "L" + result.InputLayerId.Value : "확인 불가";
            string outputLayer = result.OutputLayerId.HasValue ? "L" + result.OutputLayerId.Value : "확인 불가";
            string visitedLayers = string.Join(",", result.VisitedElements
                .Select(element => element.LayerId)
                .Distinct()
                .OrderBy(layerId => layerId)
                .Select(layerId => "L" + layerId));
            if (string.IsNullOrEmpty(visitedLayers))
                visitedLayers = "없음";

            return "연결 실패 / Input " + inputLayer
                + " / Output " + outputLayer
                + " / 지정 단자 " + result.EndpointElements.Count.ToString("N0") + "개"
                + " / 방문 " + result.VisitedElements.Count.ToString("N0")
                + "개 / " + visitedLayers;
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
            if (_chainInputSelections.Count == 0 || _chainOutputSelections.Count == 0)
            {
                message = "Input과 Output Element를 각각 확정하세요.";
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
            if (_chkChainApplyLayerRules.Checked && _chainLayerRules.Count == 0)
            {
                message = "Layer 규칙 적용을 사용하려면 규칙을 한 개 이상 추가하세요.";
                return false;
            }

            rules = _chainLayerRules.ToList();
            return true;
        }

        /// <summary>
        /// Layer 체크 또는 규칙이 바뀌면 이전 탐색 결과를 해제한다.
        /// 확정된 Input/Output은 유지하여 엔지니어가 새 조건으로 다시 탐색할 수 있게 한다.
        /// </summary>
        private void InvalidateChainTraceResult()
        {
            _chainTraceGeneration++;
            if (!_chainTraceResultShown)
                return;
            _chainTraceResultShown = false;
            _chainLastTraceResult = null;
            _chainEditMode = ChainEditMode.None;
            map.Cursor = Cursors.SizeAll;
            RestoreChainEndpointHighlight();
            UpdateChainSelectionButtons();
            _lblChainSetupStatus.Text = "Layer 조건이 바뀌었습니다. 후보 경로를 다시 찾으세요.";
        }

        /// <summary>GDS 조회 상태에 따라 Chain 설정 버튼의 사용 가능 상태를 일괄 변경한다.</summary>
        private void SetChainButtonsEnabled(bool enabled)
        {
            _btnChainInput.Enabled = enabled;
            _btnChainOutput.Enabled = enabled;
            _btnChainTrace.Enabled = enabled;
            _btnChainAdd.Enabled = enabled && _chainTraceResultShown;
            _btnChainRemove.Enabled = enabled && _chainTraceResultShown;
            _btnChainApply.Enabled = false;
            _btnChainCancel.Enabled = false;
            _btnChainRuleEdit.Enabled = enabled;
            _chkChainApplyLayerRules.Enabled = enabled;
        }

        /// <summary>새 GDS를 조회한 뒤 이전 도면의 Chain 좌표를 제거하고 설정을 시작할 수 있게 한다.</summary>
        private void ResetChainSetupAfterMapLoad(bool loadSucceeded)
        {
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
            map.Cursor = Cursors.SizeAll;
            _chkChainApplyLayerRules.Checked = false;
            UpdateChainLayerRuleSummary();
            map.ResetChainSetupOverlay();
            SetChainButtonsEnabled(loadSucceeded);
            _lblChainSetupStatus.Text = loadSucceeded
                ? "Input / Output을 지정하고 왼쪽 Layer를 체크하세요."
                : "GDS 조회 후 Input부터 지정하세요.";
        }
    }
}
