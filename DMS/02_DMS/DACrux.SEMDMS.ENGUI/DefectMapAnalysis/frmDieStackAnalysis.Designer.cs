namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDieStackAnalysis
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
            this.map = new DACrux.SEMDMS.Control.DPUCDieStackViewer();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "panel1";
            this.map.Size = new System.Drawing.Size(853, 526);
            this.map.TabIndex = 0;
            // 
            // frmDieStackAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 526);
            this.Controls.Add(this.map);
            this.Name = "frmDieStackAnalysis";
            this.Text = "frmDieStackAnalysis";
            this.Load += new System.EventHandler(this.frmDieStackAnalysis_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DACrux.SEMDMS.Control.DPUCDieStackViewer map;
    }
}