using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCPeriodPicker : UserControl
    {

        #region [ Data Field ]
        private int iShiftHour = 0;
        #endregion

        #region [ Create & Close ]
        public DUCPeriodPicker()
        {
            InitializeComponent();

            dtpStart.Value = DateTime.Now.Date;
            dtpEnd.Value = DateTime.Now.Date;
        }

        #endregion

        

        #region [ Method ]

        #region [ Control Method ]
        private void rdoButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (((RadioButton)sender).Name)
                {
                    case "rdoByDay":
                        dtpStart.Value = DateTime.Now.Date;
                        dtpEnd.Value = DateTime.Now.Date;
                        break;

                    case "rdoByWeek":
                        dtpStart.Value = DateTime.Now.Date.AddDays(-7);
                        dtpEnd.Value = DateTime.Now.Date;
                        break;

                    case "rdoByMonth":
                        dtpStart.Value = DateTime.Now.Date.AddMonths(-1);
                        dtpEnd.Value = DateTime.Now.Date;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ User Method ]
        /// <summary>
        /// </summary>
        /// <returns>yyyyMMddHHmmss 형태의 날짜 문자열이 반환됩니다.</returns>
        public string GetStartTime()
        {
            return GetStartTime("yyyyMMddHHmmss");
        }
        public string GetStartTime(string sFormat)
        {
            try
            {
                return dtpStart.Value.AddHours(iShiftHour).ToString(sFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>yyyyMMddHHmmss 형태의 날짜 문자열이 반환됩니다.</returns>
        public string GetEndTime()
        {
            return GetEndTime("yyyyMMddHHmmss");
        }
        public string GetEndTime(string sFormat)
        {
            try
            {
                return dtpEnd.Value.AddDays(1).AddSeconds(-1).AddHours(iShiftHour).ToString(sFormat);
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
