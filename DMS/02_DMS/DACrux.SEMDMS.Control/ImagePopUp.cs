using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DACrux.SEMDMS.Control
{
    public partial class ImagePopUp : Form
    {
        private Image _defectImage = null;
        public ImagePopUp()
        {
            InitializeComponent();
        }

        //--

        public ImagePopUp(
            Image oImage
            )
            : this()
        {
            _defectImage = oImage;
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

            Image image = Image.FromStream(ms);
            _defectImage = image;
            ImageControl.ImageControlDraw(
                this,
                image
                );
        }

        private void ImagePopUp_KeyDown(
            object sender,
            KeyEventArgs e
            )
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
                e.Handled = true;
            }
        }

        #region [ Property ]
        public Image DefectImage
        {
            get { return _defectImage; }
            set
            {
                _defectImage = value;
                ImageControl.ImageControlDraw(this, _defectImage);
            }
        }
        #endregion [ Property ]
    }
}
