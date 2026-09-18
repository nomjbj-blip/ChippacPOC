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
    public partial class frmWaferBinReport
        : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        private DataSet dsWafer;
        private DACrux.TEST.Control.PopUpAxisY oYAxis = null;

        List<Series> _excludeSeries = new List<Series>();

        private string[] strHeaderBase = new string[] { "DEVICE_ALIAS", "PRODUCT", "TESTAREA", "PROGRAM", "LOT_ID", "WAFER_ID", "START_TIME", "END_TIME", "YIELD", "TESTED_DIE" };

        public frmWaferBinReport()
        {
            InitializeComponent();
        }

        private void frmWaferBinReport_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
        }

        #region User Method


        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            RO.ProbeMapAnalysis oRemote = null;
            dsWafer = new DataSet();

            bool bFlag = true;
            long[] waferSeq = new long[wafer.Length];
            for (int a = 0; a < waferSeq.Length; a++)
            {
                if (!base.RESV_03.Contains(wafer[a].Testarea))
                {
                    bFlag = false;
                    break;
                }
                waferSeq[a] = DACrux.Base.Convert.longParse(wafer[a].WaferSeq);
            }

            if (!bFlag)
                return;

            oRemote = new RO.ProbeMapAnalysis();
            dsWafer = oRemote.GetWaferBinReport(waferSeq, rbtWafer.Checked == true ? true : false);
            if (dsWafer == null || dsWafer.Tables.Count <= 0)
                throw new Exception("Not Found Data");

            fnReportProcess();

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

        /// <summary>
        /// Chart 및 sheet 상에 Data 를 Bind 한다.
        /// </summary>
        private void fnReportProcess()
        {
            try
            {
                MainForm.SetStatusMessage("Chart 를 그리는 중입니다.");

                string strXValue = string.Empty;
                string strSeries = string.Empty;
                double dMaxTestDie = double.MinValue;
                List<string> lsHeader = new List<string>();

                if (dsWafer == null || dsWafer.Tables.IndexOf("WAFER_INFO") < 0 || dsWafer.Tables.IndexOf("BIN_INFO") < 0)
                    return;

                if (rbtWafer.Checked)
                    strHeaderBase = new string[] { "DEVICE_ALIAS", "PRODUCT", "TESTAREA", "PROGRAM", "LOT_ID", "WAFER_ID", "START_TIME", "END_TIME", "YIELD", "TESTED_DIE", "DEFECT_RATE", "DEFECT_CNT" };
                else
                    strHeaderBase = new string[] { "DEVICE_ALIAS", "PRODUCT", "TESTAREA", "PROGRAM", "LOT_ID", "START_TIME", "END_TIME", "YIELD", "TESTED_DIE", "DEFECT_RATE", "DEFECT_CNT" };

                lsHeader.AddRange(strHeaderBase);
                for (int iBin = 0; iBin < dsWafer.Tables["BIN_INFO"].Rows.Count; iBin++)
                {
                    if (chkRate.Checked)
                        lsHeader.Add(string.Format("YLD_BIN{0}", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"]));
                    else
                        lsHeader.Add(string.Format("BIN{0}", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"]));
                }

                DataTable dtRowData = dsWafer.Tables["WAFER_INFO"].DefaultView.ToTable(false, lsHeader.ToArray()).Copy();

                //Sheet 상에 Data Bind
                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread);
                fpSpread_Sheet.DataSource = dtRowData;

                //Header의 정보를 Bin Desc 정보로 치환해준다.
                FarPoint.Win.Spread.CellType.NumberCellType BinCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                if (chkRate.Checked)
                    BinCellType.DecimalPlaces = 2;
                else
                    BinCellType.DecimalPlaces = 0;

                BinCellType.Separator = ",";

                //Sheet 상의 Columns 명을 DESC 로 변경 한다.
                for (int ic = 0; ic < fpSpread_Sheet.Columns.Count; ic++)
                {
                    string strColumnsName = fpSpread_Sheet.Columns[ic].Label;

                    if (strColumnsName.StartsWith("BIN") == true || strColumnsName.StartsWith("YLD_BIN") == true)
                    {
                        string strBinNo = string.Empty;

                        if (strColumnsName.StartsWith("YLD_BIN") == true)
                            strBinNo = strColumnsName.Replace("YLD_BIN", "");
                        else
                            strBinNo = strColumnsName.Replace("BIN", "");

                        if (dsWafer.Tables.IndexOf("BIN_INFO") > -1)
                        {
                            //정의된 Bin 정보가 없을 경우 보여 주지 않는다.
                            DataRow[] drHeader = dsWafer.Tables["BIN_INFO"].Select(string.Format("BIN = '{0}'", strBinNo));

                            if (drHeader.Length > 0)
                            {
                                strColumnsName = string.Format("{0}({1})", drHeader[0]["BIN"], drHeader[0]["BIN_NAME"]);
                                fpSpread_Sheet.Columns[ic].Label = strColumnsName;
                                fpSpread_Sheet.Columns[ic].CellType = BinCellType;
                            }
                            else
                                fpSpread_Sheet.Columns[ic].Visible = false;
                        }
                    }
                    else if (strColumnsName == "DEFECT_CNT" || strColumnsName == "TESTED_DIE")
                        fpSpread_Sheet.Columns[ic].CellType = BinCellType;
                }

                //Column 을 Remove 해야 Excel Export 시 Data 가 없는 Column 이 안보인다.
                for (int ic = fpSpread_Sheet.Columns.Count; ic <= 0; ic--)
                {
                    if (fpSpread_Sheet.Columns[ic].Visible == false)
                        fpSpread_Sheet.Columns[ic].Remove();
                }

                fpSpread_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpSpread_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
                fpSpread_Sheet.Models.Selection.ClearSelection();
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Sheet);

                //Chart Draw
                BinChart.Series.Clear();
                BinChart.Titles.Clear();
                _excludeSeries.Clear();

                //chart Point 입력
                for (int ir = 0; ir < dtRowData.Rows.Count; ir++)
                {
                    double iTotalCount = int.Parse(dtRowData.Rows[ir]["TESTED_DIE"].ToString());

                    if (rbtWafer.Checked)
                        strXValue = dtRowData.Rows[ir]["WAFER_ID"].ToString();
                    else
                        strXValue = dtRowData.Rows[ir]["LOT_ID"].ToString();

                    Series series = null;
                    DataPoint dp = null;
                    object y = null;

                    //--

                    #region [ Bin에 대한 Series ]
                    for (int iBin = 0; iBin < dsWafer.Tables["BIN_INFO"].Rows.Count; iBin++)
                    {
                        string strBin = string.Format("BIN{0}", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"].ToString());

                        if (chkRate.Checked)
                            strBin = string.Format("YLD_BIN{0}", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"].ToString());
                        else
                            strBin = string.Format("BIN{0}", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"].ToString());

                        strSeries = string.Format("{0}({1})", dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"].ToString(), dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN_NAME"].ToString());

                        //Series 생성
                        if (BinChart.Series.IndexOf(strSeries) < 0)
                        {
                            series = new Series(strSeries);
                            series.ChartType = SeriesChartType.StackedColumn;
                            series.MarkerStyle = MarkerStyle.None;
                            series.MarkerSize = 0;
                            series.SmartLabelStyle.Enabled = true;
                            series.MarkerSize = 2;
                            series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                            series.BorderWidth = 2;
                            series.ChartType = SeriesChartType.StackedColumn;
                            series.YAxisType = AxisType.Secondary;
                            series.Tag = dsWafer.Tables["BIN_INFO"].Rows[iBin]["BIN"].ToString();

                            BinChart.Series.Add(series);
                            _excludeSeries.Add(series);
                        }

                        y = dtRowData.Rows[ir][strBin];

                        dp = new DataPoint(new Series(strSeries));

                        if (!String.IsNullOrEmpty(y.ToString()))
                        {
                            dp.Tag = String.Format("{0}", y);
                            dp.ToolTip = String.Format("{0} / {1} / {2}", strXValue, strSeries, y);
                            dp.BorderWidth = 2;
                            dp.SetValueXY(strXValue, y);
                        }
                        else
                        {
                            dp.IsEmpty = true;
                        }

                        BinChart.Series[strSeries].Points.Add(dp);
                    }
                    #endregion [ Bin에 대한 Series ]

                    //--

                    #region [ Yield 에 대한 Series ]

                    y = dtRowData.Rows[ir]["YIELD"];
                    if (BinChart.Series.IndexOf("YIELD") < 0)
                    {
                        series = new Series("YIELD");
                        series.ChartType = SeriesChartType.Line;
                        series.Color = Color.FromArgb(217, 43, 4);
                        series.YAxisType = AxisType.Primary;

                        //--

                        BinChart.Series.Add(series);
                        _excludeSeries.Add(series);
                    }

                    //--

                    dp = new DataPoint(new Series("YIELD"));
                    //dp.Label = String.Format("{0}", y);
                    dp.Tag = String.Format("{0}", y);
                    dp.ToolTip = String.Format("{0} / {1} / {2}", strXValue, strSeries, y);
                    dp.BorderWidth = 2;
                    //dp.Color = m_ColorSet[DACrux.Base.Convert.intParse(strBinNo)];
                    dp.SetValueXY(strXValue, y);

                    BinChart.Series["YIELD"].Points.Add(dp);

                    //--

                    if (BinChart.Series.IndexOf("YIELD_POINT") < 0)
                    {
                        series = new Series("YIELD_POINT");
                        series.ChartType = SeriesChartType.Point;
                        series.Color = Color.FromArgb(217, 43, 4);
                        series.YAxisType = AxisType.Primary;
                        series.IsVisibleInLegend = false;

                        //--

                        BinChart.Series.Add(series);
                    }

                    //--

                    dp = new DataPoint(new Series("YIELD_POINT"));
                    //dp.Label = String.Format("{0}", y);
                    dp.Tag = String.Format("{0}", y);
                    dp.ToolTip = String.Format("{0} / {1} / {2}", strXValue, strSeries, y);
                    dp.BorderWidth = 2;
                    //dp.Color = m_ColorSet[DACrux.Base.Convert.intParse(strBinNo)];
                    dp.SetValueXY(strXValue, y);

                    BinChart.Series["YIELD_POINT"].Points.Add(dp);

                    #endregion [ Yield 에 대한 Series ]

                    dMaxTestDie = Math.Max(double.Parse(dtRowData.Rows[ir]["TESTED_DIE"].ToString()), dMaxTestDie);
                }

                for (int isr = 0; isr < BinChart.Series.Count; isr++)
                {
                    string strBinNo = (string)BinChart.Series[isr].Tag;

                    if (string.IsNullOrEmpty(strBinNo) == false)
                    {
                        DataRow[] drBin = dsWafer.Tables["BIN_INFO"].Select(string.Format("BIN = {0}", strBinNo));
                        foreach (DataRow dr in drBin)
                        {
                            string color = dr["BIN_COLOR"].ToString();
                            if (string.IsNullOrEmpty(color) == false && color != "#FFFFFF")
                            {
                                BinChart.Series[isr].Color = ColorTranslator.FromHtml(color);
                            }
                            else
                            {
                                BinChart.Series[isr].Color = DACrux.TEST.Control.Util.GetColor(int.Parse(strBinNo));
                            }
                        }
                    }
                }

                //Zoom 관련 속성
                BinChart.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                BinChart.ChartAreas["Default"].CursorX.AutoScroll = true;
                BinChart.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                BinChart.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                BinChart.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                BinChart.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                BinChart.ChartAreas["Default"].AxisY2.ScaleView.Zoomable = true;
                BinChart.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();
                BinChart.ChartAreas["Default"].AxisY.ScaleView.ZoomReset();
                BinChart.ChartAreas["Default"].AxisY2.ScaleView.ZoomReset();

                BinChart.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                BinChart.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                BinChart.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                BinChart.ChartAreas["Default"].AxisX.Interval = 1;
                BinChart.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                BinChart.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                BinChart.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90;


                BinChart.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                BinChart.ChartAreas["Default"].AxisX.Minimum = 0;

                BinChart.ChartAreas["Default"].AxisY.Maximum = 100;
                BinChart.ChartAreas["Default"].AxisY.Minimum = 0;
                BinChart.ChartAreas["Default"].AxisY.Title = "수율 (%)";
                BinChart.ChartAreas["Default"].AxisY.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                BinChart.ChartAreas["Default"].AxisY.LabelStyle.Format = "{0.#}%";

                if (chkRate.Checked == true)
                {
                    BinChart.ChartAreas["Default"].AxisY2.Minimum = 0;
                    BinChart.ChartAreas["Default"].AxisY2.Maximum = 100;
                    BinChart.ChartAreas["Default"].AxisY2.Title = "Bin 별 Count (Rate)";
                    BinChart.ChartAreas["Default"].AxisY2.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                    BinChart.ChartAreas["Default"].AxisY2.LabelStyle.Format = "{0.#}%";
                }
                else
                {
                    BinChart.ChartAreas["Default"].AxisY2.Minimum = 0;
                    BinChart.ChartAreas["Default"].AxisY2.Maximum = Math.Max(dMaxTestDie + (dMaxTestDie * 0.2), 10);
                    BinChart.ChartAreas["Default"].AxisY2.Title = "Bin 별 Count (ea)";
                    BinChart.ChartAreas["Default"].AxisY2.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                    BinChart.ChartAreas["Default"].AxisY2.LabelStyle.Format = "D";
                }

                BinChart.ApplyPaletteColors();

                legendList1.Clear();
                foreach (Series s in BinChart.Series)
                {
                    if (s.IsVisibleInLegend)
                        legendList1.Add(s.Name, s.Color);
                }
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void SetSeriesLabelVisible(System.Windows.Forms.DataVisualization.Charting.Series series, bool visible)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint pt in series.Points)
            {
                if (visible)
                    pt.Label = (string)pt.Tag;
                else
                    pt.Label = String.Empty;
            }
        }

        private void oYAxis_On_Apply(string Y1Max, string Y1Min, string Y1Interval, string Y2Max, string Y2Min, string Y2Interval, bool Y2Used)
        {
            double dVal = double.NaN;
            try
            {
                if (double.TryParse(Y1Max, out dVal) == true)
                    BinChart.ChartAreas["Default"].AxisY.Maximum = dVal;

                if (double.TryParse(Y1Min, out dVal) == true)
                    BinChart.ChartAreas["Default"].AxisY.Minimum = dVal;

                if (double.TryParse(Y1Interval, out dVal) == true)
                    BinChart.ChartAreas["Default"].AxisY.Interval = dVal;

                if (Y2Used)
                {
                    if (double.TryParse(Y2Max, out dVal) == true)
                        BinChart.ChartAreas["Default"].AxisY2.Maximum = dVal;

                    if (double.TryParse(Y2Min, out dVal) == true)
                        BinChart.ChartAreas["Default"].AxisY2.Minimum = dVal;

                    if (double.TryParse(Y2Interval, out dVal) == true)
                        BinChart.ChartAreas["Default"].AxisY2.Interval = dVal;
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion User Method


        #region Event Handler

        private void rbtLot_CheckedChanged(object sender, EventArgs e)
        {
            DrawWafer(this.TPWaferList);
        }

        private void rbtWafer_CheckedChanged(object sender, EventArgs e)
        {
            DrawWafer(this.TPWaferList);
        }

        private void legendList1_VisibleCheckedChanged(object sender, DataSelecter.ItemCheckedChangedEventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series s in BinChart.Series)
            {
                if (!s.IsVisibleInLegend && String.Equals(s.Name, "YIELD_POINT"))
                    s.Enabled = Array.IndexOf<string>(e.ItemNameArray, s.Name.Substring(0, s.Name.IndexOf('_'))) >= 0;
                else if (s.IsVisibleInLegend)
                    s.Enabled = Array.IndexOf<string>(e.ItemNameArray, s.Name) >= 0;
            }
        }

        private void legendList1_ShowLabelCheckedChanged(object sender, DataSelecter.ItemCheckedChangedEventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series s in BinChart.Series)
            {
                if (!_excludeSeries.Contains(s))
                    continue;

                bool visible = Array.IndexOf<string>(e.ItemNameArray, s.Name) >= 0;
                SetSeriesLabelVisible(s, visible);
            }
        }

        private void sizeResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinChart.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
            BinChart.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);
        }

        private void chartConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (oYAxis != null)
                    oYAxis.Dispose();

                oYAxis = null;
                oYAxis = new DACrux.TEST.Control.PopUpAxisY(
                    BinChart.ChartAreas["Default"].AxisY.Maximum.ToString(),
                    BinChart.ChartAreas["Default"].AxisY.Minimum.ToString(),
                    BinChart.ChartAreas["Default"].AxisY.Interval.ToString(),
                    BinChart.ChartAreas["Default"].AxisY2.Maximum.ToString(),
                    BinChart.ChartAreas["Default"].AxisY2.Minimum.ToString(),
                    BinChart.ChartAreas["Default"].AxisY2.Interval.ToString());
                oYAxis.TopMost = true;
                oYAxis.Owner = this;
                oYAxis.StartPosition = FormStartPosition.CenterParent;
                oYAxis.On_Apply += new DACrux.TEST.Control.PopUpAxisY.Apply(oYAxis_On_Apply);

                oYAxis.ShowDialog();
            }
            finally
            {
            }
        }

        private void BinChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = dp.ToolTip;
            }

        }

        private void chkRate_CheckedChanged(object sender, EventArgs e)
        {
            fnReportProcess();
        }

        #endregion Event Handler

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(BinChart);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Chart";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(fpSpread);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion



    }
}
