using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS 대표 Device(Map)를 같이 쓰는 Sub Device 이름을 수동으로 등록하는 창이다(2026-10-12).
    /// 설계: 문서/2026-10-12_GDS_대표Device_SubDevice_설계.md 5.1절.
    /// 첫 행은 대표 Device 자신(언제나 포함, 저장 안 함 / 수정·삭제 불가)이고, 맨 아래 빈 행에 입력해 추가한다.
    /// [저장]은 목록 전체를 서버(GdsMapImport.SaveSubDeviceList)에 보내 통째로 바꾼다. 검사(중복 / 다른 대표에 등록됨)는 서버가 한다.
    /// </summary>
    public partial class SubDeviceSetupForm : Form
    {
        private readonly string _factory;
        private readonly string _device;
        private bool _dirty;
        private bool _filling;
        private bool _busy;

        /// <summary>Sub 목록 조회 서비스. null 이면 원격 호출 RO 를 만든다(Smoke 검사는 같은 프로세스 BSL 을 넣는다).</summary>
        public DACrux.SEMDMS.RO.GdsMapView ViewService { get; set; }

        /// <summary>Sub 목록 저장 서비스. null 이면 원격 호출 RO 를 만든다.</summary>
        public DACrux.SEMDMS.RO.GdsMapImport ImportService { get; set; }

        /// <summary>이 창에서 한 번이라도 저장했으면 true. 호출 화면이 Map 정보를 다시 읽는다.</summary>
        public bool Saved { get; private set; }

        /// <summary>마지막으로 읽은 Sub Device 이름(대표 제외), 쉼표 구분. Map 정보 표시용.</summary>
        public string SubDeviceText { get; private set; }

        public SubDeviceSetupForm(string factory, string device)
        {
            InitializeComponent();
            _factory = factory;
            _device = device;
            SubDeviceText = string.Empty;
            lblRepresentative.Text = "대표 Device: " + device + "   (Factory " + factory + ")";
        }

        private async void SubDeviceSetupForm_Shown(object sender, EventArgs e)
        {
            await LoadListAsync();
        }

        /// <summary>서버에서 Sub 목록을 읽어 표를 채운다. 첫 행은 대표 자신(읽기 전용).</summary>
        private async Task LoadListAsync()
        {
            SetBusy(true, "Sub Device 목록 조회 중");
            try
            {
                string factory = _factory, device = _device;
                DACrux.SEMDMS.RO.GdsMapView service = ViewService ?? new DACrux.SEMDMS.RO.GdsMapView();
                DataTable dt = await Task.Run(() => service.GetSubDeviceList(factory, device));
                _filling = true;
                gridSub.Rows.Clear();
                int rep = gridSub.Rows.Add(_device, "대표 (언제나 포함)");
                DataGridViewRow repRow = gridSub.Rows[rep];
                repRow.ReadOnly = true;
                repRow.Tag = "REP";
                repRow.DefaultCellStyle.BackColor = SystemColors.Control;
                repRow.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                foreach (DataRow row in dt.Rows)
                    gridSub.Rows.Add(row["SUB_DEVICE_ID"].ToString(), row["REMARK"].ToString());
                _filling = false;
                _dirty = false;
                SubDeviceText = string.Join(", ", dt.Rows.Cast<DataRow>().Select(r => r["SUB_DEVICE_ID"].ToString()).ToArray());
                SetBusy(false, "Sub Device " + dt.Rows.Count + "개");
            }
            catch (Exception ex)
            {
                _filling = false;
                SetBusy(false, "조회 실패: " + Cause(ex).Message);
            }
        }

        private void gridSub_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_filling && e.RowIndex >= 0) _dirty = true;
        }

        /// <summary>대표 행은 Delete 키로도 지울 수 없다.</summary>
        private void gridSub_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (IsRepresentative(e.Row)) e.Cancel = true;
        }

        private void gridSub_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            _dirty = true;
        }

        /// <summary>[행 삭제] 선택한 행을 목록에서 뺀다(대표 / 새 입력 행 제외). DB 반영은 [저장] 때.</summary>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            gridSub.EndEdit();
            List<DataGridViewRow> rows = gridSub.SelectedRows.Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow && !IsRepresentative(r)).ToList();
            if (rows.Count == 0 && gridSub.CurrentRow != null && !gridSub.CurrentRow.IsNewRow && !IsRepresentative(gridSub.CurrentRow))
                rows.Add(gridSub.CurrentRow);
            foreach (DataGridViewRow row in rows) gridSub.Rows.Remove(row);
            if (rows.Count > 0)
            {
                _dirty = true;
                lblStatus.Text = rows.Count + "행을 뺐습니다. [저장]을 눌러야 DB에 반영됩니다.";
            }
        }

        /// <summary>
        /// [저장] 표의 Sub 전체를 서버에 보내 통째로 바꾼다.
        /// 처리 흐름: 편집 확정 -> 대표 / 빈 행 제외하고 수집 -> SaveSubDeviceList(서버 검사 후 삭제 + 일괄 등록) -> 목록 다시 읽기.
        /// 서버 검사 오류면 DB는 바뀌지 않고 표는 그대로 둔다.
        /// </summary>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            gridSub.EndEdit();
            var subs = new List<string>();
            var remarks = new List<string>();
            foreach (DataGridViewRow row in gridSub.Rows)
            {
                if (row.IsNewRow || IsRepresentative(row)) continue;
                string sub = Convert.ToString(row.Cells[colSubDevice.Index].Value).Trim();
                if (sub.Length == 0) continue;
                subs.Add(sub);
                remarks.Add(Convert.ToString(row.Cells[colRemark.Index].Value).Trim());
            }

            SetBusy(true, "저장 중");
            try
            {
                string factory = _factory, device = _device, user = Environment.UserName;
                DACrux.SEMDMS.RO.GdsMapImport service = ImportService ?? new DACrux.SEMDMS.RO.GdsMapImport();
                int count = await Task.Run(() => service.SaveSubDeviceList(factory, device, subs.ToArray(), remarks.ToArray(), user));
                Saved = true;
                SetBusy(false, string.Empty);
                await LoadListAsync();
                lblStatus.Text = "저장 완료: Sub Device " + count + "개";
            }
            catch (Exception ex)
            {
                SetBusy(false, "저장 실패: " + Cause(ex).Message);
                MessageBox.Show(this, Cause(ex).Message, "Sub Device 저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>저장하지 않은 변경이 있으면 닫기 전에 확인한다.</summary>
        private void SubDeviceSetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_busy) { e.Cancel = true; return; }
            if (_dirty && MessageBox.Show(this, "저장하지 않은 변경이 있습니다. 닫으시겠습니까?", Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void SetBusy(bool busy, string status)
        {
            _busy = busy;
            gridSub.Enabled = btnDeleteRow.Enabled = btnSave.Enabled = !busy;
            UseWaitCursor = busy;
            if (status.Length > 0) lblStatus.Text = status;
        }

        private static bool IsRepresentative(DataGridViewRow row)
        {
            return row != null && "REP".Equals(row.Tag);
        }

        /// <summary>원격 호출 예외는 안쪽 예외에 실제 사유가 있다.</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex;
        }
    }
}
