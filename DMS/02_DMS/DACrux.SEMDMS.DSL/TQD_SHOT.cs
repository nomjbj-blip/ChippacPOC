using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_SHOT : Miracom.Middleware.QueryComponent
    {
        public TQD_SHOT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                //this.InitQueryComponent(connectID, "DACrux.SEMDMS.DSL.TQD_SHOT.xml");
                this.InitQueryComponent(connectID, "TQD_DEFECT.xml");
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

        public DataTable GetShotRepeatDefect(long step_seq, string product, float die_size_x, float die_size_y, float tolerance)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strStepSeq = string.Format("{0}", step_seq);
                string strDieSizeX = string.Format("{0}", die_size_x);
                string strDieSizeY = string.Format("{0}", die_size_y);

                string[] dynamic = new string[] { strDieSizeX, strTolerance, strDieSizeY, strTolerance };
                string[] para = new string[] { strStepSeq, product };

                dt = this.GetDataTable("SHOT_REPEAT_DEFECT", dynamic, para);
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

        public DataTable GetDieRepeatDefect(long step_seq, float tolerance)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strStepSeq = string.Format("{0}", step_seq);

                string[] dynamic = new string[] { strTolerance, strTolerance };
                string[] para = new string[] { strStepSeq };

                dt = this.GetDataTable("DIE_REPEAT_DEFECT", dynamic, para);
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
    } 
}
