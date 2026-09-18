using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.TEST.ENGUI
{
    public partial class frmBinStackBar : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {
        DataTable dtWafer;

        public frmBinStackBar()
        {
            InitializeComponent();
        }

        private void frmBinStackBar_Load(object sender, EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        void BindData()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable("WAFER");

                for (int a = 0; a < dtWafer.Columns.Count; a++)
                {
                    if (dtWafer.Columns[a].ColumnName.Equals("LOT_ID") ||
                        dtWafer.Columns[a].ColumnName.Equals("WAFER_ID") ||
                        dtWafer.Columns[a].ColumnName.IndexOf("BIN") != -1)
                    {
                        dt.Columns.Add(dtWafer.Columns[a].ColumnName, dtWafer.Columns[a].DataType);
                    }
                }

                for (int a = 0; a < dtWafer.Rows.Count; a++)
                {
                    object[] obj = new object[dt.Columns.Count];
                    int colNo = 0;
                    for (int b = 0; b < dtWafer.Columns.Count; b++)
                    {
                        if (dtWafer.Columns[b].ColumnName.Equals("LOT_ID") ||
                            dtWafer.Columns[b].ColumnName.Equals("WAFER_ID") ||
                            dtWafer.Columns[b].ColumnName.IndexOf("BIN") != -1)
                        {
                            obj[colNo++] = dtWafer.Rows[a][b];
                        }
                    }
                    dt.Rows.Add(obj);
                    colNo = 0;
                }


                //this.chart.DataSource = dt;
                this.fpSpread.ActiveSheet.DataSource = dt;

                FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCellType.FixedPoint = false;

                for (int i = 2; i < fpSpread_Sheet.Columns.Count; i++)
                {
                    fpSpread_Sheet.Columns.Get(i).CellType = numberCellType;
                }


                chart1.Series.Clear();

                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    try
                    {
                        string strSeriesN = Convert.ToString(dt.Columns[j].ColumnName);
                        chart1.Series.Add(strSeriesN);
                        chart1.Series[strSeriesN].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                        //chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.White;
                        //chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.White;
                        chart1.Legends.Clear();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i].IsNull(dt.Columns[j]) == true)
                                dt.Rows[i][j] = 0;

                            object lot_ID = Convert.ToString(dt.Rows[i][0]) + "-" + Convert.ToString(dt.Rows[i][1]);
                            chart1.Series[strSeriesN].Points.AddXY(lot_ID, Convert.ToDouble(dt.Rows[i][j]));
                        }

                        chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                        chart1.ChartAreas["ChartArea1"].AxisY.Interval = 10;

                    }
                    catch
                    {

                        MessageBox.Show(j.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        private void buttonToExcel_Click(object sender, System.EventArgs e)
        {
            try
            {
                DACrux.Utility.ExcelUtilNoStatic excel = new DACrux.Utility.ExcelUtilNoStatic();
                excel.ToExcel(fpSpread, chart1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }
        private DataPoint selectedDataPoint = null;
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (selectedDataPoint != null)
                {
                    selectedDataPoint.IsValueShownAsLabel = false;

                    selectedDataPoint = null;

                    chart1.Invalidate();

                    chart1.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult result = chart1.HitTest(e.X, e.Y);
                string SeriesPoint = string.Empty;

                //for (int i = 0; i < chart1.Series[0].Points.Count; i++)
                //{

                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    for (int p = 0; p < chart1.Series.Count; p++)
                    {

                        for (int s = 0; s < chart1.Series[p].Points.Count; s++)
                        {
                            //point1.BackSecondaryColor = Color.White;
                            //point1.BackHatchStyle = ChartHatchStyle.Percent70;
                            //chart1.Series[SeriesPoint].Points[i].BackHatchStyle = ChartHatchStyle.Percent70;
                            //chart1.Series[p].Points[s].BackSecondaryColor = Color.White;
                            chart1.Series[p].Points[s].BackHatchStyle = ChartHatchStyle.None;
                        }
                    }


                }
                else if(result.ChartElementType == ChartElementType.DataPoint ||
                        result.ChartElementType == ChartElementType.LegendItem)
                {
                    //chart1.Series[i].BackHatchStyle = ChartHatchStyle.Percent70;
                    SeriesPoint = result.Series.Name.ToString();
                    //DataPoint point2 = chart1.Series[SeriesPoint].Points[i];
                    DataPoint tooltipPoint = chart1.Series[SeriesPoint].Points[result.PointIndex];


                    tooltipPoint.ToolTip =
                               chart1.Series[SeriesPoint].Points[result.PointIndex].AxisLabel + "\n"
                               + SeriesPoint
                               + ":" + chart1.Series[SeriesPoint].Points[result.PointIndex].YValues[0].ToString();

                    //chart1.Series[SeriesPoint].Points[i].BackSecondaryColor = Color.Black;
                    for (int k = 0; k < chart1.Series.Count; k++)
                    {
                        if (!chart1.Series[k].Name.Equals(SeriesPoint))
                        {

                            for (int s = 0; s < chart1.Series[k].Points.Count; s++)
                            {
                                chart1.Series[k].Points[s].BackHatchStyle = ChartHatchStyle.Percent50;
                            }
                        }
                        else
                        {
                            for (int s = 0; s < chart1.Series[k].Points.Count; s++)
                            {
                                chart1.Series[k].Points[s].BackHatchStyle = ChartHatchStyle.None;
                                chart1.Series[k].Points[s].BorderWidth = 1;
                                chart1.Series[k].Points[s].MarkerSize = 20;
                            }
                        }
                    }
                }
                else
                {
                    for (int p = 0; p < chart1.Series.Count; p++)
                    {
                        for (int s = 0; s < chart1.Series[p].Points.Count; s++)
                        {
                            chart1.Series[p].Points[s].BackHatchStyle = ChartHatchStyle.None;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult hitresult = chart1.HitTest(e.X, e.Y);
                selectedDataPoint = null;
                if (hitresult.ChartElementType == ChartElementType.DataPoint)
                {
                    selectedDataPoint = (DataPoint)hitresult.Object;
                    selectedDataPoint.IsValueShownAsLabel = true;
                    selectedDataPoint.LabelFormat = "D2";
                    selectedDataPoint.AxisLabel.ToString();
                    chart1.Cursor = Cursors.SizeNS;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chkX_CheckedChanged(object sender, EventArgs e)
        {
            if (chkX.Checked)
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            else
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            RO.ProbeMapAnalysis oRemote = null;
            DataTable dt = null;
            try
            {
                long[] waferSeq = new long[wafer.Length];
                for (int a = 0; a < waferSeq.Length; a++) waferSeq[a] = DACrux.Base.Convert.longParse(wafer[a].WaferSeq);
                oRemote = new RO.ProbeMapAnalysis();
                dt = oRemote.GetWaferInfo(waferSeq);
                dtWafer = dt;
                BindData();

                this.TPWaferList = wafer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            try
            {
                DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[strWafer.Length];
                for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
                {
                    oWafer[iWafer].WaferSeq = strWafer[iWafer];
                }

                DrawWafer(oWafer);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }
    }
}
