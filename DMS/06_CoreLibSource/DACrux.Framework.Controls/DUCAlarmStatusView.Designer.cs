namespace DACrux.Framework.Controls
{
    partial class DUCAlarmStatusView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCAlarmStatusView));
            this.lbl_Step2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Step1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Step2
            // 
            this.lbl_Step2.BackColor = System.Drawing.Color.Yellow;
            this.lbl_Step2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step2.ForeColor = System.Drawing.Color.Black;
            this.lbl_Step2.Location = new System.Drawing.Point(91, 0);
            this.lbl_Step2.Name = "lbl_Step2";
            this.lbl_Step2.Size = new System.Drawing.Size(62, 20);
            this.lbl_Step2.TabIndex = 140;
            this.lbl_Step2.Text = "Process";
            this.lbl_Step2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 142;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Step1
            // 
            this.lbl_Step1.BackColor = System.Drawing.Color.LightPink;
            this.lbl_Step1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step1.ForeColor = System.Drawing.Color.Black;
            this.lbl_Step1.Location = new System.Drawing.Point(29, 0);
            this.lbl_Step1.Name = "lbl_Step1";
            this.lbl_Step1.Size = new System.Drawing.Size(61, 20);
            this.lbl_Step1.TabIndex = 141;
            this.lbl_Step1.Text = "조치안함";
            this.lbl_Step1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.GreenYellow;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(155, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 140;
            this.label2.Text = "조치완료";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DUCAlarmStatusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_Step2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Step1);
            this.Name = "DUCAlarmStatusView";
            this.Size = new System.Drawing.Size(220, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Step2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Step1;
        private System.Windows.Forms.Label label2;
    }
}
