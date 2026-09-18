using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmDefectPatternSearch에 대한 요약 설명입니다.
	/// </summary>
    public class frmDefectPatternSearch : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl, DACrux.Framework.Base.ISendDefect, DACrux.Framework.Base.IExportExcel
	{
		DPWafer [] wafer = null;

		public delegate void EventHandlerPattenrSearch(DPWafer [] wafer);

        private DACrux.SEMDMS.Control.DPUCPatternSearch dpucPatternSearch;
		private System.ComponentModel.IContainer components;

		public frmDefectPatternSearch()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
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
            this.dpucPatternSearch = new DACrux.SEMDMS.Control.DPUCPatternSearch();
            this.SuspendLayout();
            // 
            // dpucPatternSearch
            // 
            this.dpucPatternSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucPatternSearch.Location = new System.Drawing.Point(0, 0);
            this.dpucPatternSearch.Name = "dpucPatternSearch";
            this.dpucPatternSearch.Size = new System.Drawing.Size(744, 502);
            this.dpucPatternSearch.TabIndex = 0;
            this.dpucPatternSearch.Wafer = null;
            this.dpucPatternSearch.OnGallery += new DACrux.SEMDMS.Control.DPUCPatternSearch.EventHandlerPattenrSearch(this.dpucPatternSearch_OnGallery);
            // 
            // frmDefectPatternSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(744, 502);
            this.Controls.Add(this.dpucPatternSearch);
            this.Name = "frmDefectPatternSearch";
            this.Text = "Defect Pattern Search";
            this.Load += new System.EventHandler(this.frmDefectPatternSearch_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmDefectPatternSearch_Load(object sender, System.EventArgs e)
		{
            if (DesignMode) return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
		}

		private void dpucPatternSearch_OnGallery(DPWafer[] wafer)
		{
            frmDefectMapGallery oDefGallery = null;
            try
            {
                oDefGallery = new frmDefectMapGallery(false);
                oDefGallery.MdiParent = this.MdiParent;
                oDefGallery.Show();
                oDefGallery.DrawWafer(wafer);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
		}

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            try
            {
                dpucPatternSearch.Wafer = wafer;
                dpucPatternSearch.ListUpStepInfo();

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
                DACrux.Base.DPWafer[] oWafer = new Base.DPWafer[strWafer.Length];
                for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
                {
                    oWafer[iWafer].StepSeq = strWafer[iWafer];
                }

                DrawWafer(oWafer);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return dpucPatternSearch.GetSelectedDefect();
        }

        public void ExportExcel()
        {
            dpucPatternSearch.ExportExcel();
        }
	}
}
