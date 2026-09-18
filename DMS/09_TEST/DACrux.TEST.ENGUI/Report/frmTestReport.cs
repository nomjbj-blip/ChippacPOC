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
using System.Windows.Forms;
using DACrux.Utility;
using DACrux.TEST.Control;

namespace DACrux.TEST.ENGUI
{
    public partial class frmTestReport
        : DACruxUXBasic01, IExportExcel
    {
        private enum Columns
        {
            WAFER_SEQ = 0,
            WAFER_ID = 1,
            YIELD,
            LSL,
            USL,
            LCL,
            UCL,
            COUNT,
            MININUM,
            Q1,
            MEDIAN,
            Q3,
            MAXINUM,
            AVERAGE,
            STDDEV
        }

        private enum SeriesIndex
        {
            AVERAGE = 0,
            AVERAGE_POINT,
            YIELD,
            YIELD_POINT,
            TARGET,
            LSL,
            USL,
            LCL,
            UCL
        }

        private readonly String CHART_SERIES_BOXPLOT = "BOXPLOT";
        private readonly string AVERAGE_COLOR_HEX = "#067302";
        private readonly string BOXPLOT_COLOR_HEX = "#7EE7F2";
        private readonly string YIELD_COLOR_HEX = "#8C0343";
        private readonly string SPEC_COLOR_HEX = "#A60321";
        private readonly string CONTROL_COLOR_HEX = "#D9B60B";
        private static readonly string DATE_FORMAT = "yyyyMMdd";
        private bool isRefrash = false;

        //--


        public frmTestReport()
        {
            InitializeComponent();
        }

        //--

        #region [ Event Handler ]

        //--

        private void frmTestReport_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            fromDate.Value = DateTime.Now.AddDays(-1);
            toDate.Value = DateTime.Now;

            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = oProbeAdmin.GetTestAreaListNotPCM();

            SetBinding(
                dlbTestArea,
                dt
                );

            Utility.FPSpreadUtil.InitSpread(
                fpRawData
                );
            Utility.FPSpreadUtil.InitSpread(
                fpStatistics
                );
            Utility.FPSpreadUtil.SetSpreadData(
                CreateDataTableByWafer(),
                fpStatistics_Sheet1,
                100,
                4
                );
            Utility.FPSpreadUtil.SetAutoColumnWidth(
                fpStatistics_Sheet1
                );
            fpStatistics_Sheet1.OperationMode = OperationMode.ReadOnly;

            uceAvg.ForeColor = ColorTranslator.FromHtml(AVERAGE_COLOR_HEX);
            uceYield.ForeColor = ColorTranslator.FromHtml(YIELD_COLOR_HEX);
            uceSpecLine.ForeColor = ColorTranslator.FromHtml(SPEC_COLOR_HEX);
            uceControlLine.ForeColor = ColorTranslator.FromHtml(CONTROL_COLOR_HEX);
        }

