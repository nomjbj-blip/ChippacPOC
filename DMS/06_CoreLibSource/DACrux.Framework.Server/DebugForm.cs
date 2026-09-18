using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Server
{
    public partial class DebugForm : Form
    {
        DataServiceBase m_service;
        int  m_time;

        public DebugForm(DataServiceBase service)
        {
            InitializeComponent();

            m_service = service;
            Text = Application.ProductName;
        }

        private void EnableStartButton(bool enabled)
        {
            btnStart.Enabled = enabled;
            btnStop.Enabled = !enabled;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EnableStartButton(true);
            UpdateLabel();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            m_service.StartService();

            timer1.Start();
            EnableStartButton(false);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_service.StopService();

            m_time = 0;
            timer1.Stop();
            EnableStartButton(true);
            UpdateLabel();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_time++;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            lblTime.Text = String.Format("{0:00}:{1:00}:{2:00}", m_time / 3600, m_time / 60 % 60, m_time % 60);
        }
    }
}
