namespace DACrux.TEST.Control.Image
{
    partial class TPUAVIImage
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
            this.ImagePic = new System.Windows.Forms.PictureBox();
            this.lbYindex = new System.Windows.Forms.Label();
            this.lbBin = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbXindex = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePic)).BeginInit();
            this.SuspendLayout();
            // 
            // ImagePic
            // 
            this.ImagePic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagePic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ImagePic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagePic.Location = new System.Drawing.Point(0, 0);
            this.ImagePic.Name = "ImagePic";
            this.ImagePic.Size = new System.Drawing.Size(247, 265);
            this.ImagePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImagePic.TabIndex = 0;
            this.ImagePic.TabStop = false;
            // 
            // lbYindex
            // 
            this.lbYindex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbYindex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbYindex.Location = new System.Drawing.Point(191, 268);
            this.lbYindex.Name = "lbYindex";
            this.lbYindex.Size = new System.Drawing.Size(56, 22);
            this.lbYindex.TabIndex = 21;
            this.lbYindex.Text = "0";
            this.lbYindex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBin
            // 
            this.lbBin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBin.Location = new System.Drawing.Point(67, 290);
            this.lbBin.Name = "lbBin";
            this.lbBin.Size = new System.Drawing.Size(180, 22);
            this.lbBin.TabIndex = 22;
            this.lbBin.Text = "-";
            this.lbBin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(0, 290);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 22);
            this.label10.TabIndex = 20;
            this.label10.Text = "Bin";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbXindex
            // 
            this.lbXindex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbXindex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbXindex.Location = new System.Drawing.Point(67, 268);
            this.lbXindex.Name = "lbXindex";
            this.lbXindex.Size = new System.Drawing.Size(60, 22);
            this.lbXindex.TabIndex = 23;
            this.lbXindex.Text = "0";
            this.lbXindex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 22);
            this.label2.TabIndex = 19;
            this.label2.Text = "X Index";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(126, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 22);
            this.label1.TabIndex = 18;
            this.label1.Text = "Y Index";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TPUAVIImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbYindex);
            this.Controls.Add(this.lbBin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lbXindex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ImagePic);
            this.Name = "TPUAVIImage";
            this.Size = new System.Drawing.Size(246, 311);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImagePic;
        private System.Windows.Forms.Label lbYindex;
        private System.Windows.Forms.Label lbBin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbXindex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
