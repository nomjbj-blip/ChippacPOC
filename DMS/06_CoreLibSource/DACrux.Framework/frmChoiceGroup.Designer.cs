namespace DACrux.Framework
{
    partial class frmChoiceGroup
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChoiceGroup));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.PictureBox();
            this.txtGrpCaption = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.txtGrpName = new System.Windows.Forms.TextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.txtGrpCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstGroup = new System.Windows.Forms.ListBox();
            this.lblGroupDesc = new System.Windows.Forms.Label();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.lblGroupCode = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblGroupDesc);
            this.pnlTop.Controls.Add(this.lblGroupName);
            this.pnlTop.Controls.Add(this.lblGroupCode);
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Controls.Add(this.txtGrpCaption);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.txtGrpName);
            this.pnlTop.Controls.Add(this.pictureBox4);
            this.pnlTop.Controls.Add(this.txtGrpCode);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.lstGroup);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(560, 224);
            this.pnlTop.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(405, 10);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 22);
            this.btnOK.TabIndex = 120;
            this.btnOK.TabStop = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtGrpCaption
            // 
            this.txtGrpCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrpCaption.BackColor = System.Drawing.Color.MintCream;
            this.txtGrpCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpCaption.Location = new System.Drawing.Point(292, 183);
            this.txtGrpCaption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpCaption.Multiline = true;
            this.txtGrpCaption.Name = "txtGrpCaption";
            this.txtGrpCaption.ReadOnly = true;
            this.txtGrpCaption.Size = new System.Drawing.Size(256, 22);
            this.txtGrpCaption.TabIndex = 119;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(483, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 63;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtGrpName
            // 
            this.txtGrpName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrpName.BackColor = System.Drawing.Color.MintCream;
            this.txtGrpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpName.Location = new System.Drawing.Point(292, 129);
            this.txtGrpName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpName.Name = "txtGrpName";
            this.txtGrpName.ReadOnly = true;
            this.txtGrpName.Size = new System.Drawing.Size(256, 22);
            this.txtGrpName.TabIndex = 118;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(8, 38);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(545, 6);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 34;
            this.pictureBox4.TabStop = false;
            // 
            // txtGrpCode
            // 
            this.txtGrpCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtGrpCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpCode.Location = new System.Drawing.Point(292, 75);
            this.txtGrpCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpCode.Name = "txtGrpCode";
            this.txtGrpCode.ReadOnly = true;
            this.txtGrpCode.Size = new System.Drawing.Size(99, 22);
            this.txtGrpCode.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 26);
            this.label1.TabIndex = 33;
            this.label1.Text = "      Select User Security Group";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstGroup
            // 
            this.lstGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstGroup.FormattingEnabled = true;
            this.lstGroup.ItemHeight = 14;
            this.lstGroup.Location = new System.Drawing.Point(12, 56);
            this.lstGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstGroup.Name = "lstGroup";
            this.lstGroup.Size = new System.Drawing.Size(264, 156);
            this.lstGroup.TabIndex = 113;
            this.lstGroup.SelectedIndexChanged += new System.EventHandler(this.lslSecGroup_SelectedIndexChanged);
            // 
            // lblGroupDesc
            // 
            this.lblGroupDesc.AutoSize = true;
            this.lblGroupDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupDesc.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupDesc.Image")));
            this.lblGroupDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupDesc.Location = new System.Drawing.Point(292, 164);
            this.lblGroupDesc.Name = "lblGroupDesc";
            this.lblGroupDesc.Size = new System.Drawing.Size(124, 14);
            this.lblGroupDesc.TabIndex = 123;
            this.lblGroupDesc.Text = "     Group Description";
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupName.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupName.Image")));
            this.lblGroupName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupName.Location = new System.Drawing.Point(292, 110);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(95, 14);
            this.lblGroupName.TabIndex = 122;
            this.lblGroupName.Text = "     Group Name";
            // 
            // lblGroupCode
            // 
            this.lblGroupCode.AutoSize = true;
            this.lblGroupCode.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupCode.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupCode.Image")));
            this.lblGroupCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupCode.Location = new System.Drawing.Point(292, 56);
            this.lblGroupCode.Name = "lblGroupCode";
            this.lblGroupCode.Size = new System.Drawing.Size(92, 14);
            this.lblGroupCode.TabIndex = 121;
            this.lblGroupCode.Text = "     Group Code";
            // 
            // frmChoiceGroup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(560, 224);
            this.ControlBox = false;
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmChoiceGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Security Group";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstGroup;
        private System.Windows.Forms.TextBox txtGrpCaption;
        private System.Windows.Forms.TextBox txtGrpName;
        private System.Windows.Forms.TextBox txtGrpCode;
        private System.Windows.Forms.PictureBox btnOK;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.Label lblGroupCode;
        private System.Windows.Forms.Label lblGroupDesc;
    }
}