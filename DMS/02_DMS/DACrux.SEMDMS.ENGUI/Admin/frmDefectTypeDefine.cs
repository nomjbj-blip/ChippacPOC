using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectTypeDefine : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmDefectTypeDefine()
        {
            InitializeComponent();
        }


        private void frmDefectTypeDefine_Load(object sender, EventArgs e)
        {
            try
            {
                ViewDefectGroupList();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        // View Defect Group List
        public void ViewDefectGroupList()
        {
            SEMConfiguration oSEMConfiguration = null;
            DataTable dataList = null;

            try
            {
                oSEMConfiguration = new SEMConfiguration();

                dataList = oSEMConfiguration.SelectGroupList();
                Utility.FPSpreadUtil.InitSpread(fpSpread_Group);
                DACrux.Utility.FPSpreadUtil.SetSpreadData(dataList, fpSpread_Group_Sheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Group_Sheet);

                //fpSpread_Group_Sheet.DataSource = dataList;
                //fpSpread_Group_Sheet.ColumnHeaderAutoText.ToString();    
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        // View Defect Group by Group ID
        public void GetDefectList(string groupId)
        {
            SEMConfiguration oSEMConfiguration = null;
            DataTable dataList = null;

            try
            {
                oSEMConfiguration = new SEMConfiguration();

                dataList = oSEMConfiguration.GetDefectTypeList(groupId);
                Utility.FPSpreadUtil.InitSpread(fpSpread_Type);
                DACrux.Utility.FPSpreadUtil.SetSpreadData(dataList, fpSpread_Type_Sheet);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Type_Sheet);

                //fpSpread_Type_Sheet.DataSource = dataList;
                //fpSpread_Type_Sheet.ColumnHeaderAutoText.ToString();    
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        // Create Defect Group
        private void CreateGroup()
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            try
            {
                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                oConfigure.CreateGroup(txtGroupId.Text, txtGroupName.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Update Defect Group by Group ID
        private void UpdateGroup(string groupId, string groupName)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            try
            {
                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                oConfigure.UpdateGroup(txtGroupId.Text, txtGroupName.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Delete Defect Group by Group ID
        private void DeleteGroup(string groupId)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            try
            {
                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                oConfigure.DeleteGroup(txtGroupId.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtGroupId.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Group ID.", this.Name);
                    return;
                }
                if (txtGroupName.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Group Name.", this.Name);
                    return;
                }
                int itemp = 0;
                if (int.TryParse(txtGroupId.Text, out itemp)==false)
                {
                    MessageBox.Show(this, "Please input only number.", this.Name);
                    return;
                }
                CreateGroup();
                ViewDefectGroupList();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtGroupId.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Group ID.", this.Name);
                    return;
                }
                if (txtGroupName.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Group Name.", this.Name);
                    return;
                }
                int itemp = 0;
                if (int.TryParse(txtGroupId.Text, out itemp)==false)
                {
                    MessageBox.Show(this, "Please input only number.", this.Name);
                    return;
                }

                UpdateGroup(txtGroupId.Text, txtGroupName.Text);
                ViewDefectGroupList();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGroupId.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Group ID.", this.Name);
                    return;
                }
                int itemp = 0;
                if (int.TryParse(txtGroupId.Text, out itemp) == false)
                {
                    MessageBox.Show(this, "Please input only number.", this.Name);
                    return;
                }

                if (MessageBox.Show(this, "Are you sure you want to delete the group ID?"
                                    , this.Name
                                    , MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteGroup(txtGroupId.Text);
                    ViewDefectGroupList();
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void fpSpread_Group_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                string groupId = fpSpread_Group_Sheet.GetText(e.Row, 0);
                string groupname = fpSpread_Group_Sheet.GetText(e.Row, 1);
                GetDefectList(groupId);

                txtGroupId.Text = groupId;
                txtGroupName.Text = groupname;

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void fpSpread_Type_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                string groupId = fpSpread_Type_Sheet.GetText(e.Row, 3);
                string typeno = fpSpread_Type_Sheet.GetText(e.Row, 0);
                string typename = fpSpread_Type_Sheet.GetText(e.Row, 1);
                string typedesc = fpSpread_Type_Sheet.GetText(e.Row, 2);

                cmbGroupList.Items.Clear();
                for (int i = 0; i < fpSpread_Group_Sheet.Rows.Count; i++)
                {
                    cmbGroupList.Items.Add(fpSpread_Group_Sheet.GetText(i, 0));
                }
                cmbGroupList.Text = groupId;
                txtTypeId.Text = typeno;
                txtTypeName.Text = typename;
                txtDesc.Text = typedesc;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }


        // Type Create, Update, Delete
        private void butCreateType_Click(object sender, EventArgs e)
        {
            //DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            //try
            //{
            //    if (txtTypeId.Text.Length == 0)
            //    {
            //        MessageBox.Show(this, "Please input Type ID.", this.Name);
            //        return;
            //    }

            //    int defecttypeid = -1;

            //    if (int.TryParse(txtTypeId.Text, out defecttypeid) == false)
            //    {
            //        MessageBox.Show(this, "Please input only number.", this.Name);
            //        return;
            //    }

            //    if (txtTypeName.Text.Length == 0)
            //    {
            //        MessageBox.Show(this, "Please input Type name.", this.Name);
            //        return;
            //    }

            //    oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
            //    if (oConfigure.CreateDefectType(defecttypeid, txtTypeName.Text, txtDesc.Text, txtTypeGroupId.Text))
            //    {
            //        MessageBox.Show(this, "Complete deletion.", this.Name);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DspError(ex);
            //}
        }

        private void butUpdateType_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            try
            {
                int defecttypeid = -1;

                if (int.TryParse(txtTypeId.Text, out defecttypeid) == false)
                {
                    MessageBox.Show(this, "Please input only number.", this.Name);
                    return;
                }

                if (txtTypeName.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Type name.", this.Name);
                    return;
                }

                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                if (oConfigure.UpdateDefectType(defecttypeid
                                            , txtTypeName.Text
                                            , txtDesc.Text
                                            , cmbGroupList.Text))
                {
                    MessageBox.Show(this, "Complete Update.", this.Name);

                    txtTypeId.Text = "";
                    txtTypeName.Text = "";
                    txtDesc.Text = "";
                    GetDefectList(cmbGroupList.Text);
                    cmbGroupList.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butDeleteType_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            try
            {
                if (txtTypeId.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Type ID.", this.Name);
                    return;
                }

                int defecttypeid = -1;

                if (int.TryParse(txtTypeId.Text, out defecttypeid) == false)
                {
                    MessageBox.Show(this, "Please input only number.", this.Name);
                    return;
                }

                if (txtTypeName.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input Type name.", this.Name);
                    return;
                }

                if (defecttypeid == 0)
                {
                    MessageBox.Show(this, "Can't delete type 0", this.Name);
                    return;
                }

                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                if (oConfigure.UpdateDefectType(defecttypeid
                    , string.Format("Defect {0}",defecttypeid)
                    , string.Format("Defect Type {0}", defecttypeid)
                    , cmbGroupList.Text))
                {
                    MessageBox.Show(this, "Complete deletion.", this.Name);

                    txtTypeId.Text = "";
                    txtTypeName.Text = "";
                    txtDesc.Text = "";
                    GetDefectList(cmbGroupList.Text);
                    cmbGroupList.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

    }
}
