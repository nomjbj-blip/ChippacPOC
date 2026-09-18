using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.TEST.RO;
using DACrux.Framework.Controls;
using FarPoint.Win.Spread.CellType;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSetupBin : DACrux.Framework.Base.DACruxUXBasic01
    {
        private enum COLUMN { PROGRAM = 0, BIN, BIN_NAME, CHAR_BIN, HIGH_GEC, DISPLAY, UPPER_LIMIT_CNT, LOWER_LIMIT_CNT, COLOR, DESCRIPTION, EDITMODE }
        private readonly string UPDATE = "UPDATE";

        //--

        public frmSetupBin()
        {
            InitializeComponent();
        }

        #region Event Handler

        private void frmSetupBin_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;
            Utility.FPSpreadUtil.InitSpread(fpsBinInfo);
            GetTestArea();
        }

        //--

        private void btnCopy_Click(
            object sender,
            EventArgs e
            )
        {
            String testarea = duclbTestArea.SelectedValue as String;
            if (String.IsNullOrEmpty(testarea))
                return;
            String program = duclbProgram.SelectedValue as String;
            if (String.IsNullOrEmpty(program))
                return;

            DlgBinCopy dlg = new DlgBinCopy(
                testarea,
                program
                );
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            if (MessageBox.Show(String.Format("{0} → {1} Copy 합니다. 계속하시겠습니까?", dlg.SourceProgram, dlg.TargetProgram), "COPY", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.OK)
                return;

            ProbeAdmin obj = new ProbeAdmin();
            obj.CreateCopyBin(
                DACrux.Base.GlobalVariable.Factory,
                testarea,
                dlg.SourceProgram,
                dlg.TargetProgram,
                DACrux.Base.GlobalVariable.UserID
                );

            btnSearch.PerformClick();
        }

        //--

        private void btnNewProgram_Click(
            object sender,
            EventArgs e
            )
        {
            dlgSetupProgram dlg = new dlgSetupProgram();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
        }

        //--

        private void btnSave_Click(
            object sender, 
            EventArgs e
            )
        {
            ItemSave();
        }

        //--

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            string strProgram = string.Empty;
            if (duclbProgram == null || duclbProgram.SelectedValue == null)
                return;

            strProgram = duclbProgram.SelectedValue.ToString();

            GetBinInfo(
                strProgram
                );
        }

        //--

        private void duclbTestArea_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbTestArea.SelectedItem == null)
                return;

            GetProgram(
            duclbTestArea.SelectedValue
            );

        }

        //--

        private void duclbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbTestArea.SelectedItem == null)
                return;

            GetProgram(
                duclbTestArea.SelectedValue
                );
        }

        private void duclbProgram_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbProgram.SelectedItem == null)
                return;

            GetBinInfo(
                duclbProgram.SelectedValue
                );
        }

        //--

        private void fpsBinInfo_CellClick(
            object sender,
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
        }

        //--

        private void fpsBinInfo_CellDoubleClick(
            object sender, 
            FarPoint.Win.Spread.CellClickEventArgs e
            )
        {
            switch (e.Column)
            { 
                case (int)COLUMN.PROGRAM:
                case (int)COLUMN.BIN:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        //--

        private void fpsBinInfo_EditChange(
            object sender, 
            FarPoint.Win.Spread.EditorNotifyEventArgs e
            )
        {
            fpsBinInfo_Sheet.Cells[fpsBinInfo_Sheet.ActiveRowIndex, (int)COLUMN.EDITMODE].Value = UPDATE;
        }

        //--

        private void fpsBinInfo_EditModeOff(
            object sender, 
            EventArgs e
            )
        {
        }

        //--

        private void fpsBinInfo_EditModeOn(
            object sender, 
            EventArgs e
            )
        {
        }

        //--

        private void fpsBinInfo_SelectionChanged(
            object sender, 
            FarPoint.Win.Spread.SelectionChangedEventArgs e
            )
        {
            switch (e.Range.Column)
            { 
                case (int)COLUMN.COLOR:
                    fpsBinInfo_Sheet.Cells[e.Range.Row, (int)COLUMN.COLOR].BackColor
                        = pbBinColor.BackColor
                        = ColorDialog(fpsBinInfo_Sheet.Cells[e.Range.Row, (int)COLUMN.COLOR].BackColor);
                    fpsBinInfo_Sheet.Cells[e.Range.Row, (int)COLUMN.COLOR].Value = System.Drawing.ColorTranslator.ToHtml(fpsBinInfo_Sheet.Cells[e.Range.Row, (int)COLUMN.COLOR].BackColor);
                    fpsBinInfo_Sheet.Cells[e.Range.Row, (int)COLUMN.EDITMODE].Value = UPDATE;
                    break;
                default:
                    break;
            }
        }

        //--

        //private void grid_CommandButtonClick(
        //    object sender,
        //    Framework.PropertyGrid.CommandEventArgs e
        //    )
        //{
        //    ProbeAdmin obj = new ProbeAdmin();
        //    string program = grid.GetValue(Enum.GetName(typeof(COLUMN), COLUMN.PROGRAM)).ToString();
        //    string bin = grid.GetValue(Enum.GetName(typeof(COLUMN), COLUMN.BIN)).ToString();

        //    bool exists = obj.ExistsBinInfo(
        //        program,
        //        bin
        //        );
        //    DialogResult result;
        //    switch (e.Mode)
        //    {
        //        case Framework.PropertyGrid.CommandMode.Insert:
        //            if (exists)
        //            {
        //                MessageBox.Show("해당 데이터가 이미 존재합니다.");
        //                e.Cancel = true;
        //                return;
        //            }

        //            //--

        //            result = MessageBox.Show("저장하시겠습니까?", "입력", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == System.Windows.Forms.DialogResult.OK)
        //            {
        //                obj.InsertBinInfo(
        //                    grid.GetDictionaryValue(),
        //                    DACrux.Base.GlobalVariable.UserID
        //                    );
        //            }
        //            else
        //            {
        //                e.Cancel = true;
        //            }
        //            break;
        //        case Framework.PropertyGrid.CommandMode.Update:
        //            if (!exists)
        //            {
        //                MessageBox.Show("업데이트할 데이터가 없습니다.");
        //                e.Cancel = true;
        //                return;
        //            }

        //            //--

        //            result = MessageBox.Show("변경하시겠습니까", "수정", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == System.Windows.Forms.DialogResult.OK)
        //            {
        //                obj.UpdateBinInfo(
        //                    grid.GetDictionaryValue(),
        //                    DACrux.Base.GlobalVariable.UserID
        //                    );
        //            }
        //            else
        //            {
        //                e.Cancel = true;
        //            }
        //            break;
        //        case Framework.PropertyGrid.CommandMode.Delete:
        //            if (!exists)
        //            {
        //                MessageBox.Show("삭제할 데이터가 없습니다.");
        //                e.Cancel = true;
        //                return;
        //            }

        //            result = MessageBox.Show("삭제하시겠습니까", "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == System.Windows.Forms.DialogResult.OK)
        //            {
        //                obj.DeleteBinInfo(
        //                    program,
        //                    bin,
        //                    DACrux.Base.GlobalVariable.UserID
        //                    );
        //            }
        //            else
        //            {
        //                e.Cancel = true;
        //            }
        //            break;
        //        case Framework.PropertyGrid.CommandMode.None:
        //        default:
        //            break;
        //    }
        //}

        //private void grid_CommandComplete(
        //    object sender,
        //    Framework.PropertyGrid.CommandEventArgs e
        //    )
        //{
        //    btnSearch.PerformClick();
        //}

        //private void grid_DropDownComboBox(
        //    object sender,
        //    Framework.PropertyGrid.DropDownComboBoxEventArgs e
        //    )
        //{
        //    if (String.Equals(e.PropertyName, Enum.GetName(typeof(COLUMN), COLUMN.HIGH_GEC))
        //        || String.Equals(e.PropertyName, Enum.GetName(typeof(COLUMN), COLUMN.DISPLAY)))
        //    {
        //        e.ComboBoxValueList.Add(Boolean.TrueString);
        //        e.ComboBoxValueList.Add(Boolean.FalseString);
        //    }
        //}

        //private void grid_StateChanged(
        //    object sender,
        //    Framework.PropertyGrid.StateChangedEventArgs e
        //    )
        //{
        //    if (e.State == Framework.PropertyGrid.State.Insert)
        //    {
        //        grid.SetValue(Enum.GetName(typeof(COLUMN), COLUMN.PROGRAM), duclbProgram.SelectedValue.ToString());
        //    }
        //}

        private void pbBinColor_Click(
            object sender,
            EventArgs e
            )
        {
            pbBinColor.BackColor = ColorDialog(pbBinColor.BackColor);
        }
        #endregion Event Handler

        #region Method

        private Color ColorDialog(
            Color orig
            )
        {
            ColorDialog cdlg = new ColorDialog();
            cdlg.Color = pbBinColor.BackColor;
            if (cdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return orig;

            return cdlg.Color;
        }

        //--

        private void GetTestArea(
            )
        {
            ProbeAdmin oProbeAdmin = null;
            DataTable dt = null;
            try
            {
                oProbeAdmin = new ProbeAdmin();
                dt = oProbeAdmin.GetTestAreaList();
                duclbTestArea.DisplayMember = "TESTAREA";
                duclbTestArea.ValueMember = "TESTAREA";
                duclbTestArea.DataSource = dt;
                duclbTestArea.DataBinding();
            }
            finally { }
        }

        //--

        private void GetProgram(
            object testarea
            )
        {
            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = null;
            dt = oProbeAdmin.GetTestProgramList(
                testarea.ToString(),
                string.Empty
                );
            duclbProgram.DisplayMember = "PROGRAM";
            duclbProgram.ValueMember = "PROGRAM";
            duclbProgram.DataSource = dt;
            duclbProgram.DataBinding();
        }

        private void GetBinInfo(
            object program
            )
        {
            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = null;
            dt = oProbeAdmin.GetBinListEditable(
                program.ToString()
                );
            dt.TableName = program.ToString();
            Utility.FPSpreadUtil.InitSpread(fpsBinInfo);
            fpsBinInfo_Sheet.RowCount = 151;
            fpsBinInfo_Sheet.ColumnCount = 11;
            fpsBinInfo_Sheet.Columns[(int)COLUMN.EDITMODE].Label = Enum.GetName(typeof(COLUMN), COLUMN.EDITMODE);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.PROGRAM].Label = Enum.GetName(typeof(COLUMN), COLUMN.PROGRAM);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.BIN].Label = Enum.GetName(typeof(COLUMN), COLUMN.BIN);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.BIN_NAME].Label = Enum.GetName(typeof(COLUMN), COLUMN.BIN_NAME);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.CHAR_BIN].Label = Enum.GetName(typeof(COLUMN), COLUMN.CHAR_BIN);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.HIGH_GEC].Label = Enum.GetName(typeof(COLUMN), COLUMN.HIGH_GEC);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.DISPLAY].Label = Enum.GetName(typeof(COLUMN), COLUMN.DISPLAY);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.UPPER_LIMIT_CNT].Label = Enum.GetName(typeof(COLUMN), COLUMN.UPPER_LIMIT_CNT);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.LOWER_LIMIT_CNT].Label = Enum.GetName(typeof(COLUMN), COLUMN.LOWER_LIMIT_CNT);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.COLOR].Label = Enum.GetName(typeof(COLUMN), COLUMN.COLOR);
            fpsBinInfo_Sheet.Columns[(int)COLUMN.DESCRIPTION].Label = Enum.GetName(typeof(COLUMN), COLUMN.DESCRIPTION);

            for (int idx = 0; idx < fpsBinInfo_Sheet.RowCount; idx++)
            {
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.PROGRAM].Value = program;
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.BIN].Value = idx;

                DataRow[] rows = dt.Select(String.Format("[BIN] = '{0}'", idx));
                if (rows == null || rows.Length <= 0)
                    continue;

                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.BIN_NAME].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.BIN_NAME)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.CHAR_BIN].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.CHAR_BIN)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.HIGH_GEC].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.HIGH_GEC)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.DISPLAY].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.DISPLAY)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.UPPER_LIMIT_CNT].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.UPPER_LIMIT_CNT)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.LOWER_LIMIT_CNT].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.LOWER_LIMIT_CNT)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.COLOR].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.COLOR)].ToString();
                fpsBinInfo_Sheet.Cells[idx, (int)COLUMN.DESCRIPTION].Value = rows[0][Enum.GetName(typeof(COLUMN), COLUMN.DESCRIPTION)].ToString();
            }
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpsBinInfo_Sheet);

            if (dt != null)
            {
                //grid.DataSource = dt;
                SetColumnStyle();
                SpreadBackColor();
            }
        }

        private string GetBooleanValueToFlag(
            string sValue
            )
        {
            return String.Equals(sValue, bool.TrueString) ? "Y" : "N";
        }


        private void ItemSave()
        {
            List<String[]> saveBins = new List<string[]>();
            for (int rowIdx = 0; rowIdx < fpsBinInfo_Sheet.RowCount; rowIdx++)
            {
                if (String.IsNullOrEmpty(fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.EDITMODE].Text))
                    continue;

                String[] values = new String[10];
                //values[(int)COLUMN.EDITMODE] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.EDITMODE].Text;
                values[(int)COLUMN.PROGRAM] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.PROGRAM].Text;
                values[(int)COLUMN.BIN] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.BIN].Text;
                values[(int)COLUMN.BIN_NAME] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.BIN_NAME].Text;
                values[(int)COLUMN.CHAR_BIN] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.CHAR_BIN].Text;
                values[(int)COLUMN.HIGH_GEC] = GetBooleanValueToFlag(fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.HIGH_GEC].Text);
                values[(int)COLUMN.DISPLAY] = GetBooleanValueToFlag(fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.DISPLAY].Text);
                values[(int)COLUMN.UPPER_LIMIT_CNT] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.UPPER_LIMIT_CNT].Text;
                values[(int)COLUMN.LOWER_LIMIT_CNT] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.LOWER_LIMIT_CNT].Text;
                values[(int)COLUMN.COLOR] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.COLOR].Text;
                values[(int)COLUMN.DESCRIPTION] = fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.DESCRIPTION].Text;
                saveBins.Add(values);
            }

            if (saveBins == null || saveBins.Count <= 0)
                return;

            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            int iResult = oProbeAdmin.MergeBinInfo(saveBins);
        }

        private void SetColumnStyle()
        {
            ComboBoxCellType comboCellType = new ComboBoxCellType();
            comboCellType.Items = new String[] { bool.FalseString, bool.TrueString };
            comboCellType.ItemData = new String[] { bool.FalseString, bool.TrueString };

            //--

            NumberCellType numCellType = new NumberCellType();
            numCellType.DecimalPlaces = 10;
            numCellType.MaximumValue = 99999999999999;
            numCellType.MinimumValue = -99999999999999;

            //--

            TextCellType txtCellType = new TextCellType();

            //--

            fpsBinInfo_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
            fpsBinInfo_Sheet.Columns[(int)COLUMN.EDITMODE].Visible = false;
            fpsBinInfo_Sheet.Columns[(int)COLUMN.PROGRAM].CellType = txtCellType; // program
            fpsBinInfo_Sheet.Columns[(int)COLUMN.BIN].CellType = txtCellType; // bin
            fpsBinInfo_Sheet.Columns[(int)COLUMN.BIN_NAME].CellType = txtCellType; // bin desc
            fpsBinInfo_Sheet.Columns[(int)COLUMN.CHAR_BIN].CellType = txtCellType; // bin desc
            fpsBinInfo_Sheet.Columns[(int)COLUMN.HIGH_GEC].CellType = comboCellType; // high gec
            fpsBinInfo_Sheet.Columns[(int)COLUMN.DISPLAY].CellType = comboCellType; // display
            fpsBinInfo_Sheet.Columns[(int)COLUMN.UPPER_LIMIT_CNT].CellType = numCellType; // upper limit count
            fpsBinInfo_Sheet.Columns[(int)COLUMN.LOWER_LIMIT_CNT].CellType = numCellType; // lower limit count
            //fpsBinInfo_Sheet.Columns[(int)COLUMN.COLOR].CellType = colorCellType; // color
            fpsBinInfo_Sheet.Columns[(int)COLUMN.DESCRIPTION].CellType = txtCellType; // description
        }

        private void SpreadBackColor(
            )
        {
            for (int rowIdx = 0; rowIdx < fpsBinInfo_Sheet.Rows.Count; rowIdx++)
            {
                Color color;
                if (fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.COLOR].Value == null)
                    continue;

                color = DACrux.Utility.Util.HexConverter(fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.COLOR].Value.ToString());
                fpsBinInfo_Sheet.Cells[rowIdx, (int)COLUMN.COLOR].BackColor = color;
            }
        }
        #endregion Method
    }
}
