-- 2026-10-14: E-Test Para 설정 화면 재구성 (Parameter 별 GDS Chain 이름 + 대표 Chain 삭제)
-- 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md 4.1절
--
-- 내용
--  1) TQP_GDS_ET_ITEM.GDS_CHAIN_NAME 추가: Chain 1(첫 번째 Chain) 의 GDS Setup Chain 이름. 사용자가 Para 설정 [등록 Parameter] 탭에서 직접 입력한다.
--     GDS Chain 목록에 없는 이름도 저장한다(화면이 주황색으로 알림). 길이는 TQP_GDS_CHAIN_VER.CHAIN_NAME 과 같다.
--  2) 대표 Chain 데이터가 남아 있으면 옮긴다: Chain 1 이 든 대표 Chain 의 연결 GDS Chain 이름(지금 버전 이름 우선) -> GDS_CHAIN_NAME.
--     대표 Chain 은 GDS 대표 Device 단위라 E-Test Device 와 같은 이름 또는 Sub Device 연결로 찾는다. 로컬은 0행이었다.
--  3) TQP_GDS_ET_CHAIN_LINK / TQP_GDS_ET_REP_CHAIN 삭제 (쿼리 XML / DSL / 서비스 / Para 설정 [대표 Chain] 탭도 같이 지운다).
--
-- 이 파일을 적용하면 이전 버전 Para 설정 화면([대표 Chain] 탭)과 E-Test Limit 설정 화면은 동작하지 않는다. 서버 / 클라이언트를 같이 배포한다.
-- 테이블은 PURGE 없이 지운다(로컬 recycle bin 에 남음).
-- 적용: 로컬 docker oracle19c-DMS DMSMGR. 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON SERVEROUTPUT ON

-- 0) 사전 확인 (행 수만 출력)
DECLARE
    n_rep  NUMBER;
    n_link NUMBER;
BEGIN
    SELECT COUNT(*) INTO n_rep FROM DMSMGR.TQP_GDS_ET_REP_CHAIN;
    SELECT COUNT(*) INTO n_link FROM DMSMGR.TQP_GDS_ET_CHAIN_LINK;
    DBMS_OUTPUT.PUT_LINE('대표 Chain ' || n_rep || '행 / GDS 연결 ' || n_link || '행');
END;
/

-- 1) Parameter 별 GDS Chain 이름
ALTER TABLE DMSMGR.TQP_GDS_ET_ITEM ADD (GDS_CHAIN_NAME VARCHAR2(200 CHAR));
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_ITEM.GDS_CHAIN_NAME IS 'Chain 1 의 GDS Setup Chain 이름 (사용자 입력, GDS 대표 Device 의 TQP_GDS_CHAIN_VER.CHAIN_NAME 과 대소문자 무시 비교)';

-- 2) 대표 Chain -> GDS_CHAIN_NAME 옮기기
UPDATE DMSMGR.TQP_GDS_ET_ITEM I
   SET I.GDS_CHAIN_NAME = (
       SELECT MAX(NVL(V.CHAIN_NAME, L.GDS_CHAIN_NAME))
         FROM DMSMGR.TQP_GDS_ET_REP_CHAIN R
         JOIN DMSMGR.TQP_GDS_ET_CHAIN_LINK L
           ON L.FACTORY = R.FACTORY AND L.DEVICE_ID = R.DEVICE_ID AND L.REP_CHAIN_NAME = R.REP_CHAIN_NAME
         LEFT JOIN DMSMGR.TQP_GDS_CHAIN_VER V
           ON V.FACTORY = L.FACTORY AND V.DEVICE_ID = L.DEVICE_ID AND V.CHAIN_SEQ = L.CHAIN_SEQ AND V.IS_CURRENT = 1
        WHERE R.FACTORY = I.FACTORY
          AND R.CHAIN_NAME = I.CHAIN_NAME
          AND (UPPER(R.DEVICE_ID) = UPPER(I.DEVICE_ID)
               OR EXISTS (SELECT 1 FROM DMSMGR.TQP_GDS_MAP_SUB_DEVICE S
                           WHERE S.FACTORY = I.FACTORY AND S.DEVICE_ID = R.DEVICE_ID AND UPPER(S.SUB_DEVICE_ID) = UPPER(I.DEVICE_ID))))
 WHERE I.TEST_PROGRAM IS NOT NULL
   AND I.GDS_CHAIN_NAME IS NULL
   AND EXISTS (SELECT 1 FROM DMSMGR.TQP_GDS_ET_REP_CHAIN R2 WHERE R2.FACTORY = I.FACTORY AND R2.CHAIN_NAME = I.CHAIN_NAME);
COMMIT;

-- 3) 대표 Chain / 연결 삭제
DROP TABLE DMSMGR.TQP_GDS_ET_CHAIN_LINK;
DROP TABLE DMSMGR.TQP_GDS_ET_REP_CHAIN;

-- 확인
SELECT COUNT(*) AS PARA_COUNT, COUNT(GDS_CHAIN_NAME) AS GDS_CHAIN_COUNT FROM DMSMGR.TQP_GDS_ET_ITEM WHERE TEST_PROGRAM IS NOT NULL;
SELECT COUNT(*) AS LEFT_TABLES FROM ALL_TABLES WHERE OWNER = 'DMSMGR' AND TABLE_NAME IN ('TQP_GDS_ET_REP_CHAIN', 'TQP_GDS_ET_CHAIN_LINK');
