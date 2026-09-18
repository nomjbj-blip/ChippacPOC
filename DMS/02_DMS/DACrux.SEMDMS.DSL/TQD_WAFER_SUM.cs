using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_WAFER_SUM : Miracom.Middleware.QueryComponent
    {
        public TQD_WAFER_SUM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
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
            finally
            {
                if (dt != null) dt.Dispose();
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
            finally
            {
                if (dt != null) dt.Dispose();
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
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetWaferInfoWithoutProduct(string TESTAREA, string LOT_ID, string WAFER_ID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_WITHOUT_PRODUCT", null, new string[] { TESTAREA, LOT_ID, WAFER_ID });
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
            try
            {
                this.Execute("UPDATE_FTA", null, new string[] { FTA.ToString(), WAFER_SEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetYieldByWafer(string[] WAFERSEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_YIELD_BYWAFER", new string[] { string.Join(",", WAFERSEQ) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetYieldByLot(string[] WAFERSEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_YIELD_BYLOT", new string[] { string.Join(",", WAFERSEQ) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetYieldByDate(string[] WAFERSEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_YIELD_BYDATE", new string[] { string.Join(",", WAFERSEQ) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetBoxYieldByLot(string[] WAFERSEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BOX_YIELD_BYLOT", new string[] { string.Join(",", WAFERSEQ) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetBoxYieldByDate(string[] WAFERSEQ)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BOX_YIELD_BYDATE", new string[] { string.Join(",", WAFERSEQ) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        #region Map Parser DSL-------------------------------------------------

        public void UpdateDataProbeCnt(string Lot_Seq, string WaferID)
        {
            try
            {
                this.Execute("UPDATE_DATA_PROBE_CNT", null, new string[] { Lot_Seq, WaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDataWafer(string WaferInfo)
        {
            try
            {
                this.Execute("CREATE_WAFER", new string[] { WaferInfo }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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


        public DataTable GetWaferInfo(string[] WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO_MULTI", new string[] { string.Join(",", WaferSeq) }, null);
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
    }
}
