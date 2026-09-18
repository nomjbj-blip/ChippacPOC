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
    /// Class Name : TQ_COM_RES<br/>
    /// Summary    : Access for TQ_COM_RES Table<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-06-29<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_RES : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_RES()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_RES.xml");
        }

        #endregion

        #region ConfirmRes

        /// <summary>
        /// Confirm Whether Res ID is in DataBase
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strRes">Res (Equipment)</param>
        /// <returns>true : Exist in DataBase</returns>
        public bool ConfirmRes(string strFacility, string strRes)
        {
            try
            {
                if (System.Convert.ToInt16(this.GetDataTable("ConfirmRes", null, new string[] { strFacility, strRes }).Rows[0][0]) > 0)
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
