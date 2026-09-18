using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Framework.Controls;

namespace DACrux.Common.Setup
{
    /// <summary>
    /// Sensor Mix Setup 화면
    /// </summary>
    public partial class frmEquipSetup : DACrux.Framework.Base.DACruxUXBasic02
    {
        #region 생성자 및 Load 메서드

        ListBox m_lstSelectedObject;

        /// <summary>
        /// 생성자
        /// </summary>
        public frmEquipSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load 이벤트 발생 시 실행되는 메서드 입니다.
        /// </summary>
        private void frmOperSetup_Load(object sender, EventArgs e)
        {
            DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);

            FillEquipModel();
            FillArea();
            FillOper();
        }

        #endregion

        #region 사용자 정의 메서드

        /// <summary>
        /// 선택된 항목을 문자열 배열로 가져옵니다.
        /// </summary>
        public string[] GetSelectedItemsToArray(DUCListBox listBox)
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

        private void FillArea()
        {
            lstArea.Items.Clear();

            RO.EquipManagement obj = new RO.EquipManagement();
            DataTable dt = obj.GetArea(GlobalVariable.Factory);

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow row in dt.Rows)
                lstArea.Items.Add(row[0]);
        }

        /// <summary>
        /// 공정 데이터를 바인딩 합니다.
        /// </summary>
        private void FillOper()
        {
            lstOper.Items.Clear();

            RO.EquipManagement obj = new RO.EquipManagement();
            DataTable dt = obj.GetOper(
                GlobalVariable.Factory, 
                GetSelectedItemsToArray(lstArea)
                );

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow row in dt.Rows)
                lstOper.Items.Add(row[0]);
        }

        /// <summary>
        /// Equip Model 값을 바인딩 합니다.
        /// </summary>
        private void FillEquipModel()
        {
            lstEquipModel.Items.Clear();

            RO.EquipManagement obj = new RO.EquipManagement();
            string[] areas = GetSelectedItemsToArray(lstArea);
            string[] opers = GetSelectedItemsToArray(lstOper);

            if (areas == null || areas.Length <= 0)
                return;

            if (opers == null || opers.Length <= 0)
                return;

            DataTable dt = obj.GetEquipModel(
                GlobalVariable.Factory,
                areas,
                opers
                );

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow row in dt.Rows)
                lstEquipModel.Items.Add(row[0]);
        }

        /// <summary>
        /// Spread에 데이터를 바인딩 합니다.
        /// </summary>
        private void SetSpreadData()
        {
            try
            {
                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);

                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                RO.EquipManagement obj = new RO.EquipManagement();
                DataTable dt = obj.GetEquipList(
                    GlobalVariable.Factory,
                    GetSelectedItemsToArray(lstArea),
                    GetSelectedItemsToArray(lstOper),
                    GetSelectedItemsToArray(lstEquipModel)
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetSpreadData();
        }

        /// <summary>
        /// Excel 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DACrux.Utility.ExcelUtilNew excel = new DACrux.Utility.ExcelUtilNew();
            excel.Add(fpSpread1_Sheet1);
            excel.ToExcel(Text);
        }

        /// <summary>
        /// Close 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// DucPropertyGrid의 추가,저장 등의 명령 버튼을 클릭하면 실행되는 메서드 입니다.
        /// </summary>
        private void grid_CommandButtonClick(object sender, Framework.PropertyGrid.CommandEventArgs e)
        {
            RO.EquipManagement obj = new RO.EquipManagement();

            string equipID = grid.GetValue("EQUIP_ID") as string;
            bool exists = obj.ExistsEquipData(GlobalVariable.Factory, equipID);

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
                    obj.InsertEquipData(grid.GetDictionaryValue(), GlobalVariable.UserID);
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
                    obj.UpdateEquipData(grid.GetDictionaryValue(), GlobalVariable.UserID);
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
                    obj.DeleteEquipData(GlobalVariable.Factory, equipID);
                else
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// DucPropertyGrid의 추가, 저장 등의 명령 버튼 클릭 후 처리 완료 시 실행되는 메서드 입니다.
        /// </summary>
        private void grid_CommandComplete(object sender, Framework.PropertyGrid.CommandEventArgs e)
        {
            btnSearch.PerformClick();
        }

        /// <summary>
        /// DucPropertyGrid의 Command 버튼을 클릭하여 상태(State.Reset, State.Edit 등)가 변경되면 실행되는 메서드 입니다.
        /// </summary>
        private void grid_StateChanged(object sender, Framework.PropertyGrid.StateChangedEventArgs e)
        {
            if (e.State == Framework.PropertyGrid.State.Insert)
                grid.SetValue("FACTORY", GlobalVariable.Factory);
        }

        /// <summary>
        /// DucPropertyGrid의 ComboBox DropDown 시 실행되는 메서드 입니다.
        /// </summary>
        private void grid_DropDownComboBox(object sender, Framework.PropertyGrid.DropDownComboBoxEventArgs e)
        {
            RO.EquipManagement obj = new RO.EquipManagement();

            if (string.Equals(e.PropertyName, "EQUIP_MODEL"))
            {
                DataTable dt = obj.GetEquipModel(grid.GetValue("FACTORY") as string);

                foreach (DataRow row in dt.Rows)
                    e.ComboBoxValueList.Add(row[0].ToString());
            }
            else if (string.Equals(e.PropertyName, "AREA"))
            {
                DataTable dt = obj.GetArea(grid.GetValue("FACTORY") as string);

                foreach (DataRow row in dt.Rows)
                    e.ComboBoxValueList.Add(row[0].ToString());
            }
            else if (string.Equals(e.PropertyName, "OPER"))
            {
                DataTable dt = obj.GetOper(grid.GetValue("FACTORY") as string, new string[] { grid.GetValue("AREA") as string });

                foreach (DataRow row in dt.Rows)
                    e.ComboBoxValueList.Add(row[0].ToString());
            }
            else if (string.Equals(e.PropertyName, "HANDLER"))
            {
                string[] handlers = obj.GetHanlderNames();

                foreach (string handler in handlers)
                    e.ComboBoxValueList.Add(handler);
            }
        }

        // <summary>
        /// ALL 컨텍스트 메뉴 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void aLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_lstSelectedObject.Items.Count; i++)
            {
                m_lstSelectedObject.SetSelected(i, true);
            }
        }

        /// <summary>
        /// Invert 컨텍스트 메뉴 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_lstSelectedObject.Items.Count; i++)
            {
                m_lstSelectedObject.SetSelected(i, !m_lstSelectedObject.GetSelected(i));
            }
        }

        /// <summary>
        /// Deselect 컨텍스트 메뉴 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_lstSelectedObject.Items.Count; i++)
            {
                m_lstSelectedObject.SetSelected(i, false);
            }
        }

        private void lstArea_OnSelectedIndexChanged(
            object sender, 
            EventArgs e
            )
        {
            FillOper();
        }

        private void lstOper_OnSelectedIndexChanged(
            object sender, 
            EventArgs e
            )
        {
            FillEquipModel();
        }

        private void lstEquipModel_OnSelectedIndexChanged(
            object sender, 
            EventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        /// <summary>
        /// ListBox에서 MouseUp 시 실행되는 메서드 입니다.
        /// </summary>
        private void ListBox_MouseUp(object sender, MouseEventArgs e)
        {
            ListBox lstBox = sender as ListBox;

            if (lstBox.SelectionMode == SelectionMode.One || lstBox.SelectionMode == SelectionMode.None)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                m_lstSelectedObject = (ListBox)sender;
                popupMnu.Show(m_lstSelectedObject, e.Location);
            }
        }

        #endregion
    }
}
