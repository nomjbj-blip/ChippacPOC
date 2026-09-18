/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Histogram.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.03.20
--  Description     : DACrux/SPC Histogram Chart
--  History         : Created by YSIM at 2013.03.20
 * ********************************************************************************************************
 * 2015 년 DACrux V5 History Start

 ----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;

namespace DACrux.SPC.Visualization
{
    public enum HistogramIntervalType
    {
        Number
        ,Width
    }

    public partial class Histogram : UserControl
    {
        #region Fields
  

        DataTable m_dtRaw = null;
        string[] m_strDataMember = null;
        int[] m_idxDataMember = null;
        int m_iSegmentIntervalNumber = 10;
        double m_dSegmentIntervalWidth = 15;
        bool m_bShowPercentOnSecondaryYAxis = true;
        HistogramIntervalType m_enHistogramIntervalType = HistogramIntervalType.Number;


        double m_dUSL = double.NaN;
        double m_dLSL = double.NaN;
        double m_dTarget = double.NaN;

        double m_dMean =  double.NaN;
        double m_dVariance =  double.NaN;
        double m_dStdev  =  double.NaN;
        double m_dMinValue =  double.NaN;
        double m_dMaxValue = double.NaN;

        double m_dCp = double.NaN;
        double m_dCpk = double.NaN;

        public bool VisibleNormalizeLine = true;
        public bool VisibleSpecLimit = true;
        public bool Visible3SigmaLine = true;
        public bool VisibleHorizonGridLine = true;
        public bool VisibleFrequenceText = true;

        LineConfig m_oSPEC_CFG = new LineConfig(Color.Red);
        LineConfig m_o3SIGMA_CFG = new LineConfig(Color.Blue);

        #endregion // Fields

        public LineConfig SPECLine
        {
            get
            {
                return m_oSPEC_CFG;
            }
        }

        public int SegmentIntervalNumber
        {
            get
            {
                return m_iSegmentIntervalNumber;
            }
            set
            {
                m_iSegmentIntervalNumber = value;
            }
        }

        public double SegmentIntervalWidth
        {
            get
            {
                return m_dSegmentIntervalWidth;
            }
            set
            {
                m_dSegmentIntervalWidth = value;
            }
        }

        public bool ShowPercentOnSecondaryYAxis
        {
            get
            {
                return m_bShowPercentOnSecondaryYAxis;
            }
            set
            {
                m_bShowPercentOnSecondaryYAxis = value;
            }
        }

        public DataTable DataSource
        {
            get
            {
                return m_dtRaw;
            }
            set
            {
                m_dtRaw = value;
            }
        }

        public bool HistogramVisible
        {
            get
            {
                return this.Visible;
            }
            set
            {
                this.Visible = value;
            }
        }


        public System.Windows.Forms.DataVisualization.Charting.Axis AxisX
        {
            get
            {
                return chartHistogram.ChartAreas["HistogramArea"].AxisX;
            }
        }

        public System.Windows.Forms.DataVisualization.Charting.Axis AxisY
        {
            get
            {
                return chartHistogram.ChartAreas["HistogramArea"].AxisY;
            }
        }

        public double USL
        {
            get
            {
                return m_dUSL;
            }
            set
            {
                m_dUSL = value;
            }
        }

        public double Target
        {
            get
            {
                return m_dTarget;
            }
            set
            {
                m_dTarget = value;
            }
        }

        public double LSL
        {
            get
            {
                return m_dLSL;
            }
            set
            {
                m_dLSL = value;
            }
        }

        public string MainTitle
        {
            get
            {
                return chartHistogram.Titles["MAIN"].Text;
            }
            set
            {
                chartHistogram.Titles["MAIN"].Text = value;
            }
        }


        public Histogram()
        {
            InitializeComponent();
            m_oSPEC_CFG.LinePen.DashStyle = DashStyle.Dot;
            m_oSPEC_CFG.LinePen.DashPattern = new float[] { 5, 5 };
        }

        public void ResetChartData()
        {
            m_dtRaw = null;
        }

        public void ChartInvalidate()
        {
            chartHistogram.Invalidate();
        }

