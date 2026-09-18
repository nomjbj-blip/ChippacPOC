using System;
using System.Windows.Forms;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog.DlgDescriptiveAnalysis_
{
    
    public partial class DlgGraph : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Display Descriptive Statistics - Graphs";
        string m_HistogramText = "&Histogram of data";
        string m_HistogramNormalCurveText = "Histogram of data, with &normal curve";
        string m_BoxPlotText = "&Boxplot of data";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";        
        #endregion

        #region " CREATOR "
        public DlgGraph()
        {
            InitializeComponent();
            InitDialog();
            SelectedGraphs oDefault = new SelectedGraphs();
            oDefault.Reset();
            SetSetting(oDefault);
        }
        public DlgGraph(SelectedGraphs oSelected)
        {
            InitializeComponent();
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedGraph On_SelectedGraph;
        #endregion

        #region " METHOD "
        private void InitDialog()
        {
            this.Text = m_Title;
            this.chkHistogram.Text = m_HistogramText;
            this.chkNormalNHistogram.Text = m_HistogramNormalCurveText;
            this.chkBoxplot.Text = m_BoxPlotText;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;

            //this.btnOk.Click += new EventHandler(btnOk_Click);
            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.chkHistogram.CheckedChanged += new EventHandler(chkHistogram_CheckedChanged);
        }

        private void SetSetting(SelectedGraphs oSelected)
        {
            this.chkHistogram.Checked = oSelected.IsHistogram;
            this.chkNormalNHistogram.Checked = oSelected.IsHistogramNNormalCurve;
            this.chkBoxplot.Checked = oSelected.IsBoxPlot;
            this.chkRawDataPlot.Checked = oSelected.IsRawDataPlot;
        }

        private SelectedGraphs GetSetting()
        {
            SelectedGraphs oReturn = new SelectedGraphs();
            try
            {                
                oReturn.IsHistogram = chkHistogram.Checked;
                oReturn.IsHistogramNNormalCurve = chkNormalNHistogram.Checked;
                oReturn.IsBoxPlot = chkBoxplot.Checked;
                oReturn.IsRawDataPlot = chkRawDataPlot.Checked;
                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region " EVENT HANDLER "

        void chkHistogram_CheckedChanged(object sender, EventArgs e)
        {
            chkNormalNHistogram.Enabled = chkHistogram.Checked;
        }

        #region [ CLOSING ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            On_SelectedGraph(GetSetting());
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