using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using DACrux.Framework.Controls.Properties;
using System.Windows.Forms.Design;

namespace DACrux.Framework.Controls
{
    //[Designer(typeof(FixedHeightDUCTextBox))]
    public partial class DUCTextBox: UserControl
    {
        public new event KeyEventHandler KeyDown;
        public new event EventHandler OnTextChanged;

        EntryModeCollection m_oEntry = EntryModeCollection.Normal;
 
        public string Label
        {
            set
            {
                lbl_title.Text = "     " + value;
            }
            get
            {
                return lbl_title.Text.Trim();
            }
        }

        public int LabelWidth
        {
            set
            {
                lbl_title.Width = value;
            }
            get
            {
                return lbl_title.Width;
            }
        }

        public override string Text
        {
            set
            {
                txtTextBox.Text = value;
            }
            get
            {
                return txtTextBox.Text;
            }
        }
        
        public bool MultiLine
        {
            set
            {
                txtTextBox.Multiline = value;
                if(value==false) this.Height = txtTextBox.Height;
            }
            get
            {
                return txtTextBox.Multiline;
            }
        }

        public Image LabelImage
        {
            set
            {
                lbl_title.Image = value;
            }
            get
            {
                return lbl_title.Image;
            }
        }

        public Font LabelFont
        {
            set
            {
                lbl_title.Font = value;
            }
            get
            {
                return lbl_title.Font;
            }
        }

        public Font EntryFont
        {
            set
            {
                txtTextBox.Font = value;
            }
            get
            {
                return txtTextBox.Font;
            }
        }

        public bool ReadOnly
        {
            set
            {
                txtTextBox.ReadOnly = value;
            }
            get
            {
                return txtTextBox.ReadOnly;
            }
        }

        public EntryModeCollection EntryMode
        {
            set
            {
                m_oEntry = value;
                SetImage();
            }
            get
            {
                return m_oEntry;
            }
        }

        public CharacterCasing CharacterCasing { get { return txtTextBox.CharacterCasing; } set { txtTextBox.CharacterCasing = value ;} }


        public DUCTextBox()
        {
            InitializeComponent();
        }

        private void SetImage()
        {
            try
            {
                switch (m_oEntry)
                {
                    case EntryModeCollection.Normal:
                        lbl_title.Image = Resources.StopHS;
                        break;
                    case EntryModeCollection.Key:
                        lbl_title.Image = Resources.Change_Password;
                        break;
                    case EntryModeCollection.Filter:
                        lbl_title.Image = Resources.Filter;
                        break;
                    case EntryModeCollection.Info:
                        lbl_title.Image = Resources.Information;
                        break;
                    case EntryModeCollection.Search:
                        lbl_title.Image = Resources.Search;
                        break;
                    case EntryModeCollection.Spot:
                        lbl_title.Image = Resources.RecordHS;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtTextBox.Multiline == false && (Keys)e.KeyChar == Keys.Enter)
            {
                this.OnKeyPress(e);
            }
        }

        private void txtTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(txtTextBox, e);
            }
        }

        private void txtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (OnTextChanged != null) OnTextChanged(sender, e);
        }
    }

    //public class FixedHeightDUCTextBox : ControlDesigner
    //{
    //    //private static string[] _propsToRemove = new string[] { "Height" };

    //    public override SelectionRules SelectionRules
    //    {
    //        get {
    //            if (((DUCTextBox)base.Control).MultiLine == true)
    //            {
    //                return SelectionRules.AllSizeable | SelectionRules.Moveable;
    //            }
    //            else
    //            {
    //                return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
    //            }
    //        }
    //    }


        
    //    //protected override void PreFilterProperties(System.Collections.IDictionary properties)
    //    //{
    //    //    base.PreFilterProperties(properties);
    //    //    foreach (string p in _propsToRemove)
    //    //        if (properties.Contains(p))
    //    //            properties.Remove(p);
    //    //}
    //}
}
