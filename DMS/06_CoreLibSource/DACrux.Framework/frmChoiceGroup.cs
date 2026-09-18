using System;
using System.Data;
using System.Windows.Forms;
//using DACrux.Utility;



namespace DACrux.Framework
{
    /// <summary>
    /// Group Selection Delegate
    /// </summary>
    /// <param name="strGroupName">Selected Group Name</param>
    /// <param name="strGroupCode">Selected Group Code</param>
    public delegate void GroupSelect_Event(string strGroupName, string strGroupCode);

    /// <summary>
    /// Class Name : frmChoiceGroup<br/>
    /// Summary    : User Security Group Selection WinForm Class<br/>
    /// Author     : Miracom Hyuntai, Kim<br/>
    /// First Date : 2009-02-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmChoiceGroup : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region Class Member

        string strDelGrp = string.Empty;
        public event GroupSelect_Event On_Group_Selected;

        DataTable dtGrpLst = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmChoiceGroup(string strGrp)
        {
            InitializeComponent();

            LoadGroup(strGrp);
        }

        #endregion

        #region Group List Loading

        /// <summary>
        /// Get Group List (strGrpCode = None -> All Group)
        /// </summary>
        public void LoadGroup(string strGrpCode)
        {
            DACrux.Framework.RO.UserGroup oUserGroup = new DACrux.Framework.RO.UserGroup();
            dtGrpLst = oUserGroup.LoadGroupList(strGrpCode);
            lstGroup.ValueMember = "SEC_GRP_ID";
            lstGroup.DisplayMember = "SEC_GRP_DESC";
            lstGroup.DataSource = dtGrpLst;
        }

        #endregion

        #region Button Event

        /// <summary>
        /// Form Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Exit After Group Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            On_Group_Selected(dtGrpLst.Rows[lstGroup.SelectedIndex][1].ToString(), dtGrpLst.Rows[lstGroup.SelectedIndex][0].ToString());
            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region Group Selection Event

        /// <summary>
        /// Show Group Description By Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lslSecGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = null;
            string strSelVal = string.Empty;
            DACrux.Framework.RO.UserGroup oUserGroup = null;

            try
            {
                strSelVal = lstGroup.SelectedValue.ToString();

                oUserGroup = new DACrux.Framework.RO.UserGroup();
                dt = oUserGroup.LoadGroupInfo(strSelVal);

                txtGrpCode.Text = dt.Rows[0]["sec_grp_id"].ToString();
                txtGrpName.Text = dt.Rows[0]["sec_grp_desc"].ToString();
                txtGrpCaption.Text = dt.Rows[0]["remark"].ToString();
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

                oUserGroup = null;
            }
        }

        #endregion
    }
}