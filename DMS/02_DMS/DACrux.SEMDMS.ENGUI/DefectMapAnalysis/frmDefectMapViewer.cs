using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Framework;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectMapViewer : DACruxUXBasicDefectLink, iSEMControl, ISendDefect, IExportExcel, iFileControl
    {
        public frmDefectMapViewer()
        {
            InitializeComponent();
        }

        public frmDefectMapViewer(bool IsMenuClick)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
        }

        private void frmDefectMapViewer_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            Application.DoEvents();

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
            else if (ExistsDefectArray)
            {
                map.Draw(DefectList);
            }
        }

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            map.Wafer = wafer;
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

            Application.DoEvents();

        }

        public Base.Defect[] GetSelectedDefect()
        {
            return map.GetSelectedDefect();
        }

        public void DrawWafer(Base.DefectList defectList)
        {
            map.Draw(defectList);
        }

        public void ExportExcel()
        {
            map.ExportExcel();
        }
    }
}
