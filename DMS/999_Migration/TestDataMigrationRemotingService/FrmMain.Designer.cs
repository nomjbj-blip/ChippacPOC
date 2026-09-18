namespace TestDataMigrationRemotingService
{
    partial class FrmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoFAB2 = new System.Windows.Forms.RadioButton();
            this.rdoFab1 = new System.Windows.Forms.RadioButton();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoFAB2);
            this.groupBox1.Controls.Add(this.rdoFab1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 42);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Factory";
            // 
            // rdoFAB2
            // 
            this.rdoFAB2.AutoSize = true;
            this.rdoFAB2.Location = new System.Drawing.Point(64, 20);
            this.rdoFAB2.Name = "rdoFAB2";
            this.rdoFAB2.Size = new System.Drawing.Size(52, 16);
            this.rdoFAB2.TabIndex = 1;
            this.rdoFAB2.TabStop = true;
            this.rdoFAB2.Text = "FAB2";
            this.rdoFAB2.UseVisualStyleBackColor = true;
            // 
            // rdoFab1
            // 
            this.rdoFab1.AutoSize = true;
            this.rdoFab1.Location = new System.Drawing.Point(6, 20);
            this.rdoFab1.Name = "rdoFab1";
            this.rdoFab1.Size = new System.Drawing.Size(52, 16);
            this.rdoFab1.TabIndex = 0;
            this.rdoFab1.TabStop = true;
            this.rdoFab1.Text = "FAB1";
            this.rdoFab1.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(159, 29);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 116);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmMain";
            this.Text = "Remoting Service";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoFAB2;
        private System.Windows.Forms.RadioButton rdoFab1;
        private System.Windows.Forms.Button btnRun;
    }
}