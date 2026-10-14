/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Program.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.5.29
--  Description     : DACrux V5 Main Frame Start Program
--  History         : Created by YSIM at 2014.5.29
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2014.05.29 : YSIM
 *            - Exit Mode, Log Off Mode구분하여 Login 창 재진입 수정
 *            - Debug Mode였을때 Login창 재진입 로직 회피
----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Collections;
using Microsoft.Win32;
using System.Security.Principal;
using System.Diagnostics;
using DACruxV5.Properties;

namespace DACruxV5
{
    static class DACruxClientMain
    {
        /// <summary>
        /// Main 화면 상단(제목 표시줄)에 보일 이름.
        /// 원래는 DACrux.Base.GlobalVariable.ApplicationLongVersion("DACrux V5.0A")을 넘겼으나
        /// POC 화면 이름으로 바꾸기 위해 이 값만 넘긴다. Core(GlobalVariable)는 로그인 창 등에서 쓰므로 고치지 않는다. (2026-10-08)
        /// </summary>
        const string MainTitle = "SCK E-Test POC";

        /// <summary>
        /// 메뉴(메뉴 바 / 펼친 메뉴) 글자 크기(pt). Windows 기본 메뉴 글꼴(9pt)보다 키워 POC 시연 화면에서 잘 보이게 한다. (2026-10-08)
        /// </summary>
        const float MenuFontSize = 11f;

        /// <summary>
        /// 메뉴 바(Setup / DMS Analysis / TEST Analysis) 글자 크기(pt). 펼친 메뉴보다 한 단계 크게 + 굵게 해서
        /// 최상위 메뉴가 한눈에 보이게 한다. (2026-10-08)
        /// </summary>
        const float MenuBarFontSize = 12f;

        /// <summary>
        /// 메뉴 바 항목 사이 간격(px). 아이콘 + 글자 단추가 붙어 보이지 않게 띄운다. (2026-10-08)
        /// </summary>
        const int MenuBarToolSpacing = 6;

        /// <summary>
        /// Main 화면 메뉴를 크게 / 아이콘과 함께 보이게 한다.
        /// 메뉴는 Core(DACrux.Framework.DACruxMain)의 utbManager(Infragistics UltraToolbarsManager)가 그리는데,
        /// Core 를 고치지 않기 위해 FromForm 으로 Main 화면에 붙은 ToolbarsManager 를 찾아 표시 설정만 바꾼다.
        /// - ToolbarSettings : 툴바 공통 글자 크기
        /// - MenuSettings    : 펼친 메뉴 항목 글자 크기 (아이콘은 Core 에서 이미 큰 아이콘(32px) 사용)
        /// - 메뉴 바(IsMainMenuBar) : 기본값은 글자만 보이므로 아이콘 + 글자(ImageAndText) / 큰 아이콘(32px) /
        ///   더 큰 굵은 글자 / 항목 간격을 준다. 아이콘은 MenuFile.mnu ROOT 행의 ICON32 를 MenuHelper 가 이미 넣어 둔다.
        /// 찾지 못하면 아무것도 하지 않는다(기존 화면 그대로).
        /// </summary>
        static void ApplyMenuSize(Form mainForm)
        {
            Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utb = Infragistics.Win.UltraWinToolbars.UltraToolbarsManager.FromForm(mainForm);
            if (utb == null)
                return;

            utb.ToolbarSettings.Appearance.FontData.SizeInPoints = MenuFontSize;
            utb.MenuSettings.Appearance.FontData.SizeInPoints = MenuFontSize;

            foreach (Infragistics.Win.UltraWinToolbars.UltraToolbar bar in utb.Toolbars)
            {
                if (!bar.IsMainMenuBar)
                    continue;

                bar.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
                bar.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.True;
                bar.Settings.ToolSpacing = MenuBarToolSpacing;
                bar.Settings.Appearance.FontData.SizeInPoints = MenuBarFontSize;
                bar.Settings.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            }
        }

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// DACrux Multi Excute
            /// DACrux를 Multi로 실행하고 실행시 여러 User로 Login하는 것을 허용함
            /// DACrux.Base.Config.CurrentUser.IsMultiExcute = true; 허용
            /// DACrux.Base.Config.CurrentUser.IsMultiExcute = false; 허용하지 않음
            /// ///////////////////////////////////////////////////////////////////////
            /// 
            DACrux.Base.Config.Init();

#if DEBUG
            DACrux.Framework.Base.DACruxUXBasic01.UseFullInformation = true;
#endif

