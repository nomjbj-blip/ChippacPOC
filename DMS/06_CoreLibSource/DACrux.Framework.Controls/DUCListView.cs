using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    /// <summary>
    /// Class Name : DUCListView<br/>
    /// Summary    : DUCListView Control<br/>
    /// Author     : YSIM<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class DUCListView : System.Windows.Forms.ListView
    {
        #region [ INNER CLASS ]
        public class DUCListViewItemSeletedEventArgs : EventArgs
        {
            public ListViewItem Item;
        };

        #endregion

        #region [ PROPERTY ]

        [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Setup")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable DataSource
        {
            get
            {
                return this.dbjDataSource;
            }
            set
            {
                dbjDataSource = value;

                this.Clear();

                if (dbjDataSource != null && dbjDataSource.Rows.Count > 0)
                {
                    this.BeginUpdate();
    
                    base.CheckBoxes = base.MultiSelect;

                    if (base.MultiSelect)
                    {
                        this.Columns.Add("");
                        this.Columns[0].Width = 18;
                    }

                    int nColCount = dbjDataSource.Columns.Count;
                    if (chCustomColumns != null && dbjDataSource.Columns.Count < chCustomColumns.Count)
                        nColCount = chCustomColumns.Count;

                    for (int i = 0; i < nColCount; i++)
                    {
                        if (chCustomColumns != null && i < chCustomColumns.Count)
                            this.Columns.Add(chCustomColumns[i]);
                        else
                            this.Columns.Add(dbjDataSource.Columns[i].ColumnName);
                    }

                    foreach (DataRow dr in dbjDataSource.Rows)
                    {
                        int nStartIndex = 0;
                        ListViewItem lvItem = null;

                        if (base.MultiSelect)
                        {
                            lvItem = new ListViewItem("");
                        }
                        else
                        {
                            lvItem = new ListViewItem(dr[0].ToString());
                            nStartIndex++;
                        }

                        for (int i = nStartIndex; i < dr.Table.Columns.Count; i++)
                        {
                            lvItem.SubItems.Add(dr[i].ToString());
                        }

                        this.Items.Add(lvItem);
                    }

                    foreach (ColumnHeader header in this.Columns)
                    {
                        if (this.MultiSelect && header.Index == 0)
                            continue;

                        header.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }

                    this.EndUpdate();
                }
            }
        }

        #endregion

        #region [ MEMBER FIELD ]

        private DataTable dbjDataSource = null;
        private List<ColumnHeader> chCustomColumns = null;

        #endregion

        #region [ EVENT ]
        public event ListViewItemSelectionChangedEventHandler ItemSelected = null;

        #endregion

        #region [ METHOD ]

        public DUCListView()
        {
            InitializeComponent();

            SaveCustomColumns();
        }

        public DUCListView(IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            SaveCustomColumns();
        }

        private void SaveCustomColumns()
        {
            chCustomColumns = new List<ColumnHeader>();
            for (int i = 0; i < this.Columns.Count; i++)
                chCustomColumns.Add(this.Columns[i]);
        }

        private void DUCListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (FocusedItem == null)
                return;

            if (base.MultiSelect)
            {
                FocusedItem.Checked = !FocusedItem.Checked;
            }
        }

        private void DUCListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (ItemSelected != null)
                {
                    ItemSelected(this, e);
                }
            }
        }

        public int GetViewWidth()
        {
            int nViewWidth = 0;

            foreach (ColumnHeader header in this.Columns)
                nViewWidth += header.Width;

            return nViewWidth;
        }

        #endregion
    }
}
