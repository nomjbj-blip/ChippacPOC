using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using DACrux.Framework.Base;
using DACrux.Framework.Controls;
using DACrux.TEST.RO;
using System.Linq;
using FarPoint.Win;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPCMDataReport : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        private DataSet m_dsMap = null;
        private long[] Wafers;

        public frmPCMDataReport()
        {
            InitializeComponent();
        }

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(fpsReport);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Sheet";
            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion 


        #region [ Method ]

        private void SetParaItem(DataSet dsMap)
        {
            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string[] strFiler = null;
            string strTableName = "PARA_LIST";
            try
            {
                if (dsMap.Tables.IndexOf(strTableName) < 0 || dsMap.Tables[strTableName] == null)
                    return;

                dtTemp = dsMap.Tables[strTableName].Copy();
                dtTempFilter = dtTemp.Clone().Copy();
                dtTemp.CaseSensitive = false;

                lsParaList.Items.Clear();
                if (string.IsNullOrEmpty(TxtItemFilter.Text) == false)
                {
                    strFiler = TxtItemFilter.Text.Replace(",", ";").Replace("*", "%").Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string strVal in strFiler)
                    {
                        DataRow[] dr = dtTemp.Select(string.Format("PARAM_NAME LIKE '{0}'", strVal));
                        if (dr.Length > 0)
                        {
                            foreach (DataRow drFilter in dr)
                            {
                                dtTempFilter.Rows.Add(drFilter.ItemArray);
                            }
                        }
                    }

                    dtTemp = dtTempFilter.Copy();
                }

                string strItemName = string.Empty;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    strItemName = dtTemp.Rows[i]["PARAM_NAME"].ToString();

                    if (strItemName != "X" && strItemName != "Y" && strItemName != "WAFER_SEQ" && strItemName != "BIN")
                        lsParaList.Items.Add(strItemName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            Wafers = new long[wafer.Length];
            for (int i = 0; i < wafer.Length; i++)
            {
                Wafers[i] = Convert.ToInt32(wafer[i].WaferSeq);
            }

            DrawPCMReport(Wafers);
            this.TPWaferList = wafer;
        }

        public void DrawPCMReport(long[] WaferSeqs)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            try
            {
                chkTotalPara.Checked = true;
                oPRBMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();

                if (WaferSeqs == null || WaferSeqs.Length <= 0)
                    return;

                if (m_dsMap != null)
                    m_dsMap.Dispose();

                m_dsMap = oPRBMapAnalysis.SelectParaReportData(WaferSeqs);
                if (m_dsMap == null || m_dsMap.Tables.Count <= 0)
                {
                    MessageBox.Show("Not Found Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SetParaItem(m_dsMap);
                MeasureDataReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Data를 Load할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }


        private void MeasureDataReport()
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;

            DataTable dtData = null;
            string[] ParaItem = null;

            try
            {
                Utility.FPSpreadUtil.InitSpread(fpsReport);
                oPRBMapAnalysis = new TEST.RO.ProbeMapAnalysis();

                if (m_dsMap.Tables.IndexOf("WAFER_INFO") < 0 || m_dsMap.Tables["WAFER_INFO"] == null)
                    return;

                if (lsParaList.Items.Count <= 0)
                    return;

                if (chkTotalPara.Checked)
                {
                    ParaItem = lsParaList.Items.OfType<string>().ToArray();
                }
                else
                {
                    if (lsParaList.SelectedItems.Count <= 0)
                        throw new Exception("Item 선택 또는 Total Para 를 체크 하세요.");

                    ParaItem = lsParaList.SelectedItems.OfType<string>().ToArray();
                }


                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                //Sheet 상에 Row Data Binding
               
                dtData = oPRBMapAnalysis.SelectWaferParaDataMulti(Wafers, ParaItem);
                if (dtData == null || dtData.Rows.Count <= 0)
                    throw new Exception("Not Found Data");

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                fpsReport.ActiveSheet.DataSource = dtData;
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsReport.ActiveSheet);
                fpsReport.ActiveSheet.OperationMode = OperationMode.Normal | OperationMode.ReadOnly;
                fpsReport.ActiveSheet.FrozenColumnCount = 5;
                Utility.FPSpreadUtil.SetColBolder(fpsReport.ActiveSheet, 5);

                FarPoint.Win.Spread.CellType.NumberCellType NumberCell0 = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.NumberCellType NumberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumberCell0.Separator = ",";
                NumberCell0.DecimalPlaces = 0;
                fpsReport.ActiveSheet.Columns[6, 10].CellType = NumberCell0;
                NumberCell.Separator = ",";
                NumberCell.DecimalPlaces = 5;
                fpsReport.ActiveSheet.Columns[11, fpsReport.ActiveSheet.Columns.Count - 1].CellType = NumberCell;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        #endregion [ Method ]


        #region [ Event Handler ]
        private void frmPCMDataReport_Load(
            object sender,
            EventArgs e
            )
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

        //--

        private void TxtItemFilter_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    SetParaItem(m_dsMap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void chkTotalPara_CheckedChanged(object sender, EventArgs e)
        {
            lsParaList.Enabled = !chkTotalPara.Checked;
        }

        private void BtnAnalysis_Click(object sender, EventArgs e)
        {
            MeasureDataReport();
        }

        #endregion [ Event Handler ]
    }
}
