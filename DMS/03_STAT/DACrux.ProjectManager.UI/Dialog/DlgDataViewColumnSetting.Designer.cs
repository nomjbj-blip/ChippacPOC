namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewColumnSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgDataViewColumnSetting));
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // fpSpread
            // 
            this.fpSpread.About = "4.0.2001.2005";
            resources.ApplyResources(this.fpSpread, "fpSpread");
            this.fpSpread.BackColor = System.Drawing.SystemColors.Control;
            this.fpSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Vertical;
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            this.fpSpread_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread_Sheet.ColumnCount = 7;
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 0).Value = "Name";
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 1).ColumnSpan = 3;
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 1).Value = "Type";
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 4).Value = "Format";
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 5).Value = "Option";
            this.fpSpread_Sheet.ColumnHeader.Cells.Get(0, 6).Value = "Decimal Place";
            this.fpSpread_Sheet.Columns.Get(0).Label = "Name";
            this.fpSpread_Sheet.Columns.Get(0).Width = 103F;
            this.fpSpread_Sheet.Columns.Get(1).Label = "Type";
            this.fpSpread_Sheet.Columns.Get(1).Width = 25F;
            this.fpSpread_Sheet.Columns.Get(2).Width = 25F;
            this.fpSpread_Sheet.Columns.Get(3).Width = 25F;
            this.fpSpread_Sheet.Columns.Get(4).Label = "Format";
            this.fpSpread_Sheet.Columns.Get(4).Width = 124F;
            this.fpSpread_Sheet.Columns.Get(5).Label = "Option";
            this.fpSpread_Sheet.Columns.Get(5).Width = 78F;
            this.fpSpread_Sheet.Columns.Get(6).Label = "Decimal Place";
            this.fpSpread_Sheet.Columns.Get(6).Width = 86F;
            this.fpSpread_Sheet.RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
            this.fpSpread_Sheet.RowHeader.Columns.Default.Resizable = true;
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // DlgDataViewColumnSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.fpSpread);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewColumnSetting";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;

    }
}