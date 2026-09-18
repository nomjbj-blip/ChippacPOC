using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_INSP_SUM : Miracom.Middleware.QueryComponent
    {
        public TQD_INSP_SUM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_INSP_SUM.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertData(string stepSeq)
        {
            ExecuteNonQuery("INSERT_DATA", null, new string[] { stepSeq });
        }

        public System.Data.DataTable GetDataSheet01(
            string fromDate,
            string toDate,
            string product,
            string stepid,
            string[] lots,
            string[] wafers
            )
        {
            String lotIds = string.Format("'{0}'", string.Join("','", lots));
            String waferIds = string.Format("'{0}'", string.Join("','", wafers));
            return GetDataTable("GET_DATASHEET_01", new string[] { lotIds, waferIds }, new string[] { fromDate, toDate, product, stepid });
        }

        public DataTable GetDataSheet02(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] classnumber,
            string ordered
            )
        {
            String product = String.Format("'{0}'", String.Join("','", products));
            String stepid = String.Format("'{0}'", String.Join("','", stepids));
            String lotids = String.Format("'{0}'", String.Join("','", lots));
            String waferIds = String.Format("'{0}'", String.Join("','", wafers));
            String classNumbers = String.Format("'{0}'", String.Join("','", classnumber));
            return GetDataTable(
                "GET_DATASHEET_02",
                new string[] { 
                    product, stepid, lotids, waferIds, classNumbers, ordered
                },
                new string[] { fromDate, toDate }
                );
        }
    }
}
