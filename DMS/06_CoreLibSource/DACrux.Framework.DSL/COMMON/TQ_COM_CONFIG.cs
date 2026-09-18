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
    /// Class Name : TQ_COM_CONFIG<br/>
    /// Summary    : Access for TQ_COM_CONFIG Table<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-06-29<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_CONFIG : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_CONFIG()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_CONFIG.xml");
        }

        #endregion

        #region GetConfigValue

        /// <summary>
        /// Get Config Value 
        /// </summary>
        /// <param name="strCategory">Category String</param>
        /// <param name="strName">Item Name</param>
        /// <returns>Result Data Table</returns>
        public string GetConfigValue(string strCategory, string strName)
        {
            string strReturn = string.Empty;

            try
            {
                strReturn = this.GetDataTable("GET_CONFIG_VALUE", null, new string[] { strCategory, strName }).Rows[0][0].ToString();
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetConfigList

        /// <summary>
        /// Get Config List of Selected Category 
        /// </summary>
        /// <param name="strCategory">Category String</param>
        /// <returns>Result Data Table</returns>
        public DataTable GetConfigList(string strCategory)
        {
            try
            {
                return this.GetDataTable("GET_CONFIG_LIST", null, new string[] { strCategory });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetSummaryTolerance

        /// <summary>
        /// Get Summary Tolerance 
        /// </summary>
        /// <returns>Result Data Table</returns>
        public DataTable GetSummaryTolerance()
        {
            try
            {
                return this.GetDataTable("GetSummaryTolerance", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetHoldCodeByCode

        /// <summary>
        /// Get Hold Code By Code
        /// </summary>
        /// <returns>Hold Code</returns>
        public DataTable GetHoldCodeByCode( string strCode)
        {
            try
            {
                return this.GetDataTable("GET_HOLD_CODE_BY_CODE", null, new string[] { strCode });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
