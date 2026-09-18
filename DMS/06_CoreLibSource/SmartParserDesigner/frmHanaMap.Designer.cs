namespace SmartParser.Designer
{
    partial class frmHanaMap
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
            this.richHana = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richHana
            // 
            this.richHana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richHana.Location = new System.Drawing.Point(0, 0);
            this.richHana.Name = "richHana";
            this.richHana.Size = new System.Drawing.Size(384, 562);
            this.richHana.TabIndex = 0;
            this.richHana.Text = "";
            // 
            // frmHanaMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 562);
            this.Controls.Add(this.richHana);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHanaMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NFME Map";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richHana;



    }
}