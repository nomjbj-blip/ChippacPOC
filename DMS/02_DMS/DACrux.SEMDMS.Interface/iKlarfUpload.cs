using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// 엔지니어가 화면에서 올린 KLARF 파일(+ Defect 이미지)을 기존 DMS 자동 적재(Inspection.DataService)와 같은
    /// TQD_* 테이블 / 같은 값 규칙으로 저장하는 원격 호출 계약이다. 저장된 데이터는 기존 Defect 조회 화면에서 그대로 조회된다.
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-07_KLARF_Upload_DB적재_설계.md (F08)
    /// 호출 순서: BeginUpload(KLARF 받기 + 서버 파싱 미리보기) -> AddImage x N(이미지 파일마다 1회) -> CommitUpload(저장)
    ///            중간에 그만두면 CancelUpload. 서버 작업 폴더는 Commit / Cancel 때 지우고, 남은 폴더는 하루 뒤 BeginUpload 가 정리한다.
    /// 기존 DataService 와 같이 문장마다 자동 Commit 이다. Wafer 저장 중 오류가 나면 이번에 새로 만든 STEP 의 행을 지우고 오류를 돌려준다.
    /// </summary>
    public interface iKlarfUpload
    {
        #region [KLARF 업로드]

        /// <summary>
        /// KLARF 파일을 서버 작업 폴더에 받고 파싱해 저장될 값을 미리 돌려준다. DB 는 조회만 한다.
        /// UploadInfo 순서: 0 Factory / 1 User / 2 원본 파일명 / 3 Product(빈 값 = KLARF DeviceID) / 4 Step ID(빈 값 = KLARF StepID) /
        ///                  5 검사 설비(빈 값 = KLARF InspectionStationID 의 설비) / 6 예약(빈 값, 쓰지 않음)
        ///                  Defect 분류는 KLARF 표준대로 CLASSNUMBER / ROUGHBIN / FINEBIN 을 파일 값 그대로 저장한다 (2026-10-15).
        /// 반환: Wafer 마다 1행. 열은 KlarfUpload BSL 의 결과 표와 같다(UPLOAD_KEY / SLOT_ID / RAW_WAFER_ID / WAFER_ID / LOT_ID / PRODUCT /
        ///       STEP_ID / SETUP_ID / RESULTTIMESTAMP / EQUIP / DEFECTS / IMAGE_FILES / EXIST_STEP_SEQ / RESULT / MESSAGE ...)
        /// </summary>
        DataTable BeginUpload(string[] UploadInfo, byte[] KlarfFile);

        /// <summary>Defect 이미지 파일 1개를 작업 폴더에 받는다. 파일 이름은 KLARF 의 TiffFileName 과 같아야 Defect 에 연결된다.</summary>
        void AddImage(string strUploadKey, string strFileName, byte[] ImageFile);

        /// <summary>
        /// 작업 폴더의 KLARF 를 다시 파싱해 Wafer 마다 TQD_SETUP ~ TQD_INSP_SUM 에 저장한다.
        /// Options 순서: 0 같은 Wafer / Step / 검사시각이 이미 있으면 삭제 후 재등록(Y) / 건너뜀(N)
        /// 반환: Wafer 마다 1행 (BeginUpload 와 같은 열 + STEP_SEQ / WAFER_SEQ / SETUP_SEQ / SAVED_IMAGES / ELAPSED_MS, RESULT = SAVED / SKIPPED / ERROR)
        /// </summary>
        DataTable CommitUpload(string strUploadKey, string[] Options);

        /// <summary>저장하지 않고 작업 폴더를 지운다.</summary>
        void CancelUpload(string strUploadKey);

        /// <summary>
        /// 업로드로 만든 STEP 1개의 데이터(TQD_STEP / DEFECT / IMAGES / INSP_INFO / INSP_SUM)를 지운다.
        /// 자동 적재 데이터는 지우지 않는다(STEP COMMENT 가 'UPLOAD;' 로 시작하는 행만). 반환: 지운 STEP 수(0 또는 1)
        /// </summary>
        int DeleteUploadedStep(string strStepSeq, string strWaferSeq, string strUser);

        #endregion
    }
}
