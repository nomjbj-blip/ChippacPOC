using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPCMMapView : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        #region [ Data Field ]
        private string m_sCurrentDrawWaferID = string.Empty;
        private DACrux.Base.TPWafer[] mExcelSelDatas;
        #endregion

        #region [ Create & Close ]
        public frmPCMMapView()
        {
            InitializeComponent();
        }

        public frmPCMMapView(string strWaferSeq)
        {
            InitializeComponent();
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                DataSet ds = oPRBMapAnalysis.SelectWaferMapDrawData(DACrux.Base.Convert.longParse(strWaferSeq));

                if (ds != null && ds.Tables.Count > 0)
                {
                    //데이터를 유저컨트롤에 전달한 후 맵을 그린다.
                    tpucpcmViewer.DataSource = ds;
                    tpucpcmViewer.Draw();
                    m_sCurrentDrawWaferID = "WaferID";
                }
                else
                {
                    //초기화
                    //tpucMapView1.WaferClear();
                    throw new Exception("선택한 맵 데이터가 없습니다.");

                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void frmPCMMapView_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        #endregion

        #region [ Control Method ]

        #endregion


        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            try
            {
                if (wafer.Length != 1)
                {
                    MessageBox.Show(this, "Set only one wafer.", this.Name);
                    return;
                }

                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                DataSet ds = oPRBMapAnalysis.SelectWaferMapDrawData(DACrux.Base.Convert.longParse(wafer[0].WaferSeq));

                if (ds != null && ds.Tables.Count > 0)
                {
                    //데이터를 유저컨트롤에 전달한 후 맵을 그린다.
                    tpucpcmViewer.DataSource = ds;
                    tpucpcmViewer.Draw();
                    m_sCurrentDrawWaferID = "WaferID";
                }
                else
                {
                    //초기화
                    tpucpcmViewer.WaferClear();
                    throw new Exception("선택한 맵 데이터가 없습니다.");
                }

                this.TPWaferList = wafer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
            }
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            try
            {
                DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[strWafer.Length];
                for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
                {
                    oWafer[iWafer].WaferSeq = strWafer[iWafer];
                }

                DrawWafer(oWafer);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #region Excel Export

        public void ExportExcel()
        {
            tpucpcmViewer.ExportExcel();
        }

        #endregion 
    }

}
