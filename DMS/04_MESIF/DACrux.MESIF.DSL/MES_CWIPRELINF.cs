using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_CWIPRELINF : Miracom.Middleware.QueryComponent
    {
        public MES_CWIPRELINF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_CWIPRELINF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectReelInfo(string factory, string lot_id)
        {
            return this.GetDataTable("SELECT_REEL", null, new string[] { factory, lot_id });
        }

    }

}
