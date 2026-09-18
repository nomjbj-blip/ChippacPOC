using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_ALARM_DATA : Miracom.Middleware.QueryComponent
    {
        public TQD_ALARM_DATA()
		{
            string connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
                throw new Exception("The connect ID nothing. Please, check app.config.");

            this.InitQueryComponent(connectID, "TQD_ALARM_DATA.xml");
		}

        public void InsertAlarmData(string factory, string defectType, string product, string stepId, string equipId,
            string stepSeq, decimal? lower, decimal? target, decimal? upper, string alarmYN, string alarmType,
            int defectCount, string notiUser)
        {
            Execute("INSERT_ALARM_DATA", null, new string[]
                {
                    factory,
                    defectType,
                    product,
                    stepId,
                    equipId,
                    stepSeq,
                    lower.HasValue ? lower.Value.ToString() : null,
                    target.HasValue ? target.Value.ToString() : null,
                    upper.HasValue ? upper.Value.ToString() : null,
                    alarmYN,
                    alarmType,
                    defectCount.ToString(),
                    notiUser
                });
        }

        public DataTable GetAlarmData(string factory, string defectType, string product, string stepId, string equipId, int rowCount)
        {
            return GetDataTable("SELECT_ALARM_DATA_01", null, new string[] { factory, defectType, product, stepId, equipId, rowCount.ToString() });
        }
    }
}
