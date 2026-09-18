namespace DACrux.Data.Parser
{
    partial class PopupExportToKlarf_Sampling
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSampleCount = new System.Windows.Forms.TextBox();
            this.txtAllCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWaferID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSampleCount
            // 
            this.txtSampleCount.Location = new System.Drawing.Point(93, 47);
            this.txtSampleCount.Name = "txtSampleCount";
            this.txtSampleCount.Size = new System.Drawing.Size(92, 21);
            this.txtSampleCount.TabIndex = 6;
            this.txtSampleCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAllCount
            // 
            this.txtAllCount.Location = new System.Drawing.Point(93, 22);
            this.txtAllCount.Name = "txtAllCount";
            this.txtAllCount.ReadOnly = true;
            this.txtAllCount.Size = new System.Drawing.Size(92, 21);
            this.txtAllCount.TabIndex = 7;
            this.txtAllCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sample Count";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Defect Count";
            // 
            // lblWaferID
            // 
            this.lblWaferID.AutoSize = true;
            this.lblWaferID.ForeColor = System.Drawing.Color.Blue;
            this.lblWaferID.Location = new System.Drawing.Point(5, 3);
            this.lblWaferID.Name = "lblWaferID";
            this.lblWaferID.Size = new System.Drawing.Size(38, 12);
            this.lblWaferID.TabIndex = 8;
            this.lblWaferID.Text = "label1";
            // 
            // PopupExportToKlarf_Sampling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblWaferID);
            this.Controls.Add(this.txtSampleCount);
            this.Controls.Add(this.txtAllCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Name = "PopupExportToKlarf_Sampling";
            this.Size = new System.Drawing.Size(190, 75);
            this.Load += new System.EventHandler(this.PopupExportToKlarf_Sampling_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSampleCount;
        private System.Windows.Forms.TextBox txtAllCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWaferID;
    }
}
