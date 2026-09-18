using System;
using System.Configuration;
using System.Threading;
using DACrux.SP.Common;
using DACrux.Framework.BSL;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Framework.Server
{
    /// <summary>
    ///  데이터 처리 서비스에 대한 기본 클래스 입니다.
    /// </summary>
    public class DataServiceBase : System.ServiceProcess.ServiceBase
    {
        public static int DEFAULT_INTERVAL = 60; // 10초

        public enum ActionType
        {
            SERVICE_START,
            SERVICE_STOP,
            SUCCESS,
            ERROR,
            WARNING
        }

        private Log _log;
        private bool _stop;
        private bool _stopped;

        protected DataServiceBase()
        {
            _log = new Log(Utility.GetIntValueFromConfig("LogLevel", 0));
            StopWatch = new StopWatch();
            ServiceLogDataList = new List<ServiceLogData>();
        }

        /// <summary>
        /// 동기 방식으로 서비스를 실행 합니다. 이 메서드는 테스트 용도로만 사용되어야 합니다.
        /// </summary>
        public void StartService()
        {
            OnStart(null);
        }

        /// <summary>
        /// 동기 방식으로 서비스를 종료 합니다. 이 메서드는 테스트 용도로만 사용되어야 합니다.
        /// </summary>
        public void StopService()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            _stop = false;
            _stopped = false;

            System.Threading.ThreadPool.QueueUserWorkItem(RunService);
        }

        protected override void OnStop()
        {
            _stop = true;

            WriteLog("STOP", "REQUEST STOP");

            while (!_stopped)
                Thread.Sleep(100);

#if !DEBUG
            AppendServiceLog(ActionType.SERVICE_STOP);
            InsertServiceLogData();
#endif

            WriteLog("STOP", String.Format("SERVICE STOP. {0}\r\n\r\n", GetType().Module.Name));
        }

        /// <summary>
        /// 서비스 처음 실행 시 실행되는 메서드 입니다.
        /// </summary>
        protected virtual void ServiceStart()
        {
        }

        /// <summary>
        /// 데이터 처리를 실행합니다.
        /// </summary>
        protected virtual void Execute()
        {
            throw new NotImplementedException("Execute() 메서드는 데이터 처리를 위해 자식 클래스에서 반드시 구현하여야 합니다.");
        }

        /// <summary>
        /// 서비스를 실행 합니다.
        /// </summary>
        /// <param name="state"></param>
        protected virtual void RunService(object state)
        {
            try
            {
                ServiceStart();

                WriteLog("START", String.Format("SERVICE START. {0}", GetType().Module.Name));
                var col = Utility.GetConfigValues();

                StringBuilder sb = new StringBuilder();

                foreach (string name in col)
                {
                    string message = String.Format("{0}={1}", name, col[name]);
                    WriteLog("START", message);
                    sb.AppendLine(message);
                }

#if !DEBUG
                AppendServiceLog(ActionType.SERVICE_START, null, sb.ToString());
                InsertServiceLogData();
#endif

                while (!_stop)
                {
                    int checkInterval = Interval * 1000;

                    try
                    {
                        Execute();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            AppendServiceLog(ex);
                            WriteLog(ex);
                        }
                        catch { }
                    }

                    // 매 주기 종료 후 서비스 로그 INSERT 후 CLEAR
                    InsertServiceLogData();

                    // Service Stop 시 원활하게 Stop이 될 수 있도록 Interval 시간 범위에서 1초 간격으로 Stop 이 되었는지를 확인
                    int currTime = 0;
                    while (checkInterval > currTime)
                    {
                        if (_stop)
                        {
                            break;
                        }

                        int sleep = 1000;
                        System.Threading.Thread.Sleep(sleep);
                        currTime += sleep;
                    }

                    // refresh config
                    Utility.RefreshConfig();
                }
            }
            catch (Exception gex)
            {
                WriteLog(gex);
            }
            finally
            {
                _stopped = true;
            }
        }

        protected void WriteLog()
        {
            WriteLog(null, null);
        }

        protected void WriteLog(string message)
        {
            WriteLog(null, message);
        }

        protected void WriteLog(string category, string message)
        {
            if (String.IsNullOrWhiteSpace(category))
                _log.WriteLog(message, LogPath, LogLevel);
            else
                _log.WriteLog(string.Format("[{0, -15}] {1}", category, message), LogPath, LogLevel);
        }

        protected void WriteLog(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Error Message : " + ex.Message);
            sb.AppendLine("Stack Trace : " + ex.Message);

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Error Message : " + ex.InnerException.Message);
                sb.AppendLine("Inner Stack Trace : " + ex.InnerException.Message);
            }

            WriteLog("ERROR", sb.ToString());
        }

        /// <summary>
        /// 서비스 로그 데이터를 TABLE에 추가합니다.
        /// </summary>
        private void InsertServiceLogData()
        {
            try
            {
                if (UseTqcServiceLog && ServiceLogDataList.Count > 0)
                {
                    ServiceLog log = new ServiceLog();
                    log.InsertData(ServiceLogDataList);
                }
            }
            catch (Exception ex)
            {
                WriteLog("ERROR", "InsertServiceLogData : " + ex.Message);
            }
            finally
            {
                ServiceLogDataList.Clear();
            }
        }

        /// <summary>
        /// 서비스 로그 데이터를 추가합니다.
        /// </summary>
        protected void AppendServiceLog(ServiceLogData data)
        {
            ServiceLogDataList.Add(data);
        }

        /// <summary>
        /// 서비스 로그 데이터를 추가합니다.
        /// </summary>
        protected void AppendServiceLog(Exception ex, string fileName = null, string equipID = null, string lotID = null, string waferID = null, string handler = null)
        {
            ActionType action = ActionType.ERROR;

            if (ex is System.IO.IOException)
                action = ActionType.WARNING;

            AppendServiceLog(action, ex.Message, ex.StackTrace, fileName, equipID, lotID, waferID, handler);
        }

        /// <summary>
        /// 서비스 로그 데이터를 추가합니다.
        /// </summary>
        protected void AppendServiceLog(ActionType action, string message = null, string detailMessage = null, string fileName = null, string equipID = null, string lotID = null, string waferID = null, string handler = null, int? executeTime = null)
        {
            try
            {
                ServiceLogData data = new ServiceLogData()
                {
                    FACTORY = Factory,
                    SERVICE_NAME = GetShortServiceName(),
                    SERVER_NAME = Environment.MachineName,
                    ACTION = action.ToString(),
                    FILE_NAME = fileName,
                    EQUIP_ID = String.IsNullOrEmpty(equipID) ? String.Empty : equipID,
                    LOT_ID = String.IsNullOrEmpty(lotID) ? String.Empty : lotID,
                    WAFER_ID = String.IsNullOrEmpty(waferID) ? String.Empty : waferID,
                    HANDLER = String.IsNullOrEmpty(handler) ? String.Empty : handler,
                    EXECUTE_TIME = executeTime,
                    MESSAGE = message,
                    DETAIL_MSG = detailMessage
                };

                AppendServiceLog(data);
            }
            catch (Exception ex)
            {
                WriteLog("Error", "AppendServiceLog : " + ex.Message);
            }
        }

        /// <summary>
        /// config 파일의 설정값을 가져옵니다.
        /// </summary>
        protected string GetConfigValue(string name)
        {
            return Utility.GetConfigValue(name); 
        }

        /// <summary>
        /// 뒤에서 . 2번째까지의 명칭을 가져옵니다. DACrux.TEST.PCM.StatusService --> PCM.StatusService
        /// </summary>
        /// <returns></returns>
        private string GetShortServiceName()
        {
            string name = GetType().Namespace;
            int idx = name.Length;

            for (int i = 0; i < 2; i++)
            {
                idx = name.LastIndexOf('.', idx - 1);

                if (idx < 0)
                    break;
            }

            if (idx > 0)
                return name.Substring(idx + 1);
            else
                return name;
        }

        /// <summary>
        /// 추가로 데이터 및 이미지 파일을 복사해야 할 경로를 나타냅니다.
        /// </summary>
        protected string[] GetAdditionalCopyPathArray()
        {
            string text = Utility.GetConfigValue("ADDITIONAL_COPY_PATH");

            if (String.IsNullOrEmpty(text))
                return new string[] { };

            string[] arr = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            return arr;
        }

        protected string GetPathMinus(string path, string minusPath)
        {
            if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(minusPath))
                return path;

            return path.Replace(minusPath, String.Empty).Trim('\\');
        }

        #region 프로퍼티

        public int Interval
        {
            get { return Utility.GetIntValueFromConfig("INTERVAL", DEFAULT_INTERVAL); }
        }

        public string Factory
        {
            get { return Utility.GetConfigValue("FACTORY"); }
        }

        public string DataPath
        {
            get { return Utility.GetConfigValue("DATA_PATH"); }
        }

        public string BackupPath
        {
            get { return Utility.GetConfigValue("BACKUP_PATH"); }
        }

        public string ErrorPath
        {
            get { return Utility.GetConfigValue("ERROR_PATH"); }
        }

        public string LogPath
        {
            get { return Utility.GetConfigValue("LOG_PATH"); }
        }

        public int LogLevel
        {
            get { return Utility.GetIntValueFromConfig("LOG_LEVEL", 0); }
        }

        public string RemoveGarbageFileExtension
        {
            get { return Utility.GetConfigValue("REMOVE_GARBAGE_FILE_EXTENSION"); }
        }

        public string EquipID
        {
            get { return Utility.GetConfigValue("EQUIP_ID"); }
        }

        /// <summary>
        /// 원본 데이터 파일을 복사할 위치를 나타냅니다.
        /// </summary>
        public string OriginalFileBackupPath
        {
            get { return Utility.GetConfigValue("ORIGINAL_FILE_BACKUP_PATH"); }
        }

        /// <summary>
        /// TQC_SERVICE_LOG 테이블에 로그를 저장할 것인지를 가져옵니다.
        /// </summary>
        public bool UseTqcServiceLog
        {
            get 
            {
                string val = Utility.GetConfigValue("USE_TQC_SERVICE_LOG");
                return String.IsNullOrEmpty(val) ? false : val.ToUpper() == Boolean.TrueString.ToUpper();
            }
        }

        protected StopWatch StopWatch
        {
            get;
            private set;
        }

        protected List<ServiceLogData> ServiceLogDataList
        {
            get;
            private set;
        }

        /// <summary>
        /// 서비스 종료를 요청하였는지를 가져옵니다.
        /// </summary>
        protected bool IsReqeustServiceStop
        {
            get { return _stop; }
        }

        /// <summary>
        /// Error 발생한 File 에 대한 Bacup 경로
        /// </summary>
        /// <param name="strLotID"></param>
        /// <returns></returns>
        protected string GetErrorFullPath(string strFileName, string strEquipID)
        {
            DateTime dt = DateTime.Now;
            try
            {
                if (string.IsNullOrEmpty(strEquipID) == true)
                    strEquipID = "UNKNOWN";

                string strSubPath = string.Format(@"{0:0000}\{1:00}\{2:00}\{3}", dt.Year, dt.Month, dt.Day, strEquipID);

                //Directory 확인 후 없으면 생성
                if (!System.IO.Directory.Exists(System.IO.Path.Combine(ErrorPath, strSubPath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ErrorPath, strSubPath));

                return System.IO.Path.Combine(ErrorPath, strSubPath, strFileName);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
