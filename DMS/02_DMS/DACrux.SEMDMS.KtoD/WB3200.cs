using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using DACrux.Base;
using DACrux.SEMDMS.DSL;
using System.Runtime.InteropServices;
using System.Data;

namespace DACrux.SEMDMS.KtoD
{

    public class WB3200 : Miracom.Middleware.BaseComponent
    {
        #region ■변수선언
        private const int RK = 1000000;
        private int OffSet = 180;

        private DACrux.Base.KLARF_HEADER_TAG m_oKHeader;
        private DACrux.Base.ARGUMENT_TAG m_oPath;
        private StreamReader m_srWB3200;
        private System.DateTime m_oStartTime;
        private int m_iDieCount;
        private int m_iDefectCount;
        private int m_iMaxDefects;
        private int[] m_iParmSpecIndex;
        //private bool m_bMultiWaferCutLine =false;
        private string m_strFACILITY;
        private string m_strSubParse;
        private string m_strLinkedPath;
        private int m_iTotalImageCount;
        public ArrayList m_arrImgSourceList;
        private int m_SocketPort;
        private string m_UpdateServerIP;
        private string m_strDataIntegrate;
        private long m_lCurrentStepSeq = -1;

        private bool m_bServiceFlag = false;
        private bool m_bCommonServiceFlag = true;
        private int m_iBackupFileLifeDate;

        private string m_MainEq = string.Empty;
        private string m_RTE = string.Empty;
        private string m_Oper = string.Empty;
        private string m_ReticleID = string.Empty;
        private string m_StepperEq = string.Empty;
        public string m_FAB = string.Empty;
        private int m_iDefectInsertSum = 100;

        public ArrayList m_SampleTestPlan = new ArrayList();
        public ArrayList m_SummaryList = new ArrayList();
        public ArrayList m_AreaPerTest = new ArrayList();
        public ArrayList m_TestParametersList = new ArrayList();
        public ArrayList m_InspectionTest = new ArrayList();
        public int m_CurrentTestIndex = 0;
        private int m_ImageCount = 0;
        private int m_iMaxX = 1;
        private int m_iMaxY = 1;

        #endregion

        #region ■필요 Interface 정의


        public string DataIntegrateType
        {
            get
            {
                return m_strDataIntegrate;
            }
        }

        public string EQUIP_ID
        {
            get
            {
                return m_oKHeader.InspectionStationID[2];
            }
        }

        public string LOT_ID
        {
            get
            {
                return m_oKHeader.LotID;
            }
        }

        public string WAFER_ID
        {
            get
            {
                return m_oKHeader.WaferID;
            }
        }

        public string STEP_ID
        {
            get
            {
                return m_oKHeader.StepID;
            }
        }

        public string UPDATESERVERIP
        {
            get
            {
                return m_UpdateServerIP;
            }
        }

        public int SOCKETPORT
        {
            get
            {
                return m_SocketPort;
            }
        }



        public bool EQ_SERVICE
        {
            get
            {
                return m_bServiceFlag;
            }
        }

        public bool COMMON_SERVICE
        {
            get
            {
                return m_bCommonServiceFlag;
            }
        }

        public int BACKUPFILELIFEDATE
        {
            get
            {
                return m_iBackupFileLifeDate;
            }
        }
        #endregion

        #region ■WB3200 Parsing 진입자
        public WB3200()
        {
            Logging.WriteLog("File Parsing Start  [WB3200]", "PROC", 0);
        }

        /// <summary>
        /// 생성자를 사용하지 못함
        /// 생성자와 비스므리한 함수로 만듬
        /// </summary>
        /// <param name="Arguments">실행하기위한정보</param>
        public void WB3200_A(DACrux.Base.ARGUMENT_TAG Arguments)
        {
            TQD_CONFIG oYesConfig = new TQD_CONFIG();
            DataTable dt = null;
            try
            {
                m_iParmSpecIndex = new int[27];
                m_iDieCount = 0;
                m_iDefectCount = 0;
                m_strFACILITY = "UNKNOWN";
                m_srWB3200 = null;
                m_strSubParse = string.Empty;
                //m_bMultiWaferCutLine = false;
                m_oPath = Arguments;
                m_oStartTime = DateTime.Now;
                m_UpdateServerIP = "127.0.0.1";
                m_iBackupFileLifeDate = 3;


                dt = oYesConfig.GetCategory("INTERFACE");
                if (dt.Rows.Count > 0)
                {
                    for (int cfgIdx = 0; cfgIdx < dt.Rows.Count; cfgIdx++)
                    {
                        switch (dt.Rows[cfgIdx]["NAME"].ToString())
                        {
                            case "MAXDEFECTS":
                                m_iMaxDefects = int.Parse(dt.Rows[cfgIdx]["VALUE"].ToString());
                                break;
                            case "IMAGEPATH":
                                m_strLinkedPath = dt.Rows[cfgIdx]["VALUE"].ToString();
                                break;
                            case "UPDATESERVERIP":
                                m_UpdateServerIP = dt.Rows[cfgIdx]["VALUE"].ToString();
                                break;
                            case "SOCKETPORT":
                                m_SocketPort = int.Parse(dt.Rows[cfgIdx]["VALUE"].ToString());
                                break;
                            case "LOGGINGLEVEL":
                                Logging.SetLogLevel(int.Parse(dt.Rows[cfgIdx]["VALUE"].ToString()));
                                break;
                            case "BACKUPFILELIFEDATE":
                                m_iBackupFileLifeDate = int.Parse(dt.Rows[cfgIdx]["VALUE"].ToString());
                                break;
                        }
                    }
                }
                else
                {
                    /// DB에서 읽기를 실패하면 Default값을 준다.
                    /// =========================================================================================
                    m_iMaxDefects = 1000000;
                    m_strLinkedPath = @"d:\temp";
                    /// =========================================================================================
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 0);
                throw this.ProcessErr(ex);
            }
            finally
            {
                oYesConfig.Dispose();
                if (dt != null) dt.Dispose();
            }
        }
        #endregion

        #region ■업데이트 서버의 WB3200 파일 읽기

