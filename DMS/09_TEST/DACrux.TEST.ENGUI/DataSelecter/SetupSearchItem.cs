using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public delegate void SetSelection(string[] strItem);

    public partial class SetupSearchItem : Form
    {
        public event SetSelection OnSetSelection = null;

        public SetupSearchItem()
        {
            InitializeComponent();
        }

        public SetupSearchItem(string[] strAllItem, string[] strSelectItem)
        {
            InitializeComponent();

            fnSetItemList(strAllItem, strSelectItem);
        }

        private void fnSetItemList(string[] strAllItem, string[] strSelectItem)
        {
            ListViewItem oItem = null;
            try
            {
                for (int r = 0; r < strSelectItem.Length; r++)
                {
                    oItem = new ListViewItem(strSelectItem[r]);
                    lsSelection.Items.Add(oItem);
                }

                for (int r = 0; r < strAllItem.Length; r++)
                {
                    //정의된 Item 이 없을 경우 추가 한다.
                    oItem = new ListViewItem(strAllItem[r]);

                    if(Array.IndexOf(strSelectItem, strAllItem[r]) < 0)
                        lsAvailable.Items.Add(oItem);
                }

                lsSelection.Update();
                lsAvailable.Update();
            }
            finally
            {
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsAvailable.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsAvailable.SelectedItems.Count];

                    for (int i = 0; i < lsAvailable.SelectedItems.Count; i++)
                        oSelectItem[i] = lsAvailable.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsAvailable.SelectedItems)
                    {
                        lsAvailable.Items.Remove(eachItem);
                    }
                    
                       
                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                         if (lsSelection.FindItemWithText(oSelectItem[i].Text) == null)
                         {
                             oItem = new ListViewItem(oSelectItem[i].Text);
                             lsSelection.Items.Add(oItem);
                         }
                    }

                    lsAvailable.Update();
                    lsSelection.Update();
                }
            }
            finally
            {

            }        
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            ListViewItem oItem = null;
            ListViewItem[] oSelectItem = null;
            try
            {
                if (lsSelection.SelectedItems.Count > 0)
                {
                    oSelectItem = new ListViewItem[lsSelection.SelectedItems.Count];

                    for (int i = 0; i < lsSelection.SelectedItems.Count; i++)
                        oSelectItem[i] = lsSelection.SelectedItems[i];

                    foreach (ListViewItem eachItem in lsSelection.SelectedItems)
                    {
                        lsSelection.Items.Remove(eachItem);
                    }
                    
                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        if (lsAvailable.FindItemWithText(oSelectItem[i].Text) == null)
                        {
                            oItem = new ListViewItem(oSelectItem[i].Text);
                            lsAvailable.Items.Add(oItem);
                        }
                    }

                    lsSelection.Update();
                    lsAvailable.Update();
                }
            }
            finally
            {

            }  
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (lsSelection.SelectedItems == null || lsSelection.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelection.SelectedItems[0];
            int iCurrIdx = lsSelection.SelectedIndices[0];

            if (iCurrIdx == 0) return;
            try
            {
                lsSelection.Items.RemoveAt(iCurrIdx);
                lsSelection.Items.Insert(iCurrIdx - 1, lvSelItem);
            }
            finally
            {
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (lsSelection.SelectedItems == null || lsSelection.SelectedItems.Count == 0) return;

            ListViewItem lvSelItem = lsSelection.SelectedItems[0];
            int iCurrIdx = lsSelection.SelectedIndices[0];

            if (iCurrIdx == lsSelection.Items.Count - 1) return;
            try
            {
                lsSelection.Items.RemoveAt(iCurrIdx);
                lsSelection.Items.Insert(iCurrIdx + 1, lvSelItem);
            }
            finally
            {
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<string> lsSelectItem = null;
            try
            {
                if (OnSetSelection != null)
                {
                    lsSelectItem = new List<string>();
                    for (int i = 0; i < lsSelection.Items.Count; i++)
                    {
                        lsSelectItem.Add(lsSelection.Items[i].Text);
                    }

                    if (lsSelectItem.Count <= 0)
                    {
                        MessageBox.Show("선택된 Item이 없습니다. 최소 한개 이상의 Item을 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    OnSetSelection(lsSelectItem.ToArray());
                }

                this.Close();
            }
            finally
            {
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
