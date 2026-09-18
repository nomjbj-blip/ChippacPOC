using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.Framework.Controls;
using DACrux.SEMDMS.RO;
using FarPoint.Win.Spread.CellType;
using DACrux.Utility;
using FarPoint.Win.Spread;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDataSheet : DACruxUXBasic01, IExportExcel
    {
        private static readonly string DATE_FORMAT = "yyyyMMdd";
        enum Sheet1_Cols { DEVICE = 0, STEP_ID, INSPECTION_TIME, INSPECTOR, WAFER_ID, LOT_ID, TST_PGM, WF_MPY, TOTALDEFECT, SCAN_AREA, DEFECT_DENSITY, TOTAL_CAT, CAT0 }
        enum Sheet2_Cols { DEVICE = 0, STEP_ID, INSPECTION_TIME, INSPECTOR, WAFER_ID, LOT_ID, TST_PGM, WF_MPY, TOTALDEFECT, SCAN_AREA, DEFECT_DENSITY, TOTAL_CAT, CAT0 }

        public frmDataSheet()
        {
            InitializeComponent();
        }

        #region [ Event Handler ]


        private void frmDataSheet_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            fromDate.Value = DateTime.Now.AddDays(-30);
            toDate.Value = DateTime.Now;

            GetDataProduct();
            FillDefectCodeData();
            FillXItems();
        }

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            GetData();
        }

        private void cmbXitem_SelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        private void dlbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbProduct.SelectedIndex < 0)
                return;

            dlbStep.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionStep(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products
                );
            SetBinding(dt, dlbStep);
        }

        private void dlbStep_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbStep.SelectedIndex < 0)
                return;

            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;
            string[] stepids = GetSelectedValues(dlbStep);
            if (stepids == null || stepids.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionLot(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products,
                stepids
                );
            SetBinding(dt, dlbLot);
        }

        private void dlbLot_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbLot.SelectedIndex < 0)
                return;

            dlbWafer.ClearDataSource();
            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;
            string[] stepids = GetSelectedValues(dlbStep);
            if (stepids == null || stepids.Length <= 0)
                return;

            string[] lots = GetSelectedValues(dlbLot);
            if (lots == null || lots.Length <= 0)
                return;

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetConditionWafer(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products,
                stepids,
                lots,
                chkReviewOnly.Checked
                );
            SetBinding(dt, dlbWafer, 1, 0);
        }

        private void dlbWafer_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            btnView.PerformClick();
        }

        private void nudDecimalPlaces_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            dpucDataSheetControl1.DecimalPlaces = (int)nudDecimalPlaces.Value;
            dpucDataSheetControl1.SheetRefrash();
        }

        private void chkReview_CheckedChanged(object sender, EventArgs e)
        {
            dlbLot_OnSelectedIndexChanged(dlbLot, EventArgs.Empty);
        }

        #endregion [ Event Handler ]


        #region [ Method ]

        public void ExportExcel()
        {
            ExcelExportArgs e = new ExcelExportArgs();
            for (int idx = 0; idx < dpucDataSheetControl1.Tabs.Count; idx++)
            {
                TabPage pages = dpucDataSheetControl1.Tabs[idx] as TabPage;
                if (pages == null)
                    continue;

                FpSpread spread = pages.Controls[0] as FpSpread;
                if (spread == null)
                    continue;

                ExcelSheet sheet = new ExcelSheet();
                sheet.Add(spread);

                e.SheetList.Add(sheet);
                e.SheetList[idx].SheetName = pages.Name;
            }
            ExcelExportManager.Export(e);
        }

        //--

        private void FillDefectCodeData(
            )
        {
            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetDefectTypeList();
            SetBinding(dt, dlbDefectClass, 1, 0);
        }

        private void FillXItems(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("DISPLAY", typeof(String)), new DataColumn("VALUE", typeof(String)) });

            DataRow row = dt.NewRow(); row["DISPLAY"] = "TOTALDEFECT"; row["VALUE"] = "T3.TOTALDEFECT"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "TOTAL RANDOM"; row["VALUE"] = "T3.TOTALRANDOM"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "NEW DEFECT"; row["VALUE"] = "T3.NEWDEFECT"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "NEW RANDOM"; row["VALUE"] = "T3.NEWRANDOM"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "TOTAL DEFECT DIE"; row["VALUE"] = "T3.TOTALDEFECTDIE"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "TOTAL RANDOM DIE"; row["VALUE"] = "T3.TOTALRANDOMDIE"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "NEW DEFECT DIE"; row["VALUE"] = "T3.NEWDEFECTDIE"; dt.Rows.Add(row);
            row = dt.NewRow(); row["DISPLAY"] = "NEW RANDOM DIE"; row["VALUE"] = "T3.NEWRANDOMDIE"; dt.Rows.Add(row);

            //row = dt.NewRow(); row["DISPLAY"] = "TOTALCLUSTER"; row["VALUE"] = "T3.TOTALCLUSTER"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL0_5UM"; row["VALUE"] = "T3.TOTAL0_5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL0_7UM"; row["VALUE"] = "T3.TOTAL0_7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL1UM"; row["VALUE"] = "T3.TOTAL1UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL2UM"; row["VALUE"] = "T3.TOTAL2UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL3UM"; row["VALUE"] = "T3.TOTAL3UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL4UM"; row["VALUE"] = "T3.TOTAL4UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL5UM"; row["VALUE"] = "T3.TOTAL5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL6UM"; row["VALUE"] = "T3.TOTAL6UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTAL7UM"; row["VALUE"] = "T3.TOTAL7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALOS"; row["VALUE"] = "T3.TOTALOS"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM0_5UM"; row["VALUE"] = "T3.TOTALRANDOM0_5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM0_7UM"; row["VALUE"] = "T3.TOTALRANDOM0_7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM1UM"; row["VALUE"] = "T3.TOTALRANDOM1UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM2UM"; row["VALUE"] = "T3.TOTALRANDOM2UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM3UM"; row["VALUE"] = "T3.TOTALRANDOM3UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM4UM"; row["VALUE"] = "T3.TOTALRANDOM4UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM5UM"; row["VALUE"] = "T3.TOTALRANDOM5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM6UM"; row["VALUE"] = "T3.TOTALRANDOM6UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOM7UM"; row["VALUE"] = "T3.TOTALRANDOM7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOMOS"; row["VALUE"] = "T3.TOTALRANDOMOS"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRETICLE"; row["VALUE"] = "T3.TOTALRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALNONRETICLE"; row["VALUE"] = "T3.TOTALNONRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALRANDOMRETICLE"; row["VALUE"] = "T3.TOTALRANDOMRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "TOTALCLUSTERDIE"; row["VALUE"] = "T3.TOTALCLUSTERDIE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWCLUSTER"; row["VALUE"] = "T3.NEWCLUSTER"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW0_5UM"; row["VALUE"] = "T3.NEW0_5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW0_7UM"; row["VALUE"] = "T3.NEW0_7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW1UM"; row["VALUE"] = "T3.NEW1UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW2UM"; row["VALUE"] = "T3.NEW2UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW3UM"; row["VALUE"] = "T3.NEW3UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW4UM"; row["VALUE"] = "T3.NEW4UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW5UM"; row["VALUE"] = "T3.NEW5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW6UM"; row["VALUE"] = "T3.NEW6UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEW7UM"; row["VALUE"] = "T3.NEW7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWOS"; row["VALUE"] = "T3.NEWOS"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM0_5UM"; row["VALUE"] = "T3.NEWRANDOM0_5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM0_7UM"; row["VALUE"] = "T3.NEWRANDOM0_7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM1UM"; row["VALUE"] = "T3.NEWRANDOM1UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM2UM"; row["VALUE"] = "T3.NEWRANDOM2UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM3UM"; row["VALUE"] = "T3.NEWRANDOM3UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM4UM"; row["VALUE"] = "T3.NEWRANDOM4UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM5UM"; row["VALUE"] = "T3.NEWRANDOM5UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM6UM"; row["VALUE"] = "T3.NEWRANDOM6UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOM7UM"; row["VALUE"] = "T3.NEWRANDOM7UM"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOMOS"; row["VALUE"] = "T3.NEWRANDOMOS"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRETICLE"; row["VALUE"] = "T3.NEWRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWNONRETICLE"; row["VALUE"] = "T3.NEWNONRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWRANDOMRETICLE"; row["VALUE"] = "T3.NEWRANDOMRETICLE"; dt.Rows.Add(row);
            //row = dt.NewRow(); row["DISPLAY"] = "NEWCLUSTERDIE"; row["VALUE"] = "T3.NEWCLUSTERDIE"; dt.Rows.Add(row);

            SetBinding(dt, dlbXItems, 0, 1);
        }

        private void GetData()
        {
            string[] products = GetSelectedValues(dlbProduct);
            if (products == null || products.Length <= 0)
                return;
            string[] stepids = GetSelectedValues(dlbStep);
            if (stepids == null || stepids.Length <= 0)
                return;
            string[] lots = GetSelectedValues(dlbLot);
            if (lots == null || lots.Length <= 0)
                return;

            string[] wafers = GetSelectedValues(dlbWafer);
            if (wafers == null || !(wafers.Length > 0 && wafers.Length < 1000))
                return;


            string[] xItems = GetSelectedValues(dlbXItems);
            string[] categorise = GetSelectedValues(dlbDefectClass);
            string ordered = string.Empty;

            if (categorise == null)
            {
                categorise = new string[dlbDefectClass.Items.Count - 1];
                // uncatego
                for (int idx = 1; idx < dlbDefectClass.Items.Count; idx++)
                {
                    DataRowView item = dlbDefectClass.Items[idx] as DataRowView;
                    categorise[idx - 1] = item.Row[0].ToString();
                }
            }

            if (rbResulTime.Checked)
                ordered = String.Format("INSPECTION_TIME {0}", rbAsc.Checked ? "ASC" : "DESC");
            else if (rbWaferID.Checked)
                ordered = String.Format("WAFER_ID {0}", rbAsc.Checked ? "ASC" : "DESC");
            else ordered = ordered = String.Format("DEFECT_DENSITY {0}", rbAsc.Checked ? "ASC" : "DESC");

            DMReport obj = new DMReport();
            DataSet ds = obj.GetDataSheet(
                GetDateToString(fromDate),
                GetDateToString(toDate, 1),
                products,
                stepids,
                lots,
                wafers,
                xItems,
                categorise
                );

            DataSet displayDs = new DataSet();
            DataTable dt = null;
            DataTable dtCopy = null;
            if (xItems == null || xItems.Length <= 0)
            {
                // xitem을 선택하지 않은 경우에 Defective Die / Defect Count에 대한 Data를 표시한다.
                for (int tableIdx = 0; tableIdx < ds.Tables.Count; tableIdx++)
                {
                    dt = ds.Tables[tableIdx];
                    dt.DefaultView.Sort = ordered;
                    dtCopy = dt.DefaultView.ToTable().Copy();
                    displayDs.Tables.Add(dtCopy);
                }
            }
            else
            {
                string xItem = string.Empty;
                for (int idx = 0; idx < xItems.Length; idx++)
                {
                    if (xItems[idx].Contains("DIE"))
                    {
                        /// defective die, Die에 대한 값
                        dt = ds.Tables["DEFECTIVE DIE SHEET"].Copy();
                        dt.DefaultView.Sort = ordered;
                        dt = dt.DefaultView.ToTable();
                    }
                    else
                    {
                        /// defect count에 대한 값
                        dt = ds.Tables["DEFECT COUNT SHEET"].Copy();
                        dt.DefaultView.Sort = ordered;
                        dt = dt.DefaultView.ToTable();
                    }

                    if (idx == 0)
                        displayDs.Tables.Add(dt);

                    // DataTable Copy
                    dtCopy = dt.Copy();
                    xItem = xItems[idx].Replace("T3.", "");
                    double itemValue = double.NaN;
                    double total = double.NaN;
                    double density = double.NaN;
                    double dValue = double.NaN;
                    double scanArea = double.NaN;
                    string columnName = string.Empty;

                    for (int rowidx = 0; rowidx < dtCopy.Rows.Count; rowidx++)
                    {
                        DataRow row = dtCopy.Rows[rowidx];
                        if (dtCopy.Columns.Contains(xItem))
                            if (!double.TryParse(row[xItem].ToString(), out itemValue))
                                itemValue = double.NaN;

                        if (!double.TryParse(row["DEFECT_DENSITY"].ToString(), out density))
                            density = double.NaN;

                        if (!double.TryParse(row["TOTAL"].ToString(), out total))
                            total = double.NaN;

                        if (!double.TryParse(row["SCAN_AREA"].ToString(), out scanArea))
                            scanArea = double.NaN;

                        /// Defect class 에 대한 환산 데이터 처리, 
                        /// Data Pivot 으로 인하여 Step Seq가 넘어옴
                        /// Step Seq 이후부터 Defect class(사용자 선택가능)
                        for (int colIdx = dtCopy.Columns["STEP_SEQ"].Ordinal + 1; colIdx < dtCopy.Columns.Count; colIdx++)
                        {
                            columnName = dtCopy.Columns[colIdx].ColumnName;
                            if (string.IsNullOrEmpty(row[columnName].ToString()))
                                continue;

                            if (!double.TryParse(row[columnName].ToString(), out dValue))
                                dValue = double.NaN;

                            if (chkDensity.Checked)
                                row[columnName] = (itemValue * (dValue / total)) / scanArea;
                            else
                                row[columnName] = itemValue * (dValue / total);
                        }
                    }
                    dtCopy.TableName = xItem;
                    displayDs.Tables.Add(dtCopy);
                }
            }

            /// 사용자 정의 Control에 Data Binding 처리한다.
            dpucDataSheetControl1.DecimalPlaces = (int)nudDecimalPlaces.Value;
            dpucDataSheetControl1.DefaultColumnWidth = 100;
            dpucDataSheetControl1.DataSource = displayDs;
        }

        //--

        private void GetDataProduct()
        {
            dlbProduct.ClearDataSource();
            dlbStep.ClearDataSource();
            dlbLot.ClearDataSource();
            dlbWafer.ClearDataSource();

            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = obj.GetProduct(
                );
            SetBinding(dt, dlbProduct);
        }

        //--

        private string GetDateToString(
            DateTimePicker datepicker,
            int addDay = 0
            )
        {
            return datepicker.Value.AddDays(addDay).ToString(DATE_FORMAT);
        }

        //--

        private string[] GetSelectedValues(
            DUCListBox listBox
            )
        {
            DataTable dt = listBox.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return null;

            object[] arr = listBox.SelectedValues;

            if (arr == null || arr.Length == 0)
                return null;

            string[] results = new string[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                results[i] = arr[i].ToString();

            return results;
        }

        //--

        private void SetBinding(
            DataTable dt,
            DUCListBox dListBox,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (dt != null)
            {
                dListBox.DisplayMember = dt.Columns[dispIndex].ColumnName;
                dListBox.ValueMember = dt.Columns[valueIndex].ColumnName;
            }
            dListBox.DataSource = dt;
        }

        #endregion [ Method ]
    }
}
