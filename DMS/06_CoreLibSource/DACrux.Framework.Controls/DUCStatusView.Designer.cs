namespace DACrux.Framework.Controls
{
    partial class DUCStatusView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCStatusView));
            this.lbl_Step4 = new System.Windows.Forms.Label();
            this.lbl_Step3 = new System.Windows.Forms.Label();
            this.lbl_Step2 = new System.Windows.Forms.Label();
            this.lbl_Step1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Step8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Step4
            // 
            this.lbl_Step4.BackColor = System.Drawing.Color.Cyan;
            this.lbl_Step4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step4.ForeColor = System.Drawing.Color.White;
            this.lbl_Step4.Location = new System.Drawing.Point(312, 2);
            this.lbl_Step4.Name = "lbl_Step4";
            this.lbl_Step4.Size = new System.Drawing.Size(88, 20);
            this.lbl_Step4.TabIndex = 142;
            this.lbl_Step4.Text = "정비완료";
            this.lbl_Step4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Step3
            // 
            this.lbl_Step3.BackColor = System.Drawing.Color.MistyRose;
            this.lbl_Step3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step3.ForeColor = System.Drawing.Color.White;
            this.lbl_Step3.Location = new System.Drawing.Point(218, 2);
            this.lbl_Step3.Name = "lbl_Step3";
            this.lbl_Step3.Size = new System.Drawing.Size(88, 20);
            this.lbl_Step3.TabIndex = 137;
            this.lbl_Step3.Text = "정비시작";
            this.lbl_Step3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Step2
            // 
            this.lbl_Step2.BackColor = System.Drawing.Color.Goldenrod;
            this.lbl_Step2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step2.ForeColor = System.Drawing.Color.White;
            this.lbl_Step2.Location = new System.Drawing.Point(124, 2);
            this.lbl_Step2.Name = "lbl_Step2";
            this.lbl_Step2.Size = new System.Drawing.Size(88, 20);
            this.lbl_Step2.TabIndex = 138;
            this.lbl_Step2.Text = "신고접수";
            this.lbl_Step2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Step1
            // 
            this.lbl_Step1.BackColor = System.Drawing.Color.Orange;
            this.lbl_Step1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step1.ForeColor = System.Drawing.Color.White;
            this.lbl_Step1.Location = new System.Drawing.Point(30, 2);
            this.lbl_Step1.Name = "lbl_Step1";
            this.lbl_Step1.Size = new System.Drawing.Size(88, 20);
            this.lbl_Step1.TabIndex = 139;
            this.lbl_Step1.Text = "알람발생";
            this.lbl_Step1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 139;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Step8
            // 
            this.lbl_Step8.BackColor = System.Drawing.Color.White;
            this.lbl_Step8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step8.ForeColor = System.Drawing.Color.Black;
            this.lbl_Step8.Location = new System.Drawing.Point(406, 2);
            this.lbl_Step8.Name = "lbl_Step8";
            this.lbl_Step8.Size = new System.Drawing.Size(88, 20);
            this.lbl_Step8.TabIndex = 140;
            this.lbl_Step8.Text = "관리자 완료";
            this.lbl_Step8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DUCStatusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_Step8);
            this.Controls.Add(this.lbl_Step4);
            this.Controls.Add(this.lbl_Step3);
            this.Controls.Add(this.lbl_Step2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Step1);
            this.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DUCStatusView";
            this.Size = new System.Drawing.Size(505, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Step4;
        private System.Windows.Forms.Label lbl_Step3;
        private System.Windows.Forms.Label lbl_Step2;
        private System.Windows.Forms.Label lbl_Step1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Step8;
    }
}
