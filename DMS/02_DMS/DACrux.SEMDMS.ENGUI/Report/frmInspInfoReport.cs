using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using DACrux.Framework.Base;
using DACrux.Utility;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmInspInfoReport : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]
        private DataSet ds = null;
        enum ColIndex
        {
            SHIFT_TIME = 0, MODEL = 1, H06, H07, H08, H09, H10, H11, H12, H13, DAY_SUM,
            H14, H15, H16, H17, H18, H19, H20, H21, SWING_SUM,
            H22, H23, H00, H01, H02, H03, H04, H05, NIGHT_SUM, TOTAL
        }
        #endregion [ Data Field ]

        //-------------------------------------------------------------------------------------------------

        #region [ Constrator ]
        public frmInspInfoReport()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        //-------------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void frmInspInfoReport_Load(
            object sender,
            EventArgs e
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpSpread1);
            fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            fromDate.Value = DateTime.Now.AddDays(-7);
            toDate.Value = DateTime.Now;
        }

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            DMReport obj = null;
            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                obj = new DMReport();

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                ds = obj.GetInspectionLotOrWaferCnt(
                    DACrux.Base.GlobalVariable.Factory,
                    fromDate.Value.Date.ToString("yyyyMMdd"),
                    toDate.Value.AddDays(1).Date.ToString("yyyyMMdd"),
                    rbLot.Checked ? true : false, // Lot = True, Wafer = False
                    rbInspection.Checked ? true : false // Inspection = True, Review = False
                    );

                if (ds == null || ds.Tables == null || ds.Tables.Count <= 0)
                    return;

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                FillData(CalcuationShiftSum(ds.Tables["PIVOT_DATA"]));
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void fpSpread1_CellDoubleClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            FarPoint.Win.Spread.FpSpread spread = (sender as FarPoint.Win.Spread.FpSpread);
            if (spread == null)
                return;

            if (e.Column == (int)ColIndex.SHIFT_TIME
                || e.Column == (int)ColIndex.MODEL)
                return;

            FarPoint.Win.Spread.SheetView sheet = spread.ActiveSheet;
            if (sheet == null)
                return;

            if (String.Equals(sheet.Cells[e.Row, (int)ColIndex.MODEL].Text, "SUM"))
                return;

            if (String.Equals(sheet.Columns[e.Column].Label, "SUM")
                || String.Equals(sheet.Columns[e.Column].Label, "TOTAL"))
                return;

            if (String.IsNullOrEmpty(sheet.Cells[e.Row, e.Column].Text))
                return;

            string date = sheet.Cells[e.Row, (int)ColIndex.SHIFT_TIME].Text;
            string model = sheet.Cells[e.Row, (int)ColIndex.MODEL].Text;
            string hour = sheet.Columns[e.Column].Label;

            frmInspectionDetail dlg = new frmInspectionDetail(
                date,
                hour,
                model,
                rbInspection.Checked ? true : false
                );
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);
        }

        private void rbOption_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            /// RadioButton 에 대한 이벤트를 한곳으로 묶어 놓아서 
            /// Checked 가 false 인 경우에는 Return 하도록 처리
            if(!(sender as RadioButton).Checked)
                return;

            btnSearch.PerformClick();
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        public DataTable CalcuationShiftSum(
            DataTable dt
            )
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt;

            dt.Columns.Remove("ORD");
            dt.Columns.Add(new DataColumn("DAY_SUM", typeof(long)));
            dt.Columns.Add(new DataColumn("SWING_SUM", typeof(long)));
            dt.Columns.Add(new DataColumn("NIGHT_SUM", typeof(long)));
            dt.Columns.Add(new DataColumn("TOTAL", typeof(long)));
            dt.Columns["DAY_SUM"].SetOrdinal((int)ColIndex.DAY_SUM);
            dt.Columns["SWING_SUM"].SetOrdinal((int)ColIndex.SWING_SUM);
            dt.Columns["NIGHT_SUM"].SetOrdinal((int)ColIndex.NIGHT_SUM);

            int tmp = 0;
            int sumDay = 0;
            int sumSwing = 0;
            int sumNight = 0;

            for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
            {
                DataRow r = dt.Rows[rowIdx];
                tmp = 0;
                sumDay = 0;
                sumSwing = 0;
                sumNight = 0;

                for (int colIdx = (int)ColIndex.H06; colIdx < (int)ColIndex.DAY_SUM; colIdx++)
                {
                    int.TryParse(r[colIdx].ToString(), out tmp);
                    sumDay += tmp;
                }
                r[(int)ColIndex.DAY_SUM] = sumDay;

                for (int colIdx = (int)ColIndex.H14; colIdx < (int)ColIndex.SWING_SUM; colIdx++)
                {
                    int.TryParse(r[colIdx].ToString(), out tmp);
                    sumSwing += tmp;
                }
                r[(int)ColIndex.SWING_SUM] = sumSwing;

                for (int colIdx = (int)ColIndex.H22; colIdx < (int)ColIndex.NIGHT_SUM; colIdx++)
                {
                    int.TryParse(r[colIdx].ToString(), out tmp);
                    sumNight += tmp;
                }
                r[(int)ColIndex.NIGHT_SUM] = sumNight;
                r[(int)ColIndex.TOTAL] = sumDay + sumSwing + sumNight;
            }
            dt.Rows[dt.Rows.Count - 1][dt.Columns.Count - 1] = DBNull.Value;
            return dt;
        }

        public void ExportExcel()
        {
            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.Add(fpSpread1);

            ExcelSheet sheet2 = new ExcelSheet();
            sheet2.Add(ds.Tables["RAWDATA"]);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);
            e.SheetList.Add(sheet2);

            ExcelExportManager.Export(e);
        }

        private void FillData(
            DataTable dt
            )
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            Utility.FPSpreadUtil.InitSpread(fpSpread1);
            fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;

            Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1, 120, 0);
            fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;

            #region [ Sheet Header ]
            fpSpread1_Sheet1.ColumnHeader.RowCount = 2;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.SHIFT_TIME].Value = "Shift";
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.MODEL].RowSpan = 2;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.MODEL].Value = "DM EQ";
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H06].ColumnSpan = 9;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H06].Value = "Day";
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H14].ColumnSpan = 9;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H14].Value = "Swing";
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H22].ColumnSpan = 9;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.H22].Value = "Night";
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.TOTAL].RowSpan = 2;
            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)ColIndex.TOTAL].Value = "Total";

            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.SHIFT_TIME].Value = "Time(HH)";
            fpSpread1_Sheet1.ColumnHeader.Columns[(int)ColIndex.SHIFT_TIME].Width = 120;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H06].Value = "06"; fpSpread1_Sheet1.Columns[(int)ColIndex.H06].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H07].Value = "07"; fpSpread1_Sheet1.Columns[(int)ColIndex.H07].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H08].Value = "08"; fpSpread1_Sheet1.Columns[(int)ColIndex.H08].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H09].Value = "09"; fpSpread1_Sheet1.Columns[(int)ColIndex.H09].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H10].Value = "10"; fpSpread1_Sheet1.Columns[(int)ColIndex.H10].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H11].Value = "11"; fpSpread1_Sheet1.Columns[(int)ColIndex.H11].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H12].Value = "12"; fpSpread1_Sheet1.Columns[(int)ColIndex.H12].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H13].Value = "13"; fpSpread1_Sheet1.Columns[(int)ColIndex.H13].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.DAY_SUM].Value = "SUM"; fpSpread1_Sheet1.Columns[(int)ColIndex.DAY_SUM].Width = 35;

            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H14].Value = "14"; fpSpread1_Sheet1.Columns[(int)ColIndex.H14].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H15].Value = "15"; fpSpread1_Sheet1.Columns[(int)ColIndex.H15].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H16].Value = "16"; fpSpread1_Sheet1.Columns[(int)ColIndex.H16].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H17].Value = "17"; fpSpread1_Sheet1.Columns[(int)ColIndex.H17].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H18].Value = "18"; fpSpread1_Sheet1.Columns[(int)ColIndex.H18].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H19].Value = "19"; fpSpread1_Sheet1.Columns[(int)ColIndex.H19].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H20].Value = "20"; fpSpread1_Sheet1.Columns[(int)ColIndex.H20].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H21].Value = "21"; fpSpread1_Sheet1.Columns[(int)ColIndex.H21].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.SWING_SUM].Value = "SUM"; fpSpread1_Sheet1.Columns[(int)ColIndex.SWING_SUM].Width = 35;

            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H22].Value = "22"; fpSpread1_Sheet1.Columns[(int)ColIndex.H22].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H23].Value = "23"; fpSpread1_Sheet1.Columns[(int)ColIndex.H23].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H00].Value = "00"; fpSpread1_Sheet1.Columns[(int)ColIndex.H00].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H01].Value = "01"; fpSpread1_Sheet1.Columns[(int)ColIndex.H01].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H02].Value = "02"; fpSpread1_Sheet1.Columns[(int)ColIndex.H02].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H03].Value = "03"; fpSpread1_Sheet1.Columns[(int)ColIndex.H03].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H04].Value = "04"; fpSpread1_Sheet1.Columns[(int)ColIndex.H04].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.H05].Value = "05"; fpSpread1_Sheet1.Columns[(int)ColIndex.H05].Width = 35;
            fpSpread1_Sheet1.ColumnHeader.Cells[1, (int)ColIndex.NIGHT_SUM].Value = "SUM"; fpSpread1_Sheet1.Columns[(int)ColIndex.NIGHT_SUM].Width = 35;
            #endregion [ Sheet Header ]

            fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;

            //--

            #region [ Rowspan and back color ]
            int iRowSpanCnt = 0;
            String tmp = String.Empty;
            for (int idx = 0; idx < fpSpread1_Sheet1.RowCount; idx++)
            {
                iRowSpanCnt++;
                if (String.Equals(fpSpread1_Sheet1.Cells[idx, (int)ColIndex.MODEL].Text, "SUM"))
                {
                    if (String.Equals(fpSpread1_Sheet1.Cells[idx, (int)ColIndex.SHIFT_TIME].Text, "TOTAL"))
                        continue;

                    fpSpread1_Sheet1.Rows[idx].BackColor = Color.LightYellow;
                    fpSpread1_Sheet1.Cells[idx - iRowSpanCnt, (int)ColIndex.SHIFT_TIME].RowSpan = (iRowSpanCnt + 1);
                }

                if (String.Equals(tmp, fpSpread1_Sheet1.Cells[idx, (int)ColIndex.SHIFT_TIME].Text))
                    continue;

                tmp = fpSpread1_Sheet1.Cells[idx, (int)ColIndex.SHIFT_TIME].Text;
                iRowSpanCnt = 0;
            }
            #endregion [ Rowspan and back color ]

            //--

            fpSpread1_Sheet1.Rows[fpSpread1_Sheet1.RowCount - 1].BackColor = Color.LightSkyBlue;
            fpSpread1_Sheet1.Columns[(int)ColIndex.DAY_SUM].BackColor = Color.LightYellow;
            fpSpread1_Sheet1.Columns[(int)ColIndex.SWING_SUM].BackColor = Color.LightYellow;
            fpSpread1_Sheet1.Columns[(int)ColIndex.NIGHT_SUM].BackColor = Color.LightYellow;
            fpSpread1_Sheet1.Columns[(int)ColIndex.TOTAL].BackColor = Color.LightSkyBlue;
        }
        #endregion [ Method ]

    }
}
