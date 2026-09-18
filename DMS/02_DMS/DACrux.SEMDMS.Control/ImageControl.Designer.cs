namespace DACrux.SEMDMS.Control
{
    partial class ImageControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageControl));
            this.BrighttrackBar = new System.Windows.Forms.TrackBar();
            this.DectImage = new System.Windows.Forms.PictureBox();
            this.GammatrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.BrighttrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DectImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GammatrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // BrighttrackBar
            // 
            this.BrighttrackBar.AutoSize = false;
            this.BrighttrackBar.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BrighttrackBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrighttrackBar.Location = new System.Drawing.Point(0, 398);
            this.BrighttrackBar.Maximum = 100;
            this.BrighttrackBar.Minimum = -100;
            this.BrighttrackBar.Name = "BrighttrackBar";
            this.BrighttrackBar.Size = new System.Drawing.Size(521, 21);
            this.BrighttrackBar.TabIndex = 20;
            this.BrighttrackBar.Scroll += new System.EventHandler(this.BrighttrackBar_Scroll);
            // 
            // DectImage
            // 
            this.DectImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DectImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DectImage.InitialImage = ((System.Drawing.Image)(resources.GetObject("DectImage.InitialImage")));
            this.DectImage.Location = new System.Drawing.Point(0, 0);
            this.DectImage.Name = "DectImage";
            this.DectImage.Size = new System.Drawing.Size(500, 398);
            this.DectImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DectImage.TabIndex = 22;
            this.DectImage.TabStop = false;
            this.DectImage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DectImage_MouseDoubleClick);
            // 
            // GammatrackBar
            // 
            this.GammatrackBar.AutoSize = false;
            this.GammatrackBar.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.GammatrackBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.GammatrackBar.Location = new System.Drawing.Point(500, 0);
            this.GammatrackBar.Minimum = 1;
            this.GammatrackBar.Name = "GammatrackBar";
            this.GammatrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.GammatrackBar.Size = new System.Drawing.Size(21, 398);
            this.GammatrackBar.TabIndex = 23;
            this.GammatrackBar.Value = 1;
            this.GammatrackBar.Scroll += new System.EventHandler(this.GammatrackBar_Scroll);
            // 
            // ImageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.DectImage);
            this.Controls.Add(this.GammatrackBar);
            this.Controls.Add(this.BrighttrackBar);
            this.Name = "ImageControl";
            this.Size = new System.Drawing.Size(521, 419);
            ((System.ComponentModel.ISupportInitialize)(this.BrighttrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DectImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GammatrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar BrighttrackBar;
        private System.Windows.Forms.PictureBox DectImage;
        private System.Windows.Forms.TrackBar GammatrackBar;
    }
}
