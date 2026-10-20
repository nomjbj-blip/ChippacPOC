using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// 연관 분석 [준비] 탭의 [직접 찾기] 창. 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 16.6절
    ///
    /// Wafer Key 규칙(고객 협의 대상)으로 같은 Wafer 의 KLARF Step / E-Test 파일을 못 찾을 때 엔지니어가 목록에서 직접 고른다.
    /// 찾는 서비스는 부르는 화면이 Search 로 넘긴다(KLARF = 기존 DMS Wafer 목록, E-Test = 기존 E-Test 결과 조회 목록).
    /// 목록 열 이름이 "_" 로 시작하면 숨기고 키로만 쓴다. 고른 행은 저장하지 않는다(부르는 화면의 [준비] 표에 "직접" 으로 더한다).
    /// </summary>
    public partial class frmGdsKlarfSourceFind : Form
    {
        private readonly List<DataRow> _selected = new List<DataRow>();

        public frmGdsKlarfSourceFind()
        {
            InitializeComponent();
            cboProgram.SelectedIndex = 0;
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지
        }

        /// <summary>찾기 함수 (Device, 공정(빈 값 = 전체), Lot 글자) -> 표시용 표.</summary>
        public Func<string, string, string, DataTable> Search { get; set; }

        /// <summary>창 위 비교용 문구(지금 Wafer 의 Lot / Slot / Step).</summary>
        public string CompareText
        {
            get { return lblCompare.Text; }
            set { lblCompare.Text = value; }
        }

        /// <summary>Device 칸 보이기(E-Test 목록 서비스는 Device 가 필요하다).</summary>
        public bool ShowDevice
        {
            get { return txtDevice.Visible; }
            set { lblDevice.Visible = value; txtDevice.Visible = value; }
        }

        /// <summary>공정 콤보 보이기(E-Test 만).</summary>
        public bool ShowProgram
        {
            get { return cboProgram.Visible; }
            set { lblProgram.Visible = value; cboProgram.Visible = value; }
        }

        public string DeviceFilter
        {
            get { return txtDevice.Text.Trim(); }
            set { txtDevice.Text = value; }
        }

        public string LotFilter
        {
            get { return txtLot.Text.Trim(); }
            set { txtLot.Text = value; }
        }

        /// <summary>한 행만 고르게 할지(E-Test 는 공정 1개, 결정 B19).</summary>
        public bool SingleSelect
        {
            get { return !dgvList.MultiSelect; }
            set { dgvList.MultiSelect = !value; }
        }

        /// <summary>[추가] 로 고른 행.</summary>
        public List<DataRow> SelectedRows { get { return _selected; } }

        /// <summary>
        /// 찾기: Lot 글자는 3자 이상(목록이 너무 커지지 않게), E-Test 는 Device 도 필요하다.
        /// 서버 조회 동안 모래시계 커서. 오류는 메시지 창으로 알린다.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (Search == null) return;
            if (ShowDevice && DeviceFilter.Length == 0)
            {
                MessageBox.Show(this, "Device 를 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!ShowDevice && LotFilter.Length < 3)
            {
                MessageBox.Show(this, "Lot 글자를 3자 이상 입력하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                DataTable table;
                using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
                    table = Search(DeviceFilter, cboProgram.SelectedIndex <= 0 ? "" : cboProgram.Text, LotFilter);
                ShowList(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>찾은 목록을 보인다. "_" 로 시작하는 열은 숨긴다. 레이아웃 확인 때 바로 채울 수도 있다.</summary>
        public void ShowList(DataTable table)
        {
            dgvList.DataSource = table;
            foreach (DataGridViewColumn column in dgvList.Columns)
                if (column.Name.StartsWith("_", StringComparison.Ordinal)) column.Visible = false;
            lblCount.Text = (table == null ? 0 : table.Rows.Count) + " 건";
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            btnSearch_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _selected.Clear();
            foreach (DataGridViewRow row in dgvList.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index))
            {
                var view = row.DataBoundItem as DataRowView;
                if (view != null) _selected.Add(view.Row);
            }
            if (_selected.Count == 0)
            {
                MessageBox.Show(this, "목록에서 행을 고르세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnAdd_Click(sender, e);
        }
    }
}
