-- 2026-10-06 Claude CLAUDE-028: GDS_SHA256 NOT NULL 해제
-- 사용자 지시 "빼줘": GDS Map Setup 화면이 저장 시작 전에 원본 GDS 파일 SHA-256 을 계산하지 않는다.
-- 이 값은 재등록 확인창의 "같은 파일 / 다른 파일" 표시에만 쓰였다. 컬럼은 남기고(기존 행 값 보존) 새 저장은 NULL 로 둔다.
-- 서버(BSL GdsMapImport.BeginMapImport)는 값이 비어 있으면 검증을 건너뛰고, 값이 오면 지금처럼 64자리 16진수인지 검사한다.
-- 적용: 2026-10-06 로컬 docker oracle19c DMSMGR (사용자 승인 "진행"). 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

ALTER TABLE DMSMGR.TQP_GDS_MAP MODIFY (GDS_SHA256 NULL);

COMMENT ON COLUMN DMSMGR.TQP_GDS_MAP.GDS_SHA256 IS '원본 GDS 파일 SHA-256 (2026-10-06 이후 저장은 계산하지 않아 NULL, 이전 행만 값 있음)';

EXIT
