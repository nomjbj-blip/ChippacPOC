namespace DACrux.Framework.Controls
{
    partial class DUCNumericUpDown
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
            this.lbl_title = new System.Windows.Forms.Label();
            this.nNumber = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_title.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Image = global::DACrux.Framework.Controls.Properties.Resources.StopHS;
            this.lbl_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(91, 21);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "     Sample";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nNumber
            // 
            this.nNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nNumber.Location = new System.Drawing.Point(91, 0);
            this.nNumber.Margin = new System.Windows.Forms.Padding(0);
            this.nNumber.Name = "nNumber";
            this.nNumber.Size = new System.Drawing.Size(90, 21);
            this.nNumber.TabIndex = 2;
            this.nNumber.ValueChanged += new System.EventHandler(this.nNumber_ValueChanged);
            // 
            // DUCNumericUpDown
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.nNumber);
            this.Controls.Add(this.lbl_title);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DUCNumericUpDown";
            this.Size = new System.Drawing.Size(181, 21);
            this.VisibleChanged += new System.EventHandler(this.DUCNumericUpDown_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.NumericUpDown nNumber;
    }
}
