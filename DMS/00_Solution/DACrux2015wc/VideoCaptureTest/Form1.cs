using DACrux.Common.VideoCapture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoCaptureTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            videoCaptureControl1.Connect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            videoCaptureControl1.Disconnect();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            using (var frm = new SaveFileDialog())
            {
                var ext = videoCaptureControl1.GetImageExtension();
                frm.Filter = $"Image File(*{ext})|*{ext}";
                frm.FileName = DateTime.Now.ToString("yyyyyMMddHHmmss");

                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                videoCaptureControl1.ImageCapture(frm.FileName);
                Process.Start(frm.FileName);
            }
        }

        private void chkStretch_CheckedChanged(object sender, EventArgs e)
        {
            videoCaptureControl1.IsStretchImage = chkStretch.Checked;
        }
    }
}
