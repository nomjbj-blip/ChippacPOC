-- 2026-10-14: E-Test 로컬 업로드 데이터 정리 (Upload 재설계 S5)
-- 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9.3절 / 10.1절
--
-- 로컬 DB 전용이다. 고객 DB 에는 실행하지 않는다(E-Test 업로드 데이터가 없다).
-- 지움: TQP_GDS_ET_VALUE / TQP_GDS_ET_DIE / TQP_GDS_SRC_FILE 전부(예전 방식으로 헤더 전체를 저장한 파일)
--       + TQP_GDS_ET_ITEM 의 TEST_PROGRAM 이 없는 행(예전 업로드가 만든 측정 항목)
-- 남김: 등록 Parameter(TEST_PROGRAM 있음) / Spec(TQP_GDS_ET_LIMIT) / 대표 Chain / GDS 연결
-- 다음: 2026-10-14_tqp_gds_et_upload_registered.sql 실행 -> 새 업로드 화면으로 샘플을 다시 올린다.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON SERVEROUTPUT ON

DECLARE
    n_file  NUMBER;
    n_die   NUMBER;
    n_value NUMBER;
    n_item  NUMBER;
    n_para  NUMBER;
BEGIN
    SELECT COUNT(*) INTO n_file FROM DMSMGR.TQP_GDS_SRC_FILE;
    SELECT COUNT(*) INTO n_die FROM DMSMGR.TQP_GDS_ET_DIE;
    SELECT COUNT(*) INTO n_value FROM DMSMGR.TQP_GDS_ET_VALUE;
    SELECT COUNT(*) INTO n_item FROM DMSMGR.TQP_GDS_ET_ITEM WHERE TEST_PROGRAM IS NULL;
    SELECT COUNT(*) INTO n_para FROM DMSMGR.TQP_GDS_ET_ITEM WHERE TEST_PROGRAM IS NOT NULL;
    DBMS_OUTPUT.PUT_LINE('지울 것: 파일 ' || n_file || ' / Die ' || n_die || ' / 측정값 ' || n_value || ' / 예전 측정 항목 ' || n_item);
    DBMS_OUTPUT.PUT_LINE('남길 것: 등록 Parameter ' || n_para);
END;
/

DELETE FROM DMSMGR.TQP_GDS_ET_VALUE;
DELETE FROM DMSMGR.TQP_GDS_ET_DIE;
DELETE FROM DMSMGR.TQP_GDS_SRC_FILE;
DELETE FROM DMSMGR.TQP_GDS_ET_ITEM WHERE TEST_PROGRAM IS NULL;
COMMIT;
