using System;
using System.Data;
using FDll;
//using Miracom.Client;
using DACrux.TEST.DSL;
using System.Transactions;
using System.IO;
using System.Configuration;
//using DACrux.TEST.BSL;

namespace FDll
{
    public class DbupWmap
    {
        #region ' Process '--------------------------------------------------------------------------------
        string m_strCustomer;
        string m_strFacility;
        string m_strTestArea;
        string m_strMapID;
        string m_strNetDie;
        string m_strLossDie;
        string m_strFailDie;

        string m_strLot_Seq;
        string m_strWafer_Seq;
        string m_strPreWafer_Seq;

        string m_strWaferCount;
        string m_strWaferStartTime;
        string m_strWaferEndTime;
        string m_strLotStartTime;
        string m_strLotEndTime;
        string m_strWaferID;
        string m_strTableName;

        int m_iTestDie;
        int m_iPassDie;
        int m_iTotalDie;

        /// <summary>
        /// Header Data를 Insert
        /// </summary>
        /// <param name="hd"></param>

        public void InsertData(HeaderData hd, RowData[] rd, string TestArea, bool isMergeData)
        {
            
            string swCsvPath = @"C:\TEMP\";
            string TempLogPath = @"C:\YMS\";
            string swCtlPath = swCsvPath;

            #region ' 변수 '
            T_TPS_CUSTOMER oDSLCustomer = null;
            T_TPS_PRODUCT oDSLProduct = null;
            T_TPS_PROGRAM oDSLProgram = null;
            T_TPS_LOT oDSLLot = null;
            T_TPS_WAFER oDSLWafer = null;
            T_TPS_BINDESC oDSLBinDesc = null;
            T_PRB oDSLPrb = null;
            T_AVI_TABLE oDSLAviTable = null;
            T_TPS_WAFER_SUM oDSLWSum = null;
            T_TPS_MERGERULE oMergeRule = null;
            CommonMethods commonMethod = null;

            DataTable dtProduct = null;
            DataSet dtProgram = null;
            DataTable dtLot = null;
            DataTable dtWafer = null;
            DataTable dtBinDesc = null;
            DataTable dtAviTable = null;
            DataTable dtDummy = null;

            string strTempTableName = string.Empty;
            string mapValue = string.Empty;
            string strColumn = string.Empty;
            string strProduct = string.Empty;

            string MergeTestArea = string.Empty;

            #endregion ------------------------------------

            try
            {
                oDSLCustomer = new T_TPS_CUSTOMER();
                oDSLProduct = new T_TPS_PRODUCT();
                oDSLProgram = new T_TPS_PROGRAM();
                oDSLLot = new T_TPS_LOT();
                oDSLWafer = new T_TPS_WAFER();
                oDSLBinDesc = new T_TPS_BINDESC();
                oDSLPrb = new T_PRB();
                oDSLAviTable = new T_AVI_TABLE();
                oDSLWSum = new T_TPS_WAFER_SUM();
                oMergeRule = new T_TPS_MERGERULE();
                                                
                // Get Product Information
                //dtProduct = oDSLProduct.SelectProductInfo(hd.LotNo);
                dtProduct = oDSLProduct.SelectProductInfo(hd.Device);
                if (dtProduct == null || dtProduct.Rows.Count <= 0)
                {
                    throw new Exception("Don't Exist Product[" + hd.Device + "]");
                }
                string pro = dtProduct.Rows[0]["PRODUCT"].ToString();

                // Get Program Information
                dtProgram = oDSLProgram.GetProgramInfo(pro);
                if (dtProgram == null || dtProgram.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception("Don't Exist Program[" + hd.TestProgram + "]");
                }

                m_strCustomer = dtProduct.Rows[0]["CUSTOMER_ID"].ToString();
                m_strFacility = dtProduct.Rows[0]["FACILITY"].ToString();
                m_strMapID = dtProduct.Rows[0]["MAPID"].ToString();
                m_strTestArea = TestArea;
               
                #region Wafer 헤더 정보 Insert ----------------------------------------

                // Get Lot Information
                //dtLot = oDSLLot.GetIslot(hd.Device, m_strTestArea, hd.TestProgram, hd.LotNo);
                dtLot = oDSLLot.GetIslot(dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), hd.LotNo);

                if (dtLot == null || dtLot.Rows.Count <= 0)	// 최초Lot Data
                {
                    // :product, :testarea, :program, :lot_id, :mother_lot_id, :lot_type, :fablot, 
                    // :family, :facility, :device_alias, :start_time, :end_time,       
                    // :wip_status, :wafers, :spesail_cmt, :eng_cmt
                    oDSLLot.CreateLot(dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), hd.LotNo, hd.MotherLotNo, "", "",
                                        "", m_strFacility, "", hd.StartTime.ToString(), hd.EndTime.ToString(),
                                        "1000", "1", "", "", hd.MapProduct);

                    dtLot = oDSLLot.GetIslot(dtProduct.Rows[0]["PRODUCT"].ToString(), m_strTestArea, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), hd.LotNo);

                    m_strLot_Seq = dtLot.Rows[0][0].ToString();
                }
                else
                {
                    m_strLot_Seq = dtLot.Rows[0][0].ToString();
                }

