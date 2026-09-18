using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting.Utilities;
using DACrux.Common.RO;
using System.IO;

namespace DACrux.MapAnalysis.Control
{
    /// <summary>
    /// TPUCMapView 에 대한 요약 설명입니다.
    /// </summary>
    public partial class TPUCMapView : DACrux.Framework.Base.DACruxCTLBasic01, DACrux.Framework.Base.IExportExcel
    {
        #region [ Data Field ]

        public delegate void DutAnalysis(object oWaferMap, string[] aCheckedBins);
        public delegate void ParaAnalysis(object oWaferMap, string strPara);
        public event DutAnalysis OnDutAnalysis;
        public event ParaAnalysis OnParaAnalysis;

        public struct BinColor
        {
            public int bin;
            public Color color;

            public BinColor(int inBin, Color inColor)
            {
                bin = inBin;
                color = inColor;
            }
        }

        private List<BinColor> m_lstBinColor = null;
        private DataSet m_dsMap = null;
        private DACrux.Base.WaferRecipe oWaferRecipe = new DACrux.Base.WaferRecipe();
        private bool m_bShowDataGrid = true;
        private bool m_bShowLegends = false;
        private bool m_bShowUserInformation = false;
        private string[] m_sUserInformation = null;
        private Docking m_LegendsDockPos = Docking.Bottom;
        private int m_nDisplayFaltAngle = -1;
        private string m_sZonalGoodBins = "1";
        private string strParaItem = string.Empty;
        private DataTable dtMapData = null;
        private DataTable dtAVIImage = null;
        private string WAFER_ID = string.Empty;
        private string PROGRAM = string.Empty;
        private string DEVICE = string.Empty;
        private string TESTAREA = string.Empty;
        //private bool m_IsMapToMap = false;
        private readonly string DIENUM = "DIE_NUM";

