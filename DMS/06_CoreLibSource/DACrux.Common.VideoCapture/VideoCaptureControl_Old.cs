using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace DACrux.Common.VideoCapture
{
    // Video Capture Control 2019.07.21 Taihi,Kim.
    internal partial class VideoCaptureControl_Old : UserControl
    {
        #region 멤버 변수

        private string _imageFileName;
        private bool _connected;

        #endregion

        #region 생성자 및 Load 이벤트

        public VideoCaptureControl_Old()
        {
            InitializeComponent();
        }

        private void VideoCaptureControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            UpdateInformation();

            picCross.BackColor = Color.Transparent;
            picCross.Parent = videoSourcePlayer;
        }
        
        #endregion

        #region 사용자 정의 메서드

        /// <summary>
        /// 카메라 정보를 업데이트 합니다.
        /// </summary>
        private void UpdateInformation()
        {
            lblDisplayResolution.Text = lblImageResolution.Text = lblModel.Text = String.Empty;
            lblModel.Text = FrmVideoCaptureOption.Instance.GetModelName();
            lblDisplayResolution.Text = FrmVideoCaptureOption.Instance.GetDisplayResolutionName();
            lblImageResolution.Text = FrmVideoCaptureOption.Instance.GetImageResolutionName();
            lblImageFormat.Text = FrmVideoCaptureOption.Instance.GetImageFormatName();
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

            FrmVideoCaptureOption.Instance.VideoDevice.SnapshotFrame += VideoDevice_SnapshotFrame;
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

            FrmVideoCaptureOption.Instance.VideoDevice.SnapshotFrame -= VideoDevice_SnapshotFrame;
            _connected = false;
        }
        
        /// <summary>
        /// 비디오 장치에서 이미지를 캡처 합니다.
        /// </summary>
        public void ImageCapture(string imageFileName)
        {
            if (FrmVideoCaptureOption.Instance.VideoDevice == null)
                return;

            _imageFileName = imageFileName;

            if (!FrmVideoCaptureOption.Instance.SnapshotUsingDisplayControl && FrmVideoCaptureOption.Instance.VideoDevice.ProvideSnapshots)
            {
                FrmVideoCaptureOption.Instance.VideoDevice.SimulateTrigger();
            }
            else
            {
                System.IO.FileInfo oFile = new System.IO.FileInfo(imageFileName);

                while (!oFile.Exists)
                {
                    // 스냅샷을 지원하지 않는 경우 팝업창을 이용해서 이미지를 캡처합니다.
                    FrmSnapshotPopup.Instance.ImageCapture(
                        FrmVideoCaptureOption.Instance.VideoDevice,
                        FrmVideoCaptureOption.Instance.GetDisplayResolutionSize(),
                        imageFileName,
                        FrmVideoCaptureOption.Instance.ImageFormat);

                    System.Threading.Thread.Sleep(10);
                    oFile = new System.IO.FileInfo(imageFileName);
                }
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
        
        #endregion

        #region 이벤트 처리 메서드

        private void VideoDevice_SnapshotFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            e.Frame.Save(_imageFileName, FrmVideoCaptureOption.Instance.ImageFormat);
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

        /// <summary>
        /// 화면에 디스플레이되는 이미지를 이용하여 스냅샷을 저장합니다.
        /// </summary>
        [DefaultValue(true)]
        public bool SnapshotUsingDisplayControl
        {
            get { return FrmVideoCaptureOption.Instance.SnapshotUsingDisplayControl; }
            set { FrmVideoCaptureOption.Instance.SnapshotUsingDisplayControl = value; UpdateInformation(); }
        }

        public bool ConnectState
        {
            get{ return _connected; }
        }

        #endregion

        private void crossLineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            picCross.Visible = crossLineToolStripMenuItem.Checked;
        }
    }
}
