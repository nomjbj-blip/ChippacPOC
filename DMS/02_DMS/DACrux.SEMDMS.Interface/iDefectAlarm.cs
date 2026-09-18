using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    public interface iDefectAlarm
    {
        void AddConfigCode(string category, string code);
        void DeleteAlarmUser();
        void DeleteConfigCode(string category, string code);
        bool ExistsConfigCode(string category, string code);
        DataTable GetAlarmUser();
        DataTable GetConfigCode(string category);
        DataTable GetDefectType();
        void InsertAlarmUser(string factory, string[] alarmUserArr, string createUser);
        void InsertAlarmSetup(string[,] data);
        void DeleteAlarmSetup(string factory, string defectType, string product, string stepId);
        DataTable GetAlarmSetup(string factory, string defectType, string product, string step);
        DataTable GetEquip();
        DataSet GetAlarmData(string factory, string defectType, string equipId, string product, string stepId, int rowCount);
    }
}
