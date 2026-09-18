using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_CXDCCUMDAT : Miracom.Middleware.QueryComponent
    {
        public MES_CXDCCUMDAT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_CXDCCUMDAT.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectCXDCCUMDAT_01(string factory, string col_set_id, string char_id, string res_id)
        {
            return this.GetDataTable("SELECT_CXDCCUMDAT_01", null, new string[] { factory, col_set_id, char_id, res_id });
        }

        public void UpdateCXDCCUMDAT(string factory, string col_set_id, string char_id, string res_id, string tran_time, string flag)
        {
            string update_time = DateTime.Now.ToString("yyyyMMddHHmm");

            this.Execute("UPDATE_CXDCCUMDAT_01", null, new string[] { factory, col_set_id, char_id, res_id, tran_time, flag, update_time});
        }
    }

}
