using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.TEST.ENGUI
{
    public partial class frmYieldReport
        : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {
        DataTable dtWafer;

        public frmYieldReport()
        {
            InitializeComponent();
        }

        private void frmYieldReport_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
        }

        DataTable DataFilterByWafer()
        {
            DataTable dt = new DataTable("WAFER_YIELD");
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("WAFER_ID", typeof(string));
            dt.Columns.Add("YIELD", typeof(float));

            for (int a = 0; a < dtWafer.Rows.Count; a++)
            {
                object[] row = new object[] {
												  dtWafer.Rows[a]["LOT_ID"]
												  , dtWafer.Rows[a]["WAFER_ID"]
												  , dtWafer.Rows[a]["YIELD"]

									};
                dt.Rows.Add(row);
            }

            return dt;
        }

        object DataFilterByStatisticalYield(string lot, string wafer)
        {
            //DataRow[] dr = dtWafer.Select(string.Format("LOT_ID='{0}' AND SLOT_ID='{1}'", lot, wafer));
            DataRow[] dr = dtWafer.Select(string.Format("LOT_ID='{0}' AND WAFER_ID LIKE '%{0}-{1}'", lot, wafer));
            if (dr == null) return DBNull.Value;
            if (dr.Length == 0) return DBNull.Value;
            return dr[0]["YIELD"];
        }

        DataTable DataFilterByStatistical()
        {
            int waferCnt = 25;
            DataTable dt = new DataTable("LOT_WAFER_YIELD");
            dt.Columns.Add("LOT_ID", typeof(string));
            string waferId;
            for (int a = 1; a <= waferCnt; a++)
            {
                waferId = string.Format("{0}", a).PadLeft(2, '0');
                dt.Columns.Add(waferId, typeof(double));
            }

            ArrayList alLot = new ArrayList();
            for (int a = 0; a < dtWafer.Rows.Count; a++)
            {
                if (alLot.IndexOf(dtWafer.Rows[a]["LOT_ID"]) == -1) alLot.Add(dtWafer.Rows[a]["LOT_ID"]);
            }

            for (int a = 0; a < alLot.Count; a++)
            {
                object[] row = new object[waferCnt + 1];
                row[0] = (string)alLot[a];
                for (int b = 1; b <= waferCnt; b++)
                {
                    row[b] = DataFilterByStatisticalYield((string)alLot[a], dt.Columns[b].ColumnName);
                }
                dt.Rows.Add(row);
            }

            return dt;

        }

        void DataFilterByBoxPlot()
        {
            int waferCnt = 25;
            DataTable dt = new DataTable("LOT_WAFER_YIELD");
            dt.Columns.Add("WAFER_ID", typeof(string));

            ArrayList alLot = new ArrayList();
            for (int a = 0; a < dtWafer.Rows.Count; a++)
            {
                if (alLot.IndexOf(dtWafer.Rows[a]["LOT_ID"]) == -1) alLot.Add(dtWafer.Rows[a]["LOT_ID"]);
            }

            for (int a = 0; a < alLot.Count; a++)
            {
                dt.Columns.Add((string)alLot[a], typeof(double));
            }

            for (int b = 1; b <= waferCnt; b++)
            {
                object[] row = new object[alLot.Count + 1];
                for (int a = 0; a < alLot.Count; a++)
                {
                    row[0] = string.Format("{0}", b).PadLeft(2, '0');
                    row[a + 1] = DataFilterByStatisticalYield((string)alLot[a], string.Format("{0}", b).PadLeft(2, '0'));
                }
                dt.Rows.Add(row);

            }
            DrawGraph(dt, "Box Plot");
        }

        float Yield(string lot)
        {
            float tYield = 0;
            DataRow[] dr = dtWafer.Select(string.Format("LOT_ID='{0}'", lot));
            for (int a = 0; a < dr.Length; a++)
            {
                tYield += (float)(decimal)dr[a]["YIELD"];
            }
            return tYield / dr.Length;
        }

        DataTable DataFilterByLot()
        {
            ArrayList alLot = new ArrayList();
            for (int a = 0; a < dtWafer.Rows.Count; a++)
            {
                if (alLot.IndexOf(dtWafer.Rows[a]["LOT_ID"]) == -1) alLot.Add(dtWafer.Rows[a]["LOT_ID"]);
            }

            DataTable dt = new DataTable("LOT_YIELD");
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("YIELD", typeof(double));
            for (int a = 0; a < alLot.Count; a++)
            {
                object[] row = new object[] {
												  alLot[a], Yield((string)alLot[a])
											  };
                dt.Rows.Add(row);
            }
            return dt;
        }

        DataTable DataFilterByLotSheet()
        {
            ArrayList alLot = new ArrayList();
            for (int a = 0; a < dtWafer.Rows.Count; a++)
            {
                if (alLot.IndexOf(dtWafer.Rows[a]["LOT_ID"]) == -1) alLot.Add(dtWafer.Rows[a]["LOT_ID"]);
            }

            DataTable dt = new DataTable("LOT_SHEET");
            for (int a = 0; a < this.dtWafer.Columns.Count; a++)
            {
                if (this.dtWafer.Columns[a].ColumnName.Equals("WAFER_ID")
                    || this.dtWafer.Columns[a].ColumnName.IndexOf("BIN") != -1
                    || this.dtWafer.Columns[a].ColumnName.IndexOf("GEC") != -1) continue;
                dt.Columns.Add(this.dtWafer.Columns[a].ColumnName, this.dtWafer.Columns[a].DataType);
            }
            for (int a = 0; a < alLot.Count; a++)
            {
                DataRow[] dr = dtWafer.Select(string.Format("LOT_ID='{0}'", alLot[a]));
                object[] row = new object[dt.Columns.Count];
                int rowNo = 0;
                for (int b = 0; b < this.dtWafer.Columns.Count; b++)
                {
                    if (this.dtWafer.Columns[b].ColumnName.Equals("WAFER_ID")
                        || this.dtWafer.Columns[b].ColumnName.IndexOf("BIN") != -1
                        || this.dtWafer.Columns[b].ColumnName.IndexOf("GEC") != -1) continue;
                    row[rowNo] = dr[0][b];
                    rowNo++;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        void WaferDataLoad(DACrux.Base.TPWafer[] wafer)
        {
            RO.ProbeMapAnalysis oRemote = null;
            DataTable dt = null;

            long[] waferSeq = new long[wafer.Length];
            for (int a = 0; a < waferSeq.Length; a++) waferSeq[a] = DACrux.Base.Convert.longParse(wafer[a].WaferSeq);
            oRemote = new RO.ProbeMapAnalysis();
            dt = oRemote.GetWaferInfo(waferSeq);
            dtWafer = dt;
        }

        public void DrawGraph(DataTable dt, string rbText)
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Legends.Clear();


            chart1.ChartAreas.Add("ChartArea1");
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;

            if (!rbText.Equals("Box Plot") && !rbText.Equals("Scatter"))
            {
                chart1.Series.Add(rbText);
                chart1.Series[rbText].IsValueShownAsLabel = true;
                chart1.Series[rbText].LabelFormat = "N2";
                chart1.Series[rbText].MarkerSize = 12;
                chart1.Series[rbText].MarkerStyle = MarkerStyle.Square;
            }


            if (rbText.Equals("Wafer"))
            {
                chart1.Series[rbText].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string lot_ID = dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                    chart1.Series[rbText].Points.AddXY(lot_ID, Convert.ToDouble(dt.Rows[i][2]));
                    chart1.Series[rbText].Points[i].AxisLabel = lot_ID.ToString();
                }

                chart1.Series[rbText].IsValueShownAsLabel = false;
                chart1.Series[rbText].MarkerSize = 10;
                chart1.Series[rbText].LabelFormat = "N2";
                chart1.Series[rbText].MarkerStyle = MarkerStyle.Square;

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                chart1.ChartAreas[0].AxisY.Interval = 10;
                chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;



            }
            else if (rbText.Equals("Lot"))
            {

                chart1.Series[rbText].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[rbText].Points.DataBindXY(dt.Rows, "LOT_ID", dt.Rows, "YIELD");
                chart1.Series[rbText].IsValueShownAsLabel = false;
                chart1.Series[rbText].LabelFormat = "N2";
                chart1.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                chart1.ChartAreas[0].AxisY.Interval = 10;
            }
            else if (rbText.Equals("Scatter"))
            {
                ArrayList alData = new ArrayList();
                double[] fullData = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string scatter = Convert.ToString(dt.Rows[i][0]);
                    chart1.Series.Add(scatter);


                    for (int j = 1; j < dt.Columns.Count; j++)
                    {


                        if (dt.Rows[i].IsNull(dt.Columns[j]) == true)
                            continue;

                        alData.Add(Convert.ToDouble(dt.Rows[i][j]));
                        if (dt.Rows.Count == 1)
                        {
                            scatter += i;
                            chart1.Series.Add(scatter);
                            chart1.Series[scatter].Points.AddXY(scatter, Convert.ToDouble(dt.Rows[i][j]));
                            chart1.Series[scatter].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            chart1.Series[scatter].IsValueShownAsLabel = false;
                            chart1.Series[scatter].MarkerSize = 12;
                            chart1.Series[scatter].LabelFormat = "N2";
                            chart1.Series[scatter].MarkerStyle = MarkerStyle.Square;
                            chart1.Series[scatter].Color = Color.CadetBlue;
                        }
                        else
                        {
                            chart1.Series[scatter].Points.AddXY(i, Convert.ToDouble(dt.Rows[i][j]));
                            chart1.Series[scatter].IsValueShownAsLabel = false;
                        }

                    }

                    chart1.Series[scatter].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart1.Series[scatter].IsValueShownAsLabel = false;
                    chart1.Series[scatter].MarkerSize = 12;
                    chart1.Series[scatter].LabelFormat = "N2";

                    //X축 int값을 string으로 변환
                    double minXaxes = i - 0.5;
                    double maxXaxes = i + 0.5;
                    string xAxesValue = scatter;
                    chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(minXaxes, maxXaxes, xAxesValue);
                }

                alData.Sort();
                fullData = (double[])alData.ToArray(System.Type.GetType("System.Double"));
                double dblMin = Stat.Min(fullData);
                double dblMax = Stat.Max(fullData);
                if (dblMin != dblMax)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = dblMin - (dblMax - dblMin) * 0.2;
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = dblMax + (dblMax - dblMin) * 0.2;
                }
                chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N2";

            }
            else if (rbText.Equals("Box Plot"))
            {
                chart1.Series.Add(rbText);
                string[] lot_IDes = new string[dt.Columns.Count - 1];
                ArrayList alData = new ArrayList();
                double[] fullData = null;

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    string lot_ID = Convert.ToString(dt.Columns[i]);
                    int cnt = 0;

                    lot_IDes[i - 1] = lot_ID;

                    chart1.Series.Add(lot_ID);

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (dt.Rows[j].IsNull(dt.Columns[i]) == true)
                        {
                            cnt++;
                            continue;

                        }
                        alData.Add(Convert.ToDouble(dt.Rows[j][i]));
                        chart1.Series[lot_ID].Points.AddY(dt.Rows[j][i]);
                        chart1.Series[lot_ID].Points[j - cnt].AxisLabel = Convert.ToString(dt.Columns[i]);
                    }
                    //chart1.Series[lot_ID].ChartType = SeriesChartType.Point;
                    //chart1.Series[lot_ID].ChartArea = "ChartArea2";
                    chart1.Series[lot_ID].Enabled = false;
                    chart1.Series[lot_ID].IsValueShownAsLabel = false;

                    double minXaxes = i - 0.5;
                    double maxXaxes = i + 0.5;
                    string xAxesValue = lot_ID;
                    chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(minXaxes, maxXaxes, xAxesValue);

                }
                alData.Sort();
                fullData = (double[])alData.ToArray(System.Type.GetType("System.Double"));
                chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N2";
                chart1.Series[rbText].ChartType = SeriesChartType.BoxPlot;
                chart1.Series[rbText]["BoxPlotSeries"] = string.Join(";", lot_IDes);
                chart1.Series[rbText]["BoxPlotShowMedian"] = "true";
                chart1.Series[rbText]["BoxPlotWhiskerPercentile"] = "0";
                chart1.Series[rbText]["BoxPlotPercentile"] = "25";
                chart1.Series[rbText]["BoxPlotShowAverage"] = "true";
                chart1.Series[rbText]["BoxPlotShowMedian"] = "true";
                chart1.Series[rbText]["BoxPlotShowUnusualValues"] = "true";
                chart1.Series[rbText]["MaxPixelPointWidth"] = "80";
                chart1.Series[rbText].BorderWidth = 2;


                double dblMin = Stat.Min(fullData);
                double dblMax = Stat.Max(fullData);

                if (dblMin != dblMax)
                {
                    chart1.ChartAreas["ChartArea1"].AxisY.Minimum = dblMin - (dblMax - dblMin) * 0.2;
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = dblMax + (dblMax - dblMin) * 0.2;
                }

            }
            else if (rbText.Equals("Day"))
            {
                chart1.Series[rbText].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[rbText].Points.DataBindXY(dt.Rows, "DATE", dt.Rows, "AVG");
                chart1.Series[rbText].IsValueShownAsLabel = false;
                chart1.Series[rbText].LabelFormat = "N2";
                chart1.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                chart1.ChartAreas[0].AxisY.Interval = 10;
            }
        }


        private void radioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                RadioButton rb = (RadioButton)sender;
                if (!rb.Checked) return;
                DataTable dt;
                switch (rb.Text)
                {
                    case "Wafer":
                        dt = DataFilterByWafer();
                        DrawGraph(dt, "Wafer");
                        fpSpread.ActiveSheet.DataSource = dt;
                        break;

                    case "Lot":
                        dt = DataFilterByLot();
                        DrawGraph(dt, "Lot");
                        fpSpread.ActiveSheet.DataSource = dt;

                        break;
                    case "Scatter":
                        dt = DataFilterByStatistical();
                        DrawGraph(dt, "Scatter");
                        fpSpread.ActiveSheet.DataSource = dt;
                        break;
                    case "Box Plot":
                        DataFilterByBoxPlot();
                        fpSpread.ActiveSheet.DataSource = DataFilterByStatistical();
                        break;
                    case "Day":
                        DataFilterByDay();
                        break;
                }
                chkX.Checked = true;
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread.ActiveSheet);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonToExcel_Click(object sender, System.EventArgs e)
        {
            DACrux.Utility.ExcelUtilNoStatic excel = new Utility.ExcelUtilNoStatic();
            excel.ToExcel(fpSpread, chart1);
        }

        void DataFilterByDay()
        {
            string[] waferSeq = new string[dtWafer.Rows.Count];
            for (int a = 0; a < waferSeq.Length; a++)
                waferSeq[a] = string.Format("{0}", dtWafer.Rows[a]["WAFER_SEQ"]);
            RO.ProbeMapAnalysis oProbe = null;
            DataTable dt = null;
            try
            {
                oProbe = new RO.ProbeMapAnalysis();
                dt = oProbe.GetYieldByDay(waferSeq);
                DrawGraph(dt, "Day");
                //chart.DataSource = dt;
                fpSpread_Sheet.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oProbe = null;
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }
        //private DataPoint selectedDataPoint = null;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            string SeriesPoint = string.Empty;

            for (int i = 0; i < chart1.Series.Count; i++)
            {

                foreach (DataPoint point1 in chart1.Series[i].Points)
                {
                    point1.BorderWidth = 1;
                    point1.MarkerSize = 12;
                }


                if
                (result.ChartElementType == ChartElementType.DataPoint ||
                result.ChartElementType == ChartElementType.LegendItem)
                {
                    SeriesPoint = result.Series.Name.ToString();
                    DataPoint point2 = chart1.Series[SeriesPoint].Points[result.PointIndex];
                    DataPoint tooltipPoint = chart1.Series[SeriesPoint].Points[result.PointIndex];

                    if (SeriesPoint == "Box Plot")
                    {
                        double[] value = chart1.Series[SeriesPoint].Points[result.PointIndex].YValues;

                        tooltipPoint.ToolTip = chart1.Series[SeriesPoint].YValueMembers.ToString() + "\n" +
                        chart1.Series[SeriesPoint].Points[result.PointIndex].AxisLabel + "\n" +
                        "평균값: " + Math.Round(Stat.mean(value), 2) + "\n" +
                        "최대값: " + Stat.Max(value) + "\n" +
                        "최소값: " + Stat.Min(value);
                        //"중앙값: " + (Stat.Max(value) - Stat.Min(value)) / 2;      

                        // chart1.Series[SeriesPoint].BackHatchStyle = ChartHatchStyle.Percent70;
                    }
                    else
                    {
                        tooltipPoint.ToolTip = chart1.Series[SeriesPoint].YValueMembers.ToString() + "\n" +
                        chart1.Series[SeriesPoint].Points[result.PointIndex].AxisLabel + "\n" +
                       "Values: " + Math.Round(chart1.Series[SeriesPoint].Points[result.PointIndex].YValues[0], 2);

                        //chart1.Series[SeriesPoint].Points[result.PointIndex].BorderWidth = 3;
                        chart1.Series[SeriesPoint].Points[result.PointIndex].MarkerSize = 20;
                    }
                }
            }
        }


        private void chkX_CheckedChanged(object sender, EventArgs e)
        {
            if (chkX.Checked)
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            else
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
        }

        private void chkY_CheckedChanged(object sender, EventArgs e)
        {
            if (chkY.Checked)
            {
                for (int i = 0; i < chart1.Series.Count; i++)
                {
                    chart1.Series[i].IsValueShownAsLabel = true;
                }
            }
            else
            {
                for (int i = 0; i < chart1.Series.Count; i++)
                {
                    chart1.Series[i].IsValueShownAsLabel = false;
                }
            }
        }

        private DataPoint selectedDataPoint = null;
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            //if (selectedDataPoint != null)
            //{
            //    selectedDataPoint.IsValueShownAsLabel = false;

            //    selectedDataPoint = null;

            //    chart1.Invalidate();

            chart1.Cursor = Cursors.Default;
            //}
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            radioButtonWafer.Checked = false;
            WaferDataLoad(wafer);
            radioButtonWafer.Checked = true;

            this.TPWaferList = wafer;
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[strWafer.Length];
            for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
            {
                oWafer[iWafer].WaferSeq = strWafer[iWafer];
            }

            DrawWafer(oWafer);

            Application.DoEvents();
        }
    }
}
