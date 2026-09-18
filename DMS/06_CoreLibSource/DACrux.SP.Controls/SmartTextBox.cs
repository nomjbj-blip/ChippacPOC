using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : SmartTextBox<br/>
    /// Summary    : SmartTextBox Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2010-01-11<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class SmartTextBox : TextBox
    {
        #region " MEMBER FIELD "

        private decimal decimalValue = 0;
        private int iCursorPosition = 0;
        private bool isNumberOnly = false;
        private string strBefore = string.Empty;

        private readonly int EM_GETLINECOUNT = 0xBA;
        private readonly int EM_LINEINDEX = 0xBB;
        private readonly int EM_LINELENGTH = 0xC1;

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets or sets whether this textbox allows only number value.
        /// </summary>
        public bool NumberOnly
        {
            get { return isNumberOnly; }
            set { isNumberOnly = value; }
        }

        /// <summary>
        /// Gets or sets the textbox text.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (!isNumberOnly)
                    base.Text = value;
                else
                {
                    if (Decimal.TryParse(value, out decimalValue) || value == string.Empty)
                        base.Text = value;
                }
            }
        }

        public int LineCount
        {
            get
            {
                if (this.Multiline)
                {
                    Message msg = Message.Create(this.Handle, EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero);
                    base.DefWndProc(ref msg);
                    return msg.Result.ToInt32();
                }
                else
                    return (Text.Length > 0) ? 1 : 0;
            }
        }

        #endregion

        #region " CREATOR "

        /// <summary>
        /// Initialize textbox.
        /// </summary>
        public SmartTextBox()
        {
            
            InitializeComponent();
        }

        /// <summary>
        /// Initialize textbox.
        /// </summary>
        /// <param name="container">container control</param>
        public SmartTextBox(IContainer container)
        {
            InitializeComponent();
        }

        #endregion

        #region " EVENT HANDLER "

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (isNumberOnly)
            {
                if ((Keys)e.KeyChar == Keys.Back)
                    e.Handled = false;
                else if (!char.IsNumber(e.KeyChar))
                {
                    if (e.KeyChar == '.')
                    {
                        if (Text.IndexOf('.') > 0)
                            e.Handled = true;
                    }
                    else if (e.KeyChar == '-')
                    {
                        if (Text.IndexOf('-') > 0)
                            e.Handled = true;
                        else
                        {
                            if (this.SelectionStart > 0)
                                e.Handled = true;
                        }
                    }
                    else
                        e.Handled = true;
                }
                else
                {

                }
            }

            base.OnKeyPress(e);
            strBefore = Text;
            iCursorPosition = SelectionStart;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (isNumberOnly)
            {
                if (Text.Equals(string.Empty) || Text.Equals("-"))
                    base.OnTextChanged(e);
                else if (decimal.TryParse(Text, out decimalValue))
                    base.OnTextChanged(e);
                else
                {
                    Text = strBefore;
                }
            }
            else
                base.OnTextChanged(e);
        }

        #endregion

        #region " METHOD "        

        public int LineIndex(int Index)
        {
            if (this.Multiline)
            {
                Message msg = Message.Create(this.Handle, EM_LINEINDEX, (IntPtr)Index, IntPtr.Zero);
                base.DefWndProc(ref msg);
                return msg.Result.ToInt32();
            }
            else
                return (Text.Length > 0) ? 1 : 0;
        }

        public int LineLength(int Index)
        {
            if (this.Multiline)
            {
                Message msg = Message.Create(this.Handle, EM_LINELENGTH, (IntPtr)Index, IntPtr.Zero);
                base.DefWndProc(ref msg);
                return msg.Result.ToInt32();
            }
            else
                return (Text.Length > 0) ? 1 : 0;
        }

        #endregion
    }
}
