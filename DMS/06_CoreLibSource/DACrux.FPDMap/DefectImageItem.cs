using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using System.Net;
using System.IO;

namespace DACrux.FPDMap
{
    /// <summary>
    /// Defect 정보와 Defect Image를 표현하는 클래스
    /// </summary>
    public partial class DefectImageItem : UserControl
    {
        #region 멤버 변수

        private static Dictionary<string, Image> ImageCache;
        private static StringFormat TextStringFormat;

        public static int LABEL_HEIGHT = 20;
        private const int MARGIN = 3;

        private const string IMAGE_LOADING = "Loading...";
        private const string IMAGE_FAILED = "Loading failed";

        private Rectangle _textRect;
        private Rectangle _imageRect;
        private string _imageInfo;

        ImageDown _down = new ImageDown();

        #endregion

        #region 생성자

        static DefectImageItem()
        {
            ImageCache = new Dictionary<string, Image>();

            TextStringFormat = new StringFormat();
            TextStringFormat.Alignment = StringAlignment.Center;
            TextStringFormat.LineAlignment = StringAlignment.Center;
        }

        public DefectImageItem()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            _down.DownloadComplete += Image_DownloadCompleted;
        }

        public DefectImageItem(Defect defect)
            : this()
        {
            Defect = defect;
        }

        #endregion

        #region 메서드

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResetLayout();

            if (Defect == null)
                return;

            if (!String.IsNullOrEmpty(Defect.IMAGEURL))
            {
                if (ImageCache.ContainsKey(Defect.IMAGEURL))
                {
                    Image = ImageCache[Defect.IMAGEURL];
                }
                else
                {
                    _down.DownloadAsync(Defect.IMAGEURL, this);
                    _imageInfo = IMAGE_LOADING;
                }

                Invalidate();
            }
        }

        private void Image_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ImageCache[Defect.IMAGEURL] = _down.Image;
                Image = _down.Image;
            }
            else
            {
                _imageInfo = IMAGE_FAILED;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 테두리용 Rectangle
            Rectangle clientDrawRect = ClientRectangle;
            clientDrawRect.Inflate(-2, -2);

            // 테두리
            g.DrawRectangle(Pens.LightGray, clientDrawRect);

            if (Image != null)
            {
                // 이미지가 있는 경우
                g.DrawImage(Image, GetFittedRectangle(_imageRect, Image));
            }
            else
            {
                g.DrawString(_imageInfo, Font, Brushes.Blue, _imageRect, TextStringFormat);
            }

            // Defect 정보
            g.DrawString(Defect.ToString(), Font, Brushes.Black, _textRect, TextStringFormat);

            // 포커스 영역
            if (Focused)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(32, Color.Blue)), ClientRectangle);
                g.DrawRectangle(Pens.Blue, clientDrawRect);
                //ControlPaint.DrawFocusRectangle(g, ClientRectangle);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ResetLayout();
        }

        private Rectangle GetFittedRectangle(Rectangle rect, Image image)
        {
            double r1 = rect.Width / (double)image.Width;
            double r2 = rect.Height / (double)image.Height;
            int x, y, width, height;

            double ratio = (r1 < r2) ? r1 : r2;

            width = (int)(image.Width * ratio);
            height = (int)(image.Height * ratio);
            x = rect.X + (int)(0.5 * (rect.Width - width));
            y = rect.Y + (int)(0.5 * (rect.Height - height));
            
            return new Rectangle(x, y, width, height);
        }
        
        private void ResetLayout()
        {
            // Layout 재정렬
            _imageRect = new Rectangle(MARGIN, MARGIN, Width - 2 * MARGIN, Height - 3 * MARGIN - LABEL_HEIGHT);
            _textRect = new Rectangle(MARGIN, Height - MARGIN - LABEL_HEIGHT, Width - 2 * MARGIN, LABEL_HEIGHT);

            Invalidate();
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// Defect 을 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public Defect Defect
        {
            get;
            private set;
        }

        /// <summary>
        /// 해당 Defect에 대한 Image 정보를 가져옵니다.
        /// </summary>
        public Image Image
        {
            get;
            private set;
        }

        #endregion
    }

    class ImageDown
    {
        public event AsyncCompletedEventHandler DownloadComplete;

        public ImageDown()
        {
        }

        protected virtual void OnDownloadComplete(AsyncCompletedEventArgs e)
        {
            if (Owner == null || !Owner.InvokeRequired)
            {
                if (DownloadComplete != null)
                    DownloadComplete(this, e);
            }
            else
            {
                Owner.BeginInvoke(new Action<AsyncCompletedEventArgs>(OnDownloadComplete), e);
            }
        }

        public void Download(string ftpPath)
        {
            if (IsDownloading)
                return;

            DownloadInternal(ftpPath);
        }

        public void DownloadAsync(string ftpPath, Control owner)
        {
            Owner = owner;

            if (IsDownloading)
                return;

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadInternal, ftpPath);
        }

        private void DownloadInternal(object ftp)
        {
            try
            {
                IsDownloading = true;

                string ftpPath = ftp as string;
                WebRequest req = WebRequest.Create(ftpPath);

                if (req is HttpWebRequest)
                    req.Method = WebRequestMethods.Http.Get;
                else if (req is FtpWebRequest)
                    req.Method = WebRequestMethods.Ftp.DownloadFile;
                else if (req is FileWebRequest)
                    req.Method = WebRequestMethods.File.DownloadFile;

                if (!String.IsNullOrEmpty(ID))
                    req.Credentials = new NetworkCredential(ID, Password);

                WebResponse rep = req.GetResponse();

                using (Stream stream = rep.GetResponseStream())
                {
                    Image = Image.FromStream(stream);
                }

                OnDownloadComplete(new AsyncCompletedEventArgs(null, false, null));
            }
            catch (Exception ex)
            {
                OnDownloadComplete(new AsyncCompletedEventArgs(ex, false, null));
            }
            finally
            {
                IsDownloading = false;
            }
        }

        public string ID { get; set; }
        public string Password { get; set; }
        public bool IsDownloading { get; private set; }
        public Image Image { get; private set; }
        public Control Owner { get; private set; }
    }
}
