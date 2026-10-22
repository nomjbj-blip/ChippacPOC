using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DACrux.SEMDMS.Interface
{
    // =====================================================================================================
    //  E-Test -> YMS 적재 규칙 (2026-10-14)
    // -----------------------------------------------------------------------------------------------------
    //  목적
    //   E-Test 결과를 YMS 테이블(TQP_LOT / TQP_WAFER / TD_<PROGRAM> / TQP_PARASPEC / TQP_BINDESC / 요약)에 넣을 때 쓰는
    //   이름 / Bin / 값 / 수율 규칙을 한 곳에 둔다. 업로드 화면 미리보기와 서버 적재가 같은 결과를 내야 하므로
    //   양쪽 모두 이 클래스만 호출한다. 설계: 문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md 14절.
    //
    //  YMS 쪽 길이 제약 (로컬 DB 컬럼 길이로 확인)
    //   TQP_PARASPEC.PARAM_NAME / TABLE_NAME 50자 -> Parameter 컬럼 이름 50자, Program 45자 (TD_ + Program + 분할 접미어 1자)
    //   TQP_LOT.LOT_ID 20자 / DEVICE 30자 / DEVICE_ALIAS 30자 / TEST_AREA 30자, TQP_WAFER.WAFER_ID 30자
    //   TD 테이블을 만드는 기존 코드(TD_TABLE.CreateTDTable)는 컬럼 이름을 검사하지 않고 DDL 에 그대로 넣는다.
    //   그래서 E-Test Parameter 이름을 여기서 Oracle 식별자로 바꿔 넘긴다.
    //
    //  Parameter 컬럼 이름 (ToParamName)
    //   1. Parameter 이름 정리: 공백을 하나로, 끝 '#번호' 제거, 대문자 (업로드 파서 / Spec 의 ITEM_NAME 과 같은 규칙)
    //   2. '.' -> 'P' (8.0MA -> 8P0MA, 기존 Fab2 PCM 파서와 같은 방식)
    //   3. A~Z / 0~9 / '_' 밖 문자 -> '_', 연속 '_' 는 하나로, 앞뒤 '_' 제거
    //   4. 숫자로 시작하거나 예약 이름(X / Y / BIN / WAFER_SEQ / DIE_NUM / DIEPROBE_CNT)이면 앞에 'P_'
    //   5. 50자를 넘으면 측정 종류를 고객 약어로 줄이고(LEAKAGE -> LKG, RESISTANCE -> RES ...),
    //      그래도 넘으면 41자 + '_' + 해시 8자로 자른다
    //   예) Resistance_iforce=8.0mA@C4DC_28_37_FH -> RESISTANCE_IFORCE_8P0MA_C4DC_28_37_FH
    //       Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL (54자) -> LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL (50자)
    //   같은 Program 안의 서로 다른 Parameter 가 같은 이름이 되면 FindNameProblems 가 알려 준다 (적재 전에 막는다).
    //
    //  YMS 컬럼 이름 (ToYmsName, 2026-10-21)
    //   고객 DB(Oracle 11.2.0.4)는 테이블 / 컬럼 이름이 30바이트까지라 TD 컬럼에는 ToParamName 결과(최대 50자)를 그대로 쓸 수 없다.
    //   Parameter 이름(ITEM_NAME)이 30자 이하면 그대로 쓰고, 넘으면 등록 때 분해한 정보로 짧게 조립한다.
    //     측정 약어(RES / CONN / LKG / CONT) + 조건(8P0MA / PAR / SER) + Chain 1 + Chain 2 에서 Chain 1 과 앞이 같은 단어를 뺀 나머지
    //   그래도 30자를 넘으면 앞 21자 + '_' + 해시 8자. 등록 때 한 번 만들어 TQP_GDS_ET_ITEM.YMS_PARAM_NAME 에 저장하고 다시 계산하지 않는다.
    //   예) RESISTANCE_IFORCE_8P0MA_TSVDC_ASIC_HBM_N_FH -> RES_8P0MA_TSVDC_ASIC_HBM_N_FH
    //       LEAKAGE_SERIAL_TSVDC_ASIC1_N_FH_TSVDC_ASIC1_P_FL -> LKG_SER_TSVDC_ASIC1_N_FH_P_FL
    //   TD 테이블 이름("TD_" + Program + 분할 접미어)도 30자라 Program 은 26자 이하다.
    //   설계: 문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md 5절
    //
    //  BIN (ToBin, 2026-10-14 변경)
    //   YMS BIN = 결과 파일의 Hardware Bin. E-Test Spec 판정(POC Para / Limit)과는 따로 본다.
    //   Software Bin(예: 2001)은 BIN 으로 쓸 수 없다: YMS 요약(TD_TABLE.xml GET_RAW_DATA)이 BIN < 100 인 Die 만 세고
    //   TQP_WAFER_SUM / TQP_LOT_SUM 의 Bin 열은 BIN0 ~ BIN150 뿐이다. 그래서 Software Bin 은 Bin 정의 이름 / 설명에 적는다.
    //   양품(HIGH_GEC = Y) = 그 Hardware Bin 의 Die 가 모두 파일 DUT 결과 Pass. YMS 수율은 양품 Bin 의 Die 로 계산된다.
    //   예전 규칙(1 PASS / 2 SPEC_FAIL / 3 NOT_TESTED)으로 만든 Bin 정의는 LegacyJudgeBins 로 알아보고 다시 반영할 때 지운다.
    //
    //  걷어내기 / 재활용
    //   이 파일 하나에 모든 규칙이 있다 (System 만 사용, DB 조회 없음). 걷어낼 때는 이 파일과 csproj Compile 1행을 지운다.
    // =====================================================================================================
    public static class EtYmsRule
    {
        /// <summary>Parameter 이름(ITEM_NAME) 최대 길이 (TQP_GDS_ET_ITEM CHECK / TQP_PARASPEC.PARAM_NAME VARCHAR2(50))</summary>
        public const int MaxParamNameLength = 50;
        /// <summary>YMS TD 컬럼 이름 최대 길이 (Oracle 11g 식별자 30바이트, 2026-10-21)</summary>
        public const int MaxYmsNameLength = 30;
        /// <summary>Program 최대 길이 (TD 테이블 이름 30 = "TD_" + Program + 분할 접미어 1자, 2026-10-21 45 -> 26)</summary>
        public const int MaxProgramLength = 26;
        /// <summary>TQP_LOT.LOT_ID 길이</summary>
        public const int MaxLotIdLength = 20;
        /// <summary>TQP_WAFER.WAFER_ID 길이</summary>
        public const int MaxWaferIdLength = 30;
        /// <summary>TQP_LOT.DEVICE / DEVICE_ALIAS 길이 (Device / Map ID)</summary>
        public const int MaxDeviceLength = 30;
        /// <summary>TQP_LOT.TEST_AREA 길이</summary>
        public const int MaxTestAreaLength = 30;

        /// <summary>YMS BIN 최대값 (요약 쿼리 GET_RAW_DATA 가 BIN &lt; 100 만 센다)</summary>
        public const int MaxBin = 99;
        /// <summary>TQP_BINDESC.BIN_NAME 길이</summary>
        public const int MaxBinNameLength = 30;
        /// <summary>TQP_BINDESC.DESCRIPTION 길이</summary>
        public const int MaxBinDescriptionLength = 100;
        /// <summary>양품 Bin 색</summary>
        public const string GoodBinColor = "#00C000";

        /// <summary>
        /// 불량 Bin 색. 인덱스 = Hardware Bin 번호의 끝자리 (2 빨강 / 3 파랑 / 4 자주 / 5 금색 ...).
        /// 초록(양품) / 하늘색(측정 안 한 Die) / 회색(Wafer 바탕)은 쓰지 않는다.
        /// </summary>
        private static readonly string[] FailBinColors = new string[]
        {
            "#800000", "#FF8C00", "#FF0000", "#0000FF", "#C000C0", "#FFD700", "#8B4513", "#FF69B4", "#808000", "#4B0082"
        };

        /// <summary>
        /// 예전 규칙(E-Test Spec 판정 1 / 2 / 3)으로 만든 Bin 정의 {BIN, BIN_NAME, DESCRIPTION}.
        /// 세 값이 모두 같을 때만 지운다 (YMS Program Setup 에서 고친 정의는 남긴다).
        /// </summary>
        public static readonly string[][] LegacyJudgeBins = new string[][]
        {
            new string[] { "1", "PASS", "E-Test Spec Pass" },
            new string[] { "2", "SPEC_FAIL", "E-Test Spec Fail (out of limit)" },
            new string[] { "3", "NOT_TESTED", "E-Test Fail by not tested value only" }
        };

        /// <summary>TD 테이블에서 Parameter 이름으로 쓸 수 없는 이름 (기존 고정 컬럼과 Die 컬럼)</summary>
        public static readonly string[] ReservedNames = new string[] { "X", "Y", "BIN", "WAFER_SEQ", "DIE_NUM", "DIEPROBE_CNT" };

        private const int HashLength = 8;

        private static readonly Regex SpaceRegex = new Regex(@"\s+");
        private static readonly Regex SuffixRegex = new Regex(@"\s*#\S*\s*$");
        private static readonly Regex InvalidCharRegex = new Regex(@"[^A-Z0-9_]");
        private static readonly Regex UnderscoreRegex = new Regex(@"_+");
        private static readonly Regex ParamNameRegex = new Regex(@"^[A-Z][A-Z0-9_]*$");
        private static readonly Regex ProgramRegex = new Regex(@"^[A-Z0-9_]+$");

        // 50자를 넘을 때만 쓰는 약어. 앞 단계부터 적용하고, 줄어들면 멈춘다.
        // 1단계는 측정 종류 (고객 Naming Rule 의 RES / LKG 표기), 2단계는 조건 이름.
        private static readonly string[][] ShortWordsStep1 = new string[][]
        {
            new string[] { "LEAKAGE", "LKG" },
            new string[] { "RESISTANCE", "RES" },
            new string[] { "CONNECTIVITY", "CONN" },
            new string[] { "CONTINUITY", "CONT" }
        };
        private static readonly string[][] ShortWordsStep2 = new string[][]
        {
            new string[] { "PARALLEL", "PAR" },
            new string[] { "SERIAL", "SER" },
            new string[] { "IFORCE", "IF" }
        };

        /// <summary>
        /// Parameter 이름을 정리한다 (공백 하나로 / 끝 '#번호' 제거 / 대문자).
        /// 업로드 파서 NormalizeHeader + Spec 의 대문자 규칙과 같다. 이미 정리된 이름을 넣어도 결과가 같다.
        /// </summary>
        public static string NormalizeParameterName(string parameterName)
        {
            string clean = SpaceRegex.Replace(parameterName ?? string.Empty, " ").Trim();
            return SuffixRegex.Replace(clean, string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Parameter 이름을 YMS TD 컬럼 이름으로 바꾼다 (머리 주석 1 ~ 5).
        /// 같은 입력이면 항상 같은 결과를 낸다. 이름을 만들 수 없으면(문자가 하나도 남지 않음) 빈 문자열을 돌려준다.
        /// </summary>
        public static string ToParamName(string parameterName)
        {
            return ToParamName(parameterName, MaxParamNameLength);
        }

        /// <summary>ToParamName 과 같은 규칙을 주어진 길이 한도로 적용한다 (50 = ITEM_NAME, 30 = 분해 정보가 없는 Parameter 의 YMS 이름).</summary>
        private static string ToParamName(string parameterName, int maxLength)
        {
            string normalized = NormalizeParameterName(parameterName);
            string name = ToIdentifier(normalized.Replace('.', 'P'));
            if (name.Length == 0)
                return string.Empty;

            if (char.IsDigit(name[0]) || IsReserved(name))
                name = "P_" + name;

            if (name.Length > maxLength)
                name = ShortenWords(name, ShortWordsStep1, maxLength);
            if (name.Length > maxLength)
                name = ShortenWords(name, ShortWordsStep2, maxLength);
            if (name.Length > maxLength)
                name = CutWithHash(name, normalized, maxLength);
            return name;
        }

        /// <summary>
        /// YMS TD 컬럼 이름 (머리 주석 'YMS 컬럼 이름'). 등록 때 분해한 Parameter 정보로 만든다.
        /// itemName(ITEM_NAME) 이 30자 이하면 그대로 돌려준다. Chain 이름이 없으면(헤더 형식이 아님) ToParamName 규칙을 30자로 적용한다.
        /// 같은 입력이면 항상 같은 결과를 낸다. 이름을 만들 수 없으면 빈 문자열.
        /// </summary>
        /// <param name="itemName">Parameter 이름 (ITEM_NAME)</param>
        /// <param name="measType">측정 종류 (MEAS_TYPE, 예: RESISTANCE)</param>
        /// <param name="paraName">'=' / '@' 앞 이름 (PARA_NAME, 예: LEAKAGE_SERIAL)</param>
        /// <param name="condValue">조건 값 (COND_VALUE, 예: 8.0MA). 없으면 빈 값</param>
        /// <param name="chainName">Chain 1 (CHAIN_NAME)</param>
        /// <param name="chainName2">Chain 2 (CHAIN_NAME2, Leakage_Serial 만). 없으면 빈 값</param>
        public static string ToYmsName(string itemName, string measType, string paraName, string condValue, string chainName, string chainName2)
        {
            string item = NormalizeParameterName(itemName);
            if (item.Length > 0 && item.Length <= MaxYmsNameLength && ParamNameRegex.IsMatch(item) && !IsReserved(item))
                return item;

            string chain1 = ToIdentifier(NormalizeParameterName(chainName).Replace('.', 'P'));
            if (chain1.Length == 0)
                return ToParamName(item, MaxYmsNameLength);

            string para = ToIdentifier(NormalizeParameterName(paraName).Replace('.', 'P'));
            string meas = ToIdentifier(NormalizeParameterName(measType));
            if (meas.Length == 0)
            {
                int first = para.IndexOf('_');
                meas = first > 0 ? para.Substring(0, first) : para;
            }

            List<string> parts = new List<string>();
            parts.Add(ShortWord(meas, ShortWordsStep1));
            parts.Add(ConditionCode(meas, para, condValue));
            parts.Add(chain1);
            parts.Add(Chain2Tail(chain1, ToIdentifier(NormalizeParameterName(chainName2).Replace('.', 'P'))));

            string name = UnderscoreRegex.Replace(string.Join("_", parts.FindAll(p => p.Length > 0).ToArray()), "_").Trim('_');
            if (name.Length == 0)
                return string.Empty;
            if (char.IsDigit(name[0]) || IsReserved(name))
                name = "P_" + name;
            if (name.Length > MaxYmsNameLength)
                name = CutWithHash(name, item, MaxYmsNameLength);
            return name;
        }

        /// <summary>등록 때 분해한 결과로 YMS 컬럼 이름을 만든다 (ToYmsName 과 같은 규칙).</summary>
        public static string ToYmsName(EtParameterParts parts)
        {
            if (parts == null)
                return string.Empty;
            return ToYmsName(parts.ParameterName, parts.MeasType, parts.ParaName, parts.CondValue, parts.Chain1, parts.Chain2);
        }

        /// <summary>YMS TD 컬럼 이름으로 쓸 수 있는지 (영문 대문자로 시작 / 대문자·숫자·'_' / 30자 이하 / 예약 이름 아님)</summary>
        public static bool IsValidYmsName(string name)
        {
            return !string.IsNullOrEmpty(name)
                && name.Length <= MaxYmsNameLength
                && ParamNameRegex.IsMatch(name)
                && !IsReserved(name);
        }

        /// <summary>YMS Program 이름으로 쓸 수 있는지 (대문자·숫자·'_' / 26자 이하). TD 테이블 이름은 "TD_" 로 시작하므로 숫자로 시작해도 된다.</summary>
        public static bool IsValidProgramName(string program)
        {
            return !string.IsNullOrEmpty(program)
                && program.Length <= MaxProgramLength
                && ProgramRegex.IsMatch(program);
        }

        /// <summary>
        /// 같은 Device + 공정에 들어갈 {Parameter 이름, YMS 이름} 목록을 검사한다.
        /// YMS 이름으로 쓸 수 없는 것 / 서로 다른 Parameter 가 같은 YMS 이름인 것을 문장 목록으로 돌려준다. 비어 있으면 통과.
        /// 같은 Parameter 가 두 번 있는 것은 문제로 보지 않는다.
        /// </summary>
        public static string[] FindYmsNameProblems(IList<string> itemNames, IList<string> ymsNames)
        {
            List<string> problems = new List<string>();
            if (itemNames == null || ymsNames == null)
                return problems.ToArray();
            if (itemNames.Count != ymsNames.Count)
                throw new ArgumentException("Parameter 이름과 YMS 이름 개수가 다릅니다.");

            Dictionary<string, string> owners = new Dictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < itemNames.Count; i++)
            {
                string item = itemNames[i] ?? string.Empty;
                string yms = ymsNames[i] ?? string.Empty;
                if (!IsValidYmsName(yms))
                {
                    problems.Add("YMS 이름으로 쓸 수 없음(영문 대문자로 시작, 대문자·숫자·'_', " + MaxYmsNameLength + "자 이하): " + yms + " [" + item + "]");
                    continue;
                }
                string owner;
                if (owners.TryGetValue(yms, out owner))
                {
                    if (!string.Equals(owner, item, StringComparison.Ordinal))
                        problems.Add("같은 YMS 이름 " + yms + ": " + owner + " / " + item);
                    continue;
                }
                owners.Add(yms, item);
            }
            return problems.ToArray();
        }

        /// <summary>YMS Parameter 컬럼 이름으로 쓸 수 있는지 (영문 대문자로 시작 / 대문자·숫자·'_' / 50자 이하 / 예약 이름 아님)</summary>
        public static bool IsValidParamName(string name)
        {
            return !string.IsNullOrEmpty(name)
                && name.Length <= MaxParamNameLength
                && ParamNameRegex.IsMatch(name)
                && !IsReserved(name);
        }

        /// <summary>
        /// 한 Program 에 같이 들어갈 Parameter 이름 목록을 검사한다.
        /// 이름을 만들 수 없는 것 / 서로 다른 Parameter 가 같은 컬럼 이름이 되는 것을 문장 목록으로 돌려준다. 비어 있으면 통과.
        /// 같은 Parameter 가 목록에 두 번 있는 것은 문제로 보지 않는다 (호출하는 쪽이 중복을 걸러 넘긴다).
        /// </summary>
        public static string[] FindNameProblems(IEnumerable<string> parameterNames)
        {
            List<string> problems = new List<string>();
            Dictionary<string, string> owners = new Dictionary<string, string>(StringComparer.Ordinal);
            if (parameterNames == null)
                return problems.ToArray();

            foreach (string parameterName in parameterNames)
            {
                string normalized = NormalizeParameterName(parameterName);
                string name = ToParamName(normalized);
                if (!IsValidParamName(name))
                {
                    problems.Add("YMS 컬럼 이름을 만들 수 없음: " + normalized);
                    continue;
                }

                string owner;
                if (owners.TryGetValue(name, out owner))
                {
                    if (!string.Equals(owner, normalized, StringComparison.Ordinal))
                        problems.Add("같은 YMS 컬럼 이름 " + name + ": " + owner + " / " + normalized);
                    continue;
                }
                owners.Add(name, normalized);
            }
            return problems.ToArray();
        }

        /// <summary>YMS Program 이름 = Device + "_" + 공정 (대문자, A~Z / 0~9 / '_' 밖 문자는 '_'). 예: OMM + TSVET -> OMM_TSVET</summary>
        public static string ToProgramName(string deviceId, string testProgram)
        {
            string device = ToIdentifier((deviceId ?? string.Empty).Trim().ToUpperInvariant());
            string test = ToIdentifier((testProgram ?? string.Empty).Trim().ToUpperInvariant());
            if (device.Length == 0 || test.Length == 0)
                return string.Empty;
            return device + "_" + test;
        }

        /// <summary>YMS TESTAREA = 공정 (파일명 Test Type) 대문자. 예: TSVET / COWET / FT</summary>
        public static string ToTestArea(string testProgram)
        {
            return (testProgram ?? string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Die 의 YMS BIN = 결과 파일 Hardware Bin. 값이 없거나 0 ~ MaxBin 밖이면 YMS 요약에 들어가지 않으므로 오류로 알린다.
        /// </summary>
        public static int ToBin(int? hardwareBin)
        {
            if (!hardwareBin.HasValue)
                throw new ArgumentException("Hardware Bin 이 없습니다.");
            if (hardwareBin.Value < 0 || hardwareBin.Value > MaxBin)
                throw new ArgumentException("YMS 에 넣을 수 있는 Hardware Bin 은 0 ~ " + MaxBin + " 입니다: " + hardwareBin.Value.ToString(CultureInfo.InvariantCulture));
            return hardwareBin.Value;
        }

        /// <summary>
        /// Bin 이름 (TQP_BINDESC.BIN_NAME, 30자 이하). 예: HW2 SW2001 / Software Bin 이 여러 개면 HW2 SW2001,2002 / 없으면 HW2
        /// </summary>
        public static string BinName(int hardwareBin, IEnumerable<int> softwareBins)
        {
            string soft = JoinBins(softwareBins);
            string name = "HW" + hardwareBin.ToString(CultureInfo.InvariantCulture) + (soft.Length > 0 ? " SW" + soft : string.Empty);
            return name.Length <= MaxBinNameLength ? name : name.Substring(0, MaxBinNameLength);
        }

        /// <summary>
        /// Bin 설명 (TQP_BINDESC.DESCRIPTION, 100자 이하). 예: E-Test file Hardware Bin 2 / Software Bin 2001 / Fail
        /// mixed = 같은 Hardware Bin 에 파일 결과 Pass 와 Fail 이 섞여 있음 (양품으로 보지 않는다).
        /// </summary>
        public static string BinDescription(int hardwareBin, IEnumerable<int> softwareBins, bool good, bool mixed)
        {
            string soft = JoinBins(softwareBins);
            string text = "E-Test file Hardware Bin " + hardwareBin.ToString(CultureInfo.InvariantCulture)
                + (soft.Length > 0 ? " / Software Bin " + soft : string.Empty)
                + (good ? " / Pass" : mixed ? " / Pass and Fail mixed" : " / Fail");
            return text.Length <= MaxBinDescriptionLength ? text : text.Substring(0, MaxBinDescriptionLength);
        }

        /// <summary>Bin 색 (TQP_BINDESC.COLOR, HTML 형식. Map 화면이 ColorTranslator.FromHtml 로 읽는다). 양품 = 초록, 불량 = Hardware Bin 번호로 고른 색</summary>
        public static string BinColor(int hardwareBin, bool good)
        {
            if (good)
                return GoodBinColor;
            return FailBinColors[Math.Abs(hardwareBin) % FailBinColors.Length];
        }

        /// <summary>예전 규칙으로 만든 Bin 정의인지 (BIN / 이름 / 설명이 모두 LegacyJudgeBins 와 같음)</summary>
        public static bool IsLegacyJudgeBin(string bin, string binName, string description)
        {
            foreach (string[] legacy in LegacyJudgeBins)
            {
                if (string.Equals(legacy[0], (bin ?? string.Empty).Trim(), StringComparison.Ordinal)
                    && string.Equals(legacy[1], (binName ?? string.Empty).Trim(), StringComparison.Ordinal)
                    && string.Equals(legacy[2], (description ?? string.Empty).Trim(), StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 측정값을 YMS 적재용 문자열로 바꾼다. 값이 없으면('-' / 빈 칸) 빈 문자열 -> TD 에 NULL 로 저장된다.
        /// 기존 YMS 적재(TestDataManager.GetDecimalValue)는 숫자가 아닌 문자열을 decimal.MinValue / MaxValue 로 넣어
        /// Map 색 구간 / 통계를 깨뜨리므로 '-' 를 그대로 넘기지 않는다.
        /// </summary>
        public static string ToYmsValue(double? value)
        {
            if (!value.HasValue || double.IsNaN(value.Value) || double.IsInfinity(value.Value))
                return string.Empty;
            return value.Value.ToString("R", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// YMS 요약(TestCommon.SummaryData)과 같은 규칙의 수율(%). 화면 미리보기용.
        /// Map NETDIE 와 측정 Die 수가 1.5배 넘게 다르면 측정 Die 수로, 아니면 NETDIE 로 나눈다. NETDIE 가 없으면 측정 Die 수로 나눈다.
        /// </summary>
        public static double ExpectedYield(int goodDieCount, int dieCount, int? netDie)
        {
            if (dieCount <= 0)
                return 0d;
            double total = dieCount;
            double divisor = total;
            if (netDie.HasValue)
            {
                double net = netDie.Value;
                divisor = (net / total > 1.5 || total / net > 1.5) ? total : net;
            }
            return Math.Round(100d * goodDieCount / divisor, 2);
        }

        /// <summary>Software Bin 목록을 작은 번호부터 중복 없이 ',' 로 잇는다</summary>
        private static string JoinBins(IEnumerable<int> bins)
        {
            if (bins == null)
                return string.Empty;
            List<int> list = new List<int>(new SortedSet<int>(bins));
            return string.Join(",", list.ConvertAll(b => b.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        private static bool IsReserved(string name)
        {
            return Array.IndexOf(ReservedNames, name) >= 0;
        }

        private static string ToIdentifier(string value)
        {
            string name = InvalidCharRegex.Replace(value, "_");
            name = UnderscoreRegex.Replace(name, "_");
            return name.Trim('_');
        }

        private static string ShortenWords(string name, string[][] words, int maxLength)
        {
            string[] tokens = name.Split('_');
            foreach (string[] pair in words)
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i] == pair[0])
                        tokens[i] = pair[1];
                }
                string shortened = string.Join("_", tokens);
                if (shortened.Length <= maxLength)
                    return shortened;
            }
            return string.Join("_", tokens);
        }

        /// <summary>앞부분 + '_' + 원래 이름의 해시 8자로 자른다. 해시는 실행 환경과 무관하게 같은 값이 나오도록 직접 계산한다.</summary>
        private static string CutWithHash(string name, string hashSource, int maxLength)
        {
            string head = name.Substring(0, maxLength - HashLength - 1).TrimEnd('_');
            return head + "_" + StableHash(hashSource);
        }

        /// <summary>단어 하나를 약어 목록으로 바꾼다 (목록에 없으면 그대로).</summary>
        private static string ShortWord(string word, string[][] words)
        {
            foreach (string[] pair in words)
            {
                if (word == pair[0])
                    return pair[1];
            }
            return word;
        }

        /// <summary>
        /// YMS 이름의 조건 부분. 조건 값이 있으면 그 값('.' -> 'P', 예: 8.0MA -> 8P0MA)만 쓴다(IFORCE 같은 조건 이름은 넣지 않음).
        /// 없으면 PARA_NAME 에서 측정 종류 뒤 단어를 약어로(PARALLEL -> PAR / SERIAL -> SER / IFORCE -> IF).
        /// </summary>
        private static string ConditionCode(string meas, string para, string condValue)
        {
            string cond = ToIdentifier(NormalizeParameterName(condValue).Replace('.', 'P'));
            if (cond.Length > 0)
                return cond;

            string rest = para;
            if (meas.Length > 0 && rest.StartsWith(meas + "_", StringComparison.Ordinal))
                rest = rest.Substring(meas.Length + 1);
            else if (rest == meas)
                rest = string.Empty;
            if (rest.Length == 0)
                return string.Empty;

            string[] tokens = rest.Split('_');
            for (int i = 0; i < tokens.Length; i++)
                tokens[i] = ShortWord(tokens[i], ShortWordsStep2);
            return string.Join("_", tokens);
        }

        /// <summary>
        /// Chain 2 에서 Chain 1 과 앞에서부터 같은 단어를 뺀 나머지. 마지막 단어는 항상 남긴다. Chain 2 가 없으면 빈 문자열.
        /// 예: TSVDC_ASIC1_N_FH / TSVDC_ASIC1_P_FL -> P_FL
        /// </summary>
        private static string Chain2Tail(string chain1, string chain2)
        {
            if (string.IsNullOrEmpty(chain2))
                return string.Empty;
            string[] a = chain1.Split('_');
            string[] b = chain2.Split('_');
            int i = 0;
            while (i < a.Length && i < b.Length - 1 && a[i] == b[i])
                i++;
            return string.Join("_", b, i, b.Length - i);
        }

        /// <summary>FNV-1a 32bit 해시 8자리 (string.GetHashCode 는 32 / 64bit 에서 값이 달라 쓰지 않는다)</summary>
        private static string StableHash(string value)
        {
            uint hash = 2166136261;
            unchecked
            {
                foreach (char c in value)
                {
                    hash ^= c;
                    hash *= 16777619;
                }
            }
            return hash.ToString("X8", CultureInfo.InvariantCulture);
        }
    }
}
