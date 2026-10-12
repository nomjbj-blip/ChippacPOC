using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.ServiceProcess;

namespace DACrux.RPNet.Service
{
    partial class DACruxRPNet : ServiceBase
    {
        System.Diagnostics.EventLog m_oEventLog = null;

        public void Start()
        {
            OnStart(null);
        }

        public DACruxRPNet()
        {
            InitializeComponent();
            m_oEventLog = new System.Diagnostics.EventLog();
            m_oEventLog.Source = "DACruxRPNet";
        }

        protected override void OnStart(string[] args)
        {
            try{
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                             + "\\" + System.Configuration.ConfigurationManager.AppSettings["RPNetRemoteService"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

            
                            // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry("DACruxRPNet Service Started");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxRPNet [{0}]", ex.Message)
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
                m_oEventLog.WriteEntry("DACruxRPNet Service Stoped!!!!");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxRPNet [{0}]", ex.Message)
                      , EventLogEntryType.Error);
            }
        }
    }
}
