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
    /// Class Name : uclPeriodPicker<br/>
    /// Summary    : Period Selection User Control Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-04-16<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    /// 
    
    public partial class uclPeriodPicker : UserControl
    {
        #region Enum

        /// <summary>
        /// Period Type (Daily, Weekly, Monthly, Period)
        /// </summary>
        public enum SelectPeriodType
        {
            Daily, Weekly, Monthly, Period
        }

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public uclPeriodPicker()
        {
            InitializeComponent();

            dtpStart.Value = DateTime.Now.AddDays(-7);
            dtpEnd.Value = DateTime.Now;
        }

        #endregion

        #region Class Members

        // 시작 시간을 08시로 변경
        private int iShiftHour = 8;
        //private int iShiftHour = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Set Shift Base Hour
        /// </summary>
        public int ShiftHour
        {
            set { iShiftHour = value; }
        }

        /// <summary>
        /// Get Selected Start Time (YYYYMMDDHH24MISS)
        /// </summary>
        public string StartDate
        {
            get { return StartDateTime(); }
        }

        /// <summary>
        /// Get Selected End Time (YYYYMMDDHH24MISS)
        /// </summary>
        public string EndDate
        {
            get { return EndDateTime(); }
        }

        /// <summary>
        /// 날짜 선택 유형을 설정합니다.
        /// </summary>
        public SelectPeriodType SetSelectPeriodType
        {
            set
            {
                switch (value)
                {
                    case SelectPeriodType.Daily:
                        rdoByDay.Checked = true;
                        break;
                    case SelectPeriodType.Weekly:
                        rdoByWeek.Checked = true;
                        break;
                    case SelectPeriodType.Monthly:
                        rdoByMonth.Checked = true;
                        break;
                    default:
                        rdoByPeriod.Checked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Get Selected Week No
        /// </summary>
        public string GetWeekNo
        {
            get { return uclWeekPicker1.GetWeekNo; }
        }

        #endregion

        #region Event

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (rdoByPeriod.Checked)
                ResetRefToConditions();
            this.Cursor = Cursors.Default;
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (rdoByDay.Checked || rdoByPeriod.Checked)
                ResetRefToConditions();
            this.Cursor = Cursors.Default;
        }

        private void uclMonthPicker1_MonthValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (rdoByMonth.Checked)
                ResetRefToConditions();
            this.Cursor = Cursors.Default;
        }

        private void uclWeekPicker1_WeekValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (rdoByWeek.Checked)
                ResetRefToConditions();
            this.Cursor = Cursors.Default;
        }

        private void rdoByDay_CheckedChanged(object sender, EventArgs e)
        {
            SetCondition();
        }

        private void rdoByWeek_CheckedChanged(object sender, EventArgs e)
        {
            SetCondition();
        }

        private void rdoByMonth_CheckedChanged(object sender, EventArgs e)
        {
            SetCondition();
        }

        private void rdoByPeriod_CheckedChanged(object sender, EventArgs e)
        {
            SetCondition();
        }

        /// <summary>
        /// Set Control Visible By User Selection
        /// </summary>
        private void SetCondition()
        {
            try
            {
                if (rdoByDay.Checked)
                {
                    dtpEnd.Visible = true;
                    dtpStart.Visible = false;
                    uclWeekPicker1.Visible = false;
                    uclMonthPicker1.Visible = false;
                    lblTild.Visible = false;
                }
                else if (rdoByWeek.Checked)
                {
                    dtpEnd.Visible = false;
                    dtpStart.Visible = false;
                    uclWeekPicker1.Visible = true;
                    uclMonthPicker1.Visible = false;
                    lblTild.Visible = false;
                }
                else if (rdoByMonth.Checked)
                {
                    dtpEnd.Visible = false;
                    dtpStart.Visible = false;
                    uclWeekPicker1.Visible = false;
                    uclMonthPicker1.Visible = true;
                    lblTild.Visible = false;
                }
                else
                {
                    dtpEnd.Visible = true;
                    dtpStart.Visible = true;
                    uclWeekPicker1.Visible = false;
                    uclMonthPicker1.Visible = false;
                    lblTild.Visible = true;
                }

                ResetRefToConditions();
            }
            catch
            {
            }
        }

        #endregion

        #region StartDateTime

        /// <summary>
        /// Return Start DateTime
        /// </summary>
        /// <returns></returns>
        private string StartDateTime()
        {
            string strStartDateTime = string.Empty;
            DateTime dtmTemp;
            DateTime dtmSTemp;

            CultureInfo ci = null;
            string strTodaysWeekNo = null;
            string strSelectedWeekNo = null;
            int iDayDiff;

            try
            {
                if (rdoByDay.Checked)
                {
                    strStartDateTime = dtpEnd.Value.ToString("yyyyMMdd") + iShiftHour.ToString().PadLeft(2, '0') + "3000";  // 시작 시간을 083000 으로
                }
                else if (rdoByWeek.Checked)
                {
                    ci = new CultureInfo("en-us");

                    strTodaysWeekNo = ci.Calendar.GetWeekOfYear(DateTime.Now, ci.DateTimeFormat.CalendarWeekRule
                        , ci.DateTimeFormat.FirstDayOfWeek).ToString();

                    int nYear = Convert.ToInt16(DateTime.Now.Year.ToString()) - Convert.ToInt16(uclWeekPicker1.GetYear);
                    strSelectedWeekNo = (Convert.ToInt16(uclWeekPicker1.GetWeekNo) - (52 * nYear)).ToString();
                    iDayDiff = ((Convert.ToInt16(strSelectedWeekNo) - Convert.ToInt16(strTodaysWeekNo)) * 7);

                    dtmTemp = DateTime.Now.AddDays(iDayDiff);

                    dtmSTemp = dtmTemp.AddDays(-Convert.ToInt32(ci.Calendar.GetDayOfWeek(dtmTemp)));

                    ////if (dtmSTemp > DateTime.Now)
                    ////    dtmSTemp = dtmSTemp.AddYears(-1);

                    strStartDateTime = dtmSTemp.Year.ToString()
                        + dtmSTemp.Month.ToString().PadLeft(2, '0')
                        + dtmSTemp.Day.ToString().PadLeft(2, '0')
                        + iShiftHour.ToString().PadLeft(2, '0') + "3000";   // 시작 시간을 083000 으로
                }
                else if (rdoByMonth.Checked)
                {
                    strStartDateTime = uclMonthPicker1.GetYear + uclMonthPicker1.GetMonth + "01" + iShiftHour.ToString().PadLeft(2, '0') + "3000";  // 시작 시간을 083000 으로
                }
                else
                {
                    strStartDateTime = dtpStart.Value.ToString("yyyyMMdd") + iShiftHour.ToString().PadLeft(2, '0') + "3000";    // 시작 시간을 083000 으로
                }

                return strStartDateTime;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region EndDateTime

        /// <summary>
        /// Return End DateTime
        /// </summary>
        /// <returns></returns>
        private string EndDateTime()
        {
            string strEndDateTime = string.Empty;
            DateTime dtmTemp;
            DateTime dtmETemp;

            CultureInfo ci = null;
            string strTodaysWeekNo = null;
            string strSelectedWeekNo = null;
            int iDayDiff;

            try
            {
                if (rdoByDay.Checked)
                {
                    if (iShiftHour > 0)
                    {
                        dtmTemp = dtpEnd.Value.AddDays(1);
                        strEndDateTime = dtmTemp.ToString("yyyyMMdd") + Convert.ToString(iShiftHour).PadLeft(2, '0') + "2959";   // 끝 시간을 082959 로
                    }
                    else
                    {
                        // 조회 마지막 시간을 08시 59분 59초
                        strEndDateTime = dtpEnd.Value.ToString("yyyyMMdd") + "082959";  // 끝 시간을 082959 로
                    }
                }
                else if (rdoByWeek.Checked)
                {
                    ci = new CultureInfo("en-us");

                    strTodaysWeekNo = ci.Calendar.GetWeekOfYear(DateTime.Now, ci.DateTimeFormat.CalendarWeekRule
                        , ci.DateTimeFormat.FirstDayOfWeek).ToString();

                    strSelectedWeekNo = uclWeekPicker1.GetWeekNo;
                    iDayDiff = ((Convert.ToInt16(strSelectedWeekNo) - Convert.ToInt16(strTodaysWeekNo)) * 7);

                    dtmTemp = DateTime.Now.AddDays(iDayDiff);

                    dtmETemp = dtmTemp.AddDays(6 - Convert.ToInt32(ci.Calendar.GetDayOfWeek(dtmTemp)));

                    if (iShiftHour > 0)
                    {
                        strEndDateTime = uclWeekPicker1.GetYear // dtETemp.AddDays(1).Year.ToString()
                            + dtmETemp.AddDays(1).Month.ToString().PadLeft(2, '0')
                            + dtmETemp.AddDays(1).Day.ToString().PadLeft(2, '0')
                            + Convert.ToString(iShiftHour).PadLeft(2, '0') + "2959";    // 끝 시간을 082959 로
                    }
                    else
                    {
                        strEndDateTime = dtmETemp.Year.ToString() // dtETemp.AddDays(1).Year.ToString()
                            + dtmETemp.Month.ToString().PadLeft(2, '0')
                            + dtmETemp.Day.ToString().PadLeft(2, '0')
                            + "082959"; // 끝 시간을 082959 로
                    }
                }
                else if (rdoByMonth.Checked)
                {
                    dtmTemp = Convert.ToDateTime(uclMonthPicker1.GetYear + "-" + uclMonthPicker1.GetMonth + "-01").AddMonths(1);
                    dtmTemp = dtmTemp.AddDays(-1);

                    if (iShiftHour > 0)
                    {
                        dtmTemp = dtmTemp.AddDays(1);
                        strEndDateTime = dtmTemp.ToString("yyyyMMdd") + Convert.ToString(iShiftHour).PadLeft(2, '0') + "2959";  // 끝 시간을 082959 로
                    }
                    else
                    {
                        strEndDateTime = dtmTemp.ToString("yyyyMMdd") + "082959";   // 끝 시간을 082959 로
                    }
                }
                else
                {
                    if (iShiftHour > 0)
                    {
                        dtmTemp = dtpEnd.Value.AddDays(1);
                        strEndDateTime = dtmTemp.ToString("yyyyMMdd") + Convert.ToString(iShiftHour).PadLeft(2, '0') + "2959";  // 끝 시간을 082959 로
                    }
                    else
                    {
                        strEndDateTime = dtpEnd.Value.ToString("yyyyMMdd") + "082959";  // 끝 시간을 082959 로
                    }
                }
                return strEndDateTime;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region ICondition Members

        Control ctlFrom1 = null;
        Control ctlFrom2 = null;
        Control ctlFrom3 = null;
        Control ctlFrom4 = null;
        Control ctlFrom5 = null;
        Control ctlFrom6 = null;
        Control ctlFrom7 = null;
        Control ctlFrom8 = null;
        Control ctlFrom9 = null;

        Control ctlTo1 = null;
        Control ctlTo2 = null;
        Control ctlTo3 = null;
        Control ctlTo4 = null;
        Control ctlTo5 = null;
        Control ctlTo6 = null;
        Control ctlTo7 = null;
        Control ctlTo8 = null;
        Control ctlTo9 = null;
        Control ctlTo10 = null;
        Control ctlTo11 = null;
        Control ctlTo12 = null;
        Control ctlTo13 = null;
        Control ctlTo14 = null;
        Control ctlTo15 = null;

        private string strValue = string.Empty;
        private List<ICondition> lstRefFromConditions = new List<ICondition>();
        private List<ICondition> lstRefToConditions = new List<ICondition>();

        public void ResetRefToConditions()
        {
            foreach (ICondition condition in lstRefToConditions)
                condition.ResetItems();
        }

        public void ResetItems()
        {

        }

       
        [Browsable(false)]
        public bool MultiSelect
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        public string ConditionValueString
        {
            get { return StartDate + "~" + EndDate; }
        }

        [Browsable(false)]
        public object ConditionValueObject
        {
            get { return (new string[] { StartDate, EndDate }); }
        }

        [Browsable(false)]
        public List<ICondition> ReferFromConditions
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        [Browsable(false)]
        public List<ICondition> ReferToConditions
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_01
        {
            get
            {
                return ctlFrom1;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom1);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom1 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom1); ctlFrom1 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_02
        {
            get
            {
                return ctlFrom2;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom2);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom2 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom2); ctlFrom2 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_03
        {
            get
            {
                return ctlFrom3;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom3);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom3 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom3); ctlFrom3 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_04
        {
            get
            {
                return ctlFrom4;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom4);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom4 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom4); ctlFrom4 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_05
        {
            get
            {
                return ctlFrom5;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom5);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom5 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom5); ctlFrom5 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_06
        {
            get
            {
                return ctlFrom6;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom6);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom6 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom6); ctlFrom6 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_07
        {
            get
            {
                return ctlFrom7;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom7);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom7 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom7); ctlFrom7 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_08
        {
            get
            {
                return ctlFrom8;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom8);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom8 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom8); ctlFrom8 = null; }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_09
        {
            get
            {
                return ctlFrom9;
            }
            set
            {
                if (value is ICondition && !lstRefFromConditions.Contains((ICondition)value))
                {
                    lstRefFromConditions.Remove((ICondition)ctlFrom9);
                    lstRefFromConditions.Add((ICondition)value);

                    ctlFrom9 = value;
                }
                else if (value == null)
                { lstRefFromConditions.Remove((ICondition)ctlFrom9); ctlFrom9 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_01
        {
            get
            {
                return ctlTo1;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo1);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo1 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo1); ctlTo1 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_02
        {
            get
            {
                return ctlTo2;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo2);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo2 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo2); ctlTo2 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_03
        {
            get
            {
                return ctlTo3;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo3);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo3 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo3); ctlTo3 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_04
        {
            get
            {
                return ctlTo4;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo4);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo4 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo4); ctlTo4 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_05
        {
            get
            {
                return ctlTo5;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo5);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo5 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo5); ctlTo5 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_06
        {
            get
            {
                return ctlTo6;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo6);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo6 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo6); ctlTo6 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_07
        {
            get
            {
                return ctlTo7;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo7);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo7 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo7); ctlTo7 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_08
        {
            get
            {
                return ctlTo8;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo8);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo8 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo8); ctlTo8 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_09
        {
            get
            {
                return ctlTo9;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo9);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo9 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo9); ctlTo9 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_10
        {
            get
            {
                return ctlTo10;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo10);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo10 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo10); ctlTo10 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_11
        {
            get
            {
                return ctlTo11;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo11);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo11 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo11); ctlTo11 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_12
        {
            get
            {
                return ctlTo12;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo12);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo12 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo12); ctlTo12 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_13
        {
            get
            {
                return ctlTo13;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo13);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo13 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo13); ctlTo13 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_14
        {
            get
            {
                return ctlTo14;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo14);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo14 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo14); ctlTo14 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_15
        {
            get
            {
                return ctlTo15;
            }
            set
            {
                if (value is ICondition && !lstRefToConditions.Contains((ICondition)value))
                {
                    lstRefToConditions.Remove((ICondition)ctlTo15);
                    lstRefToConditions.Add((ICondition)value);

                    ctlTo15 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((ICondition)ctlTo15); ctlTo15 = null; }
            }
        }

        #endregion
    }
}
