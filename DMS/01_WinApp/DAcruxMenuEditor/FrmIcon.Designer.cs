namespace DACruxV5
{
    partial class FrmIcon
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picIcon16 = new System.Windows.Forms.PictureBox();
            this.btnSaveAs16 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveAs32 = new System.Windows.Forms.Button();
            this.picIcon32 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rb16X16 = new System.Windows.Forms.RadioButton();
            this.rb32X32 = new System.Windows.Forms.RadioButton();
            this.btnClearImage = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon16)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon32)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picIcon16);
            this.groupBox1.Controls.Add(this.btnSaveAs16);
            this.groupBox1.Location = new System.Drawing.Point(54, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(99, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "16X16 ICON";
            // 
            // picIcon16
            // 
            this.picIcon16.BackColor = System.Drawing.SystemColors.Window;
            this.picIcon16.Location = new System.Drawing.Point(41, 32);
            this.picIcon16.Name = "picIcon16";
            this.picIcon16.Size = new System.Drawing.Size(16, 16);
            this.picIcon16.TabIndex = 0;
            this.picIcon16.TabStop = false;
            // 
            // btnSaveAs16
            // 
            this.btnSaveAs16.Location = new System.Drawing.Point(12, 69);
            this.btnSaveAs16.Name = "btnSaveAs16";
            this.btnSaveAs16.Size = new System.Drawing.Size(75, 20);
            this.btnSaveAs16.TabIndex = 6;
            this.btnSaveAs16.Text = "Save As...";
            this.btnSaveAs16.UseVisualStyleBackColor = true;
            this.btnSaveAs16.Click += new System.EventHandler(this.btnSaveAs16_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveAs32);
            this.groupBox2.Controls.Add(this.picIcon32);
            this.groupBox2.Location = new System.Drawing.Point(197, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(99, 97);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "32X32 ICON";
            // 
            // btnSaveAs32
            // 
            this.btnSaveAs32.Location = new System.Drawing.Point(12, 69);
            this.btnSaveAs32.Name = "btnSaveAs32";
            this.btnSaveAs32.Size = new System.Drawing.Size(75, 20);
            this.btnSaveAs32.TabIndex = 6;
            this.btnSaveAs32.Text = "Save As...";
            this.btnSaveAs32.UseVisualStyleBackColor = true;
            this.btnSaveAs32.Click += new System.EventHandler(this.btnSaveAs32_Click);
            // 
            // picIcon32
            // 
            this.picIcon32.BackColor = System.Drawing.SystemColors.Window;
            this.picIcon32.Location = new System.Drawing.Point(31, 24);
            this.picIcon32.Name = "picIcon32";
            this.picIcon32.Size = new System.Drawing.Size(32, 32);
            this.picIcon32.TabIndex = 0;
            this.picIcon32.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Open File";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(72, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(249, 21);
            this.textBox1.TabIndex = 2;
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(327, 34);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(29, 23);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClearImage);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 184);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 37);
            this.panel1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(281, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(200, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(74, 13);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(37, 16);
            this.rbAll.TabIndex = 5;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rb16X16
            // 
            this.rb16X16.AutoSize = true;
            this.rb16X16.Location = new System.Drawing.Point(133, 13);
            this.rb16X16.Name = "rb16X16";
            this.rb16X16.Size = new System.Drawing.Size(85, 16);
            this.rb16X16.TabIndex = 5;
            this.rb16X16.TabStop = true;
            this.rb16X16.Text = "16X16 Only";
            this.rb16X16.UseVisualStyleBackColor = true;
            // 
            // rb32X32
            // 
            this.rb32X32.AutoSize = true;
            this.rb32X32.Location = new System.Drawing.Point(234, 13);
            this.rb32X32.Name = "rb32X32";
            this.rb32X32.Size = new System.Drawing.Size(85, 16);
            this.rb32X32.TabIndex = 5;
            this.rb32X32.TabStop = true;
            this.rb32X32.Text = "32X32 Only";
            this.rb32X32.UseVisualStyleBackColor = true;
            // 
            // btnClearImage
            // 
            this.btnClearImage.Location = new System.Drawing.Point(9, 7);
            this.btnClearImage.Name = "btnClearImage";
            this.btnClearImage.Size = new System.Drawing.Size(99, 23);
            this.btnClearImage.TabIndex = 1;
            this.btnClearImage.Text = "Clear Image";
            this.btnClearImage.UseVisualStyleBackColor = true;
            this.btnClearImage.Click += new System.EventHandler(this.btnClearImage_Click);
            // 
            // FrmIcon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(368, 221);
            this.Controls.Add(this.rb32X32);
            this.Controls.Add(this.rb16X16);
            this.Controls.Add(this.rbAll);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIcon";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Icon";
            this.Load += new System.EventHandler(this.FrmIcon_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon16)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon32)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picIcon16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picIcon32;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rb16X16;
        private System.Windows.Forms.RadioButton rb32X32;
        private System.Windows.Forms.Button btnSaveAs16;
        private System.Windows.Forms.Button btnSaveAs32;
        private System.Windows.Forms.Button btnClearImage;
    }
}