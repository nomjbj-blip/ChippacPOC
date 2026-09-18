#define WINDOWS2003
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.EnterpriseServices;
using DACrux.Base;

namespace QMS.DSL
{
#if WINDOWS2003
    [Transaction(TransactionOption.Supported, Isolation = TransactionIsolationLevel.Serializable)]
#else
    [Transaction(TransactionOption.Supported)]
#endif

    [ComVisible(true)]
    [JustInTimeActivation]
    /// <summary>
    /// Class Name : TQ_COMMON<br/>
    /// Summary    : Class To Support Dynamic Query<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-03-30<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COMMON : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COMMON()
        {
            this.InitQueryComponent("QMS.DSL.COMMON.xml");
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
                this.GetDataSet("ExcuteDynamic", new string[] { strSQL }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
