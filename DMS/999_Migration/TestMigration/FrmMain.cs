using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace FabTwoToMigrationTools
{
    public partial class FrmMain : Form
    {
        public static readonly int FAB1_PORT = 3333;
        public static readonly int FAB2_PORT = 3334;

        TcpChannel channel;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!(rdoFab1.Checked || rdoFAB2.Checked))
            {
                MessageBox.Show("Factory를 선택하세요.");
                return;
            }
                        
            // remoting
            channel = new TcpChannel(GetPort(Factory));
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(TestDataMigration_RemotingService.MiracomTPS), "MiracomTPS", WellKnownObjectMode.SingleCall);

            BaseInfo.Factory = Factory;

            Text = String.Format("[{0}] Remoting Service", Factory);
            btnRun.Enabled = false;
            rdoFab1.Enabled = rdoFAB2.Enabled = false;
        }

        public string Factory
        {
            get { return rdoFab1.Checked ? rdoFab1.Text : rdoFAB2.Checked ? rdoFAB2.Text : null; }
        }

        public static int GetPort(string factory)
        {
            return factory == "FAB1" ? FAB1_PORT : factory == "FAB2" ? FAB2_PORT : 9999;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrEmpty(Factory))
                return;

            channel.StopListening(null);
            RemotingServices.Disconnect(this);
            ChannelServices.UnregisterChannel(channel);
        }
    }
}
