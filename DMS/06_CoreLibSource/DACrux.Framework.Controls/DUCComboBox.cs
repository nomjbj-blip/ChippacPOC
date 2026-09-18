using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Design;

namespace DACrux.Framework.Controls
{
    public partial class DUCComboBox : UserControl
    {

        #region [ Data Field ]
        private bool bLabelView = true; 
        private bool bLabelImageView = true;
        #endregion

        #region [ Event ]

        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler TextChangeNotifyEvent;
        
        #endregion
        
        #region [ PROPERTY ]

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lbl_title.Font = base.Font;
                cbData.Font = base.Font;
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                cbData.ForeColor = base.ForeColor;
            }
        }

        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set
            {
                base.RightToLeft = value;
                cbData.RightToLeft = base.RightToLeft;
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get { return cbData.Text; }
            set { cbData.Text = value; }
        }

        #region [ Label ]
        [Category("Label Setup")]
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

        [Category("Label Setup")]
        [DefaultValue(70)]
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

        [Category("Label Setup")]
        [Localizable(true)]
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

        [Category("Label Setup")]
        public Font LabelFont
        {
            get { return lbl_title.Font; }
            set { lbl_title.Font = value; }
        }

        [Category("Label Setup")]
        [DefaultValue(true)]
        public bool LabelVisible
        {
            get { return bLabelView; }
            set 
            { 
                bLabelView = value;
                //pnlLabel.Visible = bLabelView;
            }
        }

        [Category("Label Setup")]
        [DefaultValue(true)]
        public bool LabelImageVisible
        {

            get { return bLabelImageView; }
            set
            {
                bLabelImageView = value;
                //pbIcon.Visible = bLabelImageView;
            }
        }

        #endregion

        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [Category("ComboBox Setup")]
        public ComboBox.ObjectCollection Items
        {
            get { return cbData.Items; }
        }

        #region [ ComboBox ]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [Category("ComboBox Setup")]
        [DefaultValue("")]
        public object DataSource
        {
            get { return cbData.DataSource; }
            set { cbData.DataSource = value; }
        }

        [Category("ComboBox Setup")]
        [DefaultValue("")]
        public string DisplayMember
        {
            get { return cbData.DisplayMember; }
            set { cbData.DisplayMember = value; }
        }

        [Category("ComboBox Setup")]
        [DefaultValue("")]
        public string ValueMember
        {
            get { return cbData.ValueMember; }
            set { cbData.ValueMember = value; }
        }

        [Category("ComboBox Setup")]
        [DefaultValue(ComboBoxStyle.DropDown)]
        public ComboBoxStyle DropDownStyle
        {
            get { return cbData.DropDownStyle; }
            set { cbData.DropDownStyle = value; }
        }

        [Category("ComboBox Setup")]
        [DefaultValue(8)]
        public int MaxDropDownItems
        {
            get { return cbData.MaxDropDownItems; }
            set { cbData.MaxDropDownItems = value; }
        }
            
        [Browsable(false)]
        public int SelectedIndex
        {
            get { return cbData.SelectedIndex; }
            set { cbData.SelectedIndex = value; }
        }

        [Browsable(false)]
        public string SelectedText
        {
            get { return cbData.SelectedText; }
            set { cbData.SelectedText = value; }
        }

        [Category("ComboBox Setup")]
        [DefaultValue(false)]
        public bool Sorted
        {
            get { return cbData.Sorted; }
            set { cbData.Sorted = value; }
        }


        #endregion

        #endregion

        #region [ METHOD ]

        #region [ Create ]

        public DUCComboBox()
        {
            InitializeComponent();
        }

        #endregion

        private void cbData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);
            }
        }

        private void cbData_DropDown(object sender, EventArgs e)
        {
            if (DropDown != null)
            {
                DropDown(this, e);
            }
        }

        private void cbData_DropDownClosed(object sender, EventArgs e)
        {
            if (DropDownClosed != null)
            {
                DropDownClosed(this, e);
            }
        }

        private void cbData_TextChanged(object sender, EventArgs e)
        {
            if (TextChangeNotifyEvent != null)
            {
                TextChangeNotifyEvent(this, e);
            }
        }

        public new void ResetText()
        {
            cbData.ResetText();
        }

        private void DUCComboBox_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                lbl_title.Visible = bLabelView;
                //pbIcon.Visible = bLabelImageView;
            }
        }

        #endregion
    }
}
