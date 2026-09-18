using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;

namespace DACrux.Data.Parser
{
    /// <summary>
    /// CP Parser
    /// </summary>
    public class ParserPcm_TffData : ParserPcm, ISaveFile
    {
        public static readonly string DATE_FORMAT = "MM/dd/yyyy HH:mm:ss";

        public static readonly string KEY_COLUMN = "KEY";
        public static readonly string VALUE_COLUMN = "VALUE";

        public ParserPcm_TffData(string fileName)
            : base(fileName)
        {
            try
            {
                DieDataList = new List<CpDieData>();

                string text = ParsingUtil.FileToString(fileName);
                string[] arr = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None);

                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                string currCategory = null;

                foreach (string line in arr)
                {
                    if (String.IsNullOrEmpty(line.Trim()))
                        continue;

                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currCategory = line;
                        dic.Add(currCategory, new List<string>());
                    }
                    else if (!String.IsNullOrEmpty(currCategory))
                    {
                        dic[currCategory].Add(line);
                    }
                }

                List<string> list = dic["[Format]"];
                TFVersion = GetValue(list, "TFversion");

                list = dic["[Lot]"];
                LotID = GetValue(list, "lot");
                ProgramName = GetValue(list, "program");
                Device = GetValue(list, "device");
                TestArea = GetValue(list, "testArea");
                //TestArea = GetValue(list, "facility");

                list = dic["[Wafer]"];

                WaferID = GetValue(list, "waferid");
                StartTime = GetDateTime(GetValue(list, "startTime"));
                EndTime = GetDateTime(GetValue(list, "endTime"));
                Operator = GetValue(list, "operator");
                EquipID = GetValue(list, "tester");
                Prober = GetValue(list, "prober");
                ProbeCard = GetValue(list, "probeCard");
                Notch = GetValue(list, "waferFlat");

                switch (Notch)
                {
                    case "0":
                        Notch = "0";
                        break;
                    case "1":
                        Notch = "90";
                        break;
                    case "2":
                        Notch = "180";
                        break;
                    case "3":
                        Notch = "270";
                        break;
                    default:
                        Notch = "0";
                        break;
                }

                WaferDiameter = GetInt(GetValue(list, "waferDiam"));
                MaxX = GetInt(GetValue(list, "maxX"));
                MaxY = GetInt(GetValue(list, "maxY"));

                ChipDataDef = dic["[ChipDataDef]"].ToArray();

                for (int i = 0; i < ChipDataDef.Length; i++)
                    ChipDataDef[i] = ChipDataDef[i].Split(':')[0];

                if (Array.IndexOf(ChipDataDef, CpTestData.X_INDEX) < 0)
                    throw new Exception(String.Format("데이터에 {0}값이 없습니다.", CpTestData.X_INDEX));

                if (Array.IndexOf(ChipDataDef, CpTestData.Y_INDEX) < 0)
                    throw new Exception(String.Format("데이터에 {0}값이 없습니다.", CpTestData.Y_INDEX));

                list = dic["[ChipDataVal]"];

                foreach (string data in list)
                {
                    CpDieData dieData = new CpDieData();
                    string[] dataArr = data.Split(',');

                    for (int i = 0; i < ChipDataDef.Length; i++)
                        dieData.Add(new CpTestData(ChipDataDef[i], dataArr[i]));

                    // 값 정렬
                    dieData.Sort();

                    DieDataList.Add(dieData);
                }

