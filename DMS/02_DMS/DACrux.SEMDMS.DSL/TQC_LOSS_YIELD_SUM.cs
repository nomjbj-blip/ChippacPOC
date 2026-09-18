
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.DSL
{
	public class TQC_LOSS_YIELD_SUM  : Miracom.Middleware.QueryComponent
	{
		public TQC_LOSS_YIELD_SUM()
		{
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_LOSS_YIELD_SUM.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        public DataTable GetLossProspectFilterDevice(string strStartTime, string strEndTime)
        {
            return this.GetDataTable("SELECT_FILTER_DATA_DEVICE", null, new string[] { strStartTime, strEndTime});
        }

        public DataTable GetLossProspectFilter(string strStartTime, string strEndTime, string strTestArea, string strDevice)
        {
            return this.GetDataTable("SELECT_FILTER_DATA", null, new string[] {strStartTime, strEndTime, strTestArea, strDevice});
        }

        public DataTable GetProtectLossMainData(string strStartTime, string strEndTime, string strTestArea, string strDevice, string strDynamic)
        {
            return this.GetDataTable("SELECT_LOSS_MAIN_DATA", new string[] { strDynamic }, new string[] { strDevice , strTestArea, strStartTime, strEndTime });
        }

        public DataTable GetProtectLossSubData(string strTestArea, string strDevice)
        {
            return this.GetDataTable("SELECT_LOSS_SUB_DATA", null, new string[] { strDevice, strTestArea });
        }

        public void InsertMatchSum(string strWaferSeq)
        {
            this.Execute("INSERT_TQC_MATCH_SUM", null, new string[] { strWaferSeq });
        }

        public void DeleteMatchSum(string strWaferSeq)
        {
            this.Execute("DELETE_TQC_MATCH_SUM", null, new string[] { strWaferSeq });
        }
	}
}
