using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Map;
using System.Data;
using DACrux.SEMDMS.Control;
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmDefecMapAnalysis에 대한 요약 설명입니다.
	/// </summary>
    public class frmDefectImgGalleryManyStep : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl
	{
		private DACrux.SEMDMS.Control.DPUCDefectImgGalleryManyStep dpucDefectImgGalleryManyStep;
		private System.ComponentModel.IContainer components = null;

		public frmDefectImgGalleryManyStep()
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
            this.components = new System.ComponentModel.Container();
            this.dpucDefectImgGalleryManyStep = new DACrux.SEMDMS.Control.DPUCDefectImgGalleryManyStep();
            this.SuspendLayout();
            // 
            // dpucDefectImgGalleryManyStep
            // 
            this.dpucDefectImgGalleryManyStep.AutoScroll = true;
            this.dpucDefectImgGalleryManyStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucDefectImgGalleryManyStep.Location = new System.Drawing.Point(0, 0);
            this.dpucDefectImgGalleryManyStep.Name = "dpucDefectImgGalleryManyStep";
            this.dpucDefectImgGalleryManyStep.Size = new System.Drawing.Size(776, 454);
            this.dpucDefectImgGalleryManyStep.TabIndex = 0;
            // 
            // frmDefectImgGalleryManyStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(776, 454);
            this.Controls.Add(this.dpucDefectImgGalleryManyStep);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmDefectImgGalleryManyStep";
            this.Text = "Defect Image Gallery";
            this.Load += new System.EventHandler(this.frmDefectImgGalleryManyStep_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmDefectImgGalleryManyStep_Load(object sender, System.EventArgs e)
		{
            if (DesignMode) return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
		}

		
        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            try
            {
                dpucDefectImgGalleryManyStep.Gallery(wafer);
                this.DPWaferList = wafer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            try
            {
                DACrux.Base.DPWafer[] stepSeq = new DACrux.Base.DPWafer[strWafer.Length];
                for (int a = 0; a < stepSeq.Length; a++) stepSeq[a].StepSeq = strWafer[a];
                DrawWafer(stepSeq);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

	}
}
