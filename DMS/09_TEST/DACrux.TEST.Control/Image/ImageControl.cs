using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DACrux.TEST.Control
{
    public partial class ImageControl : UserControl
    {
        private System.Drawing.Image m_DefectImg;
        private System.Drawing.Image m_OriginDefectImg;
        private ImagePopUp m_ImagePopup = null;

        //--

        public ImageControl(
            )
        {
            InitializeComponent();
        }

        //--

        public void ImageControlDraw(
            ImagePopUp imagePopUp,
            System.Drawing.Image oImage
            )
        {
            m_ImagePopup = imagePopUp;
            DefectImg = oImage;
            m_OriginDefectImg = m_DefectImg;
            DectImage.Image = m_DefectImg;
            Application.DoEvents();
        }

        #region [ Event Handler ]

        //--

        private void BrighttrackBar_Scroll(
            object sender, 
            EventArgs e
            )
        {
            float value = BrighttrackBar.Value * 0.01f;
            float[][] colorMatrixElements = {
	                    new float[] {
		                    1,
		                    0,
		                    0,
		                    0,
		                    0
	                    },
	                    new float[] {
		                    0,
		                    1,
		                    0,
		                    0,
		                    0
	                    },
	                    new float[] {
		                    0,
		                    0,
		                    1,
		                    0,
		                    0
	                    },
	                    new float[] {
		                    0,
		                    0,
		                    0,
		                    1,
		                    0
	                    },
	                    new float[] {
		                    value,
		                    value,
		                    value,
		                    0,
		                    1
	                    }
                    };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            ImageAttributes imageAttributes = new ImageAttributes();

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            System.Drawing.Image _img = m_DefectImg;
            //PictureBox1.Image
            Graphics _g = default(Graphics);
            Bitmap bm_dest = new Bitmap(Convert.ToInt32(_img.Width), Convert.ToInt32(_img.Height));
            _g = Graphics.FromImage(bm_dest);
            _g.DrawImage(_img, new Rectangle(0, 0, bm_dest.Width + 1, bm_dest.Height + 1), 0, 0, bm_dest.Width + 1, bm_dest.Height + 1, GraphicsUnit.Pixel, imageAttributes);
            DectImage.Image = bm_dest;
        }

        //--

        private void DectImage_MouseDoubleClick(
            object sender, 
            MouseEventArgs e
            )
        {
            if (m_ImagePopup == null)
            {
                m_ImagePopup = new ImagePopUp(DefectImg);
                m_ImagePopup.StartPosition = FormStartPosition.CenterParent;
                m_ImagePopup.Show(this);
            }
            else
            {
                m_ImagePopup.Close();
                m_ImagePopup.Dispose();
                m_ImagePopup = null;
            }
        }

        //--

        private void GammatrackBar_Scroll(
            object sender, 
            EventArgs e
            )
        {
            if (m_OriginDefectImg == null)
                return;

            Bitmap oBitMap = AdjustGamma(m_OriginDefectImg, (float)GammatrackBar.Value);
            DefectImg = oBitMap;
        }

        //--

        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private Bitmap AdjustGamma(
            System.Drawing.Image image, 
            float gamma
            )
        {
            // Set the ImageAttributes object's gamma value.
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetGamma(gamma);

            // Draw the image onto the new bitmap
            // while applying the new gamma value.
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };

            Rectangle rect =
                new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }

        //--

        public void ImageClear()
        {
            DectImage.Image = null;
            DectImage.Refresh();
        }

        #endregion [ Method ]

        #region [ Properties ]

        //--

        public System.Drawing.Image DefectImg
        {
            get
            {
                return m_DefectImg;
            }
            set
            {
                m_DefectImg = value;
                DectImage.Image = m_DefectImg;
            }
        }

        //--

        #endregion [ Properties ]
    }
}
