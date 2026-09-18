using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewSplit : Form
    {
        #region " ENUM "

        public enum ConditionType
        {
            None,
            ColumnCondition,
            SelectedColumn,
            SelectedRow,
            SelectedRange,
            SelectedRowNumber,
            SelectedRandomSample
        }

        public enum MethodType
        {
            None,
            Include,
            Exclude
        }

        #endregion

        #region " MEMBER FIELD "

        private List<DataView.ColumnInfo> lstColumnInfo = null;
        private DataTable dataSource = null;
        private DataTable resultDataSource = null;

        FarPoint.Win.Spread.CellType.ComboBoxCellType cboValidColumn;
        FarPoint.Win.Spread.CellType.ComboBoxCellType cboOperationString;
        FarPoint.Win.Spread.CellType.ComboBoxCellType cboOperationNumber;
        FarPoint.Win.Spread.CellType.ComboBoxCellType cboAndOr;

        private const int INDEX_CBO_COLUMN = 0;
        private const int INDEX_CBO_OPERATION = 1;
        private const int INDEX_TXT_VALUE = 2;
        private const int INDEX_CBO_AND_OR = 3;

        private List<string> lstValidColumnID;
        private readonly string[] arrOperationListString = new string[] { "=", "<>", "<", ">", "<=", ">=", "LIKE" };
        private readonly string[] arrOperationListNumber = new string[] { "=", "<>", "<", ">", "<=", ">=" };
        private readonly string[] arrAndOrList = new string[] { "AND", "OR" };

        FarPoint.Win.Spread.CellType.ButtonCellType btnN;
        FarPoint.Win.Spread.CellType.ButtonCellType btnC;
        FarPoint.Win.Spread.CellType.ButtonCellType btnD;

        #endregion

        #region " PROPERTY "

        public DataTable ResultDataSource
        {
            get
            {
                return resultDataSource;
            }
        }

        public ConditionType Condition
        {
            get
            {
                if (tabSplit.SelectedIndex == 1)
                    return ConditionType.ColumnCondition;
                else
                {
                    if (rbtSelectedColumn.Checked)
                        return ConditionType.SelectedColumn;
                    else if (rbtSelectedRows.Checked)
                        return ConditionType.SelectedRow;
                    else if (rbtSelectedRange.Checked)
                        return ConditionType.SelectedRange;
                    else if (rbtRowNumbers.Checked)
                        return ConditionType.SelectedRowNumber;
                    else if (rbtRandomSample.Checked)
                        return ConditionType.SelectedRandomSample;
                    else
                        return ConditionType.None;
                }
            }
        }

        public MethodType Method
        {
            get
            {
                if (rbtInclude.Checked)
                    return MethodType.Include;
                else if (rbtExclude.Checked)
                    return MethodType.Exclude;
                else
                    return MethodType.None;
            }
        }

        #endregion

        #region " CREATOR "

        public DlgDataViewSplit(List<DataView.ColumnInfo> validColumnInfo, DataTable dataSource)
        {
            InitializeComponent();

            InitSpread(validColumnInfo, dataSource);
        }

        #endregion

        #region " METHOD "

        #region [ Row Condition ]

        private bool ValidateRowNumbers(string strRowNumbers, out string ErrMsg)
        {
            string strInput = strRowNumbers.Trim();
            ErrMsg = String.Empty;

            if (strRowNumbers.Length == 0)
            {
                ErrMsg = "Not valid format.";
                //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG05");
                return false;
            }

            System.Collections.ArrayList arrList = new System.Collections.ArrayList();

            string[] arrNumbers = strInput.Split(',');
            foreach (string str in arrNumbers)
            {
                if (str.Trim().Length == 0)
                {
                    ErrMsg = "Not valid format.";
                    //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG05");
                    return false;
                }

                // "integer" 또는 "integer1" + "-" + "integer2"의 형태
                string[] arrNum = str.Split('-');

                if (arrNum.Length > 2)
                {
                    ErrMsg = "Not valid format.";
                    //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG06");
                    return false;
                }

                // 정수로 변환될수 있을까??
                for (int i = 0; i < arrNum.Length; i++)
                {
                    try
                    {
                        int iTemp = Convert.ToInt32(arrNum[i]);
                        arrList.Add(iTemp);
                    }
                    catch
                    {
                        ErrMsg = "Not valid format.";
                        //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG07");
                        //ErrMsg = "Row Numbers에 정수가 아닌 문자가 있습니다." ;
                        return false;
                    }
                }

                // "integer1" + "-" + "integer2"일 경우 integer2는 integer1보다 커야함
                if (arrNum.Length == 2)
                {
                    int iTemp1 = Convert.ToInt32(arrNum[0]);
                    int iTemp2 = Convert.ToInt32(arrNum[1]);
                    if (iTemp2 <= iTemp1)
                    {
                        ErrMsg = "Not valid format.";
                        //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG08");
                        return false;
                    }
                }

                for (int i = 0; i < arrList.Count - 1; i++)
                {
                    if ((int)arrList[i] >= (int)arrList[i + 1])
                    {
                        ErrMsg = "Not valid format.";
                        //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG09");
                        return false;
                    }
                }

                if (Convert.ToInt32(arrList[0]) < 1)
                {
                    ErrMsg = "Not valid format.";
                    //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG10");
                    return false;
                }
                if (Convert.ToInt32(arrList[arrList.Count - 1]) > dataSource.Rows.Count)
                {
                    ErrMsg = "Not valid format.";
                    //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG11");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateSampling(int spreadRowCount, object objSampling, out string ErrMsg)
        {
            ErrMsg = String.Empty;
            double dbValue = 0;
            int iRowCount = 0;
            int iSpreadRowCount = spreadRowCount;

            if (!double.TryParse(objSampling.ToString(), out dbValue))
            {
                ErrMsg = "Not valid format.";
                //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG12");
                return false;
            }

            if (dbValue <= 0)
            {
                ErrMsg = "Specified value is too small.";
                //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG13");
                return false;
            }

            if (dbValue >= 1)
            {
                iRowCount = Convert.ToInt32(Math.Floor(dbValue));
            }
            else
            {
                iRowCount = Convert.ToInt32(Math.Floor(iSpreadRowCount * dbValue));
            }

            if (iRowCount >= iSpreadRowCount)
            {
                ErrMsg = "Specified row count is more than sheet row count.";
                //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG14");
                return false;
            }
            else if (iRowCount <= 0)
            {
                ErrMsg = "Row count can not be a minus value.";
                //ErrMsg = DACrux.Interface.ILanguages.getResourceString("DATASHEET_SUBSETWND_DIALOG_MSG15");
                return false;
            }

            return true;
        }

        public DataTable GetDataTableFromRowNumbers(DataTable dt, string strRowNumbers, bool bInclude)
        {
            DataTable dtSource = dt;
            DataTable dtTarget = new DataTable();

            if (bInclude)
            {
                dtTarget = dtSource.Clone();

                string[] arrNumbers = strRowNumbers.Split(',');
                foreach (string str in arrNumbers)
                {
                    string[] arrNum = str.Split('-');
                    if (arrNum.Length == 1)
                    {
                        dtTarget.ImportRow(dtSource.Rows[Convert.ToInt32(arrNum[0]) - 1]);
                    }
                    else
                    {
                        for (int iRow = Convert.ToInt32(arrNum[0]) - 1; iRow < Convert.ToInt32(arrNum[1]); iRow++)
                        {
                            dtTarget.ImportRow(dtSource.Rows[iRow]);
                        }
                    }
                }
                dtTarget.AcceptChanges();
            }
            else
            {
                dtTarget = dtSource.Copy();

                string[] arrNumbers = strRowNumbers.Split(',');
                for (int i = arrNumbers.Length - 1; i >= 0; i--)
                {
                    string[] arrNum = arrNumbers[i].Split('-');

                    if (arrNum.Length == 1)
                    {
                        dtTarget.Rows.Remove(dtTarget.Rows[Convert.ToInt32(arrNum[0]) - 1]);
                    }
                    else
                    {
                        for (int iRow = Convert.ToInt32(arrNum[1]); iRow >= Convert.ToInt32(arrNum[0]); iRow--)
                        {
                            dtTarget.Rows.Remove(dtTarget.Rows[iRow - 1]);
                        }
                    }

                }

                dtTarget.AcceptChanges();
            }
            return dtTarget;
        }

        public DataTable GetDataTableFromSampling(DataTable dt, object objSampling, bool bInclude)
        {
            int iRowCount = 0;
            double dblValue = Convert.ToDouble(objSampling);
            DataTable dtSource = dt;
            DataTable dtTarget = dtSource.Clone();
            dtTarget.TableName = null; // Table명 삭제

            int iRealRowCount = 0;

            int iSpreadRowCount = dtSource.Rows.Count;

            if (dblValue >= 1)
            {
                iRowCount = Convert.ToInt32(Math.Floor(dblValue));
            }
            else
            {
                iRowCount = Convert.ToInt32(Math.Floor(iSpreadRowCount * dblValue));
            }

            // iRowCount 만큼 랜덤한 개수를 가져옵니다.
            if (bInclude)
            {
                iRealRowCount = iRowCount;
            }
            else
            {
                iRealRowCount = iSpreadRowCount - iRowCount;
            }

            DataTable dtList = new DataTable();
            DataTable dtSampling = new DataTable();

            // insert columns
            dtList.Columns.Add("Col1", typeof(int));
            dtSampling.Columns.Add("Col1", typeof(int));

            dtList.AcceptChanges();
            dtSampling.AcceptChanges();


            //Trace.WriteLine( "어레이 리스트 만들기 시작 : " + DateTime.Now.ToString() ) ;
            for (int i = 0; i < iSpreadRowCount; i++)
            {
                DataRow dr = dtList.NewRow();
                dr[0] = i;
                dtList.Rows.Add(dr);
            }

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            //Trace.WriteLine( "Random start : " + DateTime.Now.ToString() ) ;

            while (dtSampling.Rows.Count < iRealRowCount)
            {
                int index = random.Next(dtList.Rows.Count);
                dtSampling.ImportRow(dtList.Rows[index]);
                dtList.Rows[index].Delete();
            }
            dtList.AcceptChanges();
            dtSampling.AcceptChanges();

            foreach (DataRow dr in dtSampling.Rows)
            {
                dtTarget.ImportRow(dtSource.Rows[(int)dr[0]]);
            }
            dtTarget.AcceptChanges();

            return dtTarget;
        }

        #endregion

        #region [ Column Condition ]

        private void InitSpread(List<DataView.ColumnInfo> validColumnInfo, DataTable dataSource)
        {
            System.Resources.ResourceManager rm;

            try
            {
                this.lstColumnInfo = validColumnInfo;
                this.dataSource = dataSource;

                fpSpread_Sheet.RowCount = 1;
                fpSpread_Sheet.ColumnCount = 4;

                fpSpread_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
                fpSpread_Sheet.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
                fpSpread_Sheet.RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
                fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Vertical;
                fpSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
                fpSpread_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
                fpSpread_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Cell;
                fpSpread_Sheet.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.None;
                fpSpread.EditModePermanent = true;
                fpSpread_Sheet.DataAutoSizeColumns = false;
                fpSpread_Sheet.DataAutoCellTypes = false;
                fpSpread.SelectionRenderer = null;
                fpSpread.FocusRenderer = null;
                fpSpread_Sheet.StartingRowNumber = 1;
                fpSpread_Sheet.RowHeader.ColumnCount = 1;
                fpSpread_Sheet.RowHeader.Columns[0].Width = 20;

                cboValidColumn = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cboOperationString = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cboOperationNumber = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cboAndOr = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                SetComboColumnList();
                SetComboNumberOperationList();
                SetComboStringOperationList();
                SetComboAndOrList();

                fpSpread_Sheet.Columns[INDEX_CBO_COLUMN].Label = "Column";
                fpSpread_Sheet.Columns[INDEX_CBO_OPERATION].Label = "Operation";
                fpSpread_Sheet.Columns[INDEX_TXT_VALUE].Label = "Value";
                fpSpread_Sheet.Columns[INDEX_CBO_AND_OR].Label = "And/Or";

                fpSpread_Sheet.Columns[INDEX_CBO_COLUMN].CellType = cboValidColumn;

                fpSpread_Sheet.Columns[INDEX_CBO_OPERATION].Locked = true;
                fpSpread_Sheet.Columns[INDEX_TXT_VALUE].Locked = true;
                fpSpread_Sheet.Columns[INDEX_CBO_AND_OR].Locked = true;

                fpSpread_Sheet.Columns[INDEX_CBO_COLUMN].Width = fpSpread_Sheet.Columns[INDEX_CBO_COLUMN].GetPreferredWidth() + 20;
                fpSpread_Sheet.Columns[INDEX_CBO_OPERATION].Width = 70;
                fpSpread_Sheet.Columns[INDEX_TXT_VALUE].Width = 100;
                fpSpread_Sheet.Columns[INDEX_CBO_AND_OR].Width = 60;

                rm = new System.Resources.ResourceManager("DACrux.ProjectManager.UI.DataViewImage", this.GetType().Assembly);

                btnN = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnC = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnD = new FarPoint.Win.Spread.CellType.ButtonCellType();

                btnN.Picture = (Image)rm.GetObject("NUM_C");
                btnC.Picture = (Image)rm.GetObject("CHR_C");
                btnD.Picture = (Image)rm.GetObject("DTM_C");

                fpSpread.ComboCloseUp += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSpread_ComboCloseUp);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void SetCommonSettingComboCellType(FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType)
        {
            comboBoxCellType.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
            comboBoxCellType.Editable = false;
            comboBoxCellType.MaxDrop = comboBoxCellType.Items.Length;
            comboBoxCellType.ListAlignment = FarPoint.Win.ListAlignment.Left;
        }

        private void SetComboColumnList()
        {
            lstValidColumnID = new List<string>();
            for (int i = 0; i < lstColumnInfo.Count; i++)
            {
                lstValidColumnID.Add(lstColumnInfo[i].ColumnID);
            }
            cboValidColumn.Items = lstValidColumnID.ToArray();

            SetCommonSettingComboCellType(cboValidColumn);
        }

        private void SetComboStringOperationList()
        {
            cboOperationString.Items = arrOperationListString;

            SetCommonSettingComboCellType(cboOperationString);
        }

        private void SetComboNumberOperationList()
        {
            cboOperationNumber.Items = arrOperationListNumber;

            SetCommonSettingComboCellType(cboOperationNumber);
        }

        private void SetComboAndOrList()
        {
            cboAndOr.Items = arrAndOrList;

            SetCommonSettingComboCellType(cboAndOr);
        }

        private string GetFilterString()
        {
            string strReturn = string.Empty;

            string colName = string.Empty;
            string operation = string.Empty;
            string value = string.Empty;
            string andOr = string.Empty;

            try
            {
                for (int i = 0; i < fpSpread_Sheet.RowCount; i++)
                {
                    colName = fpSpread_Sheet.Cells[i, INDEX_CBO_COLUMN].Text.Replace(" - C", "").Replace(" - S", "").Replace(" - D", "");
                    operation = fpSpread_Sheet.Cells[i, INDEX_CBO_OPERATION].Text;
                    value = fpSpread_Sheet.Cells[i, INDEX_TXT_VALUE].Text;
                    andOr = this.fpSpread_Sheet.Cells[i, INDEX_CBO_AND_OR].Text;

                    if (colName == "")
                        break;

                    if (i < fpSpread_Sheet.RowCount - 1 && fpSpread_Sheet.Cells[i + 1, INDEX_CBO_COLUMN].Text == "")
                        andOr = "";

                    if (fpSpread_Sheet.RowHeader.Cells[i, 0].CellType == btnC)
                        value = "'" + value + "'";

                    strReturn += " " + colName + " " + operation + " " + value + " " + andOr;
                }
            }
            catch (Exception)
            {
                return "";
            }

            return strReturn;
        }

        private void SetNameRow(DataTable dataSource, ref DataTable resultDataSource)
        {
            DataRow dr = resultDataSource.NewRow();
            resultDataSource.Rows.InsertAt(dr, 0);

            for (int i = 0; i < dataSource.Columns.Count; i++)
            {
                dr[i] = dataSource.Rows[0][i];
            }

            resultDataSource.AcceptChanges();
        }

        private void ConvertDataTable(bool isNormalDataTable, ref DataTable dtSource, List<DataView.ColumnInfo> columnInfo)
        {
            System.Data.DataColumn column;
            double nTemp;
            DateTime dTemp;

            int colIndex = 0;
            string colName = string.Empty;

            try
            {
                if (isNormalDataTable)
                {
                    for (int i = 0; i < columnInfo.Count; i++)
                    {
                        colIndex = lstColumnInfo[i].ColumnIndex;
                        colName = dataSource.Columns[colIndex].ColumnName;

                        if (lstColumnInfo[i].DataType == DataType.NUMBER)
                        {
                            column = dtSource.Columns.Add("TEMP_" + colName, typeof(double));

                            for (int j = 0; j < dtSource.Rows.Count; j++)
                            {
                                if (dtSource.Rows[j][colIndex] != null)
                                {
                                    if (double.TryParse(dtSource.Rows[j][colIndex].ToString(), out nTemp))
                                    {
                                        dtSource.Rows[j]["TEMP_" + colName] = nTemp;
                                    }
                                }
                            }

                            dtSource.Columns.RemoveAt(colIndex);
                            dtSource.Columns["TEMP_" + colName].ColumnName = colName;
                            dtSource.Columns[colName].SetOrdinal(colIndex);
                        }
                        else if (lstColumnInfo[i].DataType == DataType.DATETIME)
                        {
                            column = dtSource.Columns.Add("TEMP_" + colName, typeof(DateTime));

                            for (int j = 0; j < dtSource.Rows.Count; j++)
                            {
                                if (dtSource.Rows[j][colIndex] != null)
                                {
                                    if (DateTime.TryParse(dtSource.Rows[j][colIndex].ToString(), out dTemp))
                                    {
                                        dtSource.Rows[j]["TEMP_" + colName] = dTemp;
                                    }
                                }
                            }

                            dtSource.Columns.RemoveAt(colIndex);
                            dtSource.Columns["TEMP_" + colName].ColumnName = colName;
                            dtSource.Columns[colName].SetOrdinal(colIndex);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < columnInfo.Count; i++)
                    {
                        colIndex = lstColumnInfo[i].ColumnIndex;
                        colName = dataSource.Columns[colIndex].ColumnName;

                        column = dtSource.Columns.Add("TEMP_" + colName, typeof(string));

                        for (int j = 0; j < dtSource.Rows.Count; j++)
                        {
                            if (dtSource.Rows[j][colIndex] != null)
                            {
                                dtSource.Rows[j]["TEMP_" + colName] = dtSource.Rows[j][colIndex].ToString();
                            }
                        }

                        dtSource.Columns.RemoveAt(colIndex);
                        dtSource.Columns["TEMP_" + colName].ColumnName = colName;
                        dtSource.Columns[colName].SetOrdinal(colIndex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #endregion

        #region " EVENT HANDLER "

        #region [ Common ]

        private void DlgDataViewSplit_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(this.txtRowNumbers, "Specify Rownumbers. eg) 3, 4, 2-10 ");
            toolTip.SetToolTip(this.nudRandomSample, "Specify Sampling rate/number. eg) Sampling rate : 0 < x < 1, Sampling number : x > 1 ");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string errMsg;
            DataTable dt = null;

            try
            {
                Cursor = Cursors.WaitCursor;
                
                if (tabSplit.SelectedIndex == 0)
                {
                    #region [ Assertion ]

                    if (!(rbtSelectedColumn.Checked | rbtSelectedRows.Checked | rbtSelectedRange.Checked | rbtRowNumbers.Checked | rbtRandomSample.Checked))
                    {
                        MessageBox.Show("Please check select method.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (rbtRowNumbers.Checked && !ValidateRowNumbers(txtRowNumbers.Text, out errMsg))
                    {
                        MessageBox.Show(errMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (rbtRandomSample.Checked && !ValidateSampling(dataSource.Rows.Count, nudRandomSample.Value, out errMsg))
                    {
                        MessageBox.Show(errMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (!(rbtInclude.Checked | rbtExclude.Checked))
                    {
                        MessageBox.Show("Please check whether include selected condition or exclude.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    #endregion

                    #region [ Process (RowNumber, Sampling) ]

                    if (rbtRowNumbers.Checked | rbtRandomSample.Checked)
                    {
                        dt = dataSource.Copy();
                        dt.Rows[0].Delete();
                        dt.AcceptChanges();

                        ConvertDataTable(true, ref dt, lstColumnInfo);

                        if (rbtRowNumbers.Checked)
                            resultDataSource = GetDataTableFromRowNumbers(dt, txtRowNumbers.Text, rbtInclude.Checked);
                        else if (rbtRandomSample.Checked)
                        {
                            int maxValidRowIndex = 0;
                            for (int i = 0; i < lstColumnInfo.Count; i++)
                            {
                                if (maxValidRowIndex < lstColumnInfo[i].ValidRowIndex)
                                    maxValidRowIndex = lstColumnInfo[i].ValidRowIndex;
                            }

                            for (int i = dt.Rows.Count - 1; i > maxValidRowIndex; i--)
                                dt.Rows.RemoveAt(i);

                            dt.AcceptChanges();
                            resultDataSource = GetDataTableFromSampling(dt, nudRandomSample.Value, rbtInclude.Checked);
                        }

                        ConvertDataTable(false, ref resultDataSource, lstColumnInfo);
                        SetNameRow(dataSource, ref resultDataSource);
                    }

                    #endregion
                }
                else
                {
                    #region [ Assertion ]

                    for (int i = 0; i < fpSpread_Sheet.RowCount - 1; i++)
                    {
                        for (int j = 0; j < fpSpread_Sheet.ColumnCount; j++)
                        {
                            if (fpSpread_Sheet.Cells[i, j].Text == "")
                            {
                                MessageBox.Show("Not valid condition exists.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    if (fpSpread_Sheet.Cells[fpSpread_Sheet.RowCount - 1, INDEX_CBO_COLUMN].Text != "")
                    {
                        if (fpSpread_Sheet.Cells[fpSpread_Sheet.RowCount - 1, INDEX_CBO_OPERATION].Text == "" || fpSpread_Sheet.Cells[fpSpread_Sheet.RowCount - 1, INDEX_TXT_VALUE].Text == "")
                        {
                            MessageBox.Show("Not valid condition exists.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    #endregion

                    #region [ Filtering Expression ]

                    string filter = GetFilterString();
                    if (filter == "")
                    {
                        MessageBox.Show("Condition is not valid", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    #endregion

                    #region [ Process Split ]

                    try
                    {
                        this.Cursor = Cursors.WaitCursor;

                        dt = dataSource.Copy();
                        dt.Rows[0].Delete();
                        dt.AcceptChanges();

                        ConvertDataTable(true, ref dt, lstColumnInfo);

                        System.Data.DataView dv = dt.DefaultView;
                        dv.RowFilter = filter;
                        dt = dv.ToTable();
                        dt.AcceptChanges();

                        if (dt.Rows.Count < 1)
                        {
                            MessageBox.Show("No Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        ConvertDataTable(false, ref dt, lstColumnInfo);

                        SetNameRow(dataSource, ref dt);

                        resultDataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }

                    #endregion
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region [ Row Condition ]

        private void rbtRowNumbers_CheckedChanged(object sender, EventArgs e)
        {
            txtRowNumbers.Enabled = rbtRowNumbers.Checked;
        }

        private void rbtRandomSample_CheckedChanged(object sender, EventArgs e)
        {
            nudRandomSample.Enabled = rbtRandomSample.Checked;
        }

        private void rbtSelectedRange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtSelectedRange.Checked)
            {
                rbtInclude.Checked = true;
                rbtExclude.Enabled = false;
            }
            else
            {
                rbtExclude.Enabled = true;
            }
        }

        #endregion

        #region [ Column Condition ]

        void fpSpread_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == INDEX_CBO_COLUMN)
            {
                if (fpSpread_Sheet.Cells[e.Row, e.Column].Text == "")
                    return;

                int index = lstValidColumnID.IndexOf(fpSpread_Sheet.Cells[e.Row, e.Column].Text);

                switch (lstColumnInfo[index].DataType)
                {
                    case DataType.NUMBER:
                        fpSpread_Sheet.RowHeader.Cells[e.Row, 0].CellType = btnN;
                        fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPERATION].CellType = cboOperationNumber;
                        break;
                    case DataType.TEXT:
                        fpSpread_Sheet.RowHeader.Cells[e.Row, 0].CellType = btnC;
                        fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPERATION].CellType = cboOperationString;
                        break;
                    case DataType.DATETIME:
                        fpSpread_Sheet.RowHeader.Cells[e.Row, 0].CellType = btnD;
                        fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPERATION].CellType = cboOperationNumber;
                        break;
                }

                fpSpread_Sheet.Cells[e.Row, INDEX_CBO_AND_OR].CellType = cboAndOr;
                fpSpread_Sheet.Cells[e.Row, INDEX_CBO_OPERATION, e.Row, INDEX_CBO_AND_OR].Locked = false;
            }
            else if (e.Column == INDEX_CBO_AND_OR)
            {
                if (fpSpread_Sheet.Cells[e.Row, e.Column].Text == "" || e.Row < fpSpread_Sheet.RowCount - 1)
                    return;

                fpSpread_Sheet.AddRows(fpSpread_Sheet.RowCount, 1);
            }
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (fpSpread_Sheet.RowCount < 2)
                return;

            fpSpread_Sheet.Rows.Remove(fpSpread_Sheet.ActiveRow.Index, 1);
        }

        private void tabSplit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabSplit.SelectedIndex == 1)
                btnDeleteRow.Visible = true;
            else
                btnDeleteRow.Visible = false;
        }

        #endregion


        #endregion
    }
}