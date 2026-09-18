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
    public partial class frmSingleMapView : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        #region [ Data Field ]
        private string m_sCurrentDrawWaferID = string.Empty;
        private DACrux.Base.TPWafer[] mExcelSelDatas;
        #endregion

        #region [ Create & Close ]
        public frmSingleMapView()
        {
            InitializeComponent();
        }

        public frmSingleMapView(string strWaferSeq)
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
                    tpucMapView1.DataSource = ds;
                    tpucMapView1.Draw();
                    m_sCurrentDrawWaferID = "WaferID";
                }
                else
                {
                    //초기화
                    tpucMapView1.WaferClear();
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

        private void frmSingleMapView_Load(object sender, EventArgs e)
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

        private void tpucMapSelector1_OnSelectedDraw(object sender, DACrux.Base.TPWafer[] SelDatas)
        {
           // DACrux.YMS.RO.SingleMapView oSingleMapView = null;
            mExcelSelDatas = null;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                
                if (SelDatas == null || SelDatas.Length < 1)
                    throw new Exception("선택된 Wafer가 없습니다.");

                mExcelSelDatas = SelDatas;

                //oSingleMapView = new DACrux.YMS.RO.SingleMapView();
                //DataSet ds = oSingleMapView.SelectWaferMapDrawData(SelDatas[0].WaferSeq
                //                                                    , SelDatas[0].Testarea
                //                                                    , SelDatas[0].CusDevice
                //                                                    , SelDatas[0].Program
                //                                                    , SelDatas[0].ProgramRev
                //                                                    , DACrux.Base.GlobalVariable.UserID);

                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    //데이터를 유저컨트롤에 전달한 후 맵을 그린다.
                //    if (tpucMapView1.DrawGradationDie == true)
                //        tpucMapView1.DrawGradationDie = false;

                //    tpucMapView1.DataSource = ds;
                //    tpucMapView1.Draw();
                //    m_sCurrentDrawWaferID = SelDatas[0].WaferID;
                //}
                //else
                //{
                //    //초기화
                //    tpucMapView1.WaferClear();
                //    throw new Exception("선택한 맵 데이터가 없습니다.");
                   
                //}
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

        private void tpucMapView1_OnDutAnalysis(object oWaferMap, string[] aCheckedBins)
        {
            frmDutByYieldReport dlg = null;
            DataTable dt = null;
            try
            {
                bool IsAll = true;
                for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
                {
                    if (this.MdiParent.MdiChildren[i].Text == m_sCurrentDrawWaferID)
                    {
                        this.MdiParent.MdiChildren[i].Focus();
                        return;
                    }
                }

                DACrux.Map.WaferMap map = (DACrux.Map.WaferMap)oWaferMap;

                if (((DataTable)map.DataSource) == null)
                    return;

                string sFilter = string.Empty;
                for (int i = 0; i < aCheckedBins.Length; i++)
                {
                    if (aCheckedBins[i] == "ALL")
                    {
                        sFilter = string.Empty;
                        break;
                    }
                    sFilter += "OR ";
                    sFilter += string.Format("[BIN] = '{0}' ", aCheckedBins[i]);
                }

                if (string.IsNullOrEmpty(sFilter) == true)
                {
                    sFilter = "1=1";
                    IsAll = true;
                }
                else
                {
                    sFilter = sFilter.Substring(3);
                    IsAll = false;
                }

                dt = ((DataTable)map.DataSource).Select(sFilter).CopyToDataTable<DataRow>();

                dlg = new frmDutByYieldReport(dt, IsAll);
                dlg.Text = m_sCurrentDrawWaferID;
                dlg.MdiParent = this.MdiParent;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.Show();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }
        #endregion

        private void tpucMapView1_OnParaAnalysis(object oWaferMap, string strPara)
        {
            frmWaferParaAnalysis dlg = null;
            try
            {
                if ((DataSet)tpucMapView1.DataSource == null)
                    return;

                dlg = new frmWaferParaAnalysis((DataSet)tpucMapView1.DataSource, strPara);
                dlg.Text = m_sCurrentDrawWaferID;
                dlg.MdiParent = this.MdiParent;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.Show();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            this.Cursor = Cursors.WaitCursor;

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
                    tpucMapView1.DataSource = ds;
                    tpucMapView1.Draw();
                    m_sCurrentDrawWaferID = "WaferID";
                }
                else
                {
                    //초기화
                    tpucMapView1.WaferClear();
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
                this.Cursor = Cursors.Default;
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
            tpucMapView1.ExportExcel();
        }

        #endregion 
    }

}
