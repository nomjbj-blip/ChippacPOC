using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MenuFileEditor
{
    /// <summary>
    /// 메뉴 아이콘 편집 창 (2026-10-15).
    /// 목적: ICON32 / ICON16 값을 그림으로 보며 바꾼다.
    /// 처리: [그림 열기] = PNG / ICO / BMP / GIF / JPG 를 읽어 지정 크기(32 또는 16) PNG 로 줄여 DACrux 아이콘 문자열로 바꾼다(비율 유지, 투명 배경).
    ///       [지우기] = 빈 값. [그림 저장] = 지금 아이콘을 PNG 파일로 저장. [확인] 때만 값이 반영된다.
    /// </summary>
    public partial class IconEditForm : Form
    {
        private readonly int iconSize;

        public IconEditForm()
            : this(string.Empty, 32)
        {
        }

        public IconEditForm(string hex, int iconSize)
        {
            InitializeComponent();
            this.iconSize = iconSize;
            HexValue = hex ?? string.Empty;
            Text = string.Format(CultureInfo.InvariantCulture, "아이콘 {0} x {0}", iconSize);
            ShowPreview();
        }

        /// <summary>편집 결과(DACrux 아이콘 문자열, 빈 값 가능).</summary>
        public string HexValue { get; private set; }

        /// <summary>그림 파일을 읽어 아이콘 값으로 넣는다(자동 검사도 이 함수를 쓴다).</summary>
        public void LoadImageFile(string path)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(path)))
            using (Image image = Image.FromStream(stream))
            {
                HexValue = MenuDocument.ImageToHex(image, iconSize);
            }
            ShowPreview();
        }

        private void ShowPreview()
        {
            Image old = picPreview.Image;
            Image image = MenuDocument.HexToImage(HexValue);
            picPreview.Image = image;
            picActual.Image = image;
            if (old != null) old.Dispose();
            if (HexValue.Trim().Length == 0)
                lblInfo.Text = "(없음)";
            else if (image == null)
                lblInfo.Text = "그림으로 읽을 수 없는 값";
            else
                lblInfo.Text = string.Format(CultureInfo.InvariantCulture, "{0} x {1} / {2:N0} byte", image.Width, image.Height, MenuDocument.HexToBytes(HexValue).Length);
            btnSaveImage.Enabled = image != null;
            btnClear.Enabled = HexValue.Trim().Length > 0;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "그림 파일 (*.png;*.ico;*.bmp;*.gif;*.jpg)|*.png;*.ico;*.bmp;*.gif;*.jpg;*.jpeg|모든 파일 (*.*)|*.*";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    LoadImageFile(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "그림을 읽을 수 없습니다.\r\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            HexValue = string.Empty;
            ShowPreview();
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (picPreview.Image == null) return;
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "PNG 파일 (*.png)|*.png";
                dialog.FileName = "icon" + iconSize.ToString(CultureInfo.InvariantCulture) + ".png";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                File.WriteAllBytes(dialog.FileName, MenuDocument.HexToBytes(HexValue));
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Image image = picPreview.Image;
            picPreview.Image = null;
            picActual.Image = null;
            if (image != null) image.Dispose();
            base.OnFormClosed(e);
        }
    }
}
