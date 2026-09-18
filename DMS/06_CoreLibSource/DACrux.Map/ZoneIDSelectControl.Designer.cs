namespace DACrux.Map
{
    partial class ZoneIDSelectControl
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
            this.lstZoneItem = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkShowZoneID = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstZoneItem
            // 
            this.lstZoneItem.CheckOnClick = true;
            this.lstZoneItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstZoneItem.FormattingEnabled = true;
            this.lstZoneItem.Location = new System.Drawing.Point(0, 21);
            this.lstZoneItem.Name = "lstZoneItem";
            this.lstZoneItem.Size = new System.Drawing.Size(60, 272);
            this.lstZoneItem.TabIndex = 0;
            this.lstZoneItem.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstZoneItem_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.chkShowZoneID);
            this.panel1.Controls.Add(this.chkAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(60, 21);
            this.panel1.TabIndex = 1;
            // 
            // chkShowZoneID
            // 
            this.chkShowZoneID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowZoneID.AutoSize = true;
            this.chkShowZoneID.Location = new System.Drawing.Point(42, 3);
            this.chkShowZoneID.Name = "chkShowZoneID";
            this.chkShowZoneID.Size = new System.Drawing.Size(15, 14);
            this.chkShowZoneID.TabIndex = 1;
            this.chkShowZoneID.UseVisualStyleBackColor = true;
            this.chkShowZoneID.CheckedChanged += new System.EventHandler(this.chkShowZoneID_CheckedChanged);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(3, 3);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(15, 14);
            this.chkAll.TabIndex = 0;
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // ZoneIDSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstZoneItem);
            this.Controls.Add(this.panel1);
            this.Name = "ZoneIDSelectControl";
            this.Size = new System.Drawing.Size(60, 293);
            this.Load += new System.EventHandler(this.ZoneIDListControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstZoneItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckBox chkShowZoneID;
    }
}
