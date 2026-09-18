using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_DEFECT_GROUP : Miracom.Middleware.QueryComponent
    {
        public TQD_DEFECT_GROUP()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_DEFECT_GROUP.xml");
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

        public DataTable SelectGroupList()
        {
            return this.GetDataTable("GET_DEFECT_GROUP_LIST", null, null);
        }

        public DataTable SelectGroupID(string groupId)
        {
            return this.GetDataTable("GET_DEFECT_GROUP_INFO", null, new string[] { groupId });
        }

        public object ExistsDefectGroup(string groupId)
        {
            return this.ExecuteScalar("EXIST_DEFECT_GROUP", null, new string[] { groupId });
        }

        public int InsertGroup(string groupId, string groupName, string deleteFlag, string userID)
        {
            return this.ExecuteNonQuery("INSERT_DEFECT_GROUP", null, new string[] { groupId, groupName, deleteFlag, userID });
        }

        public int UpdateGroup(string groupId, string groupName, string deleteFlag, string userID)
        {
            return this.ExecuteNonQuery("UPDATE_DEFECT_GROUP", null, new string[] { groupId, groupName, deleteFlag, userID });
        }

        public int DeleteGroup(string groupId)
        {
            return this.ExecuteNonQuery("DELETE_DEFECT_GROUP", null, new string[] { groupId });
        }
    }
}
