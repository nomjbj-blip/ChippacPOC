using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Map;
using System.Data;
using DACrux.SEMDMS.ENGUI;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmDefecMapAnalysis에 대한 요약 설명입니다.
	/// </summary>
    public class frmRepeatedDefect : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl
	{
        private DACrux.SEMDMS.Control.DPUCRepeatedDefect dpucDetailAnalysisRepeat;

		private System.ComponentModel.IContainer components = null;

		public frmRepeatedDefect()
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
            this.dpucDetailAnalysisRepeat = new DACrux.SEMDMS.Control.DPUCRepeatedDefect();
            this.SuspendLayout();
            // 
            // dpucDetailAnalysisRepeat
            // 
            this.dpucDetailAnalysisRepeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucDetailAnalysisRepeat.Location = new System.Drawing.Point(0, 0);
            this.dpucDetailAnalysisRepeat.Name = "dpucDetailAnalysisRepeat";
            this.dpucDetailAnalysisRepeat.Size = new System.Drawing.Size(816, 525);
            this.dpucDetailAnalysisRepeat.TabIndex = 0;
            // 
            // frmRepeatedDefect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(816, 525);
            this.Controls.Add(this.dpucDetailAnalysisRepeat);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmRepeatedDefect";
            this.Text = "Repeated Defect";
            this.Load += new System.EventHandler(this.frmRepeatedDefect_Load);
            this.ResumeLayout(false);

		}
		#endregion

        private void frmRepeatedDefect_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

        }


        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            try
            {
                string[] stepSeq = new string[wafer.Length];
                for (int a = 0; a < stepSeq.Length; a++) stepSeq[a] = string.Format("{0}", wafer[a].StepSeq);
                dpucDetailAnalysisRepeat.WaferInfo(stepSeq);
                dpucDetailAnalysisRepeat.Function("radioButtonRepeater");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

	}
}
