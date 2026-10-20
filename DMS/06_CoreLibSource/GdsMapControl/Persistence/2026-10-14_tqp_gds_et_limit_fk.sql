-- 2026-10-14: E-Test Para Setup 2단계 일부 (Limit 화면 정리)
-- 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md 4.5절 / 4.6절 / 4.7절
--
-- 내용
--  1) TQP_GDS_ET_LIMIT -> TQP_GDS_ET_ITEM(등록 Parameter) FK, ON DELETE CASCADE. Spec 은 등록 Parameter 에만 둘 수 있다.
--     키 (FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME) = UQ_GDS_ET_ITEM_PARA. Spec ITEM_NAME = 등록 Parameter 저장 이름.
--  2) TQP_GDS_ET_CHAIN_GROUP / ET_CHAIN_GROUP_SEQ 삭제. 대표 Chain(TQP_GDS_ET_REP_CHAIN)이 묶음 역할을 한다.
--     (쿼리 XML / DSL / 서비스 / Limit 화면 [Chain 묶기] [Setup Chain 으로 찾기] 탭도 같이 지웠다)
--  0) 먼저 확인: 등록 Parameter 가 없는 Spec 행(예전 키 = 헤더 원문 등) 또는 Chain 묶음 행이 있으면 아무것도 바꾸지 않고 멈춘다.
--     있으면 Para 설정에서 Parameter 를 등록하고 Spec 을 다시 저장하거나(Spec 표 붙여넣기는 자동 등록), 묶음을 대표 Chain 으로 옮긴 뒤 실행한다.
--     로컬은 Spec 0행 / 묶음 0행이었다.
--
-- 남은 2단계: 로컬 E-Test 데이터 정리 -> TQP_GDS_ET_ITEM.TEST_PROGRAM NOT NULL 은 Upload 재설계 구현 때 한다.
-- 이 파일을 적용하면 이전 버전 Limit 화면(Chain 묶기 탭 / 헤더 원문 키로 Spec 저장)은 동작하지 않는다. 서버 / 클라이언트를 같이 배포한다.
-- 테이블은 PURGE 없이 지운다(로컬 recycle bin 에 남음).
-- 적용: 로컬 docker oracle19c-DMS DMSMGR. 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON SERVEROUTPUT ON

-- 0) 사전 확인
DECLARE
    n_orphan NUMBER;
    n_group  NUMBER;
BEGIN
    SELECT COUNT(*) INTO n_orphan
      FROM DMSMGR.TQP_GDS_ET_LIMIT L
     WHERE NOT EXISTS (SELECT 1 FROM DMSMGR.TQP_GDS_ET_ITEM I
                        WHERE I.FACTORY = L.FACTORY AND I.DEVICE_ID = L.DEVICE_ID
                          AND I.TEST_PROGRAM = L.TEST_PROGRAM AND I.ITEM_NAME = L.ITEM_NAME);
    SELECT COUNT(*) INTO n_group FROM DMSMGR.TQP_GDS_ET_CHAIN_GROUP;
    DBMS_OUTPUT.PUT_LINE('등록 Parameter 없는 Spec ' || n_orphan || '행 / Chain 묶음 ' || n_group || '행');
    IF n_orphan > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, '등록 Parameter 가 없는 Spec 이 ' || n_orphan || '행 있습니다. Para 설정에서 등록하고 Spec 을 다시 저장한 뒤 실행하세요.');
    END IF;
    IF n_group > 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chain 묶음이 ' || n_group || '행 있습니다. 대표 Chain 으로 옮긴 뒤 실행하세요.');
    END IF;
END;
/

-- 1) Spec -> 등록 Parameter FK
ALTER TABLE DMSMGR.TQP_GDS_ET_LIMIT ADD CONSTRAINT FK_GDS_ET_LIMIT_PARA
    FOREIGN KEY (FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME)
    REFERENCES DMSMGR.TQP_GDS_ET_ITEM (FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME) ON DELETE CASCADE;

COMMENT ON TABLE DMSMGR.TQP_GDS_ET_LIMIT IS 'E-Test Spec. Device + 공정 + 등록 Parameter(TQP_GDS_ET_ITEM) 단위, FK_GDS_ET_LIMIT_PARA';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_LIMIT.ITEM_NAME IS '등록 Parameter 저장 이름 (TQP_GDS_ET_ITEM.ITEM_NAME)';

-- 2) Chain 묶음 삭제
DROP TABLE DMSMGR.TQP_GDS_ET_CHAIN_GROUP;
DROP SEQUENCE DMSMGR.ET_CHAIN_GROUP_SEQ;
