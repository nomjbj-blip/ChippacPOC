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
    public partial class frmDefectTypeGroup_Property : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region [ Construtor ]
        private enum ColumnIndex { GROUP_ID = 0, GROUP_NAME, DELETE_FLAG, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER }
        private static readonly string ADMIN = "ADMIN";
        public frmDefectTypeGroup_Property(
            )
        {
            InitializeComponent();
        }

        #endregion [ Construtor ]

        //--------------------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void frmDefectTypeGroup_Property_Load(
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

            if (btn == btnSearch)
                ItemSearch();
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

                FarPoint.Win.Spread.Model.CellRange[] oSelectItem = fpsDefectGroup.ActiveSheet.GetSelections();

                int idx = -1;
                for (int i = 0; i < oSelectItem.Length; i++)
                {
                    for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                    {
                        idx = oSelectItem[i].Row + cr;
                        fpsDefectGroup.ActiveSheet.Cells[idx, 5].Value = ColorTranslator.ToHtml(picBoxTypeColor.BackColor);
                        fpsDefectGroup.ActiveSheet.Cells[idx, 5].BackColor = System.Drawing.ColorTranslator.FromHtml(fpsDefectGroup.ActiveSheet.Cells[idx, 5].Value.ToString());
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

            picBoxTypeColor.BackColor = fpsDefectGroup.ActiveSheet.Cells[e.Range.Row, 5].BackColor;
        }

        //--

        private void grid_CommandButtonClick(
            object sender,
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            RO.SEMConfiguration oSemConfig = new SEMConfiguration();
            string groupid = grid.GetValue(Enum.GetName(typeof(ColumnIndex), ColumnIndex.GROUP_ID)).ToString();
            bool exists = oSemConfig.ExistsDefectGroup(
                groupid
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
                        oSemConfig.InsertDefectGroup(grid.GetDictionaryValue(), ADMIN);
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
                        oSemConfig.UpdateDefectGroup(grid.GetDictionaryValue(), ADMIN);
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
                        oSemConfig.DeleteDefectGroup(groupid);
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
            BtnReSearch.PerformClick();
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

            Utility.FPSpreadUtil.InitSpread(fpsDefectGroup);
            Application.DoEvents();

            oSEMConfig = new SEMConfiguration();
            dt = oSEMConfig.SelectGroupList();
            if (dt == null)
                return;

            Utility.FPSpreadUtil.SetSpreadData(dt, fpsDefectGroup_Sheet1);
            Utility.FPSpreadUtil.VisibleSpreadColumns(fpsDefectGroup_Sheet1, new int[] { (int)ColumnIndex.DELETE_FLAG }, false);
            Utility.FPSpreadUtil.SetAutoColumnSort(fpsDefectGroup_Sheet1);
            Utility.FPSpreadUtil.SetAutoColumnFilter(fpsDefectGroup_Sheet1);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpsDefectGroup_Sheet1);
            grid.DataSource = dt;
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

                fpsDefectGroup.Search(
                    fpsDefectGroup.ActiveSheetIndex,
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
                fpsDefectGroup_Sheet1.SetActiveCell(
                    foundRow,
                    foundCol
                    );
                fpsDefectGroup.ShowRow(
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

                fpsDefectGroup_Sheet1.Columns[2, 3].CellType = txtCellType;
                fpsDefectGroup_Sheet1.Columns[4].CellType = chkCellType;
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
