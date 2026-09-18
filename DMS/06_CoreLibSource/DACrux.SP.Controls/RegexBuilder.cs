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
    public partial class RegexBuilder : UserControl
    {
        #region " Member Field & Property "

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
        //[Category("test")]
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
        public string nfromtime
        {
            get { return this.toolStripMenuItem6.Text; }
            set
            {
                this.toolStripMenuItem6.Text = value;
            }
        }
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
        public string returnhome
        {
            get { return this.toolStripMenuItem10.Text; }
            set { this.toolStripMenuItem10.Text = value; }
        }
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
        public string word
        {
            get { return this.wToolStripMenuItem.Text; }
            set
            {
                this.wToolStripMenuItem.Text = value;
            }
        }
        public string nonword
        {
            get { return this.wMatchesANonwordToolStripMenuItem.Text; }
            set { this.wMatchesANonwordToolStripMenuItem.Text = value; }
        }
        public string space
        {
            get { return this.sMatchesAWhiteSpaceToolStripMenuItem.Text; }
            set { this.sMatchesAWhiteSpaceToolStripMenuItem.Text = value; }
        }
        public string nonspace
        {
            get
            {
                return this.sMatchesANonwhiteSpaceToolStripMenuItem.Text;
            }
            set { this.sMatchesANonwhiteSpaceToolStripMenuItem.Text = value; }
        }
        public string dec
        {
            get { return this.dMatchesANumberToolStripMenuItem.Text; }
            set
            {
                this.dMatchesANumberToolStripMenuItem.Text = value;
            }
        }
        public string nondec
        {
            get { return this.dMatchesANonnumberToolStripMenuItem.Text; }
            set
            {
                this.dMatchesANonnumberToolStripMenuItem.Text = value;
            }
        }
        public string startline
        {
            get { return this.toolStripMenuItem12.Text; }
            set
            {
                this.toolStripMenuItem12.Text = value;
            }
        }
        public string endline
        {
            get { return this.matchesEndOfTheStringOrLineToolStripMenuItem.Text; }
            set
            {
                this.matchesEndOfTheStringOrLineToolStripMenuItem.Text = value;
            }
        }
     
        #endregion

        #region " Creator "

        public RegexBuilder()
        {
            
            InitializeComponent();
           
            foreach (ToolStripMenuItem item in menuCounter.Items)
                item.Click += new EventHandler(Menu_Click);

            foreach (ToolStripMenuItem item in menuGroup.Items)
                item.Click += new EventHandler(Menu_Click);

            foreach (ToolStripMenuItem item in menuEtc.Items)
                item.Click += new EventHandler(Menu_Click);
            
            txtRegex.TextChanged += (s, a) => FireRegexChangedEvent();
            chkIgnoreCase.CheckedChanged += (s, a) => FireRegexChangedEvent();
        }

        #endregion

        #region " Event "

        [Category("Setup"), Description("Notify Regex string has been changed.")]
        public event EntityRegexGroupRefreshRequestedEventHandlers RegexChanged;

        #endregion

        #region " Event Handler "

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

        #endregion

        private void menuContext_MouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Hide();
        }
    }

    public delegate void EntityRegexGroupRefreshRequestedEventHandler(string strRegex, System.Text.RegularExpressions.RegexOptions option);
    public delegate void EntityFindRequestedEventHandler(RegexEntity entity);
    public delegate void SectionFindRequestedEventHandler(RegexEntity startEntity, RegexEntity endEntity);
}