                if (DieDataList.Count > 0 && DieDataList[0].Count > 0)
                {
                    // CpTestData 의 IComparer 를 이용하여 ChipDataDef 배열도 같은 순서가 되도록 정렬한다.
                    Array.Sort(ChipDataDef, DieDataList[0][0]);
                }
            }
            catch (System.IO.IOException ioex)
            {
                //IO Error 의 경우 삭제 하지 않고 다음 주기에 다시 작업 한다.
                ErrorFlag = true;
                ErrorBackupFlag = false;
                ErrorMessage = ioex.Message + Environment.NewLine + ioex.StackTrace;
            }
            catch (Exception ex)
            {
                //Parsing Error 발생 시 Error Backup 에 넣는다.
                ErrorFlag = true;
                ErrorBackupFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        public void SaveFile(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Format]");
            sb.AppendLine(String.Format("TFversion,N,{0}", TFVersion));

            sb.AppendLine("[Lot]");
            sb.AppendLine(String.Format("lot,A,{0}", LotID));
            sb.AppendLine(String.Format("program,A,{0}", ProgramName));
            sb.AppendLine(String.Format("device,A,{0}", Device));
            sb.AppendLine(String.Format("testArea,A,{0}", TestArea));
            sb.AppendLine(String.Format("facility,A,{0}", Facility));

            sb.AppendLine("[Wafer]");
            sb.AppendLine(String.Format("waferid,A,{0}", WaferID));
            sb.AppendLine(String.Format("startTime,A,{0}", DateTimeToString(StartTime)));
            sb.AppendLine(String.Format("endTime,A,{0}", DateTimeToString(EndTime)));
            sb.AppendLine(String.Format("operator,A,{0}", Operator));
            sb.AppendLine(String.Format("tester,A,{0}", EquipID));
            sb.AppendLine(String.Format("prober,A,{0}", Prober));
            sb.AppendLine(String.Format("probeCard,A,{0}", ProbeCard));
            sb.AppendLine(String.Format("waferDiam,A,{0}", WaferDiameter));
            sb.AppendLine(String.Format("maxX,A,{0}", MaxX));
            sb.AppendLine(String.Format("maxY,A,{0}", MaxY));

            sb.AppendLine("[ChipDataDef]");
            foreach (string dataName in ChipDataDef)
            {
                sb.AppendLine(String.Format("{0}:INT,,,,,,", dataName));
            }

            sb.AppendLine("[ChipDataVal]");
            foreach (CpDieData die in DieDataList)
            {
                sb.AppendLine(String.Join(",", die.ToStringArray()));
            }

            sb.AppendLine("[End]");

            File.WriteAllText(fileName, sb.ToString());
        }

        private string GetValue(List<string> list, string startVal)
        {
            foreach (string line in list)
            {
                if (line.StartsWith(startVal))
                {
                    string[] arr = line.Split(',');

                    if (arr.Length >= Enum.GetNames(typeof(CpDataOrder)).Length)
                        return arr[(int)CpDataOrder.Value];
                }
            }

            return null;
        }

        /// <summary>
        /// 정보에 관련된 데이터를 DataTable로 표현합니다.
        /// </summary>
        public DataTable GetInformationToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(KEY_COLUMN, typeof(string));
            dt.Columns.Add(VALUE_COLUMN, typeof(string));

            Type thisType = GetType();

            foreach (PropertyInfo prop in thisType.GetProperties())
            {
                Type dataType = prop.PropertyType;

                if (prop.DeclaringType != thisType)
                    continue;

                if (dataType == typeof(string))
                    dt.Rows.Add(prop.Name, prop.GetValue(this, null));
                else if (dataType == typeof(int))
                    dt.Rows.Add(prop.Name, prop.GetValue(this, null).ToString());
                else if (dataType == typeof(DateTime))
                    dt.Rows.Add(prop.Name, DateTimeToString((DateTime)prop.GetValue(this, null)));
            }

