using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Common.DSL
{
    public class TQC_SERVICE_LOG : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_SERVICE_LOG()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_SERVICE_LOG.xml");
        }

        #endregion

        #region INSERT

        public void InsertData(string[,] arr)
        {
            ExecuteMultiple("INSERT_DATA", arr);
        }

        public void InsertData(string FACTORY, string SERVICE_NAME, string TRAN_KEY,
            string SERVER_NAME, string ACTION, string EQUIP_ID, string LOT_ID,
            string WAFER_ID, string HANDLER, string MESSAGE, string DETAIL_MSG)
        {
            ExecuteNonQuery("INSERT_DATA", null, new string[] 
            {
                FACTORY,
                SERVICE_NAME,
                TRAN_KEY,
                SERVER_NAME,
                ACTION,
                EQUIP_ID,
                LOT_ID,
                WAFER_ID,
                HANDLER,
                MESSAGE,
                DETAIL_MSG
            });
        }

        #endregion
    }
}
