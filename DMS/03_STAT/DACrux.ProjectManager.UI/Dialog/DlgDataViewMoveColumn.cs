using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewMoveColumn : Form
    {
        #region " ENUM "

        public enum Option
        {
            First,
            Last,
            Previous
        }

        #endregion

        #region " MEMBER FIELD "

        private int iLastValidColumnInfo;
        private List<int> lstSelectedColumnIndices = null;
        private List<DataView.ColumnInfo> lstColumnInfo = null;

        #endregion

        #region " PROPERTY "

        public Option CheckedOption
        {
            get
            {
                if (rbtFirst.Checked) return Option.First;
                else if (rbtLast.Checked) return Option.Last;
                else return Option.Previous;
            }
        }

        public int PreviousColumnIndex
        {
            get
            {
                if (CheckedOption == Option.Previous && lblSelectedColumnID.Text.Length > 0)
                    return Convert.ToInt32(lblSelectedColumnID.Text.Replace("C", "")) - 1;
                else
                    return -1;
            }
        }

        #endregion

        #region " CREATOR "

        public DlgDataViewMoveColumn(List<DataView.ColumnInfo> totalColumns, int lastValidColumnIndex, List<int> selectedColumnIndices)
        {
            InitializeComponent();

            this.lstColumnInfo = totalColumns;
            this.iLastValidColumnInfo = lastValidColumnIndex;
            this.lstSelectedColumnIndices = selectedColumnIndices;
        }

        #endregion

        #region " EVENT HANDLER "

        private void DlgDataViewMoveColumn_Load(object sender, EventArgs e)
        {
            if (lstColumnInfo[0].ColumnIndex == lstSelectedColumnIndices[0])
                rbtFirst.Enabled = false;

            if (iLastValidColumnInfo == lstSelectedColumnIndices[lstSelectedColumnIndices.Count - 1])
                rbtLast.Enabled = false;

            ListViewItem lvItem;
            foreach (DataView.ColumnInfo columnInfo in lstColumnInfo)
            {
                if (!lstSelectedColumnIndices.Contains(columnInfo.ColumnIndex) && !lstSelectedColumnIndices.Contains(columnInfo.ColumnIndex+1))
                {
                    lvItem = new ListViewItem("C" + (columnInfo.ColumnIndex + 1).ToString());
                    lvItem.SubItems.Add(columnInfo.ColumnName);

                    lvColumnList.Items.Add(lvItem);
                }
            }

            lvColumnList.Enabled = rbtPrevious.Checked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lvColumnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvColumnList.SelectedItems.Count > 0)
            {
                lblSelectedColumnID.Text = lvColumnList.SelectedItems[0].Text;
                lvColumnList.Focus();
                lvColumnList.SelectedItems[0].Selected = true;
                lvColumnList.SelectedItems[0].Focused = true;
            }
        }

        private void rbtPrevious_CheckedChanged(object sender, EventArgs e)
        {
            lvColumnList.Enabled = rbtPrevious.Checked;
        }

        #endregion
    }
}