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
            //m_oEventLog.Log = "DACruxSEMDMSServiceLog";
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //var file = @"D:\PROJECT\MagnaChip\DMS\UOR0057\L2618803S2D1R0_@00000@_N1.jpg";
            //ImageManager img = new ImageManager(file);

            //for (int i = 0; i < img.FrameCount; i++)
            //    img.SaveFrameImageAndThumbnail(i + 1, Path.GetDirectoryName(file));

            try{
                string strCfgPath = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""))
                    + "\\" + System.Configuration.ConfigurationManager.AppSettings["MIRACOM_DACRUX_DMS"].ToString();
                RemotingConfiguration.Configure(strCfgPath, false);

                            // Event Log 
                ///////////////////////////////////////////////////////////////////////////////////////////////
                //if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                //{
                //    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                //}
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
                //if (!System.Diagnostics.EventLog.SourceExists(m_oEventLog.Source))
                //{
                //    System.Diagnostics.EventLog.CreateEventSource(m_oEventLog.Source, m_oEventLog.Log);
                //}
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
