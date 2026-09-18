using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
//using DACrux.Utility;

namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : frmGroupManagement<br/>
    /// Summary    : User Security Group Management WinForm Class<br/>
    /// Author     : Miracom Hyuntai, Kim<br/>
    /// First Date : 2009-02-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmGroupManagement : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region Class Member

        bool bInstEdit = true;

        DataTable dtGrpLst = null;
        DataTable dtUseFun = null;
        DataTable dtFun = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmGroupManagement()
        {
            InitializeComponent();
        }

        private void frmGroupManagement_Load(object sender, EventArgs e)
        {
            LoadGroup("None");
        }

        #endregion

        #region Group List Loading

        /// <summary>
        /// Get All Group List (strGrpCode : None -> All Group)
        /// </summary>
        public void LoadGroup(string strGrpCode)
        {
            DACrux.Framework.RO.UserGroup oUserGroup = null;

            oUserGroup = new DACrux.Framework.RO.UserGroup();

            dtGrpLst = oUserGroup.LoadGroupList(strGrpCode);
            lstGroup.ValueMember = "SEC_GRP_ID";
            lstGroup.DisplayMember = "SEC_GRP_DESC";
            lstGroup.DataSource = dtGrpLst;
        }

        #endregion

        #region Button Event

        #region btnSave_Click

        /// <summary>
        /// Save Current Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool a = true;
            bool b = true;
            bool c = true;
            bool d = true;
            bool f = true;

            string strGrpCode = string.Empty;
            string strGrpName = string.Empty;
            string strGrpCaption = string.Empty;
            string strFunCode = string.Empty;
            string strColName = string.Empty;

            int iCNT;
            int i;

            DACrux.Framework.RO.UserGroup oUserGroup = null;

            try
            {
                strGrpCode = txtGrpCode.Text.Trim();
                strGrpName = txtGrpName.Text.Trim();

                if (strGrpName == "")
                {
                    MessageBox.Show("Input the group name.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtGrpName.Focus();
                    return;
                }

                strGrpCaption = txtGrpCaption.Text.Trim();
                strFunCode = string.Empty;

                dtUseFun = (DataTable)lstUseFunc.DataSource;
                dtFun = (DataTable)lstFunc.DataSource;

                iCNT = dtUseFun.Rows.Count;
                strColName = "FUNC_CODE";

                oUserGroup = new DACrux.Framework.RO.UserGroup();

                if (bInstEdit) // Add Group
                {
                    a = oUserGroup.InsertGroupInfo(strGrpCode, strGrpName, strGrpCaption);

                    //그룹의 기능 등록
                    for (i = 0; i < iCNT; i++)
                    {
                        strFunCode = System.Convert.ToString(dtUseFun.Rows[i][strColName]);
                        if (b)
                        {
                            b = oUserGroup.InsertGroupFunction(strGrpCode, strFunCode);
                        }
                    }
                }
                else // Edit Group
                {

                    DialogResult DiRslt = MessageBox.Show("Do you want save it?", "Edit Group", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    c = oUserGroup.UpdateGroupInfo(strGrpCode, strGrpName, strGrpCaption);

                    // Group Function Delete & Addition
                    d = oUserGroup.DeleteGroupAllFunction(strGrpCode);

                    // Regist function which are selected
                    for (i = 0; i < iCNT; i++)
                    {
                        strFunCode = System.Convert.ToString(dtUseFun.Rows[i][strColName]);

                        if (b)
                        {
                            f = oUserGroup.InsertGroupFunction(strGrpCode, strFunCode);
                        }
                    }
                }
            }
            finally
            {
                DisableControl();
                LoadGroup("None");
            }
        }

        #endregion

        #region brnInsert_Click

        /// <summary>
        /// Insert New Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brnInsert_Click(object sender, EventArgs e)
        {
            string strMaxGrp = string.Empty;
            string strNxtGrp = string.Empty;
            DataTable dt = null;

            DACrux.Framework.RO.UserGroup oUserGroup = null;

            try
            {
                EnableControl();
                bInstEdit = true;

                oUserGroup = new DACrux.Framework.RO.UserGroup();

                //새로운 그룹코드 받기
                dt = oUserGroup.LoadMaxGroup();

                if (dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "")
                {
                    strMaxGrp = "F0000";
                }
                else
                {
                    strMaxGrp = dt.Rows[0][0].ToString();
                }

                strNxtGrp = "G" + String.Format("{0:000}", System.Convert.ToInt32(strMaxGrp.Substring(1, 3)) + 1);

                txtGrpCode.Text = strNxtGrp;
                txtGrpName.Text = "";
                txtGrpCaption.Text = "";

                dtUseFun = null;

                dtFun = oUserGroup.LoadAllFunction();

                dtUseFun = oUserGroup.LoadGroupFunction(txtGrpCode.Text);

                lstFunc.ValueMember = "FUNC_CODE";
                lstFunc.DisplayMember = "FUNC_NAME";
                lstUseFunc.ValueMember = "FUNC_CODE";
                lstUseFunc.DisplayMember = "FUNC_NAME";

                lstFunc.DataSource = dtFun;
                lstUseFunc.DataSource = dtUseFun;

                txtGrpName.Focus();
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                if (dtFun != null)
                {
                    dtFun.AcceptChanges();
                }

                if (dtUseFun != null)
                {
                    dtUseFun.AcceptChanges();
                }
            }
        }

        #endregion

        #region brnEdit_Click

        /// <summary>
        /// Enable setting to Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brnEdit_Click(object sender, EventArgs e)
        {
            EnableControl();
            bInstEdit = false;
        }

        #endregion

        #region Delete Group

        /// <summary>
        /// Delete Selected Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int iCnt;

            string strGrp = string.Empty;

            bool a = true;
            bool b = true;

            DataTable dtTmp = null;

            DACrux.Framework.RO.UserGroup oUserGroup = null;

            oUserGroup = new DACrux.Framework.RO.UserGroup();
            strGrp = dtGrpLst.Rows[lstGroup.SelectedIndex][0].ToString();

            if (strGrp == "G001")
            {
                MessageBox.Show("This is admin group. You can not delete it", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }

            dtTmp = oUserGroup.LoadGroupUser(strGrp);
            iCnt = dtTmp.Rows.Count;

            if (iCnt != 0)
            {
                try
                {
                    DialogResult DiRslt = MessageBox.Show("You have to select a new group for users in current group", "New Group", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    if (DiRslt == DialogResult.OK)
                    {
                        // frmChoiceGroup oForm = new frmChoiceGroup(strGrp);
                        // oForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    DspError(ex);
                }
                finally
                {
                    dtTmp.Dispose();
                    dtTmp = null;
                    LoadGroup("None");
                }
            }
            else
            {
                try
                {
                    DialogResult DiRslt = MessageBox.Show("Do you want delete the group?", "Delete Group", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (DiRslt == DialogResult.OK)
                    {
                        a = oUserGroup.DeleteGroupAllFunction(strGrp);
                        b = oUserGroup.DeleteGroup(strGrp);
                    }
                }
                catch (Exception ex)
                {
                    DspError(ex);
                }
                finally
                {
                    dtTmp.Dispose();
                    dtTmp = null;
                    LoadGroup("None");
                }
            }

            if (!b) MessageBox.Show("Failed.");

        }

        #endregion

        #region btnIn_Click

        /// <summary>
        /// Add Function to Selected Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIn_Click(object sender, EventArgs e)
        {
            ArrayList arrlTemp = null;
            int iSel;

            try
            {
                arrlTemp = new ArrayList();
                iSel = lstFunc.SelectedItems.Count;
                lstUseFunc.ValueMember = "FUNC_CODE";
                lstUseFunc.DisplayMember = "FUNC_NAME";

                for (int k = 0; k < iSel; k++)
                {
                    DataRowView dr = lstFunc.SelectedItems[k] as DataRowView;
                    if (dr["FUNC_NAME"].ToString() == "Function Management")
                        if (DACrux.Base.GlobalVariable.UserID != "YSIM") continue;

                    arrlTemp.Add(lstFunc.Items.IndexOf(lstFunc.SelectedItems[k]));
                }

                for (int k = arrlTemp.Count - 1; k >= 0; k--)
                {
                    dtUseFun.ImportRow(dtFun.Rows[System.Convert.ToInt32(arrlTemp[k])]);
                    dtFun.Rows[(int)arrlTemp[k]].Delete();
                }
            }
            finally
            {
                dtUseFun.AcceptChanges();
                dtFun.AcceptChanges();
            }
        }

        #endregion

        #region btnOut_Click

        /// <summary>
        /// Remove Function From Selected Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOut_Click(object sender, EventArgs e)
        {
            ArrayList arrlTemp = null;
            int iSel;

            try
            {
                arrlTemp = new ArrayList();
                iSel = lstUseFunc.SelectedItems.Count;
                lstFunc.ValueMember = "FUNC_CODE";
                lstFunc.DisplayMember = "FUNC_NAME";

                for (int k = 0; k < iSel; k++)
                {
                    arrlTemp.Add(lstUseFunc.Items.IndexOf(lstUseFunc.SelectedItems[k]));
                }

                for (int k = arrlTemp.Count - 1; k >= 0; k--)
                {
                    dtFun.ImportRow(dtUseFun.Rows[(int)arrlTemp[k]]);
                    dtUseFun.Rows[(int)arrlTemp[k]].Delete();
                }
            }
            finally
            {
                dtFun.AcceptChanges();
                dtUseFun.AcceptChanges();
            }
        }

        #endregion

        #region btnClose_Click

        /// <summary>
        /// Exit Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion

        #region lstGroup_SelectedIndexChanged

        /// <summary>
        /// Show Group Information By Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = null;
            string strSelVal = string.Empty;
            DACrux.Framework.RO.UserGroup oUserGroup = null;

            try
            {
                DisableControl();

                strSelVal = lstGroup.SelectedValue.ToString();

                oUserGroup = new DACrux.Framework.RO.UserGroup();
                dt = oUserGroup.LoadGroupInfo(strSelVal);

                txtGrpCode.Text = dt.Rows[0]["sec_grp_id"].ToString();
                txtGrpName.Text = dt.Rows[0]["sec_grp_desc"].ToString();
                txtGrpCaption.Text = dt.Rows[0]["remark"].ToString();

                dtUseFun = oUserGroup.LoadGroupFunction(strSelVal);

                lstUseFunc.ValueMember = "FUNC_CODE";
                lstUseFunc.DisplayMember = "FUNC_NAME";
                lstUseFunc.DataSource = dtUseFun;
                lstUseFunc.ClearSelected();

                dtFun = oUserGroup.LoadOutFunction(strSelVal);
                lstFunc.ValueMember = "FUNC_CODE";
                lstFunc.DisplayMember = "FUNC_NAME";
                lstFunc.DataSource = dtFun;
                lstFunc.ClearSelected();
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

                oUserGroup = null;
            }
        }

        #endregion

        #region SetControl

        /// <summary>
        /// Enable Control
        /// </summary>
        public void EnableControl()
        {
            txtGrpName.ReadOnly = false;
            txtGrpCaption.ReadOnly = false;
            btnSave.Enabled = true;
            btnSave.Visible = true;
            btnIn.Visible = true;
            btnOut.Visible = true;

            btnInsert.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

        }

        /// <summary>
        /// Disable Control
        /// </summary>
        public void DisableControl()
        {
            txtGrpName.ReadOnly = true;
            txtGrpCaption.ReadOnly = true;
            btnSave.Enabled = false;
            btnSave.Visible = false;
            btnIn.Visible = false;
            btnOut.Visible = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnInsert.Enabled = true;
        }

        #endregion


    }
}