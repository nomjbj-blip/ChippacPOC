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
    public partial class frmDefectMap : DACrux.Framework.Base.DACruxUXBasicDefectLink, DACrux.SEMDMS.Interface.iSEMControl, DACrux.Framework.Base.ISendDefect
    {
        public frmDefectMap()
        {
            InitializeComponent();

            map.DefectMap.AddContextMenuItem("Export to Klarf file...", ExportToKlarFile);
        }

        private void frmDefectMap_Load(object sender, EventArgs e)
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

        private void ExportToKlarFile(object sender, EventArgs e)
        {
            using (frmExportToKlarf frm = new frmExportToKlarf())
            {
                frm.DefectList.AddRange(GetSelectedDefect());
                frm.ShowDialog();
            }
        }

        public void DrawWafer(Base.DPWafer[] wafer)
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

            Application.DoEvents();
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return map.GetSelectedDefect();
        }
    }
}
