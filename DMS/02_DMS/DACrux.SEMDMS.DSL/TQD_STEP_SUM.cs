
using System;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
	public class TQD_STEP_SUM : Miracom.Middleware.QueryComponent
	{
		public TQD_STEP_SUM()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_STEP_SUM.xml");
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

		public void Create(string[] StepInfo)
		{
			try
			{
                string DynamicQuery = "CALL UPDATE_STEP_SUM('" + StepInfo[0] + "')";

                
                // 1) Nomal Query
                this.Execute("CALL_UPDATE_STEP_SUM", null, new string[] { StepInfo[0] });

                // 2) Dynamic Query
                //this.Execute("CALL_UPDATE_STEP_SUM", new string[] { StepInfo[0] }, null);

                // 3) Ful Dynamic Query
                //this.Execute("CALL_UPDATE_STEP_SUM", new string[] { DynamicQuery }, null);
			}
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}
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
		public DataTable Get(string[] StepInfo)
		{
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable( "SELECT_STEP_SUM", null, new string[1]{StepInfo[0]});
                return dt;
            }
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

		public DataTable GetStepInfo(string[] StepInfo)
		{
			this.TraceStartPoint();
			DataTable  dt = null;
			try
			{
				dt = this.GetDataTable( "SELECT_STEP_INFO", null, new string[1]{StepInfo[0]});
                return dt;
            }
			catch (Exception ex)
			{
				throw this.ProcessErr(ex);
			}
		}

        public DataTable GetStepSum(long step_seq)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_STEP_INFO", null, new string[] { step_seq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }
    }
}
