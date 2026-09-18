using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using DACrux.Utility;

using DACrux.Framework.RO;

namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : frmUserMonitor<br/>
    /// Summary    : User HitLog Monitoring WinForm Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmUserMonitor : DACrux.Framework.Base.DACruxUXBasic01
    {

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmUserMonitor()
        {
            InitializeComponent();            
        }

        private void frmUserMonitor_Load(object sender, EventArgs e)
        {
            try
            {

                dtpStart.Value = DateTime.Now.AddMonths(-1);
                dtpEnd.Value = DateTime.Now;

                System.Windows.Forms.DataVisualization.Charting.Chart[] chartN = { chart1, chart2, chart3, chart4, chart5 };

                for (int chartNum = 0; chartNum < chartN.Length; chartNum++)
                {
                    //chart background
                    chartN[chartNum].BackColor = System.Drawing.Color.LightYellow;
                    chartN[chartNum].ChartAreas[0].BackColor = System.Drawing.Color.LightSkyBlue;
                    chartN[chartNum].ChartAreas[0].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
                    chartN[chartNum].ChartAreas[0].BorderColor = System.Drawing.Color.Black;
                    chartN[chartNum].ChartAreas[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    chartN[chartNum].ChartAreas[0].BorderWidth = 1;
                    chartN[chartNum].ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartN[chartNum].ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartN[chartNum].ChartAreas[0].AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartN[chartNum].ChartAreas[0].AxisX.Interval = 1;

                    //chart Legends
                    chartN[chartNum].Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                    chartN[chartNum].Legends[0].BackColor = System.Drawing.Color.LightYellow;
                    chartN[chartNum].Legends[0].BorderColor = System.Drawing.Color.DarkKhaki;
                    chartN[chartNum].Legends[0].BorderWidth = 2;
                    chartN[chartNum].Legends[0].Alignment = StringAlignment.Center;
                }

                GetHitLogList();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        #endregion

        #region Get HitLog List

        /// <summary>
        /// Get Hit Log And Display
        /// </summary>
        private void GetHitLogList()
        {
            //DACrux.Interface.iDACruxStatusBar IStatus = (DACrux.Interface.iDACruxStatusBar)this.ParentForm;
            UserMonitor oUserMonitor = null;
            DataTable dt = null;
    
            string strStartTime = string.Empty;
            string strEndTime = string.Empty;
            int TopLebelVal = 0; 
           


            try
            {
                //IStatus.SetMainStatusBarMsg("User monitoring process has started.");
                //IStatus.SetMainStatusBarProgress(0, 0);

                strStartTime = dtpStart.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                strEndTime = dtpEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59";
              
                oUserMonitor = new UserMonitor();

                #region HitRateByFunction

                dt = oUserMonitor.HitRateByFunction(strStartTime, strEndTime);

                if (dt != null && dt.Rows.Count > 0)
                {

                    if ( TexTopLevel.Text.ToUpper() != "ALL" )
                    {
                        if (TexTopLevel.Text.Contains("A") || TexTopLevel.Text.Contains("L") ||TexTopLevel.Text.Contains("a") || TexTopLevel.Text.Contains("l"))
                            return;

                        TopLebelVal = System.Convert.ToInt32(this.TexTopLevel.Text);
                        for (int i = TopLebelVal; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].Delete();
                        }
                    }        
                    dt.AcceptChanges();


                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart4.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                    chart4.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
 
                    chart4.DataSource = dt;
                    chart4.Series[0]["PointWidth"] = "0.6";
                    chart4.Series[0].XValueMember = "FUNC_NAME";
                    chart4.Series[0].YValueMembers = "CNT";
                    
                    chart4.Legends.Clear();                 
                    chart4.Series[0].IsValueShownAsLabel = true;
                    chart4.Series[0].IsXValueIndexed = true;
                    chart4.ChartAreas[0].AxisY.Title = "COUNT";
          
                    chart4.ChartAreas[0].AxisX.Interval = 1;
                    chart4.DataBind();

                    if (dt != null) dt.Dispose();
                    dt = null;
                }

                #endregion

                //IStatus.SetMainStatusBarProgress(100, 20);

                #region UserCountByFunction

                dt = oUserMonitor.UserCountByFunction(strStartTime, strEndTime);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if ("ALL" != TexTopLevel.Text.ToUpper())
                    {
                        TopLebelVal = Int32.Parse(this.TexTopLevel.Text);
                        for (int i = TopLebelVal; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    dt.AcceptChanges();

                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                    chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                    chart2.DataSource = dt;
                    chart2.Series[0]["PointWidth"] = "0.6";
                    chart2.Series[0].XValueMember = "FUNC_NAME";
                    chart2.Series[0].YValueMembers = "CNT";
                    chart2.Legends.Clear();
                    chart2.Series[0].IsXValueIndexed = true;
                    chart2.Series[0].IsValueShownAsLabel = true;

                    chart2.ChartAreas[0].AxisY.Title = "COUNT";
                    chart2.ChartAreas[0].AxisX.Interval = 1;
                    chart2.DataBind();
                    if (dt != null) dt.Dispose();
                    dt = null;
                }

                #endregion

                //IStatus.SetMainStatusBarProgress(100, 40);

                #region DailyTrend

                dt = oUserMonitor.DailyTrend(strStartTime, strEndTime);
                if (dt != null && dt.Rows.Count > 0)
                {
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart3.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                    chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                    chart3.DataSource = dt;
                    chart3.Series[0]["PointWidth"] = "0.6";
                    chart3.Series[0].XValueMember = "DT";
                    chart3.Series[0].YValueMembers = "CNT";
                    chart3.Legends.Clear();
                    chart3.Series[0].IsValueShownAsLabel = true;
                    chart3.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    chart3.Series[0].IsXValueIndexed = true;
                    chart3.ChartAreas[0].AxisX.Title = "DATE";
                    chart3.ChartAreas[0].AxisY.Title = "COUNT";
                    chart3.ChartAreas[0].AxisX.Interval = 1;
                    chart3.DataBind();
                    
                    if (dt != null) dt.Dispose();
                    dt = null;
                }

                #endregion

                //IStatus.SetMainStatusBarProgress(100, 60);

                #region MonthlyTrend

                dt = oUserMonitor.MonthlyTrend(strStartTime, strEndTime);
                if (dt != null && dt.Rows.Count > 0)
                {
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                    chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                    chart1.DataSource = dt;
                    chart1.Series[0]["PointWidth"] = "0.6";
                    chart1.Series[0].XValueMember = "DT";
                    chart1.Series[0].YValueMembers = "CNT";
                    chart1.Series[0].IsValueShownAsLabel = true;
                    chart1.Legends.Clear();
                    chart1.Series[0].IsXValueIndexed = true;
                    chart1.ChartAreas[0].AxisX.Title = "MONTHLY";
                    chart1.ChartAreas[0].AxisY.Title = "COUNT";
                    chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                    chart1.DataBind();
                    if (dt != null) dt.Dispose();
                    dt = null;
                }
                #endregion

                //IStatus.SetMainStatusBarProgress(100, 80);

                #region HitRateByDepartment

                dt = oUserMonitor.HitRateByDepartment(strStartTime, strEndTime);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if ("ALL" != TexTopLevel.Text.ToUpper())
                    {
                        TopLebelVal = Int32.Parse(this.TexTopLevel.Text);
                        for (int i = TopLebelVal; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    dt.AcceptChanges();

                    chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                    
                    double total = System.Convert.ToDouble(dt.Compute("Sum(CNT)",""));
                    dt.Columns.Add("CNTValues", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double yValue = Math.Round(System.Convert.ToDouble(dt.Rows[i]["CNT"]) / total *100 ,2);
                        dt.Rows[i]["CNTValues"] = System.Convert.ToString(dt.Rows[i]["DEPT"]) + '\n' +"(" + yValue + "%  ,"+ Math.Round(System.Convert.ToDouble(dt.Rows[i]["CNT"])) + " )";
                    }
                    chart5.DataSource = dt;
                    chart5.Series[0]["PointWidth"] = "0.6";
                    chart5.Series[0].XValueMember = "CNTValues";
                    chart5.Series[0].YValueMembers = "CNT";
                    //chart5.Legends.Clear();
                    chart5.Series[0].IsVisibleInLegend = true;
                    chart5.Series[0].IsValueShownAsLabel = false;
                    chart5.ChartAreas[0].Area3DStyle.Enable3D = true;
                    chart5.Series[0].IsXValueIndexed = true;
                    chart5.ChartAreas[0].AxisX.Title = "DEPARTMENT";
                    chart5.ChartAreas[0].AxisY.Title = "COUNT";
                    chart5.Series[0]["PieLabelStyle"] = "Outside";
                    chart5.DataBind();

                    if (dt != null) dt.Dispose();
                    dt = null;
                }
                #endregion

                //IStatus.SetMainStatusBarProgress(100, 90);

                #region HitLogDetail

                dt = oUserMonitor.HitLogDetail(strStartTime, strEndTime);
                if (dt != null && dt.Rows.Count > 0)
                {
                    fpSpread1_Sheet1.DataSource = dt;
                    DACrux.Utility.FPSpreadUtil.ReCalcurateColumnWidth(fpSpread1_Sheet1);
                    if (dt != null) dt.Dispose();
                    dt = null;
                }
                #endregion

                //IStatus.SetMainStatusBarProgress(100, 100);


                //IStatus.SetMainStatusBarMsg("Completed !");
            }
            catch(Exception ex)
            {
                //IStatus.SetMainStatusBarMsg("Error has occured !");
                DspError(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

                oUserMonitor = null;
                //IStatus.SetMainStatusBarProgress(0, 0);
            }
        }

        #endregion

        #region Button Click Event

        /// <summary>
        /// Call GetHitLogList()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GetHitLogList();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Exit Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion


        private void tabHitLog_Selected(object sender, TabControlEventArgs e)
        {
            int tabIndex = tabHitLog.SelectedIndex;
            switch(tabIndex)
            {
                case 0:
                    this.topLevelLbl.Enabled = true;
                    this.TexTopLevel.Enabled = true;
                    break;
                case 1:
                    this.topLevelLbl.Enabled = true;
                    this.TexTopLevel.Enabled = true;
                    break;
                case 4:
                    this.topLevelLbl.Enabled = true;
                    this.TexTopLevel.Enabled = true;
                    break;
                default:
                    this.topLevelLbl.Enabled = false;
                    this.TexTopLevel.Enabled = false;
                    break; 
            }                  
        }

        private void frmUserMonitor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void TexTopLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'a' || e.KeyChar == 'l' || e.KeyChar == 'A' || e.KeyChar == 'L')
                return;
                
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
    }
}
