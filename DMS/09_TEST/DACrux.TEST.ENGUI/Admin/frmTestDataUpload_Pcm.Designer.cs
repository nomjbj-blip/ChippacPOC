namespace DACrux.TEST.ENGUI
{
    partial class frmTestDataUpload_Pcm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUserComment = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstEquipID = new DACrux.Framework.Controls.DUCListBox();
            this.pnlDataInput = new System.Windows.Forms.Panel();
            this.txtOperator = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProbeCard = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEquipID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlDataInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUserComment
            // 
            this.txtUserComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserComment.Location = new System.Drawing.Point(14, 201);
            this.txtUserComment.Multiline = true;
            this.txtUserComment.Name = "txtUserComment";
            this.txtUserComment.Size = new System.Drawing.Size(561, 123);
            this.txtUserComment.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(500, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(419, 332);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "- 입력 사유를 입력하세요.";
            // 
            // lstEquipID
            // 
            this.lstEquipID.DataSource = null;
            this.lstEquipID.DisplayMember = "";
            this.lstEquipID.Location = new System.Drawing.Point(12, 31);
            this.lstEquipID.Name = "lstEquipID";
            this.lstEquipID.SearchText = "";
            this.lstEquipID.SearchTitle = "EQUIP ID";
            this.lstEquipID.SelectedIndex = -1;
            this.lstEquipID.SelectedItem = null;
            this.lstEquipID.SelectedValue = null;
            this.lstEquipID.Size = new System.Drawing.Size(119, 142);
            this.lstEquipID.TabIndex = 13;
            this.lstEquipID.ValueMember = "";
            this.lstEquipID.OnSelectedIndexChanged += new System.EventHandler(this.lstEquipID_OnSelectedIndexChanged);
            // 
            // pnlDataInput
            // 
            this.pnlDataInput.Controls.Add(this.txtOperator);
            this.pnlDataInput.Controls.Add(this.label4);
            this.pnlDataInput.Controls.Add(this.txtProbeCard);
            this.pnlDataInput.Controls.Add(this.label3);
            this.pnlDataInput.Controls.Add(this.txtEquipID);
            this.pnlDataInput.Controls.Add(this.label5);
            this.pnlDataInput.Controls.Add(this.label2);
            this.pnlDataInput.Location = new System.Drawing.Point(169, 31);
            this.pnlDataInput.Name = "pnlDataInput";
            this.pnlDataInput.Size = new System.Drawing.Size(325, 142);
            this.pnlDataInput.TabIndex = 14;
            // 
            // txtOperator
            // 
            this.txtOperator.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOperator.Location = new System.Drawing.Point(102, 91);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Size = new System.Drawing.Size(131, 21);
            this.txtOperator.TabIndex = 8;
            this.txtOperator.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "OPERATOR";
            // 
            // txtProbeCard
            // 
            this.txtProbeCard.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProbeCard.Location = new System.Drawing.Point(102, 64);
            this.txtProbeCard.Name = "txtProbeCard";
            this.txtProbeCard.Size = new System.Drawing.Size(205, 21);
            this.txtProbeCard.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "PROBE CARD";
            // 
            // txtEquipID
            // 
            this.txtEquipID.Location = new System.Drawing.Point(102, 38);
            this.txtEquipID.Name = "txtEquipID";
            this.txtEquipID.ReadOnly = true;
            this.txtEquipID.Size = new System.Drawing.Size(131, 21);
            this.txtEquipID.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Data Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "EQUIP ID";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(10, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(38, 12);
            this.lblDescription.TabIndex = 15;
            this.lblDescription.Text = "label1";
            // 
            // frmTestDataUpload_Pcm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(587, 363);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.pnlDataInput);
            this.Controls.Add(this.lstEquipID);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtUserComment);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTestDataUpload_Pcm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PCM Data File Upload";
            this.Load += new System.EventHandler(this.frmTestDataUpload_Pcm_Load);
            this.pnlDataInput.ResumeLayout(false);
            this.pnlDataInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUserComment;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private Framework.Controls.DUCListBox lstEquipID;
        private System.Windows.Forms.Panel pnlDataInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProbeCard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEquipID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtOperator;
    }
}