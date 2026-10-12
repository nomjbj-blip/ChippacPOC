-- 2026-10-06: GDS Map 저장 기준(Front / Back side) 컬럼 추가
-- 설계: 문서/2026-10-06_GDS_반전_BackSide_설계.md 8절
-- GDS 원본은 Front side 기준이다. GDS Map Setup 화면에서 DB 저장 시 Back side를 고르면 PLACED 도형 좌표를
-- GDS 원점 기준 좌우 반전(X' = -X)해 저장하고, 이 컬럼에 BACK을 기록한다. SOURCE(원본 Grid)는 항상 원본이다.
-- 기존 행은 DEFAULT로 FRONT가 된다(지금까지는 반전 없이 저장했으므로).
-- 적용: 2026-10-06 로컬 docker oracle19c DMSMGR (승인 "승인"). 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQP_GDS_MAP ADD (
    VIEW_SIDE VARCHAR2(10) DEFAULT 'FRONT' NOT NULL
        CONSTRAINT CK_GDS_MAP_VIEW_SIDE CHECK (VIEW_SIDE IN ('FRONT', 'BACK'))
);

COMMENT ON COLUMN DMSMGR.TQP_GDS_MAP.VIEW_SIDE IS 'PLACED 좌표 기준: FRONT = GDS 원본, BACK = GDS 원점 기준 좌우 반전(X'' = -X)';

EXIT
