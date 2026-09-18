using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// LargeImage에 대한 요약 설명입니다.
	/// </summary>
	public class LargeImage : System.Windows.Forms.Form
	{
		public string strSaveFileName;

		public System.Windows.Forms.PictureBox gimg;
		private System.Windows.Forms.ContextMenu cmn;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolTip tip;
        private System.Windows.Forms.MenuItem menuItem3;
		private System.ComponentModel.IContainer components;

		public LargeImage()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
			strSaveFileName = "";
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.gimg = new System.Windows.Forms.PictureBox();
            this.cmn = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gimg)).BeginInit();
            this.SuspendLayout();
            // 
            // gimg
            // 
            this.gimg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gimg.Location = new System.Drawing.Point(0, 0);
            this.gimg.Name = "gimg";
            this.gimg.Size = new System.Drawing.Size(512, 512);
            this.gimg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.gimg.TabIndex = 0;
            this.gimg.TabStop = false;
            this.tip.SetToolTip(this.gimg, "Right Clk : Menu");
            this.gimg.SizeChanged += new System.EventHandler(this.pictureBox_SizeChanged);
            this.gimg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // cmn
            // 
            this.cmn.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "&Save Image";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuItem3.Text = "&Copy to Clipboard";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "&Close";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.Filter = "Png files (*.png)|*.png|Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|Gif f" +
    "iles (*.gif)|*.gif";
            // 
            // LargeImage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.gimg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LargeImage";
            this.Text = "LargeImage";
            ((System.ComponentModel.ISupportInitialize)(this.gimg)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void pictureBox_SizeChanged(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

			this.Size = new System.Drawing.Size(gimg.Width, gimg.Height);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
//			string year = DateTime.Now.Year.ToString();
//			string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
//			string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
//			string hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
//			string minute = DateTime.Now.Minute.ToString().PadLeft(2, '0');
//			string second = DateTime.Now.Second.ToString().PadLeft(2, '0');

			saveFileDialog.Filter = "Png files (*.png)|*.png|Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif";
			saveFileDialog.DefaultExt = "png";
//			saveFileDialog.FileName = string.Format("{0}_{1}_{2}_{3}{4}{5}{6}{7}{8}",
//				map.lotId, map.waferId, map.stepId, year, month, day, hour, minute, second);
			saveFileDialog.FileName = this.Text;

			if(saveFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;

			Image image = gimg.Image;
			switch(saveFileDialog.FilterIndex)
			{
				case 1:
					image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
					break;
				case 2:
					image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
					break;
				case 3:
					image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
					break;
				default:
					image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Gif);
					break;
			}

			System.Windows.Forms.MessageBox.Show(this, "Save completion", "DMSPlus");
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void pictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == System.Windows.Forms.MouseButtons.Right)	cmn.Show(this, new System.Drawing.Point(e.X, e.Y));
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(this.gimg.Image);
		}
	}
}
