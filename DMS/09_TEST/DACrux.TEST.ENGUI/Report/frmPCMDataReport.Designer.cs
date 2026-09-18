namespace DACrux.TEST.ENGUI
{
    partial class frmPCMDataReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPCMDataReport));
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.fpsReport = new FarPoint.Win.Spread.FpSpread();
            this.fpsReport_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lsParaList = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkTotalPara = new System.Windows.Forms.CheckBox();
            this.BtnAnalysis = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtItemFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fpsReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsReport_Sheet1)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            // fpsReport
            // 
            this.fpsReport.AccessibleDescription = "";
            this.fpsReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsReport.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsReport.HorizontalScrollBar.Name = "";
            enhancedScrollBarRenderer1.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer1.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer1.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsReport.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fpsReport.HorizontalScrollBar.TabIndex = 4;
            this.fpsReport.Location = new System.Drawing.Point(324, 0);
            this.fpsReport.Name = "fpsReport";
            this.fpsReport.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsReport_Sheet1});
            this.fpsReport.Size = new System.Drawing.Size(744, 486);
            this.fpsReport.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpsReport.TabIndex = 1;
            this.fpsReport.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsReport.VerticalScrollBar.Name = "";
            enhancedScrollBarRenderer2.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer2.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer2.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsReport.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fpsReport.VerticalScrollBar.TabIndex = 5;
            // 
            // fpsReport_Sheet1
            // 
            this.fpsReport_Sheet1.Reset();
            fpsReport_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsReport_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpsReport_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsReport_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsReport_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsReport_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fpsReport_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsReport_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsReport_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsReport_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fpsReport_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsReport_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fpsReport_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lsParaList);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(324, 486);
            this.panel6.TabIndex = 3;
            // 
            // lsParaList
            // 
            this.lsParaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsParaList.Enabled = false;
            this.lsParaList.FormattingEnabled = true;
            this.lsParaList.ItemHeight = 12;
            this.lsParaList.Location = new System.Drawing.Point(0, 53);
            this.lsParaList.Name = "lsParaList";
            this.lsParaList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsParaList.Size = new System.Drawing.Size(324, 397);
            this.lsParaList.TabIndex = 31;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkTotalPara);
            this.panel2.Controls.Add(this.BtnAnalysis);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 36);
            this.panel2.TabIndex = 0;
            // 
            // chkTotalPara
            // 
            this.chkTotalPara.AutoSize = true;
            this.chkTotalPara.Checked = true;
            this.chkTotalPara.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTotalPara.Location = new System.Drawing.Point(6, 9);
            this.chkTotalPara.Name = "chkTotalPara";
            this.chkTotalPara.Size = new System.Drawing.Size(82, 16);
            this.chkTotalPara.TabIndex = 1;
            this.chkTotalPara.Text = "Total Para";
            this.chkTotalPara.UseVisualStyleBackColor = true;
            this.chkTotalPara.CheckedChanged += new System.EventHandler(this.chkTotalPara_CheckedChanged);
            // 
            // BtnAnalysis
            // 
            this.BtnAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAnalysis.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BtnAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("BtnAnalysis.Image")));
            this.BtnAnalysis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAnalysis.Location = new System.Drawing.Point(162, 2);
            this.BtnAnalysis.Name = "BtnAnalysis";
            this.BtnAnalysis.Size = new System.Drawing.Size(154, 29);
            this.BtnAnalysis.TabIndex = 0;
            this.BtnAnalysis.Text = "View";
            this.BtnAnalysis.UseVisualStyleBackColor = false;
            this.BtnAnalysis.Click += new System.EventHandler(this.BtnAnalysis_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TxtItemFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 33);
            this.panel1.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Item Search";
            // 
            // TxtItemFilter
            // 
            this.TxtItemFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtItemFilter.Location = new System.Drawing.Point(108, 5);
            this.TxtItemFilter.Name = "TxtItemFilter";
            this.TxtItemFilter.Size = new System.Drawing.Size(209, 21);
            this.TxtItemFilter.TabIndex = 0;
            this.TxtItemFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtItemFilter_KeyPress);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSlateGray;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(324, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "   Test Item";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmPCMDataReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 486);
            this.Controls.Add(this.fpsReport);
            this.Controls.Add(this.panel6);
            this.Name = "frmPCMDataReport";
            this.Text = "Standard Report";
            this.Load += new System.EventHandler(this.frmPCMDataReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpsReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsReport_Sheet1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private FarPoint.Win.Spread.FpSpread fpsReport;
        private FarPoint.Win.Spread.SheetView fpsReport_Sheet1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ListBox lsParaList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkTotalPara;
        private System.Windows.Forms.Button BtnAnalysis;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtItemFilter;
        private System.Windows.Forms.Label label3;
    }
}