namespace DACrux.SP.Controls
{
    partial class uclYieldType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uclYieldType));
            this.lblTitle = new DACrux.SP.Controls.uclLabel(this.components);
            this.cboYieldType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTitle.HighLight = false;
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowImage = true;
            this.lblTitle.Size = new System.Drawing.Size(99, 20);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "    공정 구간";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboYieldType
            // 
            this.cboYieldType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboYieldType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboYieldType.FormattingEnabled = true;
            this.cboYieldType.Items.AddRange(new object[] {
            "공정",
            "SENSOR",
            "MODULE",
            "완제품"});
            this.cboYieldType.Location = new System.Drawing.Point(99, 0);
            this.cboYieldType.Name = "cboYieldType";
            this.cboYieldType.Size = new System.Drawing.Size(124, 20);
            this.cboYieldType.TabIndex = 6;
            this.cboYieldType.SelectedIndexChanged += new System.EventHandler(this.cboYieldType_SelectedIndexChanged);
            // 
            // uclYieldType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cboYieldType);
            this.Controls.Add(this.lblTitle);
            this.Name = "uclYieldType";
            this.Size = new System.Drawing.Size(223, 20);
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private uclLabel lblTitle;
        private System.Windows.Forms.ComboBox cboYieldType;
    }
}
