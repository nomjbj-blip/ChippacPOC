namespace DACrux.TEST.ENGUI
{
    partial class frmStandardReport
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStandardReport));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.numMargin = new System.Windows.Forms.NumericUpDown();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dlbLot = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProgram = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.dlbTestArea = new DACrux.Framework.Controls.DUCListBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.fpsSummary = new FarPoint.Win.Spread.FpSpread();
            this.fpsSummary_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsSummary_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.dtEnd);
            this.ultraPanel1.ClientArea.Controls.Add(this.dtStart);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel3);
            this.ultraPanel1.ClientArea.Controls.Add(this.numMargin);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnView);
            this.ultraPanel1.ClientArea.Controls.Add(this.dlbLot);
            this.ultraPanel1.ClientArea.Controls.Add(this.dlbProgram);
            this.ultraPanel1.ClientArea.Controls.Add(this.dlbProduct);
            this.ultraPanel1.ClientArea.Controls.Add(this.dlbTestArea);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1118, 137);
            this.ultraPanel1.TabIndex = 0;
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(102, 51);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(130, 21);
            this.dtEnd.TabIndex = 18;
            this.dtEnd.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(102, 24);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(130, 21);
            this.dtStart.TabIndex = 17;
            this.dtStart.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // ultraLabel3
            // 
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Location = new System.Drawing.Point(3, 108);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 11;
            this.ultraLabel3.Text = "BoxPlot Zoom";
            // 
            // numMargin
            // 
            this.numMargin.Location = new System.Drawing.Point(112, 110);
            this.numMargin.Name = "numMargin";
            this.numMargin.Size = new System.Drawing.Size(120, 21);
            this.numMargin.TabIndex = 10;
            this.numMargin.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMargin.ValueChanged += new System.EventHandler(this.numMargin_ValueChanged);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.Image = "search.gif";
            this.btnView.Appearance = appearance2;
            this.btnView.ImageList = this.imageList1;
            this.btnView.Location = new System.Drawing.Point(973, 13);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(133, 48);
            this.btnView.TabIndex = 8;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "data.png");
            this.imageList1.Images.SetKeyName(1, "find.png");
            this.imageList1.Images.SetKeyName(2, "search.gif");
            this.imageList1.Images.SetKeyName(3, "search.png");
            // 
            // dlbLot
            // 
            this.dlbLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbLot.DataSource = null;
            this.dlbLot.DisplayMember = "";
            this.dlbLot.Location = new System.Drawing.Point(802, 3);
            this.dlbLot.Name = "dlbLot";
            this.dlbLot.SearchText = "";
            this.dlbLot.SearchTitle = "LOT ID";
            this.dlbLot.SelectedIndex = -1;
            this.dlbLot.SelectedItem = null;
            this.dlbLot.SelectedValue = null;
            this.dlbLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLot.Size = new System.Drawing.Size(161, 131);
            this.dlbLot.TabIndex = 7;
            this.dlbLot.ValueMember = "";
            this.dlbLot.OnSelectedIndexChanged += new System.EventHandler(this.dlbLot_OnSelectedIndexChanged);
            // 
            // dlbProgram
            // 
            this.dlbProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbProgram.DataSource = null;
            this.dlbProgram.DisplayMember = "";
            this.dlbProgram.Location = new System.Drawing.Point(552, 3);
            this.dlbProgram.Name = "dlbProgram";
            this.dlbProgram.SearchText = "";
            this.dlbProgram.SearchTitle = "PROGRAM";
            this.dlbProgram.SelectedIndex = -1;
            this.dlbProgram.SelectedItem = null;
            this.dlbProgram.SelectedValue = null;
            this.dlbProgram.Size = new System.Drawing.Size(244, 131);
            this.dlbProgram.TabIndex = 9;
            this.dlbProgram.ValueMember = "";
            this.dlbProgram.OnSelectedIndexChanged += new System.EventHandler(this.dlbProgram_OnSelectedIndexChanged);
            // 
            // dlbProduct
            // 
            this.dlbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Location = new System.Drawing.Point(395, 3);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "PRODUCT";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.Size = new System.Drawing.Size(151, 131);
            this.dlbProduct.TabIndex = 5;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedIndexChanged += new System.EventHandler(this.dlbProduct_OnSelectedIndexChanged);
            // 
            // dlbTestArea
            // 
            this.dlbTestArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbTestArea.DataSource = null;
            this.dlbTestArea.DisplayMember = "";
            this.dlbTestArea.Location = new System.Drawing.Point(238, 3);
            this.dlbTestArea.Name = "dlbTestArea";
            this.dlbTestArea.SearchText = "";
            this.dlbTestArea.SearchTitle = "TESTAREA";
            this.dlbTestArea.SelectedIndex = -1;
            this.dlbTestArea.SelectedItem = null;
            this.dlbTestArea.SelectedValue = null;
            this.dlbTestArea.Size = new System.Drawing.Size(151, 131);
            this.dlbTestArea.TabIndex = 4;
            this.dlbTestArea.ValueMember = "";
            this.dlbTestArea.OnSelectedIndexChanged += new System.EventHandler(this.dlbTestArea_OnSelectedIndexChanged);
            // 
            // ultraLabel2
            // 
            appearance3.Image = "data.png";
            appearance3.TextHAlignAsString = "Right";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.ImageList = this.imageList1;
            this.ultraLabel2.Location = new System.Drawing.Point(3, 51);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(79, 23);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "END";
            // 
            // ultraLabel1
            // 
            appearance4.Image = "data.png";
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance4;
            this.ultraLabel1.ImageList = this.imageList1;
            this.ultraLabel1.Location = new System.Drawing.Point(3, 26);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(79, 23);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "START";
            // 
            // fpsSummary
            // 
            this.fpsSummary.AccessibleDescription = "";
            this.fpsSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsSummary.Location = new System.Drawing.Point(0, 137);
            this.fpsSummary.Name = "fpsSummary";
            this.fpsSummary.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsSummary_Sheet1});
            this.fpsSummary.Size = new System.Drawing.Size(1118, 516);
            this.fpsSummary.TabIndex = 1;
            this.fpsSummary.RowHeightChanged += new FarPoint.Win.Spread.RowHeightChangedEventHandler(this.fpsSummary_RowHeightChanged);
            this.fpsSummary.ColumnWidthChanged += new FarPoint.Win.Spread.ColumnWidthChangedEventHandler(this.fpsSummary_ColumnWidthChanged);
            // 
            // fpsSummary_Sheet1
            // 
            this.fpsSummary_Sheet1.Reset();
            fpsSummary_Sheet1.SheetName = "Sheet1";
            // 
            // frmStandardReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 653);
            this.Controls.Add(this.fpsSummary);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "frmStandardReport";
            this.Text = "Standard Report";
            this.Load += new System.EventHandler(this.frmStandardReport_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsSummary_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Framework.Controls.DUCListBox dlbTestArea;
        private System.Windows.Forms.ImageList imageList1;
        private Framework.Controls.DUCListBox dlbProduct;
        private Infragistics.Win.Misc.UltraButton btnView;
        private FarPoint.Win.Spread.FpSpread fpsSummary;
        private FarPoint.Win.Spread.SheetView fpsSummary_Sheet1;
        private Framework.Controls.DUCListBox dlbLot;
        private Framework.Controls.DUCListBox dlbProgram;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private System.Windows.Forms.NumericUpDown numMargin;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
    }
}