using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Controls
{
    public partial class DUCAlarmStatusView : UserControl
    {
        int m_iStatus = 0;
        public DUCAlarmStatusView()
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
            lbl_Step1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lbl_Step2.BorderStyle = System.Windows.Forms.BorderStyle.None;


            switch (m_iStatus)
            {
                case 0:
                    break;
                case 1:
                    lbl_Step1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    break;
                case 2:
                    lbl_Step2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    break;
            }
        }
    }
}
