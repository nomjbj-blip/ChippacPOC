namespace DACrux.BStats.StatDialog.DlgHypothesisTesting_
{
    partial class DlgHypothesisOptions
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBack = new System.Windows.Forms.Panel();
            this.chkCI = new System.Windows.Forms.CheckBox();
            this.chkCriticalValue = new System.Windows.Forms.CheckBox();
            this.chkDescStat = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.chkCI);
            this.pnlBack.Controls.Add(this.chkCriticalValue);
            this.pnlBack.Controls.Add(this.chkDescStat);
            this.pnlBack.Controls.Add(this.btnCancel);
            this.pnlBack.Controls.Add(this.btnOk);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(228, 139);
            this.pnlBack.TabIndex = 3;
            // 
            // chkCI
            // 
            this.chkCI.AutoSize = true;
            this.chkCI.Checked = true;
            this.chkCI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCI.Location = new System.Drawing.Point(24, 46);
            this.chkCI.Name = "chkCI";
            this.chkCI.Size = new System.Drawing.Size(116, 17);
            this.chkCI.TabIndex = 43;
            this.chkCI.Text = "&Confidence interval";
            this.chkCI.UseVisualStyleBackColor = true;
            // 
            // chkCriticalValue
            // 
            this.chkCriticalValue.AutoSize = true;
            this.chkCriticalValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCriticalValue.Location = new System.Drawing.Point(24, 68);
            this.chkCriticalValue.Name = "chkCriticalValue";
            this.chkCriticalValue.Size = new System.Drawing.Size(84, 17);
            this.chkCriticalValue.TabIndex = 44;
            this.chkCriticalValue.Text = "C&ritical Value";
            this.chkCriticalValue.UseVisualStyleBackColor = true;
            // 
            // chkDescStat
            // 
            this.chkDescStat.AutoSize = true;
            this.chkDescStat.Checked = true;
            this.chkDescStat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDescStat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDescStat.Location = new System.Drawing.Point(24, 22);
            this.chkDescStat.Name = "chkDescStat";
            this.chkDescStat.Size = new System.Drawing.Size(121, 17);
            this.chkDescStat.TabIndex = 42;
            this.chkDescStat.Text = "&Descriptive statistics";
            this.chkDescStat.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 103);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(90, 103);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // DlgHypothesisOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(228, 139);
            this.Controls.Add(this.pnlBack);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgHypothesisOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hypothesis testing - Options";
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkCI;
        private System.Windows.Forms.CheckBox chkCriticalValue;
        private System.Windows.Forms.CheckBox chkDescStat;
    }
}