namespace DACrux.SEMDMS.ENGUI
{
    partial class frmViewMapParsingResult
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
            this.pnlResult = new System.Windows.Forms.Panel();
            this.pnlButtom = new System.Windows.Forms.Panel();
            this.fpSpread_Result = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Result_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.lblInspEq = new System.Windows.Forms.Label();
            this.lblStepId = new System.Windows.Forms.Label();
            this.lblWaferId = new System.Windows.Forms.Label();
            this.lblLotId = new System.Windows.Forms.Label();
            this.cmbInspEq = new System.Windows.Forms.ComboBox();
            this.cmbStepId = new System.Windows.Forms.ComboBox();
            this.cmbWaferId = new System.Windows.Forms.ComboBox();
            this.cmbLotId = new System.Windows.Forms.ComboBox();
            this.pnlResult.SuspendLayout();
            this.pnlButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Result_Sheet)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlResult
            // 
            this.pnlResult.Controls.Add(this.pnlButtom);
            this.pnlResult.Controls.Add(this.pnlTop);
            this.pnlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResult.Location = new System.Drawing.Point(0, 0);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Size = new System.Drawing.Size(768, 527);
            this.pnlResult.TabIndex = 0;
            // 
            // pnlButtom
            // 
            this.pnlButtom.Controls.Add(this.fpSpread_Result);
            this.pnlButtom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtom.Location = new System.Drawing.Point(0, 100);
            this.pnlButtom.Name = "pnlButtom";
            this.pnlButtom.Size = new System.Drawing.Size(768, 427);
            this.pnlButtom.TabIndex = 1;
            // 
            // fpSpread_Result
            // 
            this.fpSpread_Result.AccessibleDescription = "fpSpread_Result, fpSpread_Result";
            this.fpSpread_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread_Result.Location = new System.Drawing.Point(0, 0);
            this.fpSpread_Result.Name = "fpSpread_Result";
            this.fpSpread_Result.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread_Result.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Result_Sheet});
            this.fpSpread_Result.Size = new System.Drawing.Size(768, 427);
            this.fpSpread_Result.TabIndex = 11;
            this.fpSpread_Result.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread_Result_Sheet
            // 
            this.fpSpread_Result_Sheet.Reset();
            fpSpread_Result_Sheet.SheetName = "fpSpread_Result";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Result_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread_Result_Sheet.ColumnCount = 0;
            fpSpread_Result_Sheet.RowCount = 0;
            this.fpSpread_Result_Sheet.ActiveColumnIndex = -1;
            this.fpSpread_Result_Sheet.ActiveRowIndex = -1;
            this.fpSpread_Result_Sheet.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, false, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpread_Result_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpread_Result_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Result_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Result_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Result_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Result_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Result_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Result_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.Columns.Default.Width = 100F;
            this.fpSpread_Result_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread_Result_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread_Result_Sheet.GroupBarHeight = 22;
            this.fpSpread_Result_Sheet.GroupBarInfo.Height = 22;
            this.fpSpread_Result_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Result_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Result_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Result_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.RowHeader.Visible = false;
            this.fpSpread_Result_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Result_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Result_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Result_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Result_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Result_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnView);
            this.pnlTop.Controls.Add(this.lblInspEq);
            this.pnlTop.Controls.Add(this.lblStepId);
            this.pnlTop.Controls.Add(this.lblWaferId);
            this.pnlTop.Controls.Add(this.lblLotId);
            this.pnlTop.Controls.Add(this.cmbInspEq);
            this.pnlTop.Controls.Add(this.cmbStepId);
            this.pnlTop.Controls.Add(this.cmbWaferId);
            this.pnlTop.Controls.Add(this.cmbLotId);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(768, 100);
            this.pnlTop.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(605, 56);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 8;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblInspEq
            // 
            this.lblInspEq.AutoSize = true;
            this.lblInspEq.Location = new System.Drawing.Point(288, 61);
            this.lblInspEq.Name = "lblInspEq";
            this.lblInspEq.Size = new System.Drawing.Size(99, 15);
            this.lblInspEq.TabIndex = 7;
            this.lblInspEq.Text = "INSPECTION EQ";
            // 
            // lblStepId
            // 
            this.lblStepId.AutoSize = true;
            this.lblStepId.Location = new System.Drawing.Point(288, 33);
            this.lblStepId.Name = "lblStepId";
            this.lblStepId.Size = new System.Drawing.Size(57, 15);
            this.lblStepId.TabIndex = 6;
            this.lblStepId.Text = "STEP_ID";
            // 
            // lblWaferId
            // 
            this.lblWaferId.AutoSize = true;
            this.lblWaferId.Location = new System.Drawing.Point(40, 61);
            this.lblWaferId.Name = "lblWaferId";
            this.lblWaferId.Size = new System.Drawing.Size(68, 15);
            this.lblWaferId.TabIndex = 5;
            this.lblWaferId.Text = "WAFER_ID";
            // 
            // lblLotId
            // 
            this.lblLotId.AutoSize = true;
            this.lblLotId.Location = new System.Drawing.Point(40, 32);
            this.lblLotId.Name = "lblLotId";
            this.lblLotId.Size = new System.Drawing.Size(45, 15);
            this.lblLotId.TabIndex = 4;
            this.lblLotId.Text = "LOT ID";
            // 
            // cmbInspEq
            // 
            this.cmbInspEq.FormattingEnabled = true;
            this.cmbInspEq.Location = new System.Drawing.Point(424, 59);
            this.cmbInspEq.Name = "cmbInspEq";
            this.cmbInspEq.Size = new System.Drawing.Size(121, 20);
            this.cmbInspEq.TabIndex = 3;
            this.cmbInspEq.Click += new System.EventHandler(this.cmbInspEq_Click);
            this.cmbInspEq.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbInspEq_MouseClick);
            // 
            // cmbStepId
            // 
            this.cmbStepId.FormattingEnabled = true;
            this.cmbStepId.Location = new System.Drawing.Point(424, 31);
            this.cmbStepId.Name = "cmbStepId";
            this.cmbStepId.Size = new System.Drawing.Size(121, 20);
            this.cmbStepId.TabIndex = 2;
            this.cmbStepId.Click += new System.EventHandler(this.cmbStepId_Click);
            this.cmbStepId.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbStepId_MouseClick);
            // 
            // cmbWaferId
            // 
            this.cmbWaferId.FormattingEnabled = true;
            this.cmbWaferId.Location = new System.Drawing.Point(146, 59);
            this.cmbWaferId.Name = "cmbWaferId";
            this.cmbWaferId.Size = new System.Drawing.Size(121, 20);
            this.cmbWaferId.TabIndex = 1;
            this.cmbWaferId.Click += new System.EventHandler(this.cmbWaferId_Click);
            this.cmbWaferId.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbWaferId_MouseClick);
            // 
            // cmbLotId
            // 
            this.cmbLotId.FormattingEnabled = true;
            this.cmbLotId.Location = new System.Drawing.Point(146, 31);
            this.cmbLotId.Name = "cmbLotId";
            this.cmbLotId.Size = new System.Drawing.Size(121, 20);
            this.cmbLotId.TabIndex = 0;
            this.cmbLotId.Click += new System.EventHandler(this.cmbLotId_Click);
            this.cmbLotId.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbLotId_MouseClick);
            // 
            // frmViewMapParsingResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(768, 527);
            this.Controls.Add(this.pnlResult);
            this.Name = "frmViewMapParsingResult";
            this.Text = "View Map Parsing Result";
            this.Load += new System.EventHandler(this.frmViewMapParsingResult_Load);
            this.pnlResult.ResumeLayout(false);
            this.pnlButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Result_Sheet)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Panel pnlButtom;
        private System.Windows.Forms.Panel pnlTop;
        private FarPoint.Win.Spread.FpSpread fpSpread_Result;
        private FarPoint.Win.Spread.SheetView fpSpread_Result_Sheet;
        private System.Windows.Forms.ComboBox cmbLotId;
        private System.Windows.Forms.ComboBox cmbInspEq;
        private System.Windows.Forms.ComboBox cmbStepId;
        private System.Windows.Forms.ComboBox cmbWaferId;
        private System.Windows.Forms.Label lblInspEq;
        private System.Windows.Forms.Label lblStepId;
        private System.Windows.Forms.Label lblWaferId;
        private System.Windows.Forms.Label lblLotId;
        private System.Windows.Forms.Button btnView;
    }
}