using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    /// <summary>
    /// Class Name : DACrux User Control<br/>
    /// Summary    : Sub Listbox Form<br/>
    /// Author     : Miracom YSIM<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmDUCListBox : Form
    {
        public System.Windows.Forms.Form MOMA_FORM = null;
        private DataTable m_dt = null;
        private string m_strValueItem = "";
        private string m_strTextItem = "";
        private string m_strDisplayColumn = "";

        private bool m_IsClear = false;
        private bool isCheckedView = false;
        //private DataTable m_dtOrg = null;

        public CharacterCasing CharacterCasing { get { return txtSearch.CharacterCasing; } set { txtSearch.CharacterCasing = value; } }

        public bool Clear
        {
            get { return m_IsClear; }
            set { m_IsClear = value; }
        }

        /// <summary>
        /// Gets or Sets title string for Listbox and Caption of this Selector.
        /// </summary>
        public string Caption
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        /// <summary>
        /// Gets or Sets whether user can select multi item or single item.
        /// </summary>
        public bool MultiSelect
        {
            get { return lvList.MultiSelect; }
            set
            {
                lvList.MultiSelect = value;
                butChecked.Visible = lvList.MultiSelect;
                butInvert.Visible = lvList.MultiSelect;
                butAll.Visible = lvList.MultiSelect;
            }
        }


        /// <summary>
        /// Gets or Sets Datasource of Listbox Items.
        /// </summary>
        public DataTable DataSource
        {
            get { return m_dt; }
            set 
            {
                m_dt = value;
                
                /// 새로운 DataTable 적용시 무조건 Reset
                m_strValueItem = "";
                m_strTextItem = "";
                m_strDisplayColumn = "";

                /// DataSource에 Null을 할당시 Return
                if (m_dt == null || m_dt.Columns.Count == 0) return;


                // DataSource가 Null이 아닐경우 Value,Text는 처음 Column할당
                m_strValueItem = m_dt.Columns[0].ColumnName;
                m_strTextItem = m_dt.Columns[0].ColumnName;
                FillList();
            }
        }

        public string ValueTextColumn
        {
            get { return m_strTextItem; }
            set 
            { 
                m_strTextItem = value;
                if (m_dt != null && m_dt.Columns.IndexOf(m_strTextItem) < 0)
                {
                    m_strTextItem = m_dt.Columns[0].ColumnName;
                }
            }
        }

        public string ValueColumn
        {
            get { return m_strValueItem; }
            set
            {
                m_strValueItem = value;
                if (m_dt != null && m_dt.Columns.IndexOf(m_strValueItem) < 0)
                {
                    m_strValueItem = m_dt.Columns[0].ColumnName;
                }
            }
        }

        public string DisplayColumn
        {
            get { return m_strDisplayColumn; }
            set
            {
                if (m_strDisplayColumn != value)
                {
                    m_strDisplayColumn = value;
                    FillList();
                }
            }
        }


        public frmDUCListBox()
        {
            InitializeComponent();
        }

        private void FillList(DataView dv = null)
        {
            try
            {
                lvList.BeginUpdate();
                lvList.Clear();

                // 0 : Non Hide, 1 : Value Item Hiden, 2 : Text Item Hidden, 3 : All Hidden 
                int iColumnHiddenState = 0;

                if (dv == null)
                {
                    if (m_dt == null)
                    {
                        return;
                    }
                    else
                    {
                        dv = new DataView(m_dt);
                    }
                }

                /// Display Columns가 있는지 확인하고 없으면 전체 추가
                string[] strColumns = m_strDisplayColumn.Split(new char[] { ',', ';',' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (strColumns.Length == 0)
                {
                    strColumns = new string[ m_dt.Columns.Count ];
                    for(int ic = 0 ; ic  < m_dt.Columns.Count; ic++)
                    {
                        strColumns[ic] = m_dt.Columns[ic].ColumnName;
                    }
                }

                // Display Columns 안에 Value Item과 Text Item이 있는지 확인하고 없으면 Column 생성
                if (Array.IndexOf(strColumns, m_strValueItem) < 0)
                {
                    lvList.Columns.Add(m_strValueItem);
                    iColumnHiddenState += 1;
                }

                if (Array.IndexOf(strColumns, m_strTextItem) < 0)
                {
                    lvList.Columns.Add(m_strTextItem);
                    iColumnHiddenState += 2;
                }

                // Display Columns 생성
                for (int i = 0; i < strColumns.Length; i++)
                {
                    if(m_dt.Columns.IndexOf(strColumns[i]) > -1)
                    {
                        lvList.Columns.Add(strColumns[i]);
                    }
                }

                // lvList Column에 추가된 값만 Row로 채운다.
                lvList.CheckBoxes = lvList.MultiSelect;
                for (int i = 0; i < dv.Count; i++)
                {
                    ListViewItem lvItem = null;
                    
                    for (int ic = 0; ic < lvList.Columns.Count; ic++)
                    {
                        if (ic == 0)
                            lvItem = new ListViewItem(dv[i][lvList.Columns[ic].Text].ToString());
                        else
                            lvItem.SubItems.Add(dv[i][lvList.Columns[ic].Text].ToString());

                        lvItem.SubItems[lvItem.SubItems.Count - 1].Name = lvList.Columns[ic].Text;
                    }

                    lvList.Items.Add(lvItem);
                }
                
                foreach (ColumnHeader header in lvList.Columns)
                {
                    switch (iColumnHiddenState)
                    {
                        // 상황에 맞게 Colunm을 숨긴다.
                        case 1:
                        case 2:
                            if (header.Index == 0)
                            {
                                if (lvList.MultiSelect)
                                    header.Width = 18;
                                else
                                    header.Width = 0;

                                continue;
                            }
                            break;
                        case 3:
                            if (header.Index < 2)
                            {
                                if (lvList.MultiSelect && header.Index == 0)
                                    header.Width = 18;
                                else
                                    header.Width = 0;

                                continue;
                            }
                            break;
                    }

                    header.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
                lvList.EndUpdate();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmDUCListBox_Deactivate(object sender, EventArgs e)
        {
            if (this.tBar.Visible)
                this.tBar.Visible = !this.tBar.Visible;
            this.Hide();
        }


        private void frmDUCListBox_Activated(object sender, EventArgs e)
        {
            try
            {
                m_IsClear = false;
                lvList.Focus();
            }
            catch
            {
                // 예외무시
            }
        }


        /// <summary>
        /// Clear All Items
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>N/A</returns>
        public void ClearItems()
        {
            txtSearch.Text = "";
            lvList.Items.Clear();
        }

        /// <summary>
        /// Clear All Items
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>N/A</returns>
        public void ClearCheckedItems()
        {
            foreach (ListViewItem item in lvList.CheckedItems)
                item.Checked = false;
        }

        private void lvList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                butCancel_Click(null, null);
            }
        }

        private void butChecked_Click(object sender, EventArgs e)
        {
            if (lvList.CheckedItems == null || lvList.CheckedItems.Count < 1)
                return;

            isCheckedView = !isCheckedView;

            ListViewItem[] lvItem = new ListViewItem[lvList.CheckedItems.Count];
            lvList.CheckedItems.CopyTo(lvItem, 0);

            if (isCheckedView)
            {
                ClearItems();
                lvList.Items.AddRange(lvItem);
            }
        }

        private void butInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvList.Items.Count; i++)
            {
                lvList.Items[i].Checked = !lvList.Items[i].Checked;
            }
        }

        private void butAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvList.Items.Count; i++)
            {
                lvList.Items[i].Checked = true;
            }
        }
        
        private void butClear_Click(object sender, EventArgs e)
        {
            m_IsClear = true;
            this.Hide();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void butSearch_Click(object sender, EventArgs e)
        {
            string strKeyword = txtSearch.Text.Trim();
            DataView dv = null;
            try
            {
                if (m_dt == null) return;

                dv = new DataView(m_dt);
                dv.RowFilter = string.Format("Convert({0},'System.String') LIKE '%{1}%'", m_strTextItem, strKeyword);
                FillList(dv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Search is not supported.[{0}]", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dv != null) dv.Dispose();
                dv = null;
            }
        }

        private void lvList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                tBar.Visible = !tBar.Visible;
            }
        }

        private void tBar_Scroll(object sender, EventArgs e)
        {
            this.Opacity = (double)(tBar.Value / 100d);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butSearch_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                butCancel_Click(null, null);
            }
        }

        private void frmDUCListBox_Shown(object sender, EventArgs e)
        {
            ToolTip ToolTip01 = new ToolTip();
            ToolTip01.AutoPopDelay = 5000;
            ToolTip01.InitialDelay = 1000;
            ToolTip01.ReshowDelay = 500;

            ToolTip01.SetToolTip(this.butChecked, "Only Checked View");
            ToolTip01.SetToolTip(this.butInvert, "Invert");
            ToolTip01.SetToolTip(this.butAll, "All Check");
            ToolTip01.SetToolTip(this.butClear, "Delete");
            ToolTip01.SetToolTip(this.butCancel, "Close");
            ToolTip01.SetToolTip(this.butSearch, "Filter");


            // 처음 표시될때 List에 포커스가 가지 않으면 스크롤이 먹지 않는다.
            lvList.Focus();
        }

        private void lvList_MouseClick(object sender, MouseEventArgs e)
        {
            if(lvList.MultiSelect)
            {
                lvList.HitTest(e.Location).Item.Checked = !lvList.HitTest(e.Location).Item.Checked;
            }
        }

        private void lvList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Hide();
        }
    }
}