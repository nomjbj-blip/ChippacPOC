using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{    
    /// <summary>
    /// Class Name : SmartListView<br/>
    /// Summary    : SmartListView Control<br/>
    /// Author     : Miracom Hyungsuk, Yang<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class SmartListView2 : System.Windows.Forms.ListView
    {
        #region " INNER CLASS "

        public class SmartListViewItemSeletedEventArgs : EventArgs
        {
            public ListViewItem Item;
        };

        #endregion

        #region " MEMBER FIELD "

        private System.ComponentModel.IContainer components = null;
        private List<ListViewItem> checkedItem = new List<ListViewItem>();

        #endregion

        #region " PROPERTY "

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private new CheckedListViewItemCollection CheckedItems
        {
            get { return null; }
            set { ; }
        }

        [Browsable(false)]
        public List<ListViewItem> CheckedItem
        {
            get { return checkedItem; }
        }

        [Browsable(false)]
        public new IEnumerable<int> CheckedIndices
        {
            get 
            {
                List<int> lstCheckedIndices = new List<int>();

                foreach(ListViewItem lvItem in this.checkedItem)
                {
                    lstCheckedIndices.Add(lvItem.Index);
                }


                return (IEnumerable<int>)lstCheckedIndices; 
            }
        }

        [Category("Setup")]
        public new bool MultiSelect
        {
            get { return base.MultiSelect; }
            set
            {
                base.MultiSelect = value;

                if (!base.MultiSelect)
                    this.Columns[0].Width = 0;
                else
                    this.Columns[0].Width = this.SmallImageList.ImageSize.Width + 4;
            }
        }

        #endregion

        #region " EVENT "

        public event EventHandler ItemSelected = null;

        #endregion

        #region " CREATOR "

        public SmartListView2()
        {
            InitializeComponent();
        }

        public SmartListView2(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region " EVENT HANDLER "

        void SmartListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (FocusedItem == null)
                return;

            if (FocusedItem.ImageIndex == 1)
            {
                if(MultiSelect)
                {
                    FocusedItem.ImageIndex = 0;
                    checkedItem.Remove(FocusedItem);
                }
            }
            else
            {
                if (!MultiSelect)
                {
                    CheckedItem.Clear();
                    foreach (ListViewItem item in Items)
                        item.ImageIndex = 0;
                }

                FocusedItem.ImageIndex = 1;

                if (!checkedItem.Contains(FocusedItem))
                    checkedItem.Add(FocusedItem);
            }

            if (ItemSelected != null)
            {
                SmartListViewItemSeletedEventArgs args = new SmartListViewItemSeletedEventArgs();
                args.Item = FocusedItem;

                ItemSelected(this, args);
            }
        }

        #endregion

        #region " METHOD "

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Clear();

                if (this.SmallImageList != null)
                {
                    this.SmallImageList.Images.Clear();
                    this.SmallImageList.Dispose();
                    this.SmallImageList = null;
                }

                if (this.imlCheckbox != null)
                {
                    this.imlCheckbox.Images.Clear();
                    this.imlCheckbox.Dispose();
                    this.imlCheckbox = null;
                }

                if (!(components == null))
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        internal void ClearHeader()
        {
            this.Columns.Clear();
            this.Columns.Add(colCheck);
        }

        public void ClearItems()
        {
            Items.Clear();
            CheckedItem.Clear();
        }

        public void ClearSelectedItems()
        {
            foreach (ListViewItem item in Items)
                item.ImageIndex = 0;

            SelectedItems.Clear();
            CheckedItem.Clear();
        }

        public bool GetItemChecked(int i)
        {
            return (Items[i].ImageIndex == 0) ? false : true;
        }

        public void SetItemChecked(int i, bool isCheck)
        {
            if (i < Items.Count)
            {
                if (isCheck == true)
                {
                    Items[i].ImageIndex = 1;

                    if (!CheckedItem.Contains(Items[i]))
                        checkedItem.Add(Items[i]);
                }
                else
                {
                    Items[i].ImageIndex = 0;
                    checkedItem.Remove(Items[i]);
                }
            }
        }

        #endregion

        #region " InitializeComponent "

        private System.Windows.Forms.ImageList imlCheckbox;
        ColumnHeader colCheck;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartListView2));
            this.imlCheckbox = new System.Windows.Forms.ImageList(this.components);
            this.colCheck = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // imlCheckbox
            // 
            this.imlCheckbox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCheckbox.ImageStream")));
            this.imlCheckbox.TransparentColor = System.Drawing.Color.Transparent;
            this.imlCheckbox.Images.SetKeyName(0, "Unchecked");
            this.imlCheckbox.Images.SetKeyName(1, "Checked");
            // 
            // colCheck
            // 
            this.colCheck.Text = " ";
            this.colCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colCheck.Width = 20;

            // 
            // SmartListView
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCheck});
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullRowSelect = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.HideSelection = false;
            this.Size = new System.Drawing.Size(100, 200);
            this.SmallImageList = this.imlCheckbox;
            this.View = System.Windows.Forms.View.Details;
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SmartListView_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
