namespace DACrux.SEMDMS.ENGUI
{
    partial class frmKillRate
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.dlbDefectClass = new DACrux.Framework.Controls.DUCListBox();
            this.dlbWafer = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLot = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLayer = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.decimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.chkNormalized = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dlbDefectClass);
            this.panel1.Controls.Add(this.dlbWafer);
            this.panel1.Controls.Add(this.dlbLot);
            this.panel1.Controls.Add(this.dlbLayer);
            this.panel1.Controls.Add(this.dlbProduct);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1032, 142);
            this.panel1.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.Location = new System.Drawing.Point(950, 12);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dlbDefectClass
            // 
            this.dlbDefectClass.DataSource = null;
            this.dlbDefectClass.DisplayMember = "";
            this.dlbDefectClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbDefectClass.Location = new System.Drawing.Point(788, 0);
            this.dlbDefectClass.Name = "dlbDefectClass";
            this.dlbDefectClass.SearchText = "";
            this.dlbDefectClass.SearchTitle = "Defect Class";
            this.dlbDefectClass.SelectedIndex = -1;
            this.dlbDefectClass.SelectedItem = null;
            this.dlbDefectClass.SelectedValue = null;
            this.dlbDefectClass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbDefectClass.Size = new System.Drawing.Size(150, 142);
            this.dlbDefectClass.TabIndex = 5;
            this.dlbDefectClass.ValueMember = "";
            // 
            // dlbWafer
            // 
            this.dlbWafer.DataSource = null;
            this.dlbWafer.DisplayMember = "";
            this.dlbWafer.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbWafer.Location = new System.Drawing.Point(638, 0);
            this.dlbWafer.Name = "dlbWafer";
            this.dlbWafer.SearchText = "";
            this.dlbWafer.SearchTitle = "Wafer";
            this.dlbWafer.SelectedIndex = -1;
            this.dlbWafer.SelectedItem = null;
            this.dlbWafer.SelectedValue = null;
            this.dlbWafer.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbWafer.Size = new System.Drawing.Size(150, 142);
            this.dlbWafer.TabIndex = 4;
            this.dlbWafer.ValueMember = "";
            this.dlbWafer.OnSelectedIndexChanged += new System.EventHandler(this.dlbWafer_OnSelectedIndexChanged);
            // 
            // dlbLot
            // 
            this.dlbLot.DataSource = null;
            this.dlbLot.DisplayMember = "";
            this.dlbLot.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbLot.Location = new System.Drawing.Point(488, 0);
            this.dlbLot.Name = "dlbLot";
            this.dlbLot.SearchText = "";
            this.dlbLot.SearchTitle = "Lot";
            this.dlbLot.SelectedIndex = -1;
            this.dlbLot.SelectedItem = null;
            this.dlbLot.SelectedValue = null;
            this.dlbLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLot.Size = new System.Drawing.Size(150, 142);
            this.dlbLot.TabIndex = 3;
            this.dlbLot.ValueMember = "";
            this.dlbLot.OnSelectedIndexChanged += new System.EventHandler(this.dlbLot_OnSelectedIndexChanged);
            this.dlbLot.EnterTextBox += new System.EventHandler(this.dlbLot_EnterTextBox);
            // 
            // dlbLayer
            // 
            this.dlbLayer.DataSource = null;
            this.dlbLayer.DisplayMember = "";
            this.dlbLayer.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbLayer.Location = new System.Drawing.Point(338, 0);
            this.dlbLayer.Name = "dlbLayer";
            this.dlbLayer.SearchText = "";
            this.dlbLayer.SearchTitle = "Layer";
            this.dlbLayer.SelectedIndex = -1;
            this.dlbLayer.SelectedItem = null;
            this.dlbLayer.SelectedValue = null;
            this.dlbLayer.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLayer.Size = new System.Drawing.Size(150, 142);
            this.dlbLayer.TabIndex = 2;
            this.dlbLayer.ValueMember = "";
            this.dlbLayer.OnSelectedIndexChanged += new System.EventHandler(this.dlbLayer_OnSelectedIndexChanged);
            // 
            // dlbProduct
            // 
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbProduct.Location = new System.Drawing.Point(188, 0);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "Device";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.Size = new System.Drawing.Size(150, 142);
            this.dlbProduct.TabIndex = 1;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedIndexChanged += new System.EventHandler(this.dlbProduct_OnSelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkNormalized);
            this.groupBox1.Controls.Add(this.decimalPlaces);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.toDate);
            this.groupBox1.Controls.Add(this.fromDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 142);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // decimalPlaces
            // 
            this.decimalPlaces.Location = new System.Drawing.Point(106, 115);
            this.decimalPlaces.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.decimalPlaces.Name = "decimalPlaces";
            this.decimalPlaces.Size = new System.Drawing.Size(76, 21);
            this.decimalPlaces.TabIndex = 25;
            this.decimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.decimalPlaces.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.decimalPlaces.ValueChanged += new System.EventHandler(this.decimalPlaces_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "Decimal Places";
            // 
            // toDate
            // 
            this.toDate.CustomFormat = "yyyy-MM-dd";
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDate.Location = new System.Drawing.Point(52, 34);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(130, 21);
            this.toDate.TabIndex = 23;
            this.toDate.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // fromDate
            // 
            this.fromDate.CustomFormat = "yyyy-MM-dd";
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDate.Location = new System.Drawing.Point(52, 12);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(130, 21);
            this.fromDate.TabIndex = 22;
            this.fromDate.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 142);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1032, 384);
            this.fpSpread1.TabIndex = 1;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 142);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 115;
            this.ultraSplitter1.Size = new System.Drawing.Size(1032, 6);
            this.ultraSplitter1.TabIndex = 25;
            // 
            // chkNormalized
            // 
            this.chkNormalized.AutoSize = true;
            this.chkNormalized.Location = new System.Drawing.Point(52, 61);
            this.chkNormalized.Name = "chkNormalized";
            this.chkNormalized.Size = new System.Drawing.Size(89, 16);
            this.chkNormalized.TabIndex = 26;
            this.chkNormalized.Text = "Normalized";
            this.chkNormalized.UseVisualStyleBackColor = true;
            // 
            // frmKillRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 526);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.panel1);
            this.Name = "frmKillRate";
            this.Text = "frmKillRate";
            this.Load += new System.EventHandler(this.frmKillRate_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private Framework.Controls.DUCListBox dlbDefectClass;
        private Framework.Controls.DUCListBox dlbWafer;
        private Framework.Controls.DUCListBox dlbLot;
        private Framework.Controls.DUCListBox dlbLayer;
        private Framework.Controls.DUCListBox dlbProduct;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DateTimePicker fromDate;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.NumericUpDown decimalPlaces;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkNormalized;
    }
}