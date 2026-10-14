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
    /// 리비전을 관리하지 않는다(2026-10-02). Map은 Factory + Device(제품)당 1개(TQP_GDS_MAP)이고,
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
        /// 처리 순서: CHAIN(하위 Chain 테이블은 FK CASCADE) -> PLACED_EL -> 헤더 TQP_GDS_MAP(LAYER / IMPORT_JOB -> IMPORT_BATCH는 FK CASCADE).
        /// 큰 테이블 사이 FK(PLACED -> CHAIN_MEMBER / EDIT 등)는 CASCADE가 아니다. 부모 행마다 내부 DELETE가 돌아
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
            new DACrux.SEMDMS.DSL.TQP_GDS_MAP().DeleteDataNonQuery("DELETE_MAP", null, aKey);
            return iPlaced;
        }

        #endregion

        #region [TQP_GDS_MAP_SUB_DEVICE]

        /// <summary>
        /// 대표 Device의 Sub Device 목록을 통째로 바꾼다(2026-10-12).
        /// 처리: 입력 검증 -> 대표 Map 확인 -> 입력 정리(빈 값 / 대표 자신 제외, 대소문자만 다른 중복 오류)
        ///       -> 다른 대표에 등록된 Sub / 다른 Map의 대표 Device 이름 오류 -> 기존 삭제 -> 일괄 등록(InsertExecuteMultiple 1회).
        /// 검사를 모두 끝낸 뒤 지우므로 검사 오류로 기존 설정이 사라지지 않는다. 반환: 저장한 Sub 수
        /// </summary>
        public int SaveSubDeviceList(string strFactory, string strDeviceID, string[] aSubDeviceIDs, string[] aRemarks, string strUpdateUser)
        {
            string strFac = Required(strFactory, "Factory", 30);
            string strDevice = Required(strDeviceID, "Device ID", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aSubDeviceIDs == null) aSubDeviceIDs = new string[0];
            if (aRemarks != null && aRemarks.Length != aSubDeviceIDs.Length)
                throw new ArgumentException("Sub Device 와 비고 개수가 다릅니다.", "aRemarks");

            DataTable dtMap = GetMapInfo(strFac, strDevice);
            if (dtMap.Rows.Count == 0)
                throw new InvalidOperationException("대표 Device 의 GDS Map 이 없습니다: " + strFac + " / " + strDevice);
            strDevice = dtMap.Rows[0]["DEVICE_ID"].ToString();

            // 입력 정리: 대표 자신은 언제나 포함이라 저장하지 않는다.
            var lstSub = new List<string[]>();
            var setUpper = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < aSubDeviceIDs.Length; i++)
            {
                string strSub = aSubDeviceIDs[i] == null ? string.Empty : aSubDeviceIDs[i].Trim();
                if (strSub.Length == 0 || string.Equals(strSub, strDevice, StringComparison.OrdinalIgnoreCase)) continue;
                Required(strSub, "Sub Device ID", 50);
                string strRemark = aRemarks == null || aRemarks[i] == null ? string.Empty : aRemarks[i].Trim();
                if (strRemark.Length > 200) throw new ArgumentException("비고는 200자 이하여야 합니다: " + strSub, "aRemarks");
                if (!setUpper.Add(strSub.ToUpperInvariant()))
                    throw new ArgumentException("Sub Device 가 중복되었습니다(대소문자 무시): " + strSub, "aSubDeviceIDs");
                lstSub.Add(new string[] { strSub, strRemark });
            }

            // 다른 대표에 이미 등록된 Sub 확인
            var lstConflict = new List<string>();
            DataTable dtAll = new DACrux.SEMDMS.DSL.TQP_GDS_MAP_SUB_DEVICE().GetData("SELECT_SUB_DEVICE_FACTORY", null, new string[] { strFac });
            foreach (DataRow row in dtAll.Rows)
            {
                string strOwner = row["DEVICE_ID"].ToString();
                if (strOwner == strDevice) continue;
                string strSub = row["SUB_DEVICE_ID"].ToString();
                if (setUpper.Contains(strSub.ToUpperInvariant()))
                    lstConflict.Add(strSub + " (대표 " + strOwner + ")");
            }
            // 다른 Map 의 대표 Device 이름이면 어느 Map 을 쓸지 모호해진다.
            DataTable dtMaps = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_DEVICE_LIST", null, null);
            foreach (DataRow row in dtMaps.Rows)
            {
                if (row["FACTORY"].ToString() != strFac) continue;
                string strMapDevice = row["DEVICE_ID"].ToString();
                if (strMapDevice != strDevice && setUpper.Contains(strMapDevice.ToUpperInvariant()))
                    lstConflict.Add(strMapDevice + " (자기 GDS Map 이 있는 대표 Device)");
            }
            if (lstConflict.Count > 0)
                throw new InvalidOperationException("다른 대표에 쓰이는 이름이라 저장할 수 없습니다: " + string.Join(", ", lstConflict.ToArray()));

            DACrux.SEMDMS.DSL.TQP_GDS_MAP_SUB_DEVICE oSub = new DACrux.SEMDMS.DSL.TQP_GDS_MAP_SUB_DEVICE();
            oSub.DeleteDataNonQuery("DELETE_SUB_DEVICE_BY_MAP", null, new string[] { strFac, strDevice });
            if (lstSub.Count == 0) return 0;
            string[,] aParas = new string[lstSub.Count, 5];
            for (int i = 0; i < lstSub.Count; i++)
            {
                aParas[i, 0] = strFac;
                aParas[i, 1] = lstSub[i][0];
                aParas[i, 2] = strDevice;
                aParas[i, 3] = lstSub[i][1];
                aParas[i, 4] = strUser;
            }
            oSub.InsertExecuteMultiple("CREATE_SUB_DEVICE", aParas);
            return lstSub.Count;
        }

        #endregion

        #region [TQP_GDS_IMPORT_JOB]

        /// <summary>
        /// 저장을 시작한다. Device에 기존 Map이 있으면 거부한다(화면이 먼저 사용자에게 묻고 DeleteMap을 호출해야 한다).
        /// ImportInfo 순서: 0 Factory / 1 Device ID / 2 Top Structure / 3 GDS 파일 이름 / 4 사용 안 함(예전 GDS SHA-256 자리, 2026-10-08 컬럼 제거로 무시) /
        /// 5 User Unit / 6 Database Unit / 7 Create User / 8 사용 안 함(예전 원본 행 수 자리, 2026-10-12 원본 행 테이블 제거) / 9 예상 PLACED 수 / 10 예상 좌표 수
        /// [11 저장 기준 VIEW_SIDE: FRONT / BACK] (2026-10-06 추가). 11개로 부르면 FRONT(기존 호출 / Probe 호환).
        ///   BACK = 화면이 PLACED 좌표를 GDS 원점 기준 좌우 반전(X' = -X)해 보낸다는 기록이다. 서버는 좌표를 바꾸지 않는다.
        /// 처리: 입력 검증 -> 기존 Map 확인 -> TQP_GDS_MAP(DRAFT) 생성 -> 저장 작업(UPLOADING) 생성 -> 작업 1행 반환
        /// </summary>
        public DataTable BeginMapImport(string[] ImportInfo)
        {
            if (ImportInfo == null || (ImportInfo.Length != 11 && ImportInfo.Length != 12))
                throw new ArgumentException("저장 시작 정보는 11개 또는 12개(저장 기준 포함) 항목이어야 합니다.", "ImportInfo");
            string strFactory = Required(ImportInfo[0], "Factory", 30);
            string strDevice = Required(ImportInfo[1], "Device ID", 50);
            string strTop = Required(ImportInfo[2], "Top Structure", 1024);
            string strFileName = ImportInfo[3] == null ? string.Empty : ImportInfo[3].Trim();
            if (strFileName.Length > 260) strFileName = strFileName.Substring(strFileName.Length - 260);
            // ImportInfo[4](예전 GDS SHA-256 자리, 2026-10-08 컬럼 제거)와 ImportInfo[8](예전 원본 행 수 자리, 2026-10-12 원본 행 테이블 제거)은 사용하지 않는다.
            // 항목 위치를 당기면 이미 배포된 화면 / Probe와 순서가 어긋나므로 자리는 남기고 값이 와도 무시한다.
            double dUserUnit = ParseUnit(ImportInfo[5], "User Unit");
            double dDatabaseUnit = ParseUnit(ImportInfo[6], "Database Unit");
            if (double.IsNaN(dUserUnit) || double.IsNaN(dDatabaseUnit) || dUserUnit <= 0 || dDatabaseUnit <= 0 || double.IsInfinity(dUserUnit) || double.IsInfinity(dDatabaseUnit))
                throw new ArgumentException("GDS 좌표 단위는 0보다 큰 유한한 숫자여야 합니다.", "ImportInfo");
            string strCreateUser = Required(ImportInfo[7], "Create User", 30);
            long lExpectedPlaced = ParseCount(ImportInfo[9], "예상 PLACED 수");
            long lExpectedPoint = ParseCount(ImportInfo[10], "예상 좌표 수");
            string strViewSide = ImportInfo.Length == 12 && ImportInfo[11] != null ? ImportInfo[11].Trim().ToUpperInvariant() : "FRONT";
            if (strViewSide != "FRONT" && strViewSide != "BACK")
                throw new ArgumentException("저장 기준은 FRONT 또는 BACK이어야 합니다: " + ImportInfo[11], "ImportInfo");

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
                strFactory, strDevice, strTop, strFileName,
                dUserUnit.ToString("G17", CultureInfo.InvariantCulture), dDatabaseUnit.ToString("G17", CultureInfo.InvariantCulture),
                strViewSide, strCreateUser });
            if (iRows != 1)
                throw new DataException("GDS Map 헤더가 한 건 생성되지 않았습니다.");
            string strImportJobSeq = CreateImportJob(strFactory, strDevice, lExpectedPlaced, lExpectedPoint, strCreateUser);
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
        ///  2) DB에서 PLACED 수 / 좌표 수 합계를 다시 센다. 배치 저장은 Bulk Insert만 하므로(배치 기록 없음) 이 DB 건수가 유일한 확인이다.
        ///  3) (2026-10-06 삭제) 배치 번호 확인은 배치 기록을 남기지 않게 되어 하지 않는다.
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
                long lExpectedPlaced = DACrux.Base.Convert.longParse(drJob["EXPECTED_PLACED_COUNT"].ToString());
                long lExpectedPoint = DACrux.Base.Convert.longParse(drJob["EXPECTED_POINT_COUNT"].ToString());

                if (string.Equals(strStatus, "READY", StringComparison.Ordinal))
                {
                    SetMapReady(strFactory, strDevice, strUser);
                    return CompleteResult("OK", "READY", lExpectedPlaced, -1, lExpectedPoint, -1, "이미 확정된 저장 작업입니다.");
                }
                if (!string.Equals(strStatus, "UPLOADING", StringComparison.Ordinal))
                    throw new InvalidOperationException("저장 작업이 " + strStatus + " 상태라서 확정할 수 없습니다.");

                long lDbPlaced, lDbPoint;
                CountMapRows(strFactory, strDevice, out lDbPlaced, out lDbPoint);
                StringBuilder sbMessage = new StringBuilder();
                if (lDbPlaced != lExpectedPlaced) sbMessage.Append("PLACED 건수 불일치 / ");
                if (lDbPoint != lExpectedPoint) sbMessage.Append("좌표 수 불일치 / ");
                if (sbMessage.Length > 0)
                    return CompleteResult("MISMATCH", strStatus, lExpectedPlaced, lDbPlaced,
                        lExpectedPoint, lDbPoint, sbMessage.ToString().TrimEnd(' ', '/'));

                DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
                int iRows = oTQP_GDS_IMPORT_JOB.UpdateDataNonQuery("UPDATE_IMPORT_JOB_READY", null, new string[] {
                    lDbPlaced.ToString(CultureInfo.InvariantCulture),
                    lDbPoint.ToString(CultureInfo.InvariantCulture), strUser, strImportJobSeq });
                if (iRows != 1)
                    throw new InvalidOperationException("저장 작업 상태가 바뀌어 확정하지 못했습니다. 다시 조회하세요.");
                SetMapReady(strFactory, strDevice, strUser);
                return CompleteResult("OK", "READY", lExpectedPlaced, lDbPlaced, lExpectedPoint, lDbPoint, "저장이 확정되었습니다.");
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

        /// <summary>Device의 PLACED 수 / 좌표 수 합계를 DB에서 센다.</summary>
        private void CountMapRows(string strFactory, string strDevice, out long lPlaced, out long lPoint)
        {
            DataTable dtPlaced = null;
            try
            {
                string[] aKey = new string[] { strFactory, strDevice };
                dtPlaced = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL().GetData("SELECT_PLACED_EL_COUNT", null, aKey);
                lPlaced = DACrux.Base.Convert.longParse(dtPlaced.Rows[0]["PLACED_COUNT"].ToString());
                lPoint = DACrux.Base.Convert.longParse(dtPlaced.Rows[0]["POINT_COUNT"].ToString());
            }
            finally
            {
                if (dtPlaced != null) dtPlaced.Dispose();
            }
        }

        /// <summary>CompleteMapImport 반환 1행을 만든다. DB 값을 세지 않은 경우 -1이다.</summary>
        private static DataTable CompleteResult(string strResult, string strJobStatus,
            long lExpectedPlaced, long lDbPlaced, long lExpectedPoint, long lDbPoint, string strMessage)
        {
            DataTable dt = new DataTable("COMPLETE_RESULT");
            dt.Columns.Add("RESULT", typeof(string));
            dt.Columns.Add("JOB_STATUS", typeof(string));
            dt.Columns.Add("EXPECTED_PLACED", typeof(long));
            dt.Columns.Add("DB_PLACED", typeof(long));
            dt.Columns.Add("EXPECTED_POINT", typeof(long));
            dt.Columns.Add("DB_POINT", typeof(long));
            dt.Columns.Add("MESSAGE", typeof(string));
            dt.Rows.Add(strResult, strJobStatus, lExpectedPlaced, lDbPlaced, lExpectedPoint, lDbPoint, strMessage);
            return dt;
        }

        /// <summary>Device에 UPLOADING 저장 작업 1건을 만든다. Device당 작업은 1건만 허용된다(DB Unique). 작업 번호는 시퀀스로 채번한다.</summary>
        private string CreateImportJob(string strFactory, string strDevice, long lExpectedPlaced, long lExpectedPoint, string strUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB oTQP_GDS_IMPORT_JOB = new DACrux.SEMDMS.DSL.TQP_GDS_IMPORT_JOB();
            DataTable dtSeq = oTQP_GDS_IMPORT_JOB.GetData("SELECT_NEXT_IMPORT_JOB_SEQ", null, null);
            string strImportJobSeq = dtSeq.Rows[0][0].ToString();
            dtSeq.Dispose();
            int iRows = oTQP_GDS_IMPORT_JOB.InsertDataNonQuery("CREATE_IMPORT_JOB", null, new string[] {
                strImportJobSeq, strFactory, strDevice,
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
        /// Layer 배치 하나를 저장한다. DefectDefine.CreateDefect와 같은 Bulk Insert 방식(InsertExecuteMultiple 1회)이다.
        /// 처리 흐름: 작업 상태 확인(Map 키) -> 배열 크기 확인 -> 행 저장. 행 검증 / 배치 기록은 하지 않고 건수는 CompleteMapImport가 DB에서 확인한다.
        /// </summary>
        public int CreateLayerBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            string[] aMapKey = GetUploadingMapKey(strImportJobSeq);
            CheckBatchParas(aParas, 3, lBatchNo);
            int iRows = aParas.GetLength(0);

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
            return iRows;
        }

        #endregion

        #region [TQP_GDS_PLACED_EL]

        /// <summary>
        /// 화면 배치 도형 배치 하나를 저장한다. DefectDefine.CreateDefect와 같은 Bulk Insert 방식이다.
        ///  - 좌표 2,000바이트(16진수 4,000자) 이하이고 TEXT가 4,000바이트 이하인 행: CREATE_PLACED_EL_MULTI + InsertExecuteMultiple 1회.
        ///  - 그 밖의 행(큰 좌표 / 긴 TEXT): CREATE_PLACED_EL + InsertDataObject(BLOB / CLOB) 한 건씩.
        ///  빈 값은 클라이언트 GdsMapPlacedRowBuilder가 null로 보낸다.
        /// </summary>
        public int CreatePlacedBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            string[] aMapKey = GetUploadingMapKey(strImportJobSeq);
            CheckBatchParas(aParas, 20, lBatchNo);
            int iRows = aParas.GetLength(0);
            List<int> lstSmall = new List<int>(iRows);
            List<int> lstLarge = new List<int>();
            for (int i = 0; i < iRows; i++)
            {
                bool bSmallText = aParas[i, 15] == null || Encoding.UTF8.GetByteCount(aParas[i, 15]) <= 4000;
                if (aParas[i, 14].Length <= MaxMultiHexLength && bSmallText) lstSmall.Add(i);
                else lstLarge.Add(i);
            }

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
                oTQP_GDS_PLACED_EL.InsertExecuteMultiple("CREATE_PLACED_EL_MULTI", aPlaced);
            }
            foreach (int i in lstLarge)
                InsertPlacedOne(oTQP_GDS_PLACED_EL, aMapKey, aParas, i);
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

        private static object NullableText(string strValue)
        {
            return strValue == null ? (object)DBNull.Value : strValue;
        }

        private static object NullableInt(string strValue, string strName)
        {
            return strValue == null ? (object)DBNull.Value : ParseInt(strValue, strName);
        }

        #endregion

        #region [Batch Common]

        /// <summary>한 번에 받을 수 있는 배치 행 수. Remoting 요청 크기와 DB 배열 바인딩 크기를 제한한다.</summary>
        private const int MaxBatchRows = 10000;
        /// <summary>HEXTORAW 다건 저장이 가능한 16진수 길이(RAW 2,000바이트). 넘으면 BLOB 한 건 저장으로 처리한다.</summary>
        private const int MaxMultiHexLength = 4000;

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

        /// <summary>16진수 문자열을 byte[]로 바꾼다. 클라이언트 행 생성기가 만든 형식을 그대로 믿는다.</summary>
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
