using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_WAFER_SUM : Miracom.Middleware.QueryComponent
    {
        public TQP_WAFER_SUM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                //connectID = "EMSMGR";
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_WAFER_SUM.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Common Select]
        public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(string sqlName, string[,] paras)
        {
            try
            {
                this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateMulti(string sqlName, string[,] paras)
        {
            try
            {
                return this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region OLD
        public DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            string strFieldSort = string.Join(",", strFields);
            string strDynamic = string.Empty;


            strDynamic = string.Format(" WHERE END_TIME BETWEEN TO_DATE('{0}','YYYYMMDDHH24MISS') AND TO_DATE('{1}','YYYYMMDDHH24MISS') ", StartTime, EndTime);

            if (strWhere != null && strWhere.Length > 0)
            {
                for (int i = 0; i < strWhere.Length; i++)
                {
                    strDynamic = strDynamic + " AND " + strWhere[i];
                }
            }

            if (strSort.Length > 0)
                strDynamic = strDynamic + " ORDER BY " + strSort;

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_LIST", new string[] { strFieldSort, strDynamic }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectPickWafers(string strLotSeq, string WaferList)
        {
            try
            {
                return GetDataTable("SELECT_PICK_WAFERS", new string[] { WaferList }, new string[] { strLotSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectWaferSumInfo(string LotID, string WaferID)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_SUM_INFO", null, new string[] { LotID, WaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectWaferSumMaxSeqMerge(string LotID, string WaferID)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_SUM_MAX_SEQ_MERGE", null, new string[] { LotID, WaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectWaferSumByLotID(string LotID)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_SUM_BY_LOTID", null, new string[] { LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectWaferSumInfo(string LotID, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_INFO_LOT_WAFER_TESTAREA", null, new string[] { LotID, WaferID, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfos(string Product, string LotID, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_INFOS", new string[] { TestArea }, new string[] { Product, LotID, WaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinSum(string BINLIST, string WAFER_SEQ)
        {
            string strDynamic = string.Empty;
            string strFieldList = string.Empty;
            string[] strBinList = BINLIST.Split(',');

            strFieldList = "BIN" + string.Join(",BIN", strBinList);

            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_LIST_WITH_SEQ", new string[] { strFieldList }, new string[] { WAFER_SEQ });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfo(string TestArea, string LotID, string WaferID, bool bDummy)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_INFO_BY_TESTAREA", null, new string[] { TestArea, LotID, WaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIWaferSum(string PROGRAM, string[] WaferSeq)
        {
            DataTable dt = null;
            string strWaferSeqs = string.Join(",", WaferSeq);
            try
            {
                dt = this.GetDataTable("GET_AVI_WAFER_SUM", new string[] { PROGRAM, strWaferSeqs, PROGRAM, strWaferSeqs }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectGoldenCheckWaferQty(string LotID, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_GOLDEN_CHECK_WAFER_QTY", new string[] { WaferID }, new string[] { LotID, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGoldenCheckAVIEach(string LotID, string Product, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_GOLDEN_CHECK_AVI_EACH", null, new string[] { LotID, Product, WaferID, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfoOverThousand(string wafer_seq, string min, string max)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_MULTI_OVER_THOUSAND", new string[] { wafer_seq, min, max }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfoOverThousand(string wafer_seq, string min, string max, string[] lot)
        {
            DataTable dt = null;
            string tmp = string.Empty;
            try
            {
                tmp = "'" + string.Join("','", lot) + "'";
                dt = this.GetDataTable("GET_WAFER_INFO_MULTI_OVER_THOUSAND_BET", new string[] { wafer_seq, min, max, tmp }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfo()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_ALL", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteWaferData(string WaferSeq)
        {
            try
            {
                this.Execute("DELETE_WAFER_DATA", null, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMaintProbeCount(string WaferSeq, string ProbeCnt)
        {
            try
            {
                this.Execute("UPDATE_MAINT_PROBE_CNT", null, new string[] { ProbeCnt, WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo_()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_K39VAGSL14_PT1", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetStdMap(string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STD_MAP", null, new string[] { PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetNextLotSeq()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAX_LOTSEQ", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetNextWaferSeq()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAX_WAFERSEQ", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateLot(string TESTAREA, string PRODUCT, string PROGRAM, string LOTID)
        {
            try
            {
                this.Execute("CREATE_NEW_LOT", null, new string[] { PRODUCT, TESTAREA, PROGRAM, LOTID, LOTID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateWafer(string LOT_SEQ, string WAFER_ID, string TESTEDDIE, string STIME, string ETIME)
        {
            try
            {
                this.Execute("CREATE_NEW_WAFER", null, new string[] { LOT_SEQ, WAFER_ID, TESTEDDIE, STIME, ETIME });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateMap(string PROGRAM, string WAFERSEQ, string DIEID, string X, string Y, string BIN, string HBIN)
        {
            //(:wafer_seq, :dieid, :x, :y, :bin, :hbin, '0',0,0,0)
            try
            {
                this.Execute("CREATE_MAP", new string[] { PROGRAM }, new string[] { WAFERSEQ, DIEID, X, Y, BIN, HBIN });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDesc(string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BINDESC", null, new string[] { PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLotTime(string STIME, string ETIME, string LOTSEQ)
        {
            try
            {
                this.Execute("UPDATE_LOT_TIME", null, new string[] { STIME, ETIME, LOTSEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcPrbWaferSum(string TESTAREA, string PRODUCT, string PROGRAM, string WAFERSEQ, string GEC)
        {
            try
            {
                this.ExecuteScalar("PROC_RUN_PRB_SUMMARY", null, new string[] { TESTAREA, PRODUCT, PROGRAM, WAFERSEQ, GEC });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetMesWaferList(string[] strFields)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MES_WAFER_LIST", null, strFields);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string product, string lotId, string waferId)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_2", null, new string[] { product, lotId, waferId });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetWaferDefectOverlay(string program, string[] waferSeq)
        {
            DataTable dt = null;

            try
            {
                string[] para = new string[] { program };
                string wafers = string.Join(",", waferSeq);
                string[] dynamic = new string[] { wafers };
                dt = this.GetDataTable("GET_WAFER_IN_DEFECT_WAFER", dynamic, para);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetYieldByDay(string waferSeq)
        {
            DataTable dt = null;

            try
            {
                //string[] dynamic = new string[] { string.Join(",", waferSeq) };
                dt = this.GetDataTable("YIELD_REPORT_BY_DAY", new string[] { waferSeq }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetWaferInfo(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFERINFO", null, new string[] { TESTAREA, PRODUCT, LOT_ID, WAFER_ID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetWaferInfo(string TestArea, string Product, string Program, string LotID, string WaferID, int ProbeCnt = 0)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_INFO_03", null, new string[] { TestArea, Product, Program, LotID, WaferID, ProbeCnt.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string TestArea, string Product, string Program, string LotID, string WaferID, int ProbeCnt = 0, bool isMaxOper = true)
        {
            try
            {
                if (isMaxOper == false)
                {
                    return GetWaferInfo(TestArea, Product, Program, LotID, WaferID, ProbeCnt);
                }
                return this.GetDataTable("GET_WAFER_INFO_04", null, new string[] { TestArea, Product, Program, LotID, WaferID, ProbeCnt.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfoWithoutProduct(string TESTAREA, string LOT_ID, string WAFER_ID)
        {
            return this.GetDataTable("GET_WAFER_INFO_WITHOUT_PRODUCT", null, new string[] { TESTAREA, LOT_ID, WAFER_ID });
        }


        public int GetOnlyVI(string PROGRAM, string WAFER_SEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_ONLYVI", new string[] { PROGRAM.Replace("-", "_") }, new string[] { WAFER_SEQ });
                if (dt != null && dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0]["CNT"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public void UpdateSummaryFTA(string WAFER_SEQ, int FTA)
        {
            this.Execute("UPDATE_FTA", null, new string[] { FTA.ToString(), WAFER_SEQ });
        }

        public DataTable GetYieldByWafer(string[] WAFERSEQ)
        {
            return this.GetDataTable("GET_YIELD_BYWAFER", new string[] { string.Join(",", WAFERSEQ) }, null);
        }

        public DataTable GetYieldByLot(string[] WAFERSEQ)
        {
            return this.GetDataTable("GET_YIELD_BYLOT", new string[] { string.Join(",", WAFERSEQ) }, null);
        }

        public DataTable GetYieldByDate(string[] WAFERSEQ)
        {
            return this.GetDataTable("GET_YIELD_BYDATE", new string[] { string.Join(",", WAFERSEQ) }, null);
        }

        public DataTable GetBoxYieldByLot(string[] WAFERSEQ)
        {
            return this.GetDataTable("GET_BOX_YIELD_BYLOT", new string[] { string.Join(",", WAFERSEQ) }, null);
        }

        public DataTable GetBoxYieldByDate(string[] WAFERSEQ)
        {
            return this.GetDataTable("GET_BOX_YIELD_BYDATE", new string[] { string.Join(",", WAFERSEQ) }, null);
        }

        #region Map Parser DSL-------------------------------------------------

        public void UpdateDataProbeCnt(string Lot_Seq, string WaferID)
        {
            this.Execute("UPDATE_DATA_PROBE_CNT", null, new string[] { Lot_Seq, WaferID });
        }

        public void CreateDataWafer(string WaferInfo)
        {
            this.Execute("CREATE_WAFER", new string[] { WaferInfo }, null);
        }

        public object CreateDataWafer(
            DataTable dt
            )
        {
            return this.ExecuteTable(
                dt
                );
        }

        public void CopyWaferSum(string WaferInfo, string TestArea, string LotSeq, string WaferSeq, string startTime, string EndTime,
            string LossDie, string Yield, string CopyFLag, string PreWaferSeq)
        {
            try
            {
                this.Execute("COPY_WAFER_SUM"
                    , new string[] { WaferInfo }
                    , new string[] { TestArea, LotSeq, WaferSeq, startTime, EndTime, LossDie, Yield, CopyFLag, PreWaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinSum(string Lot_Seq)
        {
            try
            {
                return GetDataTable("SELECT_BINSUM", null, new string[] { Lot_Seq });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion ------------------------------------------------------------

        public DataTable SelectGoldenCheck(string WaferSeq, string MapID, string TableName)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_GOLDEN_CHECK_1", new string[] { TableName }, new string[] { MapID, WaferSeq });
                if (dt.Rows.Count < 1)
                {
                    dt = this.GetDataTable("SELECT_GOLDEN_CHECK_2", new string[] { TableName }, new string[] { WaferSeq, MapID });
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpadteGoldenFlag(string WaferSeq)
        {
            try
            {
                this.Execute("UPDATE_GOLDEN_FLAG", null, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDeviceTable(string device)
        {
            try
            {
                this.Execute("CREATE_DEVICE_TABLE", new string[] { device }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectByDynamic(string Dynamic)
        {
            try
            {
                return this.GetDataTable("SELECT_BY_DYNAMIC", new string[] { Dynamic }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGoldenCheckList(string LotID, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_GOLDEN_CHECK_LIST", new string[] { WaferID }, new string[] { LotID, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGoldenCheckAVI(string LotID, string WaferID, string TestArea, string Product)
        {
            try
            {
                return this.GetDataTable("SELECT_GOLDEN_CHECK_AVI"
                    , new string[] { WaferID, WaferID, WaferID, WaferID, WaferID, WaferID, WaferID }
                    , new string[] { LotID, TestArea, Product });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeSum(string TableName, string WaferSeq)
        {
            try
            {
                return this.GetDataTable("GET_MERGE_SUM", new string[] { TableName }, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfoListByTestArea(string TestArea, string LotID)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_INFO_LIST_BY_TESTAREA", null, new string[] { TestArea, LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetWaferInfo(long WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO", null, new string[] { WaferSeq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferLotInfo(long WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_LOT_INFO", null, new string[] { WaferSeq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfo(long[] WaferSeqs)
        {
            DataTable dt = null;

            try
            {
                string[] strWaferSeqs = new string[WaferSeqs.Length];
                for (int i = 0; i < WaferSeqs.Length; i++)
                {
                    strWaferSeqs[i] = WaferSeqs[i].ToString();
                }

                dt = this.GetDataTable("GET_WAFER_INFO_MULTI", new string[] { string.Join(",", strWaferSeqs) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferLotInfo(long[] WaferSeqs)
        {
            DataTable dt = null;

            try
            {
                string[] strWaferSeqs = new string[WaferSeqs.Length];
                for (int i = 0; i < WaferSeqs.Length; i++)
                {
                    strWaferSeqs[i] = WaferSeqs[i].ToString();
                }

                dt = this.GetDataTable("GET_WAFER_LOT_INFO_MULTI", new string[] { string.Join(",", strWaferSeqs) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetMapConfigWaferList(string strStartTime, string strEndTime, string strTestArea, string strDevice, string strProgram)
        {
            string strDynamic = string.Empty;
            try
            {
                if (strProgram != "ALL")
                    strDynamic = string.Format("AND PROGRAM LIKE '{0}'", strProgram);

                return this.GetDataTable("SELECT_MAP_CONFIG_LIST", new string[] { strDynamic }, new string[] { strStartTime, strEndTime, strTestArea, strDevice });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string[] WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_MULTI(BY_WAFERIDS)", new string[] { "'" + string.Join("','", WaferSeq) + "'" }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetYieldByDay(string[] waferSeq)
        {
            DataTable dt = null;

            try
            {
                //string[] dynamic = new string[] { string.Join(",", waferSeq) };
                dt = this.GetDataTable("YIELD_REPORT_BY_DAY", new string[] { string.Join(",", waferSeq) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }


        public DataTable GetWaferLitstByAll(string lotId, string product, string testarea)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_LIST_BY_ALL", null, new string[] { lotId, product, testarea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferListByAll(string lotId, string product, string testarea, string waferList)
        {
            try
            {
                return this.GetDataTable("GET_WAFER_LIST_BY_ALL_WITH_WAFER_LIST", new string[] { waferList }, new string[] { lotId, product, testarea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelecTestAreaLastMerge(string lotId, string waferID, string product)
        {
            try
            {
                return this.GetDataTable("SELECT_TESTAREA_LAST_MERGE", null, new string[] { lotId, waferID, product });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectProbeData(string LotID)
        {
            try
            {
                return this.GetDataTable("SELECT_PROBE_DATA", null, new string[] { LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectWaferMatch(string strFactory, string strLotId, string strTestArea, string[] waferid)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_MATCH", new string[] { string.Format("'{0}'", string.Join("','", waferid)) }, new string[] { strFactory, strLotId, strTestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  "Create DMS Wafer"
        public void CreateDmsWafer(string lotSeq, string lotId, string testArea, string product, string program, string customer, string facility,
                                    string startTime, string endTime, string waferSeq, string waferId)
        {
            try
            {
                this.Execute("CREATE_DMS_WAFER", null, new string[] { lotSeq, lotId, testArea, product, program, customer, facility, 
                                                                                       startTime, endTime, waferSeq, waferId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  "Update Bin Information For DMS Lot"
        //  UPDATE_BIN_INFO_DMS
        public void UpdateBinInfoDms(string value, string waferSeq, string lotSeq, string lotId, string testarea)
        {
            try
            {
                this.Execute("UPDATE_BIN_INFO_DMS", new string[] { value }, new string[] { waferSeq, lotSeq, lotId, testarea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetWaferInfoByLotSlot(string lot_id, string slot_id, string testarea)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_WAFER_INFO_BY_LOT_SLOT", null, new string[] { lot_id, slot_id, testarea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfoByWaferId(string wafer_id, string testarea)
        {
            return this.GetDataTable("SELECT_WAFER_INFO_BY_WAFER_ID", null, new string[] { wafer_id, testarea });
        }

        public void ProbeCntPlus(long lLotSeq, string WaferID)
        {
            this.ExecuteNonQuery("PROBE_CNT_PLUS", null, new string[] { lLotSeq.ToString(), WaferID });
        }

        public void ProbeCntPlus(string LotSeq, string WaferID)
        {
            this.ExecuteNonQuery("PROBE_CNT_PLUS", null, new string[] { LotSeq, WaferID });
        }

        public void ProcWaferSum(string Product, long lWaferSeq)
        {
            this.ExecuteNonQuery("PROC_RUN_AVI_SUMMARY", null, new string[] { Product, lWaferSeq.ToString() });

        }

        public void SetAVIWaferSum(string strWaferSeq)
        {

            this.Execute("INSERT_TQP_WAFER_SUM", null, new string[] { strWaferSeq });
        }


        public void DelAVIWaferSummary(string strWaferSeq)
        {

            this.Execute("DELETE_WAFER_DATA", null, new string[] { strWaferSeq });
        }

        public void SetAVIWaferSummary(string strWaferSeq)
        {

            this.Execute("INSERT_TQP_WAFER_SUM_01", null, new string[] { strWaferSeq });
        }

        public DataTable GetWaferSummaryInfo(
            string lotseq
            )
        {
            return this.GetDataTable(
                "GET_WAFER_SUMMARY_INFO [LSEQ]",
                null,
                new string[] { lotseq }
                );
        }

        public bool GetIsWafer(
            string waferSeq
            )
        {
            object obj = this.ExecuteScalar(
                "GET_ISWAFER",
                null,
                new string[] { waferSeq }
                );

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        /// <summary>
        /// WAFER ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferID(string waferSeq, string waferID, int probeCnt)
        {
            ExecuteNonQuery("UPDATE_WAFER_ID", null, new string[] { waferSeq, waferID, probeCnt.ToString() });
        }

        public void UpdateLotID(string lotSeq, string lotID, string deviceAlias)
        {
            ExecuteNonQuery("UPDATE_LOT_ID", null, new string[] { lotSeq, lotID, deviceAlias });
        }

        public void DeleteLotData(string lotSeq)
        {
            ExecuteNonQuery("DELETE_LOT_DATA", null, new string[] { lotSeq });
        }

        /// <summary>
        /// PROGRAM 정보를 업데이트 합니다.
        /// </summary>
        public void UpdateProgram(string waferSeq, string lotSeq, string newProgram, int probeCnt)
        {
            ExecuteNonQuery("UPDATE_PROGRAM", null, new string[] { waferSeq, lotSeq, newProgram, probeCnt.ToString() });
        }

        public void IncreaseProbeCount(string lotSeq, string waferSeq)
        {
            ExecuteNonQuery("INCREASE_PROBE_COUNT", null, new string[] { lotSeq, waferSeq });
        }

        public void WaferConfigSeqUpdate(string wafer_seq, string config_seq)
        {
            ExecuteNonQuery("UPDATE_CONFIG_SEQ", null, new string[] { config_seq, wafer_seq });
        }

        public DataTable GetPatternSearch_WaferList(long[] waferSeqArr)
        {
            return GetDataTable("SELECT_WAFER_LIST_01", new string[] { String.Join<long>(",", waferSeqArr) }, null);
        }
    }
}