                // Wafer_Seq
                dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, hd.WaferID);

                m_strPreWafer_Seq = string.Empty;

                if (dtWafer != null && dtWafer.Rows.Count > 0) // wafer Retest
                {
                    hd.TestDies = System.Convert.ToInt32(dtWafer.Rows[0]["TESTED_DIE"].ToString());

                    oDSLWafer.UpdateProbeCnt(m_strLot_Seq, hd.WaferID);

                    oDSLWafer.CreateWafer(m_strLot_Seq, hd.WaferID, hd.TesterID, hd.ProberCard, hd.Operator, "0",
                                        hd.TestDies.ToString(), hd.StartTime, hd.EndTime, "0", hd.FailDies.ToString(), "0",
                                        "0", "0", "", "", "", "", hd.MapProduct);

                    m_strPreWafer_Seq = dtWafer.Rows[0]["WAFER_SEQ"].ToString();

                    dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, hd.WaferID);

                    m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                }
                else	// Wafer 최초 Test
                {
                    // :lot_seq, :wafer_id, :tester, :probe_card, :operator, :probe_cnt,
                    // :tested_die, :start_time, :end_time, :wafer_cat, :loss_die, :isp_initem, 
                    // :visual_item, :isp_outitem, :isp_incmt ,:visual_cmt, :isp_outcmt, :fvi_flag
                    oDSLWafer.CreateWafer(m_strLot_Seq, hd.WaferID, hd.TesterID, hd.ProberCard, hd.Operator, "0",
                                        hd.TestDies.ToString(), hd.StartTime, hd.EndTime, "0", hd.FailDies.ToString(), "0",
                                        "0", "0", "", "", "", "", hd.MapProduct);

                    dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, hd.WaferID);

                    m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                }

                // 해당 Lot Seq의 Wafer수, StartTime, EndTime
                m_strWaferStartTime = hd.StartTime;
                m_strWaferEndTime = hd.EndTime;
                m_strWaferCount = oDSLWafer.GetWaferCount(m_strLot_Seq).Rows[0][0].ToString();
                m_strLotStartTime = dtWafer.Rows[0]["START_TIME"].ToString();
                m_strLotEndTime = dtWafer.Rows[0]["END_TIME"].ToString();

                // Lot에 Wafer수 EndTime Update
                oDSLLot.UpdateCount(m_strLotStartTime, m_strLotEndTime, m_strWaferCount, m_strLot_Seq);

                #endregion ------------------------------------------------------------
                
                #region ' Wafer RowData Insert '

                // Path
                //string swCsvPath = System.Configuration.ConfigurationManager.AppSettings["CSVPath"].ToString() + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".csv";
                //string swCtlPath = System.Configuration.ConfigurationManager.AppSettings["CSVPath"].ToString() + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".ctl";
                //string TempLogPath = System.Configuration.ConfigurationManager.AppSettings["CSVPath"].ToString() + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".log";

                swCsvPath = swCsvPath + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".csv";
                swCtlPath = swCtlPath + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".ctl";
                TempLogPath = TempLogPath + hd.LotNo + hd.WaferID.PadLeft(2, '0') + ".log";


                // ' Data Insert much as Row Count '
                //  - Write on [ CSV File ] much as Row Count
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(swCsvPath))
                {
                    for (int i = 0; i < rd.Length; i++)
                    {

                        if (TestArea == "PROBETEST")
                        {
                            mapValue = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                                                , m_strWafer_Seq            // WaferSeq
                                                , Convert.ToString(i + 1)   // DieID
                                                , rd[i].DieX.ToString()     // X
                                                , rd[i].DieY.ToString()     // Y
                                                , rd[i].Bin.ToString()      // Bin
                                                , rd[i].Bin.ToString()      // HBin
                                                , rd[i].CharBin             // CharBin
                                                , "0"                       // SITE
                                                , "0"                       // VISUALINSP
                                                , "0"                       // AVI
                                                );
                        }
                        else if (TestArea == "INCUST" || TestArea == "AOI")
                        {
                            mapValue = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                                            , m_strWafer_Seq                // Wafer_Seq
                                            , Convert.ToString(i + 1)       // DieID
                                            , rd[i].DieX.ToString()         // X
                                            , rd[i].DieY.ToString()         // Y
                                            , rd[i].Bin.ToString()          // BIN
                                            , rd[i].CharBin.ToString()      // CHARBIN
                                            , "0"                           // VISUALINSP
                                            );
                        }
                        else if (TestArea == "PNP")
                        {
                            mapValue = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                                            , m_strWafer_Seq                // Wafer_Seq
                                            , Convert.ToString(i + 1)       // DieID
                                            , rd[i].DieX.ToString()         // X
                                            , rd[i].DieY.ToString()         // Y
                                            , rd[i].Bin.ToString()          // BIN
                                            , rd[i].CharBin.ToString()      // CHARBIN
                                            , hd.ReelID                     // REEL_ID
                                            , rd[i].Reel.ToString()         // REEL_NO
                                            , rd[i].Position.ToString()     // POSITION
                                            );
                        }
                        else if (isMergeData)
                        {
                            mapValue = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                                            , m_strWafer_Seq                // Wafer_Seq
                                            , Convert.ToString(i + 1)       // DieID
                                            , rd[i].DieX.ToString()         // X
                                            , rd[i].DieY.ToString()         // Y
                                            , rd[i].Bin.ToString()          // BIN
                                            , "NULL"                        // CHARBIN
                                            , "0"                           // VISUALINSP
                                            );
                        }

                        sw.WriteLine(mapValue);
                    }
                    sw.Close();
                }

                // SQL LOADER 로 넣기
                int iRandom = 0;
                Random Rd = new Random();
                DateTime dtNotDate = DateTime.Now;
                iRandom = Rd.Next(1, 9);  // Temp Table 이름에 난수 결합

                // Table Name, Coulumn Selection
                if (TestArea == "PROBETEST")
                {
                    // Table Name
                    m_strTableName = "T_PRB_" + dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString().ToUpper().Trim().Replace("-", "_");
                    strTempTableName
                        = "T_PRB_" + hd.LotNo.Replace(" ", "").Replace(".", "").Replace("-", "") + "_"
                        + hd.WaferID.Substring(hd.WaferID.Length - 2, 2) + dtNotDate.Millisecond.ToString();

                    // Column
                    strColumn = "(WAFER_SEQ,DIEID,X,Y,BIN,HBIN,CHARBIN,VISUALINSP,AVI)";
                }
                else
                {
                    // Table Name
                    if (isMergeData)    
                    {
                        // Search From T_TPS_MERGERULE_TYPE Table
                        commonMethod = new CommonMethods();
                        T_TPS_TESTAREA oDSLTestArea = new T_TPS_TESTAREA();

                        //commonMethod.GetMergeRule(ref Customer_merge, ref Product_merge, ref ActTetsArea_merge);
                        DataTable dtSaveTable = oDSLTestArea.SelectTestArea(TestArea);

                        m_strTableName = dtSaveTable.Rows[0]["SAVE_TABLE"].ToString();
                    }
                    else
                        m_strTableName = "T_" + TestArea;

                    //Temp Table Name
                    strTempTableName
                        = "T_" + TestArea + hd.LotNo.Replace(" ", "").Replace(".", "").Replace("-", "") + "_"
                        + hd.WaferID.Substring(hd.WaferID.Length - 2, 2) + dtNotDate.Millisecond.ToString();

                    // Column
                    if (TestArea == "PNP")
                        strColumn = "(WAFER_SEQ,DIEID,X,Y,BIN,CHARBIN,REEL_ID,REEL_NO,REEL_POS)";
                    else
                        strColumn = "(WAFER_SEQ,DIEID,X,Y,BIN,CHARBIN,VISUALINSP)";
                }

                // Create Temporary Table
                oDSLPrb.CreateTemporaryTable(strTempTableName, m_strTableName);

                // Temp Table Data Insert
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        //oDSLPrb.InsertMultiPrb(strTempTable, arrPrbList);
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(swCtlPath))
                        {
                            sw.WriteLine("LOAD DATA");
                            sw.WriteLine(string.Format("INFILE '{0}'", swCsvPath));
                            sw.WriteLine(string.Format("INTO TABLE {0}", strTempTableName));
                            sw.WriteLine("FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"'");
                            sw.WriteLine(strColumn);

                            sw.Close();
                        }

                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.FileName = "CMD.exe";
                        proc.StartInfo.Arguments = string.Format("/c SQLLDR userid=DMSMGR/dmsmgr@DMSDB control={0} log='{1}'", swCtlPath, TempLogPath);
                        proc.Start();
                        proc.WaitForExit();

                        oDSLPrb.CopyTemporaryTableToItem(m_strTableName, strTempTableName);

                        scope.Complete();
                    }
                }
                finally
                {
                    // Temp Table Drop
                    oDSLPrb.DropTemporaryTable(strTempTableName);

                    File.Delete(TempLogPath);
                    File.Delete(swCsvPath);
                    File.Delete(swCtlPath);
                }

                #endregion ---------------------------------------------------------------------

                // Get Total Die
                if (!isMergeData)
                    m_iTotalDie = Convert.ToInt32(oDSLPrb.SelectTestDie(m_strTableName, m_strWafer_Seq).Rows[0][0].ToString());
                else
                    m_iTotalDie = hd.TestDies;
                m_strFailDie = hd.FailDies.ToString();
                m_strLossDie = hd.FailDies.ToString();
                //m_strFailDie = oDSLPrb.SelectFailDie(strTableName, m_strWafer_Seq, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), dtProduct.Rows[0]["PRODUCT"].ToString()).Rows[0][0].ToString();
                //// Get Loss Die Data
                ////m_strLossDie = oDSLPrb.SelectLossDie(strTable, m_strWafer_Seq, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), dtProduct.Rows[0]["PRODUCT"].ToString()).Rows[0][0].ToString();
                //m_strLossDie = oDSLPrb.SelectFailDie(strTableName, m_strWafer_Seq, dtProgram.Tables[0].Rows[0]["PROGRAM"].ToString(), dtProduct.Rows[0]["PRODUCT"].ToString()).Rows[0][0].ToString();

                m_iPassDie = m_iTotalDie - Convert.ToInt32(m_strLossDie);

                m_iTestDie = m_iPassDie + Convert.ToInt32(m_strLossDie);

                // Wafer Loss Die Update                
                oDSLWafer.UpdateLossDie(m_strWafer_Seq, m_strLossDie);
                oDSLWafer.UpdateTestDie(m_strWafer_Seq, m_iTestDie.ToString());

                hd.TestDies = m_iTestDie;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtProduct != null) dtProduct.Dispose();
                if (dtProgram != null) dtProgram.Dispose();
                if (dtLot != null) dtLot.Dispose();
                if (dtWafer != null) dtWafer.Dispose();
                if (dtBinDesc != null) dtBinDesc.Dispose();
                if (dtAviTable != null) dtAviTable.Dispose();
                if (dtDummy != null) dtDummy.Dispose();

                dtProduct = null;
                dtProgram = null;
                dtLot = null;
                dtWafer = null;
                dtBinDesc = null;
                dtAviTable = null;
                dtDummy = null;
            }
        }

        public DataTable InsertDataForAOI(string PreWaferSeq, string strTable)
        {
            #region ' 변수 '

            T_PRB oPRB = null;
            T_TPS_LOT oDSLLot = null;
            T_TPS_WAFER oDSLWafer = null;
            T_TPS_WAFER_SUM oDSLWSum = null;

            CommonMethods commonMethod = null;

            DataTable dtLot = null;
            DataTable dtWafer = null;

            DataSet dtPreWaferSum = null;

            //string strCBin;
            string strTableName = string.Empty;
            string strTempTableName = string.Empty;
            string mapValue = string.Empty;
            string strColumn = string.Empty;

            string strLotId = string.Empty;
            string strProduct = string.Empty;
            string strWaferID = string.Empty;
            string PreTestArea = string.Empty;
            string strCustomer = string.Empty;
            string PreLotSeq = string.Empty;

            #endregion ------------------------------------

            try
            {
                oPRB = new T_PRB();
                oDSLWSum = new T_TPS_WAFER_SUM();
                oDSLLot = new T_TPS_LOT();
                oDSLWafer = new T_TPS_WAFER();

                #region Wafer 헤더 정보 Insert ----------------------------------------

                dtPreWaferSum = oDSLWSum.GetWaferInfo(PreWaferSeq);

                strProduct = dtPreWaferSum.Tables[0].Rows[0]["PRODUCT"].ToString();
                PreTestArea = dtPreWaferSum.Tables[0].Rows[0]["TESTAREA"].ToString();
                PreLotSeq = dtPreWaferSum.Tables[0].Rows[0]["LOT_SEQ"].ToString();
                strWaferID = m_strWaferID = dtPreWaferSum.Tables[0].Rows[0]["WAFER_ID"].ToString();
                strCustomer = dtPreWaferSum.Tables[0].Rows[0]["CUSTOMER"].ToString();
                strLotId = dtPreWaferSum.Tables[0].Rows[0]["LOT_ID"].ToString();
                //m_hd.MapProduct = dtPreWaferSum.Tables[0].Rows[0]["LOT_ID"].ToString();
                m_strWaferStartTime = m_strWaferEndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                dtLot = oDSLLot.GetIslot(strProduct, "AOI", strProduct, strLotId);

                if (dtLot == null || dtLot.Rows.Count <= 0)	// 최초 Lot Data
                {
                    // :product, :testarea, :program, :lot_id, :mother_lot_id, :lot_type, :fablot, 
                    // :family, :facility, :device_alias, :start_time, :end_time,       
                    // :wip_status, :wafers, :spesail_cmt, :eng_cmt
                    oDSLLot.CopyLot("AOI", DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"), PreLotSeq);

                    dtLot = oDSLLot.GetIslot(strProduct, "AOI", strProduct, strLotId);

                    m_strLot_Seq = dtLot.Rows[0][0].ToString();
                }
                else
                {
                    m_strLot_Seq = dtLot.Rows[0][0].ToString();
                }

                // Wafer_Seq
                dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, strWaferID);

                m_strPreWafer_Seq = PreWaferSeq;

                if (dtWafer != null && dtWafer.Rows.Count > 0) // ReMerge
                {
                    //hd.TestDies = System.Convert.ToInt32(dtWafer.Rows[0]["TESTED_DIE"].ToString());

                    oDSLWafer.UpdateProbeCnt(m_strLot_Seq, strWaferID);

                    oDSLWafer.CopyWafer(m_strLot_Seq, DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"), "0", PreWaferSeq);

                    dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, strWaferID);

                    m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                }
                else	// Wafer 최초 Test
                {
                     //:lot_seq, :wafer_id, :tester, :probe_card, :operator, :probe_cnt,
                     //:tested_die, :start_time, :end_time, :wafer_cat, :loss_die, :isp_initem, 
                     //:visual_item, :isp_outitem, :isp_incmt ,:visual_cmt, :isp_outcmt, :fvi_flag
                    oDSLWafer.CopyWafer(m_strLot_Seq, DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"),"0", PreWaferSeq);

                    dtWafer = oDSLWafer.GetIsWafer(m_strLot_Seq, strWaferID);

                    m_strWafer_Seq = dtWafer.Rows[0][0].ToString();
                }

                //// 해당 Lot Seq의 Wafer수
                m_strWaferCount = oDSLWafer.GetWaferCount(m_strLot_Seq).Rows[0][0].ToString();
                m_strLotStartTime = dtWafer.Rows[0]["START_TIME"].ToString();
                m_strLotEndTime = dtWafer.Rows[0]["END_TIME"].ToString();

                //// Lot에 Wafer수 EndTime Update
                oDSLLot.UpdateCount(m_strLotStartTime, m_strLotEndTime, m_strWaferCount, m_strLot_Seq);

                #endregion -----------------------------------------------------------------------------

                #region ' Wafer RowData Insert '

                //oPRB.CopyRowDataNotPrb("T_AOI", "T_AOI", m_strWafer_Seq, PreWaferSeq);
                oPRB.CopyRowDataForAOI("T_AOI", strTable, m_strWafer_Seq, PreWaferSeq);

                #endregion -----------------------------------------------------------------------------

                #region ' Merge Sum ' =================================================================
                commonMethod = new CommonMethods();

                DataTable dtMergeRule = commonMethod.GetMergeRuleByRuleName(strCustomer, strProduct, PreTestArea);
                DataTable dtMergeBinDesc = commonMethod.GetMergeBinDesc(strCustomer, strLotId, strWaferID, strProduct, dtMergeRule);

                DataRow[] drGecBin = dtMergeBinDesc.Select("HIGH_GEC = 'T'");

                DataTable dtMergeSum = oDSLWSum.GetMergeSum("T_AOI", m_strWafer_Seq);

                // Good bin Cnt
                int GecCnt = 0;
                for (int i = 0; i < dtMergeSum.Rows.Count; i++)
                {
                    for (int j = 0; j < drGecBin.Length; j++)
                    {
                        if (dtMergeSum.Rows[i]["BIN"].ToString() == drGecBin[j]["BIN"].ToString())
                            GecCnt = Convert.ToInt32(dtMergeSum.Rows[i]["CNT"].ToString());
                    }
                }
                // Gec Bin Cnt Add
                DataRow drGecCnt = dtMergeSum.NewRow();
                drGecCnt["CNT"] = GecCnt;
                dtMergeSum.Rows.Add(drGecCnt);

                // Bin Total Cnt 
                DataTable dtBin = oPRB.SelectInfo("T_AOI", m_strWafer_Seq);
                DataRow drTotalCnt = dtMergeSum.NewRow();
                drTotalCnt["CNT"] = dtBin.Rows.Count;
                dtMergeSum.Rows.Add(drTotalCnt);

                #endregion =============================================================================

                return dtMergeSum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SumData(HeaderData hd, string TestArea)
        {
            #region ' Variable Declaration ' 

            T_TPS_BINDESC oDSLBinDesc = null;
            T_TPS_MAPDEF oDSLMapDef = null;
            T_PRB oDSLPrb = null;
            T_TPS_WAFER_SUM oDSLWaferSum = null;
            T_TPS_LOT_SUM oDSLLotSum = null;
            T_TPS_PRODUCT oDSLprod = null;
            T_TPS_PROGRAM oDSLprog = null;

            DataTable dtBinDesc = null;
            DataTable dtMapDef = null;
            DataTable dtPrb = null;
            DataTable dtWaferSum = null;
            DataTable dtLotSum = null;
            CommonMethods commonMethod = null;
            //MAPANALYSIS_DATA oBSLMapAnal = null;

            string strColumnValue;
            string strTableName;

            double dou;
            int[] iGecBin;
            int iGec;
            int i;
            int[] iBin = new int[256];

            int iPassDie = 0;
            int iTestDie = 0;
            int iLossDie = 0;

            #endregion ---------------------------------------------

            try
            {
                #region ' Variable Initializer ' 

                oDSLBinDesc = new T_TPS_BINDESC();
                oDSLMapDef = new T_TPS_MAPDEF();
                oDSLPrb = new T_PRB();
                oDSLWaferSum = new T_TPS_WAFER_SUM();
                oDSLLotSum = new T_TPS_LOT_SUM();
                oDSLprod = new T_TPS_PRODUCT();
                oDSLprog = new T_TPS_PROGRAM();
                commonMethod = new CommonMethods();
                //oBSLMapAnal = new MAPANALYSIS_DATA();
                
                // Tested Die
                hd.TestDies = m_iTestDie;
                //hd.WaferID = hd.WaferID.Trim().PadLeft(2, '0');

                // Start / End Time
                m_strLotStartTime = Convert.ToDateTime(m_strLotStartTime).ToString("yyyyMMddHHmmss");
                m_strLotEndTime = Convert.ToDateTime(m_strLotEndTime).ToString("yyyyMMddHHmmss");

                // NetDie
                m_strNetDie = Convert.ToString(m_iTestDie);

                if (TestArea == "PROBETEST")
                    strTableName = "T_PRB_" + hd.Device.ToUpper().Trim().Replace("-", "_");
                else
                    strTableName = "T_" + TestArea;

                #endregion ---------------------------------------------------

                #region ' Wafer Sum '

                dtBinDesc = commonMethod.SelectGecBinByAllAtOnce(hd.Customer, hd.Device, hd.TestArea, hd.Format);

                iGecBin = new int[dtBinDesc.Rows.Count];
                for (i = 0; i < dtBinDesc.Rows.Count; i++)
                {
                    iGecBin[i] = Convert.ToInt32(dtBinDesc.Rows[i]["BIN"].ToString());
                }
                
                dtPrb = oDSLPrb.SelectBinData(strTableName, m_strWafer_Seq);

                for (i = 0; i < dtPrb.Rows.Count; i++)
                {
                    iBin[Convert.ToInt32(dtPrb.Rows[i][0].ToString())] = Convert.ToInt32(dtPrb.Rows[i][1].ToString());
                }

                iGec = 0;
                for (i = 0; i < iGecBin.Length; i++)
                {
                    iGec = iGec + iBin[iGecBin[i]];
                }

                // Wafer Yield Calculate
                dou = (double)(iGec) / (double)hd.TestDies * 100;

                strColumnValue = "";
                for (i = 1; i < 256; i++)
                {
                    if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                    strColumnValue = strColumnValue + iBin[i].ToString();
                }

                strColumnValue = strColumnValue + "," + iGec.ToString() + ",0";

                strColumnValue = "'" + m_strCustomer + "','" + m_strFacility + "','" + hd.Device.ToUpper() + "','" +
                    m_strTestArea + "','" + hd.Device.ToUpper() +"','" + hd.Format + "','" + hd.MotherLotNo + "','" + hd.LotNo + "'," +
                    m_strLot_Seq + ",'" + hd.WaferID + "'," + m_strWafer_Seq + ",'" +
                    hd.TesterID + "','" + hd.ProberCard + "','" + hd.Operator + "','F', 0," + m_strNetDie + "," +
                    "TO_DATE('" + m_strWaferStartTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    "TO_DATE('" + m_strWaferEndTime + "','YYYY-MM-DD HH24:MI:SS') " +
                    ", 0," + m_strLossDie + "," + hd.TestDies + "," +
                    dou.ToString() + ",0" + "," + strColumnValue;

                oDSLWaferSum.UpdateDataProbeCnt(m_strLot_Seq, hd.WaferID);
                oDSLWaferSum.CreateDataWafer(strColumnValue);

                // Golden Map Check Update
                DataTable dtGoldenCheck = oDSLWaferSum.SelectGoldenCheck(m_strWafer_Seq, hd.Device.ToUpper(), m_strTableName);
                if (dtGoldenCheck.Rows.Count < 1)
                    oDSLWaferSum.UpadteGoldenFlag(m_strWafer_Seq);

                #endregion ----------------------------------------------------------------

                #region ' Lot Sum '
                while(true)
                {
                    try
                    {
                        // BinCount Get
                        dtWaferSum = oDSLWaferSum.SelectBinSum(m_strLot_Seq);

                        dtLotSum = oDSLLotSum.GetIsLot(m_strLot_Seq);

                        // If
                        if (dtLotSum != null && dtLotSum.Rows.Count > 0)
                        {
                            oDSLLotSum.DeleteLot(m_strLot_Seq);
                        }

                        iPassDie = Convert.ToInt32(dtWaferSum.Rows[0]["GEC"]);
                        iLossDie = Convert.ToInt32(dtWaferSum.Rows[0]["LOSS_DIE"]);
                        iTestDie = Convert.ToInt32(dtWaferSum.Rows[0]["TESTED_DIE"]);

                        // 수율
                        dou = ((double)(iPassDie) / (double)(iTestDie)) * 100;

                        strColumnValue = "";
                        for (i = 1; i < 257; i++)
                        {
                            if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                            strColumnValue = strColumnValue + dtWaferSum.Rows[0][i].ToString();
                        }

                        strColumnValue = "'" + m_strCustomer + "','" + m_strFacility + "','" + hd.Device + "','" +
                            m_strTestArea + "','" + hd.Device + "','" + hd.MotherLotNo + "','" + hd.LotNo + "'," +
                            m_strLot_Seq + "," +
                            "TO_DATE('" + m_strLotStartTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                            "TO_DATE('" + m_strLotEndTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                            iLossDie.ToString() + "," + iTestDie.ToString() + "," + dou.ToString() + ",0," +
                            strColumnValue + "," + m_strWaferCount + ",0";

                        oDSLLotSum.CreateLot(strColumnValue);

                        break;
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.Contains("ORA-00001"))
                            throw ex;
                    }
                }
                
                #endregion --------------------------------------
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtBinDesc != null) dtBinDesc.Dispose();
                if (dtMapDef != null) dtMapDef.Dispose();
                if (dtPrb != null) dtPrb.Dispose();
                if (dtWaferSum != null) dtWaferSum.Dispose();
                if (dtLotSum != null) dtLotSum.Dispose();

                dtBinDesc = null;
                dtMapDef = null;
                dtPrb = null;
                dtWaferSum = null;
                dtLotSum = null;
            }
        }

        public void SumDataMerge(HeaderData hd, string TestArea, DataTable dtSumData)
        {
            int iGec;
            double dou;
            int[] iBin = new int[256];

            int iPassDie;
            int iLossDie;
            int iTestDie;

            string strColumnValue;

            T_TPS_WAFER_SUM oDSLWaferSum = null;
            T_TPS_LOT_SUM oDSLLotSum = null;

            DataTable dtWaferSum;
            DataTable dtLotSum;

            try
            {
                oDSLWaferSum = new T_TPS_WAFER_SUM();
                oDSLLotSum = new T_TPS_LOT_SUM();
                // Start / End Time
                m_strLotStartTime = Convert.ToDateTime(m_strLotStartTime).ToString("yyyyMMddHHmmss");
                m_strLotEndTime = Convert.ToDateTime(m_strLotEndTime).ToString("yyyyMMddHHmmss");

                // NetDie
                m_strNetDie = Convert.ToString(m_iTestDie);

                #region ' Wafer Sum '

                iGec = hd.PassDies;

                // Wafer Yield Calculate
                dou = (double)(iGec) / (double)hd.TestDies * 100;
                  
                // Bin Count
                strColumnValue = "";
                for (int i = 1; i < 256; i++)
                {
                    if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                    strColumnValue = strColumnValue + dtSumData.Rows[0][i - 1].ToString();
                }

                strColumnValue = strColumnValue + "," + iGec.ToString() + ",0";

                strColumnValue = "'" + m_strCustomer + "','" + m_strFacility + "','" + hd.Device.ToUpper() + "','" +
                    m_strTestArea + "','" + hd.Device.ToUpper() + "','" + hd.Format + "','" + hd.MotherLotNo + "','" + hd.LotNo + "'," +
                    m_strLot_Seq + ",'" + hd.WaferID + "'," + m_strWafer_Seq + ",'" +
                    hd.TesterID + "','" + hd.ProberCard + "','" + hd.Operator + "','F',0," + m_strNetDie + "," +
                    "TO_DATE('" + m_strWaferStartTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    "TO_DATE('" + m_strWaferEndTime + "','YYYY-MM-DD HH24:MI:SS') " +
                    ", 0," + m_strLossDie + "," + hd.TestDies + "," +
                    dou.ToString() + ","+ hd.VIFail.ToString() + "," + strColumnValue;

                oDSLWaferSum.UpdateDataProbeCnt(m_strLot_Seq, hd.WaferID);
                oDSLWaferSum.CreateDataWafer(strColumnValue);

                // Golden Map Check Update
                DataTable dtGoldenCheck = oDSLWaferSum.SelectGoldenCheck(m_strWafer_Seq, hd.Device.ToUpper(), m_strTableName);
                if (dtGoldenCheck.Rows.Count < 1)
                    oDSLWaferSum.UpadteGoldenFlag(m_strWafer_Seq);

                #endregion ---------------------------------------------------

                #region ' Lot Sum '

                // BinCount Get
                dtWaferSum = oDSLWaferSum.SelectBinSum(m_strLot_Seq);

                dtLotSum = oDSLLotSum.GetIsLot(m_strLot_Seq);

                // If
                if (dtLotSum != null && dtLotSum.Rows.Count > 0)
                {
                    oDSLLotSum.DeleteLot(m_strLot_Seq);
                }

                iPassDie = Convert.ToInt32(dtWaferSum.Rows[0]["GEC"]);
                iLossDie = Convert.ToInt32(dtWaferSum.Rows[0]["LOSS_DIE"]);
                iTestDie = Convert.ToInt32(dtWaferSum.Rows[0]["TESTED_DIE"]);

                // 수율
                dou = ((double)(iPassDie) / (double)(iTestDie)) * 100;

                strColumnValue = "";
                for (int i = 1; i < 257; i++)
                {
                    if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                    strColumnValue = strColumnValue + dtWaferSum.Rows[0][i].ToString();
                }

                strColumnValue = "'" + m_strCustomer + "','" + m_strFacility + "','" + hd.Device + "','" +
                    m_strTestArea + "','" + hd.Device + "','" + hd.MotherLotNo + "','" + hd.LotNo + "'," +
                    m_strLot_Seq + "," +
                    "TO_DATE('" + m_strLotStartTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    "TO_DATE('" + m_strLotEndTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    iLossDie.ToString() + "," + iTestDie.ToString() + "," + dou.ToString() + ",0," +
                    strColumnValue + "," + m_strWaferCount + ",0";

                oDSLLotSum.CreateLot(strColumnValue);
                #endregion -----------------------------------------------------------------
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SumDataForAOI(string PreWaferSeq, DataTable dtMergeSum)
        {
            int iGec;
            double dou;
            int[] iBin = new int[256];

            int iPassDie;
            int iLossDie;
            int iTestDie;

            string strColumnValue;

            T_TPS_WAFER_SUM oDSLWaferSum = null;
            T_TPS_LOT_SUM oDSLLotSum = null;

            DataTable dtWaferSum;
            DataTable dtLotSum;

            try
            {
                oDSLWaferSum = new T_TPS_WAFER_SUM();
                oDSLLotSum = new T_TPS_LOT_SUM();
                // Start / End Time
                m_strLotStartTime = Convert.ToDateTime(m_strLotStartTime).ToString("yyyyMMddHHmmss");
                m_strLotEndTime = Convert.ToDateTime(m_strLotEndTime).ToString("yyyyMMddHHmmss");



                // NetDie
                m_strNetDie = Convert.ToString(m_iTestDie);

                #region ' Wafer Sum '

                iGec = iPassDie = Convert.ToInt32(dtMergeSum.Rows[255]["CNT"].ToString());
                iLossDie = Convert.ToInt32(dtMergeSum.Rows[256]["CNT"].ToString()) - iGec;
                iTestDie = iPassDie + iLossDie;

                // Wafer Yield Calculate
                dou = (double)(iGec) / (double)iTestDie * 100;

                // Bin Count
                strColumnValue = "";
                for (int i = 0; i < 255; i++)
                {
                    if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                    strColumnValue = strColumnValue + dtMergeSum.Rows[i][1].ToString();
                }

                strColumnValue = strColumnValue + "," + iGec.ToString();


                oDSLWaferSum.UpdateDataProbeCnt(m_strLot_Seq, m_strWaferID);
                oDSLWaferSum.CopyWaferSum(strColumnValue, "AOI", m_strLot_Seq, m_strWafer_Seq, m_strWaferStartTime, m_strWaferEndTime
                    , iLossDie.ToString(), dou.ToString(), m_strPreWafer_Seq);

                #endregion ---------------------------------------------------

                #region ' Lot Sum '

                // BinCount Get
                dtWaferSum = oDSLWaferSum.SelectBinSum(m_strLot_Seq);

                dtLotSum = oDSLLotSum.GetIsLot(m_strLot_Seq);

                // If
                if (dtLotSum != null && dtLotSum.Rows.Count > 0)
                {
                    oDSLLotSum.DeleteLot(m_strLot_Seq);
                }

                iPassDie = Convert.ToInt32(dtWaferSum.Rows[0]["GEC"]);
                iLossDie = Convert.ToInt32(dtWaferSum.Rows[0]["LOSS_DIE"]);
                iTestDie = Convert.ToInt32(dtWaferSum.Rows[0]["TESTED_DIE"]);

                // 수율
                dou = ((double)(iPassDie) / (double)(iTestDie)) * 100;

                strColumnValue = "";
                for (int i = 1; i < 257; i++)
                {
                    if (strColumnValue != "") strColumnValue = strColumnValue + ",";
                    strColumnValue = strColumnValue + dtWaferSum.Rows[0][i].ToString();
                }

                DataSet dsInfo = oDSLWaferSum.GetWaferInfo(m_strWafer_Seq);

                string ProductFOrLotSum = dsInfo.Tables[0].Rows[0]["PRODUCT"].ToString();
                string LotIdForLotSum = dsInfo.Tables[0].Rows[0]["LOT_ID"].ToString();
                m_strFacility = dsInfo.Tables[0].Rows[0]["FACILITY"].ToString();
                m_strCustomer = dsInfo.Tables[0].Rows[0]["CUSTOMER"].ToString();
                m_strTestArea = "AOI";

                strColumnValue = "'" + m_strCustomer + "','" + m_strFacility + "','" + ProductFOrLotSum + "','" +
                    m_strTestArea + "','" + ProductFOrLotSum + "','" + LotIdForLotSum + "','" + LotIdForLotSum + "'," +
                    m_strLot_Seq + "," +
                    "TO_DATE('" + m_strLotStartTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    "TO_DATE('" + m_strLotEndTime + "','YYYY-MM-DD HH24:MI:SS'), " +
                    iLossDie.ToString() + "," + iTestDie.ToString() + "," + dou.ToString() + ",0," +
                    strColumnValue + "," + m_strWaferCount + ",0";

                oDSLLotSum.CreateLot(strColumnValue);
                #endregion -----------------------------------------------------------------
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion--------------------------------------------------------------------------------

        #region ' Method '-------------------------------------------------------------------------

        private void GetBinInfo(int sbin, ref DataTable dt, ref string strHBin, ref string strCBin)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr[0]) == sbin)
                {
                    strHBin = dr["BIN"].ToString();
                    strCBin = dr["BIN"].ToString();
                    return;
                }
            }
        }

        private DataTable SelectGecBinByAll(string Customer, string Product, string TestArea, string Format)
        {
            T_TPS_BINDESC oBindescDSL = null;

            string Contents = null;
            DataTable dtBinDesc = null;

            try
            {
                Contents = string.Empty;
                oBindescDSL = new T_TPS_BINDESC();

                // Customer
                Contents = Contents + "AND CUSTOMER = '" + Customer + "'";
                dtBinDesc = oBindescDSL.GetBindescByElement(Contents);
                if (dtBinDesc == null || dtBinDesc.Rows.Count < 1)
                {
                    Contents = Contents.Replace(Customer, " ");
                    Customer = " ";
                }
                // Product
                Contents = Contents + " AND PRODUCT = '" + Product + "'";
                dtBinDesc = oBindescDSL.GetBindescByElement(Contents);
                if (dtBinDesc == null || dtBinDesc.Rows.Count < 1)
                {
                    Contents = Contents.Replace(Product, " ");
                    Product = " ";
                }
                // TestArea
                Contents = Contents + " AND TESTAREA = '" + TestArea + "'";
                dtBinDesc = oBindescDSL.GetBindescByElement(Contents);
                if (dtBinDesc == null || dtBinDesc.Rows.Count < 1)
                {
                    Contents = Contents.Replace(TestArea, " ");
                    TestArea = " ";
                }
                // Format
                Contents = Contents + "AND FORMAT = '" + Format + "'";
                dtBinDesc = oBindescDSL.GetBindescByElement(Contents);
                if (dtBinDesc == null || dtBinDesc.Rows.Count < 1)
                {
                    Contents = Contents.Replace(Format, " ");
                    Format = " ";
                }

                return dtBinDesc = oBindescDSL.SelectGecBinByAll(Customer, Product, TestArea, Format);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion ----------------------------------------------------------------
    }
}
