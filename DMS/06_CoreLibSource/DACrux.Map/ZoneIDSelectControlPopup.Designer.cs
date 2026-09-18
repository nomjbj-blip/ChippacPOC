namespace DACrux.Map
{
    partial class ZoneIDSelectControlPopup
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
            this.zoneIdCtl = new DACrux.Map.ZoneIDSelectControl();
            this.SuspendLayout();
            // 
            // zoneIdCtl
            // 
            this.zoneIdCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoneIdCtl.Location = new System.Drawing.Point(0, 0);
            this.zoneIdCtl.Map = null;
            this.zoneIdCtl.Name = "zoneIdCtl";
            this.zoneIdCtl.Size = new System.Drawing.Size(82, 366);
            this.zoneIdCtl.TabIndex = 0;
            // 
            // ZoneIDSelectControlPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(82, 366);
            this.Controls.Add(this.zoneIdCtl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ZoneIDSelectControlPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ZoneID";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ZoneIDSelectControlPopup_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DACrux.Map.ZoneIDSelectControl zoneIdCtl;
    }
}