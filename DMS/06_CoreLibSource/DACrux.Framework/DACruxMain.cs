/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : DACruxMain.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux V5 Main Frame
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 ----------------------------------------------------------------------------------------------------------*/

using System;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Framework.RO;
using DACrux.ProjectManager;
using System.Collections.Generic;


namespace DACrux.Framework
{
    /// <summary>
    /// DACrux Defualt Mainform
    /// </summary>
    public partial class DACruxMain
        : Form, DACrux.Framework.Interface.iMainForm, DACrux.Framework.Interface.iLotGroupContainer//, DACrux.Framework.Interface.iDMSearch, DACrux.Framework.Interface.iTESTSearch
    {
        // DB하이텍용. 화면간 Defect 데이터 링크 활용을 위해
        public static readonly string DEFECT_LINK_ITEM = "MNU_DMS_LINK";
        public static readonly string DATA_EXPORT = "MNU_EXPORT";

        // Search control 활성화 값
        private readonly string DM = "DM";
        private readonly string TEST = "TEST";
        private readonly string NOTICESEQ = "NOTICE_SEQ";
        private bool bUpdateChecker = false;
        private System.Threading.Timer updateTimer = null;
        private int dueTime = 1000; // 1시간 뒤에 실행
        private int period = 10 * 60 * 1000; // 10분 주기로 체크

        DataTable m_dtMenu = null;
        Label lblStatus;

        public DACruxMain(
            string formText = "DACrux Framework - Default MainForm",
            System.Drawing.Icon icoMain = null
            )
        {
            DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Abort; // 별도 지정없이 종료될시 Abort Mode임

            InitializeComponent();
            CreateStatusLabel();

            m_dtMenu = GetMenu();
            MenuHelper.InitMenu(ref utbManager, m_dtMenu);

            this.Text = formText;
            this.utbManager.Ribbon.Visible = DACrux.Base.GlobalVariable.RibbonMenuEnable;

            if (icoMain != null)
                this.Icon = icoMain;
        }

        #region [ Event Handler ]
        private void frmDCMain_Load(
            object sender,
            EventArgs e
            )
        {
            //UserManagement oUser = new UserManagement();
            //if (oUser.GetLicense(DACrux.Base.GlobalVariable.LocalMacAddress) == false)
            //{
            //    MessageBox.Show("License is expire.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Exit;
            //    this.Close();
            //    return;
            //}

            ////clickOnce 형태일 경우 Version 을 상단에 뿌려 준다.
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed == true)
            {
                this.Text = string.Format("DACrux Ver : {0}", System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion);
            }

            utsMain.Panels[3].Text = string.Format("Factory : {0}", DACrux.Base.GlobalVariable.Factory);
            utsMain.Panels[4].Text = string.Format("User ID : {0}", DACrux.Base.GlobalVariable.UserID);

            Application.DoEvents();
#if !DEBUG
            updateTimer = new System.Threading.Timer(new TimerCallback(CheckedClientVersion), null, dueTime, period);
#endif
            ShortcutLoader.Load(utbManager);
        }

