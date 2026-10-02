using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCTreeView : UserControl
    {
        #region [ MEMBER FIELD ]

        public event TreeViewEventHandler AfterCheck;
        public event TreeViewEventHandler AfterCollapse;
        public event TreeViewEventHandler AfterExpand;
        public event NodeLabelEditEventHandler AfterLabelEdit;
        public event TreeViewEventHandler AfterSelect;
        public event TreeViewCancelEventHandler BeforeCheck;
        public event TreeViewCancelEventHandler BeforeCollapse;
        public event TreeViewCancelEventHandler BeforeExpand;
        public event NodeLabelEditEventHandler BeforeLabelEdit;
        public event TreeViewCancelEventHandler BeforeSelect;
        public event TreeNodeMouseClickEventHandler NodeMouseClick;
        public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick;
        public event TreeNodeMouseHoverEventHandler NodeMouseHover;


        private DataTable m_dtTreeData = null;
        private ImageList m_ImageList = null;
        private int[] m_ImageIndexList = null;
        private int[] m_SelectedImageIndexList = null;
        private Color[] m_TextColorList = null;
        private List<string> m_sNodeColumn = null;

        private string m_sRootName = string.Empty;
        private string m_sNodeTextColorIndexColumn = string.Empty;

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lbTitle.Font = base.Font;
                txtSearch.Font = base.Font;
                treeView.Font = base.Font;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return m_dtTreeData; }
            set { m_dtTreeData = (DataTable)value; }
        }

        [Category("Option"), Browsable(true)]
        public string Title
        {
            get { return lbTitle.Text; }
            set { lbTitle.Text = value; }
        }

        [Category("Option"), Browsable(true), DefaultValue(false)]
        public bool ShowTitle
        {
            get { return lbTitle.Visible; }
            set { lbTitle.Visible = value; }
        }

        [Category("Option"), Browsable(true), DefaultValue(false)]
        public bool ShowSearch
        {
            get { return pnlSearch.Visible; }
            set { pnlSearch.Visible = value; }
        }

        public ImageList ImageList
        {
            get { return m_ImageList; }
            set
            {
                m_ImageList = value;

                if (m_ImageIndexList != null)
                    treeView.ImageList = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] ImageIndexList
        {
            get { return m_ImageIndexList; }
            set 
            {
                if (value == null || value.Length < 1)
                    treeView.ImageList = null;
                else if (treeView.ImageList == null && m_ImageList != null)
                    treeView.ImageList = m_ImageList;

                m_ImageIndexList = value;
            }
        }
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] SelectedImageIndexList
        {
            get { return m_SelectedImageIndexList; }
            set { m_SelectedImageIndexList = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color[] TextColorList
        {
            get { return m_TextColorList; }
            set { m_TextColorList = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextColorIndexColumn
        {
            get { return m_sNodeTextColorIndexColumn; }
            set { m_sNodeTextColorIndexColumn = value; }
        }

        [DefaultValue(false)]
        public bool CheckBoxes
        {
            get { return treeView.CheckBoxes; }
            set { treeView.CheckBoxes = value; }
        }

        [DefaultValue(false)]
        public bool FullRowSelect
        {
            get { return treeView.FullRowSelect; }
            set { treeView.FullRowSelect = value; }
        }

        [DefaultValue(@"\")]
        public string PathSeparator
        {
            get { return treeView.PathSeparator; }
            set { treeView.PathSeparator = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeNode SelectedNode
        {
            get { return treeView.SelectedNode; }
            set { treeView.SelectedNode = value; }
        }
        
        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return treeView.ShowLines; }
            set { treeView.ShowLines = value; }
        }

        [DefaultValue(false)]
        public bool ShowNodeToolTips
        {
            get { return treeView.ShowNodeToolTips; }
            set { treeView.ShowNodeToolTips = value; }
        }

        [DefaultValue(true)]
        public bool ShowPlusMinus
        {
            get { return treeView.ShowPlusMinus; }
            set { treeView.ShowPlusMinus = value; }
        }

        [DefaultValue(true)]
        public bool ShowRootLines
        {
            get { return treeView.ShowRootLines; }
            set { treeView.ShowRootLines = value; }
        }
        #endregion
        
        #region [ Create & Close ]
        public DUCTreeView()
        {
            InitializeComponent();
        }
        #endregion

        #region [ Event ]
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (AfterCheck != null) AfterCheck(sender, e);
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (AfterCollapse != null) AfterCollapse(sender, e);
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (AfterExpand != null) AfterExpand(sender, e);
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (AfterLabelEdit != null) AfterLabelEdit(sender, e);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (AfterSelect != null) AfterSelect(sender, e);
        }

        private void treeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeCheck != null) BeforeCheck(sender, e);   
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeCollapse != null) BeforeCollapse(sender, e);
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeExpand != null) BeforeExpand(sender, e);
        }

        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (BeforeLabelEdit != null) BeforeLabelEdit(sender, e);
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (BeforeSelect != null) BeforeSelect(sender, e);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (NodeMouseClick != null) NodeMouseClick(sender, e);
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (NodeMouseDoubleClick != null) NodeMouseDoubleClick(sender, e);
        }

        private void treeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (NodeMouseHover != null) NodeMouseHover(sender, e);
        }

        #endregion

        #region [ Control Method ]
        private void butSearch_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TreeNode n in treeView.Nodes)
                {
                    SearchRecursive(n);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    butSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.Control == true && e.KeyCode == Keys.C)
                {
                    if (treeView.SelectedNode != null)
                        Clipboard.SetText(treeView.SelectedNode.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ User Method ]
        private void SearchRecursive(TreeNode treeNode)
        {
            try
            {
                if (treeNode.Text.ToUpper().Contains(txtSearch.Text.ToUpper()) && string.IsNullOrEmpty(txtSearch.Text) == false)
                {
                    treeNode.BackColor = Color.GreenYellow;
                    treeView.SelectedNode = treeNode;
                }
                else
                {
                    treeNode.BackColor = Color.Empty;
                }
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    SearchRecursive(tn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetColorRecursive(TreeNode treeNode)
        {
            try
            {
                if (treeNode.Nodes.Count < 1)
                {
                    DataRow[] dr = m_dtTreeData.Select(string.Format("[{0}] = '{1}'", m_sNodeColumn[m_sNodeColumn.Count - 1], treeNode.Text));

                    if (dr.Length > 0)
                    {
                        int nColorIndex = int.Parse(dr[0][m_sNodeTextColorIndexColumn].ToString());
                        if (m_TextColorList.Length > nColorIndex)
                            treeNode.ForeColor = m_TextColorList[nColorIndex];
                        else
                            treeNode.ForeColor = System.Drawing.SystemColors.ControlText;
                    }
                    else
                        treeNode.ForeColor = System.Drawing.SystemColors.ControlText;
                }
                
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    SetColorRecursive(tn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetNodeColumns(params string[] sNodeColumn)
        {
            try
            {
                SetNodeColumns("", sNodeColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetNodeColumns(string sRootName, string[] sNodeColumn)
        {
            try
            {
                if (m_sNodeColumn != null) m_sNodeColumn = null;
                m_sNodeColumn = new List<string>();

                m_sRootName = sRootName;
                m_sNodeColumn.AddRange(sNodeColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawTree()
        {
            try
            {
                treeView.BeginUpdate();

                // Tree 초기화
                txtSearch.Text = string.Empty;
                treeView.Nodes.Clear();

                //  Tree Data가 없으면 리턴
                if (m_dtTreeData == null || m_dtTreeData.Rows.Count < 1)
                    return;

                // Node가 될 Column을 지정하지 않을 경우 모든 컬럼을 지정
                if (m_sNodeColumn == null || m_sNodeColumn.Count < 1)
                {
                    for (int i = 0; i < m_dtTreeData.Columns.Count; i++)
                        m_sNodeColumn.Add(m_dtTreeData.Columns[i].ColumnName);
                }

                if (m_sNodeColumn.Count < 1)
                    return;

                if (string.IsNullOrEmpty(m_sRootName) == false)
                {
                    
                    int nImgIndex = -1;
                    int nSelectedImgIndex = -1;
                    GetImageIndex(0, ref nImgIndex, ref nSelectedImgIndex);
                   
                    treeView.Nodes.Add("", m_sRootName, nImgIndex, nSelectedImgIndex);
                    AddNode(treeView.TopNode, m_dtTreeData, 0);
                }
                else
                {
                    TreeNode tempTn = new TreeNode();
                    AddNode(tempTn, m_dtTreeData, 0);

                    for (int i = 0; i < tempTn.Nodes.Count; i++)
                    {
                        treeView.Nodes.Add(tempTn.Nodes[i]);
                    }
                }

                if(string.IsNullOrEmpty(m_sNodeTextColorIndexColumn) == false && m_TextColorList != null & m_TextColorList.Length > 0)
                {
                    foreach (TreeNode n in treeView.Nodes)
                        SetColorRecursive(n);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        private TreeNode AddNode(TreeNode tn, DataTable dt, int nDepth)
        {
            try
            {
                if (m_sNodeColumn.Count <= nDepth)
                    return null;

                int nImgIdx = nDepth;
                if (nDepth == 0 && treeView.Nodes.Count > 0)
                    nImgIdx++;

                DataRow[] drNodes = dt.DefaultView.ToTable(true, m_sNodeColumn[nDepth]).Select("1=1", m_sNodeColumn[nDepth]);

                string sNodeKey = string.Empty;
                string sNodeName = string.Empty;

                int nImgIndex = -1;
                int nSelectedImgIndex = -1;
                GetImageIndex(nImgIdx, ref nImgIndex, ref nSelectedImgIndex);

                for (int i = 0; i < drNodes.Length; i++)
                {
                    sNodeKey = string.Format("[{0}] = '{1}'", m_sNodeColumn[nDepth], sNodeName);
                    sNodeName = drNodes[i][m_sNodeColumn[nDepth]].ToString();

                    if (m_sNodeColumn.Count > nDepth + 1)
                    {
                        string sFilter = string.Format("[{0}] = '{1}'", m_sNodeColumn[nDepth], sNodeName);
                        string sSort = string.Format("[{0}]", m_sNodeColumn[nDepth + 1]);
                        DataRow[] drTemp = dt.Select(sFilter, sSort);
                        DataTable dtSelectNodes = dt.Clone();
                        for (int row = 0; row < drTemp.Length; row++)
                            dtSelectNodes.Rows.Add(drTemp[row].ItemArray);

                        tn.Nodes.Add(sNodeKey, sNodeName, nImgIndex, nSelectedImgIndex);
                        AddNode(tn.Nodes[sNodeKey], dtSelectNodes, nDepth + 1);
                    }
                    else
                        tn.Nodes.Add(sNodeKey, sNodeName, nImgIndex, nSelectedImgIndex);
                }

                return tn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetImageIndex(int nDepth, ref int nImageIndex, ref int nSelectedImageIndex)
        {
            try
            {
                nImageIndex = -1;
                nSelectedImageIndex = -1;

                if (m_ImageIndexList != null && m_ImageIndexList.Length > nDepth)
                    nImageIndex = m_ImageIndexList[nDepth];

                if (m_SelectedImageIndexList != null && m_SelectedImageIndexList.Length > nDepth)
                    nSelectedImageIndex = m_SelectedImageIndexList[nDepth];
                else
                    nSelectedImageIndex = nImageIndex;

                if (m_ImageList == null || m_ImageList.Images.Count <= nImageIndex || m_ImageList.Images.Count <= nSelectedImageIndex)
                {
                    nImageIndex = -1;
                    nSelectedImageIndex = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetNodeRawData(string sNodeFullPath)
        {
            try
            {
                if (string.IsNullOrEmpty(sNodeFullPath) == true)
                    return null;

                if (string.IsNullOrEmpty(m_sRootName) == false)
                    sNodeFullPath = sNodeFullPath.Replace(m_sRootName, "");

                string[] sNodes = sNodeFullPath.Split(new string[] { treeView.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);

                if (sNodes.Length < 1)
                    return m_dtTreeData;

                if (m_dtTreeData == null || m_sNodeColumn.Count < 1)
                    return null;

                string sFilter = string.Empty;
                for (int i = 0; i < sNodes.Length; i++)
                    sFilter += string.Format("AND [{0}] = '{1}'", m_sNodeColumn[i], sNodes[i]);

                DataRow[] drSelectData = m_dtTreeData.Select(sFilter.Substring(4));

                if (drSelectData.Length < 1)
                    return null;

                DataTable dtSelectNodes = m_dtTreeData.Clone();
                for (int row = 0; row < drSelectData.Length; row++)
                    dtSelectNodes.Rows.Add(drSelectData[row].ItemArray);

                return dtSelectNodes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
