using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Framework.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// 2016-03-04-정병주 : Measure Para Data 분석, Histogram, Trand Chart
    /// </summary>
    public partial class frmWaferParaAnalysis : DACrux.Framework.Base.DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]
        DataSet m_dsData = null;
        string m_strPara = "";
        #endregion

        #region [ Create & Close ]
        public frmWaferParaAnalysis()
        {
            InitializeComponent();
        }

        public frmWaferParaAnalysis(DataSet ds, string strPara)
        {
            //이전 화면에서 Dataset 형식으로 Data 를 전달 받는다.
            m_dsData = ds;
            m_strPara = strPara;
            InitializeComponent();

            label3.Text = string.Format("   Test Item : {0}", strPara);
        }

        private void frmWaferParaAnalysis_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (m_dsData != null)
                {
                    FillData(m_dsData, m_strPara);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region [ Event Handler ]

        /// <summary>
        /// Stat 으로 보낸다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnalysis_Click(
            object sender,
            EventArgs e
            )
        {
            //try
            //{
            //    if (fpSpread1.Sheets[0].RowCount <= 0)
            //        return;

            //    this.Cursor = Cursors.WaitCursor;
            //    base.SendToStat((DataTable)fpSpread1.DataSource, cbParaItem.Text);
            //}
            //catch (Exception ex)
            //{
            //    DspError(ex);
            //}
            //finally
            //{
            //    this.Cursor = Cursors.Default;
            //}
        }


        private void btnRedraw_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FillData(m_dsData, m_strPara);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void rdoChartType_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FillData(m_dsData, m_strPara);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion [ Event Handler ]

        #region [ User Method ]
        private void FillData(
            DataSet ds,
            string strParaName
            )
        {
            double dbLower = double.NaN;
            double dbUpper = double.NaN;
            DataTable dtData = null;
            DataTable dtGroup = null;


            //Para Name 이 없으면 Return
            if (string.IsNullOrEmpty(strParaName))
                return;

            this.Cursor = Cursors.WaitCursor;

            //선택한 Para 로 Data 를 불러온다.
            DataRow[] dr = ds.Tables["PARA_ITEM"].Select(string.Format("[PARAM_NAME] = '{0}'", strParaName.ToUpper()));
            if (dr == null || dr.Length == 0) return;

            //Histogram 을 위한 Limit 값을 지정 한다.
            if (double.TryParse(dr[0]["USL"].ToString(), out dbUpper) == false)
                dbUpper = double.NaN;

            if (double.TryParse(dr[0]["LSL"].ToString(), out dbLower) == false)
                dbLower = double.NaN;

            //Histogram 및 Trend 에 die Id 와 Para 정보를 넣는다.
            dtGroup = ds.Tables["MAPDATA"].DefaultView.ToTable(false, "WAFER_SEQ", "DIE_NUM", strParaName.ToUpper()).Select("1=1", "[WAFER_SEQ],[DIE_NUM]").CopyToDataTable<DataRow>();
            if (dtGroup == null || dtGroup.Rows.Count <= 0) return;

            //형식은 decimal 에서 double 로 변경 한다.
            dtData = dtGroup.Clone();
            dtData.Columns.Add(new DataColumn("WAFER_ID", typeof(string)));
            dtData.Columns["WAFER_ID"].SetOrdinal(0);

            foreach (DataRow dRow in dtGroup.Rows)
            {
                //decimal 최대 / 최소 값을 넣을 경우 Chrat 에서 Decimal 관련 오류 발생
                DataRow[] rows = ds.Tables["WAFER_INFO"].Select(String.Format("[WAFER_SEQ] ='{0}'", dRow["WAFER_SEQ"]));

                if (string.IsNullOrEmpty(dRow[strParaName].ToString()) == true)
                    continue;

                decimal oVal = Convert.ToDecimal(dRow[strParaName].ToString());
                if (oVal >= decimal.MaxValue)
                    oVal = decimal.MaxValue / 10;

                if (oVal <= decimal.MinValue)
                    oVal = decimal.MinValue / 10;

                dtData.Rows.Add(new object[] { rows[0]["WAFER_ID"], dRow["WAFER_SEQ"], dRow["DIE_NUM"], oVal });
            }

            dtData.AcceptChanges();

            SeriesChartType ChartType = new SeriesChartType();
            ChartType = SeriesChartType.Line;


            //Chart 를 선택한 대로 출력한다.
            if (rdoCB.Checked == true)
                ChartType = SeriesChartType.Column;
            else if (rdoCL.Checked == true)
                ChartType = SeriesChartType.Line;
            else if (rdoCS.Checked == true)
                ChartType = SeriesChartType.Point;


            // Chart 그리기.
            chart1.Series.Clear();
            foreach (DataRow row1 in ds.Tables["WAFER_INFO"].Rows)
            {
                Series series = chart1.Series.Add(row1["WAFER_ID"].ToString());
                DataRow[] rows = dtData.Select(String.Format("[WAFER_SEQ] = '{0}'", row1["WAFER_SEQ"].ToString()));


                DataPoint dp = null;
                series.Points.Clear();
                series.ChartType = ChartType;
                foreach (DataRow row2 in rows)
                {
                    dp = new DataPoint();
                    dp.SetValueXY(row2["DIE_NUM"], row2[strParaName]);
                    dp.ToolTip = String.Format("WAFER ID: {0}\r\nDIE NUM: {1}\r\nVALUE: {2}", row2["WAFER_ID"], row2["DIE_NUM"], row2[strParaName]);
                    series.Points.Add(dp);
                }

                series.SmartLabelStyle.Enabled = true;
                series.MarkerSize = 6;
                series.MarkerStyle = MarkerStyle.Circle;
                //series.Color = Color.Blue;
                series.BorderWidth = 2;
                //chart1.Series.Add(series);
            }

            chart1.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["Default"].CursorX.AutoScroll = true;
            chart1.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
            chart1.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
            chart1.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;

            chart1.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
            chart1.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);

            chart1.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

            chart1.ChartAreas["Default"].AxisX.ScaleView.Position = 0;
            chart1.ChartAreas["Default"].AxisX.ScaleView.MinSize = 1;
            chart1.ChartAreas["Default"].AxisY.ScaleView.MinSize = 1;
            chart1.ChartAreas["Default"].AxisY2.ScaleView.MinSize = 1;

            chart1.ChartAreas["Default"].AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chart1.ChartAreas["Default"].AxisX.Interval = 1;
            chart1.ChartAreas["Default"].RecalculateAxesScale();
            chart1.ChartAreas["Default"].AxisY.IsStartedFromZero = false;


            //Histogram 그리기
            try
            {
                histogram1.DataSource = dtData;

                histogram1.USL = double.NaN;
                histogram1.LSL = double.NaN;

                if (ChkLimit.Checked == true)
                {
                    histogram1.USL = dbUpper;
                    histogram1.LSL = dbLower;
                }

                histogram1.HistogramVisible = true;
                histogram1.DrawHistogram(strParaName.ToUpper());
            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries)[0];
                //Interval 이 0일 경우 Error 발생.
            }

            dtData.AcceptChanges();

            DACrux.Utility.Component.InitSpread(fpSpread1);
            DACrux.Utility.Component.SetSpreadData(dtData, fpSpread1_Sheet1, 100, 0, true, false);
            DACrux.Utility.FPSpreadUtil.VisibleSpreadColumns(fpSpread1_Sheet1, new string[] { "WAFER_SEQ" }, false);
            DACrux.Utility.Component.SetColumnTypeNumber(ref fpSpread1, fpSpread1_Sheet1.ColumnCount - 1, 3);
            //fpSpread1_Sheet1.RowHeader.Columns[0].Width += 10;
        }

        public void ExportExcel()
        {
            throw new NotImplementedException();
        }

        #endregion [ User Method ]
    }
}
