
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_STEP : Miracom.Middleware.QueryComponent
    {
        public TQD_STEP()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_STEP.xml");
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

        public void Update(string[] StepInfo)
        {
            try
            {
                switch (StepInfo.Length)
                {
                    case 4:
                        this.Execute("UPDATE_STEP", null, StepInfo);
                        break;
                    case 5:
                        this.Execute("UPDATE_STEP2", null, StepInfo);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public long GetStepSeq(string[] StepInfo)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_STEP_SEQ", null, StepInfo); //StepInfo[1],StepInfo[5],StepInfo[2],StepInfo[8]
                if (dt.Rows.Count == 0) return -1;
                return long.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }


        public DataTable GetExistInspWaferID(string[] InspInfo)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("CHECK_EXIST_WAFERID", null, InspInfo);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetExistInspSlot(string[] InspInfo)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("CHECK_EXIST_SLOT", null, InspInfo);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
        public string GetWaferSeq(string InspInfo)
        {
            DataTable dt = null;
            try
            {

                dt = this.GetDataTable("SELECT_WAFER_SEQ", null, new string[] { InspInfo });
                return dt.Rows[0]["WAFER_SEQ"].ToString();
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetStepInfo(long step_seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STEP_INFO", null, new string[] { step_seq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        /// <summary>
        /// Step 상의 TestNo 값을 +1 해준다.
        /// </summary>
        /// <param name="strWaferSeq"></param>
        /// <param name="strStepID"></param>
        public void UpdateStepTestNo(string strWaferSeq, string strStepID)
        {
            try
            {
                this.Execute("UPDATE_STEP_TEST_ORDER", null, new string[] { strWaferSeq, strStepID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateReviewInfo(
            long stepseq,
            long waferseq,
            string imageCnt,
            string reviewEQ,
            string reviewFile,
            string reviewPath
            )
        {
            return this.ExecuteNonQuery(
                "UPDATE_STEP[REVIEW_PARSER]",
                null,
                new string[] { reviewEQ, imageCnt, reviewFile, reviewPath, stepseq.ToString(), waferseq.ToString() }
                );
        }

        public DataTable GetMigrationData(string strStarttime, string strEndTime)
        {
            DataTable dt = this.GetDataTable("SELECT_MIGRATION_DATA", null, new string[] { strStarttime, strEndTime });
            return dt;

        }

        public DataTable GetMaintData(string strStepSeq, string strWaferSeq, string sttStepID, string strSlotID)
        {
            DataTable dt = this.GetDataTable("SELECT_MAINT_DATA", null, new string[] { strStepSeq, strWaferSeq, sttStepID, strSlotID });
            return dt;

        }

        public void CreateMaintStep(string strNewStepSeq, string strWafeSeq, string strStepID, string strSlotID, string strStepSeq)
        {
            try
            {
                this.Execute("CREATE_MAINT_COPY", null, new string[] { strNewStepSeq, strWafeSeq, strStepID, strSlotID, strStepSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMaintStep(string strWafeSeq, string strStepSeq)
        {
            try
            {
                this.Execute("DELETE_MAINT_STEP", null, new string[] { strWafeSeq, strStepSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextStpeSeq()
        {
            try
            {
                DataTable dt = this.GetDataTable("SELECT_NEXT_STEP_SEQ", null, null);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferBinInfo(string[] steps, string[] lotIDs, string[] waferIDs)
        {
            string stepIDs = String.Format("'{0}'", String.Join("','", steps));
            string lotIds = String.Format("'{0}'", String.Join("','", lotIDs));
            string waferSeqs = String.Format("'{0}'", String.Join("','", waferIDs));
            return this.GetDataTable("SELECT_TST_BIN_INFO", new string[] { lotIds, waferSeqs, stepIDs }, null);
        }
    }
}
