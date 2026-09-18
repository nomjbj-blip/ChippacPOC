using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;

namespace DACrux.SEMDMS.DSL
{
	public class TQD_CONFIG : Miracom.Middleware.QueryComponent
	{
		public TQD_CONFIG()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_CONFIG.xml");
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

		public DataTable Get(string[] cfginfo)
		{

            DataTable dt = null;
			try
			{
				dt = this.GetDataTable( "GET_VALUE", null, cfginfo);
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
				dt = this.GetDataTable( "GET_CATEGORY", null, new string[] {cfginfo});
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
    }
}
