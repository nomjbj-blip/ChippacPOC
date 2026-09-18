using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_DEFECT_TYPE : Miracom.Middleware.QueryComponent
    {
        public TQD_DEFECT_TYPE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_DEFECT_TYPE.xml");
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

        //SELECT DEFECT LIST  ---  not use
        public DataTable SelectDefectList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_DEFECT_BY_TYPE", null, null);
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

        public DataTable SelectDefectCode()
        {
            return GetDataTable(
                "SELECT_CODE_NAME",
                null,
                null
                );
        }

        public DataTable GetDefectListByGroup(string groupId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DEFECT_BY_GROUP", null, new string[] { groupId });
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

        //--

        public object ExistsDefectType(
            string classnumber
            )
        {
            return ExecuteScalar(
                "EXISTS_DEFECT_TYPE",
                null,
                new string[] { classnumber }
                );
        }

        //--

        public int InsertDefectByType(
            string classnumber,
            string name,
            string desc,
            string groupid,
            string defectcolor,
            string cid,
            string uid
            )
        {
            return this.ExecuteNonQuery(
                "INSERT_DEFECT_BY_TYPE",
                null,
                new string[] { classnumber, name, desc, groupid, "N", defectcolor, cid, uid }
                );
        }

        //--

        public int UpdateDefectByType(
            string name,
            string desc,
            string classnumber
            )
        {
            return this.ExecuteNonQuery(
                "UPDATE_DEFECT_BY_TYPE",
                null,
                new string[] { name, desc, classnumber }
                );
        }

        //--

        public int UpdateDefectByType(
            string name,
            string desc,
            string groupid,
            string color,
            string classnumber,
            string userid
            )
        {
            return this.ExecuteNonQuery(
                "UPDATE_DEFECT_TYPE",
                null,
                new string[] { name, desc, groupid, color, classnumber, userid }
                );
        }

        //--

        public int DeleteDefectType(string classnumber)
        {
            return this.ExecuteNonQuery(
                "DELETE_DEFECT_BY_TYPE",
                null,
                new string[] { classnumber }
                );
        }
        //--

        public DataTable GetClassList(bool bClass)
        {
            if (bClass)
            {
                return this.GetDataTable("SELECT_CODE_LIST", null, null);
            }
            else
            {
                return this.GetDataTable("SELECT_STATS_CODE_LIST", null, null);
            }
        }
    }
}