        private void DACruxMain_FormClosing(
            object sender,
            FormClosingEventArgs e
            )
        {
            if (!bUpdateChecker && DACrux.Base.GlobalVariable.ExitMode == DACrux.Base.EXITMODE.Abort)
            {
                DialogResult dr = MessageBox.Show("Do you want to exit?(Yes:Exit,No:Logout)", "Quit DACrux !", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Exit;
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                {
                    DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Logout;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                DACrux.Base.GlobalVariable.ExitMode = EXITMODE.Restart;
            }

            // shortcut 저장
            ShortcutLoader.Save(utbManager);
        }


        /// <summary>
        /// Main form 상의 화면에 따라 Search Control 을 활성화 한다.
        /// </summary>
        private void DACruxMain_MdiChildActivate(
            object sender,
            EventArgs e
            )
        {
            Assembly assemblyMap = null;
            Type type = null;
            System.Windows.Forms.UserControl oUserControl = null;

            //Active 된 Form 기반 Assembly정보를 가져 온다.
            if (((Form)sender).ActiveMdiChild == null)
                return;

            string strAssembly = ((Form)sender).ActiveMdiChild.GetType().FullName;
            if (m_dtMenu != null)
            {
                DataRow[] dr = m_dtMenu.Select(string.Format("ASSEMBLY = '{0}'", strAssembly));
                if (dr.Length > 0)
                {
                    if (String.Equals(dr[0]["RESV_04"].ToString(), DM))
                    {
                        udmMain.ControlPanes[TEST].Closed = true;
                        udmMain.ControlPanes[DM].Closed = false;

                        assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, "DACrux.SEMDMS.Control.dll"));
                        type = assemblyMap.GetType("DACrux.SEMDMS.Control.DPUCStepSelect");

                        if (plDMSearch.Controls == null || plDMSearch.Controls.Count == 0)
                        {
                            oUserControl = (System.Windows.Forms.UserControl)Activator.CreateInstance(type);
                            plDMSearch.Controls.Add(oUserControl);
                            oUserControl.Dock = DockStyle.Fill;
                            udmMain.ControlPanes[DM].Pinned = true;
                        }
                    }
                    else if (String.Equals(dr[0]["RESV_04"].ToString(), TEST))
                    {
                        udmMain.ControlPanes[TEST].Closed = false;
                        udmMain.ControlPanes[DM].Closed = true;

                        assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, "DACrux.TEST.ENGUI.dll"));
                        type = assemblyMap.GetType("DACrux.TEST.ENGUI.TPUCSelectWafer");

                        if (plTESTSearch.Controls == null || plTESTSearch.Controls.Count == 0)
                        {
                            oUserControl = (System.Windows.Forms.UserControl)Activator.CreateInstance(type);
                            plTESTSearch.Controls.Add(oUserControl);
                            oUserControl.Dock = DockStyle.Fill;

                            udmMain.ControlPanes[TEST].Pinned = true;
                        }
                    }
                    else
                    {
                        udmMain.ControlPanes[TEST].Closed = true;
                        udmMain.ControlPanes[DM].Closed = true;
                    }

                }
            }
        }

        public void ShowForm(string menuKey, object parameter = null)
        {
            if (m_dtMenu == null || m_dtMenu.Rows.Count == 0)
                return;

            DataRow[] dr = m_dtMenu.Select(string.Format("MENU_KEY='{0}'", menuKey));

            if (dr != null && dr.Length > 0)
            {
                Assembly assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, dr[0]["MODULE"].ToString()));
                Type type = assemblyMap.GetType(dr[0]["ASSEMBLY"].ToString());
                Form frm = Activator.CreateInstance(type) as Form;
                frm.MdiParent = this;

                if (frm is DACrux.Framework.Base.DACruxUXBasic01)
                    (frm as DACrux.Framework.Base.DACruxUXBasic01).SetParameter(parameter);

                frm.Text = dr[0]["CAPTION001"].ToString();
                frm.Show();
            }
        }

        /// <summary>
        /// Menu Click 시 처리 Process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void utbManager_ToolClick(
            object sender,
            Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e
            )
        {
            SetMainStatusBarProgress(10);                                              /// 10%. Progress Bar
            string strStatMenu = string.Empty;

            Assembly assemblyMap = null;
            Type type = null;
            string dllPath = string.Empty;
            string startUpDllPath = string.Empty;

            try
            {
                DataRow[] dr = m_dtMenu.Select(string.Format("MENU_KEY='{0}'", e.Tool.Key));
                SetMainStatusBarProgress(20);                                          /// 20%. Progress Bar

                if (dr == null || dr.Length == 0)
                {
                    // Add Assembly에서 등록하지 않은 메뉴 처리
                    switch (e.Tool.Key)
                    {
                        default:
                            break;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(dr[0]["FUNCTION_CODE"].ToString())
                        && !DACrux.Framework.UXUtil.AccessCheck(dr[0]["FUNCTION_CODE"].ToString(), true))
                        return;

                    /// DACrux/STAT Menu Click
                    if (dr[0]["FORM_TYPE"].ToString() == "CORE_FUNCTION")
                    {
                        SetMainStatusBarProgress(40); // Progress Bar
                        strStatMenu = dr[0]["RESV_01"].ToString();
                        MD_STAT_ProjectManager.ClickedMenu = (MenuType)Enum.Parse(typeof(MenuType), strStatMenu);
                        return;
                    }
                    else if (dr[0]["FORM_TYPE"].ToString() == "MODAL_FORM")
                    {
                        SetMainStatusBarProgress(40); // Progress Bar

                        assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, dr[0]["MODULE"].ToString()));
                        type = assemblyMap.GetType(dr[0]["ASSEMBLY"].ToString());

                        DACrux.Framework.Base.DACruxUXBasic00 oForm = (DACrux.Framework.Base.DACruxUXBasic00)Activator.CreateInstance(type);
                        ((Form)oForm).StartPosition = FormStartPosition.CenterParent;
                        ((Form)oForm).ShowDialog();
                    }
                    else if (dr[0]["FORM_TYPE"].ToString() == "BASIC_FORM" && dr[0]["MODULE"].ToString().Trim().Length > 0 && dr[0]["ASSEMBLY"].ToString().Trim().Length > 0)
                    {
                        SetMainStatusBarProgress(40); // Progress Bar

                        assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, dr[0]["MODULE"].ToString()));
                        type = assemblyMap.GetType(dr[0]["ASSEMBLY"].ToString());

                        // DB하이텍의 Defect 링크 기능 때문에 STOCK_TOOLBAR = DEFECT_LINK 인 경우 
                        // Defect 배열을 전달 할 수 있도록 한다. 2019.06.21 Taihi,Kim.
                        if ((e.Tool.OwnerIsMenu && e.Tool.OwningMenu.Key == DEFECT_LINK_ITEM) ||
                            (e.Tool.OwnerIsToolbar && e.Tool.OwningToolbar.Key == DEFECT_LINK_ITEM))
                        {
                            #region [ DMS Link UI]
                            DACrux.Framework.Base.ISendDefect child = ActiveMdiChild as DACrux.Framework.Base.ISendDefect;

                            if (child == null)
                                return;

                            DACrux.Base.Defect[] defectArr = child.GetSelectedDefect();
                            DACrux.Framework.Base.DACruxUXBasic01 oForm;

                            if (!string.IsNullOrEmpty(dr[0]["RESV_01"].ToString().Trim()))
                                oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type, dr[0]["RESV_01"].ToString());
                            else
                                oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type);

                            if (oForm == null || defectArr == null)
                                return;

                            if (!(oForm is DACrux.Framework.Base.DACruxUXBasicDefectLink))
                                throw new Exception(String.Format("STOCK_TOOLBAR={0} 인 항목은 DACrux.Framework.Base.DACruxUXBasicDefectLink 클래스를 상속 받아야 합니다.", DEFECT_LINK_ITEM));

                            (oForm as DACrux.Framework.Base.DACruxUXBasicDefectLink).DefectList.AddRange(defectArr);

                            oForm.MdiParent = this;
                            oForm.Dock = DockStyle.Fill;
                            oForm.Show();
                            #endregion [ DMS Link UI]
                        }
                        // Excel Export 기능 2019.08.07 Taihi,Kim.
                        else if ((e.Tool.OwnerIsMenu && e.Tool.OwningMenu.Key == DATA_EXPORT) ||
                            (e.Tool.OwnerIsToolbar && e.Tool.OwningToolbar.Key == DATA_EXPORT))
                        {
                            DACrux.Framework.Base.IExportExcel child = ActiveMdiChild as DACrux.Framework.Base.IExportExcel;

                            if (child == null)
                                return;

                            child.ExportExcel();
                        }
                        else // 일반적인 경우
                        {
                            /// Multi로 Open되는지 확인후 기존 열린 화면을 찾아 Focus만 준다.
                            /////////////////////////////////////////////////////////////////////////////
                            if (String.Equals(dr[0]["MULTI"].ToString().ToUpper(), "FALSE"))
                            {
                                for (int i = 0; i < this.MdiChildren.Length; i++)
                                {
                                    if (String.Equals(this.MdiChildren[i].GetType().FullName, type.FullName))
                                    {
                                        this.MdiChildren[i].Focus();
                                        return;
                                    }
                                }
                            }

                            DACrux.Framework.Base.DACruxUXBasic01 oForm = null;
                            if (!string.IsNullOrEmpty(dr[0]["RESV_01"].ToString().Trim()))
                                oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type, dr[0]["RESV_01"].ToString());
                            else
                                oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type);

                            switch (DACrux.Base.GlobalVariable.Language)
                            {
                                case "English":
                                    oForm.Text = dr[0]["CAPTION001"].ToString();
                                    break;

                                case "Chinese":
                                    oForm.Text = dr[0]["CAPTION003"].ToString();
                                    break;

                                case "Korean":
                                default:
                                    oForm.Text = dr[0]["CAPTION002"].ToString();
                                    break;

                            }
                            //oForm.FUNC_CODE = dr[0]["RESV_03"].ToString();
                            oForm.FUNC_CODE = dr[0]["FUNCTION_CODE"].ToString();
                            oForm.RESV_02 = dr[0]["RESV_02"].ToString();
                            oForm.RESV_03 = dr[0]["RESV_03"].ToString();
                            oForm.MdiParent = this;
                            oForm.Dock = DockStyle.Fill;
                            oForm.Show();
                        }

                    }
                    else
                    {
                        SetMainStatusBarProgress(40); // Progress Bar
                        // 다국어 지원을 위해서 Add Assembly에 등록 후 처리
                        switch (e.Tool.Key)
                        {
                            case "MNU_WINDOWS_TAB_VIEW":
                                Infragistics.Win.UltraWinToolbars.StateButtonTool but = (Infragistics.Win.UltraWinToolbars.StateButtonTool)utbManager.Tools["MNU_WINDOWS_TAB_VIEW"];
                                utMDIMain.Enabled = but.Checked;
                                break;
                            case "MNU_HELP_MANUAL":
                                if (string.IsNullOrEmpty(DACrux.Base.GlobalVariable.DACruxManual) == false)
                                    System.Diagnostics.Process.Start(string.Format("http://{0}/{1}", DACrux.Base.GlobalVariable.ServerIP, DACrux.Base.GlobalVariable.DACruxManual));
                                break;
                            case "MNU_FILE_EXIT":
                                DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Exit;
                                this.Close();
                                break;
                            case "MNU_FILE_LOGOUT":
                                DACrux.Base.GlobalVariable.ExitMode = DACrux.Base.EXITMODE.Logout;
                                this.Close();
                                break;
                        }
                    }
                }
            }
            finally
            {
                SetMainStatusBarProgress(100); // Progress Bar
            }
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        /// <summary>
        /// ClickOnce Client version 체크
        /// 신규 버전이 존재하는 경우에 Update 되도록 구성됨
        /// </summary>
        /// <param name="state"></param>

        private frmUpdate _update = new frmUpdate();

        private void CheckedClientVersion(object state)
        {
            ApplicationDeployment appDepl = null;
            UpdateCheckInfo updateCheckInfo = null;

            try
            {
                if (!ApplicationDeployment.IsNetworkDeployed)
                    return;

                appDepl = ApplicationDeployment.CurrentDeployment;
                updateCheckInfo = appDepl.CheckForDetailedUpdate();

                if (updateCheckInfo.UpdateAvailable)
                {
                    InvokeUpdateDialog();
                    ///// Update Timer 중지
                    //updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    //using (frmUpdate dlg = new frmUpdate())
                    //{
                    //    dlg.StartPosition = FormStartPosition.Manual;
                    //    dlg.Location = new Point(0, 0);
                    //    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    //    {
                    //        /// Update Timer 가동
                    //        updateTimer.Change(dueTime, period);
                    //        return;
                    //    }

                    //    SetStatusMessage("Client 업데이트 파일을 설치 중입니다.");
                    //    appDepl.Update();

                    //    SetStatusMessage("Client 재시작합니다.");
                    //    bUpdateChecker = true;
                    //    this.Close();
                    //}
                }
            }
            finally
            {
                SetStatusMessage(null);
                appDepl = null;
                updateCheckInfo = null;
            }
        }

        private void InvokeUpdateDialog(
            )
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { InvokeUpdateDialog(); }));
            }
            else
            {
                if (!_update.Visible)
                {
                    _update.Owner = this;
                    _update.StartPosition = FormStartPosition.CenterScreen;
                    _update.ShowDialog();
                }
            }
        }

        /// <summary>
        /// TQC_NOTICE_BOARD에 등록되어 있는 메시지 Popup
        /// </summary>
        private void CheckNoticePopup()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    string lang = GlobalVariable.LanguageNumber.ToString();

                    RO.NoticeManagement obj = new NoticeManagement();
                    DataTable dt = obj.SelectTodayNotice(lang);

                    if (dt == null || dt.Rows.Count == 0)
                        return;

                    string noticeSeq = dt.Rows[0]["NOTICE_SEQ"].ToString();

                    SettingData setData = new SettingData(Name);
                    if (String.Equals(setData.GetValue(NOTICESEQ), noticeSeq))
                        return;

                    string title = dt.Rows[0]["TITLE"].ToString();
                    string date = dt.Rows[0]["DATE"].ToString();
                    string content = dt.Rows[0]["BODY"].ToString();

                    ShowNoticePopup(setData, noticeSeq, title, date, content);
                }
                catch (Exception ex)
                {
                    RaiseError(ex);
                }
            });
        }

        private DataTable GetMenu(
            )
        {
            DataTable dt = null;
            DataSet ds = null;
            string strMenuFile = Application.StartupPath + @"\MenuFile.mnu";

            if (File.Exists(strMenuFile))
            {
                ds = new DataSet();
                ds.ReadXml(strMenuFile);
                return ds.Tables[0];
            }

            #region POPUP MNU
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
            dt.Columns.Add(new DataColumn("MENU_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
            dt.Columns.Add(new DataColumn("STOCK_TOOLBAR", typeof(string)));
            dt.Columns.Add(new DataColumn("CAPTION001", typeof(string)));
            dt.Columns.Add(new DataColumn("CAPTION002", typeof(string)));
            dt.Columns.Add(new DataColumn("CAPTION003", typeof(string)));
            dt.Columns.Add(new DataColumn("CAPTION004", typeof(string)));
            dt.Columns.Add(new DataColumn("CAPTION005", typeof(string)));
            dt.Columns.Add(new DataColumn("TOOL_TIP", typeof(string)));
            dt.Columns.Add(new DataColumn("ICON32", typeof(string)));
            dt.Columns.Add(new DataColumn("ICON16", typeof(string)));
            dt.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("MODULE", typeof(string)));
            dt.Columns.Add(new DataColumn("ASSEMBLY", typeof(string)));
            dt.Columns.Add(new DataColumn("HELP_URL", typeof(string)));
            dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(int)));
            dt.Columns.Add(new DataColumn("RESV_01", typeof(string)));
            dt.Columns.Add(new DataColumn("RESV_02", typeof(string)));
            dt.Columns.Add(new DataColumn("RESV_03", typeof(string)));
            dt.Columns.Add(new DataColumn("RESV_04", typeof(string)));
            dt.Columns.Add(new DataColumn("RESV_05", typeof(string)));

            #endregion

            return dt;

        }

        private void RaiseError(Exception ex)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(delegate { RaiseError(ex); }));
            else
                throw ex;
        }


        #region Stat Container
        public void SendToStat(
            DataTable dt
            )
        {
            SendToStat(dt, string.Empty);
        }

        public void SendToStat(
            DataTable dt,
            string workSheetName
            )
        {
            if (udmMain.PaneFromKey("PROJECT_MANAGER").IsVisible == false)
            {
                udmMain.PaneFromKey("PROJECT_MANAGER").Pin();
                MD_STAT_ProjectManager.Initiate();
            }
            MD_STAT_ProjectManager.NewWorkSheet(workSheetName, dt, DACrux.ProjectManager.DataSourceType.External);
        }
        #endregion

        #region Status Bar

        /// <summary>
        /// Set Message
        /// </summary>
        public void SetMainStatusBarMsg(
            string strMessage
            )
        {
            utsMain.Panels[1].Text = strMessage;
            Application.DoEvents();
        }

        /// <summary>
        /// Set Progress Rate
        /// </summary>
        public void SetMainStatusBarProgress(
            int iValue,
            int iMaxValue = 100
            )
        {
            if (utsMain.Parent == null)
                return;

            if (iMaxValue > iValue)
            {
                utsMain.Panels[0].Visible = true;
            }
            else
            {
                utsMain.Panels[0].Visible = false;
            }

            utsMain.Panels[0].ProgressBarInfo.Maximum = iMaxValue;
            utsMain.Panels[0].ProgressBarInfo.Value = iValue;
            Application.DoEvents();
        }

        #endregion Status Bar

        private void ShowNoticePopup(
            SettingData setData,
            string noticeSeq,
            string title,
            string date,
            string content
            )
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { ShowNoticePopup(setData, noticeSeq, title, date, content); }));
                return;
            }

            using (frmNotice frm = new frmNotice())
            {
                frm.NoticeSeq = noticeSeq;
                frm.Title = title;
                frm.CreateDate = date;
                frm.Content = content;
                frm.ShowDialog();

                if (frm.DontShowAgain)
                {
                    setData.SetValue(NOTICESEQ, noticeSeq);
                    setData.Save();
                }
            }
        }


        //2015-04-08-정병주 : Lot Group 관련 Interface
        #region [ iENGLotGroupContainer 멤버 ]

        public DataTable GetLotGroupAll(
            )
        {
            return ducLotGroup.SaveLotGroup();
        }

        public string[] GetGroupList(
            )
        {
            return ducLotGroup.GetGroupList();
        }

        public string[] GetListInGroup(
            string sGroup
            )
        {
            return ducLotGroup.GetListInGroup(sGroup);
        }

        #endregion


        #region Status 메시지 처리

        private void CreateStatusLabel(
            )
        {
            lblStatus = new Label();
            lblStatus.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblStatus.Size = new System.Drawing.Size(350, 40);
            lblStatus.Visible = false;
            Controls.Add(lblStatus);
        }


        public void SetStatusMessage(
            string message
            )
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { SetStatusMessage(message); }));
                return;
            }

            lblStatus.Location = new System.Drawing.Point((ClientSize.Width - lblStatus.Width) / 2, (ClientSize.Height - lblStatus.Height) / 2);
            lblStatus.Text = message;
            lblStatus.Visible = !String.IsNullOrEmpty(message);
            Application.DoEvents();
        }

        #endregion

        #endregion [ Method ]
    }

    // 2019.11.22 Taihi,Kim.
    /// <summary>
    /// DACrux 단축키 관리
    /// </summary>
    public static class ShortcutLoader
    {
        static readonly string Key = "val";
        static readonly string Separator = "=";

        public static void Load(Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarManager)
        {
            SettingData setting = new SettingData(typeof(ShortcutLoader));
            string[] values = setting.GetArrayValue(Key);

            if (values != null && values.Length > 0)
            {
                Dictionary<string, Shortcut> dic = new Dictionary<string, Shortcut>();

                foreach (string val in values)
                {
                    string[] arr = val.Split(new string[] { Separator }, StringSplitOptions.None);
                    Shortcut shortcut;

                    if (arr != null && arr.Length == 2 && Enum.TryParse<Shortcut>(arr[1], out shortcut))
                        dic[arr[0]] = shortcut;
                }

                foreach (var tool in toolbarManager.Tools)
                {
                    if (dic.ContainsKey(tool.Key))
                        tool.SharedProps.Shortcut = dic[tool.Key];
                }
            }
        }

        public static void Save(Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarManager)
        {
            SettingData setting = new SettingData(typeof(ShortcutLoader));
            List<string> list = new List<string>();

            foreach (var tool in toolbarManager.Tools)
            {
                if (tool.ShortcutResolved != Shortcut.None)
                    list.Add(String.Format("{0}={1}", tool.Key, tool.ShortcutResolved.ToString()));
            }

            if (list.Count > 0)
            {
                setting.SetArrayValue("val", list.ToArray());
                setting.Save();
            }
        }
    }
}
