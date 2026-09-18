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
    public partial class frmMapConfigUpdate : DACrux.Framework.Base.DACruxUXBasic01
    {
        private DataTable dtRowData = null;
        private DataTable dtConfig = null;

        public frmMapConfigUpdate()
        {
            InitializeComponent();
        }

        private void frmMapConfigUpdate_Load(object sender, EventArgs e)
        {
            dtStart.Value = DateTime.Now.AddMonths(-1);
            dtEnd.Value = DateTime.Now;
            BtnConfigListUp_Click(null, null);
        }



        #region [ Method ]

        /// <summary>
        /// Control 상에 Data List Up
        /// </summary>
        /// <param name="dlb"></param>
        /// <param name="dt"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        private void FillControlData(
            DACrux.Framework.Controls.DUCListBox dlb,
            DataTable dt,
            String displayMember,
            String valueMember
        )
        {
            dlb.DisplayMember = displayMember;
            dlb.ValueMember = valueMember;
            dlb.DataSource = dt;

            dlb.SelectedIndex = -1;
        }

        public void DrawMap(DataSet m_dsMap)
        {
            try
            {
                if (m_dsMap == null)
                    return;

                m_wMap.SetDefaultColors();
                SetMapBinColor(m_dsMap);

                m_wMap.WaferSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMap.OriginIndexX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMap.OriginIndexY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMap.OriginX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMap.OriginY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMap.FirstDieX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMap.FirstDieY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_wMap.EdgeSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_wMap.NotchType = DACrux.Base.Notch.Notch; //m_dsMap.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_wMap.DieMinX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_wMap.DieMinY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_wMap.DieMaxX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_wMap.DieMaxY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                TxtInfo.Text = string.Format("Max X,Y : ({0}, {1}) / Min X,Y ({2},{3})", m_wMap.DieMaxX, m_wMap.DieMaxY, m_wMap.DieMinX, m_wMap.DieMinY);

                int iXYDir = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_wMap.DieCalculation(true);

                // Map Bin Row Data ===================================================================================================
                m_wMap.DieClear();
                m_wMap.DataSource = m_dsMap.Tables["MAPDATA"];
                m_wMap.VisibleVIFail = true;
                m_wMap.VisibleFocusDie = true;
                m_wMap.VIMember = "BIN";
                m_wMap.DieFocusingType = Map.FocusType.Arraw;

                //Shot 관련 정보확인 및 Draw
                m_wMap.VisibleShot = false;
                if (m_dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && m_dsMap.Tables["SHOT_DEF"] != null && m_dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    int iTemp = 0;
                    m_wMap.VisibleShot = true;
                    foreach (DataRow dr in m_dsMap.Tables["SHOT_DEF"].Rows)
                    {
                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            m_wMap.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            m_wMap.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            m_wMap.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            m_wMap.ShotStartY = iTemp;
                    }
                }

                //Mark die 관련 정보 수집
                if (m_dsMap.Tables.IndexOf("MARKDATA") >= 0 && m_dsMap.Tables["MARKDATA"] != null && m_dsMap.Tables["MARKDATA"].Rows.Count > 0)
                {
                    foreach (DataRow dr in m_dsMap.Tables["MARKDATA"].Rows)
                    {
                        int iX = 0;
                        int iY = 0;

                        if (int.TryParse(dr["INDEX_X"].ToString(), out iX) == false)
                            continue;

                        if (int.TryParse(dr["INDEX_Y"].ToString(), out iY) == false)
                            continue;

                        if (m_wMap.IndexOf(iX, iY) < 0)
                        {
                            Base.Die oDie = new Base.Die();
                            oDie.IndexX = iX;
                            oDie.IndexY = iY;
                            oDie.DieProp = 2; // 실제 있는 Die 는 1번 Mark Die 는 2 번
                            oDie.BinNumber = 0;
                            m_wMap.AddDie(oDie);
                        }
                    }
                }

                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_wMap.Redraw();
                m_wMap.Refresh();
                m_wMap.Width = m_wMap.Width + 1; /// Size를 자동으로 조절하게 하는 Trip
                m_wMap.WaferDrawMode = Map.MapMode.Fit;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void SetMapBinColor(DataSet dsMap)
        {
            try
            {
                if (dsMap.Tables.IndexOf("MASTER_BIN") < 0 || dsMap.Tables["MASTER_BIN"] == null)
                    return;

                DataTable dtBinSum = dsMap.Tables["MASTER_BIN"];

                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    int nBin = -1;

                    if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                    {
                        if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN_COLOR"].ToString().Trim()) == false && dtBinSum.Rows[i]["BIN_COLOR"].ToString() != "#FFFFFF")
                        {
                            if (nBin > -1)
                            {
                                m_wMap.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["BIN_COLOR"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DataMapUpdate()
        {
            this.Cursor = Cursors.WaitCursor;

            FarPoint.Win.Spread.Model.CellRange[] oSelectItem = fsSheet_Sheet1.GetSelections();

            string strWaferID = string.Empty;
            string strWaferSeq = string.Empty;
            int iMapConfigSeq = -1;
            int iOldMapConfigSeq = -1;

            int idx = -1;

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();

            try
            {
                UpdateTextBox("Update 시작");

                iMapConfigSeq =int.Parse( dlbRevision.SelectedValue.ToString());

                //선택한 Wafer 에 대해서만 저장 한다.
                for (int i = 0; i < oSelectItem.Length; i++)
                {
                    for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                    {
                        idx = oSelectItem[i].Row + cr;
                        try
                        {
                            strWaferSeq = dtRowData.Rows[idx]["WAFER_SEQ"].ToString();
                            strWaferID = dtRowData.Rows[idx]["WAFER_ID"].ToString();

                            if (int.TryParse(dtRowData.Rows[idx]["MAP_CFG_SEQ"].ToString(), out iOldMapConfigSeq) == true)
                            {
                                if (iMapConfigSeq == iOldMapConfigSeq)
                                    throw new Exception("동일한 Seq 로 Update 할 수 없습니다.");
                            }

                            UpdateTextBox(">>");
                            UpdateTextBox(string.Format("Wafer Seq : {0}, Wafer ID : {1}", strWaferSeq, strWaferID));
                            oProbeMapAnalysis.WaferMapConfigUpdate(strWaferSeq, iMapConfigSeq.ToString());
                            SetSpreadColor(oSelectItem[i].Row + cr, Color.Green);
                        }
                        catch (Exception ex)
                        {
                            UpdateTextBox(string.Format("Wafer ID : {0}, Wafer Seq : {1}, Message : {2}", strWaferID,  strWaferSeq, ex.Message));
                            SetSpreadColor(oSelectItem[i].Row + cr, Color.Red);
                        }
                    }
                }

                UpdateTextBox("Update 완료");
            }
            catch (Exception ex)
            {
                UpdateTextBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetSpreadColor(int idx, Color rCol)
        {
            try
            {
                if (fsSheet.Disposing)
                {
                    return;
                }

                if (fsSheet.InvokeRequired)
                {
                    fsSheet.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            SetSpreadColor(idx, rCol);
                        }
                        ));
                }
                else
                {
                    fsSheet_Sheet1.Rows[idx].BackColor = rCol;
                }
            }
            catch (Exception)
            {
                /// 무시
            }
        }

        /// <summary>
        /// 현재 진행 상황 Text box Update
        /// </summary>
        /// <param name="data"></param>
        private void UpdateTextBox(string data)
        {
            if (TxtLog.InvokeRequired)
            {
                // 작업쓰레드인 경우
                TxtLog.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateTextBox(data);
                        }
                        ));
            }
            else
            {
                // UI 쓰레드인 경우
                TxtLog.AppendText(string.Format(">> {0}\n", data));
                TxtLog.AppendText("---------------------------------------------\n");

                TxtLog.Focus();
                TxtLog.SelectionStart = TxtLog.SelectionStart;

                if (TxtLog.Lines.Length > 1000)
                    TxtLog.Clear();
            }
        }
        #endregion [ Method ]   

        #region [ Event Handler ]

        private void BtnConfigListUp_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                dlbProduct.DataSource = null;
                dlbProgram.DataSource = null;
                dlbRevision.DataSource = null;

                TxtDir.Text = string.Empty;
                TxtAngle.Text = string.Empty;
                TxtindexX.Text = string.Empty;
                TxtindexY.Text = string.Empty;

                fsSheet_Sheet1.Rows.Clear();

                DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
                dtConfig = oProbeMapAnalysis.SelectMapConfigView(true);
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                {
                    DataTable dtFilter = dtConfig.DefaultView.ToTable(true, "TESTAREA");
                    FillControlData(
                      dlbTestArea,
                      dtFilter,
                      "TESTAREA",
                      "TESTAREA"
                      );
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dlbTestArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbProduct.DataSource = null;
            dlbProgram.DataSource = null;
            dlbRevision.DataSource = null;

            TxtDir.Text = string.Empty;
            TxtAngle.Text = string.Empty;
            TxtindexX.Text = string.Empty;
            TxtindexY.Text = string.Empty;

            if (dlbTestArea.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(true);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}'", strTestArea)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}'", strTestArea), "DEVICE_ALIAS").CopyToDataTable<DataRow>().DefaultView.ToTable(true, "DEVICE_ALIAS");

                FillControlData(
                       dlbProduct,
                       dtFilter,
                       "DEVICE_ALIAS",
                       "DEVICE_ALIAS"
                       );
            }
        }

        private void dlbProduct_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbProgram.DataSource = null;
            dlbRevision.DataSource = null;

            TxtDir.Text = string.Empty;
            TxtAngle.Text = string.Empty;
            TxtindexX.Text = string.Empty;
            TxtindexY.Text = string.Empty;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(true);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}'", strTestArea, strDevice)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}'", strTestArea, strDevice), "PROGRAM").CopyToDataTable<DataRow>().DefaultView.ToTable(true, "PROGRAM");

                FillControlData(
                         dlbProgram,
                         dtFilter,
                         "PROGRAM",
                         "PROGRAM"
                         );

            }
        }

        private void dlbProgram_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbRevision.DataSource = null;

            TxtDir.Text = string.Empty;
            TxtAngle.Text = string.Empty;
            TxtindexX.Text = string.Empty;
            TxtindexY.Text = string.Empty;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0
                || dlbProgram.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();
            string strProgram = dlbProgram.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(true);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}'", strTestArea, strDevice, strProgram)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}'", strTestArea, strDevice, strProgram), "REV_NO DESC").CopyToDataTable<DataRow>();

                FillControlData(
                       dlbRevision,
                       dtFilter,
                       "SEQ_REV_NO",
                       "MAP_CFG_SEQ"
                       );
            }
        }

        private void dlbRevision_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            TxtDir.Text = string.Empty;
            TxtAngle.Text = string.Empty;
            TxtindexX.Text = string.Empty;
            TxtindexY.Text = string.Empty;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0
                || dlbProgram.SelectedItems.Count <= 0
                || dlbRevision.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();
            string strProgram = dlbProgram.SelectedValue.ToString();
            string strConfigSeq = dlbRevision.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(true);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}' AND MAP_CFG_SEQ = '{3}'", strTestArea, strDevice, strProgram, strConfigSeq)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}' AND MAP_CFG_SEQ = '{3}'", strTestArea, strDevice, strProgram, strConfigSeq), "REV_NO").CopyToDataTable<DataRow>();

                TxtDir.Text = dtFilter.Rows[0]["ALTER_DIRECTION"].ToString();
                TxtAngle.Text = dtFilter.Rows[0]["ALTER_ANGLE"].ToString();
                TxtindexX.Text = dtFilter.Rows[0]["ALTER_INDEX_X"].ToString();
                TxtindexY.Text = dtFilter.Rows[0]["ALTER_INDEX_Y"].ToString();

                if (dtFilter.Rows.Count != 1)
                {
                    MessageBox.Show("Confiag 정보가 중복 되었습니다. 관리자에게 문의 바랍니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void chkDeleteFlag_CheckedChanged(object sender, EventArgs e)
        {
            BtnView_Click(null, null);
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DACrux.TEST.RO.ProbeMapAnalysis oReport = new DACrux.TEST.RO.ProbeMapAnalysis();

            try
            {
                fsSheet.ActiveSheet.Rows.Clear();

                if (dlbTestArea.SelectedItems.Count <= 0 || dlbProduct.SelectedItems.Count <= 0 || dlbProgram.SelectedItems.Count <= 0)
                {
                    MessageBox.Show("조회시 상위의 조회조건들은 선택해야 합니다.\nTest Area, Device Alias, Program", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string strTestArea = dlbTestArea.SelectedValue.ToString();
                string strDevice = dlbProduct.SelectedValue.ToString();
                string strProgram = dlbProgram.SelectedValue.ToString();

                dtRowData = oReport.GetMapConfigWaferList(dtStart.Value.ToString("yyyyMMdd"), dtEnd.Value.AddDays(1).ToString("yyyyMMdd"), strTestArea, strDevice, strProgram);
                if (dtRowData == null || dtRowData.Rows.Count <= 0)
                {
                    MessageBox.Show("Not Found Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                fsSheet.ActiveSheet.DataSource = dtRowData;
                fsSheet.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect | FarPoint.Win.Spread.OperationMode.ReadOnly;

                Utility.FPSpreadUtil.SetAutoColumnWidth(fsSheet_Sheet1);

                FarPoint.Win.Spread.CellType.NumberCellType nmbrcell = new FarPoint.Win.Spread.CellType.NumberCellType();
                nmbrcell.DecimalPlaces = 0;

                int iCol = -1;

                iCol = dtRowData.Columns.IndexOf("MAP_CFG_SEQ");
                if(iCol > -1)
                    fsSheet.ActiveSheet.Columns[iCol].CellType = nmbrcell;

                iCol = dtRowData.Columns.IndexOf("WAFER_SEQ");
                if (iCol > -1)
                    fsSheet.ActiveSheet.Columns[iCol].CellType = nmbrcell;

                iCol = dtRowData.Columns.IndexOf("LOT_SEQ");
                if (iCol > -1)
                    fsSheet.ActiveSheet.Columns[iCol].CellType = nmbrcell;                

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (fsSheet.ActiveSheet.SelectionCount <= 0)
            {
                MessageBox.Show("선택된 Wafer 가 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dlbRevision.SelectedItems.Count <= 0)
            {
                MessageBox.Show("선택된 Wafer Config 정보가 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(string.Format("현재 선택한 Wafer 를 Map Config Seq {0} 으로 변경 합니다. 계속 진행 하시겠습니까?", dlbRevision.SelectedValue.ToString()), "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            DataMapUpdate();

            MessageBox.Show("Update 완료 되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BtnView_Click(null, null);
        }

        private void fsSheet_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0)
                return;

            int iCol = -1;

            iCol = dtRowData.Columns.IndexOf("WAFER_SEQ");
            if (iCol > -1)
            {
                string strWaferSeq = fsSheet.ActiveSheet.Cells[e.Row, iCol].Text;

                DACrux.TEST.RO.ProbeMapAnalysis oTestMap = new RO.ProbeMapAnalysis();
                DataSet dsData = oTestMap.SelectWaferMapBasic(long.Parse(strWaferSeq));
                if (dsData == null || dsData.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                DrawMap(dsData);
            }
        }

        private void m_wMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            txtXIndex.Text = NewDie.IndexX.ToString();
            txtYIndex.Text = NewDie.IndexY.ToString();
            txtBin.Text = NewDie.BinNumber.ToString();
        }

        #endregion [ Event Handler ]




    }
}
