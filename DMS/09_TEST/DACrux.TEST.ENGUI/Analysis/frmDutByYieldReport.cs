using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmDutByYieldReport : DACrux.Framework.Base.DACruxUXBasic00
    {
        #region [ Data Field ]
        DataTable m_dtData = null;
        bool m_bIsAll = true;
        #endregion

        #region [ Create & Close ]
        public frmDutByYieldReport()
        {
            InitializeComponent();
        }

        public frmDutByYieldReport(DataTable dt, bool IsAll)
        {
            m_dtData = dt;
            m_bIsAll = IsAll;

            InitializeComponent();
        }

        private void frmDutByYieldReport_Load(object sender, EventArgs e)
        {
            try
            {
                pbCollapse1.Image = DACrux.TEST.ENGUI.Properties.Resources.Expand_large;
                splitContainer1.Panel2Collapsed = true;

                if (m_dtData != null && m_dtData.Rows.Count > 0)
                    FillData(m_dtData, false);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }
        #endregion

        #region [ Control Method ]
        private void pbCollapse1_Click(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;

                if (splitContainer1.Panel1Collapsed == false)
                    pbCollapse1.Image = DACrux.TEST.ENGUI.Properties.Resources.Collapse_large;
                else
                    pbCollapse1.Image = DACrux.TEST.ENGUI.Properties.Resources.Expand_large;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void chartTrend_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void chartTrend2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                System.Windows.Forms.DataVisualization.Charting.HitTestResult ht = ((System.Windows.Forms.DataVisualization.Charting.Chart)sender).HitTest(e.X, e.Y);

                if (ht.ChartArea == null)
                    return;

                for (int i = 0; i < chartTrend2.ChartAreas.Count; i++)
                {
                    if (ht.ChartArea != chartTrend2.ChartAreas[i])
                        chartTrend2.ChartAreas[i].Visible = !chartTrend2.ChartAreas[i].Visible;
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (fpSpread1.ActiveSheet.Rows.Count > 0)
                {
                    SaveFileDialog saveFileExcel = new SaveFileDialog();

                    saveFileExcel.Filter = "Excel File (*.xls;*.xlsx)|*.xls;*.xlsx";
                    saveFileExcel.AddExtension = true;
                    saveFileExcel.FileName = "";
                    saveFileExcel.RestoreDirectory = true;

                    if (saveFileExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        fpSpread1.ActiveSheet.Protect = false;
                        // Farpoint 자체 버그 (SaveCustomColumnHeaders를 사용할 경우 엑셀로 저장시 마지막 한줄을 누락시킨다.)
                        fpSpread1.ActiveSheet.RowCount = fpSpread1.ActiveSheet.RowCount + 1;
                        fpSpread1.SaveExcel(saveFileExcel.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                    }
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //DACrux.Interface.iDACruxStatContainer IStat = (DACrux.Interface.iDACruxStatContainer)this.MdiParent;
                //if (IStat != null) IStat.SendToStat((DataTable)fpSpread1.DataSource);
                //else
                //{
                //    IStat = (DACrux.Interface.iDACruxStatContainer)this.ParentForm.MdiParent;
                //    if (IStat != null) IStat.SendToStat((DataTable)fpSpread1.DataSource);
                //}
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region [ User Method ]
        private void FillData(DataTable dt, bool IsRedraw)
        {
            try
            {
                int nMaxDut = -1;
                int nMinDut = -1;

                // Dut Yield Table을 만든다.
                DataTable dtDut = dt.DefaultView.ToTable(true, "DUT_ID");
                dtDut.Columns.Add("DUT", typeof(string));
                dtDut.Columns.Add("YIELD", typeof(double));
                dtDut.Columns.Add("YIELD_LABEL", typeof(string));

                if (string.IsNullOrEmpty(dtDut.Compute("MAX([DUT_ID])", "1=1").ToString()) == true)
                {
                    MessageBox.Show("Dut 정보가 없습니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(dtDut.Compute("MIN([DUT_ID])", "1=1").ToString()) == true)
                {
                    MessageBox.Show("Dut 정보가 없습니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                nMaxDut = DACrux.Base.Convert.intParse(dtDut.Compute("MAX([DUT_ID])", "1=1").ToString());
                nMinDut = DACrux.Base.Convert.intParse(dtDut.Compute("MIN([DUT_ID])", "1=1").ToString());

                if (IsRedraw == false)
                {
                    numFromDut.Value = nMinDut;
                    numToDut.Value = nMaxDut;
                }

                DataSet dsDutData = new DataSet();
                int[] nTotalCnt = new int[nMaxDut];

                for (int i = 0; i < dtDut.Rows.Count; i++)
                {
                    // Dut 별 Bin Portion Table을 만든다.
                    string sDutID = dtDut.Rows[i]["DUT_ID"].ToString();

                    DataTable SelectedDut = dt.Select(string.Format("[DUT_ID] = '{0}'", sDutID)).CopyToDataTable<DataRow>();
                    dsDutData.Tables.Add(SelectedDut.DefaultView.ToTable(true, "BIN"));
                    dsDutData.Tables[dsDutData.Tables.Count - 1].TableName = "DUT " + sDutID;
                    dsDutData.Tables[dsDutData.Tables.Count - 1].Columns.Add("BIN_NAME", typeof(string));
                    dsDutData.Tables[dsDutData.Tables.Count - 1].Columns.Add("PORTION", typeof(double));
                    dsDutData.Tables[dsDutData.Tables.Count - 1].Columns.Add("PORTION_LABEL", typeof(string));

                    double dbTotalCnt = DACrux.Base.Convert.doubleParse(SelectedDut.Rows.Count.ToString());
                    double dbGoodCnt = DACrux.Base.Convert.doubleParse(SelectedDut.Select("[PF_FLAG] = 'P'").Length.ToString());

                    nTotalCnt[DACrux.Base.Convert.intParse(sDutID) - 1] = (int)dbTotalCnt;

                    // Bin 별 Portion 정보를 입력한다.
                    for (int j = 0; j < dsDutData.Tables[dsDutData.Tables.Count - 1].Rows.Count; j++)
                    {
                        double dbBinCnt = DACrux.Base.Convert.doubleParse(SelectedDut.Select(string.Format("[BIN] = {0}", dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["BIN"].ToString())).Length.ToString());
                        double dbPortion = Math.Round(dbBinCnt / dbTotalCnt * 100, 2);
                        dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["BIN_NAME"] = "BIN " + dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["BIN"].ToString();
                        dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["PORTION"] = dbPortion;
                        dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["PORTION_LABEL"] = string.Format("{0}% ({1},{2})", dbPortion, dbBinCnt, dsDutData.Tables[dsDutData.Tables.Count - 1].Rows[j]["BIN"].ToString());
                    }
                    dsDutData.Tables[dsDutData.Tables.Count - 1].AcceptChanges();

                    if (m_bIsAll == true)
                    {
                        // Dut 별 수율 정보를 입력한다.
                        double dbYield = Math.Round(dbGoodCnt / dbTotalCnt * 100, 2);

                        dtDut.Rows[i]["DUT"] = "DUT " + sDutID;
                        dtDut.Rows[i]["YIELD"] = dbYield.ToString();
                        dtDut.Rows[i]["YIELD_LABEL"] = string.Format("{0}%({1},{2})", dbYield, dbGoodCnt, dbTotalCnt);
                    }
                    else
                    {
                        double dbYield = Math.Round(dbTotalCnt / dt.Rows.Count *100, 2);

                        dtDut.Rows[i]["DUT"] = "DUT " + sDutID;
                        dtDut.Rows[i]["YIELD"] = dbYield.ToString();
                        dtDut.Rows[i]["YIELD_LABEL"] = string.Format("{0}%({1},{2})", dbYield, dbTotalCnt, dt.Rows.Count);
                    }
                }

                dtDut.AcceptChanges();

                // 차트 초기화
                DACrux.Utility.Component.InitMSChart(ref chartTrend);
                chartTrend2.Titles.Clear();
                chartTrend2.Legends.Clear();
                chartTrend2.Series.Clear();
                chartTrend2.ChartAreas.Clear();
                chartTrend2.BackColor = System.Drawing.Color.LightYellow;
                chartTrend2.Titles.Add("Default");
                chartTrend2.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
                chartTrend2.Titles[0].Text = chartTrend2.Text;

                // Dut 수율 차트 데이터 바인딩
                chartTrend.ChartAreas["Default"].AxisY.LabelStyle.Enabled = false;
                chartTrend.ChartAreas["Default"].AxisY.MajorTickMark.Enabled = false;
                chartTrend.ChartAreas["Default"].AxisY.Maximum = 100;
                chartTrend.ChartAreas["Default"].AxisX.LabelStyle.Interval = 1;
                chartTrend.ChartAreas["Default"].AxisX.MajorTickMark.Enabled = false;
                chartTrend.Legends["Default"].Enabled = false;
                chartTrend.Series.Add("DUT_YIELD");
                chartTrend.Series["DUT_YIELD"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chartTrend.Series["DUT_YIELD"].IsValueShownAsLabel = true;
                chartTrend.Series["DUT_YIELD"].Points.DataBind(dtDut.Select(string.Format("[DUT_ID] >= {0} AND [DUT_ID] <= {1}", numFromDut.Value, numToDut.Value), "[DUT_ID] DESC"), "DUT", "YIELD", "Label=YIELD_LABEL");
                
                for (int i = chartTrend.Series["DUT_YIELD"].Points.Count - 1; i >= 0; i--)
                {
                    //Color backColor = DACrux.Utility.Component.GetColor(i);
                    //chartTrend.Series["DUT_YIELD"].Points[i].Color = backColor;
                    
                    string sTableName = chartTrend.Series["DUT_YIELD"].Points[i].AxisLabel;
                    int nIndex = dsDutData.Tables.IndexOf(sTableName);
                    int nTotalIdx = DACrux.Base.Convert.intParse(sTableName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]) - 1;

                    chartTrend2.ChartAreas.Add(sTableName);
                    //chartTrend2.ChartAreas[sTableName].BackColor = Color.FromArgb(100, backColor);
                    chartTrend2.ChartAreas[sTableName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
                    chartTrend2.ChartAreas[sTableName].BorderColor = System.Drawing.Color.Black;
                    chartTrend2.ChartAreas[sTableName].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    chartTrend2.ChartAreas[sTableName].BorderWidth = 1;
                    chartTrend2.ChartAreas[sTableName].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartTrend2.ChartAreas[sTableName].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartTrend2.ChartAreas[sTableName].AxisY.MajorTickMark.Enabled = false;
                    chartTrend2.ChartAreas[sTableName].AxisY.LabelStyle.Enabled = false;
                    chartTrend2.ChartAreas[sTableName].AxisY.Title = string.Format("{0} ({1}ea)", sTableName, nTotalCnt[nTotalIdx]);
                    chartTrend2.ChartAreas[sTableName].AxisY.Maximum = 100;
                    chartTrend2.ChartAreas[sTableName].AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                    chartTrend2.ChartAreas[sTableName].AxisX.LabelStyle.Interval = 1;
                    chartTrend2.ChartAreas[sTableName].AxisX.MajorTickMark.Enabled = false;

                    chartTrend2.Series.Add(sTableName);
                    chartTrend2.Series[sTableName].ChartArea = sTableName;
                    chartTrend2.Series[sTableName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chartTrend2.Series[sTableName].IsValueShownAsLabel = true;
                    chartTrend2.Series[sTableName].Points.DataBind(dsDutData.Tables[nIndex].Select("1=1", "[BIN] DESC"), "BIN_NAME", "PORTION", "Label=PORTION_LABEL");
                }

                DACrux.Utility.Component.InitSpread(fpSpread1);
                DACrux.Utility.Component.SetSpreadData(dt, fpSpread1_Sheet1, 100, 0, true, false);
                DACrux.Utility.Component.SetDecimalPlaces(fpSpread1_Sheet1, new int[] { 9, 10 }, new int[] { 2, 2 }, false);
                fpSpread1_Sheet1.RowHeader.Columns[0].Width += 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void btnRedraw_Click(object sender, EventArgs e)
        {
            try
            {
                FillData(m_dtData, true);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }
    }
}
