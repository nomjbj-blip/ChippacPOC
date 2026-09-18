using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCUpDownListView : UserControl
    {
        public DUCUpDownListView()
        {
            InitializeComponent();
        }

        public ListView.ListViewItemCollection Items
        {
            get
            {
                return lstView.Items;
            }
        }

        public ImageList SmallImageList
        {
            set
            {
                lstView.SmallImageList = value;
            }
            get
            {
                return lstView.SmallImageList;
            }
        }

        public ImageList LargeImageList
        {
            set
            {
                lstView.LargeImageList = value;
            }
            get
            {
                return lstView.LargeImageList;
            }
        }

        public ListView.ColumnHeaderCollection Columns
        {
            get
            {
                return lstView.Columns;
            }
        }

        public ListView.SelectedListViewItemCollection SelectedItem
        {
            get
            {
                return lstView.SelectedItems;
            }
        }

        public ListView.SelectedIndexCollection SelectedIndices
        {
            get
            {
                return lstView.SelectedIndices;
            }
        }



        #region Up/Down Button 

        private void butUp_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems == null || lstView.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lstView.SelectedItems[0];
            int iCurrIdx = lstView.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lstView.Items.RemoveAt(iCurrIdx);
                lstView.Items.Insert(iCurrIdx - 1, lvSelItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butDown_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems == null || lstView.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lstView.SelectedItems[0];
            int iCurrIdx = lstView.SelectedIndices[0];

            if (iCurrIdx == lstView.Items.Count -1) return;
            try
            {
                lstView.Items.RemoveAt(iCurrIdx);
                lstView.Items.Insert(iCurrIdx+1, lvSelItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butLast_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems == null || lstView.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lstView.SelectedItems[0];
            int iCurrIdx = lstView.SelectedIndices[0];

            if (iCurrIdx == lstView.Items.Count - 1) return;
            try
            {
                lstView.Items.RemoveAt(iCurrIdx);
                lstView.Items.Insert(lstView.Items.Count, lvSelItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butFirst_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems == null || lstView.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lstView.SelectedItems[0];
            int iCurrIdx = lstView.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lstView.Items.RemoveAt(iCurrIdx);
                lstView.Items.Insert(0, lvSelItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
