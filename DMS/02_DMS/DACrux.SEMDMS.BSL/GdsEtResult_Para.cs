using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.DSL;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// E-Test Para Setup 서버 처리 (2026-10-14).
    /// 설계: 문서/2026-10-14_ETest_Para_Setup_설계.md
    ///  - Parameter 등록: 결과 파일 헤더를 Device + 공정 + Parameter 로 TQP_GDS_ET_ITEM 에 넣는다(TEST_PROGRAM 이 있는 행 = 등록 Parameter).
    ///    이름은 EtParameterNameRule 로 다시 분해 / 변환한다(화면이 보낸 값을 그대로 믿지 않는다).
    ///  - 대표 Chain: 사용자가 단자(E-Test Chain 이름)를 골라 대표 이름을 입력해 묶는다(TQP_GDS_ET_REP_CHAIN, GDS 대표 Device 단위).
    ///  - GDS 연결: GDS Setup Chain 이름이 대표 이름 또는 구성 단자와 똑같으면 잇고(NAME), 아니면 사용자가 고른다(MANUAL). 1:1.
    /// 기존 GdsEtResult 와 같이 문장마다 자동 Commit 이다.
    /// </summary>
    public partial class GdsEtResult
    {
        /// <summary>Parameter 이름 최대 길이 (EtYmsRule.MaxParamNameLength, DB CHECK 와 같음)</summary>
        private const int MaxParameterNameChars = 50;

        #region [Parameter 등록 (TQP_GDS_ET_ITEM)]

        /// <summary>Factory 의 등록 Parameter 가 있는 Device + 공정 목록을 조회한다.</summary>
        public DataTable GetEtParaProgramList(string strFactory)
        {
            return new TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_PROGRAM_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
        }

        /// <summary>
        /// Device + 공정의 등록 Parameter 목록을 조회한다.
        /// 대표 Chain / GDS Chain 열은 E-Test Device 의 GDS 대표 Device(Sub Device 연결, 없으면 같은 이름) 기준이다. 쿼리 순서: g1 g2 f d t
        /// </summary>
        public DataTable GetEtParameterList(string strFactory, string strDeviceID, string strTestProgram)
        {
            string f = Required(strFactory, "Factory", 30);
            string d = Required(strDeviceID, "Device ID", 50);
            string t = RequiredTestProgram(strTestProgram);
            string g = ResolveGdsDevice(f, d);
            return new TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_LIST", null, new string[] { g, g, f, d, t });
        }

        /// <summary>
        /// 헤더를 Parameter 로 등록한다.
        /// 처리 흐름:
        ///  1) 입력 검증: 키 5개(Factory / Device / 공정 / 출처 FILE·SPEC / 파일 이름), 행 4열(헤더 원문 / 단위 / Test No / 열 순서)
        ///  2) 이미 등록된 Parameter 를 읽는다(같은 Device + 공정)
        ///  3) 행마다 EtParameterNameRule.Parse 로 분해 / 변환. 실패 / 같은 헤더 두 번 / 다른 헤더가 같은 Parameter 이름 / 등록된 것과 단위 다름은 모아서 한 번에 거부
        ///  4) 이미 같은 헤더가 등록돼 있으면 건너뛰고, 새 행만 InsertExecuteMultiple 로 넣는다
        /// 반환 1행: REGISTERED / SKIPPED
        /// </summary>
        public DataTable RegisterEtParameters(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 5)
                throw new ArgumentException("Parameter 등록 키는 5개 항목(Factory / Device / 공정 / 출처 / 파일 이름)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string t = RequiredTestProgram(aKey[2]);
            string strSource = Clean(aKey[3]).ToUpperInvariant();
            if (strSource != "FILE" && strSource != "SPEC")
                throw new ArgumentException("등록 출처는 FILE 또는 SPEC 이어야 합니다: " + aKey[3], "aKey");
            string strFileName = OptionalChars(aKey[4], "등록 파일 이름", 260);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("등록할 헤더가 없습니다.", "aRows");
            if (aRows.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("헤더는 한 번에 " + MaxBatchRows + "개 이하로 등록하세요.", "aRows");
            if (aRows.GetLength(1) != 4)
                throw new ArgumentException("등록 열 수는 4개(헤더 / 단위 / Test No / 열 순서)여야 합니다.", "aRows");

            TQP_GDS_ET_ITEM oItem = new TQP_GDS_ET_ITEM();
            Dictionary<string, DataRow> dicExist = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            DataTable dtExist = oItem.GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { f, d, t });
            foreach (DataRow dr in dtExist.Rows)
                dicExist[dr["ITEM_NAME"].ToString()] = dr;

            List<string> lstError = new List<string>();
            List<string[]> lstInsert = new List<string[]>();
            Dictionary<string, string> dicRequest = new Dictionary<string, string>(StringComparer.Ordinal);
            int iSkipped = 0;
            int iRows = aRows.GetLength(0);
            for (int i = 0; i < iRows; i++)
            {
                EtParameterParts parts = EtParameterNameRule.Parse(aRows[i, 0]);
                if (!parts.IsValid)
                {
                    lstError.Add((i + 1) + "행 " + parts.Error + " [" + parts.HeaderText + "]");
                    continue;
                }
                if (parts.CondValue.Length > 20 || parts.MeasType.Length > 30 || parts.ChainType.Length > 30)
                {
                    lstError.Add((i + 1) + "행 조건 / 측정 종류 / Chain 종류가 너무 깁니다 [" + parts.HeaderText + "]");
                    continue;
                }
                string strUnit = OptionalChars(aRows[i, 1], parts.HeaderText + " 단위", 10);
                string strTestNo = OptionalNumber(aRows[i, 2], "Test No");
                string strOrder = OptionalNumber(aRows[i, 3], "열 순서");

                string strPrev;
                if (dicRequest.TryGetValue(parts.ParameterName, out strPrev))
                {
                    lstError.Add(string.Equals(strPrev, parts.HeaderText, StringComparison.OrdinalIgnoreCase)
                        ? "같은 헤더가 두 번 있습니다: " + parts.HeaderText
                        : "서로 다른 헤더가 같은 Parameter 이름 " + parts.ParameterName + " 이 됩니다: " + strPrev + " / " + parts.HeaderText);
                    continue;
                }
                dicRequest.Add(parts.ParameterName, parts.HeaderText);

                DataRow drOld;
                if (dicExist.TryGetValue(parts.ParameterName, out drOld))
                {
                    string strOldHeader = drOld["HEADER_TEXT"].ToString();
                    string strOldUnit = drOld["UNIT"].ToString();
                    if (!string.Equals(strOldHeader, parts.HeaderText, StringComparison.OrdinalIgnoreCase))
                        lstError.Add("이미 등록된 다른 헤더와 Parameter 이름이 같습니다(" + parts.ParameterName + "): " + strOldHeader + " / " + parts.HeaderText);
                    else if (strUnit.Length > 0 && strOldUnit.Length > 0 && !string.Equals(strUnit, strOldUnit, StringComparison.Ordinal))
                        lstError.Add("등록된 단위와 다릅니다: " + parts.HeaderText + " (" + strOldUnit + " -> " + strUnit + ")");
                    else
                        iSkipped++;
                    continue;
                }

                lstInsert.Add(new string[] {
                    f, d, t, parts.ParameterName, parts.HeaderText, parts.ParaName, parts.CondValue, parts.MeasType,
                    parts.Terminal1, parts.Terminal2, parts.ChainType, parts.ForceLeg1, parts.ForceLeg2,
                    strUnit, strTestNo, strOrder, strSource, strFileName, strUser, strUser });
            }
            dtExist.Dispose();
            if (lstError.Count > 0)
                throw new ArgumentException("등록할 수 없는 헤더가 있습니다(" + lstError.Count + "건). " + string.Join(" / ", lstError.Take(10).ToArray()), "aRows");

            if (lstInsert.Count > 0)
            {
                string[,] aInsert = new string[lstInsert.Count, 20];
                for (int r = 0; r < lstInsert.Count; r++)
                    for (int c = 0; c < 20; c++)
                        aInsert[r, c] = lstInsert[r][c];
                oItem.InsertExecuteMultiple("CREATE_ET_PARA", aInsert);
            }

            DataTable dtResult = new DataTable("ET_PARA_REGISTER");
            dtResult.Columns.Add("REGISTERED", typeof(int));
            dtResult.Columns.Add("SKIPPED", typeof(int));
            dtResult.Rows.Add(lstInsert.Count, iSkipped);
            return dtResult;
        }

        /// <summary>
        /// 사용 여부 / 비고를 저장한다. aKey: 0 Factory / 1 Device / 2 공정. aRows 열: 0 Parameter(ITEM_NAME) / 1 USE_FLAG(Y/N) / 2 비고.
        /// 등록되지 않은 Parameter 가 있으면 거부한다(그 앞 행은 이미 저장됨). 반환: 저장한 행 수
        /// </summary>
        public int SaveEtParameters(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Parameter 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string t = RequiredTestProgram(aKey[2]);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Parameter 가 없습니다.", "aRows");
            if (aRows.GetLength(1) != 3)
                throw new ArgumentException("저장 열 수는 3개(Parameter / 사용 / 비고)여야 합니다.", "aRows");

            int iRows = aRows.GetLength(0);
            string[][] aUpdate = new string[iRows][];
            for (int i = 0; i < iRows; i++)
            {
                string strItem = RequiredParameterName(aRows[i, 0]);
                string strUse = Clean(aRows[i, 1]).ToUpperInvariant();
                if (strUse != "Y" && strUse != "N")
                    throw new ArgumentException("사용 여부는 Y 또는 N 이어야 합니다: " + aRows[i, 1], "aRows");
                aUpdate[i] = new string[] { strUse, OptionalChars(aRows[i, 2], "비고", 200), strUser, f, d, t, strItem };
            }

            TQP_GDS_ET_ITEM oItem = new TQP_GDS_ET_ITEM();
            int iSaved = 0;
            foreach (string[] aParam in aUpdate)
            {
                if (oItem.UpdateDataNonQuery("UPDATE_ET_PARA", null, aParam) != 1)
                    throw new InvalidOperationException("등록되지 않은 Parameter 입니다: " + aParam[6]);
                iSaved++;
            }
            return iSaved;
        }

        /// <summary>
        /// Parameter 를 삭제한다. aKey: 0 Factory / 1 Device / 2 공정.
        /// 처리 흐름: 전부 등록된 Parameter 인지 / 측정값이 없는지 먼저 확인(하나라도 걸리면 아무것도 지우지 않음) ->
        ///           같은 키의 Spec(새 키 = ITEM_NAME, 예전 키 = 헤더 원문 대문자) 삭제 -> Parameter 삭제. 반환: 지운 Parameter 수
        /// </summary>
        public int DeleteEtParameters(string[] aKey, string[] aParameters, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Parameter 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string t = RequiredTestProgram(aKey[2]);
            Required(strUpdateUser, "Update User", 30);
            if (aParameters == null || aParameters.Length == 0)
                throw new ArgumentException("삭제할 Parameter 가 없습니다.", "aParameters");

            TQP_GDS_ET_ITEM oItem = new TQP_GDS_ET_ITEM();
            Dictionary<string, string> dicHeader = new Dictionary<string, string>(StringComparer.Ordinal);
            using (DataTable dtExist = oItem.GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { f, d, t }))
            {
                foreach (DataRow dr in dtExist.Rows)
                    dicHeader[dr["ITEM_NAME"].ToString()] = dr["HEADER_TEXT"].ToString();
            }

            List<string> lstItem = new List<string>();
            List<string> lstError = new List<string>();
            foreach (string strValue in aParameters)
            {
                string strItem = RequiredParameterName(strValue);
                if (lstItem.Contains(strItem))
                    continue;
                if (!dicHeader.ContainsKey(strItem))
                {
                    lstError.Add("등록되지 않은 Parameter: " + strItem);
                    continue;
                }
                using (DataTable dtCount = oItem.GetData("SELECT_ET_PARA_VALUE_COUNT", null, new string[] { f, d, t, strItem }))
                {
                    long lValues = Convert.ToInt64(dtCount.Rows[0]["VALUE_COUNT"], CultureInfo.InvariantCulture);
                    if (lValues > 0)
                        lstError.Add("측정값 " + lValues + "개가 저장된 Parameter(사용 안 함으로 바꾸세요): " + strItem);
                }
                lstItem.Add(strItem);
            }
            if (lstError.Count > 0)
                throw new InvalidOperationException("삭제할 수 없습니다(" + lstError.Count + "건). " + string.Join(" / ", lstError.Take(10).ToArray()));

            TQP_GDS_ET_LIMIT oLimit = new TQP_GDS_ET_LIMIT();
            int iDeleted = 0;
            foreach (string strItem in lstItem)
            {
                oLimit.DeleteDataNonQuery("DELETE_ET_LIMIT", null, new string[] { f, d, t, strItem });
                oLimit.DeleteDataNonQuery("DELETE_ET_LIMIT", null, new string[] { f, d, t, dicHeader[strItem].ToUpperInvariant() });
                iDeleted += oItem.DeleteDataNonQuery("DELETE_ET_PARA", null, new string[] { f, d, t, strItem });
            }
            return iDeleted;
        }

        /// <summary>E-Test Device 의 GDS 대표 Device: Sub Device 연결 우선, 다음 같은 이름 READY Map, 둘 다 없으면 E-Test Device 이름 그대로.</summary>
        private static string ResolveGdsDevice(string strFactory, string strDeviceID)
        {
            using (DataTable dt = new TQP_GDS_MAP_SUB_DEVICE().GetData("SELECT_MAP_DEVICE_BY_SUB", null,
                new string[] { strFactory, strDeviceID, strFactory, strDeviceID }))
            {
                return dt.Rows.Count > 0 ? dt.Rows[0]["DEVICE_ID"].ToString() : strDeviceID;
            }
        }

        /// <summary>공정(Test Type) 필수 검증: 이름 규칙으로 바꾼 뒤(예: tsvet -> TSVET) 영문자로 시작 / 50자 이하.</summary>
        private static string RequiredTestProgram(string strValue)
        {
            string strProgram = EtParameterNameRule.ToTestProgram(strValue);
            if (!EtParameterNameRule.IsSafeName(strProgram, EtParameterNameRule.MaxTestProgramLength))
                throw new ArgumentException("공정(Test Type)은 영문자로 시작하는 이름이어야 합니다: " + strValue, "TestProgram");
            return strProgram;
        }

        /// <summary>Parameter 이름(ITEM_NAME) 필수 검증: 대문자 / 숫자 / '_', 영문자로 시작, 50자 이하. 화면이 목록에서 받은 변환 이름을 그대로 보낸다.</summary>
        private static string RequiredParameterName(string strValue)
        {
            string strItem = Clean(strValue).ToUpperInvariant();
            if (!EtParameterNameRule.IsSafeName(strItem, MaxParameterNameChars))
                throw new ArgumentException("Parameter 이름이 올바르지 않습니다: " + strValue, "Parameter");
            return strItem;
        }

        #endregion

        #region [대표 Chain (TQP_GDS_ET_REP_CHAIN) / GDS 연결 (TQP_GDS_ET_CHAIN_LINK)]

        /// <summary>
        /// 단자 목록을 조회한다(등록 Parameter 의 단자 + 대표 Chain 에만 남은 단자). 쿼리 순서: f1 g1 g2 f2 f3 f4 g3.
        /// 대표 Chain 에만 남은 단자는 Force leg / Chain 종류를 이름 규칙으로 채운다.
        /// </summary>
        public DataTable GetEtTerminalList(string strFactory, string strGdsDeviceID)
        {
            string f = Required(strFactory, "Factory", 30);
            string g = Required(strGdsDeviceID, "GDS Device", 50);
            DataTable dt = new TQP_GDS_ET_REP_CHAIN().GetData("SELECT_ET_TERMINAL_LIST", null, new string[] { f, g, g, f, f, f, g });
            foreach (DataRow dr in dt.Rows)
            {
                string strTerminal = dr["TERMINAL_NAME"].ToString();
                if (dr["FORCE_LEG"] == DBNull.Value)
                    dr["FORCE_LEG"] = EtParameterNameRule.ForceLegOf(strTerminal);
                if (dr["CHAIN_TYPE"] == DBNull.Value)
                    dr["CHAIN_TYPE"] = EtParameterNameRule.ChainTypeOf(strTerminal);
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>대표 Chain 목록 + GDS 연결 상태 (LINK_STATUS 의미는 Interface 주석). 상태는 이 서버가 이름 규칙으로 계산한다.</summary>
        public DataTable GetEtRepChainList(string strFactory, string strGdsDeviceID)
        {
            string f = Required(strFactory, "Factory", 30);
            string g = Required(strGdsDeviceID, "GDS Device", 50);
            RepChainSnapshot snap = LoadRepChainSnapshot(f, g);

            DataTable dt = new DataTable("ET_REP_CHAIN");
            dt.Columns.Add("REP_CHAIN_NAME", typeof(string));
            dt.Columns.Add("TERMINALS", typeof(string));
            dt.Columns.Add("TERMINAL_COUNT", typeof(int));
            dt.Columns.Add("CHAIN_SEQ", typeof(string));
            dt.Columns.Add("GDS_CHAIN_NAME", typeof(string));
            dt.Columns.Add("LINK_TYPE", typeof(string));
            dt.Columns.Add("LINK_STATUS", typeof(string));
            dt.Columns.Add("CANDIDATES", typeof(string));
            foreach (KeyValuePair<string, List<string>> rep in snap.Members)
            {
                LinkRow link;
                snap.Links.TryGetValue(rep.Key, out link);
                List<GdsChainRow> lstCandidate = snap.FindCandidates(rep.Key);
                string strStatus;
                string strSeq = string.Empty;
                string strGdsName = string.Empty;
                string strType = string.Empty;
                if (link != null)
                {
                    GdsChainRow gds = snap.FindGds(link.ChainSeq);
                    strSeq = link.ChainSeq.ToString(CultureInfo.InvariantCulture);
                    strGdsName = gds != null ? gds.Name : link.GdsChainName;
                    strType = link.LinkType;
                    if (link.LinkType == "MANUAL")
                        strStatus = "MANUAL";
                    else
                        strStatus = gds != null && snap.NameSet(rep.Key).Contains(gds.Key) ? "NAME" : "NAME_CHANGED";
                }
                else if (lstCandidate.Count == 0)
                    strStatus = "NONE";
                else if (lstCandidate.Count > 1)
                    strStatus = "MULTIPLE";
                else
                    strStatus = snap.SeqOwner(lstCandidate[0].Seq) != null ? "TAKEN" : "READY";
                dt.Rows.Add(rep.Key, string.Join(", ", rep.Value.ToArray()), rep.Value.Count, strSeq, strGdsName, strType, strStatus,
                    string.Join(", ", lstCandidate.Select(c => c.Name).ToArray()));
            }
            return dt;
        }

        /// <summary>GDS Setup Chain 목록 + 연결된 대표. NAME_KEY 는 비교 이름(EtParameterNameRule.ToSafeName).</summary>
        public DataTable GetEtGdsChainLinkList(string strFactory, string strGdsDeviceID)
        {
            string f = Required(strFactory, "Factory", 30);
            string g = Required(strGdsDeviceID, "GDS Device", 50);
            RepChainSnapshot snap = LoadRepChainSnapshot(f, g);
            DataTable dt = new DataTable("ET_GDS_CHAIN_LINK");
            dt.Columns.Add("CHAIN_SEQ", typeof(string));
            dt.Columns.Add("CHAIN_CODE", typeof(string));
            dt.Columns.Add("CHAIN_NAME", typeof(string));
            dt.Columns.Add("NAME_KEY", typeof(string));
            dt.Columns.Add("REP_CHAIN_NAME", typeof(string));
            dt.Columns.Add("LINK_TYPE", typeof(string));
            foreach (GdsChainRow gds in snap.GdsChains)
            {
                string strRep = snap.SeqOwner(gds.Seq);
                string strType = strRep != null ? snap.Links[strRep].LinkType : string.Empty;
                dt.Rows.Add(gds.Seq.ToString(CultureInfo.InvariantCulture), gds.Code, gds.Name, gds.Key, strRep ?? string.Empty, strType);
            }
            return dt;
        }

        /// <summary>
        /// 단자를 대표 Chain 으로 묶는다. aKey: 0 Factory / 1 GDS Device / 2 대표 이름(입력값).
        /// 처리 흐름: 대표 이름 / 단자 이름을 이름 규칙으로 바꿔 검증 -> MERGE(다른 대표에 있던 단자는 옮김) -> 이름 일치 연결 다시 계산. 반환: 저장한 단자 수
        /// </summary>
        public int SaveEtRepChain(string[] aKey, string[] aTerminals, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("대표 Chain 키는 3개 항목(Factory / GDS Device / 대표 이름)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string g = Required(aKey[1], "GDS Device", 50);
            string strRep = RequiredSafeName(aKey[2], "대표 Chain");
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aTerminals == null || aTerminals.Length == 0)
                throw new ArgumentException("묶을 단자가 없습니다.", "aTerminals");
            if (aTerminals.Length > MaxBatchRows)
                throw new ArgumentException("단자는 한 번에 " + MaxBatchRows + "개 이하로 묶으세요.", "aTerminals");

            List<string> lstTerminal = new List<string>();
            foreach (string strValue in aTerminals)
            {
                string strTerminal = RequiredSafeName(strValue, "단자");
                if (!lstTerminal.Contains(strTerminal))
                    lstTerminal.Add(strTerminal);
            }
            string[,] aMerge = new string[lstTerminal.Count, 5];
            for (int i = 0; i < lstTerminal.Count; i++)
            {
                aMerge[i, 0] = f;
                aMerge[i, 1] = g;
                aMerge[i, 2] = lstTerminal[i];
                aMerge[i, 3] = strRep;
                aMerge[i, 4] = strUser;
            }
            new TQP_GDS_ET_REP_CHAIN().InsertExecuteMultiple("MERGE_ET_REP_CHAIN", aMerge);
            LinkByName(f, g, strUser);
            return lstTerminal.Count;
        }

        /// <summary>대표 이름을 바꾼다. aKey: 0 Factory / 1 GDS Device / 2 이전 이름 / 3 새 이름. 연결도 같이 바꾼 뒤 이름 일치 연결을 다시 계산한다.</summary>
        public int RenameEtRepChain(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 4)
                throw new ArgumentException("대표 Chain 이름 바꾸기 키는 4개 항목(Factory / GDS Device / 이전 이름 / 새 이름)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string g = Required(aKey[1], "GDS Device", 50);
            string strOld = RequiredSafeName(aKey[2], "이전 대표 Chain");
            string strNew = RequiredSafeName(aKey[3], "새 대표 Chain");
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (strOld == strNew)
                return 0;

            RepChainSnapshot snap = LoadRepChainSnapshot(f, g);
            if (!snap.Members.ContainsKey(strOld))
                throw new InvalidOperationException("대표 Chain 이 없습니다: " + strOld);
            if (snap.Members.ContainsKey(strNew))
                throw new InvalidOperationException("같은 이름의 대표 Chain 이 이미 있습니다: " + strNew);

            TQP_GDS_ET_CHAIN_LINK oLink = new TQP_GDS_ET_CHAIN_LINK();
            oLink.DeleteDataNonQuery("DELETE_ET_CHAIN_LINK_ORPHAN", null, new string[] { f, g, f, g });
            int iRows = new TQP_GDS_ET_REP_CHAIN().UpdateDataNonQuery("UPDATE_ET_REP_CHAIN_NAME", null, new string[] { strNew, strUser, f, g, strOld });
            oLink.UpdateDataNonQuery("UPDATE_ET_CHAIN_LINK_NAME", null, new string[] { strNew, strUser, f, g, strOld });
            LinkByName(f, g, strUser);
            return iRows;
        }

        /// <summary>단자를 대표에서 뺀다. aKey: 0 Factory / 1 GDS Device. 단자가 0 이 된 대표의 연결과 이름이 더 이상 맞지 않는 NAME 연결을 정리한다.</summary>
        public int RemoveEtRepChainMembers(string[] aKey, string[] aTerminals, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 2)
                throw new ArgumentException("단자 빼기 키는 2개 항목(Factory / GDS Device)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string g = Required(aKey[1], "GDS Device", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aTerminals == null || aTerminals.Length == 0)
                throw new ArgumentException("뺄 단자가 없습니다.", "aTerminals");

            TQP_GDS_ET_REP_CHAIN oRep = new TQP_GDS_ET_REP_CHAIN();
            int iRows = 0;
            foreach (string strValue in aTerminals.Distinct())
                iRows += oRep.DeleteDataNonQuery("DELETE_ET_REP_CHAIN_MEMBER", null, new string[] { f, g, RequiredSafeName(strValue, "단자") });
            LinkByName(f, g, strUser);
            return iRows;
        }

        /// <summary>대표 Chain 을 지운다(연결 -> 단자 순서). aKey: 0 Factory / 1 GDS Device / 2 대표 이름. 반환: 지운 단자 수</summary>
        public int DeleteEtRepChain(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("대표 Chain 삭제 키는 3개 항목(Factory / GDS Device / 대표 이름)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string g = Required(aKey[1], "GDS Device", 50);
            string strRep = RequiredSafeName(aKey[2], "대표 Chain");
            Required(strUpdateUser, "Update User", 30);
            new TQP_GDS_ET_CHAIN_LINK().DeleteDataNonQuery("DELETE_ET_CHAIN_LINK", null, new string[] { f, g, strRep });
            return new TQP_GDS_ET_REP_CHAIN().DeleteDataNonQuery("DELETE_ET_REP_CHAIN", null, new string[] { f, g, strRep });
        }

        /// <summary>이름 일치 연결을 다시 계산한다 (LinkByName). 반환 1행: LINKED / UPDATED / REMOVED / NONE / MULTIPLE / TAKEN</summary>
        public DataTable LinkEtRepChainsByName(string strFactory, string strGdsDeviceID, string strUpdateUser)
        {
            int[] aCount = LinkByName(Required(strFactory, "Factory", 30), Required(strGdsDeviceID, "GDS Device", 50), Required(strUpdateUser, "Update User", 30));
            DataTable dt = new DataTable("ET_CHAIN_LINK_RESULT");
            foreach (string strName in new string[] { "LINKED", "UPDATED", "REMOVED", "NONE", "MULTIPLE", "TAKEN" })
                dt.Columns.Add(strName, typeof(int));
            dt.Rows.Add(aCount[0], aCount[1], aCount[2], aCount[3], aCount[4], aCount[5]);
            return dt;
        }

        /// <summary>
        /// 사용자 연결(MANUAL). aKey: 0 Factory / 1 GDS Device / 2 대표 이름 / 3 GDS CHAIN_SEQ.
        /// 대표 Chain 과 GDS Setup Chain(ACTIVE / READY)이 있어야 한다. 그 GDS Chain 의 다른 대표 연결은 지우고 옮긴다(1:1). 반환 1
        /// </summary>
        public int SaveEtChainLink(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 4)
                throw new ArgumentException("연결 키는 4개 항목(Factory / GDS Device / 대표 이름 / GDS Chain 번호)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string g = Required(aKey[1], "GDS Device", 50);
            string strRep = RequiredSafeName(aKey[2], "대표 Chain");
            string strSeq = RequiredSeq(aKey[3], "GDS Chain 번호");
            string strUser = Required(strUpdateUser, "Update User", 30);

            RepChainSnapshot snap = LoadRepChainSnapshot(f, g);
            if (!snap.Members.ContainsKey(strRep))
                throw new InvalidOperationException("대표 Chain 이 없습니다: " + strRep);
            GdsChainRow gds = snap.FindGds(long.Parse(strSeq, CultureInfo.InvariantCulture));
            if (gds == null)
                throw new InvalidOperationException("GDS Setup Chain(ACTIVE / READY) 이 아닙니다: " + strSeq);

            TQP_GDS_ET_CHAIN_LINK oLink = new TQP_GDS_ET_CHAIN_LINK();
            oLink.DeleteDataNonQuery("DELETE_ET_CHAIN_LINK_BY_SEQ", null, new string[] { f, g, strSeq });
            oLink.InsertDataNonQuery("MERGE_ET_CHAIN_LINK", null, new string[] { f, g, strRep, strSeq, gds.Name, "MANUAL", strUser });
            return 1;
        }

        /// <summary>연결 해제. aKey: 0 Factory / 1 GDS Device / 2 대표 이름. 반환: 지운 행 수</summary>
        public int DeleteEtChainLink(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("연결 해제 키는 3개 항목(Factory / GDS Device / 대표 이름)이어야 합니다.", "aKey");
            Required(strUpdateUser, "Update User", 30);
            return new TQP_GDS_ET_CHAIN_LINK().DeleteDataNonQuery("DELETE_ET_CHAIN_LINK", null, new string[] {
                Required(aKey[0], "Factory", 30), Required(aKey[1], "GDS Device", 50), RequiredSafeName(aKey[2], "대표 Chain") });
        }

        /// <summary>대표 Chain 의 단자가 들어간 모든 공정의 등록 Parameter + Spec. 쿼리 순서: f1 g1 g2 f2 g3 rep f3</summary>
        public DataTable GetEtRepChainSpecInfo(string strFactory, string strGdsDeviceID, string strRepChainName)
        {
            string f = Required(strFactory, "Factory", 30);
            string g = Required(strGdsDeviceID, "GDS Device", 50);
            string strRep = RequiredSafeName(strRepChainName, "대표 Chain");
            return new TQP_GDS_ET_REP_CHAIN().GetData("SELECT_ET_PARA_BY_REP_CHAIN", null, new string[] { f, g, g, f, g, strRep, f });
        }

        /// <summary>
        /// 이름 일치 연결 계산 (설계 7.1절).
        /// 1) 단자가 없는 대표의 연결을 지운다
        /// 2) NAME 연결 중 GDS 지금 이름(비교 이름)이 대표 이름 / 구성 단자와 맞지 않는 것은 지우고, 맞지만 저장된 이름이 다르면 갱신한다
        /// 3) 연결 없는 대표: 이름이 같은 GDS Chain 이 1개이고 다른 대표가 쓰지 않으면 NAME 연결. 0개 NONE / 2개 이상 MULTIPLE / 다른 대표가 씀 TAKEN
        /// MANUAL 연결은 건드리지 않는다. 반환: LINKED / UPDATED / REMOVED / NONE / MULTIPLE / TAKEN
        /// </summary>
        private int[] LinkByName(string f, string g, string strUser)
        {
            TQP_GDS_ET_CHAIN_LINK oLink = new TQP_GDS_ET_CHAIN_LINK();
            int iLinked = 0, iUpdated = 0, iNone = 0, iMultiple = 0, iTaken = 0;
            int iRemoved = oLink.DeleteDataNonQuery("DELETE_ET_CHAIN_LINK_ORPHAN", null, new string[] { f, g, f, g });

            RepChainSnapshot snap = LoadRepChainSnapshot(f, g);
            foreach (LinkRow link in snap.Links.Values.ToList())
            {
                if (link.LinkType != "NAME")
                    continue;
                GdsChainRow gds = snap.FindGds(link.ChainSeq);
                if (gds == null || !snap.NameSet(link.RepChainName).Contains(gds.Key))
                {
                    iRemoved += oLink.DeleteDataNonQuery("DELETE_ET_CHAIN_LINK", null, new string[] { f, g, link.RepChainName });
                    snap.Links.Remove(link.RepChainName);
                    continue;
                }
                if (!string.Equals(link.GdsChainName, gds.Name, StringComparison.Ordinal))
                {
                    oLink.InsertDataNonQuery("MERGE_ET_CHAIN_LINK", null, new string[] {
                        f, g, link.RepChainName, link.ChainSeq.ToString(CultureInfo.InvariantCulture), gds.Name, "NAME", strUser });
                    iUpdated++;
                }
            }

            foreach (string strRep in snap.Members.Keys)
            {
                if (snap.Links.ContainsKey(strRep))
                    continue;
                List<GdsChainRow> lstCandidate = snap.FindCandidates(strRep);
                if (lstCandidate.Count == 0)
                {
                    iNone++;
                    continue;
                }
                if (lstCandidate.Count > 1)
                {
                    iMultiple++;
                    continue;
                }
                GdsChainRow gds = lstCandidate[0];
                if (snap.SeqOwner(gds.Seq) != null)
                {
                    iTaken++;
                    continue;
                }
                oLink.InsertDataNonQuery("MERGE_ET_CHAIN_LINK", null, new string[] {
                    f, g, strRep, gds.Seq.ToString(CultureInfo.InvariantCulture), gds.Name, "NAME", strUser });
                snap.Links[strRep] = new LinkRow(strRep, gds.Seq, gds.Name, "NAME");
                iLinked++;
            }
            return new int[] { iLinked, iUpdated, iRemoved, iNone, iMultiple, iTaken };
        }

        /// <summary>대표 Chain 구성 / 연결 / GDS Setup Chain 을 한 번에 읽는다.</summary>
        private static RepChainSnapshot LoadRepChainSnapshot(string f, string g)
        {
            RepChainSnapshot snap = new RepChainSnapshot();
            using (DataTable dt = new TQP_GDS_ET_REP_CHAIN().GetData("SELECT_ET_REP_CHAIN_MEMBER_LIST", null, new string[] { f, g }))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string strRep = dr["REP_CHAIN_NAME"].ToString();
                    List<string> lst;
                    if (!snap.Members.TryGetValue(strRep, out lst))
                    {
                        lst = new List<string>();
                        snap.Members.Add(strRep, lst);
                    }
                    lst.Add(dr["TERMINAL_NAME"].ToString());
                }
            }
            using (DataTable dt = new TQP_GDS_ET_CHAIN_LINK().GetData("SELECT_ET_CHAIN_LINK_LIST", null, new string[] { f, g }))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    LinkRow link = new LinkRow(dr["REP_CHAIN_NAME"].ToString(), Convert.ToInt64(dr["CHAIN_SEQ"], CultureInfo.InvariantCulture),
                        dr["GDS_CHAIN_NAME"].ToString(), dr["LINK_TYPE"].ToString());
                    snap.Links[link.RepChainName] = link;
                }
            }
            using (DataTable dt = new TQP_GDS_ET_REP_CHAIN().GetData("SELECT_ET_GDS_SETUP_CHAIN", null, new string[] { f, g }))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string strName = dr["CHAIN_NAME"].ToString();
                    snap.GdsChains.Add(new GdsChainRow(Convert.ToInt64(dr["CHAIN_SEQ"], CultureInfo.InvariantCulture),
                        dr["CHAIN_CODE"].ToString(), strName, EtParameterNameRule.ToSafeName(strName)));
                }
            }
            return snap;
        }

        /// <summary>이름 규칙으로 바꾼 이름이 저장 가능한지 확인하고 돌려준다(영문자로 시작, 64자 이하).</summary>
        private static string RequiredSafeName(string strValue, string strName)
        {
            string strSafe = EtParameterNameRule.ToSafeName(strValue);
            if (!EtParameterNameRule.IsSafeName(strSafe, EtParameterNameRule.MaxChainNameLength))
                throw new ArgumentException(strName + " 이름을 만들 수 없습니다(영문자로 시작, A~Z / 0~9 / '_', "
                    + EtParameterNameRule.MaxChainNameLength + "자 이하): " + strValue, strName);
            return strSafe;
        }

        /// <summary>대표 Chain 구성 / 연결 / GDS Setup Chain 을 메모리에 들고 이름 비교를 돕는다.</summary>
        private sealed class RepChainSnapshot
        {
            /// <summary>대표 이름 -> 구성 단자 (이름 순)</summary>
            public readonly SortedDictionary<string, List<string>> Members = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);
            /// <summary>대표 이름 -> 연결</summary>
            public readonly Dictionary<string, LinkRow> Links = new Dictionary<string, LinkRow>(StringComparer.Ordinal);
            /// <summary>GDS Setup Chain (이름 순)</summary>
            public readonly List<GdsChainRow> GdsChains = new List<GdsChainRow>();

            /// <summary>대표 이름 + 구성 단자 이름 (GDS 이름과 비교할 이름들)</summary>
            public HashSet<string> NameSet(string strRep)
            {
                HashSet<string> set = new HashSet<string>(StringComparer.Ordinal) { strRep };
                List<string> lst;
                if (Members.TryGetValue(strRep, out lst))
                    set.UnionWith(lst);
                return set;
            }

            /// <summary>이름(비교 이름)이 대표 이름 또는 구성 단자와 같은 GDS Chain</summary>
            public List<GdsChainRow> FindCandidates(string strRep)
            {
                HashSet<string> set = NameSet(strRep);
                return GdsChains.Where(c => set.Contains(c.Key)).ToList();
            }

            public GdsChainRow FindGds(long lSeq)
            {
                return GdsChains.FirstOrDefault(c => c.Seq == lSeq);
            }

            /// <summary>GDS Chain 을 연결해 쓰고 있는 대표 이름. 없으면 null</summary>
            public string SeqOwner(long lSeq)
            {
                foreach (LinkRow link in Links.Values)
                    if (link.ChainSeq == lSeq)
                        return link.RepChainName;
                return null;
            }
        }

        private sealed class LinkRow
        {
            public readonly string RepChainName;
            public readonly long ChainSeq;
            public readonly string GdsChainName;
            public readonly string LinkType;

            public LinkRow(string strRep, long lSeq, string strGdsName, string strType)
            {
                RepChainName = strRep;
                ChainSeq = lSeq;
                GdsChainName = strGdsName;
                LinkType = strType;
            }
        }

        private sealed class GdsChainRow
        {
            public readonly long Seq;
            public readonly string Code;
            public readonly string Name;
            /// <summary>비교 이름 (EtParameterNameRule.ToSafeName)</summary>
            public readonly string Key;

            public GdsChainRow(long lSeq, string strCode, string strName, string strKey)
            {
                Seq = lSeq;
                Code = strCode;
                Name = strName;
                Key = strKey;
            }
        }

        #endregion
    }
}
