using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Collections.Specialized;
using System.IO;

namespace DACrux.Data.Parser
{
    public abstract class ParserBase
    {
        #region 멤버 변수

        /// <summary>
        /// 파일 생성 후 체크할 기간 (분)
        /// </summary>
        public static int CHECK_CREATION_TIME_DURATION = 3;

        public static readonly string[] DateTimeFormat = new string[] 
        {
            "MM/dd/yyyy HH:mm", 
            "MM/dd/yyyy HH:mm:ss",
            "dd-MMM-yyyy HH:mm", 
            "d-MMM-yyyy HH:mm", 
            "dd-MMM-yyyy HH:mm:ss", 
            "d-MMM-yyyy HH:mm:ss",
            "MM-dd-yy HH:mm:ss",
            "MM/dd/yy HH:mm:ss",
            "MM-dd-yyyy HH:mm:ss",
            "MM/dd HH:mm:ss/yyyy",
            "yyyyMMddHHmmss", 
            "MM/dd/yyyy HH:mm:ss", 
            "MM-dd-yyyy HH:mm:ss", 
            "yyyy/MM/dd HH:mm:ss", 
            "yyyy-MM-dd HH:mm:ss",
            "yy-MM-dd HH:mm:ss"
        };
        
        #endregion

        #region 생성자

        protected ParserBase()
        {
        }

        protected ParserBase(string fileName)
            : this()
        {
            FileName = fileName;

            FileCreationTime = File.GetCreationTime(FileName);

            // File Creation Time 이 현재 시간 보다 큰 경우 현재 시간으로 변경한다. 2019.10.31 Taihi,Kim.
            if (FileCreationTime > DateTime.Now)
            {
                try
                {
                    FileCreationTime = DateTime.Now;
                    File.SetCreationTime(FileName, DateTime.Now);
                }
                catch
                {
                }
            }

            if (!String.IsNullOrEmpty(fileName))
                DirectoryName = Path.GetDirectoryName(fileName);
        }
        
        #endregion

        #region Vitual 메서드

        public virtual DateTime GetBackupDateTime()
        {
            throw new NotImplementedException("자식 클래스에서 구현되지 않았습니다.");
        }

        /// <summary>
        /// 백업을 위한 상대 경로를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public virtual string GetPathForBackup()
        {
            DateTime dt = GetBackupDateTime();
            return String.Format(@"{0:0000}\{1:00}\{2:00}\{3}", dt.Year, dt.Month, dt.Day, LotID);
        }

        #endregion

        #region 메서드

        /// <summary>
        /// 에러 체크가 필요한지를 가져옵니다. 에러 체크 기간은 파일 생성시간부터 CHECK_CREATION_TIME_DURATION 시간(분)까지 입니다.
        /// </summary>
        public bool RequireErrorCheck()
        {
            return FileCreationTime.AddMinutes(CHECK_CREATION_TIME_DURATION) > DateTime.Now;
        }

        internal static string GetString(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return null;

            return GetString(dic[key]);
        }

        public static string GetString(NameValueCollection col, string key)
        {
            return GetString(col[key]);
        }

        internal static string GetString(string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            //정병주 : Trim 이 제대로 안되서 재Trim 한다.
            return data.Trim('"').Trim();
        }

        internal static T[] ListToArray<T>(List<T> list)
        {
            if (list == null)
                return null;

            return list.ToArray();
        }

        internal static T First<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);

