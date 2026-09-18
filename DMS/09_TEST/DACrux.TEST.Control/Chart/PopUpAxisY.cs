using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.Control
{
    public partial class PopUpAxisY : Form
    {
        public delegate void Apply(string Y1Max, string Y1Min, string Y1Interval, string Y2Max, string Y2Min, string Y2Interval, bool Y2Used);
        public event Apply On_Apply;

        public PopUpAxisY()
        {
            InitializeComponent();
        }

        public PopUpAxisY(
            string strY1Max,
            string strY1Min,
            string strY1Interval
            )
            : this()
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = false;

            TxtY1Max.Text = strY1Max;
            TxtY1Min.Text = strY1Min;
            TxtY1Interval.Text = strY1Interval;
        }

        public PopUpAxisY(
            string strY1Max,
            string strY1Min,
            string strY1Interval,
            string strY2Max,
            string strY2Min,
            string strY2Interval
            )
            : this()
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;

            TxtY1Max.Text = strY1Max;
            TxtY1Min.Text = strY1Min;
            TxtY1Interval.Text = strY1Interval;

            TxtY2Max.Text = strY2Max;
            TxtY2Min.Text = strY2Min;
            TxtY2Interval.Text = strY2Interval;
        }

        #region [ Event Handler ]

        private void butApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckedValue(TxtY1Max.Text, TxtY1Min.Text, TxtY1Interval.Text))
                    return;

                if (CheckedValue(TxtY2Max.Text, TxtY2Min.Text, TxtY2Interval.Text))
                    return;

                if (On_Apply != null)
                    On_Apply(TxtY1Max.Text, TxtY1Min.Text, TxtY1Interval.Text, TxtY2Max.Text, TxtY2Min.Text, TxtY2Interval.Text, groupBox3.Enabled);
            }
            finally
            {
                this.Close();
            }

        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                const char Delete = (char)8;

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && e.KeyChar != Delete && (e.KeyChar != '-'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            finally
            {
            }
        }

        private void butClose_Click(
            object sender,
            EventArgs e
            )
        {
            this.Close();
        }

        #endregion [ Event Handler ]

        #region [ Method ]
        private bool CheckedValue(
            string strMax,
            string strMin,
            string strInterval
            )
        {
            double dMax = double.NaN;
            double dMin = double.NaN;

            if (!double.TryParse(strMax, out dMax))
                dMax = double.MaxValue;
            if (!double.TryParse(strMin, out dMin))
                dMin = double.MinValue;

            if (dMax < dMin)
                return true;

            return false;
        }

        #endregion [ Method ]

    }
}
