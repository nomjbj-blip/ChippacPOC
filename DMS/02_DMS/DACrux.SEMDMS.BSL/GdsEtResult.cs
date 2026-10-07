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
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절 (CLAUDE-029)
    /// 파일 Parsing은 클라이언트가 하고, 서버는 받은 행을 검증해 DefectDefine.CreateDefect와 같은 Bulk Insert(InsertExecuteMultiple)로 저장한다.
    /// 문장마다 자동 Commit이다. 중간에 끊기면 헤더가 LOADING으로 남고, 다음 저장 때 화면이 삭제 여부를 묻는다.
    /// </summary>
    public class GdsEtResult : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsEtResult
    {
        public GdsEtResult()
        {
        }

        #region [TQP_GDS_SRC_FILE]

        /// <summary>
        /// 같은 Wafer 파일(UNIQUE 키 Factory + WAFER_ID)이 있는지 조회한다. 없으면 0행이다.
        /// aKey: 0 Factory / 1 Lot ID / 2 Slot 번호. 2026-10-07 사용자 지시로 Device / 시험 일시는 키에서 뺐다(같은 Device에 여러 Wafer, 같은 Wafer는 1건).
        /// </summary>
        public DataTable GetEtFileByKey(string[] aKey)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("파일 키는 3개 항목(Factory / Lot / Slot)이어야 합니다.", "aKey");
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BY_WAFER", null, new string[] {
                Required(aKey[0], "Factory", 30), WaferId(Required(aKey[1], "Lot ID", 50), RequiredSlot(aKey[2])) });
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
        /// Slot 번호는 파일에 없어 화면에서 사용자가 입력한다(2026-10-07 사용자 지시).
        /// 처리: 입력 검증 -> 같은 Wafer 파일 확인(Factory + WAFER_ID, 있으면 거부, 화면이 먼저 묻고 DeleteEtFile 호출) -> 번호 채번 -> 헤더 생성(LOADING) -> 헤더 1행 반환
        /// </summary>
        public DataTable BeginEtFile(string[] FileInfo)
        {
            if (FileInfo == null || FileInfo.Length != 12)
                throw new ArgumentException("저장 시작 정보는 12개 항목이어야 합니다.", "FileInfo");
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

            DataTable dtExist = null;
            try
            {
                dtExist = GetEtFileByKey(new string[] { strFactory, strLot, strSlot });
                if (dtExist.Rows.Count > 0)
                    throw new InvalidOperationException("같은 Wafer 파일이 이미 있습니다(WAFER_ID " + dtExist.Rows[0]["WAFER_ID"]
                        + " / Device " + dtExist.Rows[0]["DEVICE_ID"] + " / 파일 번호 " + dtExist.Rows[0]["SRC_FILE_SEQ"] + "). 삭제한 뒤 다시 저장하세요.");
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
            int iRows = oTQP_GDS_SRC_FILE.InsertDataNonQuery("CREATE_SRC_FILE", null, new string[] {
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
        ///  3) Die 수 = 헤더 Die 수, 측정값 수 = Die 수 x 항목 수이면 READY(VALUE_COUNT 기록). 다르면 LOADING 그대로 두고 MISMATCH 반환.
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
                int iRows = oTQP_GDS_SRC_FILE.UpdateDataNonQuery("UPDATE_SRC_FILE_READY", null,
                    new string[] { lDbValue.ToString(CultureInfo.InvariantCulture), strUser, strSeq });
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

        /// <summary>측정 항목이 있는 Device 목록을 조회한다.</summary>
        public DataTable GetEtDeviceList(string strFactory)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            return oTQP_GDS_ET_ITEM.GetData("SELECT_ET_ITEM_DEVICE_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
        }

        /// <summary>Limit 설정 화면 목록(Para + Limit + READY 파일 값 통계)을 조회한다. 같은 Factory / Device 를 쿼리에 세 번 넘긴다(이름이 다른 바인딩 변수).</summary>
        public DataTable GetEtLimitList(string strFactory, string strDeviceID)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT_LIST", null, new string[] {
                strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice });
        }

        /// <summary>Device 의 저장된 Limit 을 조회한다(판정용).</summary>
        public DataTable GetEtLimit(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        /// <summary>
        /// Para Limit 을 저장한다(2026-10-07 사용자 결정: Para 별 구간 / Device 별 / 등록 시 대문자 / Enable / 한쪽 Limit 허용).
        /// 처리 흐름:
        ///  1) 입력 검증: Para 는 대문자로 바꾼다. 같은 Para 가 두 번 오면 거부. Lower / Upper 는 숫자 또는 빈 값, 하나는 있어야 하고 둘 다 있으면 Lower &lt;= Upper. Enable 은 Y / N.
        ///  2) Device 의 Para 목록(GetEtLimitList)에 없는 Para 는 거부. 측정 항목 단위가 섞인 Para 는 거부.
        ///  3) 단위는 측정 항목에서 채우고 MERGE 로 한 번에 저장한다(InsertExecuteMultiple).
        /// </summary>
        public int SaveEtLimit(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 2)
                throw new ArgumentException("Limit 키는 2개 항목(Factory / Device)이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Limit 이 없습니다.", "aRows");
            if (aRows.GetLength(1) != 5)
                throw new ArgumentException("Limit 열 수는 5개여야 합니다.", "aRows");

            Dictionary<string, DataRow> dicPara = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            DataTable dtList = null;
            try
            {
                dtList = GetEtLimitList(strFactory, strDevice);
                foreach (DataRow dr in dtList.Rows)
                    dicPara[dr["PARA_NAME"].ToString()] = dr;

                int iRows = aRows.GetLength(0);
                string[,] aMerge = new string[iRows, 9];
                HashSet<string> setPara = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < iRows; i++)
                {
                    string strPara = RequiredChars(aRows[i, 0], "Para", 50).ToUpperInvariant();
                    if (!setPara.Add(strPara))
                        throw new ArgumentException("같은 Para 가 두 번 있습니다: " + strPara, "aRows");
                    DataRow drPara;
                    if (!dicPara.TryGetValue(strPara, out drPara))
                        throw new InvalidOperationException("Device " + strDevice + " 에 없는 Para 입니다: " + strPara);
                    if (DACrux.Base.Convert.longParse(drPara["UNIT_KINDS"].ToString()) > 1)
                        throw new InvalidOperationException("Para " + strPara + " 의 측정 항목 단위가 섞여 있어 Limit 을 정할 수 없습니다.");
                    string strLower = OptionalDouble(aRows[i, 1], strPara + " Lower");
                    string strUpper = OptionalDouble(aRows[i, 2], strPara + " Upper");
                    if (strLower.Length == 0 && strUpper.Length == 0)
                        throw new ArgumentException("Para " + strPara + " 의 Lower / Upper 중 하나는 입력해야 합니다.", "aRows");
                    if (strLower.Length > 0 && strUpper.Length > 0
                        && double.Parse(strLower, CultureInfo.InvariantCulture) > double.Parse(strUpper, CultureInfo.InvariantCulture))
                        throw new ArgumentException("Para " + strPara + " 의 Lower(" + strLower + ") 가 Upper(" + strUpper + ") 보다 큽니다.", "aRows");
                    string strEnable = Clean(aRows[i, 3]).ToUpperInvariant();
                    if (strEnable != "Y" && strEnable != "N")
                        throw new ArgumentException("Enable 은 Y 또는 N 이어야 합니다: " + aRows[i, 3], "aRows");
                    aMerge[i, 0] = strFactory;
                    aMerge[i, 1] = strDevice;
                    aMerge[i, 2] = strPara;
                    aMerge[i, 3] = strLower;
                    aMerge[i, 4] = strUpper;
                    aMerge[i, 5] = drPara["UNIT"].ToString();
                    aMerge[i, 6] = strEnable;
                    aMerge[i, 7] = OptionalChars(aRows[i, 4], "비고", 200);
                    aMerge[i, 8] = strUser;
                }
                DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
                oTQP_GDS_ET_LIMIT.InsertExecuteMultiple("MERGE_ET_LIMIT", aMerge);
                return iRows;
            }
            finally
            {
                if (dtList != null)
                    dtList.Dispose();
            }
        }

        /// <summary>Para Limit 1행을 삭제한다. Para 는 대문자로 바꿔 찾는다.</summary>
        public int DeleteEtLimit(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Limit 삭제 키는 3개 항목(Factory / Device / Para)이어야 합니다.", "aKey");
            Required(strUpdateUser, "Update User", 30);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.DeleteDataNonQuery("DELETE_ET_LIMIT", null, new string[] {
                Required(aKey[0], "Factory", 30), Required(aKey[1], "Device ID", 50), RequiredChars(aKey[2], "Para", 50).ToUpperInvariant() });
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
