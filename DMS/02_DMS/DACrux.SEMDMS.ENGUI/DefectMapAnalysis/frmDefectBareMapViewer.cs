using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectBareMapViewer : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl, DACrux.Framework.Base.ISendDefect, DACrux.Framework.Base.IExportExcel
    {
        public frmDefectBareMapViewer()
        {
            InitializeComponent();
        }

        public frmDefectBareMapViewer(bool IsMenuClick)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

        }

        private void frmDefectBareMapViewercs_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {

            dpucBareDefecMapViewers.SetData(wafer);
            dpucBareDefecMapViewers.Draw();
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

            Application.DoEvents();

        }

        public Base.Defect[] GetSelectedDefect()
        {
            return dpucBareDefecMapViewers.GetSelectedDefect();
        }

        public void ExportExcel()
        {
            dpucBareDefecMapViewers.ExportExcel();
        }
    }
}
