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
    /// Class Name : TQ_COM_ROUTING<br/>
    /// Summary    : Access for TQ_COM_ROUTING_FLOW/OPER/RES Table<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-06-30<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_ROUTING : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_ROUTING()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_ROUTING.xml");
        }

        #endregion

        #region GetFlowCode

        /// <summary>
        /// Get Flow Code By Product / Operation Code
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <param name="strOper">Oper</param>
        /// <returns>Flow Code</returns>
        public string GetFlowCode(string strFacility, string strProduct, string strOper)
        {
            string strFlowCode = string.Empty;

            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GetFlowCode", null, new string[] { strFacility, strProduct, strOper });

                if (dt.Rows.Count > 0)
                    strFlowCode = dt.Rows[0][0].ToString();

                return strFlowCode;
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
    }
}
