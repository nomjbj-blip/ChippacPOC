using System;
using System.Data;
using System.IO;
using DACrux.Data.Handler;
using DACrux.Data.Parser;
using DACrux.TEST.BSL;

namespace DACrux.TEST.CP.DataService
{
    public partial class CPDataService : DACrux.Framework.Server.DataServiceBase
    {
        public CPDataService()
        {
            InitializeComponent();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");
            DACrux.Common.BSL.EquipManagement obj = new Common.BSL.EquipManagement();

            HandlerFactory.SetGarbageExtension(RemoveGarbageFileExtension);

#if SINGLE_EQUIP
            if (String.IsNullOrEmpty(EquipID))
                throw new Exception("EQUIP_ID 값이 설정되지 않았습니다.");

            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, EquipID, DataPath, obj.GetEquipInfo);
#else
            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, DataPath, obj.GetEquipInfo);
#endif

            TestCommon tst = new TestCommon();
            string deviceAlias = "NONE";
            string strDevice = "NONE";
            string strCustomer = string.Empty;
            int iMapConfig = -1;
            DataTable dtLotSTS = null;
            DateTime dtStartTime;
            DateTime dtEndTime;

            foreach (HandlerCp handler in handlerList)
            {
                if (IsReqeustServiceStop)
                    break;

                // 데이터 파싱 처리
                handler.LogMethod = WriteLog;
                handler.Run();

                foreach (ParserCp parser in handler.ParserList)
                {
                    try
                    {
                        if (IsReqeustServiceStop)
                            break;

                        if (parser.ErrorFlag)
                            throw new Exception(parser.ErrorMessage);
                        else
                            parser.ErrorBackupFlag = true;
                        
                        if (String.IsNullOrEmpty(parser.LotID))
                            throw new NullReferenceException("LotID가 null 입니다.");

                        WriteLog(string.Format("Start - Lot ID : {0}, WaferID : {1}", parser.LotID, parser.WaferID));

                        StopWatch.Start();

                        if (DateTime.Equals(parser.StartTime, DateTime.MinValue))
                            dtStartTime = obj.SelectSysdate();
                        else
                            dtStartTime = parser.StartTime;

                        if (DateTime.Equals(parser.EndTime, DateTime.MinValue))
                            dtEndTime = obj.SelectSysdate();
                        else
                            dtEndTime = parser.EndTime;

                        //TQC_LOT_STS 상의 Lot 기준 Device Alias 정보를 가져 온다.
                        //7자리 -> 6자리로 확인 후 없으면 Fail
                        dtLotSTS = tst.GetLotStatusInfoToTable(parser.LotID);

                        deviceAlias = "NONE";
                        strDevice = "NONE";
                        strCustomer = string.Empty;

                        if (dtLotSTS != null && dtLotSTS.Rows.Count > 0)
                        {
                            deviceAlias = dtLotSTS.Rows[0]["MASK_ID"].ToString();
                            strDevice = dtLotSTS.Rows[0]["MAT_ID"].ToString();
                            strCustomer = dtLotSTS.Rows[0]["CUSTOMER"].ToString();
                        }

                        // 1) 기준정보 저장
                        if (!String.IsNullOrEmpty(deviceAlias) && !String.Equals(deviceAlias, "NONE"))
                            tst.InsertProductInfo(Factory, deviceAlias, strDevice);
                        tst.InsertProgramInfo(Factory, parser.ProgramName, parser.TestArea, deviceAlias);

                        // 2) TQP_MAPCFG에 MAP 보정 정보가 있는지 1확인 후 보정한다.
                        //    TQC_EQUIP의 EQUIP_CMF_1가 MAP_CONFIG_ENABLE 로 정의 되어 있을 경우만 보정 한다.
                        //    해당 기능 자체가 전부다 사용하는것은 아님.
                        if (handler.EquipInfo.Cmf01 == "MAP_CONFIG_ENABLE")
                        {
                            int rotate, shiftX, shiftY;
                            string Dir;
                            bool bPST = false;

                            if (System.IO.Path.GetExtension(parser.FileName).ToUpper() == ".PST" ||
                                System.IO.Path.GetExtension(parser.FileName).ToUpper() == ".CP4" || 
                                System.IO.Path.GetExtension(parser.FileName).ToUpper() == ".CP3")
                                bPST = true;
                            else
                                bPST = false;

                            tst.GetCorrectionData(parser.TestArea, deviceAlias, parser.ProgramName, out Dir, out rotate, out shiftX, out shiftY, out iMapConfig, bPST);

                            parser.SetCorrection(Dir, rotate, shiftX, shiftY);
                        }

                        // 3) WAFER & LOT 데이터 저장
                        string lotSeq, waferSeq;
                        int programRev = tst.GetMaxProgramRev(Factory, parser.ProgramName);

                        tst.SaveWaferData(
                            Factory,
                            parser.TestArea,
                            strDevice,
                            deviceAlias,
                            parser.LotID,
                            parser.WaferID,
                            dtStartTime.ToString(Handler.TO_DATE_FORMAT),
                            dtEndTime.ToString(Handler.TO_DATE_FORMAT),
                            parser.WaferDiameter,
                            parser.Tester == null ? handler.EquipInfo.EquipID : parser.Tester,
                            parser.Prober,
                            parser.ProbeCard,
                            parser.Operator,
                            parser.ProgramName,
                            programRev,
                            parser.DieDataList.Count,
                            null,
                            parser.MaxX,
                            parser.MaxY,
                            0.ToString(),
                            null,
                            null,
                            parser.FtpRelativePath,
                            Path.GetFileName(parser.FileName),
                            parser.TFVersion,
                            null,
                            null,
                            parser.DieDataList.Count,
                            out lotSeq,
                            out waferSeq);

                        // 4) 파라미터 데이터 저장
                        // 데이터를 2차원 배열로 가져오기
                        DataTable dt = handler.GetDataToDataTable(parser);

                        TestDataManager.SaveData(Factory, parser.ProgramName, dt, waferSeq);

                        using (TestCommon testCommon = new TestCommon())
                        {
                            testCommon.SummaryData(
                                Factory,
                                lotSeq,
                                waferSeq,
                                parser.WaferID
                                );
                        }

                        // 5)Wafer Sum 및 Lot Sum 의 기준정보 Lot Status 정보로 Update
                        tst.SummaryUpdateLotStatus(parser.LotID, waferSeq, lotSeq);

                        //Map config 정보가 있을 경우 TQP_WAFER_SUM, TQP_WAFER 에 Update 해준다.
                        if (iMapConfig > 0)
                        {
                            tst.WaferConfigSeqUpdate(waferSeq, iMapConfig.ToString());
                        }

                        // CP, Defect 매핑 데이터 생성
                        DieDefectMatching.InsertDefectDieData(waferSeq, parser.ProgramName);

                        // CP, Defect 매핑 데이터 기준으로 TQC_MATCH_SUM 생성
                        tst.InsertMatchSum(waferSeq);

                        StopWatch.Stop();

                        // 백업
                        handler.Backup(parser);

                        // 원본 삭제
                        handler.Remove(parser);

                        // 로그 저장
                        WriteLog("SAVE", String.Format("TestArea={0}, LotID={1}, WaferID={2}, Device={3}, Program={4}, LotSeq={5}, WaferSeq={6}, ExecuteTime={7}",
                            parser.TestArea, parser.LotID, parser.WaferID, strDevice, parser.ProgramName, lotSeq, waferSeq, StopWatch.ExecuteTime));

                        AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.WaferID, handler.Name, StopWatch.TotalMilliseconds);
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

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.WaferID, handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.WaferID, handler.Name);
                        }
                    }
                }
            }
        }

        private void SaveWaferData(ParserCp parser)
        {

        }
    }
}
