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
    public partial class DlgTaguchiAnalysisOption : Form
    {
        TaguchiAnalysisRule m_Rule = TaguchiAnalysisRule.SN_NOMINAL_BEST_B;
        public TaguchiAnalysisRule TaguchiRule
        {

            get
            {
                return m_Rule;
            }
            set
            {
                m_Rule = value;
                switch (m_Rule)
                {
                    case TaguchiAnalysisRule.SN_LARGER_BETTER:
                        rdoRule01.Checked = true;
                        break;
                    case TaguchiAnalysisRule.SN_NOMINAL_BEST_N:
                        rdoRule02.Checked = true;
                        break;
                    case TaguchiAnalysisRule.SN_NOMINAL_BEST_B:
                        rdoRule03.Checked = true;
                        break;
                    case TaguchiAnalysisRule.SN_SMALLER_BETTER:
                        rdoRule04.Checked = true;
                        break;
                }
            }
        }

        public DlgTaguchiAnalysisOption()
        {
            InitializeComponent();

        }

        private void butOk_Click(object sender, EventArgs e)
        {
            if (rdoRule01.Checked) m_Rule = TaguchiAnalysisRule.SN_LARGER_BETTER;
            if (rdoRule02.Checked) m_Rule = TaguchiAnalysisRule.SN_NOMINAL_BEST_N;
            if (rdoRule03.Checked) m_Rule = TaguchiAnalysisRule.SN_NOMINAL_BEST_B;
            if (rdoRule04.Checked) m_Rule = TaguchiAnalysisRule.SN_SMALLER_BETTER;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
