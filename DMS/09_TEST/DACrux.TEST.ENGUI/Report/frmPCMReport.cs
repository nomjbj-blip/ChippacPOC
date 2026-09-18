using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Framework.Base;
using DACrux.Framework.Controls;
using DACrux.TEST.RO;
using FarPoint.Win.Spread.CellType;
using Infragistics.UltraChart.Shared.Styles;
using System.Diagnostics;
using FarPoint.Win.Spread;
using System.Text;
using DACrux.Utility;
using System.Windows.Forms;
using DACrux.TEST.Control;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPCMReport
        : DACruxUXBasic01, IExportExcel
    {
        private enum SeriesStatisticsIndex
        {
            AVERAGE = 0,
            AVERAGE_POINT,
            TARGET,
            LSL,
            USL,
            LCL,
            UCL
        }

        private enum SeriesRawDataIndex
        {
            RAW_DATA = 0,
            RAW_DATA_POINT,
            TARGET,
            LSL,
            USL,
            LCL,
            UCL
        }

        private enum ColumnIndex
        {
            LOT_START_TIME = 0,
            LOT_END_TIME,
            WAFER_START_TIME,
            WAFER_END_TIME,
            PROGRAM,
            LOT_ID,
            TESTER,
            PROBE_CARD,
            OPERATOR,
            WAFER_SEQ,
            WAFER_ID,
            DIE_NUM,
            DIEPROBE_CNT
        }

        private readonly String CHART_SERIES_BOXPLOT = "BOXPLOT";
        private static readonly string DATE_FORMAT = "yyyyMMdd";
        private string[] TestArea = null;
        private bool RefrashFlag = false;
        private SortedColumns SortedColumn = null;
        private string FilterColumn = string.Empty;
        private string FilterValue = string.Empty;

        //--

        public frmPCMReport(
            )
        {
            InitializeComponent();
            SortedColumn = new SortedColumns();
            SortedColumn.Add(Enum.GetName(typeof(ColumnIndex), ColumnIndex.LOT_ID), true);
            SortedColumn.Add(Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_ID), true);
            SortedColumn.Add(Enum.GetName(typeof(ColumnIndex), ColumnIndex.DIE_NUM), true);
        }

        //--

        #region [ Event Handler ]

        //--

        private void frmPCMReport_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            RefrashFlag = true;
            TestArea = new string[] { radioButton1.Checked ? radioButton1.Text : radioButton2.Text };
            dtStart.Value = DateTime.Now.AddDays(-1);
            dtEnd.Value = DateTime.Now.AddDays(1);

            Utility.FPSpreadUtil.InitSpread(
                fpRawData
                );
            Utility.FPSpreadUtil.InitSpread(
                fpStatistics
                );

            uceBoxPlot.ForeColor = Color.FromArgb(24, 127, 217);
            uceAvg.ForeColor = Color.FromArgb(217, 43, 4);
            uceSpecLine.ForeColor = Color.FromArgb(212, 54, 0);
            uceControlLine.ForeColor = Color.FromArgb(0, 76, 151);
        }

        //--


        private void btnSortOption_Click(
            object sender,
            EventArgs e
            )
        {
            frmSortOption dlg = new frmSortOption(
                Enum.GetValues(typeof(ColumnIndex))
                );
            dlg.SortedColumn = SortedColumn;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            SortedColumn = dlg.SortedColumn;

            List<string> keys = new List<string>(SortedColumn.Keys);
            FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[keys.Count];
            for (int idx = 0; idx < keys.Count; idx++)
            {
                ColumnIndex colIdx = (ColumnIndex)Enum.Parse(typeof(ColumnIndex), keys[idx], false);
                if (!Enum.IsDefined(typeof(ColumnIndex), colIdx))
                    continue;
                si[idx] = new SortInfo((int)colIdx, SortedColumn[keys[idx]]);
            }
            fpRawData_Sheet1.SortRows(0, fpRawData_Sheet1.RowCount, si);
            btnView.PerformClick();
        }

        //--

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                if (dlbLot.SelectedValue == null)
                    return;

                if (dlbPgmParam.SelectedValue == null)
                    return;

                if (dlbLot.SelectedItems.Count > 40)
                    return;

                DataTable dt = null;
                if (RefrashFlag)
                {
                    RefrashFlag = false;
                    FilterColumn = FilterValue = string.Empty;

                    MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                    if (!GetRawData())
                        return;

                    dt = (DataSource as DataSet).Tables["RAW_DATA"];
                }
                else
                {
                    dt = CreateFilterDataTable(fpRawData_Sheet1);
                }

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                DrawChartByHistogram(dt);
                if (!uceBoxPlot.Checked && !uceAvg.Checked)
                {
                    DrawChartByWaferRawData(dt);
                }
                else
                {
                    dt = CreateStatisticsDataTable(dt);
                    FillStatisticsData(dt);
                    DrawChartByWafer(dt);
                }
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        //--

        private void chkAvgLable_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        //--

        private void chkRawDataLabel_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        //--

        private void chkScaleBreaks_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        //--

        private void chkSeriesOption_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            if (uceBoxPlot.Checked)
            {
                chkRawDataLabel.Enabled = false;
            }
            else
            {
                chkRawDataLabel.Enabled = true;
            }

            if (uceAvg.Checked)
            {
                chkAvgLable.Enabled = true;
            }
            else
            {
                chkAvgLable.Enabled = false;
            }

            btnView.PerformClick();
        }

        //--

        private void date_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dlbProduct.ClearDataSource();
                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                RO.DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionDevice(
                    GetDateToString(dtStart),
                    GetDateToString(dtEnd),
                    TestArea
                    );
                SetBinding(dlbProduct, dt);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //--

        private void dlbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dlbProduct.SelectedIndex < 0)
                    return;

                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                RO.DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionProgram(
                    GetDateToString(dtStart),
                    GetDateToString(dtEnd),
                    TestArea,
                    GetSelectedValues(dlbProduct)
                    );
                SetBinding(dlbProgram, dt);

                RefrashFlag = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //--

        private void dlbProgram_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dlbProgram.SelectedIndex < 0)
                    return;

                string search = String.Empty;
                if (!String.IsNullOrEmpty(dlbLot.SearchText))
                    search = dlbLot.SearchText;

                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                RO.DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionLot(
                    GetDateToString(dtStart),
                    GetDateToString(dtEnd),
                    TestArea,
                    GetSelectedValues(dlbProduct),
                    GetSelectedValues(dlbProgram)
                    );
                SetBinding(dlbLot, dt, 0, 1);
                RefrashFlag = true;
                dlbLot.SearchText = search;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //--

        private void dlbLot_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dlbLot.SelectedIndex < 0)
                    return;

                if (dlbLot.SelectedItems.Count > 40)
                    return;

                string product = dlbProduct.SelectedValue as String;
                if (string.IsNullOrEmpty(product))
                    return;

                string program = dlbProgram.SelectedValue as String;
                if (string.IsNullOrEmpty(program))
                    return;

                ProbeAdmin oProbeAdmin = new ProbeAdmin();
                DataTable dt = oProbeAdmin.GetParaSpecList(
                    Convert.ToDateTime(dtStart.Value),
                    Convert.ToDateTime(dtEnd.Value),
                    DACrux.Base.GlobalVariable.Factory,
                    TestArea,
                    product,
                    program,
                    GetSelectedValues(dlbLot)
                    );

                if (dt == null || dt.Rows.Count <= 0)
                {
                    SetBinding(dlbPgmParam, dt);
                }
                else
                {
                    DataRow[] rows = dt.Select("[PARAM_NAME] NOT IN ('WAFER_SEQ','BIN','X','Y','SITE','TW_DIETYPE','TW_PDLFLAG','TW_TDCOUNT','TW_VALID_DATA')");
                    if (rows.Length <= 0)
                        SetBinding(dlbPgmParam, dt);
                    else SetBinding(dlbPgmParam, rows.CopyToDataTable<DataRow>());
                }

                RefrashFlag = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //--

        private void dlbLot_EnterTextBox(
            object sender,
            EventArgs e
            )
        {
            string search = dlbLot.SearchText;
            if (string.IsNullOrEmpty(search))
                return;


            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetLotInfo(
                TestArea,
                GetDateToString(dtStart),
                GetDateToString(dtEnd),
                search
                );

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("데이터를 찾을 수 없습니다.");
                return;
            }

            // PRODUCT의 경우 0번째 항목으로 선택한다.
            dlbProduct.Text = dt.Rows[0]["DEVICE"].ToString();

            // PROGRAM이 1개인 경우 선택해준다.
            dlbProgram.ClearDataSource();
            SetBinding(dlbProgram, dt, 1, 1);
            if (dt.Rows.Count == 1)
                dlbProgram.Text = dt.Rows[0]["PROGRAM"].ToString();

            RefrashFlag = true;
        }

        //--

        private void dlbPgmParam_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        //--

        private void fpRawData_AutoFilteredColumn(
            object sender,
            AutoFilteredColumnEventArgs e
            )
        {
            SheetView sheet = e.Sheet;
            //DataTable dt = null;
            FilterColumn = e.Sheet.Columns[e.Column].Label;
            FilterValue = string.Equals(e.FilterString, "(All)") || string.Equals(e.FilterString, "(NonBlanks)") ? String.Empty : e.FilterString;

            //--

            btnView.PerformClick();
        }

        private void fpRawData_AutoSortedColumn(
            object sender,
            AutoSortedColumnEventArgs e
            )
        {
            //fpRawData_Sheet1.SetColumnAllowAutoSort(e.Column, 11, false);
        }

        //--

        private void fpRawData_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            FpSpread spread = sender as FpSpread;
            if (spread == null)
                return;

            // Column Header를 선택했을 때 Parameter 변경
            if (e.ColumnHeader)
            {
                string label = spread.ActiveSheet.ColumnHeader.Columns[e.Column].Label;
                if (FindColumn(label))
                    e.Cancel = true;
                else
                    dlbPgmParam.SelectedValue = label;
            }
        }

        //--

        private void numericUpDown1_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            Utility.FPSpreadUtil.SetDecimalLength(
                fpRawData_Sheet1, (int)decimalLength.Value
                );

            fpRawData.Refresh();

            Utility.FPSpreadUtil.SetDecimalLength(
               fpStatistics_Sheet1, (int)decimalLength.Value
               );

            fpStatistics.Refresh();

            btnView.PerformClick();

            System.Windows.Forms.Application.DoEvents();
        }

        //--

        private void nudInterval_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            ChartArea cArea = chartTrend.ChartAreas[0];
            cArea.AxisX.Interval = (double)(sender as NumericUpDown).Value;
        }

        //--

        private void rbDefault_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                btnView.PerformClick();
        }

        //--

        private void tsmReset_Click(
            object sender,
            EventArgs e
            )
        {
            chartTrend.ChartAreas["CHARTAREA"].AxisX.ScaleView.ZoomReset(0);
            chartTrend.ChartAreas["CHARTAREA"].AxisY.ScaleView.ZoomReset(0);
        }

        //--

        private void tsmSetYaxis_Click(
            object sender,
            EventArgs e
            )
        {
            string strMax = chartTrend.ChartAreas["CHARTAREA"].AxisY.Maximum.ToString();
            string strMin = chartTrend.ChartAreas["CHARTAREA"].AxisY.Minimum.ToString();
            string strInterval = chartTrend.ChartAreas["CHARTAREA"].AxisY.Interval.ToString();

            PopUpAxisY dlg = new PopUpAxisY(
                strMax,
                strMin,
                strInterval
                );

            dlg = null;
            dlg = new PopUpAxisY(strMax, strMin, strInterval);
            dlg.TopMost = true;
            dlg.Owner = this.ParentForm;
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.On_Apply += new PopUpAxisY.Apply(oYAxis_On_Apply);

            dlg.ShowDialog();

        }

        private void rbArea_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            RadioButton rb = (sender as RadioButton);
            try
            {
                if (!rb.Checked) return;

                TestArea = new string[] { rb.Text };

                this.Cursor = Cursors.WaitCursor;
                dlbProduct.ClearDataSource();
                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                RO.DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionDevice(
                    GetDateToString(dtStart),
                    GetDateToString(dtEnd),
                    TestArea
                    );
                SetBinding(dlbProduct, dt);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void oYAxis_On_Apply(string Y1Max, string Y1Min, string Y1Interval, string Y2Max, string Y2Min, string Y2Interval, bool Y2Used)
        {
            double dVal = double.NaN;
            if (!double.TryParse(Y1Max, out dVal))
                dVal = double.NaN;
            chartTrend.ChartAreas["CHARTAREA"].AxisY.Maximum = dVal;

            if (!double.TryParse(Y1Min, out dVal))
                dVal = double.NaN;
            chartTrend.ChartAreas["CHARTAREA"].AxisY.Minimum = dVal;

            if (!double.TryParse(Y1Interval, out dVal))
                dVal = double.NaN;
            chartTrend.ChartAreas["CHARTAREA"].AxisY.Interval = dVal;

            if (Y2Used)
            {
                if (!double.TryParse(Y2Max, out dVal))
                    dVal = double.NaN;
                chartTrend.ChartAreas["CHARTAREA"].AxisY2.Maximum = dVal;

                if (!double.TryParse(Y2Min, out dVal))
                    dVal = double.NaN;
                chartTrend.ChartAreas["CHARTAREA"].AxisY2.Minimum = dVal;

                if (!double.TryParse(Y2Interval, out dVal))
                    dVal = double.NaN;
                chartTrend.ChartAreas["CHARTAREA"].AxisY2.Interval = dVal;
            }

            Application.DoEvents();
        }


        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private DataTable CreateFilterDataTable(
            SheetView sheet
            )
        {
            DataTable dt = new DataTable();
            DataRow[] rows = null;
            foreach (FarPoint.Win.Spread.Column col in sheet.Columns)
            {
                if (col.CellType is TextCellType)
                    dt.Columns.Add(col.Label, typeof(string));
                else if (col.CellType is NumberCellType)
                    dt.Columns.Add(col.Label, typeof(decimal));
                else if (col.CellType is DateTimeCellType)
                    dt.Columns.Add(col.Label, typeof(DateTime));
                else
                    dt.Columns.Add(col.Label, typeof(object));
            }

            for (int rowIdx = 0; rowIdx < sheet.Rows.Count; rowIdx++)
            {
                DataRow newRow = dt.NewRow();
                for (int colIdx = 0; colIdx < sheet.Columns.Count; colIdx++)
                {
                    if (sheet.Cells[rowIdx, colIdx].Value == null)
                        newRow[colIdx] = DBNull.Value;
                    else
                        newRow[colIdx] = sheet.Cells[rowIdx, colIdx].Value;
                }
                dt.Rows.Add(newRow);
            }
            dt.AcceptChanges();

            if (!string.IsNullOrEmpty(FilterValue))
            {
                rows = dt.Select(String.Format("[{0}] = '{1}'", FilterColumn, FilterValue));
                if (rows == null || rows.Length <= 0)
                    dt.Rows.Clear();
                else
                    dt = rows.CopyToDataTable<DataRow>();
            }
            dt.DefaultView.Sort = SortedColumn.ToString();
            return dt.DefaultView.ToTable();
        }

        //--

        private DataTable CreateStatisticsDataTable(
            DataTable dt
            )
        {
            DataTable dtStatistics = dt.DefaultView.ToTable(
                true,
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.LOT_ID),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.PROGRAM),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_ID),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_SEQ),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.LOT_START_TIME),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.LOT_END_TIME),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_START_TIME),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_END_TIME),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.TESTER),
                Enum.GetName(typeof(ColumnIndex), ColumnIndex.PROBE_CARD)
                );

            dtStatistics.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("LSL", typeof(decimal)),
                    new DataColumn("TARGET", typeof(decimal)),
                    new DataColumn("USL", typeof(decimal)),
                    new DataColumn("LCL", typeof(decimal)),
                    new DataColumn("UCL", typeof(decimal)),
                    new DataColumn("MININUM", typeof(double)),
                    new DataColumn("Q1", typeof(double)),
                    new DataColumn("MEDIAN", typeof(double)),
                    new DataColumn("Q3", typeof(double)),
                    new DataColumn("MAXINUM", typeof(double)),
                    new DataColumn("AVERAGE", typeof(double)),
                    new DataColumn("STDDEV", typeof(double)),
                });

            string sColumnName = dlbPgmParam.SelectedValue as String;
            double dTestLow = double.NaN;
            double dTestHigh = double.NaN;
            DataRow[] drParams = (DataSource as DataSet).Tables["PARAM_SPEC"].Select(String.Format("[PARAM_NAME] = '{0}'", dlbPgmParam.SelectedValue));
            foreach (DataRow r in dtStatistics.Rows)
            {
                DataRow[] drs = dt.Select(String.Format("[{0}] = '{1}'",
                    Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_SEQ),
                    r[Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_SEQ)]
                    ));

                List<double> dValues = new List<double>();
                for (int idx = 0; idx < drs.Length; idx++)
                {
                    double dTmp = double.NaN;
                    if (!drs[idx].Table.Columns.Contains(sColumnName))
                        continue;

                    if (!double.TryParse(drs[idx][sColumnName].ToString(), out dTmp))
                        dTmp = double.NaN;

                    if (!double.IsNaN(dTmp))
                        dValues.Add(dTmp);
                }

                if (dValues == null || dValues.Count <= 0)
                    continue;

                Tuple<double, double> dStandardDeviationAndAvg = StandardDeviationAndAverage(dValues.ToArray());
                Tuple<double, double, double> dQuertiles = Quartiles(dValues.ToArray());
                if (!String.IsNullOrEmpty(drParams[0]["TEST_HIGH"].ToString()))
                    double.TryParse(drParams[0]["TEST_HIGH"].ToString(), out dTestHigh);
                if (!String.IsNullOrEmpty(drParams[0]["TEST_LOW"].ToString()))
                    double.TryParse(drParams[0]["TEST_LOW"].ToString(), out dTestLow);

                r["LSL"] = drParams[0]["LSL"];
                r["TARGET"] = drParams[0]["TARGET"];
                r["USL"] = drParams[0]["USL"];
                r["LCL"] = drParams[0]["LCL"];
                r["UCL"] = drParams[0]["UCL"];
                r["MININUM"] = dValues.Min();
                r["Q1"] = dQuertiles.Item1;
                r["MEDIAN"] = dQuertiles.Item2;
                r["Q3"] = dQuertiles.Item3;
                r["MAXINUM"] = dValues.Max();
                r["AVERAGE"] = dStandardDeviationAndAvg.Item1;
                r["STDDEV"] = dStandardDeviationAndAvg.Item2;
            }
            return dtStatistics;
        }

        //--

        private void DrawChartByHistogram(
            DataTable dt
            )
        {
            try
            {
                String sParamName = dlbPgmParam.SelectedValue as String;
                if (!dt.Columns.Contains(sParamName))
                {
                    histogram1.DataSource = null;
                    histogram1.HistogramVisible = false;
                    return;
                }

                histogram1.DataSource = dt;
                histogram1.HistogramVisible = true;
                histogram1.DrawHistogram(sParamName);
            }
            catch
            {
            }
        }

        //--

        private void DrawChartByWafer(
            DataTable dt
            )
        {

            //--

            InitChartControl();

            //--

            ChartArea cArea = chartTrend.ChartAreas.Add("CHARTAREA");
            Legend cLegend = chartTrend.Legends.Add("LEGEND");

            //--

            Series cSeriesBoxPlot = chartTrend.Series.Add(CHART_SERIES_BOXPLOT);
            cSeriesBoxPlot.ChartType = SeriesChartType.BoxPlot;

            Series[] series = new Series[7];
            for (int idx = 0; idx < series.Length; idx++)
            {
                series[idx] = chartTrend.Series.Add(String.Format("Series-{0}", idx));
                if (Enum.GetName(typeof(SeriesStatisticsIndex), idx).Contains("POINT"))
                    series[idx].ChartType = SeriesChartType.Point;
                else
                    series[idx].ChartType = SeriesChartType.FastLine;
            }

            //--
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == typeof(decimal)
                    || col.DataType == typeof(double))
                {
                    double dTmpValue = double.NaN;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!double.TryParse(row[col].ToString(), out dTmpValue))
                            dTmpValue = double.NaN;

                        if (double.IsNaN(dTmpValue))
                            row[col] = DBNull.Value;
                        else
                            row[col] = Math.Round(dTmpValue, (int)decimalLength.Value);
                    }
                }
            }

            chartTrend.DataSource = dt;

            //--

            cArea.AxisX.Enabled = AxisEnabled.True;
            cArea.AxisX.MajorGrid.Enabled = false;
            cArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
            cArea.AxisX.MajorGrid.LineWidth = 1;
            cArea.AxisX.MajorTickMark.Enabled = false;
            cArea.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
            cArea.AxisX.LabelStyle.Enabled = true;
            cArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            cArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisX.ScaleView.Zoomable = true;
            cArea.AxisX.ScaleView.ZoomReset();
            cArea.AxisX.ScrollBar.LineColor = Color.Black;
            cArea.AxisX.ScrollBar.Size = 17;
            cArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            cArea.AxisX.Interval = (double)nudInterval.Value;
            //cArea.AxisX.IntervalOffset = double.NaN;
            cArea.AxisX.IsLabelAutoFit = true;

            cArea.CursorX.AutoScroll = true;
            cArea.CursorX.IsUserSelectionEnabled = true;
            cArea.CursorX.Interval = 0.1;
            cArea.CursorX.IntervalOffset = 0.1;
            cArea.CursorX.IntervalOffsetType = DateTimeIntervalType.Auto;


            //--

            cArea.AxisY.Enabled = AxisEnabled.True;
            cArea.AxisY.MajorGrid.Enabled = false;
            cArea.AxisY.MajorTickMark.Enabled = false;
            cArea.AxisY.LabelStyle.Enabled = true;
            //cArea.AxisY.LabelStyle.Format = GetFormat();
            cArea.AxisY.LabelStyle.Format = "{0:0." + String.Concat(Enumerable.Repeat("0", (int)decimalLength.Value)) + "}";
            cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisY.ScaleView.Zoomable = true;
            cArea.AxisY.ScaleView.ZoomReset();
            cArea.AxisY.ScrollBar.LineColor = Color.Black;
            cArea.AxisY.ScrollBar.Size = 17;
            cArea.AxisY.IsStartedFromZero = false;
            cArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            cArea.AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
            cArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            cArea.AxisY.ScaleBreakStyle.Spacing = 2;
            cArea.AxisY.ScaleBreakStyle.LineWidth = 2;
            cArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            cArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            cArea.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;


            cArea.CursorY.AutoScroll = true;
            cArea.CursorY.IsUserSelectionEnabled = true;
            cArea.CursorY.Interval = 0.1;
            cArea.CursorY.IntervalOffset = 0.1;
            cArea.CursorY.IntervalOffsetType = DateTimeIntervalType.Auto;

            //--

            cLegend.Enabled = false;

            //-

            DataPoint dpItem = null;
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                DataRow r = dt.Rows[iRow];

                if (string.IsNullOrEmpty(r["AVERAGE"].ToString()))
                    continue;

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] { 
                        r["MININUM"],
                        r["MAXINUM"], 
                        r["Q1"],
                        r["Q3"],
                        r["AVERAGE"],
                        r["MEDIAN"]
                    });

                cSeriesBoxPlot.Points.Add(
                    dpItem
                    );

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.AVERAGE)]
                    });

                series[(int)SeriesStatisticsIndex.AVERAGE].Points.Add(
                    dpItem
                    );

                series[(int)SeriesStatisticsIndex.AVERAGE_POINT].Points.Add(
                    dpItem
                    );

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.TARGET)]
                    });

                series[(int)SeriesStatisticsIndex.TARGET].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.LSL)]
                    });

                series[(int)SeriesStatisticsIndex.LSL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.USL)]
                    });

                series[(int)SeriesStatisticsIndex.USL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.LCL)]
                    });

                series[(int)SeriesStatisticsIndex.LCL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesStatisticsIndex), SeriesStatisticsIndex.UCL)]
                    });

                series[(int)SeriesStatisticsIndex.UCL].Points.Add(dpItem);
            }

            cSeriesBoxPlot.Color = Color.FromArgb(24, 127, 217);
            cSeriesBoxPlot.Enabled = uceBoxPlot.Checked;
            cSeriesBoxPlot.YAxisType = AxisType.Primary;
            cSeriesBoxPlot.IsValueShownAsLabel = false;

            series[(int)SeriesStatisticsIndex.AVERAGE].BorderWidth = 2;
            series[(int)SeriesStatisticsIndex.AVERAGE].Color = Color.FromArgb(217, 43, 4);
            series[(int)SeriesStatisticsIndex.AVERAGE].Enabled = uceAvg.Checked;
            series[(int)SeriesStatisticsIndex.AVERAGE].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.AVERAGE].IsValueShownAsLabel = false;

            series[(int)SeriesStatisticsIndex.AVERAGE_POINT].Color = Color.FromArgb(217, 43, 4);
            series[(int)SeriesStatisticsIndex.AVERAGE_POINT].Enabled = uceAvg.Checked;
            series[(int)SeriesStatisticsIndex.AVERAGE_POINT].MarkerBorderWidth = 4;
            series[(int)SeriesStatisticsIndex.AVERAGE_POINT].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.AVERAGE_POINT].IsValueShownAsLabel = chkAvgLable.Checked;

            //--

            series[(int)SeriesStatisticsIndex.TARGET].BorderWidth = 3;
            series[(int)SeriesStatisticsIndex.TARGET].Color = Color.Black;
            series[(int)SeriesStatisticsIndex.TARGET].Enabled = uceSpecLine.Checked;
            series[(int)SeriesStatisticsIndex.TARGET].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.TARGET].IsValueShownAsLabel = false;
            series[(int)SeriesStatisticsIndex.TARGET].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesStatisticsIndex.LSL].BorderWidth = 3;
            series[(int)SeriesStatisticsIndex.LSL].Color = Color.Red;
            series[(int)SeriesStatisticsIndex.LSL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesStatisticsIndex.LSL].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.LSL].IsValueShownAsLabel = false;
            series[(int)SeriesStatisticsIndex.LSL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesStatisticsIndex.USL].BorderWidth = 3;
            series[(int)SeriesStatisticsIndex.USL].Color = Color.Red;
            series[(int)SeriesStatisticsIndex.USL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesStatisticsIndex.USL].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.USL].IsValueShownAsLabel = false;
            series[(int)SeriesStatisticsIndex.USL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesStatisticsIndex.LCL].BorderWidth = 3;
            series[(int)SeriesStatisticsIndex.LCL].Color = Color.Blue;
            series[(int)SeriesStatisticsIndex.LCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesStatisticsIndex.LCL].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.LCL].IsValueShownAsLabel = false;
            series[(int)SeriesStatisticsIndex.LCL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesStatisticsIndex.UCL].BorderWidth = 3;
            series[(int)SeriesStatisticsIndex.UCL].Color = Color.Blue;
            series[(int)SeriesStatisticsIndex.UCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesStatisticsIndex.UCL].YAxisType = AxisType.Primary;
            series[(int)SeriesStatisticsIndex.UCL].IsValueShownAsLabel = false;
            series[(int)SeriesStatisticsIndex.UCL].BorderDashStyle = ChartDashStyle.Dot;

            chartTrend.DataBind();
            chartTrend.Invalidate();

            chartTrend.Update();

            System.Windows.Forms.Application.DoEvents();
        }

        //--

        private void DrawChartByWaferRawData(
            DataTable dt
            )
        {
            DataSet ds = DataSource as DataSet;
            if (ds == null || ds.Tables == null || ds.Tables.Count <= 0)
                return;

            //--

            string columnName = dlbPgmParam.SelectedValue as String;
            InitChartControl();

            //--
            if (!dt.Columns.Contains(columnName))
                return;

            if (dt.Columns[columnName].DataType == typeof(decimal)
                || dt.Columns[columnName].DataType == typeof(double))
            {
                double dTmpValue = double.NaN;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!double.TryParse(dr[columnName].ToString(), out dTmpValue))
                        dTmpValue = double.NaN;

                    if (double.IsNaN(dTmpValue))
                        dr[columnName] = DBNull.Value;
                    else
                        dr[columnName] = Math.Round(dTmpValue, (int)decimalLength.Value);
                }
            }

            DataRow[] rows = ds.Tables["PARAM_SPEC"].Select(String.Format("[PARAM_NAME] = '{0}'", columnName));
            ChartArea cArea = chartTrend.ChartAreas.Add("CHARTAREA");
            Legend cLegend = chartTrend.Legends.Add("LEGEND");

            Series[] series = new Series[7];
            for (int idx = 0; idx < series.Length; idx++)
            {
                series[idx] = chartTrend.Series.Add(String.Format("Series-{0}", idx));
                if (Enum.GetName(typeof(SeriesRawDataIndex), idx).Contains("POINT"))
                    series[idx].ChartType = SeriesChartType.Point;
                else
                    series[idx].ChartType = SeriesChartType.FastLine;
            }
            chartTrend.DataSource = dt;

            //--

            cArea.AxisX.Enabled = AxisEnabled.True;
            cArea.AxisX.MajorGrid.Enabled = true;
            cArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
            cArea.AxisX.MajorGrid.LineWidth = 1;
            cArea.AxisX.MajorTickMark.Enabled = false;
            cArea.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
            cArea.AxisX.LabelStyle.Enabled = true;
            cArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            cArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisX.ScaleView.Zoomable = true;
            cArea.AxisX.ScaleView.ZoomReset();
            cArea.AxisX.ScrollBar.LineColor = Color.Black;
            cArea.AxisX.ScrollBar.Size = 17;
            cArea.CursorX.AutoScroll = true;
            cArea.CursorX.IsUserSelectionEnabled = true;
            cArea.CursorX.Interval = 0.1;
            cArea.CursorX.IntervalOffset = 0.1;
            cArea.CursorX.IntervalOffsetType = DateTimeIntervalType.Auto;

            cArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            cArea.AxisX.Interval = (double)nudInterval.Value;
            //cArea.AxisX.IntervalOffset = 1;
            cArea.AxisX.IsLabelAutoFit = true;

            //--

            cArea.AxisY.Enabled = AxisEnabled.True;
            cArea.AxisY.MajorGrid.Enabled = false;
            cArea.AxisY.MajorTickMark.Enabled = false;
            cArea.AxisY.LabelStyle.Enabled = true;
            //cArea.AxisY.LabelStyle.Format = GetFormat();
            cArea.AxisY.LabelStyle.Format = "{0:0." + String.Concat(Enumerable.Repeat("0", (int)decimalLength.Value)) + "}";
            cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisY.ScaleView.Zoomable = true;
            cArea.AxisY.ScaleView.ZoomReset();
            cArea.AxisY.ScrollBar.LineColor = Color.Black;
            cArea.AxisY.ScrollBar.Size = 17;
            cArea.AxisY.IsStartedFromZero = false;
            cArea.CursorY.AutoScroll = false;
            cArea.CursorY.IsUserSelectionEnabled = false;
            cArea.CursorY.Interval = 0.1;
            cArea.CursorY.IntervalOffset = 0.1;
            cArea.CursorY.IntervalOffsetType = DateTimeIntervalType.Auto;

            cArea.AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
            cArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            cArea.AxisY.ScaleBreakStyle.Spacing = 2;
            cArea.AxisY.ScaleBreakStyle.LineWidth = 2;
            cArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            cArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            cArea.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;

            //--

            cLegend.Enabled = false;

            DataPoint dp = null;
            foreach (DataRow r in dt.Rows)
            {

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[columnName]
                    });
                series[(int)SeriesRawDataIndex.RAW_DATA].Points.Add(dp);
                dp.ToolTip = String.Format("WAFER ID: {0}\r\nDIENUM: {1}\r\nVALUE: {2}", r[Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_ID)], r[Enum.GetName(typeof(ColumnIndex), ColumnIndex.DIE_NUM)], r[columnName]);
                series[(int)SeriesRawDataIndex.RAW_DATA_POINT].Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        rows[0]["TARGET"]
                    });
                series[(int)SeriesRawDataIndex.TARGET].Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        rows[0]["USL"]
                    });
                series[(int)SeriesRawDataIndex.USL].Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        rows[0]["LSL"]
                    });
                series[(int)SeriesRawDataIndex.LSL].Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        rows[0]["UCL"]
                    });
                series[(int)SeriesRawDataIndex.UCL].Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        rows[0]["LCL"]
                    });
                series[(int)SeriesRawDataIndex.LCL].Points.Add(dp);
            }

            series[(int)SeriesRawDataIndex.RAW_DATA].BorderWidth = 1;
            series[(int)SeriesRawDataIndex.RAW_DATA].BorderColor = Color.DeepPink;

            series[(int)SeriesRawDataIndex.RAW_DATA_POINT].MarkerStyle = MarkerStyle.Circle;
            series[(int)SeriesRawDataIndex.RAW_DATA_POINT].MarkerBorderWidth = 3;
            series[(int)SeriesRawDataIndex.RAW_DATA_POINT].BorderColor = Color.DeepPink;
            series[(int)SeriesRawDataIndex.RAW_DATA_POINT].IsValueShownAsLabel = this.chkRawDataLabel.Checked;

            //--

            series[(int)SeriesRawDataIndex.TARGET].BorderWidth = 3;
            series[(int)SeriesRawDataIndex.TARGET].Color = Color.Black;
            series[(int)SeriesRawDataIndex.TARGET].Enabled = uceSpecLine.Checked;
            series[(int)SeriesRawDataIndex.TARGET].YAxisType = AxisType.Primary;
            series[(int)SeriesRawDataIndex.TARGET].IsValueShownAsLabel = false;
            series[(int)SeriesRawDataIndex.TARGET].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesRawDataIndex.LSL].BorderWidth = 3;
            series[(int)SeriesRawDataIndex.LSL].Color = Color.Red;
            series[(int)SeriesRawDataIndex.LSL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesRawDataIndex.LSL].YAxisType = AxisType.Primary;
            series[(int)SeriesRawDataIndex.LSL].IsValueShownAsLabel = false;
            series[(int)SeriesRawDataIndex.LSL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesRawDataIndex.USL].BorderWidth = 3;
            series[(int)SeriesRawDataIndex.USL].Color = Color.Red;
            series[(int)SeriesRawDataIndex.USL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesRawDataIndex.USL].YAxisType = AxisType.Primary;
            series[(int)SeriesRawDataIndex.USL].IsValueShownAsLabel = false;
            series[(int)SeriesRawDataIndex.USL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesRawDataIndex.LCL].BorderWidth = 3;
            series[(int)SeriesRawDataIndex.LCL].Color = Color.Blue;
            series[(int)SeriesRawDataIndex.LCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesRawDataIndex.LCL].YAxisType = AxisType.Primary;
            series[(int)SeriesRawDataIndex.LCL].IsValueShownAsLabel = false;
            series[(int)SeriesRawDataIndex.LCL].BorderDashStyle = ChartDashStyle.Dot;

            series[(int)SeriesRawDataIndex.UCL].BorderWidth = 3;
            series[(int)SeriesRawDataIndex.UCL].Color = Color.Blue;
            series[(int)SeriesRawDataIndex.UCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesRawDataIndex.UCL].YAxisType = AxisType.Primary;
            series[(int)SeriesRawDataIndex.UCL].IsValueShownAsLabel = false;
            series[(int)SeriesRawDataIndex.UCL].BorderDashStyle = ChartDashStyle.Dot;

            chartTrend.DataBind();
            chartTrend.Invalidate();

            chartTrend.Update();

            System.Windows.Forms.Application.DoEvents();
        }

        //--

        public void ExportExcel(
            )
        {
            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.Add(
                new TextObject("PCM Report", 20, true, Color.Black),
                new TextObject(string.Format("From Date: {0}", dtStart.Value), 10),
                new TextObject(string.Format("To Date: {0}", dtEnd.Value), 10),
                new TextObject(string.Format("Product: {0}", dlbProduct.SelectedValue), 10),
                new TextObject(string.Format("Program: {0}", dlbProgram.SelectedValue), 10),
                new TextObject(string.Format("Lot ID: {0}", string.Join(",", dlbLot.SelectedTexts)), 10),
                new TextObject(string.Format("Item: {0}", dlbPgmParam.SelectedValue), 10)
            );
            sheet1.Add();
            sheet1.Add(histogram1, chartTrend);

            ExcelSheet sheet2 = new ExcelSheet();
            sheet2.Add(fpRawData);

            ExcelSheet sheet3 = new ExcelSheet();
            sheet3.Add(fpStatistics);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);
            e.SheetList.Add(sheet2);
            e.SheetList.Add(sheet3);

            ExcelExportManager.Export(e);
        }

        //--

        private bool FillStatisticsData(
            DataTable dt
            )
        {
            Utility.FPSpreadUtil.InitSpread(
                   fpStatistics
                   );
            Utility.FPSpreadUtil.SetSpreadData(
                dt,
                fpStatistics_Sheet1,
                10,
                (int)decimalLength.Value
                );
            fpStatistics_Sheet1.OperationMode = OperationMode.ReadOnly;
            Utility.FPSpreadUtil.SetDecimalLength(
                fpStatistics_Sheet1,
                (int)decimalLength.Value
                );
            Utility.FPSpreadUtil.SetAutoColumnWidth(
                fpStatistics_Sheet1
                );
            Utility.FPSpreadUtil.VisibleSpreadColumns(
                fpStatistics_Sheet1,
                new string[] { Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_SEQ), Enum.GetName(typeof(ColumnIndex), ColumnIndex.DIEPROBE_CNT) },
                false
                );
            return true;
        }

        //--

        private void FillRawData(
            DataTable dt
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpRawData);
            Utility.FPSpreadUtil.SetSpreadData(dt, fpRawData_Sheet1, 100, (int)decimalLength.Value);

            //-- DIE NUM 컬럼의 CellType = NumberCellType
            NumberCellType numberCell = new NumberCellType();
            numberCell.DecimalPlaces = 0;
            numberCell.MaximumValue = 99999999999999;
            numberCell.MinimumValue = -99999999999999;
            numberCell.FixedPoint = false;
            numberCell.NegativeRed = false;
            fpRawData_Sheet1.Columns[(int)ColumnIndex.DIE_NUM].CellType = numberCell;
            Utility.FPSpreadUtil.VisibleSpreadColumns(fpRawData_Sheet1, new string[] { Enum.GetName(typeof(ColumnIndex), ColumnIndex.WAFER_SEQ), Enum.GetName(typeof(ColumnIndex), ColumnIndex.DIEPROBE_CNT) }, false);
            fpRawData_Sheet1.OperationMode = OperationMode.ReadOnly;
            fpRawData_Sheet1.FrozenColumnCount = (int)ColumnIndex.DIEPROBE_CNT;

            //--

            Utility.FPSpreadUtil.SetAutoColumnFilter(
                fpRawData_Sheet1,
                true,
                ColumnIndex.LOT_ID,
                ColumnIndex.PROGRAM,
                ColumnIndex.WAFER_ID,
                ColumnIndex.LOT_START_TIME,
                ColumnIndex.LOT_END_TIME,
                ColumnIndex.WAFER_START_TIME,
                ColumnIndex.WAFER_END_TIME,
                ColumnIndex.TESTER,
                ColumnIndex.PROBE_CARD,
                ColumnIndex.OPERATOR,
                ColumnIndex.DIE_NUM,
                ColumnIndex.DIEPROBE_CNT
                );
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1);
        }

        //--

        private bool FindColumn(
            string column
            )
        {
            bool flag = false;
            Array array = Enum.GetValues(typeof(ColumnIndex));
            for (int i = 0; i < array.Length; i++)
            {
                flag = false;
                if (string.Equals(array.GetValue(i).ToString(), column))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        //--

        private string GetFormat(
            )
        {
            StringBuilder sbFormat = new StringBuilder();
            for (int idx = 0; idx < (int)decimalLength.Value; idx++)
                sbFormat.Append("#");
            return string.Format("#.{0}", sbFormat.ToString());
        }

        //--

        private string GetDateToString(
            DateTimePicker datepicker
            )
        {
            return datepicker.Value.ToString(DATE_FORMAT);
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

        private bool GetRawData(
            )
        {
            TestCommon oTestComm = new TestCommon();

            DataSet ds = oTestComm.GetPCMReport_Comp(
                Convert.ToDateTime(dtStart.Value),
                Convert.ToDateTime(dtEnd.Value),
                DACrux.Base.GlobalVariable.Factory,
                TestArea,
                dlbProduct.SelectedValue as String,
                dlbProgram.SelectedValue as String,
                dlbPgmParam.SelectedValue as String,
                GetSelectedValues(dlbLot)
                );

            DataSource = ds;

            FillRawData(
                GetRawData(ds.Tables["RAW_DATA"])
                );

            return true;
        }

        //--

        private DataTable GetRawData(
            DataTable dt
            )
        {
            DataRow[] drsParams = null;
            double dTestHigh = 0;
            double dTestLow = 0;
            double dValue = 0;
            #region Filter RawData
            for (int colIdx = dt.Columns.Count - 1; colIdx >= 0; colIdx--)
            {
                if (FindColumn(dt.Columns[colIdx].ColumnName))
                    continue;

                DataRow[] rows = dt.Select(String.Format("[{0}] IS NOT NULL", dt.Columns[colIdx].ColumnName));

                if (rows == null || rows.Length <= 0)
                {
                    dt.Columns.Remove(dt.Columns[colIdx].ColumnName);
                    continue;
                }

                drsParams = (DataSource as DataSet).Tables["PARAM_SPEC"].Select(String.Format("[PARAM_NAME] = '{0}'", dt.Columns[colIdx].ColumnName));
                if (drsParams == null || drsParams.Length <= 0)
                    continue;

                if (String.IsNullOrEmpty(drsParams[0]["TEST_HIGH"].ToString()))
                    dTestHigh = 8999999488;
                else if (!double.TryParse(drsParams[0]["TEST_HIGH"].ToString(), out dTestHigh))
                    dTestHigh = 8999999488;

                if (String.IsNullOrEmpty(drsParams[0]["TEST_LOW"].ToString()))
                    dTestLow = -8999999488;
                else if (!double.TryParse(drsParams[0]["TEST_LOW"].ToString(), out dTestLow))
                    dTestLow = -8999999488;

                foreach (DataRow row in dt.Rows)
                {
                    if (String.IsNullOrEmpty(row[dt.Columns[colIdx].ColumnName].ToString()))
                    {
                        row[dt.Columns[colIdx].ColumnName] = DBNull.Value;
                        continue;
                    }

                    if (!double.TryParse(row[dt.Columns[colIdx].ColumnName].ToString(), out dValue))
                        dValue = double.NaN;

                    if (dTestLow > dValue || dTestHigh < dValue)
                        row[dt.Columns[colIdx].ColumnName] = dValue * 0.1;
                }
            }
            #endregion Filter RawData

            #region Clone RawData
            DataTable dtClone = dt.Clone();
            // GetDataTable
            dtClone.Columns[Enum.GetName(typeof(ColumnIndex), ColumnIndex.DIE_NUM)].DataType = Type.GetType("System.Int32");
            foreach (DataRow row in dt.Rows)
            {
                dtClone.ImportRow(row);
            }
            dtClone.AcceptChanges();
            dt = dtClone;
            #endregion Clone RawData

            DataView dv = new DataView(dt);
            dv.Sort = SortedColumn.ToString();
            return dv.ToTable();
        }

        //--

        private void InitChartControl(
            )
        {
            chartTrend.Annotations.Clear();
            chartTrend.ChartAreas.Clear();
            chartTrend.Legends.Clear();
            chartTrend.Series.Clear();
        }


        //--

        private void SetBinding(
            DUCListBox listbox,
            DataTable source,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (source != null)
            {
                listbox.DisplayMember = source.Columns[dispIndex].ColumnName;
                listbox.ValueMember = source.Columns[valueIndex].ColumnName;
            }

            listbox.DataSource = source;
        }

        //--

        private Tuple<double, double> StandardDeviationAndAverage(
            double[] afVal
            )
        {
            double average = afVal.Average();
            double sumOfDeviation = 0;
            double stddev = double.NaN;
            foreach (double value in afVal)
            {
                sumOfDeviation += (value) * (value);
            }

            double sumOfDeviationAverage = sumOfDeviation / (afVal.Length - 1);
            if (double.IsNegativeInfinity(sumOfDeviationAverage))
                stddev = double.NegativeInfinity;
            else if (double.IsPositiveInfinity(sumOfDeviationAverage))
                stddev = double.PositiveInfinity;
            else if (double.IsNaN(sumOfDeviationAverage))
                stddev = double.NaN;
            else
                stddev = Math.Sqrt(sumOfDeviationAverage - (average * average));

            return new Tuple<double, double>(average, stddev);
        }

        /// <summary>
        /// Return the quartile values of an ordered set of doubles
        ///   assume the sorting has already been done.
        ///   
        /// This actually turns out to be a bit of a PITA, because there is no universal agreement 
        ///   on choosing the quartile values. In the case of odd values, some count the median value
        ///   in finding the 1st and 3rd quartile and some discard the median value. 
        ///   the two different methods result in two different answers.
        ///   The below method produces the arithmatic mean of the two methods, and insures the median
        ///   is given it's correct weight so that the median changes as smoothly as possible as 
        ///   more data ppints are added.
        ///    
        /// This method uses the following logic:
        /// 
        /// ===If there are an even number of data points:
        ///    Use the median to divide the ordered data set into two halves. 
        ///    The lower quartile value is the median of the lower half of the data. 
        ///    The upper quartile value is the median of the upper half of the data.
        ///    
        /// ===If there are (4n+1) data points:
        ///    The lower quartile is 25% of the nth data value plus 75% of the (n+1)th data value.
        ///    The upper quartile is 75% of the (3n+1)th data point plus 25% of the (3n+2)th data point.
        ///    
        ///===If there are (4n+3) data points:
        ///   The lower quartile is 75% of the (n+1)th data value plus 25% of the (n+2)th data value.
        ///   The upper quartile is 25% of the (3n+2)th data point plus 75% of the (3n+3)th data point.
        /// 
        /// </summary>
        private Tuple<double, double, double> Quartiles(
            double[] afVal
            )
        {
            int iSize = afVal.Length;
            int iMid = iSize / 2; //this is the mid from a zero based index, eg mid of 7 = 3;

            double fQ1 = 0;
            double fQ2 = 0;
            double fQ3 = 0;

            Array.Sort(afVal);

            if (iSize % 2 == 0)
            {
                //================ EVEN NUMBER OF POINTS: =====================
                //even between low and high point
                fQ2 = (afVal[iMid - 1] + afVal[iMid]) / 2;

                int iMidMid = iMid / 2;

                //easy split 
                if (iMid % 2 == 0)
                {
                    fQ1 = (afVal[iMidMid - 1] + afVal[iMidMid]) / 2;
                    fQ3 = (afVal[iMid + iMidMid - 1] + afVal[iMid + iMidMid]) / 2;
                }
                else
                {
                    fQ1 = afVal[iMidMid];
                    fQ3 = afVal[iMidMid + iMid];
                }
            }
            else if (iSize == 1)
            {
                //================= special case, sorry ================
                fQ1 = afVal[0];
                fQ2 = afVal[0];
                fQ3 = afVal[0];
            }
            else
            {
                //odd number so the median is just the midpoint in the array.
                fQ2 = afVal[iMid];

                if ((iSize - 1) % 4 == 0)
                {
                    //======================(4n-1) POINTS =========================
                    int n = (iSize - 1) / 4;
                    fQ1 = (afVal[n - 1] * .25) + (afVal[n] * .75);
                    fQ3 = (afVal[3 * n] * .75) + (afVal[3 * n + 1] * .25);
                }
                else if ((iSize - 3) % 4 == 0)
                {
                    //======================(4n-3) POINTS =========================
                    int n = (iSize - 3) / 4;

                    fQ1 = (afVal[n] * .75) + (afVal[n + 1] * .25);
                    fQ3 = (afVal[3 * n + 1] * .25) + (afVal[3 * n + 2] * .75);
                }
            }

            return new Tuple<double, double, double>(fQ1, fQ2, fQ3);
        }

        //--

        #endregion [ Method ]

        //--

        #region [ Properties ]

        public object DataSource { get; set; }

        #endregion [ Properties ]
    }
}

