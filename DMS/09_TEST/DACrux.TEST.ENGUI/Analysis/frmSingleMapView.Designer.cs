namespace DACrux.TEST.ENGUI
{
    partial class frmSingleMapView
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
            this.tpucMapView1 = new DACrux.MapAnalysis.Control.TPUCMapView();
            this.SuspendLayout();
            // 
            // tpucMapView1
            // 
            this.tpucMapView1.BinSelectEnable = true;
            this.tpucMapView1.BinSelectVisuble = false;
            this.tpucMapView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpucMapView1.Location = new System.Drawing.Point(0, 0);
            this.tpucMapView1.Name = "tpucMapView1";
            this.tpucMapView1.ShowBinChange = false;
            this.tpucMapView1.ShowLowYield = false;
            this.tpucMapView1.Size = new System.Drawing.Size(1036, 642);
            this.tpucMapView1.TabIndex = 0;
            this.tpucMapView1.OnDutAnalysis += new DACrux.MapAnalysis.Control.TPUCMapView.DutAnalysis(this.tpucMapView1_OnDutAnalysis);
            this.tpucMapView1.OnParaAnalysis += new DACrux.MapAnalysis.Control.TPUCMapView.ParaAnalysis(this.tpucMapView1_OnParaAnalysis);
            // 
            // frmSingleMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 642);
            this.Controls.Add(this.tpucMapView1);
            this.Name = "frmSingleMapView";
            this.Text = "frmSingleMapView";
            this.Load += new System.EventHandler(this.frmSingleMapView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MapAnalysis.Control.TPUCMapView tpucMapView1;

    }
}