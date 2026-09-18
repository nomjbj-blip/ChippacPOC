using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using FarPoint.Win.Spread.CellType;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectTypeDefine_VerTwo : DACrux.Framework.Base.DACruxUXBasic01
    {
        private readonly int[] UPDATE_HEADER_INDEX = null;
        private readonly String DELETE_FLAG = "DELETE";
        private readonly String INSERT_FLAG = "INSERT";
        private readonly String UPDATE_FLAG = "UPDATE";

        public frmDefectTypeDefine_VerTwo()
        {
            InitializeComponent();
            UPDATE_HEADER_INDEX = new int[] { 2, 3, 4 };
        }

        #region Event Handler

        private void frmDefectTypeDefine_VerTwo_Load(
            object sender,
            EventArgs e
            )
        {
            GetData();
        }

        //--

        private void Button_Click(
            object sender,
            EventArgs e
            )
        {
            Button btn = sender as Button;

            try
            {
                if (btn == btnItemAdd)
                    ItemAdd();

                //--

                if (btn == btnItemRemove)
                    ItemRemove();

                //--

                if (btn == btnSave)
                    ItemSave();

                if (btn == btnSearch)
                    ItemSearch();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void fpsDefectType_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            try
            {
               // picBoxTypeColor.BackColor = fpsDefectType.ActiveSheet.Cells[e.Row, 5].BackColor;
                
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void fpsDefectType_CellDoubleClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            if (String.IsNullOrEmpty(fpsDefectType_Sheet.Cells[e.Row, 0].Text)
               || String.Equals(fpsDefectType_Sheet.Cells[e.Row, 0].Text, UPDATE_FLAG))
            {
                if (!UPDATE_HEADER_INDEX.Contains(e.Column))
                {
                    e.Cancel = true;
                    return;
                }
            }
            e.Cancel = false;
            fpsDefectType_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
        }

        //--

        private void fpsDefectType_EditModeOff(
            object sender,
            EventArgs e
            )
        {
            if (String.IsNullOrEmpty(fpsDefectType_Sheet.Cells[fpsDefectType_Sheet.ActiveRowIndex, 0].Text))
            {
                if (!String.IsNullOrEmpty(fpsDefectType_Sheet.Cells[fpsDefectType_Sheet.ActiveRowIndex, fpsDefectType_Sheet.ActiveColumnIndex].Text))
                {
                    fpsDefectType_Sheet.Cells[fpsDefectType_Sheet.ActiveRowIndex, 0].Text = UPDATE_FLAG;
                }
            }
            fpsDefectType_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
        }

        //--

        private void fpsDefectType_EditModeOn(
            object sender,
            EventArgs e
            )
        {
        }

        //--

        private void fpsDefectType_EditModeStarting(
            object sender,
            FarPoint.Win.Spread.EditModeStartingEventArgs e
            )
        {
            if (!UPDATE_HEADER_INDEX.Contains(fpsDefectType_Sheet.ActiveColumnIndex))
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        //--

        private void txtSearch_KeyDown(
            object sender, 
            KeyEventArgs e
            )
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ItemSearch();
        }

        //--

        private void BtnReSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }


        //--

        private void picBoxTypeColor_Click(object sender, EventArgs e)
        {
            try
            {
                picBoxTypeColor.BackColor = ColorDialog(picBoxTypeColor.BackColor);

                FarPoint.Win.Spread.Model.CellRange[] oSelectItem = fpsDefectType.ActiveSheet.GetSelections();

                int idx = -1;
                for (int i = 0; i < oSelectItem.Length; i++)
                {
                    for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                    {
                        idx = oSelectItem[i].Row + cr;
                        fpsDefectType.ActiveSheet.Cells[idx, 5].Value = ColorToRgbString(picBoxTypeColor.BackColor);
                        fpsDefectType.ActiveSheet.Cells[idx, 5].BackColor = ColorTodRgbString(fpsDefectType.ActiveSheet.Cells[idx, 5].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void fpsDefectType_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            if (e.Range.Row == -1) return;
            try
            {
                picBoxTypeColor.BackColor = fpsDefectType.ActiveSheet.Cells[e.Range.Row, 5].BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }

            if (e.Range.Column != 5) return;
            try
            {
                fpsDefectType.ActiveSheet.Cells[e.Range.Row, e.Range.Column].BackColor = ColorDialog(fpsDefectType.ActiveSheet.Cells[e.Range.Row, e.Range.Column].BackColor);
                picBoxTypeColor.BackColor = fpsDefectType.ActiveSheet.Cells[e.Range.Row, e.Range.Column].BackColor;
                fpsDefectType.ActiveSheet.Cells[e.Range.Row, e.Range.Column].Value = ColorToRgbString(picBoxTypeColor.BackColor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }
        #endregion Event Handler

        #region Method
        private void GetData(
            )
        {
            SEMConfiguration oSEMConfig = null;
            DataTable dt = null;

            try
            {
                oSEMConfig = new SEMConfiguration();

                dt = oSEMConfig.GetDefectTypeList();
                Utility.FPSpreadUtil.InitSpread(fpsDefectType);
                Utility.FPSpreadUtil.SetSpreadData(dt, fpsDefectType_Sheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsDefectType_Sheet);

                if (dt != null && dt.Rows.Count > 0)
                    SetColumnStyle();

                for (int ir = 0; ir < fpsDefectType_Sheet.RowCount; ir++)
                {
                    fpsDefectType_Sheet.Cells[ir, 5].BackColor = ColorTodRgbString(fpsDefectType_Sheet.Cells[ir, 5].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        string ColorToRgbString(Color c)
        {
            return  "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");

            //return ColorTranslator.ToHtml(c);
        }

        Color ColorTodRgbString(string strHex)
        {
            return System.Drawing.ColorTranslator.FromHtml(strHex);
        }
        //--

        private void ItemAdd(
            )
        {
            try
            {
                DataTable dt = fpsDefectType_Sheet.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row["EDITMODE"] = INSERT_FLAG;
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void ItemSave(
            )
        {
            try
            {
                SEMConfiguration oSEMConfig = null;
                fpsDefectType_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

                DataTable dt = fpsDefectType_Sheet.DataSource as DataTable;
                DataRow[] drs = dt.Select(String.Format("[EDITMODE] = '{0}'", UPDATE_FLAG));
                DataTable updatedt = drs.CopyToDataTable<DataRow>();

                if (drs.Length > 0)
                {
                    oSEMConfig = new SEMConfiguration();
                    //oSEMConfig.UpdateDefectType(updatedt, DACrux.Base.GlobalVariable.UserID);

                    GetData();
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void ItemSearch(
            )
        {
            int startRow = 0, foundRow = 0;
            int startCol = 0, foundCol = -1;

            try
            {
                //--

                fpsDefectType.Search(
                    fpsDefectType.ActiveSheetIndex,
                    txtSearch.Text,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    startRow,
                    startCol,
                    ref foundRow,
                    ref foundCol
                    );
                fpsDefectType_Sheet.SetActiveCell(
                    foundRow,
                    foundCol
                    );
                fpsDefectType.ShowRow(
                    0,
                    foundRow,
                    FarPoint.Win.Spread.VerticalPosition.Nearest
                    );
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        //--

        private void ItemRemove(
            )
        {
            try
            {
                DataTable dt = fpsDefectType_Sheet.DataSource as DataTable;
                DataRow row = dt.Rows[fpsDefectType_Sheet.ActiveRowIndex];
                row["EDITMODE"] = DELETE_FLAG;
                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void SetColumnStyle(
            )
        {
            try
            {
                CheckBoxCellType chkCellType = new CheckBoxCellType();
                TextCellType txtCellType = new TextCellType();

                fpsDefectType_Sheet.Columns[2, 3].CellType = txtCellType;
                fpsDefectType_Sheet.Columns[4].CellType = chkCellType;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        Color ColorDialog(Color orig)
        {
            ColorDialog cd = null;
            try
            {
                cd = new ColorDialog();
                cd.Color = picBoxTypeColor.BackColor;
                if (cd.ShowDialog(this) != DialogResult.OK) return orig;
                return cd.Color;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cd != null) cd.Dispose();
                cd = null;
            }
        }
        #endregion Method

        

    }
}
