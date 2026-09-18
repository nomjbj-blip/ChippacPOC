using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DACrux.ProjectManager.UI;
using PMUI = DACrux.ProjectManager.UI;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewTranspose : Form
    {
        #region " MEMBER FIELD "

        private System.Collections.Hashtable htColumns = new System.Collections.Hashtable();
        private DataTable dataSource = null;
        private List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo = null;

        #endregion

        #region " PROPERTY "

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }        

        #endregion

        #region " CREATOR "

        public DlgDataViewTranspose(DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo)
        {
            InitializeComponent();

            InitDialog();

            this.dataSource = dataSource;
            this.lstColumnInfo = lstColumnInfo;
            SetColumnListView();
        }

        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            this.Text = "Transpose";

            lvColumns.SmallImageList = imlColumnType;
            lvRow.SmallImageList = imlColumnType;
            lvColumn.SmallImageList = imlColumnType;
            lvData.SmallImageList = imlColumnType;

            lvColumns.AllowDrop = true;
            lvRow.AllowDrop = true;
            lvColumn.AllowDrop = true;
            lvData.AllowDrop = true;

            lvColumns.MultiSelect = true;
            lvRow.MultiSelect = true;
            lvColumn.MultiSelect = true;
            lvData.MultiSelect = true;

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.btnRight_Row.Click += new EventHandler(btnRight_Row_Click);
            this.btnLeft_Row.Click += new EventHandler(btnLeft_Row_Click);
            this.btnRight_Col.Click += new EventHandler(btnRight_Col_Click);
            this.btnLeft_Col.Click += new EventHandler(btnLeft_Col_Click);
            this.btnRight_Data.Click += new EventHandler(btnRight_Data_Click);
            this.btnLeft_Data.Click += new EventHandler(btnLeft_Data_Click);

            this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
            this.lvRow.ItemDrag += new ItemDragEventHandler(lvRow_ItemDrag);
            this.lvColumn.ItemDrag += new ItemDragEventHandler(lvColumn_ItemDrag);
            this.lvData.ItemDrag += new ItemDragEventHandler(lvData_ItemDrag);

            this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
            this.lvRow.DragEnter += new DragEventHandler(lvRow_DragEnter);
            this.lvColumn.DragEnter += new DragEventHandler(lvColumn_DragEnter);
            this.lvData.DragEnter += new DragEventHandler(lvData_DragEnter);

            this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
            this.lvRow.DragDrop += new DragEventHandler(lvRow_DragDrop);
            this.lvColumn.DragDrop += new DragEventHandler(lvColumn_DragDrop);
            this.lvData.DragDrop += new DragEventHandler(lvData_DragDrop);
        }

        private void SetColumnListView()
        {
            ListViewItem lvItem;
            DataColumn column;
            for (int i = 0; i < dataSource.Columns.Count; i++)
            {
                column = dataSource.Columns[i];
                lvItem = new ListViewItem("");

                if (column.DataType == typeof(double))
                    lvItem.ImageKey = "NUMBER";
                else if (column.DataType == typeof(string))
                    lvItem.ImageKey = "TEXT";
                else if (column.DataType == typeof(DateTime))
                    lvItem.ImageKey = "DATETIME";

                lvItem.SubItems.Add(lstColumnInfo[i].ColumnID);
                lvItem.SubItems.Add(lstColumnInfo[i].ColumnName);

                lvColumns.Items.Add(lvItem);

                lstColumnInfo[i].ColumnIndex = i;
                htColumns.Add(lvItem, lstColumnInfo[i]);
                htColumns.Add(lstColumnInfo[i], lvItem);
            }
        }

        private DataTable ProcessTranspose()
        {
            DataView.ColumnInfo colColumnInfo = null;
            List<DataView.ColumnInfo> lstRowColumnInfo = null;
            List<DataView.ColumnInfo> lstDataColumnInfo = null;

            DataTable dtSource = null;
            DataTable dtTarget = null;
            DataRow[] arrDataRow = null;
            System.Data.DataView dv;

            string strFilterExpression = string.Empty;
            string strSortExpression = string.Empty;
            string strSortPrefix = ", ";

            string strTemp = string.Empty;
            int iTemp = 0;

            bool autoSort = true;

            int rowColumnCount = 0;

            try
            {
                #region " ColumnInfo Setting "

                lstRowColumnInfo = new List<DataView.ColumnInfo>();
                lstDataColumnInfo = new List<DataView.ColumnInfo>();

                if (lvColumn.Items.Count > 0)
                    colColumnInfo = (DataView.ColumnInfo)htColumns[lvColumn.Items[0]];

                if(lvRow.Items.Count > 0)   // 
                {
                    for (int i = 0; i < lvRow.Items.Count; i++)
                        lstRowColumnInfo.Add((DataView.ColumnInfo)htColumns[lvRow.Items[i]]);
                }

                for (int i = 0; i < lvData.Items.Count; i++)
                    lstDataColumnInfo.Add((DataView.ColumnInfo)htColumns[lvData.Items[i]]);

                rowColumnCount = lstRowColumnInfo.Count;

                #endregion

                #region " DataTable Setting "

                dataSource.AcceptChanges();
                dtSource = dataSource.Copy();
                dtTarget = new DataTable();

                for (int i = 0; i < rowColumnCount; i++)
                    dtTarget.Columns.Add(string.Format("¤¥¢§{0}", i), typeof(string));

                
                #endregion
                int requiredColCount = 0;
                if(colColumnInfo == null)
                {
                    if (rowColumnCount > 0)
                    {

                        dtTarget.Rows.Add(dtTarget.NewRow());

                        for (int j = 0; j < rowColumnCount; j++)
                        {
                            dtTarget.Rows[0][j] = lstRowColumnInfo[j].ColumnName;
                        }

                        #region " Column 아이템 지정 안하고 Rows 아이템 지정 되어있을 때 (예전버전) "
                        while (dtSource.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtSource.Rows.Count; i++)
                            {
                                #region " Select Unique Rows "

                                strFilterExpression = GetSelectFilterExpression(lstRowColumnInfo, dtSource, i);
                                arrDataRow = dtSource.Select(strFilterExpression);

                                #endregion

                                #region " Target DataTable Column & Row Setting "

                                requiredColCount = rowColumnCount + (lstDataColumnInfo.Count * arrDataRow.Length);
                                if (dtTarget.Columns.Count < requiredColCount)
                                {
                                    for (int j = dtTarget.Columns.Count; j < requiredColCount; j++)
                                        dtTarget.Columns.Add((j).ToString(), typeof(string));
                                }

                                dtTarget.Rows.Add(dtTarget.NewRow());

                                


                                #endregion

                                #region " Data Filling "

                                int rowIndex = dtTarget.Rows.Count - 1;
                                int colIndex = 0;

                                for (int j = 0; j < rowColumnCount; j++)
                                {
                                    dtTarget.Rows[rowIndex][colIndex++] = arrDataRow[0][lstRowColumnInfo[j].ColumnIndex];
                                }

                                for (int j = 0; j < lstDataColumnInfo.Count; j++) 
                                {
                                    for (int k = 0; k < arrDataRow.Length; k++)
                                    {
                                        dtTarget.Rows[rowIndex][colIndex++] = arrDataRow[k][lstDataColumnInfo[j].ColumnIndex];
                                    }
                                }

                                #endregion

                                #region " Delete Selected DataRow from Source DataTable "

                                for (int j = 0; j < arrDataRow.Length; j++)
                                {
                                    dtSource.Rows.Remove(arrDataRow[j]);
                                }

                                dtSource.AcceptChanges();

                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                else
                {

                    if (rowColumnCount > 0)
                    {
                        #region " Column 아이템 지정하고 Rows도 있을 때 "

                        #region " Check Duplication of Combination "

                        List<DataView.ColumnInfo> lstColumnInfo = new List<DataView.ColumnInfo>();

                        lstColumnInfo.AddRange(lstRowColumnInfo);
                        lstColumnInfo.Add(colColumnInfo);

                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            if (dtSource.Rows[i][colColumnInfo.ColumnIndex] == null || dtSource.Rows[i][colColumnInfo.ColumnIndex].ToString() == string.Empty)
                            {
                                throw (new Exception("Column item has a empty value. Column header can not be empty."));
                            }
                            else
                            {
                                strFilterExpression = GetSelectFilterExpression(lstColumnInfo, dtSource, i);
                                if (dataSource.Select(strFilterExpression).Length > 1)
                                    throw (new Exception("Duplicated combination of Row(s) and Column exists."));
                            }
                        }
                        
                        #endregion

                        #region " Name Row "

                        dtTarget.Rows.Add(dtTarget.NewRow());

                        for (int j = 0; j < rowColumnCount; j++)
                        {
                            dtTarget.Rows[0][j] = lstRowColumnInfo[j].ColumnName;
                        }

                        #endregion

                        while (dtSource.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtSource.Rows.Count; i++)
                            {

                                #region " Select Unique Rows "

                                strFilterExpression = GetSelectFilterExpression(lstRowColumnInfo, dtSource, i);
                                arrDataRow = dtSource.Select(strFilterExpression);

                                #endregion

                                #region " Data Filling "

                                dtTarget.Rows.Add(dtTarget.NewRow());
                                int rowIndex = dtTarget.Rows.Count - 1;
                                int colIndex = 0;

                                for (int j = 0; j < rowColumnCount; j++)
                                {
                                    dtTarget.Rows[rowIndex][colIndex++] = arrDataRow[0][lstRowColumnInfo[j].ColumnIndex];
                                } 
                                
                                string colName = string.Empty;
                                for (int k = 0; k < arrDataRow.Length; k++)
                                {
                                    colName = arrDataRow[k][colColumnInfo.ColumnIndex].ToString();
                                    if (!dtTarget.Columns.Contains(colName))
                                    {
                                        dtTarget.Columns.Add(colName, typeof(string));
                                        dtTarget.Rows[0][colName] = colName;
                                    }

                                    for (int j = 0; j < lstDataColumnInfo.Count; j++)
                                    {
                                        dtTarget.Rows[rowIndex][colName] = arrDataRow[k][lstDataColumnInfo[j].ColumnIndex];
                                    }

                                }

                                #endregion

                                #region " Delete Selected DataRow from Source DataTable "

                                for (int j = 0; j < arrDataRow.Length; j++)
                                {
                                    dtSource.Rows.Remove(arrDataRow[j]);
                                }

                                dtSource.AcceptChanges();

                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region " Column 1, Rows 0, 데이터도 1개"

                        dv = dtSource.DefaultView;
                        dv.Sort = string.Format("{0}", dtSource.Columns[colColumnInfo.ColumnIndex].ColumnName);

                        DataTable dtDistinct = dv.ToTable(true, dtSource.Columns[colColumnInfo.ColumnIndex].ColumnName);

                        while (dtDistinct.Rows.Count > 0)
                        {
                            strFilterExpression = GetSelectFilterExpression(colColumnInfo.DataType, 0, 0, dtDistinct);
                            arrDataRow = dtSource.Select(strFilterExpression);

                            strTemp = dtDistinct.Rows[0][0].ToString();

                            if(strTemp != string.Empty)
                                dtTarget.Columns.Add(strTemp, typeof(string));
                            else
                                dtTarget.Columns.Add("EMPTY_COLUMN_" + (iTemp++).ToString(), typeof(string));

                            while(dtTarget.Rows.Count < arrDataRow.Length)
                                dtTarget.Rows.Add(dtTarget.NewRow());

                            dtTarget.AcceptChanges();

                            for (int i = arrDataRow.Length - 1; i >= 0 ; i--)
                            {
                                dtTarget.Rows[i][strTemp] = arrDataRow[i][lstDataColumnInfo[0].ColumnIndex];
                            }

                            dtDistinct.Rows[0].Delete();
                            dtDistinct.AcceptChanges();
                        }

                        dtTarget.Rows.InsertAt(dtTarget.NewRow(), 0);
                        for(int i=0; i<dtTarget.Columns.Count; i++)
                        {
                            dtTarget.Rows[0][i] = dtTarget.Columns[i].ColumnName;
                        }

                        autoSort = false;

                        #endregion
                    }
                }

                #region " Target DataTable Sort & NameRow Insert "

                if(autoSort)
                {
                    dv = dtTarget.DefaultView;

                    for (int i = 0; i < rowColumnCount; i++)
                        strSortExpression += strSortPrefix + dtTarget.Columns[i].ColumnName;

                    if (strSortExpression.Length > 0)
                    {
                        object[] row = dtTarget.Rows[0].ItemArray;
                        dtTarget.Rows.RemoveAt(0);

                        strSortExpression = strSortExpression.Substring(strSortPrefix.Length) + " ASC";
                        dv.Sort = strSortExpression;
                        dtTarget = dv.ToTable();

                        
                        dtTarget.Rows.InsertAt(dtTarget.NewRow(), 0);
                        for (int i = 0; i < dtTarget.Columns.Count; i++)
                            dtTarget.Rows[0][i] = row[i];

                        dtTarget.AcceptChanges();
                    }
                }

                dtTarget.AcceptChanges();

                #endregion

                return dtTarget;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GetSelectFilterExpression(DACrux.ProjectManager.UI.DataType dataType, int columnIndex, int rowIndex, DataTable dtSource)
        {
            object objValue;
            string strFilterExpression = string.Empty;

            strFilterExpression += dtSource.Columns[columnIndex].ColumnName;
            objValue = dtSource.Rows[rowIndex][columnIndex];

            if (objValue == null || objValue.ToString() == string.Empty)
                strFilterExpression += " IS NULL";
            else
            {
                if (dataType != DataType.NUMBER)
                    strFilterExpression += " = '" + objValue + "'";
                else if (dataType == DataType.NUMBER)
                    strFilterExpression += " = " + objValue;
            }

            return strFilterExpression;
        }

        private string GetSelectFilterExpression(List<DataView.ColumnInfo> lstColumnInfo, DataTable dtSource, int rowIndex)
        {
            object objValue;
            string strFilterPrefix = " AND ";
            string strFilterExpression = string.Empty;

            for (int j = 0; j < lstColumnInfo.Count; j++)
            {
                strFilterExpression += strFilterPrefix + "[" + dtSource.Columns[lstColumnInfo[j].ColumnIndex].ColumnName + "]";
                objValue = dtSource.Rows[rowIndex][lstColumnInfo[j].ColumnIndex];

                if (objValue == null || objValue.ToString() == string.Empty)
                    strFilterExpression += " IS NULL";
                else
                {
                    if (lstColumnInfo[j].DataType != DataType.NUMBER)
                        strFilterExpression += " = '" + objValue + "'";
                    else if (lstColumnInfo[j].DataType == DataType.NUMBER)
                        strFilterExpression += " = " + objValue;
                }
            }
            strFilterExpression = strFilterExpression.Substring(strFilterPrefix.Length);

            return strFilterExpression;
        }

        #endregion

        #region " EVENT HANDLER "

        #region [ ListViewItem Move Button ]

        private void btnRight_Row_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvRow, (IList)lvColumns.SelectedItems);
        }

        private void btnLeft_Row_Click(object sender, EventArgs e)
        {
            if (lvRow.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvRow, lvColumns, (IList)lvRow.SelectedItems);
        }

        private void btnRight_Col_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvColumn.Items.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumn, lvColumns, (IList)lvColumn.Items);

                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvColumn, lvColumns.SelectedItems[0]);
            }
        }

        private void btnLeft_Col_Click(object sender, EventArgs e)
        {
            if (lvColumn.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumn, lvColumns, (IList)lvColumn.SelectedItems);
        }

        void btnRight_Data_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvData, (IList)lvColumns.SelectedItems);
        }

        void btnLeft_Data_Click(object sender, EventArgs e)
        {
            if (lvData.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvData, lvColumns, (IList)lvData.SelectedItems);
        }

        #endregion

        #region [ Drag & Drop ]

        private ListView lvDragSource;

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvRow_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvColumn_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvData_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvColumns)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvRow_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvRow)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvColumn_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvColumn)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvData_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvData)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
            }

            lvDragSource = null;
        }

        void lvRow_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                IList items = (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, items);
            }

            lvDragSource = null;
        }

        void lvColumn_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                IList items = (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));

                if (items.Count > 0)
                {
                    if (lvColumn.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumn, lvColumns, (IList)lvColumn.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvDragSource, lvColumn, (ListViewItem)items[0]);
                }
            }

            lvDragSource = null;
        }

        void lvData_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                IList items = (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, items);
            }

            lvDragSource = null;
        }

        #endregion

        #region [ Closing ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                #region [ Assertion ]

                if (lvData.Items.Count < 1)
                    throw (new Exception("Select one or more columns for Transpose Data."));

                for (int i = 1; i < lvData.Items.Count; i++)
                {
                    if (lvData.Items[i - 1].ImageKey != lvData.Items[i].ImageKey)
                        throw (new Exception("Different datatype exists in Transpose Data."));
                }

                if(lvRow.Items.Count < 1)
                {
                    if(lvColumn.Items.Count < 1)
                        throw (new Exception("Must select one or more columns for Row or Column."));
                    else if(lvColumn.Items.Count != 1)
                        throw (new Exception("Select only one column for Column."));
                    else if(lvColumn.Items.Count == 1 && lvData.Items.Count > 1)
                        throw (new Exception(string.Format("Cannot transpose multiple columns in case a Column item exists. {0}Select single column for Data. Or remove Column item.", Environment.NewLine)));
                }

                #endregion

                dataSource = ProcessTranspose();

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowDataTable(DataTable dtTarget)
        {
            string strTemp = string.Empty;
            for (int a = 0; a < dtTarget.Rows.Count; a++)
            {
                for (int b = 0; b < dtTarget.Columns.Count; b++)
                    strTemp += "\t " + dtTarget.Rows[a][b].ToString();

                strTemp += "\r\n";
            }
            MessageBox.Show(strTemp);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

    }
}