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

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCChartAnalysis : DACrux.Framework.Base.DACruxCTLBasic01
    {
        private DataSet data = null;
        private enum Func { COUNT = 0, SUM, AVG, MIN, MAX, MEDIAN, STDDEV }
        private readonly String DefectClass = "CLASSNUMBER";
        private readonly String DefectSize = "DSIZE";

        //--

        public DPUCChartAnalysis(
            )
        {
            InitializeComponent();
        }

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
            this.ucmXValueMember.SelectedIndex = 0;

            //--

            this.ucmYValueMember.DataMember = "";
            this.ucmYValueMember.DisplayMember = "VALUE";
            this.ucmYValueMember.ValueMember = "KEY";
            this.ucmYValueMember.DataSource = GetChartYAxis();
            this.ucmYValueMember.SelectedIndex = 0;

            //--

            this.dpucOption1.TabsControl[1].Visible = false;

            //--
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

        private void chart1_MouseClick(
            object sender,
            MouseEventArgs e
            )
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint
                && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DataPoint dp = result.Object as DataPoint;
                //usBar.Panels["POINT"].Text = String.Format("Value: {0}", dp.YValues[0]);
            }

            if (result.ChartElementType == ChartElementType.DataPoint
                && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
            }
        }

        //--

        private void OptionEvent_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            //UltraOptionSet uos = sender as UltraOptionSet;

            //if (uos == this.uosXLabel)
            //    OptionXLabel();

            //if (uos == this.uosSortOption)
            //    OptionSort();
            if (data != null)
                DrawChart();
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
                uchkNormalize.Checked = false;
                if (String.Equals(combobox.Value.ToString(), "CLASSNUMBER"))
                    uchkNormalize.Enabled = true;
                else
                    uchkNormalize.Enabled = false;
            }

            if (data != null)
                DrawChart();
        }

        //--

        private void tsmReset_Click(
            object sender,
            EventArgs e
            )
        {
            chart1.ChartAreas["CHARTAREA"].AxisX.ScaleView.ZoomReset(0);
            chart1.ChartAreas["CHARTAREA"].AxisY.ScaleView.ZoomReset(0);
        }

        #endregion [ Event Handler ]

        //-------------------------------------------------------------------------------------

        #region [ Method ]

        public void Closed(
            )
        {
            if (data != null)
                data.Dispose();
        }

        //--

        private DataTable ChartData(
            String sXAxis,
            String sYAxis,
            bool bNormalize,
            out String sXAxisVisibleLabel,
            out String sYAxisVisibleLabel
            )
        {
            DataTable dt = null;
            if (String.Equals(sXAxis, DefectClass))
            {
                dt = ChartData_DefectClass(
                    sXAxis,
                    sYAxis,
                    bNormalize
                    );

                if (dt == null || dt.Rows.Count <= 0)
                {
                    sXAxisVisibleLabel = String.Empty;
                    sYAxisVisibleLabel = String.Empty;
                    return null;
                }

                if (rbXLabel.Checked)
                    dt.DefaultView.Sort = String.Format("[LABEL] {0}", rbAsc.Checked ? "ASC" : "DESC");
                else
                    dt.DefaultView.Sort = String.Format("[{0}] {1}", sXAxis, rbAsc.Checked ? "ASC" : "DESC");

                sXAxisVisibleLabel = "LABEL";
                sYAxisVisibleLabel = bNormalize ? String.Format("NORMALIZE_{0}", sYAxis) : sYAxis;
            }
            else if (String.Equals(sXAxis, DefectSize))
            {
                dt = ChartData_DefectSize(
                    sXAxis,
                    sYAxis
                    );


                if (rbXLabel.Checked)
                    dt.DefaultView.Sort = String.Format("[{0}] {1}", sXAxis, rbAsc.Checked ? "ASC" : rbDesc.Checked ? "DESC" : "ASC");
                else
                    dt.DefaultView.Sort = String.Format("[INDEX] {0}", rbAsc.Checked ? "ASC" : rbDesc.Checked ? "DESC" : "ASC");

                sXAxisVisibleLabel = sXAxis;
                sYAxisVisibleLabel = sYAxis;
            }
            else
            {
                dt = ChartData_Other(
                    sXAxis,
                    sYAxis
                    );

                sXAxisVisibleLabel = sXAxis;
                sYAxisVisibleLabel = sYAxis;
            }

            dt = dt.DefaultView.ToTable();
            return dt;
        }

        //--

        private DataTable ChartData_DefectClass(
            String sXAxis,
            String sYAxis,
            bool bNormalize
            )
        {
            DataTable dt = null;
            DataTable dtResult = null;
            DataRow[] drs = null;

            double dAreaForTest = double.NaN;
            double dInspectionDie = double.NaN;
            double dTotalDefects = double.NaN;
            double dTotalDefectiveDies = double.NaN;

            dt = data.Tables["DEFECT_INFO"];
            if (dt == null || dt.Rows.Count <= 0) return dt;

            dtResult = dt.DefaultView.ToTable(true, sXAxis);
            dtResult.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("LABEL", typeof(String)),
                    new DataColumn("DEFECTS", typeof(double)),
                    new DataColumn("DEFECT_DD", typeof(double)),
                    new DataColumn("DEFECT_PERCENT", typeof(double)),
                    new DataColumn("DEFECTIVE_DIE", typeof(double)),
                    new DataColumn("DEFECTIVE_DIE_PERCENT", typeof(double)),
                });

            dAreaForTest = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "SCAN_AREA",
                String.Empty
                );

            dInspectionDie = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "INSPECTED_DIE",
                String.Empty
                );

            dTotalDefects = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "DEFECTS",
                String.Empty
                );

            dTotalDefectiveDies = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "DEFECTIVE_DIE",
                String.Empty
                );

            //-- calculation for Defect count, density, percents 

            foreach (DataRow r in dtResult.Rows)
            {
                drs = data.Tables["DEFECT_TYPE_INFO"].Select(String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis]));
                r["LABEL"] = drs.Length > 0 ? drs[0]["NAME"] : r[sXAxis];
                drs = data.Tables["DEFECT_INFO"].Select(String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis]));
                r["DEFECTS"] = drs.Count();
                r["DEFECT_DD"] = (double)(drs.Count() / (dAreaForTest / 100000000000000));
                r["DEFECT_PERCENT"] = (double)((drs.Count() / dTotalDefects) * 100);
            }

            //-- calculation for defective die count, defective die percents 

            dt = data.Tables["DEFECT_INFO"].DefaultView.ToTable(true, sXAxis, "XINDEX", "YINDEX");
            foreach (DataRow r in dtResult.Rows)
            {
                drs = dt.Select(String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis]));
                r["DEFECTIVE_DIE"] = drs.Count();
                r["DEFECTIVE_DIE_PERCENT"] = (double)((drs.Count() / dInspectionDie) * 100);
            }

            if (bNormalize)
            {
                dtResult = ChartData_Normalize(
                    dtResult,
                    sXAxis,
                    sYAxis,
                    dAreaForTest,
                    dInspectionDie,
                    dTotalDefects,
                    dTotalDefectiveDies
                    );
            }

            if (dt != null && dt.Rows.Count > 0)
                dt.AcceptChanges();

            return dtResult;
        }

        private DataTable ChartData_Normalize(
            DataTable dt,
            String sXAxis,
            String sYAxis,
            double dAreaForTest,
            double dInspectionDie,
            double dTotalDefects,
            double dTotalDefectiveDies
            )
        {
            double dExcludeZeroClassForDefects = double.NaN;
            double dExcludeZeroClassForDefectiveDie = double.NaN;
            double dTmp = double.NaN;
            dt.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("NORMALIZE_DEFECTS", typeof(double)),
                    new DataColumn("NORMALIZE_DEFECT_DD", typeof(double)),
                    new DataColumn("NORMALIZE_DEFECT_PERCENT", typeof(double)),
                    new DataColumn("NORMALIZE_DEFECTIVE_DIE", typeof(double)),
                    new DataColumn("NORMALIZE_DEFECTIVE_DIE_PERCENT", typeof(double)),
                });

            DataRow[] drs = dt.Select(String.Format("[{0}] <> '0'", sXAxis));
            if (drs.Length <= 0)
                return null;

            dt = drs.CopyToDataTable<DataRow>();
            dExcludeZeroClassForDefects = GetValue(
                dt,
                Func.SUM,
                "DEFECTS",
                String.Empty
                );

            dExcludeZeroClassForDefectiveDie = GetValue(
                dt,
                Func.SUM,
                "DEFECTIVE_DIE",
                String.Empty
                );


            foreach (DataRow r in dt.Rows)
            {
                if (!double.TryParse(r["DEFECTS"].ToString(), out dTmp))
                    dTmp = double.NaN;

                //-- Normalize Defect Count 
                r["NORMALIZE_DEFECTS"] = double.IsNaN(dTmp) ? double.NaN : (double)((dTmp / dExcludeZeroClassForDefects) * dTotalDefects);

                //-- Normalize Defect Density
                r["NORMALIZE_DEFECT_DD"] = double.IsNaN(dTmp) ? double.NaN : (double)(((dTmp / dExcludeZeroClassForDefects) * dTotalDefects) / (dAreaForTest / 100000000000000));

                //-- Normailze Defect Percents
                r["NORMALIZE_DEFECT_PERCENT"] = double.IsNaN(dTmp) ? double.NaN : (double)((dTmp / dExcludeZeroClassForDefects) * 100);

                if (!double.TryParse(r["DEFECTIVE_DIE"].ToString(), out dTmp))
                    dTmp = double.NaN;
                //-- Normalize Defective Die Count
                r["NORMALIZE_DEFECTIVE_DIE"] = double.IsNaN(dTmp) ? double.NaN : (double)((dTmp / dExcludeZeroClassForDefectiveDie) * dTotalDefectiveDies);

                //-- Normailze Defective Die Percents
                r["NORMALIZE_DEFECTIVE_DIE_PERCENT"] = double.IsNaN(dTmp) ? double.NaN : (double)((dTmp / dExcludeZeroClassForDefectiveDie) * 100);

            }
            return dt;
        }

        private DataTable ChartData_DefectSize(
            String sXAxis,
            String sYAxis
            )
        {
            DataTable dt = null;
            DataTable dtResult = null;
            DataRow[] drs = null;

            double dAreaForTest = double.NaN;
            double dInspectionDie = double.NaN;
            double dTotalDefects = double.NaN;

            dt = data.Tables["DEFECT_INFO"];
            if (dt == null || dt.Rows.Count <= 0) return dt;

            dtResult = dt.DefaultView.ToTable(true, sXAxis);
            dtResult.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("DEFECTS", typeof(double)),
                    new DataColumn("DEFECT_DD", typeof(double)),
                    new DataColumn("DEFECT_PERCENT", typeof(double)),
                    new DataColumn("DEFECTIVE_DIE", typeof(double)),
                    new DataColumn("DEFECTIVE_DIE_PERCENT", typeof(double)),
                });

            dAreaForTest = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "SCAN_AREA",
                String.Empty
                );

            dTotalDefects = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "DEFECTS",
                String.Empty
                );

            dInspectionDie = GetValue(
                "INSPECTION_INFO",
                Func.SUM,
                "INSPECTED_DIE",
                String.Empty
                );

            // calculation for defect count, defect density, defect percents 
            foreach (DataRow r in dtResult.Rows)
            {
                drs = data.Tables["DEFECT_INFO"].Select(String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis]));
                r["DEFECTS"] = drs.Count();
                r["DEFECT_DD"] = (double)(drs.Count() / (dAreaForTest / 100000000000000));
                r["DEFECT_PERCENT"] = (double)((drs.Count() / dTotalDefects) * 100);
            }

            // calculation for defective die count, defective die percents 
            dt = data.Tables["DEFECT_INFO"].DefaultView.ToTable(true, sXAxis, "XINDEX", "YINDEX");
            foreach (DataRow r in dtResult.Rows)
            {
                drs = dt.Select(String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis]));
                r["DEFECTIVE_DIE"] = drs.Count();
                r["DEFECTIVE_DIE_PERCENT"] = (double)((drs.Count() / dInspectionDie) * 100);
            }

            double dMin = (double)GetValue(
                dt,
                Func.MIN,
                sXAxis,
                String.Empty
                );
            double dMax = (double)GetValue(
                dt,
                Func.MAX,
                sXAxis,
                String.Empty
                );
            double dDiff = ((dMax + 1) - dMin) / 10;

            dt = CreateDSizeTable(
                sXAxis,
                sYAxis
                );

            for (int idx = 0; idx < 10; idx++)
            {
                DataRow r = dt.NewRow();
                r["INDEX"] = idx;
                r[sXAxis] = String.Format("{0} <= x < {1}", (double)Math.Round((dMin / 1000), 3), (double)Math.Round(((dMin + dDiff) / 1000), 3));
                r[sYAxis] = (double)Math.Round(GetValue(
                    dtResult,
                    Func.SUM,
                    sYAxis,
                    String.Format("[{0}] >= '{1}' AND [{0}] < '{2}'", sXAxis, dMin, (dMin + dDiff))
                    ), 3);
                dt.Rows.Add(r);
                dMin += dDiff;
            }

            //--

            if (dt != null && dt.Rows.Count > 0)
                dt.AcceptChanges();

            return dt;
        }

        //--

        private DataTable CreateDSizeTable(
            String sXAxis,
            String sYAxis
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("INDEX", typeof(int)),
                    new DataColumn(sXAxis, typeof(String)), 
                    new DataColumn(sYAxis, typeof(double)) 
                });
            return dt;
        }

        //--

        private DataTable ChartData_Other(
            String sXAxis,
            String sYAxis
            )
        {
            DataTable dt = null;
            double denominator = double.NaN;
            double numerator = double.NaN;
            int INDEX = 0;

            //--

            if (String.Equals(sXAxis, "INSPECTION_TIME"))
            {
                #region [ inspection time ]
                dt = data.Tables["INSPECTION_INFO"].DefaultView.ToTable(true, "RESULTTIMESTAMP");
                dt.Columns.Add(new DataColumn(sXAxis, typeof(string)));
                dt.Columns.Add(new DataColumn("INDEX", typeof(int)));
                dt.Columns.Add(new DataColumn(sYAxis, typeof(double)));
                foreach (DataRow r in dt.Rows)
                {
                    DateTime date = System.Convert.ToDateTime(r["RESULTTIMESTAMP"].ToString());
                    r["INSPECTION_TIME"] = date.ToString("yyyy-MM-dd HH:mm:ss");

                    if (String.Equals(sYAxis, "DEFECT_PERCENT"))
                    {
                        denominator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "INSPECTED_DIE",
                            String.Format("[RESULTTIMESTAMP] = '{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"))
                            );
                        numerator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "DEFECTS",
                            String.Format("[RESULTTIMESTAMP] = '{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"))
                            );

                        r[sYAxis] = (double)(numerator / denominator * 100);
                    }
                    else if (String.Equals(sYAxis, "DEFECTIVE_DIE_PERCENT"))
                    {
                        denominator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "INSPECTED_DIE",
                            String.Format("[RESULTTIMESTAMP] = '{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"))
                            );
                        numerator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "DEFECTIVE_DIE",
                            String.Format("[RESULTTIMESTAMP] = '{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"))
                            );
                        r[sYAxis] = (double)(numerator / denominator * 100);
                    }
                    else
                    {
                        r[sYAxis] = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            sYAxis,
                            String.Format("[RESULTTIMESTAMP] = '{0}'", date.ToString("yyyy-MM-dd HH:mm:ss"))
                            );
                    }
                    r["INDEX"] = INDEX++;
                }
                #endregion [ inspection time ]
            }
            else
            {
                #region [ other ]
                dt = data.Tables["INSPECTION_INFO"].DefaultView.ToTable(true, sXAxis);
                dt.Columns.Add(new DataColumn("INDEX", typeof(int)));
                dt.Columns.Add(new DataColumn(sYAxis, typeof(double)));

                foreach (DataRow r in dt.Rows)
                {
                    if (String.Equals(sYAxis, "DEFECT_PERCENT"))
                    {
                        denominator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "INSPECTED_DIE",
                            String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis])
                            );
                        numerator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "DEFECTS",
                            String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis])
                            );
                        r[sYAxis] = (double)(numerator / denominator * 100);
                    }
                    else if (String.Equals(sYAxis, "DEFECTIVE_DIE_PERCENT"))
                    {
                        denominator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "INSPECTED_DIE",
                            String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis])
                            );
                        numerator = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            "DEFECTIVE_DIE",
                            String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis])
                            );
                        r[sYAxis] = (double)(numerator / denominator * 100);
                    }
                    else
                    {
                        r[sYAxis] = GetValue(
                            "INSPECTION_INFO",
                            Func.SUM,
                            sYAxis,
                            String.Format("[{0}] = '{1}'", sXAxis, r[sXAxis])
                            );
                    }
                    r["INDEX"] = INDEX++;
                }
                #endregion [ other ]
            }

            if (dt != null && dt.Rows.Count > 0)
                dt.AcceptChanges();

            return dt;
        }

        //--

        public DefectList GetDefectList(
            DataTable dt
            )
        {
            DefectList defects = new DefectList();
            foreach (DataRow r in dt.Rows)
            {
                Defect defect = new Defect();
                defect.STEP_SEQ = DACrux.Base.Convert.intParse(r["STEP_SEQ"].ToString());
                defect.DEFECTID = DACrux.Base.Convert.intParse(r["DEFECTID"].ToString());
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(r["WAFER_SEQ"].ToString());
                defect.X = DACrux.Base.Convert.doubleParse(r["X"].ToString());
                defect.Y = DACrux.Base.Convert.doubleParse(r["Y"].ToString());
                defect.XREL = DACrux.Base.Convert.doubleParse(r["XREL"].ToString());
                defect.YREL = DACrux.Base.Convert.doubleParse(r["YREL"].ToString());
                defect.XINDEX = DACrux.Base.Convert.intParse(r["XINDEX"].ToString());
                defect.YINDEX = DACrux.Base.Convert.intParse(r["YINDEX"].ToString());
                defect.XSIZE = DACrux.Base.Convert.doubleParse(r["XSIZE"].ToString());
                defect.YSIZE = DACrux.Base.Convert.doubleParse(r["YSIZE"].ToString());
                defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(r["DEFECTAREA"].ToString());
                defect.DSIZE = DACrux.Base.Convert.doubleParse(r["DSIZE"].ToString());
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                defect.TEST = DACrux.Base.Convert.intParse(r["TEST"].ToString());
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(r["CLUSTERNUMBER"].ToString());
                defect.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(r["ROUGHBINNUMBER"].ToString());
                defect.FINEBINNUMBER = DACrux.Base.Convert.intParse(r["FINEBINNUMBER"].ToString());
                defect.REVIEWSAMPLE = DACrux.Base.Convert.intParse(r["REVIEWSAMPLE"].ToString());
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(r["IMAGECOUNT"].ToString());
                defect.ADDER = DACrux.Base.Convert.intParse(r["ADDER"].ToString());
                defect.FIRST_STEP = r["FIRST_STEP"].ToString();
                defect.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(r["RETICLE_REPEAT_ID"].ToString());
                defect.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(r["DIE_REPEAT_ID"].ToString());
                defect.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(r["MAN_OPT_CLASS"].ToString());
                defect.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(r["AUTO_OPT_CLASS"].ToString());
                defect.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(r["MAN_SEM_CLASS"].ToString());
                defect.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(r["AUTO_SEM_CLASS"].ToString());
                defect.IMAGEURL = r["IMAGE_PATH"].ToString();

                //--

                defect.Images.Add(r["IMAGE_FILENAME"].ToString());

                //--

                defects.Add(defect);
            }

            return defects;
        }

        //--

        private double GetValue(
            String tablename,
            Func function,
            String column,
            String filter
            )
        {
            object o = data.Tables[tablename].Compute(String.Format("{0}({1})", Enum.GetName(typeof(Func), function), column), filter);
#if DEBUG
            Debug.WriteLine(String.Format("Value: {0}", o.ToString()));
#endif
            if (o == DBNull.Value)
                return double.NaN;
            return System.Convert.ToDouble(
                o
                );
        }

        private double GetValue(
            DataTable dt,
            Func function,
            String column,
            String filter
            )
        {
            object o = dt.Compute(String.Format("{0}({1})", Enum.GetName(typeof(Func), function), column), filter);
#if DEBUG
            Debug.WriteLine(String.Format("Value: {0}", o.ToString()));
#endif
            if (o == DBNull.Value)
                return double.NaN;
            else
                return System.Convert.ToDouble(
                    o
                    );
        }

        //--

        private DataTable SumByDataTable(
            DataTable dt,
            String sXAxis,
            String sYAxis
            )
        {
            return dt.AsEnumerable()
                .GroupBy(g => g.Field<Int64>(sXAxis))
                .Select(g =>
                {
                    var row = dt.NewRow();
                    row[sXAxis] = g.Key;
                    row[sYAxis] = g.Sum(r => r.Field<decimal>(sYAxis));
                    return row;
                })
                .CopyToDataTable();
        }

        //--

        private DataTable CountByDataTable(
            DataTable dt,
            String sXAxis,
            String sYAxis
            )
        {
            return dt.AsEnumerable()
                .GroupBy(g => g.Field<Int64>(sXAxis))
                .Select(g =>
                {
                    var row = dt.NewRow();
                    row[sXAxis] = g.Key;
                    row[sYAxis] = g.Count();
                    return row;
                })
                .CopyToDataTable();
        }

        //--

        private void DrawChart(
            )
        {
            this.chart1.ChartAreas.Clear();
            this.chart1.Legends.Clear();
            this.chart1.Series.Clear();

            //--
            try
            {
                StatusMessage("조회를 시작 합니다.");

                String sXAxis = ucmXValueMember.Value as String;
                String sYAxis = ucmYValueMember.Value as String;
                String sXAxisVisibleLabel = String.Empty;
                String sYAxisVisibleLabel = String.Empty;

                //--

                StatusMessage("Data 를 조회 중입니다.");
                DataTable chartdata = ChartData(
                    sXAxis,
                    sYAxis,
                    uchkNormalize.Checked,
                    out sXAxisVisibleLabel,
                    out sYAxisVisibleLabel
                    );
                if (chartdata == null)
                    return;

                //--
                StatusMessage("Data를 처리 중입니다.");
                ChartArea cArea = chart1.ChartAreas.Add("CHARTAREA");
                Legend cLegend = chart1.Legends.Add("LEGEND");
                Series cSeries = chart1.Series.Add("SERIES");

                chart1.DataSource = chartdata;

                //--

                cArea.AxisX.Enabled = AxisEnabled.True;
                cArea.AxisX.MajorGrid.Enabled = false;
                cArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot;
                cArea.AxisX.MajorGrid.LineWidth = 1;
                cArea.AxisX.MajorTickMark.Enabled = false;
                cArea.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
                cArea.AxisX.LabelStyle.Enabled = true;
                cArea.AxisX.LabelStyle.IsEndLabelVisible = true;
                cArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
                cArea.AxisX.ScaleView.Zoomable = true;
                cArea.AxisX.ScaleView.ZoomReset();
                cArea.AxisX.ScrollBar.LineColor = Color.Black;
                cArea.AxisX.ScrollBar.Size = 17;
                cArea.CursorX.AutoScroll = true;
                cArea.CursorX.IsUserSelectionEnabled = true;

                cArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                cArea.AxisX.Interval = 1;
                cArea.AxisX.IntervalOffset = 1;
                cArea.AxisX.IsLabelAutoFit = true;

                //--

                cArea.AxisY.Enabled = AxisEnabled.True;
                cArea.AxisY.MajorGrid.Enabled = false;
                cArea.AxisY.MajorTickMark.Enabled = false;
                cArea.AxisY.LabelStyle.Enabled = true;
                cArea.AxisY.ScaleView.SmallScrollSize = double.NaN;
                cArea.AxisY.ScaleView.Zoomable = true;
                cArea.AxisY.ScaleView.ZoomReset();
                cArea.AxisY.ScrollBar.LineColor = Color.Black;
                cArea.AxisY.ScrollBar.Size = 17;
                cArea.CursorY.AutoScroll = true;
                cArea.CursorY.IsUserSelectionEnabled = true;

                //--

                cLegend.Enabled = false;

                //--

                cSeries.ChartType = ChartTypeChanged();

                foreach (DataRow r in chartdata.Rows)
                {
                    cSeries.Points.AddXY(r[sXAxisVisibleLabel], r[sYAxisVisibleLabel]);
                }

                cSeries.IsValueShownAsLabel = true;

                chart1.DataBind();
                chart1.Invalidate();
            }
            finally
            {
                StatusMessage(null);
            }
        }

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
            row["KEY"] = "WAFER_ID";
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
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "DEFECT_DD";
            row["VALUE"] = "Defect Density";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "DEFECT_PERCENT";
            row["VALUE"] = "Defect Percent";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "Defective Die Count";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "DEFECTIVE_DIE_PERCENT";
            row["VALUE"] = "Defective Die Percent";
            dt.Rows.Add(row);

            //--

            return dt;
        }

        //--

        #endregion [ Datatable is chart option ]

        //--

        private SeriesChartType ChartTypeChanged(
            )
        {
            SeriesChartType charttype = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ucmType.Value.ToString(), true);
            return charttype;
        }

        //--

        public void DataBinding(
            )
        {
            data = DataSource as DataSet;
            if (data == null) return;

            DrawChart();
        }

        //--

        public void SetData(
            DACrux.Base.DPWafer[] wafer
            )
        {
            this.Wafer = wafer;
            //if (GetData())
            //    DrawChart();
        }

        //--

        #endregion [ Method ]

        //-------------------------------------------------------------------------------------

        #region [ Property ]
        public DACrux.Base.DPWafer[] Wafer
        {
            get;
            private set;
        }

        public object DataSource
        {
            get;
            set;
        }
        #endregion [ Property ]


        //-------------------------------------------------------------------------------------
    }
}
