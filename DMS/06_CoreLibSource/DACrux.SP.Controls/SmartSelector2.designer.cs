namespace DACrux.SP.Controls
{
    partial class SmartSelector2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartSelector2));
            this.btnListUp = new System.Windows.Forms.PictureBox();
            this.imlArrow = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new DACrux.SP.Controls.uclLabel(this.components);
            this.txtContents = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnListUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // btnListUp
            // 
            this.btnListUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnListUp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnListUp.Image = ((System.Drawing.Image)(resources.GetObject("btnListUp.Image")));
            this.btnListUp.Location = new System.Drawing.Point(183, 0);
            this.btnListUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnListUp.Name = "btnListUp";
            this.btnListUp.Size = new System.Drawing.Size(24, 22);
            this.btnListUp.TabIndex = 125;
            this.btnListUp.TabStop = false;
            this.btnListUp.Click += new System.EventHandler(this.btnListUp_Click);
            // 
            // imlArrow
            // 
            this.imlArrow.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlArrow.ImageStream")));
            this.imlArrow.TransparentColor = System.Drawing.Color.White;
            this.imlArrow.Images.SetKeyName(0, "spot1.gif");
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(181, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 22);
            this.panel1.TabIndex = 127;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowImage = true;
            this.lblTitle.Size = new System.Drawing.Size(85, 22);
            this.lblTitle.TabIndex = 126;
            this.lblTitle.Text = "     Caption";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContents
            // 
            this.txtContents.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtContents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContents.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContents.Location = new System.Drawing.Point(85, 0);
            this.txtContents.Multiline = true;
            this.txtContents.Name = "txtContents";
            this.txtContents.ReadOnly = true;
            this.txtContents.Size = new System.Drawing.Size(96, 22);
            this.txtContents.TabIndex = 129;
            this.txtContents.Text = "All";
            this.txtContents.WordWrap = false;
            // 
            // SmartSelector2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtContents);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnListUp);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SmartSelector2";
            this.Size = new System.Drawing.Size(207, 22);
            this.Load += new System.EventHandler(this.SmartSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnListUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox btnListUp;
        private uclLabel lblTitle;
        private System.Windows.Forms.ImageList imlArrow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtContents;
    }
}
