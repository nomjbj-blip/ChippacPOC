using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
	/// <summary>
	/// frmDefectMapGallery에 대한 요약 설명입니다.
	/// </summary>
    public class frmDefectMapGallery : DACruxUXBasicDefectLink, iSEMControl, ISendDefect, IExportExcel, iFileControl
	{
        private DACrux.SEMDMS.Control.DPUCDefectMapGallery map;
		private System.ComponentModel.IContainer components;
        bool m_IsMenuClick = true;

		public frmDefectMapGallery()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

		}

        public frmDefectMapGallery(bool IsMenuClick)
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
            this.map = new DACrux.SEMDMS.Control.DPUCDefectMapGallery();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(704, 445);
            this.map.TabIndex = 7;
            // 
            // frmDefectMapGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(704, 445);
            this.Controls.Add(this.map);
            this.Name = "frmDefectMapGallery";
            this.Text = "Defect Map Gallery";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDefectMapGallery_FormClosing);
            this.Load += new System.EventHandler(this.frmDefectMapGallery_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmDefectMapGallery_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
            else if (ExistsDefectArray)
            {
                map.Draw(DefectList);
            }
        }

        public void DrawWafer(Base.DefectList defectList)
        {
            map.Draw(defectList);
        }

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            map.SetData(wafer);
            map.Draw();
            this.DPWaferList = wafer;
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            DACrux.Base.DPWafer[] oWafer = new Base.DPWafer[strWafer.Length];
            for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
            {
                oWafer[iWafer].StepSeq = strWafer[iWafer];
            }

            DrawWafer(oWafer);
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return map.GetSelectedDefect();
        }

        public void ExportExcel()
        {
            map.ExportExcel();
        }

        private void frmDefectMapGallery_FormClosing(object sender, FormClosingEventArgs e)
        {
            map.SaveSettings();
        }
    }
}
