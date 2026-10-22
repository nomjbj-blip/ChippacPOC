-- 고객 DB 에 TS_TEST_TBL / TS_TEST_IDX 테이블스페이스 만들기 + 대상 사용자 QUOTA (2026-10-21)
-- 이유: E-Test 업로드가 TD_* 테이블을 만들 때 테이블은 TS_TEST_TBL, PK 인덱스는 TS_TEST_IDX 로 이름을 지정한다
--       (09_TEST/DACrux.TEST.DSL/TD_TABLE.cs). 대상 DB 에 이 두 테이블스페이스가 없으면 ORA-00959 로 실패한다.
-- 실행 계정: CREATE TABLESPACE / ALTER USER 권한이 있는 계정(DBA).
-- 대상 사용자: 아래 DEFINE TARGET_USER (기본 TRAINING). 다른 사용자면 이 줄만 바꾼다.
-- 데이터 파일 위치:
--   db_create_file_dest 가 있으면 그 위치에 이름 자동(OMF),
--   없으면 USERS(없으면 SYSTEM) 테이블스페이스 데이터 파일이 있는 폴더에 <이름>.dbf 로 만든다(ASM 이면 같은 디스크 그룹).
-- 크기: 로컬 기준 DB 와 같게 시작 TS_TEST_TBL 50MB / TS_TEST_IDX 25MB, 10MB 씩 자동 증가, 최대 UNLIMITED.
-- 이미 있으면 만들지 않고 QUOTA 만 준다. 여러 번 실행해도 된다.
-- 실행 순서: 이 파일(DBA) -> 스키마 DDL(대상 사용자) -> 2026-10-21_ts_test_mapping.sql(대상 사용자)

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE ON
SET VERIFY OFF
DEFINE TARGET_USER = TRAINING
WHENEVER SQLERROR EXIT SQL.SQLCODE

DECLARE
    v_omf  VARCHAR2(512);
    v_file VARCHAR2(600);
    v_loc  VARCHAR2(600);

    PROCEDURE make_ts(p_name VARCHAR2, p_size_mb NUMBER) IS
        n     NUMBER;
        v_sql VARCHAR2(2000);
    BEGIN
        SELECT COUNT(*) INTO n FROM DBA_TABLESPACES WHERE TABLESPACE_NAME = p_name;
        IF n > 0 THEN
            DBMS_OUTPUT.PUT_LINE('이미 있음: ' || p_name);
        ELSE
            IF v_omf IS NOT NULL THEN
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE SIZE ' || p_size_mb || 'M';
            ELSIF v_loc LIKE '+%' THEN
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE ''' || v_loc || ''' SIZE ' || p_size_mb || 'M';
            ELSE
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE ''' || v_loc || p_name || '.dbf'' SIZE ' || p_size_mb || 'M';
            END IF;
            v_sql := v_sql || ' AUTOEXTEND ON NEXT 10M MAXSIZE UNLIMITED'
                           || ' EXTENT MANAGEMENT LOCAL AUTOALLOCATE SEGMENT SPACE MANAGEMENT AUTO';
            DBMS_OUTPUT.PUT_LINE(v_sql || ';');
            EXECUTE IMMEDIATE v_sql;
            DBMS_OUTPUT.PUT_LINE('생성: ' || p_name);
        END IF;
        v_sql := 'ALTER USER &TARGET_USER QUOTA UNLIMITED ON ' || p_name;
        DBMS_OUTPUT.PUT_LINE(v_sql || ';');
        EXECUTE IMMEDIATE v_sql;
    END;
BEGIN
    SELECT VALUE INTO v_omf FROM V$PARAMETER WHERE NAME = 'db_create_file_dest';
    IF v_omf IS NULL THEN
        SELECT FILE_NAME INTO v_file
          FROM (SELECT FILE_NAME FROM DBA_DATA_FILES
                 WHERE TABLESPACE_NAME IN ('USERS', 'SYSTEM')
                 ORDER BY DECODE(TABLESPACE_NAME, 'USERS', 1, 2), FILE_ID)
         WHERE ROWNUM = 1;
        IF v_file LIKE '+%' THEN
            v_loc := SUBSTR(v_file, 1, INSTR(v_file || '/', '/') - 1);          -- ASM 디스크 그룹(+DATA)
        ELSE
            v_loc := SUBSTR(v_file, 1, GREATEST(INSTR(v_file, '/', -1), INSTR(v_file, '\', -1)));
        END IF;
        DBMS_OUTPUT.PUT_LINE('데이터 파일 위치: ' || v_loc || ' (기준 파일 ' || v_file || ')');
    ELSE
        DBMS_OUTPUT.PUT_LINE('데이터 파일 위치: OMF ' || v_omf);
    END IF;

    make_ts('TS_TEST_TBL', 50);
    make_ts('TS_TEST_IDX', 25);
END;
/

PROMPT == 확인 ==
SET PAGESIZE 50 LINESIZE 200
COLUMN TABLESPACE_NAME FORMAT A15
COLUMN FILE_NAME FORMAT A80
SELECT T.TABLESPACE_NAME, T.STATUS, F.FILE_NAME, ROUND(F.BYTES / 1024 / 1024) MB, F.AUTOEXTENSIBLE
  FROM DBA_TABLESPACES T JOIN DBA_DATA_FILES F ON F.TABLESPACE_NAME = T.TABLESPACE_NAME
 WHERE T.TABLESPACE_NAME IN ('TS_TEST_TBL', 'TS_TEST_IDX')
 ORDER BY 1;
SELECT USERNAME, TABLESPACE_NAME, MAX_BYTES FROM DBA_TS_QUOTAS
 WHERE USERNAME = UPPER('&TARGET_USER') AND TABLESPACE_NAME IN ('TS_TEST_TBL', 'TS_TEST_IDX')
 ORDER BY 2;
