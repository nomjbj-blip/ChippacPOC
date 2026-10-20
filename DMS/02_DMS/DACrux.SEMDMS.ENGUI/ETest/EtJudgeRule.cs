using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 값 판정 규칙 (2026-10-14). 서버 SQL(TQP_GDS_ET_VALUE.xml UPDATE_ET_VALUE_JUDGE)과 같은 규칙이고 화면 미리보기에만 쓴다(저장하지 않음).
    /// 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9.2절
    ///  - Spec 없음 / Enable N -> 판정 안 함('\0')
    ///  - '-'(N) -> F, 빈 칸(E) -> 판정 안 함
    ///  - 숫자: Lower 가 있고 값 &lt; Lower 또는 Upper 가 있고 값 &gt; Upper 이면 F (경계값 Pass), 아니면 P
    /// </summary>
    public static class EtJudgeRule
    {
        public const char Pass = 'P';
        public const char Fail = 'F';
        public const char None = '\0';

        /// <param name="hasSpec">Spec 이 있음</param>
        /// <param name="enable">Spec Enable = Y</param>
        /// <param name="lower">하한(R 문자열, 없으면 빈 값)</param>
        /// <param name="upper">상한(R 문자열, 없으면 빈 값)</param>
        /// <param name="status">값 상태 V / N / E</param>
        /// <param name="value">값(R 문자열)</param>
        public static char Judge(bool hasSpec, bool enable, string lower, string upper, char status, string value)
        {
            if (!hasSpec || !enable) return None;
            if (status == 'N') return Fail;
            if (status != 'V') return None;
            double v = double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
            double limit;
            if (!string.IsNullOrEmpty(lower) && double.TryParse(lower, NumberStyles.Float, CultureInfo.InvariantCulture, out limit) && v < limit) return Fail;
            if (!string.IsNullOrEmpty(upper) && double.TryParse(upper, NumberStyles.Float, CultureInfo.InvariantCulture, out limit) && v > limit) return Fail;
            return Pass;
        }

        /// <summary>Fail 사유 표시: '-' / 하한 미만 / 상한 초과</summary>
        public static string FailReason(char status, string value, string lower, string upper)
        {
            if (status == 'N') return "'-' (미측정)";
            double v = double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
            double limit;
            if (!string.IsNullOrEmpty(lower) && double.TryParse(lower, NumberStyles.Float, CultureInfo.InvariantCulture, out limit) && v < limit) return "Lower 미만";
            return "Upper 초과";
        }
    }

    /// <summary>
    /// 결과 파일 1개의 업로드 계획 (2026-10-14 Upload 재설계 10절): 파일 헤더 ↔ 등록 Parameter 대조 + 미리보기 판정.
    /// 등록 Parameter 목록은 GdsEtResult.GetEtParameterList(Factory, 파일 Device, 공정) 결과를 그대로 받는다
    /// (열 ET_ITEM_SEQ / ITEM_NAME / HEADER_TEXT / USE_FLAG / HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / CHAIN_NAME · 2 / GDS_CHAIN_NAME(Chain 1 의 GDS Setup Chain, 사용자 입력)).
    /// 대조 규칙:
    ///  - 헤더 -> EtParameterNameRule 저장 이름 ↔ 등록 Parameter 저장 이름. 등록된 헤더 원문과 대소문자만 다르면 같은 것.
    ///  - 원문이 다른데 저장 이름이 같으면 이름 충돌(건너뜀 + 경고), 파일 안에서 같은 저장 이름이 두 번이면 뒤 것 건너뜀 + 경고.
    ///  - 사용 안 함(N) Parameter 는 저장하지 않는다. 등록되지 않은 헤더는 건너뛴다(수만 센다).
    ///  - 파일 Unit 행 단위와 등록 단위가 다르면(둘 다 있을 때, 대소문자 구분) 건너뜀 + 경고. 값은 환산하지 않으므로 다른 단위 값을 저장 / 판정하지 않는다 (2026-10-16).
    /// </summary>
    public sealed class EtUploadPlan
    {
        /// <summary>저장할 Parameter 1개 (파일 항목 위치 + 등록 정보 + Spec)</summary>
        public sealed class Param
        {
            public int Index;
            public string ItemSeq;
            public string Name;
            public string Header;
            public bool HasSpec;
            public bool Enable;
            public string Lower;
            public string Upper;
            public string Chain;
            public string Gds;
            /// <summary>등록 Parameter 표시 순서 (없으면 long.MaxValue). 2026-10-15 결과 표를 조회 화면과 같은 순서로 보이려고 추가</summary>
            public long DisplayOrder;
        }

        public readonly List<Param> Matched = new List<Param>();
        /// <summary>등록(사용 Y)됐는데 파일에 없는 Parameter 의 헤더 원문</summary>
        public readonly List<string> Missing = new List<string>();
        /// <summary>이름 충돌 / 파일 안 중복으로 건너뛴 헤더 설명</summary>
        public readonly List<string> Conflicts = new List<string>();
        /// <summary>파일 단위가 등록 단위와 달라 건너뛴 Parameter 설명 ("저장 이름: 파일 nA / 등록 uA") (2026-10-16)</summary>
        public readonly List<string> UnitMismatches = new List<string>();
        /// <summary>등록되지 않아 건너뛴 헤더 수</summary>
        public int Skipped;
        /// <summary>사용 안 함(N) Parameter 라 건너뛴 헤더 수</summary>
        public int Unused;
        /// <summary>그 Device + 공정의 등록 Parameter(사용 Y) 수 (BeginEtFile 19번 값)</summary>
        public int RegisteredCount;
        /// <summary>Spec 이 있고 Enable 인 저장 Parameter 수</summary>
        public int JudgedCount;
        /// <summary>미리보기 판정 [Die, Matched 순서] (P / F / '\0')</summary>
        public char[,] Judge;
        public int[] DieFail;
        public int[] DieNotTested;
        public int SpecFailDie;
        public int NotTestedCount;
        public int EmptyCount;

        /// <summary>저장할 값 수 = Die x 저장 Parameter</summary>
        public long ValueCount(EtResultFile file)
        {
            return (long)file.DieCount * Matched.Count;
        }

        public static EtUploadPlan Build(EtResultFile file, DataTable registered)
        {
            EtUploadPlan plan = new EtUploadPlan();
            Dictionary<string, DataRow> byName = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            foreach (DataRow dr in registered.Rows)
            {
                byName[dr["ITEM_NAME"].ToString()] = dr;
                if (dr["USE_FLAG"].ToString() == "Y") plan.RegisteredCount++;
            }
            HashSet<string> used = new HashSet<string>(StringComparer.Ordinal);
            for (int k = 0; k < file.ItemCount; k++)
            {
                EtParameterParts parts = EtParameterNameRule.Parse(file.Items[k, 0]);
                DataRow dr;
                if (!parts.IsValid || !byName.TryGetValue(parts.ParameterName, out dr))
                {
                    plan.Skipped++;
                    continue;
                }
                if (dr["USE_FLAG"].ToString() != "Y")
                {
                    plan.Unused++;
                    continue;
                }
                if (!string.Equals(dr["HEADER_TEXT"].ToString(), parts.HeaderText, StringComparison.OrdinalIgnoreCase))
                {
                    plan.Conflicts.Add("이름 충돌 " + parts.ParameterName + ": 파일 " + parts.HeaderText + " / 등록 " + dr["HEADER_TEXT"]);
                    continue;
                }
                if (!used.Add(parts.ParameterName))
                {
                    plan.Conflicts.Add("파일 안 중복 " + parts.ParameterName + ": " + parts.HeaderText + " (" + (k + 1) + "열)");
                    continue;
                }
                // 단위 비교는 Para 등록과 같은 규칙(대소문자 구분). 파일에 있는 Parameter 라 누락으로는 세지 않는다 (2026-10-16)
                string fileUnit = (file.Items[k, 5] ?? string.Empty).Trim();
                string regUnit = registered.Columns.Contains("UNIT") ? dr["UNIT"].ToString().Trim() : string.Empty;
                if (fileUnit.Length > 0 && regUnit.Length > 0 && !string.Equals(fileUnit, regUnit, StringComparison.Ordinal))
                {
                    plan.UnitMismatches.Add(parts.ParameterName + ": 파일 " + fileUnit + " / 등록 " + regUnit);
                    continue;
                }
                bool hasSpec = dr["HAS_SPEC"].ToString() == "Y";
                Param p = new Param
                {
                    Index = k,
                    ItemSeq = Convert.ToInt64(dr["ET_ITEM_SEQ"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                    Name = parts.ParameterName,
                    Header = dr["HEADER_TEXT"].ToString(),
                    HasSpec = hasSpec,
                    Enable = hasSpec && dr["ENABLE_FLAG"].ToString() == "Y",
                    Lower = Number(dr["LOWER_LIMIT"]),
                    Upper = Number(dr["UPPER_LIMIT"]),
                    Chain = Pair(dr["CHAIN_NAME"], dr["CHAIN_NAME2"]),
                    Gds = dr["GDS_CHAIN_NAME"].ToString(),
                    DisplayOrder = !registered.Columns.Contains("DISPLAY_ORDER") || dr["DISPLAY_ORDER"] == DBNull.Value
                        ? long.MaxValue : Convert.ToInt64(dr["DISPLAY_ORDER"], CultureInfo.InvariantCulture)
                };
                if (p.Enable) plan.JudgedCount++;
                plan.Matched.Add(p);
            }
            foreach (DataRow dr in registered.Rows)
                if (dr["USE_FLAG"].ToString() == "Y" && !used.Contains(dr["ITEM_NAME"].ToString()))
                    plan.Missing.Add(dr["HEADER_TEXT"].ToString());

            int dies = file.DieCount;
            plan.Judge = new char[dies, plan.Matched.Count];
            plan.DieFail = new int[dies];
            plan.DieNotTested = new int[dies];
            for (int i = 0; i < dies; i++)
            {
                for (int m = 0; m < plan.Matched.Count; m++)
                {
                    Param p = plan.Matched[m];
                    char status = file.Statuses[i, p.Index];
                    if (status == 'N') { plan.DieNotTested[i]++; plan.NotTestedCount++; }
                    else if (status == 'E') plan.EmptyCount++;
                    char judge = EtJudgeRule.Judge(p.HasSpec, p.Enable, p.Lower, p.Upper, status, file.Values[i, p.Index]);
                    plan.Judge[i, m] = judge;
                    if (judge == EtJudgeRule.Fail) plan.DieFail[i]++;
                }
                if (plan.DieFail[i] > 0) plan.SpecFailDie++;
            }
            return plan;
        }

        /// <summary>저장할 Parameter 값만 서버 배치 형식으로 나눈다. 열: 0 DUT_NO / 1 ET_ITEM_SEQ / 2 MEAS_VALUE(빈 값 = NULL) / 3 VALUE_STATUS</summary>
        public List<string[,]> BuildValueBatches(EtResultFile file, int batchRows)
        {
            List<string[,]> batches = new List<string[,]>();
            long total = ValueCount(file);
            if (total == 0) return batches;
            int rowsInBatch = 0;
            string[,] current = null;
            long remaining = total;
            for (int i = 0; i < file.DieCount; i++)
            {
                for (int m = 0; m < Matched.Count; m++)
                {
                    if (current == null)
                    {
                        current = new string[(int)Math.Min(batchRows, remaining), 4];
                        rowsInBatch = 0;
                    }
                    Param p = Matched[m];
                    char status = file.Statuses[i, p.Index];
                    current[rowsInBatch, 0] = file.Dies[i, 0];
                    current[rowsInBatch, 1] = p.ItemSeq;
                    current[rowsInBatch, 2] = status == 'V' ? file.Values[i, p.Index] : string.Empty;
                    current[rowsInBatch, 3] = status.ToString();
                    rowsInBatch++;
                    remaining--;
                    if (rowsInBatch == current.GetLength(0))
                    {
                        batches.Add(current);
                        current = null;
                    }
                }
            }
            return batches;
        }

        private static string Number(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("R", CultureInfo.InvariantCulture);
        }

        private static string Pair(object first, object second)
        {
            string a = Convert.ToString(first, CultureInfo.InvariantCulture);
            string b = Convert.ToString(second, CultureInfo.InvariantCulture);
            if (b.Length == 0) return a;
            return (a.Length == 0 ? "-" : a) + " : " + b;
        }
    }
}
