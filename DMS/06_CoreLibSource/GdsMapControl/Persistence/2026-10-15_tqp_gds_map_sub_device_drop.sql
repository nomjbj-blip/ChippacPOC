-- 2026-10-15: GDS Sub Device 테이블 TQP_GDS_MAP_SUB_DEVICE 삭제 (2026-10-12_tqp_gds_map_sub_device.sql 로 만든 테이블)
-- GDS Map 복사(GdsMapImport.CopyMap)로 같은 도면을 다른 Device 이름으로 만들 수 있어 Sub Device 연결을 쓰지 않는다.
-- KLARF Product / E-Test Device 는 이름이 같은(대소문자 무시) READY GDS Map 에 연결된다. 이름이 다르면 GDS Map 조회 화면 [Map 복사]로 그 이름의 Map 을 만든다.
-- 지우기 전 로컬 데이터(2026-10-15 확인): SCK / U9 <- OMM, OMM_Hybrid
-- 설계: 문서/설계/02_GDS/2026-10-15_GDS_Map_복사_설계.md 6절
-- 적용 순서: 이 테이블을 쓰지 않는 서버 DLL(Interface / DSL / BSL / RO)과 XML 을 배포한 뒤 적용한다.
--           그 전에 지우면 배포된 이전 서버의 KLARF 연관 분석 / E-Test Para 가 테이블 없음 오류를 낸다.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON

DROP TABLE DMSMGR.TQP_GDS_MAP_SUB_DEVICE PURGE;

EXIT
