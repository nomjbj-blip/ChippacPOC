/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2015 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Regedit.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.01.30
--  Description     : DACrux Core::레지스트리 설정 클래스 Class
--  History         : Created by YSIM at 2015.01.30
 * ********************************************************************************************************
 * 2015 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.IO;

namespace DACrux.Base
{
    /// <summary>
    /// 클래스  명: Regedit<br/>
    /// 클래스요약: 레지스트리 설정 클래스<br/>
    /// 작  성  자: YSIM<br/>
    /// 최초작성일: 2015.01.30<br/>
    /// 최종수정자: YSIM<br/>
    /// 최종수정일: 2015.01.30<br/>
    /// 상세  설명: DACrux 옵션정보를 레지스트리에서 읽어오거나 저장한다<br/>
    /// 변경  내용: <br/>
    /// </summary>
    /// Registry Key 정리
    /// 
    /// HKEY_LOCAL_MACHINE
    ///          >> SOFTWARE 
    ///            (or >> Wow6432Node)
    ///                >> Miracom
    ///                     >>DACrux
    ///                         ■ UserID
    ///                         ■ EAIServerIP
    ///                         ■ EAIServerPort
    ///                         ■ EAIChannel
    ///                         ■ ServerIP
    ///                         ■ Factory
    ///                         ■ Laguage
    ///                         ■ LaguageFile
    ///                         ■ Site
    ///                         >> SPC
    ///                             ■ Area
    ///                             ■ Server
    ///                         >> DMS
    ///                             ■ Area
    ///                             ■ Server
    ///                         >> PROBE
    ///                             ■ Area
    ///                             ■ Server
    /// 
    public static class RegUtil
    {
        public static string COMPANY = "Miracom";
        public static string PRODUCT = "DACrux";

        /// <summary>
        /// 언어를 설정하거나 가져온다.
        /// </summary>
        internal static string Language
        {
            get
            {
                return GetRegKeyProductOption("Language");
            }
            set
            {
                SetRegKeyProductOption("Language", value);
            }
        }
        internal static string LanguageFile
        {
            get
            {
                return GetRegKeyProductOption("LanguageFile");
            }
            set
            {
                SetRegKeyProductOption("LanguageFile", value);
            }
        }
        internal static string Site
        {
            get
            {
                return GetRegKeyProductOption("Site");
            }
            set
            {
                SetRegKeyProductOption("Site", value);
            }
        }
        /// <summary>
        /// 마지막 로그인 사용자를 가져오거나 설정한다.
        /// </summary>
        internal static string UserID
        {
            get
            {
                return GetRegKeyProductOption("UserID");
            }
            set
            {
                SetRegKeyProductOption("UserID", value);
            }
        }

        internal static string Factory
        {
            get
            {
                return GetRegKeyProductOption("Factory");
            }
            set
            {
                SetRegKeyProductOption("Factory", value);
            }
        }

        internal static string RibbonMenuEnable
        {
            get
            {
                return GetRegKeyProductOption("RibbonMenuEnable");
            }
            set
            {
                SetRegKeyProductOption("RibbonMenuEnable", value);
            }
        }

        /// <summary>
        /// DACrux Common Server IP
        /// </summary>
        internal static string ServerIP
        {
            get
            {
                return GetRegKeyProductOption("ServerIP");
            }
            set
            {
                SetRegKeyProductOption("ServerIP", value);
            }
        }

        #region Get or Set EAIServer In DACrux

        /// <summary>
        /// Get or Set H101Server IP Address
        /// </summary>
        /// 
        public static string EAIServerIP
        {
            get
            {
                return GetRegKeyProductOption("EAIServerIP");
            }
            set
            {
                SetRegKeyProductOption("EAIServerIP", value);
            }
        }

        /// <summary>
        /// Get or Set EAIServer Port
        /// </summary>
        public static string EAIServerPort
        {
            get
            {
                return GetRegKeyProductOption("EAIServerPort");
            }
            set
            {
                SetRegKeyProductOption("EAIServerPort", value);
            }
        }

