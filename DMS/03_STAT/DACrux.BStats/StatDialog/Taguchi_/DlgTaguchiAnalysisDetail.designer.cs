namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgTaguchiAnalysisDetail
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
            this.chkY_SNRatio = new System.Windows.Forms.CheckBox();
            this.chkY_Average = new System.Windows.Forms.CheckBox();
            this.chkY_Stdev = new System.Windows.Forms.CheckBox();
            this.chkL_SNRatio = new System.Windows.Forms.CheckBox();
            this.chkL_Average = new System.Windows.Forms.CheckBox();
            this.chkL_Stdev = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkY_SNRatio
            // 
            this.chkY_SNRatio.AutoSize = true;
            this.chkY_SNRatio.Location = new System.Drawing.Point(12, 49);
            this.chkY_SNRatio.Name = "chkY_SNRatio";
            this.chkY_SNRatio.Size = new System.Drawing.Size(108, 16);
            this.chkY_SNRatio.TabIndex = 0;
            this.chkY_SNRatio.Text = "신호 대 잡음 비";
            this.chkY_SNRatio.UseVisualStyleBackColor = true;
            // 
            // chkY_Average
            // 
            this.chkY_Average.AutoSize = true;
            this.chkY_Average.Location = new System.Drawing.Point(12, 71);
            this.chkY_Average.Name = "chkY_Average";
            this.chkY_Average.Size = new System.Drawing.Size(48, 16);
            this.chkY_Average.TabIndex = 0;
            this.chkY_Average.Text = "평균";
            this.chkY_Average.UseVisualStyleBackColor = true;
            // 
            // chkY_Stdev
            // 
            this.chkY_Stdev.AutoSize = true;
            this.chkY_Stdev.Location = new System.Drawing.Point(12, 93);
            this.chkY_Stdev.Name = "chkY_Stdev";
            this.chkY_Stdev.Size = new System.Drawing.Size(76, 16);
            this.chkY_Stdev.TabIndex = 0;
            this.chkY_Stdev.Text = "표준 편차";
            this.chkY_Stdev.UseVisualStyleBackColor = true;
            // 
            // chkL_SNRatio
            // 
            this.chkL_SNRatio.AutoSize = true;
            this.chkL_SNRatio.Location = new System.Drawing.Point(200, 49);
            this.chkL_SNRatio.Name = "chkL_SNRatio";
            this.chkL_SNRatio.Size = new System.Drawing.Size(108, 16);
            this.chkL_SNRatio.TabIndex = 0;
            this.chkL_SNRatio.Text = "신호 대 잡음 비";
            this.chkL_SNRatio.UseVisualStyleBackColor = true;
            // 
            // chkL_Average
            // 
            this.chkL_Average.AutoSize = true;
            this.chkL_Average.Location = new System.Drawing.Point(200, 71);
            this.chkL_Average.Name = "chkL_Average";
            this.chkL_Average.Size = new System.Drawing.Size(48, 16);
            this.chkL_Average.TabIndex = 0;
            this.chkL_Average.Text = "평균";
            this.chkL_Average.UseVisualStyleBackColor = true;
            // 
            // chkL_Stdev
            // 
            this.chkL_Stdev.AutoSize = true;
            this.chkL_Stdev.Location = new System.Drawing.Point(200, 93);
            this.chkL_Stdev.Name = "chkL_Stdev";
            this.chkL_Stdev.Size = new System.Drawing.Size(76, 16);
            this.chkL_Stdev.TabIndex = 0;
            this.chkL_Stdev.Text = "표준 편차";
            this.chkL_Stdev.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "반응 표 표시 대상 통계량";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "선형 모형 적합 대상 통계량";
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(273, 118);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(93, 35);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(174, 118);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(93, 35);
            this.butOk.TabIndex = 2;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // DlgTaguchiAnalysisDetail
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(378, 165);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkL_Stdev);
            this.Controls.Add(this.chkY_Stdev);
            this.Controls.Add(this.chkL_Average);
            this.Controls.Add(this.chkY_Average);
            this.Controls.Add(this.chkL_SNRatio);
            this.Controls.Add(this.chkY_SNRatio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DlgTaguchiAnalysisDetail";
            this.Text = "Taguchi 분석 - 분석";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOk;
        public System.Windows.Forms.CheckBox chkY_SNRatio;
        public System.Windows.Forms.CheckBox chkY_Stdev;
        public System.Windows.Forms.CheckBox chkL_SNRatio;
        public System.Windows.Forms.CheckBox chkL_Average;
        public System.Windows.Forms.CheckBox chkL_Stdev;
        public System.Windows.Forms.CheckBox chkY_Average;
    }
}