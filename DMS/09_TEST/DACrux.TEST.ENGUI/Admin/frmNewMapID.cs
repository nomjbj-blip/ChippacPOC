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
    public partial class frmNewMapID : DACrux.Framework.Base.DACruxUXBasic01
    {
        public string NewMapID = string.Empty;

        public frmNewMapID()
        {
            InitializeComponent();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            NewMapID = txtMapID.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
