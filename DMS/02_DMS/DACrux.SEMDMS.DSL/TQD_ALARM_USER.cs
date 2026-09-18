using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_ALARM_USER : Miracom.Middleware.QueryComponent
    {
        enum InsertCol { Factory, AlarmUser, CreateUser }

        public TQD_ALARM_USER()
		{
            string connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
                throw new Exception("The connect ID nothing. Please, check app.config.");

            this.InitQueryComponent(connectID, "TQD_ALARM_USER.xml");
		}

        public DataTable GetAlarmUser()
        {
            return GetDataTable("SELECT_ALARM_USER", null, null);
        }

        public string[] GetAlarmUserEmail()
        {
            DataTable dt = GetDataTable("SELECT_ALARM_MAIL", null, null);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public void DeleteAlarmUser()
        {
            Execute("DELETE_ALARM_USER", null, null);
        }

        public void InsertAlarmUser(string factory, string[] alarmUserArr, string createUser)
        {
            if (alarmUserArr == null)
                return;

            int r = alarmUserArr.Length;
            int c = Enum.GetNames(typeof(InsertCol)).Length;

            if (r == 0 || c == 0)
                return;

            string[,] arr = new string[r, c];

            for (int i = 0; i < alarmUserArr.Length; i++)
            {
                arr[i, (int)InsertCol.Factory] = factory;
                arr[i, (int)InsertCol.AlarmUser] = alarmUserArr[i];
                arr[i, (int)InsertCol.CreateUser] = createUser;
            }

            ExecuteMultiple("INSERT_ALARM_USER", arr);
        }
    }
}