            return dt;
        }

        private string DateTimeToString(DateTime dt)
        {
            return String.Format("{0:00}/{1:00}/{2} {3:00}:{4:00}:{5:00}", dt.Month, dt.Day, dt.Year, dt.Hour, dt.Minute, dt.Second);
        }

        /// <summary>test
        /// Die 데이터를 DataTable로 표현합니다.
        /// </summary>
        public DataTable GetDieDataToDataTable()
        {
            DataTable dt = new DataTable();

            foreach (string name in ChipDataDef)
                dt.Columns.Add(name, typeof(string));

            foreach (CpDieData die in DieDataList)
                dt.Rows.Add(die.ToStringArray());

            return dt;
        }

        /// <summary>
        /// 데이터를 업데이트 합니다.
        /// </summary>
        /// <param name="dataTable">정보 데이터</param>
        /// <param name="dataTable_2">Die 데이터</param>
        public void UpdateData(DataTable infoTable, DataTable dieTable)
        {
            PropertyInfo[] propArr = GetType().GetProperties();

            foreach (DataRow row in infoTable.Rows)
            {
                string key = row[KEY_COLUMN].ToString();
                string value = row[VALUE_COLUMN].ToString();

                foreach (PropertyInfo prop in propArr)
                {
                    if (prop.Name == key)
                    {
                        ParsingUtil.SetPropertyValue(prop, this, value);
                        break;
                    }
                }
            }

            for (int r = 0; r < dieTable.Rows.Count; r++)
            {
                CpDieData die = DieDataList[r];

                for (int c = 0; c < dieTable.Columns.Count; c++)
                {
                    die[c].Value = dieTable.Rows[r][c].ToString();
                }
            }
        }

        public override DateTime GetBackupDateTime()
        {
            return StartTime;
        }

        /// <summary>
        /// Die 데이터를 보정합니다. 데이터 처리 순서는 시프트, 회전 순서 입니다..
        /// </summary>
        /// <param name="rotationAngle">회전할 각도를 나타냅니다. BOTTOM이 0도 입니다.</param>
        /// <param name="shiftX">X 방향 시프트 값</param>
        /// <param name="shiftY">Y 방향 시프트 값</param>
        public override void SetCorrection(
            int rotationAngle
            )
        {
            if (rotationAngle == 0)
                return;

            /*
             * 순서는 반드시 시프트, 회전 순으로 처리한다.
             */
            int xmin, xmax, ymin, ymax;
            GetMinMax(out xmin, out xmax, out ymin, out ymax);

            foreach (CpDieData die in DieDataList)
            {
                int x = Int32.Parse(die["X"]);
                int y = Int32.Parse(die["Y"]);

                // 1)회전
                Rotate(rotationAngle, xmin, xmax, ymin, ymax, ref x, ref y);

                die[INDEX_X] = x.ToString();
                die[INDEX_Y] = y.ToString();
            }
        }

        public static readonly string INDEX_X = "X";
        public static readonly string INDEX_Y = "Y";

        protected override void GetMinMax(
            out int xmin, 
            out int xmax, 
            out int ymin, 
            out int ymax
            )
        {
            xmin = ymin = Int32.MaxValue;
            xmax = ymax = Int32.MinValue;

            foreach (CpDieData die in DieDataList)
            {
                int x = Int32.Parse(die[INDEX_X]);
                int y = Int32.Parse(die[INDEX_Y]);

                xmin = Math.Min(xmin, x);
                ymin = Math.Min(ymin, y);
                xmax = Math.Max(xmax, x);
                ymax = Math.Max(ymax, y);
            }
        }

        public string TFVersion { get; private set; }
        public string Device { get; private set; }
        public string TestArea { get; private set; }
        public string Facility { get; private set; }

        public string WaferID { get; private set; }
        public string Prober { get; private set; }
        public int WaferDiameter { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        public string[] ChipDataDef { get; private set; }
        public List<CpDieData> DieDataList { get; private set; }

        public enum CpDataOrder
        {
            Key,
            DataType,
            Value
        }

        public class CpDieData : List<CpTestData>
        {
            public static readonly string EMPTY_VALUE = " ";

            public bool ContainsName(string name)
            {
                foreach (var data in this)
                {
                    if (data.Name == name)
                        return true;
                }

                return false;
            }

            public string[] ToStringArray()
            {
                string[] arr = new string[Count];

                for (int i = 0; i < arr.Length; i++)
                {
                    string value = String.IsNullOrEmpty(this[i].Value) ? EMPTY_VALUE : this[i].Value;
                    arr[i] = value;
                }

                return arr;
            }

            public string this[string name]
            {
                get
                {
                    foreach (var data in this)
                    {
                        if (data.Name == name)
                            return data.Value;
                    }

                    return String.Empty;
                }

                set
                {
                    foreach (var data in this)
                    {
                        if (data.Name == name)
                        {
                            data.Value = value;
                            break;
                        }
                    }
                }
            }

            public override string ToString()
            {
                return String.Format("X:{0},Y:{1}", this["X"], this["Y"]);
            }
        }

        public class CpTestData : IComparable<CpTestData>, IComparer<string>
        {
            public static string X_INDEX = "X";
            public static string Y_INDEX = "Y";

            public CpTestData(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; internal set; }
            public string Value { get; internal set; }

            public override string ToString()
            {
                return String.Format("{0}={1}", Name, Value);
            }

            /// <summary>
            /// X, Y가 맨앞으로 정렬되도록 수정
            /// </summary>
            public int CompareTo(CpTestData data)
            {
                return Compare(Name, data.Name);
            }

            public int Compare(string strA, string strB)
            {
                if (strA == strB)
                    return 0;
                else if (strA == X_INDEX)
                    return -3;
                else if (strB == X_INDEX)
                    return 3;
                else if (strA == Y_INDEX)
                    return -2;
                else if (strB == Y_INDEX)
                    return 2;
                else
                    return strA.CompareTo(strB);
            }
        }
    }
}
