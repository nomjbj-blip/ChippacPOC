/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : frmErrorBox.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Framework::DACrux 의 통일된 Error Message Window
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace DACrux.Framework
{
    internal partial class frmErrorBox : Form
    {
        private DateTime dt;
        public frmErrorBox()
        {
            InitializeComponent();
        }

        public frmErrorBox(string Msg)
        {
            InitializeComponent();
            this.Load += new EventHandler(frmErrorBox_Load);
            txtMessage.Text = DACrux.Base.MultiLanguage.Translation(Msg);
        }

        void frmErrorBox_Load(object sender, EventArgs e)
        {
            dt = DateTime.Now;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butCopy_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtMessage.Text);
        }

        private void bntSend_Click(object sender, EventArgs e)
        {
           //DACrux.Util.ScreenCapture(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height, new Point(0, 0),dt,txtMessage.Text);
        }

     }
}
