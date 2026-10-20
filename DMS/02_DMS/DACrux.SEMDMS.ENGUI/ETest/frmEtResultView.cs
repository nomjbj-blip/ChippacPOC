using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 결과 조회 화면이다 (메뉴 화면). 2026-10-15 Upload / View 분리로 업로드 화면 [등록 파일] 탭과 팝업 조회 창을 합쳐 다시 작성했다.
    /// 설계: 문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md 5절
    /// 처리 흐름:
    ///  1) Factory / Device / 공정(전체 또는 하나) / Lot 글자로 [조회] -> 저장된 파일 목록 (GetEtFileBrowseList).
    ///     목록 열: 추가 Para(지금 등록 Parameter 중 이 파일에 없는 수 - 다시 업로드해야 보임, 노란색) / Spec 바뀜(판정 뒤 Spec 수정, 주황색) / Map / YMS 등.
    ///  2) 파일을 고르면 파일 / Die / 측정값 / 등록 Parameter(지금 Spec)를 읽어 아래 공통 결과 Control(EtResultDataControl, 업로드 화면과 같은 모양)에 보인다.
    ///     판정 색은 저장된 판정(저장 당시 Spec 기준), Lower / Upper 는 지금 Spec 이다. Spec 이 판정 뒤 바뀌었으면 정보 표에 안내한다.
    ///  3) [선택 파일 삭제]: 파일 / Die / 측정값과 YMS 에 올린 데이터를 함께 지운다(등록 Parameter 는 남음).
    ///  [Map 좌표 다시 계산] 은 2026-10-15 삭제: 업로드 때 Map(Map ID = Device)이 없으면 저장하지 않으므로, Map 을 고쳤으면 파일을 다시 업로드한다.
    /// 서버 조회는 아래 busyBar 가 별도 스레드에서 돌린다.
    /// </summary>
    public partial class frmEtResultView : DACrux.Framework.Base.DACruxUXBasic01
    {
        private const string AllProgram = "(전체)";

        private RO.GdsEtResult viewService;
        /// <summary>지금 결과 Control 에 보이는 파일 번호 (같은 파일을 다시 읽지 않게)</summary>
        private string shownSeq;
        /// <summary>목록을 채우는 중(선택 변경 이벤트 무시)</summary>
        private bool filling;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtResultView()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지
            cboProgram.Items.Add(AllProgram);
            cboProgram.SelectedIndex = 0;
            UpdateButtons();
        }

        /// <summary>조회 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return viewService ?? (viewService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 와 Device 목록을 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadDeviceList();
        }

        #region [조건]

        private void txtFactory_Leave(object sender, EventArgs e)
        {
            LoadDeviceList();
        }

        /// <summary>Device 목록 (등록 Parameter 가 있는 Device). 직접 입력도 된다.</summary>
        private void LoadDeviceList()
        {
            string factory = txtFactory.Text.Trim();
            string current = cboDevice.Text;
            if (factory.Length == 0) return;
            try
            {
                DataTable dt = busyBar.RunHere("Device 목록", () => Service.GetEtDeviceList(factory));
                cboDevice.BeginUpdate();
                cboDevice.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                    cboDevice.Items.Add(dr["DEVICE_ID"].ToString());
                cboDevice.EndUpdate();
            }
            catch (Exception ex)
            {
                SetStatus("Device 목록 조회 실패(직접 입력 가능): " + Cause(ex).Message);
            }
            cboDevice.Text = current;
        }

        /// <summary>Device 를 고르면 공정 목록을 다시 채우고 바로 조회한다.</summary>
        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevice.SelectedIndex < 0) return;
            LoadProgramList();
            Search();
        }

        /// <summary>공정 목록: (전체) + Device 의 공정 (GetEtTestProgramList). 고른 공정이 있으면 유지한다.</summary>
        private void LoadProgramList()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            string current = cboProgram.SelectedItem as string;
            cboProgram.BeginUpdate();
            cboProgram.Items.Clear();
            cboProgram.Items.Add(AllProgram);
            try
            {
                if (factory.Length > 0 && device.Length > 0)
                {
                    DataTable dt = busyBar.RunHere("공정 목록", () => Service.GetEtTestProgramList(factory, device));
                    foreach (DataRow dr in dt.Rows)
                        cboProgram.Items.Add(dr["TEST_PROGRAM"].ToString());
                }
            }
            catch (Exception ex)
            {
                SetStatus("공정 목록 조회 실패: " + Cause(ex).Message);
            }
            finally
            {
                cboProgram.EndUpdate();
            }
            int index = current == null ? 0 : cboProgram.Items.IndexOf(current);
            cboProgram.SelectedIndex = index < 0 ? 0 : index;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void txtLot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            Search();
        }

        #endregion

        #region [파일 목록]

        /// <summary>목록 열 제목 (열 이름은 DB 컬럼 그대로 두고 제목만 바꾼다)</summary>
        private static readonly Dictionary<string, string> FileListHeaders = new Dictionary<string, string>
        {
            { "SRC_FILE_SEQ", "파일 번호" }, { "WAFER_ID", "WAFER_ID" }, { "LOT_ID", "Lot" }, { "SLOT_NO", "Slot" }, { "TEST_PROGRAM", "공정" },
            { "TEST_TIME", "Test Date" }, { "DIE_COUNT", "Die" }, { "PASS_DIE_COUNT", "Spec Pass Die" }, { "FAIL_DIE_COUNT", "Spec Fail Die" },
            { "ITEM_COUNT", "저장 Para" }, { "VALUE_COUNT", "값" }, { "ADDED_PARA_COUNT", "추가 Para" }, { "SPEC_CHANGED", "Spec 바뀜" },
            { "MAP_ID", "Map" }, { "MAP_DIE_COUNT", "Map 좌표 Die" }, { "JUDGE_TIME", "판정 시각" }, { "YMS_STATUS", "YMS" }, { "YMS_TIME", "YMS 시각" },
            { "YMS_MESSAGE", "YMS 메시지" }, { "LOAD_STATUS", "상태" }, { "CREATE_USER", "등록자" }, { "CREATE_TIME", "등록 시각" }, { "FILE_NAME", "파일명" }
        };

        /// <summary>조건으로 파일 목록을 조회하고 첫 파일의 결과를 보인다. 고른 파일이 목록에 남아 있으면 그 파일을 다시 고른다.</summary>
        private void Search()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("Factory / Device 를 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            string program = cboProgram.SelectedItem as string;
            if (program == null || program == AllProgram) program = string.Empty;
            string lot = txtLot.Text.Trim();
            string keepSeq = SelectedSeq();
            try
            {
                DataTable dt = busyBar.Run("파일 목록 조회", () => Service.GetEtFileBrowseList(factory, device, program, lot));
                filling = true;
                dgvFiles.DataSource = dt;
                foreach (DataGridViewColumn column in dgvFiles.Columns)
                {
                    string header;
                    if (FileListHeaders.TryGetValue(column.Name, out header)) column.HeaderText = header;
                }
                if (dgvFiles.Columns.Contains("DEVICE_ID")) dgvFiles.Columns["DEVICE_ID"].Visible = false;
                foreach (string name in new[] { "JUDGE_TIME", "YMS_TIME", "CREATE_TIME" })
                    if (dgvFiles.Columns.Contains(name)) dgvFiles.Columns[name].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                dgvFiles.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

                int fail = dt.Rows.Cast<DataRow>().Count(r => Long(r["FAIL_DIE_COUNT"]) > 0);
                int added = dt.Rows.Cast<DataRow>().Count(r => Long(r["ADDED_PARA_COUNT"]) > 0);
                int changed = dt.Rows.Cast<DataRow>().Count(r => r["SPEC_CHANGED"].ToString() == "Y");
                int noMap = dt.Rows.Cast<DataRow>().Count(r => r["MAP_ID"] == DBNull.Value || r["MAP_ID"].ToString().Length == 0);
                SetStatus("Device " + device + (program.Length > 0 ? " / 공정 " + program : string.Empty) + (lot.Length > 0 ? " / Lot " + lot : string.Empty)
                    + " / 파일 " + dt.Rows.Count + " / Spec Fail 있는 파일 " + fail + " / 추가 Para 있는 파일 " + added + " / Spec 바뀐 파일 " + changed + " / Map 없는 파일 " + noMap);

                dgvFiles.ClearSelection();
                DataGridViewRow select = dgvFiles.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => RowSeq(r) == keepSeq) ?? dgvFiles.Rows.Cast<DataGridViewRow>().FirstOrDefault();
                shownSeq = null;
                if (select != null)
                {
                    dgvFiles.CurrentCell = select.Cells[dgvFiles.Columns.Cast<DataGridViewColumn>().First(c => c.Visible).Index];
                    select.Selected = true;
                }
            }
            catch (Exception ex)
            {
                dgvFiles.DataSource = null;
                SetStatus("파일 목록 조회 실패: " + Cause(ex).Message);
                Notify("파일 목록을 조회하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                filling = false;
                UpdateButtons();
            }
            ShowSelected();
        }

        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            if (filling) return;
            ShowSelected();
            UpdateButtons();
        }

        /// <summary>고른 파일의 저장 결과를 공통 결과 Control 에 보인다(파일 / Die / 측정값 / 등록 Parameter 를 한 번에 읽는다).</summary>
        private void ShowSelected()
        {
            DataRow listRow = SelectedRow();
            if (listRow == null)
            {
                shownSeq = null;
                resultView.Clear();
                return;
            }
            string seq = listRow["SRC_FILE_SEQ"].ToString();
            if (seq == shownSeq) return;
            try
            {
                DataTable[] data = busyBar.Run("결과 조회 (파일 번호 " + seq + ")", () => FetchData(seq));
                resultView.ShowData(EtResultData.FromDb(data[0].Rows[0], data[1], data[2], data[3]));
                shownSeq = seq;
            }
            catch (Exception ex)
            {
                shownSeq = null;
                resultView.Clear();
                SetStatus("결과 조회 실패: " + Cause(ex).Message);
                Notify("결과를 조회하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 파일 / Die / 측정값 / 등록 Parameter(지금 Spec) 를 서버에서 읽는다(작업 스레드, 화면 컨트롤을 만지지 않는다).
        /// 등록 Parameter 를 못 읽어도 저장값 / 판정은 보여 주므로 넷째 표는 null 이 될 수 있다.
        /// </summary>
        private DataTable[] FetchData(string seq)
        {
            DataTable file = Service.GetEtFile(seq);
            if (file.Rows.Count == 0)
                throw new InvalidOperationException("파일 번호 " + seq + " 가 없습니다(삭제되었을 수 있습니다).");
            DataRow f = file.Rows[0];
            DataTable dies = Service.GetEtDieList(seq);
            DataTable values = Service.GetEtValueList(seq);
            DataTable para = null;
            try
            {
                para = Service.GetEtParameterList(f["FACTORY"].ToString(), f["DEVICE_ID"].ToString(), f["TEST_PROGRAM"].ToString());
            }
            catch (Exception)
            {
                // Spec 을 못 읽어도 저장값 / 판정은 보여 준다
            }
            return new DataTable[] { file, dies, values, para };
        }

        /// <summary>Test Date 형식 / Spec Fail Die 빨간색 / 추가 Para 노란 배경 / Spec 바뀜 주황색 / Map 없음 회색</summary>
        private void dgvFiles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string name = dgvFiles.Columns[e.ColumnIndex].Name;
            if (name == "TEST_TIME" && e.Value is string)
            {
                e.Value = EtResultData.FormatTestTime((string)e.Value);
                e.FormattingApplied = true;
            }
            else if (name == "FAIL_DIE_COUNT" && Long(e.Value) > 0)
                e.CellStyle.ForeColor = Color.Firebrick;
            else if (name == "ADDED_PARA_COUNT" && Long(e.Value) > 0)
                e.CellStyle.BackColor = Color.LightYellow;
            else if (name == "SPEC_CHANGED" && Convert.ToString(e.Value) == "Y")
                e.CellStyle.ForeColor = Color.DarkOrange;
            else if (name == "MAP_ID" && (e.Value == null || e.Value == DBNull.Value))
            {
                e.Value = "없음";
                e.CellStyle.ForeColor = Color.Gray;
                e.FormattingApplied = true;
            }
        }

        #endregion

        #region [삭제]

        /// <summary>목록에서 고른 파일을 삭제한다(Die / 측정값 / YMS 데이터 포함, 등록 Parameter 는 남음).</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow dr = SelectedRow();
            if (dr == null)
            {
                Notify("삭제할 파일을 목록에서 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            string seq = dr["SRC_FILE_SEQ"].ToString();
            if (Ask("E-Test 파일을 삭제할까요?\r\n\r\nWAFER_ID " + dr["WAFER_ID"] + " / 공정 " + dr["TEST_PROGRAM"] + " / Test Date " + EtResultData.FormatTestTime(dr["TEST_TIME"].ToString())
                + " / 파일 번호 " + seq + "\r\nDie / 측정값과 YMS 에 올린 데이터도 함께 삭제됩니다.", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            string user = User();
            try
            {
                int values = busyBar.Run("파일 삭제", () => Service.DeleteEtFile(seq, user));
                SetStatus("삭제 완료 / 파일 번호 " + seq + " / 측정값 " + values.ToString("N0", CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Notify("삭제하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            Search();
        }

        #endregion

        #region [공통]

        private DataRow SelectedRow()
        {
            DataGridViewRow row = dgvFiles.SelectedRows.Count == 1 ? dgvFiles.SelectedRows[0] : null;
            DataRowView view = row == null ? null : row.DataBoundItem as DataRowView;
            return view == null ? null : view.Row;
        }

        private string SelectedSeq()
        {
            DataRow dr = SelectedRow();
            return dr == null ? null : dr["SRC_FILE_SEQ"].ToString();
        }

        private static string RowSeq(DataGridViewRow row)
        {
            DataRowView view = row.DataBoundItem as DataRowView;
            return view == null ? null : view.Row["SRC_FILE_SEQ"].ToString();
        }

        private void UpdateButtons()
        {
            btnDelete.Enabled = SelectedRow() != null;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        private static long Long(object value)
        {
            long result;
            return value == null || value == DBNull.Value || !long.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? 0 : result;
        }

        private static string User()
        {
            string user = DACrux.Base.GlobalVariable.UserID;
            return string.IsNullOrEmpty(user) ? Environment.UserName : user;
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>확인창. 화면 Smoke 검사는 정해진 답을 돌려주도록 바꾼다.</summary>
        protected virtual DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, text, Text, buttons, MessageBoxIcon.Question);
        }

        /// <summary>안내창. 화면 Smoke 검사는 창을 띄우지 않고 기록만 한다.</summary>
        protected virtual void Notify(string text, MessageBoxIcon icon)
        {
            MessageBox.Show(this, text, Text, MessageBoxButtons.OK, icon);
        }

        #endregion
    }
}
