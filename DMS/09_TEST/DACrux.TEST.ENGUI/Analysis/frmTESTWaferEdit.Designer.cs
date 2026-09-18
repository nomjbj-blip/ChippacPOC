namespace DACrux.TEST.ENGUI
{
    partial class frmTESTWaferEdit
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
            this.tpucMapEditor = new DACrux.TEST.Control.TPUCMapEdit();
            this.SuspendLayout();
            // 
            // tpucMapEditor
            // 
            this.tpucMapEditor.AutoFocus = false;
            this.tpucMapEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpucMapEditor.Location = new System.Drawing.Point(0, 0);
            this.tpucMapEditor.Name = "tpucMapEditor";
            this.tpucMapEditor.Size = new System.Drawing.Size(964, 525);
            this.tpucMapEditor.TabIndex = 0;
            // 
            // frmTESTWaferEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 525);
            this.Controls.Add(this.tpucMapEditor);
            this.Name = "frmTESTWaferEdit";
            this.Text = "Visual Inspection";
            this.Load += new System.EventHandler(this.frmTESTWaferEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.TPUCMapEdit tpucMapEditor;

    }
}