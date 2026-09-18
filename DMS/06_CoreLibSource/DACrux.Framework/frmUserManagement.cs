using System;
using System.Data;
using System.Windows.Forms;
using DACrux.Base;
//using DACrux.Utility;

using DACrux.Framework.RO;


namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : frmUserManagement<br/>
    /// Summary    : User Management WinForm Class<br/>
    /// Author     : Miracom Hyuntai, Kim<br/>
    /// First Date : 2009-02-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmUserManagement : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region Class Member

        //UserManagement oUserManagement = null;
        //UserGroup oUserGroup = null;
        //UserEnvironment oUserEnv = null;

        bool bInstEdit = true;

        #endregion


        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmUserManagement()
        {
            InitializeComponent();
        }

        private void frmUserManagement_Load(object sender, EventArgs e)
        {
            try
            {
                //oUserManagement = new UserManagement();
                //oUserGroup = new UserGroup();
                //oUserEnv = new UserEnvironment();

                LoadUserList();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        #endregion

        #region Load User List

        /// <summary>
        /// Get All User List
        /// </summary>
        public void LoadUserList()
        {
            DataTable dtGrp = null;
            DataTable dtByGrp = null;
            DataTable dtUsrName = null;

            UserGroup oUserGroup = null;

            int iCntGrp;
            int iCntUsr;

            string strTemp = string.Empty;
            string strSQL = string.Empty;

            DataRow[] dr = null;
            DataRow[] drUsrName = null;

            TreeNode[] oUser = null;

            try
            {
                oUserGroup = new UserGroup();

                dtGrp = oUserGroup.LoadGroupList("None");
                dtByGrp = oUserGroup.CountByGroup();
                dtUsrName = oUserGroup.LoadJoinUserName();

                iCntGrp = dtGrp.Rows.Count;

                TreeNode[] oGroupNode = new TreeNode[iCntGrp];

                for (int i = 0; i < iCntGrp; i++)
                {
                    strTemp = (string)dtGrp.Rows[i]["SEC_GRP_ID"];
                    strSQL = "SEC_GRP_ID = '" + strTemp + "'";
                    dr = dtByGrp.Select(strSQL);

                    if (dr.Length != 1)
                        iCntUsr = 0;
                    else
                        iCntUsr = System.Convert.ToInt32(dr[0]["CNT"]);

                    oUser = new TreeNode[iCntUsr];
                    drUsrName = dtUsrName.Select(strSQL);

                    for (int j = 0; j < iCntUsr; j++)
                    {
                        string strName = string.Empty;
                        string strID = string.Empty;
                        strName = (string)drUsrName[j]["USER_NAME"];
                        strID = (string)drUsrName[j]["USER_ID"];
                        oUser[j] = new TreeNode(strName + "(" + strID + ")", 5, 5);
                    }

                    string strGrpName = (string)dtGrp.Rows[i]["SEC_GRP_DESC"];
                    oGroupNode[i] = new TreeNode(strGrpName, 0, 1, oUser);
                    oUser = null;
                }

                tvUserList.Nodes.Clear();
                tvUserList.BeginUpdate();
                tvUserList.Nodes.AddRange(oGroupNode);

                if (chkExpand.Checked)
                {
                    tvUserList.ExpandAll();
                }
                else
                {
                    tvUserList.CollapseAll();
                }

                if (tvUserList.Nodes.Count > 0)
                    tvUserList.SelectedNode = tvUserList.Nodes[0];
                tvUserList.EndUpdate();

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                if (dtGrp != null) dtGrp.Dispose();
                if (dtByGrp != null) dtByGrp.Dispose();
                if (dtUsrName != null) dtUsrName.Dispose();

                dtGrp = null;
                dtByGrp = null;
                dtUsrName = null;

                if (oUserGroup != null) oUserGroup = null;

                tvUserList.Select();
            }

        }

        public void LoadUserList(DataTable dtGrpResult, DataTable dtGrpList)
        {
            DataTable dtGrp = null;
            int iCntGrp;
            int iCntUsr;
            string strTemp = string.Empty;
            string strSQL = string.Empty;

            DataRow[] drUsrName = null;
            DataRow[] drGrpName = null;
            TreeNode[] oUser = null;

            UserGroup oUserGroup = null;

            try
            {
                oUserGroup = new UserGroup();

                dtGrp = oUserGroup.LoadGroupList("None");
                iCntGrp = dtGrpList.Rows.Count;

                TreeNode[] oGroupNode = new TreeNode[iCntGrp];

                string strName = string.Empty;
                string strID = string.Empty;
                string strGrpName = string.Empty;

                for (int i = 0; i < iCntGrp; i++)
                {
                    strTemp = (string)dtGrpList.Rows[i]["SEC_GRP_ID"];
                    iCntUsr = System.Convert.ToInt32(dtGrpList.Rows[i]["CNT"]);
                    oUser = new TreeNode[iCntUsr];
                    strSQL = "SEC_GRP_ID = '" + strTemp + "'";

                    drUsrName = dtGrpResult.Select(strSQL);

                    for (int j = 0; j < iCntUsr; j++)
                    {

                        strName = (string)drUsrName[j]["USER_NAME"];
                        strID = (string)drUsrName[j]["USER_ID"];
                        oUser[j] = new TreeNode(strName + " (" + strID + ")", 5, 5);
                    }
                    drGrpName = dtGrp.Select(strSQL);
                    strGrpName = (string)drGrpName[0]["SEC_GRP_DESC"];
                    oGroupNode[i] = new TreeNode(strGrpName, 0, 1, oUser);
                    oUser = null;
                }

                tvUserList.Nodes.Clear();
                tvUserList.BeginUpdate();
                tvUserList.Nodes.AddRange(oGroupNode);
                tvUserList.ExpandAll();
                tvUserList.EndUpdate();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtGrp.Dispose();
                dtGrp = null;
                drUsrName = null;
                drGrpName = null;
                tvUserList.Select();
            }

        }
        #endregion

        #region LoadDepartmentUserList

        /// <summary>
        /// Get User List By Department
        /// </summary>
        public void LoadDepartmentUserList()
        {
            DataTable dtByDep = null;
            DataTable dtSecUsr = null;
            DataTable dtUsrName = null;

            int iCntDep;
            int iCntUsr;

            string strTemp = string.Empty;
            string strSQL = string.Empty;

            DataRow[] dr = null;
            DataRow[] drUsrName = null;

            TreeNode[] oUser = null;
            UserGroup oUserGroup = null;

            try
            {
                oUserGroup = new UserGroup();

                dtByDep = oUserGroup.CountByDepartment();
                dtSecUsr = oUserGroup.LoadSecurityUser();
                dtUsrName = oUserGroup.LoadJoinUserName2();

                iCntDep = dtByDep.Rows.Count;

                TreeNode[] oGroupNode = new TreeNode[iCntDep];

                for (int i = 0; i < iCntDep; i++)
                {

                    strTemp = (string)dtByDep.Rows[i]["USER_CMF_1"];


                    strSQL = "USER_CMF_1 = '" + strTemp + "'";

                    dr = dtByDep.Select(strSQL);
                    iCntUsr = System.Convert.ToInt32(dr[0]["CNT"]);

                    oUser = new TreeNode[iCntUsr];
                    drUsrName = dtUsrName.Select(strSQL);

                    for (int j = 0; j < iCntUsr; j++)
                    {
                        string strName = string.Empty;
                        string strID = string.Empty;
                        strName = (string)drUsrName[j]["USER_NAME"];
                        strID = (string)drUsrName[j]["USER_ID"];
                        oUser[j] = new TreeNode(strName + "(" + strID + ")", 5, 5);
                    }

                    string strDepName = string.Empty;


                    strDepName = (string)dtByDep.Rows[i]["USER_CMF_1"];

                    oGroupNode[i] = new TreeNode(strDepName, 0, 1, oUser);
                    oUser = null;
                }

                tvUserList.Nodes.Clear();
                tvUserList.BeginUpdate();
                tvUserList.Nodes.AddRange(oGroupNode);

                if (chkExpand.Checked)
                {
                    tvUserList.ExpandAll();
                }
                else
                {
                    tvUserList.CollapseAll();
                }

                tvUserList.SelectedNode = tvUserList.Nodes[0];
                tvUserList.EndUpdate();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {

                dtByDep.Dispose();
                dtSecUsr.Dispose();
                dtUsrName.Dispose();
                dtByDep = null;
                dtSecUsr = null;
                dtUsrName = null;
                tvUserList.Select();
            }
        }

        public void LoadDepartmentUserList(DataTable dtDepResult, DataTable dtDepList)
        {
            int iCntDep;
            int iCntUsr;

            string strTemp = string.Empty;
            string strSQL = string.Empty;

            DataRow[] drUsrName = null;

            TreeNode[] oUser = null;

            string strName = string.Empty;
            string strID = string.Empty;
            string strDepName = string.Empty;

            try
            {
                iCntDep = dtDepList.Rows.Count;
                TreeNode[] oDepNode = new TreeNode[iCntDep];

                for (int i = 0; i < iCntDep; i++)
                {

                    strTemp = (string)dtDepList.Rows[i]["USER_CMF_1"];
                    strSQL = "USER_CMF_1 = '" + strTemp + "'";

                    iCntUsr = System.Convert.ToInt32(dtDepList.Rows[i]["CNT"]);

                    oUser = new TreeNode[iCntUsr];
                    strSQL = "USER_CMF_1 = '" + strTemp + "'";

                    drUsrName = dtDepResult.Select(strSQL);

                    for (int j = 0; j < iCntUsr; j++)
                    {
                        strName = (string)drUsrName[j]["USER_NAME"];
                        strID = (string)drUsrName[j]["USER_ID"];
                        oUser[j] = new TreeNode(strName + "(" + strID + ")", 5, 5);
                    }

                    strDepName = (string)dtDepList.Rows[i]["USER_CMF_1"];

                    oDepNode[i] = new TreeNode(strDepName, 0, 1, oUser);
                    oUser = null;
                }

                tvUserList.Nodes.Clear();
                tvUserList.BeginUpdate();
                tvUserList.Nodes.AddRange(oDepNode);
                tvUserList.ExpandAll();
                tvUserList.EndUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                drUsrName = null;
                tvUserList.Select();
            }
        }



        #endregion

        #region Set Control

        #region EnableControl

        /// <summary>
        /// Enable Control
        /// </summary>
        public void EnableControl()
        {
            txtUserID.ReadOnly = false;
            txtPassword1.ReadOnly = false;
            txtUserName.ReadOnly = false;
            txtDepartment.ReadOnly = false;
            txtPhoneOffice.ReadOnly = false;
            txtPhoneETC.ReadOnly = false;
            txtPhoneHome.ReadOnly = false;
            txtPhoneMobile.ReadOnly = false;
            txtEmail.ReadOnly = false;

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnSave.Visible = true;
            btnDelete.Enabled = false;
            btnGroupList.Enabled = true;
            btnGroupList.Visible = true;
            btnSelectDel.Visible = true;
            btnReset.Enabled = true;
            btnReset.Visible = true;
            btnAdd.Enabled = false;
        }
        #endregion

        #region DisableControl

        /// <summary>
        /// Disable Control
        /// </summary>
        public void DisableControl()
        {
            txtUserID.ReadOnly = true;
            txtPassword1.ReadOnly = true;
            txtUserName.ReadOnly = true;
            txtDepartment.ReadOnly = true;
            txtPhoneOffice.ReadOnly = true;
            txtPhoneETC.ReadOnly = true;
            txtPhoneHome.ReadOnly = true;
            txtPhoneMobile.ReadOnly = true;
            txtEmail.ReadOnly = false;

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
            btnSave.Visible = false;
            btnGroupList.Enabled = false;
            btnGroupList.Visible = false;
            btnSelectDel.Visible = false;
            btnReset.Enabled = false;
            btnReset.Visible = false;
            btnAdd.Enabled = true;
        }
        #endregion

        #endregion

        #region Button Click Event

        #region btnSearch_Click

        /// <summary>
        /// Search Users By keyword
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strSearch = string.Empty;
            string strSearchLike = string.Empty;

            DataTable dtGrpResult = null;
            DataTable dtGrpList = null;

            UserGroup oUserGroup = null;

            try
            {
                strSearch = txtSearch.Text.Trim();
                if (strSearch == "")
                {
                    txtSearch.Text = "All";
                    strSearch = txtSearch.Text.Trim();
                }

                oUserGroup = new UserGroup();

                if (rdoBySecGroup.Checked)
                {

                    if (strSearch.ToUpper() == "ALL")
                    {
                        LoadUserList();
                    }
                    else
                    {
                        strSearchLike = string.Empty;
                        strSearchLike = "%" + strSearch + "%";

                        dtGrpResult = oUserGroup.SearchGroupUser(strSearchLike);
                        dtGrpList = oUserGroup.SearchGroupList(strSearchLike);

                        LoadUserList(dtGrpResult, dtGrpList);

                        dtGrpResult.Dispose();
                        dtGrpResult = null;
                        dtGrpList.Dispose();
                        dtGrpList = null;
                    }
                }
                else
                {
                    if (strSearch.ToUpper() == "ALL")
                    {
                        LoadDepartmentUserList();
                    }
                    else
                    {
                        strSearchLike = string.Empty;
                        strSearchLike = "%" + strSearch + "%";
                        DataTable dtDepResult = null;
                        DataTable dtDepList = null;
                        dtDepResult = oUserGroup.SearchDepartmentUser(strSearchLike);
                        dtDepList = oUserGroup.SearchDepartmentList(strSearchLike);

                        LoadDepartmentUserList(dtDepResult, dtDepList);

                        dtDepResult.Dispose();
                        dtDepResult = null;
                        dtDepList.Dispose();
                        dtDepList = null;
                    }
                }
            }

            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                TextBoxClear();
                tvUserList.Select();

            }
        }

        #endregion

        #region btnUpdate_Click

        /// <summary>
        /// Set Control to update User Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tvUserList.SelectedNode != null)
            {
                int iNodeCnt;
                iNodeCnt = tvUserList.SelectedNode.GetNodeCount(true);
                object oPar;

                oPar = tvUserList.SelectedNode.Parent;

                if (iNodeCnt == 0 & oPar != null)
                {
                    try
                    {
                        bInstEdit = false;
                        EnableControl();
                        txtUserID.ReadOnly = true;
                        txtPassword1.Focus();
                    }
                    catch (Exception ex)
                    {
                        DspError(ex);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Select user to edit", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Select user to edit", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        #endregion

        #region btnReset_Click

        /// <summary>
        /// Control Reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            TextBoxClear();
        }

        #endregion

        #region btnSave_Click

        /// <summary>
        /// Save Current Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            string strUserID = txtUserID.Text.ToUpper().Trim();

            if (String.IsNullOrEmpty(strUserID))
            {
                MessageBox.Show("Input the user id", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtUserID.Focus();
                return;
            }

            if (!bInstEdit
                && String.Equals(strUserID, "Admin")
                && !String.Equals(DACrux.Base.GlobalVariable.UserID, "Admin"))
            {
                MessageBox.Show("It is admin account. It can not be modified.");
                return;
            }

            bool bFlag = false;
            UserGroup oUserGroup = new UserGroup();

            if (bInstEdit)
            {
                bFlag = oUserGroup.CheckID(strUserID);
                if (bFlag)
                {
                    MessageBox.Show("The user id already exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtUserID.Focus();
                    return;
                }
            }

            string strUserName = txtUserName.Text.Trim();

            if (String.IsNullOrEmpty(strUserName))
            {
                MessageBox.Show("Input user name", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtUserName.Focus();
                return;
            }
            string strPass = txtPassword1.Text.Trim();
            string strGrp = txtDepartment.Text.Trim();
            if (String.IsNullOrEmpty(strGrp))
                strGrp = "No Group";

            string strPnOffice = txtPhoneOffice.Text.Trim();
            string strPnMobile = txtPhoneMobile.Text.Trim();
            string strPnHome = txtPhoneHome.Text.Trim();
            string strPnOther = txtPhoneETC.Text.Trim();
            string strEmail = txtEmail.Text.Trim();

            if (String.IsNullOrEmpty(strPnOffice))
            {
                MessageBox.Show("Input office phone number", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtPhoneOffice.Focus();
                return;
            }
            if (String.IsNullOrEmpty(strPnMobile))
            {
                MessageBox.Show("Input mobile phone number", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtPhoneMobile.Focus();
                return;
            }
            if (String.IsNullOrEmpty(strEmail))
            {
                MessageBox.Show("Input email address", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtEmail.Focus();
                return;
            }


            //string strPnOffice = "Please correct";
            //string strPnMobile = "Please correct";
            //string strPnHome = "Please correct";
            //string strPnOther = "Please correct";
            //string strEmail = "Please correct";

            int iListCnt = lslSecGroup.Items.Count;

            if (iListCnt == 0)
            {
                MessageBox.Show("Assign user one group or more", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            string[] arrGrpName = new string[iListCnt];

            bool a = true;
            bool b = true;
            bool b1 = true;

            UserManagement oUserManagement = null;
            DataTable dtGrpName = null;

            try
            {
                dtGrpName = (oUserGroup.LoadGroupList("None"));

                DataRow[] dr = null;
                string strGrpCode = string.Empty;
                string strSQL = string.Empty;

                if (bInstEdit) //추가
                {
                    a = oUserGroup.InsertUser(strUserID, strUserName, DACrux.Base.Crypt.GetEncoding(strPass), strGrp, strPnOffice, strPnMobile, strPnHome, strPnOther, strEmail);

                    for (int i = 0; i < iListCnt; i++)
                    {
                        arrGrpName[i] = lslSecGroup.Items[i].ToString();
                        strSQL = "SEC_GRP_DESC = '" + arrGrpName[i] + "'";
                        dr = dtGrpName.Select(strSQL);
                        strGrpCode = dr[0]["SEC_GRP_ID"].ToString();

                        b1 = oUserGroup.InsertGroupUser(strGrpCode, strUserID);

                        if (!b1) { b = false; }
                    }

                    MessageBox.Show("The user added successfuly", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else  //수정
                {

                    bool c = true;
                    bool d = true;

                    string strSelected = tvUserList.SelectedNode.Text;

                    string strID = string.Empty;

                    strID = Pars("(", strSelected);
                    c = oUserGroup.DeleteGroupUser(strID);

                    oUserManagement = new UserManagement();

                    if (strPass != string.Empty)
                        d = oUserManagement.UpdateUser(" USER_NAME= '" + strUserName + "', PASSWORD= '" + DACrux.Base.Crypt.GetEncoding(strPass) + "', PHONE_OFFICE = '" + strPnOffice + "', PHONE_MOBILE = '" + strPnMobile + "', PHONE_HOME = '" + strPnHome + "', PHONE_OTHER = '" + strPnOther + "',  EMAIL_ID =  '" + strEmail + "',  USER_CMF_1 =  '" + strGrp + "'", strID);
                    else
                        d = oUserManagement.UpdateUser(" USER_NAME= '" + strUserName + "', PHONE_OFFICE = '" + strPnOffice + "', PHONE_MOBILE = '" + strPnMobile + "', PHONE_HOME = '" + strPnHome + "', PHONE_OTHER = '" + strPnOther + "',  EMAIL_ID =  '" + strEmail + "',  USER_CMF_1 =  '" + strGrp + "'", strID);

                    for (int i = 0; i < iListCnt; i++)
                    {
                        arrGrpName[i] = lslSecGroup.Items[i].ToString();
                        strSQL = "SEC_GRP_DESC = '" + arrGrpName[i] + "'";
                        dr = dtGrpName.Select(strSQL);
                        strGrpCode = dr[0]["SEC_GRP_ID"].ToString();
                        b1 = oUserGroup.InsertGroupUser(strGrpCode, strUserID);
                        if (!b1) { b = false; }
                    }

                    MessageBox.Show("The user information modified successfuly", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

                if (!b) MessageBox.Show("Failed.");
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                if (rdoBySecGroup.Checked)
                    LoadUserList();
                else
                    LoadDepartmentUserList();

                DisableControl();
                txtSearch.Text = string.Empty;
                if (dtGrpName != null) dtGrpName.Dispose();
                dtGrpName = null;
                if (oUserManagement != null) oUserManagement = null;
            }
        }

        #endregion

        #region btnAdd_Click

        /// <summary>
        /// Set Control to add user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bInstEdit = true;
                TextBoxClear();
                EnableControl();
                txtUserID.Focus();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        #endregion

        #region btnGroupList_Click

        /// <summary>
        /// Show Group Selection Box to choice user Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupList_Click(object sender, EventArgs e)
        {
            //frmSecurityGroupSelector oForm = new frmSecurityGroupSelector("None");
            //oForm.On_Group_Selected += new GroupSelect_Event(oForm_On_Group_Selected);
            //oForm.ShowDialog();
            frmChoiceGroup oForm = new frmChoiceGroup("None");
            oForm.On_Group_Selected += new GroupSelect_Event(oForm_On_Group_Selected);
            oForm.ShowDialog();
        }

        /// <summary>
        /// Group Selection delegate event
        /// </summary>
        /// <param name="strGroupName"></param>
        /// <param name="strGroupCode"></param>
        private void oForm_On_Group_Selected(string strGroupName, string strGroupCode)
        {
            int iListGrp;

            iListGrp = lslSecGroup.Items.Count;

            for (int i = 0; i < iListGrp; i++)
            {
                string strListItem = string.Empty;
                strListItem = lslSecGroup.Items[i].ToString();

                if (strListItem == strGroupName)
                {
                    MessageBox.Show("The group name already exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            lslSecGroup.Items.Add(strGroupName);
        }

        #endregion

        #region btnSelectDel_Click

        /// <summary>
        /// Delete Selected Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectDel_Click(object sender, EventArgs e)
        {
            if (btnSave.Visible == true)
            {
                if (lslSecGroup.SelectedIndex != -1)
                {
                    lslSecGroup.Items.RemoveAt(lslSecGroup.SelectedIndex);
                }
            }
        }

        #endregion

        #region btnDelete_Click

        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tvUserList.SelectedNode != null)
            {
                if (txtUserID.Text == "Admin" && DACrux.Base.GlobalVariable.UserID != "Admin")
                {
                    MessageBox.Show("It is admin account. It can not be deleted.");
                    return;
                }

                int iNodeCnt;

                iNodeCnt = tvUserList.SelectedNode.GetNodeCount(true);

                object oPar;
                oPar = tvUserList.SelectedNode.Parent;

                if (iNodeCnt == 0 & oPar != null)
                {
                    bool a = true;
                    bool b = true;

                    string strSelected = tvUserList.SelectedNode.Text;
                    string strUserID = string.Empty;

                    UserGroup oUserGroup = null;

                    try
                    {
                        DialogResult DiRslt = MessageBox.Show("Selected user will be deleted. Are you sure?", "Delete User", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                        if (DiRslt == DialogResult.OK)
                        {
                            oUserGroup = new UserGroup();

                            strUserID = Pars("(", strSelected);

                            a = oUserGroup.DeleteGroupUser(strUserID);
                            b = oUserGroup.DeleteUser(strUserID);
                        }
                    }
                    catch (Exception ex)
                    {
                        DspError(ex);
                    }
                    finally
                    {
                        LoadUserList();
                    }
                }
                else
                {
                    MessageBox.Show("You can delete the group on Group Management Function.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Please, Select user to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

        }

        #endregion

        #region Close Button

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

        #region Get User ID

        /// <summary>
        /// Get User ID on string
        /// </summary>
        /// <param name="strStart">Start Index</param>
        /// <param name="Value">string</param>
        /// <returns></returns>
        public string Pars(string strStart, string Value)
        {
            int iStart;
            int iLength;
            iStart = Value.IndexOf(strStart);
            iStart++;
            iLength = Value.Length - iStart - 1;
            string strValue = string.Empty;
            strValue = Value.Substring(iStart, iLength);
            return strValue;
        }

        #endregion

        #region tvUserList_AfterSelect

        /// <summary>
        /// Display Selected User Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvUserList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int iNodeCnt;
            iNodeCnt = tvUserList.SelectedNode.GetNodeCount(true);

            object oPar;
            oPar = tvUserList.SelectedNode.Parent;

            UserGroup oUserGroup = null;

            if (iNodeCnt == 0 & oPar != null)
            {
                oUserGroup = new UserGroup();

                DataTable dtAllUserInfo = null;
                DataTable dtJoinAllGrp = null;

                dtAllUserInfo = oUserGroup.LoadSecurityUser();
                dtJoinAllGrp = oUserGroup.LoadJoinAllGroup();

                TextBoxClear();
                DisableControl();

                string strSelected = tvUserList.SelectedNode.Text;
                string strUserID = string.Empty;

                DataRow[] dr = null;
                string strSQL = string.Empty;

                int iGrpCnt;

                try
                {
                    strUserID = Pars("(", strSelected);
                    strSQL = "USER_ID = '" + strUserID + "'";

                    dr = dtAllUserInfo.Select(strSQL);

                    if (dr.Length > 0)
                    {
                        if (dr[0]["USER_ID"] != DBNull.Value) txtUserID.Text = dr[0]["USER_ID"].ToString();
                        if (dr[0]["USER_NAME"] != DBNull.Value) txtUserName.Text = dr[0]["USER_NAME"].ToString();
                        if (dr[0]["PASSWORD"] != DBNull.Value) txtPassword1.Text = DACrux.Base.Crypt.GetDecoding(dr[0]["PASSWORD"].ToString());
                        if (dr[0]["USER_CMF_1"] != DBNull.Value) txtDepartment.Text = dr[0]["USER_CMF_1"].ToString();
                        if (dr[0]["PHONE_OFFICE"] != DBNull.Value) txtPhoneOffice.Text = dr[0]["PHONE_OFFICE"].ToString();
                        if (dr[0]["PHONE_OTHER"] != DBNull.Value) txtPhoneETC.Text = dr[0]["PHONE_OTHER"].ToString();
                        if (dr[0]["PHONE_HOME"] != DBNull.Value) txtPhoneHome.Text = dr[0]["PHONE_HOME"].ToString();
                        if (dr[0]["PHONE_MOBILE"] != DBNull.Value) txtPhoneMobile.Text = dr[0]["PHONE_MOBILE"].ToString();
                        if (dr[0]["EMAIL_ID"] != DBNull.Value) txtEmail.Text = dr[0]["EMAIL_ID"].ToString();
                    }

                    dr = null;
                    dr = dtJoinAllGrp.Select(strSQL);

                    iGrpCnt = dr.Length;

                    string strListVal = string.Empty;

                    for (int i = 0; i < iGrpCnt; i++)
                    {
                        strListVal = dr[i]["SEC_GRP_DESC"].ToString();
                        lslSecGroup.Items.Add(strListVal);
                    }
                }

                catch (Exception ex)
                {
                    DspError(ex);
                }
            }
            else
            {
                TextBoxClear();
            }
        }

        #endregion

        #region Text Box Handling

        /// <summary>
        /// Select Text When focused in txtSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
        }

        #endregion

        #region TextBoxClear

        /// <summary>
        /// Reset Control
        /// </summary>
        public void TextBoxClear()
        {
            txtUserID.Text = "";
            txtPassword1.Text = "";
            txtUserName.Text = "";
            txtDepartment.Text = "";
            txtPhoneOffice.Text = "";
            txtPhoneMobile.Text = "";
            txtPhoneHome.Text = "";
            txtPhoneETC.Text = "";
            txtEmail.Text = "";
            lslSecGroup.Items.Clear();
        }
        #endregion

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void frmUserManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

    }
}