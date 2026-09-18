using System;
using System.IO;
using System.Collections;
using System.Text;
using DACrux.TEST.DSL;
using DACrux.TEST.RO;
using System.Data;

namespace FDll
{
    public class Sinf3DOutput
    {
        bool m_BOutPutMap = false;

        public void Make_DownLoad_Map
            (string LotID, string[] WaferID, string TestArea
            , string DownLoadMapPath, bool bOutpurMap)
        {
            #region ' 변수 ' -----------------------------
            FileStream fs = null;
            StreamReader sr = null;

            T_TPS_WAFER_SUM oWaferSum = null;
            T_TPS_LOT_SUM oLotSum = null;
            T_TPS_WAFER oWafer = null;
            T_TPS_LOT oLot = null;
            T_PRB oDSLPrb = null;
            T_TPS_MAPDEF oDSLMAPDEF = null;
            DataTable dtWaferSum = null;

            MAPANALYSIS_DATA oRoMap = null;
            CommonMethods commonMethod = null;

            // T_TPS_LOT (공통 내역)
            string strLotID = string.Empty;
            string strAngle = string.Empty;
            string strProduct = null;
            string XRef = string.Empty;
            string YRef = string.Empty;
            string XDieSize;
            string YDieSize;

            string strFormatData = null;

            string strLotSeq = null;
            string strCustomer = null;
            string strWaferSeq = null;
            string strTableName = null;

            // T_TPS_WAFER (WAFER 별 내역)
            string strWaferNo = string.Empty;


            string strBinData = string.Empty;
            int iMinX = 1000;
            int iMaxX = 0;
            int iMinY = 1000;
            int iMaxY = 0;

            string[] arrGecBin = null;

            DataTable dtPrb = null;
            DataTable dtAllTestAreaBindesc;

            string FormatMapPath = string.Empty;

            #endregion --------------------------------------------
            try
            {
                oWaferSum = new T_TPS_WAFER_SUM();
                oWafer = new T_TPS_WAFER();
                oLot = new T_TPS_LOT();
                oLotSum = new T_TPS_LOT_SUM();
                oRoMap = new MAPANALYSIS_DATA();
                oDSLPrb = new T_PRB();
                oDSLMAPDEF = new T_TPS_MAPDEF();
                commonMethod = new CommonMethods();

                m_BOutPutMap = bOutpurMap;

                //strFormat = "SINF3D";

                // Format File Read
                FormatMapPath = System.Configuration.ConfigurationManager.AppSettings["SINF3D_DOWNLOAD_MAP_PATH"].ToString();

                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                fs = new FileStream(FormatMapPath, System.IO.FileMode.Open, FileAccess.Read);	// File Open
                sr = new StreamReader(fs, encode);
                strFormatData = sr.ReadToEnd();
                sr.Close();

                // Lot Sum Info
                DataTable dtLot = oLotSum.SelectLotSumByTestarea(LotID, TestArea);
                strCustomer = dtLot.Rows[0]["CUSTOMER"].ToString();
                strLotSeq = dtLot.Rows[0]["LOT_SEQ"].ToString();
                strLotID = LotID;
                strProduct = dtLot.Rows[0]["PRODUCT"].ToString();

                // Map Info
                DataSet dsMAPDEF = oDSLMAPDEF.GetMapDef(strProduct);
                strAngle = dsMAPDEF.Tables[0].Rows[0]["ANGLE"].ToString();
                XRef = dsMAPDEF.Tables[0].Rows[0]["FIRST_INDEX_X"].ToString();
                YRef = dsMAPDEF.Tables[0].Rows[0]["FIRST_INDEX_Y"].ToString();
                XDieSize = dsMAPDEF.Tables[0].Rows[0]["CHIP_SIZE_X"].ToString();
                YDieSize = dsMAPDEF.Tables[0].Rows[0]["CHIP_SIZE_Y"].ToString();

                // Wafer Sum Infos
                dtWaferSum = oWaferSum.SelectPickWafers(strLotSeq, ConvertWaferList(WaferID));

                // Get Bindesc Testarea (All)
                dtAllTestAreaBindesc = commonMethod.GetMergeBinDesc(LotID, WaferID[0], strProduct, TestArea, false);

                string[,] arrBinDesc = new string[dtAllTestAreaBindesc.Rows.Count + 1, 3];
                for (int i = 0; i < dtAllTestAreaBindesc.Rows.Count; i++)
                {
                    arrBinDesc[i, 0] = dtAllTestAreaBindesc.Rows[i]["BIN"].ToString();
                    arrBinDesc[i, 1] = dtAllTestAreaBindesc.Rows[i]["IN_CHAR_BIN"].ToString();
                    arrBinDesc[i, 2] = dtAllTestAreaBindesc.Rows[i]["OUT_CHAR_BIN"].ToString();
                }

                // Map Create as much as Wafer Count
                foreach (DataRow dr in dtWaferSum.Rows)
                {
                    string strData = strFormatData;

                    // Header 정보 (From T_TPS_WAFER) --------------
                    strWaferNo = dr["WAFER_ID"].ToString();
                    strWaferSeq = dr["WAFER_SEQ"].ToString();

                    // Get Row Data 들고오기
                    {
                        if (TestArea == "OQC_MERGE")
                            strTableName = "T_INCUST";
                        else if (TestArea == "CP_MERGE" || TestArea == "FINAL_MERGE")
                            strTableName = "T_AOI";

                        dtPrb = oDSLPrb.SelectInfo(strTableName, strWaferSeq);
                    }

                    // X, Y Min, Max
                    iMinX = Convert.ToInt32(dtPrb.Compute("Min(X)", null));
                    iMaxX = Convert.ToInt32(dtPrb.Compute("Max(X)", null));
                    iMinY = Convert.ToInt32(dtPrb.Compute("Min(Y)", null));
                    iMaxY = Convert.ToInt32(dtPrb.Compute("Max(Y)", null));

                    // Save to string Arr (DataTable)
                    string[,] arrBin = new string[iMaxX + 1, iMaxY + 1];
                    foreach (DataRow drPRB in dtPrb.Rows)
                    {
                        int x = Convert.ToInt32(drPRB["X"]);
                        int y = Convert.ToInt32(drPRB["Y"]);

                        arrBin[x, y] = drPRB["BIN"].ToString();
                    }

                    if (!bOutpurMap)
                    {
                        #region 'DownLoad Map' ======================================================

                        strBinData = string.Empty;
                        int CntNull = 0;
                        bool bStart = false;
                        int StartXPoint = 0;
                        double dCenterXPoint = ((iMaxX - iMinX + 1) / 2);
                        // arrBinData -> string Bin Data
                        for (int y = iMinY; y <= iMaxY; y++)
                        {
                            strBinData += "RowData:";

                            bStart = false;
                            StartXPoint = 0;
                            CntNull = 0;

                            for (int x = iMinX; x <= iMaxX; x++)
                            {

                                // Null Bin
                                if (string.IsNullOrEmpty(arrBin[x, y]))
                                {
                                    // 마지막 X 좌표를 만나면 지금까지 쌓아둔 Null 을 채워 넣음
                                    if (x == iMaxX)
                                    {
                                        for (int cnt = 0; cnt <= CntNull; cnt++)
                                            strBinData += ("___ ");
                                    }

                                    // Null Count 누적
                                    CntNull++;
                                }

                                // Data Bin
                                else
                                {
                                    // 전에 Bin 이 있다면 (가운데 Bin 이라면) 쌓아둔 255 를 채움
                                    if (bStart)
                                    {
                                        if (x > dCenterXPoint && StartXPoint < dCenterXPoint)
                                        {
                                            for (int cnt = 0; cnt < CntNull; cnt++)
                                                strBinData += ("___ ");
                                        }
                                        else
                                        {
                                            for (int cnt = 0; cnt < CntNull; cnt++)
                                                strBinData += ("255 ");
                                        }
                                    }
                                    // 전에 Bin
                                    else
                                    {
                                        for (int cnt = 0; cnt < CntNull; cnt++)
                                            strBinData += ("___ ");
                                    }

                                    strBinData += (GetOutputBin(arrBin[x, y], arrBinDesc, arrGecBin).PadLeft(3, '0') + " ");
                                    CntNull = 0;
                                    bStart = true;
                                    StartXPoint = x;
                                }
                            }
                            strBinData += "\r\n";
                        }

                        #endregion ==================================================================
                    }

                    else
                    {
                        #region ' Output Map ' ======================================================

                        strBinData = string.Empty;
                        int Cnt255 = 0;
                        bool start = false;
                        // arrBinData -> string Bin Data
                        for (int y = iMinY; y <= iMaxY; y++)
                        {
                            strBinData += "RowData:";
                            start = false;
                            Cnt255 = 0;

                            for (int x = iMinX; x <= iMaxX; x++)
                            {

                                // Null Bin
                                if (string.IsNullOrEmpty(arrBin[x, y]))
                                {
                                    strBinData += ("___ ");
                                }
                                // Data Bin
                                else
                                {
                                    if (start)
                                    {
                                        for (int cnt = 0; cnt < Cnt255; cnt++)
                                            strBinData += ("255 ");
                                    }
                                    else
                                    {
                                        for (int cnt = 0; cnt < Cnt255; cnt++)
                                            strBinData += ("___ ");
                                    }

                                    strBinData += (GetOutputBin(arrBin[x, y], arrBinDesc, arrGecBin).PadLeft(3, '0') + " ");
                                    Cnt255 = 0;
                                    start = true;
                                }
                            }
                            strBinData += "\r\n";
                        }

                        #endregion ==================================================================
                    }
                    
                    // Header Data Replace
                    strData = strData.Replace("%DEVICE%", strProduct);
                    strData = strData.Replace("%LOTID%", strLotID);
                    strData = strData.Replace("%WAFERID%", strWaferNo);
                    strData = strData.Replace("%ANGLE%", strAngle);
                    strData = strData.Replace("%X_DIES%", iMaxX.ToString());
                    strData = strData.Replace("%Y_DIES%", iMaxY.ToString());
                    strData = strData.Replace("%X_REFDIE%", XRef);
                    strData = strData.Replace("%Y_REFDIE%", YRef);
                    strData = strData.Replace("%X_DIE_SIZE%", XDieSize);
                    strData = strData.Replace("%Y_DIE_SIZE%", YDieSize);

                    // Bin Data Replace
                    strData = strData.Replace("%BIN_DATA%", strBinData);

                    // File Write
                    string writePath = DownLoadMapPath + @"\" + strLotID;
                    string writeFilePath = writePath + @"\" + strLotID +  "_" + strWaferNo.Split('.')[1] + ".txt";

                    // Folder Create
                    DirectoryInfo dir = new DirectoryInfo(writePath);
                    if (dir.Exists == false)
                        dir.Create();

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(writeFilePath))
                    {
                        sw.WriteLine(strData);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetOutputBin(string strBin, string[,] arrBinDesc, string[] arrGecBin)
        {
            string returnBin = string.Empty;

            try
            {
                if (m_BOutPutMap)
                {
                    if (strBin == "1")
                        return "1";

                    for (int i = 0; i < arrBinDesc.GetLength(0); i++)
                    {
                        // OutPut Bin return
                        if (strBin == arrBinDesc[i, 0])
                        {
                            // OutPut Bin Exist
                            if (!string.IsNullOrEmpty(arrBinDesc[i, 2]))
                                returnBin = arrBinDesc[i, 2];
                            // Output Bin Not Exist (return In Bin)
                            else
                                returnBin = strBin;

                            break;
                        }
                        else
                        {
                            returnBin = strBin;
                        }
                    }
                    return returnBin;
                }
                else
                {
                    if (strBin == "1")
                        return "000";
                    else
                        return "222";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // WaferList에 ' ' 를 붙여줌
        public string ConvertWaferList(string[] WaferID)
        {
            string strWaferID = string.Empty;

            try
            {
                for (int i = 0; i < WaferID.Length; i++)
                {
                    WaferID[i] = WaferID[i].PadLeft(2, '0');
                }

                if (WaferID[0].Contains("'"))
                    strWaferID = string.Join("','", WaferID);
                else
                    strWaferID = "'" + string.Join("','", WaferID) + "'";

                return strWaferID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
