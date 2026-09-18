using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Base
{
    public partial class DACruxUXBasic00 : Form
    {
        public DACruxUXBasic00()
        {
            InitializeComponent();
            if (DesignMode) return;
            UXUtil.Translation(this.Controls);
        }

        protected void DspError(Exception ex)
        {
            DCMH.DspError(ex);
        }

        protected void DspMessage(string sMessage)
        {
            DCMH.DspMessage(sMessage);
        }
    }
}
