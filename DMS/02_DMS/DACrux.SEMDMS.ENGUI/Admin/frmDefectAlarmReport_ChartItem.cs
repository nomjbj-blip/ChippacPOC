using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectAlarmReport_ChartItem : UserControl
    {
        public static readonly string DATA = "DEFECT_COUNT";
        public static readonly string LOWER = "LOWER";
        public static readonly string TARGET = "TARGET";
        public static readonly string UPPER = "UPPER";
        public static readonly string WAFER_ID = "WAFER_ID";
        public static readonly string TRAN_TIME = "TRAN_TIME";
        public static readonly string ALARM = "ALARM_TYPE";
        public static readonly string NOTIFY = "NOTI_USER";

        public frmDefectAlarmReport_ChartItem()
        {
            InitializeComponent();
        }

        private void frmDefectAlarmReport_ChartItem_Load(object sender, EventArgs e)
        {
            Series seriesData = chart.Series["seriesData"];
            seriesData.BorderWidth = 3;
            seriesData.Color = Color.Blue;
            seriesData.IsValueShownAsLabel = true;
            seriesData.MarkerSize = 10;

            Series seriesLower = chart.Series["seriesLower"];
            seriesLower.BorderWidth = 2;
            seriesLower.Color = Color.Red;
            seriesLower.IsValueShownAsLabel = false;

            Series seriesTarget = chart.Series["seriesTarget"];
            seriesTarget.BorderWidth = 2;
            seriesTarget.Color = Color.Yellow;
            seriesTarget.IsValueShownAsLabel = false;

            Series seriesUpper = chart.Series["seriesUpper"];
            seriesUpper.BorderWidth = 2;
            seriesUpper.Color = Color.Red;
            seriesUpper.IsValueShownAsLabel = false;

            if (DataSource == null || DataSource.Rows.Count == 0)
                return;

            chart.Titles[0].Text = DataSource.TableName;

            DataPoint dp;

            foreach (DataRow row in DataSource.Rows)
            {
                dp = new DataPoint();
                dp.SetValueXY(row[TRAN_TIME], row[DATA]);
                seriesData.Points.Add(dp);
                dp.ToolTip = GetTooltip(row);

                dp = new DataPoint();
                dp.SetValueXY(row[WAFER_ID], row[LOWER]);
                seriesLower.Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(row[WAFER_ID], row[TARGET]);
                seriesTarget.Points.Add(dp);

                dp = new DataPoint();
                dp.SetValueXY(row[WAFER_ID], row[UPPER]);
                seriesUpper.Points.Add(dp);

                chart.DataBind();
            }
        }

        private string GetTooltip(DataRow row)
        {
            return String.Format(
@"
WaferID = {0}
Count = {1}
Lower = {2}
Target = {3}
Upper = {4}
Alarm = {5}
Tran Time = {6}
Notify = {7}",
             row[WAFER_ID], row[DATA], row[LOWER], row[TARGET], row[UPPER], row[ALARM], row[TRAN_TIME], row[NOTIFY]);
        }

        public DataTable DataSource
        {
            get;
            set;
        }
    }
}
