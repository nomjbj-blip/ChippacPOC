-- 2026-10-12: E-Test 파일 중복 키에 공정(Test Type) 추가
-- 근거: 고객 메일(10-08 회의 답변) "U9 자재는 E/T 3회 진행: TSVET (1st) -> COWET (2nd) -> FT (3rd)"
--       결정: TSVET -> COWET -> FT 가 공정이므로 Wafer 와 공정을 조합해 키로 쓴다
-- 변경 전: UNIQUE (FACTORY, WAFER_ID)            -- 같은 Wafer 의 COWET 파일이 TSVET 파일을 지우고 재등록됨
-- 변경 후: UNIQUE (FACTORY, WAFER_ID, TEST_PROGRAM) -- TEST_PROGRAM = 파일명 5칸 Test Type = 공정
-- 설계: 문서/설계/04_ETest/2026-10-07_ETest_결과파일_Parsing_DB설계.md 14절
-- 적용: 2026-10-12 로컬 docker oracle19c DMSMGR (적용 전 새 키 중복 0). 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE DROP CONSTRAINT UQ_GDS_SRC_FILE_WAFER DROP INDEX;
ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE ADD CONSTRAINT UQ_GDS_SRC_FILE_WAFER_TEST UNIQUE (FACTORY, WAFER_ID, TEST_PROGRAM);

COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.TEST_PROGRAM IS '파일명 5칸 Test Type = 공정 (TSVET / COWET / FT). 중복 키 FACTORY + WAFER_ID + TEST_PROGRAM';

EXIT
