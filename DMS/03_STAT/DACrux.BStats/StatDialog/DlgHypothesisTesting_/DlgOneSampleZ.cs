using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgHypothesisTesting_
{
    public partial class DlgOneSampleZ : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "One sample Z test";
        string m_TestMeanText = "&Test mean:";
        string m_StdText = "&Standard deviation:";
        string m_ConfidenceLevelText = "&Confidence level:";
        string m_AlternativeText = "&Alternative:";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";
        string m_ItemLess = "less than";
        string m_ItemNotEqual = "not equal";
        string m_ItemGreater = "greater than";

        string m_ErrorMean = "Use a single numeric constant(Test mean).";
        string m_ErrorStd = "Use a single numeric constant(Standard deviation) > 0.";

        #endregion
        
        #region " CREATOR "
        public DlgOneSampleZ()
        {
            InitializeComponent();
            InitDialog();
            SelectedHypothesisTestingMethods oDefault = new SelectedHypothesisTestingMethods();
            oDefault.Reset();
            SetSetting(oDefault);
        }
        public DlgOneSampleZ(SelectedHypothesisTestingMethods oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }       

        #endregion

        #region " EVENT "
        public event DSelectedHypothesisTestingMethods On_SelectedMethods;
        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            cboAlternative.Items.Clear();
            cboAlternative.Items.Add(m_ItemLess);
            cboAlternative.Items.Add(m_ItemNotEqual);
            cboAlternative.Items.Add(m_ItemGreater);
            cboAlternative.SelectedItem = m_ItemNotEqual;
            this.Text = m_Title;
            this.lblAlternative.Text = m_AlternativeText;
            this.lblConfidenceLevel.Text = m_ConfidenceLevelText;
            this.lblTestMean.Text = m_TestMeanText;
            this.lblStd.Text = m_StdText;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.txtTestMean.Leave += new EventHandler(txtTestMean_Leave);
            this.txtStd.Leave += new EventHandler(txtStd_Leave);
        }

        private void SetSetting(SelectedHypothesisTestingMethods oSelected)
        {
            if (!double.IsNaN(oSelected.TestingMean))
                txtTestMean.Text = oSelected.TestingMean.ToString();
            else
                txtTestMean.Text = string.Empty;
            if (!double.IsNaN(oSelected.TestingStd))
                txtStd.Text = oSelected.TestingStd.ToString();
            else
                txtStd.Text = string.Empty;
            
            if (!double.IsNaN(oSelected.ConfidenceLevel))
                numericUpDown1.Value = Convert.ToDecimal(oSelected.ConfidenceLevel);
            else
                numericUpDown1.Value = 95.00M;
            if (oSelected.Alternative == AlternativeType.Less)
                cboAlternative.SelectedItem = m_ItemLess;
            else if (oSelected.Alternative == AlternativeType.Greater)
                cboAlternative.SelectedItem = m_ItemGreater;
            else
                cboAlternative.SelectedItem = m_ItemNotEqual;
        }

        private SelectedHypothesisTestingMethods GetSetting()
        {
            SelectedHypothesisTestingMethods oReturn = new SelectedHypothesisTestingMethods();
            try
            {
                if (txtTestMean.Text.Trim() == string.Empty)
                    oReturn.TestingMean = double.NaN;
                else
                    oReturn.TestingMean = Convert.ToDouble(txtTestMean.Text.Trim());
                if (txtStd.Text.Trim() == string.Empty)
                    oReturn.TestingStd = double.NaN;
                else
                    oReturn.TestingStd = Convert.ToDouble(txtStd.Text.Trim());

                oReturn.ConfidenceLevel = Convert.ToDouble(numericUpDown1.Value);
                if (cboAlternative.SelectedItem.ToString() == m_ItemLess)
                    oReturn.Alternative = AlternativeType.Less;
                else if (cboAlternative.SelectedItem.ToString() == m_ItemGreater)
                    oReturn.Alternative = AlternativeType.Greater;
                else
                    oReturn.Alternative = AlternativeType.NotEqual;

                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region " EVENT HANDLER "

        void txtStd_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtStd.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorStd);
                    txtStd.Focus();
                    txtStd.SelectAll();
                    return;
                }
                else
                {
                    if (dblValue <= 0)
                    {
                        MessageBox.Show(m_ErrorStd);
                        txtStd.Focus();
                        txtStd.SelectAll();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void txtTestMean_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtTestMean.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorMean);
                    txtTestMean.Focus();
                    txtTestMean.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
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

                // Test mean
                strText = txtTestMean.Text.Trim();
                if (strText != string.Empty)
                {
                    if (!double.TryParse(strText, out dblValue))
                    {
                        MessageBox.Show(m_ErrorMean);
                        txtTestMean.Focus();
                        txtTestMean.SelectAll();
                        return;
                    }

                }

                // Test Std
                strText = txtStd.Text.Trim();
                if (strText != string.Empty)
                {
                    if (!double.TryParse(strText, out dblValue))
                    {
                        MessageBox.Show(m_ErrorStd);
                        txtStd.Focus();
                        txtStd.SelectAll();
                        return;
                    }
                    else
                    {
                        if (dblValue < 0)
                        {
                            MessageBox.Show(m_ErrorStd);
                            txtStd.Focus();
                            txtStd.SelectAll();
                            return;
                        }
                    }
                }               
                #endregion

                On_SelectedMethods(GetSetting());
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