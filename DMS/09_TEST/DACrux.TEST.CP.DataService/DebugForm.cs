using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.CP.DataService
{
    public partial class DebugForm : Form
    {
        CPDataService obj = new CPDataService();

        public DebugForm()
        {
            InitializeComponent();

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
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            obj.StartService();
            this.WindowState = FormWindowState.Minimized;

            EnableStartButton(false);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            obj.StopService();

            EnableStartButton(true);
        }
    }
}