            return list[0];
        }

        internal static List<string> GetStringList(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return null;

            return GetStringList(dic[key]);
        }

        /// <summary>
        /// 큰 따움표로 싸여있는 문자열은 1개로 처리
        /// </summary>
        internal static List<string> GetStringList(string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            List<string> list = new List<string>();
            data = data.Trim().Replace(Environment.NewLine, String.Empty);
            bool open = false;
            int idx = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '"')
                {
                    open = !open;
                }
                else if (data[i] == ' ' && !open)
                {
                    if (idx == i)
                        continue;

                    string val = GetString(data.Substring(idx, i - idx)).Trim();

                    if (!String.IsNullOrEmpty(val))
                        list.Add(val);

                    idx = i + 1;
                }
            }

            if (idx < data.Length)
                list.Add(GetString(data.Substring(idx)));

            return list;
        }

        internal static List<string> GetStringWithLength(Dictionary<string, string> dic, string key, bool checkLength = true)
        {
            if (!dic.ContainsKey(key))
                return null;

            return GetStringWithLength(dic[key], checkLength);
        }

        internal static List<string> GetStringWithLength(string data, bool checkLength = true)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            List<string> list = GetStringList(data);
            int length = GetInt(list[0]);
            list.RemoveAt(0);

            if (checkLength && list.Count != length)
                throw new Exception(String.Format("배열의 길이({0})가 지정된 값({1})과 다릅니다.", list.Count, length));

            return list;
        }

        internal static List<int> GetIntWithLength(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new List<int>();

            return GetIntWithLength(dic[key]);
        }

        internal static List<int> GetIntWithLength(string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            List<string> list = GetStringWithLength(data);
            List<int> result = new List<int>();

            foreach (string val in list)
            {
                result.Add(GetInt(val));
            }

            return result;
        }

        internal static DateTime GetDateTime(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return DateTime.MinValue;

            return GetDateTime(dic[key]);
        }

        internal static DateTime GetDateTime(string text, bool bErrorNotTime = false)
        {
            DateTime dt;

            if (string.IsNullOrEmpty(text))
                return DateTime.MinValue;

            if (!DateTime.TryParseExact(text, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                if(bErrorNotTime == true)
                    return DateTime.MinValue;

                string[] strValue = text.Split(new string[] { "/", ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (strValue.Length == 6)
                    text = string.Format("{0}/{1}/{2} {3}:{4}:{5}", strValue[0], strValue[1], strValue[5], strValue[2], strValue[3], strValue[4]);

                if (!DateTime.TryParseExact(text, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    throw new Exception(String.Format("날짜로 변환할 수 없습니다({0})", text));
            }

            return dt;
        }

        internal static DateTime GetDateTime(string text, string format)
        {
            DateTime dt;

            if (!DateTime.TryParseExact(text, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                throw new Exception(String.Format("날짜로 변환할 수 없습니다({0})", text));

            return dt;
        }

        internal static List<Point> GetPointWithLength(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return null;

            return GetPointWithLength(dic[key]);
        }

        internal static List<Point> GetPointWithLength(string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            List<string> list = GetStringWithLength(data, false);
            List<Point> result = new List<Point>();

            if (list.Count % 2 != 0)
                throw new Exception(String.Format("데이터는 짝수이어야 합니다.({0})", list.Count));

            for (int i = 0; i < list.Count; i += 2)
                result.Add(new Point(GetInt(list[i]), GetInt(list[i + 1])));

            return result;
        }

        internal static Dictionary<int, string> GetDictionaryWithLength(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return null;

            return GetDictionaryWithLength(dic[key]);
        }

        internal static Dictionary<int, string> GetDictionaryWithLength(string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            List<string> list = GetStringWithLength(data, false);
            Dictionary<int, string> result = new Dictionary<int, string>();

            if (list.Count % 2 != 0)
                throw new Exception(String.Format("데이터는 짝수이어야 합니다.({0})", list.Count));

            for (int i = 0; i < list.Count; i += 2)
                result.Add(GetInt(list[i]), list[i + 1]);

            return result;
        }

        internal static SizeF GetSizeF(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return SizeF.Empty;

            List<string> list = GetStringList(dic[key]);
            return new SizeF((float)GetDouble(list[0]), (float)GetDouble(list[1]));
        }

        internal static int GetInt(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return 0;

            return GetInt(dic[key]);
        }

        internal static int GetInt(string text)
        {
            int num;

            if (!Int32.TryParse(text, out num))
                throw new Exception(String.Format("{0} 값을 숫자로 변환할 수 없습니다.", text));

            return num;
        }

        internal static double GetDouble(Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return 0;

            return GetDouble(dic[key]);
        }

        internal static double GetDouble(string text)
        {
            double num;

            if (!Double.TryParse(text, out num))
                throw new Exception(String.Format("{0} 값을 실수로 변환할 수 없습니다.", text));

            return num;
        }

        /// <summary>
        /// Wafer를 시계방향으로 회전합니다.
        /// </summary>
        /// <param name="clockwiseAngle">시계방향회전각도 : 0,90,180,270</param>
        public void Rotate(int clockwiseAngle, int xmin, int xmax, int ymin, int ymax, ref int x, ref int y)
        {
            if (clockwiseAngle == 0)
                return;

            while (clockwiseAngle < 0)
                clockwiseAngle += 360;

            int nx, ny;

            // 계산횟수를 줄이기 위해 각도별로 계산식 구성
            if (clockwiseAngle == 180)
            {
                nx = xmax + xmin - x;
                ny = ymax + ymin - y;
            }
            else if (clockwiseAngle == 90)
            {
                nx = y;
                ny = xmax + xmin - x;
            }
            else if (clockwiseAngle == 270)
            {
                nx = ymax + ymin - y;
                ny = x;
            }
            else
            {
                throw new Exception(String.Format("처리할 수 없는 각도({0}) 입니다.", clockwiseAngle));
            }

            x = nx;
            y = ny;

            //int cnt = clockwiseAngle % 360 / 90;

            //for (int i = 0; i < cnt; i++)
            //{
            //    int newX = Math.Abs(y);
            //    int newY = Math.Abs(colCount - x + 1);

            //    x = newX;
            //    y = newY;

            //    int temp = colCount;
            //    colCount = rowCount;
            //    rowCount = temp;
            //}
        }

        /*
        /// <summary>
        /// Wafer를 시계방향으로 회전합니다.
        /// </summary>
        /// <param name="clockwiseAngle">시계방향회전각도 : 0,90,180,270</param>
        public void RotateByOrigin(int clockwiseAngle,  int originX, int originY, ref int x, ref int y)
        {
            if (clockwiseAngle == 0)
                return;

            while (clockwiseAngle < 0)
                clockwiseAngle += 360;

            int nx, ny;

            // 계산횟수를 줄이기 위해 각도별로 계산식 구성
            if (clockwiseAngle == 180)
            {
                nx = xmax + xmin - x;
                ny = ymax + ymin - y;
            }
            else if (clockwiseAngle == 90)
            {
                nx = y;
                ny = xmax + xmin - x;
            }
            else if (clockwiseAngle == 270)
            {
                nx = ymax + ymin - y;
                ny = x;
            }
            else
            {
                throw new Exception(String.Format("처리할 수 없는 각도({0}) 입니다.", clockwiseAngle));
            }

            x = nx;
            y = ny;

            //int cnt = clockwiseAngle % 360 / 90;

            //for (int i = 0; i < cnt; i++)
            //{
            //    int newX = Math.Abs(y);
            //    int newY = Math.Abs(colCount - x + 1);

            //    x = newX;
            //    y = newY;

            //    int temp = colCount;
            //    colCount = rowCount;
            //    rowCount = temp;
            //}
        }//*/

        internal static DataTable GetDataTable(List<string> headers, Dictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return null;

            DataTable dt = new DataTable();

            foreach (string header in headers)
                dt.Columns.Add(header, typeof(double));

            string[] arr = dic[key].Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string val in arr)
            {
                DataRow row = dt.NewRow();

                List<string> list = GetStringList(val.Replace('\t', ' '));

                // Review 후의 데이터를 고려하여 2개 이하인 경우 저장하지 않음
                // 2 890 792 23 71 3.10 1.00 3.10 1.00 41 0 1 0 1 1 <-- 저장
                //  1 0                                             <-- 저장하지 않음
                if (list.Count > 2)
                {
                    for (int i = 0; i < list.Count; i++)
                        row[i] = GetDouble(list[i]);

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

         /// <summary>
        /// 맨뒤의 값을 가져옵니다.
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <param name="key">찾을 키값</param>
        /// <param name="start">시작값</param>
        /// <param name="end">종료값</param>
        public static string GetLastValue(string text, string key, string start = null, string end = null)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            int idx = text.LastIndexOf(key);

            if (idx < 0)
                return null;

            return GetValue(text.Substring(idx), key, start, end);
        }

        /// <summary>
        /// 값을 가져옵니다.
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <param name="key">찾을 키값</param>
        /// <param name="start">시작값</param>
        /// <param name="end">종료값</param>
        public static string GetValue(string text, string key, string start = null, string end = null)
        {
            int idx = 0;

            if (!String.IsNullOrEmpty(key))
                idx = text.IndexOf(key);

            if (idx < 0)
                return null;

            int idx1;
            int keyLength = String.IsNullOrEmpty(key) ? 0 : key.Length;

            if (!String.IsNullOrEmpty(start))
                idx1 = text.IndexOf(start, idx + keyLength);
            else
                idx1 = idx + keyLength;

            if (idx1 < 0)
                return null;

            int startLength = String.IsNullOrEmpty(start) ? 0 : start.Length;

            if (String.IsNullOrEmpty(end))
                return text.Substring(idx1 + startLength).Trim();

            int idx2 = text.IndexOf(end, idx1 + startLength);

            if (idx2 < 0)
                return null;

            return text.Substring(idx1 + startLength, idx2 - idx1 - startLength).Trim();
        }

        /// <summary>
        /// 텍스트를 구분자로 자릅니다. 나눈 텍스트에는 구분자를 포함합니다.
        /// </summary>
        public static string[] Cut(string text, string separator)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            List<int> list = new List<int>();
            int idx = 0;

            while (true)
            {
                idx = text.IndexOf(separator, idx);

                if (idx < 0)
                    break;

                list.Add(idx);
                idx += separator.Length;
            }

            List<string> result = new List<string>();

            int start = 0;

            for (int i = 0; i <= list.Count; i++)
            {
                string val;

                if (i < list.Count)
                {
                    val = text.Substring(start, list[i] - start);
                    start = list[i];
                }
                else
                {
                    val = text.Substring(start);
                }

                result.Add(val);
            }

            return result.ToArray();
        }

        #endregion

        #region 프로퍼티

        private string _fileName;
        private string _backupFileName;

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;

                if (!String.IsNullOrEmpty(value))
                    DirectoryName = Path.GetDirectoryName(_fileName);
            }
        }

        public DateTime FileCreationTime
        {
            get;
            private set;
        }

        public string DirectoryName
        {
            get;
            private set;
        }

        public string BackupFileName
        {
            get;
            set;
        }

        public string BackupDirectoryName
        {
            get;
            set;
        }

        public string FtpRelativePath 
        {
            get; 
            set;
        }

        public string LotID
        {
            get;
            protected set;
        }

        public bool ErrorFlag
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool ErrorBackupFlag
        {
            get;
            set;
        }

        #endregion
    }

    public class ParserList : List<ParserBase>
    {
    }

    public interface ISaveFile
    {
        void SaveFile(string fileName);
    }
}
