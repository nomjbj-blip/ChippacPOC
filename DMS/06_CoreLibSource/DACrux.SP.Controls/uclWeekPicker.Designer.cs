namespace DACrux.SP.Controls
{
    partial class uclWeekPicker
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
            this.lblYear = new System.Windows.Forms.Label();
            this.cboWeek = new System.Windows.Forms.ComboBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.lblWeek = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(57, 3);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(32, 14);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "Year";
            // 
            // cboWeek
            // 
            this.cboWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWeek.FormattingEnabled = true;
            this.cboWeek.Location = new System.Drawing.Point(93, 0);
            this.cboWeek.Name = "cboWeek";
            this.cboWeek.Size = new System.Drawing.Size(40, 22);
            this.cboWeek.TabIndex = 2;
            this.cboWeek.Text = "00";
            this.cboWeek.SelectedIndexChanged += new System.EventHandler(this.cboWeek_SelectedIndexChanged);
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(0, 0);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(56, 22);
            this.cboYear.TabIndex = 3;
            this.cboYear.Text = "2009";
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // lblWeek
            // 
            this.lblWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWeek.AutoSize = true;
            this.lblWeek.Location = new System.Drawing.Point(135, 3);
            this.lblWeek.Name = "lblWeek";
            this.lblWeek.Size = new System.Drawing.Size(55, 14);
            this.lblWeek.TabIndex = 4;
            this.lblWeek.Text = "th Week";
            // 
            // uclWeekPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblWeek);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.cboWeek);
            this.Controls.Add(this.lblYear);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "uclWeekPicker";
            this.Size = new System.Drawing.Size(191, 22);
            this.Load += new System.EventHandler(this.uclWeekPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cboWeek;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label lblWeek;
    }
}
