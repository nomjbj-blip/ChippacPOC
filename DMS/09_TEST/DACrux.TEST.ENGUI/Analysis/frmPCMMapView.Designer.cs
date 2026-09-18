namespace DACrux.TEST.ENGUI
{
    partial class frmPCMMapView
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
            this.tpucpcmViewer = new DACrux.TEST.Control.TPUCPCMView();
            this.SuspendLayout();
            // 
            // tpucpcmViewer
            // 
            this.tpucpcmViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpucpcmViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpucpcmViewer.Location = new System.Drawing.Point(0, 0);
            this.tpucpcmViewer.Name = "tpucpcmViewer";
            this.tpucpcmViewer.Size = new System.Drawing.Size(1036, 642);
            this.tpucpcmViewer.TabIndex = 0;
            // 
            // frmPCMMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 642);
            this.Controls.Add(this.tpucpcmViewer);
            this.Name = "frmPCMMapView";
            this.Text = "frmPCMMapView";
            this.Load += new System.EventHandler(this.frmPCMMapView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.TPUCPCMView tpucpcmViewer;


    }
}