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

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmChartAnalysis : DACruxUXBasic01, iSEMControl
    {
        public frmChartAnalysis()
        {
            InitializeComponent();
        }

        public frmChartAnalysis(
            bool isMenuClick
            )
            :this()
        { 
        }

        //------------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void frmChartAnalysis_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode) return;
        }

        //--

        /// <summary>
        /// 조회 기능 및 chart 에 대한 option을 xml 형태로 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChartAnalysis_FormClosed(
            object sender, 
            FormClosedEventArgs e
            )
        {
            dpucChartAnalysis1.Closed();
        }

        #endregion [ Event Handler ]

        //------------------------------------------------------------------------------------------------

        #region [ Method ]

        //public void DrawWafer(
        //    long[] steps
        //    )
        //{
        //    DefectMapAnalysis oDefectMapAnal = new DefectMapAnalysis();
        //    DataTable dt = oDefectMapAnal.GetDefectImages(
        //        steps
        //        );

        //    DefectList defects = dpucChartAnalysis1.GetDefectList(
        //        dt
        //        );

        //    dpucChartAnalysis1.DataSource = defects;
        //    dpucChartAnalysis1.DataBinding();
        //}

        public void DrawWafer(
            DACrux.Base.DPWafer[] wafer
            )
        {
            dpucChartAnalysis1.SetData(wafer);
            RO.DefectMapAnalysis oDefectMapAnal = new RO.DefectMapAnalysis();
            long[] steps = new long[wafer.Length];
            for (int idx = 0; idx < wafer.Length; idx++)
            {
                steps[idx] = long.Parse(wafer[idx].StepSeq);
            }
            DataSet ds = oDefectMapAnal.GetChartAnalysis(
                steps
                );
            dpucChartAnalysis1.SetData(
                wafer
                );
            dpucChartAnalysis1.DataSource = ds;
            dpucChartAnalysis1.DataBinding();
        }

        #endregion [ Method ]

        //------------------------------------------------------------------------------------------------
    }
}
