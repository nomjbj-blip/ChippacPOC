using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace DACrux.SEMDMS.FileMonitor
{
    public struct RESULTFILE
    {
        public string EquipID;
        public string ResultFile;
        public string Backup;
        public string Service;
        public bool SVC_FLAG;
    }

    public class KLARFMonitor
    {
        private Thread m_MonitorThread = null;
        private bool m_ThreadHelth = false;
        private int m_iReflashTerm = 60 * 60 * 1000; // 60분 * 60초 * 1000 밀리세크 = 1시간
        private int m_iScanTerm = 1000; // 60분 * 60초 * 1000 밀리세크 = 1시간
        private DataTable m_dtEquipment = null;
        private List<RESULTFILE> m_arrResult = null;
        private List<string> m_arrEquipName = null;
        private List<Process> m_arrEquipProc = null;

        private string m_Exec = string.Empty;

        System.Diagnostics.EventLog m_oEventLog;
        private string[] ImageFile = new string[] { "GIF", "JPG", "JPEG", "TIFF", "EMF", "BMP", "EXIF", "ICO", "ICON", "PNG", "WMF" };

        public KLARFMonitor()
        {
            m_oEventLog = new System.Diagnostics.EventLog();
            m_oEventLog.Source = "DACrux/DMS";
            m_oEventLog.Log = "DMSMonitorServiceLog";

            if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
            {
                System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
            }
        }

        public bool StartMonitor(string ExecFile, int ScanPriodeMillisec, int EquipInfoReflashMillisec)
        {
            try
            {
                m_iReflashTerm = Math.Max(EquipInfoReflashMillisec, 60000);
                m_iScanTerm = Math.Max(ScanPriodeMillisec, 500);

                return StartMonitor(ExecFile);
            }
            catch (Exception ex)
            {
                /// LOG
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry(string.Format("Exception on StartMonitor [{0}]", ex.Message)
                    , EventLogEntryType.Error);
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                return false;
            }
        }

        public bool StartMonitor(string ExecFile)
        {
            try
            {
                if (ExecFile.Length == 0 || !File.Exists(ExecFile)) return false;


                m_Exec = ExecFile;
                
                /// LOG
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry(string.Format("Monitor init parameter [Reflash:{0}] ,[Scan Period:{1}],[Parser:{2}]"
                                    , m_iReflashTerm
                                    , m_iScanTerm
                                    , ExecFile));
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////


                ThreadStart stThread = new ThreadStart(MonitorThread);
                m_MonitorThread = new Thread(stThread);
                m_MonitorThread.IsBackground = true;
                m_MonitorThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MonitorThread()
        {

#if DEBUG
            System.Threading.Thread.Sleep(10000);
#endif
            TimeSpan tsPeriod = new TimeSpan();
            DateTime dtReadEquipmentTime = DateTime.Now.AddHours(-24);
            try
            {
                m_ThreadHelth = true;
                m_arrResult = new List<RESULTFILE>();
                m_arrEquipProc = new List<Process>();
                m_arrEquipName = new List<string>();

                while (m_ThreadHelth)
                {
                    System.Threading.Thread.Sleep(m_iScanTerm);

                    /// Equipment 정보를 읽는다.
                    /// /////////////////////////////////////////////////////////////////////////////////////////////////////
                    tsPeriod = DateTime.Now - dtReadEquipmentTime;
                    if (tsPeriod.TotalMilliseconds > m_iReflashTerm)
                    {
                        try
                        {
                            DACrux.SEMDMS.BSL.DMSFileMonitor oDmsMonitor = new DACrux.SEMDMS.BSL.DMSFileMonitor();
                            m_dtEquipment = oDmsMonitor.GetEquipment();
                            dtReadEquipmentTime = DateTime.Now;
                        }
                        catch(Exception ex)
                        {
                            /// LOG
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            m_oEventLog.WriteEntry(string.Format("Exception on equipments Infomation[{0}]", ex.Message)
                                , EventLogEntryType.Warning);
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                    }

                    if (m_dtEquipment == null || m_dtEquipment.Rows.Count == 0)
                    {
                        continue;
                    }

                    /// Directory Search 하여 File을 찾아 Queue에 넣는다.
                    /// /////////////////////////////////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < m_dtEquipment.Rows.Count; i++)
                    {
                        try
                        {
                            //int idxProc = m_arrEquipName.IndexOf(m_dtEquipment.Rows[i]["EQUIP_ID"].ToString());
                            //if (idxProc < 0)
                            //{
                            //    m_arrEquipName.Add(m_dtEquipment.Rows[i]["EQUIP_ID"].ToString());
                            //    m_arrEquipProc.Add(new Process());
                            //}
                            //else
                            //{
                            //    if (m_arrEquipProc[idxProc].HasExited)
                            //    {
                            //        m_arrEquipName.RemoveAt(idxProc);
                            //        m_arrEquipProc.RemoveAt(idxProc);

                            //        m_arrEquipName.Add(m_dtEquipment.Rows[i]["EQUIP_ID"].ToString());
                            //        m_arrEquipProc.Add(new Process());
                            //    }
                            //}


                            string strPath = m_dtEquipment.Rows[i]["RESULT_PATH"].ToString().Replace("/", @"\").Replace(@"\\", @"\");
                            string strBackup = m_dtEquipment.Rows[i]["BACKUP_PATH"].ToString().Replace("/", @"\").Replace(@"\\", @"\");
                            string strService = m_dtEquipment.Rows[i]["SERVICE_PATH"].ToString().Replace("/", @"\").Replace(@"\\", @"\");
                            bool bSvcFlag = m_dtEquipment.Rows[i]["SERVICE_PATH"].ToString().Equals("Y");


                            string[] strFiles = Directory.GetFiles(strPath, "*");

                            if (strFiles != null && strFiles.Length > 0)
                            {
                                for (int f = 0; f < strFiles.Length; f++)
                                {
                                    // Image일때 제외
                                    if (Array.IndexOf(ImageFile, Path.GetExtension(strFiles[f])) > -1) continue;

                                    // Klarf 가 아니면 제외
                                    StreamReader sr = new StreamReader(strFiles[f]);
                                    if (sr.ReadLine().StartsWith("FileVersion") == false)
                                    {
                                        sr.Close();
                                        sr.Dispose();
                                        sr = null;
                                        continue;
                                    }

                                    RESULTFILE oResult = new RESULTFILE();
                                    oResult.EquipID = m_dtEquipment.Rows[i]["EQUIP_ID"].ToString();
                                    oResult.ResultFile = strFiles[f];
                                    oResult.Backup = strBackup;
                                    oResult.Service = strService;
                                    oResult.SVC_FLAG = bSvcFlag;

                                    if (m_arrResult.IndexOf(oResult) < 0)
                                        m_arrResult.Add(oResult);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            /// LOG
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            m_oEventLog.WriteEntry(string.Format("Error on reflash equipments information...[{0},{1}]",m_dtEquipment.Rows[i]["EQUIP_ID"].ToString(), ex.Message)
                                ,EventLogEntryType.Warning);
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            /// 문제가 있는 설비는 우선 제외시킨다.
                            m_dtEquipment.Rows.RemoveAt(i);

                        }
                    }


                    /// Queue에 있는 File을 Argument로 Process를 실행시킨다.
                    /// /////////////////////////////////////////////////////////////////////////////////////////////////////
                    /// 
                    List<int> iRemoveIdxs = new List<int>();
                    for (int i = 0; i < m_arrResult.Count; i++)
                    {
                        try
                        {
                            string strEquipID = m_arrResult[i].EquipID;
                            string strResult = m_arrResult[i].ResultFile;

                            int idxProc = m_arrEquipName.IndexOf(strEquipID);

                            if (idxProc > -1)
                            {
                                if (m_arrEquipProc[idxProc].StartInfo.FileName.Length == 0 || m_arrEquipProc[idxProc].HasExited)
                                {
                                    m_arrEquipName.RemoveAt(idxProc);
                                    m_arrEquipProc.RemoveAt(idxProc);
                                }
                                else
                                {
                                    continue;
                                }
                            }


                            m_arrEquipName.Add(strEquipID);

                            Process proc = new Process();
                            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(m_Exec);
                            proc.StartInfo.FileName = m_Exec;

                            if (m_arrResult[i].SVC_FLAG)
                            {
                                proc.StartInfo.Arguments = string.Format("-K {0} -S {1} -B {2}", strResult, m_arrResult[i].Service, m_arrResult[i].Backup);
                            }
                            else
                            {
                                proc.StartInfo.Arguments = string.Format("-K {0} -B {1}", strResult, m_arrResult[i].Backup);
                            }

                            proc.Start();
                            m_arrEquipProc.Add(proc);

                            iRemoveIdxs.Add(i);
                        }
                        catch (Exception ex)
                        {
                            /// LOG
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            m_oEventLog.WriteEntry(string.Format("Error on runnig parser...[{0},{1}]", ex.Message, m_Exec)
                                , EventLogEntryType.Error);
                            /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                    }

                    for (int irem = iRemoveIdxs.Count - 1; irem >= 0; irem--)
                    {
                        m_arrResult.RemoveAt(iRemoveIdxs[irem]);
                    }
                }
            }
            catch (Exception ex)
            {
                /// LOG
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry(string.Format("Error on runnig parser...[{0},{1}]", ex.Message, m_Exec));
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

        public bool StopMonitor()
        {
            int iTimeOut = 30000;
            try
            {
                /// LOG
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry("Monitor stopping...");
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m_MonitorThread != null)
                {
                    m_ThreadHelth = false;
                    DateTime dtStop = DateTime.Now;

                    while (m_MonitorThread.IsAlive)
                    {

                        System.Threading.Thread.Sleep(30);
                        TimeSpan ts = DateTime.Now - dtStop;
                        if (iTimeOut < ts.TotalMilliseconds) break;
                    }
                }

                if (m_MonitorThread.IsAlive)
                {
                    /// LOG
                    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    m_oEventLog.WriteEntry("Can't monitor stop.It so busy. try again please");
                    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    return false;
                }

                m_MonitorThread = null;
                /// LOG
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry("Monitor stop");
                /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
