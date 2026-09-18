using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCListBox : UserControl
    {
        public event EventHandler OnSelectedIndexChanged;
        public event EventHandler OnSelectedValueChanged;
        public event EventHandler OnSelectedValueDoubleClick;
        public event EventHandler EnterTextBox;

        private static readonly String TitleSpace = "     ";

        private bool _isBinding = false;

        public DUCListBox()
        {
            InitializeComponent();

            ClearTextBoxAtDataBinding = true;
        }

        #region Event Handler

        //--
        
        private void txtItem_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = this.lbItems.DataSource as DataTable;

            if (dt != null && !String.IsNullOrEmpty(DisplayMember))
            {
                string[] arr = txtItem.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arr.Length; i++)
                    arr[i] = String.Format("{0} LIKE '%{1}%'", DisplayMember, arr[i].Trim());

                dt.DefaultView.RowFilter = String.Join(" OR ", arr);
            }
            else
            {
                GetText();
            }
        }

        //--

        private void lbTiTle_Click(
            object sender, 
            EventArgs e
            )
        {
            txtItem.Text = String.Empty;
            lbItems.SelectedItems.Clear();
        }

        private void txtItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OnEnterTextBox(EventArgs.Empty);
        }

        //--

        /// <summary>
        /// Selected Index Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbItems_SelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (_isBinding)
                return;

            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged(this, EventArgs.Empty);
        }

        //--

        /// <summary>
        /// Selected Value Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbItems_SelectedValueChanged(
            object sender,
            EventArgs e
            )
        {
            if (_isBinding)
                return;

            if (OnSelectedValueChanged != null)
                OnSelectedValueChanged(this, EventArgs.Empty);
        }

        //--

        private void lbItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_isBinding)
                return;

            if (OnSelectedValueDoubleClick != null)
                OnSelectedValueDoubleClick(this, EventArgs.Empty);
        }

        #endregion Event Handler

        #region Method

        protected virtual void OnEnterTextBox(EventArgs e)
        {
            if (EnterTextBox != null)
                EnterTextBox(this, e);
        }

        public void DataBinding(
            )
        {
            lbItems.SelectedItems.Clear();
        }

        /// <summary>
        /// 구분자를 이용한 Multi Selection Mode 지원
        /// </summary>
        private void GetText()
        {
            String[] tmp = txtItem.Text.Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries);

            lbItems.SelectedItems.Clear();
            for (int tmpIdx = 0; tmpIdx < tmp.Length; tmpIdx++)
            {
                for (int idx = 0; idx < lbItems.Items.Count; idx++)
                {
                    if (lbItems.Items[idx] is DataRowView)
                    {
                        if ((lbItems.Items[idx] as DataRowView)[DisplayMember].ToString().Contains(tmp[tmpIdx].ToUpper()))
                        {
                            lbItems.SetSelected(idx, true);
                            break;
                        }
                    }
                    else if (lbItems.Items[idx] is ListViewItem)
                    {
                        if ((lbItems.Items[idx] as ListViewItem).Text.Contains(tmp[tmpIdx].ToUpper()))
                        {
                            lbItems.SetSelected(idx, true);
                            break;
                        }
                    }
                    else if (lbItems.Items[idx] is String)
                    {
                        if ((lbItems.Items[idx] as String).Contains(tmp[tmpIdx].ToUpper()))
                        {
                            lbItems.SetSelected(idx, true);
                            break;
                        }
                    }
                }
            }
        }

        //--

        private object[] GetSelectedValues()
        {
            if (lbItems.SelectedItems == null
                || lbItems.SelectedItems.Count < 0)
                return null;


            List<object> lstValue = new List<object>();
            for (int idx = 0; idx < lbItems.SelectedItems.Count; idx++)
            {
                DataRowView drv = lbItems.SelectedItems[idx] as DataRowView;
                if (drv != null)
                    lstValue.Add(drv[ValueMember].ToString());
                else lstValue.Add(lbItems.SelectedItems[idx].ToString());
            }

            return lstValue.ToArray();
        }

        private string[] GetSelectedTexts()
        {
            if (lbItems.SelectedItems == null
                || lbItems.SelectedItems.Count < 0)
                return null;


            List<string> lstValue = new List<string>();
            for (int idx = 0; idx < lbItems.SelectedItems.Count; idx++)
            {
                DataRowView drv = lbItems.SelectedItems[idx] as DataRowView;
                lstValue.Add(drv[DisplayMember].ToString());
            }

            return lstValue.ToArray();
        }

        private object GetValue(
            )
        {
            if (lbItems.SelectedValue is DataRowView)
            {
                DataRowView view = lbItems.SelectedValue as DataRowView;
                if (view == null)
                    return string.Empty;

                DataRow row = view.Row;
                return row[ValueMember];
            }
            else
            {
                return lbItems.SelectedValue;
            }
        }

        public void ClearDataSource()
        {
            lbItems.SelectedIndex = -1;
            lbItems.DataSource = null;
        }

        //--

        #endregion Method


        #region Property

        //--

        public String SearchTitle
        {
            get { return this.lbTiTle.Text.Trim(); }
            set { this.lbTiTle.Text = TitleSpace + value; }
        }

        //--

        [Browsable(false)]
        public String SearchText
        {
            get { return this.txtItem.Text; }
            set
            {
                this.txtItem.Text = value;
                GetText();
            }
        }

        //--

        [Browsable(false)]
        public object DataSource
        {
            get { return this.lbItems.DataSource; }
            set {
                this._isBinding = true;
                this.lbItems.DataSource = value;
                this.lbItems.SelectedIndex = -1;

                if (ClearTextBoxAtDataBinding)
                    this.txtItem.Clear();

                this._isBinding = false;

                if (!String.IsNullOrEmpty(txtItem.Text))
                    txtItem_TextChanged(txtItem, EventArgs.Empty);
            }
        }

        //--

        public String DisplayMember
        {
            get { return this.lbItems.DisplayMember; }
            set { this.lbItems.DisplayMember = value; }
        }

        //--

        public String ValueMember
        {
            get { return this.lbItems.ValueMember; }
            set { this.lbItems.ValueMember = value; }
        }

        //--

        public System.Windows.Forms.ListBox.ObjectCollection Items
        {
            get { return this.lbItems.Items; }
        }

        //--

        [DefaultValue(SelectionMode.One)]
        public SelectionMode SelectionMode
        {
            get { return this.lbItems.SelectionMode; }
            set { this.lbItems.SelectionMode = value; }
        }

        //--

        public int SelectedIndex
        {
            get { return this.lbItems.SelectedIndex; }
            set { this.lbItems.SelectedIndex = value; }
        }

        //--

        [Browsable(false)]
        public System.Windows.Forms.ListBox.SelectedIndexCollection SelectedIndexs
        {
            get { return this.lbItems.SelectedIndices; }
        }

        //--

        [Browsable(false)]
        public object SelectedItem
        {
            get { return this.lbItems.SelectedItem; }
            set { this.lbItems.SelectedItem = value; }
        }

        //--

        [Browsable(false)]
        public System.Windows.Forms.ListBox.SelectedObjectCollection SelectedItems
        {
            get { return this.lbItems.SelectedItems; }
        }

        //--

        [Browsable(false)]
        public object SelectedValue
        {
            get { return GetValue(); }
            set { this.lbItems.SelectedValue = value; }
        }

        //--

        [Browsable(false)]
        public object[] SelectedValues
        {
            get { return GetSelectedValues(); }
        }

        //--

        [Browsable(false)]
        public object[] SelectedTexts
        {
            get { return GetSelectedTexts(); }
        }

        [Browsable(false)]
        public ListBox ListBox
        {
            get { return lbItems; }
        }

        [Browsable(false)]
        public new string Text
        {
            get { return lbItems.Text; }
            set { lbItems.Text = value; }
        }

        [DefaultValue(true)]
        public bool ClearTextBoxAtDataBinding
        {
            get;
            set;
        }

        #endregion Property


    }
}
