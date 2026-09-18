namespace DACrux.SP.Controls
{
    partial class TitlePanel
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitlePanel));
            this.uclTitle1 = new DACrux.SP.Controls.uclTitle();
            this.SuspendLayout();
            // 
            // uclTitle1
            // 
            this.uclTitle1.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle1.BackgroundImage")));
            this.uclTitle1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle1.EndColor = System.Drawing.Color.White;
            this.uclTitle1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle1.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle1.Location = new System.Drawing.Point(0, 0);
            this.uclTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle1.Name = "uclTitle1";
            this.uclTitle1.Size = new System.Drawing.Size(100, 20);
            this.uclTitle1.StartColor = System.Drawing.Color.SkyBlue;
            this.uclTitle1.TabIndex = 0;
            this.uclTitle1.Title = "Title";
            // 
            // TitlePanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Size = new System.Drawing.Size(100, 250);
            this.Controls.Add(this.uclTitle1);
            //this.ResumeLayout(false);

        }

        #endregion

        private uclTitle uclTitle1;
    }
}
