/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : UserInfo.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Core::User Information 처리 Class
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DACrux.Base
{
    /// <summary>
    /// User 정보를 제공하는 Class
    /// </summary>
    /// 
    [Serializable]
    public class UserInfo
    {
        private const string CLASSCODE = "BAS002";

        private ConfigMode m_CfgMode = ConfigMode.INIFile;
        private string m_UserInfoFile = string.Empty;
        private string m_strUserID = string.Empty;
        private string m_strPassword = string.Empty;
        private string m_strLanguage = string.Empty;
        private string m_strServerIP = "Localhost";
        private string m_strEAIServerIP = "Localhost";
        private string m_strEAIServerPort = "10101";
        private string m_strEAIChannel = "/QMS";
        private string m_strFacility = string.Empty;
        private bool m_bMultiExcute = false;

        public void InitUser(string UserInfoFile = "LastUser.dat", ConfigMode cfgMode = ConfigMode.INIFile)
        {
            try
            {
                m_UserInfoFile = UserInfoFile;
                m_CfgMode = cfgMode;

                if (m_CfgMode == ConfigMode.INIFile)
                {
                    ReadFile();
                }
                else
                {
                    ReadRegist();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(CLASSCODE + "001", ex);
            }
        }

        private void ReadFile()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(m_UserInfoFile);
                string tmpLine = string.Empty;
                string[] tmpStr = null;
                while ((tmpLine = sr.ReadLine()) != null)
                {
                    if (tmpLine.Trim() == "#") continue;
                    tmpStr = tmpLine.Trim().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    m_strUserID = string.Empty;
                    m_strPassword = string.Empty;
                    m_strLanguage = string.Empty;
                    m_strServerIP = "Localhost";
                    m_strEAIServerIP = "Localhost";
                    m_strEAIServerPort = "10101";
                    m_strEAIChannel = "/QMS";
                    m_strFacility = string.Empty;
                    m_bMultiExcute = false;

                    switch (tmpStr[0].Trim().ToUpper())
                    {
                        case "USERID":
                            m_strUserID = tmpStr[1].Trim();
                            break;
                        case "PASSWORD":
                            m_strPassword = Crypt.GetDecoding(tmpStr[1].Trim());
                            break;
                        case "LANGUAGE":
                            m_strLanguage = tmpStr[1].Trim();
                            break;
                        case "SERVERIP":
                            m_strEAIServerIP = tmpStr[1].Trim();
                            break;
                        case "EAISERVER":
                            string[] tmpEAI = tmpStr[1].Trim().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            m_strEAIServerIP = tmpEAI[0].Trim();
                            m_strEAIServerPort = tmpEAI[1].Trim();
                            m_strEAIChannel = tmpEAI[2].Trim();
                            break;
                        case "FACILITY":
                            m_strFacility = tmpStr[1].Trim();
                            break;
                        case "MULTIEXCUTE":
                            m_bMultiExcute = (tmpStr[1].Trim().ToUpper() == "TRUE");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(CLASSCODE + "002", ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        private void ReadRegist()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception(CLASSCODE + "003", ex);
            }
        }

        public bool IsMultiExcute
        {
            get
            {
                return m_bMultiExcute;
            }
            set
            {
                m_bMultiExcute = value;
            }
        }

        public string UserID
        {
            get
            {
                return m_strUserID;
            }
            set
            {
                m_strUserID = value;
            }
        }

        public string Password
        {
            get
            {
                return m_strPassword;
            }
            set
            {
                m_strPassword = value;
            }
        }

        public string Language
        {
            get
            {
                return m_strLanguage;
            }
            set
            {
                m_strLanguage = value;
            }
        }

        public string ServerIP
        {
            get
            {
                return m_strServerIP;
            }
            set
            {
                m_strServerIP = value;
            }
        }

        public string EAIServerIP
        {
            get
            {
                return m_strEAIServerIP;
            }
            set
            {
                m_strEAIServerIP = value;
            }
        }

        public string EAIServerPort
        {
            get
            {
                return m_strEAIServerPort;
            }
            set
            {
                m_strEAIServerPort = value;
            }
        }

        public string EAIChannel
        {
            get
            {
                return m_strEAIChannel;
            }
            set
            {
                m_strEAIChannel = value;
            }
        }

        public string Facility
        {
            get
            {
                return m_strFacility;
            }
            set
            {
                m_strFacility = value;
            }
        }
    }
}
