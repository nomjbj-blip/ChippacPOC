using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// 현재 GDS의 Layer 목록을 사용하여 Chain Layer 연결 규칙을 행 단위로 편집한다.
    /// 엔지니어가 규칙 문자열을 직접 작성하지 않고 Layer 조합과 허용 오차를 선택할 수 있게 한다.
    /// </summary>
    public sealed class ChainLayerRuleEditorForm : Form
    {
        private readonly List<int> _layerIds;
        private readonly DataGridView _grid;

        public IList<ChainLayerConnectionRule> Rules { get; private set; }

        /// <summary>
        /// GDS에 존재하는 Layer와 현재 편집 중인 규칙으로 편집 화면을 구성한다.
        /// 저장 버튼을 누르기 전에는 호출 화면의 규칙을 변경하지 않는다.
        /// </summary>
        public ChainLayerRuleEditorForm(
            IEnumerable<int> layerIds,
            IEnumerable<ChainLayerConnectionRule> currentRules)
        {
            _layerIds = (layerIds ?? Enumerable.Empty<int>()).Distinct().OrderBy(id => id).ToList();
            Rules = new List<ChainLayerConnectionRule>();

            Text = "Chain Layer 연결 규칙";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(560, 360);
            Size = new Size(620, 430);
            ShowInTaskbar = false;

            _grid = CreateRuleGrid();
            Controls.Add(_grid);
            Controls.Add(CreateCommandPanel());

            foreach (ChainLayerConnectionRule rule in currentRules ?? Enumerable.Empty<ChainLayerConnectionRule>())
                AddRuleRow(rule.FirstLayerId, rule.SecondLayerId, rule.Tolerance);
        }

        /// <summary>
        /// From Layer, To Layer, 연결 허용 오차를 한 행에서 편집하는 표를 만든다.
        /// Layer 열에는 현재 GDS에 존재하는 값만 선택할 수 있도록 목록을 제한한다.
        /// </summary>
        private DataGridView CreateRuleGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = SystemColors.Window
            };

            grid.Columns.Add(CreateLayerColumn("FromLayer", "From Layer"));
            grid.Columns.Add(CreateLayerColumn("ToLayer", "To Layer"));
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tolerance",
                HeaderText = "연결 허용 오차",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 130
            });
            grid.DataError += (sender, args) => args.ThrowException = false;
            return grid;
        }

        /// <summary>현재 GDS Layer 번호만 선택할 수 있는 표의 Layer 열을 만든다.</summary>
        private DataGridViewComboBoxColumn CreateLayerColumn(string name, string headerText)
        {
            return new DataGridViewComboBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataSource = _layerIds.ToList(),
                Width = 150,
                FlatStyle = FlatStyle.Flat
            };
        }

        /// <summary>규칙 추가, 선택 행 삭제, 적용과 취소 버튼을 배치한다.</summary>
        private Control CreateCommandPanel()
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6)
            };

            var addButton = new Button { Text = "규칙 추가", Width = 100, Height = 30 };
            var deleteButton = new Button { Text = "선택 삭제", Width = 100, Height = 30 };
            var applyButton = new Button { Text = "적용", Width = 90, Height = 30 };
            var cancelButton = new Button { Text = "취소", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };

            addButton.Click += (sender, args) => AddEmptyRuleRow();
            deleteButton.Click += (sender, args) => DeleteSelectedRows();
            applyButton.Click += ApplyButton_Click;
            panel.Controls.Add(addButton);
            panel.Controls.Add(deleteButton);
            panel.Controls.Add(new Label { Width = 70, Height = 1 });
            panel.Controls.Add(applyButton);
            panel.Controls.Add(cancelButton);
            CancelButton = cancelButton;
            return panel;
        }

        /// <summary>현재 Layer 목록의 첫 항목을 기본값으로 사용하여 새 규칙 행을 추가한다.</summary>
        private void AddEmptyRuleRow()
        {
            if (_layerIds.Count == 0)
            {
                MessageBox.Show(this, "현재 GDS에 Layer가 없습니다.", "Layer 규칙", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AddRuleRow(_layerIds[0], _layerIds[0], 0);
        }

        /// <summary>Layer 조합과 허용 오차를 편집 표의 한 행으로 추가한다.</summary>
        private void AddRuleRow(int firstLayerId, int secondLayerId, double tolerance)
        {
            int rowIndex = _grid.Rows.Add();
            DataGridViewRow row = _grid.Rows[rowIndex];
            row.Cells["FromLayer"].Value = firstLayerId;
            row.Cells["ToLayer"].Value = secondLayerId;
            row.Cells["Tolerance"].Value = tolerance.ToString("0.###", CultureInfo.CurrentCulture);
        }

        /// <summary>선택한 규칙 행을 아래 행부터 삭제하여 행 번호 변경의 영향을 받지 않게 한다.</summary>
        private void DeleteSelectedRows()
        {
            foreach (DataGridViewRow row in _grid.SelectedRows.Cast<DataGridViewRow>().OrderByDescending(row => row.Index))
                _grid.Rows.RemoveAt(row.Index);
        }

        /// <summary>화면의 모든 행을 검증한 뒤 호출 화면에 전달할 규칙 목록을 확정한다.</summary>
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (!TryBuildRules(out List<ChainLayerConnectionRule> rules, out string message))
            {
                MessageBox.Show(this, message, "Layer 규칙", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Rules = rules;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 표의 값을 후보 추적 모델로 변환하고 Layer 조합 중복과 허용 오차를 검사한다.
        /// Layer 연결은 양방향이므로 1에서 2와 2에서 1은 같은 조합으로 판단한다.
        /// </summary>
        private bool TryBuildRules(out List<ChainLayerConnectionRule> rules, out string message)
        {
            rules = new List<ChainLayerConnectionRule>();
            message = string.Empty;
            var pairKeys = new HashSet<string>(StringComparer.Ordinal);

            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.Cells["FromLayer"].Value == null || row.Cells["ToLayer"].Value == null)
                {
                    message = "모든 규칙의 From Layer와 To Layer를 선택하세요.";
                    return false;
                }

                int firstLayerId = Convert.ToInt32(row.Cells["FromLayer"].Value, CultureInfo.InvariantCulture);
                int secondLayerId = Convert.ToInt32(row.Cells["ToLayer"].Value, CultureInfo.InvariantCulture);
                string toleranceText = Convert.ToString(row.Cells["Tolerance"].Value, CultureInfo.CurrentCulture);
                if (!TryParseTolerance(toleranceText, out double tolerance) || tolerance < 0)
                {
                    message = "연결 허용 오차는 0 이상의 숫자로 입력하세요.";
                    return false;
                }

                string pairKey = Math.Min(firstLayerId, secondLayerId) + ":" + Math.Max(firstLayerId, secondLayerId);
                if (!pairKeys.Add(pairKey))
                {
                    message = "같은 Layer 조합이 중복되어 있습니다: Layer "
                        + firstLayerId + " / Layer " + secondLayerId;
                    return false;
                }

                rules.Add(new ChainLayerConnectionRule(firstLayerId, secondLayerId, tolerance));
            }

            if (rules.Count == 0)
            {
                message = "Layer 연결 규칙을 한 개 이상 추가하세요.";
                return false;
            }

            return true;
        }

        /// <summary>Windows 지역 소수점과 점 소수점을 모두 허용하여 허용 오차를 읽는다.</summary>
        private static bool TryParseTolerance(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value)
                || double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}
