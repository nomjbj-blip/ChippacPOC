namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectAlarmSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectAlarmSetup));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnStep = new System.Windows.Forms.Button();
            this.cboStep = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnProduct = new System.Windows.Forms.Button();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDefectType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numRecalc = new System.Windows.Forms.NumericUpDown();
            this.btnRecalc = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNotifyUser = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecalc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnStep);
            this.panel2.Controls.Add(this.cboStep);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnProduct);
            this.panel2.Controls.Add(this.cboProduct);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboDefectType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.numRecalc);
            this.panel2.Controls.Add(this.btnRecalc);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnNotifyUser);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(995, 75);
            this.panel2.TabIndex = 58;
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(684, 9);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(30, 23);
            this.btnStep.TabIndex = 216;
            this.btnStep.Text = "...";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // cboStep
            // 
            this.cboStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStep.Location = new System.Drawing.Point(557, 10);
            this.cboStep.Name = "cboStep";
            this.cboStep.Size = new System.Drawing.Size(121, 20);
            this.cboStep.TabIndex = 215;
            this.cboStep.SelectedIndexChanged += new System.EventHandler(this.cboDefectType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(508, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 21);
            this.label3.TabIndex = 214;
            this.label3.Text = "   Step";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnProduct
            // 
            this.btnProduct.Location = new System.Drawing.Point(464, 8);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(30, 23);
            this.btnProduct.TabIndex = 213;
            this.btnProduct.Text = "...";
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // cboProduct
            // 
            this.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduct.Location = new System.Drawing.Point(337, 9);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 20);
            this.cboProduct.TabIndex = 212;
            this.cboProduct.SelectedIndexChanged += new System.EventHandler(this.cboDefectType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(268, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 21);
            this.label2.TabIndex = 211;
            this.label2.Text = "   Product";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDefectType
            // 
            this.cboDefectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefectType.Location = new System.Drawing.Point(109, 9);
            this.cboDefectType.Name = "cboDefectType";
            this.cboDefectType.Size = new System.Drawing.Size(143, 20);
            this.cboDefectType.TabIndex = 212;
            this.cboDefectType.SelectedIndexChanged += new System.EventHandler(this.cboDefectType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 21);
            this.label1.TabIndex = 211;
            this.label1.Text = "   Defect Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numRecalc
            // 
            this.numRecalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRecalc.Location = new System.Drawing.Point(673, 41);
            this.numRecalc.Name = "numRecalc";
            this.numRecalc.Size = new System.Drawing.Size(42, 21);
            this.numRecalc.TabIndex = 221;
            this.numRecalc.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnRecalc
            // 
            this.btnRecalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecalc.BackColor = System.Drawing.Color.White;
            this.btnRecalc.Font = new System.Drawing.Font("Arial", 9F);
            this.btnRecalc.ForeColor = System.Drawing.Color.Black;
            this.btnRecalc.Image = ((System.Drawing.Image)(resources.GetObject("btnRecalc.Image")));
            this.btnRecalc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecalc.Location = new System.Drawing.Point(728, 38);
            this.btnRecalc.Name = "btnRecalc";
            this.btnRecalc.Size = new System.Drawing.Size(157, 24);
            this.btnRecalc.TabIndex = 220;
            this.btnRecalc.Text = "      Recalc Lower/Upper";
            this.btnRecalc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecalc.UseVisualStyleBackColor = false;
            this.btnRecalc.Click += new System.EventHandler(this.btnRecalc_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(891, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 24);
            this.btnSave.TabIndex = 219;
            this.btnSave.Text = "   Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNotifyUser
            // 
            this.btnNotifyUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNotifyUser.BackColor = System.Drawing.Color.White;
            this.btnNotifyUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNotifyUser.Image = ((System.Drawing.Image)(resources.GetObject("btnNotifyUser.Image")));
            this.btnNotifyUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNotifyUser.Location = new System.Drawing.Point(728, 8);
            this.btnNotifyUser.Name = "btnNotifyUser";
            this.btnNotifyUser.Size = new System.Drawing.Size(157, 24);
            this.btnNotifyUser.TabIndex = 218;
            this.btnNotifyUser.Text = "   Notify User Setup...";
            this.btnNotifyUser.UseVisualStyleBackColor = false;
            this.btnNotifyUser.Click += new System.EventHandler(this.btnNotifyUser_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(891, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 24);
            this.btnSearch.TabIndex = 217;
            this.btnSearch.Text = "     Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(715, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 12);
            this.label4.TabIndex = 222;
            this.label4.Text = "%";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(658, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 12);
            this.label5.TabIndex = 222;
            this.label5.Text = "±";
            // 
            // fpSpread
            // 
            this.fpSpread.AccessibleDescription = "";
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.Location = new System.Drawing.Point(0, 75);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(995, 452);
            this.fpSpread.TabIndex = 59;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            fpSpread_Sheet.SheetName = "Sheet1";
            // 
            // frmDefectAlarmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 527);
            this.Controls.Add(this.fpSpread);
            this.Controls.Add(this.panel2);
            this.Name = "frmDefectAlarmSetup";
            this.Text = "DM Alarm Setup";
            this.Load += new System.EventHandler(this.frmDefectAlarmSetup_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecalc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.NumericUpDown numRecalc;
        private System.Windows.Forms.Button btnRecalc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNotifyUser;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.ComboBox cboStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDefectType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}