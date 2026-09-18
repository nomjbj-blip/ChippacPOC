using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgCapaContinuous_
{
    

    public partial class DlgEstimationofStandardDeviation : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Estimation of Standard Deviation";
        string m_WithinSubgroup = "Within subgroup";
        string m_UseUnbiasingConstants = "&Use unbiasing constants";
        string m_Rbar = "&Rbar";
        string m_Sbar = "&Sbar";
        string m_PooledStandarddeviation = "&Pooled standard deviation";
        string m_BetweenSubgroup = "Between subgroups";
        string m_UseMovingRangeofLength = "Use mo&ving range of length :";
        string m_AverageMovingRange = "&Average moving range";
        string m_MedianMovingRange = "Me&dian moving range";
        string m_SquareRootofMSSD = "S&quare root of MSSD";
        string m_UseUnbiasingConstantsOverall = "Use unbiasing constants to calculate ov&erall standard deviation";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        string m_ErrorMovingRangeLength = "Pelease select between 2 and 100";
        #endregion

        #region " CREATOR "

        public DlgEstimationofStandardDeviation()
        {
            InitializeComponent();
            InitDialog();
            SelectedCapaContinuousEstimate oDefault = new SelectedCapaContinuousEstimate();
            oDefault.Reset();
            SetSetting(oDefault);
        }

        public DlgEstimationofStandardDeviation(SelectedCapaContinuousEstimate oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }

        #endregion

        #region " EVENT "
        public event DSelectedCapaContinuousEstimation On_CapaContinuousEstimation;
        #endregion


        #region " METHOD "

        private void InitDialog()
        {
            this.Text = m_Title;
            this.grpWithinSubgroup.Text = m_WithinSubgroup;
            this.chkUseUnbiasingConstants.Text = m_UseUnbiasingConstants;
            this.rdoRbar.Text = m_Rbar;
            this.rdoSbar.Text = m_Sbar;
            this.rdoPooledStd.Text = m_PooledStandarddeviation;
            this.grpBetweenSubgroups.Text = m_BetweenSubgroup;
            this.lblUseMovingRangeofLength.Text = m_UseMovingRangeofLength;
            this.rdoAverageMovingRange.Text = m_AverageMovingRange;
            this.rdoMedianMovingRange.Text = m_MedianMovingRange;
            this.rdoSquarerootOfMSSD.Text = m_SquareRootofMSSD;
            this.chkUseUnbiasingConstantsOverall.Text = m_UseUnbiasingConstantsOverall;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.nudMovingRange.Leave += new EventHandler(nudMovingRange_Leave);
        }

        

        private void SetSetting(SelectedCapaContinuousEstimate oSelected)
        {
            this.chkUseUnbiasingConstants.Checked = oSelected.IsUseUnbiasingWithin;
            switch (oSelected.EstimationWithinSubgroup)
            {
                case EstimationWithin.Rbar:
                    rdoRbar.Checked = true;
                    break;
                case EstimationWithin.Sbar:
                    rdoSbar.Checked = true;
                    break;
                case EstimationWithin.Pooled_standard_deviation:
                    rdoPooledStd.Checked = true;
                    break;
                default:
                    rdoPooledStd.Checked = true;
                    break;
            }

            nudMovingRange.Value = Convert.ToDecimal(oSelected.MovingRangeofLength);

            switch (oSelected.EstimationBetweenSubgroup)
            {
                case EstimationBetween.Average_moving_range:
                    rdoAverageMovingRange.Checked = true;
                    break;
                case EstimationBetween.Median_moving_range:
                    rdoMedianMovingRange.Checked = true;
                    break;
                case EstimationBetween.Square_root_of_MSSD:
                    rdoSquarerootOfMSSD.Checked = true;
                    break;
                default:
                    rdoAverageMovingRange.Checked = true;
                    break;
            }
            chkUseUnbiasingConstantsOverall.Checked = oSelected.IsUseUnbiasingOverall;
        }

        private SelectedCapaContinuousEstimate GetSetting()
        {
            SelectedCapaContinuousEstimate oReturn = new SelectedCapaContinuousEstimate();
            try
            {
                oReturn.IsUseUnbiasingWithin = chkUseUnbiasingConstants.Checked;

                if (rdoRbar.Checked)
                    oReturn.EstimationWithinSubgroup = EstimationWithin.Rbar;
                if (rdoSbar.Checked)
                    oReturn.EstimationWithinSubgroup = EstimationWithin.Sbar;
                if (rdoPooledStd.Checked)
                    oReturn.EstimationWithinSubgroup = EstimationWithin.Pooled_standard_deviation;

                oReturn.MovingRangeofLength = Convert.ToInt32(nudMovingRange.Value);

                if (rdoAverageMovingRange.Checked)
                    oReturn.EstimationBetweenSubgroup = EstimationBetween.Average_moving_range;
                if (rdoMedianMovingRange.Checked)
                    oReturn.EstimationBetweenSubgroup = EstimationBetween.Median_moving_range;
                if (rdoSquarerootOfMSSD.Checked)
                    oReturn.EstimationBetweenSubgroup = EstimationBetween.Square_root_of_MSSD;

                oReturn.IsUseUnbiasingOverall = chkUseUnbiasingConstantsOverall.Checked;
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

        void nudMovingRange_Leave(object sender, EventArgs e)
        {
            int iValue;
            try
            {
                iValue = Convert.ToInt32(nudMovingRange.Value);
                if (iValue < 2 || iValue > 100)
                {
                    MessageBox.Show(m_ErrorMovingRangeLength);
                    nudMovingRange.Focus();
                    return;
                }
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
            On_CapaContinuousEstimation(GetSetting());
            this.DialogResult = DialogResult.OK;
        }
        #endregion

        #endregion
          
         
         



    }
}