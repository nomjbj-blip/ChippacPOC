using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Utility;
using System.Collections;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.TEST.ENGUI
{
    public partial class frmCumMapReport_FOI : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        enum CUM_CNT { FROM, TO, COLOR }

        private string m_strPrduct = string.Empty;
        private string m_strProgram = string.Empty;
        private long[] m_strWaferSeqs = null;
        private DataSet m_ds = null;
        private string strConfigFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM", "CumCount");

        private string[] strHeaderBase = new string[] { "DEVICE_ALIAS", "PRODUCT", "TESTAREA", "PROGRAM", "LOT_ID", "WAFER_ID", "START_TIME", "END_TIME", "YIELD", "TESTED_DIE", "LOT_SEQ", "WAFER_SEQ" };

        public frmCumMapReport_FOI()
        {
            InitializeComponent();
        }

        private void frmCumMapReport_Load(object sender, EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

                //Cum Count Setup 관련 정보 가져오기 (Local file)
                FileInfo oConfigFile = new FileInfo(strConfigFullPath);
                if (oConfigFile.Exists == true)
                {
                    DataSet dsFile = new DataSet();
                    dsFile.ReadXml(oConfigFile.FullName);
                    if (dsFile.Tables.Count > 0)
                        SetCumcountColor(dsFile.Tables[0]);
                }
                else
                {
                    SetCumcountColor(GetInitDefectConfig());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #region User Method

        private void SetCumcountColor(DataTable dtConfig)
        {
            dgCumCount.DataSource = dtConfig;

            dgCumCount.Columns[0].HeaderText = CUM_CNT.FROM.ToString();
            dgCumCount.Columns[1].HeaderText = CUM_CNT.TO.ToString();
            dgCumCount.Columns[2].HeaderText = CUM_CNT.COLOR.ToString();

            dgCumCount.Columns[0].Width = 60;
            dgCumCount.Columns[1].Width = 60;
            dgCumCount.Columns[2].Width = 80;

            dgCumCount.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgCumCount.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgCumCount.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgCumCount.Columns[0].ReadOnly = false;
            dgCumCount.Columns[1].ReadOnly = false;
            dgCumCount.Columns[2].ReadOnly = true;

            foreach (DataGridViewRow dRow in dgCumCount.Rows)
            {
                if (dRow.IsNewRow == true)
                    continue;

                Color DefectCNTColor = ColorTranslator.FromHtml(dRow.Cells[(int)CUM_CNT.COLOR].Value.ToString());
                dRow.DefaultCellStyle.BackColor = DefectCNTColor;
            }

            dgCumCount.Refresh();

            Application.DoEvents();
        }

        private DataTable GetInitDefectConfig()
        {
            DataTable dtCumConfig = new DataTable("CUM_CNT");
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.FROM.ToString(), typeof(int)));
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.TO.ToString(), typeof(int)));
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.COLOR.ToString(), typeof(string)));

            DataRow drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 1;
            drItem[(int)CUM_CNT.TO] = 2;
            drItem[(int)CUM_CNT.COLOR] = "#FFCAE5";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 3;
            drItem[(int)CUM_CNT.TO] = 4;
            drItem[(int)CUM_CNT.COLOR] = "#FFAED7";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 5;
            drItem[(int)CUM_CNT.TO] = 6;
            drItem[(int)CUM_CNT.COLOR] = "#FF77BC";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 7;
            drItem[(int)CUM_CNT.TO] = 8;
            drItem[(int)CUM_CNT.COLOR] = " #FF48A5";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 9;
            drItem[(int)CUM_CNT.TO] = 100;
            drItem[(int)CUM_CNT.COLOR] = "#FF0081";
            dtCumConfig.Rows.Add(drItem);
            dtCumConfig.AcceptChanges();

            return dtCumConfig;
        }

        public void DrawCummap(long[] WaferSeqs)
        {
            try
            {
                m_strWaferSeqs = WaferSeqs;

                DrawCummap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Data를 Load할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }


        private void DrawCummap()
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            try
            {
                if (m_ds != null)
                {
                    m_ds.Dispose();
                    m_ds = null;
                }

                m_wMap.Reset();
                m_wMap.WaferColor = Color.White;
                m_wMap.Redraw();
                chartTrend1.DataSource = null;
                chartTrend1.Legends.Clear();
                chartTrend1.ChartAreas.Clear();
                chartTrend1.Series.Clear();

                oPRBMapAnalysis = new RO.ProbeMapAnalysis();

                m_ds = oPRBMapAnalysis.GetCumWaferReport(m_strWaferSeqs);
                if (m_ds == null || m_ds.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                fnCumSheetSet();

            }
            finally
            {
            }
        }


        private void fnCumSheetSet()
        {
            List<string> lsHeader = new List<string>();
            DataTable dtView = null;
            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpread1);

                if (m_ds.Tables.IndexOf("BIN_INFO") < 0 || m_ds.Tables["BIN_INFO"].Rows.Count <= 0)
                    return;

                if (m_ds.Tables.IndexOf("WAFER_INFO") < 0 || m_ds.Tables["WAFER_INFO"].Rows.Count <= 0)
                    return;

                lsHeader.AddRange(strHeaderBase);
                for (int iBin = 0; iBin < m_ds.Tables["BIN_INFO"].Rows.Count; iBin++)
                {
                    if (rbtRate.Checked)
                        lsHeader.Add(string.Format("YLD_BIN{0}", m_ds.Tables["BIN_INFO"].Rows[iBin]["BIN"]));
                    else
                        lsHeader.Add(string.Format("BIN{0}", m_ds.Tables["BIN_INFO"].Rows[iBin]["BIN"]));
                }

                dtView = m_ds.Tables["WAFER_INFO"].DefaultView.ToTable(false, lsHeader.ToArray()).Copy();
                fpSpread1_Sheet1.DataSource = dtView;

                //Header의 정보를 Bin Desc 정보로 치환해준다.
                FarPoint.Win.Spread.CellType.NumberCellType BinCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                if (rbtRate.Checked)
                    BinCellType.DecimalPlaces = 2;
                else
                    BinCellType.DecimalPlaces = 0;

                BinCellType.Separator = ",";

                //틀고정
                fpSpread1_Sheet1.FrozenColumnCount = 5;

                for (int iCol = 0; iCol < fpSpread1_Sheet1.ColumnCount; iCol++)
                {
                    string strHeaderName = fpSpread1_Sheet1.Columns[iCol].Label;
                    if (strHeaderName.IndexOf("BIN") >= 0)
                    {
                        //Bin DESC Table 상에서 Name 을 가져오기 위해 BIN 문자열을 없애고 Matching 한다.
                        DataRow[] drHeader = m_ds.Tables["BIN_INFO"].Select(string.Format("BIN = '{0}'", strHeaderName.Substring(strHeaderName.IndexOf("BIN") + 3)));
                        if (drHeader.Length > 0)
                        {
                            fpSpread1_Sheet1.Columns[iCol].Label = string.Format("{0}({1})", drHeader[0]["BIN_NAME"], drHeader[0]["BIN"]);
                            fpSpread1_Sheet1.Columns[iCol].Tag = drHeader[0]["BIN"].ToString();
                            fpSpread1_Sheet1.Columns[iCol].CellType = BinCellType;
                            fpSpread1_Sheet1.Columns[iCol].BackColor = Color.LightYellow;
                        }
                    }
                    else if (strHeaderName.IndexOf("SEQ") >= 0)
                    {
                        //Seq 정보는 숨긴다.
                        fpSpread1_Sheet1.Columns[iCol].Visible = false;
                    }
                    else
                    {
                        //나머지는 선택 하지 못한다.
                        //fpSpread1_Sheet1.Columns[iCol].CanFocus = false;
                    }
                }

                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
                fpSpread1_Sheet1.Models.Selection.ClearSelection();
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);

            }
            finally
            {

            }
        }

        private void fnWaferMapDraw()
        {
            List<string> arrBinNo = new List<string>();
            List<string> arrWaferSeq = new List<string>();
            string strBinNo = string.Empty;
            DataTable dtMapData = null;
            DataTable dtMapIndex = null;
            DataTable dtMapIndexRow = null;
            DataTable dv = null;

            int iFrom = -1;
            int iTo = -1;
            Color crColor = Color.Empty;

            MainForm.SetStatusMessage("조회를 시작 합니다.");

            try
            {
                m_wMap.Reset();
                m_wMap.WaferColor = Color.White;
                m_wMap.Redraw();
                chartTrend1.DataSource = null;
                chartTrend1.Legends.Clear();
                chartTrend1.ChartAreas.Clear();
                chartTrend1.Series.Clear();

                if (m_ds.Tables.IndexOf("MAPDATA") < 0 || m_ds.Tables["MAPDATA"].Rows.Count <= 0)
                    return;

                FarPoint.Win.Spread.Model.CellRange[] cr = fpSpread1_Sheet1.GetSelections();

                if (fpSpread1_Sheet1 == null || fpSpread1_Sheet1.DataSource == null || fpSpread1_Sheet1.RowCount <= 0) return;

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                dv = (DataTable)fpSpread1_Sheet1.DataSource;
                for (int i = 0; i < cr.Length; i++)
                {
                    //전체 Select 및 구간 Select 에 대한 예외 처리
                    if (cr[i].ColumnCount < 0)
                    {
                        for (int ia = 0; ia < fpSpread1_Sheet1.ColumnCount; ia++)
                        {
                            strBinNo = Convert.ToString(fpSpread1_Sheet1.Columns[ia].Tag);
                            if (string.IsNullOrEmpty(strBinNo) == false)
                                arrBinNo.Add(strBinNo);
                        }
                    }
                    else
                    {
                        for (int c = 0; c < cr[i].ColumnCount; c++)
                        {
                            strBinNo = Convert.ToString(fpSpread1_Sheet1.Columns[cr[i].Column + c].Tag);
                            if (string.IsNullOrEmpty(strBinNo) == false)
                                arrBinNo.Add(strBinNo);
                        }
                    }

                    if (cr[i].RowCount < 0)
                    {
                        foreach (DataRow dr in dv.Rows)
                        {
                            arrWaferSeq.Add(dr["WAFER_SEQ"].ToString());
                        }
                    }
                    else
                    {
                        for (int r = 0; r < cr[i].RowCount; r++)
                        {
                            arrWaferSeq.Add(dv.Rows[cr[i].Row + r]["WAFER_SEQ"].ToString());
                        }
                    }
                }

                //선택된 Columns 가 없을 경우 전체 조회 한다.
                if (cr.Length <= 0)
                {
                    for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                    {
                        strBinNo = Convert.ToString(fpSpread1_Sheet1.Columns[i].Tag);
                        if (string.IsNullOrEmpty(strBinNo) == false)
                            arrBinNo.Add(strBinNo);
                    }

                    foreach (DataRow dr in dv.Rows)
                    {
                        arrWaferSeq.Add(dr["WAFER_SEQ"].ToString());
                    }

                }

                if (arrBinNo.Count <= 0 || m_ds.Tables["MAPDATA"].Select(
                    string.Format("BIN IN ({0}) AND WAFER_SEQ IN ({1})", string.Join(",", arrBinNo.ToArray()), string.Join(",", arrWaferSeq.ToArray()))).Length <= 0)
                    return;

                //선택된 Bin No 를 기준으로 Map Data 를 Filtering 한다.
                dtMapData = m_ds.Tables["MAPDATA"].Select(
                    string.Format("BIN IN ({0}) AND WAFER_SEQ IN ({1})", string.Join(",", arrBinNo.ToArray()), string.Join(",", arrWaferSeq.ToArray()))).CopyToDataTable<DataRow>();
                if (dtMapData == null || dtMapData.Rows.Count <= 0)
                    return;

                //Map Index 를 Grouping 한다.
                dtMapIndex = m_ds.Tables["MAPDATA"].DefaultView.ToTable(true, "X", "Y");

                //Map Table 을 다시 만들어 준다.
                dtMapIndexRow = new DataTable();
                dtMapIndexRow.Columns.Add(new DataColumn("X", typeof(int)));
                dtMapIndexRow.Columns.Add(new DataColumn("Y", typeof(int)));
                dtMapIndexRow.Columns.Add(new DataColumn("COUNT", typeof(int)));

                for (int iData = 0; iData < dtMapIndex.Rows.Count; iData++)
                {
                    DataRow drNew = dtMapIndexRow.NewRow();
                    drNew["X"] = dtMapIndex.Rows[iData]["X"];
                    drNew["Y"] = dtMapIndex.Rows[iData]["Y"];
                    drNew["COUNT"] = dtMapData.Select(string.Format("X = {0} AND Y = {1}", dtMapIndex.Rows[iData]["X"], dtMapIndex.Rows[iData]["Y"])).Length;
                    dtMapIndexRow.Rows.Add(drNew);
                }

                dtMapIndexRow.AcceptChanges();

                //Wafer Map Set
                /// Color Depth ====================================================================
                for (int i = 0; i <= m_strWaferSeqs.Length; i++)
                {
                    //m_wMap.SetColor(i, Color.Red, (255 / (m_strWaferSeqs.Length + 1)) * i);

                    Color oColor = Color.FromArgb((255 / (m_strWaferSeqs.Length + 1)) * i
                                   , 255
                                   , (255 / (i + 2))
                                   , (255 / (i + 2)));


                    m_wMap.SetColor(i, oColor);
                }

                //정의된 Count 별로 색상을 표현 한다.
                foreach (DataGridViewRow dRow in dgCumCount.Rows)
                {
                    if (dRow.IsNewRow == true)
                        continue;

                    if (int.TryParse(dRow.Cells[(int)CUM_CNT.FROM].Value.ToString(), out iFrom) == false)
                        continue;

                    if (int.TryParse(dRow.Cells[(int)CUM_CNT.TO].Value.ToString(), out iTo) == false)
                        continue;

                    crColor = ColorTranslator.FromHtml(dRow.Cells[(int)CUM_CNT.COLOR].Value.ToString());

                    if (iFrom > iTo)
                        throw new Exception("Form 값이 To 값보다 큽니다.");

                    //Bin(갯수) 별로 색상을 재정의 한다.
                    for (int ic = iFrom; ic <= iTo; ic++)
                    {
                        m_wMap.SetColor(ic, crColor);
                    }

                    //m_wMap.SetItemCountColorList.Add(oItemColor);
                }

                //Redraw 시 마다 File 상에 저장 해놓고 Form Load 시 불러 온다.
                if (dgCumCount.Rows.Count > 1)
                {
                    FileInfo oConfigFile = new FileInfo(strConfigFullPath);
                    if (oConfigFile != null && oConfigFile.Exists)
                    {
                        oConfigFile.Delete();
                    }

                    DirectoryInfo oDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM"));
                    if (oDir.Exists == false)
                        oDir.Create();

                    DataTable dtWrite = (DataTable)dgCumCount.DataSource;
                    dtWrite.WriteXml(oConfigFile.FullName);
                }

                /// Recipe===============================================================================

                m_wMap.VisibleDieValue = true;
                m_wMap.WaferSize = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMap.OriginIndexX = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMap.OriginIndexY = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMap.OriginX = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMap.OriginY = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMap.FirstDieX = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMap.FirstDieY = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_wMap.EdgeSize = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_wMap.NotchType = DACrux.Base.Notch.Notch; //m_ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Notch : DACrux.Base.Notch.Flat;

                m_wMap.VisibleDieBorder = true;
                m_wMap.DieBorderColor = Color.Black;
                m_wMap.WaferColor = Color.DimGray;

                int iXYDir = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                m_wMap.DataSource = dtMapIndexRow;

                //Shot 관련 정보확인 및 Draw
                if (m_ds.Tables.IndexOf("SHOT_DEF") >= 0 && m_ds.Tables["SHOT_DEF"] != null && m_ds.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    foreach (DataRow dr in m_ds.Tables["SHOT_DEF"].Rows)
                    {
                        int iTemp = 0;

                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            m_wMap.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            m_wMap.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            m_wMap.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            m_wMap.ShotStartY = iTemp;
                    }
                }

                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();

                //Chart를 그려 준다.
                fnWaferBinChart(dtMapData, arrBinNo.ToArray());
            }
            finally
            {
                this.Cursor = Cursors.Default;

                if (dtMapData != null)
                    dtMapData.Dispose();

                if (dtMapIndex != null)
                    dtMapIndex.Dispose();

                if (dtMapIndexRow != null)
                    dtMapIndexRow.Dispose();

                if (dv != null)
                    dv.Dispose();

                MainForm.SetStatusMessage(null);
            }
        }

        private void fnWaferBinChart(DataTable dtData, string[] strBinNoList)
        {
            ArrayList arrBinNo = new ArrayList();
            string strBinNo = string.Empty;
            string strBinName = string.Empty;

            try
            {
                chartTrend1.Legends.Clear();
                chartTrend1.ChartAreas.Clear();
                chartTrend1.ChartAreas.Add("Default");
                chartTrend1.Series.Clear();
                chartTrend1.Series.Add("Default");

                chartTrend1.Series["Default"].SmartLabelStyle.Enabled = true;
                chartTrend1.Series["Default"].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                chartTrend1.Series["Default"].SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                chartTrend1.Series["Default"].SmartLabelStyle.CalloutLineColor = Color.Black;
                chartTrend1.Series["Default"].SmartLabelStyle.CalloutLineWidth = 1;
                chartTrend1.Series["Default"].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;
                chartTrend1.Series["Default"].MarkerSize = 2;
                chartTrend1.Series["Default"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chartTrend1.Series["Default"].BorderWidth = 2;
                chartTrend1.Series["Default"].ChartType = SeriesChartType.Column;
                chartTrend1.Series["Default"].IsValueShownAsLabel = true;

                //chart Point 입력
                for (int ir = 0; ir < strBinNoList.Length; ir++)
                {
                    DataPoint pt = null;
                    int iValue = 0;

                    strBinNo = strBinNoList[ir];
                    strBinName = string.Format("{0}({1})", strBinNo, strBinNo);

                    iValue = dtData.Select(string.Format("BIN = {0}", strBinNo)).Length;

                    if (m_ds.Tables.IndexOf("BIN_INFO") > -1)
                    {
                        DataRow[] drHeader = m_ds.Tables["BIN_INFO"].Select(string.Format("BIN = '{0}'", strBinNo));
                        if (drHeader.Length > 0)
                            strBinName = string.Format("{0}({1})", drHeader[0]["BIN_NAME"], drHeader[0]["BIN"]);
                    }

                    pt = new DataPoint(chartTrend1.Series["Default"]);

                    pt.Tag = strBinNo;
                    pt.ToolTip = String.Format("{0} : {1}", strBinName, iValue);
                    pt.BorderWidth = 2;
                    pt.SetValueXY(strBinName, iValue);
                    chartTrend1.Series["Default"].Points.Add(pt);
                }

                //Zoom 관련 속성
                chartTrend1.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                chartTrend1.ChartAreas["Default"].CursorX.AutoScroll = true;
                chartTrend1.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                chartTrend1.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                chartTrend1.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                chartTrend1.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                chartTrend1.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
                chartTrend1.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);

                chartTrend1.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                chartTrend1.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                chartTrend1.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                chartTrend1.ChartAreas["Default"].AxisX.Interval = 1;
                chartTrend1.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                chartTrend1.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                chartTrend1.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
                chartTrend1.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                chartTrend1.ApplyPaletteColors();

            }
            finally
            {
            }
        }

        private void fnCumTrendDraw(List<Point> selectedDies)
        {
            List<string> arrBinNo = new List<string>();
            List<string> arrWaferSeq = new List<string>();
            string strBinNo = string.Empty;
            DataTable dtMapData = null;
            DataTable dtMapIndex = null;
            DataTable dtMapBin = null;
            DataTable dv = null;

            MainForm.SetStatusMessage("조회를 시작 합니다.");

            try
            {
                chartTrend1.DataSource = null;
                chartTrend1.Legends.Clear();
                chartTrend1.ChartAreas.Clear();
                chartTrend1.Series.Clear();

                chartTrend2.DataSource = null;
                chartTrend2.Legends.Clear();
                chartTrend2.ChartAreas.Clear();
                chartTrend2.Series.Clear();

                if (m_ds.Tables.IndexOf("MAPDATA") < 0 || m_ds.Tables["MAPDATA"].Rows.Count <= 0)
                    return;

                FarPoint.Win.Spread.Model.CellRange[] cr = fpSpread1_Sheet1.GetSelections();

                if (fpSpread1_Sheet1 == null || fpSpread1_Sheet1.DataSource == null || fpSpread1_Sheet1.RowCount <= 0)
                    return;

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                dv = (DataTable)fpSpread1_Sheet1.DataSource;
                for (int i = 0; i < cr.Length; i++)
                {
                    //전체 Select 및 구간 Select 에 대한 예외 처리
                    if (cr[i].RowCount < 0)
                    {
                        foreach (DataRow dr in dv.Rows)
                        {
                            arrWaferSeq.Add(dr["WAFER_SEQ"].ToString());
                        }
                    }
                    else
                    {
                        for (int r = 0; r < cr[i].RowCount; r++)
                        {
                            arrWaferSeq.Add(dv.Rows[cr[i].Row + r]["WAFER_SEQ"].ToString());
                        }
                    }
                }

                //선택된 Columns 가 없을 경우 전체 조회 한다.
                if (cr.Length <= 0)
                {
                    foreach (DataRow dr in dv.Rows)
                    {
                        arrWaferSeq.Add(dr["WAFER_SEQ"].ToString());
                    }
                }

                // Wafer Seq 중복 제거
                arrWaferSeq = arrWaferSeq.Distinct().ToList();

                if (m_ds.Tables["MAPDATA"].Select(string.Format("WAFER_SEQ IN ({0})", string.Join(",", arrWaferSeq.ToArray()))).Length <= 0)
                    return;


                //Map Index 를 selectedDies 에 맞춰서 다시 만들어준다.
                dtMapData = new DataTable();
                dtMapIndex = new DataTable();
                dtMapData.Columns.Add(new DataColumn("WAFER_SEQ", typeof(long)));
                dtMapData.Columns.Add(new DataColumn("X", typeof(int)));
                dtMapData.Columns.Add(new DataColumn("Y", typeof(int)));
                dtMapData.Columns.Add(new DataColumn("BIN", typeof(string)));

                for (int iData = 0; iData < selectedDies.Count; iData++)
                {
                    dtMapIndex = m_ds.Tables["MAPDATA"].Select(string.Format("WAFER_SEQ IN ({0}) AND X = {1} AND Y = {2}", string.Join(",", arrWaferSeq.ToArray()), selectedDies[iData].X.ToString(), selectedDies[iData].Y.ToString())).CopyToDataTable<DataRow>();

                    if (dtMapIndex.Rows.Count > 0)
                    {
                        for (int iData1 = 0; iData1 < dtMapIndex.Rows.Count; iData1++)
                        {
                            DataRow drNew = dtMapData.NewRow();
                            drNew["WAFER_SEQ"] = dtMapIndex.Rows[iData1]["WAFER_SEQ"];
                            drNew["X"] = selectedDies[iData].X;
                            drNew["Y"] = selectedDies[iData].Y;
                            drNew["BIN"] = dtMapIndex.Rows[iData1]["BIN"];

                            dtMapData.Rows.Add(drNew);
                        }
                    }
                }

                dtMapData.AcceptChanges();


                //Bin Array 를 다시 만들어 준다.
                dtMapBin = new DataTable();
                dtMapData.DefaultView.Sort = "BIN ASC";
                dtMapBin = dtMapData.DefaultView.ToTable(true, "BIN");
                if (dtMapBin.Rows.Count > 0)
                {
                    arrBinNo.Clear();

                    for (int iData = 0; iData < dtMapBin.Rows.Count; iData++)
                    {
                        arrBinNo.Add(dtMapBin.Rows[iData]["BIN"].ToString());
                    }
                }


                //Chart를 그려 준다.
                fnWaferBinChart(dtMapData, arrBinNo.ToArray());

                fnWaferTrendChart(dtMapData, arrWaferSeq.ToArray());

            }
            finally
            {
                if (dtMapData != null)
                    dtMapData.Dispose();

                if (dtMapIndex != null)
                    dtMapIndex.Dispose();

                if (dtMapBin != null)
                    dtMapBin.Dispose();

                if (dv != null)
                    dv.Dispose();

                MainForm.SetStatusMessage(null);
            }
        }

        private void fnWaferTrendChart(DataTable dtData, string[] strWaferSeqList)
        {
            ArrayList arrWaferSeq = new ArrayList();
            string strWaferSeq = string.Empty;
            string strWaferId = string.Empty;

            try
            {
                chartTrend2.Legends.Clear();
                chartTrend2.ChartAreas.Clear();
                chartTrend2.ChartAreas.Add("Default");
                chartTrend2.Series.Clear();
                chartTrend2.Series.Add("Default");
                chartTrend2.Series.Add("All");
                

                chartTrend2.Series["Default"].SmartLabelStyle.Enabled = true;
                chartTrend2.Series["Default"].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                chartTrend2.Series["Default"].SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                chartTrend2.Series["Default"].SmartLabelStyle.CalloutLineColor = Color.Black;
                chartTrend2.Series["Default"].SmartLabelStyle.CalloutLineWidth = 1;
                chartTrend2.Series["Default"].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;
                chartTrend2.Series["Default"].MarkerSize = 2;
                chartTrend2.Series["Default"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chartTrend2.Series["Default"].BorderWidth = 2;
                chartTrend2.Series["Default"].ChartType = SeriesChartType.Line;
                chartTrend2.Series["Default"].Color = Color.Blue;
                chartTrend2.Series["Default"].IsValueShownAsLabel = true;
                
                chartTrend2.Series["All"].SmartLabelStyle.Enabled = true;
                chartTrend2.Series["All"].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                chartTrend2.Series["All"].SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                chartTrend2.Series["All"].SmartLabelStyle.CalloutLineColor = Color.Black;
                chartTrend2.Series["All"].SmartLabelStyle.CalloutLineWidth = 1;
                chartTrend2.Series["All"].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;
                chartTrend2.Series["All"].MarkerSize = 2;
                chartTrend2.Series["All"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chartTrend2.Series["All"].BorderWidth = 2;
                chartTrend2.Series["All"].ChartType = SeriesChartType.Line;
                chartTrend2.Series["All"].Color = Color.Red;
                chartTrend2.Series["All"].IsValueShownAsLabel = true;
                
                //chart Point 입력 - Selecteddies
                for (int ir = 0; ir < strWaferSeqList.Length; ir++)
                {
                    DataPoint pt = null;
                    double dValue = 0;

                    strWaferSeq = strWaferSeqList[ir];
                    strWaferId = string.Format("{0}", strWaferSeq);

                    double dAllDie = m_ds.Tables["MAPDATA"].Select(string.Format("WAFER_SEQ = {0}", strWaferSeq)).Length;
                    double dGoodDie = m_ds.Tables["MAPDATA"].Select(string.Format("WAFER_SEQ = {0} AND BIN = {1}", strWaferSeq, "0")).Length;
                    double dRate = (dAllDie - dGoodDie) / dAllDie * 100;
                    dValue = Double.Parse(dRate.ToString("N2"));

                    if (m_ds.Tables.IndexOf("WAFER_INFO") > -1)
                    {
                        DataRow[] drHeader = m_ds.Tables["WAFER_INFO"].Select(string.Format("WAFER_SEQ = '{0}'", strWaferSeq));
                        if (drHeader.Length > 0)
                            strWaferId = string.Format("{0}", drHeader[0]["WAFER_ID"]);
                    }

                    pt = new DataPoint(chartTrend2.Series["Default"]);

                    pt.Tag = strWaferSeq;
                    pt.ToolTip = String.Format("{0} : {1}", strWaferId, dValue);
                    pt.BorderWidth = 2;
                    pt.SetValueXY(strWaferId, dValue);
                    chartTrend2.Series["All"].Points.Add(pt);
                }

                //chart Point 입력 - Selecteddies
                for (int ir = 0; ir < strWaferSeqList.Length; ir++)
                {
                    DataPoint pt = null;
                    double dValue = 0;

                    strWaferSeq = strWaferSeqList[ir];
                    strWaferId = string.Format("{0}", strWaferSeq);

                    double dAllDie = dtData.Select(string.Format("WAFER_SEQ = {0}", strWaferSeq)).Length;
                    double dGoodDie = dtData.Select(string.Format("WAFER_SEQ = {0} AND BIN = {1}", strWaferSeq, "0")).Length;
                    double dRate = (dAllDie - dGoodDie) / dAllDie * 100;
                    dValue = Double.Parse(dRate.ToString("N2"));

                    if (m_ds.Tables.IndexOf("WAFER_INFO") > -1)
                    {
                        DataRow[] drHeader = m_ds.Tables["WAFER_INFO"].Select(string.Format("WAFER_SEQ = '{0}'", strWaferSeq));
                        if (drHeader.Length > 0)
                            strWaferId = string.Format("{0}", drHeader[0]["WAFER_ID"]);
                    }

                    pt = new DataPoint(chartTrend2.Series["Default"]);

                    pt.Tag = strWaferSeq;
                    pt.ToolTip = String.Format("{0} : {1}", strWaferId, dValue);
                    pt.BorderWidth = 2;
                    pt.SetValueXY(strWaferId, dValue);
                    chartTrend2.Series["Default"].Points.Add(pt);
                }

                //Zoom 관련 속성
                chartTrend2.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                chartTrend2.ChartAreas["Default"].CursorX.AutoScroll = true;
                chartTrend2.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                chartTrend2.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                chartTrend2.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                chartTrend2.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                chartTrend2.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
                chartTrend2.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);

                chartTrend2.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                chartTrend2.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                chartTrend2.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                chartTrend2.ChartAreas["Default"].AxisX.Interval = 1;
                chartTrend2.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                chartTrend2.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                chartTrend2.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
                chartTrend2.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                chartTrend2.ApplyPaletteColors();

            }
            finally
            {
            }
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            bool bFlag = true;
            long[] Wafers = new long[wafer.Length];
            for (int i = 0; i < wafer.Length; i++)
            {
                if (!base.RESV_03.Contains(wafer[i].Testarea))
                {
                    bFlag = false;
                    break;
                }
                Wafers[i] = Convert.ToInt32(wafer[i].WaferSeq);
            }

            if (!bFlag)
                return;

            DrawCummap(Wafers);
            this.TPWaferList = wafer;
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

        #endregion User Method

        #region Event Handler

        private void rbtType_Click(object sender, EventArgs e)
        {
            fnCumSheetSet();
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            fnWaferMapDraw();
        }

        private void m_wMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            try
            {
                txtXIndex.Text = NewDie.IndexX.ToString();
                txtYIndex.Text = NewDie.IndexY.ToString();
                txtBin.Text = NewDie.BinNumber.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnReDrawChart_Click(object sender, EventArgs e)
        {
            List<Point> selectedDies = m_wMap.SelectedDies;

            try
            {
                fnCumTrendDraw(selectedDies);
            }
            finally
            {

            }
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            fnWaferMapDraw();
        }

        private void dgCumCount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)CUM_CNT.COLOR)
            {
                ColorDialog ColorDLG = new ColorDialog();

                if (ColorDLG.ShowDialog() == DialogResult.OK)
                {
                    dgCumCount.Rows[e.RowIndex].Cells[2].Value = ColorTranslator.ToHtml(ColorDLG.Color);
                    dgCumCount.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorDLG.Color;
                }
            }
        }

        private void colorResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetCumcountColor(GetInitDefectConfig());
        }

        #endregion Event Handler

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpSpread1.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(m_wMap);
            sheet.Add();
            sheet.Add(chartTrend1);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map n Chart";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion 

    }
}
