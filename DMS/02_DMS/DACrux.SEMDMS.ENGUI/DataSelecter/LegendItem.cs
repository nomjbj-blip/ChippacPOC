using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI.DataSelecter
{
    public partial class LegendItem : UserControl
    {
        public LegendItem()
        {
            InitializeComponent();
        }

        public event EventHandler VisibleCheckedChanged
        {
            add { chkVisible.CheckedChanged += value; }
            remove { chkVisible.CheckedChanged -= value; }
        }

        public event EventHandler ShowLabelCheckedChanged
        {
            add { chkShowLabel.CheckedChanged += value; }
            remove { chkShowLabel.CheckedChanged -= value; }
        }

        public string ItemName
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public Color ItemColor
        {
            get { return pnlItem.BackColor; }
            set { pnlItem.BackColor = value; }
        }

        public bool IsVisibleChecked
        {
            get { return chkVisible.Checked; }
            set { chkVisible.Checked = value; }
        }

        public bool IsShowLabelChecked
        {
            get { return chkShowLabel.Checked; }
            set { chkShowLabel.Checked = value; }
        }

        private void LegendItem_Click(object sender, EventArgs e)
        {
            chkVisible.Checked = !chkVisible.Checked;
            chkShowLabel.Checked = chkVisible.Checked;
        }
    }
}
