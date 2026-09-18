using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.Generic;

namespace DACrux.Common.DSL
{
    public class TQC_CONFIG : Miracom.Middleware.QueryComponent
    {
        public TQC_CONFIG()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_CONFIG.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //#region [Common Select]
        //public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        return this.GetDataTable(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region [Common Insert]
        //public void InsertData(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        this.GetDataTable(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        return this.ExecuteNonQuery(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void InsertExecuteMultiple(string sqlName, string[,] paras)
        //{
        //    try
        //    {
        //        this.ExecuteMultiple(sqlName, null, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region [Common Update]
        //public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        this.GetDataTable(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        return this.ExecuteNonQuery(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region [Common Delete]
        //public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        this.GetDataTable(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        //{
        //    try
        //    {
        //        return this.ExecuteNonQuery(sqlName, dynamic, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        public DataTable Get(string[] cfginfo)
        {

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_VALUE", null, cfginfo);
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetCategory(string cfginfo)
        {

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_CATEGORY", null, new string[] { cfginfo });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public int SetValue(string ValueCategory, string ValueName, string Value, string ValueType, string Description)
        {
            try
            {
                return this.ExecuteNonQuery(
                    "CREATE_CONFIG_VALUE",
                    null,
                    new string[] { ValueCategory, ValueName, Value, ValueType, Description }
                    );
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public int UpdateValue(string ValueCategory, string ValueName, string Value, string ValueType, string Description)
        {
            try
            {
                return this.ExecuteNonQuery(
                    "UPDATE_CONFIG_VALUE",
                    null,
                    new string[] { Value, ValueType, Description, ValueCategory, ValueName }
                    );
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetValue()
        {
            try
            {
                return this.GetDataTable("SELECT_CONFIG_ALL", null, null);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetDefectFtp()
        {
            return this.GetDataTable(
                "SELECT_DEFECT_FTP", null, null
                );
        }

        public DataTable GetAVIImageFTPInfo()
        {
            try
            {
                return this.GetDataTable("SELECT_AVI_FTP", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapFTPInfo()
        {
            try
            {
                return this.GetDataTable("SELECT_AVI_MAP_UPLOAD", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> GetData(string category)
        {
            DataTable dt = GetDataTable("GET_CATEGORY", null, new string[] { category });

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (DataRow row in dt.Rows)
            {
                dic.Add(row["NAME"].ToString(), row["VALUE"].ToString());
            }

            return dic;
        }


        #region Global Config

        public DataTable GetConfigListAll()
        {
            try
            {
                return this.GetDataTable("SELECT_CONFIG_ALL", null, null);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetConfigListDuple(string strCategory, string strName)
        {
            try
            {
                return this.GetDataTable("SELECT_CATEGORY_DUPLE", null, new string[] { strCategory, strName });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
        

        public void UpdateConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            try
            {
                this.Execute("UPDATE_CONFIG_VALUE", null, new string[] { strValue, strType, strComment, strCategory, strName });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public void DeleteConfiagData(string strCategory, string strName)
        {
            try
            {
                this.Execute("DELETE_CONFIG_VALUE", null, new string[] { strCategory, strName });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public void InsertConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            try
            {
                this.Execute("CREATE_CONFIG_VALUE", null, new string[] { strCategory, strName, strValue, strType, strComment });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }


        #endregion Global Config
    }
}
