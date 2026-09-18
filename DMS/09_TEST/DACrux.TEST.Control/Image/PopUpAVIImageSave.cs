using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Common.RO;

namespace DACrux.TEST.Control.Image
{
    public partial class PopUpAVIImageSave : Form
    {
        DACrux.Base.DieList m_ModifyDies = null;
        DACrux.Base.TestImageList m_ImageList = null;

        public PopUpAVIImageSave()
        {
            InitializeComponent();
        }

        public PopUpAVIImageSave(DACrux.Base.DieList oModifyDies, DACrux.Base.TestImageList oImageList)
        {
            InitializeComponent();

            m_ModifyDies = oModifyDies;
            m_ImageList = oImageList;
            fnDrawScopeImages();

        }

        private void fnDrawScopeImages()
        {
            if (m_ImageList == null || m_ImageList.Count <= 0)
                return;

            foreach (DACrux.Base.TestImage oDie in m_ImageList)
            {
                DACrux.TEST.Control.Image.TPUAVIImage oForm =
                    new DACrux.TEST.Control.Image.TPUAVIImage(oDie.LocalImagePath, oDie.XY.X.ToString(), oDie.XY.Y.ToString(), oDie.BinNumber.ToString());

                ControlAdd(oForm);
            }


            //if (m_ModifyDies == null || m_ModifyDies.Count <= 0)
            //    return;

            //foreach (DACrux.Base.Die oDie in m_ModifyDies)
            //{
            //    DACrux.TEST.Control.Image.TPUAVIImage oForm =
            //        new DACrux.TEST.Control.Image.TPUAVIImage(oDie.ScopeImageLocal, oDie.IndexX.ToString(), oDie.IndexY.ToString(), oDie.BinNumber.ToString());

            //    ControlAdd(oForm);
            //}
        }

        private void ControlAdd(DACrux.TEST.Control.Image.TPUAVIImage container)
        {
            if (AVIImageList.InvokeRequired)
            {
                AVIImageList.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }
                ));
            }
            else
            {
                if (container == null) return;

                container.Height = container.Height + container.Margin.Top;
                container.Width = AVIImageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                AVIImageList.Controls.Add(container);
                AVIImageList.ResumeLayout();

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int iCount = 0;

            try
            {
                if (MessageBox.Show(string.Format("변경된 사항에 대해 수정 하시겠습니까? 수정된 Die 개수 : {0}", m_ModifyDies.Count), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                if (m_ModifyDies == null || m_ModifyDies.Count <= 0)
                    return;

                //DACrux.TEST.RO.DataSelect oData = new DACrux.TEST.RO.DataSelect();
                ComConfiguration obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIImageFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return;


                using (DACrux.Utility.ServerCommunicationFtp oFTP = new Utility.ServerCommunicationFtp())
                {
                    oFTP.Server = dtConfig.Rows[0]["IP"].ToString();
                    oFTP.Port = DACrux.Base.Convert.intParse(dtConfig.Rows[0]["PORT"].ToString());
                    oFTP.UserID = dtConfig.Rows[0]["ID"].ToString();
                    oFTP.Password = dtConfig.Rows[0]["PASS"].ToString();
                    oFTP.ChmodValue = 777;

                    // 모든 Die의 업로드 폴더가 같은 경우 foreach 밖으로 뺄 수 있음.
                    // Directory 경로는 동일 하기 때문에 처음에만 만들어 준다.
                    oFTP.CreateDirectory(m_ModifyDies[0].ScopeImagePath);
                    oFTP.SetCurrentDirectory(m_ModifyDies[0].ScopeImagePath);

                    //File Upload
                    iCount = 0;
                    foreach (DACrux.Base.Die oDie in m_ModifyDies)
                    {
                        if (string.IsNullOrEmpty(oDie.ScopeImageLocal) == false)
                            oFTP.SendFile(oDie.ScopeImageLocal, oDie.ScopeImage);

                       lbStatus.Text = string.Format("FTP File Upload 중입니다...({0}/{1})", iCount, m_ModifyDies.Count);
                    }

                    iCount = 0;
                    if (m_ImageList != null && m_ImageList.Count > 0)
                    {
                        foreach (DACrux.Base.TestImage oImage in m_ImageList)
                        {
                            if (string.IsNullOrEmpty(oImage.LocalImagePath) == false)
                                oFTP.SendFile(oImage.LocalImagePath, oImage.LocalImageName);

                            lbStatus.Text = string.Format("FTP File Upload 중입니다...({0}/{1})", iCount, m_ImageList.Count);
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
