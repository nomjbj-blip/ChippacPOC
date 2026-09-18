namespace DACrux.SPC.Visualization
{
    partial class frmSPCChartConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSPCChartConfig));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.lstChartType = new System.Windows.Forms.ListBox();
            this.butApply = new System.Windows.Forms.Button();
            this.ucChartCfg = new DACrux.SPC.Visualization.ucChartConfig();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Azure;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 60);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(172, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Statistical Process Control Chart ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Cornsilk;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AntiqueWhite;
            this.panel2.Controls.Add(this.butApply);
            this.panel2.Controls.Add(this.butCancel);
            this.panel2.Controls.Add(this.butOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 412);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(703, 48);
            this.panel2.TabIndex = 3;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(606, 6);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(85, 36);
            this.butCancel.TabIndex = 0;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(515, 6);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(85, 36);
            this.butOk.TabIndex = 0;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // lstChartType
            // 
            this.lstChartType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstChartType.FormattingEnabled = true;
            this.lstChartType.ItemHeight = 12;
            this.lstChartType.Items.AddRange(new object[] {
            "XBAR",
            "SIGMA",
            "RANGE",
            "RAW",
            "EWMA_MV",
            "EWMA_S",
            "EWMA_R",
            "MA",
            "MS"});
            this.lstChartType.Location = new System.Drawing.Point(0, 60);
            this.lstChartType.Name = "lstChartType";
            this.lstChartType.Size = new System.Drawing.Size(170, 352);
            this.lstChartType.TabIndex = 4;
            this.lstChartType.SelectedIndexChanged += new System.EventHandler(this.lstChartType_SelectedIndexChanged);
            // 
            // butApply
            // 
            this.butApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butApply.Location = new System.Drawing.Point(415, 6);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(85, 36);
            this.butApply.TabIndex = 0;
            this.butApply.Text = "Apply";
            this.butApply.UseVisualStyleBackColor = true;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // ucChartCfg
            // 
            this.ucChartCfg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChartCfg.Enabled = false;
            this.ucChartCfg.Location = new System.Drawing.Point(170, 60);
            this.ucChartCfg.Name = "ucChartCfg";
            this.ucChartCfg.Size = new System.Drawing.Size(533, 352);
            this.ucChartCfg.TabIndex = 0;
            this.ucChartCfg.Load += new System.EventHandler(this.ucChartCfg_Load);
            // 
            // frmSPCChartConfig
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(703, 460);
            this.Controls.Add(this.ucChartCfg);
            this.Controls.Add(this.lstChartType);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSPCChartConfig";
            this.Text = "Chart Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSPCChartConfig_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ucChartConfig ucChartCfg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.ListBox lstChartType;
        private System.Windows.Forms.Button butApply;
    }
}