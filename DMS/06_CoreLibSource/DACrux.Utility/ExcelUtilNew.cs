using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Drawing;
using System.IO;

namespace DACrux.Utility
{
    /// <summary>
    /// 엑셀 파일을 저장하기 위한 클래스
    /// </summary>
    public class ExcelUtilNew
    {
        public static string TO_EXCEL_NORMAL = "Normal Mode(&N)";
        public static string TO_EXCEL_FAST = "Fast Mode(&F)";

        private List<Param> _list = new List<Param>();
        private ContextMenuStrip _ctxMenu = new ContextMenuStrip();
        private SaveMode _saveMode;
        private bool isUseSheetName;

        //Excel 탭 이름 설정 여부 세팅
        //true : Excel Export 시, SheetName을 사용합니다.
        //False : Excel Export 시, sheet + num 형식을 사용합니다.
        public bool IsUseSheetName
        {
            get { return isUseSheetName; }
            set { isUseSheetName = value; }
        }

        public ExcelUtilNew()
        {
            _ctxMenu.Items.Add(TO_EXCEL_NORMAL);
            _ctxMenu.Items.Add(TO_EXCEL_FAST);
            _ctxMenu.ItemClicked += new ToolStripItemClickedEventHandler(ctxMenu_ItemClicked);
        }

        public ExcelUtilNew(SaveMode saveMode)
        {
            _saveMode = saveMode;
        }

        /// <summary>
        /// 엑셀로 저장할 Spread를 추가합니다.
        /// </summary>
        public void Add(SheetView sheet)
        {
            Add(sheet, null);
        }

        /// <summary>
        /// 엑셀로 저장할 Spread를 추가합니다. 차트 혹은 내보낼 컨트롤이 있는 경우 매개변수로 전달 합니다.
        /// </summary>
        public void Add(SheetView sheet, params Control[] controls)
        {
            if (sheet == null || sheet.RowCount == 0)
                return;

            _list.Add(new Param() { Sheet = sheet, Controls = controls });
        }

        /// <summary>
        /// 추가된 리스트를 클리어 합니다.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        private void ctxMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string fileName = _ctxMenu.Tag as string;

            if (e.ClickedItem.Text == TO_EXCEL_NORMAL)
                ToExcel(fileName, isUseSheetName, SaveMode.Normal);
            else
                ToExcel(fileName, false, SaveMode.Fast);
        }

        public void ToExcel(string fileName)
        {
            if (_ctxMenu.Items.Count > 0)
            {
                _ctxMenu.Tag = fileName;
                _ctxMenu.Show(Control.MousePosition);
            }
            else
            {
                ToExcel(fileName, false, _saveMode);
            }
        }

