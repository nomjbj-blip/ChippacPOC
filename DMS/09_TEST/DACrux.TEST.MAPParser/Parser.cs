using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SP.Common;

namespace DACrux.TEST.MAPParser
{

    public class Parser
    {
        #region [변수]
        string strProductValue = string.Empty;
        string strRunValue = string.Empty;
        string strWaferValue = string.Empty;
        string strFlatValue = string.Empty;
        string strXValue = string.Empty;
        string strYValue = string.Empty;
        string strTotalValue = string.Empty;
        string strWaferNOValue = string.Empty;
        string strBinCode = string.Empty;
        string strValue = string.Empty;
        //string key = null;
        string oldProduct = "old";
        string oldRun = "old";

        // Config Value
        string strSPRPath = string.Empty;
        string strMapPath = string.Empty;
        int iLogLevel = 0;
        static bool bUseDBPath = true;

        string strBottom = string.Empty;
        string strRight = string.Empty;
        string strTop = string.Empty;
        string strLeft = string.Empty;
        string strFF = string.Empty;

        DataTable dtBin = null;
        DataTable dtSort = null;
        DataRow drBinSort = null;

        int totalcount = 0;
        //private bool checkRun = false;

        static Analysis oAnalysis = null;
        private readonly string strBin = "bin";
        private readonly string strWafer = "Wafer_ID";
        private readonly string strRun = "Run_ID";
        private readonly string strProduct = "Product";
        private readonly string strFlat = "FlatZone";
        //private readonly string strFN = "180";
        private readonly string strTotal = "Total";
        private readonly string strWaferNum = "Wafer_Num";

        private DateTime dtMakeTime = new DateTime();
        private DateTime dtStartTime = new DateTime();
        private DateTime dtEndTime = new DateTime();
        //static private bool NonKey = false;

        static Log log = new Log();
        DACrux.TEST.DSL.TQP_PROGRAM oProgram = null;
        DACrux.TEST.DSL.T_PRB oPrb = null;
        DACrux.TEST.DSL.TQP_PRODUCT oProduct = null;
        DACrux.TEST.DSL.TQP_WAFER oWafer = null;
        DACrux.TEST.DSL.TQP_LOT oLot = null;
        DACrux.TEST.DSL.TQP_MAPDEF oMapDef = null;
        DACrux.TEST.DSL.TQP_CUSTOMER oCust = null;
        DACrux.TEST.DSL.TQP_USEMAP oUseMap = null;
        DACrux.TEST.DSL.TQP_BINDESC oBinDesc = null;
        DACrux.Map.EditWaferMap m_wMap;
        RowData[] rd = null;

        private ParsingType parsingType;
        private enum ParsingType
        {
            Normal,
            Special,
            NonKey
        }

        // Manual Convert 관련 변수
        private bool IsManualConvert = false;
        private StringBuilder sbData = null;
        private StringBuilder sbLog = null;

        #endregion

        private struct KeyValue
        {
            public string strKey;
            public int iRowNum;
        }

        public struct RowData
        {
            public int DieX;
            public int DieY;
            public int Bin;
            public int HBin;
            public string CharBin;
            public int Site;
            public int VisualInsp;
            public int Avi;
        }

        public void main(string[] args)
        {
            Utility.ARGUMENT_TAG argument = null;

            try
            {
                #region [ 변수 초기화 ]

                // 변수를 초기화 합니다.
                argument = new Utility.ARGUMENT_TAG();
                argument.LOGPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"]; // 임시 로그 경로 설정, 이후 DB에 저장된 경로로 변경됨
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}]", "START"), argument.LOGPath, 0);

                InitGlobalVariables();
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Variables Initialization Complete"), argument.LOGPath, 1);


                #endregion

                #region [ 입력 인자 분석 ]

                // 입력된 인자값을 분석합니다.
                if (ParsingArgument(args, ref argument) == false)
                {
                    // 분석에 실패한 경우 오류 출력 후 파싱을 종료합니다.
                    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "PARSSING FAIL", "입력된 Argument 형식이 잘못되어 있거나 파싱 파일에 대한 DB 정보를 찾을 수 없습니다."), argument.LOGPath, 0);

                    StringBuilder sb = new StringBuilder();

