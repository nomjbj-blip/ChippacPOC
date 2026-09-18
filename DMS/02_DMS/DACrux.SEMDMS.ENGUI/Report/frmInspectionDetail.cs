using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using System.Globalization;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmInspectionDetail : Form
    {
        #region [ Constructor ]
        public frmInspectionDetail()
        {
            InitializeComponent();
            Utility.FPSpreadUtil.InitSpread(fpSpread1);
        }

        public frmInspectionDetail(
            string date,
            string hour,
            string model, 
            bool bArea
            )
            : this()
        {
            Date = date;
            Hour = hour;
            Model = model;
            AreaFlag = bArea;
        }
        #endregion [ Constructor ]

        #region [ Event Handler ]

        private void frmInspectionDetail_Load(
            object sender, 
            EventArgs e
            )
        {
            DMReport obj = new DMReport();
            Tuple<DateTime, DateTime> dateRange = GetDateRange();
            DataTable dt = obj.GetInspectionDetail(
                DACrux.Base.GlobalVariable.Factory,
                dateRange.Item1.ToString("yyyyMMddHHmmss"),
                dateRange.Item2.ToString("yyyyMMddHHmmss"),
                Model,
                AreaFlag
                );

            Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1, 120, 0);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
        }

        #endregion [ Event Handler ]

        #region [ Method ]
        private Tuple<DateTime, DateTime> GetDateRange(
            )
        {
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;
            String date = String.Format("{0}{1}", Date, Hour);
            DateTime.TryParseExact(date, "yyyyMMddHH", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate);
 
            switch (Hour)
            { 
                case "00":
                case "01":
                case "02":
                case "03":
                case "04":
                case "05":
                    fromDate = fromDate.AddDays(1);
                    toDate = fromDate.AddHours(1);
                    break;
                default:
                    toDate = fromDate.AddHours(1);
                    break;
            }

            return new Tuple<DateTime, DateTime>(fromDate, toDate);
        }
        #endregion [ Method ]


        #region [ Property ]
        public String Date
        {
            get;
            private set;
        }

        public String Hour
        {
            get;
            private set;
        }

        public String Model
        {
            get;
            private set;
        }

        public bool AreaFlag
        {
            get;
            private set;
        }
        #endregion [ Property ]
    }
}
