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

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectAlarmSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region 멤버 변수

        public const string CATEGORY_PRODUCT = "DM_PRODUCT";
        public const string CATEGORY_STEP = "DM_STEP";

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

        public frmDefectAlarmSetup()
        {
            InitializeComponent();
        }

        private void frmDefectAlarmSetup_Load(object sender, EventArgs e)
        {
            SetDefectType();
            SetProduct();
            SetStep();

            fpSpread_Sheet.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
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

        private void SetProduct()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetConfigCode(CATEGORY_PRODUCT);

            cboProduct.Items.Clear();

            foreach (DataRow row in dt.Rows)
                cboProduct.Items.Add(row[0]);
        }

        private void SetStep()
        {
            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetConfigCode(CATEGORY_STEP);

            cboStep.Items.Clear();

            foreach (DataRow row in dt.Rows)
                cboStep.Items.Add(row[0]);
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

            if (String.IsNullOrEmpty(cboProduct.Text))
            {
                if (showError)
                    ShowErrorMessage("Product를 선택하세요.");
                
                cboProduct.Focus();
                return false;
            }

            if (String.IsNullOrEmpty(cboStep.Text))
            {
                if (showError)
                    ShowErrorMessage("Defect Type을 선택하세요.");
                
                cboStep.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region 이벤트 처리 메서드

        private void cboDefectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fpSpread_Sheet.RowCount = 0;

            if (CheckValid(false))
                btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CheckValid(true))
                return;

            RO.DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetAlarmSetup(GlobalVariable.Factory, cboDefectType.Text, cboProduct.Text, cboStep.Text);

            fpSpread.DataSource = dt;
            FPSpreadUtil.SetAutoColumnWidth(fpSpread_Sheet);

            fpSpread_Sheet.Columns[(int)Col.USE_FLAG].CellType = new CheckBoxCellType();
            fpSpread_Sheet.Columns[(int)Col.USE_FLAG].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            fpSpread_Sheet.Columns[(int)Col.FACTORY].Visible = false;

            foreach (Col col in new Col[] { Col.DEFECT_TYPE, Col.EQUIP_ID, Col.PRODUCT, Col.STEP_ID, Col.CREATE_USER, Col.CREATE_TIME })
            {
                fpSpread_Sheet.Columns[(int)col].Locked = true;
                fpSpread_Sheet.Columns[(int)col].BackColor = Color.WhiteSmoke;
                fpSpread_Sheet.Columns[(int)col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            }

            FPSpreadUtil.SetDecimalLength(fpSpread_Sheet, 0, Col.LOWER, Col.TARGET, Col.UPPER);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            using (frmDefectAlarmSetup_Code frm = new frmDefectAlarmSetup_Code())
            {
                frm.Category = CATEGORY_PRODUCT;
                frm.ShowDialog();

                if (frm.IsChanged)
                    SetProduct();
            }
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            using (frmDefectAlarmSetup_Code frm = new frmDefectAlarmSetup_Code())
            {
                frm.Category = CATEGORY_STEP;
                frm.ShowDialog();

                if (frm.IsChanged)
                    SetStep();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fpSpread_Sheet.RowCount == 0)
            {
                ShowErrorMessage("저장할 데이터가 없습니다.");
                return;
            }

            if (!ShowQuestionMessage("저장하시겠습니까?"))
                return;

            List<string[]> list = new List<string[]>();

            for (int i = 0; i < fpSpread_Sheet.RowCount; i++)
            {
                double val;
                bool existsData =
                    Double.TryParse(fpSpread_Sheet.Cells[i, (int)Col.LOWER].Text, out val) ||
                    Double.TryParse(fpSpread_Sheet.Cells[i, (int)Col.TARGET].Text, out val) ||
                    Double.TryParse(fpSpread_Sheet.Cells[i, (int)Col.UPPER].Text, out val);

                if (!existsData)
                    continue;

                string[] arr = new string[(int)Col.CREATE_USER + 1];
                arr[(int)Col.FACTORY] = fpSpread_Sheet.Cells[i, (int)Col.FACTORY].Text;
                arr[(int)Col.DEFECT_TYPE] = fpSpread_Sheet.Cells[i, (int)Col.DEFECT_TYPE].Text;
                arr[(int)Col.PRODUCT] = fpSpread_Sheet.Cells[i, (int)Col.PRODUCT].Text;
                arr[(int)Col.STEP_ID] = fpSpread_Sheet.Cells[i, (int)Col.STEP_ID].Text;
                arr[(int)Col.EQUIP_ID] = fpSpread_Sheet.Cells[i, (int)Col.EQUIP_ID].Text;
                arr[(int)Col.LOWER] = fpSpread_Sheet.Cells[i, (int)Col.LOWER].Text;
                arr[(int)Col.TARGET] = fpSpread_Sheet.Cells[i, (int)Col.TARGET].Text;
                arr[(int)Col.UPPER] = fpSpread_Sheet.Cells[i, (int)Col.UPPER].Text;
                arr[(int)Col.USE_FLAG] = fpSpread_Sheet.Cells[i, (int)Col.USE_FLAG].Text == Boolean.TrueString ? "Y" : "N";
                arr[(int)Col.CREATE_USER] = GlobalVariable.UserID;

                list.Add(arr);
            }

            if (list.Count == 0)
            {
                ShowErrorMessage("저장할 데이터가 없습니다.");
                return;
            }

            string[,] data = new string[list.Count, list[0].Length];

            for (int y = 0; y < list.Count; y++)
                for (int x = 0; x < list[0].Length; x++)
                    data[y, x] = list[y][x];

            RO.DefectAlarm obj = new DefectAlarm();
            obj.DeleteAlarmSetup(list[0][(int)Col.FACTORY], list[0][(int)Col.DEFECT_TYPE], list[0][(int)Col.PRODUCT], list[0][(int)Col.STEP_ID]);
            obj.InsertAlarmSetup(data);

            btnSearch.PerformClick();
            MessageBox.Show("저장을 완료하였습니다.");
        }

        private void btnRecalc_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fpSpread_Sheet.RowCount; i++)
            {
                double val;

                if (double.TryParse(fpSpread_Sheet.Cells[i, (int)Col.TARGET].Text, out val))
                {
                    fpSpread_Sheet.Cells[i, (int)Col.LOWER].Value = val * (100 - (double)numRecalc.Value) / 100f;
                    fpSpread_Sheet.Cells[i, (int)Col.UPPER].Value = val * (100 + (double)numRecalc.Value) / 100f;
                }
            }

            MessageBox.Show("Lower/Upper 계산을 완료하였습니다.");
        }

        private void btnNotifyUser_Click(object sender, EventArgs e)
        {
            using (frmDefectAlarmSetup_Notify frm = new frmDefectAlarmSetup_Notify())
            {
                frm.ShowDialog();
            }
        }

        #endregion
    }
}
