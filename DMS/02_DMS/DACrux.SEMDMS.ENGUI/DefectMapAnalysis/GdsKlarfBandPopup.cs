using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// [색 구간 설정] 팝업 (GDS-KLARF 연관 분석 Simple 화면용, 2026-10-22).
    /// 설계: 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md 3.5절 (결정 S8)
    ///
    /// 화면 A(frmGdsKlarfAnalysis)는 자기 색 구간 표(pnlBand)를 작은 창으로 옮겨 쓰는 방식이라 다른 화면이 가져다 쓸 수 없다.
    /// 그래서 같은 모양 / 동작(자동 구간 / 표시 기준 / FROM · TO / 색 / Die 수 + 기준 미만 / Defect 없음 행)을 창 하나로 만들었다. 화면 A 는 바꾸지 않는다.
    /// 색 기준은 Chain 위 Defect 수(DIE.IN_CHAIN_COUNT) 하나다. 구간 값은 화면을 연 동안만 유지한다(저장 안 함, 화면 A 와 같음).
    ///
    /// 사용 순서
    ///  1) 분석 결과를 받으면 SetDieValues(Die 마다 [Defect 수, Chain 위 Defect 수]) -> 자동 구간이면 구간을 다시 만들고 표를 채운다.
    ///  2) 화면은 DieColor 로 Map 을 칠한다.
    ///  3) 사용자가 구간 / 색 / 자동 구간 / 표시 기준을 바꾸면 BandsChanged -> 화면이 Map 을 다시 칠한다.
    /// 닫기(X / Esc)는 숨기기만 하고, 화면을 닫을 때 화면이 해제한다.
    /// </summary>
    public partial class GdsKlarfBandPopup : Form
    {
        /// <summary>고칠 수 없는 행(기준 미만 / Defect 없음) 표시.</summary>
        private const string FixedRowTag = "FIXED";

        private List<GdsKlarfColorBand> _bands = GdsKlarfColorBands.Auto(1, 1);

        /// <summary>마지막 분석 결과의 Die 값 [Defect 수, Chain 위 Defect 수]. 구간 표 Die 수와 자동 구간 최대값에 쓴다.</summary>
        private readonly List<int[]> _values = new List<int[]>();

        private bool _loading;

        /// <summary>사용자가 구간 / 색 / 자동 구간 / 표시 기준을 바꿨다. 화면은 Map 을 다시 칠한다.</summary>
        public event EventHandler BandsChanged;

        public GdsKlarfBandPopup()
        {
            InitializeComponent();
            // FROM 칸에 고정 행 이름("기준 미만" / "Defect 없음")이 들어가므로 다른 칸보다 넓게
            colBandFrom.FillWeight = 130;
            RefreshGrid();
        }

        /// <summary>표시 기준(이 값보다 적은 Chain 위 Defect 수는 연회색).</summary>
        public int Threshold { get { return (int)numThreshold.Value; } }

        /// <summary>자동 구간 사용 여부.</summary>
        public bool AutoBand { get { return chkAutoBand.Checked; } }

        /// <summary>현재 색 구간(복사본).</summary>
        public List<GdsKlarfColorBand> Bands
        {
            get { return _bands.Select(b => new GdsKlarfColorBand { From = b.From, To = b.To, Color = b.Color }).ToList(); }
        }

        /// <summary>Die 채움색: Defect 없음 흰색 / 표시 기준 미만 연회색 / 그 밖 구간 색(GdsKlarfColorBands.DieColor).</summary>
        public Color DieColor(int defectCount, int chainCount)
        {
            return GdsKlarfColorBands.DieColor(defectCount, chainCount, Threshold, _bands);
        }

        /// <summary>
        /// 분석 결과의 Die 값을 받는다(Wafer 를 바꾸면 빈 목록). 자동 구간이면 최대 Chain 위 Defect 수로 구간을 다시 만든다.
        /// 사용자가 고친 구간(자동 구간 꺼짐)은 Wafer 를 바꿔도 그대로 둔다(화면 A 와 같음).
        /// </summary>
        public void SetDieValues(IEnumerable<int[]> values)
        {
            _values.Clear();
            if (values != null) _values.AddRange(values);
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxValue());
            RefreshGrid();
        }

        /// <summary>기준 컨트롤(버튼) 오른쪽 아래에 맞춰 보인다. 이미 보이면 앞으로 가져온다.</summary>
        public void ShowNear(System.Windows.Forms.Control anchor)
        {
            Location = anchor.PointToScreen(new Point(anchor.Width - Width, anchor.Height + 2));
            if (!Visible) Show(anchor.TopLevelControl as Form);
            Activate();
        }

        /// <summary>X 로 닫으면 숨기기만 한다(구간 값 유지).</summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }
            base.OnFormClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && !dgvBand.IsCurrentCellInEditMode) Hide();
            base.OnKeyDown(e);
        }

        private int MaxValue()
        {
            return _values.Select(v => v[1]).DefaultIfEmpty(0).Max();
        }

        /// <summary>구간 표를 다시 채운다: 구간 행 + 기준 미만 / Defect 없음(고정 행). Die 수 = 마지막 분석 결과 기준.</summary>
        private void RefreshGrid()
        {
            int threshold = Threshold;
            dgvBand.Rows.Clear();
            foreach (GdsKlarfColorBand band in _bands)
                AddBandRow(band, _values.Count(d => d[0] > 0 && d[1] >= threshold && band.Contains(d[1])));
            AddFixedRow("기준 미만", GdsKlarfColorBands.BelowThresholdColor, _values.Count(d => d[0] > 0 && d[1] < threshold));
            AddFixedRow("Defect 없음", GdsKlarfColorBands.NoDefectColor, _values.Count(d => d[0] == 0));
        }

        private void AddBandRow(GdsKlarfColorBand band, int count)
        {
            int i = dgvBand.Rows.Add(band.From, band.To == GdsKlarfColorBands.OpenEnd ? "이상" : band.To.ToString(CultureInfo.InvariantCulture), "", count);
            PaintColorCell(dgvBand.Rows[i], band.Color);
        }

        private void AddFixedRow(string text, Color color, int count)
        {
            int i = dgvBand.Rows.Add(text, "", "", count);
            DataGridViewRow row = dgvBand.Rows[i];
            row.ReadOnly = true;
            row.Tag = FixedRowTag;
            row.DefaultCellStyle.ForeColor = Color.DimGray;
            PaintColorCell(row, color);
        }

        private void PaintColorCell(DataGridViewRow row, Color color)
        {
            DataGridViewCell cell = row.Cells[colBandColor.Index];
            cell.Style.BackColor = color;
            cell.Style.SelectionBackColor = color;
        }

        /// <summary>FROM / TO 를 고치면 사용자 구간이 된다. 숫자가 아니거나 FROM &gt; TO 면 되돌린다. "이상"(빈 값)은 끝까지(999).</summary>
        private void dgvBand_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvBand.Rows[e.RowIndex].Tag != null || e.RowIndex >= _bands.Count) return;
            DataGridViewRow row = dgvBand.Rows[e.RowIndex];
            int from, to;
            string toText = System.Convert.ToString(row.Cells[colBandTo.Index].Value).Trim();
            if (!int.TryParse(System.Convert.ToString(row.Cells[colBandFrom.Index].Value), out from) || from < 1)
            {
                BeginInvoke(new Action(RefreshGrid));
                return;
            }
            if (toText.Length == 0 || toText == "이상") to = GdsKlarfColorBands.OpenEnd;
            else if (!int.TryParse(toText, out to) || to < from)
            {
                BeginInvoke(new Action(RefreshGrid));
                return;
            }
            _bands[e.RowIndex].From = from;
            _bands[e.RowIndex].To = to;
            SetUserBands();
        }

        /// <summary>색 칸을 더블클릭하면 색을 고른다(화면 A / 기존 Defect Count Color 와 같은 방식).</summary>
        private void dgvBand_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colBandColor.Index || dgvBand.Rows[e.RowIndex].Tag != null || e.RowIndex >= _bands.Count) return;
            colorDialog.Color = _bands[e.RowIndex].Color;
            if (colorDialog.ShowDialog(this) != DialogResult.OK) return;
            _bands[e.RowIndex].Color = colorDialog.Color;
            SetUserBands();
        }

        /// <summary>
        /// 사용자 구간으로 바꾼다(자동 구간 끔, Wafer 를 바꿔도 유지).
        /// 표 이벤트(CellEndEdit / CellDoubleClick) 안에서 Rows.Clear 를 하면 DataGridView 재진입 예외가 나므로(화면 A 2026-10-12 확인)
        /// 이벤트가 끝난 뒤(BeginInvoke) 표를 다시 채우고 화면에 알린다.
        /// </summary>
        private void SetUserBands()
        {
            _loading = true;
            chkAutoBand.Checked = false;
            _loading = false;
            BeginInvoke(new Action(() => { RefreshGrid(); OnBandsChanged(); }));
        }

        private void chkAutoBand_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading || !chkAutoBand.Checked) return;
            _bands = GdsKlarfColorBands.Auto(Threshold, MaxValue());
            RefreshGrid();
            OnBandsChanged();
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (chkAutoBand.Checked) _bands = GdsKlarfColorBands.Auto(Threshold, MaxValue());
            else if (_bands.Count > 0) _bands[0].From = Math.Min(Threshold, _bands[0].To);
            RefreshGrid();
            OnBandsChanged();
        }

        private void OnBandsChanged()
        {
            EventHandler handler = BandsChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
