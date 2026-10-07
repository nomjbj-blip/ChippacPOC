using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 결과 파일(xlsx) 1개를 읽은 결과. 배열 열 순서는 서버 iGdsEtResult 계약과 같다.
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9.2절 / 10.2절 (CLAUDE-029)
    /// </summary>
    public class EtResultFile
    {
        public string FilePath;
        public string FileName;
        /// <summary>파일명 앞부분 원문 (예: CAI_OMM_ATV_A0)</summary>
        public string FilePrefix;
        /// <summary>시험 프로그램 (예: TSVET)</summary>
        public string TestProgram;
        /// <summary>시험 일시 yyyyMMddHHmmss</summary>
        public string TestTime;
        public string LotId;
        /// <summary>파일명 Wafer 원문 (예: RH-HS2615-02). DB WAFER_ID는 Lot + 사용자 입력 Slot으로 만든다.</summary>
        public string WaferId;
        /// <summary>파일명에서 얻은 Slot 기본값(1~99). 얻지 못하면 0이고 화면에서 사용자가 입력한다.</summary>
        public int SlotNo;
        public string SheetName;
        /// <summary>측정 항목. 열: 0 ITEM_NAME / 1 PARA_NAME / 2 COND_VALUE / 3 CHAIN_NAME / 4 CHAIN_NAME2 / 5 UNIT / 6 TEST_NO / 7 DISPLAY_ORDER</summary>
        public string[,] Items;
        /// <summary>Die. 열: 0 DUT_NO / 1 DIE_X / 2 DIE_Y / 3 HW_BIN / 4 SW_BIN / 5 PASS_FLAG / 6 FLAG_CODE / 7 TEST_TIME_MS / 8 TESTS_EXECUTED / 9 HEAD_SITE</summary>
        public string[,] Dies;
        /// <summary>측정값 [Die, 항목]. 숫자는 R 형식 문자열, 미측정 / 빈 값은 빈 문자열</summary>
        public string[,] Values;
        /// <summary>측정값 상태 [Die, 항목]: V 숫자 / N 미측정(-) / E 빈 값</summary>
        public char[,] Statuses;
        public int NotTestedCount;
        public int EmptyCount;
        public int FailDieCount;
        /// <summary>저장은 가능하지만 사용자에게 알릴 내용 (예: 내용의 Wafer ID 열 값이 파일명 Lot과 다름)</summary>
        public List<string> Warnings = new List<string>();

        public int DieCount { get { return Dies == null ? 0 : Dies.GetLength(0); } }
        public int ItemCount { get { return Items == null ? 0 : Items.GetLength(0); } }
        public long ValueCount { get { return (long)DieCount * ItemCount; } }
    }

    /// <summary>
    /// E-Test 결과 xlsx를 읽어 서버 저장 형식으로 바꾼다. 규칙을 하나라도 어기면 InvalidDataException으로 중단한다(일부만 저장하지 않음).
    /// 처리 흐름:
    ///  1) 파일명을 뒤에서부터 나눈다: 마지막 = Wafer / 그 앞 = Lot / 14자리 일시 / 시험 프로그램 / 나머지 = 접두어. Wafer가 {Lot}-{숫자}면 Slot 기본값으로 쓴다(최종 Slot은 사용자 입력).
    ///  2) 시트 DUT Summary를 연다. Excel이 파일을 열고 있어도 읽도록 공유 읽기 스트림을 쓴다.
    ///  3) 1행에서 고정 열은 이름으로 찾고, @가 들어간 열은 측정 항목으로 본다. 헤더는 {PARA}[={조건}]@{Chain}[:{Chain2}]로 나눈다.
    ///  4) A열이 Test Number / Unit인 행과 #숫자 Die 행을 찾는다. Upper / Lower Limit 행은 읽지 않는다(Limit은 별도 설정 화면).
    ///  5) 측정값: 숫자 = V / - = N(미측정) / 빈 칸 = E. 그 밖의 문자는 중단한다.
    /// </summary>
    public static class EtResultFileParser
    {
        public const string DataSheetName = "DUT Summary";

        private static readonly Regex HeaderRegex = new Regex(@"^(?<para>[^=@]+)(?:=(?<cond>[^@]+))?@(?<c1>[^:]+)(?::(?<c2>.+))?$", RegexOptions.Compiled);
        private static readonly Regex DutRegex = new Regex(@"^#(?<no>\d+)$", RegexOptions.Compiled);
        private static readonly Regex XyRegex = new Regex(@"^\(\s*(?<x>-?\d+)\s*,\s*(?<y>-?\d+)\s*\)$", RegexOptions.Compiled);
        private static readonly Regex NumberRegex = new Regex(@"-?\d+", RegexOptions.Compiled);
        private static readonly Regex TimeMsRegex = new Regex(@"^(?<ms>\d+)\s*ms$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const int MaxErrorList = 10;

        /// <summary>파일을 읽어 저장 형식으로 바꾼다.</summary>
        public static EtResultFile Parse(string filePath)
        {
            EtResultFile result = new EtResultFile();
            result.FilePath = filePath;
            result.FileName = Path.GetFileName(filePath);
            ParseFileName(result);

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (XLWorkbook workbook = new XLWorkbook(stream))
            {
                IXLWorksheet sheet;
                if (!workbook.TryGetWorksheet(DataSheetName, out sheet))
                    throw new InvalidDataException("시트 '" + DataSheetName + "'가 없습니다.");
                result.SheetName = sheet.Name;
                ParseSheet(sheet, result);
            }
            return result;
        }

        /// <summary>파일명 기준 정보(Lot / Slot / 시험 프로그램 / 일시 / 접두어)를 채운다.</summary>
        public static void ParseFileName(EtResultFile result)
        {
            string name = Path.GetFileNameWithoutExtension(result.FileName);
            string[] tokens = name.Split('_');
            if (tokens.Length < 5)
                throw new InvalidDataException("파일명 형식이 다릅니다(접두어_시험프로그램_일시_Lot_Wafer): " + result.FileName);
            int last = tokens.Length - 1;
            result.WaferId = tokens[last];
            result.LotId = tokens[last - 1];
            result.TestTime = tokens[last - 2];
            result.TestProgram = tokens[last - 3];
            result.FilePrefix = string.Join("_", tokens, 0, last - 3);

            DateTime time;
            if (!DateTime.TryParseExact(result.TestTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                throw new InvalidDataException("파일명의 시험 일시가 14자리(yyyyMMddHHmmss)가 아닙니다: " + result.TestTime);
            if (result.LotId.Length == 0 || result.TestProgram.Length == 0)
                throw new InvalidDataException("파일명에서 Lot / 시험 프로그램을 찾지 못했습니다: " + result.FileName);
            // Slot 번호는 파일에 정식 항목이 없어 화면에서 사용자가 입력한다(2026-10-07 사용자 지시).
            // 파일명 Wafer가 '{Lot}-{숫자}' 형식이면 그 숫자를 입력 기본값으로만 쓰고, 아니면 0(입력 필요)으로 둔다.
            string slotText = result.WaferId.StartsWith(result.LotId + "-", StringComparison.Ordinal)
                ? result.WaferId.Substring(result.LotId.Length + 1) : string.Empty;
            int slot;
            result.SlotNo = slotText.Length > 0 && slotText.All(char.IsDigit) && int.TryParse(slotText, out slot) && slot >= 1 && slot <= 99 ? slot : 0;
        }

        /// <summary>시트 내용을 읽는다.</summary>
        private static void ParseSheet(IXLWorksheet sheet, EtResultFile result)
        {
            IXLColumn lastColumn = sheet.LastColumnUsed();
            IXLRow lastRowUsed = sheet.LastRowUsed();
            if (lastColumn == null || lastRowUsed == null)
                throw new InvalidDataException("시트가 비어 있습니다.");
            int lastCol = lastColumn.ColumnNumber();
            int lastRow = lastRowUsed.RowNumber();

            // 1행: 고정 열(이름) / 측정 열(@ 포함)
            Dictionary<string, int> fixedCols = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            List<int> measureCols = new List<int>();
            for (int c = 2; c <= lastCol; c++)
            {
                string header = Text(sheet.Cell(1, c));
                if (header.IndexOf('@') >= 0) measureCols.Add(c);
                else if (header.Length > 0 && !fixedCols.ContainsKey(header)) fixedCols.Add(header, c);
            }
            if (measureCols.Count == 0)
                throw new InvalidDataException("1행에 측정 항목(@가 들어간 헤더)이 없습니다.");
            int colXy = RequiredColumn(fixedCols, "(X, Y)");
            int colFlag = RequiredColumn(fixedCols, "DUT Flag");

            // A열: Test Number / Unit 행, #숫자 Die 행
            int rowTestNo = 0, rowUnit = 0;
            List<int> dieRows = new List<int>();
            for (int r = 2; r <= lastRow; r++)
            {
                string a = Text(sheet.Cell(r, 1));
                if (string.Equals(a, "Test Number", StringComparison.OrdinalIgnoreCase)) rowTestNo = r;
                else if (string.Equals(a, "Unit", StringComparison.OrdinalIgnoreCase)) rowUnit = r;
                else if (DutRegex.IsMatch(a)) dieRows.Add(r);
            }
            if (rowUnit == 0)
                throw new InvalidDataException("A열에 'Unit' 행이 없습니다.");
            if (dieRows.Count == 0)
                throw new InvalidDataException("A열에 '#번호' Die 행이 없습니다.");

            // 측정 항목
            result.Items = new string[measureCols.Count, 8];
            List<string> headerErrors = new List<string>();
            HashSet<string> headerNames = new HashSet<string>(StringComparer.Ordinal);
            for (int k = 0; k < measureCols.Count; k++)
            {
                int c = measureCols[k];
                string header = Text(sheet.Cell(1, c));
                Match m = HeaderRegex.Match(header);
                if (!m.Success || !headerNames.Add(header))
                {
                    headerErrors.Add(ColumnName(c) + "열 " + header + (m.Success ? " (중복)" : ""));
                    continue;
                }
                result.Items[k, 0] = header;
                result.Items[k, 1] = m.Groups["para"].Value.Trim();
                result.Items[k, 2] = m.Groups["cond"].Value.Trim();
                result.Items[k, 3] = m.Groups["c1"].Value.Trim();
                result.Items[k, 4] = m.Groups["c2"].Value.Trim();
                result.Items[k, 5] = Text(sheet.Cell(rowUnit, c));
                result.Items[k, 6] = rowTestNo > 0 ? IntegerText(sheet.Cell(rowTestNo, c)) : string.Empty;
                result.Items[k, 7] = (k + 1).ToString(CultureInfo.InvariantCulture);
            }
            ThrowIfAny("측정 항목 헤더를 나눌 수 없습니다", headerErrors);

            // Die
            int colHw = OptionalColumn(fixedCols, "Hardware Bin");
            int colSw = OptionalColumn(fixedCols, "Software Bin");
            int colTime = OptionalColumn(fixedCols, "Test Time");
            int colExecuted = OptionalColumn(fixedCols, "Tests Executed");
            int colSite = OptionalColumn(fixedCols, "Test Head - Site");
            int colWafer = OptionalColumn(fixedCols, "Wafer ID");
            result.Dies = new string[dieRows.Count, 10];
            List<string> dieErrors = new List<string>();
            HashSet<string> duts = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> xys = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> otherWafers = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < dieRows.Count; i++)
            {
                int r = dieRows[i];
                string dut = DutRegex.Match(Text(sheet.Cell(r, 1))).Groups["no"].Value.TrimStart('0');
                if (dut.Length == 0) dut = "0";
                Match xy = XyRegex.Match(Text(sheet.Cell(r, colXy)));
                string flag = Text(sheet.Cell(r, colFlag));
                string pass = flag.StartsWith("Pass", StringComparison.OrdinalIgnoreCase) ? "P"
                    : flag.StartsWith("Fail", StringComparison.OrdinalIgnoreCase) ? "F" : string.Empty;
                if (!xy.Success) dieErrors.Add(r + "행 (X, Y) 형식: " + Text(sheet.Cell(r, colXy)));
                else if (!xys.Add(xy.Groups["x"].Value + "," + xy.Groups["y"].Value)) dieErrors.Add(r + "행 같은 좌표 " + Text(sheet.Cell(r, colXy)));
                if (!duts.Add(dut)) dieErrors.Add(r + "행 같은 DUT #" + dut);
                if (pass.Length == 0) dieErrors.Add(r + "행 DUT Flag: " + flag);
                if (colWafer > 0)
                {
                    string waferCell = Text(sheet.Cell(r, colWafer));
                    if (waferCell.Length > 0 && !string.Equals(waferCell, result.LotId, StringComparison.Ordinal)) otherWafers.Add(waferCell);
                }
                if (pass == "F") result.FailDieCount++;

                result.Dies[i, 0] = dut;
                result.Dies[i, 1] = xy.Success ? xy.Groups["x"].Value : string.Empty;
                result.Dies[i, 2] = xy.Success ? xy.Groups["y"].Value : string.Empty;
                result.Dies[i, 3] = colHw > 0 ? FirstNumber(Text(sheet.Cell(r, colHw))) : string.Empty;
                result.Dies[i, 4] = colSw > 0 ? FirstNumber(Text(sheet.Cell(r, colSw))) : string.Empty;
                result.Dies[i, 5] = pass;
                int dash = flag.LastIndexOf('-');
                result.Dies[i, 6] = dash >= 0 ? flag.Substring(dash + 1).Trim() : string.Empty;
                string timeText = colTime > 0 ? Text(sheet.Cell(r, colTime)) : string.Empty;
                Match ms = TimeMsRegex.Match(timeText);
                result.Dies[i, 7] = ms.Success ? ms.Groups["ms"].Value : string.Empty;
                if (timeText.Length > 0 && !ms.Success && result.Warnings.Count < MaxErrorList)
                    result.Warnings.Add(r + "행 Test Time 형식이 달라 저장하지 않음: " + timeText);
                result.Dies[i, 8] = colExecuted > 0 ? IntegerText(sheet.Cell(r, colExecuted)) : string.Empty;
                result.Dies[i, 9] = colSite > 0 ? Text(sheet.Cell(r, colSite)) : string.Empty;
            }
            ThrowIfAny("Die 행 형식이 다릅니다", dieErrors);
            if (otherWafers.Count > 0)
                result.Warnings.Add("내용의 Wafer ID 열 값이 파일명 Lot(" + result.LotId + ")과 다릅니다: " + string.Join(", ", otherWafers.Take(5).ToArray()));

            // 측정값
            result.Values = new string[dieRows.Count, measureCols.Count];
            result.Statuses = new char[dieRows.Count, measureCols.Count];
            List<string> valueErrors = new List<string>();
            for (int i = 0; i < dieRows.Count; i++)
            {
                for (int k = 0; k < measureCols.Count; k++)
                {
                    IXLCell cell = sheet.Cell(dieRows[i], measureCols[k]);
                    double number;
                    if (TryNumber(cell, out number))
                    {
                        result.Values[i, k] = number.ToString("R", CultureInfo.InvariantCulture);
                        result.Statuses[i, k] = 'V';
                        continue;
                    }
                    string text = Text(cell);
                    result.Values[i, k] = string.Empty;
                    if (text == "-") { result.Statuses[i, k] = 'N'; result.NotTestedCount++; }
                    else if (text.Length == 0) { result.Statuses[i, k] = 'E'; result.EmptyCount++; }
                    else valueErrors.Add(ColumnName(measureCols[k]) + dieRows[i] + " = " + text);
                }
            }
            ThrowIfAny("숫자 / - / 빈 칸이 아닌 측정값이 있습니다", valueErrors);
        }

        /// <summary>
        /// 측정값 저장 배치를 만든다. 열: 0 DUT_NO / 1 ET_ITEM_SEQ / 2 MEAS_VALUE / 3 VALUE_STATUS (CreateEtValueBatch 형식)
        /// itemSeqByName: MergeEtItem 반환의 ITEM_NAME -> ET_ITEM_SEQ
        /// </summary>
        public static List<string[,]> BuildValueBatches(EtResultFile file, IDictionary<string, string> itemSeqByName, int batchRows)
        {
            string[] itemSeqs = new string[file.ItemCount];
            for (int k = 0; k < file.ItemCount; k++)
            {
                if (!itemSeqByName.TryGetValue(file.Items[k, 0], out itemSeqs[k]))
                    throw new InvalidOperationException("서버에서 측정 항목 번호를 받지 못했습니다: " + file.Items[k, 0]);
            }
            List<string[,]> batches = new List<string[,]>();
            long total = file.ValueCount;
            long index = 0;
            string[,] batch = null;
            int row = 0;
            for (int i = 0; i < file.DieCount; i++)
            {
                for (int k = 0; k < file.ItemCount; k++, index++)
                {
                    if (batch == null)
                    {
                        batch = new string[(int)Math.Min(batchRows, total - index), 4];
                        row = 0;
                    }
                    batch[row, 0] = file.Dies[i, 0];
                    batch[row, 1] = itemSeqs[k];
                    batch[row, 2] = file.Values[i, k];
                    batch[row, 3] = file.Statuses[i, k].ToString();
                    row++;
                    if (row == batch.GetLength(0))
                    {
                        batches.Add(batch);
                        batch = null;
                    }
                }
            }
            return batches;
        }

        #region [Cell]

        /// <summary>셀 값을 문자열로 읽는다. 숫자는 R 형식(왕복 가능한 최소 자릿수).</summary>
        private static string Text(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty()) return string.Empty;
            object value = cell.Value;
            if (value is double) return ((double)value).ToString("R", CultureInfo.InvariantCulture);
            return Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
        }

        /// <summary>숫자 셀(또는 숫자 모양 문자열)이면 double로 돌려준다.</summary>
        private static bool TryNumber(IXLCell cell, out double number)
        {
            number = 0;
            if (cell == null || cell.IsEmpty()) return false;
            object value = cell.Value;
            if (value is double) { number = (double)value; return !double.IsNaN(number) && !double.IsInfinity(number); }
            string text = Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
            return text.Length > 0 && text != "-" && double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out number)
                && !double.IsNaN(number) && !double.IsInfinity(number);
        }

        /// <summary>정수 셀이면 정수 문자열, 아니면 빈 문자열.</summary>
        private static string IntegerText(IXLCell cell)
        {
            double number;
            if (!TryNumber(cell, out number) || number != Math.Floor(number)) return string.Empty;
            return ((long)number).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>문자열 안의 첫 정수 (예: "Bin 5001" -> "5001"). 없으면 빈 문자열.</summary>
        private static string FirstNumber(string text)
        {
            Match m = NumberRegex.Match(text ?? string.Empty);
            return m.Success ? m.Value : string.Empty;
        }

        private static int RequiredColumn(Dictionary<string, int> cols, string name)
        {
            int c;
            if (!cols.TryGetValue(name, out c))
                throw new InvalidDataException("1행에 '" + name + "' 열이 없습니다.");
            return c;
        }

        private static int OptionalColumn(Dictionary<string, int> cols, string name)
        {
            int c;
            return cols.TryGetValue(name, out c) ? c : 0;
        }

        /// <summary>열 번호 -> Excel 열 이름 (1 -> A, 28 -> AB)</summary>
        private static string ColumnName(int col)
        {
            string name = string.Empty;
            while (col > 0) { int m = (col - 1) % 26; name = (char)('A' + m) + name; col = (col - m) / 26; }
            return name;
        }

        private static void ThrowIfAny(string title, List<string> errors)
        {
            if (errors.Count == 0) return;
            throw new InvalidDataException(title + " (" + errors.Count + "건)\r\n" + string.Join("\r\n", errors.Take(MaxErrorList).ToArray())
                + (errors.Count > MaxErrorList ? "\r\n..." : string.Empty));
        }

        #endregion
    }
}
