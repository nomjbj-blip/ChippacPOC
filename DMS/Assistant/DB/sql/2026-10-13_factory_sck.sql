-- 2026-10-13: Factory FAB1 / FAB2 -> SCK 통합 + 백업 테이블 삭제 (로컬 DMSMGR)
-- 결정: FAB1 / FAB2 모두 SCK 로 변경 / 같은 키가 겹치면 SCK > FAB2 > FAB1 순으로 남김 / *_BAK 등 백업 테이블 삭제.
-- 점검: 문서/2026-10-13_Factory_SCK_통합_점검.md
--
-- 순서
--   1) 겹치는 키 정리: FACTORY 가 들어간 UNIQUE 인덱스마다, 우선순위가 높은 Factory 에 같은 키가 있으면 낮은 쪽 행 삭제
--      + TQC_CONFIG_USER(키 없음) 는 (USERID, CATEGORY, NAME) 기준 같은 방식
--   2) 충돌 재확인: 남은 충돌이 있으면 오류로 중단 (UPDATE 전)
--   3) FACTORY / FACILITY 컬럼의 FAB1 / FAB2 -> SCK (테이블마다 COMMIT)
--   4) 백업 테이블 DROP (휴지통으로 이동, PURGE 하지 않음)
--   5) 남은 FAB1 / FAB2 0건 확인 + 통계 재수집
-- 바꾸지 않는 것: HANDLER 컬럼의 HandlerPcm_Fab2 등(클래스 이름), 로그 MESSAGE 문구, MESMGR(MRC).
-- 실행 전 백업: Data Pump factory_sck_20261013.dmp (DATA_PUMP_DIR)
-- 실행: Get-Content <이 파일> | docker exec -i oracle19c bash -lc 'sqlplus -s / as sysdba'

set serveroutput on size unlimited
set feedback off
whenever sqlerror exit failure rollback
alter session set current_schema = DMSMGR;

declare
  -- 삭제할 백업 테이블 (실행 소스에서 사용하지 않음)
  type t_list is table of varchar2(128);
  v_bak t_list := t_list('TQD_COLORBYSIZE_BAK', 'TQD_DATA_MAINT_HIS_BAK', 'TQD_DEFECT_BAK', 'TQD_DEFECT_TYPE_BAK',
                         'TQD_IMAGES_BAK', 'TQD_INSP_INFO_BAK', 'TQD_INSP_SUM_BAK', 'TQD_LOT_BAK', 'TQD_PRODUCT_BAK',
                         'TQD_PRODUCT_BAK2', 'TQD_SETUP_BAK', 'TQD_SETUP_MAP_BAK', 'TQD_STEP_BAK', 'TQD_WAFER_BAK',
                         'TQP_DATA_TABLES_BAK', 'TQP_PARASPEC_1106', 'TQP_PARASPEC_BAK', 'TQP_WAFER_SUM_BAK');
  c_rank constant varchar2(100) := 'decode(%A%.FACTORY, ''SCK'', 3, ''FAB2'', 2, ''FAB1'', 1, 0)';
  v_sql  varchar2(4000);
  v_on   varchar2(2000);
  v_n    number;
  v_bad  number := 0;

  function is_bak(p varchar2) return boolean is
  begin
    for i in 1 .. v_bak.count loop if v_bak(i) = p then return true; end if; end loop;
    return false;
  end;

  -- 키(FACTORY 제외) 기준으로 우선순위 낮은 Factory 행 삭제
  procedure dedup(p_table varchar2, p_on varchar2) is
  begin
    v_sql := 'delete from ' || p_table || ' a where a.FACTORY in (''FAB1'', ''FAB2'') and exists (select 1 from '
          || p_table || ' b where ' || p_on || ' and ' || replace(c_rank, '%A%', 'b') || ' > ' || replace(c_rank, '%A%', 'a') || ')';
    execute immediate v_sql;
    if sql%rowcount > 0 then dbms_output.put_line('dedup ' || p_table || ' deleted=' || sql%rowcount); end if;
  end;

  function key_on(p_owner varchar2, p_index varchar2) return varchar2 is
    v varchar2(2000);
  begin
    for k in (select column_name from dba_ind_columns where index_owner = p_owner and index_name = p_index
              and column_name <> 'FACTORY' order by column_position) loop
      v := v || case when v is not null then ' and ' end || 'b."' || k.column_name || '" = a."' || k.column_name || '"';
    end loop;
    return v;
  end;
