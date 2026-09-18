using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.RO;
using FarPoint.Win.Spread;

namespace DACrux.Framework
{
    public partial class frmServiceLog
        : DACrux.Framework.Base.DACruxUXBasic02
    {
        enum ColumnIndex
        {
            FACTORY = 0,
            SERVICE_NAME,
            TRAN_DATE,
            SERVER_NAME,
            ACTION,
            FILE_NAME,
            EQUIP_ID,
            LOT_ID,
            WAFER_ID,
            HANLDER,
            EXECUTE_TIME,
            MESSAGE,
            DETAIL_MSG
        }

        enum Action { ERROR = 0, SERVICE_START, SERVICE_STOP, SUCCESS, WARNING }

        public frmServiceLog()
        {
            InitializeComponent();
            dtStart.Value = DateTime.Now.AddDays(-1);
            dtEnd.Value = DateTime.Now;
        }

        #region [ Event Handler ]
        private void frmServiceLog_Load(
            object sender,
            EventArgs e
            )
        {
            DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);
            FillServiceList();
            FillServiceAction();
        }

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                ServiceLog obj = new ServiceLog();
                txtMessage.Text = String.Empty;
                txtDetailMessage.Text = String.Empty;

                MainForm.SetStatusMessage("Data를 조회 중입니다.");
                DataTable dt = obj.GetDataServiceList(
                    dtStart.Value.Date,
                    dtEnd.Value.AddDays(1).Date,
                    DACrux.Base.GlobalVariable.Factory,
                    lstServiceName.SelectedValues,
                    lstAction.SelectedValues,
                    txtEquipID.Text,
                    txtLotID.Text
                    );

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);
                FillData(dt);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void butClose_Click(
            object sender,
            EventArgs e
            )
        {
            this.Close();
        }

        private void fpSpread1_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            SheetView sheet = e.View.Sheets[0];
            txtMessage.Text = sheet.Cells[e.Row, (int)ColumnIndex.MESSAGE].Text;
            txtDetailMessage.Text = sheet.Cells[e.Row, (int)ColumnIndex.DETAIL_MSG].Text.Trim().Replace("   ", "\r\n");
        }

        private void fpSpread1_KeyDown(
            object sender,
            KeyEventArgs e
            )
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                SheetView sheet = (sender as FpSpread).ActiveSheet;
                e.Handled = true;
                Clipboard.Clear();
                string lotId = sheet.Cells[sheet.ActiveRowIndex, (int)ColumnIndex.LOT_ID].Text;
                Clipboard.SetText(lotId);
            }
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        private void FillServiceList()
        {
            ServiceLog obj = new ServiceLog();
            string[] data = obj.GetDataServiceName();
            lstServiceName.Items.AddRange(data);
        }

        //--

        private void FillServiceAction()
        {
            ServiceLog obj = new ServiceLog();
            string[] data = obj.GetDataServiceAction();
            lstAction.Items.AddRange(data);
        }

        //--

        private void FillData(
            DataTable dt
            )
        {
            DACrux.Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1, 70, 3);
            DACrux.Utility.FPSpreadUtil.SetColumnVisible(fpSpread1_Sheet1, new int[] { (int)ColumnIndex.MESSAGE, (int)ColumnIndex.DETAIL_MSG }, new bool[] { false, false });
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.FACTORY].Width = 75;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.SERVICE_NAME].Width = 145;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.TRAN_DATE].Width = 145;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.SERVER_NAME].Width = 110;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.ACTION].Width = 110;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.ACTION].HorizontalAlignment = CellHorizontalAlignment.Left;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.FILE_NAME].Width = 500;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.FILE_NAME].HorizontalAlignment = CellHorizontalAlignment.Left;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.EQUIP_ID].Width = 80;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.LOT_ID].Width = 85;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.WAFER_ID].Width = 331;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.WAFER_ID].HorizontalAlignment = CellHorizontalAlignment.Left;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.HANLDER].Width = 140;
            fpSpread1_Sheet1.Columns[(int)ColumnIndex.EXECUTE_TIME].Width = 140;

            for (int rowIdx = 0; rowIdx < fpSpread1_Sheet1.RowCount; rowIdx++)
            {
                string action = fpSpread1_Sheet1.Cells[rowIdx, (int)ColumnIndex.ACTION].Text;
                Action serviceAction = (Action)Enum.Parse(typeof(Action), action);

                switch (serviceAction)
                {
                    case Action.ERROR:
                        fpSpread1_Sheet1.Rows[rowIdx].ForeColor = Color.Red;
                        fpSpread1_Sheet1.Rows[rowIdx].Font = new Font(this.Font, FontStyle.Bold);
                        break;
                    case Action.SERVICE_START:
                    case Action.SERVICE_STOP:
                        fpSpread1_Sheet1.Rows[rowIdx].ForeColor = Color.Blue;
                        fpSpread1_Sheet1.Rows[rowIdx].Font = new Font(this.Font, FontStyle.Regular);
                        break;
                    case Action.WARNING:
                        fpSpread1_Sheet1.Rows[rowIdx].ForeColor = Color.SkyBlue;
                        fpSpread1_Sheet1.Rows[rowIdx].Font = new Font(this.Font, FontStyle.Regular);
                        break;
                    default:
                        break;
                }
            }
        }


        #endregion [ Method ]
    }
}
