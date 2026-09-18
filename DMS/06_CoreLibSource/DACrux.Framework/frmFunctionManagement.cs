using System;
using System.Data;
using System.Windows.Forms;
//using DACrux.Utility;

namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : frmFunctionManagement<br/>
    /// Summary    : Function Management WinForm Class<br/>
    /// Author     : Miracom AndyKuo<br/>
    /// First Date : 2009-02-25<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmFunctionManagement : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region Class Member

        bool bInstEdit = true;
        DataTable dtFuncLst = null;
        string strOldFuncCode = string.Empty;
        string strOldFuncName = string.Empty;
        string strOldHelpUrl = string.Empty;


        #endregion



        #region Form Event

        #region Creator
        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmFunctionManagement()
        {
            InitializeComponent();
        }

        private void frmFunctionManagement_Load(object sender, EventArgs e)
        {

            LoadFunctionList();
        }
        #endregion

        #endregion

        #region Button Event

        #region Save Button

        /// <summary>
        /// Save New Funciton or Update Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bInsert = false;
            bool bUpdate = false;
            DACrux.Framework.RO.FunctionManagement oFunMgt = null;
            string strFuncCode = string.Empty;
            string strFuncName = string.Empty;
            string strHelpUrl = string.Empty;
            oFunMgt = new DACrux.Framework.RO.FunctionManagement();
            DataTable dtTmp = null;

            try
            {
                strFuncCode = txtFunctionCode.Text.Trim().ToUpper();
                strFuncName = txtFunctionName.Text.Trim();
                strHelpUrl = txtHelpUrl.Text.Trim();
                //************************************************
                //Check Function Code is not null
                if (strFuncCode == "")
                {
                    MessageBox.Show("Please Input the Function Code.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    txtFunctionCode.Focus();
                    return;
                }
                //************************************************
                //Check Function Name is not null
                if (strFuncName == "")
                {
                    MessageBox.Show("Please Input the Function Name.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    txtFunctionName.Focus();
                    return;
                }
                //************************************************
                //Check Function Code's Existence For Insert
                if (bInstEdit == true)
                {
                    dtTmp = oFunMgt.CheckFuncCodeExistence(strFuncCode);

                    if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                    {
                        MessageBox.Show("The Function Code already exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        txtFunctionCode.Focus();
                        return;
                    }
                }
                //************************************************
                //Check Function Name's Existence For Insert
                if (bInstEdit == true)
                {
                    dtTmp = oFunMgt.CheckFuncNameExistence(strFuncName);

                    if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                    {
                        MessageBox.Show("The Function Name already exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        txtFunctionName.Focus();
                        return;
                    }
                }
                //************************************************
                //Check Function Code&Name For Update
                if (bInstEdit == false)
                {
                    if (strOldFuncCode == strFuncCode)
                    {
                        if (strOldFuncName == strFuncName)
                        {
                            if (strHelpUrl == strOldHelpUrl)
                            {
                                MessageBox.Show("Nothing To Update", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }
                        else
                        {
                            dtTmp = oFunMgt.CheckFuncNameExistence(strFuncName);

                            if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                            {
                                MessageBox.Show("The Function Name already Exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                txtFunctionName.Text = "";
                                txtFunctionName.Focus();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strOldFuncName == strFuncName)
                        {
                            dtTmp = oFunMgt.CheckFuncCodeExistence(strFuncCode);
                            if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                            {
                                MessageBox.Show("The Function Code already Exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                txtFunctionCode.Text = "";
                                txtFunctionCode.Focus();
                                return;
                            }
                        }
                        else
                        {
                            dtTmp = oFunMgt.CheckFuncCodeExistence(strFuncCode);
                            if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                            {
                                MessageBox.Show("The Function Code already Exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                txtFunctionCode.Text = "";
                                txtFunctionCode.Focus();
                                return;
                            }
                            dtTmp = null;
                            dtTmp = oFunMgt.CheckFuncNameExistence(strFuncName);

                            if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                            {
                                MessageBox.Show("The Function Name already Exist", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                txtFunctionName.Text = "";
                                txtFunctionName.Focus();
                                return;
                            }
                        }
                    }
                }

                //************************************************
                if (bInstEdit) // Add Function
                {
                    DialogResult DiRslt = MessageBox.Show("Do you want create it?", "Insert Function", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (DiRslt == DialogResult.OK)
                    {
                        bInsert = oFunMgt.InsertFunction(strFuncCode, strFuncName, strHelpUrl);
                    }
                }
                else // Update Function
                {
                    DialogResult DiRslt = MessageBox.Show("Do you want modify it?", "Update Function", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (DiRslt == DialogResult.OK)
                    {
                        if (strOldFuncCode == strFuncCode)
                        {
                            bUpdate = oFunMgt.UpdateFunction(strFuncCode, strFuncName, strHelpUrl, strOldFuncCode);
                        }
                        else
                        {
                            dtTmp = oFunMgt.CheckAttachedFunction(strOldFuncCode);
                            if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                            {

                                DialogResult DiRBox = MessageBox.Show("The Function Have Attached To Security Group, Do you want to Continue?", "Notice:Modify Security Group & Function", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                if (DiRBox == DialogResult.Yes)
                                {
                                    bUpdate = oFunMgt.UpdateFunction(strFuncCode, strFuncName, strHelpUrl, strOldFuncCode);
                                    bUpdate = oFunMgt.UpdateGroupFunction(strFuncCode, strOldFuncCode);
                                }
                            }
                            else
                            {
                                bUpdate = oFunMgt.UpdateFunction(strFuncCode, strFuncName, strHelpUrl, strOldFuncCode);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (bInsert == true || bUpdate == true)
                {
                    DisableControl();
                    TextBoxClear();
                    LoadFunctionList();
                }
            }
        }
        #endregion

        #region Insert Button

        /// <summary>
        /// Create New Funciton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {

            string strTemp = string.Empty;
            bInstEdit = true;
            TextBoxClear();
            EnableControl();
            if (dtFuncLst.Rows.Count > 0)
            {
                strTemp = dtFuncLst.Rows[dtFuncLst.Rows.Count - 1]["FUNC_CODE"].ToString().Trim();
                strTemp = strTemp.Substring(1, strTemp.Length - 1);
                txtFunctionCode.Text = String.Format("F{0:0000}", System.Convert.ToInt32(strTemp) + 1);
            }
            else
            {
                txtFunctionCode.Text = "F0001";
            }
            txtFunctionName.Focus();
        }
        #endregion

        #region Update Button

        /// <summary>
        /// Update Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (txtFunctionCode.Text.Trim() == "")
            {
                MessageBox.Show("Please Choose a Function For Update.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                strOldFuncCode = txtFunctionCode.Text.Trim();
                strOldFuncName = txtFunctionName.Text.Trim();
                strOldHelpUrl = txtHelpUrl.Text.Trim();
                EnableControl();
                txtFunctionCode.Focus();
                bInstEdit = false;
            }

        }
        #endregion

        #region Delete Button

        /// <summary>
        /// Delete Funciton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool bDelGrpFunc = false;
            bool bDelFunc = false;
            DACrux.Framework.RO.FunctionManagement oFunMgt = null;
            string strFuncCode = string.Empty;

            DataTable dtTmp = null;

            try
            {
                oFunMgt = new DACrux.Framework.RO.FunctionManagement();
                strFuncCode = txtFunctionCode.Text.Trim().ToUpper();

                if (strFuncCode == "")
                {
                    MessageBox.Show("Please Input the Function Code.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    txtFunctionCode.Focus();
                    return;
                }


                DialogResult DiRslt = MessageBox.Show("Do you want delete it?", "Delete Function", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (DiRslt == DialogResult.OK)
                {

                    dtTmp = oFunMgt.CheckAttachedFunction(strFuncCode);
                    if (System.Convert.ToInt32(dtTmp.Rows.Count) != 0)
                    {

                        DialogResult DiRBox = MessageBox.Show("The Function Have Attached To Security Group, Do you want to Continue?", "Notice:Remove Function From Attached Security Group", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (DiRBox == DialogResult.Yes)
                        {
                            bDelGrpFunc = oFunMgt.DeleteGroupFunction(strFuncCode);
                            bDelFunc = oFunMgt.DeleteFunction(strFuncCode);
                        }
                    }
                    else
                    {
                        bDelFunc = oFunMgt.DeleteFunction(strFuncCode);
                    }

                }
            }
            finally
            {
                if (bDelFunc == true || bDelGrpFunc == true)
                {
                    DisableControl();
                    TextBoxClear();
                    LoadFunctionList();
                }
            }
        }
        #endregion

        #region Reset Button

        /// <summary>
        /// Cancel Modify of Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            TextBoxClear();
            LoadFunctionList();
            DisableControl();
        }
        #endregion

        #region Close Button

        /// <summary>
        /// Close the Function Management
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region Function Initial

        #region LoadFunctionList

        /// <summary>
        /// Get All Function List (strFucCode : None -> All Function)
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>N/A</returns>
        public void LoadFunctionList()
        {
            DACrux.Framework.RO.FunctionManagement oFuncMgt = null;
            int i;
            string strTemp = string.Empty;


            oFuncMgt = new DACrux.Framework.RO.FunctionManagement();
            dtFuncLst = oFuncMgt.LoadFunctionList();

            if (dtFuncLst == null || dtFuncLst.Rows.Count == 0) return;

            lstFunction.Items.Clear();
            for (i = 0; i < dtFuncLst.Rows.Count; i++)
            {
                strTemp = System.Convert.ToString(dtFuncLst.Rows[i][0]) + "   " + System.Convert.ToString(dtFuncLst.Rows[i][1]);
                lstFunction.Items.Add(strTemp);
            }
        }
        #endregion

        #region TextBoxClear

        /// <summary>
        /// Reset Control
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>N/A</returns>
        public void TextBoxClear()
        {
            txtFunctionCode.Text = "";
            txtFunctionName.Text = "";
            txtHelpUrl.Text = "";
        }

        #endregion

        #endregion

        #region SetControl
        /// <summary>
        /// Enable Control
        /// </summary>
        public void EnableControl()
        {
            txtFunctionCode.ReadOnly = false;
            txtFunctionName.ReadOnly = false;
            txtHelpUrl.ReadOnly = false;
            btnSave.Enabled = true;
            btnSave.Visible = true;
            btnReset.Enabled = true;
            btnReset.Visible = true;
            btnInsert.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        /// <summary>
        /// Disable Control
        /// </summary>
        public void DisableControl()
        {
            txtFunctionCode.ReadOnly = true;
            txtFunctionName.ReadOnly = true;
            txtHelpUrl.ReadOnly = true;
            btnSave.Enabled = false;
            btnSave.Visible = false;
            btnReset.Enabled = false;
            btnReset.Visible = false;
            btnInsert.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }
        #endregion

        #region Control Event

        /// <summary>
        /// Get Selected Function Information By ListBoxItem Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFuncCode = string.Empty;
            string strSQL = string.Empty;
            string strFuncName = string.Empty;
            string strHelpUrl = string.Empty;
            string strSelected = string.Empty;
            DataRow[] drFunc = null;

            if (lstFunction.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Function ID.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            strSelected = lstFunction.SelectedItem.ToString().Trim();
            int iLength = strSelected.IndexOf(" ");
            strFuncCode = strSelected.Substring(0, iLength);

            strSQL = "FUNC_CODE = '" + strFuncCode + "'";

            drFunc = dtFuncLst.Select(strSQL);
            txtFunctionCode.Text = drFunc[0].ItemArray[0].ToString();
            txtFunctionName.Text = drFunc[0].ItemArray[1].ToString();
            txtHelpUrl.Text = drFunc[0].ItemArray[2].ToString();
            drFunc = null;

        }
        #endregion

    }
}