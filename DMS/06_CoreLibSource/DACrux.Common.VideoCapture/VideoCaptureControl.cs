using AForge.Controls;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Common.VideoCapture
{
    // Video Capture Control 2019.07.21 Taihi,Kim.
    public partial class VideoCaptureControl : UserControl
    {
        #region 멤버 변수

        public const int CROSSHAIR_SIZE = 20;
        public const string CROSSHAIR_COLOR = "Red";

        private string _imageFileName;
        private bool _connected;
        private bool _isStretchImage;
        private bool _showCrosshair;
        private Color _crosshairColor;
        private Pen _crosshairPen;
        private Action<Bitmap> _delUpdateImage;

        #endregion

        #region 생성자 및 Load 이벤트

        public VideoCaptureControl()
        {
            InitializeComponent();

            _delUpdateImage = new Action<Bitmap>(UpdateImageAsync);
            _isStretchImage = false;

            CrosshairSize = CROSSHAIR_SIZE;
            CrosshairColor = Color.FromName(CROSSHAIR_COLOR);
        }

        private void VideoCaptureControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            UpdateInformation();
            SetSizeMode();
        }
        
        #endregion

        #region 사용자 정의 메서드

        /// <summary>
        /// 카메라 정보를 업데이트 합니다.
        /// </summary>
        private void UpdateInformation()
        {
            if (!statusStrip1.Visible)
                return;

            lblDisplayResolution.Text = lblImageResolution.Text = lblModel.Text = String.Empty;
            lblModel.Text = FrmVideoCaptureOption.Instance.GetModelName();
            lblDisplayResolution.Text = FrmVideoCaptureOption.Instance.GetDisplayResolutionName();
            lblImageResolution.Text = FrmVideoCaptureOption.Instance.GetImageResolutionName();
            lblImageFormat.Text = FrmVideoCaptureOption.Instance.GetImageFormatName();
            _showCrosshair = FrmVideoCaptureOption.Instance.ShowCrosshair();
        }

        /// <summary>
        /// 옵션 창을 보여 줍니다.
        /// </summary>
        public void ShowOption()
        {
            Size prevSize = FrmVideoCaptureOption.Instance.GetDisplayResolutionSize();

            if (FrmVideoCaptureOption.Instance.ShowDialog() == DialogResult.OK)
            {
                UpdateInformation();

                // 그전에 연결 상태였으면 다시 재연결
                if (_connected && prevSize != FrmVideoCaptureOption.Instance.GetDisplayResolutionSize())
                {
                    Disconnect();
                    Application.DoEvents();
                    Connect();
                }
            }
        }

        /// <summary>
        /// 비디오 장치에 연결합니다.
        /// </summary>
        public void Connect()
        {
            if (FrmVideoCaptureOption.Instance.VideoDevice == null)
            {
                ShowOption();
                return;
            }

            FrmVideoCaptureOption.Instance.VideoDevice.NewFrame += VideoDevice_NewFrame;
            videoSourcePlayer.VideoSource = FrmVideoCaptureOption.Instance.VideoDevice;
            videoSourcePlayer.Start();

            _connected = true;
        }

        /// <summary>
        /// 비디오 장치 연결을 끊습니다.
        /// </summary>
        public void Disconnect()
        {
            if (FrmVideoCaptureOption.Instance.VideoDevice == null)
                return;

            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
            videoSourcePlayer.VideoSource = null;

            FrmVideoCaptureOption.Instance.VideoDevice.NewFrame -= VideoDevice_NewFrame;

            picCross.Image = null;
            _connected = false;
        }
        
        /// <summary>
        /// 비디오 장치에서 이미지를 캡처 합니다.
        /// </summary>
        public void ImageCapture(string imageFileName)
        {
            if (videoSourcePlayer != null && picCross.Image != null)
            {
                picCross.Image.Save(imageFileName);
            }
        }

        /// <summary>
        /// 이미지에 대한 확장자를 가져옵니다. <para />
        /// 예) ImageCodeInfo 의 "*.JPG;*.JPEG;*.JPE;*.JFIF" 데이터를 ".jpg" 로 가져옵니다.
        /// </summary>
        public string GetImageExtension()
        {
            if (FrmVideoCaptureOption.Instance.ImageFormat == null)
                return null;

            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.FormatID == FrmVideoCaptureOption.Instance.ImageFormat.Guid)
                    return codec.FilenameExtension.Split(';')[0].Substring(1).ToLower();
            }

            return null;
        }

        private void SetSizeMode()
        {
            picCross.SizeMode = _isStretchImage ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Zoom;
        }

        private void UpdateImageAsync(Bitmap bmp)
        {
            if (picCross.InvokeRequired)
            {
                picCross.BeginInvoke(_delUpdateImage, bmp);
                return;
            }

            var old = picCross.Image;
            picCross.Image = bmp;
            old?.Dispose();
        }

        private void UpdateCrosshairPen()
        {
            _crosshairPen = new Pen(_crosshairColor, 1);
        }

        #endregion

        #region 이벤트 처리 메서드

        private void VideoDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            UpdateImageAsync(e.Frame);
        }

        private void crossLineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            _showCrosshair = crossLineToolStripMenuItem.Checked;
            picCross.Invalidate();
        }

        private void picCross_Paint(object sender, PaintEventArgs e)
        {
            if (!_showCrosshair)
                return;

            int cx = picCross.Width / 2;
            int cy = picCross.Height / 2;
            
            e.Graphics.DrawLine(_crosshairPen, cx - CrosshairSize, cy, cx + CrosshairSize, cy);
            e.Graphics.DrawLine(_crosshairPen, cx, cy - CrosshairSize, cx, cy + CrosshairSize);
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            ShowOption();
        }

#endregion

        #region 프로퍼티

        protected override Size DefaultSize
        {
            get { return new Size(320, 240 + 22/*statusStrip1.Height*/); }
        }

        public bool ConnectState
        {
            get{ return _connected; }
        }

        /// <summary>
        /// 십자 마크 크기를 나타냅니다.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(CROSSHAIR_SIZE)]
        [Description("십자 마크 크기를 나타냅니다.")]
        public int CrosshairSize
        {
            get;
            set;
        }

        /// <summary>
        /// 십자 마크 색상을 나타냅니다.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(typeof(Color), CROSSHAIR_COLOR)]
        [Description("십자 마크 색상을 나타냅니다.")]
        public Color CrosshairColor
        {
            get {  return _crosshairColor; }
            set { _crosshairColor = value; UpdateCrosshairPen(); }
        }

        [DefaultValue(true)]
        public bool ShowStatusBar
        {
            get { return statusStrip1.Visible; }
            set { statusStrip1.Visible = value; }
        }

        [DefaultValue(false)]
        public bool IsStretchImage
        {
            get { return _isStretchImage; }
            set { _isStretchImage = value; SetSizeMode(); }
        }

        #endregion
    }
}
