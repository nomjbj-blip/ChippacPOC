/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2015 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : RemoteConfig.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.01.30
--  Description     : DACrux Core::리모팅 설정
--  History         : Created by YSIM at 2015.01.30
 * ********************************************************************************************************
 * 2015 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DACrux.Base
{
    public static class RemoteConfig
    {
        public const string COMPANY = "Miracom";
        public const string PRODUCT = "DACrux";

        public static string url(ApplicationUnit application)
        {
            string strServer = string.Empty;
            try
            {
                if (DACrux.Base.GlobalVariable.ConfigurationMode == ConfigMode.INIFile)
                {
                    strServer = DACrux.Base.GlobalVariable.ServerIP;
                }
                else
                {
                    strServer = (string)DACrux.Base.RegUtil.GetServerIP(application);
                    if (strServer == null || strServer.Length == 0 || strServer == string.Empty)
                    {
                        strServer = DACrux.Base.GlobalVariable.ServerIP;
                    }
                }
                return string.Format("tcp://{0}:{1}", strServer, DACrux.Base.GlobalVariable.ApplicationPort[(int)application]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string url(ApplicationUnit application, string ObjUri)
        {
            string strServer = string.Empty;
            try
            {
                if (DACrux.Base.GlobalVariable.ConfigurationMode == ConfigMode.INIFile)
                {
                    strServer = DACrux.Base.GlobalVariable.ServerIP;
                }
                else
                {
                    strServer = (string)DACrux.Base.RegUtil.GetServerIP(application);
                    if (strServer == null || strServer.Length == 0 || strServer == string.Empty)
                    {
                        strServer = DACrux.Base.GlobalVariable.ServerIP;
                    }
                }
                return string.Format("tcp://{0}:{1}/{2}", strServer
                    , DACrux.Base.GlobalVariable.ApplicationPort[(int)application]
                    , ObjUri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object url(ApplicationUnit application, string ObjUri,Type RmtType)
        {
            string strServer = string.Empty;
            try
            {
                if (DACrux.Base.GlobalVariable.ConfigurationMode == ConfigMode.INIFile)
                {
                    strServer = DACrux.Base.GlobalVariable.ServerIP;
                }
                else
                {
                    strServer = (string)DACrux.Base.RegUtil.GetServerIP(application);
                    if (strServer == null || strServer.Length == 0 || strServer == string.Empty)
                    {
                        strServer = DACrux.Base.GlobalVariable.ServerIP;
                    }
                }
                string strUrl =  string.Format("tcp://{0}:{1}/{2}", strServer
                                , DACrux.Base.GlobalVariable.ApplicationPort[(int)application]
                                , ObjUri);

                return Activator.GetObject(RmtType, strUrl) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
