using AForge.Controls;
using DACrux.Common.VideoCapture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VideoCaptureTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            FrmVideoCaptureOption.Instance.VideoDevice.NewFrame += VideoDevice_NewFrame;
            videoSourcePlayer.VideoSource = FrmVideoCaptureOption.Instance.VideoDevice;
            videoSourcePlayer.Start();
        }

        private void VideoDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            var bmp = e.Frame.Clone() as Bitmap;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    var old = pictureBox1.Image;
                    pictureBox1.Image = bmp;
                    old?.Dispose();
                }));                
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int w = 100;
            int h = 100;
            int cx = pictureBox1.Width / 2;
            int cy = pictureBox1.Height / 2;

            using (Pen pen = new Pen(Color.Red, 1))
            {
                e.Graphics.DrawLine(pen, cx - w, cy, cx + w, cy);
                e.Graphics.DrawLine(pen, cx, cy - h, cx, cy + h);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }

        private void btnCapture_Click(object sender, EventArgs e)
        {

        }
    }
}
