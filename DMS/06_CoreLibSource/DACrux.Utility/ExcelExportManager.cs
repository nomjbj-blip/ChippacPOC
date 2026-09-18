using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;
using DACrux.Base;
using FarPoint.Win.Spread;

namespace DACrux.Utility
{
    // Excel Export Manager 2019.08.04 Taihi,Kim.
    public static class ExcelExportManager
    {
        // 별도로 추가할 Sheet를 나타냅니다.
        private static Form _worksheetAddForm = new Form();

        public static void Export(ExcelExportArgs e)
        {
            if (e.SheetList.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fileName;

            _worksheetAddForm = new Form();

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.RestoreDirectory = true;
                dlg.Filter = "Excel file|*.xlsx";
                dlg.DefaultExt = ".xlsx";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                fileName = dlg.FileName;
            }

            using (ExcelExportManager_Status status = new ExcelExportManager_Status())
            {
                status.TotalCount = e.SheetList.GetTotalItemCount();
                status.Show();

                using (var wb = new XLWorkbook())
                {
                    for (int i = 0; i < e.SheetList.Count; i++)
                    {
                        ExcelSheet sheet = e.SheetList[i];
                        var ws = wb.Worksheets.Add(sheet.SheetName);

                        int rowIndex = 1;

                        foreach (object obj in sheet.ItemList)
                        {
                            status.CurrentCount++;

                            if (obj is DataTable[])
                                AppendData(ws, ref rowIndex, obj as DataTable[]);
                            else if (obj is Control[])
                                AppendData(ws, ref rowIndex, obj as Control[]);
                            else if (obj is Image[])
                                AppendData(ws, ref rowIndex, obj as Image[], sheet.ImageScale);
                            else if (obj is string[])
                                AppendData(ws, ref rowIndex, obj as string[], sheet.ImageScale);
                            else if (obj is TextObject[])
                                AppendData(ws, ref rowIndex, obj as TextObject[]);
                            else if (obj == ExcelSheet.EmptyItem)
                                rowIndex++;
                            else
                                throw new Exception("Unknown types : " + obj.ToString());
                        }
                    }

                    // 추가로 Append 할 Sheet 데이터가 있는 경우 처리
                    if (_worksheetAddForm.Controls.Count > 0)
                    {
                        foreach (Control ctl in _worksheetAddForm.Controls)
                        {
                            DataGridView dgv = ctl as DataGridView;

                            if (dgv == null)
                                return;

                            var ws = wb.Worksheets.Add(GetUniqueName(wb));

                            //Sheet 이름을 Control 의 이름으로 넣어 준다.
                            if (!string.IsNullOrEmpty(dgv.Name))
                            {
                                //동일한 이름이 있을 경우 변경한다.
                                if (wb.Worksheets.Contains(dgv.Name))
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        string strTabName = string.Format("{0}({1})", dgv.Name, i);
                                        if (!wb.Worksheets.Contains(strTabName))
                                        {
                                            ws.Name = strTabName;
                                            break;
                                        }
                                    }
                                }
                                else
                                    ws.Name = dgv.Name;
                            }
                               
                            int rowIndex = 1;
                            AppendData(ws, rowIndex, dgv);
                        }

                        for (int i = 0; i < _worksheetAddForm.Controls.Count; i++)
                            _worksheetAddForm.Controls[i].Dispose();

                        _worksheetAddForm.Controls.Clear();
                    }

                    wb.SaveAs(fileName);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            Application.DoEvents();

            if (MessageBox.Show("파일을 여시겠습니까?", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                System.Diagnostics.Process.Start(fileName);
        }

        private static string GetUniqueName(XLWorkbook wb)
        {
            int i = 1;

            while (true)
            {
                string name = String.Format("Sheet{0}", i++);
                bool exists = false;

                foreach (var sheet in wb.Worksheets)
                {
                    if (sheet.Name == name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    return name;
            }
        }

        private static void AppendData(IXLWorksheet ws, ref int rowIndex, DataTable[] dtArr)
        {
            int col = 1;
            int maxRowCount = 0;

            foreach (DataTable dt in dtArr)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return;

                ws.Cell(rowIndex, col).InsertTable(dt);

                col += dt.Columns.Count + 1;
                maxRowCount = Math.Max(dt.Rows.Count, maxRowCount);
            }

            rowIndex += maxRowCount + 1;
        }

        private static void AppendData(IXLWorksheet ws, ref int rowIndex, Control[] ctlArr)
        {
            int col = 1;
            int maxRowCount = 0;

            foreach (Control ctl in ctlArr)
            {
                if (ctl is FpSpread)
                {
                    FpSpread spread = ctl as FpSpread;
                    AppendData(ws, rowIndex, ref col, spread.ActiveSheet);
                    int rowCount = spread.ActiveSheet.RowCount + spread.ActiveSheet.ColumnHeader.RowCount;
                    maxRowCount = Math.Max(rowCount, maxRowCount);
                }
                else
                {
                    int height = AppendData(ws, rowIndex, ref col, ctl);
                    int rowCount = (int)Math.Ceiling(GetRowHeight(height) / ws.Row(rowIndex).Height);
                    maxRowCount = Math.Max(rowCount, maxRowCount);
                }

                // IDataGrid 를 구현하였으면 새로운 ExcelSheet에 데이터를 표현한다.
                if (ctl is IDataGrid)
                {
                    DataGridView dgv = (ctl as IDataGrid).ToDataGridView();

                    if (dgv != null)
                    {
                        if (dgv.Parent != null)
                            dgv.Parent.Controls.Remove(dgv);

                        _worksheetAddForm.Controls.Add(dgv);
                    }
                }
            }

            rowIndex += maxRowCount + 1;
        }

        private static void AppendData(IXLWorksheet ws, int rowIndex, ref int colIndex, SheetView sheet)
        {
            int row = rowIndex;
            int col = colIndex;

            ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;

            row += sheet.ColumnHeader.RowCount;

            for (int r = 0; r < sheet.RowCount; r++)
            {
                // row header
                var cell = GetDefaultHeaderCell(ws, row + r, col);
                cell.Value = sheet.RowHeader.Cells[r, 0].Value;

                if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                    cell.Value = sheet.RowHeader.Rows[r].Label;
            }

            col++;
            row = rowIndex;

            for (int c = 0; c < sheet.ColumnCount; c++)
            {
                // column header
                for (int r = 0; r < sheet.ColumnHeader.RowCount; r++)
                {
                    var cell = GetDefaultHeaderCell(ws, row + r, col + c);
                    cell.Value = sheet.ColumnHeader.Cells[r, c].Value;

                    if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()) && r == 0)
                        cell.Value = sheet.ColumnHeader.Columns[c].Label;
                }

                // set column width
                ws.Column(col + c).Width = GetColumnWidth(sheet.Columns[c].Width);
            }

            row += sheet.ColumnHeader.RowCount;

            for (int r = 0; r < sheet.RowCount; r++)
            {
                // set row height
                ws.Row(row + r).Height = GetRowHeight(sheet.Rows[r].Height);

                for (int c = 0; c < sheet.ColumnCount; c++)
                {
                    var fpCell = sheet.Cells[r, c];

                    // set data
                    var cell = ws.Cell(row + r, col + c);
                    cell.Value = fpCell.Value;
                    cell.Style.Fill.BackgroundColor = XLColor.FromColor(fpCell.BackColor);

                    // background image
                    var generalCell = fpCell.CellType as FarPoint.Win.Spread.CellType.GeneralCellType;

                    if (generalCell != null && generalCell.BackgroundImage != null && generalCell.BackgroundImage.Image != null)
                    {
                        var picture = ws.AddPicture(ImageToStream(generalCell.BackgroundImage.Image));
                        picture.MoveTo(ws.Cell(row + r, col + c));
                    }
                }
            }

            colIndex = col + sheet.ColumnCount + 1;
        }

        private static void AppendData(IXLWorksheet ws, int rowIndex, DataGridView GridView)
        {
            int row = rowIndex;
            int col = 1;

            ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;

            row += 1;

            for (int r = 0; r < GridView.RowCount; r++)
            {
                // row header
                var cell = GetDefaultHeaderCell(ws, row + r, col);
                cell.Value = GridView.Rows[r].HeaderCell.Value;

                //if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                //    cell.Value = sheet.RowHeader.Rows[r].Label;
            }

            col++;
            row = rowIndex;

            for (int c = 0; c < GridView.ColumnCount; c++)
            {
                var cell = GetDefaultHeaderCell(ws, row, col + c);
                cell.Value = GridView.Columns[c].HeaderText;

                ws.Column(col + c).Width = GetColumnWidth(GridView.Columns[c].Width);
            }

            row += 1;
            for (int r = 0; r < GridView.RowCount; r++)
            {
                // set row height
                ws.Row(row + r).Height = GetRowHeight(GridView.Rows[r].Height);

                for (int c = 0; c < GridView.ColumnCount; c++)
                {
                    var grCell = GridView.Rows[r].Cells[c];

                    // set data
                    var cell = ws.Cell(row + r, col + c);
                    cell.Value = grCell.Value;
                    cell.Style.Fill.BackgroundColor = XLColor.FromColor(grCell.Style.BackColor);
                }
            }

            rowIndex = row + GridView.RowCount + 1;
        }

        private static int AppendData(IXLWorksheet ws, int rowIndex, ref int colIndex, Control ctl)
        {
            Bitmap bmp = new Bitmap(ctl.Width, ctl.Height);
            ctl.DrawToBitmap(bmp, ctl.ClientRectangle);

            var picture = ws.AddPicture(ImageToStream(bmp));
            picture.MoveTo(ws.Cell(rowIndex, colIndex));

             colIndex += (int)Math.Ceiling(GetColumnWidth(bmp.Width) / ws.Column(rowIndex).Width);

            return bmp.Height;
        }

        private static void AppendData(IXLWorksheet ws, ref int rowIndex, Image[] imageArr, float imageScale)
        {
            List<Bitmap> bitmapList = new List<Bitmap>();

            foreach (Image image in imageArr)
            {
                if (image == null)
                    continue;

                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapList.Add(Image.FromStream(ms) as Bitmap);
            }

            AppendDataCore(ws, ref rowIndex, bitmapList, imageScale);
        }

        private static void AppendData(IXLWorksheet ws, ref int rowIndex, string[] imagePathArr, float imageScale)
        {
            int maxHeight = 0;
            List<Bitmap> bitmapList = new List<Bitmap>();

            foreach (string imagePath in imagePathArr)
            {
                if (String.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                    continue;

                try
                {
                    Bitmap bmp = Image.FromFile(imagePath) as Bitmap;
                    bitmapList.Add(bmp);

                    maxHeight = Math.Max(bmp.Height, maxHeight);
                }
                catch
                {
                    // 이미지 파일 오류 시 무시
                }
            }

            AppendDataCore(ws, ref rowIndex, bitmapList, imageScale);
        }

        private static void AppendDataCore(IXLWorksheet ws, ref int rowIndex, List<Bitmap> bitmapList, float imageScale)
        {
            if (bitmapList == null || bitmapList.Count == 0)
                return;

            int col = 1;
            int maxHeight = 0;

            foreach (Bitmap bitmap in bitmapList)
            {
                maxHeight = Math.Max(maxHeight, bitmap.Height);

                var picture = ws.AddPicture(bitmap);
                picture.MoveTo(ws.Cell(rowIndex, col++));
                picture.Scale(imageScale);
            }

            // 몇 row나 차지하는지 계산
            rowIndex += (int)Math.Ceiling(GetRowHeight(maxHeight) *imageScale / ws.Row(rowIndex).Height) + 1;
        }

        private static void AppendData(IXLWorksheet ws, ref int rowIndex, TextObject[] textObjectArr)
        {
            foreach (TextObject textObject in textObjectArr)
            {
                if (textObject == null)
                    continue;

                var cell = ws.Cell(rowIndex++, 1);

                cell.Value = textObject.Text;
                cell.Style.Font.Bold = textObject.Font.Bold;
                cell.Style.Font.FontName = textObject.Font.Name;
                cell.Style.Font.FontSize = textObject.Font.Size;
                cell.Style.Font.FontColor = XLColor.FromColor(textObject.FontColor);
            }
        }

        private static IXLCell GetDefaultHeaderCell(IXLWorksheet ws, int row, int col)
        {
            var cell = ws.Cell(row, col);
            cell.Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            return cell;
        }

        private static Stream ImageToStream(Image image)
        {
            if (image == null)
                return null;

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms;
        }

        private static double GetColumnWidth(double pixels)
        {
            var tempWidth = pixels * 0.14099;
            var correction = (tempWidth / 100) * -1.30;

            return tempWidth - correction;
        }

        private static double GetRowHeight(double pixels)
        {
            return pixels * 0.75;
        }
    }
}
