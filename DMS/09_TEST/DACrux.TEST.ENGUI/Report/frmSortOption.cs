using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread.CellType;
using System.Diagnostics;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSortOption : Form
    {
        #region [ Constrator ]
        enum SheetColumnIndex { Column = 0, Ascending }

        string[] columns = null;
        public frmSortOption()
        {
            InitializeComponent();
            SortedColumn = new SortedColumns();
        }

        public frmSortOption(
            Array array
            )
            : this()
        {
            columns = new string[array.Length];
            for (int idx = 0; idx < array.Length; idx++)
            {
                columns[idx] = array.GetValue(idx).ToString();
            }

            ComboBoxCellType comboBox = new ComboBoxCellType();
            comboBox.ItemData = (string[])columns;
            comboBox.Items = (string[])columns;
            comboBox.EditorValue = EditorValue.ItemData;

            fpSpread1_Sheet1.Columns[(int)SheetColumnIndex.Column].CellType = comboBox;
        }

        #endregion [ Constrator ]

        //-----------------------------------------------------------------------------

        #region [ Event Handler ]

        private void frmSortOption_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            LoadSortedColumn();
        }

        private void btnAdd_Click(
            object sender,
            EventArgs e
            )
        {
            fpSpread1_Sheet1.Rows.Add(fpSpread1_Sheet1.Rows.Count, 1);
        }

        private void btnDown_Click(
            object sender,
            EventArgs e
            )
        {
            if (fpSpread1_Sheet1.ActiveRowIndex >= fpSpread1_Sheet1.Rows.Count - 1)
                return;

            string sCol = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Column].Text;
            bool bFlag = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Ascending].Text == Boolean.TrueString ? true : false;

            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Column].Value = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex + 1, (int)SheetColumnIndex.Column].Value;
            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Ascending].Value = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex + 1, (int)SheetColumnIndex.Ascending].Value;

            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex + 1, (int)SheetColumnIndex.Column].Value = sCol;
            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex + 1, (int)SheetColumnIndex.Ascending].Value = bFlag;
            fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex + 1, (int)SheetColumnIndex.Column);
        }

        private void btnOk_Click(
            object sender,
            EventArgs e
            )
        {
            string sColName = string.Empty;
            string sTmp = string.Empty;
            bool bAscending = true;
            SortedColumn = new SortedColumns();
            for (int idx = 0; idx < fpSpread1_Sheet1.Rows.Count; idx++)
            {
                sColName = fpSpread1_Sheet1.Cells[idx, (int)SheetColumnIndex.Column].Text as string;
                sTmp = fpSpread1_Sheet1.Cells[idx, (int)SheetColumnIndex.Ascending].Text as string;

                if (String.IsNullOrEmpty(sColName))
                    continue;

                if (sTmp == Boolean.TrueString)
                    bAscending = true;
                else bAscending = false;
                SortedColumn.Add(sColName, bAscending);
            }
        }

        private void btnUp_Click(
            object sender,
            EventArgs e
            )
        {
            if (fpSpread1_Sheet1.ActiveRowIndex <= 0)
                return;

            string sCol = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Column].Text;
            bool bFlag = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Ascending].Text == Boolean.TrueString ? true : false;

            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Column].Value = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex - 1, (int)SheetColumnIndex.Column].Value;
            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex, (int)SheetColumnIndex.Ascending].Value = fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex - 1, (int)SheetColumnIndex.Ascending].Value;

            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex - 1, (int)SheetColumnIndex.Column].Value = sCol;
            fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex - 1, (int)SheetColumnIndex.Ascending].Value = bFlag;
            fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex - 1, (int)SheetColumnIndex.Column);
        }

        private void btnRemove_Click(
            object sender,
            EventArgs e
            )
        {
            fpSpread1_Sheet1.Rows.Remove(fpSpread1_Sheet1.Rows.Count - 1, 1);
        }

        private void fpSpread1_ComboSelChange(
            object sender, 
            FarPoint.Win.Spread.EditorNotifyEventArgs e
            )
        {
            FarPoint.Win.FpCombo ctl = e.EditingControl as FarPoint.Win.FpCombo;
        }

        #endregion [ Event Handler ]

        //-----------------------------------------------------------------------------

        #region [ Method ]

        private void LoadSortedColumn()
        {
            fpSpread1_Sheet1.Rows.Add(0, SortedColumn.Count);
            int rowIdx = 0;
            foreach (KeyValuePair<string, bool> pv in SortedColumn)
            {
                fpSpread1_Sheet1.Cells[rowIdx, (int)SheetColumnIndex.Column].Value = pv.Key;
                fpSpread1_Sheet1.Cells[rowIdx, (int)SheetColumnIndex.Ascending].Value = pv.Value;
                fpSpread1_Sheet1.Rows[rowIdx++].Height = 20;
            }
        }
        #endregion [ Method ]

        //-----------------------------------------------------------------------------

        #region [ Property ]

        public SortedColumns SortedColumn
        {
            get;
            set;
        }

        #endregion [ Property ]
    }

    public class SortedColumns : Dictionary<string, bool>
    {
        public override string ToString()
        {
            StringBuilder sbText = new StringBuilder();
            foreach (KeyValuePair<string, bool> pv in this)
            {
                sbText.AppendFormat("{0} {1},", pv.Key, pv.Value == true ? "ASC" : "DESC");
            }
            return sbText.ToString().Substring(0, sbText.Length - 1);
        }
    }
}
