using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Reflection;

namespace DACrux.Data.Parser
{
    public static class ParsingUtil
    {
        public static readonly int MAX_RETRY = 10000;
        public static readonly int FILE_READ_WAIT_TIME = 1000;

        public static string FileToString(string fileName)
        {
            CheckFileWriting(fileName);

            using (StreamReader sr = File.OpenText(fileName))
            {
                return sr.ReadToEnd();
            }
        }

        // 파일이 기록중인 상태인지를 체크합니다.
        private static void CheckFileWriting(string fileName)
        {
            long len1 = GetFileSize(fileName);

            // 파일 크기가 0 인 경우 다음 주기에 처리되도록 IOException 발생
            if (len1 == 0)
                throw new System.IO.IOException(String.Format("'{0}' 파일 크기가 0 입니다.", Path.GetFileName(fileName)));

            System.Threading.Thread.Sleep(FILE_READ_WAIT_TIME);

            long len2 = GetFileSize(fileName);

            if (len1 != len2)
                throw new System.IO.IOException(String.Format("'{0}' 파일이 Writing 중입니다. ", Path.GetFileName(fileName)));
        }

        /// <summary>
        /// 파일이 모두 기록될때까지 대기합니다.
        /// (파일을 읽어오는 스토리지가 Windows 파일시스템이 아닌 경우 파일이 기록되는 도중 읽어오는 문제가 있음)
        /// </summary>
        private static bool WaitFileWriteAll(string fileName)
        {
            int retry = 0;
            long len = GetFileSize(fileName);

            // 파일 크기가 0인 경우 다음 주기에 처리되도록 IOException 발생
            if (len == 0)
                throw new System.IO.IOException(String.Format("'{0}' 파일 크기가 0 입니다.", Path.GetFileName(fileName)));

            while (retry++ < MAX_RETRY)
            {
                System.Threading.Thread.Sleep(FILE_READ_WAIT_TIME);
                long tmp = GetFileSize(fileName);

                if (tmp == len)
                    break;

                len = tmp;
            }

            if (retry > MAX_RETRY)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 파일의 길이를 가져옵니다.
        /// </summary>
        private static long GetFileSize(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return fs.Length;
            }
        }

        /// <summary>
        /// 데이터를 Key와 Value로 가져옵니다. Key가 이미 존재하는 경우 데이터를 Append합니다.
        /// </summary>
        public static Dictionary<string, string> GetKeyValueDictionary(string[] arr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (string text in arr)
            {
                string val = text.TrimStart();
                int index = val.IndexOf(' ');

                if (index > 0)
                {
                    string key = val.Substring(0, index).Trim();
                    string value = val.Substring(index + 1);

                    // 이미 해당 키가 존재하는 경우 데이터를 Append 한다.
                    if (!dic.ContainsKey(key))
                        dic.Add(key, value);
                    else
                        dic[key] = dic[key] + Environment.NewLine + value;
                }
            }

            return dic;
        }

        /// <summary>
        /// 지정된 인스턴스의 프로퍼티에 값을 설정합니다.
        /// </summary>
        /// <param name="owner">인스턴스</param>
        /// <param name="headers">헤더(프로퍼티명과 명칭이 같아야 함)</param>
        /// <param name="data">데이터(헤더와 순서가 맞아야 함)</param>
        public static void SetPropertyData(object owner, List<string> headers, List<double> data)
        {
            if (owner == null || headers == null || data == null || headers.Count == 0 || data.Count == 0)
                return;

            Type t = owner.GetType();

            for (int i = 0; i < headers.Count; i++)
            {
                PropertyInfo prop = t.GetProperty(headers[i]);

                if (prop != null)
                    SetPropertyValue(prop, owner, data[i]);
            }
        }

        public static void SetPropertyValue(PropertyInfo prop, object owner, object value)
        {
            if (prop == null || owner == null)
                return;

            if (value == null)
                prop.SetValue(owner, null, null);
            else
                prop.SetValue(owner, Convert.ChangeType(value, prop.PropertyType), null);
        }
    }
}
