using System;
using System.Text;
using DACrux.Base;
using DACrux.Common.BSL;
using DACrux.Data.Handler;
using DACrux.Data.Parser;
using DACrux.Data.Parser.Klarf;
using DACrux.Framework.Server;
using DACrux.SEMDMS.BSL;
using System.IO;

namespace DACrux.SEMDMS.Review.DataService
{
    public partial class ReviewDataService : DataServiceBase
    {
        DMReview oReview;
        EquipManagement oEquipMng;

        public ReviewDataService()
        {
            InitializeComponent();

            oReview = new DMReview();
            oEquipMng = new EquipManagement();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");

            HandlerFactory.SetGarbageExtension(RemoveGarbageFileExtension);

#if SINGLE_EQUIP
            if (String.IsNullOrEmpty(EquipID))
                throw new Exception("EQUIP_ID 값이 설정되지 않았습니다.");

            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, EquipID, DataPath, oEquipMng.GetEquipInfo);
#else
            DACrux.Data.Handler.HandlerList handlerList = DACrux.Data.Handler.HandlerFactory.CreateInstance(Factory, DataPath, oEquipMng.GetEquipInfo);
#endif

            foreach (HandlerInsp handler in handlerList)
            {
                if (IsReqeustServiceStop)
                    break;

                handler.Run();

                long waferseq = long.MinValue;
                long stepseq = long.MinValue;

                foreach (ParserKlarf parser in handler.ParserList)
                {
                    string strStepID = parser.StepID;
                    try
                    {
                        if (IsReqeustServiceStop)
                            break;

                        if (parser.ErrorFlag)
                            throw new Exception(parser.ErrorMessage);
                        else
                            parser.ErrorBackupFlag = true;

                        StopWatch.Start();

                        WriteLog("DATA FILE", parser.FileName);

                        //FAB1 의 경우 Step ID 를 8자리로 사용한다.
                        //8자리가 아닌 경우 Setup 정보의 _이후 8자리를 사용 한다.
                        if (Factory == "FAB1" && parser.StepID.Length != 8)
                        {
                            //Ex) StepID "AMTDL_1439TRET"
                            string[] strStepInfo = parser.SetupID.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                            if (strStepInfo.Length != 2)
                                throw new Exception("FAB1 Setup ID 정의 Rule 이 틀립니다.");

                            strStepID = strStepInfo[1].Trim();
                        }

                        foreach (var wafer in parser.Wafers)
                        {
                            bool bFlag = oReview.GetInspectionInfo(
                                Factory,
                                parser.ResultTimestamp,
                                parser.DeviceID,
                                parser.LotID.ToUpper(),
                                wafer.WaferID.ToUpper(),
                                strStepID,
                                out waferseq,
                                out stepseq
                                );

                            if (!bFlag)
                            {
                                parser.ErrorFlag = true;
                                throw new Exception("Inspection 정보가 없습니다.");
                            }

                            string[,] arr = handler.GetImage2DArray(wafer, "REVIEW");

                            for (int idx = 0; idx < arr.GetLength(0); idx++)
                            {
                                arr[idx, (int)HandlerInsp.ImageCol.STEP_SEQ] = stepseq.ToString();
                                arr[idx, (int)HandlerInsp.ImageCol.WAFER_SEQ] = waferseq.ToString();
                            }

                            // TQD_DEFECT은 삭제하지 않고 계속 APPEND 한다.
                            //oReview.DeleteDefectImage(stepseq, waferseq);

                            if (arr != null && arr.GetLength(0) > 0)
                            {
                                oReview.InsertDefectImage(arr);
                            }

                            arr = new string[wafer.ImageDefectList.Count, 8];

                            for (int idx = 0; idx < wafer.ImageDefectList.Count; idx++)
                            {
                                int colIdx = 0;
                                arr[idx, colIdx++] = stepseq.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].DEFECTID.ToString();
                                arr[idx, colIdx++] = waferseq.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].CLASSNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].ROUGHBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].FINEBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].REVIEWSAMPLE.ToString();
                                arr[idx, colIdx++] = wafer.ImageDefectList[idx].IMAGECOUNT.ToString();
                            }

