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

namespace DACrux.MapAnalysis.Control
{
    /// <summary>
    /// TPUCMapDefine 에 대한 요약 설명입니다.
    /// </summary>
    public partial class TPUCMapDefine : System.Windows.Forms.UserControl
    {
        #region [ Data Field ]

        public delegate void WaferReflash(DACrux.Base.TPWafer[] oWafer);
        public event WaferReflash OnWaferReflash;

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
        private bool m_bShowUserInformation = false;
        private string[] m_sUserInformation = null;
        private int m_nDisplayFaltAngle = -1;
        private string m_sZonalGoodBins = "1";
        private string strParaItem = string.Empty;

        public List<BinColor> mBinColor
        {
            get { return m_lstBinColor; }
        }

        public DACrux.Base.TPWafer[] mWaferInfo;

        [Category("Option"), DefaultValue(true)]
        public bool ShowCellInfo
        {
            set { pnlInfo.Visible = value; }
            get { return pnlInfo.Visible; }
        }

        #region [ Bin Chart Option ]

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BinChart_ShowDataGrid
        {
            set { m_bShowDataGrid = value; }
            get { return m_bShowDataGrid; }
        }


        #endregion

        #region [ Wafer Map Option ]

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

        public TPUCMapDefine()
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

        private void m_wMap_OnChangeCurrentDie(object sender, DACrux.Base.Die NewDie)
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
                m_wMap.DataSource = null;
                m_wMap.DieClear();
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConfigClear()
        {
            try
            {
                TxtChangeXIndex.Text = "0";
                TxtChangeYIndex.Text = "0";
                TxtChangeFlatZone.Text = "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public void Draw(int nAngle = -1)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (m_dsMap == null)
                    throw new Exception("Map Data Empty.");

                TxtChangeXIndex.Text = "0";
                TxtChangeYIndex.Text = "0";
                TxtChangeFlatZone.Text = "0";
                TxtSetProgram.Text = "";
                chkProgram.Checked = false;
                TxtAlterXIndex.Text = "0";
                TxtAlterYIndex.Text = "0";
                TxtAlterAngle.Text = "0";
                TxtComment.Text = "";
                TxtLastUpdateUser.Text = "";
                TxtLastUpdateTime.Text = "";
                TxtCreateUser.Text = "";
                TxtCreateTime.Text = "";
                TxtConfigSeq.Text = "";

                if (m_dsMap.Tables.IndexOf("WAFER_INFO") > -1)
                {
                    if (m_dsMap.Tables["WAFER_INFO"].Columns.IndexOf("DEVICE_ALIAS") > -1)
                        TxtdeviceId.Text = m_dsMap.Tables["WAFER_INFO"].Rows[0]["DEVICE_ALIAS"].ToString();
                    if (m_dsMap.Tables["WAFER_INFO"].Columns.IndexOf("TESTAREA") > -1)
                        TxtAreaId.Text = m_dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString();
                    if (m_dsMap.Tables["WAFER_INFO"].Columns.IndexOf("PROGRAM") > -1)
                        TxtProgram.Text = m_dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString();
                }

                if (TxtAreaId.Text != "MULTIPROBE")
                {
                    MessageBox.Show("MULTIPROBE 외의 AREA 는 사용할 수 없습니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (m_dsMap.Tables.IndexOf("MAPCFG") > -1)
                {
                    TxtConfigSeq.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["MAP_CFG_SEQ"].ToString();

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("ALTER_INDEX_X") > -1)
                        TxtAlterXIndex.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["ALTER_INDEX_X"].ToString();
                    else
                        TxtAlterXIndex.Text = "0";

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("ALTER_INDEX_Y") > -1)
                        TxtAlterYIndex.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["ALTER_INDEX_Y"].ToString();
                    else
                        TxtAlterYIndex.Text = "0";

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("ALTER_ANGLE") > -1)
                        TxtAlterAngle.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["ALTER_ANGLE"].ToString();
                    else
                        TxtAlterAngle.Text = "0";

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("PROGRAM") > -1)
                    {
                        TxtSetProgram.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["PROGRAM"].ToString();
                        if (TxtSetProgram.Text == "ALL")
                            chkProgram.Checked = false;
                        else
                            chkProgram.Checked = true;
                    }

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("USER_COMMENT") > -1)
                        TxtComment.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["USER_COMMENT"].ToString();

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("UPDATE_USER") > -1)
                        TxtLastUpdateUser.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["UPDATE_USER"].ToString();

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("UPDATE_TIME") > -1)
                        TxtLastUpdateTime.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["UPDATE_TIME"].ToString();

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("CREATE_USER") > -1)
                        TxtCreateUser.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["CREATE_USER"].ToString();

                    if (m_dsMap.Tables["MAPCFG"].Columns.IndexOf("CREATE_TIME") > -1)
                        TxtCreateTime.Text = m_dsMap.Tables["MAPCFG"].Rows[0]["CREATE_TIME"].ToString();
                }

                if (MakeWaferRecipe(m_dsMap, ref oWaferRecipe) == true)
                {
                    SetMapBinColor(m_dsMap);
                    DrawWaferMap(m_dsMap, oWaferRecipe);
                }
                else
                {
                    WaferClear();
                }
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

        public void ControlDraw()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (m_dsMap == null)
                    throw new Exception("Map Data Empty.");

                if (MakeWaferRecipe(m_dsMap, ref oWaferRecipe) == true)
                {
                    SetMapBinColor(m_dsMap);
                    DrawWaferMap(m_dsMap, oWaferRecipe);
                }
                else
                {
                    WaferClear();
                }
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

        private bool MakeWaferRecipe(DataSet dsMap, ref DACrux.Base.WaferRecipe oWaferRecipe)
        {
            DataTable dtWaferInfo = null;
            DataTable dtMapData = null;

            double m_dMargin = 0.95D;

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

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["XY_DIRECTION"].ToString(), out oWaferRecipe.XYDIR) == false)
                        oWaferRecipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;

                    oWaferRecipe.WAFER_SIZE = DACrux.Base.Util.GetValue(dtWaferInfo.Rows[0]["WAFER_SIZE"].ToString(), 200000);

                    if (double.TryParse(dtWaferInfo.Rows[0]["EDGE_SIZE"].ToString(), out oWaferRecipe.EDGE_SIZE) == false)
                        oWaferRecipe.EDGE_SIZE = 3d;

                    //Mark Die 때문에 Standard 는 Bottom 이다.
                    oWaferRecipe.ANGLE = 0;
                    //if (nAngle != -1)
                    //    oWaferRecipe.ANGLE = nAngle;

                    m_nDisplayFaltAngle = oWaferRecipe.ANGLE;

                    //화면상에 보여 줄때 Bottom 으로 저장이 되어 있기 때문에 Rotation 각을 보고 Max 값을 치환 해준다.
                    //nRotationAngle = (360 - (180 - oWaferRecipe.ANGLE)) % 360;

                    //if (nRotationAngle == 90 || nRotationAngle == 270)
                    //{
                    //    int iTempMax = 0;
                    //    iTempMax = oWaferRecipe.DIE_INDEX_MAX_X;

                    //    oWaferRecipe.DIE_INDEX_MAX_X = oWaferRecipe.DIE_INDEX_MAX_Y;
                    //    oWaferRecipe.DIE_INDEX_MAX_Y = iTempMax;

                    //}

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

                TxtPitchX.Text = oWaferRecipe.DIE_SIZE_X.ToString();
                TxtPitchY.Text = oWaferRecipe.DIE_SIZE_Y.ToString();
                TxtXMin.Text = oWaferRecipe.DIE_INDEX_MIN_X.ToString();
                TxtYMin.Text = oWaferRecipe.DIE_INDEX_MIN_Y.ToString();
                TxtXMax.Text = oWaferRecipe.DIE_INDEX_MAX_X.ToString();
                TxtYMax.Text = oWaferRecipe.DIE_INDEX_MAX_Y.ToString();
                TxtNetDie.Text = oWaferRecipe.NETDIE.ToString();
                TxtWaferSize.Text = oWaferRecipe.WAFER_SIZE.ToString();
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
            DataTable dtWafer = null;

            int iOriX = 0;
            int iOriY = 0;
            int iOriAngle = 0;
            int iAlterX = 0;
            int iAlterY = 0;
            int iAlterAngle = 0;

            int iX = 0;
            int iY = 0;

            int iXMax = 0;
            int iXMin = 0;
            int iYMax = 0;
            int iYMin = 0;

            int iTemp= 0;

            try
            {
                if (int.TryParse(TxtChangeXIndex.Text, out iAlterX) == false)
                    iAlterX = 0;

                if (int.TryParse(TxtChangeYIndex.Text, out iAlterY) == false)
                    iAlterY = 0;

                if (int.TryParse(TxtChangeFlatZone.Text, out iAlterAngle) == false)
                    iAlterAngle = 0;

                m_wMap.ResetSelectedDie();
                m_wMap.DieClear();

                if (dsMap.Tables.IndexOf("MAPDATA") < 0 || dsMap.Tables["MAPDATA"] == null)
                    return;

                iXMax = System.Convert.ToInt32(dsMap.Tables["MAPDATA"].Compute("MAX(X)", ""));
                iXMin = System.Convert.ToInt32(dsMap.Tables["MAPDATA"].Compute("MIN(X)", ""));
                iYMax = System.Convert.ToInt32(dsMap.Tables["MAPDATA"].Compute("MAX(Y)", ""));
                iYMin = System.Convert.ToInt32(dsMap.Tables["MAPDATA"].Compute("MIN(Y)", ""));

                dtWafer = dsMap.Tables["MAPDATA"].Clone();

                foreach (DataRow dr in dsMap.Tables["MAPDATA"].Rows)
                {
                    DataRow drTemp = dtWafer.NewRow();
                    drTemp.ItemArray = dr.ItemArray;

                    //변경 시키는 Map 관련 정보 기반 좌표 이동
                    if (int.TryParse(drTemp["X"].ToString(), out iOriX) == false)
                        continue;

                    if (int.TryParse(drTemp["Y"].ToString(), out iOriY) == false)
                        continue;

                    iOriX = iOriX + iAlterX;
                    iOriY = iOriY + iAlterY;

                    iAlterAngle = (360 - (iOriAngle - iAlterAngle)) % 360;

                    switch (iAlterAngle)
                    {
                        case 0:
                            iX = iOriX;
                            iY = iOriY;
                            break;
                        case 90:
                            iX = iOriY;
                            iY = iXMax + iXMin - iOriX;
                            break;
                        case 180:
                            iX = iXMax + iXMin - iOriX;
                            iY = iYMax + iYMin - iOriY;
                            break;
                        case 270:
                             iX = (iYMax + iYMin) - iOriY;
                            iY = iOriX;
                            break;
                        default:
                            iX = iOriX;
                            iY = iOriY;
                            break;
                    }


                    //switch (iAlterAngle)
                    //{
                    //    case 0:
                    //        iX = iOriX;
                    //        iY = iOriY;
                    //        break;
                    //    case 90:
                    //        iX = iOriY;
                    //        iY = oWaferRecipe.DIE_INDEX_MAX_X + oWaferRecipe.DIE_INDEX_MIN_X - iOriX;
                    //        break;
                    //    case 180:
                    //        iX = oWaferRecipe.DIE_INDEX_MAX_X + oWaferRecipe.DIE_INDEX_MIN_X - iOriX;
                    //        iY = oWaferRecipe.DIE_INDEX_MAX_Y + oWaferRecipe.DIE_INDEX_MIN_Y - iOriY;
                    //        break;
                    //    case 270:
                    //        iX = (oWaferRecipe.DIE_INDEX_MAX_Y + oWaferRecipe.DIE_INDEX_MIN_Y) - iOriY;
                    //        iY = iOriX;
                    //        break;
                    //    default:
                    //        iX = iOriX;
                    //        iY = iOriY;
                    //        break;
                    //}


                    drTemp["X"] = iX;
                    drTemp["Y"] = iY;

                    dtWafer.Rows.Add(drTemp);
                }

                dtWafer.AcceptChanges();

                m_wMap.SetWaferRecipe(oWaferRecipe);
                m_wMap.DataSource = dtWafer;

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
                    foreach(DataRow dr in dsMap.Tables["MARKDATA"].Rows)
                    {

                        if(int.TryParse(dr["INDEX_X"].ToString(), out iX) == false)
                            continue;
                        
                        if(int.TryParse(dr["INDEX_Y"].ToString(), out iY) == false)
                            continue;

                        if(m_wMap.IndexOf(iX, iY) < 0)
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

                //if (m_nDisplayFaltAngle != -1)
                //    m_wMap.ViewAngle = (360 - (oWaferRecipe.ANGLE - (m_nDisplayFaltAngle * 90))) % 360;

                //Mark Die 때문에 Standard 는 Bottom 이다.
                m_wMap.ViewAngle = 0;

                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();
            }
            catch (Exception ex)
            {
                m_wMap.DieClear();
                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
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
                            }

                            m_lstBinColor.Add(new BinColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString())));
                        }
                        else
                        {
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

        private void SaveMapConfigData()
        {
        }
        #endregion


        #region [ Event Handler ]

        private void BtnUp_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            switch (TxtChangeFlatZone.Text)
            {
                case "0":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case "90":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case "180":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case "270":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;

            }

            ControlDraw();
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            switch (TxtChangeFlatZone.Text)
            {
                case "0":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case "90":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case "180":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case "270":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
            }

            ControlDraw();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            switch (TxtChangeFlatZone.Text)
            {
                case "0":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case "90":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case "180":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case "270":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;

            }

            ControlDraw();
        }

        private void BtnRight_Click(object sender, EventArgs e)
        { 
            int iIndex = 0;
            switch (TxtChangeFlatZone.Text)
            {
                case "0":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case "90":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case "180":
                    if (int.TryParse(TxtChangeXIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case "270":
                    if (int.TryParse(TxtChangeYIndex.Text, out iIndex) == false)
                        iIndex = 0;

                    TxtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;

            }
            ControlDraw();
        }



        private void BtnLeftRotation_Click(object sender, EventArgs e)
        {
            switch (TxtChangeFlatZone.Text)
            {
                case "270":
                    TxtChangeFlatZone.Text = "0";
                    break;
                case "90":
                    TxtChangeFlatZone.Text = "180";
                    break;
                case "180":
                    TxtChangeFlatZone.Text = "270";
                    break;
                default: TxtChangeFlatZone.Text = "90";
                    break;
            }

            ControlDraw();
        }

        private void BtnRightRotation_Click(object sender, EventArgs e)
        {
            switch (TxtChangeFlatZone.Text)
            {
                case "90":
                    TxtChangeFlatZone.Text = "0";
                    break;
                case "270":
                    TxtChangeFlatZone.Text = "180";
                    break;
                case "180":
                    TxtChangeFlatZone.Text = "90";
                    break;
                default: TxtChangeFlatZone.Text = "270";
                    break;
            }

            ControlDraw();
        }


        private void BtnTL_Click(object sender, EventArgs e)
        {
            TxtSetDir.Text = "TL";
        }

        private void BtnTR_Click(object sender, EventArgs e)
        {
            TxtSetDir.Text = "TR";
        }

        private void BtnLL_Click(object sender, EventArgs e)
        {
            TxtSetDir.Text = "LL";
        }

        private void BtnLR_Click(object sender, EventArgs e)
        {
            TxtSetDir.Text = "LR";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("현재 정보로 해당 Device ({0}) 에 저장 하시겠습니까?", TxtdeviceId.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(TxtComment.Text) == true)
            {
                MessageBox.Show("Comment 에 입력 사항이 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtComment.Focus();
                return;
            }

            if (chkProgram.Checked == true && MessageBox.Show(string.Format("해당 Program {0} 으로 예외 처리됩니다. 계속 하시겠습니까?", TxtProgram.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            this.Cursor = Cursors.WaitCursor;

            int iOriX = 0;
            int iOriY = 0;
            int iOriAngle = 0;
            int iAlterX = 0;
            int iAlterY = 0;
            int iAlterAngle = 0;

            try
            {
                if (int.TryParse(TxtAlterXIndex.Text, out iOriX) == false)
                    iOriX = 0;

                if (int.TryParse(TxtAlterYIndex.Text, out iOriY) == false)
                    iOriY = 0;

                if (int.TryParse(TxtChangeXIndex.Text, out iAlterX) == false)
                    iAlterX = 0;

                if (int.TryParse(TxtChangeYIndex.Text, out iAlterY) == false)
                    iAlterY = 0;

                if (int.TryParse(TxtAlterAngle.Text, out iOriAngle) == false)
                    iOriAngle = 0;

                if (int.TryParse(TxtChangeFlatZone.Text, out iAlterAngle) == false)
                    iAlterAngle = 0;

                //iAlterAngle = (360 - (iOriAngle - iAlterAngle)) % 360;
                iAlterAngle = (iOriAngle + iAlterAngle) % 360;
                iAlterX = iOriX + iAlterX;
                iAlterY = iOriY + iAlterY;

                DACrux.TEST.RO.ProbeMapAnalysis oTest = new TEST.RO.ProbeMapAnalysis();
                oTest.UpdateMapConfig(TxtAreaId.Text, 
                                      TxtdeviceId.Text, 
                                      chkProgram.Checked == false ? "ALL" : TxtProgram.Text,
                                      string.IsNullOrEmpty(TxtSetDir.Text) ? "LL" : TxtSetDir.Text,
                                      iAlterAngle.ToString(),
                                      iAlterX.ToString(),
                                      iAlterY.ToString(),
                                      DACrux.Base.GlobalVariable.UserID, 
                                      TxtComment.Text);

                MessageBox.Show("저장 되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (OnWaferReflash != null) OnWaferReflash(mWaferInfo);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TxtSetProgram.Text) == true || string.IsNullOrEmpty(TxtConfigSeq.Text) == true)
            {
                MessageBox.Show("저장되어 있는 정보가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(TxtComment.Text) == true)
            {
                MessageBox.Show("Comment 에 입력 사항이 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtComment.Focus();
                return;
            }

            if (MessageBox.Show(string.Format("현재 정보를 삭제 하시겠습니까?\nArea:{0}, Device:{1}, PGM:{2}",TxtAreaId.Text, TxtdeviceId.Text, TxtSetProgram.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                DACrux.TEST.RO.ProbeMapAnalysis oTest = new TEST.RO.ProbeMapAnalysis();
                oTest.DeleteMapConfig(TxtConfigSeq.Text, DACrux.Base.GlobalVariable.UserID, TxtComment.Text);

                MessageBox.Show("삭제 되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (OnWaferReflash != null) OnWaferReflash(mWaferInfo);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnResetMap_Click(object sender, EventArgs e)
        {
            ConfigClear();
            Draw();

        }

        private void PicWafer_Click(object sender, EventArgs e)
        {
            BtnResetMap_Click(null, null);
        }


        #endregion

        private void btnView_Click(object sender, EventArgs e)
        {

        }

        private void ducListBox1_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dlbProduct_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dlbProgram_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dlbRevision_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }



       


    }
}
