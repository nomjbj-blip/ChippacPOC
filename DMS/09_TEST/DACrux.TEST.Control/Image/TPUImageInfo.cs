using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DACrux.TEST.Control
{
    public delegate void SelectDieIndex(Point oIndex);

    public partial class TPUImageInfo : UserControl
    {
        public event SelectDieIndex OnSelectDieIndex = null;
       
        private System.Drawing.Image DefectImg;

        private string m_ImageFTPPath = string.Empty;
        private string m_ImageLocalPath = string.Empty;
        private string m_FTPIP = string.Empty;
        private string m_FTPPort = string.Empty;
        private string m_FTPID = string.Empty;
        private string m_FTPPass = string.Empty;

        public TPUImageInfo()
        {
            InitializeComponent();
        }

        public TPUImageInfo(
            string strRemoteImage,
            string strLocalImage,
            string strXindex, 
            string strYindex, 
            string strBin,
            string strWaferID,
            string strProgram,
            string strDevice,
            string strFTPIP,
            string strFTPPort,
            string strFTPID,
            string strFTPPass
            )
        {
            InitializeComponent();
            lbXindex.Text = strXindex;
            lbYindex.Text = strYindex;
            lbBin.Text = strBin;
            lbWaferID.Text = strWaferID;
            lbProgram.Text = strProgram;
            lbDevice.Text = strDevice;

            m_ImageFTPPath = strRemoteImage;
            m_ImageLocalPath = strLocalImage;
            m_FTPIP = strFTPIP;
            m_FTPPort = strFTPPort;
            m_FTPID = strFTPID;
            m_FTPPass = strFTPPass;

            System.Threading.Thread trd = new System.Threading.Thread(new System.Threading.ThreadStart(DrawImage));
            trd.Start();
        }

        /// <summary>
        /// Image 를 Download 받아 출력 한다.
        /// </summary>
        private void DrawImage()
        {
            MemoryStream ms = null;
            FileInfo oImage = null;

            DACrux.Utility.HFtpClient oFTP = null;

            try
            {
                oFTP = new DACrux.Utility.HFtpClient(m_FTPIP, DACrux.Base.Convert.intParse(m_FTPPort), m_FTPID, m_FTPPass);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");

                oFTP.Down(m_ImageFTPPath, m_ImageLocalPath);
                oImage = new FileInfo(m_ImageLocalPath);
                if (oImage.Exists)
                {
                    ms = new MemoryStream();
                    using (FileStream fs = File.OpenRead(oImage.FullName))
                    {
                        fs.CopyTo(ms);
                    }

                    DefectImg = System.Drawing.Image.FromStream(ms);

                    imageControl1.ImageControlDraw(
                        null,
                        DefectImg
                        );
                }
            }
            catch (Exception) { }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                ms = null;

                //Image File 은 사용후 삭제 한다.
                if (oImage != null && oImage.Exists)
                    oImage.Delete();
            }
        }

        private void BtnFocus_Click(object sender, EventArgs e)
        {
            if (OnSelectDieIndex != null)
            {
                OnSelectDieIndex(new Point(DACrux.Base.Convert.intParse(lbXindex.Text), DACrux.Base.Convert.intParse(lbYindex.Text)));
            }
        }
    }
}
