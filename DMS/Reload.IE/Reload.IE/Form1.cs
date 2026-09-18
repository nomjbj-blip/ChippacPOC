using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Reload.IE
{
    public partial class Form1 : Form
    {
        private Timer timer1 = null;
        private Timer timer2 = null;
        private int Count = 0;

        public Form1()
        {
            InitializeComponent();
            ChangedMode(true);

            timer2 = new Timer();
            timer2.Interval = 1000;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Start();
        }

        //-------------------------------------------------------

        private void btnStart_Click(
            object sender, 
            EventArgs e
            )
        {
            int interval = Convert.ToInt32(numInterval.Value);
            timer1 = new Timer();
            timer1.Interval = interval * 60 * 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            ChangedMode(false);

            WindowState = FormWindowState.Minimized;
        }

        //-------------------------------------------------------

        private void btnStop_Click(
            object sender, 
            EventArgs e
            )
        {
            timer1.Stop();
            ChangedMode(true);
        }

        //-------------------------------------------------------

        private void btnSearch_Click(
            object sender, 
            EventArgs e
            )
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(txtUri.Text);
        }

        //-------------------------------------------------------

        void timer1_Tick(
            object sender, 
            EventArgs e
            )
        {
            webBrowser1.Navigate(txtUri.Text);
            Count++;

            Process[] procs = Process.GetProcessesByName("CDViewer.exe");
            if (procs.Length < 1)
            {
                /// 동작중이 아닐 경우
                //ProcessStartInfo psi = new ProcessStartInfo();
                //psi.UseShellExecute = true;
                //psi.FileName = @"C:\Program Files (x86)\Citrix\ICA Client\CDViewer.exe";
                //psi.WorkingDirectory = Environment.CurrentDirectory;
                //psi.Verb = "runas";
                //psi.Arguments = "";
                //Process.Start(psi);
            }
        }

        //-------------------------------------------------------

        void timer2_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            lblCnt.Text = string.Format("Count: {0}",  Count);
        }

        //-------------------------------------------------------

        void ChangedMode(
            bool state
            )
        {
            this.btnStart.Enabled = state;
            this.btnStop.Enabled = !state;
        }

        private void webBrowser1_DocumentCompleted(
            object sender, 
            WebBrowserDocumentCompletedEventArgs e
            )
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
    }
}
