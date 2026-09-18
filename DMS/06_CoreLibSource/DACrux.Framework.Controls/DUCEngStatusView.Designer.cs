namespace DACrux.Framework.Controls
{
    partial class DUCEngStatusView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCEngStatusView));
            this.lbl_Step7 = new System.Windows.Forms.Label();
            this.lbl_Step5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Step8 = new System.Windows.Forms.Label();
            this.lbl_Step6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Step7
            // 
            this.lbl_Step7.BackColor = System.Drawing.Color.Gray;
            this.lbl_Step7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step7.ForeColor = System.Drawing.Color.White;
            this.lbl_Step7.Location = new System.Drawing.Point(276, 2);
            this.lbl_Step7.Name = "lbl_Step7";
            this.lbl_Step7.Size = new System.Drawing.Size(117, 20);
            this.lbl_Step7.TabIndex = 140;
            this.lbl_Step7.Text = "조치완료";
            this.lbl_Step7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Step5
            // 
            this.lbl_Step5.BackColor = System.Drawing.Color.Chartreuse;
            this.lbl_Step5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step5.ForeColor = System.Drawing.Color.White;
            this.lbl_Step5.Location = new System.Drawing.Point(30, 2);
            this.lbl_Step5.Name = "lbl_Step5";
            this.lbl_Step5.Size = new System.Drawing.Size(117, 20);
            this.lbl_Step5.TabIndex = 141;
            this.lbl_Step5.Text = "알람발생";
            this.lbl_Step5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lbl_Step8.Location = new System.Drawing.Point(399, 2);
            this.lbl_Step8.Name = "lbl_Step8";
            this.lbl_Step8.Size = new System.Drawing.Size(117, 20);
            this.lbl_Step8.TabIndex = 140;
            this.lbl_Step8.Text = "품질완료";
            this.lbl_Step8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Step6
            // 
            this.lbl_Step6.BackColor = System.Drawing.Color.RoyalBlue;
            this.lbl_Step6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Step6.ForeColor = System.Drawing.Color.White;
            this.lbl_Step6.Location = new System.Drawing.Point(153, 2);
            this.lbl_Step6.Name = "lbl_Step6";
            this.lbl_Step6.Size = new System.Drawing.Size(117, 20);
            this.lbl_Step6.TabIndex = 140;
            this.lbl_Step6.Text = "신고접수";
            this.lbl_Step6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DUCEngStatusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_Step6);
            this.Controls.Add(this.lbl_Step8);
            this.Controls.Add(this.lbl_Step7);
            this.Controls.Add(this.lbl_Step5);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DUCEngStatusView";
            this.Size = new System.Drawing.Size(525, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Step7;
        private System.Windows.Forms.Label lbl_Step5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Step8;
        private System.Windows.Forms.Label lbl_Step6;
    }
}
