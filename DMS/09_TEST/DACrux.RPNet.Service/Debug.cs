using System;
using System.Windows.Forms;

namespace DACrux.RPNet.Service
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void Debug_Load(object sender, EventArgs e)
        {
            DACrux.RPNet.Service.DACruxRPNet obj = new DACruxRPNet();
            obj.Start();
        }
    }
}