        public List<BinColor> mBinColor
        {
            get { return m_lstBinColor; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowCellInfo
        {
            set { pnlInfo.Visible = value; }
            get { return pnlInfo.Visible; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowMapInfo
        {
            set
            {
                ultraDockManager1.ControlPanes["WM"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["WM"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowParaItem
        {
            set
            {
                ultraDockManager1.ControlPanes["PI"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["PI"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowBinChart
        {
            set
            {
                ultraDockManager1.ControlPanes["CT"].Closed = !value;
                chkGoodBin.Visible = value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["CT"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowRawData
        {
            set
            {
                ultraDockManager1.ControlPanes["RW"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["RW"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowAVIImage
        {
            set
            {
                ultraDockManager1.ControlPanes["AV"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["AV"].Closed; }
        }


        [Category("Option"), DefaultValue(true)]
        public bool ShowLowYield
        {
            set
            {
                ultraDockManager1.ControlPanes["AD"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["AD"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowZonalChart
        {
            set
            {
                ultraDockManager1.ControlPanes["ZL"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["ZL"].Closed; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool ShowBinChange
        {
            set
            {
                ultraDockManager1.ControlPanes["BC"].Closed = !value;
                SetFocusDockingPanes();
            }
            get { return !ultraDockManager1.ControlPanes["BC"].Closed; }
        }

        private void SetFocusDockingPanes()
        {
            if (ultraDockManager1 == null || ultraDockManager1.ControlPanes.Count < 1)
                return;

            for (int i = 0; i < ultraDockManager1.ControlPanes.Count; i++)
            {
                if (ultraDockManager1.ControlPanes[i].Closed == false)
                {
                    ultraDockManager1.ControlPanes[i].Activate();
                    break;
                }
            }
        }
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool IsMapToMap
        //{
        //    set { m_wMap.DieDefaultColor = value; }
        //    get { return m_wMap.DieDefaultColor; }
        //}

        #region [ Bin Chart Option ]

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BinChart_ShowDataGrid
        {
            set { m_bShowDataGrid = value; }
            get { return m_bShowDataGrid; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BinChart_ShowLegends
        {
            set
            {
                m_bShowLegends = value;
                if (BinTrendChart == null || BinTrendChart.Legends == null || BinTrendChart.Legends.Count < 1)
                    return;

                for (int i = 0; i < BinTrendChart.Legends.Count; i++)
                    BinTrendChart.Legends[i].Enabled = m_bShowLegends;
            }
            get { return m_bShowLegends; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Docking BinChart_LegendsDock
        {
            set
            {
                m_LegendsDockPos = value;
                if (BinTrendChart == null || BinTrendChart.Legends == null || BinTrendChart.Legends.Count < 1)
                    return;

                for (int i = 0; i < BinTrendChart.Legends.Count; i++)
                    BinTrendChart.Legends[i].Docking = m_LegendsDockPos;
            }
            get { return m_LegendsDockPos; }
        }

        #endregion

        #region [ Wafer Map Option ]

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DACrux.Base.DieDisplayValue WaferMap_DisplayVilue
        {
            set
            {
                m_wMap.DisplayDieValue = value;

                switch (m_wMap.DisplayDieValue)
                {
                    case DACrux.Base.DieDisplayValue.Bin:
                        cbDisplayValue.SelectedIndex = 0;
                        break;
                    case DACrux.Base.DieDisplayValue.BinChar:
                        cbDisplayValue.SelectedIndex = 1;
                        break;
                    case DACrux.Base.DieDisplayValue.Shot:
                        cbDisplayValue.SelectedIndex = 2;
                        break;
                    case DACrux.Base.DieDisplayValue.Site:
                        cbDisplayValue.SelectedIndex = 3;
                        break;
                    default:
                        cbDisplayValue.SelectedIndex = -1;
                        break;
                }
            }
            get { return m_wMap.DisplayDieValue; }
        }

        // Wafer Map 정보를 사용자가 지정한 것으로 표시할지 여부를 설정하거나 가져온다.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowUserInformation
        {
            set { m_bShowUserInformation = value; }
            get { return m_bShowUserInformation; }
        }

        // Wafer Map에 정보를 사용자가 지정하여 표시한다.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] UserInformation
        {
            set
            {
                m_sUserInformation = value;

                if (m_sUserInformation == null)
                    m_bShowUserInformation = false;
                else
                    m_bShowUserInformation = true;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExcelButtonVisible
        {
            set { btnToExcel.Visible = value; }
            get { return btnToExcel.Visible; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MapDrawButtonVisible
        {
            set { btnApply.Visible = value; }
            get { return btnApply.Visible; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DutAnalysisVisible
        {
            set { btnDutAnalysis.Visible = value; }
            get { return btnDutAnalysis.Visible; }
        }

        [Category("Option"), DefaultValue(true)]
        public bool BinSelectVisuble
        {
            set { cbDisplayValue.Visible = value; }
            get { return cbDisplayValue.Visible; }
        }

        public bool BinSelectEnable
        {
            set { cbDisplayValue.Enabled = value; }
            get { return cbDisplayValue.Enabled; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelecetedBin
        {
            set { m_wMap.SelecetedBin = value; }
            get { return m_wMap.SelecetedBin; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ToGradationDieColor
        {
            set { m_wMap.ToGradationDieColor = value; }
            get { return m_wMap.ToGradationDieColor; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color FromGradationDieColor
        {
            set { m_wMap.FromGradationDieColor = value; }
            get { return m_wMap.FromGradationDieColor; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGradationDie
        {
            set { m_wMap.DrawGradationDie = value; }
            get { return m_wMap.DrawGradationDie; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DACrux.Base.DisplayFlatZone DisplayFlatZone
        {
            // 맵은 저장된 Flat 위치로 그린 후 회전시켜 보여준다.
            set { m_nDisplayFaltAngle = (int)value; }
            get { return (DACrux.Base.DisplayFlatZone)m_nDisplayFaltAngle; }
        }
        #endregion

        /* // DataSource Teble 구조
         * 4개의 DataTable을 가지며 모든 컬럼 데이터가 필수요소는 아니므로 필요한 컬럼만 넘겨줘도 무방하다.
         * Test Para Item의 Limit 값이 없을 경우 Map Data 테이블에서 가공할수도 있다. Table Name을 대괄호로 표시하였다.
         * 
         * Wafer Information[WAFER_INFO] Table
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *  CUS_DEVICE | WAFER_SIZE | CHIP_SIZE_X | CHIP_SIZE_Y | ORIGIN_MICRO_X | ORIGIN_MICRO_Y | ORIGIN_INDEX_X | ORIGIN_INDEX_Y | FIRST_MICRO_X | FIRST_MICRO_Y | FIRST_INDEX_X | FIRST_INDEX_Y | EDGE_SIZE | ANGLE | NETDIE | NOTCH_TYPE | ST_START | ST_INTYPE | ST_XCNT | ST_YCNT | ST_START_X | ST_START_Y | DIE_INDEX_MIN_X | DIE_INDEX_MAX_X | DIE_INDEX_MIN_Y | DIE_INDEX_MAX_Y | XY_DIRECTION | REFERENCEDIE_SETTING | MAP_FILE | SAMPLEDIE | REVERSE_FLAG  
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         * | DEVICE | LOT_ID    | WAFER_ID  | TEST_TYPE | TESTER_ID | PROBE_CARD_ID | PROGRAM   | PROGRAM_REV   |START_TIME | END_TIME
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * 
         * Bin Summary[BINSUM] Table 구조 (Sample)
         * ------------------------------------------------------
         *  BIN  | BIN_CHAR     | BIN_NAME  | COLOR     | PF   | CNT   | TOTAL
         * ------------------------------------------------------
         *   1   | .            | Good Bin  | 000255000 | P    | 9890  | 10000
         *   2   | D            | Fail Bin  | 255000000 | F    | 30    | 10000
         *   
         * 
         * Map Data[MAPDATA] Tablel 구조 (Sample)
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *  WAFER_SEQ   | DIE_ID    | X     | Y     | BIN   | HBIN  | CHAR_BIN  | REPROB_BIN | DUT_ID   | SHOT_NO   | SHOT_CT   | SHOT_IT | PF_FLAG | TEST_ITEM | TEST_ITEM__C .... (TEST_ITEM 컬럼은 검사항목의 이름이며 TEST_ITEM__C는 해당 값의 문자형임. 검사 수만큼 컬럼이 연속됨)
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *     311      |    1      | 100   | 100   |  3    |  3    |	 3      |            |   1      |    1      |  0.39     |   0     |	   F    |  -0.049   |	-0.0490
         *     311      |	 2      | 101   | 100   |  3    |  3    |    3      |            |   2      |    1      |  0.39     |	0     |    F    |  -0.0127  |   -0.0127
         *     
         * 
         * Test Para Item[PARA_ITEM] Table 구조 (Sample)         
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *  FACTORY   | OPER    | DEVICE        | ITEM                  | PROGRAM_ID   | PROGRAM_REV  | TESTER_ID  | ITEM_NO | LSL   | USL   | DIS_LOWER    | DIS_UPPER     | UNIT   | DESCRIPTION | USER_FLAG | USER_ID | UPDATE_TIME
         * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *   PT01    |	P0400   | NOON010PC30   | MIPIDN_LP_H_DCLEVEL   |  PGM_TEST    |    REV0_8    |            |     1   |	0.36565      |	0.6895       |	0.36565     |	0.6895      |	m    |		       |     N     |         |             
         *   PT01    |	P0400   | NOON010PC30   | MIPIDP_LP_L_DCLEVEL2	|  PGM_TEST2   |    REV0_8    |            |   	 4   |	0.01235      |               |	0.01235     |               |        |             |     N     |         |		        
         *   
           // */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object DataSource
        {
            set
            {
                if (value == null)
                    m_dsMap = null;
                else
                    m_dsMap = (DataSet)value;

                m_wMap.DieClear();
                m_wMap.SelecetedBin = "ALL";
            }
            get { return m_dsMap; }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ParaItem
        {
            get { return strParaItem; }
            set { strParaItem = value; }
        }

        #endregion

        #region [ Create & Close ]

        public TPUCMapView()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Control Method ]

        private void chkOption_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (((CheckBox)sender).Name)
                {
                    case "chkCenterMark":
                        m_wMap.CenterMark = ((CheckBox)sender).Checked;
                        break;
                    case "chkScale":
                        m_wMap.ScaleMark = ((CheckBox)sender).Checked;
                        break;
                    case "chkBinVisible":
                        m_wMap.VisibleDieValue = ((CheckBox)sender).Checked;
                        if (m_wMap.VisibleDieValue == true && cbDisplayValue.SelectedIndex < 0)
                            cbDisplayValue.SelectedIndex = 0;
                        break;
                    case "chkVIFail":
                        m_wMap.VisibleVIFail = ((CheckBox)sender).Checked;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void m_wMap_OnSelectDies(object sender, List<Point> selectedDies)
        {
            try
            {
                Point[] ptTotailDies = (Point[])m_wMap.DiesIndex;

                int iSelDieIndex = -1;
                int iGoodCount = 0;

                foreach (Point inPt in selectedDies)
                {
                    iSelDieIndex = Array.IndexOf(ptTotailDies, inPt);
                    if (ShowRawData == true)
                    {
                        fpRawData_Sheet1.Models.Selection.AddSelection(iSelDieIndex, 0, 1, fpRawData_Sheet1.ColumnCount);
                    }

                    if (m_wMap.Dies[iSelDieIndex].BinNumber == 1)
                        iGoodCount++;
                }

                if (TESTAREA == "AVI")
                    fnAVIMapGathering(selectedDies);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AVIImageList.Refresh();
            }
        }

        /// <summary>
        /// AVI Image 정보를 기준으로 가져온다.
        /// </summary>
        /// <param name="selectedDies">null 일 경우 전체 조회</param>
        private void fnAVIMapGathering(List<Point> selectedDies)
        {
            ComConfiguration obj = null;

            DirectoryInfo oDirectory = null;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;
            string strLocalFullPath = string.Empty;

            string strFileName = string.Empty;
            string strFilePath = string.Empty;

            int iCount = 0;

            DataTable dtImages = null;
            try
            {
                AVIImageList.Controls.Clear();
                AVIImageList.Refresh();

                if (m_dsMap.Tables.Contains("IMAGES") == true && m_dsMap.Tables["IMAGES"].Rows.Count > 0)
                    dtImages = m_dsMap.Tables["IMAGES"].Copy();
                else if (m_dsMap.Tables.Contains("MAPDATA") == true && m_dsMap.Tables["MAPDATA"].Rows.Count > 0)
                    dtImages = m_dsMap.Tables["MAPDATA"].Copy();
                else
                    return;

                if (dtImages.Rows.Count <= 0 || dtImages.Columns.Contains("IMAGE_PATH") == false)
                    return;

                StatusMessage("AVI Image를 Download 받는 중입니다.");
                obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIImageFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();

                strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "AVI", this.Name);
                oDirectory = new DirectoryInfo(strLocalPathName);
                if (oDirectory.Exists)
                    oDirectory.Delete(true);

                oDirectory.Create();

                // selectedDies 가 null 인 경우 전체 View 
                if (selectedDies == null)
                {
                    try
                    {
                        DataRow[] odies = dtImages.Select("IMAGE_PATH <> '' ");
                        foreach (DataRow dr in odies)
                        {
                            strFileName = dr["IMAGE_FILE_NAME"].ToString();
                            strFilePath = dr["IMAGE_PATH"].ToString();
                            strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);

                            if (strFilePath.StartsWith("BACKUP") == true)
                                strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                            else
                                strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                            DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                                  strFTPFullPath,
                                  strLocalFullPath,
                                  dr["X"].ToString(),
                                  dr["Y"].ToString(),
                                  dr["BIN"].ToString(),
                                  WAFER_ID,
                                  PROGRAM,
                                  DEVICE,
                                  strFTPIP,
                                  strFTPPort,
                                  strFTPID,
                                  strFTPPass);

                            ControlAdd(oForm);

                            StatusMessage(string.Format("AVI Image를 Download 받는 중입니다. ({0}/{1})", iCount, odies.Length));
                            iCount++;
                        }
                    }
                    catch { }
                }
                else
                {
                    foreach (Point inPt in selectedDies)
                    {
                        //Map Data 에 IMAGE_PATH Columns 정보가 있을 경우 AVI Map 으로 처리 한다.
                        try
                        {
                            DataRow[] odies = dtImages.Select(string.Format("X = {0} AND Y = {1} AND IMAGE_PATH <> '' ", inPt.X, inPt.Y));
                            foreach (DataRow dr in odies)
                            {
                                strFileName = dr["IMAGE_FILE_NAME"].ToString();
                                strFilePath = dr["IMAGE_PATH"].ToString();
                                strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);

                                if (strFilePath.StartsWith("BACKUP") == true)
                                    strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                                else
                                    strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                                DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                                      strFTPFullPath,
                                      strLocalFullPath,
                                      dr["X"].ToString(),
                                      dr["Y"].ToString(),
                                      dr["BIN"].ToString(),
                                      WAFER_ID,
                                      PROGRAM,
                                      DEVICE,
                                      strFTPIP,
                                      strFTPPort,
                                      strFTPID,
                                      strFTPPass);

                                ControlAdd(oForm);
                            }
                        }
                        catch { }

                        StatusMessage(string.Format("AVI Image를 Download 받는 중입니다. ({0}/{1})", iCount, selectedDies.Count));
                        iCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtImages != null)
                    dtImages.Dispose();
                dtImages = null;

                AVIImageList.Refresh();
                StatusMessage(null);
            }
        }

        private void m_wMap_OnChangeCurrentDie(object sender, DACrux.Base.Die NewDie)
        {
            //DataRow[] drAVI = null;
            try
            {
                txtXIndex.Text = NewDie.IndexX.ToString();
                txtYIndex.Text = NewDie.IndexY.ToString();
                txtBin.Text = NewDie.BinNumber.ToString();
                txtShotNo.Text = NewDie.ShotID.ToString();
                txtDutNo.Text = NewDie.SiteNumber.ToString();
                txtValue.Text = NewDie.ParametricValue.ToString();

                if (m_wMap.Dies != null && fpRawData_Sheet1.RowCount > 0 && ShowRawData == true)
                {
                    int i = m_wMap.Dies.IndexOf(NewDie);

                    if (fpRawData_Sheet1.RowCount > i)
                    {
                        fpRawData_Sheet1.ActiveRowIndex = i;
                        fpRawData_Sheet1.Models.Selection.AddSelection(i, 0, 1, fpRawData_Sheet1.ColumnCount);
                        fpRawData.SetViewportTopRow(0, i);
                    }
                }

                // Die num 를 가져오는 부분 추가
                DataRow[] drs = m_dsMap.Tables["MAPDATA"].Select(string.Format("[X] = '{0}' AND [Y] = '{1}'", txtXIndex.Text, txtYIndex.Text));
                if (drs != null && drs.Length > 0)
                {
                    txtDieNum.Text = drs[0]["DIE_NUM"].ToString();
                }
                else
                {
                    txtDieNum.Text = string.Empty;
                }

                //if (AVIImageList.Controls.Count > 0)
                //    AVIImageList.Controls.Clear();

                ////Map Data 에 IMAGE_PATH Columns 정보가 있을 경우 AVI Map 으로 처리 한다.
                //try
                //{
                //    if (TESTAREA == "AVI" && dtAVIImage != null && dtAVIImage.Select(string.Format("X = {0} AND Y = {1}", NewDie.IndexX, NewDie.IndexY)).Length > 0)
                //    {
                //        drAVI = dtAVIImage.Select(string.Format("X = {0} AND Y = {1}", NewDie.IndexX, NewDie.IndexY));
                //        foreach (DataRow dr in drAVI)
                //        {
                //            DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                //                dr["IMAGE_PATH"].ToString(),
                //                txtXIndex.Text,
                //                txtYIndex.Text,
                //                txtBin.Text,
                //                LOT_ID,
                //                PROGRAM,
                //                DEVICE);
                //            ControlAdd(oForm);
                //        }

                //    }
                //}
                //catch (Exception) { }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ControlAdd(DACrux.TEST.Control.TPUImageInfo container)
        {
            if (AVIImageList.InvokeRequired)
            {
                AVIImageList.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }
                ));
            }
            else
            {
                if (container == null) return;

                container.OnSelectDieIndex += new TEST.Control.SelectDieIndex(SelectDieIndex);

                container.Height = container.Height + container.Margin.Top;
                container.Width = AVIImageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                AVIImageList.Controls.Add(container);
                AVIImageList.ResumeLayout();
            }
        }

        private void SelectDieIndex(Point oIndex)
        {
            try
            {
                m_wMap.SelDie(oIndex.X, oIndex.Y);
                //m_wMap.SetSelectedDie(oIndex.X, oIndex.Y);
                //m_wMap.Focus();
                //m_wMap.SetFocusDie(oIndex.X, oIndex.Y);
                //m_wMap.Focus();
                m_wMap.Focus();
            }
            finally
            {
            }
        }

        private void fpRawData_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            try
            {
                if (ShowRawData == false)
                    return;

                FarPoint.Win.Spread.Model.CellRange[] cr = fpRawData.ActiveSheet.GetSelections();
                m_wMap.ResetSelectedDie();
                for (int i = 0; i < cr.Length; i++)
                {
                    for (int r = 0; r < cr[i].RowCount; r++)
                    {
                        m_wMap.SetSelectedDie(DACrux.Base.Convert.intParse(fpRawData.ActiveSheet.Cells[cr[i].Row + r, 1].Value.ToString())
                                            , DACrux.Base.Convert.intParse(fpRawData.ActiveSheet.Cells[cr[i].Row + r, 2].Value.ToString()));
                    }
                }
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BinTrendChart_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (ShowBinChart == false)
                    return;

                this.Cursor = Cursors.WaitCursor;

                //if (m_wMap.DisplayDieValue.ToString().ToUpper() != "BIN") return;

                HitTestResult result = BinTrendChart.HitTest(e.X, e.Y);
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    DataPoint dp = result.Series.Points[result.PointIndex];
                    dp.ToolTip = string.Format("{0}\n{1}\n{2}", result.Series.YValueMembers
                                                                , result.Series.Points[result.PointIndex].AxisLabel
                                                                , result.Series.Points[result.PointIndex].YValues[0]);

                    if (result.PointIndex > -1)
                    {
                        if (ModifierKeys == Keys.ControlKey || ModifierKeys == Keys.Control)
                        {
                            chkboxSelBin.SetItemChecked(result.PointIndex + 1, true);
                        }
                        else
                        {
                            for (int i = 0; i < chkboxSelBin.Items.Count; i++)
                            {
                                chkboxSelBin.SetItemChecked(i, false);
                            }
                            chkboxSelBin.SetItemChecked(result.PointIndex + 1, true);
                        }
                    }
                    else
                    {
                        chkboxSelBin.SetItemChecked(0, true);
                    }
                }
                else
                {
                    chkboxSelBin.SetItemChecked(0, true);
                }
                btnApply_Click(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnMapCreate_Click(object sender, EventArgs e)
        {
            //RO.T_PRB oPrb = null;
            string strFile = string.Empty;

            try
            {
                // 파일 변환 로직 완료되면 기능 오픈예정
                // oPrb = new RO.T_PRB();
                // strFile = oPrb.CreateMapFile(m_DS.Tables["MAPDATA"], mapInfoProduct);

                if (m_dsMap != null && m_dsMap.Tables["MAPDATA"] != null && m_dsMap.Tables["MAPDATA"].Rows.Count > 0)
                {
                    SaveFileDialog saveFileExcel = new SaveFileDialog();

                    if (saveFileExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // Map File Write
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileExcel.FileName))
                        {
                            sw.WriteLine(strFile);
                            sw.Close();
                        }

                        if (MessageBox.Show("Map File 을 지금 Open 하시겠습니까?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("notepad.exe", saveFileExcel.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DACrux.Utility.ExcelUtilNoStatic excel;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                if (fpRawData_Sheet1.Rows.Count > 0)
                {
                    excel = new DACrux.Utility.ExcelUtilNoStatic();
                    excel.ToExcel(fpRawData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            btnApply.Cursor = Cursors.WaitCursor;
            try
            {
                if (m_wMap.DrawGradationDie == true)
                    m_wMap.DrawGradationDie = false;

                if (cbDisplayValue.SelectedIndex > -1)
                    m_wMap.DisplayDieValue = (DACrux.Base.DieDisplayValue)Enum.Parse(typeof(DACrux.Base.DieDisplayValue), cbDisplayValue.SelectedIndex.ToString());
                else
                    m_wMap.VisibleDieValue = false;

                //INDEX를 통해서 값을 넣어준다. 처음에 조회시에 체크박스 Tag에 값을 넣어두었음
                string[] strBinNum = ((string)chkboxSelBin.Tag).Split(',');
                string strSelBinNum = string.Empty;
                //chkboxSelBin.CheckedItems.CopyTo(strSelBin, 0);


                //object ia = chkboxSelBin.GetItemChecked(i);
                for (int i = 0; i < chkboxSelBin.Items.Count; i++)
                {
                    if (chkboxSelBin.GetItemChecked(i))
                    {
                        strSelBinNum += strBinNum[i] + ",";
                    }
                }

                strSelBinNum = strSelBinNum.Remove(strSelBinNum.LastIndexOf(','), 1);

                if (strSelBinNum.Contains("ALL"))
                {
                    m_wcZonal.SelecetedBin = m_sZonalGoodBins.Trim(',');
                    m_wMap.SelecetedBin = "ALL";
                }
                else
                {
                    m_wMap.SelecetedBin = m_wcZonal.SelecetedBin = strSelBinNum;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                btnApply.Cursor = Cursors.Default;
            }
        }

        private void chkGoodBin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (BinTrendChart == null
                 || BinTrendChart.Series.Count < 1
                 || m_dsMap == null
                 || m_dsMap.Tables.IndexOf("BINSUM") < 0
                 || m_dsMap.Tables["BINSUM"].Rows.Count < 1)
                    return;

                for (int i = 0; i < m_dsMap.Tables["BINSUM"].Rows.Count; i++)
                {
                    if (m_dsMap.Tables["BINSUM"].Rows[i]["PF"].ToString() == "P" && i < BinTrendChart.Series["Count"].Points.Count)
                        BinTrendChart.Series["Count"].Points[i].YValues[0] = chkGoodBin.Checked == true ? DACrux.Base.Convert.doubleParse(m_dsMap.Tables["BINSUM"].Rows[i]["CNT"].ToString()) : 0.0d;
                }

                BinTrendChart.ResetAutoValues();
                BinTrendChart.Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butSelectedReset_Click(object sender, EventArgs e)
        {
            try
            {
                m_wMap.ResetSelectedDie();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkboxSelBin_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (e.Index == 0)
                {
                    for (int i = 1; i < chkboxSelBin.Items.Count; i++)
                        chkboxSelBin.SetItemChecked(i, false);
                }
                else
                {
                    chkboxSelBin.SetItemChecked(0, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbDisplayValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cbDisplayValue.SelectedIndex)
                {
                    case 0:
                        m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
                        break;
                    case 1:
                        m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.BinChar;
                        break;
                    case 2:
                        m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Shot;
                        break;
                    case 3:
                        m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Site;
                        break;
                }
                if (chkBinVisible.Checked == true)
                    chkOption_CheckedChanged(chkBinVisible, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDutAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_wMap.DataSource != null && ((DataTable)m_wMap.DataSource).Rows.Count > 0)
                {
                    string[] strBinNum = ((string)chkboxSelBin.Tag).Split(',');
                    string strSelBinNum = string.Empty;

                    for (int i = 0; i < chkboxSelBin.Items.Count; i++)
                    {
                        if (chkboxSelBin.GetItemChecked(i))
                        {
                            strSelBinNum += strBinNum[i] + ",";
                        }
                    }

                    if (string.IsNullOrEmpty(strSelBinNum) == false)
                        strSelBinNum = strSelBinNum.Substring(0, strSelBinNum.Length - 1);

                    if (OnDutAnalysis != null) OnDutAnalysis(m_wMap, strSelBinNum.Split(','));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnTestDataDraw_Click(object sender, EventArgs e)
        {
            btnTestDataDraw.Cursor = Cursors.WaitCursor;

            string strWaferSeq = string.Empty;
            string strSelBinNum = string.Empty;

            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;

            DataTable dtItem = null;

            try
            {
                oPRBMapAnalysis = new TEST.RO.ProbeMapAnalysis();

                if (lsParaList.Items.Count <= 0 || lsParaList.SelectedItems.Count <= 0)
                    return;

                if (m_dsMap.Tables.IndexOf("WAFER_INFO") < 0 || m_dsMap.Tables["WAFER_INFO"] == null)
                    return;

                if (m_dsMap.Tables.IndexOf("MAPDATA") < 0 || m_dsMap.Tables["MAPDATA"] == null)
                    return;

                for (int i = 0; i < clbParaCut.Items.Count; i++)
                {
                    if (clbParaCut.GetItemChecked(i) == true)
                    {
                        if (i == 0)
                        {
                            strSelBinNum += "," + "ALL";
                            break;
                        }
                        else
                            strSelBinNum += "," + (DACrux.Base.Convert.intParse(cbCutCnt.Text) - i).ToString();
                    }
                }

                strWaferSeq = m_dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_SEQ"].ToString();
                strParaItem = lsParaList.SelectedItems[0].ToString();


                dtItem = oPRBMapAnalysis.SelectWaferParaItem(DACrux.Base.Convert.longParse(strWaferSeq), strParaItem, (int)numericUpDown1.Value);

                //기존 MAPDATA 의 정보를 선택한 Para 를 추가하여 대체 한다.
                m_dsMap.Tables.Remove("MAPDATA");
                m_dsMap.AcceptChanges();

                m_dsMap.Tables.Add(dtItem);
                m_dsMap.AcceptChanges();

                double dbUpper = double.NaN;
                double dbLower = double.NaN;
                double USL = double.NaN;
                double LSL = double.NaN;
                double UCL = double.NaN;
                double LCL = double.NaN;
                double UTL = double.NaN;
                double LTL = double.NaN;

                TxtUSL.Text = string.Empty;
                TxtLSL.Text = string.Empty;
                TxtUCL.Text = string.Empty;
                TxtLCL.Text = string.Empty;

                DataRow[] drs = m_dsMap.Tables["PARA_ITEM"].Select(string.Format("[PARAM_NAME] = '{0}'", strParaItem));
                if (!String.Equals(strParaItem, DIENUM) && (drs == null || drs.Length == 0))
                    return;

                if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                    dbLower = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MIN"]);

                if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                    dbUpper = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MAX"]);

                //Limit 값 관리
                if (drs.Length > 0)
                {
                    TxtUSL.Text = drs[0]["USL"].ToString();
                    USL = Base.Convert.doubleParse(drs[0]["USL"].ToString());
                    TxtLSL.Text = drs[0]["LSL"].ToString();
                    LSL = Base.Convert.doubleParse(drs[0]["LSL"].ToString());
                    TxtUCL.Text = drs[0]["UCL"].ToString();
                    UCL = Base.Convert.doubleParse(drs[0]["UCL"].ToString());
                    TxtLCL.Text = drs[0]["LCL"].ToString();
                    LCL = Base.Convert.doubleParse(drs[0]["LCL"].ToString());
                    txtUTL.Text = drs[0]["UTL"].ToString();
                    UTL = Base.Convert.doubleParse(drs[0]["UTL"].ToString());
                    txtLTL.Text = drs[0]["LTL"].ToString();
                    LTL = Base.Convert.doubleParse(drs[0]["LTL"].ToString());
                }
                else
                {
                    TxtUSL.Text = string.Empty;
                    TxtLSL.Text = string.Empty;
                    TxtUCL.Text = string.Empty;
                    TxtLCL.Text = string.Empty;
                    txtUTL.Text = string.Empty;
                    txtLTL.Text = string.Empty;
                    USL = LSL = UCL = LCL = UTL = LTL = double.NaN;
                }

                m_wMap.DrawGradationDie = true;
                m_wMap.FromGradationDieColor = Color.Lime;
                m_wMap.ToGradationDieColor = Color.Red;
                m_wMap.GradationInterval = DACrux.Base.Convert.intParse(cbCutCnt.Text);
                m_wMap.ParametricColumn = strParaItem;
                m_wMap.GradationMaxValue = dbUpper;
                m_wMap.GradationMinValue = dbLower;
                m_wMap.DisplayValue = "PCMVALUE";
                m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.PCMValue;
                m_wMap.VisibleDieValue = true;
                m_wMap.ParaLimit = false;

                //Specification Limit Draw
                if (chkSpecific.Checked == true && (!double.IsNaN(USL) || double.IsNaN(LSL)))
                {
                    m_wMap.GradationInterval = 2;
                    m_wMap.GradationMaxValue = USL;
                    m_wMap.GradationMinValue = LSL;
                    m_wMap.ParaLimit = true;
                }

                //Control Limit Draw
                if (chkControl.Checked == true && (!double.IsNaN(UCL) || double.IsNaN(LCL)))
                {
                    m_wMap.GradationInterval = 2;
                    m_wMap.GradationMaxValue = UCL;
                    m_wMap.GradationMinValue = LCL;
                    m_wMap.ParaLimit = true;
                }

                //Control Limit Draw
                if (chkTightenLimit.Checked == true && (!double.IsNaN(UTL) || double.IsNaN(LTL)))
                {
                    m_wMap.GradationInterval = 2;
                    m_wMap.GradationMaxValue = UTL;
                    m_wMap.GradationMinValue = LTL;
                    m_wMap.ParaLimit = true;
                }


                this.DrawWaferMap(m_dsMap, oWaferRecipe);

                if (strSelBinNum.Contains("ALL"))
                {
                    m_wcZonal.SelecetedBin = m_sZonalGoodBins.Trim(',');
                    m_wMap.SelecetedBin = "ALL";
                }
                else
                {
                    m_wMap.SelecetedBin = m_wcZonal.SelecetedBin = strSelBinNum;
                }
                //==================================================

                if (ShowRawData == true)
                    SetMapRawData(m_dsMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestDataDraw.Cursor = Cursors.Default;
            }
        }

        private void cbWaferInfoVisible_Click(object sender, EventArgs e)
        {
            try
            {
                m_wMap.VisibleInfomation = cbWaferInfoVisible.Checked;
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lsParaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dbUpper = 0;
            double dbLower = 0;
            try
            {
                if (lsParaList.Items.Count <= 0 || lsParaList.SelectedItems.Count <= 0)
                    return;

                if (cbCutCnt.Text.Length == 0) return;

                strParaItem = lsParaList.SelectedItems[0].ToString();

                DataRow[] dr = m_dsMap.Tables["PARA_ITEM"].Select(string.Format("[PARAM_NAME] = '{0}'", strParaItem));
                if (!String.Equals(strParaItem, DIENUM) && (dr == null || dr.Length == 0)) return;

                if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                    dbLower = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MIN"]);

                if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                    dbUpper = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MAX"]);

                //if (string.IsNullOrEmpty(dr[0]["LSL"].ToString()) == true)
                //{
                //    if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                //        dbLower = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MIN"]);
                //}
                //else
                //    dbLower = DACrux.Base.Convert.doubleParse(dr[0]["LSL"].ToString());

                //if (string.IsNullOrEmpty(dr[0]["USL"].ToString()) == true)
                //{
                //    if (m_dsMap.Tables.IndexOf("PARA_LIST") > -1 && m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem)).Length > 0)
                //        dbUpper = Convert.ToDouble(m_dsMap.Tables["PARA_LIST"].Select(string.Format("PARAM_NAME = '{0}'", strParaItem))[0]["MAX"]);
                //}
                //else
                //    dbUpper = DACrux.Base.Convert.doubleParse(dr[0]["USL"].ToString());


                //==================================================================================

                int nCutCnt = DACrux.Base.Convert.intParse(cbCutCnt.Text);
                double dbGap = (dbUpper - dbLower) / nCutCnt;

                if (double.IsNaN(dbGap) || dbGap == 0)
                    nCutCnt = 0;

                clbParaCut.Items.Clear();
                for (int i = 0; i < nCutCnt; i++)
                {
                    clbParaCut.Items.Insert(0, string.Format("{0:0.#######} ~ {1:0.#######}", dbLower + (i * dbGap), dbLower + ((i + 1) * dbGap)));
                    clbParaCut.SetItemChecked(0, true);
                }

                clbParaCut.Items.Insert(0, "ALL");
                clbParaCut.SetItemChecked(0, true);

                string strSelBinNum = string.Empty;
                for (int i = 0; i < clbParaCut.Items.Count; i++)
                {
                    if (clbParaCut.GetItemChecked(i) == true)
                    {
                        if (i == 0)
                        {
                            strSelBinNum += "," + "ALL";
                            break;
                        }
                        else
                            strSelBinNum += "," + (nCutCnt - i).ToString();
                    }
                }

                strSelBinNum = strSelBinNum.Substring(1);

                btnTestDataDraw.PerformClick();
                lsParaList.Focus();
            }
            finally
            {

            }
        }

        private void lsParaList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && ((sender as ListBox).SelectedIndex - 1) > -1)
            {
                (sender as ListBox).SelectedIndex--;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Down && ((sender as ListBox).SelectedIndex + 1) < (sender as ListBox).Items.Count)
            {
                (sender as ListBox).SelectedIndex++;
                e.Handled = true;
            }
        }

        private void numericUpDown1_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            btnTestDataDraw.PerformClick();
        }

        #endregion

        #region [ User Method ]
        public void SetMapBinColor(int nBinNumber, Color BinColor)
        {
            try
            {
                m_wMap.SetColor(nBinNumber, BinColor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetDisplayBinValue(DACrux.Base.DieDisplayValue DisplayVilue, bool bVisubled)
        {
            try
            {
                m_wMap.DisplayDieValue = DisplayVilue;
                m_wMap.VisibleDieValue = bVisubled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //맵을 Reset합니다.
        public void Reset()
        {
            try
            {
                //맵초기화
                m_wMap.Reset();
                m_wcZonal.Reset();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WaferClear()
        {
            try
            {
                chkboxSelBin.Items.Clear();
                clbParaCut.Items.Clear();

                DACrux.Utility.Component.InitMSChart(ref BinTrendChart);
                DACrux.Utility.Component.InitSpread(fpRawData, fpAbnData, fpBinChange);

                m_wcZonal.DataSource = null;
                m_wcZonal.DieClear();
                m_wMap.DataSource = null;
                m_wMap.DieClear();

                m_wMap.Redraw();
                m_wcZonal.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Draw(int nAngle = -1)
        {
            // nAngle 값이 입력될 경우 해당 값으로 너치 위치를 그린다.
            // Map이 회전되어 그려지는 것이 아니며 Local Map을 그릴때 너치 방향을 바꾸기 위한 용도이다.
            try
            {
                if (m_dsMap == null)
                    throw new Exception("Map Data Empty.");

                if (AVIImageList.Controls.Count > 0)
                    AVIImageList.Controls.Clear();

                lsParaList.Items.Clear();

                m_wMap.DisplayValue = "BIN";
                m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
                m_wMap.VisibleDieValue = true;
                m_wMap.ParametricColumn = string.Empty;
                m_wMap.DrawGradationDie = false;

                if (chkBinVisible.Checked != m_wMap.VisibleDieValue)
                    m_wMap.VisibleDieValue = chkBinVisible.Checked;
                if (cbDisplayValue.SelectedIndex != (int)m_wMap.DisplayDieValue)
                    m_wMap.DisplayDieValue = (DACrux.Base.DieDisplayValue)Enum.Parse(typeof(DACrux.Base.DieDisplayValue), cbDisplayValue.SelectedIndex.ToString());

                if (MakeWaferRecipe(m_dsMap, ref oWaferRecipe, nAngle) == true)
                {
                    SetMapBinColor(m_dsMap);
                    DrawWaferMap(m_dsMap, oWaferRecipe);

                    if (ShowMapInfo == true)
                        SetWaferMapInfo(m_dsMap);

                    if (ShowParaItem == true)
                        SetParaItem(m_dsMap);

                    if (ShowBinChart == true)
                        DrawChart(m_dsMap);

                    if (ShowRawData == true)
                        SetMapRawData(m_dsMap);

                    if (ShowLowYield == true)
                        SetLowYieldData(m_dsMap);

                    if (ShowBinChange == true)
                        SetBinChange(m_dsMap);

                    if (ShowZonalChart == true)
                    {
                        //if(thZonal != null && thZonal.IsAlive)
                        //{
                        //    while (thZonal.IsAlive)
                        //    {
                        //        thZonal.Abort();
                        //    }
                        //}

                        //thZonal = new System.Threading.Thread(() => DrawZonalMap(m_dsMap));
                        //thZonal.Start();
                        DrawZonalMap(m_dsMap);
                        //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DrawZonalMap), m_dsMap);
                        //System.Threading.Thread thZonal = new System.Threading.Thread(() => DrawZonalMap(m_dsMap, oWaferRecipe));
                        //thZonal.Start();
                    }
                }
                else
                {
                    WaferClear();
                }

                m_wMap.DisplayValue = "BIN";
                m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawShotMap(DataTable dtData, DACrux.Base.WaferRecipe oRecipe)
        {
            try
            {
                if (dtData == null)
                    throw new Exception("Map Data Empty.");

                // Bin Color 설정
                m_lstBinColor = new List<BinColor>();
                m_lstBinColor.Add(new BinColor(1, Color.FromArgb(255, 0, 255, 0)));

                // Map 그리기
                m_wMap.ResetSelectedDie();
                m_wMap.DieClear();

                m_wMap.SetWaferRecipe(oRecipe);
                m_wMap.DataSource = dtData;

                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool MakeWaferRecipe(DataSet dsMap, ref DACrux.Base.WaferRecipe oWaferRecipe, int nAngle = -1)
        {
            DataTable dtWaferInfo = null;
            int nRotationAngle = -1;

            try
            {
                int nIdx = dsMap.Tables.IndexOf("RECIPE");
                if (nIdx > -1 && dsMap.Tables[nIdx].Rows.Count > 0)
                    dtWaferInfo = dsMap.Tables[nIdx];

                nIdx = dsMap.Tables.IndexOf("MAPDATA");
                if (nIdx > -1 && dsMap.Tables[nIdx].Rows.Count > 0)
                    dtMapData = dsMap.Tables[nIdx];

                if (dtMapData == null)
                    return false;

                oWaferRecipe = new DACrux.Base.WaferRecipe();

                double m_dMargin = 0.95D;

                if (dtWaferInfo == null || dtWaferInfo.Rows.Count < 1)
                {
                    throw new Exception("정의된 Map Define 정보가 없습니다. ");
                }
                else
                {
                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_X"].ToString(), out oWaferRecipe.FIRST_DIE_X) == false)
                        oWaferRecipe.FIRST_DIE_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_Y"].ToString(), out oWaferRecipe.FIRST_DIE_Y) == false)
                        oWaferRecipe.FIRST_DIE_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([Y])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_X"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_X) == false)
                        oWaferRecipe.DIE_INDEX_MIN_X = oWaferRecipe.FIRST_DIE_X;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_Y) == false)
                        oWaferRecipe.DIE_INDEX_MIN_Y = oWaferRecipe.FIRST_DIE_Y;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_X"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_X) == false)
                        oWaferRecipe.DIE_INDEX_MAX_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_Y) == false)
                        oWaferRecipe.DIE_INDEX_MAX_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([Y])", "1=1").ToString());

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["NOTCH_TYPE"].ToString(), out oWaferRecipe.NOTCH_TYPE) == false)
                        oWaferRecipe.NOTCH_TYPE = DACrux.Base.Notch.Flat;

                    if (int.TryParse(dtWaferInfo.Rows[0]["ANGLE"].ToString(), out oWaferRecipe.ANGLE) == false)
                        oWaferRecipe.ANGLE = 180;

                    if (!int.TryParse(dsMap.Tables["WAFER_INFO"].Rows[0]["WAF_FLAT"].ToString(), out oWaferRecipe.ANGLE))
                        oWaferRecipe.ANGLE = 180;

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["XY_DIRECTION"].ToString(), out oWaferRecipe.XYDIR) == false)
                        oWaferRecipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;

                    oWaferRecipe.WAFER_SIZE = DACrux.Base.Util.GetValue(dtWaferInfo.Rows[0]["WAFER_SIZE"].ToString(), 200000);

                    if (double.TryParse(dtWaferInfo.Rows[0]["EDGE_SIZE"].ToString(), out oWaferRecipe.EDGE_SIZE) == false)
                        oWaferRecipe.EDGE_SIZE = 3d;

                    if (nAngle != -1)
                        oWaferRecipe.ANGLE = nAngle;

                    m_nDisplayFaltAngle = oWaferRecipe.ANGLE;

                    //화면상에 보여 줄때 Bottom 으로 저장이 되어 있기 때문에 Rotation 각을 보고 Max 값을 치환 해준다.
                    nRotationAngle = (360 - (180 - oWaferRecipe.ANGLE)) % 360;

                    if (nRotationAngle == 90 || nRotationAngle == 270)
                    {
                        int iTempMax = 0;
                        iTempMax = oWaferRecipe.DIE_INDEX_MAX_X;

                        oWaferRecipe.DIE_INDEX_MAX_X = oWaferRecipe.DIE_INDEX_MAX_Y;
                        oWaferRecipe.DIE_INDEX_MAX_Y = iTempMax;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_X"].ToString(), out oWaferRecipe.DIE_SIZE_X) == false)
                    {
                        oWaferRecipe.DIE_SIZE_X = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_X - oWaferRecipe.DIE_INDEX_MIN_X) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_X"].ToString(), out oWaferRecipe.ORIGIN_DIE_X) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_X = oWaferRecipe.DIE_INDEX_MIN_X + (int)Math.Floor((double)oWaferRecipe.XDIES / 2.0d);
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_Y"].ToString(), out oWaferRecipe.ORIGIN_DIE_Y) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_Y = oWaferRecipe.DIE_INDEX_MIN_Y + (int)Math.Floor((double)oWaferRecipe.YDIES / 2.0d);
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_X"].ToString(), out oWaferRecipe.ORIGIN_X) == false)
                    {
                        if (oWaferRecipe.XDIES < 20)
                        {
                            if (oWaferRecipe.XDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_X = 0;
                            else
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90)
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else if (oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_X = -oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else
                                oWaferRecipe.ORIGIN_X = 0;
                        }
                    }


                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_Y"].ToString(), out oWaferRecipe.ORIGIN_Y) == false)
                    {
                        if (oWaferRecipe.YDIES < 20)
                        {
                            if (oWaferRecipe.YDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90 || oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else if (oWaferRecipe.ANGLE == 0)
                                oWaferRecipe.ORIGIN_Y = -oWaferRecipe.DIE_SIZE_Y / 2.0d;
                            else if (oWaferRecipe.ANGLE == 180)
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetVisibleInfomation(bool bVisible)
        {
            try
            {
                m_wMap.VisibleInfomation = bVisible;
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawWaferMap(DataSet dsMap, DACrux.Base.WaferRecipe oWaferRecipe)
        {
            ComConfiguration oComConfig = null;
            int iTemp = 0;
            try
            {
                //m_wMap.ResetSelectedDie();
                m_wMap.DieClear();

                if (dsMap.Tables.IndexOf("MAPDATA") < 0 || dsMap.Tables["MAPDATA"] == null)
                    return;

                m_wMap.SetWaferRecipe(oWaferRecipe);
                m_wMap.AngleOffSet = (360 - oWaferRecipe.ANGLE) % 360;
                m_wMap.DataSource = dsMap.Tables["MAPDATA"];
                //m_wMap.InkingDieAdd();
                //m_wMap.ParametricColumn = "DIE_NUM";
                //m_wMap.DisplayDieValue = Base.DieDisplayValue.PCMValue;
                //m_wMap.VisibleDieValue = true;

                //Shot 관련 정보확인 및 Draw
                m_wMap.VisibleShot = false;
                if (dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && dsMap.Tables["SHOT_DEF"] != null && dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsMap.Tables["SHOT_DEF"].Rows)
                    {
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

                //Mark die 관련 정보 수집
                if (dsMap.Tables.IndexOf("MARKDATA") >= 0 && dsMap.Tables["MARKDATA"] != null && dsMap.Tables["MARKDATA"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsMap.Tables["MARKDATA"].Rows)
                    {
                        int iX = 0;
                        int iY = 0;

                        if (int.TryParse(dr["INDEX_X"].ToString(), out iX) == false)
                            continue;

                        if (int.TryParse(dr["INDEX_Y"].ToString(), out iY) == false)
                            continue;

                        if (m_wMap.IndexOf(iX, iY) < 0)
                        {
                            Base.Die oDie = new Base.Die();
                            oDie.IndexX = iX;
                            oDie.IndexY = iY;
                            oDie.DieProp = 2; // 실제 있는 Die 는 1번 Mark Die 는 2 번
                            oDie.BinNumber = 0;
                            m_wMap.AddDie(oDie);
                        }
                    }
                }

                if (m_nDisplayFaltAngle != -1)
                    m_wMap.ViewAngle = (360 - (oWaferRecipe.ANGLE - (m_nDisplayFaltAngle * 90))) % 360;

                if (m_wMap.DrawGradationDie == true)
                {
                    //Para 의 값의 유무를 확인 한다.
                    string strValidation = dsMap.Tables["MAPDATA"].Compute(string.Format("MAX([{0}])", m_wMap.ParametricColumn.ToUpper()), "1=1").ToString();

                    if (string.IsNullOrEmpty(strValidation))
                    {
                        return;
                    }


                    // 그라데이션 색상을 쓰기 위해서는 "PCMVALUE" 컬럼의 데이터가 필요하다.
                    // "PCMVALUE" 값은 그라데이션의 깊이 값이어도 되며 파라메터 측정값이어도 된다.
                    if (dsMap.Tables["MAPDATA"].Columns.IndexOf(m_wMap.ParametricColumn) < 0)
                    {
                        m_wMap.DrawGradationDie = false;
                    }
                    else
                    {
                        if (double.IsNaN(m_wMap.GradationMaxValue) == true || double.IsNaN(m_wMap.GradationMinValue) == true)
                        {
                            m_wMap.GradationMaxValue = Convert.ToDouble(dsMap.Tables["MAPDATA"].Compute(string.Format("MAX([{0}])", m_wMap.ParametricColumn.ToUpper()), "1=1"));
                            m_wMap.GradationMinValue = Convert.ToDouble(dsMap.Tables["MAPDATA"].Compute(string.Format("MIN([{0}])", m_wMap.ParametricColumn.ToUpper()), "1=1"));

                            m_wMap.GradationInterval = Math.Max(2, (int)Math.Round(m_wMap.GradationMaxValue - m_wMap.GradationMinValue));
                        }
                    }
                }

                if (dsMap.Tables.IndexOf("WAFER_INFO") > -1 && dsMap.Tables["WAFER_INFO"] != null && dsMap.Tables["WAFER_INFO"].Rows.Count > 0)
                {
                    WAFER_ID = dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString();
                    PROGRAM = dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString();
                    DEVICE = dsMap.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString();
                    TESTAREA = dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString();

                    List<string> sWaferInfo = new List<string>();
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("PRODUCT") > -1)
                        sWaferInfo.Add(string.Format("PRODUCT  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("LOT_ID") > -1)
                        sWaferInfo.Add(string.Format("LOT_ID :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("WAFER_ID") > -1 && string.IsNullOrEmpty(dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString()) == false)
                    {
                        sWaferInfo.Add(string.Format("WAFER_ID :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString()));
                        m_wMap.WaferID = dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString();
                    }
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("TESTSTEP") > -1)
                        sWaferInfo.Add(string.Format("TESTAREA  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("PROBE_CARD") > -1 && string.IsNullOrEmpty(dsMap.Tables["WAFER_INFO"].Rows[0]["PROBE_CARD"].ToString()) == false)
                        sWaferInfo.Add(string.Format("PROBE_CARD  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["PROBE_CARD"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("TESTER") > -1 && string.IsNullOrEmpty(dsMap.Tables["WAFER_INFO"].Rows[0]["TESTER"].ToString()) == false)
                        sWaferInfo.Add(string.Format("TESTER  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["TESTER"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("PROGRAM") > -1 && string.IsNullOrEmpty(dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString()) == false)
                        sWaferInfo.Add(string.Format("PROGRAM  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("START_TIME") > -1)
                        sWaferInfo.Add(string.Format("START_TIME  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["START_TIME"].ToString()));
                    if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf("END_TIME") > -1)
                        sWaferInfo.Add(string.Format("END_TIME  :{0}", dsMap.Tables["WAFER_INFO"].Rows[0]["END_TIME"].ToString()));

                    m_wMap.SetInfomation(sWaferInfo.ToArray());
                }

                if (m_bShowUserInformation == true && m_sUserInformation != null)
                    m_wMap.SetInfomation(m_sUserInformation);

                //=================================================================================================================================
                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                //=================================================================================================================================
                oComConfig = new ComConfiguration();
                DataTable dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();

                    foreach (DataRow drInfo in dtInfo.Rows)
                    {
                        if (dsMap.Tables["WAFER_INFO"].Columns.IndexOf(drInfo["NAME"].ToString()) > -1)
                            sWaferInfo.Add(string.Format("{0}  :{1}", drInfo["VALUE"], dsMap.Tables["WAFER_INFO"].Rows[0][drInfo["NAME"].ToString()].ToString()));
                    }

                    m_wMap.SetInfomation(sWaferInfo.ToArray());
                }

                //Wafer Information 사용 여부
                dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION_ENABLE", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    if (dtInfo.Rows[0]["VALUE"].ToString() == "N")
                        m_wMap.SetInfomation(null);
                }

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMapOption.Rows)
                    {
                        string strType = dr["NAME"].ToString();
                        Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                        switch (strType)
                        {
                            //Wafer Base Color
                            case "WAFER_TEST_MAP_BG":
                                m_wMap.WaferColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_TEST_MAP_LINE":
                                m_wMap.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();

                if (dsMap.Tables.IndexOf("BINSUM") > -1 && dsMap.Tables["BINSUM"] != null)
                {
                    if (dsMap.Tables["BINSUM"].Columns.IndexOf("CNT") > -1
                        && dsMap.Tables["BINSUM"].Columns.IndexOf("PF") > -1
                        && dsMap.Tables["MAPDATA"].Rows.Count > 0
                        && dsMap.Tables["BINSUM"].Compute("SUM([CNT])", "[PF] = 'P'") != System.DBNull.Value)
                        txtSelYield.Text = Math.Round(Convert.ToDouble(dsMap.Tables["BINSUM"].Compute("SUM([CNT])", "[PF] = 'P'")) / (double)dsMap.Tables["MAPDATA"].Rows.Count * 100.0, 2).ToString();

                    m_sZonalGoodBins = string.Empty;
                    //DataRow[] drGoodRow = dsMap.Tables["BINSUM"].Select("[PF] = 'P'");
                    //for (int i = 0; i < drGoodRow.Length; i++)
                    //{
                    //    m_sZonalGoodBins += string.Format(",{0}", drGoodRow[i]["BIN"].ToString());
                    //}
                }

                //AVI Image List 를 미리 Local 에 저장해 놓는다.
                //AVIImageGetList(dsMap.Tables["MAPDATA"]);
                //thImage = new System.Threading.Thread(() => AVIImageGetList(dsMap.Tables["MAPDATA"]));
                //thImage.Start();
            }
            catch (Exception ex)
            {
                m_wMap.DieClear();
                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                throw ex;
            }
        }

        private void DrawZonalMap(DataSet dsMap)
        {
            try
            {
                m_wcZonal.DieClear();

                if (dsMap.Tables.IndexOf("MAPDATA") < 0 || dsMap.Tables["MAPDATA"] == null)
                    return;

                if (TESTAREA == "AVI")
                    m_wcZonal.SetGoodBin = "0";
                else
                    m_wcZonal.SetGoodBin = "1";

                m_wcZonal.SetWaferRecipe(oWaferRecipe);
                m_wcZonal.AngleOffSet = (360 - oWaferRecipe.ANGLE) % 360;
                m_wcZonal.DataSource = dsMap.Tables["MAPDATA"].Copy();
                m_wcZonal.Width = m_wcZonal.Width + 1;
                m_wcZonal.DrawYield();
                m_wcZonal.Width = m_wcZonal.Width + 1;
            }
            catch (Exception)
            {
                m_wcZonal.DieClear();
                //throw ex;
            }
            finally
            {

            }
        }


        private void AVIImageGetList(DataTable dtList)
        {
            DataTable dtTemp = null;
            //AVI 의 경우 Image 를 미리 Download 받는다.
            if (dtAVIImage != null)
                dtAVIImage.Dispose();
            dtAVIImage = null;

            if (TESTAREA == "AVI" && dtList.Select("IMAGE_PATH <> ''").Length > 0)
            {
                //Image 를 가져오는데 문제가 생기더라도 Map 을 보여주는데는 이상이 없어야 한다.
                try
                {
                    //DACrux.TEST.Control.AVIImage oAVI = new TEST.Control.AVIImage();
                    dtTemp = dtList.Select("IMAGE_PATH <> ''").CopyToDataTable<DataRow>();
                    dtAVIImage = AVIFTPFileDownload(dtTemp, this.Name);
                }
                catch (Exception) { }
            }
        }

        private DataTable AVIFTPFileDownload(DataTable FileList, string ModuleName)
        {
            DACrux.Utility.HFtpClient oFTP = null;
            //DACrux.Utility.ServerCommunicationFtp oFTP = null;
            string strLocalFullPath = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;
            //string strFTPMainPath = string.Empty;

            FileInfo oImageFile = null;
            DirectoryInfo oDirectory = null;

            DataTable dtResult = null;

            //DACrux.TEST.RO.DataSelect oData = null;
            ComConfiguration obj = null;

            try
            {
                if (FileList == null || FileList.Rows.Count <= 0)
                    return null;

                StatusMessage("AVI Image를 Download 받는 중입니다..");
                dtResult = new DataTable();
                dtResult.Columns.Add(new DataColumn("X", typeof(int)));
                dtResult.Columns.Add(new DataColumn("Y", typeof(int)));
                dtResult.Columns.Add(new DataColumn("BIN", typeof(int)));
                dtResult.Columns.Add(new DataColumn("IMAGE_PATH", typeof(string)));
                dtResult.Columns.Add(new DataColumn("THUMB_PATH", typeof(string)));
                dtResult.AcceptChanges();

                //oData = new RO.DataSelect();
                obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIImageFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return null;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();
                //strFTPMainPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();

                //Local Directory 삭제 후 생성 (쓰레기 Data 는 미리 삭제..)
                strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "AVI", ModuleName);
                oDirectory = new DirectoryInfo(strLocalPathName);
                if (oDirectory.Exists)
                    oDirectory.Delete(true);

                oDirectory.Create();

                oFTP = new DACrux.Utility.HFtpClient(strFTPIP, DACrux.Base.Convert.intParse(strFTPPort), strFTPID, strFTPPass);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");

                //oFTP = new Utility.ServerCommunicationFtp();
                //oFTP.Server = strFTPIP;
                //oFTP.Port = DACrux.Base.Convert.intParse(strFTPPort);
                //oFTP.UserID = strFTPID;
                //oFTP.Password = strFTPPass;
                //oFTP.ChmodValue = 777;

                //Image 및 Thumb File 을 Download 받는다.
                string strFileName = string.Empty;
                string strFilePath = string.Empty;
                for (int f = 0; f < FileList.Rows.Count; f++)
                {
                    StatusMessage(string.Format("AVI Image를 Download 받는 중입니다/ ({0}/{1})", f, FileList.Rows.Count));
                    DataRow drNew = dtResult.NewRow();
                    drNew["X"] = FileList.Rows[f]["X"];
                    drNew["Y"] = FileList.Rows[f]["Y"];
                    drNew["BIN"] = FileList.Rows[f]["BIN"];

                    //File Name 가져 오는 부분
                    strFileName = FileList.Rows[f]["IMAGE_FILE_NAME"].ToString();
                    strFilePath = FileList.Rows[f]["IMAGE_PATH"].ToString();
                    strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);

                    if (strFilePath.StartsWith("BACKUP") == true)
                        strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                    else
                        strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                    //oFTP.ReceiveFile(strFTPFullPath, strLocalFullPath);
                    oFTP.Down(strFTPFullPath, strLocalFullPath);
                    oImageFile = new FileInfo(strLocalFullPath);
                    if (!oImageFile.Exists)
                        throw new Exception("AVI Image File을 Download 하였으나 존재 하지 않습니다.");

                    drNew["IMAGE_PATH"] = strLocalFullPath;

                    //Thumb File Name 가져 오는 부분
                    //if (string.IsNullOrEmpty(FileList.Rows[f]["THUMB_FILENAME"].ToString()) == false)
                    //{
                    //    strFileName = FileList.Rows[f]["THUMB_FILENAME"].ToString();
                    //    strFilePath = FileList.Rows[f]["THUMB_PATH"].ToString();
                    //    strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);
                    //    strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                    //    //oFTP.ReceiveFile(strFTPFullPath, strLocalFullPath); 
                    //    oFTP.Down(strFTPFullPath, strLocalFullPath);
                    //    oImageFile = new FileInfo(strLocalFullPath);
                    //    if (!oImageFile.Exists)
                    //        throw new Exception("AVI Image File을 Download 하였으나 존재 하지 않습니다.");

                    //    drNew["THUMB_PATH"] = strLocalFullPath;
                    //}

                    dtResult.Rows.Add(drNew);
                }
                dtResult.AcceptChanges();
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                StatusMessage(null);
            }
        }


        private void DrawChart(DataSet dsMap)
        {
            try
            {
                // Chart Data는 이미 정렬되어 들어와야 한다.
                // Control내에서는 데이터 정렬을 하지 않는다.
                DACrux.Utility.Component.InitMSChart(ref BinTrendChart);
                chkGoodBin.Checked = true;

                if (dsMap.Tables.IndexOf("BINSUM") < 0 || dsMap.Tables["BINSUM"] == null)
                    return;

                DataTable dtBinSum = dsMap.Tables["BINSUM"];

                string sXField = string.Empty;
                if (m_wMap.DisplayDieValue == DACrux.Base.DieDisplayValue.BinChar)
                {
                    sXField = "BIN_CHAR";
                }
                else
                {
                    sXField = "BIN";
                    m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
                }

                BinTrendChart.ChartAreas[0].CursorX.Interval = 1;
                //BinTrendChart.ChartAreas[0].CursorX.IsUserEnabled = true;
                BinTrendChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                BinTrendChart.ChartAreas[0].AxisX.Maximum = dtBinSum.Rows.Count;
                BinTrendChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                BinTrendChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

                BinTrendChart.Legends["Default"].Docking = m_LegendsDockPos;
                BinTrendChart.Legends["Default"].IsTextAutoFit = true;
                BinTrendChart.Legends["Default"].Name = "Default";
                BinTrendChart.Legends["Default"].TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Auto;
                BinTrendChart.Legends["Default"].Enabled = m_bShowLegends;

                BinTrendChart.Series.Add("Count");
                BinTrendChart.Series["Count"].IsValueShownAsLabel = true;
                BinTrendChart.Series["Count"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                //BinTrendChart.Series["Count"].YAxisType = AxisType.Primary;
                //BinTrendChart.Series["Count"].IsXValueIndexed = true;
                //BinTrendChart.Series["Count"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                //BinTrendChart.Series["Count"].MarkerSize = 1;
                //BinTrendChart.Series["Count"].BorderWidth = 2;
                //BinTrendChart.Series["Count"].SmartLabelStyle.Enabled = true;
                //BinTrendChart.Series["Count"].Points.DataBind(dtBinSum.Rows, sXField, "CNT", string.Format("AxisLabel={0}", sXField));
                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    BinTrendChart.Series["Count"].Points.AddXY(dtBinSum.Rows[i][sXField].ToString(), dtBinSum.Rows[i]["CNT"].ToString());

                    int iBin = int.Parse(dtBinSum.Rows[i][sXField].ToString());

                    int iIndex = m_lstBinColor.FindIndex(x => x.bin == iBin);
                    if (iIndex > -1)
                    {
                        BinTrendChart.Series["Count"].Points[BinTrendChart.Series["Count"].Points.Count - 1].Color = m_lstBinColor[iIndex].color;
                    }
                }

                //for (int i = 0; i < BinTrendChart.Series["Count"].Points.Count; i++)
                //    BinTrendChart.Series["Count"].Points[i].Color = m_lstBinColor[i].color;

                if (m_bShowDataGrid == true)
                {
                    DACrux.TEST.Control.ChartDataTableHelper dataSheet = new DACrux.TEST.Control.ChartDataTableHelper();
                    dataSheet.Initialize(BinTrendChart, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetWaferMapInfo(DataSet dsMap)
        {
            try
            {
                // 체크박스를 달아준다.
                chkboxSelBin.Items.Clear();
                chkboxSelBin.Items.Add("ALL");

                if (dsMap.Tables.IndexOf("BINSUM") < 0 || dsMap.Tables["BINSUM"] == null)
                    return;

                //셀렉트빈은 초기값 'ALL'
                string[] strOldSelBin = m_wMap.SelecetedBin.Split(',');
                string sDisplayCol = string.Empty;

                //초기 설정
                if (Array.IndexOf(strOldSelBin, "ALL") > -1) chkboxSelBin.SetItemChecked(0, true);

                DataTable dtBinSum = dsMap.Tables["BINSUM"];

                //BIN정보를 별도로 저장한다. 0 번쨰는 ALL
                string[] strBinList = new string[dtBinSum.Rows.Count + 1];
                strBinList[0] = "ALL";

                double dbTotal = Convert.ToDouble(dtBinSum.Compute("SUM([CNT])", "1=1"));

                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN"].ToString()) || dtBinSum.Rows[i]["BIN"].ToString() == "-1")
                        sDisplayCol = "BIN_CHAR";
                    else if (m_wMap.DisplayDieValue == DACrux.Base.DieDisplayValue.BinChar)
                        sDisplayCol = "BIN_CHAR";
                    else
                        sDisplayCol = "BIN";

                    string sBinName = dtBinSum.Rows[i][sDisplayCol].ToString().PadLeft(3, ' ');
                    //  string.Format '-' 에서 '/'로 변경합니다.
                    //if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN_NAME"].ToString()) == false)
                    //    sBinName = string.Format("{0} / {1}", sBinName, dtBinSum.Rows[i]["BIN_NAME"].ToString());

                    //if (sDisplayCol == "BIN_CHAR")
                    //    sBinName += "\t[BIN CHAR]";

                    chkboxSelBin.Items.Add(string.Format("{0} / {1} ( {2} , {3}% )", sBinName
                                                                , dtBinSum.Rows[i]["BIN_NAME"].ToString().PadRight(10, ' ')
                                                                , dtBinSum.Rows[i]["CNT"].ToString().PadLeft(5, ' ')
                                                                , Math.Round(DACrux.Base.Convert.doubleParse(dtBinSum.Rows[i]["CNT"].ToString()) / dbTotal * 100, 1).ToString().PadLeft(3, ' ')));


                    //chkboxSelBin.Items.Add(sBinName);
                    strBinList[i + 1] = dtBinSum.Rows[i]["BIN"].ToString();

                    if (Array.IndexOf(strOldSelBin, dtBinSum.Rows[i][sDisplayCol].ToString()) > -1) chkboxSelBin.SetItemChecked(i + 1, true);
                }

                //Check 박스 테그에 담아준다.
                chkboxSelBin.Tag = string.Join(",", strBinList);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pnlWaferMapInfo.Refresh();
            }
        }

        private void SetParaItem(DataSet dsMap)
        {
            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string[] strFiler = null;
            string strTableName = string.Empty;
            try
            {
                if (chkTotalPara.Checked == true)
                    strTableName = "PARA_ITEM";
                else
                    strTableName = "PARA_LIST";

                if (dsMap.Tables.IndexOf(strTableName) < 0 || dsMap.Tables[strTableName] == null)
                    return;

                dtTemp = dsMap.Tables[strTableName].Copy();
                dtTempFilter = dtTemp.Clone().Copy();
                dtTemp.CaseSensitive = false;

                lsParaList.Items.Clear();
                if (string.IsNullOrEmpty(TxtItemFilter.Text) == false)
                {
                    strFiler = TxtItemFilter.Text.Replace(",", ";").Replace("*", "%").Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string strVal in strFiler)
                    {
                        DataRow[] dr = dtTemp.Select(string.Format("PARAM_NAME LIKE '{0}'", strVal));
                        if (dr.Length > 0)
                        {
                            foreach (DataRow drFilter in dr)
                            {
                                dtTempFilter.Rows.Add(drFilter.ItemArray);
                            }
                        }
                    }

                    dtTemp = dtTempFilter.Copy();
                }

                string strItemName = string.Empty;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    strItemName = dtTemp.Rows[i]["PARAM_NAME"].ToString();

                    //if (strItemName != "X" && strItemName != "Y" && strItemName != "WAFER_SEQ" && strItemName != "BIN")
                    //    lsParaList.Items.Add(strItemName);
                    if (!String.Equals(strItemName, "WAFER_SEQ") && !String.Equals(strItemName, "BIN"))
                        lsParaList.Items.Add(strItemName);
                }

                clbParaCut.Items.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetMapRawData(DataSet dsMap)
        {
            DataTable dt = null;
            try
            {
                // DACrux.Utility.Component.InitSpread(fpRawData);

                fpRawData.ActiveSheet.Rows.Clear();

                if (dsMap.Tables.IndexOf("MAPDATA") < 0 || dsMap.Tables["MAPDATA"] == null)
                    return;

                dt = dsMap.Tables["MAPDATA"];

                Utility.FPSpreadUtil.SetSpreadData(dt, fpRawData_Sheet1, 120, 0);
                Utility.FPSpreadUtil.VisibleSpreadColumns(fpRawData_Sheet1, new string[] { "WAFER_SEQ" }, false);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetBinChange(DataSet dsMap)
        {
            DataTable dt = null;
            try
            {
                DACrux.Utility.Component.InitSpread(fpBinChange);

                if (dsMap.Tables.IndexOf("BIN_CHANGE") < 0 || dsMap.Tables["BIN_CHANGE"] == null)
                    return;

                dt = dsMap.Tables["BIN_CHANGE"];

                if (dt.Columns.IndexOf("WAFER_SEQ") > -1)
                {
                    dt.Columns.Remove("WAFER_SEQ");
                }

                DACrux.Utility.Component.SetSpreadData(dt, fpBinChange_Sheet1, 100);
                fpBinChange_Sheet1.RowHeader.Columns[0].Width += 10;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetLowYieldData(DataSet dsMap)
        {
            try
            {
                DACrux.Utility.Component.InitSpread(fpAbnData);

                if (dsMap.Tables.IndexOf("LOW_YIELD") < 0 || dsMap.Tables["LOW_YIELD"] == null)
                    return;

                DACrux.Utility.Component.SetSpreadData(dsMap.Tables["LOW_YIELD"], fpAbnData_Sheet1, 100);
                fpAbnData_Sheet1.RowHeader.Columns[0].Width += 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetMapBinColor(DataSet dsMap)
        {
            try
            {
                if (dsMap.Tables.IndexOf("BINSUM") < 0 || dsMap.Tables["BINSUM"] == null)
                    return;

                m_lstBinColor = new List<BinColor>();
                DataTable dtBinSum = dsMap.Tables["BINSUM"];

                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    int nBin = -1;

                    if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                    {
                        if (string.IsNullOrEmpty(dtBinSum.Rows[i]["COLOR"].ToString().Trim()) == false && dtBinSum.Rows[i]["COLOR"].ToString() != "#FFFFFF")
                        {
                            if (nBin > -1)
                            {
                                m_wMap.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString()));
                                m_wcZonal.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString()));
                            }
                            m_lstBinColor.Add(new BinColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString())));
                        }
                        else
                        {
                            if (nBin > -1)
                            {
                                m_wMap.SetColor(nBin, DACrux.TEST.Control.Util.GetColor(nBin));
                            }

                            m_lstBinColor.Add(new BinColor(nBin, DACrux.TEST.Control.Util.GetColor(nBin)));
                        }
                    }
                    else
                        m_lstBinColor.Add(new BinColor(nBin, DACrux.TEST.Control.Util.GetColor(nBin)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void BtnParaAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                //btnTestDataDraw_Click(null, null);
                btnTestDataDraw.PerformClick();

                if (lsParaList.Items.Count <= 0 || lsParaList.SelectedItems.Count <= 0)
                    return;

                if (m_dsMap != null)
                {
                    if (OnParaAnalysis != null)
                        OnParaAnalysis(m_dsMap, lsParaList.SelectedItems[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtItemFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    SetParaItem(m_dsMap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clbParaCut_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (e.Index == 0)
                {
                    for (int i = 1; i < clbParaCut.Items.Count; i++)
                        clbParaCut.SetItemChecked(i, false);
                }
                else
                {
                    clbParaCut.SetItemChecked(0, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChkValue_CheckedChanged(object sender, EventArgs e)
        {
            //btnTestDataDraw_Click(null, null);
            btnTestDataDraw.PerformClick();
        }

        private void cbCutCnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsParaList_SelectedIndexChanged(null, null);
        }

        private void chkTotalPara_CheckedChanged(object sender, EventArgs e)
        {
            SetParaItem(m_dsMap);
        }

        private void AVIImageList_MouseEnter(object sender, EventArgs e)
        {
            AVIImageList.Focus();
        }

        private void AVIImageList_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                AVIImageList.SuspendLayout();
                foreach (System.Windows.Forms.Control ctrl in AVIImageList.Controls)
                {

                    ctrl.Height = ctrl.Height + (ctrl.Margin.Top + ctrl.Margin.Bottom);
                    ctrl.Width = AVIImageList.Width - 30;
                }
                AVIImageList.ResumeLayout();
            }
            catch (Exception) { }
        }

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(m_wMap);
            sheet.Add();
            sheet.Add(pnlChart);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpRawData.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            sheet = new DACrux.Utility.ExcelSheet();
            List<System.Windows.Forms.Control> lsImages = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control cr in AVIImageList.Controls)
            {
                if (cr.Name == "TPUImageInfo")
                    lsImages.Add(cr);
            }

            sheet.Add();
            if (lsImages.Count > 0)
                sheet.Add(lsImages.ToArray());

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "AVI Image";

            //DataGridView WaferMapGrid = m_wMap.ToDataGridView();
            //if (WaferMapGrid != null)
            //{
            //    sheet = new DACrux.Utility.ExcelSheet();
            //    sheet.Add(WaferMapGrid);
            //    e.SheetList.Add(sheet);
            //    e.SheetList[e.SheetList.Count - 1].SheetName = "GridMap";
            //}

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion

        private void aVIImageAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("AVI 전체 Image 를 가져오겠습니까?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                fnAVIMapGathering(null);
                //try
                //{
                //    fnAVIMapGathering(null);
                //    if (dtAVIImage == null || dtAVIImage.Rows.Count <= 0)
                //    {
                //        MessageBox.Show("AVI Image 정보가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }

                //    if (AVIImageList.Controls.Count > 0)
                //        AVIImageList.Controls.Clear();

                //    DataRow[] drAVI = dtAVIImage.Select("1 = 1");
                //    foreach (DataRow dr in drAVI)
                //    {
                //        DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                //            dr["IMAGE_PATH"].ToString(),
                //            dr["X"].ToString(),
                //            dr["Y"].ToString(),
                //            dr["BIN"].ToString(),
                //            LOT_ID,
                //            PROGRAM,
                //            DEVICE);
                //        ControlAdd(oForm);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }

        }

        private void chkSpecific_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpecific.Checked)
                chkTightenLimit.Checked = chkControl.Checked = !chkSpecific.Checked;

            btnTestDataDraw.PerformClick();
        }

        private void chkControl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkControl.Checked)
                chkTightenLimit.Checked = chkSpecific.Checked = !chkControl.Checked;

            btnTestDataDraw.PerformClick();
        }

        private void chkTightenLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTightenLimit.Checked)
                chkSpecific.Checked = chkControl.Checked = !chkTightenLimit.Checked;
            btnTestDataDraw.PerformClick();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkSpecific.Checked = false;
            chkControl.Checked = false;
            chkTightenLimit.Checked = false;
        }

        private void m_wMap_OnSelectDies(object sender)
        {

        }

        private void chkZoneMap_Click(object sender, EventArgs e)
        {
            ultraDockManager1.ControlPanes["ZL"].Closed = !chkZoneMap.Checked;
        }

    }
}
