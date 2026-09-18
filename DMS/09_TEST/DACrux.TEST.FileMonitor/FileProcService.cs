using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Runtime.Remoting;
using DACrux.SP.Common;
using DACrux.TEST.BSL;

namespace DACrux.TEST.FileMonitor
{
    partial class FileProcService : ServiceBase
    {
        public struct WatchFileInfo
        {
            public string file;
            public bool compareOne;
        }

        System.Diagnostics.Process parseProcess = null;
        System.Threading.Thread fileThread = null;
        System.Threading.Thread archiveThread = null;

        static Queue<WatchFileInfo> FileList = null;
        static Log log = null;
        static string strPath = string.Empty;
        static string strExcutePath = string.Empty;
        static string strLogPath = string.Empty;
        static int iLogLevel = 0;
        static int iProcessCount = 0;  // 기본 10개
        static int iRunProcessCount = 0;
        static List<string> WatcherForder = null;
        //static List<string> ParsingList = null; // 큐에 파일을 중복으로 저장하지 않게 방지하는 코드가 있지만 추가되는 현상이 발생하므로 현재 파싱 중인 리스트를 비교하여 파싱여부를 확인한다.
        DACrux.TEST.BSL.ProbeAdmin oCustomer = null;


        public FileProcService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                        + "\\" + System.Configuration.ConfigurationManager.AppSettings["MIRACOM_DACRUX_TEST"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

                #region [ Config File Read ]
                
                // Config File Read
                strPath = System.Configuration.ConfigurationManager.AppSettings["DATA_ROOT"];
                strExcutePath = System.Configuration.ConfigurationManager.AppSettings["ExcutePath"];
                strLogPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["LogLevel"], out iLogLevel);
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaxProcessCount"], out iProcessCount);
                
                if (iProcessCount == 0) // 설정값을 반환받지 못할 경우 기본값 10으로 설정
                    iProcessCount = 10;
                
                #endregion

                FileList = new Queue<WatchFileInfo>();
                log = new Log(iLogLevel);

                #region [ 경로 체크 ]

