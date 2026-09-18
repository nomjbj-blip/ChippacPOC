using System;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
//using ChartFX.WinForms;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : Component<br/>
    /// Summary    : Component Utility Class<br/>
    /// Author     : Miracom Hyungsuk, Yang<br/>
    /// First Date : 2009-11-17<br/> 
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public static class Component
    {
        #region [ FarPoint Spread ]

        #region InitSpread
        /// <summary>
        /// Initialize Farpoint Spread/SheetView objects.
        /// </summary>
        /// <param name="spreadList"></param>
        /// <returns></returns>
        public static void InitSpread(params FpSpread [] spreadList)
		{
            try
            {
                ColumnHeaderRenderer renderer = new ColumnHeaderRenderer();
                renderer.WordWrap = false;

                foreach (FpSpread fp in spreadList)
                {
                    foreach (SheetView sv in fp.Sheets)
                    {
                        sv.ColumnHeader.Rows[0].Renderer = renderer;
                        sv.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
                        sv.DataAutoSizeColumns = false;
                        sv.DataAutoCellTypes = false;
                        sv.ColumnCount = 0;
                        sv.OperationMode = OperationMode.ExtendedSelect;
                    }

                    fp.ScrollBarTrackPolicy = ScrollBarTrackPolicy.Both;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ SetSpreadData ]

        public static void SetSpreadData(DataTable dt, SheetView sheet, int minWidth)
        {
            try
            {
                sheet.Reset();
                sheet.DataAutoCellTypes = false;
                sheet.OperationMode = OperationMode.ExtendedSelect;
                sheet.DataSource = dt;

                for (int c = 0; c < sheet.ColumnCount; c++)
                {
                    // DataType Setting
                    switch (dt.Columns[c].DataType.Name)
                    {
                        case "String":
                            FarPoint.Win.Spread.CellType.TextCellType text = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = text;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "Decimal":
                            FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = 0;
                            num.ShowSeparator = true;
                            sheet.Columns[c].CellType = num;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DOUBLE":
                        case "INT16":
                        case "INT32":
                        case "INT64":
                            NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
                            intType.DecimalPlaces = 0;
                            intType.ShowSeparator = true;
                            sheet.Columns[c].CellType = intType;
                            sheet.Columns[c].HorizontalAlignment = CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = CellVerticalAlignment.Center;
                            break;
                        case "DateTime":
                            FarPoint.Win.Spread.CellType.DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                            date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                            sheet.Columns[c].CellType = date;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        default:
                            MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = type;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                    }
                    sheet.Columns[c].Width = (sheet.GetPreferredColumnWidth(c, true, false) > minWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetSpreadData(DataTable dt, FarPoint.Win.Spread.SheetView sheet, int minColumnWidth, int defultPlaces, bool IsColumnFixed = false, bool IsShowSeparator = true)
        {
            try
            {
                sheet.DataSource = dt;

                for (int c = 0; c < sheet.ColumnCount; c++)
                {
                    // DataType Setting
                    switch (dt.Columns[c].DataType.Name.ToUpper())
                    {
                        case "CHAR":
                        case "STRING":
                            FarPoint.Win.Spread.CellType.TextCellType text = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = text;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "FLOAT":
                        case "DOUBLE":
                        case "DECIMAL":
                            FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = defultPlaces;
                            num.ShowSeparator = IsShowSeparator;
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
                            intType.ShowSeparator = IsShowSeparator;
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
                        default:
                            System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "SetSpreadData"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = type;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                    }
                    if (IsColumnFixed == true)
                        sheet.Columns[c].Width = minColumnWidth;
                    else
                        sheet.Columns[c].Width = (sheet.GetPreferredColumnWidth(c, true, false) > minColumnWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ SetSpreadSubSum ]

        public static void SetSpreadSubSum(SheetView sv, int iTargetCol, int[] iSumCol, int[] IsAvgCol)
        {
            string sColName = string.Empty;
            int iRCount = 0;
            double[] dbTotal = new double[iSumCol.Length];

            try
            {
                for (int i = 0; i <= sv.RowCount; i++)
                {
                    if (i != sv.RowCount && sColName == sv.Cells[i, iTargetCol].Text) 
                    {
                        for (int j = 0; j < iSumCol.Length; j++)
                        {
                            dbTotal[j] += System.Convert.ToDouble(sv.Cells[i, iSumCol[j]].Value == DBNull.Value ? 0 : sv.Cells[i, iSumCol[j]].Value);
                        }
                        iRCount++;
                    }
                    else
                    {
                        if (sColName != string.Empty && iRCount > 1)
                        {
                            sv.Rows.Add(i, 1);

                            for (int j = 0; j < iSumCol.Length; j++)
                                sv.Cells[i, iSumCol[j]].Value = dbTotal[j];

                            if (IsAvgCol != null)
                            {
                                for (int k = 0; k < IsAvgCol.Length; k++)
                                    sv.Cells[i, IsAvgCol[k]].Value = Convert.ToDouble(sv.Cells[i, IsAvgCol[k]].Value == DBNull.Value ? 0 : sv.Cells[i, IsAvgCol[k]].Value) / (double)iRCount;
                            }

                            sv.Cells[i, iTargetCol].Value = string.Format("[{0}]", sv.Cells[i - 1, iTargetCol].Value);
                            //sv.SetRowMerge(i, FarPoint.Win.Spread.Model.MergePolicy.Always);
                            sv.Rows[i].BackColor = System.Drawing.Color.LavenderBlush;
                            i++;                        
                        }

                        if (i != sv.RowCount)
                        {
                            sColName = sv.Cells[i, (sv.Columns[iTargetCol].Index)].Text;
                            iRCount = 1;
                            
                            for (int j = 0; j < iSumCol.Length; j++)
                                dbTotal[j] = Convert.ToDouble(sv.Cells[i, iSumCol[j]].Value == DBNull.Value ? 0 : sv.Cells[i, iSumCol[j]].Value);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void InsertSubSum(DataTable dt, string sTargetCol, string sSumCol, bool IsAvg)
        {
            try
            {
                string sColumn = string.Empty;
                int irowCount = 0;
                double dbTotal = 0.0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (sColumn == dt.Rows[i][sTargetCol].ToString())
                    {
                        dbTotal += System.Convert.ToDouble(dt.Rows[i][sSumCol]);
                        irowCount++;
                    }
                    else
                    {
                        //if (sColumn != string.Empty && irowCount > 1)
                        //{
                        //    DataRow drRow = dt.NewRow();
                        //    drRow[s1st] = sColumn;
                        //    if (s2nd != "None")
                        //        drRow[s2nd] = "Sub Total";
                        //    if (s3rd != "None")
                        //        drRow[s3rd] = "Sub Total";
                        //    if (s4th != "None")
                        //        drRow[s4th] = "Sub Total";
                        //    if (s5th != "None")
                        //        drRow[s5th] = "Sub Total";

                        //    drRow["Yield"] = dbTotal / irowCount;

                        //    dt.Rows.InsertAt(drRow, i);
                        //}

                        sColumn = dt.Rows[i][0].ToString();
                        irowCount = 1;
                        //dbTotal = System.Convert.ToDouble(dt.Rows[i]["Yield"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region [ SetDecimalPlaces ]

        public static void SetDecimalPlaces(SheetView sheet, string[] arrColName, int[] arrDecimalPlaces)
        {
            try
            {
                int nColIndex = -1;
                int nCount = arrDecimalPlaces.Length;
                FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();

                for (int i = 0; i < arrColName.Length; i++)
                {
                    for (int j = 0; j < sheet.Columns.Count; j++)
                    {
                        if (sheet.Columns[j].Label == arrColName[i])
                        {
                            nColIndex = j;
                            break;
                        }
                    }
                    
                    if (i >= nCount)
                    {
                        num.DecimalPlaces = arrDecimalPlaces[nCount - 1];
                    }
                    else
                    {
                        num.DecimalPlaces = arrDecimalPlaces[i];
                    }
                    sheet.Columns[nColIndex].CellType = num;

                    nColIndex = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetDecimalPlaces(FarPoint.Win.Spread.SheetView sheet, int[] arrColIndex, int[] arrDecimalPlaces, bool IsShowSeparator = true)
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
                    num.ShowSeparator = IsShowSeparator;
                    num.MaximumValue = 99999999999999;
                    sheet.Columns[arrColIndex[i]].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ SetDateFormat ]

        public static void SetDateFormat(SheetView sheet, string[] arrColName, FarPoint.Win.Spread.CellType.DateTimeFormat[] arrDateTimeFormat)
        {
            try
            {
                int nColIndex = -1;
                int nCount = arrDateTimeFormat.Length;
                FarPoint.Win.Spread.CellType.DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                for (int i = 0; i < arrColName.Length; i++)
                {
                    for (int j = 0; j < sheet.Columns.Count; j++)
                    {
                        if (sheet.Columns[j].Label == arrColName[i])
                        {
                            nColIndex = j;
                            break;
                        }
                    }

                    if (i >= nCount)
                    {
                        date.DateTimeFormat = arrDateTimeFormat[nCount - 1];
                    }
                    else
                    {
                        date.DateTimeFormat = arrDateTimeFormat[i];
                    }
                    sheet.Columns[nColIndex].CellType = date;
                    sheet.Columns[nColIndex].Width = sheet.GetPreferredColumnWidth(nColIndex, true, false) + 30;
                    
                    nColIndex = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SetSheetColumnWidth
        /// <summary>
        /// Sets proper width of SheetView columns.
        /// </summary>
        /// <param name="spreadList"></param>
        /// <returns></returns>
        public static void SetSheetColumnWidth(params SheetView[] sheetList)
        {
            try
            {
                foreach (SheetView sv in sheetList)
                {
                    sv.DataAutoSizeColumns = false;
                    sv.DataAutoCellTypes = false;

                    for (int i = 0; i < sv.Columns.Count; i++)
                    {
                        sv.Columns[i].Width = sv.GetPreferredColumnWidth(i, false) + 10;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region [ ChartFx ]

        #region InitChart
        /// <summary>
        /// Initialize ChartFX objects
        /// </summary>
        /// <param name="chartList"></param>
        //public static void InitChart(params Chart [] chartList)
        //{
        //    foreach (Chart chart in chartList)
        //    {
        //        chart.Reset();

                //chart.AllowChanges ^= ChartFX.WinForms.AllowChanges.All;
                //chart.AllowDrag = false;
                //chart.AllowDrop = false;
                //chart.AllSeries.AxisX.LabelAngle = 0;
                //chart.Antialiasing = true;

                //chart.Data.InterpolateHidden = true;
                //chart.DataGrid.Visible = false;
                //chart.FileContents = ChartFX.WinForms.FileContents.All;
                //chart.LegendBox.Visible = false;
                //chart.Points.Clear();
                //chart.Series.Clear();
                //chart.UseWaitCursor = false;
                //chart.View3D.Enabled = false;

                //chart.ContextMenus = false;
                //chart.Border = new ChartFX.WinForms.Adornments.SimpleBorder(ChartFX.WinForms.Adornments.SimpleBorderType.Color, Color.LightGray);
                //chart.Background = new ChartFX.WinForms.Adornments.SolidBackground(Color.White);
                //chart.AxisY.DataFormat.Format = ChartFX.WinForms.AxisFormat.Number;
                //chart.AxisY.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Number;                  
                //chart.Data.Clear();
        //    }
        //}
        #endregion

        #endregion



        public static void InitMSChart(ref Chart chart)
        {
            chart.Titles.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            chart.BackColor = System.Drawing.Color.LightYellow;
            chart.ChartAreas.Add("Default");
            chart.ChartAreas["Default"].BackColor = System.Drawing.Color.LightSkyBlue;
            chart.ChartAreas["Default"].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chart.ChartAreas["Default"].BorderColor = System.Drawing.Color.Black;
            chart.ChartAreas["Default"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chart.ChartAreas["Default"].BorderWidth = 1;
            chart.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            chart.Titles.Add("Default");
            chart.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
            chart.Titles[0].Text = chart.Text;

            chart.Legends.Add("Default");
            chart.Legends["Default"].Alignment = System.Drawing.StringAlignment.Center;
            chart.Legends["Default"].BackColor = System.Drawing.Color.LightYellow;
            chart.Legends["Default"].BorderColor = System.Drawing.Color.DarkKhaki;
            chart.Legends["Default"].BorderWidth = 2;
        }

        /// <summary>
        /// 2014-07-01-김준용: Palette 를 사용하기 위해 추가
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="palette"></param>
        public static void InitMSChart(ref Chart chart, ChartColorPalette palette)
        {
            chart.Titles.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Clear();


            chart.ChartAreas.Add("Default");
            //chart.ChartAreas["Default"].BackColor = System.Drawing.Color.LightSkyBlue;
            //chart.ChartAreas["Default"].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            //chart.ChartAreas["Default"].BorderColor = System.Drawing.Color.Black;
            //chart.ChartAreas["Default"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            //chart.ChartAreas["Default"].BorderWidth = 1;
            //chart.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            //chart.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            //chart.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            chart.Titles.Add("Default");
            chart.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
            chart.Titles[0].Text = chart.Text;

            chart.Legends.Add("Default");
            chart.Legends["Default"].Alignment = System.Drawing.StringAlignment.Center;
            //chart.Legends["Default"].BackColor = System.Drawing.Color.LightYellow;
            //chart.Legends["Default"].BorderColor = System.Drawing.Color.DarkKhaki;
            //chart.Legends["Default"].BorderWidth = 2;

            chart.Palette = palette;
        }

        public static void SetColumnTypeNumber(ref FarPoint.Win.Spread.FpSpread Spread,
                                               int nStartIndex, int nDecimalPlaces)
        {
            try
            {
                FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                num.DecimalPlaces = nDecimalPlaces;
                num.ShowSeparator = true;
                num.MaximumValue = 99999999999999;
                for (int i = nStartIndex - 1; i < Spread.ActiveSheet.Columns.Count; i++)
                {
                    Spread.ActiveSheet.Columns[i].CellType = num;
                    //Spread.ActiveSheet.Cells[0, i, Spread.ActiveSheet.Rows.Count - 1, i].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


