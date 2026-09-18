using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using DACrux.Framework.Base;
using DACrux.Utility;
using DACrux.Framework.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmLossYieldProspect : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]

        DataSet dsData = null;
        DataTable dtDeviceList = null;
        DataTable dtFilterData = null;
        DataTable dtMainGroup = null;

        DACrux.SEMDMS.Control.PopUpAxisYConfig oYAxis;

        List<Series> _excludeSeries = new List<Series>();
        List<String> StepInfos = null;

        enum TQC_MAP_MATCH { LOT_ID, WAFER_ID, TESTAREA, DEVICE_ALIAS, PRODUCT, PROGRAM, CUSTOMER, TECH, PROSPECT_YIELD, PROSPECT_INCLUDE_AVG_YIELD, ACTUAL_YIELD };
        #endregion [ Data Field ]

        //-------------------------------------------------------------------------------------------------

        #region [ Constrator ]
        public frmLossYieldProspect()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        //-------------------------------------------------------------------------------------------------


        private void frmLossYieldProspect_Load(object sender, EventArgs e)
        {
            dtStart.Value = DateTime.Now.AddDays(-7);
            dtEnd.Value = DateTime.Now;

            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
            DataTable dtDeviceList = oDMapAnalysis.GetShotDeviceList();

            //Device 정보는 Map Def 에서 가져온다.
            SetBinding(dlbDevice, dtDeviceList, 0, 0);

            //Test Area 정보 Fix
            SetBinding(dlbTestArea, GetTestArea(), 0, 0);

            //Wafer No 정보 Fix
            //SetBinding(dlbWaferNo, GetWaferNo(), 0, 0);

            //GetDeviceTestArea();

            //bLoad = true;
        }

        private void GetTrendData()
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();

            DataTable dtStepList = null;
            int iStepCount = 0;
            try
            {
                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                StepInfos = new List<String>();

                if (dlbDevice.SelectedIndex < 0)
                {
                    MessageBox.Show("Device 를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dlbDevice.Focus();
                    return;
                }

                if (dlbTestArea.SelectedIndex < 0)
                {
                    MessageBox.Show("Test Area 를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dlbTestArea.Focus();
                    return;
                }

                dsData = oConfig.GetProspectYieldReport(dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"), dlbTestArea.SelectedValue.ToString(), dlbDevice.SelectedValue.ToString(),
                                                       GetSelectedValues(dlbProduct), GetSelectedValues(dlbLotID), GetSelectedValues(dlbWaferNo), GetSelectedValues(dlbStep));

                if(dsData == null)
                    throw new Exception("Not Found Data");

                if (dtMainGroup != null)
                    dtMainGroup.Dispose();
                dtMainGroup = null;

                MainForm.SetStatusMessage("Data 화면에 출력 중입니다.");
                dtMainGroup = dsData.Tables["MAIN"].DefaultView.ToTable(true,
                                                                    TQC_MAP_MATCH.LOT_ID.ToString(),
                                                                    TQC_MAP_MATCH.WAFER_ID.ToString(),
                                                                    TQC_MAP_MATCH.TESTAREA.ToString(),
                                                                    TQC_MAP_MATCH.DEVICE_ALIAS.ToString(),
                                                                    TQC_MAP_MATCH.PRODUCT.ToString(),
                                                                    TQC_MAP_MATCH.PROGRAM.ToString(),
                                                                    TQC_MAP_MATCH.CUSTOMER.ToString(),
                                                                    TQC_MAP_MATCH.TECH.ToString(),
                                                                    TQC_MAP_MATCH.PROSPECT_YIELD.ToString(),
                                                                    TQC_MAP_MATCH.PROSPECT_INCLUDE_AVG_YIELD.ToString(),
                                                                    TQC_MAP_MATCH.ACTUAL_YIELD.ToString()).Select("1 = 1", "LOT_ID, WAFER_ID, TESTAREA, DEVICE_ALIAS, PRODUCT,PROGRAM").CopyToDataTable<DataRow>();

                dtStepList = dsData.Tables["MAIN"].DefaultView.ToTable(true, "STEP_ID").Select("1 = 1", "STEP_ID").CopyToDataTable<DataRow>();

                FarPoint.Win.Spread.CellType.NumberCellType oNumIntCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.NumberCellType oNumDoubleCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.TextCellType oTextCell = new FarPoint.Win.Spread.CellType.TextCellType();
                oNumIntCell.DecimalPlaces = 0;
                oNumDoubleCell.DecimalPlaces = 2;

                fpsCommon.ActiveSheet.DataSource = null;
                fpsCommon.ActiveSheet.Rows.Clear();
                fpsCommon.ActiveSheet.Columns.Clear();
                fpsCommon.ActiveSheet.ColumnHeader.Columns.Clear();
                fpsCommon.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;

                iStepCount = dtMainGroup.Columns.Count;
                //Step 01 :  Group Column 생성
                for (int iStep = 0; iStep < dtStepList.Rows.Count; iStep++)
                {
                    dtMainGroup.Columns.Add(new DataColumn(dtStepList.Rows[iStep]["STEP_ID"].ToString(), typeof(double)));
                    StepInfos.Add(dtStepList.Rows[iStep]["STEP_ID"].ToString());
                }

                dtMainGroup.AcceptChanges();

                fpsCommon.ActiveSheet.DataSource = dtMainGroup;
                Utility.FPSpreadUtil.SpreadSortingAll(fpsCommon.ActiveSheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsCommon.ActiveSheet);

                //=========================================================================================================================================
                // Data 넣기
                //=========================================================================================================================================

                for (int ir = 0; ir < fpsCommon.ActiveSheet.RowCount; ir++)
                {
                    MainForm.SetStatusMessage(string.Format("Data 화면에 출력 중입니다. ({0}/{1})", ir, fpsCommon.ActiveSheet.RowCount));

                    double dPrpospect = 0;
                    double dPrpospectAll = 0;
                    //값 집어 넣기
                    for (int ic = iStepCount; ic < fpsCommon.ActiveSheet.ColumnCount; ic++)
                    {
                        fpsCommon.ActiveSheet.Cells[ir, ic].Value = 0;
                        fpsCommon.ActiveSheet.Cells[ir, ic].BackColor = Color.Empty;
                        fpsCommon.ActiveSheet.Cells[ir, ic].Tag = "EMPTY";

                        //예상 Yield 를 미리 넣어 놓는다.
                        if (dsData.Tables.IndexOf("SUB") > -1 && dsData.Tables["SUB"].Rows.Count > 0)
                        {
                            DataRow[] drPredictYield = dsData.Tables["SUB"].Select(string.Format("STEP_ID = '{0}' AND LOSS_YIELD > 0", fpsCommon.ActiveSheet.Columns[ic].Label));
                            foreach (DataRow dr in drPredictYield)
                            {
                                fpsCommon.ActiveSheet.Cells[ir, ic].Value = dr["LOSS_YIELD"].ToString();
                                fpsCommon.ActiveSheet.Cells[ir, ic].BackColor = Color.LightGray;
                                fpsCommon.ActiveSheet.Cells[ir, ic].Tag = "SUB";
                            }
                        }

                        DataRow[] drYield = dsData.Tables["MAIN"].Select(string.Format("WAFER_ID = '{0}' AND PRODUCT = '{1}' AND  PROGRAM = '{2}' AND CUSTOMER = '{3}' AND STEP_ID = '{4}'",
                               fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.WAFER_ID].Value,
                               fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.PRODUCT].Value,
                               fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.PROGRAM].Value,
                               fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.CUSTOMER].Value,
                               fpsCommon.ActiveSheet.Columns[ic].Label));

                        foreach (DataRow dr in drYield)
                        {
                            fpsCommon.ActiveSheet.Cells[ir, ic].Value = dr["LOSS_YIELD"].ToString();
                            fpsCommon.ActiveSheet.Cells[ir, ic].BackColor = Color.Empty;
                            fpsCommon.ActiveSheet.Cells[ir, ic].Tag = "MAIN";
                        }
                    }

                    //재계산
                    for (int ic = iStepCount; ic < fpsCommon.ActiveSheet.ColumnCount; ic++)
                    {
                        if (fpsCommon.ActiveSheet.Cells[ir, ic].Tag.ToString() == "SUB")
                        {
                            dPrpospectAll += double.Parse(fpsCommon.ActiveSheet.Cells[ir, ic].Value.ToString());
                        }
                        else
                        {
                            dPrpospect += double.Parse(fpsCommon.ActiveSheet.Cells[ir, ic].Value.ToString());
                            dPrpospectAll += double.Parse(fpsCommon.ActiveSheet.Cells[ir, ic].Value.ToString());
                        }
                    }

                    fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.PROSPECT_YIELD].Value = (double)(100 - dPrpospect);
                    fpsCommon.ActiveSheet.Cells[ir, (int)TQC_MAP_MATCH.PROSPECT_INCLUDE_AVG_YIELD].Value = (double)(100 - dPrpospectAll);
                }

                DrawChart(fpsCommon.ActiveSheet.DataSource as DataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

     

        #region [ Method ]

        public void ExportExcel()
        {
            ExcelSheet sheet1 = new ExcelSheet();
            ExcelExportArgs e = new ExcelExportArgs();

            //sheet1.SheetName = "Data";
            //sheet1.Add(fpsCommon);
            //e.SheetList.Add(sheet1);

            //sheet1 = new ExcelSheet();
            //sheet1.SheetName = "Chart";
            //sheet1.Add(chartTrend);
            //sheet1.Add();
            //sheet1.Add(chartBox);
            //e.SheetList.Add(sheet1);
            
            //ExcelExportManager.Export(e);
        }

        private void SetBinding(
            DUCListBox listbox,
            DataTable source,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (source != null)
            {
                listbox.DisplayMember = source.Columns[dispIndex].ColumnName;
                listbox.ValueMember = source.Columns[valueIndex].ColumnName;
            }

            listbox.DataSource = source;
        }

        private string[] GetSelectedValues(
            DUCListBox listBox
            )
        {
            DataTable dt = listBox.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return null;

            object[] arr = listBox.SelectedValues;

            if (arr == null || arr.Length == 0)
                return null;

            string[] results = new string[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                results[i] = arr[i].ToString();

            return results;
        }

        private DataTable GetTestArea()
        {
            DataTable dtPara = new DataTable();
            dtPara.Columns.Add(new DataColumn("ITEM", typeof(string)));
            dtPara.AcceptChanges();

            DataRow dr = null;
            dr = dtPara.NewRow();
            dr[0] ="MULTIPROBE";
            dtPara.Rows.Add(dr);

            dr = dtPara.NewRow();
            dr[0] ="POSTLASER";
            dtPara.Rows.Add(dr);

            dr = dtPara.NewRow();
            dr[0] = "PRELASER";
            dtPara.Rows.Add(dr);

            dtPara.AcceptChanges();

            return dtPara;
        }

        private DataTable GetWaferNo()
        {
            DataTable dtPara = new DataTable();
            dtPara.Columns.Add(new DataColumn("ITEM", typeof(string)));
            dtPara.AcceptChanges();

            DataRow dr = null;
            dr = dtPara.NewRow();
            dr[0] = "-ALL-";
            dtPara.Rows.Add(dr);

            for (int ir = 1; ir <= 25; ir++)
            {
                dr = dtPara.NewRow();
                dr[0] = string.Format("{0:00}", ir);
                dtPara.Rows.Add(dr);
            }

            dtPara.AcceptChanges();

            return dtPara;
        }

        private void GetFilterData()
        {
            DACrux.SEMDMS.RO.SEMConfiguration oData = new SEMConfiguration();
            try
            {
                MainForm.SetStatusMessage("조회 기준정보를 가져오는 중입니다.");

                if (dtFilterData != null)
                    dtFilterData.Dispose();

                dtFilterData = null;

                dlbProduct.ClearDataSource();
                dlbLotID.ClearDataSource();
                dlbWaferNo.ClearDataSource();
                dlbStep.ClearDataSource();

                if (dlbTestArea.SelectedIndex < 0)
                    return;

                if (dlbDevice.SelectedIndex < 0)
                    return;

                dtFilterData = oData.GetLossProspectFilter(dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"), dlbTestArea.SelectedValue.ToString(), dlbDevice.SelectedValue.ToString());
                if (dtFilterData != null && dtFilterData.Rows.Count > 0)
                {
                    SetBinding(dlbProduct, dtFilterData.DefaultView.ToTable(true, "PRODUCT").Select("1 = 1", "PRODUCT").CopyToDataTable<DataRow>(), 0, 0);
                    SetBinding(dlbLotID, dtFilterData.DefaultView.ToTable(true, "LOT_ID").Select("1 = 1", "LOT_ID").CopyToDataTable<DataRow>(), 0, 0);
                    SetBinding(dlbWaferNo, dtFilterData.DefaultView.ToTable(true, "WAFER_ID").Select("1 = 1", "WAFER_ID").CopyToDataTable<DataRow>(), 0, 0);
                    SetBinding(dlbStep, dtFilterData.DefaultView.ToTable(true, "STEP_ID").Select("1 = 1", "STEP_ID").CopyToDataTable<DataRow>(), 0, 0);
                }

            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void GetDeviceTestArea()
        {
            DACrux.SEMDMS.RO.SEMConfiguration oData = new SEMConfiguration();
            try
            {
                MainForm.SetStatusMessage("조회 기준정보를 가져오는 중입니다.");

                if (dtDeviceList != null)
                    dtDeviceList.Dispose();

                dlbDevice.ClearDataSource();
                dlbTestArea.ClearDataSource();
                dlbProduct.ClearDataSource();
                dlbLotID.ClearDataSource();
                dlbWaferNo.ClearDataSource();
                dlbStep.ClearDataSource();

                dtDeviceList = oData.GetLossProspectFilterDevice(dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtDeviceList != null && dtDeviceList.Rows.Count > 0)
                {

                    //Device 정보는 Map Def 에서 가져온다.
                    SetBinding(dlbDevice, dtDeviceList.DefaultView.ToTable(true, "DEVICE_ALIAS").Select("1 = 1", "DEVICE_ALIAS").CopyToDataTable<DataRow>(), 0, 0);

                    //Test Area 정보 Fix
                    SetBinding(dlbTestArea, dtDeviceList.DefaultView.ToTable(true, "TESTAREA").Select("1 = 1", "TESTAREA").CopyToDataTable<DataRow>(), 0, 0);
                }
                else
                    throw new Exception("Not Found Data");

            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }

        }


        private void DrawChart(DataTable dtChart)
        {
            string strXValue = string.Empty;
            string strSeries = string.Empty;
            double dMaxYieldLoss = double.MinValue;

            Series m_series = null;

            try
            {
                YieldChart.Series.Clear();
                YieldChart.Titles.Clear();
                _excludeSeries.Clear();

                m_series = new Series(TQC_MAP_MATCH.PROSPECT_YIELD.ToString());
                m_series.ChartType = SeriesChartType.Line;
                m_series.Color = Color.Blue;
                m_series.YAxisType = AxisType.Secondary;

                YieldChart.Series.Add(m_series);
                _excludeSeries.Add(m_series);

                //--

                m_series = new Series(TQC_MAP_MATCH.PROSPECT_INCLUDE_AVG_YIELD.ToString());
                m_series.ChartType = SeriesChartType.Line;
                m_series.Color = Color.Pink;
                m_series.YAxisType = AxisType.Secondary;

                YieldChart.Series.Add(m_series);
                _excludeSeries.Add(m_series);

                //--

                m_series = new Series(TQC_MAP_MATCH.ACTUAL_YIELD.ToString());
                m_series.ChartType = SeriesChartType.Line;
                m_series.Color = Color.Red;
                m_series.YAxisType = AxisType.Secondary;

                YieldChart.Series.Add(m_series);
                _excludeSeries.Add(m_series);

                //--

                foreach (string Step in StepInfos)
                {
                    m_series = new Series(Step);
                    m_series.ChartType = SeriesChartType.StackedColumn;
                    m_series.SmartLabelStyle.Enabled = true;
                    m_series.MarkerSize = 2;
                    m_series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    m_series.BorderWidth = 2;
                    m_series.YAxisType = AxisType.Primary;

                    YieldChart.Series.Add(m_series);
                    _excludeSeries.Add(m_series);
                }

                //chart Point 입력
                for (int ir = 0; ir < dtChart.Rows.Count; ir++)
                {
                    strXValue = dtChart.Rows[ir]["WAFER_ID"].ToString();
                    DataPoint dp = null;
                    object y = null;

                    //--

                    #region [ Yield 에 대한 Series ]

                    strSeries = TQC_MAP_MATCH.PROSPECT_YIELD.ToString();
                    y = dtChart.Rows[ir][strSeries];
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

                    YieldChart.Series[strSeries].Points.Add(dp);

                    //--

                    strSeries = TQC_MAP_MATCH.PROSPECT_INCLUDE_AVG_YIELD.ToString();
                    y = dtChart.Rows[ir][strSeries];
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

                    YieldChart.Series[strSeries].Points.Add(dp);

                    //--

                    strSeries = TQC_MAP_MATCH.ACTUAL_YIELD.ToString();
                    y = dtChart.Rows[ir][strSeries];
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

                    YieldChart.Series[strSeries].Points.Add(dp);


                    #endregion [ Yield 에 대한 Series ]


                    #region [ Step 별 Data ]

                    double dTemp = 0;
                    foreach (string Step in StepInfos)
                    {
                        strSeries = Step;
                        y = dtChart.Rows[ir][strSeries];
                        dp = new DataPoint(new Series(strSeries));

                        if (!String.IsNullOrEmpty(y.ToString()))
                        {
                            dp.Tag = String.Format("{0}", y);
                            dp.ToolTip = String.Format("{0} / {1} / {2}", strXValue, strSeries, y);
                            dp.BorderWidth = 2;
                            dp.SetValueXY(strXValue, y);

                            dTemp += double.Parse(y.ToString());
                        }
                        else
                        {
                            dp.IsEmpty = true;
                        }

                        YieldChart.Series[strSeries].Points.Add(dp);
                    }

                    dMaxYieldLoss = Math.Max(dTemp, dMaxYieldLoss);
                    #endregion [  Step 별 Data ]

                }

                //Zoom 관련 속성
                YieldChart.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                YieldChart.ChartAreas["Default"].CursorX.AutoScroll = true;
                YieldChart.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                YieldChart.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                YieldChart.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                YieldChart.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                YieldChart.ChartAreas["Default"].AxisY2.ScaleView.Zoomable = true;
                YieldChart.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();
                YieldChart.ChartAreas["Default"].AxisY.ScaleView.ZoomReset();
                YieldChart.ChartAreas["Default"].AxisY2.ScaleView.ZoomReset();

                YieldChart.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                YieldChart.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                YieldChart.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                YieldChart.ChartAreas["Default"].AxisX.Interval = 1;
                YieldChart.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                YieldChart.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                YieldChart.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90;


                YieldChart.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                YieldChart.ChartAreas["Default"].AxisX.Minimum = 0;

                YieldChart.ChartAreas["Default"].AxisY.Maximum = Math.Max(dMaxYieldLoss + (dMaxYieldLoss * 0.2), 20);
                YieldChart.ChartAreas["Default"].AxisY.Minimum = 0;
                YieldChart.ChartAreas["Default"].AxisY.Title = "Yield Loss";
                YieldChart.ChartAreas["Default"].AxisY.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                YieldChart.ChartAreas["Default"].AxisY.LabelStyle.Format = "{0.#}%";

                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.Spacing = 2;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineWidth = 2;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineColor = Color.Red;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
                YieldChart.ChartAreas["Default"].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;


                YieldChart.ChartAreas["Default"].AxisY2.Minimum = 0;
                YieldChart.ChartAreas["Default"].AxisY2.Maximum = 100;
                YieldChart.ChartAreas["Default"].AxisY2.Title = "Yield";
                YieldChart.ChartAreas["Default"].AxisY2.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                YieldChart.ChartAreas["Default"].AxisY2.LabelStyle.Format = "{0.#}%";
                
                YieldChart.ApplyPaletteColors();

                legendList1.Clear();
                foreach (Series s in YieldChart.Series)
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
                    YieldChart.ChartAreas["Default"].AxisY.Maximum = dVal;

                if (double.TryParse(Y1Min, out dVal) == true)
                    YieldChart.ChartAreas["Default"].AxisY.Minimum = dVal;

                if (double.TryParse(Y1Interval, out dVal) == true)
                    YieldChart.ChartAreas["Default"].AxisY.Interval = dVal;

                if (Y2Used)
                {
                    if (double.TryParse(Y2Max, out dVal) == true)
                        YieldChart.ChartAreas["Default"].AxisY2.Maximum = dVal;

                    if (double.TryParse(Y2Min, out dVal) == true)
                        YieldChart.ChartAreas["Default"].AxisY2.Minimum = dVal;

                    if (double.TryParse(Y2Interval, out dVal) == true)
                        YieldChart.ChartAreas["Default"].AxisY2.Interval = dVal;
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion [ Method ]

//===============================================================================================================================================================================
//===============================================================================================================================================================================
    
        #region [ Event Handler ]


        private void btnView_Click(object sender, EventArgs e)
        {
            GetTrendData();
        }


        private void chkScaleBreak_CheckedChanged(object sender, EventArgs e)
        {
            DrawChart(fpsCommon.ActiveSheet.DataSource as DataTable);
        }


        private void dlbDevice_OnSelectedValueChanged(object sender, EventArgs e)
        {
            GetFilterData();
        }

        private void dlbTestArea_OnSelectedValueChanged(object sender, EventArgs e)
        {
            GetFilterData();
        }



        private void dlbProduct_OnSelectedValueChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;
            try
            {
                MainForm.SetStatusMessage("조회 기준정보를 가져오는 중입니다.");

                dlbLotID.ClearDataSource();
                dlbWaferNo.ClearDataSource();
                dlbStep.ClearDataSource();

                if (dtFilterData == null || dtFilterData.Rows.Count <= 0)
                    return;

                if (dlbProduct.SelectedIndex < 0)
                    return;

                if (dtFilterData.Select(string.Format("PRODUCT IN ('{0}')", string.Join("','", GetSelectedValues(dlbProduct)))).Length <= 0)
                    return;

                dtFilter = dtFilterData.Select(string.Format("PRODUCT IN ('{0}')", string.Join("','", GetSelectedValues(dlbProduct)))).CopyToDataTable<DataRow>();

                SetBinding(dlbLotID, dtFilter.DefaultView.ToTable(true, "LOT_ID").Select("1 = 1", "LOT_ID").CopyToDataTable<DataRow>(), 0, 0);
                SetBinding(dlbWaferNo, dtFilter.DefaultView.ToTable(true, "WAFER_ID").Select("1 = 1", "WAFER_ID").CopyToDataTable<DataRow>(), 0, 0);
                SetBinding(dlbStep, dtFilter.DefaultView.ToTable(true, "STEP_ID").Select("1 = 1", "STEP_ID").CopyToDataTable<DataRow>(), 0, 0);

            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void dlbLotID_OnSelectedValueChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;
            try
            {
                MainForm.SetStatusMessage("조회 기준정보를 가져오는 중입니다.");

                dlbWaferNo.ClearDataSource();
                dlbStep.ClearDataSource();

                if (dlbProduct.SelectedIndex < 0 || dlbLotID.SelectedIndex < 0)
                    return;

                if (dtFilterData == null || dtFilterData.Rows.Count <= 0)
                    return;

                if (dtFilterData.Select(string.Format("PRODUCT IN ('{0}') AND LOT_ID IN ('{1}')", string.Join("','", GetSelectedValues(dlbProduct)), string.Join("','", GetSelectedValues(dlbLotID)))).Length <= 0)
                    return;

                dtFilter = dtFilterData.Select(string.Format("PRODUCT IN ('{0}') AND LOT_ID IN ('{1}')", string.Join("','", GetSelectedValues(dlbProduct)), string.Join("','", GetSelectedValues(dlbLotID)))).CopyToDataTable<DataRow>();

                SetBinding(dlbWaferNo, dtFilter.DefaultView.ToTable(true, "WAFER_ID").Select("1 = 1", "WAFER_ID").CopyToDataTable<DataRow>(), 0, 0);
                SetBinding(dlbStep, dtFilter.DefaultView.ToTable(true, "STEP_ID").Select("1 = 1", "STEP_ID").CopyToDataTable<DataRow>(), 0, 0);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }


        private void dlbWaferNo_OnSelectedValueChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;
            try
            {
                MainForm.SetStatusMessage("조회 기준정보를 가져오는 중입니다.");

                dlbStep.ClearDataSource();

                if (dlbProduct.SelectedIndex < 0 || dlbLotID.SelectedIndex < 0 || dlbWaferNo.SelectedIndex < 0)
                    return;

                if (dtFilterData == null || dtFilterData.Rows.Count <= 0)
                    return;

                if (dtFilterData.Select(string.Format("PRODUCT IN ('{0}') AND LOT_ID IN ('{1}') AND WAFER_ID IN ('{2}')", string.Join("','", GetSelectedValues(dlbProduct)), string.Join("','", GetSelectedValues(dlbLotID)), string.Join("','", GetSelectedValues(dlbWaferNo)))).Length <= 0)
                    return;

                dtFilter = dtFilterData.Select(string.Format("PRODUCT IN ('{0}') AND LOT_ID IN ('{1}') AND WAFER_ID IN ('{2}')", string.Join("','", GetSelectedValues(dlbProduct)), string.Join("','", GetSelectedValues(dlbLotID)), string.Join("','", GetSelectedValues(dlbWaferNo)))).CopyToDataTable<DataRow>();

                SetBinding(dlbStep, dtFilter.DefaultView.ToTable(true, "STEP_ID").Select("1 = 1", "STEP_ID").CopyToDataTable<DataRow>(), 0, 0);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            //if(bLoad)
            //    GetDeviceTestArea();
        }

        private void legendList1_ShowLabelCheckedChanged(object sender, DataSelecter.ItemCheckedChangedEventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series s in YieldChart.Series)
            {
                if (!_excludeSeries.Contains(s))
                    continue;

                bool visible = Array.IndexOf<string>(e.ItemNameArray, s.Name) >= 0;
                SetSeriesLabelVisible(s, visible);
            }
        }

        private void legendList1_VisibleCheckedChanged(object sender, DataSelecter.ItemCheckedChangedEventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series s in YieldChart.Series)
            {
                if (!s.IsVisibleInLegend && String.Equals(s.Name, "YIELD_POINT"))
                    s.Enabled = Array.IndexOf<string>(e.ItemNameArray, s.Name.Substring(0, s.Name.IndexOf('_'))) >= 0;
                else if (s.IsVisibleInLegend)
                    s.Enabled = Array.IndexOf<string>(e.ItemNameArray, s.Name) >= 0;
            }
        }


        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YieldChart.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
            YieldChart.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);
        }

        private void chartConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (oYAxis != null)
                    oYAxis.Dispose();

                oYAxis = null;
                oYAxis = new DACrux.SEMDMS.Control.PopUpAxisYConfig(
                    YieldChart.ChartAreas["Default"].AxisY.Maximum.ToString(),
                    YieldChart.ChartAreas["Default"].AxisY.Minimum.ToString(),
                    YieldChart.ChartAreas["Default"].AxisY.Interval.ToString(),
                    YieldChart.ChartAreas["Default"].AxisY2.Maximum.ToString(),
                    YieldChart.ChartAreas["Default"].AxisY2.Minimum.ToString(),
                    YieldChart.ChartAreas["Default"].AxisY2.Interval.ToString());
                oYAxis.TopMost = true;
                oYAxis.Owner = this;
                oYAxis.StartPosition = FormStartPosition.CenterParent;
                oYAxis.On_Apply += new DACrux.SEMDMS.Control.PopUpAxisYConfig.Apply(oYAxis_On_Apply);

                oYAxis.ShowDialog();
            }
            finally
            {
            }
        }
       
        #endregion [ Event Handler ]

    }
}
