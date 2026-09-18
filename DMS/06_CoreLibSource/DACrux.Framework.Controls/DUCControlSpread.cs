using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace DACrux.Framework.Controls
{
    public partial class DUCControlSpread : UserControl
    {
        [Category("Miracom")]
        public event EventHandler<DUCControlSpreadRedrawArgs> ControlRedraw;

        /// <summary>
        /// 생성자
        /// </summary>
        public DUCControlSpread()
        {
            InitializeComponent();
            ControlList = new DUCControlSpreadItemList();
            fpSpread1_Sheet1.OperationMode = OperationMode.Normal | OperationMode.ReadOnly;
        }

        protected virtual void OnControlRedraw(DUCControlSpreadRedrawArgs e)
        {
            if (ControlRedraw != null)
                ControlRedraw(this, e);
        }

        /// <summary>
        /// Spread에 컨트롤을 바인딩 합니다.
        /// </summary>
        public void SetData(DUCControlSpreadItemList list)
        {
            SetData(list, null, null);
        }

        /// <summary>
        /// Spread에 컨트롤을 바인딩 합니다.
        /// </summary>
        public void SetData(DUCControlSpreadItemList list , Func<string[], string[]> colHeaderSorter, Func<string[], string[]> rowHeaderSorter )
        {
            Application.DoEvents();
            fpSpread1.ZoomFactor = 1;

            fpSpread1_Sheet1.Rows.Clear();
            fpSpread1_Sheet1.Columns.Clear();
            ControlList = list;

            if (list == null || list.Count == 0)
                return;

            string[] columns = list.GetColumnArray();
            string[] rows = list.GetRowArray();

            // column 정렬이 필요한 경우
            if (colHeaderSorter != null)
                columns = colHeaderSorter.Invoke(columns);

            // row 정렬이 필요한 경우
            if (rowHeaderSorter != null)
                rows = rowHeaderSorter.Invoke(rows);

            fpSpread1_Sheet1.RowCount = rows.Length;
            fpSpread1_Sheet1.ColumnCount = columns.Length;

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                fpSpread1_Sheet1.Rows[i].Height = ControlList[0].Control.Height;
                fpSpread1_Sheet1.RowHeader.Cells[i, 0].Text = rows[i];
            }

            for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
            {
                fpSpread1_Sheet1.Columns[i].Width = ControlList[0].Control.Width;
                fpSpread1_Sheet1.ColumnHeader.Cells[0, i].Text = columns[i];
            }

            for (int r = 0; r < fpSpread1_Sheet1.RowCount; r++)
            {
                for (int c = 0; c < fpSpread1_Sheet1.ColumnCount; c++)
                {
                    GeneralCellType cell = new GeneralCellType();
                    fpSpread1_Sheet1.Cells[r, c].CellType = cell;

                    string colName = fpSpread1_Sheet1.RowHeader.Cells[r, 0].Text;
                    string rowName = fpSpread1_Sheet1.ColumnHeader.Cells[0, c].Text;

                    Control ctl = list.GetControl(colName, rowName);

                    if (ctl != null)
                        SetCellImage(r, c, GetControlImage(ctl));
                }
            }
        }

        private int GetColIndex(string colName)
        {
            for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
            {
                if (fpSpread1_Sheet1.ColumnHeader.Cells[0, i].Text == colName)
                    return i;
            }

            return -1;
        }

        private int GetRowIndex(string rowName)
        {
            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                if (fpSpread1_Sheet1.RowHeader.Cells[i, 0].Text == rowName)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 전부 새로 그립니다.
        /// </summary>
        public void RedrawAll()
        {
            if (ControlList == null)
                return;

            foreach (var item in ControlList)
                SetCellImage(GetRowIndex(item.Row), GetColIndex(item.Col), GetControlImage(item.Control));

            fpSpread1.Refresh();
        }

        /// <summary>
        /// 컨트롤의 크기를 변경합니다.
        /// </summary>
        public void ResizeControl(int colCount, int rowCount)
        {
            if (ControlList == null || colCount == 0 || rowCount == 0 || fpSpread1.Width == 0)
                return;

            if (fpSpread1_Sheet1.ColumnCount == 0 || fpSpread1_Sheet1.RowCount == 0)
                return;

            int width = fpSpread1.Width - SystemInformation.VerticalScrollBarWidth;

            foreach (Column col in fpSpread1_Sheet1.RowHeader.Columns)
                width -= (int)col.Width;

            width = (int)(width / colCount);

            int height = fpSpread1.Height - SystemInformation.HorizontalScrollBarHeight;

            foreach (Row row in fpSpread1_Sheet1.ColumnHeader.Rows)
                height -= (int)row.Height;

            height = (int)(height / rowCount);
            
            for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                fpSpread1_Sheet1.Columns[i].Width = width;

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                fpSpread1_Sheet1.Rows[i].Height = height;

            foreach (var item in ControlList)
            {
                item.Control.Size = new Size(width, height);
                SetCellImage(GetRowIndex(item.Row), GetColIndex(item.Col), GetControlImage(item.Control));
            }

            fpSpread1.Refresh();
        }

        /// <summary>
        /// 컨트롤의 크기를 변경합니다.
        /// </summary>
        public void ResizeControl(System.Drawing.Size size)
        {
            if (ControlList == null)
                return;

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                fpSpread1_Sheet1.Rows[i].Height = size.Height;

            for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                fpSpread1_Sheet1.Columns[i].Width = size.Width;

            foreach (var item in ControlList)
            {
                item.Control.Size = size;
                SetCellImage(GetRowIndex(item.Row), GetColIndex(item.Col), GetControlImage(item.Control));
            }

            fpSpread1.Refresh();
        }

        /// <summary>
        /// 컨트롤을 Image로 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private Image GetControlImage(Control ctl)
        {
            if (ctl.Width == 0 || ctl.Height == 0)
                return null;

            OnControlRedraw(new DUCControlSpreadRedrawArgs(ctl));

            Bitmap bmp = new Bitmap(ctl.ClientRectangle.Width, ctl.ClientRectangle.Height);
            ctl.DrawToBitmap(bmp, ctl.ClientRectangle);
            return bmp;
        }

        /// <summary>
        /// Cell에 이미지를 설정합니다.
        /// </summary>
        private void SetCellImage(int row, int col, Image image)
        {
            GeneralCellType cell = fpSpread1_Sheet1.Cells[row, col].CellType as GeneralCellType;

            if (cell != null)
            {
                if (cell.BackgroundImage != null)
                    cell.BackgroundImage.Dispose();

                cell.BackgroundImage = new FarPoint.Win.Picture(image);
            }
        }

        private void fpSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            if (e.ColumnList.Count == 0)
                return;

            ColumnWidthChangeExtents val = e.ColumnList[0] as ColumnWidthChangeExtents;

            for (int col = val.FirstColumn; col <= val.LastColumn; col++)
            {
                int width = (int)(fpSpread1_Sheet1.Columns[col].Width * fpSpread1.ZoomFactor);

                for (int row = 0; row < fpSpread1_Sheet1.RowCount; row++)
                {
                    string colName = fpSpread1_Sheet1.RowHeader.Cells[row, 0].Text;
                    string rowName = fpSpread1_Sheet1.ColumnHeader.Cells[0, col].Text;

                    Control ctl = ControlList.GetControl(colName, rowName);

                    if (ctl != null)
                    {
                        ctl.Width = width;
                        SetCellImage(row, col, GetControlImage(ctl));
                    }
                }
            }
        }

        private void fpSpread1_RowHeightChanged(object sender, RowHeightChangedEventArgs e)
        {
            if (e.RowList.Count == 0)
                return;

            RowHeightChangeExtents val = e.RowList[0] as RowHeightChangeExtents;

            for (int row = val.FirstRow; row <= val.LastRow; row++)
            {
                int height = (int)(fpSpread1_Sheet1.Rows[row].Height * fpSpread1.ZoomFactor);

                for (int col = 0; col < fpSpread1_Sheet1.ColumnCount; col++)
                {
                    string rowName = fpSpread1_Sheet1.RowHeader.Cells[row, 0].Text;
                    string colName = fpSpread1_Sheet1.ColumnHeader.Cells[0, col].Text;

                    Control ctl = ControlList.GetControl(rowName, colName);

                    if (ctl != null)
                    {
                        ctl.Height = height;
                        SetCellImage(row, col, GetControlImage(ctl));
                    }
                }
            }
        }

        private void fpSpread1_UserZooming(object sender, ZoomEventArgs e)
        {
            if (fpSpread1_Sheet1.RowCount == 0 || fpSpread1_Sheet1.ColumnCount == 0)
                return;

            for (int r = 0; r < fpSpread1_Sheet1.RowCount ; r++)
            {
                int height = (int)(fpSpread1_Sheet1.Rows[r].Height * e.NewZoomFactor);

                for (int c = 0; c < fpSpread1_Sheet1.ColumnCount; c++)
                {
                    int width = (int)(fpSpread1_Sheet1.Columns[c].Width * e.NewZoomFactor);

                    string rowName = fpSpread1_Sheet1.RowHeader.Cells[r, 0].Text;
                    string colName = fpSpread1_Sheet1.ColumnHeader.Cells[0, c].Text;

                    Control ctl = ControlList.GetControl(rowName, colName);

                    if (ctl != null)
                    {
                        ctl.Width = width;
                        ctl.Height = height;
                        SetCellImage(r, c, GetControlImage(ctl));
                    }
                }
            }
        }

        /// <summary>
        /// 선택된 항목들을 가져옵니다.
        /// </summary>
        public Control[] GetSelectedControls()
        {
            FarPoint.Win.Spread.Model.CellRange[] selections = fpSpread1_Sheet1.GetSelections();

            if (selections == null)
                return null;

            List<Control> list = new List<Control>();

            foreach (FarPoint.Win.Spread.Model.CellRange cell in selections)
            {
                int rStart = cell.Row == -1 ? 0 : cell.Row;
                int rEnd = cell.RowCount == -1 ? fpSpread1_Sheet1.RowCount : rStart + cell.RowCount;
                int cStart = cell.Column == -1 ? 0 : cell.Column;
                int cEnd = cell.ColumnCount == -1 ? fpSpread1_Sheet1.ColumnCount : cStart + cell.ColumnCount;

                for (int r = rStart; r < rEnd; r++)
                {
                    for (int c = cStart; c < cEnd; c++)
                    {
                        string rowName = fpSpread1_Sheet1.RowHeader.Cells[r, 0].Text;
                        string colName = fpSpread1_Sheet1.ColumnHeader.Cells[0, c].Text;

                        Control ctl = ControlList.GetControl(rowName, colName);

                        if (ctl != null)
                            list.Add(ctl);
                    }
                }
            }

            return list.ToArray();
        }

        public Size GetItemSize(int columnCount, int rowCount)
        {
            int width = 150;
            int height = 150;

            if (columnCount >  0)
                width = (int)((Width - fpSpread1_Sheet1.RowHeader.Columns[0].Width - SystemInformation.VerticalScrollBarWidth - 4) / columnCount);

            if (rowCount > 0)
                height = (int)((Height - fpSpread1_Sheet1.ColumnHeader.Rows[0].Height - SystemInformation.HorizontalScrollBarHeight - 4) / rowCount);

            return new Size(width, height);
        }

        public void Clear()
        {
            fpSpread1_Sheet1.Rows.Clear();
            fpSpread1_Sheet1.Columns.Clear();
        }

        [Browsable(false)]
        public FpSpread Spread
        {
            get { return fpSpread1; }
        }

        public DUCControlSpreadItemList ControlList
        {
            get;
            private set;
        }
    }

    public class DUCControlSpreadRedrawArgs : EventArgs
    {
        public DUCControlSpreadRedrawArgs(Control ctl)
        {
            Control = ctl;
        }

        public Control Control { get; private set; }
    }

    public class DUCControlSpreadItemList : List<DUCControlSpreadItem>
    {
        public void Add(Control ctl, string row, string col)
        {
            DUCControlSpreadItem item = new DUCControlSpreadItem();
            item.Control = ctl;
            item.Row = row;
            item.Col = col;
            base.Add(item);
        }
        
        public string[] GetColumnArray()
        {
            List<string> list = new List<string>();

            foreach (var item in this)
            {
                if (!list.Contains(item.Col))
                    list.Add(item.Col);
            }

            list.Sort();
            return list.ToArray();
        }

        public string[] GetRowArray()
        {
            List<string> list = new List<string>();

            foreach (var item in this)
            {
                if (!list.Contains(item.Row))
                    list.Add(item.Row);
            }

            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 해당 인덱스의 컨트롤을 가져옵니다.
        /// </summary>
        public Control GetControl(string row, string col)
        {
            foreach (var item in this)
            {
                if (item.Row == row && item.Col == col)
                    return item.Control;
            }

            return null;
        }
    }

    public class DUCControlSpreadItem
    {
        public Control Control { get; set; }
        public string Row { get; set; }
        public string Col { get; set; }
    }
}
