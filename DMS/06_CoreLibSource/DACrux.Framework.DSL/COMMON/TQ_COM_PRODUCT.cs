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
    /// Class Name : TQ_COM_PRODUCT<br/>
    /// Summary    : Access for TQ_COM_PRODUCT Table<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-06-29<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQ_COM_PRODUCT : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_PRODUCT()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_PRODUCT.xml");
        }

        #endregion

        #region ConfirmProduct

        /// <summary>
        /// Confirm Whether Product is in DataBase
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <returns>true : Exist in DataBase</returns>
        public bool ConfirmProduct(string strFacility, string strProduct)
        {
            try
            {
                if (System.Convert.ToInt16(this.GetDataTable("ConfirmProduct", null, new string[] { strFacility, strProduct }).Rows[0][0]) > 0)
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
