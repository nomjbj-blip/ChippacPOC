using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace DACruxV5
{
    public partial class FrmIcon : Form
    {
        public FrmIcon()
        {
            InitializeComponent();
        }

        public static void SaveImageFile(Image image)
        {
            if (image == null)
                return;

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                ImageFormat format;
                string ext;

                Util.GetImageFormat(image, out format, out ext);
                dlg.Filter = "Image File|*" + ext;
                
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    image.Save(dlg.FileName, format);
            }
        }

        private void FrmIcon_Load(object sender, EventArgs e)
        {
            picIcon16.Image = Util.StringToImage(Icon16X16);
            picIcon32.Image = Util.StringToImage(Icon32X32);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.bmp;*.emf;*.exif;*.ico;*.jpg;*.png;*.tif;*.wmf";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (Image image = Image.FromFile(dlg.FileName))
                    {
                        if (Apply == ApplyMode.All || Apply == ApplyMode.Icon16X16)
                            picIcon16.Image = Util.GetImage(image, 16);

                        if (Apply == ApplyMode.All || Apply == ApplyMode.Icon32X32)
                            picIcon32.Image = Util.GetImage(image, 32);
                    }
                }
            }
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            picIcon16.Image = picIcon32.Image = null;
        }

        private void btnSaveAs16_Click(object sender, EventArgs e)
        {
            SaveImageFile(picIcon16.Image);
        }

        private void btnSaveAs32_Click(object sender, EventArgs e)
        {
            SaveImageFile(picIcon32.Image);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you want to Apply?", "Apply", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                != System.Windows.Forms.DialogResult.OK)
                return;

            Icon16X16 = Util.BytesToString(Util.ImageToBytes(picIcon16.Image));
            Icon32X32 = Util.BytesToString(Util.ImageToBytes(picIcon32.Image));
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public string Icon16X16
        {
            get;
            set;
        }

        public string Icon32X32
        {
            get;
            set;
        }

        public ApplyMode Apply
        {
            get { return rb16X16.Checked ? ApplyMode.Icon16X16 : rb32X32.Checked ? ApplyMode.Icon32X32 : ApplyMode.All; }
        }

        public enum ApplyMode
        {
            All,
            Icon16X16,
            Icon32X32
        }
    }
}
