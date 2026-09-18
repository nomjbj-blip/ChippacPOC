
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.DSL
{
	public class TQD_LOT  : Miracom.Middleware.QueryComponent
	{
		public TQD_LOT()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_LOT.xml");
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

		public void Create(string[] LotInfo)
		{
			try
			{
                this.Execute("CREATE_LOT", null, LotInfo);
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

		public long GetLotSeq(string[] LotInfo)
		{
			DataTable dt = null;
			try
			{
				
				dt = this.GetDataTable( "SELECT_LOT_SEQ", null, new string[2]{LotInfo[1],LotInfo[0]});
				if(dt.Rows.Count == 0) return -1;
                return long.Parse(dt.Rows[0]["LOT_SEQ"].ToString());
            }
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

		public DataTable FindLot(string[] LotInfo)
		{
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable( "FIND_LOT", null, LotInfo);
				return dt;
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

        public DataTable GetLotData(string strProduct, string strLotID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_LOT_SEQ", null, new string[] {strProduct, strLotID});
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetLotDataSeq(string strLotID, string strProduct)
        {
            try
            {
                return this.GetDataTable("SELECT_LOT_DATA", null, new string[] { strLotID, strProduct });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
	}
}