        public void SetVisibleNormalizeLine(bool IsVisible)
        {
            chartHistogram.Series["NormalDistribution"].Enabled = VisibleNormalizeLine = IsVisible;
        }

        public void SetVisibleFrequncyText(bool IsVisible)
        {
            chartHistogram.Series["HistogramArea"].IsValueShownAsLabel = VisibleFrequenceText = IsVisible;
        }

        /// <summary>
        /// 주어진 Data의 Histogram을 그린다.
        /// </summary>
        /// <param name="ValueMember">Multi Colums => "VALUE_01;VALUE_02;VALUE_03"</param>
        public void DrawHistogram(DataTable DataSource, double dUSL, double dLSL, double dTARGET, string ValueMember = "EXT_MV")
        {
            m_dUSL = dUSL;
            m_dLSL = dLSL;
            m_dTarget = dTARGET;

            m_dtRaw = DataSource;

            DrawHistogram(ValueMember);
        }

        /// <summary>
        /// 주어진 Data의 Histogram을 그린다.
        /// </summary>
        /// <param name="ValueMember">Multi Colums => "VALUE_01;VALUE_02;VALUE_03"</param>
        public void DrawHistogram(string ValueMember = "EXT_MV")
        {
            try
            {
                if (m_dtRaw == null) return;
                if (ValueMember == null) ValueMember = "EXT_MV";
                /// 1. Datamember Column 설정
                ///////////////////////////////////////////////
                m_strDataMember = ValueMember.Split(new char[] { ';', ',', '/' }, StringSplitOptions.RemoveEmptyEntries);

                m_idxDataMember = new int[m_strDataMember.Length];
                for (int i = 0; i < m_strDataMember.Length; i++)
                {
                    m_idxDataMember[i] = m_dtRaw.Columns.IndexOf(m_strDataMember[i]);
                }

                /// 2. CpCpk 계산
                ///////////////////////////////////////////////
                Caculate();


                /// 3. Histogram Drawing을 위한 Data Set
                ///////////////////////////////////////////////
                double tmp = double.NaN;
                chartHistogram.Series["RawData"].Points.Clear();
                chartHistogram.Series["DataDistribution"].Points.Clear();

                for (int m = 0; m < m_strDataMember.Length; m++)
                {
                    for (int i = 0; i < m_dtRaw.Rows.Count; i++)
                    {
                        if (double.TryParse(m_dtRaw.Rows[i][m_strDataMember[m]].ToString(), out tmp))
                        {
                            chartHistogram.Series["RawData"].Points.AddY(tmp);
                            chartHistogram.Series["DataDistribution"].Points.AddXY(tmp, 1);
                        }
                    }
                }

                /// 4. Histogram Drawing
                ///////////////////////////////////////////////
                CreateHistogram(chartHistogram, "RawData", "HistogramArea");
                chartHistogram.ChartAreas["Default"].AxisX.Minimum = chartHistogram.ChartAreas["HistogramArea"].AxisX.Minimum;
                chartHistogram.ChartAreas["Default"].AxisX.Maximum = chartHistogram.ChartAreas["HistogramArea"].AxisX.Maximum;
                chartHistogram.ChartAreas["Default"].AxisX.Interval = chartHistogram.ChartAreas["HistogramArea"].AxisX.Interval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Caculate()
        {
            DACrux.Stat.Core.ProcessCapability oProc = null;
            oProc = new DACrux.Stat.Core.ProcessCapability(m_dtRaw, m_idxDataMember, m_dUSL.ToString(), m_dTarget.ToString(), m_dLSL.ToString());
            m_dCp = oProc.CP;
            m_dCpk = oProc.CPK;
        }

        public void CreateHistogram(Chart chartControl,string dataSeriesName,string histogramSeriesName)
        {
            // Validate input
            if (chartControl == null)
            {
                throw (new ArgumentNullException("chartControl"));
            }
            if (chartControl.Series.IndexOf(dataSeriesName) < 0)
            {
                throw (new ArgumentException("Series with name'" + dataSeriesName + "' was not found.", "dataSeriesName"));
            }

            // Make data series invisible
            chartControl.Series[dataSeriesName].Enabled = false;

            // Check if histogram series exsists
            Series histogramSeries = null;
            if (chartControl.Series.IndexOf(histogramSeriesName) < 0)
            {
                // Add new series
                histogramSeries = chartControl.Series.Add(histogramSeriesName);

                // Set new series chart type and other attributes
                histogramSeries.ChartType = SeriesChartType.Column;
                histogramSeries.BorderColor = Color.Black;
                histogramSeries.BorderWidth = 1;
                histogramSeries.BorderDashStyle = ChartDashStyle.Solid;
            }
            else
            {
                histogramSeries = chartControl.Series[histogramSeriesName];
                histogramSeries.Points.Clear();
            }

            histogramSeries.IsValueShownAsLabel = VisibleFrequenceText;

            // Get data series minimum and maximum values

            m_dMean = chartControl.DataManipulator.Statistics.Mean(dataSeriesName);
            m_dVariance = chartControl.DataManipulator.Statistics.Variance(dataSeriesName, true);

            m_dStdev = Math.Sqrt(m_dVariance);
            m_dMinValue = m_dMean - m_dStdev * 6;
            m_dMaxValue = m_dMean + m_dStdev * 6;
            int pointCount = 0;


            if (m_enHistogramIntervalType == HistogramIntervalType.Number || double.IsNaN(m_dSegmentIntervalWidth))
            {
                // Calculate interval width if it's not set
                m_dSegmentIntervalWidth = (m_dMaxValue - m_dMinValue) / m_iSegmentIntervalNumber;
                m_dSegmentIntervalWidth = RoundInterval(m_dSegmentIntervalWidth);

            }

            // Round minimum and maximum values
            m_dMinValue = Math.Floor(m_dMinValue / m_dSegmentIntervalWidth) * m_dSegmentIntervalWidth;
            m_dMaxValue = Math.Ceiling(m_dMaxValue / m_dSegmentIntervalWidth) * m_dSegmentIntervalWidth;
            ///
            ////////////////////////////////////////////////////////////////////////////
            double dFx = double.NaN;
            double dFy = double.NaN;
            double dNormal = double.NaN;
            double coef = 1.0 / Math.Sqrt(2 * Math.PI);
            ////////////////////////////////////////////////////////////////////////////
            ///

            // Create histogram series points
            double currentPosition = m_dMinValue;
            chartControl.Series["NormalDistribution"].Points.Clear();
            for (currentPosition = m_dMinValue; currentPosition <= m_dMaxValue; currentPosition += this.SegmentIntervalWidth)
            {
                // Count all points from data series that are in current interval
                int count = 0;
                foreach (DataPoint dataPoint in chartControl.Series[dataSeriesName].Points)
                {
                    if (!dataPoint.IsEmpty)
                    {
                        double endPosition = currentPosition + this.SegmentIntervalWidth;
                        if (dataPoint.YValues[0] >= currentPosition &&
                            dataPoint.YValues[0] < endPosition)
                        {
                            ++count;
                        }

                        // Last segment includes point values on both segment boundaries
                        else if (endPosition >= m_dMaxValue)
                        {
                            if (dataPoint.YValues[0] >= currentPosition &&
                                dataPoint.YValues[0] <= endPosition)
                            {
                                ++count;
                            }
                        }

                        pointCount += count;
                    }
                }

                dFx = (currentPosition - m_dMinValue) * this.SegmentIntervalWidth / (m_dMaxValue - m_dMinValue) + currentPosition;
                dFy = GetdNormal(dFx, m_dMean, m_dStdev);
                dNormal = GetdNormal(m_dMean, m_dMean, m_dStdev);
                chartControl.Series["NormalDistribution"].Points.AddXY(dFx, dFy);

                // Add data point into the histogram series
                histogramSeries.Points.AddXY(currentPosition + this.SegmentIntervalWidth / 2.0, count);
            }

            // Adjust series attributes
            chartControl.Series["NormalDistribution"].YAxisType = AxisType.Secondary;
            chartControl.Series["NormalDistribution"]["LineTension"] = "0.4";
            chartControl.Series["NormalDistribution"].Enabled = VisibleNormalizeLine;


            // Adjust series attributes
            histogramSeries["PointWidth"] = "1";

            // Adjust chart area
            ChartArea chartArea = chartControl.ChartAreas[histogramSeries.ChartArea];
            chartArea.AxisY.Title = "Frequency";
            chartArea.AxisX.Minimum = m_dMinValue;
            chartArea.AxisX.Maximum = m_dMaxValue;

            // Set axis interval based on the histogram class interval
            // and do not allow more than 10 labels on the axis.
            double axisInterval = this.SegmentIntervalWidth;
            while ((m_dMaxValue - m_dMinValue) / axisInterval > 10.0)
            {
                axisInterval *= 2.0;
            }
            chartArea.AxisX.Interval = axisInterval;

            // Set chart area secondary Y axis
            chartArea.AxisY2.Enabled = AxisEnabled.Auto;
            if (this.ShowPercentOnSecondaryYAxis)
            {
                chartArea.RecalculateAxesScale();

                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartArea.AxisY2.LabelStyle.Format = "P0";
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.Title = "Percent of Total";

                //chartArea.AxisY2.Minimum = 0;
                //chartArea.AxisY2.Maximum = 100;
                //chartArea.AxisY2.Maximum = chartArea.AxisY.Maximum / (pointCount / 100.0);
                //double minStep = (chartArea.AxisY2.Maximum > 20.0) ? 5.0 : 1.0;
                //chartArea.AxisY2.Interval = Math.Ceiling((chartArea.AxisY2.Maximum / 5.0 / minStep)) * minStep;
            }

            //2015-04-27-정병주 : Histogram Label 이 누적 되어 겹치는 현상으로 인해 임의로 초기화 하는 기능 추가
            chartArea.AxisX.CustomLabels.Clear();
            chartArea.AxisX.CustomLabels.Add(m_dMinValue
                , m_dMaxValue
                , string.Format("Cp/Cpk:{0:0.00#}/{1:0.00#}", m_dCp, m_dCpk)
                , 1
                , LabelMarkStyle.LineSideMark);
        }

        public static double GetdNormal(double dX, double dMu, double dSigma)
        {
            try
            {
                const double onebys2pi = 0.398942280401433; //=> 1.0 / Math.Sqrt( 2 * Math.PI );

                if (dSigma <= 0)
                {
                    return double.MaxValue;
                }

                double dTemp = (dX - dMu) * (dX - dMu) / (dSigma * dSigma);
                return onebys2pi * Math.Exp(-0.5 * dTemp);
            }
            catch
            {
                return double.MaxValue;
            }
        }

        /// <summary>
        /// Helper method which rounds specified axsi interval.
        /// </summary>
        /// <param name="interval">Calculated axis interval.</param>
        /// <returns>Rounded axis interval.</returns>
        internal double RoundInterval(double interval)
        {
            // If the interval is zero return error
            if (interval == 0.0)
            {
                throw (new ArgumentOutOfRangeException("interval", "Interval can not be zero."));
            }

            // If the real interval is > 1.0
            double step = -1;
            double tempValue = interval;
            while (tempValue > 1.0)
            {
                step++;
                tempValue = tempValue / 10.0;
                if (step > 1000)
                {
                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));
                }
            }

            // If the real interval is < 1.0
            tempValue = interval;
            if (tempValue < 1.0)
            {
                step = 0;
            }

            while (tempValue < 1.0)
            {
                step--;
                tempValue = tempValue * 10.0;
                if (step < -1000)
                {
                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));
                }
            }

