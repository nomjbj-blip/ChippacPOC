using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// Alarm Notify User Assign 팝업 화면
    /// </summary>
    public partial class frmDefectAlarmSetup_Notify : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region 생성자 및 Load 이벤트

        /// <summary>
        /// 생성자
        /// </summary>
        public frmDefectAlarmSetup_Notify()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load 이벤트 발생 시 실행되는 메서드 입니다.
        /// </summary>
        private void frmDefectAlarmSetup_Notify_Load(object sender, EventArgs e)
        {
            LoadUserGroup();
            AppendUsers();
        }

        #endregion

        #region 사용자 정의 메서드

        /// <summary>
        /// 할당된 Node를 할당된 사용자 TreeView에 추가합니다.
        /// </summary>
        private void Add()
        {
            TreeNode focused = trvUserList.SelectedNode;

            if (focused == null)
                return;

            List<TreeNode> list = new List<TreeNode>();

            if (focused.Nodes.Count > 0)
            {
                if (MessageBox.Show(String.Format("그룹 '{0}'의 사용자를 추가하시겠습니까?", focused.Text), "추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (TreeNode node in focused.Nodes)
                    {
                        list.Add(node);
                    }
                }
            }
            else
            {
                list.Add(focused);
            }

            foreach (TreeNode selNode in list)
            {
                bool exists = false;

                foreach (TreeNode node in trvSelected.Nodes)
                {
                    if (node.Text == selNode.Text)
                    {
                        exists = true;
                        break;
                    }

                }

                if (!exists)
                    trvSelected.Nodes.Add(selNode.Clone() as TreeNode);
            }
        }

        /// <summary>
        /// 선택된 Node를 할당된 사용자 TreeView에서 제거합니다.
        /// </summary>
        private void Remove()
        {
            TreeNode node = trvSelected.SelectedNode;

            if (node == null)
                return;

            trvSelected.Nodes.Remove(node);
        }

        /// <summary>
        /// 지정된 사용자 아이디에 대한 TreeNode를 가져옵니다.
        /// </summary>
        private TreeNode GetNode(TreeNode node, string userId)
        {
            if (node.Parent == null)
            {
                foreach (TreeNode child in node.Nodes)
                {
                    node = GetNode(child, userId);

                    if (node != null)
                        return node;
                }
            }
            else
            {
                if (GetUserId(node.Text) == userId)
                    return node;
            }

            return null;
        }

        /// <summary>
        /// 지정된 텍스트에서 사용자 아이디만 가져옵니다.
        /// </summary>
        private string GetUserId(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            string splitter = "(";
            string trimChars = ")";

            if (text.IndexOf(splitter) < 0)
                return null;

            return text.Substring(text.IndexOf(splitter) + splitter.Length).Trim(trimChars.ToCharArray());
        }

        /// <summary>
        /// 사용자 그룹 정보를 바인딩 합니다.
        /// </summary>
        private void LoadUserGroup()
        {
            DataTable dtGrp = null;
            DataTable dtByGrp = null;
            DataTable dtUsrName = null;

            int iCntGrp;
            int iCntUsr;

            string strTemp = string.Empty;
            string strSQL = string.Empty;

            DataRow[] dr = null;
            DataRow[] drUsrName = null;

            TreeNode[] oUser = null;


            DACrux.Framework.RO.UserGroup oUserGroup = new DACrux.Framework.RO.UserGroup();

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

            trvUserList.BeginUpdate();
            trvUserList.Nodes.Clear();
            trvUserList.Nodes.AddRange(oGroupNode);
            trvUserList.ExpandAll();
            trvUserList.EndUpdate();

            if (trvUserList.Nodes.Count > 0)
                trvUserList.SelectedNode = trvUserList.Nodes[0];
        }

        /// <summary>
        /// 할당된 사용자 데이터를 TreeView에 추가합니다.
        /// </summary>
        private void AppendUsers()
        {
            trvSelected.Nodes.Clear();

            string[] assignUsers = GetAssignUserGroup();

            if (assignUsers == null)
                return;

            foreach (string userId in assignUsers)
            {
                foreach (TreeNode node in trvUserList.Nodes)
                {
                    TreeNode n = GetNode(node, userId);

                    if (n != null)
                    {
                        trvSelected.Nodes.Add(n.Clone() as TreeNode);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 할당된 사용자 데이터를 문자열 배열로 가져옵니다.
        /// </summary>
        private string[] GetAssignUserGroup()
        {

            DefectAlarm obj = new DefectAlarm();
            DataTable dt = obj.GetAlarmUser();

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = (string)dt.Rows[i][0];

            return arr;
        }

        #endregion

        #region 이벤트 처리 메서드

        /// <summary>
        /// Add 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        /// <summary>
        /// Remove 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            Remove();
        }

        /// <summary>
        /// 전체 사용자 TreeView 더블 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void trvUserList_DoubleClick(object sender, EventArgs e)
        {
            Add();
        }

        /// <summary>
        /// 할당된 사용자 TreeView 더블 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void trvSelected_DoubleClick(object sender, EventArgs e)
        {
            Remove();
        }

        /// <summary>
        /// Save 버튼 클릭 시 실행되는 메서드 입니다.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> assignUsers = new List<string>();

            foreach (TreeNode node in trvSelected.Nodes)
                assignUsers.Add(GetUserId(node.Text));

            DefectAlarm obj = new DefectAlarm();
            obj.DeleteAlarmUser();
            obj.InsertAlarmUser(GlobalVariable.Factory, assignUsers.ToArray(), GlobalVariable.UserID);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion
    }
}
