using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI
{
    public partial class StatView : UserControl
    {
        private Hashtable htStat = new Hashtable();

        public event ResultObjsRemovedHandler ResultsRemoved;
        public event ImageClickedHandler ImageClicked;
        public event StatDeletedHandler StatDeleted;
        public event StatChangedHandler StatChanged;

        public StatView()
        {
            InitializeComponent();

            lvlist.LargeImageList = imlStatType;
            lvlist.Columns.Add("Name", 50, HorizontalAlignment.Center);
            lvlist.Columns.Add("Type", 100, HorizontalAlignment.Left);
            lvlist.Columns.Add("File Path", 150, HorizontalAlignment.Left);
        }

        public void Add(StatInformation statInfo)
        {
            ListViewItem lvItem = new ListViewItem(statInfo.Title);
            lvItem.SubItems.Add(statInfo.Type.ToString());

            lvItem.Name = statInfo.Title;

            lvlist.Items.Add(lvItem);
            lvItem.ImageKey = statInfo.Type.ToString();
             
            htStat.Add(lvItem, statInfo);
            htStat.Add(statInfo, lvItem);

            webBrowser.Navigate(DACrux.ProjectManager.UI.Common.TempPath + statInfo.ResultFilePath);
            webBrowser.Document.Click += new HtmlElementEventHandler(doc_Click);
        }

        private void DrawStat(StatInformation statInfo)
        {
            webBrowser.Navigate(DACrux.ProjectManager.UI.Common.TempPath + statInfo.ResultFilePath);
        }

        void doc_Click(object sender, HtmlElementEventArgs e)
        {
            if (!e.CtrlKeyPressed)
                return;

            HtmlElement elem;
            string imagePath = string.Empty;

            try
            {
                elem = webBrowser.Document.GetElementFromPoint(e.MousePosition);

                if (elem.TagName.ToUpper() == "IMG")
                {
                    imagePath = new FileInfo((new Uri(elem.GetAttribute("src"))).LocalPath).Name;

                    if (imagePath.Trim() != string.Empty && ImageClicked != null)
                        ImageClicked(imagePath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StatInformation statInfo = (StatInformation)htStat[lvlist.SelectedItems[0]];

            DrawStat(statInfo);
        }

        private void cmsStat_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (lvlist.SelectedItems.Count < 1)
                return;

            int iGraphCnt;
            StatInformation statInfo;

            switch (e.ClickedItem.Name)
            {
                case "cmiExportStat":
                    {
                        if (lvlist.SelectedItems.Count < 1)
                            return;

                        if (lvlist.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Cannot export multiple analysis at the same time. Please select only one analysis.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }

                        statInfo = (StatInformation)htStat[lvlist.SelectedItems[0]];
                        
                        try
                        {
                            SaveFileDialog dlg = new SaveFileDialog();
                            dlg.DefaultExt = "*.html";
                            dlg.Filter = "HTML files (*.html)|*.html";
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                string strOriginFileKeyword = System.IO.Path.GetFileNameWithoutExtension(statInfo.ResultFilePath);
                                string strOriginFilePath = System.IO.Path.Combine(DACrux.ProjectManager.UI.Common.TempPath, statInfo.ResultFilePath);
                                string strDestFilePath = dlg.FileName;
                                string strDestFolder = System.IO.Path.GetDirectoryName(dlg.FileName);

                                String[] files = Directory.GetFiles(DACrux.ProjectManager.UI.Common.TempPath, string.Format("{0}*.*", strOriginFileKeyword));

                                foreach (string file in files)
                                    File.Copy(file, Path.Combine(strDestFolder, System.IO.Path.GetFileName(file)));

                                File.Move(Path.Combine(strDestFolder, statInfo.ResultFilePath), strDestFilePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;
                case "cmiRenameStat":                    
                    lvlist.SelectedItems[0].BeginEdit();                    
                    break;
                case "cmiRemoveStat":
                    {
                        switch (MessageBox.Show("Do you want to delete all graphs in this Analysis?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.Yes:
                                {
                                    for (int i = lvlist.SelectedItems.Count - 1; i >= 0; i--)
                                    {
                                        #region Remove Graph

                                        statInfo = (StatInformation)htStat[lvlist.SelectedItems[i]];

                                        if (statInfo.GraphInformations == null)
                                            iGraphCnt = 0;
                                        else
                                            iGraphCnt = statInfo.GraphInformations.Length;

                                        if (iGraphCnt > 0)
                                        {
                                            List<string> arrImagePath = new List<string>(iGraphCnt);
                                            foreach (GraphInformation oGraph in statInfo.GraphInformations)
                                            {
                                                if (oGraph.ImagePath.Trim() != string.Empty && ResultsRemoved != null)
                                                    arrImagePath.Add(oGraph.ImagePath);
                                            }
                                            ResultsRemoved(arrImagePath.ToArray());

                                        }

                                        #endregion

                                        if (StatDeleted != null)
                                            StatDeleted(statInfo);

                                        htStat[lvlist.SelectedItems[i]] = null;

                                        htStat.Remove(lvlist.SelectedItems[i]);
                                        lvlist.Items.Remove(lvlist.SelectedItems[i]);

                                    }
                                    webBrowser.Navigate("");
                                }
                                break;
                            case DialogResult.No:
                                {
                                    for (int i = lvlist.SelectedItems.Count - 1; i >= 0; i--)
                                    {
                                        statInfo = (StatInformation)htStat[lvlist.SelectedItems[i]];

                                        if (StatDeleted != null)
                                            StatDeleted(statInfo);

                                        htStat[lvlist.SelectedItems[i]] = null;

                                        htStat.Remove(lvlist.SelectedItems[i]);
                                        lvlist.Items.Remove(lvlist.SelectedItems[i]);

                                    }
                                    webBrowser.Navigate("");
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        public bool RemoveAnalysis(string[] imagePath)
        {
            if (lvlist == null || lvlist.Items.Count < 1 || imagePath.Length < 1)
                return false;



            StatInformation statInfo;
            for (int i = lvlist.Items.Count-1; i >=0; i--)
            {
                statInfo = (StatInformation)htStat[lvlist.Items[i]];
                if (statInfo.GraphInformations != null)
                {
                    bool bDelete = false;
                    for (int j = 0; j < statInfo.GraphInformations.Length; j++)
                    {
                        for (int k = 0; k < imagePath.Length; k++)
                        {
                            if (statInfo.GraphInformations[j].ImagePath == imagePath[k])
                            {
                                if (StatDeleted != null)
                                    StatDeleted(statInfo);

                                htStat[lvlist.Items[i]] = null;

                                htStat.Remove(lvlist.Items[i]);
                                lvlist.Items.Remove(lvlist.Items[i]);
                                bDelete = true;
                                break;
                            }
                        }
                        if (bDelete)
                            break;
                    }
                }
            }
            webBrowser.Navigate("");
            return true;
        }

        private void lvlist_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (lvlist.Items[e.Item].SubItems[0].Text == e.Label)
                return;

            try
            {
                foreach (ListViewItem lvItem in lvlist.Items)
                {
                    if (lvItem.SubItems[0].Text == e.Label)
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("Same Analysis name exists.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (StatChanged != null)
                    StatChanged((StatInformation)htStat[lvlist.SelectedItems[0]]);

                ((StatInformation)htStat[lvlist.Items[e.Item]]).Title = e.Label;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ActivateStat(string imagePath)
        {
            if (lvlist == null || lvlist.Items.Count < 1)
                return false;

            StatInformation statInfo;
            for (int i = 0; i < lvlist.Items.Count; i++)
            {
                statInfo = (StatInformation)htStat[lvlist.Items[i]];
                if (statInfo.GraphInformations != null)
                {

                    for (int j = 0; j < statInfo.GraphInformations.Length; j++)
                    {
                        if (statInfo.GraphInformations[j].ImagePath == imagePath)
                        {
                            DrawStat(statInfo);
                            return true;
                        }
                    }
                }
            }          

            return false;
        }
    }
}
