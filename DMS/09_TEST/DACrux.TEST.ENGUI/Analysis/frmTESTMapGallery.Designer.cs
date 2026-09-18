namespace DACrux.TEST.ENGUI
{
    partial class frmTESTMapGallery
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
            this.tpucMapGallery1 = new DACrux.MapAnalysis.Control.TPUCMapGallery();
            this.SuspendLayout();
            // 
            // tpucMapGallery1
            // 
            this.tpucMapGallery1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpucMapGallery1.Location = new System.Drawing.Point(0, 0);
            this.tpucMapGallery1.Name = "tpucMapGallery1";
            this.tpucMapGallery1.Size = new System.Drawing.Size(1036, 642);
            this.tpucMapGallery1.TabIndex = 0;
            this.tpucMapGallery1.WaferInfo = null;
            this.tpucMapGallery1.OnParaAnalysis += new DACrux.MapAnalysis.Control.TPUCMapGallery.ParaAnalysis(this.tpucMapGallery1_OnParaAnalysis);
            // 
            // frmPCMMapGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 642);
            this.Controls.Add(this.tpucMapGallery1);
            this.Name = "frmPCMMapGallery";
            this.Text = "frmMapGallery";
            this.ResumeLayout(false);

        }

        #endregion

        private MapAnalysis.Control.TPUCMapGallery tpucMapGallery1;


    }
}