using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    [DefaultEvent("Search")]
    public partial class DUCUserIDValidator : UserControl
    {
        public static readonly int FIXED_HEIGHT = 21;

        public event EventHandler<UserIDValidatorEventArgs> Search;

        public DUCUserIDValidator()
        {
            InitializeComponent();

            IDFieldWidth = 100;
            UserID = null;
        }

        protected virtual void OnSearch(UserIDValidatorEventArgs e)
        {
            if (Search != null)
                Search(this, e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, FIXED_HEIGHT, specified);
        }

        private void CheckValidate()
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("ID를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtID.Focus();
                return;
            }

            UserIDValidatorEventArgs args = new UserIDValidatorEventArgs(txtID.Text);

            OnSearch(args);

            txtName.Text = args.UserName;
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CheckValidate();
        }

        [DefaultValue(100)]
        public int IDFieldWidth
        {
            get { return txtID.Width; }
            set { txtID.Width = value; }
        }

        [DefaultValue(null)]
        public string UserID
        {
            get { return txtID.Text; }
            set { txtID.Text = value; }
        }

        public string UserName
        {
            get { return txtName.Text; }
        }

        public bool IsValid
        {
            get { return !String.IsNullOrEmpty(txtName.Text); }
        }
    }

    public class UserIDValidatorEventArgs : EventArgs
    {
        public UserIDValidatorEventArgs(string userID)
        {
            UserID = userID;    
        }

        /// <summary>
        /// UserID를 나타냅니다.
        /// </summary>
        public string UserID { get; private set; }
        /// <summary>
        /// UserID에 대한 사용자 이름을 설정하거나 가져옵니다.
        /// </summary>
        public string UserName { get; set; }
    }
}
