using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.Base;
using DACrux.Data.Parser;
using DACrux.Data.Parser.Klarf;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// 엔지니어가 화면에서 올린 KLARF 를 기존 DMS 자동 적재(Inspection.DataService)와 같은 TQD_* 테이블 / 같은 값 규칙으로 저장한다.
    /// 설계: 문서/2026-10-07_KLARF_Upload_DB적재_설계.md (CLAUDE-031, F08)
    ///
    /// 만든 이유: 기존 자동 적재는 설비 폴더 감시 서비스라 화면에서 바로 올릴 수 없고, 고객 샘플(XD106.02)은 기존 규칙
    ///            (SetupID 40자 / Step ID 8자리 규칙 / 이미지가 하위 폴더)으로는 저장되지 않는다.
    /// 처리 원칙:
    ///  - 파싱은 기존 Parser(ParserKlarf / ParserKlarf_18)를 그대로 쓰고, 저장은 기존 DefectDefine public 함수를 같은 프로세스에서 호출한다.
    ///    순서와 계산은 InspectionDataService.cs Step 1~12 와 같다(Step 13 Alarm 메일은 하지 않음).
    ///  - DataService 와 의도적으로 다른 점(설계 6.2절): Step ID 8자리 규칙 / LotID 앞 'A' 제거(FAB1 규칙) 미적용,
    ///    ANGLE 은 Parser 각도, SETUP_MAP 은 Test 마다 그 Test 의 SampleTestPlan, SetupID 는 size 128 쿼리(기존 쿼리는 40자로 잘림),
    ///    분류 기준 선택(D3: 기본 RoughBin -> CLASSNUMBER), 같은 키가 있으면 삭제 후 재등록(D1).
    ///  - 기존 DataService 처럼 문장마다 자동 Commit 이다. Wafer 저장 중 오류가 나면 이번에 새로 만든 STEP 의 행을 지운다.
    ///    SETUP / PRODUCT / LOT / WAFER 는 다른 STEP 이 함께 쓰는 행이라 남겨 두고 다음 저장에서 재사용한다.
    /// 이미지(구현 2단계, 사용자 결정 D4 = FTP 상대 경로): Service 설정 KLARF_IMAGE_ROOT(FTP 가 공개하는 폴더 아래) \ yyyyMMdd(저장일) \ {LotID} 에
    ///   KLARF 원본 / 이미지 원본 / 썸네일(thumb_)을 저장하고(HandlerInsp.Backup 과 같은 함수), DB 에는 드라이브 루트를 뺀 '/' 상대 경로
    ///   (Handler.GetImageRelativePath 와 같은 규칙)를 넣는다. 화면은 TQC_CONFIG DEFECT_FTP 의 MAIN_PATH + 이 경로로 받는다.
    /// </summary>
    public class KlarfUpload : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iKlarfUpload
    {
        /// <summary>KLARF 파일 최대 크기. Remoting 한 번에 받는 크기를 제한한다.</summary>
        private const int MaxKlarfBytes = 200 * 1024 * 1024;

        /// <summary>이미지 파일 1개 최대 크기(설계 11절).</summary>
        private const int MaxImageBytes = 50 * 1024 * 1024;

        /// <summary>DMS ID 컬럼 길이(TQD_LOT.LOT_ID / TQD_WAFER.WAFER_ID / TQD_STEP.STEP_ID / TQD_PRODUCT.PRODUCT / 설비 컬럼).</summary>
        private const int IdColumnLength = 40;

        /// <summary>TQD_SETUP.SETUP_ID 길이(2026-10-07 D2 DDL 로 128 확대).</summary>
        private const int SetupIdLength = 128;

        /// <summary>TQD_STEP.COMMENT 길이.</summary>
        private const int StepCommentLength = 500;

        /// <summary>업로드 데이터 구분 값. TQD_STEP.COMMENT 앞부분 / TQD_INSP_INFO.COMMENT. 이 값으로 거르는 기존 쿼리는 없다(설계 6.2절).</summary>
        private const string UploadCommentPrefix = "UPLOAD;";
        private const string InspInfoComment = "Upload";

        /// <summary>작업 폴더에 업로드 설정을 남기는 파일 이름(Begin 에서 쓰고 Commit 에서 읽음).</summary>
        private const string InfoFileName = "_upload_info.txt";

        /// <summary>
        /// ParserKlarf.GetOriginDieIndex 는 static 델리게이트라 파싱하는 동안만 업로드용 조회(size 128)로 바꿔 끼운다.
        /// 같은 Service 프로세스에서 동시에 두 업로드가 파싱하면 서로 덮어쓰므로 잠근다.
        /// </summary>
        private static readonly object ParseLock = new object();

        private static readonly Regex UploadKeyPattern = new Regex(@"^\d{17}_[0-9a-f]{8}$", RegexOptions.Compiled);

        /// <summary>UploadInfo 배열 위치(Interface 주석과 같은 순서).</summary>
        private enum UploadInfoIndex { FACTORY, USER, FILE_NAME, PRODUCT, STEP_ID, EQUIP, CLASS_SOURCE }

        /// <summary>DataService 의 TQD_STEP 배열 순서(InspectionDataService.cs 27행 enum 과 같음).</summary>
        private enum StepCol { TEST, WAFER_SEQ, STEP_ID, SLOT_ID, RESULT_ID, INSPECTION_EQ, REVIEW_EQ, SETUP_SEQ, RESULTTIMESTAMP, FILETIMESTAMP, MAIN_EQ, ROUTE, OPER, SCAN_AREA, DEFECTIVE_DIE, DEFECTS, IMAGES, DEFECT_DD, INSP_FILENAME, INSP_FILEPATH, COMMENT }

        /// <summary>DataService 의 TQD_DEFECT 배열 순서(InspectionDataService.cs 28행 enum 과 같음).</summary>
        private enum DefectCol { STEP_SEQ, DEFECTID, WAFER_SEQ, X, Y, XREL, YREL, XINDEX, YINDEX, XSIZE, YSIZE, DEFECTAREA, DSIZE, CLASSNUMBER, TEST, CLUSTERNUMBER, ROUGHBINNUMBER, FINEBINNUMBER, REVIEWSAMPLE, IMAGECOUNT, ADDER, FIRST_STEP, RETICLE_REPEAT_ID, DIE_REPEAT_ID, MAN_OPT_CLASS, AUTO_OPT_CLASS, MAN_SEM_CLASS, AUTO_SEM_CLASS }

        /// <summary>New Defect 허용오차(um). DataService 는 설비 EQUIP_CMF_6 값, 없으면 100 이다. 업로드는 설비 설정이 없으므로 기본값을 쓴다.</summary>
        private const double NewDefectTolerance = 100;

        public KlarfUpload()
        {
        }

        #region [KLARF 업로드]

        /// <summary>
        /// KLARF 를 작업 폴더에 받고 파싱해 Wafer 별 저장 예정 값 / 기존 등록 여부를 돌려준다. DB 는 조회만 한다.
        /// 처리 흐름: 입력 검증 -> 오래된 작업 폴더 정리 -> 작업 폴더 생성 / KLARF / 설정 저장 -> 파싱 -> 미리보기 표
        /// </summary>
        public DataTable BeginUpload(string[] UploadInfo, byte[] KlarfFile)
        {
            UploadSetting setting = UploadSetting.FromArray(UploadInfo);
            if (KlarfFile == null || KlarfFile.Length == 0)
                throw new Exception("KLARF 파일 내용이 없습니다.");
            if (KlarfFile.Length > MaxKlarfBytes)
                throw new Exception(String.Format("KLARF 파일이 너무 큽니다. ({0:N0} byte, 최대 {1:N0})", KlarfFile.Length, MaxKlarfBytes));

            CleanupOldWorkFolders();

            string key = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            string dir = GetWorkFolder(key);
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllBytes(Path.Combine(dir, setting.FileName), KlarfFile);
                File.WriteAllLines(Path.Combine(dir, InfoFileName), setting.ToArray(), Encoding.UTF8);

                ParserKlarf parser = ParseKlarf(Path.Combine(dir, setting.FileName));
                DataTable result = CreateResultTable(key);
                Dictionary<int, string> rawIds = ReadRawWaferIds(Path.Combine(dir, setting.FileName));
                foreach (Wafer wafer in parser.Wafers)
                    result.Rows.Add(BuildWaferRow(result, setting, parser, wafer, rawIds, true));
                return result;
            }
            catch
            {
                DeleteFolderQuietly(dir);
                throw;
            }
        }

        /// <summary>
        /// 이미지 파일 1개를 작업 폴더에 받는다. KLARF 와 같은 폴더에 두어야 기존 ImageMapper 가 Defect 에 연결한다(설계 6.3절, S4).
        /// 이미지 저장소 복사 / TQD_IMAGES 저장은 CommitUpload 에서 한다.
        /// </summary>
        public void AddImage(string strUploadKey, string strFileName, byte[] ImageFile)
        {
            string dir = GetExistingWorkFolder(strUploadKey);
            string name = SafeFileName(strFileName, "이미지 파일 이름");
            if (String.Equals(name, InfoFileName, StringComparison.OrdinalIgnoreCase))
                throw new Exception("사용할 수 없는 이미지 파일 이름입니다: " + name);
            UploadSetting setting = UploadSetting.Read(Path.Combine(dir, InfoFileName));
            if (String.Equals(name, setting.FileName, StringComparison.OrdinalIgnoreCase))
                throw new Exception("이미지 파일 이름이 KLARF 파일 이름과 같습니다: " + name);
            if (ImageFile == null || ImageFile.Length == 0)
                throw new Exception("이미지 파일 내용이 없습니다: " + name);
            if (ImageFile.Length > MaxImageBytes)
                throw new Exception(String.Format("이미지 파일이 너무 큽니다: {0} ({1:N0} byte, 최대 {2:N0})", name, ImageFile.Length, MaxImageBytes));

            File.WriteAllBytes(Path.Combine(dir, name), ImageFile);
        }

        /// <summary>
        /// 작업 폴더의 KLARF 를 다시 파싱해(이미지가 들어온 뒤라 ImageMapper 연결 결과가 Begin 과 다를 수 있음) Wafer 마다 저장한다.
        /// Wafer 1장이 실패해도 다음 Wafer 를 계속 저장하고, 결과 표의 RESULT / MESSAGE 로 알려 준다. 작업 폴더는 항상 지운다.
        /// </summary>
        public DataTable CommitUpload(string strUploadKey, string[] Options)
        {
            string dir = GetExistingWorkFolder(strUploadKey);
            ParserKlarf parser = null;
            try
            {
                UploadSetting setting = UploadSetting.Read(Path.Combine(dir, InfoFileName));
                bool overwrite = Options != null && Options.Length > 0 && String.Equals(Trim(Options[0]), "Y", StringComparison.OrdinalIgnoreCase);

                string klarfPath = Path.Combine(dir, setting.FileName);
                parser = ParseKlarf(klarfPath);
                Dictionary<int, string> rawIds = ReadRawWaferIds(klarfPath);

                // 이미지 / KLARF 원본을 저장소로 먼저 복사한다(파일 1벌을 Wafer 들이 함께 쓴다). 실패하면 DB 저장 전에 멈춘다.
                ImageStore store = SaveFilesToImageStore(parser, klarfPath);

                DataTable result = CreateResultTable(strUploadKey);
                foreach (Wafer wafer in parser.Wafers)
                {
                    DataRow row = BuildWaferRow(result, setting, parser, wafer, rawIds, false);
                    row["IMAGE_PATH"] = store.RelativePath;
                    result.Rows.Add(row);
                    SaveWafer(setting, parser, wafer, row, overwrite, store);
                }
                return result;
            }
            finally
            {
                // ImageMapper 의 ImageLoader 가 이미지 파일을 열고 있어 먼저 닫아야 작업 폴더를 지울 수 있다.
                if (parser != null && parser.ImageManagerList != null)
                    parser.ImageManagerList.Dispose();
                DeleteFolderQuietly(dir);
            }
        }

        /// <summary>저장하지 않고 작업 폴더를 지운다. 이미 없으면 아무 것도 하지 않는다.</summary>
        public void CancelUpload(string strUploadKey)
        {
            ValidateUploadKey(strUploadKey);
            DeleteFolderQuietly(GetWorkFolder(strUploadKey));
        }

        /// <summary>
        /// 업로드로 만든 STEP 1개의 데이터를 지운다. STEP COMMENT 가 'UPLOAD;' 로 시작하지 않으면(자동 적재 데이터) 지우지 않고 0 을 돌려준다.
        /// </summary>
        public int DeleteUploadedStep(string strStepSeq, string strWaferSeq, string strUser)
        {
            long stepSeq = RequiredNumber(strStepSeq, "STEP_SEQ");
            long waferSeq = RequiredNumber(strWaferSeq, "WAFER_SEQ");
            Required(strUser, "User", 30);

            TQD_STEP oTQD_STEP = new TQD_STEP();
            DataTable dt = oTQD_STEP.GetData("SELECT_STEP_KLARF_UPLOAD", null, new string[] { stepSeq.ToString(CultureInfo.InvariantCulture), waferSeq.ToString(CultureInfo.InvariantCulture) });
            try
            {
                if (dt.Rows.Count == 0)
                    return 0;
                string comment = dt.Rows[0]["STEP_COMMENT"] == DBNull.Value ? "" : dt.Rows[0]["STEP_COMMENT"].ToString();
                if (!comment.StartsWith(UploadCommentPrefix, StringComparison.Ordinal))
                    return 0;
            }
            finally
            {
                dt.Dispose();
            }

            DeleteStepRows(stepSeq, waferSeq);
            return 1;
        }

        #endregion

        #region [저장 - DataService Step 1~12]

        /// <summary>
        /// Wafer 1장을 저장한다. 순서는 InspectionDataService.cs 182~578행과 같다.
        /// 처리 흐름: 길이 검증 -> 기존 등록 확인(재등록이면 삭제 / 아니면 건너뜀) -> Step 1 SETUP -> 2 SETUP_MAP -> 3 PRODUCT -> 4 LOT -> 5 WAFER
        ///            -> 6 STEP -> 7 DEFECT(분류 기준 / New Defect) -> 8 INSP_INFO -> 9 TEST_ORDER -> 11 IMAGECOUNT -> 12 INSP_SUM
        /// 오류: 새로 만든 STEP 의 행을 지우고 row.RESULT = ERROR 로 남긴다(예외를 밖으로 던지지 않음).
        /// </summary>
        private void SaveWafer(UploadSetting setting, ParserKlarf parser, Wafer wafer, DataRow row, bool overwrite, ImageStore store)
        {
            DateTime started = DateTime.Now;
            DefectDefine oDefectDefine = new DefectDefine();

            string product = row["PRODUCT"].ToString();
            string stepId = row["STEP_ID"].ToString();
            string equip = row["EQUIP"].ToString();
            string resultTime = parser.ResultTimestamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            string setupTime = parser.SetupTimestamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            long setupSeq = -1;
            long waferSeq = -1;
            long stepSeq = -1;
            bool createdStep = false;

            try
            {
                if (row["RESULT"].ToString() == "INVALID")
                    throw new Exception(row["MESSAGE"].ToString());

                // 기존 등록 확인: DataService 와 같은 키 (WAFER_SEQ = LOT + WAFER_ID, STEP_ID, RESULTTIMESTAMP, TEST '1')
                long existingStep = FindExistingStep(oDefectDefine, product, parser.LotID, wafer.WaferID, stepId, resultTime, out waferSeq);
                if (existingStep >= 0)
                {
                    if (!overwrite)
                    {
                        SetResult(row, "SKIPPED", "이미 등록되어 건너뜀 (STEP_SEQ " + existingStep + ")", started);
                        row["STEP_SEQ"] = existingStep;
                        row["WAFER_SEQ"] = waferSeq;
                        return;
                    }
                    DeleteStepRows(existingStep, waferSeq);
                    row["DELETED_STEP_SEQ"] = existingStep;
                }

                #region [Step 01 : TQD_SETUP 확인 생성 (SETUP_ID size 128 쿼리)]
                TQD_SETUP oTQD_SETUP = new TQD_SETUP();
                setupSeq = SelectSetupSeq(oTQD_SETUP, parser.SetupID, parser.StepID, setupTime);
                if (setupSeq < 0)
                {
                    oTQD_SETUP.InsertData("CREATE_SETUP_INFO_KLARF_UPLOAD", null, new string[] {
                        parser.SetupID,                                   // SETUP_ID (KLARF 원문, 최대 128)
                        parser.StepID,                                    // STEP_ID (DataService 와 같이 KLARF StepID 원문)
                        setupTime,                                        // SETUP_TIME
                        Num(parser.GetAngle()),                           // ANGLE (Parser 각도, 설계 6.2절)
                        Num(parser.SampleSize * 1000),                    // WAFER_SIZE (um)
                        "N",                                              // NOTCH_TYPE (DataService 와 같음)
                        Num(parser.DiePitchX),                            // DIE_PITCH_X
                        Num(parser.DiePitchY),                            // DIE_PITCH_Y
                        Num(wafer.DieOriginX),                            // DIE_ORIGIN_X (좌하단 (1,1) 기준으로 옮긴 양)
                        Num(wafer.DieOriginY),                            // DIE_ORIGIN_Y
                        Num(wafer.SampleCenterLocationX),                 // ORIGIN_X
                        Num(wafer.SampleCenterLocationY) });              // ORIGIN_Y
                    setupSeq = SelectSetupSeq(oTQD_SETUP, parser.SetupID, parser.StepID, setupTime);
                    if (setupSeq < 0)
                        throw new Exception("TQD_SETUP 생성 실패");
                }
                row["SETUP_SEQ"] = setupSeq;
                #endregion

                #region [Step 02 : TQD_SETUP_MAP 확인 생성 (Test 마다 그 Test 의 SampleTestPlan)]
                // DataService 는 여러 Test 를 합친 목록(oIndex[ir])으로 배열을 만들어 Test 가 2개 이상이면 어긋날 수 있다. 업로드는 Test 별 계획을 그대로 쓴다.
                List<Point> allDies = new List<Point>();
                foreach (InspectionTest test in wafer.TestList)
                {
                    List<Point> testDies = new List<Point>();
                    foreach (Point pt in test.SampleTestPlan)
                    {
                        if (!testDies.Contains(pt)) testDies.Add(pt);
                        if (!allDies.Contains(pt)) allDies.Add(pt);
                    }

                    DataTable dtMap = oDefectDefine.GetSetupTestData(setupSeq.ToString(CultureInfo.InvariantCulture), test.TestNo.ToString(CultureInfo.InvariantCulture));
                    bool exists = dtMap != null && dtMap.Rows.Count > 0;
                    if (dtMap != null) dtMap.Dispose();
                    if (exists || testDies.Count == 0)
                        continue;

                    string[,] para = new string[testDies.Count, 4];
                    for (int i = 0; i < testDies.Count; i++)
                    {
                        para[i, 0] = setupSeq.ToString(CultureInfo.InvariantCulture);
                        para[i, 1] = test.TestNo.ToString(CultureInfo.InvariantCulture);
                        para[i, 2] = testDies[i].X.ToString(CultureInfo.InvariantCulture);
                        para[i, 3] = testDies[i].Y.ToString(CultureInfo.InvariantCulture);
                    }
                    oDefectDefine.CreateSetupMap(para);
                }
                #endregion

                #region [Step 03 : TQD_PRODUCT 확인 생성]
                DataTable dtProduct = oDefectDefine.GetProductInfo(product);
                bool productExists = dtProduct != null && dtProduct.Rows.Count > 0;
                if (dtProduct != null) dtProduct.Dispose();
                if (!productExists)
                {
                    oDefectDefine.CreateProduct(new string[] {
                        product,                                          // PRODUCT
                        setting.Factory,                                  // FACILITY (로그인 Factory)
                        "NONE",                                           // TECHNOLOGY
                        allDies.Count.ToString(CultureInfo.InvariantCulture), // NETDIE (DataService 와 같이 검사 Die 수)
                        Num(parser.SampleSize * 1000),                    // WAFER_SIZE
                        Num(parser.GetAngle()),                           // ANGLE
                        "N",                                              // NOTCH_TYPE
                        Num(parser.DiePitchX),                            // DIE_PITCH_X
                        Num(parser.DiePitchY),                            // DIE_PITCH_Y
                        Num(wafer.DieOriginX),                            // DIE_ORIGIN_X
                        Num(wafer.DieOriginY),                            // DIE_ORIGIN_Y
                        Num(wafer.SampleCenterLocationX),                 // ORIGIN_X
                        Num(wafer.SampleCenterLocationY),                 // ORIGIN_Y
                        "Upload" });                                      // DESCRIPTION
                }
                #endregion

                #region [Step 04 : TQD_LOT 확인 생성]
                string[] lotInfo = new string[] { parser.LotID, product, "Upload" };
                long lotSeq = oDefectDefine.GetLotSeq(lotInfo);
                if (lotSeq < 0)
                {
                    oDefectDefine.CreateLotInfo(lotInfo);
                    lotSeq = oDefectDefine.GetLotSeq(lotInfo);
                    if (lotSeq < 0)
                        throw new Exception("TQD_LOT 생성 실패");
                }
                #endregion

                #region [Step 05 : TQD_WAFER 확인 생성]
                string[] waferInfo = new string[] { lotSeq.ToString(CultureInfo.InvariantCulture), wafer.WaferID, wafer.Slot.ToString(CultureInfo.InvariantCulture), "Upload" };
                waferSeq = oDefectDefine.GetWaferSeq(waferInfo);
                if (waferSeq < 0)
                {
                    oDefectDefine.CreateWaferInfo(waferInfo);
                    waferSeq = oDefectDefine.GetWaferSeq(waferInfo);
                    if (waferSeq < 0)
                        throw new Exception("TQD_WAFER 생성 실패");
                }
                row["WAFER_SEQ"] = waferSeq;
                #endregion

                #region [Step 06 : TQD_STEP 생성]
                string[] stepInfo = new string[Enum.GetNames(typeof(StepCol)).Length];
                stepInfo[(int)StepCol.TEST] = "1";
                stepInfo[(int)StepCol.WAFER_SEQ] = waferSeq.ToString(CultureInfo.InvariantCulture);
                stepInfo[(int)StepCol.STEP_ID] = stepId;
                stepInfo[(int)StepCol.SLOT_ID] = wafer.Slot.ToString(CultureInfo.InvariantCulture);
                stepInfo[(int)StepCol.RESULT_ID] = parser.LotID;
                stepInfo[(int)StepCol.INSPECTION_EQ] = equip;
                // REVIEW_EQ 는 NOT NULL(DEFAULT ' ')이다. DataService 는 "" 를 넘기는데 Oracle 에서 "" = NULL 이라 ORA-01400 이 난다(2026-10-07 Probe 실측).
                // 기존 78행과 같은 값(공백 1자)을 넣는다.
                stepInfo[(int)StepCol.REVIEW_EQ] = " ";
                stepInfo[(int)StepCol.SETUP_SEQ] = setupSeq.ToString(CultureInfo.InvariantCulture);
                stepInfo[(int)StepCol.RESULTTIMESTAMP] = resultTime;
                stepInfo[(int)StepCol.FILETIMESTAMP] = parser.FileTimestamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                stepInfo[(int)StepCol.MAIN_EQ] = equip;
                stepInfo[(int)StepCol.ROUTE] = " ";
                stepInfo[(int)StepCol.OPER] = " ";
                stepInfo[(int)StepCol.SCAN_AREA] = "-1";
                stepInfo[(int)StepCol.DEFECTIVE_DIE] = "-1";
                stepInfo[(int)StepCol.DEFECTS] = "-1";
                stepInfo[(int)StepCol.IMAGES] = "0";
                stepInfo[(int)StepCol.DEFECT_DD] = "-1";
                stepInfo[(int)StepCol.INSP_FILENAME] = setting.FileName;
                stepInfo[(int)StepCol.INSP_FILEPATH] = store.RelativePath; // 이미지 저장소 FTP 상대 경로(D4). 저장소 미사용이면 ' '
                stepInfo[(int)StepCol.COMMENT] = BuildStepComment(setting, row["RAW_WAFER_ID"].ToString());

                oDefectDefine.CreateStepSeq(stepInfo);
                stepSeq = oDefectDefine.GetStepInfo(new string[] { waferSeq.ToString(CultureInfo.InvariantCulture), stepId, resultTime, "1" });
                if (stepSeq < 0)
                    throw new Exception("TQD_STEP 생성 실패");
                createdStep = true;
                row["STEP_SEQ"] = stepSeq;
                #endregion

                #region [Step 07 : TQD_DEFECT 생성 (분류 기준 적용 / New Defect)]
                ApplyClassSource(wafer.DefectList, setting.ClassSource);

                DataTable prevXY = oDefectDefine.GetLastestStepDefectXY(wafer.WaferID, stepId, parser.ResultTimestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                if (prevXY != null && prevXY.Rows.Count > 0)
                {
                    List<PointD> list = new List<PointD>();
                    foreach (DataRow r in prevXY.Rows)
                        list.Add(new PointD(System.Convert.ToDouble(r[0], CultureInfo.InvariantCulture), System.Convert.ToDouble(r[1], CultureInfo.InvariantCulture)));
                    DefectHelper.CalculateNewDefect(wafer.DefectList, list, NewDefectTolerance);
                }
                else
                {
                    foreach (Defect d in wafer.DefectList)
                        d.ADDER = 1;
                }
                if (prevXY != null) prevXY.Dispose();

                if (wafer.DefectList.Count > 0)
                    oDefectDefine.CreateDefect(BuildDefectArray(wafer.DefectList, stepSeq, waferSeq));

                int nDefDie = 0;
                DataTable dtDefect = oDefectDefine.GetDefectDataCount(waferSeq.ToString(CultureInfo.InvariantCulture), stepSeq.ToString(CultureInfo.InvariantCulture));
                if (dtDefect != null)
                {
                    if (dtDefect.Rows.Count > 0)
                        nDefDie = dtDefect.DefaultView.ToTable(true, "XY").Rows.Count;
                    dtDefect.Dispose();
                }
                #endregion

                #region [Step 08 : TQD_INSP_INFO 생성 (Test 마다 1행, 집계식은 DataService 와 같음)]
                foreach (InspectionTest test in wafer.TestList)
                {
                    DataTable dtInsp = oDefectDefine.GetInspInfoDuple(stepSeq.ToString(CultureInfo.InvariantCulture), test.TestNo.ToString(CultureInfo.InvariantCulture));
                    bool inspExists = dtInsp != null && dtInsp.Rows.Count > 0;
                    if (dtInsp != null) dtInsp.Dispose();
                    if (inspExists)
                        continue;

                    DefectList defects = new DefectList();
                    defects.AddRange(wafer.DefectList.FindAll(delegate(Defect d) { return d.TEST == test.TestNo; }));
                    int classified = defects.FindAll(delegate(Defect d) { return d.CLASSNUMBER > 0; }).Count;
                    double density = (double)((defects.Count / test.AreaPerTest) * 100000000);
                    double areaTest = (double)(test.AreaPerTest / 100000000);
                    DefectStat stat = defects.GetDefectStat();

                    // 기존 DefectDefine.CreateInspData(CREATE_INSP_INFO_PARSE)는 NOT NULL 컬럼 SUMMARY_FLAG 를 넣지 않아 ORA-01400 이 난다(Probe 실측).
                    // 같은 INSERT 에 SUMMARY_FLAG = ' ' 만 더한 추가 쿼리를 기존 DSL 범용 함수로 실행한다. 배열 순서는 CreateInspData 와 같다.
                    new TQD_INSP_INFO().InsertData("CREATE_INSP_INFO_KLARF_UPLOAD", null, new string[] {
                        stepSeq.ToString(CultureInfo.InvariantCulture),            // STEP_SEQ
                        waferSeq.ToString(CultureInfo.InvariantCulture),           // WAFER_SEQ
                        resultTime,                                                // RESULTTIMESTAMP
                        parser.FileTimestamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), // FILETIMESTAMP
                        setting.Factory,                                           // FACTORY
                        "NONE",                                                    // TECHNOLOGY
                        product,                                                   // PRODUCT
                        parser.LotID,                                              // LOT_ID
                        wafer.WaferID,                                             // WAFER_ID
                        stepId,                                                    // STEP_ID
                        test.TestNo.ToString(CultureInfo.InvariantCulture),        // TEST
                        wafer.Slot.ToString(CultureInfo.InvariantCulture),         // SLOT_ID
                        setupSeq.ToString(CultureInfo.InvariantCulture),           // SETUP_SEQ
                        DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), // STATUS (처리 시각, DataService 와 같음)
                        equip,                                                     // MAIN_EQ
                        "",                                                        // ROUTE
                        "",                                                        // OPER
                        equip,                                                     // INSPECTION_EQ
                        wafer.DefectHeaders != null ? String.Join(",", wafer.DefectHeaders.ToArray()) : "", // INSPECTION_PARAM
                        defects.Count.ToString(CultureInfo.InvariantCulture),      // DEFECTS
                        classified.ToString(CultureInfo.InvariantCulture),         // CLASSIFIED_DEFECTS
                        "0",                                                       // KILLER_DEFECTS
                        stat.Random.ToString(CultureInfo.InvariantCulture),        // RANDOM_DEFECTS
                        "0",                                                       // ADDER_KILLER_DEFECT
                        stat.NewRandom.ToString(CultureInfo.InvariantCulture),     // ADDER_RANDOM_DEFECT
                        stat.Cluster.ToString(CultureInfo.InvariantCulture),       // CLUSTERS
                        stat.NewCluster.ToString(CultureInfo.InvariantCulture),    // ADDER_CLUSTERS
                        Num(stat.AreaCluster),                                     // CLUSTER_AREA
                        Num(Math.Round(density, 5)),                               // DEFECT_DD
                        "0",                                                       // KILLER_DEFECT_DD
                        Num(stat.AreaRandom / areaTest),                           // RANDOM_DEFECT_DD
                        test.SampleTestPlan.Length.ToString(CultureInfo.InvariantCulture), // INSPECTED_DIE
                        nDefDie.ToString(CultureInfo.InvariantCulture),            // DEFECTIVE_DIE (DataService 와 같이 전체 Test 합산)
                        "0",                                                       // ADDER_DEF_DIE
                        "0",                                                       // KILL_DEF_DIE
                        "0",                                                       // KILL_ADDER_DEF_DIE
                        "0",                                                       // IMAGES (Step 10 이후 UPDATE_INSP_INFO[REVIEW_PARSER] 로 실제 수 갱신)
                        Num(Math.Round(areaTest, 5)),                              // SCAN_AREA
                        stat.New.ToString(CultureInfo.InvariantCulture),           // ADDER_DEFECTS
                        "0",                                                       // KILL_RND_DEFECTS
                        InspInfoComment,                                           // COMMENT
                        "",                                                        // RETICLE_ID
                        "",                                                        // INTERLOCK_FLAG
                        "" });                                                     // STEPPER_EQ
                }
                #endregion

                // Step 09 : 같은 Wafer / Step 의 TEST_ORDER +1 (새 STEP 이므로 항상 실행, 최신이 0)
                oDefectDefine.UpdateStepTestNo(waferSeq.ToString(CultureInfo.InvariantCulture), stepId);

                // Step 10 : TQD_IMAGES 생성 (저장소에 실제로 복사된 이미지만)
                string[,] images = BuildImageArray(wafer, stepSeq, waferSeq, store);
                if (images.GetLength(0) > 0)
                    new DMReview().InsertDefectImage(images);
                row["SAVED_IMAGES"] = images.GetLength(0);

                // Step 11 : TQD_DEFECT.IMAGECOUNT 를 TQD_IMAGES 기준으로 다시 계산
                oDefectDefine.UpdateImageCount(stepSeq);

                // DataService 는 INSP_INFO / STEP 의 IMAGES 를 0 으로 둬서 왼쪽 조회의 "Include Image"(IMAGES > 0)에 걸리지 않는다(설계 6.2절).
                // INSP_INFO 는 기존 Review 경로 쿼리(UPDATE_INSP_INFO[REVIEW_PARSER]: IMAGES / CLASSIFIED_DEFECTS 재계산)를 그대로 쓰고,
                // STEP 은 IMAGES 만 갱신하는 추가 쿼리를 쓴다(기존 UPDATE_STEP[REVIEW_PARSER] 는 REVIEW_* 칸까지 덮어씀).
                string stepText = stepSeq.ToString(CultureInfo.InvariantCulture);
                string waferText = waferSeq.ToString(CultureInfo.InvariantCulture);
                new TQD_INSP_INFO().UpdateDataNonQuery("UPDATE_INSP_INFO[REVIEW_PARSER]", null, new string[] { stepText, waferText });
                new TQD_STEP().UpdateDataNonQuery("UPDATE_STEP_IMAGES_KLARF_UPLOAD", null, new string[] { stepText, waferText });

                // Step 12 : TQD_INSP_SUM 생성
                oDefectDefine.CreateInspSum(stepSeq.ToString(CultureInfo.InvariantCulture));

                SetResult(row, "SAVED", row["DELETED_STEP_SEQ"] == DBNull.Value ? "" : "기존 STEP_SEQ " + row["DELETED_STEP_SEQ"] + " 삭제 후 재등록", started);
            }
            catch (Exception ex)
            {
                string cleanup = "";
                if (createdStep && stepSeq >= 0)
                {
                    try
                    {
                        DeleteStepRows(stepSeq, waferSeq);
                        cleanup = " / 저장 중 만든 STEP_SEQ " + stepSeq + " 정리함";
                    }
                    catch (Exception ex2)
                    {
                        cleanup = " / STEP_SEQ " + stepSeq + " 정리 실패: " + FirstLine(ex2.Message);
                    }
                    row["STEP_SEQ"] = DBNull.Value;
                }
                SetResult(row, "ERROR", FirstLine(ex.Message) + cleanup, started);
            }
        }

        /// <summary>LOT -> WAFER -> STEP 순서로 기존 STEP 을 찾는다. 없으면 -1. waferSeq 는 찾은 Wafer(없으면 -1).</summary>
        private static long FindExistingStep(DefectDefine oDefectDefine, string product, string lotId, string waferId, string stepId, string resultTime, out long waferSeq)
        {
            waferSeq = -1;
            long lotSeq = oDefectDefine.GetLotSeq(new string[] { lotId, product, "" });
            if (lotSeq < 0)
                return -1;
            waferSeq = oDefectDefine.GetWaferSeq(new string[] { lotSeq.ToString(CultureInfo.InvariantCulture), waferId, "", "" });
            if (waferSeq < 0)
                return -1;
            return oDefectDefine.GetStepInfo(new string[] { waferSeq.ToString(CultureInfo.InvariantCulture), stepId, resultTime, "1" });
        }

        /// <summary>
        /// STEP 1개의 데이터를 지운다. 순서는 기존 Data Maint 삭제(DefectMapAnalysis.cs 1860~1870행: STEP -> DEFECT -> IMAGES -> INSP_INFO)와 같고,
        /// 업로드는 TQD_INSP_SUM 도 함께 지운다(재등록 때 새 STEP_SEQ 로 다시 만들어지므로 이전 요약 행이 남지 않게 함).
        /// 기존 쿼리 ID 를 그대로 쓰고, INSP_SUM 삭제만 추가 쿼리(DELETE_INSP_SUM_KLARF_UPLOAD)다.
        /// </summary>
        private static void DeleteStepRows(long stepSeq, long waferSeq)
        {
            string step = stepSeq.ToString(CultureInfo.InvariantCulture);
            string wafer = waferSeq.ToString(CultureInfo.InvariantCulture);
            new TQD_STEP().DeleteDataNonQuery("DELETE_MAINT_STEP", null, new string[] { step, wafer });
            new TQD_DEFECT().DeleteDataNonQuery("DELETE_DEFECT", null, new string[] { wafer, step });
            new TQD_IMAGES().DeleteDataNonQuery("DELETE_IMAGES", null, new string[] { step, wafer });
            new TQD_INSP_INFO().DeleteDataNonQuery("DELETE_STEP_MAINT", null, new string[] { step, wafer });
            // TQD_INSP_SUM DSL 에는 범용 삭제 함수가 없어 QueryComponent 의 public ExecuteNonQuery 로 추가 쿼리를 실행한다(DSL 수정 없음).
            new TQD_INSP_SUM().ExecuteNonQuery("DELETE_INSP_SUM_KLARF_UPLOAD", null, new string[] { step });
        }

        /// <summary>분류 기준(D3)을 CLASSNUMBER 에 반영한다. ROUGHBIN / FINEBIN 이면 그 값을 CLASSNUMBER 로 복사하고, 원래 BIN 값은 그대로 둔다.</summary>
        private static void ApplyClassSource(DefectList defects, string classSource)
        {
            foreach (Defect d in defects)
            {
                if (classSource == "ROUGHBIN") d.CLASSNUMBER = d.ROUGHBINNUMBER;
                else if (classSource == "FINEBIN") d.CLASSNUMBER = d.FINEBINNUMBER;
            }
        }

        /// <summary>DataService Step 7 과 같은 열 순서로 TQD_DEFECT 배열을 만든다.</summary>
        private static string[,] BuildDefectArray(DefectList defects, long stepSeq, long waferSeq)
        {
            string[,] para = new string[defects.Count, Enum.GetNames(typeof(DefectCol)).Length];
            for (int i = 0; i < defects.Count; i++)
            {
                Defect d = defects[i];
                para[i, (int)DefectCol.STEP_SEQ] = stepSeq.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.DEFECTID] = d.DEFECTID.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.WAFER_SEQ] = waferSeq.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.X] = Num(d.X);
                para[i, (int)DefectCol.Y] = Num(d.Y);
                para[i, (int)DefectCol.XREL] = Num(d.XREL);
                para[i, (int)DefectCol.YREL] = Num(d.YREL);
                para[i, (int)DefectCol.XINDEX] = d.XINDEX.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.YINDEX] = d.YINDEX.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.XSIZE] = Num(d.XSIZE);
                para[i, (int)DefectCol.YSIZE] = Num(d.YSIZE);
                para[i, (int)DefectCol.DEFECTAREA] = Num(d.DEFECTAREA);
                para[i, (int)DefectCol.DSIZE] = Num(d.DSIZE);
                para[i, (int)DefectCol.CLASSNUMBER] = d.CLASSNUMBER.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.TEST] = d.TEST.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.CLUSTERNUMBER] = d.CLUSTERNUMBER.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.ROUGHBINNUMBER] = d.ROUGHBINNUMBER.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.FINEBINNUMBER] = d.FINEBINNUMBER.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.REVIEWSAMPLE] = d.REVIEWSAMPLE.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.IMAGECOUNT] = d.IMAGECOUNT.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.ADDER] = d.ADDER.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.FIRST_STEP] = d.FIRST_STEP == null ? "" : d.FIRST_STEP;
                para[i, (int)DefectCol.RETICLE_REPEAT_ID] = d.RETICLE_REPEAT_ID.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.DIE_REPEAT_ID] = d.DIE_REPEAT_ID.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.MAN_OPT_CLASS] = d.MAN_OPT_CLASS.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.AUTO_OPT_CLASS] = d.AUTO_OPT_CLASS.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.MAN_SEM_CLASS] = d.MAN_SEM_CLASS.ToString(CultureInfo.InvariantCulture);
                para[i, (int)DefectCol.AUTO_SEM_CLASS] = d.AUTO_SEM_CLASS.ToString(CultureInfo.InvariantCulture);
            }
            return para;
        }

        /// <summary>TQD_STEP.COMMENT: 업로드 구분 + 사용자 / 원본 파일 / 원본 WaferID / 분류 기준. 500자를 넘으면 파일 이름을 줄인다.</summary>
        private static string BuildStepComment(UploadSetting setting, string rawWaferId)
        {
            string tail = ";RAW_WAFER_ID=" + rawWaferId + ";CLASS_SRC=" + setting.ClassSource;
            string head = UploadCommentPrefix + "USER=" + setting.User + ";FILE=";
            int room = StepCommentLength - head.Length - tail.Length;
            string file = setting.FileName.Length > room ? setting.FileName.Substring(0, Math.Max(0, room)) : setting.FileName;
            return head + file + tail;
        }

        #endregion

        #region [이미지 저장소 - D4 FTP 상대 경로]

        /// <summary>이미지 저장소 정보. Folder = 실제 저장 폴더, RelativePath = DB 에 넣는 FTP 상대 경로(저장소 미사용이면 " ").</summary>
        private class ImageStore
        {
            public string Folder;
            public string RelativePath = " ";

            /// <summary>
            /// 이번 업로드에서 저장소에 복사한 이미지 파일 이름. 같은 Lot / 날짜 폴더에는 이전 업로드 파일이 남아 있을 수 있어
            /// 폴더에 파일이 있는지가 아니라 이번에 받은 파일인지로 TQD_IMAGES 행을 만든다(2026-10-07 Probe 에서 확인).
            /// </summary>
            public HashSet<string> SavedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// KLARF 원본과 이미지 원본 / 썸네일을 KLARF_IMAGE_ROOT \ yyyyMMdd(저장일) \ {LotID} 에 저장한다(HandlerInsp.Backup 187~219행과 같은 함수 사용).
        /// 폴더 형식은 2026-10-07 사용자 지시(예: D:\FTP\SCK\20261007\Lot). 같은 Wafer 를 다른 날 다시 올리면 새 날짜 폴더에 저장되고 DB 경로도 그 폴더를 가리킨다.
        /// 설정이 없을 때: 이미지가 없으면 저장소 없이 진행(INSP_FILEPATH ' '), 이미지가 있으면 DB 저장 전에 오류로 멈춘다(이미지 없는 데이터가 생기지 않게).
        /// </summary>
        private static ImageStore SaveFilesToImageStore(ParserKlarf parser, string klarfPath)
        {
            ImageStore store = new ImageStore();
            bool hasImages = parser.ImageManagerList != null && parser.ImageManagerList.HasData();
            string root = System.Configuration.ConfigurationManager.AppSettings["KLARF_IMAGE_ROOT"];
            if (String.IsNullOrWhiteSpace(root))
            {
                if (hasImages)
                    throw new Exception("이미지 저장소 설정(KLARF_IMAGE_ROOT)이 없습니다. SEMDMS Service 설정을 확인하세요.");
                return store;
            }
            if (!Path.IsPathRooted(root))
                throw new Exception("이미지 저장소 설정(KLARF_IMAGE_ROOT)은 절대 경로여야 합니다: " + root);

            // 2026-10-07 사용자 지시: 저장 폴더 = KLARF_IMAGE_ROOT \ yyyyMMdd(저장한 날짜) \ LotID  (예: D:\FTP\SCK\20261007\PFIC_RH-HS2615_C4BUMP)
            // 기존 자동 적재의 yyyy\MM\dd(검사일자) 대신 업로드한 날짜를 쓴다. 폴더 이름에 쓸 수 없는 문자는 '_' 로 바꾼다.
            store.Folder = Path.Combine(Path.Combine(root.Trim(), DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture)), SafeFolderName(parser.LotID));
            // Handler.GetImageRelativePath 와 같은 규칙: 드라이브 루트를 떼고 '\' -> '/'
            store.RelativePath = store.Folder.Replace(Path.GetPathRoot(store.Folder), String.Empty).Replace('\\', '/');
            if (store.RelativePath.Length > 200)
                throw new Exception("이미지 저장 경로가 200자를 넘습니다(TQD_IMAGES.IMAGE_PATH): " + store.RelativePath);
            Directory.CreateDirectory(store.Folder);

            File.Copy(klarfPath, Path.Combine(store.Folder, Path.GetFileName(klarfPath)), true);
            if (hasImages)
            {
                foreach (ImageManager mgr in parser.ImageManagerList.SingleList)
                {
                    File.Copy(mgr.ImageFullName, Path.Combine(store.Folder, mgr.ImageName), true);
                    mgr.SaveThumbnailImage(store.Folder);
                    store.SavedNames.Add(mgr.ImageName);
                }
                foreach (ImageManager mgr in parser.ImageManagerList.MultiList)
                {
                    foreach (KeyValuePair<int, string> item in mgr.FrameDictionary)
                    {
                        string frame = mgr.SaveFrameImageAndThumbnail(item.Key, store.Folder);
                        if (!String.IsNullOrEmpty(frame))
                            store.SavedNames.Add(Path.GetFileName(frame));
                    }
                }
            }
            return store;
        }

        /// <summary>
        /// TQD_IMAGES 배열을 만든다. 열 / 값은 HandlerInsp.GetImage2DArray 와 같고(ImageDefectList 기준, IMAGE_ID = Defect 안 순번,
        /// THUMB = 'thumb_' + 파일명), UPLOAD = 'UPLOAD' 로 자동 적재('INSP')와 구분한다.
        /// 다른 점: 이번 업로드에서 저장소에 복사하지 않은 이미지는 행을 만들지 않는다(DataService 는 파일 없이도 행을 만듦, 설계 6.3절).
        /// </summary>
        private static string[,] BuildImageArray(Wafer wafer, long stepSeq, long waferSeq, ImageStore store)
        {
            List<string[]> rows = new List<string[]>();
            if (store.Folder != null && wafer.ImageDefectList != null)
            {
                foreach (Defect d in wafer.ImageDefectList)
                {
                    if (d.IMAGECOUNT <= 0 || d.Images == null)
                        continue;
                    for (int i = 0; i < d.Images.Count; i++)
                    {
                        string image = d.Images.GetDefectImage(i).IMAGEPATH;
                        if (String.IsNullOrEmpty(image) || !store.SavedNames.Contains(Path.GetFileName(image)))
                            continue;
                        if (image.Length > 190)
                            throw new Exception("이미지 파일 이름이 너무 깁니다(TQD_IMAGES 200자): " + image);
                        rows.Add(new string[] {
                            stepSeq.ToString(CultureInfo.InvariantCulture),        // STEP_SEQ
                            d.DEFECTID.ToString(CultureInfo.InvariantCulture),     // DEFECTID
                            i.ToString(CultureInfo.InvariantCulture),              // IMAGE_ID (쿼리에서 MAX + 1 + i)
                            waferSeq.ToString(CultureInfo.InvariantCulture),       // WAFER_SEQ
                            d.TEST.ToString(CultureInfo.InvariantCulture),         // TEST
                            Path.GetExtension(image),                              // IMAGE_TYPE
                            store.RelativePath,                                    // IMAGE_PATH (FTP 상대 경로)
                            image,                                                 // IMAGE_FILENAME
                            " ",                                                   // IMAGE_SERVER
                            store.RelativePath,                                    // THUMB_PATH
                            "thumb_" + image,                                      // THUMB_FILENAME (Handler.THUMBNAIL_PREFIX 와 같음)
                            "UPLOAD" });                                           // UPLOAD
                    }
                }
            }

            string[,] arr = new string[rows.Count, 12];
            for (int r = 0; r < rows.Count; r++)
                for (int c = 0; c < 12; c++)
                    arr[r, c] = rows[r][c];
            return arr;
        }

        #endregion

        #region [파싱 / 미리보기]

        /// <summary>
        /// KLARF 를 기존 Parser 로 읽는다. 순서: Origin Index 조회 델리게이트 교체(size 128 쿼리) -> FileAnalyzer 로 1.2 / 1.8 판정 -> 파싱
        /// -> 오류 / Wafer / Pitch / SampleSize 검사(frmKlarfFileViewer 와 같은 기준) -> Notch 각도가 0 보다 크면 회전(HandlerInsp.Run 과 같음).
        /// </summary>
        private ParserKlarf ParseKlarf(string path)
        {
            lock (ParseLock)
            {
                ParserKlarf.GetOriginDieIndexDelegate previous = ParserKlarf.GetOriginDieIndex;
                ParserKlarf.GetOriginDieIndex = GetOriginDieIndex;
                try
                {
                    ParserKlarf parser;
                    try
                    {
                        FileAnalyzer analyzer = new FileAnalyzer(path);
                        parser = analyzer.FileType == FileType.KlarfFile_1_8 ? new ParserKlarf_18(path) : new ParserKlarf(path);
                    }
                    catch (Exception ex)
                    {
                        // ParserKlarf_18 은 내부에서 예외를 잡지 않는다.
                        throw new Exception("KLARF 파일을 읽지 못했습니다: " + FirstLine(ex.Message));
                    }

                    if (parser.ErrorFlag)
                        throw new Exception("KLARF 파일을 읽지 못했습니다: " + FirstLine(parser.ErrorMessage));
                    if (parser.Wafers == null || parser.Wafers.Count == 0)
                        throw new Exception("Wafer 정보가 없습니다. WaferID / Slot / DieOrigin 항목을 확인하세요.");
                    if (parser.DiePitchX <= 0 || parser.DiePitchY <= 0)
                        throw new Exception("DiePitch 값이 없습니다.");
                    if (parser.SampleSize <= 0)
                        throw new Exception("SampleSize (Wafer 크기) 값이 없습니다.");

                    if (parser.GetAngle() > 0)
                        parser.RotateAndRecalculate(360 - parser.GetAngle(), DieIndexSort.CenterToLowerLeft);
                    return parser;
                }
                finally
                {
                    ParserKlarf.GetOriginDieIndex = previous;
                }
            }
        }

        /// <summary>
        /// 저장된 Setup 의 Origin Die Index 조회(DefectDefine.GetOriginDieIndex 와 같은 역할). 기존 SELECT_ORIGIN_INDEX 는 SETUP_ID 를 40자로
        /// 잘라 비교하므로 업로드는 size 128 쿼리를 쓴다. 없으면 Point.Empty(Parser 가 WaferDieCalculator 로 계산).
        /// </summary>
        private static Point GetOriginDieIndex(string setupID, string stepID, DateTime setupTime)
        {
            TQD_SETUP oTQD_SETUP = new TQD_SETUP();
            DataTable dt = oTQD_SETUP.GetData("SELECT_ORIGIN_INDEX_KLARF_UPLOAD", null,
                new string[] { setupID, stepID, setupTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) });
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return Point.Empty;
                return new Point(Int32.Parse(dt.Rows[0]["DIE_ORIGIN_X"].ToString(), CultureInfo.InvariantCulture),
                                 Int32.Parse(dt.Rows[0]["DIE_ORIGIN_Y"].ToString(), CultureInfo.InvariantCulture));
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        private static long SelectSetupSeq(TQD_SETUP oTQD_SETUP, string setupId, string stepId, string setupTime)
        {
            DataTable dt = oTQD_SETUP.GetData("SELECT_SETUP_SEQ_KLARF_UPLOAD", null, new string[] { setupId, stepId, setupTime });
            try
            {
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>결과 표 열. Begin(미리보기)과 Commit(저장 결과)이 같은 표를 쓴다.</summary>
        private static DataTable CreateResultTable(string key)
        {
            DataTable dt = new DataTable("KLARF_UPLOAD");
            foreach (string col in new string[] { "UPLOAD_KEY", "FILE_NAME", "FILE_VERSION", "RAW_WAFER_ID", "WAFER_ID", "LOT_ID", "PRODUCT", "STEP_ID",
                                                  "SETUP_ID", "SETUP_TIME", "RESULTTIMESTAMP", "EQUIP", "CLASS_SOURCE", "IMAGE_NAMES", "IMAGE_PATH", "RESULT", "MESSAGE" })
                dt.Columns.Add(col, typeof(string));
            foreach (string col in new string[] { "SLOT_ID", "TEST_COUNT", "INSPECTED_DIE", "DEFECTS", "IMAGE_FILES", "SAVED_IMAGES", "ANGLE", "DIE_ORIGIN_X", "DIE_ORIGIN_Y" })
                dt.Columns.Add(col, typeof(int));
            foreach (string col in new string[] { "EXIST_STEP_SEQ", "DELETED_STEP_SEQ", "STEP_SEQ", "WAFER_SEQ", "SETUP_SEQ", "ELAPSED_MS" })
                dt.Columns.Add(col, typeof(long));
            dt.Columns.Add("ORIGIN_FROM_DB", typeof(bool));
            dt.Columns["UPLOAD_KEY"].DefaultValue = key;
            return dt;
        }

        /// <summary>
        /// Wafer 1장의 저장 예정 값을 만든다. 화면 입력(Product / Step ID / 설비)이 비면 KLARF 값을 쓴다.
        /// 길이 / 필수 값 검사에 걸리면 RESULT = INVALID 로 표시하고 Commit 에서 저장하지 않는다.
        /// </summary>
        private static DataRow BuildWaferRow(DataTable table, UploadSetting setting, ParserKlarf parser, Wafer wafer, Dictionary<int, string> rawIds, bool checkExisting)
        {
            DataRow row = table.NewRow();
            string rawId;
            string product = setting.Product.Length > 0 ? setting.Product : Trim(parser.DeviceID);
            string stepId = setting.StepId.Length > 0 ? setting.StepId : Trim(parser.StepID);
            string equip = setting.Equip.Length > 0 ? setting.Equip : Trim(parser.Equip);

            List<Point> dies = new List<Point>();
            foreach (InspectionTest test in wafer.TestList)
                foreach (Point pt in test.SampleTestPlan)
                    if (!dies.Contains(pt)) dies.Add(pt);

            List<string> imageNames = new List<string>();
            foreach (Defect d in wafer.DefectList)
            {
                if (d.Images == null) continue;
                for (int i = 0; i < d.Images.Count; i++)
                {
                    // DefectImageList 의 인덱서는 string 을 돌려주므로 DefectImage 는 GetDefectImage 로 꺼낸다.
                    string p = d.Images.GetDefectImage(i).IMAGEPATH;
                    if (!String.IsNullOrEmpty(p) && !imageNames.Contains(p)) imageNames.Add(p);
                }
            }

            row["FILE_NAME"] = setting.FileName;
            row["FILE_VERSION"] = parser is ParserKlarf_18 ? "1.8" : "1.2";
            row["SLOT_ID"] = wafer.Slot;
            row["RAW_WAFER_ID"] = rawIds.TryGetValue(wafer.Slot, out rawId) ? rawId : "";
            row["WAFER_ID"] = wafer.WaferID;
            row["LOT_ID"] = parser.LotID;
            row["PRODUCT"] = product;
            row["STEP_ID"] = stepId;
            row["SETUP_ID"] = parser.SetupID;
            row["SETUP_TIME"] = parser.SetupTimestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["RESULTTIMESTAMP"] = parser.ResultTimestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["EQUIP"] = equip;
            row["CLASS_SOURCE"] = setting.ClassSource;
            row["TEST_COUNT"] = wafer.TestList.Count;
            row["INSPECTED_DIE"] = dies.Count;
            row["DEFECTS"] = wafer.DefectList.Count;
            row["IMAGE_FILES"] = imageNames.Count;
            row["IMAGE_NAMES"] = String.Join(";", imageNames.ToArray());
            row["ANGLE"] = parser.GetAngle();
            row["DIE_ORIGIN_X"] = (int)wafer.DieOriginX;
            row["DIE_ORIGIN_Y"] = (int)wafer.DieOriginY;
            row["ORIGIN_FROM_DB"] = parser.MapOriginFromDB;

            List<string> errors = new List<string>();
            CheckLength(errors, "Product", product, IdColumnLength);
            CheckLength(errors, "Lot ID", parser.LotID, IdColumnLength);
            CheckLength(errors, "Wafer ID", wafer.WaferID, IdColumnLength);
            CheckLength(errors, "Step ID", stepId, IdColumnLength);
            CheckLength(errors, "KLARF StepID", parser.StepID, IdColumnLength);
            CheckLength(errors, "Setup ID", parser.SetupID, SetupIdLength);
            CheckLength(errors, "설비", equip, IdColumnLength);
            if (wafer.TestList.Count == 0 || dies.Count == 0)
                errors.Add("검사 Die(SampleTestPlan)가 없습니다.");

            if (errors.Count > 0)
            {
                row["RESULT"] = "INVALID";
                row["MESSAGE"] = String.Join(" / ", errors.ToArray());
            }
            else
            {
                row["RESULT"] = "READY";
                row["MESSAGE"] = "";
                if (checkExisting)
                {
                    long waferSeq;
                    long existing = FindExistingStep(new DefectDefine(), product, parser.LotID, wafer.WaferID, stepId,
                        parser.ResultTimestamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), out waferSeq);
                    if (existing >= 0)
                    {
                        row["EXIST_STEP_SEQ"] = existing;
                        row["WAFER_SEQ"] = waferSeq;
                        row["RESULT"] = "EXISTS";
                        row["MESSAGE"] = "이미 등록됨 (STEP_SEQ " + existing + ")";
                    }
                }
            }
            return row;
        }

        /// <summary>
        /// 원본 WaferID(Parser 가 DMS 규칙으로 바꾸기 전 값)를 Slot 별로 읽는다. frmKlarfFileViewer.ReadRawWaferIds 와 같은 방식이며
        /// KLARF 1.2 의 "WaferID ...; Slot n;" 형식에만 맞는다(1.8 은 빈 값).
        /// </summary>
        private static Dictionary<int, string> ReadRawWaferIds(string path)
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();
            string text = File.ReadAllText(path);
            foreach (Match m in Regex.Matches(text, "WaferID\\s+\"?([^\";]*)\"?\\s*;\\s*Slot\\s+(\\d+)"))
            {
                int slot;
                if (Int32.TryParse(m.Groups[2].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out slot) && !ids.ContainsKey(slot))
                    ids.Add(slot, m.Groups[1].Value.Trim());
            }
            return ids;
        }

        #endregion

        #region [작업 폴더 / 공통]

        /// <summary>업로드 설정. Begin 에서 검증해 작업 폴더에 저장하고 AddImage / Commit 이 다시 읽는다.</summary>
        private class UploadSetting
        {
            public string Factory;
            public string User;
            public string FileName;
            public string Product;
            public string StepId;
            public string Equip;
            public string ClassSource;

            public static UploadSetting FromArray(string[] info)
            {
                if (info == null || info.Length != Enum.GetNames(typeof(UploadInfoIndex)).Length)
                    throw new Exception("UploadInfo 는 7개 항목이어야 합니다. (Factory / User / 파일명 / Product / Step ID / 설비 / 분류 기준)");
                UploadSetting s = new UploadSetting();
                s.Factory = Required(info[(int)UploadInfoIndex.FACTORY], "Factory", IdColumnLength);
                s.User = Required(info[(int)UploadInfoIndex.USER], "User", 30);
                s.FileName = SafeFileName(info[(int)UploadInfoIndex.FILE_NAME], "KLARF 파일 이름");
                if (s.FileName.Length > 256)
                    throw new Exception("KLARF 파일 이름이 256자를 넘습니다.");
                s.Product = Trim(info[(int)UploadInfoIndex.PRODUCT]);
                s.StepId = Trim(info[(int)UploadInfoIndex.STEP_ID]);
                s.Equip = Trim(info[(int)UploadInfoIndex.EQUIP]);
                s.ClassSource = Trim(info[(int)UploadInfoIndex.CLASS_SOURCE]).ToUpperInvariant();
                if (s.ClassSource.Length == 0) s.ClassSource = "ROUGHBIN";
                if (s.ClassSource != "ROUGHBIN" && s.ClassSource != "FINEBIN" && s.ClassSource != "CLASS")
                    throw new Exception("분류 기준은 ROUGHBIN / FINEBIN / CLASS 중 하나여야 합니다: " + s.ClassSource);
                return s;
            }

            public string[] ToArray()
            {
                return new string[] { Factory, User, FileName, Product, StepId, Equip, ClassSource };
            }

            public static UploadSetting Read(string path)
            {
                if (!File.Exists(path))
                    throw new Exception("업로드 설정 파일이 없습니다. 처음부터 다시 업로드하세요.");
                return FromArray(File.ReadAllLines(path, Encoding.UTF8));
            }
        }

        /// <summary>작업 폴더 루트. Service 설정 KLARF_UPLOAD_WORK_PATH, 없으면 %TEMP%\DACrux_KlarfUpload.</summary>
        private static string GetWorkRoot()
        {
            string root = System.Configuration.ConfigurationManager.AppSettings["KLARF_UPLOAD_WORK_PATH"];
            if (String.IsNullOrWhiteSpace(root))
                root = Path.Combine(Path.GetTempPath(), "DACrux_KlarfUpload");
            return root;
        }

        private static string GetWorkFolder(string key)
        {
            ValidateUploadKey(key);
            return Path.Combine(GetWorkRoot(), key);
        }

        private static string GetExistingWorkFolder(string key)
        {
            string dir = GetWorkFolder(key);
            if (!Directory.Exists(dir))
                throw new Exception("업로드 작업이 없습니다(이미 저장 / 취소되었거나 하루가 지나 정리됨). 처음부터 다시 업로드하세요.");
            return dir;
        }

        /// <summary>업로드 키 형식 검사. 화면에서 받은 값으로 폴더 경로를 만들기 때문에 형식이 다르면 거부한다(경로 조작 방지).</summary>
        private static void ValidateUploadKey(string key)
        {
            if (key == null || !UploadKeyPattern.IsMatch(key))
                throw new Exception("업로드 키 형식이 올바르지 않습니다.");
        }

        /// <summary>하루 지난 작업 폴더를 지운다(화면이 중간에 닫혀 Commit / Cancel 이 오지 않은 경우).</summary>
        private static void CleanupOldWorkFolders()
        {
            string root = GetWorkRoot();
            if (!Directory.Exists(root))
                return;
            foreach (string dir in Directory.GetDirectories(root))
            {
                if (UploadKeyPattern.IsMatch(Path.GetFileName(dir)) && Directory.GetCreationTime(dir) < DateTime.Now.AddDays(-1))
                    DeleteFolderQuietly(dir);
            }
        }

        private static void DeleteFolderQuietly(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
            }
            catch
            {
                // 정리 실패는 저장 결과에 영향을 주지 않는다. 남은 폴더는 다음 BeginUpload 가 하루 뒤 정리한다.
            }
        }

        /// <summary>경로 없이 파일 이름만 허용한다(폴더 구분자 / 상위 폴더 / 잘못된 문자 거부).</summary>
        private static string SafeFileName(string name, string label)
        {
            string value = Trim(name);
            if (value.Length == 0)
                throw new Exception(label + " 값이 없습니다.");
            if (value != Path.GetFileName(value) || value == "." || value == ".." || value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new Exception(label + " 에 경로나 사용할 수 없는 문자가 있습니다: " + value);
            return value;
        }

        /// <summary>Lot ID 를 폴더 이름으로 쓸 수 있게 바꾼다(경로 구분자 / 잘못된 문자 -> '_').</summary>
        private static string SafeFolderName(string value)
        {
            string v = Trim(value);
            if (v.Length == 0) return "_";
            char[] chars = v.ToCharArray();
            char[] invalid = Path.GetInvalidFileNameChars();
            for (int i = 0; i < chars.Length; i++)
                if (Array.IndexOf(invalid, chars[i]) >= 0) chars[i] = '_';
            string s = new string(chars);
            return s == "." || s == ".." ? "_" : s;
        }

        private static void SetResult(DataRow row, string result, string message, DateTime started)
        {
            row["RESULT"] = result;
            row["MESSAGE"] = message;
            row["ELAPSED_MS"] = (long)(DateTime.Now - started).TotalMilliseconds;
        }

        private static void CheckLength(List<string> errors, string label, string value, int max)
        {
            if (String.IsNullOrEmpty(value))
                errors.Add(label + " 값이 없습니다.");
            else if (Encoding.UTF8.GetByteCount(value) > max)
                errors.Add(String.Format("{0} 이 {1} byte 를 넘습니다 ({2})", label, max, value));
        }

        private static string Required(string value, string label, int max)
        {
            string v = Trim(value);
            if (v.Length == 0)
                throw new Exception(label + " 값이 없습니다.");
            if (Encoding.UTF8.GetByteCount(v) > max)
                throw new Exception(String.Format("{0} 이 {1} byte 를 넘습니다.", label, max));
            return v;
        }

        private static long RequiredNumber(string value, string label)
        {
            long n;
            if (!Int64.TryParse(Trim(value), NumberStyles.Integer, CultureInfo.InvariantCulture, out n) || n < 0)
                throw new Exception(label + " 값이 올바르지 않습니다: " + value);
            return n;
        }

        private static string Trim(string value)
        {
            return value == null ? "" : value.Trim();
        }

        private static string FirstLine(string message)
        {
            if (String.IsNullOrEmpty(message)) return "";
            int idx = message.IndexOfAny(new char[] { '\r', '\n' });
            return idx > 0 ? message.Substring(0, idx) : message;
        }

        /// <summary>숫자를 DB 문자열로 바꾼다. DataService 는 ToString()(현재 문화권)을 쓰며, 한국어 문화권에서는 결과가 같다.</summary>
        private static string Num(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
