-- 2026-10-12: GDS 대표 Device / Sub Device 테이블 TQP_GDS_MAP_SUB_DEVICE
-- 요청: "GDS View 화면에서 대표 디바이스를 U9으로 하고 사용가능한 Sub 디바이스를 U9, OMM, OMM Hybrid 등등 메뉴얼로 Setup.
--              다른 디바이스 조회 및 연동하는 부분을 Sub Device 로 연동"
-- 대표 Device = TQP_GDS_MAP.DEVICE_ID (Map 1개). Sub Device = 대표 Map을 같이 쓰는 다른 데이터의 Device 이름(KLARF Product / E-Test 파일명 등).
-- 대표 자신은 저장하지 않는다(언제나 자기 Sub). Sub 1개는 대표 1개에만 속한다. 대소문자만 다른 중복은 UPPER 고유 색인으로 막는다.
-- FK 없음: Map 은 삭제 후 재등록하므로(리비전 없음) CASCADE 면 재등록 때 Sub 설정이 사라진다. 찾을 때 READY Map 과 조인한다.
-- 설계: 문서/설계/02_GDS/2026-10-12_GDS_대표Device_SubDevice_설계.md 3절
-- 적용: 2026-10-12 로컬 docker oracle19c DMSMGR. 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

CREATE TABLE DMSMGR.TQP_GDS_MAP_SUB_DEVICE
   (    FACTORY          VARCHAR2(30 CHAR) NOT NULL,
        SUB_DEVICE_ID    VARCHAR2(50 CHAR) NOT NULL,
        DEVICE_ID        VARCHAR2(50 CHAR) NOT NULL,
        REMARK           VARCHAR2(200 CHAR),
        CREATE_USER      VARCHAR2(30),
        CREATE_TIME      DATE DEFAULT SYSDATE NOT NULL,
        CONSTRAINT CK_GDS_MAP_SUB_DEVICE_SELF CHECK (UPPER(SUB_DEVICE_ID) <> UPPER(DEVICE_ID)),
        CONSTRAINT PK_GDS_MAP_SUB_DEVICE PRIMARY KEY (FACTORY, SUB_DEVICE_ID)
   );

CREATE UNIQUE INDEX DMSMGR.UX_GDS_MAP_SUB_DEVICE_UPPER ON DMSMGR.TQP_GDS_MAP_SUB_DEVICE (FACTORY, UPPER(SUB_DEVICE_ID));
CREATE INDEX DMSMGR.IX_GDS_MAP_SUB_DEVICE_MAP ON DMSMGR.TQP_GDS_MAP_SUB_DEVICE (FACTORY, DEVICE_ID);

COMMENT ON TABLE DMSMGR.TQP_GDS_MAP_SUB_DEVICE IS 'GDS 대표 Device(Map) 를 같이 쓰는 Sub Device 이름 (KLARF Product / E-Test Device 등)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_MAP_SUB_DEVICE.SUB_DEVICE_ID IS 'Sub Device 이름, 입력 그대로 (비교는 UPPER)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_MAP_SUB_DEVICE.DEVICE_ID IS '대표 Device = TQP_GDS_MAP.DEVICE_ID (FK 없음)';

EXIT