                    foreach (string value in args)
                    {
                        sb.Append(value);
                        sb.Append(" ");
                    }
                    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] Argument : {1}", "PARSSING FAIL", sb.ToString()), argument.LOGPath, 0);

                    return; // Argument 파싱이 되지않아 경로를 알지 못하므로 throw하지 않는다.
                }
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Argument Parsing Complete"), argument.LOGPath, 1);

                FileInfo fi = new FileInfo(argument.ResultPath);

                // 파싱할 파일이 실제 존재하는지 체크합니다.
                if (!fi.Exists)
                {
                    // 존재하지 않을 경우 오류 출력 후 파싱을 종료합니다.
                    throw new Exception(string.Format("{0} [{1}]", "해당 파일은 존재하지 않습니다.", argument.ResultPath));
                }

                if (fi.Extension.ToUpper() == ".REF" || fi.Extension.ToUpper() == ".REX" || fi.Extension.ToUpper() == ".REQ" || fi.Extension.ToUpper() == ".RPY")
                {
                    // REF, REQ 파일일 경우 Backup 폴더로 이동하고 메시지 출력 후 파싱을 종료합니다.
                    MoveFile("BACKUP", fi.FullName, argument);
                    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1} ({2})", "PARSSING FAIL", "대상 파일이 아니므로 백업하고 종료합니다.", argument.ResultPath), argument.LOGPath, 0);
                    return; // 해당 파일은 Error 폴더로 가면 안되기 떄문에 Backup 폴더로 이동 후 종료합니다.
                }

                // 직접(수동) 인자값을 입력한 경우 변환할 Bin의 수를 체크합니다.
                if (!(argument.CusBin.Count == argument.HanaBin.Count))
                {
                    // 같지 않을 경우 오류 출력 후 파싱을 종료합니다.
                    //WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "PARSSING FAIL", "변환되어야 할 BIN의 수량이 다릅니다."), argument.LOGPath, 0);
                    throw new Exception(string.Format("{0} [{1}]", "고객사 Map의 Bin 수량과 변환될 HANA Map의 BIN 수량이 다릅니다.", argument.ResultPath));
                }

                // 파싱 파일의 생성 시간을 저장합니다.
                dtMakeTime = fi.CreationTime;
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "TARGET FILE", argument.ResultPath), argument.LOGPath, 0);

                #endregion

                #region [ 정규식을 이용한 데이터 추출 ]

                // SPR 파일에 정의된 정규식을 이용하여 데이터를 추출합니다.
                RegexAnalysis(argument);
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Regex Analysis Complete"), argument.LOGPath, 1);

                //2011-11-23-이태훈 변수에 값채우기
                strBottom = oAnalysis.strBottom == "" ? "BOTTOM" : oAnalysis.strBottom;
                strRight = oAnalysis.strRight == "" ? "RIGHT" : oAnalysis.strRight;
                strLeft = oAnalysis.strLeft == "" ? "LEFT" : oAnalysis.strLeft;
                strTop = oAnalysis.strTop == "" ? "TOP" : oAnalysis.strTop;

                #endregion

                //#region [ Special Code에 대한 예외 처리 ]

                //// Special Code가 'Y'인 경우
                ////if (argument.strSPCode.ToUpper().Equals("Y"))
                ////{
                ////    parsingType = ParsingType.Special;
                ////    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "SP Code Parsing Start"), argument.LOGPath, 0);
                ////    ParsingForSPCode(ref oAnalysis, ref argument);
                ////    return;
                ////}

                //#endregion

                #region [ Validation 체크 후 파싱 시작 ]

                string validation = string.Empty;
                if (parsingType == ParsingType.Special)
                {
                    if (oAnalysis.values["EOW"].Count != 2)
                        validation = string.Format("ERROR : oAnalysis.values[\"EOW\"].Count({0}) 값이 2가 아닙니다.", oAnalysis.values["EOW"].Count);
                    else
                        validation = "Success";
                }
                else
                {
                    parsingType = ParsingType.Normal;
                    validation = ValidationMap(argument.ResultPath, ref oAnalysis);
                }

                if (!validation.StartsWith("Success"))
                {
                    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "PARSSING FAIL", "Map Format이 상이합니다." + validation), argument.LOGPath, 0);
                    DiffFormat(argument);
                    return;
                }

                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Parsing Start"), argument.LOGPath, 0);

                #endregion

                #region [ Regex 데이터를 재가공 ]

                //Regex 데이터에서 기본 정보와 Bin 정보를 분리하여 저장한다.
                DataRow[] drMapInfo = RemakeRegexData();
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Remaking Regex Data Complete"), argument.LOGPath, 1);

                #endregion

                #region[ Data Insert ]
                totalcount = drMapInfo.Length / (oAnalysis.values.Count - 1);

                int binvaluecount = 0;
                int count = 0;
                int maxX = 0;
                int maxY = 0;
                int minX = 0;
                int minY = 0;


                oProgram = new DSL.TQP_PROGRAM();
                oProduct = new DSL.TQP_PRODUCT();
                oWafer = new DSL.TQP_WAFER();
                oLot = new DSL.TQP_LOT();
                oMapDef = new DSL.TQP_MAPDEF();
                oPrb = new DSL.T_PRB();
                oCust = new DSL.TQP_CUSTOMER();
                oUseMap = new DSL.TQP_USEMAP();
                oBinDesc = new DSL.TQP_BINDESC();
                string m_strCustomer = string.Empty;
                string m_strFacility = string.Empty;
                string m_strMapID = string.Empty;
                string m_strTestArea = string.Empty;
                string m_strLot_Seq = string.Empty;
                string m_strWafer_Seq = string.Empty;
                string m_strWaferStartTime = string.Empty;
                string m_strWaferEndTime = string.Empty;
                string m_strWaferCount = string.Empty;
                string m_strLotStartTime = string.Empty;
                string m_strLotEndTime = string.Empty;
                string strNextLotSeq = string.Empty;
                string strNextWaferSeq = string.Empty;
                string m_strGecBin = string.Empty;
                string strNumBin = string.Empty;
                int TestedDies = 0;


                //각각의 WaferMap을 만들기 위한 구문
                //
                foreach (DataRow data in drMapInfo)
                {
                    count += GetWaferMapInfo(data);

                    if (count > 3)
                    {
                        if (string.IsNullOrEmpty(strWaferValue) && !string.IsNullOrEmpty(strRunValue) && !string.IsNullOrEmpty(strWaferNOValue))
                            strWaferValue = strRunValue + "-" + strWaferNOValue;

                        if (string.IsNullOrEmpty(strProductValue) || string.IsNullOrEmpty(strFlatValue)
                            || string.IsNullOrEmpty(strRunValue) || string.IsNullOrEmpty(strWaferValue))
                        {
                            throw new Exception(string.Format("ERROR : 변수가 모두 매치되지 않았습니다.\n[Product:{0}], [WaferID:{1}], [RunID:{2}], [FlatZone:{3}]",
                                                                        strProductValue, strWaferValue, strRunValue, strFlatValue));
                        }

                        count = 0;
                        int goodcnt = 0;
                        int DieID = 1;

                        //GEC Bin 조회
                        //Good Bin 수량 체크에 필요
                        DataTable dtBinDesc = oBinDesc.SelectGecBin(strProductValue);
                        if (dtBinDesc.Rows.Count > 0)
                            m_strGecBin = dtBinDesc.Rows[0][0].ToString();

                        StringBuilder sbBin = new StringBuilder();
                        strYValue = (dtBin.Rows.Count / totalcount).ToString();
                        strXValue = ((int)dtBin.Rows[0][2] / oAnalysis.CharCount).ToString();

                        for (int binrowcount = binvaluecount; binrowcount < (dtBin.Rows.Count / totalcount) + binvaluecount; binrowcount++)
                        {
                            if (binvaluecount >= dtBin.Rows.Count)
                                break;

                            int start = (int)dtBin.Rows[binrowcount][1];
                            for (int a = 0; a < (int)dtBin.Rows[binrowcount][2]; a += oAnalysis.CharCount)
                            {

                                string hbin = Utility.ConvertBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                //string hbin = Utility.CusBinToHanaBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                sbBin.Append(hbin);
                                if (!hbin.Equals(".") && !hbin.Equals(" "))
                                {
                                    drBinSort = dtSort.NewRow();
                                    drBinSort["DieID"] = DieID;
                                    drBinSort["Y"] = (binvaluecount == 0 ? binrowcount + 1 : (binrowcount % binvaluecount) + 1);
                                    drBinSort["X"] = (a / oAnalysis.CharCount) + 1;
                                    //drBinSort["HBin"] = Utility.HanaBinToInt(hbin); //Utility.ConvertBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                    if (hbin == m_strGecBin)
                                    {
                                        goodcnt++;
                                        //strNumBin = Utility.HanaBinToInt(hbin).ToString();
                                    }
                                    //drBinSort["Bin"] = Utility.HanaBinToInt(hbin);
                                    drBinSort["CharBin"] = strValue.Substring(start, oAnalysis.CharCount);
                                    drBinSort["SITE"] = "0";
                                    drBinSort["VISUALINSP"] = "0";
                                    drBinSort["AVI"] = "0";

                                    dtSort.Rows.Add(drBinSort);
                                    DieID++;
                                }
                                start += oAnalysis.CharCount;
                            }
                            sbBin.Append("\n");
                        }
                        if (oldProduct.Equals(strProductValue) && oldRun.Equals(strRunValue))
                        {
                        }
                        else
                        {
                            oldProduct = strProductValue;
                            oldRun = strRunValue;
                            //checkRun = true;
                        }

                        WriteLog("Convert START", strProductValue + "   /    " + strWaferValue.Replace("\n", "") + " : " + argument.ResultPath, argument.LOGPath, 2);

                        sbBin = new StringBuilder();
                        WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "DB Insert START", strProductValue + "   /    " + strWaferValue.Replace("\n", "") + " : " + argument.ResultPath), argument.LOGPath, 2);

                        DataTable dtWaferSeq = oPrb.SelectMaxWaferSeq();
                        DataTable dtCustomer = oCust.GetCustomerList(args[1].ToString().Replace("^^^", " ").Replace("\"", ""));
                        DataTable dtMapDef = oMapDef.GetMapDef(strProductValue);

                        //Map 존재여부 확인
                        //Map이 존재하지 않는 경우 Map ID(Map ID ->Product Name) 생성
                        if (dtMapDef.Rows.Count == 0)
                        {
                            maxX = int.Parse(dtSort.Rows[0]["X"].ToString());
                            maxY = int.Parse(dtSort.Rows[0]["Y"].ToString());
                            minX = int.Parse(dtSort.Rows[0]["X"].ToString());
                            minY = int.Parse(dtSort.Rows[0]["Y"].ToString());
                            for (int i = 1; i < dtSort.Rows.Count; i++)
                            {
                                //X좌표의 최대값
                                if (int.Parse(dtSort.Rows[i]["X"].ToString()) > maxX)
                                {
                                    maxX = int.Parse(dtSort.Rows[i]["X"].ToString());
                                }

                                //Y좌표의 최대값
                                if (int.Parse(dtSort.Rows[i]["Y"].ToString()) > maxY)
                                {
                                    maxY = int.Parse(dtSort.Rows[i]["Y"].ToString());
                                }

                                //X좌표의 최소값
                                if (int.Parse(dtSort.Rows[i]["X"].ToString()) < minX)
                                {
                                    minX = int.Parse(dtSort.Rows[i]["X"].ToString());
                                }

                                //Y좌표의 최소값
                                if (int.Parse(dtSort.Rows[i]["Y"].ToString()) < minY)
                                {
                                    minY = int.Parse(dtSort.Rows[i]["Y"].ToString());
                                }
                            }
                            //Map 생성
                            MapCreate(strProductValue, int.Parse(strFF), maxX, maxY, minX, minY, DieID - 1);
                        }

                        //Product 존재여부 확인
                        DataTable dtProduct = oProduct.SelectProductInfo(strProductValue);
                        if (dtProduct == null || dtProduct.Rows.Count <= 0)
                        {
                            //존재하지 않는 경우 Product 정보 생성
                            oProduct.SetDeviceDef("HMKB1", strProductValue, dtCustomer.Rows[0]["CUSTOMER_ID"].ToString(), dtCustomer.Rows[0]["CUSTOMER_ID"].ToString(), strProductValue, strProductValue);
                        }
                        dtProduct = oProduct.SelectProductInfo(strProductValue);

                        //Program 존재여부 확인
                        DataTable dtProgram = oProgram.GetProgramInfo(strProductValue);
                        if (dtProgram == null || dtProgram.Rows.Count <= 0)
                        {
                            //존재하지 않는 경우 Program 정보 생성
                            oProgram.CreateProgram(strProductValue, strProductValue, "MULTIPROBE", 90.0, "V1.0", "");
                        }
                        dtProgram = oProgram.GetProgramInfo(strProductValue);

                        m_strCustomer = dtProduct.Rows[0]["CUSTOMER_ID"].ToString();
                        m_strFacility = dtProduct.Rows[0]["FACTORY"].ToString();
                        m_strMapID = dtProduct.Rows[0]["MAPID"].ToString();
                        m_strTestArea = dtProgram.Rows[0]["TESTAREA"].ToString();

                        #region Wafer 헤더 정보 Insert ----------------------------------------

                        // Get Lot Information
                        DataTable dtLot = oLot.GetIslot(dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Rows[0]["PROGRAM"].ToString(), strRunValue);

                        if (dtLot == null || dtLot.Rows.Count <= 0)	// 최초Lot Data
                        {
                            strNextLotSeq = oLot.SelectNextLotSeq();
                            m_strLot_Seq = strNextLotSeq;
                            oLot.CreateLot(m_strLot_Seq, dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Rows[0]["PROGRAM"].ToString(), strRunValue, strRunValue, "", "",
                                                "", m_strFacility, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                "1000", "1", "", "");

                            dtLot = oLot.GetIslot(dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Rows[0]["PROGRAM"].ToString(), strRunValue);

                            m_strLot_Seq = dtLot.Rows[0][0].ToString();
                        }
                        else
                        {
                            m_strLot_Seq = dtLot.Rows[0][0].ToString();
                        }

                        // Wafer_Seq
                        DataTable dtWafer = oWafer.GetIsWafer(m_strLot_Seq, strWaferValue);

                        string m_strPreWafer_Seq = string.Empty;

                        if (dtWafer != null && dtWafer.Rows.Count > 0) // wafer Retest
                        {
                            TestedDies = System.Convert.ToInt32(dtWafer.Rows[0]["TESTED_DIE"].ToString());

                            oWafer.UpdateProbeCnt(m_strLot_Seq, strWaferValue);

                            oWafer.CreateWafer(m_strWafer_Seq, m_strLot_Seq, strWaferValue, "", "", "ADMIN", "0",
                                                TestedDies.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0", (DieID - goodcnt).ToString(), "0",
                                                "0", "0", "", "", "", "");

                            m_strPreWafer_Seq = dtWafer.Rows[0]["WAFER_SEQ"].ToString();

                            dtWafer = oWafer.GetIsWafer(m_strLot_Seq, strWaferValue);

                            m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                        }
                        else	// Wafer 최초 Test
                        {
                            // :lot_seq, :wafer_id, :tester, :probe_card, :operator, :probe_cnt,
                            // :tested_die, :start_time, :end_time, :wafer_cat, :loss_die, :isp_initem, 
                            // :visual_item, :isp_outitem, :isp_incmt ,:visual_cmt, :isp_outcmt, :fvi_flag
                            strNextWaferSeq = oWafer.SelectNextWaferSeq();
                            m_strWafer_Seq = strNextWaferSeq;
                            oWafer.CreateWafer(m_strWafer_Seq, m_strLot_Seq, strWaferValue, "", "", "ADMIN", "0",
                                                (DieID - 1).ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0", (DieID - 1 - goodcnt).ToString(), "0",
                                                "0", "0", "", "", "", "");

                            dtWafer = oWafer.GetIsWafer(m_strLot_Seq, strWaferValue);
                            m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                        }

                        // 해당 Lot Seq의 Wafer수, StartTime, EndTime
                        m_strWaferStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        m_strWaferEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        m_strWaferCount = oWafer.GetWaferCount(m_strLot_Seq).Rows[0][0].ToString();
                        m_strLotStartTime = dtWafer.Rows[0]["START_TIME"].ToString();
                        m_strLotEndTime = dtWafer.Rows[0]["END_TIME"].ToString();

                        // Lot에 Wafer수 EndTime Update
                        oLot.UpdateCount(m_strLotStartTime, m_strLotEndTime, m_strWaferCount, m_strLot_Seq);

                        #endregion ------------------------------------------------------------

                        //Probe Bin Data Insert
                        rd = new RowData[dtSort.Rows.Count];
                        for (int i = 0; i < dtSort.Rows.Count; i++)
                        {
                            rd[i].DieX = Convert.ToInt32(dtSort.Rows[i]["X"].ToString());
                            rd[i].DieY = Convert.ToInt32(dtSort.Rows[i]["Y"].ToString());
                            rd[i].Bin = Convert.ToInt32(dtSort.Rows[i]["BIN"].ToString());
                            rd[i].HBin = Convert.ToInt32(dtSort.Rows[i]["HBin"].ToString());
                            rd[i].CharBin = dtSort.Rows[i]["CharBin"].ToString();
                            rd[i].Site = Convert.ToInt32(dtSort.Rows[i]["SITE"].ToString());
                            rd[i].VisualInsp = Convert.ToInt32(dtSort.Rows[i]["VISUALINSP"].ToString());
                            rd[i].Avi = Convert.ToInt32(dtSort.Rows[i]["AVI"].ToString());

                            oPrb.InsertPrb(strProductValue, m_strWafer_Seq, Convert.ToString(i + 1), rd[i].DieX.ToString(), rd[i].DieY.ToString(), rd[i].Bin.ToString(), rd[i].HBin.ToString(), rd[i].CharBin, rd[i].Site.ToString(), rd[i].VisualInsp.ToString(), rd[i].Avi.ToString());
                        }

                        dtEndTime = DateTime.Now;
                        //checkRun = false;
                        dtSort.Clear();
                        strWaferValue = null;
                        binvaluecount += (dtBin.Rows.Count / totalcount);
                    }
                }

                if (count > 0)
                {
                    throw new Exception(string.Format("ERROR : 변수가 모두 매치되지 않았습니다.\n[Product:{0}], [WaferID:{1}], [RunID:{2}], [FlatZone:{3}]",
                                                                        strProductValue, strWaferValue, strRunValue, strFlatValue));
                }

                MoveFile("BACKUP", argument.ResultPath, argument);
                WriteLog("COMPLETE", argument.ResultPath, argument.LOGPath, 0);

                //Lot Summary, Wafer Summary Procedure 호출
                oPrb.CallProcedure(m_strTestArea, strProductValue, strProductValue, m_strWafer_Seq, strNumBin);

                #endregion
            }
            catch (Exception ex)
            {
                if (argument.ErrorPath != null && argument.ResultPath != null)
                {
                    MoveFile("ERROR", argument.ResultPath, argument);

                    dtEndTime = DateTime.Now;

                    WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "main", ex.Message), argument.LOGPath, 0);
                    WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), argument.LOGPath, 1);
                }
            }
            finally
            {
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}]", "PARSSING END"), argument.LOGPath, 1);
            }
        }

        #region [Method]

        // 전역 변수 초기화
        private void InitGlobalVariables()
        {
            try
            {
                dtBin = new DataTable();
                dtBin.Columns.Add("name");
                dtBin.Columns.Add("Start", typeof(int));
                dtBin.Columns.Add("End", typeof(int));

                dtSort = new DataTable();
                dtSort.Columns.Add("DieID");
                dtSort.Columns.Add("X");
                dtSort.Columns.Add("Y");
                dtSort.Columns.Add("Bin");
                dtSort.Columns.Add("HBin");
                dtSort.Columns.Add("CharBin");
                dtSort.Columns.Add("SITE");
                dtSort.Columns.Add("VISUALINSP");
                dtSort.Columns.Add("AVI");


                strSPRPath = System.Configuration.ConfigurationManager.AppSettings["SPRPath"];
                strMapPath = System.Configuration.ConfigurationManager.AppSettings["MapPath"];
                iLogLevel = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LogLevel"]);
                bUseDBPath = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseDBPath"]);

                Log.iLogLevel = iLogLevel;
                //oMap = new EMS.DA.MAPParser.BSL.HanaMap();
            }
            catch (Exception)
            {
                //WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "InitVariable", ex.Message), argument.LOGPath, 0);
                //WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), argument.LOGPath, 1);
            }
        }

        // Argument 파싱
        private bool ParsingArgument(string[] strArgs, ref Utility.ARGUMENT_TAG argValues)
        {
            bool bResult = true;

            try
            {
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Argument Parsing Start"), argValues.LOGPath, 2);

                DACrux.TEST.DSL.TQP_CUSTOMER oCustomer = new DSL.TQP_CUSTOMER();

                for (int i = 0; i < strArgs.Length; i++)
                {
                    if (strArgs[i].StartsWith("-") == false)
                        continue;

                    

                    switch (strArgs[i].ToUpper())
                    {
                        case "-R":
                            // 변환될 파일
                            argValues.ResultPath = strArgs[i + 1].ToString().Replace("^^^", " ").Replace("\"", "");
                            break;

                        case "-P":
                            // spr 파일
                            argValues.FullFileName = strArgs[i + 1].ToString().Replace("^^^", " ").Replace("\"", "");

                            break;
                        case "-X":
                            // X 좌표의 시작점
                            argValues.X = int.Parse(strArgs[i + 1].ToString().Replace("^^^", " ").Replace("\"", ""));
                            break;
                        case "-Y":
                            // Y 좌표의 시작점
                            argValues.Y = int.Parse(strArgs[i + 1].ToString().Replace("^^^", " ").Replace("\"", ""));
                            break;

                        case "-D":
                            // DB에서 가져와야할 데이터 명
                            argValues.DBData = strArgs[i + 1].ToString().Replace("^^^", " ").Replace("\"", "");
                            DataTable dt = null;
                            //20150323 맵경로로 리스트 조회 필요함
                            dt = oCustomer.GetCustomerList(argValues.DBData);
                            if (dt == null || dt.Rows.Count < 1)
                            {
                                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "PARSSING FAIL", "ParsingArgument : SelectCustomer"), argValues.LOGPath, 0);
                                bResult = false;
                                continue;
                            }

                            argValues.BackupPath = dt.Rows[0]["BAKDIR"].ToString();
                            argValues.LOGPath = dt.Rows[0]["LOGDIR"].ToString();
                            argValues.ErrorPath = dt.Rows[0]["ERRDIR"].ToString();
                            argValues.FullFileName = dt.Rows[0]["SPR_NAME"].ToString();

                            if (bUseDBPath)
                                argValues.LOGPath = dt.Rows[0]["LOGDIR"].ToString();
                            else
                                argValues.LOGPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

                            argValues.strCustCode = dt.Rows[0]["CUSTCODE"].ToString();

                            dt.Reset();
                            dt = null;
                            //dt = oHana.SelectBin(argValues.strMapseq);

                            //if (dt == null)
                            //{
                            //    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "PARSSING FAIL", "ParsingArgument : SelectBin"), argValues.LOGPath, 0);
                            //    bResult = false;
                            //    continue;
                            //}

                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    argValues.CusBin.Add(dr["FROMBIN"].ToString());
                            //    argValues.HanaBin.Add(dr["HBIN"].ToString());
                            //}

                            break;
                        case "-C":
                            // 고객사 Bin
                            if (!(strArgs[i + 1].ToString().IndexOf("-") > -1))
                                argValues.CusBin.Add(strArgs[i + 1].ToString().Replace("\"", ""));
                            else
                            {
                                i--;
                                argValues.CusBin.Add(" ");
                            }
                            break;
                        case "-H":
                            // 변환될 Hana Bin
                            argValues.HanaBin.Add(strArgs[i + 1].ToString().Replace("\"", ""));
                            break;
                        case "-L":
                            // Log Path
                            argValues.LOGPath = strArgs[i + 1].ToString().Replace("\"", "");
                            break;

                        case "-E":
                            // Error Path
                            argValues.ErrorPath = strArgs[i + 1].ToString().Replace("\"", "");
                            break;

                        case "-B":
                            // Backup Path
                            argValues.BackupPath = strArgs[i + 1].ToString().Replace("\"", "");
                            break;
                        case "-CN":
                            // Convert Path
                            argValues.CNVPath = strArgs[i + 1].ToString().Replace("\"", "");
                            break;
                        //case "-Q":
                        //    // Queue Path
                        //    argValues.QueuePath = strArgs[i + 1].ToString();
                        //    break;
                    }
                }

                return bResult;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "ParsingArgument", ex.Message), argValues.ErrorPath, 0);
                WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), argValues.ErrorPath, 1);
                throw ex;
            }
            finally
            {
                // DB에 기준 정보가 없는 파일에 대한 처리
                if (bResult == false)
                {
                    string erFile = string.Empty;

                    for (int i = 0; i < strArgs.Length; i++)
                    {
                        if (strArgs[i].StartsWith("-R") == true && ((i + 1) < strArgs.Length))
                        {
                            erFile = strArgs[i + 1];
                            break;
                        }
                    }

                    if (erFile != string.Empty)
                    {
                        FileInfo fi = new FileInfo(erFile);
                        Directory.CreateDirectory(Path.Combine(fi.DirectoryName, "Error"));
                        fi.MoveTo(Path.Combine(fi.DirectoryName, "Error\\") + fi.Name);
                    }
                }
            }
        }

        // 정규식으로 데이터 추출
        private void RegexAnalysis(Utility.ARGUMENT_TAG argValues)
        {
            try
            {
                #region [ SPR 파일 포멧 가져오기 ]

                // 고객사별 SPR 파일을 읽어 맵 포멧을 가져옵니다.
                oAnalysis = Analysis.GetInstance();
                oAnalysis.OpenFormatFile(FormatFileType.Parser, Path.Combine(strSPRPath, argValues.FullFileName));
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "SPR FILE READ", Path.Combine(strSPRPath, argValues.FullFileName)), argValues.LOGPath, 2);

                #endregion

                #region [ 타겟 파일 인코딩 ]

                // 타겟 파일을 인코딩 형식으로 변환하여 저장합니다.
                oAnalysis.SampleFiles.Add(argValues.ResultPath, Utility.ReadStringFromFile(argValues.ResultPath, oAnalysis.encoding));
                strValue = oAnalysis.SampleFiles[argValues.ResultPath].Replace(Environment.NewLine, "\n").TrimStart('\n').TrimEnd('\n').Replace("\r", "");
                //strValue = strValue.Replace(Environment.NewLine, "\n").TrimStart('\n').TrimEnd('\n').Replace("\r", "");
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}] {1}", "ENCODING FILE", oAnalysis.encoding.ToString()), argValues.LOGPath, 2);

                #endregion

                #region [ 데이터 추출 ]

                oAnalysis.values.Clear();

                // SPR 파일에 정의된 엔터티 별로 정규식을 적용하여 해당 데이터를 추출합니다.
                for (int i = 0; i < oAnalysis.Entities.Count; i++)
                {
                    //if (argValues.strSPCode.ToUpper().Equals("N") &&
                    //        (oAnalysis.Entities[i].ToString().ToUpper().Equals("BINCODE") ||
                    //         oAnalysis.Entities[i].ToString().ToUpper().Equals("EOW")))
                    //    continue;

                    if (oAnalysis.Entities[i].ToString().ToUpper().Equals("WAFER_ID") &&
                            string.IsNullOrEmpty(oAnalysis.Entities[strWafer].RegexString) &&
                            string.IsNullOrEmpty(oAnalysis.Entities[i].DefaultValue))
                        continue;

                    if (oAnalysis.Entities[i].ToString().ToUpper().Equals("WAFER_NUM") &&
                            !string.IsNullOrEmpty(oAnalysis.Entities[strWafer].RegexString))
                        continue;

                    if (!string.IsNullOrEmpty(oAnalysis.Entities[i].RegexString) && !string.IsNullOrEmpty(oAnalysis.Entities[i].RegexValueGroupName))
                    {
                        //정규식의 값이 있을 경우
                        oAnalysis.values.Add(oAnalysis.Entities[i].Name
                                                       , GetCaptures(ref strValue
                                                       , oAnalysis.Entities[i].RegexString
                                                       , oAnalysis.Entities[i].RegexValueGroupName
                                                       , oAnalysis.Entities[i].RegexOption));
                    }
                    else if (!string.IsNullOrEmpty(oAnalysis.Entities[strWafer].RegexString) && !string.IsNullOrEmpty(oAnalysis.Entities[i].DefaultValue))
                    {
                        //정규식이 없고 기본 값만 존재할 경우 강제로 WaferID의 위치를 집어 넣음
                        oAnalysis.values.Add(oAnalysis.Entities[i].Name
                                                       , GetCapturesDefault(strValue
                                                       , oAnalysis.Entities[strWafer].RegexString
                                                       , oAnalysis.Entities[strWafer].RegexValueGroupName
                                                       , oAnalysis.Entities[strWafer].RegexOption));
                    }
                    else if (!string.IsNullOrEmpty(oAnalysis.Entities[strWaferNum].RegexString) && !string.IsNullOrEmpty(oAnalysis.Entities[i].DefaultValue))
                    {
                        //정규식이 없고 기본 값만 존재하며 WaferID는 없지만 Wafer Num이 존재할 경우 강제로 Wafer Num의 위치를 집어 넣음
                        oAnalysis.values.Add(oAnalysis.Entities[i].Name
                                                       , GetCapturesDefault(strValue
                                                       , oAnalysis.Entities[strWaferNum].RegexString
                                                       , oAnalysis.Entities[strWaferNum].RegexValueGroupName
                                                       , oAnalysis.Entities[strWaferNum].RegexOption));
                    }
                    //HOON
                    // NOKEY 타지 않는다.
                    //else if (string.IsNullOrEmpty(oAnalysis.Entities[i].RegexValueGroupName))
                    //{
                    //    //정규식이 없고 기본 값만 존재할 경우 강제로 WaferID의 위치를 집어 넣음
                    //    oAnalysis.values.Add(oAnalysis.Entities[i].Name
                    //                                   , GetCapturesNoKey(strValue
                    //                                   , oAnalysis.Entities[strWafer].RegexString
                    //                                   , oAnalysis.Entities[strWafer].RegexOption));
                    //}
                    else
                    {
                        // 기본값만 존재 &&  WaferID가 없는 경우(특수한 경우 : RunID와 WaferNo의 조합으로 만듦)
                        oAnalysis.values.Add(oAnalysis.Entities[i].Name
                                                      , GetCapturesDefault(strValue
                                                      , oAnalysis.Entities[strRun].RegexString
                                                      , oAnalysis.Entities[strRun].RegexValueGroupName
                                                      , oAnalysis.Entities[strRun].RegexOption));
                    }
                }

                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format("[{0, -15}]", "GET DATA"), argValues.LOGPath, 2);

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Regex 데이터에서 기본 정보와 Bin 정보를 분리하여 저장한다.
        private DataRow[] RemakeRegexData()
        {
            DataTable dt = null;
            List<Token> val = null;

            try
            {
                val = new List<Token>();
                var dic = oAnalysis.values;

                dt = dtBin.Clone();

                string key = string.Empty;

                foreach (KeyValuePair<string, List<Token>> k in dic)
                {
                    key = k.Key;
                    val = k.Value.ToList<Token>();

                    for (int valcount = 0; valcount < val.Count; valcount++)
                    {
                        DataRow dr = null;
                        if (!key.Equals(strBin))
                            dr = dt.NewRow();
                        else
                            dr = dtBin.NewRow();

                        dr["name"] = key;
                        dr["Start"] = val[valcount].Index.ToString();
                        dr["End"] = val[valcount].Length.ToString();

                        if (!key.Equals(strBin))
                            dt.Rows.Add(dr);
                        else if (val[valcount].Length != 0)
                            dtBin.Rows.Add(dr);
                    }
                }
                return dt.Select("1=1", "Start ASC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (val != null)
                {
                    val.Clear();
                    val = null;
                }
            }
        }

        //각각의 WaferMap을 만들기 위해 기본 정보를 입력하는 구문
        private int GetWaferMapInfo(DataRow drMapInfo)
        {
            try
            {
                int count = 0;

                switch (drMapInfo[0].ToString().ToLower())
                {
                    case "product": // Nom, sp 동일
                        count++;
                        if ((int)drMapInfo[2] > 0)
                            strProductValue = strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]);
                        else
                            strProductValue = oAnalysis.Entities[strProduct].DefaultValue;
                        break;
                    case "wafer_id":
                        count++;
                        if ((int)drMapInfo[2] > 0)
                        {
                            if (parsingType == ParsingType.Normal)
                                strWaferValue = strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]).Replace(".", "-").Trim();
                            else if (parsingType == ParsingType.Special)
                            {
                                strWaferValue = strValue.Substring((int)drMapInfo[1], strValue.IndexOf("-")) + strValue.Substring((int)drMapInfo[2] - 1, (int)drMapInfo[2]).Trim();
                                strRunValue = strValue.Substring((int)drMapInfo[1], strValue.IndexOf("-") - 1);
                            }
                        }
                        else
                        {
                            count--;
                        }
                        break;
                    case "run_id":
                        if (parsingType == ParsingType.Normal)
                        {
                            count++;
                            if ((int)drMapInfo[2] > 0)
                                strRunValue = strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]);
                            else
                                strRunValue = oAnalysis.Entities[strRun].DefaultValue;
                        }
                        break;
                    case "flatzone":
                        count++;
                        if ((int)drMapInfo[2] > 0)
                            strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft) || string.IsNullOrEmpty(strTop))
                                            ? strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2])
                                            : (strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]) == strBottom ? "BOTTOM" :
                                               strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]) == strRight ? "RIGHT" :
                                               strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]) == strTop ? "TOP" :
                                               strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]) == strLeft ? "LEFT" :
                                               strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2])
                                            );//strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]).Replace(strBottom, "BOTTOM").Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                        else
                            strFlatValue = oAnalysis.Entities[strFlat].DefaultValue.Replace(strBottom, "BOTTOM").Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                        switch (strFlatValue.ToUpper())
                        {
                            // ParsingType.Normal인 경우
                            case "DOWN":
                            case "BOTTOM":
                                strFF = "0";
                                break;
                            case "LEFT":
                                strFF = "90";
                                break;
                            case "RIGHT":
                                strFF = "270";
                                break;
                            case "UP":
                            case "TOP":
                                strFF = "180";
                                break;

                            // ParsingType.Special인 경우
                            case "0":
                                strFF = "180";
                                strFlatValue = "TOP";
                                break;
                            case "3":
                                strFF = "270";
                                strFlatValue = "RIGHT";
                                break;
                            case "5":
                                strFF = "0";
                                strFlatValue = "BOTTOM";
                                break;
                            case "7":
                                strFF = "90";
                                strFlatValue = "LEFT";
                                break;
                        }
                        break;
                    case "total":
                        count++;
                        if ((int)drMapInfo[2] > 0)
                            strTotalValue = strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]);
                        else
                            strTotalValue = oAnalysis.Entities[strTotal].DefaultValue;
                        break;
                    case "wafer_num":
                        if (parsingType == ParsingType.Normal)
                        {
                            count++;
                            if ((int)drMapInfo[2] > 0)
                                strWaferNOValue = strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]);
                            else
                                strWaferNOValue = oAnalysis.Entities[strWaferNum].DefaultValue;
                        }
                        break;
                    case "bincode":
                        if (parsingType == ParsingType.Special)
                        {
                            count++;
                            if ((int)drMapInfo[2] > 0)
                                strBinCode = Convert.ToString(Convert.ToInt32(strValue.Substring((int)drMapInfo[1], (int)drMapInfo[2]), 16), 2);
                            else
                                strBinCode = oAnalysis.Entities[strWaferNum].DefaultValue;
                        }
                        break;
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ 정규식 구문 ]

        //  정규식 구문
        public static List<Token> GetCaptures(ref string strTargetContent, string strRegex, string strGroupName, RegexOptions regexOptions)
        {
            List<Token> lstCapture = new List<Token>();

            Regex regex = new Regex(strRegex, regexOptions | RegexOptions.Compiled);

            foreach (string Group in regex.GetGroupNames())
            {
                if (Group.ToUpper().Equals("REPLACE_SPACE"))
                {
                    List<string> temp = new List<string>();
                    foreach (Match match in Regex.Matches(strTargetContent, strRegex, regexOptions | RegexOptions.Compiled))
                        temp.Add(strTargetContent.Substring(match.Groups[strGroupName].Index, match.Groups[strGroupName].Length));

                    foreach (string oldstring in temp)
                    {
                        if (String.IsNullOrEmpty(oldstring) == false)
                            //strTargetContent = strTargetContent.Replace(oldstring, Regex.Replace(oldstring, "[\\.]? [\\.]?", "", regexOptions | RegexOptions.Compiled));
                            strTargetContent = strTargetContent.Replace(oldstring, Regex.Replace(oldstring, " ", "", regexOptions | RegexOptions.Compiled));
                    }
                }
            }

            if (!string.IsNullOrEmpty(strGroupName))
            {
                lstCapture = (from Match match in regex.Matches(strTargetContent)
                              select new Token() { Index = match.Groups[strGroupName].Index, Length = match.Groups[strGroupName].Length }).ToList();
            }
            else
            {
                MatchCollection matches = regex.Matches(strTargetContent);

                for (int i = 0; i < matches.Count; i++)
                    lstCapture.Add(new Token() { Index = matches[i].Index, Length = matches[i].Length });
            }

            return lstCapture;
        }

        public static List<Token> GetCapturesDefault(string strTargetContent, string strRegex, string strGroupName, RegexOptions regexOptions)
        {
            List<Token> lstCapture = new List<Token>();
            Regex regex = new Regex(strRegex, regexOptions | RegexOptions.Compiled);

            if (!string.IsNullOrEmpty(strGroupName))
            {
                lstCapture = (from Match match in regex.Matches(strTargetContent)
                              select new Token() { Index = match.Groups[strGroupName].Index, Length = 0 }).ToList();
            }
            else
            {
                lstCapture = (from Match match in regex.Matches(strTargetContent)
                              select new Token() { Index = match.Index, Length = 0 }).ToList();
            }

            return lstCapture;
        }

        public static List<Token> GetCapturesNoKey(string strTargetContent, string strRegex, RegexOptions regexOptions)
        {
            List<Token> lstCapture = new List<Token>();

            Regex regex = new Regex(strRegex, regexOptions | RegexOptions.Compiled);


            MatchCollection matches = regex.Matches(strTargetContent);

            for (int i = 0; i < matches.Count; i++)
                lstCapture.Add(new Token() { Index = matches[i].Index, Length = -1 });

            //NonKey = true;
            return lstCapture;
        }

        #endregion

        void ParsingForSPCode(ref Analysis oAnalysis, ref Utility.ARGUMENT_TAG argument)
        {
            try
            {
                DataRow[] drSort = RemakeRegexData();

                int count = 0;
                foreach (DataRow data in drSort)
                {
                    //GetWaferMapInfo(data);
                    #region [ 원본 삭제 예정 ]
                    switch (data[0].ToString().ToLower())
                    {
                        case "product":
                            count += 1;
                            if ((int)data[2] > 0)
                                strProductValue = strValue.Substring((int)data[1], (int)data[2]);
                            else
                                strProductValue = oAnalysis.Entities[strProduct].DefaultValue;

                            break;
                        case "wafer_id":
                            count += 1;
                            if ((int)data[2] > 0)
                            {
                                string sFileName = argument.ResultPath.Substring(argument.ResultPath.LastIndexOf("\\") + 1).Split('.')[0] + "-";
                                if (strValue.Substring((int)data[1], strValue.IndexOf("-")) == sFileName)
                                {
                                    strWaferValue = strValue.Substring((int)data[1], strValue.IndexOf("-")) + strValue.Substring((int)data[2] - 1, (int)data[2]).Trim();
                                    strRunValue = strValue.Substring((int)data[1], strValue.IndexOf("-") - 1);
                                }
                                else
                                {
                                    strWaferValue = sFileName + strValue.Substring((int)data[2] - 1, (int)data[2]).Trim();
                                    strRunValue = sFileName;
                                }
                            }
                            else
                            {
                                count--;
                            }

                            break;

                        case "flatzone":
                            count += 1;
                            if ((int)data[2] > 0)
                                strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft)
                                    || string.IsNullOrEmpty(strTop)) ? strValue.Substring((int)data[1], (int)data[2])
                                    : strValue.Substring((int)data[1], (int)data[2]).Replace(strBottom, "BOTTOM")
                                    .Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                            else
                                strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft)
                                    || string.IsNullOrEmpty(strTop)) ? strValue.Substring((int)data[1], (int)data[2])
                                    : oAnalysis.Entities[strFlat].DefaultValue.Replace(strBottom, "BOTTOM")
                                    .Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                            switch (strFlatValue)
                            {
                                case "5":
                                    strFF = "0";
                                    strFlatValue = "BOTTOM";
                                    break;
                                case "7":
                                    strFF = "90";
                                    strFlatValue = "LEFT";
                                    break;
                                case "3":
                                    strFF = "270";
                                    strFlatValue = "RIGHT";
                                    break;
                                case "0":
                                    strFF = "180";
                                    strFlatValue = "TOP";
                                    break;
                            }
                            break;
                        case "total":
                            count += 1;
                            if ((int)data[2] > 0)
                                strTotalValue = strValue.Substring((int)data[1], (int)data[2]);
                            else
                                strTotalValue = oAnalysis.Entities[strTotal].DefaultValue;
                            break;

                        case "bincode":
                            count += 1;
                            if ((int)data[2] > 0)
                                strBinCode = Convert.ToString(Convert.ToInt32(strValue.Substring((int)data[1], (int)data[2]), 16), 2);
                            else
                                strBinCode = oAnalysis.Entities[strWaferNOValue].DefaultValue;
                            break;


                        // 2012.02.29 삼성 메모리 형태의 신규맵의 추가로 인해 수정
                        case "run_id":
                            count += 1;
                            if ((int)data[2] > 0)
                                strRunValue = strValue.Substring((int)data[1], (int)data[2]);
                            else
                                strRunValue = oAnalysis.Entities[strRun].DefaultValue;
                            break;
                        case "wafer_num":
                            count += 1;
                            if ((int)data[2] > 0)
                                strWaferNOValue = strValue.Substring((int)data[1], (int)data[2]);
                            else
                                strWaferNOValue = oAnalysis.Entities[strWaferNum].DefaultValue;
                            break;
                    }
                    #endregion
                }
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Bin Code 정의 완료"), argument.LOGPath, 2);
                //WriteLog("Bin Code 정의 완료", argument.LOGPath, 2);

                // 2012.02.29 삼성 메모리 형태의 신규맵의 추가로 인해 수정
                if (string.IsNullOrEmpty(strWaferValue))
                {
                    if (string.IsNullOrEmpty(strRunValue) == false && string.IsNullOrEmpty(strWaferNOValue) == false)
                    {
                        strWaferValue = strRunValue + "-" + strWaferNOValue;
                    }
                }

                ParsingCut(dtBin, strWaferValue, strProductValue, argument, strBinCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ParsingCut(DataTable SamsungBin, string StrWaferID, string StrProduct, Utility.ARGUMENT_TAG argument, string strBincode)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add("X", typeof(int));
            dt.Columns.Add("Y", typeof(int));
            dt.Columns.Add("B", typeof(string));
            dt.Columns.Add("H", typeof(string));

            string strLine = string.Empty;
            string xSubTemp = string.Empty;
            string ySubTemp = string.Empty;
            string bSubTemp = string.Empty;

            int nValue = 0;
            try
            {
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Customer Map -> HanaMap 변환"), argument.LOGPath, 2);
                //WriteLog("Customer Map -> HanaMap 변환", argument.LOGPath, 2);
                for (int xCnt = 0; xCnt < SamsungBin.Rows.Count; xCnt++)
                {
                    dr = dt.NewRow();
                    strLine = strValue.Substring((int)SamsungBin.Rows[xCnt]["Start"], (int)SamsungBin.Rows[xCnt]["End"]);
                    xSubTemp = strLine.Substring(strLine.IndexOf("X=") + 2, strLine.IndexOf("Y=") - 2);
                    if (int.TryParse(xSubTemp.Trim(), out nValue))
                        dr["X"] = nValue;
                    else
                        throw new Exception(string.Format("ERROR : xSubTemp ({0}) 값을 숫자로 변환하지 못해 X값을 넣지 못했습니다.", xSubTemp.Trim()));

                    ySubTemp = strLine.Substring(strLine.IndexOf("Y=") + 2, strLine.Length - strLine.IndexOf("B="));
                    if (int.TryParse(ySubTemp.Trim(), out nValue))
                        dr["Y"] = nValue;
                    else
                        throw new Exception(string.Format("ERROR : ySubTemp ({0}) 값을 숫자로 변환하지 못해 Y값을 넣지 못했습니다.", ySubTemp.Trim()));

                    //---------구분을 두 종류 3자리, 2자리-----------

                    bSubTemp = strLine.Substring(strLine.IndexOf("B=") + 2, strLine.Length - strLine.IndexOf("B=") - 2);
                    dr["B"] = bSubTemp.Trim().Replace("#", "");

                    if (int.TryParse(bSubTemp.Replace("#", ""), out nValue))
                    {
                        if (strBincode.Length + 1 <= nValue)
                        {
                            if (nValue > 32)
                            {
                                if (argument.CusBin.Count > 0)
                                {
                                    for (int i = 0; i < argument.CusBin.Count; i++)
                                    {
                                        if (argument.CusBin[i].Equals(nValue.ToString()))
                                        {
                                            dr["H"] = argument.HanaBin[i];
                                            break;
                                        }
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(dr["H"].ToString()))
                                dr["H"] = "D";
                        }
                        else
                            dr["H"] = strBincode[strBincode.Length - nValue];
                    }
                    else
                        throw new Exception(string.Format("ERROR : bSubTemp ({0}) 값을 숫자로 변환하지 못했습니다.", bSubTemp.Replace("#", "")));

                    if (dr["H"].ToString() == "0")
                        dr["H"] = "D";

                    dt.Rows.Add(dr);
                }

                ConvertSort(dt, StrWaferID, StrProduct, argument);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConvertSort(DataTable dt, string StrWaferID, string StrProduct, Utility.ARGUMENT_TAG argument)
        {
            string CNVPath = Path.Combine(argument.CNVPath, StrWaferID + ".ASC");
            if (!Directory.Exists(argument.CNVPath))
                Directory.CreateDirectory(argument.CNVPath);

            StreamWriter writer = new StreamWriter(CNVPath, false, Encoding.Default);
            StringBuilder sbBin = new StringBuilder();
            StringBuilder Harder = new StringBuilder();

            DataView dv1 = new DataView();
            //DataRow DbBinSort = null;
            string[] strBuilderArr = null;
            int dtCnti = 0;

            try
            {
                int dbCntemp = Convert.ToInt32(dt.Compute("MAX([X])", "1=1"));
                int dbMinCntemp = Convert.ToInt32(dt.Compute("MIN([X])", "1=1"));

                dv1 = dt.DefaultView;
                dv1.Sort = "Y ASC, X ASC"; //X와 B가 제대로인 정렬

                // bin 맵 , 굿빈과 널빈 2바이트 같도록 구성 (B 값이 2개, 3개 종류별로 구분)
                for (dtCnti = 0; dtCnti < dv1.Count; )
                {
                    for (int x = dbMinCntemp; x < dbCntemp + 1; x++) // X축 최소값부터 X축 최대값까지 Map파일만 작성
                    {
                        if (dtCnti < dv1.Count)
                        {
                            if (dv1[dtCnti][0].Equals(x))
                            {
                                sbBin.Append(dv1[dtCnti][3]);

                                if (dtCnti < dv1.Count)
                                {
                                    dtCnti++;
                                }
                            }
                            else
                                sbBin.Append(".");
                        }
                        else
                            sbBin.Append(".");
                    }
                    if (dtCnti != dv1.Count)
                        sbBin.Append('\n');
                }

                strBuilderArr = sbBin.ToString().Split('\n');

                dtStartTime = DateTime.Now;
                int DieIDs = 1;
                int Goodcnt = 0;

                for (int i = 0; i < strBuilderArr.Length; i++)
                {
                    for (int j = 0; j < strBuilderArr[i].Length; j++)
                    {
                        DataRow DbBinSort = dtSort.NewRow();
                        if (!strBuilderArr[i][j].Equals('.'))
                        {
                            DbBinSort["DieID"] = DieIDs;
                            DbBinSort["Y"] = i + 1;
                            DbBinSort["X"] = j + 1;
                            DbBinSort["HBin"] = strBuilderArr[i][j];
                            if (strBuilderArr[i][j].ToString() == "1")
                                Goodcnt++;

                            //DbBinSort["Bin"] = Utility.ConvertHBin(strBuilderArr[i][j].ToString());
                            //DbBinSort["Bin"] = Utility.HanaBinToInt(strBuilderArr[i][j].ToString());

                            //strBuilderSecTempArrChar = strBuilderSecTempArr[i][SBST].ToString() + strBuilderSecTempArr[i][SBST + 1].ToString();
                            DbBinSort["CharBin"] = dv1[DieIDs - 1]["B"];
                            DbBinSort["CNVBin"] = dv1[DieIDs - 1]["B"];
                            DieIDs++;

                            dtSort.Rows.Add(DbBinSort);
                        }
                        //SBST += 1;
                    }
                    //SBST = 0;
                }

                string strRef = (Utility.ReferencePoint(strFlatValue, sbBin.ToString()))[0];
                string strXCNT = ((strBuilderArr[0].Length)).ToString();
                string strYCNT = (strBuilderArr.Length).ToString();
                string DieNum = (DieIDs - 1).ToString();

                Harder.AppendLine("DEVICE:" + StrProduct);
                Harder.AppendLine("WAFERID:" + StrWaferID);
                Harder.AppendLine("X:" + strXCNT);
                Harder.AppendLine("Y:" + strYCNT);
                Harder.AppendLine("REFDIE:" + '\n' + sbBin.ToString());
                Harder.AppendLine('#' + StrWaferID + ":    ");
                Harder.AppendLine("FLAT ZONE : " + strFlatValue);

                writer.WriteLine(Harder.ToString());
                writer.Close();

                //----- INSERT----------------------------------------
                dtStartTime = DateTime.Now;
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Convert & DB Insert START"), argument.LOGPath, 2);
                //WriteLog("Convert & DB Insert START", argument.LOGPath, 2);
                //string sLot_Seq = oMap.InsertMap(strProductValue, strWaferValue, strRunValue, strFlatValue, dtSort, /*checkRun*/true, strRef, strXCNT, strYCNT, strFN, strFF, DieNum, argument.strMapseq, argument.strCustCode, Goodcnt, argument.LOGPath);
                dtEndTime = DateTime.Now;

                //EMS.DA.MAPParser.BSL.Parser psr = new EMS.DA.MAPParser.BSL.Parser();
                //if (psr.MapDuplicationCheck(sLot_Seq, strWaferValue))
                //{
                //    MoveFile("ERROR", argument.ResultPath, argument);
                //    WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "Move the file", argument.ResultPath), argument.LOGPath, 2);
                //    oMap.InsertMapResult(dtMakeTime, dtStartTime, dtEndTime, "N", StrWaferID, argument.DBData, argument.ResultPath.Substring(argument.ResultPath.LastIndexOf('\\') + 1), "MAP 수량이 기존 MAP과 다릅니다. 생산관리파트에 문의하시기 바랍니다");
                //    throw new Exception("MAP 수량이 기존 MAP과 다릅니다. 생산관리파트에 문의하시기 바랍니다");
                //}

                //oMap.InsertMapResult(dtMakeTime, dtStartTime, dtEndTime, "Y", StrWaferID, argument.DBData, argument.ResultPath.Substring(argument.ResultPath.LastIndexOf('\\') + 1), "");
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Convert & Inser 완료"), argument.LOGPath, 2);
                //WriteLog("Convert & Inser 완료", argument.LOGPath, 2);
                StrWaferID = null;


                MoveFile("BACKUP", argument.ResultPath, argument);

                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1} : {2}", "", "COMPLETE", argument.ResultPath), argument.LOGPath, 3);
                WriteLog(string.Format("[{0, 15}]", "PARSER"), string.Format(" {0, -15}  {1}", "", "Parsing Complete"), argument.LOGPath, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DiffFormat(Utility.ARGUMENT_TAG argument)
        {
            // 포멧이 다른 파일을 Error 폴더로 이동합니다.
            try
            {
                MoveFile("ERROR", argument.ResultPath, argument);
                dtEndTime = DateTime.Now;
                //oMap.InsertMapResult(dtMakeTime, dtStartTime, dtEndTime, "N", "", argument.DBData, argument.ResultPath.Substring(argument.ResultPath.LastIndexOf('\\') + 1), "Map format Is different.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ParsingForNonKey(ref Analysis oAnalysis, ref Utility.ARGUMENT_TAG argument)
        {
            try
            {
                int nTemp = 0;

                StringBuilder sbBin = new StringBuilder();
                string[] strReadToLine = strValue.Split('\n');

                if (int.TryParse(oAnalysis.Entities[strProduct].RowNum, out nTemp) == false)
                    throw new Exception(string.Format("Product.RowNum 값을 숫자로 변경할 수 없습니다. Product.RowNum = '{0}'", oAnalysis.Entities[strProduct].RowNum));

                string strKeyTemp = strReadToLine[nTemp - 1];

                for (int a = 0; a < strReadToLine.Length; a++)
                {
                    if (strReadToLine[a].Equals(strKeyTemp))
                        totalcount++;
                }

                DataRow[] drSort = RemakeRegexData();


                #region[Data Insert]
                int binvaluecount = 0;
                //각각의 WaferMap을 만들기 위한 구문
                //
                for (int counts = 1; counts <= totalcount; counts++)
                {

                    foreach (DataRow data in drSort)
                    {

                        switch (data[0].ToString().ToLower())
                        {
                            case "product":

                                if ((int)data[2] < 0)
                                {
                                    if (int.TryParse(oAnalysis.Entities[data[0].ToString()].RowNum, out nTemp) == false)
                                        throw new Exception(string.Format("oAnalysis.Entities[data[0].ToString()].RowNum 값을 숫자로 변경할 수 없습니다. [Product] oAnalysis.Entities[data[0].ToString()].RowNum = '{0}'", oAnalysis.Entities[data[0].ToString()].RowNum));

                                    strProductValue = strReadToLine[int.Parse(oAnalysis.Entities[data[0].ToString()].RowNum) - 1
                                           + strReadToLine.Length - (strReadToLine.Length / counts)];
                                    List<Token> t = new List<Token>();
                                    t = GetCaptures(ref strProductValue
                                                          , oAnalysis.Entities[strProduct].RegexString
                                                          , oAnalysis.Entities[strProduct].RegexValueGroupName
                                                          , oAnalysis.Entities[strProduct].RegexOption
                                                           );
                                    strProductValue = strProductValue.Substring(t[0].Index, t[0].Length);
                                }
                                else if ((int)data[2] > 0)
                                    strProductValue = strValue.Substring((int)data[1], (int)data[2]);
                                else
                                    strProductValue = oAnalysis.Entities[strProduct].DefaultValue;

                                break;
                            case "wafer_id":

                                if ((int)data[2] < 0)
                                {
                                    if (int.TryParse(oAnalysis.Entities[data[0].ToString()].RowNum, out nTemp) == false)
                                        throw new Exception(string.Format("oAnalysis.Entities[data[0].ToString()].RowNum 값을 숫자로 변경할 수 없습니다. [WaferID] oAnalysis.Entities[data[0].ToString()].RowNum = '{0}'", oAnalysis.Entities[data[0].ToString()].RowNum));

                                    strWaferValue = strReadToLine[int.Parse(oAnalysis.Entities[data[0].ToString()].RowNum) - 1
                                        + strReadToLine.Length - (strReadToLine.Length / counts)];
                                    List<Token> t = new List<Token>();
                                    t = GetCaptures(ref strWaferValue
                                                          , oAnalysis.Entities[strWafer].RegexString
                                                          , oAnalysis.Entities[strWafer].RegexValueGroupName
                                                          , oAnalysis.Entities[strWafer].RegexOption
                                                           );
                                    strWaferValue = strWaferValue.Substring(t[0].Index, t[0].Length);
                                }
                                else if ((int)data[2] > 0)
                                    strWaferValue = strValue.Substring((int)data[1], (int)data[2]);
                                else
                                {
                                    //count--;
                                }
                                //strWaferValue = oAnalysis.Entities[strWafer].DefaultValue;
                                break;
                            case "run_id":

                                if ((int)data[2] < 0)
                                {
                                    if (int.TryParse(oAnalysis.Entities[data[0].ToString()].RowNum, out nTemp) == false)
                                        throw new Exception(string.Format("oAnalysis.Entities[data[0].ToString()].RowNum 값을 숫자로 변경할 수 없습니다. [RunID] oAnalysis.Entities[data[0].ToString()].RowNum = '{0}'", oAnalysis.Entities[data[0].ToString()].RowNum));

                                    strRunValue = strReadToLine[int.Parse(oAnalysis.Entities[data[0].ToString()].RowNum) - 1
                                       + strReadToLine.Length - (strReadToLine.Length / counts)];
                                    List<Token> t = new List<Token>();
                                    t = GetCaptures(ref strRunValue
                                                          , oAnalysis.Entities[strRun].RegexString
                                                          , oAnalysis.Entities[strRun].RegexValueGroupName
                                                          , oAnalysis.Entities[strRun].RegexOption
                                                           );
                                    strRunValue = strRunValue.Substring(t[0].Index, t[0].Length);
                                }
                                else if ((int)data[2] > 0)
                                    strRunValue = strValue.Substring((int)data[1], (int)data[2]);
                                else
                                    strRunValue = oAnalysis.Entities[strRun].DefaultValue;
                                break;

                            case "flatzone":
                                if (!string.IsNullOrEmpty(oAnalysis.Entities[strFlat].DefaultValue))
                                {
                                    strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft)
                                            || string.IsNullOrEmpty(strTop)) ? strValue.Substring((int)data[1], (int)data[2])
                                            : oAnalysis.Entities[strFlat].DefaultValue.Replace(strBottom, "BOTTOM")
                                            .Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                                }
                                else if ((int)data[2] < 0)
                                {
                                    if (int.TryParse(oAnalysis.Entities[data[0].ToString()].RowNum, out nTemp) == false)
                                        throw new Exception(string.Format("oAnalysis.Entities[data[0].ToString()].RowNum 값을 숫자로 변경할 수 없습니다. [FlatZone] oAnalysis.Entities[data[0].ToString()].RowNum = '{0}'", oAnalysis.Entities[data[0].ToString()].RowNum));

                                    strFlatValue = strReadToLine[int.Parse(oAnalysis.Entities[data[0].ToString()].RowNum) - 1
                                      + strReadToLine.Length - (strReadToLine.Length / counts)];
                                    List<Token> t = new List<Token>();
                                    t = GetCaptures(ref strFlatValue
                                                          , oAnalysis.Entities[strFlat].RegexString
                                                          , oAnalysis.Entities[strFlat].RegexValueGroupName
                                                          , oAnalysis.Entities[strFlat].RegexOption
                                                           );
                                    strFlatValue = strFlatValue.Substring(t[0].Index, t[0].Length).Replace(strBottom, "BOTTOM").Replace(strRight, "RIGHT")
                                        .Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                                }
                                else if ((int)data[2] > 0)
                                    //strFlatValue = strValue.Substring((int)data[1], (int)data[2]);
                                    strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft)
                                      || string.IsNullOrEmpty(strTop)) ? strValue.Substring((int)data[1], (int)data[2])
                                      : strValue.Substring((int)data[1], (int)data[2]).Replace(strBottom, "BOTTOM")
                                    .Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                                else
                                    strFlatValue = (string.IsNullOrEmpty(strBottom) || string.IsNullOrEmpty(strRight) || string.IsNullOrEmpty(strLeft)
                                        || string.IsNullOrEmpty(strTop)) ? strValue.Substring((int)data[1], (int)data[2])
                                        : oAnalysis.Entities[strFlat].DefaultValue.Replace(strBottom, "BOTTOM")
                                        .Replace(strRight, "RIGHT").Replace(strTop, "TOP").Replace(strLeft, "LEFT");
                                switch (strFlatValue.ToUpper())
                                {
                                    case "BOTTOM":
                                        strFF = "0";
                                        break;
                                    case "LEFT":
                                        strFF = "90";
                                        break;
                                    case "RIGHT":
                                        strFF = "270";
                                        break;
                                    case "TOP":
                                        strFF = "180";
                                        break;
                                }
                                break;

                            case "total":

                                strTotalValue = "";
                                break;

                            case "wafer_num":
                                if ((int)data[2] > 0)
                                {
                                    if (int.TryParse(oAnalysis.Entities[data[0].ToString()].RowNum, out nTemp) == false)
                                        throw new Exception(string.Format("oAnalysis.Entities[data[0].ToString()].RowNum 값을 숫자로 변경할 수 없습니다. [WaferNum] oAnalysis.Entities[data[0].ToString()].RowNum = '{0}'", oAnalysis.Entities[data[0].ToString()].RowNum));

                                    strWaferNOValue = strReadToLine[int.Parse(oAnalysis.Entities[data[0].ToString()].RowNum) - 1
                                     + strReadToLine.Length - (strReadToLine.Length / counts)];
                                    List<Token> t = new List<Token>();
                                    t = GetCaptures(ref strWaferNOValue
                                                          , oAnalysis.Entities["Wafer_Num"].RegexString
                                                          , oAnalysis.Entities["Wafer_Num"].RegexValueGroupName
                                                          , oAnalysis.Entities["Wafer_Num"].RegexOption
                                                           );
                                    strWaferNOValue = strWaferNOValue.Substring(t[0].Index, t[0].Length);
                                }
                                else if ((int)data[2] > 0)
                                    strWaferNOValue = strValue.Substring((int)data[1], (int)data[2]);
                                else
                                    strWaferNOValue = oAnalysis.Entities["Wafer_Num"].DefaultValue;
                                break;
                        }

                        strYValue = (dtBin.Rows.Count / totalcount).ToString();
                        strXValue = dtBin.Rows[0][2].ToString();

                        if (string.IsNullOrEmpty(strWaferValue))
                            strWaferValue = strRunValue + "-" + strWaferNOValue;
                        if (string.IsNullOrEmpty(strProductValue) || string.IsNullOrEmpty(strFlatValue)
                             || string.IsNullOrEmpty(strRunValue) || string.IsNullOrEmpty(strWaferValue))
                        {
                            WriteLog("Error", "변수가 모두 매치되지 않았습니다", argument.LOGPath, 0);
                            return;
                        }
                        //count = 0;
                        int goodcount = 0;
                        int DieID = 1;
                        for (int binrowcount = binvaluecount; binrowcount < (dtBin.Rows.Count / totalcount) + binvaluecount; binrowcount += oAnalysis.CharCount)
                        {
                            if (binvaluecount >= dtBin.Rows.Count)
                                break;
                            int start = (int)dtBin.Rows[binrowcount][1];

                            //binvaluecount = 0;
                            for (int a = 0; a < (int)dtBin.Rows[binrowcount][2]; a++)
                            {

                                //string hbin = Utility.ConvertBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                string hbin = Utility.CusBinToHanaBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                sbBin.Append(hbin);
                                if (!hbin.Equals("."))
                                {
                                    drBinSort = dtSort.NewRow();
                                    drBinSort["DieID"] = DieID;
                                    drBinSort["X"] = a + 1;
                                    drBinSort["Y"] = (binvaluecount == 0 ? binrowcount + 1 : (binrowcount % binvaluecount) + 1);
                                    drBinSort["HBin"] = hbin; // Utility.ConvertBin(strValue.Substring(start, oAnalysis.CharCount), argument.HanaBin, argument.CusBin);
                                    if (hbin == "1")
                                        goodcount++;
                                    //drBinSort["Bin"] = strValue.Substring(start, 1);

                                    //drBinSort["Bin"] = Utility.ConvertHBin(hbin);
                                    //drBinSort["Bin"] = Utility.HanaBinToInt(hbin);

                                    drBinSort["CharBin"] = strValue.Substring(start, oAnalysis.CharCount);
                                    drBinSort["CNVBin"] = strValue.Substring(start, oAnalysis.CharCount);
                                    dtSort.Rows.Add(drBinSort);
                                    DieID++;

                                }
                                start++;
                                //binvaluecount++;
                            }
                            sbBin.Append(Environment.NewLine);

                        }
                        if (oldProduct.Equals(strProductValue) && oldRun.Equals(strRunValue))
                        {
                        }
                        else
                        {
                            oldProduct = strProductValue;
                            oldRun = strRunValue;
                            //checkRun = true;
                        }
                        WriteLog(strProductValue + "   /    " + strWaferValue.Replace("\n", "") + " : " + argument.ResultPath, "Convert START", argument.LOGPath, 2);
                        //하나맵으로 변환 -> 이동
                        dtStartTime = DateTime.Now;
                        string strRef = (Utility.ReferencePoint(strFlatValue, sbBin.ToString()))[0];
                        Utility.ConvertMap(strProductValue.Replace("\n", ""), strWaferValue.Replace("\n", ""), strXValue.Replace("\n", "")
                            , strYValue.Replace("\n", ""), sbBin.ToString(), strFlatValue.Replace("\n", "")
                            , strTotalValue.Replace("\n", ""), argument.CNVPath.Replace("\n", ""), argument.CusBin, argument.HanaBin);
                        sbBin = new StringBuilder();
                        WriteLog(strProductValue + "   /    " + strWaferValue.Replace("\n", "") + " : " + argument.ResultPath, "DB Insert START", argument.LOGPath, 2);
                        dtEndTime = DateTime.Now;

                        //checkRun = false;
                        dtSort.Clear();
                        strWaferValue = null;
                        binvaluecount += (dtBin.Rows.Count / totalcount);


                        //dtSort.ImportRow(data);
                    }

                }
                MoveFile("BACKUP", argument.ResultPath, argument);
                WriteLog("COMPLETE", argument.ResultPath, argument.LOGPath, 0);

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MoveFile(string sMoveType, string sFile, Utility.ARGUMENT_TAG argument)
        {
            if (!File.Exists(sFile))
                return;

            try
            {
                string sMovePath = string.Empty;

                switch (sMoveType)
                {
                    case "BACKUP":
                        Directory.CreateDirectory(argument.BackupPath);
                        sMovePath = Path.Combine(argument.BackupPath, sFile.Substring(sFile.LastIndexOf("\\") + 1));
                        if (File.Exists(sMovePath))
                        {
                            string sFileName = sFile.Substring(sFile.LastIndexOf("\\") + 1);
                            if (sFileName.IndexOf('.') == -1)
                                File.Move(sMovePath, Path.Combine(argument.BackupPath, sFileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss")));
                            else
                            {
                                sFileName = sFileName.Replace(".", "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".");
                                File.Move(sMovePath, Path.Combine(argument.BackupPath, sFileName));
                            }
                        }
                        break;

                    case "ERROR":
                        Directory.CreateDirectory(argument.ErrorPath);
                        sMovePath = Path.Combine(argument.ErrorPath, sFile.Substring(sFile.LastIndexOf("\\") + 1));
                        break;

                    case "OTHER_FORMAT":
                        sMovePath = Path.Combine(argument.ErrorPath, "Format");
                        Directory.CreateDirectory(sMovePath);
                        sMovePath = Path.Combine(sMovePath, sFile.Substring(sFile.LastIndexOf("\\") + 1));
                        break;
                }

                File.Copy(sFile, sMovePath, true);
                File.Delete(sFile);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "MoveFile", ex.Message), argument.LOGPath, 0);
                WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), argument.LOGPath, 1);
            }
        }

        #endregion

        public string ConvertData(string[] args)
        {
            try
            {
                sbData = new StringBuilder();
                sbLog = new StringBuilder();
                IsManualConvert = true;

                return sbData.ToString();
            }
            catch (Exception)
            {
                return sbLog.ToString();
            }
        }

        private void WriteLog(string strLogType, string strLog, string logPath, int loglevel)
        {
            try
            {
                if (!IsManualConvert)
                {
                    log.WriteLog(strLogType, strLog, logPath, loglevel);
                    Console.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strLog));
                }
                else
                    sbLog.AppendLine(string.Format("{0}\t{1}\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strLogType, strLog));
            }
            catch (Exception) { }
        }

        public string ValidationMap(string strFileName, ref Analysis oAnalysis)
        {
            try
            {
                int iRowCNT = 0;
                int iOneWaferRowCNT = 0;
                List<KeyValue> strValidationKey = new List<KeyValue>();
                KeyValue key = new KeyValue();
                StreamReader sr = new StreamReader(strFileName);
                string strTotalValue = sr.ReadToEnd();

                string[] strData = strTotalValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Dispose();

                string sWafer = string.Empty;

                foreach (string tempKey in oAnalysis.values.Keys)
                {
                    if (tempKey.Equals(EntityString.Wafer_ID.ToString()))
                    {
                        sWafer = "Wafer_ID";
                        break;
                    }
                    else if (tempKey.Equals(EntityString.Wafer_Num.ToString()))
                        sWafer = "Wafer_Num";
                }

                //if ((strData.Length % oAnalysis.values["Wafer_ID"].Count) > 1)
                if ((strData.Length % oAnalysis.values[sWafer].Count) > 1)
                    return string.Format("ERROR : (Line Count({0}) % {1} Count({2})) > 1", strData.Length, sWafer, oAnalysis.values[sWafer].Count);
                //if ((oAnalysis.values[EntityString.bin.ToString()].Count % y > 0) && (oAnalysis.Entities[EntityString.Y.ToString()].RegexString != string.Empty))
                //    return string.Format("ERROR : (Bin Count({0}) % Y({1})) > 0", oAnalysis.values[EntityString.bin.ToString()].Count, y);
                //iOneWaferRowCNT = strData.Length / oAnalysis.values["Wafer_ID"].Count;
                iOneWaferRowCNT = strData.Length / oAnalysis.values[sWafer].Count;

                for (int row = 0; row < oAnalysis.Entities.Count; row++)
                {
                    if (string.IsNullOrEmpty(oAnalysis.Entities[row].DefaultValue))
                    {
                        iRowCNT++;
                        if (!string.IsNullOrEmpty(oAnalysis.Entities[row].strKey) && !string.IsNullOrEmpty(oAnalysis.Entities[row].RowNum))
                        {
                            key.strKey = oAnalysis.Entities[row].strKey;
                            key.iRowNum = int.Parse(oAnalysis.Entities[row].RowNum);
                            strValidationKey.Add(key);
                        }
                    }
                }
                for (int keylen = 0; keylen < strValidationKey.Count; keylen++)
                {
                    for (int datalen = 0; datalen < oAnalysis.values[sWafer].Count; datalen++)
                    {
                        if (!strData[(strValidationKey[keylen].iRowNum) - 1 + (iOneWaferRowCNT * datalen)].StartsWith(strValidationKey[keylen].strKey))
                        {
                            return string.Format("ERROR : Key Mismatch ({0} = {1})", strData[(strValidationKey[keylen].iRowNum) - 1 + (iOneWaferRowCNT * datalen)], strValidationKey[keylen].strKey);
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void MapCreate(string product, int flatZone, int xmax, int ymax, int xmin, int ymin, int netDie)
        {
            DataTable dt = null;
            int iDiesX = 0;
            int iDiesY = 0;

            try
            {
                m_wMap = new DACrux.Map.EditWaferMap();

                m_wMap.WaferMargin = 97.5 / 100;
                m_wMap.FirstDieX = xmin;
                m_wMap.FirstDieY = ymin;
                m_wMap.DieMinX = xmin;
                m_wMap.DieMaxX = xmax;
                m_wMap.DieMinY = ymin;
                m_wMap.DieMaxY = ymax;
                m_wMap.NotchAngle = flatZone;
                m_wMap.NotchType = DACrux.Base.Notch.Flat;

                m_wMap.DieCalculation(false);
                m_wMap.Redraw();


                iDiesX = (int)Math.Ceiling((m_wMap.WaferSize / m_wMap.DieSizeX));
                iDiesY = (int)Math.Ceiling((m_wMap.WaferSize / m_wMap.DieSizeY));

                m_wMap.DieMinX = xmin;
                m_wMap.DieMaxX = iDiesX; 

                m_wMap.DieMinY = ymin;
                m_wMap.DieMaxY =iDiesY;


                if ((m_wMap.DieMaxX % 2) == 0)
                {
                    m_wMap.OriginX = 0;
                }
                else
                {
                    m_wMap.OriginX = m_wMap.DieSizeX - m_wMap.DieSizeX / 2.0d;
                }
                m_wMap.OriginIndexX = m_wMap.DieMinX + (int)Math.Floor((double)m_wMap.XDies / 2.0d);


                if ((m_wMap.DieMaxY % 2) == 0)
                {
                    m_wMap.OriginY = 0;
                }
                else
                {
                    m_wMap.OriginY = -m_wMap.DieSizeY / 2.0d;
                }
                m_wMap.OriginIndexY = m_wMap.DieMinY + (int)Math.Floor((double)m_wMap.YDies / 2.0d);

                m_wMap.DieCalculation(true);
                m_wMap.Redraw();

                oMapDef.SetMapDef(product
                    , m_wMap.WaferSize
                    , m_wMap.DieSizeX
                    , m_wMap.DieSizeY
                    , m_wMap.OriginX
                    , m_wMap.OriginY
                    , m_wMap.OriginIndexX
                    , m_wMap.OriginIndexY
                    , m_wMap.FirstDieX * m_wMap.DieSizeX
                    , m_wMap.FirstDieY * m_wMap.DieSizeY
                    , m_wMap.FirstDieX
                    , m_wMap.FirstDieY
                    , (double)1
                    , m_wMap.NotchAngle
                    , netDie
                    , (m_wMap.NotchType == DACrux.Base.Notch.Flat) ? 0 : 1
                    , 0
                    , 0
                    , 0
                    , 0
                    , 0
                    , 0
                    , m_wMap.DieMinX
                    , m_wMap.DieMaxX
                    , m_wMap.DieMinY
                    , m_wMap.DieMaxY
                    , (int)m_wMap.XYDirect
                    , m_wMap.ReferenceDieSetting);

                /// Map 부분
                /// 
                dt = new DataTable();
                //ds.Tables.Add();
                dt.Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("USECODE", System.Type.GetType("System.Int32")));

                for (int i = 0; i < m_wMap.Dies.Count; i++)
                {
                    dt.Rows.Add(new object[] { m_wMap.Dies[i].IndexX, m_wMap.Dies[i].IndexY, m_wMap.Dies[i].DieProp });
                }
                oUseMap.CreateUseMap(product, dt);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}