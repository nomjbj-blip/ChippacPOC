using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI.Dialog
{
    public partial class DlgDataViewInsertRemove : Form
    {
        public enum Type
        {
            CELLS,
            ROW,
            COLUMN
        }

        private Type type;

        public Type CheckedType
        {
            get { return type; }
        }

        public DlgDataViewInsertRemove(string name)
        {
            InitializeComponent();

            this.Text = name;
            grbGroupBox.Text = name;

            type = Type.CELLS;
        }

        private void DlgDataViewInsertRemove_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.L:
            //        type = Type.CELLS;
            //        rbtCells.Checked = true;
            //        rbtRow.Checked = false;
            //        rbtColumn.Checked = false;
            //        break;
            //    case Keys.R:
            //        type = Type.ROW;
            //        rbtCells.Checked = false;
            //        rbtRow.Checked = true;
            //        rbtColumn.Checked = false;
            //        break;
            //    case Keys.C:
            //        type = Type.COLUMN;
            //        rbtCells.Checked = false;
            //        rbtRow.Checked = false;
            //        rbtColumn.Checked = true;
            //        break;
            //}
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            type = ((rbtCells.Checked) ? Type.CELLS : ((rbtRow.Checked) ? Type.ROW : Type.COLUMN));

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}