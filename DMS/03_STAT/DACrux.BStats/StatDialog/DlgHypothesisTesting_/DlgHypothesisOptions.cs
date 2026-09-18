using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgHypothesisTesting_
{
    public partial class DlgHypothesisOptions : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Hypothesis testing - Options";
        string m_DescriptiveStatisticsText = "&Descriptive statistics";
        string m_ConfidenceIntervalText = "&Confidence interval";
        string m_CriticalValue = "C&riticalValue";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";
        #endregion

        #region " CREATOR "
        public DlgHypothesisOptions()
        {
            InitializeComponent();
            InitDialog();
            SelectedHypothesisTestingOptions oDefault = new SelectedHypothesisTestingOptions();
            oDefault.Reset();
            SetSetting(oDefault);
        }
        public DlgHypothesisOptions(SelectedHypothesisTestingOptions oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion


        #region " EVENT "
        public event DSelectedHypothesisTestingOptions On_SelectedOptions;
        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            this.Text = m_Title;
            this.chkCI.Text = m_ConfidenceIntervalText;
            this.chkDescStat.Text = m_DescriptiveStatisticsText;
            this.chkCriticalValue.Text = m_CriticalValue;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void SetSetting(SelectedHypothesisTestingOptions oSelected)
        {
            this.chkCI.Checked = oSelected.IsConfidenceInterval;
            this.chkDescStat.Checked = oSelected.IsDescriptiveStat;
            this.chkCriticalValue.Checked = oSelected.IsCriticalValue;
        }

        private SelectedHypothesisTestingOptions GetSetting()
        {
            SelectedHypothesisTestingOptions oReturn = new SelectedHypothesisTestingOptions();
            try
            {
                oReturn.IsDescriptiveStat = chkDescStat.Checked;
                oReturn.IsConfidenceInterval = chkCI.Checked;
                oReturn.IsCriticalValue = chkCriticalValue.Checked;
                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 



        #region " EVENT HANDLER "
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            On_SelectedOptions(GetSetting());
            this.DialogResult = DialogResult.OK;
        }
        #endregion



    }

}