using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Service
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void Debug_Load(object sender, EventArgs e)
        {
            DACrux.SEMDMS.Service.DACruxSEMDMS obj = new DACruxSEMDMS();
            obj.Start();
        }
    }
}
