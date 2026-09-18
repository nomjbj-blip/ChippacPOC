using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : uclPeriodSelection<br/>
    /// Summary    : Week Selection User Control Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-04-16<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class uclWeekPicker : UserControl
    {
        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public uclWeekPicker()
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
        /// Get Selected Week No (1 ~ 52)
        /// </summary>
        public string GetWeekNo
        {
            get { return cboWeek.Text; }
        }

        #endregion

        #region Form Load Event

        /// <summary>
        /// Form Load Event - Initialize Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclWeekPicker_Load(object sender, EventArgs e)
        {
            int iYear;
            CultureInfo ci = null;

            try
            {
                ci = new CultureInfo("en-us");

                cboYear.Items.Clear();
                cboWeek.Items.Clear();

                iYear = DateTime.Now.AddYears(-5).Year;

                for (int i = iYear; i <= (iYear + 5); i++)
                    cboYear.Items.Add(i.ToString());

                cboYear.SelectedIndex = 5;

                for (int i = 1; i <= 54; i++)
                    cboWeek.Items.Add(i.ToString().PadLeft(2, '0'));

                cboWeek.Text = ci.Calendar.GetWeekOfYear(DateTime.Now, ci.DateTimeFormat.CalendarWeekRule
                                , ci.DateTimeFormat.FirstDayOfWeek).ToString().PadLeft(2, '0');
            }
            catch
            {
            }
        }

        #endregion

        #region Week Value Event
        public event EventHandler WeekValueChanged;
        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekValueChanged != null)
                WeekValueChanged(this, e);
        }

        private void cboWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekValueChanged != null)
                WeekValueChanged(this, e);
        }
        #endregion
    }
}
