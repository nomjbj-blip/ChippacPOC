using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectAlarmSetup_Code : Form
    {
        #region 생성자 및 Load 이벤트

        public frmDefectAlarmSetup_Code()
        {
            InitializeComponent();
        }

        private void frmDefectAlarmSetup_Code_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Category))
            {
                MessageBox.Show("Category가 정의되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetCodeList();
        }
        
        #endregion

        #region 이벤트 처리 메서드

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();

            if (String.IsNullOrEmpty(code))
            {
                txtCode.Focus();
                MessageBox.Show("값을 입력하세요.");
                return;
            }

            if (MessageBox.Show("추가 하시겠습니까?", "추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (ExistsCode(code))
            {
                MessageBox.Show(String.Format("'{0}' 코드가 이미 존재합니다.", code), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return;
            }

            AddCode(code);
            SetCodeList();
            MessageBox.Show("추가 하였습니다.");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();

            if (String.IsNullOrEmpty(code))
            {
                txtCode.Focus();
                MessageBox.Show("값을 입력하세요.");
                return;
            }

            if (MessageBox.Show("삭제 하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!ExistsCode(code))
            {
                MessageBox.Show(String.Format("삭제할 '{0}' 코드가 존재하지 않습니다.", code), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DeleteCode(code);
            SetCodeList();
            MessageBox.Show("삭제 하였습니다.");
        }

        private void lstCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCode.Clear();

            if (lstCode.SelectedIndex >= 0)
                txtCode.Text = lstCode.Text;
        }
        
        #endregion

        #region 사용자 정의 메서드

        private void SetCodeList()
        {
            RO.DefectAlarm obj = new RO.DefectAlarm();
            DataTable dt = obj.GetConfigCode(Category);

            txtCode.Clear();
            lstCode.Items.Clear();

            foreach (DataRow row in dt.Rows)
                lstCode.Items.Add(row[0]);
        }

        public bool ExistsCode(string code)
        {
            RO.DefectAlarm obj = new RO.DefectAlarm();
            return obj.ExistsConfigCode(Category, code);
        }

        public void AddCode(string code)
        {
            RO.DefectAlarm obj = new RO.DefectAlarm();
            obj.AddConfigCode(Category, code);
            IsChanged = true;
        }

        public void DeleteCode(string code)
        {
            RO.DefectAlarm obj = new RO.DefectAlarm();
            obj.DeleteConfigCode(Category, code);
            IsChanged = true;
        }

        #endregion

        #region 프로퍼티

        public string Category
        {
            get;
            set;
        }

        public bool IsChanged
        {
            get;
            private set;
        }

        #endregion
    }
}
