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
    /// Class Name : TQ_COM_TESTHIST<br/>
    /// Summary    : <br/>
    /// Author     : ATTI<br/>
    /// First Date : 2010-06-02<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_TESTHIST : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_TESTHIST()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_TESTHIST.xml");
        }

        #endregion

        #region Select Test history
        public DataTable SelectTestHist(string strLot, string strOper)
        {
            try
            {
                return this.GetDataTable("SELECT_TEST_LIST", null, new string[] { strLot, strOper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
