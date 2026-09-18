using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DACrux.SEMDMS.FileMonitor
{
    public partial class FileProcService : ServiceBase
    {
        KLARFMonitor oMonitor = null;
        EventLog m_oEventLog = null;
        //string m_strSource = "DACrux/DMS";
        //string m_strLogName = "FileMonitorServiceLog";

        public FileProcService()
        {
            InitializeComponent();
            m_oEventLog = new System.Diagnostics.EventLog();
        }

        protected override void OnStart(string[] args)
        {
            string strExecute = null;
            string strEquipReflash = null;
            string strResultScanTerm = null;
            try
            {
                // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.Source = "DACrux/DMS";
                m_oEventLog.Log = "DMSMonitorServiceLog";

                if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                }
                m_oEventLog.WriteEntry("Start DACrux/DMS Monitoring Service");
                ///////////////////////////////////////////////////////////////////////////////////////////////

                strExecute = System.Configuration.ConfigurationManager.AppSettings["EXECUTE"].ToString();
                strEquipReflash = System.Configuration.ConfigurationManager.AppSettings["EQUIP_INFO_REFLASH"].ToString();
                strResultScanTerm = System.Configuration.ConfigurationManager.AppSettings["RESULT_SCAN_TERM"].ToString();

                oMonitor = new KLARFMonitor();
                oMonitor.StartMonitor(strExecute, int.Parse(strResultScanTerm), int.Parse(strEquipReflash));

            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Can't start DACrux/DMS Monitoring Service [ {0} ]", ex.Message));
            }
        }

        protected override void OnStop()
        {
            if (oMonitor != null) oMonitor.StopMonitor();
            m_oEventLog.WriteEntry("End DACrux/DMS Monitoring Service");
        }
    }
}
