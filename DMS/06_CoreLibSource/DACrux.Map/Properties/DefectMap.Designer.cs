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
            this.mnuDefectImageMark = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // ctxmWaferMap
            // 
            this.ctxmWaferMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitemMOUSEDRAGMODE_DEFECT,
            this.mnuREALSIZEDEFECT,
            this.mnuDefectImageMark});
            // 
            // mnuREALSIZEDEFECT
            // 
            this.mnuREALSIZEDEFECT.Index = 12;
            this.mnuREALSIZEDEFECT.Text = "Real Size Defect Drawing";
            this.mnuREALSIZEDEFECT.Visible = false;
            this.mnuREALSIZEDEFECT.Click += new System.EventHandler(this.mnuREALSIZEDEFECT_Click);
            // 
            // mnuitemMOUSEDRAGMODE_DEFECT
            // 
            this.mnuitemMOUSEDRAGMODE_DEFECT.Index = 13;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Text = "Defect Select";
            this.mnuitemMOUSEDRAGMODE_DEFECT.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_DEFECT_Click);
            // 
            // mnuDefectImageMark
            // 
            this.mnuDefectImageMark.Checked = true;
            this.mnuDefectImageMark.Index = 14;
            this.mnuDefectImageMark.Text = "Defect Image Mark";
            this.mnuDefectImageMark.Click += new System.EventHandler(this.mnuDefectImageMark_Click);
            // 
            // DefectMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DefectMap";
            this.ResumeLayout(false);

		}
		#endregion

    }
}
