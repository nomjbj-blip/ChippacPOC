using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using FarPoint.Win.Spread.CellType;
using DACrux.Utility;
using DACrux.Base;
using DACrux.Framework.Base;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectAlarmReport : DACruxUXBasic01, IExportExcel
    {
        #region 멤버 변수

        public const string CATEGORY_PRODUCT = "DM_PRODUCT";
        public const string CATEGORY_STEP = "DM_STEP";

        public const int MARGIN = 4;

        enum Col
        {
            FACTORY,
            DEFECT_TYPE,
            PRODUCT,
            STEP_ID,
            EQUIP_ID,
            LOWER,
            TARGET,
            UPPER,
            USE_FLAG,
            CREATE_USER,
            CREATE_TIME
        }

        #endregion

        #region 생성자 및 Load 이벤트

        public frmDefectAlarmReport()
        {
            InitializeComponent();
        }

        private void frmDefectAlarmReport_Load(object sender, EventArgs e)
        {
            SetDefectType();
            SetEquip();
            SetProduct();
            SetStep();

            SettingData setting = new SettingData(GetType());
            numChartCount.Value = setting.GetValue<int>(numChartCount.Name, 2);
            numDataCount.Value = setting.GetValue<int>(numDataCount.Name, 50);
        }

        private void frmDefectAlarmReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingData setting = new SettingData(GetType());
            setting.SetValue(numChartCount.Name, numChartCount.Value);
            setting.SetValue(numDataCount.Name, numDataCount.Value);
            setting.Save();
        }


        #endregion

        #region 사용자 정의 메서드

        private void SetDefectType()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetDefectType();

            cboDefectType.Items.Clear();

            foreach (DataRow row in dt.Rows)
                cboDefectType.Items.Add(row[0]);
        }

        private void SetEquip()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetEquip();

            cboEquip.Items.Clear();

            foreach (DataRow row in dt.Rows)
                cboEquip.Items.Add(row[0]);
        }

        private void SetProduct()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetConfigCode(CATEGORY_PRODUCT);

            DataRow row = dt.NewRow();
            row.ItemArray = new object[] { "ALL", String.Empty };
            dt.Rows.InsertAt(row, 0);

            cboProduct.DataSource = null;
            cboProduct.DisplayMember = dt.Columns[0].ColumnName;
            cboProduct.ValueMember = dt.Columns[1].ColumnName;

            cboProduct.DataSource = dt;
            cboProduct.SelectedIndex = 0;
        }

        private void SetStep()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetConfigCode(CATEGORY_STEP);

            DataRow row = dt.NewRow();
            row.ItemArray = new object[] { "ALL", String.Empty };
            dt.Rows.InsertAt(row, 0);

            cboStep.DataSource = null;
            cboStep.DisplayMember = dt.Columns[0].ColumnName;
            cboStep.ValueMember = dt.Columns[1].ColumnName;

            cboStep.DataSource = dt;
            cboStep.SelectedIndex = 0;
        }

        private bool CheckValid(bool showError)
        {
            if (String.IsNullOrEmpty(cboDefectType.Text))
            {
                if (showError)
                    ShowErrorMessage("Defect Type을 선택하세요.");
                
                cboDefectType.Focus();
                return false;
            }

            if (String.IsNullOrEmpty(cboEquip.Text))
            {
                if (showError)
                    ShowErrorMessage("Product를 선택하세요.");

                cboEquip.Focus();
                return false;
            }

            return true;
        }

        private int GetChartWidth()
        {
            return panel.Width - SystemInformation.VerticalScrollBarWidth - MARGIN;
        }

        private int GetChartHeight()
        {
            return (panel.Height - (int)numChartCount.Value * (panel.Margin.Top - panel.Margin.Bottom)) / (int)numChartCount.Value;
        }

        public void ExportExcel()
        {
            ExcelExportArgs e = new ExcelExportArgs();

            foreach (frmDefectAlarmReport_ChartItem ctl in panel.Controls)
            {
                ExcelSheet sheet = new ExcelSheet();
                sheet.Add(ctl);
                sheet.Add(ctl.DataSource);
                e.SheetList.Add(sheet);
            }

            ExcelExportManager.Export(e);
        }

        #endregion

        #region 이벤트 처리 메서드

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CheckValid(true))
                return;

            panel.Controls.Clear();

            DefectAlarm obj = new DefectAlarm();
            DataSet ds = obj.GetAlarmData(GlobalVariable.Factory, cboDefectType.Text, cboEquip.Text, cboProduct.SelectedValue.ToString(), cboStep.SelectedValue.ToString(), (int)numDataCount.Value);

            foreach (DataTable dt in ds.Tables)
            {
                frmDefectAlarmReport_ChartItem chartItem = new frmDefectAlarmReport_ChartItem();
                chartItem.Width = GetChartWidth();
                chartItem.Height = GetChartHeight();
                chartItem.DataSource = dt;
                panel.Controls.Add(chartItem);
            }
        }

        private void panel_SizeChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control ctl in panel.Controls)
                ctl.Width = GetChartWidth();

            foreach (System.Windows.Forms.Control ctl in panel.Controls)
                ctl.Height = GetChartHeight();
        }

        private void numChartCount_ValueChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control ctl in panel.Controls)
                ctl.Height = GetChartHeight();
        }

        private void numDataCount_ValueChanged(object sender, EventArgs e)
        {
            if (CheckValid(false))
                btnSearch.PerformClick();
        }

        #endregion        
    }
}
