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

            /// ///////////////////////////////////////////////////////////////////////

            bool bNew = true;
            Mutex mutex = null;

            try
            {
                /// 중복 Check
                if (DACrux.Base.GlobalVariable.MultiExcute == false)
                {
                    mutex = new Mutex(true, "DCAruxV5", out bNew);

                    if (bNew == false)
                    {
                        DACrux.Framework.DCMH.DspMessage("Already Running DACrux!!!");
                        Application.Exit();
                    }
                    mutex.ReleaseMutex();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                System.Drawing.Icon oIcon = (System.Drawing.Icon)(Resources.MainIcon);
#if DEBUG
                DACrux.Base.GlobalVariable.UserID = "ysim";
                DACrux.Base.GlobalVariable.Password = "misy";
                DACrux.Framework.DACruxMain mMain = new DACrux.Framework.DACruxMain(DACrux.Base.GlobalVariable.ApplicationLongVersion, oIcon);
                Application.Run(mMain);                    
#else
                while (DACrux.Base.GlobalVariable.ExitMode != DACrux.Base.EXITMODE.Exit)
                {
                    DACrux.Framework.frmLoginDACrux oLogin = new DACrux.Framework.frmLoginDACrux();
                    if (oLogin.ShowDialog() == DialogResult.OK)
                    {
                        DACrux.Framework.DACruxMain mMain = new DACrux.Framework.DACruxMain(DACrux.Base.GlobalVariable.ApplicationLongVersion, oIcon);
                        Application.Run(mMain);
                        switch (DACrux.Base.GlobalVariable.ExitMode)
                        {
                            case DACrux.Base.EXITMODE.Abort:
                                /// 비정상 종료이므로 Message Display 하고 
                                /// Login 창을 다시 보여줌
                                DACrux.Framework.DCMH.DspError("DACrux is Aborted!!!");
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
            catch (Exception ex)
            {
                DACrux.Framework.DCMH.DspMessage(ex.Message);
            }
        }

    }
}
