using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.Framework.Controls;
using DACrux.Utility;
using FarPoint.Win.Spread.CellType;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmKillRate : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]
        private readonly string DATETIME_FORMAT = "yyyyMMdd";
        enum ColumnIndex { YEAR = 0, MONTH, WW, DEVICE, WAFER_ID, INSP_DATE, STEP_ID, NETDIE, TESTED_DIE, GEC, YIELD }

        int ddStartIndex = int.MaxValue, ddEndIndex = int.MinValue; // Defective Die Index
        int fbStartIdx = int.MaxValue, fbEndIdex = int.MinValue; // Fail Bin Index
        int krStartIndex = int.MaxValue, krEndIndex = int.MinValue; // Killing Rate Index
        int ylStartIndex = int.MaxValue, ylEndIndex = int.MinValue; // Yield Loss Index
        #endregion [ Data Field ]

        #region [ Constrator ]
        public frmKillRate()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        #region [ Event Handler ]

        private void frmKillRate_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            Utility.FPSpreadUtil.InitSpread(fpSpread1);

            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetDefectTypeList();
            SetBinding(dlbDefectClass, dt, 1, 0);

            fromDate.Value = DateTime.Now.AddDays(-1);
            toDate.Value = DateTime.Now;
        }

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                if (dlbWafer.SelectedItems.Count <= 0)
                    return;

                if (dlbWafer.SelectedItems.Count > 1000)
                {
                    ShowMessage("선택된 Wafer의 장수가 1000장입니다. 1000장 이상 조회할 수 없습니다.");
                    return;
                }

                string[] defectCodes = null;
                /// 선택된 Defect Class Number가 존재하지 않는 경우
                if (dlbDefectClass.SelectedItems.Count <= 0)
                {
                    defectCodes = new string[dlbDefectClass.Items.Count - 1];
                    for (int idx = 1; idx < dlbDefectClass.Items.Count; idx++)
                    {
                        DataRowView view = dlbDefectClass.Items[idx] as DataRowView;
                        defectCodes[idx - 1] = view.Row["CLASSNUMBER"].ToString();
                    }
                }
                else
                {
                    defectCodes = GetSelectedValues(dlbDefectClass);
                }

                string[] steps = GetSelectedValues(dlbLayer);
                if (steps == null || steps.Length <= 0)
                    return;

                string[] lots = GetSelectedValues(dlbLot);
                if (steps == null || steps.Length <= 0)
                    return;

                string[] wafers = GetSelectedValues(dlbWafer);
                if (steps == null || steps.Length <= 0)
                    return;

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                RO.DMReport obj = new RO.DMReport();
                DataTable dt = obj.GetKillingRate(
                    ConvertDateTimeToString(fromDate),
                    ConvertDateTimeToString(toDate, 1),
                    steps,
                    lots,
                    wafers,
                    defectCodes,
                    chkNormalized.Checked
                    );

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                FillData(dt);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
                this.Cursor = Cursors.Default;
            }
        }

        private void date_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            dlbProduct.ClearDataSource();
            dlbLayer.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetProduct(
                ConvertDateTimeToString(fromDate),
                ConvertDateTimeToString(toDate, 1)
                );
            SetBinding(dlbProduct, dt);
        }

        private void decimalPlaces_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            for (int idx = krStartIndex; idx < fpSpread1_Sheet1.ColumnCount; idx++)
            {
                if (fpSpread1_Sheet1.Columns[idx].CellType is NumberCellType)
                    (fpSpread1_Sheet1.Columns[idx].CellType as NumberCellType).DecimalPlaces = (int)((sender as NumericUpDown).Value);
            }
            fpSpread1_Sheet1.FpSpread.Refresh();
        }

        private void dlbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbProduct.SelectedIndex < 0)
                return;

            dlbLayer.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string product = dlbProduct.SelectedValue.ToString();
            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetConditionStep(
                ConvertDateTimeToString(fromDate),
                ConvertDateTimeToString(toDate, 1),
                product
                );

            SetBinding(dlbLayer, dt);
        }

        private void dlbLayer_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbLayer.SelectedIndex < 0)
                return;

            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string product = dlbProduct.SelectedValue.ToString();
            string[] steps = GetSelectedValues(dlbLayer);
            if (steps == null || steps.Length <= 0)
                return;
            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetConditionLot(
                ConvertDateTimeToString(fromDate),
                ConvertDateTimeToString(toDate, 1),
                product,
                steps
                );

            SetBinding(dlbLot, dt);
        }

        private void dlbLot_EnterTextBox(
            object sender, 
            EventArgs e
            )
        {
            string search = dlbLot.SearchText;
            if (String.IsNullOrEmpty(search))
                return;

            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetLotInfo(
                ConvertDateTimeToString(fromDate),
                ConvertDateTimeToString(toDate, 1),
                search
                );
            if (dt == null || dt.Rows.Count <= 0)
            {
                ShowMessage("데이터를 찾을 수 없습니다.");
                return;
            }
            dlbProduct.Text = dt.Rows[0]["PRODUCT"].ToString();
            dlbLayer.ClearDataSource();
            SetBinding(dlbLayer, dt, 1, 1);
            if (dt.Rows.Count == 1)
                dlbLayer.Text = dt.Rows[0]["STEP_ID"].ToString();
        }

        private void dlbLot_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbLot.SelectedIndex < 0)
                return;


            dlbWafer.ClearDataSource();

            string product = dlbProduct.SelectedValue as String;
            string[] steps = GetSelectedValues(dlbLayer);
            if (steps == null || steps.Length <= 0)
                return;

            string[] lots = GetSelectedValues(dlbLot);
            if (lots == null || lots.Length <= 0)
                return;

            RO.SEMConfiguration obj = new RO.SEMConfiguration();
            DataTable dt = obj.GetConditionWafer(
                ConvertDateTimeToString(fromDate),
                ConvertDateTimeToString(toDate, 1),
                product,
                steps,
                lots
                );

            SetBinding(dlbWafer, dt, 1, 0);
        }

        private void dlbWafer_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbWafer.SelectedIndex < 0)
                return;

            btnView.PerformClick();
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        private string ConvertDateTimeToString(
            DateTimePicker date,
            int AddDay = 0
            )
        {
            return date.Value.AddDays(AddDay).ToString(DATETIME_FORMAT);
        }

        //--

        public void ExportExcel()
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(fpSpread1);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        //--

        private void FillData(
            DataTable source
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpSpread1);
            fpSpread1_Sheet1.ColumnHeader.RowCount = 2;
            fpSpread1_Sheet1.DataSource = source;

            //--

            // Defective Die Index
            ddStartIndex = int.MaxValue;
            ddEndIndex = int.MinValue;
            // Fail Bin Index
            fbStartIdx = int.MaxValue;
            fbEndIdex = int.MinValue;
            // Killing Rate Index
            krStartIndex = int.MaxValue; 
            krEndIndex = int.MinValue;
            // Yield Loss Index
            ylStartIndex = int.MaxValue;
            ylEndIndex = int.MinValue; 

            for (int idx = 0; idx < source.Columns.Count; idx++)
            {
                if (fpSpread1_Sheet1.Columns[idx].Label.Contains("DEFECTIVE_"))
                    fpSpread1_Sheet1.Columns[idx].Label = fpSpread1_Sheet1.Columns[idx].Label.Replace("DEFECTIVE_", "");
                if (fpSpread1_Sheet1.Columns[idx].Label.Contains("FAILDIE_"))
                    fpSpread1_Sheet1.Columns[idx].Label = fpSpread1_Sheet1.Columns[idx].Label.Replace("FAILDIE_", "");
                if (fpSpread1_Sheet1.Columns[idx].Label.Contains("KILL_"))
                    fpSpread1_Sheet1.Columns[idx].Label = fpSpread1_Sheet1.Columns[idx].Label.Replace("KILL_", "");
                if (fpSpread1_Sheet1.Columns[idx].Label.Contains("YIELDLOSS_"))
                    fpSpread1_Sheet1.Columns[idx].Label = fpSpread1_Sheet1.Columns[idx].Label.Replace("YIELDLOSS_", "");

                if (source.Columns[idx].ColumnName.Contains("_SUM"))
                    fpSpread1_Sheet1.Columns[idx].BackColor = Color.SkyBlue;

                if (source.Columns[idx].ColumnName.Contains("DEFECTIVE_"))
                {
                    ddStartIndex = Math.Min(idx, ddStartIndex);
                    ddEndIndex = Math.Max(idx, ddEndIndex);
                }

                if (source.Columns[idx].ColumnName.Contains("FAILDIE_"))
                {
                    fbStartIdx = Math.Min(idx, fbStartIdx);
                    fbEndIdex = Math.Max(idx, fbEndIdex);
                }

                if (source.Columns[idx].ColumnName.Contains("KILL_"))
                {
                    krStartIndex = Math.Min(idx, krStartIndex);
                    krEndIndex = Math.Max(idx, krEndIndex);
                }

                if (source.Columns[idx].ColumnName.Contains("YIELDLOSS_"))
                {
                    ylStartIndex = Math.Min(idx, ylStartIndex);
                    ylEndIndex = Math.Max(idx, ylEndIndex);
                }
            }

            NumberCellType numberCell = new NumberCellType();
            numberCell.DecimalPlaces = (int)decimalPlaces.Value;
            numberCell.Separator = ",";
            numberCell.MaximumValue = 99999999999999;
            numberCell.MinimumValue = -99999999999999;


            for (int idx = 0; idx < ddStartIndex; idx++)
            {
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].Text = fpSpread1_Sheet1.Columns[idx].Label;
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].RowSpan = 2;
            }

            fpSpread1_Sheet1.ColumnHeader.Cells[0, ddStartIndex].ColumnSpan = ddEndIndex - ddStartIndex + 1;
            for (int idx = ddStartIndex; idx <= ddEndIndex; idx++)
            {
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].Text = "DEFECTIVE DIE";
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fpSpread1_Sheet1.Columns[idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            }
            fpSpread1_Sheet1.ColumnHeader.Cells[0, fbStartIdx].ColumnSpan = fbEndIdex - fbStartIdx + 1;
            for (int idx = fbStartIdx; idx <= fbEndIdex; idx++)
            {
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].Text = "FAIL DIE";
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fpSpread1_Sheet1.Columns[idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            }
            fpSpread1_Sheet1.ColumnHeader.Cells[0, krStartIndex].ColumnSpan = krEndIndex - krStartIndex + 1;
            for (int idx = krStartIndex; idx <= krEndIndex; idx++)
            {
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].Text = "KILL RATE";
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fpSpread1_Sheet1.Columns[idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fpSpread1_Sheet1.Columns[idx].CellType = numberCell;
            }
            fpSpread1_Sheet1.ColumnHeader.Cells[0, ylStartIndex].ColumnSpan = ylEndIndex - ylStartIndex + 1;
            for (int idx = ylStartIndex; idx <= ylEndIndex; idx++)
            {
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].Text = "YIELD LOSS";
                fpSpread1_Sheet1.ColumnHeader.Cells[0, idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fpSpread1_Sheet1.Columns[idx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fpSpread1_Sheet1.Columns[idx].CellType = numberCell;
            }
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
        }

        //--

        private string[] GetSelectedValues(
            DUCListBox listBox
            )
        {
            DataTable dt = listBox.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return null;

            object[] arr = listBox.SelectedValues;

            if (arr == null || arr.Length == 0)
                return null;

            string[] results = new string[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                results[i] = arr[i].ToString();

            return results;
        }

        private void SetBinding(
            DUCListBox lstBox,
            DataTable source,
            int iDisplayMember = 0,
            int iValueMember = 0
            )
        {
            if (source != null)
            {
                lstBox.DisplayMember = source.Columns[iDisplayMember].ColumnName;
                lstBox.ValueMember = source.Columns[iValueMember].ColumnName;
            }
            lstBox.DataSource = source;
        }

        #endregion [ Method ]
    }
}
