using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// GDS Map 전체 도형을 DB에 저장(Import)하는 원격 호출 계약이다.
    /// 설계: 문서/설계/02_GDS/2026-10-02_GDS_Map_DB저장_설계.md
    /// 큰 도면을 한 번에 보내지 않고 저장 작업(Job)을 만든 뒤 LAYER -> PLACED 배치로 나눠 보낸다(원본 행 테이블은 2026-10-12 제거).
    /// 2026-10-02: 리비전 관리 없음. Map은 Factory + Device당 1개(TQP_GDS_MAP)이고 다시 등록하려면 DeleteMap 후 등록한다.
    /// 이번 단계는 Job 시작 / 조회 / 취소와 완료 배치 조회만 제공한다. 배치 업로드와 완료 확정은 다음 단계에서 추가한다.
    /// </summary>
    public interface iGdsMapImport
    {
        #region [TQP_GDS_MAP]

        /// <summary>
        /// Factory + Device(제품)의 기존 GDS Map 헤더 / 저장 작업 상태 / 건수를 조회한다. 없으면 0행이다.
        /// 리비전 관리 없음, Device당 Map 1개(2026-10-02). 화면은 저장 전에 이 결과를 보여 주고 삭제 후 재등록 여부를 묻는다.
        /// 반환 열: FACTORY / DEVICE_ID / TOP_STRUCTURE / GDS_FILE_NAME / MAP_STATUS / VIEW_SIDE(2026-10-06) / CREATE_USER / CREATE_TIME /
        ///          IMPORT_JOB_SEQ / JOB_STATUS / PLACED_COUNT / POINT_COUNT
        /// </summary>
        DataTable GetMapInfo(string strFactory, string strDeviceID);

        /// <summary>Device의 GDS 데이터(헤더 / Layer / 배치 도형 / 저장 작업 / Chain)를 모두 삭제한다. 반환: 지운 배치 도형 수</summary>
        int DeleteMap(string strFactory, string strDeviceID, string strUpdateUser);

        /// <summary>
        /// 저장이 끝난(READY) Map 을 새 Device 이름으로 복사한다(2026-10-15). Map 헤더 / Layer / 배치 도형 / 저장 작업 건수 / Chain 전체를 복사하고
        /// Chain 은 새 Chain 번호를 받는다(Code / 이름 / 색은 그대로). E-Test / KLARF 데이터는 복사하지 않는다.
        /// 오류: 원본 없음 / 원본 READY 아님 / 새 이름이 원본과 같음 / 이미 있는 Device(대소문자 무시). 중간에 실패하면 새 Device 데이터를 지운다.
        /// 반환 1행: TARGET_DEVICE_ID / LAYER_COUNT / PLACED_COUNT / CHAIN_COUNT
        /// </summary>
        DataTable CopyMap(string strFactory, string strSourceDeviceID, string strTargetDeviceID, string strUser);

        #endregion

        #region [TQP_GDS_IMPORT_JOB]

        /// <summary>
        /// 저장을 시작한다. Device에 기존 Map이 있으면 오류다(먼저 DeleteMap).
        /// ImportInfo 순서: 0 Factory / 1 Device ID / 2 Top Structure / 3 GDS 파일 이름 / 4 사용 안 함(예전 GDS SHA-256 자리, 2026-10-08 컬럼 제거, 값이 와도 무시) /
        /// 5 User Unit / 6 Database Unit / 7 Create User / 8 사용 안 함(예전 원본 행 수 자리, 2026-10-12 제거, 값이 와도 무시) / 9 예상 PLACED 수 / 10 예상 좌표 수
        /// [11 저장 기준 FRONT / BACK, 2026-10-06 추가. 생략(11개)하면 FRONT. BACK = PLACED가 GDS 원점 기준 좌우 반전 좌표]
        /// 반환: SELECT_IMPORT_JOB 결과 1행(IMPORT_JOB_SEQ / FACTORY / DEVICE_ID / JOB_STATUS / 예상 / 수신 건수)
        /// </summary>
        DataTable BeginMapImport(string[] ImportInfo);

        /// <summary>저장 작업 1건(상태 / 예상 건수 / 수신 건수)을 조회한다. 없으면 0행이다.</summary>
        DataTable GetImportJob(string strImportJobSeq);

        /// <summary>진행 중인 저장 작업을 CANCELLED로 바꾼다. 저장된 일부 데이터는 DRAFT로 남고, 다음 저장 때 삭제 여부를 묻는다.</summary>
        void CancelImportJob(string strImportJobSeq, string strUpdateUser);

        /// <summary>
        /// 모든 배치를 보낸 뒤 저장을 확정한다. DB에서 PLACED 건수와 좌표 수 합계를 다시 세고,
        /// 종류별 배치 번호가 0부터 빠짐없이 있는지 확인한다. 예상 건수와 모두 같으면 작업 READY -> Map 헤더 READY로 바꾼다.
        /// 다르면 작업을 UPLOADING으로 두고(빠진 배치를 다시 보낼 수 있게) 결과만 반환한다.
        /// 반환 1행: RESULT(OK / MISMATCH) / JOB_STATUS / EXPECTED_PLACED / DB_PLACED /
        ///           EXPECTED_POINT / DB_POINT / MESSAGE
        /// </summary>
        DataTable CompleteMapImport(string strImportJobSeq, string strUpdateUser);

        #endregion

        #region [TQP_GDS_IMPORT_BATCH]

        /// <summary>이미 Commit된 배치 목록(STREAM_KIND / BATCH_NO / PAYLOAD_SHA256)을 조회한다. 재개 시 이 배치는 다시 보내지 않는다.</summary>
        DataTable GetImportBatch(string strImportJobSeq);

        #endregion

        #region [TQP_GDS_LAYER]

        /// <summary>
        /// Layer 배치 하나를 저장한다. aParas 열: 0 LAYER_ID / 1 DATA_TYPE / 2 COLOR_ARGB / 3 DISPLAY_NAME (Factory / Device는 서버가 작업에서 채운다)
        /// Layer 는 번호 + DataType(TEXT 는 TEXTTYPE) 단위다. 예전 3열(LAYER_ID / COLOR_ARGB / DISPLAY_NAME)은 DATA_TYPE 0 으로 저장한다 (2026-10-20).
        /// 같은 (Job, LAYER, 번호) 배치가 이미 저장되어 있으면 내용(서버 계산 해시)이 같을 때 0을 반환하고, 다르면 오류다.
        /// 반환: 이번 호출에서 저장 처리한 행 수
        /// </summary>
        int CreateLayerBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser);

        #endregion

        #region [TQP_GDS_PLACED_EL]

        /// <summary>
        /// 화면 배치 도형 배치 하나를 저장한다. aParas 열:
        /// 0 PLACED_ELEMENT_ID / 1 SOURCE_ELEMENT_ID / 2 LAYER_ID / 3 DATA_TYPE / 4 ELEMENT_TYPE(BOUNDARY / PATH / TEXT) / 5 CLOSED_FLAG(0/1) /
        /// 6 PATH_WIDTH(G17, PATH만) / 7 PATH_TYPE(PATH만) / 8 MIN_X / 9 MIN_Y / 10 MAX_X / 11 MAX_Y (G17) / 12 POINT_COUNT /
        /// 13 POINTS_FORMAT_VERSION / 14 POINTS(X/Y double little-endian의 16진수) / 15 TEXT_VALUE / 16 TEXT_TYPE / 17 TEXT_FONT /
        /// 18 TEXT_HORIZONTAL / 19 TEXT_VERTICAL (TEXT가 아니면 빈 값)
        /// 재전송 규칙과 반환은 CreateLayerBatch와 같다. LAYER가 먼저 저장되어 있어야 한다(DB FK).
        /// </summary>
        int CreatePlacedBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser);

        #endregion
    }
}
