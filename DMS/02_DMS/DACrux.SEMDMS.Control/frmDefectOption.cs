using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    public partial class frmDefectOption : Form
    {
        public frmDefectOption(
            )
        {
            InitializeComponent();
        }

        public frmDefectOption(
            double sizeMax,
            double sizeMin,
            bool sizeDefectMode,
            int divisionCnt,
            int divisionMaxCnt
            )
            : this()
        {
            nudDivisionCnt.Maximum = divisionMaxCnt;

            //--

            DefectSizeMax = sizeMax;
            DefectSizeMin = sizeMin;
            DivisionCount = divisionCnt;
            DefectSizeMode = sizeDefectMode;
        }

        //-----------------------------------------------------------------------------

        #region [ Event Handler ]

        //--

        //private void btnClose_Click(
        //    object sender,
        //    EventArgs e
        //    )
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.Close();
        //}

        //--

        //private void btnOk_Click(
        //    object sender,
        //    EventArgs e
        //    )
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    this.Close();
        //}

        //--

        private void rbSizeMode_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            RadioButton rb = sender as RadioButton;
            if (rb == rbAutomatic)
            {
                txtMaxinum.Enabled = false;
                txtMininum.Enabled = false;
            }
            else
            {
                txtMaxinum.Enabled = true;
                txtMininum.Enabled = true;
            }
        }

        //--

        private void txtAllEvt_KeyPress(
            object sender, 
            KeyPressEventArgs e
            )
        {
            if (!(char.IsDigit(e.KeyChar) 
                || e.KeyChar == Convert.ToChar(Keys.Back) 
                || e.KeyChar == 46)) // '.' 입력
                e.Handled = true;
        }

        //--

        #endregion [ Event Handler ]

        //-----------------------------------------------------------------------------

        #region [ Properties ]

        //--

        /// <summary>
        /// true: Automatic
        /// false: Manual
        /// </summary>
        public bool DefectSizeMode
        {
            get
            {
                return (rbAutomatic.Checked ? true : (rbManual.Checked ? false : false));
            }

            private set
            {
                if (value)
                {
                    rbAutomatic.Checked = true;
                    rbManual.Checked = false;
                }
                else
                {
                    rbAutomatic.Checked = false;
                    rbManual.Checked = true;
                }
            }
        }

        //--

        public double DefectSizeMin
        {
            get
            {
                return DACrux.Base.Convert.doubleParse(txtMininum.Text);
            }
            private set
            {
                txtMininum.Text = value.ToString();
            }
        }

        //--

        public double DefectSizeMax
        {
            get
            {
                return DACrux.Base.Convert.doubleParse(txtMaxinum.Text);
            }
            private set
            {
                txtMaxinum.Text = value.ToString();
            }
        }

        //--

        public int DivisionCount
        {
            get
            {
                return (int)nudDivisionCnt.Value;
            }

            private set
            {
                nudDivisionCnt.Value = value;
            }
        }

        //--

        #endregion [ Properties ]

        //-----------------------------------------------------------------------------
    }
}