                            oReview.SaveDefect(
                                stepseq,
                                waferseq,
                                wafer.ImageDefectList.Count,
                                handler.EquipInfo.EquipID,
                                Path.GetFileName(parser.FileName),
                                parser.FtpRelativePath,
                                arr
                                );

                            arr = new string[wafer.ClassifedDefectList.Count, 7];

                            for (int idx = 0; idx < wafer.ClassifedDefectList.Count; idx++)
                            {
                                int colIdx = 0;
                                arr[idx, colIdx++] = stepseq.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].DEFECTID.ToString();
                                arr[idx, colIdx++] = waferseq.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].CLASSNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].ROUGHBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].FINEBINNUMBER.ToString();
                                arr[idx, colIdx++] = wafer.ClassifedDefectList[idx].REVIEWSAMPLE.ToString();
                            }

                            oReview.SaveDefect(
                                stepseq,
                                waferseq,
                                arr
                                );
                        }

                        // TQD_DEFECT의 IMAGECOUNT 업데이트
                        oReview.UpdateImageCount(stepseq);

                        // TQD_REVIEW_SUM 데이터 생성
                        oReview.InsertReviewSum(stepseq);

                        // 백업
                        handler.Backup(parser);

                        string[] copyPathArray = GetAdditionalCopyPathArray();

                        // 추가로 데이터 및 원본 이미지 파일을 복사해야 할 경우
                        if (copyPathArray != null && copyPathArray.Length > 0)
                        {
                            try
                            {
                                foreach (string copyPath in copyPathArray)
                                    handler.AddtionalCopy(parser, copyPath);
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 진행한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }

                        // KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // 설비 별로 임시로 데이터 및 원본 이미지 파일을 복사해야 할 경우 2019.10.24
                        if (!String.IsNullOrWhiteSpace(handler.EquipInfo.Grp01))
                        {
                            try
                            {
                                handler.AddtionalCopy(parser, handler.EquipInfo.Grp01);
                            }
                            catch (Exception ex)
                            {
                                // 추가 복사 오류가 발생하더라도 그대로 처리한다.
                                AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                                WriteLog(ex);
                            }
                        }
                        // END : KLARITY 임시 로직 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                        // 원본 삭제
                        handler.Remove(parser);

                        StopWatch.Stop();

                        WriteLog("SAVE", String.Format("FileName={0}, EquipID = {1} LotID={2}, WaferID={3}, Device={4}, ExecuteTime={5}",
                            Path.GetFileName(parser.FileName), handler.EquipInfo.EquipID, parser.LotID, parser.Wafers, parser.DeviceID, StopWatch.ExecuteTime));

                        if (!parser.ErrorFlag)
                            AppendServiceLog(ActionType.SUCCESS, null, null, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name, StopWatch.TotalMilliseconds);
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

                                AppendServiceLog(ex, strBacupPath, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                            }
                        }
                        else
                        {
                            AppendServiceLog(ex, parser.FileName, handler.EquipInfo.EquipID, parser.LotID, parser.Wafers.ToString(), handler.Name);
                        }
                    }
                    finally
                    {
                        if (parser != null && parser.ImageManagerList != null && parser.ImageManagerList.HasData())
                            parser.ImageManagerList.Dispose();
                    }
                }
            }
        }

        private string[] GetDefectColumnsOrder()
        {
            return new string[] { 
                "CLASSNUMBER", // 0
                "ROUGHBINNUMBER",
                "FINEBINNUMBER",
                "REVIEWSAMPLE",
                "IMAGECOUNT",
                "WAFER_SEQ", // 5
                "STEP_SEQ",  // 6
                "DEFECTID"
            };
        }
    }
}
