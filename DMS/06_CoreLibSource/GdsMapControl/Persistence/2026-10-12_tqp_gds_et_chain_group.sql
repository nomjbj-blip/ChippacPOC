-- 2026-10-12: E-Test Chain 묶음 (대표 Chain 방식을 대체)
-- 기준: 대표 Chain 이름 없이 묶기만 하고, Setup Chain 으로 찾으면 묶인 Chain 의 Limit 등 정보를 모두 찾는다.
--   -> 대표 이름 없이 Chain 여러 개(보통 _FH + _FL 2개)를 같은 GROUP_SEQ 로 묶는다.
--   -> GDS Setup Chain 이름으로 찾기: E-Test Chain 이름이 같으면(EXACT) 그 Chain, 없으면 이름_FH / 이름_FL (SUFFIX),
--      찾은 Chain 이 묶여 있으면 같은 묶음의 Chain 전부 -> 그 Chain 들의 Spec(Limit) / 측정 항목 / 값 통계.
-- 같은 날 만든 TQP_GDS_ET_CHAIN_MAP(대표 Chain, 2026-10-12_tqp_gds_et_chain_map.sql)은 배포 전이고 0행이라 지운다.
-- Device 단위(E-Test 파일명 Device), 공정 무관. Chain 1개는 묶음 1개에만 들어간다(PK).
-- 설계: 문서/2026-10-07_ETest_Limit_설정_설계.md 15절
-- 적용: 2026-10-12 로컬 docker oracle19c DMSMGR. 고객 DB 적용 여부는 별도 확인 (고객 DB 에 CHAIN_MAP 이 없으면 1단계는 건너뛴다).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

-- 1) 대표 Chain 테이블 제거 (없으면 넘어감)
BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE DMSMGR.TQP_GDS_ET_CHAIN_MAP PURGE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN RAISE; END IF;
END;
/

-- 2) 묶음 번호
CREATE SEQUENCE DMSMGR.ET_CHAIN_GROUP_SEQ START WITH 1 INCREMENT BY 1 NOCACHE;

-- 3) 묶음
CREATE TABLE DMSMGR.TQP_GDS_ET_CHAIN_GROUP
   (    FACTORY          VARCHAR2(30 CHAR) NOT NULL,
        DEVICE_ID        VARCHAR2(50 CHAR) NOT NULL,
        CHAIN_NAME       VARCHAR2(64 CHAR) NOT NULL,
        GROUP_SEQ        NUMBER(12) NOT NULL,
        REMARK           VARCHAR2(200 CHAR),
        CREATE_USER      VARCHAR2(30),
        CREATE_TIME      DATE DEFAULT SYSDATE NOT NULL,
        UPDATE_USER      VARCHAR2(30),
        UPDATE_TIME      DATE,
        CONSTRAINT CK_GDS_ET_CHAIN_GROUP_UPPER CHECK (CHAIN_NAME = UPPER(CHAIN_NAME)),
        CONSTRAINT PK_GDS_ET_CHAIN_GROUP PRIMARY KEY (FACTORY, DEVICE_ID, CHAIN_NAME)
   );

CREATE INDEX DMSMGR.IX_GDS_ET_CHAIN_GROUP_SEQ ON DMSMGR.TQP_GDS_ET_CHAIN_GROUP (FACTORY, DEVICE_ID, GROUP_SEQ);

COMMENT ON TABLE DMSMGR.TQP_GDS_ET_CHAIN_GROUP IS 'E-Test Chain 묶음. 같은 GROUP_SEQ 끼리 한 묶음(보통 _FH + _FL). Setup Chain 으로 찾을 때 묶음 전체의 Spec 을 함께 찾는다';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_CHAIN_GROUP.CHAIN_NAME IS 'E-Test Chain 이름 (헤더 @ 뒤 / : 뒤), 대문자. TQP_GDS_ET_ITEM.CHAIN_NAME / CHAIN_NAME2 와 UPPER 로 비교';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_CHAIN_GROUP.GROUP_SEQ IS '묶음 번호 (ET_CHAIN_GROUP_SEQ). 묶음마다 2개 이상';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_CHAIN_GROUP.REMARK IS '묶은 방법 (수동 묶음 / FH-FL 자동 묶음)';

EXIT
