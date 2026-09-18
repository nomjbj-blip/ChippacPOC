using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using DACrux.Base;
using DACrux.Framework.Server;
using DACrux.Data.Handler;
using DACrux.SEMDMS.BSL;
using DACrux.Data.Parser.Klarf;
using System.Drawing;

namespace DACrux.SEMDMS.Inspection.DataService
{
    public partial class InspectionDataService : DACrux.Framework.Server.DataServiceBase
    {
        public static readonly string FAB1 = "FAB1";
        public static readonly string FAB2 = "FAB2";

        enum TQD_SETUP { SETUP_ID, STEP_ID, SETUP_TIME, ANGLE, WAFER_SIZE, NOTCH_TYPE, DIE_PITCH_X, DIE_PITCH_Y, DIE_ORIGIN_X, DIE_ORIGIN_Y, ORIGIN_X, ORIGIN_Y }
        enum TQD_SETUP_MAP { SETUP_SEQ, TEST_NO, INDEX_X, INDEX_Y }
        enum TQD_PRODUCT { PRODUCT, FACILITY, TECHNOLOGY, NETDIE, WAFER_SIZE, ANGLE, NOTCH_TYPE, DIE_PITCH_X, DIE_PITCH_Y, DIE_ORIGIN_X, DIE_ORIGIN_Y, ORIGIN_X, ORIGIN_Y, DESCRIPTION }
        enum TQD_LOT { LOT_ID, PRODUCT, COMMENT }
        enum TQD_WAFER { LOT_SEQ, WAFER_ID, SLOT_ID, COMMENT }
        enum TQD_STEP { TEST, WAFER_SEQ, STEP_ID, SLOT_ID, RESULT_ID, INSPECTION_EQ, REVIEW_EQ, SETUP_SEQ, RESULTTIMESTAMP, FILETIMESTAMP, MAIN_EQ, ROUTE, OPER, SCAN_AREA, DEFECTIVE_DIE, DEFECTS, IMAGES, DEFECT_DD, INSP_FILENAME, INSP_FILEPATH, COMMENT }
        enum TQD_DEFECT { STEP_SEQ, DEFECTID, WAFER_SEQ, X, Y, XREL, YREL, XINDEX, YINDEX, XSIZE, YSIZE, DEFECTAREA, DSIZE, CLASSNUMBER, TEST, CLUSTERNUMBER, ROUGHBINNUMBER, FINEBINNUMBER, REVIEWSAMPLE, IMAGECOUNT, ADDER, FIRST_STEP, RETICLE_REPEAT_ID, DIE_REPEAT_ID, MAN_OPT_CLASS, AUTO_OPT_CLASS, MAN_SEM_CLASS, AUTO_SEM_CLASS }

        public InspectionDataService()
        {
            InitializeComponent();
        }

        protected override void ServiceStart()
        {
            // Inspeciton 설비에서 올라온 Klarf 파일에는 ClassLookup 정보가 없다.
            // 따라서, Review 설비로 Klarf 파일을 전달 시 ClassLookup 데이터를 넣어주어야 한다.
            DACrux.SEMDMS.BSL.DefectMapAnalysis obj = new DACrux.SEMDMS.BSL.DefectMapAnalysis();
            Dictionary<int, string> dic = new Dictionary<int, string>();

            DataTable dt = obj.GetDefectClassInfo();

            foreach (DataRow row in dt.Rows)
                dic.Add(Int32.Parse(row["CLASSNUMBER"].ToString()), row["NAME"].ToString());

            DACrux.Data.Parser.Klarf.ParserKlarf.UserClassLookup = dic;

            DmsCache.Instance.WaferDieInfoDel = GetWaferDieInfo;
        }

        protected override void Execute()
        {
            WriteLog("Execute()");

            //=====================================================================================================================
            DataTable dtTemp = null;

            List<System.Drawing.Point> oIndex = null;
            DateTime oTime = DateTime.Now;

            DACrux.SEMDMS.BSL.DefectDefine oDefectDefine = new DACrux.SEMDMS.BSL.DefectDefine();
            DACrux.Common.BSL.EquipManagement oEquipment = new Common.BSL.EquipManagement();
            DACrux.Data.Handler.HandlerFactory.SetGarbageExtension(RemoveGarbageFileExtension);

            // 저장된 Map의 Origin Die 인덱스를 가져올 수 있도록 한다.
            DACrux.Data.Parser.Klarf.ParserKlarf.GetOriginDieIndex = oDefectDefine.GetOriginDieIndex;

#if SINGLE_EQUIP
            if (String.IsNullOrEmpty(EquipID))
                throw new Exception("EQUIP_ID 값이 설정되지 않았습니다.");

            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, EquipID, DataPath, oEquipment.GetEquipInfo);
#else
            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, DataPath, oEquipment.GetEquipInfo);
#endif

