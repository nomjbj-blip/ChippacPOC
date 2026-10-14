using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DACrux.SEMDMS.Interface
{
    // =====================================================================================================
    //  E-Test Parameter 이름 분해 / 이름 변환 규칙 (2026-10-14)
    // -----------------------------------------------------------------------------------------------------
    //  목적
    //   E-Test 결과 파일 헤더(예: Resistance_iforce=8.0mA@TSVDC_TR_FH)를 Parameter 로 등록할 때
    //   측정 종류 / 조건 / 단자(Chain) 이름으로 나누고, DB 와 소스에서 그대로 쓸 수 있는 이름으로 바꾼다.
    //   Para Setup 화면 미리보기와 서버 등록이 같은 결과를 내야 하므로 양쪽 모두 이 클래스만 호출한다.
    //   설계: 문서/2026-10-14_ETest_Para_Setup_설계.md 5절 / 5.1절
    //
    //  헤더 형식 (고객 Naming Rule, 2026-10-12 메일)
    //   RES: [Measurement type]_[Input current force]@[Chain type]_[Target region]_[Force leg]
    //   LKG: [Measurement type]_[Connection type]@[Target region 1]_[Force leg 1]:[Target region 2]_[Force leg 2]
    //   -> '@' 앞 = Parameter 종류('=' 뒤는 조건 값), '@' 뒤 = 단자 이름, ':' 가 있으면 단자 2개
    //
    //  이름 변환 (원래 헤더에서 문제 문자 '=' '@' ':' '.' 공백 '#' 만 바꾸고 글자 순서는 그대로 둔다)
    //   Parameter 이름 = EtYmsRule.ToParamName. YMS 컬럼 이름과 같은 값이 되도록 같은 규칙을 쓴다.
    //   단자 / 대표 Chain / GDS Chain 비교 이름 = ToSafeName: 대문자 / '.' -> 'P' / A~Z 0~9 '_' 밖 문자는 '_' /
    //   연속 '_' 하나로 / 앞뒤 '_' 제거. 영문자로 시작해야 한다.
    //   원문 헤더는 CleanHeader(공백 정리 / 끝 '#번호' 제거, 대소문자 유지)로 따로 보관한다.
    //   예) Resistance_iforce=8.0mA@TSVDC_TR_FH -> RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH, 단자 TSVDC_TR_FH
    //
    //  단자를 어느 대표 Chain 으로 묶을지는 여기서 정하지 않는다(사용자가 대표 이름을 입력해 묶는다).
    //  System 만 사용하고 DB 조회가 없다. 걷어낼 때는 이 파일과 csproj Compile 1행을 지운다.
    // =====================================================================================================
    public static class EtParameterNameRule
    {
        /// <summary>단자 / 대표 Chain 이름 최대 길이 (TQP_GDS_ET_ITEM.CHAIN_NAME / TQP_GDS_ET_REP_CHAIN 64 CHAR)</summary>
        public const int MaxChainNameLength = 64;
        /// <summary>헤더 원문 최대 길이 (TQP_GDS_ET_ITEM.HEADER_TEXT 200 CHAR)</summary>
        public const int MaxHeaderLength = 200;
        /// <summary>공정(Test Type) 최대 길이 (TQP_GDS_ET_ITEM.TEST_PROGRAM 50 CHAR)</summary>
        public const int MaxTestProgramLength = 50;

        /// <summary>
        /// Chain type 목록 (2026-10-12 메일 U9 Measurement type). 단자 이름이 이 값으로 시작하면 그 값을 Chain type 으로 본다.
        /// 긴 이름부터 비교한다(PIC_SERP 를 PIC 보다 먼저). 목록에 없으면 첫 '_' 앞을 쓴다.
        /// </summary>
        public static readonly string[] ChainTypes = new string[] { "PIC_EDM", "PIC_SERP", "PIC_COMB", "ASICDC", "HBMDC", "TSVDC", "C4DC" };

        private static readonly string[] ChainTypesByLength = ChainTypes.OrderByDescending(t => t.Length).ToArray();

        private static readonly Regex SpaceRegex = new Regex(@"\s+");
        private static readonly Regex SuffixRegex = new Regex(@"\s*#\S*\s*$");
        private static readonly Regex InvalidCharRegex = new Regex(@"[^A-Z0-9_]");
        private static readonly Regex UnderscoreRegex = new Regex(@"_+");
        private static readonly Regex SafeNameRegex = new Regex(@"^[A-Z][A-Z0-9_]*$");
        // 헤더 분해: {Parameter 종류}[={조건 값}]@{단자 1}[:{단자 2}] (Parsing 설계 10.2절과 같은 정규식)
        private static readonly Regex HeaderRegex = new Regex(@"^(?<para>[^=@]+)(?:=(?<cond>[^@]+))?@(?<c1>[^:]+)(?::(?<c2>.+))?$");

        /// <summary>
        /// 헤더 원문을 정리한다: 공백을 하나로, 앞뒤 공백 제거, 끝 '#번호' 제거 (FT: "...@TSVDC_HBM_C_P_FH #717"). 대소문자는 그대로 둔다.
        /// </summary>
        public static string CleanHeader(string header)
        {
            string clean = SpaceRegex.Replace(header ?? string.Empty, " ").Trim();
            return SuffixRegex.Replace(clean, string.Empty).Trim();
        }

        /// <summary>Parameter 이름(DB / 소스용). YMS 컬럼 이름 규칙(EtYmsRule.ToParamName)과 같은 결과다. 만들 수 없으면 빈 문자열.</summary>
        public static string ToParameterName(string header)
        {
            return EtYmsRule.ToParamName(CleanHeader(header));
        }

        /// <summary>
        /// 단자 / 대표 Chain / GDS Chain 비교용 이름으로 바꾼다.
        /// 대문자 -> '.' 를 'P' 로 -> A~Z / 0~9 / '_' 밖 문자는 '_' -> 연속 '_' 는 하나로 -> 앞뒤 '_' 제거.
        /// 예: "tsvdc top1" -> "TSVDC_TOP1", "TSVDC_TR_FH" -> 그대로. 같은 입력이면 항상 같은 결과이고, 이미 바꾼 이름을 넣어도 같다.
        /// </summary>
        public static string ToSafeName(string value)
        {
            string name = SpaceRegex.Replace(value ?? string.Empty, " ").Trim().ToUpperInvariant().Replace('.', 'P');
            name = InvalidCharRegex.Replace(name, "_");
            name = UnderscoreRegex.Replace(name, "_");
            return name.Trim('_');
        }

        /// <summary>바꾼 이름이 저장 가능한지: 영문 대문자로 시작 / 대문자·숫자·'_' 만 / 길이 이하.</summary>
        public static bool IsSafeName(string name, int maxLength)
        {
            return !string.IsNullOrEmpty(name) && name.Length <= maxLength && SafeNameRegex.IsMatch(name);
        }

        /// <summary>공정(Test Type) 이름: 이름 규칙(ToSafeName)으로 바꾼 값. 예: tsvet -> TSVET</summary>
        public static string ToTestProgram(string testProgram)
        {
            return ToSafeName(testProgram);
        }

        /// <summary>단자 이름 끝의 Force leg (FH / FL). 없으면 빈 문자열.</summary>
        public static string ForceLegOf(string terminalName)
        {
            string name = terminalName ?? string.Empty;
            if (name.EndsWith("_FH", StringComparison.Ordinal))
                return "FH";
            if (name.EndsWith("_FL", StringComparison.Ordinal))
                return "FL";
            return string.Empty;
        }

        /// <summary>단자 이름의 Chain type. 목록(ChainTypes)으로 시작하면 그 값, 아니면 첫 '_' 앞.</summary>
        public static string ChainTypeOf(string terminalName)
        {
            string name = terminalName ?? string.Empty;
            foreach (string type in ChainTypesByLength)
            {
                if (name.StartsWith(type, StringComparison.Ordinal))
                    return type;
            }
            int index = name.IndexOf('_');
            return index > 0 ? name.Substring(0, index) : name;
        }

        /// <summary>
        /// 헤더 1개를 분해하고 이름을 바꾼다.
        /// 처리 흐름: 원문 정리 -> 대문자로 정규식 분해 -> Parameter 종류 / 조건 / 단자 이름 변환 -> Parameter 이름(EtYmsRule) ->
        ///           Force leg / Chain type. 문제가 있으면 Error 에 사유를 넣고(등록 불가), Force leg 가 없으면 Warning 만 넣는다.
        /// </summary>
        public static EtParameterParts Parse(string header)
        {
            EtParameterParts parts = new EtParameterParts();
            parts.HeaderText = CleanHeader(header);
            if (parts.HeaderText.Length == 0)
                return parts.Fail("헤더가 비어 있습니다.");
            if (parts.HeaderText.Length > MaxHeaderLength)
                return parts.Fail("헤더가 " + MaxHeaderLength + "자를 넘습니다.");

            Match m = HeaderRegex.Match(parts.HeaderText.ToUpperInvariant());
            if (!m.Success)
                return parts.Fail("헤더 형식이 아닙니다('@' 뒤에 Chain 이름이 있어야 함, 예: Resistance_iforce=8.0mA@TSVDC_TR_FH).");

            parts.ParaName = ToSafeName(m.Groups["para"].Value);
            if (!IsSafeName(parts.ParaName, 50))
                return parts.Fail("측정 종류 이름을 만들 수 없습니다: " + m.Groups["para"].Value);
            int underscore = parts.ParaName.IndexOf('_');
            parts.MeasType = underscore > 0 ? parts.ParaName.Substring(0, underscore) : parts.ParaName;
            parts.CondValue = m.Groups["cond"].Success ? m.Groups["cond"].Value.Trim() : string.Empty;
            string conditionName = underscore > 0 ? parts.ParaName.Substring(underscore + 1) : string.Empty;
            if (parts.CondValue.Length > 0)
                parts.Condition = conditionName.Length > 0 ? conditionName + "=" + parts.CondValue : parts.CondValue;
            else
                parts.Condition = conditionName;

            parts.Terminal1 = ToSafeName(m.Groups["c1"].Value);
            if (!IsSafeName(parts.Terminal1, MaxChainNameLength))
                return parts.Fail("Chain 이름을 만들 수 없습니다(영문자로 시작, " + MaxChainNameLength + "자 이하): " + m.Groups["c1"].Value);
            parts.Terminal2 = m.Groups["c2"].Success ? ToSafeName(m.Groups["c2"].Value) : string.Empty;
            if (m.Groups["c2"].Success && !IsSafeName(parts.Terminal2, MaxChainNameLength))
                return parts.Fail("두 번째 Chain 이름을 만들 수 없습니다(영문자로 시작, " + MaxChainNameLength + "자 이하): " + m.Groups["c2"].Value);

            parts.ParameterName = EtYmsRule.ToParamName(parts.HeaderText);
            if (!EtYmsRule.IsValidParamName(parts.ParameterName))
                return parts.Fail("Parameter 이름을 만들 수 없습니다: " + parts.HeaderText);

            parts.ForceLeg1 = ForceLegOf(parts.Terminal1);
            parts.ForceLeg2 = ForceLegOf(parts.Terminal2);
            parts.ChainType = ChainTypeOf(parts.Terminal1);
            if (parts.ForceLeg1.Length == 0 || (parts.Terminal2.Length > 0 && parts.ForceLeg2.Length == 0))
                parts.Warning = "Force leg(_FH / _FL) 가 없는 Chain 이름입니다.";
            return parts;
        }

        /// <summary>
        /// 함께 등록할 헤더 목록을 검사한다. 분해 실패 / 같은 헤더 두 번 / 서로 다른 헤더가 같은 Parameter 이름이 되는 것을 문장 목록으로 돌려준다.
        /// 비어 있으면 통과. 헤더 비교는 원문 정리 후 대소문자 무시.
        /// </summary>
        public static string[] FindNameProblems(IEnumerable<string> headers)
        {
            List<string> problems = new List<string>();
            if (headers == null)
                return problems.ToArray();
            Dictionary<string, string> owners = new Dictionary<string, string>(StringComparer.Ordinal);
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string header in headers)
            {
                EtParameterParts parts = Parse(header);
                if (!parts.IsValid)
                {
                    problems.Add(parts.Error + " [" + parts.HeaderText + "]");
                    continue;
                }
                if (!seen.Add(parts.HeaderText))
                {
                    problems.Add("같은 헤더가 두 번 있습니다: " + parts.HeaderText);
                    continue;
                }
                string owner;
                if (owners.TryGetValue(parts.ParameterName, out owner))
                {
                    problems.Add("서로 다른 헤더가 같은 Parameter 이름 " + parts.ParameterName + " 이 됩니다: " + owner + " / " + parts.HeaderText);
                    continue;
                }
                owners.Add(parts.ParameterName, parts.HeaderText);
            }
            return problems.ToArray();
        }
    }

    /// <summary>
    /// 헤더 1개를 분해한 결과 (EtParameterNameRule.Parse). 이름 값은 모두 DB / 소스용으로 바꾼 값이고 HeaderText 만 원문 정리 값이다.
    /// Error 가 비어 있지 않으면 등록할 수 없다.
    /// </summary>
    public sealed class EtParameterParts
    {
        /// <summary>원문 헤더 (공백 정리 / 끝 '#번호' 제거, 대소문자 유지)</summary>
        public string HeaderText = string.Empty;
        /// <summary>Parameter 이름 (ITEM_NAME, 예: RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH)</summary>
        public string ParameterName = string.Empty;
        /// <summary>'=' / '@' 앞 이름 (PARA_NAME, 예: RESISTANCE_IFORCE / LEAKAGE_SERIAL)</summary>
        public string ParaName = string.Empty;
        /// <summary>'=' 뒤 조건 값 (COND_VALUE, 예: 8.0MA). 없으면 빈 문자열</summary>
        public string CondValue = string.Empty;
        /// <summary>Measurement type (MEAS_TYPE, 예: RESISTANCE / LEAKAGE)</summary>
        public string MeasType = string.Empty;
        /// <summary>화면 표시용 조건 (예: IFORCE=8.0MA / SERIAL / PARALLEL)</summary>
        public string Condition = string.Empty;
        /// <summary>단자 1 이름 (CHAIN_NAME)</summary>
        public string Terminal1 = string.Empty;
        /// <summary>단자 2 이름 (CHAIN_NAME2, Leakage_Serial 만)</summary>
        public string Terminal2 = string.Empty;
        /// <summary>단자 1 Force leg (FH / FL / 빈 값)</summary>
        public string ForceLeg1 = string.Empty;
        /// <summary>단자 2 Force leg (FH / FL / 빈 값)</summary>
        public string ForceLeg2 = string.Empty;
        /// <summary>단자 1 의 Chain type (예: TSVDC / C4DC / PIC_SERP)</summary>
        public string ChainType = string.Empty;
        /// <summary>등록할 수 없는 사유. 비어 있으면 정상</summary>
        public string Error = string.Empty;
        /// <summary>등록은 되지만 확인이 필요한 점 (Force leg 없음)</summary>
        public string Warning = string.Empty;

        /// <summary>등록할 수 있는지</summary>
        public bool IsValid
        {
            get { return Error.Length == 0; }
        }

        internal EtParameterParts Fail(string reason)
        {
            Error = reason;
            return this;
        }
    }
}
