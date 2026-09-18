using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;
using DACrux.SEMDMS.RO;
using DACrux.Utility;
using FarPoint.Win.Spread.CellType;
using System.Diagnostics;
using DACrux.Framework.Controls;
using System.Threading.Tasks;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectDataSummary
        : DACruxUXBasic01, iSEMControl, IExportExcel
    {
        #region [ Data Field ]
        Dictionary<String, String> dicBreakDown = null;
        Dictionary<String, String> dicFuncs = null;
        Dictionary<String, String> dicInspInfo = null;
        Dictionary<String, String> dicYAxisInfo = null;
        // true 인 경우에는 Data RoundTrip 발생, false 의 경우에는 조회된 데이터로 데이터 Summary
        private bool IsView = false;
        Dictionary<int, String> dicDefectType = null;
        internal SizeOptionList SizeRanges = null;

        //--
        // field values
        private readonly string NONE = "NONE";
        private readonly string DEFECT_DENSITY = "DEFECT_DENSITY";
        private readonly string DEFECTIVE_DIE = "DEFECTIVE_DIE";
        private readonly string DEFECTIVE_DIE_PERCENTAGE = "DEFECTIVE_DIE_PERCENTAGE";
        private readonly string INSPECTIONAREA = "INSPECTION_AREA";
        private readonly string INSPECTIONTIME = "INSPECTION_TIME";
        private readonly string INSPECTEDDIE = "INSPECTED_DIE";
        private readonly string XINDEX = "XINDEX";
        private readonly string YINDEX = "YINDEX";
        private readonly string WAFER_COUNT = "WAFER_COUNT";
        private readonly string WAFERID = "WAFER_ID";
        private readonly string LOTID = "LOT_ID";

        #endregion [ Data Field ]

        //----------------------------------------------------------------------------------------------------------

        #region [ Constactor ]

        public frmDefectDataSummary()
        {
            InitializeComponent();
        }

        #endregion [ Constactor ]

        //----------------------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        //----------------------------------------------------------------------------------------------------------

        private void frmDefectDataSummary_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            Utility.FPSpreadUtil.InitSpread(fpSpread1);
            fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;

            FillBreakDown();
            FillFunction();
            FillInfoCols();
            FillYAxisInfo();

            SetBindingForDefectType();
        }

        //----------------------------------------------------------------------------------------------------------

        private void btnMove_Click(
            object sender,
            EventArgs e
            )
        {
            Button btn = sender as Button;
            if (btn == btnLeftToRight)
            {
                foreach (ListViewItem item in lsvAllItems.SelectedItems)
                {
                    if (lsvSelectedItems.Items.ContainsKey(item.Name))
                        continue;

                    ListViewItem selItem = lsvSelectedItems.Items.Add((ListViewItem)item.Clone());
                    selItem.Name = item.Name;
                    selItem.ImageIndex = 0;
                    selItem.Tag = item.Tag;
                }
            }
            else if (btn == btnRightToLeft)
            {
                foreach (ListViewItem item in lsvSelectedItems.SelectedItems)
                {
                    lsvSelectedItems.Items.Remove(item);
                }
            }
            else if (btn == btnAll)
            {
                foreach (ListViewItem item in lsvAllItems.Items)
                {
                    if (lsvSelectedItems.Items.ContainsKey(item.Name))
                        continue;

                    ListViewItem selItem = lsvSelectedItems.Items.Add((ListViewItem)item.Clone());
                    selItem.Name = item.Name;
                    selItem.ImageIndex = 0;
                    selItem.Tag = item.Tag;

                }
            }
            else if (btn == btnRemove)
            {
                lsvSelectedItems.Items.Clear();
            }

            lsvSelectedItems.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            IsView = true;
            DataSource = null;
        }

        //----------------------------------------------------------------------------------------------------------

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            DMReport obj = null;
            DataTable dt = null;
            String[] xItems = null;
            String item = String.Empty;
            String func = String.Empty;
            String breakDown = String.Empty;
            long[] Steps = null;

