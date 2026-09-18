using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Infragistics.Win.UltraWinToolbars;
using DACrux.Base;

namespace DACruxV5
{
    public partial class FrmMain : Form
    {
        public static readonly Col[] REQUIRED_FILEDS;

        private UltraToolbarHelper _helper;

        static FrmMain()
        {
            REQUIRED_FILEDS = new Col[]
            {
                Col.MENU_KEY,
                Col.POPUP_MENU,
                Col.CAPTION001,
                Col.CAPTION002,
                Col.CAPTION003,
                Col.TOOL_TIP,
                Col.MNU_ORDER
            };
        }

        public FrmMain()
        {
            InitializeComponent();
        }

        #region 이벤트 메서드

        private void Form1_Load(object sender, EventArgs e)
        {
            _helper = new UltraToolbarHelper(menu);
            _helper.MainToolDisplay = MainMenuDisplayType.RibbonGroupFirst;
            _helper.DisableRibbonMenu = SettingData.DisableRibbonMenu;
            _helper.AppStyle = SettingData.AppStyle;
            
            txtPath.Text = Properties.Settings.Default.MenuPath;
            LoadXmlFile(txtPath.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MenuPath = txtPath.Text;
            Properties.Settings.Default.Save();

            SettingData.QuickAccessTools = _helper.GetQuickAccessTools();
            SettingData.DisableRibbonMenu = _helper.DisableRibbonMenu;
            SettingData.Save();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.FileName = txtPath.Text;
                dlg.Filter = "Menu File|*.mnu";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtPath.Text = dlg.FileName;
                    LoadXmlFile(dlg.FileName);
                }
            }
        }

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadXmlFile(txtPath.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtPath.Text))
            {
                txtPath.Focus();
                MessageBox.Show("파일명이 없습니다.");
                return;
            }

            if (MessageBox.Show("메뉴 파일을 저장하시겠습니까?", "저장", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                != System.Windows.Forms.DialogResult.Yes)
                return;

            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt != null)
            {
                dt.WriteXml(txtPath.Text, XmlWriteMode.WriteSchema);
                dt.AcceptChanges();
                BindingSpread(dt);
            }
        }

        private void btnNewRow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("선택된 Row가 있는 경우 Row 추가 시 MENU_KEY, ICON32, ICON16, RESV_05, ENABLE, VISIBLE, IS_START, MULTI 값을 복사합니다.");

            DataTable dt = (fpSpread1.DataSource) as DataTable;

            if (dt != null)
            {
                int selectedIndex = fpSpread1_Sheet1.ActiveRowIndex;

                fpSpread1_Sheet1.Models.ResetViewRowIndexes();
                fpSpread1_Sheet1.ClearRowFilter();

                dt.Rows.Add(dt.NewRow());
                fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.RowCount - 1, 0);
                fpSpread1.ShowActiveCell(FarPoint.Win.Spread.VerticalPosition.Nearest, FarPoint.Win.Spread.HorizontalPosition.Nearest);

                int index = fpSpread1_Sheet1.ActiveRowIndex;

                // Set Default Value
                fpSpread1_Sheet1.Cells[index, (int)Col.ENABLE].Value = true;
                fpSpread1_Sheet1.Cells[index, (int)Col.VISIBLE].Value = true;
                fpSpread1_Sheet1.Cells[index, (int)Col.IS_START].Value = false;
                fpSpread1_Sheet1.Cells[index, (int)Col.MULTI].Value = false;

                if (selectedIndex >= 0)
                {
                    fpSpread1_Sheet1.Cells[index, (int)Col.POPUP_MENU].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.POPUP_MENU].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.ICON32].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.ICON32].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.ICON16].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.ICON16].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.RESV_05].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.RESV_05].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.ENABLE].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.ENABLE].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.VISIBLE].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.VISIBLE].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.IS_START].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.IS_START].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.MULTI].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.MULTI].Value;
                    fpSpread1_Sheet1.Cells[index, (int)Col.FORM_TYPE].Value = fpSpread1_Sheet1.Cells[selectedIndex, (int)Col.FORM_TYPE].Value;
                    SetCellImage(index);
                }
            }
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (fpSpread1_Sheet1.ActiveRowIndex < 0)
                return;

            if (MessageBox.Show("선택한 Row를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != System.Windows.Forms.DialogResult.Yes)
                return;

            fpSpread1_Sheet1.Rows.Remove(fpSpread1_Sheet1.ActiveRowIndex, 1);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (!CheckData(dt))
                return;

            BindingSpread(dt);
        }

        private void LoadXmlFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return;

            if (!File.Exists(fileName))
                return;

            DataTable dt = new DataTable("Table1");
            dt.ReadXml(fileName);
            dt.AcceptChanges();

            BindingSpread(dt);
        }

        private void BindingSpread(DataTable dt)
        {
            fpSpread1_Sheet1.DataSource = null;
            fpSpread1_Sheet1.RowCount = 0;
            fpSpread1_Sheet1.ColumnCount = 0;
            fpSpread1_Sheet1.Models.ResetViewRowIndexes();
            fpSpread1_Sheet1.ClearRowFilter();

            DACrux.Common.Utillity.SetSpreadData(dt, fpSpread1_Sheet1);
            DACrux.Common.Utillity.SetAutoColumnSort(fpSpread1_Sheet1);
            DACrux.Common.Utillity.SetAutoColumnFilter(fpSpread1_Sheet1);
            DACrux.Common.Utillity.SetAutoColumnWidth(fpSpread1_Sheet1);

            if (dt == null)
                return;

            // ICON32 IMAGE 설정
            FarPoint.Win.Spread.CellType.ImageCellType image32 = new FarPoint.Win.Spread.CellType.ImageCellType();
            fpSpread1_Sheet1.Columns.Add((int)Col.ICON32IMAGE, 1);
            fpSpread1_Sheet1.Columns[(int)Col.ICON32IMAGE].CellType = image32;

            // ICON16 IMAGE 설정
            FarPoint.Win.Spread.CellType.ImageCellType image16 = new FarPoint.Win.Spread.CellType.ImageCellType();
            fpSpread1_Sheet1.Columns.Add((int)Col.ICON16IMAGE, 1);
            fpSpread1_Sheet1.Columns[(int)Col.ICON16IMAGE].CellType = image16;

            // IMAGE BUTTON 설정
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCell = new FarPoint.Win.Spread.CellType.ButtonCellType();
            fpSpread1_Sheet1.Columns.Add((int)Col.ICON_BUTTON, 1);
            fpSpread1_Sheet1.Columns[(int)Col.ICON_BUTTON].CellType = buttonCell;

            // 기본 설정
            fpSpread1_Sheet1.Columns[(int)Col.ICON16].Locked = true;
            fpSpread1_Sheet1.Columns[(int)Col.ICON32].Locked = true;
            fpSpread1_Sheet1.Columns[(int)Col.ICON16].Width = 100;
            fpSpread1_Sheet1.Columns[(int)Col.ICON32].Width = 100;
            fpSpread1_Sheet1.Columns[(int)Col.MENU_ID].Width = 50;
            fpSpread1_Sheet1.Columns[(int)Col.ICON32IMAGE].Width = 20;
            fpSpread1_Sheet1.Columns[(int)Col.ICON16IMAGE].Width = 20;
            fpSpread1_Sheet1.Columns[(int)Col.ICON_BUTTON].Width = 30;
            fpSpread1_Sheet1.Columns[(int)Col.ICON_BUTTON].Label = " ";
            fpSpread1_Sheet1.Columns[(int)Col.ICON32IMAGE].Label = " ";
            fpSpread1_Sheet1.Columns[(int)Col.ICON16IMAGE].Label = " ";
            chkFrozen_CheckedChanged(null, null);

            _helper.Binding(fpSpread1.DataSource as DataTable);
            _helper.SetQuickAccessTool(SettingData.QuickAccessTools);

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                SetCellImage(i);
                SetRowBackColor(i);
            }
        }

        private bool CheckData(DataTable dt)
        {
            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                foreach (Col col in REQUIRED_FILEDS)
                {
                    object val = fpSpread1_Sheet1.Cells[i, (int)col].Value;

                    if (val == null || val == DBNull.Value || String.IsNullOrEmpty(val.ToString()))
                    {
                        fpSpread1.Focus();
                        fpSpread1_Sheet1.SetActiveCell(i, (int)col);
                        fpSpread1.ShowActiveCell(FarPoint.Win.Spread.VerticalPosition.Nearest, FarPoint.Win.Spread.HorizontalPosition.Nearest);
                        MessageBox.Show(String.Format("'{0}'는 필수 입력값입니다.", col));
                        return false;
                    }
                }
            }

            return true;
        }

        private void SetCellImage(int row)
        {
            fpSpread1_Sheet1.Cells[row, (int)Col.ICON32IMAGE].Value = 
                Util.GetImage(Util.StringToImage((string)fpSpread1_Sheet1.Cells[row, (int)Col.ICON32].Value), 20);

            fpSpread1_Sheet1.Cells[row, (int)Col.ICON16IMAGE].Value =
                Util.GetImage(Util.StringToImage((string)fpSpread1_Sheet1.Cells[row, (int)Col.ICON16].Value), 20);
        }

        private void SetRowBackColor(int row)
        {
            MenuItem item = _helper.MenuList[GetDataRow(row)];

            if (item != null && !item.IsActive())        // ENABLE or VISIBLE false
            {
                fpSpread1_Sheet1.Rows[row].BackColor = Color.LightGray;
            }
            else if (item != null && item.POPUP_MENU == MenuItemList.ROOT)
            {
                fpSpread1_Sheet1.Rows[row].BackColor = Color.Blue;
                fpSpread1_Sheet1.Rows[row].ForeColor = Color.White;
            }
            else if (item != null && item.RESV_05 == MenuItemList.SUB_MENU)
            {
                fpSpread1_Sheet1.Rows[row].BackColor = Color.LightBlue;
            }
            else
            {
                fpSpread1_Sheet1.Rows[row].ResetBackColor();
                fpSpread1_Sheet1.Rows[row].ResetForeColor();
            }
        }

        private DataRow GetDataRow(int row)
        {
            if (row >= 0 && fpSpread1_Sheet1.DataSource is DataTable)
                return (fpSpread1_Sheet1.DataSource as DataTable).DefaultView[row].Row;

            return null;
        }

        private void menu_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            textBox1.Text = e.Tool.Key;

            switch (e.Tool.Key)
            {
                case "MNU_WINDOWS_APPLICATION_STYLE":
                    frmWindowsStyle oWinStyle = new frmWindowsStyle();
                    oWinStyle.ShowDialog(this);
                    break;
                case UltraToolbarHelper.KEY_USING_RIBBON:
                    StateButtonTool button = e.Tool as StateButtonTool;
                    menu.Ribbon.Visible = button.Checked;
                    DACrux.Base.GlobalVariable.RibbonMenuEnable = button.Checked;
                    break;
            }

            if (_helper != null && !String.IsNullOrEmpty(e.Tool.Key))
            {
                for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                {
                    if (fpSpread1_Sheet1.Cells[i, (int)Col.MENU_KEY].Text == e.Tool.Key)
                    {
                        fpSpread1_Sheet1.SetActiveCell(i, (int)Col.MENU_KEY);
                        fpSpread1.ShowActiveCell(FarPoint.Win.Spread.VerticalPosition.Nearest, FarPoint.Win.Spread.HorizontalPosition.Nearest);
                        break;
                    }
                }
            }
        }

        private void fpSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == (int)Col.ICON_BUTTON)
            {
                using (FrmIcon frm = new FrmIcon())
                {
                    frm.Icon16X16 = fpSpread1_Sheet1.Cells[e.Row, (int)Col.ICON16].Text;
                    frm.Icon32X32 = fpSpread1_Sheet1.Cells[e.Row, (int)Col.ICON32].Text;

                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        fpSpread1_Sheet1.Cells[e.Row, (int)Col.ICON16].Value = frm.Icon16X16;
                        fpSpread1_Sheet1.Cells[e.Row, (int)Col.ICON32].Value = frm.Icon32X32;
                        SetCellImage(e.Row);
                    }
                }
            }
        }

        private void fpSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                if (e.Column == (int)Col.ICON32IMAGE || e.Column == (int)Col.ICON16IMAGE)
                {
                    if (MessageBox.Show("이미지를 모두 저장하시겠습니까?", "저장", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                        return;

                    using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                    {
                        dlg.ShowNewFolderButton = true;

                        if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            return;

                        for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                        {
                            byte[] bytes = Util.StringToBytes((string)fpSpread1_Sheet1.Cells[i, e.Column + 1].Value);

                            using (Image image = Util.BytesToImage(bytes))
                            {
                                if (image != null)
                                {
                                    string ext;
                                    System.Drawing.Imaging.ImageFormat format;

                                    Util.GetImageFormat(image, out format, out ext);
                                    string fileName = Path.Combine(dlg.SelectedPath,
                                        fpSpread1_Sheet1.Columns[e.Column + 1].Label + "_" + fpSpread1_Sheet1.Cells[i, (int)Col.MENU_KEY].Text + ext);

                                    using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                                    {
                                        fs.Write(bytes, 0, bytes.Length);
                                    }
                                }
                            }
                        }
                    }

                    MessageBox.Show("저장을 완료하였습니다.");
                }
            }
            else
            {
                // set fronzen column
                if (!chkFrozen.Checked)
                {
                    txtFrozen.Text = fpSpread1_Sheet1.Columns[e.Column].Label;
                    txtFrozen.Tag = e.Column;
                }
            }
        }

        private void chkFrozen_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkFrozen.Checked)
                fpSpread1_Sheet1.FrozenColumnCount = 0;
            else
                fpSpread1_Sheet1.FrozenColumnCount = (txtFrozen.Tag == null) ? 0 : (int)txtFrozen.Tag + 1;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (_helper.MenuList == null || dt == null || dt.Rows.Count == 0)
                return;

            using (FrmSort frm = new FrmSort())
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BindingSpread(_helper.MenuList.Sort(frm.SortType));
                }
            }
        }

        private void btnValidation_Click(object sender, EventArgs e)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (!CheckData(dt))
                return;

            if (_helper.MenuList == null)
                return;

            FrmValidation frm = new FrmValidation();
            frm.Owner = this;

            foreach (MenuItem item in _helper.MenuList.GetDuplicatedItems())
                frm.Duplicated.Items.Add(item.MENU_KEY);

            foreach (MenuItem item in _helper.MenuList.GetNotConnectedItems())
                frm.NotConnected.Items.Add(item.MENU_KEY);

            frm.Show();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (fpSpread1_Sheet1.ActiveRowIndex <= 0)
                return;

            int index = fpSpread1_Sheet1.ActiveRowIndex;

            if (String.IsNullOrEmpty(fpSpread1_Sheet1.Cells[index, (int)Col.POPUP_MENU].Text) || String.IsNullOrEmpty(fpSpread1_Sheet1.Cells[index - 1, (int)Col.POPUP_MENU].Text))
                return;

            if (fpSpread1_Sheet1.Cells[index, (int)Col.POPUP_MENU].Text != fpSpread1_Sheet1.Cells[index - 1, (int)Col.POPUP_MENU].Text)
                return;

            SwappingRow(index, index - 1);
            fpSpread1_Sheet1.SetActiveCell(index - 1, fpSpread1_Sheet1.ActiveColumnIndex);
        }

        private void btnDn_Click(object sender, EventArgs e)
        {
            if (fpSpread1_Sheet1.ActiveRowIndex >= fpSpread1_Sheet1.RowCount - 1)
                return;

            int index = fpSpread1_Sheet1.ActiveRowIndex;

            if (String.IsNullOrEmpty(fpSpread1_Sheet1.Cells[index, (int)Col.POPUP_MENU].Text) || String.IsNullOrEmpty(fpSpread1_Sheet1.Cells[index + 1, (int)Col.POPUP_MENU].Text))
                return;

            if (fpSpread1_Sheet1.Cells[index, (int)Col.POPUP_MENU].Text != fpSpread1_Sheet1.Cells[index + 1, (int)Col.POPUP_MENU].Text)
                return;

            SwappingRow(index, index + 1);
            fpSpread1_Sheet1.SetActiveCell(index + 1, fpSpread1_Sheet1.ActiveColumnIndex);
        }

        private void SwappingRow(int index1, int index2)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null)
                return;

            object tmpVal;

            foreach (int c in new int[] { (int)Col.ICON16IMAGE, (int)Col.ICON32IMAGE })
            {
                tmpVal = fpSpread1_Sheet1.Cells[index1, c].Value;
                fpSpread1_Sheet1.Cells[index1, c].Value = fpSpread1_Sheet1.Cells[index2, c].Value;
                fpSpread1_Sheet1.Cells[index2, c].Value = tmpVal;
            }
            
            object[] tmpArr = dt.Rows[index1].ItemArray;
            dt.Rows[index1].ItemArray = dt.Rows[index2].ItemArray;
            dt.Rows[index2].ItemArray = tmpArr;

            tmpVal = dt.Rows[index1][MenuItem.COLUMN_MNU_ORDER];
            dt.Rows[index1][MenuItem.COLUMN_MNU_ORDER] = dt.Rows[index2][MenuItem.COLUMN_MNU_ORDER];
            dt.Rows[index2][MenuItem.COLUMN_MNU_ORDER] = tmpVal;
        }

        #endregion
        
        private void fpSpread1_DragOver(object sender, DragEventArgs e)
        {
            Point pt = fpSpread1.PointToClient(new Point(e.X, e.Y));
            FarPoint.Win.Spread.Model.CellRange range = fpSpread1.GetCellFromPixel(0, 0, pt.X, pt.Y);

            if (range.Column == (int)Col.ICON16IMAGE || range.Column == (int)Col.ICON32IMAGE)
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void fpSpread1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point pt = fpSpread1.PointToClient(new Point(e.X, e.Y));
                FarPoint.Win.Spread.Model.CellRange range = fpSpread1.GetCellFromPixel(0, 0, pt.X, pt.Y);

                string[] arr = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (arr == null || arr.Length == 0)
                    throw new Exception("이미지 정보가 없습니다.");

                if (arr.Length != 1)
                    throw new Exception("하나의 이미지만 Drop 할 수 있습니다.");

                try
                {
                    int iconSize = range.Column == (int)Col.ICON16IMAGE ? 16 : 32;
                    Image image = Util.GetImage(Image.FromFile(arr[0]), iconSize);
                    
                    fpSpread1_Sheet1.Cells[range.Row, range.Column + 1].Value = Util.BytesToString(Util.ImageToBytes(image));
                    SetCellImage(range.Row);
                }
                catch
                {
                    throw new Exception("인식할 수 없는 이미지 파일입니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public enum Col
    {
        MENU_KEY,
        MENU_ID,
        POPUP_MENU,
        STOCK_TOOLBAR,
        CAPTION001,
        CAPTION002,
        CAPTION003,
        CAPTION004,
        CAPTION005,
        TOOL_TIP,
        ICON32IMAGE,
        ICON32,
        ICON16IMAGE,
        ICON16,
        ICON_BUTTON,
        FORM_TYPE,
        MODULE,
        ASSEMBLY,
        HELP_URL,
        MNU_ORDER,
        RESV_01,
        RESV_02,
        RESV_03,
        RESV_04,
        RESV_05,
        ENABLE,
        VISIBLE,
        IS_START,
        MULTI
    }
}
