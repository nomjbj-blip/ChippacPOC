-- 2026-10-14: TQP_MAPDEF.FACTORY 기본값 FAB2 -> SCK + 남은 FAB2 Map 2건 SCK 로 변경 (로컬 DMSMGR)
-- 원인: 2026-10-13_factory_sck.sql 은 데이터만 SCK 로 바꾸고 컬럼 기본값(DATA_DEFAULT)은 그대로 두었다.
--       Map Define / File Map Setup 의 Map 저장 쿼리(TQP_MAPDEF.xml CREATE_MAP)는 FACTORY 를 넣지 않아 기본값 'FAB2' 로 저장된다.
--       그래서 10-13 이후 File Map Setup 으로 만든 OMM / OMM_TCB 가 FACTORY = FAB2 로 들어갔고(사용 Die TQP_USEMAP 은 SCK),
--       KLARF Upload 의 Map 확인(FACTORY = 로그인 Factory SCK + MAPID)에서 "Map 이 없습니다" 오류가 났다.
-- 확인(실행 전): DMSMGR 전체에서 FACTORY 기본값이 있는 컬럼 = TQC_SERVICE_LOG / TQD_PRODUCT (' ') / TQP_MAPDEF ('FAB2') 뿐,
--               SCK / PROBE 가 아닌 FACTORY 값 = TQP_MAPDEF FAB2 2건(OMM, OMM_TCB) 뿐. 두 MAPID 는 SCK 행이 없어 키 충돌 없음.
-- 실행: Get-Content <이 파일> | docker exec -i oracle19c-DMS bash -lc 'export ORACLE_SID=DMS; sqlplus -s / as sysdba'
-- 고객 DB 도 같은 기본값이면 같은 변경이 필요하다(먼저 2단계 SELECT 로 확인).

set serveroutput on size unlimited
set feedback off
whenever sqlerror exit failure rollback
alter session set current_schema = DMSMGR;

-- 1) 키 충돌 확인: FAB2 행과 같은 MAPID 의 SCK 행이 있으면 중단
declare
  v_conflict number;
begin
  select count(*) into v_conflict
    from tqp_mapdef a
   where a.factory = 'FAB2'
     and exists (select 1 from tqp_mapdef b where b.factory = 'SCK' and b.mapid = a.mapid);
  if v_conflict > 0 then
    raise_application_error(-20001, 'SCK 에 같은 MAPID 가 있는 FAB2 행 ' || v_conflict || '건 - 수동 확인 필요');
  end if;
end;
/

-- 2) 바꾸기 전 상태
select 'BEFORE ' || factory || ' ' || mapid as info from tqp_mapdef where factory <> 'SCK' order by mapid;

-- 3) 기본값 변경 + 데이터 변경
alter table tqp_mapdef modify (factory default 'SCK');
update tqp_mapdef set factory = 'SCK' where factory = 'FAB2';
commit;

-- 4) 확인: 기본값 SCK / FAB1 · FAB2 행 0
declare
  v_default varchar2(100);
  v_left number;
begin
  select data_default into v_default from all_tab_columns
   where owner = 'DMSMGR' and table_name = 'TQP_MAPDEF' and column_name = 'FACTORY';
  select count(*) into v_left from tqp_mapdef where factory in ('FAB1', 'FAB2');
  dbms_output.put_line('TQP_MAPDEF.FACTORY DEFAULT = ' || trim(v_default) || ' / FAB1·FAB2 남은 행 = ' || v_left);
end;
/
select 'AFTER ' || factory || ' ' || mapid as info from tqp_mapdef where mapid like 'OMM%' order by mapid;
exit
