using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DACrux.Base;
using DACrux.Utility;
using FarPoint.Win.Spread;
using DACrux.Framework.Base;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPcmEquipStatus : DACrux.Framework.Base.DACruxUXBasic01, IExportExcel
    {
        #region 멤버변수

        public static readonly string REFRESH_TIME = "REFRESH_TIME";

        private int _interval;
        private Action<int> _updateIntervalDelegate;

        enum Col
        {
            EQUIP_ID,
            DEVICE,
            PROGRAM,
            LOT_ID,
            WAFER_ID,
            STATUS,
            LAST_STATUS_TIME,
            OPERATOR,
            PROBE_CARD,
            PROBE_CARD_USAGE,
            END_PGM,
            END_WAFER,
            END_TIME,
            TEST_TIME,
            FLAG
        }

        enum HisCol
        {
            EQUIP_ID,
            INPUT_TIME,
            PROBE_CARD,
            OPERATOR
        }

        enum Status
        {
            IDLE, RUN, OFFLINE
        }

        #endregion

        #region 생성자 및 Load/Closing 이벤트

        public frmPcmEquipStatus()
        {
            InitializeComponent();

            FPSpreadUtil.InitSpread(fpSpread1, fpSpread2);
            //fpSpread1_Sheet1.OperationMode = OperationMode.ReadOnly;
            pnlDataInput.Enabled = false;
        }

        private void frmPcmEquipStatus_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            SettingData settingData = new SettingData(Name);
            numRefresh.Value = settingData.GetValue<int>(REFRESH_TIME, 1);
            SetTimer();

            _updateIntervalDelegate = new Action<int>(UpdateInterval);
            System.Threading.ThreadPool.QueueUserWorkItem(RefreshInterval_async);

            dtStart.Value = DateTime.Now.AddDays(-1);
            dtEnd.Value = DateTime.Now;
        }

        private void frmPcmEquipStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingData settingData = new SettingData(Name);
            settingData.SetValue(REFRESH_TIME, numRefresh.Value);
            settingData.Save();
        }

        #endregion

        #region 사용자 정의 메서드

        private void RefreshInterval_async(object state)
        {
            while (!IsDisposed)
            {
                UpdateInterval(Math.Max(_interval--, 0));

                //if (_interval < 0)
                //    _interval = timer1.Interval / 1000;

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void UpdateInterval(int second)
        {
            if (InvokeRequired)
                BeginInvoke(_updateIntervalDelegate, second);
            else
                lblInterval.Text = String.Format("{0:00}:{1:00}", (int)(second / 60), second % 60);
        }

        private void ClearInputControl()
        {
            txtEquipID.Clear();
            txtProbeCard.Clear();
        }

        private void SearchAll(bool async = true)
        {
            if (!Visible)
                return;

            SearchEquipStatus(async);

            if (pnlDataInput.Enabled && !String.IsNullOrEmpty(txtEquipID.Text))
                SearchDataInputHis(txtEquipID.Text, async);
        }

        private void SearchEquipStatus(bool async)
        {
            if (async)
                System.Threading.ThreadPool.QueueUserWorkItem(SearchEquipStatus_aync, null);
            else
                SearchEquipStatus_aync(null);
        }

        private void SearchEquipStatus_aync(object state)
        {
            try
            {
                RO.TestCommon obj = new RO.TestCommon();
                DataTable dt = obj.GetPcmEquipStatus(GlobalVariable.Factory);

                BindingEquipData(dt);
            }
            catch (Exception ex)
            {
                // 백그라운드 실행일때는 무시
                if (!InvokeRequired)
                    throw ex;
            }
        }

        private void SetTimer()
        {
            timer1.Stop();

            timer1.Interval = (int)(numRefresh.Value * 1000 * 60);
            _interval = timer1.Interval / 1000;

            timer1.Start();
        }

        private void BindingEquipData(DataTable dt)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<DataTable>(BindingEquipData), dt);
                return;
            }

            DataTable source = fpSpread1_Sheet1.DataSource as DataTable;

            if (source == null) // 최초 바인딩
            {
                fpSpread1_Sheet1.DataSource = dt;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    fpSpread1_Sheet1.Columns[i].Locked = true;
                    fpSpread1_Sheet1.Columns[i].HorizontalAlignment = CellHorizontalAlignment.Center;
                }

                fpSpread1_Sheet1.Columns[(int)Col.PROBE_CARD_USAGE].HorizontalAlignment = CellHorizontalAlignment.Right;
                FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, 0, Col.PROBE_CARD_USAGE);
                fpSpread1_Sheet1.Columns[(int)Col.EQUIP_ID].Width = 80;
                fpSpread1_Sheet1.Columns[(int)Col.DEVICE].Width = 65;
                fpSpread1_Sheet1.Columns[(int)Col.PROGRAM].Width = 100;
                fpSpread1_Sheet1.Columns[(int)Col.LOT_ID].Width = 80;
                fpSpread1_Sheet1.Columns[(int)Col.WAFER_ID].Width = 100;
                fpSpread1_Sheet1.Columns[(int)Col.WAFER_ID].HorizontalAlignment = CellHorizontalAlignment.Right;
                fpSpread1_Sheet1.Columns[(int)Col.STATUS].Width = 120;
                fpSpread1_Sheet1.Columns[(int)Col.LAST_STATUS_TIME].Width = 150;
                fpSpread1_Sheet1.Columns[(int)Col.OPERATOR].Width = 80;
                fpSpread1_Sheet1.Columns[(int)Col.PROBE_CARD].Width = 120;
                fpSpread1_Sheet1.Columns[(int)Col.PROBE_CARD_USAGE].Width = 100;
                fpSpread1_Sheet1.Columns[(int)Col.END_PGM].Width = 120;
                fpSpread1_Sheet1.Columns[(int)Col.END_WAFER].Width = 100;
                fpSpread1_Sheet1.Columns[(int)Col.END_WAFER].HorizontalAlignment = CellHorizontalAlignment.Right;
                fpSpread1_Sheet1.Columns[(int)Col.END_TIME].Width = 150;
                fpSpread1_Sheet1.Columns[(int)Col.TEST_TIME].Width = 80;
                fpSpread1_Sheet1.Columns[(int)Col.TEST_TIME].HorizontalAlignment = CellHorizontalAlignment.Right;
                fpSpread1_Sheet1.Columns[(int)Col.FLAG].Visible = false;
            }
            else
            {
                for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                {
                    string equipID = fpSpread1_Sheet1.Cells[i, (int)Col.EQUIP_ID].Text;

                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        if (equipID == (string)dt.Rows[r][(int)Col.EQUIP_ID])
                        {
                            source.Rows[i].ItemArray = dt.Rows[i].ItemArray;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                SetRowBackColor(i);
        }

        private void SetRowBackColor(int row)
        {
            string sFlag = fpSpread1_Sheet1.Cells[row, (int)Col.FLAG].Text;

            if (String.IsNullOrEmpty(sFlag))
            {
                fpSpread1_Sheet1.Rows[row].ResetBackColor();
                return;
            }

            if (String.Equals(sFlag, "Y"))
                fpSpread1_Sheet1.Rows[row].BackColor = Color.LightGray;
            else fpSpread1_Sheet1.Rows[row].BackColor = Color.White;
        }

        private void SearchDataInputHis(string equipID, bool async)
        {
            if (async)
                System.Threading.ThreadPool.QueueUserWorkItem(SearchDataInputHis_async, equipID);
            else
                SearchDataInputHis_async(equipID);
        }

        private void SearchDataInputHis_async(object state)
        {
            string equipID = (string)state;

            try
            {
                RO.TestCommon obj = new RO.TestCommon();
                DataTable dt = obj.GetPcmInputDataHistory(
                    GlobalVariable.Factory,
                    equipID,
                    dtStart.Value.Date.ToString("yyyyMMdd"),
                    dtEnd.Value.AddDays(1).Date.ToString("yyyyMMdd")
                    );

                BindingHisData(dt);
            }
            catch (Exception ex)
            {
                // 백그라운드 실행일때는 무시
                if (!InvokeRequired)
                    throw ex;
            }
        }

        private void BindingHisData(DataTable dt)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<DataTable>(BindingHisData), dt);
                return;
            }

            fpSpread2_Sheet1.DataSource = dt;
            FPSpreadUtil.SetAutoColumnWidth(fpSpread2_Sheet1);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                fpSpread2_Sheet1.Columns[i].Locked = true;
                fpSpread2_Sheet1.Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            }

            fpSpread2_Sheet1.Columns[(int)HisCol.EQUIP_ID].Width = 80;
            fpSpread2_Sheet1.Columns[(int)HisCol.INPUT_TIME].Width = 150;
            fpSpread2_Sheet1.Columns[(int)HisCol.PROBE_CARD].Width = 120;
            fpSpread2_Sheet1.Columns[(int)HisCol.OPERATOR].Width = 100;
        }

        public void ExportExcel()
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(fpSpread2);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }
        #endregion

        private void frmPcmEquipStatus_VisibleChanged(object sender, EventArgs e)
        {
            SearchAll();
        }

        private void numRefresh_ValueChanged(object sender, EventArgs e)
        {
            SetTimer();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _interval = timer1.Interval / 1000;
            SearchAll();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.SetStatusMessage("데이터를 조회중입니다...");

                //명시적으로 Search 버튼을 누를때는 동기 방식으로 조회
                SearchAll(false);
                SetTimer();
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void fpSpread1_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            if (e.Range.Row < 0)
            {
                pnlDataInput.Enabled = false;
                ClearInputControl();
                return;
            }

            string equipID = fpSpread1_Sheet1.Cells[e.Range.Row, (int)Col.EQUIP_ID].Text;
            string lotID = fpSpread1_Sheet1.Cells[e.Range.Row, (int)Col.LOT_ID].Text;

            if (chkAll.Checked)
                chkAll.Checked = false;

            // DATA INPUT HIS
            SearchDataInputHis(equipID, false);

            // DATA INPUT
            pnlDataInput.Enabled = true;
            txtEquipID.Text = equipID;
            txtProbeCard.Text = fpSpread1_Sheet1.Cells[e.Range.Row, (int)Col.PROBE_CARD].Text;

            //pnlDataInput.Focus();
            //Clipboard.Clear();
            //if (String.IsNullOrEmpty(lotID))
            //    return;

            //Clipboard.SetText(lotID);
        }


        private void fpSpread1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                string lotID = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)Col.LOT_ID].Text;
                if (string.IsNullOrEmpty(lotID))
                    return;

                Clipboard.SetText(lotID);
                e.Handled = true;
            }
        }

        private void ducOperator_Search(object sender, Framework.Controls.UserIDValidatorEventArgs e)
        {
            DACrux.Framework.RO.UserManagement obj = new Framework.RO.UserManagement();
            e.UserName = obj.GetUserName(e.UserID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtEquipID.Text))
            {
                ShowErrorMessage("EQUIP ID 가 선택되지 않았습니다.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtProbeCard.Text))
            {
                ShowErrorMessage("PROBE CARD 를 입력하세요.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtOperator.Text))
            {
                ShowErrorMessage("OPERATOR 정보를 확인하세요.");
                return;
            }

            if (!ShowQuestionMessage("저장하시겠습니까?"))
                return;

            RO.TestCommon obj = new RO.TestCommon();
            obj.InsertPcmInputData(
                GlobalVariable.Factory,
                txtEquipID.Text.Trim(),
                txtProbeCard.Text.Trim(),
                txtOperator.Text.Trim(),
                GlobalVariable.UserID);

            btnSearch.PerformClick();
            ShowMessage("저장하였습니다.");
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                SearchDataInputHis("ALL", false);
        }

        private void date_ValueChanged(
            object sender, 
            EventArgs e
            )
        {
            chkAll.Checked = false;
        }
    }
}
