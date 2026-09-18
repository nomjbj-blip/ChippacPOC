using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using Infragistics.Win.UltraWinTabControl;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCOption : UserControl
    {
        public event EventHandler OnItemApply;

        //---------------------------------------------------------------

        public DPUCOption(
            )
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------

        #region [ Event Handler ]

        //--

        private void DPUCOption_Load(
            object sender,
            EventArgs e
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsDisplayClass);
            Utility.FPSpreadUtil.InitSpread(fpsDefects);
        }

        //--

        private void btnApply_Click(
            object sender,
            EventArgs e
            )
        {
            if (OnItemApply != null)
                OnItemApply(
                    this,
                    EventArgs.Empty
                    );
        }

        //--

        private void rdbClassCodeOrName_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            DataTable dt = DataSource_DefectClass as DataTable;
            if (dt == null || dt.Rows.Count <= 0) return;

            SetNewDefectClass(dt);
        }

        //--

        #endregion [ Event Handler ]

        //---------------------------------------------------------------

        #region [ Method ]

        //--

        internal void DataBinding_DefectClass()
        {
            DataTable dt = DataSource_DefectClass as DataTable;
            if (dt == null || dt.Rows.Count <= 0) return;

            Utility.FPSpreadUtil.InitSpread(
                fpsDisplayClass
                );
            Utility.FPSpreadUtil.SetSpreadData(
                dt,
                fpsDisplayClass_Sheet,
                100,
                0
                );
            Utility.FPSpreadUtil.VisibleSpreadColumns(
                fpsDisplayClass_Sheet,
                new string[] { "GROUP_ID", "DELETE_FLAG", "CREATE_TIME", "CREATE_USER", "UPDATE_TIME", "UPDATE_USER", "EDITMODE" },
                false
                );

            SetNewDefectClass(
                dt
                );
        }

        //--

        internal void DataBinding_Defects(
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsDefects);
            DefectList defects = DataSource_Defects as DefectList;
            if (defects == null)
                return;

            //--

            DataTable dt = DataSource_DefectClass as DataTable;
            if (dt == null || dt.Rows.Count <= 0)
                return;

            //--

            DataRow[] drs = null;

            fpsDefects_Sheet.ColumnCount = 4;
            fpsDefects_Sheet.RowCount = defects.Count;

            fpsDefects_Sheet.Columns[0].Label = "Defect ID";
            fpsDefects_Sheet.Columns[0].Width = 60;
            fpsDefects_Sheet.Columns[1].Label = "Defect Class";
            fpsDefects_Sheet.Columns[1].Width = 100;
            fpsDefects_Sheet.Columns[2].Label = "Step ID";
            fpsDefects_Sheet.Columns[2].Width = 75;
            fpsDefects_Sheet.Columns[3].Label = "Device ID";
            fpsDefects_Sheet.Columns[3].Width = 75;

            //--

            for (int rowidx = 0; rowidx < fpsDefects_Sheet.Rows.Count; rowidx++)
            {
                fpsDefects_Sheet.Cells[rowidx, 0].Value = defects[rowidx].DEFECTID;

                drs = dt.Select(String.Format("[CLASSNUMBER] = '{0}'", defects[rowidx].CLASSNUMBER));
                if (drs == null || drs.Length <= 0)
                    fpsDefects_Sheet.Cells[rowidx, 1].Value = defects[rowidx].CLASSNUMBER;
                else
                    fpsDefects_Sheet.Cells[rowidx, 1].Value = drs[0]["NAME"];

                DmsWaferDieInfo dieinfo = DmsCache.Instance[defects[rowidx].STEP_SEQ];
                fpsDefects_Sheet.Cells[rowidx, 2].Value = dieinfo.StepInfo.StepID;
                fpsDefects_Sheet.Cells[rowidx, 3].Value = dieinfo.StepInfo.DeviceID;
            }
        }

        //--

        #region [ Setting Inspection Information ]

        internal void SetInformation(
            long[] steps
            )
        {
            DefectMapAnalysis oDefectMapAnal = new DefectMapAnalysis();
            DataTable dt = oDefectMapAnal.GetStepInfo(steps);
            Inspections = new InspectionInfoList();
            foreach (DataRow dr in dt.Rows)
            {
                DateTime date = System.Convert.ToDateTime(
                    dr["RESULTTIMESTAMP"].ToString()
                    );

                Inspections.Add(
                    new InspectionInfo(
                        DACrux.Base.Convert.longParse(dr["STEP_SEQ"].ToString()),
                        dr["LOT_ID"].ToString(),
                        DACrux.Base.Convert.longParse(dr["WAFER_SEQ"].ToString()),
                        dr["WAFER_ID"].ToString(),
                        dr["SLOT_ID"].ToString(),
                        date,
                        dr["INSPECTION_EQ"].ToString(),
                        dr["STEP_ID"].ToString(),
                        DACrux.Base.Convert.longParse(dr["DEFECTS"].ToString()),
                        DACrux.Base.Convert.longParse(dr["CLASSIFIED_DEFECTS"].ToString()),
                        DACrux.Base.Convert.longParse(dr["DEFECTIVE_DIE"].ToString()),
                        DACrux.Base.Convert.longParse(dr["INSPECTED_DIE"].ToString()),
                        DACrux.Base.Convert.doubleParse(dr["DEFECT_DD"].ToString()),
                        DACrux.Base.Convert.doubleParse(dr["SCAN_AREA"].ToString())
                        ));
            }

            //--

            Information = new OptionInformation();
            Information.Lots = Inspections.Select(x => x.LotID).Distinct().Count();
            Information.Wafers = Inspections.Select(x => x.WaferID).Distinct().Count();
            Information.Inspections = Inspections.Select(x => x.StepID).Distinct().Count();
            Information.Insepcted_Wafer = Inspections.Select(x => new { x.StepID, x.WaferSeq }).Distinct().Count();
            Information.Steps = Inspections.Select(x => x.StepID).Distinct().Count();
            Information.Inspectors = Inspections.Select(x => x.Inspector).Distinct().Count();

            IEnumerable<String> IesScanTools = Inspections.Select(x => x.Inspector).Distinct();
            Information.Scan_Tools = String.Format("{0}", String.Join(", ", IesScanTools));

            Information.Defects = Inspections.Select(x => x.Defects).Sum();
            Information.Inspected_Area = Inspections.Select(x => x.AreaPerTest).Sum();
            DateTime datetime = Inspections.Select(x => x.InspectionTime).Min();
            Information.Inspection_DateTime_From = datetime.ToString("MM/dd/yy HH:mm:ss");
            datetime = Inspections.Select(x => x.InspectionTime).Max();
            Information.Inspection_DateTime_To = datetime.ToString("MM/dd/yy HH:mm:ss");
            Information.Total_Classified_Defects = Inspections.Select(x => x.ClassifiedDefects).Sum();
            Information.Total_Inspected_Dies = Inspections.Select(x => x.InspectDie).Sum();
            Information.Total_Defective_Dies = Inspections.Select(x => x.DefectiveDie).Sum();

            //--

            Dictionary<string, double[]> dicSumByLotsDefects = new Dictionary<string, double[]>();
            Dictionary<string, double[]> dicSumByLotsDefectiveDies = new Dictionary<string, double[]>();
            Dictionary<string, double[]> dicSumByLotsInspectedDies = new Dictionary<string, double[]>();
            Dictionary<string, double[]> dicSumByWafersDefects = new Dictionary<string, double[]>();
            Dictionary<string, double[]> dicSumByInspectWafersDefects = new Dictionary<string, double[]>();

            foreach (InspectionInfo p in Inspections)
            {
                if (dicSumByLotsDefects.ContainsKey(p.LotID))
                {
                    List<double> dValues = dicSumByLotsDefects[p.LotID].ToList<double>();
                    dValues.Add(p.Defects);
                    dicSumByLotsDefects[p.LotID] = dValues.ToArray<double>();
                }
                else
                {
                    dicSumByLotsDefects.Add(p.LotID, new double[] { p.Defects });
                }

                //--

                if (dicSumByLotsDefectiveDies.ContainsKey(p.LotID))
                {
                    List<double> dValues = dicSumByLotsDefectiveDies[p.LotID].ToList<double>();
                    dValues.Add(p.DefectiveDie);
                    dicSumByLotsDefectiveDies[p.LotID] = dValues.ToArray<double>();
                }
                else
                {
                    dicSumByLotsDefectiveDies.Add(p.LotID, new double[] { p.DefectiveDie });
                }

                //--

                if (dicSumByLotsInspectedDies.ContainsKey(p.LotID))
                {
                    List<double> dValues = dicSumByLotsInspectedDies[p.LotID].ToList<double>();
                    dValues.Add(p.InspectDie);
                    dicSumByLotsInspectedDies[p.LotID] = dValues.ToArray<double>();
                }
                else
                {
                    dicSumByLotsInspectedDies.Add(p.LotID, new double[] { p.InspectDie });
                }

                //--

                if (dicSumByWafersDefects.ContainsKey(p.WaferID))
                {
                    List<double> dValues = dicSumByWafersDefects[p.WaferID].ToList<double>();
                    dValues.Add(p.Defects);
                    dicSumByWafersDefects[p.WaferID] = dValues.ToArray<double>();
                }
                else
                {
                    dicSumByWafersDefects.Add(p.WaferID, new double[] { p.Defects });
                }

                //--

                string key = string.Format("{0}:{1}", p.WaferID, p.InspectionTime);
                if (dicSumByInspectWafersDefects.ContainsKey(key))
                {
                    List<double> dValues = dicSumByInspectWafersDefects[key].ToList<double>();
                    dValues.Add(p.Defects);
                    dicSumByInspectWafersDefects[key] = dValues.ToArray<double>();
                }
                else
                {
                    dicSumByInspectWafersDefects.Add(key, new double[] { p.Defects });
                }
            }

            //--

            List<string> lstLotsKeys = new List<string>(dicSumByLotsDefects.Keys);
            double[] dValue = new double[lstLotsKeys.Count];
            for (int idx = 0; idx < lstLotsKeys.Count; idx++)
            {
                dValue[idx] = dicSumByLotsDefects[lstLotsKeys[idx]].Sum();
            }
            Information.Lot_Average_Defects = dValue.Average();

            //--

            List<string> lstWafersKeys = new List<string>(dicSumByWafersDefects.Keys);
            dValue = new double[lstWafersKeys.Count];
            for (int idx = 0; idx < lstWafersKeys.Count; idx++)
            {
                dValue[idx] = dicSumByWafersDefects[lstWafersKeys[idx]].Sum();
            }
            Information.Wafer_Average_Defects = dValue.Average();

            //--

            List<string> lstInspWaferKeys = new List<string>(dicSumByInspectWafersDefects.Keys);
            dValue = new double[lstInspWaferKeys.Count];
            for (int idx = 0; idx < lstInspWaferKeys.Count; idx++)
            {
                dValue[idx] = dicSumByInspectWafersDefects[lstInspWaferKeys[idx]].Sum();
            }
            Information.Inspected_Wafer_Average_Defects = dValue.Average();

            //--

            lstLotsKeys = new List<string>(dicSumByLotsDefectiveDies.Keys);
            double[] dDefectiveDies = new double[lstLotsKeys.Count];
            double[] dInspectedDies = new double[lstLotsKeys.Count];
            dValue = new double[lstLotsKeys.Count];
            for (int idx = 0; idx < lstLotsKeys.Count; idx++)
            {
                dDefectiveDies[idx] = dicSumByLotsDefectiveDies[lstLotsKeys[idx]].Sum();
                dInspectedDies[idx] = dicSumByLotsInspectedDies[lstLotsKeys[idx]].Sum();
                dValue[idx] = dDefectiveDies[idx] / dInspectedDies[idx] * 100;
            }
            Information.Average_Defective_Die_Percentages = Math.Round(dValue.Average(), 5);

            //--

            double dTotalDefectiveDie = DACrux.Base.Convert.doubleParse(Information.Total_Defective_Dies.ToString());
            double dTotalInspectedDie = DACrux.Base.Convert.doubleParse(Information.Total_Inspected_Dies.ToString());
            Information.Total_Defective_Die_Percentage = Math.Round(dTotalDefectiveDie / dTotalInspectedDie * 100, 5);
            Information.Total_Defect_Density = Inspections.Select(x => x.DefectDensity).Sum();

            StringBuilder sValue = new StringBuilder();
            sValue.AppendLine(String.Format("Lots: {0}", Information.Lots));
            sValue.AppendLine(String.Format("Wafers With Defects: {0}", Information.Wafers));
            sValue.AppendLine(String.Format("Inspections: {0}", Information.Inspections));
            sValue.AppendLine(String.Format("Inspected Wafers: {0}", Information.Insepcted_Wafer));
            sValue.AppendLine(String.Format("Inspectors: {0}", Information.Inspectors));
            sValue.AppendLine(String.Format("Steps: {0}", Information.Steps));
            sValue.AppendLine(String.Format("Scan Tools: {0}", Information.Scan_Tools));
            sValue.AppendLine(String.Format("Defects: {0}", Information.Defects));
            sValue.AppendLine(String.Format("Inspected Area(SqCm): {0}", Information.Inspected_Area));
            sValue.AppendLine(String.Format("Inspection Time From: {0}", Information.Inspection_DateTime_From));
            sValue.AppendLine(String.Format("Inspection Time To: {0}", Information.Inspection_DateTime_To));
            sValue.AppendLine(String.Format("Total Classified Defects: {0}", Information.Total_Classified_Defects));
            sValue.AppendLine(String.Format("Total Inspected Dies: {0}", Information.Total_Inspected_Dies));
            sValue.AppendLine(String.Format("Total Defective Dies: {0}", Information.Total_Defective_Dies));
            sValue.AppendLine(String.Format("Lot Average Defects: {0}", Information.Lot_Average_Defects));
            sValue.AppendLine(String.Format("Wafer Average Defects: {0}", Information.Wafer_Average_Defects));
            sValue.AppendLine(String.Format("Inspected Wafer Average Defects: {0}", Information.Inspected_Wafer_Average_Defects));
            sValue.AppendLine(String.Format("Average Defective Die Percentage: {0}", Information.Average_Defective_Die_Percentages));
            sValue.AppendLine(String.Format("Total Defective Die Percentage: {0}", Information.Total_Defective_Die_Percentage));
            sValue.AppendLine(String.Format("Total Defect Density: {0}", Information.Total_Defect_Density));

            //--

            txtInfor.Text = sValue.ToString();
        }

        #endregion  [ Setting Inspection Information ]

        //--

        private void SetNewDefectClass(
            DataTable dt
            )
        {
            uceClassNew.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string displayValue = string.Empty;
                if (rdbClassCode.Checked)
                    displayValue = String.Format("{0}-{1}", dr["CLASSNUMBER"], dr["NAME"]);
                if (rdbClassName.Checked)
                    displayValue = String.Format("{0}-{1}", dr["NAME"], dr["CLASSNUMBER"]);

                uceClassNew.Items.Add(
                    new Infragistics.Win.ValueListItem(dr["CLASSNUMBER"].ToString(), displayValue
                    ));
            }
        }

        #endregion [ Method ]

        //---------------------------------------------------------------

        #region [ Property ]

        //--

        public object DataSource_DefectClass
        {
            get;
            set;
        }

        //--

        public String DataMember_DefectClass
        {
            get;
            set;
        }

        //--

        public String DisplayMember_DefectClass
        {
            get;
            set;
        }

        //--

        public String ValueMember_DefectClass
        {
            get;
            set;
        }

        //--

        public object DataSource_Defects
        {
            get;
            set;
        }

        //--

        public Infragistics.Win.ValueListItem Selected_DefectClass
        {
            get { return uceClassNew.SelectedItem; }
            private set { uceClassNew.SelectedItem = value; }
        }

        //--

        public OptionInformation Information
        {
            get;
            private set;
        }

        //--

        internal InspectionInfoList Inspections
        {
            get;
            private set;
        }

        //--

        public UltraTabsCollection TabsControl
        {
            get
            {
                return this.ultraTabControl.Tabs;
            }
        }

        //--

        #endregion [ Property ]

        //---------------------------------------------------------------
    }

    //---------------------------------------------------------------

    #region [ Class Information ]

    //---------------------------------------------------------------

    public class OptionInformation
    {
        [Description("Lot IDs")]
        public object Lots { get; set; }

        [Description("Wafers with Defects")]
        public object Wafers { get; set; }

        [Description("Inspections")]
        public object Inspections { get; set; }

        [Description("Inspected Wafers")]
        public object Insepcted_Wafer { get; set; }

        [Description("Inspectors")]
        public object Inspectors { get; set; }

        [Description("Steps")]
        public object Steps { get; set; }

        [Description("Scan Tools")]
        public object Scan_Tools { get; set; }

        [Description("Defects")]
        public object Defects { get; set; }

        [Description("Inspected Area(SqCm)")]
        public object Inspected_Area { get; set; }

        [Description("Inspection Time From")]
        public object Inspection_DateTime_From { get; set; }

        [Description("Inspection Time To")]
        public object Inspection_DateTime_To { get; set; }

        [Description("Total Classified Defects")]
        public object Total_Classified_Defects { get; set; }

        [Description("Total Inspected Dies")]
        public object Total_Inspected_Dies { get; set; }

        [Description("Total Defective Dies")]
        public object Total_Defective_Dies { get; set; }

        [Description("Lot Average Defects")]
        public object Lot_Average_Defects { get; set; }

        [Description("Wafer Average Defects")]
        public object Wafer_Average_Defects { get; set; }

        [Description("Inspected Wafer Average Defects")]
        public object Inspected_Wafer_Average_Defects { get; set; }

        [Description("Average Defective Die Percentage")]
        public object Average_Defective_Die_Percentages { get; set; }

        [Description("Total Defective Die Percentage")]
        public object Total_Defective_Die_Percentage { get; set; }

        [Description("Total Defect Density")]
        public object Total_Defect_Density { get; set; }
    }

    //---------------------------------------------------------------

    #endregion [ Class Information ]

    //---------------------------------------------------------------

}
