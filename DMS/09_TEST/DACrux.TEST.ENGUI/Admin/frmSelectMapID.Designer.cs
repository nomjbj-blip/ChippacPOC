namespace DACrux.TEST.ENGUI
{
    partial class frmSelectMapID
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
            this.lstMapID = new System.Windows.Forms.ListBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstMapID
            // 
            this.lstMapID.ItemHeight = 12;
            this.lstMapID.Location = new System.Drawing.Point(10, 20);
            this.lstMapID.Name = "lstMapID";
            this.lstMapID.Size = new System.Drawing.Size(264, 196);
            this.lstMapID.TabIndex = 4;
            this.lstMapID.SelectedIndexChanged += new System.EventHandler(this.lstMapID_SelectedIndexChanged);
            this.lstMapID.DoubleClick += new System.EventHandler(this.lstMapID_DoubleClick);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(58, 222);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 3;
            this.butOK.Text = "OK";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(154, 222);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "Cancel";
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // frmSelectMapID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 252);
            this.Controls.Add(this.lstMapID);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Name = "frmSelectMapID";
            this.Text = "Selected Map ID";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstMapID;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
    }
}