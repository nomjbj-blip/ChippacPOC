using System;
using System.Windows;
using System.Reflection;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace DACrux.TEST.Control
{
    /// <summary>
    /// Class Name : ExcelUtil<br/>
    /// Summary    : Excel Handling Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class ExcelUtil : IDisposable
	{
		#region Class Member

		private bool disposed = false;

		private Microsoft.Office.Interop.Excel.Application oExcel;
		private Microsoft.Office.Interop.Excel._Workbook oWorkBook;
		private Microsoft.Office.Interop.Excel._Worksheet oSheet;


		#endregion

		#region Work Book Open / Close

		/// <summary>
        /// Open Work Book
		/// </summary>
		public void OpenWorkBook()
		{
			oExcel = new Microsoft.Office.Interop.Excel.Application();
			oExcel.Visible = true;

			oWorkBook = (Microsoft.Office.Interop.Excel._Workbook)(oExcel.Workbooks.Add(Missing.Value));
			oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWorkBook.ActiveSheet;
		}

		/// <summary>
        /// Close Work Book
		/// </summary>
		public void CloseWorkBook()
		{
			oWorkBook.Close(Missing.Value, Missing.Value, Missing.Value);
		}

		#endregion

		#region Output - Grid , Excel

		/// <summary>
		/// View Excel with Grid Header
		/// </summary>
		/// <param name="fpSpread"></param>
		public void SetValueWithHeader(FarPoint.Win.Spread.FpSpread fpSpread)
		{
			int iCurrentRow = 1;
			int iRowCount = fpSpread.ActiveSheet.RowCount;
			int iColCount = fpSpread.ActiveSheet.ColumnCount;

			oSheet.Activate();

			// Fill Header
			for (int i=0; i<iColCount; i++)
			{
				oSheet.Cells[1, i+1] = fpSpread.ActiveSheet.Columns[i].Label;
				oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, i+1]).get_Resize(Missing.Value, Missing.Value);
				oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, i+1]).Interior.ColorIndex = 10;
				oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, i+1]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
				oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, i+1]).Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
			}

			// Fill Contents
			for (int i=0; i<iRowCount; i++)
			{
				iCurrentRow++;
				for (int j=0; j<iColCount; j++)
				{
					oSheet.Cells[iCurrentRow, j+1] = fpSpread.ActiveSheet.Cells[i, j];
					oSheet.get_Range(oSheet.Cells[iCurrentRow, j+1],oSheet.Cells[iCurrentRow, j+1]).ColumnWidth = fpSpread.ActiveSheet.Columns[j].Width*8.11/80;
					oSheet.get_Range(oSheet.Cells[iCurrentRow, j+1],oSheet.Cells[iCurrentRow, j+1]).Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
				}
			}
		}

		/// <summary>
        /// View Excel without Grid Header
		/// </summary>
		/// <param name="fpSpread"></param>
		public void SetValueWithoutHeader(FarPoint.Win.Spread.FpSpread fpSpread)
		{
			int iRowCount = fpSpread.ActiveSheet.RowCount;
			int iColCount = fpSpread.ActiveSheet.ColumnCount;

			oSheet.Activate();

			for (int i=0; i<iRowCount; i++)
			{
				for (int j=0; j<iColCount; j++)
				{
					oSheet.Cells[i+1, j+1] = fpSpread.ActiveSheet.Cells[i, j];
					oSheet.get_Range(oSheet.Cells[i+1, j+1],oSheet.Cells[i+1, j+1]).Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
				}
			}
		}

		/// <summary>
        /// Set Values Excel with Grid Header
		/// </summary>
		/// <param name="fpSpread"></param>
		public void SetValuesWithHeaderMy(FarPoint.Win.Spread.FpSpread fpSpread)
		{
			string strFileName = @"C:\temp\" + GetDateTime() + ".xls";

			if(!System.IO.Directory.Exists(@"C:\temp"))
				System.IO.Directory.CreateDirectory(@"C:\temp");

			oExcel.Visible = false;
			fpSpread.SaveExcel(strFileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
			oWorkBook.Close(false, Missing.Value, false);
			oWorkBook = oExcel.Workbooks.Open(strFileName, XlUpdateLinks.xlUpdateLinksNever, true, Missing.Value, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, false, false, Missing.Value, false, true, Missing.Value);
			oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWorkBook.ActiveSheet;

			oSheet.Unprotect(Missing.Value);
			oExcel.Visible = true;

			System.IO.File.Delete(strFileName);
		}

		/// <summary>
        /// Set Values by array
        /// </summary>
		/// <param name="fpSpread"></param>
		public void SetValuesByArray(FarPoint.Win.Spread.FpSpread fpSpread)
		{
			int intColCount = fpSpread.ActiveSheet.ColumnCount;
			int intRowCount = fpSpread.ActiveSheet.RowCount;

			oSheet.Activate();

			Range wksRange;
			wksRange = oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[intRowCount, intColCount]);
			wksRange.set_Value(Missing.Value, fpSpread.ActiveSheet.GetArray(0, 0, intRowCount, intColCount));
			wksRange.NumberFormatLocal = "0.00_ ";
		}

		#endregion

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
			
			oSaveFileDlg.FileName = strProgName + GetDateTime() + ".xls";

			if (System.Windows.Forms.DialogResult.OK == oSaveFileDlg.ShowDialog())
				fpSpread.SaveExcel(oSaveFileDlg.FileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
		}
		#endregion

        #region GetDateTime
        /// <summary>
		/// Get Current DateTime (yyyyMMddhhmmss)
		/// </summary>
		/// <returns></returns>
		private string GetDateTime()
		{
			return DateTime.Now.ToString("yyyyMMddhhmmss");
		}
		#endregion

        #region Dispose
        /// <summary>
        /// Dispose all resources
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
        /// Dispose all resources
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				disposed = true;
			}
		}

		/// <summary>
        /// Call Dispose
		/// </summary>
		~ExcelUtil()
		{
			Dispose(false);
		}

		#endregion



        public enum Color
        {
            //			Black = 0 ,	Blue =1,	Green =2, 		Cyan =3, 	
            //			Red = 3 ,	Magenta =7 ,	Yellow =6, 		White =15,
            //			Gray =15,	Lightblue = 9,	Lightgreen =4,	Lightcyan =11, 
            //			Lightred =12, Lightmagenta = 13, Lightyellow = 14, Brightwhite=15

            black = 1, blue = 5, bluegreen = 34, brown = 9, darkblue = 11, darkbrown = 52,
            darkgray = 56, darkgreen = 51, darkviolet = 54, darkyellow = 44, darkyellowgreen = 12,
            gray = 48, green = 10, indigo = 49, lightbluish = 33, lightbrown = 53, lightgray = 15,
            lightgreen = 4, lightorange = 45, lightsky = 8, magenta = 39, mgray = 16, orange = 46,
            pastelblue = 14, pastelbluegreen = 35, pastelbluesky = 42, pastelgreen = 50,
            pastelorange = 40, pastelpink = 38, pastelsky = 37, pastelviolet = 47, pastelyellow = 36,
            pink = 7, red = 3, sblue = 55, skyblue = 41, violet = 13, white = 2, yellow = 6,
            yellowgreen = 43,

        }

        public enum Alignment
        {
            xlVAlignCenter = Excel.XlVAlign.xlVAlignCenter,
            xlVAlignBottom = Excel.XlVAlign.xlVAlignBottom,
            xlVAlignDistributed = Excel.XlVAlign.xlVAlignDistributed,
            xlVAlignJustify = Excel.XlVAlign.xlVAlignJustify,
            xlVAlignTop = Excel.XlVAlign.xlVAlignTop
        }

        public enum HAlignment
        {
            xlHAlignCenter = Excel.XlHAlign.xlHAlignCenter,
            xlHAlignCenterAcrossSelection = Excel.XlHAlign.xlHAlignCenterAcrossSelection,
            xlHAlignDistributed = Excel.XlHAlign.xlHAlignDistributed,
            xlHAlignFill = Excel.XlHAlign.xlHAlignFill,
            xlHAlignGeneral = Excel.XlHAlign.xlHAlignGeneral,
            xlHAlignJustify = Excel.XlHAlign.xlHAlignJustify,
            xlHAlignLeft = Excel.XlHAlign.xlHAlignLeft,
            xlHAlignRight = Excel.XlHAlign.xlHAlignRight
        }

        /// <summary>
        /// 엑셀 workbook 얻기
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="visible">엑셀 보이기 여부</param>
        /// <param name="sheetsCount">생성할 엑셀시트 수</param>
        /// <returns></returns>
        public Excel._Workbook fnGetExcelWorkbook(bool visible, int sheetCounts)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;

            oXL = new Excel.Application();
            oXL.Visible = visible;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            if (sheetCounts > 0)
                oWB.Worksheets.Add(Missing.Value, Missing.Value, sheetCounts, Missing.Value);

            return oWB;
        }

        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, string[,] values)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
        }


        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, object values)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
        }

        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">정렬</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
                                 int rowIdx2, int colIdx2, string[,] values,
                                 bool bold, string font, int size, Color color, Alignment align)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).Font.Bold = bold;
            workSheet.get_Range(c1, c2).Font.Name = font;
            workSheet.get_Range(c1, c2).Font.Size = size;
            workSheet.get_Range(c1, c2).Font.ColorIndex = color;
            workSheet.get_Range(c1, c2).HorizontalAlignment = align;
            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
        }

        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, string[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;

            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).Font.Bold = bold;
            workSheet.get_Range(c1, c2).Font.Name = font;
            workSheet.get_Range(c1, c2).Font.Size = size;
            workSheet.get_Range(c1, c2).Font.ColorIndex = color;
            workSheet.get_Range(c1, c2).VerticalAlignment = align;
            workSheet.get_Range(c1, c2).HorizontalAlignment = Halign;

            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
            if (gridline)
            {
                workSheet.get_Range(c1, c2).Borders.Weight = Excel.XlBorderWeight.xlThin;
            }
            workSheet.get_Range(c1, c2).Interior.ColorIndex = bgcolor;
        }
        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 3월 2일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값(double array)</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, double[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            Excel.Range oRange;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            oRange = workSheet.get_Range(c1, c2);
            oRange.Font.Bold = bold;
            oRange.Font.Name = font;
            oRange.Font.Size = size;
            oRange.Font.ColorIndex = color;
            oRange.VerticalAlignment = align;
            oRange.HorizontalAlignment = Halign;
            if (gridline)
            {
                oRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
            }
            oRange.Interior.ColorIndex = bgcolor;
            oRange.set_Value(Missing.Value, values);
        }
        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 3월 2일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값(date array)</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, DateTime[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            Excel.Range oRange;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            oRange = workSheet.get_Range(c1, c2);

            oRange.Font.Bold = bold;
            oRange.Font.Name = font;
            oRange.Font.Size = size;
            oRange.Font.ColorIndex = color;
            oRange.VerticalAlignment = align;
            oRange.HorizontalAlignment = Halign;
            oRange.NumberFormat = "YY/MM/DD HH:MM";
            if (gridline)
            {
                oRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
            }
            oRange.Interior.ColorIndex = bgcolor;
            oRange.set_Value(Missing.Value, values);
        }
        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 3월 2일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값(int Array)</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, int[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            Excel.Range oRange;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            oRange = workSheet.get_Range(c1, c2);
            oRange.Font.Bold = bold;
            oRange.Font.Name = font;
            oRange.Font.Size = size;
            oRange.Font.ColorIndex = color;
            oRange.VerticalAlignment = align;
            oRange.HorizontalAlignment = Halign;
            if (gridline)
            {
                oRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
            }
            oRange.Interior.ColorIndex = bgcolor;
            oRange.set_Value(Missing.Value, values);
        }
        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>
        /// <param name="merge">셀병합여부</param>

        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, string[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor, bool merge)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;


            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            if (merge)
                workSheet.get_Range(c1, c2).Merge(merge);

            workSheet.get_Range(c1, c2).Font.Bold = bold;
            workSheet.get_Range(c1, c2).Font.Name = font;
            workSheet.get_Range(c1, c2).Font.Size = size;
            workSheet.get_Range(c1, c2).Font.ColorIndex = color;
            workSheet.get_Range(c1, c2).VerticalAlignment = align;
            workSheet.get_Range(c1, c2).HorizontalAlignment = Halign;

            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
            if (gridline)
                workSheet.get_Range(c1, c2).Borders.Weight = Excel.XlBorderWeight.xlThin;


            workSheet.get_Range(c1, c2).Interior.ColorIndex = bgcolor;
        }
        /// <summary>
        /// 엑셀에 값 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 3월 12일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        /// <param name="values">값</param>
        /// <param name="bold">볼드체사용여부</param>
        /// <param name="font">폰트종류</param>
        /// <param name="size">폰트사이즈</param>
        /// <param name="color">폰트색상</param>
        /// <param name="align">수직정렬</param>
        /// <param name="Halign">수평정렬</param>
        /// <param name="gridline">격자생성여부</param>
        /// <param name="bgcolor">셀배경색상</param>
        /// <param name="merge">셀병합여부</param>
        /// <param name="rowHeight">열높이</param>
        public void fnSetValue(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2, string[,] values,
            bool bold, string font, int size, Color color, Alignment align, HAlignment Halign, bool gridline, Color bgcolor, bool merge,
            int rowHeight)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;


            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx2, colIdx2] as Microsoft.Office.Interop.Excel.Range;
            if (merge)
                workSheet.get_Range(workSheet.Cells[rowIdx1, colIdx1], workSheet.Cells[rowIdx2, colIdx2]).Merge(merge);

            workSheet.get_Range(c1, c2).Font.Bold = bold;
            workSheet.get_Range(c1, c2).Font.Name = font;
            workSheet.get_Range(c1, c2).Font.Size = size;
            workSheet.get_Range(c1, c2).Font.ColorIndex = color;
            workSheet.get_Range(c1, c2).VerticalAlignment = align;
            workSheet.get_Range(c1, c2).HorizontalAlignment = Halign;
            workSheet.get_Range(c1, c2).RowHeight = rowHeight;
            //workSheet.get_Range(workSheet.Cells[rowIdx1,colIdx1],workSheet.Cells[rowIdx2,colIdx2]).ColumnWidth = colWidth;

            workSheet.get_Range(c1, c2).set_Value(Missing.Value, values);
            if (gridline)
                workSheet.get_Range(c1, c2).Borders.Weight = Excel.XlBorderWeight.xlThin;


            workSheet.get_Range(c1, c2).Interior.ColorIndex = bgcolor;
        }

        /// <summary>
        /// 엑셀Range Object 얻어오기
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 3월 2일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="rowIdx1">시작행</param>
        /// <param name="colIdx1">시작열</param>
        /// <param name="rowIdx2">끝행</param>
        /// <param name="colIdx2">끝열</param>
        public Excel.Range fnGetRange(Excel._Workbook workbook, int sheetIdx, int rowIdx1, int colIdx1,
            int rowIdx2, int colIdx2)
        {
            if (rowIdx1 < 1 || colIdx1 < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;

            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx1, colIdx1] as Microsoft.Office.Interop.Excel.Range;

            Excel.Range chartRange = workSheet.get_Range(c1, c2);
            return chartRange;

        }

        /// <summary>
        /// 엑셀시트명 적용
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="sheetname">시트명</param>

        public void fnSetSheetName(Excel._Workbook workbook, int sheetIdx, string sheetname)
        {
            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();
            workSheet.Name = sheetname;
        }

        /// <summary>
        /// 엑셀에 이미지 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="filePath">이미지 파일 경로</param>
        /// <param name="rowIdx">시작행</param>
        /// <param name="colIdx">시작열</param>
        /// 
        public void fnSetImage(Excel._Workbook workbook, int sheetIdx, string filePath,
            int rowIdx, int colIdx)
        {
            if (rowIdx < 1 || colIdx < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Excel.Pictures oPic;

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).Select();

            oPic = (Excel.Pictures)workSheet.Pictures(Missing.Value);
            oPic.Insert(filePath, Missing.Value).Select(true);
        }

        /// <summary>
        /// 엑셀에 이미지 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="filePath">이미지 파일 경로</param>
        /// <param name="rowIdx">시작행</param>
        /// <param name="colIdx">시작열</param>
        /// <param name="width">이미지 넓이 pix</param>
        /// <param name="height">이미지 높이 pix</param>
        public void fnSetImage(Excel._Workbook workbook, int sheetIdx, string filePath,
            int rowIdx, int colIdx, int width, int height)
        {
            if (rowIdx < 1 || colIdx < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Excel.Pictures oPic;

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).Select();
            oPic = (Excel.Pictures)workSheet.Pictures(Missing.Value);
            oPic.Insert(filePath, Missing.Value).Select(true);

            oPic.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoFalse;
            int iPicCount = oPic.Count;

            workSheet.Shapes.get_Range(iPicCount).Select(Missing.Value);
            workSheet.Shapes.get_Range(iPicCount).Height = height;
            workSheet.Shapes.get_Range(iPicCount).Width = width;
        }

        /// <summary>
        /// 엑셀에 이미지 세팅
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="filePath">이미지 파일 경로</param>
        /// <param name="rowIdx">시작행</param>
        /// <param name="colIdx">시작열</param>
        /// <param name="width">폭배율</param>
        /// <param name="height">높이배율</param>
        /// 

        public void fnSetImage(Excel._Workbook workbook, int sheetIdx, string filePath,
            int rowIdx, int colIdx, float width, float height)
        {
            if (rowIdx < 1 || colIdx < 1)
                throw new Exception("시작셀의 인덱스는 1부터 시작합니다.");

            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Excel.Pictures oPic;

            Microsoft.Office.Interop.Excel.Range c1 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;
            Microsoft.Office.Interop.Excel.Range c2 = workSheet.Cells[rowIdx, colIdx] as Microsoft.Office.Interop.Excel.Range;

            workSheet.get_Range(c1, c2).Select();

            oPic = (Excel.Pictures)workSheet.Pictures(Missing.Value);
            oPic.Insert(filePath, Missing.Value).Select(true);

            oPic.ShapeRange.ScaleHeight(height, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoScaleFrom.msoScaleFromTopLeft);
            oPic.ShapeRange.ScaleWidth(width, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoScaleFrom.msoScaleFromTopLeft);
        }
        /// <summary>
        /// 엑셀 시트의 이미지 규격화 함수
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="sheetIdx">시트 인덱스</param>
        /// <param name="width">폭(pix)</param>
        /// <param name="height">높이(pix)</param>
        public void fnResize(Excel._Workbook workbook, int sheetIdx, int width, int height)
        {
            Excel._Worksheet workSheet;
            workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
            workSheet.Activate();

            Excel.Pictures oPic;
            oPic = (Excel.Pictures)workSheet.Pictures(Missing.Value);
            oPic.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoFalse;
            for (int i = 1; i < oPic.Count + 1; i++)
            {
                workSheet.Shapes.get_Range(i).Select(Missing.Value);
                workSheet.Shapes.get_Range(i).Height = height;
                workSheet.Shapes.get_Range(i).Width = width;
            }
        }

        /// <summary>
        /// 엑셀 파일 저장
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        /// <param name="fileName">저장할 파일 경로</param>
        public void fnSaveWorkbook(Excel._Workbook workbook, string fileName)
        {
            workbook.SaveAs(fileName, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        }
        /// <summary>
        /// 엑셀 차트
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 4월 12일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="oWorkSheet">워크시트 개체</param>
        /// <param name="dLeft">Left 위치</param>
        /// <param name="dTop">Top 위치</param>
        /// <param name="dWidth">사이즈</param>
        /// <param name="dHeight">폭</param>
        public Excel._Chart fnGetChartObject(Excel.Worksheet oWorkSheet, double dLeft, double dTop, double dWidth, double dHeight)
        {
            Excel.ChartObjects oChartObjects = null;
            Excel.ChartObject oChartObj = null;
            Excel._Chart oChartInst = null;

            try
            {
                oChartObjects = (Excel.ChartObjects)oWorkSheet.ChartObjects(Type.Missing);
                oChartObj = oChartObjects.Add(dLeft, dTop, dWidth, dHeight);
                oChartInst = oChartObj.Chart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oChartInst;

        }
        /// <summary>
        /// 엑셀 차트
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 4월 12일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="oChartInst"></param>
        /// <param name="oRangeValue"></param>
        /// <param name="sChartName"></param>
        public void fnChardWizard(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void fnChardWizard(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, string sPlotBy)
        {
            Excel.Series oSeries = null;

            try
            {
                if (sPlotBy.Equals("Col"))  // 열방향
                {
                    oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                        Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);
                }
                else				// 행방향
                {
                    oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                        Excel.XlRowCol.xlRows, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);
                }
                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // xy line draw on/off 파라미터 추가
        public void fnChardWizard(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, bool bDrawLine)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;


                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnChardWizard(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, bool bDrawLine, double dMaxScale)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;
                oAxisY.MaximumScale = dMaxScale;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;


                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnChardWizardOneTimeXValue(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, double dFirstScanDate, bool bDrawLine)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.MinimumScale = dFirstScanDate - 3;
                oAxisX.MaximumScale = dFirstScanDate + 3;
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;

                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnChardWizardOneTimeXValue(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, double dFirstScanDate, bool bDrawLine, double dMaxScale)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;
                oAxisY.MaximumScale = dMaxScale;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.MinimumScale = dFirstScanDate - 3;
                oAxisX.MaximumScale = dFirstScanDate + 3;
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;

                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnChardWizardLogView(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, double dMaxScale, bool bDrawLine)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);


                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.ScaleType = Excel.XlScaleType.xlScaleLogarithmic;
                oAxisY.MaximumScale = dMaxScale;
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;

                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fnChardWizardLogViewOneTimeXValue(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sChartName, string sSeriesName, double dMaxScale, double dFirstScanDate, bool bDrawLine)
        {
            Excel.Series oSeries = null;

            try
            {
                oChartInst.ChartWizard(oRangeValue, Excel.XlChartType.xlXYScatter, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, sChartName, Type.Missing, Type.Missing, Type.Missing);

                Excel.Axis oAxisY = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                oAxisY.HasMajorGridlines = bDrawLine;
                oAxisY.HasMinorGridlines = bDrawLine;
                oAxisY.ScaleType = Excel.XlScaleType.xlScaleLogarithmic;
                oAxisY.MaximumScale = dMaxScale;

                Excel.Axis oAxisX = (Excel.Axis)oChartInst.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                oAxisX.MinimumScale = dFirstScanDate - 3;
                oAxisX.MaximumScale = dFirstScanDate + 3;
                oAxisX.HasMinorGridlines = bDrawLine;
                oAxisX.HasMajorGridlines = bDrawLine;

                oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 엑셀 차트
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2005년 4월 12일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="oChartInst"></param>
        /// <param name="oRangeValue"></param>
        /// <param name="sChartName"></param>
        public void fnSetChardAddSeries(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sSeriesName, int iSeriesIndex)
        {
            Excel.SeriesCollection oSeriesCollection = null;
            Excel.Series oSeries = null;

            oSeriesCollection = (Excel.SeriesCollection)oChartInst.SeriesCollection(Type.Missing);

            try
            {
                oSeriesCollection.Add(oRangeValue, Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
            try
            {
                oSeries = (Excel.Series)oChartInst.SeriesCollection(iSeriesIndex);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
        }
        public void fnSetChardAddSeries(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sSeriesName, int iSeriesIndex, string sPlotBy)
        {
            Excel.SeriesCollection oSeriesCollection = null;
            Excel.Series oSeries = null;

            oSeriesCollection = (Excel.SeriesCollection)oChartInst.SeriesCollection(Type.Missing);

            try
            {
                if (sPlotBy.Equals("Col"))
                {
                    oSeriesCollection.Add(oRangeValue, Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing);
                }
                else
                {
                    oSeriesCollection.Add(oRangeValue, Excel.XlRowCol.xlRows, Type.Missing, Type.Missing, Type.Missing);
                }
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
            try
            {
                oSeries = (Excel.Series)oChartInst.SeriesCollection(iSeriesIndex);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
        }
        public void fnSetChardAddSeriesLimitLine(Excel._Chart oChartInst, Excel.Range oRangeXValue, Excel.Range oRangeValue, string sSeriesName, int iSeriesIndex, int iLimit, bool bOneData)
        {
            Excel.SeriesCollection oSeriesCollection = null;
            Excel.Series oSeries = null;
            Excel.Trendlines oTrendLines = null;
            Excel.Trendline oTrendLine = null;

            oSeriesCollection = (Excel.SeriesCollection)oChartInst.SeriesCollection(Type.Missing);

            try
            {
                oSeriesCollection.Add(oRangeValue, Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
            try
            {

                oSeries = (Excel.Series)oChartInst.SeriesCollection(iSeriesIndex);
                oSeries.XValues = oRangeXValue;
                oSeries.Name = sSeriesName;
                oSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;
                oTrendLines = (Excel.Trendlines)oSeries.Trendlines(Type.Missing);
                //				if(bOneData)
                //				{
                oTrendLine = oTrendLines.Add(Excel.XlTrendlineType.xlLinear, Type.Missing, Type.Missing, 2, 2, iLimit, Type.Missing, Type.Missing, iLimit.ToString());
                //				}
                //				else
                //				{
                //					oTrendLine = oTrendLines.Add(Excel.XlTrendlineType.xlLinear,Type.Missing,Type.Missing,Type.Missing,Type.Missing,iLimit,Type.Missing,Type.Missing,iLimit.ToString());
                //				}

                oTrendLine.Border.Color = System.Drawing.Color.Red.R;
            }
            catch (Exception ex)
            { if (ex.GetType().Name.ToString() != "InvalidCastException") { throw ex; } }
        }

        /// <summary>
        /// 워크북 닫기
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 9월 8일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        /// <param name="workbook">워크북 오브젝트</param>
        public void fnCloseWorkbook(Excel._Workbook workbook)
        {
            workbook.Close(Missing.Value, Missing.Value, Missing.Value);
        }

        /// <summary>
        /// 엑셀 프로세스 끝내기
        /// - 작  성  자 : 곽동일
        /// - 최초작성일 : 2004년 10월 20일
        /// - 최종수정자 : 
        /// - 최종수정일 : 
        /// - 주요변경로그
        /// </summary>
        public static void fnExcelProcessExit()
        {
            GC.Collect();                                                                                                                             //가비지 컬렉터
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 모든 cell의 크기를 바꾼다
        /// </summary>
        /// <param name="workbook">excel 객체</param>
        /// <param name="sheetIdx">sheet index</param>
        /// <param name="width">cell 넓이</param>
        /// <param name="height">cell 높이</param>
        public void fnAllCellResize(Excel._Workbook workbook, int sheetIdx, int width, int height)
        {
            try
            {
                Excel._Worksheet workSheet;
                workSheet = (Excel._Worksheet)workbook.Worksheets[sheetIdx];
                workSheet.Activate();

                workSheet.Rows.RowHeight = height * 375 / 500;
                workSheet.Columns.ColumnWidth = width * 54.78 / 500;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 엑셀파일의 내용을 Spread로 임포트 합니다.
        /// </summary>
        /// <param name="obj">Spread객체</param>
        /// <param name="ExcelName">path를 포함하는 파일명</param>
        /// <param name="Excel_Start_Row">읽어올 엑셀의 시작 로우 (1부터 시작)</param>
        /// <param name="Excel_Start_Col">읽어올 엑셀의 시작 컬럼 (1부터 시작)</param>
        /// <param name="Spread_Start_Row">출력해줄 Spread의 시작 로우 Index</param>
        /// <param name="Spread_Start_Col">출력해줄 Spread의 시작 컬럼 index</param>
        /// <param name="removeHeaderColumn">엑셀의 첫번째 로우를 삭제 하여 임포트 할것인가 설정 true= 삭제,false = 그냥둠</param>
        /// <returns></returns>
        public static bool ExcelToSpread(object obj, string ExcelName, int Excel_Start_Row, int Excel_Start_Col, int Spread_Start_Row, int Spread_Start_Col, bool removeHeaderColumn, int i_Col_Cnt)
        {
            int i_row = 0;
            int i_col = 0;

            int i_start_row = 0;
            int i_start_col = 0;

            if (obj is FarPoint.Win.Spread.FpSpread)
            {
                //FarPoint.Win.Spread.SheetView spdList = ((FarPoint.Win.Spread.FpSpread)obj).ActiveSheet;
            }
            else
            {
                return false;
            }

            if (ExcelName.Length == 0)
            {
                return false;
            }

            if (ExcelName.Substring(ExcelName.Length - 4) != "xlsx" && ExcelName.Substring(ExcelName.Length - 3) != "xls")
            {
                return false;
            }

            FarPoint.Win.Spread.SheetView spdList = ((FarPoint.Win.Spread.FpSpread)obj).ActiveSheet;

            spdList.RowCount = 0;

            Excel.Application xlApp1 = new Excel.Application();
            Excel.Workbooks xlWKBooks1 = xlApp1.Workbooks;
            Excel.Workbook xlWKBook1 = xlWKBooks1.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet xlSheet1 = (Excel.Worksheet)xlWKBook1.ActiveSheet;

            xlApp1.DisplayAlerts = false;
            xlApp1.AlertBeforeOverwriting = false;

            try
            {

                xlWKBooks1 = xlApp1.Workbooks;
                xlWKBook1 = xlApp1.Workbooks.Open(ExcelName,
                                                  0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, true, true);

                xlSheet1 = (Excel.Worksheet)xlWKBook1.Worksheets[1];

                Excel.Range range = null;
                range = xlSheet1.UsedRange;//get_Range(xlSheet1.Cells[1, 1], xlSheet1.Cells[1, 1]);

                object[,] valueArray = (object[,])range.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                i_row = range.Rows.Count;
                i_col = range.Columns.Count;

                for (i_start_row = Excel_Start_Row; i_start_row <= i_row; i_start_row++)
                {
                    if (i_start_row - Excel_Start_Row + Spread_Start_Row >= spdList.RowCount)
                    {
                        spdList.RowCount++;
                    }

                    for (i_start_col = Excel_Start_Col; i_start_col <= i_col; i_start_col++)
                    {
                        if (i_start_col - Excel_Start_Col + Spread_Start_Col >= spdList.ColumnCount)
                        {
                            if (i_Col_Cnt != 0 && i_Col_Cnt == spdList.ColumnCount)
                            {
                                break;
                            }
                            spdList.ColumnCount++;
                        }
                        spdList.Cells[i_start_row - Excel_Start_Row + Spread_Start_Row, i_start_col - Excel_Start_Col + Spread_Start_Col].Value = Convert.ToString(valueArray[i_start_row, i_start_col]);
                    }
                }

                if (removeHeaderColumn)
                {
                    spdList.RemoveRows(0, 1);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWKBook1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWKBooks1);

                xlApp1.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp1);

                GC.Collect();

                xlSheet1 = null;
                xlWKBook1 = null;
                xlWKBooks1 = null;
                xlApp1 = null;

            }
        }

        public static bool DataTableToExcel(System.Data.DataTable dt, string ExcelName)
        {

            Excel.Application xlApp1 = new Excel.Application();
            Excel.Workbooks xlWKBooks1 = xlApp1.Workbooks;
            Excel.Workbook xlWKBook1 = xlWKBooks1.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet xlSheet1 = (Excel.Worksheet)xlWKBook1.ActiveSheet;
            Excel.Range oRange;

            xlApp1.DisplayAlerts = false;
            xlApp1.AlertBeforeOverwriting = false;

            try
            {
                // Check for data
                if (dt == null || dt.Rows.Count < 1)
                    return false;

                // Check for file name
                if (ExcelName.Length == 0)
                {
                    return false;
                }

                int iRow = 1;
                // Headers 
                for (int i = 0; i < dt.Columns.Count; i++)
                    xlSheet1.Cells[iRow, i + 1] = dt.Columns[i].ColumnName;

                // Inserting datas
                iRow++;
                for (int rowNo = 0; rowNo < dt.Rows.Count; rowNo++)
                {
                    // In each row
                    for (int colNo = 0; colNo < dt.Columns.Count; colNo++)
                    {

                        // In each column
                        xlSheet1.Cells[iRow, colNo + 1] = dt.Rows[rowNo][colNo].ToString();
                    }
                    // Moving to next row
                    iRow++;
                }

                // Range of the excel sheet
                oRange = xlSheet1.get_Range("A1", "IV1");
                oRange.EntireColumn.AutoFit();

                xlApp1.UserControl = false;

                // to save the excel sheet....
                xlWKBook1.SaveAs(
                ExcelName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWKBook1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWKBooks1);

                xlApp1.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp1);

                GC.Collect();

                xlSheet1 = null;
                xlWKBook1 = null;
                xlWKBooks1 = null;
                xlApp1 = null;
            }
        }


	}
}