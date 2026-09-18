using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    public class DefectAlarm
    {
        DACrux.SEMDMS.Interface.iDefectAlarm m_OBJ;

        public DefectAlarm()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iDefectAlarm),
                        strUrl + "/DACrux.SEMDMS.BSL.DefectAlarm.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iDefectAlarm;
        }

        public DataTable GetDefectType()
        {
            return m_OBJ.GetDefectType();
        }

        public DataTable GetConfigCode(string category)
        {
            return m_OBJ.GetConfigCode(category);
        }

        public bool ExistsConfigCode(string category, string code)
        {
            return m_OBJ.ExistsConfigCode(category, code);
        }

        public void AddConfigCode(string category, string code)
        {
            m_OBJ.AddConfigCode(category, code);
        }

        public void DeleteConfigCode(string category, string code)
        {
            m_OBJ.DeleteConfigCode(category, code);
        }

        public DataTable GetAlarmUser()
        {
            return m_OBJ.GetAlarmUser();
        }

        public void DeleteAlarmUser()
        {
            m_OBJ.DeleteAlarmUser();
        }

        public void InsertAlarmUser(string factory, string[] alarmUserArr, string createUser)
        {
            m_OBJ.InsertAlarmUser(factory, alarmUserArr, createUser);
        }

        public void InsertAlarmSetup(string[,] data)
        {
            m_OBJ.InsertAlarmSetup(data);
        }

        public void DeleteAlarmSetup(string factory, string defectType, string product, string stepId)
        {
            m_OBJ.DeleteAlarmSetup(factory, defectType, product, stepId);
        }

        public DataTable GetAlarmSetup(string factory, string defectType, string product, string stepId)
        {
            return m_OBJ.GetAlarmSetup(factory, defectType, product, stepId);
        }

        public DataTable GetEquip()
        {
            return m_OBJ.GetEquip();
        }

        public DataSet GetAlarmData(string factory, string defectType, string equipId, string product, string stepId, int rowCount)
        {
            return m_OBJ.GetAlarmData(factory, defectType, equipId, product, stepId, rowCount);
        }
    }
}
