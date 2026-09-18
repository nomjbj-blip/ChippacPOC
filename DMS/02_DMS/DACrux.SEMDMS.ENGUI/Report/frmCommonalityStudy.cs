using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using DACrux.Framework.Base;
using DACrux.Utility;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmCommonalityStudy : DACruxUXBasic01, IExportExcel
    {
        #region [ Data Field ]

        DACrux.SEMDMS.ENGUI.frmCommonalityLotList frmLotList = null;
        enum STATUS { LOT = 0, WAFER }
        enum TQC_LOT_HIS { FACTORY, LOT_ID, LOT_TYPE, TRAN_TIME, RECIPE, FLOW, FLOW_DESC, FLOW_NO, OPER, OPER_DESC, OPER_LONG_DESC, FLOW_OPER_SEQ, FLOW_AREA, RES_ID, RES_MODEL, RES_AREA }
        enum TQC_LOT_HIS_GRP { LOT_ID, LOT_TYPE, RECIPE, FLOW, FLOW_DESC, FLOW_NO, OPER, OPER_DESC, OPER_LONG_DESC, FLOW_OPER_SEQ, FLOW_AREA, RES_ID, RES_MODEL, RES_AREA }

        private string[] m_strLotFieldAvilable = new string[] { "LOT_TYPE", "RECIPE", "FLOW_NO", "OPER_LONG_DESC", "FLOW_AREA", "RES_AREA" };
        private string[] m_strLotFieldSelect = new string[] { "FLOW", "FLOW_DESC", "OPER", "OPER_DESC", "RES_ID", "RES_MODEL" };

        private string[] m_strWaferFieldAvilable = new string[] { "PARA_2", "PARA_3", "PARA_4", "PARA_5", "PARA_6", "PARA_7", "PARA_8", "PARA_9", "PARA_10", "PARA_11", "PARA_12", "PARA_13", "PARA_14", "PARA_15" };
        private string[] m_strWaferFieldSelect = new string[] { "LPT", "OPN", "RECIPE", "DESCRIPTION", "EQUIP", "PARA_1" };

        private int iLotNoStartCol = -1;
        private int iLotDetailStartCol = -1;

        private int iWaferNoStartCol = -1;
        private STATUS mode = STATUS.LOT;
        private List<string> m_Item = new List<string>();
        private DataTable dtHeader = null;
        private DataTable dtLotList = null;

        private Color cGood = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
        private Color cBad = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));

        #endregion [ Data Field ]

        //-------------------------------------------------------------------------------------------------

        #region [ Constrator ]
        public frmCommonalityStudy()
        {
            InitializeComponent();
        }
        #endregion [ Constrator ]

        //-------------------------------------------------------------------------------------------------

        #region [ Method ]

        public void ExportExcel()
        {
            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.SheetName = "Summary";
            sheet1.Add(fpsCommon);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);

            if (TabItem.SelectedIndex == 1)
            {
                sheet1 = new ExcelSheet();
                sheet1.SheetName = "Detail Row";
                sheet1.Add(fpsWaferDetail);
                e.SheetList.Add(sheet1);
            }

            ExcelExportManager.Export(e);
        }

        private void oLotList_OnSelected(object sender, string[] strLotList)
        {
            foreach (string str in strLotList)
            {
                lsTotal.Items.Add(str);
            }
        }

        private void fnDataViewLotBase()
        {
            DataTable dtLotSort = null;
            DataSet dsData = null;
            List<string> strItem = new List<string>();
            List<string> strGoodLot = new List<string>();
            List<string> strBadLot = new List<string>();

            DACrux.Common.RO.ComConfiguration oCom = new Common.RO.ComConfiguration();

            int iMaxLotCnt = -1;
            int iTotalGood = -1;
            int iTotalBad = -1;

            int iColGood = -1;
            int iColBad = -1;

            try
            {
                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (lsSelectionLot.Items.Count <= 0)
                {
                    MessageBox.Show("Selection Item 을 선택 하세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsSelectionLot.Focus();
                    return;
                }

                if (lsGoodLot.Items.Count <= 0)
                {
                    MessageBox.Show("Good Lot 에 Lot List 가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsGoodLot.Focus();
                    return;
                }

                if (lsBadLot.Items.Count <= 0)
                {
                    MessageBox.Show("Bad Lot 에 Lot List 가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsBadLot.Focus();
                    return;
                }

                foreach (ListViewItem Item in lsSelectionLot.Items)
                {
                    strItem.Add(Item.SubItems[0].Text.ToString());
                }

                foreach (object row in lsGoodLot.Items)
                {
                    strGoodLot.Add(row.ToString());
                }

                foreach (object row in lsBadLot.Items)
                {
                    strBadLot.Add(row.ToString());
                }

                dsData = oCom.GetCommonality(strGoodLot.ToArray(), strBadLot.ToArray(), strItem.ToArray(), chkInspectionDM.Checked);
                if (dsData == null || dsData.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                MainForm.SetStatusMessage("Data 화면에 출력 중입니다.");
                m_Item = new List<string>();
                foreach (ListViewItem Item in lsSelectionLot.Items)
                {
                    m_Item.Add(Item.SubItems[0].Text.ToString());
                }

                iMaxLotCnt = int.Parse(dsData.Tables["GRP"].Compute("MAX(TOTAL)", string.Empty).ToString());
                iTotalGood = dsData.Tables["LOT"].DefaultView.ToTable(true, "SECTION", "LOT_ID").Select("SECTION = 'GOOD'").Length;
                iTotalBad = dsData.Tables["LOT"].DefaultView.ToTable(true, "SECTION", "LOT_ID").Select("SECTION = 'BAD'").Length;

                dtLotSort = dsData.Tables["LOT"].DefaultView.ToTable(true, "LOT_ID").Select("1 = 1", "LOT_ID").CopyToDataTable<DataRow>();

                dtHeader = dsData.Tables["GRP"].DefaultView.ToTable(false, m_Item.ToArray()).Copy();
                dtLotList = dsData.Tables["LOT"].Copy();

                FarPoint.Win.Spread.CellType.NumberCellType oNumIntCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.NumberCellType oNumDoubleCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.TextCellType oTextCell = new FarPoint.Win.Spread.CellType.TextCellType();
                oNumIntCell.DecimalPlaces = 0;
                oNumDoubleCell.DecimalPlaces = 2;

                fpsCommon_Sheet1.DataSource = null;
                fpsCommon_Sheet1.Rows.Clear();
                fpsCommon_Sheet1.Columns.Clear();
                fpsCommon_Sheet1.ColumnHeader.Columns.Clear();
                fpsCommon_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;

                //Step 01 :  Group Column 생성
                fpsCommon_Sheet1.DataSource = dtHeader;

                //Step 02 :  Good / Bad Column 생성
                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "BAD";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumDoubleCell;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 50;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].AllowAutoFilter = true;
                iColBad = fpsCommon_Sheet1.ColumnCount - 1;

                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "GOOD";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumDoubleCell;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 50;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].AllowAutoFilter = true;
                iColGood = fpsCommon_Sheet1.ColumnCount - 1;

                Utility.FPSpreadUtil.SpreadSortingAll(fpsCommon_Sheet1);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsCommon_Sheet1);

                //고정 Column
                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "_";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 2;
                Utility.FPSpreadUtil.SetColBolder(fpsCommon_Sheet1, fpsCommon_Sheet1.ColumnCount - 1);
                //현재까지의 Column 은 고정 
                fpsCommon_Sheet1.FrozenColumnCount = fpsCommon_Sheet1.ColumnCount - 1;

                iLotNoStartCol = fpsCommon_Sheet1.ColumnCount;
                //Step 03 :  Lot No 생성 ex) L1, L2, L3
                for (int iLotCnt = 0; iLotCnt < iMaxLotCnt; iLotCnt++)
                {
                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("L{0}", iLotCnt + 1);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oTextCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 30;
                }

                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "_";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 2;
                Utility.FPSpreadUtil.SetColBolder(fpsCommon_Sheet1, fpsCommon_Sheet1.ColumnCount - 1);

                iLotDetailStartCol = fpsCommon_Sheet1.ColumnCount;
                //Step 04 :  LotID 순으로 Lot / Time / Recipe 를 출력.
                for (int iLotGrp = 0; iLotGrp < dtLotSort.Rows.Count; iLotGrp++)
                {
                    string strLotID = dtLotSort.Rows[iLotGrp]["LOT_ID"].ToString();
                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("L_{0}", strLotID);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oTextCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 60;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Tag = strLotID;

                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("T_{0}", strLotID);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oTextCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 100;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Tag = strLotID;

                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("R_{0}", strLotID);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oTextCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 80;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Tag = strLotID;
                }

                //Raw Data Set
                for (int ir = 0; ir < dtHeader.Rows.Count; ir++)
                {
                    MainForm.SetStatusMessage(string.Format("Data 화면에 출력 중입니다. ({0}/{1})", ir, dtHeader.Rows.Count));
                    string strFilter = string.Empty;
                    DataTable dtFilter = null;
                    int iFilterGood = -1;
                    int iFilterFBad = -1;

                    for (int ic = 0; ic < dtHeader.Columns.Count; ic++)
                    {
                        if (ic == 0)
                        {
                            strFilter += string.Format(" {0} = '{1}' ", dtHeader.Columns[ic].ColumnName, dtHeader.Rows[ir][dtHeader.Columns[ic].ColumnName].ToString());
                        }
                        else
                        {
                            strFilter += string.Format(" AND {0} = '{1}' ", dtHeader.Columns[ic].ColumnName, dtHeader.Rows[ir][dtHeader.Columns[ic].ColumnName].ToString());
                        }
                    }

                    //시간 순서 대로 넣는다.
                    dtFilter = dtLotList.Select(strFilter, "TRAN_TIME").CopyToDataTable<DataRow>();

                    //Good / Bad Rate
                    iFilterGood = dtFilter.DefaultView.ToTable(true, "SECTION", "LOT_ID").Select("SECTION = 'GOOD'").Length;
                    iFilterFBad = dtFilter.DefaultView.ToTable(true, "SECTION", "LOT_ID").Select("SECTION = 'BAD'").Length;

                    fpsCommon_Sheet1.Cells[ir, iColGood].Value = ((double)iFilterGood / (double)iTotalGood * 100);
                    fpsCommon_Sheet1.Cells[ir, iColBad].Value = ((double)iFilterFBad / (double)iTotalBad * 100);


                    for (int iLot = 0; iLot < dtFilter.Rows.Count; iLot++)
                    {
                        fpsCommon_Sheet1.Cells[ir, iLotNoStartCol + iLot].Value = string.Format("{0} : {1}", dtFilter.Rows[iLot]["SECTION"].ToString()[0], dtFilter.Rows[iLot]["LOT_ID"].ToString());

                        //Lot No 별로 순서대로 입력
                        if (dtFilter.Rows[iLot]["SECTION"].ToString() == "GOOD")
                        {
                            fpsCommon_Sheet1.Cells[ir, iLotNoStartCol + iLot].BackColor = cGood;
                        }
                        else
                        {
                            fpsCommon_Sheet1.Cells[ir, iLotNoStartCol + iLot].BackColor = cBad;
                        }


                        //Detail 정보 관련 입력
                        int iComplete = 0;
                        for (int iDetail = iLotDetailStartCol; iDetail < fpsCommon_Sheet1.ColumnCount; iDetail++)
                        {
                            if (fpsCommon_Sheet1.Columns[iDetail].Tag.ToString() == dtFilter.Rows[iLot]["LOT_ID"].ToString())
                            {
                                if (fpsCommon_Sheet1.Columns[iDetail].Label.StartsWith("L") == true)
                                {
                                    fpsCommon_Sheet1.Cells[ir, iDetail].Value = dtFilter.Rows[iLot]["LOT_ID"].ToString();
                                    iComplete++;
                                }

                                if (fpsCommon_Sheet1.Columns[iDetail].Label.StartsWith("T") == true)
                                {
                                    fpsCommon_Sheet1.Cells[ir, iDetail].Value = dtFilter.Rows[iLot]["TRAN_TIME"].ToString();
                                    iComplete++;
                                }

                                if (fpsCommon_Sheet1.Columns[iDetail].Label.StartsWith("R") == true)
                                {
                                    fpsCommon_Sheet1.Cells[ir, iDetail].Value = dtFilter.Rows[iLot]["RECIPE"].ToString();
                                    iComplete++;
                                }

                                if (dtFilter.Rows[iLot]["SECTION"].ToString() == "GOOD")
                                {
                                    fpsCommon_Sheet1.Cells[ir, iDetail].BackColor = cGood;
                                }
                                else
                                {
                                    fpsCommon_Sheet1.Cells[ir, iDetail].BackColor = cBad;
                                }
                            }

                            //입력 완료되면 For 문에서 나온다. 속도..
                            if (iComplete >= 3)
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dtLotSort != null)
                    dtLotSort.Dispose();
                dtLotSort = null;

                if (dsData != null)
                    dsData.Dispose();
                dsData = null;

                MainForm.SetStatusMessage(null);
            }
        }

        private void fnDataViewWaferBase()
        {
            DataTable dtLotSort = null;
            DataSet dsData = null;
            List<string> strItem = new List<string>();
            List<string> strGoodLot = new List<string>();
            List<string> strBadLot = new List<string>();

            DACrux.Common.RO.ComConfiguration oCom = new Common.RO.ComConfiguration();

            int iMaxLotCnt = -1;
            int iTotalGood = -1;
            int iTotalBad = -1;

            int iColGood = -1;
            int iColBad = -1;

            try
            {
                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (lsSelectionWafer.Items.Count <= 0)
                {
                    MessageBox.Show("Selection Item 을 선택 하세요.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsSelectionWafer.Focus();
                    return;
                }

                if (lsGoodLot.Items.Count <= 0)
                {
                    MessageBox.Show("Good Lot 에 Lot List 가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsGoodLot.Focus();
                    return;
                }

                if (lsBadLot.Items.Count <= 0)
                {
                    MessageBox.Show("Bad Lot 에 Lot List 가 없습니다.", "Editer Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsBadLot.Focus();
                    return;
                }

                foreach (ListViewItem Item in lsSelectionWafer.Items)
                {
                    strItem.Add(Item.SubItems[0].Text.ToString());
                }

                foreach (object row in lsGoodLot.Items)
                {
                    strGoodLot.Add(row.ToString());
                }

                foreach (object row in lsBadLot.Items)
                {
                    strBadLot.Add(row.ToString());
                }

                dsData = oCom.GetCommonalityWafer(strGoodLot.ToArray(), strBadLot.ToArray(), strItem.ToArray(), chkInspectionDM.Checked);
                if (dsData == null || dsData.Tables.Count <= 0)
                    throw new Exception("Not Found Data");


                MainForm.SetStatusMessage("Data 화면에 출력 중입니다.");
                m_Item = new List<string>();
                foreach (ListViewItem Item in lsSelectionWafer.Items)
                {
                    m_Item.Add(Item.SubItems[0].Text.ToString());
                }

                iMaxLotCnt = int.Parse(dsData.Tables["GRP"].Compute("MAX(TOTAL)", string.Empty).ToString());
                iTotalGood = dsData.Tables["WAFER"].DefaultView.ToTable(true, "SECTION", "LOT_ID", "WAFER_ID").Select("SECTION = 'GOOD'").Length;
                iTotalBad = dsData.Tables["WAFER"].DefaultView.ToTable(true, "SECTION", "LOT_ID", "WAFER_ID").Select("SECTION = 'BAD'").Length;

                dtLotSort = dsData.Tables["WAFER"].DefaultView.ToTable(true, "LOT_ID").Select("1 = 1", "LOT_ID").CopyToDataTable<DataRow>();

                dtHeader = dsData.Tables["GRP"].DefaultView.ToTable(false, m_Item.ToArray()).Copy();
                dtLotList = dsData.Tables["WAFER"].Copy();

                FarPoint.Win.Spread.CellType.NumberCellType oNumIntCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.NumberCellType oNumDoubleCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                FarPoint.Win.Spread.CellType.TextCellType oTextCell = new FarPoint.Win.Spread.CellType.TextCellType();
                oNumIntCell.DecimalPlaces = 0;
                oNumDoubleCell.DecimalPlaces = 2;

                fpsCommon_Sheet1.DataSource = null;
                fpsCommon_Sheet1.Rows.Clear();
                fpsCommon_Sheet1.Columns.Clear();
                fpsCommon_Sheet1.ColumnHeader.Columns.Clear();
                fpsCommon_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;

                //Step 01 :  Group Column 생성
                fpsCommon_Sheet1.DataSource = dtHeader;

                //Step 02 :  Good / Bad Column 생성
                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "BAD";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumDoubleCell;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 50;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Visible = true;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].AllowAutoFilter = true;
                iColBad = fpsCommon_Sheet1.ColumnCount - 1;

                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "GOOD";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumDoubleCell;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 50;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Visible = true;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].AllowAutoFilter = true;
                iColGood = fpsCommon_Sheet1.ColumnCount - 1;

                Utility.FPSpreadUtil.SpreadSortingAll(fpsCommon_Sheet1);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsCommon_Sheet1);

                //고정 Column
                fpsCommon_Sheet1.ColumnCount++;
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = "_";
                fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 2;
                Utility.FPSpreadUtil.SetColBolder(fpsCommon_Sheet1, fpsCommon_Sheet1.ColumnCount - 1);
                //현재까지의 Column 은 고정 
                fpsCommon_Sheet1.FrozenColumnCount = fpsCommon_Sheet1.ColumnCount - 1;

                iWaferNoStartCol = fpsCommon_Sheet1.ColumnCount;
                //Step 03 :  Wafer No 생성 

                List<String> goodWafers = new List<String>();
                List<String> badWafers = new List<String>();

                int idx = 0;
                for (int i = 0; i < dtLotList.Rows.Count; i++)
                {
                    DataRow row = dtLotList.Rows[i];
                    string waferId = row["WAFER_ID"].ToString();
                    if (String.Equals(row["SECTION"].ToString(), "BAD"))
                    {
                        idx = badWafers.BinarySearch(waferId);
                        if (idx < 0)
                            badWafers.Insert(~idx, waferId);
                    }

                    if (String.Equals(row["SECTION"].ToString(), "GOOD"))
                    {
                        idx = goodWafers.BinarySearch(waferId);
                        if (idx < 0)
                            goodWafers.Insert(~idx, waferId);
                    }
                }

                for (int iWafer = 0; iWafer < goodWafers.Count; iWafer++)
                {
                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("G_{0}", goodWafers[iWafer]);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumIntCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 100;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Tag = string.Format("GOOD_{0}", goodWafers[iWafer]);
                }

                for (int iWafer = 0; iWafer < badWafers.Count; iWafer++)
                {
                    fpsCommon_Sheet1.ColumnCount++;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Label = string.Format("B_{0}", badWafers[iWafer]);
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].CellType = oNumIntCell;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Width = 100;
                    fpsCommon_Sheet1.Columns[fpsCommon_Sheet1.ColumnCount - 1].Tag = string.Format("BAD_{0}", badWafers[iWafer]);
                }


                //Raw Data Set
                for (int ir = 0; ir < dtHeader.Rows.Count; ir++)
                {
                    MainForm.SetStatusMessage(string.Format("Data 화면에 출력 중입니다. ({0}/{1})", ir, dtHeader.Rows.Count));
                    string strFilter = string.Empty;
                    DataTable dtFilter = null;
                    int iFilterGood = -1;
                    int iFilterFBad = -1;
                    int iValue = -1;

                    for (int ic = 0; ic < dtHeader.Columns.Count; ic++)
                    {
                        if (ic == 0)
                        {
                            strFilter += string.Format(" {0} = '{1}' ", dtHeader.Columns[ic].ColumnName, dtHeader.Rows[ir][dtHeader.Columns[ic].ColumnName].ToString());
                        }
                        else
                        {
                            if (dtHeader.Rows[ir][dtHeader.Columns[ic].ColumnName] == DBNull.Value)
                            {
                                strFilter += string.Format(" AND {0} IS NULL ", dtHeader.Columns[ic].ColumnName);
                            }
                            else
                            {
                                strFilter += string.Format(" AND {0} = '{1}' ", dtHeader.Columns[ic].ColumnName, dtHeader.Rows[ir][dtHeader.Columns[ic].ColumnName].ToString());
                            }
                        }
                    }

                    dtFilter = dtLotList.Select(strFilter).CopyToDataTable<DataRow>();

                    //Good / Bad Rate
                    iFilterGood = dtFilter.DefaultView.ToTable(true, "SECTION", "LOT_ID", "WAFER_ID").Select("SECTION = 'GOOD'").Length;
                    iFilterFBad = dtFilter.DefaultView.ToTable(true, "SECTION", "LOT_ID", "WAFER_ID").Select("SECTION = 'BAD'").Length;

                    fpsCommon_Sheet1.Cells[ir, iColGood].Value = ((double)iFilterGood / (double)iTotalGood * 100);
                    fpsCommon_Sheet1.Cells[ir, iColBad].Value = ((double)iFilterFBad / (double)iTotalBad * 100);

                    for (int iWaferCol = iWaferNoStartCol; iWaferCol < fpsCommon_Sheet1.ColumnCount; iWaferCol++)
                    {
                        string strTagName = fpsCommon_Sheet1.Columns[iWaferCol].Tag.ToString();

                        if (string.IsNullOrEmpty(strTagName) == true || strTagName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Length != 2)
                            continue;

                        string[] strTag = strTagName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

                        DataRow[] rows = dtFilter.Select(string.Format("SECTION = '{0}' AND WAFER_ID = '{1}'", strTag[0], strTag[1]));
                        if (rows == null || rows.Length <= 0)
                        {
                            fpsCommon_Sheet1.Cells[ir, iWaferCol].Value = DBNull.Value;
                        }
                        else
                        {
                            fpsCommon_Sheet1.Cells[ir, iWaferCol].Value = rows[0]["TRAN_TIME"].ToString();
                            fpsCommon_Sheet1.Cells[ir, iWaferCol].BackColor = String.Equals(strTag[0], "BAD") ? cBad : cGood;
                        }
                    }
                }

                fpsWaferDetail.ActiveSheet.DataSource = null;
                fpsWaferDetail.ActiveSheet.Rows.Clear();
                fpsWaferDetail.ActiveSheet.Columns.Clear();
                fpsWaferDetail.ActiveSheet.ColumnHeader.Columns.Clear();
                fpsWaferDetail.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpsWaferDetail.ActiveSheet.DataSource = dtLotList.Copy();

                Utility.FPSpreadUtil.SetAutoColumnFilter(fpsWaferDetail.ActiveSheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsWaferDetail.ActiveSheet);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dtLotSort != null)
                    dtLotSort.Dispose();
                dtLotSort = null;

                if (dsData != null)
                    dsData.Dispose();
                dsData = null;

                MainForm.SetStatusMessage(null);
            }
        }

        #endregion [ Method ]


        #region [ Event Handler ]

        private void frmCommonalityStudy_Load(object sender, EventArgs e)
        {
            frmLotList = new frmCommonalityLotList();
            frmLotList.OnSelected += new frmCommonalityLotList.Selected(oLotList_OnSelected);
            frmLotList.TopMost = true;
            frmLotList.Owner = this;
            frmLotList.StartPosition = FormStartPosition.CenterParent;

            ListViewItem oItem = null;

            lsAvailableLot.Clear();
            lsSelectionLot.Clear();

            lsAvailableWafer.Clear();
            lsSelectionWafer.Clear();

            for (int r = 0; r < m_strLotFieldAvilable.Length; r++)
            {
                oItem = new ListViewItem(m_strLotFieldAvilable[r]);
                lsAvailableLot.Items.Add(oItem);
            }

            for (int r = 0; r < m_strLotFieldSelect.Length; r++)
            {
                oItem = new ListViewItem(m_strLotFieldSelect[r]);
                lsSelectionLot.Items.Add(oItem);
            }

            for (int r = 0; r < m_strWaferFieldAvilable.Length; r++)
            {
                oItem = new ListViewItem(m_strWaferFieldAvilable[r]);
                lsAvailableWafer.Items.Add(oItem);
            }

            for (int r = 0; r < m_strWaferFieldSelect.Length; r++)
            {
                oItem = new ListViewItem(m_strWaferFieldSelect[r]);
                lsSelectionWafer.Items.Add(oItem);
            }
        }

        private void btnSearchLotList(object sender, EventArgs e)
        {
            frmLotList.ShowDialog();
        }

        private void lsTotal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.V && e.Control == true)
                {
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    foreach (string ln in lines)
                    {
                        lsTotal.Items.Add(ln.Trim().Replace(",", "").Replace(":", "").Replace("/", ""));
                    }
                }

                if (e.KeyCode == Keys.C && e.Control == true && lsTotal.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (object row in lsTotal.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();
                    }
                    sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }

                if (e.KeyCode == Keys.X && e.Control == true)
                {
                    StringBuilder sb = new StringBuilder();
                    // we use this collection to keep all the Selected Items
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsTotal.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();

                        // Keep on adding selected item into a new List of Object
                        selectedItemList.Add(row);
                    }
                    sb.Remove(sb.Length - 1, 1);    // Just to avoid copying last empty row                
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                    // Removing selected items from the ListBox
                    foreach (object ln in selectedItemList)
                    {
                        lsTotal.Items.Remove(ln);
                    }
                }

                if (e.KeyCode == Keys.Delete)
                {
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsTotal.SelectedItems)
                    {
                        selectedItemList.Add(row);
                    }

                    foreach (object ln in selectedItemList)
                    {
                        lsTotal.Items.Remove(ln);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lsGoodLot_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.V && e.Control == true)
                {
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    foreach (string ln in lines)
                    {
                        lsGoodLot.Items.Add(ln.Trim().Replace(",", "").Replace(":", "").Replace("/", ""));
                    }
                }

                if (e.KeyCode == Keys.C && e.Control == true && lsGoodLot.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (object row in lsGoodLot.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();
                    }
                    sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }

                if (e.KeyCode == Keys.X && e.Control == true)
                {
                    StringBuilder sb = new StringBuilder();
                    // we use this collection to keep all the Selected Items
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsGoodLot.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();

                        // Keep on adding selected item into a new List of Object
                        selectedItemList.Add(row);
                    }
                    sb.Remove(sb.Length - 1, 1);    // Just to avoid copying last empty row                
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                    // Removing selected items from the ListBox
                    foreach (object ln in selectedItemList)
                    {
                        lsGoodLot.Items.Remove(ln);
                    }
                }

                if (e.KeyCode == Keys.Delete)
                {
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsGoodLot.SelectedItems)
                    {
                        selectedItemList.Add(row);
                    }

                    foreach (object ln in selectedItemList)
                    {
                        lsGoodLot.Items.Remove(ln);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lsBadLot_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.V && e.Control == true)
                {
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    foreach (string ln in lines)
                    {
                        lsBadLot.Items.Add(ln.Trim().Replace(",", "").Replace(":", "").Replace("/", ""));
                    }
                }

                if (e.KeyCode == Keys.C && e.Control == true && lsBadLot.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (object row in lsBadLot.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();
                    }
                    sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }

                if (e.KeyCode == Keys.X && e.Control == true)
                {
                    StringBuilder sb = new StringBuilder();
                    // we use this collection to keep all the Selected Items
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsBadLot.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();

                        // Keep on adding selected item into a new List of Object
                        selectedItemList.Add(row);
                    }
                    sb.Remove(sb.Length - 1, 1);    // Just to avoid copying last empty row                
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                    // Removing selected items from the ListBox
                    foreach (object ln in selectedItemList)
                    {
                        lsBadLot.Items.Remove(ln);
                    }
                }

                if (e.KeyCode == Keys.Delete)
                {
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in lsBadLot.SelectedItems)
                    {
                        selectedItemList.Add(row);
                    }

                    foreach (object ln in selectedItemList)
                    {
                        lsBadLot.Items.Remove(ln);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void allSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBox oListBox = null;

            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        oListBox = owner.SourceControl as ListBox;
                    }
                }

                if (oListBox != null)
                {
                    for (int i = 0; i < oListBox.Items.Count; i++)
                    {
                        oListBox.SetSelected(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBox oListBox = null;
            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        oListBox = owner.SourceControl as ListBox;
                    }
                }

                if (oListBox != null)
                {
                    StringBuilder sb = new StringBuilder();
                    // we use this collection to keep all the Selected Items
                    List<object> selectedItemList = new List<object>();
                    foreach (object row in oListBox.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();

                        // Keep on adding selected item into a new List of Object
                        selectedItemList.Add(row);
                    }
                    sb.Remove(sb.Length - 1, 1);    // Just to avoid copying last empty row                
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                    // Removing selected items from the ListBox
                    foreach (object ln in selectedItemList)
                    {
                        oListBox.Items.Remove(ln);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBox oListBox = null;
            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        oListBox = owner.SourceControl as ListBox;
                    }
                }

                if (oListBox != null)
                {
                    // Getting Text from Clip board
                    string s = Clipboard.GetText();
                    //Parsing criteria: New Line 
                    string[] lines = s.Split('\n');
                    foreach (string ln in lines)
                    {
                        oListBox.Items.Add(ln.Trim().Replace(",", "").Replace(":", "").Replace("/", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBox oListBox = null;
            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        oListBox = owner.SourceControl as ListBox;
                    }
                }

                if (oListBox != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (object row in oListBox.SelectedItems)
                    {
                        sb.Append(row.ToString());
                        sb.AppendLine();
                    }
                    sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListBox oListBox = null;
            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        oListBox = owner.SourceControl as ListBox;
                    }
                }

                if (oListBox != null)
                {
                    oListBox.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnMoveGood_Click(object sender, EventArgs e)
        {
            List<object> selectedItemList = new List<object>();
            foreach (object row in lsTotal.SelectedItems)
            {
                selectedItemList.Add(row);
                lsGoodLot.Items.Add(row.ToString());
            }

            foreach (object ln in selectedItemList)
            {
                lsTotal.Items.Remove(ln);
            }
        }

        private void BtnMoveBad_Click(object sender, EventArgs e)
        {
            List<object> selectedItemList = new List<object>();
            foreach (object row in lsTotal.SelectedItems)
            {
                selectedItemList.Add(row);
                lsBadLot.Items.Add(row.ToString());
            }

            foreach (object ln in selectedItemList)
            {
                lsTotal.Items.Remove(ln);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsAvailableLot.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsAvailableLot.SelectedItems.Count];

                    for (int i = 0; i < lsAvailableLot.SelectedItems.Count; i++)
                        oSelectItem[i] = lsAvailableLot.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsAvailableLot.SelectedItems)
                    {
                        lsAvailableLot.Items.Remove(eachItem);
                    }

                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        if (lsSelectionLot.FindItemWithText(oSelectItem[i].Text) == null)
                        {
                            oItem = new ListViewItem(oSelectItem[i].Text);
                            lsSelectionLot.Items.Add(oItem);
                        }
                    }

                    lsAvailableLot.Update();
                    lsSelectionLot.Update();
                }
            }
            finally
            {

            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsSelectionLot.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsSelectionLot.SelectedItems.Count];

                    for (int i = 0; i < lsSelectionLot.SelectedItems.Count; i++)
                        oSelectItem[i] = lsSelectionLot.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsSelectionLot.SelectedItems)
                    {
                        lsSelectionLot.Items.Remove(eachItem);
                    }

                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        if (lsAvailableLot.FindItemWithText(oSelectItem[i].Text) == null)
                        {
                            oItem = new ListViewItem(oSelectItem[i].Text);
                            lsAvailableLot.Items.Add(oItem);
                        }
                    }

                    lsSelectionLot.Update();
                    lsAvailableLot.Update();
                }
            }
            finally
            {

            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (lsSelectionLot.SelectedItems == null || lsSelectionLot.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelectionLot.SelectedItems[0];
            int iCurrIdx = lsSelectionLot.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lsSelectionLot.Items.RemoveAt(iCurrIdx);
                lsSelectionLot.Items.Insert(iCurrIdx - 1, lvSelItem);
            }
            finally
            {
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (lsSelectionLot.SelectedItems == null || lsSelectionLot.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelectionLot.SelectedItems[0];
            int iCurrIdx = lsSelectionLot.SelectedIndices[0];

            if (iCurrIdx == lsSelectionLot.Items.Count - 1) return;
            try
            {
                lsSelectionLot.Items.RemoveAt(iCurrIdx);
                lsSelectionLot.Items.Insert(iCurrIdx + 1, lvSelItem);
            }
            finally
            {
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (TabItem.SelectedIndex == 0)
            {
                fnDataViewLotBase();
            }
            else
            {
                fnDataViewWaferBase();
            }

        }

        private void TabItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabItem.SelectedIndex == 0)
            {
                mode = STATUS.LOT;
                chkInspectionDM.Visible = true;
                pnDetail.Visible = false;
                spDetail.Visible = false;
                label1.Text = "Total Lot List";
                label2.Text = "Good Lot List";
                label3.Text = "Bad Lot List";
            }
            else
            {
                mode = STATUS.WAFER;
                chkInspectionDM.Visible = false;
                pnDetail.Visible = true;
                spDetail.Visible = true;
                label1.Text = "Total Lot List";
                label2.Text = "Good Wafer List";
                label3.Text = "Bad Wafer List";
            }

            fpsWaferDetail.ActiveSheet.DataSource = null;
            fpsWaferDetail.ActiveSheet.Rows.Clear();
            fpsWaferDetail.ActiveSheet.Columns.Clear();
            fpsWaferDetail.ActiveSheet.ColumnHeader.Columns.Clear();

            fpsCommon_Sheet1.DataSource = null;
            fpsCommon_Sheet1.Rows.Clear();
            fpsCommon_Sheet1.Columns.Clear();
            fpsCommon_Sheet1.ColumnHeader.Columns.Clear();
        }


        private void fpsCommon_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //try
            //{
            //    MainForm.SetStatusMessage("상세 정보를 출력 중입니다.");

            //    string strTagName = string.Empty;
            //    string strFilter = string.Empty;
            //    DataTable dtFilter = null;

            //    fpsWaferDetail.ActiveSheet.DataSource = null;
            //    fpsWaferDetail.ActiveSheet.Rows.Clear();
            //    fpsWaferDetail.ActiveSheet.Columns.Clear();
            //    fpsWaferDetail.ActiveSheet.ColumnHeader.Columns.Clear();
            //    fpsWaferDetail.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;

            //    if (TabItem.SelectedIndex != 1 || fpsCommon_Sheet1.Columns[e.Column].Tag == null)
            //        return;

            //    strTagName = fpsCommon_Sheet1.Columns[e.Column].Tag.ToString();

            //    if (string.IsNullOrEmpty(strTagName) == true || strTagName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Length != 2)
            //        return;

            //    for (int ic = 0; ic < dtHeader.Columns.Count; ic++)
            //    {
            //        if (ic == 0)
            //        {
            //            strFilter += string.Format(" {0} = '{1}' ", fpsCommon_Sheet1.Columns[ic].Label, fpsCommon_Sheet1.Cells[e.Row, ic].Value.ToString());
            //        }
            //        else
            //        {
            //            strFilter += string.Format(" AND {0} = '{1}' ", fpsCommon_Sheet1.Columns[ic].Label, fpsCommon_Sheet1.Cells[e.Row, ic].Value.ToString());
            //        }
            //    }

            //    string[] TagArr = strTagName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            //    strFilter += string.Format("AND SECTION = '{0}' AND SLOT_NO = {1}", TagArr[0], TagArr[1]);

            //    if (dtLotList.Select(strFilter).Length <= 0)
            //        return;

            //    dtFilter = dtLotList.Select(strFilter).CopyToDataTable<DataRow>();
            //    if (dtFilter == null || dtFilter.Rows.Count <= 0)
            //        return;

            //    pnDetail.Visible = true;

            //    //Step 01 :  Group Column 생성
            //    fpsWaferDetail.ActiveSheet.DataSource = dtFilter;
            //    Application.DoEvents();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    MainForm.SetStatusMessage(null);
            //}
        }

        private void BtnDetailView_Click(object sender, EventArgs e)
        {

        }


        private void txtGoodLot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string id = txtGoodLot.Text.Trim();
                if (mode == STATUS.LOT)
                {
                    if (string.IsNullOrEmpty(id))
                        return;

                    if (id.Contains('-'))
                    {
                        MessageBox.Show(String.Format("LOT ID 형식이 아닙니다. 확인 후 다시 입력해주세요."), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    lsGoodLot.Items.Add(id);
                }
                else
                {
                    if (string.IsNullOrEmpty(id))
                        return;

                    if (!id.Contains('-'))
                    {
                        MessageBox.Show(String.Format("Wafer ID 형식이 아닙니다. 확인 후 다시 입력해주세요."), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    lsGoodLot.Items.Add(id);
                }
            }
        }

        private void txtBadLot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string id = txtBadLot.Text.Trim();
                if (mode == STATUS.LOT)
                {
                    if (string.IsNullOrEmpty(id))
                        return;

                    if (id.Contains('-'))
                    {
                        MessageBox.Show(String.Format("LOT ID 형식이 아닙니다. 확인 후 다시 입력해주세요."), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    lsBadLot.Items.Add(id);
                }
                else
                {
                    if (string.IsNullOrEmpty(id))
                        return;

                    if (!id.Contains('-'))
                    {
                        MessageBox.Show(String.Format("Wafer ID 형식이 아닙니다. 확인 후 다시 입력해주세요."), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    lsBadLot.Items.Add(id);
                }
            }
        }

        private void BtnAddWafer_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsAvailableWafer.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsAvailableWafer.SelectedItems.Count];

                    for (int i = 0; i < lsAvailableWafer.SelectedItems.Count; i++)
                        oSelectItem[i] = lsAvailableWafer.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsAvailableWafer.SelectedItems)
                    {
                        lsAvailableWafer.Items.Remove(eachItem);
                    }

                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        if (lsSelectionWafer.FindItemWithText(oSelectItem[i].Text) == null)
                        {
                            oItem = new ListViewItem(oSelectItem[i].Text);
                            lsSelectionWafer.Items.Add(oItem);
                        }
                    }

                    lsAvailableWafer.Update();
                    lsSelectionWafer.Update();
                }
            }
            finally
            {

            }
        }

        private void BtnDelWafer_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsSelectionWafer.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsSelectionWafer.SelectedItems.Count];

                    for (int i = 0; i < lsSelectionWafer.SelectedItems.Count; i++)
                        oSelectItem[i] = lsSelectionWafer.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsSelectionWafer.SelectedItems)
                    {
                        lsSelectionWafer.Items.Remove(eachItem);
                    }

                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        if (lsAvailableWafer.FindItemWithText(oSelectItem[i].Text) == null)
                        {
                            oItem = new ListViewItem(oSelectItem[i].Text);
                            lsAvailableWafer.Items.Add(oItem);
                        }
                    }

                    lsSelectionWafer.Update();
                    lsAvailableWafer.Update();
                }
            }
            finally
            {

            }
        }

        private void BtnUpWafer_Click(object sender, EventArgs e)
        {
            if (lsSelectionWafer.SelectedItems == null || lsSelectionWafer.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelectionWafer.SelectedItems[0];
            int iCurrIdx = lsSelectionWafer.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lsSelectionWafer.Items.RemoveAt(iCurrIdx);
                lsSelectionWafer.Items.Insert(iCurrIdx - 1, lvSelItem);
            }
            finally
            {
            }
        }

        private void BtnDownWafer_Click(object sender, EventArgs e)
        {
            if (lsSelectionWafer.SelectedItems == null || lsSelectionWafer.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelectionWafer.SelectedItems[0];
            int iCurrIdx = lsSelectionWafer.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lsSelectionWafer.Items.RemoveAt(iCurrIdx);
                lsSelectionWafer.Items.Insert(iCurrIdx - 1, lvSelItem);
            }
            finally
            {
            }
        }


        #endregion [ Event Handler ]


    }
}
