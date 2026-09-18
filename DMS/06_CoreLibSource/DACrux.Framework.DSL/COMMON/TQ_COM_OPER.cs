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
    /// Class Name : TQ_COM_OPER<br/>
    /// Summary    : Access for TQ_COM_OPER Table<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-06-29<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_OPER : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_OPER()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_OPER.xml");
        }

        #endregion

        #region ConfirmOper

        /// <summary>
        /// Confirm Whether Operation Code is in DataBase
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strOper">Oper</param>
        /// <returns>true : Exist in DataBase</returns>
        public bool ConfirmOper(string strFacility, string strOper)
        {
            try
            {
                if (System.Convert.ToInt16(this.GetDataTable("ConfirmOper", null, new string[] { strFacility, strOper }).Rows[0][0]) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
