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
    public class TQ_COM_BINDESC : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_BINDESC()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_BINDESC.xml");
        }

        #endregion

        #region Get Bin Description
        public DataTable GetBinDesc(string strFacility, string strPartID, string strFullPartID, string strHbin, string strBin)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DESC", null, new string[] { strFacility, strPartID, strFullPartID, strHbin, strBin });
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

        #region Get Bin Description
        public DataTable GetBinDesc(string strPartID, string strFullPartID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DESC2", null, new string[] { strPartID, strFullPartID });
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

        #region Get Device List
        public DataTable GetDeviceList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DEVICE_LIST", null, null);
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

        #region Get Producr List
        public DataTable GetProductList(string strDvc)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PRODUCT_LIST", null, new string[] {strDvc});
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


        public DataTable GetDistinctDevice(string strCustomer, string strStartDate, string strEndDate)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DISTINCT_DEVICE", null, new string[] { strCustomer, strStartDate, strEndDate});
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

        public DataTable GetDistinctDevice2(string strCustomer, string strStartDate, string strEndDate)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DISTINCT_DEVICE2", null, new string[] { strCustomer, strStartDate, strEndDate });
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

        public DataTable GetBinSpec(string strPartID, string strFullPartID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_SPEC", null, new string[] { strPartID, strFullPartID });
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

        #region insert bindesc
        public void InsertBinDesc(string strPartID, string strFullPartID, string strBinGroup, string strBin, string strBinDesc)
        {
            try
            {
                this.Execute("INSERT_BINDESC", null, new string[] { strPartID, strFullPartID, strBinGroup, strBin, strBinDesc });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region delete Function
        public void DeleteBinDesc(string strPartID, string strFullPartID, string strBinGroup, string strBin)
        {
            try
            {
                this.Execute("DELETE_BINDESC", null, new string[] { strPartID, strFullPartID, strBinGroup, strBin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region update Function
        public void UpdateBinDesc(string strPartID, string strFullPartID, string strBinGroup, string strBin, string strBinDesc)
        {
            try
            {
                this.Execute("UPDATE_BINDESC", null, new string[] { strPartID, strFullPartID, strBinGroup, strBin, strBinDesc });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public DataTable GetBinList(string strPartID, string strFullPartID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_LIST", null, new string[] { strPartID, strFullPartID });
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
    }
}
