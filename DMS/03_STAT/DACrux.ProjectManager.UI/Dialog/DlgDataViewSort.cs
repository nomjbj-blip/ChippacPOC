using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewSort : Form
    {
        #region " ENUM "

        public enum Mode 
        {
            ASC,
            DESC
        }

        #endregion

        #region " MEMBER FIELD "

        private List<DataView.ColumnInfo> lstValidColumnInfo = null;

        #endregion

        #region " CREATOR "

        public DlgDataViewSort(List<DataView.ColumnInfo> validColumnInfo, bool isSelection)
        {
            InitializeComponent();

            this.lstValidColumnInfo = validColumnInfo;
            chkSelectedAreaOnly.Enabled = isSelection;
            chkSelectedAreaOnly.Checked = chkSelectedAreaOnly.Enabled;

            this.lvLeft.AllowDrop = true;
            this.lvRight.AllowDrop = true;
            this.lvLeft.MultiSelect = true;
            this.lvRight.MultiSelect = true;

            this.lvLeft.ItemDrag += new ItemDragEventHandler(lvLeft_ItemDrag);
            this.lvRight.ItemDrag += new ItemDragEventHandler(lvRight_ItemDrag);

            this.lvLeft.DragEnter += new DragEventHandler(lvLeft_DragEnter);
            this.lvRight.DragEnter += new DragEventHandler(lvRight_DragEnter);

            this.lvLeft.DragDrop += new DragEventHandler(lvLeft_DragDrop);
            this.lvRight.DragDrop += new DragEventHandler(lvRight_DragDrop);
        }

        #endregion

        #region " EVENT HANDLER "

        private void DlgDataViewSort_Load(object sender, EventArgs e)
        {
            ListViewItem lvItem;

            foreach (DataView.ColumnInfo columnInfo in lstValidColumnInfo)
            {
                lvItem = new ListViewItem("", 0);
                lvItem.SubItems.Add("C" + (columnInfo.ColumnIndex + 1).ToString() + ((columnInfo.DataType == DataType.NUMBER) ? "" : ((columnInfo.DataType == DataType.TEXT) ? " - C" : " - D")));
                lvItem.SubItems.Add(columnInfo.ColumnName);

                lvLeft.Items.Add(lvItem);
            }
        }

        private void MoveSelectedItemToRight()
        {
            ListViewItem lvItem;
            int index;

            if (lvLeft.SelectedItems.Count > 0)
            {
                lvItem = lvLeft.SelectedItems[0];
                index = lvItem.Index;
                lvLeft.Items.Remove(lvItem);
                lvRight.Items.Add(lvItem);

                if(lvLeft.Items.Count > 0)
                {
                    if(index == lvLeft.Items.Count)
                        index--;

                    lvLeft.Focus();
                    lvLeft.Items[index].Selected = true;
                    lvLeft.Items[index].Focused = true;
                }
            }
        }

        private void MoveSelectedItemToLeft()
        {
            ListViewItem lvItem;
            int index;

            if (lvRight.SelectedItems.Count > 0)
            {
                lvItem = lvRight.SelectedItems[0];
                index = lvItem.Index;
                lvRight.Items.Remove(lvItem);
                lvLeft.Items.Add(lvItem);

                if (lvRight.Items.Count > 0)
                {
                    if (index == lvRight.Items.Count)
                        index--;

                    lvRight.Focus();
                    lvRight.Items[index].Selected = true;
                    lvRight.Items[index].Focused = true;
                }
            }
        }

        private void lvLeft_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.MoveListViewItem(lvLeft, lvRight);
        }

        private void lvRight_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.MoveListViewItem(lvRight, lvLeft);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (lvLeft.SelectedItems.Count > 0)
                Common.MoveListViewItems(lvLeft, lvRight, (IList)lvLeft.SelectedItems);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (lvRight.SelectedItems.Count > 0)
                Common.MoveListViewItems(lvRight, lvLeft, (IList)lvRight.SelectedItems);
        }


        ListView lvDragSource;

        void lvRight_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
                lvDragSource = null;
            }
        }

        void lvLeft_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
                lvDragSource = null;
            }
        }

        void lvRight_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvRight)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvLeft_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvLeft)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvRight_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvLeft_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            ListViewItem lvItem;
            int index;

            if (lvRight.SelectedItems.Count > 0)
            {
                lvItem = lvRight.SelectedItems[0];
                index = lvItem.Index;

                if(index > 0)
                {
                    lvRight.Items.Remove(lvItem);
                    lvRight.Items.Insert(index - 1, lvItem);

                    lvRight.Focus();
                    lvRight.Items[index - 1].Selected = true;
                    lvRight.Items[index - 1].Focused = true;
                }
                else
                {
                    lvRight.Focus();
                    lvRight.Items[index].Selected = true;
                    lvRight.Items[index].Focused = true;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            ListViewItem lvItem;
            int index;

            if (lvRight.SelectedItems.Count > 0)
            {
                lvRight.Focus();

                lvItem = lvRight.SelectedItems[0];
                index = lvItem.Index;

                if (index < lvRight.Items.Count - 1)
                {
                    lvRight.Items.Remove(lvItem);
                    lvRight.Items.Insert(index + 1, lvItem);

                    lvRight.Items[index + 1].Selected = true;
                    lvRight.Items[index + 1].Focused = true;
                }
                else
                {
                    lvRight.Items[index].Selected = true;
                    lvRight.Items[index].Focused = true;
                }
            }
        }

        private void btnSortMode_Click(object sender, EventArgs e)
        {
            if (lvRight.SelectedItems.Count < 1)
                return;

            lvRight.Focus();

            if (lvRight.SelectedItems[0].ImageIndex == 0)
                lvRight.SelectedItems[0].ImageIndex = 1;
            else
                lvRight.SelectedItems[0].ImageIndex = 0;

            // Sort 모드 변경시 자동으로 다음 항목 넘어가기
            //if (lvRight.SelectedItems[0].Index < lvRight.Items.Count - 1)
            //{
            //    lvRight.Items[lvRight.SelectedItems[0].Index+1].Focused = true;
            //    lvRight.Items[lvRight.SelectedItems[0].Index+1].Selected = true;
            //}
        }

        public string SortResult = null;

        public bool SelectionOnly
        {
            get
            {
                if (chkSelectedAreaOnly.Enabled && chkSelectedAreaOnly.Checked)
                    return true;
                else
                    return false;
            }
        }

        public bool CreateNewWorkSheet
        {
            get
            {
                return chkNewWorksheet.Checked;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SortResult = "";

            for (int i = 0; i < lvRight.Items.Count; i++)
            {
                SortResult += (Convert.ToInt32((lvRight.Items[i].SubItems[1].Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[0]).Substring(1)) - 1).ToString();
                SortResult += " " + ((lvRight.Items[i].ImageIndex == 0) ? Mode.ASC.ToString() : Mode.DESC.ToString()) + ",";
            }

            if (SortResult.Length > 0)
                SortResult = SortResult.Substring(0, SortResult.LastIndexOf(','));

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion
    }
}