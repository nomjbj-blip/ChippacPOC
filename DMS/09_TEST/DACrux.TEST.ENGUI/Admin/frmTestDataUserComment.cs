using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmTestDataUserComment : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmTestDataUserComment()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtUserComment.Text))
            {
                ShowErrorMessage("내용을 입력하세요.");
                txtUserComment.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        public string UserComment
        {
            get { return txtUserComment.Text; }
            set { txtUserComment.Text = value; }
        }

        public string Description 
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }
    }
}
