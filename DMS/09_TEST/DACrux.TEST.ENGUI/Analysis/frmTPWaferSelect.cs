using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
	/// <summary>
	/// frmTPWaferSelect에 대한 요약 설명입니다.
	/// </summary>
    public class frmTPWaferSelect : DACrux.Framework.Base.DACruxUXBasic01
	{
		private TPUCSelectWafer tpucSelectWafer1;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public TPWafer[] m_SelDatas = null;
		
		public TPWafer[] SelectedDatas
		{
			get
			{
				return m_SelDatas;
			}
		}

		public frmTPWaferSelect()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmTPWaferSelect));
			this.tpucSelectWafer1 = new TPUCSelectWafer();
			this.SuspendLayout();
			// 
			// tpucSelectWafer1
			// 
			this.tpucSelectWafer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tpucSelectWafer1.Location = new System.Drawing.Point(0, 0);
			this.tpucSelectWafer1.MultiWafer = false;
			this.tpucSelectWafer1.Name = "tpucSelectWafer1";
			this.tpucSelectWafer1.Size = new System.Drawing.Size(692, 573);
			this.tpucSelectWafer1.TabIndex = 0;
			this.tpucSelectWafer1.OnSelected += new Selected(this.tpucSelectWafer1_OnSelected);
			// 
			// frmTPWaferSelect
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(692, 573);
			this.Controls.Add(this.tpucSelectWafer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmTPWaferSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Wafer Select";
			this.ResumeLayout(false);

		}
		#endregion

		private void tpucSelectWafer1_OnSelectionCancel(object sender)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
		}

		private void tpucSelectWafer1_OnSelected(object sender, TPWafer[] SelDatas)
		{
			m_SelDatas = SelDatas;
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}

		public bool MultiSelect
		{
			set
			{
				tpucSelectWafer1.MultiWafer = value;
			}get
			 {
				 return tpucSelectWafer1.MultiWafer;
			 }
		}
	}
}