        /// <summary>
        /// 엑셀 파일로 저장합니다.
        /// </summary>
        public void ToExcel(string fileName, bool useSheetName, SaveMode saveMode)
        {
            if (_list.Count == 0)
                return;

            FpSpread spread = new FpSpread();
            AppendNewSheet(spread, useSheetName);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel File|*.xlsx";

            if (!String.IsNullOrEmpty(fileName))
                dlg.FileName = fileName + "_";

            foreach (char ch in Path.GetInvalidFileNameChars())
                dlg.FileName = dlg.FileName.Replace(ch, 'A');

            dlg.FileName += DateTime.Now.ToString("yyyyMMddHHmmss");

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            bool saveSuccess = false;

            if (saveMode == SaveMode.Fast)
            {
                List<System.Data.DataTable> dtList = new List<System.Data.DataTable>();

                foreach (SheetView sheet in spread.Sheets)
                {
                    if (sheet.DataSource != null && sheet.DataSource is System.Data.DataTable)
                    {
                        dtList.Add(sheet.DataSource as System.Data.DataTable);
                    }
                    else //DataSource가 Null인 경우(Chart) 탭 생성을 위해 Dummy database 추가
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.TableName = "Chart";//Chart 형식인 경우 Table Name을 Chart로 표기한다.
                        dtList.Add(dt as System.Data.DataTable);
                    }
                }

                if (dtList.Count > 0)
                {
                    OpenXmlExcel.WriteFile(dlg.FileName, dtList.ToArray());
                }
                else // DataSource가 DataTable이 아닌 경우 FarPoint에서 지원하는 기능 이용
                {
                    saveSuccess = spread.SaveExcel(dlg.FileName,
                        FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders |
                        FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
                }
            }
            else //normal 모드.
            {
                saveSuccess = spread.SaveExcel(dlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveBothCustomRowAndColumnHeaders |
                    FarPoint.Excel.ExcelSaveFlags.SaveAlternatingRowStyles |
                    FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            }

            if (ExistsAppendObjects())
                AppendObjects(dlg.FileName);

            System.Diagnostics.Process.Start(dlg.FileName);
        }

        /// <summary>
        /// Spread의 Protect 속성값을 설정합니다.
        /// </summary>
        private void SetSheetProtect(bool protect)
        {
            foreach (Param param in _list)
                param.Sheet.Protect = protect;
        }

        private void AppendNewSheet(FpSpread spread, bool useSheetName)
        {
            int n = 0;

            foreach (Param param in _list)
            {
                SheetView sheet = param.Sheet;

                if (!useSheetName)
                    sheet.SheetName = "Sheet" + (++n).ToString();

                spread.Sheets.Add(sheet);
            }

            n = 0;
            foreach (Param param in _list)
            {
                if (param.Controls == null || param.Controls.Length == 0)
                    continue;

                bool exists = false;

                foreach (Control ctl in param.Controls)
                {
                    if (ctl != null)
                        exists = true;
                }

                if (!exists)
                    continue;

                SheetView sheet = new SheetView();
                sheet.SheetName = "Chart" + (++n).ToString();
                sheet.Protect = false;
                spread.Sheets.Add(sheet);
            }
        }

        /// <summary>
        /// 차트, 컨트롤 등의 추가 Object가 존재하는지 가져옵니다.
        /// </summary>
        private bool ExistsAppendObjects()
        {
            foreach (Param p in _list)
            {
                if (p.Controls != null && p.Controls.Length > 0)
                {
                    foreach (Control ctl in p.Controls)
                    {
                        if (ctl != null)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Object를 추가합니다. 방식은 Excel objec를 이용하여 저장된 파일을 열어서 append하는 방식입니다.
        /// </summary>
        private void AppendObjects(string fileName)
        {
            Microsoft.Office.Interop.Excel._Workbook workbook = null;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            Microsoft.Office.Interop.Excel.Application application = null;

            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();
                workbook = application.Workbooks.Open(fileName, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing) as Microsoft.Office.Interop.Excel.Workbook;

                if (workbook == null)
                    return;

                int n = 0;

                foreach (Param param in _list)
                {
                    if (param.Controls == null || param.Controls.Length == 0)
                        continue;

                    worksheet = workbook.Worksheets[_list.Count + ++n] as Microsoft.Office.Interop.Excel._Worksheet;

                    if (worksheet == null)
                        continue;

                    for (int j = 0; j < param.Controls.Length; j++)
                    {
                        Control ctl = param.Controls[j];

                        if (ctl == null)
                            continue;

                        string imgFile = GetImageFile(ctl);

                        if (String.IsNullOrEmpty(imgFile))
                            continue;

                        worksheet.Shapes.AddPicture(imgFile,
                            Microsoft.Office.Core.MsoTriState.msoFalse,
                            Microsoft.Office.Core.MsoTriState.msoCTrue,
                            100 + j * 20, 100 + j * 50, ctl.Width, ctl.Height);

                        File.Delete(imgFile);
                    }
                }

                workbook.Save();
                workbook.Close();
            }
            finally
            {
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                }

                application = null;

                GC.Collect();                                                                                                                             //가비지 컬렉터
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 컨트롤을 이미지 파일로 저장한 후 경로를 가져옵니다.
        /// </summary>
        private string GetImageFile(Control ctl)
        {
            string imgFile = Path.GetTempFileName();
            Bitmap bmp = new Bitmap(ctl.Width, ctl.Height);
            ctl.DrawToBitmap(bmp, ctl.ClientRectangle);
            bmp.Save(imgFile);
            return imgFile;
        }

        class Param
        {
            public SheetView Sheet { get; set; }
            public Control[] Controls { get; set; }
        }
    }

    public enum SaveMode
    {
        Normal,
        Fast
    }
}
