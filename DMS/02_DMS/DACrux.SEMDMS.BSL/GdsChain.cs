using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS Chain 저장 / 조회 / 삭제의 업무 검증과 DSL 호출을 담당한다.
    /// 설계: 문서/설계/02_GDS/2026-10-06_GDS_Chain_DB저장_설계.md 10.1절
    /// 2026-10-06 결정: 버전 이력 없음(VERSION_NO = 1, 다시 저장하면 이전 내용을 지우고 새로 저장) / 삭제는 실제 삭제.
    /// 기존 GdsMapImport와 같이 문장마다 자동 Commit이다. 저장 도중 끊기면 VERSION_STATUS = DRAFT로 남고,
    /// 화면은 이를 "저장 미완료"로 보여 주며 다시 저장하면 처음부터 저장한다.
    /// </summary>
    public class GdsChain : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsChain
    {
        /// <summary>한 번에 받을 수 있는 배치 행 수. GdsMapImport와 같은 크기다.</summary>
        private const int MaxBatchRows = 10000;

        public GdsChain()
        {
        }

        #region [TQP_GDS_CHAIN]

        /// <summary>Device의 Chain 목록(저장 상태 / Member 수 / 편집 수 포함)을 조회한다.</summary>
        public DataTable GetChainList(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_CHAIN oTQP_GDS_CHAIN = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN();
            return oTQP_GDS_CHAIN.GetData("SELECT_CHAIN_LIST", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        /// <summary>Chain 1개를 실제로 삭제한다. 하위 VER / MEMBER / LAYER / RULE / EDIT는 FK CASCADE로 함께 지워진다.</summary>
        public int DeleteChain(string strFactory, string strDeviceID, string strChainSeq, string strUpdateUser)
        {
            Required(strUpdateUser, "Update User", 30);
            DACrux.SEMDMS.DSL.TQP_GDS_CHAIN oTQP_GDS_CHAIN = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN();
            return oTQP_GDS_CHAIN.DeleteDataNonQuery("DELETE_CHAIN_ONE", null, ChainKey(strFactory, strDeviceID, strChainSeq));
        }

        #endregion

        #region [TQP_GDS_CHAIN_VER]

        /// <summary>
        /// Chain 저장 시작.
        /// 처리 흐름:
        ///  1) 입력 검증(Info 9개 / Layer 1개 이상 / 규칙 Layer 순서 정리)
        ///  2) Device Map이 READY인지 확인(Chain 도형은 저장 완료된 Map의 PLACED만 참조할 수 있다)
        ///  3) 신규: Code 중복 확인 -> 시퀀스 채번 -> CHAIN 생성 / 기존: Chain 확인(Code가 같아야 함) -> 수정 일시 갱신 -> 이전 저장 내용 삭제
        ///  4) VERSION 1(DRAFT) 생성 -> Layer / 규칙 저장
        /// 반환 1행: CHAIN_SEQ / CHAIN_CODE
        /// </summary>
        public DataTable BeginChainSave(string[] ChainInfo, string[,] aLayers, string[,] aRules)
        {
            if (ChainInfo == null || ChainInfo.Length != 9)
                throw new ArgumentException("Chain 저장 정보는 9개 항목이어야 합니다.", "ChainInfo");
            string strFactory = Required(ChainInfo[0], "Factory", 30);
            string strDevice = Required(ChainInfo[1], "Device ID", 50);
            string strChainSeq = ChainInfo[2] == null ? string.Empty : ChainInfo[2].Trim();
            string strCode = Required(ChainInfo[3], "Chain Code", 32);
            string strName = Required(ChainInfo[4], "Chain 이름", 800);
            if (strName.Length > 200)
                throw new ArgumentException("Chain 이름은 200자 이하로 입력하세요.", "ChainInfo");
            int iColor = ParseInt(ChainInfo[5], "색상");
            int iApplyRules = ParseInt(ChainInfo[6], "Layer 규칙 적용");
            if (iApplyRules != 0 && iApplyRules != 1)
                throw new ArgumentException("Layer 규칙 적용 값은 0 또는 1이어야 합니다.", "ChainInfo");
            decimal dCellSize;
            if (!decimal.TryParse(ChainInfo[7], NumberStyles.Number, CultureInfo.InvariantCulture, out dCellSize) || dCellSize <= 0)
                throw new ArgumentException("격자 크기는 0보다 큰 숫자여야 합니다.", "ChainInfo");
            string strUser = Required(ChainInfo[8], "User", 30);

            // Layer 는 번호 + DataType 단위다 (2026-10-20). 예전 열 수(DataType 없음)로 부르면 DataType 0 으로 저장한다.
            if (aLayers == null || aLayers.GetLength(0) == 0 || (aLayers.GetLength(1) != 1 && aLayers.GetLength(1) != 2))
                throw new ArgumentException("탐색 Layer는 1개 이상, 열은 LAYER_ID / DATA_TYPE 2개(또는 LAYER_ID 1개)여야 합니다.", "aLayers");
            if (aRules != null && aRules.GetLength(0) > 0 && aRules.GetLength(1) != 3 && aRules.GetLength(1) != 5)
                throw new ArgumentException("Layer 규칙 열은 FIRST_LAYER_ID / FIRST_DATA_TYPE / SECOND_LAYER_ID / SECOND_DATA_TYPE / TOLERANCE 5개"
                    + "(또는 FIRST_LAYER_ID / SECOND_LAYER_ID / TOLERANCE 3개)여야 합니다.", "aRules");

            CheckMapReady(strFactory, strDevice);

            DACrux.SEMDMS.DSL.TQP_GDS_CHAIN oTQP_GDS_CHAIN = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN();
            DataTable dt = null;
            try
            {
                if (strChainSeq.Length == 0)
                {
                    dt = oTQP_GDS_CHAIN.GetData("SELECT_CHAIN_BY_CODE", null, new string[] { strFactory, strDevice, strCode });
                    if (dt.Rows.Count > 0)
                        throw new InvalidOperationException("Chain Code " + strCode + "가 이미 있습니다. 불러온 뒤 저장하거나 다른 Code를 사용하세요.");
                    dt.Dispose();
                    dt = oTQP_GDS_CHAIN.GetData("SELECT_NEXT_CHAIN_SEQ", null, null);
                    strChainSeq = dt.Rows[0][0].ToString();
                    if (oTQP_GDS_CHAIN.InsertDataNonQuery("CREATE_CHAIN", null, new string[] { strFactory, strDevice, strChainSeq, strCode, strUser }) != 1)
                        throw new DataException("Chain이 한 건 생성되지 않았습니다.");
                }
                else
                {
                    strChainSeq = RequiredSeq(strChainSeq, "Chain 번호");
                    dt = oTQP_GDS_CHAIN.GetData("SELECT_CHAIN", null, new string[] { strFactory, strDevice, strChainSeq });
                    if (dt.Rows.Count == 0)
                        throw new InvalidOperationException("Chain을 찾을 수 없습니다(다른 사용자가 삭제했을 수 있습니다): " + strCode);
                    if (!string.Equals(dt.Rows[0]["CHAIN_CODE"].ToString(), strCode, StringComparison.Ordinal))
                        throw new InvalidOperationException("Chain 번호 " + strChainSeq + "의 Code가 " + dt.Rows[0]["CHAIN_CODE"] + "입니다. Code는 바꿀 수 없습니다.");
                    oTQP_GDS_CHAIN.UpdateDataNonQuery("UPDATE_CHAIN", null, new string[] { strUser, strFactory, strDevice, strChainSeq });
                    // 덮어쓰기: 이전 저장 내용(VER -> MEMBER / LAYER / RULE / EDIT CASCADE)을 지운다.
                    new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER().DeleteDataNonQuery("DELETE_CHAIN_VER", null, new string[] { strFactory, strDevice, strChainSeq });
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER oTQP_GDS_CHAIN_VER = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER();
            if (oTQP_GDS_CHAIN_VER.InsertDataNonQuery("CREATE_CHAIN_VER", null, new string[] {
                    strFactory, strDevice, strChainSeq, strName, iColor.ToString(CultureInfo.InvariantCulture),
                    iApplyRules.ToString(CultureInfo.InvariantCulture), dCellSize.ToString(CultureInfo.InvariantCulture), strUser }) != 1)
                throw new DataException("Chain 저장 내용이 한 건 생성되지 않았습니다.");

            int iLayers = aLayers.GetLength(0);
            bool bLayerDataType = aLayers.GetLength(1) == 2;
            string[,] aLayer = new string[iLayers, 5];
            for (int i = 0; i < iLayers; i++)
            {
                aLayer[i, 0] = strFactory;
                aLayer[i, 1] = strDevice;
                aLayer[i, 2] = strChainSeq;
                aLayer[i, 3] = ParseInt(aLayers[i, 0], "LAYER_ID").ToString(CultureInfo.InvariantCulture);
                aLayer[i, 4] = (bLayerDataType ? ParseDataType(aLayers[i, 1], "DATA_TYPE") : 0).ToString(CultureInfo.InvariantCulture);
            }
            new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_LAYER().InsertExecuteMultiple("CREATE_CHAIN_LAYER_DT", aLayer);

            if (aRules != null && aRules.GetLength(0) > 0)
            {
                int iRules = aRules.GetLength(0);
                bool bRuleDataType = aRules.GetLength(1) == 5;
                string[,] aRule = new string[iRules, 8];
                for (int i = 0; i < iRules; i++)
                {
                    int iFirst = ParseInt(aRules[i, 0], "FIRST_LAYER_ID");
                    int iFirstType = bRuleDataType ? ParseDataType(aRules[i, 1], "FIRST_DATA_TYPE") : 0;
                    int iSecond = ParseInt(aRules[i, bRuleDataType ? 2 : 1], "SECOND_LAYER_ID");
                    int iSecondType = bRuleDataType ? ParseDataType(aRules[i, 3], "SECOND_DATA_TYPE") : 0;
                    double dTolerance = ParseDouble(aRules[i, bRuleDataType ? 4 : 2], "TOLERANCE");
                    if (dTolerance < 0)
                        throw new ArgumentException("허용 오차는 0 이상이어야 합니다.", "aRules");
                    // DB CHECK(FIRST_LAYER_ID <= SECOND_LAYER_ID)에 맞춰 (번호, DataType)이 작은 쪽을 앞에 둔다.
                    if (iFirst > iSecond || (iFirst == iSecond && iFirstType > iSecondType))
                    {
                        int iSwap = iFirst; iFirst = iSecond; iSecond = iSwap;
                        iSwap = iFirstType; iFirstType = iSecondType; iSecondType = iSwap;
                    }
                    aRule[i, 0] = strFactory;
                    aRule[i, 1] = strDevice;
                    aRule[i, 2] = strChainSeq;
                    aRule[i, 3] = iFirst.ToString(CultureInfo.InvariantCulture);
                    aRule[i, 4] = iFirstType.ToString(CultureInfo.InvariantCulture);
                    aRule[i, 5] = iSecond.ToString(CultureInfo.InvariantCulture);
                    aRule[i, 6] = iSecondType.ToString(CultureInfo.InvariantCulture);
                    aRule[i, 7] = dTolerance.ToString("G17", CultureInfo.InvariantCulture);
                }
                new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_RULE().InsertExecuteMultiple("CREATE_CHAIN_RULE_DT", aRule);
            }

            DataTable dtResult = new DataTable("CHAIN_SAVE");
            dtResult.Columns.Add("CHAIN_SEQ", typeof(string));
            dtResult.Columns.Add("CHAIN_CODE", typeof(string));
            dtResult.Rows.Add(strChainSeq, strCode);
            return dtResult;
        }

        /// <summary>
        /// Chain 저장 확정.
        /// 처리 흐름: 저장 내용 확인(이미 READY면 그대로 OK) -> DB의 Member 수 / INPUT 수 / OUTPUT 수 / 편집 수를 센다
        ///  -> 예상 건수와 같고 INPUT 1개 / OUTPUT 0~1개면 READY. 아니면 DRAFT 그대로 MISMATCH 반환.
        /// </summary>
        public DataTable CompleteChainSave(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 5)
                throw new ArgumentException("확정 키는 Factory / Device / Chain 번호 / 예상 Member 수 / 예상 편집 수 5개여야 합니다.", "aKey");
            string[] aChainKey = ChainKey(aKey[0], aKey[1], aKey[2]);
            long lExpectedMember = ParseCount(aKey[3], "예상 Member 수");
            long lExpectedEdit = ParseCount(aKey[4], "예상 편집 수");
            string strUser = Required(strUpdateUser, "Update User", 30);

            string strStatus = GetVersionStatus(aChainKey);
            DataTable dtMember = null;
            DataTable dtEdit = null;
            long lMember, lInput, lOutput, lEdit;
            try
            {
                dtMember = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER_COUNT", null, aChainKey);
                dtEdit = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_EDIT().GetData("SELECT_CHAIN_EDIT_COUNT", null, aChainKey);
                lMember = DACrux.Base.Convert.longParse(dtMember.Rows[0]["MEMBER_COUNT"].ToString());
                lInput = DACrux.Base.Convert.longParse(dtMember.Rows[0]["INPUT_COUNT"].ToString());
                lOutput = DACrux.Base.Convert.longParse(dtMember.Rows[0]["OUTPUT_COUNT"].ToString());
                lEdit = DACrux.Base.Convert.longParse(dtEdit.Rows[0]["EDIT_COUNT"].ToString());
            }
            finally
            {
                if (dtMember != null) dtMember.Dispose();
                if (dtEdit != null) dtEdit.Dispose();
            }

            if (string.Equals(strStatus, "READY", StringComparison.Ordinal))
                return CompleteResult("OK", strStatus, lExpectedMember, lMember, lExpectedEdit, lEdit, lInput, lOutput, "이미 확정된 Chain입니다.");

            var aProblem = new List<string>();
            if (lMember != lExpectedMember) aProblem.Add("Member " + lMember + " / 예상 " + lExpectedMember);
            if (lEdit != lExpectedEdit) aProblem.Add("편집 기록 " + lEdit + " / 예상 " + lExpectedEdit);
            if (lInput != 1) aProblem.Add("INPUT " + lInput + "개(1개여야 함)");
            if (lOutput > 1) aProblem.Add("OUTPUT " + lOutput + "개(0~1개여야 함)");
            if (aProblem.Count > 0)
                return CompleteResult("MISMATCH", strStatus, lExpectedMember, lMember, lExpectedEdit, lEdit, lInput, lOutput,
                    "저장 확정 불가: " + string.Join(" / ", aProblem.ToArray()));

            new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER().UpdateDataNonQuery("UPDATE_CHAIN_VER_READY", null,
                new string[] { strUser, aChainKey[0], aChainKey[1], aChainKey[2] });
            return CompleteResult("OK", GetVersionStatus(aChainKey), lExpectedMember, lMember, lExpectedEdit, lEdit, lInput, lOutput, "저장 완료");
        }

        /// <summary>CompleteChainSave 반환 1행을 만든다.</summary>
        private static DataTable CompleteResult(string strResult, string strStatus, long lExpectedMember, long lDbMember,
            long lExpectedEdit, long lDbEdit, long lInput, long lOutput, string strMessage)
        {
            DataTable dt = new DataTable("COMPLETE_RESULT");
            dt.Columns.Add("RESULT", typeof(string));
            dt.Columns.Add("VERSION_STATUS", typeof(string));
            dt.Columns.Add("EXPECTED_MEMBER", typeof(long));
            dt.Columns.Add("DB_MEMBER", typeof(long));
            dt.Columns.Add("EXPECTED_EDIT", typeof(long));
            dt.Columns.Add("DB_EDIT", typeof(long));
            dt.Columns.Add("INPUT_COUNT", typeof(long));
            dt.Columns.Add("OUTPUT_COUNT", typeof(long));
            dt.Columns.Add("MESSAGE", typeof(string));
            dt.Rows.Add(strResult, strStatus, lExpectedMember, lDbMember, lExpectedEdit, lDbEdit, lInput, lOutput, strMessage);
            return dt;
        }

        /// <summary>Chain 저장 상태(DRAFT / READY)를 돌려준다. 저장 내용이 없으면 오류다.</summary>
        private string GetVersionStatus(string[] aChainKey)
        {
            DataTable dt = null;
            try
            {
                dt = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER().GetData("SELECT_CHAIN_VER", null, aChainKey);
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("Chain 저장 내용을 찾을 수 없습니다. BeginChainSave부터 다시 하세요: " + aChainKey[2]);
                return dt.Rows[0]["VERSION_STATUS"].ToString();
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>Device Map이 저장 완료(READY)인지 확인한다. Chain 도형은 READY Map의 PLACED만 참조한다.</summary>
        private void CheckMapReady(string strFactory, string strDevice)
        {
            DataTable dt = null;
            try
            {
                dt = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new string[] { strFactory, strDevice });
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("Factory " + strFactory + " / Device " + strDevice + "에 저장된 GDS Map이 없습니다. 먼저 Map을 DB에 저장하세요.");
                if (!string.Equals(dt.Rows[0]["MAP_STATUS"].ToString(), "READY", StringComparison.Ordinal))
                    throw new InvalidOperationException("GDS Map 저장이 끝나지 않았습니다(상태 " + dt.Rows[0]["MAP_STATUS"] + "). Map 저장을 마친 뒤 Chain을 저장하세요.");
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        #endregion

        #region [TQP_GDS_CHAIN_MEMBER]

        /// <summary>
        /// Chain 포함 도형 배치를 저장한다(InsertExecuteMultiple 1회, 호출 단위 원자적).
        /// 처리 흐름: 키 확인 -> 저장 상태가 DRAFT인지 확인 -> 배열 크기 확인 -> 행 저장. 건수는 CompleteChainSave가 DB에서 확인한다.
        /// </summary>
        public int CreateChainMemberBatch(string[] aKey, string[,] aParas, string strUpdateUser)
        {
            string[] aChainKey = DraftChainKey(aKey);
            CheckBatchParas(aParas, 5);
            int iRows = aParas.GetLength(0);
            string[,] aMember = new string[iRows, 8];
            for (int i = 0; i < iRows; i++)
            {
                aMember[i, 0] = aChainKey[0];
                aMember[i, 1] = aChainKey[1];
                aMember[i, 2] = aChainKey[2];
                aMember[i, 3] = RequiredId(aParas[i, 0]);
                aMember[i, 4] = Required(aParas[i, 1], "ROLE_CODE", 10);
                aMember[i, 5] = Required(aParas[i, 2], "INCLUSION_SOURCE", 10);
                aMember[i, 6] = OptionalDouble(aParas[i, 3], "MARKER_X");
                aMember[i, 7] = OptionalDouble(aParas[i, 4], "MARKER_Y");
            }
            new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().InsertExecuteMultiple("CREATE_CHAIN_MEMBER", aMember);
            return iRows;
        }

        #endregion

        #region [TQP_GDS_CHAIN_EDIT]

        /// <summary>Chain 수동 편집 기록 배치를 저장한다. 처리 흐름은 CreateChainMemberBatch와 같다.</summary>
        public int CreateChainEditBatch(string[] aKey, string[,] aParas, string strUpdateUser)
        {
            string[] aChainKey = DraftChainKey(aKey);
            string strUser = Required(strUpdateUser, "Update User", 30);
            CheckBatchParas(aParas, 4);
            int iRows = aParas.GetLength(0);
            string[,] aEdit = new string[iRows, 8];
            for (int i = 0; i < iRows; i++)
            {
                aEdit[i, 0] = aChainKey[0];
                aEdit[i, 1] = aChainKey[1];
                aEdit[i, 2] = aChainKey[2];
                aEdit[i, 3] = RequiredSeq(aParas[i, 0], "EDIT_SEQ");
                aEdit[i, 4] = RequiredId(aParas[i, 1]);
                aEdit[i, 5] = Required(aParas[i, 2], "ACTION_CODE", 10);
                aEdit[i, 6] = string.IsNullOrEmpty(aParas[i, 3]) ? string.Empty : RequiredSeq(aParas[i, 3], "BATCH_EDIT_SEQ");
                aEdit[i, 7] = strUser;
            }
            new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_EDIT().InsertExecuteMultiple("CREATE_CHAIN_EDIT", aEdit);
            return iRows;
        }

        #endregion

        #region [Chain 상세 조회]

        /// <summary>Chain 탐색 Layer 조회. 결과: LAYER_ID / DATA_TYPE (2026-10-20 DataType 추가).</summary>
        public DataTable GetChainLayers(string strFactory, string strDeviceID, string strChainSeq)
        {
            return new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_LAYER().GetData("SELECT_CHAIN_LAYER_DT", null, ChainKey(strFactory, strDeviceID, strChainSeq));
        }

        /// <summary>Chain Layer 연결 규칙 조회. 결과에 FIRST_DATA_TYPE / SECOND_DATA_TYPE 이 있다 (2026-10-20).</summary>
        public DataTable GetChainRules(string strFactory, string strDeviceID, string strChainSeq)
        {
            return new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_RULE().GetData("SELECT_CHAIN_RULE_DT", null, ChainKey(strFactory, strDeviceID, strChainSeq));
        }

        public DataTable GetChainMembers(string strFactory, string strDeviceID, string strChainSeq)
        {
            return new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER", null, ChainKey(strFactory, strDeviceID, strChainSeq));
        }

        public DataTable GetChainEdits(string strFactory, string strDeviceID, string strChainSeq)
        {
            return new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_EDIT().GetData("SELECT_CHAIN_EDIT", null, ChainKey(strFactory, strDeviceID, strChainSeq));
        }

        #endregion

        #region [Validation]

        /// <summary>Factory / Device / Chain 번호를 검증해 DSL 키 배열로 만든다.</summary>
        private static string[] ChainKey(string strFactory, string strDeviceID, string strChainSeq)
        {
            return new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50), RequiredSeq(strChainSeq, "Chain 번호") };
        }

        /// <summary>배치 키(Factory / Device / Chain 번호)를 검증하고, 저장 상태가 DRAFT(저장 중)인지 확인한다.</summary>
        private string[] DraftChainKey(string[] aKey)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("배치 키는 Factory / Device / Chain 번호 3개여야 합니다.", "aKey");
            string[] aChainKey = ChainKey(aKey[0], aKey[1], aKey[2]);
            string strStatus = GetVersionStatus(aChainKey);
            if (!string.Equals(strStatus, "DRAFT", StringComparison.Ordinal))
                throw new InvalidOperationException("Chain이 " + strStatus + " 상태라서 배치를 받을 수 없습니다. BeginChainSave부터 다시 하세요.");
            return aChainKey;
        }

        /// <summary>행 수 / 열 수를 확인한다.</summary>
        private static void CheckBatchParas(string[,] aParas, int iColumns)
        {
            if (aParas == null || aParas.GetLength(0) == 0)
                throw new ArgumentException("배치에 행이 없습니다.", "aParas");
            if (aParas.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("배치 행 수는 " + MaxBatchRows + "건 이하여야 합니다.", "aParas");
            if (aParas.GetLength(1) != iColumns)
                throw new ArgumentException("배치 열 수는 " + iColumns + "개여야 합니다.", "aParas");
        }

        /// <summary>빈 값이면 빈 문자열(DB NULL), 아니면 G17 double 문자열로 정리한다.</summary>
        private static string OptionalDouble(string strValue, string strName)
        {
            if (string.IsNullOrEmpty(strValue) || strValue.Trim().Length == 0)
                return string.Empty;
            return ParseDouble(strValue, strName).ToString("G17", CultureInfo.InvariantCulture);
        }

        private static double ParseDouble(string strValue, string strName)
        {
            double dValue;
            if (!double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) || double.IsNaN(dValue) || double.IsInfinity(dValue))
                throw new ArgumentException(strName + " 값이 숫자가 아닙니다: " + strValue, strName);
            return dValue;
        }

        private static int ParseInt(string strValue, string strName)
        {
            int iValue;
            if (!int.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out iValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return iValue;
        }

        /// <summary>GDS DataType / TextType(0 ~ 65535)을 확인한다 (2026-10-20).</summary>
        private static int ParseDataType(string strValue, string strName)
        {
            int iValue = ParseInt(strValue, strName);
            if (iValue < 0 || iValue > 65535)
                throw new ArgumentException(strName + " 값은 0 ~ 65535 사이여야 합니다: " + strValue, strName);
            return iValue;
        }

        private static long ParseCount(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue < 0)
                throw new ArgumentException(strName + "는 0 이상의 정수여야 합니다.", strName);
            return lValue;
        }

        private static string RequiredSeq(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(strValue == null ? null : strValue.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue <= 0)
                throw new ArgumentException(strName + "는 1 이상의 정수여야 합니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>배치 도형 ID는 화면 / PLACED_EL과 정확히 같아야 하므로 공백을 정리하지 않고 길이만 확인한다.</summary>
        private static string RequiredId(string strValue)
        {
            int iLength = strValue == null ? 0 : Encoding.UTF8.GetByteCount(strValue);
            if (iLength == 0 || iLength > 1024)
                throw new ArgumentException("PLACED_ELEMENT_ID는 1~1024바이트여야 합니다.", "aParas");
            return strValue;
        }

        /// <summary>Oracle 컬럼 바이트 길이에 맞는 필수 문자열인지 확인한다.</summary>
        private static string Required(string strValue, string strName, int iMaxBytes)
        {
            string strResult = strValue == null ? string.Empty : strValue.Trim();
            int iLength = Encoding.UTF8.GetByteCount(strResult);
            if (iLength == 0 || iLength > iMaxBytes)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxBytes + "바이트로 입력하세요.", strName);
            return strResult;
        }

        #endregion
    }
}
