namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectAlarmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectAlarmReport));
            this.panel2 = new System.Windows.Forms.Panel();
            this.numDataCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cboStep = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboEquip = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDefectType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.FlowLayoutPanel();
            this.numChartCount = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartCount)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numChartCount);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.numDataCount);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cboStep);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cboEquip);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cboProduct);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboDefectType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(995, 65);
            this.panel2.TabIndex = 58;
            // 
            // numDataCount
            // 
            this.numDataCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numDataCount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDataCount.Location = new System.Drawing.Point(939, 38);
            this.numDataCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numDataCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDataCount.Name = "numDataCount";
            this.numDataCount.Size = new System.Drawing.Size(42, 21);
            this.numDataCount.TabIndex = 223;
            this.numDataCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numDataCount.ValueChanged += new System.EventHandler(this.numDataCount_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(866, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 12);
            this.label5.TabIndex = 224;
            this.label5.Text = "Data Count";
            // 
            // cboStep
            // 
            this.cboStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStep.Location = new System.Drawing.Point(696, 10);
            this.cboStep.Name = "cboStep";
            this.cboStep.Size = new System.Drawing.Size(121, 20);
            this.cboStep.TabIndex = 215;
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(647, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 21);
            this.label3.TabIndex = 214;
            this.label3.Text = "   Step";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboEquip
            // 
            this.cboEquip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEquip.Location = new System.Drawing.Point(328, 9);
            this.cboEquip.Name = "cboEquip";
            this.cboEquip.Size = new System.Drawing.Size(106, 20);
            this.cboEquip.TabIndex = 212;
            // 
            // label4
            // 
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(259, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 21);
            this.label4.TabIndex = 211;
            this.label4.Text = "   Equip ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboProduct
            // 
            this.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduct.Location = new System.Drawing.Point(513, 9);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(121, 20);
            this.cboProduct.TabIndex = 212;
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(444, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 21);
            this.label2.TabIndex = 211;
            this.label2.Text = "   Product";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDefectType
            // 
            this.cboDefectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefectType.Location = new System.Drawing.Point(109, 9);
            this.cboDefectType.Name = "cboDefectType";
            this.cboDefectType.Size = new System.Drawing.Size(143, 20);
            this.cboDefectType.TabIndex = 212;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 21);
            this.label1.TabIndex = 211;
            this.label1.Text = "   Defect Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(891, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 24);
            this.btnSearch.TabIndex = 217;
            this.btnSearch.Text = "     Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panel.Location = new System.Drawing.Point(0, 65);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(995, 462);
            this.panel.TabIndex = 59;
            this.panel.WrapContents = false;
            this.panel.SizeChanged += new System.EventHandler(this.panel_SizeChanged);
            // 
            // numChartCount
            // 
            this.numChartCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numChartCount.Location = new System.Drawing.Point(800, 38);
            this.numChartCount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numChartCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numChartCount.Name = "numChartCount";
            this.numChartCount.Size = new System.Drawing.Size(42, 21);
            this.numChartCount.TabIndex = 225;
            this.numChartCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numChartCount.ValueChanged += new System.EventHandler(this.numChartCount_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(727, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 12);
            this.label6.TabIndex = 226;
            this.label6.Text = "Chart Count";
            // 
            // frmDefectAlarmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 527);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.panel2);
            this.Name = "frmDefectAlarmReport";
            this.Text = "DM Alarm Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDefectAlarmReport_FormClosing);
            this.Load += new System.EventHandler(this.frmDefectAlarmReport_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cboStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDefectType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboEquip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numDataCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel panel;
        private System.Windows.Forms.NumericUpDown numChartCount;
        private System.Windows.Forms.Label label6;
    }
}