#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            try
            {
                if (Wafers == null)
                    return;
                this.Cursor = Cursors.WaitCursor;

                obj = new DMReport();
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                Steps = new long[Wafers.Length];
                for (int idx = 0; idx < Wafers.Length; idx++)
                {
                    Steps[idx] = Base.Convert.longParse(Wafers[idx].StepSeq);
                }

                if (Steps == null || Steps.Length <= 0)
                    return;

                xItems = new String[lsvSelectedItems.Items.Count];
                for (int idx = 0; idx < lsvSelectedItems.Items.Count; idx++)
                {
                    xItems[idx] = lsvSelectedItems.Items[idx].Tag as String;
                }

                if (xItems == null || xItems.Length <= 0)
                    return;

                item = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (IsView)
                {
                    IsView = false;
                    DataSource = obj.GetDmSummaryData(
                        Steps
                        );
                }

                MainForm.SetStatusMessage("Data 를 처리 중입니다.");
                breakDown = GetValueToKey(dicBreakDown, cmbBreakDown.Text);
                /// break down 별 yaxis 에 대한 계산 방법이 다름
                switch (breakDown)
                {
                    case "CLASSNUMBER":
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByDefectClassForDefectiveDies(
                                DataSource as DataTable,
                                xItems
                                );
                        else
                            dt = FillDataIsBreakDownByDefectClass(
                                DataSource as DataTable,
                                xItems
                                );
                        break;
                    case "DEFECTAREA":
                    case "XSIZE":
                    case "YSIZE":
                    case "DSIZE":
                        #region [ Defect Size에 대한 BreakDown 계산 ]
                        double dMaxinumValue = 0d;
                        double dMininumValue = 0d;

                        if (!ValidationOptionSize(out dMaxinumValue, out dMininumValue))
                            return;

                        SizeOptionList dataBreakDownList = new SizeOptionList();
                        int interval = (int)nudInterval.Value;
                        double dDiff = (dMaxinumValue - dMininumValue) / interval;

                        if (SizeRanges == null)
                        {
                            for (int idx = 0; idx < interval; idx++)
                            {
                                dataBreakDownList.Add(new SizeOption()
                                {
                                    MININUM = dMininumValue,
                                    MAXINUM = dMininumValue + dDiff,
                                    RANGENAME = String.Format("{0} <= x < {1}", dMininumValue, dMininumValue + dDiff)
                                });

                                dMininumValue += dDiff;
                            }
                        }
                        else
                        {
                            dataBreakDownList = SizeRanges;
                        }
                        #endregion [ Defect Size에 대한 BreakDown 계산 ]
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByDefectSizeForDefectiveDies(
                                DataSource as DataTable,
                                xItems,
                                dataBreakDownList
                                );
                        else
                            dt = FillDataIsBreakDownByDefectSize(
                                DataSource as DataTable,
                                xItems,
                                dataBreakDownList
                                );
                        break;
                    case "INSPECTED_DIE":
                    case "INSPECTION_TYPE":
                        break;
                    case "INSEPCTED_LOT":
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByInspectionForDefectiveDies(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                LOTID
                                );
                        else
                            dt = FillDataIsBreakDownByInspection(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                LOTID
                                );
                        break;
                    case "INSPECTED_LOT_WAFER":
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByInspectionForDefectiveDies(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                LOTID,
                                WAFERID
                                );
                        else
                            dt = FillDataIsBreakDownByInspection(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                LOTID,
                                WAFERID
                                );
                        break;
                    case "INSPECTED_WAFER":
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByInspectionForDefectiveDies(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                WAFERID
                                );
                        else
                            dt = FillDataIsBreakDownByInspection(
                                DataSource as DataTable,
                                xItems,
                                INSPECTIONTIME,
                                WAFERID
                                );
                        break;
                    case "INSPECTION_TIME":
                    case "INSPECTOR":
                        if (item.Contains(DEFECTIVE_DIE))
                            dt = FillDataIsBreakDownByInspectionForDefectiveDies(
                                DataSource as DataTable,
                                xItems,
                                breakDown
                                );
                        else
                            dt = FillDataIsBreakDownByInspection(
                                DataSource as DataTable,
                                xItems,
                                breakDown
                                );
                        break;
                    default:
                        dt = FillDataIsBreakDownByNone(
                            DataSource as DataTable,
                            xItems
                            );
                        break;
                }

                if (item.Contains("COUNT") || item.Contains("DEFECTS") || item.Contains(INSPECTEDDIE))
                    decimalPlaces.Value = 0;
                else if (decimalPlaces.Value == 0)
                    decimalPlaces.Value = 3;

                NumberCellType numberCellType = new NumberCellType();
                numberCellType.DecimalPlaces = (int)decimalPlaces.Value;
                numberCellType.MaximumValue = 99999999999999;
                numberCellType.MinimumValue = -99999999999999;

                Utility.FPSpreadUtil.InitSpread(fpSpread1);
                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
                if (dt != null && dt.Rows.Count > 0)
                {
                    Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1);

                    for (int idx = xItems.Length; idx < dt.Columns.Count; idx++)
                        fpSpread1_Sheet1.Columns[idx].CellType = numberCellType;

                    Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                }
            }
            finally
            {
#if DEBUG
                Debug.WriteLine(String.Format("ElapsedMilliseconds: {0}", sw.ElapsedMilliseconds));
#endif
                this.Cursor = Cursors.Default;
                MainForm.SetStatusMessage(null);
            }
        }

        //----------------------------------------------------------------------------------------------------------

        private void btnSize_Click(
            object sender,
            EventArgs e
            )
        {
            double dMaxinumValue = 0d;
            double dMininumValue = 0d;

            if (!ValidationOptionSize(out dMaxinumValue, out dMininumValue))
                return;

            dlgSizePopup dlg = new dlgSizePopup(
                dMaxinumValue,
                dMininumValue,
                (int)nudInterval.Value
                );

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                SizeRanges = null;
                return;
            }

            SizeRanges = dlg.SeletedValues;
        }

        //----------------------------------------------------------------------------------------------------------

        private void cmbBreakDown_SelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (String.Equals(GetValueToKey(dicBreakDown, (sender as ComboBox).Text), "DEFECTAREA")
                || String.Equals(GetValueToKey(dicBreakDown, (sender as ComboBox).Text), "XSIZE")
                || String.Equals(GetValueToKey(dicBreakDown, (sender as ComboBox).Text), "YSIZE")
                || String.Equals(GetValueToKey(dicBreakDown, (sender as ComboBox).Text), "DSIZE"))
                grbSize.Enabled = true;
            else
                grbSize.Enabled = false;

            if (String.Equals(GetValueToKey(dicBreakDown, (sender as ComboBox).Text), "CLASSNUMBER"))
                grbNormalize.Enabled = true;
            else grbNormalize.Enabled = false;
        }

        //----------------------------------------------------------------------------------------------------------

        private void cmbYAxis_SelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            ComboBox combo = (sender as ComboBox);
            string sYAxis = GetValueToKey(dicYAxisInfo, combo.SelectedItem.ToString());

            cmbFunc.SelectedIndex = 0;
            cmbSecBreakdown.SelectedIndex = 0;
        }

        //----------------------------------------------------------------------------------------------------------

        private void chkNormalize_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            rbWithBin.Enabled = rbWithoutBin.Enabled = (sender as CheckBox).Checked;
        }

        //----------------------------------------------------------------------------------------------------------

        private void lsvItems_MouseDoubleClick(
            object sender,
            MouseEventArgs e
            )
        {
            ListView lsv = (sender as ListView);
            if (lsv == lsvAllItems
                && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                foreach (ListViewItem item in (sender as ListView).SelectedItems)
                {
                    if (!lsvSelectedItems.Items.ContainsKey(item.Name))
                    {
                        ListViewItem addItem = lsvSelectedItems.Items.Add((ListViewItem)item.Clone());
                        addItem.Name = item.Name;
                        addItem.ImageIndex = 0;
                        addItem.Tag = item.Tag;
                    }
                }
            }
            else if (lsv == lsvSelectedItems
                && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                foreach (ListViewItem item in (sender as ListView).SelectedItems)
                {
                    lsvSelectedItems.Items.Remove(item);
                }
            }

            lsvSelectedItems.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            IsView = true;
            DataSource = null;
        }

        //----------------------------------------------------------------------------------------------------------

        private void decimalPlaces_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            List<int> lst = new List<int>();
            for (int idx = lsvSelectedItems.Items.Count; idx < fpSpread1_Sheet1.ColumnCount; idx++)
            {
                lst.Add(idx);
            }
            Utility.FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, (int)decimalPlaces.Value, lst.ToArray());
            fpSpread1.Refresh();
        }

        //----------------------------------------------------------------------------------------------------------

        private void txtMaxinum_KeyPress(
            object sender,
            KeyPressEventArgs e
            )
        {
            SizeRanges = null;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //----------------------------------------------------------------------------------------------------------

        private void txtMininum_KeyPress(
            object sender,
            KeyPressEventArgs e
            )
        {
            SizeRanges = null;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        #endregion [ Event Handler ]

        //----------------------------------------------------------------------------------------------------------

        #region [ Method ]

        //----------------------------------------------------------------------------------------------------------

        private void AddColumns(
            object obj,
            DataTable dt
            )
        {
            if (obj is List<int>)
            {
                // Defect Class Number에 대한 Description으로 Column 생성
                List<int> dataBreakDown = obj as List<int>;
                for (int idx = 0; idx < dataBreakDown.Count; idx++)
                {
                    if (dt.Columns.Contains(dataBreakDown[idx].ToString()))
                        continue;

                    // Defect class number 
                    // Name이 존재하는 경우, Name으로 생성
                    // 존재하지 않는 경우에는 Class Number로 생성한다
                    if (!dicDefectType.ContainsKey(dataBreakDown[idx]))
                        dt.Columns.Add(new DataColumn(dataBreakDown[idx].ToString(), typeof(double)));
                    else
                        dt.Columns.Add(new DataColumn(dicDefectType[dataBreakDown[idx]], typeof(double)));
                }
            }
            else if (obj is SizeOptionList)
            {
                SizeOptionList dataBreakDown = obj as SizeOptionList;
                for (int idx = 0; idx < dataBreakDown.Count; idx++)
                {
                    if (dt.Columns.Contains(dataBreakDown[idx].RANGENAME))
                        continue;

                    // Defect Size 에 대한 Range 로 Column 생성
                    // Klarity가 이와 같이 되어 있음
                    dt.Columns.Add(new DataColumn(dataBreakDown[idx].RANGENAME, typeof(double)));
                }
            }
            else if (obj is List<String>)
            {
                List<String> dataBreakDown = obj as List<String>;
                for (int idx = 0; idx < dataBreakDown.Count; idx++)
                {
                    if (dt.Columns.Contains(dataBreakDown[idx]))
                        continue;

                    // Defect Size 에 대한 Range 로 Column 생성
                    // Klarity가 이와 같이 되어 있음
                    dt.Columns.Add(new DataColumn(dataBreakDown[idx], typeof(double)));
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------

        private void AddDefectData(
            ref Dictionary<String, Double> dicInspectionArea,
            ref Dictionary<String, NodeList> dic,
            DataRow row,
            String keyCondition,
            String keyValue,
            String yAxis
            )
        {
            NodeList nodes = null;
            Node node = null;

            if (!dic.ContainsKey(keyCondition))
            {
                nodes = new NodeList();
                node = new Node();
                node.Key = keyValue;

                if (!row.Table.Columns.Contains(yAxis))
                    node.Values.Add(1);
                else
                    node.Values.Add(Base.Convert.doubleParse(row[yAxis].ToString()));

                nodes.Add(node);
                dic.Add(keyCondition, nodes);
            }
            else
            {
                nodes = dic[keyCondition];
                int idx = nodes.BinarySearch(keyValue);
                if (idx < 0)
                {
                    //Node에 데이터가 존재하지 않는 경우
                    node = new Node();
                    node.Key = keyValue;

                    if (!row.Table.Columns.Contains(yAxis))
                        node.Values.Add(1);
                    else
                        node.Values.Add(Base.Convert.doubleParse(row[yAxis].ToString()));

                    nodes.Insert(~idx, node);
                }
                else
                {
                    //Node에 데이터가 존재하는 경우
                    node = nodes[idx];

                    if (!row.Table.Columns.Contains(yAxis))
                        node.Values.Add(1);
                    else
                        node.Values.Add(Base.Convert.doubleParse(row[yAxis].ToString()));

                    nodes[idx] = node;
                }
                dic[keyCondition] = nodes;
            }

            #region [ Inspection Area ]
            if (!dicInspectionArea.ContainsKey(keyCondition))
                dicInspectionArea.Add(keyCondition, Base.Convert.doubleParse(row[INSPECTIONAREA].ToString()));
            #endregion [ Inspection Area ]
        }

        //----------------------------------------------------------------------------------------------------------

        private void AddDefectiveDieData(
            ref Dictionary<String, Double> dicInspectionDies,
            ref Dictionary<String, Dies> dicDefectiveDies,
            ref Dictionary<String, DefectiveDies> dic,
            DataRow row,
            String keyCondition,
            String keyValue,
            Point point
            )
        {
            DefectiveDies defectiveDies = null;
            DefectiveDieInfo dieInfo = null;
            Dies dies = null;

            #region [ BreakDown 별 Defective Die ]
            if (!dic.ContainsKey(keyCondition))
            {
                defectiveDies = new DefectiveDies();
                dieInfo = new DefectiveDieInfo();
                dieInfo.Key = keyValue;
                dieInfo.DieIndex.Add(point);
                defectiveDies.Add(dieInfo);
                dic.Add(keyCondition, defectiveDies);
            }
            else
            {
                defectiveDies = dic[keyCondition];
                dieInfo = null;
                int idx = defectiveDies.BinarySearch(keyValue);
                if (idx < 0)
                {
                    //Defective Dies에 데이터가 존재하지 않는 경우
                    dieInfo = new DefectiveDieInfo();
                    dieInfo.Key = keyValue;
                    dieInfo.DieIndex.Add(point);
                    defectiveDies.Insert(~idx, dieInfo);
                }
                else
                {
                    //Defective Dies에 데이터가 존재하는 경우
                    dieInfo = defectiveDies[idx];
                    if (!dieInfo.DieIndex.Contains(point))
                        dieInfo.DieIndex.Add(point);
                    defectiveDies[idx] = dieInfo;
                }

                dic[keyCondition] = defectiveDies;
            }
            #endregion [ BreakDown 별 Defective Die ]
            #region [ Defective Die ]
            if (!dicDefectiveDies.ContainsKey(keyCondition))
            {
                dies = new Dies();
                dies.Add(point);
                dicDefectiveDies.Add(keyCondition, dies);
            }
            else
            {
                dies = dicDefectiveDies[keyCondition];
                if (!dies.Contains(point))
                    dies.Add(point);
                dicDefectiveDies[keyCondition] = dies;
            }
            #endregion [ Defective Die ]
            #region [ Inspection Die ]
            if (!dicInspectionDies.ContainsKey(keyCondition))
            {
                double dValue = Base.Convert.doubleParse(row[INSPECTEDDIE].ToString());
                dicInspectionDies.Add(keyCondition, dValue);
            }
            #endregion [ Inspection Die ]
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        ///  BreakDown is NONE 
        ///  해당하는 Defective Die Count 계산
        /// </summary>
        private double[] CalculationDefectiveDieCount(
            DataRow[] rows,
            string keyCondition
            )
        {
            Dictionary<string, Dictionary<Point, int>> dic = new Dictionary<string, Dictionary<Point, int>>();
            Dictionary<Point, int> dicTmp = null;
            Point point = new Point();

            foreach (DataRow row in rows)
            {
                point = new Point(
                    Base.Convert.intParse(row[XINDEX].ToString()),
                    Base.Convert.intParse(row[YINDEX].ToString())
                    );

                if (!dic.ContainsKey(keyCondition))
                {
                    dicTmp = new Dictionary<Point, int>();
                    dicTmp.Add(point, 1);
                    dic.Add(keyCondition, dicTmp);
                }
                else
                {
                    dicTmp = dic[keyCondition];
                    if (!dicTmp.ContainsKey(point))
                    {
                        dicTmp.Add(point, 1);
                    }
                    else
                    {
                        dicTmp[point]++;
                    }
                    dic[keyCondition] = dicTmp;
                }
            }

            double[] rValue = new double[dic.Count];
            List<string> wafers = new List<string>(dic.Keys);
            for (int idx = 0; idx < wafers.Count; idx++)
            {
                rValue[idx] = dic[wafers[idx]].Count;
            }

            return rValue;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        ///  BreakDown is NONE 
        ///  해당하는 Inspection Die 계산
        /// </summary>
        private int CalculationInspectionDie(
            DataRow[] rows,
            string sYAxis
            )
        {
            List<int> lst = new List<int>();
            foreach (DataRow row in rows)
            {
                int value = Base.Convert.intParse(row[sYAxis].ToString());
                int idx = lst.BinarySearch(value);
                if (idx < 0)
                    lst.Insert(~idx, value);
            }

            return lst[lst.Count - 1];
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown is NONE 
        /// 해당하는 Wafer Count 계산
        /// </summary>
        private int CalculationWaferCount(
            DataRow[] rows
            )
        {
            List<String> lst = new List<string>();

            foreach (DataRow row in rows)
            {
                int idx = lst.BinarySearch(row["WAFER_SEQ"].ToString());
                if (idx < 0)
                    lst.Insert(~idx, row["WAFER_SEQ"].ToString());
            }

            return lst.Count;
        }

        //----------------------------------------------------------------------------------------------------------

        private DataTable FillDataIsBreakDownByNone(
            DataTable rawData,
            string[] xItem
            )
        {
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);
            String[] selectedConditions = SelectedXItem(xItem);
            String selectedCondition = String.Empty;

            DataTable dt = rawData.DefaultView.ToTable(true, xItem);
            selectedCondition = String.Format("{0}", String.Join(" AND ", selectedConditions)).Replace('[', '{').Replace(']', '}');

            dt.Columns.Add(new DataColumn(sYAxis, typeof(string)));

            DataRow[] rows = null;
            foreach (DataRow r in dt.Rows)
            {
                GetSelectedConditionValues(selectedConditions, r, xItem);
                if (String.Equals(sYAxis, INSPECTEDDIE))
                {
                    rows = rawData.Select(String.Format(selectedCondition, selectedConditions));
                    r[sYAxis] = CalculationInspectionDie(rows, sYAxis);
                }
                else if (rawData.Columns.Contains(sYAxis))
                {
                    r[sYAxis] = rawData.Compute(String.Format("{0}({1})", (String.Equals(sFunction, NONE) ? "SUM" : sFunction), sYAxis), String.Format(selectedCondition, selectedConditions));
                }
                else
                {
                    rows = rawData.Select(String.Format(selectedCondition, selectedConditions));
                    r[sYAxis] = GetValue(rows, sYAxis, String.Format("{0}", String.Join(",", selectedConditions)));
                }
            }
            dt.AcceptChanges();

            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Defect Class 에 대한 Defective Die / Defective Die Percentages 를 제외한 나머지 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByDefectClass(
            DataTable rawData,
            string[] xItem
            )
        {
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sBreakDown = GetValueToKey(dicBreakDown, cmbBreakDown.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);
            List<int> dataBreakDownList = new List<int>();
            String keyCondition = String.Empty;

            /// 사용자가 선택한 X Item에 대한 값을 저장
            Dictionary<string, NodeList> dic = new Dictionary<string, NodeList>();
            Dictionary<string, double> dicInspectionArea = new Dictionary<string, double>();

            DataTable dt = rawData.DefaultView.ToTable(true, xItem);
            NodeList nodes = null;
            try
            {
                // Summary
                foreach (DataRow r in rawData.Rows)
                {
                    int idx = dataBreakDownList.BinarySearch(Base.Convert.intParse(r[sBreakDown].ToString()));
                    if (idx < 0)
                        dataBreakDownList.Insert(~idx, Base.Convert.intParse(r[sBreakDown].ToString()));

                    keyCondition = GetSelectedConditionValues(r, xItem);
                    int classNumber = Base.Convert.intParse(r[sBreakDown].ToString());
                    String className = String.Empty;
                    if (!dicDefectType.ContainsKey(classNumber))
                        className = classNumber.ToString();
                    else className = dicDefectType[classNumber];

                    AddDefectData(
                        ref dicInspectionArea,
                        ref dic,
                        r,
                        keyCondition,
                        className,
                        sYAxis
                        );
                }

                AddColumns(dataBreakDownList, dt);

                // Dictionary 에 저장된 데이터를 DataTable 로 변환
                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItem);
                    nodes = dic[keyCondition];
                    if (chkNormalize.Checked)
                    {
                        /// Item1: Total
                        /// Item2: Review Sum
                        Tuple<double, double> sumValue = GetTotalAndReviewSum(nodes, sFunction);
#if DEBUG
                        Debug.WriteLine(String.Format("Item1: {0}, Item2: {1}", sumValue.Item1, sumValue.Item2));
#endif
                        // Uncategorized 제외
                        nodes.Remove(dicDefectType[0]);
                        foreach (Node dv in nodes)
                        {
                            if (String.Equals(sYAxis, INSPECTEDDIE) || String.Equals(sYAxis, WAFER_COUNT))
                                r[dv.Key] = dv.Values[dv.Values.Count - 1];
                            else if (String.Equals(sYAxis, DEFECT_DENSITY))
                                r[dv.Key] = (GetValue(dv.Values.ToArray<double>(), sFunction) / sumValue.Item2 * sumValue.Item1) / dicInspectionArea[keyCondition];
                            else
                                r[dv.Key] = GetValue(dv.Values.ToArray<double>(), sFunction) / sumValue.Item2 * sumValue.Item1;
                        }
                    }
                    else
                    {
                        foreach (Node dv in nodes)
                        {
                            if (String.Equals(sYAxis, INSPECTEDDIE) || String.Equals(sYAxis, WAFER_COUNT))
                                r[dv.Key] = dv.Values[dv.Values.Count - 1];
                            else if (String.Equals(sYAxis, DEFECT_DENSITY))
                                r[dv.Key] = GetValue(dv.Values.ToArray<double>(), sFunction) / dicInspectionArea[keyCondition];
                            else
                                r[dv.Key] = GetValue(dv.Values.ToArray<double>(), sFunction);
                        }
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dicInspectionArea != null)
                {
                    dicInspectionArea.Clear();
                    dicInspectionArea = null;
                }

                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }
            }

            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Defect Class 에 대한 Defective Die / Defective Die Percentages 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByDefectClassForDefectiveDies(
            DataTable rawData,
            string[] xItem
            )
        {
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sBreakDown = GetValueToKey(dicBreakDown, cmbBreakDown.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);
            List<int> dataBreakDownList = new List<int>();
            String keyCondition = String.Empty;

            Dictionary<string, DefectiveDies> dic = new Dictionary<string, DefectiveDies>();
            Dictionary<string, Dies> dicDefectiveDies = new Dictionary<string, Dies>();
            Dictionary<string, double> dicInspectionDies = new Dictionary<string, double>();
            DataTable dt = null;

            try
            {
                dt = rawData.DefaultView.ToTable(true, xItem);
                // Summary
                foreach (DataRow r in rawData.Rows)
                {
                    int idx = dataBreakDownList.BinarySearch(Base.Convert.intParse(r[sBreakDown].ToString()));
                    if (idx < 0)
                        dataBreakDownList.Insert(~idx, Base.Convert.intParse(r[sBreakDown].ToString()));

                    keyCondition = GetSelectedConditionValues(r, xItem);
                    Point point = new Point(
                        Base.Convert.intParse(r[XINDEX].ToString()),
                        Base.Convert.intParse(r[YINDEX].ToString())
                        );

                    int classNumber = Base.Convert.intParse(r[sBreakDown].ToString());
                    String className = String.Empty;
                    if (!dicDefectType.ContainsKey(classNumber))
                        className = classNumber.ToString();
                    else className = dicDefectType[classNumber];
                    AddDefectiveDieData(
                        ref dicInspectionDies,
                        ref dicDefectiveDies,
                        ref dic,
                        r,
                        keyCondition,
                        className,
                        point
                        );
                }

                AddColumns(dataBreakDownList, dt);

                // Dictionary 에 저장된 데이터를 DataTable 로 변환
                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItem);
                    DefectiveDies defectiveDies = dic[keyCondition];
                    if (chkNormalize.Checked)
                    {
                        /// Item1: Total
                        /// Item2: Review Sum
                        Tuple<double, double> sumValue = GetTotalAndReviewSum(defectiveDies, sFunction);
#if DEBUG
                        Debug.WriteLine(String.Format("Item1: {0}, Item2: {1}", sumValue.Item1, sumValue.Item2));
#endif
                        // Uncategorized 제외
                        defectiveDies.Remove(dicDefectType[0]);
                        foreach (DefectiveDieInfo dieInfo in defectiveDies)
                        {
                            if (String.Equals(sYAxis, DEFECTIVE_DIE_PERCENTAGE))
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / sumValue.Item2 * (double)dicDefectiveDies[keyCondition].Count) / (double)dicInspectionDies[keyCondition] * 100;
                            else
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / sumValue.Item2 * (double)dicDefectiveDies[keyCondition].Count);
                        }
                    }
                    else
                    {
                        foreach (DefectiveDieInfo dieInfo in defectiveDies)
                        {
                            if (String.Equals(sYAxis, DEFECTIVE_DIE_PERCENTAGE))
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / (double)dicDefectiveDies[keyCondition].Count) * 100;
                            else
                                r[dieInfo.Key] = dieInfo.DieIndex.Count;
                        }
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }

                if (dicDefectiveDies != null)
                {
                    dicDefectiveDies.Clear();
                    dicDefectiveDies = null;
                }

                if (dicInspectionDies != null)
                {
                    dicInspectionDies.Clear();
                    dicInspectionDies = null;
                }
            }

            //--

            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Defect Area, Defect Size, X Size, Y Size 에 대한 Defective Die / Defective Die Percentages 를 제외한 나머지 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByDefectSize(
            DataTable rawData,
            string[] xItem,
            SizeOptionList dataBreakDownList
            )
        {
            String keyCondition = String.Empty;
            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sBreakDown = GetValueToKey(dicBreakDown, cmbBreakDown.Text);
            String sFunc = GetValueToKey(dicFuncs, cmbFunc.Text);

            Dictionary<string, NodeList> dic = new Dictionary<string, NodeList>();
            Dictionary<string, double> dicInspectionArea = new Dictionary<string, double>();
            DataTable dt = rawData.DefaultView.ToTable(true, xItem);
            NodeList nodes = null;
            try
            {
                foreach (DataRow r in rawData.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItem);
                    double dBreakDownValue = Base.Convert.doubleParse(r[sBreakDown].ToString());
                    string sDescription = dataBreakDownList.GetDescription(dBreakDownValue);
                    AddDefectData(
                        ref dicInspectionArea,
                        ref dic,
                        r,
                        keyCondition,
                        sDescription,
                        sYAxis
                        );
                }

                AddColumns(dataBreakDownList, dt);

                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItem);
                    nodes = dic[keyCondition];
                    foreach (Node n in nodes)
                    {
                        if (String.IsNullOrEmpty(n.Key))
                            continue;

                        if (String.Equals(sYAxis, INSPECTEDDIE) || String.Equals(sYAxis, WAFER_COUNT))
                            r[n.Key] = n.Values[n.Values.Count - 1];
                        else if (String.Equals(sYAxis, DEFECT_DENSITY))
                            r[n.Key] = GetValue(n.Values.ToArray<double>(), sFunc) / dicInspectionArea[keyCondition];
                        else
                            r[n.Key] = GetValue(n.Values.ToArray<double>(), sFunc);
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dicInspectionArea != null)
                {
                    dicInspectionArea.Clear();
                    dicInspectionArea = null;
                }

                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }
            }
            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Defect Area, Defect Size, X Size, Y Size 에 대한 Defective Die / Defective Die Percentages 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByDefectSizeForDefectiveDies(
            DataTable rawData,
            string[] xItemFilter,
            SizeOptionList dataBreakDownList
            )
        {
            String[] selectedConditions = SelectedXItem(xItemFilter);
            String keyCondition = String.Empty;
            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sBreakDown = GetValueToKey(dicBreakDown, cmbBreakDown.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);

            Dictionary<String, DefectiveDies> dic = new Dictionary<String, DefectiveDies>();
            Dictionary<String, Dies> dicDefectiveDies = new Dictionary<String, Dies>();
            Dictionary<String, Double> dicInspectionDies = new Dictionary<String, Double>();
            DataTable dt = null;

            try
            {
                dt = rawData.DefaultView.ToTable(true, xItemFilter);
                foreach (DataRow r in rawData.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    double dBreakDownValue = Base.Convert.doubleParse(r[sBreakDown].ToString());
                    string sDescription = dataBreakDownList.GetDescription(dBreakDownValue);

                    Point point = new Point(
                        Base.Convert.intParse(r[XINDEX].ToString()),
                        Base.Convert.intParse(r[YINDEX].ToString())
                        );
                    AddDefectiveDieData(
                        ref dicInspectionDies,
                        ref dicDefectiveDies,
                        ref dic,
                        r,
                        keyCondition,
                        sDescription,
                        point
                        );
                }

                AddColumns(dataBreakDownList, dt);

                // Dictionary 에 저장된 데이터를 DataTable 로 변환
                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    DefectiveDies defectiveDies = dic[keyCondition];
                    foreach (DefectiveDieInfo dieInfo in defectiveDies)
                    {
                        if (String.IsNullOrEmpty(dieInfo.Key))
                            continue;

                        if (String.Equals(sYAxis, DEFECTIVE_DIE_PERCENTAGE))
                            r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / (double)dicDefectiveDies[keyCondition].Count) * 100;
                        else
                            r[dieInfo.Key] = dieInfo.DieIndex.Count;
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }

                if (dicDefectiveDies != null)
                {
                    dicDefectiveDies.Clear();
                    dicDefectiveDies = null;
                }

                if (dicInspectionDies != null)
                {
                    dicInspectionDies.Clear();
                    dicInspectionDies = null;
                }
            }

            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Inspector 에 대한 Defective Die / Defective Die Percentages 를 제외한 나머지 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByInspection(
            DataTable rawData,
            string[] xItemFilter,
            params string[] breakDowns
            )
        {
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);
            List<String> dataBreakDownList = new List<String>();
            String keyCondition = String.Empty;
            String breakDownCondition = String.Empty;

            DataTable dt = null;
            Dictionary<string, NodeList> dic = new Dictionary<string, NodeList>();
            Dictionary<string, double> dicInspectionArea = new Dictionary<string, double>();

            try
            {
                dt = rawData.DefaultView.ToTable(true, xItemFilter);
                NodeList nodes = null;
                foreach (DataRow r in rawData.Rows)
                {
                    breakDownCondition = GetSelectedConditionValues(r, breakDowns);
                    int idx = dataBreakDownList.BinarySearch(breakDownCondition);
                    if (idx < 0)
                        dataBreakDownList.Insert(~idx, breakDownCondition);

                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    AddDefectData(
                        ref dicInspectionArea,
                        ref dic,
                        r,
                        keyCondition,
                        breakDownCondition,
                        sYAxis
                        );
                }

                AddColumns(dataBreakDownList, dt);

                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    nodes = dic[keyCondition];
                    foreach (Node n in nodes)
                    {
                        if (String.IsNullOrEmpty(n.Key))
                            continue;

                        if (String.Equals(sYAxis, INSPECTEDDIE) || String.Equals(sYAxis, WAFER_COUNT))
                            r[n.Key] = n.Values[n.Values.Count - 1];
                        else if (String.Equals(sYAxis, DEFECT_DENSITY))
                            r[n.Key] = GetValue(n.Values.ToArray<double>(), sFunction) / dicInspectionArea[keyCondition];
                        else
                            r[n.Key] = GetValue(n.Values.ToArray<double>(), sFunction);
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dataBreakDownList != null)
                {
                    dataBreakDownList.Clear();
                    dataBreakDownList = null;
                }

                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }

                if (dicInspectionArea != null)
                {
                    dicInspectionArea.Clear();
                    dicInspectionArea = null;
                }
            }
            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// BreakDown 항목이 Inspector 에 대한 Defective Die / Defective Die Percentages 계산
        /// </summary>
        private DataTable FillDataIsBreakDownByInspectionForDefectiveDies(
            DataTable rawData,
            string[] xItemFilter,
            params string[] breakDowns
            )
        {
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            String sYAxis = GetValueToKey(dicYAxisInfo, cmbYAxis.Text);
            String sFunction = GetValueToKey(dicFuncs, cmbFunc.Text);
            List<String> dataBreakDownList = new List<String>();
            String keyCondition = String.Empty;
            String breakDownCondition = String.Empty;

            Dictionary<string, DefectiveDies> dic = new Dictionary<string, DefectiveDies>();
            Dictionary<string, Dies> dicDefectiveDies = new Dictionary<string, Dies>();
            Dictionary<string, double> dicInspectionDies = new Dictionary<string, double>();
            DataTable dt = null;

            try
            {
                dt = rawData.DefaultView.ToTable(true, xItemFilter);
                // Summary
                foreach (DataRow r in rawData.Rows)
                {
                    breakDownCondition = GetSelectedConditionValues(r, breakDowns);
                    int idx = dataBreakDownList.BinarySearch(breakDownCondition);
                    if (idx < 0)
                        dataBreakDownList.Insert(~idx, breakDownCondition);

                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    Point point = new Point(
                        Base.Convert.intParse(r[XINDEX].ToString()),
                        Base.Convert.intParse(r[YINDEX].ToString())
                        );
                    AddDefectiveDieData(
                        ref dicInspectionDies,
                        ref dicDefectiveDies,
                        ref dic,
                        r,
                        keyCondition,
                        breakDownCondition,
                        point
                        );
                }

                AddColumns(dataBreakDownList, dt);

                // Dictionary 에 저장된 데이터를 DataTable 로 변환
                foreach (DataRow r in dt.Rows)
                {
                    keyCondition = GetSelectedConditionValues(r, xItemFilter);
                    DefectiveDies defectiveDies = dic[keyCondition];
                    if (chkNormalize.Checked)
                    {
                        /// Item1: Total
                        /// Item2: Review Sum
                        Tuple<double, double> sumValue = GetTotalAndReviewSum(defectiveDies, sFunction);
#if DEBUG
                        Debug.WriteLine(String.Format("Item1: {0}, Item2: {1}", sumValue.Item1, sumValue.Item2));
#endif
                        // Uncategorized 제외
                        defectiveDies.Remove(dicDefectType[0]);
                        foreach (DefectiveDieInfo dieInfo in defectiveDies)
                        {
                            if (String.Equals(sYAxis, DEFECTIVE_DIE_PERCENTAGE))
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / sumValue.Item2 * (double)dicDefectiveDies[keyCondition].Count) / (double)dicInspectionDies[keyCondition] * 100;
                            else
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / sumValue.Item2 * (double)dicDefectiveDies[keyCondition].Count);
                        }
                    }
                    else
                    {
                        foreach (DefectiveDieInfo dieInfo in defectiveDies)
                        {
                            if (String.Equals(sYAxis, DEFECTIVE_DIE_PERCENTAGE))
                                r[dieInfo.Key] = ((double)dieInfo.DieIndex.Count / (double)dicDefectiveDies[keyCondition].Count) * 100;
                            else
                                r[dieInfo.Key] = dieInfo.DieIndex.Count;
                        }
                    }
                }
                dt.AcceptChanges();
            }
            finally
            {
                if (dic != null)
                {
                    dic.Clear();
                    dic = null;
                }

                if (dicDefectiveDies != null)
                {
                    dicDefectiveDies.Clear();
                    dicDefectiveDies = null;
                }

                if (dicInspectionDies != null)
                {
                    dicInspectionDies.Clear();
                    dicInspectionDies = null;
                }
            }

            //--

            return dt;
        }

        //----------------------------------------------------------------------------------------------------------

        private String GetSelectedConditionValues(
            DataRow r,
            string[] xItems
            )
        {
            String[] selectedConditions = new String[xItems.Length];
            for (int colIdx = 0; colIdx < xItems.Length; colIdx++)
            {
                string value = r[xItems[colIdx]].ToString();
                selectedConditions[colIdx] = value.Trim();
            }

            return String.Format("{0}", String.Join("|", selectedConditions));
        }

        //----------------------------------------------------------------------------------------------------------

        private void GetSelectedConditionValues(
            String[] selectedConditions,
            DataRow r,
            string[] xItems
            )
        {
            for (int colIdx = 0; colIdx < xItems.Length; colIdx++)
            {
                selectedConditions[colIdx] = r[xItems[colIdx]].ToString();
            }
        }

        //----------------------------------------------------------------------------------------------------------

        private Tuple<double, double> GetTotalAndReviewSum(
            object values,
            String sFunction
            )
        {
            double rTotal = 0d;
            double rValue = 0d;
            // defect class "0" 이 아닌 값만 Review로 간주
            string sKey = dicDefectType[0];

            if (values is NodeList)
            {
                foreach (Node dv in (values as NodeList))
                {
                    rTotal += GetValue(dv.Values.ToArray<double>(), sFunction);
                    if (!String.Equals(dv.Key, sKey))
                        rValue += GetValue(dv.Values.ToArray<double>(), sFunction);
                }
            }
            else if (values is DefectiveDies)
            {
                foreach (DefectiveDieInfo dv in (values as DefectiveDies))
                {
                    rTotal += dv.DieIndex.Count;
                    if (!String.Equals(dv.Key, sKey))
                        rValue += dv.DieIndex.Count;
                }
            }
            return new Tuple<double, double>(rTotal, rValue);
        }

        //----------------------------------------------------------------------------------------------------------

        private double GetValue(
            double[] values,
            string sFunction
            )
        {
            double dValue = double.NaN;
            switch (sFunction)
            {
                case "AVG":
                    dValue = values.Average();
                    break;
                case "STDDEV":
                    double dAvg = values.Average();
                    double sumOfDeviation = 0;
                    foreach (double v in values)
                        sumOfDeviation += (v * v);

                    double sumOfDeviationAverage = sumOfDeviation / (values.Length - 1);
                    if (double.IsNegativeInfinity(sumOfDeviationAverage))
                        dValue = double.NegativeInfinity;
                    else if (double.IsPositiveInfinity(sumOfDeviationAverage))
                        dValue = double.PositiveInfinity;
                    else if (double.IsNaN(sumOfDeviationAverage))
                        dValue = double.NaN;
                    else
                        dValue = Math.Sqrt(sumOfDeviationAverage - (dAvg * dAvg));
                    break;
                case "MEDIAN":
                    int iMid = values.Length / 2;
                    if (values.Length % 2 == 0)
                        dValue = (values[iMid - 1] + values[iMid]) / 2;
                    else if (values.Length == 1)
                        dValue = values[0];
                    else
                        dValue = values[iMid];
                    break;
                case "RANGE":
                    break;
                case "MAX":
                    dValue = values.Max();
                    break;
                case "MIN":
                    dValue = values.Min();
                    break;
                case "SUM":
                default:
                    dValue = values.Sum();
                    break;
            }

            return dValue;
        }

        //----------------------------------------------------------------------------------------------------------

        private object GetValue(
            DataRow[] rows,
            string sYAxis,
            string keyCondition
            )
        {
            double rValue = 0d;
            double dInspectionDieCnt = double.NaN;
            switch (sYAxis)
            {
                case "DEFECTS":
                    rValue = rows.Length;
                    break;
                case "DEFECT_DENSITY":
                    rValue = rows.Length / Base.Convert.doubleParse(rows[0][INSPECTIONAREA].ToString());
                    break;
                case "DEFECTIVE_DIE_COUNT":
                    rValue = CalculationDefectiveDieCount(rows, keyCondition).Sum();
                    break;
                case "DEFECTIVE_DIE_PERCENTAGE":
                    if (!double.TryParse(rows[0][INSPECTEDDIE].ToString(), out dInspectionDieCnt))
                        dInspectionDieCnt = double.NaN;
                    rValue = CalculationDefectiveDieCount(rows, keyCondition).Sum() / dInspectionDieCnt * 100;
                    break;
                case "DIE_COUNT":
                    rValue = Base.Convert.doubleParse(rows[0][INSPECTEDDIE].ToString());
                    break;
                case "WAFER_COUNT":
                    rValue = CalculationWaferCount(rows);
                    break;
            }

            return rValue;
        }

        //----------------------------------------------------------------------------------------------------------

        private string GetValueToKey(
            Dictionary<String, String> dicInfo, String value
            )
        {
            String keys = String.Empty;
            foreach (KeyValuePair<String, String> pv in dicInfo)
            {
                if (String.Equals(pv.Value, value))
                {
                    keys = pv.Key;
                    break;
                }
            }

            return keys;
        }

        private string[] SelectedXItem(
            string[] xItems
            )
        {
            String[] selectedConditions = new String[xItems.Length];
            for (int idx = 0; idx < xItems.Length; idx++)
            {
                selectedConditions[idx] = String.Format("{0} = '[{1}]'", xItems[idx], idx);
            }
            return selectedConditions;
        }

        //---------------------------------------------------------------------------------------------------------------

        private bool ValidationOptionSize(
            out double dMaxinum,
            out double dMininum
            )
        {
            dMaxinum = Base.Convert.doubleParse(txtMaxinum.Text);
            dMininum = Base.Convert.doubleParse(txtMininum.Text);
            if (String.IsNullOrEmpty(txtMaxinum.Text) || String.IsNullOrEmpty(txtMininum.Text) || (int)nudInterval.Value == 0)
            {
                ShowMessage("Invlid mininum, maxinum or interval value.");
                return false;
            }
            if (dMaxinum < dMininum)
            {
                ShowMessage(String.Format("Max Value: {0}, Min Value: {1} 확인 바랍니다.", dMaxinum, dMininum));
                return false;
            }
            return true;
        }

        #region [ Data Method ]

        private void Fill(
            ComboBox cmbBox,
            Dictionary<String, String> dic
            )
        {
            foreach (KeyValuePair<String, String> pv in dic)
            {
                cmbBox.Items.Add(pv.Value);
            }
            cmbBox.SelectedIndex = 0;
        }

        private void FillBreakDown(
            )
        {
            dicBreakDown = new Dictionary<String, String>();
            dicBreakDown.Add("NONE", "None");
            dicBreakDown.Add("DEFECTAREA", "Defect Area[sq-um]");
            dicBreakDown.Add("CLASSNUMBER", "Defect Class");
            dicBreakDown.Add("DSIZE", "Defect Size[um]");
            dicBreakDown.Add("XSIZE", "Defect X Size[um]");
            dicBreakDown.Add("YSIZE", "Defect Y Size[um]");
            //dicBreakDown.Add("INSPECTED_DIE", "Inspected Die");
            dicBreakDown.Add("INSEPCTED_LOT", "Insepcted Lot");
            dicBreakDown.Add("INSPECTED_LOT_WAFER", "Inspected Lot Wafer");
            dicBreakDown.Add("INSPECTED_WAFER", "Inspected Wafer");
            dicBreakDown.Add("INSPECTION_TIME", "Inspection Time");
            //dicBreakDown.Add("INSPECTION_TYPE", "Inspection Type");
            dicBreakDown.Add("INSPECTOR", "Inspector");

            Fill(cmbBreakDown, dicBreakDown);
        }

        /// <summary>
        /// X 축에 대한 선택 항목
        /// </summary>
        private void FillInfoCols(
            )
        {
            dicInspInfo = new Dictionary<string, string>();
            dicInspInfo.Add("PRODUCT", "DEVICE");
            dicInspInfo.Add("DIE_PITCH_X", "DIE PITCH X");
            dicInspInfo.Add("DIE_PITCH_Y", "DIE PITCH Y");
            dicInspInfo.Add("FACTORY", "FACTORY");
            dicInspInfo.Add("INSPECTION_AREA", "INSPECTED AREA");
            dicInspInfo.Add("INSPECTION_TIME", "INSPECTION TIME");
            dicInspInfo.Add("INSPECTOR", "INSPECTOR");
            dicInspInfo.Add("LOT_ID", "LOT ID");
            dicInspInfo.Add("SLOT_ID", "SLOT ID");
            dicInspInfo.Add("STEP_ID", "STEP ID");
            dicInspInfo.Add("WAFER_ID", "WAFER ID");

            lsvAllItems.HeaderStyle = ColumnHeaderStyle.None;
            lsvAllItems.Columns.Add("ITEM", -2, HorizontalAlignment.Left);

            lsvSelectedItems.HeaderStyle = ColumnHeaderStyle.None;
            lsvSelectedItems.Columns.Add("ITEM", -2, HorizontalAlignment.Left);

            lsvAllItems.Items.Clear();
            ListViewItem item = null;
            foreach (KeyValuePair<String, String> pv in dicInspInfo)
            {
                item = lsvAllItems.Items.Add(new ListViewItem(pv.Value));
                item.Name = pv.Key;
                item.ImageIndex = 0;
                item.Tag = pv.Key;
            }

            lsvAllItems.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            lsvSelectedItems.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //---------------------------------------------------------------------------------------------------------------

        private void FillFunction(
            )
        {
            dicFuncs = new Dictionary<String, String>();
            dicFuncs.Add("NONE", "None");
            dicFuncs.Add("AVG", "Average");
            dicFuncs.Add("SUM", "Sum");
            dicFuncs.Add("STDDEV", "Std. Deviation");
            dicFuncs.Add("MEDIAN", "Median");
            dicFuncs.Add("RANGE", "Range");
            dicFuncs.Add("MAX", "Max");
            dicFuncs.Add("MIN", "Min");

            Fill(cmbFunc, dicFuncs);
            Fill(cmbSecBreakdown, dicFuncs);
        }

        /// <summary>
        /// Y 축에 대한 범례를 설정하는 부분
        /// </summary>
        private void FillYAxisInfo(
            )
        {
            dicYAxisInfo = new Dictionary<String, String>();
            dicYAxisInfo.Add("DEFECTAREA", "Defect Area");
            dicYAxisInfo.Add("DEFECTS", "Defect Count");
            dicYAxisInfo.Add("DEFECT_DENSITY", "Defect Density");
            dicYAxisInfo.Add("DEFECTIVE_DIE_COUNT", "Defective Die Count");
            dicYAxisInfo.Add("DEFECTIVE_DIE_PERCENTAGE", "Defective Die Percentage");
            dicYAxisInfo.Add("INSPECTED_DIE", "Die Count"); // 해당 부분 문의 필요
            dicYAxisInfo.Add("IMAGECOUNT", "Image Count");
            dicYAxisInfo.Add("WAFER_COUNT", "Wafer Count"); // 해당 부분 문의 필요

            Fill(cmbYAxis, dicYAxisInfo);
        }

        #endregion [ Data Method ]

        //---------------------------------------------------------------------------------------------------------------

        #region [ Interface Method ]

        //---------------------------------------------------------------------------------------------------------------

        public void DrawWafer(
            Base.DPWafer[] wafer
            )
        {
            Wafers = wafer;
            IsView = true;
            DataSource = null;
            grbWafer.Text = string.Format("Inspected Wafer: {0} ", wafer.Length);
        }

        //---------------------------------------------------------------------------------------------------------------

        public void ExportExcel(
            )
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(fpSpread1);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        //---------------------------------------------------------------------------------------------------------------

        #endregion [ Interface Method ]

        private void SetBindingForDefectType(
            )
        {
            SEMConfiguration obj = new SEMConfiguration();
            dicDefectType = obj.GetDefectTypeAll();
        }

        #endregion [ Method ]

        //---------------------------------------------------------------------------------------------------------------

        #region [ Property ]
        public Base.DPWafer[] Wafers
        {
            get;
            private set;
        }

        public object DataSource
        {
            get;
            private set;
        }
        #endregion [ Property ]
    }


    #region [ Internal Class ]
    internal class SizeOption
    {
        internal double MININUM;
        internal double MAXINUM;
        internal string RANGENAME;
    }
    internal class SizeOptionList : List<SizeOption>
    {
        internal string GetDescription(
            double value
            )
        {
            string rDesc = string.Empty;
            for (int idx = 0; idx < base.Count; idx++)
            {
                if (base[idx].MININUM <= value && value < base[idx].MAXINUM)
                {
                    rDesc = base[idx].RANGENAME;
                    break;
                }
            }
            return rDesc;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------

    internal class Dies : List<Point>, IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            int cmp1 = x.X.CompareTo(y.X);
            int cmp2 = x.Y.CompareTo(y.Y);

            if (cmp1 == 0 && cmp2 == 0)
                return 0;
            else if (cmp1 == 0)
                return cmp2;
            else
                return cmp1;
        }
    }
    internal class DefectiveDieInfo : IComparable, ICloneable
    {
        internal String Key;
        internal Dies DieIndex;

        internal DefectiveDieInfo()
        {
            Key = null;
            DieIndex = new Dies();
        }

        public int CompareTo(object obj)
        {
            DefectiveDieInfo d = obj as DefectiveDieInfo;
            return Key.CompareTo(d.Key);
        }

        public object Clone()
        {
            DefectiveDieInfo d = new DefectiveDieInfo();
            d.Key = Key;
            d.DieIndex = DieIndex;
            return d;
        }
    }
    internal class DefectiveDies : List<DefectiveDieInfo>, IComparer<DefectiveDieInfo>
    {
        public int Compare(DefectiveDieInfo x, DefectiveDieInfo y)
        {
            return x.Key.CompareTo(y.Key);
        }

        internal int BinarySearch(
            string gubun
            )
        {
            DefectiveDieInfo dieInfo = new DefectiveDieInfo();
            dieInfo.Key = gubun;
            return base.BinarySearch(dieInfo);
        }

        internal bool Remove(
            string searchKey
            )
        {
            DefectiveDieInfo dieInfo = null;
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (String.Equals(this[idx].Key, searchKey))
                {
                    dieInfo = this[idx];
                    break;
                }
            }
            if (dieInfo != null)
                return base.Remove(dieInfo);
            else return true;
        }

        internal new bool Remove(
            DefectiveDieInfo dieInfo
            )
        {
            return base.Remove(dieInfo);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------

    internal class Node : IComparable, ICloneable
    {
        internal String Key;
        internal List<double> Values;

        internal Node()
        {
            Key = String.Empty;
            Values = new List<double>();
        }

        public int CompareTo(object obj)
        {
            Node value = obj as Node;
            return Key.CompareTo(value.Key);
        }

        public object Clone()
        {
            Node value = new Node();
            value.Key = Key;
            value.Values = Values;
            return value;
        }
    }
    internal class NodeList : List<Node>, IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return x.Key.CompareTo(y.Key);
        }

        public int BinarySearch(
            String key
            )
        {
            Node value = new Node();
            value.Key = key;
            return base.BinarySearch(value);
        }

        public bool Remove(
            string searchKey
            )
        {
            Node value = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (String.Equals(this[i].Key, searchKey))
                {
                    value = this[i];
                    break;
                }
            }

            if (value != null)
                return Remove(value);
            else
                return true;
        }
    }
    #endregion [ Inner Class ]

}
