namespace DACrux.SEMDMS.ENGUI
{
    partial class dlgSizePopup
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ducListBox1 = new DACrux.Framework.Controls.DUCListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 32);
            this.panel1.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(50, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(131, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ducListBox1
            // 
            this.ducListBox1.DataSource = null;
            this.ducListBox1.DisplayMember = "";
            this.ducListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ducListBox1.Location = new System.Drawing.Point(0, 0);
            this.ducListBox1.Name = "ducListBox1";
            this.ducListBox1.SearchText = "";
            this.ducListBox1.SearchTitle = "";
            this.ducListBox1.SelectedIndex = -1;
            this.ducListBox1.SelectedItem = null;
            this.ducListBox1.SelectedValue = null;
            this.ducListBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ducListBox1.Size = new System.Drawing.Size(209, 229);
            this.ducListBox1.TabIndex = 1;
            this.ducListBox1.ValueMember = "";
            // 
            // dlgSizePopup
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(209, 261);
            this.Controls.Add(this.ducListBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSizePopup";
            this.Text = "Size Option";
            this.Load += new System.EventHandler(this.dlgSizePopup_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private Framework.Controls.DUCListBox ducListBox1;
    }
}