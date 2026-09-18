namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectMapViewer
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
            this.map = new DACrux.SEMDMS.Control.DPUCDefecMapViewer();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(879, 519);
            this.map.TabIndex = 27;
            this.map.Wafer = null;
            // 
            // frmDefectMapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 519);
            this.Controls.Add(this.map);
            this.Name = "frmDefectMapViewer";
            this.Text = "Defect Map Analysis";
            this.Load += new System.EventHandler(this.frmDefectMapViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.DPUCDefecMapViewer map;
    }
}