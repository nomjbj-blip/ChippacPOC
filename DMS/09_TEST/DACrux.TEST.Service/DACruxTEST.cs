using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;
using System.IO;

namespace DACrux.TEST.Service
{
    partial class DACruxTEST : ServiceBase
    {
        System.Diagnostics.EventLog m_oEventLog = null;

        public void Start()
        {
            OnStart(null);
        }

        public DACruxTEST()
        {
            InitializeComponent();
            m_oEventLog = new System.Diagnostics.EventLog();
            m_oEventLog.Source = "DACruxTEST";
            //m_oEventLog.Log = "DACruxTESTServiceLog";
        }

        protected override void OnStart(string[] args)
        {
            try{
                // TODO: 여기에 서비스를 시작하는 코드를 추가합니다.
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                             + "\\" + System.Configuration.ConfigurationManager.AppSettings["MIRACOM_DACRUX_PROBE"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

            
                            // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                //if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                //{
                //    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                //}
                m_oEventLog.WriteEntry("DACruxTEST Service Started");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxTEST [{0}]", ex.Message)
                                      , EventLogEntryType.Error);
            }
            finally
            {
            }

        }

        protected override void OnStop()
        {
            try
            {
                // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                //if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                //{
                //    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                //}
                m_oEventLog.WriteEntry("DACruxTEST Service Stoped!!!!");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxTEST [{0}]", ex.Message)
                      , EventLogEntryType.Error);
            }
        }
    }
}
