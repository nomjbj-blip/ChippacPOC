
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.DSL
{
	public class TQD_PRODUCT  : Miracom.Middleware.QueryComponent
	{
		public TQD_PRODUCT()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_PRODUCT.xml");
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

		public void Create(string[] ProdInfo)
		{
			try
			{
                this.Execute("CREATE_PRODUCT", null, ProdInfo);
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}
		public DataTable GetProduct(string[] ProdInfo)
		{
			this.TraceStartPoint();
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable( "SELECT_PRODUCT", null, new string[1]{ProdInfo[0]});
                return dt;
            }
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

        public DataTable GetProduct()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("PRODUCT", null, null);
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

        public DataTable GetInfo(string Device)
        {
            try
            {
                DataTable dt = this.GetDataTable("SELECT_PRODUCT", null, new string[] { Device });

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyInsertProduct(string strProduct, string strOriProduct)
        {
            try
            {
                this.GetDataTable("CREATE_COPY_PRODUCT", null, new string[] { strProduct, strOriProduct });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

	}
}
