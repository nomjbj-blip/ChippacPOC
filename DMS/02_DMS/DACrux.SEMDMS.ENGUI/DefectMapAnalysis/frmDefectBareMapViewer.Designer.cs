namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectBareMapViewer
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
            this.dpucBareDefecMapViewers = new DACrux.SEMDMS.Control.DPUCBareDefecMapViewer();
            this.SuspendLayout();
            // 
            // dpucBareDefecMapViewers
            // 
            this.dpucBareDefecMapViewers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucBareDefecMapViewers.Location = new System.Drawing.Point(0, 0);
            this.dpucBareDefecMapViewers.Name = "dpucBareDefecMapViewers";
            this.dpucBareDefecMapViewers.Size = new System.Drawing.Size(879, 519);
            this.dpucBareDefecMapViewers.TabIndex = 0;
            this.dpucBareDefecMapViewers.Wafer = null;
            // 
            // frmDefectBareMapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 519);
            this.Controls.Add(this.dpucBareDefecMapViewers);
            this.Name = "frmDefectBareMapViewer";
            this.Text = "Defect Map Analysis";
            this.Load += new System.EventHandler(this.frmDefectBareMapViewercs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.DPUCBareDefecMapViewer dpucBareDefecMapViewers;

    }
}