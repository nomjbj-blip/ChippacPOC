using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>고객 Spec 표 1행을 읽은 결과. Error 가 비어 있지 않으면 저장하지 않는다.</summary>
    public class EtSpecRow
    {
        /// <summary>붙여넣은 텍스트의 줄 번호(1부터)</summary>
        public int LineNo;
        public string Device;
        /// <summary>공정 = Test Type (대문자)</summary>
        public string TestProgram;
        /// <summary>Parameter = 헤더 전체 (공백 정리 / 끝 '#번호' 제거 / 대문자). 저장 이름은 EtParameterNameRule.ToParameterName(ItemName).</summary>
        public string ItemName;
        /// <summary>헤더 전체 원문 대소문자 그대로(공백 정리 / 끝 '#번호' 제거). Spec 표 붙여넣기가 Parameter 를 등록할 때 HEADER_TEXT 로 쓴다(2026-10-14).</summary>
        public string HeaderText;
        /// <summary>표의 Parameter 열 (Resistance / Leakage, 참고용)</summary>
        public string Kind;
        /// <summary>하한 / 상한 (R 형식 문자열, 없으면 빈 값)</summary>
        public string Lower;
        public string Upper;
        public string Unit;
        public string SpecText;
        public string Error;
    }

    /// <summary>
    /// 고객 Spec 표(Excel 에서 복사한 탭 구분 텍스트)를 읽는다 (2026-10-12).
    /// 기준: 표에 있는 Parameter 만 정의 / 키 = Device + 공정 + Parameter(헤더 전체) / '#번호' 제거 / 정의된 항목의 '-' 만 Fail.
    /// 열: Device / Test Type / Chain name(헤더 전체) / Parameter / Spec / Unit. 첫 줄이 제목이면 제목으로 열을 찾고, 아니면 이 순서로 읽는다.
    /// Spec 문장 해석 (공백 / 대소문자 무시, "Fail" 앞 조건만 본다. 조건은 or 로 여러 개 가능):
    ///   val &gt; N -> Upper N / val &lt; N -> Lower N / abs(val) &gt; N -> Lower -N, Upper N / val = "-" -> 별도 Limit 없음('-' 는 정의 항목이면 항상 Fail)
    ///   그 밖의 문장(&gt;= 등)은 오류 행으로 둔다.
    /// Device 열은 화면에서 고른 Device 로 바꿔 저장한다(GDS Device U9 / E-Test Device OMM 연결은 보류).
    /// </summary>
    public static class EtSpecTable
    {
        private static readonly string[] DefaultColumns = { "DEVICE", "TEST TYPE", "CHAIN NAME", "PARAMETER", "SPEC", "UNIT" };
        private static readonly Regex FailPartRegex = new Regex(@"^if(?<cond>.+?),?fail", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex CondRegex = new Regex(
            @"^(?:(?<abs>abs\(val\))|val)(?<op>>|<)(?<num>[-+]?(?:\d+\.?\d*|\.\d+)(?:e[-+]?\d+)?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex DashRegex = new Regex(@"^val=(?:""-""|'-'|-)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Parameter 이름 정리: EtResultFileParser.NormalizeHeader + 대문자. 같은 공정 안 중복 행을 찾는 키로 쓴다.</summary>
        public static string NormalizeItemName(string header)
        {
            return EtResultFileParser.NormalizeHeader(header).ToUpperInvariant();
        }

        /// <summary>붙여넣은 텍스트를 행 목록으로 읽는다. 빈 줄은 건너뛴다. 같은 공정에 같은 Parameter 가 두 번 나오면 뒤 행을 오류로 둔다.</summary>
        public static List<EtSpecRow> Parse(string text)
        {
            List<EtSpecRow> rows = new List<EtSpecRow>();
            string[] lines = (text ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            int[] map = null;
            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim().Length == 0)
                    continue;
                string[] cells = line.Split('\t');
                if (map == null)
                {
                    map = HeaderMap(cells);
                    if (map != null)
                        continue;
                    map = new int[] { 0, 1, 2, 3, 4, 5 };
                }

                EtSpecRow row = new EtSpecRow { LineNo = i + 1 };
                row.Device = Cell(cells, map[0]);
                row.TestProgram = Cell(cells, map[1]).ToUpperInvariant();
                row.HeaderText = EtResultFileParser.NormalizeHeader(Cell(cells, map[2]));
                row.ItemName = row.HeaderText.ToUpperInvariant();
                row.Kind = Cell(cells, map[3]);
                row.SpecText = Regex.Replace(Cell(cells, map[4]), @"\s+", " ");
                row.Unit = Cell(cells, map[5]);
                row.Error = Validate(row);
                if (row.Error == null && !seen.Add(row.TestProgram + "\t" + row.ItemName))
                    row.Error = "같은 공정에 같은 Parameter 가 이미 있습니다";
                rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// Spec 문장을 Lower / Upper 로 바꾼다. 해석할 수 없으면 false 와 이유를 돌려준다.
        /// 예: "If val > 715, Fail; otherwise, Pass" -> Upper 715 / "If val = "-" or abs(val) > 50, Fail; ..." -> Lower -50, Upper 50
        /// </summary>
        public static bool TryParseSpec(string spec, out string lower, out string upper, out string error)
        {
            lower = string.Empty;
            upper = string.Empty;
            error = null;
            string compact = Regex.Replace(spec ?? string.Empty, @"\s+", string.Empty)
                .Replace('“', '"').Replace('”', '"').Replace('‘', '\'').Replace('’', '\'');
            Match m = FailPartRegex.Match(compact);
            if (!m.Success)
            {
                error = "Spec 문장을 해석할 수 없습니다(\"If ... , Fail\" 형식이 아님)";
                return false;
            }
            double? lo = null, up = null;
            foreach (string part in Regex.Split(m.Groups["cond"].Value, "or", RegexOptions.IgnoreCase))
            {
                if (DashRegex.IsMatch(part))
                    continue;
                Match c = CondRegex.Match(part);
                double n;
                if (!c.Success || !double.TryParse(c.Groups["num"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out n))
                {
                    error = "해석할 수 없는 조건: " + part;
                    return false;
                }
                bool isAbs = c.Groups["abs"].Success;
                bool isGreater = c.Groups["op"].Value == ">";
                if (isAbs && (!isGreater || n < 0))
                {
                    error = "abs(val) 조건은 abs(val) > 0 이상 숫자만 지원합니다: " + part;
                    return false;
                }
                if (isAbs)
                {
                    if (!SetOnce(ref lo, -n) || !SetOnce(ref up, n)) { error = "Lower / Upper 조건이 겹칩니다: " + part; return false; }
                }
                else if (isGreater)
                {
                    if (!SetOnce(ref up, n)) { error = "Upper 조건이 겹칩니다: " + part; return false; }
                }
                else if (!SetOnce(ref lo, n)) { error = "Lower 조건이 겹칩니다: " + part; return false; }
            }
            if (lo == null && up == null)
            {
                error = "Lower / Upper 를 정할 조건이 없습니다";
                return false;
            }
            if (lo != null && up != null && lo.Value > up.Value)
            {
                error = "Lower 가 Upper 보다 큽니다";
                return false;
            }
            lower = lo == null ? string.Empty : lo.Value.ToString("R", CultureInfo.InvariantCulture);
            upper = up == null ? string.Empty : up.Value.ToString("R", CultureInfo.InvariantCulture);
            return true;
        }

        private static bool SetOnce(ref double? target, double value)
        {
            if (target != null)
                return false;
            target = value;
            return true;
        }

        private static string Validate(EtSpecRow row)
        {
            if (row.TestProgram.Length == 0)
                return "Test Type(공정)이 비어 있습니다";
            if (row.TestProgram.Length > 50)
                return "Test Type(공정)이 50자를 넘습니다";
            int at = row.ItemName.IndexOf('@');
            if (at <= 0 || at == row.ItemName.Length - 1)
                return "Chain name 이 측정 헤더 전체(예: Resistance_iforce=8.0mA@TSVDC_TL_FH) 형식이 아닙니다";
            if (row.ItemName.Length > 200)
                return "Chain name 이 200자를 넘습니다";
            if (row.Unit.Length > 10)
                return "Unit 이 10자를 넘습니다";
            if (row.SpecText.Length > 200)
                return "Spec 문장이 200자를 넘습니다";
            string lower, upper, error;
            if (!TryParseSpec(row.SpecText, out lower, out upper, out error))
                return error;
            row.Lower = lower;
            row.Upper = upper;
            return null;
        }

        /// <summary>제목 줄이면 열 위치(Device / Test Type / Chain name / Parameter / Spec / Unit)를 돌려준다. 제목 줄이 아니면 null.</summary>
        private static int[] HeaderMap(string[] cells)
        {
            List<string> names = cells.Select(x => Regex.Replace(x.Trim(), @"\s+", " ").ToUpperInvariant()).ToList();
            if (!names.Contains("TEST TYPE") || !names.Contains("CHAIN NAME"))
                return null;
            int[] map = new int[DefaultColumns.Length];
            for (int k = 0; k < DefaultColumns.Length; k++)
                map[k] = names.IndexOf(DefaultColumns[k]);
            return map;
        }

        private static string Cell(string[] cells, int index)
        {
            return index >= 0 && index < cells.Length ? cells[index].Trim() : string.Empty;
        }
    }
}
