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
    public delegate void SetOrigin(double OriginX, double OriginY);

    public partial class DlgSetOrigin : Form
    {
        public event SetOrigin OnSetOrigin = null;
        private int m_iMoveButtonIdx = 0;

        private double m_dGage = 0.001d;
        private double m_OldOriginX = 0;
        private double m_OldOriginY = 0;

        public DlgSetOrigin(double OriginX, double OriginY)
        {
            InitializeComponent();
            m_OldOriginX = OriginX;
            m_OldOriginY = OriginY;

            txtOriginX.Text = m_OldOriginX.ToString();
            txtOriginY.Text = m_OldOriginY.ToString();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (OnSetOrigin != null) OnSetOrigin(m_OldOriginX, m_OldOriginY);

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            try
            {
                double.TryParse(txtOriginX.Text, out m_OldOriginX);
                double.TryParse(txtOriginY.Text, out m_OldOriginY);

                if (OnSetOrigin != null) OnSetOrigin(m_OldOriginX, m_OldOriginY);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butApply_Click(object sender, EventArgs e)
        {
            try
            {
                double.TryParse(txtOriginX.Text, out m_OldOriginX);
                double.TryParse(txtOriginY.Text, out m_OldOriginY);

                if (OnSetOrigin != null) OnSetOrigin(m_OldOriginX, m_OldOriginY);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void tmrReclick_Tick(object sender, System.EventArgs e)
        {
            double orgX = m_OldOriginX;
            double orgY = m_OldOriginY;
            try
            {
                double.TryParse(txtOriginX.Text, out orgX);
                double.TryParse(txtOriginY.Text, out orgY);

                m_dGage = Math.Round(m_dGage + (m_dGage * 0.3),5);
                switch (m_iMoveButtonIdx)
                {
                    case 0:
                        orgY = orgY + m_dGage;
                        break;
                    case 1:
                        orgX = orgX + m_dGage;
                        break;
                    case 2:
                        orgY -= m_dGage;
                        break;
                    case 3:
                        orgX -= m_dGage;
                        break;
                }
                txtOriginX.Text = orgX.ToString();
                txtOriginY.Text = orgY.ToString();

                if (chkPreview.Checked)
                {
                    if (OnSetOrigin != null) OnSetOrigin(orgX, orgY);
                }
            }
            catch
            {
                ///무시
            }
        }

        private void ScrollBut_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Button but = (Button)sender;
            m_iMoveButtonIdx = DACrux.Base.Convert.intParse(but.Tag.ToString());
            tmrReclick.Enabled = true;
        }

        private void ScrollBut_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            tmrReclick.Enabled = false;
            m_dGage = 0.001d;

        }

    }
}
