using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test Para Limit 설정 화면이다. (CLAUDE-029)
    /// 설계: 문서/2026-10-07_ETest_Limit_설정_설계.md 10절
    /// 2026-10-07 사용자 결정: Para 별로 사용자가 구간(Lower / Upper)을 고른다 / Device 별 설정 / 등록 시 Para 대문자 / Enable 기능 / Limit 은 한쪽만 있어도 된다.
    /// 처리 흐름:
    ///  1) Factory / Device 를 고르고 [조회]: 측정 항목의 Para(대문자 묶음)별 항목 수 / 업로드 값 통계 / 저장된 Limit 을 보여 준다.
    ///  2) Lower / Upper / Enable / 비고를 고치면 그 행이 노란색이 된다(잘못된 숫자 / Lower > Upper 는 붉은색).
    ///     Limit 이 없던 Para 에 값을 넣으면 Enable 을 자동으로 체크한다.
    ///  3) [저장]: 바뀐 행만 SaveEtLimit 으로 저장한다. 저장된 Limit 의 Lower / Upper 를 모두 지운 행은 삭제할지 묻는다.
    ///  4) [선택 Limit 삭제]: 고른 Para 의 Limit 을 지운다(판정 안 함으로 돌아감).
    /// 범위 밖 수는 저장된 Enable Limit 기준이며 [저장] 후 다시 조회할 때 갱신된다.
    /// </summary>
    public partial class frmEtLimitSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private static readonly Color DirtyColor = Color.LightYellow;
        private static readonly Color ErrorColor = Color.MistyRose;

        private RO.GdsEtResult limitService;
        private string loadedFactory = string.Empty;
        private string loadedDevice = string.Empty;
        private bool filling;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtLimitSetup()
        {
            InitializeComponent();
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colItems, colValues, colNotTested, colMin, colMedian, colP99, colMax, colLower, colUpper, colOut })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colLower, colUpper, colEnable, colRemark })
                column.HeaderCell.Style.Font = new Font(dgvLimit.Font, FontStyle.Bold);
            UpdateButtons();
        }

        /// <summary>Limit 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return limitService ?? (limitService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 와 Device 목록을 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadDeviceList();
        }

        #region [조회]

        /// <summary>측정 항목이 있는 Device 를 콤보에 채운다. 직접 입력도 된다.</summary>
        private void LoadDeviceList()
        {
            string current = cboDevice.Text;
            string factory = txtFactory.Text.Trim();
            if (factory.Length == 0) return;
            try
            {
                DataTable dt = Service.GetEtDeviceList(factory);
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

        private void txtFactory_Leave(object sender, EventArgs e)
        {
            LoadDeviceList();
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevice.SelectedIndex >= 0) SearchLimits();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchLimits();
        }

        /// <summary>저장 안 된 변경이 있으면 묻고 Para 목록을 다시 조회한다.</summary>
        private void SearchLimits()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("Factory / Device 를 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            if (DirtyRows().Count > 0 && Ask("저장하지 않은 변경이 있습니다. 버리고 다시 조회할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            LoadLimits(factory, device);
        }

        /// <summary>Para 별 Limit / 통계를 표에 채운다. 각 행 Tag 에 조회 당시 값(원본)을 넣어 변경 여부를 비교한다.</summary>
        private void LoadLimits(string factory, string device)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = Service.GetEtLimitList(factory, device);
                filling = true;
                dgvLimit.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    bool hasLimit = dr["HAS_LIMIT"].ToString() == "Y";
                    string lower = Number(dr["LOWER_LIMIT"], "R");
                    string upper = Number(dr["UPPER_LIMIT"], "R");
                    bool enable = hasLimit && dr["ENABLE_FLAG"].ToString() == "Y";
                    string remark = dr["REMARK"].ToString();
                    int index = dgvLimit.Rows.Add(dr["PARA_NAME"].ToString(), dr["UNIT"].ToString(), Number(dr["ITEM_COUNT"], "N0"),
                        Number(dr["VALUE_COUNT"], "N0"), Number(dr["NOT_TESTED_COUNT"], "N0"),
                        Number(dr["MIN_VALUE"], "G6"), Number(dr["MEDIAN_VALUE"], "G6"), Number(dr["P99_VALUE"], "G6"), Number(dr["MAX_VALUE"], "G6"),
                        lower, upper, enable, hasLimit ? Number(dr["OUT_COUNT"], "N0") : string.Empty, remark,
                        hasLimit ? dr["UPDATE_USER"] + " " + FormatDate(dr["UPDATE_TIME"]) : string.Empty);
                    DataGridViewRow row = dgvLimit.Rows[index];
                    row.Tag = new LimitState { HasLimit = hasLimit, Lower = lower, Upper = upper, Enable = enable, Remark = remark };
                    if (dr["UNIT_KINDS"].ToString() != "0" && dr["UNIT_KINDS"].ToString() != "1")
                        row.Cells[colUnit.Index].Style.BackColor = ErrorColor;
                    if (hasLimit && Number(dr["OUT_COUNT"], "N0") != "0")
                        row.Cells[colOut.Index].Style.ForeColor = Color.Firebrick;
                }
                loadedFactory = factory;
                loadedDevice = device;
                int limits = 0;
                foreach (DataGridViewRow row in dgvLimit.Rows) if (((LimitState)row.Tag).HasLimit) limits++;
                SetStatus(string.Format("조회 완료 / Factory {0} / Device {1} / Para {2} / 저장된 Limit {3}", factory, device, dgvLimit.Rows.Count, limits));
            }
            catch (Exception ex)
            {
                SetStatus("조회 실패 / " + Cause(ex).Message);
                Notify("Limit 을 조회하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                filling = false;
                Cursor = Cursors.Default;
                UpdateButtons();
            }
        }

        #endregion

        #region [편집 표시]

        /// <summary>체크박스는 클릭 즉시 값이 반영되도록 바로 확정한다.</summary>
        private void dgvLimit_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLimit.IsCurrentCellDirty && dgvLimit.CurrentCell is DataGridViewCheckBoxCell)
                dgvLimit.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>값이 바뀌면 그 행의 색을 다시 정한다. Limit 이 없던 Para 에 처음 값을 넣으면 Enable 을 켠다.</summary>
        private void dgvLimit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (filling || e.RowIndex < 0) return;
            DataGridViewRow row = dgvLimit.Rows[e.RowIndex];
            LimitState state = (LimitState)row.Tag;
            if ((e.ColumnIndex == colLower.Index || e.ColumnIndex == colUpper.Index) && !state.HasLimit && !state.EnableTouched
                && (CellText(row, colLower).Length > 0 || CellText(row, colUpper).Length > 0))
            {
                filling = true;
                row.Cells[colEnable.Index].Value = true;
                filling = false;
            }
            if (e.ColumnIndex == colEnable.Index) state.EnableTouched = true;
            PaintRow(row);
            UpdateButtons();
        }

        /// <summary>바뀐 행은 노란색, 잘못된 값은 붉은색.</summary>
        private void PaintRow(DataGridViewRow row)
        {
            string error = RowError(row);
            Color back = error.Length > 0 ? ErrorColor : IsDirty(row) ? DirtyColor : Color.Empty;
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colLower, colUpper, colEnable, colRemark })
                row.Cells[column.Index].Style.BackColor = back;
            row.Cells[colPara.Index].ToolTipText = error;
        }

        private bool IsDirty(DataGridViewRow row)
        {
            LimitState s = (LimitState)row.Tag;
            return CellText(row, colLower) != s.Lower || CellText(row, colUpper) != s.Upper
                || CellChecked(row) != s.Enable || CellText(row, colRemark) != s.Remark;
        }

        /// <summary>행 입력 오류 문구. 없으면 빈 문자열. (두 칸 모두 빈 값은 "삭제" 또는 "저장 안 함" 이라 오류가 아니다.)</summary>
        private string RowError(DataGridViewRow row)
        {
            string lowerText = CellText(row, colLower);
            string upperText = CellText(row, colUpper);
            double lower = 0, upper = 0;
            if (lowerText.Length > 0 && !TryNumber(lowerText, out lower)) return "Lower 가 숫자가 아닙니다.";
            if (upperText.Length > 0 && !TryNumber(upperText, out upper)) return "Upper 가 숫자가 아닙니다.";
            if (lowerText.Length > 0 && upperText.Length > 0 && lower > upper) return "Lower 가 Upper 보다 큽니다.";
            if (lowerText.Length == 0 && upperText.Length == 0 && !((LimitState)row.Tag).HasLimit && (CellChecked(row) || CellText(row, colRemark).Length > 0))
                return "Lower / Upper 중 하나는 입력해야 합니다.";
            return string.Empty;
        }

        private List<DataGridViewRow> DirtyRows()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvLimit.Rows)
                if (row.Tag is LimitState && IsDirty(row)) rows.Add(row);
            return rows;
        }

        #endregion

        #region [저장 / 삭제]

        /// <summary>
        /// 바뀐 행만 저장한다.
        ///  - Lower / Upper 가 하나라도 있는 행: SaveEtLimit (MERGE)
        ///  - 저장된 Limit 의 Lower / Upper 를 모두 지운 행: 삭제할지 묻고 DeleteEtLimit
        ///  - Limit 이 없던 Para 에 비고 / Enable 만 바꾼 행: 오류(한쪽 Limit 필요)
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvLimit.IsCurrentCellInEditMode) dgvLimit.EndEdit();
            List<DataGridViewRow> dirty = DirtyRows();
            if (dirty.Count == 0)
            {
                Notify("저장할 변경이 없습니다.", MessageBoxIcon.Information);
                return;
            }
            List<string> errors = new List<string>();
            List<DataGridViewRow> saves = new List<DataGridViewRow>();
            List<DataGridViewRow> deletes = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dirty)
            {
                string error = RowError(row);
                if (error.Length > 0) { errors.Add(CellText(row, colPara) + ": " + error); continue; }
                if (CellText(row, colLower).Length == 0 && CellText(row, colUpper).Length == 0)
                {
                    if (((LimitState)row.Tag).HasLimit) deletes.Add(row);
                }
                else saves.Add(row);
            }
            if (errors.Count > 0)
            {
                Notify("입력을 확인하세요.\r\n\r\n" + string.Join("\r\n", errors.ToArray()), MessageBoxIcon.Warning);
                return;
            }
            if (deletes.Count > 0)
            {
                List<string> names = new List<string>();
                foreach (DataGridViewRow row in deletes) names.Add(CellText(row, colPara));
                if (Ask("Lower / Upper 를 모두 지운 Para 는 Limit 이 삭제됩니다(판정 안 함).\r\n\r\n" + string.Join(", ", names.ToArray()) + "\r\n\r\n계속할까요?",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            string user = User();
            try
            {
                Cursor = Cursors.WaitCursor;
                if (saves.Count > 0)
                {
                    string[,] rows = new string[saves.Count, 5];
                    for (int i = 0; i < saves.Count; i++)
                    {
                        rows[i, 0] = CellText(saves[i], colPara);
                        rows[i, 1] = CellText(saves[i], colLower);
                        rows[i, 2] = CellText(saves[i], colUpper);
                        rows[i, 3] = CellChecked(saves[i]) ? "Y" : "N";
                        rows[i, 4] = CellText(saves[i], colRemark);
                    }
                    Service.SaveEtLimit(new string[] { loadedFactory, loadedDevice }, rows, user);
                }
                foreach (DataGridViewRow row in deletes)
                    Service.DeleteEtLimit(new string[] { loadedFactory, loadedDevice, CellText(row, colPara) }, user);
                LoadLimits(loadedFactory, loadedDevice);
                SetStatus(string.Format("저장 완료 / 저장 {0} / 삭제 {1} / Factory {2} / Device {3}", saves.Count, deletes.Count, loadedFactory, loadedDevice));
            }
            catch (Exception ex)
            {
                SetStatus("저장 실패 / " + Cause(ex).Message);
                Notify("Limit 을 저장하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>고른 Para 의 저장된 Limit 을 삭제한다.</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvLimit.CurrentRow;
            if (row == null || !((LimitState)row.Tag).HasLimit)
            {
                Notify("저장된 Limit 이 있는 Para 를 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            string para = CellText(row, colPara);
            if (Ask("Para " + para + " 의 Limit 을 삭제할까요? (판정 안 함으로 돌아갑니다)"
                + (DirtyRows().Count > 0 ? "\r\n저장하지 않은 다른 변경은 버려집니다." : string.Empty), MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                Service.DeleteEtLimit(new string[] { loadedFactory, loadedDevice, para }, User());
                LoadLimits(loadedFactory, loadedDevice);
                SetStatus("삭제 완료 / Para " + para);
            }
            catch (Exception ex)
            {
                Notify("삭제하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region [공통]

        /// <summary>조회 당시 Limit 값(변경 비교용)</summary>
        private sealed class LimitState
        {
            public bool HasLimit;
            public string Lower;
            public string Upper;
            public bool Enable;
            public string Remark;
            /// <summary>사용자가 Enable 을 직접 바꿨으면 자동 체크하지 않는다.</summary>
            public bool EnableTouched;
        }

        private void UpdateButtons()
        {
            bool loaded = dgvLimit.Rows.Count > 0;
            btnSave.Enabled = loaded;
            btnDelete.Enabled = loaded;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        private static string User()
        {
            string user = DACrux.Base.GlobalVariable.UserID;
            return string.IsNullOrEmpty(user) ? Environment.UserName : user;
        }

        private static string CellText(DataGridViewRow row, DataGridViewColumn column)
        {
            return Convert.ToString(row.Cells[column.Index].Value, CultureInfo.InvariantCulture).Trim();
        }

        private bool CellChecked(DataGridViewRow row)
        {
            object value = row.Cells[colEnable.Index].Value;
            return value is bool && (bool)value;
        }

        private static bool TryNumber(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>DB 숫자(NULL 이면 빈 문자열)를 형식에 맞춰 문자열로 바꾼다.</summary>
        private static string Number(object value, string format)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            double number = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            return number.ToString(format, CultureInfo.InvariantCulture);
        }

        private static string FormatDate(object value)
        {
            return value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value);
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
