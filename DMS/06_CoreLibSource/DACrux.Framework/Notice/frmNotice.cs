using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework
{
    public partial class frmNotice : Form
    {
        public frmNotice()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #region [ Property ]

        public string NoticeSeq
        {
            get;
            set;
        }

        public string Title
        {
            get { return txtTitle.Text; }
            set { txtTitle.Text = value; }
        }

        public string Content
        {
            get { return txtContent.Text; }
            set { txtContent.Text = value; }
        }

        public string CreateDate
        {
            get { return lblCreateDate.Text; }
            set { lblCreateDate.Text = value; }
        }

        public bool DontShowAgain
        {
            get { return chkDontShow.Checked; }
            set { chkDontShow.Checked = value; }
        }

        #endregion [ Property ]
    }
}
