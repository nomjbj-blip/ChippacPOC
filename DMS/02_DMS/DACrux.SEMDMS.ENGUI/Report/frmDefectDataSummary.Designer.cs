namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectDataSummary
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectDataSummary));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.decimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.grbSize = new System.Windows.Forms.GroupBox();
            this.btnSize = new System.Windows.Forms.Button();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.txtMininum = new System.Windows.Forms.TextBox();
            this.txtMaxinum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grbNormalize = new System.Windows.Forms.GroupBox();
            this.rbWithoutBin = new System.Windows.Forms.RadioButton();
            this.rbWithBin = new System.Windows.Forms.RadioButton();
            this.chkNormalize = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFunc = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSecBreakdown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBreakDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYAxis = new System.Windows.Forms.ComboBox();
            this.grbWafer = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnAll = new System.Windows.Forms.Button();
            this.lsvSelectedItems = new System.Windows.Forms.ListView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.lsvAllItems = new System.Windows.Forms.ListView();
            this.btnRightToLeft = new System.Windows.Forms.Button();
            this.btnLeftToRight = new System.Windows.Forms.Button();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).BeginInit();
            this.grbSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.grbNormalize.SuspendLayout();
            this.grbWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.decimalPlaces);
            this.panel1.Controls.Add(this.grbSize);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.grbNormalize);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbFunc);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbSecBreakdown);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbBreakDown);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbYAxis);
            this.panel1.Controls.Add(this.grbWafer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1066, 144);
            this.panel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(767, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "Decimal Length";
            // 
            // decimalPlaces
            // 
            this.decimalPlaces.Location = new System.Drawing.Point(866, 112);
            this.decimalPlaces.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.decimalPlaces.Name = "decimalPlaces";
            this.decimalPlaces.Size = new System.Drawing.Size(46, 21);
            this.decimalPlaces.TabIndex = 17;
            this.decimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.decimalPlaces.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.decimalPlaces.ValueChanged += new System.EventHandler(this.decimalPlaces_ValueChanged);
            // 
            // grbSize
            // 
            this.grbSize.Controls.Add(this.btnSize);
            this.grbSize.Controls.Add(this.nudInterval);
            this.grbSize.Controls.Add(this.txtMininum);
            this.grbSize.Controls.Add(this.txtMaxinum);
            this.grbSize.Controls.Add(this.label7);
            this.grbSize.Controls.Add(this.label6);
            this.grbSize.Controls.Add(this.label5);
            this.grbSize.Location = new System.Drawing.Point(761, 6);
            this.grbSize.Name = "grbSize";
            this.grbSize.Size = new System.Drawing.Size(155, 101);
            this.grbSize.TabIndex = 12;
            this.grbSize.TabStop = false;
            this.grbSize.Text = "Breakdown Size";
            // 
            // btnSize
            // 
            this.btnSize.Location = new System.Drawing.Point(129, 68);
            this.btnSize.Name = "btnSize";
            this.btnSize.Size = new System.Drawing.Size(22, 23);
            this.btnSize.TabIndex = 6;
            this.btnSize.UseVisualStyleBackColor = true;
            this.btnSize.Click += new System.EventHandler(this.btnSize_Click);
            // 
            // nudInterval
            // 
            this.nudInterval.Location = new System.Drawing.Point(70, 68);
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(53, 21);
            this.nudInterval.TabIndex = 5;
            // 
            // txtMininum
            // 
            this.txtMininum.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtMininum.Location = new System.Drawing.Point(70, 41);
            this.txtMininum.Name = "txtMininum";
            this.txtMininum.Size = new System.Drawing.Size(81, 21);
            this.txtMininum.TabIndex = 4;
            this.txtMininum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMininum_KeyPress);
            // 
            // txtMaxinum
            // 
            this.txtMaxinum.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtMaxinum.Location = new System.Drawing.Point(70, 14);
            this.txtMaxinum.Name = "txtMaxinum";
            this.txtMaxinum.Size = new System.Drawing.Size(81, 21);
            this.txtMaxinum.TabIndex = 3;
            this.txtMaxinum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxinum_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "Interval";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Mininum";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Maxinum";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(918, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(145, 135);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grbNormalize
            // 
            this.grbNormalize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbNormalize.Controls.Add(this.rbWithoutBin);
            this.grbNormalize.Controls.Add(this.rbWithBin);
            this.grbNormalize.Controls.Add(this.chkNormalize);
            this.grbNormalize.Location = new System.Drawing.Point(389, 99);
            this.grbNormalize.Name = "grbNormalize";
            this.grbNormalize.Size = new System.Drawing.Size(366, 42);
            this.grbNormalize.TabIndex = 10;
            this.grbNormalize.TabStop = false;
            // 
            // rbWithoutBin
            // 
            this.rbWithoutBin.AutoSize = true;
            this.rbWithoutBin.Enabled = false;
            this.rbWithoutBin.Location = new System.Drawing.Point(232, 17);
            this.rbWithoutBin.Name = "rbWithoutBin";
            this.rbWithoutBin.Size = new System.Drawing.Size(85, 16);
            this.rbWithoutBin.TabIndex = 2;
            this.rbWithoutBin.Text = "Without Bin";
            this.rbWithoutBin.UseVisualStyleBackColor = true;
            // 
            // rbWithBin
            // 
            this.rbWithBin.AutoSize = true;
            this.rbWithBin.Checked = true;
            this.rbWithBin.Enabled = false;
            this.rbWithBin.Location = new System.Drawing.Point(147, 17);
            this.rbWithBin.Name = "rbWithBin";
            this.rbWithBin.Size = new System.Drawing.Size(68, 16);
            this.rbWithBin.TabIndex = 1;
            this.rbWithBin.TabStop = true;
            this.rbWithBin.Text = "With Bin";
            this.rbWithBin.UseVisualStyleBackColor = true;
            // 
            // chkNormalize
            // 
            this.chkNormalize.AutoSize = true;
            this.chkNormalize.Location = new System.Drawing.Point(6, 17);
            this.chkNormalize.Name = "chkNormalize";
            this.chkNormalize.Size = new System.Drawing.Size(82, 16);
            this.chkNormalize.TabIndex = 0;
            this.chkNormalize.Text = "Normalize";
            this.chkNormalize.UseVisualStyleBackColor = true;
            this.chkNormalize.CheckedChanged += new System.EventHandler(this.chkNormalize_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(389, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Function";
            // 
            // cmbFunc
            // 
            this.cmbFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFunc.FormattingEnabled = true;
            this.cmbFunc.Location = new System.Drawing.Point(389, 73);
            this.cmbFunc.Name = "cmbFunc";
            this.cmbFunc.Size = new System.Drawing.Size(180, 20);
            this.cmbFunc.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(573, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Secondary Breakdown";
            this.label3.Visible = false;
            // 
            // cmbSecBreakdown
            // 
            this.cmbSecBreakdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecBreakdown.FormattingEnabled = true;
            this.cmbSecBreakdown.Location = new System.Drawing.Point(575, 73);
            this.cmbSecBreakdown.Name = "cmbSecBreakdown";
            this.cmbSecBreakdown.Size = new System.Drawing.Size(180, 20);
            this.cmbSecBreakdown.TabIndex = 6;
            this.cmbSecBreakdown.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(573, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Breakdown";
            // 
            // cmbBreakDown
            // 
            this.cmbBreakDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBreakDown.FormattingEnabled = true;
            this.cmbBreakDown.Location = new System.Drawing.Point(575, 28);
            this.cmbBreakDown.Name = "cmbBreakDown";
            this.cmbBreakDown.Size = new System.Drawing.Size(180, 20);
            this.cmbBreakDown.TabIndex = 4;
            this.cmbBreakDown.SelectedIndexChanged += new System.EventHandler(this.cmbBreakDown_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(389, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Y Axis";
            // 
            // cmbYAxis
            // 
            this.cmbYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYAxis.FormattingEnabled = true;
            this.cmbYAxis.Location = new System.Drawing.Point(389, 28);
            this.cmbYAxis.Name = "cmbYAxis";
            this.cmbYAxis.Size = new System.Drawing.Size(180, 20);
            this.cmbYAxis.TabIndex = 1;
            this.cmbYAxis.SelectedIndexChanged += new System.EventHandler(this.cmbYAxis_SelectedIndexChanged);
            // 
            // grbWafer
            // 
            this.grbWafer.Controls.Add(this.btnRemove);
            this.grbWafer.Controls.Add(this.btnAll);
            this.grbWafer.Controls.Add(this.lsvSelectedItems);
            this.grbWafer.Controls.Add(this.lsvAllItems);
            this.grbWafer.Controls.Add(this.btnRightToLeft);
            this.grbWafer.Controls.Add(this.btnLeftToRight);
            this.grbWafer.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbWafer.Location = new System.Drawing.Point(0, 0);
            this.grbWafer.Name = "grbWafer";
            this.grbWafer.Size = new System.Drawing.Size(383, 144);
            this.grbWafer.TabIndex = 0;
            this.grbWafer.TabStop = false;
            this.grbWafer.Text = "Inspected Wafer";
            // 
            // btnRemove
            // 
            this.btnRemove.ImageList = this.imageList1;
            this.btnRemove.Location = new System.Drawing.Point(217, 20);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(160, 28);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "<< REMOVE";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow_back.gif");
            this.imageList1.Images.SetKeyName(1, "arrow_back.png");
            this.imageList1.Images.SetKeyName(2, "arrow_down.gif");
            this.imageList1.Images.SetKeyName(3, "arrow_down.png");
            this.imageList1.Images.SetKeyName(4, "arrow_next.gif");
            this.imageList1.Images.SetKeyName(5, "arrow_next.png");
            this.imageList1.Images.SetKeyName(6, "arrow_top.gif");
            this.imageList1.Images.SetKeyName(7, "arrow_top.png");
            // 
            // btnAll
            // 
            this.btnAll.ImageList = this.imageList1;
            this.btnAll.Location = new System.Drawing.Point(6, 20);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(160, 28);
            this.btnAll.TabIndex = 5;
            this.btnAll.Text = "ALL >>";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // lsvSelectedItems
            // 
            this.lsvSelectedItems.AllowColumnReorder = true;
            this.lsvSelectedItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsvSelectedItems.GridLines = true;
            this.lsvSelectedItems.HideSelection = false;
            this.lsvSelectedItems.Location = new System.Drawing.Point(217, 54);
            this.lsvSelectedItems.Name = "lsvSelectedItems";
            this.lsvSelectedItems.Size = new System.Drawing.Size(160, 90);
            this.lsvSelectedItems.SmallImageList = this.imageList2;
            this.lsvSelectedItems.TabIndex = 4;
            this.lsvSelectedItems.TileSize = new System.Drawing.Size(128, 13);
            this.lsvSelectedItems.UseCompatibleStateImageBehavior = false;
            this.lsvSelectedItems.View = System.Windows.Forms.View.Details;
            this.lsvSelectedItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvItems_MouseDoubleClick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "marker.png");
            // 
            // lsvAllItems
            // 
            this.lsvAllItems.AllowColumnReorder = true;
            this.lsvAllItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsvAllItems.FullRowSelect = true;
            this.lsvAllItems.GridLines = true;
            this.lsvAllItems.HideSelection = false;
            this.lsvAllItems.LabelEdit = true;
            this.lsvAllItems.Location = new System.Drawing.Point(6, 54);
            this.lsvAllItems.Name = "lsvAllItems";
            this.lsvAllItems.Size = new System.Drawing.Size(160, 90);
            this.lsvAllItems.SmallImageList = this.imageList2;
            this.lsvAllItems.TabIndex = 3;
            this.lsvAllItems.TileSize = new System.Drawing.Size(128, 13);
            this.lsvAllItems.UseCompatibleStateImageBehavior = false;
            this.lsvAllItems.View = System.Windows.Forms.View.Details;
            this.lsvAllItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvItems_MouseDoubleClick);
            // 
            // btnRightToLeft
            // 
            this.btnRightToLeft.ImageIndex = 0;
            this.btnRightToLeft.ImageList = this.imageList1;
            this.btnRightToLeft.Location = new System.Drawing.Point(172, 88);
            this.btnRightToLeft.Name = "btnRightToLeft";
            this.btnRightToLeft.Size = new System.Drawing.Size(34, 28);
            this.btnRightToLeft.TabIndex = 2;
            this.btnRightToLeft.UseVisualStyleBackColor = true;
            this.btnRightToLeft.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnLeftToRight
            // 
            this.btnLeftToRight.ImageIndex = 4;
            this.btnLeftToRight.ImageList = this.imageList1;
            this.btnLeftToRight.Location = new System.Drawing.Point(172, 54);
            this.btnLeftToRight.Name = "btnLeftToRight";
            this.btnLeftToRight.Size = new System.Drawing.Size(34, 28);
            this.btnLeftToRight.TabIndex = 1;
            this.btnLeftToRight.UseVisualStyleBackColor = true;
            this.btnLeftToRight.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 144);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 181;
            this.ultraSplitter1.Size = new System.Drawing.Size(1066, 6);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 150);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1066, 376);
            this.fpSpread1.TabIndex = 2;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // frmDefectDataSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 526);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel1);
            this.Name = "frmDefectDataSummary";
            this.Text = "frmDefectDataSummary";
            this.Load += new System.EventHandler(this.frmDefectDataSummary_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).EndInit();
            this.grbSize.ResumeLayout(false);
            this.grbSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            this.grbNormalize.ResumeLayout(false);
            this.grbNormalize.PerformLayout();
            this.grbWafer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbWafer;
        private System.Windows.Forms.Button btnRightToLeft;
        private System.Windows.Forms.Button btnLeftToRight;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBreakDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYAxis;
        private System.Windows.Forms.GroupBox grbNormalize;
        private System.Windows.Forms.RadioButton rbWithoutBin;
        private System.Windows.Forms.RadioButton rbWithBin;
        private System.Windows.Forms.CheckBox chkNormalize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFunc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSecBreakdown;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lsvSelectedItems;
        private System.Windows.Forms.ListView lsvAllItems;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.GroupBox grbSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown decimalPlaces;
        private System.Windows.Forms.Button btnSize;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.TextBox txtMininum;
        private System.Windows.Forms.TextBox txtMaxinum;

    }
}