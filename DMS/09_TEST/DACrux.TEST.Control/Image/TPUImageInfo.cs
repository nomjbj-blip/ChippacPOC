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
        // Image.FromStream 으로 만든 이미지는 원본 스트림이 살아 있어야 하므로 이미지와 함께 보관한다.
        private MemoryStream m_ImageStream;

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

            // 다운로드만 백그라운드 스레드에서 하고 화면 표시는 ShowImage 에서 UI 스레드로 넘긴다.
            // 앱 종료를 막지 않도록 IsBackground 로 둔다. (2026-10-08 CLAUDE-036)
            System.Threading.Thread trd = new System.Threading.Thread(new System.Threading.ThreadStart(DrawImage));
            trd.IsBackground = true;
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

                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                    ShowImage(image, ms);
                    ms = null; // 소유권을 ShowImage 로 넘겼다.
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("AVI Image 표시 실패 ({0}): {1}", m_ImageFTPPath, ex.Message);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                ms = null;

                //Image File 은 사용후 삭제 한다.
                try
                {
                    if (oImage != null && oImage.Exists)
                        oImage.Delete();
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        /// <summary>
        /// 다운로드 스레드에서 만든 이미지를 UI 스레드에서 표시한다.
        /// 컨트롤이 이미 해제되었으면(한 번도 표시되지 않은 이미지) 이미지 / 스트림을 바로 해제한다.
        /// 표시된 이미지는 이미지 팝업이 계속 그릴 수 있으므로 직접 해제하지 않고 GC 에 맡긴다. (2026-10-08 CLAUDE-036)
        /// </summary>
        private void ShowImage(System.Drawing.Image image, MemoryStream stream)
        {
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<System.Drawing.Image, MemoryStream>(ShowImage), image, stream);
                }
                catch (InvalidOperationException)
                {
                    // 창 핸들이 이미 사라졌다(목록이 지워지는 중).
                    image.Dispose();
                    stream.Dispose();
                }
                return;
            }

            if (IsDisposed)
            {
                image.Dispose();
                stream.Dispose();
                return;
            }

            DefectImg = image;
            m_ImageStream = stream;

            imageControl1.ImageControlDraw(
                null,
                DefectImg
                );
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
