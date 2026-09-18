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
        //private static string m_strEQUIPID = "UNKNOWN";
        //private static string m_strFILENAME = "UNKNOWN";
        //private static string m_strFILETYPE = "UNKNOWN";
        private static string m_strLOGPATH = "UNKNOWN";
        //private static string m_strERRPATH = "UNKNOWN";
        //private static string m_strBAKPATH = "UNKNOWN";
        private static int m_intLogLevel = 0;

        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            Console.Title = "Parser";
            Parser p = new Parser();
            p.main(args);

            #region Folding
            //try
            //{
            //    if (args.Length == 0)
            //    {
            //        Console.WriteLine("No Argument ");
            //    }

            //    m_strLOGPATH = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

            //    //System.Threading.Thread.Sleep(10000);

            //    for (int i = 0; i < args.Length; i = i + 2)
            //    {
            //        switch (args[i].ToUpper())
            //        {
            //            //case "-TYPE":
            //            //case "-T":
            //            //    m_strFILETYPE = args[i + 1];
            //            //    break;
            //            case "-EQUIPID":
            //            case "-E":
            //                m_strEQUIPID = args[i + 1];
            //                break;
            //            case "-FILE":
            //            case "-F":
            //                m_strFILENAME = args[i + 1];
            //                m_strERRPATH = Path.GetDirectoryName(m_strFILENAME).Replace(@"\Process", "") + @"\Error";
            //                m_strBAKPATH = Path.GetDirectoryName(m_strFILENAME).Replace(@"\Process", "") + @"\Backup";
            //                break;
            //            case "-LOG":
            //            case "-L":
            //                m_strLOGPATH = args[i + 1];
            //                break;
            //            case "-LOGLEVEL":
            //            case "-V":
            //                m_intLogLevel = int.Parse(args[i + 1]);
            //                break;
            //        }
            //    }

            //    /// Loging (0)
            //    /// Parse Argument
            //    /// ///////////////////////////////////////////////////////////////////////
            //    WriteLog("L0:===============================================================");
            //    WriteLog("L0:%%TIME%% Start data read");
            //    //WriteLog(string.Format("L0:Result file type : {0}", m_strFILETYPE));
            //    WriteLog(string.Format("L0:Result file name : {0}", m_strFILENAME));
            //    WriteLog(string.Format("L0:Result Equipment ID : {0}", m_strEQUIPID));
            //    WriteLog("L0:===============================================================");
            //    /// ///////////////////////////////////////////////////////////////////////



            //    //#else
            //    //            Console.Title = "Parser";
            //    //            Parser p = new Parser();
            //    //            p.main(args);
            //    //#endif

            //}
            //catch (Exception ex)
            //{
            //    //WriteLog(string.Format("L0:%%TIME%% [Error] [{0}]", ex.Message));
            //    //if (File.Exists(m_strFILENAME)) File.Copy(m_strFILENAME, string.Format(@"{0}\{1}", m_strERRPATH, Path.GetFileName(m_strFILENAME)), true);
            //    //WriteLog(string.Format(@"L0:%%TIME%% Copy to error directory  [{0}\{1}]", m_strERRPATH, Path.GetFileName(m_strFILENAME)));
            //    return;
            //}
            //finally
            //{
            //    //if (File.Exists(m_strFILENAME)) File.Move(m_strFILENAME, string.Format(@"{0}\{1}", m_strBAKPATH, Path.GetFileName(m_strFILENAME)));
            //    //WriteLog(string.Format(@"L0:%%TIME%% Copy to backup directory [{0}\{1}]", m_strBAKPATH, Path.GetFileName(m_strFILENAME)));
            //    WriteLog("L0:%%TIME%% End data read");
            //    WriteLog("L0:===============================================================");
            //}

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
