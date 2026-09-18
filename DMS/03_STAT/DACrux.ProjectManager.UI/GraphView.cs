using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using DACrux.ProjectManager.UI;

namespace DACrux.ProjectManager.UI
{
    public partial class GraphView : UserControl
    {
        
        private System.Collections.Hashtable htGraphInfo = new System.Collections.Hashtable();

        public event ResultObjsRemovedHandler ResultsRemoved;
        public event GraphUpdatingHandler GraphUpdating;
        public event GraphDeletedHandler GraphDeleted;
        public event ImageClickedHandler ImageClicked;

        public GraphView()
        {
            InitializeComponent();

            lvlist.LargeImageList = imlGraphType;

            lvlist.Columns.Add("Name", 50, HorizontalAlignment.Center);
            lvlist.Columns.Add("Type", 100, HorizontalAlignment.Left);
            lvlist.Columns.Add("Created By", 150, HorizontalAlignment.Left);

            
            graphPanel.GraphUpdating += new GraphUpdatingHandler(graphPanel_GraphUpdating);
            graphPanel.ImageClicked += new ImageClickedHandler(graphPanel_ImageClicked);
        }

        void graphPanel_ImageClicked(string imagePath)
        {
            ImageClicked(imagePath);
        }


        private void graphPanel_GraphUpdating(GraphInformation graphInfo)
        {
            if(GraphUpdating != null)
                GraphUpdating(graphInfo);
        }

        public void Add(GraphInformation graphInfo)
        {
            ListViewItem lvItem = new ListViewItem(graphInfo.Name);
            lvItem.SubItems.Add(graphInfo.Type.ToString() + " Graph");
            lvItem.SubItems.Add(graphInfo.CreatedInformation);

            lvItem.Name = graphInfo.Name;
            lvItem.ImageKey = graphInfo.Type.ToString();

            lvlist.Items.Add(lvItem);

            graphInfo.IsDrawn = false;
            graphPanel.DrawGraph(graphInfo);

            htGraphInfo.Add(lvItem, graphInfo);
        }

        private void lvlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvlist.SelectedItems.Count > 0)
                //리스트뷰의 SelectListViewItemCollection[0] 값을 가져와서 그래픽정보를 뿌려준다.
                graphPanel.DrawGraph(htGraphInfo[lvlist.SelectedItems[0]] as GraphInformation);
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
                        MessageBox.Show("Same graph name exists.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (e.Label == null || e.Label == "")
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Graph name does not change", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);              
                    return;
                }
                        
                ((GraphInformation)htGraphInfo[lvlist.Items[e.Item]]).Name = e.Label;

                if (e.Label != null)
                {
                    graphPanel.DrawGraph(htGraphInfo[lvlist.SelectedItems[0]] as GraphInformation);
                }
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmsGraph_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(lvlist.SelectedItems.Count < 1)
                return;

            switch(e.ClickedItem.Name)
            {
                case "cmiRenameGraph":
                    lvlist.SelectedItems[0].BeginEdit();
                    break;
                case "cmiRemoveGraph":
                    {
                        switch (MessageBox.Show("Do you want to delete the Analysis contains this graph?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.Yes:
                                {
                                    for (int i = lvlist.SelectedItems.Count - 1; i >= 0; i--)
                                    {
                                        #region Remove Analysis                                        
                                        if (ResultsRemoved != null)
                                            ResultsRemoved(new string[] { ((GraphInformation)htGraphInfo[lvlist.SelectedItems[i]]).ImagePath });
                                        #endregion

                                        if (GraphDeleted != null)
                                            GraphDeleted((GraphInformation)htGraphInfo[lvlist.SelectedItems[i]]);

                                        htGraphInfo[lvlist.SelectedItems[i]] = null;

                                        htGraphInfo.Remove(lvlist.SelectedItems[i]);
                                        lvlist.Items.Remove(lvlist.SelectedItems[i]);
                                    }
                                    graphPanel.GraphInfo.Type = GraphType.None;
                                    graphPanel.GraphInfo.DataSource = null;

                                    graphPanel.InitPanel();
                                }
                                break;
                            case DialogResult.No:
                                {
                                    for (int i = lvlist.SelectedItems.Count - 1; i >= 0; i--)
                                    {
                                        if (GraphDeleted != null)
                                            GraphDeleted((GraphInformation)htGraphInfo[lvlist.SelectedItems[i]]);

                                        htGraphInfo[lvlist.SelectedItems[i]] = null;

                                        htGraphInfo.Remove(lvlist.SelectedItems[i]);
                                        lvlist.Items.Remove(lvlist.SelectedItems[i]);
                                    }
                                    graphPanel.GraphInfo.Type = GraphType.None;
                                    graphPanel.GraphInfo.DataSource = null;

                                    graphPanel.InitPanel();
                                }
                                break;
                        }
                        
                    }
                    break;
            }
        }

        public bool RemoveGraphs(string[] imagePath)
        {
            if (lvlist == null || lvlist.Items.Count < 1 || imagePath.Length < 1)
                return false;
            GraphInformation graphInfo;
            for (int i = lvlist.Items.Count-1; i >=0 ; i--)
            {
                graphInfo = (GraphInformation)htGraphInfo[lvlist.Items[i]];

                for (int j = 0; j < imagePath.Length; j++)
                {
                    if (graphInfo.ImagePath == imagePath[j])
                    {
                        if (GraphDeleted != null)
                            GraphDeleted(graphInfo);

                        htGraphInfo[lvlist.Items[i]] = null;

                        htGraphInfo.Remove(lvlist.Items[i]);
                        lvlist.Items.Remove(lvlist.Items[i]);

                        
                        break;
                    }
                }
                
            }
            graphPanel.GraphInfo.Type = GraphType.None;
            graphPanel.GraphInfo.DataSource = null;

            graphPanel.InitPanel();
            return true;
        }


        public bool ActivateGraph(string imagePath)
        {
            if (lvlist == null || lvlist.Items.Count < 1)
                return false;

            GraphInformation graphInfo;
            for(int i=0; i<lvlist.Items.Count; i++)
            {
                graphInfo = (GraphInformation)htGraphInfo[lvlist.Items[i]];

                if (graphInfo.ImagePath == imagePath)
                {
                    graphPanel.DrawGraph(graphInfo);
                    return true;
                }
            }

            return false;
        }

        private void GraphView_Load(object sender, EventArgs e)
        {

        }
    }
}
