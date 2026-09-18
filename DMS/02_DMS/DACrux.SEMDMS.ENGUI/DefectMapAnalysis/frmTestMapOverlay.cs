using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.SEMDMS.Control;
//using DMSPlus.DataSelect;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmTestMapOverlay에 대한 요약 설명입니다.
	/// </summary>
    public class frmTestMapOverlay : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl
	{
        frmDMSStepSelect stepSelect = null;
        private DPUCTestMapOverlay dpucTestMapOverlay;
		private System.ComponentModel.IContainer components;

		public frmTestMapOverlay()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		
			try
			{
                stepSelect = new frmDMSStepSelect();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, this.Name);
			}
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
            this.dpucTestMapOverlay = new DACrux.SEMDMS.Control.DPUCTestMapOverlay();
            this.SuspendLayout();
            // 
            // dpucTestMapOverlay
            // 
            this.dpucTestMapOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucTestMapOverlay.Location = new System.Drawing.Point(0, 0);
            this.dpucTestMapOverlay.Name = "dpucTestMapOverlay";
            this.dpucTestMapOverlay.Size = new System.Drawing.Size(664, 477);
            this.dpucTestMapOverlay.TabIndex = 0;
            // 
            // frmTestMapOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(664, 477);
            this.Controls.Add(this.dpucTestMapOverlay);
            this.Name = "frmTestMapOverlay";
            this.Text = "Test Map Overlay";
            this.Load += new System.EventHandler(this.frmTestMapOverlay_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmTestMapOverlay_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{

			try
			{
                //waferSelect = new TEST.ENGUI.frmTPWaferSelect();

                //waferSelect.MultiSelect = true;
                //if (waferSelect.ShowDialog() == DialogResult.OK)
                //{
                //    dpucTestMapOverlay.ListUpWaferInfo(waferSelect.SelectedDatas);
                //}
                //else
                //{
                //    if(waferSelect != null) waferSelect.Dispose();
                //    waferSelect = null;
                //    this.Dispose();
                //}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, this.Name);
			}
		}


        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            try
            {
                //dpucTestMapOverlay.ListUpWaferInfo(wafer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }
	}
}
