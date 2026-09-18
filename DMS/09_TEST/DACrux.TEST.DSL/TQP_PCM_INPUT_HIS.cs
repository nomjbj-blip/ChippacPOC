using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class TQP_PCM_INPUT_HIS : Miracom.Middleware.QueryComponent
    {
        public TQP_PCM_INPUT_HIS()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQP_PCM_INPUT_HIS.xml");
        }

        public void InsertData(string factory, string equipID, string probeCard, string oper, string inputUserID)
        {
            Execute("INSERT_DATA", null, new string[] { factory, equipID, probeCard, oper, inputUserID });
        }

        public DataTable GetData(string factory, string equipID, string fromDate, string toDate)
        {
            string query = String.Empty;

            if (!String.IsNullOrEmpty(equipID) && !String.Equals(equipID, "ALL"))
                query = String.Format(" AND EQUIP_ID IN ('{0}')", equipID);

            return GetDataTable("SELECT_DATA", new string[] { query }, new string[] { factory, fromDate, toDate });
        }
    }
}
