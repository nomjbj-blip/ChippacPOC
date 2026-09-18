using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    public partial class DlgTaguchiAnalysisDetail : Form
    {
        public DlgTaguchiAnalysisDetail()
        {
            InitializeComponent();
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
