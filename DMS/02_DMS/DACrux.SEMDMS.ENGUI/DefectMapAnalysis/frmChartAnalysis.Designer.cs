namespace DACrux.SEMDMS.ENGUI
{
    partial class frmChartAnalysis
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
            this.dpucChartAnalysis1 = new DACrux.SEMDMS.Control.DPUCChartAnalysis();
            this.SuspendLayout();
            // 
            // dpucChartAnalysis1
            // 
            this.dpucChartAnalysis1.DataSource = null;
            this.dpucChartAnalysis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucChartAnalysis1.Location = new System.Drawing.Point(0, 0);
            this.dpucChartAnalysis1.Name = "dpucChartAnalysis1";
            this.dpucChartAnalysis1.Size = new System.Drawing.Size(784, 561);
            this.dpucChartAnalysis1.TabIndex = 0;
            // 
            // frmChartAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.dpucChartAnalysis1);
            this.Name = "frmChartAnalysis";
            this.Text = "Chart Analysis";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmChartAnalysis_FormClosed);
            this.Load += new System.EventHandler(this.frmChartAnalysis_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.DPUCChartAnalysis dpucChartAnalysis1;
    }
}