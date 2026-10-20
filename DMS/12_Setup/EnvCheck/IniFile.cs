using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCK.EnvCheck
{
    /// <summary>
    /// DACruxConfig.ini 를 DACrux 프로그램(DACrux.Base.Config.Init)과 같은 규칙으로 읽는다.
    /// 규칙: 빈 줄 / # 주석 무시, [섹션], "키 = 값"(앞뒤 공백 제거), 읽기 인코딩은 시스템 기본값.
    /// 점검 도구는 ini 를 읽기만 한다. 쓰기는 DACrux 와 Setup 만 한다.
    /// </summary>
    internal static class IniFile
    {
        /// <summary>
        /// 섹션 이름(대괄호 포함, 대문자) -> (키 대문자 -> 값) 사전으로 돌려준다. 파일이 없으면 빈 사전.
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> Read(string path)
        {
            var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(path)) return result;

            string section = string.Empty;
            foreach (string raw in File.ReadAllLines(path, Encoding.Default))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line.ToUpperInvariant();
                    if (!result.ContainsKey(section)) result[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    continue;
                }

                int eq = line.IndexOf('=');
                if (eq <= 0 || section.Length == 0) continue;
                result[section][line.Substring(0, eq).Trim()] = line.Substring(eq + 1).Trim();
            }
            return result;
        }

        /// <summary>
        /// 섹션 / 키 값을 돌려준다. 없으면 defaultValue.
        /// </summary>
        public static string Get(Dictionary<string, Dictionary<string, string>> ini, string section, string key, string defaultValue)
        {
            Dictionary<string, string> values;
            string value;
            if (ini.TryGetValue(section, out values) && values.TryGetValue(key, out value)) return value;
            return defaultValue;
        }
    }
}
