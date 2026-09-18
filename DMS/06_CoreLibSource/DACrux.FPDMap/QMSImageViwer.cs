using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DACrux.DMSVFPD.Map
{
    public struct ImageSelectTag
    {
        public Bitmap BMPImage;
        public string ImageID;
        public string ImagePath;
    }

    public partial class QMSImageViwer : UserControl
    {

        private ArrayList m_arrImages = null;
        private Bitmap[] m_bmpTempVisible = null;

        /// <summary>
        /// 전체
        /// </summary>
        private Graphics m_gdiMain = null;
        private Graphics m_gdiTempMain = null;
        private Bitmap m_bmpMain = null;
        private Bitmap m_bmpBack = null;

        private int m_iCenterControl = 0;
		int m_arrImageIdx = 0;


        private int m_iMouseStart = 0;

        public void AddImage(ImageSelectTag oImage)
        {
            if (m_arrImages == null)
            {
                m_arrImages = new ArrayList();
            }

            m_arrImages.Add(oImage);
        }

        public object GetImage()
        {
            return m_arrImages;
        }

        public void ResetImage()
        {
            try
            {
                if (m_arrImages != null) m_arrImages = null;
                m_arrImages = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Indexing()
        {
            try
            {
                for (int i = 0; i < m_arrImages.Count; i++)
                {
                    if (i >= m_bmpTempVisible.Length) break;
                    m_bmpTempVisible[i] = (Bitmap)((ImageSelectTag)m_arrImages[i]).BMPImage.Clone();
                }

                m_arrImageIdx = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public QMSImageViwer()
        {
            //Bitmap[] tmpVisible = new Bitmap[8];
			m_bmpTempVisible = new Bitmap[7];
			for (int a = 0; a < m_bmpTempVisible.Length; a++) m_bmpTempVisible[a] = new Bitmap(200, 200);

            if (m_arrImages == null)
            {
                m_arrImages = new ArrayList();
            }

            //Graphics[] oGdis = new Graphics[7];

            //tmpVisible[0] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7955.JPG");
            //tmpVisible[1] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7959.JPG");
            //tmpVisible[2] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7986.JPG");
            //tmpVisible[3] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7987.JPG");
            //m_bmpVisible[4] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7988.JPG");
            //m_bmpVisible[5] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_7991.JPG");
            //m_bmpVisible[6] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_8167.JPG");
            //m_bmpVisible[7] = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\DSC_8273.JPG");
			//m_bmpBack = new Bitmap(@"D:\02_프로젝트\00_COC프로젝트\04_개발단계\0404_QMS_SOURCE\97_Image\ImageViwerSample\ImgSelectV2.jpg");

            //ImageSelectTag[] oImgStag = new ImageSelectTag[8];
            //oImgStag[0].BMPImage = new Bitmap(tmpVisible[0], 200, (int)(tmpVisible[0].Height * (200f / (float)tmpVisible[0].Width)));
            //oImgStag[1].BMPImage = new Bitmap(tmpVisible[1], 200, (int)(tmpVisible[1].Height * (200f / (float)tmpVisible[1].Width)));
            //oImgStag[2].BMPImage = new Bitmap(tmpVisible[2], 200, (int)(tmpVisible[2].Height * (200f / (float)tmpVisible[2].Width)));
            //oImgStag[3].BMPImage = new Bitmap(tmpVisible[3], 200, (int)(tmpVisible[3].Height * (200f / (float)tmpVisible[3].Width))); 
            //oImgStag[4].BMPImage = m_bmpVisible[4];
            //oImgStag[5].BMPImage = m_bmpVisible[5];
            //oImgStag[6].BMPImage = m_bmpVisible[6];
            //oImgStag[7].BMPImage = m_bmpVisible[7];

            //AddImage(oImgStag[0]);
            //AddImage(oImgStag[1]);
            //AddImage(oImgStag[2]);
            //AddImage(oImgStag[3]);
            //Indexing();
            //AddImage(oImgStag[4]);
            //AddImage(oImgStag[5]);
            //AddImage(oImgStag[6]);
            //AddImage(oImgStag[7]);


            //m_bmpTempVisible[0] = new Bitmap(m_bmpVisible[0], 200, (int)(m_bmpVisible[0].Height * (200f / (float)m_bmpVisible[0].Width)));
            //m_bmpTempVisible[1] = new Bitmap(m_bmpVisible[1], 200, (int)(m_bmpVisible[1].Height * (200f / (float)m_bmpVisible[1].Width)));
            //m_bmpTempVisible[2] = new Bitmap(m_bmpVisible[2], 200, (int)(m_bmpVisible[2].Height * (200f / (float)m_bmpVisible[2].Width)));
            //m_bmpTempVisible[3] = new Bitmap(m_bmpVisible[3], 200, (int)(m_bmpVisible[3].Height * (200f / (float)m_bmpVisible[3].Width)));
            //m_bmpTempVisible[4] = new Bitmap(m_bmpVisible[4], 200, (int)(m_bmpVisible[4].Height * (200f / (float)m_bmpVisible[4].Width)));
            //m_bmpTempVisible[5] = new Bitmap(m_bmpVisible[5], 200, (int)(m_bmpVisible[5].Height * (200f / (float)m_bmpVisible[5].Width)));
            //m_bmpTempVisible[6] = new Bitmap(m_bmpVisible[6], 200, (int)(m_bmpVisible[6].Height * (200f / (float)m_bmpVisible[6].Width)));

            //oGdis[0] = Graphics.FromImage(m_bmpTempVisible[0]);
            //oGdis[1] = Graphics.FromImage(m_bmpTempVisible[1]);
            //oGdis[2] = Graphics.FromImage(m_bmpTempVisible[2]);
            //oGdis[3] = Graphics.FromImage(m_bmpTempVisible[3]);
            //oGdis[4] = Graphics.FromImage(m_bmpTempVisible[4]);
            //oGdis[5] = Graphics.FromImage(m_bmpTempVisible[5]);
            //oGdis[6] = Graphics.FromImage(m_bmpTempVisible[6]);

            //oGdis[0].DrawImage(m_bmpVisible[0], new RectangleF(0, 0, m_bmpTempVisible[0].Width, m_bmpTempVisible[0].Height));
            //oGdis[1].DrawImage(m_bmpVisible[1], new RectangleF(0, 0, m_bmpTempVisible[1].Width, m_bmpTempVisible[1].Height));
            //oGdis[2].DrawImage(m_bmpVisible[2], new RectangleF(0, 0, m_bmpTempVisible[2].Width, m_bmpTempVisible[2].Height));
            //oGdis[3].DrawImage(m_bmpVisible[3], new RectangleF(0, 0, m_bmpTempVisible[3].Width, m_bmpTempVisible[3].Height));
            //oGdis[4].DrawImage(m_bmpVisible[4], new RectangleF(0, 0, m_bmpTempVisible[4].Width, m_bmpTempVisible[4].Height));
            //oGdis[5].DrawImage(m_bmpVisible[5], new RectangleF(0, 0, m_bmpTempVisible[5].Width, m_bmpTempVisible[5].Height));
            //oGdis[6].DrawImage(m_bmpVisible[6], new RectangleF(0, 0, m_bmpTempVisible[6].Width, m_bmpTempVisible[6].Height));

            //oGdis[0].Flush();
            //oGdis[1].Flush();
            //oGdis[2].Flush();
            //oGdis[3].Flush();
            //oGdis[4].Flush();
            //oGdis[5].Flush();
            //oGdis[6].Flush();

            InitializeComponent();
        }


		public void LeftMove()
        {

            for (int i = 0; i <= 100; i=i+10)
                MovePannel(-i);
            ShiftLeft();
        }

        public void RightMove()
        {
            for (int i = 0; i <= 100; i = i + 10)
                MovePannel(i);
            ShiftRight();

        }
        private void ShiftLeft()
        {
            Bitmap obmpTemp = null;
            int iTempIdx = m_arrImageIdx;
            try
            {
                if (m_arrImages.Count > 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (iTempIdx == m_arrImages.Count - 1)
                        {
                            iTempIdx = -1;
                        }
                        iTempIdx++;
                        obmpTemp = (Bitmap)((ImageSelectTag)m_arrImages[iTempIdx]).BMPImage.Clone();
                        m_bmpTempVisible[i] = new Bitmap(obmpTemp, 200, (int)(obmpTemp.Height * (200f / (float)obmpTemp.Width)));
                        if (i == 0) m_arrImageIdx = iTempIdx;
                    }
                }
                else
                {
                    obmpTemp = new Bitmap(m_bmpTempVisible[0]);
                    m_bmpTempVisible[0] = (Bitmap)m_bmpTempVisible[1].Clone();
                    m_bmpTempVisible[1] = (Bitmap)m_bmpTempVisible[2].Clone();
                    m_bmpTempVisible[2] = (Bitmap)m_bmpTempVisible[3].Clone();
                    m_bmpTempVisible[3] = (Bitmap)m_bmpTempVisible[4].Clone();
                    m_bmpTempVisible[4] = (Bitmap)m_bmpTempVisible[5].Clone();
                    m_bmpTempVisible[5] = (Bitmap)m_bmpTempVisible[6].Clone();
                    m_bmpTempVisible[6] = (Bitmap)obmpTemp.Clone();
                }

                Redraw();
                if (m_arrImageIdx > m_arrImages.Count-1) m_arrImageIdx = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ShiftRight()
        {
            Bitmap obmpTemp = null;
            int iTempIdx = m_arrImageIdx-1;
            if (iTempIdx < 0) iTempIdx = m_arrImages.Count - 1;

            if (m_arrImages.Count > 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (iTempIdx > m_arrImages.Count - 1)
                    {
                        iTempIdx = 0;
                    }
                    obmpTemp = (Bitmap)((ImageSelectTag)m_arrImages[iTempIdx]).BMPImage.Clone();
                    m_bmpTempVisible[i] = new Bitmap(obmpTemp, 200, (int)(obmpTemp.Height * (200f / (float)obmpTemp.Width)));
                    if (i == 0) m_arrImageIdx = iTempIdx;
                    iTempIdx++;
                }
            }
            else
            {
                obmpTemp = new Bitmap(m_bmpTempVisible[6]);
                m_bmpTempVisible[6] = (Bitmap)m_bmpTempVisible[5].Clone();
                m_bmpTempVisible[5] = (Bitmap)m_bmpTempVisible[4].Clone();
                m_bmpTempVisible[4] = (Bitmap)m_bmpTempVisible[3].Clone();
                m_bmpTempVisible[3] = (Bitmap)m_bmpTempVisible[2].Clone();
                m_bmpTempVisible[2] = (Bitmap)m_bmpTempVisible[1].Clone();
                m_bmpTempVisible[1] = (Bitmap)m_bmpTempVisible[0].Clone();
                m_bmpTempVisible[0] = (Bitmap)obmpTemp.Clone();
            }

            Redraw();
            if (m_arrImageIdx < 0) m_arrImageIdx = m_arrImages.Count - 1;
		}

        private void QMSImageViwer_Resize(object sender, EventArgs e)
        {
            if (DesignMode) return;
            if (this.Width * this.Height == 0)
            {
                return;
            }

            if (m_bmpMain != null) m_bmpMain.Dispose();
            m_bmpMain = new Bitmap(this.Width, this.Height);

            m_gdiMain = this.CreateGraphics();
            m_gdiTempMain = Graphics.FromImage(m_bmpMain);
            m_gdiTempMain.Clear(this.BackColor);
			if(m_bmpBack != null)
				m_gdiTempMain.DrawImage(m_bmpBack, new Rectangle(0, 0, this.Width, this.Height));

            m_iCenterControl = (int)(this.Width / 2) - 350;
            Redraw();   
        }


        public void Redraw()
        {
			if (m_gdiTempMain == null) return;
			if (m_gdiMain == null) return;

            Bitmap[] obmpTemp = null;
            try
            {
                obmpTemp = new Bitmap[7];

                obmpTemp[0] = (Bitmap)m_bmpTempVisible[0].Clone();
                obmpTemp[1] = (Bitmap)m_bmpTempVisible[1].Clone();
                obmpTemp[2] = (Bitmap)m_bmpTempVisible[2].Clone();
                obmpTemp[3] = (Bitmap)m_bmpTempVisible[3].Clone();
                obmpTemp[4] = (Bitmap)m_bmpTempVisible[4].Clone();
                obmpTemp[5] = (Bitmap)m_bmpTempVisible[5].Clone();
                obmpTemp[6] = (Bitmap)m_bmpTempVisible[6].Clone();
                         

                Brightness(obmpTemp[0], 100);
                Brightness(obmpTemp[1], 80);
                Brightness(obmpTemp[2], 50);
                Brightness(obmpTemp[4], 50);
                Brightness(obmpTemp[5], 80);
                Brightness(obmpTemp[6], 100);

                GaussianBlur(obmpTemp[0], 1000);
                GaussianBlur(obmpTemp[1], 800);
                GaussianBlur(obmpTemp[2], 500);
                GaussianBlur(obmpTemp[4], 500);
                GaussianBlur(obmpTemp[5], 800);
                GaussianBlur(obmpTemp[6], 1000);

                m_gdiTempMain.DrawImage(obmpTemp[0], new Rectangle(m_iCenterControl + 0, 0, (int)((float)obmpTemp[0].Width * 0.4f), (int)(obmpTemp[0].Height * (((float)obmpTemp[0].Width * 0.4f) / (float)obmpTemp[0].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[6], new Rectangle(m_iCenterControl + 600, 0, (int)((float)obmpTemp[6].Width * 0.4f), (int)(obmpTemp[6].Height * (((float)obmpTemp[6].Width * 0.4f) / (float)obmpTemp[6].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[1], new Rectangle(m_iCenterControl + 50, 20, (int)((float)obmpTemp[1].Width * 0.5f), (int)(obmpTemp[1].Height * (((float)obmpTemp[1].Width * 0.5f) / (float)obmpTemp[1].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[5], new Rectangle(m_iCenterControl + 550, 20, (int)((float)obmpTemp[5].Width * 0.5f), (int)(obmpTemp[5].Height * (((float)obmpTemp[5].Width * 0.5f) / (float)obmpTemp[5].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[2], new Rectangle(m_iCenterControl + 100, 40, (int)((float)obmpTemp[2].Width * 0.6f), (int)(obmpTemp[2].Height * (((float)obmpTemp[2].Width * 0.6f) / (float)obmpTemp[2].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[4], new Rectangle(m_iCenterControl + 500, 40, (int)((float)obmpTemp[4].Width * 0.6f), (int)(obmpTemp[4].Height * (((float)obmpTemp[4].Width * 0.6f) / (float)obmpTemp[4].Width))));
                m_gdiTempMain.DrawImage(obmpTemp[3], new Point(m_iCenterControl + 250, 30));

                m_gdiMain.DrawImageUnscaled(m_bmpMain, 0, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void MovePannel(int Pct)
        {
            Bitmap[] obmpTemp = null;
            double r = (double)(Pct / 100d);
            int xm = (int)(50 * r);
            int ym = (int)(20 * r);
            int size = (int)(20 * r);

            int xm2 = (int)(150 * r);
            int ym2 = (int)(10 * r);
            int size2 = (int)(80 * r);

            int xm3 = (int)(150 * r);
            int ym3 = (int)(10 * r);
            int size3 = (int)(80 * r);

            int xm4 = (int)(250 * r);
            int ym4 = (int)(10 * r);
            int size4 = (int)(80 * r);

            try
            {
                m_gdiTempMain.Clear(this.BackColor);
                if(m_bmpBack != null) m_gdiTempMain.DrawImage(m_bmpBack, new Rectangle(0, 0, this.Width, this.Height));
                obmpTemp = new Bitmap[7];


                obmpTemp[0] = (Bitmap)m_bmpTempVisible[0].Clone();
                obmpTemp[1] = (Bitmap)m_bmpTempVisible[1].Clone();
                obmpTemp[2] = (Bitmap)m_bmpTempVisible[2].Clone();
                obmpTemp[3] = (Bitmap)m_bmpTempVisible[3].Clone();
                obmpTemp[4] = (Bitmap)m_bmpTempVisible[4].Clone();
                obmpTemp[5] = (Bitmap)m_bmpTempVisible[5].Clone();
                obmpTemp[6] = (Bitmap)m_bmpTempVisible[6].Clone();


                Brightness(obmpTemp[0], 100);
                Brightness(obmpTemp[1], 80);
                Brightness(obmpTemp[2], 50);
                Brightness(obmpTemp[4], 50);
                Brightness(obmpTemp[5], 80);
                Brightness(obmpTemp[6], 100);

                GaussianBlur(obmpTemp[0], 1000);
                GaussianBlur(obmpTemp[1], 800);
                GaussianBlur(obmpTemp[2], 500);
                GaussianBlur(obmpTemp[4], 500);
                GaussianBlur(obmpTemp[5], 800);
                GaussianBlur(obmpTemp[6], 1000);


                if (Pct < 0)
                {
                    m_gdiTempMain.DrawImage(obmpTemp[0], new Rectangle(m_iCenterControl + 0 + xm
                        , 0 + ym
                        , (int)((float)obmpTemp[0].Width * 0.4f) + size
                        , (int)(obmpTemp[0].Height * ((float)(((float)obmpTemp[0].Width * 0.4f) + size) / (float)obmpTemp[0].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[6], new Rectangle(m_iCenterControl + 600 + xm
                        , 0 - ym
                        , (int)((float)obmpTemp[6].Width * 0.4f) - size
                        , (int)(obmpTemp[6].Height * ((float)(((float)obmpTemp[6].Width * 0.4f) - size) / (float)obmpTemp[6].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[1], new Rectangle(m_iCenterControl + 50 + xm
                        , 20 + ym
                        , (int)((float)obmpTemp[1].Width * 0.5f) + size
                        , (int)(obmpTemp[1].Height * ((float)(((float)obmpTemp[1].Width * 0.5f) + size) / (float)obmpTemp[1].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[5], new Rectangle(m_iCenterControl + 550 + xm
                        , 20 - ym
                        , (int)((float)obmpTemp[5].Width * 0.5f) - size
                        , (int)(obmpTemp[5].Height * ((float)(((float)obmpTemp[5].Width * 0.5f) - size) / (float)obmpTemp[5].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[2], new Rectangle(m_iCenterControl + 100 + xm
                        , 40 + ym
                        , (int)((float)obmpTemp[2].Width * 0.6f) + size
                        , (int)(obmpTemp[2].Height * ((float)(((float)obmpTemp[2].Width * 0.6f) + size) / (float)obmpTemp[2].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[3], new Rectangle(m_iCenterControl + 250 + xm2
                        , 30 - ym2
                        , obmpTemp[3].Width + size2
                        , (int)(obmpTemp[3].Height * ((float)(obmpTemp[3].Width + size2) / (float)obmpTemp[3].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[4], new Rectangle(m_iCenterControl + 500 + xm4
                        , 40 + ym4
                        , (int)((float)obmpTemp[4].Width * 0.6f) - size4
                        , (int)(obmpTemp[4].Height * ((float)((float)(obmpTemp[4].Width * 0.6f) - size4) / (float)obmpTemp[4].Width))));
                }
                else if (Pct > 0)
                {
                    m_gdiTempMain.DrawImage(obmpTemp[0], new Rectangle(m_iCenterControl + 0 + xm
                        , 0 + ym
                        , (int)((float)obmpTemp[0].Width * 0.4f) + size
                        , (int)(obmpTemp[0].Height * ((float)(((float)obmpTemp[0].Width * 0.4f) + size) / (float)obmpTemp[0].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[6], new Rectangle(m_iCenterControl + 600 + xm
                        , 0 - ym
                        , (int)((float)obmpTemp[6].Width * 0.4f) - size
                        , (int)(obmpTemp[6].Height * ((float)(((float)obmpTemp[6].Width * 0.4f) - size) / (float)obmpTemp[6].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[1], new Rectangle(m_iCenterControl + 50 + xm
                        , 20 + ym
                        , (int)((float)obmpTemp[1].Width * 0.5f) + size
                        , (int)(obmpTemp[1].Height * ((float)(((float)obmpTemp[1].Width * 0.5f) + size) / (float)obmpTemp[1].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[5], new Rectangle(m_iCenterControl + 550 + xm
                        , 20 - ym
                        , (int)((float)obmpTemp[5].Width * 0.5f) - size
                        , (int)(obmpTemp[5].Height * ((float)(((float)obmpTemp[5].Width * 0.5f) - size) / (float)obmpTemp[5].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[4], new Rectangle(m_iCenterControl + 500 + xm
                        , 40 - ym
                        , (int)((float)obmpTemp[4].Width * 0.6f) - size
                        , (int)(obmpTemp[4].Height * ((float)(((float)obmpTemp[4].Width * 0.6f) - size) / (float)obmpTemp[4].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[3], new Rectangle(m_iCenterControl + 250 + xm4
                        , 30 + ym4, obmpTemp[3].Width - size4
                        , (int)(obmpTemp[3].Height * ((float)(obmpTemp[3].Width - size4) / (float)obmpTemp[3].Width))));
                    m_gdiTempMain.DrawImage(obmpTemp[2], new Rectangle(m_iCenterControl + 100 + xm3
                        , 40 - ym3
                        , (int)((float)obmpTemp[2].Width * 0.6f) + size3
                        , (int)(obmpTemp[2].Height * ((float)(((float)obmpTemp[2].Width * 0.6f) + size3) / (float)obmpTemp[2].Width))));

                }
                else
                {
                    return;
                }

                //m_gdiTempMain.DrawImage(obmpTemp[3], new Point(250 + xm, 30));

                m_gdiMain.DrawImageUnscaled(m_bmpMain, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public static bool Brightness(Bitmap b, int nBrightness)
        {
            if (nBrightness < -255 || nBrightness > 255)
                return false;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            int nVal = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + nBrightness);

                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;

                        p[0] = (byte)nVal;

                        ++p;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            return true;
        }

        public static bool GaussianBlur(Bitmap b, int nWeight /* default to 4*/)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Factor = nWeight + 12;

            return Conv3x3(b, m);
        }

        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }
                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LeftMove();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RightMove();
        }

        private void QMSImageViwer_MouseMove(object sender, MouseEventArgs e)
        {
            int iGap = 0;
            int iCnt = 0;
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    iGap = (e.X - m_iMouseStart) % 100;
                    iCnt = (int)((e.X - m_iMouseStart) / 100);
                    if (iCnt < 0)
                    {
                        ShiftLeft();
                        m_iMouseStart = m_iMouseStart + (iCnt * 100);
                    }
                    else if (iCnt > 0)
                    {
                        ShiftRight();
                        m_iMouseStart = m_iMouseStart + (iCnt * 100);
                    }
                    else
                    {
                        MovePannel(iGap);
                    }
                }
            }
            catch
            {
            }
        }

        private void QMSImageViwer_MouseDown(object sender, MouseEventArgs e)
        {
            m_iMouseStart = e.X;
        }

        private void QMSImageViwer_MouseUp(object sender, MouseEventArgs e)
        {
            int iGap = 0;
            //int iCnt = 0;
            try
            {
                iGap = (e.X - m_iMouseStart) % 100;
                if (iGap < 0)
                {
                    if (iGap > -50)
                    {
                        for (int i = iGap; i <= 0; i = i + 10)
                        {
                            MovePannel(i);
                        }
                        MovePannel(0);
                    }
                    else
                    {
                        for (int i = iGap; i > -100; i = i - 10)
                        {
                            MovePannel(i);
                        }
                        MovePannel(-100);
                        ShiftLeft();
                    }
                }
                else if (iGap>0)
                {
                    if (iGap < 50)
                    {
                        for (int i = iGap; i >= 0; i = i - 10)
                        {
                            MovePannel(i);
                        }
                        MovePannel(0);
                    }
                    else
                    {
                        for (int i = iGap; i < 100; i = i + 10)
                        {
                            MovePannel(i);
                        }
                        MovePannel(100);
                        ShiftRight();
                    }
                }
            }
            catch
            {
            }
        }

        private void QMSImageViwer_Paint(object sender, PaintEventArgs e)
        {
			if (DesignMode) return;
            Redraw();
        }

        private void QMSImageViwer_MouseClick(object sender, MouseEventArgs e)
        {
            int iban = this.Width / 2;
            try
            {
                if (e.X < iban)
                {
                    LeftMove();
                }
                else
                {
                    RightMove();
                }
            }
            catch (Exception ex)
            {
                DACrux.Framework.DCMH.DspError(ex);
            }
        }
    }

    public class ConvMatrix
    {
        public int TopLeft = 0, TopMid = 0, TopRight = 0;
        public int MidLeft = 0, Pixel = 1, MidRight = 0;
        public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
        public int Factor = 1;
        public int Offset = 0;
        public void SetAll(int nVal)
        {
            TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
        }
    }
}
