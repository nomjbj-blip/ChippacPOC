namespace DACrux.TEST.ENGUI
{
    partial class frmVisualInspDefine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVisualInspDefine));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.fpBin = new FarPoint.Win.Spread.FpSpread();
            this.fpBin_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.butUpdate = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSpecType = new System.Windows.Forms.ListBox();
            this.txtVIType = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpBin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpBin_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstSpecType);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 395);
            this.panel1.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.LightSlateGray;
            this.label15.Dock = System.Windows.Forms.DockStyle.Top;
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Image = ((System.Drawing.Image)(resources.GetObject("label15.Image")));
            this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(264, 20);
            this.label15.TabIndex = 17;
            this.label15.Text = "    Visual Inspection Spec Type";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.butClose);
            this.panel2.Controls.Add(this.butUpdate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 395);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(787, 45);
            this.panel2.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(264, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 395);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // fpBin
            // 
            this.fpBin.AccessibleDescription = "";
            this.fpBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpBin.Location = new System.Drawing.Point(267, 33);
            this.fpBin.Name = "fpBin";
            this.fpBin.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpBin_Sheet1});
            this.fpBin.Size = new System.Drawing.Size(520, 362);
            this.fpBin.TabIndex = 4;
            // 
            // fpBin_Sheet1
            // 
            this.fpBin_Sheet1.Reset();
            fpBin_Sheet1.SheetName = "Sheet1";
            // 
            // butUpdate
            // 
            this.butUpdate.Location = new System.Drawing.Point(619, 6);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 29);
            this.butUpdate.TabIndex = 0;
            this.butUpdate.Text = "Update";
            this.butUpdate.UseVisualStyleBackColor = true;
            this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(700, 6);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 29);
            this.butClose.TabIndex = 0;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtVIType);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(267, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(520, 33);
            this.panel3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inspection Type :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lstSpecType
            // 
            this.lstSpecType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSpecType.FormattingEnabled = true;
            this.lstSpecType.ItemHeight = 12;
            this.lstSpecType.Items.AddRange(new object[] {
            "IQC",
            "AVI-QC",
            "PROBE-QC",
            "FINAL-QC"});
            this.lstSpecType.Location = new System.Drawing.Point(0, 20);
            this.lstSpecType.Name = "lstSpecType";
            this.lstSpecType.Size = new System.Drawing.Size(264, 375);
            this.lstSpecType.TabIndex = 18;
            this.lstSpecType.SelectedIndexChanged += new System.EventHandler(this.lstSpecType_SelectedIndexChanged);
            // 
            // txtVIType
            // 
            this.txtVIType.Location = new System.Drawing.Point(131, 5);
            this.txtVIType.Name = "txtVIType";
            this.txtVIType.ReadOnly = true;
            this.txtVIType.Size = new System.Drawing.Size(150, 21);
            this.txtVIType.TabIndex = 1;
            // 
            // frmVisualInspDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 440);
            this.Controls.Add(this.fpBin);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmVisualInspDefine";
            this.Text = "Visual Inspection Define";
            this.Load += new System.EventHandler(this.frmVisualInspDefine_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpBin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpBin_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListBox lstSpecType;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.Splitter splitter1;
        private FarPoint.Win.Spread.FpSpread fpBin;
        private FarPoint.Win.Spread.SheetView fpBin_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtVIType;
        private System.Windows.Forms.Label label1;
    }
}