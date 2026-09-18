using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace DACrux.ProjectManager.UI
{
    public sealed partial class DataView : UserControl
    {
        #region " MEMBER FIELD "

        private List<ColumnInfo> lstColumnInfo = new List<ColumnInfo>();
        private static ColumnInfoComparer columnInfoComparer = new ColumnInfoComparer();
        private string strBeforeEdit = string.Empty;
        private bool isControlKeyDown = false;
        private bool isInitialized = false;
        private bool isEventConnected = false;
        private bool isSplited = false;

        public static string SHEET_MISSING_VALUE = "*";
        private static readonly float COLUMN_MIN_WIDTH = 80F;

        private FarPoint.Win.Spread.Model.CellRange crCommon;
        private static FarPoint.Win.LineBorder lineBorderMultiCells = new FarPoint.Win.LineBorder(Color.Black, 3);
        private static FarPoint.Win.LineBorder lineBorderSingleCell = new FarPoint.Win.LineBorder(Color.Black, 2);
        private static FarPoint.Win.Spread.CellType.TextCellType CommonCellType = new FarPoint.Win.Spread.CellType.TextCellType();

        private static int DEFAULT_COL_COUNT = 20;
        private static int DEFAULT_ROW_COUNT = 51;

        #endregion

        #region " INNER CLASS "

        public sealed class ColumnInfo
        {
            #region " MEMBER FIELD "

            private bool isInitialized = false;

            private string columnName = string.Empty;
            private int columnIndex = 0;
            private DataType dataType = DataType.NUMBER;
            private int validRowIndex = 0;

            private int dateTimeFormatIndex = 0;
            private int roundOptionIndex = 0;
            private int decimalPlace = 0;

            private FarPoint.Win.Spread.SheetView sv = null;

            public static readonly string[] DateTimeFormat = new string[] {
              "yyyy-MM-dd"
            , "yyyy-MM-dd HH:mm:ss"
            //, "yyyyMMddHHmmss"
            //, "yyyyMMdd"
            , "yyyy'/'MM'/'dd"
            , "yyyy'/'MM'/'dd HH:mm:ss"
            };

            public static readonly string[] RoundOption = new string[] {
              "None"
            , "Round"
            , "Up"
            , "Truncate"
            };

            #endregion

            #region " PROPERTY "

            public int ColumnIndex
            {
                get { return columnIndex; }
                set { columnIndex = value; }
            }

            public string ColumnName
            {
                get { return columnName; }
                set { columnName = value; }
            }

            public string ColumnID
            {
                get
                {
                    return "C" + (columnIndex + 1).ToString() + ((dataType == DataType.NUMBER) ? "" : ((dataType == DataType.TEXT) ? " - C" : " - D"));
                }
            }

            public int ValidRowIndex
            {
                get
                {
                    return validRowIndex;
                }
                set
                {
                    validRowIndex = value;

                    if (value == 0)
                        DataType = DataType.NUMBER;
                }
            }

            public int DateTimeFormatIndex
            {
                get { return dateTimeFormatIndex; }
                set { dateTimeFormatIndex = value; }
            }

            public int RoundOptionIndex
            {
                get { return roundOptionIndex; }
                set { roundOptionIndex = value; }
            }

            public int DecimalPlace
            {
                get { return decimalPlace; }
                set { decimalPlace = value; }
            }

            public string CurrentDateTimeFormat
            {
                get
                {
                    return DateTimeFormat[dateTimeFormatIndex];
                }
            }

            public DataType DataType
            {
                get
                {
                    return dataType;
                }
                set
                {
                    dataType = value;

                    if (isInitialized && sv != null)
                    {
                        if (value == DataType.TEXT)
                        {
                            sv.Columns[ColumnIndex].Label = sv.Columns[ColumnIndex].Label.Replace(" - D", "").Replace(" - C", "") + " - C";
                            sv.Columns[ColumnIndex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        }
                        else if (value == DataType.DATETIME)
                        {
                            sv.Columns[ColumnIndex].Label = sv.Columns[ColumnIndex].Label.Replace(" - C", "").Replace(" - D", "") + " - D";
                            sv.Columns[ColumnIndex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        }
                        else if (value == DataType.NUMBER)
                        {
                            sv.Columns[ColumnIndex].Label = sv.Columns[ColumnIndex].Label.Replace(" - C", "").Replace(" - D", "");
                            sv.Columns[ColumnIndex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        }
                    }
                }
            }

            #endregion

            #region " CREATOR "

            public ColumnInfo(FarPoint.Win.Spread.SheetView sheet, int columnIndex
                , DataType dataType, string columnName
                , int dateTimeFormatIndex, int roundOptionIndex
                , int decimalPlace, int validRowIndex)
            {
                sv = sheet;
                this.columnIndex = columnIndex;

                this.dataType = dataType;
                this.columnName = columnName;

                this.dateTimeFormatIndex = dateTimeFormatIndex;
                this.roundOptionIndex = roundOptionIndex;
                this.decimalPlace = decimalPlace;

                ValidRowIndex = validRowIndex;

                isInitialized = true;
            }

            public ColumnInfo(int columnIndex
                , DataType dataType, string columnName
                , int dateTimeFormatIndex, int roundOptionIndex
                , int decimalPlace, int validRowIndex)
            {
                this.columnIndex = columnIndex;

                this.dataType = dataType;
                this.columnName = columnName;

                this.dateTimeFormatIndex = dateTimeFormatIndex;
                this.roundOptionIndex = roundOptionIndex;
                this.decimalPlace = decimalPlace;

                this.validRowIndex = validRowIndex;

                isInitialized = true;
            }

            #endregion

            #region " EVENT & DELEGATE "

            public delegate void DelColumnSettingChanged(ColumnInfo columnInfo);
            public DelColumnSettingChanged delColumnSettingChanged = null;

            public delegate void DelColumnNameChanged(int columnIndex, string columnName);
            public DelColumnNameChanged delColumnNameChanged = null;

            #endregion

            #region " METHOD "

            public static int GetIndexOf(string[] strArray, string itemName)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Equals(itemName))
                        return i;
                }
                return -1;
            }

            public void ApplyColumnSetting()
            {
                if (validRowIndex > 0 && delColumnSettingChanged != null)
                    delColumnSettingChanged(this);
            }

            public void ApplyColumnName()
            {
                if (delColumnNameChanged != null)
                    delColumnNameChanged(ColumnIndex, columnName);
            }

            public ColumnInfo Copy(NameRowPolicy nameRowPolicy)
            {
                ColumnInfo columnInfo;

                if (nameRowPolicy == NameRowPolicy.Excluded)
                    columnInfo = new ColumnInfo(this.ColumnIndex, this.dataType, this.columnName, this.dateTimeFormatIndex, this.roundOptionIndex, this.decimalPlace, this.validRowIndex - 1);
                else
                    columnInfo = new ColumnInfo(this.ColumnIndex, this.dataType, this.columnName, this.dateTimeFormatIndex, this.roundOptionIndex, this.decimalPlace, this.validRowIndex);

                return columnInfo;
            }

            #endregion
        }

        private sealed class ColumnInfoComparer : IComparer<ColumnInfo>
        {
            int IComparer<ColumnInfo>.Compare(ColumnInfo x, ColumnInfo y)
            {
                if (x.ColumnIndex > y.ColumnIndex)
                    return 1;
                else if (x.ColumnIndex == y.ColumnIndex)
                    return 0;
                else
                    return -1;
            }
        };

        private sealed class SortInfoAscComparer : System.Collections.IComparer
        {
            private double doubleX;
            private double doubleY;
            private DateTime dateTimeX;
            private DateTime dateTimeY;

            string strX;
            string strY;

            public int Compare(object x, object y)
            {
                if (x == y)
                {
                    return 0;
                }
                else if (x == null || y == null)
                {
                    if (x != null)
                        return -1;
                    else
                        return 1;
                }
                else
                {
                    strX = x.ToString();
                    strY = y.ToString();

                    if (double.TryParse(strX, out doubleX) && double.TryParse(strY, out doubleY))
                    {
                        return doubleX.CompareTo(doubleY);
                    }
                    else if (DateTime.TryParse(strX, out dateTimeX) && DateTime.TryParse(strY, out dateTimeY))
                    {
                        return dateTimeX.CompareTo(dateTimeY);
                    }
                    else
                    {
                        if (strX == DataView.SHEET_MISSING_VALUE || strY == DataView.SHEET_MISSING_VALUE)
                        {
                            if (strX != DataView.SHEET_MISSING_VALUE)
                                return -1;
                            else
                                return 1;
                        }
                        else
                        {
                            return string.Compare(strX, strY, false);
                        }
                    }
                }
            }
        };

        private sealed class SortInfoDescComparer : System.Collections.IComparer
        {
            private double doubleX;
            private double doubleY;
            private DateTime dateTimeX;
            private DateTime dateTimeY;

            string strX;
            string strY;

            public int Compare(object x, object y)
            {
                if (x == y)
                {
                    return 0;
                }
                else if (x == null || y == null)
                {
                    if (x == null)
                        return -1;
                    else
                        return 1;
                }
                else
                {
                    strX = x.ToString();
                    strY = y.ToString();

                    if (double.TryParse(strX, out doubleX) && double.TryParse(strY, out doubleY))
                    {
                        return doubleX.CompareTo(doubleY);
                    }
                    else if (DateTime.TryParse(strX, out dateTimeX) && DateTime.TryParse(strY, out dateTimeY))
                    {
                        return dateTimeX.CompareTo(dateTimeY);
                    }
                    else
                    {
                        if (strX == DataView.SHEET_MISSING_VALUE || strY == DataView.SHEET_MISSING_VALUE)
                        {
                            if (strX == DataView.SHEET_MISSING_VALUE)
                                return -1;
                            else
                                return 1;
                        }
                        else
                        {
                            return string.Compare(strX, strY, false);
                        }
                    }
                }
            }
        };

        #endregion

        #region " ENUM "

        public enum NameRowPolicy
        {
            Excluded,
            Included
        }

        public enum TrimPolicy
        {
            Both,
            Column,
            Row
        }

        #endregion

        #region " PROPERTY "

        [Browsable(false)]
        public DataTable DataSource
        {
            get
            {
                try
                {
                    if (fpSpread_Sheet.RowCount < 2)
                        return null;

                    ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();
                    DataTable dt = ((DataTable)fpSpread_Sheet.DataSource).Copy();

                    dt.Rows.RemoveAt(0);
                    dt.AcceptChanges();

                    List<ColumnInfo> lstValidColumnInfo = CopiedValidColumnInfoList;

                    if (lstValidColumnInfo.Count > 0)
                    {
                        dt = ConvertDataTable(true, dt, lstValidColumnInfo);
                        dt = GetTrimmedDataTable(dt, lstValidColumnInfo);
                    }
                    else
                        dt = null;

                    dt.AcceptChanges();
                    return dt;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    DataTable dt = Common.ConvertToDataViewCompatibleDataTable(value);
                    InitSpread(dt);
                }
            }
        }

        [Browsable(false)]
        public DataTable StatDataSource
        {
            get
            {
                try
                {
                    if (fpSpread_Sheet.RowCount < 2)
                        return null;

                    ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();
                    DataTable dt = ((DataTable)fpSpread_Sheet.DataSource).Copy();

                    dt.Rows.RemoveAt(0);
                    dt.AcceptChanges();

                    List<ColumnInfo> lstValidColumnInfo = CopiedValidColumnInfoList;

                    if (lstValidColumnInfo.Count > 0)
                    {
                        dt = ConvertDataTable(true, dt, lstValidColumnInfo);
                        dt = GetTrimmedDataTable(dt, lstValidColumnInfo, TrimPolicy.Row);
                        SetProperColumnName(dt, lstValidColumnInfo);
                    }
                    else
                        dt = null;

                    dt.AcceptChanges();

                    return dt;
                }
                catch
                {
                    return null;
                }
            }
        }

        private void SetProperColumnName(DataTable dt, List<ColumnInfo> lstValidColumnInfo)
        {
            if (dt == null || dt.Rows.Count < 1 || dt.Columns.Count < lstValidColumnInfo.Count)
                return;

            for(int i=0; i < dt.Columns.Count; i++)
            {
                try
                {
                    dt.Columns[i].ColumnName = lstColumnInfo[i].ColumnName;
                }
                catch
                {
                    continue;
                }
            }
        }

        [Browsable(false)]
        public DataTable PureDataSource
        {
            get
            {
                ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();
                DataTable dt = ((DataTable)fpSpread_Sheet.DataSource).Copy();

                return dt;
            }
        }

        [Browsable(false)]
        public List<ColumnInfo> CopiedValidColumnInfoList
        {
            get
            {
                return GetValidColumnInfoList(NameRowPolicy.Excluded);
            }
        }

        [Browsable(false)]
        public List<ColumnInfo> StatColumnInfoList
        {
            get
            {
                ColumnInfo columnInfo;
                List<ColumnInfo> validColumns = new List<ColumnInfo>();

                int iTemp = 0;
                foreach (ColumnInfo colInfo in lstColumnInfo)
                {
                    if (colInfo.ValidRowIndex > 0)
                    {
                        columnInfo = colInfo.Copy(NameRowPolicy.Excluded);
                        columnInfo.ColumnIndex = iTemp++;
                        validColumns.Add(columnInfo);
                    }
                }

                return validColumns;
            }
        }

        public static int DefaultColumnCount
        {
            get { return DataView.DEFAULT_COL_COUNT; }
            set { if (value > 0) DataView.DEFAULT_COL_COUNT = value; }
        }

        public static int DefaultRowCount
        {
            get { return DataView.DEFAULT_ROW_COUNT; }
            set { if (value > 0) DataView.DEFAULT_ROW_COUNT = value; }
        }

        [Browsable(false)]
        public bool IsSplited
        {
            get { return isSplited; }
            set { isSplited = value; }
        }

        #endregion

        #region " CREATOR "

        public DataView()
        {
            InitializeComponent();

            this.DataSource = GetEmptyDataTable(DEFAULT_ROW_COUNT, DEFAULT_COL_COUNT);
            isInitialized = false;
        }

        #endregion
        
        #region " METHOD "

        #region [ Initialization ]

        private void InitSpread(DataTable dataSource)
        {
            if (isInitialized)
                return;

            try
            {
                #region [ DefaultStyle ]
                fpSpread_Sheet.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                fpSpread_Sheet.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.None;
                fpSpread.VisualStyles = FarPoint.Win.VisualStyles.Off;
                fpSpread_Sheet.Columns.Default.Width = COLUMN_MIN_WIDTH;
                fpSpread_Sheet.DataAutoSizeColumns = false;
                fpSpread_Sheet.DataAutoCellTypes = false;
                fpSpread.FocusRenderer = null;
                fpSpread_Sheet.SelectionBackColor = Color.LightCyan;
                fpSpread_Sheet.SelectionForeColor = Color.Red;
                #endregion

                #region [ Default Skin ]
                this.fpSpread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
                FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
                FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
                //this.fpSpread.FocusRenderer = ((FarPoint.Win.Spread.IFocusIndicatorRenderer)(resources.GetObject("fpSpread.FocusRenderer")));
                this.fpSpread.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
                this.fpSpread.HorizontalScrollBar.Name = "";
                this.fpSpread.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
                this.fpSpread.HorizontalScrollBar.TabIndex = 5;
                this.fpSpread.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
                this.fpSpread.VerticalScrollBar.Name = "";
                this.fpSpread.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
                this.fpSpread.VerticalScrollBar.TabIndex = 6;
                this.fpSpread_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
                this.fpSpread_Sheet.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
                this.fpSpread_Sheet.SheetCornerStyle.Parent = "CornerDefault";
                #endregion

                #region [ Binding ]
                fpSpread_Sheet.DataSource = dataSource;
                #endregion

                #region [ ColumnInfo & CellType ]
                CommonCellType.WordWrap = false;
                CommonCellType.Multiline = false;
                float preferredWidth = 0;
                lstColumnInfo.Clear();

                if (!IsSplited)
                {
                    for (int i = 0; i < fpSpread_Sheet.ColumnCount; i++)
                    {
                        fpSpread_Sheet.Columns[i].CellType = CommonCellType;

                        lstColumnInfo.Add(new ColumnInfo(fpSpread_Sheet, i, DataType.NUMBER, string.Empty, 0, 0, 0, 0));
                        lstColumnInfo[i].delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                        lstColumnInfo[i].delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);
                        
                        preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(i, false);

                        if (preferredWidth < COLUMN_MIN_WIDTH)
                            fpSpread_Sheet.Columns[i].Width = COLUMN_MIN_WIDTH;
                        else
                            fpSpread_Sheet.Columns[i].Width = preferredWidth;
                    }
                }
                else
                {
                    for (int i = 0; i < fpSpread_Sheet.ColumnCount; i++)
                    {
                        dataSource.Columns[i].ColumnName = "TEMP_COLUMN_NAME" + i.ToString();
                    }

                    ColumnInfo columnInfo;
                    for (int i = 0; i < fpSpread_Sheet.ColumnCount; i++)
                    {
                        fpSpread_Sheet.Columns[i].CellType = CommonCellType;
                        dataSource.Columns[i].ColumnName = "C" + (i+1).ToString();

                        columnInfo = GetParsedColumnInfo(i); 
                        columnInfo.delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                        columnInfo.delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);
                        lstColumnInfo.Add(columnInfo);

                        columnInfo.DataType = columnInfo.DataType;
                        if(columnInfo.DataType != DataType.TEXT)
                        {
                            SetMissingValue(columnInfo.ColumnIndex, 1, columnInfo.ValidRowIndex, false);
                        }

                        preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(i, false);

                        if (preferredWidth < COLUMN_MIN_WIDTH)
                            fpSpread_Sheet.Columns[i].Width = COLUMN_MIN_WIDTH;
                        else
                            fpSpread_Sheet.Columns[i].Width = preferredWidth;

                    }

                    if(GetMaxValidRowIndex() == fpSpread_Sheet.RowCount-1)
                        fpSpread_Sheet.Rows.Add(fpSpread_Sheet.RowCount, 1);


                    InsertColumns(fpSpread_Sheet.ColumnCount, 20);

                }
                #endregion

                #region [ Inputmap ]
                FarPoint.Win.Spread.InputMap im;

                im = fpSpread.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.Normal);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.F2, Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Z, Keys.Control), FarPoint.Win.Spread.SpreadActions.Undo);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Z, Keys.Shift | Keys.Control), FarPoint.Win.Spread.SpreadActions.Redo);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.X, Keys.Control), FarPoint.Win.Spread.SpreadActions.None);

                im = fpSpread.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
                im.Put(new FarPoint.Win.Spread.Keystroke(Keys.X, Keys.Control), FarPoint.Win.Spread.SpreadActions.None);
                #endregion

                #region [ Name Row ]
                fpSpread_Sheet.FrozenRowCount = 1;
                fpSpread_Sheet.StartingRowNumber = 0;
                fpSpread_Sheet.RowHeader.Cells[0, 0].Value = " ";
                fpSpread_Sheet.RowHeader.Cells[0, 0].BackColor = System.Drawing.Color.FromArgb(224, 224, 255);
                fpSpread_Sheet.Rows[0].BackColor = System.Drawing.Color.FromArgb(224, 224, 255);
                fpSpread_Sheet.Rows[0].Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                fpSpread_Sheet.Rows[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fpSpread_Sheet.Rows[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                #endregion

                #region [ Spread Editmode ]
                fpSpread.AllowDragFill = true;
                fpSpread.EditModeReplace = true;
                fpSpread.AllowEditOverflow = true;
                #endregion

                #region [ Context Menu ]
                fpSpread.ContextMenuStrip = cmsDataView;
                #endregion

                #region [ EventHandler ]

                if(!isEventConnected)
                {
                    this.fpSpread.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread_CellClick);
                    this.fpSpread.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fpSpread_KeyDown);
                    this.fpSpread.KeyUp += new KeyEventHandler(fpSpread_KeyUp);
                    this.fpSpread.DragFillBlock += new FarPoint.Win.Spread.DragFillBlockEventHandler(fpSpread_DragFillBlock);
                    this.fpSpread.SelectionChanging += new FarPoint.Win.Spread.SelectionChangingEventHandler(fpSpread_SelectionChanging);
                    this.fpSpread.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(fpSpread_SelectionChanged);
                    this.fpSpread.ClipboardPasting += new FarPoint.Win.Spread.ClipboardPastingEventHandler(fpSpread_ClipboardPasting);
                    this.fpSpread_Sheet.CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(fpSpread_Sheet_CellChanged);

                    isEventConnected = true;
                }

                #endregion
            }
            catch
            {
                throw (new Exception("DataView.InitSpread()"));
            }

            isInitialized = true;
        }

        #endregion

        #region [ DataType ]

        private DataType GetInputDataType(string content)
        {
            content = content.Trim();

            if (content == SHEET_MISSING_VALUE)
                return DataType.NUMBER;

            double tempDouble = 0;
            DateTime tempDateTime = DateTime.MinValue;

            if (double.TryParse(content, out tempDouble))
                return DataType.NUMBER;
            else if (DateTime.TryParse(content, out tempDateTime))
                return DataType.DATETIME;
            else
                return DataType.TEXT;
        }

        private DataType GetColumnDataType(int columnIndex, int validRowIndex)
        {
            if (validRowIndex < 1)
                return DataType.NUMBER;

            object value = null;
            for (int i = 1; i <= validRowIndex; i++)
            {
                value = fpSpread_Sheet.Models.Data.GetValue(i, columnIndex);

                if (value == null || value.ToString() == string.Empty || value.ToString() == SHEET_MISSING_VALUE)
                    continue;
                else
                {
                    DataType type = GetInputDataType(value.ToString());
                    if (type != DataType.NUMBER)
                        return type;
                }
            }

            return DataType.NUMBER;
        }

        private bool CheckValidInput(int columnIndex, ref string content)
        {
            bool isValid = false;

            switch (lstColumnInfo[columnIndex].DataType)
            {
                case DataType.NUMBER:
                    if (content == SHEET_MISSING_VALUE)
                        isValid = true;
                    else
                    {
                        double temp;
                        isValid = double.TryParse(content, out temp);
                        content = GetProcessedDoubleString(lstColumnInfo[columnIndex], temp);
                    }
                    break;
                case DataType.TEXT:
                    isValid = true;
                    break;
                case DataType.DATETIME:
                    if (content == SHEET_MISSING_VALUE)
                        isValid = true;
                    else
                    {
                        DateTime dateTime;
                        isValid = DateTime.TryParse(content, out dateTime);
                        content = dateTime.ToString(lstColumnInfo[columnIndex].CurrentDateTimeFormat);
                    }
                    
                    break;
            }

            return isValid;
        }

        private string GetProcessedDoubleString(ColumnInfo columnInfo, double dValue)
        {
            string strResult = "";
            double pow;

            try
            {
                switch (ColumnInfo.RoundOption[columnInfo.RoundOptionIndex])
                {
                    case "Round":
                        strResult = Math.Round(dValue, columnInfo.DecimalPlace, MidpointRounding.AwayFromZero).ToString();
                        break;
                    case "Up":
                        pow = Math.Pow(10, columnInfo.DecimalPlace);
                        strResult = (Math.Ceiling(dValue * pow) / pow).ToString();
                        break;
                    case "Truncate":
                        pow = Math.Pow(10, columnInfo.DecimalPlace);
                        strResult = (Math.Floor(dValue * pow) / pow).ToString();
                        break;
                    case "None":
                        strResult = dValue.ToString();
                        break;
                }
            }
            catch
            {
                return "";
            }

            return strResult;
        }

        #endregion

        #region [ ContextMenu ]

        public void ProcessDataViewContextMenu(string menuName)
        {
            try
            {
                lstColumnInfo.Sort(columnInfoComparer);

                switch (menuName)
                {
                    case "cmiColumnSetting":
                        OnColumnSetting();
                        break;
                    case "cmiInsert":
                        OnInsert();
                        break;
                    case "cmiRemove":
                        OnRemove();
                        break;
                    case "cmiMoveColumn":
                        OnMoveColumn();
                        break;
                    case "cmiSort":
                        OnSort();
                        break;
                    case "cmiSplit":
                        OnSplit();
                        break;
                    case "cmiSplitSelected":
                        OnSplitSelected();
                        break;
                    case "cmiTranspose":
                        OnTranspose();
                        break;
                    case "cmiExportToExcel":
                        OnExportToExcel();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " \r\n\t: DataView.ProcessDataViewContextMenu(string menuName)");
            }
        }

        private void OnColumnSetting()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewColumnSetting dlgColumnSetting = new DACrux.ProjectManager.UI.Dialog.DlgDataViewColumnSetting(lstColumnInfo, (DataTable)fpSpread_Sheet.DataSource);
            dlgColumnSetting.ShowDialog();
        }

        private void OnInsert()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove dlgInsert = new DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove("Insert");

            try
            {
                if (dlgInsert.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    crCommon = fpSpread_Sheet.GetSelection(0);

                    if (crCommon == null)
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

                    switch (dlgInsert.CheckedType)
                    {
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.CELLS:
                            AddCells(crCommon);
                            break;
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.ROW:
                            InsertRows(crCommon);
                            break;
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.COLUMN:
                            InsertColumns(crCommon);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OnRemove()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove dlgRemove = new DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove("Remove");

            try
            {
                Cursor = Cursors.WaitCursor;

                if (dlgRemove.ShowDialog() == DialogResult.OK)
                {
                    crCommon = fpSpread_Sheet.GetSelection(0);

                    if (crCommon == null)
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

                    switch (dlgRemove.CheckedType)
                    {
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.CELLS:
                            RemoveCells(crCommon);
                            break;
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.ROW:
                            RemoveRows(crCommon);
                            break;
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewInsertRemove.Type.COLUMN:
                            RemoveColumns(crCommon);
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OnMoveColumn()
        {
            int lastValidColumnIndex;
            List<ColumnInfo> lstValidColumns;
            List<int> lstSelectedColumnIndices;
            DACrux.ProjectManager.UI.Dialog.DlgDataViewMoveColumn dlgMoveColumn;

            DataTable dt;

            int startIndex = 0;
            int middleIndex = 0;
            int endIndex = 0;

            if (fpSpread_Sheet.SelectionCount > 0)
                crCommon = fpSpread_Sheet.GetSelection(0);
            else
                crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

            int selectedLeftIndex = 0;
            int selectedRowIndex = crCommon.Row;
            int selectedRowCount = crCommon.RowCount;

            try
            {
                lstValidColumns = GetValidColumnInfoList();
                lstSelectedColumnIndices = GetSelectedColumnIndices();

                if (lstValidColumns != null && lstValidColumns.Count > 0)
                    lastValidColumnIndex = lstValidColumns[lstValidColumns.Count - 1].ColumnIndex;
                else
                {
                    MessageBox.Show("No data in selected column.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dlgMoveColumn = new DACrux.ProjectManager.UI.Dialog.DlgDataViewMoveColumn(lstColumnInfo, lastValidColumnIndex, lstSelectedColumnIndices);

                if (dlgMoveColumn.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    dt = (DataTable)fpSpread_Sheet.DataSource;

                    switch (dlgMoveColumn.CheckedOption)
                    {
                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewMoveColumn.Option.First:
                            startIndex = 0;
                            middleIndex = lstSelectedColumnIndices[0];
                            endIndex = lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1] + 1;

                            selectedLeftIndex = startIndex;
                            break;

                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewMoveColumn.Option.Last:
                            startIndex = lstSelectedColumnIndices[0];
                            middleIndex = lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1] + 1;
                            endIndex = lastValidColumnIndex + 1;

                            selectedLeftIndex = endIndex - (middleIndex - startIndex);
                            break;

                        case DACrux.ProjectManager.UI.Dialog.DlgDataViewMoveColumn.Option.Previous:
                            if (lstSelectedColumnIndices[0] < dlgMoveColumn.PreviousColumnIndex)
                            {
                                startIndex = lstSelectedColumnIndices[0];
                                middleIndex = lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1] + 1;
                                endIndex = dlgMoveColumn.PreviousColumnIndex;

                                selectedLeftIndex = endIndex - (middleIndex - startIndex);
                            }
                            else
                            {
                                startIndex = dlgMoveColumn.PreviousColumnIndex;
                                middleIndex = lstSelectedColumnIndices[0];
                                endIndex = lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1] + 1;

                                selectedLeftIndex = startIndex;
                            }
                            break;
                    }

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (i < middleIndex)
                        {
                            lstColumnInfo[i].ColumnIndex += endIndex - middleIndex;
                        }
                        else
                        {
                            fpSpread_Sheet.MoveColumn(i, i - (middleIndex - startIndex), true);
                            dt.Columns[i].SetOrdinal(i - (middleIndex - startIndex));
                            lstColumnInfo[i].ColumnIndex = i - (middleIndex - startIndex);
                        }
                    }

                    dt.AcceptChanges();

                    fpSpread_Sheet.SetActiveCell(selectedRowIndex, selectedLeftIndex);
                    fpSpread_Sheet.AddSelection(selectedRowIndex, selectedLeftIndex, selectedRowCount, lstSelectedColumnIndices.Count);

                    ClearSheetSelection();
                    DrawSheetSelection(fpSpread_Sheet.GetSelection(0));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                lstColumnInfo.Sort(columnInfoComparer);

                for (int i = 0; i < endIndex; i++)
                    fpSpread_Sheet.Columns[i].Label = "C" + (lstColumnInfo[i].ColumnIndex + 1).ToString() + ((lstColumnInfo[i].DataType == DataType.NUMBER) ? "" : ((lstColumnInfo[i].DataType == DataType.TEXT) ? " - C" : " - D"));

                Cursor = Cursors.Default;
            }
        }

        private void OnSort()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewSort dlgSort;

            string [] arrColumn;
            string [] arrSplit;

            SortInfoDescComparer descComparer;
            SortInfoAscComparer ascComparer;
            FarPoint.Win.Spread.SortInfo[] sorters;

            DataTable dtBackup;
            DataTable dtSorted;

            ColumnInfo[] arrColumnInfo;
            List<ColumnInfo> lstValidColumnInfo;
            List<ColumnInfo> lstColumnInfoBackup;
            bool isSelection = true;

            try
            {
                lstValidColumnInfo = GetValidColumnInfoList();

                if (lstValidColumnInfo == null || lstValidColumnInfo.Count < 1)
                {
                    MessageBox.Show("No data in current worksheet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (fpSpread_Sheet.SelectionCount > 0)
                {
                    FarPoint.Win.Spread.Model.CellRange cr = fpSpread_Sheet.GetSelection(0);
                    if (cr.RowCount == 1 && cr.ColumnCount == 1)
                        isSelection = false;
                    else
                        isSelection = true;
                }
                else
                    isSelection = false;

                dlgSort = new DACrux.ProjectManager.UI.Dialog.DlgDataViewSort(lstValidColumnInfo, isSelection);

                if(dlgSort.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    arrColumn = dlgSort.SortResult.Split(new char[] { ',' });
                    sorters = new FarPoint.Win.Spread.SortInfo[arrColumn.Length];
                    descComparer = new SortInfoDescComparer();
                    ascComparer = new SortInfoAscComparer();

                    for (int i = 0; i < arrColumn.Length; i++)
                    {
                        if (arrColumn[i].Trim() == "")
                            continue;

                        arrSplit = arrColumn[i].Split(new char[] { ' ' });

                        if(arrSplit[1] == "DESC")
                            sorters[i] = new FarPoint.Win.Spread.SortInfo(Convert.ToInt32(arrSplit[0]), false, descComparer);
                        else
                            sorters[i] = new FarPoint.Win.Spread.SortInfo(Convert.ToInt32(arrSplit[0]), true, ascComparer);
                    }

                    if (dlgSort.CreateNewWorkSheet && NewDataSourceSplited != null)
                    {
                        #region [ Backup ]
                        fpSpread_Sheet.Models.ResetViewRowIndexes();
                        ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges(); 
                        dtBackup = ((DataTable)fpSpread_Sheet.DataSource).Copy();
                        lstColumnInfoBackup = new List<ColumnInfo>();
                        arrColumnInfo = new ColumnInfo[lstColumnInfo.Count];
                        lstColumnInfo.CopyTo(arrColumnInfo);
                        lstColumnInfoBackup.AddRange(arrColumnInfo);
                        #endregion

                        dtSorted = ProcessSort(dlgSort.SelectionOnly, lstValidColumnInfo, sorters);
                    
                        NewDataSourceSplited("", dtSorted);
                        fpSpread_Sheet.DataSource = dtBackup;
                        lstColumnInfo = lstColumnInfoBackup;
                    }
                    else
                    {
                        ProcessSort(dlgSort.SelectionOnly, lstValidColumnInfo, sorters);
                    }

                    ClearSheetSelection();

                    if (fpSpread_Sheet.SelectionCount > 0)
                        crCommon = fpSpread_Sheet.GetSelection(0);
                    else
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

                    DrawSheetSelection(crCommon);

                    Cursor = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        private void OnSplit()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit dlgSplit;
            List<ColumnInfo> lstValidColumnInfo;
            DataTable dataSource;
            DataTable dtSelected;
            FarPoint.Win.Spread.Model.CellRange crSelected;

            try
            {
                lstValidColumnInfo = GetValidColumnInfoList();
                if (lstValidColumnInfo == null || lstValidColumnInfo.Count < 1)
                {
                    MessageBox.Show("No data in current worksheet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataSource = (DataTable)fpSpread_Sheet.DataSource;
                dataSource.AcceptChanges();
                dlgSplit = new DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit(lstValidColumnInfo, dataSource);
                dtSelected = null;
                crSelected = null;

                if(dlgSplit.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    if (dlgSplit.Condition == DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.ColumnCondition)
                    {
                        dtSelected = dlgSplit.ResultDataSource;
                    }
                    else
                    {
                        switch(dlgSplit.Condition)
                        {
                            case DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.SelectedColumn:
                                if(dlgSplit.Method == DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.MethodType.Include)
                                {
                                    if (fpSpread_Sheet.SelectionCount < 1)
                                        crCommon = new FarPoint.Win.Spread.Model.CellRange(1, fpSpread_Sheet.ActiveColumnIndex, fpSpread_Sheet.RowCount - 1, 1);
                                    else
                                    {
                                        crSelected = fpSpread_Sheet.GetSelection(0);
                                        crCommon = new FarPoint.Win.Spread.Model.CellRange(1, crSelected.Column, fpSpread_Sheet.RowCount - 1, crSelected.ColumnCount);
                                    }
                                    dtSelected = GetDataTableFromSelection(crCommon);
                                }
                                else if(dlgSplit.Method == DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.MethodType.Exclude)
                                {
                                    List<int> lstSelectedColumnIndices = GetSelectedColumnIndices();

                                    if(lstSelectedColumnIndices == null || lstSelectedColumnIndices.Count < 1)
                                    {
                                        MessageBox.Show("No selected columns", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }

                                    dtSelected = dataSource.Copy();

                                    for (int i = lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1]; i >= lstSelectedColumnIndices[0]; i--)
                                    {
                                        dtSelected.Columns.RemoveAt(i);
                                    }
                                }
                                break;
                            case DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.SelectedRow:
                                if (dlgSplit.Method == DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.MethodType.Include)
                                {
                                    if (fpSpread_Sheet.SelectionCount < 1)
                                        crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveRowIndex, 0, 1, fpSpread_Sheet.ColumnCount);
                                    else
                                    {
                                        crSelected = fpSpread_Sheet.GetSelection(0);
                                        crCommon = new FarPoint.Win.Spread.Model.CellRange(crSelected.Row, 0, crSelected.RowCount, fpSpread_Sheet.ColumnCount);
                                    }
                                    dtSelected = GetDataTableFromSelection(crCommon);
                                }
                                else if(dlgSplit.Method == DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.MethodType.Exclude)
                                {
                                    if (fpSpread_Sheet.SelectionCount < 1)
                                        crSelected = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveRowIndex, 0, 1, fpSpread_Sheet.ColumnCount);
                                    else
                                    {
                                        crSelected = fpSpread_Sheet.GetSelection(0);
                                    }

                                    dtSelected = dataSource.Copy();

                                    for (int i = crSelected.Row + crSelected.RowCount - 1; i >= crSelected.Row; i--)
                                    {
                                        dtSelected.Rows.RemoveAt(i);
                                    }
                                }
                                break;
                            case DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.SelectedRange:
                                if (fpSpread_Sheet.SelectionCount < 1)
                                    crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveRowIndex, fpSpread_Sheet.ActiveColumnIndex, 1, 1);
                                else
                                {
                                    crSelected = fpSpread_Sheet.GetSelection(0);
                                    crCommon = new FarPoint.Win.Spread.Model.CellRange(crSelected.Row, crSelected.Column, crSelected.RowCount, crSelected.ColumnCount);
                                }
                                dtSelected = GetDataTableFromSelection(crCommon);
                                break;
                            case DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.SelectedRowNumber:
                            case DACrux.ProjectManager.UI.Dialog.DlgDataViewSplit.ConditionType.SelectedRandomSample:
                                dtSelected = dlgSplit.ResultDataSource;
                                break;
                        }
                    }

                    if (NewDataSourceSplited != null && dtSelected != null)
                    {
                        dtSelected.AcceptChanges();
                        NewDataSourceSplited("", dtSelected);
                    }

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
            	throw(ex);
            }
        }

        private void OnSplitSelected()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                crCommon = fpSpread_Sheet.GetSelection(0);
                if (crCommon == null)
                    return;

                DataTable dt = GetDataTableFromSelection(crCommon);

                if (dt != null && NewDataSourceSplited != null)
                    NewDataSourceSplited("", dt);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OnTranspose()
        {
            DACrux.ProjectManager.UI.Dialog.DlgDataViewTranspose dlgTranspose;

            DataTable dataSource;
            List<ColumnInfo> lstValidColumnInfo;

            try
            {
                dataSource = DataSource;
                lstValidColumnInfo = CopiedValidColumnInfoList;

                if(dataSource == null || dataSource.Columns.Count < 1 || dataSource.Rows.Count < 1)
                {
                    MessageBox.Show("No data in current worksheet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dlgTranspose = new DACrux.ProjectManager.UI.Dialog.DlgDataViewTranspose(dataSource, lstValidColumnInfo);

                if (dlgTranspose.ShowDialog() == DialogResult.OK)
                {
                    if (NewDataSourceSplited != null)
                        NewDataSourceSplited("", dlgTranspose.DataSource);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void OnExportToExcel()
        {
            SaveFileDialog dlgSaveFile;
            //DataTable dt;

            try
            {
                dlgSaveFile = new SaveFileDialog();
                dlgSaveFile.AddExtension = true;
                dlgSaveFile.FileName = this.ParentForm.Text;
                dlgSaveFile.Filter = "Excel Files(*.xls)|*.xls";
                
                if(dlgSaveFile.ShowDialog() == DialogResult.OK && dlgSaveFile.FileName.Trim() != string.Empty)
                {
                    //dt = DataSource;
                    //Microsoft.Office.Interop.Excel.WorksheetClass sheet = new Microsoft.Office.Interop.Excel.WorksheetClass();
                    Cursor = Cursors.WaitCursor;

                    fpSpread.SaveExcel(dlgSaveFile.FileName, FarPoint.Excel.ExcelSaveFlags.DataOnly);

                    MessageBox.Show("Export completed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Please check the file is already opened by other program.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        # region [ Cell Contents ]

        private void BeginCellEdit()
        {
            fpSpread_Sheet.CellChanged -= fpSpread_Sheet_CellChanged;
        }

        private void EndCellEdit()
        {
            fpSpread_Sheet.CellChanged += fpSpread_Sheet_CellChanged;
        }

        private void ProcessCellChanged(int row, int column)
        {
            string strChangedText = fpSpread_Sheet.Cells[row, column].Text;

            if (lstColumnInfo[column].DataType == DataType.TEXT && strChangedText.Equals(SHEET_MISSING_VALUE))
                fpSpread_Sheet.Models.Data.SetValue(row, column, string.Empty);

            if (row == 0) // Name Row
            {
                lstColumnInfo[column].ColumnName = strChangedText;
            }
            else if (row > 0) // Normal Row
            {
                if (strChangedText == "") // No Value
                {
                    if (lstColumnInfo[column].ValidRowIndex == 0)
                        fpSpread_Sheet.Columns[column].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;

                    if (lstColumnInfo[column].ValidRowIndex == row)
                    {
                        lstColumnInfo[column].ValidRowIndex--;

                        if (lstColumnInfo[column].ValidRowIndex == 0)
                            fpSpread_Sheet.Columns[column].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    }
                    else
                    {
                        if (lstColumnInfo[column].DataType != DataType.TEXT && lstColumnInfo[column].ValidRowIndex > row)
                            fpSpread_Sheet.Cells[row, column].Value = SHEET_MISSING_VALUE;
                    }
                }
                else // Has Value
                {
                    if (lstColumnInfo[column].ValidRowIndex == 0) // First Value
                    {
                        lstColumnInfo[column].ValidRowIndex = row;
                        lstColumnInfo[column].DataType = GetInputDataType(strChangedText);
                        ProcessColumnSettingChanged(lstColumnInfo[column]);
                    }
                    else // Not First Value
                    {
                        if (!CheckValidInput(column, ref strChangedText))
                            fpSpread_Sheet.Cells[row, column].Value = SHEET_MISSING_VALUE;
                        else
                            fpSpread_Sheet.Cells[row, column].Value = strChangedText;

                        SetMissingValue(column, row);
                        lstColumnInfo[column].ValidRowIndex = (lstColumnInfo[column].ValidRowIndex < row) ? row : lstColumnInfo[column].ValidRowIndex;
                    }
                }
            }

            float preferredWidth = fpSpread_Sheet.GetPreferredCellSize(row, column).Width;
            if (fpSpread_Sheet.Columns[column].Width < preferredWidth)
                fpSpread_Sheet.Columns[column].Width = preferredWidth;

        }

        private void SetMissingValue(int colIndex, int rowIndex)
        {
            SetMissingValue(colIndex, lstColumnInfo[colIndex].ValidRowIndex, rowIndex, false);
        }

        private void SetMissingValue(int colIndex, int rowIndexFrom, int rowIndexTo, bool isIgnoreType)
        {
            if(!isIgnoreType)
            {
                if (lstColumnInfo[colIndex].ValidRowIndex > 0 && lstColumnInfo[colIndex].DataType != DataType.TEXT) // Number, DateTime -> 결측치 표시
                {
                    for (int i = rowIndexFrom; i <= rowIndexTo; i++)
                    {
                        if (fpSpread_Sheet.Cells[i, colIndex].Text == "")
                            fpSpread_Sheet.Cells[i, colIndex].Value = SHEET_MISSING_VALUE;
                    }
                }
                else
                {
                    for (int i = rowIndexFrom; i <= rowIndexTo; i++)
                    {
                        if (fpSpread_Sheet.Cells[i, colIndex].Text == SHEET_MISSING_VALUE)
                            fpSpread_Sheet.Cells[i, colIndex].Value = "";
                    }
                }
            }
            else
            {
                if(lstColumnInfo[colIndex].ValidRowIndex > 0)
                {
                    for (int i = rowIndexFrom; i <= rowIndexTo; i++)
                    {
                        if (fpSpread_Sheet.Cells[i, colIndex].Text == "")
                            fpSpread_Sheet.Cells[i, colIndex].Value = SHEET_MISSING_VALUE;
                    }
                }
            }
        }

        #endregion

        #region [ Cell & Row & Column ]

        private void AddCells(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startRowIndex = cr.Row;
            int rowCount = cr.RowCount;
            int startColumnIndex = cr.Column;
            int columnCount = cr.ColumnCount;

            if (cr.Row == 0)
            {
                startRowIndex = 1;
                rowCount = cr.RowCount - 1;
            }

            int endRowIndex = startRowIndex + rowCount - 1;
            int endColumnIndex = startColumnIndex + columnCount - 1;

            int requiredRowCount = 0;

            BeginCellEdit();
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                if (lstColumnInfo[i].ValidRowIndex + rowCount >= fpSpread_Sheet.RowCount - 1)
                {
                    requiredRowCount = lstColumnInfo[i].ValidRowIndex + rowCount - fpSpread_Sheet.RowCount + 2;
                    fpSpread_Sheet.Rows.Add(fpSpread_Sheet.Rows.Count, requiredRowCount);
                }

                if (lstColumnInfo[i].ValidRowIndex >= startRowIndex)
                {
                    fpSpread_Sheet.MoveRange(startRowIndex, i, endRowIndex + 1, i, lstColumnInfo[i].ValidRowIndex - startRowIndex + 1, 1, true);
                    lstColumnInfo[i].ValidRowIndex += rowCount;

                    if (lstColumnInfo[i].DataType != DataType.TEXT) // Number, DateTime -> 결측치 표시
                    {
                        for (int j = startRowIndex; j <= endRowIndex; j++)
                        {
                            if (fpSpread_Sheet.Cells[j, lstColumnInfo[i].ColumnIndex].Text == "")
                                fpSpread_Sheet.Cells[j, lstColumnInfo[i].ColumnIndex].Value = SHEET_MISSING_VALUE;
                        }
                    }
                }
            }
            EndCellEdit();

            ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();

            ClearSheetSelection();
            DrawSheetSelection(cr);
        }

        private void InsertRows(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startRowIndex = cr.Row;
            int rowCount = cr.RowCount;

            if (startRowIndex < 1 || rowCount < 1)
                return;

            int endRowIndex = startRowIndex + rowCount - 1;

            //fpSpread_Sheet.AddRows(startRowIndex, rowCount);
            DataTable dt = (DataTable)fpSpread_Sheet.DataSource;
            for (int i = startRowIndex; i < startRowIndex + rowCount; i++ )
                dt.Rows.InsertAt(dt.NewRow(), i);
            dt.AcceptChanges();

            fpSpread_Sheet.Models.ResetViewRowIndexes();

            foreach (ColumnInfo ci in lstColumnInfo)
            {
                if (ci.ValidRowIndex >= startRowIndex)
                {
                    ci.ValidRowIndex += rowCount;

                    if (ci.DataType != DataType.TEXT) // Number, DateTime -> 결측치 표시
                    {
                        for (int i = startRowIndex; i <= endRowIndex; i++)
                        {
                            if (fpSpread_Sheet.Cells[i, ci.ColumnIndex].Text == "")
                                fpSpread_Sheet.Cells[i, ci.ColumnIndex].Value = SHEET_MISSING_VALUE;
                        }
                    }
                }
            }

            //((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();

            ClearSheetSelection();
            DrawSheetSelection(cr);
        }
        private void InsertColumns(int iStart, int Count)
        {
            int startColumnIndex = iStart;
            int columnCount = Count;

            if (startColumnIndex < 0 || columnCount < 1)
                return;

            int endColumnIndex = startColumnIndex + columnCount - 1;
            float preferredWidth = 0;

            DataTable dt = (DataTable)fpSpread_Sheet.DataSource;

            for (int i = 0; i < Count; i++)
            {
                if (i <= endColumnIndex)
                {
                    fpSpread_Sheet.Columns.Add(i + iStart, 1);
                    dt.Columns.Add().SetOrdinal(i + iStart);

                    fpSpread_Sheet.Columns[i].CellType = CommonCellType;

                    lstColumnInfo.Insert(i + iStart, new ColumnInfo(fpSpread_Sheet, i + iStart, DataType.NUMBER, string.Empty, 0, 0, 0, 0));
                    lstColumnInfo[i + iStart].delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                    lstColumnInfo[i + iStart].delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);

                    preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(i + iStart, false);

                    if (preferredWidth < COLUMN_MIN_WIDTH)
                        fpSpread_Sheet.Columns[i + iStart].Width = COLUMN_MIN_WIDTH;
                    else
                        fpSpread_Sheet.Columns[i + iStart].Width = preferredWidth;
                }
                else
                {
                    lstColumnInfo[i + iStart].ColumnIndex += columnCount;
                }

                fpSpread_Sheet.Columns[i + iStart].Label = lstColumnInfo[i + iStart].ColumnID;
            }

            dt.AcceptChanges();
            ClearSheetSelection();
        }

        private void InsertColumns(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startColumnIndex = cr.Column;
            int columnCount = cr.ColumnCount;

            if (startColumnIndex < 0 || columnCount < 1)
                return;

            int endColumnIndex = startColumnIndex + columnCount - 1;
            float preferredWidth = 0;

            DataTable dt = (DataTable)fpSpread_Sheet.DataSource;

            for (int i = startColumnIndex; i < fpSpread_Sheet.Columns.Count; i++)
            {
                if (i <= endColumnIndex)
                {
                    fpSpread_Sheet.Columns.Add(i, 1);
                    dt.Columns.Add().SetOrdinal(i);

                    fpSpread_Sheet.Columns[i].CellType = CommonCellType;

                    lstColumnInfo.Insert(i, new ColumnInfo(fpSpread_Sheet, i, DataType.NUMBER, string.Empty, 0, 0, 0, 0));
                    lstColumnInfo[i].delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                    lstColumnInfo[i].delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);

                    preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(i, false);

                    if (preferredWidth < COLUMN_MIN_WIDTH)
                        fpSpread_Sheet.Columns[i].Width = COLUMN_MIN_WIDTH;
                    else
                        fpSpread_Sheet.Columns[i].Width = preferredWidth;
                }
                else
                {
                    lstColumnInfo[i].ColumnIndex += columnCount;
                }

                fpSpread_Sheet.Columns[i].Label = lstColumnInfo[i].ColumnID;
            }

            dt.AcceptChanges();

            ClearSheetSelection();
            DrawSheetSelection(cr);
        }

        private void RemoveCells(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startRowIndex = cr.Row;
            int rowCount = cr.RowCount;

            int startColumnIndex = cr.Column;
            int columnCount = cr.ColumnCount;


            BeginCellEdit();

            if (cr.Row == 0)
            {
                if (cr.RowCount > 1)
                {
                    startRowIndex = 1;
                    rowCount = cr.RowCount - 1;
                }
                else
                {
                    for (int i = startColumnIndex; i < startColumnIndex + columnCount; i++)
                    {
                        fpSpread_Sheet.Models.Data.SetValue(cr.Row, i, string.Empty);
                        lstColumnInfo[i].ColumnName = string.Empty;
                    }
                    return;
                }
            }
            else if(cr.Row < 0)
            {
                startRowIndex = 0;
                rowCount = fpSpread_Sheet.RowCount;
            }

            int maxValidRowIndex = 0;
            for (int i = startColumnIndex; i < startColumnIndex + columnCount; i++)
            {
                if (maxValidRowIndex < lstColumnInfo[i].ValidRowIndex)
                    maxValidRowIndex = lstColumnInfo[i].ValidRowIndex;

                if (lstColumnInfo[i].ValidRowIndex >= startRowIndex)
                {
                    if (lstColumnInfo[i].ValidRowIndex >= (startRowIndex + rowCount))
                        lstColumnInfo[i].ValidRowIndex -= rowCount;
                    else
                        lstColumnInfo[i].ValidRowIndex = startRowIndex - 1;
                }
            }

            if (maxValidRowIndex >= startRowIndex)
            {
                fpSpread_Sheet.ClearRange(startRowIndex, startColumnIndex, rowCount, columnCount, true);
                fpSpread_Sheet.MoveRange(startRowIndex + rowCount, startColumnIndex, startRowIndex, startColumnIndex, maxValidRowIndex - startRowIndex, columnCount, true);
            }

            ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();

            EndCellEdit();
        }

        private void RemoveRows(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startRowIndex = cr.Row;
            int rowCount = cr.RowCount;

            if (startRowIndex < 1 || rowCount < 1)
                return;

            int endRowIndex = startRowIndex + rowCount - 1;

            
            fpSpread_Sheet.RemoveRows(startRowIndex, rowCount);

            foreach (ColumnInfo ci in lstColumnInfo)
            {
                if (ci.ValidRowIndex >= startRowIndex)
                    ci.ValidRowIndex -= rowCount;
            }

            if (cr.Row >= fpSpread_Sheet.RowCount)
                cr = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.RowCount-1, cr.Column, 1, 1);
            else if (cr.Row + cr.RowCount >= fpSpread_Sheet.RowCount)
                cr = new FarPoint.Win.Spread.Model.CellRange(cr.Row, cr.Column, fpSpread_Sheet.RowCount - cr.Row, cr.ColumnCount);

            if (cr.Column < 0)
                cr = new FarPoint.Win.Spread.Model.CellRange(cr.Row, 0, cr.RowCount, fpSpread_Sheet.ColumnCount);

            ((DataTable)fpSpread_Sheet.DataSource).AcceptChanges();

            ClearSheetSelection();
            DrawSheetSelection(cr);
        }

        private void RemoveColumns(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int startColumnIndex = cr.Column;
            int columnCount = cr.ColumnCount;

            if (startColumnIndex < 0 || columnCount < 1)
                return;
            else if (columnCount >= fpSpread_Sheet.ColumnCount)
                return;

            int endColumnIndex = startColumnIndex + columnCount - 1;

            DataTable dt = (DataTable)fpSpread_Sheet.DataSource;

            for (int i = fpSpread_Sheet.Columns.Count - 1; i >= startColumnIndex; i--)
            {
                if (i > endColumnIndex)
                {
                    lstColumnInfo[i].ColumnIndex -= columnCount;

                    fpSpread_Sheet.Columns[i].Label = lstColumnInfo[i].ColumnID;
                }
                else
                {
                    fpSpread_Sheet.Columns.Remove(i, 1);
                    dt.Columns.RemoveAt(i);
                    lstColumnInfo.RemoveAt(i);
                }
            }

            dt.AcceptChanges();

            if (cr.Column >= fpSpread_Sheet.ColumnCount)
                cr = new FarPoint.Win.Spread.Model.CellRange(cr.Row, fpSpread_Sheet.ColumnCount - 1, 1, 1);
            else if (cr.Column + cr.ColumnCount >= fpSpread_Sheet.ColumnCount)
                cr = new FarPoint.Win.Spread.Model.CellRange(cr.Row, cr.Column, cr.RowCount, fpSpread_Sheet.ColumnCount - cr.Column);

            if (cr.Row < 0)
                cr = new FarPoint.Win.Spread.Model.CellRange(0, cr.Column, fpSpread_Sheet.RowCount, cr.ColumnCount);

            ClearSheetSelection();
            DrawSheetSelection(cr);
        }

        private bool GetRowColumnCount(string clipboardString, ref int rowCount, ref int colCount)
        {
            try
            {
                foreach (char ch in clipboardString.ToCharArray())
                {
                    if (ch == '\t')
                        colCount++;
                    else if (ch == '\r')
                        rowCount++;
                }

                if (colCount > 0)
                    colCount = colCount / rowCount;

                colCount++;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private List<int> GetSelectedColumnIndices()
        {
            List<int> selectedColumns;

            try
            {
                selectedColumns = new List<int>();
                crCommon = fpSpread_Sheet.GetSelection(0);

                if (crCommon == null)
                {
                    selectedColumns.Add(fpSpread_Sheet.ActiveCell.Column.Index);
                }
                else
                {
                    for (int i = crCommon.Column; i < crCommon.Column + crCommon.ColumnCount; i++)
                    {
                        selectedColumns.Add(i);
                    }
                }
            }
            catch
            {
                return null;
            }
            return selectedColumns;
        }

        private DataTable ProcessSort(bool isSelectionOnly, List<ColumnInfo> lstValidColumnInfo, FarPoint.Win.Spread.SortInfo[] sorters)
        {
            int maxValidRowIndex = 0;
            int fromRowIndex = 0;
            int fromColumnIndex = 0;
            int toColumnIndex = 0;

            DataTable dtSorted = null;

            try
            {
                if (!isSelectionOnly)
                {
                    maxValidRowIndex = GetMaxValidRowIndex();
                    fromRowIndex = 1;
                    fromColumnIndex = 0;
                    toColumnIndex = fpSpread_Sheet.Columns.Count - 1;
                }
                else
                {
                    maxValidRowIndex = GetMaxValidRowIndex(crCommon);
                    fromRowIndex = crCommon.Row;
                    fromColumnIndex = crCommon.Column;
                    toColumnIndex = crCommon.Column + crCommon.ColumnCount - 1;
                }

                BeginCellEdit();

                for (int i = 0; i < lstValidColumnInfo.Count; i++)
                {
                    if (fromColumnIndex <= lstValidColumnInfo[i].ColumnIndex && toColumnIndex >= lstValidColumnInfo[i].ColumnIndex)
                    {
                        SetMissingValue(lstValidColumnInfo[i].ColumnIndex, fromRowIndex, maxValidRowIndex, true);
                        if (lstValidColumnInfo[i].ValidRowIndex < maxValidRowIndex)
                            lstValidColumnInfo[i].ValidRowIndex = maxValidRowIndex;
                    }
                }

                fpSpread_Sheet.SortRange(fromRowIndex, fromColumnIndex, maxValidRowIndex - fromRowIndex + 1, toColumnIndex - fromColumnIndex + 1, true, sorters);

                for (int i = 0; i < lstValidColumnInfo.Count; i++)
                {
                    if (fromColumnIndex <= lstValidColumnInfo[i].ColumnIndex && toColumnIndex >= lstValidColumnInfo[i].ColumnIndex)
                        SetMissingValue(lstValidColumnInfo[i].ColumnIndex, fromRowIndex, maxValidRowIndex, false);
                }

                EndCellEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            dtSorted = GetDataTableFromSelection(new FarPoint.Win.Spread.Model.CellRange(fromRowIndex, fromColumnIndex, maxValidRowIndex - fromRowIndex + 1, toColumnIndex - fromRowIndex + 1));

            return dtSorted;
        }

        #endregion

        #region [ Column Information ]

        private int GetMaxValidRowIndex()
        {
            crCommon = new FarPoint.Win.Spread.Model.CellRange(1, 0, fpSpread_Sheet.RowCount - 1, fpSpread_Sheet.ColumnCount);
            return GetMaxValidRowIndex(crCommon);
        }

        private int GetMaxValidRowIndex(FarPoint.Win.Spread.Model.CellRange cr)
        {
            int maxValidRow = 0;
            int crMaxRowIndex = 0;

            try
            {
                if (cr == null)
                {
                    if (fpSpread_Sheet.ActiveCell == null)
                        return -1;
                    else
                        return lstColumnInfo[fpSpread_Sheet.ActiveCell.Column.Index].ValidRowIndex;
                }
                else
                {
                    crMaxRowIndex = cr.Row + cr.RowCount - 1;
                    for (int i = cr.Column; i < cr.Column + cr.ColumnCount; i++)
                    {
                        if (lstColumnInfo[i].ValidRowIndex >= crMaxRowIndex)
                            return crMaxRowIndex;
                        else if (maxValidRow < lstColumnInfo[i].ValidRowIndex)
                            maxValidRow = lstColumnInfo[i].ValidRowIndex;
                    }

                    return maxValidRow;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void OnColumnSettingChanged(ColumnInfo columnInfo)
        {
            BeginCellEdit();
            ProcessColumnSettingChanged(columnInfo);
            EndCellEdit();
        }

        private void OnColumnNameChanged(int columnIndex, string columnName)
        {
            BeginCellEdit();
            fpSpread_Sheet.Cells[0, columnIndex].Value = columnName;
            EndCellEdit();
        }

        private void CopyColumnSetting(int srcColumnIndex, int dstColumnIndex)
        {
            lstColumnInfo[dstColumnIndex].DateTimeFormatIndex = lstColumnInfo[srcColumnIndex].DateTimeFormatIndex;
            lstColumnInfo[dstColumnIndex].RoundOptionIndex = lstColumnInfo[srcColumnIndex].RoundOptionIndex;
            lstColumnInfo[dstColumnIndex].DecimalPlace = lstColumnInfo[srcColumnIndex].DecimalPlace;
        }

        private void ProcessColumnSettingChanged(ColumnInfo columnInfo)
        {
            string strTemp = "";
            int column = columnInfo.ColumnIndex;

            DataTable dt = (DataTable)fpSpread_Sheet.DataSource;

            switch (columnInfo.DataType)
            {
                case DataType.NUMBER:
                    double nTemp = 0;
                    for (int i = 1; i <= columnInfo.ValidRowIndex; i++)
                    {
                        strTemp = dt.Rows[i][column].ToString().Trim();
                        if (strTemp == "" || !double.TryParse(strTemp, out nTemp))
                            fpSpread_Sheet.Cells[i, column].Value = SHEET_MISSING_VALUE;
                        else
                            fpSpread_Sheet.Cells[i, column].Value = GetProcessedDoubleString(columnInfo, nTemp);
                    }
                    break;

                case DataType.DATETIME:
                    DateTime dTemp;
                    for (int i = 1; i <= columnInfo.ValidRowIndex; i++)
                    {
                        strTemp = dt.Rows[i][column].ToString().Trim();
                        if (strTemp == "" || !DateTime.TryParse(strTemp, out dTemp))
                            fpSpread_Sheet.Cells[i, column].Value = SHEET_MISSING_VALUE;
                        else
                            fpSpread_Sheet.Cells[i, column].Value = dTemp.ToString(columnInfo.CurrentDateTimeFormat);
                    }
                    break;

                case DataType.TEXT:
                    for (int i = 1; i <= columnInfo.ValidRowIndex; i++)
                    {
                        strTemp = dt.Rows[i][column].ToString().Trim();
                        if (strTemp == "*")
                            fpSpread_Sheet.Cells[i, column].Value = "";
                    }
                    break;
            }

            float preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(column, false);
            if (fpSpread_Sheet.Columns[column].Width < preferredWidth)
                fpSpread_Sheet.Columns[column].Width = preferredWidth;
        }

        private ColumnInfo GetParsedColumnInfo(int columnIndex)
        {
            DataType dataType;
            string columnName;
            int validRowIndex = 0;

            for (int i = fpSpread_Sheet.RowCount - 1; i >= 0; i--)
            {
                if (fpSpread_Sheet.Cells[i, columnIndex].Text != "")
                {
                    validRowIndex = i;
                    break;
                }
            }

            dataType = GetColumnDataType(columnIndex, validRowIndex);

            columnName = fpSpread_Sheet.Cells[0, columnIndex].Text;

            return new ColumnInfo(fpSpread_Sheet, columnIndex, dataType, columnName, 0, 0, 0, validRowIndex);
        }

        private List<ColumnInfo> GetValidColumnInfoList()
        {
            return GetValidColumnInfoList(NameRowPolicy.Included);
        }

        private List<ColumnInfo> GetValidColumnInfoList(NameRowPolicy nameRowPolicy)
        {
            List<ColumnInfo> validColumns = null;

            try
            {
                validColumns = new List<ColumnInfo>();

                if (nameRowPolicy == NameRowPolicy.Included)
                {
                    foreach (ColumnInfo columnInfo in lstColumnInfo)
                    {
                        if (columnInfo.ValidRowIndex > 0)
                        {
                            validColumns.Add(columnInfo);
                        }
                    }
                }
                else if (nameRowPolicy == NameRowPolicy.Excluded)
                {
                    ColumnInfo columnInfo;

                    foreach (ColumnInfo colInfo in lstColumnInfo)
                    {
                        if (colInfo.ValidRowIndex > 0)
                        {
                            columnInfo = colInfo.Copy(NameRowPolicy.Excluded);
                            validColumns.Add(columnInfo);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return validColumns;
        }

        #endregion

        #region [ Selection Appearance ]

        private void ClearSheetSelection()
        {
            BeginCellEdit();

            fpSpread_Sheet.Cells[0, 0, fpSpread_Sheet.RowCount - 1, fpSpread_Sheet.ColumnCount - 1].ResetBorder();
            fpSpread_Sheet.Cells[0, 0, fpSpread_Sheet.RowCount - 1, fpSpread_Sheet.ColumnCount - 1].ResetBackColor();

            EndCellEdit();
        }

        private void DrawSheetSelection(FarPoint.Win.Spread.Model.CellRange cr)
        {
            if (cr.RowCount == 1 && cr.ColumnCount == 1)
            {
                BeginCellEdit();
                fpSpread_Sheet.SetOutlineBorder(cr, lineBorderSingleCell);
                EndCellEdit();
            }
            else
                DrawSheetSelection(cr, lineBorderMultiCells);
        }

        private void DrawSheetSelection(FarPoint.Win.Spread.Model.CellRange cr, FarPoint.Win.IBorder lineBorder)
        {
            BeginCellEdit();

            fpSpread_Sheet.SetOutlineBorder(cr, lineBorder);
            fpSpread_Sheet.Cells[cr.Row, cr.Column, cr.Row + cr.RowCount - 1, cr.Column + cr.ColumnCount - 1].BackColor = Color.LightCyan;
            fpSpread_Sheet.ActiveCell.BackColor = Color.White;

            EndCellEdit();
        }

        #endregion

        #region [ DataSource Process ]

        private DataTable GetDataTableFromSelection(FarPoint.Win.Spread.Model.CellRange cr)
        {
            return GetDataTableFromSelection(cr, NameRowPolicy.Included);
        }

        private DataTable GetDataTableFromSelection(FarPoint.Win.Spread.Model.CellRange cr, NameRowPolicy nameRowPolicy)
        {
            if (cr == null)
                return null;

            DataTable dt = new DataTable();
            FarPoint.Win.Spread.Model.IArraySupport ias = (FarPoint.Win.Spread.Model.IArraySupport)fpSpread_Sheet.Models.Data;
            object[,] data = ias.GetArray(crCommon.Row, crCommon.Column, crCommon.RowCount, crCommon.ColumnCount);
            object[] row = new object[crCommon.ColumnCount];

            if (nameRowPolicy == NameRowPolicy.Included)
            {
                for (int i = 0; i < crCommon.ColumnCount; i++)
                {
                    dt.Columns.Add("C" + (i + 1).ToString(), typeof(string));

                    row[i] = fpSpread_Sheet.Models.Data.GetValue(0, crCommon.Column + i);
                }
                dt.Rows.Add(row);
            }

            // Selection
            for (int i = 1; i <= crCommon.RowCount; i++)
            {
                row = new object[crCommon.ColumnCount];
                for (int j = 0; j < row.Length; j++)
                    row[j] = data[i - 1, j];

                dt.Rows.Add(row);
            }

            return dt;
        }

        private DataTable GetEmptyDataTable(int rowCount, int colCount)
        {
            DataTable dt = new DataTable();

            DataColumn dc;
            DataRow dr;

            for (int i = 0; i < colCount; i++)
            {
                dc = new DataColumn("C" + (i + 1).ToString(), typeof(string));
                dt.Columns.Add(dc);
            }

            for (int i = 0; i < rowCount; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Converts a datatable to be suitable for external purpose or internal uses.
        /// </summary>
        /// <param name="isNormalDataTable">Whether the result table should be external or internal.</param>
        /// <param name="dtSource">The source DataTable</param>
        /// <param name="columnInfo">The columninfo list of source datatable.</param>
        /// <returns></returns>
        private static DataTable ConvertDataTable(bool isNormalDataTable, DataTable dtSource, List<ColumnInfo> columnInfo)
        {
            System.Data.DataColumn column;
            double nTemp;
            DateTime dTemp;

            int colIndex = 0;
            string colName = string.Empty;

            dtSource.AcceptChanges();
            DataTable dtTarget = dtSource.Copy();

            try
            {
                if (isNormalDataTable)
                {
                    for (int i = 0; i < columnInfo.Count; i++)
                    {
                        colIndex = columnInfo[i].ColumnIndex;
                        colName = dtTarget.Columns[colIndex].ColumnName;

                        if (columnInfo[i].DataType == DataType.NUMBER)
                        {
                            column = dtTarget.Columns.Add("TEMP_" + colName, typeof(double));

                            for (int j = 0; j < dtTarget.Rows.Count; j++)
                            {
                                if (dtTarget.Rows[j][colIndex] != null)
                                {
                                    if (double.TryParse(dtTarget.Rows[j][colIndex].ToString(), out nTemp))
                                    {
                                        dtTarget.Rows[j]["TEMP_" + colName] = nTemp;
                                    }
                                }
                            }

                            dtTarget.Columns.RemoveAt(colIndex);
                            dtTarget.Columns["TEMP_" + colName].ColumnName = colName;
                            dtTarget.Columns[colName].SetOrdinal(colIndex);
                        }
                        else if (columnInfo[i].DataType == DataType.DATETIME)
                        {
                            column = dtTarget.Columns.Add("TEMP_" + colName, typeof(DateTime));

                            for (int j = 0; j < dtTarget.Rows.Count; j++)
                            {
                                if (dtTarget.Rows[j][colIndex] != null)
                                {
                                    if (DateTime.TryParse(dtTarget.Rows[j][colIndex].ToString(), out dTemp))
                                    {
                                        dtTarget.Rows[j]["TEMP_" + colName] = dTemp;
                                    }
                                }
                            }

                            dtTarget.Columns.RemoveAt(colIndex);
                            dtTarget.Columns["TEMP_" + colName].ColumnName = colName;
                            dtTarget.Columns[colName].SetOrdinal(colIndex);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < columnInfo.Count; i++)
                    {
                        colIndex = columnInfo[i].ColumnIndex;
                        colName = dtTarget.Columns[colIndex].ColumnName;

                        column = dtTarget.Columns.Add("TEMP_" + colName, typeof(string));

                        for (int j = 0; j < dtTarget.Rows.Count; j++)
                        {
                            if (dtTarget.Rows[j][colIndex] != null)
                            {
                                dtTarget.Rows[j]["TEMP_" + colName] = dtTarget.Rows[j][colIndex].ToString();
                            }
                        }

                        dtTarget.Columns.RemoveAt(colIndex);
                        dtTarget.Columns["TEMP_" + colName].ColumnName = colName;
                        dtTarget.Columns[colName].SetOrdinal(colIndex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return dtTarget;
        }

        /// <summary>
        /// Removes columns and rows which are not necessary.
        /// </summary>
        /// <param name="dataTable">The source datatable</param>
        /// <param name="lstValidColumnInfo">The columninfo list of source datatable.</param>
        /// <returns></returns>
        private static DataTable GetTrimmedDataTable(DataTable dataTable, List<ColumnInfo> lstValidColumnInfo)
        {
            return GetTrimmedDataTable(dataTable, lstValidColumnInfo, TrimPolicy.Both);
        }

        /// <summary>
        /// Removes columns and rows which are not necessary.
        /// </summary>
        /// <param name="dataTable">The source datatable</param>
        /// <param name="lstValidColumnInfo">The columninfo list of source datatable.</param>
        /// <param name="trimPolicy">Specifies trim policy.</param>
        /// <returns></returns>
        private static DataTable GetTrimmedDataTable(DataTable dataTable, List<ColumnInfo> lstValidColumnInfo, TrimPolicy trimPolicy)
        {
            dataTable.AcceptChanges();
            DataTable dt = dataTable.Copy();

            int index = lstValidColumnInfo.Count - 1;
            int maxValidRowIndex = 0;

            if(trimPolicy == TrimPolicy.Both)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (lstValidColumnInfo[index].ColumnIndex != i)
                    {
                        dt.Columns.RemoveAt(i);
                    }
                    else
                    {
                        if (maxValidRowIndex < lstValidColumnInfo[index].ValidRowIndex)
                            maxValidRowIndex = lstValidColumnInfo[index].ValidRowIndex;

                        if (index > 0)
                            index--;
                    }
                }

                for (int i = dt.Rows.Count - 1; i > maxValidRowIndex; i--)
                    dt.Rows.RemoveAt(i);
            }
            else if(trimPolicy == TrimPolicy.Column)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (lstValidColumnInfo[index].ColumnIndex != i)
                    {
                        dt.Columns.RemoveAt(i);
                    }
                }
            }
            else if(trimPolicy == TrimPolicy.Row)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (lstValidColumnInfo[index].ColumnIndex == i)
                    {
                        if (maxValidRowIndex < lstValidColumnInfo[index].ValidRowIndex)
                            maxValidRowIndex = lstValidColumnInfo[index].ValidRowIndex;

                        if (index > 0)
                            index--;
                    }
                }

                for (int i = dt.Rows.Count - 1; i > maxValidRowIndex; i--)
                    dt.Rows.RemoveAt(i);
            }

            dt.AcceptChanges();
            return dt;
        }

        #endregion

        #endregion

        #region " EVENT HANDLER "

        private void DataView_Load(object sender, EventArgs e)
        {
        }

        private void cmsDataView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessDataViewContextMenu(e.ClickedItem.Name);
        }

        private void fpSpread_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                fpSpread.ActiveSheet.SetActiveCell(0, e.Column);
                fpSpread_Sheet.AddSelection(1, e.Column, fpSpread_Sheet.RowCount - 1, 1);
            }
            else if (fpSpread_Sheet.SelectionCount < 1 || !fpSpread_Sheet.GetSelection(0).Contains(e.Row, e.Column))
            {
                fpSpread.ActiveSheet.SetActiveCell(e.Row, e.Column);

                if (e.Button == MouseButtons.Right)
                {
                    ClearSheetSelection();
                    DrawSheetSelection(new FarPoint.Win.Spread.Model.CellRange(e.Row, e.Column, 1, 1));
                }
            }
        }

        private void fpSpread_SelectionChanging(object sender, FarPoint.Win.Spread.SelectionChangingEventArgs e)
        {
            ProcessSelectionChange(e.Range);
        }

        private void fpSpread_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            if (e.Range == null)
                return;

            crCommon = e.Range;

            if (crCommon.RowCount == 1 && crCommon.ColumnCount == 1)
            {
                ClearSheetSelection();
                DrawSheetSelection(crCommon, lineBorderSingleCell);
            }
            else
            {
                ProcessSelectionChange(crCommon);
            }
        }

        private void ProcessSelectionChange(FarPoint.Win.Spread.Model.CellRange cellRange)
        {
            ClearSheetSelection();

            if (cellRange.Row < 0 || cellRange.Column < 0)
            {
                if (cellRange.Row < 0)
                {
                    if (cellRange.Column < 0)
                    {
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(1, 0, fpSpread_Sheet.RowCount - 1, fpSpread_Sheet.ColumnCount);
                    }
                    else
                    {
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(1, cellRange.Column, fpSpread_Sheet.RowCount - 1, cellRange.ColumnCount);
                    }
                }
                else
                {
                    if (cellRange.Column < 0)
                    {
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(cellRange.Row, 0, cellRange.RowCount, fpSpread_Sheet.ColumnCount);
                    }
                    else
                    {
                        crCommon = new FarPoint.Win.Spread.Model.CellRange(cellRange.Row, cellRange.Column, cellRange.RowCount, cellRange.ColumnCount);
                    }
                }

                DrawSheetSelection(crCommon, lineBorderMultiCells);
            }
            else
            {
                DrawSheetSelection(cellRange, lineBorderMultiCells);
            }
        }

        private void fpSpread_Sheet_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            if (e.Row >= 0 && e.Column >= 0)
            {
                BeginCellEdit();
                ProcessCellChanged(e.Row, e.Column);
                EndCellEdit();
            }
        }

        private void fpSpread_KeyDown(object sender, KeyEventArgs e)
        {
            isControlKeyDown = e.Control;

            if (fpSpread_Sheet.ActiveCell.Row.Index == fpSpread_Sheet.RowCount - 1)
            {
                fpSpread_Sheet.Rows.Add(fpSpread_Sheet.RowCount, 1);
                fpSpread_Sheet.SetActiveCell(fpSpread_Sheet.RowCount - 2, fpSpread_Sheet.ActiveCell.Column.Index);

                ClearSheetSelection();
                DrawSheetSelection(new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1));
            }

            if (e.KeyCode == Keys.A && e.Control)
            {
                #region " Processing CTRL + A "

                int activeRowIndex = fpSpread_Sheet.ActiveCell.Row.Index;
                int activeColIndex = fpSpread_Sheet.ActiveCell.Column.Index;

                int rightIndex = activeColIndex;
                int leftIndex = activeColIndex;
                int bottomIndex = 1;

                if (activeRowIndex <= lstColumnInfo[activeColIndex].ValidRowIndex)
                {
                    while (rightIndex < fpSpread_Sheet.Columns.Count
                        && lstColumnInfo[rightIndex].ValidRowIndex > 0)
                    {
                        if (bottomIndex < lstColumnInfo[rightIndex].ValidRowIndex)
                            bottomIndex = lstColumnInfo[rightIndex].ValidRowIndex;

                        rightIndex++;
                    }
                    rightIndex--;

                    while (leftIndex >= 0
                        && lstColumnInfo[leftIndex].ValidRowIndex > 0)
                    {
                        if (bottomIndex < lstColumnInfo[leftIndex].ValidRowIndex)
                            bottomIndex = lstColumnInfo[leftIndex].ValidRowIndex;

                        leftIndex--;
                    }
                    leftIndex++;
                }
                else
                {
                    leftIndex = 0;
                    rightIndex = fpSpread_Sheet.ColumnCount - 1;
                    bottomIndex = fpSpread_Sheet.RowCount - 1;
                }

                fpSpread_Sheet.ClearSelection();
                ClearSheetSelection();

                fpSpread_Sheet.AddSelection(1, leftIndex, bottomIndex, rightIndex - leftIndex + 1);
                DrawSheetSelection(new FarPoint.Win.Spread.Model.CellRange(1, leftIndex, bottomIndex, rightIndex - leftIndex + 1));

                #endregion
            }
            else if (e.KeyCode == Keys.Delete)
            {
                #region " Processing Delete Key "

                if (fpSpread_Sheet.SelectionCount > 0)
                    crCommon = fpSpread_Sheet.GetSelection(0);
                else
                    crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

                RemoveCells(crCommon);

                #endregion
            }
            else if (e.KeyCode == Keys.X && e.Control)
            {
                #region " Processing CTRL + X "

                if (fpSpread_Sheet.SelectionCount > 0)
                    crCommon = fpSpread_Sheet.GetSelection(0);
                else
                    crCommon = new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, 1, 1);

                CopyCellsToClipboard(crCommon);
                RemoveCells(crCommon);

                #endregion
            }
        }

        private void CopyCellsToClipboard(FarPoint.Win.Spread.Model.CellRange crCommon)
        {
            FarPoint.Win.Spread.Model.IArraySupport ias = null;
            string strCells = string.Empty;
            string strRow = string.Empty;
            object [,] arrCells = null;

            try
            {
                ias = (FarPoint.Win.Spread.Model.IArraySupport)fpSpread_Sheet.Models.Data;
                arrCells = ias.GetArray(crCommon.Row, crCommon.Column, crCommon.RowCount, crCommon.ColumnCount);

                for (int i = 0; i < crCommon.RowCount; i++)
                {
                    strRow = string.Empty;

                    for (int j = 0; j < crCommon.ColumnCount; j++)
                        strRow += "\t" + (string)arrCells[i, j];

                    strCells += strRow.Substring(1) + "\r\n";
                }

                Clipboard.SetData(System.Windows.Forms.DataFormats.StringFormat, strCells);
            }
            catch
            {
                MessageBox.Show("Failed to copy to clipboard.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
            }  
        }

        [System.Diagnostics.DebuggerStepThrough()]
        private void fpSpread_KeyUp(object sender, KeyEventArgs e)
        {
            isControlKeyDown = e.Control;
        }

        private void fpSpread_DragFillBlock(object sender, FarPoint.Win.Spread.DragFillBlockEventArgs e)
        {
            if (e.RowBegin > 0)
            {
                int increment = 0;
                int row = e.RowBegin;
                int column = e.ColumnBegin;
                string strValue = fpSpread_Sheet.Cells[row, column].Text.Trim();

                if (strValue == "" || strValue == SHEET_MISSING_VALUE || !CheckValidInput(column, ref strValue) || isControlKeyDown == true)
                    return;

                e.Cancel = true;

                switch (lstColumnInfo[column].DataType)
                {
                    case DataType.DATETIME:
                        #region " DOWN "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Down)
                        {
                            for (int i = row + 1; i < row + e.NumberToCopy + 1; i++)
                            {
                                increment++;
                                fpSpread_Sheet.SetValue(i, column, DateTime.Parse(strValue).AddDays(increment).ToString(lstColumnInfo[column].CurrentDateTimeFormat));
                            }
                        }
                        #endregion
                        #region " UP "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Up)
                        {
                            for (int i = row - 1; i >= row - e.NumberToCopy; i--)
                            {
                                increment--;
                                fpSpread_Sheet.SetValue(i, column, DateTime.Parse(strValue).AddDays(increment).ToString(lstColumnInfo[column].CurrentDateTimeFormat));
                            }
                        }
                        #endregion
                        #region " RIGHT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Right)
                        {
                            for (int i = column + 1; i < column + e.NumberToCopy + 1; i++)
                            {
                                CopyColumnSetting(column, i);

                                increment++;
                                fpSpread_Sheet.SetValue(row, i, DateTime.Parse(strValue).AddDays(increment).ToString(lstColumnInfo[column].CurrentDateTimeFormat));
                            }
                        }
                        #endregion
                        #region " LEFT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Left)
                        {
                            for (int i = column - 1; i >= column - e.NumberToCopy; i--)
                            {
                                CopyColumnSetting(column, i);

                                increment--;
                                fpSpread_Sheet.SetValue(row, i, DateTime.Parse(strValue).AddDays(increment).ToString(lstColumnInfo[column].CurrentDateTimeFormat));
                            }
                        }
                        #endregion
                        break;
                    case DataType.NUMBER:
                        #region " DOWN "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Down)
                        {
                            for (int i = row + 1; i < row + e.NumberToCopy + 1; i++)
                            {
                                increment++;
                                fpSpread_Sheet.SetValue(i, column, double.Parse(strValue) + increment);
                            }
                        }
                        #endregion
                        #region " UP "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Up)
                        {
                            for (int i = row - 1; i >= row - e.NumberToCopy; i--)
                            {
                                increment--;
                                fpSpread_Sheet.SetValue(i, column, double.Parse(strValue) + increment);
                            }
                        }
                        #endregion
                        #region " RIGHT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Right)
                        {
                            for (int i = column + 1; i < column + e.NumberToCopy + 1; i++)
                            {
                                CopyColumnSetting(column, i);

                                increment++;
                                fpSpread_Sheet.SetValue(row, i, double.Parse(strValue) + increment);
                            }
                        }
                        #endregion
                        #region " LEFT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Left)
                        {
                            for (int i = column - 1; i >= column - e.NumberToCopy; i--)
                            {
                                CopyColumnSetting(column, i);

                                increment--;
                                fpSpread_Sheet.SetValue(row, i, double.Parse(strValue) + increment);
                            }
                        }
                        #endregion
                        break;
                    case DataType.TEXT:
                        #region " COMMON "
                        int parsed;
                        int start = 0;
                        int end = -1;
                        string strPre = string.Empty;
                        string strPost = string.Empty;

                        for (int i = strValue.Length - 1; i >= 0; i--)
                        {
                            if (int.TryParse(strValue[i].ToString(), out parsed))
                            {
                                end = i;
                                strPost = (end == strValue.Length - 1) ? string.Empty : strValue.Substring(end + 1);
                                break;
                            }
                        }

                        for (int i = end; i >= 0; i--)
                        {
                            if (!int.TryParse(strValue[i].ToString(), out parsed))
                            {
                                start = (i == end) ? end : i + 1;
                                strPre = strValue.Substring(0, start);
                                break;
                            }
                        }

                        #endregion
                        #region " DOWN "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Down)
                        {
                            if (end >= 0)
                            {
                                string strNumber = strValue.Substring(start, end - start + 1);

                                for (int i = row + 1; i < row + e.NumberToCopy + 1; i++)
                                {
                                    increment++;
                                    fpSpread_Sheet.SetValue(i, column, strPre + (double.Parse(strNumber) + increment).ToString() + strPost);
                                }
                            }
                            else if (end == -1)
                            {
                                for (int i = row + 1; i < row + e.NumberToCopy + 1; i++)
                                    fpSpread_Sheet.SetValue(i, column, strValue);
                            }
                        }
                        #endregion
                        #region " UP "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Up)
                        {
                            if (end >= 0)
                            {
                                string strNumber = strValue.Substring(start, end - start + 1);

                                for (int i = row - 1; i >= row - e.NumberToCopy; i--)
                                {
                                    increment--;
                                    fpSpread_Sheet.SetValue(i, column, strPre + (double.Parse(strNumber) + increment).ToString() + strPost);
                                }
                            }
                            else
                            {
                                for (int i = row - 1; i < row - e.NumberToCopy; i--)
                                    fpSpread_Sheet.SetValue(i, column, strValue);
                            }
                        }
                        #endregion
                        #region " RIGHT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Right)
                        {
                            if (end >= 0)
                            {
                                string strNumber = strValue.Substring(start, end - start + 1);

                                for (int i = column + 1; i < column + e.NumberToCopy + 1; i++)
                                {
                                    increment++;
                                    fpSpread_Sheet.SetValue(row, i, strPre + (double.Parse(strNumber) + increment).ToString() + strPost);
                                }
                            }
                            else
                            {
                                for (int i = column + 1; i < column + e.NumberToCopy + 1; i++)
                                    fpSpread_Sheet.SetValue(row, i, strValue);
                            }
                        }
                        #endregion
                        #region " LEFT "
                        if (e.Direction == FarPoint.Win.Spread.FillDirection.Left)
                        {
                            if (end >= 0)
                            {
                                string strNumber = strValue.Substring(start, end - start + 1);

                                for (int i = column - 1; i >= column - e.NumberToCopy; i--)
                                {
                                    increment--;
                                    fpSpread_Sheet.SetValue(row, i, strPre + (double.Parse(strNumber) + increment).ToString() + strPost);
                                }
                            }
                            else
                            {
                                for (int i = column - 1; i >= column - e.NumberToCopy; i--)
                                    fpSpread_Sheet.SetValue(row, i, strValue);
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
            }

            fpSpread_Sheet.ClearSelection();
        }

        private void fpSpread_ClipboardPasting(object sender, FarPoint.Win.Spread.ClipboardPastingEventArgs e)
        {
            e.Handled = true;

            #region [ Local Variable ]

            string[] arrRows;
            IDataObject data;
            string strValue;
            int rowCount = 0;
            int colCount = 0;
            int requiredRowsCount = 0;
            int requiredColsCount = 0;
            float preferredWidth = 0;

            DataTable dt;

            #endregion

            try
            {
                #region [ Data Validation ]

                data = Clipboard.GetDataObject();

                if (!data.GetDataPresent(typeof(System.String)))
                    return;

                strValue = data.GetData(typeof(System.String)).ToString();

                if (!GetRowColumnCount(strValue, ref rowCount, ref colCount))
                {
                    MessageBox.Show("Can't recognize format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (rowCount == 0)
                    return;

                #endregion

                #region [ Column & Row Add ]

                requiredRowsCount = fpSpread_Sheet.ActiveCell.Row.Index + rowCount - fpSpread_Sheet.RowCount;
                requiredColsCount = fpSpread_Sheet.ActiveCell.Column.Index + colCount - fpSpread_Sheet.ColumnCount;

                if (requiredRowsCount > 0)
                    fpSpread_Sheet.Rows.Add(fpSpread_Sheet.Rows.Count, requiredRowsCount + 1);

                if (requiredColsCount > 0)
                {
                    dt = (DataTable)fpSpread_Sheet.DataSource;
                    int endColIndex = fpSpread_Sheet.Columns.Count + requiredColsCount;
                    for (int i = fpSpread_Sheet.Columns.Count; i < endColIndex; i++)
                    {
                        fpSpread_Sheet.Columns.Add(i, 1);
                        dt.Columns.Add().SetOrdinal(i);

                        fpSpread_Sheet.Columns[i].CellType = CommonCellType;

                        lstColumnInfo.Add(new ColumnInfo(fpSpread_Sheet, i, DataType.NUMBER, string.Empty, 0, 0, 0, 0));
                        lstColumnInfo[i].delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                        lstColumnInfo[i].delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);

                        preferredWidth = fpSpread_Sheet.GetPreferredColumnWidth(i, false);

                        if (preferredWidth < COLUMN_MIN_WIDTH)
                            fpSpread_Sheet.Columns[i].Width = COLUMN_MIN_WIDTH;
                        else
                            fpSpread_Sheet.Columns[i].Width = preferredWidth;

                        fpSpread_Sheet.Columns[i].Label = lstColumnInfo[i].ColumnID;
                    }
                }

                #endregion

                #region [ Data Filling ]

                arrRows = strValue.Split(new string[] { "\t", "\r\n" }, StringSplitOptions.None);
                int index = 0;

                BeginCellEdit();
                for (int i = fpSpread_Sheet.ActiveRowIndex; i < fpSpread_Sheet.ActiveRowIndex + rowCount; i++)
                {
                    for (int j = fpSpread_Sheet.ActiveColumnIndex; j < fpSpread_Sheet.ActiveColumnIndex + colCount; j++)
                    {
                        fpSpread_Sheet.Models.Data.SetValue(i, j, arrRows[index++]);
                    }
                }

                if(fpSpread_Sheet.ActiveRowIndex == 0)
                {
                    for(int i=fpSpread_Sheet.ActiveColumnIndex; i<fpSpread_Sheet.ActiveColumnIndex + colCount; i++)
                    {
                        lstColumnInfo[i].ColumnName = (string)fpSpread_Sheet.Models.Data.GetValue(0, i);
                    }
                }

                #endregion

                #region [ ColumnType & Missing Value & Column Width ]

                for (int j = fpSpread_Sheet.ActiveColumnIndex; j < fpSpread_Sheet.ActiveColumnIndex + colCount; j++)
                {
                    RefreshColumn(j);
                }
                EndCellEdit();

                #endregion

                #region [ Draw Selection ]

                ClearSheetSelection();
                DrawSheetSelection(new FarPoint.Win.Spread.Model.CellRange(fpSpread_Sheet.ActiveCell.Row.Index, fpSpread_Sheet.ActiveCell.Column.Index, rowCount, colCount));

                #endregion
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void RefreshColumn(int columnIndex)
        {
            ColumnInfo columnInfo;
            
            try
            {
                columnInfo = lstColumnInfo[columnIndex];

                if (columnInfo.ValidRowIndex == 0)
                {
                    columnInfo = GetParsedColumnInfo(columnIndex);
                    lstColumnInfo[columnIndex] = columnInfo;

                    columnInfo.delColumnSettingChanged += new ColumnInfo.DelColumnSettingChanged(OnColumnSettingChanged);
                    columnInfo.delColumnNameChanged += new ColumnInfo.DelColumnNameChanged(OnColumnNameChanged);

                    fpSpread_Sheet.Columns[columnIndex].Label = columnInfo.ColumnID;
                }
                else
                {
                    for (int i = fpSpread_Sheet.RowCount - 1; i > 0; i--)
                    {
                        if (fpSpread_Sheet.Models.Data.GetValue(i, columnIndex) != null && fpSpread_Sheet.Models.Data.GetValue(i, columnIndex).ToString() != string.Empty)
                        {
                            columnInfo.ValidRowIndex = i;
                            break;
                        }
                    }
                }

                ProcessColumnSettingChanged(columnInfo);
                columnInfo.DataType = columnInfo.DataType;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region " EVENT & DELEGATE "

        public event NewDataSourceSplitedHandler NewDataSourceSplited;

        #endregion
    }
}
