using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DACrux.Mining
{
	/// <summary>
	/// frmAddLot에 대한 요약 설명입니다.
	/// </summary>
	public partial class frmAddLot : System.Windows.Forms.Form
	{
		public frmAddLot()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		}

		private void butOK_Click(object sender, System.EventArgs e)
		{
			if(txtLotID.Text.Trim().Length==0) 
			{
				this.DialogResult = DialogResult.Cancel;
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
			this.Close();
		}

		private void butCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		public string GetLotID()
		{
			return txtLotID.Text;
		}
	}
}
