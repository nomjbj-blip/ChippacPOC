namespace DACrux.TEST.Control
{
    partial class TPUImageInfo
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
            this.lbXindex = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbYindex = new System.Windows.Forms.Label();
            this.imageControl1 = new DACrux.TEST.Control.ImageControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lbWaferID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbProgram = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbDevice = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbBin = new System.Windows.Forms.Label();
            this.BtnFocus = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbXindex
            // 
            this.lbXindex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbXindex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbXindex.Location = new System.Drawing.Point(67, 239);
            this.lbXindex.Name = "lbXindex";
            this.lbXindex.Size = new System.Drawing.Size(59, 22);
            this.lbXindex.TabIndex = 17;
            this.lbXindex.Text = "0";
            this.lbXindex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "X Index";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(126, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 22);
            this.label1.TabIndex = 11;
            this.label1.Text = "Y Index";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbYindex
            // 
            this.lbYindex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbYindex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbYindex.Location = new System.Drawing.Point(198, 239);
            this.lbYindex.Name = "lbYindex";
            this.lbYindex.Size = new System.Drawing.Size(92, 22);
            this.lbYindex.TabIndex = 17;
            this.lbYindex.Text = "0";
            this.lbYindex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageControl1
            // 
            this.imageControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageControl1.DefectImg = null;
            this.imageControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageControl1.Location = new System.Drawing.Point(0, 0);
            this.imageControl1.Name = "imageControl1";
            this.imageControl1.Size = new System.Drawing.Size(287, 233);
            this.imageControl1.TabIndex = 20;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.imageControl1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 233);
            this.panel1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(0, 283);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 22);
            this.label3.TabIndex = 12;
            this.label3.Text = "Wafer ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbWaferID
            // 
            this.lbWaferID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbWaferID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbWaferID.Location = new System.Drawing.Point(67, 283);
            this.lbWaferID.Name = "lbWaferID";
            this.lbWaferID.Size = new System.Drawing.Size(223, 22);
            this.lbWaferID.TabIndex = 17;
            this.lbWaferID.Text = "-";
            this.lbWaferID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(0, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 22);
            this.label5.TabIndex = 12;
            this.label5.Text = "Program";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProgram
            // 
            this.lbProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbProgram.Location = new System.Drawing.Point(67, 305);
            this.lbProgram.Name = "lbProgram";
            this.lbProgram.Size = new System.Drawing.Size(223, 22);
            this.lbProgram.TabIndex = 17;
            this.lbProgram.Text = "-";
            this.lbProgram.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(0, 327);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 22);
            this.label7.TabIndex = 12;
            this.label7.Text = "Device";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbDevice
            // 
            this.lbDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDevice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbDevice.Location = new System.Drawing.Point(67, 327);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(223, 22);
            this.lbDevice.TabIndex = 17;
            this.lbDevice.Text = "-";
            this.lbDevice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(0, 261);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 22);
            this.label10.TabIndex = 12;
            this.label10.Text = "Bin";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBin
            // 
            this.lbBin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBin.Location = new System.Drawing.Point(67, 261);
            this.lbBin.Name = "lbBin";
            this.lbBin.Size = new System.Drawing.Size(131, 22);
            this.lbBin.TabIndex = 17;
            this.lbBin.Text = "-";
            this.lbBin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnFocus
            // 
            this.BtnFocus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFocus.BackColor = System.Drawing.Color.Gray;
            this.BtnFocus.Location = new System.Drawing.Point(198, 260);
            this.BtnFocus.Name = "BtnFocus";
            this.BtnFocus.Size = new System.Drawing.Size(92, 23);
            this.BtnFocus.TabIndex = 19;
            this.BtnFocus.Text = "Find Index";
            this.BtnFocus.UseVisualStyleBackColor = false;
            this.BtnFocus.Click += new System.EventHandler(this.BtnFocus_Click);
            // 
            // TPUImageInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.BtnFocus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbYindex);
            this.Controls.Add(this.lbDevice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbProgram);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbWaferID);
            this.Controls.Add(this.lbBin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lbXindex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TPUImageInfo";
            this.Size = new System.Drawing.Size(289, 349);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbXindex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbYindex;
        private ImageControl imageControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbWaferID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbProgram;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbDevice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbBin;
        private System.Windows.Forms.Button BtnFocus;
    }
}
