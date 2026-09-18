using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmZoneDefine에 대한 요약 설명입니다.
	/// </summary>
    public class frmZoneDefine : DACrux.Framework.Base.DACruxUXBasic01
	{
        private DACrux.SEMDMS.Control.DPUCZoneDefine dpucZoneDefine;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmZoneDefine()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		}

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
            this.dpucZoneDefine = new DACrux.SEMDMS.Control.DPUCZoneDefine();
			this.SuspendLayout();
			// 
			// dpucZoneDefine
			// 
			this.dpucZoneDefine.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dpucZoneDefine.Location = new System.Drawing.Point(0, 0);
			this.dpucZoneDefine.Name = "dpucZoneDefine";
			this.dpucZoneDefine.Size = new System.Drawing.Size(808, 550);
			this.dpucZoneDefine.TabIndex = 0;
			// 
			// frmZoneDefine
			// 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(808, 550);
            this.Controls.Add(this.dpucZoneDefine);
            this.Name = "frmZoneDefine";
            this.Text = "frmZoneDefine";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
