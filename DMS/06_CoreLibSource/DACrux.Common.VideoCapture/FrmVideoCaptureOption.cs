using System;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel;

namespace DACrux.Common.VideoCapture
{
    // Video Capture Control Option 2019.07.21 Taihi,Kim.
    public partial class FrmVideoCaptureOption : Form
    {
        #region 멤버 변수

        public static readonly string VIDEO_DEVICE_NAME = "VIDEO_DEVICE";
        public static readonly string DISPLAY_RESOLUTION = "DISPLAY_RESOLUTION";
        public static readonly string IMAGE_RESOLUTION = "IMAGE_RESOLUTION";
        public static readonly string IMAGE_FORMAT = "IMAGE_FORMAT";
        public static readonly string SHOW_CROSSHAIR = "SHOW_CROSSHAIR";

        public static readonly ImageFormat[] ImageFormatArray;

        private static FrmVideoCaptureOption _form;

        private FilterInfoCollection _videoDeviceList;
        private bool _snapshotUsingDisplayControl;
        private SettingData _setting;
        
        #endregion

        #region 생성자

        static FrmVideoCaptureOption()
        {
            ImageFormatArray = new ImageFormat[]
            {
                ImageFormat.Jpeg,
                ImageFormat.Bmp,
                ImageFormat.Gif,
                ImageFormat.Png,
                ImageFormat.Tiff
            };
        }

        private FrmVideoCaptureOption()
        {
            InitializeComponent();

            SnapshotUsingDisplayControl = true;
            _setting = new SettingData();

            SetDeviceNames();
            SetImageFormat();
            SetShowCrosshair();
        }
        
        #endregion

        #region 사용자 정의 메서드

        private void SetDeviceNames()
        {
            _videoDeviceList = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (_videoDeviceList == null || _videoDeviceList.Count == 0)
                return;

            foreach (FilterInfo videoDevice in _videoDeviceList)
                cboDevices.Items.Add(videoDevice.Name);

            if (_setting.HasValue(VIDEO_DEVICE_NAME))
                cboDevices.Text = _setting.GetValue(VIDEO_DEVICE_NAME);
        }

        private void SetDisplayResolution()
        {
            cboDisplayResolution.Items.Clear();

            if (VideoDevice == null)
                return;

            foreach (VideoCapabilities capa in VideoDevice.VideoCapabilities)
                cboDisplayResolution.Items.Add(GetResolution(capa.FrameSize));

            if (_setting.HasValue(DISPLAY_RESOLUTION))
                cboDisplayResolution.Text = _setting.GetValue(DISPLAY_RESOLUTION);
        }

        private void SetImageResolution()
        {
            cboImageResolution.Items.Clear();

            if (VideoDevice == null)
                return;

            foreach (VideoCapabilities capa in VideoDevice.SnapshotCapabilities)
                cboImageResolution.Items.Add(GetResolution(capa.FrameSize));

            if (_setting.HasValue(IMAGE_RESOLUTION))
                cboImageResolution.Text = _setting.GetValue(IMAGE_RESOLUTION);
        }

        private void SetImageFormat()
        {
            foreach (ImageFormat format in ImageFormatArray)
                cboImageFormat.Items.Add(format.ToString());

            if (_setting.HasValue(IMAGE_FORMAT))
                cboImageFormat.Text = _setting.GetValue(IMAGE_FORMAT);
        }

        private void SetShowCrosshair()
        {
            chkShowCrosshair.Checked = _setting.GetValue(SHOW_CROSSHAIR, true);
        }

        private void ShowMessageAndFocus(Control messageCtl, Control focusCtl)
        {
            string message = "'{0}'을(를) 선택하세요.";
            MessageBox.Show(String.Format(message, messageCtl.Text.Substring(0, messageCtl.Text.Length - 2)));
            focusCtl.Focus();
        }

        private void UpdateControl()
        {
            lblImageResolution.Enabled = cboImageResolution.Enabled = !SnapshotUsingDisplayControl;
        }

        public string GetModelName()
        {
            if (cboDevices.SelectedIndex < 0)
                return null;
            else
                return _videoDeviceList[cboDevices.SelectedIndex].Name;
        }

        public string GetDisplayResolutionName()
        {
            if (VideoDevice == null || cboDisplayResolution.SelectedIndex < 0)
                return null;
            else
                return GetResolution(VideoDevice.VideoCapabilities[cboDisplayResolution.SelectedIndex].FrameSize);
        }

