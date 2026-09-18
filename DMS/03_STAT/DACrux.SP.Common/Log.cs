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
        private static Queue<string> qLog = null;

        #endregion

        #region [ 생성자 ]

        public Log()
        {
            qLog = new Queue<string>();
        }

        public Log(int iLevel)
        {
            qLog = new Queue<string>();
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

            if(qLog == null) qLog = new Queue<string>();

            DateTime dtNow = DateTime.Now;
            qLog.Enqueue(string.Format("{0}\t{1}\t{2}", dtNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), strLogType, strLog));
            qLog.TrimExcess();


            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(WriteThread));
            t.Start(Path.Combine(logPath, dtNow.ToString("yyyyMMdd") + ".log"));
        }

        private static void WriteThread(object objLogInfo)
        {
            try
            {
                string sLogMsg = string.Empty;
                string sLogPath = (string)objLogInfo;

                FileStream fs = new FileStream(sLogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1024, false);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                while (qLog != null && qLog.Count > 0)
                {
                    try { sLogMsg = qLog.Dequeue(); }
                    catch (InvalidOperationException) { continue; }

                    sw.WriteLine(sLogMsg);
                }

                sw.Flush();
                sw.Close();
                fs.Close();
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
