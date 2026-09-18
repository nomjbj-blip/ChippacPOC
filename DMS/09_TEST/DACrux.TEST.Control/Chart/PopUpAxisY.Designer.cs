namespace DACrux.TEST.Control
{
    partial class PopUpAxisY
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopUpAxisY));
            this.butClose = new System.Windows.Forms.Button();
            this.butApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtY1Max = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtY1Min = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtY1Interval = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TxtY2Max = new System.Windows.Forms.TextBox();
            this.TxtY2Interval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtY2Min = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.BackColor = System.Drawing.Color.White;
            this.butClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.butClose.Image = ((System.Drawing.Image)(resources.GetObject("butClose.Image")));
            this.butClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butClose.Location = new System.Drawing.Point(332, 5);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(95, 35);
            this.butClose.TabIndex = 167;
            this.butClose.Text = "   Close";
            this.butClose.UseVisualStyleBackColor = false;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butApply
            // 
            this.butApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butApply.BackColor = System.Drawing.Color.White;
            this.butApply.ForeColor = System.Drawing.SystemColors.ControlText;
            this.butApply.Image = ((System.Drawing.Image)(resources.GetObject("butApply.Image")));
            this.butApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butApply.Location = new System.Drawing.Point(231, 5);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(95, 35);
            this.butApply.TabIndex = 153;
            this.butApply.Text = "        Apply";
            this.butApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butApply.UseVisualStyleBackColor = false;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Max";
            // 
            // TxtY1Max
            // 
            this.TxtY1Max.Location = new System.Drawing.Point(70, 20);
            this.TxtY1Max.Name = "TxtY1Max";
            this.TxtY1Max.Size = new System.Drawing.Size(130, 21);
            this.TxtY1Max.TabIndex = 3;
            this.TxtY1Max.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Min";
            // 
            // TxtY1Min
            // 
            this.TxtY1Min.Location = new System.Drawing.Point(70, 49);
            this.TxtY1Min.Name = "TxtY1Min";
            this.TxtY1Min.Size = new System.Drawing.Size(130, 21);
            this.TxtY1Min.TabIndex = 3;
            this.TxtY1Min.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Interval";
            // 
            // TxtY1Interval
            // 
            this.TxtY1Interval.Location = new System.Drawing.Point(70, 76);
            this.TxtY1Interval.Name = "TxtY1Interval";
            this.TxtY1Interval.Size = new System.Drawing.Size(130, 21);
            this.TxtY1Interval.TabIndex = 3;
            this.TxtY1Interval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtY1Max);
            this.groupBox2.Controls.Add(this.TxtY1Interval);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TxtY1Min);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 134);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y1 축";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TxtY2Max);
            this.groupBox3.Controls.Add(this.TxtY2Interval);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.TxtY2Min);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(215, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 134);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Y2 축";
            // 
            // TxtY2Max
            // 
            this.TxtY2Max.Location = new System.Drawing.Point(70, 20);
            this.TxtY2Max.Name = "TxtY2Max";
            this.TxtY2Max.Size = new System.Drawing.Size(130, 21);
            this.TxtY2Max.TabIndex = 3;
            // 
            // TxtY2Interval
            // 
            this.TxtY2Interval.Location = new System.Drawing.Point(70, 76);
            this.TxtY2Interval.Name = "TxtY2Interval";
            this.TxtY2Interval.Size = new System.Drawing.Size(130, 21);
            this.TxtY2Interval.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Max";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Interval";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "Min";
            // 
            // TxtY2Min
            // 
            this.TxtY2Min.Location = new System.Drawing.Point(70, 49);
            this.TxtY2Min.Name = "TxtY2Min";
            this.TxtY2Min.Size = new System.Drawing.Size(130, 21);
            this.TxtY2Min.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butClose);
            this.panel1.Controls.Add(this.butApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 134);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 43);
            this.panel1.TabIndex = 6;
            // 
            // PopUpAxisY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 177);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(245, 170);
            this.Name = "PopUpAxisY";
            this.Text = "Y Axis Max/Min";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtY1Max;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtY1Min;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtY1Interval;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TxtY2Max;
        private System.Windows.Forms.TextBox TxtY2Interval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtY2Min;
        private System.Windows.Forms.Panel panel1;
    }
}