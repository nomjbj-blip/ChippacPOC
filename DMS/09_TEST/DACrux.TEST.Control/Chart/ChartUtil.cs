using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace DACrux.TEST.Control
{
    static class ChartLimitSeries
    {
        public const string USL = "USL";
        public const string LSL = "LSL";
        public const string UCL = "UCL";
        public const string LCL = "LCL";
    }

    //-----------------------------------------------------------------------------------------------------------------------

    public static class ChartUtil
    {
        //-----------------------------------------------------------------------------------------------------------------------

        public static void DrawLimitLine(Chart chart, string key, Dictionary<string, double[]> LIMIT)
        {
            HorizontalLineAnnotation annH = null;
            VerticalLineAnnotation annV = null;

            try
            {
                ChartArea chartArea = chart.ChartAreas[key];
                //if (chart.Annotations.Count > 0) chart.Annotations.Clear();
                foreach (KeyValuePair<string, double[]> kv in LIMIT)
                {
                    System.Diagnostics.Debug.WriteLine(kv.Key);

                    if (kv.Value == null || kv.Value.Length == 0) continue;

                    // --
                    double[] VALUE = kv.Value;
                    double currValue = double.NaN;
                    int idxS = 0;
                    annH = null;
                    for (int idx = 0; idx < VALUE.Length; idx++)
                    {
                        // Value 값이 NaN 이면 
                        if (double.IsNaN(VALUE[idx]))
                        {
                            if (annH != null)
                            {
                                annH.Width = idxS;
                                chart.Annotations.Add(annH);
                                // --
                                idxS = 0;
                                currValue = double.NaN;
                                annH = null;
                            }

                            continue;
                        }

                        idxS++;
                        if (double.Equals(VALUE[idx], currValue)) continue;

                        if (annH != null)
                        {
                            int offset = 0;
                            if (double.IsNaN(VALUE[idx]) == false)
                            {
                                // 수직라인
                                annV = InitVerticalLineAnnotation(kv.Key, chartArea, idx + 0.5, currValue, 0);
                                annV.Height = VALUE[idx] - currValue;
                                chart.Annotations.Add(annV);

                                // --
                                offset = 1;
                            }
                            if (double.Equals(annH.AnchorX, 0.0))
                                annH.Width = idxS - offset + 0.5;
                            else
                                annH.Width = idxS - offset;
                            chart.Annotations.Add(annH);
                            // --
                            annH = null;
                            idxS = 0 + offset;
                        }
                        if (idx == 0)
                            annH = InitHorizontalLineAnnotation(kv.Key, chartArea, idx, VALUE[idx], 0);
                        else
                            annH = InitHorizontalLineAnnotation(kv.Key, chartArea, idx + 0.5, VALUE[idx], 0);

                        // --
                        currValue = VALUE[idx];
                    }

                    if (annH != null)
                    {
                        // 
                        if (double.Equals(annH.AnchorOffsetX, 0.0))
                            annH.Width = idxS + 1;
                        else
                            annH.Width = idxS + 0.5;
                        // --
                        chart.Annotations.Add(annH);

                        // --
                        // 마지막이면 Spec 
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (annH != null) annH = null;
                if (annV != null) annV = null;
            }
        }

        private static HorizontalLineAnnotation InitHorizontalLineAnnotation(string LimitLineName, ChartArea chartArea, double X, double Y, double offset)
        {
            HorizontalLineAnnotation ann = null;
            try
            {
                ann = new HorizontalLineAnnotation();
                ann.AxisX = chartArea.AxisX;
                ann.AxisY = chartArea.AxisY;
                ann.IsSizeAlwaysRelative = false;
                ann.Y = Y;
                ann.AnchorX = X;
                ann.AnchorOffsetX = offset;
                //ann.Width = width;
                ann.IsInfinitive = false;
                ann.ClipToChartArea = chartArea.Name;
                //ann.LineColor = Color.Red;
                ann.LineWidth = 2;
                ann.LineDashStyle = ChartDashStyle.Dash;

                switch (LimitLineName)
                {
                    case ChartLimitSeries.USL:
                    case ChartLimitSeries.LSL:
                        ann.LineColor = Color.MediumVioletRed;
                        break;

                    case ChartLimitSeries.UCL:
                    case ChartLimitSeries.LCL:
                        ann.LineColor = Color.MediumBlue;
                        break;

                    default:
                        ann.LineColor = Color.YellowGreen;
                        break;
                }

                return ann;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static VerticalLineAnnotation InitVerticalLineAnnotation(string LimitLineName, ChartArea chartArea, double X, double Y, double offset)
        {
            VerticalLineAnnotation ann = null;
            try
            {
                ann = new VerticalLineAnnotation();
                ann.AxisX = chartArea.AxisX;
                ann.AxisY = chartArea.AxisY;
                ann.IsSizeAlwaysRelative = false;
                ann.AnchorX = X;
                ann.Y = Y;
                ann.AnchorOffsetX = offset;
                ann.AnchorOffsetY = offset;
                //ann.Height = height;
                ann.IsInfinitive = false;
                ann.ClipToChartArea = chartArea.Name;
                //ann.LineColor = Color.Red;
                ann.LineDashStyle = ChartDashStyle.Dash;
                ann.LineWidth = 2;

                switch (LimitLineName)
                {
                    case ChartLimitSeries.USL:
                    case ChartLimitSeries.LSL:
                        ann.LineColor = Color.MediumVioletRed;
                        break;

                    case ChartLimitSeries.UCL:
                    case ChartLimitSeries.LCL:
                        ann.LineColor = Color.MediumBlue;
                        break;

                    default:
                        ann.LineColor = Color.YellowGreen;
                        break;
                }

                return ann;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
    }
}
