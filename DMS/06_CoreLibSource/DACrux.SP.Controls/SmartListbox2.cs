using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : SmartListbox<br/>
    /// Summary    : SmartListbox Control<br/>
    /// Author     : Miracom Hyungsuk, Yang<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class SmartListbox2 : Form
    {
        #region " FIELD "

        private bool isCheckAll = false;
        private bool isMultiSelect = false;
        private DataTable m_dt = new DataTable();
        private string m_colName = string.Empty;
        private string m_caption = string.Empty;
        public int m_maxWidth = 0;

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets or Sets title string for Listbox and Caption of this Selector.
        /// </summary>
        public string Caption
        {
            get
            {
                return m_caption;
            }
            set
            {
                m_caption = value;
                lblTitle.Text = m_caption;
            }
        }

        /// <summary>
        /// Gets or Sets Datasource of Listbox Items.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return m_dt;
            }
            set
            {
                m_dt = value;

                if (m_dt != null)
                {
                    if (m_dt.Columns.Count < 2)
                    {
                        lvList.Columns[1].Width = lvList.Columns[1].Width + lvList.Columns[2].Width;
                        lvList.Columns[2].Width = 0;
                    }

                    if (m_dt == null || m_dt.Rows.Count < 1)
                        return;

                    int iPrefferedWidth = 0;
                    int iCurrentWidth = 0;
                    lvList.Items.Clear();
                    lvList.CheckedItem.Clear();
                    ListViewItem lvItem = null;
                    Graphics g = lvList.CreateGraphics();
                    Font font = this.Font;
                    if(m_dt.Columns.Count > 1)
                    {
                        foreach (DataRow dr in m_dt.Rows)
                        {
                            lvItem = new ListViewItem("", 0);
                            lvItem.SubItems.Add(dr[0].ToString());

                            iCurrentWidth = lvList.Columns[1].Width;
                            iPrefferedWidth = (int)g.MeasureString(dr[0].ToString(), font).Width + lvList.SmallImageList.ImageSize.Width;

                            if(iCurrentWidth < iPrefferedWidth)
                                lvList.Columns[1].Width = iPrefferedWidth;

                            lvItem.SubItems.Add(dr[1].ToString());
                            iCurrentWidth = lvList.Columns[2].Width;
                            iPrefferedWidth = (int)g.MeasureString(dr[1].ToString(), font).Width;
                            
                            if(iCurrentWidth < iPrefferedWidth)
                                lvList.Columns[2].Width = iPrefferedWidth;

                            lvList.Items.Add(lvItem);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in m_dt.Rows)
                        {
                            lvItem = new ListViewItem("", 0);
                            lvItem.SubItems.Add(dr[0].ToString());

                            iCurrentWidth = lvList.Columns[1].Width;
                            iPrefferedWidth = (int)g.MeasureString(dr[0].ToString(), font).Width + lvList.SmallImageList.ImageSize.Width;

                            if (iCurrentWidth < iPrefferedWidth)
                                lvList.Columns[1].Width = iPrefferedWidth;

                            lvList.Items.Add(lvItem);
                        }
                    }

                    m_maxWidth = lvList.Columns[0].Width + lvList.Columns[1].Width + lvList.Columns[2].Width + 22;
                    g.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets or Sets whether user can select multi item or single item.
        /// </summary>
        public bool MultiSelect
        {
            get
            {
                return lvList.MultiSelect;
            }
            set
            {
                lvList.MultiSelect = value;
                isMultiSelect = lvList.MultiSelect;
            }
        }

        #endregion

        #region " CREATOR "

        public SmartListbox2()
        {
            InitializeComponent();

            txtSearch.Text = "";
            //lvList.SmallImageList = imlCheckbox;

            lvList.Columns.Add("Code", 30, HorizontalAlignment.Left);
            lvList.Columns.Add("Desc", 50, HorizontalAlignment.Left);

            lvList.ItemSelected += new EventHandler(OnItemSelected);
        }

        #endregion

        #region " EVENT HANDLER "

        private void OnItemSelected(object sender, EventArgs e)
        {
            if (!isMultiSelect)
            {
                SmartListView2.SmartListViewItemSeletedEventArgs args = e as SmartListView2.SmartListViewItemSeletedEventArgs;

                lvList.CheckedItem.Clear();
                lvList.CheckedItem.Add(args.Item);

                Visible = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ListViewItem lvItem = null;
            DataRow[] drItem = null;
            string strKeyword = txtSearch.Text.Trim();

            if (strKeyword == string.Empty)
            {
                this.DataSource = m_dt;
                return;
            }

            if (m_dt.Columns.Count > 1)
            {
                strKeyword = m_dt.Columns[0] + " LIKE '%" + strKeyword + "%' or " + m_dt.Columns[1] + " LIKE '%" + strKeyword + "%' ";
            }
            else if (m_dt.Columns.Count == 1)
            {
                strKeyword = m_dt.Columns[0] + " LIKE '%" + strKeyword + "%' ";
            }
            else
            {
                MessageBox.Show("Search is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            drItem = m_dt.Select(strKeyword);
            lvList.Items.Clear();
            lvList.CheckedItem.Clear();
            for (int i = 0; i < drItem.Length; i++)
            {
                lvItem = new ListViewItem("", 0);
                lvItem.SubItems.Add(drItem[i].ItemArray[0].ToString());
                if (m_dt.Columns.Count != 1)
                    lvItem.SubItems.Add(drItem[i].ItemArray[1].ToString());
                lvList.Items.Add(lvItem);
            }  
        }

        private void btnAllItem_Click(object sender, EventArgs e)
        {
            isCheckAll = !isCheckAll;
            if (isCheckAll == true)
                lvList.CheckedItem.Clear();
            for (int i = 0; i < lvList.Items.Count; i++)
            {
                lvList.SetItemChecked(i, isCheckAll);
            }
        }

        private void btnChecked_Click(object sender, EventArgs e)
        {
            ListViewItem[] lvItem = null;
            DataRow[] drItem = null;
            string strKeyword = string.Empty;
            if (lvList.Items.Count.Equals(lvList.CheckedItem.Count))
            {
                return;
            }
            else if (lvList.CheckedItem.Count > 0)
            {
                lvItem = new ListViewItem[lvList.CheckedItem.Count];
                for (int i = 0; i < lvList.CheckedItem.Count; i++)
                {
                    strKeyword = m_dt.Columns[0] + "= '" + lvList.CheckedItem[i].SubItems[1].Text.ToString().Trim() + "' ";
                    drItem = m_dt.Select(strKeyword);
                    lvItem[i] = new ListViewItem("", 0);
                    lvItem[i].SubItems.Add(drItem[0].ItemArray[0].ToString());
                    if (m_dt.Columns.Count > 1)
                        lvItem[i].SubItems.Add(drItem[0].ItemArray[1].ToString());
                }
                lvList.Items.Clear();
                lvList.CheckedItem.Clear();
                for (int i = 0; i < lvItem.Length; i++)
                {
                    lvList.Items.Add(lvItem[i]);
                    lvList.Items[i].Checked = true;
                    lvList.Items[i].ImageIndex = 1;
                    lvList.CheckedItem.Add(lvList.Items[i]);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void SmartListbox_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        private void SmartListbox_VisibleChanged(object sender, EventArgs e)
        {
            btnAllItem.Visible = isMultiSelect;
            btnChecked.Visible = isMultiSelect;
        }

        #endregion

        #region " METHOD "

        public void SetPosition(int x, int y)
        {
            this.SetDisplayRectLocation(x, y);
        }

        public void SetVisible(bool isVisible)
        {
            pnlBack.Visible = isVisible;
        }

        /// <summary>
        /// Get Bigger Value
        /// </summary>
        /// <param name="number1">int</param>
        /// <param name="number2">int</param>
        /// <returns>int</returns>
        private int GetBigger(int number1, int number2)
        {
            return (number1 > number2) ? number1 : number2;
        }

        /// <summary>
        /// Get All Item String
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns></returns>
        public string GetAllItemString()
        {
            string strSelected = string.Empty;

            try
            {
                for (int i = 0; i < lvList.Items.Count; i++)
                {
                    strSelected += string.Format(",{0}", lvList.Items[i].SubItems[1].Text);
                }
                if (strSelected.Length > 0)
                    strSelected = strSelected.Substring(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return strSelected;
        }

        /// <summary>
        /// Get Selected Item Value
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>string</returns>
        public string GetSelectedString()
        {
            string strSelected = string.Empty;
            string[] strTemp;
            int iChange;
            bool bIsNumber = true;
            try
            {
                if (lvList.CheckedItem.Count > 1)
                {
                    if (lvList.CheckedItem.Count == m_dt.Rows.Count )
                    {
                        strSelected = "All";
                    }
                    else
                    {
                        for (int i = 0; i < lvList.CheckedItem.Count; i++)
                        {
                            if (isNumber(lvList.CheckedItem[i].SubItems[1].Text) == false)
                                bIsNumber = false;
                            strSelected += string.Format(",{0}", lvList.CheckedItem[i].SubItems[1].Text);
                        }
                        if (strSelected.Length > 0)
                            strSelected = strSelected.Substring(1);
                        /// <summary>
                        /// Bubble Sort  Made by AndyKuo
                        /// </summary>
                        if (bIsNumber)
                        {
                            strTemp = strSelected.Split(',');
                            for (int i = 0; i < strTemp.Length; i++)
                            {
                                for (int j = i; j < strTemp.Length; j++)
                                {
                                    if (Convert.ToInt16(strTemp[i]) > Convert.ToInt16(strTemp[j]))
                                    {

                                        iChange = Convert.ToInt16(strTemp[i]);
                                        strTemp[i] = strTemp[j];
                                        strTemp[j] = Convert.ToString(iChange);
                                    }
                                }
                            }
                            strSelected = "";
                            for (int i = 0; i < strTemp.Length; i++)
                            {
                                strSelected += string.Format(",{0}", strTemp[i]);
                            }
                            if (strSelected.Length > 0)
                                strSelected = strSelected.Substring(1);
                        }
                    }
                }
                else if (lvList.CheckedItem.Count == 0)
                {
                    strSelected = "";
                }
                else if (lvList.CheckedItem.Count == 1)
                {
                    strSelected = string.Format("{0}", lvList.CheckedItem[0].SubItems[1].Text);
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return strSelected;
        }

        /// <summary>
        /// Gets Selected items indices.
        /// </summary>
        /// <returns></returns>
        internal void GetSelectedIndices(ref List<int> SelectedItemIndices)
        {
            SelectedItemIndices.Clear();

            foreach(ListViewItem lvItem in lvList.CheckedItem)
            {
                SelectedItemIndices.Add(lvItem.Index);
            }
        }

        /// <summary>
        /// Clear All Items
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>N/A</returns>
        public void ClearItems()
        {
            lvList.CheckedItem.Clear();
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
            lvList.CheckedItem.Clear();
        }
        
        /// <summary>
        /// Check Parameter is Number Type or not
        /// </summary>
        /// <param name="strTemp">string</param>
        /// <returns>bool</returns>
        private bool isNumber(string strTemp)
        {
            bool bFlag = true;
            char[] str = strTemp.ToCharArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str[i]))
                {
                    bFlag = false;
                    return bFlag;
                }
            }
            return bFlag;
        }

        #endregion

        private void SmartListbox_Load(object sender, EventArgs e)
        {
            lvList.Focus();
        }
    }
}