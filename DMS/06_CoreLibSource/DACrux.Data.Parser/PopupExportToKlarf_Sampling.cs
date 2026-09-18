using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Data.Parser
{
    public partial class PopupExportToKlarf_Sampling : UserControl
    {
        private bool _enabledSampling;

        public PopupExportToKlarf_Sampling()
        {
            InitializeComponent();
            EnabledSampling = false;
        }

        private void PopupExportToKlarf_Sampling_Load(object sender, EventArgs e)
        {
            if (Parser == null)
            {
                Enabled = false;
                return;
            }

            lblWaferID.Text = String.Format("{0}_{1:00}_{2}", Parser.LotID, Parser.Wafers[0].Slot, Parser.StepID);
            txtAllCount.Text = txtSampleCount.Text = String.Format("{0:N0}", Parser.GetDefectCount());
        }

        private void UpdateUI()
        {
            txtSampleCount.Enabled = _enabledSampling;
        }

        public bool CheckValid()
        {
            int allCount = 0, sampleCount = 0;

            if (!Int32.TryParse(txtAllCount.Text, out allCount) || allCount <= 0)
            {
                ShowError("Defect 수량이 없거나 잘못되었습니다.");
                return false;
            }

            if (EnabledSampling && (!Int32.TryParse(txtSampleCount.Text, out sampleCount) || sampleCount <= 0))
            {
                ShowError("Defect 수량이 없거나 잘못되었습니다.");
                return false;
            }

            if (EnabledSampling && allCount < sampleCount)
            {
                ShowError("Sample 수량이 전체 Defect 수량보다 많습니다.");
                return false;
            }

            if (EnabledSampling)
                Count = sampleCount;
            else
                Count = allCount;

            return true;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [DefaultValue(false)]
        public bool EnabledSampling
        {
            get { return _enabledSampling; }
            set { _enabledSampling = value; UpdateUI(); }
        }

        public Parser.Klarf.ParserKlarf Parser
        {
            get;
            set;
        }

        public int Count
        {
            get;
            private set;
        }

        public long StepSeq 
        {
            get; 
            set;
        }
    }
}