begin
  -- 1) 겹치는 키 정리
  for u in (select i.table_name, i.index_name from dba_indexes i
            where i.owner = 'DMSMGR' and i.uniqueness = 'UNIQUE'
              and exists (select 1 from dba_ind_columns c where c.index_owner = i.owner and c.index_name = i.index_name and c.column_name = 'FACTORY')
              and not exists (select 1 from dba_ind_columns c where c.index_owner = i.owner and c.index_name = i.index_name and c.column_name like 'SYS_NC%')
            order by i.table_name) loop
    if not is_bak(u.table_name) then
      v_on := key_on('DMSMGR', u.index_name);
      if v_on is not null then dedup(u.table_name, v_on); end if;
    end if;
  end loop;
  dedup('TQC_CONFIG_USER', 'b.USERID = a.USERID and b.CATEGORY = a.CATEGORY and b.NAME = a.NAME');
  commit;

  -- 2) 충돌 재확인 (FAB1 / FAB2 / SCK 를 하나로 봤을 때 키가 두 Factory 에 걸치는지)
  for u in (select i.table_name, i.index_name from dba_indexes i
            where i.owner = 'DMSMGR' and i.uniqueness = 'UNIQUE'
              and exists (select 1 from dba_ind_columns c where c.index_owner = i.owner and c.index_name = i.index_name and c.column_name = 'FACTORY')
              and not exists (select 1 from dba_ind_columns c where c.index_owner = i.owner and c.index_name = i.index_name and c.column_name like 'SYS_NC%')) loop
    if not is_bak(u.table_name) then
      v_on := key_on('DMSMGR', u.index_name);
      if v_on is not null then
        execute immediate 'select count(*) from ' || u.table_name || ' a where a.FACTORY in (''FAB1'', ''FAB2'', ''SCK'') and exists (select 1 from '
          || u.table_name || ' b where ' || v_on || ' and b.FACTORY in (''FAB1'', ''FAB2'', ''SCK'') and b.FACTORY <> a.FACTORY)' into v_n;
        if v_n > 0 then dbms_output.put_line('CONFLICT ' || u.table_name || ' ' || u.index_name || ' rows=' || v_n); v_bad := v_bad + 1; end if;
      end if;
    end if;
  end loop;
  if v_bad > 0 then raise_application_error(-20001, 'key conflict remains, stop before update'); end if;

  -- 3) FACTORY / FACILITY -> SCK
  for c in (select t.table_name, c.column_name from dba_tab_columns c join dba_tables t on t.owner = c.owner and t.table_name = c.table_name
            where c.owner = 'DMSMGR' and c.column_name in ('FACTORY', 'FACILITY') and c.data_type in ('VARCHAR2', 'CHAR')
            order by t.num_rows nulls first, t.table_name) loop
    if not is_bak(c.table_name) then
      execute immediate 'update ' || c.table_name || ' set "' || c.column_name || '" = ''SCK'' where "' || c.column_name || '" in (''FAB1'', ''FAB2'')';
      v_n := sql%rowcount;
      commit;
      if v_n > 0 then dbms_output.put_line('update ' || c.table_name || '.' || c.column_name || ' rows=' || v_n); end if;
    end if;
  end loop;

  -- 4) 백업 테이블 삭제
  for i in 1 .. v_bak.count loop
    select count(*) into v_n from dba_tables where owner = 'DMSMGR' and table_name = v_bak(i);
    if v_n = 1 then
      execute immediate 'drop table ' || v_bak(i);
      dbms_output.put_line('drop ' || v_bak(i));
    end if;
  end loop;

  -- 5) 확인 + 통계
  v_bad := 0;
  for c in (select c.table_name, c.column_name from dba_tab_columns c join dba_tables t on t.owner = c.owner and t.table_name = c.table_name
            where c.owner = 'DMSMGR' and c.column_name in ('FACTORY', 'FACILITY') and c.data_type in ('VARCHAR2', 'CHAR')) loop
    execute immediate 'select count(*) from ' || c.table_name || ' where "' || c.column_name || '" in (''FAB1'', ''FAB2'')' into v_n;
    if v_n > 0 then dbms_output.put_line('REMAIN ' || c.table_name || '.' || c.column_name || ' rows=' || v_n); v_bad := v_bad + 1; end if;
  end loop;
  dbms_output.put_line('remaining FAB columns=' || v_bad);
  dbms_stats.gather_schema_stats('DMSMGR', options => 'GATHER STALE');
  dbms_output.put_line('stats gathered (stale)');
end;
/
exit
