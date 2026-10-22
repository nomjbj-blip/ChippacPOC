-- 기본 데이터 INSERT 스크립트 추출 (2026-10-21, 읽기 전용)
-- 목적: 새 DB 에 스키마 DDL 을 만든 뒤, POC 데이터 없이 로그인해서 화면을 쓰는 데 필요한 기본 데이터만 옮긴다.
--   로그인 / 메뉴 / 메뉴 권한은 ADMIN 계정이 소스에 고정되어 있어 DB 데이터가 없어도 된다(frmLoginDACrux / UXUtil.AccessCheck).
-- 대상 (인자로 받은 스키마, 기본 DMSMGR)
--   권한: TQC_SECURITY_GROUP / TQC_FUNCTION_LIST / TQC_GROUP_FUNCTION (ADMIN 외 사용자를 만들고 권한을 줄 때)
--   설정: TQC_CONFIG (시험 범주 BJ_TEST 제외, 암호 항목은 빈 값, DEFECT_FTP 는 실행할 때 대상 서버 값으로 넣는다)
--         TQC_CONFIG_USER (USERID = ADMIN 만)
--   Defect 기준: TQD_DEFECT_GROUP (GROUP_ID = 1 Default 만) / TQD_DEFECT_TYPE (그 그룹의 유형) / TQD_COLORBYSIZE (USER_ID = ADMIN 만)
-- 제외: POC 데이터(GDS / KLARF / E-Test / YMS / Map), 사용자(TQC_USER / TQC_GROUP_USER, 원본에 없음),
--       설비 / 공정(TQC_EQUIP / TQC_EQUIP_MODEL / TQC_OPER, 장비별 자동 적재 서비스와 PCM 설비 화면에서만 사용), 로그 / 이력 테이블
-- 결과: 컨테이너 /tmp/basedata/base_data.sql (UTF-8)
-- 실행: Export-BaseData.ps1 이 부른다.
--   docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba @/tmp/basedata/extract_base_data.sql DMSMGR'

WHENEVER SQLERROR EXIT SQL.SQLCODE
SET PAGESIZE 0 LINESIZE 32767 TRIMSPOOL ON TAB OFF FEEDBACK OFF HEADING OFF VERIFY OFF ECHO OFF TERMOUT OFF NEWPAGE NONE
SET SERVEROUTPUT ON SIZE UNLIMITED FORMAT WRAPPED

VARIABLE owner VARCHAR2(128)
EXEC :owner := UPPER('&1')

SPOOL /tmp/basedata/base_data.sql

DECLARE
    TYPE t_lines IS TABLE OF VARCHAR2(32767);
    v_owner VARCHAR2(128) := :owner;
    v_total NUMBER := 0;

    PROCEDURE emit(p_text VARCHAR2) IS
    BEGIN
        DBMS_OUTPUT.PUT_LINE(p_text);
    END;

    -- p_table 의 컬럼 순서대로 INSERT 문을 만든다. p_from 은 별칭 t 를 쓰는 FROM 절(부분 쿼리 가능), 숨김 / 가상 컬럼은 뺀다.
    -- 문자열은 작은따옴표를 두 번 쓰고 줄바꿈은 CHR(13) / CHR(10) 으로 바꿔 한 줄에 쓴다. 지원하지 않는 형식이면 오류로 멈춘다.
    PROCEDURE gen(p_table VARCHAR2, p_from VARCHAR2, p_order VARCHAR2, p_note VARCHAR2) IS
        v_cols VARCHAR2(32767);
        v_expr VARCHAR2(32767);
        v_rows t_lines;
        v_one  VARCHAR2(4000);
    BEGIN
        FOR c IN (SELECT COLUMN_NAME, DATA_TYPE FROM DBA_TAB_COLS
                   WHERE OWNER = v_owner AND TABLE_NAME = p_table AND HIDDEN_COLUMN = 'NO' AND VIRTUAL_COLUMN = 'NO'
                   ORDER BY COLUMN_ID) LOOP
            IF c.DATA_TYPE IN ('VARCHAR2', 'CHAR', 'NVARCHAR2', 'NCHAR') THEN
                v_one := 'NVL2(t."' || c.COLUMN_NAME || q'[", '''' || REPLACE(REPLACE(REPLACE(t."]' || c.COLUMN_NAME
                      || q'[", '''', ''''''), CHR(13), ''' || CHR(13) || '''), CHR(10), ''' || CHR(10) || ''') || '''', 'NULL')]';
            ELSIF c.DATA_TYPE IN ('NUMBER', 'FLOAT', 'BINARY_DOUBLE', 'BINARY_FLOAT') THEN
                v_one := 'NVL(TO_CHAR(t."' || c.COLUMN_NAME || q'[", 'TM9', 'NLS_NUMERIC_CHARACTERS=''.,'''), 'NULL')]';
            ELSIF c.DATA_TYPE = 'DATE' THEN
                v_one := 'NVL2(t."' || c.COLUMN_NAME || q'[", 'TO_DATE(''' || TO_CHAR(t."]' || c.COLUMN_NAME
                      || q'[", 'YYYY-MM-DD HH24:MI:SS') || ''', ''YYYY-MM-DD HH24:MI:SS'')', 'NULL')]';
            ELSE
                RAISE_APPLICATION_ERROR(-20002, 'Unsupported type ' || c.DATA_TYPE || ': ' || p_table || '.' || c.COLUMN_NAME);
            END IF;
            v_cols := v_cols || CASE WHEN v_cols IS NOT NULL THEN ', ' END || '"' || c.COLUMN_NAME || '"';
            v_expr := v_expr || CASE WHEN v_expr IS NOT NULL THEN q'[ || ', ' || ]' END || v_one;
        END LOOP;
        IF v_cols IS NULL THEN
            RAISE_APPLICATION_ERROR(-20003, 'Table not found: ' || v_owner || '.' || p_table);
        END IF;

        EXECUTE IMMEDIATE 'SELECT ''INSERT INTO "' || p_table || '" (' || v_cols || ') VALUES ('' || ' || v_expr
                       || q'[ || ');' FROM ]' || p_from || ' ORDER BY ' || p_order
            BULK COLLECT INTO v_rows;

        emit('');
        emit('-- ' || p_table || ' ' || v_rows.COUNT || '건' || CASE WHEN p_note IS NOT NULL THEN ' (' || p_note || ')' END);
        FOR i IN 1 .. v_rows.COUNT LOOP
            emit(v_rows(i));
        END LOOP;
        v_total := v_total + v_rows.COUNT;
    END;

    FUNCTION src(p_table VARCHAR2) RETURN VARCHAR2 IS
    BEGIN
        RETURN '"' || v_owner || '"."' || p_table || '"';
    END;
