-- 2026-10-08: TQP_GDS_MAP.GDS_SHA256 컬럼 제거
-- 결정 "1"(컬럼까지 완전히 제거).(2026-10-06)에서 화면 계산을 빼고 NULL 허용으로 둔 컬럼의 후속 정리다.
-- 이 값은 재등록 확인창의 "같은 파일 / 다른 파일" 표시에만 쓰였고, 2026-10-06 이후 저장분은 모두 NULL이다.
-- 함께 바꾼 것: 08_DBQuery/ORACLE/TQP_GDS_MAP.xml(SELECT_MAP_INFO / CREATE_MAP), BSL GdsMapImport.BeginMapImport(ImportInfo[4] 무시).
-- 순서 주의: XML / BSL을 먼저 반영한 뒤 실행한다. 예전 BSL은 CREATE_MAP에 이 컬럼 값을 넘기므로 이 DDL 뒤에는 저장이 실패한다.
-- 적용: 2026-10-08 로컬 docker oracle19c DMSMGR. 고객 DB 적용 여부는 별도 확인. 기존 행의 SHA 값은 사라진다(복구 불가).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQP_GDS_MAP DROP COLUMN GDS_SHA256;

-- 확인: 0이면 제거 완료
SELECT COUNT(*) AS SHA_COLUMN_COUNT FROM ALL_TAB_COLUMNS
 WHERE OWNER = 'DMSMGR' AND TABLE_NAME = 'TQP_GDS_MAP' AND COLUMN_NAME = 'GDS_SHA256';

EXIT
