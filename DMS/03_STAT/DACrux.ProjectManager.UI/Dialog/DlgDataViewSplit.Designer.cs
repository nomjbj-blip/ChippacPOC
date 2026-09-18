namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewSplit
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
            this.tabSplit = new System.Windows.Forms.TabControl();
            this.tabPageRange = new System.Windows.Forms.TabPage();
            this.grbIncludeExclude = new System.Windows.Forms.GroupBox();
            this.rbtExclude = new System.Windows.Forms.RadioButton();
            this.rbtInclude = new System.Windows.Forms.RadioButton();
            this.grbCondition = new System.Windows.Forms.GroupBox();
            this.nudRandomSample = new System.Windows.Forms.NumericUpDown();
            this.txtRowNumbers = new System.Windows.Forms.TextBox();
            this.rbtRandomSample = new System.Windows.Forms.RadioButton();
            this.rbtRowNumbers = new System.Windows.Forms.RadioButton();
            this.rbtSelectedRange = new System.Windows.Forms.RadioButton();
            this.rbtSelectedColumn = new System.Windows.Forms.RadioButton();
            this.rbtSelectedRows = new System.Windows.Forms.RadioButton();
            this.tabPageValue = new System.Windows.Forms.TabPage();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.tabSplit.SuspendLayout();
            this.tabPageRange.SuspendLayout();
            this.grbIncludeExclude.SuspendLayout();
            this.grbCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRandomSample)).BeginInit();
            this.tabPageValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSplit
            // 
            this.tabSplit.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabSplit.Controls.Add(this.tabPageRange);
            this.tabSplit.Controls.Add(this.tabPageValue);
            this.tabSplit.Location = new System.Drawing.Point(12, 11);
            this.tabSplit.Name = "tabSplit";
            this.tabSplit.SelectedIndex = 0;
            this.tabSplit.Size = new System.Drawing.Size(393, 240);
            this.tabSplit.TabIndex = 0;
            this.tabSplit.SelectedIndexChanged += new System.EventHandler(this.tabSplit_SelectedIndexChanged);
            // 
            // tabPageRange
            // 
            this.tabPageRange.Controls.Add(this.grbIncludeExclude);
            this.tabPageRange.Controls.Add(this.grbCondition);
            this.tabPageRange.Location = new System.Drawing.Point(4, 25);
            this.tabPageRange.Name = "tabPageRange";
            this.tabPageRange.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRange.Size = new System.Drawing.Size(385, 211);
            this.tabPageRange.TabIndex = 0;
            this.tabPageRange.Text = "Range Condition";
            this.tabPageRange.UseVisualStyleBackColor = true;
            // 
            // grbIncludeExclude
            // 
            this.grbIncludeExclude.Controls.Add(this.rbtExclude);
            this.grbIncludeExclude.Controls.Add(this.rbtInclude);
            this.grbIncludeExclude.Location = new System.Drawing.Point(0, 148);
            this.grbIncludeExclude.Name = "grbIncludeExclude";
            this.grbIncludeExclude.Size = new System.Drawing.Size(385, 64);
            this.grbIncludeExclude.TabIndex = 1;
            this.grbIncludeExclude.TabStop = false;
            this.grbIncludeExclude.Text = "Include / Exclude";
            // 
            // rbtExclude
            // 
            this.rbtExclude.AutoSize = true;
            this.rbtExclude.Location = new System.Drawing.Point(17, 40);
            this.rbtExclude.Name = "rbtExclude";
            this.rbtExclude.Size = new System.Drawing.Size(62, 17);
            this.rbtExclude.TabIndex = 12;
            this.rbtExclude.Text = "Exclude";
            this.rbtExclude.UseVisualStyleBackColor = true;
            // 
            // rbtInclude
            // 
            this.rbtInclude.AutoSize = true;
            this.rbtInclude.Checked = true;
            this.rbtInclude.Location = new System.Drawing.Point(17, 18);
            this.rbtInclude.Name = "rbtInclude";
            this.rbtInclude.Size = new System.Drawing.Size(60, 17);
            this.rbtInclude.TabIndex = 11;
            this.rbtInclude.TabStop = true;
            this.rbtInclude.Text = "Include";
            this.rbtInclude.UseVisualStyleBackColor = true;
            // 
            // grbCondition
            // 
            this.grbCondition.Controls.Add(this.nudRandomSample);
            this.grbCondition.Controls.Add(this.txtRowNumbers);
            this.grbCondition.Controls.Add(this.rbtRandomSample);
            this.grbCondition.Controls.Add(this.rbtRowNumbers);
            this.grbCondition.Controls.Add(this.rbtSelectedRange);
            this.grbCondition.Controls.Add(this.rbtSelectedColumn);
            this.grbCondition.Controls.Add(this.rbtSelectedRows);
            this.grbCondition.Location = new System.Drawing.Point(0, 6);
            this.grbCondition.Name = "grbCondition";
            this.grbCondition.Size = new System.Drawing.Size(385, 136);
            this.grbCondition.TabIndex = 0;
            this.grbCondition.TabStop = false;
            this.grbCondition.Text = "Condition";
            // 
            // nudRandomSample
            // 
            this.nudRandomSample.DecimalPlaces = 4;
            this.nudRandomSample.Enabled = false;
            this.nudRandomSample.Location = new System.Drawing.Point(140, 107);
            this.nudRandomSample.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudRandomSample.Name = "nudRandomSample";
            this.nudRandomSample.Size = new System.Drawing.Size(140, 21);
            this.nudRandomSample.TabIndex = 7;
            this.nudRandomSample.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRowNumbers
            // 
            this.txtRowNumbers.Enabled = false;
            this.txtRowNumbers.Location = new System.Drawing.Point(140, 84);
            this.txtRowNumbers.Name = "txtRowNumbers";
            this.txtRowNumbers.Size = new System.Drawing.Size(140, 21);
            this.txtRowNumbers.TabIndex = 5;
            // 
            // rbtRandomSample
            // 
            this.rbtRandomSample.AutoSize = true;
            this.rbtRandomSample.Location = new System.Drawing.Point(17, 107);
            this.rbtRandomSample.Name = "rbtRandomSample";
            this.rbtRandomSample.Size = new System.Drawing.Size(101, 17);
            this.rbtRandomSample.TabIndex = 4;
            this.rbtRandomSample.Text = "Random Sample";
            this.rbtRandomSample.UseVisualStyleBackColor = true;
            this.rbtRandomSample.CheckedChanged += new System.EventHandler(this.rbtRandomSample_CheckedChanged);
            // 
            // rbtRowNumbers
            // 
            this.rbtRowNumbers.AutoSize = true;
            this.rbtRowNumbers.Location = new System.Drawing.Point(17, 84);
            this.rbtRowNumbers.Name = "rbtRowNumbers";
            this.rbtRowNumbers.Size = new System.Drawing.Size(91, 17);
            this.rbtRowNumbers.TabIndex = 3;
            this.rbtRowNumbers.Text = "Row Numbers";
            this.rbtRowNumbers.UseVisualStyleBackColor = true;
            this.rbtRowNumbers.CheckedChanged += new System.EventHandler(this.rbtRowNumbers_CheckedChanged);
            // 
            // rbtSelectedRange
            // 
            this.rbtSelectedRange.AutoSize = true;
            this.rbtSelectedRange.Location = new System.Drawing.Point(17, 62);
            this.rbtSelectedRange.Name = "rbtSelectedRange";
            this.rbtSelectedRange.Size = new System.Drawing.Size(100, 17);
            this.rbtSelectedRange.TabIndex = 2;
            this.rbtSelectedRange.Text = "Selected Range";
            this.rbtSelectedRange.UseVisualStyleBackColor = true;
            this.rbtSelectedRange.CheckedChanged += new System.EventHandler(this.rbtSelectedRange_CheckedChanged);
            // 
            // rbtSelectedColumn
            // 
            this.rbtSelectedColumn.AutoSize = true;
            this.rbtSelectedColumn.Location = new System.Drawing.Point(17, 40);
            this.rbtSelectedColumn.Name = "rbtSelectedColumn";
            this.rbtSelectedColumn.Size = new System.Drawing.Size(104, 17);
            this.rbtSelectedColumn.TabIndex = 1;
            this.rbtSelectedColumn.Text = "Selected Column";
            this.rbtSelectedColumn.UseVisualStyleBackColor = true;
            // 
            // rbtSelectedRows
            // 
            this.rbtSelectedRows.AutoSize = true;
            this.rbtSelectedRows.Location = new System.Drawing.Point(17, 18);
            this.rbtSelectedRows.Name = "rbtSelectedRows";
            this.rbtSelectedRows.Size = new System.Drawing.Size(95, 17);
            this.rbtSelectedRows.TabIndex = 0;
            this.rbtSelectedRows.Text = "Selected Rows";
            this.rbtSelectedRows.UseVisualStyleBackColor = true;
            // 
            // tabPageValue
            // 
            this.tabPageValue.Controls.Add(this.fpSpread);
            this.tabPageValue.Location = new System.Drawing.Point(4, 26);
            this.tabPageValue.Name = "tabPageValue";
            this.tabPageValue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageValue.Size = new System.Drawing.Size(385, 210);
            this.tabPageValue.TabIndex = 1;
            this.tabPageValue.Text = "Value Condition";
            this.tabPageValue.UseVisualStyleBackColor = true;
            // 
            // fpSpread
            // 
            this.fpSpread.About = "4.0.2001.2005";
            this.fpSpread.AccessibleDescription = "fpSpread, Sheet, Row 0, Column 0, ";
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.Location = new System.Drawing.Point(3, 3);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(379, 204);
            this.fpSpread.TabIndex = 0;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            this.fpSpread_Sheet.SheetName = "Sheet";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(337, 257);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(267, 257);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.Location = new System.Drawing.Point(326, 10);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(75, 21);
            this.btnDeleteRow.TabIndex = 8;
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.UseVisualStyleBackColor = true;
            this.btnDeleteRow.Visible = false;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // DlgDataViewSplit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 292);
            this.Controls.Add(this.btnDeleteRow);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabSplit);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewSplit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Split";
            this.Load += new System.EventHandler(this.DlgDataViewSplit_Load);
            this.tabSplit.ResumeLayout(false);
            this.tabPageRange.ResumeLayout(false);
            this.grbIncludeExclude.ResumeLayout(false);
            this.grbIncludeExclude.PerformLayout();
            this.grbCondition.ResumeLayout(false);
            this.grbCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRandomSample)).EndInit();
            this.tabPageValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSplit;
        private System.Windows.Forms.TabPage tabPageRange;
        private System.Windows.Forms.TabPage tabPageValue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grbCondition;
        private System.Windows.Forms.RadioButton rbtRandomSample;
        private System.Windows.Forms.RadioButton rbtRowNumbers;
        private System.Windows.Forms.RadioButton rbtSelectedRange;
        private System.Windows.Forms.RadioButton rbtSelectedColumn;
        private System.Windows.Forms.RadioButton rbtSelectedRows;
        private System.Windows.Forms.TextBox txtRowNumbers;
        private System.Windows.Forms.NumericUpDown nudRandomSample;
        private System.Windows.Forms.GroupBox grbIncludeExclude;
        private System.Windows.Forms.RadioButton rbtExclude;
        private System.Windows.Forms.RadioButton rbtInclude;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.Button btnDeleteRow;

    }
}