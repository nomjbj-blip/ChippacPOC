using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_ALARM_SETUP : Miracom.Middleware.QueryComponent
    {
        public TQD_ALARM_SETUP()
		{
            string connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
                throw new Exception("The connect ID nothing. Please, check app.config.");

            this.InitQueryComponent(connectID, "TQD_ALARM_SETUP.xml");
		}

        public DataTable GetConfigCode(string category)
        {
            return GetDataTable("SELECT_CONFIG_CODE", null, new string[] { category });
        }

        public DataTable GetEquip()
        {
            return GetDataTable("SELECT_EQUIP", null, null);
        }

        public bool ExistsConfigCode(string category, string code)
        {
            object obj = ExecuteScalar("EXISTS_CONFIG_CODE", null, new string[] { category, code });

            if (obj == null || obj == DBNull.Value)
                return false;

            int num;

            if (!Int32.TryParse(obj.ToString(), out num))
                return false;

            return num > 0;
        }

        public void AddConfigCode(string category, string code)
        {
            Execute("INSERT_CONFIG_CODE", null, new string[] { category, code });
        }

        public void DeleteConfigCode(string category, string code)
        {
            Execute("DELETE_CONFIG_CODE", null, new string[] { category, code });
        }

        public DataTable GetAlarmSetup(string factory, string defectType, string product, string step)
        {
            return GetDataTable("SELECT_ALARM_SETUP", null, new string[] { factory, defectType, product, step });
        }

        public DataTable GetAlarmSetup(string factory, string defectType, string product, string step, string equiipId)
        {
            return GetDataTable("SELECT_ALARM_SETUP_01", null, new string[] { factory, defectType, product, step, equiipId });
        }

        public void DeleteAlarmSetup(string factory, string defectType, string product, string stepId)
        {
            Execute("DELETE_ALARM_SETUP", null, new string[] { factory, defectType, product, stepId });
        }

        public void InsertAlarmSetup(string[,] data)
        {
            ExecuteMultiple("INSERT_ALARM_SETUP", data);
        }
    }
}
