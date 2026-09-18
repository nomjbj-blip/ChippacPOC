using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Base
{
    public partial class DACruxCTLBasic01 : DACruxCTLBasic00
    {
        public DACruxCTLBasic01()
        {
            InitializeComponent();
        }

        public void ToStat(DataTable dt)
        {
            DACrux.Framework.Interface.iMainForm oMainForm = (DACrux.Framework.Interface.iMainForm)this.ParentForm.MdiParent;
            oMainForm.SendToStat(dt);
        }

        protected DACrux.Framework.Interface.iMainForm MainForm
        {
            get
            {
                return FindForm() as DACrux.Framework.Interface.iMainForm;
            }
        }

        public void StatusMessage(string strMessage)
        {
            if (this.ParentForm == null)
                return;

            DACrux.Framework.Interface.iMainForm oMainForm = (DACrux.Framework.Interface.iMainForm)this.ParentForm.MdiParent;
            oMainForm.SetStatusMessage(strMessage);

        }
        public interface IExportExcel
        {
            void ExportExcel();
        }

        /// <summary>
        /// 정보 메시지를 보여줍니다.
        /// </summary>
        protected void ShowMessage(string message)
        {
            MessageBox.Show(message, "DACrux", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 질문 메시지를 보여줍니다.
        /// </summary>
        protected bool ShowQuestionMessage(string message)
        {
            return MessageBox.Show(message, "DACrux", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>
        /// 에러 메시지를 보여줍니다.
        /// </summary>
        protected void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
