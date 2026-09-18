namespace DACrux.Framework.Controls
{
    partial class DUCListBox
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
            this.txtItem = new System.Windows.Forms.TextBox();
            this.lbItems = new System.Windows.Forms.ListBox();
            this.lbTiTle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtItem
            // 
            this.txtItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtItem.Location = new System.Drawing.Point(3, 24);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(144, 21);
            this.txtItem.TabIndex = 0;
            this.txtItem.TextChanged += new System.EventHandler(this.txtItem_TextChanged);
            this.txtItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItem_KeyDown);
            // 
            // lbItems
            // 
            this.lbItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbItems.FormattingEnabled = true;
            this.lbItems.ItemHeight = 12;
            this.lbItems.Location = new System.Drawing.Point(3, 49);
            this.lbItems.Name = "lbItems";
            this.lbItems.Size = new System.Drawing.Size(144, 28);
            this.lbItems.TabIndex = 2;
            this.lbItems.SelectedIndexChanged += new System.EventHandler(this.lbItems_SelectedIndexChanged);
            this.lbItems.SelectedValueChanged += new System.EventHandler(this.lbItems_SelectedValueChanged);
            this.lbItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbItems_MouseDoubleClick);
            // 
            // lbTiTle
            // 
            this.lbTiTle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTiTle.AutoSize = true;
            this.lbTiTle.Image = global::DACrux.Framework.Controls.Properties.Resources.Search;
            this.lbTiTle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTiTle.Location = new System.Drawing.Point(3, 7);
            this.lbTiTle.Name = "lbTiTle";
            this.lbTiTle.Size = new System.Drawing.Size(0, 12);
            this.lbTiTle.TabIndex = 3;
            this.lbTiTle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTiTle.Click += new System.EventHandler(this.lbTiTle_Click);
            // 
            // DUCListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbTiTle);
            this.Controls.Add(this.lbItems);
            this.Controls.Add(this.txtItem);
            this.Name = "DUCListBox";
            this.Size = new System.Drawing.Size(150, 79);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.ListBox lbItems;
        private System.Windows.Forms.Label lbTiTle;
    }
}
