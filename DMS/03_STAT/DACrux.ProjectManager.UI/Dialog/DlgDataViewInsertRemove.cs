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