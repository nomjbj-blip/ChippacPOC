namespace DACrux.Utility
{
	partial class HButton
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
			this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBoxIcon
			// 
			this.pictureBoxIcon.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBoxIcon.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxIcon.Name = "pictureBoxIcon";
			this.pictureBoxIcon.Size = new System.Drawing.Size(34, 31);
			this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxIcon.TabIndex = 0;
			this.pictureBoxIcon.TabStop = false;
			this.pictureBoxIcon.MouseLeave += new System.EventHandler(this.pictureBoxIcon_MouseLeave);
			this.pictureBoxIcon.Click += new System.EventHandler(this.pictureBoxIcon_Click);
			this.pictureBoxIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxIcon_MouseDown);
			this.pictureBoxIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxIcon_MouseUp);
			this.pictureBoxIcon.MouseEnter += new System.EventHandler(this.pictureBoxIcon_MouseEnter);
			// 
			// HButton
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBoxIcon);
			this.Name = "HButton";
			this.Size = new System.Drawing.Size(104, 31);
			this.MouseLeave += new System.EventHandler(this.HButton_MouseLeave);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HButton_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HButton_MouseUp);
			this.SizeChanged += new System.EventHandler(this.HButton_SizeChanged);
			this.MouseEnter += new System.EventHandler(this.HButton_MouseEnter);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBoxIcon;
	}
}
