namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectImageGallery
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
            this.dpucImageGallery1 = new DACrux.SEMDMS.Control.DPUCImageGalleryGrid();
            this.SuspendLayout();
            // 
            // dpucImageGallery1
            // 
            this.dpucImageGallery1.ImageDataSource = null;
            this.dpucImageGallery1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucImageGallery1.Location = new System.Drawing.Point(0, 0);
            this.dpucImageGallery1.Name = "dpucImageGallery1";
            this.dpucImageGallery1.Size = new System.Drawing.Size(784, 561);
            this.dpucImageGallery1.TabIndex = 0;
            // 
            // frmDefectImageGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.dpucImageGallery1);
            this.Name = "frmDefectImageGallery";
            this.Text = "Defect Image Gallery";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDefectImageGallery_FormClosed);
            this.Load += new System.EventHandler(this.frmDefectImageGallery_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DACrux.SEMDMS.Control.DPUCImageGalleryGrid dpucImageGallery1;

    }
}