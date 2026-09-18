namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDataSheet
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
            this.dlbDefectClass = new DACrux.Framework.Controls.DUCListBox();
            this.dlbXItems = new DACrux.Framework.Controls.DUCListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAsc = new System.Windows.Forms.RadioButton();
            this.rbDesc = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbWaferID = new System.Windows.Forms.RadioButton();
            this.rbResulTime = new System.Windows.Forms.RadioButton();
            this.rbDefectDensity = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.nudDecimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.btnView = new System.Windows.Forms.Button();
            this.dlbWafer = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLot = new DACrux.Framework.Controls.DUCListBox();
            this.dlbStep = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkReviewOnly = new System.Windows.Forms.CheckBox();
            this.chkDensity = new System.Windows.Forms.CheckBox();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dpucDataSheetControl1 = new DACrux.SEMDMS.Control.DPUCDataSheetControl();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dlbDefectClass);
            this.panel1.Controls.Add(this.dlbXItems);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nudDecimalPlaces);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dlbWafer);
            this.panel1.Controls.Add(this.dlbLot);
            this.panel1.Controls.Add(this.dlbStep);
            this.panel1.Controls.Add(this.dlbProduct);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1215, 151);
            this.panel1.TabIndex = 0;
            // 
            // dlbDefectClass
            // 
            this.dlbDefectClass.DataSource = null;
            this.dlbDefectClass.DisplayMember = "";
            this.dlbDefectClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbDefectClass.Location = new System.Drawing.Point(785, 0);
            this.dlbDefectClass.Name = "dlbDefectClass";
            this.dlbDefectClass.SearchText = "";
            this.dlbDefectClass.SearchTitle = "Defect Class";
            this.dlbDefectClass.SelectedIndex = -1;
            this.dlbDefectClass.SelectedItem = null;
            this.dlbDefectClass.SelectedValue = null;
            this.dlbDefectClass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbDefectClass.Size = new System.Drawing.Size(165, 151);
            this.dlbDefectClass.TabIndex = 26;
            this.dlbDefectClass.ValueMember = "";
            // 
            // dlbXItems
            // 
            this.dlbXItems.DataSource = null;
            this.dlbXItems.DisplayMember = "";
            this.dlbXItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbXItems.Location = new System.Drawing.Point(628, 0);
            this.dlbXItems.Name = "dlbXItems";
            this.dlbXItems.SearchText = "";
            this.dlbXItems.SearchTitle = "X Item";
            this.dlbXItems.SelectedIndex = -1;
            this.dlbXItems.SelectedItem = null;
            this.dlbXItems.SelectedValue = null;
            this.dlbXItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbXItems.Size = new System.Drawing.Size(157, 151);
            this.dlbXItems.TabIndex = 31;
            this.dlbXItems.ValueMember = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAsc);
            this.groupBox2.Controls.Add(this.rbDesc);
            this.groupBox2.Location = new System.Drawing.Point(953, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(123, 58);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // rbAsc
            // 
            this.rbAsc.AutoSize = true;
            this.rbAsc.Checked = true;
            this.rbAsc.Location = new System.Drawing.Point(5, 10);
            this.rbAsc.Name = "rbAsc";
            this.rbAsc.Size = new System.Drawing.Size(83, 16);
            this.rbAsc.TabIndex = 27;
            this.rbAsc.TabStop = true;
            this.rbAsc.Text = "Ascending";
            this.rbAsc.UseVisualStyleBackColor = true;
            // 
            // rbDesc
            // 
            this.rbDesc.AutoSize = true;
            this.rbDesc.Location = new System.Drawing.Point(5, 32);
            this.rbDesc.Name = "rbDesc";
            this.rbDesc.Size = new System.Drawing.Size(90, 16);
            this.rbDesc.TabIndex = 28;
            this.rbDesc.Text = "Descending";
            this.rbDesc.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbWaferID);
            this.groupBox1.Controls.Add(this.rbResulTime);
            this.groupBox1.Controls.Add(this.rbDefectDensity);
            this.groupBox1.Location = new System.Drawing.Point(953, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 82);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // rbWaferID
            // 
            this.rbWaferID.AutoSize = true;
            this.rbWaferID.Checked = true;
            this.rbWaferID.Location = new System.Drawing.Point(5, 37);
            this.rbWaferID.Name = "rbWaferID";
            this.rbWaferID.Size = new System.Drawing.Size(114, 16);
            this.rbWaferID.TabIndex = 29;
            this.rbWaferID.TabStop = true;
            this.rbWaferID.Text = "Sort By Wafer ID";
            this.rbWaferID.UseVisualStyleBackColor = true;
            // 
            // rbResulTime
            // 
            this.rbResulTime.AutoSize = true;
            this.rbResulTime.Location = new System.Drawing.Point(5, 12);
            this.rbResulTime.Name = "rbResulTime";
            this.rbResulTime.Size = new System.Drawing.Size(97, 16);
            this.rbResulTime.TabIndex = 27;
            this.rbResulTime.Text = "Sort By Time";
            this.rbResulTime.UseVisualStyleBackColor = true;
            // 
            // rbDefectDensity
            // 
            this.rbDefectDensity.AutoSize = true;
            this.rbDefectDensity.Location = new System.Drawing.Point(5, 60);
            this.rbDefectDensity.Name = "rbDefectDensity";
            this.rbDefectDensity.Size = new System.Drawing.Size(104, 16);
            this.rbDefectDensity.TabIndex = 28;
            this.rbDefectDensity.Text = "Defect Density";
            this.rbDefectDensity.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1114, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "Decimal Places";
            // 
            // nudDecimalPlaces
            // 
            this.nudDecimalPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDecimalPlaces.Location = new System.Drawing.Point(1168, 127);
            this.nudDecimalPlaces.Name = "nudDecimalPlaces";
            this.nudDecimalPlaces.Size = new System.Drawing.Size(44, 21);
            this.nudDecimalPlaces.TabIndex = 24;
            this.nudDecimalPlaces.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDecimalPlaces.ValueChanged += new System.EventHandler(this.nudDecimalPlaces_ValueChanged);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.Location = new System.Drawing.Point(1116, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(96, 23);
            this.btnView.TabIndex = 23;
            this.btnView.Text = "Search";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dlbWafer
            // 
            this.dlbWafer.DataSource = null;
            this.dlbWafer.DisplayMember = "";
            this.dlbWafer.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbWafer.Location = new System.Drawing.Point(512, 0);
            this.dlbWafer.Name = "dlbWafer";
            this.dlbWafer.SearchText = "";
            this.dlbWafer.SearchTitle = "Wafer ID";
            this.dlbWafer.SelectedIndex = -1;
            this.dlbWafer.SelectedItem = null;
            this.dlbWafer.SelectedValue = null;
            this.dlbWafer.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbWafer.Size = new System.Drawing.Size(116, 151);
            this.dlbWafer.TabIndex = 22;
            this.dlbWafer.ValueMember = "";
            this.dlbWafer.OnSelectedIndexChanged += new System.EventHandler(this.dlbWafer_OnSelectedIndexChanged);
            // 
            // dlbLot
            // 
            this.dlbLot.DataSource = null;
            this.dlbLot.DisplayMember = "";
            this.dlbLot.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbLot.Location = new System.Drawing.Point(414, 0);
            this.dlbLot.Name = "dlbLot";
            this.dlbLot.SearchText = "";
            this.dlbLot.SearchTitle = "Lot ID";
            this.dlbLot.SelectedIndex = -1;
            this.dlbLot.SelectedItem = null;
            this.dlbLot.SelectedValue = null;
            this.dlbLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLot.Size = new System.Drawing.Size(98, 151);
            this.dlbLot.TabIndex = 21;
            this.dlbLot.ValueMember = "";
            this.dlbLot.OnSelectedIndexChanged += new System.EventHandler(this.dlbLot_OnSelectedIndexChanged);
            // 
            // dlbStep
            // 
            this.dlbStep.DataSource = null;
            this.dlbStep.DisplayMember = "";
            this.dlbStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbStep.Location = new System.Drawing.Point(312, 0);
            this.dlbStep.Name = "dlbStep";
            this.dlbStep.SearchText = "";
            this.dlbStep.SearchTitle = "Step ID";
            this.dlbStep.SelectedIndex = -1;
            this.dlbStep.SelectedItem = null;
            this.dlbStep.SelectedValue = null;
            this.dlbStep.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbStep.Size = new System.Drawing.Size(102, 151);
            this.dlbStep.TabIndex = 20;
            this.dlbStep.ValueMember = "";
            this.dlbStep.OnSelectedIndexChanged += new System.EventHandler(this.dlbStep_OnSelectedIndexChanged);
            // 
            // dlbProduct
            // 
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbProduct.Location = new System.Drawing.Point(200, 0);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "Device";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbProduct.Size = new System.Drawing.Size(112, 151);
            this.dlbProduct.TabIndex = 19;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedIndexChanged += new System.EventHandler(this.dlbProduct_OnSelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkReviewOnly);
            this.panel2.Controls.Add(this.chkDensity);
            this.panel2.Controls.Add(this.toDate);
            this.panel2.Controls.Add(this.fromDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 151);
            this.panel2.TabIndex = 18;
            // 
            // chkReviewOnly
            // 
            this.chkReviewOnly.AutoSize = true;
            this.chkReviewOnly.Location = new System.Drawing.Point(12, 74);
            this.chkReviewOnly.Name = "chkReviewOnly";
            this.chkReviewOnly.Size = new System.Drawing.Size(133, 16);
            this.chkReviewOnly.TabIndex = 25;
            this.chkReviewOnly.Text = "Review 항목만 조회";
            this.chkReviewOnly.UseVisualStyleBackColor = true;
            this.chkReviewOnly.CheckedChanged += new System.EventHandler(this.chkReview_CheckedChanged);
            // 
            // chkDensity
            // 
            this.chkDensity.AutoSize = true;
            this.chkDensity.Location = new System.Drawing.Point(12, 52);
            this.chkDensity.Name = "chkDensity";
            this.chkDensity.Size = new System.Drawing.Size(94, 16);
            this.chkDensity.TabIndex = 24;
            this.chkDensity.Text = "Density 환산";
            this.chkDensity.UseVisualStyleBackColor = true;
            // 
            // toDate
            // 
            this.toDate.CustomFormat = "yyyy-MM-dd";
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDate.Location = new System.Drawing.Point(67, 25);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(101, 21);
            this.toDate.TabIndex = 21;
            // 
            // fromDate
            // 
            this.fromDate.CustomFormat = "yyyy-MM-dd";
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDate.Location = new System.Drawing.Point(67, 3);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(101, 21);
            this.fromDate.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "End Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "Start Date";
            // 
            // dpucDataSheetControl1
            // 
            this.dpucDataSheetControl1.DataSource = null;
            this.dpucDataSheetControl1.DecimalPlaces = 0;
            this.dpucDataSheetControl1.DefaultColumnWidth = 0;
            this.dpucDataSheetControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucDataSheetControl1.Location = new System.Drawing.Point(0, 151);
            this.dpucDataSheetControl1.Name = "dpucDataSheetControl1";
            this.dpucDataSheetControl1.Size = new System.Drawing.Size(1215, 539);
            this.dpucDataSheetControl1.TabIndex = 1;
            // 
            // frmDataSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 690);
            this.Controls.Add(this.dpucDataSheetControl1);
            this.Controls.Add(this.panel1);
            this.Name = "frmDataSheet";
            this.Text = "frmDataSheet";
            this.Load += new System.EventHandler(this.frmDataSheet_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Framework.Controls.DUCListBox dlbWafer;
        private Framework.Controls.DUCListBox dlbLot;
        private Framework.Controls.DUCListBox dlbStep;
        private Framework.Controls.DUCListBox dlbProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDecimalPlaces;
        private System.Windows.Forms.RadioButton rbDefectDensity;
        private System.Windows.Forms.RadioButton rbResulTime;
        private Framework.Controls.DUCListBox dlbDefectClass;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbAsc;
        private System.Windows.Forms.RadioButton rbDesc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbWaferID;
        private System.Windows.Forms.CheckBox chkDensity;
        private Control.DPUCDataSheetControl dpucDataSheetControl1;
        private Framework.Controls.DUCListBox dlbXItems;
        private System.Windows.Forms.CheckBox chkReviewOnly;

    }
}