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
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectTypeDefine_Property : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region [ Construtor ]
        private enum ColumnIndex { CLASSNUMBER = 0, NAME, DESCRIPTION, GROUP_ID, DELETE_FLAG, DEFECT_COLOR, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER }
        public frmDefectTypeDefine_Property(
            )
        {
            InitializeComponent();
        }

        #endregion [ Construtor ]

        //--------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void frmDefectTypeDefine_Property_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

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
                if (btn == btnSearch)
                    ItemSearch();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
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

        private void BtnReSearch_Click(
            object sender,
            EventArgs e
            )
        {
            GetData();
        }

        //--

        private void picBoxTypeColor_Click(
            object sender,
            EventArgs e
            )
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
                        fpsDefectType.ActiveSheet.Cells[idx, 5].Value = ColorTranslator.ToHtml(picBoxTypeColor.BackColor);
                        fpsDefectType.ActiveSheet.Cells[idx, 5].BackColor = System.Drawing.ColorTranslator.FromHtml(fpsDefectType.ActiveSheet.Cells[idx, 5].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        //--

        private void fpsDefectType_SelectionChanged(
            object sender,
            FarPoint.Win.Spread.SelectionChangedEventArgs e
            )
        {
            if (e.Range.Row == -1)
                return;

            picBoxTypeColor.BackColor = fpsDefectType.ActiveSheet.Cells[e.Range.Row, 5].BackColor;
        }

        //--

        private void grid_CommandButtonClick(
            object sender,
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            RO.SEMConfiguration oSemConfig = new SEMConfiguration();
            string classnumber = grid.GetValue("CLASSNUMBER").ToString();
            bool exists = oSemConfig.ExistsDefectType(
                classnumber
                );


            DialogResult result;
            switch (e.Mode)
            {
                case Framework.PropertyGrid.CommandMode.Insert:
                    if (exists)
                    {
                        MessageBox.Show("해당 데이터가 이미 존재합니다.");
                        e.Cancel = true;
                        return;
                    }

                    result = MessageBox.Show("저장하시겠습니까?", "Insert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                        oSemConfig.InsertDefectByType(grid.GetDictionaryValue(), GlobalVariable.UserID, GlobalVariable.UserID);
                    else
                        e.Cancel = true;
                    break;
                case Framework.PropertyGrid.CommandMode.Update:
                    if (!exists)
                    {
                        MessageBox.Show("업데이트할 데이터가 없습니다.");
                        e.Cancel = true;
                        return;
                    }

                    result = MessageBox.Show("변경하시겠습니까?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                        oSemConfig.UpdateDefectByType(grid.GetDictionaryValue(), GlobalVariable.UserID, GlobalVariable.UserID);
                    else
                        e.Cancel = true;
                    break;
                case Framework.PropertyGrid.CommandMode.Delete:
                    if (!exists)
                    {
                        MessageBox.Show("삭제할 데이터가 없습니다.");
                        e.Cancel = true;
                        return;
                    }

                    result = MessageBox.Show("삭제하시겠습니까?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                        oSemConfig.DeleteDefectByType(classnumber);
                    else
                        e.Cancel = true;
                    break;
                case Framework.PropertyGrid.CommandMode.None:
                default:
                    break;
            }
        }

        //--

        private void grid_CommandComplete(
            object sender,
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        //--

        private void grid_DropDownComboBox(
            object sender,
            Framework.PropertyGrid.DropDownComboBoxEventArgs e
            )
        {
            SEMConfiguration obj = new SEMConfiguration();
            DataTable dt = null;

            if (String.Equals(e.PropertyName, "GROUP_ID"))
            {
                dt = obj.SelectGroupList();
                foreach (DataRow row in dt.Rows)
                {
                    e.ComboBoxValueList.Add(String.Format("{0}({1})", row["GROUP_NAME"].ToString(), row["GROUP_ID"]));
                }
            }
        }

        //--

        private void grid_StateChanged(
            object sender,
            Framework.PropertyGrid.StateChangedEventArgs e
            )
        {
            if (e.State == Framework.PropertyGrid.State.Insert)
            {
                grid.SetValue("DELETE_FLAG", "N");
            }
        }

        //--

        #endregion [ Event Handler ]

        //--------------------------------------------------------------------------------------------

        #region [ Method ]

        //-- 

        Color ColorDialog(
            Color orig
            )
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

        //--

        private void GetData(
            )
        {
            SEMConfiguration oSEMConfig = null;
            DataTable dt = null;

            try
            {
                Utility.FPSpreadUtil.InitSpread(fpsDefectType);
                Application.DoEvents();

                oSEMConfig = new SEMConfiguration();
                dt = oSEMConfig.GetDefectTypeList();
                if (dt == null)
                    return;

                Utility.FPSpreadUtil.SetSpreadData(dt, fpsDefectType_Sheet);
                Utility.FPSpreadUtil.VisibleSpreadColumns(fpsDefectType_Sheet, new int[] { (int)ColumnIndex.DELETE_FLAG }, false);
                Utility.FPSpreadUtil.SetAutoColumnSort(fpsDefectType_Sheet);
                Utility.FPSpreadUtil.SetAutoColumnFilter(fpsDefectType_Sheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpsDefectType_Sheet);
                grid.DataSource = dt;

                if (dt != null && dt.Rows.Count > 0)
                    SetColumnStyle();

                for (int ir = 0; ir < fpsDefectType_Sheet.RowCount; ir++)
                {
                    fpsDefectType_Sheet.Cells[ir, 5].BackColor = System.Drawing.ColorTranslator.FromHtml(fpsDefectType_Sheet.Cells[ir, 5].Value.ToString());
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

        //-- 

        private bool VaildationData(
            bool exists
            )
        {
            if (exists)
            {
                MessageBox.Show("해당 데이터가 이미 존재합니다.");
                return false;
            }

            return true;
        }

        #endregion [ Method ]
    }
}
