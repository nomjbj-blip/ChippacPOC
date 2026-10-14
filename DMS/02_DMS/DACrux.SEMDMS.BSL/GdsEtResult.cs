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
    /// E-Test 결과 파일 저장의 업무 검증과 DSL 호출을 담당한다.
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절
    /// 파일 Parsing은 클라이언트가 하고, 서버는 받은 행을 검증해 DefectDefine.CreateDefect와 같은 Bulk Insert(InsertExecuteMultiple)로 저장한다.
    /// 문장마다 자동 Commit이다. 중간에 끊기면 헤더가 LOADING으로 남고, 다음 저장 때 화면이 삭제 여부를 묻는다.
    /// </summary>
    public partial class GdsEtResult : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsEtResult
    {
        public GdsEtResult()
        {
        }

        #region [TQP_GDS_SRC_FILE]

        /// <summary>
        /// 같은 Wafer 파일이 있는지 조회한다. 없으면 0행이다.
        /// aKey: 0 Factory / 1 Lot ID / 2 Slot 번호 [/ 3 공정(Test Type)].
        ///  - 4항목: UNIQUE 키(Factory + WAFER_ID + 공정)로 1건 조회. TSVET -> COWET -> FT 가 공정이라 Wafer 와 공정을 조합해 키로 쓴다 (2026-10-12).
        ///  - 3항목: 공정과 관계없이 그 Wafer 의 파일 전체(TSVET / COWET / FT). 정리 / 이전 호출 호환용.
        /// Device / 시험 일시는 키가 아니다 (2026-10-07).
        /// </summary>
        public DataTable GetEtFileByKey(string[] aKey)
        {
            if (aKey == null || (aKey.Length != 3 && aKey.Length != 4))
                throw new ArgumentException("파일 키는 3개(Factory / Lot / Slot) 또는 4개(+ 공정) 항목이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strWafer = WaferId(Required(aKey[1], "Lot ID", 50), RequiredSlot(aKey[2]));
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            if (aKey.Length == 4)
                return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BY_WAFER_TEST", null,
                    new string[] { strFactory, strWafer, Required(aKey[3], "공정(Test Type)", 50) });
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BY_WAFER", null, new string[] { strFactory, strWafer });
        }

        /// <summary>WAFER_ID 가상 컬럼과 같은 규칙(Lot + '-' + 2자리 Slot)으로 만든다.</summary>
        private static string WaferId(string strLot, string strSlot)
        {
            return strLot + "-" + int.Parse(strSlot, CultureInfo.InvariantCulture).ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>파일 1건을 조회한다. 없으면 0행이다.</summary>
        public DataTable GetEtFile(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        /// <summary>Device의 파일 목록을 최근 시험 순으로 조회한다.</summary>
        public DataTable GetEtFileList(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_LIST", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        /// <summary>
        /// 파일 1건을 실제로 삭제한다(기존 데이터 삭제 후 재등록).
        /// 처리 순서: 측정값 -> Die -> 헤더. FK가 CASCADE이지만 부모 행마다 내부 DELETE가 도는 것을 피하려고 자식부터 직접 지운다(GdsMapImport.DeleteMap과 같은 이유).
        /// 측정 항목(TQP_GDS_ET_ITEM)은 Device 단위라 지우지 않는다. 반환: 지운 측정값 수
        /// </summary>
        public int DeleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            string[] aKey = new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") };
            Required(strUpdateUser, "Update User", 30);
            int iValues = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().DeleteDataNonQuery("DELETE_ET_VALUE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().DeleteDataNonQuery("DELETE_ET_DIE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().DeleteDataNonQuery("DELETE_SRC_FILE", null, aKey);
            return iValues;
        }

        /// <summary>
        /// 저장을 시작한다.
        /// FileInfo 순서: 0 Factory / 1 Device ID / 2 파일 이름 / 3 Lot ID / 4 Slot 번호 / 5 시험 프로그램 / 6 시험 일시(yyyyMMddHHmmss) /
        ///                7 파일 접두어(선택) / 8 시트 이름 / 9 Die 수 / 10 항목 수 / 11 User
        /// [12 Customer / 13 File Device / 14 Wafer Type / 15 Wafer Rev / 16 Lot-Wafer No / 17 Fab(선택)] - 2026-10-12 파일명 8칸 규칙(PARSE_RULE_VER 2).
        ///   18항목이면 CREATE_SRC_FILE_V2 로 저장하고 7 파일 접두어는 쓰지 않는다. 12항목으로 부르면 이전 방식(PARSE_RULE_VER 1, 배포된 화면 / Probe 호환).
        /// Slot 번호는 파일에 없어 화면에서 사용자가 입력한다 (2026-10-07).
        /// 처리: 입력 검증 -> 같은 Wafer 파일 확인(Factory + WAFER_ID, 있으면 거부, 화면이 먼저 묻고 DeleteEtFile 호출) -> 번호 채번 -> 헤더 생성(LOADING) -> 헤더 1행 반환
        /// </summary>
        public DataTable BeginEtFile(string[] FileInfo)
        {
            if (FileInfo == null || (FileInfo.Length != 12 && FileInfo.Length != 18))
                throw new ArgumentException("저장 시작 정보는 12개 또는 18개(파일명 규칙 포함) 항목이어야 합니다.", "FileInfo");
            string strFactory = Required(FileInfo[0], "Factory", 30);
            string strDevice = Required(FileInfo[1], "Device ID", 50);
            string strFileName = RequiredChars(FileInfo[2], "파일 이름", 260);
            string strLot = Required(FileInfo[3], "Lot ID", 50);
            string strSlot = RequiredSlot(FileInfo[4]);
            string strProgram = Required(FileInfo[5], "시험 프로그램", 50);
            string strTestTime = RequiredTestTime(FileInfo[6]);
            string strPrefix = OptionalChars(FileInfo[7], "파일 접두어", 100);
            string strSheet = OptionalChars(FileInfo[8], "시트 이름", 50);
            long lDieCount = ParseCount(FileInfo[9], "Die 수");
            long lItemCount = ParseCount(FileInfo[10], "항목 수");
            if (lDieCount == 0 || lItemCount == 0)
                throw new ArgumentException("Die 수와 항목 수는 1 이상이어야 합니다.", "FileInfo");
            string strUser = Required(FileInfo[11], "Create User", 30);
            bool bRuleV2 = FileInfo.Length == 18;
            string[] aRule = null;
            if (bRuleV2)
                aRule = new string[] {
                    RequiredChars(FileInfo[12], "Customer", 30), RequiredChars(FileInfo[13], "File Device", 50),
                    RequiredChars(FileInfo[14], "Wafer Type", 30), RequiredChars(FileInfo[15], "Wafer Rev", 20),
                    RequiredChars(FileInfo[16], "Lot-Wafer No", 60), OptionalChars(FileInfo[17], "Fab", 50) };

            DataTable dtExist = null;
            try
            {
                dtExist = GetEtFileByKey(new string[] { strFactory, strLot, strSlot, strProgram });
                if (dtExist.Rows.Count > 0)
                    throw new InvalidOperationException("같은 Wafer / 같은 공정 파일이 이미 있습니다(WAFER_ID " + dtExist.Rows[0]["WAFER_ID"]
                        + " / 공정 " + dtExist.Rows[0]["TEST_PROGRAM"] + " / Device " + dtExist.Rows[0]["DEVICE_ID"]
                        + " / 파일 번호 " + dtExist.Rows[0]["SRC_FILE_SEQ"] + "). 삭제한 뒤 다시 저장하세요.");
            }
            finally
            {
                if (dtExist != null)
                    dtExist.Dispose();
            }

            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            DataTable dtSeq = oTQP_GDS_SRC_FILE.GetData("SELECT_NEXT_SRC_FILE_SEQ", null, null);
            string strSrcFileSeq = dtSeq.Rows[0][0].ToString();
            dtSeq.Dispose();
            int iRows = bRuleV2
                ? oTQP_GDS_SRC_FILE.InsertDataNonQuery("CREATE_SRC_FILE_V2", null, new string[] {
                    strSrcFileSeq, strFileName, strFactory, strDevice, strLot, strSlot, strProgram, strTestTime, strSheet,
                    lDieCount.ToString(CultureInfo.InvariantCulture), lItemCount.ToString(CultureInfo.InvariantCulture),
                    aRule[0], aRule[1], aRule[2], aRule[3], aRule[4], aRule[5], strUser, strUser })
                : oTQP_GDS_SRC_FILE.InsertDataNonQuery("CREATE_SRC_FILE", null, new string[] {
                    strSrcFileSeq, strFileName, strFactory, strDevice, strLot, strSlot, strProgram, strTestTime, strPrefix, strSheet,
                    lDieCount.ToString(CultureInfo.InvariantCulture), lItemCount.ToString(CultureInfo.InvariantCulture),
                    strUser, strUser });
            if (iRows != 1)
                throw new DataException("E-Test 파일 헤더가 한 건 생성되지 않았습니다.");
            return GetEtFile(strSrcFileSeq);
        }

        /// <summary>
        /// 저장 완료 확정.
        /// 처리 흐름:
        ///  1) 헤더 조회. 이미 READY면 OK를 돌려준다.
        ///  2) DB에서 Die 수 / 측정값 수를 다시 센다. 배치 저장은 Bulk Insert만 하므로 이 DB 건수가 유일한 확인이다.
        ///  3) Die 수 = 헤더 Die 수, 측정값 수 = Die 수 x 항목 수이면 READY(VALUE_COUNT / PASS·FAIL Die 수 / 미측정 수 기록). 다르면 LOADING 그대로 두고 MISMATCH 반환.
        /// </summary>
        public DataTable CompleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            string strSeq = RequiredSeq(strSrcFileSeq, "파일 번호");
            string strUser = Required(strUpdateUser, "Update User", 30);
            DataTable dtFile = null;
            DataTable dtDie = null;
            DataTable dtValue = null;
            try
            {
                dtFile = GetEtFile(strSeq);
                if (dtFile.Rows.Count == 0)
                    throw new InvalidOperationException("E-Test 파일을 찾을 수 없습니다: " + strSeq);
                DataRow drFile = dtFile.Rows[0];
                string strStatus = drFile["LOAD_STATUS"].ToString();
                long lExpectedDie = DACrux.Base.Convert.longParse(drFile["DIE_COUNT"].ToString());
                long lExpectedValue = lExpectedDie * DACrux.Base.Convert.longParse(drFile["ITEM_COUNT"].ToString());

                string[] aKey = new string[] { strSeq };
                dtDie = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().GetData("SELECT_ET_DIE_COUNT", null, aKey);
                dtValue = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().GetData("SELECT_ET_VALUE_COUNT", null, aKey);
                long lDbDie = DACrux.Base.Convert.longParse(dtDie.Rows[0]["DIE_COUNT"].ToString());
                long lFailDie = DACrux.Base.Convert.longParse(dtDie.Rows[0]["FAIL_DIE_COUNT"].ToString());
                long lDbValue = DACrux.Base.Convert.longParse(dtValue.Rows[0]["VALUE_COUNT"].ToString());
                long lNotTested = DACrux.Base.Convert.longParse(dtValue.Rows[0]["NOT_TESTED_COUNT"].ToString());
                long lEmpty = DACrux.Base.Convert.longParse(dtValue.Rows[0]["EMPTY_COUNT"].ToString());

                if (string.Equals(strStatus, "READY", StringComparison.Ordinal))
                    return CompleteResult("OK", "READY", lExpectedDie, lDbDie, lFailDie, lExpectedValue, lDbValue, lNotTested, lEmpty, "이미 확정된 파일입니다.");

                StringBuilder sbMessage = new StringBuilder();
                if (lDbDie != lExpectedDie) sbMessage.Append("Die 건수 불일치 / ");
                if (lDbValue != lExpectedValue) sbMessage.Append("측정값 건수 불일치 / ");
                if (sbMessage.Length > 0)
                    return CompleteResult("MISMATCH", strStatus, lExpectedDie, lDbDie, lFailDie, lExpectedValue, lDbValue, lNotTested, lEmpty,
                        sbMessage.ToString().TrimEnd(' ', '/'));

                DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
                // 2026-10-12: READY 로 바꿀 때 파일 요약(Pass / Fail Die 수, 미측정 셀 수)을 함께 기록한다(등록 목록에서 바로 보이도록).
                int iRows = oTQP_GDS_SRC_FILE.UpdateDataNonQuery("UPDATE_SRC_FILE_READY_V2", null, new string[] {
                    lDbValue.ToString(CultureInfo.InvariantCulture), (lDbDie - lFailDie).ToString(CultureInfo.InvariantCulture),
                    lFailDie.ToString(CultureInfo.InvariantCulture), lNotTested.ToString(CultureInfo.InvariantCulture), strUser, strSeq });
                if (iRows != 1)
                    throw new InvalidOperationException("파일 상태가 바뀌어 확정하지 못했습니다. 다시 조회하세요.");
                return CompleteResult("OK", "READY", lExpectedDie, lDbDie, lFailDie, lExpectedValue, lDbValue, lNotTested, lEmpty, "저장이 확정되었습니다.");
            }
            finally
            {
                if (dtFile != null) dtFile.Dispose();
                if (dtDie != null) dtDie.Dispose();
                if (dtValue != null) dtValue.Dispose();
            }
        }

        /// <summary>CompleteEtFile 반환 1행을 만든다.</summary>
        private static DataTable CompleteResult(string strResult, string strStatus, long lExpectedDie, long lDbDie, long lFailDie,
            long lExpectedValue, long lDbValue, long lNotTested, long lEmpty, string strMessage)
        {
            DataTable dt = new DataTable("COMPLETE_RESULT");
            dt.Columns.Add("RESULT", typeof(string));
            dt.Columns.Add("LOAD_STATUS", typeof(string));
            dt.Columns.Add("EXPECTED_DIE", typeof(long));
            dt.Columns.Add("DB_DIE", typeof(long));
            dt.Columns.Add("FAIL_DIE", typeof(long));
            dt.Columns.Add("EXPECTED_VALUE", typeof(long));
            dt.Columns.Add("DB_VALUE", typeof(long));
            dt.Columns.Add("NOT_TESTED", typeof(long));
            dt.Columns.Add("EMPTY", typeof(long));
            dt.Columns.Add("MESSAGE", typeof(string));
            dt.Rows.Add(strResult, strStatus, lExpectedDie, lDbDie, lFailDie, lExpectedValue, lDbValue, lNotTested, lEmpty, strMessage);
            return dt;
        }

        /// <summary>파일이 있고 LOADING 상태인지 확인한 뒤 정리한 파일 번호를 돌려준다.</summary>
        private string GetLoadingFileSeq(string strSrcFileSeq)
        {
            string strSeq = RequiredSeq(strSrcFileSeq, "파일 번호");
            DataTable dt = null;
            try
            {
                dt = GetEtFile(strSeq);
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("E-Test 파일을 찾을 수 없습니다: " + strSeq);
                string strStatus = dt.Rows[0]["LOAD_STATUS"].ToString();
                if (!string.Equals(strStatus, "LOADING", StringComparison.Ordinal))
                    throw new InvalidOperationException("E-Test 파일이 " + strStatus + " 상태라서 배치를 받을 수 없습니다.");
                return strSeq;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        #endregion

        #region [TQP_GDS_ET_ITEM]

        /// <summary>
        /// 파일의 측정 항목을 Device 항목 마스터에 맞춘다.
        /// 처리 흐름:
        ///  1) 입력 검증(열 수 / 필수 값 / 파일 안 중복 헤더)
        ///  2) Device 기존 항목 조회 -> 같은 ITEM_NAME인데 단위가 다르면 거부(어느 항목인지 최대 10개 표시)
        ///  3) 없는 항목만 InsertExecuteMultiple로 추가(10,000행씩)
        ///  4) Device 전체 항목을 다시 조회해 반환(화면은 ITEM_NAME -> ET_ITEM_SEQ로 측정값 행을 만든다)
        /// </summary>
        public DataTable MergeEtItem(string[] aKey, string[,] aItems, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 2)
                throw new ArgumentException("항목 키는 2개 항목(Factory / Device)이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aItems == null || aItems.GetLength(0) == 0)
                throw new ArgumentException("측정 항목이 없습니다.", "aItems");
            if (aItems.GetLength(1) != 8)
                throw new ArgumentException("측정 항목 열 수는 8개여야 합니다.", "aItems");

            int iRows = aItems.GetLength(0);
            HashSet<string> setName = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < iRows; i++)
            {
                string strName = RequiredChars(aItems[i, 0], "ITEM_NAME", 200);
                if (!setName.Add(strName))
                    throw new ArgumentException("파일 안에 같은 측정 항목 헤더가 두 번 있습니다: " + strName, "aItems");
                RequiredChars(aItems[i, 1], "PARA_NAME", 50);
                RequiredChars(aItems[i, 3], "CHAIN_NAME", 64);
                OptionalChars(aItems[i, 2], "COND_VALUE", 20);
                OptionalChars(aItems[i, 4], "CHAIN_NAME2", 64);
                OptionalChars(aItems[i, 5], "UNIT", 10);
                OptionalNumber(aItems[i, 6], "TEST_NO");
                OptionalNumber(aItems[i, 7], "DISPLAY_ORDER");
            }

            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            Dictionary<string, string> dicUnit = new Dictionary<string, string>(StringComparer.Ordinal);
            DataTable dtExist = null;
            try
            {
                dtExist = GetEtItemList(strFactory, strDevice);
                foreach (DataRow dr in dtExist.Rows)
                    dicUnit[dr["ITEM_NAME"].ToString()] = dr["UNIT"].ToString();
            }
            finally
            {
                if (dtExist != null)
                    dtExist.Dispose();
            }

            List<int> lstNew = new List<int>();
            List<string> lstUnitDiff = new List<string>();
            for (int i = 0; i < iRows; i++)
            {
                string strName = aItems[i, 0].Trim();
                string strUnit = Clean(aItems[i, 5]);
                string strOldUnit;
                if (!dicUnit.TryGetValue(strName, out strOldUnit))
                    lstNew.Add(i);
                else if (!string.Equals(strOldUnit, strUnit, StringComparison.Ordinal))
                    lstUnitDiff.Add(strName + " (" + strOldUnit + " -> " + strUnit + ")");
            }
            if (lstUnitDiff.Count > 0)
                throw new InvalidOperationException("등록된 측정 항목과 단위가 다릅니다(" + lstUnitDiff.Count + "건): "
                    + string.Join(" / ", lstUnitDiff.Take(10).ToArray()));

            for (int iStart = 0; iStart < lstNew.Count; iStart += MaxBatchRows)
            {
                int iCount = Math.Min(MaxBatchRows, lstNew.Count - iStart);
                string[,] aInsert = new string[iCount, 12];
                for (int k = 0; k < iCount; k++)
                {
                    int i = lstNew[iStart + k];
                    aInsert[k, 0] = strFactory;
                    aInsert[k, 1] = strDevice;
                    aInsert[k, 2] = aItems[i, 0].Trim();
                    aInsert[k, 3] = aItems[i, 1].Trim();
                    aInsert[k, 4] = Clean(aItems[i, 2]);
                    aInsert[k, 5] = aItems[i, 3].Trim();
                    aInsert[k, 6] = Clean(aItems[i, 4]);
                    aInsert[k, 7] = Clean(aItems[i, 5]);
                    aInsert[k, 8] = Clean(aItems[i, 6]);
                    aInsert[k, 9] = Clean(aItems[i, 7]);
                    aInsert[k, 10] = strUser;
                    aInsert[k, 11] = strUser;
                }
                oTQP_GDS_ET_ITEM.InsertExecuteMultiple("CREATE_ET_ITEM", aInsert);
            }
            return GetEtItemList(strFactory, strDevice);
        }

        /// <summary>Device의 측정 항목 목록을 조회한다.</summary>
        public DataTable GetEtItemList(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            return oTQP_GDS_ET_ITEM.GetData("SELECT_ET_ITEM", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        #endregion

        #region [TQP_GDS_ET_DIE]

        /// <summary>
        /// Die 정보 배치를 저장한다. 처리 흐름: 파일 LOADING 확인 -> 열 수 / 필수 값 확인 -> InsertExecuteMultiple 1회.
        /// 같은 DUT 번호 / 같은 (X, Y)가 섞이면 DB 제약으로 배치 전체가 거부된다.
        /// </summary>
        public int CreateEtDieBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            string strSeq = GetLoadingFileSeq(strSrcFileSeq);
            Required(strUpdateUser, "Update User", 30);
            CheckBatchParas(aParas, 10);
            int iRows = aParas.GetLength(0);

            string[,] aDie = new string[iRows, 11];
            for (int i = 0; i < iRows; i++)
            {
                string strPass = Clean(aParas[i, 5]);
                if (strPass != "P" && strPass != "F")
                    throw new ArgumentException("PASS_FLAG는 P 또는 F여야 합니다: " + aParas[i, 5], "aParas");
                aDie[i, 0] = strSeq;
                aDie[i, 1] = ParseInt(aParas[i, 0], "DUT_NO").ToString(CultureInfo.InvariantCulture);
                aDie[i, 2] = ParseInt(aParas[i, 1], "DIE_X").ToString(CultureInfo.InvariantCulture);
                aDie[i, 3] = ParseInt(aParas[i, 2], "DIE_Y").ToString(CultureInfo.InvariantCulture);
                aDie[i, 4] = OptionalNumber(aParas[i, 3], "HW_BIN");
                aDie[i, 5] = OptionalNumber(aParas[i, 4], "SW_BIN");
                aDie[i, 6] = strPass;
                aDie[i, 7] = OptionalChars(aParas[i, 6], "FLAG_CODE", 10);
                aDie[i, 8] = OptionalNumber(aParas[i, 7], "TEST_TIME_MS");
                aDie[i, 9] = OptionalNumber(aParas[i, 8], "TESTS_EXECUTED");
                aDie[i, 10] = OptionalChars(aParas[i, 9], "HEAD_SITE", 30);
            }
            DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE oTQP_GDS_ET_DIE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE();
            oTQP_GDS_ET_DIE.InsertExecuteMultiple("CREATE_ET_DIE", aDie);
            return iRows;
        }

        /// <summary>파일의 Die 목록을 조회한다.</summary>
        public DataTable GetEtDieList(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE oTQP_GDS_ET_DIE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE();
            return oTQP_GDS_ET_DIE.GetData("SELECT_ET_DIE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        #endregion

        #region [TQP_GDS_ET_VALUE]

        /// <summary>
        /// 측정값 배치를 저장한다. 처리 흐름: 파일 LOADING 확인 -> 열 수 / 상태 값 확인 -> InsertExecuteMultiple 1회.
        /// 값은 클라이언트가 G17 문자열로 보내고 XML이 TO_BINARY_DOUBLE로 바꾼다. 미측정(N) / 빈 값(E)은 값을 빈 문자열로 보낸다(NULL 저장).
        /// 없는 Die / 항목 번호가 섞이면 FK로 배치 전체가 거부된다. 건수는 CompleteEtFile이 DB에서 확인한다.
        /// </summary>
        public int CreateEtValueBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            string strSeq = GetLoadingFileSeq(strSrcFileSeq);
            Required(strUpdateUser, "Update User", 30);
            CheckBatchParas(aParas, 4);
            int iRows = aParas.GetLength(0);

            string[,] aValue = new string[iRows, 5];
            for (int i = 0; i < iRows; i++)
            {
                string strStatus = Clean(aParas[i, 3]);
                string strValue = Clean(aParas[i, 2]);
                if (strStatus == "V")
                {
                    double dValue;
                    if (!double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) || double.IsNaN(dValue) || double.IsInfinity(dValue))
                        throw new ArgumentException("측정값이 숫자가 아닙니다(DUT " + aParas[i, 0] + " / 항목 " + aParas[i, 1] + "): " + strValue, "aParas");
                }
                else if (strStatus == "N" || strStatus == "E")
                    strValue = string.Empty;
                else
                    throw new ArgumentException("VALUE_STATUS는 V / N / E여야 합니다: " + aParas[i, 3], "aParas");
                aValue[i, 0] = strSeq;
                aValue[i, 1] = Clean(aParas[i, 0]);
                aValue[i, 2] = Clean(aParas[i, 1]);
                aValue[i, 3] = strValue;
                aValue[i, 4] = strStatus;
            }
            DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE oTQP_GDS_ET_VALUE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE();
            oTQP_GDS_ET_VALUE.InsertExecuteMultiple("CREATE_ET_VALUE", aValue);
            return iRows;
        }

        /// <summary>파일의 측정값 전체를 조회한다.</summary>
        public DataTable GetEtValueList(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE oTQP_GDS_ET_VALUE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE();
            return oTQP_GDS_ET_VALUE.GetData("SELECT_ET_VALUE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        #endregion

        #region [TQP_GDS_ET_LIMIT]

        /// <summary>Spec 항목 이름 최대 길이(TQP_GDS_ET_LIMIT.ITEM_NAME VARCHAR2(200 CHAR)).</summary>
        private const int MaxItemNameChars = 200;

        /// <summary>헤더 끝의 '#번호'(FT: "...@TSVDC_HBM_C_P_FH #717"). 헤더 원문을 그대로 항목명으로 쓰고 '#번호' 만 지운다 (2026-10-12).</summary>
        private static readonly System.Text.RegularExpressions.Regex ItemSuffixRegex =
            new System.Text.RegularExpressions.Regex(@"\s*#\S*\s*$", System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        /// <summary>측정 항목이 있는 Device 목록을 조회한다.</summary>
        public DataTable GetEtDeviceList(string strFactory)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            return oTQP_GDS_ET_ITEM.GetData("SELECT_ET_ITEM_DEVICE_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
        }

        /// <summary>Device 의 공정 목록(업로드된 파일 + Spec 이 있는 공정)을 조회한다.</summary>
        public DataTable GetEtTestProgramList(string strFactory, string strDeviceID)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_TEST_PROGRAM_LIST", null, new string[] { strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice });
        }

        /// <summary>
        /// Spec 화면 목록(정의 Parameter + 대표 Chain + 데이터 유무 + 그 공정 READY 파일 값 통계 + Fail 수)을 조회한다.
        /// 같은 Factory / Device / 공정을 이름이 다른 바인딩 변수로 여러 번 넘긴다(쿼리 순서: f1 d1 t1 f2 d2 f3 d3 t3).
        /// 2026-10-12 묶음 기능: 대표 Chain 조인(f4 d4)을 뺐다. 묶음 / Setup Chain 은 화면이 Chain 묶음 목록으로 계산한다.
        /// </summary>
        public DataTable GetEtLimitList(string strFactory, string strDeviceID, string strTestProgram)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            string strCleanProgram = RequiredChars(strTestProgram, "공정(Test Type)", 50).ToUpperInvariant();
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT_LIST", null, new string[] {
                strCleanFactory, strCleanDevice, strCleanProgram,
                strCleanFactory, strCleanDevice,
                strCleanFactory, strCleanDevice, strCleanProgram });
        }

        /// <summary>Device + 공정의 저장된 Spec 을 조회한다(판정용).</summary>
        public DataTable GetEtLimit(string strFactory, string strDeviceID, string strTestProgram)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT", null, new string[] {
                Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50), RequiredChars(strTestProgram, "공정(Test Type)", 50).ToUpperInvariant() });
        }

        /// <summary>
        /// Spec 을 저장한다(있으면 수정 / 없으면 추가).
        /// 기준 (2026-10-12): 키 = Device + 공정 + Parameter(헤더 전체) / 고객 Spec 표에 있는 Parameter 만 정의 / 정의된 항목의 '-' 만 Fail / '#번호' 제거 / 대문자.
        /// 처리 흐름:
        ///  1) 입력 검증: Parameter 는 공백 정리 + 끝 '#번호' 제거 + 대문자, '@' 가 있어야 한다. 같은 Parameter 가 두 번 오면 거부.
        ///     Lower / Upper 는 숫자 또는 빈 값, 하나는 있어야 하고 둘 다 있으면 Lower &lt;= Upper. Enable 은 Y / N.
        ///  2) 아직 업로드되지 않은 Parameter 도 저장한다(고객 Spec 표를 먼저 등록할 수 있게). 단위가 비어 있으면 측정 항목 단위로 채운다.
        ///  3) MERGE 로 한 번에 저장한다(InsertExecuteMultiple).
        /// </summary>
        public int SaveEtLimit(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Spec 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            string strProgram = RequiredChars(aKey[2], "공정(Test Type)", 50).ToUpperInvariant();
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Spec 이 없습니다.", "aRows");
            if (aRows.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("Spec 은 한 번에 " + MaxBatchRows + "건 이하로 저장하세요.", "aRows");
            if (aRows.GetLength(1) != 7)
                throw new ArgumentException("Spec 열 수는 7개여야 합니다.", "aRows");

            Dictionary<string, string> dicUnit = new Dictionary<string, string>(StringComparer.Ordinal);
            DataTable dtItem = null;
            try
            {
                dtItem = GetEtItemList(strFactory, strDevice);
                foreach (DataRow dr in dtItem.Rows)
                {
                    string strKey = NormalizeItemName(dr["ITEM_NAME"].ToString());
                    if (!dicUnit.ContainsKey(strKey))
                        dicUnit[strKey] = dr["UNIT"].ToString();
                }
            }
            finally
            {
                if (dtItem != null)
                    dtItem.Dispose();
            }

            int iRows = aRows.GetLength(0);
            string[,] aMerge = new string[iRows, 11];
            HashSet<string> setItem = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < iRows; i++)
            {
                string strItem = RequiredItemName(aRows[i, 0]);
                if (!setItem.Add(strItem))
                    throw new ArgumentException("같은 Parameter 가 두 번 있습니다: " + strItem, "aRows");
                string strLower = OptionalDouble(aRows[i, 1], strItem + " Lower");
                string strUpper = OptionalDouble(aRows[i, 2], strItem + " Upper");
                if (strLower.Length == 0 && strUpper.Length == 0)
                    throw new ArgumentException(strItem + " 의 Lower / Upper 중 하나는 입력해야 합니다.", "aRows");
                if (strLower.Length > 0 && strUpper.Length > 0
                    && double.Parse(strLower, CultureInfo.InvariantCulture) > double.Parse(strUpper, CultureInfo.InvariantCulture))
                    throw new ArgumentException(strItem + " 의 Lower(" + strLower + ") 가 Upper(" + strUpper + ") 보다 큽니다.", "aRows");
                string strEnable = Clean(aRows[i, 3]).ToUpperInvariant();
                if (strEnable != "Y" && strEnable != "N")
                    throw new ArgumentException("Enable 은 Y 또는 N 이어야 합니다: " + aRows[i, 3], "aRows");
                string strUnit = OptionalChars(aRows[i, 4], strItem + " 단위", 10);
                string strItemUnit;
                if (strUnit.Length == 0 && dicUnit.TryGetValue(strItem, out strItemUnit))
                    strUnit = strItemUnit;
                aMerge[i, 0] = strFactory;
                aMerge[i, 1] = strDevice;
                aMerge[i, 2] = strProgram;
                aMerge[i, 3] = strItem;
                aMerge[i, 4] = strLower;
                aMerge[i, 5] = strUpper;
                aMerge[i, 6] = strUnit;
                aMerge[i, 7] = strEnable;
                aMerge[i, 8] = OptionalChars(aRows[i, 5], "Spec 원문", 200);
                aMerge[i, 9] = OptionalChars(aRows[i, 6], "비고", 200);
                aMerge[i, 10] = strUser;
            }
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            oTQP_GDS_ET_LIMIT.InsertExecuteMultiple("MERGE_ET_LIMIT", aMerge);
            return iRows;
        }

        /// <summary>Spec 1행을 삭제한다. aKey: 0 Factory / 1 Device / 2 공정 / 3 Parameter(저장 때와 같은 규칙으로 정리해 찾는다).</summary>
        public int DeleteEtLimit(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 4)
                throw new ArgumentException("Spec 삭제 키는 4개 항목(Factory / Device / 공정 / Parameter)이어야 합니다.", "aKey");
            Required(strUpdateUser, "Update User", 30);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.DeleteDataNonQuery("DELETE_ET_LIMIT", null, new string[] {
                Required(aKey[0], "Factory", 30), Required(aKey[1], "Device ID", 50),
                RequiredChars(aKey[2], "공정(Test Type)", 50).ToUpperInvariant(), RequiredItemName(aKey[3]) });
        }

        /// <summary>Parameter 이름 정리: 연속 공백 1칸 / 끝 '#번호' 제거 / 대문자. 화면 EtSpecTable.NormalizeItemName 과 같은 규칙이다.</summary>
        private static string NormalizeItemName(string strValue)
        {
            string strClean = System.Text.RegularExpressions.Regex.Replace(Clean(strValue), @"\s+", " ");
            return ItemSuffixRegex.Replace(strClean, string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>Spec Parameter 필수 검증: 정리 후 1~200자, '@'(Chain 구분) 포함.</summary>
        private static string RequiredItemName(string strValue)
        {
            string strItem = NormalizeItemName(strValue);
            if (strItem.Length == 0 || strItem.Length > MaxItemNameChars)
                throw new ArgumentException("Parameter 는 1~" + MaxItemNameChars + "자로 입력하세요: " + strValue, "Parameter");
            int iAt = strItem.IndexOf('@');
            if (iAt <= 0 || iAt == strItem.Length - 1)
                throw new ArgumentException("Parameter 는 측정 헤더 전체(예: RESISTANCE_IFORCE=8.0MA@TSVDC_TL_FH)여야 합니다: " + strValue, "Parameter");
            return strItem;
        }

        /// <summary>빈 값이면 빈 문자열(NULL), 아니면 유한한 숫자인지 확인해 R 형식(왕복 가능) 문자열로 돌려준다.</summary>
        private static string OptionalDouble(string strValue, string strName)
        {
            string strClean = Clean(strValue);
            if (strClean.Length == 0)
                return string.Empty;
            double dValue;
            if (!double.TryParse(strClean, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) || double.IsNaN(dValue) || double.IsInfinity(dValue))
                throw new ArgumentException(strName + " 값이 숫자가 아닙니다: " + strValue, strName);
            return dValue.ToString("R", CultureInfo.InvariantCulture);
        }

        #endregion

        #region [TQP_GDS_ET_CHAIN_GROUP]

        /// <summary>Chain 이름 최대 길이(TQP_GDS_ET_ITEM.CHAIN_NAME / TQP_GDS_ET_CHAIN_GROUP.CHAIN_NAME VARCHAR2(64 CHAR)).</summary>
        private const int MaxChainNameChars = 64;

        /// <summary>
        /// Chain 묶기 목록: 측정 항목에서 뽑은 E-Test Chain(CHAIN_NAME / CHAIN_NAME2, 대문자) + 묶음 번호 / 묶음 Chain 목록 / 묶음 수.
        /// 쿼리 순서: f1 d1 f2 d2 f3 d3.
        /// </summary>
        public DataTable GetEtChainGroupList(string strFactory, string strDeviceID)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP oTQP_GDS_ET_CHAIN_GROUP = new DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP();
            return oTQP_GDS_ET_CHAIN_GROUP.GetData("SELECT_ET_CHAIN_GROUP_LIST", null, new string[] {
                strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice });
        }

        /// <summary>Setup Chain 목록: Factory 의 GDS Chain 이름(현재 READY 버전). GDS Device 는 화면이 Sub Device 연결(GdsMapView.GetMapDeviceBySub)로 거른다.</summary>
        public DataTable GetEtGdsChainList(string strFactory)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP oTQP_GDS_ET_CHAIN_GROUP = new DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP();
            return oTQP_GDS_ET_CHAIN_GROUP.GetData("SELECT_ET_GDS_CHAIN_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
        }

        /// <summary>
        /// Chain 을 묶는다. 대표 Chain 이름 없이 묶음 번호만 둔다 (2026-10-12).
        /// aKey: 0 Factory / 1 Device. aRows 열: 0 묶음 구분값(같은 값끼리 한 묶음, 화면 임시 값) / 1 E-Test Chain(대문자로 저장) / 2 비고
        /// 처리 흐름:
        ///  1) 입력 검증: Chain 대문자, ':' / '@' 거부, 같은 Chain 두 번 거부, 묶음마다 Chain 2개 이상.
        ///  2) 새 묶음 번호를 묶음 수만큼 받는다(ET_CHAIN_GROUP_SEQ).
        ///  3) MERGE 로 한 번에 넣는다. 이미 다른 묶음에 있던 Chain 은 새 묶음으로 옮긴다.
        ///  4) 옮긴 뒤 1개만 남은 이전 묶음은 지운다.
        /// 측정 항목에 아직 없는 Chain 도 받는다. 반환: 만든 묶음 수
        /// </summary>
        public int CreateEtChainGroups(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 2)
                throw new ArgumentException("Chain 묶음 키는 2개 항목(Factory / Device)이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("묶을 Chain 이 없습니다.", "aRows");
            if (aRows.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("Chain 묶음은 한 번에 " + MaxBatchRows + "행 이하로 저장하세요.", "aRows");
            if (aRows.GetLength(1) != 3)
                throw new ArgumentException("Chain 묶음 열 수는 3개여야 합니다.", "aRows");

            int iRows = aRows.GetLength(0);
            List<string> lstGroupKey = new List<string>();
            Dictionary<string, int> dicGroupSize = new Dictionary<string, int>(StringComparer.Ordinal);
            HashSet<string> setChain = new HashSet<string>(StringComparer.Ordinal);
            string[] aChain = new string[iRows];
            string[] aGroupKey = new string[iRows];
            for (int i = 0; i < iRows; i++)
            {
                aGroupKey[i] = RequiredChars(aRows[i, 0], "묶음 구분값", 100);
                aChain[i] = RequiredChainName(aRows[i, 1]);
                if (!setChain.Add(aChain[i]))
                    throw new ArgumentException("같은 Chain 이 두 번 있습니다: " + aChain[i], "aRows");
                int iSize;
                if (!dicGroupSize.TryGetValue(aGroupKey[i], out iSize))
                    lstGroupKey.Add(aGroupKey[i]);
                dicGroupSize[aGroupKey[i]] = iSize + 1;
            }
            foreach (KeyValuePair<string, int> kv in dicGroupSize)
                if (kv.Value < 2)
                    throw new ArgumentException("묶음에는 Chain 이 2개 이상 있어야 합니다(구분값 " + kv.Key + ").", "aRows");

            DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP oTQP_GDS_ET_CHAIN_GROUP = new DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP();
            Dictionary<string, string> dicSeq = new Dictionary<string, string>(StringComparer.Ordinal);
            DataTable dtSeq = null;
            try
            {
                dtSeq = oTQP_GDS_ET_CHAIN_GROUP.GetData("SELECT_NEXT_ET_CHAIN_GROUP_SEQ", null,
                    new string[] { lstGroupKey.Count.ToString(CultureInfo.InvariantCulture) });
                if (dtSeq.Rows.Count != lstGroupKey.Count)
                    throw new InvalidOperationException("묶음 번호를 받지 못했습니다.");
                for (int g = 0; g < lstGroupKey.Count; g++)
                    dicSeq[lstGroupKey[g]] = Convert.ToInt64(dtSeq.Rows[g]["GROUP_SEQ"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            }
            finally
            {
                if (dtSeq != null)
                    dtSeq.Dispose();
            }

            string[,] aMerge = new string[iRows, 6];
            for (int i = 0; i < iRows; i++)
            {
                aMerge[i, 0] = strFactory;
                aMerge[i, 1] = strDevice;
                aMerge[i, 2] = aChain[i];
                aMerge[i, 3] = dicSeq[aGroupKey[i]];
                aMerge[i, 4] = OptionalChars(aRows[i, 2], "비고", 200);
                aMerge[i, 5] = strUser;
            }
            oTQP_GDS_ET_CHAIN_GROUP.InsertExecuteMultiple("MERGE_ET_CHAIN_GROUP", aMerge);
            oTQP_GDS_ET_CHAIN_GROUP.DeleteDataNonQuery("DELETE_ET_CHAIN_GROUP_SINGLE", null, new string[] { strFactory, strDevice, strFactory, strDevice });
            return lstGroupKey.Count;
        }

        /// <summary>묶음 해제: Chain 이 들어 있는 묶음 전체를 지운다. aKey: 0 Factory / 1 Device / 2 E-Test Chain. 반환: 지운 행 수(묶여 있지 않으면 0)</summary>
        public int DeleteEtChainGroup(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Chain 묶음 해제 키는 3개 항목(Factory / Device / Chain)이어야 합니다.", "aKey");
            Required(strUpdateUser, "Update User", 30);
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP oTQP_GDS_ET_CHAIN_GROUP = new DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP();
            return oTQP_GDS_ET_CHAIN_GROUP.DeleteDataNonQuery("DELETE_ET_CHAIN_GROUP_BY_CHAIN", null, new string[] {
                strFactory, strDevice, strFactory, strDevice, RequiredChainName(aKey[2]) });
        }

        /// <summary>
        /// Setup Chain(GDS Chain 이름)으로 E-Test Chain 을 찾는다. 묶인 Chain 까지 넓혀 Limit 등 정보를 모두 찾는 데 쓴다 (2026-10-12).
        /// 찾는 순서:
        ///  1) EXACT: E-Test Chain 이름이 Setup Chain 이름과 같다(대소문자 무시). 예: TSVDC_ASIC1_P_FL (Input Label 이 그대로 Chain 이름인 경우)
        ///  2) SUFFIX: 1) 이 없을 때만, Setup Chain 이름 + _FH / _FL. 예: PIC_COMB12_M4 -> PIC_COMB12_M4_FH / _FL
        ///  3) GROUP: 찾은 Chain 이 묶여 있으면 같은 묶음의 나머지 Chain
        /// E-Test Chain 은 측정 항목 / 묶음 / Spec 에 나온 Chain 에서 찾는다.
        /// 반환 열: CHAIN_NAME / MATCH_TYPE(EXACT / SUFFIX / GROUP) / GROUP_SEQ(빈 값 = 묶음 없음). 없으면 0행.
        /// </summary>
        public DataTable GetEtChainMatch(string strFactory, string strDeviceID, string strSetupChain)
        {
            string f = Required(strFactory, "Factory", 30);
            string d = Required(strDeviceID, "Device ID", 50);
            string k = RequiredChars(strSetupChain, "Setup Chain", 200).ToUpperInvariant();
            DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP oTQP_GDS_ET_CHAIN_GROUP = new DACrux.SEMDMS.DSL.TQP_GDS_ET_CHAIN_GROUP();
            DataTable dtHit = oTQP_GDS_ET_CHAIN_GROUP.GetData("SELECT_ET_CHAIN_MATCH", null, new string[] {
                f, d, f, d, f, d, f, d, f, d, k, f, d, k, k, k });

            DataTable dtResult = new DataTable("ET_CHAIN_MATCH");
            dtResult.Columns.Add("CHAIN_NAME", typeof(string));
            dtResult.Columns.Add("MATCH_TYPE", typeof(string));
            dtResult.Columns.Add("GROUP_SEQ", typeof(string));
            bool bExact = dtHit.Rows.Cast<DataRow>().Any(r => r["MATCH_TYPE"].ToString() == "EXACT");
            HashSet<string> setChain = new HashSet<string>(StringComparer.Ordinal);
            List<string> lstGroup = new List<string>();
            foreach (DataRow dr in dtHit.Rows)
            {
                if (bExact && dr["MATCH_TYPE"].ToString() != "EXACT")
                    continue;
                string strGroup = dr["GROUP_SEQ"] == DBNull.Value ? string.Empty : Convert.ToInt64(dr["GROUP_SEQ"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                if (setChain.Add(dr["CHAIN_NAME"].ToString()))
                    dtResult.Rows.Add(dr["CHAIN_NAME"].ToString(), dr["MATCH_TYPE"].ToString(), strGroup);
                if (strGroup.Length > 0 && !lstGroup.Contains(strGroup))
                    lstGroup.Add(strGroup);
            }
            dtHit.Dispose();
            foreach (string strGroup in lstGroup)
            {
                using (DataTable dtMember = oTQP_GDS_ET_CHAIN_GROUP.GetData("SELECT_ET_CHAIN_GROUP_MEMBER", null, new string[] { f, d, strGroup }))
                {
                    foreach (DataRow dr in dtMember.Rows)
                        if (setChain.Add(dr["CHAIN_NAME"].ToString()))
                            dtResult.Rows.Add(dr["CHAIN_NAME"].ToString(), "GROUP", strGroup);
                }
            }
            return dtResult;
        }

        /// <summary>
        /// Setup Chain 으로 찾은 E-Test Chain(GetEtChainMatch, 묶음 포함)의 Spec / 측정 항목 정보를 모두 모은다.
        /// Chain 마다 SELECT_ET_LIMIT_BY_CHAIN 을 부르고 같은 (공정, Parameter) 는 한 행으로 합친다(Leakage_Serial 처럼 Chain 2개가 모두 걸리는 항목).
        /// 반환 열: TEST_PROGRAM(빈 값 = Spec 없는 측정 항목) / ITEM_NAME / CHAIN_NAME / CHAIN_NAME2 / DEFINED(Y/N) / LOWER_LIMIT / UPPER_LIMIT / UNIT / ENABLE_FLAG / SPEC_TEXT / REMARK /
        ///          FILE_COUNT / VALUE_COUNT / NOT_TESTED_COUNT / MIN_VALUE / MAX_VALUE / FAIL_COUNT / MATCH_CHAIN(이 행을 찾게 한 E-Test Chain, 여러 개면 ", ")
        /// 통계는 READY 파일 기준. Spec 행은 그 공정 파일, Spec 없는 측정 항목은 모든 공정 파일. Fail 수는 Enable Spec 기준 범위 밖 + '-'.
        /// </summary>
        public DataTable GetEtChainSpecInfo(string strFactory, string strDeviceID, string strSetupChain)
        {
            string f = Required(strFactory, "Factory", 30);
            string d = Required(strDeviceID, "Device ID", 50);
            DataTable dtMatch = GetEtChainMatch(f, d, strSetupChain);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            DataTable dtResult = null;
            Dictionary<string, DataRow> dicRow = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            foreach (DataRow drMatch in dtMatch.Rows)
            {
                string c = drMatch["CHAIN_NAME"].ToString();
                DataTable dt = oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT_BY_CHAIN", null, new string[] { f, d, c, c, f, d, c, c, f, d });
                if (dtResult == null)
                {
                    dtResult = dt.Clone();
                    dtResult.TableName = "ET_CHAIN_SPEC_INFO";
                    dtResult.Columns.Add("MATCH_CHAIN", typeof(string));
                }
                foreach (DataRow dr in dt.Rows)
                {
                    string strKey = dr["TEST_PROGRAM"] + "\t" + dr["ITEM_NAME"].ToString().ToUpperInvariant();
                    DataRow drOld;
                    if (dicRow.TryGetValue(strKey, out drOld))
                    {
                        drOld["MATCH_CHAIN"] = drOld["MATCH_CHAIN"] + ", " + c;
                        continue;
                    }
                    DataRow drNew = dtResult.NewRow();
                    foreach (DataColumn col in dt.Columns)
                        drNew[col.ColumnName] = dr[col];
                    drNew["MATCH_CHAIN"] = c;
                    dtResult.Rows.Add(drNew);
                    dicRow[strKey] = drNew;
                }
                dt.Dispose();
            }
            dtMatch.Dispose();
            if (dtResult == null)
            {
                // 찾은 Chain 이 없으면 열만 있는 빈 표를 돌려준다(화면이 열 이름을 그대로 쓰게).
                dtResult = oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT_BY_CHAIN", null, new string[] { f, d, string.Empty, string.Empty, f, d, string.Empty, string.Empty, f, d });
                dtResult.TableName = "ET_CHAIN_SPEC_INFO";
                dtResult.Columns.Add("MATCH_CHAIN", typeof(string));
                return dtResult;
            }
            DataView dv = new DataView(dtResult) { Sort = "DEFINED DESC, TEST_PROGRAM ASC, ITEM_NAME ASC" };
            DataTable dtSorted = dv.ToTable("ET_CHAIN_SPEC_INFO");
            dtResult.Dispose();
            return dtSorted;
        }

        /// <summary>E-Test Chain 이름 필수 검증: 1~64자, 대문자. ':' / '@' 는 Chain 구분자라 거부한다.</summary>
        private static string RequiredChainName(string strValue)
        {
            string strChain = RequiredChars(strValue, "Chain", MaxChainNameChars).ToUpperInvariant();
            if (strChain.IndexOf(':') >= 0 || strChain.IndexOf('@') >= 0)
                throw new ArgumentException("Chain 이름에 ':' 또는 '@' 를 쓸 수 없습니다: " + strValue, "Chain");
            return strChain;
        }

        #endregion

        #region [Batch Common]

        /// <summary>한 번에 받을 수 있는 배치 행 수. Remoting 요청 크기와 DB 배열 바인딩 크기를 제한한다(GdsMapImport와 같음).</summary>
        private const int MaxBatchRows = 10000;

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

        #endregion

        #region [Validation]

        /// <summary>null을 빈 문자열로 바꾸고 앞뒤 공백을 지운다. 빈 문자열은 Oracle에서 NULL로 저장된다.</summary>
        private static string Clean(string strValue)
        {
            return strValue == null ? string.Empty : strValue.Trim();
        }

        /// <summary>정수 문자열을 int로 바꾼다.</summary>
        private static int ParseInt(string strValue, string strName)
        {
            int iValue;
            if (!int.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out iValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return iValue;
        }

        /// <summary>빈 값이면 빈 문자열(NULL), 아니면 정수인지 확인한 값을 돌려준다.</summary>
        private static string OptionalNumber(string strValue, string strName)
        {
            string strClean = Clean(strValue);
            if (strClean.Length == 0)
                return string.Empty;
            long lValue;
            if (!long.TryParse(strClean, NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>건수 문자열을 0 이상의 정수로 바꾼다.</summary>
        private static long ParseCount(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue < 0)
                throw new ArgumentException(strName + "는 0 이상의 정수여야 합니다.", strName);
            return lValue;
        }

        /// <summary>Slot 번호가 1~99 정수인지 확인한다(WAFER_ID = Lot + '-' + 2자리 Slot).</summary>
        private static string RequiredSlot(string strValue)
        {
            int iSlot;
            if (!int.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out iSlot) || iSlot < 1 || iSlot > 99)
                throw new ArgumentException("Slot 번호는 1~99 정수여야 합니다: " + strValue, "Slot");
            return iSlot.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>시험 일시가 yyyyMMddHHmmss 14자리 날짜인지 확인한다.</summary>
        private static string RequiredTestTime(string strValue)
        {
            string strClean = Clean(strValue);
            DateTime dtValue;
            if (!DateTime.TryParseExact(strClean, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
                throw new ArgumentException("시험 일시는 yyyyMMddHHmmss 14자리여야 합니다: " + strValue, "TestTime");
            return strClean;
        }

        /// <summary>시퀀스 번호(파일 번호) 문자열이 1 이상의 정수인지 확인하고 정리한 값을 돌려준다.</summary>
        private static string RequiredSeq(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue <= 0)
                throw new ArgumentException(strName + "는 1 이상의 정수여야 합니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>바이트 길이 컬럼(VARCHAR2(n) / n CHAR 중 짧은 쪽 기준)에 맞는 필수 문자열인지 확인한다.</summary>
        private static string Required(string strValue, string strName, int iMaxBytes)
        {
            string strResult = Clean(strValue);
            int iLength = Encoding.UTF8.GetByteCount(strResult);
            if (iLength == 0 || iLength > iMaxBytes)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxBytes + "바이트로 입력하세요.", strName);
            return strResult;
        }

        /// <summary>CHAR 길이 컬럼(VARCHAR2(n CHAR))에 맞는 필수 문자열인지 확인한다.</summary>
        private static string RequiredChars(string strValue, string strName, int iMaxChars)
        {
            string strResult = Clean(strValue);
            if (strResult.Length == 0 || strResult.Length > iMaxChars)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxChars + "자로 입력하세요: " + strValue, strName);
            return strResult;
        }

        /// <summary>선택 문자열(빈 값이면 NULL 저장)이 CHAR 길이를 넘지 않는지 확인한다.</summary>
        private static string OptionalChars(string strValue, string strName, int iMaxChars)
        {
            string strResult = Clean(strValue);
            if (strResult.Length > iMaxChars)
                throw new ArgumentException(strName + "은(는) " + iMaxChars + "자 이하로 입력하세요: " + strValue, strName);
            return strResult;
        }

        #endregion
    }
}
