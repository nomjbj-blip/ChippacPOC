using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCNumericUpDown : UserControl
    {
        #region [ DATA FIELD ]
        private bool bLabelView = true;
        private bool bLabelImageView = true;
        #endregion

        #region [ EVENT ]
        
        public event EventHandler ValueChangeNotifyEvent;

        #endregion
        
        #region [ PROPERTY ]

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lbl_title.Font = base.Font;
                nNumber.Font = base.Font;
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                nNumber.ForeColor = base.ForeColor;
            }
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

        #region [ NumericUpDown ]
        [Category("NumericUpDown Setup")]
        [DefaultValue(0)]
        public decimal Value
        {
            get { return nNumber.Value; }
            set { nNumber.Value = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(1)]
        public decimal Increment
        {
            get { return nNumber.Increment; }
            set { nNumber.Increment = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(100)]
        public decimal Maximum
        {
            get { return nNumber.Maximum; }
            set { nNumber.Maximum = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(0)]
        public decimal Minimum
        {
            get { return nNumber.Minimum; }
            set { nNumber.Minimum = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return nNumber.ReadOnly; }
            set { nNumber.ReadOnly = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(0)]
        public int DecimalPlaces
        {
            get { return nNumber.DecimalPlaces; }
            set { nNumber.DecimalPlaces = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(false)]
        public bool Hexadecimal
        {
            get { return nNumber.Hexadecimal; }
            set { nNumber.Hexadecimal = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(false)]
        public bool ThousandsSeparator
        {
            get { return nNumber.ThousandsSeparator; }
            set { nNumber.ThousandsSeparator = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return nNumber.TextAlign; }
            set { nNumber.TextAlign = value; }
        }

        [Category("NumericUpDown Setup")]
        [DefaultValue(LeftRightAlignment.Right)]
        public LeftRightAlignment UpDownAlign
        {
            get { return nNumber.UpDownAlign; }
            set { nNumber.UpDownAlign = value; }
        }

        #endregion

        #endregion

        #region [ METHOD ]

        #region [ Create ]

        public DUCNumericUpDown()
        {
            InitializeComponent();
        }

        #endregion

        private void nNumber_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChangeNotifyEvent != null)
            {
                ValueChangeNotifyEvent(this, e);
            }
        }


        private void DUCNumericUpDown_VisibleChanged(object sender, EventArgs e)
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
