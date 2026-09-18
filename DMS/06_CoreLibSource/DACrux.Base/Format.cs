using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DACrux.Base
{
    public static class Format
    {
        public const string OracleDateTime = "YYYY-MM-DD HH24:MI:SS";
        public const string OracleDate = "YYYY-MM-DD";
        public const string OracleTime = "HH24:MI:SS";

        public const string APPDateTime = "yyyy-MM-dd HH:mm:ss";
        public const string APPDate = "yyyy-MM-dd";
        public const string APPTime = "HH:mm:ss";


        /// <summary>
        /// DateLib에 대한 요약 설명입니다.
        /// </summary>
        public static string GetDateString(ref string strDate, string txtLine)
        {
            MatchCollection Matches;

            // Case 1 YYYY-MM-DD
            Matches = Regex.Matches(txtLine, "[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-3][0-9]");
            foreach (Match NextMatch in Matches)
            {
                strDate = NextMatch.ToString();
                return "YYYY-MM-DD";
            }

            // Case 2 YYYY/MM/DD
            Matches = Regex.Matches(txtLine, "[0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-3][0-9]");
            foreach (Match NextMatch in Matches)
            {
                strDate = NextMatch.ToString();
                return "YYYY/MM/DD";
            }

            // Case 3 MM-DD-YY
            Matches = Regex.Matches(txtLine, "[0-9][0-9]-[0-3][0-9]-[0-9][0-9]");
            foreach (Match NextMatch in Matches)
            {
                strDate = NextMatch.ToString();
                return "MM-DD-YY";
            }

            // Case 4 MM/DD/YY
            Matches = Regex.Matches(txtLine, "[0-9][0-9]/[0-3][0-9]/[0-9][0-9]");
            foreach (Match NextMatch in Matches)
            {
                strDate = NextMatch.ToString();
                return "MM/DD/YY";
            }

            return "";
        }


        public static string GetTimeString(ref string strTime, string txtLine)
        {
            MatchCollection Matches;

            // Case 1 HH24:MI:SS
            Matches = Regex.Matches(txtLine, "[0-2][0-9]:[0-5][0-9]:[0-5][0-9]");
            foreach (Match NextMatch in Matches)
            {
                strTime = NextMatch.ToString();
                return "HH:mm:ss";
            }
            return "";

        }
    }
}