        //--

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (GetData())
                {
                    MainForm.SetStatusMessage("Data를 처리 중입니다.");
                    DrawChartByLot();
                    DrawChartByWafer();
                }
            }
            finally
            {
                MainForm.SetStatusMessage(null);
                this.Cursor = Cursors.Default;
            }
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
                dlbTestArea.ClearDataSource();
                dlbProduct.ClearDataSource();
                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                ProbeAdmin oProbeAdmin = new ProbeAdmin();
                DataTable dt = oProbeAdmin.GetTestAreaListNotPCM();
                SetBinding(
                    dlbTestArea,
                    dt
                    );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //--

        private void dlbTestArea_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dlbTestArea.SelectedIndex < 0)
                    return;

                String[] testarea = GetSelectedValues(dlbTestArea);
                if (testarea == null || testarea.Length <= 0)
                    return;

                dlbProduct.ClearDataSource();
                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionDevice(
                    GetDateToString(fromDate, -1), 
                    GetDateToString(toDate, 1), 
                    testarea
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

                String[] testarea = GetSelectedValues(dlbTestArea);
                if (testarea == null || testarea.Length <= 0)
                    return;

                dlbProgram.ClearDataSource();
                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionProgram(
                    GetDateToString(fromDate, -1),
                    GetDateToString(toDate, 1), 
                    testarea, 
                    GetSelectedValues(dlbProduct)
                    );
                SetBinding(dlbProgram, dt);
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

                String[] testarea = GetSelectedValues(dlbTestArea);
                if (testarea == null || testarea.Length <= 0)
                    return;

                string search = String.Empty;
                if (!String.IsNullOrEmpty(dlbLot.SearchText))
                    search = dlbLot.SearchText;


                dlbLot.ClearDataSource();
                dlbPgmParam.ClearDataSource();

                DataSelect obj = new DataSelect();
                DataTable dt = obj.GetConditionLot(
                    GetDateToString(fromDate, -1),
                    GetDateToString(toDate, 1), 
                    testarea,
                    GetSelectedValues(dlbProduct),
                    GetSelectedValues(dlbProgram)
                    );
                SetBinding(dlbLot, dt, 0, 1);
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

                dlbPgmParam.ClearDataSource();

                String[] testarea = GetSelectedValues(dlbTestArea);
                if (testarea == null || testarea.Length <= 0)
                    return;

                String product = dlbProduct.SelectedValue as String;
                if (String.IsNullOrEmpty(product))
                    return;

                String program = dlbProgram.SelectedValue as String;
                if (String.IsNullOrEmpty(program))
                    return;

                ProbeAdmin oProbeAdmin = new ProbeAdmin();
                DataTable dt = oProbeAdmin.GetParaSpecList(
                    Convert.ToDateTime(fromDate.Value.AddDays(-1)),
                    Convert.ToDateTime(toDate.Value.AddDays(1)),
                    DACrux.Base.GlobalVariable.Factory,
                    testarea,
                    product,
                    program,
                    GetSelectedValues(dlbLot)
                    );

                SetBinding(dlbPgmParam, dt);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dlbLot_EnterTextBox(
            object sender,
            EventArgs e
            )
        {
            string search = dlbLot.SearchText;
            if (string.IsNullOrEmpty(search))
                return;

            string[] testarea =  GetSelectedValues(dlbTestArea);
            if(testarea == null || testarea.Length <= 0)
                return;

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetLotInfo(
                testarea, 
                GetDateToString(fromDate, -1), 
                GetDateToString(toDate, 1), 
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

            //dlbLot.SearchText = search;
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

        private void fpRawData_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            // Column Header를 선택했을 때 Parameter 변경
            if (e.ColumnHeader)
            {
                string label = fpRawData_Sheet1.ColumnHeader.Columns[e.Column].Label;
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
                fpRawData_Sheet1, (int)numericUpDown1.Value
                );
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

        private void tsmSetYAxis_Click(
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

        //--

        private void uceSeriesOption_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            //btnView.PerformClick();
            DrawChartByWafer();
        }

        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private DataTable CreateDataTableByWafer(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("WAFER_SEQ", typeof(decimal)),
                    new DataColumn("WAFER_ID", typeof(String)),
                    new DataColumn("YIELD", typeof(decimal)),
                    new DataColumn("LSL", typeof(decimal)),
                    new DataColumn("USL", typeof(decimal)),
                    new DataColumn("LCL", typeof(decimal)),
                    new DataColumn("UCL", typeof(decimal)),
                    new DataColumn("COUNT", typeof(decimal)),
                    new DataColumn("MININUM", typeof(decimal)),
                    new DataColumn("Q1", typeof(decimal)),
                    new DataColumn("MEDIAN", typeof(decimal)),
                    new DataColumn("Q3", typeof(decimal)),
                    new DataColumn("MAXINUM", typeof(decimal)),
                    new DataColumn("AVERAGE", typeof(decimal)),
                    new DataColumn("STDDEV", typeof(decimal))
                });

            return dt;
        }

        //--

        private bool DefaultRawColumn(
            string columnName
            )
        {
            bool bFlag = false;

            switch (columnName)
            {
                case "LOT_ID":
                case "PROGRAM":
                case "WAFER_ID":
                case "LOT_START_TIME":
                case "LOT_END_TIME":
                case "WAFER_START_TIME":
                case "WAFER_END_TIME":
                case "TESTER":
                case "PROBE_CARD":
                case "YIELD":
                case "WAFER_SEQ":
                case "DIE_NUM":
                    bFlag = true;
                    break;
                default:
                    bFlag = false;
                    break;
            }

            return bFlag;
        }

        //--

        private void DrawChartByLot(
            )
        {
            DataTable dt = fpRawData_Sheet1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count <= 0)
                return;

            //--

            String sParamName = dlbPgmParam.SelectedValue as String;

            //--

            dt = dt.DefaultView.ToTable(false, sParamName);

            try
            {
                histogram1.DataSource = dt;
                histogram1.HistogramVisible = true;
                histogram1.DrawHistogram(sParamName);
            }
            catch
            { }
        }

        //--

        private void DrawChartByWafer(
            )
        {
            DataTable dt = fpStatistics_Sheet1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count <= 0) return;

            //--

            InitChartControl();

            //--

            chartTrend.DataSource = dt;

            //--

            ChartArea cArea = chartTrend.ChartAreas.Add("CHARTAREA");
            Legend cLegend = chartTrend.Legends.Add("LEGEND");

            //--

            Series cSeriesBoxPlot = chartTrend.Series.Add(CHART_SERIES_BOXPLOT);
            cSeriesBoxPlot.ChartType = SeriesChartType.BoxPlot;

            Series[] series = new Series[9];
            for (int idx = 0; idx < series.Length; idx++)
            {
                series[idx] = chartTrend.Series.Add(string.Format("Series-{0}", idx));
                if (Enum.GetName(typeof(SeriesIndex), idx).Contains("POINT"))
                    series[idx].ChartType = SeriesChartType.FastPoint;
                else
                    series[idx].ChartType = SeriesChartType.FastLine;
            }

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
            cArea.CursorX.AutoScroll = true;
            cArea.CursorX.IsUserSelectionEnabled = true;

            cArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            cArea.AxisX.Interval = 1;
            cArea.AxisX.IntervalOffset = double.NaN;
            cArea.AxisX.IsLabelAutoFit = true;

            //--

            cArea.AxisY.Enabled = AxisEnabled.True;
            cArea.AxisY.MajorGrid.Enabled = false;
            cArea.AxisY.MajorTickMark.Enabled = false;
            cArea.AxisY.LabelStyle.Enabled = true;
            cArea.AxisY.LabelStyle.Format = "#.###";
            cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisY.ScaleView.Zoomable = true;
            cArea.AxisY.ScaleView.ZoomReset();
            cArea.AxisY.ScrollBar.LineColor = Color.Black;
            cArea.AxisY.ScrollBar.Size = 17;
            cArea.AxisY.IsStartedFromZero = false;
            cArea.CursorY.AutoScroll = true;
            cArea.CursorY.IsUserSelectionEnabled = true;

            //--

            cArea.AxisY2.Enabled = AxisEnabled.True;
            cArea.AxisY2.MajorGrid.Enabled = false;
            cArea.AxisY2.MajorTickMark.Enabled = false;
            cArea.AxisY2.Maximum = 100;
            cArea.AxisY2.Minimum = 0;
            cArea.AxisY2.LabelStyle.Enabled = true;
            cArea.AxisY2.IsStartedFromZero = cArea.AxisY.IsStartedFromZero = false;

            //--

            cLegend.Enabled = false;

            //--

            DataPoint dpItem = null;
            double dTmp = double.NaN;
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                DataRow r = dt.Rows[iRow];

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
                        r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.AVERAGE)]
                    });
                series[(int)SeriesIndex.AVERAGE].Points.Add(dpItem);
                series[(int)SeriesIndex.AVERAGE_POINT].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                        r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.YIELD)]
                    });
                series[(int)SeriesIndex.YIELD].Points.Add(dpItem);
                series[(int)SeriesIndex.YIELD_POINT].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                       r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.TARGET)]
                    });
                series[(int)SeriesIndex.TARGET].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                       r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.LSL)]
                    });
                series[(int)SeriesIndex.LSL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                       r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.USL)]
                    });
                series[(int)SeriesIndex.USL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                       r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.LCL)]
                    });
                series[(int)SeriesIndex.LCL].Points.Add(dpItem);

                //--

                dpItem = new DataPoint();
                dpItem.SetValueXY(
                    r["WAFER_ID"],
                    new object[] {
                       r[Enum.GetName(typeof(SeriesIndex), SeriesIndex.UCL)]
                    });
                series[(int)SeriesIndex.UCL].Points.Add(dpItem);
            }

            //--

            cSeriesBoxPlot.Color = ColorTranslator.FromHtml(BOXPLOT_COLOR_HEX);
            cSeriesBoxPlot.YAxisType = AxisType.Primary;
            cSeriesBoxPlot.IsValueShownAsLabel = false;

            //--

            series[(int)SeriesIndex.AVERAGE].BorderWidth = 2;
            series[(int)SeriesIndex.AVERAGE].Color = ColorTranslator.FromHtml(AVERAGE_COLOR_HEX);
            series[(int)SeriesIndex.AVERAGE].Enabled = uceAvg.Checked;
            series[(int)SeriesIndex.AVERAGE].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.AVERAGE].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.AVERAGE_POINT].BorderWidth = 2;
            series[(int)SeriesIndex.AVERAGE_POINT].Color = ColorTranslator.FromHtml(AVERAGE_COLOR_HEX);
            series[(int)SeriesIndex.AVERAGE_POINT].Enabled = uceAvg.Checked;
            series[(int)SeriesIndex.AVERAGE_POINT].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.AVERAGE_POINT].IsValueShownAsLabel = false;

            //--

            series[(int)SeriesIndex.YIELD].Color = ColorTranslator.FromHtml(YIELD_COLOR_HEX);
            series[(int)SeriesIndex.YIELD].Enabled = uceYield.Checked;
            series[(int)SeriesIndex.YIELD].MarkerBorderWidth = 4;
            series[(int)SeriesIndex.YIELD].YAxisType = AxisType.Secondary;
            series[(int)SeriesIndex.YIELD].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.YIELD_POINT].Color = ColorTranslator.FromHtml(YIELD_COLOR_HEX);
            series[(int)SeriesIndex.YIELD_POINT].Enabled = uceYield.Checked;
            series[(int)SeriesIndex.YIELD_POINT].MarkerBorderWidth = 4;
            series[(int)SeriesIndex.YIELD_POINT].YAxisType = AxisType.Secondary;
            series[(int)SeriesIndex.YIELD_POINT].IsValueShownAsLabel = false;

            //--

            series[(int)SeriesIndex.TARGET].BorderWidth = 3;
            series[(int)SeriesIndex.TARGET].Color = ColorTranslator.FromHtml(SPEC_COLOR_HEX);
            series[(int)SeriesIndex.TARGET].Enabled = uceSpecLine.Checked;
            series[(int)SeriesIndex.TARGET].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.TARGET].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.LSL].BorderWidth = 3;
            series[(int)SeriesIndex.LSL].Color = ColorTranslator.FromHtml(SPEC_COLOR_HEX);
            series[(int)SeriesIndex.LSL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesIndex.LSL].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.LSL].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.USL].BorderWidth = 3;
            series[(int)SeriesIndex.USL].Color = ColorTranslator.FromHtml(SPEC_COLOR_HEX);
            series[(int)SeriesIndex.USL].Enabled = uceSpecLine.Checked;
            series[(int)SeriesIndex.USL].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.USL].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.LCL].BorderWidth = 3;
            series[(int)SeriesIndex.LCL].Color = ColorTranslator.FromHtml(CONTROL_COLOR_HEX);
            series[(int)SeriesIndex.LCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesIndex.LCL].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.LCL].IsValueShownAsLabel = false;

            series[(int)SeriesIndex.UCL].BorderWidth = 3;
            series[(int)SeriesIndex.UCL].Color = ColorTranslator.FromHtml(CONTROL_COLOR_HEX);
            series[(int)SeriesIndex.UCL].Enabled = uceControlLine.Checked;
            series[(int)SeriesIndex.UCL].YAxisType = AxisType.Primary;
            series[(int)SeriesIndex.UCL].IsValueShownAsLabel = false;

            chartTrend.DataBind();
            chartTrend.Invalidate();
        }

        //--

        public void ExportExcel()
        {

            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.Add(
                new TextObject("Test Report", 20, true, Color.Black),
                new TextObject(string.Format("From Date: {0}", fromDate.Value), 10),
                new TextObject(string.Format("To Date: {0}", toDate.Value), 10),
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

        private bool GetData(
            )
        {
            if (dlbPgmParam.SelectedItem == null)
                return false;

            if (dlbLot.SelectedItems.Count > 40)
            {
                MessageBox.Show("선택한 LOT이 최대치 (40 Lots)를 넘어갔습니다. 조정 후 재조회 바랍니다.");
                return false;
            }

            TestCommon oTestComm = new TestCommon();
            string product = dlbProduct.SelectedValue as String;
            if (String.IsNullOrEmpty(product))
                return false;

            string program = dlbProgram.SelectedValue as String;
            if (String.IsNullOrEmpty(program))
                return false;

            string pgmParam = dlbPgmParam.SelectedValue as String;
            if (String.IsNullOrEmpty(pgmParam))
                return false;

            Utility.FPSpreadUtil.InitSpread(fpStatistics);
            Utility.FPSpreadUtil.InitSpread(fpRawData);

            DataTable dt = oTestComm.GetTestReportRawData(
                GetDateToString(fromDate),
                GetDateToString(toDate),
                DACrux.Base.GlobalVariable.Factory,
                GetSelectedValues(dlbTestArea),
                product,
                program,
                GetSelectedValues(dlbLot),
                pgmParam
                );

            //--

            DataTable rawDt = null;
            if (pgmParam.Length > 2)
                rawDt = GetRawData(dt, pgmParam.Substring(0, 2));
            else
                rawDt = GetRawData(dt, pgmParam);

            Utility.FPSpreadUtil.SetSpreadData(rawDt, fpRawData_Sheet1, 100, 0);
            Utility.FPSpreadUtil.VisibleSpreadColumns(fpRawData_Sheet1, new string[] { "WAFER_SEQ" }, false);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1);
            fpRawData_Sheet1.OperationMode = OperationMode.ReadOnly;

            //--

            NumberCellType numberCellType = new NumberCellType();
            numberCellType.DecimalPlaces = (int)numericUpDown1.Value;
            numberCellType.ShowSeparator = true;
            numberCellType.MinimumValue = -99999999999999;
            numberCellType.MaximumValue = 99999999999999;

            for (int iCol = 11; iCol < fpRawData_Sheet1.Columns.Count; iCol++)
            {
                fpRawData_Sheet1.Columns[iCol].CellType = numberCellType;
            }

            //--

            Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1, false, 0);
            Utility.FPSpreadUtil.SetSpreadData(dt, fpRawData_Sheet1, 100, (int)numericUpDown1.Value);

            dt = oTestComm.GetTestReportStatistics(
                GetDateToString(fromDate),
                GetDateToString(toDate),
                DACrux.Base.GlobalVariable.Factory,
                GetSelectedValues(dlbTestArea),
                product,
                program,
                GetSelectedValues(dlbLot),
                pgmParam
                );
            Utility.FPSpreadUtil.SetSpreadData(dt, fpStatistics_Sheet1, 100, (int)numericUpDown1.Value);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpStatistics_Sheet1);

            return true;
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

        private DataTable GetRawData(
            DataTable dt,
            string parameter
            )
        {
            DataTable rawDt = new DataTable();
            bool bCreateRow = true;
            int idx = 0;

            foreach (DataColumn col in dt.Columns)
            {
                if (DefaultRawColumn(col.ColumnName)
                    || col.ColumnName.Contains(parameter))
                {
                    rawDt.Columns.Add(new DataColumn(col.ColumnName, col.DataType));
                    DataRow newdr = null;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (bCreateRow)
                        {
                            newdr = rawDt.NewRow();
                            newdr[col.ColumnName] = row[col.ColumnName];
                            rawDt.Rows.Add(newdr);
                        }
                        else
                        {
                            newdr = rawDt.Rows[idx];
                            newdr[col.ColumnName] = row[col.ColumnName];
                            idx++;
                        }
                    }
                    bCreateRow = false;
                    idx = 0;
                }
            }

            rawDt.DefaultView.Sort = "WAFER_SEQ ASC, DIE_NUM ASC";
            return rawDt.DefaultView.ToTable();
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

        #endregion [ Method ]
    }
}

