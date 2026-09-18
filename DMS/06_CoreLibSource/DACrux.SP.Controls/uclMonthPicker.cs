using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : uclMonthPicker<br/>
    /// Summary    : Month Selection User Control Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-04-16<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class uclMonthPicker : UserControl
    {
        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public uclMonthPicker()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get Selected Year
        /// </summary>
        public string GetYear
        {
            get { return cboYear.Text; }
        }

        /// <summary>
        /// Get Selected Month (01 ~ 12)
        /// </summary>
        public string GetMonth
        {
            get { return cboMonth.Text; }
        }

        #endregion

        #region Form Load Event

        /// <summary>
        /// Form Load Event - Initialize Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclMonthPicker_Load(object sender, EventArgs e)
        {
            int iYear;
            int iMonth;

            try
            {
                cboYear.Items.Clear();
                cboMonth.Items.Clear();

                iYear = DateTime.Now.AddYears(-5).Year;
                iMonth = DateTime.Now.Month;

                for (int i = iYear; i <= (iYear + 5); i++)
                    cboYear.Items.Add(i.ToString());

                cboYear.SelectedIndex = 5;

                for (int i = 1; i <= 12; i++)
                    cboMonth.Items.Add(i.ToString().PadLeft(2, '0'));

                cboMonth.SelectedIndex = iMonth - 1;
            }
            catch
            {
            }
        }

        #endregion

        #region Month Value Event
        public event EventHandler MonthValueChanged;
        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MonthValueChanged != null)
                MonthValueChanged(this, e);
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MonthValueChanged != null)
                MonthValueChanged(this, e);
        }
        #endregion

    }
}
