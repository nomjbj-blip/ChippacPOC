namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgTaguchiFactor
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.rdoFactorDefine = new System.Windows.Forms.RadioButton();
            this.rdoFactorCombine = new System.Windows.Forms.RadioButton();
            this.butCloseEffect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(321, 257);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(134, 34);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(461, 257);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(93, 34);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.fpSpread1.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.Location = new System.Drawing.Point(12, 78);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(542, 173);
            this.fpSpread1.TabIndex = 5;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.EditModeOff += new System.EventHandler(this.fpSpread1_EditModeOff);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread1_Sheet1.ColumnCount = 4;
            fpSpread1_Sheet1.RowCount = 2;
            this.fpSpread1_Sheet1.AutoCalculation = false;
            this.fpSpread1_Sheet1.AutoGenerateColumns = false;
            this.fpSpread1_Sheet1.AutoUpdateNotes = false;
            this.fpSpread1_Sheet1.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "이름";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수준 값";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "열";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수준";
            textCellType1.StringTrim = System.Drawing.StringTrimming.Character;
            this.fpSpread1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpSpread1_Sheet1.Columns.Get(0).Label = "이름";
            this.fpSpread1_Sheet1.Columns.Get(0).Locked = false;
            this.fpSpread1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpSpread1_Sheet1.Columns.Get(1).Label = "수준 값";
            this.fpSpread1_Sheet1.Columns.Get(1).Width = 288F;
            comboBoxCellType1.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            comboBoxCellType1.CharacterSet = FarPoint.Win.ComboCharacterSet.Numeric;
            this.fpSpread1_Sheet1.Columns.Get(2).CellType = comboBoxCellType1;
            this.fpSpread1_Sheet1.Columns.Get(2).Label = "열";
            this.fpSpread1_Sheet1.Columns.Get(2).Width = 70F;
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.ReadOnly = true;
            this.fpSpread1_Sheet1.Columns.Get(3).CellType = numberCellType1;
            this.fpSpread1_Sheet1.Columns.Get(3).Label = "수준";
            this.fpSpread1_Sheet1.DataAutoCellTypes = false;
            this.fpSpread1_Sheet1.DataAutoHeadings = false;
            this.fpSpread1_Sheet1.RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Letters;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // rdoFactorDefine
            // 
            this.rdoFactorDefine.AutoSize = true;
            this.rdoFactorDefine.Checked = true;
            this.rdoFactorDefine.Location = new System.Drawing.Point(7, 34);
            this.rdoFactorDefine.Name = "rdoFactorDefine";
            this.rdoFactorDefine.Size = new System.Drawing.Size(202, 16);
            this.rdoFactorDefine.TabIndex = 6;
            this.rdoFactorDefine.TabStop = true;
            this.rdoFactorDefine.Text = "배열의 열에 아래 지정된 대로(C)";
            this.rdoFactorDefine.UseVisualStyleBackColor = true;
            // 
            // rdoFactorCombine
            // 
            this.rdoFactorCombine.AutoSize = true;
            this.rdoFactorCombine.Location = new System.Drawing.Point(7, 56);
            this.rdoFactorCombine.Name = "rdoFactorCombine";
            this.rdoFactorCombine.Size = new System.Drawing.Size(305, 16);
            this.rdoFactorCombine.TabIndex = 6;
            this.rdoFactorCombine.Text = "다음에 선택된 상호작용의 추정을 허용하기 위해 (E)";
            this.rdoFactorCombine.UseVisualStyleBackColor = true;
            this.rdoFactorCombine.CheckedChanged += new System.EventHandler(this.rdoFactorCombine_CheckedChanged);
            // 
            // butCloseEffect
            // 
            this.butCloseEffect.Enabled = false;
            this.butCloseEffect.Location = new System.Drawing.Point(421, 49);
            this.butCloseEffect.Name = "butCloseEffect";
            this.butCloseEffect.Size = new System.Drawing.Size(133, 23);
            this.butCloseEffect.TabIndex = 7;
            this.butCloseEffect.Text = "상호작용";
            this.butCloseEffect.UseVisualStyleBackColor = true;
            this.butCloseEffect.Click += new System.EventHandler(this.butCloseEffect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "요인할당";
            // 
            // DlgTaguchiFactor
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(565, 303);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCloseEffect);
            this.Controls.Add(this.rdoFactorCombine);
            this.Controls.Add(this.rdoFactorDefine);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgTaguchiFactor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Taguchi Design - 사용가능한 설계";
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.RadioButton rdoFactorDefine;
        private System.Windows.Forms.RadioButton rdoFactorCombine;
        private System.Windows.Forms.Button butCloseEffect;
        private System.Windows.Forms.Label label1;
    }
}