#define WINDOWS2003
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.EnterpriseServices;
using DACrux.Base;
using Miracom.Middleware;

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
    /// Class Name : TQ_USER_CONTROL<br/>
    /// Summary    : Access for Sequence DataTable<br/>
    /// Author     : Miracom AndyKuo<br/>
    /// First Date : 2009-05-21<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_ROUTING_SEQ : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_ROUTING_SEQ()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_ROUTING_SEQ.xml");
        }
        
        #endregion

        #region GetFlowSequence

        /// <summary>
        /// Get Facility List
        /// </summary>
        /// <returns>Result DataTable</returns>
        public DataTable GetFlowSequence(string strFacility,string strProduct,string strFlow)
        {
            try
            {
                return this.GetDataTable("GetFlowSequence", null, new string[] { strFacility, strProduct, strFlow });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetOperSequence

        /// <summary>
        /// Get Facility List
        /// </summary>
        /// <returns>Result DataTable</returns>
        public DataTable GetOperSequence(string strFacility,string strFlow,string strOper)
        {
            try
            {
                return this.GetDataTable("GetOperSequence", null, new string[] { strFacility, strFlow, strOper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
