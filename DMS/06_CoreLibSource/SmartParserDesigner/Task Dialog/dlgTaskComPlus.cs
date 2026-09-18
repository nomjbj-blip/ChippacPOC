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
    public partial class dlgTaskComPlus : Form, ITaskDialog
    {
        #region " Member Field "

        private bool isDragging = false;
        private int iPos = -1;

        private Dictionary<string, MethodInfo[]> dicMethod = new Dictionary<string, MethodInfo[]>();
        private Dictionary<string, ParameterInfo[]> dicParameter = new Dictionary<string, ParameterInfo[]>();

        private COMPlusTask task;

        #endregion

        #region " Property "

        public ITask Task
        {
            get { return task; }
        }

        #endregion

        #region " Creator "

        public dlgTaskComPlus()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            task = new COMPlusTask();

            InitDialog();
        }

        public dlgTaskComPlus(COMPlusTask task)
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

            uclTitleMethod.MouseDown += new MouseEventHandler(uclTitleMethod_MouseDown);
            uclTitleMethod.MouseMove += new MouseEventHandler(uclTitleMethod_MouseMove);
            uclTitleMethod.MouseUp += new MouseEventHandler(uclTitleMethod_MouseUp);

            uclTitleParameter.MouseDown += new MouseEventHandler(uclTitleParameter_MouseDown);
            uclTitleParameter.MouseMove += new MouseEventHandler(uclTitleParameter_MouseMove);
            uclTitleParameter.MouseUp += new MouseEventHandler(uclTitleParameter_MouseUp);

            lvComponent.SelectedIndexChanged += new EventHandler(lvComponent_SelectedIndexChanged);
            lvMethod.SelectedIndexChanged += new EventHandler(lvMethod_SelectedIndexChanged);

            lvComponent.HideSelection = false;
            lvMethod.HideSelection = false;

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

                if (lvComponent.SelectedItems.Count < 1)
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Component.", MessageBoxIcon.Information);
                    return;
                }

                if (lvMethod.SelectedItems.Count < 1)
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Method.", MessageBoxIcon.Information);
                    return;
                }

                task.Name = txtTaskName.Text;
                task.ResultUsage = (ResultUseTypeItem)cboResultUseType.EnumValue;
                task.ComponentName = lvComponent.SelectedItems[0].Text;
                task.MethodName = lvMethod.SelectedItems[0].Text;

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

            if (pnlCom.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (pnlMethod.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            pnlCom.Size = new Size(pnlCom.Width, pnlCom.Height + (e.Y - iPos));
        }

        private void uclTitleMethod_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = false;
        }

        private void uclTitleParameter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            iPos = e.Y;
            isDragging = true;
        }

        private void uclTitleParameter_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            if (pnlMethod.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (pnlParameter.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            pnlMethod.Size = new Size(pnlMethod.Width, pnlMethod.Height + (e.Y - iPos));
        }

        private void uclTitleParameter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = false;
        }

        #endregion

        #region [ ListView Selection ]

        void lvComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvComponent.SelectedItems.Count < 1)
                return;

            lvMethod.Items.Clear();
            lvParameter.Items.Clear();

            string strComName = lvComponent.SelectedItems[0].Text;

            if (!dicMethod.ContainsKey(strComName))
                dicMethod.Add(strComName, new COMWrapper(strComName).ComObject.GetType().GetMethods());

            foreach (MethodInfo mi in dicMethod[strComName])
            {
                ListViewItem item = new ListViewItem();
                item.Name = item.Text = mi.Name;

                if (!dicParameter.ContainsKey(mi.Name))
                    dicParameter.Add(mi.Name, mi.GetParameters());

                item.SubItems.Add(dicParameter[mi.Name].Length.ToString());
                item.SubItems.Add(mi.ReturnType.ToString());

                item.Tag = mi;

                lvMethod.Items.Add(item);
            }

        }

        void lvMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMethod.SelectedItems.Count < 1)
                return;

            lvParameter.Items.Clear();
            string strMethodName = lvMethod.SelectedItems[0].Text;

            if (!dicParameter.ContainsKey(strMethodName))
                dicParameter.Add(strMethodName, ((MethodInfo)lvMethod.SelectedItems[0].Tag).GetParameters());

            foreach (ParameterInfo pi in dicParameter[strMethodName])
            {
                ListViewItem item = new ListViewItem();
                item.Name = item.Text = pi.Name;

                item.SubItems.Add(pi.ParameterType.ToString());

                lvParameter.Items.Add(item);
            }
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

                COMAdminHelper oComHelper = new COMAdminHelper();
                Dictionary<string, string[]> dicCOMAppComponents = oComHelper.GetCOMComponentList();

                foreach (KeyValuePair<string, string[]> kv in dicCOMAppComponents)
                {
                    foreach (string progID in kv.Value)
                    {
                        ListViewItem item = new ListViewItem();

                        item.Name = item.Text = progID;
                        item.SubItems.Add(kv.Key);
                        item.Tag = progID;

                        lvComponent.Items.Add(item);
                    }
                }

                if (task != null)
                {
                    txtTaskName.Text = task.Name;

                    cboResultUseType.Text = task.ResultUsage.ToString();

                    if (lvComponent.Items.ContainsKey(task.ComponentName))
                        lvComponent.Items[task.ComponentName].Selected = true;

                    if (lvMethod.Items.ContainsKey(task.MethodName))
                        lvMethod.Items[task.MethodName].Selected = true;

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
