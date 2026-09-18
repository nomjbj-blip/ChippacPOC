/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : IniHandle.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Framework::DACrux 의 Login Window
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace DACrux.Framework
{
    public class Utillity
    {
        #region [ Spread ]
        public static void InitSpread(params FpSpread[] spreadList)
        {
            try
            {
                FarPoint.Win.Spread.CellType.ColumnHeaderRenderer renderer = new FarPoint.Win.Spread.CellType.ColumnHeaderRenderer();
                renderer.WordWrap = false;

                foreach (FarPoint.Win.Spread.FpSpread fp in spreadList)
                {
                    foreach (FarPoint.Win.Spread.SheetView sv in fp.Sheets)
                    {
                        sv.Reset();

                        sv.ColumnHeader.Rows[0].Renderer = renderer;
                        sv.DataAutoSizeColumns = false;
                        sv.DataAutoCellTypes = false;

                        sv.ColumnCount = 0;
                        sv.RowCount = 0;

                        sv.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                        sv.OperationMode = OperationMode.SingleSelect;

                        FarPoint.Win.Spread.DefaultSkins.Default.Apply(sv);
                        //FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(sv);
                    }
                    fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                    fp.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;

                    FarPoint.Win.Spread.DefaultSkins.Default.Apply(fp);
                    //FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(fp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void VisibleSpreadColumns(SheetView sheet, string[] Columns, bool IsView)
        {
            try
            {
                for (int i = 0; i < sheet.ColumnCount; i++)
                {
                    sheet.Columns[i].Visible = !IsView;
                }

                DataTable dt = sheet.GetDataView(false).Table;

                for (int i = 0; i < Columns.Length; i++)
                {
                    int nIndex = dt.Columns.IndexOf(Columns[i]);

                    if (nIndex > -1)
                    {
                        sheet.Columns[nIndex].Visible = IsView;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AdjustSpread(SheetView sheet, int minColumnWidth = 100, int defultPlaces = 0, bool bShowSeparator = true)
        {
            try
            {
                DataTable dt = (sheet.GetDataView(false)).Table;

                for (int col = 0; col < sheet.ColumnCount; col++)
                {
                    // DataType Setting
                    switch (dt.Columns[col].DataType.Name.ToUpper())
                    {
                        case "STRING":
                            TextCellType text = new TextCellType();
                            sheet.Columns[col].CellType = text;
                            sheet.Columns[col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[col].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DECIMAL":
                            NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = defultPlaces;
                            num.ShowSeparator = bShowSeparator;
                            sheet.Columns[col].CellType = num;
                            sheet.Columns[col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[col].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DOUBLE":
                        case "INT32":
                            NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
                            intType.DecimalPlaces = 0;
                            intType.ShowSeparator = bShowSeparator;
                            sheet.Columns[col].CellType = intType;
                            sheet.Columns[col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[col].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DATETIME":
                            DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                            date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                            sheet.Columns[col].CellType = date;
                            sheet.Columns[col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[col].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[col].DataType.Name), "Notice"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[col].CellType = type;
                            sheet.Columns[col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[col].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                    }
                    sheet.Columns[col].Width = (sheet.GetPreferredColumnWidth(col, true, true) > minColumnWidth ? (sheet.GetPreferredColumnWidth(col, true, true) + 15) : minColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetSpreadData(System.Data.DataTable dt, FpSpread spread, int minColumnWidth = 100, int defultPlaces = 0, bool bShowSeparator = true)
        {
            SetSpreadData(dt, spread.ActiveSheet, minColumnWidth, defultPlaces, bShowSeparator);
        }
        public static void SetSpreadData(System.Data.DataTable dt, SheetView sheet, int minColumnWidth = 100, int defultPlaces = 0, bool bShowSeparator = true)
        {
            try
            {
                sheet.AutoCalculation = false;

                sheet.DataSource = dt;
                //sheet.Columns.AddRange(dt.Columns);
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    // DataType Setting
                    switch (dt.Columns[c].DataType.Name.ToUpper())
                    {
                        case "STRING":
                            TextCellType text = new TextCellType();
                            sheet.Columns[c].CellType = text;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                        case "DECIMAL":
                            NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = defultPlaces;
                            num.ShowSeparator = bShowSeparator;
                            sheet.Columns[c].CellType = num;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                        case "DOUBLE":
                        case "INT32":
                            NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
                            intType.DecimalPlaces = 0;
                            intType.ShowSeparator = bShowSeparator;
                            sheet.Columns[c].CellType = intType;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                        case "DATETIME":
                            DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                            date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                            sheet.Columns[c].CellType = date;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "Notice"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = type;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                    }
                    sheet.Columns[c].Width = sheet.GetPreferredColumnWidth(c, false);// > minColumnWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minColumnWidth);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PivotInASE(DataTable dtSource, string[] sTargetRows, string sSiteCountCol = "SITE_COUNT", string sValColName = "SITE")
        {
            DataTable dt = null;

            try
            {
                if (dtSource == null || dtSource.Rows.Count < 1)
                    return null;

                dt = new DataTable();

                // sTargetRows 컬럼 생성
                foreach (string sTargetRow in sTargetRows)
                {
                    string columnName = dtSource.Columns[sTargetRow].ColumnName;
                    Type type = dtSource.Columns[sTargetRow].DataType;
                    dt.Columns.Add(columnName, type);
                }

                // Average 컬럼 생성    
                dt.Columns.Add("AVERAGE", typeof(decimal));

                int nMaxSite = Convert.ToInt32(dtSource.Compute(string.Format("MAX([{0}])", sSiteCountCol), "1=1"));

                for (int i = 0; i < nMaxSite; i++)
                {
                    dt.Columns.Add(string.Format("{0} {1}", sValColName, i + 1), typeof(decimal));
                }

                // Pivot 데이터 Row 생성
                foreach (DataRow dr in dtSource.Rows)
                {
                    bool IsInsert = true;

                    // 현재 데이터가 이미 추가되어 있는지 체크한다.
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        bool IsContinue = false;
                        foreach (string sTargetRow in sTargetRows)
                        {
                            // sTargetRow 값이 하나라도 다르다면 신규 Row 추가
                            if (dt.Rows[row][sTargetRow].ToString() != dr[sTargetRow].ToString())
                            {
                                IsContinue = true;
                                break;
                            }
                        }

                        if (IsContinue)
                            continue;

                        // 다르지 않고 이미 추가된 Row가 있다면 데이터 입력
                        string sColName = string.Format("{0} {1}", sValColName, dr["SITE_NUM"].ToString());
                        dt.Rows[row][sColName] = dr["RAW_VALUE"];
                        IsInsert = false;

                        if (!IsInsert)
                            break;
                    }

                    // 새로운 행을 추가한다.
                    if (IsInsert)
                    {
                        DataRow drRow = dt.NewRow();
                        foreach (string sTargetRow in sTargetRows)
                        {
                            drRow[sTargetRow] = dr[sTargetRow];
                        }

                        string sColName = string.Format("{0} {1}", sValColName, dr["SITE_NUM"].ToString());
                        drRow[sColName] = dr["RAW_VALUE"];

                        dt.Rows.Add(drRow);
                    }    
                }

                double dbSum = 0;
                double dbTemp = 0;
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    int iCount = 0;
                    dbSum = 0;
                    for (int col = sTargetRows.Length + 1; col < dt.Columns.Count; col++)
                    {
                        if (double.TryParse(dt.Rows[row][col].ToString(), out dbTemp))
                        {
                            dbSum += dbTemp;
                            iCount++;
                        }
                    }

                    dt.Rows[row]["AVERAGE"] = dbSum / iCount;
                }

                dt.AcceptChanges();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// DataTable Pivot
        /// ------------------------------------------------------------------------------------------------------
        ///  TargetRows[0]  | TargetRows[1]..| sPivotRowsName | PivotCols[0]  | PivotCols[1]  | PivotCols[2]| ...
        /// ------------------------------------------------------------------------------------------------------
        ///                                  | PivotRows[0]   |               |               |             |
        ///                                  | PivotRows[1]   |               |               |             |
        ///                                  | PivotRows[2]   |               |               |             |
        ///                                  | PivotRows[3]   |               |               |             |
        ///                                  | ...            |               |               |             |
        /// </summary>
        /// <param name="sourceDr">Data Rows</param>
        /// <param name="sTargetRows">Pivot 기준 컬럼 데이타</param>
        /// <param name="sPivotRows">행으로 변환될 컬럼</param>
        /// <param name="sPivotRowsName">행으로 변환될 컬럼들의 컬럼명</param>
        /// <param name="sPivotCols">열로 변환될 컬럼</param>
        /// <returns></returns>
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, false);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols, bool bSetDefault)
        {
            DataTable dt = null;

            try
            {
                if (sourceDr == null || sourceDr.Length < 1)
                    return null;

                if (sPivotCols.Length > 1 && sPivotCols.Length != sPivotRows.Length)
                    return null;


                dt = new DataTable();

                // sTargetRows 컬럼 생성
                foreach (string sTargetRow in sTargetRows)
                {
                    string columnName = sourceDr[0].Table.Columns[sTargetRow].ColumnName;
                    Type type = sourceDr[0].Table.Columns[sTargetRow].DataType;
                    dt.Columns.Add(columnName, type);
                }

                // sPivotRows 컬럼 생성
                if (sPivotCols.Length == 1)
                    dt.Columns.Add(sPivotRowsName, typeof(string));

                // sPivotCols 컬럼 생성
                DataSet dsTemp = new DataSet();
                foreach (string sPivotCol in sPivotCols)
                {
                    dsTemp.Tables.Add(sPivotCol);
                }
                // 임시 DataSet에 각각의 컬럼을 넣는다.
                foreach (DataRow dr in sourceDr)
                {
                    for (int i = 0; i < sPivotCols.Length; i++)
                    {
                        string columnName = dr[sPivotCols[i]].ToString();
                        Type type = dr.Table.Columns[sPivotRows[i]].DataType;

                        if (!dsTemp.Tables[sPivotCols[i]].Columns.Contains(columnName))
                        {
                            dsTemp.Tables[sPivotCols[i]].Columns.Add(columnName, type);
                        }
                    }
                }
                // 임시 DataSet의 테이블 컬럼을 하나의 테이블 컬럼으로 연결해서 넣는다.
                foreach (string sPivotCol in sPivotCols)
                {
                    foreach (DataColumn dc in dsTemp.Tables[sPivotCol].Columns)
                    {
                        if (!dt.Columns.Contains(dc.ColumnName))
                        {
                            if (bSetDefault)
                            {
                                dc.AllowDBNull = bSetDefault;

                                //switch (dc.DataType.Name.ToUpper())
                                //{
                                //    case "STRING":
                                //        dc.DefaultValue = string.Empty;
                                //        break;
                                //    case "DECIMAL":
                                //        dc.DefaultValue = double.NaN;
                                //        break;
                                //    case "INT32":
                                //        dc.DefaultValue = 0;
                                //        break;
                                //    case "DATETIME":
                                //        dc.DefaultValue = DateTime.Today;
                                //        break;
                                //}
                            }
                            dt.Columns.Add(dc.ColumnName, dc.DataType);
                        }
                    }
                }

                // Pivot 데이터 Row 생성
                foreach (DataRow dr in sourceDr)
                {
                    bool IsInsert = true;

                    // 현재 입력된 데이터 체크
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {

                        bool IsContinue = false;
                        foreach (string sTargetRow in sTargetRows)
                        {
                            // sTargetRow 중 하나라도 다르면 다음 행 비교
                            if (dt.Rows[row][sTargetRow].ToString() != dr[sTargetRow].ToString())
                            {
                                IsContinue = true;
                                break;
                            }
                        }

                        if (IsContinue)
                            continue;

                        if (sPivotCols.Length == 1)
                        {
                            for (int i = 0; i < sPivotRows.Length; i++)
                            {
                                if (dt.Rows[row + i][sPivotRowsName].ToString() == sPivotRows[i])
                                {
                                    dt.Rows[row + i][dr[sPivotCols[0]].ToString()] = dr[sPivotRows[i]];
                                    IsInsert = false;
                                    //break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < sPivotCols.Length; i++)
                            {
                                dt.Rows[row][dr[sPivotCols[i]].ToString()] = dr[sPivotRows[i]];
                                IsInsert = false;
                            }
                        }

                        if (!IsInsert)
                            break;
                    }

                    // 새로운 행을 추가한다.
                    if (IsInsert)
                    {
                        if (sPivotCols.Length == 1)
                        {
                            for (int i = 0; i < sPivotRows.Length; i++)
                            {
                                DataRow drRow = dt.NewRow();

                                foreach (string sTargetRow in sTargetRows)
                                {
                                    drRow[sTargetRow] = dr[sTargetRow];
                                }

                                drRow[sPivotRowsName] = sPivotRows[i];
                                drRow[dr[sPivotCols[0]].ToString()] = dr[sPivotRows[i]];
                                dt.Rows.Add(drRow);
                            }
                        }
                        else
                        {
                            DataRow drRow = dt.NewRow();

                            foreach (string sTargetRow in sTargetRows)
                            {
                                drRow[sTargetRow] = dr[sTargetRow];
                            }

                            for (int i = 0; i < sPivotCols.Length; i++)
                            {
                                drRow[dr[sPivotCols[i]].ToString()] = dr[sPivotRows[i]];
                            }
                            dt.Rows.Add(drRow);
                        }
                    }
                }

                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable DataPivot(DataRow[] sourceDr, string[] sDefaultCols, int iMaxCols, string sPivotCol, string sDataCol)
        {
            DataTable dt = null;
            try
            {
                if (sourceDr == null || sourceDr.Length < 1)
                    return null;

                dt = new DataTable();

                // sTargetRows 컬럼 생성
                foreach (string sTargetRow in sDefaultCols)
                {
                    string columnName = sourceDr[0].Table.Columns[sTargetRow].ColumnName;
                    Type type = sourceDr[0].Table.Columns[sTargetRow].DataType;
                    dt.Columns.Add(columnName, type);
                }


                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FitColumnSize2(SheetView sv)
        {
            for (int a = 0; a < sv.ColumnCount; a++)
                sv.Columns[a].Width = sv.GetPreferredColumnWidth(a, true, false) + 15;
        }

        public static void FitColumnSize(SheetView sv)
        {
            for (int a = 0; a < sv.ColumnCount; a++)
                sv.Columns[a].Width = sv.Columns[a].GetPreferredWidth();
        }

        #endregion

        #region ReCalcurateColumnWidth
        /// <summary>
        /// Recalculate column width of Spread Sheet
        /// </summary>
        /// <param name="sheetList"></param>
        public static void ReCalcurateColumnWidth(params SheetView[] sheetList)
        {
            try
            {
                foreach (SheetView sv in sheetList)
                {
                    for (int i = 0; i < sv.ColumnHeader.RowCount; i++)
                        sv.ColumnHeader.Rows[i].Renderer = SpreadColumnHeaderRendererProvider.Renderer;

                    sv.DataAutoSizeColumns = false;
                    sv.DataAutoCellTypes = false;

                    for (int i = 0; i < sv.Columns.Count; i++)
                    {
                        sv.Columns[i].Width = sv.GetPreferredColumnWidth(i, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// SpreadColumnHeaderRendererProvider
        /// </summary>
        public static class SpreadColumnHeaderRendererProvider
        {
            private static ColumnHeaderRenderer renderer = new ColumnHeaderRenderer();

            public static ColumnHeaderRenderer Renderer
            {
                get { return SpreadColumnHeaderRendererProvider.renderer; }
                set { SpreadColumnHeaderRendererProvider.renderer = value; }
            }

            static SpreadColumnHeaderRendererProvider()
            {
                renderer.WordWrap = false;
            }
        }

        #region SaveAsExcel
        /// <summary>
        /// Save as to Excel
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="strProgName"></param>
        public void SaveAsExcel(FarPoint.Win.Spread.FpSpread fpSpread, string strProgName)
        {
            System.Windows.Forms.SaveFileDialog oSaveFileDlg = new System.Windows.Forms.SaveFileDialog();
            oSaveFileDlg.Title = "Save Excel File SaveFileDialog";
            oSaveFileDlg.OverwritePrompt = true;
            oSaveFileDlg.RestoreDirectory = true;

            oSaveFileDlg.FileName = strProgName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";

            if (System.Windows.Forms.DialogResult.OK == oSaveFileDlg.ShowDialog())
                fpSpread.SaveExcel(oSaveFileDlg.FileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        }
        #endregion
        #region InitSpread
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ver">Vertical</param>
        /// <param name="hor">Horiaontal</param>
        /// <param name="Edit">Edit Mode</param>
        /// <param name="spreadList"></param>
        public static void InitSpread(CellVerticalAlignment ver, CellHorizontalAlignment hor,bool Edit
             ,params FpSpread[] spreadList)
        {
            try
            {

                foreach (FpSpread fp in spreadList)
                {
                    if(!Edit)
                        fp.ActiveSheet.OperationMode = OperationMode.ReadOnly;
                    FarPoint.Win.Spread.DefaultSkins.Classic.Apply(fp);
                    FarPoint.Win.Spread.Cell cellrange;
                    if (fp.ActiveSheet.RowCount > 0)
                    {
                        cellrange = fp.ActiveSheet.Cells[0, 0, fp.ActiveSheet.RowCount - 1, fp.ActiveSheet.ColumnCount - 1];
                        cellrange.VerticalAlignment = ver;
                        cellrange.HorizontalAlignment = hor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion
        #region Clear Control
        public static void ResetItem(Control control)
        {
            foreach (Control ctl in control.Controls)
            {
                if (ctl.Controls.Count > 0)
                    return;
                else
                {
                    if (ctl is TextBox)
                        ctl.Text = string.Empty;
                    else if (ctl is CheckBox)
                    {
                        (ctl as CheckBox).Checked = false;
                    }
                    else if (ctl is ComboBox)
                    {
                        //(ctl as ComboBox).Items.Clear();
                        (ctl as ComboBox).Text = string.Empty;
                    }
                    else if (ctl is ListView)
                        (ctl as ListView).Items[0].Selected = true;
                    else if (ctl is RadioButton)
                        (ctl as RadioButton).Checked = false;
                    
                }
            }
        }
        #endregion
        #region SendMail
        static private void sendmail(string strFileName, DateTime dt, string strErrMessage)
        {
           // DACrux.Framework.RO.UserGroup oUserGroup = null;
            string strMailServer = "smtp.hanamicron.co.kr";
            int iMailPort = 25;
            DataTable dtSecUsr = new DataTable();
            string strMailSenderID = "HanaMailMaster@hmicron.com";
            string strMailSenderPW = "p@ssw0rd";
            string strMailSenderName = "Developer";
            string strMailSubject = "Engineering UI에서 Error가 보고 되었습니다";
            string strMailContents = string.Empty;
            StringBuilder strMailBody;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName()); 

            //string strMailFlag = "N";

            //string[] strEmailList = null;
            try
            {
                //oUserGroup = new DACrux.Framework.RO.UserGroup();
                //dtSecUsr = oUserGroup.LoadSecurityUser().Tables[0];
                //dtSecUsr = oUserGroup.SearchGroupUser("ADMIN").Tables[0];
                strMailBody = new StringBuilder();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strMailServer);
                mail.From = new MailAddress(strMailSenderID, strMailSenderName, System.Text.Encoding.UTF8);
                mail.To.Add("louis0723.lim@miracom.co.kr");
                mail.Subject = strMailSubject;
                strMailBody = strMailBody.AppendFormat("Event Time      : {0} \n",dt.ToString());
                strMailBody = strMailBody.AppendFormat("Computer Name   : {0} \n", Environment.MachineName);
                strMailBody = strMailBody.AppendFormat("Computer IP     : {0} \n",ipEntry.AddressList[4].ToString());
                strMailBody = strMailBody.AppendFormat("Windows Version : {0} \n",Environment.OSVersion);
                strMailBody = strMailBody.AppendFormat("Program Name    : {0} \n",Application.ProductName);
                //strMailBody = strMailBody.AppendFormat("Server Version  : 2011072909\n");
                //strMailBody = strMailBody.AppendFormat("Client Version  : 2011072909\n");
                strMailBody = strMailBody.AppendFormat("User ID         :  {0}\n", DACrux.Base.GlobalVariable.UserID);
                //strMailBody = strMailBody.AppendFormat("User Name       : {0} \n", DACrux.Base.Regedit.Server);
                //strMailBody = strMailBody.AppendFormat("User Group      : QA_GERNERAL\n");
                strMailBody = strMailBody.AppendFormat("Server Name     : DAServer\n");
                //strMailBody = strMailBody.AppendFormat("Site ID         : HMMS\n");
                strMailBody = strMailBody.AppendFormat("Server Address  : {0}\n", DACrux.Base.GlobalVariable.ServerIP);
                //strMailBody = strMailBody.AppendFormat("OI Name         : HMC0570\n");
                strMailBody = strMailBody.AppendFormat("Form Name         : {0} \n",Application.OpenForms[0].Name);
                strMailBody = strMailBody.AppendFormat("Error Message     : {0} \n", strErrMessage);
                strMailBody = strMailBody.AppendFormat("ClipBoard Data  : \n");
                mail.Body = strMailBody.ToString();
                System.Net.Mail.Attachment a;
                a = new Attachment(strFileName);
                mail.Attachments.Add(a);
                SmtpServer.Port = iMailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(strMailSenderID, strMailSenderPW);

                //SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        static public void screenCapture(int intBitWidth, int intBitHeight, Point ptSource, DateTime dt, string strErrMessage)
        {
            try
            {
                Bitmap bitmap = new System.Drawing.Bitmap(intBitWidth, intBitHeight);
                Graphics g = Graphics.FromImage(bitmap);
                string strFileName = string.Empty;
                strFileName = Application.StartupPath+"\\" + "Error_"+dt.ToString("yyyyMMddHHmmss")+".PNG";
                //g.CopyFromScreen(intBitWidth, intBitHeight, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceAnd);
                g.CopyFromScreen(ptSource, new Point(0, 0), new Size(intBitWidth, intBitHeight));
                bitmap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png);
                //picInfo.Image=bitmap;
                //picInfo.SizeMode = PictureBoxSizeMode.StretchImage;
                // picCapImage.
                sendmail(strFileName, dt, strErrMessage);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }
        #endregion

    }
     
}
