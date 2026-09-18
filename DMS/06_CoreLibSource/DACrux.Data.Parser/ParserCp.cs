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
    public class ParserCp : ParserTest, ISaveFile
    {
        public static readonly string DATE_FORMAT = "MM/dd/yyyy HH:mm:ss";

        public static readonly string KEY_COLUMN = "KEY";
        public static readonly string VALUE_COLUMN = "VALUE";

        public ParserCp(string fileName)
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

                if (dic == null || dic.Count <= 0 || dic.ContainsKey("[Format]") == false)
                    return;

                if (text.Contains("[End]") == false)
                    throw new IOException(string.Format("CP [End] 문자열이 없어 다음 주기 재시도. File : {0}", fileName));

                List<string> list = dic["[Format]"];
                TFVersion = GetValue(list, "TFversion");

                list = dic["[Lot]"];
                LotID = GetValue(list, "lot");

                //ProgramName 상에 A129F5-1ST-V03-CP2 형식으로 '-' 가 있는 경우가 있어 '-' -> '_' 로 변경시켜 준다.
                ProgramName = GetValue(list, "program").Replace("-", "_");
                Device = GetValue(list, "device");
                TestArea = GetValue(list, "testArea");
                Facility = GetValue(list, "facility");

                list = dic["[Wafer]"];

                WaferID = GetValue(list, "waferid");

                //Wafer ID 가 LotID-1, LotID-2, LotID-3 형태로 나와 LotID-01, LotID-02, LotID-03 형태로 변경 해준다.
                string[] strSpl = WaferID.Trim().Split(new string[] { "-", "_", "." }, StringSplitOptions.RemoveEmptyEntries);
                if (strSpl.Length == 2)
                {
                    WaferID = string.Format("{0}-{1:00}", strSpl[0], Convert.ToInt32(strSpl[1]));
                }

                StartTime = GetDateTime(GetValue(list, "startTime"), true);
                EndTime = GetDateTime(GetValue(list, "endTime"), true);
                Operator = GetValue(list, "operator");
                EquipID = GetValue(list, "tester");
                Prober = GetValue(list, "prober");
                Tester = GetValue(list, "tester");
                ProbeCard = GetValue(list, "probeCard");
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

                    //if (ChipDataDef.Length != dataArr.Length)
                    //    throw new Exception(string.Format("Header 와 Value 의 개수가 상이 합니다. Header : {0}, Vlaue : {1}", ChipDataDef.Length, dataArr.Length));

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

        protected virtual string GetValue(List<string> list, string startVal)
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

        /// <summary>
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
        /// <param name="rotate">회전할 각도를 나타냅니다. BOTTOM이 0도 입니다.</param>
        /// <param name="shiftX">X 방향 시프트 값</param>
        /// <param name="shiftY">Y 방향 시프트 값</param>
        public void SetCorrection(string Dir, int rotate, int shiftX, int shiftY)
        {
            if (string.IsNullOrEmpty(Dir) && rotate == 0 && shiftX == 0 && shiftY == 0)
                return;

            /*
             * 순서는 반드시 XY_DIRECTION -> ROTATION -> X, Y SHIFT
             */

            int xmin, xmax, ymin, ymax;
            GetMinMax(out xmin, out xmax, out ymin, out ymax);

            foreach (CpDieData die in DieDataList)
            {
                int x = Int32.Parse(die["X"]);
                int y = Int32.Parse(die["Y"]);

                //1) Swap
                switch (Dir)
                {
                    case "LL":
                        //x = x;
                        //y = y;
                        break;
                    case "TL":
                        //x = x;
                        y = (ymax + ymin - y);
                        break;
                    case "TR":
                        x = (xmax + xmin - x);
                        y = (ymax + ymin - y);
                        break;
                    case "LR":
                        x = (xmax + xmin - x);
                        //y = y;
                        break;
                }        

                // 2)회전
                Rotate(rotate, xmin, xmax, ymin, ymax, ref x, ref y);

                // 3)시프트
                x += shiftX;
                y += shiftY;

                die[INDEX_X] = x.ToString();
                die[INDEX_Y] = y.ToString();
            }
        }

        public static readonly string INDEX_X = "X";
        public static readonly string INDEX_Y = "Y";

        private void GetMinMax(out int xmin, out int xmax, out int ymin, out int ymax)
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

        public string TFVersion { get; protected set; }
        //public string ProgramName { get; protected set; }
        public string Device { get; protected set; }
        public string TestArea { get; protected set; }
        public string Facility { get; protected set; }
        public string WaferID { get; protected set; }
        //public DateTime StartTime { get; protected set; }
        //public DateTime EndTime { get; protected set; }
        //public string Operator { get; protected set; }
        public string Tester { get; protected set; }
        public string Prober { get; protected set; }
        //public string ProbeCard { get; protected set; }
        public int WaferDiameter { get; protected set; }
        public int MaxX { get; protected set; }
        public int MaxY { get; protected set; }

        public string[] ChipDataDef { get; protected set; }
        public List<CpDieData> DieDataList { get; protected set; }

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
