namespace DACrux.SEMDMS.ENGUI
{
    partial class frmChartAnalysisNew
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
            this.dpucChartAnal = new DACrux.SEMDMS.Control.DPUCChartAnalysisDefectList();
            this.SuspendLayout();
            // 
            // dpucChartAnal
            // 
            this.dpucChartAnal.DataSource = null;
            this.dpucChartAnal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucChartAnal.Location = new System.Drawing.Point(0, 0);
            this.dpucChartAnal.Name = "dpucChartAnal";
            this.dpucChartAnal.Size = new System.Drawing.Size(784, 561);
            this.dpucChartAnal.TabIndex = 0;
            // 
            // frmChartAnalysisNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.dpucChartAnal);
            this.Name = "frmChartAnalysisNew";
            this.Text = "Chart Analysis";
            this.ResumeLayout(false);

        }

        #endregion

        private Control.DPUCChartAnalysisDefectList dpucChartAnal;
    }
}