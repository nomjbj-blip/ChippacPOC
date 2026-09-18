using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;

namespace DACrux.TEST.Control
{
    public partial class ucParameterTrend : UserControl
    {
        #region [ Data Field ]
        private object _dataSource = null;
        private String DEFAULT_CHARTAREA = "DEFAULT";
        private String DEFAULT_LEGEND = "LEGEND";

        private String SERIES_BOXPLOT = "BOXPLOT";
        private String SERIES_FASTLINE = "SERIES_FASTLINE";
        private String SERIES_POINT = "SERIES_POINT";

        enum ColumnIndex
        {
            LOT_ID = 0,
            PROGRAM,
            WAFER_ID,
            LOT_START_TIME,
            LOT_END_TIME,
            WAFER_START_TIME,
            WAFER_END_TIME,
            TESTER,
            PROBE_CARD,
            WAFER_SEQ,
            DIE_NUM,
            DIEPROBE_CNT
        }
        #endregion [ Data Field ]

        #region [ Constructor ]
        public ucParameterTrend()
        {
            InitializeComponent();
        }
        #endregion [ Constructor ]

        #region [ Event Handler ]
        #endregion [ Event Handler ]

        #region [ Method ]

        private void CreateStatisticsDataTable(
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
            if (String.IsNullOrEmpty(SelectedParameterName))
                return;

            DataRow[] drParams = (DataSource as DataSet).Tables["PARAM_SPEC"].Select(String.Format("[PARAM_NAME] = '{0}'", SelectedParameterName));
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
                    if (!double.TryParse(drs[idx][SelectedParameterName].ToString(), out dTmp))
                        dTmp = double.NaN;

                    if (!double.IsNaN(dTmp))
                        dValues.Add(dTmp);
                }

                if (dValues == null || dValues.Count <= 0)
                    continue;

                Tuple<double, double> dStandardDeviationAndAvg = StandardDeviationAndAverage(dValues.ToArray());
                Tuple<double, double, double> dQuertiles = Quartiles(dValues.ToArray());

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

            StatisticsSource =  dtStatistics;
        }

        private void DrawStatisticsChart()
        {
            InitChartControl();
            DataTable dt = (StatisticsSource as DataTable);
            if (dt == null || dt.Rows.Count <= 0) 
                return;

            ChartArea cArea = SetChartArea(DEFAULT_CHARTAREA);
            Legend cLegend = chart1.Legends.Add(DEFAULT_LEGEND);

            Series seriesBoxPlot = chart1.Series.Add(SERIES_BOXPLOT);
            seriesBoxPlot.ChartArea = DEFAULT_CHARTAREA;
            seriesBoxPlot.Legend = DEFAULT_LEGEND;
            seriesBoxPlot.IsValueShownAsLabel = false;

            Series series = chart1.Series.Add(SERIES_FASTLINE);
            series.ChartArea = DEFAULT_CHARTAREA;
            series.Legend = DEFAULT_LEGEND;
            series.IsValueShownAsLabel = false;

            Dictionary<String, double[]> dicLimitValues = new Dictionary<string, double[]>();
            double[] dUsl = new double[dt.Rows.Count];
            double[] dTarget = new double[dt.Rows.Count];
            double[] dLsl = new double[dt.Rows.Count];
            double[] dUcl = new double[dt.Rows.Count];
            double[] dLcl = new double[dt.Rows.Count];

            chart1.DataSource = dt;
            cLegend.Enabled = false;

            DataPoint dp = null;
            for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
            {
                DataRow row = dt.Rows[rowIdx];
                if (string.IsNullOrEmpty(row["AVERAGE"].ToString()))
                {
                    dUsl[rowIdx] = double.NaN;
                    dTarget[rowIdx] = double.NaN;
                    dLsl[rowIdx] = double.NaN;
                    dUcl[rowIdx] = double.NaN;
                    dLcl[rowIdx] = double.NaN;
                    continue;
                }

                dp = new DataPoint();
                dp.SetValueXY(row["WAFER_ID"], new object[] { row["MININUM"], row["MAXINUM"], row["Q1"], row["Q3"], row["AVERAGE"], row["MEDIAN"] });
                seriesBoxPlot.Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(row["WAFER_ID"], new object[] { row["AVERAGE"] });
                series.Points.Add(dp);

                dUsl[rowIdx] = Base.Convert.doubleParse(row["USL"].ToString());
                dTarget[rowIdx] = Base.Convert.doubleParse(row["TARGET"].ToString());
                dLsl[rowIdx] = Base.Convert.doubleParse(row["LSL"].ToString());
                dUcl[rowIdx] = Base.Convert.doubleParse(row["UCL"].ToString());
                dLcl[rowIdx] = Base.Convert.doubleParse(row["LCL"].ToString());
            }

            series = new Series(SERIES_POINT);
            series.Legend = DEFAULT_LEGEND;
            series.ChartType = SeriesChartType.Point;
            series.IsValueShownAsLabel = chkAverageLabel.Checked;
            series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
            series.SmartLabelStyle.Enabled = true;
            series.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top;
            series.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
            series.SmartLabelStyle.MaxMovingDistance = 1;

            chart1.DataManipulator.Filter(CompareMethod.EqualTo, double.NaN, "SERIES_FASTLINE", "SERIES_POINT");

            dicLimitValues.Add("USL", dUsl);
            dicLimitValues.Add("TARGET", dTarget);
            dicLimitValues.Add("LSL", dLsl);
            dicLimitValues.Add("UCL", dUcl);
            dicLimitValues.Add("LCL", dLcl);

            DrawLimitLine(dicLimitValues);
        }

