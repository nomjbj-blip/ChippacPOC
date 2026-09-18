using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.SEMDMS.DSL
{
    public class TQC_LOT_HIS : Miracom.Middleware.QueryComponent
    {
        public TQC_LOT_HIS()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_LOT_HIS.xml");
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


        public DataTable GetTrendItemList(string strItemName, string strQuery, string strStartTime, string strEndTime)
        {
            try
            {
                return this.GetDataTable("SELECT_TREND_ITEM", new string[] { strItemName, strQuery, strItemName, strItemName }, new string[] { strStartTime, strEndTime });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTrendItemListStep(string strQuery, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_ITEM_STEP", new string[] { strQuery }, new string[] { strStartTime, strEndTime });
        }

        public DataTable GetTrendReportReviewToDefect(string strItem, string strEquipment, string strFilter, string strPivotValue, string strGroup, string strOrder, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_01", new string[] { strItem, strFilter, strPivotValue, strGroup, strOrder }, new string[] { strStartTime, strEndTime, strEquipment });
        }

        public DataTable GetTrendReportReviewToDefectRaw(string strEquipment, string strFilter, string strPivotValue, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_RAW_01", new string[] { strFilter, strPivotValue }, new string[] { strStartTime, strEndTime, strEquipment });
        }

        public DataTable GetTrendReportReviewToDefectiveDie(string strItem, string strEquipment, string strFilter, string strPivotValue, string strGroup, string strOrder, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_03", new string[] { strItem, strFilter, strPivotValue, strGroup, strOrder }, new string[] { strStartTime, strEndTime, strEquipment });
        }

        public DataTable GetTrendReportReviewToDefectiveDieRaw(string strEquipment, string strFilter, string strPivotValue, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_RAW_03", new string[] { strFilter, strPivotValue }, new string[] { strStartTime, strEndTime, strEquipment });
        }

        public DataTable GetTrendReportInspect(string strItem, string strEquipment, string strFilter, string strGroup, string strOrder, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_02", new string[] { strItem, strFilter, strGroup, strOrder }, new string[] { strStartTime, strEndTime, strEquipment });
        }

        public DataTable GetTrendReportInspectRaw(string strEquipment, string strFilter, string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_TREND_REVIEW_RAW_02", new string[] { strFilter }, new string[] { strStartTime, strEndTime, strEquipment });
        }
    }
}
