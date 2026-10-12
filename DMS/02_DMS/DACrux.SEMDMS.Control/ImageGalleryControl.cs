using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.SEMDMS.Control
{
    public partial class ImageGalleryControl : UserControl
    {
        #region 멤버변수
        
        public static readonly int MARGIN = 1;
        public static readonly StringFormat TEXT_FORMAT;

        public event EventHandler ImagedLoadComplete;
        private string m_description;
        
        #endregion

        #region 생성자

        static ImageGalleryControl()
        {
            TEXT_FORMAT = new StringFormat();
            TEXT_FORMAT.FormatFlags = StringFormatFlags.NoWrap;
        }

        public ImageGalleryControl()
        {
            InitializeComponent();
        }
        
        #endregion

        #region 사용자 정의 메서드

        protected virtual void OnImagedLoadComplete(EventArgs e)
        {
            if (ImagedLoadComplete != null)
                ImagedLoadComplete(this, e);
        }

        public void SetImage(string ftpPath)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SetImageAsync));
            t.IsBackground = true;
            t.Start(ftpPath);
        }

        private void SetImageAsync(object arg)
        {
            string ftpPath = arg as string;

            // 이미지마다 만드는 작업 스레드이다. 처리되지 않은 예외(FTP 설정 없음 / 손상된 이미지 / 창 핸들 없음)는
            // 클라이언트 프로세스를 종료시키므로 여기서 잡는다. (2026-10-08)
            try
            {
                // Image.FromStream 은 원본 스트림이 살아 있어야 하므로 FTP 스트림을 메모리로 복사해 그 사본으로 만든다.
                System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
                using (System.IO.Stream stream = DefectMapDraw.fnFTPGetStream(ftpPath))
                {
                    if (stream == null)
                        return;

                    stream.CopyTo(imageStream);
                }
                imageStream.Position = 0;

                SetImageToControl(Image.FromStream(imageStream));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Defect Image 표시 실패 ({0}): {1}", ftpPath, ex.Message);
            }
        }

        private void SetImageToControl(Image image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Image>(SetImageToControl), image);
                return;
            }

            pictureBox1.Image = image;

            if (OwnerCell != null && OwnerCell.CellType is FarPoint.Win.Spread.CellType.GeneralCellType)
            {
                Bitmap bmp = null;

                if (image != null)
                {
                    bmp = new Bitmap(Width, Height);
                    DrawToBitmap(bmp, ClientRectangle);
                }

                (OwnerCell.CellType as FarPoint.Win.Spread.CellType.GeneralCellType).BackgroundImage.Image = bmp;
                OwnerCell.Invalidate();
            }

            // raise event
            OnImagedLoadComplete(EventArgs.Empty);
        }

        public void UpdateTextArea()
        {
            int line = String.IsNullOrEmpty(m_description) ? 0 : m_description.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
            panel1.Height = panel1.Font.Height * line + MARGIN;
        }
        
        #endregion

        #region 이벤트 처리 메서드

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(panel1.ForeColor))
                e.Graphics.DrawString(m_description, panel1.Font, brush, 0, MARGIN, TEXT_FORMAT);
        }
        
        #endregion

        #region 프로퍼티

        public string Description
        {
            get { return m_description; }
            set { m_description = value; UpdateTextArea(); }
        }

        public Image DefectImage
        {
            get { return pictureBox1.Image; }
        }

        public Defect Defect
        {
            get;
            set;
        }

        public int ImageID
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public FarPoint.Win.Spread.Cell OwnerCell
        {
            get;
            set;
        }

        #endregion
    }
}
