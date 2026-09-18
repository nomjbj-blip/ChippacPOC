using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewColumnSetting : Form
    {
        #region " MEMBER FIELD "

        private List<DataView.ColumnInfo> lstColumnInfo = null;
        private List<int> ChangedColumnIndices = new List<int>();
        private DataTable dataSource = null;

        private bool isColumnSettingChanged = false;
        private string strBefore = "";

        private const int INDEX_TXT_NAME = 0;
        private const int INDEX_BTN_NUMBER = 1;
        private const int INDEX_BTN_TEXT = 2;
        private const int INDEX_BTN_DATETIME = 3;
        private const int INDEX_CBO_FORMAT = 4;
        private const int INDEX_CBO_OPTION = 5;
        private const int INDEX_NUM_DECIMAL = 6;

        private const int MAXIMUM_DECIMAL_PLACE = 5;

        FarPoint.Win.Spread.CellType.ButtonCellType btnN;
        FarPoint.Win.Spread.CellType.ButtonCellType btnC;
        FarPoint.Win.Spread.CellType.ButtonCellType btnD;

        FarPoint.Win.Spread.CellType.ButtonCellType btnN_G;
        FarPoint.Win.Spread.CellType.ButtonCellType btnC_G;
        FarPoint.Win.Spread.CellType.ButtonCellType btnD_G;

        FarPoint.Win.Spread.CellType.ComboBoxCellType cboFormat;
        FarPoint.Win.Spread.CellType.ComboBoxCellType cboOption;
        FarPoint.Win.Spread.CellType.NumberCellType numDecimalPlace;

        #endregion

        #region " PROPERTY "

        public bool IsColumnSettingChanged
        {
            get
            {
                return isColumnSettingChanged;
            }
            set
            {
                isColumnSettingChanged = value;
                btnApply.Enabled = isColumnSettingChanged;
            }
        }

        #endregion

        #region " CREATOR "

        public DlgDataViewColumnSetting(List<DataView.ColumnInfo> columnInfo, DataTable dataSource)
        {
            InitializeComponent();

            InitSpread(columnInfo, dataSource);
        }

        #endregion

        #region " METHOD "

        private void InitSpread(List<DataView.ColumnInfo> columnInfo, DataTable dataSource)
        {
            try
            {
                this.lstColumnInfo = columnInfo;
                this.dataSource = dataSource;

                fpSpread_Sheet.RowCount = lstColumnInfo.Count;

                fpSpread_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
                fpSpread_Sheet.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
                fpSpread_Sheet.RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
                fpSpread.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                fpSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                fpSpread_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
                fpSpread_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Cell;
                fpSpread_Sheet.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.None;
                fpSpread.EditModePermanent = false;
                fpSpread_Sheet.DataAutoSizeColumns = false;
                fpSpread_Sheet.DataAutoCellTypes = false;
                fpSpread.SelectionRenderer = null;
                fpSpread.FocusRenderer = null;
                fpSpread_Sheet.StartingRowNumber = 1;

                btnN = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnC = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnD = new FarPoint.Win.Spread.CellType.ButtonCellType();

                btnN_G = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnC_G = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnD_G = new FarPoint.Win.Spread.CellType.ButtonCellType();

                cboFormat = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cboOption = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                numDecimalPlace = new FarPoint.Win.Spread.CellType.NumberCellType();

                System.Resources.ResourceManager rm = new System.Resources.ResourceManager("DACrux.ProjectManager.UI.DataViewImage", this.GetType().Assembly);

                btnN.Picture = (Image)rm.GetObject("NUM_C");
                btnC.Picture = (Image)rm.GetObject("CHR_C");
                btnD.Picture = (Image)rm.GetObject("DTM_C");

                btnN_G.Picture = (Image)rm.GetObject("NUM_G");
                btnC_G.Picture = (Image)rm.GetObject("CHR_G");
                btnD_G.Picture = (Image)rm.GetObject("DTM_G");

                cboFormat.Items = DataView.ColumnInfo.DateTimeFormat;
                cboOption.Items = DataView.ColumnInfo.RoundOption;
                numDecimalPlace.SpinButton = true;
                numDecimalPlace.DecimalPlaces = 0;
                numDecimalPlace.MinimumValue = 0;
                numDecimalPlace.MaximumValue = MAXIMUM_DECIMAL_PLACE;

                for (int i = 0; i < lstColumnInfo.Count; i++)
                {
                    fpSpread_Sheet.RowHeader.Cells[i, 0].Value = "C" + (lstColumnInfo[i].ColumnIndex + 1).ToString();
                    fpSpread_Sheet.Cells[i, INDEX_TXT_NAME].Value = lstColumnInfo[i].ColumnName;
                    ChangeTypeButton(i, lstColumnInfo[i].DataType, true);
                }
                fpSpread_Sheet.RowHeader.Columns[0].Width = fpSpread_Sheet.RowHeader.Columns[0].GetPreferredWidth() + 20;

                fpSpread.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread_CellClick);
                fpSpread.EditModeStarting += new FarPoint.Win.Spread.EditModeStartingEventHandler(fpSpread_EditModeStarting);
                fpSpread.EditModeOff += new EventHandler(fpSpread_EditModeOff);
                fpSpread.ComboCloseUp += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSpread_ComboCloseUp);
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        void fpSpread_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if(!ChangedColumnIndices.Contains(e.Row))
                ChangedColumnIndices.Add(e.Row);
            
            if (e.Row >= 0 && e.Column == INDEX_CBO_OPTION)
            {
                if (fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPTION].Text == DataView.ColumnInfo.RoundOption[0])
                {
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].CellType = null;
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].Value = null;
                }
                else
                {
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].CellType = numDecimalPlace;
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].Value = lstColumnInfo[e.Row].DecimalPlace;
                }
            }
        }

        private void GetSelectedSetting(int rowIndex, out string name, out DataType dataType, out int dateTimeFormatIndex, out int roundOptionIndex, out int decimalPlace)
        {
            name = fpSpread_Sheet.Cells[rowIndex, INDEX_TXT_NAME].Text;

            if (fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_NUMBER].CellType == btnN)
            {
                dataType = DataType.NUMBER;
                dateTimeFormatIndex = -1;
                roundOptionIndex = DataView.ColumnInfo.GetIndexOf(DataView.ColumnInfo.RoundOption, fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Text);
                decimalPlace = Convert.ToInt32(fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value);
            }
            else if (fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_TEXT].CellType == btnC)
            {
                dataType = DataType.TEXT;
                dateTimeFormatIndex = -1;
                roundOptionIndex = -1;
                decimalPlace = -1;
            }
            else if (fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_DATETIME].CellType == btnD)
            {
                dataType = DataType.DATETIME;
                dateTimeFormatIndex = DataView.ColumnInfo.GetIndexOf(DataView.ColumnInfo.DateTimeFormat, fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Text);
                roundOptionIndex = -1;
                decimalPlace = -1;
            }
            else
            {
                dataType = lstColumnInfo[rowIndex].DataType;
                dateTimeFormatIndex = lstColumnInfo[rowIndex].DateTimeFormatIndex;
                roundOptionIndex = lstColumnInfo[rowIndex].RoundOptionIndex;
                decimalPlace = lstColumnInfo[rowIndex].DecimalPlace;
            }
        }

        private void ChangeTypeButton(int rowIndex, DataType dataType)
        {
            ChangeTypeButton(rowIndex, dataType, true);
        }

        private void ChangeTypeButton(int rowIndex, DataType dataType, bool isGetData)
        {
            if (!ChangedColumnIndices.Contains(rowIndex))
                ChangedColumnIndices.Add(rowIndex);

            switch (dataType)
            {
                case DataType.NUMBER:
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_NUMBER].CellType = btnN;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_TEXT].CellType = btnC_G;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_DATETIME].CellType = btnD_G;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].CellType = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].CellType = cboOption;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].CellType = numDecimalPlace;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Locked = true;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Locked = false;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Locked = false;

                    if (isGetData)
                    {
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Value = null;
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Value = DataView.ColumnInfo.RoundOption[lstColumnInfo[rowIndex].RoundOptionIndex];
                        if (lstColumnInfo[rowIndex].RoundOptionIndex != 0)
                            fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value = lstColumnInfo[rowIndex].DecimalPlace;
                    }
                    else
                    {
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Value = null;
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Value = DataView.ColumnInfo.RoundOption[0];
                        fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value = null;
                    }

                    fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Value = fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Text.Replace(" - C", "").Replace(" - D", "");
                    break;

                case DataType.TEXT:
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_NUMBER].CellType = btnN_G;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_TEXT].CellType = btnC;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_DATETIME].CellType = btnD_G;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].CellType = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].CellType = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].CellType = null;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Locked = true;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Locked = true;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Locked = true;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Value = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Value = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value = null;

                    fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Value = fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Text.Replace(" - D", "") + " - C";
                    break;

                case DataType.DATETIME:
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_NUMBER].CellType = btnN_G;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_TEXT].CellType = btnC_G;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_BTN_DATETIME].CellType = btnD;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].CellType = cboFormat;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].CellType = null;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].CellType = null;

                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Locked = false;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Locked = true;
                    fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Locked = true;

                    if (isGetData)
                    {
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Value = DataView.ColumnInfo.DateTimeFormat[lstColumnInfo[rowIndex].DateTimeFormatIndex];
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Value = null;
                        fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value = null;
                    }
                    else
                    {
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_FORMAT].Value = DataView.ColumnInfo.DateTimeFormat[0];
                        fpSpread_Sheet.Cells[rowIndex, INDEX_CBO_OPTION].Value = null;
                        fpSpread_Sheet.Cells[rowIndex, INDEX_NUM_DECIMAL].Value = null;
                    }

                    fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Value = fpSpread_Sheet.RowHeader.Cells[rowIndex, 0].Text.Replace(" - C", "") + " - D";
                    break;
            }
        }

        private bool CheckValidColumn(DataTable dataSource, int columnIndex, DataType dataType)
        {
            string strTemp;
            
            try
            {
                switch (dataType)
                {
                    case DataType.NUMBER:
                        double nTemp = 0;
                        for (int i = 1; i <= lstColumnInfo[columnIndex].ValidRowIndex; i++)
                        {
                            strTemp = dataSource.Rows[i][columnIndex].ToString().Trim();
                            if (strTemp != "" && strTemp != "*" && !double.TryParse(strTemp, out nTemp))
                                return false;
                        }
                        break;
                    case DataType.DATETIME:
                        DateTime dTemp;
                        for (int i = 1; i <= lstColumnInfo[columnIndex].ValidRowIndex; i++)
                        {
                            strTemp = dataSource.Rows[i][columnIndex].ToString().Trim();
                            if (strTemp != "" && strTemp != "*" && !DateTime.TryParse(strTemp, out dTemp))
                                return false;
                        }
                        break;
                    case DataType.TEXT:
                        return true;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void ApplyChange()
        {
            DataType dataType;
            string strName;
            int iDateTimeFormatIndex;
            int iRoundOptionIndex;
            int iDecimalPlace;
            bool isChanged;

            try
            {
                foreach (int i in ChangedColumnIndices)
                {
                    isChanged = false;

                    GetSelectedSetting(i, out strName, out dataType, out iDateTimeFormatIndex, out iRoundOptionIndex, out iDecimalPlace);

                    if (lstColumnInfo[i].ColumnName != strName)
                    {
                        lstColumnInfo[i].ColumnName = strName;
                        lstColumnInfo[i].ApplyColumnName();
                    }

                    if (iDateTimeFormatIndex >= 0 && lstColumnInfo[i].DateTimeFormatIndex != iDateTimeFormatIndex)
                    {
                        lstColumnInfo[i].DateTimeFormatIndex = iDateTimeFormatIndex;
                        isChanged = true;
                    }

                    if (iRoundOptionIndex >= 0 && lstColumnInfo[i].RoundOptionIndex != iRoundOptionIndex)
                    {
                        lstColumnInfo[i].RoundOptionIndex = iRoundOptionIndex;
                        isChanged = true;
                    }

                    if (iDecimalPlace >= 0 && lstColumnInfo[i].DecimalPlace != iDecimalPlace)
                    {
                        lstColumnInfo[i].DecimalPlace = iDecimalPlace;
                        isChanged = true;
                    }

                    if (lstColumnInfo[i].DataType != dataType)
                    {
                        lstColumnInfo[i].DataType = dataType;
                        isChanged = true;
                    }

                    if (isChanged)
                        lstColumnInfo[i].ApplyColumnSetting();
                }

                ChangedColumnIndices.Clear();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " EVENT HANDLER "

        private void fpSpread_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!e.ColumnHeader && e.Column >= INDEX_BTN_NUMBER && e.Column <= INDEX_BTN_DATETIME)
            {
                bool isValid = false;
                string strMessage = "This column has incompatible values with the selected type. Do you want to lose those values?";

                switch (e.Column)
                {
                    case INDEX_BTN_NUMBER:
                        if (fpSpread_Sheet.Cells[e.Row, e.Column].CellType == btnN)
                            return;

                        isValid = CheckValidColumn(dataSource, e.Row, DataType.NUMBER);
                        if (isValid || (!isValid && MessageBox.Show(strMessage, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK))
                        {
                            ChangeTypeButton(e.Row, DataType.NUMBER);
                            IsColumnSettingChanged = true;
                        }
                        break;

                    case INDEX_BTN_TEXT:
                        if (fpSpread_Sheet.Cells[e.Row, e.Column].CellType == btnC)
                            return;

                        ChangeTypeButton(e.Row, DataType.TEXT);
                        IsColumnSettingChanged = true;
                        break;

                    case INDEX_BTN_DATETIME:
                        if (fpSpread_Sheet.Cells[e.Row, e.Column].CellType == btnD)
                            return;

                        isValid = CheckValidColumn(dataSource, e.Row, DataType.DATETIME);
                        if (isValid || (!isValid && MessageBox.Show(strMessage, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK))
                        {
                            ChangeTypeButton(e.Row, DataType.DATETIME);
                            IsColumnSettingChanged = true;
                        }
                        break;
                }
            }
        }

        private void fpSpread_Sheet_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            if(!ChangedColumnIndices.Contains(e.Row))
                ChangedColumnIndices.Add(e.Row);

            if (e.Row >= 0 && e.Column == INDEX_CBO_OPTION)
            {
                if (fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPTION].Text == DataView.ColumnInfo.RoundOption[0])
                {
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].CellType = null;
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].Value = null;
                }
                else
                {
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].CellType = numDecimalPlace;
                    fpSpread_Sheet.Cells[e.Row, INDEX_NUM_DECIMAL].Value = lstColumnInfo[e.Row].DecimalPlace;
                }
            }
        }

        private void fpSpread_EditModeStarting(object sender, FarPoint.Win.Spread.EditModeStartingEventArgs e)
        {
            strBefore = fpSpread_Sheet.ActiveCell.Text;
        }

        private void fpSpread_EditModeOff(object sender, EventArgs e)
        {
            if (strBefore != fpSpread_Sheet.ActiveCell.Text)
                IsColumnSettingChanged = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApplyChange();

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyChange();

            IsColumnSettingChanged = false;
        }

        #endregion
    }
}