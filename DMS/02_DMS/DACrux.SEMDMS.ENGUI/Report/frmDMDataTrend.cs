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
using DACrux.SEMDMS.RO;
using DACrux.Utility;
using System.Windows.Forms.DataVisualization.Charting;
using FarPoint.Win.Spread;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDMDataTrend : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Member ]
        private static readonly string DATE_FORMAT = "yyyyMMdd";
        private bool IsView = false;
        private enum ColumnIndex { RN = 0, ROWSPAN, PRODUCT, STEP_ID, INSPECTION_TIME, INSPECTOR, WAFER_ID, LOT_ID }
        #endregion [ Data Member ]

        #region [ Constrator ]
        public frmDMDataTrend()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        #region [ Event Handler ]
        private void frmDMDataTrend_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            fromDate.Value = DateTime.Now.AddDays(-30);
            toDate.Value = DateTime.Now;
            Utility.FPSpreadUtil.InitSpread(fpSpread1);

            lstXItem.SelectedIndex = 0;

            SEMConfiguration obj = new SEMConfiguration();
            FillDefectType(
                obj.GetDefectTypeList()
                );
        }

        //--

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            GetData();
        }

        //--

        private void cmbXitem_SelectedValueChanged(
            object sender,
            EventArgs e
            )
        {
            IsView = false;
            btnView.PerformClick();
        }

        //--

        private void chkScaleBreaks_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count <= 0)
                return;

            DrawChart(dt);
        }

        //--

        private void date_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            dlbProduct.ClearDataSource();
            dlbStep.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetProduct(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1)
                );
            SetBinding(dt, dlbProduct);
        }

        //--

        private void dlbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbProduct.SelectedIndex < 0)
                return;

            dlbStep.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionStep(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products
                );
            SetBinding(dt, dlbStep);

        }

        //--

        private void dlbStep_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbStep.SelectedIndex < 0)
                return;

            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;
            string[] stepids = GetSelectedValues(dlbStep);
            if (stepids == null || stepids.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionLot(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products,
                stepids
                );
            SetBinding(dt, dlbLot);
        }

        //--

        private void dlbLot_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbLot.SelectedIndex < 0)
                return;

            dlbWafer.ClearDataSource();
            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;
            string[] stepids = GetSelectedValues(dlbStep);
            if (stepids == null || stepids.Length <= 0)
                return;

            string[] lots = GetSelectedValues(dlbLot);
            if (lots == null || lots.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionWafer(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products,
                stepids,
                lots
                );
            SetBinding(dt, dlbWafer, 1, 0);
        }

        //--

        private void dlbWafer_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            IsView = true;
            btnView.PerformClick();
        }

        private void dlbDefectClass_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            IsView = true;
            btnView.PerformClick();
        }

        private void fpSpread1_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
        }

        private void nudDecimalPlaces_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            Utility.FPSpreadUtil.SetDecimalLength(
               fpSpread1_Sheet1, (int)nudDecimalPlaces.Value
               );

            fpSpread1.Refresh();
        }

        private void tsm_Click(
            object sender,
            EventArgs e
            )
        {
            chart1.ChartAreas["CHARTAREA"].AxisX.ScaleView.ZoomReset(0);
            chart1.ChartAreas["CHARTAREA"].AxisY.ScaleView.ZoomReset(0);
        }


        #endregion [ Event Handler ]

        #region [ Method ]

        private void DrawChart(
            DataTable dt
            )
        {
            InitChartControl();
            ChartArea chartArea = chart1.ChartAreas.Add("CHARTAREA");
            Legend legend = chart1.Legends.Add("LEGEND");
            chartArea.AxisX.Enabled = AxisEnabled.True;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chartArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScaleView.ZoomReset();
            chartArea.AxisX.ScrollBar.LineColor = Color.Black;
            chartArea.AxisX.ScrollBar.Size = 17;
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IsLabelAutoFit = true;

            chartArea.CursorX.AutoScroll = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0.1;
            chartArea.CursorX.IntervalOffset = 0.1;
            chartArea.CursorX.IntervalOffsetType = DateTimeIntervalType.Auto;

            //--

            chartArea.AxisY.Enabled = AxisEnabled.True;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = true;
            //cArea.AxisY.LabelStyle.Format = GetFormat();
            chartArea.AxisY.LabelStyle.Format = "{0:0." + String.Concat(Enumerable.Repeat("0", (int)nudDecimalPlaces.Value)) + "}";
            chartArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
            chartArea.AxisY.ScaleView.Zoomable = true;
            chartArea.AxisY.ScaleView.ZoomReset();
            chartArea.AxisY.ScrollBar.LineColor = Color.Black;
            chartArea.AxisY.ScrollBar.Size = 17;
            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            chartArea.AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
            chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartArea.AxisY.ScaleBreakStyle.Spacing = 2;
            chartArea.AxisY.ScaleBreakStyle.LineWidth = 2;
            chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            chartArea.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;


            chartArea.CursorY.AutoScroll = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.Interval = 0.1;
            chartArea.CursorY.IntervalOffset = 0.1;
            chartArea.CursorY.IntervalOffsetType = DateTimeIntervalType.Auto;

            //--

            legend.Enabled = false;
            for (int idx = 0; idx < lstXItem.SelectedItems.Count; idx++)
            {
                string columnName = lstXItem.SelectedItems[idx].ToString();

                string seriesPoint = string.Format("P_SERIES_{0}", columnName);
                string seriesFastLine = string.Format("FL_SERIES_{0}", columnName);

                Series series = chart1.Series.Add(seriesPoint);
                series.ChartType = SeriesChartType.Point;
                series.BorderWidth = 5;
                series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
                series.SmartLabelStyle.Enabled = true;
                series.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top;
                series.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
                series.SmartLabelStyle.MaxMovingDistance = 1;
                series.MarkerSize = 10;
                series.BorderColor = Color.Blue;
                DataPoint dp = null;
                foreach (DataRow row in dt.Rows)
                {
                    dp = new DataPoint();
                    dp.SetValueXY(
                        String.Format("{0}#{1}", row["WAFER_ID"].ToString(), row["STEP_ID"].ToString()),
                        new object[] { row[columnName].ToString() }
                        );
                    dp.ToolTip = GetToolTip(row);
                    series.Points.Add(dp);
                }

                //--

                series = chart1.Series.Add(seriesFastLine);
                series.ChartType = SeriesChartType.FastLine;
                series.BorderColor = Color.Blue;
                series.MarkerSize = 1;
                series.IsValueShownAsLabel = false;

                chart1.DataManipulator.Filter(CompareMethod.EqualTo, double.NaN, seriesPoint, seriesFastLine);
            }
        }

        //--

        public void ExportExcel()
        {
            ExcelSheet sheet1 = new ExcelSheet();
            string columnNames = string.Empty;

            sheet1.Add(
                new TextObject("DM DataTrend", 20, true, Color.Black),
                new TextObject(string.Format("From Date: {0}", fromDate.Value), 10),
                new TextObject(string.Format("To Date: {0}", toDate.Value), 10),
                new TextObject(string.Format("Product: {0}", String.Join(", ", GetSelectedValues(dlbProduct))), 10),
                new TextObject(string.Format("Step ID: {0}", String.Join(", ", GetSelectedValues(dlbStep))), 10),
                new TextObject(string.Format("Lot ID: {0}", String.Join(", ", GetSelectedValues(dlbLot))), 10),
                new TextObject(string.Format("Wafer ID: {0}", String.Join(", ", GetSelectedValues(dlbWafer))), 10),
                new TextObject(string.Format("Defect Class: {0}", String.Join(", ", GetClassNumber())), 10),
                new TextObject(string.Format("Item: {0}", columnNames), 10)
            );
            sheet1.Add(chart1);

            ExcelSheet sheet2 = new ExcelSheet();
            sheet2.Add(fpSpread1);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);
            e.SheetList.Add(sheet2);

            ExcelExportManager.Export(e);
        }

        //--

        private void FillDefectType(
            DataTable dt
            )
        {
            dlbDefectClass.ClearDataSource();
            SetBinding(dt, dlbDefectClass, 1, 0);
        }

        //--

        private bool FindColumn(
            string label
            )
        {
            ColumnIndex colIndex;
            if (!Enum.TryParse(label, out colIndex))
                return false;
            else
                return true;
        }

        //--

        private string[] GetClassNumber()
        {
            string[] classNumber = GetSelectedValues(dlbDefectClass);
            if (classNumber == null)
            {
                classNumber = new string[dlbDefectClass.Items.Count];
                for (int idx = 0; idx < dlbDefectClass.Items.Count; idx++)
                {
                    DataRowView drv = dlbDefectClass.Items[idx] as DataRowView;
                    if (drv == null)
                        continue;
                    classNumber[idx] = drv.Row[0].ToString();
                }
            }

            return classNumber;
        }

        //--

        private void GetData(
            )
        {
            DataTable dt = null;
            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                this.Cursor = Cursors.WaitCursor;

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (IsView)
                {
                    string[] products = GetSelectedValues(dlbProduct);
                    if (products == null || products.Length <= 0)
                        return;

                    string[] stepids = GetSelectedValues(dlbStep);
                    if (stepids == null || stepids.Length <= 0)
                        return;

                    string[] lots = GetSelectedValues(dlbLot);
                    if (lots == null || lots.Length <= 0)
                        return;

                    string[] wafers = GetSelectedValues(dlbWafer);
                    if (wafers == null || !(wafers.Length > 0 && wafers.Length < 1000))
                        return;

                    string[] classnumber = GetClassNumber();

                    RO.DMReport obj = new RO.DMReport();

                    string ordered = string.Empty;
                    if (rbResulTime.Checked)
                        ordered = String.Format("INSPECTION_TIME {0}", rbAsc.Checked ? "ASC" : "DESC");
                    else if (rbWaferID.Checked)
                        ordered = String.Format("WAFER_ID {0}", rbAsc.Checked ? "ASC" : "DESC");
                    else if (rbLotID.Checked)
                        ordered = String.Format("LOT_ID {0}", rbAsc.Checked ? "ASC" : "DESC");
                    else ordered = ordered = String.Format("STEP_ID {0}", rbAsc.Checked ? "ASC" : "DESC");

                    dt = obj.GetDataTrend(
                        GetDateToString(fromDate),
                        GetDateToString(toDate, 1),
                        products,
                        stepids,
                        lots,
                        wafers,
                        classnumber,
                        ordered,
                        chkLastInspection.Checked
                        );
                }
                else
                {
                    dt = fpSpread1_Sheet1.DataSource as DataTable;
                }

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                Utility.FPSpreadUtil.InitSpread(fpSpread1);
                if (dt == null)
                    return;

                Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1, 100, (int)nudDecimalPlaces.Value);
                Utility.FPSpreadUtil.VisibleSpreadColumns(fpSpread1_Sheet1, new int[] { (int)ColumnIndex.RN, (int)ColumnIndex.ROWSPAN }, false);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                fpSpread1_Sheet1.FrozenColumnCount = (int)ColumnIndex.LOT_ID + 1;

                //--

                int iRowSpanCnt = 0;
                for (int rowIdx = 0; rowIdx < fpSpread1_Sheet1.RowCount; rowIdx++)
                {
                    iRowSpanCnt = Base.Convert.intParse(fpSpread1_Sheet1.Cells[rowIdx, (int)ColumnIndex.ROWSPAN].Text);
                    if (iRowSpanCnt != 1)
                    {
                        fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.RN].RowSpan = iRowSpanCnt;
                        fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.ROWSPAN].RowSpan = iRowSpanCnt;
                        fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.PRODUCT].RowSpan = iRowSpanCnt;
                        fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.STEP_ID].RowSpan = iRowSpanCnt;
                        fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.INSPECTION_TIME].RowSpan = iRowSpanCnt;
                        //fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.INSPECTOR].RowSpan = iRowSpanCnt;
                        //fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.WAFER_ID].RowSpan = iRowSpanCnt;
                        //fpSpread1_Sheet1.Cells[(rowIdx - iRowSpanCnt) + 1, (int)ColumnIndex.LOT_ID].RowSpan = iRowSpanCnt;
                    }
                }

                //--

                DrawChart(
                    dt
                    );
            }
            finally
            {
                this.Cursor = Cursors.Default;
                MainForm.SetStatusMessage(null);
            }
        }

        //--

        private string GetDateToString(
            DateTimePicker datepicker,
            int addDay = 0
            )
        {
            return datepicker.Value.AddDays(addDay).ToString(DATE_FORMAT);
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

        //--

        private string GetToolTip(DataRow row)
        {
            return String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}",
                row["STEP_ID"].ToString(),
                row["INSPECTION_TIME"].ToString(),
                row["INSPECTOR"].ToString(),
                row["LOT_ID"].ToString(),
                row["WAFEr_ID"].ToString()
                );
        }

        //--

        private void InitChartControl(
            )
        {
            chart1.Annotations.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();
            chart1.Series.Clear();
        }

        //--

        private void SetBinding(
            DataTable dt,
            DUCListBox dListBox,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (dt != null)
            {
                dListBox.DisplayMember = dt.Columns[dispIndex].ColumnName;
                dListBox.ValueMember = dt.Columns[valueIndex].ColumnName;
            }
            dListBox.DataSource = dt;
        }

        //--

        #endregion [ Method ]
    }
}
