using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Framework.DSL
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
            ExecuteMultiple("INSERT_DATA_01", arr);
        }

        public void InsertData(string FACTORY, string SERVICE_NAME, string TRAN_KEY,
            string SERVER_NAME, string ACTION, string FILE_NAME, string EQUIP_ID, 
            string LOT_ID, string WAFER_ID, string HANDLER, string MESSAGE, string DETAIL_MSG)
        {
            ExecuteNonQuery("INSERT_DATA_01", null, new string[] 
            {
                FACTORY,
                SERVICE_NAME,
                TRAN_KEY,
                SERVER_NAME,
                ACTION,
                FILE_NAME,
                EQUIP_ID,
                LOT_ID,
                WAFER_ID,
                HANDLER,
                MESSAGE,
                DETAIL_MSG
            });
        }

        #endregion

        #region [ Select ]
        public string[] GetDataServiceName()
        {
            DataTable dt = this.GetDataTable("GET_SERVICE_NAME", null, null);
            string[] returnVal = new string[dt.Rows.Count];
            for (int idx = 0; idx < dt.Rows.Count; idx++)
            {
                returnVal[idx] = dt.Rows[idx]["SERVICE_NAME"].ToString();
            }

            return returnVal;
        }

        public string[] GetDataServiceAction()
        {
            DataTable dt = this.GetDataTable("GET_SERVICE_ACTION", null, null);
            string[] returnVal = new string[dt.Rows.Count];
            for (int idx = 0; idx < dt.Rows.Count; idx++)
            {
                returnVal[idx] = dt.Rows[idx]["ACTION"].ToString();
            }

            return returnVal;
        }

        public DataTable GetDataServiceList(
            DateTime dtStart,
            DateTime dtEnd,
            string factory,
            string[] names,
            string[] actions,
            string equipid,
            string lotid
            )
        {
            StringBuilder sbQuery = new StringBuilder();
            if (names != null && names.Length > 0)
                sbQuery.AppendLine(string.Format("  AND SERVICE_NAME IN ('{0}')", string.Join("','", names)));

            if (actions != null && actions.Length > 0)
                sbQuery.AppendLine(string.Format("  AND ACTION IN ('{0}')", string.Join("','", actions)));

            if (!string.IsNullOrEmpty(equipid))
                sbQuery.AppendLine(string.Format("  AND EQUIP_ID LIKE '{0}%'", equipid));

            if (!string.IsNullOrEmpty(lotid))
                sbQuery.AppendLine(string.Format("  AND LOT_ID LIKE '{0}%'", lotid));

            return this.GetDataTable(
                "GET_SERVICE_LIST",
                new string[] { sbQuery.ToString() },
                new string[] { dtStart.ToString("yyyyMMddHHmmss"), dtEnd.ToString("yyyyMMddHHmmss"), factory }
                );
        }

        #endregion [ Select ]
    }
}
