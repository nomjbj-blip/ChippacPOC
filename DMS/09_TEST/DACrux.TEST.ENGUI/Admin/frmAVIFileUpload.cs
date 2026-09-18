using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Common.RO;
using System.IO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmAVIFileUpload : DACrux.Framework.Base.DACruxUXBasic01
    {
        List<string> lsMapFiles = new List<string>();
        List<string> lsImageFiles = new List<string>();

        public frmAVIFileUpload()
        {
            InitializeComponent();
        }

        #region [ Method ]


        private void GetDriveFolderList()
        {
            TreeNode rootNode = null;

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            trFolder.Nodes.Clear();

            rootNode = new TreeNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            rootNode.ImageIndex = 4; //Drive
            rootNode.SelectedImageIndex = 4;
            trFolder.Nodes.Add(rootNode);
            SetDirectoryNode(rootNode);


            foreach (DriveInfo dname in allDrives)
            {
                if (dname.DriveType == DriveType.Fixed)
                {
                    rootNode = new TreeNode(dname.Name);
                    rootNode.ImageIndex = 0; //Drive
                    rootNode.SelectedImageIndex = 0;
                    trFolder.Nodes.Add(rootNode);
                    SetDirectoryNode(rootNode);
                }
            }

            foreach (DriveInfo dname in allDrives)
            {
                if (dname.DriveType == DriveType.Network)
                {
                    rootNode = new TreeNode(dname.Name);
                    rootNode.ImageIndex = 1; //Network
                    rootNode.SelectedImageIndex = 1;
                    trFolder.Nodes.Add(rootNode);
                    SetDirectoryNode(rootNode);
                }
            }
        }

        private void SetDirectoryNode(TreeNode dirNode)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirNode.FullPath);
                foreach (DirectoryInfo dirItem in dir.GetDirectories())
                {
                    TreeNode newNode = new TreeNode(dirItem.Name);
                    newNode.ImageIndex = 2;
                    newNode.SelectedImageIndex = 3;
                    dirNode.Nodes.Add(newNode);
                    newNode.Nodes.Add("*");
                }
            }
            finally
            {
            }
        }

        private void SetListView(string sFullPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sFullPath);
                int DirectCount = 0;
                //하위 Directory 확인
                foreach (DirectoryInfo dirItem in dir.GetDirectories())
                {
                    ListViewItem lsvitem = new ListViewItem();

                    lsvitem.ImageIndex = 2;
                    lsvitem.Text = dirItem.Name;
                    DirectCount++;
                }
            }
            finally
            {
            }
        }
        #endregion [ Method ]   

        #region [ Event Handler ]

        private void BtnFolderOpen_Click(object sender, EventArgs e)
        {
            if (chkFileGet.Checked == true)
            {
                using (OpenFileDialog dlgOpen = new OpenFileDialog())
                {
                    dlgOpen.Title = "Select AVI File";
                    dlgOpen.Multiselect = true; // 파일 다중 선택
                    if (dlgOpen.ShowDialog() == DialogResult.OK)
                    {
                        lsMapFiles = new List<string>();
                        lsImageFiles = new List<string>();

                        lsFiles.BeginUpdate();

                        for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                        {
                            System.IO.FileInfo oFile = new System.IO.FileInfo(dlgOpen.FileNames[i]);
                            ListViewItem lvi = new ListViewItem(oFile.Name);
                            lvi.SubItems.Add(oFile.Extension);
                            lvi.SubItems.Add(oFile.Length.ToString());
                            lvi.SubItems.Add(oFile.LastWriteTime.ToString());
                            lvi.SubItems.Add(oFile.FullName);

                            if (oFile.Extension.ToUpper() == ".JPG" || oFile.Extension.ToUpper() == ".JPEG" || oFile.Extension.ToUpper() == ".BMP")
                            {
                                lvi.ImageIndex = 1;
                                lsImageFiles.Add(oFile.FullName);

                            }
                            else
                            {
                                lvi.ImageIndex = 0;
                                lsMapFiles.Add(oFile.FullName);
                            }

                            // ListViewItem객체를 Items 속성에 추가
                            lsFiles.Items.Add(lvi);
                        }

                        // 리스뷰를 Refresh하여 보여줌
                        lsFiles.EndUpdate();
                    }
                }
            }
            else
            {
                using (FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog())
                {
                    lsMapFiles = new List<string>();
                    lsImageFiles = new List<string>();
                    oFolderBrowserDialog.Description = "AVI 폴더를 선택해주세요.";
                    oFolderBrowserDialog.ShowNewFolderButton = true;

                    if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        lsFiles.BeginUpdate();
                        DirFileSearch(oFolderBrowserDialog.SelectedPath);
                        lsFiles.EndUpdate();
                    }
                }
            }

        }

        private void DirFileSearch(string AVIDir)         
        {
            try
            {
                MainForm.SetStatusMessage("File을 가져 오는 중입니다..");

                foreach (string oFileName in Directory.GetFiles(AVIDir
                    , string.IsNullOrEmpty(TxtFolderPath.Text.Trim()) == true ? "*.*" : string.Format("*{0}*", TxtFolderPath.Text.Trim())
                    , chkAllLevelDir.Checked == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(s => TxtExtension.Text.Contains(Path.GetExtension(s).ToLower())))
                {
                    System.IO.FileInfo oFile = new System.IO.FileInfo(oFileName);
                    ListViewItem lvi = new ListViewItem(oFile.Name);
                    lvi.SubItems.Add(oFile.Extension);
                    lvi.SubItems.Add(oFile.Length.ToString());
                    lvi.SubItems.Add(oFile.LastWriteTime.ToString());
                    lvi.SubItems.Add(oFile.FullName);

                    if (oFile.Extension.ToUpper() == ".JPG" || oFile.Extension.ToUpper() == ".JPEG" || oFile.Extension.ToUpper() == ".BMP")
                    {
                        lvi.ImageIndex = 1;
                        lsImageFiles.Add(oFile.FullName);

                    }
                    else
                    {
                        lvi.ImageIndex = 0;
                        lsMapFiles.Add(oFile.FullName);
                    }

                    // ListViewItem객체를 Items 속성에 추가
                    lsFiles.Items.Add(lvi);
                }           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }

        } 

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lsFiles.Items.Clear();
            lsMapFiles = new List<string>();
            lsImageFiles = new List<string>();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int iSuccess = 0;
            System.IO.FileInfo oFileInfo = null;
            try
            {
                if (lsFiles == null || lsFiles.Items.Count <= 0)
                    return;

                if (MessageBox.Show(string.Format("파일 {0} 개를 Upload 하시겠습니까?", lsFiles.Items.Count), "Upload", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                ComConfiguration obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIMapFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return;

                 DACrux.Utility.HFtpClient oFTP = new Utility.HFtpClient(dtConfig.Rows[0]["IP"].ToString(), 
                     DACrux.Base.Convert.intParse(dtConfig.Rows[0]["PORT"].ToString()),
                     dtConfig.Rows[0]["ID"].ToString(),
                     dtConfig.Rows[0]["PASS"].ToString());

                 bool IsFtpConnect = oFTP.LoginTest();
                 if (IsFtpConnect == false)
                     throw new Exception("FTP에 접속할 수 없습니다.");

                 //string strFTPPath = "/EQUIP/FOI/AVI/TEST";
                 string strFTPPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();

                //Image File 먼저 Upload 후 Map File 을 Upload 한다.
                 foreach (string strFile in lsImageFiles)
                 {
                     System.Threading.Thread.Sleep(5);
                     oFileInfo = new FileInfo(strFile);
                     oFileInfo.IsReadOnly = false;
                     oFTP.Upload(oFileInfo.FullName, string.Format("{0}/{1}", strFTPPath, oFileInfo.Name));
                     iSuccess++;
                     MainForm.SetStatusMessage(string.Format("File을 Upload 하는 중입니다. ({0}/{1})", iSuccess, lsImageFiles.Count + lsMapFiles.Count));
                     Application.DoEvents();
                 }

                 foreach (string strFile in lsMapFiles)
                 {
                     System.Threading.Thread.Sleep(5);
                     oFileInfo = new FileInfo(strFile);
                     oFileInfo.IsReadOnly = false;
                     oFTP.Upload(oFileInfo.FullName, string.Format("{0}/{1}", strFTPPath, oFileInfo.Name));
                     iSuccess++;
                     MainForm.SetStatusMessage(string.Format("File을 Upload 하는 중입니다. ({0}/{1})", iSuccess, lsImageFiles.Count + lsMapFiles.Count));
                     Application.DoEvents();
                 }

                //oFTP.SetCurrentDirectory(dtConfig.Rows[0]["MAIN_PATH"].ToString());
                //using (DACrux.Utility.ServerCommunicationFtp oFTP = new Utility.ServerCommunicationFtp())
                //{
                //    oFTP.Server = dtConfig.Rows[0]["IP"].ToString();
                //    oFTP.Port = DACrux.Base.Convert.intParse(dtConfig.Rows[0]["PORT"].ToString());
                //    oFTP.UserID = dtConfig.Rows[0]["ID"].ToString();
                //    oFTP.Password = dtConfig.Rows[0]["PASS"].ToString();
                //    oFTP.ChmodValue = 777;

                //    oFTP.SetCurrentDirectory("/EQUIP/FOI/AVI/TEST");
                //    //oFTP.SetCurrentDirectory(dtConfig.Rows[0]["MAIN_PATH"].ToString());

                //    //Image File 먼저 Upload 후 Map File 을 Upload 한다.
                //    foreach (string strFile in lsImageFiles)
                //    {
                //        System.Threading.Thread.Sleep(20);
                //        oFileInfo = new FileInfo(strFile);
                //        oFileInfo.IsReadOnly = false;
                //        oFTP.SendFile(oFileInfo.FullName, oFileInfo.Name);
                //        iSuccess++;
                //        MainForm.SetStatusMessage(string.Format("File을 Upload 하는 중입니다. ({0}/{1})", iSuccess, lsImageFiles.Count + lsMapFiles.Count));
                //        Application.DoEvents();
                //    }

                //    foreach (string strFile in lsMapFiles)
                //    {
                //        System.Threading.Thread.Sleep(20);
                //        oFileInfo = new FileInfo(strFile);
                //        oFileInfo.IsReadOnly = false;
                //        oFTP.SendFile(oFileInfo.FullName, oFileInfo.Name);
                //        iSuccess++;
                //        MainForm.SetStatusMessage(string.Format("File을 Upload 하는 중입니다. ({0}/{1})", iSuccess, lsImageFiles.Count + lsMapFiles.Count));
                //        Application.DoEvents();
                //    }
                //}

                MessageBox.Show("Upload 를 완료 하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lsFiles.Items.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
                lsMapFiles = new List<string>();
                lsImageFiles = new List<string>();
            }
        }

        private void frmAVIFileUpload_Load(object sender, EventArgs e)
        {
            lsFiles.BeginUpdate();
            lsFiles.Clear();
            lsFiles.View = View.Details;
            lsFiles.SmallImageList = FileImageList;
            lsFiles.Columns.Add("파일명", 500, HorizontalAlignment.Left);
            lsFiles.Columns.Add("확장자", 50, HorizontalAlignment.Left);
            lsFiles.Columns.Add("사이즈", 70, HorizontalAlignment.Left);
            lsFiles.Columns.Add("날짜", 150, HorizontalAlignment.Left);
            lsFiles.Columns.Add("Full Directory", 700, HorizontalAlignment.Left);
            lsFiles.EndUpdate();

            //File 을 선택 하는 형태가 Fab 마다 틀리다.
            if (DACrux.Base.GlobalVariable.Factory == "FAB1")
                chkFileGet.Checked = true;
            else
                chkFileGet.Checked = false;

            GetDriveFolderList();
        }

        private void lsFiles_KeyUp(object sender, KeyEventArgs e)
        {
            //Delete Key 선택 시 선택한 File 을 삭제 시킨다.
            if (e.KeyCode == Keys.Delete)
            {
                if (lsFiles.SelectedItems.Count <= 0)
                    return;

                if (lsFiles.SelectedItems.Count <= 0)
                    return;

                lsFiles.BeginUpdate();

                foreach (ListViewItem item in lsFiles.SelectedItems)
                {
                    FileInfo oFile = new FileInfo(item.SubItems[4].Text);
                    int iIndex = -1;
                    iIndex = lsImageFiles.IndexOf(oFile.FullName);

                    if (iIndex > -1)
                        lsImageFiles.Remove(oFile.FullName);

                    iIndex = lsMapFiles.IndexOf(oFile.FullName);

                    if (iIndex > -1)
                        lsMapFiles.Remove(oFile.FullName);

                    item.Remove();
                }

                lsFiles.EndUpdate();
            }
        }

        private void BtnFindFolder_Click(object sender, EventArgs e)
        {
            if (trFolder.SelectedNode == null)
                return;

            if (trFolder.SelectedNode.Level == 0)
            {
                MessageBox.Show("최상위 Dirive 에서는 검색 할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DirectoryInfo oDir = new DirectoryInfo(trFolder.SelectedNode.FullPath);
            if (oDir.Exists)
            {
                lsFiles.BeginUpdate();
                DirFileSearch(trFolder.SelectedNode.FullPath);
                lsFiles.EndUpdate();
            }
            else
            {
                MessageBox.Show(string.Format("현재 경로를 찾을 수 없습니다.", BtnFindFolder.Text), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void getFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trFolder.SelectedNode.Level == 0)
            {
                MessageBox.Show("최상위 Dirive 에서는 검색 할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DirectoryInfo oDir = new DirectoryInfo(trFolder.SelectedNode.FullPath);
            if (oDir.Exists)
            {
                lsFiles.BeginUpdate();
                DirFileSearch(trFolder.SelectedNode.FullPath);
                lsFiles.EndUpdate();
            }
            else
            {
                MessageBox.Show(string.Format("현재 경로를 찾을 수 없습니다.", BtnFindFolder.Text), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDriveFolderList();
        }

        private void trFolder_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "*")
            {
                e.Node.ImageIndex = 2;
                e.Node.SelectedImageIndex = 2;
            }
        }

        private void trFolder_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "*")
            {
                e.Node.Nodes.Clear();
                e.Node.ImageIndex = 3;
                e.Node.SelectedImageIndex = 3;
                SetDirectoryNode(e.Node);
            }
        }

        private void trFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetListView(e.Node.FullPath);
        }


        private void TxtFolderPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (trFolder.SelectedNode == null || (Keys)e.KeyChar != Keys.Enter)
                return;

            if (trFolder.SelectedNode.Level == 0)
            {
                MessageBox.Show("최상위 Dirive 에서는 검색 할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DirectoryInfo oDir = new DirectoryInfo(trFolder.SelectedNode.FullPath);
            if (oDir.Exists)
            {
                lsFiles.BeginUpdate();
                DirFileSearch(trFolder.SelectedNode.FullPath);
                lsFiles.EndUpdate();
            }
            else
            {
                MessageBox.Show(string.Format("현재 경로를 찾을 수 없습니다.", BtnFindFolder.Text), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }


        private void lsFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewSorter Sorter = new ListViewSorter();
            lsFiles.ListViewItemSorter = Sorter;
            if (!(lsFiles.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)lsFiles.ListViewItemSorter;

            if (Sorter.LastSort == e.Column)
            {
                if (lsFiles.Sorting == SortOrder.Ascending)
                    lsFiles.Sorting = SortOrder.Descending;
                else
                    lsFiles.Sorting = SortOrder.Ascending;
            }
            else
            {
                lsFiles.Sorting = SortOrder.Descending;
            }
            Sorter.ByColumn = e.Column;

            lsFiles.Sort();
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lsFiles.Items.Clear();
            lsMapFiles = new List<string>();
            lsImageFiles = new List<string>();
        }

        #endregion [ Event Handler ]


       
    }
}

/// <summary>
/// List View Columns Sorting Class
/// </summary>
public class ListViewSorter : System.Collections.IComparer
{
    public int Compare(object o1, object o2)
    {
        if (!(o1 is ListViewItem))
            return (0);
        if (!(o2 is ListViewItem))
            return (0);

        ListViewItem lvi1 = (ListViewItem)o2;
        string str1 = lvi1.SubItems[ByColumn].Text;
        ListViewItem lvi2 = (ListViewItem)o1;
        string str2 = lvi2.SubItems[ByColumn].Text;

        int result;
        if (lvi1.ListView.Sorting == SortOrder.Ascending)
            result = String.Compare(str1, str2);
        else
            result = String.Compare(str2, str1);

        LastSort = ByColumn;

        return (result);
    }


    public int ByColumn
    {
        get { return Column; }
        set { Column = value; }
    }
    int Column = 0;

    public int LastSort
    {
        get { return LastColumn; }
        set { LastColumn = value; }
    }
    int LastColumn = 0;
}   


