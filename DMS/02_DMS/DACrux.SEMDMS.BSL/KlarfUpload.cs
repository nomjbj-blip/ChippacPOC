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
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-07_KLARF_Upload_DB적재_설계.md (F08)
    ///
    /// 만든 이유: 기존 자동 적재는 설비 폴더 감시 서비스라 화면에서 바로 올릴 수 없고, 고객 샘플(XD106.02)은 기존 규칙
    ///            (SetupID 40자 / Step ID 8자리 규칙 / 이미지가 하위 폴더)으로는 저장되지 않는다.
    /// 처리 원칙:
    ///  - 파싱은 기존 Parser(ParserKlarf / ParserKlarf_18)를 그대로 쓰고, 저장은 기존 DefectDefine public 함수를 같은 프로세스에서 호출한다.
    ///    순서와 계산은 InspectionDataService.cs Step 1~12 와 같다(Step 13 Alarm 메일은 하지 않음).
    ///  - DataService 와 의도적으로 다른 점(설계 6.2절): Step ID 8자리 규칙 / LotID 앞 'A' 제거(FAB1 규칙) 미적용,
    ///    ANGLE 은 Parser 각도, SetupID 는 size 128 쿼리(기존 쿼리는 40자로 잘림),
    ///    같은 키가 있으면 삭제 후 재등록(D1).
    ///  - Defect 분류는 KLARF 표준대로 저장한다 (2026-10-15): CLASSNUMBER / ROUGHBINNUMBER / FINEBINNUMBER 를 파일 값 그대로 저장하고
    ///    ClassLookup 은 CLASSNUMBER 의 이름표로 본다(기존 자동 적재와 같음). 예전의 RoughBin -> CLASSNUMBER 복사는 하지 않는다.
    ///  - Map 은 Map Setup(TQP_MAPDEF / TQP_USEMAP, Map ID = Device) 기준으로 저장한다 (2026-10-13).
    ///    SETUP_MAP = Map Setup 사용 Die 전체(Test 마다 같은 목록), SETUP 의 Wafer 크기 / Pitch / 원점 = Map Setup 값,
    ///    Defect Die 번호는 Map Setup 원점에 맞춰 옮긴다. KLARF 에서는 Defect 정보만 쓰고 검사 Die(SampleTestPlan)로 Map 을 만들지 않는다.
    ///    같은 Setup 이 이미 있고 값 / Die 가 Map Setup 과 다르면 저장할 때 Map Setup 값으로 바꾼다 (재업로드 시 교체).
    ///  - 기존 DataService 처럼 문장마다 자동 Commit 이다. Wafer 저장 중 오류가 나면 이번에 새로 만든 STEP 의 행을 지운다.
    ///    SETUP / PRODUCT / LOT / WAFER 는 다른 STEP 이 함께 쓰는 행이라 남겨 두고 다음 저장에서 재사용한다.
    /// 이미지(구현 2단계, 결정 D4 = FTP 상대 경로): Service 설정 KLARF_IMAGE_ROOT(FTP 가 공개하는 폴더 아래) \ yyyyMMdd(저장일) \ {Device} \ {LotID} 에
    ///   KLARF 원본 / 이미지 원본 / 썸네일(thumb_, JPEG)을 저장하고(파일 이름 규칙은 HandlerInsp.Backup 과 같음), DB 에는 드라이브 루트를 뺀 '/' 상대 경로
    ///   (Handler.GetImageRelativePath 와 같은 규칙)를 넣는다. 화면은 TQC_CONFIG DEFECT_FTP 의 MAIN_PATH + 이 경로로 받는다.
    ///   Service 설정 KLARF_IMAGE_FTP_BASE(MAIN_PATH 가 가리키는 실제 폴더)가 있으면 드라이브 루트 대신 그 폴더 기준 경로를 넣는다 (2026-10-21).
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

        /// <summary>UploadInfo 배열 위치(Interface 주석과 같은 순서). RESERVED(옛 분류 기준 자리)는 받기만 하고 쓰지 않는다 (2026-10-15).</summary>
        private enum UploadInfoIndex { FACTORY, USER, FILE_NAME, PRODUCT, STEP_ID, EQUIP, RESERVED }

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
            // 작업 기간 확인: 기간이 지나면 여기서 멈추고 저장하지 않는다 (2026-10-23)
            new WorkRule().Confirm(WorkModule.Klarf, WorkKind.Upload);
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
                SetupMapLayout map = LoadSetupMapAndAlign(setting, parser);
                DataTable result = CreateResultTable(key);
                Dictionary<int, string> rawIds = ReadRawWaferIds(Path.Combine(dir, setting.FileName));
                foreach (Wafer wafer in parser.Wafers)
                    result.Rows.Add(BuildWaferRow(result, setting, parser, wafer, rawIds, true, map));
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
            // 작업 기간 확인: 기간이 지나면 여기서 멈추고 저장하지 않는다 (2026-10-23)
            new WorkRule().Confirm(WorkModule.Klarf, WorkKind.Upload);
            string dir = GetExistingWorkFolder(strUploadKey);
            ParserKlarf parser = null;
            try
            {
                UploadSetting setting = UploadSetting.Read(Path.Combine(dir, InfoFileName));
                bool overwrite = Options != null && Options.Length > 0 && String.Equals(Trim(Options[0]), "Y", StringComparison.OrdinalIgnoreCase);

                string klarfPath = Path.Combine(dir, setting.FileName);
                parser = ParseKlarf(klarfPath);
                SetupMapLayout map = LoadSetupMapAndAlign(setting, parser);
                Dictionary<int, string> rawIds = ReadRawWaferIds(klarfPath);

                // 이미지 / KLARF 원본을 저장소로 먼저 복사한다(파일 1벌을 Wafer 들이 함께 쓴다). 실패하면 DB 저장 전에 멈춘다.
                ImageStore store = SaveFilesToImageStore(parser, klarfPath, ResolveProduct(setting, parser));

                DataTable result = CreateResultTable(strUploadKey);
                foreach (Wafer wafer in parser.Wafers)
                {
                    DataRow row = BuildWaferRow(result, setting, parser, wafer, rawIds, false, map);
                    row["IMAGE_PATH"] = store.RelativePath;
                    result.Rows.Add(row);
                    SaveWafer(setting, parser, wafer, row, overwrite, store, map);
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
            // 작업 기간 확인: 기간이 지나면 여기서 멈추고 저장하지 않는다 (2026-10-23)
            new WorkRule().Confirm(WorkModule.Klarf, WorkKind.Upload);
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
        /// <param name="map">Map Setup 기준값. BuildWaferRow 가 Map 이 없거나 값이 잘못되면 INVALID 로 막으므로 여기서는 사용할 수 있는 값이다.</param>
        private void SaveWafer(UploadSetting setting, ParserKlarf parser, Wafer wafer, DataRow row, bool overwrite, ImageStore store, SetupMapLayout map)
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

                #region [Step 01 : TQD_SETUP 확인 생성 / Map Setup 값으로 맞춤 (SETUP_ID size 128 쿼리)]
                // Wafer 크기 / Pitch / 원점은 Map Setup 값이다 (2026-10-13). Defect 조회 화면이 이 값으로 Die 위치를 그린다.
                // DIE_ORIGIN 은 Map Setup 원점 번호다. LoadSetupMapAndAlign 이 Defect / 검사 Die 번호를 이 원점 기준으로 옮겨 두었다.
                TQD_SETUP oTQD_SETUP = new TQD_SETUP();
                setupSeq = SelectSetupSeq(oTQD_SETUP, parser.SetupID, parser.StepID, setupTime);
                if (setupSeq < 0)
                {
                    oTQD_SETUP.InsertData("CREATE_SETUP_INFO_KLARF_UPLOAD", null, new string[] {
                        parser.SetupID,                                   // SETUP_ID (KLARF 원문, 최대 128)
                        parser.StepID,                                    // STEP_ID (DataService 와 같이 KLARF StepID 원문)
                        setupTime,                                        // SETUP_TIME
                        Num(parser.GetAngle()),                           // ANGLE (Parser 각도, 설계 6.2절)
                        Num(map.WaferSize),                               // WAFER_SIZE (um, Map Setup)
                        "N",                                              // NOTCH_TYPE (DataService 와 같음)
                        Num(map.PitchX),                                  // DIE_PITCH_X (Map Setup CHIP_SIZE_X)
                        Num(map.PitchY),                                  // DIE_PITCH_Y (Map Setup CHIP_SIZE_Y)
                        Num(map.OriginIndex.X),                           // DIE_ORIGIN_X (Map Setup 원점, LeftBottom 번호)
                        Num(map.OriginIndex.Y),                           // DIE_ORIGIN_Y
                        Num(map.OriginX),                                 // ORIGIN_X (Map Setup 원점 um)
                        Num(map.OriginY) });                              // ORIGIN_Y
                    setupSeq = SelectSetupSeq(oTQD_SETUP, parser.SetupID, parser.StepID, setupTime);
                    if (setupSeq < 0)
                        throw new Exception("TQD_SETUP 생성 실패");
                }
                else if (!SetupMatchesMap(oTQD_SETUP, setupSeq, map))
                {
                    // 같은 Setup 이 예전 규칙(KLARF 값)으로 저장되어 있으면 Map Setup 값으로 바꾼다 (재업로드 시 교체, 2026-10-13 결정).
                    oTQD_SETUP.UpdateDataNonQuery("UPDATE_SETUP_INFO_KLARF_UPLOAD", null, new string[] {
                        Num(map.WaferSize), Num(map.PitchX), Num(map.PitchY), Num(map.OriginIndex.X), Num(map.OriginIndex.Y),
                        Num(map.OriginX), Num(map.OriginY), setupSeq.ToString(CultureInfo.InvariantCulture) });
                }
                row["SETUP_SEQ"] = setupSeq;
                #endregion

                #region [Step 02 : TQD_SETUP_MAP 확인 생성 / 교체 (Test 마다 Map Setup 사용 Die 전체)]
                // Defect 조회 화면은 이 목록을 Die 로 그린다. KLARF 검사 Die(SampleTestPlan)가 아니라 Map Setup 사용 Die 를 넣어
                // Map Setup 모양 그대로 보이게 한다 (2026-10-13). 같은 Test 에 다른 목록이 이미 있으면 지우고 다시 넣는다.
                foreach (InspectionTest test in wafer.TestList)
                {
                    string strTest = test.TestNo.ToString(CultureInfo.InvariantCulture);
                    DataTable dtMap = oDefectDefine.GetSetupTestData(setupSeq.ToString(CultureInfo.InvariantCulture), strTest);
                    bool exists = dtMap != null && dtMap.Rows.Count > 0;
                    bool same = exists && SetupMapMatches(dtMap, map);
                    if (dtMap != null) dtMap.Dispose();
                    if (same)
                        continue;

                    TQD_SETUP_MAP oTQD_SETUP_MAP = new TQD_SETUP_MAP();
                    if (exists)
                        oTQD_SETUP_MAP.DeleteDataNonQuery("DELETE_SETUP_MAP_KLARF_UPLOAD", null, new string[] { setupSeq.ToString(CultureInfo.InvariantCulture), strTest });

                    string[,] para = new string[map.Dies.Count, 4];
                    for (int i = 0; i < map.Dies.Count; i++)
                    {
                        para[i, 0] = setupSeq.ToString(CultureInfo.InvariantCulture);
                        para[i, 1] = strTest;
                        para[i, 2] = map.Dies[i].X.ToString(CultureInfo.InvariantCulture);
                        para[i, 3] = map.Dies[i].Y.ToString(CultureInfo.InvariantCulture);
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
                        map.Dies.Count.ToString(CultureInfo.InvariantCulture), // NETDIE (Map Setup 사용 Die 수)
                        Num(map.WaferSize),                               // WAFER_SIZE (Map Setup)
                        Num(parser.GetAngle()),                           // ANGLE
                        "N",                                              // NOTCH_TYPE
                        Num(map.PitchX),                                  // DIE_PITCH_X (Map Setup)
                        Num(map.PitchY),                                  // DIE_PITCH_Y
                        Num(map.OriginIndex.X),                           // DIE_ORIGIN_X
                        Num(map.OriginIndex.Y),                           // DIE_ORIGIN_Y
                        Num(map.OriginX),                                 // ORIGIN_X
                        Num(map.OriginY),                                 // ORIGIN_Y
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
                stepInfo[(int)StepCol.COMMENT] = BuildStepComment(setting, row["RAW_WAFER_ID"].ToString(), parser.DiePitchX, parser.DiePitchY);

                oDefectDefine.CreateStepSeq(stepInfo);
                stepSeq = oDefectDefine.GetStepInfo(new string[] { waferSeq.ToString(CultureInfo.InvariantCulture), stepId, resultTime, "1" });
                if (stepSeq < 0)
                    throw new Exception("TQD_STEP 생성 실패");
                createdStep = true;
                row["STEP_SEQ"] = stepSeq;
                #endregion

                #region [Step 07 : TQD_DEFECT 생성 (New Defect)]
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
                #endregion

                #region [Step 08 : TQD_INSP_INFO 생성 (Test 마다 1행, 집계식은 DataService 와 같음. DEFECTIVE_DIE 만 Test 별)]
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

                    // Defect Die 수는 이 Test 의 Defect 로만 센다. 같은 행의 DEFECTS / CLASSIFIED 와 같은 기준이다. (2026-10-19)
                    // 이전에는 DataService 처럼 STEP 전체 Defect 의 Die 수를 모든 Test 행에 넣어, Test 가 여러 개인 파일에서
                    // Defect 가 없는 Test 행에도 다른 Test 의 Die 수가 들어갔다(예: Test 80 행 DEFECTS 0 / DEFECTIVE_DIE 51).
                    // 원본 SummaryList NDEFDIE 는 쓰지 않는다. DEFECTS 도 Summary NDEFECT 가 아니라 DefectList 로 센 값이다.
                    HashSet<Point> defectDies = new HashSet<Point>();
                    foreach (Defect d in defects)
                        defectDies.Add(new Point(d.XINDEX, d.YINDEX));

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
                        defectDies.Count.ToString(CultureInfo.InvariantCulture),   // DEFECTIVE_DIE (이 Test 의 Defect Die 수)
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

                SetResult(row, "SAVED", JoinMessage(row["DELETED_STEP_SEQ"] == DBNull.Value ? "" : "기존 STEP_SEQ " + row["DELETED_STEP_SEQ"] + " 삭제 후 재등록",
                    row["MAP_NOTE"].ToString()), started);
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

        /// <summary>
        /// TQD_STEP.COMMENT: 업로드 구분 + 사용자 / 원본 파일 / KLARF 원본 Die Pitch / 원본 WaferID. 500자를 넘으면 파일 이름을 줄인다.
        /// RAW_DIE_PITCH(2026-10-16, 통합 연관 분석 설계 05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 5.2절 결정 B6):
        /// TQD_SETUP 에는 Map Setup Pitch 를 저장하므로, XREL / YREL 기준인 파일 DiePitch(µm)를 "x,y" 로 따로 남긴다.
        /// </summary>
        private static string BuildStepComment(UploadSetting setting, string rawWaferId, double pitchX, double pitchY)
        {
            string tail = ";RAW_DIE_PITCH=" + Num(pitchX) + "," + Num(pitchY) + ";RAW_WAFER_ID=" + rawWaferId;
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
        /// 이미지 저장 폴더를 DB(TQD_IMAGES.IMAGE_PATH / TQD_STEP.INSP_FILEPATH)에 넣는 FTP 상대 경로로 바꾼다.
        /// 화면(DefectMapDraw.fnFTPImageDownload)은 DEFECT_FTP 의 MAIN_PATH + 이 경로 + 파일 이름으로 받는다.
        ///  - ftpBase(Service 설정 KLARF_IMAGE_FTP_BASE)가 없으면: 지금까지와 같이 드라이브 루트를 뺀 경로(Handler.GetImageRelativePath 와 같은 규칙).
        ///    예) D:\FTP\SCK\20261007\Lot -> FTP/SCK/20261007/Lot (FTP 루트 = 드라이브 루트, MAIN_PATH = /%2f/)
        ///  - 있으면: 그 폴더 기준 경로. FTP 루트가 드라이브 루트가 아닌 서버에서 쓴다. (2026-10-21)
        ///    예) FTP /DMS_IMAGE = C:\FTP\DMS_IMAGE 이면 KLARF_IMAGE_ROOT / KLARF_IMAGE_FTP_BASE = C:\FTP\DMS_IMAGE, MAIN_PATH = /DMS_IMAGE
        ///        -> C:\FTP\DMS_IMAGE\20261021\Lot -> 20261021/Lot
        /// 저장 폴더가 기준 폴더 아래가 아니면 화면에서 받을 수 없으므로 오류로 멈춘다.
        /// </summary>
        private static string ToFtpRelativePath(string folder, string ftpBase)
        {
            if (String.IsNullOrWhiteSpace(ftpBase))
                return folder.Replace(Path.GetPathRoot(folder), String.Empty).Replace('\\', '/');

            ftpBase = ftpBase.Trim();
            if (!Path.IsPathRooted(ftpBase))
                throw new Exception("이미지 FTP 기준 폴더 설정(KLARF_IMAGE_FTP_BASE)은 절대 경로여야 합니다: " + ftpBase);

            // 끝에 '\' 를 붙여 비교해야 C:\FTP\DMS 가 C:\FTP\DMS_IMAGE 의 앞부분으로 맞지 않는다.
            string basePath = Path.GetFullPath(ftpBase).TrimEnd('\\', '/') + "\\";
            string folderPath = Path.GetFullPath(folder);
            if (!folderPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
                throw new Exception(String.Format("이미지 저장 폴더가 FTP 기준 폴더(KLARF_IMAGE_FTP_BASE) 아래가 아닙니다. KLARF_IMAGE_ROOT 를 확인하세요: {0} / 기준 {1}", folderPath, basePath));

            return folderPath.Substring(basePath.Length).Replace('\\', '/');
        }

        /// <summary>
        /// KLARF 원본과 이미지 원본 / 썸네일을 KLARF_IMAGE_ROOT \ yyyyMMdd(저장일) \ {Device} \ {LotID} 에 저장한다.
        /// 폴더 형식: 2026-10-07 요청(날짜 \ Lot)에 Device(저장하는 Product 값)를 더했다 (2026-10-21). 예: D:\FTP\SCK\20261021\OMM_TCB\Lot.
        /// 같은 Wafer 를 다른 날 다시 올리면 새 날짜 폴더에 저장되고 DB 경로도 그 폴더를 가리킨다. 이미 올린 파일 / DB 경로는 그대로다.
        /// 파일 이름 규칙(원본 / 프레임 '이름_0001.확장자' / 'thumb_' + 이름)은 HandlerInsp.Backup 과 같고, 썸네일은 SaveJpegThumbnail 로 만든다.
        /// 설정이 없을 때: 이미지가 없으면 저장소 없이 진행(INSP_FILEPATH ' '), 이미지가 있으면 DB 저장 전에 오류로 멈춘다(이미지 없는 데이터가 생기지 않게).
        /// </summary>
        private static ImageStore SaveFilesToImageStore(ParserKlarf parser, string klarfPath, string product)
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

            // 저장 폴더 = KLARF_IMAGE_ROOT \ yyyyMMdd(저장한 날짜) \ Device \ LotID (2026-10-21, 예: D:\FTP\SCK\20261021\OMM_TCB\PFIC_U9-FS2635_C4BUMP)
            // 기존 자동 적재의 yyyy\MM\dd(검사일자) 대신 업로드한 날짜를 쓴다. 폴더 이름에 쓸 수 없는 문자는 '_' 로 바꾼다.
            string dayFolder = Path.Combine(root.Trim(), DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            store.Folder = Path.Combine(Path.Combine(dayFolder, SafeFolderName(product)), SafeFolderName(parser.LotID));
            // 폴더를 만들기 전에 DB 경로를 정한다. 기준 폴더 설정이 잘못되었으면 파일을 복사하기 전에 멈춘다.
            store.RelativePath = ToFtpRelativePath(store.Folder, System.Configuration.ConfigurationManager.AppSettings["KLARF_IMAGE_FTP_BASE"]);
            if (store.RelativePath.Length > 200)
                throw new Exception("이미지 저장 경로가 200자를 넘습니다(TQD_IMAGES.IMAGE_PATH): " + store.RelativePath);
            Directory.CreateDirectory(store.Folder);

            File.Copy(klarfPath, Path.Combine(store.Folder, Path.GetFileName(klarfPath)), true);
            if (hasImages)
            {
                foreach (ImageManager mgr in parser.ImageManagerList.SingleList)
                {
                    string target = Path.Combine(store.Folder, mgr.ImageName);
                    File.Copy(mgr.ImageFullName, target, true);
                    SaveThumbnailFromFile(target);
                    store.SavedNames.Add(mgr.ImageName);
                }
                foreach (ImageManager mgr in parser.ImageManagerList.MultiList)
                {
                    foreach (string frame in SaveFrameImagesAndThumbnails(mgr.ImageFullName, mgr.FrameDictionary.Keys, store.Folder))
                        store.SavedNames.Add(frame);
                }
            }
            return store;
        }

        /// <summary>썸네일 파일 이름 규칙(Handler.THUMBNAIL_PREFIX / ImageLoader.GetThumbnailImageName 과 같음).</summary>
        private const string ThumbnailPrefix = "thumb_";

        /// <summary>썸네일 JPEG 품질. 화면 목록 표시용이라 원본보다 작게 둔다.</summary>
        private const long ThumbnailJpegQuality = 85L;

        /// <summary>
        /// 저장한 이미지 파일 옆에 'thumb_' + 이름 썸네일을 만든다. 이미지를 읽을 수 없으면 원본을 그대로 복사한다
        /// (기존 Core 와 같이 썸네일 파일이 없어서 화면 목록이 비지 않게).
        /// </summary>
        private static void SaveThumbnailFromFile(string imagePath)
        {
            string thumbPath = Path.Combine(Path.GetDirectoryName(imagePath), ThumbnailPrefix + Path.GetFileName(imagePath));
            try
            {
                using (Image image = Image.FromFile(imagePath))
                    SaveJpegThumbnail(image, thumbPath);
            }
            catch (Exception)
            {
                File.Copy(imagePath, thumbPath, true);
            }
        }

        /// <summary>
        /// 여러 장이 든 이미지(Multi Page TIFF 등)를 프레임마다 '이름_0001.확장자' 로 저장하고 썸네일을 만든다. 저장한 파일 이름 목록을 돌려준다.
        /// 프레임 번호(frameSeqs)는 1부터(ImageManager.FrameDictionary 키), 프레임 파일 형식은 원본 형식 그대로(ImageLoader.SaveImageAndThumbnail 과 같음).
        /// 이미지를 읽을 수 없으면 아무것도 저장하지 않는다(기존 Core 도 이 경우 프레임을 만들지 않음).
        /// </summary>
        private static List<string> SaveFrameImagesAndThumbnails(string imagePath, IEnumerable<int> frameSeqs, string folder)
        {
            List<string> saved = new List<string>();
            Image image;
            try { image = Image.FromFile(imagePath); }
            catch (Exception) { return saved; }

            using (image)
            {
                System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(image.FrameDimensionsList[0]);
                int frames = image.GetFrameCount(dimension);
                System.Drawing.Imaging.ImageFormat format = ImageLoader.GetImageFormat(image);
                foreach (int seq in frameSeqs)
                {
                    if (seq < 1 || seq > frames)
                        continue;
                    string name = String.Format(CultureInfo.InvariantCulture, "{0}_{1:0000}{2}",
                        Path.GetFileNameWithoutExtension(imagePath), seq, Path.GetExtension(imagePath));
                    image.SelectActiveFrame(dimension, seq - 1);
                    image.Save(Path.Combine(folder, name), format);
                    SaveJpegThumbnail(image, Path.Combine(folder, ThumbnailPrefix + name));
                    saved.Add(name);
                }
            }
            return saved;
        }

        /// <summary>
        /// 썸네일 = 원본 가로 / 세로 절반 크기의 JPEG. 파일 이름(확장자)은 'thumb_' + 원본 이름 그대로 둔다(DB THUMB_FILENAME 규칙).
        /// 왜 따로 만드는가 (2026-10-21): Core ImageLoader.SaveThumbnailImage 는 형식을 정하지 않고 저장해 실제 내용이 PNG 가 되고
        /// (이름은 .jpg) 원본 JPEG 보다 약 5배 커진다. Core 는 기존 자동 적재도 쓰므로 고치지 않고 업로드만 여기서 만든다.
        /// </summary>
        private static void SaveJpegThumbnail(Image image, string thumbPath)
        {
            int width = Math.Max(1, image.Width / 2);
            int height = Math.Max(1, image.Height / 2);
            System.Drawing.Imaging.ImageCodecInfo jpeg = null;
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                if (codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid) jpeg = codec;

            using (Bitmap thumb = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(thumb))
                {
                    g.Clear(Color.White);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, width, height);
                }
                using (System.Drawing.Imaging.EncoderParameters parameters = new System.Drawing.Imaging.EncoderParameters(1))
                {
                    parameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ThumbnailJpegQuality);
                    if (jpeg != null)
                        thumb.Save(thumbPath, jpeg, parameters);
                    else
                        thumb.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
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
                                                  "SETUP_ID", "SETUP_TIME", "RESULTTIMESTAMP", "EQUIP", "IMAGE_NAMES", "IMAGE_PATH", "RESULT", "MESSAGE", "MAP_NOTE" })
                dt.Columns.Add(col, typeof(string));
            // MAP_DIES = Map Setup 사용 Die 수(SETUP_MAP 저장 수), MAP_NOTE = Map Setup 과 KLARF 가 다른 점 (2026-10-13)
            foreach (string col in new string[] { "SLOT_ID", "TEST_COUNT", "INSPECTED_DIE", "DEFECTS", "IMAGE_FILES", "SAVED_IMAGES", "ANGLE", "DIE_ORIGIN_X", "DIE_ORIGIN_Y", "MAP_DIES" })
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
        /// Map Setup 이 없거나 쓸 수 없으면 INVALID, KLARF 와 다른 점(Pitch / 원점 / Map 밖 Die)은 MESSAGE 에 참고로 남긴다.
        /// </summary>
        /// <param name="map">LoadSetupMapAndAlign 결과. null = Map Setup 없음</param>
        private static DataRow BuildWaferRow(DataTable table, UploadSetting setting, ParserKlarf parser, Wafer wafer, Dictionary<int, string> rawIds, bool checkExisting, SetupMapLayout map)
        {
            DataRow row = table.NewRow();
            string rawId;
            string product = ResolveProduct(setting, parser);
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
            row["TEST_COUNT"] = wafer.TestList.Count;
            row["INSPECTED_DIE"] = dies.Count;
            row["DEFECTS"] = wafer.DefectList.Count;
            row["IMAGE_FILES"] = imageNames.Count;
            row["IMAGE_NAMES"] = String.Join(";", imageNames.ToArray());
            row["ANGLE"] = parser.GetAngle();
            // 저장할 원점 번호: Map Setup 이 있으면 Map Setup 원점(Die 번호도 이 값으로 옮겨져 있음), 없으면 Parser 값
            bool mapUsable = map != null && map.Error.Length == 0;
            row["DIE_ORIGIN_X"] = mapUsable ? map.OriginIndex.X : (int)wafer.DieOriginX;
            row["DIE_ORIGIN_Y"] = mapUsable ? map.OriginIndex.Y : (int)wafer.DieOriginY;
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
            // Device(Product)에 Setup 된 Map(TQP_MAPDEF / TQP_USEMAP, Factory + MAPID = Product)이 없거나 쓸 수 없으면 저장하지 않는다 (2026-10-13)
            // 업로드는 이 Map 으로 SETUP / SETUP_MAP 을 만들고 Defect 조회 화면이 그대로 그리므로, Map 없이 저장할 수 없다.
            if (product.Length > 0 && map == null)
                errors.Add(String.Format("Device {0} 의 Map 이 없습니다 (Factory {1}). Map Setup(Map Define / File Map Setup)에서 먼저 등록하세요.", product, setting.Factory));
            else if (map != null && map.Error.Length > 0)
                errors.Add(String.Format("Device {0} 의 Map Setup 을 쓸 수 없습니다: {1}", product, map.Error));

            string mapNote = map != null && map.Error.Length == 0 ? BuildMapNote(parser, wafer, dies, map) : "";
            row["MAP_DIES"] = map != null && map.Error.Length == 0 ? map.Dies.Count : 0;
            row["MAP_NOTE"] = mapNote;

            if (errors.Count > 0)
            {
                row["RESULT"] = "INVALID";
                row["MESSAGE"] = String.Join(" / ", errors.ToArray());
            }
            else
            {
                row["RESULT"] = "READY";
                row["MESSAGE"] = mapNote;
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
                        row["MESSAGE"] = JoinMessage("이미 등록됨 (STEP_SEQ " + existing + ")", mapNote);
                    }
                }
            }
            return row;
        }

        /// <summary>화면 입력 Product 가 비면 KLARF DeviceID 를 쓴다. BuildWaferRow 와 Map Setup 조회가 같은 값을 쓰도록 한 곳에 둔다.</summary>
        private static string ResolveProduct(UploadSetting setting, ParserKlarf parser)
        {
            return setting.Product.Length > 0 ? setting.Product : Trim(parser.DeviceID);
        }

        private static string JoinMessage(string first, string second)
        {
            if (String.IsNullOrEmpty(second)) return first;
            if (String.IsNullOrEmpty(first)) return second;
            return first + " / " + second;
        }

        #region [Map Setup 기준 Map (2026-10-13)]

        /// <summary>
        /// Map Setup(TQP_MAPDEF + TQP_USEMAP)에 등록된 Device Map 을 업로드 저장 기준값으로 바꾼 것.
        /// 만든 이유: Defect 조회 화면(Defect Map Viewer 등)은 TQD_SETUP / TQD_SETUP_MAP 으로 Die 를 그린다. KLARF 검사 Die(SampleTestPlan)를
        ///   그대로 넣으면 검사한 Die 만 보이고 KLARF Pitch 로 계산한 모양이 되므로, Map Setup 의 사용 Die / Pitch / 원점을 넣고 KLARF 는 Defect 만 쓴다.
        /// 번호: Map Setup 은 XY_DIRECTION 방향 번호로 저장되고 Defect 조회 화면은 LeftBottom 번호를 쓴다.
        ///   File Map Setup / Wafer Map 조회 화면과 같은 식(TEST.ENGUI KlarfMapRule.FromDirection / StoredMapLayout.ToLeftBottomOrigin)으로 바꾼다.
        ///   BSL 은 TEST.ENGUI 를 참조할 수 없어 같은 식을 여기에 둔다 (FromStored / ToLeftBottomOrigin).
        /// </summary>
        private class SetupMapLayout
        {
            public string MapId = "";
            /// <summary>쓸 수 없는 Map 이면 이유. 빈 문자열이면 사용 가능.</summary>
            public string Error = "";
            /// <summary>WAFER_SIZE (um).</summary>
            public double WaferSize;
            /// <summary>CHIP_SIZE_X / Y (um). Die Pitch 로 저장한다.</summary>
            public double PitchX;
            public double PitchY;
            /// <summary>원점 (um). GET_MAPDEF 가 Y 를 CHIP_SIZE_Y - ORIGIN_MICRO_Y 로 돌려준 값으로 KLARF SampleCenterLocation 과 같은 기준이다.</summary>
            public double OriginX;
            public double OriginY;
            /// <summary>원점 Die 번호 (LeftBottom). KLARF 원본 (0,0) Die 가 이 번호가 되도록 Defect 번호를 옮긴다.</summary>
            public Point OriginIndex;
            /// <summary>사용 Die (USECODE 1) LeftBottom 번호. SETUP_MAP 에 그대로 넣는다.</summary>
            public List<Point> Dies = new List<Point>();
            public HashSet<Point> DieSet = new HashSet<Point>();
        }

        /// <summary>
        /// Map Setup 을 읽고, 쓸 수 있으면 모든 Wafer 의 Die 번호(SampleTestPlan / Defect)를 Map Setup 원점 기준으로 옮긴다.
        /// 왜: Parser 는 저장된 Setup 원점 또는 WaferDieCalculator 계산값으로 번호를 옮긴다. Map Setup 원점과 다르면 Defect 가 다른 Die 에 그려진다.
        /// Product 가 비었거나 Map 이 없으면 null (BuildWaferRow 가 INVALID 로 표시).
        /// </summary>
        private static SetupMapLayout LoadSetupMapAndAlign(UploadSetting setting, ParserKlarf parser)
        {
            string product = ResolveProduct(setting, parser);
            if (product.Length == 0)
                return null;
            SetupMapLayout map = LoadSetupMap(setting.Factory, product);
            if (map == null || map.Error.Length > 0)
                return map;

            foreach (Wafer wafer in parser.Wafers)
            {
                // Wafer.DieOriginX / Y 는 Parser 안에서만 바꿀 수 있어(internal set) 그대로 두고 번호만 옮긴다.
                // 그래서 이 함수는 파싱 1번에 1번만 부른다. 이후 원점은 wafer.DieOriginX 가 아니라 map.OriginIndex 를 쓴다.
                int dx = map.OriginIndex.X - (int)wafer.DieOriginX;
                int dy = map.OriginIndex.Y - (int)wafer.DieOriginY;
                if (dx != 0 || dy != 0)
                    wafer.AddIndex(dx, dy);
            }
            return map;
        }

        /// <summary>
        /// Map Setup 1건을 읽는다. 기존 TEST DSL 의 GetMapDef(GET_MAPDEF) / GetDies(SELECT_USEMAP_ALLDIE) 를 그대로 쓰고,
        /// 두 쿼리가 MAPID 로만 찾으므로 Factory 는 여기서 거른다.
        /// 흐름: 정의 행 찾기(Factory / 삭제 아님) -> 값 단위 확인 -> XY 방향 -> 원점 번호 LeftBottom 변환 -> 사용 Die 번호 변환
        /// </summary>
        private static SetupMapLayout LoadSetupMap(string factory, string mapId)
        {
            DataTable dtDef = null;
            DataTable dtDies = null;
            try
            {
                dtDef = new DACrux.TEST.DSL.TQP_MAPDEF().GetMapDef(mapId);
                DataRow def = null;
                bool deleted = false;
                if (dtDef != null)
                {
                    foreach (DataRow r in dtDef.Rows)
                    {
                        if (!String.Equals(Trim(r["FACTORY"].ToString()), factory, StringComparison.OrdinalIgnoreCase))
                            continue;
                        if (r["DELETE_FLAG"].ToString() == "Y") { deleted = true; continue; }
                        def = r;
                        break;
                    }
                }
                if (def == null && !deleted)
                    return null;

                SetupMapLayout map = new SetupMapLayout();
                map.MapId = mapId;
                if (def == null)
                {
                    map.Error = "삭제된 Map 입니다 (DELETE_FLAG = Y)";
                    return map;
                }

                map.WaferSize = MapNumber(def, "WAFER_SIZE");
                map.PitchX = MapNumber(def, "CHIP_SIZE_X");
                map.PitchY = MapNumber(def, "CHIP_SIZE_Y");
                map.OriginX = MapNumber(def, "ORIGIN_MICRO_X");
                map.OriginY = MapNumber(def, "ORIGIN_MICRO_Y");
                // 단위 확인: um 저장이 기준이다 (StoredMapLayout 과 같은 경계). mm 로 들어간 Map 은 Die 수가 맞지 않아 저장하지 않는다.
                if (map.WaferSize < 1000)
                    map.Error = "WAFER_SIZE 가 um 값이 아닙니다 (" + Num(map.WaferSize) + ")";
                else if (map.PitchX < 10 || map.PitchY < 10)
                    map.Error = "CHIP_SIZE 가 um 값이 아닙니다 (" + Num(map.PitchX) + " x " + Num(map.PitchY) + ")";

                int dir = (int)MapNumber(def, "XY_DIRECTION");
                if (map.Error.Length == 0 && !Enum.IsDefined(typeof(XYDirection), dir))
                    map.Error = "XY_DIRECTION 값이 올바르지 않습니다 (" + dir + ")";
                if (map.Error.Length > 0)
                    return map;

                XYDirection direction = (XYDirection)dir;
                int sumX = (int)MapNumber(def, "DIE_INDEX_MIN_X") + (int)MapNumber(def, "DIE_INDEX_MAX_X");
                int sumY = (int)MapNumber(def, "DIE_INDEX_MIN_Y") + (int)MapNumber(def, "DIE_INDEX_MAX_Y");
                map.OriginIndex = ToLeftBottomOrigin(direction,
                    new Point((int)MapNumber(def, "ORIGIN_INDEX_X"), (int)MapNumber(def, "ORIGIN_INDEX_Y")), sumX, sumY);

                dtDies = new DACrux.TEST.DSL.TQP_USEMAP().GetDies(mapId);
                if (dtDies != null)
                {
                    foreach (DataRow r in dtDies.Rows)
                    {
                        if (!String.Equals(Trim(r["FACTORY"].ToString()), factory, StringComparison.OrdinalIgnoreCase))
                            continue;
                        if ((int)MapNumber(r, "USECODE") != 1)
                            continue;
                        Point lb = FromStored(new Point((int)MapNumber(r, "X"), (int)MapNumber(r, "Y")), direction, sumX, sumY);
                        if (map.DieSet.Add(lb))
                            map.Dies.Add(lb);
                    }
                }
                if (map.Dies.Count == 0)
                    map.Error = "사용 Die(USECODE 1)가 없습니다";
                return map;
            }
            finally
            {
                if (dtDef != null) dtDef.Dispose();
                if (dtDies != null) dtDies.Dispose();
            }
        }

        /// <summary>저장 방향 번호 -> LeftBottom 번호. Right 계열은 X, Top 계열은 Y 를 저장 범위(MIN + MAX) 안에서 뒤집는다 (KlarfMapRule.FromDirection 과 같음).</summary>
        private static Point FromStored(Point stored, XYDirection direction, int sumX, int sumY)
        {
            int x = stored.X;
            int y = stored.Y;
            if (direction == XYDirection.RightBottom || direction == XYDirection.RightTop)
                x = sumX - x;
            if (direction == XYDirection.LeftTop || direction == XYDirection.RightTop)
                y = sumY - y;
            return new Point(x, y);
        }

        /// <summary>
        /// 저장 원점 번호 -> LeftBottom 원점 번호 (StoredMapLayout.ToLeftBottomOrigin 과 같음).
        /// 저장값은 기존 WaferMap.AddDie 의 방향별 원점 모서리 차이를 맞춘 KlarfMapRule.MapOriginIndex 결과이므로 그 식을 거꾸로 푼다.
        /// </summary>
        private static Point ToLeftBottomOrigin(XYDirection direction, Point stored, int sumX, int sumY)
        {
            switch (direction)
            {
                case XYDirection.LeftTop:
                    return new Point(stored.X + 1, sumY - stored.Y);
                case XYDirection.RightTop:
                    return new Point(sumX - stored.X, sumY - stored.Y);
                case XYDirection.RightBottom:
                    return new Point(sumX - stored.X, stored.Y);
                default:
                    return stored;
            }
        }

        private static double MapNumber(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row.IsNull(column))
                return 0;
            return System.Convert.ToDouble(row[column], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Map Setup 과 KLARF 가 다른 점을 한 줄로 만든다 (저장은 막지 않음). 비어 있으면 같다.
        /// Pitch 가 다르면 KLARF XREL 이 KLARF Pitch 기준이라 Defect 가 Die 안에서 조금 밀릴 수 있다 (F05 Pitch 설정과 같은 제한).
        /// Map 밖 검사 Die / Defect 는 Defect 조회 화면에서 Die 없이 보이거나 빠질 수 있다.
        /// </summary>
        private static string BuildMapNote(ParserKlarf parser, Wafer wafer, List<Point> inspectedDies, SetupMapLayout map)
        {
            List<string> notes = new List<string>();
            if (Math.Abs(parser.DiePitchX - map.PitchX) > 0.001 || Math.Abs(parser.DiePitchY - map.PitchY) > 0.001)
                notes.Add(String.Format(CultureInfo.InvariantCulture, "Pitch 다름 (KLARF {0} x {1} / Map {2} x {3})", parser.DiePitchX, parser.DiePitchY, map.PitchX, map.PitchY));
            if (Math.Abs(wafer.SampleCenterLocationX - map.OriginX) > 1 || Math.Abs(wafer.SampleCenterLocationY - map.OriginY) > 1)
                notes.Add(String.Format(CultureInfo.InvariantCulture, "원점 다름 (KLARF {0} / {1}, Map {2} / {3} um)",
                    wafer.SampleCenterLocationX, wafer.SampleCenterLocationY, map.OriginX, map.OriginY));
            int outDies = 0;
            foreach (Point pt in inspectedDies)
                if (!map.DieSet.Contains(pt)) outDies++;
            if (outDies > 0)
                notes.Add("Map 밖 검사 Die " + outDies);
            int outDefects = 0;
            foreach (Defect d in wafer.DefectList)
                if (!map.DieSet.Contains(new Point(d.XINDEX, d.YINDEX))) outDefects++;
            if (outDefects > 0)
                notes.Add("Map 밖 Defect " + outDefects);
            return notes.Count == 0 ? "" : "Map Setup 참고: " + String.Join(", ", notes.ToArray());
        }

        /// <summary>저장된 Setup 의 Wafer 크기 / Pitch / 원점이 Map Setup 값과 같은지 (기존 GET_SETUP 쿼리).</summary>
        private static bool SetupMatchesMap(TQD_SETUP oTQD_SETUP, long setupSeq, SetupMapLayout map)
        {
            DataTable dt = oTQD_SETUP.GetData("GET_SETUP", null, new string[] { setupSeq.ToString(CultureInfo.InvariantCulture) });
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;
                DataRow r = dt.Rows[0];
                return Same(MapNumber(r, "WAFER_SIZE"), map.WaferSize)
                    && Same(MapNumber(r, "DIE_PITCH_X"), map.PitchX) && Same(MapNumber(r, "DIE_PITCH_Y"), map.PitchY)
                    && Same(MapNumber(r, "DIE_ORIGIN_X"), map.OriginIndex.X) && Same(MapNumber(r, "DIE_ORIGIN_Y"), map.OriginIndex.Y)
                    && Same(MapNumber(r, "ORIGIN_X"), map.OriginX) && Same(MapNumber(r, "ORIGIN_Y"), map.OriginY);
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        /// <summary>Setup Map 의 Test 1개 Die 목록이 Map Setup 사용 Die 와 같은지 (개수 + 모든 번호).</summary>
        private static bool SetupMapMatches(DataTable dtMap, SetupMapLayout map)
        {
            HashSet<Point> stored = new HashSet<Point>();
            foreach (DataRow r in dtMap.Rows)
                stored.Add(new Point((int)MapNumber(r, "INDEX_X"), (int)MapNumber(r, "INDEX_Y")));
            return stored.Count == dtMap.Rows.Count && stored.SetEquals(map.DieSet);
        }

        private static bool Same(double a, double b)
        {
            return Math.Abs(a - b) < 0.0005;
        }

        #endregion

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

            public static UploadSetting FromArray(string[] info)
            {
                if (info == null || info.Length != Enum.GetNames(typeof(UploadInfoIndex)).Length)
                    throw new Exception("UploadInfo 는 7개 항목이어야 합니다. (Factory / User / 파일명 / Product / Step ID / 설비 / 예약(빈 값))");
                UploadSetting s = new UploadSetting();
                s.Factory = Required(info[(int)UploadInfoIndex.FACTORY], "Factory", IdColumnLength);
                s.User = Required(info[(int)UploadInfoIndex.USER], "User", 30);
                s.FileName = SafeFileName(info[(int)UploadInfoIndex.FILE_NAME], "KLARF 파일 이름");
                if (s.FileName.Length > 256)
                    throw new Exception("KLARF 파일 이름이 256자를 넘습니다.");
                s.Product = Trim(info[(int)UploadInfoIndex.PRODUCT]);
                s.StepId = Trim(info[(int)UploadInfoIndex.STEP_ID]);
                s.Equip = Trim(info[(int)UploadInfoIndex.EQUIP]);
                return s;
            }

            public string[] ToArray()
            {
                return new string[] { Factory, User, FileName, Product, StepId, Equip, "" };
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
