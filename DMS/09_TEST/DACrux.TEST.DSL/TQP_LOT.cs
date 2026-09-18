using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_LOT : Miracom.Middleware.QueryComponent
    {
        public TQP_LOT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_LOT.xml");
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


        public DataTable SelectLotInfo(string LotSeq)
        {
            try
            {
                return this.GetDataTable("SELECT_LOTINFO", null, new string[] { LotSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetLotList(string StartTime, string EndTime, string TESTAREA, string PRODUCT, string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_LOT_LIST", null, new string[] { StartTime, EndTime, TESTAREA, PRODUCT, PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferCountInLot(string LotSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_COUNT_IN_LOT", null, new string[] { LotSeq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLot(string LotSeq)
        {
            try
            {
                this.Execute("DELETE_LOT", null, new string[] { LotSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLotAVI(string factory, string testArea, string lotID)
        {
            try
            {
                this.Execute("DELETE_LOT_AVI", null, new string[] { factory, testArea, lotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotInfoByLotID(string LotID)
        {
            try
            {
                return this.GetDataTable("SELECT_LOT_INFO_BY_LOT_ID", null, new string[] { LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestArea(string StartTime, string EndTime)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_TESTAREA_LIST", null, new string[] { StartTime, EndTime });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetProductList(string StartTime, string EndTime, string[] TESTAREA)
        {
            return this.GetDataTable("GET_PRODUCT_LIST", new string[] { string.Format("'{0}'", string.Join("','", TESTAREA)) }, new string[] { StartTime, EndTime });
        }

        public DataTable GetProgramList(string StartTime, string EndTime, string TESTAREA, string PRODUCT)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PROGRAM_LIST", null, new string[] { StartTime, EndTime, TESTAREA, PRODUCT });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProgramList(string dtStart, string dtEnd, string[] testarea, string[] device)
        {
            return this.GetDataTable("GET_PROGRAM_LIST_01", new string[] { string.Format("'{0}'", string.Join("','", testarea)), string.Format("'{0}'", string.Join("','", device)) }, new string[] { dtStart, dtEnd });
        }


        #region Map Parser 에서 쓰는 DSL--------------------------------------------------

        public DataTable GetIslot(string Device, string strTestArea, string TestProgram, string LotID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_ISLOT", null, new string[] { Device, strTestArea, TestProgram, LotID });
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

        public void CreateLot(string lotSeq,
                                string Device,
                                string TestArea,
                                string TestProgram,
                                string LotNo,
                                string MotherLotNo,
                                string LotType,
                                string FabLot,
                                string Family,
                                string Facility,
                                string device_alias,
                                string StartTime,
                                string EndTime,
                                string WipStatus,
                                string wafers,
                                string SpesailCmt,
                                string EngCmt)
        {
            try
            {
                this.Execute("CREATE_LOT", null, new string[] {lotSeq, 
                                                                Device,
                                                                TestArea,
                                                                TestProgram,
                                                                LotNo, 
                                                                MotherLotNo,
                                                                LotType, 
                                                                FabLot,
                                                                Family,
                                                                Facility,
                                                                device_alias,
                                                                StartTime,
                                                                EndTime,
                                                                WipStatus,
                                                                wafers,
                                                                SpesailCmt,
                                                                EngCmt});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyLot(string TestArea, string STime, string ETime, string PreLotSeq)
        {
            try
            {
                this.Execute("COPY_LOT", null, new string[] { TestArea, STime, ETime, PreLotSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCount(string StartTime, string EndTime, string Wafers, string Lot_Seq)
        {
            try
            {
                this.Execute("UPDATE_COUNT", null, new string[] { StartTime, EndTime, Wafers, Lot_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextLotSeq()
        {
            try
            {
                return ExecuteScalar("SELECT_NEXT_LOTSEQ", null, null).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion------------------------------------------------------------------------

        public DataTable SelectCurrentLotTesarea(string lotId)
        {
            DataTable ds = null;
            try
            {
                ds = this.GetDataTable("SELECT_CURTLOT_TESTAREA", null, new string[] { lotId });
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ds != null) ds.Dispose();
            }
        }

        public long CreateLot(string TestArea, string Product, string Program, string LotID)
        {
            DataTable dt = null;
            long lLotSeq = -1;
            try
            {
                if (string.IsNullOrEmpty(Program)) Program = string.Format("{0}_{1}", TestArea, Product);

                dt = this.SelectLotInfo(TestArea, Product, Program, LotID);

                if (dt == null || dt.Rows.Count == 0)
                {
                    int ir = this.ExecuteNonQuery("CREATE_LOT_02", null, new string[] { Product, TestArea, Program, LotID });
                    if (ir > 0)
                    {
                        dt = this.SelectLotInfo(TestArea, Product, Program, LotID);
                    }
                    else
                    {
                        return -1;
                    }
                }

                if (dt == null || dt.Rows.Count == 0) return -1;
                if (long.TryParse(dt.Rows[0]["LOT_SEQ"].ToString(), out lLotSeq) == false) throw new Exception("Unknown Error on create Lot");

                return lLotSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotInfo(string TestArea, string Product, string Program, string LotID)
        {
            try
            {
                return this.GetDataTable("SELECT_LOTINFO_02", null, new string[] { TestArea, Product, Program, LotID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLotSeq(string factory, string testArea, string lotID)
        {
            object obj = ExecuteScalar("SELECT_LOT_SEQ", null, new string[] { factory, testArea, lotID });

            if (obj == null || obj == DBNull.Value)
                return null;

            return obj.ToString();
        }

        public string GetLotSeq(string factory, string testArea, string lotID, string strProgram)
        {
            object obj = ExecuteScalar("SELECT_LOT_SEQ_01", null, new string[] { factory, testArea, lotID, strProgram });

            if (obj == null || obj == DBNull.Value)
                return null;

            return obj.ToString();
        }

        public string GetNewLotSeq()
        {
            return ExecuteScalar("SELECT_NEXT_LOTSEQ", null, null).ToString();
        }

        public void MergeData(
            string FACTORY,
            string LOT_SEQ,
            string LOT_ID,
            string TEST_AREA,
            string PROGRAM,
            string DEVICE,
            string DEVICE_ALIAS,
            string START_TIME,
            string END_TIME,
            string WAFER_DIAM)
        {
            ExecuteNonQuery("MERGE_DATA", null, new string[]
            {
                FACTORY,
                LOT_SEQ,
                LOT_ID,
                TEST_AREA,
                PROGRAM,
                DEVICE,
                DEVICE_ALIAS,
                START_TIME,
                END_TIME,
                WAFER_DIAM
            });
        }

        public DataTable GetLotSummaryInfo(string lotseq)
        {
            return this.GetDataTable(
                "GET_LOT_SUMMARY_INFO",
                null,
                new string[] { lotseq }
                );
        }

        /// <summary>
        /// 해당 TESTAREA의 DEVICE 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionDevice(string fromDate, string toDate, string[] testAreaArr)
        {
            if (testAreaArr == null || testAreaArr.Length == 0)
                return null;

            string testArea = String.Format("'{0}'", String.Join("','", testAreaArr));

            return GetDataTable("SELECT_CONDITION_DEVICE", new string[] { testArea }, new string[] { fromDate, toDate });
        }

        /// <summary>
        /// 해당 TESTAREA, DEVICE의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionProgram(string fromDate, string toDate, string[] testAreaArr, string[] deviceArr)
        {
            if (testAreaArr == null || testAreaArr.Length == 0 || deviceArr == null || deviceArr.Length == 0)
                return null;

            string testArea = String.Format("'{0}'", String.Join("','", testAreaArr));
            string product = String.Format("'{0}'", String.Join("','", deviceArr));

            return GetDataTable("SELECT_CONDITION_PROGRAM", new string[] { testArea, product }, new string[] { fromDate, toDate });
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] programArr)
        {
            string program = null;

            if (programArr != null && programArr.Length > 0)
                program = String.Format("'{0}'", String.Join("','", programArr));

            return GetDataTable("SELECT_CONDITION_LOT", new string[] { program }, new string[] { fromDate, toDate });
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] testareaArr, string[] productArr, string[] programArr)
        {
            string testarea = string.Empty;
            string product = string.Empty;
            string program = string.Empty;

            if (testareaArr != null && testareaArr.Length > 0)
                testarea = String.Format("'{0}'", String.Join("','", testareaArr));

            if (productArr != null && productArr.Length > 0)
                product = String.Format("'{0}'", String.Join("','", productArr));

            if (programArr != null && programArr.Length > 0)
                program = String.Format("'{0}'", String.Join("','", programArr));

            return GetDataTable("SELECT_CONDITION_LOT_01", new string[] { testarea, product, program }, new string[] { fromDate, toDate });
        }

        public string[] GetProgram(string[] lotSeqArr)
        {
            if (lotSeqArr == null || lotSeqArr.Length == 0)
                return null;

            string lotSeq = String.Join(",", lotSeqArr);

            DataTable dt = GetDataTable("SELECT_DISTINCT_PROGRAM", new string[] { lotSeq }, null);

            if (dt == null | dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public void UpdateLotID(string lotSeq, string lotID, string deviceAlias)
        {
            ExecuteNonQuery("UPDATE_LOT_ID", null, new string[] { lotSeq, lotID, deviceAlias });
        }

        public DataTable GetLotInfo(string testArea, string fromDate, string toDate, string lotID)
        {
            return GetDataTable("SELECT_LOT_INFO_01", null, new string[] { testArea, fromDate, toDate, lotID });
        }

        public DataTable GetLotInfo(string[] testArea, string fromDate, string toDate, string lotID)
        {
            return GetDataTable("SELECT_LOT_INFO_02", new string[] { string.Format("'{0}'", string.Join("','", testArea)) }, new string[] { fromDate, toDate, lotID });
        }

        public DataRow GetLotInfo(string lotSeq)
        {
            DataTable dt = GetDataTable("SELECT_LOT_INFO_03", null, new string[] { lotSeq });

            if (dt == null || dt.Rows.Count == 0)
                throw new Exception(String.Format("Not found lot Info.(lot_seq = {0})", lotSeq));

            return dt.Rows[0];
        }

        public DataRow GetLotInfo(string lotID, string testArea, string program)
        {
            DataTable dt = GetDataTable("SELECT_LOT_INFO_04", null, new string[] { lotID, testArea, program });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows[0];
        }

        /// <summary>
        /// PROGRAM 정보를 업데이트 합니다.
        /// </summary>
        public void UpdateProgram(string lotSeq, string newProgram)
        {
            ExecuteNonQuery("UPDATE_PROGRAM", null, new string[] { lotSeq, newProgram });
        }

        public void InsertData(DataTable newLotDt)
        {
            ExecuteTable(newLotDt);
        }
    }
}
