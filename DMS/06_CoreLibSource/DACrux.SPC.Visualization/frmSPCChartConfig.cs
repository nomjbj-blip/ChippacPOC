using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SPC.Visualization
{
    public delegate void ApplyReflash(object sender);

    public partial class frmSPCChartConfig : Form
    {
        public SPCChartConfig m_XBAR_CHART_CFG = null;
        public SPCChartConfig m_SIGMA_CHART_CFG = null;
        public SPCChartConfig m_RANGE_CHART_CFG = null;
        public SPCChartConfig m_RAW_CHART_CFG = null;
        public SPCChartConfig m_EWMA_MV_CHART_CFG = null;
        public SPCChartConfig m_EWMA_R_CHART_CFG = null;
        public SPCChartConfig m_EWMA_S_CHART_CFG = null;
        public SPCChartConfig m_MA_CHART_CFG = null;
        public SPCChartConfig m_MS_CHART_CFG = null;

        public event ApplyReflash OnApplyReflash = null;

        const string XBAR = "XBAR";
        const string SIGMA = "SIGMA";
        const string RANGE = "RANGE";
        const string RAW = "RAW";
        const string EWMA_MV = "EWMA_MV";
        const string EWMA_R = "EWMA_R";
        const string EWMA_S = "EWMA_S";
        const string MA = "MA";
        const string MS = "MS";

        public SPCChartConfig XBAR_CHART_CFG 
        { 
            set 
            {
                m_XBAR_CHART_CFG = value;
            } get { 
                return m_XBAR_CHART_CFG; 
            } 
        }
        public SPCChartConfig SIGMA_CHART_CFG { set { m_SIGMA_CHART_CFG = value; } get { return m_SIGMA_CHART_CFG; } }
        public SPCChartConfig RANGE_CHART_CFG { set { m_RANGE_CHART_CFG = value; } get { return m_RANGE_CHART_CFG; } }
        public SPCChartConfig RAW_CHART_CFG { set { m_RAW_CHART_CFG = value; } get { return m_RAW_CHART_CFG; } }
        public SPCChartConfig EWMA_MV_CHART_CFG { set { m_EWMA_MV_CHART_CFG = value; } get { return m_EWMA_MV_CHART_CFG; } }
        public SPCChartConfig EWMA_R_CHART_CFG { set { m_EWMA_R_CHART_CFG = value; } get { return m_EWMA_R_CHART_CFG; } }
        public SPCChartConfig EWMA_S_CHART_CFG { set { m_EWMA_S_CHART_CFG = value; } get { return m_EWMA_S_CHART_CFG; } }
        public SPCChartConfig MA_CHART_CFG { set { m_MA_CHART_CFG = value; } get { return m_MA_CHART_CFG; } }
        public SPCChartConfig MS_CHART_CFG { set { m_MS_CHART_CFG = value; } get { return m_MS_CHART_CFG; } }

        string m_strLastSelectChartType = string.Empty;

        public frmSPCChartConfig(bool Apply= false)
        {
            InitializeComponent();
            butApply.Visible = Apply;
        }

        private void lstChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SPCChartConfig oSelectedCfg = null;
            try
            {
                /// 1. Before Select
                /// 변경되기 전에 자기 자신의 변수에 입력한다.

                switch(m_strLastSelectChartType)
                {
                    case XBAR:
                        m_XBAR_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case SIGMA:
                        m_SIGMA_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case RANGE:
                        m_RANGE_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case RAW:
                        m_RAW_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case EWMA_MV:
                        m_EWMA_MV_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case EWMA_R:
                        m_EWMA_R_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case EWMA_S:
                        m_EWMA_S_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case MA:
                        m_MA_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                    case MS:
                        m_MS_CHART_CFG = ucChartCfg.GetChartConfig();;
                        break;
                }

                /// 2. 선택이 바뀐 Data를 Control에 적용시켜 보여준다.
                switch (lstChartType.Text.ToUpper())
                {
                    case XBAR:
                        oSelectedCfg = m_XBAR_CHART_CFG;
                        break;
                    case SIGMA:
                        oSelectedCfg = m_SIGMA_CHART_CFG;
                        break;
                    case RANGE:
                        oSelectedCfg = m_RANGE_CHART_CFG;
                        break;
                    case RAW:
                        oSelectedCfg = m_RAW_CHART_CFG;
                        break;
                    case EWMA_MV:
                        oSelectedCfg = m_EWMA_MV_CHART_CFG;
                        break;
                    case EWMA_R:
                        oSelectedCfg = m_EWMA_R_CHART_CFG;
                        break;
                    case EWMA_S:
                        oSelectedCfg = m_EWMA_S_CHART_CFG;
                        break;
                    case MA:
                        oSelectedCfg = m_MA_CHART_CFG;
                        break;
                    case MS:
                        oSelectedCfg = m_MS_CHART_CFG;
                        break;
                }
                if (oSelectedCfg == null) return;
                ucChartCfg.SetChartConfig(oSelectedCfg);
                m_strLastSelectChartType = lstChartType.Text.ToUpper();
                ucChartCfg.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            /// 1. Before Select
            /// 변경되기 전에 자기 자신의 변수에 입력한다.
            /// 
            switch (m_strLastSelectChartType)
            {
                case XBAR:
                    m_XBAR_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case SIGMA:
                    m_SIGMA_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case RANGE:
                    m_RANGE_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case RAW:
                    m_RAW_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case EWMA_MV:
                    m_EWMA_MV_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case EWMA_R:
                    m_EWMA_R_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case EWMA_S:
                    m_EWMA_S_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case MA:
                    m_MA_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
                case MS:
                    m_MS_CHART_CFG = ucChartCfg.GetChartConfig();
                    break;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void ucChartCfg_Load(object sender, EventArgs e)
        {

        }

        private void butApply_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            if (OnApplyReflash != null)
            {
                /// 1. Before Select
                /// 변경되기 전에 자기 자신의 변수에 입력한다.
                /// 
                switch (m_strLastSelectChartType)
                {
                    case XBAR:
                        m_XBAR_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case SIGMA:
                        m_SIGMA_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case RANGE:
                        m_RANGE_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case RAW:
                        m_RAW_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case EWMA_MV:
                        m_EWMA_MV_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case EWMA_R:
                        m_EWMA_R_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case EWMA_S:
                        m_EWMA_S_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case MA:
                        m_MA_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                    case MS:
                        m_MS_CHART_CFG = ucChartCfg.GetChartConfig();
                        break;
                }

                OnApplyReflash(this);
            }
        }

        private void frmSPCChartConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.Retry)
            {
                e.Cancel = true;
            }
        }
    }
}
