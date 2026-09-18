using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.UXBase
{
    public partial class DACruxUXBasic01 : Form
    {

        private string m_FUNC_CODE = string.Empty;
        public DACruxUXBasic01()
        {
            InitializeComponent();
            UXUtil.Translation(this.Controls);
        }

        protected void DspError(string sMessage)
        {
            DCMH.DspError(sMessage);
        }

        protected void DspMessage(string sMessage)
        {
            DCMH.DspMessage(sMessage);
        }

        public string FUNC_CODE
        {
            get
            {
                return m_FUNC_CODE;
            }
            set
            {
                m_FUNC_CODE = value;
            }
        }
    }
}
