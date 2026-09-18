using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Resources;
using DACrux.SP.Common;
using DACrux.SP.Controls;

namespace SmartParser.Designer
{
    public partial class frmActionManagement : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region " Member Field & Property "

        private Analysis analysis = null;

        #endregion

        #region " Creator "

        public frmActionManagement()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            analysis = Analysis.GetInstance();

            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(EntitiesItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(EntitiesItemRemoved);

            analysis.Sections.ItemAdded += new ItemAddedEventHandler<Section>(SectionsItemAdded);
            analysis.Sections.ItemRemoved += new ItemRemovedEventHandler<Section>(SectionsItemRemoved);

            InitListView();
            InitComboBox();
        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region [ ListView ]

                foreach (ITask task in analysis.Tasks)
                {
                    lvTask.Items.Add(GetTaskListViewItem(task));
                }

                foreach (Script script in analysis.Scripts)
                {
                    lvScript.Items.Add(GetScriptListViewItem(script));
                }

                #endregion

                #region [ ComboBox ]

                foreach (string name in Enum.GetNames(typeof(TaskTypeItem)))
                    cboTaskType.Items.Add(name);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SectionsItemAdded(Section section)
        {
            try
            {
                section.SectionRenamed += new ItemRenamedEventHandler<Section>(SectionRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void SectionsItemRemoved(Section section)
        {
            try
            {
                section.SectionRenamed -= new ItemRenamedEventHandler<Section>(SectionRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void SectionRenamed(Section item, string prevName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemAdded(RegexEntity entity)
        {
            try
            {
                entity.EntityRenamed += new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemRemoved(RegexEntity entity)
        {
            try
            {
                entity.EntityRenamed -= new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntityRenamed(RegexEntity item, string prevName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #region [ Task ]

        void lvTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvTask.SelectedItems.Count < 1)
                    return;

                cboTaskType.Text = lvTask.SelectedItems[0].SubItems[1].Text.Replace(" ", "_");
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cboTaskType.Text))
                    return;

                TaskTypeItem item = (TaskTypeItem)Enum.Parse(typeof(TaskTypeItem), cboTaskType.Text);

                ITaskDialog dlg = null;

                switch (item)
                {
                    case TaskTypeItem.COM_PLUS:
                        dlg = new dlgTaskComPlus();
                        break;
                    case TaskTypeItem.DATABASE:
                        dlg = new dlgTaskDB();
                        break;
                    case TaskTypeItem.ENTITY_TRAVERSE:
                        dlg = new dlgTaskEntityTraverse();
                        break;
                    case TaskTypeItem.ENTITY_VALUE:
                        dlg = new dlgTaskEntityValue();
                        break;
                }

                if (dlg != null && dlg.ShowDialog() == DialogResult.OK)
                {
                    analysis.Tasks.Add(dlg.Task);
                    lvTask.Items.Add(GetTaskListViewItem(dlg.Task));
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvTask.SelectedItems.Count < 1)
                    return;

                TaskTypeItem item = (TaskTypeItem)Enum.Parse(typeof(TaskTypeItem), cboTaskType.Text);

                ITaskDialog dlg = null;

                switch (item)
                {
                    case TaskTypeItem.COM_PLUS:
                        dlg = new dlgTaskComPlus(lvTask.SelectedItems[0].Tag as COMPlusTask);
                        break;
                    case TaskTypeItem.DATABASE:
                        dlg = new dlgTaskDB(lvTask.SelectedItems[0].Tag as DBTask);
                        break;
                    case TaskTypeItem.ENTITY_TRAVERSE:
                        dlg = new dlgTaskEntityTraverse(lvTask.SelectedItems[0].Tag as EntityTraverseTask);
                        break;
                    case TaskTypeItem.ENTITY_VALUE:
                        dlg = new dlgTaskEntityValue(lvTask.SelectedItems[0].Tag as EntityValueTask);
                        break;
                }

                if (dlg != null && dlg.ShowDialog() == DialogResult.OK)
                {
                    lvTask.SelectedItems[0].Name = lvTask.SelectedItems[0].Text = dlg.Task.Name;
                    lvTask.SelectedItems[0].SubItems[1].Text = item.ToString().Replace("_", " ");
                    lvTask.SelectedItems[0].SubItems[2].Text = dlg.Task.ResultUsage.ToString();
                    lvTask.SelectedItems[0].ImageKey = dlg.Task.Type.ToString();
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvTask.SelectedItems.Count < 1)
                    return;

                if (DACrux.SP.Common.Utility.ShowMessageBox("Do you want to delete the task?", MessageBoxIcon.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ITask task = lvTask.SelectedItems[0].Tag as ITask;
                    analysis.Tasks.Remove(task);

                    var targetTaskFiltered = from Script script in analysis.Scripts
                                             where script.TargetTask == task
                                             select script;

                    targetTaskFiltered.ToList().ForEach(sc => sc.TargetTask = null);

                    var argsFilterer = from Script script in analysis.Scripts
                                       where Array.Exists(script.Args, data => data is ITask && (data as ITask) == task)
                                       select script;

                    argsFilterer.ToList().ForEach(sc => sc.Args = Array.FindAll(sc.Args, data => !(data is ITask) || (data as ITask) != task).ToArray());

                    lvTask.SelectedItems[0].Remove();
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region [ Script ]

        private void btnAddScript_Click(object sender, EventArgs e)
        {
            try
            {
                dlgScript dlg = new dlgScript();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    analysis.Scripts.Add(dlg.Script);
                    lvScript.Items.Add(GetScriptListViewItem(dlg.Script));
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnEditScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvScript.SelectedItems.Count < 1)
                    return;

                dlgScript dlg = new dlgScript(lvScript.SelectedItems[0].Tag as Script);

                if (dlg != null && dlg.ShowDialog() == DialogResult.OK)
                {
                    lvScript.SelectedItems[0].Name = lvScript.SelectedItems[0].Text = dlg.Script.CommandType.ToString();
                    lvScript.SelectedItems[0].SubItems[1].Text = dlg.Script.RunningMode.ToString();
                    lvScript.SelectedItems[0].SubItems[2].Text = dlg.Script.TargetTask.Name;
                    lvScript.SelectedItems[0].SubItems[3].Text = string.Join(", ", (from Data data in dlg.Script.Args select (data is ITask) ? (data as ITask).Name : data.Value).ToArray());
                    lvScript.SelectedItems[0].ImageKey = dlg.Script.CommandType.ToString();
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvScript.SelectedItems.Count < 1)
                    return;

                if (DACrux.SP.Common.Utility.ShowMessageBox("Do you want to delete the script?", MessageBoxIcon.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Script script = lvScript.SelectedItems[0].Tag as Script;
                    analysis.Scripts.Remove(script);
                    lvScript.SelectedItems[0].Remove();
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvScript.Items.Count < 2)
                    return;

                if (lvScript.SelectedItems.Count < 1)
                    return;

                ListViewItem item = lvScript.SelectedItems[0];
                int index = item.Index;

                if (index == 0)
                    return;

                analysis.Scripts.Remove(item.Tag as Script);
                item.Remove();

                analysis.Scripts.Insert(index - 1, (item.Tag as Script));
                lvScript.Items.Insert(index - 1, item);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Information);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvScript.Items.Count < 2)
                    return;

                if (lvScript.SelectedItems.Count < 1)
                    return;

                ListViewItem item = lvScript.SelectedItems[0];
                int index = item.Index;

                if (index == lvScript.Items.Count - 1)
                    return;

                analysis.Scripts.Remove(item.Tag as Script);
                item.Remove();

                analysis.Scripts.Insert(index + 1, (item.Tag as Script));
                lvScript.Items.Insert(index + 1, item);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Information);
            }
        }

        #endregion

        #endregion

        #region " Method "

        private void InitListView()
        {
            ResourceManager rm = new ResourceManager("SmartParser.Controls.res", System.Reflection.Assembly.Load("SmartParser.Controls"));

            ImageList imgListTask = new ImageList();
            ImageList imgListScript = new ImageList();
            imgListTask.ImageSize = new Size(15, 15);
            imgListScript.ImageSize = new Size(15, 15);

            foreach (string name in Enum.GetNames(typeof(TaskTypeItem)))
                imgListTask.Images.Add(name, (Image)rm.GetObject(name));

            foreach (string name in Enum.GetNames(typeof(ScriptCommandTypeItem)))
                imgListScript.Images.Add(name, (Image)rm.GetObject(name));

            lvTask.SmallImageList = imgListTask;
            lvScript.SmallImageList = imgListScript;

            lvTask.MultiSelect = false;
            lvScript.MultiSelect = false;

            lvTask.Columns.Add("Name", 150, HorizontalAlignment.Left);
            lvTask.Columns.Add("Type", 100, HorizontalAlignment.Left);
            lvTask.Columns.Add("Result Usage"
                , (lvTask.Width < 350) ? 100 : (lvTask.Width - 250 - 20)
                , HorizontalAlignment.Left);

            lvScript.Columns.Add("Command", 100, HorizontalAlignment.Left);
            lvScript.Columns.Add("Mode", 100, HorizontalAlignment.Left);
            lvScript.Columns.Add("Target", 100, HorizontalAlignment.Left);
            lvScript.Columns.Add("Arguments"
                , (lvScript.Width < 400) ? 100 : (lvScript.Width - 300 - 20)
                , HorizontalAlignment.Left);

            lvTask.SelectedIndexChanged += new EventHandler(lvTask_SelectedIndexChanged);

            lvTask.MouseDoubleClick += btnEdit_Click;
            lvScript.MouseDoubleClick += btnEditScript_Click;
        }

        private void InitComboBox()
        {
            cboTaskType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private ListViewItem GetTaskListViewItem(ITask task)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = task.Name;
            item.SubItems.Add(task.Type.ToString().Replace("_", " "));
            item.SubItems.Add(task.ResultUsage.ToString());

            item.ImageKey = task.Type.ToString();
            item.Tag = task;

            return item;
        }

        private ListViewItem GetScriptListViewItem(Script script)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = script.CommandType.ToString().Replace("_", " ");
            item.SubItems.Add(script.RunningMode.ToString());
            item.SubItems.Add(script.TargetTask.Name);
            item.SubItems.Add(string.Join(", ", (from Data data in script.Args
                                                 select (data is ITask) ? (data as ITask).Name : data.Value).ToArray()));

            item.ImageKey = script.CommandType.ToString();
            item.Tag = script;

            return item;
        }

        #endregion

    }
}
