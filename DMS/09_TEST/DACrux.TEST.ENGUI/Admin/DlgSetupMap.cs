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
    public delegate void WaferGridApply(int MinX, int MinY, int MaxX, int MaxY);
    public partial class DlgSetupMap : Form
    {
        private int m_MinX = 1;
        private int m_MinY = 1;

        private int m_MaxX = 1;
        private int m_MaxY = 1;

        public event WaferGridApply OnWaferGridApply = null;
        public DlgSetupMap(int MinX = 1, int MinY = 1, int MaxX = 1, int MaxY = 1)
        {
            InitializeComponent();

            m_MinX = MinX;
            m_MinY = MinY;
            m_MaxX = MaxX;
            m_MaxY = MaxY;

            txtMinX.Text = m_MinX.ToString();
            txtMinY.Text = m_MinY.ToString();
            txtMaxX.Text = m_MaxX.ToString();
            txtMaxY.Text = m_MaxY.ToString();


            int XCnt = MaxX - MinX + 1;
            int YCnt = MaxY - MinY + 1;

            if (XCnt < 0) txtXCnt.Text = "Err";
            else txtXCnt.Text = XCnt.ToString();

            if (YCnt < 0) txtYCnt.Text = "Err";
            else txtYCnt.Text = YCnt.ToString();
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) ||
                char.IsSymbol(e.KeyChar) ||
                char.IsWhiteSpace(e.KeyChar) ||
                char.IsPunctuation(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == '.')
            {
                // Integer 였을때는 . 도 허용 안함
                e.Handled = true;
            }
            
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length == 0 )
            {
                // 첫번째 글자에 '-' 허용
                e.Handled = false;
            }
        }

        private void butApply_Click(object sender, EventArgs e)
        {
            if (OnWaferGridApply != null) OnWaferGridApply( DACrux.Base.Convert.intParse(txtMinX.Text)
                                                           , DACrux.Base.Convert.intParse(txtMinY.Text)
                                                           , DACrux.Base.Convert.intParse(txtMaxX.Text)
                                                           , DACrux.Base.Convert.intParse(txtMaxY.Text));
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (OnWaferGridApply != null) OnWaferGridApply(DACrux.Base.Convert.intParse(txtMinX.Text)
                                               , DACrux.Base.Convert.intParse(txtMinY.Text)
                                               , DACrux.Base.Convert.intParse(txtMaxX.Text)
                                               , DACrux.Base.Convert.intParse(txtMaxY.Text));

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void txtBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                int MinX = 0;
                int MinY = 0;
                int MaxX = 0;
                int MaxY = 0;

                int.TryParse(txtMinX.Text, out MinX);
                int.TryParse(txtMinY.Text, out MinY);
                int.TryParse(txtMaxX.Text, out MaxX);
                int.TryParse(txtMaxY.Text, out MaxY);

                int XCnt = MaxX - MinX + 1;
                int YCnt = MaxY - MinY + 1;

                if (XCnt < 0) txtXCnt.Text = "Err";
                else txtXCnt.Text = XCnt.ToString();

                if (YCnt < 0) txtYCnt.Text = "Err";
                else txtYCnt.Text = YCnt.ToString();
            }
            catch
            {
                // 무시
            }
        }
    }
}
