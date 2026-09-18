namespace DACrux.SEMDMS.Control
{
    partial class DPUCImageGalleryGrid
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.INFORMATION = new System.Windows.Forms.TabPage();
            this.dpucInformation1 = new DACrux.SEMDMS.Control.DPUCInformation();
            this.RECLASSIFY = new System.Windows.Forms.TabPage();
            this.dpucReclassify1 = new DACrux.SEMDMS.Control.DPUCReclassify();
            this.REMOVEIMG = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ugSelectedImages = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpsRemoveImages = new FarPoint.Win.Spread.FpSpread();
            this.fpsRemoveImages_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.fpsDefectImages = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefectImages_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkZoomable = new System.Windows.Forms.CheckBox();
            this.chkNoDisplay = new System.Windows.Forms.CheckBox();
            this.chkNone = new System.Windows.Forms.CheckBox();
            this.pnlColCount = new System.Windows.Forms.Panel();
            this.btnCountApply = new System.Windows.Forms.Button();
            this.numRowCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numColCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.INFORMATION.SuspendLayout();
            this.RECLASSIFY.SuspendLayout();
            this.REMOVEIMG.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSelectedImages)).BeginInit();
            this.ugSelectedImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsRemoveImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsRemoveImages_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectImages_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlColCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColCount)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.tabControl1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraPanel1.Location = new System.Drawing.Point(509, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(291, 600);
            this.ultraPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.INFORMATION);
            this.tabControl1.Controls.Add(this.RECLASSIFY);
            this.tabControl1.Controls.Add(this.REMOVEIMG);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(291, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // INFORMATION
            // 
            this.INFORMATION.Controls.Add(this.dpucInformation1);
            this.INFORMATION.Location = new System.Drawing.Point(4, 22);
            this.INFORMATION.Name = "INFORMATION";
            this.INFORMATION.Padding = new System.Windows.Forms.Padding(3);
            this.INFORMATION.Size = new System.Drawing.Size(283, 574);
            this.INFORMATION.TabIndex = 0;
            this.INFORMATION.Text = "Information";
            this.INFORMATION.UseVisualStyleBackColor = true;
            // 
            // dpucInformation1
            // 
            this.dpucInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucInformation1.Location = new System.Drawing.Point(3, 3);
            this.dpucInformation1.Name = "dpucInformation1";
            this.dpucInformation1.Size = new System.Drawing.Size(277, 568);
            this.dpucInformation1.TabIndex = 0;
            // 
            // RECLASSIFY
            // 
            this.RECLASSIFY.Controls.Add(this.dpucReclassify1);
            this.RECLASSIFY.Location = new System.Drawing.Point(4, 22);
            this.RECLASSIFY.Name = "RECLASSIFY";
            this.RECLASSIFY.Padding = new System.Windows.Forms.Padding(3);
            this.RECLASSIFY.Size = new System.Drawing.Size(283, 574);
            this.RECLASSIFY.TabIndex = 1;
            this.RECLASSIFY.Text = "Reclassify";
            this.RECLASSIFY.UseVisualStyleBackColor = true;
            // 
            // dpucReclassify1
            // 
            this.dpucReclassify1.DataSource = null;
            this.dpucReclassify1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucReclassify1.IsDisplayApply = true;
            this.dpucReclassify1.Location = new System.Drawing.Point(3, 3);
            this.dpucReclassify1.Name = "dpucReclassify1";
            this.dpucReclassify1.Size = new System.Drawing.Size(277, 568);
            this.dpucReclassify1.TabIndex = 0;
            // 
            // REMOVEIMG
            // 
            this.REMOVEIMG.Controls.Add(this.panel2);
            this.REMOVEIMG.Location = new System.Drawing.Point(4, 22);
            this.REMOVEIMG.Name = "REMOVEIMG";
            this.REMOVEIMG.Padding = new System.Windows.Forms.Padding(3);
            this.REMOVEIMG.Size = new System.Drawing.Size(283, 574);
            this.REMOVEIMG.TabIndex = 2;
            this.REMOVEIMG.Text = "Remove Image";
            this.REMOVEIMG.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ugSelectedImages);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(277, 568);
            this.panel2.TabIndex = 0;
            // 
            // ugSelectedImages
            // 
            this.ugSelectedImages.Controls.Add(this.fpsRemoveImages);
            this.ugSelectedImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugSelectedImages.Location = new System.Drawing.Point(0, 0);
            this.ugSelectedImages.Name = "ugSelectedImages";
            this.ugSelectedImages.Size = new System.Drawing.Size(277, 537);
            this.ugSelectedImages.TabIndex = 1;
            this.ugSelectedImages.Text = "Selected Images";
            this.ugSelectedImages.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // fpsRemoveImages
            // 
            this.fpsRemoveImages.AccessibleDescription = "";
            this.fpsRemoveImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsRemoveImages.Location = new System.Drawing.Point(2, 18);
            this.fpsRemoveImages.Name = "fpsRemoveImages";
            this.fpsRemoveImages.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsRemoveImages_Sheet1});
            this.fpsRemoveImages.Size = new System.Drawing.Size(273, 517);
            this.fpsRemoveImages.TabIndex = 0;
            // 
            // fpsRemoveImages_Sheet1
            // 
            this.fpsRemoveImages_Sheet1.Reset();
            fpsRemoveImages_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsRemoveImages_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpsRemoveImages_Sheet1.ColumnCount = 0;
            fpsRemoveImages_Sheet1.RowCount = 0;
            this.fpsRemoveImages_Sheet1.ActiveColumnIndex = -1;
            this.fpsRemoveImages_Sheet1.ActiveRowIndex = -1;
            this.fpsRemoveImages_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.fpsRemoveImages_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpsRemoveImages_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 537);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(277, 31);
            this.panel3.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(199, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(499, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 237;
            this.ultraSplitter1.Size = new System.Drawing.Size(10, 600);
            this.ultraSplitter1.TabIndex = 2;
            // 
            // fpsDefectImages
            // 
            this.fpsDefectImages.AccessibleDescription = "";
            this.fpsDefectImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDefectImages.Location = new System.Drawing.Point(0, 30);
            this.fpsDefectImages.Name = "fpsDefectImages";
            this.fpsDefectImages.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDefectImages_Sheet1});
            this.fpsDefectImages.Size = new System.Drawing.Size(499, 570);
            this.fpsDefectImages.TabIndex = 3;
            this.fpsDefectImages.RowHeightChanged += new FarPoint.Win.Spread.RowHeightChangedEventHandler(this.fpsDefectImages_RowHeightChanged);
            this.fpsDefectImages.ColumnWidthChanged += new FarPoint.Win.Spread.ColumnWidthChangedEventHandler(this.fpsDefectImages_ColumnWidthChanged);
            this.fpsDefectImages.EnterCell += new FarPoint.Win.Spread.EnterCellEventHandler(this.fpsDefectImages_EnterCell);
            this.fpsDefectImages.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsDefectImages_CellClick);
            this.fpsDefectImages.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsDefectImages_CellDoubleClick);
            this.fpsDefectImages.UserZooming += new FarPoint.Win.Spread.UserZoomingEventHandler(this.fpsDefectImages_UserZooming);
            this.fpsDefectImages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fpsDefectImages_KeyDown);
            this.fpsDefectImages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fpsDefectImages_MouseUp);
            // 
            // fpsDefectImages_Sheet1
            // 
            this.fpsDefectImages_Sheet1.Reset();
            fpsDefectImages_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsDefectImages_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpsDefectImages_Sheet1.ColumnCount = 1;
            fpsDefectImages_Sheet1.RowCount = 1;
            this.fpsDefectImages_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpsDefectImages_Sheet1.RowHeader.Columns.Get(0).Width = 100F;
            this.fpsDefectImages_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpsDefectImages_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpsDefectImages_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkZoomable);
            this.panel1.Controls.Add(this.chkNoDisplay);
            this.panel1.Controls.Add(this.chkNone);
            this.panel1.Controls.Add(this.pnlColCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 30);
            this.panel1.TabIndex = 4;
            // 
            // chkZoomable
            // 
            this.chkZoomable.AutoSize = true;
            this.chkZoomable.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkZoomable.Location = new System.Drawing.Point(375, 0);
            this.chkZoomable.Name = "chkZoomable";
            this.chkZoomable.Size = new System.Drawing.Size(57, 30);
            this.chkZoomable.TabIndex = 18;
            this.chkZoomable.Text = "Zoom";
            this.chkZoomable.UseVisualStyleBackColor = true;
            // 
            // chkNoDisplay
            // 
            this.chkNoDisplay.AutoSize = true;
            this.chkNoDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkNoDisplay.Location = new System.Drawing.Point(289, 0);
            this.chkNoDisplay.Name = "chkNoDisplay";
            this.chkNoDisplay.Size = new System.Drawing.Size(86, 30);
            this.chkNoDisplay.TabIndex = 17;
            this.chkNoDisplay.Text = "No Display";
            this.chkNoDisplay.UseVisualStyleBackColor = true;
            this.chkNoDisplay.CheckedChanged += new System.EventHandler(this.chkNoDisplay_CheckedChanged);
            // 
            // chkNone
            // 
            this.chkNone.AutoSize = true;
            this.chkNone.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkNone.Location = new System.Drawing.Point(230, 0);
            this.chkNone.Name = "chkNone";
            this.chkNone.Size = new System.Drawing.Size(59, 30);
            this.chkNone.TabIndex = 16;
            this.chkNone.Text = "NONE";
            this.chkNone.UseVisualStyleBackColor = true;
            this.chkNone.CheckedChanged += new System.EventHandler(this.chkNone_CheckedChanged);
            // 
            // pnlColCount
            // 
            this.pnlColCount.Controls.Add(this.btnCountApply);
            this.pnlColCount.Controls.Add(this.numRowCount);
            this.pnlColCount.Controls.Add(this.label2);
            this.pnlColCount.Controls.Add(this.numColCount);
            this.pnlColCount.Controls.Add(this.label1);
            this.pnlColCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlColCount.Location = new System.Drawing.Point(0, 0);
            this.pnlColCount.Name = "pnlColCount";
            this.pnlColCount.Size = new System.Drawing.Size(230, 30);
            this.pnlColCount.TabIndex = 15;
            // 
            // btnCountApply
            // 
            this.btnCountApply.Location = new System.Drawing.Point(171, 4);
            this.btnCountApply.Name = "btnCountApply";
            this.btnCountApply.Size = new System.Drawing.Size(53, 23);
            this.btnCountApply.TabIndex = 15;
            this.btnCountApply.Text = "Apply";
            this.btnCountApply.UseVisualStyleBackColor = true;
            this.btnCountApply.Click += new System.EventHandler(this.btnCountApply_Click);
            // 
            // numRowCount
            // 
            this.numRowCount.Location = new System.Drawing.Point(119, 5);
            this.numRowCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numRowCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRowCount.Name = "numRowCount";
            this.numRowCount.Size = new System.Drawing.Size(47, 21);
            this.numRowCount.TabIndex = 17;
            this.numRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRowCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Row";
            // 
            // numColCount
            // 
            this.numColCount.Location = new System.Drawing.Point(38, 5);
            this.numColCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numColCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numColCount.Name = "numColCount";
            this.numColCount.Size = new System.Drawing.Size(47, 21);
            this.numColCount.TabIndex = 15;
            this.numColCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numColCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Col";
            // 
            // DPUCImageGalleryGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fpsDefectImages);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "DPUCImageGalleryGrid";
            this.Size = new System.Drawing.Size(800, 600);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.INFORMATION.ResumeLayout(false);
            this.RECLASSIFY.ResumeLayout(false);
            this.REMOVEIMG.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSelectedImages)).EndInit();
            this.ugSelectedImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsRemoveImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsRemoveImages_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectImages_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlColCount.ResumeLayout(false);
            this.pnlColCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private FarPoint.Win.Spread.FpSpread fpsDefectImages;
        private FarPoint.Win.Spread.SheetView fpsDefectImages_Sheet1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage INFORMATION;
        private DPUCInformation dpucInformation1;
        private System.Windows.Forms.TabPage RECLASSIFY;
        private DPUCReclassify dpucReclassify1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlColCount;
        private System.Windows.Forms.Button btnCountApply;
        private System.Windows.Forms.NumericUpDown numRowCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numColCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNone;
        private System.Windows.Forms.CheckBox chkNoDisplay;
        private System.Windows.Forms.TabPage REMOVEIMG;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.Misc.UltraGroupBox ugSelectedImages;
        private FarPoint.Win.Spread.FpSpread fpsRemoveImages;
        private FarPoint.Win.Spread.SheetView fpsRemoveImages_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkZoomable;
    }
}
