using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Data;

namespace DACrux.Utility
{
    public static class OpenXmlExcel
    {
        #region 멤버 변수

        private static readonly OpenXmlAttribute[] DataAttribute;
        private static readonly OpenXmlAttribute[] HeaderAttribute;

        #endregion

        #region Private 메서드

        static OpenXmlExcel()
        {
            DataAttribute = new OpenXmlAttribute[]
            {
                new OpenXmlAttribute("t", null, "str"),
                new OpenXmlAttribute("s", null, "1")
            };

            HeaderAttribute = new OpenXmlAttribute[]
            {
                new OpenXmlAttribute("t", null, "str"),
                new OpenXmlAttribute("s", null, "2")
            };
        }

        private static Stylesheet GetStylesheet()
        {
            Fonts fonts = new Fonts(
                    new Font(new FontSize() { Val = 10 }), // Index 0 - default
                    new Font(new FontSize() { Val = 10 }, new Bold(), new Color() { Rgb = "FFFFFF" }) // Index 1 - header
                );

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border(
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat() { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            return new Stylesheet(fonts, fills, borders, cellFormats);
        }

        #endregion

        #region Public 메서드

        /// <summary>
        /// DataTable을 Excel로 저장합니다.
        /// </summary>
        public static void WriteFile(string fileName, params DataTable[] dtArray)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = doc.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                workbookPart.AddNewPart<WorkbookStylesPart>();
                workbookPart.WorkbookStylesPart.Stylesheet = GetStylesheet();
                workbookPart.WorkbookStylesPart.Stylesheet.Save();

                Sheets sheets = workbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                for (int i = 0; i < dtArray.Length; i++)
                {
                    DataTable dt = dtArray[i];

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                    Sheet sheet = new Sheet();
                    sheet.Id = workbookPart.GetIdOfPart(worksheetPart);
                    sheet.SheetId = (uint)(i + 1);

                    //기존 로직의 경우 Sheet가 2개이상 존재하면 Sheet Name이 "dt"로 중복되어 오류 발생함.
                    //table Name이 Chart인 경우에만 Chart{0}형식으로 표기 이외는 Sheet{0}로 표기 2018.08.16 서영화
                    sheet.Name = dt.TableName == "Chart" ? String.Format("Chart{0}", i + 1) : String.Format("Sheet{0}", i + 1);
                    sheets.Append(sheet);

                    using (OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart))
                    {
                        writer.WriteStartElement(new Worksheet());
                        writer.WriteStartElement(new SheetData());

                        Row r = new Row();
                        Cell c = new Cell();

                        // Write header
                        writer.WriteStartElement(r);
                        foreach (System.Data.DataColumn col in dt.Columns)
                        {
                            writer.WriteStartElement(c, HeaderAttribute);
                            writer.WriteElement(new CellValue(col.ColumnName));
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();

                        // Write data
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            writer.WriteStartElement(r);

                            foreach (System.Data.DataColumn col in dt.Columns)
                            {
                                writer.WriteStartElement(c, DataAttribute);
                                writer.WriteElement(new CellValue(row[col].ToString()));
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                }
            }
        }

        /// <summary>
        /// Excel 파일을 읽어들입니다.
        /// </summary>
        public static DataTable ReadFile(string fileName, bool isFirstRowIsHeader = false)
        {
            DataTable dt = new DataTable();

            using (var doc = SpreadsheetDocument.Open(fileName, false))
            {
                var sheet = doc.WorkbookPart.Workbook.Sheets.FirstChild as Sheet;
                var dic = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
                var sheetPart = doc.WorkbookPart.GetPartById(sheet.Id) as WorksheetPart;

                var rows = sheetPart.Worksheet.Descendants<Row>();

                foreach (var row in rows)
                {
                    if (isFirstRowIsHeader && row.RowIndex.Value == 1)
                    {
                        var cells = row.Elements<Cell>();

                        foreach (Cell cell in cells)
                        {
                            var colunmName = dic.ElementAt(Int32.Parse(cell.CellValue.Text)).InnerText;
                            //Headers.Add(colunmName);
                            dt.Columns.Add(colunmName);
                        }
                    }
                    else
                    {
                        var cells = row.Elements<Cell>();
                        string[] arr = new string[cells.Count()];
                        int i = 0;

                        foreach (Cell cell in cells)
                        {
                            arr[i++] = GetCellValue(doc, cell);
                        }

                        // Append Column & Data
                        while (arr.Length > dt.Columns.Count)
                            dt.Columns.Add();

                        dt.Rows.Add(arr);
                    }
                }
            }

            return dt;
        }

        private static string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            int oaDate;
            string value = null;

            if (cell.CellValue != null)
                value = cell.CellValue.InnerText;

            if (cell.DataType != null)
            {
                if (cell.DataType.Value == CellValues.SharedString)
                    return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(DACrux.Base.Convert.intParse(value)).InnerText;
                else if (cell.DataType == CellValues.Boolean)
                    value = value == "0" ? Boolean.FalseString : Boolean.TrueString;
            }
            else if (!String.IsNullOrEmpty(value) && cell.StyleIndex == 2 && Int32.TryParse(value, out oaDate))
            {
                value = DateTime.FromOADate(oaDate).ToString("yyyy-MM-dd");
            }

            return value;
        }

        public static DataTable FirstRowToColumnName(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt;

            object[] arr = dt.Rows[0].ItemArray;

            for (int i = 0; i < dt.Columns.Count; i++)
                dt.Columns[i].ColumnName = arr[i].ToString();

            dt.Rows.RemoveAt(0);

            return dt;
        }

        #endregion
    }
}
