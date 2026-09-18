using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.TEST.Control
{
    public partial class ucTrendChart : UserControl
    {
        public string Title { get; set; }
        public string AxisPara { get; set; }
        public DataSet DataSet { get; set; }
        public double USL { get; set; }
        public double LSL { get; set; }

        DACrux.TEST.Control.PopUpAxisY oYAxis = null;
        //System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

        public ucTrendChart()
        {
            InitializeComponent();
        }

        public ucTrendChart(string strTitle, string strAxisPara, double dUSL, double dLSL, DataSet ds)
        {
            InitializeComponent();

            Title = strTitle;
            AxisPara = strAxisPara;
            USL = dUSL;
            LSL = dLSL;
            DataSet = ds;

            System.Threading.Thread trd = new System.Threading.Thread(new System.Threading.ThreadStart(DrawChart));
            trd.Start();
        }

        private void DrawChart()
        {
            if (chart.InvokeRequired)
            {
                chart.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        DrawChart();
                    }
                    ));
            }
            else
            {
                float PreVal = 0.0f;
                double AVG = 0.0;
                double STDEV = 0.0;

                //변수들을 초기화해준다.
                double sumX;
                double sumY;
                double multilpeXY;
                double squareX;

                //XY 1차 회귀식
                //시간 순에 따른 추세예측 값
                double a;
                double b;

                double[] dBoxValues = new double[6];
                double dVal = double.NaN;

                double dChartMax = double.NaN;
                double dChartMin = double.NaN;


                DataTable dtData = null;

                try
                {
                    if (DataSet.Tables.IndexOf("MAPDATA") <= -1)
                        throw new Exception("Trend Chart 관련 Table 이 DataSet 에 없습니다.");

                    if (DataSet.Tables["MAPDATA"] == null || DataSet.Tables["MAPDATA"].Rows.Count <= 0)
                        throw new Exception("Trend Chart 관련 Table 상에 Data 가 없습니다.");

                    if (DataSet.Tables.IndexOf("BOX") <= -1)
                        throw new Exception("BoxPlot Chart 관련 Table 이 DataSet 에 없습니다.");

                    dtData = DataSet.Tables["MAPDATA"].Select(string.Format("{0} IS NOT NULL OR {0} <> '' ", AxisPara)).CopyToDataTable<DataRow>();

                    labTitle.Text = Title;

                    //변수들을 초기화해준다.
                    sumX = 0.0;
                    sumY = 0.0;
                    multilpeXY = 0.0;
                    squareX = 0.0;

                    //XY 1차 회귀식
                    a = 0.0;
                    b = 0.0;

                    chart.Series["DataSeries"].Points.Clear();
                    chart.Series["DataSeries"].IsValueShownAsLabel = false;
                    chart.Series["DataSeries"].IsXValueIndexed = true;
                    chart.Series["DataSeries"].IsVisibleInLegend = true;
                    chart.Series["DataSeries"].Color = Color.Blue;
                    chart.Series["DataSeries"].MarkerStyle = MarkerStyle.Circle;
                    chart.Series["DataSeries"].MarkerSize = 5;

                    chart.ChartAreas["Data Chart Area"].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
                    chart.ChartAreas["Data Chart Area"].AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
                    //chart.ChartAreas["Data Chart Area"].AxisX.Interval = 1;
                    //chart.ChartAreas["Data Chart Area"].AxisX.LabelStyle.Format = "MM-dd HH:mm:ss";

                    //chart.ChartAreas["Data Chart Area"].AxisY.IntervalType = IntervalAutoMode.VariableCount;
                    //chart.ChartAreas["Data Chart Area"].AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
                    //chart.ChartAreas["Data Chart Area"].AxisX.Interval = 1;
                    chart.ChartAreas["Data Chart Area"].AxisY.LabelStyle.Format = "{#.#000}";

                    chart.ChartAreas["Data Chart Area"].AxisX.IsLabelAutoFit = true;
                    chart.ChartAreas["Data Chart Area"].AxisY.IsStartedFromZero = false;
                    chart.ChartAreas["Data Chart Area"].AxisY2.IsStartedFromZero = false;

                    //차트 Zoom기능 
                    chart.ChartAreas["Data Chart Area"].CursorX.IsUserSelectionEnabled = true;
                    chart.ChartAreas["Data Chart Area"].CursorY.IsUserSelectionEnabled = true;
                    chart.ChartAreas["Data Chart Area"].CursorX.SelectionColor = Color.Red;
                    chart.ChartAreas["Data Chart Area"].CursorY.SelectionColor = Color.Red;
                    chart.ChartAreas["Data Chart Area"].AxisX.ScaleView.Zoomable = true;
                    chart.ChartAreas["Data Chart Area"].AxisY.ScaleView.Zoomable = true;
                    chart.ChartAreas["Data Chart Area"].AxisX.ScrollBar.IsPositionedInside = true;
                    chart.ChartAreas["Data Chart Area"].AxisY.ScrollBar.IsPositionedInside = true;


                    //추세선 그린다. //////////////////
                    //베타 Cov(x,y) / Var(x) , 알바 추론
                    ////////////////////////////////// 

                    if (chart.Series.IndexOf("추세선") <= -1)
                        chart.Series.Add("추세선");
                    else
                        chart.Series["추세선"].Points.Clear();

                    if (chart.Series.IndexOf("+3SIGMA") <= -1)
                        chart.Series.Add("+3SIGMA");
                    else
                        chart.Series["+3SIGMA"].Points.Clear();

                    if (chart.Series.IndexOf("-3SIGMA") <= -1)
                        chart.Series.Add("-3SIGMA");
                    else
                        chart.Series["-3SIGMA"].Points.Clear();

                    if (chart.Series.IndexOf("USL") <= -1)
                        chart.Series.Add("USL");
                    else
                        chart.Series["USL"].Points.Clear();

                    if (chart.Series.IndexOf("LSL") <= -1)
                        chart.Series.Add("LSL");
                    else
                        chart.Series["LSL"].Points.Clear();

                    if (dtData.Columns.IndexOf("X_PREDICT") < 0)
                        dtData.Columns.Add(new DataColumn("X_PREDICT", typeof(decimal)));

                    chart.Series["추세선"].ChartType = SeriesChartType.Spline;
                    chart.Series["추세선"].IsValueShownAsLabel = false;
                    chart.Series["추세선"].IsXValueIndexed = true;
                    chart.Series["추세선"].IsVisibleInLegend = true;
                    chart.Series["추세선"].BorderWidth = 3;
                    chart.Series["추세선"].Color = Color.Green;

                    chart.Series["+3SIGMA"].ChartType = SeriesChartType.Line;
                    chart.Series["+3SIGMA"].BorderDashStyle = ChartDashStyle.Dot;
                    chart.Series["+3SIGMA"].IsValueShownAsLabel = false;
                    chart.Series["+3SIGMA"].IsXValueIndexed = true;
                    chart.Series["+3SIGMA"].IsVisibleInLegend = true;
                    chart.Series["+3SIGMA"].BorderWidth = 3;
                    chart.Series["+3SIGMA"].Color = Color.DarkGray;

                    chart.Series["-3SIGMA"].ChartType = SeriesChartType.Line;
                    chart.Series["-3SIGMA"].BorderDashStyle = ChartDashStyle.Dot;
                    chart.Series["-3SIGMA"].IsValueShownAsLabel = false;
                    chart.Series["-3SIGMA"].IsXValueIndexed = true;
                    chart.Series["-3SIGMA"].IsVisibleInLegend = true;
                    chart.Series["-3SIGMA"].BorderWidth = 3;
                    chart.Series["-3SIGMA"].Color = Color.DarkGray;

                    chart.Series["USL"].ChartType = SeriesChartType.Line;
                    chart.Series["USL"].IsValueShownAsLabel = false;
                    chart.Series["USL"].IsXValueIndexed = true;
                    chart.Series["USL"].IsVisibleInLegend = true;
                    chart.Series["USL"].BorderWidth = 3;
                    chart.Series["USL"].Color = Color.Red;

                    chart.Series["LSL"].ChartType = SeriesChartType.Line;
                    chart.Series["LSL"].IsValueShownAsLabel = false;
                    chart.Series["LSL"].IsXValueIndexed = true;
                    chart.Series["LSL"].IsVisibleInLegend = true;
                    chart.Series["LSL"].BorderWidth = 3;
                    chart.Series["LSL"].Color = Color.Red;

                    if (dtData.Rows.Count == 1)
                    {
                        dtData.Rows[dtData.Rows.Count - 1]["DIE_NUM"] = dtData.Rows.Count;
                    }
                    else
                    {
                        for (int cntX = 1; cntX < dtData.Rows.Count - 1; cntX++)
                        {

                            dtData.Rows[cntX - 1]["DIE_NUM"] = cntX;
                        }

                        dtData.Rows[dtData.Rows.Count - 2]["DIE_NUM"] = dtData.Rows.Count - 1;
                        dtData.Rows[dtData.Rows.Count - 1]["DIE_NUM"] = dtData.Rows.Count;
                    }



                    for (int cntX = 0; cntX < dtData.Rows.Count; cntX++)
                    {
                        if (string.IsNullOrEmpty(dtData.Rows[cntX][AxisPara].ToString()) == true)
                            continue;

                        multilpeXY += System.Convert.ToDouble(dtData.Rows[cntX]["DIE_NUM"]) * System.Convert.ToDouble(dtData.Rows[cntX][AxisPara]);
                        sumX += System.Convert.ToDouble(dtData.Rows[cntX]["DIE_NUM"]);
                        sumY += System.Convert.ToDouble(dtData.Rows[cntX][AxisPara]);
                        squareX += System.Convert.ToDouble(dtData.Rows[cntX]["DIE_NUM"]) * System.Convert.ToDouble(dtData.Rows[cntX]["DIE_NUM"]);

                    }

                    a = (dtData.Rows.Count * multilpeXY - sumX * sumY) / (dtData.Rows.Count * squareX - sumX * sumX);
                    b = (sumY - a * sumX) / dtData.Rows.Count;

                    for (int j = 0; j <= dtData.Rows.Count; j++)
                    {
                        if (dtData.Rows.Count == j || dtData.Rows[j][AxisPara] == DBNull.Value)
                        {
                            PreVal = System.Convert.ToSingle(a * (j + 1) + b);
                            continue;
                        }
                        dtData.Rows[j]["X_PREDICT"] = a * (j + 1) + b;
                    }
                    dtData.AcceptChanges();


                    if (dtData.Rows.Count > 1)
                    {
                        AVG = (System.Convert.ToDouble(dtData.Compute(string.Format("AVG({0})", AxisPara), string.Empty)));
                        STDEV = Math.Sqrt(System.Convert.ToDouble(dtData.Compute(string.Format("VAR({0})", AxisPara), string.Empty)));
                    }
                    else
                    {
                        throw new Exception("분석하기 위한 데이터가 너무 적습니다.");
                    }

                    for (int j = 0; j < dtData.Rows.Count; j++)
                    {
                        if (string.IsNullOrEmpty(dtData.Rows[j][AxisPara].ToString()) == true)
                            continue;

                        chart.Series["DataSeries"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], dtData.Rows[j][AxisPara]);
                        //if (!string.IsNullOrEmpty(dtData.Rows[j]["RESULT"].ToString()))
                        //{
                        //    chart.Series["DataSeries"].Points[chart.Series["DataSeries"].Points.Count - 1].MarkerColor = Color.Red;
                        //    chart.Series["DataSeries"].Points[chart.Series["DataSeries"].Points.Count - 1].MarkerBorderColor = Color.Red;
                        //}

                        if (CheckTrend.Checked)
                        {
                            chart.Series["추세선"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], dtData.Rows[j]["X_PREDICT"]);
                        }

                        if (chkSigma.Checked)
                        {
                            chart.Series["+3SIGMA"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], AVG + 3 * STDEV);
                            chart.Series["-3SIGMA"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], AVG - 3 * STDEV);
                        }

                        if (chkSpec.Checked)
                        {
                            if (USL != double.NaN)
                                chart.Series["USL"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], USL);

                            if (LSL != double.NaN)
                                chart.Series["LSL"].Points.AddXY(dtData.Rows[j]["DIE_NUM"], LSL);
                        }

                        labUSL.Text = USL.ToString();
                        labLSL.Text = LSL.ToString();

                        //Tag 에 각 Point 별 정보를 넣는다.
                        chart.Series["DataSeries"].Points[chart.Series["DataSeries"].Points.Count - 1].Tag =
                                new string[] { dtData.Rows[j]["X"].ToString()
                                        ,  dtData.Rows[j]["Y"].ToString()
                                        ,  dtData.Rows[j][AxisPara].ToString()
                                        ,  dtData.Rows[j]["BIN"].ToString()};
                    }

                    dChartMax = Math.Max(AVG + 5 * STDEV, System.Convert.ToDouble(dtData.Compute(string.Format("MAX({0})", AxisPara), string.Empty)));
                    dChartMin = Math.Min(AVG - 5 * STDEV, System.Convert.ToDouble(dtData.Compute(string.Format("MIN({0})", AxisPara), string.Empty)));


                    if ((dChartMax - dChartMin) == 0)
                    {
                        dChartMax = dChartMax + 3;
                        dChartMin = dChartMin - 3;
                    }

                    if (!string.IsNullOrEmpty(labUSL.Text) && labUSL.Text != "NaN" && chkSpec.Checked)
                        dChartMax = Math.Max(dChartMax, DACrux.Base.Convert.doubleParse(labUSL.Text) + STDEV);

                    if (!string.IsNullOrEmpty(labLSL.Text) && labLSL.Text != "NaN" && chkSpec.Checked)
                        dChartMin = Math.Min(dChartMin, DACrux.Base.Convert.doubleParse(labLSL.Text) - STDEV);


                    chart.ChartAreas["Data Chart Area"].AxisY.Minimum = dChartMin;
                    chart.ChartAreas["Data Chart Area"].AxisY.Maximum = dChartMax;

                    //cursorx = chart.ChartAreas["Data Chart Area"].CursorX;

                    //// Set cursor properties 
                    //cursorx.LineWidth = 1;
                    //cursorx.LineDashStyle = ChartDashStyle.Solid;
                    //cursorx.LineColor = Color.Black;
                    //cursorx.SelectionColor = Color.Yellow;
                    //cursorx.IsUserEnabled = true;

                    //BoxPlot Draw
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    chart.Series["BoxPlotSeries"].Points.Clear();
                    chart.ChartAreas["Box Chart Area"].AxisY.Minimum = dChartMin;
                    chart.ChartAreas["Box Chart Area"].AxisY.Maximum = dChartMax;

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["MIN"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[0] = dVal; // 하위 수염
                    labMin.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["MAX"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[1] = dVal; // 상위 수염 
                    labMax.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["Q1"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[2] = dVal; // 하위 상자
                    labQ1.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["Q3"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[3] = dVal; // 상위 상자
                    labQ3.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["AVG"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[4] = dVal; // 평균
                    labAvg.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["MEDIAN"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    dBoxValues[5] = dVal; // 중앙값 
                    labMedian.Text = Math.Round(dVal, 5).ToString();

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["CNT"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    labCnt.Text = Math.Round(dVal, 5).ToString(); // Count

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["SUM"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    labSum.Text = Math.Round(dVal, 5).ToString(); //Sum

                    if (double.TryParse(DataSet.Tables["BOX"].Rows[0]["STDDEV"].ToString(), out dVal) == false)
                        dVal = double.NaN;
                    labStd.Text = Math.Round(dVal, 5).ToString(); //STDDEVs

                    chart.Series["BoxPlotSeries"].Points.DataBindY(dBoxValues);
                    chart.Series["BoxPlotSeries"]["BoxPlotShowAverage"] = "true";
                    chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                    chart.Series["BoxPlotSeries"]["BoxPlotShowUnusualValues"] = "true";
                }
                catch { }
                finally
                {
                }
            }

        }

        private void CopyImage_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            Rectangle rect;
            try
            {
                rect = chart.DisplayRectangle;
                bmp = new Bitmap(rect.Width, rect.Height);
                chart.DrawToBitmap(bmp, new Rectangle(0, 0, rect.Width, rect.Height));

                Clipboard.Clear();
                Clipboard.SetImage(bmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chartResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart.ChartAreas["Data Chart Area"].AxisX.ScaleView.ZoomReset(0);
            chart.ChartAreas["Data Chart Area"].AxisY.ScaleView.ZoomReset(0);

            chart.ChartAreas["Box Chart Area"].AxisX.ScaleView.ZoomReset(0);
            chart.ChartAreas["Box Chart Area"].AxisY.ScaleView.ZoomReset(0);
        }

        private void chart_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Right)
            //{
            //    // Set the new cursor position 
            //    chart.ChartAreas["Box Chart Area"].CursorX.Position += chart.ChartAreas["Box Chart Area"].CursorX.Interval;
            //}
            //else if (e.KeyCode == Keys.Left)
            //{
            //    // Set the new cursor position 
            //    chart.ChartAreas["Box Chart Area"].CursorX.Position -= chart.ChartAreas["Box Chart Area"].CursorX.Interval;
            //}
        }

        private void yAxisMaxMinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strMax = string.Empty;
            string strMin = string.Empty;
            string strInterval = string.Empty;
            try
            {
                if (oYAxis != null)
                    oYAxis.Dispose();

                strMax = chart.ChartAreas["Data Chart Area"].AxisY.Maximum.ToString();
                strMin = chart.ChartAreas["Data Chart Area"].AxisY.Minimum.ToString();
                strInterval = chart.ChartAreas["Data Chart Area"].AxisY.Interval.ToString();

                oYAxis = null;
                oYAxis = new PopUpAxisY(strMax, strMin, strInterval);
                oYAxis.TopMost = true;
                oYAxis.Owner = this.ParentForm;
                oYAxis.StartPosition = FormStartPosition.CenterParent;
                oYAxis.On_Apply += new PopUpAxisY.Apply(oYAxis_On_Apply);

                oYAxis.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void oYAxis_On_Apply(string Y1Max, string Y1Min, string Y1Interval, string Y2Max, string Y2Min, string Y2Interval, bool Y2Used)
        {
            double dVal = double.NaN;
            try
            {
                if (!double.TryParse(Y1Max, out dVal))
                    dVal = double.NaN;
                chart.ChartAreas["Data Chart Area"].AxisY.Maximum = dVal;
                chart.ChartAreas["Box Chart Area"].AxisY.Maximum = dVal;

                if (!double.TryParse(Y1Min, out dVal))
                    dVal = double.NaN;
                chart.ChartAreas["Data Chart Area"].AxisY.Minimum = dVal;
                chart.ChartAreas["Box Chart Area"].AxisY.Minimum = dVal;

                if (!double.TryParse(Y1Interval, out dVal))
                    dVal = double.NaN;
                chart.ChartAreas["Data Chart Area"].AxisY.Interval = dVal;

                if (Y2Used)
                {
                    if (!double.TryParse(Y2Max, out dVal))
                        dVal = double.NaN;
                    chart.ChartAreas["Data Chart Area"].AxisY.Maximum = dVal;
                    chart.ChartAreas["Box Chart Area"].AxisY.Maximum = dVal;

                    if (!double.TryParse(Y2Min, out dVal))
                        dVal = double.NaN;
                    chart.ChartAreas["Data Chart Area"].AxisY.Minimum = dVal;
                    chart.ChartAreas["Box Chart Area"].AxisY.Minimum = dVal;

                    if (!double.TryParse(Y2Interval, out dVal))
                        dVal = double.NaN;
                    chart.ChartAreas["Data Chart Area"].AxisY.Interval = dVal;
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkSigma_CheckedChanged(object sender, EventArgs e)
        {
            chart.Series["+3SIGMA"].Enabled = chkSigma.Checked;
            chart.Series["-3SIGMA"].Enabled = chkSigma.Checked;
        }

        private void chkSpec_CheckedChanged(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void CheckTrend_CheckedChanged(object sender, EventArgs e)
        {
            chart.Series["추세선"].Enabled = CheckTrend.Checked;
        }

    }

}
