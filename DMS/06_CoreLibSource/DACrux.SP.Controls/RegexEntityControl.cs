using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DACrux.SP.Common;

namespace DACrux.SP.Controls
{
    public delegate void EntityRegexGroupRefreshRequestedEventHandlers(string strRegex, System.Text.RegularExpressions.RegexOptions option);
    public delegate void EntityFindRequestedEventHandlers(RegexEntity entity);
    public delegate void SectionFindRequestedEventHandlers(RegexEntity startEntity, RegexEntity endEntity);
    public partial class RegexEntityControl : UserControl
    {
        #region " Member Field & Property "

        private RegexEntity entity = null;
        RegexOptions option = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public string RegexString
        {
            get { return txtRegex.Text; }
            set { txtRegex.Text = value; }
        }

        public bool ShowIgnoreCase
        {
            get { return chkIgnoreCase.Visible; }
            set { chkIgnoreCase.Visible = value; }
        }

        public RegexOptions RegexOption
        {
            get
            {
                return (chkIgnoreCase.Checked) ?
                    (RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase) :
                    (RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            }
            set
            {
                chkIgnoreCase.Checked = ((value & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase);
                option = value;
            }
        }

        public bool IsValidRegex
        {
            get
            {
                try
                {
                    new Regex(txtRegex.Text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        private string strRegex = string.Empty;
        //private RegexOptions option = RegexOptions.None;
        public RexEntry rexentrys = new RexEntry();
        private string regulars = string.Empty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RegexEntity EntityContent
        {
            get
            {
                try
                {
                    if (entity == null)
                        entity = new RegexEntity();
                    entity.Name = txtName.Text;
                    entity.DefaultValue = txtDefault.Text;

                    entity.Force = chkMustExists.Checked;
                    entity.Label = txtLabel.Text;
                    entity.RegexString = txtRegex.Text;

                    if (chkStreamMode.Checked)
                    {
                        option &= ~RegexOptions.Singleline;
                        option = option | RegexOptions.Multiline;
                    }
                    entity.RegexOption = option;
                    entity.RegexValueGroupName = cboValueGroup.Text;
                    entity.RegexValueSeperator = txtSeparator.Text;
                    entity.ValueType = GetValueType();
                    entity.StreamMode = chkStreamMode.Checked ? StreamModeItem.Stream : StreamModeItem.None;

                    return entity;
                }
                catch (Exception ex)
                {
                    Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    return null;
                }
                finally
                {
                    entity = null;
                }
            }
            set
            {
                try
                {
                    if (value == null)
                    {
                        ResetControls();
                        entity = null;
                        return;
                    }

                    txtName.Text = value.Name;
                    txtDefault.Text = value.DefaultValue;
                    chkMustExists.Checked = value.Force;
                    txtLabel.Text = value.Label;
                    txtRegex.Text = value.RegexString;
                    option = value.RegexOption;
                    if (option.ToString().IndexOf("IgnoreCase") > -1)
                    {
                        chkIgnoreCase.Checked = true;
                    }
                    else
                    {
                        chkIgnoreCase.Checked = false;
                    }
                    cboValueGroup.Text = value.RegexValueGroupName;
                    txtSeparator.Text = value.RegexValueSeperator;
                    SetValueType(value.ValueType);
                    chkStreamMode.Checked = value.StreamMode == StreamModeItem.Stream;

                    entity = value;
                }
                catch (Exception ex)
                {
                    Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    ResetControls();
                    entity = null;
                }

            }
        }
        [Browsable(false)]
        public string Def
        {
            get
            {
                return this.lblDefault.Text;
            }
            set
            {
                this.lblDefault.Text = value;
            }
        }
        [Browsable(false)]
        public string array
        {
            get
            {
                return this.chkArray.Text;
            }
            set
            {
                this.chkArray.Text = value;
            }
        }
        [Browsable(false)]
        public string find
        {
            get
            {
                return this.btnFind.Text;
            }
            set
            {
                this.btnFind.Text = value;

            }
        }
        [Browsable(false)]
        public string defaults
        {
            get
            {
                return lblDefault.Text;
            }
            set
            {
                lblDefault.Text = value;
            }
        }
        [Browsable(false)]
        public string name
        {
            get
            {
                return this.lblName.Text;
            }
            set
            {
                this.lblName.Text = value;
            }
        }
        [Browsable(false)]
        public string label
        {
            get
            {
                return this.lblLabel.Text;
            }
            set
            {
                this.lblLabel.Text = value;
            }
        }
        [Browsable(false)]
        public string remove
        {
            get
            {
                return this.btnRemove.Text;
            }
            set
            {
                this.btnRemove.Text = value;
            }
        }
        [Browsable(false)]
        public string save
        {
            get
            {
                return this.btnSave.Text;
            }
            set
            {
                this.btnSave.Text = value;
            }

        }
        [Browsable(false)]
        public string sep
        {
            get
            {
                return this.label4.Text;
            }
            set
            {
                this.label4.Text = value;
            }
        }
        [Browsable(false)]
        public string streammode
        {
            get
            {
                return this.chkStreamMode.Text;
            }
            set
            {
                this.chkStreamMode.Text = value;
            }
        }
        [Browsable(false)]
        public string valuegroup
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }
        [Browsable(false)]
        public string valuetype
        {
            get
            {
                return this.lblValueType.Text;
            }
            set
            {
                this.lblValueType.Text = value;
            }
        }
        [Browsable(false)]
        public string valuemust
        {
            get
            {
                return this.chkMustExists.Text;
            }
            set
            {
                this.chkMustExists.Text = value;
            }
        }
        [Browsable(false)]
        public string regular
        {
            get
            {
                return label2.Text;
            }
            set
            {
                label2.Text = value;
            }
        }
        [Browsable(false)]
        public string Ignorecase
        {
            get
            {
                return this.chkIgnoreCase.Text;
            }
            set
            {
                this.chkIgnoreCase.Text = value;
            }
        }
        [Browsable(false)]
        public string etc
        {
            get
            {
                return this.btnEtc.Text;
            }
            set
            {
                this.btnEtc.Text = value;
            }
        }
        [Browsable(false)]
        public string grouping
        {
            get
            {
                return this.btnGrouping.Text;
            }
            set
            {
                this.btnGrouping.Text = value;
            }
        }
        [Browsable(false)]
        public string quanti
        {
            get
            {
                return this.btnQuantifiers.Text;
            }
            set
            {
                this.btnQuantifiers.Text = value;
            }

        }
        [Browsable(false)]
        public string zero
        {
            get
            {
                return this.toolStripMenuItem1.Text;
            }
            set
            {
                this.toolStripMenuItem1.Text = value;
            }
        }
        [Browsable(false)]
        public string one
        {
            get
            {
                return this.toolStripMenuItem2.Text;
            }
            set
            {
                this.toolStripMenuItem2.Text = value;
            }
        }
        [Browsable(false)]
        public string zerone
        {
            get
            {
                return this.toolStripMenuItem3.Text;
            }
            set
            {
                this.toolStripMenuItem3.Text = value;
            }
        }
        [Browsable(false)]
        public string ntime
        {
            get
            {
                return this.toolStripMenuItem4.Text;
            }
            set
            {
                this.toolStripMenuItem4.Text = value;
            }
        }
        [Browsable(false)]
        public string nleasttime
        {
            get
            {
                return this.toolStripMenuItem5.Text;
            }
            set
            {
                this.toolStripMenuItem5.Text = value;
            }
        }
        [Browsable(false)]
        public string nfromtime
        {
            get { return this.toolStripMenuItem6.Text; }
            set
            {
                this.toolStripMenuItem6.Text = value;
            }
        }
        [Browsable(false)]
        public string groupname
        {
            get
            {
                return this.toolStripMenuItem7.Text;
            }
            set
            {
                this.toolStripMenuItem7.Text = value;
            }
        }
        [Browsable(false)]
        public string groupequal
        {
            get
            {
                return this.toolStripMenuItem8.Text;
            }
            set
            {
                this.toolStripMenuItem8.Text = value;
            }
        }
        [Browsable(false)]
        public string tab
        {
            get
            {
                return this.toolStripMenuItem9.Text;
            }
            set
            {
                this.toolStripMenuItem9.Text = value;
            }
        }
        [Browsable(false)]
        public string returnhome
        {
            get { return this.toolStripMenuItem10.Text; }
            set { this.toolStripMenuItem10.Text = value; }
        }
        [Browsable(false)]
        public string newline
        {
            get
            {
                return this.vToolStripMenuItem.Text;
            }
            set
            {
                this.vToolStripMenuItem.Text = value;
            }
        }
        [Browsable(false)]
        public string word
        {
            get { return this.wToolStripMenuItem.Text; }
            set
            {
                this.wToolStripMenuItem.Text = value;
            }
        }
        [Browsable(false)]
        public string nonword
        {
            get { return this.wMatchesANonwordToolStripMenuItem.Text; }
            set { this.wMatchesANonwordToolStripMenuItem.Text = value; }
        }
        [Browsable(false)]
        public string space
        {
            get { return this.sMatchesAWhiteSpaceToolStripMenuItem.Text; }
            set { this.sMatchesAWhiteSpaceToolStripMenuItem.Text = value; }
        }
        [Browsable(false)]
        public string nonspace
        {
            get
            {
                return this.sMatchesANonwhiteSpaceToolStripMenuItem.Text;
            }
            set { this.sMatchesANonwhiteSpaceToolStripMenuItem.Text = value; }
        }
        [Browsable(false)]
        public string dec
        {
            get { return this.dMatchesANumberToolStripMenuItem.Text; }
            set
            {
                this.dMatchesANumberToolStripMenuItem.Text = value;
            }
        }
        [Browsable(false)]
        public string nondec
        {
            get { return this.dMatchesANonnumberToolStripMenuItem.Text; }
            set
            {
                this.dMatchesANonnumberToolStripMenuItem.Text = value;
            }
        }
        [Browsable(false)]
        public string startline
        {
            get { return this.toolStripMenuItem12.Text; }
            set
            {
                this.toolStripMenuItem12.Text = value;
            }
        }
        [Browsable(false)]
        public string endline
        {
            get { return this.matchesEndOfTheStringOrLineToolStripMenuItem.Text; }
            set
            {
                this.matchesEndOfTheStringOrLineToolStripMenuItem.Text = value;
            }
        }
        [Browsable(false)]
        private void ResetControls()
        {
            this.Reset(cboValueType, chkMustExists);
        }

        #endregion

        #region " Creator "

        public RegexEntityControl()
        {

            InitializeComponent();
            foreach (ToolStripMenuItem item in menuCounter.Items)
                item.Click += new EventHandler(Menu_Click);

            foreach (ToolStripMenuItem item in menuGroup.Items)
                item.Click += new EventHandler(Menu_Click);

            foreach (ToolStripMenuItem item in menuEtc.Items)
                item.Click += new EventHandler(Menu_Click);
            txtRegex.TextChanged += (s, a) => uclRegexChanged(txtRegex.Text, option);
            chkIgnoreCase.CheckedChanged += (s, a) => FireRegexChangedEvent();
            btnSave.Click += new EventHandler(btnSave_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnFind.Click += new EventHandler(btnFind_Click);

            chkArray.CheckedChanged += new EventHandler(chkArray_CheckedChanged);
            chkStreamMode.CheckedChanged += new EventHandler(chkStreamMode_CheckedChanged);
        }

        void chkStreamMode_CheckedChanged(object sender, EventArgs e)
        {
            if (entity == null)
                return;

            //if (chkStreamMode.Checked)
            //{
            //    this.entity.RegexOption &= ~RegexOptions.Singleline;
            //    this.entity.RegexOption &= RegexOptions.Multiline;
            //}
            //else
            //{
            //    this.entity.RegexOption &= RegexOptions.Singleline;
            //    this.entity.RegexOption &= ~RegexOptions.Multiline;
            //}
        }

        #endregion

        #region " Event Handler "

        void chkArray_CheckedChanged(object sender, EventArgs e)
        {
            txtSeparator.Enabled = chkArray.Checked;
        }

        void uclRegexChanged(string strRegex, RegexOptions option)
        {
            try
            {
                this.strRegex = txtRegex.Text;
                this.option = option;

                Regex expr = new Regex(strRegex, option);
                cboValueGroup.Items.Clear();
                cboValueGroup.Items.AddRange((from string name in expr.GetGroupNames()
                                              where name.CompareTo("0") > 0
                                              select name).ToArray<string>());

                //if (this.ParentForm.MdiParent is DACrux.SP.Common.IStatusBar)
                //    (this.ParentForm.MdiParent as DACrux.SP.Common.IStatusBar).SetMessage(string.Empty);
            }
            catch 
            {
                //if (this.ParentForm.MdiParent is DACrux.SP.Common.IStatusBar)
                //    (this.ParentForm.MdiParent as DACrux.SP.Common.IStatusBar).SetMessage(ex.Message);
                //else
                //    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.strRegex = txtRegex.Text;
                //this.option = option;

                if (FindRequested != null)
                    FindRequested(EntityContent);
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    Utility.ShowMessageBox("Please input the entity name.", MessageBoxIcon.Information);
                    txtName.Focus();
                    return;
                }

                if (chkArray.Checked && txtSeparator.Text.IsNullOrEmpty())
                {
                    Utility.ShowMessageBox("Please input the seperator character(s).", MessageBoxIcon.Information);
                    txtSeparator.Focus();
                    return;
                }

                if (SaveRequested != null)
                {
                    SaveRequested(EntityContent);
                    //ResetControls();
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (entity.Name == "bin" || entity.Name == "Wafer_ID" || entity.Name == "Product"
                    || entity.Name == "FlatZone" || entity.Name == "X" || entity.Name == "Y" || entity.Name == "Total"
                    || entity.Name == "Run_ID" || entity.Name == "Wafer_Num")
                {
                    MessageBox.Show("기본속성입니다.");
                    return;
                }

                if (RemoveRequested != null)
                {
                    RemoveRequested(EntityContent);
                    ResetControls();
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region " Method "

        private void SetValueType(Type type)
        {
            chkArray.Checked = false;
            cboValueType.Text = string.Empty;

            if (type == null)
                return;

            chkArray.Checked = type.ToString().Contains("[]");

            if (chkArray.Checked)
            {
                if (type == typeof(string[]))
                    cboValueType.Text = "String";
                else if (type == typeof(double[]))
                    cboValueType.Text = "Number";
                else if (type == typeof(DateTime[]))
                    cboValueType.Text = "DateTime";
            }
            else
            {
                if (type == typeof(string))
                    cboValueType.Text = "String";
                else if (type == typeof(double))
                    cboValueType.Text = "Number";
                else if (type == typeof(DateTime))
                    cboValueType.Text = "DateTime";
            }

        }

        private Type GetValueType()
        {
            if (chkArray.Checked)
            {
                switch (cboValueType.Text)
                {
                    case "String":
                        return typeof(string[]);
                    case "Number":
                        return typeof(double[]);
                    case "DateTime":
                        return typeof(DateTime[]);
                }
            }
            else
            {
                switch (cboValueType.Text)
                {
                    case "String":
                        return typeof(string);
                    case "Number":
                        return typeof(double);
                    case "DateTime":
                        return typeof(DateTime);
                }
            }

            return null;
        }

        #endregion

        #region " Event "

        public event EntityFindRequestedEventHandler FindRequested;
        public event RegexEntitySaveRequestedEventHandler SaveRequested;
        public event RegexEntityRemoveRequestedEventHandler RemoveRequested;

        #endregion
        [Category("Setup"), Description("Notify Regex string has been changed.")]
        public event EntityRegexGroupRefreshRequestedEventHandler RegexChanged;
        private void FireRegexChangedEvent()
        {
            try
            {
                if (chkIgnoreCase.Checked)
                    option = option | RegexOptions.IgnoreCase;
                else
                    option = option ^ RegexOptions.IgnoreCase;

                if (RegexChanged != null)
                    RegexChanged(txtRegex.Text, option);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnEtc_Click(object sender, EventArgs e)
        {
            menuEtc.Show(btnEtc, new Point(btnEtc.Width + 2, 0));
        }

        private void btnQuantifiers_Click(object sender, EventArgs e)
        {
            menuCounter.Show(btnQuantifiers, new Point(btnQuantifiers.Width + 2, 0));
        }

        private void btnGrouping_Click(object sender, EventArgs e)
        {
            menuGroup.Show(btnGrouping, new Point(btnGrouping.Width + 2, 0));
        }

        void Menu_Click(object sender, EventArgs e)
        {
            txtRegex.Text += (sender as ToolStripMenuItem).Tag.ToString();
        }
    }

    public delegate void RegexEntitySaveRequestedEventHandler(RegexEntity entity);
    public delegate void RegexEntityRemoveRequestedEventHandler(RegexEntity entity);
}
