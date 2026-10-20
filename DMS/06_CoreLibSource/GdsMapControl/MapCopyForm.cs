using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map 복사 창(2026-10-15). 설계: 문서/설계/02_GDS/2026-10-15_GDS_Map_복사_설계.md 5절.
    /// 새 Device 이름을 받아 서버(GdsMapImport.CopyMap)가 Map / Chain 을 DB 안에서 복사한다.
    /// 중복 이름 / 원본 상태 검사는 서버가 하고, 실패하면 창을 닫지 않고 사유를 보여 다시 입력할 수 있게 한다.
    /// </summary>
    public partial class MapCopyForm : Form
    {
        private readonly string _factory;
        private readonly string _sourceDevice;
        private bool _busy;

        /// <summary>복사 서비스. null 이면 원격 호출 RO 를 만든다(Smoke 검사는 같은 프로세스 BSL 을 넣는다).</summary>
        public DACrux.SEMDMS.RO.GdsMapImport ImportService { get; set; }

        /// <summary>복사한 새 Device 이름(성공했을 때만).</summary>
        public string TargetDeviceId { get; private set; }

        /// <summary>복사한 도형 수.</summary>
        public long PlacedCount { get; private set; }

        /// <summary>복사한 Chain 수.</summary>
        public int ChainCount { get; private set; }

        public MapCopyForm(string factory, string sourceDevice)
        {
            InitializeComponent();
            _factory = factory;
            _sourceDevice = sourceDevice;
            txtSource.Text = sourceDevice;
        }

        private void txtTarget_TextChanged(object sender, EventArgs e)
        {
            if (!_busy) lblStatus.Text = string.Empty;
        }

        /// <summary>
        /// [복사] 처리 흐름: 이름 확인(빈 값 / 원본과 같음) -> CopyMap(서버가 중복 확인 후 복사, 실패하면 새 Device 정리) -> 창 닫기(OK).
        /// 서버 오류면 창을 닫지 않고 사유를 보인다.
        /// </summary>
        private async void btnCopy_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            string target = txtTarget.Text.Trim();
            if (target.Length == 0)
            {
                ShowError("새 Device 이름을 입력하세요.");
                return;
            }
            if (string.Equals(target, _sourceDevice, StringComparison.OrdinalIgnoreCase))
            {
                ShowError("원본과 다른 이름을 입력하세요.");
                return;
            }

            SetBusy(true);
            try
            {
                string factory = _factory, source = _sourceDevice, user = Environment.UserName;
                DACrux.SEMDMS.RO.GdsMapImport service = ImportService ?? new DACrux.SEMDMS.RO.GdsMapImport();
                DataTable result = await Task.Run(() => service.CopyMap(factory, source, target, user));
                DataRow row = result.Rows[0];
                TargetDeviceId = row["TARGET_DEVICE_ID"].ToString();
                PlacedCount = Convert.ToInt64(row["PLACED_COUNT"], CultureInfo.InvariantCulture);
                ChainCount = Convert.ToInt32(row["CHAIN_COUNT"], CultureInfo.InvariantCulture);
                SetBusy(false);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                SetBusy(false);
                ShowError(FirstLine(Cause(ex).Message));
            }
        }

        /// <summary>복사 중에는 닫지 않는다(서버 작업은 끝까지 진행된다).</summary>
        private void MapCopyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_busy) e.Cancel = true;
        }

        private BusyStatus _busyIndicator;
        private BusyStatus BusyIndicator
        {
            get { return _busyIndicator ?? (_busyIndicator = new BusyStatus(this, null)); }
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            txtTarget.Enabled = btnCopy.Enabled = btnCancel.Enabled = !busy;
            progressCopy.Visible = busy;
            if (busy)
            {
                lblStatus.ForeColor = SystemColors.ControlText;
                lblStatus.Text = "복사 중";
                BusyIndicator.Start();
            }
            else
                BusyIndicator.Stop();
        }

        private void ShowError(string message)
        {
            lblStatus.ForeColor = Color.Firebrick;
            lblStatus.Text = message;
            txtTarget.Focus();
            txtTarget.SelectAll();
        }

        /// <summary>원격 호출 예외는 안쪽 예외에 실제 사유가 있다.</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex;
        }

        /// <summary>ArgumentException 의 "Parameter name" 줄은 빼고 첫 줄만 보인다.</summary>
        private static string FirstLine(string message)
        {
            message = message ?? string.Empty;
            int end = message.IndexOfAny(new[] { '\r', '\n' });
            return end < 0 ? message : message.Substring(0, end);
        }
    }
}
