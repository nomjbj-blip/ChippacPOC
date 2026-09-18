using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Controls;
using System.Drawing.Imaging;
using AForge.Video.DirectShow;
using System.Threading;

namespace DACrux.Common.VideoCapture
{
    // Video Capture Popup 2019.07.21 Taihi,Kim.
    public partial class FrmSnapshotPopup : Form
    {
        #region 멤버 변수

        private Bitmap _image;
        private string _imageFileName;
        private ImageFormat _imageFormat;

        private static FrmSnapshotPopup _form;
        
        #endregion

        #region 생성자

        private FrmSnapshotPopup()
        {
            InitializeComponent();
        }
        
        #endregion

        #region 사용자 정의 함수

        public void ImageCapture(VideoCaptureDevice videoDevice, Size size, string imageFileName, ImageFormat imageFormat)
        {
            ClientSize = size;

            _imageFileName = imageFileName;
            _imageFormat = imageFormat;

            videoSourcePlayer.NewFrame += videoSourcePlayer_NewFrame;
            videoSourcePlayer.VideoSource = videoDevice;
            videoSourcePlayer.Start();

            Application.DoEvents();
        }

        private void SaveImage()
        {
            if (videoSourcePlayer.IsDisposed || _image == null)
                return;

            try
            {
                videoSourcePlayer.NewFrame -= videoSourcePlayer_NewFrame;

                _image.Save(_imageFileName, _imageFormat);

                if (_image != null)
                {
                    _image.Dispose();
                    _image = null;
                }

                videoSourcePlayer.VideoSource = null;
            }
            catch { }
        }

        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            if (videoSourcePlayer.InvokeRequired)
            {
                _image = image;

                videoSourcePlayer.BeginInvoke(new Action(SaveImage));
            }
        }
        
        #endregion

        #region 프로퍼티

        public static FrmSnapshotPopup Instance
        {
            get
            {
                if (_form == null)
                    _form = new FrmSnapshotPopup();

                return _form;
            }
        } 

        #endregion
    }
}
