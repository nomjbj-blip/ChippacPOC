using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// 클릭 위치에 겹친 GDS Element를 엔지니어가 직접 확인하고 Input/Output으로 확정하는 창이다.
    /// 후보를 바꿀 때 지도에 미리보기만 요청하며 확인 버튼을 누르기 전에는 설정값을 변경하지 않는다.
    /// </summary>
    public sealed class ChainElementCandidateForm : Form
    {
        private readonly IList<ChainElementCandidate> _candidates;
        private readonly Action<ChainElementCandidate> _preview;
        private readonly DataGridView _grid;

        public ChainElementCandidate SelectedCandidate { get; private set; }

        public ChainElementCandidateForm(string role, IList<ChainElementCandidate> candidates,
            Action<ChainElementCandidate> preview)
        {
            if (candidates == null || candidates.Count == 0)
                throw new ArgumentException("표시할 Element 후보가 없습니다.", nameof(candidates));

            _candidates = candidates;
            _preview = preview;
            Text = role + " Element 선택";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1000, 460);
            MinimumSize = new Size(720, 320);

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            AddColumn("번호", 45);
            AddColumn("Layer", 55);
            AddColumn("DataType", 65);
            AddColumn("Element", 95);
            AddColumn("Min X", 90);
            AddColumn("Min Y", 90);
            AddColumn("Max X", 90);
            AddColumn("Max Y", 90);
            AddColumn("Point 수", 65);
            AddColumn("PATH 폭", 75);

            foreach (ChainElementCandidate candidate in _candidates)
            {
                int index = _grid.Rows.Add(_grid.Rows.Count + 1, candidate.LayerId,
                    candidate.DataType, candidate.ElementType,
                    candidate.Bounds.MinX.ToString("0.###"), candidate.Bounds.MinY.ToString("0.###"),
                    candidate.Bounds.MaxX.ToString("0.###"), candidate.Bounds.MaxY.ToString("0.###"),
                    candidate.PointCount, candidate.PathWidth.ToString("0.###"));
                _grid.Rows[index].Tag = candidate;
            }

            var confirm = new Button { Text = role + " 확정", Width = 110, Height = 30 };
            var cancel = new Button { Text = "취소", Width = 80, Height = 30, DialogResult = DialogResult.Cancel };
            var footer = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 42,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };
            footer.Controls.Add(cancel);
            footer.Controls.Add(confirm);
            Controls.Add(_grid);
            Controls.Add(footer);
            AcceptButton = confirm;
            CancelButton = cancel;

            _grid.SelectionChanged += (sender, args) => PreviewSelectedCandidate();
            _grid.CellDoubleClick += (sender, args) => ConfirmSelection();
            confirm.Click += (sender, args) => ConfirmSelection();
            Shown += (sender, args) => PreviewSelectedCandidate();
        }

        /// <summary>후보의 속성을 같은 너비 정책의 조회 전용 열로 표시한다.</summary>
        private void AddColumn(string title, int minimumWidth)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = title,
                MinimumWidth = minimumWidth,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        /// <summary>목록 행을 바꿀 때 선택한 배치 Element 한 개를 지도에 미리 강조한다.</summary>
        private void PreviewSelectedCandidate()
        {
            if (_grid.CurrentRow == null)
                return;
            _preview?.Invoke(_grid.CurrentRow.Tag as ChainElementCandidate);
        }

        /// <summary>엔지니어가 확인한 행을 최종 선택 결과로 반환한다.</summary>
        private void ConfirmSelection()
        {
            SelectedCandidate = _grid.CurrentRow?.Tag as ChainElementCandidate;
            if (SelectedCandidate == null)
                return;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
