namespace DACrux.TEST.ENGUI
{
    partial class DlgSetupMap
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaxY = new System.Windows.Forms.TextBox();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.txtMinY = new System.Windows.Forms.TextBox();
            this.txtYCnt = new System.Windows.Forms.TextBox();
            this.txtXCnt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butApply = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(29, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 21);
            this.label3.TabIndex = 8;
            this.label3.Text = "Y Grid";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(130, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "~";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(130, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(29, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 21);
            this.label1.TabIndex = 11;
            this.label1.Text = "X Grid";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaxY
            // 
            this.txtMaxY.Location = new System.Drawing.Point(152, 46);
            this.txtMaxY.Name = "txtMaxY";
            this.txtMaxY.Size = new System.Drawing.Size(45, 21);
            this.txtMaxY.TabIndex = 4;
            this.txtMaxY.TextChanged += new System.EventHandler(this.txtBoxTextChanged);
            this.txtMaxY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // txtMinX
            // 
            this.txtMinX.Location = new System.Drawing.Point(83, 22);
            this.txtMinX.Name = "txtMinX";
            this.txtMinX.Size = new System.Drawing.Size(45, 21);
            this.txtMinX.TabIndex = 1;
            this.txtMinX.TextChanged += new System.EventHandler(this.txtBoxTextChanged);
            this.txtMinX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // txtMaxX
            // 
            this.txtMaxX.Location = new System.Drawing.Point(152, 22);
            this.txtMaxX.Name = "txtMaxX";
            this.txtMaxX.Size = new System.Drawing.Size(45, 21);
            this.txtMaxX.TabIndex = 2;
            this.txtMaxX.TextChanged += new System.EventHandler(this.txtBoxTextChanged);
            this.txtMaxX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // txtMinY
            // 
            this.txtMinY.Location = new System.Drawing.Point(83, 46);
            this.txtMinY.Name = "txtMinY";
            this.txtMinY.Size = new System.Drawing.Size(45, 21);
            this.txtMinY.TabIndex = 3;
            this.txtMinY.TextChanged += new System.EventHandler(this.txtBoxTextChanged);
            this.txtMinY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // txtYCnt
            // 
            this.txtYCnt.BackColor = System.Drawing.Color.Silver;
            this.txtYCnt.Location = new System.Drawing.Point(203, 46);
            this.txtYCnt.Name = "txtYCnt";
            this.txtYCnt.ReadOnly = true;
            this.txtYCnt.Size = new System.Drawing.Size(45, 21);
            this.txtYCnt.TabIndex = 13;
            // 
            // txtXCnt
            // 
            this.txtXCnt.BackColor = System.Drawing.Color.Silver;
            this.txtXCnt.Location = new System.Drawing.Point(203, 22);
            this.txtXCnt.Name = "txtXCnt";
            this.txtXCnt.ReadOnly = true;
            this.txtXCnt.Size = new System.Drawing.Size(45, 21);
            this.txtXCnt.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtYCnt);
            this.groupBox1.Controls.Add(this.txtXCnt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMaxY);
            this.groupBox1.Controls.Add(this.txtMinX);
            this.groupBox1.Controls.Add(this.txtMaxX);
            this.groupBox1.Controls.Add(this.txtMinY);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(323, 78);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Die Virtual Grid in Wafer";
            // 
            // butApply
            // 
            this.butApply.Location = new System.Drawing.Point(88, 94);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(75, 32);
            this.butApply.TabIndex = 0;
            this.butApply.Text = "Apply";
            this.butApply.UseVisualStyleBackColor = true;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(169, 94);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 32);
            this.butOK.TabIndex = 1;
            this.butOK.Text = "Ok";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(250, 94);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 32);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // DlgSetupMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 134);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butApply);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgSetupMap";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Map Grid";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaxY;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.TextBox txtMinY;
        private System.Windows.Forms.TextBox txtYCnt;
        private System.Windows.Forms.TextBox txtXCnt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butApply;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
    }
}