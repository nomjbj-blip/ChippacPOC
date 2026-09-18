using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace DACrux.SP.Common
{
    /// <summary>
    /// Class Name : Log<br/>
    /// iLogLevel : 단계별 Log 작성을 위한 변수
    /// 0(최소로그) ~ 4(최대로그) 단계로 구성
    /// </summary>
    public class Log
    {  
        #region [ Class Member ]

        public static int iLogLevel = 0;
        private static System.Collections.Concurrent.ConcurrentQueue<string> qLog = new System.Collections.Concurrent.ConcurrentQueue<string>();
        private static object LockObject = new object();

        #endregion

        #region [ 생성자 ]

        public Log()
        {
        }

        public Log(int iLevel)
        {
            iLogLevel = iLevel;
        } 

        #endregion

        #region [ Write Log ]

        public void WriteLog(string strLog,string logPath,int loglevel)
        {
            WriteLog(strLog, "", logPath, loglevel);
        }

        public void WriteLog(string strLogType,string strLog,string logPath,int loglevel)
        {
            if (loglevel > iLogLevel)   // Log Level Check
                return;

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            DateTime dtNow = DateTime.Now;
            qLog.Enqueue(string.Format("{0}\t{1}\t{2}", dtNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), strLogType, strLog));

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(WriteThread));
            t.IsBackground = true;
            t.Start(Path.Combine(logPath, dtNow.ToString("yyyyMMdd") + ".log"));
        }

        private static void WriteThread(object objLogInfo)
        {
            try
            {
                string sLogMsg = string.Empty;
                string sLogPath = (string)objLogInfo;

                using (FileStream fs = new FileStream(sLogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1024, false))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    while (qLog != null && qLog.Count > 0)
                    {
                        if (qLog.TryDequeue(out sLogMsg))
                            sw.WriteLine(sLogMsg);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (System.Threading.Thread.CurrentThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    try { System.Threading.Thread.CurrentThread.Abort(); }
                    catch (System.Threading.ThreadAbortException) { }
                }
            }
        }
        
        #endregion
        
    }
}
