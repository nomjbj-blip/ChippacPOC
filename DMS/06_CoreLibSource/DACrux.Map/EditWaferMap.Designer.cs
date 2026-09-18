namespace DACrux.Map
{
	partial class EditWaferMap
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
				if (m_DieIndexer != null) m_DieIndexer = null;
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
            this.mnuitemEDITMODE_SKIP = new System.Windows.Forms.MenuItem();
            this.mnuitemEDITMODE_ADD = new System.Windows.Forms.MenuItem();
            this.mnuitemEDITMODE_MARKDIE = new System.Windows.Forms.MenuItem();
            this.mnuitemEDITMODE_DELETE = new System.Windows.Forms.MenuItem();
            this.mnuitemEDITMODE_MARKFIRST = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPMODE_EDIT = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // ctxmWaferMap
            // 
            this.ctxmWaferMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitemMAPMODE_EDIT});
            // 
            // mnuitemMAPSELECTSTYLE_CIRCLE
            // 
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            // 
            // mnuitem_SPRIT2
            // 
            this.mnuitem_SPRIT2.MergeOrder = 6;
            // 
            // mnuitemEDITMODE_SKIP
            // 
            this.mnuitemEDITMODE_SKIP.Index = 2;
            this.mnuitemEDITMODE_SKIP.MergeOrder = 19;
            this.mnuitemEDITMODE_SKIP.Text = "Skip Die";
            this.mnuitemEDITMODE_SKIP.Click += new System.EventHandler(this.mnuitemEDITMODE_SKIP_Click);
            // 
            // mnuitemEDITMODE_ADD
            // 
            this.mnuitemEDITMODE_ADD.Index = 1;
            this.mnuitemEDITMODE_ADD.MergeOrder = 18;
            this.mnuitemEDITMODE_ADD.Text = "Add Die";
            this.mnuitemEDITMODE_ADD.Click += new System.EventHandler(this.mnuitemEDITMODE_ADD_Click);
            // 
            // mnuitemEDITMODE_MARKDIE
            // 
            this.mnuitemEDITMODE_MARKDIE.Index = 0;
            this.mnuitemEDITMODE_MARKDIE.MergeOrder = 17;
            this.mnuitemEDITMODE_MARKDIE.Text = "Mark Die";
            this.mnuitemEDITMODE_MARKDIE.Click += new System.EventHandler(this.mnuitemEDITMODE_MARKDIE_Click);
            // 
            // mnuitemEDITMODE_DELETE
            // 
            this.mnuitemEDITMODE_DELETE.Index = 3;
            this.mnuitemEDITMODE_DELETE.MergeOrder = 20;
            this.mnuitemEDITMODE_DELETE.Text = "Delete Die";
            this.mnuitemEDITMODE_DELETE.Click += new System.EventHandler(this.mnuitemEDITMODE_DELETE_Click);
            // 
            // mnuitemEDITMODE_MARKFIRST
            // 
            this.mnuitemEDITMODE_MARKFIRST.Index = 4;
            this.mnuitemEDITMODE_MARKFIRST.MergeOrder = 21;
            this.mnuitemEDITMODE_MARKFIRST.Text = "First Die";
            this.mnuitemEDITMODE_MARKFIRST.Click += new System.EventHandler(this.mnuitemEDITMODE_MARKFIRST_Click);
            // 
            // mnuitemMAPMODE_EDIT
            // 
            this.mnuitemMAPMODE_EDIT.Index = 12;
            this.mnuitemMAPMODE_EDIT.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitemEDITMODE_MARKDIE,
            this.mnuitemEDITMODE_ADD,
            this.mnuitemEDITMODE_SKIP,
            this.mnuitemEDITMODE_DELETE,
            this.mnuitemEDITMODE_MARKFIRST});
            this.mnuitemMAPMODE_EDIT.MergeOrder = 100;
            this.mnuitemMAPMODE_EDIT.Text = "Edit Map";
            // 
            // EditWaferMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "EditWaferMap";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WaferMap_Paint);
            this.ResumeLayout(false);

		}

		#endregion
	}
}
