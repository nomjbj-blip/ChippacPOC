using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Map
{
    public partial class ZoneIDSelectControlPopup : Form
    {
        public ZoneIDSelectControlPopup()
        {
            InitializeComponent();
        }

        public ZoneIDSelectControl Control
        {
            get { return zoneIdCtl; }
            set { zoneIdCtl = value; }
        }

        [Browsable(false)]
        public DefectMap Map
        {
            get { return zoneIdCtl.Map; }
            set { zoneIdCtl.Map = value; }
        }

        private void ZoneIDSelectControlPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;

            if (Map != null)
                Map.VisibleZone = false;

            Map.Redraw();
        }
    }
}
