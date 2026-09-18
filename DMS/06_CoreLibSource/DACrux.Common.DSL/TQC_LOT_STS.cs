using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.DSL
{
    public class TQC_LOT_STS : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_LOT_STS()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_LOT_STS.xml");
        }

        #endregion

        #region [ Select ]

        public DataTable SelectLotSTS(string strLotID)
        {
            return this.GetDataTable("SELECT_TQC_LOT_STS", null, new string[] { strLotID });
        }

        public DataTable SelectSysDate()
        {
            return this.GetDataTable("SELECT_SYS_DATE", null, null);
        }

        public DataTable GetLotInfo(
            string lotid
            )
        {
            return this.GetDataTable(
                "SELECT_TQC_LOT_STS",
                null,
                new string[] { lotid }
                );
        }

        public DataTable GetLotInfoLength(
           string lotid
           )
        {
            return this.GetDataTable(
                "SELECT_TQC_LOT_STS_01",
                null,
                new string[] { lotid }
                );
        }

        public DataTable GetLotAll(
           )
        {
            return this.GetDataTable(
                "SELECT_TQC_LOT_STS_ALL",
                null,
                null
                );
        }
        #endregion [ Select ]
    }
}
