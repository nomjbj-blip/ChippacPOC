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
    public class TQ_COM_EQUIP_STATUS : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQ_COM_EQUIP_STATUS()
        {
            this.InitQueryComponent("QMS.DSL.TQ_COM_EQUIP_STATUS.xml");
        }

        #endregion

        public void InsertEquipStatus(string[] strSQL)
        {
            try
            {
                this.Execute("INSERT_EQUIP_STATUS", null, strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEquipStatus()
        {
            try
            {
                this.Execute("DELETE_EQUIP_STATUS", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectEquipStatus()
        {
            try
            {
                return this.GetDataTable("SELECT_EQUIP_STATUS", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