        public void Read3200()
        {
            try
            {
                Read3200(m_oPath.SubProcessFile.Replace("D_Category", "DefMsr"), true);
                Read3200(m_oPath.SubProcessFile, true);
                ReadImage();
                Read3200(m_oPath.SubProcessFile.Replace("D_Category", "BumpHiDiaSt"), false, "DIE");
                Read3200(m_oPath.SubProcessFile.Replace("D_Category", "DefMsr"), false); //<--DB Action
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        private void ReadImage()
        {
            try
            {
                string ImagePath = Path.Combine(Path.GetDirectoryName(m_oPath.SubProcessFile), "PIC", m_oKHeader.WaferID);
                string[] ImageFiles = Directory.GetFiles(ImagePath, "*.jpg");

                string tmpFileName = "";
                int idx = -1;
                for (int i = 0; i < ImageFiles.Length; i++)
                {
                    tmpFileName = Path.GetFileNameWithoutExtension(ImageFiles[i]);
                    if (tmpFileName.ToUpper().EndsWith("PASS") != true)
                    {
                        if (int.TryParse(tmpFileName.Substring(0, 5), out idx) == false) continue;
                        AddIMAGE(ImageFiles[i], idx);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <b>■업데이트 서버의 WB3200 파일 읽기</b><br>
        /// - 작  성  자 : 미라콤 임영신<br>
        /// - 최초작성일 : 2013년 11월 07일<br>
        /// - 최종수정자 : 임영신<br>
        /// - 최종수정일 : 2013년 11월 07일<br>
        /// - 주요변경로그<br>
        /// </summary>
        /// <returns></returns>
        public void Read3200(string FileName, bool isPreParsing, string FileType = "DEFECT")
        {
            string strLine = string.Empty;
            int iLineCount = 0;
            DateTime oDtNow = DateTime.Now;
            string strEndTime = string.Empty;
            try
            {
                /// +
                /// +
                /// File을 Parsing하기 위해 Open을 한다.
                ///============================================================================================
                m_srWB3200 = new StreamReader(FileName);
                ///============================================================================================
                /// +
                /// +

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("File Open : [{0}]", m_oPath.SubProcessFile), "PROC", 3);
                ///--------------------------------------------------------------------------------------------


                while ((strLine = m_srWB3200.ReadLine()) != null)
                {
                    /// +
                    /// +
                    /// Line Count를 센다.
                    ///============================================================================================
                    iLineCount++;
                    ///============================================================================================
                    /// +
                    /// +

                    ///[LOG]---------------------------------------------------------------------------------------
                    Logging.WriteLog(string.Format("[{0}][{1}]", iLineCount, strLine), "PROC", 5);
                    ///--------------------------------------------------------------------------------------------

                    ReadLine(strLine.Trim(), isPreParsing, FileType);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                throw this.ProcessErr(ex);
            }

            finally
            {
                m_strSubParse = "";
                /// 1.Memory 해제
                ///============================================================================================
                m_srWB3200.Close();
                m_srWB3200 = null;

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog("사용된 Memory가 해제 되었습니다. ", "PROC", 5);
                ///--------------------------------------------------------------------------------------------
            }
        }
        #endregion

        #region ■Line을 읽으면서 공통으로 적용해야할 Rule정의
        private string[] SplitLine(string strLine)
        {
            string[] strWorkA;
            string[] strSplitResult;

            strLine = strLine.Replace("\t", " ");
            StringBuilder stbLine = new StringBuilder(256);
            StringBuilder stbWork = new StringBuilder(256);
            try
            {
                // 1. 따옴표 안의 내용을 하나로 만든다 역는 문자는 '@'

                strWorkA = strLine.Split('"');
                if (strWorkA.Length > 0)
                {
                    for (int i = 0; strWorkA.Length > i; i++)
                    {
                        strWorkA[i] = strWorkA[i].Trim();	//앞 뒤 공백을 없앤다.

                        if ((i % 2) == 1)
                        {
                            stbWork.Remove(0, stbWork.Length);
                            if (strWorkA[i].Length == 0) strWorkA[i] = " ";
                            stbWork.Append(strWorkA[i].Replace(" ", "_"));
                            stbWork.Replace('~', '_');
                            stbWork.Replace('!', '_');
                            //stbWork.Replace('@','_');


                            /// (2004-11-12) TWI201에서 Image File 이름에 #,^가 삽입됨
                            ///              따라서, # 을 허용함.
                            ///              
                            /// (2004-12-22) # 이 삽입된 Path는 Http로 Service할 수 없음
                            ///              따라서, 원래대로 다시 '_' 로 대치함
                            /// 

                            stbWork.Replace('#', '_');

                            stbWork.Replace('$', '_');
                            stbWork.Replace('%', '_');
                            //TWI201에서 Image File 이름에 #,^가 삽입됨(2004-11-12)
                            //stbWork.Replace('^','_');		
                            stbWork.Replace('&', '_');
                            stbWork.Replace('*', '_');
                            stbWork.Replace('?', '_');
                            stbLine.Append(stbWork.ToString());
                        }
                        else
                        {
                            stbLine.Append(strWorkA[i]);
                        }
                        stbLine.Append(" ");
                    }
                }
                else if (strWorkA.Length == 1)
                {
                    stbLine.Append(strWorkA[0]);
                }

                while (stbLine.ToString().IndexOf("  ") > -1)
                {
                    stbLine.Replace("  ", " ");
                }

                while (stbLine.ToString().IndexOf("   ") > -1)
                {
                    stbLine.Replace("   ", " ");
                }


                /// 1.Split완료
                ///============================================================================================
                strSplitResult = stbLine.ToString().Trim().Split(',');
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("Split된 Word [{0}]", stbLine.ToString().Trim().Replace(",", "").Replace(" ", "][")), "PROC", 5);
                ///--------------------------------------------------------------------------------------------
                return strSplitResult;
            }

            catch (Exception ex)
            {
                Logging.WriteLog(string.Format("[{0}]를 Split도중 Error가 발생하였습니다.[ERROR MSG : {1}]", strLine, ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                throw this.ProcessErr(ex);
            }

            finally
            {
                stbLine = null;
                stbWork = null;
            }
        }
        #endregion

        #region ■Line을 읽어 각각의 Parsing Logic으로 분배함
        private void ReadLine(string strLine, bool isPreParsing, string FileType = "DEFECT")
        {
            string[] strField;
            try
            {
                if (strLine.ToUpper().StartsWith("NO.") && FileType == "DEFECT")
                {
                    m_iDefectCount = 0;
                    m_iTotalImageCount = 0;
                }
                strField = SplitLine(strLine.Replace(";", "").Trim());
                if (strField.Length == 0) return;
                //Matrix X, 10
                //Matrix Y, 10
                switch (strField[0].ToUpper())
                {
                    case "START TIME":	//Start time, 2011-05-12 09:07:42
                        PrsFIELTIMESTAMP(strField);
                        break;
                    //case "TIFFFILENAME":	//TiffFileName img-30021.tif;
                    //    PrsTIFFFILENAME(strField);
                    //break;
                    //case "INSPECTIONSTATIONID": //InspectionStationID "KLA_TENCOR" "KLA2139" "QPE1041";
                    //    PrsINSPECTIONSTATIONID(strField);
                    //    break;
                    //case "SAMPLETYPE":		//SampleType WAFER;
                    //    PrsSAMPLETYPE(strField);
                    //    break;
                    case "END TIME":	//End time, 2011-05-12 09:13:54
                        PrsRESULTTIMESTAMP(strField);
                        break;
                    case "LOT":			//LOT,4E64243-01AA000_UBM  
                        //PrsLOTID(strField);
                        break;
                    case "WAFER SIZE":  //Wafer size, 8
                        PrsSAMPLESIZE(strField);
                        break;
                    case "PRODUCT": //Product, A994
                        PrsDEVICEID(strField);
                        break;
                    case "RECIPE":  //Recipe, 0
                        PrsSETUPID(strField);
                        break;
                    case "PRCPRM":  //A994_UBM_01   //OPR,1234 
                        PrsSTEPID(strField);
                        break;
                    case "JUDGMENT":
                        //PrsRESULTID(strField);
                        break;
                    case "SAMPLEORIENTATIONMARKTYPE":
                        PrsSAMPLEORIENTATIONMARKTYPE(strField);
                        break;
                    case "90-DEGREE ROTATION": //90-degree rotation, 0
                        PrsORIENTATIONMARKLOCATION(strField);
                        break;
                    case "CHIP SIZE X":    //Chip size X, 17.954000
                        PrsDIEPITCH(strField);
                        break;
                    case "CHIP SIZE Y":    //Chip size Y, 17.864000
                        PrsDIEPITCH(strField, false);
                        break;
                    case "DIEORIGIN":
                        PrsDIEORIGIN(strField);
                        break;
                    case "WID": //WID,4E64243-01
                        PrsWAFERID(strField, isPreParsing);
                        break;
                    case "SLOT":
                        PrsSLOT(strField);
                        break;
                    case "MATRIX X":
                        if (int.TryParse(strField[1].Trim(), out m_iMaxX) == false) m_iMaxX = 1;
                        break;
                    case "MATRIX Y":
                        if (int.TryParse(strField[1].Trim(), out m_iMaxY) == false) m_iMaxY = 1;
                        break;
                    //case "SAMPLECENTERLOCATION":
                    //    PrsSAMPLECENTERLOCATION(strField);
                    //    break;
                    //case "CLASSLOOKUP":
                    //    PrsCLASSLOOKUP(strField);
                    //    m_strSubParse = "CLASSLOOKUP";
                    //    break;
                    //case "INSPECTIONTEST":
                    //    PrsINSPECTIONTEST(strField);
                    //    break;
                    //case "INSEPCTIONTEST":    //  M6 TSV501 장비에서 올라올 때 철자가 틀리게 올라오는 사항임(추정연 대리님과 협의 완료, 2005.06.22)
                    //    PrsINSPECTIONTEST(strField);
                    //    break;
                    //case "SAMPLETESTPLAN":
                    //    PrsSAMPLETESTPLAN(strField);
                    //    m_strSubParse = "SAMPLETESTPLAN";
                    //    // 멀티 SampleTestPlan 인써트를 위해 추가 2005.05.27. by 강혜경
                    //    m_SampleTestPlan.Add(m_oKHeader.TestDieInfo);
                    //    break;
                    case "INSPECTION DIES": // Inspection dies,72
                        PrsSAMPLETESTPLAN(strField);
                        m_strSubParse = "INSPECTION DIES";
                        m_SampleTestPlan.Add(m_oKHeader.TestDieInfo);
                        break;
                    case "AREAPERTEST":
                        PrsAREAPERTEST(strField);
                        break;
                    case "TESTPARAMETERSSPEC":
                        PrsTESTPARAMETERSSPEC(strField);
                        m_strSubParse = "TESTPARAMETERSSPEC";
                        break;
                    case "TESTPARAMETERSLIST":
                        PrsTESTPARAMETERSLIST(strField);
                        m_strSubParse = "TESTPARAMETERSLIST";
                        break;
                    case "DEFECTCLUSTERSPEC":
                        PrsDEFECTCLUSTERSPEC(strField);
                        m_strSubParse = "DEFECTCLUSTERSPEC";
                        break;
                    //case "DEFECTRECORDSPEC":
                    //    PrsDEFECTRECORDSPEC(strField);
                    //    break;
                    case "NO.":
                        if (FileType == "DEFECT")
                        {
                            m_strSubParse = "No.";
                            PrsDEFECTRECORDSPEC(strField);
                        }
                        break;
                    //case "SUMMARYSPEC":
                    //    PrsSUMMARYSPEC(strField);
                    //    m_strSubParse = "SUMMARYSPEC";
                    //    break;
                    //case "SUMMARYLIST":
                    //    m_strSubParse = "SUMMARYLIST";
                    //    break;
                    case "END":
                        ///Summary Spec List가 없는 경우 ===============================================
                        ///예) krf,trf
                        ///
                        if (!isPreParsing)
                        {
                            if (m_oKHeader.SummaryList == null || m_oKHeader.SummaryList.Length == 0)
                            {
                                //InsertNewInsp();
                                if (InsertNewInsp() == -1)
                                    InsertNewInsp();
                            }
                            ///=============================================================================
                            PrsENDOFFILE();
                        }
                        break;
                    default:
                        switch (m_strSubParse)
                        {
                            case "INSPECTION DIES":
                                if (strField[0] == "No.") break;
                                SubPrsSAMPLETESTPLAN(strField);
                                break;
                            //case "TESTPARAMETERSSPEC":
                            //    SubPrsTESTPARAMETERSSPEC(strField);
                            //    break;
                            //case "TESTPARAMETERSLIST":
                            //    SubPrsTESTPARAMETERSLIST(strField);
                            //    break;
                            //case "DEFECTCLUSTERSPEC":
                            //    SubPrsDEFECTCLUSTERSPEC(strField);
                            //    break;
                            case "No.":
                                if (FileType == "DEFECT")
                                {
                                    AddDefect(strField);
                                }
                                else if (FileType == "DIE")
                                {
                                    SubPrsSAMPLETESTPLAN(strField);
                                }
                                break;
                            //case "SUMMARYSPEC":
                            //    SubPrsSUMMARYSPEC(strField);
                            //    break;
                            //case "SUMMARYLIST":
                            //    SubPrsSUMMARYLIST(strField);
                            //    break;
                            //case "CLASSLOOKUP":
                            //    SubPrsCLASSLOOKUP(strField);
                            //    break;
                            default:
                                break;
                        }
                        break;
                }

                //if(strLine.IndexOf(";")>0) m_strSubParse = "";
                //if (strLine.IndexOf(";") > 0)
                //{
                //    if (m_strSubParse.Equals("SUMMARYLIST"))
                //    {
                //        m_bMultiWaferCutLine = true;
                //    }
                //    m_strSubParse = "";
                //}
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region FileTimestamp 09-08-04 00:48:28;
        /// <summary>
        /// FileTimestamp Line을 Parsing함
        /// </summary>
        /// <param name="RLine">[FileTimestamp] [09-08-04] [00:48:28]</param>
        public void PrsFIELTIMESTAMP(string[] RLine)
        {
            StringBuilder stbRLine = new StringBuilder(256);
            try
            {
                for (int i = 1; i < RLine.Length; i++)
                {
                    stbRLine.Append(RLine[i]);
                }

                string strDate = "";
                string strTime = "";

                string i_DateStyle = DACrux.Base.Format.GetDateString(ref strDate, stbRLine.ToString());
                string i_TimeStyle = DACrux.Base.Format.GetTimeString(ref strTime, stbRLine.ToString());

                m_oKHeader.FileTimestamp = strDate + " " + strTime;
            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("FileTimeStamp Parsing Error [ERROR MSG][{1}]", RLine[1], ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
                stbRLine = null;
            }
        }
        #endregion

        #region TiffSpec        6.0     R       NA;
        /// <summary>
        /// 의미가 없음 형식상 존재
        /// </summary>
        /// <param name="RLine"></param>
        private void PrsTIFFSPEC(string[] RLine)
        {
            //	int iSpace = line.IndexOf(' ');
            //	rt_value = line.Substring(i_space+1).Trim();
        }
        #endregion

        //#region TiffFilename	TOB0001_0220040908005148.t01;
        //public void PrsTIFFFILENAME(string[] RLine)
        //{
        //    DateTime dtLoggingTime = DateTime.Now;
        //    try
        //    {
        //        string strParentPath = Path.GetDirectoryName(m_oPath.ProcessPath);
        //        File.Move(string.Format(@"{0}\{1}", strParentPath, RLine[1])
        //            , string.Format(@"{0}\{1}", m_oPath.ProcessPath, RLine[1]));

        //        ///[LOG]---------------------------------------------------------------------------------------
        //        Logging.WriteLog(string.Format("File Move[{0} to {1}]", RLine[1], string.Format(@"{0}\{1}", m_oPath.ProcessPath, RLine[1])), "PROC", 3);
        //        ///--------------------------------------------------------------------------------------------

        //        if (m_oKHeader.TiffFileName != null && m_oKHeader.TiffFileName.Length > 0)
        //        {
        //            if (m_oImgLoader != null)
        //            {
        //                m_oImgLoader.Dispose();
        //            }
        //            File.Delete(m_oKHeader.TiffFileName);
        //            //File.Move(m_oKHeader.TiffFileName,string.Format(@"{0}\{1}_{2}",m_oPath.ServicePath,RLine[1],m_oPath.TransTime));
        //        }

        //        m_oKHeader.TiffFileName = string.Format(@"{0}\{1}", m_oPath.ProcessPath, RLine[1]);
        //        m_oImgLoader = new ImageLoader(m_oKHeader.TiffFileName);

        //    }
        //    catch (Exception ex)
        //    {
        //        ///[LOG]---------------------------------------------------------------------------------------
        //        Logging.WriteLog(string.Format("Image Loading Error[{0}] [ERROR MSG][{1}]", RLine[1], ex.Message), "ERROR", 0);
        //        Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
        //        ///--------------------------------------------------------------------------------------------
        //        throw this.ProcessErr(ex);
        //    }
        //    finally
        //    {
        //    }
        //}
        //#endregion

        #region InspectionStationID "Accretech" "WW1400L" "PPW101";
        /// <summary>
        /// 장비 ID,Type,Maker 분류
        /// 각 FAB의 장비별로 하드코딩 허용할 수 있는 영역
        /// </summary>
        /// <param name="RLine">"Accretech" "WW1400L" "PPW101"</param>
        public void PrsINSPECTIONSTATIONID(string[] RLine)
        {
            TQD_UPDATESRV oUpdateSrv = null;
            try
            {
                oUpdateSrv = new TQD_UPDATESRV();

                m_oKHeader.InspectionStationID[0] = "WB3200";	//장비 Maker
                m_oKHeader.InspectionStationID[1] = "WB3200";	//장비 Model
                m_oKHeader.InspectionStationID[2] = RLine[1].Trim();	//장비 ID			

                DataTable oDt = oUpdateSrv.SelectEquipInfo(new string[1] { m_oKHeader.InspectionStationID[2] });

                if (oDt.Rows.Count == 0)
                {
                    ///--------------------------------------------------------------------------------------------

                    string strRepEquip_id = string.Empty;
                    strRepEquip_id = Path.GetFileName(Path.GetDirectoryName(m_oPath.ResultPath));
                    ///[LOG]---------------------------------------------------------------------------------------
                    Logging.WriteLog(string.Format("DB에 '{0}'가 등록되어있지 않습니다.'{1}'로 강제 변경하였습니다.", m_oKHeader.InspectionStationID[2], strRepEquip_id), "PROC", 0);
                    ///--------------------------------------------------------------------------------------------
                    m_oKHeader.InspectionStationID[2] = strRepEquip_id;
                    oDt = oUpdateSrv.SelectEquipInfo(new string[1] { m_oKHeader.InspectionStationID[2] });
                }

                if (oDt != null && oDt.Rows.Count > 0)
                {
                    if (oDt.Rows[0]["SVC_FLAG"].ToString() == "Y")
                    {
                        m_bServiceFlag = true;
                    }
                    else
                    {
                        m_bServiceFlag = false;
                    }

                    if (oDt.Rows[0]["SVC_FLAG2"].ToString() == "Y")
                    {
                        m_bCommonServiceFlag = true;
                    }
                    else
                    {
                        m_bCommonServiceFlag = false;
                    }

                    m_strFACILITY = oDt.Rows[0]["FACILITY"].ToString();
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("Equip ID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
                if (oUpdateSrv != null)
                    oUpdateSrv.Dispose();

            }
        }
        #endregion

        #region SampleType WAFER;
        public void PrsSAMPLETYPE(string[] RLine)
        {
            try
            {
                m_oKHeader.SampleType = RLine[1];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleType Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region ResultTimestamp 09-08-04 00:48:26;
        public void PrsRESULTTIMESTAMP(string[] RLine)
        {
            string strRLine = string.Empty;
            string strDate = "";
            string strTime = "";
            try
            {
                for (int i = 1; i < RLine.Length; i++)
                {
                    strRLine += RLine[i];
                }

                string i_DateStyle = DACrux.Base.Format.GetDateString(ref strDate, strRLine);
                string i_TimeStyle = DACrux.Base.Format.GetTimeString(ref strTime, strRLine);

                m_oKHeader.ResultTimestamp = string.Format("{0} {1}", strDate, strTime);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("ResultTimeStamp Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region LotID "TOB0001_02";
        public void PrsLOTID(string[] RLine)
        {
            try
            {
                m_oKHeader.LotID = RLine[1].Trim();
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("LotID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region SampleSize 1 300;
        public void PrsSAMPLESIZE(string[] RLine)
        {
            try
            {
                m_oKHeader.SampleSize = new int[1];
                m_oKHeader.SampleSize[0] = int.Parse(RLine[1]) * 25;
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleSize Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }
        #endregion

        #region DeviceID "HY어쩌구저쩌구";
        public void PrsDEVICEID(string[] RLine)
        {
            try
            {
                m_oKHeader.DeviceID = RLine[1].Replace("\"", "").ToUpper().Trim();
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DeviceID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }
        #endregion

        #region SetupID "GC_TDA_BLM_WTDA" 09-08-04 00:35:41;
        public void PrsSETUPID(string[] RLine)
        {
            try
            {
                m_oKHeader.SetupID = RLine[1].Replace("\"", "");
                string strRLine = string.Join(" ", RLine, 0, RLine.Length).Trim();
                m_oKHeader.SetupTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SetupID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region StepID "BLMK";
        public void PrsSTEPID(string[] RLine)
        {
            string[] sArray;

            try
            {
                m_oKHeader.StepID = RLine[1].Trim();
                sArray = m_oKHeader.StepID.Split('_');
                m_oKHeader.DeviceID = sArray[0].ToString();
                m_oKHeader.StepID = sArray[1].ToString();
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("StepID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region ResultID "BLMK";
        public void PrsRESULTID(string[] RLine)
        {
            try
            {
                m_oKHeader.ResultID = RLine[1].Replace("\"", "");
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("ResultID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region SampleCenterLocation 9344.529000 260.100000;

        public void PrsSAMPLEORIENTATIONMARKTYPE(string[] RLine)
        {
            try
            {
                m_oKHeader.SampleOrientationMarkType = RLine[1];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleOrientationMakType Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region OrientationMarkLocation DOWN;

        public void PrsORIENTATIONMARKLOCATION(string[] RLine)
        {
            try
            {
                string strAngle = RLine[1].Trim();
                int iAngle = 0;

                if (!int.TryParse(strAngle, out iAngle))
                {
                    m_oKHeader.OrientationMarkLocation = RLine[1].Trim();
                }
                else
                {
                    iAngle = Int32.Parse(strAngle);

                    // Offset : 180
                    if (OffSet != 0)
                    {
                        switch (iAngle)
                        {
                            case 0:
                                iAngle = 180;
                                break;
                            case 90:
                                iAngle = 270;
                                break;
                            case 180:
                                iAngle = 0;
                                break;
                            case 270:
                                iAngle = 90;
                                break;
                        }
                    }

                    m_oKHeader.OrientationMarkLocation = iAngle.ToString();
                }

                //m_oKHeader.OrientationMarkLocation = RLine[1].Trim();
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("OrientationMarkLocation Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region DiePitch 9.3999290000e+003 5.8599620000e+003;
        public void PrsDIEPITCH(string[] RLine, bool IsX = true)
        {
            try
            {
                if (IsX)
                {
                    m_oKHeader.DiePitchX = double.Parse(RLine[1]);
                }
                else
                {
                    m_oKHeader.DiePitchY = double.Parse(RLine[1]);
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DiePitch Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region DieOrigin 0.000000 0.000000;
        public void PrsDIEORIGIN(string[] RLine)
        {
            try
            {
                m_oKHeader.DieOriginX = double.Parse(RLine[1]);
                m_oKHeader.DieOriginY = double.Parse(RLine[2]);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DieOrigin Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region WaferID "@02";
        public void PrsWAFERID(string[] RLine, bool isPreParsing)
        {
            string strWID;
            //string[] sArray;

            try
            {
                strWID = RLine[1].Trim();

                if (isPreParsing)
                    m_oKHeader.WaferID = strWID;
                else
                {
                    m_oKHeader.WaferID = strWID.Substring(strWID.Length - 2, 2).PadLeft(2, '0');
                    m_oKHeader.LotID = strWID.Substring(0, strWID.Length - 3);
                    //m_oKHeader.DeviceID = m_oKHeader.StepID.Substring(0, m_oKHeader.StepID.IndexOf('_'));
                    //m_oKHeader.StepID = m_oKHeader.StepID.Substring(m_oKHeader.StepID.StartsWith('_').ToString(), m_oKHeader.StepID.EndsWith('_').ToString());

                    //sArray = m_oKHeader.StepID.Split('_');
                    //m_oKHeader.DeviceID = sArray[0].ToString();
                    //m_oKHeader.StepID = sArray[1].ToString();
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("WaferID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region Slot 2;
        public void PrsSLOT(string[] RLine)
        {
            try
            {
                m_oKHeader.Slot = int.Parse(RLine[1]);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SlotID Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region SampleCenterLocation 9344.529000 260.100000;

        public void PrsSAMPLECENTERLOCATION(string[] RLine)
        {
            try
            {
                m_oKHeader.SampleCenterLocationX = double.Parse(RLine[1]);
                m_oKHeader.SampleCenterLocationY = double.Parse(RLine[2]);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleCenterLocation Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region ClassLookup 256

        public void PrsCLASSLOOKUP(string[] RLine)
        {
            try
            {
                m_oKHeader.ClassLookup = int.Parse(RLine[1]);
                m_oKHeader.DefectClass = new string[m_oKHeader.ClassLookup];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("ClassLookUp Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region 0 "0" ☞
        public void SubPrsCLASSLOOKUP(string[] RLine)
        {
            int iIdx = -1;
            string strClassName = string.Empty;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        if (iIdx == -1)
                        {
                            iIdx = int.Parse(RLine[i]);
                        }
                        else
                        {
                            strClassName += RLine[i];
                        }
                    }
                }

                if (iIdx > -1)
                {
                    m_oKHeader.DefectClass[iIdx] = strClassName;
                }
            }
            catch (Exception ex)
            {
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-02-18
                ///	- 변경로그 : ClassLookup의 갯수가 모자라는 경우, 넘치는 경우, 중간에 빠지는 경우, 순서가 바뀌는 경우등
                ///	             에러를 무시한다.

                //				///[LOG]---------------------------------------------------------------------------------------
                //				Logging.WriteLog(string.Format("ClassLookUp Parsing Error [ERROR MSG : {0}]",ex.Message ),"ERROR",0);
                //				Logging.WriteLog(string.Format("Error Message [{0}]",ex.Message ),"ERROR",1);
                //				///--------------------------------------------------------------------------------------------			
                //				throw this.ProcessErr(ex);

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("ClassLookUp Parsing Error [ERROR MSG : {0}]", ex.Message +
                                                "ClassLookup의 정보가 올바르지 않습니다."), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message +
                                                "ClassLookup의 정보가 올바르지 않습니다."), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------			
                /// ====================================================================
            }

            finally
            {
            }
        }
        #endregion
        #region 1  "1"
        #endregion
        #region :
        #endregion
        #region :
        #endregion
        #region  255 "";
        #endregion

        #region InspectionTest 1;
        public void PrsINSPECTIONTEST(string[] RLine)
        {
            try
            {
                m_oKHeader.InspectionTest = int.Parse(RLine[1]);
                // 멀티 SampleTestPlan 인써트를 위해 추가 2005.05.27. by 강혜경
                m_InspectionTest.Add(m_oKHeader.InspectionTest);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("InspectionTest Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region Inspection dies,72
        public void PrsSAMPLETESTPLAN(string[] RLine)
        {
            try
            {
                m_oKHeader.SampleTestPlan = int.Parse(RLine[1]);
                m_oKHeader.TestDieInfo = new DIEINFO_TAG[m_oKHeader.SampleTestPlan];
                m_iDieCount = 0;
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleTestPlan Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion
        #region   -14    -6 ☞
        public void SubPrsSAMPLETESTPLAN(string[] RLine)
        {
            try
            {
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-02-18
                ///	- 변경로그 : M6 Klarf file 에 줄바꿈이 있는 경우에 대비하여 추가함
                if (RLine.Length == 1)
                    return;
                /// ====================================================================
                m_oKHeader.TestDieInfo[m_iDieCount].DX = int.Parse(RLine[1]);
                m_oKHeader.TestDieInfo[m_iDieCount].DY = int.Parse(RLine[2]);
                m_oKHeader.TestDieInfo[m_iDieCount].TEST = m_oKHeader.InspectionTest;
                m_iDieCount++;
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SampleTestPlan Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion
        #region		-13    -6
        #endregion
        #region		-12    -6
        #endregion
        #region		-11    -6
        #endregion
        #region		-10    -6
        #endregion
        #region :
        #endregion
        #region :
        #endregion

        #region AreaPerTest 1.0050160835e+010;

        public void PrsAREAPERTEST(string[] RLine)
        {
            try
            {
                m_oKHeader.AreaPerTest = double.Parse(RLine[1]);
                // 멀티 SampleTestPlan 인써트를 위해 추가 2005.05.27. by 강혜경
                m_AreaPerTest.Add(m_oKHeader.AreaPerTest);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("AreaPerTest Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion

        #region DefectRecordSpec 17 DEFECTID XREL YREL XINDEX YINDEX XSIZE YSIZE DEFECTAREA DSIZE CLASSNUMBER TEST CLUSTERNUMBER ROUGHBINNUMBER FINEBINNUMBER REVIEWSAMPLE IMAGECOUNT IMAGELIST;

        public void PrsDEFECTRECORDSPEC(string[] RLine)
        {
            int iCount = 0;
            int iIdx = 0;
            string strMerge = string.Empty;
            try
            {
                iCount = 27;

                m_oKHeader.DefectRecordSpec = new string[iCount];
                m_oKHeader.Defects = new DEFECT_TAG[m_iMaxDefects];

                for (int i = 0; i < iCount; i++)
                {
                    switch (RLine[i].Trim())
                    {
                        case "No.": m_iParmSpecIndex[iIdx] = 1; break;             //DEFECTID
                        case "Chip X Position[mm]": m_iParmSpecIndex[iIdx] = 5; break;                 //XREL
                        case "Chip Y Position[mm]": m_iParmSpecIndex[iIdx] = 6; break;                 //YREL
                        case "Chip X": m_iParmSpecIndex[iIdx] = 7; break;               //XINDEX
                        case "Chip Y": m_iParmSpecIndex[iIdx] = 8; break;               //YINDEX
                        case "Width[um]": m_iParmSpecIndex[iIdx] = 9; break;                //XSIZE
                        case "Length[um]": m_iParmSpecIndex[iIdx] = 10; break;               //YSIZE
                        case "Area(Particle)[um2]": m_iParmSpecIndex[iIdx] = 11; break;          //DEFECTAREA
                        case "Height[um]": m_iParmSpecIndex[iIdx] = 12; break;               //DSIZE
                        case "Defect Kind": m_iParmSpecIndex[iIdx] = 13; break;         //CLASSNUMBER
                        //case "TEST": m_iParmSpecIndex[iIdx] = 14; break;                //TEST
                        //case "CLUSTERNUMBER": m_iParmSpecIndex[iIdx] = 15; break;       //CLUSTERNUMBER
                        //case "ROUGHBINNUMBER": m_iParmSpecIndex[iIdx] = 16; break;      //ROUGHBINNUMBER
                        //case "FINEBINNUMBER": m_iParmSpecIndex[iIdx] = 17; break;       //FINEBINNUMBER
                        //case "REVIEWSAMPLE": m_iParmSpecIndex[iIdx] = 18; break;        //REVIEWSAMPLE
                        //case "IMAGECOUNT": m_iParmSpecIndex[iIdx] = 19; break;          //IMAGECOUNT
                        //case "IMAGELIST": m_iParmSpecIndex[iIdx] = 20; break;           //IMAGELIST
                        default: m_iParmSpecIndex[iIdx] = -1; break;
                    }
                    m_oKHeader.DefectRecordSpec[iIdx++] = RLine[i].Trim();
                    strMerge = strMerge + ";" + RLine[i];
                }

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectRecordSpec [Record Count : {0}][Record List : {1}]", strMerge, strMerge), "PROC", 1);
                ///--------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectRecordSpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }
        #endregion
        #region DefectList
        public void SubPrsDEFECTLIST()
        {
            //할일없음
        }

        #endregion

        #region 1 7.4686784000e+003 4.3462184000e+003 11 5 4.396000 3.516800 15.459853 4.3960000000e+000 0 1 0 22 0 0 1 1

        public void AddDefect(string[] RLine)
        {
            if (m_iDefectCount > m_iMaxDefects) return;						//m_iMaxDefects 이상은 허용하지 않는다.
            //int iImageList = 0;
            int iIdx = 0;
            //int iAddImages = 0;
            int i = 0;
            //string[] strTmpImagePath = null;

            //int iKey = 0;

            try
            {
                if (RLine.Length >= m_oKHeader.DefectRecordSpec.Length)		//DefectRecordSpec의 수보다 Defect정보가 많은지 확인한다.
                {
                    m_oKHeader.Defects[m_iDefectCount].DValue = new int[m_oKHeader.DefectRecordSpec.Length];
                    for (i = 0; i < m_oKHeader.DefectRecordSpec.Length; i++)
                    {
                        switch (m_oKHeader.DefectRecordSpec[i].Trim())
                        {
                            case "No.":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i]));
                                break;
                            case "Chip X Position[mm]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK);
                                break;
                            case "Chip Y Position[mm]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK);
                                break;
                            case "Chip X":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()));
                                break;
                            case "Chip Y":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()));
                                break;
                            case "Width[um]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK / 1000);
                                break;
                            case "Length[um]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK / 1000);
                                break;
                            case "Area(Particle)[um2]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK / 1000);
                                break;
                            case "Height[um]":
                                m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = (int)(double.Parse(RLine[i].Trim()) * RK / 1000);
                                break;
                            case "Defect Kind":
                                if (RLine[i].Trim().ToUpper().StartsWith("OTHER"))
                                {
                                    m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = 0;
                                }
                                else
                                {
                                    m_oKHeader.Defects[m_iDefectCount].DValue[iIdx] = 1;
                                }
                                break;
                            default: break;
                        }
                        iIdx++;
                    }
                    m_iDefectCount++;
                }
            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectLine Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }

        #region ■ BEFORE : Image 이름과 Directory 관리방안 변경으로 함수 변경 2005-01-07일
        /*
		public void AddIMAGE(string[] RLine)
		{
			int iImgDefectID = -1;
			string strImagePath = string.Empty;
			string strLotPath	= string.Empty;
			string strWaferPath = string.Empty;
			string strStepPath	= string.Empty;

			try
			{
				if( int.Parse(RLine[0]) > 0 ) 
				{
					iImgDefectID	= m_iDefectCount-1;		
					strLotPath		= string.Format(@"{0}\{1}",m_strLinkedPath,m_oKHeader.LotID);
					strWaferPath	= string.Format(@"{0}\{1}",strLotPath,m_oKHeader.WaferID);
					strStepPath		= string.Format(@"{0}\{1}",strWaferPath,m_oKHeader.StepID);

					/// +
					/// +
					/// LOT / WAFER_ID / STEP 형태의 Directory를 있는지 확인하고 없으면 순차적으로 Create한다.
					/// ====================================================================================================================================
					if(!Directory.Exists(strLotPath))	Directory.CreateDirectory(strLotPath);
					if(!Directory.Exists(strWaferPath))	Directory.CreateDirectory(strWaferPath);
					if(!Directory.Exists(strStepPath))	Directory.CreateDirectory(strStepPath);
					/// ====================================================================================================================================
					/// +
					/// +

					for(int imgIndex = 0;imgIndex < m_oKHeader.Defects[iImgDefectID].IMAGENAME.Length; imgIndex++)
					{
						m_iTotalImageCount++;																//Image 개수를 Count한다.
						if(m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex] != null) continue;			//이미 Image가 있으면 다음 Array로 넘어간다.
						
						/// +
						/// +
						/// 각 DEFECTID에 해당하는 IMAGENAME을 LOT / WAFER_ID / STEP / LOTID_WAFERID_STEPID_DEFECTID_IMAGEID_TIME.jpg 로 결정한다.
						/// ====================================================================================================================================
						m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex]
							= string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}_{6}.jpg"
							,strStepPath
							,m_oKHeader.LotID
							,m_oKHeader.WaferID
							,m_oKHeader.StepID
							,m_oKHeader.Defects[iImgDefectID].DValue[0].ToString().PadLeft(6,'0')
							,imgIndex.ToString().PadLeft(2,'0')
							,m_oPath.TransTime);
						/// ====================================================================================================================================
						/// +
						/// +
						
						m_oImgLoader.SaveImage(m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex],int.Parse(RLine[1]),IMAGEFORMAT.JPG);
						
						/// +
						/// +
						/// Image File이 정상으로 Save가 되었는지 확인하고 Logging한다.
						/// ====================================================================================================================================

						if(File.Exists(m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex]))
						{
							Logging.WriteLog(string.Format("Image File Save [{0}]",m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex]),"PROC",0);
						}
						else
						{
							Logging.WriteLog(string.Format("Image File Save Error [{0}]",m_oKHeader.Defects[iImgDefectID].IMAGENAME[imgIndex]),"ERROR",0);
						}
						/// ====================================================================================================================================
						/// +
						/// +
						break;
					}
				}
			}
			catch(Exception ex)
			{
				/// +
				/// +
				/// Exception 이 발생하여 Logging을 한다
				/// ====================================================================================================================================
				Logging.WriteLog(string.Format("Image Parsing Error [ERROR MSG : {0}]",ex.Message ),"ERROR",0);
				Logging.WriteLog(string.Format("Error Message [{0}]",ex.Message ),"ERROR",1);
				/// ====================================================================================================================================
				/// +
				/// +
				throw this.ProcessErr(ex);
			}
			finally
			{
			}
		}
*/
        #endregion

        #region ■ AFTER : Image 이름과 Directory 관리방안 변경으로 함수 변경 2005-01-07일
        /// ====================================================================
        ///	- 수정자 : 미라콤 임영신
        ///	- 수정일 : 2005-02-28
        ///	- 변경로그 : Multi Tiff 유무 파라메터를 추가하여 변경함,AddDefect에서 이 메서드를 참조함
        //public void AddIMAGE(string[] RLine)		
        public void AddIMAGE(string ImgFileName, int DefectIdx)
        /// ====================================================================
        {
            string strEquipPath = string.Empty;
            string strDatePath = string.Empty;
            string strLotPath = string.Empty;

            try
            {
                if (ImgFileName.Length == 0)
                    return;

                strEquipPath = string.Format(@"{0}\{1}", m_strLinkedPath, m_oKHeader.InspectionStationID[2]);
                strDatePath = string.Format(@"{0}\{1}", strEquipPath, DateTime.Now.ToString("yyyyMMdd"));
                strLotPath = string.Format(@"{0}\{1}", strDatePath, m_oKHeader.LotID);

                /// +
                /// +
                /// m_strLinkedPath/EquipID/yyymmdd/LOT 형태의 Directory가 있는지 확인하고 없으면 순차적으로 Create한다.
                /// ====================================================================================================
                if (!Directory.Exists(strEquipPath)) Directory.CreateDirectory(strEquipPath);
                if (!Directory.Exists(strDatePath)) Directory.CreateDirectory(strDatePath);
                if (!Directory.Exists(strLotPath)) Directory.CreateDirectory(strLotPath);
                /// ====================================================================================================
                /// +
                /// +
                m_oKHeader.Defects[DefectIdx].DValue[19] = 1;
                m_oKHeader.Defects[DefectIdx].IMAGENAME = new string[2];
                m_oKHeader.Defects[DefectIdx].IMAGENAME[0] = ImgFileName;
                m_oKHeader.Defects[DefectIdx].IMAGENAME[1] = string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}_{6}.jpg"
                                                        , strLotPath
                                                        , m_oKHeader.LotID
                                                        , m_oKHeader.WaferID
                                                        , m_oKHeader.StepID
                                                        , m_oKHeader.Defects[DefectIdx].DValue[0].ToString().PadLeft(6, '0')
                                                        , DefectIdx.ToString().PadLeft(2, '0')
                                                        , m_oPath.TransTime);
                m_ImageCount++;
            }
            catch (Exception ex)
            {
                /// Exception 이 발생하여 Logging을 한다
                /// ====================================================================================================================================
                Logging.WriteLog(string.Format("Image Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                /// ====================================================================================================================================
            }
        }

        #endregion

        #endregion

        #region  2 4.3146112000e+003 4.1110939000e+003 -8 -6 0.376800 0.376800 0.141978 3.7700000000e-001 0 1 0 12 0 0 1 1
        #endregion

        #region  3 5.0228696000e+003 4.6956363000e+003 2 -6 0.251200 0.251200 0.063101 2.5100000000e-001 0 1 0 12 0 0 1 1
        #endregion

        #region  4 2.7934696000e+003 2.9068424000e+003 9 -5 0.251200 0.376800 0.094652 3.7700000000e-001 0 1 0 12 0 0 1 1
        #endregion

        #region  5 8.9176000000e+003 4.3589040000e+003 -3 -5 0.125600 1.130400 0.141978 1.1300000000e+000 0 1 0 11 0 0 1 1
        #endregion

        #region  6 8.9211168000e+003 4.3585272000e+003 -3 -5 0.125600 0.125600 0.015775 1.2600000000e-001 0 1 0 12 0 0 0 0
        #endregion

        #region TestParametersSpec 3 PIXELSIZE INSPECTIONMODE SAMPLEPERCENTAGE;
        public void PrsTESTPARAMETERSSPEC(string[] RLine)
        {
            int iCount = 0;
            try
            {
                iCount = int.Parse(RLine[1]);

                m_oKHeader.TestParametersSpec = new string[iCount];
                m_oKHeader.TestParametersList = new double[iCount];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("TestParametersSpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }


        public void SubPrsTESTPARAMETERSSPEC(string[] RLine)
        {
            int iIdx = 0;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        m_oKHeader.TestParametersSpec[iIdx++] = RLine[i];
                    }
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("TestParametersSpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region TestParametersList 0.250000 1 1.000000;
        public void PrsTESTPARAMETERSLIST(string[] RLine)
        {
            //처리사항 없음
        }


        public void SubPrsTESTPARAMETERSLIST(string[] RLine)
        {
            int iIdx = 0;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        m_oKHeader.TestParametersList[iIdx++] = double.Parse(RLine[i]);
                    }
                }
                // 멀티 SampleTestPlan 인써트를 위해 추가 2005.05.27. by 강혜경
                double[] dbTestParametersList = (double[])m_oKHeader.TestParametersList.Clone();
                m_TestParametersList.Add(dbTestParametersList);
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("TestParametersList Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }

        #endregion

        #region DefectClusterSpec 3 THRESHOLD MINSIZE MERGETOL;
        public void PrsDEFECTCLUSTERSPEC(string[] RLine)
        {
            int iCount = 0;
            try
            {
                iCount = int.Parse(RLine[1]);

                m_oKHeader.DefectClusterSpec = new string[iCount];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectClusterSpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }


        public void SubPrsDEFECTCLUSTERSPEC(string[] RLine)
        {
            int iIdx = 0;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        m_oKHeader.DefectClusterSpec[iIdx++] = RLine[i];
                    }
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectClusterSpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }

        #endregion

        #region DefectClusterSetup 0.000000 0 0.000000;
        public void PrsDEFECTCLUSTERSETUP(string[] RLine)
        {
            int iCount = 0;
            try
            {
                iCount = int.Parse(RLine[1]);

                m_oKHeader.DefectClusterSetup = new int[iCount];
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectClusterSetup Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }


        public void SubPrsDEFECTCLUSTERSETUP(string[] RLine)
        {
            int iIdx = 0;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        m_oKHeader.DefectClusterSetup[iIdx++] = int.Parse(RLine[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectClusterSetup Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region SummarySpec 5 TESTNO NDEFECT DEFDENSITY NDIE NDEFDIE ;
        public void PrsSUMMARYSPEC(string[] RLine)
        {

            int iCount = -1;
            int iIdx = 0;
            try
            {
                for (int i = 0; RLine.Length > i; i++)
                {
                    if (RLine[i].Length == 0) continue;
                    if (RLine[i].ToUpper() == "SUMMARYSPEC")
                    {
                        iCount = 0;
                    }
                    else if (iCount == 0)
                    {
                        iCount = int.Parse(RLine[1]);
                        m_oKHeader.SummarySpec = new string[iCount];
                        m_oKHeader.SummaryList = new double[iCount];
                    }
                    else if (iCount > -1)
                    {
                        m_oKHeader.SummarySpec[iIdx++] = RLine[i];
                    }
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SummarySpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }


        public void SubPrsSUMMARYSPEC(string[] RLine)
        {
            int iIdx = 0;
            try
            {
                for (int i = 0; RLine.Length > i; i++)
                {
                    if (RLine[i].Length == 0) continue;
                    m_oKHeader.SummarySpec[iIdx++] = RLine[i];
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SummarySpec Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region SummaryList
        public void PrsSUMMARYLIST(string[] RLine)
        {
            //Dummy
        }

        #endregion

        #region  1 61 0.371268 880 59;
        public void SubPrsSUMMARYLIST(string[] RLine)
        {
            int iIdx = 0;
            string strMerge = string.Empty;
            try
            {
                for (int i = 0; i < RLine.Length; i++)
                {
                    if (RLine[i].Length > 0)
                    {
                        Logging.WriteLog(System.DateTime.Now.Millisecond.ToString(), "TEST", 0);
                        m_oKHeader.SummaryList[iIdx++] = double.Parse(RLine[i]);
                        strMerge = strMerge + " " + RLine[i];
                        Logging.WriteLog(System.DateTime.Now.Millisecond.ToString(), "TEST", 0);
                    }
                }
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-05-27
                ///	- 변경로그 : 주석처리, SummaryList의 멀티 라인을 아래 코드로 인해 읽지 못하기 때문
                //m_bMultiWaferCutLine = true;

                // 멀티 SampleTestPlan 인써트를 위해 추가 2005.05.27. by 강혜경
                double[] dbSummaryList = (double[])m_oKHeader.SummaryList.Clone();
                m_SummaryList.Add(dbSummaryList);
                /// ====================================================================
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SummaryList [{0}]", strMerge), "PROC", 2);
                ///--------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SummaryList Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region EndOfFile;
        public void PrsENDOFFILE()
        {
            try
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog("EndOfFile로 인해 File Parsing을 종료합니다.", "PROC", 2);
                ///--------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("EndOfFile Parsing Error [ERROR MSG : {0}]", ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region ■UPDB Product -> TQD_PRODUCT
        public int UploadDataProduct()
        {
            int istatus = 0;
            DACrux.SEMDMS.DSL.TQD_PRODUCT oYesProductTx = null;

            try
            {
                oYesProductTx = new DACrux.SEMDMS.DSL.TQD_PRODUCT();
                string[] strDBData = new string[14];

                int iAngle = 0;
                int iSampleSize = (int)(m_oKHeader.SampleSize[0] * RK);
                int iDiePitchX = (int)(m_oKHeader.DiePitchX * RK);
                int iDiePitchY = (int)(m_oKHeader.DiePitchY * RK);
                int iDieOriginX = (int)(m_oKHeader.DieOriginX * RK);
                int iDieOriginY = (int)(m_oKHeader.DieOriginY * RK);
                int iOriginX = (int)(m_oKHeader.SampleCenterLocationX * RK);
                int iOriginY = (int)(m_oKHeader.SampleCenterLocationY * RK);
                char cNotchType = 'N';

                if (int.TryParse(m_oKHeader.OrientationMarkLocation, out iAngle) == false) iAngle = 0;
                cNotchType = 'N';

                strDBData[0] = m_oKHeader.DeviceID;		///PRODUCT
                strDBData[1] = m_strFACILITY;				///FACILITY
                strDBData[2] = " ";						///TECHNOLOGY
                strDBData[3] = m_oKHeader.SampleTestPlan.ToString();	///NETDIE
                strDBData[4] = iSampleSize.ToString();	///WAFER_SIZE
                strDBData[5] = iAngle.ToString();	///ANGLE
                strDBData[6] = cNotchType.ToString();	///NOTCH_TYPE
                strDBData[7] = iDiePitchX.ToString();	///DIE_PITCH_X
                strDBData[8] = iDiePitchY.ToString();	///DIE_PITCH_Y
                strDBData[9] = iDieOriginX.ToString();	///DIE_ORIGIN_X
                strDBData[10] = iDieOriginY.ToString();	///DIE_ORIGIN_Y
                strDBData[11] = iOriginX.ToString();	///ORIGIN_X
                strDBData[12] = iOriginY.ToString();	///ORIGIN_Y
                strDBData[13] = " ";					///COMMENT

                DataTable dt = oYesProductTx.GetProduct(strDBData);
                if (dt.Rows.Count == 0)
                {
                    oYesProductTx.Create(strDBData);
                    ///[LOG]---------------------------------------------------------------------------------------
                    Logging.WriteLog(string.Format("[PRODUCT       : {0}]", strDBData[0]), "PROC", 2);
                    Logging.WriteLog(string.Format("[FACILITY      : {0}]", strDBData[1]), "PROC", 2);
                    Logging.WriteLog(string.Format("[TECHNOLOGY    : {0}]", strDBData[2]), "PROC", 2);
                    Logging.WriteLog(string.Format("[NETDIE        : {0}]", strDBData[3]), "PROC", 2);
                    Logging.WriteLog(string.Format("[WAFER_SIZE    : {0}]", strDBData[4]), "PROC", 2);
                    Logging.WriteLog(string.Format("[ANGLE         : {0}]", strDBData[5]), "PROC", 2);
                    Logging.WriteLog(string.Format("[NOTCH_TYPE    : {0}]", strDBData[6]), "PROC", 2);
                    Logging.WriteLog(string.Format("[DIE_PITCH_X   : {0}]", strDBData[7]), "PROC", 2);
                    Logging.WriteLog(string.Format("[DIE_PITCH_Y   : {0}]", strDBData[8]), "PROC", 2);
                    Logging.WriteLog(string.Format("[DIE_ORIGIN_X  : {0}]", strDBData[9]), "PROC", 2);
                    Logging.WriteLog(string.Format("[DIE_ORIGIN_Y  : {0}]", strDBData[10]), "PROC", 2);
                    Logging.WriteLog(string.Format("[ORIGIN_X      : {0}]", strDBData[11]), "PROC", 2);
                    Logging.WriteLog(string.Format("[ORIGIN_Y      : {0}]", strDBData[12]), "PROC", 2);
                    ///--------------------------------------------------------------------------------------------
                }
                else
                {
                    istatus = 1;
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("Product Upload Error [PRODUCT : {0}] [ERROR MSG : {1}]", m_oKHeader.DeviceID, ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }
            finally
            {
                //ContextUtil.SetComplete();
                oYesProductTx.Dispose();
            }

            return istatus;
        }

        #endregion

        #region ■UPDB LOT  -> TQD_LOT
        public int UploadDataLot(ref long LotSeq)
        {
            DACrux.SEMDMS.DSL.TQD_LOT oYesLotTx = null;
            int istatus = 0;
            try
            {
                oYesLotTx = new DACrux.SEMDMS.DSL.TQD_LOT();
                string[] strDBData = new string[3];

                strDBData[0] = m_oKHeader.LotID;		///FACILITY			
                strDBData[1] = m_oKHeader.DeviceID;		///PRODUCT
                strDBData[2] = " ";						///COMMENT

                LotSeq = oYesLotTx.GetLotSeq(strDBData);

                if (LotSeq == -1)
                {
                    oYesLotTx.Create(strDBData);
                    LotSeq = oYesLotTx.GetLotSeq(strDBData);
                }
                else
                {
                    istatus = 1;
                }
            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("LotID Upload Error [LOTID : {0}] [ERROR MSG : {0}]", m_oKHeader.LotID, ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }

            finally
            {
                //ContextUtil.SetComplete();
                oYesLotTx.Dispose();
            }
            return istatus;
        }

        #endregion

        #region ■UPDB Wafer  -> TQD_WAFER
        public int UploadDataWafer(long LotSeq, ref long WaferSeq)
        {
            DACrux.SEMDMS.DSL.TQD_WAFER oYesWaferTx = null;
            int istatus = 0;

            try
            {
                oYesWaferTx = new DACrux.SEMDMS.DSL.TQD_WAFER();
                string[] strDBData = new string[4];

                strDBData[0] = LotSeq.ToString();		///	LOT_SEQ              BIGINT NOT NULL,
                strDBData[1] = m_oKHeader.WaferID;		///	WAFER_ID             VARCHAR(40) NOT NULL,
                strDBData[2] = m_oKHeader.Slot.ToString();///	SLOT_ID              INTEGER NOT NULL,
                strDBData[3] = " ";						///	COMMENT              VARCHAR(128)

                WaferSeq = oYesWaferTx.GetWaferSeq(strDBData);
                if (WaferSeq == -1)
                {
                    oYesWaferTx.Create(strDBData);
                    WaferSeq = oYesWaferTx.GetWaferSeq(strDBData);
                }
                else
                {
                    istatus = 1;
                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("Wafer Upload Error [LOT_SEQ : {0}] [WAFERID : {0}] [ERROR MSG : {0}]", LotSeq, m_oKHeader.WaferID, ex.Message), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }

            finally
            {
                //ContextUtil.SetComplete();
                oYesWaferTx.Dispose();
            }

            return istatus;

        }

        #endregion

        #region ■UPDB Setup  -> TQD_SETUP

        private void OriginOffset()
        {
            int[] XAxis = new int[m_iMaxX];
            int[] YAxis = new int[m_iMaxY];
            try
            {
                for (int i = 0; i < m_oKHeader.TestDieInfo.Length; i++)
                {
                    XAxis[m_oKHeader.TestDieInfo[i].DX]++;
                    YAxis[m_oKHeader.TestDieInfo[i].DY]++;
                }

                int x_left = XAxis[0] / 2;
                int x_right = XAxis[m_iMaxX - 1] / 2;

                int y_bottom = YAxis[0] / 2;
                int y_top = YAxis[m_iMaxY - 1] / 2;

                double xOffset = ((double)(m_oKHeader.SampleSize[0]) - (m_oKHeader.DiePitchX * m_iMaxX)) * ((double)x_left / (double)(x_left + x_right));
                double yOffset = ((double)(m_oKHeader.SampleSize[0]) - (m_oKHeader.DiePitchY * m_iMaxY)) * ((double)y_bottom / (double)(y_bottom + y_top));

                m_oKHeader.SampleCenterLocationX = (double)(m_oKHeader.SampleSize[0]) / 2 - xOffset;
                m_oKHeader.SampleCenterLocationY = (double)(m_oKHeader.SampleSize[0]) / 2 - yOffset;

            }
            catch
            {
                m_oKHeader.SampleCenterLocationX = 0d;
                m_oKHeader.SampleCenterLocationY = 0d;
            }
        }

        public int UploadDataSetup(ref long SetupSeq)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oYesSetupTx = null;
            int istatus = 0;

            try
            {
                OriginOffset();
                oYesSetupTx = new DACrux.SEMDMS.DSL.TQD_SETUP();
                string[] strDBData = new string[12];

                int iAngle = 0;
                int iSampleSize = (int)(m_oKHeader.SampleSize[0] * RK);
                int iDiePitchX = (int)(m_oKHeader.DiePitchX * RK);
                int iDiePitchY = (int)(m_oKHeader.DiePitchY * RK);
                int iDieOriginX = (int)(m_oKHeader.DieOriginX * RK);
                int iDieOriginY = (int)(m_oKHeader.DieOriginY * RK);
                int iOriginX = (int)(m_oKHeader.SampleCenterLocationX * RK);
                int iOriginY = (int)(m_oKHeader.SampleCenterLocationY * RK);
                char cNotchType = 'N';

                if (int.TryParse(m_oKHeader.OrientationMarkLocation, out iAngle) == false) iAngle = 0;

                strDBData[0] = m_oKHeader.SetupID;				//SETUP_ID             VARCHAR(40) NOT NULL,
                strDBData[1] = m_oKHeader.StepID;				//STEP_ID              VARCHAR(40) NOT NULL,
                strDBData[2] = m_oKHeader.SetupTime;			//SETUP_TIME           TIMESTAMP NOT NULL,
                strDBData[3] = iAngle.ToString();				//ANGLE                INTEGER NOT NULL DEFAULT 0,
                strDBData[4] = iSampleSize.ToString();			//WAFER_SIZE           INTEGER NOT NULL DEFAULT 300000000,
                strDBData[5] = cNotchType.ToString();			//NOTCH_TYPE           CHAR(1) NOT NULL DEFAULT 'F',
                strDBData[6] = iDiePitchX.ToString();			//DIE_PITCH_X          INTEGER NOT NULL DEFAULT 0,
                strDBData[7] = iDiePitchY.ToString();			//DIE_PITCH_Y          INTEGER NOT NULL DEFAULT 0,
                strDBData[8] = iDieOriginX.ToString();			//DIE_ORIGIN_X         INTEGER NOT NULL DEFAULT 0,
                strDBData[9] = iDieOriginY.ToString();			//DIE_ORIGIN_Y         INTEGER NOT NULL DEFAULT 0,
                strDBData[10] = iOriginX.ToString();			//ORIGIN_X             INTEGER NOT NULL DEFAULT 0,
                strDBData[11] = iOriginY.ToString();			//ORIGIN_Y             INTEGER NOT NULL DEFAULT 0

                /// =========================================================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-02-18
                ///	- 변경로그 : M6, Klarf file Spec에 SetupID의 SetupTime이 없는 경우 기존 M10 소스 막고 조건문 추가
                ///	             Setup_ID를 조회하여 
                ///						존재하지 않으면 현재 시간으로 대치하여 인써트하고
                ///						존재하면 인써트 하지 않는다.
                ///				: PrsSETUPID에 위 로직을 추가함, 기존 코드로 복원
                ///				: SetupID를 매번 인써트함, 업데이트 로직 제거 , 권택수 과장님 요청
                ///		
                oYesSetupTx.Create(strDBData);
                SetupSeq = oYesSetupTx.GetSetupSeq(strDBData);

                //				SetupSeq = oYesSetupTx.GetSetupSeq(strDBData);				
                //				if(SetupSeq == -1)	// -1 : 같은 Setup 정보가 없다 
                //				{
                //					oYesSetupTx.Create(strDBData);
                //					SetupSeq = oYesSetupTx.GetSetupSeq(strDBData);
                //				}
                //				else
                //				{
                //					istatus = 1;
                //				}

                //				SetupSeq = oYesSetupTx.GetSetupSeq(m_oKHeader.SetupID);	
                //				if(SetupSeq == -1)	// -1 : 같은 Setup 정보가 없다 
                //				{
                //					strDBData[2] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); 
                //					oYesSetupTx.Create(strDBData);
                //					SetupSeq = oYesSetupTx.GetSetupSeq(strDBData);
                //				}
                //				else
                //				{
                //					istatus = 1;
                //				}
                /// =========================================================================================================
            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("SETUPID Upload Error [SETUPID : {0}] ", m_oKHeader.SetupID), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }

            finally
            {
                //ContextUtil.SetComplete();
                oYesSetupTx.Dispose();
            }

            return istatus;
        }

        #endregion

        #region ■UPDB Step  -> TQD_STEP, TQD_INSP_INFO
        public int UploadDataStep(long wafer_seq, long setup_seq, ref long StepSeq)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oYesStepTx = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oYesStepInfo = null;

            /// Y2R에서 Data를 가져가기 위한 기준시간(즉,마지막으로 Data가 바뀐시간)을 넣어주기 위한 변수
            /// ==========================================================================================
            string strStatus = string.Empty;
            DateTime oCurrDt = DateTime.Now;
            /// ==========================================================================================

            int istatus = 0;
            try
            {
                oYesStepTx = new DACrux.SEMDMS.DSL.TQD_STEP();
                oYesStepInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-02-17
                ///	- 변경로그 : M10, EDB --> M6, M6HIS 로 변경 , GetMESInfo() 로 대치
                //				oEdbLotHistory = new EDBTB_LOTHISTORY();
                /// ====================================================================
                strStatus = oCurrDt.ToString("yyyyMMddHHmmss");


                string[] strDBData = new string[22];

                strDBData[14] = "0";
                strDBData[15] = "0";
                strDBData[16] = "0";

                //double[] summaryList = (double[])m_SummaryList[m_CurrentTestIndex];
                //double[] testParametersList = (double[])m_TestParametersList[m_CurrentTestIndex];
                double areaPerTest = 1;
                if (m_oKHeader.TestDieInfo != null)
                    areaPerTest = (double)(m_oKHeader.DiePitchX * m_oKHeader.DiePitchY * m_oKHeader.TestDieInfo.Length);


                //case "NDEFECTDIE":
                ArrayList tmpArr = new ArrayList();
                for (int i = 0; i < m_oKHeader.Defects.Length; i++)
                {
                    if (m_oKHeader.Defects[i].DValue == null) continue;
                    string xy = string.Format("X={0};Y={1}", m_oKHeader.Defects[i].DValue[1]
                                                           , m_oKHeader.Defects[i].DValue[2]);
                    if (tmpArr.IndexOf(xy) < 0)
                    {
                        tmpArr.Add(xy);
                    }
                }

                //case "NDEFECT":
                strDBData[15] = m_iDefectCount.ToString();
                //case "DEFDENSITY":
                strDBData[17] = ((double)m_iDefectCount / areaPerTest).ToString();


                strDBData[14] = tmpArr.Count.ToString();

                strDBData[0] = m_oKHeader.InspectionTest.ToString();
                //strDBData[0] = summaryList[0].ToString();        
                strDBData[1] = wafer_seq.ToString();
                strDBData[2] = m_oKHeader.StepID;
                strDBData[3] = m_oKHeader.Slot.ToString();
                strDBData[4] = m_oKHeader.ResultID;
                strDBData[5] = m_oKHeader.InspectionStationID[2];
                strDBData[6] = m_oKHeader.InspectionStationID[2];
                strDBData[7] = setup_seq.ToString();
                strDBData[8] = m_oKHeader.ResultTimestamp;
                strDBData[9] = m_oKHeader.FileTimestamp;
                strDBData[10] = m_MainEq;		//strMainEQ;	//Main Eq							             
                strDBData[11] = m_RTE;			//strRoute;		//Route					             
                strDBData[12] = m_Oper;			//Oper;						             
                //strDBData[13] = m_oKHeader.AreaPerTest.ToString();	
                strDBData[13] = areaPerTest.ToString();
                //strDBData[14] = 									             
                //strDBData[15] =									             
                //strDBData[16] = m_iTotalImageCount.ToString();
                strDBData[16] = m_ImageCount.ToString();
                //if(strDBData[17]==null) strDBData[17] = string.Format("{0:20,5}",(double.Parse(strDBData[15])/m_oKHeader.AreaPerTest));
                if (strDBData[17] == null) strDBData[17] = string.Format("{0:20,5}", (double.Parse(strDBData[15]) / areaPerTest));
                strDBData[18] = m_oPath.ResultFile;
                strDBData[19] = m_oPath.ServiceFile;
                strDBData[20] = m_ReticleID;	//strReticleID;	 
                strDBData[21] = m_StepperEq;

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("[---INSERT INTO TQD_STEP -----------------------------]", strDBData[0]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	TEST            	: {0} ]", strDBData[0]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	WAFER_SEQ       	: {0} ]", strDBData[1]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	STEP_ID         	: {0} ]", strDBData[2]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	SLOT_ID         	: {0} ]", strDBData[3]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	RESULT_ID       	: {0} ]", strDBData[4]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	INSPECTION_EQ   	: {0} ]", strDBData[5]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	REVIEW_EQ       	: {0} ]", strDBData[6]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	SETUP_SEQ       	: {0} ]", strDBData[7]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	RESULTTIMESTAMP 	: {0} ]", strDBData[8]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	FILETIMESTAMP   	: {0} ]", strDBData[9]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	MAIN_EQ         	: {0} ]", strDBData[10]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	ROUTE           	: {0} ]", strDBData[11]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	OPER            	: {0} ]", strDBData[12]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	SCAN_AREA       	: {0} ]", strDBData[13]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	DEFECTIVE_DIE   	: {0} ]", strDBData[14]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	DEFECTS         	: {0} ]", strDBData[15]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	IMAGES          	: {0} ]", strDBData[16]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	DEFECT_DD       	: {0} ]", strDBData[17]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	INSP_FILENAME   	: {0} ]", strDBData[18]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	SERVICE_FILENAME	: {0} ]", strDBData[19]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	RETICLE_ID         	: {0} ]", strDBData[20]), "PROC", 2);
                Logging.WriteLog(string.Format("[INSERT	STEPPER_EQ         	: {0} ]", strDBData[21]), "PROC", 2);
                ///--------------------------------------------------------------------------------------------				

                //StepSeq = oYesStepTx.GetStepSeq(new string[] {strDBData[1],strDBData[5],strDBData[2],strDBData[8]}); ///WAFER_SEQ,INSPECTION_EQ,STEP_ID,RESULTTIMESTAMP
                StepSeq = oYesStepTx.GetStepSeq(new string[] { strDBData[1], strDBData[5], strDBData[2], strDBData[8], strDBData[0] }); ///WAFER_SEQ,INSPECTION_EQ,STEP_ID,RESULTTIMESTAMP,TEST

                Logging.WriteLog(string.Format("[---INSERT Completed ------------------------------------]", strDBData[0]), "PROC", 2);


                if (StepSeq == -1)
                {
                    oYesStepTx.Create(strDBData);
                    //StepSeq = oYesStepTx.GetStepSeq(new string[] {strDBData[1],strDBData[5],strDBData[2],strDBData[8]});
                    StepSeq = oYesStepTx.GetStepSeq(new string[] { strDBData[1], strDBData[5], strDBData[2], strDBData[8], strDBData[0] });

                    StringBuilder InspectionParm = new StringBuilder();
                    for (int i = 0; i < m_oKHeader.DefectRecordSpec.Length; i++)
                    {
                        InspectionParm.Append(m_oKHeader.DefectRecordSpec[i]);
                        InspectionParm.Append(",");
                    }
                    InspectionParm.Remove(39, InspectionParm.Length - 39);

                    if (strDBData[20] == null || strDBData[20].Length == 0) strDBData[20] = "UNKNOWN";
                    oYesStepInfo.Create(new string[]{StepSeq.ToString()		//STEP_SEQ                  
																		,wafer_seq.ToString()					//WAFER_SEQ
																		,m_oKHeader.ResultTimestamp             //RESULTTIMESTAMP   
																		,m_oKHeader.FileTimestamp 				//FILETIMESTAMP
																		," "									//TECHNOLOGY
																		,m_oKHeader.DeviceID                    //PRODUCT     
																		,m_oKHeader.LotID						//LOT_ID
																		,m_oKHeader.WaferID						//WAFER_ID
																		,m_oKHeader.StepID						//STEP_ID
																		,m_oKHeader.InspectionTest.ToString()	//TEST 
																		,m_oKHeader.Slot.ToString()				//SLOT_ID
																		,setup_seq.ToString()					//SETUP_SEQ             
																		,strStatus								//STATUS
																		,m_MainEq	//strMainEQ								//MAIN_EQ
																		,m_RTE		//strRoute								//ROUTE
																		,m_Oper		//strOper								//OPER
																		,m_oKHeader.InspectionStationID[2]      //INSPECTION_EQ            
																		,InspectionParm.ToString()				//INSPECTION_PARAM
																		,strDBData[15]							//DEFECTS
																		,"0"									//CLASSIFIED_DEFECTS    
																		,"0"									//KILLER_DEFECTS        
																		,"0"									//RANDOM_DEFECTS
																		,"0"									//ADDER_KILLER_DEFECT
																		,"0"									//ADDER_RANDOM_DEFECT
																		,"0"									//CLUSTERS           
																		,"0"					   				//ADDER_CLUSTERS     
																		,"0"					   				//CLUSTER_AREA       
																		,strDBData[17]			   				//DEFECT_DD          
																		,"0"					   				//KILLER_DEFECT_DD   
																		,"0"					   				//RANDOM_DEFECT_DD   
																		,"0"					   				//INSPECTED_DIE      
																		,strDBData[14]			   				//DEFECTIVE_DIE      
																		,"0"					   				//ADDER_DEF_DIE      
																		,"0"					   				//KILL_DEF_DIE       
																		,"0"					   				//KILL_ADDER_DEF_DIE 
																		//,m_iTotalImageCount.ToString()		//IMAGES 
																	    ,strDBData[16]							//IMAGES
																		//,m_oKHeader.AreaPerTest.ToString()	//SCAN_AREA 
																		,areaPerTest.ToString()				//SCAN_AREA
																		,"0"					   				//ADDER_DEFECTS      
																		,"0"									//KILL_RND_DEFECTS 
																		,strDBData[20]						
																		,""
																		,strDBData[21]						//STEPPER_EQ
																	});
                    //ContextUtil.SetComplete();
                }
                else
                {
                    istatus = 1;
                }

            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("[---INSERT Error STEP_ID : {0} ----------------------------]", m_oKHeader.StepID), "PROC", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 0);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }
            finally
            {
                oYesStepTx.Dispose();
            }

            return istatus;
        }

        #endregion

        #region ■UpdateDB Step  -> TQD_STEP
        public int UpdateDataStep(long StepSeq)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oYesStepTx = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oYesStepInfo = null;
            /// Y2R에서 Data를 가져가기 위한 기준시간(즉,마지막으로 Data가 바뀐시간)을 넣어주기 위한 변수
            /// ==========================================================================================
            string strStatus = string.Empty;
            DateTime oCurrDt = DateTime.Now;
            /// ==========================================================================================

            int istatus = 0;
            try
            {
                oYesStepTx = new DACrux.SEMDMS.DSL.TQD_STEP();
                oYesStepInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                strStatus = oCurrDt.ToString("yyyyMMddHHmmss");

                string[] strDBData = new string[4];
                string[] strIFData = new string[11];

                strDBData[0] = m_oKHeader.InspectionStationID[2];	//REVIEW_EQ
                strDBData[1] = m_oKHeader.FileTimestamp;			//FILETIMESTAMP                      
                //strDBData[2] = m_iTotalImageCount.ToString();		//IMAGES   
                strDBData[2] = m_ImageCount.ToString();     //IMAGES  
                strDBData[3] = StepSeq.ToString();					//SLOT_ID    

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("[UPDATE REVIEW_EQ : {0}] ", strDBData[0]), "PROC", 0);
                Logging.WriteLog(string.Format("[UPDATE FILETIMESTAMP : {0}] ", strDBData[1]), "PROC", 0);
                Logging.WriteLog(string.Format("[UPDATE IMAGES : {0}] ", strDBData[2]), "PROC", 0);
                ///--------------------------------------------------------------------------------------------

                oYesStepTx.Update(strDBData);
                Logging.WriteLog(string.Format("[UPDATE Completed : STEP_SEQ = {0}] ", strDBData[3]), "PROC", 0);

                strIFData[0] = m_oKHeader.FileTimestamp;
                strIFData[1] = strStatus;
                strIFData[2] = "0";
                strIFData[3] = "0";
                strIFData[4] = "0";
                strIFData[5] = "0";
                strIFData[6] = "0";
                strIFData[7] = "0";
                //strIFData[8] = m_iTotalImageCount.ToString();
                strIFData[8] = m_ImageCount.ToString();
                strIFData[9] = "0";
                strIFData[10] = StepSeq.ToString();

                oYesStepInfo.Update(strIFData);

            }

            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("STEPID Upload Error [STEPID : {0}] ", m_oKHeader.StepID), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }

            finally
            {
                //ContextUtil.SetComplete();
                oYesStepTx.Dispose();
            }

            return istatus;
        }

        #endregion

        #region ■UPDB ScanSample -> TQD_SCAN_SAMPLE
        public void UploadDataScanSample(long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_SCAN_SAMPLE oYesScanSampleTx = null;
            string[] strQuery = null;
            int iBatchCnt = 0;

            try
            {
                //INSERT INTO TQD_SCAN_SAMPLE(STEP_SEQ,TEST,INDEX_X,INDEX_Y) VALUES(?,?,?,?);
                strQuery = new string[1];
                oYesScanSampleTx = new DACrux.SEMDMS.DSL.TQD_SCAN_SAMPLE();
                string[] strDBData = new string[4];
                strDBData[0] = step_seq.ToString();			//SETUP_ID             VARCHAR(40) NOT NULL,
                string sQueryPrefix = string.Empty;
                string sQueryValues = string.Empty;

                strQuery[0] = "";

                sQueryPrefix = "INSERT INTO TQD_SCAN_SAMPLE(STEP_SEQ,TEST,INDEX_X,INDEX_Y) VALUES";
                if (oYesScanSampleTx.GetScanSampleCount(step_seq) == 0)
                {
                    DIEINFO_TAG[] testDieInfo;
                    int iSampleTestPlanArrayListIndex = 0;
                    for (int i = 0; i < m_SampleTestPlan.Count; i++)
                    {
                        testDieInfo = (DIEINFO_TAG[])m_SampleTestPlan[i];
                        if (m_oKHeader.InspectionTest == testDieInfo[i].TEST)
                        {
                            iSampleTestPlanArrayListIndex = i;
                        }
                    }

                    testDieInfo = (DIEINFO_TAG[])m_SampleTestPlan[iSampleTestPlanArrayListIndex];
                    for (int i = 0; i < testDieInfo.Length; i++)
                    {
                        strDBData[1] = testDieInfo[i].TEST.ToString();  //InspectionTest
                        strDBData[2] = testDieInfo[i].DX.ToString();	//INDEX_X				INTEGER NOT NULL
                        strDBData[3] = testDieInfo[i].DY.ToString();	//INDEX_Y				INTEGER NOT NULL,

                        iBatchCnt += 1;

                        if (iBatchCnt == m_iDefectInsertSum || i == testDieInfo.Length - 1)
                        {
                            sQueryValues += "("
                                + strDBData[0] + "," + strDBData[1] + ","
                                + strDBData[2] + "," + strDBData[3] + ")";
                            iBatchCnt = 0;

                        }
                        else
                        {
                            sQueryValues += "("
                                + strDBData[0] + "," + strDBData[1] + ","
                                + strDBData[2] + "," + strDBData[3] + ")";
                        }

                        strQuery[0] = sQueryPrefix + sQueryValues;
                        oYesScanSampleTx.Create(strQuery);
                        strQuery[0] = "";
                        sQueryValues = "";
                    }
                    //Logging.WriteLog(string.Format("X: {0} , Y: {1}",strDBData[2],strDBData[3] ),"PROC",0);
                    //						oYesScanSampleTx.Create(strDBData);
                }
                //oYesScanSampleTx.Create(strQuery);

            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("ScanSample Upload Error [step_seq : {0}]", step_seq), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }
            finally
            {
                //ContextUtil.SetComplete();
                oYesScanSampleTx.Dispose();
            }
        }

        #endregion

        #region ■UPDB Defect -> TQD_DEFECT
        public void UploadDataDefect(long wafer_seq, long step_seq, string option)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oYesDefectTx = null;
            DACrux.SEMDMS.DSL.TQD_IMAGES oYesImageTx = null;

            int iCurrDefectID = 0;
            bool bImport = false;
            string strWrite = string.Empty;
            StreamWriter srLDFILE = null;
            string strTTable = string.Empty;
            string strBatchSql = string.Empty;
            string strBatchSqlValues = string.Empty;
            int iBatchCnt = 0;
            int iDefectCnt = m_iDefectCount;
            int iDefectCntIncr = 0;

            try
            {
                oYesDefectTx = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oYesImageTx = new DACrux.SEMDMS.DSL.TQD_IMAGES();

                string[] strDBData = new string[28] {step_seq.ToString(),"-1",wafer_seq.ToString(),
														"NN","NN","NN","NN","0","0","0","0","0","0",
														"0","0","0","0","0","0","0","FSTSIL","0","0",
														"0","0","0","0","0"};
                strTTable = "TQD_DEFECT";

                // 2013-09-11 YSIM Normal 한 방법으로 Data를 올린다.
                //if (option == "IMPORT" || option == "CMD")
                //{
                //    bImport = true;				//Import방식을 이용하여  DB에 올린다.
                //}
                //else
                //{
                //    strTTable = string.Format("SESSION.{0}_{1}", EQUIP_ID, Environment.TickCount.ToString());
                //    strTTable = strTTable.Replace("-", "");
                //    oYesDefectTx.CreateTempTable(new string[] { strTTable });
                //}

                if (oYesDefectTx.GetDefectCount(strDBData) == 0)
                {
                    if (bImport) srLDFILE = new StreamWriter(m_oPath.CSVFile); 		//1. File을 연다

                    strBatchSql += "INSERT INTO " + strTTable + " values";

                    for (int i = 0; m_iDefectCount > i; i++)
                    {
                        // 테스트 별로 Step_Sep 를 생성하여 인써트
                        int iTestNo = 0;

                        if (iTestNo == m_oKHeader.InspectionTest)
                        {
                            iDefectCntIncr++;

                            strDBData.Initialize();
                            strDBData[1] = "-1";
                            strDBData[3] = "NN";
                            strDBData[4] = "NN";
                            strDBData[5] = "NN";
                            strDBData[6] = "NN";
                            strDBData[20] = "FIRST";

                            for (int j = 0; 27 > j; j++)
                            {
                                if (m_iParmSpecIndex[j] == 0)
                                {
                                    strDBData[m_iParmSpecIndex[j]] = "0";
                                }
                                else if (j == 19)
                                {
                                    strDBData[19] = m_oKHeader.Defects[i].DValue[j].ToString();
                                }
                                else if (m_iParmSpecIndex[j] > 0)
                                {
                                    strDBData[m_iParmSpecIndex[j]] = m_oKHeader.Defects[i].DValue[j].ToString();
                                }
                            }
                            strDBData[0] = step_seq.ToString();
                            //strDBData[1] = "0";									//트랜젝션 TEST를 위해 강제 Error 발생

                            strDBData[3] = (m_oKHeader.DiePitchX * RK * double.Parse(strDBData[7]) + double.Parse(strDBData[5]) + m_oKHeader.DieOriginX).ToString();
                            strDBData[4] = (m_oKHeader.DiePitchY * RK * double.Parse(strDBData[8]) + double.Parse(strDBData[6]) + m_oKHeader.DieOriginY).ToString();
                            strDBData[2] = wafer_seq.ToString();
                            strDBData[20] = "1";								//ADDER                INTEGER,
                            strDBData[21] = m_oKHeader.StepID;					//FIRST_STEP           VARCHAR(40),
                            strDBData[22] = "-1";								//RETICLE_REPEAT_ID    INTEGER NOT NULL DEFAULT 0,
                            strDBData[23] = "-1";								//DIE_REPEAT_ID        INTEGER NOT NULL DEFAULT 0,
                            strDBData[24] = "0";								//MANA_OPT_CLASS        INTEGER NOT NULL DEFAULT 0,
                            strDBData[25] = "0";								//AUTO_OPT_CLASS       INTEGER NOT NULL DEFAULT 0,
                            strDBData[26] = "0";								//MAN_SEM_CLASS        INTEGER NOT NULL DEFAULT 0,
                            strDBData[27] = "0";								//AUTO_SEM_CLASS       INTEGER NOT NULL DEFAULT 0

                            DataCheck(ref strDBData);

                            iCurrDefectID = int.Parse(strDBData[1]);
                            if (bImport)
                            {
                                srLDFILE.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27}", strDBData);
                            }
                            else
                            {

                                /// oYesDefectTx.Create(strDBData);
                                /// Temp Table 생성 -> TQD_DEFECT으로 Insert into select 방안사용
                                /// ===========================================================================
                                /// 
                                iBatchCnt += 1;

                                if (iBatchCnt == m_iDefectInsertSum || iDefectCntIncr == iDefectCnt || i == m_iDefectCount - 1)
                                {
                                    strBatchSqlValues += "(";
                                    strBatchSqlValues += strDBData[0] + "," + strDBData[1] + "," + strDBData[2] + "," + strDBData[3] + ",";
                                    strBatchSqlValues += strDBData[4] + "," + strDBData[5] + "," + strDBData[6] + "," + strDBData[7] + ",";
                                    strBatchSqlValues += strDBData[8] + "," + strDBData[9] + "," + strDBData[10] + "," + strDBData[11] + ",";
                                    strBatchSqlValues += strDBData[12] + "," + strDBData[13] + "," + strDBData[14] + "," + strDBData[15] + ",";
                                    strBatchSqlValues += strDBData[16] + "," + strDBData[17] + "," + strDBData[18] + "," + strDBData[19] + ",";
                                    strBatchSqlValues += strDBData[20] + ",'" + strDBData[21] + "'," + strDBData[22] + "," + strDBData[23] + ",";
                                    strBatchSqlValues += strDBData[24] + "," + strDBData[25] + "," + strDBData[26] + "," + strDBData[27] + ")";
                                    iBatchCnt = 0;

                                }
                                else
                                {
                                    strBatchSqlValues += "(";
                                    strBatchSqlValues += strDBData[0] + "," + strDBData[1] + "," + strDBData[2] + "," + strDBData[3] + ",";
                                    strBatchSqlValues += strDBData[4] + "," + strDBData[5] + "," + strDBData[6] + "," + strDBData[7] + ",";
                                    strBatchSqlValues += strDBData[8] + "," + strDBData[9] + "," + strDBData[10] + "," + strDBData[11] + ",";
                                    strBatchSqlValues += strDBData[12] + "," + strDBData[13] + "," + strDBData[14] + "," + strDBData[15] + ",";
                                    strBatchSqlValues += strDBData[16] + "," + strDBData[17] + "," + strDBData[18] + "," + strDBData[19] + ",";
                                    strBatchSqlValues += strDBData[20] + ",'" + strDBData[21] + "'," + strDBData[22] + "," + strDBData[23] + ",";
                                    strBatchSqlValues += strDBData[24] + "," + strDBData[25] + "," + strDBData[26] + "," + strDBData[27] + ")";
                                }
                            }

                            oYesDefectTx.Create(strBatchSql + strBatchSqlValues);
                            strBatchSqlValues = "";

                            if (m_oKHeader.Defects[i].IMAGENAME != null)
                            {
                                ImageLoader tmpLoader = new ImageLoader(m_oKHeader.Defects[i].IMAGENAME[0]);
                                tmpLoader.SaveImage(m_oKHeader.Defects[i].IMAGENAME[1], 0, IMAGEFORMAT.JPG);

                                oYesImageTx.CreateImages(new string[]{step_seq.ToString()
																,strDBData[1]
																,"0"
																,m_oKHeader.InspectionTest.ToString()
																,wafer_seq.ToString()
																,"JPG" 
																,Path.GetDirectoryName(m_oKHeader.Defects[i].IMAGENAME[1])
																,Path.GetFileName(m_oKHeader.Defects[i].IMAGENAME[1])
																,m_UpdateServerIP 
																,tmpLoader.GetThumeNail(100,100,m_oKHeader.Defects[i].IMAGENAME[1])});
                                tmpLoader.Dispose();
                            }
                        }
                    }

                    if (bImport)
                    {
                        srLDFILE.Close();
                        srLDFILE = null;

                        if (option == "CMD")
                        {
                            oYesDefectTx.Import2(m_oPath.CSVFile);
                        }
                        else
                        {
                            oYesDefectTx.Import(m_oPath.CSVFile);
                        }
                    }
                    else
                    {
                        if (strTTable != "TQD_DEFECT")
                            oYesDefectTx.MoveDefect(new string[] { strTTable }, new string[] { step_seq.ToString() });
                    }

                    //oYesDefectTx.UpdateAdder(new String[3]{wafer_seq.ToString(),step_seq.ToString(),"20000" });
                }
                else
                {
                    //	한번더 올리거나,Review를 했거나, 등등...
                }
                //ContextUtil.SetComplete();
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectData Upload Error [DefectID : {0}]", iCurrDefectID), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]" + ex.StackTrace, ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                //ContextUtil.SetAbort();
                throw this.ProcessErr(ex);
            }

            finally
            {
                if (srLDFILE != null) srLDFILE.Close();
                //if (File.Exists(m_oPath.CSVFile)) File.Delete(m_oPath.CSVFile);
                oYesDefectTx.Dispose();
                oYesImageTx.Dispose();
            }
        }

        #endregion

        #region ■Defect Data를 Check한다.
        public void DataCheck(ref string[] DefectData)
        {
            try
            {
                int iTmpValue = 0;
                if (DefectData[1] != "-1")
                {
                    if (DefectData[3] == "NN")
                    {
                        iTmpValue = (int)(m_oKHeader.DiePitchX * RK) * int.Parse(DefectData[7]) + int.Parse(DefectData[5]) - (int)(m_oKHeader.SampleCenterLocationX * RK);
                        DefectData[3] = iTmpValue.ToString();
                    }
                    if (DefectData[4] == "NN")
                    {
                        iTmpValue = (int)(m_oKHeader.DiePitchY * RK) * int.Parse(DefectData[8]) + int.Parse(DefectData[6]) - (int)(m_oKHeader.SampleCenterLocationY * RK);
                        DefectData[4] = iTmpValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region ■UpdateDB Defect -> TQD_DEFECT
        public void UpdateDataDefect(long lot_seq, long wafer_seq, long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oYesDefectTx = null;
            DACrux.SEMDMS.DSL.TQD_IMAGES oYesImageTx = null;
            DataTable dtImgCnt;
            DataView dvImgCnt;

            string[] sUpdateDefectQuery = null;
            string strWrite = string.Empty;
            string strThumbName = string.Empty;
            string strTemp = string.Empty;
            int iCurrDefectID = 0;
            int iCurrImageCount;
            int iTemp;
            int iBatchCnt = 0;

            try
            {
                oYesDefectTx = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oYesImageTx = new DACrux.SEMDMS.DSL.TQD_IMAGES();

                dtImgCnt = oYesImageTx.GetImageCount(new string[1] { step_seq.ToString() });
                dvImgCnt = new DataView(dtImgCnt);

                sUpdateDefectQuery = new string[2];
                sUpdateDefectQuery[0] = "";
                sUpdateDefectQuery[1] = "";
                string[] strDBData = new string[28] {step_seq.ToString(),"-1",wafer_seq.ToString(),
														"NN","NN","NN","NN","0","0","0","0","0","0",
														"0","0","0","0","0","0","0","FSTSIL","0","0",
														"0","0","0","0","0"};

                //string[] strImage = new string[10] {step_seq.ToString(),"1","0","1",wafer_seq.ToString(),"JPG"," "," ","177.175.2.186"," "};
                string[] strImage = new string[10] { step_seq.ToString(), "1", "0", "1", wafer_seq.ToString(), "JPG", " ", " ", "177.176.2.24", " " };


                for (int i = 0; m_iDefectCount > i; i++)
                {
                    strDBData.Initialize();
                    strDBData[1] = "-1";
                    strDBData[3] = "NN";
                    strDBData[4] = "NN";
                    strDBData[5] = "NN";
                    strDBData[6] = "NN";
                    strDBData[20] = "FIRST";
                    iCurrImageCount = 0;
                    iTemp = 0;

                    for (int j = 0; 27 > j; j++)
                    {
                        if (m_iParmSpecIndex[j] == 0)
                        {
                            strDBData[m_iParmSpecIndex[j]] = "0";
                        }
                        else if (j == 19)
                        {
                            strDBData[19] = m_oKHeader.Defects[i].DValue[j].ToString();
                        }
                        else if (m_iParmSpecIndex[j] > 0)
                        {
                            strDBData[m_iParmSpecIndex[j]] = m_oKHeader.Defects[i].DValue[j].ToString();
                        }
                    }

                    strDBData[3] = (m_oKHeader.DiePitchX * RK * double.Parse(strDBData[7]) + double.Parse(strDBData[5]) + m_oKHeader.DieOriginX).ToString();
                    strDBData[4] = (m_oKHeader.DiePitchY * RK * double.Parse(strDBData[8]) + double.Parse(strDBData[6]) + m_oKHeader.DieOriginY).ToString();
                    strDBData[0] = step_seq.ToString();
                    strDBData[2] = wafer_seq.ToString();
                    strDBData[20] = "1";								//ADDER                INTEGER,
                    strDBData[21] = m_oKHeader.StepID;					//FIRST_STEP           VARCHAR(40),
                    strDBData[22] = "0";								//RETICLE_REPEAT_ID    INTEGER NOT NULL DEFAULT 0,
                    strDBData[23] = "0";								//DIE_REPEAT_ID        INTEGER NOT NULL DEFAULT 0,
                    strDBData[24] = "0";								//MANA_OPT_CLASS        INTEGER NOT NULL DEFAULT 0,
                    strDBData[25] = "0";								//AUTO_OPT_CLASS       INTEGER NOT NULL DEFAULT 0,
                    strDBData[26] = "0";								//MAN_SEM_CLASS        INTEGER NOT NULL DEFAULT 0,
                    strDBData[27] = "0";								//AUTO_SEM_CLASS       INTEGER NOT NULL DEFAULT 0

                    iCurrDefectID = int.Parse(strDBData[1]);
                    ///==============================================================================
                    /// TQD_DEFECT-IMAGECOUNT에 기존 Data와 현재 들어갈 Image 개수를 합산한다.
                    ///==============================================================================
                    dvImgCnt.RowFilter = string.Format("DEFECTID = {0}", strDBData[1]);
                    if (dvImgCnt.Count > 0)
                    {
                        iCurrImageCount = int.Parse(strDBData[19]) + int.Parse(dvImgCnt[0]["NEXT_IMAGE_ID"].ToString());
                        strDBData[19] = iCurrImageCount.ToString();
                    }
                    ///------------------------------------------------------------------------------

                    if (strDBData[13] != "0" || strDBData[16] != "0" || strDBData[17] != "0" || strDBData[18] != "0" || strDBData[19] != "0")
                    {
                        //						Logging.WriteLog(string.Format("DefectData Update [DefectID:{0}][CLASSNUMBER:{1}][ROUGHBINNUMBER:{2}][FINEBINNUMBER:{3}][REVIEWSAMPLE:{4}][IMAGECOUNT:{5}]"
                        //														,strDBData[1],strDBData[13],strDBData[16],strDBData[17],strDBData[18],strDBData[19]),"PROC",0);

                        iBatchCnt += 1;
                        if (iBatchCnt == 1000 || i == m_iDefectCount - 1)
                        {
                            sUpdateDefectQuery[0] += "UPDATE TQD_DEFECT "
                                + "SET CLASSNUMBER = " + strDBData[13]
                                + " ,ROUGHBINNUMBER = " + strDBData[16]
                                + " ,FINEBINNUMBER = " + strDBData[17]
                                + " ,REVIEWSAMPLE = " + strDBData[18]
                                + " ,IMAGECOUNT = " + strDBData[19]
                                + " WHERE "
                                + " WAFER_SEQ = " + wafer_seq.ToString()
                                + " AND STEP_SEQ = " + step_seq.ToString()
                                + " AND DEFECTID = " + strDBData[1];
                            iBatchCnt = 0;
                        }
                        else
                        {
                            sUpdateDefectQuery[0] += "UPDATE TQD_DEFECT "
                                + "SET CLASSNUMBER = " + strDBData[13]
                                + " ,ROUGHBINNUMBER = " + strDBData[16]
                                + " ,FINEBINNUMBER = " + strDBData[17]
                                + " ,REVIEWSAMPLE = " + strDBData[18]
                                + " ,IMAGECOUNT = " + strDBData[19]
                                + " WHERE "
                                + " WAFER_SEQ = " + wafer_seq.ToString()
                                + " AND STEP_SEQ = " + step_seq.ToString()
                                + " AND DEFECTID = " + strDBData[1];
                        }

                        oYesDefectTx.UpdateBatch(sUpdateDefectQuery);
                        sUpdateDefectQuery[0] = "";
                    }
                    //Logging.WriteLog("DefectData Update Completed","PROC",0);

                    if (m_oKHeader.Defects[i].IMAGENAME != null)
                    {
                        Logging.WriteLog(string.Format("{0} Images Exist", m_oKHeader.Defects[i].IMAGENAME.Length), "PROC", 0);
                        iTemp = iCurrImageCount + 1;
                        if (dvImgCnt.Count > 0)
                        {
                            strTemp = m_oKHeader.Defects[i].IMAGENAME[1].Replace(string.Format("_{0}_{1}_20", strDBData[1].PadLeft(6, '0'), "0".PadLeft(2, '0'))
                                , string.Format("_{0}_{1}_20", strDBData[1].PadLeft(6, '0'), iTemp.ToString().PadLeft(2, '0')));

                            strImage[6] = Path.GetDirectoryName(strTemp);
                            strImage[7] = Path.GetFileName(strTemp);
                            if (!m_oKHeader.Defects[i].IMAGENAME[1].Equals(strTemp))
                            {
                                Logging.WriteLog(string.Format("Image File Rename [{0} to {1}]", m_oKHeader.Defects[i].IMAGENAME[1], strTemp), "PROC", 0);
                                File.Move(m_oKHeader.Defects[i].IMAGENAME[1], strTemp);
                                Logging.WriteLog("Image File Rename Completed ", "PROC", 0);
                            }
                        }
                        else
                        {
                            strImage[6] = Path.GetDirectoryName(m_oKHeader.Defects[i].IMAGENAME[1]);
                            strImage[7] = Path.GetFileName(m_oKHeader.Defects[i].IMAGENAME[1]);
                            strTemp = m_oKHeader.Defects[i].IMAGENAME[1];
                        }

                        strImage[1] = strDBData[1];
                        strImage[2] = iTemp.ToString();
                        strImage[3] = m_oKHeader.InspectionTest.ToString();
                        strImage[5] = "JPG";
                        strImage[8] = m_UpdateServerIP;

                        ImageLoader tmpLoader = new ImageLoader();
                        Logging.WriteLog(string.Format("Create Thumbnail Image From {0}", m_oKHeader.Defects[i].IMAGENAME[1]), "PROC", 0);
                        strImage[9] = tmpLoader.GetThumeNail(100, 100, m_oKHeader.Defects[i].IMAGENAME[0]);
                        Logging.WriteLog("Create Thumbnail Image - Completed", "PROC", 0);

                        Logging.WriteLog(string.Format("Image Data Insert [STEP_SEQ:{0}][DEFECTID:{1}][IMAGE_ID:{2}][TEST:{3}][WAFER_SEQ:{4}][IMAGE_TYPE:{5}][IMAGE_PATH:{6}][IMAGE_FILENAME:{7}][IMAGE_SERVER:{8}]"
                            , strImage[0], strImage[1], strImage[2], strImage[3], strImage[4], strImage[5], strImage[6], strImage[7], strImage[8]), "PROC", 0);

                        oYesImageTx.CreateImages(strImage);
                        Logging.WriteLog("Image Data Insert Completed", "PROC", 0);

                        tmpLoader.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog(string.Format("DefectData Update Error [DefectID : {0}]", iCurrDefectID), "ERROR", 0);
                Logging.WriteLog(string.Format("Error Message [{0}]", ex.Message), "ERROR", 1);
                ///--------------------------------------------------------------------------------------------
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-02-18
                ///	- 변경로그 : tif 파일에 실제 이미지가 없는 경우 m_oImgLoader.GetThumeNail 에서 개체 오류로 전체 프로세스가 중단된다.
                ///	             따라서 이미지를 로드하지 못하여 발생하는 에러를 throw 하지 않고 로깅만 한다.
                //throw this.ProcessErr(ex);	
                /// ====================================================================
            }
            finally
            {
                oYesDefectTx.Dispose();
                oYesImageTx.Dispose();
            }
        }

        #endregion

        private void PreInsertNewInsp()
        {
            AccuracyCheck();

            /// ====================================================================
            ///	- 수정자 : 미라콤 임영신
            ///	- 수정일 : 2005-03-10
            ///	- 변경로그 : 함수 추가 
            ///		MES의 MainEq,RTE,Oper,Reticle 정보를 가져온다.				
            //GetMESInfo();
            /// ====================================================================

            /// ROTATE 3월 16일 추가 ================================================
            RotatePoint rp = new RotatePoint();
            try
            {
                //rp.RotateKLARF(ref m_oKHeader); 
                rp.RotateKLARF(ref m_oKHeader, ref m_SampleTestPlan);
            }
            catch
            {
                Logging.WriteLog("Rotate중 Error가 났습니다.이 Data는 정상 Data가 아니지만 Debug를 위해 Upload를 진행합니다.", "PROC", 0);
            }
            /// ========================================================================
        }

        #region ■Step을 DB로 Insert함
        //public void InsertNewInsp()
        public int InsertNewInsp()
        {
            int iReturn = 0;
            string strLog = string.Empty;
            long llot_seq = -1;
            long lwafer_seq = -1;
            long lsetup_seq = -1;
            long lstep_seq = -1;
            System.DateTime oCurrentTime;
            System.TimeSpan oFileWaitTime;
            DataTable dtExist = null;
            int iDefectCount = 0;
            try
            {
                //				AccuracyCheck();
                //
                //				/// ====================================================================
                //				///	- 수정자 : 미라콤 임영신
                //				///	- 수정일 : 2005-03-10
                //				///	- 변경로그 : 함수 추가 
                //				///		MES의 MainEq,RTE,Oper,Reticle 정보를 가져온다.				
                //				GetMESInfo();
                //				/// ====================================================================
                //		
                //				/// ROTATE 3월 16일 추가 ================================================
                //				RotatePoint rp = new RotatePoint();
                //				try
                //				{
                //					rp.RotateKLARF(ref m_oKHeader);
                //				}
                //				catch
                //				{
                //					Logging.WriteLog("Rotate중 Error가 났습니다.이 Data는 정상 Data가 아니지만 Debug를 위해 Upload를 진행합니다.","PROC",0);
                //				}
                //				/// ========================================================================

                TQD_STEP oYesStepTx = new TQD_STEP();
                /// 
                /// 1. Wafer ID가 없을때
                /// 2. Wafer ID 길이가 0일때
                /// 3. Wafer ID 길이가 3자리 이상일때(3자리를 용서하는 이유 @)
                /// ==============================================================================================================================================================

                // STEP_SEQ 조건에 TEST 추가(2005.05.31 강혜경)
                if (m_oKHeader.WaferID == null || m_oKHeader.WaferID.Length == 0 || m_oKHeader.WaferID.Length > 3)
                {
                    //oExistDs = oYesStepTx.GetExistInspSlot(new string[3] {m_oKHeader.LotID,m_oKHeader.Slot.ToString().PadLeft(2,'0'),m_oKHeader.ResultTimestamp});
                    dtExist = oYesStepTx.GetExistInspSlot(new string[4] { m_oKHeader.LotID, m_oKHeader.Slot.ToString().PadLeft(2, '0'), m_oKHeader.ResultTimestamp, m_oKHeader.InspectionTest.ToString() });
                }
                else
                {
                    //oExistDs = oYesStepTx.GetExistInspWaferID(new string[4] {m_oKHeader.LotID,m_oKHeader.WaferID,m_oKHeader.StepID,m_oKHeader.ResultTimestamp});
                    dtExist = oYesStepTx.GetExistInspWaferID(new string[5] { m_oKHeader.LotID, m_oKHeader.WaferID, m_oKHeader.StepID, m_oKHeader.ResultTimestamp.ToString(), m_oKHeader.InspectionTest.ToString() });
                }
                /// ==============================================================================================================================================================

                /// oExistDs = oYesStepTx.GetExistInspWaferID(new string[4] {m_oKHeader.LotID,m_oKHeader.WaferID,m_oKHeader.StepID,m_oKHeader.ResultTimestamp});

                if (dtExist.Rows.Count > 0)
                {
                    /// 이미 KLARF Key 값에 의한 Data가 DB에 존재 했을때 Update만 실행한다.
                    llot_seq = long.Parse(dtExist.Rows[0]["LOT_SEQ"].ToString());
                    lwafer_seq = long.Parse(dtExist.Rows[0]["WAFER_SEQ"].ToString());
                    lsetup_seq = long.Parse(dtExist.Rows[0]["SETUP_SEQ"].ToString());
                    lstep_seq = long.Parse(dtExist.Rows[0]["STEP_SEQ"].ToString());
                    /// ====================================================================
                    ///	- 수정자 : 미라콤 임영신
                    ///	- 수정일 : 2005-03-12
                    ///	- 변경로그 :  아래 로직을 추가함
                    ///	Step까지는 존재하고(WAFER,STEP)
                    /// Defect 의 갯수가 존재 하지 않는다.(Defect 갯수가 0)
                    /// TQD_DEFECT에서 Step_seq를 기준으로 Defect을 Count해야함
                    /// Defect Count가 0일때 조치사항
                    /// step_sum (delete Step_seq)
                    /// insp_info (delete Step_seq)
                    /// Step (delete Step_seq)
                    /// Insert Step,insp_info,Defect,Image,step_sum
                    /// 
                    //					Logging.WriteLog(strLog,"PROC",0);
                    //					UpdateDataStep(lstep_seq);
                    //					UpdateDataDefect(llot_seq,lwafer_seq,lstep_seq);					
                    //					m_strDataIntegrate = "UPDATE";
                    //					m_oKHeader.Defects.Initialize();

                    TQD_DEFECT oDefectCount = new TQD_DEFECT();
                    iDefectCount = oDefectCount.GetDefectCount(lstep_seq.ToString());
                    if (iDefectCount > 0)
                    {
                        strLog = string.Format("[LOT_SEQ : {0}] [WAFER_SEQ : {1}] [SETUP_SEQ : {2}] [STEP_SEQ : {3}] is Exist", llot_seq, lwafer_seq, lsetup_seq, lstep_seq);
                        Logging.WriteLog(strLog, "PROC", 0);
                        UpdateDataStep(lstep_seq);
                        UpdateDataDefect(llot_seq, lwafer_seq, lstep_seq);
                        m_strDataIntegrate = "UPDATE";
                        //m_oKHeader.Defects.Initialize();
                    }
                    else
                    {
                        strLog = string.Format("[LOT_SEQ : {0}] [WAFER_SEQ : {1}] [SETUP_SEQ : {2}] [STEP_SEQ : {3}] is Exist. Defect Counts is Zero. Retry !", llot_seq, lwafer_seq, lsetup_seq, lstep_seq);
                        Logging.WriteLog(strLog, "PROC", 0);
                        RollBackStepInfo(lstep_seq);
                        return -1;
                    }
                    /// ====================================================================
                }
                else
                {
                    m_strDataIntegrate = "INSERT";
                    //Product Data를 Upload한다.
                    if (UploadDataProduct() == 1)
                    {
                        strLog = string.Format("[DEVICE : {0} ]는 이미 DB에 존재합니다.", m_oKHeader.DeviceID);
                    }
                    else
                    {
                        strLog = string.Format("[DEVICE : {0} ]가 DB에 Insert 되었습니다.", m_oKHeader.DeviceID);
                    }
                    Logging.WriteLog(strLog, "PROC", 0);

                    //Lot Data를 upload한다.
                    if (UploadDataLot(ref llot_seq) == 1)
                    {
                        strLog = string.Format("[LOT : {0} ]는 이미 DB에 존재합니다.", m_oKHeader.LotID);
                    }
                    else
                    {
                        strLog = string.Format("[LOT : {0} ] [LOT_SEQ : {1} ]가 Insert되었습니다.", m_oKHeader.LotID, llot_seq.ToString());
                    }
                    Logging.WriteLog(strLog, "PROC", 0);

                    //Wafer Data를 upload한다.
                    if (llot_seq >= 0)
                    {
                        if (UploadDataWafer(llot_seq, ref lwafer_seq) == 1)
                        {
                            strLog = string.Format("[WAFER : {0} ]는 이미 DB에 존재합니다.", m_oKHeader.WaferID);
                        }
                        else
                        {
                            strLog = string.Format("[WAFER : {0} ] [WAFER_SEQ : {1} ]가 Insert되었습니다.", m_oKHeader.WaferID, lwafer_seq.ToString());
                        }
                    }
                    else
                    {
                        strLog = string.Format("[WAFER : {0} ] Insert를 하지 못했습니다.", m_oKHeader.WaferID);
                    }
                    Logging.WriteLog(strLog, "PROC", 0);

                    //Setup Data를 Upload한다
                    if (lwafer_seq >= 0)
                    {
                        if (UploadDataSetup(ref lsetup_seq) == 1)
                        {
                            strLog = string.Format("[SETUP : {0} ] 는 이미 DB에 존재합니다.", m_oKHeader.SetupID);
                        }
                        else
                        {
                            strLog = string.Format("[SETUP : {0} ]가 Insert되었습니다.", m_oKHeader.SetupID);
                        }
                    }
                    else
                    {
                        strLog = string.Format("[SETUP : {0} ] Insert를 하지 못했습니다.", m_oKHeader.SetupID);
                    }
                    Logging.WriteLog(strLog, "PROC", 0);

                    //ScanSample과 Step Data를 Upload한다.
                    if (lsetup_seq >= 0 && lwafer_seq >= 0)
                    {
                        UploadDataStep(lwafer_seq, lsetup_seq, ref lstep_seq);
                        Logging.WriteLog("DB Upload Step", "PROC", 0);
                        UploadDataScanSample(lstep_seq);
                        Logging.WriteLog("DB Upload ScanSample", "PROC", 0);

                    }
                    else
                    {
                        strLog = string.Format("[SETUP : {0} ] Insert를 하지 못했습니다.", m_oKHeader.SetupID);
                    }

                    //Defect Data를 Upload한다
                    if (lstep_seq >= 0)
                    {
                        UploadDataDefect(lwafer_seq, lstep_seq, m_oPath.UploadComand);
                        Logging.WriteLog("DB Upload Defect", "PROC", 0);
                        //m_oKHeader.Defects.Initialize();
                    }

                    Logging.WriteLog(string.Format("[PRODUCT : {0}] [LOT : {1}] [WAFER : {2}] [STEP : {3} ] Upload 완료", m_oKHeader.DeviceID, m_oKHeader.LotID, m_oKHeader.WaferID, m_oKHeader.StepID), "PROC", 0);

                }

                try
                {
                    StepSummary oStepSum = new StepSummary();
                    Logging.WriteLog(string.Format("STEP Summary Skip."), "PROC", 0);
                    //oStepSum.RunStepSummary(lstep_seq);
                    //Logging.WriteLog(string.Format("STEP Summary End"), "PROC", 0);
                    if (m_iDefectCount != 0)
                        oStepSum.RunStepDCount(lstep_seq);
                    Logging.WriteLog(string.Format("DCOUNT Summary End"), "PROC", 0);


                    /// ====================================================================
                    ///   - 수정자 : 미라콤 임영신
                    ///   - 수정일 : 2005-02-17
                    ///   - 변경로그 : M10만 있는 기능이므로 분기 처리함
                    /// 
                    if (m_FAB.Equals("M10"))
                    {
                        oStepSum.WriteStepSumXML(lstep_seq);
                        Logging.WriteLog(string.Format("XML Write 끝"), "PROC", 0);
                    }
                    /// ====================================================================					

                }
                catch
                {
                    Logging.WriteLog("[XML File Write 실패]", "ERROR", 0);
                }


                ///RDMS Check=============================================================================
                try
                {
                    Logging.WriteLog("[Check RDMS]", "PROC", 0);
                    //Miracom.YES.RDMS.BSL.FilteringRDMS ftrRDMS = new Miracom.YES.RDMS.BSL.FilteringRDMS();

                    //ftrRDMS.FindRD((int)lstep_seq);
                    //ContextUtil.SetComplete();
                }
                catch (Exception ex)
                {
                    Logging.WriteLog("[Check RDMS]" + ex.Message, "ERROR", 0);
                }
                ///=======================================================================================

                ///InterLock Check=============================================================================
                try
                {
                    Logging.WriteLog("[Check Interlock]", "PROC", 0);
                    //Miracom.YES.InterLock.BSL.Check ChkInterLock = new Miracom.YES.InterLock.BSL.Check();
                    string strInterLockFlag = "";
                    //switch (ChkInterLock.GetInterLockInfo(lstep_seq))
                    //{
                    //    case 0:
                    //        break;
                    //    case 1:
                    //        strInterLockFlag = "CONTROL_LIMIT_OUT";
                    //        Logging.WriteLog(string.Format("CONTROL Out !!!"), "PROC", 0);
                    //        break;
                    //    case 2:
                    //        strInterLockFlag = "SPEC_LIMIT_OUT";
                    //        ///SendMessage("LOT_HOLD");
                    //        ///
                    //        Logging.WriteLog(string.Format("SPEC Out [LOT HOLD] !!!"), "PROC", 0);

                    //        break;
                    //}

                    if (strInterLockFlag.Length > 0)
                    {
                        TQD_INSP_INFO oYesInspInfo = new TQD_INSP_INFO();
                        TQD_INTERLOCK_SPEC oYesInterlockSpec = new TQD_INTERLOCK_SPEC();
                        bool bEquipHold = true;
                        DataTable dtInspInfo = oYesInspInfo.CheckEquipHold(m_oKHeader.DeviceID, m_oKHeader.StepID, m_MainEq, lstep_seq);
                        DataTable dtInterLock = oYesInterlockSpec.GetInterLockSpec(m_oKHeader.DeviceID, m_oKHeader.StepID, m_MainEq);
                        if (dtInterLock != null && dtInterLock.Rows.Count > 0 && dtInspInfo.Rows.Count > int.Parse(dtInterLock.Rows[0]["REPEAT_COUNT"].ToString()))
                        {
                            for (int i = 0; i < dtInterLock.Rows.Count; i++)
                            {
                                bEquipHold = true;
                                for (int j = 0; j < int.Parse(dtInterLock.Rows[0]["REPEAT_COUNT"].ToString()); i++)
                                {
                                    if (dtInspInfo.Rows[j]["INTERLOCK_FLAG"] == null
                                        || dtInspInfo.Rows[j]["INTERLOCK_FLAG"].ToString().Length == 0)
                                    {
                                        bEquipHold = false;
                                    }
                                }
                                if (bEquipHold) break;
                            }

                        }
                        else
                        {
                            bEquipHold = false;
                        }

                        if (bEquipHold)
                        {
                            //장비 HOLD
                            Logging.WriteLog(string.Format("CONTROL BATCH [EQUIP HOLD] !!!"), "PROC", 0);
                        }

                        oYesInspInfo.UpdateInterlockFlag(lstep_seq, strInterLockFlag);
                        //ContextUtil.SetComplete();
                        oYesInspInfo.Dispose();
                    }
                }
                catch
                {
                    Logging.WriteLog("[Check Interlock]", "ERROR", 0);
                }
                ///=======================================================================================

                ///
                /// DB에 Wafer Map을 Image로 맹길어 넣어놓는다.
                /// Image를 어디다 Save하라는 요청이 없었음
                /// 따라서, Path가 필요없어 넣어놓지 않았음
                /// 향후 요청이 오면 Path를 만들고 ,큰 Map Image를 만들어 저장한후 정보를 아래의 "UNKNOWN" 칸에 넣어야 함
                /// ===========================================================================================================
                TQD_MAP_IMAGES oYesMapImages = new TQD_MAP_IMAGES();
                try
                {
                    if (m_iDefectCount != 0)
                    {
                        //Miracom.YES.LIB.MapGen.MapGenerate.GenerateFormat(m_oPath.ProcessPath, lstep_seq.ToString(), 199, Miracom.YES.LIB.MapGen.MapGenerate.GenerateFormatEnum.JPEG);
                        //Logging.WriteLog("[WAFER Map Create 성공]", "PROC", 0);
                        //oYesMapImages.Create(new string[] {lstep_seq.ToString()
                        //                                                     ,m_oKHeader.InspectionTest.ToString()
                        //                                                     ,"JPEG"
                        //                                                     ,"UNKNOWN"  
                        //                                                     ,"UNKNOWN"
                        //                                                     ,"UNKNOWN"
                        //                                                     ,string.Format(@"{0}\{1}.JPG",m_oPath.ProcessPath,lstep_seq)});

                        Logging.WriteLog("[Upload to Database 성공]", "PROC", 0);
                    }
                }
                catch
                {
                    Logging.WriteLog("[Map Image Upload to Database 실패]", "ERROR", 0);
                }
                finally
                {
                    oYesMapImages.Dispose();
                    if (File.Exists(string.Format(@"{0}\{1}.JPG", m_oPath.ProcessPath, lstep_seq.ToString())))
                    {
                        File.Delete(string.Format(@"{0}\{1}.JPG", m_oPath.ProcessPath, lstep_seq.ToString()));
                    }
                }
                ///============================================================================================================


                //ContextUtil.SetComplete();

                oCurrentTime = DateTime.Now;
                oFileWaitTime = oCurrentTime - m_oStartTime;

                Logging.WriteLog(string.Format("{0,8} {1,16} {2,16} {3,16} {4,7} {5,7} {6,5} {7}", m_oKHeader.InspectionStationID[2]
                                                                            , m_oKHeader.LotID
                                                                            , m_oKHeader.WaferID
                                                                            , m_oKHeader.StepID
                                                                            , m_iDefectCount.ToString()
                                                                            , m_iTotalImageCount.ToString()
                                                                            , oFileWaitTime.Seconds.ToString()
                                                                            , m_strDataIntegrate
                                                                            ), "UPHIST", 0);

                ///[LOG]---------------------------------------------------------------------------------------
                Logging.WriteLog("DB Upload가 정상 완료되었습니다. ", "PROC", 0);
                ///--------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                Logging.WriteLog(string.Format("[PRODUCT : {0}] [LOT : {1}] [WAFER : {2}] [STEP : {3} ] Upload 실패", m_oKHeader.DeviceID, m_oKHeader.LotID, m_oKHeader.WaferID, m_oKHeader.StepID), "ERROR", 0);
                //ContextUtil.SetAbort();
                //throw this.ProcessErr(ex);
            }
            finally
            {
                m_lCurrentStepSeq = lstep_seq;		///현재 Update완료된 Step Seq값을 유지 한다.
                //m_bMultiWaferCutLine = false;
                m_strSubParse = "";
            }
            return iReturn;
        }

        #endregion

        #region ■Structure를 Initialize함
        public void KStructInit()
        {
            try
            {
                m_oKHeader.InspectionStationID = new string[] { " ", " ", " " };
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                throw this.ProcessErr(ex);
            }

            finally
            {
            }
        }

        #endregion

        #region ■Data AccuacyCheck Logic부문
        public void AccuracyCheck()
        {
            try
            {
                /// ====================================================================
                ///	- 수정자 : 미라콤 임영신
                ///	- 수정일 : 2005-03-02
                ///	- 변경로그 : DeviceID 를 무조건 LOTID  앞 3자리, 대문자로 처리함, 권택수 과장님과 협의완료
                ///					
                //				if(m_oKHeader.DeviceID == null || m_oKHeader.DeviceID.Length == 0)
                //				{
                //					if(m_oKHeader.LotID.Length>=3)
                //					{
                //						m_oKHeader.DeviceID = m_oKHeader.LotID.Substring(0,3);
                //					}
                //					else
                //					{
                //						m_oKHeader.DeviceID = m_oKHeader.LotID;
                //					}
                //				}				

                if (m_oKHeader.LotID.Length >= 3)
                {
                    if (m_FAB.Equals("RND"))
                    {
                        //m_oKHeader.DeviceID = m_oKHeader.LotID.Substring(0, 4).ToUpper();
                    }
                    else
                    {
                        //m_oKHeader.DeviceID = m_oKHeader.LotID.Substring(0, 3).ToUpper();
                    }
                }
                else
                {
                    // m_oKHeader.DeviceID = m_oKHeader.LotID.ToUpper();
                }
                /// ====================================================================

                if (m_oKHeader.ResultID == null || m_oKHeader.ResultID.Length == 0)
                {
                    m_oKHeader.ResultID = m_oKHeader.StepID;
                }

                if (m_oKHeader.WaferID == null || m_oKHeader.WaferID.Length > 2 || m_oKHeader.WaferID.Length == 0)
                {
                    m_oKHeader.WaferID = m_oKHeader.Slot.ToString().PadLeft(2, '0');
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, "ERROR", 3);
                throw this.ProcessErr(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region ■KLARF를 Pre Parsing함
        /// <summary>
        /// Image가 전부 들어왔는지 확인
        /// 보통은 Image가 먼저 들어오고 
        /// 나중에 KLARF가 들어옴
        /// Abnormal한 경우를 대비하여 행하여짐
        ///
        /// </summary>
        /// <param name="klafile"></param>
        /// <returns></returns>
        public bool PreParsingWB3200(string klafile)
        {
            bool bImageWait = true;
            string strLine = string.Empty;
            string strPath = string.Empty;
            string[] strField;

            try
            {
                if (!File.Exists(klafile.Replace("D_Category", "DefMsr")))
                {
                    bImageWait = false;
                }
                else
                {
                    //FAB 구분 추가==========================================
                    m_FAB = FABCheck();
                    //=======================================================

                    m_srWB3200 = new StreamReader(klafile.Replace("D_Category", "TskDat"));
                    m_oKHeader = new KLARF_HEADER_TAG();
                    KStructInit();

                    m_arrImgSourceList = new ArrayList();

                    strPath = Path.GetDirectoryName(klafile);

                    while ((strLine = m_srWB3200.ReadLine()) != null)
                    {
                        strLine = strLine.Trim();
                        strField = SplitLine(strLine.Replace(";", "").Trim());

                        if (strField[0].ToUpper().Equals("TIFFFILENAME"))
                        {
                            if (!File.Exists(string.Format(@"{0}\{1}", strPath, strField[1])))
                            {
                                bImageWait = false;
                                break;
                            }
                            else
                            {
                                m_arrImgSourceList.Add(string.Format(@"{0}\{1}", strPath, strField[1]));
                            }
                        }
                        else if (strField[0].ToUpper().Equals("LOTNO"))
                        {
                            //PrsLOTID(strField);
                        }
                        else if (strField[0].ToUpper().Equals("WAFERID"))
                        {
                            PrsWAFERID(strField, false);
                        }
                        else if (strField[0].ToUpper().StartsWith("MACHINENO"))
                        {
                            PrsINSPECTIONSTATIONID(strField);
                        }
                        else if (strField[0].ToUpper().Equals("PROCESSPRM"))
                        {
                            PrsSTEPID(strField);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(string.Format("KLARF File wrong : MSG[{0}]", ex.Message), "READDATA", 0);
                bImageWait = false;
            }

            finally
            {
                if (m_srWB3200 != null)
                {
                    m_srWB3200.Close();
                }
            }
            return bImageWait;
        }

        #endregion

        #region ■KLARF를 Parsing하기 시작할때 Log를 남김
        public void CreateUpdateLog(string Facility, string EquipID, string StartTime)
        {
            DACrux.SEMDMS.DSL.TQD_UPDATELOG oYesUpdatelog = null;
            try
            {
                oYesUpdatelog = new DACrux.SEMDMS.DSL.TQD_UPDATELOG();
                oYesUpdatelog.Create(new string[] { Facility, EquipID, StartTime });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                oYesUpdatelog.Dispose();
            }
        }

        #endregion

        #region ■KLARF를 Parsing 종료후 Log를 남김
        public void UpdateUpdateLog(string EquipID
            , string StartTime
            , string EndTime
            , string SvcTime
            , string TrType
            , string SuccessFlag
            , long StepSeq
            , string ResultFile
            , string ErrorFile
            , string BackupFile
            , int Defects
            , int Images)
        {
            DACrux.SEMDMS.DSL.TQD_UPDATELOG oYesUpdatelog = null;
            try
            {
                oYesUpdatelog = new DACrux.SEMDMS.DSL.TQD_UPDATELOG();
                oYesUpdatelog.Update(new string[] { EndTime, SvcTime, TrType, SuccessFlag, StepSeq.ToString(), ResultFile, ErrorFile, BackupFile, Defects.ToString(), Images.ToString(), EquipID, StartTime });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                oYesUpdatelog.Dispose();
            }
        }
        #endregion

        #region ■오류가 발생한 Step 에 대한 처리
        /// <summary>
        /// step_sum (delete Step_seq)
        /// insp_info (delete Step_seq)
        /// Step (delete Step_seq)
        /// </summary>
        /// <param name="stepSeq"></param>
        private void RollBackStepInfo(long stepSeq)
        {
            this.TraceStartPoint();

            TQD_STEP_SUM oYesStepSum = null;
            TQD_INSP_INFO oYesInspInfo = null;
            TQD_STEP oYesStep = null;
            try
            {
                oYesStepSum = new TQD_STEP_SUM();
                oYesInspInfo = new TQD_INSP_INFO();
                oYesStep = new TQD_STEP();
                oYesStepSum.Delete(stepSeq.ToString());
                oYesInspInfo.Delete(stepSeq.ToString());
                oYesStep.Delete(stepSeq.ToString());

                //ContextUtil.SetComplete();
            }
            catch (Exception ex)
            {
                //ContextUtil.SetAbort();
                //throw this.ProcessErr(ex);
                string strLog = string.Format("Error:[TQD_STEP_SUM, TQD_INSP_INFO TQD_STEP의 {0}을 삭제하던 중 에러가 발생하였습니다:" + ex.Message, stepSeq);
                Logging.WriteLog(strLog, "PROC", 0);

            }
            finally
            {
                if (oYesStepSum != null) oYesStepSum.Dispose();
                if (oYesInspInfo != null) oYesInspInfo.Dispose();
                if (oYesStep != null) oYesStep.Dispose();

                this.TraceEndPoint();
            }

        }
        #endregion


        public string FABCheck()
        {
            DACrux.SEMDMS.DSL.TQD_CONFIG oYesConfig = null;
            DataTable dt = null;

            try
            {
                oYesConfig = new TQD_CONFIG();
                dt = oYesConfig.Get(new string[] { "INTERFACE", "FAB" });

                if (dt.Rows.Count > 0)
                {
                    m_FAB = dt.Rows[0]["VALUE"].ToString().Trim();
                }
                return m_FAB;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                if (oYesConfig != null) oYesConfig.Dispose();
            }
        }
    }
}
