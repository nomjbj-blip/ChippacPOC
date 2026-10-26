using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SEMDMS.DSL;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// E-Test Bin 설정 서버 처리 (2026-10-22).
    /// 설계: 문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md
    ///  - E-Test Device + 공정의 YMS Program(Device_공정, EtYmsRule.ToProgramName) Bin 정의(TQP_BINDESC)에서 이름 / 양품 / 색 / 설명만 고친다.
    ///    Map 기호 / 표시 / Limit 수는 MERGE_ET_BIN 이 건드리지 않는다. 업로드의 Bin 자동 등록(EtYmsPublisher.RegisterBins)과 같은 테이블이다.
    ///  - 정의 읽기 / 쓰기 / 삭제는 TQP_GDS_ET_YMS.xml 의 *_ET_BIN 쿼리를 쓴다. 기존 TQP_BINDESC.xml MERGE_BIN 은 10열을 모두 덮어써 숨긴 열 값을 다시 읽어야 한다.
    ///    (기존 SELECT_BIN_EDITABLE / MERGE_BIN 바인딩 크기 25 / 40 으로 긴 Program / 설명이 잘리던 문제는 2026-10-22 TQP_BINDESC.xml 에서 고침)
    ///  - 파일 Bin 분포(Software Bin / Die / Pass / Fail / 파일 수)는 READY 파일 Die(TQP_GDS_ET_DIE)에서 읽어 정의와 합친다.
    /// 기존 GdsEtResult 와 같이 문장마다 자동 Commit 이다. 저장 / 삭제는 모든 행을 먼저 검사하고 틀린 행이 있으면 아무것도 쓰지 않는다.
    /// </summary>
    public partial class GdsEtResult
    {
        /// <summary>Bin 색 형식: HTML #RRGGBB (YMS Map 이 ColorTranslator.FromHtml 로 읽는다)</summary>
        private static readonly Regex BinColorPattern = new Regex("^#[0-9A-F]{6}$", RegexOptions.CultureInvariant);

        #region [Bin 설정 (TQP_BINDESC)]

        /// <summary>Factory 의 READY 파일이 있는 Device + 공정 목록. YMS_PROGRAM 은 서버 이름 규칙으로 채운다.</summary>
        public DataTable GetEtBinProgramList(string strFactory)
        {
            DataTable dt = new TQP_GDS_ET_YMS().GetData("SELECT_ET_BIN_PROGRAM_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
            dt.Columns.Add("YMS_PROGRAM", typeof(string));
            foreach (DataRow dr in dt.Rows)
                dr["YMS_PROGRAM"] = YmsProgram(dr["DEVICE_ID"].ToString(), dr["TEST_PROGRAM"].ToString());
            return dt;
        }

        /// <summary>
        /// Bin 정의 + 파일 Bin 분포를 Bin 별 한 행으로 합친다. aKey: 0 Factory / 1 Device / 2 공정.
        /// 처리 흐름:
        ///  1) 정의(SELECT_ET_BIN_DEF)를 Bin 번호로 넣는다(HAS_DEF = Y).
        ///  2) 파일 Die 묶음(SELECT_ET_BIN_USE: Hardware Bin x Software Bin x 파일)을 Hardware Bin 으로 더한다. Hardware Bin 이 없으면 BIN 빈 값 행.
        ///  3) 행 순서 = Bin 번호, 번호 없는 행은 끝. IN_RANGE = 0 ~ EtYmsRule.MaxBin (밖이면 화면에서 고치지 않는다).
        /// </summary>
        public DataTable GetEtBinList(string[] aKey)
        {
            string f, d, t, program;
            BinKey(aKey, out f, out d, out t, out program);
            Dictionary<string, BinSummary> byBin = new Dictionary<string, BinSummary>(StringComparer.Ordinal);
            TQP_GDS_ET_YMS oYms = new TQP_GDS_ET_YMS();
            using (DataTable dtDef = oYms.GetData("SELECT_ET_BIN_DEF", null, new string[] { program }))
            {
                foreach (DataRow dr in dtDef.Rows)
                {
                    BinSummary s = Summary(byBin, BinText(dr["BIN"]));
                    s.HasDef = true;
                    s.Name = dr["BIN_NAME"].ToString();
                    s.Good = string.Equals(dr["HIGH_GEC"].ToString().Trim(), "Y", StringComparison.OrdinalIgnoreCase);
                    s.Color = dr["COLOR"].ToString().Trim();
                    s.Description = dr["DESCRIPTION"].ToString();
                }
            }
            using (DataTable dtUse = oYms.GetData("SELECT_ET_BIN_USE", null, new string[] { f, d, t }))
            {
                foreach (DataRow dr in dtUse.Rows)
                {
                    BinSummary s = Summary(byBin, BinText(dr["HW_BIN"]));
                    s.Die += Convert.ToInt64(dr["DIE_COUNT"], CultureInfo.InvariantCulture);
                    s.Pass += Convert.ToInt64(dr["PASS_COUNT"], CultureInfo.InvariantCulture);
                    s.Fail += Convert.ToInt64(dr["FAIL_COUNT"], CultureInfo.InvariantCulture);
                    s.Files.Add(dr["SRC_FILE_SEQ"].ToString());
                    if (dr["SW_BIN"] != DBNull.Value)
                        s.SoftBins.Add(Convert.ToInt64(dr["SW_BIN"], CultureInfo.InvariantCulture));
                }
            }

            DataTable dt = new DataTable("ET_BIN_LIST");
            foreach (string column in new[] { "BIN", "BIN_NAME", "HIGH_GEC", "COLOR", "DESCRIPTION", "HAS_DEF", "SW_BINS" })
                dt.Columns.Add(column, typeof(string));
            foreach (string column in new[] { "DIE_COUNT", "PASS_COUNT", "FAIL_COUNT", "FILE_COUNT" })
                dt.Columns.Add(column, typeof(long));
            dt.Columns.Add("IN_RANGE", typeof(string));
            dt.Columns.Add("YMS_PROGRAM", typeof(string));
            foreach (BinSummary s in byBin.Values.OrderBy(v => BinOrder(v.Bin)).ThenBy(v => v.Bin, StringComparer.Ordinal))
            {
                int bin;
                bool inRange = int.TryParse(s.Bin, NumberStyles.Integer, CultureInfo.InvariantCulture, out bin) && bin >= 0 && bin <= EtYmsRule.MaxBin;
                dt.Rows.Add(s.Bin, s.Name, s.Good ? "Y" : "N", s.Color, s.Description, s.HasDef ? "Y" : "N",
                    string.Join(",", s.SoftBins.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray()),
                    s.Die, s.Pass, s.Fail, (long)s.Files.Count, inRange ? "Y" : "N", program);
            }
            return dt;
        }

        /// <summary>
        /// Bin 정의 저장. aKey: 0 Factory / 1 Device / 2 공정. aRows 열: 0 BIN(0 ~ 99) / 1 이름 / 2 양품(Y / N) / 3 색(빈 값 또는 #RRGGBB) / 4 설명.
        /// 화면에서 바뀐 행과 새 행만 받는다. 같은 Bin 두 번 / 범위 밖 / 길이 초과(이름 30바이트, 설명 100바이트) / 색 형식 오류가 하나라도 있으면
        /// 아무것도 저장하지 않는다. 있으면 4열만 바꾸고(MERGE_ET_BIN), 없으면 새 정의(표시 Y)를 만든다.
        /// 반환 1행: SAVED_COUNT / NEW_COUNT / GOOD_CHANGED(양품이 바뀐 Bin 이 있으면 Y) / YMS_FILE_COUNT(YMS 반영 파일 수, 수율 다시 계산 질문용)
        /// </summary>
        public DataTable SaveEtBins(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            // 작업 기간 확인: 기간이 지나면 여기서 멈추고 저장하지 않는다 (2026-10-23)
            new WorkRule().Confirm(WorkModule.ETest, WorkKind.Setup);
            string f, d, t, program;
            BinKey(aKey, out f, out d, out t, out program);
            Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Bin 이 없습니다.", "aRows");
            if (aRows.GetLength(1) != 5)
                throw new ArgumentException("저장 열 수는 5개(Bin / 이름 / 양품 / 색 / 설명)여야 합니다.", "aRows");
            if (aRows.GetLength(0) > EtYmsRule.MaxBin + 1)
                throw new ArgumentException("한 번에 " + (EtYmsRule.MaxBin + 1) + "행 이하로 저장하세요.", "aRows");

            TQP_GDS_ET_YMS oYms = new TQP_GDS_ET_YMS();
            Dictionary<string, bool> dicGood = new Dictionary<string, bool>(StringComparer.Ordinal);
            using (DataTable dtDef = oYms.GetData("SELECT_ET_BIN_DEF", null, new string[] { program }))
            {
                foreach (DataRow dr in dtDef.Rows)
                    dicGood[BinText(dr["BIN"])] = string.Equals(dr["HIGH_GEC"].ToString().Trim(), "Y", StringComparison.OrdinalIgnoreCase);
            }

            List<string> lstError = new List<string>();
            HashSet<string> setBin = new HashSet<string>(StringComparer.Ordinal);
            List<string[]> lstMerge = new List<string[]>();
            int iNew = 0;
            bool bGoodChanged = false;
            for (int i = 0; i < aRows.GetLength(0); i++)
            {
                try
                {
                    string strBin = RequiredBin(aRows[i, 0]);
                    if (!setBin.Add(strBin))
                        throw new ArgumentException("같은 Bin 이 두 번 있습니다: " + strBin);
                    string strName = OptionalBytes(aRows[i, 1], "Bin " + strBin + " 이름", EtYmsRule.MaxBinNameLength);
                    string strGood = Clean(aRows[i, 2]).ToUpperInvariant();
                    if (strGood != "Y" && strGood != "N")
                        throw new ArgumentException("Bin " + strBin + " 의 양품은 Y 또는 N 이어야 합니다: " + aRows[i, 2]);
                    string strColor = Clean(aRows[i, 3]).ToUpperInvariant();
                    if (strColor.Length > 0 && !BinColorPattern.IsMatch(strColor))
                        throw new ArgumentException("Bin " + strBin + " 의 색은 #RRGGBB 형식이어야 합니다: " + aRows[i, 3]);
                    string strDesc = OptionalBytes(aRows[i, 4], "Bin " + strBin + " 설명", EtYmsRule.MaxBinDescriptionLength);

                    bool bOldGood;
                    if (dicGood.TryGetValue(strBin, out bOldGood))
                        bGoodChanged |= bOldGood != (strGood == "Y");
                    else
                    {
                        iNew++;
                        bGoodChanged |= strGood == "Y";
                    }
                    lstMerge.Add(new string[] { program, strBin, strName, strGood, strColor, strDesc });
                }
                catch (ArgumentException ex)
                {
                    lstError.Add((i + 1) + "행 " + ex.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0]);
                }
            }
            if (lstError.Count > 0)
                throw new ArgumentException("저장할 수 없는 행이 있습니다(" + lstError.Count + "건, 아무것도 저장하지 않음). " + string.Join(" / ", lstError.Take(10).ToArray()), "aRows");

            oYms.UpdateExecuteMultiple("MERGE_ET_BIN", ToArray(lstMerge));

            DataTable dtResult = new DataTable("ET_BIN_SAVE");
            dtResult.Columns.Add("SAVED_COUNT", typeof(int));
            dtResult.Columns.Add("NEW_COUNT", typeof(int));
            dtResult.Columns.Add("GOOD_CHANGED", typeof(string));
            dtResult.Columns.Add("YMS_FILE_COUNT", typeof(int));
            dtResult.Rows.Add(lstMerge.Count, iNew, bGoodChanged ? "Y" : "N", YmsFileCount(f, d, t));
            return dtResult;
        }

        /// <summary>
        /// Bin 정의 삭제. aKey: 0 Factory / 1 Device / 2 공정. aBins = Bin 번호. 정의가 없는 Bin / 같은 Bin 두 번이 있으면 아무것도 지우지 않는다.
        /// 파일에 있는 Bin 은 다음 YMS 반영 때 업로드 규칙 기본값으로 다시 만들어진다. 반환: 지운 정의 수
        /// </summary>
        public int DeleteEtBins(string[] aKey, string[] aBins, string strUpdateUser)
        {
            // 작업 기간 확인: 기간이 지나면 여기서 멈추고 저장하지 않는다 (2026-10-23)
            new WorkRule().Confirm(WorkModule.ETest, WorkKind.Setup);
            string f, d, t, program;
            BinKey(aKey, out f, out d, out t, out program);
            Required(strUpdateUser, "Update User", 30);
            if (aBins == null || aBins.Length == 0)
                throw new ArgumentException("지울 Bin 이 없습니다.", "aBins");

            TQP_GDS_ET_YMS oYms = new TQP_GDS_ET_YMS();
            HashSet<string> setDef = new HashSet<string>(StringComparer.Ordinal);
            using (DataTable dtDef = oYms.GetData("SELECT_ET_BIN_DEF", null, new string[] { program }))
            {
                foreach (DataRow dr in dtDef.Rows)
                    setDef.Add(BinText(dr["BIN"]));
            }
            List<string> lstError = new List<string>();
            HashSet<string> setBin = new HashSet<string>(StringComparer.Ordinal);
            List<string[]> lstDelete = new List<string[]>();
            foreach (string strValue in aBins)
            {
                string strBin = BinText(Clean(strValue));
                if (!setBin.Add(strBin))
                    lstError.Add("같은 Bin 이 두 번 있습니다: " + strBin);
                else if (!setDef.Contains(strBin))
                    lstError.Add("정의가 없는 Bin 입니다: " + strValue);
                else
                    lstDelete.Add(new string[] { program, strBin });
            }
            if (lstError.Count > 0)
                throw new ArgumentException("지울 수 없는 Bin 이 있습니다(아무것도 지우지 않음). " + string.Join(" / ", lstError.Take(10).ToArray()), "aBins");

            oYms.UpdateExecuteMultiple("DELETE_ET_BIN", ToArray(lstDelete));
            return lstDelete.Count;
        }

        /// <summary>Bin 설정 키 검사: 0 Factory / 1 Device / 2 공정 -> YMS Program (빈 값이면 거부)</summary>
        private static void BinKey(string[] aKey, out string f, out string d, out string t, out string program)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Bin 설정 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            f = Required(aKey[0], "Factory", 30);
            d = Required(aKey[1], "Device ID", 50);
            t = RequiredTestProgram(aKey[2]);
            program = YmsProgram(d, t);
            if (program.Length == 0 || program.Length > EtYmsRule.MaxProgramLength)
                throw new ArgumentException("YMS Program 이름을 만들 수 없습니다(Device / 공정 확인, " + EtYmsRule.MaxProgramLength + "자 이하): " + program, "aKey");
        }

        /// <summary>Device + 공정의 READY 파일 중 YMS 반영(DONE) 파일 수</summary>
        private static int YmsFileCount(string f, string d, string t)
        {
            using (DataTable dt = new TQP_GDS_ET_YMS().GetData("SELECT_ET_BIN_PROGRAM_LIST", null, new string[] { f }))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.Equals(dr["DEVICE_ID"].ToString(), d, StringComparison.Ordinal) && string.Equals(dr["TEST_PROGRAM"].ToString(), t, StringComparison.Ordinal))
                        return Convert.ToInt32(dr["YMS_FILE_COUNT"], CultureInfo.InvariantCulture);
                }
            }
            return 0;
        }

        /// <summary>Bin 번호 필수: 0 ~ EtYmsRule.MaxBin 정수</summary>
        private static string RequiredBin(string strValue)
        {
            int iBin;
            if (!int.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out iBin) || iBin < 0 || iBin > EtYmsRule.MaxBin)
                throw new ArgumentException("Bin 은 0 ~ " + EtYmsRule.MaxBin + " 정수여야 합니다: " + strValue);
            return iBin.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>선택 문자열(빈 값 = NULL)이 바이트 길이 컬럼(VARCHAR2(n), BYTE)을 넘지 않는지 확인한다.</summary>
        private static string OptionalBytes(string strValue, string strName, int iMaxBytes)
        {
            string strResult = Clean(strValue);
            if (Encoding.UTF8.GetByteCount(strResult) > iMaxBytes)
                throw new ArgumentException(strName + "은(는) " + iMaxBytes + "바이트 이하로 입력하세요(한글 1자 = 3바이트): " + strValue);
            return strResult;
        }

        /// <summary>DB 숫자 / 문자를 Bin 글자로 (NULL = 빈 값, 숫자는 앞 0 없이)</summary>
        private static string BinText(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            string text = Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
            decimal number;
            if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out number) && number == decimal.Truncate(number))
                return decimal.Truncate(number).ToString(CultureInfo.InvariantCulture);
            return text;
        }

        /// <summary>행 순서: Bin 번호, 번호가 아니거나 없으면 끝</summary>
        private static long BinOrder(string bin)
        {
            long value;
            return long.TryParse(bin, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : long.MaxValue;
        }

        private static BinSummary Summary(Dictionary<string, BinSummary> byBin, string bin)
        {
            BinSummary s;
            if (!byBin.TryGetValue(bin, out s))
                byBin[bin] = s = new BinSummary { Bin = bin };
            return s;
        }

        /// <summary>Bin 한 행: 정의 + 파일 Die 합</summary>
        private sealed class BinSummary
        {
            public string Bin;
            public bool HasDef;
            public string Name = string.Empty;
            public bool Good;
            public string Color = string.Empty;
            public string Description = string.Empty;
            public long Die;
            public long Pass;
            public long Fail;
            public readonly HashSet<string> Files = new HashSet<string>(StringComparer.Ordinal);
            public readonly SortedSet<long> SoftBins = new SortedSet<long>();
        }

        #endregion
    }
}
