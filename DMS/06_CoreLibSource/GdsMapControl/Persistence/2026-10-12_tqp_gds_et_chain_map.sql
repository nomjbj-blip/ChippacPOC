-- 2026-10-12: E-Test Chain -> 대표 Chain(GDS Chain) 묶음
-- ** 대체됨 (같은 날, 대표 Chain 대신 묶음 번호로 변경) -> 2026-10-12_tqp_gds_et_chain_group.sql 이 이 테이블을 지우고 TQP_GDS_ET_CHAIN_GROUP 을 만든다. 이 파일은 다시 실행하지 않는다. **
-- 요청 (2026-10-12): E-Test 결과에서 Chain 정보를 뽑을 수 있다. GDS 에 Setup 되는 Chain 은 Input 값에 해당하는 Chain 정보를 대표로 둔다.
--   따라서 E-Test Spec 설정 시 2개의 Chain 을 대표 Chain 으로 묶는 기능이 필요하다.
-- 확인한 데이터: E-Test Chain 은 양 끝 Pad 이름으로 나온다(TSVDC_ASIC1_P_FH / TSVDC_ASIC1_P_FL, 샘플 1,306개 = _FH 879 + _FL 427).
--   GDS Chain 이름은 Input Label 기준이다(TQP_GDS_CHAIN_VER.CHAIN_NAME 예: TSVDC_TOP1_P, TSVDC_L_N).
--   -> E-Test Chain 이름 1행 = 대표 Chain 1개. 같은 대표 Chain 을 가진 행끼리 "묶음"이다(보통 _FH + _FL 2개).
-- 키는 E-Test Device(파일명 Device) 기준이다. GDS Device(U9)와 E-Test Device(OMM) 연결은 보류라 대표 Chain 은 이름(문자열)으로만 둔다(FK 없음).
-- 공정(TSVET / COWET / FT)과 관계없이 Device 단위다(물리 Chain 은 공정이 달라도 같다).
-- 설계: 문서/2026-10-07_ETest_Limit_설정_설계.md 13절
-- 적용: 2026-10-12 로컬 docker oracle19c DMSMGR. 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

CREATE TABLE DMSMGR.TQP_GDS_ET_CHAIN_MAP
   (    FACTORY          VARCHAR2(30 CHAR) NOT NULL,
        DEVICE_ID        VARCHAR2(50 CHAR) NOT NULL,
        CHAIN_NAME       VARCHAR2(64 CHAR) NOT NULL,
        REP_CHAIN_NAME   VARCHAR2(200 CHAR) NOT NULL,
        REMARK           VARCHAR2(200 CHAR),
        CREATE_USER      VARCHAR2(30),
        CREATE_TIME      DATE DEFAULT SYSDATE NOT NULL,
        UPDATE_USER      VARCHAR2(30),
        UPDATE_TIME      DATE,
        CONSTRAINT CK_GDS_ET_CHAIN_MAP_UPPER CHECK (CHAIN_NAME = UPPER(CHAIN_NAME)),
        CONSTRAINT PK_GDS_ET_CHAIN_MAP PRIMARY KEY (FACTORY, DEVICE_ID, CHAIN_NAME)
   );

CREATE INDEX DMSMGR.IX_GDS_ET_CHAIN_MAP_REP ON DMSMGR.TQP_GDS_ET_CHAIN_MAP (FACTORY, DEVICE_ID, REP_CHAIN_NAME);

COMMENT ON TABLE DMSMGR.TQP_GDS_ET_CHAIN_MAP IS 'E-Test Chain -> 대표 Chain(GDS Chain 이름). 같은 대표 Chain 끼리 묶음 (보통 _FH + _FL)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_CHAIN_MAP.CHAIN_NAME IS 'E-Test Chain 이름 (헤더 @ 뒤 / : 뒤), 대문자. TQP_GDS_ET_ITEM.CHAIN_NAME / CHAIN_NAME2 와 UPPER 로 비교';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_CHAIN_MAP.REP_CHAIN_NAME IS '대표 Chain = GDS Chain 이름(Input Label 기준, 예: TSVDC_TOP1_P). 문자열 연결(FK 없음)';

EXIT