        /// <summary>
        /// Get or Set EAI Channel
        /// </summary>
        public static string EAIChannel
        {
            get
            {
                return GetRegKeyProductOption("EAIChannel");
            }
            set
            {
                SetRegKeyProductOption("EAIChannel", value);
            }
        }

        #endregion

        #region Get or Set Server By DACrux Module Level

        /// <summary>
        /// Get or Set Server IP Address
        /// </summary>
        /// 
        /// HKEY_LOCAL_MACHINE
        ///          >> SOFTWARE
        ///                >> Miracom
        ///                     >>DACrux
        ///                         {■ Language}
        ///                         {■ UserID}
        ///                         {■ ServerIP}
        ///                         {■ Factory}
        ///                         >> SPC
        ///                            {■ Area}
        ///                             ■ Server
        ///                         >> DMS
        ///                            {■ Area}
        ///                             ■ Server
        ///                         >> PROBE
        ///                            {■ Area}
        ///                             ■ Server
        ///                             


        public static string GetServerIP(string Module)
        {
            return GetRegKeyModuleOption(Module, "Server");
        }

        public static string GetServerIP(ApplicationUnit AppUnit)
        {
            string tmpAppName = Enum.GetName(typeof(ApplicationUnit), AppUnit);
            string[] tmpArr = tmpAppName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            PRODUCT = tmpArr[1];
            string Module = tmpArr[2];
            return GetServerIP(Module);
        }

        public static string GetModuleArea(string Module)
        {
            return GetRegKeyModuleOption(Module, "Area");
        }

        public static string GetModuleArea(ApplicationUnit AppUnit)
        {
            string tmpAppName = Enum.GetName(typeof(ApplicationUnit), AppUnit);
            string[] tmpArr = tmpAppName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            PRODUCT = tmpArr[1];
            string Module = tmpArr[2];
            return GetModuleArea(Module);
        }


        public static void SetServerIP(string Module, string IP)
        {
            SetRegKeyModuleOption(Module, "Server", IP);
        }

        public static void SetModuleArea(string Module, string Area)
        {
            SetRegKeyModuleOption(Module, "Area", Area);
        }

        #endregion


        #region Set Registry Value
        /// <summary>
        /// 레지스트리에 사용자 옵션을 설정한다.
        /// </summary>
        /// <param name="strKey">설정하고자하는 데이타</param>
        /// <param name="strValue">strKey에 해당하는 문자열값</param>
        public static void SetRegKeyProductOption(string strKey, string strValue)
        {
            RegistryKey HKSoftware = null;
            try
            {
                HKSoftware = Registry.LocalMachine.CreateSubKey("SOFTWARE");
            }
            catch
            {
            }

            if (HKSoftware == null)
                HKSoftware = Registry.CurrentUser.CreateSubKey("SOFTWARE");

            RegistryKey HKMiracom = HKSoftware.CreateSubKey(COMPANY);
            RegistryKey HKDACrux = HKMiracom.CreateSubKey(PRODUCT);

            HKDACrux.SetValue(strKey, strValue);

            HKDACrux.Close();
            HKMiracom.Close();
            HKSoftware.Close();

        }

        /// <summary>
        /// 레지스트리에 사용자 옵션을 설정한다.
        /// </summary>
        /// <param name="Module">DACrux Module 단위를 나타냄</param>
        /// <param name="strKey">설정하고자하는 데이타</param>
        /// <param name="strValue">strKey에 해당하는 문자열값</param>
        public static void SetRegKeyModuleOption(string Module, string strKey, string strValue)
        {
            RegistryKey HKSoftware = null;
            try
            {
                HKSoftware = Registry.LocalMachine.CreateSubKey("SOFTWARE");
            }
            catch
            {
            }

            if (HKSoftware == null)
                HKSoftware = Registry.CurrentUser.CreateSubKey("SOFTWARE");

            RegistryKey HKMiracom = HKSoftware.CreateSubKey(COMPANY);
            RegistryKey HKDACrux = HKMiracom.CreateSubKey(PRODUCT);
            RegistryKey HKUserOption = HKDACrux.CreateSubKey(Module);

            HKUserOption.SetValue(strKey, strValue);

            HKUserOption.Close();
            HKDACrux.Close();
            HKMiracom.Close();
            HKSoftware.Close();

        }

