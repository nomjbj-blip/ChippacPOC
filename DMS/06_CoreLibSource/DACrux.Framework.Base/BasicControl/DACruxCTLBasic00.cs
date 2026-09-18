using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Base
{
    public partial class DACruxCTLBasic00 : UserControl
    {
        public DACruxCTLBasic00()
        {
            InitializeComponent();
            try
            {
                UXUtil.Translation(this.Controls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DspError(Exception ex)
        {
            DCMH.DspError(ex);
        }

        //protected void DspError(string sMessage)
        //{
        //    DCMH.DspError(sMessage);
        //}

        protected void DspMessage(string sMessage)
        {
            DCMH.DspMessage(sMessage);
        }
    }
}
