using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 결과 1개 파일의 화면 표시 데이터 (2026-10-15). 업로드 화면(저장 전 분석 결과)과 조회 화면(DB 저장 결과)이 같은 모양으로 쓰는 중립 모델이다.
    /// 설계: 문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md 3절
    ///  - Info   : 항목 / 값 목록 (두 화면이 같은 항목을 같은 순서로). 2026-10-15 파일 기준 정보까지만 보인다(저장 Parameter 아래 개수 / 상태 / 안내 행 삭제).
    ///             업로드 화면은 Device / 공정 / Lot / Lot-Wafer No 를 더블클릭으로 고칠 수 있다(EditableInfo).
    ///  - Dies   : 1단계 Die (DUT / 좌표 / Map 좌표 / 판정 / Fail 수 / '-' 수 / DUT Flag 참고)
    ///  - Params : 2단계에 보이는 정의된 Parameter (등록 Parameter 사용 Y 중 저장 대상)
    ///  - Cells  : [Die, Param] 값 / 상태 / 판정
    /// 판정은 여기서 계산하지 않는다. 업로드는 EtUploadPlan 미리보기 판정(EtJudgeRule), 조회는 DB 에 저장된 판정을 그대로 옮긴다.
    /// 만드는 곳: FromUpload(업로드 화면) / FromDb(조회 화면). 그리는 곳: EtResultDataControl.
    /// </summary>
    public sealed class EtResultData
    {
        /// <summary>1단계 Die 1개</summary>
        public sealed class Die
        {
            public string Dut;
            public string X;
            public string Y;
            /// <summary>시스템 Map 좌표 "x,y", Map 밖이면 "밖", Map 이 없으면 빈 값</summary>
            public string Map;
            /// <summary>Pass / Fail / Spec 없음 / 빈 값(판정 전)</summary>
            public string Judge;
            public int FailCount;
            public int NotTestedCount;
            /// <summary>파일 DUT Flag (참고, 판정 아님)</summary>
            public string Flag;
        }

        /// <summary>2단계 Parameter 1개 (정의된 Parameter)</summary>
        public sealed class Param
        {
            /// <summary>저장 이름 (ITEM_NAME)</summary>
            public string Name;
            /// <summary>헤더 원문</summary>
            public string Header;
            public string Chain;
            public string Gds;
            public string Unit;
            public string Lower;
            public string Upper;
        }

        /// <summary>[Die, Param] 값 1개</summary>
        public struct Cell
        {
            /// <summary>표시 값 (숫자 R 형식, '-', 빈 칸이면 빈 값)</summary>
            public string Value;
            /// <summary>V(숫자) / N('-') / E(빈 칸) / '\0'(값 없음)</summary>
            public char Status;
            /// <summary>P / F / '\0'(판정 안 함)</summary>
            public char Judge;
        }

        public readonly List<KeyValuePair<string, string>> Info = new List<KeyValuePair<string, string>>();
        public readonly List<Die> Dies = new List<Die>();
        public readonly List<Param> Params = new List<Param>();
        public Cell[,] Cells = new Cell[0, 0];
        /// <summary>정보 표 제목 (예: "업로드 미리보기 - 준비", "저장된 결과 - 파일 번호 12")</summary>
        public string Title = string.Empty;

        /// <summary>정보 항목 이름 (업로드 화면에서 고칠 수 있는 칸)</summary>
        public const string InfoDevice = "Device";
        public const string InfoProgram = "공정";
        public const string InfoLot = "Lot";
        public const string InfoWaferNo = "Lot-Wafer No";
        /// <summary>정보 표에서 더블클릭으로 고칠 수 있는 항목 이름 (조회 화면은 비어 있음)</summary>
        public readonly HashSet<string> EditableInfo = new HashSet<string>(StringComparer.Ordinal);

        private void AddInfo(string name, string value)
        {
            Info.Add(new KeyValuePair<string, string>(name, value ?? string.Empty));
        }

        #region [업로드: 저장 전 분석 결과]

        /// <summary>
        /// 업로드 화면의 연 파일(저장 전 분석 결과)을 표시 데이터로 바꾼다. 2단계 Parameter = EtUploadPlan.Matched(저장할 등록 Parameter).
        /// </summary>
        /// <param name="mapText">Die 별 시스템 Map 좌표 "x,y" / "밖" (null 이면 Map 확인 전)</param>
        /// <param name="savedSeq">저장한 파일 번호 (저장 전이면 빈 값)</param>
        /// <param name="editable">Device / 공정 / Lot / Lot-Wafer No 를 고칠 수 있게 할지 (저장 전)</param>
        public static EtResultData FromUpload(EtResultFile f, EtUploadPlan plan, string program, int slot, string waferId,
            string[] mapText, string status, string savedSeq, bool editable)
        {
            EtResultData data = new EtResultData();
            bool saved = !string.IsNullOrEmpty(savedSeq);
            data.Title = saved ? "업로드 결과 - 저장됨 (파일 번호 " + savedSeq + ")" : "업로드 미리보기 (저장 전) - " + status;
            if (f == null) return data;

            data.AddInfo("파일명", f.FileName);
            data.AddInfo(InfoDevice, f.FileDevice);
            data.AddInfo(InfoProgram, program);
            data.AddInfo(InfoLot, f.LotId);
            data.AddInfo(InfoWaferNo, f.WaferId);
            data.AddInfo("Slot / WAFER_ID", (slot == 0 ? "?" : slot.ToString(CultureInfo.InvariantCulture)) + " / " + waferId);
            data.AddInfo("Test Date", FormatTestTime(f.TestTime));
            data.AddInfo("Customer / Wafer Type / Rev", f.Customer + " / " + f.WaferType + " / " + f.WaferRev);
            data.AddInfo("Die", f.DieCount.ToString(CultureInfo.InvariantCulture));
            data.AddInfo("헤더", f.ItemCount.ToString("N0", CultureInfo.InvariantCulture));
            data.AddInfo("등록 (사용 Y)", plan == null ? string.Empty : plan.RegisteredCount.ToString(CultureInfo.InvariantCulture));
            if (editable)
                foreach (string name in new[] { InfoDevice, InfoProgram, InfoLot, InfoWaferNo })
                    data.EditableInfo.Add(name);

            for (int i = 0; i < f.DieCount; i++)
            {
                data.Dies.Add(new Die
                {
                    Dut = f.Dies[i, 0],
                    X = f.Dies[i, 1],
                    Y = f.Dies[i, 2],
                    Map = mapText == null ? string.Empty : mapText[i],
                    Judge = plan == null ? string.Empty : plan.JudgedCount == 0 ? "Spec 없음" : plan.DieFail[i] > 0 ? "Fail" : "Pass",
                    FailCount = plan == null ? 0 : plan.DieFail[i],
                    NotTestedCount = plan == null ? 0 : plan.DieNotTested[i],
                    Flag = DutFlag(f.Dies[i, 5], f.Dies[i, 6])
                });
            }
            if (plan == null) return data;

            // Parameter 순서: 조회 화면과 같게 표시 순서 -> 저장 이름 (Matched 는 파일 열 순서)
            int[] order = Enumerable.Range(0, plan.Matched.Count)
                .OrderBy(m => plan.Matched[m].DisplayOrder).ThenBy(m => plan.Matched[m].Name, StringComparer.Ordinal).ToArray();
            foreach (int m in order)
            {
                EtUploadPlan.Param p = plan.Matched[m];
                data.Params.Add(new Param
                {
                    Name = p.Name,
                    Header = p.Header,
                    Chain = p.Chain,
                    Gds = p.Gds,
                    Unit = f.Items[p.Index, 5],
                    Lower = p.HasSpec ? p.Lower : string.Empty,
                    Upper = p.HasSpec ? p.Upper + (p.Enable ? string.Empty : " (Off)") : string.Empty
                });
            }
            data.Cells = new Cell[f.DieCount, order.Length];
            for (int i = 0; i < f.DieCount; i++)
                for (int j = 0; j < order.Length; j++)
                {
                    EtUploadPlan.Param p = plan.Matched[order[j]];
                    char st = f.Statuses[i, p.Index];
                    data.Cells[i, j] = new Cell
                    {
                        Status = st,
                        Value = st == 'V' ? f.Values[i, p.Index] : st == 'N' ? "-" : string.Empty,
                        Judge = plan.Judge[i, order[j]]
                    };
                }
            return data;
        }

        #endregion

        #region [조회: DB 저장 결과]

        /// <summary>
        /// 조회 화면: 저장된 파일 1건을 표시 데이터로 바꾼다. 2단계 Parameter = 이 파일에 저장된 Parameter(저장 당시 등록 Parameter).
        /// </summary>
        /// <param name="file">GetEtFile 1행</param>
        /// <param name="dies">GetEtDieList</param>
        /// <param name="values">GetEtValueList</param>
        /// <param name="para">GetEtParameterList (지금 Spec / GDS Chain). null 이면 Lower / Upper / GDS Chain 은 빈 값</param>
        public static EtResultData FromDb(DataRow file, DataTable dies, DataTable values, DataTable para)
        {
            EtResultData data = new EtResultData();
            data.Title = "저장된 결과 - 파일 번호 " + file["SRC_FILE_SEQ"] + (file["LOAD_STATUS"].ToString() == "READY" ? string.Empty : " [" + file["LOAD_STATUS"] + "]");
            Dictionary<string, DataRow> spec = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            if (para != null)
                foreach (DataRow dr in para.Rows)
                    spec[dr["ITEM_NAME"].ToString()] = dr;

            int dieCount = dies.Rows.Count;

            // 파라미터 순서: 표시 순서 -> 저장 이름 (조회 창과 같음)
            List<DataRow> firstRows = values.Rows.Cast<DataRow>()
                .GroupBy(v => v["ET_ITEM_SEQ"].ToString()).Select(g => g.First())
                .OrderBy(v => v["DISPLAY_ORDER"] == DBNull.Value ? long.MaxValue : Convert.ToInt64(v["DISPLAY_ORDER"], CultureInfo.InvariantCulture))
                .ThenBy(v => v["ITEM_NAME"].ToString(), StringComparer.Ordinal).ToList();
            Dictionary<string, int> paramIndex = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (DataRow v in firstRows)
            {
                DataRow s;
                bool hasSpec = spec.TryGetValue(v["ITEM_NAME"].ToString(), out s) && s["HAS_SPEC"].ToString() == "Y";
                bool enable = hasSpec && s["ENABLE_FLAG"].ToString() == "Y";
                paramIndex[v["ET_ITEM_SEQ"].ToString()] = data.Params.Count;
                data.Params.Add(new Param
                {
                    Name = v["ITEM_NAME"].ToString(),
                    Header = v["HEADER_TEXT"].ToString(),
                    Chain = Pair(v["CHAIN_NAME"], v["CHAIN_NAME2"]),
                    Gds = s == null ? string.Empty : s["GDS_CHAIN_NAME"].ToString(),
                    Unit = v["UNIT"].ToString(),
                    Lower = hasSpec ? Number(s["LOWER_LIMIT"]) : string.Empty,
                    Upper = hasSpec ? Number(s["UPPER_LIMIT"]) + (enable ? string.Empty : " (Off)") : string.Empty
                });
            }

            data.AddInfo("파일명", file["FILE_NAME"].ToString());
            data.AddInfo(InfoDevice, file["DEVICE_ID"].ToString());
            data.AddInfo(InfoProgram, file["TEST_PROGRAM"].ToString());
            data.AddInfo(InfoLot, file["LOT_ID"].ToString());
            data.AddInfo(InfoWaferNo, file["LOT_WAFER_NO"].ToString());
            data.AddInfo("Slot / WAFER_ID", file["SLOT_NO"] + " / " + file["WAFER_ID"]);
            data.AddInfo("Test Date", FormatTestTime(file["TEST_TIME"].ToString()));
            data.AddInfo("Customer / Wafer Type / Rev", file["CUSTOMER"] + " / " + file["WAFER_TYPE"] + " / " + file["WAFER_REV"]);
            data.AddInfo("Die", dieCount.ToString(CultureInfo.InvariantCulture));
            data.AddInfo("헤더", Count(file["HEADER_COUNT"]));
            data.AddInfo("등록 (사용 Y)", Count(file["DEFINED_COUNT"]) + " (저장 당시)");

            Dictionary<string, int> dieIndex = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (DataRow d in dies.Rows)
            {
                dieIndex[d["DUT_NO"].ToString()] = data.Dies.Count;
                string judge = d["JUDGE_FLAG"].ToString();
                data.Dies.Add(new Die
                {
                    Dut = d["DUT_NO"].ToString(),
                    X = d["DIE_X"].ToString(),
                    Y = d["DIE_Y"].ToString(),
                    Map = d["MAP_X"] == DBNull.Value ? string.Empty : d["MAP_X"] + "," + d["MAP_Y"],
                    Judge = judge == "F" ? "Fail" : judge == "P" ? "Pass" : string.Empty,
                    FailCount = Int(d["FAIL_COUNT"]),
                    NotTestedCount = Int(d["NOT_TESTED_COUNT"]),
                    Flag = DutFlag(d["PASS_FLAG"].ToString(), d["FLAG_CODE"].ToString())
                });
            }

            data.Cells = new Cell[data.Dies.Count, data.Params.Count];
            foreach (DataRow v in values.Rows)
            {
                int i, m;
                if (!dieIndex.TryGetValue(v["DUT_NO"].ToString(), out i) || !paramIndex.TryGetValue(v["ET_ITEM_SEQ"].ToString(), out m)) continue;
                string st = v["VALUE_STATUS"].ToString();
                string judge = v["JUDGE_FLAG"].ToString();
                data.Cells[i, m] = new Cell
                {
                    Status = st.Length == 0 ? '\0' : st[0],
                    Value = st == "V" ? Number(v["MEAS_VALUE"]) : st == "N" ? "-" : string.Empty,
                    Judge = judge.Length == 0 ? '\0' : judge[0]
                };
            }
            return data;
        }

        #endregion

        #region [공통]

        private static string DutFlag(string passFlag, string code)
        {
            return (passFlag == "F" ? "Fail" : "Pass") + (string.IsNullOrEmpty(code) ? string.Empty : " (" + code + ")");
        }

        private static string Pair(object first, object second)
        {
            string a = Convert.ToString(first, CultureInfo.InvariantCulture);
            string b = Convert.ToString(second, CultureInfo.InvariantCulture);
            if (b.Length == 0) return a;
            return (a.Length == 0 ? "-" : a) + " : " + b;
        }

        private static string Number(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("R", CultureInfo.InvariantCulture);
        }

        private static int Int(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private static string Count(object value)
        {
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString("N0", CultureInfo.InvariantCulture);
        }

        internal static string FormatTestTime(string time)
        {
            DateTime value;
            return DateTime.TryParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
                ? value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : time;
        }

        #endregion
    }
}
