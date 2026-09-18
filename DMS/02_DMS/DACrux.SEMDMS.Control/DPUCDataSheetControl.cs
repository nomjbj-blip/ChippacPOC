using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCDataSheetControl : UserControl
    {
        private DataSet SheetData = null;
        private TabPage[] tabPages = null;
        private FpSpread[] spreads = null;
        private SheetView[] sheets = null;
        private readonly double MaxValue = 99999999999999;
        private readonly double MinValue = -99999999999999;

        private enum ColumnIndex { DEVICE = 0, STEP_ID, INSPECTION_TIME, INSPECTOR, WAFER_ID, LOT_ID, TST_PGM, WF_MPY, SCAN_AREA, DEFECT_DENSITY }

        public DPUCDataSheetControl()
        {
            InitializeComponent();
        }

        #region [ Event Handler ]

        private void DPUCDataSheetControl_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;
        }

        #endregion [ Event Handler ]

        #region [ Method ]
        private void SetBinding(
            object value
            )
        {
            InitComponent();

            SheetData = value as DataSet;
            if (SheetData == null || SheetData.Tables.Count <= 0)
                return;

            tabPages = new TabPage[SheetData.Tables.Count];
            spreads = new FpSpread[SheetData.Tables.Count];
            sheets = new SheetView[SheetData.Tables.Count];

            for (int idx = 0; idx < SheetData.Tables.Count; idx++)
            {
                spreads[idx] = new FpSpread();
                sheets[idx] = new SheetView();
                tabPages[idx] = new TabPage();

                //--

                tabPages[idx].Controls.Add(spreads[idx]);
                tabPages[idx].Name = SheetData.Tables[idx].TableName;
                tabPages[idx].Padding = new System.Windows.Forms.Padding(3);
                tabPages[idx].UseVisualStyleBackColor = true;
                tabPages[idx].Text = SheetData.Tables[idx].TableName;

                //--

                spreads[idx].AccessibleDescription = String.Empty;
                spreads[idx].Dock = DockStyle.Fill;
                spreads[idx].Location = new Point(3, 3);
                spreads[idx].Name = String.Format("fps{0}", SheetData.Tables[idx].TableName);
                spreads[idx].Sheets.Add(sheets[idx]);
                spreads[idx].Size = new System.Drawing.Size(463, 291);
                spreads[idx].TabIndex = 0;

                //--

                sheets[idx].Reset();
                sheets[idx].SheetName = String.Format("fps{0}_Sheet", SheetData.Tables[idx].TableName);
                Utility.FPSpreadUtil.InitSpread(spreads[idx]);
                Utility.FPSpreadUtil.SetSpreadData(SheetData.Tables[idx], sheets[idx]);
                Utility.FPSpreadUtil.VisibleSpreadColumns(sheets[idx], new string[] { "STEP_SEQ" }, false);

                FarPoint.Win.Spread.CellType.NumberCellType numberCell1 = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCell1.MaximumValue = MaxValue;
                numberCell1.MinimumValue = -MinValue;
                numberCell1.DecimalPlaces = 3;

                sheets[idx].Columns[(int)ColumnIndex.DEFECT_DENSITY].CellType = numberCell1;
                sheets[idx].Columns[(int)ColumnIndex.SCAN_AREA].CellType = numberCell1;

                FarPoint.Win.Spread.CellType.NumberCellType numberCell2 = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCell1.MaximumValue = MaxValue;
                numberCell1.MinimumValue = -MinValue;
                numberCell2.DecimalPlaces = DecimalPlaces;
                TotalColumnIndex = SheetData.Tables[idx].Columns["TOTAL"].Ordinal + 1;
                sheets[idx].Columns[TotalColumnIndex, sheets[idx].ColumnCount - 1].CellType = numberCell2;

                Utility.FPSpreadUtil.SetAutoColumnFilter(sheets[idx], true, ColumnIndex.DEVICE, ColumnIndex.STEP_ID, ColumnIndex.INSPECTION_TIME, ColumnIndex.INSPECTOR, ColumnIndex.WAFER_ID, ColumnIndex.LOT_ID, ColumnIndex.TST_PGM, ColumnIndex.WF_MPY, ColumnIndex.SCAN_AREA, ColumnIndex.DEFECT_DENSITY);
                Utility.FPSpreadUtil.SetAutoColumnSort(sheets[idx], true, ColumnIndex.DEVICE, ColumnIndex.STEP_ID, ColumnIndex.INSPECTION_TIME, ColumnIndex.INSPECTOR, ColumnIndex.WAFER_ID, ColumnIndex.LOT_ID, ColumnIndex.TST_PGM, ColumnIndex.WF_MPY, ColumnIndex.SCAN_AREA, ColumnIndex.DEFECT_DENSITY);
                Utility.FPSpreadUtil.SetAutoColumnWidth(sheets[idx]);
                sheets[idx].FrozenColumnCount = (int)ColumnIndex.TST_PGM;


                //--

                tabControl1.Controls.Add(tabPages[idx]);
            }
        }

        private void InitComponent()
        {
            if (tabControl1.Controls.Count <= 0)
                return;

            foreach (TabPage page in tabControl1.TabPages)
            {
                for (int idx = 0; idx < page.Controls.Count; idx++)
                {
                    FpSpread spread = page.Controls[idx] as FpSpread;
                    if (spread == null)
                        continue;

                    spread.Reset();
                    spread = null;
                }
            }

            tabControl1.Controls.Clear();
        }

        public void SheetRefrash()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                for (int idx = 0; idx < page.Controls.Count; idx++)
                {
                    FpSpread spread = page.Controls[idx] as FpSpread;
                    if (spread == null)
                        continue;

                    SheetView sheet = spread.ActiveSheet;
                    FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                    numberCell.MaximumValue = MaxValue;
                    numberCell.MinimumValue = -MinValue;
                    numberCell.DecimalPlaces = DecimalPlaces;
                    sheet.Columns[TotalColumnIndex, sheet.ColumnCount - 1].CellType = numberCell;

                    spread.Refresh();
                }
            }
        }
        #endregion [ Method ]

        #region [ Property ]
        public object DataSource
        {
            get { return SheetData; }
            set { SetBinding(value); }
        }

        public int DefaultColumnWidth
        {
            get;
            set;
        }

        public int DecimalPlaces
        {
            get;
            set;
        }

        public TabControl.TabPageCollection Tabs
        {
            get { return this.tabControl1.TabPages; }
        }

        public int TotalColumnIndex
        {
            get;
            private set;
        }
        #endregion [ Property ]
    }
}
