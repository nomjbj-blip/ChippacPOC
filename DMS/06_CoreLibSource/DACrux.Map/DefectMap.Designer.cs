namespace DACrux.Map
{
	partial class DefectMap
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

		#region 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.mnuREALSIZEDEFECT = new System.Windows.Forms.MenuItem();
            this.mnuitemMOUSEDRAGMODE_DEFECT = new System.Windows.Forms.MenuItem();
            this.mnuitemMOUSEDRAGMODE_DIE = new System.Windows.Forms.MenuItem();
            this.mnuDefectImageMark = new System.Windows.Forms.MenuItem();
            this.menuItemDefectView = new System.Windows.Forms.MenuItem();
            this.menuItemDefectView_All = new System.Windows.Forms.MenuItem();
            this.menuItemDefectView_ImageDefectOnly = new System.Windows.Forms.MenuItem();
            this.menuItemDefectView_NonImageDefectOnly = new System.Windows.Forms.MenuItem();
            this.mnuitem_VISIBLE_ZONE = new System.Windows.Forms.MenuItem();
            this.mnuitem_ZONE_OPTION = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // ctxmWaferMap
            // 
            this.ctxmWaferMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitem_VISIBLE_ZONE,
            this.mnuitem_ZONE_OPTION,
            this.mnuitemMOUSEDRAGMODE_DEFECT,
            this.mnuitemMOUSEDRAGMODE_DIE,
            this.mnuREALSIZEDEFECT,
            this.mnuDefectImageMark,
            this.menuItemDefectView});
            this.ctxmWaferMap.Popup += new System.EventHandler(this.ctxmWaferMap_Popup);
            // 
            // menuitemMAPMODE_ZOOMIN
            // 
            this.menuitemMAPMODE_ZOOMIN.Index = 9;
            // 
            // menuitemMAPMODE_ZOOMOUT
            // 
            this.menuitemMAPMODE_ZOOMOUT.Index = 10;
            // 
            // mnuitem_SPRIT1
            // 
            this.mnuitem_SPRIT1.Index = 11;
            // 
            // mnuitem_SPRIT2
            // 
            this.mnuitem_SPRIT2.Index = 8;
            // 
            // mnuitem_SPRIT3
            // 
            this.mnuitem_SPRIT3.Index = 15;
            // 
            // mnuCOPYTOCLIP
            // 
            this.mnuCOPYTOCLIP.Index = 16;
            // 
            // mnuitemCLEAR_SELECTEDDIE
            // 
            this.mnuitemCLEAR_SELECTEDDIE.Index = 12;
            this.mnuitemCLEAR_SELECTEDDIE.Visible = false;
            // 
            // menuItemShotOption
            // 
            this.menuItemShotOption.Index = 13;
            // 
            // mnuREALSIZEDEFECT
            // 
            this.mnuREALSIZEDEFECT.Index = 19;
            this.mnuREALSIZEDEFECT.Text = "Real Size Defect Drawing";
            this.mnuREALSIZEDEFECT.Click += new System.EventHandler(this.mnuREALSIZEDEFECT_Click);
            // 
            // mnuitemMOUSEDRAGMODE_DEFECT
            // 
            this.mnuitemMOUSEDRAGMODE_DEFECT.Index = 17;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Text = "Defect Select";
            this.mnuitemMOUSEDRAGMODE_DEFECT.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_DEFECT_Click);
            // 
            // mnuitemMOUSEDRAGMODE_DIE
            // 
            this.mnuitemMOUSEDRAGMODE_DIE.Index = 18;
            this.mnuitemMOUSEDRAGMODE_DIE.Text = "Die Select";
            this.mnuitemMOUSEDRAGMODE_DIE.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_DIE_Click);
            // 
            // mnuDefectImageMark
            // 
            this.mnuDefectImageMark.Index = 20;
            this.mnuDefectImageMark.Text = "Defect Image Mark";
            this.mnuDefectImageMark.Click += new System.EventHandler(this.mnuDefectImageMark_Click);
            // 
            // menuItemDefectView
            // 
            this.menuItemDefectView.Index = 21;
            this.menuItemDefectView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDefectView_All,
            this.menuItemDefectView_ImageDefectOnly,
            this.menuItemDefectView_NonImageDefectOnly});
            this.menuItemDefectView.Text = "Defect View";
            // 
            // menuItemDefectView_All
            // 
            this.menuItemDefectView_All.Checked = true;
            this.menuItemDefectView_All.Index = 0;
            this.menuItemDefectView_All.Text = "All";
            this.menuItemDefectView_All.Click += new System.EventHandler(this.menuItemDefectView_Click);
            // 
            // menuItemDefectView_ImageDefectOnly
            // 
            this.menuItemDefectView_ImageDefectOnly.Index = 1;
            this.menuItemDefectView_ImageDefectOnly.Text = "Image Defect";
            this.menuItemDefectView_ImageDefectOnly.Click += new System.EventHandler(this.menuItemDefectView_Click);
            // 
            // menuItemDefectView_NonImageDefectOnly
            // 
            this.menuItemDefectView_NonImageDefectOnly.Index = 2;
            this.menuItemDefectView_NonImageDefectOnly.Text = "Non-image Defect";
            this.menuItemDefectView_NonImageDefectOnly.Click += new System.EventHandler(this.menuItemDefectView_Click);
            // 
            // mnuitem_VISIBLE_ZONE
            // 
            this.mnuitem_VISIBLE_ZONE.Index = 7;
            this.mnuitem_VISIBLE_ZONE.Text = "Visible Zone";
            this.mnuitem_VISIBLE_ZONE.Click += new System.EventHandler(this.mnuitem_VISIBLE_ZONE_Click);
            this.mnuitem_VISIBLE_ZONE.Popup += new System.EventHandler(this.ctxmWaferMap_Popup);
            // 
            // mnuitem_ZONE_OPTION
            // 
            this.mnuitem_ZONE_OPTION.Index = 14;
            this.mnuitem_ZONE_OPTION.Text = "Zone Option...";
            this.mnuitem_ZONE_OPTION.Click += new System.EventHandler(this.mnuitem_ZONE_OPTION_Click);
            // 
            // DefectMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DefectMap";
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.MenuItem menuItemDefectView;
        private System.Windows.Forms.MenuItem menuItemDefectView_All;
        private System.Windows.Forms.MenuItem menuItemDefectView_ImageDefectOnly;
        private System.Windows.Forms.MenuItem menuItemDefectView_NonImageDefectOnly;
        private System.Windows.Forms.MenuItem mnuitem_VISIBLE_ZONE;
        private System.Windows.Forms.MenuItem mnuitem_ZONE_OPTION;
    }
}
