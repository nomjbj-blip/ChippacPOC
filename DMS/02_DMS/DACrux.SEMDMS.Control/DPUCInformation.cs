using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Map;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCInformation
        : UserControl
    {
        #region [ Data Field ]
        Dictionary<string, object> descriptionInfo = null;
        private object _dataSource = null;
        #endregion [ Data Field]

        #region [ Constructor ]

        public DPUCInformation(
            )
        {
            InitializeComponent();
            descriptionInfo = new Dictionary<string, object>();
        }

        #endregion [ Constructor ]

        #region [ Method ]

        private void BindingDefect(Defect defect)
        {
            if (defect == null)
                return;

            descriptionInfo.Clear();
            descriptionInfo.Add("Defect ID", defect.DEFECTID);
            descriptionInfo.Add("Cluster ID", defect.CLUSTERNUMBER);
            descriptionInfo.Add("Defect Size in um", defect.DSIZE);
            descriptionInfo.Add("Defect Area in Sq um", defect.DEFECTAREA);
            descriptionInfo.Add("Class Number", defect.CLASSNUMBER);
            descriptionInfo.Add("Class Name", DmsCache.Instance.ClassLookup[defect.CLASSNUMBER]);
            descriptionInfo.Add("Location in Die[X, Y] in um", String.Format("{0}, {1}", defect.XREL, defect.YREL));
            descriptionInfo.Add("Location in Wafer[X, Y] in um", String.Format("{0}, {1}", defect.X, defect.Y));
            descriptionInfo.Add("Defect X-Size in um", defect.XSIZE);
            descriptionInfo.Add("Defect Y-Size in um", defect.YSIZE);

            Point index = new Point()
            {
                X = defect.XINDEX - DmsCache.Instance[defect.STEP_SEQ].StepInfo.DieOriginX,
                Y = defect.YINDEX - DmsCache.Instance[defect.STEP_SEQ].StepInfo.DieOriginY
            };
            descriptionInfo.Add("Die Location", String.Format("{0}, {1}", index.X, index.Y));

            descriptionInfo.Add("Lot ID", DmsCache.Instance[defect.STEP_SEQ].StepInfo.LotID);
            descriptionInfo.Add("Wafer ID", DmsCache.Instance[defect.STEP_SEQ].StepInfo.WaferID);
            descriptionInfo.Add("Scribe ID", String.Format("@{0}", DmsCache.Instance[defect.STEP_SEQ].StepInfo.Slot));
            descriptionInfo.Add("Step ID", DmsCache.Instance[defect.STEP_SEQ].StepInfo.StepID);
            descriptionInfo.Add("Inspection Time", DmsCache.Instance[defect.STEP_SEQ].StepInfo.ResultTimestamp);
            descriptionInfo.Add("Setup ID", DmsCache.Instance[defect.STEP_SEQ].StepInfo.SetupID);
            descriptionInfo.Add("Setup Time", DmsCache.Instance[defect.STEP_SEQ].StepInfo.SetupTimestamp);
            descriptionInfo.Add("Device", DmsCache.Instance[defect.STEP_SEQ].StepInfo.DeviceID);
            descriptionInfo.Add("Slot ID", DmsCache.Instance[defect.STEP_SEQ].StepInfo.Slot);
            descriptionInfo.Add("Wafer Size", DmsCache.Instance[defect.STEP_SEQ].StepInfo.WaferSize);

            double diePitchX = DmsCache.Instance[defect.STEP_SEQ].StepInfo.DiePitchX;
            double diePitchY = DmsCache.Instance[defect.STEP_SEQ].StepInfo.DiePitchY;

            descriptionInfo.Add("Die Size [X]", String.Format("{0} ({1} cm)", Math.Round(diePitchX, 1), Math.Round(diePitchX / 10000, 3)));
            descriptionInfo.Add("Die Size [Y]", String.Format("{0} ({1} cm)", Math.Round(diePitchY, 1), Math.Round(diePitchY / 10000, 3)));
            descriptionInfo.Add("Defect Die Index X", index.X);
            descriptionInfo.Add("Defect Die Index Y", index.Y);
        }

        private void BindingDefectList(Defect[] defects)
        {
            descriptionInfo.Clear();
            descriptionInfo.Add("Defect Count", defects.Length);
            descriptionInfo.Add("New Defect Count", defects.Sum(x => x.ADDER));
            descriptionInfo.Add("Carryover Defect Count", defects.Length - defects.Sum(x => x.ADDER));
            descriptionInfo.Add("Cluster Defect Count", defects.Count(x => x.CLUSTERNUMBER > 0));
            descriptionInfo.Add("Total Defect Area", defects.Sum(x => x.DEFECTAREA));
            descriptionInfo.Add("Average Defect Area", Math.Round(defects.Average(x => x.DEFECTAREA), 3));
            descriptionInfo.Add("Average X Size", Math.Round(defects.Average(x => x.XSIZE), 3));
            descriptionInfo.Add("Average Y Size", Math.Round(defects.Average(x => x.YSIZE), 3));
            descriptionInfo.Add("Average Size", Math.Round(defects.Average(x => x.XSIZE), 3));
        }

        private void BindingDefectList(DefectList defects)
        {
            long[] steps = null;

            if (Wafer == null || Wafer.Length == 0)
            {
                steps = defects.GetStepSeqArray();
            }
            else
            {
                steps = new long[Wafer.Length];

                for (int i = 0; i < Wafer.Length; i++)
                    steps[i] = Base.Convert.longParse(Wafer[i].StepSeq);
            }

            GetInspectionData(steps);
            descriptionInfo.Clear();
            descriptionInfo.Add("Lots", InspectionInfos.Lots());
            descriptionInfo.Add("Wafers", InspectionInfos.Wafers());
            descriptionInfo.Add("Inspections", InspectionInfos.Inspections());
            descriptionInfo.Add("Insepcted Wafer", InspectionInfos.InspectedWafers());
            descriptionInfo.Add("Insepctors", InspectionInfos.Insepctors());
            descriptionInfo.Add("Steps", InspectionInfos.Steps());
            descriptionInfo.Add("Scan Tools", InspectionInfos.ScanTools());
            descriptionInfo.Add("Defects", InspectionInfos.SumByDefects());
            descriptionInfo.Add("Inspection Area(SqCm)", InspectionInfos.SumByScanArea());
            Tuple<DateTime, DateTime> inspectTime = InspectionInfos.InspectionFromToTime();
            descriptionInfo.Add("Inspection Time From", inspectTime.Item1);
            descriptionInfo.Add("Inspection Time To", inspectTime.Item2);
            descriptionInfo.Add("Total Classified Defects", InspectionInfos.TotalClassifiedDefects());
            double totalInspectedDies = InspectionInfos.TotalInspectedDies();
            double totalDefectiveDies = InspectionInfos.TotalDefectiveDies();
            descriptionInfo.Add("Total Inspected Dies", totalInspectedDies);
            descriptionInfo.Add("Total Defective Dies", totalDefectiveDies);
            descriptionInfo.Add("Lot Average Defects", InspectionInfos.LotAvgDefects());
            descriptionInfo.Add("Wafer Average Defects", InspectionInfos.WaferAvgDefects());
            descriptionInfo.Add("Inspected Wafer Average Defects", InspectionInfos.InspWaferAvgDefects());
            double avgDefectiveDiePercentage = (double)Math.Round(InspectionInfos.AvgDefectiveDiePercentage(), 5);
            descriptionInfo.Add("Average Defective Die Percentage", avgDefectiveDiePercentage);
            double defectiveDiePercentage = (double)Math.Round(totalDefectiveDies / totalInspectedDies * 100, 5);
            descriptionInfo.Add("Total Defective Die Percentage", defectiveDiePercentage);
            descriptionInfo.Add("Total Defect Density", InspectionInfos.TotalDefectDensity());
            descriptionInfo.Add(String.Empty, String.Empty);
        }

        private void BindingDefectMap(DefectMap map)
        {
            if (map == null)
                return;

            descriptionInfo.Clear();

            if (map.Defects.Count > 0)
                BindingDefectList(map.Defects);

            BindingRecipe(map.GetWaferRecipe());

            descriptionInfo.Add("Defect Count", map.Defects.Count);
        }

        private void BindingWaferMap(WaferMap map)
        {
            if (map == null)
                return;

            descriptionInfo.Clear();
            BindingRecipe(map.GetWaferRecipe());
        }

        private void BindingRecipe(
            WaferRecipe recipe
            )
        {
            descriptionInfo.Add("First Die Index", String.Format("{0}, {1}", recipe.FIRST_DIE_X, recipe.FIRST_DIE_Y));
            descriptionInfo.Add("Die Size", String.Format("{0}, {1}", recipe.DIE_SIZE_X, recipe.DIE_SIZE_Y));
            descriptionInfo.Add("Origin Die", String.Format("{0}, {1}", recipe.ORIGIN_DIE_X, recipe.ORIGIN_DIE_Y));
            descriptionInfo.Add("Origin", String.Format("{0}, {1}", recipe.ORIGIN_X, recipe.ORIGIN_Y));
            descriptionInfo.Add("Die Index(Min: X, Y), (Max: X, Y)", String.Format("({0}, {1}), ({2}, {3})", recipe.DIE_INDEX_MIN_X, recipe.DIE_INDEX_MIN_Y, recipe.DIE_INDEX_MAX_X, recipe.DIE_INDEX_MAX_Y));
            descriptionInfo.Add("Notch Type", recipe.NOTCH_TYPE);
            descriptionInfo.Add("Angle", recipe.ANGLE);
            descriptionInfo.Add("Direction", recipe.XYDIR);
            descriptionInfo.Add("Wafer Size", recipe.WAFER_SIZE);
        }

        private void FillText(
            )
        {
            StringBuilder sText = new StringBuilder();
            foreach (KeyValuePair<string, object> pv in descriptionInfo)
            {
                if (!String.IsNullOrEmpty(pv.Key))
                    sText.AppendLine(String.Format("{0}: {1}", pv.Key, pv.Value));
                else
                    sText.AppendLine();
            }
            txtInformation.Text = sText.ToString();
        }

        private void GetInspectionData(
            long[] steps
            )
        {
            DefectMapAnalysis oDefectMapAnal = new DefectMapAnalysis();
            DataTable dt = oDefectMapAnal.GetStepInfo(steps);
            InspectionInfos = new InspectionInfoList();
            foreach (DataRow dr in dt.Rows)
            {
                DateTime date = System.Convert.ToDateTime(
                    dr["RESULTTIMESTAMP"].ToString()
                    );

                InspectionInfos.Add(
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
        }

        #endregion [ Method ]

        #region [ Property ]

        internal object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;

                if (_dataSource == null)
                {
                    descriptionInfo.Clear();
                }
                else if (value is Defect)
                {
                    BindingDefect(value as Defect);
                }
                else if (value is DefectList)
                {
                    BindingDefectList(value as DefectList);
                }
                else if (value is Defect[])
                {
                    var arr = value as Defect[];

                    if (arr.Length == 1)
                        BindingDefect(arr[0]);
                    else if (arr.Length > 1)
                        BindingDefectList(arr);
                }
                else if (value is DefectMap)
                {
                    BindingDefectMap(value as DefectMap);
                }
                else if (value is WaferMap)
                {
                    BindingWaferMap(value as WaferMap);
                }

                FillText();
            }
        }

        internal InspectionInfoList InspectionInfos
        {
            get;
            private set;
        }

        internal Base.DPWafer[] Wafer
        {
            get;
            set;
        }

        #endregion [ Property ]
    }

    //---------------------------------------------------------------

    internal class InspectionInfo
    {
        #region [ Constructor ]

        //--

        internal InspectionInfo()
        {
            StepSeq = long.MinValue;
            LotID = String.Empty;
            WaferSeq = long.MinValue;
            WaferID = String.Empty;
            SlotID = String.Empty;
            InspectionTime = DateTime.MinValue;
            Inspector = String.Empty;
            StepID = String.Empty;
            Defects = long.MinValue;
            ClassifiedDefects = long.MinValue;
            DefectiveDie = long.MinValue;
            InspectDie = long.MinValue;
            DefectDensity = double.NaN;
            AreaPerTest = double.NaN;
        }

        internal InspectionInfo(
            long stepseq,
            String lot,
            long waferseq,
            String wafer,
            String slot,
            DateTime date,
            String inspector,
            String step,
            long defects,
            long classifieddefects,
            long defectivedie,
            long inspectdie,
            double defectdensity,
            double areapertest
            )
        {
            StepSeq = stepseq;
            LotID = lot;
            WaferSeq = waferseq;
            WaferID = wafer;
            SlotID = slot;
            InspectionTime = date;
            Inspector = inspector;
            StepID = step;
            Defects = defects;
            ClassifiedDefects = classifieddefects;
            DefectiveDie = defectivedie;
            InspectDie = inspectdie;
            DefectDensity = defectdensity;
            AreaPerTest = areapertest;
        }

        //--

        #endregion [ Constructor ]

        //--

        #region [ Properties ]
        internal long StepSeq { get; private set; }
        internal String LotID { get; private set; }
        internal long WaferSeq { get; private set; }
        internal String WaferID { get; private set; }
        internal String SlotID { get; private set; }
        internal DateTime InspectionTime { get; private set; }
        internal String Inspector { get; private set; }
        internal String StepID { get; private set; }
        internal long Defects { get; private set; }
        internal long ClassifiedDefects { get; private set; }
        internal long DefectiveDie { get; private set; }
        internal long InspectDie { get; private set; }
        internal double DefectDensity { get; private set; }
        internal double AreaPerTest { get; private set; }
        #endregion [ Properties ]
    }

    //---------------------------------------------------------------

    internal class InspectionInfoList
        : List<InspectionInfo>
    {
        #region [ Method ]

        //--

        internal InspectionInfo GetInspectionInfo(
            long stepseq
            )
        {
            InspectionInfo info = null;
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (this[idx].StepSeq == stepseq)
                {
                    info = this[idx];
                    break;
                }
            }

            return info;
        }

        //--

        internal InspectionInfoList GetWaferID(
            long waferseq
            )
        {
            InspectionInfoList list = new InspectionInfoList();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (this[idx].WaferSeq == waferseq)
                    list.Add(this[idx]);
            }

            return list;
        }

        //--

        internal String GetStepID(
            long stepseq
            )
        {
            InspectionInfo info = null;
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (this[idx].StepSeq == stepseq)
                {
                    info = this[idx];
                    break;
                }
            }

            return (info == null) ? String.Empty : info.StepID;
        }

        //--

        internal int Lots(
            )
        {
            List<string> tmp = new List<string>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].LotID) < 0)
                    tmp.Add(this[idx].LotID);
            }

            return tmp.Count;
        }

        //--

        internal int Wafers(
            )
        {
            List<string> tmp = new List<string>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].WaferID) < 0)
                    tmp.Add(this[idx].WaferID);
            }

            return tmp.Count;
        }

        //--

        internal int Inspections(
            )
        {
            List<DateTime> tmp = new List<DateTime>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].InspectionTime) < 0)
                    tmp.Add(this[idx].InspectionTime);
            }

            return tmp.Count;
        }

        //--

        internal int InspectedWafers(
            )
        {
            return this.Count;
        }

        //--

        internal object Insepctors(
            )
        {
            List<string> tmp = new List<string>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].Inspector) < 0)
                    tmp.Add(this[idx].Inspector);
            }

            return tmp.Count;
        }

        //--

        internal int Steps(
            )
        {
            List<string> tmp = new List<string>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].StepID) < 0)
                    tmp.Add(this[idx].StepID);
            }

            return tmp.Count;
        }

        //--

        internal string ScanTools(
            )
        {
            List<string> tmp = new List<string>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.IndexOf(this[idx].Inspector) < 0)
                    tmp.Add(this[idx].Inspector);
            }

            return String.Format("{0}", String.Join(", ", tmp));
        }

        //--

        internal long SumByDefects(
            )
        {
            long rValue = 0;
            for (int idx = 0; idx < this.Count; idx++)
            {
                rValue += this[idx].Defects;
            }

            return rValue;
        }

        //--

        internal double SumByScanArea(
            )
        {
            double rValue = 0d;
            for (int idx = 0; idx < this.Count; idx++)
            {
                rValue += this[idx].AreaPerTest;
            }

            return rValue;
        }

        //--

        internal Tuple<DateTime, DateTime> InspectionFromToTime(
            )
        {
            if (this.Count <= 0)
                return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);

            List<DateTime> tmp = new List<DateTime>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                tmp.Add(this[idx].InspectionTime);
            }
            tmp.Sort();

            return new Tuple<DateTime, DateTime>(tmp[0], tmp[tmp.Count - 1]);
        }

        //--

        internal long TotalClassifiedDefects(
            )
        {
            long lSum = 0;
            for (int idx = 0; idx < this.Count; idx++)
            {
                lSum += this[idx].ClassifiedDefects;
            }

            return lSum;
        }

        //--

        internal long TotalInspectedDies(
            )
        {
            long lSum = 0;
            for (int idx = 0; idx < this.Count; idx++)
            {
                lSum += this[idx].InspectDie;
            }

            return lSum;
        }

        //--

        internal long TotalDefectiveDies(
            )
        {
            long lSum = 0;
            for (int idx = 0; idx < this.Count; idx++)
            {
                lSum += this[idx].DefectiveDie;
            }

            return lSum;
        }

        //--

        internal double LotAvgDefects(
            )
        {
            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.ContainsKey(this[idx].LotID))
                {
                    List<double> dTmp = tmp[this[idx].LotID].ToList<double>();
                    dTmp.Add(this[idx].Defects);
                    tmp[this[idx].LotID] = dTmp.ToArray<double>();
                }
                else
                {
                    tmp.Add(this[idx].LotID, new double[] { this[idx].Defects });
                }
            }

            List<string> tmpKeys = new List<string>(tmp.Keys);
            double[] dValue = new double[tmpKeys.Count];
            for (int idx = 0; idx < tmpKeys.Count; idx++)
            {
                dValue[idx] = tmp[tmpKeys[idx]].Sum();
            }

            return dValue.Average();
        }

        //--

        internal double WaferAvgDefects(
            )
        {
            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
            for (int idx = 0; idx < this.Count; idx++)
            {
                if (tmp.ContainsKey(this[idx].WaferID))
                {
                    List<double> dTmp = tmp[this[idx].WaferID].ToList<double>();
                    dTmp.Add(this[idx].Defects);
                    tmp[this[idx].WaferID] = dTmp.ToArray<double>();
                }
                else
                {
                    tmp.Add(this[idx].WaferID, new double[] { this[idx].Defects });
                }
            }

            List<string> tmpKeys = new List<string>(tmp.Keys);
            double[] dValue = new double[tmpKeys.Count];
            for (int idx = 0; idx < tmpKeys.Count; idx++)
            {
                dValue[idx] = tmp[tmpKeys[idx]].Sum();
            }

            return dValue.Average();
        }

        //--

        internal double InspWaferAvgDefects(
            )
        {
            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
            string key = string.Empty;
            for (int idx = 0; idx < this.Count; idx++)
            {
                key = string.Format("{0}:{1}", this[idx].WaferID, this[idx].InspectionTime);
                if (tmp.ContainsKey(key))
                {
                    List<double> dTmp = tmp[key].ToList<double>();
                    dTmp.Add(this[idx].Defects);
                    tmp[key] = dTmp.ToArray<double>();
                }
                else
                {
                    tmp.Add(key, new double[] { this[idx].Defects });
                }
            }

            List<string> tmpKeys = new List<string>(tmp.Keys);
            double[] dValue = new double[tmpKeys.Count];
            for (int idx = 0; idx < tmpKeys.Count; idx++)
            {
                dValue[idx] = tmp[tmpKeys[idx]].Sum();
            }

            return dValue.Average();
        }

        //--

        internal double AvgDefectiveDiePercentage(
            )
        {
            Dictionary<string, double[]> tmp1 = new Dictionary<string, double[]>();
            Dictionary<string, double[]> tmp2 = new Dictionary<string, double[]>();
            string key = string.Empty;
            for (int idx = 0; idx < this.Count; idx++)
            {
                key = this[idx].LotID;
                if (tmp1.ContainsKey(key))
                {
                    List<double> dTmp = tmp1[key].ToList<double>();
                    dTmp.Add(this[idx].DefectiveDie);
                    tmp1[key] = dTmp.ToArray<double>();
                }
                else
                {
                    tmp1.Add(key, new double[] { this[idx].DefectiveDie });
                }
            }

            for (int idx = 0; idx < this.Count; idx++)
            {
                key = this[idx].LotID;
                if (tmp2.ContainsKey(key))
                {
                    List<double> dTmp = tmp2[key].ToList<double>();
                    dTmp.Add(this[idx].InspectDie);
                    tmp2[key] = dTmp.ToArray<double>();
                }
                else
                {
                    tmp2.Add(key, new double[] { this[idx].InspectDie });
                }
            }

            List<string> tmpKeys = new List<string>(tmp1.Keys);
            double[] defectiveDies = new double[tmpKeys.Count];
            double[] inspectedDies = new double[tmpKeys.Count];
            double[] dValue = new double[tmpKeys.Count];
            for (int idx = 0; idx < tmpKeys.Count; idx++)
            {
                defectiveDies[idx] = tmp1[tmpKeys[idx]].Sum();
                inspectedDies[idx] = tmp2[tmpKeys[idx]].Sum();
                dValue[idx] = defectiveDies[idx] / inspectedDies[idx] * 100;
            }

            return dValue.Average();
        }

        //--

        internal double TotalDefectDensity(
            )
        {
            double rValue = 0;
            for (int idx = 0; idx < this.Count; idx++)
            {
                rValue += this[idx].DefectDensity;
            }

            return rValue;
        }

        //--

        #endregion [ Method ]
    }
}
