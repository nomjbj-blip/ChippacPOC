using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DACrux.SP.Common;
using System.Reflection;

namespace SmartParser.Designer
{
    public partial class dlgScript : Form
    {
        #region " Member Field "

        private bool isDragging = false;
        private int iPos = -1;

        Script script = null;

        #endregion

        #region " Property "

        public Script Script
        {
            get { return script; }
            set { script = value; }
        }

        #endregion

        #region " Creator "

        public dlgScript()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            script = new Script();

            InitDialog();
        }

        public dlgScript(Script script)
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            this.script = script;

            InitDialog();
        }

        #endregion

        #region " Event Handler "

        #region [ Loading ]

        private void InitDialog()
        {
            KeyPreview = true;
            this.KeyDown += dlgOption_KeyDown;
            StartPosition = FormStartPosition.CenterParent;

            uclTitleArgument.MouseDown += new MouseEventHandler(uclTitleMethod_MouseDown);
            uclTitleArgument.MouseMove += new MouseEventHandler(uclTitleMethod_MouseMove);
            uclTitleArgument.MouseUp += new MouseEventHandler(uclTitleMethod_MouseUp);

            lvArgument.SelectedIndexChanged += new EventHandler(lvArgument_SelectedIndexChanged);

            lvTaskTarget.HideSelection = false;
            lvArgument.HideSelection = false;

            cboCommand.EnumType = typeof(ScriptCommandTypeItem);
            cboRunningMode.EnumType = typeof(TaskRunningModeItem);

            cboCommand.SelectedIndexChanged += new EventHandler(cboCommand_SelectedIndexChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BeginInvoke(new MethodInvoker(SetControls));
        }

        #endregion

        #region [ Button ]

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cboCommand.Text))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select Command.", MessageBoxIcon.Information);
                    cboCommand.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cboRunningMode.Text))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Result Usage.", MessageBoxIcon.Information);
                    cboRunningMode.Focus();
                    return;
                }

                if (lvTaskTarget.SelectedItems.Count < 1)
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Target Task.", MessageBoxIcon.Information);
                    return;
                }

                script.CommandType = (ScriptCommandTypeItem)cboCommand.EnumValue;
                script.RunningMode = (TaskRunningModeItem)cboRunningMode.EnumValue;
                script.TargetTask = (ITask)lvTaskTarget.SelectedItems[0].Tag;
                script.Args = (from ListViewItem item in lvArgument.Items
                               select item.Tag as Data).ToArray();

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            //if (lvFileList.Items.Count < 1)
            //{
            //    MessageBox.Show("Please select one or more Sample file(s).", "Information"
            //        , MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvArgument.Items.Count < 2)
                    return;

                if (lvArgument.SelectedItems.Count < 1)
                    return;

                ListViewItem item = lvArgument.SelectedItems[0];
                int index = item.Index;

                if (index == 0)
                    return;

                item.Remove();

                lvArgument.Items.Insert(index - 1, item);
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
                if (lvArgument.Items.Count < 2)
                    return;

                if (lvArgument.SelectedItems.Count < 1)
                    return;

                ListViewItem item = lvArgument.SelectedItems[0];
                int index = item.Index;

                if (index == lvArgument.Items.Count - 1)
                    return;

                item.Remove();

                lvArgument.Items.Insert(index + 1, item);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Information);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvArgument.SelectedItems.Count < 1)
                    return;

                if (!(lvArgument.SelectedItems[0].Tag is ITask))
                    txtData.Text = string.Empty;

                lvArgument.SelectedItems[0].Remove();
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Information);
            }
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            if (lvTaskArgument.SelectedItems.Count < 1)
                return;

            lvArgument.Items.Add(GetArgsListViewItem(lvTaskArgument.SelectedItems[0].Tag as Data));
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (lvArgument.SelectedItems.Count < 1)
            {
                // add
                lvArgument.Items.Add(GetArgsListViewItem(new Data(txtData.Text)));
            }
            else
            {
                // edit
                lvArgument.SelectedItems[0].Text = (lvArgument.SelectedItems[0].Tag as Data).Value = txtData.Text;
            }

        }

        #endregion

        #region [ Title Moving ]

        private void uclTitleMethod_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            iPos = e.Y;
            isDragging = true;
        }

        private void uclTitleMethod_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            if (pnlTarget.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (pnlArgument.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            pnlTarget.Size = new Size(pnlTarget.Width, pnlTarget.Height + (e.Y - iPos));
        }

        private void uclTitleMethod_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = false;
        }

        #endregion

        #region [ ListView Selection ]

        void lvArgument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvArgument.SelectedItems.Count < 1)
            {
                btnSaveData.Text = "Add";
                return;
            }
            else
                btnSaveData.Text = "Save";

            object tag = lvArgument.SelectedItems[0].Tag;

            if (tag is ITask)
            {
                ListViewItem item = lvTaskArgument.Items[lvArgument.SelectedItems[0].Text];
                item.Selected = true;
                lvTaskArgument.EnsureVisible(item.Index);
            }
            else
            {
                txtData.Text = (tag as Data).Value;
            }
        }

        #endregion

        #region [ ComboBox Selection ]

        void cboCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptCommandTypeItem cmdType = (ScriptCommandTypeItem)cboCommand.EnumValue;
            DACrux.SP.Common.Utility.SetEnable(pnlArgument, (cmdType == ScriptCommandTypeItem.SET_ARGS || cmdType == ScriptCommandTypeItem.SET_VALUE));
        }

        #endregion

        #region [ Etc ]

        void dlgOption_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region " Method "

        private ListViewItem GetTaskListViewItem(ITask task)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = task.Name;
            item.SubItems.Add(task.Type.ToString().Replace("_", " "));
            item.SubItems.Add(task.ResultUsage.ToString());

            item.Tag = task;

            return item;
        }

        private ListViewItem GetArgsListViewItem(Data data)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = (data is ITask) ? (data as ITask).Name : data.Value;

            item.Tag = data;

            return item;
        }

        private void SetControls()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Analysis analysis  = Analysis.GetInstance();

                foreach (ITask task in analysis.Tasks)
                {
                    lvTaskTarget.Items.Add(GetTaskListViewItem(task));
                    lvTaskArgument.Items.Add(GetTaskListViewItem(task));
                }

                if (script != null)
                {
                    cboCommand.Text = script.CommandType.ToString(); 
                    cboRunningMode.Text = script.RunningMode.ToString();

                    if (lvTaskTarget.Items.ContainsKey(script.TargetTask.Name))
                        lvTaskTarget.Items[script.TargetTask.Name].Selected = true;

                    foreach (Data data in script.Args)
                        lvArgument.Items.Add(GetArgsListViewItem(data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
