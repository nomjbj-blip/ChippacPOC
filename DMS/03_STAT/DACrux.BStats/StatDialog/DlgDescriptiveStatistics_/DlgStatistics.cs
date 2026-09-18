using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgDescriptiveAnalysis_
{
    

    public partial class DlgStatistics : Form
    {
        #region " MEMBER FIELD "

        #region [ LANGUAGE ]
        string m_Title = "Descriptive Statistics - Statistics";
        string m_Mean = "M&ean";
        string m_MeanSE = "SE of me&an";
        string m_Standarddeviation = "&Standard Deviation";
        string m_Variance = "&Variance";
        string m_Coefficientofvariation = "Coefficient of variation";
        string m_TrimmedMean = "Trimmed mean";
        string m_Sum = "S&um";
        string m_Min = "M&inimum";
        string m_Max = "Ma&ximum";
        string m_Range = "&Range";
        string m_Nnonmissing = "&N nonmissing";
        string m_Nmissing = "N missin&g";
        string m_Ntotal = "N &total";
        string m_CumulativeN = "&Cumulative N";
        string m_Percent = "&Percent";
        string m_CumulativePercent = "Cumu&lative percent";
        string m_Q1 = "&First quartile";
        string m_Median = "M&edian";
        string m_Q3 = "&Third quartile";
        string m_InterquartileRange = "Inter&quartile range";
        string m_Mode = "Mode";
        string m_SumofSquares = "Sum of Squares";
        string m_Skewness = "Ske&wness";
        string m_Kurtosis = "&Kurtosis";
        string m_MSSD = "MSS&D";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";
        #endregion

        #endregion

        #region " CREATOR "
        public DlgStatistics()
        {
            InitializeComponent();
            InitDialog();
            SelectedStatistics oDefault = new SelectedStatistics();
            oDefault.Reset();
            SetSetting(oDefault);
        }
        public DlgStatistics(SelectedStatistics oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedStatistics On_SelectedStatistics;
        #endregion

        #region " METHOD "
        private void InitDialog()
        {
            this.Text = m_Title;
            this.chkMean.Text = m_Mean;
            this.chkMeanSE.Text = m_MeanSE;
            this.chkStandardDeviation.Text = m_Standarddeviation;
            this.chkVariance.Text = m_Variance;
            this.chkCoefficientOfVariation.Text = m_Coefficientofvariation;
            this.chkTrimmedMean.Text = m_TrimmedMean;
            this.chkSum.Text = m_Sum;
            this.chkMinimum.Text = m_Min;
            this.chkMaximum.Text = m_Max;
            this.chkRange.Text = m_Range;
            this.chkNnonmissing.Text = m_Nnonmissing;
            this.chkNmissing.Text = m_Nmissing;
            this.chkNtotal.Text = m_Ntotal;
            this.chkCumulativeN.Text = m_CumulativeN;
            this.chkPercent.Text = m_Percent;
            this.chkCumulativePercent.Text = m_CumulativePercent;
            this.chkQ1.Text = m_Q1;
            this.chkMedian.Text = m_Median;
            this.chkQ3.Text = m_Q3;
            this.chkInterquartileRange.Text = m_InterquartileRange;
            this.chkMode.Text = m_Mode;
            this.chkSumOfSquares.Text = m_SumofSquares;
            this.chkSkewness.Text = m_Skewness;
            this.chkKurtosis.Text = m_Kurtosis;
            this.chkMSSD.Text = m_MSSD;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;


            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }
        
        private void SetSetting(SelectedStatistics oSelected)
        {
            this.chkMean.Checked = oSelected.IsMean;
            this.chkMeanSE.Checked = oSelected.IsMeanSE;
            this.chkStandardDeviation.Checked = oSelected.IsStandarddeviation;
            this.chkVariance.Checked = oSelected.IsVariance;
            this.chkCoefficientOfVariation.Checked = oSelected.IsCoefficientofvariation;
            this.chkTrimmedMean.Checked = oSelected.IsTrimmedMean;
            this.chkSum.Checked = oSelected.IsSum;
            this.chkMinimum.Checked = oSelected.IsMin;
            this.chkMaximum.Checked = oSelected.IsMax;
            this.chkRange.Checked = oSelected.IsRange;
            this.chkNnonmissing.Checked = oSelected.IsNnonmissing;
            this.chkNmissing.Checked = oSelected.IsNmissing;
            this.chkNtotal.Checked = oSelected.IsNtotal;
            this.chkCumulativeN.Checked = oSelected.IsCumulativeN;
            this.chkPercent.Checked = oSelected.IsPercent;
            this.chkCumulativePercent.Checked = oSelected.IsCumulativePercent;
            this.chkQ1.Checked = oSelected.IsQ1;
            this.chkMedian.Checked = oSelected.IsMedian;
            this.chkQ3.Checked = oSelected.IsQ3;
            this.chkInterquartileRange.Checked = oSelected.IsInterquartileRange;
            this.chkMode.Checked = oSelected.IsMode;
            this.chkSumOfSquares.Checked = oSelected.IsSumofSquares;
            this.chkSkewness.Checked = oSelected.IsSkewness;
            this.chkKurtosis.Checked = oSelected.IsKurtosis;
            this.chkMSSD.Checked = oSelected.IsMSSD;
        }

        private SelectedStatistics GetSetting()
        {
            SelectedStatistics oReturn = new SelectedStatistics();
            try
            {
                oReturn.IsCoefficientofvariation = chkCoefficientOfVariation.Checked;
                oReturn.IsCumulativeN = chkCumulativeN.Checked;
                oReturn.IsCumulativePercent = chkCumulativePercent.Checked;
                oReturn.IsInterquartileRange = chkInterquartileRange.Checked;
                oReturn.IsKurtosis = chkKurtosis.Checked;
                oReturn.IsMax = chkMaximum.Checked;
                oReturn.IsMean = chkMean.Checked;
                oReturn.IsMeanSE = chkMeanSE.Checked;
                oReturn.IsMedian = chkMedian.Checked;
                oReturn.IsMin = chkMinimum.Checked;
                oReturn.IsMode = chkMode.Checked;
                oReturn.IsMSSD = chkMSSD.Checked;
                oReturn.IsNmissing = chkNmissing.Checked;
                oReturn.IsNnonmissing = chkNnonmissing.Checked;
                oReturn.IsNtotal = chkNtotal.Checked;
                oReturn.IsPercent = chkPercent.Checked;
                oReturn.IsQ1 = chkQ1.Checked;
                oReturn.IsQ3 = chkQ3.Checked;
                oReturn.IsRange = chkRange.Checked;
                oReturn.IsSkewness = chkSkewness.Checked;
                oReturn.IsStandarddeviation = chkStandardDeviation.Checked;
                oReturn.IsSum = chkSum.Checked;
                oReturn.IsSumofSquares = chkSumOfSquares.Checked;
                oReturn.IsTrimmedMean = chkTrimmedMean.Checked;
                oReturn.IsVariance = chkVariance.Checked;                
                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region " EVENT HANDLER "

        #region [ CLOSING ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            On_SelectedStatistics(GetSetting());
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

    }
}