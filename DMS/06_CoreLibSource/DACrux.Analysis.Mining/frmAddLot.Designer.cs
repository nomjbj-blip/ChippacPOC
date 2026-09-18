namespace DACrux.Mining
{
    partial class frmAddLot
    {
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        #region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtLotID = new System.Windows.Forms.TextBox();
			this.butOK = new System.Windows.Forms.Button();
			this.butCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtLotID
			// 
			this.txtLotID.Location = new System.Drawing.Point(24, 16);
			this.txtLotID.Name = "txtLotID";
			this.txtLotID.Size = new System.Drawing.Size(192, 21);
			this.txtLotID.TabIndex = 0;
			this.txtLotID.Text = "";
			// 
			// butOK
			// 
			this.butOK.Location = new System.Drawing.Point(24, 48);
			this.butOK.Name = "butOK";
			this.butOK.TabIndex = 1;
			this.butOK.Text = "Ok";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(136, 48);
			this.butCancel.Name = "butCancel";
			this.butCancel.TabIndex = 1;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// frmAddLot
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(234, 80);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.txtLotID);
			this.Controls.Add(this.butCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddLot";
			this.ShowInTaskbar = false;
			this.Text = "Add Lot";
			this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.TextBox txtLotID;
		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Button butCancel;
    }
}
