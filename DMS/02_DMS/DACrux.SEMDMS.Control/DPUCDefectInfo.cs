using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCDefectInfo : UserControl
    {
        private Image DefectImg;
        private string m_ImageFTPPath = string.Empty;

        public DPUCDefectInfo()
        {
            InitializeComponent();
        }

        public DPUCDefectInfo(
            string strDefectNo,
            string strImage,
            string strXindex,
            string strYindex,
            string strClass,
            string strCluster,
            string strRelX,
            string strRelY,
            string strXSize,
            string strYSize
            )
        {
            InitializeComponent();

            try
            {
                lbDefectNo.Text = strDefectNo;
                lbXindex.Text = strXindex;
                lbYindex.Text = strYindex;
                lbClass.Text = strClass;
                lbCluster.Text = strCluster;
                lbRelX.Text = strRelX;
                lbRelY.Text = strRelY;
                lbXSize.Text = strXSize;
                lbYSize.Text = strYSize;

                m_ImageFTPPath = strImage;

                imageControl1.ImageControlDraw(
                    null,
                    DefectimageList.Images[0]
                    );

                System.Threading.Thread.Sleep(10);
                System.Threading.Thread trd = new System.Threading.Thread(new System.Threading.ThreadStart(DrawImage));
                trd.Start();
            }
            catch
            {
                imageControl1.ImageControlDraw(
                    null,
                    DefectimageList.Images[1]
                    );
            }
        }

        private void DrawImage()
        {
            MemoryStream ms = null;
            FileInfo oImage = null;
            try
            {
                oImage = DefectMapDraw.fnFTPImageDownload(m_ImageFTPPath);
                if (oImage == null)
                    return;

                if (oImage.Exists == false)
                    return;

                ms = new MemoryStream();
                using (FileStream fs = File.OpenRead(oImage.FullName))
                {
                    fs.CopyTo(ms);
                }

                DefectImg = Image.FromStream(ms);

                imageControl1.ImageControlDraw(
                    null,
                    DefectImg
                    );
            }
            catch (Exception) { }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                ms = null;

                //Image File 은 사용후 삭제 한다.
                if (oImage != null && oImage.Exists)
                    oImage.Delete();
            }
        }


    }
}
