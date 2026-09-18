using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCEngStatusView : UserControl
    {
        int m_iStatus = 0;
        public DUCEngStatusView()
        {
            InitializeComponent();
        }

        public int Status
        {
            set
            {
                m_iStatus = value;
            }
            get
            {
                return m_iStatus;
            }
        }

        public void SetStatus(int iStatus)
        {
            m_iStatus = iStatus;
            lbl_Step5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbl_Step7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbl_Step8.BorderStyle = System.Windows.Forms.BorderStyle.None;


            switch (m_iStatus)
            {
                case 0:
                    break;
                case 5:
                    lbl_Step5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    break;
                case 6:
                    lbl_Step7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    break;
                case 7:
                    lbl_Step8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    break;
            }
        }
    }
}
