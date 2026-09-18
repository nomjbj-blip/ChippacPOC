using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.TEST.DSL;

namespace DACrux.TEST.BSL
{
    public class ProbeMapAnalysis : Miracom.Middleware.BaseComponent, DACrux.TEST.Interface.iProbeMapAnalysis
    {
        enum MapUpdate { WAFER_SEQ, DIE_NUM, IndexX, IndexY }

        public static string[] m_tbl = new string[] {
                                     "A","A","B","C","D","E","F","G","H","I","J"
                                    ,"K","L","M","N","O","P","Q","R","S","T"
                                    ,"U","V","W","X","Y","Z"};

        public DataSet GetWaferMap(string TestArea, string Product, string Program, string LotID, string WaferID, int ProbeCnt, bool IsMaxOper = false)
        {
            TQP_WAFER_SUM oSum = null;
            long WaferSeq = -1;
            try
            {
                oSum = new TQP_WAFER_SUM();
                DataTable tmpDT = oSum.GetWaferInfo(TestArea, Product, Program, LotID, WaferID, ProbeCnt, IsMaxOper);
                if (tmpDT != null && tmpDT.Rows.Count > 0)
                {
                    if (long.TryParse(tmpDT.Rows[0]["WAFER_SEQ"].ToString(), out WaferSeq) == false) WaferSeq = -1;
                }

                if (WaferSeq == -1)
                {
                    return GetEnptyWaferMap(TestArea, Product);
                }

                return GetWaferMap(WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEnptyWaferMap(string TestArea, string Product)
        {
            DataSet rDS = null;
            DataTable rDT_WaferInfo = null;
            DataTable rDT_MapDef = null;
            DataTable rDT_MapDefTemp = null;
            DataTable rDT_BinSum = null;
            DataTable rDT_MapData = null;

            string strTestArea = string.Empty;
            string strMapID = string.Empty;
            try
            {
                T_AVI_TABLE oAvi = new T_AVI_TABLE();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();

                rDS = new DataSet();

                /// Map 정보
                rDT_WaferInfo = oWafer.GetWaferInfo(-1);
                strMapID = oProduct.GetMapID(Product);
                rDT_MapDefTemp = oMapDef.GetMapDef(strMapID);
                rDT_MapDef = rDT_MapDefTemp.Copy();
                rDT_BinSum = oAvi.GetBinDistribution(Product, -1, TestArea);
                rDT_MapData = oAvi.GetAVIMapData(TestArea, Product, -1);

                rDT_BinSum.TableName = "BINSUM";
                rDT_MapDef.TableName = "RECIPE";
                rDT_WaferInfo.TableName = "WAFER_INFO";
                rDT_MapData.TableName = "MAPDATA";

                rDS.Tables.Add(rDT_BinSum);
                rDS.Tables.Add(rDT_MapDef);
                rDS.Tables.Add(rDT_WaferInfo);
                rDS.Tables.Add(rDT_MapData);

                return rDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataSet GetWaferMap(long WaferSeq)
        {
            DataSet rDS = null;
            DataTable rDT_WaferInfo = null;
            DataTable rDT_MapDef = null;
            DataTable rDT_BinSum = null;
            DataTable rDT_MapData = null;
            DataTable dtTables = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strMapID = string.Empty;
            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                T_AVI_TABLE oAvi = new T_AVI_TABLE();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();

                rDS = new DataSet();

                rDT_WaferInfo = oWafer.GetWaferLotInfo(WaferSeq);
                if (rDT_WaferInfo != null && rDT_WaferInfo.Rows.Count > 0)
                {
                    strTestArea = rDT_WaferInfo.Rows[0]["TESTAREA"].ToString();
                    //strProduct = rDT_WaferInfo.Rows[0]["PRODUCT"].ToString();
                    strProduct = rDT_WaferInfo.Rows[0]["DEVICE_ALIAS"].ToString();
                    strProgram = rDT_WaferInfo.Rows[0]["PROGRAM"].ToString();

                    if (strTestArea == "PARAMETRIC" || strTestArea == "INLINE")
                    {
                        StringBuilder sQuery = new StringBuilder();
                        Dictionary<string, string> dicAlias = new Dictionary<string, string>();


                        dtTables = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                        if (dtTables != null && dtTables.Rows.Count > 0)
                        {
                            string[] strTable = new string[dtTables.Rows.Count];


                            for (int it = 0; it < dtTables.Rows.Count; it++)
                            {
                                strTable[it] = string.Format("{0} {1}", dtTables.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTables.Rows[it]["RN"].ToString())]);
                                dicAlias.Add(dtTables.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTables.Rows[it]["RN"].ToString())]);
                            }

                            sQuery.AppendLine(string.Join(",", strTable));
                            sQuery.AppendLine(" WHERE 1 = 1 ");

                            int ic = 0;
                            string[] arrTemp = new string[2];
                            foreach (KeyValuePair<string, string> var in dicAlias)
                            {
                                if (ic == 0)
                                {
                                    sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                                }
                                else
                                {
                                    sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                                    sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                                }
                                arrTemp[0] = var.Key;
                                arrTemp[1] = var.Value;
                                ic++;
                            }
                        }

                        rDT_MapData = oProgram.GetData("GET_MAPDATA_PCM", new string[] { sQuery.ToString() }, null);
                        if (rDT_MapData != null && rDT_MapData.Rows.Count > 0)
                        {
                            int iXmax, iYmax, iXmin, iYmin = 0;

                            if (System.Convert.ToInt32(rDT_MapData.Compute("MAX(X)", "")) % 2 <= 0)
                                iXmax = System.Convert.ToInt32(rDT_MapData.Compute("MAX(X)", "")) + 1;
                            else
                                iXmax = System.Convert.ToInt32(rDT_MapData.Compute("MAX(X)", "")) + 2;

                            if (System.Convert.ToInt32(rDT_MapData.Compute("MAX(Y)", "")) % 2 <= 0)
                                iYmax = System.Convert.ToInt32(rDT_MapData.Compute("MAX(Y)", "")) + 1;
                            else
                                iYmax = System.Convert.ToInt32(rDT_MapData.Compute("MAX(Y)", "")) + 2;

                            if (System.Convert.ToInt32(rDT_MapData.Compute("MIN(X)", "")) % 2 <= 0)
                                iXmin = System.Convert.ToInt32(rDT_MapData.Compute("MIN(X)", "")) - 1;
                            else
                                iXmin = System.Convert.ToInt32(rDT_MapData.Compute("MIN(X)", "")) - 2;

                            if (System.Convert.ToInt32(rDT_MapData.Compute("MIN(Y)", "")) % 2 <= 0)
                                iYmin = System.Convert.ToInt32(rDT_MapData.Compute("MIN(Y)", "")) - 1;
                            else
                                iYmin = System.Convert.ToInt32(rDT_MapData.Compute("MIN(Y)", "")) - 2;

                            rDT_MapDef = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);

                        }

                        rDT_BinSum = oProgram.GetData("GET_BIN_DISTRIBUTION_FOR_PCM", new string[] { sQuery.ToString() }, null);
                    }
                    else
                    {
                        strMapID = oProduct.GetMapID(strProduct);
                        rDT_MapDef = oMapDef.GetMapDef(strMapID);

                        rDT_BinSum = oProgram.GetBinDistribution(strProgram, WaferSeq);
                        rDT_MapData = oProgram.GetMapData(strProgram, WaferSeq);

                    }
                }
                else
                {
                    return null;
                }

                rDT_BinSum.TableName = "BINSUM";
                rDT_MapDef.TableName = "RECIPE";
                rDT_WaferInfo.TableName = "WAFER_INFO";
                rDT_MapData.TableName = "MAPDATA";

                rDS.Tables.Add(rDT_BinSum);
                rDS.Tables.Add(rDT_MapDef);
                rDS.Tables.Add(rDT_WaferInfo);
                rDS.Tables.Add(rDT_MapData);

                return rDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetCumMap(long[] WaferSeqs)
        {
            DataSet rDS = null;

            DataTable rDT_WaferInfo = null;
            DataTable rDT_MapDef = null;
            DataTable rDT_BinSum = null;


            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strMapID = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                rDS = new DataSet();


                /// Wafer Infomation================================================================
                rDT_WaferInfo = oWafer.GetWaferLotInfo(WaferSeqs);
                rDT_WaferInfo.TableName = "WAFER_INFO";

                if (rDT_WaferInfo != null && rDT_WaferInfo.Rows.Count > 0)
                {
                    //strProduct = rDT_WaferInfo.Rows[0]["PRODUCT"].ToString();
                    strProduct = rDT_WaferInfo.Rows[0]["DEVICE_ALIAS"].ToString();
                    strProgram = rDT_WaferInfo.Rows[0]["PROGRAM"].ToString();
                    strMapID = oProduct.GetMapID(strProduct);

                    /// BIN Description & Summary========================================================
                    rDT_BinSum = oProgram.GetBinDistribution(strProgram, WaferSeqs);

                    /// Map Information ========================================================
                    rDT_MapDef = oMapDef.GetMapDef(strMapID);
                }
                else
                {
                    return null;
                }

                rDT_BinSum.TableName = "BINDESC";
                rDT_MapDef.TableName = "RECIPE";

                rDS.Tables.Add(rDT_WaferInfo.Copy());
                rDS.Tables.Add(rDT_BinSum.Copy());
                rDS.Tables.Add(rDT_MapDef.Copy());

                return rDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetCumRawData(string Program, long[] WaferSeq, string[] SelBin)
        {
            DataTable rDT = null;
            TQP_PROGRAM oProgram = new TQP_PROGRAM();
            try
            {
                /// Raw Data=======================================================================
                rDT = oProgram.GetMapData(Program, WaferSeq, SelBin);
                rDT.TableName = "MAPDATA";
                return rDT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistribution(string program, long waferSeq)
        {
            DataTable rDT = null;
            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                rDT = oProgram.GetBinDistribution(program, waferSeq);
                return rDT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapData(string program, long[] WaferSeq, string[] BINS)
        {
            DataTable dt = null;
            TQP_PROGRAM oProgram = null;

            try
            {
                oProgram = new TQP_PROGRAM();

                dt = oProgram.GetMapData(program.Replace("-", "_").Replace(".", ""), WaferSeq, BINS);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetYieldByDay(string[] waferSeq)
        {
            DataTable rDT = null;
            try
            {
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                rDT = oWaferSum.GetYieldByDay(waferSeq);
                return rDT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(long WaferSeq)
        {
            DataTable dt = null;
            try
            {
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                dt = oWaferSum.GetWaferLotInfo(WaferSeq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(long[] WaferSeq)
        {
            DataTable dt = null;
            try
            {
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                dt = oWaferSum.GetWaferInfo(WaferSeq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetWaferInfo(string[] WaferIDs)
        {
            DataTable dt = null;
            try
            {
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                dt = oWaferSum.GetWaferInfo(WaferIDs);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDataForDefectOverlay(string PROGRAM, string WaferSeq)
        {
            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                return oProgram.GetMapDataForDefectOverlay(PROGRAM, WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateParaSpec(string[,] ParaInfo)
        {
            TQP_PARASPEC oTpsParaSpec = null;
            try
            {
                oTpsParaSpec = new TQP_PARASPEC();
                oTpsParaSpec.CreateParaSpec(ParaInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetParaSpecListEditable(string factory, string Program)
        {
            TQP_PARASPEC oTpsParaSpec = null;
            try
            {
                oTpsParaSpec = new TQP_PARASPEC();
                return oTpsParaSpec.GetParaSpecListEditable(factory, Program);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramParaSpec(string Program, string[,] ParaInfo)
        {
            TQP_PARASPEC oTpsParaSpec = null;
            try
            {
                oTpsParaSpec = new TQP_PARASPEC();
                oTpsParaSpec.UpdateProgramParaSpec(Program, ParaInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProgramParaSpec(string Program)
        {
            TQP_PARASPEC oTpsParaSpec = null;
            try
            {
                oTpsParaSpec = new TQP_PARASPEC();
                oTpsParaSpec.DeleteProgramParaSpec(Program);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapData(string Program, long WaferSeq, string paraItem, int cutCnt)
        {
            TQP_PROGRAM oProgram = null;
            try
            {
                oProgram = new TQP_PROGRAM();
                return oProgram.GetMapData(Program, WaferSeq, paraItem, cutCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDataCnt(string Program, long WaferSeq, string paraItem, int cutCnt)
        {
            TQP_PROGRAM oProgram = null;
            try
            {
                oProgram = new TQP_PROGRAM();
                return oProgram.GetMapDataCnt(Program, WaferSeq, paraItem, cutCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfoByLotSlot(string lot_id, string slot_id, string testarea)
        {
            DataTable dt = null;
            TQP_WAFER_SUM oWaferSum = null;

            try
            {
                oWaferSum = new TQP_WAFER_SUM();
                dt = oWaferSum.GetWaferInfoByLotSlot(lot_id, slot_id, testarea);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfoByWaferId(string wafer_id, string testarea)
        {
            DataTable dt = null;
            TQP_WAFER_SUM oWaferSum = null;

            try
            {
                oWaferSum = new TQP_WAFER_SUM();
                dt = oWaferSum.GetWaferInfoByWaferId(wafer_id, testarea);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDefByLotID(string LotID)
        {
            TQP_MAPDEF oDSL = null;
            try
            {
                oDSL = new TQP_MAPDEF();
                return oDSL.GetMapDefByLotID(LotID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string TestArea, string Product, string LotID, string WaferID)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferInfo(TestArea, Product, LotID, WaferID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistribution(string PROGRAM, string[] WaferSeq, bool bMulti, bool bVI)
        {
            DataTable dt = new DataTable();
            TQP_PROGRAM oProgramDSL = null;
            TQP_BINDESC oBindescDSL = null;
            TQP_WAFER_SUM oDSLWaferSum = null;
            //TQP_MERGERULE oMergeRule = null;
            TQP_PRODUCT oProduct = null;
            TQP_LOT_SUM oDSLLotSum = null;
            //SEMDMS.DSL.T_DMS_INSP_INFO oInspInfo = null;

            //DataSet ds = null;
            //bool bUndefine = false;

            string strCustomer = null;
            string strProduct = null;
            string strTestArea = null;
            //string strFormat = null;
            string strLotId = null;
            string strWaferIdForAVI = null;
            string strCopyFlag = null;
            string strTestAreaGroup = string.Empty;

            //            string DynamicQuery_BinDESC
            //                =
            //@"SELECT *
            //FROM
            //(
            //    WITH RT AS (
            //    SELECT A.*,
            //           (DECODE(A.CUSTOMER, :customer, 1, 0)
            //           + DECODE(A.PRODUCT, :product, 10, 0)
            //           + DECODE(A.TESTAREA, :testarea, 20, 0)
            //           + DECODE(A.FORMAT, :format, 30, 0)) AS NUM 
            //        FROM TQP_BINDESC A
            //        WHERE 1=1
            //        AND A.CUSTOMER IN (' ', :customer)
            //        AND A.PRODUCT IN (' ',  :product)
            //        AND A.TESTAREA IN (' ', :testarea)
            //        AND A.FORMAT IN (' ', :format)
            //        )
            //    SELECT * FROM RT
            //    WHERE 1=1
            //    AND NUM = (SELECT MAX(NUM) FROM RT)
            //    ORDER BY BIN
            //)";

            //DataTable dtBinDesc = null;
            //DataTable dtMergeBinDesc = null;
            //DataTable dtMergeRule = null;
            DataTable dtProduct = null;
            //DataTable dtDMSProduct = null;

            try
            {
                oProgramDSL = new TQP_PROGRAM();
                oDSLWaferSum = new TQP_WAFER_SUM();
                oBindescDSL = new TQP_BINDESC();
                oDSLLotSum = new TQP_LOT_SUM();
                oProduct = new TQP_PRODUCT();

                // QC VisualINSP 추가
                // AVI 의 AVI 이면
                if (WaferSeq[0] == "AVI")
                {
                    strTestAreaGroup = WaferSeq[0];

                    strTestArea = WaferSeq[1];
                    strLotId = WaferSeq[2];
                    strWaferIdForAVI = WaferSeq[3].Substring(WaferSeq[3].Length - 2, 2);

                    strProduct = PROGRAM;

                    // Customer 검색
                    dtProduct = oProduct.SelectCustomerWithMapdef(strProduct);
                    if (dtProduct.Rows.Count < 1)
                    {
                        throw new Exception(string.Format("Product : {0} is not define as Golden Map", strProduct));
                    }

                    strCustomer = dtProduct.Rows[0]["CUSTOMER"].ToString();

                    // Format 검색
                    //dtDMSProduct = oInspInfo.SelectProduct(strLotId, strTestArea, strWaferIdForAVI);
                    //strFormat = dtDMSProduct.Rows[0]["FORMAT"].ToString();
                }
                else
                {
                    DataTable dsWaferSum = oDSLWaferSum.GetWaferLotInfo(DACrux.Base.Convert.longParse(WaferSeq[0]));
                    strLotId = dsWaferSum.Rows[0]["LOT_ID"].ToString();
                    strTestArea = dsWaferSum.Rows[0]["TESTAREA"].ToString();
                    strCustomer = dsWaferSum.Rows[0]["CUSTOMER"].ToString();
                    //strCopyFlag = dsWaferSum.Tables[0].Rows[0]["COPY_DATA"].ToString();
                    strProduct = PROGRAM;
                    //strFormat = " ";
                }

                // Get DB Table Name =========================================================================================
                string strTableName = string.Empty;

                string Program = PROGRAM.ToUpper().Replace("-", "_").Replace(".", "_").Replace(" ", "");
                if (strTestArea.Contains("MERGE"))
                {
                    //DataTable dtTestArea = oTestArea.SelectTestArea(strTestArea);
                    //strTableName = dtTestArea.Rows[0]["SAVE_TABLE"].ToString().Replace("%", Program);
                }

                //else if (strTestArea != "PROBETEST" && strTestArea != "MULTIPROBE")
                //    strTableName = string.Format("T_{0}_{1}", strTestArea, Program);
                //else
                strTableName = "TD_" + Program;

                // Get Info By Dynamic Query =========================================================================
                // MERGE, AOI(COPY), VI의 PROBETEST 는 이쪽을 사용
                if (strTestArea.Contains("MERGE") || (strTestArea == "AOI" && strCopyFlag == "T"))//  || (strTestArea == "PROBETEST" && bVI))
                {
                    //string Dynamic_bindesc = string.Empty;
                    //string Dynamic_total = string.Empty;

                    //string TestArea = string.Empty;
                    //string TestAreaDesc = string.Empty;

                    //if (strTestArea == "AOI")
                    //{
                    //    DataTable dtLot = oDSLLotSum.SelectLotInfoByLotIDExcludeFinal(strLotId);
                    //    strTestArea = dtLot.Rows[0]["TESTAREA"].ToString();
                    //}

                    //if (bVI)
                    //    dtMergeRule = oMergeRule.SelectMergeRuleByRuleName(strCustomer, strProduct, "CP_MERGE");
                    //else
                    //dtMergeRule = oMergeRule.SelectMergeRuleByRuleName(strCustomer, strProduct, strTestArea);

                    //if (dtMergeRule == null || dtMergeRule.Rows.Count < 1)
                    //    throw new Exception(string.Format("MergeRule is not Exist strCustomer Customer : {0}, Product : {1}, TestArea : {2}", strCustomer, strProduct, strTestArea));

                    //for (int i = 0; i < dtMergeRule.Rows.Count; i++)
                    //{
                    //    Dynamic_bindesc = DynamicQuery_BinDESC;

                    //    TestArea = dtMergeRule.Rows[i]["RULE_VALUE"].ToString();
                    //    DataTable dtTestArea = oTestArea.SelectAreaGroup(TestArea);
                    //    TestAreaDesc = dtTestArea.Rows[0][0].ToString();
                    //     Format
                    //    DataTable dtWaferSumList = oDSLWaferSum.GetWaferInfoListByTestArea(TestArea, strLotId);
                    //    if (TestAreaDesc != "AVI")
                    //        strFormat = dtWaferSumList.Rows[0]["FORMAT"].ToString() == null ? " " : dtWaferSumList.Rows[0]["FORMAT"].ToString();
                    //    else
                    //        strFormat = " ";

                    //    if (i > 0)
                    //        Dynamic_total += "UNION \n";

                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":customer", string.Format("'{0}'", strCustomer));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":product", string.Format("'{0}'", strProduct));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":testarea", string.Format("'{0}'", TestArea));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":format", string.Format("'{0}'", strFormat));

                    //    Dynamic_total += Dynamic_bindesc + "\n";
                    //    Dynamic_total += "UNION \n";

                    //     VI input
                    //    if (TestAreaDesc == "AVI")
                    //        TestArea = "VI_AVI";
                    //    else
                    //        TestArea = "VI_" + TestArea;

                    //    Dynamic_bindesc = DynamicQuery_BinDESC;

                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":customer", string.Format("'{0}'", strCustomer));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":product", string.Format("'{0}'", strProduct));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":testarea", string.Format("'{0}'", TestArea));
                    //    Dynamic_bindesc = Dynamic_bindesc.Replace(":format", "' '");

                    //    Dynamic_total += Dynamic_bindesc + "\n";
                    //}

                    //ds = oProgramDSL.GetBinDistributionByAllBoth(Dynamic_total, strTableName, WaferSeq);

                    // ProbeTest 의 VI 는 해당 Wafer 에 쓰일 모든 BIN 과 COLOR를 가져온다.
                    //if (strTestArea == "PROBETEST" && bVI)
                    //{
                    //    ds = oProgramDSL.GetBinDistributionForProbeVI(Dynamic_total);
                    //}
                }
                else
                {
                    // Get BinDesc Args ======================================================================================================
                    //dtBinDesc = oCommon.SelectBinByAllAtOnce(ref strCustomer, ref strProduct, ref strTestArea, ref strFormat);

                    if (strTestAreaGroup == "AVI" && bVI)
                    {
                        //
                        //ds = oProgramDSL.getBinDistributionForAVI(strCustomer, strProduct, strTestArea, strFormat, strLotId, strWaferIdForAVI);

                    }
                    else
                    {
                        if (!bMulti)
                            dt = oProgramDSL.GetBinDistributionByAll(strProduct, strTestArea, strTableName, WaferSeq[0]);

                        else
                            dt = oProgramDSL.GetBinDistributionMultiWaferByAll(strProduct, strTestArea, strTableName, WaferSeq);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string TestArea, string LotID, string WaferID, bool bDummy)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferInfo(TestArea, LotID, WaferID, bDummy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertFailDie(string PRODUCT, string[] strValue)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.InsertFailDie(PRODUCT, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME)
        {
            T_AVI_TABLE oDSL = null;
            long lLotSeq = 0;
            try
            {
                oDSL = new T_AVI_TABLE();
                lLotSeq = oDSL.InsertLot(FACILITY, TESTAREA, PRODUCT, PROGRAM, LOTID, FABLOT, START_TIME, END_TIME);

                return lLotSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long InsertWafer(string LOT_SEQ
                            , string WAFER_ID
                            , string TESTER
                            , string OPERATOR
                            , string TESTED_DIE
                            , string START_TIME
                            , string LOSS_DIE)
        {
            T_AVI_TABLE oDSL = null;
            long lWaferSeq = 0;
            try
            {
                oDSL = new T_AVI_TABLE();
                lWaferSeq = oDSL.InsertWafer(LOT_SEQ, WAFER_ID, TESTER, OPERATOR, TESTED_DIE, START_TIME, LOSS_DIE);
                return lWaferSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region [동부 하이텍 추가]

        /// <summary>
        /// TEST Map Draw 쿼리
        /// </summary>
        /// <param name="WaferSeq"></param>
        /// <returns></returns>
        private readonly string DIENUM = "DIE_NUM";
        public System.Data.DataSet SelectWaferMapDrawData(
            long WaferSeq
            )
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtItem = null;
            DataRow drItem = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            StringBuilder sQuery = new StringBuilder();

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();
                TQP_PARASPEC oParaSepc = new TQP_PARASPEC();
                TQP_FOI_DIE oMapFoi = new TQP_FOI_DIE();
                TQP_FOI_IMAGES oMapFoiImage = new TQP_FOI_IMAGES();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //AVI 와 이외의 공정의 Data Map 을 가져오는 방식이 다르다.
                if (strTestArea == "AVI")
                {
                    dtTemp = oMapFoi.SelectFOIBinSummary(WaferSeq.ToString());
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("AVI 또는 Scope Bin 에 대한 Summary 정보를 가져오지 못했습니다.");

                    dtTemp.TableName = "BINSUM";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapFoiImage.SelectFOIImageList(WaferSeq.ToString());
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "IMAGES";
                        dsData.Tables.Add(dtTemp.Copy());
                    }

                    dtTemp = oMapFoi.SelectFOIMap(WaferSeq.ToString(), "0");
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("AVI 또는 Scope Map Data를 불러오지 못했습니다.");

                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "MAPDATA";
                        dsData.Tables.Add(dtTemp.Copy());
                    }
                }
                else
                {

                    // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                    dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                    Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                    string[] strTable = new string[dtTemp.Rows.Count];

                    for (int it = 0; it < dtTemp.Rows.Count; it++)
                    {
                        strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                        dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    }

                    sQuery.AppendLine(string.Join(",", strTable));
                    sQuery.AppendLine(" WHERE 1 = 1 ");

                    int ic = 0;
                    string[] arrTemp = new string[2];
                    foreach (KeyValuePair<string, string> var in dicAlias)
                    {
                        if (ic == 0)
                        {
                            sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                        }
                        else
                        {
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) ", arrTemp[1], var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM(+) ", arrTemp[1], var.Value));
                        }
                        arrTemp[0] = var.Key;
                        arrTemp[1] = var.Value;
                        ic++;
                    }

                    dtTemp = oProgram.GetData("GET_BIN_SUMMARY", new string[] { sQuery.ToString() }, new string[] { strProgram });
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "BINSUM";
                        dsData.Tables.Add(dtTemp.Copy());
                    }

                    //===============================================================================================

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "MAPDATA";
                        dsData.Tables.Add(dtTemp.Copy());
                    }
                    else
                        throw new Exception("Map Data 가 없습니다.");
                }


                //===============================================================================================


                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");

                //dtTemp = oMapDef.GetMapDef(strProduct);
                //if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //{
                //    //Map Define 정보가 없을 경우 Logic 으로 임의로 만들어 준다. 
                //    dtTemp = dtMapDefine(iYmax, iXmax, iXmin, iYmin);
                //    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //        throw new Exception("Map Define 정보를 불러오지 못했습니다.");
                //}

                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //Parameter 관련 정보를 가져온다.
                dtTemp = oParaSepc.GetData("SELECT_PARA_ITEM", null, new string[] { strProgram, WaferSeq.ToString() });
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    List<string> lsItem = new List<string>();
                    DataTable dtPara;

                    dtItem = new DataTable();
                    dtItem.Columns.Add(new DataColumn("PARAM_NAME", typeof(string)));
                    dtItem.Columns.Add(new DataColumn("MAX", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("MIN", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("AVG", typeof(double)));
                    dtItem.AcceptChanges();

                    // TQP_PARASPEC 테이블에 존재하지 않는 PARAMETER 항목 추가
                    lsItem.Add(String.Format("A.{0}", DIENUM)); 
                    for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                    {
                        lsItem.Add(dtTemp.Rows[ir]["PARAM_NAME"].ToString());

                        //500 개를 기준으로 여러번 Para를 확인 한다.
                        if (ir != 0 && (int)(ir % 500) == (int)0)
                        {
                            lsItem.Sort();

                            // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                            // GetDataTable_Numeric_IgnoreException로 수정
                            //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                            dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                            foreach (DataRow dr in dtPara.Rows)
                            {
                                drItem = dtItem.NewRow();
                                drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                                drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                                drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                                drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                                dtItem.Rows.Add(drItem);
                            }

                            lsItem.Clear();
                        }

                    }

                    //남은  Para 확인
                    if (lsItem.Count > 0)
                    {
                        // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                        // GetDataTable_Numeric_IgnoreException로 수정
                        //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                        dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                        foreach (DataRow dr in dtPara.Rows)
                        {
                            drItem = dtItem.NewRow();
                            drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                            drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                            drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                            drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                            dtItem.Rows.Add(drItem);
                        }
                    }

                    dtTemp.TableName = "PARA_ITEM";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtItem.TableName = "PARA_LIST";
                    dsData.Tables.Add(dtItem.Copy());
                }
                //===============================================================================================

                if (!string.Equals(strTestArea, "PARAMETRIC") && !string.Equals(strTestArea, "INLINE"))
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        public DataSet SelectWaferMapDrawData(
            long[] WaferSeq
            )
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtItem = null;
            DataRow drItem = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            StringBuilder sQuery = new StringBuilder();

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();
                TQP_PARASPEC oParaSepc = new TQP_PARASPEC();
                TQP_FOI_DIE oMapFoi = new TQP_FOI_DIE();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                dtTemp = dtTemp.DefaultView.ToTable(true, "TESTAREA", "DEVICE_ALIAS", "PROGRAM");
                if (dtTemp.Rows.Count > 1)
                    throw new Exception("서로 다른 Program 이 존재합니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ  = {0}.WAFER_SEQ ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ IN ('{1}') ", var.Value, string.Join("','", WaferSeq)));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM(+) ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                dtTemp = oProgram.GetData("GET_BIN_SUMMARY", new string[] { sQuery.ToString() }, new string[] { strProgram });
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "BINSUM";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "MAPDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================


                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");

                //dtTemp = oMapDef.GetMapDef(strProduct);
                //if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //{
                //    //Map Define 정보가 없을 경우 Logic 으로 임의로 만들어 준다. 
                //    dtTemp = dtMapDefine(iYmax, iXmax, iXmin, iYmin);
                //    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //        throw new Exception("Map Define 정보를 불러오지 못했습니다.");
                //}

                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //Parameter 관련 정보를 가져온다.
                //dtTemp = oParaSepc.GetData("SELECT_PARA_ITEM", null, new string[] { strProgram, WaferSeq.ToString() });
                dtTemp = oParaSepc.GetParameterItemByMultiWafers(strProgram, WaferSeq);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    List<string> lsItem = new List<string>();
                    DataTable dtPara;

                    dtItem = new DataTable();
                    dtItem.Columns.Add(new DataColumn("PARAM_NAME", typeof(string)));
                    dtItem.Columns.Add(new DataColumn("MAX", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("MIN", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("AVG", typeof(double)));
                    dtItem.AcceptChanges();

                    // TQP_PARASPEC 테이블에 존재하지 않는 Parameter 항목 추가
                    lsItem.Add(String.Format("A.{0}", DIENUM)); 
                    for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                    {
                        lsItem.Add(dtTemp.Rows[ir]["PARAM_NAME"].ToString());

                        //500 개를 기준으로 여러번 Para를 확인 한다.
                        if (ir != 0 && (int)(ir % 500) == (int)0)
                        {
                            lsItem.Sort();

                            // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                            // GetDataTable_Numeric_IgnoreException로 수정
                            //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                            dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                            foreach (DataRow dr in dtPara.Rows)
                            {
                                drItem = dtItem.NewRow();
                                drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                                drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                                drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                                drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                                dtItem.Rows.Add(drItem);
                            }

                            lsItem.Clear();
                        }

                    }

                    //남은  Para 확인
                    if (lsItem.Count > 0)
                    {
                        // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                        // GetDataTable_Numeric_IgnoreException로 수정
                        //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                        dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                        foreach (DataRow dr in dtPara.Rows)
                        {
                            drItem = dtItem.NewRow();
                            drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                            drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                            drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                            drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                            dtItem.Rows.Add(drItem);
                        }
                    }

                    dtTemp.TableName = "PARA_ITEM";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtItem.TableName = "PARA_LIST";
                    dsData.Tables.Add(dtItem.Copy());
                }
                //===============================================================================================

                if (!string.Equals(strTestArea, "PARAMETRIC") && !string.Equals(strTestArea, "INLINE"))
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        public DataSet GetCumMapReport(long[] WaferSeqs)
        {
            DataSet rDS = null;

            DataTable rDT_WaferInfo = null;
            DataTable rDT_MapDef = null;
            DataTable rDT_BinSum = null;
            DataTable dtTemp = null;

            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strMapID = string.Empty;
            string strTestArea = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_PRODUCT oProduct = new TQP_PRODUCT();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                rDS = new DataSet();


                /// Wafer Infomation================================================================
                rDT_WaferInfo = oWafer.GetWaferLotInfo(WaferSeqs);
                rDT_WaferInfo.TableName = "WAFER_INFO";

                if (rDT_WaferInfo != null && rDT_WaferInfo.Rows.Count > 0)
                {
                    strTestArea = rDT_WaferInfo.Rows[0]["TESTAREA"].ToString();
                    //strProduct = rDT_WaferInfo.Rows[0]["PRODUCT"].ToString();
                    strProduct = rDT_WaferInfo.Rows[0]["DEVICE_ALIAS"].ToString();
                    strProgram = rDT_WaferInfo.Rows[0]["PROGRAM"].ToString();
                    strMapID = oProduct.GetMapID(strProduct);

                    /// BIN Description & Summary========================================================
                    rDT_BinSum = oProgram.GetBinDistribution(strProgram, WaferSeqs);

                    if (strTestArea == "AVI")
                        throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                    // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                    dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                    StringBuilder sQuery = new StringBuilder();
                    Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                    string[] strTable = new string[dtTemp.Rows.Count];

                    for (int it = 0; it < dtTemp.Rows.Count; it++)
                    {
                        strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                        dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    }

                    sQuery.AppendLine(string.Join(",", strTable));
                    sQuery.AppendLine(" WHERE 1 = 1 ");

                    int ic = 0;
                    string[] arrTemp = new string[2];
                    foreach (KeyValuePair<string, string> var in dicAlias)
                    {
                        if (ic == 0)
                        {
                            sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeqs[0]));
                        }
                        else
                        {
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                        }
                        arrTemp[0] = var.Key;
                        arrTemp[1] = var.Value;
                        ic++;
                    }

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");

                    dtTemp.TableName = "MAPDATA";

                    //===============================================================================================


                    int iXmax, iYmax, iXmin, iYmin = 0;

                    iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                    iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                    iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                    iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                    //Map 에 대한 Recipe 정보를 불러 온다.

                    dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("Map Define 정보를 불러오지 못했습니다.");

                    rDT_MapDef = dtTemp.Copy();

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>==>>>>

                    //rDT_MapDef = oMapDef.GetMapDef(strMapID);
                }
                else
                {
                    return null;
                }

                rDT_BinSum.TableName = "BINDESC";
                rDT_MapDef.TableName = "RECIPE";

                rDS.Tables.Add(rDT_WaferInfo.Copy());
                rDS.Tables.Add(rDT_BinSum.Copy());
                rDS.Tables.Add(rDT_MapDef.Copy());

                return rDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCumWaferReport(long[] WaferSeqs)
        {
            DataSet dsData = null;

            DataTable dtTemp = null;
            DataTable dtGroup = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;

            try
            {
                dsData = new DataSet();

                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_BINDESC oBin = new TQP_BINDESC();
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();

                string[] strWaferSeqs = new string[WaferSeqs.Length];
                for (int i = 0; i < WaferSeqs.Length; i++)
                {
                    strWaferSeqs[i] = WaferSeqs[i].ToString();
                }

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetData("SELECT_WAFER_CUM_REPORT", new string[] { string.Join(",", strWaferSeqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                dtGroup = dtTemp.DefaultView.ToTable(true, "PROGRAM");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 Program 이 있습니다. 확인 바랍니다.");

                dtGroup = dtTemp.DefaultView.ToTable(true, "TESTAREA");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 TESTAREA 가 있습니다. 확인 바랍니다.");

                dtGroup = dtTemp.DefaultView.ToTable(true, "DEVICE_ALIAS");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 DEVICE ALIAS 가 있습니다. 확인 바랍니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                //===============================================================================================

                //Program 별 bin 정보를 불러 온다.
                dtTemp = oBin.GetData("SELECT_BIN_DESC", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    dtTemp = oBin.GetData("SELECT_BIN_DESC_ALL", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception(string.Format("Bin에 대한 정보가 없습니다. Program : {0}", strProgram));
                }

                dtTemp.TableName = "BIN_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //if (strTestArea == "AVI")
                //    throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ IN ({1}) ", var.Value, string.Join(",", strWaferSeqs)));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("TEST Map Data를 불러오지 못했습니다.");

                dtTemp.TableName = "MAPDATA";
                dsData.Tables.Add(dtTemp.Copy());
                //===============================================================================================

                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");

                dtTemp.TableName = "RECIPE"; 
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea != "PARAMETRIC" && strTestArea != "INLINE")
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            finally
            {
                if (dsData != null)
                    dsData.Dispose();
            }
        }


        public System.Data.DataTable SelectWaferParaItem(long WaferSeq, string strParaItem, int decimalLength)
        {
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                if (strTestArea == "AVI")
                    throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM(+) ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                string selectedCondition = string.Empty;
                if (String.Equals(strParaItem, "DIE_NUM"))
                    selectedCondition = "A.DIE_NUM";
                else selectedCondition = strParaItem;

                dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { string.Format(", ROUND({0}, {1}) AS {2}", selectedCondition, decimalLength, strParaItem), sQuery.ToString() }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("TEST Map Data를 불러오지 못했습니다.");

                dtTemp.TableName = "MAPDATA";
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        public System.Data.DataTable SelectWaferParaItem(long[] WaferSeq, string strParaItem, int decimalLength)
        {
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ IN ('{1}') ", var.Value, string.Join("','", WaferSeq)));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM(+) ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                string selectedCondition = string.Empty;
                if (String.Equals(strParaItem, "DIE_NUM"))
                    selectedCondition = "A.DIE_NUM";
                else selectedCondition = strParaItem;

                dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { string.Format(", ROUND({0}, {1}) AS {2}", selectedCondition, decimalLength, strParaItem), sQuery.ToString() }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("TEST Map Data를 불러오지 못했습니다.");

                dtTemp.TableName = "MAPDATA";
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        /// <summary>
        /// 정의된 Map Define 이 없을 경우 정의 해준다.
        /// </summary>
        /// <param name="iYmax"></param>
        /// <param name="iXmax"></param>
        /// <param name="iXmin"></param>
        /// <param name="iYmin"></param>
        /// <param name="iAngle"></param>
        /// <returns></returns>
        public DataTable dtMapDefine(string strDevice, string strTESTAREA, int iYmax, int iXmax, int iXmin, int iYmin, int iAngle = 0)
        {
            DataTable dtTable = null;
            DataTable dtTemp = null;

            double OriginX = 0;
            double OriginY = 0;

            double OriginIndexX = 0;
            double OriginIndexY = 0;

            double edge = 3d;
            double margin = 0.95D;
            double wsize = 200000d;

            int nOffsetXIdx = 2;
            int nOffsetYIdx = 2;

            int iXDIES = 0;
            int iYDIES = 0;

            DACrux.TEST.DSL.TQP_MAPDEF oMapdef = null;

            try
            {
                dtTable = new DataTable();

                dtTable.Columns.Add(new DataColumn("WAFER_SIZE", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("CHIP_SIZE_X", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("CHIP_SIZE_Y", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("ORIGIN_MICRO_X", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("ORIGIN_MICRO_Y", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("ORIGIN_INDEX_X", typeof(int)));
                dtTable.Columns.Add(new DataColumn("ORIGIN_INDEX_Y", typeof(int)));
                dtTable.Columns.Add(new DataColumn("FIRST_MICRO_X", typeof(int)));
                dtTable.Columns.Add(new DataColumn("FIRST_MICRO_Y", typeof(int)));
                dtTable.Columns.Add(new DataColumn("FIRST_INDEX_X", typeof(int)));
                dtTable.Columns.Add(new DataColumn("FIRST_INDEX_Y", typeof(int)));
                dtTable.Columns.Add(new DataColumn("EDGE_SIZE", typeof(decimal)));
                dtTable.Columns.Add(new DataColumn("ANGLE", typeof(int)));
                //dtTable.Columns.Add(new DataColumn("NETDIE", typeof(int)));
                dtTable.Columns.Add(new DataColumn("NOTCH_TYPE", typeof(int)));
                dtTable.Columns.Add(new DataColumn("DIE_INDEX_MIN_X", typeof(int)));
                dtTable.Columns.Add(new DataColumn("DIE_INDEX_MAX_X", typeof(int)));
                dtTable.Columns.Add(new DataColumn("DIE_INDEX_MIN_Y", typeof(int)));
                dtTable.Columns.Add(new DataColumn("DIE_INDEX_MAX_Y", typeof(int)));
                dtTable.Columns.Add(new DataColumn("XY_DIRECTION", typeof(int)));
                dtTable.Columns.Add(new DataColumn("REFERENCEDIE_SETTING", typeof(int)));
                dtTable.AcceptChanges();

                oMapdef = new DACrux.TEST.DSL.TQP_MAPDEF();
                dtTemp = oMapdef.GetData("GET_MAPDEF_MAP", null, new string[] { strDevice });
                if (dtTemp != null && dtTemp.Rows.Count > 0 && strTESTAREA != "PARAMETRIC" && strTESTAREA != "INLINE")
                {
                    dtTable.Rows.Add(new object[] { 
                            dtTemp.Rows[0]["WAFER_SIZE"],                     // WAFER_SIZE
                            //((decimal)dtTemp.Rows[0]["CHIP_SIZE_X"] / 1000),  //CHIP_SIZE_X
                            //((decimal)dtTemp.Rows[0]["CHIP_SIZE_Y"] / 1000),  //CHIP_SIZE_Y
                            ((decimal)dtTemp.Rows[0]["CHIP_SIZE_X"]),  //CHIP_SIZE_X
                            ((decimal)dtTemp.Rows[0]["CHIP_SIZE_Y"]),  //CHIP_SIZE_Y
                            dtTemp.Rows[0]["ORIGIN_MICRO_X"],                 //ORIGIN_MICRO_X
                            dtTemp.Rows[0]["ORIGIN_MICRO_Y"],                 //ORIGIN_MICRO_Y
                            ((decimal)dtTemp.Rows[0]["ORIGIN_INDEX_X"]),  //ORIGIN_INDEX_X
                            ((decimal)dtTemp.Rows[0]["ORIGIN_INDEX_Y"]),  //ORIGIN_INDEX_Y
                            dtTemp.Rows[0]["FIRST_MICRO_X"],                  //FIRST_MICRO_X
                            dtTemp.Rows[0]["FIRST_MICRO_Y"],                  //FIRST_MICRO_Y
                            dtTemp.Rows[0]["FIRST_INDEX_X"],                  //FIRST_INDEX_X
                            dtTemp.Rows[0]["FIRST_INDEX_Y"],                  //FIRST_INDEX_Y
                            //((decimal)dtTemp.Rows[0]["EDGE_SIZE"] / 10),      //EDGE_SIZE
                            ((decimal)dtTemp.Rows[0]["EDGE_SIZE"]),      //EDGE_SIZE
                            0,                                                //ANGLE
                            dtTemp.Rows[0]["NOTCH_TYPE"],                     //NOTCH_TYPE
                            dtTemp.Rows[0]["DIE_INDEX_MIN_X"],                //DIE_INDEX_MIN_X
                            dtTemp.Rows[0]["DIE_INDEX_MAX_X"],                //DIE_INDEX_MAX_X
                            dtTemp.Rows[0]["DIE_INDEX_MIN_Y"],                //DIE_INDEX_MIN_Y
                            dtTemp.Rows[0]["DIE_INDEX_MAX_Y"],                //DIE_INDEX_MAX_Y
                            1,                                                //XY_DIRECTION //LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3
                            0                                                 //REFERENCEDIE_SETTING
                            });

                    dtTable.AcceptChanges();
                    return dtTable;
                }

                iXDIES = iXmax - iXmin + 1;
                iYDIES = iYmax - iYmin + 1;

                double dDieSizeX = wsize / (iXDIES + nOffsetXIdx) * margin;
                double dDieSizeY = wsize / (iYDIES + nOffsetYIdx) * margin;


                OriginIndexX = iXmin + (int)Math.Floor((iXmax - iXmin + 1) / 2.0d);  //(-dDieSizeX * 0.5 * (xcnt % 2));
                OriginIndexY = iYmin + (int)Math.Floor((iYmax - iYmin + 1) / 2.0d);  // (-dDieSizeY * 0.5 * (ycnt % 2));

                if (iXDIES < 20)
                {
                    if (iXDIES % 2 == 0)
                        OriginX = 0;
                    else
                        OriginX = dDieSizeX / 2.0d;
                }
                else
                {
                    if (iAngle == 90)
                        OriginX = dDieSizeX / 2.0d;
                    else if (iAngle == 270)
                        OriginX = -dDieSizeX / 2.0d;
                    else
                        OriginX = 0;
                }

                if (iYDIES < 20)
                {
                    if (iYDIES % 2 == 0)
                        OriginY = 0;
                    else
                        OriginY = dDieSizeY / 2.0d;
                }
                else
                {
                    if (iAngle == 90 || iAngle == 270)
                        OriginY = 0;
                    else if (iAngle == 0)
                        OriginY = -dDieSizeY / 2.0d;
                    else if (iAngle == 180)
                        OriginY = dDieSizeY / 2.0d;
                }

                dtTable.Rows.Add(new object[] { 
                wsize,                             // WAFER_SIZE
                dDieSizeX,                         //CHIP_SIZE_X
                dDieSizeY,                         //CHIP_SIZE_Y
                OriginX,                           //ORIGIN_MICRO_X
                OriginY,                           //ORIGIN_MICRO_Y
                OriginIndexX,                      //ORIGIN_INDEX_X
                OriginIndexY,                      //ORIGIN_INDEX_Y
                dDieSizeX,                         //FIRST_MICRO_X
                dDieSizeY,                         //FIRST_MICRO_Y
                iXmin,                             //FIRST_INDEX_X
                iYmin,                             //FIRST_INDEX_Y
                edge,                              //EDGE_SIZE
                0,                                 //ANGLE
                0,                                 //NOTCH_TYPE
                iXmin,                             //DIE_INDEX_MIN_X
                iXmax,                             //DIE_INDEX_MAX_X
                iYmin,                             //DIE_INDEX_MIN_Y
                iYmax,                             //DIE_INDEX_MAX_Y
                1,                                 //XY_DIRECTION //LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3
                0                                  //REFERENCEDIE_SETTING
                });

                dtTable.AcceptChanges();
                return dtTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// AVI Wafer Map 을 그리기 위한 기존 정보
        /// </summary>
        /// <param name="WaferSeq"></param>
        /// <returns></returns>
        public System.Data.DataSet SelectWaferMapBasic(long WaferSeq)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_BINDESC oBin = new TQP_BINDESC();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_FOI_DIE oMapFoi = new TQP_FOI_DIE();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //AVI 와 이외의 공정의 Data Map 을 가져오는 방식이 다르다.
                if (strTestArea == "AVI")
                {
                    dtTemp = oMapFoi.SelectFOIMap(WaferSeq.ToString(), "0");
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");
                }
                else
                {
                    // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                    dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                    StringBuilder sQuery = new StringBuilder();
                    Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                    string[] strTable = new string[dtTemp.Rows.Count];

                    for (int it = 0; it < dtTemp.Rows.Count; it++)
                    {
                        strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                        dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    }

                    sQuery.AppendLine(string.Join(",", strTable));
                    sQuery.AppendLine(" WHERE 1 = 1 ");

                    int ic = 0;
                    string[] arrTemp = new string[2];
                    foreach (KeyValuePair<string, string> var in dicAlias)
                    {
                        if (ic == 0)
                        {
                            sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                        }
                        else
                        {
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                        }
                        arrTemp[0] = var.Key;
                        arrTemp[1] = var.Value;
                        ic++;
                    }

                    //===============================================================================================

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");
                }

                dtTemp.TableName = "MAPDATA";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================
                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");


                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { strProgram });
                dtTemp.TableName = "MASTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //변경 할 Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { "SCOPE" });
                dtTemp.TableName = "ALTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea != "PARAMETRIC" && strTestArea != "INLINE")
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        public System.Data.DataSet SelectWaferMapConfigMap(long WaferSeq)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtMapConfig = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strMapConfigSeq = string.Empty;
            string strDir = string.Empty;
            string strExtention = string.Empty;
            bool bPST = false;

            int rotate = -1;
            int shiftX = -1;
            int shiftY = -1;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_MAPCFG oMapCfg = new TQP_MAPCFG();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();
                strMapConfigSeq = dtTemp.Rows[0]["MAP_CFG_SEQ"].ToString();
                strExtention = dtTemp.Rows[0]["EXTENTION"].ToString();

                if(strExtention == ".PST" || strExtention == ".CP4" || strExtention == ".CP3")
                    bPST = true;
                else
                    bPST = false;
                
                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //AVI 와 이외의 공정의 Data Map 을 가져오는 방식이 다르다.
                if (strTestArea == "AVI" || strTestArea == "PARAMETRIC")
                {
                    throw new Exception("해당 Test Area는 Map Control을 사용 할 수 없습니다.");
                }
                else
                {
                    // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                    dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                    StringBuilder sQuery = new StringBuilder();
                    Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                    string[] strTable = new string[dtTemp.Rows.Count];

                    for (int it = 0; it < dtTemp.Rows.Count; it++)
                    {
                        strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                        dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    }

                    sQuery.AppendLine(string.Join(",", strTable));
                    sQuery.AppendLine(" WHERE 1 = 1 ");

                    int ic = 0;
                    string[] arrTemp = new string[2];
                    foreach (KeyValuePair<string, string> var in dicAlias)
                    {
                        if (ic == 0)
                        {
                            sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                        }
                        else
                        {
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                        }
                        arrTemp[0] = var.Key;
                        arrTemp[1] = var.Value;
                        ic++;
                    }

                    //===============================================================================================

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");
                }

                dtTemp.TableName = "MAPDATA";

                //원본 Map 으로 변경하여 뿌려 준다.
                dtMapConfig = oMapCfg.GetData("SELECT_MAPCFG_SEQ", null, new string[] { strMapConfigSeq });
                if (dtMapConfig == null || dtMapConfig.Rows.Count <= 0)
                {
                    strDir = "LL";
                    rotate = 0;
                    shiftX = 0;
                    shiftY = 0;

                    //PST 형식만 적용
                    if (bPST)
                    {
                        //Config 없이 들어온 경우 
                        int iChar = 0;
                        //Program 첫글자의 숫자 여부에 따라 Default shift
                        if (int.TryParse(strProgram[0].ToString(), out iChar) == true)
                        {
                            strDir = "TL";
                            shiftX = 1;
                        }
                        else
                        {
                            shiftX = 1;
                            shiftY = 1;
                        }
                    }
                }
                else
                {
                    strDir = dtMapConfig.Rows[0]["ALTER_DIRECTION"].ToString();
                    Int32.TryParse(dtMapConfig.Rows[0]["ALTER_ANGLE"].ToString(), out rotate);
                    Int32.TryParse(dtMapConfig.Rows[0]["ALTER_INDEX_X"].ToString(), out shiftX);
                    Int32.TryParse(dtMapConfig.Rows[0]["ALTER_INDEX_Y"].ToString(), out shiftY);
                    
                    rotate = (360 - (rotate - 0)) % 360;
                }

                int xmax = int.Parse(dtTemp.Compute("MAX([X])", "1=1").ToString());
                int ymax = int.Parse(dtTemp.Compute("MAX([Y])", "1=1").ToString());
                int xmin = int.Parse(dtTemp.Compute("MIN([X])", "1=1").ToString());
                int ymin = int.Parse(dtTemp.Compute("MIN([Y])", "1=1").ToString());

                for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                {
                    //Shift 한다.
                    int IndexX = Int32.Parse(dtTemp.Rows[ir]["X"].ToString());
                    int IndexY = Int32.Parse(dtTemp.Rows[ir]["Y"].ToString());

                    //1)회전
                    Rotate(rotate, xmin, xmax, ymin, ymax, ref IndexX, ref IndexY);

                    //2) Swap
                    switch (strDir)
                    {
                        case "LL":
                            //IndexX = IndexX;
                            //IndexY = IndexY;
                            break;
                        case "TL":
                            if (rotate == 270 || rotate == 90)
                            {
                                //IndexX = IndexX;
                                IndexY = (xmax + xmin - IndexY);
                            }
                            else
                            {
                                //IndexX = IndexX;
                                IndexY = (ymax + ymin - IndexY);
                            }
                            break;
                        case "TR":
                            if (rotate == 270 || rotate == 90)
                            {
                                IndexX = (ymax + ymin - IndexX);
                                IndexY = (xmax + xmin - IndexY);
                            }
                            else
                            {
                                IndexX = (xmax + xmin - IndexX);
                                IndexY = (ymax + ymin - IndexY);
                            }
                            break;
                        case "LR":
                            if (rotate == 270 || rotate == 90)
                            {
                                IndexX = (ymax + ymin - IndexX);
                                //IndexY = IndexY;
                            }
                            else
                            {
                                IndexX = (xmax + xmin - IndexX);
                                //IndexY = IndexY;
                            }
                            break;
                    }

                    // 3)시프트
                    if (rotate == 270 || rotate == 90)
                    {
                        IndexX -= shiftY;
                        IndexY -= shiftX;
                    }
                    else
                    {
                        IndexX -= shiftX;
                        IndexY -= shiftY;
                    }

                    dtTemp.Rows[ir]["X"] = IndexX;
                    dtTemp.Rows[ir]["Y"] = IndexY;
                }

                dtTemp.AcceptChanges();

                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================
                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");


                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea != "PARAMETRIC" && strTestArea != "INLINE")
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }


        /// <summary>
        ///  AVI Shot 기준 Wafer Map 을 그리기 위한 기존 정보
        /// </summary>
        /// <param name="strDevice"></param>
        /// <returns></returns>
        public System.Data.DataSet SelectWaferShotMapBasic(string strDevice)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strTestArea = "AVI";
            string strProduct = strDevice;
            string strProgram = "SCOPE";

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_BINDESC oBin = new TQP_BINDESC();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_FOI_DIE oMapFoi = new TQP_FOI_DIE();

                dsData = new DataSet();

                //===============================================================================================

                dtTemp = oMapDef.GetMapData(strDevice);


                dtTemp.TableName = "MAPDATA";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================
                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");


                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { strProgram });
                dtTemp.TableName = "MASTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //변경 할 Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { "SCOPE" });
                dtTemp.TableName = "ALTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                dtTemp.TableName = "SHOT_DEF";
                dsData.Tables.Add(dtTemp.Copy());

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        public System.Data.DataSet SelectWaferMapConfig(long WaferSeq)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;

            int iAngle = 0;
            int iXoffset = 0;
            int iYoffset = 0;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_BINDESC oBin = new TQP_BINDESC();
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_MAPCFG oMapConfig = new TQP_MAPCFG();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea == "AVI")
                    throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                //===============================================================================================
                // Wafer Map Offet 관련 정보 

                dtTemp = oMapConfig.GetData("SELECT_MAPCFG", null, new string[] { strTestArea, strProduct, strProgram });
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "MAPCFG";
                    dsData.Tables.Add(dtTemp.Copy());

                    if (int.TryParse(dtTemp.Rows[0]["ALTER_ANGLE"].ToString(), out iAngle) == false)
                        iAngle = 0;

                    if (int.TryParse(dtTemp.Rows[0]["ALTER_INDEX_X"].ToString(), out iXoffset) == false)
                        iXoffset = 0;

                    if (int.TryParse(dtTemp.Rows[0]["ALTER_INDEX_Y"].ToString(), out iYoffset) == false)
                        iYoffset = 0;

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA_CFG", new string[] { "", sQuery.ToString() }, new string[] { iAngle.ToString(), iXoffset.ToString(), iYoffset.ToString() });
                }
                else
                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);

                //===============================================================================================


                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("TEST Map Data를 불러오지 못했습니다.");

                dtTemp.TableName = "MAPDATA";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================
                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");


                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { strProgram });
                dtTemp.TableName = "MASTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea != "PARAMETRIC" && strTestArea != "INLINE")
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                dsData.AcceptChanges();

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }


        public System.Data.DataTable SelectWaferShotMap(string strMapID)
        {
            TQP_MAPDEF oMapDef = new TQP_MAPDEF();
            return oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strMapID });
        }

        public void UpdateMapConfig(string strTestArea, string strProduct, string strProgram, string strDir, string strAngle, string strIndexX, string strIndexY, string strUser, string strcomment)
        {
            DataTable dtTemp = null;
            int iRev = 0;
            try
            {
                TQP_MAPCFG oMapConfig = new TQP_MAPCFG();

                //Data가 있으면 생성 하고 없으면 Update 한다.
                dtTemp = oMapConfig.GetData("SELECT_MAPCFG_03", null, new string[] { strTestArea, strProduct, strProgram });
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp = dtTemp.Select("1 = 1", "REV_NO DESC").CopyToDataTable<DataRow>();

                    //마지막 Revision No 를 가져와 +1하여 생성한다.
                    iRev = int.Parse(dtTemp.Rows[0]["REV_NO"].ToString());
                    iRev++;

                    //기존 정의된 정보는 DB 상에서 Delete Flag Y로 Update 한다.
                    oMapConfig.UpdateData("DELETE_MAPCFG", null, new string[] { strUser, strTestArea, strProduct, strProgram });
                }

                oMapConfig.InsertData("CREATE_MATCFG", null, new string[] { iRev.ToString(), strTestArea, strProduct, strProgram, strDir, strAngle, strIndexX, strIndexY, strUser, strcomment });
            }
            finally
            {
            }
        }

        public DataTable SelectMapConfigList(string strProduct)
        {
            try
            {
                TQP_MAPCFG oMapConfig = new TQP_MAPCFG();
                return oMapConfig.GetData("SELECT_MAPCFG_02", null, new string[] { strProduct });
            }
            finally
            {
            }
        }

        public DataTable SelectMapConfigView(bool bDeleteFlag)
        {
            string strWhere = string.Empty;
            try
            {
                TQP_MAPCFG oMapConfig = new TQP_MAPCFG();

                if (bDeleteFlag)
                    strWhere = " AND  NVL(DELETE_FLAG, 'N') = 'N' ";

                return oMapConfig.GetData("SELECT_MAPCFG_VIEW", new string[] { strWhere }, null);
            }
            finally
            {
            }
        }

        public void DeleteMapConfig(string strSequence, string strUser, string strcomment)
        {
            try
            {
                TQP_MAPCFG oMapConfig = new TQP_MAPCFG();
                oMapConfig.UpdateData("DELETE_MAPCFG_SEQ", null, new string[] { strUser, strcomment, strSequence });
            }
            finally
            {
            }
        }

        public System.Data.DataSet SelectWaferParaData(long WaferSeq, string[] strParaItem)
        {
            DataTable dtTemp = null;
            DataSet ds = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            try
            {
                ds = new DataSet();
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                if (strTestArea == "AVI")
                    throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                //===============================================================================================

                dtTemp = oProgram.GetData("SELECT_ITEM_GROUP", new string[] { string.Join(",", strParaItem), sQuery.ToString(), string.Join(",", strParaItem) }, null);
                dtTemp.TableName = "BOX";
                ds.Tables.Add(dtTemp);

                dtTemp = oProgram.GetData("SELECT_ITEM_DETAIL", new string[] { string.Format(", {0}", string.Join(",", strParaItem)), sQuery.ToString() }, null);
                dtTemp.TableName = "MAPDATA";
                ds.Tables.Add(dtTemp);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (ds != null)
                    ds.Dispose();
            }
        }

        public System.Data.DataTable SelectWaferParaDataMulti(long[] WaferSeqList, string[] strParaItem)
        {
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();

                if (WaferSeqList == null || WaferSeqList.Length <= 0)
                    throw new Exception("Wafer List 정보가 없습니다.");

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeqList);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                if (strTestArea == "AVI")
                    throw new Exception("AVI 또는 Scope Map 은 해당 화면에서 사용할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                StringBuilder sQuery = new StringBuilder();
                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ IN ( {1} ) ", var.Value, string.Join(",", WaferSeqList)));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                    }
                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                dtTemp = oProgram.GetData("SELECT_MEASURE_RAW_DATA", new string[] { string.Format(", {0}", string.Join(",", strParaItem)), sQuery.ToString() }, null);
                dtTemp.TableName = "MAPDATA";

                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        public DataSet GetWaferBinReport(long[] WaferSeq, bool bWaferBase)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtGroup = null;

            string strProgram = string.Empty;

            try
            {
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                TQP_BINDESC oBin = new TQP_BINDESC();

                dsData = new DataSet();
                string[] strWaferSeqs = new string[WaferSeq.Length];
                for (int i = 0; i < WaferSeq.Length; i++)
                {
                    strWaferSeqs[i] = WaferSeq[i].ToString();
                }

                if (bWaferBase)
                    dtTemp = oWaferSum.GetData("SELECT_WAFER_BIN_WAFER_BASE", new string[] { string.Join(",", strWaferSeqs) }, null);
                else
                    dtTemp = oWaferSum.GetData("SELECT_WAFER_BIN_LOT_BASE", new string[] { string.Join(",", strWaferSeqs) }, null);

                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Info 정보가 없습니다.");

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                dtGroup = dtTemp.DefaultView.ToTable(true, "PROGRAM");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 Program 이 있습니다. 확인 바랍니다.");

                dtGroup = dtTemp.DefaultView.ToTable(true, "TESTAREA");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 TESTAREA 가 있습니다. 확인 바랍니다.");

                dtGroup = dtTemp.DefaultView.ToTable(true, "DEVICE_ALIAS");
                if (dtGroup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 DEVICE ALIAS 가 있습니다. 확인 바랍니다.");

                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();
                dtTemp = oBin.GetData("SELECT_BIN_DESC", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    dtTemp = oBin.GetData("SELECT_BIN_DESC_ALL", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception(string.Format("Bin에 대한 정보가 없습니다. Program : {0}", strProgram));
                }

                dtTemp.TableName = "BIN_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public System.Data.DataTable GetUserMapConfig(string strUserID, string strCategory = "")
        //{
        //    TQD_CONFIG_USER oUserConfig = null;

        //    try
        //    {
        //        oUserConfig = new TQD_CONFIG_USER();
        //        //=================================================================================================================================
        //        // 사용자별 Map User Config 정보를 가져 온다.
        //        //=================================================================================================================================
        //        return oUserConfig.GetData("SELECT_CONFIG_USER_03", null, new string[] { strUserID, strCategory });
        //    }
        //    finally
        //    {

        //    }
        //}

        public DataTable GetMapConfigWaferList(string strStartTime, string strEndTime, string strTestArea, string strDevice, string strProgram)
        {
            TQP_WAFER_SUM oTQP_WAFER_SUM = new TQP_WAFER_SUM();
            return oTQP_WAFER_SUM.GetMapConfigWaferList(strStartTime, strEndTime, strTestArea, strDevice, strProgram);
        }

        public void WaferMapConfigUpdate(string strWaferSeq, string strMapConfigSeq)
        {
            DataTable dtWaferInfo = null;
            DataTable dtWaferMap = null;
            DataTable dtTables = null;
            DataTable dtMapConfig = null;
            DataTable dtOldMapConfig = null;
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;
            string Dir = string.Empty;
            string OldDir = string.Empty;

            string[,] oMap = null;

            int iOldMapConfigSeq = -1;
            int rotate, shiftX, shiftY = -1;
            int OldRotate = -1, OldShiftX = -1, OldShiftY = -1;
            int xmin, xmax, ymin, ymax = -1;

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
                TQP_WAFER oWafer = new TQP_WAFER();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();
                TQP_MAPCFG oMapCfg = new TQP_MAPCFG();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtWaferInfo = oWaferSum.GetWaferLotInfo(long.Parse(strWaferSeq));
                if (dtWaferInfo == null || dtWaferInfo.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtWaferInfo.Rows[0]["TESTAREA"].ToString();
                strProduct = dtWaferInfo.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtWaferInfo.Rows[0]["PROGRAM"].ToString();

                dtMapConfig = oMapCfg.GetData("SELECT_MAPCFG_SEQ", null, new string[] { strMapConfigSeq });
                if (dtMapConfig == null || dtMapConfig.Rows.Count <= 0)
                    throw new Exception("Map Config 에 대한 정보가 없습니다.");

                Dir = dtMapConfig.Rows[0]["ALTER_DIRECTION"].ToString();
                Int32.TryParse(dtMapConfig.Rows[0]["ALTER_ANGLE"].ToString(), out rotate);
                Int32.TryParse(dtMapConfig.Rows[0]["ALTER_INDEX_X"].ToString(), out shiftX);
                Int32.TryParse(dtMapConfig.Rows[0]["ALTER_INDEX_Y"].ToString(), out shiftY);


                //현재 Wafer 와 Setup 하려는 Wafer 간의 Shift 정보를 도출 한다.
                //없으면 설정하려는 Shift 정보로 그대로 설정
                if (int.TryParse(dtWaferInfo.Rows[0]["MAP_CFG_SEQ"].ToString(), out iOldMapConfigSeq) == true)
                {
                    dtOldMapConfig = oMapCfg.GetData("SELECT_MAPCFG_SEQ", null, new string[] { iOldMapConfigSeq.ToString() });
                    if (dtOldMapConfig == null || dtOldMapConfig.Rows.Count <= 0)
                        throw new Exception("현재 Wafer의 Map Config 에 대한 정보가 없습니다.");

                    if (strMapConfigSeq == iOldMapConfigSeq.ToString())
                        throw new Exception("동일한 Seq 로 Update 할 수 없습니다.");

                    OldDir = dtOldMapConfig.Rows[0]["ALTER_DIRECTION"].ToString();

                    if (Int32.TryParse(dtOldMapConfig.Rows[0]["ALTER_ANGLE"].ToString(), out OldRotate) == false)
                        OldRotate = -1;

                    //RotationAngle = (360 - (현재 Angle - 회전 Angle)) % 360)
                    OldRotate = (360 - (OldRotate - 0)) % 360;

                    if (Int32.TryParse(dtOldMapConfig.Rows[0]["ALTER_INDEX_X"].ToString(), out OldShiftX) == false)
                        OldShiftX = -1;

                    if (Int32.TryParse(dtOldMapConfig.Rows[0]["ALTER_INDEX_Y"].ToString(), out OldShiftY) == false)
                        OldShiftY = -1;
                }
                else
                {
                    //Config 없이 들어온 경우 
                    int iChar = 0;

                    OldDir = "LL";
                    OldRotate = 0;
                    OldShiftX = 0;
                    OldShiftY = 0;
                    //Program 첫글자의 숫자 여부에 따라 Default shift
                    if (int.TryParse(strProgram[0].ToString(), out iChar) == true)
                    {
                        OldDir = "TL";
                        OldShiftX = 1;
                    }
                    else
                    {
                        OldShiftX = 1;
                        OldShiftY = 1;
                    }
                }

                // X, Y 값이 있는 Table 을 가져 온다.
                dtTables = oProgram.GetData("SELECT_MAP_INDEX_TABLE", null, new string[] { strProgram });
                if (dtTables == null || dtTables.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Table 정보가 없습니다.");

                //해당 Table 의 정보를 기준으로 Data Update 한다.
                foreach (DataRow drTable in dtTables.Rows)
                {
                    dtWaferMap = oProgram.GetData("SELECT_MAP_RAW_DATA", new string[] { drTable["TABLE_NAME"].ToString() }, new string[] { strWaferSeq });

                    if (dtWaferMap == null || dtWaferMap.Rows.Count <= 0)
                        continue;

                    xmax = int.Parse(dtWaferMap.Compute("MAX([X])", "1=1").ToString());
                    ymax = int.Parse(dtWaferMap.Compute("MAX([Y])", "1=1").ToString());
                    xmin = int.Parse(dtWaferMap.Compute("MIN([X])", "1=1").ToString());
                    ymin = int.Parse(dtWaferMap.Compute("MIN([Y])", "1=1").ToString());

                    //Map Config Map 정보가 있을 경우 기존 Map 으로 원복 후 Shift 한다. 
                    //shift 이력이 있을 경우 이전 Index 정보로 원복 시키고 shift 한다.
                    if (!string.IsNullOrEmpty(OldDir) && OldRotate != -1)
                    {
                        oMap = new string[dtWaferMap.Rows.Count, Enum.GetNames(typeof(MapUpdate)).Length];

                        for (int ir = 0; ir < dtWaferMap.Rows.Count; ir++)
                        {
                            oMap[ir, (int)MapUpdate.WAFER_SEQ] = dtWaferMap.Rows[ir]["WAFER_SEQ"].ToString();
                            oMap[ir, (int)MapUpdate.DIE_NUM] = dtWaferMap.Rows[ir]["DIE_NUM"].ToString();

                            //Shift 한다.
                            int IndexX = Int32.Parse(dtWaferMap.Rows[ir]["X"].ToString());
                            int IndexY = Int32.Parse(dtWaferMap.Rows[ir]["Y"].ToString());

                            //1)회전
                            Rotate(OldRotate, xmin, xmax, ymin, ymax, ref IndexX, ref IndexY);

                            //2) Swap
                            switch (OldDir)
                            {
                                case "LL":
                                    //IndexX = IndexX;
                                    //IndexY = IndexY;
                                    break;
                                case "TL":
                                    if (OldRotate == 270 || OldRotate == 90)
                                    {
                                        //IndexX = IndexX;
                                        IndexY = (xmax + xmin - IndexY);
                                    }
                                    else
                                    {
                                        //IndexX = IndexX;
                                        IndexY = (ymax + ymin - IndexY);
                                    }
                                    break;
                                case "TR":
                                    if (OldRotate == 270 || OldRotate == 90)
                                    {
                                        IndexX = (ymax + ymin - IndexX);
                                        IndexY = (xmax + xmin - IndexY);
                                    }
                                    else
                                    {
                                        IndexX = (xmax + xmin - IndexX);
                                        IndexY = (ymax + ymin - IndexY);
                                    }
                                    break;
                                case "LR":
                                    if (OldRotate == 270 || OldRotate == 90)
                                    {
                                        IndexX = (ymax + ymin - IndexX);
                                        //IndexY = IndexY;
                                    }
                                    else
                                    {
                                        IndexX = (xmax + xmin - IndexX);
                                        //IndexY = IndexY;
                                    }
                                    break;
                            }

                            // 3)시프트
                            if (OldRotate == 270 || OldRotate == 90)
                            {
                                IndexX -= OldShiftY;
                                IndexY -= OldShiftX;
                            }
                            else
                            {
                                IndexX -= OldShiftX;
                                IndexY -= OldShiftY;
                            }

                            oMap[ir, (int)MapUpdate.IndexX] = IndexX.ToString();
                            oMap[ir, (int)MapUpdate.IndexY] = IndexY.ToString();
                        }

                        //Bulk Update 진행
                        if (oMap != null && oMap.Length > 0)
                        {
                            oProgram.MapDataIndexUpdate(drTable["TABLE_NAME"].ToString(), oMap);
                        }

                        //Update 후 다시 Shift 하기위해 DB Select 한다.
                        dtWaferMap = oProgram.GetData("SELECT_MAP_RAW_DATA", new string[] { drTable["TABLE_NAME"].ToString() }, new string[] { strWaferSeq });

                        if (dtWaferMap == null || dtWaferMap.Rows.Count <= 0)
                            continue;

                        xmax = int.Parse(dtWaferMap.Compute("MAX([X])", "1=1").ToString());
                        ymax = int.Parse(dtWaferMap.Compute("MAX([Y])", "1=1").ToString());
                        xmin = int.Parse(dtWaferMap.Compute("MIN([X])", "1=1").ToString());
                        ymin = int.Parse(dtWaferMap.Compute("MIN([Y])", "1=1").ToString());

                    }

                    oMap = new string[dtWaferMap.Rows.Count, Enum.GetNames(typeof(MapUpdate)).Length];
                    for (int ir = 0; ir < dtWaferMap.Rows.Count; ir++)
                    {
                        oMap[ir, (int)MapUpdate.WAFER_SEQ] = dtWaferMap.Rows[ir]["WAFER_SEQ"].ToString();
                        oMap[ir, (int)MapUpdate.DIE_NUM] = dtWaferMap.Rows[ir]["DIE_NUM"].ToString();

                        //Shift 한다.
                        int IndexX = Int32.Parse(dtWaferMap.Rows[ir]["X"].ToString());
                        int IndexY = Int32.Parse(dtWaferMap.Rows[ir]["Y"].ToString());

                        //1) Swap
                        switch (Dir)
                        {
                            case "LL":
                                //IndexX = IndexX;
                                //IndexY = IndexY;
                                break;
                            case "TL":
                                //IndexX = IndexX;
                                IndexY = (ymax + ymin - IndexY);
                                break;
                            case "TR":
                                IndexX = (xmax + xmin - IndexX);
                                IndexY = (ymax + ymin - IndexY);
                                break;
                            case "LR":
                                IndexX = (xmax + xmin - IndexX);
                                //IndexY = IndexY;
                                break;
                        }

                        // 2)회전
                        Rotate(rotate, xmin, xmax, ymin, ymax, ref IndexX, ref IndexY);

                        // 3)시프트
                        IndexX += shiftX;
                        IndexY += shiftY;


                        oMap[ir, (int)MapUpdate.IndexX] = IndexX.ToString();
                        oMap[ir, (int)MapUpdate.IndexY] = IndexY.ToString();

                    }

                    //Bulk Update 진행
                    if (oMap != null && oMap.Length > 0)
                    {
                        oProgram.MapDataIndexUpdate(drTable["TABLE_NAME"].ToString(), oMap);
                    }
                }

                oWaferSum.UpdateData("UPDATE_MAP_CFG", null, new string[] { strMapConfigSeq, strWaferSeq });
                oWafer.UpdateData("UPDATE_MAP_CFG", null, new string[] { strMapConfigSeq, strWaferSeq });
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtWaferInfo != null)
                    dtWaferInfo.Dispose();

                if (dtWaferMap != null)
                    dtWaferMap.Dispose();

                if (dtTables != null)
                    dtTables.Dispose();

                if (dtMapConfig != null)
                    dtMapConfig.Dispose();

                if (dtOldMapConfig != null)
                    dtOldMapConfig.Dispose();

                if (dtTemp != null)
                    dtTemp.Dispose();

                oMap = null;
            }



        }

        /// <summary>
        /// Wafer를 시계방향으로 회전합니다.
        /// </summary>
        /// <param name="clockwiseAngle">시계방향회전각도 : 0,90,180,270</param>
        public void Rotate(int clockwiseAngle, int xmin, int xmax, int ymin, int ymax, ref int x, ref int y)
        {
            if (clockwiseAngle == 0)
                return;

            while (clockwiseAngle < 0)
                clockwiseAngle += 360;

            int nx, ny;

            // 계산횟수를 줄이기 위해 각도별로 계산식 구성
            if (clockwiseAngle == 180)
            {
                nx = xmax + xmin - x;
                ny = ymax + ymin - y;
            }
            else if (clockwiseAngle == 90)
            {
                nx = y;
                ny = xmax + xmin - x;
            }
            else if (clockwiseAngle == 270)
            {
                nx = ymax + ymin - y;
                ny = x;
            }
            else
            {
                throw new Exception(String.Format("처리할 수 없는 각도({0}) 입니다.", clockwiseAngle));
            }

            x = nx;
            y = ny;

        }


        public System.Data.DataSet SelectParaReportData(long[] WaferSeqList)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtItem = null;
            DataRow drItem = null;
            DataTable dtWaferList = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;
            string strColums = string.Empty;

            StringBuilder sQuery = new StringBuilder();

            try
            {
                TQP_PROGRAM oProgram = new TQP_PROGRAM();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_DATA_TABLES oTables = new TQP_DATA_TABLES();
                TQP_PARASPEC oParaSepc = new TQP_PARASPEC();

                dsData = new DataSet();

                if (WaferSeqList == null || WaferSeqList.Length <= 0)
                    throw new Exception("Wafer List 정보가 없습니다.");

                dtWaferList = oWafer.GetWaferLotInfo(WaferSeqList);

                if (dtWaferList == null || dtWaferList.Rows.Count <= 0)
                    throw new Exception("Wafer List 정보가 DB에 없습니다.");

                //Program 이 다른 정보가 있는지 확인 한다.
                if (dtWaferList.DefaultView.ToTable(true, "PROGRAM").Rows.Count > 1)
                    throw new Exception("서로 다른 Program 으로 조회 할 수 없습니다.");

                strTestArea = dtWaferList.Rows[0]["TESTAREA"].ToString();
                strProduct = dtWaferList.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtWaferList.Rows[0]["PROGRAM"].ToString();

                dtWaferList.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtWaferList.Copy());

                //===============================================================================================

                //AVI 와 이외의 공정의 Data를 가져오지 못한다.
                if (strTestArea == "AVI")
                    throw new Exception("AVI 또는 Scope는 조회 할 수 없습니다.");

                // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                string[] strTable = new string[dtTemp.Rows.Count];

                for (int it = 0; it < dtTemp.Rows.Count; it++)
                {
                    strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                }

                sQuery.AppendLine(string.Join(",", strTable));
                sQuery.AppendLine(" WHERE 1 = 1 ");

                int ic = 0;
                string[] arrTemp = new string[2];
                foreach (KeyValuePair<string, string> var in dicAlias)
                {
                    if (ic == 0)
                    {
                        sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ ", var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ IN ({1}) ", var.Value, string.Join(",", WaferSeqList)));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) ", arrTemp[1], var.Value));
                        sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM(+) ", arrTemp[1], var.Value));
                    }

                    arrTemp[0] = var.Key;
                    arrTemp[1] = var.Value;
                    ic++;
                }

                //Parameter 관련 정보를 가져온다.
                dtTemp = oParaSepc.GetData("SELECT_PARA_ITEM", null, new string[] { strProgram, WaferSeqList[0].ToString() });
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    List<string> lsItem = new List<string>();
                    DataTable dtPara;

                    dtItem = new DataTable();
                    dtItem.Columns.Add(new DataColumn("PARAM_NAME", typeof(string)));
                    dtItem.Columns.Add(new DataColumn("MAX", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("MIN", typeof(double)));
                    dtItem.Columns.Add(new DataColumn("AVG", typeof(double)));
                    dtItem.AcceptChanges();

                    for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                    {
                        lsItem.Add(dtTemp.Rows[ir]["PARAM_NAME"].ToString());

                        //500 개를 기준으로 여러번 Para를 확인 한다.
                        if (ir != 0 && (int)(ir % 500) == (int)0)
                        {
                            lsItem.Sort();

                            // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                            // GetDataTable_Numeric_IgnoreException로 수정
                            //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                            dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                            foreach (DataRow dr in dtPara.Rows)
                            {
                                drItem = dtItem.NewRow();
                                drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                                drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                                drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                                drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                                dtItem.Rows.Add(drItem);
                            }

                            lsItem.Clear();
                        }

                    }

                    //남은  Para 확인
                    if (lsItem.Count > 0)
                    {
                        // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 해당 메시지 발생
                        // GetDataTable_Numeric_IgnoreException로 수정
                        //dtPara = oParaSepc.GetData("SELECT_PARA_NOT_EMPTY", new string[] { string.Join(",", lsItem.ToArray()), sQuery.ToString(), string.Join(",", lsItem.ToArray()) }, null);
                        dtPara = oParaSepc.GetParameterNotEmpty(lsItem.ToArray(), sQuery.ToString());
                        foreach (DataRow dr in dtPara.Rows)
                        {
                            drItem = dtItem.NewRow();
                            drItem["PARAM_NAME"] = dr["PARAM_NAME"].ToString();
                            drItem["MAX"] = DACrux.Base.Convert.doubleParse(dr["MAX"].ToString());
                            drItem["MIN"] = DACrux.Base.Convert.doubleParse(dr["MIN"].ToString());
                            drItem["AVG"] = DACrux.Base.Convert.doubleParse(dr["AVG"].ToString());
                            dtItem.Rows.Add(drItem);
                        }
                    }

                    dtTemp.TableName = "PARA_ITEM";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtItem.TableName = "PARA_LIST";
                    dsData.Tables.Add(dtItem.Copy());
                }

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dtItem != null)
                    dtItem.Dispose();

                if (dtWaferList != null)
                    dtWaferList.Dispose();

                if (dsData != null)
                    dsData.Dispose();

            }
        }

        public System.Data.DataSet SelectAVICumData(long[] WaferSeq)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtDeviceGrpup = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;

            try
            {
                TQP_MAPDEF oMapDef = new TQP_MAPDEF();
                TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();
                TQP_FOI_DIE oMapFoi = new TQP_FOI_DIE();
                TQP_FOI_IMAGES oMapFoiImage = new TQP_FOI_IMAGES();
                TQP_BINDESC oBin = new TQP_BINDESC();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                string[] strWaferSeqs = new string[WaferSeq.Length];
                for (int i = 0; i < WaferSeq.Length; i++)
                {
                    strWaferSeqs[i] = WaferSeq[i].ToString();
                }

                dtTemp = oWafer.GetData("SELECT_WAFER_BIN_WAFER_BASE", new string[] { string.Join(",", strWaferSeqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Info 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                dtDeviceGrpup = dtTemp.DefaultView.ToTable(true, "PROGRAM");
                if (dtDeviceGrpup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 Program 이 있습니다. 확인 바랍니다.");

                dtDeviceGrpup = dtTemp.DefaultView.ToTable(true, "TESTAREA");
                if (dtDeviceGrpup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 TESTAREA 가 있습니다. 확인 바랍니다.");

                dtDeviceGrpup = dtTemp.DefaultView.ToTable(true, "DEVICE_ALIAS");
                if (dtDeviceGrpup.Rows.Count != 1)
                    throw new Exception("Wafer 중 서로 다른 DEVICE ALIAS 가 있습니다. 확인 바랍니다.");

                if (strTestArea != "AVI")
                    throw new Exception("AVI 외에는 사용 할 수 없습니다.");

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                dtTemp = oMapFoiImage.SelectFOIImageListMulti(strWaferSeqs);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "IMAGES";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                dtTemp = oMapFoi.SelectFOIDataMultiImage(strWaferSeqs);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "MAPDATA_IMAGES";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                dtTemp = oMapFoi.SelectFOIDataMulti(strWaferSeqs);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("AVI 또는 Scope Map Data를 불러오지 못했습니다.");

                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "MAPDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                int iXmax, iYmax, iXmin, iYmin = 0;

                iXmax = System.Convert.ToInt32(dtTemp.Compute("MAX(X)", ""));
                iYmax = System.Convert.ToInt32(dtTemp.Compute("MAX(Y)", ""));
                iXmin = System.Convert.ToInt32(dtTemp.Compute("MIN(X)", ""));
                iYmin = System.Convert.ToInt32(dtTemp.Compute("MIN(Y)", ""));

                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = dtMapDefine(strProduct, strTestArea, iYmax, iXmax, iXmin, iYmin);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Map Define 정보를 불러오지 못했습니다.");

                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                dtTemp = oMapDef.GetData("GET_MAPDEF_MAP", null, new string[] { strProduct });
                dtTemp.TableName = "SHOT_DEF";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                dtTemp = oBin.GetData("SELECT_BIN_DESC", null, new string[] { strProgram });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    dtTemp = oBin.GetData("SELECT_BIN_DESC_ALL", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception(string.Format("Bin에 대한 정보가 없습니다. Program : {0}", strProgram));
                }

                dtTemp.TableName = "BIN_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        #endregion

        #region Pattern Search

        public DataTable GetPatternSearch_WaferList(long[] waferSeqArr)
        {
            TQP_WAFER_SUM obj = new TQP_WAFER_SUM();
            return obj.GetPatternSearch_WaferList(waferSeqArr);
        }

        public List<int> GetPatternSearch_DieList(long waferSeq)
        {
            TD_TABLE obj = new TD_TABLE();
            DataTable dt = obj.GetPatternSearch_XYList(waferSeq);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<int> list = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(int.Parse(row[0].ToString()));
            }

            return list;
        }

        #endregion
    }
}
