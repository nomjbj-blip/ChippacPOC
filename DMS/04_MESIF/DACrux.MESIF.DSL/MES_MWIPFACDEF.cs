using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPFACDEF : Miracom.Middleware.QueryComponent
    {
        public MES_MWIPFACDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_MWIPFACDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectFactoryList()
        {
            return this.GetDataTable("SELECT_FACTORY_LIST", null, null);
        }

        public DataTable SelectFactoryInfo(string factory)
        {
            return this.GetDataTable("SELECT_FACTORY_INFO", null, new string[] { factory });
        }
    }
}
