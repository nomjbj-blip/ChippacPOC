-- 2026-10-07: E-Test 파일 중복 기준 변경 (요청)
-- "디바이스 기준으로 중복 저장 가능. 결과 data 는 wafer 별 / Lot 별로 생성되므로 디바이스는 중복 허용, 중복 방지는 wafer id 기준으로 우선 정의.
--  wafer 의 slot No 가 없으므로 사용자가 입력할 수 있도록 한다."
-- 변경 전: UNIQUE (FACTORY, DEVICE_ID, LOT_ID, SLOT_NO, TEST_PROGRAM, TEST_TIME)
-- 변경 후: UNIQUE (FACTORY, WAFER_ID)  -- WAFER_ID 는 LOT_ID + '-' + 2자리 SLOT_NO 가상 컬럼, SLOT_NO 는 업로드 화면에서 사용자가 입력
-- 같은 Wafer 를 다시 올리면 (시험 일시 / Device 가 달라도) 화면이 묻고 기존 파일 삭제 후 재등록한다.
-- 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 12절
-- 적용: 2026-10-07 로컬 docker oracle19c DMSMGR (적용 전 WAFER_ID 중복 0 확인). 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE DROP CONSTRAINT UQ_GDS_SRC_FILE_TEST DROP INDEX;
ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE ADD CONSTRAINT UQ_GDS_SRC_FILE_WAFER UNIQUE (FACTORY, WAFER_ID);

EXIT
