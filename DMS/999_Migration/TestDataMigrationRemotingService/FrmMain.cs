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

namespace TestDataMigrationRemotingService
{
    public partial class FrmMain : Form
    {
        TcpChannel channel;

        public FrmMain()
        {
            InitializeComponent();

            TestData d = new TestData();
            string str = d.Sysdate();
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

            BaseInfo.Factory = Factory;
                        
            // remoting
            channel = new TcpChannel(MiracomTPS.GetPort(Factory));
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(TestDataMigrationRemotingService.MiracomTPS), "MiracomTPS", WellKnownObjectMode.SingleCall);

            Text = String.Format("[{0}] Remoting Service", Factory);
            btnRun.Enabled = false;
            rdoFab1.Enabled = rdoFAB2.Enabled = false;
        }

        public string Factory
        {
            get { return rdoFab1.Checked ? rdoFab1.Text : rdoFAB2.Checked ? rdoFAB2.Text : null; }
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
