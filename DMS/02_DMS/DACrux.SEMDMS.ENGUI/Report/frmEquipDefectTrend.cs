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
    public partial class frmEquipDefectTrend : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]

        DataSet dsData = null;

        private DACrux.SEMDMS.Control.PopUpAxisY oYAxis = null;
        #endregion [ Data Field ]

        //-------------------------------------------------------------------------------------------------

        #region [ Constrator ]
        public frmEquipDefectTrend()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        //-------------------------------------------------------------------------------------------------


        private void frmEquipDefectTrend_Load(object sender, EventArgs e)
        {
            dtStart.Value = DateTime.Now.AddDays(-7);
            dtEnd.Value = DateTime.Now;

            ckClass.Checked = true;

            SetBinding(dlbPara, GetParaList(), 0, 0);

            dlbEquip_EnterTextBox(null, null);
        }


        private void GetTrendData()
        {
            string strMode = string.Empty;

            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();

            try
            {
                cmbItem.Items.Clear();
                cmbItem.Items.Add("ALL");

                if (dlbEquip.SelectedIndex < 0)
                {
                    MessageBox.Show("Equip 을 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dlbEquip.Focus();
                    return;
                }

                if (ckChamber.Checked == true && dlbPara.SelectedIndex < 0)
                {
                    MessageBox.Show("Chamber Info 를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dlbPara.Focus();
                    return;
                }

                if (dlbClass.SelectedIndex < 0)
                {
                    MessageBox.Show("Class 를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dlbClass.Focus();
                    return;
                }

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");

                //Mode 선택
                if (rbTimeMode.Checked)
                    strMode = "TRAN_TIME";
                else if (rbLotMode.Checked)
                    strMode = "LOT_ID";
                else if (rbWaferMode.Checked)
                    strMode = "WAFER_ID";
                else
                    return;

                if (dsData != null)
                    dsData.Dispose();
                dsData = null;

                dsData = oConfig.GetTrendReportReview(
                    ckClass.Checked,
                    rbDefects.Checked,
                    chkNormalize.Checked,
                    strMode,
                    ckChamber.Checked == true ? dlbPara.SelectedValue.ToString() : string.Empty, GetSelectedValues(dlbClass),
                    dlbEquip.SelectedValue.ToString(),
                    GetSelectedValues(dlbRoute),
                    GetSelectedValues(dlbLotID),
                    GetSelectedValues(dlbStep),
                    dtStart.Value.ToString("yyyyMMdd000000"),
                    dtEnd.Value.ToString("yyyyMMdd235959")
                    );

                MainForm.SetStatusMessage("Chart 를 그리는 중입니다.");

                DataTable dtItems = dsData.Tables["CHART"].DefaultView.ToTable(true, ckChamber.Checked ? dlbPara.SelectedValue.ToString() : "EQUIP").Select("1 = 1", ckChamber.Checked == true ? dlbPara.SelectedValue.ToString() : "EQUIP").CopyToDataTable<DataRow>();

                foreach (DataRow drItem in dtItems.Rows)
                {
                    cmbItem.Items.Add(drItem[0].ToString());
                }
                cmbItem.Text = "ALL";

                DrawChart(dsData.Tables["CHART"], dlbEquip.SelectedValue.ToString(), ckChamber.Checked ? dlbPara.SelectedValue.ToString() : "EQUIP");

                //Sheet 상에 Data Bind
                DACrux.Utility.FPSpreadUtil.InitSpread(fpsCommon);
                //fpsCommon.ActiveSheet.DataSource = dsData.Tables["RAW"];
                DACrux.Utility.FPSpreadUtil.SetSpreadData(dsData.Tables["RAW"], fpsCommon_Sheet1);
                fpsCommon.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpsCommon.ActiveSheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;

                FarPoint.Win.Spread.CellType.DateTimeCellType DateCellType = new FarPoint.Win.Spread.CellType.DateTimeCellType();
                DateCellType.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;

                FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCellType.DecimalPlaces = (int)nudDecimalPlaces.Value;
                numberCellType.MaximumValue = 99999999999999;
                numberCellType.MinimumValue = -99999999999999;

                int iCol = dsData.Tables["RAW"].Columns.IndexOf("TRAN_TIME");
                if (iCol > -1)
                    fpsCommon.ActiveSheet.Columns[iCol].CellType = DateCellType;
                iCol = dsData.Tables["RAW"].Columns.IndexOf("REVIEW_SUM");
                fpsCommon_Sheet1.Columns[iCol + 1, fpsCommon_Sheet1.ColumnCount - 1].CellType = numberCellType;

                fpsCommon.ActiveSheet.Models.Selection.ClearSelection();
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsCommon.ActiveSheet);
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

        private void DrawChart(DataTable dtChart, string strEquipment, string strParaName)
        {
            string strXValue = string.Empty;
            string strXName = string.Empty;
            string strSeries = string.Empty;

            double dMaxValue = 0;

            DataTable dtGroupX = null;
            DataTable dtGroupS = null;

            try
            {
                if (rbTimeMode.Checked)
                    strXName = "TRAN_TIME";
                else if (rbLotMode.Checked)
                    strXName = "LOT_ID";
                else if (rbWaferMode.Checked)
                    strXName = "WAFER_ID";
                else
                    return;

                //Chart Init
                chartTrend.Series.Clear();

                //Titles
                chartTrend.Titles["Default"].Text = string.Format("[{0}] Defect Trend", strEquipment);

                dtGroupX = dtChart.DefaultView.ToTable(true, strXName).Select("1 = 1", strXName).CopyToDataTable<DataRow>();
                dtGroupS = dtChart.DefaultView.ToTable(true, strParaName).Select("1 = 1", strParaName).CopyToDataTable<DataRow>();

                for (int s = 0; s < dtGroupS.Rows.Count; s++)
                {
                    Series series = null;
                    strSeries = dtGroupS.Rows[s][strParaName].ToString();

                    series = new Series(strSeries);
                    series.ChartType = SeriesChartType.Line;
                    series.SmartLabelStyle.Enabled = true;
                    series.MarkerSize = 5;
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
                    series.MarkerBorderColor = Color.Black;
                    series.BorderWidth = 3;
                    series.BorderDashStyle = ChartDashStyle.Solid;
                    series.YAxisType = AxisType.Primary;

                    series.SmartLabelStyle.Enabled = true;
                    series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                    series.SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                    series.SmartLabelStyle.CalloutLineColor = Color.Black;
                    series.SmartLabelStyle.CalloutLineWidth = 1;
                    series.SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;

                    series.IsValueShownAsLabel = true;
                    series["PieLabelStyle"] = "Outside";
                    series.BorderWidth = 1;
                    series.BorderColor = Color.Black;

                    chartTrend.Series.Add(series);
                }


                for (int ir = 0; ir < dtGroupX.Rows.Count; ir++)
                {
                    string strXGroupName = dtGroupX.Rows[ir][0].ToString();
                    DataPoint dp = null;
                    double y = double.NaN;

                    for (int s = 0; s < dtGroupS.Rows.Count; s++)
                    {
                        string strSeriesName = dtGroupS.Rows[s][0].ToString();
                        DataRow[] drFilter = dtChart.Select(string.Format("{0} = '{1}' AND {2} = '{3}'",
                                                           strXName, strXGroupName, strParaName, strSeriesName));

                        dp = new DataPoint(new Series(strSeriesName));

                        if (drFilter == null || drFilter.Length <= 0 || drFilter[0]["DATA"].ToString() == "0")
                        {
                            dp.IsEmpty = true;
                        }
                        else
                        {
                            if(!double.TryParse(drFilter[0]["DATA"].ToString(), out y))
                                y = double.NaN;

                            dp.ToolTip = String.Format("{0} / {1} ", strXGroupName, y);
                            dp.BorderWidth = 2;
                            dp.SetValueXY(strXGroupName, double.IsNaN(y) ? y : Math.Round(y, (int)nudDecimalPlaces.Value));

                            dMaxValue = Math.Max(dMaxValue, y);
                        }

                        chartTrend.Series[strSeriesName].Points.Add(dp);
                    }
                }

                //Legend Setup
                // Set legend style
                chartTrend.Legends["Default"].LegendStyle = LegendStyle.Table;
                // Set table style if legend style is Table
                chartTrend.Legends["Default"].TableStyle = LegendTableStyle.Auto;
                // Set legend docking
                chartTrend.Legends["Default"].Docking = Docking.Bottom;
                // Set legend alignment
                chartTrend.Legends["Default"].Alignment = StringAlignment.Center;
                // Disable legend
                chartTrend.Legends["Default"].Enabled = true;
                chartTrend.Legends["Default"].Font = new System.Drawing.Font("굴림", 12, FontStyle.Bold);


                //Zoom 관련 속성
                chartTrend.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                chartTrend.ChartAreas["Default"].CursorX.AutoScroll = true;
                chartTrend.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                chartTrend.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                chartTrend.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                chartTrend.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                chartTrend.ChartAreas["Default"].AxisY2.ScaleView.Zoomable = true;
                chartTrend.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();
                chartTrend.ChartAreas["Default"].AxisY.ScaleView.ZoomReset();
                chartTrend.ChartAreas["Default"].AxisY2.ScaleView.ZoomReset();

                chartTrend.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                chartTrend.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                chartTrend.ChartAreas["Default"].AxisX.IsStartedFromZero = false;
                chartTrend.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                //chartTrend.ChartAreas["Default"].AxisX.Interval = 1;
                //chartTrend.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                chartTrend.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                chartTrend.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90;


                chartTrend.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                //chartTrend.ChartAreas["Default"].AxisX.Minimum = 0;

                chartTrend.ChartAreas["Default"].AxisY.Maximum = dMaxValue + (int)(dMaxValue * 0.2);
                //chartTrend.ChartAreas["Default"].AxisY.Minimum = 0;
                chartTrend.ChartAreas["Default"].AxisY.Title = "[ea]";
                chartTrend.ChartAreas["Default"].AxisY.TitleFont = new Font("굴림", 12, FontStyle.Bold);
                //chartTrend.ChartAreas["Default"].AxisY.LabelStyle.Format = "{0.#}%";

                nudInterval.Value = 0;
                chartTrend.ApplyPaletteColors();

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

            sheet1.SheetName = "Data";
            sheet1.Add(fpsCommon);
            e.SheetList.Add(sheet1);

            sheet1 = new ExcelSheet();
            sheet1.SheetName = "Chart";
            sheet1.Add(chartTrend);
            sheet1.Add();
            e.SheetList.Add(sheet1);

            ExcelExportManager.Export(e);
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

        private DataTable GetParaList()
        {
            DataTable dtPara = new DataTable();
            dtPara.Columns.Add(new DataColumn("ITEM", typeof(string)));
            dtPara.AcceptChanges();

            DataRow dr = null;
            for (int ir = 0; ir < 15; ir++)
            {
                dr = dtPara.NewRow();
                dr[0] = string.Format("PARA_{0}", ir + 1);
                dtPara.Rows.Add(dr);
            }

            dtPara.AcceptChanges();

            return dtPara;
        }


        private void oYAxis_On_Apply(string Y1Max, string Y1Min, string Y1Interval)
        {
            double dVal = double.NaN;
            try
            {
                if (double.TryParse(Y1Max, out dVal) == true)
                    chartTrend.ChartAreas["Default"].AxisY.Maximum = dVal;

                if (double.TryParse(Y1Min, out dVal) == true)
                    chartTrend.ChartAreas["Default"].AxisY.Minimum = dVal;

                if (double.TryParse(Y1Interval, out dVal) == true)
                    chartTrend.ChartAreas["Default"].AxisY.Interval = dVal;

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

        private void dlbEquip_EnterTextBox(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");
            try
            {
                dlbEquip.ClearDataSource();
                dlbRoute.ClearDataSource();
                dlbLotID.ClearDataSource();
                dlbStep.ClearDataSource();

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");

                dtConfig = oConfig.GetItemList("EQUIP", dlbEquip.SearchText, null, null, null, null, dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbEquip, dtConfig, 0, 0);
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

        private void dlbRoute_EnterTextBox(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                dlbRoute.ClearDataSource();
                dlbLotID.ClearDataSource();
                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("LPT", dlbRoute.SearchText, GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbRoute, dtConfig, 1, 0);

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

        private void dlbLotID_EnterTextBox(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                dlbLotID.ClearDataSource();
                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("LOT_ID", dlbLotID.SearchText, GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbLotID, dtConfig, 0, 0);

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

        private void dlbStep_EnterTextBox(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("STEP_ID", dlbLotID.SearchText, GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbStep, dtConfig, 0, 0);

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

        private void dlbEquip_OnSelectedValueDoubleClick(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                if (dlbEquip.SelectedIndex < 0)
                    return;

                dlbRoute.ClearDataSource();
                dlbLotID.ClearDataSource();
                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("LPT", "", GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbRoute, dtConfig, 1, 0);

                //Step 정보는 언제나 넣는다.
                dtConfig = oConfig.GetItemList("STEP_ID", "", GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbStep, dtConfig, 0, 0);

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

        private void dlbRoute_OnSelectedValueDoubleClick(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                if (dlbRoute.SelectedIndex < 0)
                    return;

                dlbLotID.ClearDataSource();
                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("LOT_ID", "", GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbLotID, dtConfig, 0, 0);

                //Step 정보는 언제나 넣는다.
                dtConfig = oConfig.GetItemList("STEP_ID", "", GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbStep, dtConfig, 0, 0);


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

        private void dlbLotID_OnSelectedValueDoubleClick(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfig = new SEMConfiguration();
            DataTable dtConfig = null;

            MainForm.SetStatusMessage("Data 를 조회 중입니다.");

            try
            {
                if (dlbLotID.SelectedIndex < 0)
                    return;

                dlbStep.ClearDataSource();

                dtConfig = oConfig.GetItemList("STEP_ID", "", GetSelectedValues(dlbEquip), GetSelectedValues(dlbRoute), GetSelectedValues(dlbLotID), GetSelectedValues(dlbStep), dtStart.Value.ToString("yyyyMMdd000000"), dtEnd.Value.ToString("yyyyMMdd235959"));
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                    SetBinding(dlbStep, dtConfig, 0, 0);

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

        private void ckClass_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RO.SEMConfiguration oSemConfig = new SEMConfiguration();

                dlbClass.ClearDataSource();
                DataTable dtClass = oSemConfig.GetClassList(ckClass.Checked);
                grbClass.Enabled = ckClass.Checked;

                if (dtClass == null || dtClass.Rows.Count == 0)
                {
                    MessageBox.Show("Not Found Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dlbClass.ClearDataSource();
                SetBinding(dlbClass, dtClass, 1, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ckChamber_CheckedChanged(object sender, EventArgs e)
        {
            dlbPara.Enabled = ckChamber.Checked;
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartTrend.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
            chartTrend.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);
        }

        private void nudInterval_ValueChanged(object sender, EventArgs e)
        {
            chartTrend.ChartAreas["Default"].AxisX.Interval = (double)(sender as NumericUpDown).Value;
        }

        private void chartConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (oYAxis != null)
                    oYAxis.Dispose();

                oYAxis = null;
                oYAxis = new DACrux.SEMDMS.Control.PopUpAxisY(
                    chartTrend.ChartAreas["Default"].AxisY.Maximum.ToString(),
                    chartTrend.ChartAreas["Default"].AxisY.Minimum.ToString(),
                    chartTrend.ChartAreas["Default"].AxisY.Interval.ToString());
                oYAxis.TopMost = true;
                oYAxis.Owner = this;
                oYAxis.StartPosition = FormStartPosition.CenterParent;
                oYAxis.On_Apply += new DACrux.SEMDMS.Control.PopUpAxisY.Apply(oYAxis_On_Apply);

                oYAxis.ShowDialog();
            }
            finally
            {
            }
        }

        private void chartTrend_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = chartTrend.HitTest(e.X, e.Y);

            try
            {
                for (int i = 0; i < chartTrend.Series.Count; i++)
                {

                    foreach (DataPoint point1 in chartTrend.Series[i].Points)
                    {
                        point1.MarkerSize = 5;
                    }

                    if (result.ChartElementType == ChartElementType.DataPoint)
                    {
                        DataPoint tooltipPoint = chartTrend.Series[result.Series.Name.ToString()].Points[result.PointIndex];
                        chartTrend.Series[result.Series.Name.ToString()].Points[result.PointIndex].MarkerSize = 20;
                    }
                }
            }
            finally
            { }
        }

        private void chartTrend_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            try
            {
                if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
                {
                    e.Text = chartTrend.Series[e.HitTestResult.Series.Name.ToString()].Points[e.HitTestResult.PointIndex].ToolTip;
                }
            }
            finally
            { }

        }

        private void chkNormalized_CheckedChanged(object sender, EventArgs e)
        {
            btnView.PerformClick();
        }

        private void cmbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItem.Text == "ALL")
            {
                foreach (Series sr in chartTrend.Series)
                {
                    sr.Enabled = true;
                }
            }
            else
            {
                foreach (Series sr in chartTrend.Series)
                {
                    if (sr.Name == cmbItem.Text)
                        sr.Enabled = true;
                    else
                        sr.Enabled = false;
                }
            }
        }

        private void nudDecimalPlaces_ValueChanged(object sender, EventArgs e)
        {
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            numberCellType.DecimalPlaces = (int)nudDecimalPlaces.Value;
            numberCellType.MaximumValue = 99999999999999;
            numberCellType.MinimumValue = -99999999999999;

            int iCol = dsData.Tables["RAW"].Columns.IndexOf("REVIEW_SUM");
            fpsCommon_Sheet1.Columns[iCol + 1, fpsCommon_Sheet1.ColumnCount - 1].CellType = numberCellType;
            fpsCommon.Refresh();

            DrawChart(dsData.Tables["CHART"], dlbEquip.SelectedValue.ToString(), ckChamber.Checked ? dlbPara.SelectedValue.ToString() : "EQUIP");
        }

        #endregion [ Event Handler ]
    }
}
