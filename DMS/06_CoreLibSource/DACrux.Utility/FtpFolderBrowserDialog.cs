using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace DACrux.Utility
{
    // FTP 폴더 탐색을 위한 Dialog 2019.08.15 Taihi,Kim.
    public partial class FtpFolderBrowserDialog : Form, IDisposable
    {
        public static string NEW_NODE_NAME = "새폴더";

        private bool _newAppend;

        public FtpFolderBrowserDialog()
        {
            InitializeComponent();

            FtpInfo = new ServerCommunicationFtp();
            lblStatus.Text = String.Empty;

            trvPath.ImageList = new ImageList();
            trvPath.ImageList.Images.Add(SystemIcons.GetFolderIcon(false).ToBitmap());
            trvPath.ImageList.Images.Add(SystemIcons.GetFolderIcon(true).ToBitmap());
            trvPath.ImageIndex = 0;
            trvPath.SelectedImageIndex = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(FtpInfo.Server))
            {
                ErrorMesssage("FTP 서버 정보가 입력되지 않았습니다.");
                DialogResult = DialogResult.None;
                return;
            }

            Visible = true;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                lblStatus.Visible = true;
                trvPath.Nodes.Clear();

                TreeNode root = trvPath.Nodes.Add(RootPath);
                UpdateStatus(RootPath);
                AppendDirectory(root);
                trvPath.SelectedNode = trvPath.TopNode;
                trvPath.ExpandAll();

                if (!String.IsNullOrEmpty(SelectedPath))
                    SetSelectedNode(trvPath.TopNode);
            }
            finally
            {
                lblStatus.Visible = false;
                Cursor.Current = Cursors.Default;
            }
        }

        public TreeNode GetDirectoryToTreeNode(string serverPath)
        {
            serverPath = FtpInfo.Normalize(serverPath);
            TreeNode root = new TreeNode(serverPath);
            AppendDirectory(root);

            return root;
        }

        private void AppendDirectory(TreeNode parent)
        {
            foreach (string dir in FtpInfo.GetDirectoryList(parent.FullPath))
            {
                TreeNode node = parent.Nodes.Add(dir);
                UpdateStatus(dir);
                //AppendDirectory(node);
            }
        }

        private bool SetSelectedNode(TreeNode node)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode.FullPath == SelectedFullPath)
                {
                    trvPath.SelectedNode = subNode;
                    return true;
                }

                if (subNode.Nodes.Count > 0)
                {
                    bool result = SetSelectedNode(subNode);

                    if (result)
                        return result;
                }
            }

            return false;
        }

        private void UpdateStatus(string dir)
        {
            lblStatus.Text = String.Format("디렉토리를 조회중입니다...\r\n({0})", dir);
            Application.DoEvents();
        }

        private void ErrorMesssage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            if (trvPath.SelectedNode == null)
                return;

            _newAppend = true;
            TreeNode newNode = trvPath.SelectedNode.Nodes.Add(NEW_NODE_NAME);
            trvPath.SelectedNode = newNode;
            newNode.BeginEdit();
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            if (trvPath.SelectedNode == null)
                return;

            TreeNode node = trvPath.SelectedNode;

            string message = String.Format("선택한 폴더('{0}')를 삭제하시겠습니까?", node.Text);

            if (MessageBox.Show(message, "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            FtpInfo.SetTop();
            FtpInfo.DeleteDirectory(node.FullPath);
            node.Remove();
        }

        private void trvPath_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!_newAppend)
                e.CancelEdit = e.Node.Level == 0;
        }

        private void trvPath_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            string path = e.Node.Parent.FullPath;
            string oldName = e.Node.Text.Trim();
            string newName = e.Label.Trim();

            System.Diagnostics.Debug.WriteLine(oldName + " : " + newName);

            if (!_newAppend) // RENAME
            {
                if (String.IsNullOrEmpty(newName))
                {
                    ErrorMesssage("디렉토리 명을 입력하세요.");
                    e.Node.BeginEdit();
                    return;
                }

                if (oldName == newName)
                    return;

                FtpInfo.SetTop();
                FtpInfo.Rename(path, oldName, newName);
            }
            else // CREATE
            {
                try
                {
                    if (String.IsNullOrEmpty(newName))
                        newName = oldName;

                    if (newName == NEW_NODE_NAME || String.IsNullOrEmpty(newName))
                    {
                        ErrorMesssage("디렉토리 명에 문제가 있습니다.");
                        e.Node.Remove();
                        return;
                    }

                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        if (node.Text == newName)
                        {
                            ErrorMesssage("이미 디렉토리가 존재합니다.");
                            node.BeginEdit();
                            return;
                        }
                    }

                    FtpInfo.SetTop();
                    FtpInfo.CreateDirectory(String.Format("{0}{1}{2}", path, trvPath.PathSeparator, newName));
                    trvPath.SelectedNode = e.Node;
                }
                finally
                {
                    _newAppend = false;
                }
            }

            e.Node.Text = newName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (trvPath.SelectedNode == null)
            {
                ErrorMesssage("폴더를 선택하세요.");
                return;
            }

            SelectedPath = RemoveRootPath(trvPath.SelectedNode.FullPath);
            DialogResult = DialogResult.OK;
        }

        private string RemoveRootPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return null;

            path = path.Replace(RootPath, String.Empty).Trim('/');

            if (!String.IsNullOrEmpty(path))
                return path;
            else
                return "/";
        }

        public new void Dispose()
        {
            if (FtpInfo != null)
            {
                FtpInfo.Dispose();
                FtpInfo = null;
            }
        }

        /// <summary>
        /// 루트 폴더를 가져오거나 설정합니다.
        /// </summary>
        public string RootPath
        {
            get;
            set;
        }

        /// <summary>
        /// 대화 상자에서 tree view 컨트롤 위에 표시되는 설명 텍스트를 가져오거나 설정합니다.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 선택한 경로를 가져오거나 설정합니다.
        /// </summary>
        public string SelectedPath
        { 
            get; 
            set;
        }

        public string SelectedFullPath
        {
            get { return ServerCommunicationFtp.Combine(RootPath, SelectedPath); }
        }

        public ServerCommunicationFtp FtpInfo
        {
            get;
            private set;
        }
    }
}

namespace System.Windows.Forms
{
    public class TreeViewEx : TreeView
    {
        public TreeViewEx()
        {
            AlwaysRaiseAfterLabelEditEvent = true;
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            if (AlwaysRaiseAfterLabelEditEvent && String.IsNullOrEmpty(e.Label))
                e = new NodeLabelEditEventArgs(e.Node, e.Node.Text);

            base.OnAfterLabelEdit(e);
        }

        /// <summary>
        /// Node 텍스트를 변경하지 않아도 AfterLabelEdit 이벤트를 발생시킬 것인지를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(true)]
        [Category("Miracom")]
        public bool AlwaysRaiseAfterLabelEditEvent
        {
            get;
            set;
        }
    }
}
