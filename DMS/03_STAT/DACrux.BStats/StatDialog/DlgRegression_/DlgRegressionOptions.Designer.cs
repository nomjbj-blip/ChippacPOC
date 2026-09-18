namespace DACrux.BStats.StatDialog.DlgRegression_
{
    partial class DlgRegressionOptions
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgRegressionOptions));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.chkAnova = new System.Windows.Forms.CheckBox();
            this.chkTableofCoefficients = new System.Windows.Forms.CheckBox();
            this.chkRsquared = new System.Windows.Forms.CheckBox();
            this.chkTableofResiduals = new System.Windows.Forms.CheckBox();
            this.grpGraph = new System.Windows.Forms.GroupBox();
            this.chkNormalplot = new System.Windows.Forms.CheckBox();
            this.chkHistogramofResiduals = new System.Windows.Forms.CheckBox();
            this.chkResidualsVsFits = new System.Windows.Forms.CheckBox();
            this.chkResidualsVsOrder = new System.Windows.Forms.CheckBox();
            this.lblGraphVariables = new System.Windows.Forms.Label();
            this.btnLeft_Graph = new System.Windows.Forms.Button();
            this.btnRight_Graph = new System.Windows.Forms.Button();
            this.lvGraphVariables = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = new System.Windows.Forms.ColumnHeader("(none)");
            this.colID = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.grpGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.grpResults);
            this.pnlBackground.Controls.Add(this.grpGraph);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(364, 496);
            this.pnlBackground.TabIndex = 1;
            // 
            // grpResults
            // 
            this.grpResults.Controls.Add(this.chkAnova);
            this.grpResults.Controls.Add(this.chkTableofCoefficients);
            this.grpResults.Controls.Add(this.chkRsquared);
            this.grpResults.Controls.Add(this.chkTableofResiduals);
            this.grpResults.Location = new System.Drawing.Point(19, 324);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(332, 118);
            this.grpResults.TabIndex = 33;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Results";
            // 
            // chkAnova
            // 
            this.chkAnova.AutoSize = true;
            this.chkAnova.Checked = true;
            this.chkAnova.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnova.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAnova.Location = new System.Drawing.Point(13, 42);
            this.chkAnova.Name = "chkAnova";
            this.chkAnova.Size = new System.Drawing.Size(119, 17);
            this.chkAnova.TabIndex = 31;
            this.chkAnova.Text = "&Analysis of variance";
            this.chkAnova.UseVisualStyleBackColor = true;
            // 
            // chkTableofCoefficients
            // 
            this.chkTableofCoefficients.AutoSize = true;
            this.chkTableofCoefficients.Checked = true;
            this.chkTableofCoefficients.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTableofCoefficients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTableofCoefficients.Location = new System.Drawing.Point(13, 19);
            this.chkTableofCoefficients.Name = "chkTableofCoefficients";
            this.chkTableofCoefficients.Size = new System.Drawing.Size(120, 17);
            this.chkTableofCoefficients.TabIndex = 28;
            this.chkTableofCoefficients.Text = "Table of &coefficients";
            this.chkTableofCoefficients.UseVisualStyleBackColor = true;
            // 
            // chkRsquared
            // 
            this.chkRsquared.AutoSize = true;
            this.chkRsquared.Checked = true;
            this.chkRsquared.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRsquared.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRsquared.Location = new System.Drawing.Point(13, 66);
            this.chkRsquared.Name = "chkRsquared";
            this.chkRsquared.Size = new System.Drawing.Size(73, 17);
            this.chkRsquared.TabIndex = 29;
            this.chkRsquared.Text = "&R-squared";
            this.chkRsquared.UseVisualStyleBackColor = true;
            // 
            // chkTableofResiduals
            // 
            this.chkTableofResiduals.AutoSize = true;
            this.chkTableofResiduals.Checked = true;
            this.chkTableofResiduals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTableofResiduals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTableofResiduals.Location = new System.Drawing.Point(13, 90);
            this.chkTableofResiduals.Name = "chkTableofResiduals";
            this.chkTableofResiduals.Size = new System.Drawing.Size(146, 17);
            this.chkTableofResiduals.TabIndex = 30;
            this.chkTableofResiduals.Text = "Table of fits &and residuals";
            this.chkTableofResiduals.UseVisualStyleBackColor = true;
            // 
            // grpGraph
            // 
            this.grpGraph.Controls.Add(this.chkNormalplot);
            this.grpGraph.Controls.Add(this.chkHistogramofResiduals);
            this.grpGraph.Controls.Add(this.chkResidualsVsFits);
            this.grpGraph.Controls.Add(this.chkResidualsVsOrder);
            this.grpGraph.Controls.Add(this.lblGraphVariables);
            this.grpGraph.Controls.Add(this.btnLeft_Graph);
            this.grpGraph.Controls.Add(this.btnRight_Graph);
            this.grpGraph.Controls.Add(this.lvGraphVariables);
            this.grpGraph.Controls.Add(this.lblColumns);
            this.grpGraph.Controls.Add(this.lvColumns);
            this.grpGraph.Location = new System.Drawing.Point(19, 13);
            this.grpGraph.Name = "grpGraph";
            this.grpGraph.Size = new System.Drawing.Size(332, 305);
            this.grpGraph.TabIndex = 32;
            this.grpGraph.TabStop = false;
            this.grpGraph.Text = "Graph";
            // 
            // chkNormalplot
            // 
            this.chkNormalplot.AutoSize = true;
            this.chkNormalplot.Checked = true;
            this.chkNormalplot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalplot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNormalplot.Location = new System.Drawing.Point(169, 65);
            this.chkNormalplot.Name = "chkNormalplot";
            this.chkNormalplot.Size = new System.Drawing.Size(135, 17);
            this.chkNormalplot.TabIndex = 71;
            this.chkNormalplot.Text = "&Normal plot of residuals";
            this.chkNormalplot.UseVisualStyleBackColor = true;
            // 
            // chkHistogramofResiduals
            // 
            this.chkHistogramofResiduals.AutoSize = true;
            this.chkHistogramofResiduals.Checked = true;
            this.chkHistogramofResiduals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHistogramofResiduals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHistogramofResiduals.Location = new System.Drawing.Point(169, 41);
            this.chkHistogramofResiduals.Name = "chkHistogramofResiduals";
            this.chkHistogramofResiduals.Size = new System.Drawing.Size(129, 17);
            this.chkHistogramofResiduals.TabIndex = 68;
            this.chkHistogramofResiduals.Text = "&Histogram of residuals";
            this.chkHistogramofResiduals.UseVisualStyleBackColor = true;
            // 
            // chkResidualsVsFits
            // 
            this.chkResidualsVsFits.AutoSize = true;
            this.chkResidualsVsFits.Checked = true;
            this.chkResidualsVsFits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResidualsVsFits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkResidualsVsFits.Location = new System.Drawing.Point(169, 89);
            this.chkResidualsVsFits.Name = "chkResidualsVsFits";
            this.chkResidualsVsFits.Size = new System.Drawing.Size(121, 17);
            this.chkResidualsVsFits.TabIndex = 69;
            this.chkResidualsVsFits.Text = "Residuals versus &fits";
            this.chkResidualsVsFits.UseVisualStyleBackColor = true;
            // 
            // chkResidualsVsOrder
            // 
            this.chkResidualsVsOrder.AutoSize = true;
            this.chkResidualsVsOrder.Checked = true;
            this.chkResidualsVsOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResidualsVsOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkResidualsVsOrder.Location = new System.Drawing.Point(169, 112);
            this.chkResidualsVsOrder.Name = "chkResidualsVsOrder";
            this.chkResidualsVsOrder.Size = new System.Drawing.Size(132, 17);
            this.chkResidualsVsOrder.TabIndex = 70;
            this.chkResidualsVsOrder.Text = "Residuals &versus order";
            this.chkResidualsVsOrder.UseVisualStyleBackColor = true;
            // 
            // lblGraphVariables
            // 
            this.lblGraphVariables.AutoSize = true;
            this.lblGraphVariables.Location = new System.Drawing.Point(167, 150);
            this.lblGraphVariables.Name = "lblGraphVariables";
            this.lblGraphVariables.Size = new System.Drawing.Size(132, 13);
            this.lblGraphVariables.TabIndex = 67;
            this.lblGraphVariables.Text = "Residuals VS the variables";
            // 
            // btnLeft_Graph
            // 
            this.btnLeft_Graph.Location = new System.Drawing.Point(147, 188);
            this.btnLeft_Graph.Name = "btnLeft_Graph";
            this.btnLeft_Graph.Size = new System.Drawing.Size(17, 20);
            this.btnLeft_Graph.TabIndex = 66;
            this.btnLeft_Graph.Text = "◄";
            this.btnLeft_Graph.UseCompatibleTextRendering = true;
            this.btnLeft_Graph.UseVisualStyleBackColor = true;
            // 
            // btnRight_Graph
            // 
            this.btnRight_Graph.Location = new System.Drawing.Point(147, 167);
            this.btnRight_Graph.Name = "btnRight_Graph";
            this.btnRight_Graph.Size = new System.Drawing.Size(17, 20);
            this.btnRight_Graph.TabIndex = 65;
            this.btnRight_Graph.Text = "►";
            this.btnRight_Graph.UseCompatibleTextRendering = true;
            this.btnRight_Graph.UseVisualStyleBackColor = true;
            // 
            // lvGraphVariables
            // 
            this.lvGraphVariables.AllowDrop = true;
            this.lvGraphVariables.BackColor = System.Drawing.Color.Lavender;
            this.lvGraphVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvGraphVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvGraphVariables.FullRowSelect = true;
            this.lvGraphVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvGraphVariables.Location = new System.Drawing.Point(169, 167);
            this.lvGraphVariables.Name = "lvGraphVariables";
            this.lvGraphVariables.Size = new System.Drawing.Size(149, 119);
            this.lvGraphVariables.TabIndex = 64;
            this.lvGraphVariables.UseCompatibleStateImageBehavior = false;
            this.lvGraphVariables.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 20;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Width = 70;
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(17, 21);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 63;
            this.lblColumns.Text = "Columns";
            // 
            // lvColumns
            // 
            this.lvColumns.AllowDrop = true;
            this.lvColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colID,
            this.colName});
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumns.Location = new System.Drawing.Point(17, 39);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(125, 244);
            this.lvColumns.TabIndex = 26;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 20;
            // 
            // colID
            // 
            this.colID.Text = "ID";
            this.colID.Width = 50;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 70;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(289, 460);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(225, 460);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // DlgRegressionOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 496);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgRegressionOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgRegressionOptions";
            this.pnlBackground.ResumeLayout(false);
            this.grpResults.ResumeLayout(false);
            this.grpResults.PerformLayout();
            this.grpGraph.ResumeLayout(false);
            this.grpGraph.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.CheckBox chkTableofResiduals;
        private System.Windows.Forms.CheckBox chkRsquared;
        private System.Windows.Forms.CheckBox chkTableofCoefficients;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkAnova;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.GroupBox grpGraph;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.Label lblGraphVariables;
        private System.Windows.Forms.Button btnLeft_Graph;
        private System.Windows.Forms.Button btnRight_Graph;
        private System.Windows.Forms.ListView lvGraphVariables;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.CheckBox chkNormalplot;
        private System.Windows.Forms.CheckBox chkHistogramofResiduals;
        private System.Windows.Forms.CheckBox chkResidualsVsFits;
        private System.Windows.Forms.CheckBox chkResidualsVsOrder;
        private System.Windows.Forms.ImageList imlColumnType;
    }
}