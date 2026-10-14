using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    // =====================================================================================================
    //  E-Test 결과 -> YMS 테이블 적재 (2026-10-14)
    // -----------------------------------------------------------------------------------------------------
    //  목적
    //   E-Test Wafer 1장(파일 1개)을 YMS 가 읽는 테이블에 넣어, 기존 YMS 화면(TEST Map View / Gallery / Cum / Bin Stack /
    //   Yield / Param Analysis / Standard Report / Test Report)에서 바로 조회되게 한다.
    //   설계: 문서/2026-10-14_ETest_Upload_YMS적재_설계.md 14절.
    //
    //  처리 순서 (YMS 설비 파일 적재 PCMDataService 와 같은 기존 함수를 같은 순서로 부른다)
    //   1. 입력 확인 (YMS 컬럼 길이 / 이름 규칙 / Die 좌표)
    //   2. 이전 반영 삭제          TestDataMaint.DeleteWaferData    (다시 반영해도 결과가 같게)
    //   3. Product / Program 등록  TestCommon.InsertProductInfo / InsertProgramInfo / GetMaxProgramRev
    //   4. Lot / Wafer             TestCommon.SaveWaferData         -> LOT_SEQ / WAFER_SEQ (콜백으로 바로 알림)
    //   5. Die 데이터              TestDataManager.SaveData         -> TD_<PROGRAM> (없으면 테이블 / 컬럼 / PARASPEC 자동 생성)
    //   6. Parameter 정의 보정     UPDATE_YMS_PARASPEC              -> 설명(헤더 원문) / 단위 / LSL / USL / 형식
    //   7. Bin 정의                TQP_BINDESC                      -> 1 PASS / 2 SPEC_FAIL / 3 NOT_TESTED (없을 때만)
    //   8. 요약                    TestCommon.SummaryData           -> TQP_WAFER_SUM / TQP_LOT_SUM (수율)
    //
    //  저장 경계
    //   하나의 트랜잭션이 아니다. TD 테이블 생성 / 컬럼 추가가 DDL(자동 Commit)이고 기존 함수들이 각각 Commit 한다.
    //   그래서 4번에서 WAFER_SEQ 를 콜백으로 바로 넘겨 호출하는 쪽이 기록하게 하고, 다음 반영 때 2번이 반쯤 들어간 데이터를 지운다.
    //   실패는 예외로 던지지 않고 결과(Status FAIL / Step / Message)로 돌려준다.
    //
    //  입력 (EtYmsWafer)
    //   Die 의 MapX / MapY = 시스템 Map 좌표 (File Map Setup Map 기준, LeftBottom (1,1)). YMS Map 은 이 값을 변환 없이 Map Index 로 쓴다.
    //   Map 좌표가 없는 Die(Map 밖)는 넣지 않고 SkipDieCount 로 센다. Map 이 아예 없는 파일은 호출하는 쪽이 보류한다.
    //   Die 판정 / Fail 수는 E-Test 판정 결과를 그대로 받는다. 값이 없는('-' / 빈 칸) Parameter 는 null -> TD 에 NULL.
    // =====================================================================================================

    /// <summary>YMS 에 넣을 Parameter 1개 (TD 컬럼 1개)</summary>
    [Serializable]
    public class EtYmsParameter
    {
        /// <summary>E-Test Parameter 이름 (헤더 정리 값)</summary>
        public string ParameterName { get; set; }
        /// <summary>YMS 컬럼 이름 (EtYmsRule.ToParamName 으로 만든 값)</summary>
        public string YmsName { get; set; }
        /// <summary>TQP_PARASPEC.PARAM_DESC 에 넣을 헤더 원문 (없으면 ParameterName)</summary>
        public string Description { get; set; }
        /// <summary>단위 (TQP_PARASPEC.UOM)</summary>
        public string Unit { get; set; }
        /// <summary>Spec 하한 (TQP_PARASPEC.LSL), 없으면 null</summary>
        public double? Lower { get; set; }
        /// <summary>Spec 상한 (TQP_PARASPEC.USL), 없으면 null</summary>
        public double? Upper { get; set; }
    }

    /// <summary>YMS 에 넣을 Die 1개 (TD 행 1개)</summary>
    [Serializable]
    public class EtYmsDie
    {
        /// <summary>E-Test DUT 번호 (메시지 표시용)</summary>
        public int DutNo { get; set; }
        /// <summary>시스템 Map X (없으면 Map 밖 Die)</summary>
        public int? MapX { get; set; }
        /// <summary>시스템 Map Y (없으면 Map 밖 Die)</summary>
        public int? MapY { get; set; }
        /// <summary>Die 판정 P / F</summary>
        public string Judge { get; set; }
        /// <summary>범위 밖으로 Fail 인 값 수</summary>
        public int RangeFailCount { get; set; }
        /// <summary>'-' 로 Fail 인 값 수</summary>
        public int NotTestedFailCount { get; set; }
        /// <summary>Parameter 순서와 같은 측정값. 값이 없으면 null</summary>
        public double?[] Values { get; set; }
    }

    /// <summary>YMS 에 넣을 Wafer 1장 (E-Test 파일 1개)</summary>
    [Serializable]
    public class EtYmsWafer
    {
        public EtYmsWafer()
        {
            WaferDiameterMm = 300;
            Parameters = new List<EtYmsParameter>();
            Dies = new List<EtYmsDie>();
        }

        public string Factory { get; set; }
        /// <summary>E-Test 파일 Device (YMS DEVICE / PRODUCT)</summary>
        public string DeviceId { get; set; }
        /// <summary>공정 = 파일명 Test Type (YMS TESTAREA, Program 의 뒤쪽)</summary>
        public string TestProgram { get; set; }
        /// <summary>좌표 계산에 쓴 Map ID (YMS DEVICE_ALIAS = TQP_MAPDEF.MAPID)</summary>
        public string MapId { get; set; }
        public string LotId { get; set; }
        public string WaferId { get; set; }
        /// <summary>파일명 Lot-Wafer No 원문 (TQP_WAFER.COMMENTS)</summary>
        public string LotWaferNo { get; set; }
        /// <summary>원본 파일명 (TQP_WAFER.DATA_SOURCE)</summary>
        public string FileName { get; set; }
        /// <summary>파일명 Test Date (YMS 시작 / 종료 시간)</summary>
        public DateTime TestTime { get; set; }
        /// <summary>Wafer 지름 mm (Map WAFER_SIZE / 1000, 모르면 300)</summary>
        public int WaferDiameterMm { get; set; }
        public List<EtYmsParameter> Parameters { get; set; }
        public List<EtYmsDie> Dies { get; set; }
    }

    /// <summary>YMS 적재 결과</summary>
    [Serializable]
    public class EtYmsPublishResult
    {
        public string Status { get; set; }
        /// <summary>마지막으로 진행한 단계 (실패 위치)</summary>
        public string Step { get; set; }
        public string Message { get; set; }
        public string Program { get; set; }
        public string TestArea { get; set; }
        public string LotSeq { get; set; }
        public string WaferSeq { get; set; }
        public int DieCount { get; set; }
        public int SkipDieCount { get; set; }
        public int ParamCount { get; set; }
        public int Bin1Count { get; set; }
        public int Bin2Count { get; set; }
        public int Bin3Count { get; set; }
        public decimal? NetDie { get; set; }
        public decimal? TestedDie { get; set; }
        public decimal? Yield { get; set; }

        /// <summary>원격 호출 결과로 넘기기 위한 1행 표 (열 이름은 설계 14.6절)</summary>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable("YMS_RESULT");
            foreach (string column in new string[] { "YMS_STATUS", "YMS_STEP", "YMS_MESSAGE", "YMS_PROGRAM", "YMS_TESTAREA", "YMS_LOT_SEQ", "YMS_WAFER_SEQ" })
                dt.Columns.Add(column, typeof(string));
            foreach (string column in new string[] { "DIE_COUNT", "SKIP_DIE_COUNT", "PARAM_COUNT", "BIN1_COUNT", "BIN2_COUNT", "BIN3_COUNT" })
                dt.Columns.Add(column, typeof(int));
            foreach (string column in new string[] { "NETDIE", "TESTED_DIE", "YIELD" })
                dt.Columns.Add(column, typeof(decimal));

            DataRow row = dt.NewRow();
            row["YMS_STATUS"] = Status;
            row["YMS_STEP"] = Step;
            row["YMS_MESSAGE"] = Message;
            row["YMS_PROGRAM"] = Program;
            row["YMS_TESTAREA"] = TestArea;
            row["YMS_LOT_SEQ"] = LotSeq;
            row["YMS_WAFER_SEQ"] = WaferSeq;
            row["DIE_COUNT"] = DieCount;
            row["SKIP_DIE_COUNT"] = SkipDieCount;
            row["PARAM_COUNT"] = ParamCount;
            row["BIN1_COUNT"] = Bin1Count;
            row["BIN2_COUNT"] = Bin2Count;
            row["BIN3_COUNT"] = Bin3Count;
            row["NETDIE"] = NetDie.HasValue ? (object)NetDie.Value : DBNull.Value;
            row["TESTED_DIE"] = TestedDie.HasValue ? (object)TestedDie.Value : DBNull.Value;
            row["YIELD"] = Yield.HasValue ? (object)Yield.Value : DBNull.Value;
            dt.Rows.Add(row);
            return dt;
        }
    }

    /// <summary>
    /// E-Test Wafer 를 YMS 테이블에 넣고 지운다. 원격 서비스 클래스가 아니라 GdsEtResult 등 서버 코드가 같은 프로세스에서 쓴다.
    /// YMS 쪽 함수(TEST BSL / DSL)는 Service 설정의 TEST_CONNECT_ID 로, 보정 쿼리는 DMS_CONNECT_ID 로 같은 DB 에 붙는다.
    /// </summary>
    public class EtYmsPublisher
    {
        public const string StatusDone = "DONE";
        public const string StatusFail = "FAIL";

        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";
        private const int MaxCommentsChars = 256;
        private const int MaxDataSourceChars = 1000;
        private const int MaxParamDescChars = 200;
        private const int MaxUnitChars = 30;
        private const int MaxMessageChars = 500;

        /// <summary>
        /// Wafer 1장을 YMS 에 넣는다.
        /// previousWaferSeq: 이 파일을 전에 반영한 YMS WAFER_SEQ (있으면 먼저 지운다).
        /// onWaferCreated: Lot / Wafer 를 만든 직후 호출 (LOT_SEQ / WAFER_SEQ 를 바로 기록하는 용도). 없으면 null.
        /// </summary>
        public EtYmsPublishResult Publish(EtYmsWafer wafer, string previousWaferSeq, string updateUser, Action<EtYmsPublishResult> onWaferCreated)
        {
            EtYmsPublishResult result = new EtYmsPublishResult();
            result.Status = StatusFail;
            string step = "입력 확인";
            try
            {
                List<EtYmsDie> dies = CheckInput(wafer, result);
                string factory = wafer.Factory.Trim();
                string device = wafer.DeviceId.Trim().ToUpperInvariant();
                string mapId = wafer.MapId.Trim();
                string waferId = wafer.WaferId.Trim();

                step = "이전 반영 삭제";
                if (!string.IsNullOrEmpty(previousWaferSeq))
                    DeleteWafer(previousWaferSeq);

                step = "Product / Program";
                DACrux.TEST.BSL.TestCommon common = new DACrux.TEST.BSL.TestCommon();
                common.InsertProductInfo(factory, device, mapId);
                common.InsertProgramInfo(factory, result.Program, result.TestArea, device);
                int programRev = common.GetMaxProgramRev(factory, result.Program);

                step = "Lot / Wafer";
                int maxX = 0, maxY = 0;
                foreach (EtYmsDie die in dies)
                {
                    maxX = Math.Max(maxX, die.MapX.Value);
                    maxY = Math.Max(maxY, die.MapY.Value);
                }
                string time = wafer.TestTime.ToString(TimeFormat, CultureInfo.InvariantCulture);
                string lotSeq, waferSeq;
                common.SaveWaferData(
                    factory, result.TestArea, device, mapId, wafer.LotId.Trim(), waferId, time, time,
                    wafer.WaferDiameterMm > 0 ? wafer.WaferDiameterMm : 300,
                    null, null, null, null,                         // TESTER / PROBER / PROBE_CARD / OPERATOR: 파일에 없음
                    result.Program, programRev, -1, null,
                    maxX, maxY, "0",
                    Cut(wafer.LotWaferNo, MaxCommentsChars),        // COMMENTS: Lot-Wafer No 원문
                    null, null, null,                               // OVERDRIVE / TEMP / PGMPATH
                    Cut(wafer.FileName, MaxDataSourceChars),        // DATA_SOURCE: 원본 파일명
                    null, null, null, null,                         // MAXSITE / TFVERSION / PROBECARD_TOUCHDOWNS / SMSFAMILY_NAME
                    dies.Count, true, out lotSeq, out waferSeq);
                result.LotSeq = lotSeq;
                result.WaferSeq = waferSeq;
                if (onWaferCreated != null)
                    onWaferCreated(result);

                step = "Die 데이터";
                DACrux.TEST.BSL.TestDataManager.SaveData(factory, result.Program, BuildRawTable(wafer.Parameters, dies), waferSeq);

                step = "Parameter 정의";
                UpdateParameterSpec(factory, result.Program, programRev, wafer.Parameters, updateUser);

                step = "Bin 정의";
                RegisterBins(result.Program);

                step = "요약";
                common.SummaryData(factory, lotSeq, waferSeq, waferId);

                step = "결과 확인";
                DataTable summary = GetWaferSummary(waferSeq);
                if (summary.Rows.Count == 0)
                    throw new InvalidOperationException("TQP_WAFER_SUM 행이 만들어지지 않았습니다.");
                result.NetDie = ToDecimal(summary.Rows[0]["NETDIE"]);
                result.TestedDie = ToDecimal(summary.Rows[0]["TESTED_DIE"]);
                result.Yield = ToDecimal(summary.Rows[0]["YIELD"]);

                result.Status = StatusDone;
                result.Step = "완료";
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                result.Status = StatusFail;
                result.Step = step;
                result.Message = Cut(step + ": " + inner.Message, MaxMessageChars);
            }
            return result;
        }

        /// <summary>
        /// YMS Wafer 를 지운다 (기존 TestDataMaint.DeleteWaferData: Wafer / Wafer 요약 / TD 행 삭제, 빈 TD 테이블 정리,
        /// Lot 요약 재생성, 빈 Lot 삭제). 이미 없으면 아무것도 하지 않고 false.
        /// </summary>
        public bool DeleteWafer(string waferSeq)
        {
            if (string.IsNullOrEmpty(waferSeq) || GetWafer(waferSeq).Rows.Count == 0)
                return false;
            DACrux.TEST.BSL.TestDataMaint maint = new DACrux.TEST.BSL.TestDataMaint();
            maint.DeleteWaferData(waferSeq);
            return true;
        }

        /// <summary>YMS Wafer 1건 (기존 쿼리 TQP_WAFER.xml GET_WAFER_INFO [WSQ]). 없으면 0행.</summary>
        public DataTable GetWafer(string waferSeq)
        {
            DACrux.TEST.DSL.TQP_WAFER oWafer = new DACrux.TEST.DSL.TQP_WAFER();
            return oWafer.GetData("GET_WAFER_INFO [WSQ]", null, new string[] { waferSeq });
        }

        /// <summary>YMS Wafer 요약 1건 (기존 쿼리 TQP_WAFER_SUM.xml GET_WAFER_LOT_INFO, TEST Map View 와 같은 조회). 없으면 0행.</summary>
        public DataTable GetWaferSummary(string waferSeq)
        {
            DACrux.TEST.DSL.TQP_WAFER_SUM oWaferSum = new DACrux.TEST.DSL.TQP_WAFER_SUM();
            return oWaferSum.GetData("GET_WAFER_LOT_INFO", null, new string[] { waferSeq });
        }

        /// <summary>
        /// 입력을 확인하고 YMS 에 넣을 Die 를 고른다 (Map 좌표 있는 Die, 위 행부터 / 같은 행은 왼쪽부터).
        /// 시스템 좌표(LeftBottom) 의 "Y 큰 것부터, X 작은 것부터" 순서가 고객 통합 Die# 순서(LeftTop 행 우선)와 같아,
        /// YMS DIE_NUM(넘긴 행 순서)이 Die# 순서로 붙는다. Die# 값 자체는 넣지 않는다.
        /// </summary>
        private static List<EtYmsDie> CheckInput(EtYmsWafer wafer, EtYmsPublishResult result)
        {
            if (wafer == null)
                throw new ArgumentNullException("wafer");
            RequiredText(wafer.Factory, "Factory", 30);
            string device = RequiredText(wafer.DeviceId, "Device", EtYmsRule.MaxDeviceLength);
            RequiredText(wafer.MapId, "Map ID", EtYmsRule.MaxDeviceLength);
            RequiredText(wafer.LotId, "Lot ID", EtYmsRule.MaxLotIdLength);
            RequiredText(wafer.WaferId, "Wafer ID", EtYmsRule.MaxWaferIdLength);
            string testArea = EtYmsRule.ToTestArea(RequiredText(wafer.TestProgram, "공정(Test Type)", EtYmsRule.MaxTestAreaLength));
            if (wafer.TestTime == DateTime.MinValue)
                throw new ArgumentException("시험 일시가 없습니다.");

            string program = EtYmsRule.ToProgramName(device, testArea);
            if (program.Length == 0 || program.Length > EtYmsRule.MaxProgramLength)
                throw new ArgumentException("YMS Program 이름을 만들 수 없거나 " + EtYmsRule.MaxProgramLength + "자를 넘습니다: " + program);
            result.Program = program;
            result.TestArea = testArea;

            List<EtYmsParameter> parameters = wafer.Parameters ?? new List<EtYmsParameter>();
            HashSet<string> names = new HashSet<string>(StringComparer.Ordinal);
            foreach (EtYmsParameter parameter in parameters)
            {
                if (parameter == null || !EtYmsRule.IsValidParamName(parameter.YmsName))
                    throw new ArgumentException("YMS 컬럼 이름이 올바르지 않습니다: " + (parameter == null ? "(null)" : parameter.YmsName + " / " + parameter.ParameterName));
                if (!names.Add(parameter.YmsName))
                    throw new ArgumentException("같은 YMS 컬럼 이름이 두 번 있습니다: " + parameter.YmsName);
            }
            result.ParamCount = parameters.Count;

            List<EtYmsDie> dies = new List<EtYmsDie>();
            HashSet<long> positions = new HashSet<long>();
            foreach (EtYmsDie die in wafer.Dies ?? new List<EtYmsDie>())
            {
                if (die == null)
                    continue;
                int valueCount = die.Values == null ? 0 : die.Values.Length;
                if (valueCount != parameters.Count)
                    throw new ArgumentException("DUT #" + die.DutNo + " 의 값 수(" + valueCount + ")가 Parameter 수(" + parameters.Count + ")와 다릅니다.");
                int bin = EtYmsRule.ToBin(die.Judge, die.RangeFailCount, die.NotTestedFailCount);
                if (!die.MapX.HasValue || !die.MapY.HasValue)
                {
                    result.SkipDieCount++;
                    continue;
                }
                if (!positions.Add(((long)die.MapX.Value << 32) | (uint)die.MapY.Value))
                    throw new ArgumentException("같은 Map 좌표 Die 가 두 번 있습니다: (" + die.MapX.Value + ", " + die.MapY.Value + ")");
                if (bin == EtYmsRule.BinPass) result.Bin1Count++;
                else if (bin == EtYmsRule.BinSpecFail) result.Bin2Count++;
                else result.Bin3Count++;
                dies.Add(die);
            }
            if (dies.Count == 0)
                throw new ArgumentException("Map 좌표가 있는 Die 가 없습니다 (Map 밖 " + result.SkipDieCount + "개).");

            dies.Sort(delegate (EtYmsDie a, EtYmsDie b)
            {
                int byY = b.MapY.Value.CompareTo(a.MapY.Value);
                return byY != 0 ? byY : a.MapX.Value.CompareTo(b.MapX.Value);
            });
            result.DieCount = dies.Count;
            return dies;
        }

        /// <summary>TestDataManager.SaveData 가 받는 형식: 열 X / Y / BIN / Parameter(YMS 이름), 값은 문자열 (빈 문자열 = NULL)</summary>
        private static DataTable BuildRawTable(List<EtYmsParameter> parameters, List<EtYmsDie> dies)
        {
            DataTable raw = new DataTable("ETEST_YMS");
            raw.Columns.Add("X", typeof(string));
            raw.Columns.Add("Y", typeof(string));
            raw.Columns.Add("BIN", typeof(string));
            foreach (EtYmsParameter parameter in parameters)
                raw.Columns.Add(parameter.YmsName, typeof(string));

            foreach (EtYmsDie die in dies)
            {
                DataRow row = raw.NewRow();
                row["X"] = die.MapX.Value.ToString(CultureInfo.InvariantCulture);
                row["Y"] = die.MapY.Value.ToString(CultureInfo.InvariantCulture);
                row["BIN"] = EtYmsRule.ToBin(die.Judge, die.RangeFailCount, die.NotTestedFailCount).ToString(CultureInfo.InvariantCulture);
                for (int i = 0; i < parameters.Count; i++)
                    row[3 + i] = EtYmsRule.ToYmsValue(die.Values[i]);
                raw.Rows.Add(row);
            }
            return raw;
        }

        /// <summary>
        /// 기존 함수가 자동 등록한 PARASPEC 행(형식 FLOAT / 설명·단위·Spec 없음)을 보정한다.
        /// X / Y / BIN 은 INT, Parameter 는 FLOAT + 헤더 원문 / 단위 / Spec 하한·상한.
        /// </summary>
        private static void UpdateParameterSpec(string factory, string program, int programRev, List<EtYmsParameter> parameters, string updateUser)
        {
            string rev = programRev.ToString(CultureInfo.InvariantCulture);
            string user = Cut(updateUser, 30);
            string[,] rows = new string[parameters.Count + 3, 10];
            string[][] fixedColumns = new string[][]
            {
                new string[] { "X", "Die X (system map index)" },
                new string[] { "Y", "Die Y (system map index)" },
                new string[] { "BIN", "E-Test Spec judge (1 PASS / 2 SPEC_FAIL / 3 NOT_TESTED)" }
            };
            for (int i = 0; i < fixedColumns.Length; i++)
                SetSpecRow(rows, i, fixedColumns[i][1], string.Empty, null, null, "INT", user, factory, program, rev, fixedColumns[i][0]);

            for (int i = 0; i < parameters.Count; i++)
            {
                EtYmsParameter parameter = parameters[i];
                string description = string.IsNullOrEmpty(parameter.Description) ? parameter.ParameterName : parameter.Description;
                SetSpecRow(rows, 3 + i, Cut(description, MaxParamDescChars), Cut(parameter.Unit, MaxUnitChars), parameter.Lower, parameter.Upper,
                    "FLOAT", user, factory, program, rev, parameter.YmsName);
            }

            DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS oYms = new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS();
            oYms.UpdateExecuteMultiple("UPDATE_YMS_PARASPEC", rows);
        }

        /// <summary>UPDATE_YMS_PARASPEC 파라미터 순서: 설명 / 단위 / LSL / USL / 형식 / 사용자 / Factory / Program / Rev / 이름</summary>
        private static void SetSpecRow(string[,] rows, int index, string description, string unit, double? lower, double? upper,
            string paramType, string user, string factory, string program, string rev, string paramName)
        {
            rows[index, 0] = description ?? string.Empty;
            rows[index, 1] = unit ?? string.Empty;
            rows[index, 2] = EtYmsRule.ToYmsValue(lower);
            rows[index, 3] = EtYmsRule.ToYmsValue(upper);
            rows[index, 4] = paramType;
            rows[index, 5] = user ?? string.Empty;
            rows[index, 6] = factory;
            rows[index, 7] = program;
            rows[index, 8] = rev;
            rows[index, 9] = paramName;
        }

        /// <summary>Program 의 Bin 1 / 2 / 3 을 없을 때만 등록한다. Bin Define 화면에서 고친 값(색 / 이름 등)은 덮어쓰지 않는다.</summary>
        private static void RegisterBins(string program)
        {
            DACrux.TEST.DSL.TQP_BINDESC oBin = new DACrux.TEST.DSL.TQP_BINDESC();
            foreach (int bin in new int[] { EtYmsRule.BinPass, EtYmsRule.BinSpecFail, EtYmsRule.BinNotTested })
            {
                string code = bin.ToString(CultureInfo.InvariantCulture);
                if (Convert.ToInt32(oBin.ExistsBinInfo(program, code), CultureInfo.InvariantCulture) > 0)
                    continue;
                oBin.InsertBinInfo(program, code, EtYmsRule.BinName(bin), string.Empty,
                    bin == EtYmsRule.BinPass ? "Y" : "N",   // HIGH_GEC: Good Bin -> 수율 분자
                    "Y", string.Empty, string.Empty, EtYmsRule.BinColor(bin), EtYmsRule.BinDescription(bin), string.Empty);
            }
        }

        private static string RequiredText(string value, string name, int maxChars)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0)
                throw new ArgumentException(name + " 값이 없습니다.");
            if (text.Length > maxChars)
                throw new ArgumentException(name + " 이(가) YMS 컬럼 길이 " + maxChars + "자를 넘습니다: " + text);
            return text;
        }

        private static string Cut(string value, int maxChars)
        {
            if (value == null)
                return null;
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }

        private static decimal? ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }
    }
}
