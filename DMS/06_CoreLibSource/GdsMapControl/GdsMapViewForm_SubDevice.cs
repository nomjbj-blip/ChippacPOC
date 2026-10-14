using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map 조회 화면의 대표 Device / Sub Device 처리 (2026-10-12).
    /// 설계: 문서/2026-10-12_GDS_대표Device_SubDevice_설계.md 5.1절.
    ///  - [Map ▾] > Sub Device 설정: 지금 Device(대표)의 Sub Device 를 수동 등록하는 창(SubDeviceSetupForm)을 연다.
    ///  - Map 정보(Device 콤보 툴팁): 대표의 Sub Device 이름을 함께 보여 준다.
    ///  - Device 입력: Map 이 없는 이름을 입력하고 Enter 하면 Sub 등록을 찾아 대표 Device Map 을 연다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>Sub -> 대표로 바꿔 연 경우 다음 상태 줄 앞에 붙일 안내. 한 번 쓰면 비운다.</summary>
        private string _subRedirectNote = string.Empty;

        private string TakeSubRedirectNote()
        {
            string note = _subRedirectNote;
            _subRedirectNote = string.Empty;
            return note.Length == 0 ? string.Empty : note + " / ";
        }

        /// <summary>
        /// Map 이 없는 Device 이름이면 Sub 등록(또는 대소문자만 다른 대표)을 찾아 대표 Device 로 바꿔 연다.
        /// 처리: GetMapDeviceBySub -> 대표가 있고 입력과 다르면 콤보를 대표로 바꾸고 Map 정보를 다시 읽는다. 바꿨으면 true.
        /// 서비스 오류(배포 전 Service 등)는 기존처럼 "Map 없음"으로 두도록 false 를 돌려준다.
        /// </summary>
        private async Task<bool> RedirectToRepresentativeAsync(string factory, string device)
        {
            string representative;
            try
            {
                DataTable found = await Task.Run(() => new DACrux.SEMDMS.RO.GdsMapView().GetMapDeviceBySub(factory, device));
                if (found.Rows.Count == 0) return false;
                representative = found.Rows[0]["DEVICE_ID"].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Sub Device 찾기 실패: {0}", ex.Message);
                return false;
            }
            if (device != cboDevice.Text.Trim() || string.Equals(representative, device, StringComparison.Ordinal)) return false;

            _subRedirectNote = "Sub Device " + device + " -> 대표 " + representative;
            int index = cboDevice.Items.IndexOf(representative);
            if (index >= 0 && cboDevice.SelectedIndex != index)
            {
                // 선택 변경 이벤트가 Map 정보를 다시 읽는다(미저장 Chain 확인 포함).
                cboDevice.SelectedIndex = index;
            }
            else
            {
                cboDevice.Text = representative;
                LoadMapInfo();
            }
            return true;
        }

        /// <summary>대표 Map 의 Sub Device 이름을 읽어 Map 정보에 붙인다. 실패하면 Map 정보만 그대로 둔다.</summary>
        private async Task LoadSubDeviceTextAsync(MapItem item)
        {
            try
            {
                string factory = item.Factory, device = item.DeviceId;
                DataTable list = await Task.Run(() => new DACrux.SEMDMS.RO.GdsMapView().GetSubDeviceList(factory, device));
                item.SubDevices = string.Join(", ", list.Rows.Cast<DataRow>().Select(r => r["SUB_DEVICE_ID"].ToString()).ToArray());
                if (ReferenceEquals(item, _currentMap) || ReferenceEquals(item, _shownMap)) UpdateMapInfoText();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Sub Device 목록 조회 실패: {0}", ex.Message);
            }
        }

        /// <summary>[Map ▾] > Sub Device 설정: 지금 Device(대표)의 Sub Device 설정 창을 연다. 저장했으면 Map 정보를 새로 표시한다.</summary>
        private void mnuSubDevice_Click(object sender, EventArgs e)
        {
            MapItem item = _currentMap;
            if (item == null)
            {
                MessageBox.Show(this, "GDS Map 이 있는 Device 를 먼저 선택하세요.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var dialog = new SubDeviceSetupForm(item.Factory, item.DeviceId))
            {
                dialog.ShowDialog(this);
                if (!dialog.Saved) return;
                item.SubDevices = dialog.SubDeviceText;
                UpdateMapInfoText();
                SetStatus("Sub Device 저장: " + item.DeviceId + " <- " + (dialog.SubDeviceText.Length == 0 ? "(없음)" : dialog.SubDeviceText));
            }
        }
    }
}
