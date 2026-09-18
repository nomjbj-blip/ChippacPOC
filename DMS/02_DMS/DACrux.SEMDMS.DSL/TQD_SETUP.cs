
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.DSL
{
	public class TQD_SETUP  : Miracom.Middleware.QueryComponent
	{
		public TQD_SETUP()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_SETUP.xml");
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

		public void Create(string[] SetupInfo)
		{
			try
			{
                this.Execute("CREATE_SETUP", null, SetupInfo);
			}
			catch (Exception ex)
            {
				throw this.ProcessErr(ex);
			}
		}

		public long GetSetupSeq(string[] SetupInfo)
		{
			DataTable dt = null;
			try
			{
				
				dt = this.GetDataTable( "SELECT_SETUP_SEQ", null, new string[2]{SetupInfo[0],SetupInfo[2]});
				if(dt.Rows.Count == 0) return -1;
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}

			return long.Parse(dt.Rows[0][0].ToString());
		}

		public long GetSetupSeq(string SetupID)
		{
			DataTable dt = null;
			try
			{

                dt = this.GetDataTable("SELECT_SETUP_SEQ_2", null, new string[1] { SetupID });
				if(dt.Rows.Count == 0) return -1;
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
			return long.Parse(dt.Rows[0][0].ToString());
		}

		public DataTable GetRecipe(string[] setupinfo)
		{
			DataTable dt = null;
			try
			{
                dt = this.GetDataTable("GET_RECIPE", null, setupinfo);
                return dt;
            }
			catch (Exception ex)
			{
				//예외처리
				throw this.ProcessErr(ex);
			}
		}

        public DataTable GetSetupRecipe(long SetupSeq)
		{
			this.TraceStartPoint();
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable( "GET_SETUP", null,new string[] {SetupSeq.ToString()});
                return dt;
            }
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

        /// <summary>
        /// 저장된 Map의 ORGIN 인덱스 정보를 가져옵니다.
        /// </summary>
        public DataTable GetOriginDieIndex(string setupID, string stepID, DateTime setupTime)
        {
            return GetDataTable("SELECT_ORIGIN_INDEX", null, new string[] { setupID, stepID, setupTime.ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }
}
