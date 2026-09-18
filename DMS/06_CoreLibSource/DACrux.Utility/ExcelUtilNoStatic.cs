using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using ChartFX.WinForms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Office;
using FarPoint.Win.Spread;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : ExcelUtil<br/>
    /// Summary    : Excel Handling Class (NoStatic)<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class ExcelUtilNoStatic
	{
		#region Class Member

		Microsoft.Office.Interop.Excel._Workbook workbook = null;
		Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
		Microsoft.Office.Interop.Excel.Application application = null;

		#endregion

        #region Excel Handling

        /// <summary>
        /// Export Charts & Spread to Excel
        /// </summary>
        /// <param name="fpSpread">Spread</param>
        /// <param name="charts">Chart FX</param>
        public void ToExcel(FarPoint.Win.Spread.FpSpread fpSpread, System.Windows.Forms.DataVisualization.Charting.Chart [] charts, int colCount, int width, int height)
        {
            try
            {
                string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
                string excelFile = file;
                if (System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
                fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

                OpenExcel(excelFile, charts, colCount, width, height, 0 );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();   
                GC.WaitForPendingFinalizers();
            }
        }

        private void OpenExcel(string excelFile, System.Windows.Forms.DataVisualization.Charting.Chart[] charts, int colCount, int width, int height, int test)
        {
            test = 0;

            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();

                application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

                workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
                worksheet.Activate();

                worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


                //worksheet.get_Range(worksheet.Cells[0, 0], worksheet.Cells[0,0]).Select();
                //worksheet.Rows.InsertIndent(3);

                application.Visible = true;

                string chartFile;
                int row = 0;
                int col = 0;

                Rectangle rect = new Rectangle(0, 0, width, height);
              
                for (int i = 0; i < charts.Length; i++)
                {
                    chartFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, string.Format("temp_chart_{0}_{1}.bmp", DateTime.Now.ToString("yyyyMMddHHmmss"), i));
                    if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);

                    charts[i].SaveImage(chartFile, ChartImageFormat.Bmp);

                    col = (i % colCount);
                    row = (i / colCount);
                    worksheet.Shapes.AddPicture(chartFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, col * (width + 10), row * (height + 10), width, height);

                    System.IO.File.Delete(chartFile);
                }
            }
            catch (Exception ex)
            {
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                }
                application = null;

                throw ex;
            }
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
        }

		/// <summary>
		/// Export Chart & Spread to Excel
		/// </summary>
		/// <param name="fpSpread">Spread</param>
		/// <param name="chart">Chart FX</param>
		public void ToExcel(FpSpread fpSpread, Chart chart)
		{
			try
			{
				string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp"+ DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
				string excelFile = file;
				if(System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
				fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

				string chartTempFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "temp_chart.bmp");
				if(System.IO.File.Exists(chartTempFile)) System.IO.File.Delete(chartTempFile);

                chart.SaveImage(chartTempFile, ChartImageFormat.Bmp);
                //chartFX
                //chart.Export(ChartFX.WinForms.FileFormat.Bitmap, chartTempFile);

				int row = fpSpread.ActiveSheet.ColumnHeader.RowCount + fpSpread.ActiveSheet.Rows.Count + 1;
				OpenExcel(excelFile, chartTempFile, row);
			}
			catch(Exception ex)
			{
				throw ex;
			}
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
		}

		/// <summary>
        /// Export Spread to Excel
		/// </summary>
		/// <param name="fpSpread">Spread</param>
		public void ToExcel(FarPoint.Win.Spread.FpSpread fpSpread)
		{
			try
			{
				string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp"+ DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
				string excelFile = file;
				if(System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
				fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

				OpenExcel(excelFile);
			}
			catch(Exception ex)
			{
				throw ex;
			}
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
		}

		/// <summary>
		/// Export Charts to Excel
		/// </summary>
		/// <param name="chart">Chart FX</param>
		/// <param name="colCnt">Column Count</param>
		/// <param name="imageWidth">Width</param>
		/// <param name="imageHeight">Height</param>
		public void ToExcel (System.Windows.Forms.DataVisualization.Charting.Chart[] chart, int colCnt, int imageWidth, int imageHeight)
		{
			try
			{
				string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp"+ DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
				string excelFile = file;
				if(System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
				FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();
				FarPoint.Win.Spread.SheetView sheet = new FarPoint.Win.Spread.SheetView();
				spread.Sheets.Add(sheet);
				spread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);
				sheet.Dispose();
				spread.Dispose();

				OpenExcel(excelFile, chart, colCnt, imageWidth, imageHeight);
			}
			catch(Exception ex)
			{
				throw ex;
			}
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
		}

        /// <summary>
        /// Open Excel File by selected file & Chart Set
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="chart">Chart FX</param>
        /// <param name="colCnt">Column Count</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
		void OpenExcel(string excelFile, System.Windows.Forms.DataVisualization.Charting.Chart[] chart, int colCnt, int width, int height)
		{
            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();

                application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

                workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
                worksheet.Activate();

                worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                worksheet.Rows.RowHeight = height * 375 / 500;
                worksheet.Columns.ColumnWidth = width * 85 / 500;

                application.Visible = true;

                Microsoft.Office.Interop.Excel.Pictures oPic;
                string chartFile;
                int row = 0;
                int col = 0;
                for (int a = 0; a < chart.Length; a++)
                {
                    chartFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "temp_chart" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".bmp");
                    if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);

                    chart[a].SaveImage(chartFile, ChartImageFormat.Bmp);
                    //chartFX
                    //chart[a].Export(ChartFX.WinForms.FileFormat.Bitmap, chartFile);

                    col = (a % colCnt) + 1;
                    row = (a / colCnt) + 1;

                    Microsoft.Office.Interop.Excel.Range c1 = worksheet.Cells[row, col];
                    Microsoft.Office.Interop.Excel.Range c2 = worksheet.Cells[row, col];

                    worksheet.get_Range(c1, c2).Select();
                    oPic = (Microsoft.Office.Interop.Excel.Pictures)worksheet.Pictures(System.Reflection.Missing.Value);
                    oPic.Insert(chartFile, System.Reflection.Missing.Value);
                }
            }
            catch (Exception ex)
            {
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                }
                application = null;

                throw ex;
            }
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
		}

        /// <summary>
        /// Open Excel with File & Chart File
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="chartFile">Chart File</param>
        /// <param name="row">Row No</param>
		void OpenExcel(string excelFile, string chartFile, int row)
		{
			try
			{
				application = new Microsoft.Office.Interop.Excel.Application();

				application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

				workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

				worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
				worksheet.Activate();

				worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

				application.Visible = true;

				Microsoft.Office.Interop.Excel.Pictures oPic;

                Microsoft.Office.Interop.Excel.Range c1 = worksheet.Cells[row, 1];
                Microsoft.Office.Interop.Excel.Range c2 = worksheet.Cells[row, 1];

                worksheet.get_Range(c1, c2).Select();
				//worksheet.get_Range(worksheet.Cells[row,1],worksheet.Cells[row,1]).Select();
				oPic = (Microsoft.Office.Interop.Excel.Pictures)worksheet.Pictures(System.Reflection.Missing.Value);
				oPic.Insert(chartFile, System.Reflection.Missing.Value);
			}
			catch(Exception ex)
			{
				if(application != null)
				{
					application.Quit();
					System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
				}
				application = null;
				throw ex;
			}
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
		}

		/// <summary>
		/// Open Excel File
		/// </summary>
		/// <param name="excelFile">Excel File</param>
		void OpenExcel(string excelFile)
		{
			try
			{
				application = new Microsoft.Office.Interop.Excel.Application();

				application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

				workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

				worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
				worksheet.Activate();

				worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

				application.Visible = true;
			}
			catch(Exception ex)
			{
				if(application != null)
				{
					application.Quit();
					System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
				}
				application = null;
				throw ex;
			}
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }

		}

		/// <summary>
        /// Release instance memory when closed Excel. Using NAR Method
		/// </summary>
		/// <param name="Wb">Work Book</param>
		/// <param name="Wn">Excel Window</param>
		void Application_ExcelDeactivate(Microsoft.Office.Interop.Excel.Workbook Wb, Microsoft.Office.Interop.Excel.Window Wn)
		{
			try
			{
				this.application.Quit();
                this.NAR(this.worksheet);
				this.NAR(this.workbook);
				this.NAR(this.application);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				GC.Collect();                                                                                                                             //가비지 컬렉터
				GC.WaitForPendingFinalizers();
			}
		}

        //2012-04-26-박진규
        //파라미터 하나 더 추가
        public void ToExcel(FarPoint.Win.Spread.FpSpread fpSpread, System.Windows.Forms.DataVisualization.Charting.Chart[] charts, int colCount, int width, int height, int test)
        {
            try
            {
                //사용자가 지정한 경로를 정해주어야 함
                string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");

                string excelFile = file;

                //파일 덮어 쓸 때, 기존의 파일은 지워짐
                if (System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
                fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

                OpenExcel(excelFile, charts, colCount, width, height, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        //2012-04-27-박진규
        public void OpenExcel(string excelFile, System.Windows.Forms.DataVisualization.Charting.Chart[] charts, int colCount, int width, int height, int test, int test1)
        {
            test = 0;
            test1 = 0;

            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();

                application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

                workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
                worksheet.Activate();

                worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


                //worksheet.get_Range(worksheet.Cells[0, 0], worksheet.Cells[0,0]).Select();
                //worksheet.Rows.InsertIndent(3);

                application.Visible = true;

                string chartFile;
                int row = 0;
                int col = 0;

                Rectangle rect = new Rectangle(0, 0, width, height);

                for (int i = 0; i < charts.Length; i++)
                {
                    chartFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, string.Format("temp_chart_{0}_{1}.bmp", DateTime.Now.ToString("yyyyMMddHHmmss"), i));
                    if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);

                    charts[i].SaveImage(chartFile, ChartImageFormat.Bmp);

                    col = (i % colCount);
                    row = (i / colCount);
                    worksheet.Shapes.AddPicture(chartFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, col * (width + 10), row * (height + 10), width, height);

                    System.IO.File.Delete(chartFile);
                }
            }
            catch (Exception ex)
            {
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                }
                application = null;

                throw ex;
            }
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
        }

        //void OpenExcel(string excelFile, SPCHistogram.SPCHistogram[] chart, int colCnt, int width, int height, string[] filelist)
        //{
        //    try
        //    {
        //        application = new Microsoft.Office.Interop.Excel.Application();

        //        //Excel을 사용자가 닫으면 그 이벤트를 받기 위해 핸들러를 걸어줍니다.
        //        //핸들러(메쏘드) 이름은 Application_ExcelDeactivate 입니다.
        //        application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

        //        workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
        //            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

        //        worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
        //        worksheet.Activate();

        //        worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        //        worksheet.Rows.RowHeight = height * 375 / 500;
        //        worksheet.Columns.ColumnWidth = width * 85 / 500;

        //        //사용자에게 Excel 공개
        //        application.Visible = true;

        //        //여기서 ListView나 DataSet을 읽어 Excel을 채웁니다.
        //        Microsoft.Office.Interop.Excel.Pictures oPic;
        //        string chartFile;
        //        int row = 0;
        //        int col = 0;
        //        for (int a = 0; a < chart.Length; a++)
        //        {
        //            chartFile = filelist[a];
        //            //if(System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);

        //            chart[0].SaveImage(chartFile);

        //            col = (a % colCnt) + 1;
        //            row = (a / colCnt) + 1;

        //            worksheet.get_Range(worksheet.Cells[row, col], worksheet.Cells[row, col]).Select();
        //            oPic = (Microsoft.Office.Interop.Excel.Pictures)worksheet.Pictures(System.Reflection.Missing.Value);
        //            oPic.Insert(chartFile, System.Reflection.Missing.Value);
        //            // if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (application != null)
        //        {
        //            application.Quit();
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
        //        }
        //        application = null;
        //        GC.Collect();                                                                                                                             //가비지 컬렉터
        //        GC.WaitForPendingFinalizers();
        //        throw ex;
        //    }
        //}
        
        // 박진규 ToExcel 오버로딩
        public void ToExcel(FarPoint.Win.Spread.FpSpread fpSpread, System.Windows.Forms.DataVisualization.Charting.Chart[] charts, int colCount, int width, int height, string FullPath)
        {
            try
            {
                string file = string.Format(@"{0}", FullPath);
                string excelFile = file;

                //파일 덮어 쓸 때, 기존의 파일은 지워짐
                if (System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);

                //그림이 들어간 것이 저장
                fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

                application = new Microsoft.Office.Interop.Excel.Application();

                application.WindowDeactivate += new Microsoft.Office.Interop.Excel.AppEvents_WindowDeactivateEventHandler(Application_ExcelDeactivate);

                workbook = (Microsoft.Office.Interop.Excel.Workbook)(application.Workbooks.Open(excelFile, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing));

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
                worksheet.Activate();

                worksheet.Protect(Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //worksheet.get_Range(worksheet.Cells[0, 0], worksheet.Cells[0,0]).Select();
                //worksheet.Rows.InsertIndent(3);

                //엑셀 파일이 보이고,
                application.Visible = true;

                string chartFile;
                int row = 0;
                int col = 0;

                //Rectangle rect = new Rectangle(0, 0, width, height);

                for (int i = 0; i < charts.Length; i++)
                {
                    chartFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, string.Format("temp_chart_{0}_{1}.bmp", DateTime.Now.ToString("yyyyMMddHHmmss"), i));
                    if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);

                    charts[i].SaveImage(chartFile, ChartImageFormat.Bmp);

                    col = (i % colCount);
                    row = (i / colCount);

                    //그림이 들어간다.
                    worksheet.Shapes.AddPicture(chartFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, col * (width + 10), row * (height + 10), width, height);

                    System.IO.File.Delete(chartFile);
                }

                //OpenExcel(excelFile, charts, colCount, width, height, 0);
                //OpenExcel(excelFile, chartTempFile, row);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        

        public void ToExcel(FarPoint.Win.Spread.FpSpread fpSpread, string FullPath)
        {
            try
            {
                string file = string.Format(@"{0}", FullPath);
                string excelFile = file;
                if (System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
                fpSpread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);

                OpenExcel(excelFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
        }

        //public void ToExcel(SPCHistogram.SPCHistogram[] chart, int colCnt, int imageWidth, int imageHeight)
        //{
        //    try
        //    {
        //        string file = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, "excel_temp.xls");
        //        string excelFile = file;
        //        if (System.IO.File.Exists(excelFile)) System.IO.File.Delete(excelFile);
        //        FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();
        //        FarPoint.Win.Spread.SheetView sheet = new FarPoint.Win.Spread.SheetView();
        //        spread.Sheets.Add(sheet);
        //        spread.SaveExcel(excelFile, FarPoint.Win.Spread.Model.IncludeHeaders.BothCustomOnly);
        //        sheet.Dispose();
        //        spread.Dispose();
        //        string[] filelist = new string[chart.Length];
        //        for (int k = 0; k < chart.Length; k++)
        //        {
        //            string chartFile = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, k + "temp_chart.bmp");
        //            if (System.IO.File.Exists(chartFile)) System.IO.File.Delete(chartFile);
        //            filelist[k] = chartFile;
        //            chart[k].SaveImage(chartFile);
        //        }
        //        OpenExcel(excelFile, chart, colCnt, imageWidth, imageHeight, filelist);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
		#endregion

		#region NAR
		/// <summary>
        /// Method to release COM object
		/// </summary>
        /// <param name="obj">COM object</param>
		private void NAR(object obj)
		{
			try
			{
				if(obj != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
			}
			catch(Exception ex) 
			{
				throw ex;
			}
			finally
			{
				obj = null;
			}
		}
		#endregion
	}
}
