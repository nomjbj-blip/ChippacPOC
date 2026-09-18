using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace DACrux.TEST.AVI.DataService
{
    public partial class AVIDataService : DACrux.Framework.Server.DataServiceBase
    {
        enum TQP_FOI_DIE { WAFER_SEQ, DIE_NUM, TEST_AREA, DIEPROBE_CNT, X, Y, AVI_BIN, BIN_CHAR, IMAGE_PATH, THUMB_PATH, IMAGE_FILE_NAME, THUMB_FILENAME }

        public static readonly string TESTAREA = "AVI";
        public static readonly string TO_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static readonly int WAFERDIAMETER = 200;

        public AVIDataService()
        {
            InitializeComponent();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");

            string deviceAlias = string.Empty;

            string lotSeq = "-1";
            string waferSeq = "-1";
            string strLotID = string.Empty;
            string strWaferID = string.Empty;
            string[,] para = null;

            DateTime oTime = DateTime.Now;
            DataTable dtTemp = null;
            DataTable dtScopeRaw = null;

            System.IO.FileInfo oFile = null;
            System.IO.FileInfo oImage = null;

            DACrux.TEST.BSL.ProbeAdmin oProbe = new DACrux.TEST.BSL.ProbeAdmin();
            DACrux.TEST.BSL.TestCommon oTest = new DACrux.TEST.BSL.TestCommon();

            DACrux.Common.BSL.EquipManagement oEquipment = new Common.BSL.EquipManagement();
            DACrux.Data.Handler.HandlerFactory.SetGarbageExtension(RemoveGarbageFileExtension);
            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, DataPath, oEquipment.GetEquipInfo);

            foreach (DACrux.Data.Handler.HandlerAvi handler in handlerList)
            {
                // 데이터 파싱 처리
                //WriteLog("Step -", "Data Parsing Start");

                //FAV 별로 AVI Parsing 부분이 상이 하여 분기 한다.
                handler.FACTORY = Factory;
                handler.LogMethod = WriteLog;
                handler.Run();

                foreach (DACrux.Data.Parser.ParserAviMapFile parser in handler.ParserList)
                {
                    try
                    {
                        if (IsReqeustServiceStop)
                            break;

                        if (parser.ErrorFlag)
                            throw new Exception(parser.ErrorMessage);
                        else
                            parser.ErrorBackupFlag = true;

                        StopWatch.Start();

                        WriteLog("File Name", parser.FileName);

                        //실제 File 이 없을 경우 Skip
                        oFile = new System.IO.FileInfo(parser.FileName);
                        if (oFile.Exists == false)
                            continue;

                        strLotID = parser.LotID;
                        strWaferID = string.Format("{0}-{1:00}", strLotID, parser.WAFER);

                        //TQC_LOT_STS 상의 Lot 기준 Device Alias 정보를 가져 온다.
                        //7자리 -> 6자리로 확인 후 없으면 Fail
                        deviceAlias = oTest.GetLotStatusInfo(parser.LotID);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 01 : TQP_PRODUCT 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 01", "TQP_PRODUCT 확인 생성");
                        if (!String.IsNullOrEmpty(deviceAlias) && !String.Equals(deviceAlias, "NONE"))
                            oTest.InsertProductInfo(Factory, deviceAlias, deviceAlias);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 02 : TQP_PROGRAM 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 02", "TQP_PROGRAM 확인 생성");

                        //AVI 의 경우 Program Fix
                        oTest.InsertProgramInfo(Factory, TESTAREA, TESTAREA, TESTAREA);
                        //oTest.InsertProgramInfo(Factory, parser.DEVICE, TESTAREA, deviceAlias);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 03 : TQP_WAFER 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 03", "TQP_WAFER 확인 생성");
                        oTest.SaveWaferData(
                            Factory,
                            TESTAREA,
                            parser.DEVICE,
                            deviceAlias,
                            strLotID,
                            strWaferID,
                            oTime.ToString(TO_DATE_FORMAT), //oFile.CreationTime.ToString(TO_DATE_FORMAT),
                            oTime.ToString(TO_DATE_FORMAT), //oFile.CreationTime.ToString(TO_DATE_FORMAT),
                            WAFERDIAMETER,
                            handler.EquipInfo.EquipID,
                            "Unknown",
                            "Unknown",
                            "Parser",
                            TESTAREA,
                            0,
                            parser.DieList.Count,
                            "Unknown",
                            parser.INDEX_XMAX,
                            parser.INDEX_YMAX,
                            0.ToString(),
                            "Parsing",
                            null,
                            parser.FtpRelativePath,
                            Path.GetFileName(parser.FileName),
                            "-1",
                            "-1",
                            "Unknown",
                            parser.DieList.Count,
                            out lotSeq,
                            out waferSeq
                            );

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 04 : TQP_FOI_DIE 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 04", "TQP_FOI_DIE 확인 생성");

                        //AVI Data 를 넣기 전에 Scope Data 가 있는지 확인 하고 있으면 Scope Bin Data 를 우선으로 Merge 한다.
                        dtScopeRaw = oTest.SelectScopeRawData(strLotID, strWaferID);

                        para = new string[parser.DieList.Count, Enum.GetNames(typeof(TQP_FOI_DIE)).Length];
                        int iBin = -1;
                        for (int iDie = 0; iDie < parser.DieList.Count; iDie++)
                        {
                            //임의로 문자형은 99번 Bin으로 설정 한다.
                            if (int.TryParse(parser.DieList[iDie].BIN, out iBin) == false)
                                iBin = 99;

                            para[iDie, (int)TQP_FOI_DIE.WAFER_SEQ] = waferSeq.ToString();
                            para[iDie, (int)TQP_FOI_DIE.DIE_NUM] = (iDie + 1).ToString();
                            para[iDie, (int)TQP_FOI_DIE.TEST_AREA] = "AVI";
                            para[iDie, (int)TQP_FOI_DIE.DIEPROBE_CNT] = 0.ToString();
                            para[iDie, (int)TQP_FOI_DIE.X] = parser.DieList[iDie].X.ToString();
                            para[iDie, (int)TQP_FOI_DIE.Y] = parser.DieList[iDie].Y.ToString();
                            para[iDie, (int)TQP_FOI_DIE.AVI_BIN] = iBin.ToString();
                            para[iDie, (int)TQP_FOI_DIE.BIN_CHAR] = parser.DieList[iDie].BIN;
                            para[iDie, (int)TQP_FOI_DIE.IMAGE_PATH] = "";
                            para[iDie, (int)TQP_FOI_DIE.THUMB_PATH] = "";
                            para[iDie, (int)TQP_FOI_DIE.IMAGE_FILE_NAME] = "";
                            para[iDie, (int)TQP_FOI_DIE.THUMB_FILENAME] = "";

                            if (dtScopeRaw != null && dtScopeRaw.Rows.Count > 0)
                            {
                                DataRow[] drScope = dtScopeRaw.Select(string.Format("X = {0} AND Y = {1} AND BIN <> 0", parser.DieList[iDie].X, parser.DieList[iDie].Y));
                                if (drScope != null && drScope.Length > 0)
                                {
                                    para[iDie, (int)TQP_FOI_DIE.AVI_BIN] = drScope[0]["BIN"].ToString();
                                    para[iDie, (int)TQP_FOI_DIE.BIN_CHAR] = drScope[0]["BIN_CHAR"].ToString();
                                }
                            }

                            //Image File Backup 및 삭제 처리
                            if (parser.DieList[iDie].IMAGE != null && parser.DieList[iDie].IMAGE.Length > 0)
                            {
                                oImage = new FileInfo(parser.DieList[iDie].IMAGE[0].FullName);
                                if (oImage.Exists == true)
                                {
                                    oFile = new FileInfo(parser.DieList[iDie].IMAGE_BACKUP);
                                    if (oFile != null && oFile.Exists)
                                    {
                                        oFile.IsReadOnly = false;
                                        oFile.Delete();
                                    }

                                    //File.Delete(parser.DieList[iDie].IMAGE_BACKUP);
                                    oImage = oImage.CopyTo(parser.DieList[iDie].IMAGE_BACKUP, true);
                                    para[iDie, 8] = parser.FtpRelativePath.Substring(parser.FtpRelativePath.LastIndexOf("BACKUP/"));
                                    para[iDie, 10] = oImage.Name;
                                    if (oImage.Exists == true)
                                    {
                                        //모든 Image 삭제
                                        foreach (FileInfo of in parser.DieList[iDie].IMAGE)
                                        {
                                            of.Refresh();
                                            if (of != null && of.Exists)
                                            {
                                                of.IsReadOnly = false;
                                                of.Delete();
                                            }
                                        }

                                        //oImage = new FileInfo(parser.DieList[iDie].IMAGE[0].FullName);
                                        //oImage.Delete();
                                    }
                                }
                            }

                            //Thumenail Image File Backup 및 삭제 처리
                            if (string.IsNullOrEmpty(parser.DieList[iDie].THUMENAIL) == false)
                            {
                                oImage = new FileInfo(parser.DieList[iDie].THUMENAIL);
                                if (oImage != null && oImage.Exists == true)
                                {
                                    //기존 File 삭제 후 생성
                                    oFile = new FileInfo(parser.DieList[iDie].THUMENAIL_BACKUP);
                                    if (oFile != null && oFile.Exists)
                                    {
                                        oFile.IsReadOnly = false;
                                        oFile.Delete();
                                    }

                                    //File.Delete(parser.DieList[iDie].THUMENAIL_BACKUP);
                                    oImage = oImage.CopyTo(parser.DieList[iDie].THUMENAIL_BACKUP, true);
                                    para[iDie, 9] = parser.FtpRelativePath.Substring(parser.FtpRelativePath.LastIndexOf("BACKUP/"));
                                    para[iDie, 11] = oImage.Name;
                                    //기존 Thumnail file 삭제
                                    oImage.IsReadOnly = false;
                                    oImage.Delete();
                                    //File.Delete(parser.DieList[iDie].THUMENAIL);
                                }
                            }
                        }

                        //TQP_FOI_DIE Table Data 생성 (AVI Only)
                        oTest.DeleteFOIDiesMulti(waferSeq.ToString());
                        oTest.CreateFOIDiesMulti(para);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 05 : TQP_BINDESC 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 05", "TQP_FOI_DIE 확인 생성");
                        //TQP_BINDESC 에 정의되지 않을 경우 임의로 생성 해준다.
                        dtTemp = oTest.SelectFOIBinList(waferSeq);
                        foreach (DataRow dr in dtTemp.Rows)
                            oTest.SetBinDESC_BinInfo(TESTAREA, dr["BIN"].ToString(), dr["BIN_CHAR"].ToString(), "Parser Auto Create");

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 06 : TQP_WAFER_SUM 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 06", "TQP_WAFER_SUM 확인 생성");
                        oTest.SetAVIWaferSummary(waferSeq, lotSeq, strWaferID);

                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //Step 07 : TQP_LOT_SUM 확인 생성
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //WriteLog("Step 07", "TQP_LOT_SUM 확인 생성");
                        oTest.SetAVILotSummary(lotSeq);

                        StopWatch.Stop();

                        // 백업
                        handler.Backup(parser);

                        // 원본 삭제
                        handler.Remove(parser);

                        WriteLog("SAVE", String.Format("FileName={0}, LotID={1}, WaferID={2}, Device={3}, ExecuteTime={4}",
                            Path.GetFileName(parser.FileName), strLotID, strWaferID, parser.DEVICE, StopWatch.ExecuteTime));

                        AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.WAFER, handler.Name, StopWatch.TotalMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        // ErrorFlag를 설정하여 데이터 파일이 백업이나 삭제가 되지 않도록 한다.
                        parser.ErrorFlag = true;

                        WriteLog(ex);

                        FileInfo FileError = new FileInfo(parser.FileName);

                        //Error 발생된 File 의 경우 Error 경로에 넣는다.
                        if (parser.ErrorBackupFlag == true && FileError.Exists)
                        {
                            string strBacupPath = GetErrorFullPath(FileError.Name, handler.EquipInfo.EquipID);
                            if (string.IsNullOrEmpty(strBacupPath) == false)
                            {
                                //File 을 Error 경로에 넣는다.
                                try
                                {
                                    FileInfo FileBackup = new FileInfo(strBacupPath);

                                    try
                                    {
                                        if (FileBackup.Exists && FileBackup.IsReadOnly)
                                            FileBackup.IsReadOnly = false;
                                    }
                                    catch { }

                                    File.Copy(FileError.FullName, FileBackup.FullName, true);

                                    try
                                    {
                                        if (FileError.IsReadOnly)
                                            FileError.IsReadOnly = false;
                                    }
                                    catch { }

                                    FileError.Delete();
                                }
                                catch { }

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.WAFER, handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.WAFER, handler.Name);
                        }
                    }
                }
            }
        }
    }
}
