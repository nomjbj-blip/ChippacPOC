namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDMSStepSelect
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
this.dpucStepSelect = new DACrux.SEMDMS.Control.DPUCStepSelect();
this.SuspendLayout();
// 
// dpucStepSelect
// 
this.dpucStepSelect.Dock = System.Windows.Forms.DockStyle.Fill;
this.dpucStepSelect.Location = new System.Drawing.Point(0, 0);
this.dpucStepSelect.MultiSelect = true;
this.dpucStepSelect.Name = "dpucStepSelect";
this.dpucStepSelect.Size = new System.Drawing.Size(682, 561);
this.dpucStepSelect.TabIndex = 0;
this.dpucStepSelect.OnSelected += new DACrux.SEMDMS.Control.Selected(this.dpucStepSelect1_OnSelected);
// 
// frmDMSStepSelect
// 
this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
this.ClientSize = new System.Drawing.Size(682, 561);
this.Controls.Add(this.dpucStepSelect);
this.Name = "frmDMSStepSelect";
this.Text = "DMS Step Select";
this.ResumeLayout(false);

        }

        #endregion

        //private DMSPlus.DataSelect.Control.DPUCStepSelect dpucStepSelect1;
       private DACrux.SEMDMS.Control.DPUCStepSelect dpucStepSelect;
    }
}