        public string GetImageResolutionName()
        {
            if (VideoDevice == null || cboImageResolution.SelectedIndex < 0 || !VideoDevice.ProvideSnapshots || SnapshotUsingDisplayControl)
                return null;
            else
                return GetResolution(VideoDevice.SnapshotCapabilities[cboImageResolution.SelectedIndex].FrameSize);
        }

        public string GetImageFormatName()
        {
            if (cboImageFormat.SelectedIndex < 0)
                return null;
            else
                return ImageFormatArray[cboImageFormat.SelectedIndex].ToString();
        }

        public bool ShowCrosshair()
        {
            return chkShowCrosshair.Checked;
        }

        public static string GetResolution(Size size)
        {
            return String.Format("{0}x{1}", size.Width, size.Height);
        }

        public Size GetDisplayResolutionSize()
        {
            if (VideoDevice == null || cboDisplayResolution.SelectedIndex < 0)
                return Size.Empty;
            else
                return VideoDevice.VideoCapabilities[cboDisplayResolution.SelectedIndex].FrameSize;
        } 
        #endregion

        #region 이벤트 처리 메서드

        private void cboDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoDevice = new VideoCaptureDevice(_videoDeviceList[cboDevices.SelectedIndex].MonikerString);
            SetDisplayResolution();
            SetImageResolution();
        }

        private void cboDisplayResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VideoDevice == null)
                return;

            if (cboDisplayResolution.SelectedIndex >= 0)
                VideoDevice.VideoResolution = VideoDevice.VideoCapabilities[cboDisplayResolution.SelectedIndex];

            //VideoDevice.VideoResolution = VideoDevice.VideoCapabilities[cboDisplayResolution.SelectedIndex];
            //VideoDevice.DesiredFrameSize = VideoDevice.VideoCapabilities[cboDisplayResolution.SelectedIndex].FrameSize;
        }

        private void cboImageResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VideoDevice == null)
                return;

            if (cboImageResolution.SelectedIndex >= 0)
            {
                VideoDevice.DesiredSnapshotSize = VideoDevice.SnapshotCapabilities[cboImageResolution.SelectedIndex].FrameSize;
                VideoDevice.ProvideSnapshots = true;
            }
            else
            {
                VideoDevice.ProvideSnapshots = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboDevices.SelectedIndex < 0)
            {
                ShowMessageAndFocus(lblDeviceName, cboDevices);
                return;
            }

            if (cboDisplayResolution.SelectedIndex < 0)
            {
                ShowMessageAndFocus(lblDisplayResolution, cboDisplayResolution);
                return;
            }

            if (!SnapshotUsingDisplayControl && cboImageResolution.Items.Count > 0 && cboImageResolution.SelectedIndex < 0)
            {
                ShowMessageAndFocus(lblImageResolution, cboImageResolution);
                return;
            }

            if (cboImageFormat.SelectedIndex < 0)
            {
                ShowMessageAndFocus(lblImageFormat, cboImageFormat);
                return;
            }

            if (MessageBox.Show("저장하시겠습니까?", "저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var setting = new SettingData();

                setting.SetValue(VIDEO_DEVICE_NAME, cboDevices.Text);
                setting.SetValue(DISPLAY_RESOLUTION, cboDisplayResolution.Text);
                setting.SetValue(IMAGE_FORMAT, cboImageFormat.Text);

                if (!SnapshotUsingDisplayControl)
                    setting.SetValue(IMAGE_RESOLUTION, cboImageResolution.Text);

                setting.SetValue(SHOW_CROSSHAIR, chkShowCrosshair.Checked);

                setting.Save();

                DialogResult = DialogResult.OK;
            }
        }
        
        #endregion

        #region 프로퍼티

        public static FrmVideoCaptureOption Instance
        {
            get
            {
                if (_form == null)
                    _form = new FrmVideoCaptureOption();

                return _form;
            }
        }

        public VideoCaptureDevice VideoDevice
        {
            get;
            private set;
        }

        public ImageFormat ImageFormat
        {
            get
            {
                if (cboImageFormat.SelectedIndex < 0)
                    return ImageFormat.Jpeg;
                else
                    return ImageFormatArray[cboImageFormat.SelectedIndex];
            }
        }

        /// <summary>
        /// 화면에 디스플레이되는 이미지를 이용하여 스냅샷을 저장합니다.
        /// </summary>
        public bool SnapshotUsingDisplayControl
        {
            get { return _snapshotUsingDisplayControl; }
            set { _snapshotUsingDisplayControl = value; UpdateControl(); }
        }

        #endregion
    }
}
