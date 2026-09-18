namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgTaguchiAnalysisOption
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
            this.rdoRule01 = new System.Windows.Forms.RadioButton();
            this.rdoRule02 = new System.Windows.Forms.RadioButton();
            this.rdoRule03 = new System.Windows.Forms.RadioButton();
            this.rdoRule04 = new System.Windows.Forms.RadioButton();
            this.chkTarget = new System.Windows.Forms.CheckBox();
            this.chkStddevIns = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdoRule01
            // 
            this.rdoRule01.AutoSize = true;
            this.rdoRule01.Location = new System.Drawing.Point(12, 33);
            this.rdoRule01.Name = "rdoRule01";
            this.rdoRule01.Size = new System.Drawing.Size(338, 16);
            this.rdoRule01.TabIndex = 0;
            this.rdoRule01.Text = "클수록 좋음                         -10 X Log(sum(1/Y**2)/n)";
            this.rdoRule01.UseVisualStyleBackColor = true;
            // 
            // rdoRule02
            // 
            this.rdoRule02.AutoSize = true;
            this.rdoRule02.Location = new System.Drawing.Point(12, 55);
            this.rdoRule02.Name = "rdoRule02";
            this.rdoRule02.Size = new System.Drawing.Size(266, 16);
            this.rdoRule02.TabIndex = 0;
            this.rdoRule02.Text = "목표 수준이 가장 좋음(N)      -10*Log(s**2)";
            this.rdoRule02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRule02.UseVisualStyleBackColor = true;
            // 
            // rdoRule03
            // 
            this.rdoRule03.AutoSize = true;
            this.rdoRule03.Location = new System.Drawing.Point(12, 77);
            this.rdoRule03.Name = "rdoRule03";
            this.rdoRule03.Size = new System.Drawing.Size(310, 16);
            this.rdoRule03.TabIndex = 0;
            this.rdoRule03.Text = "목표 수준이 가장 좋음(B)      10*Log(YBar**2/s**2)";
            this.rdoRule03.UseVisualStyleBackColor = true;
            // 
            // rdoRule04
            // 
            this.rdoRule04.AutoSize = true;
            this.rdoRule04.Location = new System.Drawing.Point(12, 99);
            this.rdoRule04.Name = "rdoRule04";
            this.rdoRule04.Size = new System.Drawing.Size(311, 16);
            this.rdoRule04.TabIndex = 0;
            this.rdoRule04.Text = "작을수록 좋음                      -10*Log(sum(Y**2)/n";
            this.rdoRule04.UseVisualStyleBackColor = true;
            // 
            // chkTarget
            // 
            this.chkTarget.AutoSize = true;
            this.chkTarget.Location = new System.Drawing.Point(12, 132);
            this.chkTarget.Name = "chkTarget";
            this.chkTarget.Size = new System.Drawing.Size(320, 16);
            this.chkTarget.TabIndex = 1;
            this.chkTarget.Text = "반응이 목표값과 일치할 때 가장 좋은 경우 수정된 공식";
            this.chkTarget.UseVisualStyleBackColor = true;
            this.chkTarget.Visible = false;
            // 
            // chkStddevIns
            // 
            this.chkStddevIns.AutoSize = true;
            this.chkStddevIns.Location = new System.Drawing.Point(12, 154);
            this.chkStddevIns.Name = "chkStddevIns";
            this.chkStddevIns.Size = new System.Drawing.Size(175, 16);
            this.chkStddevIns.TabIndex = 1;
            this.chkStddevIns.Text = "모든 표준 편차에 In(s) 사용";
            this.chkStddevIns.UseVisualStyleBackColor = true;
            this.chkStddevIns.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "신호 대 잡음 비:                          공식";
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(313, 175);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(91, 28);
            this.butCancel.TabIndex = 7;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(200, 175);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(91, 28);
            this.butOk.TabIndex = 6;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // DlgTaguchiAnalysisOption
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(416, 211);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkStddevIns);
            this.Controls.Add(this.chkTarget);
            this.Controls.Add(this.rdoRule04);
            this.Controls.Add(this.rdoRule03);
            this.Controls.Add(this.rdoRule02);
            this.Controls.Add(this.rdoRule01);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgTaguchiAnalysisOption";
            this.ShowInTaskbar = false;
            this.Text = "Taguchi 분석 - 옵션";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoRule01;
        private System.Windows.Forms.RadioButton rdoRule02;
        private System.Windows.Forms.RadioButton rdoRule03;
        private System.Windows.Forms.RadioButton rdoRule04;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOk;
        public System.Windows.Forms.CheckBox chkTarget;
        public System.Windows.Forms.CheckBox chkStddevIns;
    }
}