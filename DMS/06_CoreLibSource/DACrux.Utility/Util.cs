using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DACrux.Utility
{
    public static class Util
    {

        public static String HexConverter(
            Color color
            )
        {
            return String.Format("{0:X6}", color.ToArgb() & 0x00FFFFFF);
        }

        public static Color HexConverter(
            string htmlColor
            )
        {
            return (Color)ColorTranslator.FromHtml(htmlColor);
        }

        public static String RGBConvert(
            Color color
            )
        {
            return String.Format("RGB({0},{1},{2})", color.R, color.G, color.B);
        }

        /// <summary>
        /// FTP 경로를 반환 한다.
        /// </summary>
        /// <param name="strSubject">ex) DM, TEST, FOI</param>
        /// <param name="strStep">ex) Review, AVI, PCM, CP....</param>
        /// <param name="strLotID"></param>
        /// <returns></returns>
        public static string FTPBackupPath(string strSubject, string strStep, string strLotID)
        {
            DateTime dtTime = DateTime.Now;
            return string.Format("{0}/{1}/{2}/{3}/{4}/{5}", strSubject, strStep, dtTime.ToString("yyyy"), dtTime.ToString("MM"), dtTime.ToString("dd"), strLotID);
        }
    }
}
