
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_INSP_INFO : Miracom.Middleware.QueryComponent
    {
        public TQD_INSP_INFO()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_INSP_INFO.xml");
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

        public object ExecuteScala(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            return base.ExecuteScalar(
                sqlName,
                dynamic,
                paras
                );
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

        public void Create(string[] stepinfo)
        {
            try
            {
                this.Execute("CREATE_INSP_INFO", null, stepinfo);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public void CreateMig(string[] stepinfo)
        {
            try
            {
                this.Execute("CREATE_INSP_INFO_MIG", null, stepinfo);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public void Delete(string stepSeq)
        {
            try
            {
                this.Execute("DELETE_STEP", null, new string[] { stepSeq });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
        public void Update(string[] stepinfo)
        {
            try
            {
                this.Execute("UPDATE_INSP_INFO", null, stepinfo);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetInspInfoByLotId(string LotId)
        {
            try
            {
                return this.GetDataTable("GET_INSP_INFO_BY_LOT_ID", null, new string[] { LotId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProdutListPeriod(string[] period)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PRODUCT_LIST_PERIOD", null, period);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            return dt;
        }

        public DataTable GetLotListPeriod(string[] period)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_LOT_LIST_PERIOD", null, period);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            return dt;
        }

        public DataTable GetWaferListPeriod(string[] period)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_LIST_PERIOD", null, period);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetStepListPeriod(string[] period)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_LIST_PERIOD", null, period);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetSetupList(string product)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_SETUP_LIST_IN_PRODUCT", null, new string[] { product });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public string GetStepSeqs(string[] period)
        {
            string stepids = string.Empty;
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_SEQS", null, period); //StepInfo[1],StepInfo[5],StepInfo[2],StepInfo[8]
                if (dt.Rows.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            stepids = dt.Rows[i]["STEP_SEQ"].ToString();
                        }
                        else
                        {
                            stepids = string.Format("{0},{1}", stepids, dt.Rows[i]["STEP_SEQ"].ToString());
                        }
                    }
                }
                return stepids;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetStepInfoByStepSeqAndWaferSeq(
            string factory,
            DateTime resultTimeStamp,
            string product,
            string lot,
            string wafer,
            string step
            )
        {
            return this.GetDataTable(
                "GET_STEP_SEQS",
                null,
                new string[] { 
                    factory,
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    product,
                    lot,
                    wafer,
                    step
                });
        }

        public DataTable GetStepInfoByStepSeqAndWaferSeq_01(
            string factory,
            DateTime resultTimeStamp,
            string lot,
            string slot,
            string step
            )
        {
            return this.GetDataTable(
                "GET_STEP_SEQS_01",
                null,
                new string[] { 
                    factory,
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    lot,
                    slot,
                    step
                });
        }

        public DataTable GetStepInfoByStepSeqAndWaferSeq_02(
            string factory,
            DateTime resultTimeStamp,
            string lot,
            string slot,
            string step
            )
        {
            return this.GetDataTable(
                "GET_STEP_SEQS_02",
                null,
                new string[] { 
                    factory,
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    resultTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    lot,
                    slot,
                    step
                });
        }

        public DataTable FindLot(string[] LotInfo)
        {
            return this.GetDataTable("FIND_LOT", null, LotInfo);
        }

        public DataTable FindLot(string fromDate, string toDate, string lotID)
        {
            return this.GetDataTable("FIND_LOT", null, new string[] { fromDate, toDate, String.Format("{0}%", lotID) });
        }

        public DataTable GetStepInfo(string[] StepInfo)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_INFO", null, StepInfo);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetStepInfo(long[] step_seq)
        {
            DataTable dt = null;
            try
            {
                //string[] str_step_seq = new string[step_seq.Length];
                //for (int i = 0; i < str_step_seq.Length; i++) str_step_seq[i] = step_seq[i].ToString();

                dt = this.GetDataTable("GET_INSP_INFO_MULTI_SEQ"
                    , new string[] { string.Join(",", step_seq) }
                    , null);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetStepInfoByStepSeq(string step_seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_INFO_BY_STEP_SEQ", null, new string[] { step_seq });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetStepListByWafer(string[] StepInfo)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_LIST_BY_WAFER", null, StepInfo);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetWaferListPeriod(string Query)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("DYNAMIC", new string[] { Query }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public void UpdateInterlockFlag(long StepSeq, string InterlockFlag)
        {
            try
            {
                this.Execute("UPDATE_INTERLOCK_FLAG", null, new string[] { InterlockFlag, StepSeq.ToString() });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public int UpdateReclassifiedDefect(
            string[,] Params
            )
        {
            return this.ExecuteMultipleEx(
                "UPDATE_RECLASSIFIED_DEFECT_CNT",
                null,
                Params
                );
        }

        public DataTable CheckEquipHold(string Product, string StepID, string MainEq, long StepSeq)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("CHECK_EQUIP_HOLD", null, new string[] { Product, StepID, MainEq, StepSeq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetDynamicQuery(string Query)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("DYNAMIC", new string[] { Query }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }


        public DataTable GetStepList(string start, string end, string[] fieldsort, string[] where, string sort)
        {
            string strFieldSort = string.Join(",", fieldsort);
            string strDynamic = string.Empty;

            if (where != null && where.Length > 0)
            {
                for (int i = 0; i < where.Length; i++)
                {
                    strDynamic = strDynamic + " AND " + where[i];
                }
            }

            if (sort.Length > 0)
                strDynamic = strDynamic + " ORDER BY " + sort;

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_LIST_01", new string[] { strFieldSort, strDynamic }, new string[] { start, end });
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

        public DataTable GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            DataTable dt = null;
            string strLike = string.Empty;
            try
            {
                if (LikeDevice.Length > 0)
                {
                    strLike = strLike + string.Format(" AND PRODUCT LIKE ('%{0}%') ", LikeDevice);
                }
                if (LikeLot.Length > 0)
                {
                    strLike = strLike + string.Format(" AND LOT_ID LIKE ('%{0}%') ", LikeLot);
                }
                if (LikeWafer.Length > 0)
                {
                    strLike = strLike + string.Format(" AND WAFER_ID = '{0}' ", LikeWafer);
                }

                dt = this.GetDataTable("GET_STEP_LIST_BY_STEP_ID", new string[] { strLike }, new string[] { TestArea, StartTime, EndTime });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable SelectMapParsingResultList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_BY_LOTID", null, null);
                return dt;
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
            }
        }

        public DataTable SelectMapParsingResultLotList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_LOTLIST", null, null);
                return dt;
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
            }
        }

        public DataTable SelectMapParsingResultWaferList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_WAFERLIST", null, null);
                return dt;
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
            }
        }

        public DataTable SelectMapParsingResultStepList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_STEPLIST", null, null);
                return dt;
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
            }
        }

        public DataTable SelectMapParsingResultInspEqList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_INSPEQLIST", null, null);
                return dt;
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
            }
        }

        public DataTable SelectMapParsingResultByItem(string lotId, string waferId, string stepId, string inspEq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_RESULT_BY_ITEM", null, new string[] { lotId, waferId, stepId, inspEq });
                return dt;
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
            }
        }

        #region ' TestMap Overlay '
        public DataTable GetStepInfoOverlay(string lotId, string waferid)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_INSP_INFO_OVERLAY",
                    null,
                    new string[] { lotId, waferid }
                    );
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public int UpdateClassifiedCnt(
            long waferseq,
            long stepseq
            )
        {
            return this.ExecuteNonQuery(
                "UPDATE_INSP_INFO[REVIEW_PARSER]",
                null,
                new string[] { stepseq.ToString(), waferseq.ToString() }
                );
        }

        public DataTable GetWaferInfo(long[] stepSeqArr)
        {
            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return null;

            string where = String.Format("'{0}'", String.Join("','", stepSeqArr));

            return GetDataTable("SELECT_WAFER_LIST_01", new string[] { where }, null);
        }

        public DataTable GetStepInfoOverlayMulti(string lotid, string[] wafers)
        {
            return this.GetDataTable(
                "GET_INSP_INFO_OVERLAY[MULTI]",
                new string[] { string.Format("'{0}'", string.Join("','", wafers)) },
                new string[] { lotid }
                );
        }

        public DataTable GetStepWaferData(string strStepSeq, string strWaferSeq)
        {
            return GetDataTable("SELECT_INSP_STEP_WAFER", null, new string[] { strStepSeq, strWaferSeq });
        }

        public void CreateMaintCopy(string strNewStepSeq, string strNewWaferSeq, string strProduct, string strLotID, string strWaferID, string strStepID, string strSlotID, string strOriStepSeq, string strOriWaferSeq)
        {
            this.Execute("CREATE_INSP_INFO_MAINT_COPY", null, new string[] { strNewStepSeq, strNewWaferSeq, strProduct, strLotID, strWaferID, strStepID, strSlotID, strOriStepSeq, strOriWaferSeq });
        }

        public void UpdateInspMaint(string strProduct, string strLotID, string strWaferID, string strStepID, string strSlotID, string strStepSeq, string strWaferSeq)
        {
            this.Execute("UPDATE_MAINT_STEP", null, new string[] { strProduct, strLotID, strWaferID, strStepID, strSlotID, strStepSeq, strWaferSeq });
        }

        public void DeleteInspMaint(string strStepSeq, string strWaferSeq)
        {
            this.Execute("DELETE_STEP_MAINT", null, new string[] { strStepSeq, strWaferSeq });
        }

        public DataTable GetInspIncludeLotSeq(string strStepSeq)
        {
            return this.GetDataTable("SELECT_DM_MAINT_WAFER_SINGLE", null, new string[] { strStepSeq });
        }

        public DataTable GetInspectionLotOrWaferCnt(string factory, string fromDate, string toDate, bool bTypeFlag, bool bAreaFlag)
        {
            String sType = bTypeFlag ? "LOT_QTY" : "WAFER_QTY";
            if (bAreaFlag)
                return GetDataTable("SELECT_INSP_LOT_OR_WAFER_CNT", new string[] { bTypeFlag ? "LOT_QTY" : "WAFER_QTY" }, new string[] { factory, fromDate, toDate });
            else
                return GetDataTable("SELECT_REVIEW_LOT_OR_WAFER_CNT", new string[] { bTypeFlag ? "LOT_QTY" : "WAFER_QTY" }, new string[] { factory, fromDate, toDate });
        }

        public DataTable GetInspectionLotOrWaferRawData(string factory, string fromdate, string todate, bool bAreaFlag)
        {
            if (bAreaFlag)
                return GetDataTable("SELECT_INSP_LOT_OR_WAFER_RAWDATA", null, new string[] { factory, fromdate, todate });
            else
                return GetDataTable("SELECT_REVIEW_LOT_OR_WAFER_RAWDATA", null, new string[] { factory, fromdate, todate });
        }


        public DataTable GetInspectionDetail(string factory, string fromDate, string toDate, string model, bool bAreaFlag)
        {
            if (bAreaFlag)
                return GetDataTable("SELECT_INSP_DETAIL", null, new string[] { factory, fromDate, toDate, model });
            else
                return GetDataTable("SELECT_REVIEW_DETAIL", null, new string[] { factory, fromDate, toDate, model });
        }


        public DataTable UpdateMaintLotID(string strStepSeq, string strLotID)
        {
            return this.GetDataTable("UPDATE_MAINT_LOT", new string[] { strStepSeq }, new string[] { strLotID });
        }


        public object[,] GetInspectionDefectSummaryData(long[] steps)
        {
            return GetDataToObjectArray("SELECT_DM_DEFECT_SUMMARY", new String[] { String.Format("{0}", String.Join(",", steps)) }, null);
        }

        public DataTable GetInspectionDefectiveDieSummaryData(string[] xAxis, string yAxis, long[] steps, string groupBy, string orderBy)
        {
            String selectCondition = String.Format("{0}", String.Join(",", xAxis));
            if (!String.IsNullOrEmpty(yAxis))
                selectCondition = String.Format("{0}, {1}", selectCondition, yAxis);
            return GetDataTable("SELECT_DM_DEFECTIVE_SUMMARY", new String[] { selectCondition, String.Format("{0}", String.Join(",", steps)), groupBy, orderBy }, null);
        }

        public DataTable GetInspInfo01(long[] stepSeqArr)
        {
            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return null;

            return GetDataTable("SELECT_INSP_INFO_01", new string[] { String.Join(",", stepSeqArr) }, null);
        }

        public DataTable GetInspInfo02(long[] stepSeqArr)
        {
            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return null;

            return GetDataTable("SELECT_INSP_INFO_02", new string[] { String.Join(",", stepSeqArr) }, null);
        }

        #region [ Condition ]
        public DataTable GetConditionProduct(
            string fromDate,
            string toDate
            )
        {
            return GetDataTable("SELECT_CONDITION_PRODUCT", null, new string[] { fromDate, toDate });
        }


        public DataTable GetConditionStep(
            string fromDate,
            string toDate,
            string product
            )
        {
            return GetDataTable("SELECT_CONDITION_STEP", null, new string[] { fromDate, toDate, product });
        }


        public DataTable GetConditionStep(
            string fromDate,
            string toDate,
            string[] products
            )
        {
            string product = String.Format("'{0}'", String.Join("','", products));
            return GetDataTable("SELECT_CONDITION_STEP_MULTI", new string[] { product }, new string[] { fromDate, toDate });
        }


        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string product,
            string stepid
            )
        {
            return GetDataTable("SELECT_CONDITION_LOT", null, new string[] { fromDate, toDate, product, stepid });
        }

        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string product,
            string[] stepids
            )
        {
            string steps = String.Format("'{0}'", String.Join("','", stepids));
            return GetDataTable("SELECT_CONDITION_LOT_MULTI_01", new string[] { steps }, new string[] { fromDate, toDate, product });
        }

        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids
            )
        {
            string product = String.Format("'{0}'", String.Join("','", products));
            string steps = String.Format("'{0}'", String.Join("','", stepids));
            return GetDataTable("SELECT_CONDITION_LOT_MULTI_02", new string[] { product, steps }, new string[] { fromDate, toDate });
        }


        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string product,
            string stepid,
            string[] lotids
            )
        {
            return GetDataTable("SELECT_CONDITION_WAFER", new string[] { string.Format("'{0}'", string.Join("','", lotids)) }, new string[] { fromDate, toDate, product, stepid });
        }

        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string product,
            string[] stepids,
            string[] lotids
            )
        {
            string steps = String.Format("'{0}'", String.Join("','", stepids));
            string lots = String.Format("'{0}'", string.Join("','", lotids));
            return GetDataTable("SELECT_CONDITION_WAFER_MULTI_01", new string[] { steps, lots }, new string[] { fromDate, toDate, product });
        }

        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lotids,
            bool reviewOnly
            )
        {
            string product = String.Format("'{0}'", String.Join("','", products));
            string steps = String.Format("'{0}'", String.Join("','", stepids));
            string lots = String.Format("'{0}'", string.Join("','", lotids));
            string review = reviewOnly ? "AND EXISTS (SELECT * FROM TQD_STEP WHERE STEP_SEQ = A.STEP_SEQ AND REVIEW_EQ IS NOT NULL)" : String.Empty;

            return GetDataTable("SELECT_CONDITION_WAFER_MULTI_02", new string[] { product, steps, lots, review }, new string[] { fromDate, toDate });
        }

        #endregion [ Condition ]

        public DataTable GetStepSeqBy(string lotID, string equipID, string resultTimestamp)
        {
            return GetDataTable("SELECT_STEP_SEQ_BY_01", null, new string[] { lotID, equipID, resultTimestamp });
        }

        public DataTable GetPatternSearch_WaferList(long[] stepSeqArr)
        {
            return GetDataTable("SELECT_WAFER_LIST_02", new string[] { String.Join<long>(",", stepSeqArr) }, null);
        }

        public DataTable GetLotinspInfo(string lotID, bool onlyLastInspection)
        {
            string query = String.Empty;

            if (onlyLastInspection)
                query = "AND B.TEST_ORDER = 0";

            return GetDataTable("SELECT_LOT_INSP_INFO", new string[] { query }, new string[] { lotID });
        }
    }
}