        private void DrawRawDataChart()
        {
            InitChartControl();
            DataTable dt = (_dataSource as DataTable);
            if (dt == null || dt.Rows.Count <= 0)
                return;

            ChartArea cArea = SetChartArea(DEFAULT_CHARTAREA);
            Legend cLegend = chart1.Legends.Add(DEFAULT_LEGEND);

            Series series = chart1.Series.Add(SERIES_FASTLINE);
            series.ChartArea = DEFAULT_CHARTAREA;
            series.Legend = DEFAULT_LEGEND;
            series.IsValueShownAsLabel = false;

            Dictionary<String, double[]> dicLimitValues = new Dictionary<string, double[]>();
            double[] dUsl = new double[dt.Rows.Count];
            double[] dTarget = new double[dt.Rows.Count];
            double[] dLsl = new double[dt.Rows.Count];
            double[] dUcl = new double[dt.Rows.Count];
            double[] dLcl = new double[dt.Rows.Count];

            chart1.DataSource = dt;
            cLegend.Enabled = false;

            DataPoint dp = null;
            for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
            {
                DataRow row = dt.Rows[rowIdx];
                if (string.IsNullOrEmpty(row[SelectedParameterName].ToString()))
                {
                    dUsl[rowIdx] = double.NaN;
                    dTarget[rowIdx] = double.NaN;
                    dLsl[rowIdx] = double.NaN;
                    dUcl[rowIdx] = double.NaN;
                    dLcl[rowIdx] = double.NaN;
                    continue;
                }

                dp = new DataPoint();
                dp.SetValueXY(row["WAFER_ID"], new object[] { row[SelectedParameterName] });
                series.Points.Add(dp);

                dUsl[rowIdx] = Base.Convert.doubleParse(row["USL"].ToString());
                dTarget[rowIdx] = Base.Convert.doubleParse(row["TARGET"].ToString());
                dLsl[rowIdx] = Base.Convert.doubleParse(row["LSL"].ToString());
                dUcl[rowIdx] = Base.Convert.doubleParse(row["UCL"].ToString());
                dLcl[rowIdx] = Base.Convert.doubleParse(row["LCL"].ToString());
            }

            series = new Series(SERIES_POINT);
            series.Legend = DEFAULT_LEGEND;
            series.ChartType = SeriesChartType.Point;
            series.IsValueShownAsLabel = chkAverageLabel.Checked;
            series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Partial;
            series.SmartLabelStyle.Enabled = true;
            series.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top;
            series.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
            series.SmartLabelStyle.MaxMovingDistance = 1;

            chart1.DataManipulator.Filter(CompareMethod.EqualTo, double.NaN, SERIES_FASTLINE, SERIES_POINT);

            dicLimitValues.Add("USL", dUsl);
            dicLimitValues.Add("TARGET", dTarget);
            dicLimitValues.Add("LSL", dLsl);
            dicLimitValues.Add("UCL", dUcl);
            dicLimitValues.Add("LCL", dLcl);

            DrawLimitLine(dicLimitValues);

        }

        private void DrawLimitLine(Dictionary<string, double[]> LimitValues)
        {
            try
            {
                if (chart1.Annotations.Count > 0) chart1.Annotations.Clear();
                if (LimitValues == null || LimitValues.Count == 0) return;

                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                // --
                ChartUtil.DrawLimitLine(chart1, DEFAULT_CHARTAREA, LimitValues);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string GetFormat()
        {
            StringBuilder sbFormat = new StringBuilder();
            for (int idx = 0; idx < (int)DecimalLength; idx++)
                sbFormat.Append("#");
            return string.Format("#.{0}", sbFormat.ToString());
        }

        private void InitChartControl()
        {
            chart1.Annotations.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();
            chart1.Series.Clear();
        }

        private ChartArea SetChartArea(
            string name
            )
        {
            ChartArea cArea = chart1.ChartAreas.Add(name);

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
            cArea.AxisY.LabelStyle.Format = GetFormat();
            cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
            cArea.AxisY.ScaleView.Zoomable = true;
            cArea.AxisY.ScaleView.ZoomReset();
            cArea.AxisY.ScrollBar.LineColor = Color.Black;
            cArea.AxisY.ScrollBar.Size = 17;
            cArea.AxisY.IsStartedFromZero = false;
            cArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            cArea.AxisY.LabelStyle.Format = "{0:0." + String.Concat(Enumerable.Repeat("0", (int)DecimalLength)) + "}"; ;

            cArea.CursorY.AutoScroll = true;
            cArea.CursorY.IsUserSelectionEnabled = true;
            cArea.CursorY.Interval = 0.1;
            cArea.CursorY.IntervalOffset = 0.1;
            cArea.CursorY.IntervalOffsetType = DateTimeIntervalType.Auto;

            return cArea;
        }

        private SeriesChartType SetChartType(
            string name
            )
        {
            return (SeriesChartType)Enum.Parse(typeof(SeriesChartType), name, true);
        }

        //--------------------------------------------------------------------------------------------------

        #region [ Statistis Method ]

        /// <summary>
        /// Average and Standard Deviation Calculation
        /// </summary>
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

        #endregion [ Statistis Method ]

        //--------------------------------------------------------------------------------------------------

        #endregion [ Method ]

        #region [ Property ]

        public Chart Chart
        {
            get { return this.chart1; }
            private set { this.chart1 = value; }
        }

        //--

        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                if (String.IsNullOrEmpty(SelectedParameterName))
                    return;

                CreateStatisticsDataTable((_dataSource as DataTable));
                DrawStatisticsChart();
            }
        }

        //--

        public decimal DecimalLength
        {
            get;
            set;
        }

        //--

        public String SelectedParameterName
        {
            get;
            set;
        }

        public object StatisticsSource
        {
            get;
            private set;
        }
        #endregion [ Property ]
    }
}
