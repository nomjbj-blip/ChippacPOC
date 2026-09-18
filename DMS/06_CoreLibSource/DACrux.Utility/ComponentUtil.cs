using System;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
//using ChartFX.WinForms;
//using ChartFX.WinForms.Adornments;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : ComponentUtil<br/>
    /// Summary    : Component Utility Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2009-11-17<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public static class ComponentUtil
    {
        #region [ FarPoint Spread ]

        #region InitSpread
        /// <summary>
        /// Initialize Farpoint Spread/SheetView objects.
        /// </summary>
        /// <param name="spreadList"></param>
        /// <returns>결과테이블</returns>
        public static void InitSpread(params FpSpread[] spreadList)
        {
            try
            {
                foreach (FpSpread fp in spreadList)
                {
                    foreach (SheetView sv in fp.Sheets)
                    {
                        for (int i = 0; i < sv.ColumnHeader.RowCount; i++)
                            sv.ColumnHeader.Rows[i].Renderer = SpreadColumnHeaderRendererProvider.Renderer;

                        sv.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
                        sv.DataAutoSizeColumns = false;
                        sv.DataAutoCellTypes = false;
                        sv.ColumnCount = 0;
                        sv.RowCount = 0;
                        sv.OperationMode = OperationMode.ExtendedSelect;
                    }

                    for (int i = 0; i < fp.ActiveSheet.RowCount; i++)
                    {
                        fp.ActiveSheet.Rows[i].VerticalAlignment = CellVerticalAlignment.Center;
                        fp.ActiveSheet.Rows[i].HorizontalAlignment = CellHorizontalAlignment.Center;
                    }


                    fp.VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
                    fp.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;

                    fp.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
                    fp.ScrollBarTrackPolicy = ScrollBarTrackPolicy.Both;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SetSpreadAlignCenter

        public static void SetSpreadAlignCenter(params FpSpread[] spreadList)
        {
            try
            {
                foreach (FpSpread fp in spreadList)
                {
                    FarPoint.Win.Spread.Cell cellrange;
                    if (fp.ActiveSheet.RowCount > 0)
                    {
                        cellrange = fp.ActiveSheet.Cells[0, 0, fp.ActiveSheet.RowCount - 1, fp.ActiveSheet.ColumnCount - 1];
                        cellrange.VerticalAlignment = CellVerticalAlignment.Center;
                    }
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
        /// <returns>결과테이블</returns>
        public static void SetSheetColumnWidth(params SheetView[] sheetList)
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

        #endregion

        #region [ ChartFx ]

        #region InitChart
        /// <summary>
        /// Initialize ChartFX objects
        /// </summary>
        /// <param name="chartList"></param>
        //public static void InitChart(params Chart[] chartList)
        //{
        //    foreach (Chart chart in chartList)
        //    {
        //        chart.Reset();

        //        chart.AllowChanges ^= ChartFX.WinForms.AllowChanges.All;
        //        chart.AllowDrag = false;
        //        chart.AllowDrop = false;
        //        chart.AllSeries.AxisX.LabelAngle = 0;
        //        chart.Antialiasing = true;

        //        chart.Data.InterpolateHidden = true;
        //        chart.DataGrid.Visible = false;
        //        chart.FileContents = ChartFX.WinForms.FileContents.All;
        //        chart.LegendBox.Visible = false;
        //        chart.Points.Clear();
        //        chart.Series.Clear();
        //        chart.Data.Clear();
        //        chart.UseWaitCursor = false;
        //        chart.View3D.Enabled = false;

        //        chart.ContextMenus = false;
        //        chart.Border = new SimpleBorder(SimpleBorderType.Raised);
        //        chart.Background = new SolidBackground(Color.White);
        //        chart.AxisY.DataFormat.Format = ChartFX.WinForms.AxisFormat.Number;
        //        chart.AxisY.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Number;

        //        //chart.PlotAreaMargin.Top = 5;
        //        //chart.PlotAreaMargin.Bottom = 5;
        //        //chart.PlotAreaMargin.Left = 5;
        //        //chart.PlotAreaMargin.Right = 5;
        //    }
        //}
        #endregion

        #endregion
    }

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
}
