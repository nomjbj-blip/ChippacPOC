using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmZipTest : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmZipTest()
        {
            InitializeComponent();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = txtPath.Text;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txtPath.Text = dlg.SelectedPath;
            }
        }

        private void btnMakeZip_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPath.Text))
            {
                ShowErrorMessage("저장할 경로를 설정하세요.");
                return;
            }

            if (String.IsNullOrEmpty(txtLot.Text))
            {
                ShowErrorMessage("Lot ID를 입력 하세요.");
                return;
            }

            try
            {
                DACrux.SEMDMS.RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
                
                // 만약 상태 메시지 수신 받지 않을 경우 null
                Action<string> statusMessage = SetStatusMessage;

                StatusMessage(String.Format("'{0}'에 대한 파일을 생성하고 있습니다.", txtLot.Text));

                obj.MakeZipFile(txtLot.Text, txtPath.Text, chkLastInspection.Checked, statusMessage);
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void SetStatusMessage(string message)
        {
            StatusMessage(message);
        }
    }
}
