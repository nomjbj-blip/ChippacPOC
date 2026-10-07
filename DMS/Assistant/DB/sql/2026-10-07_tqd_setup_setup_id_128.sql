-- 2026-10-07 Claude CLAUDE-031 (F08 KLARF 업로드): TQD_SETUP.SETUP_ID 40 -> 128 BYTE 확대
-- 사용자 결정 D2 "컬럼 128자로 확대": 고객 KLARF SetupID 가 46자 (ROOT-CAI-OMM_Hybrid-OMM_Hybrid_C4Bump_Silterra)
--   기존 VARCHAR2(40) 이면 ORA-12899 로 TQD_SETUP 저장(Step 1)이 실패한다.
-- 영향: 확대만 한다. 기존 값 / PK(SETUP_SEQ) / 인덱스 IDX_TQD_SETUP_01(SETUP_ID, STEP_ID, SETUP_TIME) 그대로 사용.
--   의존 객체(뷰 / 프로시저) 없음, 같은 이름 컬럼은 백업 테이블 TQD_SETUP_BAK 뿐 (2026-10-07 로컬 조회).
-- 주의: 기존 쿼리 XML 일부(CREATE_SETUP_INFO / SELECT_ORIGIN_INDEX)는 :setup_id 를 size="40" 으로 바인딩한다.
--   컬럼만 넓혀서는 40자 초과 값이 그대로 들어간다는 보장이 없으므로, 업로드 BSL 은 size 128 로 바인딩한 쿼리를 추가해 쓴다.
-- 적용: 2026-10-07 로컬 docker oracle19c DMSMGR (사용자 승인 "진행"). 고객 DB 에도 같은 DDL 필요.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQD_SETUP MODIFY (SETUP_ID VARCHAR2(128 BYTE));

SELECT DATA_LENGTH, NULLABLE FROM DBA_TAB_COLUMNS
 WHERE OWNER = 'DMSMGR' AND TABLE_NAME = 'TQD_SETUP' AND COLUMN_NAME = 'SETUP_ID';

EXIT
