using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgANOVA_
{
    
    public partial class DlgGraph : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "ANOVA - Graphs";
        string m_Histogramofresiduals = "&Histogram of residuals";
        string m_Normalplotofresiduals = "&Normal plot of residuals";
        string m_ResidualsVSfits = "Residuals versus &fits";
        string m_BoxplotofData = "&Boxplots of data";

        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        #region Message
        #endregion


        #endregion


        #region " CREATOR "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSelected"></param>
        public DlgGraph(SelectedAnovaGraph oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedAnovaGraph On_SelectedOptions;
        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                chkHistogramofResiduals.Text = m_Histogramofresiduals;
                chkNormalplot.Text = m_Normalplotofresiduals;
                chkResidualsVsFits.Text = m_ResidualsVSfits;
                chkBoxplot.Text = m_BoxplotofData;
                btnCancel.Text = m_Cancel;
                btnOk.Text = m_Ok;

                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetSetting(SelectedAnovaGraph oSelected)
        {
            try
            {
                chkBoxplot.Checked = oSelected.IsBoxPlot;
                chkHistogramofResiduals.Checked = oSelected.IsGraphHistogramResiduals;
                chkNormalplot.Checked = oSelected.IsGraphProbabilityPlotResiduals;
                chkResidualsVsFits.Checked = oSelected.IsGraphResidualsVsFittedValues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SelectedAnovaGraph GetSetting()
        {
            SelectedAnovaGraph oReturn = new SelectedAnovaGraph();
            try
            {
                oReturn.IsBoxPlot = chkBoxplot.Checked;
                oReturn.IsGraphHistogramResiduals = chkHistogramofResiduals.Checked;
                oReturn.IsGraphProbabilityPlotResiduals = chkNormalplot.Checked;
                oReturn.IsGraphResidualsVsFittedValues = chkResidualsVsFits.Checked;
                return oReturn;
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
            try
            {                
                if(On_SelectedOptions!=null) 
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