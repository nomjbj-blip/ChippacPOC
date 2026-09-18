using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    public class DefectAlarm : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iDefectAlarm
    {
        public static readonly string CATEGORY_MAIL = "MAIL_OPTION";
        public static readonly string MAIL_SERVER = "SERVER";
        public static readonly string MAIL_PORT = "PORT";
        public static readonly string MAIL_DEFAULT_ADDR = "DEFAULT_MAIL";

        public static readonly string NAME = "NAME";
        public static readonly string VALUE = "VALUE";

        public static readonly string LOWER = "LOWER";
        public static readonly string TARGET = "TARGET";
        public static readonly string UPPER = "UPPER";
        
        public static readonly string TOTAL_DEFECT = "TOTAL_DEFECT";
        public static readonly string RANDOM_DEFECT = "RANDOM_DEFECT";

        public const string CATEGORY_PRODUCT = "DM_PRODUCT";
        public const string CATEGORY_STEP = "DM_STEP";

        public static readonly string YES = "Y";
        public static readonly string NO = "N";


        #region Alarm Setup

        public DataTable GetDefectType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Code");
            dt.Rows.Add(TOTAL_DEFECT);
            dt.Rows.Add(RANDOM_DEFECT);
            return dt;
        }

        public DataTable GetEquip()
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            return obj.GetEquip();
        }

        public DataTable GetConfigCode(string category)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            return obj.GetConfigCode(category);
        }

        public bool ExistsConfigCode(string category, string code)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            return obj.ExistsConfigCode(category, code);
        }

        public void AddConfigCode(string category, string code)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            obj.AddConfigCode(category, code);
        }

        public void DeleteConfigCode(string category, string code)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            obj.DeleteConfigCode(category, code);
        }

        public DataTable GetAlarmSetup(string factory, string defectType, string product, string step)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            return obj.GetAlarmSetup(factory, defectType, product, step);
        }

        public DataTable GetAlarmSetup(string factory, string defectType, string product, string step, string equipId)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            return obj.GetAlarmSetup(factory, defectType, product, step, equipId);
        }

        public void DeleteAlarmSetup(string factory, string defectType, string product, string stepId)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            obj.DeleteAlarmSetup(factory, defectType, product, stepId);
        }

        public void InsertAlarmSetup(string[,] data)
        {
            TQD_ALARM_SETUP obj = new TQD_ALARM_SETUP();
            obj.InsertAlarmSetup(data);
        }
        
        #endregion

        #region Alarm User

        public DataTable GetAlarmUser()
        {
            TQD_ALARM_USER obj = new TQD_ALARM_USER();
            return obj.GetAlarmUser();
        }

        public void DeleteAlarmUser()
        {
            TQD_ALARM_USER obj = new TQD_ALARM_USER();
            obj.DeleteAlarmUser();
        }

        public void InsertAlarmUser(string factory, string[] alarmUserArr, string createUser)
        {
            TQD_ALARM_USER obj = new TQD_ALARM_USER();
            obj.InsertAlarmUser(factory, alarmUserArr, createUser);
        }
        
        #endregion

        #region Alarm Data

        public void CheckAlarm(string factory, string product, string stepId, string equipId, string stepSeq, DACrux.Data.Parser.Klarf.Wafer wafer)
        {
            // TOTAL_DEFECT 검사
            TQD_ALARM_SETUP setup = new TQD_ALARM_SETUP();
            DataTable dt = setup.GetAlarmSetup(factory, TOTAL_DEFECT, product, stepId, equipId);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal? lower = GetValue(dt.Rows[0][LOWER]);
                decimal? target = GetValue(dt.Rows[0][TARGET]);
                decimal? upper = GetValue(dt.Rows[0][UPPER]);
                int count = wafer.DefectList.Count;

                SaveAlarmData(factory, TOTAL_DEFECT, product, stepId, equipId, stepSeq, wafer.Parser.LotID, wafer.WaferID, lower, target, upper, count);
            }

            // RANDOM_DEFECT 검사
            dt = setup.GetAlarmSetup(factory, RANDOM_DEFECT, product, stepId, equipId);

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal? lower = GetValue(dt.Rows[0][LOWER]);
                decimal? target = GetValue(dt.Rows[0][TARGET]);
                decimal? upper = GetValue(dt.Rows[0][UPPER]);
                int count = wafer.DefectList.GetDefectStat().Random;

                SaveAlarmData(factory, RANDOM_DEFECT, product, stepId, equipId, stepSeq, wafer.Parser.LotID, wafer.WaferID, lower, target, upper, count);
            }
        }

        private decimal? GetValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            decimal val;

            if (decimal.TryParse(value.ToString(), out val))
                return val;
            else
                return null;
        }

        private void SaveAlarmData(string factory, string defectType, string product, string stepId, string equipId, string stepSeq, string lotID, string waferID, decimal? lower, decimal? target, decimal? upper, int count)
        {
            string alarmYN = NO;
            string alarmType = null;
            string notiUser = null;

            if (lower.HasValue && lower.Value > count)
            {
                alarmYN = YES;
                alarmType = LOWER;
            }

            if (upper.HasValue && upper.Value < count)
            {
                alarmYN = YES;
                alarmType = UPPER;
            }

            // 메일 발송
            if (alarmYN == YES)
            {
                TQD_ALARM_USER user = new TQD_ALARM_USER();
                string[] notiUserArr = user.GetAlarmUserEmail();
                notiUser = String.Join(";", notiUserArr);
                
                string server, sender;
                int port;
                GetMailConfig(out server, out port, out sender);

                string subject = String.Format("DM ALARM [{0} / {1} / {2} / {3} / {4} / {5}]", defectType, product, stepId, equipId, alarmType, waferID);
                string body = String.Format(@"
{3} 설비의 {0} 카운트가 '{10}'를 벗어났습니다. 확인 바랍니다.
 
- Defect Type = {0}
- Prodcut = {1}
- Step ID = {2}
- Equip ID = {3}
- Alarm Type = {4}
- Lot ID = {5}
- Wafer ID = {6}
- Lower = {7}
- Target = {8}
- Upper = {9}
- Defect Count = {11}
- Alarm Type = {10}
", defectType, product, stepId, equipId, alarmType, lotID, waferID, lower, target, upper, count, alarmType);

                DACrux.Base.Util.SendMail(server, port, sender, notiUserArr, subject, body);
            }

            // 데이터 INSERT
            TQD_ALARM_DATA obj = new TQD_ALARM_DATA();
            obj.InsertAlarmData(factory, defectType, product, stepId, equipId, stepSeq, lower, target, upper, alarmYN, alarmType, count, notiUser);
        }

        private void GetMailConfig(out string server, out int port, out string defaultMail)
        {
            server = defaultMail = null;
            port = 0;

            DACrux.Common.DSL.TQC_CONFIG obj = new Common.DSL.TQC_CONFIG();
            DataTable dt = obj.GetCategory(CATEGORY_MAIL);

            foreach (DataRow row in dt.Rows)
            {
                if (row[NAME].ToString() == MAIL_SERVER)
                    server = row[VALUE].ToString();
                else if (row[NAME].ToString() == MAIL_PORT)
                    port = Int32.Parse(row[VALUE].ToString());
                else if (row[NAME].ToString() == MAIL_DEFAULT_ADDR)
                    defaultMail = row[VALUE].ToString();
            }
        }

        public DataSet GetAlarmData(string factory, string defectType, string equipId, string product, string stepId, int rowCount)
        {
            TQD_ALARM_DATA data = new TQD_ALARM_DATA();
            TQD_ALARM_SETUP setup = new TQD_ALARM_SETUP();

            string[] productArr = null;
            string[] stepArr = null;

            if (!String.IsNullOrEmpty(product))
                productArr = new string[] { product };
            else
                productArr = DataTableToArray(setup.GetConfigCode(CATEGORY_PRODUCT));

            if (!String.IsNullOrEmpty(stepId))
                stepArr = new string[] { stepId };
            else
                stepArr = DataTableToArray(setup.GetConfigCode(CATEGORY_STEP));

            DataSet ds = new DataSet();

            foreach (string productItem in productArr)
            {
                foreach (string stepItem in stepArr)
                {
                    DataTable dt = data.GetAlarmData(factory, defectType, productItem, stepItem, equipId, rowCount);

                    if (dt == null || dt.Rows.Count == 0)
                        continue;

                    dt.TableName = String.Format("EQUIP = {0}, PRODUCT = {1}, STEP = {2}", equipId, productItem, stepItem);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        private string[] DataTableToArray(DataTable dt, int column = 0)
        {
            if (dt == null || dt.Rows.Count == 0 || dt.Columns.Count <= column)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = dt.Rows[i][column].ToString();

            return arr;
        }

        #endregion
    }
}
