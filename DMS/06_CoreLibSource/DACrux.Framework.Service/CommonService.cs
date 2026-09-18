using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;
using System.IO;

namespace DACrux.Framework.Service
{
    public partial class CommonService : ServiceBase
    {
        System.Diagnostics.EventLog m_oEventLog = null;
        #region Creator

        public CommonService()
        {
            InitializeComponent();

            m_oEventLog = new System.Diagnostics.EventLog();
            m_oEventLog.Source = "DACrux";
            m_oEventLog.Log = "DACruxServiceLog";

        }

        public void Start()
        {
            OnStart(null);
        }

        #endregion

        protected override void OnStart(string[] args)
        {

            try
            {
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                    + "\\" + System.Configuration.ConfigurationManager.AppSettings["MIRACOM_QMS_COMMON"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

                // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                }
                m_oEventLog.WriteEntry("DACruxCore & Framework Service Started");
                ///////////////////////////////////////////////////////////////////////////////////////////////
                
                
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxCore [{0}]", ex.Message)
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
                if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                }
                m_oEventLog.WriteEntry("DACruxCore & Framework Service Stoped!!!!");
                ///////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                m_oEventLog.WriteEntry(string.Format("Exception on DACruxCore [{0}]", ex.Message)
                      , EventLogEntryType.Error);
            }
        }
    }
}
