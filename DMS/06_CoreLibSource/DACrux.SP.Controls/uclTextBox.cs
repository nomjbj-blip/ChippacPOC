using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    [DefaultEvent("TextChanged")]
    public partial class uclTextBox : UserControl
    {
        #region " MEMBER FIELD "

        private readonly static int DEFAULT_CAPTION_WIDTH = 88;

        #endregion

        #region " PROPERTY "

        [Category("Setup")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color TextColor
        {
            get { return txtContents.ForeColor; }
            set { txtContents.ForeColor = value; }
        }

        [Category("Setup")]
        public bool ShowImage
        {
            get { return lblTitle.ShowImage; }
            set { lblTitle.ShowImage = value; }
        }

        [Category("Setup")]
        public bool ShowCaption
        {
            get { return lblTitle.Visible; }
            set
            {
                lblTitle.Visible = value;

                if (value)
                    txtContents.Dock = DockStyle.None;
                else
                    txtContents.Dock = DockStyle.Fill;
            }
        }

        [Category("Setup")]
        public string Caption
        {
            get { return lblTitle.Text.TrimStart(' '); }
            set
            {
                lblTitle.Text = value;


                //lblTitle.ShowImage = lblTitle.ShowImage; 
            }
        }

        [Category("Setup")]
        public bool ReadOnly
        {
            get { return txtContents.ReadOnly; }
            set { txtContents.ReadOnly = value; }
        }

        [Category("Setup")]
        public int CaptionWidth
        {
            get { return lblTitle.Width; }
            set
            {
                int iRight = txtContents.Right;

                if (value < 1)
                    lblTitle.Width = DEFAULT_CAPTION_WIDTH;
                else
                    lblTitle.Width = value;

                txtContents.Left = lblTitle.Width;
                txtContents.Width = iRight - txtContents.Left;
            }
        }

        [Category("Setup")]
        public bool NumberValueOnly
        {
            get { return txtContents.NumberOnly; }
            set
            {
                txtContents.NumberOnly = value;

                if (value)
                    txtContents.TextAlign = HorizontalAlignment.Right;
                else
                    txtContents.TextAlign = HorizontalAlignment.Left;
            }
        }

        [Category("Setup")]
        public int MaxLength
        {
            get { return txtContents.MaxLength; }
            set { txtContents.MaxLength = value; }
        }

        [Category("Setup")]
        [Browsable(true)]
        public override string Text
        {
            get { return txtContents.Text; }
            set
            {
                txtContents.Text = value;

                //if(!bNumberValueOnly)
                //    txtContents.Text = value; 
                //else
                //{
                //    decimal d = 0;

                //    if(Decimal.TryParse(value, out d) || value == string.Empty)
                //        txtContents.Text = value; 
                //}
            }
        }

        [Category("Setup")]
        [Browsable(true)]
        public HorizontalAlignment TextAlign
        {
            get { return txtContents.TextAlign; }
            set { txtContents.TextAlign = value; }
        }

        [Category("Setup")]
        [Browsable(true)]
        public bool Multiline
        {
            get { return txtContents.Multiline; }
            set { txtContents.Multiline = value; }
        }

        [Category("Setup")]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return txtContents.BorderStyle; }
            set { txtContents.BorderStyle = value; }
        }

        [Category("Setup")]
        public bool HighLight
        {
            get { return lblTitle.HighLight; }
            set { lblTitle.HighLight = value; }
        }

        #endregion

        #region " EVENT HANDLER "

        [Category("Setup"), Browsable(true)]
        public new event EventHandler TextChanged
        {
            add
            {
                txtContents.TextChanged += value;
            }
            remove
            {
                txtContents.TextChanged -= value;
            }
        }

        #endregion

        #region " CREATOR "

        public uclTextBox()
        {
            InitializeComponent();

            //txtContents.KeyPress += new KeyPressEventHandler(txtContents_KeyPress);
        }

        #endregion

        #region " EVENT HANDLER "

        void txtContents_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                if (this.Parent.GetNextControl(this, true) != null)
                    this.Parent.GetNextControl(this, true).Focus();

                e.Handled = true;
            }
        }

        #endregion

        #region " METHOD "

        public void SelectAll()
        {
            txtContents.SelectAll();
        }

        #endregion
    }
}
