namespace DACrux.TEST.ENGUI.DataSelecter
{
    partial class LegendItem
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
            this.chkVisible = new System.Windows.Forms.CheckBox();
            this.pnlItem = new System.Windows.Forms.Panel();
            this.chkShowLabel = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkVisible
            // 
            this.chkVisible.Checked = true;
            this.chkVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVisible.Location = new System.Drawing.Point(3, -1);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(16, 24);
            this.chkVisible.TabIndex = 0;
            this.chkVisible.UseVisualStyleBackColor = true;
            // 
            // pnlItem
            // 
            this.pnlItem.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlItem.Location = new System.Drawing.Point(29, 1);
            this.pnlItem.Name = "pnlItem";
            this.pnlItem.Size = new System.Drawing.Size(28, 19);
            this.pnlItem.TabIndex = 1;
            this.pnlItem.Click += new System.EventHandler(this.LegendItem_Click);
            // 
            // chkShowLabel
            // 
            this.chkShowLabel.Location = new System.Drawing.Point(15, -1);
            this.chkShowLabel.Name = "chkShowLabel";
            this.chkShowLabel.Size = new System.Drawing.Size(16, 24);
            this.chkShowLabel.TabIndex = 3;
            this.chkShowLabel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(63, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.LegendItem_Click);
            // 
            // LegendItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlItem);
            this.Controls.Add(this.chkShowLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkVisible);
            this.Name = "LegendItem";
            this.Size = new System.Drawing.Size(223, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVisible;
        private System.Windows.Forms.Panel pnlItem;
        private System.Windows.Forms.CheckBox chkShowLabel;
        private System.Windows.Forms.Label label1;
    }
}
