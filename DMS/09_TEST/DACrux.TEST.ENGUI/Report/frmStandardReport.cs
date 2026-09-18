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
    public partial class frmStandardReport : DACruxUXBasic01, DACrux.Framework.Base.IExportExcel
    {
        private enum Columns
        {
            PROGRAM = 0,
            REV = 1,
            PARAM_NAME,
            INDEX,
            BOX_PLOT,
            LSL,
            USL,
            LCL,
            UCL,
            COUNT,
            MININUM,
            Q1,
            MEDIAN,
            Q3,
            MAXINUM,
            AVERAGE,
            STDDEV
        }


        private static readonly string DATE_FORMAT = "yyyyMMdd";


        public frmStandardReport()
        {
            InitializeComponent();
        }

        #region [ Event Handler ]
        private void frmStandardReport_Load(
            object sender,
            EventArgs e
            )
        {
            dtStart.Value = DateTime.Now.AddDays(-1).Date;
            dtEnd.Value = DateTime.Now.Date;
            DataSelect obj = new DataSelect();
            DataTable dt = obj.GetConditionTestArea();
            SetBinding(dlbTestArea, dt);

            Utility.FPSpreadUtil.InitSpread(fpsSummary);
            Utility.FPSpreadUtil.SetSpreadData(CreateSummaryDataTable(), fpsSummary_Sheet1, 100, 4);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpsSummary_Sheet1);
        }

        //--

        private void btnView_Click(
            object sender,
            EventArgs e
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpsSummary);
            GetData();
        }

        //--

        private void date_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            DataSelect obj = new DataSelect();
            DataTable dt = obj.GetConditionTestArea();
            SetBinding(dlbTestArea, dt);
        }

        //--

        private void dlbTestArea_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbTestArea.SelectedIndex < 0)
                return;

            dlbProduct.ClearDataSource();
            dlbProgram.ClearDataSource();
            dlbLot.ClearDataSource();

            DataSelect obj = new DataSelect();
            DataTable dt = obj.GetConditionDevice(
                GetDateToString(dtStart),
                GetDateToString(dtEnd),
                GetSelectedValues(dlbTestArea)
                );
            SetBinding(dlbProduct, dt);
        }

        //--

        private void dlbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbProduct.SelectedIndex < 0)
                return;

            dlbProgram.ClearDataSource();
            dlbLot.ClearDataSource();

            DataSelect obj = new DataSelect();
            DataTable dt = obj.GetConditionProgram(
                GetDateToString(dtStart),
                GetDateToString(dtEnd),
                GetSelectedValues(dlbTestArea),
                GetSelectedValues(dlbProduct)
                );
            SetBinding(dlbProgram, dt);
        }

        //--

        private void dlbProgram_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbProgram.SelectedIndex < 0)
                return;

            dlbLot.ClearDataSource();

            DataSelect obj = new DataSelect();
            DataTable dt = obj.GetConditionLot(
                GetDateToString(dtStart),
                GetDateToString(dtEnd),
                GetSelectedValues(dlbTestArea),
                GetSelectedValues(dlbProduct),
                GetSelectedValues(dlbProgram)
                );
            SetBinding(dlbLot, dt, 0, 1);
        }

        //--

        private void dlbLot_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (dlbLot.SelectedIndex < 0)
                return;

            btnView.PerformClick();
        }

        private void dlbLot_EnterTextBox(
            object sender,
            EventArgs e
            )
        {
            string search = dlbLot.SearchText;
            if (string.IsNullOrEmpty(search))
                return;


            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetLotInfo(
                GetSelectedValues(dlbTestArea),
                GetDateToString(dtStart),
                GetDateToString(dtEnd),
                search
                );

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("데이터를 찾을 수 없습니다.");
                return;
            }

            // PRODUCT의 경우 0번째 항목으로 선택한다.
            dlbProduct.Text = dt.Rows[0]["DEVICE"].ToString();

            // PROGRAM이 1개인 경우 선택해준다.
            dlbProgram.ClearDataSource();
            SetBinding(dlbProgram, dt, 1, 1);
            if (dt.Rows.Count == 1)
                dlbProgram.Text = dt.Rows[0]["PROGRAM"].ToString();

            dlbLot.SearchText = search;
        }

        //--

        private void fpsSummary_ColumnWidthChanged(
            object sender,
            FarPoint.Win.Spread.ColumnWidthChangedEventArgs e
            )
        {
            bool include = false;

            foreach (FarPoint.Win.Spread.ColumnWidthChangeExtents col in e.ColumnList)
            {
                if (col.FirstColumn <= (int)Columns.BOX_PLOT && col.LastColumn >= (int)Columns.BOX_PLOT)
                    include = true;
            }

            if (!include)
                return;

            DrawBoxPlot(
                fpsSummary_Sheet1
                );
        }

        //--

        private void fpsSummary_RowHeightChanged(
            object sender,
            FarPoint.Win.Spread.RowHeightChangedEventArgs e
            )
        {
            DrawBoxPlot(
                fpsSummary_Sheet1
                );
        }

        //--

        private void numMargin_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            DrawBoxPlot(
                fpsSummary_Sheet1
                );
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        private DataTable CreateSummaryDataTable(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { 
                new DataColumn("PROGRAM", typeof(string)), 
                new DataColumn("REV", typeof(int)), 
                new DataColumn("PARAM_NAME", typeof(string)), 
                new DataColumn("INDEX", typeof(int)), 
                new DataColumn("BOX_PLOT", typeof(string)), 
                new DataColumn("LSL", typeof(decimal)), 
                new DataColumn("USL", typeof(decimal)), 
                new DataColumn("LCL", typeof(decimal)), 
                new DataColumn("UCL", typeof(decimal)), 
                new DataColumn("COUNT", typeof(int)), 
                new DataColumn("MININUM", typeof(decimal)), 
                new DataColumn("Q1", typeof(decimal)), 
                new DataColumn("MEDIAN", typeof(decimal)), 
                new DataColumn("Q3", typeof(decimal)), 
                new DataColumn("MAXINUM", typeof(decimal)),
                new DataColumn("AVERAGE", typeof(decimal)),
                new DataColumn("STDDEV", typeof(decimal)) 
            });
            return dt;
        }

        /// <summary>
        /// BoxPlot 그래프를 시트에 넣어줍니다.
        /// </summary>
        /// 
        private readonly int MEDIAN_RADIUS = 2;
        private readonly int QUARTILE_MARGIN = 4;
        private readonly int Y_MARGIN = 1;

        private void DrawBoxPlot(
            SheetView sheet
            )
        {
            DataTable dt = sheet.DataSource as DataTable;
            double q1 = double.NaN;
            double q3 = double.NaN;
            double median = double.NaN;
            double usl = double.NaN;
            double lsl = double.NaN;
            double min = double.NaN;
            double max = double.NaN;
            double avg = double.NaN;
            double stddev = double.NaN;

            if (dt == null || dt.Rows.Count == 0)
                return;

            Pen pen = new Pen(Color.Black, 1);
            float width = (float)sheet.Columns[(int)Columns.BOX_PLOT].Width;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                float height = (float)sheet.Rows[i].Height;
                Bitmap bmp = new Bitmap((int)width, (int)height);

                usl = lsl = min = q1 = median = q3 = max = avg = stddev = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.USL].ToString(), out usl))
                    usl = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.LSL].ToString(), out lsl))
                    lsl = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.MININUM].ToString(), out min))
                    min = double.MinValue;

                if (!double.TryParse(dt.Rows[i][(int)Columns.Q1].ToString(), out q1))
                    q1 = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.MEDIAN].ToString(), out median))
                    median = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.Q3].ToString(), out q3))
                    q3 = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.MAXINUM].ToString(), out max))
                    max = double.MaxValue;

                if (!double.TryParse(dt.Rows[i][(int)Columns.AVERAGE].ToString(), out avg))
                    avg = double.NaN;

                if (!double.TryParse(dt.Rows[i][(int)Columns.STDDEV].ToString(), out stddev))
                    stddev = double.NaN;

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    Matrix mx = new Matrix();
                    mx.Translate((float)((width - width * 0.01 * (float)numMargin.Value) / 2), 0);
                    mx.Scale((float)((numMargin.Value - 2) / 100), 1);
                    g.Transform = mx;

                    if (Double.IsNaN(usl)) usl = max;
                    if (Double.IsNaN(lsl)) lsl = min;

                    if ((usl - lsl) == 0)
                        continue;

                    double scale = Math.Abs(width / (usl - lsl));

                    // 3sigma Range
                    g.FillRectangle(
                        Brushes.Yellow,
                        (float)(((avg - 3) * (stddev + (-(lsl)))) * scale),
                        0,
                        (float)(6 * stddev * scale),
                        height
                        );

                    // Average Line
                    g.DrawLine(
                        Pens.Red,
                        (float)((avg - lsl) * scale),
                        Y_MARGIN,
                        (float)((avg - lsl) * scale),
                        height - Y_MARGIN
                        );

                    // USL Line
                    g.DrawLine(
                        Pens.Red,
                        (float)width,
                        Y_MARGIN,
                        (float)width,
                        height - Y_MARGIN
                        );

                    // LSL Line
                    g.DrawLine(
                        Pens.Red,
                        0,
                        Y_MARGIN,
                        0,
                        height - Y_MARGIN
                        );

                    // Quartile Rectangle
                    g.DrawRectangle(
                        pen,
                        (float)((q1 - lsl) * scale),
                        QUARTILE_MARGIN,
                        (float)((q3 - q1) * scale),
                        height - 2 * QUARTILE_MARGIN
                        );

                    // Median Circle
                    g.DrawEllipse(
                        pen,
                        (float)(Math.Max(((median - lsl) * scale - MEDIAN_RADIUS), 0)),
                        (float)(height / 2 - MEDIAN_RADIUS),
                        2 * MEDIAN_RADIUS,
                        2 * MEDIAN_RADIUS
                        );

                    // Min Line
                    g.DrawLine(
                        pen,
                        (float)((min - lsl) * scale),
                        (float)(height / 2),
                        (float)((q1 - lsl) * scale),
                        (float)(height / 2)
                        );

                    // Max Line
                    g.DrawLine(
                        pen,
                        (float)((q3 - lsl) * scale),
                        (float)(height / 2),
                        (float)((max - lsl) * scale),
                        (float)(height / 2)
                        );
                }

                GeneralCellType cell = new GeneralCellType();
                sheet.Cells[i, (int)Columns.BOX_PLOT].CellType = cell;
                cell.BackgroundImage = new Picture(bmp);
            }
        }

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(fpsSummary);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Sheet";
            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion

        private void GetData(
            )
        {
            try
            {
                if (dlbLot.SelectedItems.Count > 5)
                {
                    DspMessage("You can not select more than 5 selected Lot items.");
                    return;
                }

                //--

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                String testarea = dlbTestArea.SelectedValue as String;
                if (String.IsNullOrEmpty(testarea))
                    return;

                String product = dlbProduct.SelectedValue as String;
                if (String.IsNullOrEmpty(testarea))
                    return;

                String program = dlbProgram.SelectedValue as String;
                if (String.IsNullOrEmpty(testarea))
                    return;

                TestCommon oTestComm = new TestCommon();
                DataTable dt = oTestComm.GetStandardReport(
                    dtStart.Value,
                    dtEnd.Value,
                    DACrux.Base.GlobalVariable.Factory,
                    testarea,
                    product,
                    program,
                    GetSelectedValues(dlbLot)
                    );
                if (dt != null && dt.Rows.Count > 0)
                    dt = dt.Select(String.Empty, "INDEX ASC").CopyToDataTable<DataRow>();

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                Utility.FPSpreadUtil.InitSpread(fpsSummary);
                Utility.FPSpreadUtil.SetSpreadData(dt, fpsSummary_Sheet1);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsSummary_Sheet1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Utility.FPSpreadUtil.SetDecimalPlaces(
                        fpsSummary_Sheet1,
                        new int[] { (int)Columns.LSL, (int)Columns.USL, (int)Columns.LCL, (int)Columns.UCL, (int)Columns.MININUM, (int)Columns.Q1, (int)Columns.AVERAGE, (int)Columns.MEDIAN, (int)Columns.STDDEV, (int)Columns.Q3, (int)Columns.MAXINUM },
                        new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }
                        );
                    DrawBoxPlot(fpsSummary_Sheet1);
                }
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }


        private string GetDateToString(
            DateTimePicker datepicker
            )
        {
            return datepicker.Value.ToString(DATE_FORMAT);
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



        private void SetBinding(
            DUCListBox listbox,
            DataTable source,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (source != null)
            {
                listbox.DisplayMember = source.Columns[dispIndex].ColumnName;
                listbox.ValueMember = source.Columns[valueIndex].ColumnName;
            }

            listbox.DataSource = source;
        }

        #endregion [ Method ]


    }
}
