using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQ_COMMON<br/>
    /// Summary    : Class To Support Dynamic Query<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-03-30<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_COMMON : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_COMMON()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "EMS.PKG.COMMON.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //this.InitQueryComponent("DACrux.COMMON.xml");
        }

        #endregion

        #region GetDynamic

        /// <summary>
        /// Get Dynamic DSL (For Select Query)
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <returns>Result Data Table</returns>
        public DataTable GetDynamic(string strSQL)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GetDynamic", new string[] { strSQL }, null);
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

        #endregion

        #region ExcuteDynamic

        /// <summary>
        /// Excute Dynamic DSL (For Insert, Delete, Update Query)
        /// </summary>
        /// <param name="strSQL">SQL</param>
        public void ExcuteDynamic(string strSQL)
        {
            try
            {
                this.GetDataTable("ExcuteDynamic", new string[] { strSQL }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
