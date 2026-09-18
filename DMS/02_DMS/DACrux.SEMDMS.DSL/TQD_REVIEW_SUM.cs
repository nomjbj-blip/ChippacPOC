using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_REVIEW_SUM : Miracom.Middleware.QueryComponent
    {
        public TQD_REVIEW_SUM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_REVIEW_SUM.xml");
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

        public void DeleteData(string stepSeq)
        {
            ExecuteNonQuery("DELETE_DATA", null, new string[] { stepSeq });
        }

        /// <summary>
        /// Defect Count
        /// </summary>
        public DataTable GetDataSheet01(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] xItem,
            string[] pivotValue
            )
        {
            return GetDataTable(
                "GET_DATASHEET_01",
                new string[] { 
                    (xItem == null || xItem.Length <= 0) ? String.Empty: string.Format("{0},", string.Join(", ", xItem)),
                    string.Format("'{0}'", string.Join("','", products)), 
                    string.Format("'{0}'", string.Join("','", stepids)), 
                    string.Format("'{0}'", string.Join("','", lots)), 
                    string.Format("'{0}'", string.Join("','", wafers)),
                    string.Format("{0}", string.Join(", ", pivotValue)), 
                    string.Format("'{0}'", string.Join("','", products)), 
                    string.Format("'{0}'", string.Join("','", stepids)), 
                    string.Format("'{0}'", string.Join("','", lots)), 
                    string.Format("'{0}'", string.Join("','", wafers))
                },
                new string[] { fromDate, toDate }
                );
        }

        /// <summary>
        /// Defective Die 
        /// </summary>
        public DataTable GetDataSheet02(
            string fromDate, 
            string toDate, 
            string[] products, 
            string[] stepids, 
            string[] lots, 
            string[] wafers, 
            string[] xItem, 
            string[] pivotValues
            )
        {
            return GetDataTable(
                "GET_DATASHEET_02",
                new string[] { 
                    (xItem == null || xItem.Length <= 0) ? String.Empty: string.Format("{0},", string.Join(", ", xItem)),
                    string.Format("'{0}'", string.Join("','", products)), 
                    string.Format("'{0}'", string.Join("','", stepids)), 
                    string.Format("'{0}'", string.Join("','", lots)), 
                    string.Format("'{0}'", string.Join("','", wafers)),
                    string.Format("{0}", string.Join(", ", pivotValues)), 
                    string.Format("'{0}'", string.Join("','", products)), 
                    string.Format("'{0}'", string.Join("','", stepids)), 
                    string.Format("'{0}'", string.Join("','", lots)), 
                    string.Format("'{0}'", string.Join("','", wafers))
                },
                new string[] { fromDate, toDate }
                );
        }
    }
}
