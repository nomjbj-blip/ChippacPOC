using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class TQP_CHANGE_HIS : Miracom.Middleware.QueryComponent
    {
        public TQP_CHANGE_HIS()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_CHANGE_HIS.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertData(string factory, string testArea, string product, string program, string lotID, string waferID,
            string tranUser, string tranUserIP, string category, string tranType, int tranCount, string prevValue, string currValue, string userComment)
        {
            Execute("INSERT_DATA", null, new string[] { factory, testArea, product, program, lotID, waferID, tranUser, tranUserIP, category, 
                tranType, tranCount.ToString(), prevValue, currValue, userComment  });
        }

        public DataTable GetData(string factory, string testArea, string fromDate, string toDate)
        {
            return GetDataTable("SELECT_DATA", null, new string[] { factory, testArea, fromDate, toDate });
        }
    }
}
