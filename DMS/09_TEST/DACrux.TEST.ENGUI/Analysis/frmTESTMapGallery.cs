using System;
using System.Data;
using DACrux.Framework.Base;
using DACrux.TEST.Interface;
using DACrux.TEST.RO;
using System.Windows.Forms;


namespace DACrux.TEST.ENGUI
{
    public partial class frmTESTMapGallery
        : DACruxUXBasic01, iTESTControl, IExportExcel
    {
        #region [ Data Field ]

        private DACrux.Base.TPWafer[] mExcelSelDatas;

        #endregion

        #region [ Create & Close ]

        public frmTESTMapGallery(
            )
        {
            InitializeComponent();
        }

        public frmTESTMapGallery(
            string waferSeq
            )
            : this()
        {
        }

        #endregion [ Create & Close ]

        #region [ Event Handler ]

        private void tpucMapGallery1_OnParaAnalysis(
            object oWaferMap, 
            string strPara
            )
        {
            frmWaferParaAnalysis dlg = null;
            try
            {
                if (!(tpucMapGallery1.DataSource is DataSet))
                    return;

                dlg = new frmWaferParaAnalysis((tpucMapGallery1.DataSource as DataSet), strPara);
                dlg.Text = "WAFER ID";
                dlg.MdiParent = this.MdiParent;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.Show();
            }
            finally
            { 
            }
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        public void DrawWafer(
            Base.TPWafer[] wafer
            )
        {
            if (wafer == null || wafer.Length <= 0)
                return;

            tpucMapGallery1.WaferInfo = wafer;
            tpucMapGallery1.Draw();
        }

        public void ExportExcel()
        {
            tpucMapGallery1.ExportExcel();
        }

        #endregion [ Method ]
    }

}
