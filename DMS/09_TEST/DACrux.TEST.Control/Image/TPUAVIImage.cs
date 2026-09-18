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

namespace DACrux.TEST.Control.Image
{
    public partial class TPUAVIImage : UserControl
    {
        public TPUAVIImage()
        {
            InitializeComponent();
        }

        public TPUAVIImage(string strImage,
            string strXindex, 
            string strYindex, 
            string strBin)
        {
            InitializeComponent();

            MemoryStream ms = null;
            FileInfo oImage = null;
            try
            {

                lbXindex.Text = strXindex;
                lbYindex.Text = strYindex;
                lbBin.Text = strBin;

                oImage = new FileInfo(strImage);
                ms = new MemoryStream();
                using (FileStream fs = File.OpenRead(strImage))
                {
                    fs.CopyTo(ms);
                }

                ImagePic.Image = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception) { }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                ms = null;
            }
        }
    }
}
