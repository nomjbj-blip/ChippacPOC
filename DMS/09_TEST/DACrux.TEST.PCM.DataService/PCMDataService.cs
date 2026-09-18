using System;
using System.Data;
using System.IO;
using DACrux.Common.BSL;
using DACrux.Data.Handler;
using DACrux.Data.Parser;
using DACrux.TEST.BSL;

namespace DACrux.TEST.PCM.DataService
{
    public partial class PCMDataService : DACrux.Framework.Server.DataServiceBase
    {
        private string deviceAlias = string.Empty;

        public PCMDataService()
        {
            InitializeComponent();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");
            EquipManagement obj = new EquipManagement();
            DACrux.Data.Handler.HandlerFactory.SetGarbageExtension(RemoveGarbageFileExtension);

#if SINGLE_EQUIP
            if (String.IsNullOrEmpty(EquipID))
                throw new Exception("EQUIP_ID 값이 설정되지 않았습니다.");

            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, EquipID, DataPath, obj.GetEquipInfo);
#else
            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, DataPath, obj.GetEquipInfo);
#endif

            TestCommon tst = new TestCommon();

            //--

            foreach (HandlerPcm handler in handlerList)
            {
                if (IsReqeustServiceStop)
                    break;

                WriteLog("Execute()", String.Format("{0}", String.Join(", ", handler.FileNames)));

                handler.LogMethod = WriteLog;
                handler.Run();

                foreach (ParserPcm parser in handler.ParserList)
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
                        deviceAlias = tst.GetLotStatusInfo(parser.LotID);

                        if (!String.IsNullOrEmpty(deviceAlias) && !String.Equals(deviceAlias, "NONE"))
                            tst.InsertProductInfo(Factory, deviceAlias, deviceAlias);

                        if (String.IsNullOrEmpty(parser.ProgramName))
                            throw new Exception("Program Name이 존재하지 않습니다.");

                        tst.InsertProgramInfo(
                            Factory,
                            parser.ProgramName,
                            handler.EquipInfo.Oper,
                            deviceAlias
                            );

                        int programRev = tst.GetMaxProgramRev(
                            Factory,
                            parser.ProgramName
                            );

                        ProcessingDefaultPcmData(
                            handler,
                            parser,
                            tst,
                            programRev
                            );

                        StopWatch.Stop();

                        /// 백업
                        WriteLog("BACKUP()");
                        handler.Backup(parser);

                        /// 삭제
                        WriteLog("REMOVE()");
                        handler.Remove(parser);

                        WriteLog(
                            "SAVE",
                            String.Format("Total ExecuteTime = {0}", StopWatch.ExecuteTime
                            ));

                        AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, parser.EquipID, parser.LotID, parser.WaferDataList.ToString(), handler.Name, StopWatch.TotalMilliseconds);
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

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.WaferDataList.ToString(), handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.WaferDataList.ToString(), handler.Name);
                        }
                    }
                }
            }
        }

        private void ProcessingDefaultPcmData(
            HandlerPcm handler,
            ParserPcm parser,
            TestCommon tst,
            int programRev
            )
        {
            /// 2) WaferDataList에 담겨있는 데이터 저장
            DataTable dt = null;
            string lotSeq = string.Empty;
            string waferSeq = string.Empty;
            string product = string.Empty;
            bool bIncreaseProbeCnt = true;
            int rotate = 0;
            int iMaxSite = 0;
            int iMaxX = 0;
            int iMaxY = 0;
            int.TryParse(parser.Notch, out rotate);

            if (!(parser is ParserPcm_Fab2_MEMS))
            {
                rotate = (360 - rotate) % 360;
                parser.SetCorrection(
                    rotate
                    );
            }
            // TQP_WAFER 테이블의 PROBE_CNT 증가 여부
            bIncreaseProbeCnt = string.Equals(handler.EquipInfo.Cmf01, "Y") ? true : false;
            product = tst.GetProductInfo(parser.LotID);

            for (int idx = 0; idx < parser.WaferDataList.Count; idx++)
            {
                iMaxSite = 0;
                /// 검사항목 Parameter Count
                for (int iDieNum = 0; iDieNum < parser.WaferDataList[idx].Count; iDieNum++)
                {
                    iMaxSite = Math.Max(iMaxSite, parser.WaferDataList[idx][iDieNum].Count);
                    iMaxX = Math.Max(iMaxX, Int32.Parse(parser.WaferDataList[idx][iDieNum].X));
                    iMaxY = Math.Max(iMaxX, Int32.Parse(parser.WaferDataList[idx][iDieNum].Y));
                }

                tst.SaveWaferData(
                    Factory,
                    handler.EquipInfo.Oper,
                    product,
                    deviceAlias,
                    parser.LotID,
                    parser.WaferDataList[idx].WaferID,
                    parser.StartTime.ToString(Handler.TO_DATE_FORMAT),
                    parser.EndTime.ToString(Handler.TO_DATE_FORMAT),
                    200,
                    parser.EquipID,
                    parser.ProbeID,
                    parser.ProbeCard,
                    parser.Operator,
                    parser.ProgramName,
                    programRev,
                    -1,
                    null,
                    iMaxX, // max x
                    iMaxY, // max y 
                    (parser is ParserPcm_Fab2_MEMS) ? parser.Notch : "0", // Fab2 MEMS에 대한 각도에 대해서만 저장
                    null,
                    parser.OverDrive,
                    parser.Temp,
                    parser.FtpRelativePath,
                    Path.GetFileName(parser.FileName),
                    iMaxSite.ToString(),
                    String.Empty,
                    null,
                    null,
                    parser.WaferDataList[idx].Count,
                    bIncreaseProbeCnt,
                    out lotSeq,
                    out waferSeq
                    );

                dt = handler.GetDataToDataTable(
                    parser.WaferDataList[idx]
                    );

                // 데이터 누락되는 건이 발생하여 로그 찍음
                object[] values = null;
                foreach (DataColumn col in dt.Columns)
                {
                    if (String.Equals(col.ColumnName, "X")
                        || String.Equals(col.ColumnName, "Y")
                        || String.Equals(col.ColumnName, "BIN"))
                        continue;

                    values = new object[dt.Rows.Count];
                    for (int rowidx = 0; rowidx < dt.Rows.Count; rowidx++)
                    {
                        values[rowidx] = dt.Rows[rowidx][col.ColumnName];
                    }

                    WriteLog("PROCESSING", String.Format("{0}: {1}", col.ColumnName, String.Join(",", values)));
                }

                /// 3) RawData 저장
                TestDataManager.SaveData(
                    Factory,
                    parser.ProgramName,
                    dt,
                    waferSeq,
                    bIncreaseProbeCnt
                    );

                //--

                /// 4) Summary Data 생성
                tst.SummaryData(
                    Factory,
                    lotSeq,
                    waferSeq,
                    parser.WaferDataList[idx].WaferID
                    );

                //--

                tst.UpdateParsingInfo(
                    Factory,
                    parser.EquipID,  //handler.EquipInfo.EquipID,
                    waferSeq
                    );

                //--

                // Die Defect 데이터 매칭 데이터 생성
                DieDefectMatching.InsertDefectDieData(waferSeq, parser.ProgramName);

                WriteLog(
                    "SAVE",
                    String.Format("TestArea={0}, EquipID = {1}, LotID={2}, WaferID={3}, Device={4}, Program={5}, LotSeq={6}, WaferSeq={7}",
                    handler.EquipInfo.Oper,
                    parser.EquipID, // handler.EquipInfo.EquipID,
                    parser.LotID,
                    parser.WaferDataList[idx].WaferID,
                    deviceAlias,
                    parser.ProgramName,
                    lotSeq,
                    waferSeq
                    ));
            }
        }
    }
}
