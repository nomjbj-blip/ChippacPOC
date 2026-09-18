using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Framework.Base
{
    public partial class DACruxUXBasic01 : DACruxUXBasic00
    {
        private DACrux.Base.DPWafer[] m_DPWaferList = null;
        private DACrux.Base.TPWafer[] m_TPWaferList = null;

        //------------------------------------------------------------------------------

        public DACruxUXBasic01(
            )
        {
            InitializeComponent();
            FUNC_CODE = string.Format("FUN:{0}", this.Text).Substring(0, 10);
        }

        //------------------------------------------------------------------------------

        #region [ Event Handler ]

        private void DACruxUXBasic01_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            DACrux.Framework.RO.UserHitLog.StartFunction(
                FUNC_CODE,
                DACrux.Base.GlobalVariable.UserID,
                DACrux.Base.IPUtil.GetLocalIPAddress(),
                String.Empty
                );

            if (UseFullInformation)
            {
                // Debug 모드 일때 화면의 이름 앞에 ASSEMBLY 정보 등록
                Text = string.Format("[{0}:{1}]{2}", FUNC_CODE, this.GetType(), this.Text);
            }
        }

        public void StatusMessage(string strMessage)
        {
            if (this.ParentForm == null)
                return;

            DACrux.Framework.Interface.iMainForm oMainForm = (DACrux.Framework.Interface.iMainForm)this.MdiParent;
            oMainForm.SetStatusMessage(strMessage);
        }

        //--

        private void DACruxUXBasic01_FormClosing(
            object sender,
            FormClosingEventArgs e
            )
        {
            DACrux.Framework.RO.UserHitLog.EndFunction(
                FUNC_CODE,
                DACrux.Base.GlobalVariable.UserID,
                DACrux.Base.IPUtil.GetLocalIPAddress()
                );
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_parameter != null)
                LoadParameter(_parameter);

            _parameter = null;
        }

        #endregion [ Event Handler ]

        //------------------------------------------------------------------------------

        #region [ Method ]

        //public void SendToStat(DataTable dt)
        //public void SendToStat(DataTable dt, string workSheetName)
        public void SetMainStatusBarMsg(
            string strMessage
            )
        {
            DACrux.Framework.Interface.iMainForm oMain = (DACrux.Framework.Interface.iMainForm)this.MdiParent;
            oMain.SetMainStatusBarMsg(strMessage);
        }

        //--

        public void SetMainStatusBarProgress(
            int iValue,
            int iMaxValue = 100
            )
        {
            if (iValue < iMaxValue)
                this.Cursor = Cursors.WaitCursor;
            else
                this.Cursor = Cursors.Default;
            DACrux.Framework.Interface.iMainForm oMain = (DACrux.Framework.Interface.iMainForm)this.MdiParent;
            oMain.SetMainStatusBarProgress(iValue, iMaxValue);
        }

        //--

        public void SendToStat(
            DataTable dt,
            string workSheetName
            )
        {
            DACrux.Framework.Interface.iMainForm IStat = (DACrux.Framework.Interface.iMainForm)this.MdiParent;
            if (IStat != null)
            {
                IStat.SendToStat(
                    dt,
                    workSheetName
                    );
            }
            else
            {
                IStat = (DACrux.Framework.Interface.iMainForm)this.ParentForm.ParentForm.MdiParent;
                if (IStat != null)
                    IStat.SendToStat(dt);
            }
        }

        #endregion [ Method ]

        //------------------------------------------------------------------------------

        #region [ Properties ]

        protected DACrux.Framework.Interface.iMainForm MainForm
        {
            get { return MdiParent as DACrux.Framework.Interface.iMainForm; }
        }

        public string FUNC_CODE
        {
            get;
            set;
        }

        //--

        public string RESV_01 { get; set; }
        public string RESV_02 { get; set; }
        public string RESV_03 { get; set; }
        public string RESV_04 { get; set; }
        public string RESV_05 { get; set; }

        //--

        public string[] WaferList
        {
            get;
            set;
        }

        public DACrux.Base.DPWafer[] DPWaferList
        {
            get
            {
                return m_DPWaferList;
            }
            set
            {
                string[] strSeq = new string[value.Length];
                for (int iseq = 0; iseq < value.Length; iseq++)
                {
                    strSeq[iseq] = value[iseq].StepSeq;
                }

                WaferList = strSeq;
                m_DPWaferList = value;
            }
        }

        public DACrux.Base.TPWafer[] TPWaferList
        {
            get
            {
                return m_TPWaferList;
            }
            set
            {
                string[] strSeq = new string[value.Length];
                for (int iseq = 0; iseq < value.Length; iseq++)
                {
                    strSeq[iseq] = value[iseq].WaferSeq;
                }

                WaferList = strSeq;
                m_TPWaferList = value;
            }
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
        protected bool ShowQuestionMessage(string message, MessageBoxDefaultButton defaultButon = MessageBoxDefaultButton.Button2)
        {
            return MessageBox.Show(message, "DACrux", MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultButon) == DialogResult.Yes;
        }

        /// <summary>
        /// 에러 메시지를 보여줍니다.
        /// </summary>
        protected void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 비동기 Thread에서 발생한 Exception을 UI Thread로 전달합니다.
        /// </summary>
        protected void ThrowExceptionAsync(Exception ex)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<Exception>(ThrowExceptionAsync), ex);
            else
                throw ex;
        }

        #endregion [ Properties ]

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show(string.Format("현재 화면을 닫으시겠습니까?\n{0}", this.Text), "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Close();
            }
            else
            {
                base.OnKeyUp(e);
            }
        }

        public static bool UseFullInformation
        {
            get;
            set;
        }

        private object _parameter;

        public void SetParameter(object parameter)
        {
            _parameter = parameter;
        }

        public virtual void LoadParameter(object parameter)
        {
        }

        public void ShowForm(string menuKey, object parameter)
        {
            DACrux.Framework.Interface.iMainForm mainForm = this.MdiParent as DACrux.Framework.Interface.iMainForm;

            if (mainForm != null)
            {
                mainForm.ShowForm(menuKey, parameter);
            }
            else
            {
                mainForm = this.ParentForm.ParentForm.MdiParent as DACrux.Framework.Interface.iMainForm;

                if (mainForm != null)
                    mainForm.ShowForm(menuKey, parameter);
            }
        }
    }

    public interface IExportExcel
    {
        void ExportExcel();
    }
}
