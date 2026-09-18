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
    /// Class Name : VQ_COM_PRODUCT<br/>
    /// Summary    : Access for VQ_COM_PRODUCT Table<br/>
    /// Author     : Miracom YSIM<br/>
    /// First Date : 2009-05-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class VQ_COM_PRODUCT : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public VQ_COM_PRODUCT()
        {
            this.InitQueryComponent("QMS.DSL.VQ_COM_PRODUCT.xml");
        }
        
        #endregion
        public DataTable GET_CUSTOMER_LIST()
        {
            DataTable dt = null;
            try
            {
                dt = GetDataTable("GET_CUSTOMER_LIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GET_PRODUCT_LIST(string strCustomer,string strDevice)
        {
            DataTable dt = null;
            try
            {
                dt = GetDataTable("GET_PRODUCT_LIST", null, new string[] { strCustomer, strDevice });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GET_DEVICE_LIST(string strCustomer)
        {
            DataTable dt = null;
            try
            {
                dt = GetDataTable("GET_DEVICE_LIST", null, new string[] { strCustomer });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GET_LOADFROM_PRODUCT_LIST(string strCustomer)
        {
            DataTable dt = null;
            try
            {
                dt = GetDataTable("GET_LOADFROM_PRODUCT_LIST", null, new string[] { strCustomer });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
