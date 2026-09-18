using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;
using DACrux.SEMDMS.RO;
using System.Diagnostics;
using DACrux.Framework;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmChartAnalysisNew 
        : DACruxUXBasicDefectLink, ISendDefect, iSEMControl, IExportExcel
    {
        public frmChartAnalysisNew()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmChartAnalysisNew_Load);
            this.FormClosed += new FormClosedEventHandler(frmChartAnalysisNew_FormClosed);
        }

        #region [ Event Handler ]

        //--

        void frmChartAnalysisNew_Load(
            object sender, 
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            Application.DoEvents();

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWafer(WaferList);
            }
            else if (ExistsDefectArray)
            {
                dpucChartAnal.DataSource = DefectList;
                dpucChartAnal.DataBinding();
            }
        }

        //--

        void frmChartAnalysisNew_FormClosed(
            object sender, 
            FormClosedEventArgs e
            )
        {
            dpucChartAnal.Closed();
        }

        //--

        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private void DrawWafer(
            String[] wafers
            )
        {
            DPWafer[] dpWafer = new DPWafer[wafers.Length];
            for (int idx = 0; idx < dpWafer.Length; idx++)
            {
                dpWafer[idx].StepSeq = wafers[idx];
            }

            DrawWafer(
                dpWafer
                );
        }

        public void DrawWafer(
            DPWafer[] wafers
            )
        {
            long[] steps = new long[wafers.Length];
            for (int idx = 0; idx < wafers.Length; idx++)
            {
                steps[idx] = long.Parse(wafers[idx].StepSeq);
            }

            dpucChartAnal.SetData(
                wafers
                );

            DrawWafer(
                steps
                );
        }

        //--

        private void DrawWafer(
            long[] steps
            )
        {
            DefectMapAnalysis oDefectMapAnal = new DefectMapAnalysis();
            DefectList defects = oDefectMapAnal.GetDefectDataToObjectArray_Comp(steps);

            dpucChartAnal.DataSource = defects;
            dpucChartAnal.DataBinding();
        }

        //--

        public Defect[] GetSelectedDefect()
        {
            return dpucChartAnal.GetSelectedDefect();
        }
        #endregion [ Method ]

        public void ExportExcel()
        {
            dpucChartAnal.ExportExcel();
        }
    }
}
