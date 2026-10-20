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
    /// E-Test Para 설정 서버 처리 (2026-10-14).
    /// 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md (등록 / 이름 변환), 문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md (GDS Chain / Spec 편집)
    ///  - Parameter 등록: 결과 파일 헤더를 Device + 공정 + Parameter 로 TQP_GDS_ET_ITEM 에 넣는다.
    ///    이름은 EtParameterNameRule 로 다시 분해 / 변환한다(화면이 보낸 값을 그대로 믿지 않는다).
    ///  - [등록 Parameter] 탭: Parameter 마다 사용 / 비고 / GDS Chain(Chain 1 의 GDS Setup Chain 이름, 사용자 입력) / Spec 을 같이 편집하고 수정된 행만 저장한다.
    ///    대표 Chain / GDS 연결(TQP_GDS_ET_REP_CHAIN / TQP_GDS_ET_CHAIN_LINK)은 같은 날 화면 재구성으로 지웠다.
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

        /// <summary>Device + 공정의 등록 Parameter 목록을 조회한다(업로드 계획 / 조회 창). GDS_CHAIN_NAME 은 Parameter 에 저장된 값 그대로다.</summary>
        public DataTable GetEtParameterList(string strFactory, string strDeviceID, string strTestProgram)
        {
            string f = Required(strFactory, "Factory", 30);
            string d = Required(strDeviceID, "Device ID", 50);
            string t = RequiredTestProgram(strTestProgram);
            return new TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_LIST", null, new string[] { f, d, t });
        }

        /// <summary>
        /// E-Test Para 설정 화면 목록: 등록 Parameter + GDS Chain + Spec. 쿼리 순서: f1 d1 t1 f2 d2 t2
        /// </summary>
        public DataTable GetEtParameterSetupList(string strFactory, string strDeviceID, string strTestProgram)
        {
            string f = Required(strFactory, "Factory", 30);
            string d = Required(strDeviceID, "Device ID", 50);
            string t = RequiredTestProgram(strTestProgram);
            return new TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_SETUP_LIST", null, new string[] { f, d, t, f, d, t });
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
                    parts.Chain1, parts.Chain2, parts.ChainType, parts.ForceLeg1, parts.ForceLeg2,
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
        /// [등록 Parameter] 탭 저장 (2026-10-14 Para 화면 재구성). 화면에서 수정된 행만 받는다. aKey: 0 Factory / 1 Device / 2 공정.
        /// aRows 열: 0 Parameter(저장 이름) / 1 사용(Y/N) / 2 비고 / 3 GDS Chain / 4 Lower / 5 Upper / 6 Enable(Y/N)
        /// 처리 흐름:
        ///  1) 값 확인: 등록된 Parameter / 같은 행 두 번 / 사용 Y·N / 비고 200자 / GDS Chain 200자 / Lower·Upper 숫자 / Lower ≤ Upper /
        ///     Enable 이면 Limit 1개 이상. 틀린 것은 모아서 한 번에 거부(아무것도 저장하지 않음)
        ///  2) 배열 UPDATE (사용 / 비고 / GDS Chain) - 받은 행 전부
        ///  3) Limit 이 있고 Spec(Lower / Upper / Enable)이 기존과 다른 행: 배열 MERGE(MERGE_ET_LIMIT). 단위 = 기존 Spec 단위, 없으면 등록 단위.
        ///     Spec 원문 / Spec 비고는 기존 값을 그대로 둔다. 같은 값이면 다시 쓰지 않는다(GDS Chain 만 고친 행의 Spec 수정자 / 시각 유지)
        ///  4) Limit 이 모두 빈 행 중 Spec 이 있던 것: 배열 DELETE
        /// 반환 1행: UPDATED / SPEC_SAVED(새로 만들거나 바뀐 Spec) / SPEC_DELETED
        /// </summary>
        public DataTable SaveEtParameterSetup(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Parameter 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string t = RequiredTestProgram(aKey[2]);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Parameter 가 없습니다.", "aRows");
            if (aRows.GetLength(1) != 7)
                throw new ArgumentException("저장 열 수는 7개(Parameter / 사용 / 비고 / GDS Chain / Lower / Upper / Enable)여야 합니다.", "aRows");
            if (aRows.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("한 번에 " + MaxBatchRows + "행 이하로 저장하세요.", "aRows");

            TQP_GDS_ET_ITEM oItem = new TQP_GDS_ET_ITEM();
            Dictionary<string, string> dicUnit = new Dictionary<string, string>(StringComparer.Ordinal);
            using (DataTable dtItem = oItem.GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { f, d, t }))
            {
                foreach (DataRow dr in dtItem.Rows)
                    dicUnit[dr["ITEM_NAME"].ToString()] = dr["UNIT"].ToString();
            }
            TQP_GDS_ET_LIMIT oLimit = new TQP_GDS_ET_LIMIT();
            Dictionary<string, DataRow> dicSpec = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            DataTable dtSpec = oLimit.GetData("SELECT_ET_LIMIT", null, new string[] { f, d, t });
            foreach (DataRow dr in dtSpec.Rows)
                dicSpec[dr["ITEM_NAME"].ToString()] = dr;

            int iRows = aRows.GetLength(0);
            List<string> lstError = new List<string>();
            HashSet<string> setItem = new HashSet<string>(StringComparer.Ordinal);
            List<string[]> lstUpdate = new List<string[]>();
            List<string[]> lstMerge = new List<string[]>();
            List<string[]> lstDelete = new List<string[]>();
            for (int i = 0; i < iRows; i++)
            {
                string strItem;
                try
                {
                    strItem = RequiredParameterName(aRows[i, 0]);
                    if (!setItem.Add(strItem))
                        throw new ArgumentException("같은 Parameter 가 두 번 있습니다: " + strItem);
                    string strItemUnit;
                    if (!dicUnit.TryGetValue(strItem, out strItemUnit))
                        throw new ArgumentException("등록되지 않은 Parameter 입니다: " + strItem);
                    string strUse = Clean(aRows[i, 1]).ToUpperInvariant();
                    if (strUse != "Y" && strUse != "N")
                        throw new ArgumentException(strItem + " 의 사용은 Y 또는 N 이어야 합니다: " + aRows[i, 1]);
                    string strRemark = OptionalChars(aRows[i, 2], strItem + " 비고", 200);
                    string strGds = OptionalChars(aRows[i, 3], strItem + " GDS Chain", 200);
                    string strLower = OptionalDouble(aRows[i, 4], strItem + " Lower");
                    string strUpper = OptionalDouble(aRows[i, 5], strItem + " Upper");
                    string strEnable = Clean(aRows[i, 6]).ToUpperInvariant();
                    if (strEnable != "Y" && strEnable != "N")
                        throw new ArgumentException(strItem + " 의 Enable 은 Y 또는 N 이어야 합니다: " + aRows[i, 6]);
                    if (strLower.Length > 0 && strUpper.Length > 0
                        && double.Parse(strLower, CultureInfo.InvariantCulture) > double.Parse(strUpper, CultureInfo.InvariantCulture))
                        throw new ArgumentException(strItem + " 의 Lower(" + strLower + ") 가 Upper(" + strUpper + ") 보다 큽니다.");
                    if (strEnable == "Y" && strLower.Length == 0 && strUpper.Length == 0)
                        throw new ArgumentException(strItem + " 은 Enable 인데 Lower / Upper 가 없습니다.");

                    lstUpdate.Add(new string[] { strUse, strRemark, strGds, strUser, f, d, t, strItem });
                    DataRow drSpec;
                    dicSpec.TryGetValue(strItem, out drSpec);
                    if ((strLower.Length > 0 || strUpper.Length > 0) && !SameSpec(drSpec, strLower, strUpper, strEnable))
                    {
                        string strUnit = drSpec != null && drSpec["UNIT"].ToString().Length > 0 ? drSpec["UNIT"].ToString() : strItemUnit;
                        lstMerge.Add(new string[] { f, d, t, strItem, strLower, strUpper, strUnit, strEnable,
                            drSpec != null ? drSpec["SPEC_TEXT"].ToString() : string.Empty, drSpec != null ? drSpec["REMARK"].ToString() : string.Empty, strUser });
                    }
                    else if (strLower.Length == 0 && strUpper.Length == 0 && drSpec != null)
                        lstDelete.Add(new string[] { f, d, t, strItem });
                }
                catch (ArgumentException ex)
                {
                    lstError.Add((i + 1) + "행 " + ex.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0]);
                }
            }
            dtSpec.Dispose();
            if (lstError.Count > 0)
                throw new ArgumentException("저장할 수 없는 행이 있습니다(" + lstError.Count + "건, 아무것도 저장하지 않음). " + string.Join(" / ", lstError.Take(10).ToArray()), "aRows");

            oItem.UpdateExecuteMultiple("UPDATE_ET_PARA", ToArray(lstUpdate));
            if (lstMerge.Count > 0)
                oLimit.InsertExecuteMultiple("MERGE_ET_LIMIT", ToArray(lstMerge));
            if (lstDelete.Count > 0)
                oLimit.DeleteExecuteMultiple("DELETE_ET_LIMIT", ToArray(lstDelete));

            DataTable dtResult = new DataTable("ET_PARA_SETUP_SAVE");
            dtResult.Columns.Add("UPDATED", typeof(int));
            dtResult.Columns.Add("SPEC_SAVED", typeof(int));
            dtResult.Columns.Add("SPEC_DELETED", typeof(int));
            dtResult.Rows.Add(lstUpdate.Count, lstMerge.Count, lstDelete.Count);
            return dtResult;
        }

        /// <summary>저장된 Spec 행이 있고 Lower / Upper(숫자로 비교, 빈 값 = NULL) / Enable 이 모두 같은지.</summary>
        private static bool SameSpec(DataRow drSpec, string strLower, string strUpper, string strEnable)
        {
            return drSpec != null && SameLimit(drSpec["LOWER_LIMIT"], strLower) && SameLimit(drSpec["UPPER_LIMIT"], strUpper)
                && string.Equals(drSpec["ENABLE_FLAG"].ToString(), strEnable, StringComparison.Ordinal);
        }

        private static bool SameLimit(object oSaved, string strValue)
        {
            if (oSaved == null || oSaved == DBNull.Value)
                return strValue.Length == 0;
            return strValue.Length > 0 && Convert.ToDouble(oSaved, CultureInfo.InvariantCulture) == double.Parse(strValue, CultureInfo.InvariantCulture);
        }

        /// <summary>같은 열 수의 행 목록을 배열 실행용 2차원 배열로 바꾼다.</summary>
        private static string[,] ToArray(List<string[]> lstRows)
        {
            int iCols = lstRows.Count == 0 ? 0 : lstRows[0].Length;
            string[,] aRows = new string[lstRows.Count, iCols];
            for (int r = 0; r < lstRows.Count; r++)
                for (int c = 0; c < iCols; c++)
                    aRows[r, c] = lstRows[r][c];
            return aRows;
        }

        /// <summary>
        /// Parameter 를 삭제한다. aKey: 0 Factory / 1 Device / 2 공정.
        /// 처리 흐름: 전부 등록된 Parameter 인지 / 측정값이 없는지 먼저 확인(등록 목록 1회 + 값 수 1회 조회, 하나라도 걸리면 아무것도 지우지 않음) ->
        ///           같은 키의 Spec 삭제(FK ON DELETE CASCADE 로도 지워지지만 순서를 드러내려고 먼저 지운다) -> Parameter 삭제. 삭제는 각각 배열 DELETE 1회.
        ///           반환: 지운 Parameter 수 (2026-10-14 Parameter 마다 DB 를 3번 왕복하던 것을 묶음 처리로: 2,755개 = 약 8,300번 -> 4번)
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
            HashSet<string> setExist = RegisteredNames(oItem, f, d, t);
            Dictionary<string, long> dicValues = new Dictionary<string, long>(StringComparer.Ordinal);
            using (DataTable dtCount = oItem.GetData("SELECT_ET_PARA_VALUE_COUNTS", null, new string[] { f, d, t }))
            {
                foreach (DataRow dr in dtCount.Rows)
                    dicValues[dr["ITEM_NAME"].ToString()] = Convert.ToInt64(dr["VALUE_COUNT"], CultureInfo.InvariantCulture);
            }

            List<string> lstItem = new List<string>();
            HashSet<string> setItem = new HashSet<string>(StringComparer.Ordinal);
            List<string> lstError = new List<string>();
            foreach (string strValue in aParameters)
            {
                string strItem = RequiredParameterName(strValue);
                if (!setItem.Add(strItem))
                    continue;
                long lValues;
                if (!setExist.Contains(strItem))
                    lstError.Add("등록되지 않은 Parameter: " + strItem);
                else if (dicValues.TryGetValue(strItem, out lValues) && lValues > 0)
                    lstError.Add("측정값 " + lValues + "개가 저장된 Parameter(사용 안 함으로 바꾸세요): " + strItem);
                else
                    lstItem.Add(strItem);
            }
            if (lstError.Count > 0)
                throw new InvalidOperationException("삭제할 수 없습니다(" + lstError.Count + "건). " + string.Join(" / ", lstError.Take(10).ToArray()));

            string[,] aDelete = new string[lstItem.Count, 4];
            for (int i = 0; i < lstItem.Count; i++)
            {
                aDelete[i, 0] = f;
                aDelete[i, 1] = d;
                aDelete[i, 2] = t;
                aDelete[i, 3] = lstItem[i];
            }
            new TQP_GDS_ET_LIMIT().DeleteExecuteMultiple("DELETE_ET_LIMIT", aDelete);
            oItem.DeleteExecuteMultiple("DELETE_ET_PARA", aDelete);
            return lstItem.Count;
        }

        /// <summary>Device + 공정의 등록 Parameter 이름(저장 이름) 집합. 한 번 조회로 존재 여부를 확인할 때 쓴다.</summary>
        private static HashSet<string> RegisteredNames(TQP_GDS_ET_ITEM oItem, string f, string d, string t)
        {
            HashSet<string> setName = new HashSet<string>(StringComparer.Ordinal);
            using (DataTable dtExist = oItem.GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { f, d, t }))
            {
                foreach (DataRow dr in dtExist.Rows)
                    setName.Add(dr["ITEM_NAME"].ToString());
            }
            return setName;
        }

        /// <summary>
        /// E-Test Device 의 GDS Device: 이름이 같은(대소문자 무시) READY Map, 없으면 E-Test Device 이름 그대로.
        /// Sub Device 연결은 2026-10-15 제거했다. 이름이 다르면 GDS Map 조회 화면 [Map 복사]로 E-Test Device 이름의 Map 을 만든다.
        /// </summary>
        private static string ResolveGdsDevice(string strFactory, string strDeviceID)
        {
            using (DataTable dt = new TQP_GDS_MAP().GetData("SELECT_MAP_BY_NAME", null, new string[] { strFactory, strDeviceID }))
            {
                foreach (DataRow dr in dt.Rows)
                    if (dr["MAP_STATUS"].ToString() == "READY") return dr["DEVICE_ID"].ToString();
            }
            return strDeviceID;
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

        /// <summary>
        /// Spec 의 Parameter: 헤더 원문('@' 포함)이면 EtParameterNameRule 로 저장 이름을 만들고, 아니면 저장 이름으로 보고 검사한다.
        /// 고객 Spec 표(헤더 원문)와 Spec 화면(저장 이름)이 같은 키가 되게 한다 (2026-10-14).
        /// </summary>
        private static string RequiredSpecParameterName(string strValue)
        {
            if (Clean(strValue).IndexOf('@') < 0)
                return RequiredParameterName(strValue);
            EtParameterParts parts = EtParameterNameRule.Parse(strValue);
            if (!parts.IsValid)
                throw new ArgumentException("Parameter 헤더를 해석할 수 없습니다: " + parts.Error + " [" + strValue + "]", "Parameter");
            return parts.ParameterName;
        }

        #endregion

        #region [GDS Chain 참고 목록 / 인계 API]

        /// <summary>E-Test Device 의 GDS Device (같은 이름 READY Map -> 그대로). 화면 GDS Device 콤보 기본값.</summary>
        public string GetEtGdsDevice(string strFactory, string strDeviceID)
        {
            return ResolveGdsDevice(Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50));
        }

        /// <summary>GDS Device 의 Setup Chain 목록(ACTIVE + 현재 READY 버전). [등록 Parameter] 탭 오른쪽 목록(이름 복사용).</summary>
        public DataTable GetEtGdsChainList(string strFactory, string strGdsDeviceID)
        {
            return new TQP_GDS_ET_ITEM().GetData("SELECT_ET_GDS_SETUP_CHAIN", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strGdsDeviceID, "GDS Device", 50) });
        }

        /// <summary>
        /// Setup Chain(GDS Chain 이름)의 E-Test Parameter / Spec 을 찾는다. 인계 API 라 이름 / 인자는 2026-10-12 그대로 둔다.
        /// 2026-10-14 Para 화면 재구성: Device -> Parameter 의 GDS Chain(Chain 1, 사용자 입력)이 같은(대소문자 무시) 모든 공정 등록 Parameter + Spec.
        /// 2026-10-15 Sub Device 연결 제거: GDS Device 와 E-Test Device 는 이름이 같으므로(대소문자 무시) 그 Device 의 Parameter 만 찾는다. 쿼리 순서: factory device gds
        /// </summary>
        public DataTable GetEtChainSpecInfo(string strFactory, string strDeviceID, string strSetupChain)
        {
            string f = Required(strFactory, "Factory", 30);
            string g = ResolveGdsDevice(f, Required(strDeviceID, "Device ID", 50));
            string strChain = RequiredChars(strSetupChain, "Setup Chain", 200);
            DataTable dt = new TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_BY_GDS_CHAIN", null, new string[] { f, g, strChain });
            dt.TableName = "ET_CHAIN_SPEC_INFO";
            return dt;
        }

        #endregion
    }
}
