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
    public partial class dlgTaskEntityValue : Form, ITaskDialog
    {
        #region " Member Field "

        private Dictionary<string, MethodInfo[]> dicMethod = new Dictionary<string, MethodInfo[]>();
        private Dictionary<string, ParameterInfo[]> dicParameter = new Dictionary<string, ParameterInfo[]>();

        private EntityValueTask task;

        #endregion

        #region " Property "

        public ITask Task
        {
            get { return task; }
        }

        #endregion

        #region " Creator "

        public dlgTaskEntityValue()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            task = new EntityValueTask();

            InitDialog();
        }

        public dlgTaskEntityValue(EntityValueTask task)
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            this.task = task;

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

            lvEntity.HideSelection = false;

            cboResultUseType.EnumType = typeof(ResultUseTypeItem);
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
                if (string.IsNullOrEmpty(txtTaskName.Text))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please input Task Name.", MessageBoxIcon.Information);
                    txtTaskName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cboResultUseType.Text))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Result Usage.", MessageBoxIcon.Information);
                    cboResultUseType.Focus();
                    return;
                }

                if (lvEntity.SelectedItems.Count < 1)
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Entity.", MessageBoxIcon.Information);
                    return;
                }

                task.Name = txtTaskName.Text;
                task.ResultUsage = (ResultUseTypeItem)cboResultUseType.EnumValue;
                task.Entity = (RegexEntity)lvEntity.SelectedItems[0].Tag;

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
        }

        #endregion

        #region [ Etc ]

        void dlgOption_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

        #region " Method "

        private void SetControls()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Analysis analysis = Analysis.GetInstance();

                foreach (Entity entity in analysis.Entities)
                {
                    ListViewItem item = new ListViewItem();

                    item.Name = item.Text = entity.Name;
                    item.SubItems.Add((entity.Parent == null) ? string.Empty : entity.Parent.Name);
                    item.Tag = entity;

                    lvEntity.Items.Add(item);
                }

                if (task != null)
                {
                    txtTaskName.Text = task.Name;

                    cboResultUseType.Text = task.ResultUsage.ToString();

                    if (lvEntity.Items.ContainsKey(task.Entity.Name))
                        lvEntity.Items[task.Entity.Name].Selected = true;
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