            foreach (DACrux.Data.Handler.HandlerInsp handler in handlerList)
            {
                if (IsReqeustServiceStop)
                    break;

                // KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 설비 별로 임시로 데이터 및 원본 이미지 파일을 복사해야 할 경우 2019.10.24
                /*
                if (!String.IsNullOrWhiteSpace(handler.EquipInfo.Grp01))
                {
                    try
                    {
                        // LOTEND*.trf 파일이 남아 있는 경우 복사한다. 이 파일 없으면 review 파일을 못 만든다고 함. 한동은 수석 요청 2019.10.27
                        foreach (string file in Directory.GetFiles(Path.Combine(DataPath, handler.EquipInfo.EquipID), "LOTEND*.trf"))
                        {
                            // 복사 후에는 trf 파일을 삭제한다.
                            File.Copy(file, Path.Combine(handler.EquipInfo.Grp01, Path.GetFileName(file)), true);
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                        AppendServiceLog(ex, String.Empty, EquipID);
                        WriteLog(ex);
                    }
                }
                */
                //END: KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // 데이터 파싱 처리
                //WriteLog("Step -", "Data Parsing Start");
                handler.LogMethod = WriteLog;
                handler.Run();

                //
                // 일반 KLARF 파일과 TRF KLARF 파일을 따로 처리한다. 2019.11.04 Taihi,Kim.
                //
                foreach (DACrux.Data.Parser.Klarf.ParserKlarf parser in handler.ParserList)
                {
                    // KLARF 파일만 처리
                    if (!(parser.GetType() == typeof(DACrux.Data.Parser.Klarf.ParserKlarf) || parser.GetType() == typeof(DACrux.Data.Parser.Klarf.ParserKlarf_18)))
                        continue;

                    try
                    {
                        if (IsReqeustServiceStop)
                            break;

                        if (parser.ErrorFlag)
                            throw new Exception(parser.ErrorMessage);
                        else
                            parser.ErrorBackupFlag = true;

                        string[] strDBData = null;

                        string strArea = string.Empty;

                        long SetupSeq = -1;
                        long LotSeq = -1;
                        long WaferSeq = -1;
                        long StepSeq = -1;

                        double dCenterXLocation = 0;
                        double dCenterYLocation = 0;

                        int iClassifiedCnt = 0;
                        int iNDEFDIE = 0;
                        double dDEFDENSITY = 0;
                        double dAreaTest = 0;
                        bool bStepNew = false;
                        string[,] para = null;

                        StopWatch.Start();

                        WriteLog("File Name", parser.FileName);

                        foreach (var wafer in parser.Wafers)
                        {
                            int iAngle = 0;
                            string strStepID = parser.StepID;

                            //FAB1 의 경우 Step ID 를 8자리로 사용한다.
                            //8자리가 아닌 경우 Setup 정보의 _이후 8자리를 사용 한다.
                            if (Factory == FAB1 && parser.StepID.Length != 8)
                            {
                                //Ex) SetupID "AMTDL_1439TRET"
                                string[] strStepInfo = parser.SetupID.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                                if (strStepInfo.Length != 2)
                                    throw new Exception("FAB1 Step ID 정의 Rule 이 틀립니다.");

                                strStepID = strStepInfo[1].Trim();
                            }

                            switch (parser.OrientationMarkLocation)
                            {
                                case "TOP": iAngle = 180; break;
                                case "LEFT": iAngle = 90; break;
                                case "RIGHT": iAngle = 270; break;
                                case "DOWN": iAngle = 0; break;
                            }

                            dCenterXLocation = (double)(parser.DiePitchX * wafer.DieOriginX) + wafer.SampleCenterLocationX;
                            dCenterYLocation = (double)(parser.DiePitchY * wafer.DieOriginY) + wafer.SampleCenterLocationY;

                            #region [Step 01 : TQD_SETUP 확인 생성]

                            // MAP 존재여부 및 저장 시에는 8자리로 변환한 STEP ID를 사용하지 않고 KLARF 파일에서 올라온 STEP ID를 사용한다. 2019.12.06 Taihi,Kim.
                            //WriteLog("Step 01", "TQD_SETUP 확인 생성");
                            //WriteLog("-", string.Format("SetupID : {0}, SetupTimestamp : {1}", parser.SetupID, parser.SetupTimestamp.ToString("yyyyMMddHHmmss")));
                            SetupSeq = oDefectDefine.GetSetupDuplicate(parser.SetupID, parser.StepID, parser.SetupTimestamp.ToString("yyyyMMddHHmmss"));
                            if (SetupSeq < 0)
                            {
                                strDBData = new string[Enum.GetNames(typeof(TQD_SETUP)).Length];
                                strDBData[(int)TQD_SETUP.SETUP_ID] = parser.SetupID;				                               //SETUP_ID            
                                strDBData[(int)TQD_SETUP.STEP_ID] = parser.StepID;			                                           //STEP_ID             
                                strDBData[(int)TQD_SETUP.SETUP_TIME] = parser.SetupTimestamp.ToString("yyyyMMddHHmmss");	       //SETUP_TIME          
                                strDBData[(int)TQD_SETUP.ANGLE] = iAngle.ToString();                                               //ANGLE               
                                strDBData[(int)TQD_SETUP.WAFER_SIZE] = (parser.SampleSize * 1000).ToString();	                   //WAFER_SIZE          
                                strDBData[(int)TQD_SETUP.NOTCH_TYPE] = "N";                                                        //NOTCH_TYPE          
                                strDBData[(int)TQD_SETUP.DIE_PITCH_X] = parser.DiePitchX.ToString();			                   //DIE_PITCH_X         
                                strDBData[(int)TQD_SETUP.DIE_PITCH_Y] = parser.DiePitchY.ToString();			                   //DIE_PITCH_Y         
                                strDBData[(int)TQD_SETUP.DIE_ORIGIN_X] = wafer.DieOriginX.ToString();			                   //DIE_ORIGIN_X  Index    
                                strDBData[(int)TQD_SETUP.DIE_ORIGIN_Y] = wafer.DieOriginY.ToString();			                   //DIE_ORIGIN_Y  Index      
                                strDBData[(int)TQD_SETUP.ORIGIN_X] = wafer.SampleCenterLocationX.ToString();                       //ORIGIN_X            
                                strDBData[(int)TQD_SETUP.ORIGIN_Y] = wafer.SampleCenterLocationY.ToString();                       //ORIGIN_Y            

                                oDefectDefine.CreateSetupData(strDBData);

                                SetupSeq = oDefectDefine.GetSetupDuplicate(parser.SetupID, parser.StepID, parser.SetupTimestamp.ToString("yyyyMMddHHmmss"));
                                if (SetupSeq < 0)
                                    throw new Exception("TQD_SETUP 생성 실패");
                            }
                            //WriteLog("Setup Seq", SetupSeq.ToString());

                            #endregion

                            #region [Step 02 : TQD_SETUP_MAP 확인 생성]

                            //WriteLog("Step 02 ", "TQD_SETUP_MAP 확인 생성");
                            oIndex = new List<System.Drawing.Point>();
                            foreach (var oData in wafer.TestList)
                            {
                                para = new string[oData.SampleTestPlan.Length, Enum.GetNames(typeof(TQD_SETUP_MAP)).Length];
                                for (int ir = 0; ir < oData.SampleTestPlan.Length; ir++)
                                {
                                    //Data 가 있는지 확인 하고 없으면 생성 한다.
                                    System.Drawing.Point oPt = new System.Drawing.Point(oData.SampleTestPlan[ir].X, oData.SampleTestPlan[ir].Y);
                                    if (oIndex.IndexOf(oPt) < 0)
                                        oIndex.Add(oPt);

                                    //Test No 별로 Array에 넣는다.
                                    para[ir, (int)TQD_SETUP_MAP.SETUP_SEQ] = SetupSeq.ToString();
                                    para[ir, (int)TQD_SETUP_MAP.TEST_NO] = oData.TestNo.ToString();
                                    para[ir, (int)TQD_SETUP_MAP.INDEX_X] = oIndex[ir].X.ToString();	//INDEX_X
                                    para[ir, (int)TQD_SETUP_MAP.INDEX_Y] = oIndex[ir].Y.ToString();	//INDEX_Y	
                                }

                                //Test No 별로 Map 에 대한 정보를 확인 후 없으면 생성 한다.
                                dtTemp = oDefectDefine.GetSetupTestData(SetupSeq.ToString(), oData.TestNo.ToString());
                                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                                    oDefectDefine.CreateSetupMap(para);
                            }
                            #endregion

                            #region [Step 03 : TQD_PRODUCT 확인 생성]

                            //WriteLog("Step 03", "TQD_PRODUCT 확인 생성");
                            //WriteLog("-", string.Format("DeviceID : {0}", parser.DeviceID));
                            dtTemp = oDefectDefine.GetProductInfo(parser.DeviceID);
                            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                            {
                                strDBData = new string[Enum.GetNames(typeof(TQD_PRODUCT)).Length];
                                strDBData[(int)TQD_PRODUCT.PRODUCT] = parser.DeviceID;	                                 //product
                                strDBData[(int)TQD_PRODUCT.FACILITY] = this.Factory;        	                         //facility
                                strDBData[(int)TQD_PRODUCT.TECHNOLOGY] = "NONE";	                                             //technology
                                strDBData[(int)TQD_PRODUCT.NETDIE] = oIndex.Count.ToString();	                         //netdie
                                strDBData[(int)TQD_PRODUCT.WAFER_SIZE] = (parser.SampleSize * 1000).ToString();	     //wafer_size
                                strDBData[(int)TQD_PRODUCT.ANGLE] = iAngle.ToString();	                             //angle
                                strDBData[(int)TQD_PRODUCT.NOTCH_TYPE] = "N";	                                             //notch_type
                                strDBData[(int)TQD_PRODUCT.DIE_PITCH_X] = parser.DiePitchX.ToString();	             //die_pitch_x
                                strDBData[(int)TQD_PRODUCT.DIE_PITCH_Y] = parser.DiePitchY.ToString();               //die_pitch_y
                                strDBData[(int)TQD_PRODUCT.DIE_ORIGIN_X] = wafer.DieOriginX.ToString();                      //die_origin_x Index
                                strDBData[(int)TQD_PRODUCT.DIE_ORIGIN_Y] = wafer.DieOriginY.ToString();                     //die_origin_y Index
                                strDBData[(int)TQD_PRODUCT.ORIGIN_X] = wafer.SampleCenterLocationX.ToString();   //origin_x
                                strDBData[(int)TQD_PRODUCT.ORIGIN_Y] = wafer.SampleCenterLocationY.ToString();   //origin_y
                                strDBData[(int)TQD_PRODUCT.DESCRIPTION] = "Parser";                                        //description
                                oDefectDefine.CreateProduct(strDBData);
                            }

                            #endregion

                            #region [Step 04 : TQD_LOT 확인 생성]

                            //WriteLog("Step 04", "TQD_LOT 확인 생성");
                            //WriteLog("-", string.Format("LotID : {0}", parser.LotID));
                            strDBData = new string[Enum.GetNames(typeof(TQD_LOT)).Length];
                            strDBData[(int)TQD_LOT.LOT_ID] = parser.LotID;         //LOT ID		
                            strDBData[(int)TQD_LOT.PRODUCT] = parser.DeviceID;	   //PRODUCT
                            strDBData[(int)TQD_LOT.COMMENT] = "Parser";            //COMMENT

                            LotSeq = oDefectDefine.GetLotSeq(strDBData);

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (LotSeq < 0)
                            {
                                oDefectDefine.CreateLotInfo(strDBData);
                                LotSeq = oDefectDefine.GetLotSeq(strDBData);

                                if (LotSeq < 0)
                                    throw new Exception("TQD_LOT 생성 실패");
                            }
                            //WriteLog("LotSeq", LotSeq.ToString());

                            #endregion

                            #region [Step 05 : TQD_WAFER 확인 생성]
                            //////////////////////////////////////////////////////////////////////////////////////////////
                            //WriteLog("Step 05", "TQD_WAFER 확인 생성");
                            //WriteLog("-", string.Format("WaferID : {0}, Slot : {1}", wafer.WaferID, wafer.Slot));
                            strDBData = new string[Enum.GetNames(typeof(TQD_WAFER)).Length];
                            strDBData[(int)TQD_WAFER.LOT_SEQ] = LotSeq.ToString();      // LOT_SEQ  
                            strDBData[(int)TQD_WAFER.WAFER_ID] = wafer.WaferID;         // WAFER_ID 
                            strDBData[(int)TQD_WAFER.SLOT_ID] = wafer.Slot.ToString();  // SLOT_ID 
                            strDBData[(int)TQD_WAFER.COMMENT] = "Parser";               // COMMENT
                            WaferSeq = oDefectDefine.GetWaferSeq(strDBData);

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (WaferSeq < 0)
                            {
                                oDefectDefine.CreateWaferInfo(strDBData);
                                WaferSeq = oDefectDefine.GetWaferSeq(strDBData);

                                if (WaferSeq < 0)
                                    throw new Exception("TQD_WAFER 생성 실패");
                            }

                            #endregion

                            #region [Step 06 : TQD_STEP 확인 생성]

                            //WriteLog("Step 06", "TQD_STEP 확인 생성");
                            //WriteLog("-", string.Format("StepID : {0}, Equip : {1}, ResultTimestamp : {2}", strStepID, parser.Equip, parser.ResultTimestamp.ToString("yyyyMMddHHmmss")));
                            //Step 정보 조회 및 생성
                            strDBData = new string[Enum.GetNames(typeof(TQD_STEP)).Length];
                            strDBData[(int)TQD_STEP.TEST] = "1";                                                             //TEST
                            strDBData[(int)TQD_STEP.WAFER_SEQ] = WaferSeq.ToString();                                        //WAFER_SEQ
                            strDBData[(int)TQD_STEP.STEP_ID] = strStepID;                                                //STEP_ID
                            strDBData[(int)TQD_STEP.SLOT_ID] = wafer.Slot.ToString();                                        //SLOT_ID
                            strDBData[(int)TQD_STEP.RESULT_ID] = parser.LotID;                                               //RESULT_ID
                            strDBData[(int)TQD_STEP.INSPECTION_EQ] = parser.Equip;                                           //INSPECTION_EQ
                            strDBData[(int)TQD_STEP.REVIEW_EQ] = "";                                                         //REVIEW_EQ
                            strDBData[(int)TQD_STEP.SETUP_SEQ] = SetupSeq.ToString();                                        //SETUP_SEQ
                            strDBData[(int)TQD_STEP.RESULTTIMESTAMP] = parser.ResultTimestamp.ToString("yyyyMMddHHmmss");    //RESULTTIMESTAMP
                            strDBData[(int)TQD_STEP.FILETIMESTAMP] = parser.FileTimestamp.ToString("yyyyMMddHHmmss");        //FILETIMESTAMP
                            strDBData[(int)TQD_STEP.MAIN_EQ] = parser.Equip;                                                 //MAIN_EQ	             
                            strDBData[(int)TQD_STEP.ROUTE] = " ";                                                            //ROUTE		             
                            strDBData[(int)TQD_STEP.OPER] = " ";                                                             //OPER				             
                            strDBData[(int)TQD_STEP.SCAN_AREA] = "-1";                                                       //SCAN_AREA
                            strDBData[(int)TQD_STEP.DEFECTIVE_DIE] = "-1";                                                   //DEFECTIVE_DIE
                            strDBData[(int)TQD_STEP.DEFECTS] = "-1";                                                         //DEFECTS
                            strDBData[(int)TQD_STEP.IMAGES] = "0";                                                           //IMAGES
                            strDBData[(int)TQD_STEP.DEFECT_DD] = "-1";                                                       //DEFECT_DD 
                            strDBData[(int)TQD_STEP.INSP_FILENAME] = Path.GetFileName(parser.FileName);                      //INSP_FILENAME Parsing 대상 File Name
                            strDBData[(int)TQD_STEP.INSP_FILEPATH] = parser.FtpRelativePath;                                 //INSP_FILEPATH, Backup 경로
                            strDBData[(int)TQD_STEP.COMMENT] = "Parser";	                                                 //COMMENT;	 

                            StepSeq = oDefectDefine.GetStepInfo(new string[] { WaferSeq.ToString(), strStepID, parser.ResultTimestamp.ToString("yyyyMMddHHmmss"), "1" }); ///WAFER_SEQ,STEP_ID,RESULTTIMESTAMP,TEST

                            //Data 가 있는지 확인 하고 없으면 생성 한다.
                            if (StepSeq < 0)
                            {
                                //-1로 Test No 를 생성 하고 이후에 Update 한다.
                                oDefectDefine.CreateStepSeq(strDBData);
                                StepSeq = oDefectDefine.GetStepInfo(new string[] { WaferSeq.ToString(), strStepID, parser.ResultTimestamp.ToString("yyyyMMddHHmmss"), "1" });

                                if (StepSeq < 0)
                                    throw new Exception("TQD_STEP 생성 실패");

                                bStepNew = true;
                            }

                            #endregion

                            #region [Step 07 : TQD_DEFECT, TQD_IMAGES 확인 생성]

                            //WriteLog("Step 07", "TQD_DEFECT, TQD_IMAGES 확인 생성");
                            //Data 존재 확인 후 없으면 생성 한다.
                            dtTemp = oDefectDefine.GetDefectDataCount(WaferSeq.ToString(), StepSeq.ToString());
                            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                            {
                                Dictionary<string, int> oDicDecfect = new Dictionary<string, int>();
                                string[,] DefectPara = new string[wafer.DefectList.Count, Enum.GetNames(typeof(TQD_DEFECT)).Length];

                                for (int ir = 0; ir < wafer.DefectList.Count; ir++)
                                {
                                    double dX = wafer.DefectList[ir].X;
                                    double dY = wafer.DefectList[ir].Y;

                                    // 클러스터 계산을 직접 해야 하는 경우 먼저 값을 초기화한다.
                                    if (handler.NeedsCalculateCluster())
                                        wafer.DefectList[ir].CLUSTERNUMBER = 0;

                                    //X, Y 값이 없을 경우 임의로 만들어 준다.
                                    //계산식 : Center Die 의 Centerloaction 의 좌표 정보를 기준으로 현재 Defect 의 위치 정보를 빼준다.
                                    if (dX == 0)
                                        dX = (double)(wafer.DefectList[ir].XINDEX * parser.DiePitchX + wafer.DefectList[ir].XREL) - dCenterXLocation;

                                    if (dY == 0)
                                        dY = (double)(wafer.DefectList[ir].YINDEX * parser.DiePitchY + wafer.DefectList[ir].YREL) - dCenterYLocation;
                                }

                                // 마지막 Step의 Defect 데이터가 있는 경우 NEW DEFECT 계산
                                DataTable prevXYDt = oDefectDefine.GetLastestStepDefectXY(wafer.WaferID, strStepID, parser.ResultTimestamp.ToString("yyyy-MM-dd HH:mm:ss"));

                                if (prevXYDt != null && prevXYDt.Rows.Count > 0)
                                {
                                    List<PointD> list = new List<PointD>();

                                    foreach (DataRow row in prevXYDt.Rows)
                                        list.Add(new PointD((double)(decimal)row[0], (double)(decimal)row[1]));

                                    // New Defect 계산
                                    DACrux.Data.Parser.DefectHelper.CalculateNewDefect(wafer.DefectList, list, handler.GetNewDefectTolerance());
                                }
                                else
                                {
                                    foreach (Defect defect in wafer.DefectList)
                                        defect.ADDER = 1;
                                }

                                // CLUSTER GROUP 계산이 필요한 경우 처리
                                if (handler.NeedsCalculateCluster())
                                {
                                    int minClusterCount = handler.GetMinimunClusterCount();
                                    double clusterThreshold = handler.GetClusterThreadhold();

                                    DACrux.Data.Parser.DefectHelper.CalculateCluster(wafer.DefectList, minClusterCount, clusterThreshold);
                                }

                                for (int ir = 0; ir < wafer.DefectList.Count; ir++)
                                {
                                    DefectPara[ir, (int)TQD_DEFECT.STEP_SEQ] = StepSeq.ToString();                                        //STEP_SEQ
                                    DefectPara[ir, (int)TQD_DEFECT.DEFECTID] = wafer.DefectList[ir].DEFECTID.ToString();	              //DEFECTID
                                    DefectPara[ir, (int)TQD_DEFECT.WAFER_SEQ] = WaferSeq.ToString();                                      //WAFER_SEQ
                                    DefectPara[ir, (int)TQD_DEFECT.X] = wafer.DefectList[ir].X.ToString();                                //X
                                    DefectPara[ir, (int)TQD_DEFECT.Y] = wafer.DefectList[ir].Y.ToString();	                              //Y
                                    DefectPara[ir, (int)TQD_DEFECT.XREL] = wafer.DefectList[ir].XREL.ToString();	                      //XREL
                                    DefectPara[ir, (int)TQD_DEFECT.YREL] = wafer.DefectList[ir].YREL.ToString();	                      //YREL
                                    DefectPara[ir, (int)TQD_DEFECT.XINDEX] = wafer.DefectList[ir].XINDEX.ToString();                      // XINDEX
                                    DefectPara[ir, (int)TQD_DEFECT.YINDEX] = wafer.DefectList[ir].YINDEX.ToString();	                  // YINDEX
                                    DefectPara[ir, (int)TQD_DEFECT.XSIZE] = wafer.DefectList[ir].XSIZE.ToString();	                      //XSIZE
                                    DefectPara[ir, (int)TQD_DEFECT.YSIZE] = wafer.DefectList[ir].YSIZE.ToString();	  	  	  	          //YSIZE
                                    DefectPara[ir, (int)TQD_DEFECT.DEFECTAREA] = wafer.DefectList[ir].DEFECTAREA.ToString();	  	  	  //DEFECTAREA
                                    DefectPara[ir, (int)TQD_DEFECT.DSIZE] = wafer.DefectList[ir].DSIZE.ToString();    	  	  	          //DSIZE
                                    DefectPara[ir, (int)TQD_DEFECT.CLASSNUMBER] = wafer.DefectList[ir].CLASSNUMBER.ToString();            //CLASSNUMBER
                                    DefectPara[ir, (int)TQD_DEFECT.TEST] = wafer.DefectList[ir].TEST.ToString();                          //TEST
                                    DefectPara[ir, (int)TQD_DEFECT.CLUSTERNUMBER] = wafer.DefectList[ir].CLUSTERNUMBER.ToString();        //CLUSTERNUMBER
                                    DefectPara[ir, (int)TQD_DEFECT.ROUGHBINNUMBER] = wafer.DefectList[ir].ROUGHBINNUMBER.ToString();      //ROUGHBINNUMBER	
                                    DefectPara[ir, (int)TQD_DEFECT.FINEBINNUMBER] = wafer.DefectList[ir].FINEBINNUMBER.ToString();        //FINEBINNUMBER
                                    DefectPara[ir, (int)TQD_DEFECT.REVIEWSAMPLE] = wafer.DefectList[ir].REVIEWSAMPLE.ToString();          //REVIEWSAMPLE	
                                    DefectPara[ir, (int)TQD_DEFECT.IMAGECOUNT] = wafer.DefectList[ir].IMAGECOUNT.ToString();              //IMAGECOUNT
                                    DefectPara[ir, (int)TQD_DEFECT.ADDER] = wafer.DefectList[ir].ADDER.ToString();                        //ADDER
                                    DefectPara[ir, (int)TQD_DEFECT.FIRST_STEP] = wafer.DefectList[ir].FIRST_STEP == null ? "" : wafer.DefectList[ir].FIRST_STEP.ToString(); //FIRST_STEP
                                    DefectPara[ir, (int)TQD_DEFECT.RETICLE_REPEAT_ID] = wafer.DefectList[ir].RETICLE_REPEAT_ID.ToString();//RETICLE_REPEAT_ID
                                    DefectPara[ir, (int)TQD_DEFECT.DIE_REPEAT_ID] = wafer.DefectList[ir].DIE_REPEAT_ID.ToString();        //DIE_REPEAT_ID
                                    DefectPara[ir, (int)TQD_DEFECT.MAN_OPT_CLASS] = wafer.DefectList[ir].MAN_OPT_CLASS.ToString();        //MAN_OPT_CLASS
                                    DefectPara[ir, (int)TQD_DEFECT.AUTO_OPT_CLASS] = wafer.DefectList[ir].AUTO_OPT_CLASS.ToString();      //AUTO_OPT_CLASS
                                    DefectPara[ir, (int)TQD_DEFECT.MAN_SEM_CLASS] = wafer.DefectList[ir].MAN_SEM_CLASS.ToString();        //MAN_SEM_CLASS
                                    DefectPara[ir, (int)TQD_DEFECT.AUTO_SEM_CLASS] = wafer.DefectList[ir].AUTO_SEM_CLASS.ToString();      //AUTO_SEM_CLASS
                                }

                                if (wafer.DefectList.Count > 0)
                                {
                                    //속도 문제 때문에 Bulk Insert 한다.
                                    oDefectDefine.CreateDefect(DefectPara);

                                    dtTemp = oDefectDefine.GetDefectDataCount(WaferSeq.ToString(), StepSeq.ToString());
                                }
                            }

                            //NDEFDIE 값 처리 (Defect 발생 die 개수)
                            if (dtTemp != null && dtTemp.Rows.Count > 0)
                            {
                                DataTable dtXYGroup = dtTemp.DefaultView.ToTable(true, "XY");
                                iNDEFDIE = dtXYGroup.Rows.Count;

                                iClassifiedCnt = dtTemp.Select("CLASSNUMBER <> 0").Length;
                            }

                            #endregion

                            #region [Step 08 : TQD_INSP_INFO 의 정보가 있는지 확인 후 없으면 저장 한다. Key 값은 Step Seq 에 Test No 다.]

                            //WriteLog("Step 08", "TQD_INSP_INFO 의 정보가 있는지 확인 후 없으면 저장 한다.");

                            foreach (var oData in wafer.TestList)
                            {
                                dtTemp = oDefectDefine.GetInspInfoDuple(StepSeq.ToString(), oData.TestNo.ToString());
                                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                                {
                                    /// 각각의 Test ID 별 tqd_insp_info 테이블에 정보 등록
                                    DefectList defects = new DefectList();
                                    defects.AddRange(wafer.DefectList.FindAll(delegate(Defect d) { return d.TEST == oData.TestNo; }));
                                    /// Classified Defect Count
                                    iClassifiedCnt = defects.FindAll(delegate(Defect d) { return d.CLASSNUMBER > 0; }).Count;

                                    dDEFDENSITY = (double)((defects.Count / oData.AreaPerTest) * 100000000);
                                    dAreaTest = (double)(oData.AreaPerTest / 100000000);

                                    DefectStat stat = defects.GetDefectStat();

                                    oDefectDefine.CreateInspData(new string[]{   StepSeq.ToString()		                                             //STEP_SEQ                  
                                                                        ,WaferSeq.ToString()				            	                         //WAFER_SEQ
                                                                        ,parser.ResultTimestamp.ToString("yyyyMMddHHmmss")                           //RESULTTIMESTAMP   
                                                                        ,parser.FileTimestamp.ToString("yyyyMMddHHmmss")	                         //FILETIMESTAMP
                                                                        ,this.Factory                                                                //Factory
                                                                        ,"NONE"									                                     //TECHNOLOGY
                                                                        ,parser.DeviceID                                                             //PRODUCT     
                                                                        ,parser.LotID		                                                         //LOT_ID
                                                                        ,wafer.WaferID			                                                     //WAFER_ID
                                                                        ,strStepID                                                                   //STEP_ID
                                                                        ,oData.TestNo.ToString()	                                                 //TEST 
                                                                        ,wafer.Slot.ToString()			                                             //SLOT_ID
                                                                        ,SetupSeq.ToString()					                                     //SETUP_SEQ             
                                                                        ,oTime.ToString("yyyyMMddHHmmss")					                         //STATUS
                                                                        ,parser.Equip                                                                //strMainEQ	
                                                                        ,""		                                                                     //strRoute								   
                                                                        ,""		                                                                     //strOper								
                                                                        ,parser.Equip                                                                //INSPECTION_EQ            
                                                                        ,(wafer.DefectHeaders != null) ? String.Join(",", wafer.DefectHeaders) : ""  //INSPECTION_PARAM
                                                                        ,defects.Count.ToString()                                                    //DEFECTS // oData.Summary == null ? "0" : oData.Summary.NDEFECT.ToString()
                                                                        ,iClassifiedCnt.ToString()		                                             //CLASSIFIED_DEFECTS    
                                                                        ,"0"							                                             //KILLER_DEFECTS        
                                                                        ,stat.Random.ToString()                    	                                 //RANDOM_DEFECTS
                                                                        ,"0"							                                             //ADDER_KILLER_DEFECT
                                                                        ,stat.NewRandom.ToString()	                                                 //ADDER_RANDOM_DEFECT
                                                                        ,stat.Cluster.ToString()                            						 //CLUSTERS           
                                                                        ,stat.NewCluster.ToString()		                                             //ADDER_CLUSTERS     
                                                                        ,stat.AreaCluster.ToString()					   		                     //CLUSTER_AREA       
                                                                        ,Math.Round(dDEFDENSITY, 5).ToString()                                                      //DEFECT_DD          
                                                                        ,"0"					   		                                             //KILLER_DEFECT_DD   
                                                                        ,(stat.AreaRandom/dAreaTest).ToString()					   		             //RANDOM_DEFECT_DD   
                                                                        ,oData.SampleTestPlan.Length.ToString()                                      //INSPECTED_DIE  //oData.Summary == null ? "0" : oData.Summary.NDIE.ToString()      
                                                                        ,iNDEFDIE.ToString()                                                         //DEFECTIVE_DIE      // oData.Summary == null ? "0" : oData.Summary.NDEFDIE.ToString()
                                                                        ,"0"                                                                         //ADDER_DEF_DIE  // oData.Summary == null ? "0" : oData.Summary.NDEFDIE.ToString()    
                                                                        ,"0"					   		                                             //KILL_DEF_DIE       
                                                                        ,"0"					   		                                             //KILL_ADDER_DEF_DIE 0
                                                                        ,"0"                                                                         //IMAGES
                                                                        ,Math.Round(dAreaTest, 5).ToString()                                                        //SCAN_AREA
                                                                        ,stat.New.ToString()                                                         //ADDER_DEFECTS      
                                                                        ,"0"									                                     //KILL_RND_DEFECTS 
                                                                        ,"Parser"                                                                    //Commnet
                                                                        ,""			                                                                 //RETICLE_ID			
                                                                        ,""                                                                          //INTERLOCK_FLAG
                                                                        ,""						                                                     //STEPPER_EQ
                                                                    });
                                }
                            }

                            #endregion

                            #region [Step 09 : TQD_STEP 상의 TEST_ORDER 를 Update 시켜 준다.]
                            //          기준은 WAFER_SEQ,STEP_ID 이다.
                            //          신규로 생성이 되었을 경우만 Update 한다.

                            if (bStepNew)
                                oDefectDefine.UpdateStepTestNo(WaferSeq.ToString(), strStepID);

                            #endregion

                            #region [Step 10 : 같이 업로드된 이미지가 있는 경우 처리한다.]

                            string[,] arr = handler.GetImage2DArray(wafer, "INSP");

                            for (int idx = 0; idx < arr.GetLength(0); idx++)
                            {
                                arr[idx, (int)HandlerInsp.ImageCol.STEP_SEQ] = StepSeq.ToString();
                                arr[idx, (int)HandlerInsp.ImageCol.WAFER_SEQ] = WaferSeq.ToString();

                            }

                            if (arr != null && arr.GetLength(0) > 0)
                            {
                                BSL.DMReview review = new BSL.DMReview();
                                review.InsertDefectImage(arr);
                            }

                            #endregion

                            #region [Step 11 : TQD_DEFECT의 IMAGECOUNT를 일괄 업데이트 한다.]

                            oDefectDefine.UpdateImageCount(StepSeq);

                            #endregion

                            #region [Step 12 : TQD_INSP_SUM 테이블에 SUMMARY 데이터를 생성]

                            // TQD_INSP_SUM 테이블에 SUMMARY 데이터를 생성한다.
                            oDefectDefine.CreateInspSum(StepSeq.ToString());

                            #endregion

                            #region [Step 13 : DM ALARM 체크]

                            DefectAlarm alarm = new DefectAlarm();
                            alarm.CheckAlarm(handler.EquipInfo.Factory, parser.DeviceID, strStepID, handler.EquipInfo.EquipID, StepSeq.ToString(), wafer);

                            #endregion
                        }

                        StopWatch.Stop();

                        // 백업
                        handler.Backup(parser);

                        string[] copyPathArray = GetAdditionalCopyPathArray();

                        // 추가로 데이터 및 원본 이미지 파일을 복사해야 할 경우
                        if (copyPathArray != null && copyPathArray.Length > 0)
                        {
                            try
                            {
                                foreach (string copyPath in copyPathArray)
                                    handler.AddtionalCopy(parser, copyPath);
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }

                        // KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // 설비 별로 임시로 데이터 및 원본 이미지 파일을 복사해야 할 경우 2019.10.24
                        if (!String.IsNullOrWhiteSpace(handler.EquipInfo.Grp01))
                        {
                            try
                            {
                                handler.AddtionalCopy(parser, handler.EquipInfo.Grp01);

                                /*// 해당 LOT ID로 .trf 파일이 있는 경우 복사한다. 이 파일 없으면 review 파일을 못 만든다고 함. 한동은 수석 요청 2019.10.27
                                foreach (string file in Directory.GetFiles(parser.DirectoryName, "LOTEND*.trf"))
                                {
                                    if (DACrux.Data.Parser.ParsingUtil.FileToString(file).Contains(String.Format("LotID \"{0}\";", parser.LotID)))
                                    {
                                        handler.FileCopy(file, Path.Combine(handler.EquipInfo.Grp01, Path.GetFileName(file)));

                                        // 복사 후에는 trf 파일을 삭제한다.
                                        File.Delete(file);
                                        break;
                                    }
                                }*/
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }
                        // END : KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // REVIEW 할 폴더로 파일을 복사해야 하는 경우 //(REVIEW 폴더 경로가 있고 LOTEND 시 파일 생성하도록 지정되어 있지 않을때)
                        if (!String.IsNullOrEmpty(handler.ReviewCopyPath))// && !handler.EquipInfo.IsCreateReviewFileAtLotEnd())
                        {
                            handler.CopyReviewKlarfFile(parser);
                        }

                        // 원본 삭제
                        handler.Remove(parser);

                        WriteLog("SAVE", String.Format("FileName={0}, LotID={1}, WaferID={2}, Device={3}, ExecuteTime={4}",
                            Path.GetFileName(parser.FileName), parser.LotID, parser.Wafers, parser.DeviceID, StopWatch.ExecuteTime));

                        AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name, StopWatch.TotalMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        // ErrorFlag를 설정하여 데이터 파일이 백업이나 삭제가 되지 않도록 한다.
                        parser.ErrorFlag = true;

                        WriteLog(ex);

                        FileInfo FileError = new FileInfo(parser.FileName);

                        //Error 발생된 File 의 경우 Error 경로에 넣는다.
                        if (parser.ErrorBackupFlag == true && FileError.Exists)
                        {
                            string strBacupPath = GetErrorFullPath(FileError.Name, handler.EquipInfo.EquipID);
                            if (string.IsNullOrEmpty(strBacupPath) == false)
                            {
                                //File 을 Error 경로에 넣는다.
                                try
                                {
                                    FileInfo FileBackup = new FileInfo(strBacupPath);

                                    try
                                    {
                                        if (FileBackup.Exists && FileBackup.IsReadOnly)
                                            FileBackup.IsReadOnly = false;
                                    }
                                    catch { }

                                    File.Copy(FileError.FullName, FileBackup.FullName, true);

                                    try
                                    {
                                        if (FileError.IsReadOnly)
                                            FileError.IsReadOnly = false;
                                    }
                                    catch { }

                                    FileError.Delete();
                                }
                                catch { }

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                        }
                    }
                }

                //
                // 일반 KLARF 파일과 TRF KLARF 파일을 따로 처리한다. 2019.11.04 Taihi,Kim.
                //
                foreach (DACrux.Data.Parser.Klarf.ParserKlarf parser in handler.ParserList)
                {
                    // trf KLARF 파일만 처리
                    if (parser.GetType() != typeof(DACrux.Data.Parser.Klarf.ParserKlarf_Trf))
                        continue;

                    try
                    {
                        if (IsReqeustServiceStop)
                            break;

                        if (parser.ErrorFlag)
                            throw new Exception(parser.ErrorMessage);
                        else
                            parser.ErrorBackupFlag = true;

                        StopWatch.Start();

                        DMReview oReview = new DMReview();

                        foreach (var wafer in parser.Wafers)
                        {
                            long waferseq, stepseq;

                            // FAB1 .trf 파일의 경우 STEP_ID  변경을 위한 SETUP_ID 데이터가 없으므로
                            // 부득이하게 STEP_ID LIKE 검색을 한다.
                            bool bFlag;

                            if (Factory == FAB1)
                            {
                                bFlag = oReview.GetInspectionInfo_02(
                                    Factory,
                                    parser.ResultTimestamp,
                                    parser.LotID,
                                    wafer.Slot.ToString(),
                                    parser.StepID,
                                    out waferseq,
                                    out stepseq
                                    );
                            }
                            else
                            {
                                bFlag = oReview.GetInspectionInfo_01(
                                    Factory,
                                    parser.ResultTimestamp,
                                    parser.LotID,
                                    wafer.Slot.ToString(),
                                    parser.StepID,
                                    out waferseq,
                                    out stepseq
                                    );
                            }

                            if (!bFlag)
                            {
                                parser.ErrorFlag = true;
                                parser.ErrorBackupFlag = false;
                                throw new Exception("Inspection 정보가 없습니다.");
                            }

                            string[,] arr = handler.GetImage2DArray(wafer, "INSP");

                            for (int idx = 0; idx < arr.GetLength(0); idx++)
                            {
                                arr[idx, (int)HandlerInsp.ImageCol.TEST] = "1";
                                arr[idx, (int)HandlerInsp.ImageCol.STEP_SEQ] = stepseq.ToString();
                                arr[idx, (int)HandlerInsp.ImageCol.WAFER_SEQ] = waferseq.ToString();
                            }

                            if (arr != null && arr.GetLength(0) > 0)
                            {
                                oReview.InsertDefectImage(arr);
                            }

                            arr = new string[wafer.ImageDefectList.Count, 8];

                            for (int idx = 0; idx < wafer.ImageDefectList.Count; idx++)
                            {
                                int colIdx = 0;
                                arr[idx, colIdx++] = stepseq.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].DEFECTID.ToString();
                                arr[idx, colIdx++] = waferseq.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].CLASSNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].ROUGHBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].FINEBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].REVIEWSAMPLE.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].IMAGECOUNT.ToString();
                            }

                            oReview.SaveDefect(
                                stepseq,
                                waferseq,
                                wafer.ImageDefectList.Count,
                                handler.EquipInfo.EquipID,
                                Path.GetFileName(parser.FileName),
                                parser.FtpRelativePath,
                                arr
                                );

                            arr = new string[wafer.ClassifedDefectList.Count, 7];

                            for (int idx = 0; idx < wafer.ClassifedDefectList.Count; idx++)
                            {
                                int colIdx = 0;
                                arr[idx, colIdx++] = stepseq.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].DEFECTID.ToString();
                                arr[idx, colIdx++] = waferseq.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].CLASSNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].ROUGHBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].FINEBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].REVIEWSAMPLE.ToString();
                            }

                            oReview.SaveDefect(
                                stepseq,
                                waferseq,
                                arr
                                );
                        }

                        //
                        // LOTEND 파일 때 Review 파일을 생성하도록 설정된 경우 (CMF07=Y) 
                        // 같은 LOT, EQUIP, ResultTimestamp 데이터를 모아 Review 설비로 파일을 내려줄 수 있도록 한다. 2019.11.24 Taihi,Kim.
                        //
                        if (handler.IsTrfKlarfLotEndFile(parser.FileName) && !String.IsNullOrEmpty(handler.ReviewCopyPath) && handler.EquipInfo.IsCreateReviewFileAtLotEnd())
                        {
                            long[] stepSeqArr = oDefectDefine.GetStepSeqBy(parser.LotID, parser.Equip, parser.ResultTimestamp.ToString("yyyy-MM-dd HH:mm:ss"));

                            if (stepSeqArr != null && stepSeqArr.Length > 0)
                            {
                                ParserKlarf export = null;
                                DefectMapAnalysis obj = new DefectMapAnalysis();

                                foreach (long stepSeq in stepSeqArr)
                                {
                                    Defect[] defectArr = DataTableToDefectArr(obj.GetDefectData(stepSeq));
                                    DmsWaferDieInfo info = DmsCache.Instance[stepSeq];

                                    ParserKlarf tmp = new ParserKlarf(defectArr, info);

                                    // Setup Map의 Angle이 0이 아니면 Review 설비에서 작업이 가능하도록 원래 각도로 돌려서 데이터를 내려준다. 2019.11.25 Taihi,Kim.
                                    if (info.StepInfo.Angle > 0)
                                    {
                                        tmp.RotateAndRecalculate(info.StepInfo.Angle, DieIndexSort.LowerLeftToCenter);
                                        tmp.OrientationMarkLocation = ParserKlarf.AngleToOrientationMarkLocation(info.StepInfo.Angle);
                                    }

                                    // 1개 Parser에 Wafer 추가
                                    if (export == null)
                                        export = tmp;
                                    else
                                        export.Wafers.Add(tmp.Wafers[0]);
                                }

                                // REVIEW 할 폴더로 파일을 복사해야 하는 경우
                                if (export != null)
                                {
                                    // 파일명은 LOTID_STEPID 로 한다.
                                    export.FileName = String.Format("{0}_{1}.000", export.LotID, export.StepID);

                                    handler.MakeReviewKlarfFile(export);
                                }
                            }
                        }

                        StopWatch.Stop();

                        // 백업
                        handler.Backup(parser);

                        string[] copyPathArray = GetAdditionalCopyPathArray();

                        // 추가로 데이터 및 원본 이미지 파일을 복사해야 할 경우
                        if (copyPathArray != null && copyPathArray.Length > 0)
                        {
                            try
                            {
                                foreach (string copyPath in copyPathArray)
                                    handler.AddtionalCopy(parser, copyPath);
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }

                        // KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // 설비 별로 임시로 데이터 및 원본 이미지 파일을 복사해야 할 경우 2019.10.24
                        if (!String.IsNullOrWhiteSpace(handler.EquipInfo.Grp01))
                        {
                            try
                            {
                                handler.AddtionalCopy(parser, handler.EquipInfo.Grp01);
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }
                        // END : KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // 원본 삭제
                        handler.Remove(parser);

                        WriteLog("SAVE", String.Format("FileName={0}, LotID={1}, WaferID={2}, Device={3}, ExecuteTime={4}",
                            Path.GetFileName(parser.FileName), parser.LotID, parser.Wafers, parser.DeviceID, StopWatch.ExecuteTime));

                        AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name, StopWatch.TotalMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        // ErrorFlag를 설정하여 데이터 파일이 백업이나 삭제가 되지 않도록 한다.
                        parser.ErrorFlag = true;

                        WriteLog(ex);

                        FileInfo FileError = new FileInfo(parser.FileName);

                        //Error 발생된 File 의 경우 Error 경로에 넣는다.
                        if (parser.ErrorBackupFlag == true && FileError.Exists)
                        {
                            string strBacupPath = GetErrorFullPath(FileError.Name, handler.EquipInfo.EquipID);
                            if (string.IsNullOrEmpty(strBacupPath) == false)
                            {
                                //File 을 Error 경로에 넣는다.
                                try
                                {
                                    FileInfo FileBackup = new FileInfo(strBacupPath);

                                    try
                                    {
                                        if (FileBackup.Exists && FileBackup.IsReadOnly)
                                            FileBackup.IsReadOnly = false;
                                    }
                                    catch { }

                                    File.Copy(FileError.FullName, FileBackup.FullName, true);

                                    try
                                    {
                                        if (FileError.IsReadOnly)
                                            FileError.IsReadOnly = false;
                                    }
                                    catch { }

                                    FileError.Delete();
                                }
                                catch { }

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                        }
                    }
                }
            }
        }

        private string[] GetDefectColumnsOrder()
        {
            // 명칭은 Defect 클래스의 변수명과 같도록 설정할것
            // 순서는 Insert 쿼리의 컬럼 순서와 맞출것
            return new string[]
            {
                "STEP_SEQ",
                "DEFECTID",
                "WAFER_SEQ",
                "X",
                "Y",
                "XREL",
                "YREL",
                "XINDEX",
                "YINDEX",
                "XSIZE",
                "YSIZE",
                "DEFECTAREA",
                "DSIZE",
                "CLASSNUMBER",
                "TEST",
                "CLUSTERNUMBER",
                "ROUGHBINNUMBER",
                "FINEBINNUMBER",
                "REVIEWSAMPLE",
                "IMAGECOUNT",
                "IMAGESEQ",
                "ADDER",
                "FIRST_STEP",
                "RETICLE_REPEAT_ID",
                "DIE_REPEAT_ID",
                "MAN_OPT_CLASS",
                "AUTO_OPT_CLASS",
                "MAN_SEM_CLASS",
                "AUTO_SEM_CLASS"
            };
        }

        private Defect[] DataTableToDefectArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            Defect[] defectArr = new Defect[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                Defect defect = new Defect();

                defect.STEP_SEQ = DACrux.Base.Convert.intParse(r["STEP_SEQ"].ToString());
                defect.DEFECTID = DACrux.Base.Convert.intParse(r["DEFECTID"].ToString());
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(r["WAFER_SEQ"].ToString());
                defect.X = DACrux.Base.Convert.doubleParse(r["X"].ToString());
                defect.Y = DACrux.Base.Convert.doubleParse(r["Y"].ToString());
                defect.XREL = DACrux.Base.Convert.doubleParse(r["XREL"].ToString());
                defect.YREL = DACrux.Base.Convert.doubleParse(r["YREL"].ToString());
                defect.XINDEX = DACrux.Base.Convert.intParse(r["XINDEX"].ToString());
                defect.YINDEX = DACrux.Base.Convert.intParse(r["YINDEX"].ToString());
                defect.XSIZE = DACrux.Base.Convert.doubleParse(r["XSIZE"].ToString());
                defect.YSIZE = DACrux.Base.Convert.doubleParse(r["YSIZE"].ToString());
                defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(r["DEFECTAREA"].ToString());
                defect.DSIZE = DACrux.Base.Convert.doubleParse(r["DSIZE"].ToString());
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                defect.TEST = DACrux.Base.Convert.intParse(r["TEST"].ToString());
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(r["CLUSTERNUMBER"].ToString());
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(r["IMAGECOUNT"].ToString());
                defect.ADDER = DACrux.Base.Convert.intParse(r["ADDER"].ToString());

                defectArr[i] = defect;
            }

            return defectArr;
        }

        private DmsWaferDieInfo GetWaferDieInfo(long stepSeq)
        {
            DefectMapAnalysis oMap = new DefectMapAnalysis();
            DataSet ds = oMap.GetDefectMapInfo(stepSeq);

            if (ds == null || ds.Tables.IndexOf("MAP") < 0 || ds.Tables.IndexOf("DIE") < 0)
                return null;

            if (ds.Tables["MAP"].Rows.Count == 0)
                return null;

            DataRow mapRow = ds.Tables["MAP"].Rows[0];
            DmsStepInfo step = new DmsStepInfo();

            step.Maker = mapRow["MAKER"].ToString();
            step.Model = mapRow["MODEL"].ToString();
            step.Equip = mapRow["EQUIP"].ToString();
            step.ResultTimestamp = mapRow["RESULTTIMESTAMP"].ToString();
            step.LotID = mapRow["LOT_ID"].ToString();
            step.WaferID = mapRow["WAFER_ID"].ToString();
            step.WaferSize = DACrux.Base.Convert.intParse(mapRow["WAFER_SIZE"].ToString());
            step.SampleSize = step.WaferSize / 1000 / 1000;
            step.DeviceID = mapRow["PRODUCT"].ToString();
            step.SetupID = mapRow["SETUP_ID"].ToString();
            step.SetupTimestamp = mapRow["SETUP_TIME"].ToString();
            step.StepID = mapRow["STEP_ID"].ToString();
            step.SampleOrientationMarkType = "NOTCH";
            step.DiePitchX = DACrux.Base.Convert.doubleParse(mapRow["DIE_PITCH_X"].ToString());
            step.DiePitchY = DACrux.Base.Convert.doubleParse(mapRow["DIE_PITCH_Y"].ToString());
            step.DieOriginX = DACrux.Base.Convert.intParse(mapRow["DIE_ORIGIN_X"].ToString());
            step.DieOriginY = DACrux.Base.Convert.intParse(mapRow["DIE_ORIGIN_Y"].ToString());
            step.Slot = DACrux.Base.Convert.intParse(mapRow["SLOT_ID"].ToString());
            step.Angle = DACrux.Base.Convert.intParse(mapRow["ANGLE"].ToString());
            step.SampleCenterLocationX = DACrux.Base.Convert.doubleParse(mapRow["ORIGIN_X"].ToString());
            step.SampleCenterLocationY = DACrux.Base.Convert.doubleParse(mapRow["ORIGIN_Y"].ToString());
            step.AreaPerTest = DACrux.Base.Convert.doubleParse(mapRow["SCAN_AREA"].ToString());
            step.Inspector = mapRow["INSPECTION_EQ"].ToString();
            step.ShotArrayX = DACrux.Base.Convert.intParse(mapRow["ST_XCNT"].ToString(), 1);
            step.ShotArrayY = DACrux.Base.Convert.intParse(mapRow["ST_YCNT"].ToString(), 1);
            step.ShotStartX = DACrux.Base.Convert.intParse(mapRow["ST_START_X"].ToString(), 1);
            step.ShotStartY = DACrux.Base.Convert.intParse(mapRow["ST_START_Y"].ToString(), 1);

            DataTable dieDt = ds.Tables["DIE"];
            Point[] dies = new Point[dieDt.Rows.Count];

            for (int i = 0; i < dieDt.Rows.Count; i++)
            {
                dies[i] = new Point(DACrux.Base.Convert.intParse(dieDt.Rows[i]["INDEX_X"].ToString()), DACrux.Base.Convert.intParse(dieDt.Rows[i]["INDEX_Y"].ToString()));
            }

            return new DmsWaferDieInfo(stepSeq, step, dies);
        }
    }
}
