using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using System.Diagnostics;

namespace DACrux.Common.Setup
{
    /// <summary>
    /// Sensor Mix Setup 화면
    /// </summary>
    public partial class frmOperSetup 
        : DACrux.Framework.Base.DACruxUXBasic02
    {
        #region 생성자 및 Load 메서드

        /// <summary>
        /// 생성자
        /// </summary>
        public frmOperSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load 이벤트 발생 시 실행되는 메서드 입니다.
        /// </summary>
        private void frmOperSetup_Load(
            object sender, 
            EventArgs e
            )
        {
            DACrux.Utility.FPSpreadUtil.InitSpread(
                fpSpread1
                );

            FillArea();
            SetSpreadData();
        }

        private void FillArea(
            )
        {
            lstArea.Items.Clear();

            RO.EquipManagement obj = new RO.EquipManagement();
            DataTable dt = obj.GetArea(GlobalVariable.Factory);

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow row in dt.Rows)
                lstArea.Items.Add(row[0]);
        }
        
        #endregion

        #region 사용자 정의 메서드

        /// <summary>
        /// 선택된 항목을 문자열 배열로 가져옵니다.
        /// </summary>
        public string[] GetSelectedItemsToArray(
            ListBox listBox
            )
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count == 0)
                return null;

            List<string> list = new List<string>();

            for (int i = 0; i < listBox.SelectedItems.Count; i++)
            {
                list.Add(listBox.SelectedItems[i].ToString());
            }

            return list.ToArray();
        }

        /// <summary>
        /// Spread에 데이터를 바인딩 합니다.
        /// </summary>
        private void SetSpreadData(
            )
        {
            try
            {
                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);

                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                RO.EquipManagement obj = new RO.EquipManagement();
                DataTable dt = obj.GetOper02(
                    DACrux.Base.GlobalVariable.Factory,
                    GetSelectedItemsToArray(lstArea)
                    );

                if (dt == null)
                    return;

                DACrux.Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread1_Sheet1);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnSort(fpSpread1_Sheet1);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnFilter(fpSpread1_Sheet1);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                grid.DataSource = dt;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        
        #endregion

        #region 이벤트 처리 메서드
        
        /// <summary>
        /// Search 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnSearch_Click(
            object sender, 
            EventArgs e
            )
        {
            SetSpreadData();
        }

        /// <summary>
        /// Excel 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnToExcel_Click(
            object sender,
            EventArgs e
            )
        {
            DACrux.Utility.ExcelUtilNew excel = new DACrux.Utility.ExcelUtilNew();
            excel.Add(fpSpread1_Sheet1);
            excel.ToExcel(Text);
        }

        /// <summary>
        /// Close 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void butClose_Click(
            object sender, 
            EventArgs e
            )
        {
            Close();
        }

        /// <summary>
        /// DucPropertyGrid의 추가,저장 등의 명령 버튼을 클릭하면 실행되는 메서드 입니다.
        /// </summary>
        private void grid_CommandButtonClick(
            object sender, 
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            RO.EquipManagement obj = new RO.EquipManagement();

            string area = grid.GetValue("AREA") as string;
            string oper = grid.GetValue("OPER") as string;
            bool exists = obj.ExistsOperData(GlobalVariable.Factory, area, oper);

            if (e.Mode == Framework.PropertyGrid.CommandMode.Insert)
            {
                if (exists)
                {
                    MessageBox.Show("해당 데이터가 이미 존재합니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("저장하시겠습니까?", "Insert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                    obj.InsertOperData(grid.GetDictionaryValue(), GlobalVariable.UserID);
                else
                    e.Cancel = true;
            }
            else if (e.Mode == Framework.PropertyGrid.CommandMode.Update)
            {
                if (!exists)
                {
                    MessageBox.Show("업데이트할 데이터가 없습니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("업데이트 하시겠습니까?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                    obj.UpdateOperData(grid.GetDictionaryValue(), GlobalVariable.UserID);
                else
                    e.Cancel = true;
            }
            else if (e.Mode == Framework.PropertyGrid.CommandMode.Delete)
            {
                if (!exists)
                {
                    MessageBox.Show("삭제할 데이터가 없습니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("삭제하시겠습니까?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                    obj.DeleteOperData(GlobalVariable.Factory, area, oper);
                else
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// DucPropertyGrid의 추가, 저장 등의 명령 버튼 클릭 후 처리 완료 시 실행되는 메서드 입니다.
        /// </summary>
        private void grid_CommandComplete(
            object sender, 
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        /// <summary>
        /// DucPropertyGrid의 Command 버튼을 클릭하여 상태(State.Reset, State.Edit 등)가 변경되면 실행되는 메서드 입니다.
        /// </summary>
        private void grid_StateChanged(
            object sender, 
            Framework.PropertyGrid.StateChangedEventArgs e
            )
        {
            if (e.State == Framework.PropertyGrid.State.Insert)
                grid.SetValue("FACTORY", GlobalVariable.Factory);
        }

        /// <summary>
        /// DucPropertyGrid의 ComboBox DropDown 시 실행되는 메서드 입니다.
        /// </summary>
        private void grid_DropDownComboBox(
            object sender, 
            Framework.PropertyGrid.DropDownComboBoxEventArgs e
            )
        {
            RO.EquipManagement obj = new RO.EquipManagement();
            DataTable dt = null;

            if (String.Equals(e.PropertyName, "AREA"))
            {
                dt = obj.GetArea(grid.GetValue("FACTORY") as string);

                foreach (DataRow row in dt.Rows)
                    e.ComboBoxValueList.Add(row[0].ToString());
            }
        }

        #endregion
    }
}
