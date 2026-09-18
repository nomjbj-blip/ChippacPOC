using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Utility;
using DACrux.Common.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmTestDataUpload_Pcm : DACrux.Framework.Base.DACruxUXBasic01
    {
        public static readonly string TESTAREA_PCM = "PARAMETRIC";
        public static readonly string TESTAREA_CP = "MULTIPROBE";

        public frmTestDataUpload_Pcm()
        {
            InitializeComponent();
        }

        private void frmTestDataUpload_Pcm_Load(object sender, EventArgs e)
        {
            DACrux.Common.RO.EquipManagement obj = new DACrux.Common.RO.EquipManagement();
            DataTable dt = obj.GetEquipIDList(GlobalVariable.Factory, Oper);

            lstEquipID.DisplayMember = lstEquipID.ValueMember = dt.Columns[0].ColumnName;
            lstEquipID.DataSource = dt;

            pnlDataInput.Enabled = (Oper == TESTAREA_PCM);

            if (FileNames != null)
                lblDescription.Text = String.Format("{0} 개의 파일이 선택되었습니다.", FileNames.Length);
        }

        private void ducOperator_Search(object sender, Framework.Controls.UserIDValidatorEventArgs e)
        {
            DACrux.Framework.RO.UserManagement obj = new Framework.RO.UserManagement();
            e.UserName = obj.GetUserName(e.UserID);
        }

        private void lstEquipID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEquipID.SelectedIndex < 0)
                txtEquipID.Clear();
            else
                txtEquipID.Text = lstEquipID.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (FileNames == null || FileNames.Length == 0)
            {
                ShowErrorMessage("업로드할 파일 정보가 없습니다.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtEquipID.Text))
            {
                ShowErrorMessage("EQUIP ID 가 선택되지 않았습니다.");
                return;
            }

            if (pnlDataInput.Enabled && String.IsNullOrWhiteSpace(txtProbeCard.Text))
            {
                ShowErrorMessage("PROBE CARD 를 입력하세요.");
                return;
            }

            //if (pnlDataInput.Enabled && String.IsNullOrWhiteSpace(txtOperator.Text))
            //{
            //    ShowErrorMessage("OPERATOR 정보를 확인하세요.");
            //    return;
            //}

            if (String.IsNullOrWhiteSpace(txtUserComment.Text))
            {
                ShowErrorMessage("내용을 입력하세요.");
                txtUserComment.Focus();
                return;
            }

            if (!ShowQuestionMessage("파일을 업로드 하시겠습니까?"))
                return;

            ComConfiguration obj = new ComConfiguration();
            var dic = obj.GetData("FTP_INFO");

            string path = String.Empty;

            if (Oper == TESTAREA_PCM)
                path = dic["PCM_WORKING_PATH"].Trim('/');
            else if (Oper == TESTAREA_CP)
                path = dic["CP_WORKING_PATH"].Trim('/');
            else
                throw new Exception(String.Format("해당 공정('{0}')에 대한 FTP 경로가 설정되지 않았습니다.", Oper));

            ServerCommunicationFtp ftp = new ServerCommunicationFtp();
            ftp.Server = dic["SERVER"];
            ftp.Port = Int32.Parse(dic["PORT"]);
            ftp.UserID = dic["USER"];
            ftp.Password = dic["PASS"];
            ftp.ChmodValue = (short)DACrux.Base.Convert.intParse(dic["CHMOD_VALUE"], 0);
            ftp.SetCurrentDirectory(String.Format("{0}/{1}", path, txtEquipID.Text));

            // 파일 업로드
            foreach (string fileName in FileNames)
            {
                //  PCM인 경우 파일명에 ProbeCard, Operator 정보를 붙인다.
                //string newFileName = DACrux.Data.Handler.HandlerPcm.GetUserUploadFileName(fileName, txtProbeCard.Text, txtOperator.Text);
                string newFileName = DACrux.Data.Handler.HandlerPcm.GetUserUploadFileName(fileName, txtProbeCard.Text, String.Empty);
                ftp.SendFile(fileName, System.IO.Path.GetFileName(newFileName));
            }

            //ftp.DeleteFile("/EQUIP/TEST/PCM/AMPT44/test.txt");

            ShowMessage("전송이 완료되었습니다. 데이터 처리까지는 최대 수 분이 소요될 수 있습니다.");
            
            DialogResult = DialogResult.OK;
        }

        public string UserComment
        {
            get { return txtUserComment.Text; }
            set { txtUserComment.Text = value; }
        }

        public string Description 
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        public string Oper
        {
            get;
            set;
        }

        public string[] FileNames
        {
            get;
            set;
        }
    }
}
