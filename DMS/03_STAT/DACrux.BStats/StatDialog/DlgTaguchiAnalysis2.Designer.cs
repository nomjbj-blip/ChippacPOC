namespace DACrux.BStats.StatDialog
{
    partial class DlgTaguchiAnalysis2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.butGraph = new System.Windows.Forms.Button();
            this.butAnalysis = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.butOption = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.butAnalysisSel = new System.Windows.Forms.Button();
            this.butAnalysisDeSel = new System.Windows.Forms.Button();
            this.dgAnalysisList = new System.Windows.Forms.DataGridView();
            this.COLUMNID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COLUMNNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.butParaSel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_Analysis = new System.Windows.Forms.Label();
            this.dgAnalsysisSelList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgFactorSelList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_Factor = new System.Windows.Forms.Label();
            this.butFactorSel = new System.Windows.Forms.Button();
            this.butFactorDeSel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnalysisList)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnalsysisSelList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFactorSelList)).BeginInit();
            this.SuspendLayout();
            // 
            // butGraph
            // 
            this.butGraph.Location = new System.Drawing.Point(6, 15);
            this.butGraph.Name = "butGraph";
            this.butGraph.Size = new System.Drawing.Size(77, 29);
            this.butGraph.TabIndex = 5;
            this.butGraph.Text = "그래프";
            this.butGraph.UseVisualStyleBackColor = true;
            this.butGraph.Click += new System.EventHandler(this.butGraph_Click);
            // 
            // butAnalysis
            // 
            this.butAnalysis.Location = new System.Drawing.Point(89, 15);
            this.butAnalysis.Name = "butAnalysis";
            this.butAnalysis.Size = new System.Drawing.Size(77, 29);
            this.butAnalysis.TabIndex = 5;
            this.butAnalysis.Text = "분석";
            this.butAnalysis.UseVisualStyleBackColor = true;
            this.butAnalysis.Click += new System.EventHandler(this.butAnalysis_Click);
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(150, 286);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(91, 28);
            this.butOk.TabIndex = 5;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butOption
            // 
            this.butOption.Location = new System.Drawing.Point(172, 15);
            this.butOption.Name = "butOption";
            this.butOption.Size = new System.Drawing.Size(77, 29);
            this.butOption.TabIndex = 5;
            this.butOption.Text = "옵션";
            this.butOption.UseVisualStyleBackColor = true;
            this.butOption.Click += new System.EventHandler(this.butOption_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(263, 286);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(91, 28);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butAnalysisSel
            // 
            this.butAnalysisSel.Location = new System.Drawing.Point(170, 120);
            this.butAnalysisSel.Name = "butAnalysisSel";
            this.butAnalysisSel.Size = new System.Drawing.Size(28, 29);
            this.butAnalysisSel.TabIndex = 5;
            this.butAnalysisSel.Text = ">";
            this.butAnalysisSel.UseVisualStyleBackColor = true;
            this.butAnalysisSel.Click += new System.EventHandler(this.butAnalysisSel_Click);
            // 
            // butAnalysisDeSel
            // 
            this.butAnalysisDeSel.Location = new System.Drawing.Point(170, 155);
            this.butAnalysisDeSel.Name = "butAnalysisDeSel";
            this.butAnalysisDeSel.Size = new System.Drawing.Size(28, 29);
            this.butAnalysisDeSel.TabIndex = 5;
            this.butAnalysisDeSel.Text = "<";
            this.butAnalysisDeSel.UseVisualStyleBackColor = true;
            this.butAnalysisDeSel.Click += new System.EventHandler(this.butAnalysisDeSel_Click);
            // 
            // dgAnalysisList
            // 
            this.dgAnalysisList.AllowUserToAddRows = false;
            this.dgAnalysisList.AllowUserToDeleteRows = false;
            this.dgAnalysisList.AllowUserToResizeColumns = false;
            this.dgAnalysisList.AllowUserToResizeRows = false;
            this.dgAnalysisList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgAnalysisList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgAnalysisList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgAnalysisList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgAnalysisList.ColumnHeadersVisible = false;
            this.dgAnalysisList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COLUMNID,
            this.COLUMNNAME});
            this.dgAnalysisList.Location = new System.Drawing.Point(12, 24);
            this.dgAnalysisList.MultiSelect = false;
            this.dgAnalysisList.Name = "dgAnalysisList";
            this.dgAnalysisList.ReadOnly = true;
            this.dgAnalysisList.RowHeadersVisible = false;
            this.dgAnalysisList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgAnalysisList.RowTemplate.Height = 23;
            this.dgAnalysisList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgAnalysisList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgAnalysisList.Size = new System.Drawing.Size(151, 198);
            this.dgAnalysisList.TabIndex = 4;
            // 
            // COLUMNID
            // 
            this.COLUMNID.HeaderText = "ColumnID";
            this.COLUMNID.Name = "COLUMNID";
            this.COLUMNID.ReadOnly = true;
            this.COLUMNID.Width = 60;
            // 
            // COLUMNNAME
            // 
            this.COLUMNNAME.HeaderText = "ColumnName";
            this.COLUMNNAME.Name = "COLUMNNAME";
            this.COLUMNNAME.ReadOnly = true;
            // 
            // butParaSel
            // 
            this.butParaSel.Location = new System.Drawing.Point(255, 15);
            this.butParaSel.Name = "butParaSel";
            this.butParaSel.Size = new System.Drawing.Size(77, 29);
            this.butParaSel.TabIndex = 5;
            this.butParaSel.Text = "항";
            this.butParaSel.UseVisualStyleBackColor = true;
            this.butParaSel.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butParaSel);
            this.groupBox1.Controls.Add(this.butOption);
            this.groupBox1.Controls.Add(this.butAnalysis);
            this.groupBox1.Controls.Add(this.butGraph);
            this.groupBox1.Location = new System.Drawing.Point(12, 228);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 52);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // lbl_Analysis
            // 
            this.lbl_Analysis.AutoSize = true;
            this.lbl_Analysis.Location = new System.Drawing.Point(202, 105);
            this.lbl_Analysis.Name = "lbl_Analysis";
            this.lbl_Analysis.Size = new System.Drawing.Size(85, 12);
            this.lbl_Analysis.TabIndex = 7;
            this.lbl_Analysis.Text = "반응 데이터 열";
            // 
            // dgAnalsysisSelList
            // 
            this.dgAnalsysisSelList.AllowUserToAddRows = false;
            this.dgAnalsysisSelList.AllowUserToDeleteRows = false;
            this.dgAnalsysisSelList.AllowUserToResizeColumns = false;
            this.dgAnalsysisSelList.AllowUserToResizeRows = false;
            this.dgAnalsysisSelList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgAnalsysisSelList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgAnalsysisSelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgAnalsysisSelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgAnalsysisSelList.ColumnHeadersVisible = false;
            this.dgAnalsysisSelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgAnalsysisSelList.Location = new System.Drawing.Point(204, 120);
            this.dgAnalsysisSelList.MultiSelect = false;
            this.dgAnalsysisSelList.Name = "dgAnalsysisSelList";
            this.dgAnalsysisSelList.ReadOnly = true;
            this.dgAnalsysisSelList.RowHeadersVisible = false;
            this.dgAnalsysisSelList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgAnalsysisSelList.RowTemplate.Height = 23;
            this.dgAnalsysisSelList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgAnalsysisSelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgAnalsysisSelList.Size = new System.Drawing.Size(151, 102);
            this.dgAnalsysisSelList.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ColumnID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ColumnName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dgFactorSelList
            // 
            this.dgFactorSelList.AllowUserToAddRows = false;
            this.dgFactorSelList.AllowUserToDeleteRows = false;
            this.dgFactorSelList.AllowUserToResizeColumns = false;
            this.dgFactorSelList.AllowUserToResizeRows = false;
            this.dgFactorSelList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgFactorSelList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgFactorSelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgFactorSelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgFactorSelList.ColumnHeadersVisible = false;
            this.dgFactorSelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgFactorSelList.Enabled = false;
            this.dgFactorSelList.Location = new System.Drawing.Point(204, 24);
            this.dgFactorSelList.MultiSelect = false;
            this.dgFactorSelList.Name = "dgFactorSelList";
            this.dgFactorSelList.ReadOnly = true;
            this.dgFactorSelList.RowHeadersVisible = false;
            this.dgFactorSelList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgFactorSelList.RowTemplate.Height = 23;
            this.dgFactorSelList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgFactorSelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgFactorSelList.Size = new System.Drawing.Size(151, 78);
            this.dgFactorSelList.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "ColumnID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ColumnName";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // lbl_Factor
            // 
            this.lbl_Factor.AutoSize = true;
            this.lbl_Factor.Enabled = false;
            this.lbl_Factor.Location = new System.Drawing.Point(202, 9);
            this.lbl_Factor.Name = "lbl_Factor";
            this.lbl_Factor.Size = new System.Drawing.Size(85, 12);
            this.lbl_Factor.TabIndex = 7;
            this.lbl_Factor.Text = "요인 데이터 열";
            // 
            // butFactorSel
            // 
            this.butFactorSel.Enabled = false;
            this.butFactorSel.Location = new System.Drawing.Point(170, 24);
            this.butFactorSel.Name = "butFactorSel";
            this.butFactorSel.Size = new System.Drawing.Size(28, 29);
            this.butFactorSel.TabIndex = 5;
            this.butFactorSel.Text = ">";
            this.butFactorSel.UseVisualStyleBackColor = true;
            this.butFactorSel.Click += new System.EventHandler(this.butFactorSel_Click);
            // 
            // butFactorDeSel
            // 
            this.butFactorDeSel.Enabled = false;
            this.butFactorDeSel.Location = new System.Drawing.Point(170, 59);
            this.butFactorDeSel.Name = "butFactorDeSel";
            this.butFactorDeSel.Size = new System.Drawing.Size(28, 29);
            this.butFactorDeSel.TabIndex = 5;
            this.butFactorDeSel.Text = "<";
            this.butFactorDeSel.UseVisualStyleBackColor = true;
            this.butFactorDeSel.Click += new System.EventHandler(this.butFactorDeSel_Click);
            // 
            // DlgTaguchiAnalysis2
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(371, 322);
            this.Controls.Add(this.lbl_Factor);
            this.Controls.Add(this.lbl_Analysis);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.butFactorDeSel);
            this.Controls.Add(this.butAnalysisDeSel);
            this.Controls.Add(this.dgFactorSelList);
            this.Controls.Add(this.butFactorSel);
            this.Controls.Add(this.butAnalysisSel);
            this.Controls.Add(this.dgAnalsysisSelList);
            this.Controls.Add(this.dgAnalysisList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgTaguchiAnalysis2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Taguchi Analysis";
            ((System.ComponentModel.ISupportInitialize)(this.dgAnalysisList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAnalsysisSelList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFactorSelList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butGraph;
        private System.Windows.Forms.Button butAnalysis;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butOption;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butAnalysisSel;
        private System.Windows.Forms.Button butAnalysisDeSel;
        private System.Windows.Forms.DataGridView dgAnalysisList;
        private System.Windows.Forms.Button butParaSel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_Analysis;
        private System.Windows.Forms.DataGridViewTextBoxColumn COLUMNID;
        private System.Windows.Forms.DataGridViewTextBoxColumn COLUMNNAME;
        private System.Windows.Forms.DataGridView dgAnalsysisSelList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView dgFactorSelList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Label lbl_Factor;
        private System.Windows.Forms.Button butFactorSel;
        private System.Windows.Forms.Button butFactorDeSel;
    }
}