            /// ///////////////////////////////////////////////////////////////////////

            //bool bNew = true;
            //Mutex mutex = null;

            /// 중복 Check
            //if (DACrux.Base.GlobalVariable.MultiExcute == false)
            //{
            //    mutex = new Mutex(true, "DCAruxV5", out bNew);

            //    if (bNew == false)
            //    {
            //        DACrux.Framework.DCMH.DspMessage("Already Running DACrux!!!");
            //        Application.Exit();
            //    }
            //    mutex.ReleaseMutex();
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            // 작업 스레드 / ThreadPool 예외는 ThreadException 으로 오지 않고 프로세스를 바로 종료시킨다.
            // 종료를 막을 수는 없으므로 원인을 로그 파일에 남긴다. (2026-10-08)
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            System.Drawing.Icon oIcon = (System.Drawing.Icon)(Resources.MainIcon);
#if DEBUG
            DACrux.Base.GlobalVariable.UserID = "ADMIN";
            DACrux.Base.GlobalVariable.Password = "1111";
            DACrux.Framework.DACruxMain mMain = new DACrux.Framework.DACruxMain(MainTitle, oIcon);
            ApplyMenuSize(mMain);
            mMain.Load += new EventHandler(mMain_Load);
            Application.Run(mMain);
#else
           while (DACrux.Base.GlobalVariable.ExitMode != DACrux.Base.EXITMODE.Exit)
            {
                DACrux.Framework.frmLoginDACrux oLogin = new DACrux.Framework.frmLoginDACrux();
                //oLogin.UsingFactorySelect(true);

                if (oLogin.ShowDialog() == DialogResult.OK)
                {
                    DACrux.Framework.DACruxMain mMain = new DACrux.Framework.DACruxMain(
                        MainTitle,
                        oIcon
                        );
                    ApplyMenuSize(mMain);

                    Application.Run(mMain);
                    switch (DACrux.Base.GlobalVariable.ExitMode)
                    {
                        case DACrux.Base.EXITMODE.Abort:
                            /// 비정상 종료이므로 Message Display 하고 
                            /// Login 창을 다시 보여줌
                            DACrux.Framework.DCMH.DspMessage("DACrux is Aborted!!!");
                            break;
                        case DACrux.Base.EXITMODE.Exit:
                            /// 종료 Mode였을때 System을 종료함
                            mMain.Dispose();
                            mMain = null;
                            Application.Exit();
                            break;
                        case DACrux.Base.EXITMODE.Logout:
                            /// Logout Mode였을때 Login 창을 다시 보여줌
                            continue;
                        default:
                            break;
                    }
                }
                else
                {
                    // 시스템 종료
                    break;
                }
            }
#endif
        }

#if DEBUG
        static void mMain_Load(object sender, EventArgs e)
        {
            //DACrux.SEMDMS.ENGUI.frmZipTest frm = new DACrux.SEMDMS.ENGUI.frmZipTest();
            //frm.MdiParent = sender as Form;
            //frm.Show();
        }
#endif

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            WriteErrorLog("ThreadException", e.Exception);

            if (e.Exception.InnerException != null)
                MessageBox.Show(e.Exception.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

#if DEBUG
            MessageBox.Show(String.Format("{0}\r\n\r\n{1}", e.Exception.Message, e.Exception.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteErrorLog(e.IsTerminating ? "UnhandledException(terminating)" : "UnhandledException", e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 처리되지 않은 예외를 %LOCALAPPDATA%\DACrux\Log\DACrux_Error_yyyyMMdd.log 에 남긴다.
        /// 설치 폴더(Program Files)는 쓰기 권한이 없을 수 있어 사용자 폴더를 쓴다. 로그 실패는 무시한다. (2026-10-08)
        /// </summary>
        static void WriteErrorLog(string kind, Exception ex)
        {
            try
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DACrux", "Log");
                Directory.CreateDirectory(dir);

                string file = Path.Combine(dir, String.Format("DACrux_Error_{0:yyyyMMdd}.log", DateTime.Now));
                string text = String.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] {1} (Thread {2})\r\n{3}\r\n\r\n",
                    DateTime.Now, kind, Thread.CurrentThread.ManagedThreadId, ex != null ? ex.ToString() : "(exception object is not System.Exception)");

                File.AppendAllText(file, text);
            }
            catch
            {
                // 로그를 남기지 못해도 원래 예외 처리 흐름을 바꾸지 않는다.
            }
        }

    }
}