        /// <summary>
        /// Set Registry Value
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strValue">Value</param>
        public static void SetRegKey(string strKey, string strValue)
        {
            RegistryKey HKSoftware = null;
            try
            {
                HKSoftware = Registry.LocalMachine.CreateSubKey("SOFTWARE");
            }
            catch
            {
            }

            if (HKSoftware == null)
                HKSoftware = Registry.CurrentUser.CreateSubKey("SOFTWARE");

            RegistryKey HKMiracom = HKSoftware.CreateSubKey(COMPANY);
            RegistryKey HKDACrux = HKMiracom.CreateSubKey(PRODUCT);

            HKDACrux.SetValue(strKey, strValue);

            HKDACrux.Close();
            HKMiracom.Close();
            HKSoftware.Close();
        }

        #endregion

        #region Get Registry Value
        /// <summary>
        /// 레지스트리에서 사용자 옵션을 가져온다.
        /// </summary>
        /// <param name="strKey">검색하고자 하는 문자열</param>
        /// <returns>해당하는 문자열값</returns>
        /// 
        /// 하위의 Registry Key의 Parameter값을 가져온다.
        /// HKEY_LOCAL_MACHINE
        ///          >> SOFTWARE
        ///                >> Miracom
        ///                     >>DACrux
        ///                         ■ LanguageOption   
        ///                         ■ LastUser  
        ///                         ■ Facility
        public static string GetRegKeyProductOption(string strKey)
        {
            string strRet;
            RegistryKey HKSoftware = null;
            try
            {
                HKSoftware = Registry.LocalMachine.CreateSubKey("SOFTWARE");
            }
            catch
            {
            }

            if (HKSoftware == null)
                HKSoftware = Registry.CurrentUser.CreateSubKey("SOFTWARE");

            RegistryKey HKMiracom = HKSoftware.CreateSubKey(COMPANY);
            RegistryKey HKDACrux = HKMiracom.CreateSubKey(PRODUCT);

            strRet = (string)HKDACrux.GetValue(strKey);
            if (strRet != null)
            {
                HKDACrux.Close();
                HKMiracom.Close();
                HKSoftware.Close();

                return strRet;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 레지스트리에서 사용자 옵션을 가져온다.
        /// </summary>
        /// <param name="strKey">검색하고자 하는 문자열</param>
        /// <returns>해당하는 문자열값</returns>
        /// 
        /// 하위의 Registry Key의 Parameter값을 가져온다.
        /// HKEY_LOCAL_MACHINE
        ///          >> SOFTWARE
        ///                >> Miracom
        ///                     >>DACrux
        ///                         {■ LanguageOption} 이항목은 GetRegKeyProductOption()
        ///                         {■ LastUser}       이항목은 GetRegKeyProductOption()
        ///                         {■ Facility}       이항목은 GetRegKeyProductOption()
        ///                         >> SPC
        ///                             ■ Area
        ///                             ■ Server
        public static string GetRegKeyModuleOption(string Module, string strKey)
        {
            string strRet;
            RegistryKey HKSoftware = null;
            try
            {
                HKSoftware = Registry.LocalMachine.CreateSubKey("SOFTWARE");
            }
            catch
            {
            }

            if (HKSoftware == null)
                HKSoftware = Registry.CurrentUser.CreateSubKey("SOFTWARE");

            RegistryKey HKMiracom = HKSoftware.CreateSubKey(COMPANY);
            RegistryKey HKDACrux = HKMiracom.CreateSubKey(PRODUCT);
            RegistryKey HKUserModule = HKDACrux.CreateSubKey(Module);

            strRet = (string)HKUserModule.GetValue(strKey);

            if (strRet != null)
            {
                HKUserModule.Close();
                HKDACrux.Close();
                HKMiracom.Close();
                HKSoftware.Close();

                return strRet;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }

}
