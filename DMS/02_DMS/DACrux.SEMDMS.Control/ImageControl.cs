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

namespace DACrux.SEMDMS.Control
{
    public partial class ImageControl : UserControl
    {
        private Image m_DefectImg;
        private Image m_OriginDefectImg;
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
            Image oImage
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
            Image _img = m_DefectImg;
            if (_img == null)
                return;

            // 스크롤 틱마다 실행되므로 ImageAttributes / Graphics 를 바로 해제하고, 이전 결과 Bitmap 도 해제한다.
            // (2026-10-08 CLAUDE-036: 트랙바를 끌면 틱마다 이미지 크기만큼 GDI 객체가 쌓이던 문제)
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            Bitmap bm_dest = new Bitmap(Convert.ToInt32(_img.Width), Convert.ToInt32(_img.Height));

            using (ImageAttributes imageAttributes = new ImageAttributes())
            using (Graphics _g = Graphics.FromImage(bm_dest))
            {
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                _g.DrawImage(_img, new Rectangle(0, 0, bm_dest.Width + 1, bm_dest.Height + 1), 0, 0, bm_dest.Width + 1, bm_dest.Height + 1, GraphicsUnit.Pixel, imageAttributes);
            }

            Image oldDisplayed = DectImage.Image;
            DectImage.Image = bm_dest;
            ReleaseOwnedImage(oldDisplayed);
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

            Image oldDisplayed = DectImage.Image;
            Image oldDefect = m_DefectImg;

            Bitmap oBitMap = AdjustGamma(m_OriginDefectImg, (float)GammatrackBar.Value);
            DefectImg = oBitMap;

            // 이전 감마 / 밝기 결과는 이 컨트롤이 만든 Bitmap 이므로 해제한다. (CLAUDE-036)
            ReleaseOwnedImage(oldDefect);
            if (oldDisplayed != oldDefect)
                ReleaseOwnedImage(oldDisplayed);
        }

        //--

        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private Bitmap AdjustGamma(
            Image image, 
            float gamma
            )
        {
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
            using (ImageAttributes attributes = new ImageAttributes())
            using (Graphics gr = Graphics.FromImage(bm))
            {
                // Set the ImageAttributes object's gamma value.
                attributes.SetGamma(gamma);

                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }

        //--

        /// <summary>
        /// 밝기 / 감마 조정으로 이 컨트롤이 만든 이미지를 해제한다.
        /// 원본(m_OriginDefectImg, 호출한 쪽 소유)과 지금 표시 중인 이미지는 해제하지 않는다.
        /// 팝업이 열려 있으면 팝업이 그 이미지를 그리고 있을 수 있으므로 해제하지 않는다. (2026-10-08 CLAUDE-036)
        /// </summary>
        private void ReleaseOwnedImage(Image image)
        {
            if (image == null || m_ImagePopup != null)
                return;

            if (image == m_OriginDefectImg || image == m_DefectImg || image == DectImage.Image)
                return;

            image.Dispose();
        }

        //--

        #endregion [ Method ]

        #region [ Properties ]

        //--

        public Image DefectImg
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
