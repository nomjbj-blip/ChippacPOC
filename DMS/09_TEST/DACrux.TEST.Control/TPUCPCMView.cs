using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DACrux.TEST.Control
{
    public partial class TPUCPCMView : DACrux.Framework.Base.DACruxCTLBasic01, DACrux.Framework.Base.IExportExcel
    {

        #region [ Data Field ]

        private DataSet m_dsMap = null;
        private string strParaItem = string.Empty;
        private int m_nDisplayFaltAngle = -1;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object DataSource
        {
            set
            {
                if (value == null)
                    m_dsMap = null;
                else
                    m_dsMap = (DataSet)value;
            }
            get { return m_dsMap; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ParaItem
        {
            get { return strParaItem; }
            set { strParaItem = value; }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DACrux.Base.DisplayFlatZone DisplayFlatZone
        {
            // 맵은 저장된 Flat 위치로 그린 후 회전시켜 보여준다.
            set { m_nDisplayFaltAngle = (int)value; }
            get { return (DACrux.Base.DisplayFlatZone)m_nDisplayFaltAngle; }
        }


        #endregion

        #region [ Event Handler ]
        public TPUCPCMView()
        {
            InitializeComponent();
        }


        private void lsParaList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PCMDataDraw();
        }

        private void lsParaList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TxtItemFilter_KeyPress(object sender, KeyPressEventArgs e)
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


        private void BtnAnalysis_Click(object sender, EventArgs e)
        {
            PCMDataDraw();
        }

        private void LayoutPanel_MouseEnter(object sender, EventArgs e)
        {
            LayoutPanel.Focus();
        }

        private void LayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                LayoutPanel.SuspendLayout();
                foreach (System.Windows.Forms.Control ctrl in LayoutPanel.Controls)
                {

                    ctrl.Height = ctrl.Height + (ctrl.Margin.Top + ctrl.Margin.Bottom);
                    ctrl.Width = LayoutPanel.Width - 30;
                }
                LayoutPanel.ResumeLayout();
            }
            catch (Exception) { }
        }


        private void chkTotalPara_CheckedChanged(object sender, EventArgs e)
        {
            SetParaItem(m_dsMap);
        }


        private void TPUCPCMView_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region [ User Method ]

        public void Draw()
        {
            try
            {
                if (m_dsMap == null)
                    throw new Exception("Map Data Empty.");

                SetParaItem(m_dsMap);
                SetMapRawData(m_dsMap);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PCMDataDraw()
        {
            string strItem = string.Empty;
            string strWaferSeq = string.Empty;

            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;

            DataSet dsItem = null;
            DataSet dsTemp = null;
            DataTable dtGroup = null;
            DataTable dtTemp = null;

            try
            {
                oPRBMapAnalysis = new TEST.RO.ProbeMapAnalysis();

                if (LayoutPanel.Controls.Count > 0)
                    LayoutPanel.Controls.Clear();

                LayoutPanel.FlowDirection = FlowDirection.TopDown;
                LayoutPanel.WrapContents = false;

                dsItem = new DataSet();
                dtGroup = new DataTable();

                if (lsParaList.Items.Count <= 0 || lsParaList.SelectedItems.Count <= 0)
                    return;

                if (lsParaList.SelectedItems.Count >= 1000)
                    throw new Exception("1000 개 이상의 Item 은 선택 할 수 없습니다.");

                if (m_dsMap.Tables.IndexOf("WAFER_INFO") < 0 || m_dsMap.Tables["WAFER_INFO"] == null)
                    return;

                if (m_dsMap.Tables.IndexOf("MAPDATA") < 0 || m_dsMap.Tables["MAPDATA"] == null)
                    return;

                strWaferSeq = m_dsMap.Tables["WAFER_INFO"].Rows[0]["WAFER_SEQ"].ToString();
                strItem = lsParaList.SelectedItems[0].ToString();

                string[] ParaItem = lsParaList.SelectedItems.OfType<string>().ToArray();

                StatusMessage("Data 를 조회 중입니다.");
                //Sheet 상에 Row Data Binding
                Utility.FPSpreadUtil.InitSpread(fpGroupData, fpRawData);
                dsItem = oPRBMapAnalysis.SelectWaferParaData(DACrux.Base.Convert.longParse(strWaferSeq), ParaItem);
                if(dsItem == null || dsItem.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                StatusMessage("Data를 처리 중입니다.");
                if (dsItem.Tables.IndexOf("BOX") > -1 && dsItem.Tables["BOX"] != null && dsItem.Tables["BOX"].Rows.Count > 0)
                {
                    fpGroupData.ActiveSheet.DataSource = dsItem.Tables["BOX"];
                    Utility.FPSpreadUtil.SetAutoColumnWidth(fpGroupData.ActiveSheet);


                    dtGroup = dsItem.Tables["BOX"].DefaultView.ToTable(true, "ITEM");
                }

                if (dsItem.Tables.IndexOf("MAPDATA") > -1 && dsItem.Tables["MAPDATA"] != null && dsItem.Tables["MAPDATA"].Rows.Count > 0)
                {
                    fpRawData.ActiveSheet.DataSource = dsItem.Tables["MAPDATA"];
                    Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData.ActiveSheet);

                    //기존 MAPDATA 의 정보를 선택한 Para 를 추가하여 대체 한다.
                    m_dsMap.Tables.Remove("MAPDATA");
                    m_dsMap.AcceptChanges();

                    m_dsMap.Tables.Add(dsItem.Tables["MAPDATA"].Copy());
                    m_dsMap.AcceptChanges();
                }

                foreach (DataRow drData in dtGroup.Rows)
                {
                    double dbUpper = double.NaN;
                    double dbLower = double.NaN;

                    strItem = drData["ITEM"].ToString();
                    DataRow[] dr = m_dsMap.Tables["PARA_ITEM"].Select(string.Format("[PARAM_NAME] = '{0}'", strItem));
                    if (dr != null && dr.Length > 0)
                    {
                        if (string.IsNullOrEmpty(dr[0]["LSL"].ToString()) == true)
                            dbLower = double.NaN;
                        else
                            dbLower = DACrux.Base.Convert.doubleParse(dr[0]["LSL"].ToString());

                        if (string.IsNullOrEmpty(dr[0]["USL"].ToString()) == true)
                            dbUpper = double.NaN;
                        else
                            dbUpper = DACrux.Base.Convert.doubleParse(dr[0]["USL"].ToString());
                    }

                    dsTemp = new DataSet();
                    dtTemp = dsItem.Tables["BOX"].Select(string.Format("ITEM = '{0}'", strItem)).CopyToDataTable<DataRow>();
                    dtTemp.TableName = "BOX";
                    dsTemp.Tables.Add(dtTemp.Copy());

                    dtTemp = dsItem.Tables["MAPDATA"].DefaultView.ToTable(false, "DIE_NUM", "X", "Y", "BIN", strItem);
                    dtTemp.TableName = "MAPDATA";
                    dsTemp.Tables.Add(dtTemp.Copy());

                    //Chart가 닫혀진 상태에서는 chart 를 그리지는 않는다.
                    if(SPChart.Collapsed == false)
                        DrawChart(dsTemp, drData["ITEM"].ToString(), dbUpper, dbLower);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StatusMessage(null);
            }
        }


        private void DrawChart(DataSet ds, string strParaName, double USL, double LSL)
        {
            ucTrendChart container = null;
            try
            {
                if (ds == null || ds.Tables.Count == 0)
                    return;

                container = new ucTrendChart(strParaName, strParaName, USL, LSL, ds);
                //container.Title = strParaName;
                //container.AxisPara = strParaName;
                //container.DataSet = ds;
                //container.USL = USL;
                //container.LSL = LSL;
                //container.DrawChart();

                ControlAdd(container);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (container != null)
                //    container.Dispose();

                //container = null;
            }
        }

        private void ControlAdd(ucTrendChart container)
        {
            if (container == null) return;

            container.Height = container.Height + container.Margin.Top;
            container.Width = LayoutPanel.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
            LayoutPanel.Controls.Add(container);
            LayoutPanel.ResumeLayout();
        }

        private void SetParaItem(DataSet dsMap)
        {
            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string[] strFiler = null;
            string strTableName = string.Empty;
            try
            {
                if (chkTotalPara.Checked == true)
                    strTableName = "PARA_ITEM";
                else
                    strTableName = "PARA_LIST";

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

        private void SetMapRawData(DataSet dsMap)
        {
            DataTable dt = null;
            try
            {
                // DACrux.Utility.Component.InitSpread(fpRawData);

                fpRawData.ActiveSheet.Rows.Clear();

                if (dsMap.Tables.IndexOf("MAPDATA") < 0 || dsMap.Tables["MAPDATA"] == null)
                    return;

                dt = dsMap.Tables["MAPDATA"];

                if (dt.Columns.IndexOf("WAFER_SEQ") > -1)
                {
                    dt.Columns.Remove("WAFER_SEQ");
                }

                fpRawData_Sheet1.DataSource = dt;
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1);

                //DACrux.Utility.Component.SetSpreadData(dt, fpRawData_Sheet1, 100);
                //fpRawData_Sheet1.RowHeader.Columns[0].Width += 10;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WaferClear()
        {
            try
            {
                DACrux.Utility.Component.InitSpread(fpRawData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool MakeWaferRecipe(DataSet dsMap, ref DACrux.Base.WaferRecipe oWaferRecipe)
        {
            DataTable dtWaferInfo = null;
            DataTable dtMapData = null;
            int nRotationAngle = -1;

            try
            {
                int nIdx = dsMap.Tables.IndexOf("RECIPE");
                if (nIdx > -1 && dsMap.Tables[nIdx].Rows.Count > 0)
                    dtWaferInfo = dsMap.Tables[nIdx];

                nIdx = dsMap.Tables.IndexOf("MAPDATA");
                if (nIdx > -1 && dsMap.Tables[nIdx].Rows.Count > 0)
                    dtMapData = dsMap.Tables[nIdx];

                if (dtMapData == null)
                    return false;

                oWaferRecipe = new DACrux.Base.WaferRecipe();

                double m_dMargin = 0.95D;
                oWaferRecipe.WAFER_SIZE = 200.0d; // 기본값
                oWaferRecipe.EDGE_SIZE = 0.3d;

                if (dtWaferInfo == null || dtWaferInfo.Rows.Count < 1)
                {
                    throw new Exception("정의된 Map Define 정보가 없습니다. ");
                    //oWaferRecipe.FIRST_DIE_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([X])", "1=1").ToString());
                    //oWaferRecipe.FIRST_DIE_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([Y])", "1=1").ToString());
                    //oWaferRecipe.DIE_INDEX_MIN_X = oWaferRecipe.FIRST_DIE_X;
                    //oWaferRecipe.DIE_INDEX_MIN_Y = oWaferRecipe.FIRST_DIE_Y;
                    //oWaferRecipe.DIE_INDEX_MAX_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([X])", "1=1").ToString());
                    //oWaferRecipe.DIE_INDEX_MAX_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([Y])", "1=1").ToString());

                    //oWaferRecipe.NOTCH_TYPE = DACrux.Base.Notch.Flat;
                    //oWaferRecipe.ANGLE = 0;
                    //oWaferRecipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;
                    //m_nDisplayFaltAngle = 0;

                    //if (nAngle != -1)
                    //    oWaferRecipe.ANGLE = nAngle;

                    //int nOffsetXIdx = 2;
                    //int nOffsetYIdx = 2;

                    //oWaferRecipe.DIE_SIZE_X = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_X - oWaferRecipe.DIE_INDEX_MIN_X + nOffsetXIdx) * m_dMargin;
                    //oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y + nOffsetYIdx) * m_dMargin;
                }
                else
                {
                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_X"].ToString(), out oWaferRecipe.FIRST_DIE_X) == false)
                        oWaferRecipe.FIRST_DIE_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_Y"].ToString(), out oWaferRecipe.FIRST_DIE_Y) == false)
                        oWaferRecipe.FIRST_DIE_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([Y])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_X"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_X) == false)
                        oWaferRecipe.DIE_INDEX_MIN_X = oWaferRecipe.FIRST_DIE_X;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_Y) == false)
                        oWaferRecipe.DIE_INDEX_MIN_Y = oWaferRecipe.FIRST_DIE_Y;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_X"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_X) == false)
                        oWaferRecipe.DIE_INDEX_MAX_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_Y) == false)
                        oWaferRecipe.DIE_INDEX_MAX_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([Y])", "1=1").ToString());

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["NOTCH_TYPE"].ToString(), out oWaferRecipe.NOTCH_TYPE) == false)
                        oWaferRecipe.NOTCH_TYPE = DACrux.Base.Notch.Flat;

                    if (int.TryParse(dtWaferInfo.Rows[0]["ANGLE"].ToString(), out oWaferRecipe.ANGLE) == false)
                        oWaferRecipe.ANGLE = 180;

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["XY_DIRECTION"].ToString(), out oWaferRecipe.XYDIR) == false)
                        oWaferRecipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;

                    oWaferRecipe.WAFER_SIZE = DACrux.Base.Util.GetValue(dtWaferInfo.Rows[0]["WAFER_SIZE"].ToString(), 200000);

                    if (double.TryParse(dtWaferInfo.Rows[0]["EDGE_SIZE"].ToString(), out oWaferRecipe.EDGE_SIZE) == false)
                        oWaferRecipe.EDGE_SIZE = 3d;

                    m_nDisplayFaltAngle = oWaferRecipe.ANGLE;

                    //화면상에 보여 줄때 Bottom 으로 저장이 되어 있기 때문에 Rotation 각을 보고 Max 값을 치환 해준다.
                    nRotationAngle = (360 - (180 - oWaferRecipe.ANGLE)) % 360;

                    if (nRotationAngle == 90 || nRotationAngle == 270)
                    {
                        int iTempMax = 0;
                        iTempMax = oWaferRecipe.DIE_INDEX_MAX_X;

                        oWaferRecipe.DIE_INDEX_MAX_X = oWaferRecipe.DIE_INDEX_MAX_Y;
                        oWaferRecipe.DIE_INDEX_MAX_Y = iTempMax;

                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_X"].ToString(), out oWaferRecipe.DIE_SIZE_X) == false)
                    {
                        oWaferRecipe.DIE_SIZE_X = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_X - oWaferRecipe.DIE_INDEX_MIN_X) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }


                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_X"].ToString(), out oWaferRecipe.ORIGIN_DIE_X) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_X = oWaferRecipe.DIE_INDEX_MIN_X + (int)Math.Floor((double)oWaferRecipe.XDIES / 2.0d);
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_Y"].ToString(), out oWaferRecipe.ORIGIN_DIE_Y) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_Y = oWaferRecipe.DIE_INDEX_MIN_Y + (int)Math.Floor((double)oWaferRecipe.YDIES / 2.0d);
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_X"].ToString(), out oWaferRecipe.ORIGIN_X) == false)
                    {
                        if (oWaferRecipe.XDIES < 20)
                        {
                            if (oWaferRecipe.XDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_X = 0;
                            else
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90)
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else if (oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_X = -oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else
                                oWaferRecipe.ORIGIN_X = 0;
                        }
                    }


                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_Y"].ToString(), out oWaferRecipe.ORIGIN_Y) == false)
                    {
                        if (oWaferRecipe.YDIES < 20)
                        {
                            if (oWaferRecipe.YDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90 || oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else if (oWaferRecipe.ANGLE == 0)
                                oWaferRecipe.ORIGIN_Y = -oWaferRecipe.DIE_SIZE_Y / 2.0d;
                            else if (oWaferRecipe.ANGLE == 180)
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(lsParaList);
            sheet.Add();
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Para Info";

            sheet = new DACrux.Utility.ExcelSheet();
            foreach (System.Windows.Forms.Control cr in LayoutPanel.Controls)
            {
                if (cr.Name == "ucTrendChart")
                {
                    sheet.Add(cr);
                    sheet.Add();
                }
            }

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Trend Chart";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpRawData.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpGroupData.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Group Data";


            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion 



    }
}
