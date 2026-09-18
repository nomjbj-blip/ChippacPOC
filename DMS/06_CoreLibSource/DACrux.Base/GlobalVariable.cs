/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : GlobalVariable.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.02.05
--  Description     : DACrux Core::GlobalVariable
--  History         : Created by YSIM at 2015.02.05
 * ********************************************************************************************************
 * .Net Framework 2.0 Based
 * 2015-03-23 : YSIM => Local IP / Mac 정보 추가

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DACrux.Base
{
    public sealed class GlobalVariable
    {
        public static bool InitConfiguration = false;
        // User Information
        public static string UserID = string.Empty;         //Save Value
        public static string Password = string.Empty;
        public static string UserDescription = string.Empty;
        public static string UserGroup = string.Empty;
        public static string Department = string.Empty;
        public static string UserArea = string.Empty;
        public static string UserSubArea = string.Empty;

        //* 2015-03-23 : YSIM => Local IP / Mac 정보 추가
        public static string LocalMacAddress = string.Empty;
        public static string LocalIP = string.Empty;

        public static string ServerIP = "Localhost";        //Save Value
        public static string EAIServerIP = "Localhost";     //Save Value
        public static string EAIServerPort = "10101";       //Save Value
        public static string EAIChannel = "/QMS";           //Save Value
        public static bool MultiExcute = false;             //Save Value

        // Site Information
        public static string Site = string.Empty;            //Save Value
        public static string Factory = string.Empty;         //Save Value
        public static string FactoryList = string.Empty;         //Save Value
        public static string Passport = string.Empty;

        //Manual URL
        public static string DACruxManual = "DACrux/Manual/DBH_통합_DMS_구축_사용자_메뉴얼_V1.0.pdf";

        // Application 
        public const string AppName = "DACrux";
        public const string AppVersion = "5";
        public static string AppSubVersion = "0";
        public static string AppSpesialVersion = "A";
        public static string ApplicationLongVersion
        {
            get
            {
                return string.Format("{0} V{1}.{2}{3}", AppName, AppVersion, AppSubVersion, AppSpesialVersion);
            }
        }

        internal static string m_strLanguage = "English";     //Save Value
        internal static int m_iLanguageNumber = 1;     //Save Value
        public static int LanguageNumber
        {
            get
            {
                // 최초에만 Language Index를 찾는다.
                if (m_iLanguageNumber == -1 && DACrux.Base.MultiLanguage.LanguageList != null)
                {
                    m_iLanguageNumber = Array.IndexOf(DACrux.Base.MultiLanguage.LanguageList, m_strLanguage);
                }
                return m_iLanguageNumber;
            }
        }
        public static string Language
        {
            set
            {
                m_strLanguage = value;
                // Language가 바뀔때 Index를 찾는다.
                if (DACrux.Base.MultiLanguage.LanguageList != null)
                {
                    m_iLanguageNumber = Array.IndexOf(DACrux.Base.MultiLanguage.LanguageList, m_strLanguage);
                }
                else
                {
                    m_iLanguageNumber = -1;
                }
            }
            get
            {
                return m_strLanguage;
            }
        }

        public static string LanguageFile = "Global.Lng";       //Save Value
        public static DACrux.Base.ConfigMode ConfigurationMode = ConfigMode.INIFile;
        internal const string CONFIGFILE = "DACruxConfig.ini";
        public static int[] ApplicationPort = null;             //Save Value

        public static void SaveGlobalVariable()
        {
            DACrux.Base.Config.SaveConfiguration();
        }

        public static DACrux.Base.EXITMODE ExitMode = DACrux.Base.EXITMODE.Abort;
        public static bool RibbonMenuEnable = false;
    }
}
