using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Base;
using System.Data;
using DACrux.SEMDMS.Control;
//using Miracom.Common.Base;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class frmDefectSourceAnalysis : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl, DACrux.Framework.Base.ISendDefect, DACrux.Framework.Base.IExportExcel
    {
        private DACrux.SEMDMS.Control.DPUCDefectSourceAnalysis dpucDetailAnalysisDsa;

        private System.ComponentModel.IContainer components = null;

        public frmDefectSourceAnalysis()
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.dpucDetailAnalysisDsa = new DACrux.SEMDMS.Control.DPUCDefectSourceAnalysis();
            this.SuspendLayout();
            // 
            // dpucDetailAnalysisDsa
            // 
            this.dpucDetailAnalysisDsa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucDetailAnalysisDsa.Location = new System.Drawing.Point(0, 0);
            this.dpucDetailAnalysisDsa.Name = "dpucDetailAnalysisDsa";
            this.dpucDetailAnalysisDsa.Size = new System.Drawing.Size(1179, 590);
            this.dpucDetailAnalysisDsa.TabIndex = 0;
            // 
            // frmDefectSourceAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(1179, 590);
            this.Controls.Add(this.dpucDetailAnalysisDsa);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmDefectSourceAnalysis";
            this.Text = "Defect Source Analysis";
            this.Load += new System.EventHandler(this.frmDefectSourceAnalysis_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmDefectSourceAnalysis_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) 
                return;

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                long[] stepSeqArr = new long[WaferList.Length];

                for (int i = 0; i < WaferList.Length; i++)
                    stepSeqArr[i] = DACrux.Base.Convert.longParse(WaferList[i]);

                DrawWafer(stepSeqArr);
            }
        }

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            this.DPWaferList = wafer;

            if (wafer == null || wafer.Length == 0)
                return;

            long[] stepSeqArr = new long[wafer.Length];

            for (int i = 0; i < wafer.Length; i++)
                stepSeqArr[i] = DACrux.Base.Convert.longParse(wafer[i].StepSeq);

            DrawWafer(stepSeqArr);
        }

        public void DrawWafer(long[] stepSeqArr)
        {
            dpucDetailAnalysisDsa.WaferInfo(stepSeqArr);
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return dpucDetailAnalysisDsa.GetSelectedDefect();
        }

        public void ExportExcel()
        {
            dpucDetailAnalysisDsa.ExportExcel();
        }
    }
}
