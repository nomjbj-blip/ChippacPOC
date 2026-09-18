using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPFLWDEF : Miracom.Middleware.QueryComponent
    {
        public MES_MWIPFLWDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_MWIPFLWDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectFlowList(string factory)
        {
            return this.GetDataTable("SELECT_FLOW_LIST", null,  new string[] { factory });
        }

        public DataTable SelectFlowInfo(string factory, string flow)
        {
            return this.GetDataTable("SELECT_FLOW_INFO", null, new string[] { factory, flow });
        }

    }
}
