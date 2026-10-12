using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DACrux.TEST.MAPParser
{
    class Program
    {
        private static string m_strLOGPATH = "UNKNOWN";
        private static int m_intLogLevel = 0;

        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            Console.Title = "Parser";
            Parser p = new Parser();
            p.main(args);

            #region Folding

            //    /// Loging (0)
            //    /// Parse Argument
            //    /// ///////////////////////////////////////////////////////////////////////

            //    //#else
            //    //#endif

            #endregion ---------------------------------------------
        }

        public static void WriteLog(string Message)
        {
            try
            {
                if (int.Parse(Message.Substring(1, 1)) <= m_intLogLevel)
                    WriteLog(m_strLOGPATH, Message);
            }
            catch
            {
                return;
            }
        }

        public static string GetFileName(string LOGPATH)
        {
            return string.Format(@"{0}\WatcherLog_{1}.log", LOGPATH, DateTime.Now.ToString("yyyyMMdd"));
        }

        public static void WriteLog(string LOGPATH, string Message)
        {
            string strFullPath = GetFileName(LOGPATH);
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(LOGPATH)) Directory.CreateDirectory(LOGPATH);

                sw = new StreamWriter(strFullPath, true);
                sw.AutoFlush = true;
                Message = Message.Substring(3);
                if (Message.IndexOf("==========") > -1)
                {
                    sw.WriteLine(Message);
                }
                else if (Message.IndexOf("%%TIME%%") > -1)
                {
                    sw.WriteLine(Message.Replace("%%TIME%%", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    sw.WriteLine("                    " + Message);
                }
                sw.Close();

            }
            catch
            {
                /// Error 무시
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                }
            }
        }
    }
}
