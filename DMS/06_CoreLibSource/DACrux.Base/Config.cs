/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Config.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Core::Configuration
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015-03-23 : YSIM => Local IP / Mac 정보 추가

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DACrux.Base
{
    /// <summary>
    /// CODE : BAS001
    /// </summary>
    public sealed class Config
    {
        private const string CLASSCODE = "BAS001";
        /// <summary>
        /// Load application GloabalValiable
        /// </summary>
        public static void Init()
        {
            StreamReader sr = null;
            GlobalVariable.ApplicationPort = new int[20];  // 에휴.. 20개 이상은 맹글지 말자...많이 만들었다 아이가~~
            try
            {
                DACrux.Base.GlobalVariable.InitConfiguration = true;
                /// 0. 임시 List 초기화
                List<string> tmpAppName = new List<string>();
                List<int> tmpAppPort = new List<int>();

                /// Local IP / Mac 정보 (2015-03-23 : YSIM)
                DACrux.Base.GlobalVariable.LocalIP = DACrux.Base.IPUtil.GetLocalIPAddress();
                DACrux.Base.GlobalVariable.LocalMacAddress = DACrux.Base.IPUtil.GetMacAddress(DACrux.Base.GlobalVariable.LocalIP);

                /// 1. File Read
                string tmp = string.Empty;
                sr = new StreamReader(GlobalVariable.CONFIGFILE, Encoding.Default);
                string strCategory = string.Empty;
                int iAppIdx = -1;

                while ((tmp = sr.ReadLine()) != null)
                {
                    // 0. #으로 시작시 무시
                    if(tmp.Trim().Length == 0 || tmp.Trim().StartsWith("#")) continue;

                    // 1. [APPLICATION UNIT] Category Read
                    if (tmp.Trim().ToUpper().StartsWith("[APPLICATION UNIT]"))
                    {
                        /// Application Port 정보 지금부터 읽기 시작
                        /// 
                        strCategory = "[APPLICATION UNIT]";
                        continue;
                    }

                    // 2. [CONFIG MODE] Category Read
                    if (tmp.Trim().ToUpper().StartsWith("[CONFIG MODE]"))
                    {
                        /// Application Port 정보 지금부터 읽기 시작
                        /// 
                        strCategory = "[CONFIG MODE]";
                        continue;
                    }

                    // 3. [LAST OPTION] Categor Read
                    if (tmp.Trim().ToUpper().StartsWith("[LAST OPTION]"))
                    {
                        /// LAST OPTION 정보 지금부터 읽기 시작
                        /// 
                        strCategory = "[LAST OPTION]";
                        continue;
                    }

                    string[] tmpArr = tmp.Split(new string[] { @"\t" ,"="}, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpArr.Length < 2) continue;

                    switch (strCategory)
                    {
                        case "[APPLICATION UNIT]":
                            ApplicationUnit tmpApp = (ApplicationUnit)Enum.Parse(typeof(ApplicationUnit), tmpArr[0].Trim(), true);
                            iAppIdx = (int)tmpApp;
                            DACrux.Base.GlobalVariable.ApplicationPort[iAppIdx] = DACrux.Base.Convert.intParse(tmpArr[1].Trim());
                            break;
                        case "[CONFIG MODE]":
                            if (tmpArr[0].Trim().ToUpper() == "CONFIG_STOCK")
                            {
                                DACrux.Base.GlobalVariable.ConfigurationMode = (ConfigMode)Enum.Parse(typeof(ConfigMode), tmpArr[1].Trim(), true);
                                if(DACrux.Base.GlobalVariable.ConfigurationMode == DACrux.Base.ConfigMode.Registry)
                                {
                                    // Registry에 저장하는 Mode였을때 Registry 값을 읽어옴
                                    DACrux.Base.GlobalVariable.UserID = DACrux.Base.RegUtil.UserID;
                                    DACrux.Base.GlobalVariable.ServerIP = DACrux.Base.RegUtil.ServerIP;
                                    DACrux.Base.GlobalVariable.EAIServerIP = DACrux.Base.RegUtil.EAIServerIP;
                                    DACrux.Base.GlobalVariable.EAIServerPort = DACrux.Base.RegUtil.EAIServerPort;
                                    DACrux.Base.GlobalVariable.EAIChannel = DACrux.Base.RegUtil.EAIChannel;
                                    DACrux.Base.GlobalVariable.Language = DACrux.Base.RegUtil.Language;
                                    DACrux.Base.GlobalVariable.LanguageFile = DACrux.Base.RegUtil.LanguageFile;
                                    DACrux.Base.GlobalVariable.Site = DACrux.Base.RegUtil.Site;
                                    DACrux.Base.GlobalVariable.Factory = DACrux.Base.RegUtil.Factory;

                                }
                            }
                            break;
                        case "[LAST OPTION]":
                            switch(tmpArr[0].Trim().ToUpper())
                            {
                                case "USERID":
                                    DACrux.Base.GlobalVariable.UserID = tmpArr[1].Trim();
                                    break;
                                case "SERVERIP":
                                    DACrux.Base.GlobalVariable.ServerIP = tmpArr[1].Trim();
                                    break;
                                case "EAISERVERIP":
                                    DACrux.Base.GlobalVariable.EAIServerIP = tmpArr[1].Trim();
                                    break;
                                case "EAISERVERPORT":
                                    DACrux.Base.GlobalVariable.EAIServerPort = tmpArr[1].Trim();
                                    break;
                                case "EAICHANNEL":
                                    DACrux.Base.GlobalVariable.EAIChannel = tmpArr[1].Trim();
                                    break;
                                case "LANGUAGE":
                                    if(tmpArr[1].Trim() == "") tmpArr[1] = "English"; // Default
                                    DACrux.Base.GlobalVariable.Language = tmpArr[1].Trim();
                                    break;
                                case "LANGUAGEFILE":
                                    DACrux.Base.GlobalVariable.LanguageFile = tmpArr[1].Trim();
                                    break;
                                case "SITE":
                                    DACrux.Base.GlobalVariable.Site = tmpArr[1].Trim();
                                    break;
                                case "FACTORY":
                                    DACrux.Base.GlobalVariable.Factory = tmpArr[1].Trim();
                                    break;
                                case "FACTORYLIST":
                                    DACrux.Base.GlobalVariable.FactoryList = tmpArr[1].Trim();
                                    break;
                                case "MANUALURL":
                                    DACrux.Base.GlobalVariable.DACruxManual = tmpArr[1].Trim();
                                    break;
                                case "RIBBONMENUENABLE":
                                    if (bool.TryParse(tmpArr[1].Trim(),out DACrux.Base.GlobalVariable.RibbonMenuEnable)==false)
                                    {
                                        DACrux.Base.GlobalVariable.RibbonMenuEnable = false;
                                    }
                                    break;
                            }
                            break;
                    }
                }

                DACrux.Base.MultiLanguage.LoadLanguage(GlobalVariable.LanguageFile, true);

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Default
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_QMS_COMMON] = 7510;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_RMS] = 7511;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_PMS] = 7512;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_PRB] = 7513;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_PKG] = 7514;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_DMS] = 7515;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_PCM] = 7516;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_SPC] = 7517;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_TMS] = 7518;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_DACRUX_YMS] = 7519;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_EMS_DA] = 7520;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_EMS_DAOPER] = 7521;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_EMS_WB] = 7522;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_EMS_DP] = 7523;
                GlobalVariable.ApplicationPort[(int)ApplicationUnit.MIRACOM_EMS_ENGUI] = 7530;
                SaveConfiguration();
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

        /// <summary>
        /// Save application Global Vaiable
        /// </summary>
        public static void SaveConfiguration()
        {
            StringBuilder sb = null;
            StreamWriter sw = null;
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("# DACrux Configuration ini file");
                sb.AppendFormat("# Create Time {0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm_ss"));
                sb.AppendLine("# Service port define of DACrux's Application module");
                sb.AppendLine("[APPLICATION UNIT]");
                for (int i = 0; i < DACrux.Base.GlobalVariable.ApplicationPort.Length; i++)
                {
                    string appModuleName = Enum.GetName(typeof(DACrux.Base.ApplicationUnit), i);
                    int iPort = DACrux.Base.GlobalVariable.ApplicationPort[i];
                    if (iPort == 0) continue;
                    sb.AppendFormat("{0} = {1}\n", appModuleName, iPort);
                }
                sb.AppendLine("");

                sb.AppendLine("[CONFIG MODE]");
                sb.AppendLine("# Method of Application Configuration");
                sb.AppendLine("# Default ");
                sb.AppendLine("# CONFIG_STOCK = Registry");
                sb.AppendLine("# Configuration mode has two option that \"Registry\" or \"INIFile\"");
                sb.AppendFormat("CONFIG_STOCK = {0}\n", Enum.GetName(typeof(DACrux.Base.ConfigMode), DACrux.Base.GlobalVariable.ConfigurationMode));

                sb.AppendLine("");
                if (DACrux.Base.GlobalVariable.ConfigurationMode == ConfigMode.INIFile)
                {
                    sb.AppendLine("[LAST OPTION]");
                    sb.AppendFormat("UserID = {0}\n", DACrux.Base.GlobalVariable.UserID);
                    sb.AppendFormat("ServerIP = {0}\n", DACrux.Base.GlobalVariable.ServerIP);
                    sb.AppendFormat("EAIServerIP = {0}\n", DACrux.Base.GlobalVariable.EAIServerIP);
                    sb.AppendFormat("EAIServerPort = {0}\n", DACrux.Base.GlobalVariable.EAIServerPort);
                    sb.AppendFormat("EAIChannel = {0}\n", DACrux.Base.GlobalVariable.EAIChannel);
                    sb.AppendFormat("Language = {0}\n", DACrux.Base.GlobalVariable.m_strLanguage);
                    sb.AppendFormat("LanguageFile = {0}\n", DACrux.Base.GlobalVariable.LanguageFile);
                    sb.AppendFormat("Site = {0}\n", DACrux.Base.GlobalVariable.Site);
                    sb.AppendFormat("Factory = {0}\n", DACrux.Base.GlobalVariable.Factory);
                    sb.AppendFormat("FactoryList = {0}\n", DACrux.Base.GlobalVariable.FactoryList);
                    sb.AppendFormat("RibbonMenuEnable = {0}\n", DACrux.Base.GlobalVariable.RibbonMenuEnable);
                }
                else
                {
                    DACrux.Base.RegUtil.UserID = DACrux.Base.GlobalVariable.UserID;
                    DACrux.Base.RegUtil.ServerIP = DACrux.Base.GlobalVariable.ServerIP;
                    DACrux.Base.RegUtil.EAIServerIP = DACrux.Base.GlobalVariable.EAIServerIP;
                    DACrux.Base.RegUtil.EAIServerPort = DACrux.Base.GlobalVariable.EAIServerPort;
                    DACrux.Base.RegUtil.EAIChannel = DACrux.Base.GlobalVariable.EAIChannel;
                    DACrux.Base.RegUtil.Language = DACrux.Base.GlobalVariable.Language;
                    DACrux.Base.RegUtil.LanguageFile = DACrux.Base.GlobalVariable.LanguageFile;
                    DACrux.Base.RegUtil.Site = DACrux.Base.GlobalVariable.Site;
                    DACrux.Base.RegUtil.Factory = DACrux.Base.GlobalVariable.Factory;
                    DACrux.Base.RegUtil.RibbonMenuEnable = DACrux.Base.GlobalVariable.RibbonMenuEnable.ToString();
                }

                sw = new StreamWriter(DACrux.Base.GlobalVariable.CONFIGFILE);
                sw.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    sw = null;
                }
            }
        }
    }
}
