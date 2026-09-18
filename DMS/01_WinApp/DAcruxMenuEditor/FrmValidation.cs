using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACruxV5
{
    public partial class FrmValidation : Form
    {
        public FrmValidation()
        {
            InitializeComponent();
        }

        public ListBox NotConnected
        {
            get { return lstNotConnected; }
        }

        public ListBox Duplicated
        {
            get { return lstDuplicated; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmValidation_Load(object sender, EventArgs e)
        {
            lblDuplicated.Text += lstDuplicated.Items.Count.ToString();
            lblNotConnected.Text += lstNotConnected.Items.Count.ToString();
        }
    }
}