BEGIN
    emit('-- ' || v_owner || ' 기본 데이터 (POC 데이터 제외, 로그인 후 화면 사용에 필요한 최소 데이터)');
    emit('-- 추출: ' || TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI') || ' / 원본 DB ' || SYS_CONTEXT('USERENV', 'DB_NAME'));
    emit('-- 먼저 스키마 DDL(<날짜>_DMSMGR_schema_ddl.sql)을 같은 사용자로 실행해 테이블을 만든 뒤 실행한다.');
    emit('-- 로그인: 소스에 고정된 ADMIN 계정을 쓴다(사용자 테이블은 비어 있다). ADMIN 은 메뉴 권한 검사도 하지 않는다.');
    emit('-- 실행: 주석 / 설정의 한글이 깨지지 않게 NLS_LANG 을 UTF-8 로 지정한 뒤 sqlplus <사용자>/<비밀번호>@<대상 DB> @<이 파일>');
    emit('--   실행하면 DEFECT_FTP 암호를 묻는다(화면에 보이지 않음). 아래 DEFINE 의 FTP 주소 / 계정 / 경로는 대상 서버에 맞게 고친다.');
    emit('--   대상 테이블에 이미 행이 있으면 아무것도 넣지 않고 멈춘다. 중간에 오류가 나면 모두 되돌린다(ROLLBACK).');
    emit('');
    emit('-- DEFECT_FTP: Defect 이미지를 받는 FTP. 화면은 MAIN_PATH + DB 이미지 경로(KLARF_IMAGE_ROOT 에서 드라이브만 뺀 경로)로 받는다.');
    emit('DEFINE FTP_IP = 10.86.253.162');
    emit('DEFINE FTP_PORT = 21');
    emit('DEFINE FTP_ID = POC');
    emit(q'[DEFINE FTP_MAIN_PATH = '/DMS_IMAGE']');
    emit(q'[ACCEPT FTP_PASS CHAR DEFAULT ' ' PROMPT 'DEFECT_FTP password: ' HIDE]');
    emit('');
    emit('SET DEFINE OFF');
    emit('SET SQLBLANKLINES ON');
    emit('SET VERIFY OFF');
    emit('SET FEEDBACK OFF');
    emit('SPOOL base_data_run.log');
    emit('WHENEVER SQLERROR EXIT FAILURE ROLLBACK');
    emit('');
    emit('-- 0. 대상 테이블이 비어 있는지 확인');
    emit('DECLARE');
    emit('    n NUMBER;');
    emit('BEGIN');
    FOR t IN (SELECT COLUMN_VALUE TNAME FROM TABLE(SYS.ODCIVARCHAR2LIST('TQC_SECURITY_GROUP', 'TQC_FUNCTION_LIST', 'TQC_GROUP_FUNCTION',
                     'TQC_CONFIG', 'TQC_CONFIG_USER', 'TQD_DEFECT_GROUP', 'TQD_DEFECT_TYPE', 'TQD_COLORBYSIZE'))) LOOP
        emit('    SELECT COUNT(*) INTO n FROM "' || t.TNAME || '";');
        emit('    IF n > 0 THEN RAISE_APPLICATION_ERROR(-20001, ''STOP: ' || t.TNAME || ' already has '' || n || '' rows. Nothing inserted.''); END IF;');
    END LOOP;
    emit('END;');
    emit('/');

    emit('');
    emit('-- 1. 권한 (ADMIN 외 사용자를 만들 때 쓴다)');
    gen('TQC_SECURITY_GROUP', src('TQC_SECURITY_GROUP') || ' t', '1', NULL);
    gen('TQC_FUNCTION_LIST', src('TQC_FUNCTION_LIST') || ' t', '1', NULL);
    gen('TQC_GROUP_FUNCTION', src('TQC_GROUP_FUNCTION') || ' t', '1', NULL);

    emit('');
    emit('-- 2. 설정');
    gen('TQC_CONFIG',
        '(SELECT CATEGORY, NAME, CASE WHEN UPPER(NAME) LIKE ''%PASS%'' OR UPPER(NAME) LIKE ''%PW%'' THEN '' '' ELSE VALUE END VALUE, TYPE, "COMMENT"'
        || ' FROM ' || src('TQC_CONFIG') || ' WHERE CATEGORY NOT IN (''BJ_TEST'', ''DEFECT_FTP'')) t',
        '1', '시험 범주 BJ_TEST 와 DEFECT_FTP 제외, 암호 항목은 빈 값');
    emit('');
    emit('SET DEFINE ON');
    gen('TQC_CONFIG',
        '(SELECT CATEGORY, NAME, CHR(38) || ''FTP_'' || NAME VALUE, TYPE, "COMMENT" FROM ' || src('TQC_CONFIG')
        || ' WHERE CATEGORY = ''DEFECT_FTP'') t',
        '1', 'DEFECT_FTP, 값은 위 DEFINE / ACCEPT');
    emit('SET DEFINE OFF');
    gen('TQC_CONFIG_USER', src('TQC_CONFIG_USER') || ' t WHERE t.USERID = ''ADMIN''', '1', 'ADMIN 만');

    emit('');
    emit('-- 3. Defect 기준');
    gen('TQD_DEFECT_GROUP', src('TQD_DEFECT_GROUP') || ' t WHERE t.GROUP_ID = 1', '1', 'Default 그룹만');
    gen('TQD_DEFECT_TYPE', src('TQD_DEFECT_TYPE') || ' t WHERE t.GROUP_ID = 1', '1', 'Default 그룹 유형');
    gen('TQD_COLORBYSIZE', src('TQD_COLORBYSIZE') || ' t WHERE t.USER_ID = ''ADMIN''', '1', 'ADMIN 만');

    emit('');
    emit('COMMIT;');
    emit('WHENEVER SQLERROR CONTINUE');
    emit('');
    emit('-- 4. 확인 (기대값: 이 파일의 표 머리 건수, 합계 ' || v_total || '건)');
    emit('SET PAGESIZE 100 FEEDBACK ON');
    emit(q'[SELECT 'TQC_SECURITY_GROUP' T, COUNT(*) N FROM TQC_SECURITY_GROUP UNION ALL SELECT 'TQC_FUNCTION_LIST', COUNT(*) FROM TQC_FUNCTION_LIST UNION ALL ]'
      || q'[SELECT 'TQC_GROUP_FUNCTION', COUNT(*) FROM TQC_GROUP_FUNCTION UNION ALL SELECT 'TQC_CONFIG', COUNT(*) FROM TQC_CONFIG UNION ALL ]'
      || q'[SELECT 'TQC_CONFIG_USER', COUNT(*) FROM TQC_CONFIG_USER UNION ALL SELECT 'TQD_DEFECT_GROUP', COUNT(*) FROM TQD_DEFECT_GROUP UNION ALL ]'
      || q'[SELECT 'TQD_DEFECT_TYPE', COUNT(*) FROM TQD_DEFECT_TYPE UNION ALL SELECT 'TQD_COLORBYSIZE', COUNT(*) FROM TQD_COLORBYSIZE;]');
    emit(q'[SELECT NAME, CASE WHEN NAME = 'PASS' THEN '(set)' ELSE VALUE END VALUE FROM TQC_CONFIG WHERE CATEGORY = 'DEFECT_FTP' ORDER BY NAME;]');
    emit('SPOOL OFF');
END;
/

SPOOL OFF
EXIT
