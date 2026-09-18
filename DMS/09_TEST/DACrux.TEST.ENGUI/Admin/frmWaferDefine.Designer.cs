namespace DACrux.TEST.ENGUI
{
    partial class frmWaferDefine
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
            this.tpucMapConfigs = new DACrux.MapAnalysis.Control.TPUCMapConfig();
            this.SuspendLayout();
            // 
            // tpucMapConfigs
            // 
            this.tpucMapConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpucMapConfigs.Location = new System.Drawing.Point(0, 0);
            this.tpucMapConfigs.Name = "tpucMapConfigs";
            this.tpucMapConfigs.Size = new System.Drawing.Size(1036, 642);
            this.tpucMapConfigs.TabIndex = 0;
            // 
            // frmWaferDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 642);
            this.Controls.Add(this.tpucMapConfigs);
            this.Name = "frmWaferDefine";
            this.Text = "frmWaferDefine";
            this.ResumeLayout(false);

        }

        #endregion

        private MapAnalysis.Control.TPUCMapConfig tpucMapConfigs;



    }
}