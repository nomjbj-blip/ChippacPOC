using System;
using System.Drawing;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgCapaContinuous_
{
    

    /// <summary>
    /// 신뢰구간 부분 일단 숨김....시간음따...
    /// </summary>
    public partial class DlgCapaContinuousOptions : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Capability Analysis - Options";
        string m_Target = "&Target (adds Cpm to table)";
        string m_Ksigma = "Use toleran&ce of K*sigma for capability statistics  K = ";
        string m_PerformAnalysis = "Perform Analysis";
        string m_BetweenWithinAnalysis = "Bet&ween/within analysis";
        string m_OverallAnalysis = "O&verall analysis";
        string m_Display = "Display";
        string m_PartsPerMillion = "&Parts per million";
        string m_Percents = "Pe&rcents";
        string m_CapabilityStats = "Capabi&lity stats (Cp, Pp)";
        string m_BenchmarkZ = "B&enchmark Z's";
        string m_IncludeConfidenceIntervals = "I&nclude confidence intervals";
        string m_ConfidenceLevel = "Con&fidence level";
        string m_ConfidenceIntervals = "Confi&dence intervals";
        string m_UserTitle = "T&itle";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";
        string m_TwoSide = "two-sided";
        string m_Upper = "upper";
        string m_Lower = "lower";

        string m_ErrorConfidenceLevel = "ErrorConfidenceLevel";
        string m_ErrorKsigma = "Use a single numeric constant(sigma tolerance) > 0.";
        string m_ErrorTarget = "Use a single numeric constant(target).";
        #endregion

        #region " CREATOR "

        public DlgCapaContinuousOptions()
        {
            InitializeComponent();
            this.Size = new Size(450, 297);  // 신뢰구간 부분 일단 숨김....시간음따...  나중에 신뢰구간 생기면 풀것
            InitDialog();
            SelectedCapaContinuousOptions oDefault = new SelectedCapaContinuousOptions();
            oDefault.Reset();
            SetSetting(oDefault);
        }

        public DlgCapaContinuousOptions(SelectedCapaContinuousOptions oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedCapaContinuousOptions On_SelectedOptions;
        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            this.Text = m_Title;
            this.lblTarget.Text = m_Target;
            this.lblK.Text = m_Ksigma;
            this.lblPerformAnalysis.Text = m_PerformAnalysis;
            this.chkBetweenWithinAnalysis.Text = m_BetweenWithinAnalysis;
            this.chkOverallAnalysis.Text = m_OverallAnalysis;
            this.lblDisplay.Text = m_Display;
            this.rdoPartsPerMillion.Text = m_PartsPerMillion;
            this.rdoPercents.Text = m_Percents;
            this.rdoCapabilityStats.Text = m_CapabilityStats;
            this.rdoBenchmarkZ.Text = m_BenchmarkZ;
            this.chkIncludeConfidenceIntervals.Text = m_IncludeConfidenceIntervals;
            this.lblConfidenceLevel.Text = m_ConfidenceLevel;
            this.lblConfidenceIntervals.Text = m_ConfidenceIntervals;
            this.lblTitle.Text = m_UserTitle;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;
            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.txtTarget.Leave += new EventHandler(txtTarget_Leave);
            this.txtK.Leave += new EventHandler(txtK_Leave);
            this.txtConfidenceLevel.Leave += new EventHandler(txtConfidenceLevel_Leave);
            this.chkIncludeConfidenceIntervals.CheckedChanged += new EventHandler(chkIncludeConfidenceIntervals_CheckedChanged);

            cboConfidenceIntervals.Items.Clear();
            cboConfidenceIntervals.Items.Add(m_TwoSide);
            cboConfidenceIntervals.Items.Add(m_Lower);
            cboConfidenceIntervals.Items.Add(m_Upper);
            cboConfidenceIntervals.SelectedIndex = 0;
        }

        private void SetSetting(SelectedCapaContinuousOptions oSelected)
        {
            if (double.IsNaN(oSelected.Target))
            {
                this.txtTarget.Text = string.Empty;
            }
            else
            {
                this.txtTarget.Text = oSelected.Target.ToString();
            }            
            this.txtK.Text = oSelected.KsigmaForCapability.ToString();
            this.chkBetweenWithinAnalysis.Checked = oSelected.IsBetweenWithinAnalysis;
            this.chkOverallAnalysis.Checked = oSelected.IsOverallAnalysis;
            this.chkIncludeConfidenceIntervals.Checked = oSelected.IsIncludeConfidenceIntervals;
            switch (oSelected.ResultDisplayType)
            {
                case DisplayType.Parts_per_million:
                    rdoPartsPerMillion.Checked = true;
                    break;
                case DisplayType.Percents:
                    rdoPercents.Checked = true;
                    break;
                default:
                    rdoPartsPerMillion.Checked = true;
                    break;
            }

            switch (oSelected.ResultStatisticType)
            {
                case StatisticType.Capability_stats:
                    rdoCapabilityStats.Checked = true;
                    break;
                case StatisticType.Benchmark_Z:
                    rdoBenchmarkZ.Checked = true;
                    break;
                default:
                    rdoCapabilityStats.Checked = true;
                    break;
            }
            chkIncludeConfidenceIntervals.Checked = oSelected.IsIncludeConfidenceIntervals;
            txtConfidenceLevel.Text = oSelected.ConfidenceLevel.ToString();
            switch (oSelected.ConfidenceIntervals)
            {
                case ConfidenceIntervalsType.Two_Side:
                    cboConfidenceIntervals.SelectedItem = m_TwoSide;
                    break;
                case ConfidenceIntervalsType.Upper:
                    cboConfidenceIntervals.SelectedItem = m_Upper;
                    break;
                case ConfidenceIntervalsType.Lower:
                    cboConfidenceIntervals.SelectedItem = m_Lower;
                    break;
                default:
                    cboConfidenceIntervals.SelectedItem = m_TwoSide;
                    break;
            }
            txtTitle.Text = oSelected.UserTitle;
   
        }
        

        private SelectedCapaContinuousOptions GetSetting()
        {
            SelectedCapaContinuousOptions oReturn = new SelectedCapaContinuousOptions();
            string strTarget = string.Empty;
            string strKsigma = string.Empty;
            string strConfidenceLevel = string.Empty;


            try
            {
                strTarget = txtTarget.Text.Trim();
                if (strTarget != string.Empty)
                    oReturn.Target = Convert.ToDouble(strTarget);
                else
                    oReturn.Target = double.NaN;
                strKsigma = txtK.Text.Trim();
                if (strKsigma != string.Empty)
                    oReturn.KsigmaForCapability = Convert.ToInt32(strKsigma);
                else
                    oReturn.KsigmaForCapability = 6;

                oReturn.IsBetweenWithinAnalysis = chkBetweenWithinAnalysis.Checked;
                oReturn.IsOverallAnalysis = chkOverallAnalysis.Checked;
                if (rdoPartsPerMillion.Checked)
                    oReturn.ResultDisplayType = DisplayType.Parts_per_million;
                else
                    oReturn.ResultDisplayType = DisplayType.Percents;
                if (rdoCapabilityStats.Checked)
                    oReturn.ResultStatisticType = StatisticType.Capability_stats;
                else
                    oReturn.ResultStatisticType = StatisticType.Benchmark_Z;

                if (chkIncludeConfidenceIntervals.Checked)
                {
                    oReturn.IsIncludeConfidenceIntervals = true;
                    strConfidenceLevel = txtConfidenceLevel.Text.Trim();

                    if (strConfidenceLevel != string.Empty)
                        oReturn.ConfidenceLevel = Convert.ToDouble(strConfidenceLevel);
                    else
                        oReturn.ConfidenceLevel = 95.0;
                    switch (cboConfidenceIntervals.SelectedIndex)
                    {
                        case 0:
                            oReturn.ConfidenceIntervals = ConfidenceIntervalsType.Two_Side;
                            break;
                        case 1:
                            oReturn.ConfidenceIntervals = ConfidenceIntervalsType.Lower;
                            break;
                        case 2:
                            oReturn.ConfidenceIntervals = ConfidenceIntervalsType.Upper;
                            break;
                        default:
                            oReturn.ConfidenceIntervals = ConfidenceIntervalsType.Two_Side;
                            break;
                    }
                }
                else
                    oReturn.IsIncludeConfidenceIntervals = false;
                oReturn.UserTitle = txtTitle.Text;                
                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion

        #region " EVENT HANDLER "

        #region [ INPUT LEAVE ]
        void txtConfidenceLevel_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtConfidenceLevel.Text.Trim();
                if (strText == string.Empty)
                    return;
                if(!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorConfidenceLevel);
                    txtConfidenceLevel.Focus();
                    txtConfidenceLevel.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void txtK_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtK.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorKsigma);
                    txtK.Focus();
                    txtK.SelectAll();
                    return;
                }
                else
                {
                    if (dblValue < 0)
                    {
                        MessageBox.Show(m_ErrorKsigma);
                        txtK.Focus();
                        txtK.SelectAll();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void txtTarget_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtTarget.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorTarget);
                    txtTarget.Focus();
                    txtTarget.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ IncludeConfidenceIntervals ]
        void chkIncludeConfidenceIntervals_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblConfidenceIntervals.Enabled = chkIncludeConfidenceIntervals.Checked;
                lblConfidenceLevel.Enabled = chkIncludeConfidenceIntervals.Checked;
                txtConfidenceLevel.Enabled = chkIncludeConfidenceIntervals.Checked;
                cboConfidenceIntervals.Enabled = chkIncludeConfidenceIntervals.Checked;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        #endregion

        #region [ CLOSING ]
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                #region Value Check

                // Confidence Level
                strText = txtConfidenceLevel.Text.Trim();
                if (strText != string.Empty)
                {
                    if (!double.TryParse(strText, out dblValue))
                    {
                        MessageBox.Show(m_ErrorConfidenceLevel);
                        txtConfidenceLevel.Focus();
                        txtConfidenceLevel.SelectAll();
                        return;
                    }
                    
                }

                // K sigma
                strText = txtK.Text.Trim();
                if (strText != string.Empty)
                {
                    if (!double.TryParse(strText, out dblValue))
                    {
                        MessageBox.Show(m_ErrorKsigma);
                        txtK.Focus();
                        txtK.SelectAll();
                        return;
                    }
                    else
                    {
                        if (dblValue < 0)
                        {
                            MessageBox.Show(m_ErrorKsigma);
                            txtK.Focus();
                            txtK.SelectAll();
                            return;
                        }
                    }
                }

                // Target
                strText = txtTarget.Text.Trim();
                if (strText != string.Empty)
                {
                    if (!double.TryParse(strText, out dblValue))
                    {
                        MessageBox.Show(m_ErrorTarget);
                        txtTarget.Focus();
                        txtTarget.SelectAll();
                        return;
                    }
                }
                #endregion
                On_SelectedOptions(GetSetting());
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

    }
}