                // 경로 체크
                if (strPath == null || strExcutePath == null || strLogPath == null ||
                    strPath == string.Empty || strExcutePath == string.Empty || strLogPath == string.Empty)
                {
                    string tempLogPath = strLogPath == null ? @"D:\BUMP\LOG" : (strLogPath == string.Empty ? @"D:\BUMP\LOG" : strLogPath);
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), "--------------------------------------------------", tempLogPath, 0);
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}] File Folder\t: {1}", "START ERROR", strPath), tempLogPath, 0);
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Work Folder\t: {1}", "", strExcutePath), tempLogPath, 0);
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Log Folder\t: {1}", "", strLogPath), tempLogPath, 0);
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Log Level\t: {1}", "", iLogLevel), tempLogPath, 0);

                    OnStop();
                    return;
                }

                #endregion

                //log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), "--------------------------------------------------", strLogPath, 0);
                //log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}] File Folder\t: {1}", "START", strPath), strLogPath, 0);
                //log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Work Folder\t: {1}", "", strExcutePath), strLogPath, 0);
                //log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Log Folder\t: {1}", "", strLogPath), strLogPath, 0);
                //log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format(" {0, -15}  Log Level\t: {1}", "", iLogLevel), strLogPath, 0);

                #region [ Get AUTO_FLAG is 'Y' ]

                oCustomer = new DACrux.TEST.BSL.ProbeAdmin();
                DataTable dt = oCustomer.GetCustomerList();
                if (dt == null)
                {
                    log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}] {1}", "SERVICE START FAIL", "DB 고객 정보를 가져오지 못했습니다."), strLogPath, 0);
                    OnStop();
                }

                WatcherForder = new List<string>();
                //ParsingList = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    WatcherForder.Add(dr["MAP_RCVDIR"].ToString());
                }

                #endregion 

                #region [ Watcher Setup ]

                // Watcher Setup
                this.m_RFWatcher.Path = strPath;
                this.m_RFWatcher.IncludeSubdirectories = true;
                this.m_RFWatcher.EnableRaisingEvents = true;

                log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}] {1}", "WATCHER SETUP", strPath), strLogPath, 4);

                #endregion

                // 이미 존재하는 파일을 찾아 리스트에 추가
                ExistFileEventAtRestart();
                log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}] Tatle Count\t: {1} Files", "ADD FILES", FileList.Count), strLogPath, 0);

                fileThread = new System.Threading.Thread(new System.Threading.ThreadStart(FileThread));
                fileThread.Start();

                archiveThread = new System.Threading.Thread(new System.Threading.ThreadStart(ArchiveThread));
                archiveThread.Start();
            }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "OnStart", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);
            }
        }

        protected override void OnStop()
        {
            try
            {
                m_RFWatcher.EnableRaisingEvents = false;
                System.Threading.Thread.Sleep(3000);

                if (FileList != null)
                {
                    FileList.Clear();
                    FileList = null;
                }

                string tempLogPath = strLogPath == null ? @"D:\BUMP\LOG" : (strLogPath == string.Empty ? @"D:\BUMP\LOG" : strLogPath);
                log.WriteLog(string.Format("[{0, -15}]", "SERVICE"), string.Format("[{0, -15}]", "END", strPath), tempLogPath, 0);
            }
            finally
            {    
                log = null;
                if (fileThread != null && fileThread.IsAlive)
                {
                    fileThread.Abort();
                    fileThread.Join();
                }

                fileThread = null;

                if (archiveThread != null && archiveThread.IsAlive)
                {
                    archiveThread.Abort();
                    archiveThread.Join();
                }

                archiveThread = null;
            }
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (File.GetAttributes(e.FullPath).ToString().StartsWith("Archive") == true) // File이면 true, Folder면 false
                {    
                    AddFile(e.FullPath);
                }
            }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION", "fileSystemWatcher1_Created", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);
            }
        }

        private void FileThread()
        {
            DateTime tFindTime = DateTime.Now.AddMinutes(10);
            int iPosition = 0;

            try
            {
                WatchFileInfo wfi;
                log.WriteLog(string.Format("[{0, -15}]", "WATCHER"), string.Format("[{0, -15}]", "START", strPath), strLogPath, 4);

                while (m_RFWatcher.EnableRaisingEvents)
                {
                    iPosition = 1;
                    while (FileList != null && FileList.Count > 0 && m_RFWatcher.EnableRaisingEvents)
                    {
                        iPosition = 2;
                        // 현재 실행중인 프로세서와 최대 실행 수를 비교하여 프로세서 생성 수를 조절한다.
                        if (iRunProcessCount < iProcessCount)
                        {
                            iPosition = 3;
                            try { wfi = FileList.Dequeue(); }
                            catch (InvalidOperationException) { continue; }

                            iPosition = 4;
                            if (wfi.file == null || !File.Exists(wfi.file) || wfi.file.Split('_')[0].ToString().Equals("")) continue;

                            iPosition = 5;
                            if (wfi.compareOne == false)
                            {
                                try
                                {
                                    iPosition = 6;
                                    // 파싱할 파일을 읽을 수 있는지 테스트
                                    FileStream fs = File.OpenRead(wfi.file);
                                    iPosition = 7;
                                    if (fs != null)
                                        wfi.compareOne = true;

                                    iPosition = 8;
                                    fs.Close();
                                }
                                catch (System.IO.IOException) { iPosition = 9; };

                                iPosition = 10;
                                FileList.Enqueue(wfi);
                                iPosition = 11;
                                continue;
                            }
                            else
                            {
                                iPosition = 12;
                                // 현재 파일 파싱 시작
                                if (!File.Exists(strExcutePath))
                                {
                                    iPosition = 13;
                                    log.WriteLog(string.Format("[{0, -15}]", "WATCHER"), string.Format("[{0, -15}] {1}", "NOT EXIST FILE", strExcutePath), strLogPath, 0);

                                    iPosition = 14;
                                    continue;
                                }

                                iPosition = 15;
                                string strFolder = wfi.file.Substring(wfi.file.IndexOf(strPath), wfi.file.LastIndexOf("\\"));
                                iPosition = 16;
                                string arguements = string.Format("-D \"{0}\" -R \"{1}\"", strFolder, wfi.file);
                                iPosition = 17;
                                System.Diagnostics.ProcessStartInfo si = new ProcessStartInfo(strExcutePath, arguements);
                                iPosition = 18;
                                parseProcess = System.Diagnostics.Process.Start(si);
                                iRunProcessCount++;
                                iPosition = 19;

                                log.WriteLog(string.Format("[{0, -15}]", "WATCHER"), string.Format("[{0, -15}] {1}", "PARSING START", arguements), strLogPath, 0);
                            }
                        }
                        Thread.Sleep(10);
                        iPosition = 20;
                        iRunProcessCount = (Process.GetProcessesByName(strExcutePath.Substring(strExcutePath.LastIndexOf("\\") + 1).Replace(".exe", ""))).Length;
                    }
                    Thread.Sleep(1);

                    if (iRunProcessCount < 1)
                    {
                        if (tFindTime < DateTime.Now)
                        {
                            try
                            {
                                iPosition = 21;
                                ProbeAdmin oCustomer = new ProbeAdmin();

                                DataTable dt = oCustomer.GetCustomerList();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        if (WatcherForder.IndexOf(dr["MAP_RCVDIR"].ToString()) == -1)
                                            WatcherForder.Add(dr["MAP_RCVDIR"].ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + iPosition.ToString(), "FileThread", ex.Message), strLogPath, 0);
                            }

                            ExistFileEventAtRestart();
                            FileList.TrimExcess();
                            tFindTime = DateTime.Now.AddMinutes(10);
                        }
                    }
                }
                log.WriteLog(string.Format("[{0, -15}]", "WATCHER"), string.Format("[{0, -15}]", "END", strPath), strLogPath, 4);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + iPosition.ToString(), "FileThread", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);

                string msg = string.Empty;
                msg += "Error Function : FileThread()\n";
                msg += string.Format("Error Message : {0}\n", ex.Message);

                SendMail("Map Parser Exception 발생 긴급 체크", msg, "house@hanamicron.co.kr;thlee@hanamicron.co.kr;ym9720.kim@miracom.co.kr");
            }
        }

        private void AddFile(string file)
        {
            //bool IsSecMap = false;

            // DB상에 AUTO FLAG가 Y인 폴더만 파일을 가져온다.
            string tempFile = file.Replace(strPath, "").TrimStart('\\');
            foreach (string Folder in WatcherForder.ToArray())
            {
                if (tempFile.StartsWith(Folder) == true && tempFile.ToUpper().Contains("ERROR") == false)
                {
                    WatchFileInfo wfi;
                    wfi = FileList.ToList().Find(delegate(WatchFileInfo wf)
                                                        {
                                                            return wf.file == file;
                                                        });
                    // 중복이 없으면 추가
                    if (wfi.file == null)
                    {
                        wfi.file = file;
                        wfi.compareOne = false;

                        if (FileList == null) FileList = new Queue<WatchFileInfo>();
                        FileList.Enqueue(wfi);

                        log.WriteLog(string.Format("[{0, -15}]", "WATCHER"), string.Format("[{0, -15}] {1}", "ADD FILE", file), strLogPath, 1);
                    }
                    return;
                }
            }
        }

        private void ExistFileEventAtRestart()
        {
            try
            {
                List<FileInfo> lstSearchFile = null;
                System.IO.DirectoryInfo[] aWatchingFolder = null;

                // 감시 중인 폴더의 하위 폴더 리스트를 모두 가져온다.
                aWatchingFolder = (new DirectoryInfo(m_RFWatcher.Path)).GetDirectories("*", System.IO.SearchOption.AllDirectories);

                for (int i = 0; i < aWatchingFolder.Length; i++)
                {
                    if (lstSearchFile == null)
                        lstSearchFile = new List<FileInfo>();

                    // 각 폴더의 파일 리스트를 가져온다.
                    FileInfo[] files = aWatchingFolder[i].GetFiles();

                    if (files.Length > 0)
                        lstSearchFile.AddRange(files);
                }
                lstSearchFile.Sort(new CompareFileInfoEntries());


                for (int j = 0; j < lstSearchFile.Count; j++)
                {
                    try
                    {
                        AddFile(lstSearchFile[j].FullName);
                    }
                    catch (Exception ex)
                    {
                        log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1}", "AddFile", ex.Message), strLogPath, 0);
                        log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1}", "ExistFileEventAtRestart", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);
            }
        }

        public void ArchiveThread()
        {
            DateTime tArchiveTime = DateTime.Now.AddMinutes(-1);
            System.IO.DirectoryInfo[] aBackUpFolder = null;
            
            int iPosition = 0;
            try
            {
                
                while (m_RFWatcher.EnableRaisingEvents)
                {
                    iPosition = 1;
                    if (tArchiveTime > DateTime.Now)
                    {
                        iPosition = 2;
                        System.Threading.Thread.Sleep(900000);
                        continue;
                    }

                    iPosition = 3;
                    if (aBackUpFolder != null)
                        aBackUpFolder = null;

                    iPosition = 4;
                    aBackUpFolder = (new DirectoryInfo(m_RFWatcher.Path)).GetDirectories("*BACK", System.IO.SearchOption.AllDirectories);

                    iPosition = 5;
                    for (int i = 0; i < aBackUpFolder.Length; i++)
                    {
                        iPosition = 6;
                        System.IO.DirectoryInfo[] aArchiveFolder = aBackUpFolder[i].GetDirectories();

                        iPosition = 7;
                        foreach (DirectoryInfo di in aArchiveFolder)
                        {
                            iPosition = 8;
                            if (!FileMove(di))
                                continue;
                            iPosition = 9;
                            if (!ZipArchive(di))
                                continue;
                            iPosition = 10;
                            DeleteFolder(di.FullName);
                        }
                    }
                    iPosition = 11;
                    tArchiveTime = DateTime.Now.AddHours(1);
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + iPosition.ToString(), "ArchiveThread", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);

                string msg = string.Empty;
                msg += "Error Function : ArchiveThread()\n";
                msg += string.Format("Error Message : {0}\n", ex.Message);

                SendMail("Map Parser Exception 발생 긴급 체크", msg, "house@hanamicron.co.kr;thlee@hanamicron.co.kr;ym9720.kim@miracom.co.kr");
            }
        }

        private bool FileMove(DirectoryInfo di)
        {
            int nStep = 0;
            try
            {
                FileInfo[] files = di.GetFiles();
                nStep = 10;
                foreach (FileInfo fi in files)
                {
                    nStep = 20;
                    if (fi.LastWriteTime.AddDays(1) > DateTime.Now || fi.Extension.ToUpper().Equals(".ZIP"))
                        continue;

                    nStep = 30;
                    string fTemp = Path.Combine(di.FullName, fi.LastWriteTime.ToString("yyyyMMdd"));
                    if (!Directory.Exists(fTemp))
                        Directory.CreateDirectory(fTemp);

                    nStep = 40;
                    string sCurrentRunID = string.Empty;
                    if (fi.Extension.Length < 1)
                        sCurrentRunID = (fi.Name.Split('-', '_'))[0];
                    else
                        sCurrentRunID = (fi.Name.Replace(fi.Extension, "").Split('-', '_'))[0];

                    nStep = 50;
                    fTemp = Path.Combine(fTemp, sCurrentRunID);
                    if (!Directory.Exists(fTemp))
                        Directory.CreateDirectory(fTemp);

                    nStep = 60;
                    string sMoveFile = Path.Combine(fTemp, fi.Name);
                    int iCount = 0;
                    while (File.Exists(sMoveFile))
                    {
                        nStep = 70;
                        sMoveFile = string.Format("{0}_Duplicate{1:D2}", Path.Combine(fTemp, fi.Name), ++iCount);
                        System.Threading.Thread.Sleep(1);
                    }

                    nStep = 80;
                    fi.MoveTo(sMoveFile);

                    nStep = 90;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + nStep, "FileMove", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);

                string msg = string.Empty;
                msg += "Error Function : FileMove()\n";
                msg += string.Format("Error Message : {0}\n", ex.Message);

                SendMail("Map Parser Exception 발생 긴급 체크", msg, "house@hanamicron.co.kr;thlee@hanamicron.co.kr;ym9720.kim@miracom.co.kr");
                return false;
            }

        }

        private bool ZipArchive(DirectoryInfo di)
        {
            int nStep = 0;
            try
            {
                DirectoryInfo[] subDirs = di.GetDirectories();
                nStep = 10;

                foreach (DirectoryInfo subDir in subDirs)
                {
                    nStep = 20;
                    string sRunID = subDir.FullName.Substring(subDir.FullName.LastIndexOf('\\') + 1);

                    nStep = 30;
                    string sZipFile = Path.Combine(di.FullName.Replace(sRunID, ""), sRunID + ".zip");
                    int nIndex = 0;

                    nStep = 40;
                    while (File.Exists(sZipFile))
                    {
                        nStep = 50;
                        sZipFile = Path.Combine(di.FullName.Replace(sRunID, ""), sRunID + string.Format("{0:D2}.zip", ++nIndex));
                        System.Threading.Thread.Sleep(1);
                    }

                    nStep = 60;
                    using (var zip = File.OpenWrite(sZipFile))
                    {
                        nStep = 70;
                        using (var zipWriter = SharpCompress.Writer.WriterFactory.Open(zip, SharpCompress.Common.ArchiveType.Zip, SharpCompress.Common.CompressionType.None))
                        {
                            nStep = 80;
                            SharpCompress.Writer.IWriterExtensions.WriteAll(zipWriter, subDir.FullName, "*", SearchOption.AllDirectories);
                            zipWriter.Dispose();
                            nStep = 90;
                        }

                        zip.Close();
                        zip.Dispose();
                    }
                }
                nStep = 100;
                GC.Collect();
                nStep = 110;
                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + nStep, "ZipArchive", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);

                string msg = string.Empty;
                msg += "Error Function : ZipArchive()\n";
                msg += string.Format("Error Message : {0}\n", ex.Message);

                SendMail("Map Parser Exception 발생 긴급 체크", msg, "house@hanamicron.co.kr;thlee@hanamicron.co.kr;ym9720.kim@miracom.co.kr");
                return false;
            }
        }

        private void DeleteFolder(string sFolderPath)
        {
            int nStep = 0;
            try
            {
                #region [ sFolderPath 폴더 안에 폴더가 생성되어 있는 경우 파일과 폴더를 삭제함 ]
                DirectoryInfo[] di = (new DirectoryInfo(sFolderPath)).GetDirectories("*", SearchOption.AllDirectories);

                nStep = 10;
                for (int i = 0; i < di.Length; i++)
                {
                    nStep = 20;
                    FileInfo[] fi = di[i].GetFiles();

                    nStep = 30;
                    for (int j = 0; j < fi.Length; j++)
                    {
                        nStep = 40;
                        File.SetAttributes(fi[j].FullName, FileAttributes.Normal);
                        File.Delete(fi[j].FullName);
                        nStep = 50;
                    }
                }

                nStep = 60;
                for (int i = 0; i < di.Length; i++)
                {
                    nStep = 70;
                    if (Directory.Exists(di[i].FullName))
                        Directory.Delete(di[i].FullName, true);
                }

                nStep = 80;

                #endregion
            }
            catch (Exception ex) 
            {
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format("[{0, -15}] {1} : {2}", "POSITION : " + nStep, "DeleteFolder", ex.Message), strLogPath, 0);
                log.WriteLog(string.Format("[{0, -15}]", "ERROR"), string.Format(" {0, -15}  {1}", "", ex.ToString()), strLogPath, 1);

                string msg = string.Empty;
                msg += "Error Function : DeleteFolder()\n";
                msg += string.Format("Error Message : {0}\n", ex.Message);

                SendMail("Map Parser Exception 발생 긴급 체크", msg, "house@hanamicron.co.kr;thlee@hanamicron.co.kr;ym9720.kim@miracom.co.kr");
            }
        }

        public void SendMail(String subject, String message, String toList)
        {
            try
            {
                // 메세지를 만든다.
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                // 보내는 사람
                mail.From = new System.Net.Mail.MailAddress("hanamailmaster@hanamicron.co.kr", "Hana Mailing Master");

                // 받는 사람
                String[] toArray = toList.Split(';');
                foreach (String to in toArray)
                {
                    mail.To.Add(new System.Net.Mail.MailAddress(to));
                }

                // 제목
                mail.Subject = subject;

                // 내용
                mail.Body = message + "\n 본 메일은 하나마이크론(주)의 보안 사항을 포함하고 있을 수 있습니다.\n 수신 후 삭제 하도록 하십시오";

                // encoding
                mail.BodyEncoding = System.Text.Encoding.UTF8;

                // html 여부
                mail.IsBodyHtml = false;

                // 메일 우선 순위
                mail.Priority = System.Net.Mail.MailPriority.High;

                // smtp 접속 후 메일 발송
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.hanamicron.co.kr");
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                smtpClient.Port = 25;

                smtpClient.Credentials = new System.Net.NetworkCredential("HanaMailMaster\\hmicron.com", "p@ssw0rd");

                smtpClient.Send(mail);
            }
            catch
            {
            }
        }
    }

    public class CompareFileInfoEntries : IComparer<FileInfo>
    {
        public int Compare(FileInfo f1, FileInfo f2)
        {
            return (DateTime.Compare(f1.CreationTime, f2.CreationTime));
        }
    }  
}
