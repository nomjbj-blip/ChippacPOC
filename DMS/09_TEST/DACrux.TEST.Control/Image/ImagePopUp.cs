using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DACrux.TEST.Control
{
    public partial class ImagePopUp : Form
    {
        public ImagePopUp()
        {
            InitializeComponent();
        }

        //--

        public ImagePopUp(
            System.Drawing.Image oImage
            )
            :this()
        {
            ImageControl.ImageControlDraw(
                this,
                oImage
                );
        }

        //--

        public ImagePopUp(
            string imagepath
            )
            : this()
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = File.OpenRead(imagepath))
            {
                fs.CopyTo(ms);
            }

            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            ImageControl.ImageControlDraw(
                this,
                image
                );
        }

        private void ImagePopUp_KeyUp(object sender, KeyEventArgs e)
        {
            //현재 창을 끈다.
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            //Control C 키 조합시 이미지를 clipBoard 에 저장 한다.
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {
                Clipboard.SetImage(ImageControl.DefectImg);
            }
        }
    }
}
