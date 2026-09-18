using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    public partial class uclOperType_Modify : UserControl
    {
        #region " MEMBER FIELD "

        private readonly static int DEFAULT_CAPTION_WIDTH = 85;
        private Point position = new Point(0, 0);

        public event EventHandler SelectedItemChanged;

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets or Sets the width of Caption.
        /// </summary>
        [Category("Setup")]
        public int CaptionWidth
        {
            get { return lblTitle.Width; }
            set
            {
                if (value < 1)
                    lblTitle.Width = DEFAULT_CAPTION_WIDTH;
                else
                    lblTitle.Width = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return cboYieldType.SelectedIndex;
            }
            set
            {
                cboYieldType.SelectedIndex = value;
            }
        }

        public new string Text
        {
            get
            {
                return cboYieldType.Text;
            }
            set
            {
                cboYieldType.Text = value;
            }
        }


        #endregion

        #region " CREATOR "

        public uclOperType_Modify()
        {
            InitializeComponent();

            cboYieldType.SelectedIndex = 0;
        }

        #endregion

        private void cboYieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, e);
        }
    }
}