            double tempDiff = interval / Math.Pow(10.0, step);
            if (tempDiff < 3.0)
            {
                tempDiff = 2.0;
            }
            else if (tempDiff < 7.0)
            {
                tempDiff = 5.0;
            }
            else
            {
                tempDiff = 10.0;
            }

            // Make a correction of the real interval
            return tempDiff * Math.Pow(10.0, step);
        }


        public void SaveImage(string filePath)
        {
            try
            {
                chartHistogram.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveImage(string filePath, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            try
            {
                chartHistogram.SaveImage(filePath, imgFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void chartHistogram_PostPaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        {
            try
            {
                Series series = (Series)e.ChartElement;
                if (series.Name == "HistogramArea")
                {
                    if (VisibleSpecLimit)
                    {
                        DrawSpecLimit(e);
                    }

                    if(Visible3SigmaLine)
                    {
                        Draw3SigmaLine(e);
                    }

                    if (VisibleHorizonGridLine)
                    {
                    }
                }
            }
            catch
            {
                /// 오류 무시
                /// e.ChartElement 가 Series외에 ChartArea등으로 나올 수 있음
            }
        }

        private void DrawSpecLimit(ChartPaintEventArgs e)
        {
            try
            {
                StringFormat sf3SigmaL = new StringFormat();
                sf3SigmaL.Alignment = StringAlignment.Center;
                sf3SigmaL.LineAlignment = StringAlignment.Near;

                Series series = (Series)e.ChartElement;

                /// 0. Valiable Initialization
                /// /////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///USL
                System.Drawing.PointF posUSL_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posUSL_end = System.Drawing.PointF.Empty;
                ///TARGET
                System.Drawing.PointF posTARGET_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posTARGET_end = System.Drawing.PointF.Empty;
                ///LSL
                System.Drawing.PointF posLSL_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posLSL_end = System.Drawing.PointF.Empty;

                int st = (int)chartHistogram.ChartAreas[series.Name].AxisY.Minimum;
                int end = (int)chartHistogram.ChartAreas[series.Name].AxisY.Maximum;


                /// 2. Draw Line
                /// /////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (double.IsNaN(m_dUSL) == false && m_dUSL != double.MaxValue)
                {
                    /// 2.1. USL Position
                    posUSL_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dUSL);
                    posUSL_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posUSL_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dUSL);
                    posUSL_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posUSL_st = e.ChartGraphics.GetAbsolutePoint(posUSL_st);
                    posUSL_end = e.ChartGraphics.GetAbsolutePoint(posUSL_end);

                    /// 2.1. USL Draw
                    e.ChartGraphics.Graphics.DrawLine(m_oSPEC_CFG.LinePen, posUSL_st, posUSL_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("USL:{0}",m_dUSL)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_oSPEC_CFG.LineBrush
                                    , posUSL_st);

                }

                if (double.IsNaN(m_dTarget) == false && m_dTarget != double.MaxValue)
                {
                    /// 2.2. Target Position
                    posTARGET_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dTarget);
                    posTARGET_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posTARGET_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dTarget);
                    posTARGET_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posTARGET_st = e.ChartGraphics.GetAbsolutePoint(posUSL_st);
                    posTARGET_end = e.ChartGraphics.GetAbsolutePoint(posUSL_end);

                    /// 2.2. Target Draw
                    e.ChartGraphics.Graphics.DrawLine(m_oSPEC_CFG.LinePen, posTARGET_st, posTARGET_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("Target:{0}", m_dTarget)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_oSPEC_CFG.LineBrush
                                    , posTARGET_st);
                }

                if (double.IsNaN(m_dLSL) == false && m_dLSL != double.MinValue)
                {
                    /// 2.3. LSL Position
                    posLSL_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dLSL);
                    posLSL_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posLSL_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dLSL);
                    posLSL_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posLSL_st = e.ChartGraphics.GetAbsolutePoint(posLSL_st);
                    posLSL_end = e.ChartGraphics.GetAbsolutePoint(posLSL_end);

                    /// 2.3. LSL Draw
                    e.ChartGraphics.Graphics.DrawLine(m_oSPEC_CFG.LinePen, posLSL_st, posLSL_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("LSL:{0}", m_dLSL)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_oSPEC_CFG.LineBrush
                                    , posLSL_st);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Draw3SigmaLine(ChartPaintEventArgs e)
        {
            try
            {
                double d3SigmaL = m_dMean - (m_dStdev * 3);
                double d3SigmaU = m_dMean + (m_dStdev * 3);
                StringFormat sf3SigmaL = new StringFormat();
                sf3SigmaL.Alignment = StringAlignment.Center;
                sf3SigmaL.LineAlignment = StringAlignment.Far;

                Series series = (Series)e.ChartElement;

                /// 0. Valiable Initialization
                /// /////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///USL
                System.Drawing.PointF posSigmaU_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posSigmaU_end = System.Drawing.PointF.Empty;
                ///TARGET
                System.Drawing.PointF posMean_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posMean_end = System.Drawing.PointF.Empty;
                ///LSL
                System.Drawing.PointF posSigmaL_st = System.Drawing.PointF.Empty;
                System.Drawing.PointF posSigmaL_end = System.Drawing.PointF.Empty;

                int st = (int)chartHistogram.ChartAreas[series.Name].AxisY.Minimum;
                int end = (int)chartHistogram.ChartAreas[series.Name].AxisY.Maximum;


                /// 2. Draw Line
                /// /////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (double.IsNaN(d3SigmaU) == false && d3SigmaU != double.MaxValue)
                {
                    /// 2.1. USL Position
                    posSigmaU_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, d3SigmaU);
                    posSigmaU_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posSigmaU_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, d3SigmaU);
                    posSigmaU_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posSigmaU_st = e.ChartGraphics.GetAbsolutePoint(posSigmaU_st);
                    posSigmaU_end = e.ChartGraphics.GetAbsolutePoint(posSigmaU_end);

                    /// 2.1. USL Draw
                    e.ChartGraphics.Graphics.DrawLine(m_o3SIGMA_CFG.LinePen, posSigmaU_st, posSigmaU_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("+3∂:{0:0.00#}", d3SigmaU)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_o3SIGMA_CFG.LineBrush
                                    , posSigmaU_end
                                    , sf3SigmaL);

                }

                if (double.IsNaN(m_dMean) == false && m_dMean != double.MaxValue)
                {
                    /// 2.2. Target Position
                    posMean_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dMean);
                    posMean_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posMean_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, m_dMean);
                    posMean_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posMean_st = e.ChartGraphics.GetAbsolutePoint(posMean_st);
                    posMean_end = e.ChartGraphics.GetAbsolutePoint(posMean_end);

                    /// 2.2. Target Draw
                    e.ChartGraphics.Graphics.DrawLine(m_o3SIGMA_CFG.LinePen, posMean_st, posMean_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("Mean:{0:0.00#}", m_dMean)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_o3SIGMA_CFG.LineBrush
                                    , posMean_end
                                    , sf3SigmaL);
                }

                if (double.IsNaN(d3SigmaL) == false && d3SigmaL != double.MinValue)
                {
                    /// 2.3. LSL Position
                    posSigmaL_st.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, d3SigmaL);
                    posSigmaL_st.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, st);

                    posSigmaL_end.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, d3SigmaL);
                    posSigmaL_end.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, end);

                    posSigmaL_st = e.ChartGraphics.GetAbsolutePoint(posSigmaL_st);
                    posSigmaL_end = e.ChartGraphics.GetAbsolutePoint(posSigmaL_end);

                    /// 2.3. LSL Draw
                    /// 
                    e.ChartGraphics.Graphics.DrawLine(m_o3SIGMA_CFG.LinePen, posSigmaL_st, posSigmaL_end);
                    e.ChartGraphics.Graphics.DrawString(string.Format("-3∂:{0:0.00#}", d3SigmaL)
                                    , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                                    , m_o3SIGMA_CFG.LineBrush
                                    , posSigmaL_end
                                    , sf3SigmaL);

                    //e.ChartGraphics.Graphics.DrawString(string.Format("Cp/Cpk:{0:0.00#}/{1:0.00#}", m_dCp,m_dCpk)
                    //                , chartHistogram.ChartAreas[series.Name].AxisX.LabelStyle.Font
                    //                , m_o3SIGMA_CFG.LineBrush
                    //                , posSigmaL_end.X + 30, posSigmaL_end.Y
                    //                , sf3SigmaL);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void copyToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(this.Width, this.Height);
                this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                Clipboard.Clear();
                Clipboard.SetImage(bmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
