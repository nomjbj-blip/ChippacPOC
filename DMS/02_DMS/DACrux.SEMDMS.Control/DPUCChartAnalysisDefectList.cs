using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Infragistics.Win.UltraWinEditors;
using DACrux.Base;
using System.Collections.Generic;
using DACrux.Framework.Base;
using DACrux.SEMDMS.RO;
using System.Collections;
using System.Globalization;
using DACrux.Utility;
using System.Reflection;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCChartAnalysisDefectList
        : DACrux.Framework.Base.DACruxCTLBasic01, ISendDefect, IExportExcel
    {

        #region [ Constructor ]
        private readonly string[] DATETIME_FORMAT = null;
        private static readonly string DefectClass = "CLASSNUMBER";
        private static readonly string DefectSize = "DSIZE";
        private static readonly string DefectCount = "DEFECTS";
        private DefectList SelectedDefects = null;
        private Dictionary<int, Color> dicSizeColor = null;
        private DataTable dtSizeInfo = null;
        private Dictionary<string, Color> dicClassColor = null;
        private InspectionInfoList InspectionInfos = null;

        //--

        private enum ReclassifiedNewDefectColIndex
        {
            STEP_SEQ = 0,
            DEFECTID,
            WAFER_SEQ,
            CLASSNUMBER,
            NEW_DEFECT_CLASS
        }


        public DPUCChartAnalysisDefectList(
            )
        {
            InitializeComponent();
            SelectedDefects = new DefectList();
            //DefectTypeInfo = new DefectTypeList();
            DATETIME_FORMAT = new string[] { "yyyyMMddHHmmss", "MM/dd/yy HH:mm:ss", "MM-dd-yy HH:mm:ss" };
            Gubun = new DefectGubun() { New = true, CarryOver = true, Cluster = true, Random = true };
        }

        //--
        #endregion [ Constructor ]

        //-------------------------------------------------------------------------------------

        #region [ Event Handler ]

        public void DPUCChartAnalysis_Load(
            object sender,
            EventArgs e
            )
        {
            this.ucmType.DataMember = "";
            this.ucmType.DisplayMember = "VALUE";
            this.ucmType.ValueMember = "KEY";
            this.ucmType.DataSource = GetChartType();
            this.ucmType.SelectedIndex = 0;

            //--

            this.ucmXValueMember.DataMember = "";
            this.ucmXValueMember.DisplayMember = "VALUE";
            this.ucmXValueMember.ValueMember = "KEY";
            this.ucmXValueMember.DataSource = GetChartXAxis();
            this.ucmXValueMember.SelectedIndex = ucmXValueMember.Items.Count - 1;

            //--

            DataTable dtYAxis = GetChartYAxis();
            DataRow[] drsYAxis = dtYAxis.Select(
                String.Format("[XVALUE] = '{0}'", ucmXValueMember.SelectedItem.DataValue)
                );
            if (drsYAxis != null && drsYAxis.Length > 0)
            {
                ucmYValueMember.DataMember = "";
                ucmYValueMember.DisplayMember = "VALUE";
                ucmYValueMember.ValueMember = "KEY";
                ucmYValueMember.DataSource = drsYAxis.CopyToDataTable<DataRow>();
                ucmYValueMember.SelectedIndex = 0;
            }

            //--

            //this.dpucOption1.TabsControl[1].Visible = false;
            //this.dpucOption1.TabsControl[2].Visible = false;

            //--

            //2019-07-11-정병주 : Defect Size / Defect Class 별로 X 축을 정의 한다.
            DefectMapAnalysis oDMapAnalysis = new DefectMapAnalysis();
            dicSizeColor = oDMapAnalysis.GetDefectSizeColor(GlobalVariable.UserID);
            dtSizeInfo = oDMapAnalysis.SelectColorByDefectSize(GlobalVariable.UserID);
            dicClassColor = oDMapAnalysis.GetDefectClassColor();

            //--

            // default size defect mode automatic
            DefectSizeMode = true;
            DivisionCount = dicSizeColor.Count;

            UltraComboEditor_SelectionChangeCommitted(ucmXValueMember, EventArgs.Empty);
        }

        //--

        private void btnDefectSize_Click(
            object sender,
            EventArgs e
            )
        {
            PopupSetupDialog();
        }

        //--

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            //if (GetData())
            DrawChart();
        }

        //--

        private void btnYFilter_Click(
            object sender,
            EventArgs e
            )
        {
            PopupYFilterDialog();
        }

        //--

        private void chart1_MouseClick(
            object sender,
            MouseEventArgs e
            )
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            HitTestResult result = chart1.HitTest(e.X, e.Y);

            DataPoint dp = null;

            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.LegendItem)
            {
                dp = result.Object as DataPoint;
            }
            else if (result.ChartElementType == ChartElementType.AxisLabels && result.Axis.AxisName == AxisName.X && result.Object is CustomLabel)
            {
                dp = chart1.Series[0].Points[(int)(result.Object as CustomLabel).FromPosition];
            }

            if (dp == null)
                return;

            String sXAisx = ucmXValueMember.Value as String;

            DefectList tmpDefects = new DefectList();
            dp.BackSecondaryColor = Color.Red;
            dp.BackHatchStyle = ChartHatchStyle.Percent25;
            dp.BorderWidth = 3;

            String sTag = dp.Tag as String;
            String[] sValues = sTag.Split(new char[] { ':', '>', '<', '=' }, StringSplitOptions.RemoveEmptyEntries);

            DateTime datetime;
            switch (sXAisx)
            {
                case "INSPECTED_WAFER":
                    datetime = DateTime.ParseExact(
                        String.Format("{0}:{1}:{2}", sValues[1], sValues[2], sValues[3]),
                        DATETIME_FORMAT,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None
                        );

                    foreach (InspectionInfo isp in InspectionInfos)
                    {
                        if (String.Equals(isp.WaferID, sValues[0])
                            && isp.InspectionTime == datetime
                            )
                        {
                            tmpDefects.AddRange(
                                (DataSource as DefectList).GetDefectArray(
                                    isp.StepSeq
                                    ));
                        }
                    }
                    break;
                case "INSPECTION_TIME":
                    datetime = DateTime.ParseExact(
                        String.Format("{0}:{1}:{2}", sValues[0], sValues[1], sValues[2]),
                        DATETIME_FORMAT,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None
                        );

                    foreach (InspectionInfo isp in InspectionInfos)
                    {
                        if (isp.InspectionTime == datetime)
                        {
                            tmpDefects.AddRange(
                                (DataSource as DefectList).GetDefectArray(
                                    isp.StepSeq
                                    ));
                        }
                    }
                    break;
                case "INSPECTION_EQ":
                    foreach (InspectionInfo isp in InspectionInfos)
                    {
                        if (String.Equals(isp.Inspector, sValues[0]))
                        {
                            tmpDefects.AddRange(
                                (DataSource as DefectList).GetDefectArray(
                                    isp.StepSeq
                                    ));
                        }
                    }
                    break;
                case "DSIZE":
                    foreach (Defect d in (DataSource as DefectList))
                    {
                        if (chkFixed.Checked)
                        {
                            if (d.DSIZE > DACrux.Base.Convert.doubleParse(sValues[0])
                                && d.DSIZE <= DACrux.Base.Convert.doubleParse(sValues[2])
                                )
                            {
                                tmpDefects.Add(d);
                            }
                        }
                        else
                        {
                            if (d.DSIZE >= DACrux.Base.Convert.doubleParse(sValues[0])
                                && d.DSIZE < DACrux.Base.Convert.doubleParse(sValues[2])
                                )
                            {
                                tmpDefects.Add(d);
                            }
                        }
                    }
                    break;
                case "CLASSNUMBER":
                    int iClassNum = DmsCache.Instance.ClassLookup.GetKey(sValues[0]);
                    if (iClassNum == int.MaxValue)
                        iClassNum = DACrux.Base.Convert.intParse(sValues[0]);

                    foreach (Defect d in (DataSource as DefectList))
                    {
                        if (d.CLASSNUMBER == iClassNum)
                        {
                            tmpDefects.Add(d);
                        }
                    }
                    break;
                case "WAFER_ID":
                    foreach (InspectionInfo isp in InspectionInfos)
                    {
                        if (String.Equals(isp.WaferID, sValues[0]))
                        {
                            tmpDefects.AddRange(
                                (DataSource as DefectList).GetDefectArray(
                                    isp.StepSeq
                                    ));
                        }
                    }
                    break;
                case "STEP_ID":
                default:
                    foreach (InspectionInfo isp in InspectionInfos)
                    {
                        if (String.Equals(isp.StepID, sValues[0]))
                        {
                            tmpDefects.AddRange(
                                (DataSource as DefectList).GetDefectArray(
                                    isp.StepSeq
                                    ));
                        }
                    }
                    break;
            }

            tmpDefects = Calculation_Defects(tmpDefects);

            // Shift:더하기, Ctrl:빼기
            if (System.Windows.Forms.Control.ModifierKeys == Keys.Shift)
            {
                for (int i = 0; i < tmpDefects.Count; i++)
                {
                    int idx = SelectedDefects.BinarySearch(tmpDefects[i]);
                    
                    if (idx < 0)
                        SelectedDefects.Insert(~idx, tmpDefects[i]);
                }
            }
            else if (System.Windows.Forms.Control.ModifierKeys == Keys.Control)
            {
                for (int i = 0; i < tmpDefects.Count; i++)
                {
                    int idx = SelectedDefects.BinarySearch(tmpDefects[i].STEP_SEQ, tmpDefects[i].DEFECTID);

                    if (idx >= 0)
                        SelectedDefects.RemoveAt(idx);
                }
            }
            else
            {
                SelectedDefects.Clear();

                for (int i = 0; i < tmpDefects.Count; i++)
                {
                    int idx = SelectedDefects.BinarySearch(tmpDefects[i]);

                    if (idx < 0)
                        SelectedDefects.Insert(~idx, tmpDefects[i]);
                }
            }

            dpucReclassify1.DataSource = SelectedDefects;
        }

        //--

        private void chart1_MouseMove(
            object sender,
            MouseEventArgs e
            )
        {
            Chart chart = (sender as Chart);
            if (chart == null)
                return;

            //--

            HitTestResult result = chart.HitTest(e.X, e.Y);
            if (chart.Legends.Count <= 0)
                return;

            //Control Key 를 누르고 있을 경우 여러 Point 를 선택
            //if (ModifierKeys != Keys.Shift || ModifierKeys != Keys.ShiftKey)
            //{
            //    SelectedDefects.Clear();
            //}

            // Reset Data Point Attributes
            foreach (Series sVal in chart.Series)
            {
                foreach (DataPoint point in sVal.Points)
                {
                    point.BackSecondaryColor = Color.Black;
                    point.BackHatchStyle = ChartHatchStyle.None;
                    point.BorderWidth = 1;
                }
            }

            // If a Data Point or a Legend item is selected.
            if (result.ChartElementType == ChartElementType.DataPoint ||
                result.ChartElementType == ChartElementType.LegendItem)
            {
                // Set cursor type 
                Cursor = Cursors.Hand;
                if (result.PointIndex < 0)
                    return;

                // Find selected data point
                DataPoint point = result.Series.Points[result.PointIndex];

                // Set End Gradient Color to White
                point.BackSecondaryColor = Color.White;

                // Set selected hatch style
                point.BackHatchStyle = ChartHatchStyle.Percent25;

                // Increase border width
                point.BorderWidth = 3;
            }
            else
            {
                // Set default cursor
                this.Cursor = Cursors.Default;
            }

        }

        //--

        private void chkFixed_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            UltraCheckEditor checkEditor = (sender as UltraCheckEditor);
            btnDefectSize.Enabled = !checkEditor.Checked;
            btnSearch.PerformClick();
        }

        private void chkScaleBreaks_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        //--

        private void dpucReclassify1_OnReclassifyApply(
            object sender,
            EventArgs e
            )
        {
            if (dpucReclassify1.DataSource == null || dpucReclassify1.DataSource.Count <= 0)
                return;

            DefectList defects = dpucReclassify1.DataSource as DefectList;
            string[,] Params = new string[defects.Count, 5];
            object item = dpucReclassify1.SelectedNewDefectClass;
            if (item == null) return;

            if (MessageBox.Show("Reclassify를 진행하시겠습니까?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;

            for (int idx = 0; idx < defects.Count; idx++)
            {
                Params[idx, (int)ReclassifiedNewDefectColIndex.STEP_SEQ] = defects[idx].STEP_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.DEFECTID] = defects[idx].DEFECTID.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.WAFER_SEQ] = defects[idx].WAFER_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.CLASSNUMBER] = defects[idx].CLASSNUMBER.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.NEW_DEFECT_CLASS] = item.ToString();
            }

            DefectMapAnalysis obj = new DefectMapAnalysis();
            long[] steps = new long[Wafer.Length];
            for (int idx = 0; idx < Wafer.Length; idx++)
            {
                steps[idx] = Base.Convert.longParse(Wafer[idx].StepSeq);
            }

            DataSource = obj.SetReclassifyDefectList_Comp(
                steps,
                Params,
                Base.GlobalVariable.UserID,
                Base.GlobalVariable.LocalIP
                );

            DataBinding();
        }

        private void OptionEvent_ValueChanged(
            object sender,
            EventArgs e
            )
        {
        }

        //--

        private void uchkNormalize_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            DrawChart();
        }

        //--

        private void UltraComboEditor_SelectionChangeCommitted(
            object sender,
            EventArgs e
            )
        {
            UltraComboEditor combobox = sender as UltraComboEditor;
            if (combobox == null) return;

            if (combobox == ucmXValueMember)
            {
                if (String.Equals(combobox.Value.ToString(), DefectClass))
                {
                    uchkNormalize.Enabled = true;
                    tsmSetup.Enabled = false;
                    btnDefectSize.Enabled = false;
                    btnYFilter.Enabled = false;
                    chkFixed.Enabled = false;
                }
                else if (String.Equals(combobox.Value.ToString(), DefectSize))
                {
                    uchkNormalize.Enabled = false;
                    tsmSetup.Enabled = true; ;
                    btnYFilter.Enabled = false;
                    DefectSizeMode = true;
                    chkFixed.Enabled = true;
                    btnDefectSize.Enabled = !chkFixed.Checked;
                }
                else
                {
                    uchkNormalize.Enabled = false;
                    tsmSetup.Enabled = false;
                    btnDefectSize.Enabled = false;
                    chkFixed.Enabled = false;

                    if (String.Equals(ucmYValueMember.Value.ToString(), DefectCount))
                        btnYFilter.Enabled = true;
                    else
                        btnYFilter.Enabled = false;
                }

                //--

                String sPrevYValue = ucmYValueMember.Items[ucmYValueMember.SelectedIndex].DataValue as String;
                ucmYValueMember.Items.Clear();
                DataTable dtYAxis = GetChartYAxis();
                DataRow[] drsYAxis = dtYAxis.Select(
                    String.Format("[XVALUE] = '{0}'", combobox.SelectedItem.DataValue)
                    );

                if (drsYAxis != null && drsYAxis.Length > 0)
                {
                    ucmYValueMember.DataMember = "";
                    ucmYValueMember.DisplayMember = "VALUE";
                    ucmYValueMember.ValueMember = "KEY";
                    ucmYValueMember.DataSource = drsYAxis.CopyToDataTable<DataRow>();
                    ucmYValueMember.SelectedIndex = 0;
                }

                if (!String.IsNullOrEmpty(sPrevYValue))
                {
                    for (int idx = 0; idx < ucmYValueMember.Items.Count; idx++)
                    {
                        String strNextYValue = ucmYValueMember.Items[idx].DataValue as String;
                        if (String.Equals(sPrevYValue, strNextYValue))
                        {
                            ucmYValueMember.SelectedIndex = idx;
                            break;
                        }
                    }
                }
            }

            if (combobox == ucmYValueMember)
            {
                if (String.Equals(combobox.Value.ToString(), DefectCount))
                    btnYFilter.Enabled = true;
                else
                    btnYFilter.Enabled = false;
            }

            DrawChart();
        }

        //--

        private void tsmSetup_Click(
            object sender,
            EventArgs e
            )
        {
            PopupSetupDialog();
        }

        //--

        private void tsmReset_Click(
            object sender,
            EventArgs e
            )
        {
            int idx = chart1.ChartAreas.IndexOf("CHARTAREA");
            if (idx < 0)
                return;
            chart1.ChartAreas["CHARTAREA"].AxisX.ScaleView.ZoomReset(0);
            chart1.ChartAreas["CHARTAREA"].AxisY.ScaleView.ZoomReset(0);
        }

        //--

        #endregion [ Event Handler ]

        //-------------------------------------------------------------------------------------

        #region [ Method ]

        //--

        public void Closed(
            )
        {
        }

        //--

        #region [ Defect Class 에 따른 Chart Data ]
        private Dictionary<string, double> ChartData_DefectClass(
            )
        {
            String sYAxis = ucmYValueMember.Value as String;
            Dictionary<string, object[]> dicChartData = new Dictionary<string, object[]>();

            DefectList defects = DataSource as DefectList;
            if (defects == null)
                return null;

            //--

            double dValue = double.NaN;
            /// Item1: AreaForTest
            /// Item2: Total Defect
            /// Item3: Total Defective Die
            /// Item4: Total Inspection DIe
            Tuple<double, double, double, double> inspectionSum = GetInspectionSum();
            List<int> classNumber = null;

            if (String.Equals(sYAxis, "DEFECTIVE_DIE")
                || String.Equals(sYAxis, "DEFECTIVE_DIE_PERCENTS"))
            {
                #region [ Defective Die ]
                Dictionary<string, Dictionary<Point, int>> dic = new Dictionary<string, Dictionary<Point, int>>();
                Dictionary<Point, int> dicTmp = null;
                string keyCondition = "{0}-{1}";
                classNumber = new List<int>();
                foreach (Defect d in defects)
                {
                    int idx = classNumber.BinarySearch(d.CLASSNUMBER);
                    if (idx < 0)
                        classNumber.Insert(~idx, d.CLASSNUMBER);

                    Point point = new Point(d.XINDEX, d.YINDEX);

                    if (!dic.ContainsKey(String.Format(keyCondition, d.STEP_SEQ, d.CLASSNUMBER)))
                    {
                        dicTmp = new Dictionary<Point, int>();
                        dicTmp.Add(point, 1);
                        dic.Add(String.Format(keyCondition, d.STEP_SEQ, d.CLASSNUMBER), dicTmp);
                    }
                    else
                    {
                        dicTmp = dic[String.Format(keyCondition, d.STEP_SEQ, d.CLASSNUMBER)];
                        if (!dicTmp.ContainsKey(point))
                            dicTmp.Add(point, 1);
                        else
                            dicTmp[point]++;
                        dic[String.Format(keyCondition, d.STEP_SEQ, d.CLASSNUMBER)] = dicTmp;
                    }
                }
                #endregion [ Defective Die ]
                #region [ Y 항목에 대한 계산 ]
                foreach (KeyValuePair<string, Dictionary<Point, int>> pv in dic)
                {
                    string[] keys = pv.Key.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    switch (sYAxis)
                    {
                        case "DEFECTIVE_DIE_PERCENTS":
                            if (uchkNormalize.Checked)
                                dValue = pv.Value.Count;
                            else
                                dValue = Math.Round(pv.Value.Count / inspectionSum.Item4 * 100, (int)decimalPlaces.Value);
                            break;

                        case "DEFECTIVE_DIE":
                        default:
                            dValue = pv.Value.Count;
                            break;
                    }

                    AddDicValue(
                        dicChartData,
                        keys[keys.Length - 1],
                        dValue
                        );

                }
                #endregion [ Y 항목에 대한 계산 ]
            }
            else
            {
                classNumber = new List<int>();
                foreach (Defect d in defects)
                {
                    int idx = classNumber.BinarySearch(d.CLASSNUMBER);
                    if (idx < 0)
                        classNumber.Insert(~idx, d.CLASSNUMBER);
                }

                foreach (int iclass in classNumber)
                {
                    // item1: Cluster Defect
                    // itme2: Non-Cluster Defect
                    Tuple<DefectList, DefectList> tupleDefects = Calculation_Defects(defects, iclass);

                    //--

                    switch (sYAxis)
                    {
                        case "DEFECT_DD":
                            if (!uchkNormalize.Checked)
                                dValue = Math.Round((tupleDefects.Item1.Count + tupleDefects.Item2.Count) / inspectionSum.Item1, (int)decimalPlaces.Value);
                            else
                                dValue = (tupleDefects.Item1.Count + tupleDefects.Item2.Count) / inspectionSum.Item1;
                            break;

                        case "DEFECT_PERCENTS":
                            dValue = Math.Round(((tupleDefects.Item1.Count + tupleDefects.Item2.Count) / inspectionSum.Item2) * 100, (int)decimalPlaces.Value);
                            if (uchkNormalize.Checked)
                                dValue = (tupleDefects.Item1.Count + tupleDefects.Item2.Count);
                            break;

                        case "DEFECTS":
                        default:
                            dValue = (tupleDefects.Item1.Count + tupleDefects.Item2.Count);
                            break;
                    }

                    AddDicValue(
                        dicChartData,
                        iclass.ToString(),
                        dValue
                        );
                }
            }

            if (uchkNormalize.Checked)
            {
                dicChartData = ChartData_Normalize(
                    dicChartData
                    );
            }

            return GetChartValues(
                dicChartData
                ); ;
        }

        //--

        private Dictionary<string, object[]> ChartData_Normalize(
            Dictionary<string, object[]> dicChartData
            )
        {
            String sYAxis = ucmYValueMember.Value as String;

            //--

            Dictionary<string, object[]> dicTmpChartData = new Dictionary<string, object[]>();

            List<string> lstKeys = new List<string>(dicChartData.Keys);
            /// Item1: AreaForTest
            /// Item2: Total Defect
            /// Item3: Total Defective Die
            /// Item4: Total Inspection DIe
            Tuple<double, double, double, double> inspectionSum = GetInspectionSum();

            //--

            DefectList defects = DataSource as DefectList;
            lstKeys.Remove("0"); // Uncategorized 제외
            double dReviewTotal = 0d;
            double dValue = 0d;
            for (int idx = 0; idx < lstKeys.Count; idx++)
            {
                double[] dValues = Array.ConvertAll(
                    dicChartData[lstKeys[idx]], x => DACrux.Base.Convert.doubleParse(x.ToString())
                    );
                dReviewTotal += dValues.Sum();
            }

            //--

            for (int idx = 0; idx < lstKeys.Count; idx++)
            {
                double[] dValues = Array.ConvertAll(dicChartData[lstKeys[idx]], x => DACrux.Base.Convert.doubleParse(x.ToString()));

                switch (sYAxis)
                {
                    case "DEFECT_DD":
                        dValue = Math.Round((dValues.Sum() / dReviewTotal * inspectionSum.Item2) / inspectionSum.Item1, (int)decimalPlaces.Value);
                        break;

                    case "DEFECT_PERCENTS":
                        dValue = Math.Round((dValues.Sum() / dReviewTotal * 100), (int)decimalPlaces.Value);
                        break;

                    case "DEFECTIVE_DIE":
                        dValue = Math.Round((dValues.Sum() / dReviewTotal * inspectionSum.Item3), (int)decimalPlaces.Value);
                        break;

                    case "DEFECTIVE_DIE_PERCENTS":
                        dValue = Math.Round(((dValues.Sum() / dReviewTotal * inspectionSum.Item3) / inspectionSum.Item4) * 100, (int)decimalPlaces.Value);
                        break;

                    case "DEFECTS":
                    default:
                        dValue = Math.Round(dValues.Sum() / dReviewTotal * inspectionSum.Item2, (int)decimalPlaces.Value);
                        break;
                }

                AddDicValue(
                    dicTmpChartData,
                    lstKeys[idx],
                    dValue
                    );
            }

            return dicChartData = dicTmpChartData;
        }
        #endregion [ Defect Class 에 따른 Chart Data ]

        //--

        #region [ Defect Size 에 따른 Chart Data ]
        private Dictionary<string, double> ChartData_DefectSize(
            )
        {
            String sYAxis = ucmYValueMember.Value as String;
            Dictionary<string, object[]> dicChartData = new Dictionary<string, object[]>();
            Dictionary<string, object[]> dicTmp = new Dictionary<string, object[]>();

            DefectList defects = DataSource as DefectList;
            if (defects == null)
                return null;

            List<Defect> lstDefect = null;
            List<string> lstKeys = null;
            double dMin = double.NaN;
            double dDiff = double.NaN;

            //--

            double dValue = 0d;
            /// Item1: AreaForTest
            /// Item2: Total Defect
            /// Item3: Total Defective Die
            /// Item4: Total Inspection DIe
            Tuple<double, double, double, double> inspectionSum = GetInspectionSum();

            //--

            List<double> defectSize = new List<double>();
            int idx = 0;
            foreach (Defect d in defects)
            {
                idx = defectSize.BinarySearch(d.DSIZE);
                if (idx < 0)
                    defectSize.Insert(~idx, d.DSIZE);
            }

            foreach (double size in defectSize)
            {
                lstDefect = defects.FindAll(delegate(Defect d)
                {
                    return (d.DSIZE == size);
                });

                AddDicValue(
                    dicTmp,
                    size.ToString(),
                    lstDefect.Count
                    );
            }

            lstKeys = new List<string>(dicTmp.Keys);
            if (chkFixed.Checked)
            {
                #region [ Defect Size is Fixed Mode ]
                switch (sYAxis)
                {
                    case "DEFECTIVE_DIE":
                    case "DEFECTIVE_DIE_PERCENTS":
                        foreach (DataRow r in dtSizeInfo.Rows)
                        {
                            if (!double.TryParse(r["SIZE_FROM"].ToString(), out dMin))
                                dMin = double.NaN;
                            if (!double.TryParse(r["SIZE_TO"].ToString(), out dDiff))
                                dDiff = double.NaN;

                            dValue = GetDefectiveDieCountBySize(defects, dMin, dDiff, chkFixed.Checked);
                            if (String.Equals(sYAxis, "DEFECTIVE_DIE_PERCENTS"))
                            {
                                dValue = Math.Round((dValue / inspectionSum.Item4 * 100), (int)decimalPlaces.Value);
                            }

                            AddDicValue(
                                dicChartData,
                                String.Format("{0}", dDiff),
                                dValue
                                );
                        }
                        break;

                    case "DEFECTS":
                    case "DEFECT_DD":
                    default:
                        foreach (DataRow r in dtSizeInfo.Rows)
                        {
                            if (!double.TryParse(r["SIZE_FROM"].ToString(), out dMin))
                                dMin = double.NaN;
                            if (!double.TryParse(r["SIZE_TO"].ToString(), out dDiff))
                                dDiff = double.NaN;

                            dValue = GetDefectCountBySize(dicTmp, dMin, dDiff, chkFixed.Checked);
                            if (String.Equals(sYAxis, "DEFECT_DD"))
                            {
                                dValue = dValue / inspectionSum.Item1;
                            }

                            AddDicValue(
                                dicChartData,
                                String.Format("{0}", dDiff),
                                Math.Round(dValue, (int)decimalPlaces.Value)
                                );
                        }
                        break;
                }
                #endregion [ Defect Size is Fixed Mode ]
            }
            else
            {
                #region [ Defect Size is Range Mode ]
                if (DefectSizeMode)
                {
                    /// Defect Size Mode가 Automatic 인 경우에는 Defect List의 Size의 최대, 최소 값을 계산해서 보여줌
                    DefectSizeMin = Array.ConvertAll(lstKeys.ToArray(), x => DACrux.Base.Convert.doubleParse(x)).Min();
                    DefectSizeMax = Array.ConvertAll(lstKeys.ToArray(), x => DACrux.Base.Convert.doubleParse(x)).Max();
                }
                dMin = DefectSizeMin;
                dDiff = Math.Round(((DefectSizeMax + 0.01) - DefectSizeMin) / DivisionCount, (int)decimalPlaces.Value);

                switch (sYAxis)
                {
                    case "DEFECTIVE_DIE":
                    case "DEFECTIVE_DIE_PERCENTS":
                        for (idx = 0; idx < DivisionCount; idx++)
                        {
                            dValue = GetDefectiveDieCountBySize(defects, dMin, (dMin + dDiff));
                            if (String.Equals(sYAxis, "DEFECTIVE_DIE_PERCENTS"))
                            {
                                dValue = Math.Round((dValue / inspectionSum.Item4 * 100), (int)decimalPlaces.Value);
                            }

                            AddDicValue(
                                dicChartData,
                                String.Format("{0} <= x < {1}", dMin, (dMin + dDiff)),
                                dValue
                                );

                            dMin += dDiff;
                        }
                        break;

                    case "DEFECTS":
                    case "DEFECT_DD":
                    default:
                        for (idx = 0; idx < DivisionCount; idx++)
                        {
                            dValue = GetDefectCountBySize(dicTmp, dMin, (dMin + dDiff));
                            if (String.Equals(sYAxis, "DEFECT_DD"))
                            {
                                dValue = dValue / inspectionSum.Item1;
                            }

                            AddDicValue(
                                dicChartData,
                                String.Format("{0} <= x < {1}", dMin, (dMin + dDiff)),
                                Math.Round(dValue, (int)decimalPlaces.Value)
                                );
                            dMin += dDiff;
                        }
                        break;
                }
                #endregion [ Defect Size is Range Mode ]
            }

            //--

            return GetChartValues(
                dicChartData
                );
        }

        private double GetDefectCountBySize(
            Dictionary<string, object[]> dicData,
            double minValue,
            double maxValue,
            bool isCondition = false
            )
        {
            double dValue = 0d;
            IEnumerable<KeyValuePair<string, object[]>> keyValues = null;
            if (isCondition)
            {
                keyValues = dicData
                    .Select(x => x)
                    .Where(x => DACrux.Base.Convert.doubleParse(x.Key) > minValue && DACrux.Base.Convert.doubleParse(x.Key) <= maxValue);
            }
            else
            {
                keyValues = dicData
                    .Select(x => x)
                    .Where(x => DACrux.Base.Convert.doubleParse(x.Key) >= minValue && DACrux.Base.Convert.doubleParse(x.Key) < maxValue);
            }

            foreach (KeyValuePair<string, object[]> pv in keyValues)
            {
                dValue += Array.ConvertAll(pv.Value, x => DACrux.Base.Convert.doubleParse(x.ToString())).Sum();
            }

            return dValue;
        }

        private double GetDefectiveDieCountBySize(
            DefectList defects,
            double minValue,
            double maxValue,
            bool isCondition = false
            )
        {
            List<Defect> lstDefect = null;
            if (isCondition)
            {
                lstDefect = defects.FindAll(delegate(Defect d)
                {
                    return ((d.DSIZE > minValue) && (d.DSIZE <= maxValue));
                });
            }
            else
            {
                lstDefect = defects.FindAll(delegate(Defect d)
                {
                    return ((d.DSIZE >= minValue) && (d.DSIZE < maxValue));
                });
            }

            var vDefectiveDies = lstDefect.Select(x => new { x.STEP_SEQ, x.XINDEX, x.YINDEX }).Distinct();
            return vDefectiveDies.Count();
        }



        #endregion [ Defect Size 에 따른 Chart Data ]

        //--

        #region [ Normal case 에 따른 Chart Data ]
        private Dictionary<string, double> ChartData_Other(
            )
        {
            if (InspectionInfos == null || InspectionInfos.Count <= 0)
                return null;


            String sXAxis = ucmXValueMember.Value as String;
            String sYAxis = ucmYValueMember.Value as String;
            Dictionary<string, object[]> dicChartData = new Dictionary<string, object[]>();

            //--
            double[] dAreaForTest = null;
            long[] dTotalInspectedDie = null;
            if (String.Equals(sXAxis, "STEP_ID"))
            {
                var values = InspectionInfos.Select(x => new { x.StepID })
                                            .Distinct();
                foreach (var v1 in values)
                {
                    List<InspectionInfo> insplist = InspectionInfos.FindAll(delegate(InspectionInfo id)
                    {
                        return (string.Equals(id.StepID, v1.StepID));
                    });

                    dAreaForTest = new double[insplist.Count];
                    dTotalInspectedDie = new long[insplist.Count];
                    DefectList lstDefect = new DefectList();
                    for (int idx = 0; idx < insplist.Count; idx++)
                    {
                        InspectionInfo insp = insplist[idx];
                        dAreaForTest[idx] = insp.AreaPerTest;
                        dTotalInspectedDie[idx] = insp.InspectDie;
                        lstDefect.AddRange(
                            (DataSource as DefectList).GetDefectArray(insp.StepSeq)
                            );
                    }

                    AddDicValue(
                        dicChartData,
                        String.Format("{0}", insplist[0].StepID),
                        Calculation_Defects(lstDefect, dAreaForTest.Sum(), dTotalInspectedDie.Sum())
                        );
                }
            }
            else if (String.Equals(sXAxis, "WAFER_ID"))
            {
                var values = InspectionInfos
                    .Select(x => new { x.WaferID })
                    .Distinct();
                foreach (var v1 in values)
                {
                    List<InspectionInfo> insplist = InspectionInfos.FindAll(delegate(InspectionInfo id)
                    {
                        return (String.Equals(id.WaferID, v1.WaferID));
                    });

                    dAreaForTest = new double[insplist.Count];
                    dTotalInspectedDie = new long[insplist.Count];
                    DefectList lstDefect = new DefectList();
                    for (int idx = 0; idx < insplist.Count; idx++)
                    {
                        InspectionInfo insp = insplist[idx];
                        dAreaForTest[idx] = insp.AreaPerTest;
                        dTotalInspectedDie[idx] = insp.InspectDie;
                        lstDefect.AddRange(
                            (DataSource as DefectList).GetDefectArray(insp.StepSeq)
                            );
                    }

                    AddDicValue(
                        dicChartData,
                        String.Format("{0}", insplist[0].WaferID),
                        Calculation_Defects(lstDefect, dAreaForTest.Sum(), dTotalInspectedDie.Sum())
                        );
                }
            }
            else if (String.Equals(sXAxis, "INSPECTED_WAFER"))
            {
                var values = InspectionInfos
                    .Select(x => new { x.WaferID, x.InspectionTime })
                    .Distinct();
                foreach (var v1 in values)
                {
                    List<InspectionInfo> insplist = InspectionInfos.FindAll(delegate(InspectionInfo id)
                    {
                        return (String.Equals(id.WaferID, v1.WaferID) && id.InspectionTime == v1.InspectionTime);
                    });

                    dAreaForTest = new double[insplist.Count];
                    dTotalInspectedDie = new long[insplist.Count];
                    DefectList lstDefect = new DefectList();
                    for (int idx = 0; idx < insplist.Count; idx++)
                    {
                        InspectionInfo insp = insplist[idx];
                        dAreaForTest[idx] = insp.AreaPerTest;
                        dTotalInspectedDie[idx] = insp.InspectDie;
                        lstDefect.AddRange(
                            (DataSource as DefectList).GetDefectArray(insp.StepSeq)
                            );
                    }

                    //--

                    AddDicValue(
                        dicChartData,
                        String.Format("{0}:{1}", v1.WaferID, v1.InspectionTime.ToString(DATETIME_FORMAT[1])),
                        Calculation_Defects(lstDefect, dAreaForTest.Sum(), dTotalInspectedDie.Sum())
                        );
                }
            }
            else if (String.Equals(sXAxis, "INSPECTION_TIME"))
            {
                var values = InspectionInfos.Select(x => new { x.InspectionTime }).Distinct();
                foreach (var v1 in values)
                {
                    List<InspectionInfo> insplist = InspectionInfos.FindAll(
                        x => x.InspectionTime == v1.InspectionTime
                        );

                    dAreaForTest = new double[insplist.Count];
                    dTotalInspectedDie = new long[insplist.Count];
                    DefectList lstDefect = new DefectList();
                    for (int idx = 0; idx < insplist.Count; idx++)
                    {
                        InspectionInfo insp = insplist[idx];
                        dAreaForTest[idx] = insp.AreaPerTest;
                        dTotalInspectedDie[idx] = insp.InspectDie;
                        lstDefect.AddRange(
                            (DataSource as DefectList).GetDefectArray(insp.StepSeq)
                            );
                    }

                    //--

                    AddDicValue(
                        dicChartData,
                        v1.InspectionTime.ToString(DATETIME_FORMAT[1]),
                        Calculation_Defects(lstDefect, dAreaForTest.Sum(), dTotalInspectedDie.Sum())
                        );
                }
            }
            else if (String.Equals(sXAxis, "INSPECTION_EQ"))
            {
                var values = InspectionInfos.Select(x => new { x.Inspector }).Distinct();
                foreach (var v1 in values)
                {
                    List<InspectionInfo> insplist = InspectionInfos.FindAll(
                        x => String.Equals(x.Inspector, v1.Inspector)
                        );

                    //--

                    dAreaForTest = new double[insplist.Count];
                    dTotalInspectedDie = new long[insplist.Count];
                    DefectList lstDefect = new DefectList();
                    for (int idx = 0; idx < insplist.Count; idx++)
                    {
                        InspectionInfo insp = insplist[idx];
                        dAreaForTest[idx] = insp.AreaPerTest;
                        dTotalInspectedDie[idx] = insp.InspectDie;
                        lstDefect.AddRange(
                            (DataSource as DefectList).GetDefectArray(insp.StepSeq)
                            );
                    }

                    AddDicValue(
                        dicChartData,
                        v1.Inspector,
                        Calculation_Defects(lstDefect, dAreaForTest.Sum(), dTotalInspectedDie.Sum())
                        );
                }
            }
            return GetChartValues(
                dicChartData
                );
        }
        #endregion [ Normal case 에 따른 Chart Data ]

        //--

        private void AddDicValue(
            Dictionary<string, object> dic,
            string key,
            object value
            )
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
            }
            else
            {
                dic[key] = value;
            }
        }

        private void AddDicValue(
            Dictionary<string, object[]> dic,
            string key,
            object value
            )
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, new object[] { value });
            }
            else
            {
                List<object> oValues = dic[key].ToList<object>();
                oValues.Add(value);
                dic[key] = oValues.ToArray();
            }
        }

        //--

        private DefectList Calculation_Defects(
            DefectList defectList
            )
        {
            DefectList targetList = new DefectList();
            if ((Gubun.New && !Gubun.CarryOver && !Gubun.Cluster && !Gubun.Random)
                || (Gubun.New && !Gubun.CarryOver && Gubun.Cluster && Gubun.Random))
            {
                // New Defect 에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER >= 1)
                        targetList.Add(d);
                }
            }
            else if ((Gubun.New && !Gubun.CarryOver && Gubun.Cluster && !Gubun.Random))
            {
                // New Defect 이면서 Cluster 에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER >= 1 && d.CLUSTERNUMBER >= 1)
                        targetList.Add(d);
                }
            }
            else if (Gubun.New && !Gubun.CarryOver && !Gubun.Cluster && Gubun.Random)
            {
                // New Defect 이면서 Random 에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER >= 1 && d.CLUSTERNUMBER == 0)
                        targetList.Add(d);
                }
            }
            else if ((!Gubun.New && Gubun.CarryOver && !Gubun.Cluster && !Gubun.Random)
                || (!Gubun.New && Gubun.CarryOver && Gubun.Cluster && Gubun.Random))
            {
                // CarryOver 에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER == 0)
                        targetList.Add(d);
                }
            }
            else if (!Gubun.New && Gubun.CarryOver && Gubun.Cluster && !Gubun.Random)
            {
                // Carryover 이면서 cluster에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER == 0 && d.CLUSTERNUMBER >= 1)
                        targetList.Add(d);
                }
            }
            else if (!Gubun.New && Gubun.CarryOver && !Gubun.Cluster && Gubun.Random)
            {
                // Carryover 이면서 random 에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.ADDER == 0 && d.CLUSTERNUMBER == 0)
                        targetList.Add(d);
                }
            }
            else if ((!Gubun.New && !Gubun.CarryOver && Gubun.Cluster && !Gubun.Random)
                || (Gubun.New && Gubun.CarryOver && Gubun.Cluster && !Gubun.Random))
            {
                // Cluster에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.CLUSTERNUMBER >= 1)
                        targetList.Add(d);
                }
            }
            else if ((!Gubun.New && !Gubun.CarryOver && !Gubun.Cluster && Gubun.Random)
                || (Gubun.New && Gubun.CarryOver && !Gubun.Cluster && Gubun.Random))
            {
                // Random에 해당하는 경우
                foreach (Defect d in defectList)
                {
                    if (d.CLUSTERNUMBER == 0)
                        targetList.Add(d);
                }
            }
            else
            {
                // Default case
                targetList = defectList;
            }

            return targetList;
        }

        //--

        private Tuple<DefectList, DefectList> Calculation_Defects(
            DefectList defects,
            int iclass
            )
        {
            DefectList nonClusterDefect = new DefectList();
            DefectList clusterDefect = new DefectList();
            foreach (Defect d in defects)
            {
                if (d.CLASSNUMBER != iclass)
                    continue;

                // noncluster defect 추가
                if (d.CLUSTERNUMBER == 0)
                {
                    nonClusterDefect.Add(d);
                }
                /// cluster에 대한 집계
                else if (d.CLUSTERNUMBER > 0)
                {
                    int idx = clusterDefect.FindIndex(delegate(Defect dd)
                    {
                        return dd.STEP_SEQ == d.STEP_SEQ && dd.WAFER_SEQ == d.WAFER_SEQ && dd.CLUSTERNUMBER == d.CLUSTERNUMBER;
                    });

                    if (idx < 0)
                    {
                        clusterDefect.Insert(~idx, d);
                    }
                }

            }

            return new Tuple<DefectList, DefectList>(clusterDefect, nonClusterDefect);
        }

        //--

        private double Calculation_Defects(
            DefectList defectList,
            double dAreaPerTest,
            double dTotalInspectDie
            )
        {
            String sYAxis = ucmYValueMember.Value as String;
            double dValue = double.NaN;
            switch (sYAxis)
            {
                case "DEFECTIVE_DIE":
                    var vDefects1 = defectList.Select(x => new { x.STEP_SEQ, x.XINDEX, x.YINDEX }).Distinct();
                    dValue = vDefects1.Count();
                    break;

                case "DEFECTIVE_DIE_PERCENTS":
                    var vDefects2 = defectList.Select(x => new { x.STEP_SEQ, x.XINDEX, x.YINDEX }).Distinct();
                    dValue = Math.Round(vDefects2.Count() / dTotalInspectDie * 100, (int)decimalPlaces.Value);
                    break;

                case "DEFECT_DD":
                    dValue = Math.Round(defectList.Count() / dAreaPerTest, (int)decimalPlaces.Value);
                    break;

                case "SCAN_AREA":
                    dValue = dAreaPerTest;
                    break;

                case "DEFECTS":
                default:
                    dValue = Calculation_Defects(defectList).Count;
                    break;
            }

            return dValue;
        }

        //--

        private Color GetColor(
            string key
            )
        {
            Color[] color = new Color[]{
                ColorTranslator.FromHtml("#403BD9"),
                ColorTranslator.FromHtml("#0511F2"),
                ColorTranslator.FromHtml("#565BBF"),
                ColorTranslator.FromHtml("#F2E52E"),
                ColorTranslator.FromHtml("#BFB745"),
                ColorTranslator.FromHtml("#3726A6"),
                ColorTranslator.FromHtml("#4A44F2"),
                ColorTranslator.FromHtml("#F2E635"),
                ColorTranslator.FromHtml("#F2BE22"),
                ColorTranslator.FromHtml("#F20505"),
                ColorTranslator.FromHtml("#C004D9"),
                ColorTranslator.FromHtml("#AB05F2"),
                ColorTranslator.FromHtml("#6D0FF2"),
                ColorTranslator.FromHtml("#3316F2"),
                ColorTranslator.FromHtml("#5D9ACC"),
                ColorTranslator.FromHtml("#1FBF15"),
                ColorTranslator.FromHtml("#FFDE33"),
                ColorTranslator.FromHtml("#FF7504"),
                ColorTranslator.FromHtml("#E0100E"),
                ColorTranslator.FromHtml("#F20CCC"),
                ColorTranslator.FromHtml("#0476D9"),
                ColorTranslator.FromHtml("#F2E30F"),
                ColorTranslator.FromHtml("#F2CC0C"),
                ColorTranslator.FromHtml("#F24444"),
                ColorTranslator.FromHtml("#0455BF"),
                ColorTranslator.FromHtml("#74BF04"),
                ColorTranslator.FromHtml("#F2CB05"),
                ColorTranslator.FromHtml("#F27405"),
                ColorTranslator.FromHtml("#D90404"),
                ColorTranslator.FromHtml("#E766FF"),
                ColorTranslator.FromHtml("#AD5DE8"),
                ColorTranslator.FromHtml("#9D73FF"),
                ColorTranslator.FromHtml("#615DE8"),
                ColorTranslator.FromHtml("#668AFF"),
                ColorTranslator.FromHtml("#255941"),
                ColorTranslator.FromHtml("#32734E"),
                ColorTranslator.FromHtml("#BFAE99"),
                ColorTranslator.FromHtml("#593825"),
                ColorTranslator.FromHtml("#D9886A"),
                ColorTranslator.FromHtml("#11FFBE"),
                ColorTranslator.FromHtml("#3BF500"),
                ColorTranslator.FromHtml("#FF00D7"),
                ColorTranslator.FromHtml("#C400EB"),
                ColorTranslator.FromHtml("#9B00FF")
            };

            int iKey = DACrux.Base.Convert.intParse(key);
            return color[iKey % color.Length];
        }

        //--

        private SeriesChartType ChartTypeChanged(
            )
        {
            SeriesChartType charttype = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ucmType.Value.ToString(), true);
            return charttype;
        }

        //--

        private void DrawChart(
            )
        {
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            String sXAxisVisibleLabel = String.Empty;
            String sYAxisVisibleLabel = String.Empty;

            //--

            Dictionary<string, double> dicChartData = null;
            Dictionary<string, double> dicTmpData = null;
            List<string> lstKeys = null;
            int idx = 0;

            try
            {
                StatusMessage("조회를 시작 합니다.");
                chart1.ChartAreas.Clear();
                chart1.Legends.Clear();
                chart1.Series.Clear();

                //--

                String sXAxis = ucmXValueMember.Value as String;
                String sYAxis = ucmYValueMember.Value as String;

                StatusMessage("Data 를 조회 중입니다.");
                if (String.Equals(sXAxis, DefectClass))
                {
                    #region [ Defect Class ]
                    dicChartData = ChartData_DefectClass();
                    if (dicChartData == null || dicChartData.Count <= 0)
                        return;

                    if (rbXChronological.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => DACrux.Base.Convert.intParse(x.Key))
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => DACrux.Base.Convert.intParse(x.Key))
                                .ToDictionary(x => x.Key, x => x.Value);

                        dicTmpData = new Dictionary<string, double>();
                        foreach (KeyValuePair<string, double> pv in dicChartData)
                        {
                            String sKeys = DmsCache.Instance.ClassLookup[DACrux.Base.Convert.intParse(pv.Key)];
                            dicTmpData.Add(sKeys, pv.Value);
                        }

                        dicChartData = dicTmpData;
                    }
                    else if (rbXValue.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);

                        dicTmpData = new Dictionary<string, double>();
                        foreach (KeyValuePair<string, double> pv in dicChartData)
                        {
                            String sKeys = DmsCache.Instance.ClassLookup[DACrux.Base.Convert.intParse(pv.Key)];
                            dicTmpData.Add(sKeys, pv.Value);
                        }

                        dicChartData = dicTmpData;
                    }
                    else if (rbXLabel.Checked)
                    {
                        dicTmpData = new Dictionary<string, double>();
                        foreach (KeyValuePair<string, double> pv in dicChartData)
                        {
                            String sKeys = DmsCache.Instance.ClassLookup[DACrux.Base.Convert.intParse(pv.Key)];
                            dicTmpData.Add(sKeys, pv.Value);
                        }

                        if (rbAsc.Checked)
                            dicChartData = dicTmpData.OrderBy(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicTmpData.OrderByDescending(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);
                    }
                    #endregion [ Defect Class ]
                }
                else if (String.Equals(sXAxis, DefectSize))
                {
                    #region [ Defect Size ]
                    dicChartData = ChartData_DefectSize();
                    if (dicChartData == null || dicChartData.Count <= 0)
                        return;

                    if (rbXChronological.Checked)
                    {
                        if (rbDesc.Checked)
                        {
                            dicTmpData = new Dictionary<string, double>();
                            lstKeys = new List<string>(dicChartData.Keys);
                            for (int sidx = lstKeys.Count - 1; 0 <= sidx; sidx--)
                            {
                                if (!dicTmpData.ContainsKey(lstKeys[sidx]))
                                {
                                    dicTmpData.Add(lstKeys[sidx], dicChartData[lstKeys[sidx]]);
                                }
                                else
                                {
                                    double dValue = dicTmpData[lstKeys[sidx]];
                                    dValue += dicChartData[lstKeys[sidx]];
                                    dicTmpData[lstKeys[sidx]] = dValue;
                                }
                            }
                            dicChartData = dicTmpData;
                        }
                    }
                    else if (rbXValue.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (rbXLabel.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);
                    }
                    #endregion [ Defect Size ]
                }
                else
                {
                    #region [ Other ]
                    dicChartData = ChartData_Other();
                    if (dicChartData == null || dicChartData.Count <= 0)
                        return;

                    if (rbXValue.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);
                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => x.Value)
                                .ToDictionary(x => x.Key, x => x.Value);
                    }
                    else if (rbXLabel.Checked)
                    {
                        if (rbAsc.Checked)
                            dicChartData = dicChartData.OrderBy(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);

                        if (rbDesc.Checked)
                            dicChartData = dicChartData.OrderByDescending(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        lstKeys = new List<string>(dicChartData.Keys);
                        if (rbDesc.Checked)
                        {
                            dicTmpData = new Dictionary<string, double>();
                            for (idx = lstKeys.Count - 1; idx >= 0; idx--)
                            {
                                if (!dicTmpData.ContainsKey(lstKeys[idx]))
                                {
                                    dicTmpData.Add(lstKeys[idx], dicChartData[lstKeys[idx]]);
                                }
                                else
                                {
                                    double dValue = dicTmpData[lstKeys[idx]];
                                    dValue += dicChartData[lstKeys[idx]];
                                    dicTmpData[lstKeys[idx]] = dValue;
                                }
                            }
                            dicChartData = dicTmpData;
                        }
                    }

                    #endregion [ Other ]
                }

                //--
                StatusMessage("Data를 처리 중입니다.");

                chart1.AntiAliasing = AntiAliasingStyles.All;
                chart1.DataSource = dicChartData;
                ChartArea cArea = chart1.ChartAreas.Add("CHARTAREA");

                //--
                cArea.BackColor = Color.LightGray;
                cArea.AxisX.Enabled = AxisEnabled.True;
                cArea.AxisX.MajorGrid.Enabled = false;
                cArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
                cArea.AxisX.MajorGrid.LineWidth = 1;
                cArea.AxisX.MajorTickMark.Enabled = false;
                cArea.AxisX.MajorTickMark.LineDashStyle = ChartDashStyle.Solid;
                cArea.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90;
                cArea.AxisX.LabelStyle.Angle = -90;
                cArea.AxisX.LabelStyle.Enabled = true;
                cArea.AxisX.LabelStyle.IsEndLabelVisible = false;
                cArea.AxisX.LabelStyle.IsStaggered = false;
                cArea.AxisX.Title = ucmXValueMember.Text;
                cArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
                cArea.AxisX.ScaleView.Zoomable = true;
                cArea.AxisX.ScaleView.ZoomReset();
                cArea.AxisX.ScrollBar.IsPositionedInside = true;
                cArea.AxisX.ScrollBar.LineColor = Color.Black;
                cArea.AxisX.ScrollBar.Size = 17;
                cArea.AxisX.Interval = 1;
                cArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                cArea.AxisX.IntervalOffset = 1;
                cArea.AxisX.IntervalOffsetType = DateTimeIntervalType.Auto;
                cArea.AxisX.IsLabelAutoFit = false;

                cArea.CursorX.AutoScroll = true;
                cArea.CursorX.IsUserEnabled = true;
                cArea.CursorX.IsUserSelectionEnabled = true;

                //--

                cArea.AxisY.Enabled = AxisEnabled.True;
                cArea.AxisY.MajorGrid.Enabled = false;
                cArea.AxisY.MajorTickMark.Enabled = false;
                cArea.AxisY.LabelStyle.Enabled = true;
                cArea.AxisY.Title = uchkNormalize.Checked ? String.Format("Normalize {0}", ucmYValueMember.Text) : ucmYValueMember.Text;
                cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
                cArea.AxisY.ScaleView.Zoomable = true;
                cArea.AxisY.ScaleView.ZoomReset();
                cArea.AxisX.ScrollBar.IsPositionedInside = true;
                cArea.AxisY.ScrollBar.LineColor = Color.Black;
                cArea.AxisY.ScrollBar.Size = 17;
                //cArea.AxisY.Interval = 1;
                //cArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
                //cArea.AxisY.IntervalOffset = 0.01;
                cArea.AxisY.IntervalOffsetType = DateTimeIntervalType.Auto;
                cArea.AxisY.IsLabelAutoFit = false;
                cArea.CursorY.AutoScroll = true;
                cArea.CursorY.IsUserEnabled = true;
                cArea.CursorY.IsUserSelectionEnabled = true;

                cArea.AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
                cArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
                cArea.AxisY.ScaleBreakStyle.Spacing = 2;
                cArea.AxisY.ScaleBreakStyle.LineWidth = 2;
                cArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
                cArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
                cArea.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;

                //--

                cArea.BorderWidth = 1;
                cArea.ShadowOffset = 0;

                Legend cLegend = chart1.Legends.Add("LEGEND");
                cLegend.Enabled = false;
                Series cSeries = chart1.Series.Add("SERIES");
                cSeries.ChartType = ChartTypeChanged();
                cSeries.MarkerSize = 2;
                cSeries.MarkerStyle = MarkerStyle.Circle;
                cSeries.BorderWidth = 2;
                idx = 0;
                foreach (KeyValuePair<string, double> pv in dicChartData)
                {
                    idx = cSeries.Points.AddXY(pv.Key, pv.Value);
                    DataPoint dp = cSeries.Points[idx];
                    dp.IsValueShownAsLabel = true;
                    dp.Tag = pv.Key;

                    //Defect Size 의 경우 Point 별로 색상을 정의 해준다.
                    if (String.Equals(sXAxis, DefectSize)
                        && dicSizeColor != null && dicSizeColor.Count > 0)
                    {
                        dp.Color = dicSizeColor[idx + 1];
                    }

                    if (String.Equals(sXAxis, DefectClass)
                        && dicClassColor != null && dicClassColor.Count > 0)
                    {
                        if (dicClassColor.ContainsKey(pv.Key))
                            dp.Color = dicClassColor[pv.Key];
                        else dp.Color = GetColor(pv.Key);
                    }
                }

                cSeries.SmartLabelStyle.Enabled = true;
                cSeries.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                cSeries.SmartLabelStyle.CalloutBackColor = Color.Black;
                cSeries.SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                cSeries.SmartLabelStyle.CalloutLineWidth = 1;
                cSeries.SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;

                chart1.AntiAliasing = AntiAliasingStyles.All;
            }
            finally
            {
                StatusMessage(null);
#if DEBUG
                Debug.WriteLine(String.Format("ElapsedMilliseconds: {0}", sw.ElapsedMilliseconds));
#endif

            }
        }

        //--

        public void DataBinding(
            )
        {
            DefectList defects = DataSource as DefectList;
            if (defects == null)
                return;

            dpucInformation1.Wafer = Wafer;
            dpucInformation1.DataSource = defects;
            dpucReclassify1.DataSource = null;
            InspectionInfos = dpucInformation1.InspectionInfos;

            //DefectMapAnalysis oDefectMapAnal = new DefectMapAnalysis();
            //DataTable dt = oDefectMapAnal.GetDefectClassInfo();

            DrawChart();
        }

        //--

        private Dictionary<string, double> GetChartValues(
            Dictionary<string, object[]> dicChartData
            )
        {
            Dictionary<string, double> dicResult = new Dictionary<string, double>();
            foreach (KeyValuePair<string, object[]> pv in dicChartData)
            {
                if (!dicResult.ContainsKey(pv.Key))
                {
                    dicResult.Add(pv.Key, Array.ConvertAll(pv.Value, x => DACrux.Base.Convert.doubleParse(x.ToString())).Sum());
                }
                else
                {
                    double dValue = dicResult[pv.Key];
                    dValue += Array.ConvertAll(pv.Value, x => DACrux.Base.Convert.doubleParse(x.ToString())).Sum();
                    dicResult[pv.Key] = dValue;
                }
            }

            return dicResult;
        }

        //--

        private Tuple<double, double, double, double> GetInspectionSum()
        {
            double dAreaForTest = 0d;
            double dTotalDefects = 0d;
            double dTotalDefectiveDie = 0d;
            double dTotalInspectDie = 0d;

            foreach (InspectionInfo inspInfo in InspectionInfos)
            {
                dAreaForTest += inspInfo.AreaPerTest;
                dTotalDefects += inspInfo.Defects;
                dTotalDefectiveDie += inspInfo.DefectiveDie;
                dTotalInspectDie += inspInfo.InspectDie;
            }

            return new Tuple<double, double, double, double>(dAreaForTest, dTotalDefects, dTotalDefectiveDie, dTotalInspectDie);
        }

        //--

        public Defect[] GetSelectedDefect(
            )
        {
            if (SelectedDefects == null || SelectedDefects.Count <= 0)
                return (DataSource as DefectList).ToArray<Defect>();
            else
                return SelectedDefects.ToArray<Defect>();
        }

        //--

        #region [ Export Excel ]

        private DataTable ExportChartData(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("X-AXIS", typeof(string)), new DataColumn("Y-AXIS", typeof(double)) });

            Dictionary<string, double> dic = chart1.DataSource as Dictionary<string, double>;
            if (dic == null || dic.Count <= 0)
                return null;

            foreach (KeyValuePair<string, double> pv in dic)
            {
                DataRow row = dt.NewRow();
                row["X-AXIS"] = pv.Key;
                row["Y-AXIS"] = pv.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }

        public void ExportExcel(
            )
        {
            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.Add(chart1);
            sheet1.Add(ExportChartData());

            ExcelSheet sheet2 = new ExcelSheet();
            sheet2.Add(ExportTable());

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);
            e.SheetList.Add(sheet2);

            ExcelExportManager.Export(e);
        }

        //--

        private DataTable ExportTable(
            )
        {
            if (DataSource is DefectList)
                return (DataSource as DefectList).ToDataTable();
            else
                return null;
        }

        #endregion [ Export Excel ]

        //--

        #region [ Datatable is chart option ]

        //--

        private DataTable GetChartType(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = "COLUMN";
            row["VALUE"] = "Bar";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "LINE";
            row["VALUE"] = "Line";
            dt.Rows.Add(row);

            return dt;
        }

        //--

        private DataTable GetChartXAxis(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = "STEP_ID";
            row["VALUE"] = "Step ID";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "WAFER_ID";
            row["VALUE"] = "Wafer ID";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "INSPECTED_WAFER";
            row["VALUE"] = "Inspected Wafer";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "INSPECTION_TIME";
            row["VALUE"] = "Inspected Time";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "INSPECTION_EQ";
            row["VALUE"] = "Inspector";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "DSIZE";
            row["VALUE"] = "Defect Size";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "CLASSNUMBER";
            row["VALUE"] = "Defect Class";
            dt.Rows.Add(row);

            //--

            return dt;
        }

        //--

        private DataTable GetChartYAxis(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("XVALUE", typeof(String)),
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["XVALUE"] = "STEP_ID";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "STEP_ID";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "STEP_ID";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "STEP_ID";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "STEP_ID";
            row["KEY"] = "SCAN_AREA";
            row["VALUE"] = "Inspection Area";
            dt.Rows.Add(row);

            //---

            row = dt.NewRow();
            row["XVALUE"] = "WAFER_ID";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "WAFER_ID";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "WAFER_ID";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "WAFER_ID";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "WAFER_ID";
            row["KEY"] = "SCAN_AREA";
            row["VALUE"] = "Inspection Area";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTED_WAFER";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTED_WAFER";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTED_WAFER";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTED_WAFER";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTED_WAFER";
            row["KEY"] = "SCAN_AREA";
            row["VALUE"] = "Inspection Area";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_TIME";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_TIME";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_TIME";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_TIME";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_TIME";
            row["KEY"] = "SCAN_AREA";
            row["VALUE"] = "Inspection Area";
            dt.Rows.Add(row);


            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_EQ";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_EQ";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_EQ";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "INSPECTION_EQ";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);


            //--

            row = dt.NewRow();
            row["XVALUE"] = "DSIZE";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "DSIZE";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            //row = dt.NewRow();
            //row["XVALUE"] = "DSIZE";
            //row["KEY"] = "DEFECT_PERCENTS";
            //row["VALUE"] = "Defect Percents";
            //dt.Rows.Add(row);


            //--

            row = dt.NewRow();
            row["XVALUE"] = "DSIZE";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "DSIZE";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "CLASSNUMBER";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "CLASSNUMBER";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "CLASSNUMBER";
            row["KEY"] = "DEFECT_PERCENTS";
            row["VALUE"] = "Defect Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "CLASSNUMBER";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "CLASSNUMBER";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "";
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "";
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "";
            row["KEY"] = "DEFECT_PERCENTS";
            row["VALUE"] = "Defect Percentage";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "";
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["XVALUE"] = "";
            row["KEY"] = "DEFECTIVE_DIE_PERCENTS";
            row["VALUE"] = "Defective Die Percentage";
            dt.Rows.Add(row);

            return dt;
        }

        //--

        #endregion [ Datatable is chart option ]

        //--

        public void SetData(
            DACrux.Base.DPWafer[] wafer
            )
        {
            this.Wafer = wafer;
        }

        //--

        private void PopupSetupDialog(
            )
        {
            frmDefectOption dlg = new frmDefectOption(
                DefectSizeMax,
                DefectSizeMin,
                DefectSizeMode, // auto(true) or manual(false)
                DivisionCount,
                dicSizeColor.Count
                );

            //--

            dlg.StartPosition = FormStartPosition.CenterScreen;
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            //--

            this.DefectSizeMode = dlg.DefectSizeMode;
            this.DefectSizeMax = dlg.DefectSizeMax;
            this.DefectSizeMin = dlg.DefectSizeMin;
            this.DivisionCount = dlg.DivisionCount;

            btnSearch.PerformClick();
        }

        //--

        private void PopupYFilterDialog(
            )
        {
            frmChartYOption dlg = new frmChartYOption(
                Gubun
                );
            dlg.StartPosition = FormStartPosition.CenterScreen;
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            Gubun = dlg.Gubun;
            btnSearch.PerformClick();
        }

        #endregion [ Method ]

        //-------------------------------------------------------------------------------------

        #region [ Property ]

        //--

        public DACrux.Base.DPWafer[] Wafer
        {
            get;
            private set;
        }

        //--

        public object DataSource
        {
            get;
            set;
        }

        //--

        public bool DefectSizeMode
        {
            get;
            private set;
        }

        //--

        public double DefectSizeMax
        {
            get;
            private set;
        }

        //--

        public double DefectSizeMin
        {
            get;
            private set;
        }

        //--

        public int DivisionCount
        {
            get;
            private set;
        }

        //--

        public DefectGubun Gubun
        {
            get;
            set;
        }

        public bool IsReclassifiedPermesion
        {
            get { return tabControl1.TabPages["RECLASSIFY"].Visible; }
            set { tabControl1.TabPages["RECLASSIFY"].Visible = value; }
        }

        #endregion [ Property ]

        //-------------------------------------------------------------------------------------

    }
}
