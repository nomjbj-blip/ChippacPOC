using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using DACrux.Data.Parser.Klarf;

namespace DACrux.SEMDMS.Service
{
    public partial class DACruxSEMDMS : ServiceBase
    {
        System.Diagnostics.EventLog m_oEventLog = null;
        public DACruxSEMDMS()
        {
            InitializeComponent();
            m_oEventLog = new System.Diagnostics.EventLog();
            m_oEventLog.Source = "DACruxSEMDMS";
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            try{
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                    + "\\" + System.Configuration.ConfigurationManager.AppSettings["MIRACOM_DACRUX_DMS"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

                            // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                m_oEventLog.WriteEntry("DACruxSEMDMS Service Started");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxSEMDMS [{0}]", ex.Message)
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
                m_oEventLog.WriteEntry("DACruxSEMDMS Service Stoped!!!!");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxSEMDMS [{0}]", ex.Message)
                      , EventLogEntryType.Error);
            }
        }
    }
}
