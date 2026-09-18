namespace DACrux.ProjectManager.UI
{
    sealed partial class DataView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataView));
            this.cmsDataView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiColumnSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMoveColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSort = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSplit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSplitSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiTranspose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.cmsDataView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsDataView
            // 
            this.cmsDataView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiColumnSetting,
            this.toolStripSeparator1,
            this.cmiInsert,
            this.cmiRemove,
            this.cmiMoveColumn,
            this.toolStripSeparator2,
            this.cmiSort,
            this.cmiSplit,
            this.cmiSplitSelected,
            this.cmiTranspose,
            this.toolStripSeparator3,
            this.cmiExportToExcel});
            this.cmsDataView.Name = "cmsDataView";
            this.cmsDataView.Size = new System.Drawing.Size(171, 220);
            this.cmsDataView.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsDataView_ItemClicked);
            // 
            // cmiColumnSetting
            // 
            this.cmiColumnSetting.Image = ((System.Drawing.Image)(resources.GetObject("cmiColumnSetting.Image")));
            this.cmiColumnSetting.Name = "cmiColumnSetting";
            this.cmiColumnSetting.Size = new System.Drawing.Size(170, 22);
            this.cmiColumnSetting.Text = "Column Setting..";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // cmiInsert
            // 
            this.cmiInsert.Image = ((System.Drawing.Image)(resources.GetObject("cmiInsert.Image")));
            this.cmiInsert.Name = "cmiInsert";
            this.cmiInsert.Size = new System.Drawing.Size(170, 22);
            this.cmiInsert.Text = "Insert..";
            // 
            // cmiRemove
            // 
            this.cmiRemove.Image = ((System.Drawing.Image)(resources.GetObject("cmiRemove.Image")));
            this.cmiRemove.Name = "cmiRemove";
            this.cmiRemove.Size = new System.Drawing.Size(170, 22);
            this.cmiRemove.Text = "Remove..";
            // 
            // cmiMoveColumn
            // 
            this.cmiMoveColumn.Image = ((System.Drawing.Image)(resources.GetObject("cmiMoveColumn.Image")));
            this.cmiMoveColumn.Name = "cmiMoveColumn";
            this.cmiMoveColumn.Size = new System.Drawing.Size(170, 22);
            this.cmiMoveColumn.Text = "Move Column..";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // cmiSort
            // 
            this.cmiSort.Image = ((System.Drawing.Image)(resources.GetObject("cmiSort.Image")));
            this.cmiSort.Name = "cmiSort";
            this.cmiSort.Size = new System.Drawing.Size(170, 22);
            this.cmiSort.Text = "Sort";
            // 
            // cmiSplit
            // 
            this.cmiSplit.Image = ((System.Drawing.Image)(resources.GetObject("cmiSplit.Image")));
            this.cmiSplit.Name = "cmiSplit";
            this.cmiSplit.Size = new System.Drawing.Size(170, 22);
            this.cmiSplit.Text = "Split";
            // 
            // cmiSplitSelected
            // 
            this.cmiSplitSelected.Image = ((System.Drawing.Image)(resources.GetObject("cmiSplitSelected.Image")));
            this.cmiSplitSelected.Name = "cmiSplitSelected";
            this.cmiSplitSelected.Size = new System.Drawing.Size(170, 22);
            this.cmiSplitSelected.Text = "Split Selected";
            // 
            // cmiTranspose
            // 
            this.cmiTranspose.Image = ((System.Drawing.Image)(resources.GetObject("cmiTranspose.Image")));
            this.cmiTranspose.Name = "cmiTranspose";
            this.cmiTranspose.Size = new System.Drawing.Size(170, 22);
            this.cmiTranspose.Text = "Transpose";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(167, 6);
            // 
            // cmiExportToExcel
            // 
            this.cmiExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("cmiExportToExcel.Image")));
            this.cmiExportToExcel.Name = "cmiExportToExcel";
            this.cmiExportToExcel.Size = new System.Drawing.Size(170, 22);
            this.cmiExportToExcel.Text = "Export To Excel";
            // 
            // fpSpread
            // 
            this.fpSpread.About = "4.0.2001.2005";
            this.fpSpread.AccessibleDescription = "fpSpread, Sheet, Row 0, Column 0, ";
            this.fpSpread.BackColor = System.Drawing.SystemColors.Control;
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.Location = new System.Drawing.Point(0, 0);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(794, 616);
            this.fpSpread.TabIndex = 1;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            this.fpSpread_Sheet.SheetName = "Sheet";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // DataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fpSpread);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DataView";
            this.Size = new System.Drawing.Size(794, 616);
            this.Load += new System.EventHandler(this.DataView_Load);
            this.cmsDataView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsDataView;
        private System.Windows.Forms.ToolStripMenuItem cmiColumnSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiInsert;
        private System.Windows.Forms.ToolStripMenuItem cmiRemove;
        private System.Windows.Forms.ToolStripMenuItem cmiSort;
        private System.Windows.Forms.ToolStripMenuItem cmiSplit;
        private System.Windows.Forms.ToolStripMenuItem cmiSplitSelected;
        private System.Windows.Forms.ToolStripMenuItem cmiTranspose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.ToolStripMenuItem cmiMoveColumn;
        private System.Windows.Forms.ToolStripMenuItem cmiExportToExcel;
    }
}
