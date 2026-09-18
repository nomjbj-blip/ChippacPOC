namespace DACrux.SP.Controls
{
    partial class uclTextBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uclTextBox));
            this.lblTitle = new DACrux.SP.Controls.uclLabel(this.components);
            this.txtContents = new DACrux.SP.Controls.SmartTextBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTitle.HighLight = false;
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Location = new System.Drawing.Point(0, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowImage = true;
            this.lblTitle.Size = new System.Drawing.Size(70, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "     Type";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContents
            // 
            this.txtContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContents.Location = new System.Drawing.Point(70, 1);
            this.txtContents.Multiline = true;
            this.txtContents.Name = "txtContents";
            this.txtContents.NumberOnly = false;
            this.txtContents.Size = new System.Drawing.Size(115, 19);
            this.txtContents.TabIndex = 2;
            // 
            // uclTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtContents);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "uclTextBox";
            this.Size = new System.Drawing.Size(185, 21);
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private uclLabel lblTitle;
        private SmartTextBox txtContents;

    }
}
