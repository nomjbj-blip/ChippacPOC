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
    /// GDS Map 전체 도형 DB 저장(Import)의 업무 검증과 DSL 호출을 담당한다.
    /// 설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 4절 / 20절.
    /// 2026-10-02 사용자 결정으로 리비전을 관리하지 않는다. Map은 Factory + Device(제품)당 1개(TQP_GDS_MAP)이고,
    /// 기존 데이터가 있으면 화면이 사용자에게 묻고 DeleteMap으로 지운 뒤 다시 등록한다.
    /// 저장 작업(IMPORT_JOB)은 Device당 1건이며 작업 번호(IMPORT_JOB_SEQ)는 시퀀스다.
    /// </summary>
    public class GdsMapImport : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsMapImport
    {
        public GdsMapImport()
        {
        }

        #region [TQP_GDS_MAP]

        /// <summary>Device의 기존 Map 헤더 / 저장 작업 상태 / 건수를 조회한다. 없으면 0행이다. 화면은 저장 전에 이 결과로 삭제 여부를 묻는다.</summary>
        public DataTable GetMapInfo(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_MAP oTQP_GDS_MAP = new DACrux.SEMDMS.DSL.TQP_GDS_MAP();
            return oTQP_GDS_MAP.GetData("SELECT_MAP_INFO", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        /// <summary>
        /// Device의 GDS 데이터를 모두 삭제한다(기존 데이터 삭제 후 재등록).
        /// 처리 순서: CHAIN(하위 Chain 테이블은 FK CASCADE) -> PLACED_EL -> SOURCE_EL -> 헤더 TQP_GDS_MAP(LAYER / IMPORT_JOB -> IMPORT_BATCH는 FK CASCADE).
        /// 큰 테이블 사이 FK(SOURCE -> PLACED, PLACED -> CHAIN_MEMBER / EDIT 등)는 CASCADE가 아니다. 부모 행마다 내부 DELETE가 돌아
        /// 샘플 1 Device 삭제가 10분을 넘긴 측정 결과 때문에 자식부터 직접 지우도록 바꿨다(2026-10-02_tqp_gds_fk_delete_rule.sql).
        /// 문장마다 자동 Commit이라 중간에 끊기면 일부만 지워질 수 있지만, 같은 함수를 다시 부르면 남은 데이터를 마저 지운다.
        /// 반환: 지운 배치 도형 수
        /// </summary>
        public int DeleteMap(string strFactory, string strDeviceID, string strUpdateUser)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            Required(strUpdateUser, "Update User", 30);
            string[] aKey = new string[] { strCleanFactory, strCleanDevice };
            new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().DeleteDataNonQuery("DELETE_CHAIN", null, aKey);
            int iPlaced = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL().DeleteDataNonQuery("DELETE_PLACED_EL", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_SOURCE_EL().DeleteDataNonQuery("DELETE_SOURCE_EL", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_MAP().DeleteDataNonQuery("DELETE_MAP", null, aKey);
            return iPlaced;
        }

        #endregion

        #region [TQP_GDS_IMPORT_JOB]

        /// <summary>
        /// 저장을 시작한다. Device에 기존 Map이 있으면 거부한다(화면이 먼저 사용자에게 묻고 DeleteMap을 호출해야 한다).
        /// ImportInfo 순서: 0 Factory / 1 Device ID / 2 Top Structure / 3 GDS 파일 이름 / 4 GDS SHA-256 /
        /// 5 User Unit / 6 Database Unit / 7 Create User / 8 예상 SOURCE 수 / 9 예상 PLACED 수 / 10 예상 좌표 수
        /// 처리: 입력 검증 -> 기존 Map 확인 -> TQP_GDS_MAP(DRAFT) 생성 -> 저장 작업(UPLOADING) 생성 -> 작업 1행 반환
        /// </summary>
        public DataTable BeginMapImport(string[] ImportInfo)
        {
            if (ImportInfo == null || ImportInfo.Length != 11)
                throw new ArgumentException("저장 시작 정보는 11개 항목이어야 합니다.", "ImportInfo");
            string strFactory = Required(ImportInfo[0], "Factory", 30);
            string strDevice = Required(ImportInfo[1], "Device ID", 50);
            string strTop = Required(ImportInfo[2], "Top Structure", 1024);
            string strFileName = ImportInfo[3] == null ? string.Empty : ImportInfo[3].Trim();
            if (strFileName.Length > 260) strFileName = strFileName.Substring(strFileName.Length - 260);
            string strSha256 = Required(ImportInfo[4], "GDS SHA-256", 64).ToUpperInvariant();
            if (strSha256.Length != 64)
                throw new ArgumentException("GDS SHA-256은 64자리 16진수여야 합니다.", "ImportInfo");
            foreach (char ch in strSha256)
                if (!Uri.IsHexDigit(ch))
                    throw new ArgumentException("GDS SHA-256은 64자리 16진수여야 합니다.", "ImportInfo");
            double dUserUnit = ParseUnit(ImportInfo[5], "User Unit");
            double dDatabaseUnit = ParseUnit(ImportInfo[6], "Database Unit");
            if (double.IsNaN(dUserUnit) || double.IsNaN(dDatabaseUnit) || dUserUnit <= 0 || dDatabaseUnit <= 0 || double.IsInfinity(dUserUnit) || double.IsInfinity(dDatabaseUnit))
                throw new ArgumentException("GDS 좌표 단위는 0보다 큰 유한한 숫자여야 합니다.", "ImportInfo");
            string strCreateUser = Required(ImportInfo[7], "Create User", 30);
            long lExpectedSource = ParseCount(ImportInfo[8], "예상 SOURCE 수");
            long lExpectedPlaced = ParseCount(ImportInfo[9], "예상 PLACED 수");
            long lExpectedPoint = ParseCount(ImportInfo[10], "예상 좌표 수");

            DataTable dtInfo = null;
            try
            {
                dtInfo = GetMapInfo(strFactory, strDevice);
                if (dtInfo.Rows.Count > 0)
                    throw new InvalidOperationException("Factory " + strFactory + " / Device " + strDevice
                        + "에 기존 GDS 데이터가 있습니다. 삭제한 뒤 다시 등록하세요.");
            }
            finally
            {
                if (dtInfo != null)
                    dtInfo.Dispose();
            }

            DACrux.SEMDMS.DSL.TQP_GDS_MAP oTQP_GDS_MAP = new DACrux.SEMDMS.DSL.TQP_GDS_MAP();
            int iRows = oTQP_GDS_MAP.InsertDataNonQuery("CREATE_MAP", null, new string[] {
                strFactory, strDevice, strTop, strFileName, strSha256,
                dUserUnit.ToString("G17", CultureInfo.InvariantCulture), dDatabaseUnit.ToString("G17", CultureInfo.InvariantCulture),
                strCreateUser });
            if (iRows != 1)
                throw new DataException("GDS Map 헤더가 한 건 생성되지 않았습니다.");
            string strImportJobSeq = CreateImportJob(strFactory, strDevice, lExpectedSource, lExpectedPlaced, lExpectedPoint, strCreateUser);
            return GetImportJob(strImportJobSeq);
        }

        /// <summary>저장 작업 1건을 조회한다. 없으면 0행이다.</summary>
        public DataTable GetImportJob(string strImportJobSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
            return oTQP_GDS_IMPORT_JOB.GetData("SELECT_IMPORT_JOB", null, new string[] { RequiredSeq(strImportJobSeq, "작업 번호") });
        }

        /// <summary>진행 중인 저장 작업만 CANCELLED로 바꾼다. 저장된 일부 데이터는 남고, 다음 저장 때 삭제 여부를 묻는다.</summary>
        public void CancelImportJob(string strImportJobSeq, string strUpdateUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
            int iRows = oTQP_GDS_IMPORT_JOB.UpdateDataNonQuery("UPDATE_IMPORT_JOB_CANCEL", null,
                new string[] { Required(strUpdateUser, "Update User", 30), RequiredSeq(strImportJobSeq, "작업 번호") });
            if (iRows != 1)
                throw new InvalidOperationException("진행 중인 저장 작업이 아니어서 취소할 수 없습니다.");
        }

        /// <summary>
        /// 저장 완료 확정.
        /// 처리 흐름:
        ///  1) 작업 조회. 이미 READY면 Map 헤더만 READY로 맞추고 끝낸다(앞선 호출에서 헤더 변경 전에 끊긴 경우).
        ///  2) DB에서 SOURCE 수 / PLACED 수 / 좌표 수 합계를 다시 센다. 배치마다 누적한 수신 건수는 재전송에 따라 실제보다 클 수 있어 쓰지 않는다.
        ///  3) LAYER 배치가 있는지, 종류별 배치 번호가 0부터 빠짐없이 있는지 확인한다.
        ///  4) 예상 건수와 모두 같으면 작업 READY(수신 건수를 DB 값으로 맞춤) -> Map 헤더 READY. 다르면 UPLOADING 그대로 두고 MISMATCH 반환.
        /// </summary>
        public DataTable CompleteMapImport(string strImportJobSeq, string strUpdateUser)
        {
            string strUser = Required(strUpdateUser, "Update User", 30);
            DataTable dtJob = null;
            try
            {
                dtJob = GetImportJob(strImportJobSeq);
                if (dtJob.Rows.Count == 0)
                    throw new InvalidOperationException("저장 작업을 찾을 수 없습니다: " + strImportJobSeq);
                DataRow drJob = dtJob.Rows[0];
                string strStatus = drJob["JOB_STATUS"].ToString();
                string strFactory = drJob["FACTORY"].ToString();
                string strDevice = drJob["DEVICE_ID"].ToString();
                long lExpectedSource = DACrux.Base.Convert.longParse(drJob["EXPECTED_SOURCE_COUNT"].ToString());
                long lExpectedPlaced = DACrux.Base.Convert.longParse(drJob["EXPECTED_PLACED_COUNT"].ToString());
                long lExpectedPoint = DACrux.Base.Convert.longParse(drJob["EXPECTED_POINT_COUNT"].ToString());

                if (string.Equals(strStatus, "READY", StringComparison.Ordinal))
                {
                    SetMapReady(strFactory, strDevice, strUser);
                    return CompleteResult("OK", "READY", lExpectedSource, -1, lExpectedPlaced, -1, lExpectedPoint, -1, "이미 확정된 저장 작업입니다.");
                }
                if (!string.Equals(strStatus, "UPLOADING", StringComparison.Ordinal))
                    throw new InvalidOperationException("저장 작업이 " + strStatus + " 상태라서 확정할 수 없습니다.");

                long lDbSource, lDbPlaced, lDbPoint;
                CountMapRows(strFactory, strDevice, out lDbSource, out lDbPlaced, out lDbPoint);
                StringBuilder sbMessage = new StringBuilder(CheckBatchNumbers(strImportJobSeq));
                if (lDbSource != lExpectedSource) sbMessage.Append("SOURCE 건수 불일치 / ");
                if (lDbPlaced != lExpectedPlaced) sbMessage.Append("PLACED 건수 불일치 / ");
                if (lDbPoint != lExpectedPoint) sbMessage.Append("좌표 수 불일치 / ");
                if (sbMessage.Length > 0)
                    return CompleteResult("MISMATCH", strStatus, lExpectedSource, lDbSource, lExpectedPlaced, lDbPlaced,
                        lExpectedPoint, lDbPoint, sbMessage.ToString().TrimEnd(' ', '/'));

                DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
                int iRows = oTQP_GDS_IMPORT_JOB.UpdateDataNonQuery("UPDATE_IMPORT_JOB_READY", null, new string[] {
                    lDbSource.ToString(CultureInfo.InvariantCulture), lDbPlaced.ToString(CultureInfo.InvariantCulture),
                    lDbPoint.ToString(CultureInfo.InvariantCulture), strUser, strImportJobSeq });
                if (iRows != 1)
                    throw new InvalidOperationException("저장 작업 상태가 바뀌어 확정하지 못했습니다. 다시 조회하세요.");
                SetMapReady(strFactory, strDevice, strUser);
                return CompleteResult("OK", "READY", lExpectedSource, lDbSource, lExpectedPlaced, lDbPlaced, lExpectedPoint, lDbPoint, "저장이 확정되었습니다.");
            }
            finally
            {
                if (dtJob != null)
                    dtJob.Dispose();
            }
        }

        /// <summary>Map 헤더를 READY로 바꾼다. 이미 READY면(0행) 그대로 둔다.</summary>
        private void SetMapReady(string strFactory, string strDevice, string strUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_MAP oTQP_GDS_MAP = new DACrux.SEMDMS.DSL.TQP_GDS_MAP();
            oTQP_GDS_MAP.UpdateDataNonQuery("UPDATE_MAP_READY", null, new string[] { strUser, strFactory, strDevice });
        }

        /// <summary>Device의 SOURCE 수 / PLACED 수 / 좌표 수 합계를 DB에서 센다.</summary>
        private void CountMapRows(string strFactory, string strDevice, out long lSource, out long lPlaced, out long lPoint)
        {
            DataTable dtSource = null;
            DataTable dtPlaced = null;
            try
            {
                string[] aKey = new string[] { strFactory, strDevice };
                dtSource = new DACrux.SEMDMS.DSL.TQP_GDS_SOURCE_EL().GetData("SELECT_SOURCE_EL_COUNT", null, aKey);
                dtPlaced = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL().GetData("SELECT_PLACED_EL_COUNT", null, aKey);
                lSource = DACrux.Base.Convert.longParse(dtSource.Rows[0]["SOURCE_COUNT"].ToString());
                lPlaced = DACrux.Base.Convert.longParse(dtPlaced.Rows[0]["PLACED_COUNT"].ToString());
                lPoint = DACrux.Base.Convert.longParse(dtPlaced.Rows[0]["POINT_COUNT"].ToString());
            }
            finally
            {
                if (dtSource != null) dtSource.Dispose();
                if (dtPlaced != null) dtPlaced.Dispose();
            }
        }

        /// <summary>LAYER 배치가 있는지, 종류별 배치 번호가 0부터 빠짐없이 있는지 확인한다. 문제가 없으면 빈 문자열이다.</summary>
        private string CheckBatchNumbers(string strImportJobSeq)
        {
            DataTable dt = null;
            try
            {
                dt = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH().GetData("SELECT_IMPORT_BATCH_SUMMARY", null, new string[] { strImportJobSeq });
                StringBuilder sb = new StringBuilder();
                bool bLayer = false;
                foreach (DataRow dr in dt.Rows)
                {
                    string strKind = dr["STREAM_KIND"].ToString();
                    long lCount = DACrux.Base.Convert.longParse(dr["BATCH_COUNT"].ToString());
                    long lMin = DACrux.Base.Convert.longParse(dr["MIN_NO"].ToString());
                    long lMax = DACrux.Base.Convert.longParse(dr["MAX_NO"].ToString());
                    if (strKind == "LAYER") bLayer = true;
                    if (lMin != 0 || lMax - lMin + 1 != lCount)
                        sb.Append(strKind + " 배치 번호 누락(" + lCount + "건 / " + lMin + "~" + lMax + ") / ");
                }
                if (!bLayer) sb.Insert(0, "LAYER 배치 없음 / ");
                return sb.ToString();
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>CompleteMapImport 반환 1행을 만든다. DB 값을 세지 않은 경우 -1이다.</summary>
        private static DataTable CompleteResult(string strResult, string strJobStatus, long lExpectedSource, long lDbSource,
            long lExpectedPlaced, long lDbPlaced, long lExpectedPoint, long lDbPoint, string strMessage)
        {
            DataTable dt = new DataTable("COMPLETE_RESULT");
            dt.Columns.Add("RESULT", typeof(string));
            dt.Columns.Add("JOB_STATUS", typeof(string));
            dt.Columns.Add("EXPECTED_SOURCE", typeof(long));
            dt.Columns.Add("DB_SOURCE", typeof(long));
            dt.Columns.Add("EXPECTED_PLACED", typeof(long));
            dt.Columns.Add("DB_PLACED", typeof(long));
            dt.Columns.Add("EXPECTED_POINT", typeof(long));
            dt.Columns.Add("DB_POINT", typeof(long));
            dt.Columns.Add("MESSAGE", typeof(string));
            dt.Rows.Add(strResult, strJobStatus, lExpectedSource, lDbSource, lExpectedPlaced, lDbPlaced, lExpectedPoint, lDbPoint, strMessage);
            return dt;
        }

        /// <summary>Device에 UPLOADING 저장 작업 1건을 만든다. Device당 작업은 1건만 허용된다(DB Unique). 작업 번호는 시퀀스로 채번한다.</summary>
        private string CreateImportJob(string strFactory, string strDevice, long lExpectedSource, long lExpectedPlaced, long lExpectedPoint, string strUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
            DataTable dtSeq = oTQP_GDS_IMPORT_JOB.GetData("SELECT_NEXT_IMPORT_JOB_SEQ", null, null);
            string strImportJobSeq = dtSeq.Rows[0][0].ToString();
            dtSeq.Dispose();
            int iRows = oTQP_GDS_IMPORT_JOB.InsertDataNonQuery("CREATE_IMPORT_JOB", null, new string[] {
                strImportJobSeq, strFactory, strDevice,
                lExpectedSource.ToString(CultureInfo.InvariantCulture),
                lExpectedPlaced.ToString(CultureInfo.InvariantCulture),
                lExpectedPoint.ToString(CultureInfo.InvariantCulture),
                strUser, strUser });
            if (iRows != 1)
                throw new DataException("저장 작업(Job)이 한 건 생성되지 않았습니다.");
            return strImportJobSeq;
        }

        #endregion

        #region [TQP_GDS_IMPORT_BATCH]

        /// <summary>이미 Commit된 배치 목록을 조회한다.</summary>
        public DataTable GetImportBatch(string strImportJobSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH oTQP_GDS_IMPORT_BATCH = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH();
            return oTQP_GDS_IMPORT_BATCH.GetData("SELECT_IMPORT_BATCH", null, new string[] { RequiredSeq(strImportJobSeq, "작업 번호") });
        }

        #endregion

        #region [TQP_GDS_LAYER]

        /// <summary>
        /// Layer 배치 하나를 저장한다. Layer 수는 적으므로 같은 키가 있으면 건너뛰는 INSERT를 다건(ExecuteMultiple)으로 실행한다.
        /// 처리 흐름: 작업 상태 확인 -> 입력 검증 -> 서버 해시 계산 -> 이미 저장된 배치인지 확인 -> 행 저장 -> 배치 기록
        /// </summary>
        public int CreateLayerBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            string strUser = Required(strUpdateUser, "Update User", 30);
            string[] aMapKey = GetUploadingMapKey(strImportJobSeq);
            CheckBatchParas(aParas, 3, lBatchNo);
            int iRows = aParas.GetLength(0);
            for (int i = 0; i < iRows; i++)
            {
                ParseInt(aParas[i, 0], "LAYER_ID");
                ParseInt(aParas[i, 1], "COLOR_ARGB");
                if (aParas[i, 2] != null && aParas[i, 2].Length > 100)
                    throw new ArgumentException("DISPLAY_NAME은 100자 이하여야 합니다.", "aParas");
            }

            long lPayloadBytes;
            string strSha256 = ComputeBatchSha256(aParas, out lPayloadBytes);
            if (IsBatchCompleted(strImportJobSeq, "LAYER", lBatchNo, strSha256)) return 0;

            string[,] aLayer = new string[iRows, 8];
            for (int i = 0; i < iRows; i++)
            {
                aLayer[i, 0] = aMapKey[0];
                aLayer[i, 1] = aMapKey[1];
                aLayer[i, 2] = aParas[i, 0].Trim();
                aLayer[i, 3] = aParas[i, 1].Trim();
                aLayer[i, 4] = aParas[i, 2];
                aLayer[i, 5] = aMapKey[0];
                aLayer[i, 6] = aMapKey[1];
                aLayer[i, 7] = aParas[i, 0].Trim();
            }
            DACrux.SEMDMS.DSL.TQP_GDS_LAYER oTQP_GDS_LAYER = new DACrux.SEMDMS.DSL.TQP_GDS_LAYER();
            oTQP_GDS_LAYER.InsertExecuteMultiple("CREATE_LAYER", aLayer);

            CompleteBatch(strImportJobSeq, "LAYER", lBatchNo, strSha256, iRows, 0, lPayloadBytes, 0, 0, 0, strUser);
            return iRows;
        }

        #endregion

        #region [TQP_GDS_SOURCE_EL]

        /// <summary>
        /// 원본 Grid 행 배치 하나를 저장한다.
        /// 처리 흐름:
        ///  1) 작업 상태 / 입력 검증 / 서버 해시 계산 / 이미 저장된 배치인지 확인
        ///  2) Payload 2,000바이트(16진수 4,000자) 이하 행: CREATE_SOURCE_EL_MULTI를 InsertExecuteMultiple로 한 번에 저장.
        ///     ExecuteMultiple은 호출 단위로 원자적이라 중복 키(ORA-00001)가 하나라도 있으면 전체가 롤백된다.
        ///     이 경우(이전 전송이 이미 일부 / 전부 들어간 재전송) 같은 키를 건너뛰는 CREATE_SOURCE_EL로 한 건씩 다시 넣는다.
        ///  3) 2,000바이트 초과 행: BLOB이 필요하므로 CREATE_SOURCE_EL을 InsertDataObject(Execute1)로 한 건씩 저장
        ///  4) 배치 기록 + 수신 SOURCE 건수 누적
        /// </summary>
        public int CreateSourceBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            string strUser = Required(strUpdateUser, "Update User", 30);
            string[] aMapKey = GetUploadingMapKey(strImportJobSeq);
            CheckBatchParas(aParas, 9, lBatchNo);
            int iRows = aParas.GetLength(0);
            List<int> lstSmall = new List<int>();
            List<int> lstLarge = new List<int>();
            for (int i = 0; i < iRows; i++)
            {
                CheckSourceRow(aParas, i);
                if (aParas[i, 8].Length <= MaxMultiHexLength) lstSmall.Add(i);
                else lstLarge.Add(i);
            }

            long lPayloadBytes;
            string strSha256 = ComputeBatchSha256(aParas, out lPayloadBytes);
            if (IsBatchCompleted(strImportJobSeq, "SOURCE", lBatchNo, strSha256)) return 0;

            DACrux.SEMDMS.DSL.TQP_GDS_SOURCE_EL oTQP_GDS_SOURCE_EL = new DACrux.SEMDMS.DSL.TQP_GDS_SOURCE_EL();
            if (lstSmall.Count > 0)
            {
                string[,] aSource = new string[lstSmall.Count, 11];
                for (int k = 0; k < lstSmall.Count; k++)
                {
                    int i = lstSmall[k];
                    aSource[k, 0] = aMapKey[0];
                    aSource[k, 1] = aMapKey[1];
                    for (int c = 0; c < 9; c++)
                        aSource[k, c + 2] = aParas[i, c];
                }
                try
                {
                    oTQP_GDS_SOURCE_EL.InsertExecuteMultiple("CREATE_SOURCE_EL_MULTI", aSource);
                }
                catch (Exception ex)
                {
                    if (!IsDuplicateKey(ex)) throw;
                    foreach (int i in lstSmall)
                        InsertSourceOne(oTQP_GDS_SOURCE_EL, aMapKey, aParas, i);
                }
            }
            foreach (int i in lstLarge)
                InsertSourceOne(oTQP_GDS_SOURCE_EL, aMapKey, aParas, i);

            CompleteBatch(strImportJobSeq, "SOURCE", lBatchNo, strSha256, iRows, 0, lPayloadBytes, iRows, 0, 0, strUser);
            return iRows;
        }

        /// <summary>원본 행 한 건을 BLOB(byte[])으로 저장한다. 같은 키가 이미 있으면 DB에서 건너뛴다.</summary>
        private void InsertSourceOne(DACrux.SEMDMS.DSL.TQP_GDS_SOURCE_EL oTQP_GDS_SOURCE_EL, string[] aMapKey, string[,] aParas, int i)
        {
            oTQP_GDS_SOURCE_EL.InsertDataObject("CREATE_SOURCE_EL", new object[] {
                aMapKey[0], aMapKey[1], aParas[i, 0], aParas[i, 1], DACrux.Base.Convert.longParse(aParas[i, 2]), aParas[i, 3],
                ParseInt(aParas[i, 4], "LAYER_ID"), ParseInt(aParas[i, 5], "DATA_TYPE"), ParseInt(aParas[i, 6], "PARSER_DUPLICATE"),
                ParseInt(aParas[i, 7], "SOURCE_PAYLOAD_VERSION"), HexToBytes(aParas[i, 8]),
                aMapKey[0], aMapKey[1], aParas[i, 0] });
        }

        /// <summary>원본 행 한 건의 형식과 DB 컬럼 길이 / 제약을 확인한다.</summary>
        private static void CheckSourceRow(string[,] aParas, int i)
        {
            string strID = aParas[i, 0];
            if (string.IsNullOrEmpty(strID) || strID.Length > 512)
                throw new ArgumentException((i + 1) + "번째 행의 SOURCE_ELEMENT_ID는 1~512자여야 합니다.", "aParas");
            if (string.IsNullOrEmpty(aParas[i, 1]) || aParas[i, 1].Length > 256)
                throw new ArgumentException((i + 1) + "번째 행의 STRUCTURE_NAME은 1~256자여야 합니다.", "aParas");
            ParseCount(aParas[i, 2], "SOURCE_ORDINAL");
            if (Array.IndexOf(SourceElementTypes, aParas[i, 3]) < 0)
                throw new ArgumentException((i + 1) + "번째 행의 ELEMENT_TYPE이 올바르지 않습니다: " + aParas[i, 3], "aParas");
            ParseInt(aParas[i, 4], "LAYER_ID");
            ParseInt(aParas[i, 5], "DATA_TYPE");
            if (aParas[i, 6] != "0" && aParas[i, 6] != "1")
                throw new ArgumentException((i + 1) + "번째 행의 PARSER_DUPLICATE는 0 또는 1이어야 합니다.", "aParas");
            if (ParseInt(aParas[i, 7], "SOURCE_PAYLOAD_VERSION") <= 0)
                throw new ArgumentException((i + 1) + "번째 행의 SOURCE_PAYLOAD_VERSION은 1 이상이어야 합니다.", "aParas");
            string strHex = aParas[i, 8];
            if (string.IsNullOrEmpty(strHex) || strHex.Length % 2 != 0)
                throw new ArgumentException((i + 1) + "번째 행의 SOURCE_PAYLOAD가 올바른 16진수 문자열이 아닙니다.", "aParas");
            foreach (char ch in strHex)
                if (!Uri.IsHexDigit(ch))
                    throw new ArgumentException((i + 1) + "번째 행의 SOURCE_PAYLOAD가 올바른 16진수 문자열이 아닙니다.", "aParas");
        }

        #endregion

        #region [TQP_GDS_PLACED_EL]

        /// <summary>
        /// 화면 배치 도형 배치 하나를 저장한다. 처리 흐름은 CreateSourceBatch와 같다.
        ///  - 좌표 2,000바이트(16진수 4,000자) 이하이고 TEXT가 4,000바이트 이하인 행: CREATE_PLACED_EL_MULTI + InsertExecuteMultiple.
        ///    중복 키면 CREATE_PLACED_EL(같은 키 건너뛰기)로 한 건씩 재시도.
        ///  - 그 밖의 행(큰 좌표 / 긴 TEXT): CREATE_PLACED_EL + InsertDataObject(BLOB / CLOB) 한 건씩.
        ///  - 배치 기록의 POINT_COUNT와 작업의 수신 PLACED / 좌표 수를 함께 누적한다.
        /// </summary>
        public int CreatePlacedBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            string strUser = Required(strUpdateUser, "Update User", 30);
            string[] aMapKey = GetUploadingMapKey(strImportJobSeq);
            CheckBatchParas(aParas, 20, lBatchNo);
            int iRows = aParas.GetLength(0);
            long lPoints = 0;
            List<int> lstSmall = new List<int>();
            List<int> lstLarge = new List<int>();
            for (int i = 0; i < iRows; i++)
            {
                lPoints += CheckPlacedRow(aParas, i);
                bool bSmallText = aParas[i, 15] == null || Encoding.UTF8.GetByteCount(aParas[i, 15]) <= 4000;
                if (aParas[i, 14].Length <= MaxMultiHexLength && bSmallText) lstSmall.Add(i);
                else lstLarge.Add(i);
            }

            long lPayloadBytes;
            string strSha256 = ComputeBatchSha256(aParas, out lPayloadBytes);
            if (IsBatchCompleted(strImportJobSeq, "PLACED", lBatchNo, strSha256)) return 0;

            DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL oTQP_GDS_PLACED_EL = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL();
            if (lstSmall.Count > 0)
            {
                string[,] aPlaced = new string[lstSmall.Count, 22];
                for (int k = 0; k < lstSmall.Count; k++)
                {
                    int i = lstSmall[k];
                    aPlaced[k, 0] = aMapKey[0];
                    aPlaced[k, 1] = aMapKey[1];
                    for (int c = 0; c < 20; c++)
                        aPlaced[k, c + 2] = aParas[i, c];
                }
                try
                {
                    oTQP_GDS_PLACED_EL.InsertExecuteMultiple("CREATE_PLACED_EL_MULTI", aPlaced);
                }
                catch (Exception ex)
                {
                    if (!IsDuplicateKey(ex)) throw;
                    foreach (int i in lstSmall)
                        InsertPlacedOne(oTQP_GDS_PLACED_EL, aMapKey, aParas, i);
                }
            }
            foreach (int i in lstLarge)
                InsertPlacedOne(oTQP_GDS_PLACED_EL, aMapKey, aParas, i);

            CompleteBatch(strImportJobSeq, "PLACED", lBatchNo, strSha256, iRows, lPoints, lPayloadBytes, 0, iRows, lPoints, strUser);
            return iRows;
        }

        /// <summary>배치 도형 한 건을 BLOB / CLOB으로 저장한다. 같은 키가 이미 있으면 DB에서 건너뛴다.</summary>
        private void InsertPlacedOne(DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL oTQP_GDS_PLACED_EL, string[] aMapKey, string[,] aParas, int i)
        {
            oTQP_GDS_PLACED_EL.InsertDataObject("CREATE_PLACED_EL", new object[] {
                aMapKey[0], aMapKey[1], aParas[i, 0], aParas[i, 1], ParseInt(aParas[i, 2], "LAYER_ID"), ParseInt(aParas[i, 3], "DATA_TYPE"),
                aParas[i, 4], ParseInt(aParas[i, 5], "CLOSED_FLAG"), NullableText(aParas[i, 6]), NullableInt(aParas[i, 7], "PATH_TYPE"),
                aParas[i, 8], aParas[i, 9], aParas[i, 10], aParas[i, 11],
                ParseInt(aParas[i, 12], "POINT_COUNT"), ParseInt(aParas[i, 13], "POINTS_FORMAT_VERSION"), HexToBytes(aParas[i, 14]),
                NullableText(aParas[i, 15]), NullableInt(aParas[i, 16], "TEXT_TYPE"), NullableInt(aParas[i, 17], "TEXT_FONT"),
                NullableText(aParas[i, 18]), NullableText(aParas[i, 19]),
                aMapKey[0], aMapKey[1], aParas[i, 0] });
        }

        /// <summary>
        /// 배치 도형 한 건의 형식과 DB 제약(종류 / 닫힘 / PATH_TYPE / Bounds / 좌표 수와 바이트 수)을 확인하고 좌표 수를 반환한다.
        /// 빈 문자열은 null로 바꿔 DB에 NULL로 들어가게 한다.
        /// </summary>
        private static int CheckPlacedRow(string[,] aParas, int i)
        {
            for (int c = 6; c < 20; c++)
                if (aParas[i, c] != null && aParas[i, c].Length == 0) aParas[i, c] = null;
            string strRow = (i + 1) + "번째 행의 ";
            if (string.IsNullOrEmpty(aParas[i, 0]) || Encoding.UTF8.GetByteCount(aParas[i, 0]) > 1024)
                throw new ArgumentException(strRow + "PLACED_ELEMENT_ID는 1~1024바이트여야 합니다.", "aParas");
            if (string.IsNullOrEmpty(aParas[i, 1]) || aParas[i, 1].Length > 512)
                throw new ArgumentException(strRow + "SOURCE_ELEMENT_ID는 1~512자여야 합니다.", "aParas");
            ParseInt(aParas[i, 2], "LAYER_ID");
            ParseInt(aParas[i, 3], "DATA_TYPE");
            string strType = aParas[i, 4];
            if (strType != "BOUNDARY" && strType != "PATH" && strType != "TEXT")
                throw new ArgumentException(strRow + "ELEMENT_TYPE이 올바르지 않습니다: " + strType, "aParas");
            if (aParas[i, 5] != "0" && aParas[i, 5] != "1")
                throw new ArgumentException(strRow + "CLOSED_FLAG는 0 또는 1이어야 합니다.", "aParas");
            if (aParas[i, 6] != null) ParseFinite(aParas[i, 6], "PATH_WIDTH");
            if (strType == "PATH")
            {
                int iPathType = ParseInt(aParas[i, 7], "PATH_TYPE");
                if (iPathType < 0 || iPathType > 255)
                    throw new ArgumentException(strRow + "PATH_TYPE은 0~255여야 합니다.", "aParas");
            }
            else if (aParas[i, 7] != null)
                throw new ArgumentException(strRow + "PATH가 아닌 도형은 PATH_TYPE이 없어야 합니다.", "aParas");
            double dMinX = ParseFinite(aParas[i, 8], "MIN_X"), dMinY = ParseFinite(aParas[i, 9], "MIN_Y");
            double dMaxX = ParseFinite(aParas[i, 10], "MAX_X"), dMaxY = ParseFinite(aParas[i, 11], "MAX_Y");
            if (dMinX > dMaxX || dMinY > dMaxY)
                throw new ArgumentException(strRow + "Bounds의 최소값이 최대값보다 큽니다.", "aParas");
            int iPointCount = ParseInt(aParas[i, 12], "POINT_COUNT");
            if (iPointCount <= 0)
                throw new ArgumentException(strRow + "POINT_COUNT는 1 이상이어야 합니다.", "aParas");
            if (ParseInt(aParas[i, 13], "POINTS_FORMAT_VERSION") <= 0)
                throw new ArgumentException(strRow + "POINTS_FORMAT_VERSION은 1 이상이어야 합니다.", "aParas");
            string strHex = aParas[i, 14];
            if (strHex == null || (long)strHex.Length != (long)iPointCount * 32)
                throw new ArgumentException(strRow + "좌표 16진수 길이가 POINT_COUNT x 16바이트와 다릅니다.", "aParas");
            foreach (char ch in strHex)
                if (!Uri.IsHexDigit(ch))
                    throw new ArgumentException(strRow + "좌표가 올바른 16진수 문자열이 아닙니다.", "aParas");
            if (aParas[i, 16] != null) ParseInt(aParas[i, 16], "TEXT_TYPE");
            if (aParas[i, 17] != null) ParseInt(aParas[i, 17], "TEXT_FONT");
            if ((aParas[i, 18] != null && aParas[i, 18].Length > 20) || (aParas[i, 19] != null && aParas[i, 19].Length > 20))
                throw new ArgumentException(strRow + "TEXT 정렬 값은 20자 이하여야 합니다.", "aParas");
            return iPointCount;
        }

        private static object NullableText(string strValue)
        {
            return strValue == null ? (object)DBNull.Value : strValue;
        }

        private static object NullableInt(string strValue, string strName)
        {
            return strValue == null ? (object)DBNull.Value : ParseInt(strValue, strName);
        }

        /// <summary>좌표 / 폭 문자열(G17)이 유한한 double인지 확인한다. DB에는 문자열 그대로 넘겨 TO_BINARY_DOUBLE로 정확히 변환한다.</summary>
        private static double ParseFinite(string strValue, string strName)
        {
            double dValue;
            if (!double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue)
                || double.IsNaN(dValue) || double.IsInfinity(dValue))
                throw new ArgumentException(strName + " 값이 유한한 숫자가 아닙니다: " + strValue, strName);
            return dValue;
        }

        #endregion

        #region [Batch Common]

        /// <summary>한 번에 받을 수 있는 배치 행 수. Remoting 요청 크기와 DB 배열 바인딩 크기를 제한한다.</summary>
        private const int MaxBatchRows = 5000;
        /// <summary>HEXTORAW 다건 저장이 가능한 16진수 길이(RAW 2,000바이트). 넘으면 BLOB 한 건 저장으로 처리한다.</summary>
        private const int MaxMultiHexLength = 4000;
        private static readonly string[] SourceElementTypes = { "BOUNDARY", "PATH", "TEXT", "SREF", "AREF" };

        /// <summary>작업이 있고 UPLOADING 상태인지 확인한 뒤 Map 키(0 FACTORY / 1 DEVICE_ID)를 돌려준다. 행의 Map 키는 클라이언트가 아니라 서버가 채운다.</summary>
        private string[] GetUploadingMapKey(string strImportJobSeq)
        {
            DataTable dt = null;
            try
            {
                dt = GetImportJob(strImportJobSeq);
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("저장 작업을 찾을 수 없습니다: " + strImportJobSeq);
                string strStatus = dt.Rows[0]["JOB_STATUS"].ToString();
                if (!string.Equals(strStatus, "UPLOADING", StringComparison.Ordinal))
                    throw new InvalidOperationException("저장 작업이 " + strStatus + " 상태라서 배치를 받을 수 없습니다.");
                return new string[] { dt.Rows[0]["FACTORY"].ToString(), dt.Rows[0]["DEVICE_ID"].ToString() };
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>배치 번호 / 행 수 / 열 수를 확인한다.</summary>
        private static void CheckBatchParas(string[,] aParas, int iColumns, long lBatchNo)
        {
            if (lBatchNo < 0)
                throw new ArgumentException("배치 번호는 0 이상이어야 합니다.", "lBatchNo");
            if (aParas == null || aParas.GetLength(0) == 0)
                throw new ArgumentException("배치에 행이 없습니다.", "aParas");
            if (aParas.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("배치 행 수는 " + MaxBatchRows + "건 이하여야 합니다.", "aParas");
            if (aParas.GetLength(1) != iColumns)
                throw new ArgumentException("배치 열 수는 " + iColumns + "개여야 합니다.", "aParas");
        }

        /// <summary>
        /// 배치 내용으로 SHA-256을 계산한다. 같은 배치 번호의 재전송이 같은 내용인지 서버가 직접 판단하기 위한 값이다.
        /// 형식: 열 수 + 각 셀 UTF-8(null은 \u0000) + 셀 구분 \u001F + 행 구분 \u001E. lPayloadBytes는 이 바이트 수다.
        /// </summary>
        private static string ComputeBatchSha256(string[,] aParas, out long lPayloadBytes)
        {
            int iRows = aParas.GetLength(0);
            int iColumns = aParas.GetLength(1);
            lPayloadBytes = 0;
            using (System.Security.Cryptography.SHA256 oSha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] aHeader = Encoding.UTF8.GetBytes(iColumns.ToString(CultureInfo.InvariantCulture) + "\u001E");
                oSha.TransformBlock(aHeader, 0, aHeader.Length, null, 0);
                lPayloadBytes += aHeader.Length;
                for (int i = 0; i < iRows; i++)
                {
                    for (int c = 0; c < iColumns; c++)
                    {
                        byte[] aCell = Encoding.UTF8.GetBytes((aParas[i, c] ?? "\u0000") + (c == iColumns - 1 ? "\u001E" : "\u001F"));
                        oSha.TransformBlock(aCell, 0, aCell.Length, null, 0);
                        lPayloadBytes += aCell.Length;
                    }
                }
                oSha.TransformFinalBlock(new byte[0], 0, 0);
                StringBuilder sb = new StringBuilder(64);
                foreach (byte b in oSha.Hash)
                    sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        /// <summary>같은 (작업, 종류, 번호) 배치가 이미 저장되어 있으면 true. 내용(해시)이 다르면 오류다.</summary>
        private bool IsBatchCompleted(string strImportJobSeq, string strStreamKind, long lBatchNo, string strSha256)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH oTQP_GDS_IMPORT_BATCH = null;
            DataTable dt = null;
            try
            {
                oTQP_GDS_IMPORT_BATCH = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH();
                dt = oTQP_GDS_IMPORT_BATCH.GetData("SELECT_IMPORT_BATCH_ONE", null,
                    new string[] { strImportJobSeq, strStreamKind, lBatchNo.ToString(CultureInfo.InvariantCulture) });
                if (dt.Rows.Count == 0) return false;
                if (!string.Equals(dt.Rows[0]["PAYLOAD_SHA256"].ToString().Trim(), strSha256, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(strStreamKind + " 배치 " + lBatchNo + "번이 다른 내용으로 이미 저장되어 있습니다.");
                return true;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>
        /// 행 저장이 끝난 배치를 기록하고 작업의 수신 건수를 누적한다.
        /// 배치 기록이 동시에 먼저 생겼다면(중복 키) 같은 내용인지만 확인하고 건수는 다시 더하지 않는다.
        /// </summary>
        private void CompleteBatch(string strImportJobSeq, string strStreamKind, long lBatchNo, string strSha256, int iItemCount, long lPointCount,
            long lPayloadBytes, long lReceivedSource, long lReceivedPlaced, long lReceivedPoint, string strUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH oTQP_GDS_IMPORT_BATCH = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_BATCH();
            try
            {
                oTQP_GDS_IMPORT_BATCH.InsertDataNonQuery("CREATE_IMPORT_BATCH", null, new string[] {
                    strImportJobSeq, strStreamKind, lBatchNo.ToString(CultureInfo.InvariantCulture), "1", strSha256,
                    iItemCount.ToString(CultureInfo.InvariantCulture), lPointCount.ToString(CultureInfo.InvariantCulture),
                    lPayloadBytes.ToString(CultureInfo.InvariantCulture) });
            }
            catch (Exception ex)
            {
                if (!IsDuplicateKey(ex) || !IsBatchCompleted(strImportJobSeq, strStreamKind, lBatchNo, strSha256)) throw;
                return;
            }

            if (lReceivedSource == 0 && lReceivedPlaced == 0 && lReceivedPoint == 0) return;
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
            oTQP_GDS_IMPORT_JOB.UpdateDataNonQuery("UPDATE_IMPORT_JOB_RECEIVED", null, new string[] {
                lReceivedSource.ToString(CultureInfo.InvariantCulture), lReceivedPlaced.ToString(CultureInfo.InvariantCulture),
                lReceivedPoint.ToString(CultureInfo.InvariantCulture), strUser, strImportJobSeq });
        }

        /// <summary>Oracle 중복 키 오류(ORA-00001, 배열 실행이면 ORA-24381 안에 포함)인지 확인한다.</summary>
        private static bool IsDuplicateKey(Exception ex)
        {
            for (Exception e = ex; e != null; e = e.InnerException)
                if (e.Message != null && e.Message.Contains("ORA-00001"))
                    return true;
            return false;
        }

        /// <summary>16진수 문자열을 byte[]로 바꾼다. 형식은 CheckSourceRow에서 확인한다.</summary>
        private static byte[] HexToBytes(string strHex)
        {
            byte[] aBytes = new byte[strHex.Length / 2];
            for (int i = 0; i < aBytes.Length; i++)
                aBytes[i] = (byte)((Uri.FromHex(strHex[i * 2]) << 4) | Uri.FromHex(strHex[i * 2 + 1]));
            return aBytes;
        }

        #endregion

        #region [Validation]

        /// <summary>정수 문자열을 int로 바꾼다.</summary>
        private static int ParseInt(string strValue, string strName)
        {
            int iValue;
            if (!int.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out iValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return iValue;
        }

        /// <summary>건수 문자열을 0 이상의 정수로 바꾼다.</summary>
        private static long ParseCount(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue < 0)
                throw new ArgumentException(strName + "는 0 이상의 정수여야 합니다.", strName);
            return lValue;
        }

        /// <summary>단위 문자열(클라이언트가 G17 / 소수 표기로 전달)을 double로 바꾼다. 범위 검증은 BeginMapImport가 한다.</summary>
        private static double ParseUnit(string strValue, string strName)
        {
            double dValue;
            if (!double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue))
                throw new ArgumentException(strName + " 값이 숫자가 아닙니다.", strName);
            return dValue;
        }

        /// <summary>시퀀스 번호(작업 번호) 문자열이 1 이상의 정수인지 확인하고 정리한 값을 돌려준다.</summary>
        private static string RequiredSeq(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(strValue == null ? null : strValue.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue <= 0)
                throw new ArgumentException(strName + "는 1 이상의 정수여야 합니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
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
