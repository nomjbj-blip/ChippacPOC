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

            using (System.IO.Stream stream = DefectMapDraw.fnFTPGetStream(ftpPath))
            {
                if (stream == null)
                    return;

                SetImageToControl(Image.FromStream(stream));
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
