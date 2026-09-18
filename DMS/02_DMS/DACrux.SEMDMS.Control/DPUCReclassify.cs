using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using Infragistics.Win;
using DACrux.SEMDMS.RO;
using FarPoint.Win.Spread.CellType;
using DACrux.Framework.Controls;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCReclassify
        : UserControl
    {
        //----------------------------------------------------------------------------------------------------

        #region [ Data Field ]
        public event EventHandler OnReclassifyApply;
        private enum DefectClassColIndex { CLASSNUMBER = 0, NAME, DESCRIPTION, DEFECT_COLOR }
        private enum ColumnIndex { DEFECTID = 0, DEFECTCLASS, STEPID, DEVICEID }

        private DataTable dtClassInfo = null;
        private DefectList _selectedDefects = null;
        private object _defectClass = null;
        #endregion [ Data Field ]

        #region [ Constructor ]

        //----------------------------------------------------------------------------------------------------

        public DPUCReclassify()
        {
            InitializeComponent();
            Utility.FPSpreadUtil.InitSpread(fpsDisplayClass);
            Utility.FPSpreadUtil.InitSpread(fpsDefects);
        }

        #endregion [ Constructor ]

        //----------------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void DPUCReclassify_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            DefectClassBinding();
        }

        //--

        private void btnApply_Click(
            object sender,
            EventArgs e
            )
        {
            if (OnReclassifyApply != null)
                OnReclassifyApply(this, e);
        }

        //--

        private void rdbClassCodeOrName_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            if (!(sender as RadioButton).Checked)
                return;
            SetNewDefectClass();
        }

        #endregion [ Event Handler ]

        //----------------------------------------------------------------------------------------------------

        #region [ Method ]

        private void DefectClassBinding()
        {
            SEMConfiguration obj = new SEMConfiguration();
            dtClassInfo = obj.GetDefectTypeList();

            if (dtClassInfo == null || dtClassInfo.Rows.Count <= 0)
                return;

            TextCellType text = new TextCellType();

            fpsDisplayClass_Sheet.ColumnCount = 4;
            fpsDisplayClass_Sheet.RowCount = dtClassInfo.Rows.Count;
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.CLASSNUMBER].Label = Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.CLASSNUMBER);
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.CLASSNUMBER].CellType = text;
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.CLASSNUMBER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.NAME].Label = Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.NAME);
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.NAME].CellType = text;
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.DESCRIPTION].Label = Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.DESCRIPTION);
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.DESCRIPTION].CellType = text;
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.DEFECT_COLOR].Label = Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.DEFECT_COLOR);
            fpsDisplayClass_Sheet.Columns[(int)DefectClassColIndex.DEFECT_COLOR].CellType = text;

            for (int rowIdx = 0; rowIdx < dtClassInfo.Rows.Count; rowIdx++)
            {
                DataRow row = dtClassInfo.Rows[rowIdx];
                fpsDisplayClass_Sheet.Cells[rowIdx, (int)DefectClassColIndex.CLASSNUMBER].Value = row[Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.CLASSNUMBER)];
                fpsDisplayClass_Sheet.Cells[rowIdx, (int)DefectClassColIndex.NAME].Value = row[Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.NAME)];
                fpsDisplayClass_Sheet.Cells[rowIdx, (int)DefectClassColIndex.DESCRIPTION].Value = row[Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.DESCRIPTION)];
                fpsDisplayClass_Sheet.Cells[rowIdx, (int)DefectClassColIndex.DEFECT_COLOR].Value = row[Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.DEFECT_COLOR)];
                fpsDisplayClass_Sheet.Cells[rowIdx, (int)DefectClassColIndex.DEFECT_COLOR].BackColor = GetClassColor(row[Enum.GetName(typeof(DefectClassColIndex), DefectClassColIndex.DEFECT_COLOR)]);
            }

            Utility.FPSpreadUtil.SetAutoColumnWidth(fpsDisplayClass_Sheet);
            SetNewDefectClass();
        }

        private Color GetClassColor(object color)
        {
            return ColorTranslator.FromHtml(color.ToString());
        }

        //--

        private void SelectedDefects()
        {
            Utility.FPSpreadUtil.InitSpread(fpsDefects);
            if (_selectedDefects == null || _selectedDefects.Count <= 0)
                return;

            fpsDefects_Sheet.ColumnCount = 4;
            fpsDefects_Sheet.RowCount = _selectedDefects.Count;

            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEFECTID].Label = "Defect ID";
            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEFECTID].Width = 60;
            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEFECTCLASS].Label = "Defect Class";
            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEFECTCLASS].Width = 100;
            fpsDefects_Sheet.Columns[(int)ColumnIndex.STEPID].Label = "Step ID";
            fpsDefects_Sheet.Columns[(int)ColumnIndex.STEPID].Width = 75;
            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEVICEID].Label = "Device ID";
            fpsDefects_Sheet.Columns[(int)ColumnIndex.DEVICEID].Width = 75;

            //--

            for (int idx = 0; idx < _selectedDefects.Count; idx++)
            {
                Defect defect = _selectedDefects[idx];
                fpsDefects_Sheet.Cells[idx, (int)ColumnIndex.DEFECTID].Value = defect.DEFECTID;
                fpsDefects_Sheet.Cells[idx, (int)ColumnIndex.DEFECTCLASS].Value = DmsCache.Instance.ClassLookup[defect.CLASSNUMBER];

                DmsWaferDieInfo dieinfo = DmsCache.Instance[defect.STEP_SEQ];
                fpsDefects_Sheet.Cells[idx, 2].Value = dieinfo.StepInfo.StepID;
                fpsDefects_Sheet.Cells[idx, 3].Value = dieinfo.StepInfo.DeviceID;
            }
        }

        //--

        private void SetNewDefectClass(
            )
        {
            string search = dlbNewClassNumber.SearchText;
            dlbNewClassNumber.ClearDataSource();
            if (!dtClassInfo.Columns.Contains("CLASSCODE") && !dtClassInfo.Columns.Contains("CLASSNAME"))
            {
                dtClassInfo.Columns.AddRange(new DataColumn[] { new DataColumn("CLASSCODE", typeof(String)), new DataColumn("CLASSNAME", typeof(String)) });

                foreach (DataRow dr in dtClassInfo.Rows)
                {
                    dr["CLASSCODE"] = String.Format("{0}-{1}", dr["CLASSNUMBER"], dr["NAME"]);
                    dr["CLASSNAME"] = String.Format("{0}-{1}", dr["NAME"], dr["CLASSNUMBER"]);
                }
                dtClassInfo.AcceptChanges();
            }

            dlbNewClassNumber.DataSource = dtClassInfo;
            dlbNewClassNumber.DisplayMember = rdbClassName.Checked ? "CLASSNAME" : "CLASSCODE";
            dlbNewClassNumber.ValueMember = "CLASSNUMBER";
            dlbNewClassNumber.SearchText = search;
        }

        #endregion [ Method ]

        //----------------------------------------------------------------------------------------------------

        #region [ Property ]

        //--

        public DefectList DataSource
        {
            get
            {
                return _selectedDefects;
            }
            set
            {
                _selectedDefects = value;
                SelectedDefects();
            }
        }

        //--

        public object SelectedNewDefectClass
        {
            get
            {
                return dlbNewClassNumber.SelectedValue;
            }
            private set
            {
                dlbNewClassNumber.SelectedItem = value;
            }
        }

        public bool IsDisplayApply
        {
            get
            {
                return btnApply.Visible;
            }
            set
            {
                btnApply.Visible = value;
            }
        }
        #endregion [ Property ]

        //----------------------------------------------------------------------------------------------------
    }
}
