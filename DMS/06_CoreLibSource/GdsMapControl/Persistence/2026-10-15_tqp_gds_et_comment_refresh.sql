-- 2026-10-15: E-Test 테이블 주석을 현재 동작 기준으로 다시 적는다 (구조 / 데이터 변경 없음)
-- 1) TQP_GDS_ET_ITEM: 2026-10-14 Para Setup 재정의(등록 Parameter 마스터) 전 기준 주석 갱신
--    - ITEM_NAME / PARA_NAME / CHAIN_NAME / CHAIN_NAME2 = 헤더 원문이 아니라 변환 이름, COND_VALUE = 대문자, TEST_PROGRAM = 필수
--    - CHAIN_TYPE / FORCE_LEG / FORCE_LEG2 = 용어 "단자" -> "Chain" (2026-10-14 화면 용어 변경)
--    설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md 4.2절
-- 2) TQP_GDS_SRC_FILE: PARSE_RULE_VER 3(2026-10-14 업로드 재설계, 등록 Parameter 만 저장) 반영
--    - PARSE_RULE_VER 에 3 추가, FAB_NAME / VIEW_SIDE 는 3 부터 저장하지 않음(NULL)
-- 3) TQP_GDS_SRC_FILE YMS 컬럼 6개: 로컬 DB 에 문자 깨짐(U+FFFD)으로 저장된 주석을 2026-10-14_tqp_gds_et_yms.sql 원문으로 다시 넣는다
-- 실행: 한글 주석이 깨지지 않도록 클라이언트 문자셋을 UTF-8 로 지정한다.
--   docker cp <이 파일> oracle19c-DMS:/tmp/comment_refresh.sql
--   docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba @/tmp/comment_refresh.sql'
-- 고객 DB 에 E-Test DDL 을 적용할 때도 이 파일을 마지막에 실행한다(주석만 바뀜).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

-- 1) TQP_GDS_ET_ITEM
COMMENT ON TABLE DMSMGR.TQP_GDS_ET_ITEM IS 'E-Test 등록 Parameter 마스터 (Factory + Device + 공정 단위). 측정값(TQP_GDS_ET_VALUE) / Spec(TQP_GDS_ET_LIMIT) 이 참조';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.ITEM_NAME IS 'Parameter 저장 이름. 헤더 원문을 변환한 이름 (예: RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH, 50자 이하). Spec / YMS 컬럼 이름과 같은 값';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.PARA_NAME IS 'Para 이름. 헤더 = 앞 부분을 변환한 이름 (예: CONNECTIVITY_IFORCE / LEAKAGE_PARALLEL)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.COND_VALUE IS '헤더 = 뒤 조건 값, 대문자 (예: 1.0MA). 없으면 NULL';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.CHAIN_NAME IS 'Chain 1 이름. 헤더 @ 뒤를 변환한 이름, Force leg 포함 (예: PIC_SERP1_M4_FH)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.CHAIN_NAME2 IS 'Chain 2 이름 (Leakage_Serial 등 A:B 형식의 두 번째 Chain, 변환 이름). 없으면 NULL';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.TEST_PROGRAM IS '공정 (Test Type: TSVET / COWET / FT), 대문자. Parameter 등록 때 지정';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.CHAIN_TYPE IS 'Chain 1 의 Chain type (TSVDC / C4DC / PIC_SERP ...)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.FORCE_LEG IS 'Chain 1 끝 Force leg (FH / FL)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.FORCE_LEG2 IS 'Chain 2 끝 Force leg (FH / FL)';

-- 2) TQP_GDS_SRC_FILE: 파일명 해석 / 저장 규칙
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.PARSE_RULE_VER IS '파일명 해석 / 저장 규칙: 1 = 뒤에서부터 분해 (2026-10-07) / 2 = 8칸 규칙 + $Fab$ (2026-10-12) / 3 = 등록 Parameter 만 저장 (2026-10-14, 현재 업로드)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.FAB_NAME IS '파일명 끝 $...$ 꼬리표 (예: Silterra). PARSE_RULE_VER 2 까지만 저장, 3 부터 NULL';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.VIEW_SIDE IS '좌표 기준 면. PARSE_RULE_VER 2 는 BACK, 3 부터 저장하지 않음 (NULL)';

-- 3) TQP_GDS_SRC_FILE: YMS 반영 상태 (2026-10-14_tqp_gds_et_yms.sql 원문)
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_STATUS IS 'YMS 반영 상태: NULL 반영 안 함 / DONE / WAIT(Map 없음, 좌표 다시 계산 때 반영) / FAIL';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_PROGRAM IS 'YMS Program = Device_공정 (예: OMM_TSVET), TD 테이블 TD_<Program>';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_LOT_SEQ IS '반영한 TQP_LOT.LOT_SEQ';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_WAFER_SEQ IS '반영한 TQP_WAFER.WAFER_SEQ (다시 반영 / 파일 삭제 때 이 Wafer 를 먼저 지운다). Lot / Wafer 를 만든 직후 기록';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_TIME IS 'YMS 반영 시각';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.YMS_MESSAGE IS 'YMS 반영 실패 사유 (단계: 메시지) 또는 보류 안내';

-- 확인: 깨진 문자(U+FFFD, UTF-8 EF BF BD)가 남은 주석 수 = 0 이어야 한다
SELECT COUNT(*) AS BROKEN_COMMENTS
  FROM DBA_COL_COMMENTS
 WHERE OWNER = 'DMSMGR' AND TABLE_NAME LIKE 'TQP_GDS%'
   AND INSTR(RAWTOHEX(UTL_RAW.CAST_TO_RAW(COMMENTS)), 'EFBFBD') > 0;

EXIT
