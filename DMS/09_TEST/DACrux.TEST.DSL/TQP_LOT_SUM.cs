using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_LOT_SUM : Miracom.Middleware.QueryComponent
    {
        public TQP_LOT_SUM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_LOT_SUM.xml");
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

        public DataTable GetLotList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            string strFieldSort = string.Join(",", strFields);
            string strDynamic = string.Empty;


            strDynamic = string.Format(" WHERE END_TIME BETWEEN TO_DATE('{0}','YYYY-MM-DD') AND TO_DATE('{1}','YYYY-MM-DD') ", StartTime, EndTime);

            if (strWhere != null && strWhere.Length > 0)
            {
                for (int i = 0; i < strWhere.Length; i++)
                {
                    strDynamic = " AND " + strWhere[i];
                }
            }

            if (strSort.Length > 0)
                strDynamic = strDynamic + " ORDER BY " + strSort;

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_LOT_LIST", new string[] { strFieldSort, strDynamic }, null);
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

        public DataTable SelectLotSumByTestarea(string LotID, string TestArea)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_LOT_SUM_BY_TESTAREA", null, new string[] { LotID, TestArea });

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable SelectLotSumByLotID(string LotID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_LOT_SUM_BY_LOT_ID", null, new string[] { LotID });

                return dt;
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

        public DataTable GetWaferInfo(string WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO", null, new string[] { WaferSeq });
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

        public DataTable GetBinSum_MAP(string BINLIST, string WAFER_SEQ)
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

        public DataTable GetWaferInfo_MAP(string WaferSeq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_WAFER_INFO", null, new string[] { WaferSeq });
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

        public DataTable GetWaferInfo_MAP(string[] WaferSeq)
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

        public DataTable GetYiledByProduct(string product)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_YIELD_BY_PRODUCT", new string[] { product }, null);
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

        public DataTable GetLotSumInfo(string LotSeq)
        {
            try
            {
                return this.GetDataTable("GET_LOT_SUM_INFO", null, new string[] { LotSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotInfoByLotIDExcludeFinal(string LotId)
        {
            try
            {
                return this.GetDataTable("SELECT_LOT_INFO_BY_LOT_ID_EXCLUDE_FINAL", null, new string[] { LotId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotSumLastMerge(string LotID)
        {
            try
            {
                return this.GetDataTable("SELECT_LOT_SUM_LAST_MERGE", null, new string[] { LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Map Parser DSL ---------------------------------------------------

        public bool GetIsLot(string Lot_Seq)
        {
            object obj = this.ExecuteScalar("GET_ISLOT", null, new string[] { Lot_Seq });
            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        public void DeleteLot(string Lot_Seq)
        {
            try
            {
                this.Execute("DELETE_LOT", null, new string[] { Lot_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateLot(string strTable)
        {
            try
            {
                this.Execute("CREATE_LOT", new string[] { strTable }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  "Create DMS Lot"
        public void CreateDmsLot(string lotSeq, string lotId, string testArea, string product, string program, string customer, string facility, string startTime, string endTime)
        {
            try
            {
                this.Execute("CREATE_DMS_LOT", null, new string[] { lotSeq, lotId, testArea, product, program, customer, facility, startTime, endTime });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public object CreateDataLot(
            DataTable dt
            )
        {
            return this.ExecuteTable(dt);
        }

        public void DelAVILotSummary(string strLotSeq)
        {
            this.Execute("DELETE_LOT", null, new string[] { strLotSeq });
        }

        public void SetAVILotSummary(string strLotSeq)
        {
            this.Execute("INSERT_LOT_SUMMARY", null, new string[] { strLotSeq });
        }

        public void InsertLotSummary(string lotSeq)
        {
            this.Execute("INSERT_LOT_SUMMARY", null, new string[] { lotSeq });
        }

        public void UpdateLotID(string lotSeq, string lotID, string deviceAlias)
        {
            ExecuteNonQuery("UPDATE_LOT_ID", null, new string[] { lotSeq, lotID, deviceAlias });
        }

        public void DeleteLotData(string lotSeq)
        {
            ExecuteNonQuery("DELETE_LOT", null, new string[] { lotSeq });
        }

        /// <summary>
        /// PROGRAM 정보를 업데이트 합니다.
        /// </summary>
        public void UpdateProgram(string lotSeq, string newProgram)
        {
            ExecuteNonQuery("UPDATE_PROGRAM", null, new string[] { lotSeq, newProgram });
        }
    }
}
