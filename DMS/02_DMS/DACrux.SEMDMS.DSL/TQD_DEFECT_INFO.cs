using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;


namespace DACrux.SEMDMS.DSL
{
    public class TQD_DEFECT_INFO : Miracom.Middleware.QueryComponent
    {
        #region  "TQD_DEFECT_INFO config"
        public TQD_DEFECT_INFO()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_DEFECT_INFO.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

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

        #region  "Create Defect By Die (Defect/Die 1 by 1)"
        public void CreateDefectByDie(string stepSeq, string lotSeq, string lotId, string waferSeq, string waferId, string xIndex, string yIndex, string classnumber)
        {
            try
            {
                this.Execute("CREATE_DEFECT_BY_DIE", null, new string[] { stepSeq, lotSeq, lotId, waferSeq, waferId, xIndex, yIndex, classnumber, "MAP", "", "", "", "" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  "Select Fail Count"
        public DataTable SelectFailCount(string stepSeq, string lotSeq, string lotId, string waferSeq, string waferId)
        {
            try
            {
                return this.GetDataTable("SELECT_FAIL_COUNT", null, new string[] { stepSeq, lotSeq, lotId, waferSeq, waferId });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
        #endregion

        #region  "Select FailBin Summary Data By Wafer"
        public DataTable SelectBinSumByWafer(string stepSeq, string lotSeq, string lotId, string waferSeq, string waferId)
        {
            try
            {
                return this.GetDataTable("SELECT_FAIL_SUMMARY_BY_WAFER", null, new string[] { stepSeq, lotSeq, lotId, waferSeq, waferId });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
        #endregion
    }
}
