namespace DACrux.TEST.Control
{
    partial class ImagePopUp
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
            this.ImageControl = new DACrux.TEST.Control.ImageControl();
            this.SuspendLayout();
            // 
            // ImageControl
            // 
            this.ImageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageControl.DefectImg = null;
            this.ImageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageControl.Location = new System.Drawing.Point(0, 0);
            this.ImageControl.Name = "ImageControl";
            this.ImageControl.Size = new System.Drawing.Size(784, 537);
            this.ImageControl.TabIndex = 0;
            // 
            // ImagePopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 537);
            this.Controls.Add(this.ImageControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "ImagePopUp";
            this.Text = "ImagePopUp";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ImagePopUp_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageControl ImageControl;
    }
}