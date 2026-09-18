using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Framework.Server
{
    public static class Utility
    {
        public static char[] SEPARATOR = new char[] { ';' };

        /// <summary>
        /// Config 파일 정보를 새로 읽어들입니다.
        /// </summary>
        public static void RefreshConfig()
        {
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// appSettings의 key에 대한 값을 가져옵니다.
        /// </summary>
        public static string GetConfigValue(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// appSettings의 key에 대한 값을 숫자값으로 가져옵니다.
        /// </summary>
        public static int GetIntValueFromConfig(string name, int defaultValue)
        {
            string value = GetConfigValue(name);

            int num;

            if (Int32.TryParse(value, out num))
                return num;
            else
                return defaultValue;
        }

        /// <summary>
        /// appSettings의 key에 대한 값을 문자열 배열로 가져옵니다.
        /// </summary>
        public static string[] GetConfigValueWithSplit(string name)
        {
            string value = GetConfigValue(name);

            if (String.IsNullOrEmpty(value))
                return null;

            return value.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 배열을 문자열로 가져옵니다.
        /// </summary>
        public static string ArrayToString(string[] values, string separator = ", ")
        {
            return String.Join(separator, values);
        }

        /// <summary>
        /// 키와 값을 컬렉션으로 가져옵니다.
        /// </summary>
        internal static System.Collections.Specialized.NameValueCollection GetConfigValues()
        {
            return System.Configuration.ConfigurationManager.AppSettings;
        }
    }
}
