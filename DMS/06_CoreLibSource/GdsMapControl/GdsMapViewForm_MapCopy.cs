using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map 조회 화면의 [Map 복사] (2026-10-15). 설계: 문서/설계/02_GDS/2026-10-15_GDS_Map_복사_설계.md 3절 C4 / C6 / C7, 5절.
    /// 지금 고른 Device 의 저장된 Map / Chain 을 새 Device 이름으로 복사한다. 복사는 서버가 DB 안에서 하고(GdsMapImport.CopyMap),
    /// 이 화면은 미저장 Chain 확인 / 복사 창 / Device 목록 갱신만 한다. 버튼은 DMS 호스트 모드에서만 보인다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>
        /// [Map 복사] 처리 흐름: 저장하지 않은 Chain 변경 확인(DB 에 저장된 내용만 복사) -> 복사 창(새 이름 입력, 서버 복사)
        /// -> Device 목록 다시 읽기(지금 화면 / 도면 / Chain 은 그대로) -> 상태 줄에 결과 한 줄.
        /// </summary>
        private async void btnCopyMap_Click(object sender, EventArgs e)
        {
            MapItem source = _currentMap;
            if (source == null || _loadCancel != null || _chainDbSaving) return;
            bool shown = _shownMap != null && _shownMap.DeviceId == source.DeviceId;
            int unsaved = shown ? CountUnsavedChains() : 0;
            if (unsaved > 0 && MessageBox.Show(this,
                    "저장하지 않은 Chain 변경 " + unsaved + "건은 복사되지 않습니다.\r\nDB 에 저장된 내용만 복사하시겠습니까?",
                    "GDS Map 복사", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            string done;
            using (var dialog = new MapCopyForm(source.Factory, source.DeviceId))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                done = "Map 복사 완료: " + source.DeviceId + " -> " + dialog.TargetDeviceId
                    + " / 도형 " + dialog.PlacedCount.ToString("N0") + " / Chain " + dialog.ChainCount + "개";
            }
            SetStatus(done);
            try
            {
                using (ViewBusy.Busy())
                    _deviceTable = await Task.Run(() => new DACrux.SEMDMS.RO.GdsMapView().GetMapDeviceList());
                RefreshDeviceItems();
            }
            catch (Exception ex)
            {
                ShowError("Device 목록 조회", ex);
            }
        }

        /// <summary>
        /// Device 목록만 새로 채운다. 지금 고른 Device / 도면 / Chain 은 그대로 둔다.
        /// 목록을 바꾸는 동안 콤보 선택 이벤트가 Map 정보를 다시 읽지 않도록 _restoringCondition 을 켠다.
        /// </summary>
        private void RefreshDeviceItems()
        {
            string current = cboDevice.Text;
            _restoringCondition = true;
            cboDevice.BeginUpdate();
            try
            {
                cboDevice.Items.Clear();
                foreach (DataRow row in _deviceTable.Rows)
                    if (string.Equals(row["FACTORY"].ToString(), _factory, StringComparison.Ordinal))
                        cboDevice.Items.Add(row["DEVICE_ID"].ToString());
                int index = cboDevice.Items.IndexOf(current);
                if (index >= 0) cboDevice.SelectedIndex = index;
                else cboDevice.Text = current;
            }
            finally
            {
                cboDevice.EndUpdate();
                _restoringCondition = false;
            }
        }
    }
}
