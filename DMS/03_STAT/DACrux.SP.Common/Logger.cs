using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SP.Common;
using System.IO;
using System.Windows.Forms;

namespace DACrux.SP.Common
{
    [Serializable]
    public sealed class ConsoleLogger : ILogger
    {
        public void WriteLog(LogTypeItem type, string message)
        {
            Console.WriteLine(string.Format("[{0}]\t{1}", type.ToString(), message));
        }
    }

    [Serializable]
    public sealed class MSMQLogger : ILogger
    {
        private MSMQLogger() { }

        public MSMQLogger(string queueName)
        {
            // check queue, create queue if queueName is string.Empty.
        }

        public void WriteLog(LogTypeItem type, string message)
        {

        }
    }

    [Serializable]
    public sealed class FileLogger : ILogger
    {
        private FileLogger() { }

        string fileName = string.Empty;

        public FileLogger(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                this.fileName = Path.Combine(System.Windows.Forms.Application.StartupPath, @"SPR_LOG.txt");
            }
            else
                this.fileName = fileName;
        }

        public void WriteLog(LogTypeItem type, string message)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(string.Format("{0}\t[{1}]\t{2}"
                    , System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , type.ToString().PadRight(10, ' ')
                    , message));
            }
        }
    }

    [Serializable]
    public sealed class ControlLogger : ILogger
    {
        private ControlLogger() { }

        Control ctl = null;

        string fileName = string.Empty;

        public ControlLogger(Control ctl)
        {
            if (ctl == null)
                return;

            this.ctl = ctl;
        }

        public void WriteLog(LogTypeItem type, string message)
        {
            ctl.Text += string.Format("{0}\t[{1}]\t{2}\n"
                    , System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , type.ToString().PadRight(10, ' ')
                    , message);
        }
    }

    [Serializable]
    public sealed class DBLogger : ILogger
    {
        private DBLogger() { }

        public DBLogger(ConnectionInformation connInfo)
        {
        }

        public void WriteLog(LogTypeItem type, string message)
        {

        }
    }

    [Serializable]
    public sealed class StringLogger : ILogger
    {
        string strLog;

        private StringLogger() { }

        public StringLogger(ref string strLog)
        {
            this.strLog = strLog;
        }

        public void WriteLog(LogTypeItem type, string message)
        {
            strLog = string.Format("{0}\t[{1}]\t{2}"
                , System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                , type.ToString().PadRight(10, ' ')
                , message);
        }
    }
}