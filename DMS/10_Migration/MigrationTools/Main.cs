using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;

namespace MigrationTools
{
    public partial class TxtWaferNo : Form
    {
        System.Threading.Thread trMigrationUpload = null;
        private readonly string m_ESDA_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.147.13.13)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=anamdb)));User ID=T_ESDA;Password=T_ESDA;";
        //--
        private System.Data.OracleClient.OracleConnection connSourceDb = null;
        private const int RK = 1;

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        public TxtWaferNo(
            )
        {
            InitializeComponent();
            dtFrom.Value = DateTime.Now.AddMonths(-1);
            dtTo.Value = DateTime.Now;
            dtFrom.Value = new DateTime(2019, 01, 01);
            dtTo.Value = new DateTime(2019, 01, 15);
        }

        private void btnExecute_Click(
            object sender,
            EventArgs e
            )
        {
            try
            {

                if (MessageBox.Show(string.Format("Data Migration 을 진행 하시겠습니까?"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    return;

                TxtLog.Clear();
                TxtErrorLog.Clear();

                //MigrationUpload();
                trMigrationUpload = new System.Threading.Thread(new System.Threading.ThreadStart(MigrationUpload));
                trMigrationUpload.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGetData_Click(object sender, EventArgs e)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable dt = new DataTable();
            try
            {
                lbCount.Text = "Get Row Count :  0";
                UpdateProcess("Processsing : 0 / 0");
                UpdateFailCount("Fail Count : 0");
               

                sQuery.AppendLine("SELECT TO_CHAR(ITIME, 'MMDDYYHH24MISS') AS TRAN_TIME, TO_CHAR(ITIME, 'YYYYMMDDHH24MISS') AS SYS_TIME, A.*, B.*, C.*  FROM  ");
                sQuery.AppendLine("T_ESDA_ILEVEL A, T_ESDA_IWAFER B, T_ESDA_ILOT C  WHERE 1 = 1 ");
                sQuery.AppendLine("AND A.WAFER_SEQ = B.WAFER_SEQ ");
                sQuery.AppendLine("AND B.LOT_SEQ = C.LOT_SEQ ");
                sQuery.AppendLine(string.Format("AND A.ITIME BETWEEN TO_DATE('{0}', 'YYYY-MM-DD') AND TO_DATE('{1}', 'YYYY-MM-DD') ", dtFrom.Text, dtTo.Text));
                //sQuery.AppendLine("AND C.LOT_NUMBER in( '9034400')");
               // sQuery.AppendLine("AND A.SLOTID in ( 24)");
                //sQuery.AppendLine("ORDER BY C.LOT_NUMBER");
                //sQuery.AppendLine("and A.LEVEL_SEQ = 10161802");

                if(!string.IsNullOrEmpty(TxtLotID.Text.Trim()))
                    sQuery.AppendLine(string.Format("AND C.LOT_NUMBER = '{0}' ", TxtLotID.Text.Trim()));

                if (!string.IsNullOrEmpty(TxtWafer.Text.Trim())) 
                    sQuery.AppendLine(string.Format("AND A.SLOTID = {0} ", TxtWafer.Text.Trim()));

                sQuery.AppendLine("ORDER BY A.ITIME");

                connSourceDb = new System.Data.OracleClient.OracleConnection();
                connSourceDb.ConnectionString = m_ESDA_ConnectionString;
                connSourceDb.Open();

                using (System.Data.OracleClient.OracleCommand oCommand = connSourceDb.CreateCommand())
                {
                    oCommand.CommandText = sQuery.ToString();
                    oCommand.CommandTimeout = 60;
                    oCommand.CommandType = CommandType.Text;

                    System.Data.OracleClient.OracleDataAdapter oAdapter = new System.Data.OracleClient.OracleDataAdapter(oCommand);
                    oAdapter.Fill(dt);
                    oAdapter.Dispose();
                    oCommand.Dispose();
                }

                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);
                //DACrux.Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1, 100, 0, false);
                fpSpread1.DataSource = dt;
                fpSpread1_Sheet1.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;

                if (dt != null)
                    lbCount.Text = string.Format("Get Row Count : {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connSourceDb != null)
                {
                    connSourceDb.Close();
                    connSourceDb.Dispose();
                    connSourceDb = null;
                }

            }

        }

        /// <summary>
        /// Migration Main 
        /// </summary>
        private void MigrationUpload()
        {
            DataTable dt = null;
            DataTable dtTemp = null;

            string strError = string.Empty;
            string strSetupID = string.Empty;

            MigrationTools.ESDA.ESDA oEsda = null;

            MapInfo.MapInfo oMapinfo;
            MapInfo.Map oMap;
            //MapInfo.MAP_SETUP oMapSetup;
            MapInfo.DEFECT_INFO oDefect;

            DACrux.SEMDMS.RO.DefectDefine oDefectDefine = null;

            DateTime oCurrDt = DateTime.Now;
            DateTime oTranDt = DateTime.Now;
            DateTime oStartDt = DateTime.Now;
            DateTime oEndDt = DateTime.Now;

            string strNowTime = oCurrDt.ToString("yyyyMMddHHmmss");
            string[] strDBData = null;

            long SetupSeq = -1;
            long LotSeq = - 1;
            long WaferSeq = -1;
            long StepSeq = -1;

            decimal dSampleX = 0;
            decimal dSampleY = 0;

            decimal dOrignX = 0;
            decimal dOrignY = 0;

            decimal dPITCH_X = 0;
            decimal dPITCH_Y = 0;

            int dOrignXIndex = 0;
            int dOrignYIndex = 0;

            int iFailCount = 0;
            int iImageCount = 0;
            int iClassifiedCnt = 0;

            string strTranTime = string.Empty; 
            string strDevice = string.Empty;
            string strLevelName = string.Empty;
            string strLotID = string.Empty;
            string strWaferID= string.Empty;
            string strStepID = string.Empty;
            string strSlotID = string.Empty;
            string strMachine = string.Empty;
            string strArea = string.Empty;

            try
            {
                // 선택한 Row Sheet만 저장 한다.
                dt = fpSpread1_Sheet1.DataSource as DataTable;
                FarPoint.Win.Spread.Model.CellRange[] oSelectItem = fpSpread1_Sheet1.GetSelections();
                int idx = -1;

                ControlEnable(BtnGetData, false);
                ControlEnable(btnExecute, false);
                ControlEnable(dtFrom, false);
                ControlEnable(dtTo, false);
                ControlEnable(BtnStop, true);

                UpdateTextBox(string.Format("Migration 시작"));

                oDefectDefine = new DACrux.SEMDMS.RO.DefectDefine();

                for (int i = 0; i < oSelectItem.Length; i++)
                {
                    for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                    {
                        oMapinfo = new MapInfo.MapInfo();
                        oMap = new MapInfo.Map();
                        //oMapSetup = new MapInfo.MAP_SETUP();
                        oDefect = new MapInfo.DEFECT_INFO();

                        oStartDt = DateTime.Now;
                        UpdateProcess(string.Format("Processsing : {0} / {1}", (cr + 1), oSelectItem[i].RowCount));

                        idx = oSelectItem[i].Row + cr;
                        strTranTime = dt.Rows[idx]["TRAN_TIME"].ToString().Trim();
                        strDevice = dt.Rows[idx]["DEVICE"].ToString().Trim();
                        strLevelName = dt.Rows[idx]["LEVEL_NAME"].ToString().Trim();
                        strLotID = dt.Rows[idx]["LOT_NUMBER"].ToString().Trim();
                        strWaferID = string.Format("{0}-{1:00}", dt.Rows[idx]["LOT_NUMBER"].ToString().Trim(), dt.Rows[idx]["SLOTID"]);
                        strStepID = dt.Rows[idx]["LEVEL_NAME"].ToString().Trim();//.Substring(dt.Rows[idx]["LEVEL_NAME"].ToString().Trim().Length - 4); //LEVEL_NAME 의 끝 4자리 EX) 7155M3ET, 1439TRET
                        strSlotID = dt.Rows[idx]["SLOTID"].ToString().Trim();
                        strMachine = dt.Rows[idx]["MACHINE"].ToString().Trim();
                        strArea = dt.Rows[idx]["AREA"].ToString().Trim();

                        //UpdateTextBox(string.Format("Start Time : {0}\nLot ID : {1}", oStartDt, strLotID));
                        //UpdateTextBox(string.Format("Wafer ID : {0}", strWaferID));

                        oTranDt = DateTime.ParseExact(dt.Rows[idx]["SYS_TIME"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);


                        //사용 현황에 대해 성공 또는 오류 Message 출력
                        try
                        {
                            oEsda = new ESDA.ESDA();

                            //UpdateTextBox(string.Format("ESDA Recipe File 정보 가져오기."));
                            oEsda.fnFTPRecipe(strDevice, ref oMapinfo);

                            //UpdateTextBox(string.Format("ESDA Map File 정보 가져오기."));
                            oEsda.fnFTPMap(strDevice, ref oMap);

                            //UpdateTextBox(string.Format("ESDA MAP_SETUP Table 정보 가져 오기."));
                            //oEsda.fnGetMapSetup(strDevice, ref oMapSetup);
                            //UpdateTextBox(string.Format("ESDA T_ESDA_IDATA , T_ESDA_IMAGES Table 정보 가져 오기."));
                            oEsda.fnGetDefectList(dt.Rows[idx]["LEVEL_SEQ"].ToString().Trim(), ref oDefect);

                            //===========================================================================================================================
                            //Migration 시작
                            //===========================================================================================================================

                            //UpdateTextBox(string.Format("Migration 시작"));
                            decimal dTemp = 0;
                            dPITCH_X = (decimal)oMapinfo.X_period * RK;
                            dPITCH_Y = (decimal)oMapinfo.Y_period * RK;

                            //dOrignX = (decimal)(dPITCH_X * oMapSetup.TEST_X) + oMapSetup.ORIGIN_X; 
                            //dOrignY = (decimal)(dPITCH_Y * oMapSetup.TEST_Y) + oMapSetup.ORIGIN_Y; 

                            if (decimal.TryParse(dt.Rows[idx]["SAMPLEX"].ToString().Trim(), out dTemp) == false)
                                throw new Exception("SAMPLEX 변환 실패");

                            dSampleX = (dTemp * RK) + dPITCH_X;

                            dOrignXIndex = (int)Math.Truncate(dSampleX / dPITCH_X);
                            dOrignX = dSampleX % dPITCH_X;

                            if (decimal.TryParse(dt.Rows[idx]["SAMPLEY"].ToString().Trim(), out dTemp) == false)
                                throw new Exception("SAMPLEX 변환 실패");

                            dSampleY = (dTemp * RK) + dPITCH_Y;

                            dOrignYIndex = (int)Math.Truncate(dSampleY / dPITCH_Y);
                            dOrignY = dSampleY % dPITCH_Y;


                            //dTemp = MapSize - (oMapinfo.X_os * RK);
                            //dOrignX = (((dTemp - (dTemp - (dPITCH_X * oMapinfo.Max_cols))) - MapEdgeMargine) / 2) + dPITCH_X;

                            //dTemp = MapSize - (oMapinfo.Y_os * RK);
                            //dOrignY = (((dTemp - (dTemp - (dPITCH_Y * oMapinfo.Max_rows))) - MapEdgeMargine) / 2) + dPITCH_Y;

                            //TQD_SETUP 확인 생성
                            //UpdateTextBox(string.Format("TQD_SETUP 확인 생성"));
                            strSetupID = string.Format("{0}_{1}", strDevice, strLevelName);
                            SetupSeq = oDefectDefine.GetSetupInfo(strSetupID, dOrignXIndex.ToString(), dOrignYIndex.ToString());
                            if (SetupSeq < 0)
                            {
                                //UpdateTextBox(string.Format("TQD_SETUP Data 생성 한다."));
                                strDBData = new string[14];
                                strDBData[0] = strSetupID;				                                    //SETUP_ID             VARCHAR(40) NOT NULL,
                                strDBData[1] = strStepID;				                                    //STEP_ID              VARCHAR(40) NOT NULL,
                                strDBData[2] = strTranTime;			                                        //SETUP_TIME           TIMESTAMP NOT NULL,
                                strDBData[3] = "0";                                                         // oMapSetup.TEST_ANGLE.ToString(); Default Donw        //ANGLE                INTEGER NOT NULL DEFAULT 0,
                                //strDBData[4] = oMapSetup.WAFER_SIZE == 0 ? "200000000" : oMapSetup.WAFER_SIZE.ToString();	   //WAFER_SIZE           INTEGER NOT NULL DEFAULT 300000000,
                                //strDBData[5] = string.IsNullOrEmpty(oMapSetup.NOTCH_TYPE) ? "F" : oMapSetup.NOTCH_TYPE;//NOTCH_TYPE           CHAR(1) NOT NULL DEFAULT 'F',
                                strDBData[4] = "200000";	   //WAFER_SIZE           INTEGER NOT NULL DEFAULT 300000000,
                                strDBData[5] = "F";//NOTCH_TYPE           CHAR(1) NOT NULL DEFAULT 'F',
                                strDBData[6] = dPITCH_X.ToString();			                //DIE_PITCH_X          INTEGER NOT NULL DEFAULT 0,
                                strDBData[7] = dPITCH_Y.ToString();			                //DIE_PITCH_Y          INTEGER NOT NULL DEFAULT 0,
                                strDBData[8] = dOrignXIndex.ToString();			                            //DIE_ORIGIN_X         INTEGER NOT NULL DEFAULT 0,
                                strDBData[9] = dOrignYIndex.ToString();			                            //DIE_ORIGIN_Y         INTEGER NOT NULL DEFAULT 0,
                                strDBData[10] = dOrignX.ToString();  //ORIGIN_X             INTEGER NOT NULL DEFAULT 0,
                                strDBData[11] = dOrignY.ToString();  //ORIGIN_Y             INTEGER NOT NULL DEFAULT 0
                                strDBData[12] = oMapinfo.Street_width.ToString();  //STREET_X
                                strDBData[13] = oMapinfo.Street_height.ToString();  //STREET_Y

                                oDefectDefine.CreateSetupInfo(strDBData);

                                SetupSeq = oDefectDefine.GetSetupInfo(strSetupID);
                                if (SetupSeq < 0)
                                    throw new Exception("TQD_SETUP 생성 실패");
                            }

                            //TQD_SETUP_MAP 확인 생성
                            //UpdateTextBox(string.Format("TQD_SETUP_MAP 확인 생성"));
                            if (oDefectDefine.GetSetupMapCount(SetupSeq) == 0)
                            {
                                //Data 가 있는지 확인 하고 없으면 생성 한다.
                                //UpdateTextBox(string.Format("TQD_SETUP_MAP 처리"));
                                string[,] para = new string[oMap.TestDieInfo.Length, 4];
                                for (int ir = 0; ir < oMap.TestDieInfo.Length; ir++)
                                {
                                    para[ir, 0] = SetupSeq.ToString();
                                    para[ir, 1] = "1";  //InspectionTest
                                    para[ir, 2] = oMap.TestDieInfo[ir].DX.ToString();	//INDEX_X
                                    para[ir, 3] = oMap.TestDieInfo[ir].DY.ToString();	//INDEX_Y	
                                }

                                oDefectDefine.CreateSetupMap(para);
                            }

                            //TQD_PRODUCT 확인 생성
                            //UpdateTextBox(string.Format("TQD_PRODUCT 확인 생성"));
                            dtTemp = oDefectDefine.GetProductInfo(strDevice);
                            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                            {
                                //UpdateTextBox(string.Format("TQD_PRODUCT Data 생성 한다."));
                                strDBData = new string[14];
                                strDBData[0] = strDevice;	//product
                                strDBData[1] = DACrux.Base.GlobalVariable.Factory;	//facility
                                strDBData[2] = " ";	//technology
                                strDBData[3] = oMap.TestDieInfo.Length.ToString();	//netdie
                                strDBData[4] = "200000";	//wafer_size
                                strDBData[5] = "0";	//angle
                                strDBData[6] = "F";	//notch_type
                                strDBData[7] = dPITCH_X.ToString();	//die_pitch_x
                                strDBData[8] = dPITCH_Y.ToString();	//die_pitch_y
                                strDBData[9] = dOrignXIndex.ToString();  	//die_origin_x
                                strDBData[10] = dOrignYIndex.ToString();	//die_origin_y
                                strDBData[11] = dOrignX.ToString();	//origin_x
                                strDBData[12] = dOrignY.ToString();	//origin_y
                                strDBData[13] = "Migration";	//description
                                oDefectDefine.CreateProduct(strDBData);

                            }

                            //TQD_LOT 확인 생성
                            //UpdateTextBox(string.Format("TQD_LOT 확인 생성"));
                            strDBData = new string[3];
                            strDBData[0] = strLotID;  //LOT ID		
                            strDBData[1] = strDevice;	  //PRODUCT
                            strDBData[2] = " ";						                      //COMMENT

                            LotSeq = oDefectDefine.GetLotSeq(strDBData);

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (LotSeq < 0)
                            {
                                //UpdateTextBox(string.Format("TQD_LOT Data 생성 한다."));
                                oDefectDefine.CreateLotInfo(strDBData);
                                LotSeq = oDefectDefine.GetLotSeq(strDBData);

                                if (LotSeq < 0)
                                    throw new Exception("TQD_LOT 생성 실패");
                            }

                            //TQD_WAFER 확인 생성
                            //UpdateTextBox(string.Format("TQD_WAFER 확인 생성"));
                            strDBData = new string[4];
                            strDBData[0] = LotSeq.ToString();		                        //	LOT_SEQ  
                            strDBData[1] = strWaferID;                                      //	WAFER_ID 
                            strDBData[2] = strSlotID;        //	SLOT_ID 
                            strDBData[3] = " ";						                        //	COMMENT
                            WaferSeq = oDefectDefine.GetWaferSeq(strDBData);

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (WaferSeq < 0)
                            {
                                //UpdateTextBox(string.Format("TQD_WAFER Data 생성 한다."));
                                oDefectDefine.CreateWaferInfo(strDBData);
                                WaferSeq = oDefectDefine.GetWaferSeq(strDBData);

                                if (WaferSeq < 0)
                                    throw new Exception("TQD_WAFER 생성 실패");
                            }

                            decimal oDefectDD = decimal.Zero;

                            if(strArea != "0")
                                oDefectDD = (decimal)oDefect.DefectCount / decimal.Parse(strArea);
                            
                            //Step 정보 조회 및 생성
                            //UpdateTextBox(string.Format("Step 정보 확인 생성"));
                            strDBData = new string[20];
                            strDBData[0] = "1";                                    //TEST
                            strDBData[1] = WaferSeq.ToString();                    //WAFER_SEQ
                            strDBData[2] = strStepID;                              //STEP_ID, LEVEL_NAME 의 끝 4자리 EX) 7155M3ET, 1439TRET
                            strDBData[3] = strSlotID;                              //SLOT_ID
                            strDBData[4] = strLotID;                               //RESULT_ID
                            strDBData[5] = strMachine;                             //INSPECTION_EQ
                            strDBData[6] = strMachine;                             //REVIEW_EQ
                            strDBData[7] = SetupSeq.ToString();                    //SETUP_SEQ
                            strDBData[8] = strTranTime;                            //RESULTTIMESTAMP
                            strDBData[9] = strTranTime;                            //FILETIMESTAMP
                            strDBData[10] = strMachine;                            //MAIN_EQ	             
                            strDBData[11] = " ";                                   //ROUTE		             
                            strDBData[12] = " ";                                   //OPER				             
                            strDBData[13] = strArea;                               //SCAN_AREA
                            strDBData[14] = oDefect.DefectDieCount.ToString();     //DEFECTIVE_DIE
                            strDBData[15] = oDefect.DefectCount.ToString();        //DEFECTS
                            strDBData[16] = "0";                                   //IMAGES
                            strDBData[17] = oDefectDD.ToString();                  //DEFECT_DD 
                            strDBData[18] = " ";                                   //INSP_FILENAME
                            strDBData[19] = " ";                                   //INSP_FILEPATH

                            StepSeq = oDefectDefine.GetStepSeq(new string[] { strDBData[1], strDBData[2], strDBData[8], strDBData[0] }); ///WAFER_SEQ,INSPECTION_EQ,STEP_ID,RESULTTIMESTAMP,TEST

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (StepSeq < 0)
                            {
                                //UpdateTextBox(string.Format("TQD_STEP Data 생성 한다."));

                                oDefectDefine.CreateStepInfo(strDBData);
                                StepSeq = oDefectDefine.GetStepSeq(new string[] { strDBData[1], strDBData[2], strDBData[8], strDBData[0] });

                                if (StepSeq < 0)
                                    throw new Exception("TQD_STEP 생성 실패");

                            }

                            //각각의 Sequence 를 Log 처리
                            //UpdateTextBox(string.Format("SetupSeq : {0} / LotSeq : {1} / WaferSeq : {2} / StepSeq : {3}", SetupSeq, LotSeq, WaferSeq, StepSeq));

                            //TQD_DEFECT, TQD_IMAGES 확인 생성
                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            //UpdateTextBox(string.Format("TQD_DEFECT, TQD_IMAGES 확인 생성"));
                            iImageCount = 0;
                            iClassifiedCnt = 0;
                            if (oDefect.DefectCount > 0 && oDefectDefine.GetDefectCount(WaferSeq.ToString(), StepSeq.ToString()) == 0)
                            {
                                //UpdateTextBox(string.Format("TQD_DEFECT 처리"));
                                string[,] para = new string[oDefect.DefectList.Length, 28];
                                List<string[]> lsArr = new List<string[]>();
                                for (int ir = 0; oDefect.DefectList.Length > ir; ir++)
                                {
                                    para[ir, 0] = StepSeq.ToString(); //step_seq
                                    para[ir, 1] = oDefect.DefectList[ir].Index.ToString();	//defectid
                                    para[ir, 2] = WaferSeq.ToString(); //wafer_seq
                                    para[ir, 3] = ((oDefect.DefectList[ir].Xmicron) - dSampleX + dPITCH_X).ToString();	//X
                                    para[ir, 4] = ((oDefect.DefectList[ir].Ymicron) - dSampleY + dPITCH_Y).ToString();	//Y
                                    para[ir, 5] = (oDefect.DefectList[ir].Xmicron - (dPITCH_X * (oDefect.DefectList[ir].DIE_X - 1))).ToString();	//XREL
                                    para[ir, 6] = (oDefect.DefectList[ir].Ymicron - (dPITCH_Y * (oDefect.DefectList[ir].DIE_Y - 1))).ToString();	//YREL
                                    para[ir, 7] = oDefect.DefectList[ir].DIE_X.ToString();	// XINDEX
                                    para[ir, 8] = oDefect.DefectList[ir].DIE_Y.ToString();	// YINDEX
                                    para[ir, 9] = oDefect.DefectList[ir].Xsize.ToString();	//XSIZE
                                    para[ir, 10] = oDefect.DefectList[ir].Ysize.ToString();	//YSIZE
                                    para[ir, 11] = dt.Rows[idx]["AREA"].ToString().Trim();	//DEFECTAREA
                                    para[ir, 12] = oDefect.DefectList[ir].DefectSize.ToString(); //DSIZE
                                    para[ir, 13] = oDefect.DefectList[ir].Defect_category.ToString(); //CLASSNUMBER
                                    para[ir, 14] = "1"; //TEST
                                    para[ir, 15] = oDefect.DefectList[ir].Defect_cluster.ToString(); //CLUSTERNUMBER
                                    para[ir, 16] = oDefect.DefectList[ir].Roughbin.ToString(); //ROUGHBINNUMBER	
                                    para[ir, 17] = "0"; //FINEBINNUMBER
                                    para[ir, 18] = "0"; //REVIEWSAMPLE	
                                    para[ir, 19] = string.IsNullOrEmpty(oDefect.DefectList[ir].Image_file_name.Trim()) ? "0" : "1"; //IMAGECOUNT
                                    para[ir, 20] = oDefect.DefectList[ir].Newdefect == "Y" ? "1" : "0"; //ADDER
                                    para[ir, 21] = oDefect.DefectList[ir].First_level.ToString(); //FIRST_STEP
                                    para[ir, 22] = "0"; //RETICLE_REPEAT_ID
                                    para[ir, 23] = "0"; //DIE_REPEAT_ID
                                    para[ir, 24] = "0"; //MAN_OPT_CLASS
                                    para[ir, 25] = "0"; //AUTO_OPT_CLASS
                                    para[ir, 26] = "0"; //MAN_SEM_CLASS
                                    para[ir, 27] = "0"; //AUTO_SEM_CLASS

                                    if (string.IsNullOrEmpty(oDefect.DefectList[ir].Image_file_name.Trim()) == false)
                                    {
                                        string[] ArrImage = oDefect.DefectList[ir].Image_file_name.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                                        string[] ArrThumb = oDefect.DefectList[ir].Thumb_file_name.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                                        //기존 SubPath 에 대해 Migration 시 직접 넣어 준다.
                                        string strSubPath = "/DMS/DEFECTIMAGES/";

                                        lsArr.Add(new string[]{ StepSeq.ToString()                         //step_seq
                                                                                ,oDefect.DefectList[ir].Index.ToString()	//defectid
                                                                                ,"0"                                        //image_id
                                                                                ,"1"
                                                                                ,WaferSeq.ToString()
                                                                                ,".jpg"
                                                                                ,strSubPath + oDefect.DefectList[ir].Image_file_name.Replace(string.Format("/{0}", ArrImage[ArrImage.Length - 1].Trim()), "")
                                                                                ,ArrImage[ArrImage.Length - 1].Trim()
                                                                                ,"localhost"
                                                                                ,strSubPath + oDefect.DefectList[ir].Thumb_file_name.Replace(string.Format("/{0}", ArrThumb[ArrThumb.Length - 1].Trim()), "")
                                                                                ,ArrThumb[ArrThumb.Length - 1].Trim()});
                                        iImageCount++;
                                    }

                                    if(oDefect.DefectList[ir].Defect_category > 0)
                                        iClassifiedCnt++;
                                }

                                oDefectDefine.CreateDefect(para);

                                //Defect 의 Image 가 있을 경우에만 저장 Logic 을 탄다.
                                if (lsArr.Count > 0)
                                {
                                    para = new string[lsArr.Count, 11];
                                    int iRow = 0;
                                    foreach (string[] Var in lsArr)
                                    {
                                        para[iRow, 0] = Var[0]; // STEP_SEQ
                                        para[iRow, 1] = Var[1]; // DEFECTID
                                        para[iRow, 2] = Var[2]; // IMAGE_ID
                                        para[iRow, 3] = Var[4]; // WAFER_SEQ
                                        para[iRow, 4] = Var[3]; // TEST
                                        para[iRow, 5] = Var[5]; // IMAGE_TYPE
                                        para[iRow, 6] = Var[6]; // IMAGE_PATH
                                        para[iRow, 7] = Var[7]; // IMAGE_FILENAME
                                        para[iRow, 8] = Var[8]; // IMAGE_SERVER
                                        para[iRow, 9] = Var[9]; // THUMB_PATH
                                        para[iRow, 10] = Var[10]; // THUMB_FILENAME
                                        iRow++;
                                    }

                                    oDefectDefine.CreateImagesMulti(para);
                                }
                            }

                            //TQD_INSP_INFO 의 정보가 있는지 확인 후 없으면 저장 한다.
                            //UpdateTextBox(string.Format("TQD_INSP_INFO 확인 생성"));
                            dtTemp = oDefectDefine.GetInspInfo(StepSeq.ToString());

                            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                            {
                                oDefectDefine.CreateInspInfo(new string[]{  StepSeq.ToString()		                          //STEP_SEQ                  
																	,WaferSeq.ToString()				            	      //WAFER_SEQ
																	,strTranTime                                              //RESULTTIMESTAMP   
																	,strTranTime				                              //FILETIMESTAMP
                                                                    ,DACrux.Base.GlobalVariable.Factory                       //Factory
																	," "									                  //TECHNOLOGY
																	,strDevice                                                //PRODUCT     
																	,strLotID		                                          //LOT_ID
																	,strWaferID			                                      //WAFER_ID
																	,strStepID                                                //STEP_ID
																	,"1"	                                                  //TEST 
																	,strSlotID			                                      //SLOT_ID
																	,SetupSeq.ToString()					                  //SETUP_SEQ             
																	,""								                          //STATUS
																	,strMachine                                               //strMainEQ	
																	,""		                                                  //strRoute								   
																	,""		                                                  //strOper								
																	,strMachine                                               //INSPECTION_EQ            
																	," "				                                      //INSPECTION_PARAM
																	,strDBData[15]					                          //DEFECTS
																	,iClassifiedCnt.ToString()   	                          //CLASSIFIED_DEFECTS    
																	,"0"							                          //KILLER_DEFECTS        
																	,"0"							                          //RANDOM_DEFECTS
																	,"0"							                          //ADDER_KILLER_DEFECT
																	,"0"							                          //ADDER_RANDOM_DEFECT
																	,"0"							                          //CLUSTERS           
																	,"0"					   		                          //ADDER_CLUSTERS     
																	,"0"					   		                          //CLUSTER_AREA       
																	,strDBData[17]			   		                          //DEFECT_DD          
																	,"0"					   		                          //KILLER_DEFECT_DD   
																	,"0"					   		                          //RANDOM_DEFECT_DD   
																	,oMap.TestDieInfo.Length.ToString()                       //INSPECTED_DIE      
																	,strDBData[14]			   		                          //DEFECTIVE_DIE      
																	,strDBData[14]					   		                  //ADDER_DEF_DIE      
																	,"0"					   		                          //KILL_DEF_DIE       
																	,"0"					   		                          //KILL_ADDER_DEF_DIE 0
																    ,iImageCount.ToString() //strDBData[16]					  //IMAGES
																	,strArea			                                      //SCAN_AREA
																	,"0"					   				                  //ADDER_DEFECTS      
																	,"0"									                  //KILL_RND_DEFECTS 
                                                                    ,"Migration Data"                                         //Commnet
																	,""			                                              //RETICLE_ID			
																	,""                                                       //INTERLOCK_FLAG
																	," "						                              //STEPPER_EQ
																});
                            }

                            oEndDt = DateTime.Now;
                            TimeSpan dateDiff = oEndDt - oStartDt;
                            UpdateTextBox(string.Format("WaferID : {0}, Device : {1}, StepID : {2}, 소요시간 : {3}", strWaferID, strDevice, strStepID, dateDiff.TotalSeconds));
                            SetSpreadColor(oSelectItem[i].Row + cr, Color.Green);
                            Application.DoEvents();
                        }
                        catch (Exception ex)
                        {
                            UpdateTextBox(ex.Message);
                            SetSpreadColor(oSelectItem[i].Row + cr, Color.Red);

                            iFailCount++;
                            UpdateFailCount(string.Format("Fail Count : {0}", iFailCount));

                            UpdateErrorTextBox(string.Format("WaferID : {0}, Device : {1}, StepID : {2}", strWaferID, strDevice, strStepID));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                UpdateTextBox(ex.Message);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                ControlEnable(BtnGetData, true);
                ControlEnable(btnExecute, true);
                ControlEnable(dtFrom, true);
                ControlEnable(dtTo, true);
                ControlEnable(BtnStop, false);
            }
        }

        #region ThumeNail

        /// <summary>
        /// ThumeNail 생성
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="imagename"></param>
        /// <returns></returns>
        public string GetThumeNail(int x, int y, string imagename)
        {
            string strThumbFile = string.Empty;
            string strImgFile = string.Empty;
            string strPath = string.Empty;
            try
            {
                Image m_oImageOne = Image.FromFile(imagename); //큰 이미지 Load
                Image.GetThumbnailImageAbort myCallback =
                    new Image.GetThumbnailImageAbort(ThumbnailCallback); //Dummy CallBack
                Image oThumb = m_oImageOne.GetThumbnailImage(x, y, myCallback, IntPtr.Zero);

                //FileName Make
                strImgFile = Path.GetFileNameWithoutExtension(imagename);
                strPath = Path.GetDirectoryName(imagename);

                if (!Directory.Exists(string.Format(@"{0}\Thumb", strPath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\Thumb", strPath));
                }

                strThumbFile = string.Format(@"{0}\Thumb\{1}_thumb.jpg", strPath, strImgFile);


                oThumb.Save(strThumbFile, ImageFormat.Jpeg);
                m_oImageOne.Dispose();
                oThumb.Dispose();
                return strThumbFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
        #endregion

        #region UI thread

        /// <summary>
        /// Sheet Row 생상 변경
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="rCol"></param>
        private void SetSpreadColor(int idx, Color rCol)
        {
            try
            {
                if (fpSpread1.Disposing)
                {
                    return;
                }

                if (fpSpread1.InvokeRequired)
                {
                    fpSpread1.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            SetSpreadColor(idx, rCol);
                        }
                        ));
                }
                else
                {
                    fpSpread1_Sheet1.Rows[idx].BackColor = rCol;
                }
            }
            catch (Exception)
            {
                /// 무시
            }
        }

        /// <summary>
        /// 현재 진행 상황 Text box Update
        /// </summary>
        /// <param name="data"></param>
        private void UpdateTextBox(string data)
        {
            if (TxtLog.InvokeRequired)
            {
                // 작업쓰레드인 경우
                TxtLog.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateTextBox(data);
                        }
                        ));
            }
            else
            {
                // UI 쓰레드인 경우
                TxtLog.AppendText(string.Format(">> {0}\n", data));
                TxtLog.AppendText("---------------------------------------------\n");

                TxtLog.Focus();
                TxtLog.SelectionStart = TxtLog.SelectionStart;

                if (TxtLog.Lines.Length > 1000)
                    TxtLog.Clear();
            }
        }

        private void UpdateErrorTextBox(string data)
        {
            if (TxtErrorLog.InvokeRequired)
            {
                // 작업쓰레드인 경우
                TxtErrorLog.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateErrorTextBox(data);
                        }
                        ));
            }
            else
            {
                // UI 쓰레드인 경우
                TxtErrorLog.AppendText(string.Format(">> {0}\n", data));
                TxtErrorLog.AppendText("---------------------------------------------\n");

                TxtErrorLog.Focus();
                TxtErrorLog.SelectionStart = TxtErrorLog.SelectionStart;
            }
        }

        private void UpdateProcess(string data)
        {
            if (lbProcess.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbProcess.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateProcess(data);
                        }
                        ));
            }
            else
            {
                // UI 쓰레드인 경우
                lbProcess.Text = data;
            }
        }

        private void UpdateFailCount(string data)
        {
            if (lbFailCount.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbProcess.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateFailCount(data);
                        }
                        ));
            }
            else
            {
                // UI 쓰레드인 경우
                lbFailCount.Text = data;
            }
        }

        private void ControlEnable(Control uiControl, bool Enable)
        {
            try
            {
                if (uiControl.Parent.Disposing)
                {
                    return;
                }

                if (uiControl.InvokeRequired)
                {
                    fpSpread1.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            ControlEnable(uiControl, Enable);
                        }
                        ));
                }
                else
                {
                    uiControl.Enabled = Enable;
                }
            }
            catch (Exception)
            {
                /// 무시
            }
        }

        #endregion

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (trMigrationUpload != null && trMigrationUpload.IsAlive)
            {
                trMigrationUpload.Abort();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                var dir = new DirectoryInfo(@"D:\DBHitek\Recipe");
                FileInfo[] files = dir.GetFiles("*.drv");

                for(int i = 0; i < files.Length; i++)
                {
                   fnFTPRecipe(files[i]);
                } 
            }
            catch(Exception ex)
            {
            }


        }

        public string[] fnFTPRecipe(FileInfo oRecipe)
        {
            string[] strVal = null;

            StreamReader sr = null;

            //DACrux.SEMDMS.RO.DefectDefine oDefectDefine = null;
            try
            {
                //oDefectDefine = new DACrux.SEMDMS.RO.DefectDefine();
                strVal = new string[9];

                using (sr = new System.IO.StreamReader(oRecipe.FullName, System.Text.Encoding.Default))
                {
                    string line = string.Empty;
                    string[] sPara = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.ToUpper().StartsWith("DEVICE"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[0] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("MAX_ROWS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[1] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("MAX_COLS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[2] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("X_PERIOD"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[3] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("Y_PERIOD"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[4] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("X_OS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[5] = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("Y_OS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[6] = sPara[1].Trim();

                        }
                        else if (line.ToUpper().StartsWith("STREET_WIDTH"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[7] = sPara[1].Trim();

                        }
                        else if (line.ToUpper().StartsWith("STREET_HEIGHT"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strVal[8] = sPara[1].Trim();

                        }
                    }
                }

                //DRV_RECIPE 라는 Table 상에 File 내용 저장..
               // oDefectDefine.RECIPE_CREATE(strVal);

                return strVal;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }
            }
        }

        private void chkFail_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                for (int ir = 0; ir < fpSpread1_Sheet1.RowCount; ir++)
                {
                    fpSpread1_Sheet1.Rows[ir].Visible = !chkFail.Checked;

                    if (chkFail.Checked && fpSpread1_Sheet1.Rows[ir].BackColor == Color.Red)
                        fpSpread1_Sheet1.Rows[ir].Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
