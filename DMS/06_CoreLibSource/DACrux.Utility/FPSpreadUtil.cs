using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace DACrux.Utility
{
    /// <summary>
    /// <br> FPSpread 공통 함수</br>
    /// - 작  성  자 : 임영신
    /// - 최초작성일 : 2006년1월 
    /// - 최종수정자 : 
    /// - 최종수정일 : 
    /// - 주요변경로그
    /// </summary>
    #region 그리드에 라이닝을 하기위한 펑크션 클래스
    /*==========================================================================================
	*  그리드에 라이닝을 하기위한 펑크션 클래스
	==========================================================================================*/
    //UnderLine Bolder Class Util
    public class GridUnderLineBorder : FarPoint.Win.IBorder
    {
        int underlineCount;
        private FarPoint.Win.Inset m_Inset;

        public FarPoint.Win.Inset Inset
        {
            get
            {
                return m_Inset;
            }
        }

        public GridUnderLineBorder()
        {

            FarPoint.Win.Inset baseinset = new FarPoint.Win.Inset(0, 0, 0, 1);

            this.underlineCount = 1;
            m_Inset = new FarPoint.Win.Inset(baseinset.Left, baseinset.Top, baseinset.Right, Math.Max(baseinset.Bottom, 2 * underlineCount - 1));

        }
        void FarPoint.Win.IBorder.Paint(System.Drawing.Graphics g, int x, int y, int width, int height)
        {
            if (width > 0 & height > 0)
            {
                int underlineheight = Math.Min(2 * underlineCount - 1, height);
                int underlinewidth = width;
                int underlinex = x + ((width - underlinewidth) / 2);
                int underliney = y + height - underlineheight;
                while (underlineheight > 0)
                {
                    g.FillRectangle(System.Drawing.Brushes.Black, underlinex, y, underlinewidth, underlineheight);
                    underlineheight -= 2;
                }
            }
        }
    }
    //GridVerticalLineBolder Class Util
    public class GridVerticalLineBolder : FarPoint.Win.IBorder
    {
        int underlineCount;
        private FarPoint.Win.Inset m_Inset;

        public FarPoint.Win.Inset Inset
        {
            get
            {
                return m_Inset;
            }
        }

        public GridVerticalLineBolder()
        {
            FarPoint.Win.Inset baseinset = new FarPoint.Win.Inset(0, 0, 1, 0);

            this.underlineCount = 1;
            m_Inset = new FarPoint.Win.Inset(baseinset.Left, baseinset.Top, Math.Max(baseinset.Right, 2 * underlineCount - 1), baseinset.Bottom);
        }
        void FarPoint.Win.IBorder.Paint(System.Drawing.Graphics g, int x, int y, int width, int height)
        {
            if (width > 0 & height > 0)
            {
                int underlineheight = height;
                int underlinewidth = Math.Min(2 * underlineCount - 1, width);
                int underlinex = x + width - underlinewidth;
                int underliney = y + ((height - underlineheight) / 2);
                while (underlineheight > 0)
                {
                    g.FillRectangle(System.Drawing.Brushes.Black, x, underliney, underlinewidth, underlineheight);
                    underlineheight -= 2;
                }
            }
        }
    }
    public class GridXYLineBolder : FarPoint.Win.IBorder
    {
        private FarPoint.Win.Inset m_Inset;

        public GridXYLineBolder()
        {
            m_Inset = new FarPoint.Win.Inset(0, 0, 3, 0);
        }

        public FarPoint.Win.Inset Inset
        {
            get
            {
                return m_Inset;
            }
        }
        void FarPoint.Win.IBorder.Paint(System.Drawing.Graphics g, int x, int y, int width, int height)
        {
            if (width > 0 & height > 0)
            {
                int boderwidth = System.Math.Min(3, width);
                int borderheight = System.Math.Min(3, height);
                System.Drawing.Brush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(x, y, width, height), System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
                System.Drawing.Brush brushTmp = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(x, y, width, height), System.Drawing.Color.White, System.Drawing.Color.Silver, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                //System.Drawing.Brush brushTmp = new System.Drawing.Drawing2D.
                g.FillRectangle(brushTmp, x + width - 1, y, 3, height);
                g.FillRectangle(brush, x, y, boderwidth - 2, height);
                g.FillRectangle(brush, x, y, width, borderheight - 2);
                brush.Dispose();
            }
        }
    }
    #endregion

    public static class FPSpreadUtil
    {
        public static void FitColumnSize(ref FarPoint.Win.Spread.SheetView sv)
        {
            for (int a = 0; a < sv.ColumnCount; a++)
                sv.Columns[a].Width = sv.Columns[a].GetPreferredWidth();
        }

        //그리드 클리어
        public static void ClearAll(FarPoint.Win.Spread.SheetView oShv)
        {
            try
            {
                oShv.RowCount = 0;
                oShv.ColumnCount = 0;
            }
            catch (Exception ex)
            { throw ex; }
        }
        //그리드 헤더 텍스트 설정
        public static void SetHeaderText(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes, string[] HeaderTexts)
        {
            try
            {
                for (int i = 0; i < ColIndexes.Length; i++)
                {
                    oShv.ColumnHeader.Columns[ColIndexes[i]].Label = HeaderTexts[i];
                }
            }
            catch (Exception ex)
            { throw ex; }

        }
        //그리드 컬럼폭 설정
        public static void SetColWidth(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes, int[] ColSizes)
        {
            try
            {

                for (int i = 0; i < ColIndexes.Length; i++)
                {
                    oShv.Columns[ColIndexes[i]].Width = ColSizes[i];
                }
            }
            catch (Exception ex)
            { throw ex; }

        }
        public static void SetRowScrollDisable(FarPoint.Win.Spread.SheetView oShv, int[] RowIndexes)
        {
            try
            {
                for (int i = 0; i < RowIndexes.Length; i++)
                {
                    oShv.Rows[RowIndexes[i]].Resizable = false;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        //그리드 사이즈 고정 컬럼 설정
        public static void SetColSizeFix(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes)
        {
            try
            {
                for (int i = 0; i < ColIndexes.Length; i++)
                {
                    oShv.Columns[ColIndexes[i]].Resizable = false;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        //Column 텍스트 VerticalAlign 설정
        public static void SetColVerticalAlign(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes, FarPoint.Win.Spread.CellVerticalAlignment[] enVerAlign)
        {
            FarPoint.Win.Spread.Column oCol;
            for (int i = 0; i < ColIndexes.Length; i++)
            {
                oCol = oShv.Columns[ColIndexes[i]];
                oCol.VerticalAlignment = enVerAlign[i];
            }
        }
        public static void SetColVerticalAlign(FarPoint.Win.Spread.SheetView oShv, FarPoint.Win.Spread.CellVerticalAlignment enVerAlign)
        {
            FarPoint.Win.Spread.Column oCol;
            for (int i = 0; i < oShv.Columns.Count; i++)
            {
                oCol = oShv.Columns[i];
                oCol.VerticalAlignment = enVerAlign;
            }
        }
        //Column 텍스트 HorizontalAlign 설정
        public static void SetColHorizontalcalAlign(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes, FarPoint.Win.Spread.CellHorizontalAlignment[] enHoriAlign)
        {
            FarPoint.Win.Spread.Column oCol;
            for (int i = 0; i < ColIndexes.Length; i++)
            {
                oCol = oShv.Columns[ColIndexes[i]];
                oCol.HorizontalAlignment = enHoriAlign[i];
            }
        }

        //Column 텍스트 Default Align(Center) 설정
        public static void SetColDefaultAlign(FarPoint.Win.Spread.SheetView oShv)
        {
            for (int i = 0; i < oShv.Columns.Count; i++)
            {
                oShv.Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                oShv.Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
            }
        }


        #region 데이타 바인딩 컬렉션
        //정상 데이타 바인딩
        /*
        public static void DataBind(FarPoint.Win.Spread.FpSpread oSp,DataSet oDs)
        {
            try
            {
                oSp.DataSource=oDs;
            }
            catch(Exception ex)
            {throw ex;}
			
        }
        */
        //메뉴얼 데이타 바인딩,특정위치에서 로우를 삽입한다.컬럼사이즈와 컬럼시작부분의 데이타수가 안맞을경우
        //에러가 발생할것이다.
        public static void DataBind(FarPoint.Win.Spread.SheetView oShv, DataSet oDs, int RowIndex, int ColIndex)
        {
            if (oDs.Tables[0].Rows.Count == 0) { return; }
            DataTable oDT;
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                oDT = oDs.Tables[0];
                AddRow(oShv, RowIndex, oDT.Rows.Count);
                oCellRange = oShv.Cells[RowIndex, ColIndex, oDT.Rows.Count - 1, oDT.Columns.Count - 1];
                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    for (int j = 0; j < oDT.Columns.Count; j++)
                    {
                        SetCellValue(oShv, RowIndex + i, ColIndex + j, oDT.Rows[i][j].ToString());
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void DataBind(FarPoint.Win.Spread.SheetView oShv, DataTable oDt, int RowIndex, int ColIndex)
        {
            if (oDt.Rows.Count == 0) { return; }
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                AddRow(oShv, RowIndex, oDt.Rows.Count);
                oCellRange = oShv.Cells[RowIndex, ColIndex, oDt.Rows.Count - 1, oDt.Columns.Count - 1];
                for (int i = 0; i < oDt.Rows.Count; i++)
                {
                    for (int j = 0; j < oDt.Columns.Count; j++)
                    {
                        SetCellValue(oShv, RowIndex + i, ColIndex + j, oDt.Rows[i][j].ToString());
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void DataBind(FarPoint.Win.Spread.SheetView oShv, DataTable oDt, int RowIndex, int[] arrColIdx, string[] arrColName)
        {
            if (oDt.Rows.Count == 0) { return; }
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                AddRow(oShv, RowIndex, oDt.Rows.Count);
                oCellRange = oShv.Cells[RowIndex, arrColIdx[0], oDt.Rows.Count - 1, arrColIdx.Length - 1];
                for (int i = 0; i < oDt.Rows.Count; i++)
                {
                    for (int j = 0; j < arrColIdx.Length; j++)
                    {
                        SetCellValue(oShv, RowIndex + i, arrColIdx[j], oDt.Rows[i][arrColName[j]]);
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 컬럼 로우 추가,제거
        /*==========================================================================================
		* 
		* 
		* 
		* 
		==========================================================================================*/
        public static void AddImageColumn(FarPoint.Win.Spread.SheetView oShv, int ColIndex, System.Windows.Forms.ImageListStreamer oImgStream)
        {
            AddColumn(oShv, ColIndex);

            FarPoint.Win.Spread.Column oCol;
            FarPoint.Win.Spread.CellType.GeneralCellType oCell = new FarPoint.Win.Spread.CellType.GeneralCellType();
            oCell.BackgroundImage = new FarPoint.Win.Picture(oImgStream);

            oCol = oShv.Columns[ColIndex];
            oCol.CellType = oCell;
        }
        public static void AddCheckBoxColumn(FarPoint.Win.Spread.SheetView oShv, int ColumnPositionIndex)
        {
            AddColumn(oShv, ColumnPositionIndex);

            FarPoint.Win.Spread.Column oCol;
            FarPoint.Win.Spread.CellType.CheckBoxCellType oCell = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            oCol = oShv.Columns[ColumnPositionIndex];
            oCol.CellType = oCell;
        }
        //콤보박스 셀타입 설정 #1,콤보 텍스트만을 설정하며 키와 텍스트가 동일
        public static void AddComboColumn(FarPoint.Win.Spread.SheetView oShv, int ColumnPositionIndex, string[] cboTexts)
        {
            AddColumn(oShv, ColumnPositionIndex);
            FarPoint.Win.Spread.Column oCol;
            FarPoint.Win.Spread.CellType.ComboBoxCellType oCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            oCell.Items = cboTexts;
            oCol = oShv.Columns[ColumnPositionIndex];
            oCol.CellType = oCell;
        }
        //콤보박스 셀타입 설정 #2,콤보 텍스트와 키를 설정(키와 텍스트가 다름)
        public static void AddComboColumn(FarPoint.Win.Spread.SheetView oShv, int ColumnPositionIndex, string[] cboTexts, string[] cboKeys)
        {
            AddColumn(oShv, ColumnPositionIndex);
            FarPoint.Win.Spread.Column oCol;
            FarPoint.Win.Spread.CellType.ComboBoxCellType oCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            oCell.Items = cboTexts;

            oCol = oShv.Columns[ColumnPositionIndex];
            oCol.CellType = oCell;

        }
        public static void AddButtonColumn(FarPoint.Win.Spread.SheetView oShv, int ColumnPositionIndex, string cellText)
        {
            AddColumn(oShv, ColumnPositionIndex);
            FarPoint.Win.Spread.Column oCol;
            FarPoint.Win.Spread.CellType.ButtonCellType oCell = new FarPoint.Win.Spread.CellType.ButtonCellType();
            oCell.Text = cellText;
            oCol = oShv.Columns[ColumnPositionIndex];
            oCol.CellType = oCell;
        }
        public static void AddColumn(FarPoint.Win.Spread.SheetView oShv, int ColumnPositionIndex)
        {
            //FarPoint.Win.Spread.Column oCol;
            oShv.AddColumns(ColumnPositionIndex, 1);
        }
        //시트 마지막에 로우 추가
        public static void AddRow(FarPoint.Win.Spread.SheetView oShv)
        {
            oShv.AddRows(oShv.RowCount, 1);
        }
        //지정된 위치에 로우 추가
        public static void AddRow(FarPoint.Win.Spread.SheetView oShv, int RowIndex)
        {
            oShv.AddRows(RowIndex, 1);
        }
        //지정된 위치에 로우 n개 추가
        public static void AddRow(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int RowCount)
        {
            oShv.AddRows(RowIndex, RowCount);
        }
        //액티브 로우 제거
        public static void RemoveRow(FarPoint.Win.Spread.SheetView oShv)
        {
            oShv.RemoveRows(oShv.ActiveRowIndex, 1);
        }
        //지정된 위치인덱스의 로우 제거
        public static void RemoveRow(FarPoint.Win.Spread.SheetView oShv, int RowIndex)
        {
            oShv.RemoveRows(RowIndex, 1);
        }
        #endregion

        #region 컬럼타입지정
        public static void SetColumnTypeNumeric(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int DecimalPoint)
        {
            FarPoint.Win.Spread.CellType.NumberCellType nmbrcell = new FarPoint.Win.Spread.CellType.NumberCellType();
            try
            {
                nmbrcell.DecimalSeparator = ",";
                nmbrcell.DecimalPlaces = DecimalPoint;
                nmbrcell.LeadingZero = FarPoint.Win.Spread.CellType.LeadingZero.UseRegional;
                oShv.Columns[ColIndex].CellType = nmbrcell;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetCellTypeNumeric(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int RowIndex, int DecimalPoint)
        {
            FarPoint.Win.Spread.CellType.NumberCellType nmbrcell = null;
            string strCellText = string.Empty;
            double dCellValue = 0.0;
            bool bRet = false;
            try
            {
                strCellText = GetCellText(oShv, RowIndex, ColIndex);
                try
                {
                    dCellValue = Convert.ToDouble(strCellText);
                    bRet = true;
                }
                catch { bRet = false; }
                if (bRet)
                {
                    nmbrcell = new FarPoint.Win.Spread.CellType.NumberCellType();
                    nmbrcell.Separator = ",";
                    nmbrcell.DecimalSeparator = ".";
                    nmbrcell.DecimalPlaces = DecimalPoint;
                    nmbrcell.LeadingZero = FarPoint.Win.Spread.CellType.LeadingZero.UseRegional;
                    oShv.Cells[RowIndex, ColIndex].CellType = nmbrcell;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetCellTypeText(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int RowIndex)
        {
            FarPoint.Win.Spread.CellType.GeneralCellType oCell = new FarPoint.Win.Spread.CellType.GeneralCellType();
            try
            {
                oShv.Cells[RowIndex, ColIndex].CellType = oCell;

            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetCellTypeDate(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int RowIndex)
        {
            FarPoint.Win.Spread.CellType.DateTimeCellType datecell = new FarPoint.Win.Spread.CellType.DateTimeCellType();

            try
            {
                datecell.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined;
                datecell.UserDefinedFormat = "YY/MM/DD HH:MM:SS";
                oShv.Cells[RowIndex, ColIndex].CellType = datecell;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 자동 소팅 설정
        public static void SetAutoSortColumn(
            FarPoint.Win.Spread.SheetView oShv,
            int[] columnIndexArr
            )
        {
            FarPoint.Win.Spread.Column oCol;
            try
            {
                for (int i = 0; i < columnIndexArr.Length; i++)
                {
                    oCol = oShv.Columns[columnIndexArr[i]];
                    oCol.ShowSortIndicator = true;
                    oCol.AllowAutoSort = true;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region Visible,UnVisible 설정
        public static void SetColumnVisible(FarPoint.Win.Spread.SheetView oShv, int[] ColIndexes, bool[] Visible)
        {
            try
            {
                for (int i = 0; i < ColIndexes.Length; i++)
                {
                    oShv.Columns[ColIndexes[i]].Visible = Visible[i];
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetRowVisible(FarPoint.Win.Spread.SheetView oShv, int[] RowIndexes, bool Visible)
        {
            try
            {
                for (int i = 0; i < RowIndexes.Length; i++)
                {
                    oShv.Rows[RowIndexes[i]].Visible = Visible;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region Get,Set Cell Value,Get,Set Clip Value
        /*==========================================================================================
		* 
		* 
		* 
		* 
		==========================================================================================*/
        public static string GetCellText(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex)
        {
            string CellText = String.Empty;
            try
            {
                CellText = oShv.GetText(RowIndex, ColIndex);
            }
            catch (Exception ex)
            { throw ex; }
            return CellText;
        }
        public static object GetCellValue(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex)
        {
            object CellValue;
            try
            {
                CellValue = oShv.GetValue(RowIndex, ColIndex);
            }
            catch (Exception ex)
            { throw ex; }

            return CellValue;
        }
        public static object[,] GetCellArray(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, int RowCount, int ColCount)
        {
            object[,] CellClipArray;
            try
            {
                CellClipArray = oShv.GetArray(RowIndex, ColIndex, RowCount, ColCount);
            }
            catch (Exception ex)
            { throw ex; }
            return CellClipArray;
        }
        public static void SetCellImage(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, System.Drawing.Image oImage)
        {
            FarPoint.Win.Spread.CellType.GeneralCellType oCell = new FarPoint.Win.Spread.CellType.GeneralCellType();
            oCell.BackgroundImage = new FarPoint.Win.Picture(oImage);
            oShv.Cells[RowIndex, ColIndex].CellType = oCell;
        }
        public static void SetCellText(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, string strCellData)
        {
            try
            {
                oShv.SetText(RowIndex, ColIndex, strCellData);
            }
            catch (Exception ex)
            { throw ex; }

        }
        public static void SetCellText(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int[] ColIndexs, string[] strCellDatas)
        {
            try
            {
                for (int i = 0; i < ColIndexs.Length; i++)
                {
                    oShv.SetText(RowIndex, ColIndexs[i], strCellDatas[i]);
                }
            }
            catch (Exception ex)
            { throw ex; }

        }
        public static void SetCellValue(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, object objCellData)
        {
            try
            {
                oShv.SetValue(RowIndex, ColIndex, objCellData);
            }
            catch (Exception ex)
            { throw ex; }

        }

        public static void SetCellArray(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, object[,] CellClipArray)
        {
            try
            {
                oShv.SetArray(RowIndex, ColIndex, CellClipArray);
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region SetCellStyle [BackGroundColor,ForeColor,Bold]
        /*==========================================================================================
		* 
		* 
		* 
		* 
		==========================================================================================*/
        public static void SetCellFontBold(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int RowIndex)
        {
            FarPoint.Win.Spread.Cell oCell;
            try
            {
                oCell = oShv.Cells[RowIndex, ColIndex];
            }
            catch (Exception ex)
            { throw ex; }

        }
        public static void SetCellFontBold(FarPoint.Win.Spread.SheetView oShv, int ColIndexBOF, int RowIndexBOF, int ColIndexEOF, int RowIndexEOF)
        {
            FarPoint.Win.Spread.Cell oCell;
            try
            {
                for (int i = RowIndexBOF; i < RowIndexEOF; i++)
                {
                    for (int j = ColIndexBOF; j < ColIndexEOF; j++)
                    {
                        oCell = oShv.Cells[i, j];
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetRowBackGroundColor(FarPoint.Win.Spread.SheetView oShv, int RowIndex, Color COLOR)
        {
            FarPoint.Win.Spread.Cell oCellRange;
            int ColCnt = oShv.Columns.Count;
            try
            {
                for (int i = 0; i < oShv.Columns.Count; i++)
                {
                    oCellRange = oShv.Cells[RowIndex, i];
                    oCellRange.BackColor = COLOR;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetCellBackGroundColor(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, Color COLOR)
        {
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                oCellRange = oShv.Cells[RowIndex, ColIndex];
                oCellRange.BackColor = COLOR;
            }
            catch (Exception ex)
            { throw ex; }

        }
        public static void SetCellBackGroundColor(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, int RowCount, int ColCount, Color COLOR)
        {
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                oCellRange = oShv.Cells[RowIndex, ColIndex, RowCount, ColCount];
                oCellRange.BackColor = COLOR;
            }
            catch (Exception ex)
            { throw ex; }


        }
        public static void SetCellFontColor(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex, Color COLOR)
        {
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                oCellRange = oShv.Cells[RowIndex, ColIndex];
                oCellRange.ForeColor = COLOR;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetCellFontColor(FarPoint.Win.Spread.SheetView oShv, int ColIndex, int RowIndex, int ColCount, int RowCount, Color COLOR)
        {
            FarPoint.Win.Spread.Cell oCellRange;
            try
            {
                oCellRange = oShv.Cells[RowIndex, ColIndex, RowCount, ColCount];
                oCellRange.ForeColor = COLOR;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetRowBolder(FarPoint.Win.Spread.SheetView oShv, int RowIndex)
        {
            FarPoint.Win.Spread.Row oRow;
            try
            {
                oRow = oShv.Rows[RowIndex];
                oRow.Border = new GridUnderLineBorder();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetColBolder(FarPoint.Win.Spread.SheetView oShv, int ColIndex)
        {
            FarPoint.Win.Spread.Column oColumn;
            try
            {
                oColumn = oShv.Columns[ColIndex];
                oColumn.Border = new GridVerticalLineBolder();
            }
            catch (Exception ex)
            { throw ex; }
        }
        public static void SetRowColBolder(FarPoint.Win.Spread.SheetView oShv, int RowIndex, int ColIndex)
        {
            FarPoint.Win.Spread.Row oRow;
            FarPoint.Win.Spread.Column oColumn;
            try
            {
                oRow = oShv.Rows[RowIndex];
                oRow.Border = new GridUnderLineBorder();
                oColumn = oShv.Columns[ColIndex];
                oColumn.Border = new GridVerticalLineBolder();
                oShv.Cells[RowIndex, ColIndex].Border = new GridXYLineBolder();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region Sheet 조작
        public static FarPoint.Win.Spread.SheetView AddSheet(
            FarPoint.Win.Spread.FpSpread oSp,
            string SheetName
            )
        {
            FarPoint.Win.Spread.SheetView oShv = new FarPoint.Win.Spread.SheetView();
            oShv.SheetName = SheetName;
            oSp.Sheets.Add(oShv);
            return oShv;
        }

        public static void RemoveSheet(
            FarPoint.Win.
            Spread.FpSpread oSp
            )
        {
            FarPoint.Win.Spread.SheetView oShv = oSp.ActiveSheet;
            oSp.Sheets.Remove(oShv);
        }

        #endregion

        #region column 이동
        public static void MoveColumn(FarPoint.Win.Spread.SheetView sv, string label, int toIdx)
        {
            int fromIdx = -1;
            for (int i = 0; i < sv.ColumnCount; i++)
            {
                if (sv.Columns[i].Label.Equals(label))
                {
                    fromIdx = i;
                    break;
                }
            }
            if (fromIdx == -1) return;

            object[] val = new object[sv.RowCount];

            for (int i = 0; i < sv.RowCount; i++)
            {
                val[i] = sv.Cells[i, fromIdx].Value;
            }

            sv.Columns.Remove(fromIdx, 1);

            sv.Columns.Add(toIdx, 1);
            sv.Columns[toIdx].Label = label;
            for (int i = 0; i < val.Length; i++)
            {
                sv.Cells[i, toIdx].Value = val[i];
            }
        }
        #endregion

        public static object[] GetRowToObject(
            FarPoint.Win.Spread.SheetView sv,
            int rowNo
            )
        {
            object[] obj = new object[sv.ColumnCount];
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i] = sv.Cells[rowNo, i].Value;
            }
            return obj;
        }

        public static int GetColumnNo(
            FarPoint.Win.Spread.SheetView sv,
            string colName
            )
        {
            for (int a = 0; a < sv.Columns.Count; a++)
            {
                if (sv.Columns[a].Label.Equals(colName))
                {
                    return a;
                }
            }
            return -1;
        }

        // 2014.03.27 '추가' : 주영진
        #region Header Sorting 버튼 추가
        public static void SetSpreadHeadSorting(int colCnt, FarPoint.Win.Spread.SheetView sv)
        {
            try
            {
                if (colCnt == 0)
                    return;
                for (int i = 0; i < sv.ColumnCount; i++)
                {
                    sv.SetColumnAllowAutoSort(i, true);
                    sv.GetColumnAllowAutoSort(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // 2014.04.01 '추가' : 주영진
        #region column 넓이 자동 조정
        //public static void SpreadColumnFitSizeDataOrHeader(FarPoint.Win.Spread.SheetView sv)
        //{
        //    for (int i = 0; i < sv.ColumnCount; i++)
        //    {
        //        sv.Columns[i].Width = sv.Columns[i].Label.Length * 10 > sv.Columns[i].GetPreferredWidth() 
        //            ? sv.Columns[i].Label.Length * 10 : sv.Columns[i].GetPreferredWidth();
        //    }
        //    for (int i = 0; i < sv.RowCount; i++)
        //    {
        //        sv.Rows[i].Height = sv.Rows[i].GetPreferredHeight();
        //    }
        //    for (int i = 0; i < sv.ColumnHeaderRowCount; i++)
        //    {
        //        sv.ColumnHeader.Rows[i].Height = sv.ColumnHeader.Rows[i].GetPreferredHeight();
        //    }
        //    for (int i = 0; i < sv.RowHeaderColumnCount; i++)
        //    {
        //        sv.RowHeader.Columns[i].Width = sv.RowHeader.Columns[i].GetPreferredWidth();
        //    }
        //}

        #endregion

        // 2014.04.05 '추가' : 주영진
        #region ' 정수형으로 Cell Type 변경 '
        public static void SetCellTypeToIntByDataTable(ref FarPoint.Win.Spread.SheetView fpSheet, DataTable dt)
        {
            FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
            numberCell.DecimalPlaces = 0;

            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    try
            //    {
            //        long num = DACrux.Base.Convert.longParse(dt.Rows[0][i].ToString());
            //        fpSheet.Columns[i].CellType = numberCell;
            //    }
            //    catch
            //    {
            //    }
            //}

            long num = long.MinValue;
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == typeof(System.String))
                    continue;
                else if (col.DataType == typeof(System.DateTime))
                    continue;

                fpSheet.Columns[col.Ordinal].CellType = numberCell;
            }
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
        public static void InitSpread(
            CellVerticalAlignment ver,
            CellHorizontalAlignment hor,
            bool Edit,
            params FpSpread[] spreadList
            )
        {
            try
            {

                foreach (FpSpread fp in spreadList)
                {
                    if (!Edit)
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

        public static void InitSpread(
            params FpSpread[] spreadList
            )
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

                        //DataAutoCellTypes 을 False 로 놓을 경우 Excel Save 시 Header 내용이 없어 진다. 
                        sv.DataAutoCellTypes = true;

                        sv.ColumnCount = 0;
                        sv.RowCount = 0;

                        sv.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                        sv.OperationMode = OperationMode.SingleSelect;

                        //FarPoint.Win.Spread.DefaultSkins.Default.Apply(sv);
                        //FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(sv);
                    }
                    fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                    fp.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;

                    //FarPoint.Win.Spread.DefaultSkins.Default.Apply(fp);
                    //FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(fp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void VisibleSpreadColumns(
            SheetView sheet,
            int[] Columns,
            bool isView
            )
        {
            try
            {
                for (int i = 0; i < sheet.ColumnCount; i++)
                {
                    sheet.Columns[i].Visible = !isView;
                }

                DataTable dt = sheet.GetDataView(false).Table;

                for (int i = 0; i < Columns.Length; i++)
                {
                    sheet.Columns[Columns[i]].Visible = isView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void VisibleSpreadColumns(
            SheetView sheet,
            string[] Columns,
            bool IsView
            )
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

        public static void AdjustSpread(
            SheetView sheet
            )
        {
            AdjustSpread(sheet, 100, 0, true);
        }

        public static void AdjustSpread(
            SheetView sheet,
            int minColumnWidth,
            int defultPlaces,
            bool bShowSeparator
            )
        {
            TextCellType text = null;
            NumberCellType num = null;
            DateTimeCellType date = null;
            DataTable dt = null;
            try
            {
                dt = (sheet.GetDataView(false)).Table;
                foreach (DataColumn col in dt.Columns)
                {
                    //sheet.Columns[col.Ordinal].Width = (sheet.GetPreferredColumnWidth(col.Ordinal, true, false) > minColumnWidth ? (sheet.GetPreferredColumnWidth(col.Ordinal, true, true) + 15) : minColumnWidth);
                    if (col.DataType == typeof(System.String))
                    {
                        text = new TextCellType();
                        sheet.Columns[col.Ordinal].CellType = text;
                        sheet.Columns[col.Ordinal].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        sheet.Columns[col.Ordinal].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                    else if (col.DataType == typeof(System.Decimal))
                    {
                        num = new FarPoint.Win.Spread.CellType.NumberCellType();
                        num.DecimalPlaces = defultPlaces;
                        num.ShowSeparator = bShowSeparator;
                        sheet.Columns[col.Ordinal].CellType = num;
                        sheet.Columns[col.Ordinal].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        sheet.Columns[col.Ordinal].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                    else if (col.DataType == typeof(System.Double)
                        || col.DataType == typeof(System.Int16)
                        || col.DataType == typeof(System.Int32)
                        || col.DataType == typeof(System.Int64)
                        || col.DataType == typeof(System.UInt16)
                        || col.DataType == typeof(System.UInt32)
                        || col.DataType == typeof(System.UInt64))
                    {
                        num = new FarPoint.Win.Spread.CellType.NumberCellType();
                        num.DecimalPlaces = 0;
                        num.ShowSeparator = bShowSeparator;
                        sheet.Columns[col.Ordinal].CellType = num;
                        sheet.Columns[col.Ordinal].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        sheet.Columns[col.Ordinal].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                    else if (col.DataType == typeof(System.DateTime))
                    {
                        date = new FarPoint.Win.Spread.CellType.DateTimeCellType();
                        date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                        sheet.Columns[col.Ordinal].CellType = date;
                        sheet.Columns[col.Ordinal].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        sheet.Columns[col.Ordinal].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(
                            string.Format("Not define {0}", col.DataType.Name),
                            "Notice",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                        text = new FarPoint.Win.Spread.CellType.TextCellType();
                        sheet.Columns[col.Ordinal].CellType = text;
                        sheet.Columns[col.Ordinal].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        sheet.Columns[col.Ordinal].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (text != null)
                {
                    text.Dispose();
                    text = null;
                }

                if (num != null)
                {
                    num.Dispose();
                    num = null;
                }

                if (date != null)
                {
                    date.Dispose();
                    date = null;
                }
            }
        }

        public static void FitColumnSize(
            SheetView sv
            )
        {
            for (int a = 0; a < sv.ColumnCount; a++)
                sv.Columns[a].Width = sv.Columns[a].GetPreferredWidth();
        }

        public static void FitColumnSize2(
            SheetView sv
            )
        {
            for (int a = 0; a < sv.ColumnCount; a++)
                sv.Columns[a].Width = sv.GetPreferredColumnWidth(a, true, false) + 15;
        }

        #endregion

        #region [ DataTable Pivot ]
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
        public static DataTable TablePivot(
            DataTable sourceDt,
            string[] sTargetRows,
            string[] sPivotRows,
            string[] sPivotCols
            )
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }

        public static DataTable TablePivot(
            DataTable sourceDt,
            string[] sTargetRows,
            string[] sPivotRows,
            string[] sPivotCols,
            bool bSetDefault
            )
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }

        public static DataTable TablePivot(
            DataRow[] sourceDr,
            string[] sTargetRows,
            string[] sPivotRows,
            string[] sPivotCols
            )
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }

        public static DataTable TablePivot(
            DataRow[] sourceDr,
            string[] sTargetRows,
            string[] sPivotRows,
            string[] sPivotCols,
            bool bSetDefault
            )
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }

        public static DataTable TablePivot(
            DataTable sourceDt,
            string[] sTargetRows,
            string[] sPivotRows,
            string sPivotRowsName,
            string[] sPivotCols
            )
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols);
        }

        public static DataTable TablePivot(
            DataRow[] sourceDr,
            string[] sTargetRows,
            string[] sPivotRows,
            string sPivotRowsName,
            string[] sPivotCols
            )
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, false);
        }

        public static DataTable TablePivot(
            DataTable sourceDt,
            string[] sTargetRows,
            string[] sPivotRows,
            string sPivotRowsName,
            string[] sPivotCols,
            bool bSetDefault
            )
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, bSetDefault);
        }

        public static DataTable TablePivot(
            DataRow[] sourceDr,
            string[] sTargetRows,
            string[] sPivotRows,
            string sPivotRowsName,
            string[] sPivotCols,
            bool bSetDefault
            )
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

        public static DataTable DataPivot(
            DataRow[] sourceDr,
            string[] sDefaultCols,
            int iMaxCols,
            string sPivotCol,
            string sDataCol
            )
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

        public static DataTable PivotInASE(
            DataTable dtSource,
            string[] sTargetRows,
            string sSiteCountCol = "SITE_COUNT",
            string sValColName = "SITE"
            )
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
        #endregion [ DataTable Pivot ]

        #region ReCalcurateColumnWidth
        /// <summary>
        /// Recalculate column width of Spread Sheet
        /// </summary>
        /// <param name="sheetList"></param>
        public static void ReCalcurateColumnWidth(
            params SheetView[] sheetList
            )
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

        #region [ SetSpreadData ]

        /// <summary>
        /// 2013-05-22-정병주 : Sequence Number 형식에 Separator(',') 가 되어 오류가 발생이 되어 임의로 하나 만든다.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheet"></param>
        /// <param name="minColumnWidth"></param>
        /// <param name="defultPlaces"></param>
        public static void SetSpreadDataNotSeparator(
            DataTable dt,
            FarPoint.Win.Spread.SheetView sheet,
            int minColumnWidth,
            int defultPlaces
            )
        {
            try
            {
                sheet.DataSource = dt;

                for (int c = 0; c < sheet.ColumnCount; c++)
                {
                    // DataType Setting
                    switch (dt.Columns[c].DataType.Name.ToUpper())
                    {
                        case "STRING":
                            FarPoint.Win.Spread.CellType.TextCellType text = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = text;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DECIMAL":
                            FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = defultPlaces;
                            num.MaximumValue = 99999999999999;
                            sheet.Columns[c].CellType = num;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "INT16":
                        case "INT32":
                        case "INT64":
                            FarPoint.Win.Spread.CellType.NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
                            intType.DecimalPlaces = 0;
                            sheet.Columns[c].CellType = intType;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DATETIME":
                            FarPoint.Win.Spread.CellType.DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                            date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                            sheet.Columns[c].CellType = date;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DOUBLE":
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "Notice"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = type;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                    }
                    sheet.Columns[c].Width = (sheet.GetPreferredColumnWidth(c, true, false) > minColumnWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetSpreadData(
            DataTable dt,
            FpSpread spread
            )
        {
            SetSpreadData(
                dt,
                spread.ActiveSheet
                );
        }

        public static void SetSpreadData(
            DataTable dt,
            SheetView sheet
            )
        {
            SetSpreadData(
                dt,
                sheet,
                100,
                0,
                true
                );
        }

        public static void SetSpreadData(
            DataTable dt,
            SheetView sheet,
            int minColumnWidth,
            int defaultPlaces
            )
        {
            SetSpreadData(
                dt,
                sheet,
                minColumnWidth,
                defaultPlaces,
                true
                );
        }


        public static void SetSpreadData(
            DataTable dt,
            SheetView sheet,
            int minColumnWidth,
            int defaultPlaces,
            bool bShowSeparator
            )
        {
            try
            {
                sheet.AutoCalculation = false;

                sheet.DataSource = dt;
                AdjustSpread(sheet, minColumnWidth, defaultPlaces, bShowSeparator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void SetSpreadData(DataTable dt, FarPoint.Win.Spread.SheetView sheet, int minColumnWidth, int defultPlaces)
        //{
        //    try
        //    {
        //        sheet.DataSource = dt;

        //        for (int c = 0; c < sheet.ColumnCount; c++)
        //        {
        //            // DataType Setting
        //            switch (dt.Columns[c].DataType.Name.ToUpper())
        //            {
        //                case "STRING":
        //                    FarPoint.Win.Spread.CellType.TextCellType text = new FarPoint.Win.Spread.CellType.TextCellType();
        //                    sheet.Columns[c].CellType = text;
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //                case "DECIMAL":
        //                    FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
        //                    num.DecimalPlaces = defultPlaces;
        //                    num.ShowSeparator = true;
        //                    num.MaximumValue = 99999999999999;
        //                    sheet.Columns[c].CellType = num;
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //                case "INT16":
        //                case "INT32":
        //                case "INT64":
        //                    FarPoint.Win.Spread.CellType.NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
        //                    intType.DecimalPlaces = 0;
        //                    intType.ShowSeparator = true;
        //                    sheet.Columns[c].CellType = intType;
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //                case "DATETIME":
        //                    FarPoint.Win.Spread.CellType.DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

        //                    date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
        //                    sheet.Columns[c].CellType = date;
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //                case "DOUBLE":
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //                default:
        //                    System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "Notice"
        //                                                            , System.Windows.Forms.MessageBoxButtons.OK
        //                                                            , System.Windows.Forms.MessageBoxIcon.Information);
        //                    FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
        //                    sheet.Columns[c].CellType = type;
        //                    sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //                    sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    break;
        //            }
        //            sheet.Columns[c].Width = (sheet.GetPreferredColumnWidth(c, true, false) > minColumnWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minColumnWidth);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion [ SetSpreadData ]

        /// <summary>
        /// 2013-03-22-정병주 : Spread 초기화 시 OperationMode를 Normal
        /// </summary>
        /// <param name="spreadList"></param>
        public static void InitSpreadModeNormal(params FarPoint.Win.Spread.FpSpread[] spreadList)
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

                        sv.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

                        FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(sv);
                    }
                    fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                    fp.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;

                    FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(fp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetDecimalPlaces(FarPoint.Win.Spread.SheetView sheet, int[] arrColIndex, int[] arrDecimalPlaces)
        {
            try
            {
                for (int i = 0; i < arrColIndex.Length; i++)
                {
                    if (sheet.Columns.Count <= arrColIndex[i])
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Input value is too large [{0}]", arrColIndex[i]), "Notice"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                        continue;
                    }

                    FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                    num.DecimalPlaces = arrDecimalPlaces[i];
                    num.ShowSeparator = true;
                    num.MinimumValue = -99999999999999;
                    num.MaximumValue = 99999999999999;
                    sheet.Columns[arrColIndex[i]].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SpreadSorting(int colCnt, FarPoint.Win.Spread.SheetView sv)
        {
            try
            {
                if (colCnt == 0)
                    return;

                for (int i = 0; i < sv.ColumnCount; i++)
                {
                    sv.SetColumnAllowAutoSort(i, true);
                    sv.GetColumnAllowAutoSort(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SpreadSortingAll(FarPoint.Win.Spread.SheetView sv)
        {
            try
            {
                for (int i = 0; i < sv.ColumnCount; i++)
                {
                    sv.SetColumnAllowAutoSort(i, true);
                    sv.GetColumnAllowAutoSort(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Set Decimal Length

        private static int[] GetIntArray(Enum[] enumArray)
        {
            if (enumArray == null || enumArray.Length == 0)
                return null;

            int[] arr = new int[enumArray.Length];

            for (int i = 0; i < enumArray.Length; i++)
                arr[i] = (int)(object)enumArray[i];

            return arr;
        }

        public static void SetDecimalLength(SheetView sheet, int decimalLength, params Enum[] enumArray)
        {
            SetDecimalLength(sheet, decimalLength, GetIntArray(enumArray));
        }

        public static void SetDecimalLength(SheetView sheet, int decimalLength, int[] columns)
        {
            if (sheet == null || sheet.ColumnCount == 0 || columns == null || columns.Length == 0)
                return;

            foreach (int column in columns)
            {
                if (column > sheet.ColumnCount - 1)
                    continue;

                if (sheet.Columns[column].CellType is NumberCellType)
                    (sheet.Columns[column].CellType as NumberCellType).DecimalPlaces = decimalLength;
            }

            sheet.FpSpread.Refresh();
        }

        public static void SetDecimalLength(SheetView sheet, int decimalLength, int row, params Enum[] enumArray)
        {
            SetDecimalLength(sheet, decimalLength, row, GetIntArray(enumArray));
        }

        public static void SetDecimalLength(SheetView sheet, int decimalLength, int row, int[] columns)
        {
            if (sheet == null || sheet.ColumnCount == 0 || columns == null || columns.Length == 0)
                return;

            foreach (int column in columns)
            {
                if (column >= sheet.ColumnCount)
                    continue;

                if (sheet.Cells[row, column].CellType is NumberCellType)
                {
                    (sheet.Cells[row, column].CellType as NumberCellType).DecimalPlaces = decimalLength;
                }
                else if (sheet.Columns[column].CellType is NumberCellType)
                {
                    if (sheet.Cells[row, column].CellType == null)
                        sheet.Cells[row, column].CellType = new NumberCellType();

                    (sheet.Cells[row, column].CellType as NumberCellType).DecimalPlaces = decimalLength;
                }
            }

            sheet.FpSpread.Refresh();
        }

        public static void SetDecimalLength(SheetView sheet, int decimalLength)
        {
            if (sheet == null || sheet.ColumnCount == 0)
                return;

            for (int column = 0; column < sheet.ColumnCount; column++)
            {
                if (sheet.Columns[column].CellType is NumberCellType)
                    (sheet.Columns[column].CellType as NumberCellType).DecimalPlaces = decimalLength;
            }

            sheet.FpSpread.Refresh();
        }

        public static void SetDecimalLength(SheetView sheet, int row, int col, int decimalLength)
        {
            if (sheet == null || sheet.ColumnCount == 0 || sheet.RowCount <= row || sheet.ColumnCount <= col)
                return;

            sheet.Cells[row, col].CellType = new FarPoint.Win.Spread.CellType.NumberCellType() { DecimalPlaces = decimalLength };
            sheet.FpSpread.Refresh();
        }

        #endregion

        #region Auto Column Width

        public static readonly int COLUMN_WIDTH_DEFAULT_MARGIN = 5;
        public static readonly int COLUMN_AUTOSORT_DEFAULT_MARGIN = 15;
        public static readonly int COLUMN_AUTOFILTER_DEFAULT_MARGIN = 15;

        /// <summary>
        /// Spread의 너비를 자동 조정합니다.
        /// </summary>
        /// <param name="sheet">SheetView</param>
        public static void SetAutoColumnWidth(FarPoint.Win.Spread.SheetView sheet)
        {
            SetAutoColumnWidth(sheet, true, 0);
        }

        /// <summary>
        /// Spread의 너비를 자동 조정합니다.
        /// </summary>
        /// <param name="sheet">SheetView</param>
        /// <param name="checkRow">너비를 검사할 최대 Row 갯수입니다.</param>
        public static void SetAutoColumnWidth(FarPoint.Win.Spread.SheetView sheet, int checkRow)
        {
            SetAutoColumnWidth(sheet, false, checkRow);
        }

        /// <summary>
        /// Spread의 너비를 자동 조정합니다.
        /// </summary>
        /// <param name="sheet">SheetView</param>
        /// <param name="allRow">모든 Row에 대해서 검사할지를 지정합니다.</param>
        /// <param name="checkRow">너비를 검사할 최대 Row 갯수입니다.</param>
        public static void SetAutoColumnWidth(FarPoint.Win.Spread.SheetView sheet, bool allRow, int checkRow)
        {
            int[] arr = new int[sheet.ColumnCount];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = i;

            SetAutoColumnWidth(sheet, allRow, checkRow, arr);
        }

        /// <summary>
        /// Spread의 너비를 자동 조정합니다.
        /// </summary>
        /// <param name="sheet">SheetView</param>
        /// <param name="allRow">모든 Row에 대해서 검사할지를 지정합니다.</param>
        /// <param name="checkRow">너비를 검사할 최대 Row 갯수입니다.</param>
        public static void SetAutoColumnWidth(FarPoint.Win.Spread.SheetView sheet, bool allRow, int checkRow, int[] columnIndices)
        {
            using (Graphics g = sheet.FpSpread.CreateGraphics())
            {
                int cnt = allRow ? sheet.RowCount : Math.Min(checkRow, sheet.RowCount);
                Font font = sheet.FpSpread.Font;
                Font headerFont = sheet.ColumnHeader.DefaultStyle.Font != null ? sheet.ColumnHeader.DefaultStyle.Font : font;

                for (int i = 0; i < columnIndices.Length; i++)
                {
                    int col = columnIndices[i];
                    float max = 0;

                    int headerMargin = 0;
                    headerMargin += sheet.Columns[col].AllowAutoSort ? COLUMN_AUTOSORT_DEFAULT_MARGIN : 0;
                    headerMargin += sheet.Columns[col].AllowAutoFilter ? COLUMN_AUTOFILTER_DEFAULT_MARGIN : 0;

                    max = Math.Max(g.MeasureString(sheet.ColumnHeader.Columns[col].Label, headerFont).Width + headerMargin + COLUMN_WIDTH_DEFAULT_MARGIN, max);

                    if (sheet.ColumnFooter.Visible)
                        max = Math.Max(g.MeasureString(sheet.ColumnFooter.Cells[0, col].Text, headerFont).Width + COLUMN_WIDTH_DEFAULT_MARGIN, max);

                    for (int r = 0; r < cnt; r++)
                    {
                        string text = sheet.Cells[r, col].Text;

                        if (String.IsNullOrEmpty(text))
                        {
                            object value = sheet.Cells[r, col].Value;

                            if (value != null && value != DBNull.Value)
                                text = value.ToString();
                        }

                        max = Math.Max(g.MeasureString(text, font).Width + COLUMN_WIDTH_DEFAULT_MARGIN, max);
                    }

                    sheet.Columns[col].Width = max;
                }
            }
        }

        #endregion

        #region Auto Column Sort

        public static void SetColumnSort(FarPoint.Win.Spread.SheetView sheet, bool allowSort, params int[] columns)
        {
            if (sheet == null || sheet.ColumnCount == 0)
                return;

            foreach (int column in columns)
            {
                if (column >= sheet.ColumnCount - 1)
                    continue;

                sheet.Columns[column].AllowAutoSort = allowSort;
            }
        }

        public static void SetAutoColumnSort(FarPoint.Win.Spread.SheetView sheet)
        {
            SetAutoColumnSort(sheet, true);
        }

        public static void SetAutoColumnSort(FarPoint.Win.Spread.SheetView sheet, bool allowSort)
        {
            if (sheet == null || sheet.ColumnCount == 0)
                return;

            for (int i = 0; i < sheet.ColumnCount; i++)
                sheet.Columns[i].AllowAutoSort = allowSort;
        }

        public static void SetAutoColumnSort(FarPoint.Win.Spread.SheetView sheet, bool allowSort, params Enum[] columns)
        {
            if (columns == null || columns.Length == 0)
                return;

            int[] arr = new int[columns.Length];

            for (int i = 0; i < columns.Length; i++)
                arr[i] = (int)(object)columns[i];

            SetAutoColumnSort(sheet, allowSort, arr);
        }

        public static void SetAutoColumnSort(FarPoint.Win.Spread.SheetView sheet, bool allowSort, params int[] columns)
        {
            if (sheet == null || sheet.ColumnCount == 0 || columns == null || columns.Length == 0)
                return;

            foreach (int index in columns)
                sheet.Columns[index].AllowAutoSort = allowSort;
        }

        #endregion

        #region Auto Column Filter

        public static void SetAutoColumnFilter(FarPoint.Win.Spread.SheetView sheet, bool allowFilter, params Enum[] columns)
        {
            if (columns == null || columns.Length == 0)
                return;

            int[] arr = new int[columns.Length];

            for (int i = 0; i < columns.Length; i++)
                arr[i] = (int)(object)columns[i];

            SetAutoColumnFilter(sheet, allowFilter, arr);
        }

        public static void SetAutoColumnFilter(FarPoint.Win.Spread.SheetView sheet, bool allowFilter, params int[] columns)
        {
            if (sheet == null || sheet.ColumnCount == 0)
                return;

            foreach (int column in columns)
            {
                if (column >= sheet.ColumnCount - 1)
                    continue;

                sheet.Columns[column].AllowAutoFilter = allowFilter;
            }
        }

        public static void SetAutoColumnFilter(FarPoint.Win.Spread.SheetView sheet)
        {
            SetAutoColumnFilter(sheet, true);
        }

        public static void SetAutoColumnFilter(FarPoint.Win.Spread.SheetView sheet, bool allowFilter)
        {
            if (sheet == null || sheet.ColumnCount == 0)
                return;

            for (int i = 0; i < sheet.ColumnCount; i++)
                sheet.Columns[i].AllowAutoFilter = allowFilter;
        }

        #endregion

    }
}
