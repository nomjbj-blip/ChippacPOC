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
    public partial class dlgTaskDB : Form, ITaskDialog
    {
        #region " Member Field "

        private bool isDragging = false;
        private int iPos = -1;

        private DBTask task;

        #endregion

        #region " Property "

        public ITask Task
        {
            get { return task; }
        }

        #endregion

        #region " Creator "

        public dlgTaskDB()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            task = new DBTask();

            InitDialog();
        }

        public dlgTaskDB(DBTask task)
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BeginInvoke(new MethodInvoker(SetControls));
        }

        #endregion

        #region [ Query Text ]

        private Regex regexParameter = new Regex(@"(\?\?)|:(?<parameter>[_a-zA-Z0-9]+)\b", RegexOptions.ExplicitCapture | RegexOptions.Singleline);

        void txtQuery_TextChanged(object sender, EventArgs e)
        {
            MatchCollection matches = regexParameter.Matches(txtQuery.Text);

            lvParameter.Items.Clear();

            ListViewItem item = null;
            int iDynamic = 1;
            foreach (Match match in matches)
            {
                item = new ListViewItem();

                if (match.Value != "??") // static parameter
                    item.Text = item.Name = match.Groups["parameter"].Value;
                else
                    item.Text = item.Name = "DYNAMIC_" + (iDynamic++).ToString();

                if (!lvParameter.Items.ContainsKey(item.Name))
                    lvParameter.Items.Add(item);
            }
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

                if (string.IsNullOrEmpty(cboQueryType.Text))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please select a Query Type.", MessageBoxIcon.Information);
                    cboQueryType.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtQuery.Text.Trim()))
                {
                    DACrux.SP.Common.Utility.ShowMessageBox("Please input the query text.", MessageBoxIcon.Information);
                    txtTaskName.Focus();
                    return;
                }

                task.Name = txtTaskName.Text;
                task.ResultUsage = (ResultUseTypeItem)cboResultUseType.EnumValue;
                task.QueryType = (QueryTypeItem)cboQueryType.EnumValue;
                task.Query = txtQuery.Text;
                task.TargetColumn = txtTargetColumn.Text;

                task.ParameterNames = (from ListViewItem item in lvParameter.Items
                                       select item.Name).ToList();

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

        #region [ Title Moving ]

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

            if (pnlQuery.Size.Height < 1)
            {
                if ((e.Y - iPos) < 0) return;
            }

            if (pnlParameter.Size.Height < 1)
            {
                if ((e.Y - iPos) > 0) return;
            }

            pnlQuery.Size = new Size(pnlQuery.Width, pnlQuery.Height + (e.Y - iPos));
        }

        private void uclTitleParameter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            isDragging = false;
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

        private void InitDialog()
        {
            KeyPreview = true;
            this.KeyDown += dlgOption_KeyDown;
            StartPosition = FormStartPosition.CenterParent;

            uclTitleParameter.MouseDown += new MouseEventHandler(uclTitleParameter_MouseDown);
            uclTitleParameter.MouseMove += new MouseEventHandler(uclTitleParameter_MouseMove);
            uclTitleParameter.MouseUp += new MouseEventHandler(uclTitleParameter_MouseUp);

            txtQuery.Settings.Keywords.Add("INSERT");
            txtQuery.Settings.Keywords.Add("INTO");
            txtQuery.Settings.Keywords.Add("SELECT");
            txtQuery.Settings.Keywords.Add("FROM");
            txtQuery.Settings.Keywords.Add("WHERE");
            txtQuery.Settings.Keywords.Add("AND");
            txtQuery.Settings.Keywords.Add("OR");
            txtQuery.Settings.Keywords.Add("CASE");
            txtQuery.Settings.Keywords.Add("WHEN");
            txtQuery.Settings.Keywords.Add("THEN");
            txtQuery.Settings.Keywords.Add("IF");
            txtQuery.Settings.Keywords.Add("ELSE");
            txtQuery.Settings.Keywords.Add("ENDIF");
            txtQuery.Settings.Keywords.Add("UPDATE");
            txtQuery.Settings.Keywords.Add("SET");
            txtQuery.Settings.Keywords.Add("VALUES");
            txtQuery.Settings.Keywords.Add("CREATE");
            txtQuery.Settings.Keywords.Add("DROP");
            txtQuery.Settings.Keywords.Add("TRUNC");
            txtQuery.Settings.Keywords.Add("TRUNCATE");
            txtQuery.Settings.Keywords.Add("VALUES");
            txtQuery.Settings.Keywords.Add("COUNT");
            txtQuery.Settings.Keywords.Add("SUM");
            txtQuery.Settings.Keywords.Add("AVG");
            txtQuery.Settings.Keywords.Add("DECODE");
            txtQuery.Settings.Keywords.Add("NVL");
            txtQuery.Settings.Keywords.Add("MAX");
            txtQuery.Settings.Keywords.Add("MIN");
            txtQuery.Settings.Keywords.Add("CEIL");
            txtQuery.Settings.Keywords.Add("ROUND");
            txtQuery.Settings.Keywords.Add("FUNCTION");
            txtQuery.Settings.Keywords.Add("PROCEDURE");
            txtQuery.Settings.Keywords.Add("TABLE");
            txtQuery.Settings.Keywords.Add("VIEW");

            txtQuery.Settings.KeywordColor = Color.Blue;
            txtQuery.CompileKeywords();

            txtQuery.Settings.DBParameter = ":[a-zA-Z_0-9]+\\b";
            txtQuery.Settings.DBParameterColor = Color.Green;
            txtQuery.Settings.DBDynamicQuery = @"\?\?";

            txtQuery.TextChanged += new EventHandler(txtQuery_TextChanged);

            cboResultUseType.EnumType = typeof(ResultUseTypeItem);
            cboQueryType.EnumType = typeof(QueryTypeItem);

            cboQueryType.SelectedIndexChanged += new EventHandler(cboQueryType_SelectedIndexChanged);
        }

        void cboQueryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTargetColumn.Visible = txtTargetColumn.Visible = ((QueryTypeItem)cboQueryType.EnumValue) == QueryTypeItem.Select;
        }

        private void SetControls()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (task != null)
                {
                    txtTaskName.Text = task.Name;
                    cboResultUseType.Text = task.ResultUsage.ToString();
                    cboQueryType.Text = task.QueryType.ToString();
                    txtQuery.Text = task.Query;
                    txtTargetColumn.Text = task.TargetColumn;

                    txtQuery.ProcessAllLines();
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
