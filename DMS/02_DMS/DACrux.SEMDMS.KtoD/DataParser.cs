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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DACrux.SEMDMS.KtoD
{

    /// <Summary>
    /// 
    /// <b>■KLARF File Parsing을 위한 Process Class</b><br>
    ///  
    /// - 작  성  자 : 미라콤 임영신<br>
    /// - 최초작성일 : 2004년 07월 07일<br>
    /// - 최종수정자 : 임영신<br>
    /// - 최종수정일 : 2012년 04월 04일<br>
    /// - 주요변경로그<br>
    /// 2004.07.07 생성<br>
    /// 2012.04.04 수정<br>
    /// 2013.09.11 반도체 Standard 및 WLP Line의 KLARF 처리로직 추가<br>
    /// </Summary>
    /// <Remarks>없음</Remarks>
    /// 
    public class DataParser
    {
        public static string[] args_value = null;
        /// <Summary>
        /// <b>■ 외부에서 File을 Argument로 전달 받아 처리하는 Main 서비스</b><br>
        /// - 작  성  자 : 미라콤 임영신<br>
        /// - 최초작성일 : 2004년 07월 07일<br>
        /// - 최종수정자 : 임영신<br>
        /// - 최종수정일 : 2013년 11월 12일<br>
        /// - 주요변경로그<br>
        /// 2013-11-12 : YSIM > Add on WIN3200 Parsing Logic
        /// </Summary>
        /// 
        /// <param name="args">KLARF File Name</param>
        /// <returns>없음</returns>
        /// 
        public static void Main(string[] args)
        {
            args_value = args;
            DACrux.Base.ARGUMENT_TAG oArgValues = new DACrux.Base.ARGUMENT_TAG();

            /// Argument에 대한 Parsing
            /// =================================================================================
            ParsingArgument(args, ref oArgValues);
            ///----------------------------------------------------------------------------------

            if (oArgValues.FileFormat == "WB3200")
            {
                Main_WB3200(oArgValues);
            }
            else
            {
                Main_KLARF(oArgValues);

            }
        }

        public void TestMain(string[] args)
        {
            args_value = args;
            DACrux.Base.ARGUMENT_TAG oArgValues = new DACrux.Base.ARGUMENT_TAG();

            /// Argument에 대한 Parsing
            /// =================================================================================
            ParsingArgument(args, ref oArgValues);
            ///----------------------------------------------------------------------------------

            if (oArgValues.FileFormat == "WB3200")
            {
                Main_WB3200(oArgValues);
            }
            else
            {
                Main_KLARF(oArgValues);

            }
        }

        #region ◎ KLARF Parsing Logic
        public static void Main_KLARF(DACrux.Base.ARGUMENT_TAG oArgValues)
        {
            //System.Threading.Thread.Sleep(1000);

            KLARF oKLARFParser = null;
            string strBackupImage = string.Empty;
            string strSourceImage = string.Empty;
            string strEndTime = string.Empty;
            string stepSeqDms = string.Empty;

            try
            {
                oKLARFParser = new KLARF();
                oKLARFParser.KLARF_A(oArgValues);

                if (Path.GetExtension(oArgValues.ResultFile).ToUpper().Equals(".ZIP"))
                {
                    Process.Start(oArgValues.ResultFile);
                }
                /// 1.현재의 File이 처리 가능한 File인지 Check
                /// 2.KLARF에 Image Name이 있다면 이 Image가 전부 존재하는지 Check
                /// =================================================================================
                if (!oKLARFParser.PreParsingKlarf(oArgValues.ResultFile))
                {
                    Logging.SocketOpen(oKLARFParser.UPDATESERVERIP, oKLARFParser.SOCKETPORT);
                    Logging.WriteLog(string.Format("SKEEP,{0}", oArgValues.ResultFile), "SENDDATA", 0);
                    return; //위 두 조건에 만족하지 않으면 아무런 Action을 취하지 않고 Return
                }
                ///----------------------------------------------------------------------------------


                /// Logging을 시작한다. (LOG LEVEL = 0)
                /// =================================================================================
                Logging.Start(string.Format(@"{0}\verbose.log", oArgValues.LogPath)
                             , string.Format(@"{0}\history.log", oArgValues.LogPath), oArgValues.LogLevel);

                Logging.SocketOpen(oKLARFParser.UPDATESERVERIP, oKLARFParser.SOCKETPORT);


                Logging.WriteLog("###File path config################################################", "PROC", 0);
                Logging.WriteLog(string.Format("###[RESULT   ] [{0}]", oArgValues.ResultFile), "PROC", 0);
                Logging.WriteLog(string.Format("###[BACKUP   ] [{0}]", oArgValues.BackupFile), "PROC", 0);
                Logging.WriteLog(string.Format("###[SERVICE.1] [{0}]", oArgValues.ServiceFile), "PROC", 1);
                Logging.WriteLog(string.Format("###[DBCOMMAND] [{0}]", oArgValues.UploadComand), "PROC", 1);
                Logging.WriteLog(string.Format("###[COMMAND  ] [{0}]", string.Join(" ", args_value)), "PROC", 0);
                Logging.WriteLog("###################################################################", "PROC", 0);
                ///----------------------------------------------------------------------------------

                if (oArgValues.ResultFile != null && !File.Exists(oArgValues.ResultFile))
                {
                    Logging.WriteLog(string.Format("[PROCESSING] | [FILE_NOT_FOUND] : [{0}]", oArgValues.ResultFile), "PROC", 0);
                    ConsolMessage();
                    return;
                }

                if (oArgValues.ResultFile == null)
                {
                    ConsolMessage();
                    return;
                }

                Logging.WriteLog(string.Format("[PROCESSING] | [PROC_START] : [{0}]", oArgValues.ResultFile), "PROC", 0);

                /// File을 Backup한다.
                /// =================================================================================
                string[] strArchiveFiles = null;
                if (oArgValues.BackupPath != null && oArgValues.BackupPath.Length > 0)
                {
                    if (!Directory.Exists(oArgValues.BackupPath))
                    {
                        Directory.CreateDirectory(oArgValues.BackupPath);
                        Logging.WriteLog(string.Format("[CREATE_PATH] | [BACKUP_PATH] : [{0}]", oArgValues.BackupPath), "PROC", 0);
                    }

                    if (!Directory.Exists(oArgValues.ArchivePath))
                    {
                        Directory.CreateDirectory(oArgValues.ArchivePath);
                        Logging.WriteLog(string.Format("[CREATE_PATH] | [BACKUP_PATH] : [{0}]", oArgValues.ArchivePath), "PROC", 0);
                    }

                    string strArchiveFileName = string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}.ZIP", oArgValues.ArchivePath
                                                                                    , oKLARFParser.EQUIP_ID
                                                                                    , oKLARFParser.LOT_ID
                                                                                    , oKLARFParser.WAFER_ID
                                                                                    , oKLARFParser.STEP_ID
                                                                                    , oArgValues.TransTime);

                    if (oKLARFParser.m_arrImgSourceList.Count > 0)
                    {
                        strArchiveFiles = new string[oKLARFParser.m_arrImgSourceList.Count + 1];
                        int iFiles = 0;
                        strArchiveFiles[iFiles++] = oArgValues.ResultFile;
                        Logging.WriteLog(string.Format("[ARCHIVE BACKUP] | [KLARF : {0}]", oArgValues.ResultFile), "PROC", 0);

                        System.Collections.IEnumerator imgEnumerator = oKLARFParser.m_arrImgSourceList.GetEnumerator();
                        while (imgEnumerator.MoveNext())
                        {
                            strArchiveFiles[iFiles++] = imgEnumerator.Current.ToString();
                            Logging.WriteLog(string.Format("[ARCHIVE BACKUP] | [IMAGE : {0}]", imgEnumerator.Current.ToString()), "PROC", 0);
                        }
                        imgEnumerator.Reset();

                    }
                    else
                    {
                        strArchiveFiles = new string[1] { oArgValues.ResultFile };
                    }

                    // Zip Make

                    DACrux.Utility.ZipUtil.Compress(strArchiveFileName, Path.GetDirectoryName(oArgValues.ResultFile), strArchiveFiles, SharpCompress.Common.ArchiveType.Zip);
                    //ZipArchive oArchive = new ZipArchive();
                    //oArchive.CreateArchive(strArchiveFileName, oArgValues.ResultFile, strArchiveFiles, true);

                    Logging.WriteLog(string.Format("[BACKUP] | [File Name {0}]", strArchiveFileName), "PROC", 0);

                }
                ///----------------------------------------------------------------------------------


                /// File을 Processing으로 Move한다.
                /// =================================================================================
                if (!Directory.Exists(oArgValues.ProcessPath))
                {
                    Directory.CreateDirectory(oArgValues.ProcessPath);
                    Logging.WriteLog(string.Format("[CREATE_PATH] | [PROCESSING_PATH] : [{0}]", oArgValues.ResultFile), "PROC", 0);
                }

                //Move로 변경 해야 한다.
                //File.Move(oArgValues.ResultFile, oArgValues.ProcessFile);
                File.Copy(oArgValues.ResultFile, oArgValues.ProcessFile);
                Logging.WriteLog(string.Format("[PROCESSING] | [FILE_MOVE] : [{0} to {1}]", oArgValues.ResultFile, oArgValues.ProcessFile), "PROC", 0);
                ///----------------------------------------------------------------------------------


                /// ProcessMonitoring 을 하기 위해 SENDDATA를 한다.
                /// =================================================================================
                Logging.WriteLog(string.Format("PARSER,START,{0},{1},{2},{3},{4}"
                    , oArgValues.ResultEquipID
                    , oKLARFParser.LOT_ID
                    , oKLARFParser.WAFER_ID
                    , oKLARFParser.STEP_ID
                    , oArgValues.KLARFFileName), "SENDDATA", 0);
                /// Data를DB에 저장한다.
                oKLARFParser.CreateUpdateLog("NFME", oArgValues.ResultEquipID, oArgValues.TransTimeUseDB);
                ///==================================================================================



                /// File을 Processing하고 DB에 Upload한다.
                /// =================================================================================

                string strCmnFile = string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}{6}", oArgValues.CommonServicePath
                    , oKLARFParser.EQUIP_ID
                    , oKLARFParser.LOT_ID
                    , oKLARFParser.WAFER_ID
                    , oKLARFParser.STEP_ID
                    , oArgValues.TransTime
                    , Path.GetExtension(oArgValues.ResultFile));

                string strSrvFile = string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}{6}", oArgValues.ServicePath
                    , oKLARFParser.EQUIP_ID
                    , oKLARFParser.LOT_ID
                    , oKLARFParser.WAFER_ID
                    , oKLARFParser.STEP_ID
                    , oArgValues.TransTime
                    , Path.GetExtension(oArgValues.ResultFile));

                oKLARFParser.ReadKLARF();
                Logging.WriteLog(string.Format("[PROCESSING] | [PROC_END]"), "PROC", 0);

                //oKLARFParser.UpdateStepSummary();

                /// File을 Service한다.
                /// =================================================================================
                if (oKLARFParser.COMMON_SERVICE)
                {
                    if (!Directory.Exists(oArgValues.CommonServicePath)) Directory.CreateDirectory(oArgValues.CommonServicePath);
                    File.Copy(oArgValues.ProcessFile, strCmnFile);
                    Logging.WriteLog(string.Format("[COMMON SERVICE] | [FILE_COPY] | [{0} to {1}]", oArgValues.ProcessFile, strCmnFile), "PROC", 0);
                }

                if (oArgValues.ServicePath != null && oKLARFParser.EQ_SERVICE)
                {
                    if (!Directory.Exists(oArgValues.ServicePath)) Directory.CreateDirectory(oArgValues.ServicePath);
                    File.Move(oArgValues.ProcessFile, strSrvFile);
                    Logging.WriteLog(string.Format("[EQUIP SERVICE] | [FILE_MOVE] | [{0} to {1}]", oArgValues.ProcessFile, strSrvFile), "PROC", 0);
                }

                if (File.Exists(oArgValues.ProcessFile)) File.Delete(oArgValues.ProcessFile);
                Logging.WriteLog(string.Format("[EQUIP SERVICE] | [FILE_DELETE] | [{0}]", oArgValues.ProcessFile), "PROC", 0);
                ///----------------------------------------------------------------------------------
                ///

                /// Backup File을 Maint한다.
                /// =================================================================================
                BackupManagement(oArgValues.BackupPath, oKLARFParser.BACKUPFILELIFEDATE);
                ///----------------------------------------------------------------------------------
                ///
            }
            catch (Exception ex)
            {
                if (oArgValues.ProcessFile != null && File.Exists(oArgValues.ProcessFile))
                {
                    File.Move(oArgValues.ProcessFile, oArgValues.ErrorFile);
                    Logging.WriteLog(string.Format("[MOVE_TO_FILE] | [FILE_MOVE] | [{0} to {1}]", oArgValues.ProcessFile, oArgValues.ErrorFile), "ERROR", 0);

                    if (oKLARFParser.m_arrImgSourceList != null && oKLARFParser.m_arrImgSourceList.Count > 0)
                    {
                        System.Collections.IEnumerator imgEnumerator = oKLARFParser.m_arrImgSourceList.GetEnumerator();
                        while (imgEnumerator.MoveNext())
                        {
                            strSourceImage = string.Format(@"{0}\{1}", oArgValues.ResultPath, imgEnumerator.Current);
                            strBackupImage = string.Format(@"{0}\{1}_{2}{3}", oArgValues.ErrorPath, Path.GetFileNameWithoutExtension(imgEnumerator.Current.ToString())
                                , oArgValues.TransTime
                                , Path.GetExtension(imgEnumerator.Current.ToString()));

                            if (!File.Exists(strSourceImage))
                            {
                                strSourceImage = string.Format(@"{0}\{1}", oArgValues.ProcessPath, imgEnumerator.Current);
                                if (!File.Exists(strSourceImage)) continue;
                            }

                            File.Move(strSourceImage, strBackupImage);
                            Logging.WriteLog(string.Format("[MOVE_TO_FILE] | [IMAGE] : [{0} to {1}]", strSourceImage, strBackupImage), "PROC", 0);
                        }
                        imgEnumerator.Reset();
                    }
                }
                else
                {
                    Logging.WriteLog(ex.Message, "ERROR", 0);
                }

                Logging.WriteLog(string.Format("PARSER,ERROR,{0},{1},{2},{3},{4}"
                    , oArgValues.ResultEquipID
                    , oKLARFParser.LOT_ID
                    , oKLARFParser.WAFER_ID
                    , oKLARFParser.STEP_ID
                    , oArgValues.KLARFFileName), "SENDDATA", 0);


                /// ProcessMonitoring 을 하기 위해 SENDDATA를 한다.
                /// =================================================================================

                DateTime oDtNow = DateTime.Now;
                strEndTime = oDtNow.ToString("yyyy-MM-dd HH:mm:ss");

                /// Process Monitoring을 하기 위해 Data를 저장한다.
                /// ===========================================================================================
                oKLARFParser.UpdateUpdateLog(oArgValues.ResultEquipID, oArgValues.TransTimeUseDB, strEndTime, strEndTime, oKLARFParser.DataIntegrateType, "0001"
                    , -1, oArgValues.ResultFile, oArgValues.ErrorFile, oArgValues.BackupFile, 0, 0);
                ///----------------------------------------------------------------------------------

            }

            finally
            {
                Logging.SocketClose();
                Logging.End();
            }
        }
        #endregion

        #region ◎ WB3200 Parsing Logic
        public static void Main_WB3200(DACrux.Base.ARGUMENT_TAG oArgValues)
        {
            string strBackupImage = string.Empty;
            string strSourceImage = string.Empty;
            string strEndTime = string.Empty;
            WB3200 oWB3200Parser = null;

            try
            {
                ///// 1.현재의 File이 처리 가능한 File인지 Check
                ///// 2.KLARF에 Image Name이 있다면 이 Image가 전부 존재하는지 Check
                ///// =================================================================================
                //if (!oWB3200Parser.PreParsingWB3200(oArgValues.ResultFile))
                //{
                //Logging.SocketOpen(oWB3200Parser.UPDATESERVERIP, oWB3200Parser.SOCKETPORT);
                //Logging.WriteLog(string.Format("SKEEP,{0}", oArgValues.ResultFile), "SENDDATA", 0);
                //    return; //위 두 조건에 만족하지 않으면 아무런 Action을 취하지 않고 Return
                //}
                /////----------------------------------------------------------------------------------

                /// ProcessMonitoring 을 하기 위해 SENDDATA를 한다.
                /// =================================================================================
                /// 
                Logging.WriteLog(string.Format("UNZIP,START,{0},,,", oArgValues.KLARFFileName), "SENDDATA", 0);

                /// Data를DB에 저장한다.
                ///==================================================================================

                Logging.WriteLog("###File path config################################################", "PROC", 0);
                Logging.WriteLog(string.Format("###[RESULT   ] [{0}]", oArgValues.ResultFile), "PROC", 0);
                Logging.WriteLog(string.Format("###[BACKUP   ] [{0}]", oArgValues.BackupFile), "PROC", 0);
                Logging.WriteLog(string.Format("###[SERVICE.1] [{0}]", oArgValues.ServiceFile), "PROC", 1);
                Logging.WriteLog(string.Format("###[DBCOMMAND] [{0}]", oArgValues.UploadComand), "PROC", 1);
                Logging.WriteLog(string.Format("###[COMMAND  ] [{0}]", string.Join(" ", args_value)), "PROC", 0);
                Logging.WriteLog("###################################################################", "PROC", 0);
                ///----------------------------------------------------------------------------------

                if (oArgValues.ResultFile != null && !File.Exists(oArgValues.ResultFile))
                {
                    Logging.WriteLog(string.Format("[PROCESSING] | [FILE_NOT_FOUND] : [{0}]", oArgValues.ResultFile), "PROC", 0);
                    ConsolMessage();
                    return;
                }

                if (oArgValues.ResultFile == null)
                {
                    ConsolMessage();
                    return;
                }

                Logging.WriteLog(string.Format("[PROCESSING] | [PROC_START] : [{0}]", oArgValues.ResultFile), "PROC", 0);


                /// File을 Backup한다.
                /// =================================================================================
                if (oArgValues.BackupPath != null && oArgValues.BackupPath.Length > 0)
                {
                    if (!Directory.Exists(oArgValues.BackupPath))
                    {
                        Directory.CreateDirectory(oArgValues.BackupPath);
                        Logging.WriteLog(string.Format("[CREATE_PATH] | [BACKUP_PATH] : [{0}]", oArgValues.BackupPath), "PROC", 0);
                    }

                    if (!Directory.Exists(oArgValues.ArchivePath))
                    {
                        Directory.CreateDirectory(oArgValues.ArchivePath);
                        Logging.WriteLog(string.Format("[CREATE_PATH] | [BACKUP_PATH] : [{0}]", oArgValues.ArchivePath), "PROC", 0);
                    }

                    string strArchiveFileName = string.Format(@"{0}\{1}_{2}{3}", oArgValues.ArchivePath
                                                                               , Path.GetFileNameWithoutExtension(oArgValues.ResultFile)
                                                                               , oArgValues.TransTime
                                                                               , Path.GetExtension(oArgValues.ResultFile));

                    File.Copy(oArgValues.ResultFile, strArchiveFileName);
                    Logging.WriteLog(string.Format("[BACKUP] | [File Name {0}]", strArchiveFileName), "PROC", 0);
                }
                ///----------------------------------------------------------------------------------


                /// File을 Processing으로 Move한다.
                /// =================================================================================
                if (!Directory.Exists(oArgValues.ProcessPath))
                {
                    Directory.CreateDirectory(oArgValues.ProcessPath);
                    Logging.WriteLog(string.Format("[CREATE_PATH] | [PROCESSING_PATH] : [{0}]", oArgValues.ResultFile), "PROC", 0);
                }

                File.Move(oArgValues.ResultFile, oArgValues.ProcessFile);
                Logging.WriteLog(string.Format("[PROCESSING] | [FILE_MOVE] : [{0} to {1}]", oArgValues.ResultFile, oArgValues.ProcessFile), "PROC", 0);
                ///----------------------------------------------------------------------------------

                string strFileRoot = Path.Combine(Path.GetDirectoryName(oArgValues.ProcessFile), Path.GetFileNameWithoutExtension(oArgValues.ProcessFile));

                // 압축푼다..
                DACrux.Utility.ZipUtil.Uncompress(oArgValues.ProcessFile, strFileRoot);
                //DACrux.Utility.ZipManager.UnZipFiles(oArgValues.ProcessFile, strFileRoot, null, false);
                //Uncompress(oArgValues.ProcessFile, strFileRoot);

                if (Directory.Exists(strFileRoot) == false)
                {
                    Logging.WriteLog(string.Format("[UNZIP] | [UNZIP] | [Can't find {0}]", strFileRoot), "ERROR", 0);
                }

                //string strResultRoot = Pa

                string[] strWaferFileList = Directory.GetFiles(strFileRoot, "D_Category*.csv", SearchOption.AllDirectories);


                for (int i = 0; i < strWaferFileList.Length; i++)
                {
                    try
                    {
                        oArgValues.SubProcessFile = strWaferFileList[i];

                        /// Logging을 시작한다. (LOG LEVEL = 0)
                        /// =================================================================================
                        Logging.Start(string.Format(@"{0}\verbose.log", oArgValues.LogPath)
                                     , string.Format(@"{0}\history.log", oArgValues.LogPath), oArgValues.LogLevel);

                        oWB3200Parser = new WB3200();
                        oWB3200Parser.WB3200_A(oArgValues);
                        oWB3200Parser.PreParsingWB3200(oArgValues.SubProcessFile);

                        if (i == 0)
                            Logging.SocketOpen(oWB3200Parser.UPDATESERVERIP, oWB3200Parser.SOCKETPORT);

                        oWB3200Parser.Read3200();
                        Logging.WriteLog(string.Format("[PROCESSING] | [WAFER_END]"), "PROC", 0);

                        oWB3200Parser.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(string.Format("[PROCESSING] | [PARSING] | [{0} : {1}]", strFileRoot, ex.Message), "ERROR", 0);
                    }
                }
                Logging.WriteLog(string.Format("[PROCESSING] | [PROC_END]"), "PROC", 0);

                /// File을 Processing하고 DB에 Upload한다.
                /// =================================================================================

                string strCmnFile = string.Format(@"{0}\{1}_{2}{3}", oArgValues.CommonServicePath
                                                                    , Path.GetFileNameWithoutExtension(oArgValues.ResultFile)
                                                                    , oArgValues.TransTime
                                                                    , Path.GetExtension(oArgValues.ResultFile));

                string strSrvFile = string.Format(@"{0}\{1}_{2}{3}", oArgValues.ServicePath
                                                                    , Path.GetFileNameWithoutExtension(oArgValues.ResultFile)
                                                                    , oArgValues.TransTime
                                                                    , Path.GetExtension(oArgValues.ResultFile));

                /// File을 Service한다.
                /// =================================================================================
                if (oWB3200Parser.COMMON_SERVICE && oArgValues.CommonServicePath.Length > 0)
                {
                    if (!Directory.Exists(oArgValues.CommonServicePath)) Directory.CreateDirectory(oArgValues.CommonServicePath);
                    File.Copy(oArgValues.ProcessFile, strCmnFile);
                    Logging.WriteLog(string.Format("[COMMON SERVICE] | [FILE_COPY] | [{0} to {1}]", oArgValues.ProcessFile, strCmnFile), "PROC", 0);
                }

                if (oArgValues.ServicePath != null && oArgValues.ServicePath.Length > 0 && oWB3200Parser.EQ_SERVICE)
                {
                    if (!Directory.Exists(oArgValues.ServicePath)) Directory.CreateDirectory(oArgValues.ServicePath);
                    File.Move(oArgValues.ProcessFile, strSrvFile);
                    Logging.WriteLog(string.Format("[EQUIP SERVICE] | [FILE_MOVE] | [{0} to {1}]", oArgValues.ProcessFile, strSrvFile), "PROC", 0);
                }

                File.Delete(oArgValues.ProcessFile);
                Directory.Delete(strFileRoot, true);

                Logging.WriteLog(string.Format("[EQUIP SERVICE] | [FILE_DELETE] | [{0}]", oArgValues.ProcessFile), "PROC", 0);
                ///----------------------------------------------------------------------------------


                /// Backup File을 Maint한다.
                /// =================================================================================
                BackupManagement(oArgValues.BackupPath, oWB3200Parser.BACKUPFILELIFEDATE);
                ///----------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                if (oArgValues.ProcessFile != null && File.Exists(oArgValues.ProcessFile))
                {
                    File.Move(oArgValues.ProcessFile, oArgValues.ErrorFile);
                    Logging.WriteLog(string.Format("[MOVE_TO_FILE] | [FILE_MOVE] | [{0} to {1}]", oArgValues.ProcessFile, oArgValues.ErrorFile), "ERROR", 0);

                    if (oWB3200Parser.m_arrImgSourceList != null && oWB3200Parser.m_arrImgSourceList.Count > 0)
                    {
                        System.Collections.IEnumerator imgEnumerator = oWB3200Parser.m_arrImgSourceList.GetEnumerator();
                        while (imgEnumerator.MoveNext())
                        {
                            strSourceImage = string.Format(@"{0}\{1}", oArgValues.ResultPath, imgEnumerator.Current);
                            strBackupImage = string.Format(@"{0}\{1}_{2}{3}", oArgValues.ErrorPath, Path.GetFileNameWithoutExtension(imgEnumerator.Current.ToString())
                                , oArgValues.TransTime
                                , Path.GetExtension(imgEnumerator.Current.ToString()));

                            if (!File.Exists(strSourceImage))
                            {
                                strSourceImage = string.Format(@"{0}\{1}", oArgValues.ProcessPath, imgEnumerator.Current);
                                if (!File.Exists(strSourceImage)) continue;
                            }

                            File.Move(strSourceImage, strBackupImage);
                            Logging.WriteLog(string.Format("[MOVE_TO_FILE] | [IMAGE] : [{0} to {1}]", strSourceImage, strBackupImage), "PROC", 0);
                        }
                        imgEnumerator.Reset();
                    }
                }
                else
                {
                    Logging.WriteLog(ex.Message, "ERROR", 0);
                }

                Logging.WriteLog(string.Format("PARSER,ERROR,{0},,,", oArgValues.KLARFFileName), "SENDDATA", 0);


                /// ProcessMonitoring 을 하기 위해 SENDDATA를 한다.
                /// =================================================================================

                DateTime oDtNow = DateTime.Now;
                strEndTime = oDtNow.ToString("yyyy-MM-dd HH:mm:ss");

                /// Process Monitoring을 하기 위해 Data를 저장한다.
                /// ===========================================================================================
                oWB3200Parser.UpdateUpdateLog(oArgValues.ResultEquipID, oArgValues.TransTimeUseDB, strEndTime, strEndTime, oWB3200Parser.DataIntegrateType, "0001"
                    , -1, oArgValues.ResultFile, oArgValues.ErrorFile, oArgValues.BackupFile, 0, 0);
                ///----------------------------------------------------------------------------------

            }

            finally
            {
                Logging.SocketClose();
                Logging.End();
            }
        }
        #endregion

        #region ■ BACKUP File을 TQD_CONFIG에 설정된 기간만 존재하게 하는 함수
        private static void BackupManagement(string Path, int Period)
        {
            DateTime oDtNow = DateTime.Now;
            DateTime oDtOld = oDtNow.AddDays(-1 * Period);
            string strDir = string.Empty;
            strDir = oDtOld.ToString("yyyy-MM-dd");
            try
            {
                Directory.Delete(strDir, true);
            }
            catch
            {
                /// Directory 지우다가 Error가 발생하지는 않을것 같음 
                /// 하지만 만에 하나 발생시 다른 작업에 영향이 없이 Exception을 무시함
                /// 할일없음
            }
        }
        #endregion

        #region ■ Consol에 Message Display.
        private static void ConsolMessage()
        {
            try
            {
                Console.WriteLine("\n\tKLARF File을 해석하여 DB에 Update합니다\n");
                Console.WriteLine("\tDataParser[.exe] -k (KLARF File Name) [-c IMPORT | INSERT ] [-t INSPECTION | OPT_REVIEW | SAM_REVIEW] [-s KLARF Service path] \n\n");
                Console.WriteLine("\t\t ?     Help");
                Console.WriteLine("\t\t-k     KLARF File Name Full Path");
                Console.WriteLine("\t\t-c     Default INSERT ,IMPORT is Upload CSV Format");
                Console.WriteLine("\t\t-s     Service Path of KLARF File");
                Console.WriteLine("\t\t-t     Result Type Review or Inspection");
                Console.WriteLine("\t\t-l     Process Logging Level");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                //작업사항 없음
            }
        }
        #endregion

        #region ■ Argument를 Parsing한다.
        /// <Summary>
        /// <b>■ Argument를 Parsing한다.</b><br>
        /// - 작  성  자 : 미라콤 임영신<br>
        /// - 주요변경로그<br>
        /// </Summary>
        /// <param name="KFileName">KLARF File Name</param>
        /// <returns>TargetFilePath</returns>		
        static void ParsingArgument(string[] ArgValues, ref DACrux.Base.ARGUMENT_TAG retArgValues)
        {
            try
            {
                for (int i = 0; i < ArgValues.Length; i = i + 2)
                {
                    switch (ArgValues[i].ToUpper())
                    {
                        //KLARF File Name을 Arg로 받기위한 Option Flag
                        case "-K":
                            retArgValues.ResultFile = ArgValues[i + 1];
                            break;

                        //Insert 방식을 Arg로 받기위한 Option Flag
                        case "-C":
                            if (ArgValues[i + 1].ToUpper() == "INSERT" || ArgValues[i + 1].ToUpper() == "IMPORT" || ArgValues[i + 1].ToUpper() == "CMD")
                            {
                                retArgValues.UploadComand = ArgValues[i + 1];
                            }
                            else
                            {
                                retArgValues.UploadComand = "INSERT";
                            }
                            break;

                        //Service Path를 Arg로 받기위한 Option Flag
                        case "-S":
                            retArgValues.ServicePath = ArgValues[i + 1];
                            break;

                        //장비의 Type을 Arg로 받기위한 Option Flag
                        case "-T":
                            if (ArgValues[i + 1].ToUpper() == "INSPECTION" || ArgValues[i + 1].ToUpper() == "OPT_REVIEW" || ArgValues[i + 1].ToUpper() == "SAM_REVIEW")
                            {
                                retArgValues.ResultType = ArgValues[i + 1];
                            }
                            else
                            {
                                retArgValues.ResultType = "INSPECTION";
                            }
                            break;

                        //장비의 Backup Path를 Arg로 받기위한 Option Flag
                        case "-B":
                            retArgValues.BackupPath = ArgValues[i + 1];
                            break;

                        case "-L":
                            retArgValues.LogLevel = int.Parse(ArgValues[i + 1]);
                            break;

                        case "-F":
                            retArgValues.FileFormat = ArgValues[i + 1].ToUpper();
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                //작업사항 없음
            }
        }
        #endregion
    }
}
