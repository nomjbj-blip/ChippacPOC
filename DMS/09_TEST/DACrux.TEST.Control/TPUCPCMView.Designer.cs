namespace DACrux.TEST.Control
{
    partial class TPUCPCMView
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPUCPCMView));
            this.LayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SPChart = new Infragistics.Win.Misc.UltraSplitter();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lsParaList = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkTotalPara = new System.Windows.Forms.CheckBox();
            this.BtnAnalysis = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtItemFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SPWafer = new Infragistics.Win.Misc.UltraSplitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fpRawData = new FarPoint.Win.Spread.FpSpread();
            this.fpRawData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fpGroupData = new FarPoint.Win.Spread.FpSpread();
            this.fpGroupData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData_Sheet1)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpGroupData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpGroupData_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.AutoScroll = true;
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 316);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(1299, 395);
            this.LayoutPanel.TabIndex = 0;
            this.LayoutPanel.WrapContents = false;
            this.LayoutPanel.SizeChanged += new System.EventHandler(this.LayoutPanel_SizeChanged);
            this.LayoutPanel.MouseEnter += new System.EventHandler(this.LayoutPanel_MouseEnter);
            // 
            // SPChart
            // 
            this.SPChart.BackColor = System.Drawing.SystemColors.Control;
            this.SPChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SPChart.Location = new System.Drawing.Point(0, 306);
            this.SPChart.Name = "SPChart";
            this.SPChart.RestoreExtent = 395;
            this.SPChart.Size = new System.Drawing.Size(1299, 10);
            this.SPChart.TabIndex = 1;
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
            this.panel6.Size = new System.Drawing.Size(402, 306);
            this.panel6.TabIndex = 2;
            // 
            // lsParaList
            // 
            this.lsParaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsParaList.FormattingEnabled = true;
            this.lsParaList.ItemHeight = 12;
            this.lsParaList.Location = new System.Drawing.Point(0, 53);
            this.lsParaList.Name = "lsParaList";
            this.lsParaList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsParaList.Size = new System.Drawing.Size(402, 217);
            this.lsParaList.TabIndex = 31;
            this.lsParaList.SelectedIndexChanged += new System.EventHandler(this.lsParaList_SelectedIndexChanged);
            this.lsParaList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsParaList_MouseDoubleClick);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkTotalPara);
            this.panel2.Controls.Add(this.BtnAnalysis);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 270);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(402, 36);
            this.panel2.TabIndex = 0;
            // 
            // chkTotalPara
            // 
            this.chkTotalPara.AutoSize = true;
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
            this.BtnAnalysis.Location = new System.Drawing.Point(240, 2);
            this.BtnAnalysis.Name = "BtnAnalysis";
            this.BtnAnalysis.Size = new System.Drawing.Size(154, 29);
            this.BtnAnalysis.TabIndex = 0;
            this.BtnAnalysis.Text = "Analysis";
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
            this.panel1.Size = new System.Drawing.Size(402, 33);
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
            this.TxtItemFilter.Size = new System.Drawing.Size(254, 21);
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
            this.label3.Size = new System.Drawing.Size(402, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "   Test Item";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SPWafer
            // 
            this.SPWafer.BackColor = System.Drawing.SystemColors.Control;
            this.SPWafer.Location = new System.Drawing.Point(402, 0);
            this.SPWafer.Name = "SPWafer";
            this.SPWafer.RestoreExtent = 402;
            this.SPWafer.Size = new System.Drawing.Size(10, 306);
            this.SPWafer.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(412, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(887, 306);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fpRawData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(879, 280);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Raw";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fpRawData
            // 
            this.fpRawData.AccessibleDescription = "";
            this.fpRawData.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpRawData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpRawData.Location = new System.Drawing.Point(3, 3);
            this.fpRawData.Name = "fpRawData";
            this.fpRawData.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.AsNeeded;
            this.fpRawData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpRawData_Sheet1});
            this.fpRawData.Size = new System.Drawing.Size(873, 274);
            this.fpRawData.TabIndex = 15;
            this.fpRawData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpRawData_Sheet1
            // 
            this.fpRawData_Sheet1.Reset();
            fpRawData_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpRawData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpRawData_Sheet1.ColumnCount = 0;
            fpRawData_Sheet1.RowCount = 0;
            this.fpRawData_Sheet1.ActiveColumnIndex = -1;
            this.fpRawData_Sheet1.ActiveRowIndex = -1;
            this.fpRawData_Sheet1.AllowNoteEdit = false;
            this.fpRawData_Sheet1.AutoCalculation = false;
            this.fpRawData_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.DataAutoCellTypes = false;
            this.fpRawData_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpRawData_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpRawData_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpRawData_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpRawData_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpRawData_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpRawData_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpRawData_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpRawData_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fpGroupData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(879, 280);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "통계";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fpGroupData
            // 
            this.fpGroupData.AccessibleDescription = "";
            this.fpGroupData.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpGroupData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpGroupData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpGroupData.Location = new System.Drawing.Point(3, 3);
            this.fpGroupData.Name = "fpGroupData";
            this.fpGroupData.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.AsNeeded;
            this.fpGroupData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpGroupData_Sheet1});
            this.fpGroupData.Size = new System.Drawing.Size(873, 274);
            this.fpGroupData.TabIndex = 16;
            this.fpGroupData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpGroupData_Sheet1
            // 
            this.fpGroupData_Sheet1.Reset();
            fpGroupData_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpGroupData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpGroupData_Sheet1.ColumnCount = 0;
            fpGroupData_Sheet1.RowCount = 0;
            this.fpGroupData_Sheet1.ActiveColumnIndex = -1;
            this.fpGroupData_Sheet1.ActiveRowIndex = -1;
            this.fpGroupData_Sheet1.AllowNoteEdit = false;
            this.fpGroupData_Sheet1.AutoCalculation = false;
            this.fpGroupData_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpGroupData_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpGroupData_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpGroupData_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpGroupData_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpGroupData_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpGroupData_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.DataAutoCellTypes = false;
            this.fpGroupData_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpGroupData_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpGroupData_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpGroupData_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpGroupData_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpGroupData_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpGroupData_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpGroupData_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpGroupData_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpGroupData_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpGroupData_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpGroupData_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpGroupData_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpGroupData_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpGroupData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // TPUCPCMView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.SPWafer);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.SPChart);
            this.Controls.Add(this.LayoutPanel);
            this.Name = "TPUCPCMView";
            this.Size = new System.Drawing.Size(1299, 711);
            this.Load += new System.EventHandler(this.TPUCPCMView_Load);
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData_Sheet1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpGroupData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpGroupData_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel LayoutPanel;
        private Infragistics.Win.Misc.UltraSplitter SPChart;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ListBox lsParaList;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.Misc.UltraSplitter SPWafer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtItemFilter;
        private FarPoint.Win.Spread.FpSpread fpRawData;
        private FarPoint.Win.Spread.SheetView fpRawData_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnAnalysis;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private FarPoint.Win.Spread.FpSpread fpGroupData;
        private FarPoint.Win.Spread.SheetView fpGroupData_Sheet1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkTotalPara;
    }
}
