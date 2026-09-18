namespace DACrux.SEMDMS.ENGUI
{
    partial class frmInspInfoReport
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbWafer = new System.Windows.Forms.RadioButton();
            this.rbLot = new System.Windows.Forms.RadioButton();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbInspection = new System.Windows.Forms.RadioButton();
            this.rbReview = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.toDate);
            this.panel1.Controls.Add(this.fromDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 63);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(681, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 50);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbWafer);
            this.groupBox1.Controls.Add(this.rbLot);
            this.groupBox1.Location = new System.Drawing.Point(217, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 54);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // rbWafer
            // 
            this.rbWafer.AutoSize = true;
            this.rbWafer.Checked = true;
            this.rbWafer.Location = new System.Drawing.Point(62, 20);
            this.rbWafer.Name = "rbWafer";
            this.rbWafer.Size = new System.Drawing.Size(54, 16);
            this.rbWafer.TabIndex = 1;
            this.rbWafer.TabStop = true;
            this.rbWafer.Text = "Wafer";
            this.rbWafer.UseVisualStyleBackColor = true;
            this.rbWafer.CheckedChanged += new System.EventHandler(this.rbOption_CheckedChanged);
            // 
            // rbLot
            // 
            this.rbLot.AutoSize = true;
            this.rbLot.Location = new System.Drawing.Point(6, 20);
            this.rbLot.Name = "rbLot";
            this.rbLot.Size = new System.Drawing.Size(40, 16);
            this.rbLot.TabIndex = 0;
            this.rbLot.Text = "Lot";
            this.rbLot.UseVisualStyleBackColor = true;
            this.rbLot.CheckedChanged += new System.EventHandler(this.rbOption_CheckedChanged);
            // 
            // toDate
            // 
            this.toDate.CustomFormat = "yyyy-MM-dd";
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDate.Location = new System.Drawing.Point(81, 36);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(130, 21);
            this.toDate.TabIndex = 17;
            // 
            // fromDate
            // 
            this.fromDate.CustomFormat = "yyyy-MM-dd";
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDate.Location = new System.Drawing.Point(81, 9);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(130, 21);
            this.fromDate.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "To Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "From Date";
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 63);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(784, 498);
            this.fpSpread1.TabIndex = 1;
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellDoubleClick);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbReview);
            this.groupBox2.Controls.Add(this.rbInspection);
            this.groupBox2.Location = new System.Drawing.Point(358, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 54);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Area";
            // 
            // rbInspection
            // 
            this.rbInspection.AutoSize = true;
            this.rbInspection.Checked = true;
            this.rbInspection.Location = new System.Drawing.Point(6, 20);
            this.rbInspection.Name = "rbInspection";
            this.rbInspection.Size = new System.Drawing.Size(81, 16);
            this.rbInspection.TabIndex = 1;
            this.rbInspection.TabStop = true;
            this.rbInspection.Tag = "Inspection";
            this.rbInspection.Text = "Inspection";
            this.rbInspection.UseVisualStyleBackColor = true;
            this.rbInspection.CheckedChanged += new System.EventHandler(this.rbOption_CheckedChanged);
            // 
            // rbReview
            // 
            this.rbReview.AutoSize = true;
            this.rbReview.Location = new System.Drawing.Point(93, 20);
            this.rbReview.Name = "rbReview";
            this.rbReview.Size = new System.Drawing.Size(64, 16);
            this.rbReview.TabIndex = 2;
            this.rbReview.Tag = "Review";
            this.rbReview.Text = "Review";
            this.rbReview.UseVisualStyleBackColor = true;
            this.rbReview.CheckedChanged += new System.EventHandler(this.rbOption_CheckedChanged);
            // 
            // frmInspInfoReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.panel1);
            this.Name = "frmInspInfoReport";
            this.Text = "Inspection Wafer";
            this.Load += new System.EventHandler(this.frmInspInfoReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbWafer;
        private System.Windows.Forms.RadioButton rbLot;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbReview;
        private System.Windows.Forms.RadioButton rbInspection;
    }
}