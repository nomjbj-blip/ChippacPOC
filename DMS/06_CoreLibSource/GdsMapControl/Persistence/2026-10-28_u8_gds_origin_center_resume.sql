-- U8 GDS 원점 이동 이어서 실행 - 백업 테이블 값에 이동 값을 더해 쓴다 (2026-10-28)
-- 쓰는 때: 2026-10-22_u8_gds_origin_center.sql 이 백업 테이블(TQP_GDS_PLACED_EL_U8BAK / TQP_GDS_CHAIN_MEMBER_U8BAK)을 만든 뒤
--          이동 단계에서 끝나지 않아(응답 없음 / 접속 끊김) COMMIT 전에 끊긴 경우. 좌표는 그대로이고 백업만 남아 있다.
-- 다른 점: 1) 처음에 U8 행을 잠근다. 다른 세션이 잡고 있으면 30초 뒤 오류로 끝난다(무한 대기 없음).
--          2) 지금 좌표가 백업과 똑같은지 확인한다. 다르면 멈춘다(백업 뒤에 U8 이 바뀜).
--          3) 백업 값 + 이동 값을 쓴다. 다시 실행해도 두 번 옮겨지지 않는다.
--          4) 도형을 10 묶음(ORA_HASH(PLACED_ELEMENT_ID, 9))으로 나눠 묶음마다 결과를 찍는다. COMMIT 은 맨 끝 검증 뒤 한 번.
-- 이동 값 / 바뀌는 곳 / 검증은 2026-10-22_u8_gds_origin_center.sql 과 같다. 되돌리기 = 2026-10-22_u8_gds_origin_center_restore.sql
-- 실행: GDS 테이블 소유 계정으로 sqlplus 에서 실행(같은 폴더에 2026-10-28_u8_gds_origin_center_resume_part.sql 필요).
--       끝나면 SEMDMS Service / DACrux 를 다시 시작한다(서버 / 화면 도형 Cache).

WHENEVER SQLERROR EXIT ROLLBACK
SET SERVEROUTPUT ON
SET FEEDBACK OFF
SET VERIFY OFF
SET TIMING ON

-- 1) U8 행 잠금 (다른 세션이 잡고 있으면 ORA-30006 으로 끝남)
DECLARE
    CURSOR c_el IS SELECT 1 FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' FOR UPDATE WAIT 30;
    CURSOR c_mb IS SELECT 1 FROM TQP_GDS_CHAIN_MEMBER WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' FOR UPDATE WAIT 30;
BEGIN
    DBMS_APPLICATION_INFO.SET_MODULE('U8 GDS origin center', 'lock');
    OPEN c_el;
    CLOSE c_el;
    OPEN c_mb;
    CLOSE c_mb;
    DBMS_OUTPUT.PUT_LINE('U8 행 잠금 ' || TO_CHAR(SYSDATE, 'HH24:MI:SS'));
END;
/

-- 2) 확인: 백업이 옮기기 전 좌표이고, 지금 U8 도형 / Chain 표시점이 백업과 똑같다
DECLARE
    v_el     NUMBER;
    v_bak    NUMBER;
    v_join   NUMBER;
    v_diff   NUMBER;
    v_mb     NUMBER;
    v_mbbak  NUMBER;
    v_mbjoin NUMBER;
    v_mbdiff NUMBER;
    v_minx   BINARY_DOUBLE;
    v_miny   BINARY_DOUBLE;
BEGIN
    DBMS_APPLICATION_INFO.SET_ACTION('check');
    SELECT COUNT(*) INTO v_el FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';
    SELECT COUNT(*), MIN(MIN_X), MIN(MIN_Y) INTO v_bak, v_minx, v_miny FROM TQP_GDS_PLACED_EL_U8BAK;
    SELECT COUNT(*),
           NVL(SUM(CASE WHEN DECODE(N.MIN_X, B.MIN_X, 0, 1) + DECODE(N.MIN_Y, B.MIN_Y, 0, 1)
                           + DECODE(N.MAX_X, B.MAX_X, 0, 1) + DECODE(N.MAX_Y, B.MAX_Y, 0, 1) > 0 THEN 1
                        WHEN N.POINTS_BIN IS NULL AND B.POINTS_BIN IS NULL THEN 0
                        WHEN N.POINTS_BIN IS NULL OR B.POINTS_BIN IS NULL THEN 1
                        WHEN DBMS_LOB.COMPARE(N.POINTS_BIN, B.POINTS_BIN) <> 0 THEN 1
                        ELSE 0 END), 0)
      INTO v_join, v_diff
      FROM TQP_GDS_PLACED_EL N JOIN TQP_GDS_PLACED_EL_U8BAK B
        ON B.FACTORY = N.FACTORY AND B.DEVICE_ID = N.DEVICE_ID AND B.PLACED_ELEMENT_ID = N.PLACED_ELEMENT_ID
     WHERE N.FACTORY = 'SCK' AND N.DEVICE_ID = 'U8';
    SELECT COUNT(*) INTO v_mb FROM TQP_GDS_CHAIN_MEMBER WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';
    SELECT COUNT(*) INTO v_mbbak FROM TQP_GDS_CHAIN_MEMBER_U8BAK;
    SELECT COUNT(*), NVL(SUM(DECODE(N.MARKER_X, B.MARKER_X, 0, 1) + DECODE(N.MARKER_Y, B.MARKER_Y, 0, 1)), 0)
      INTO v_mbjoin, v_mbdiff
      FROM TQP_GDS_CHAIN_MEMBER N JOIN TQP_GDS_CHAIN_MEMBER_U8BAK B
        ON B.FACTORY = N.FACTORY AND B.DEVICE_ID = N.DEVICE_ID AND B.CHAIN_SEQ = N.CHAIN_SEQ AND B.VERSION_NO = N.VERSION_NO
       AND B.PLACED_ELEMENT_ID = N.PLACED_ELEMENT_ID
     WHERE N.FACTORY = 'SCK' AND N.DEVICE_ID = 'U8';
    DBMS_OUTPUT.PUT_LINE('도형 지금 ' || v_el || ' / 백업 ' || v_bak || ' / 짝 ' || v_join || ' / 다름 ' || v_diff
        || ' / 백업 최소 X ' || v_minx || ' / 최소 Y ' || v_miny);
    DBMS_OUTPUT.PUT_LINE('Chain Member 지금 ' || v_mb || ' / 백업 ' || v_mbbak || ' / 짝 ' || v_mbjoin || ' / 표시점 다름 ' || v_mbdiff);
    IF v_el = 0 THEN RAISE_APPLICATION_ERROR(-20001, 'SCK / U8 도형이 없습니다.'); END IF;
    IF v_minx <= 0 OR v_miny <= 0 THEN RAISE_APPLICATION_ERROR(-20002, '백업이 옮기기 전 좌표가 아닙니다(최소 X / Y <= 0).'); END IF;
    IF v_el <> v_bak OR v_join <> v_el OR v_diff > 0 OR v_mb <> v_mbbak OR v_mbjoin <> v_mb OR v_mbdiff > 0 THEN
        RAISE_APPLICATION_ERROR(-20006, '지금 U8 이 백업과 다릅니다. 백업을 다시 만들어야 합니다.');
    END IF;
END;
/

-- 3) 도형 이동: 10 묶음
@@2026-10-28_u8_gds_origin_center_resume_part.sql 0
@@2026-10-28_u8_gds_origin_center_resume_part.sql 1
@@2026-10-28_u8_gds_origin_center_resume_part.sql 2
@@2026-10-28_u8_gds_origin_center_resume_part.sql 3
@@2026-10-28_u8_gds_origin_center_resume_part.sql 4
@@2026-10-28_u8_gds_origin_center_resume_part.sql 5
@@2026-10-28_u8_gds_origin_center_resume_part.sql 6
@@2026-10-28_u8_gds_origin_center_resume_part.sql 7
@@2026-10-28_u8_gds_origin_center_resume_part.sql 8
@@2026-10-28_u8_gds_origin_center_resume_part.sql 9

-- 4) Chain 표시점 이동
DECLARE
    c_dx CONSTANT BINARY_DOUBLE := -11915d;
    c_dy CONSTANT BINARY_DOUBLE := -15026.5d;
BEGIN
    DBMS_APPLICATION_INFO.SET_ACTION('marker');
    MERGE INTO TQP_GDS_CHAIN_MEMBER N
    USING (SELECT FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, PLACED_ELEMENT_ID, MARKER_X, MARKER_Y
             FROM TQP_GDS_CHAIN_MEMBER_U8BAK WHERE MARKER_X IS NOT NULL) B
       ON (N.FACTORY = B.FACTORY AND N.DEVICE_ID = B.DEVICE_ID AND N.CHAIN_SEQ = B.CHAIN_SEQ AND N.VERSION_NO = B.VERSION_NO
           AND N.PLACED_ELEMENT_ID = B.PLACED_ELEMENT_ID)
     WHEN MATCHED THEN UPDATE SET N.MARKER_X = B.MARKER_X + c_dx, N.MARKER_Y = B.MARKER_Y + c_dy;
    DBMS_OUTPUT.PUT_LINE('Chain 표시점 ' || SQL%ROWCOUNT || ' 옮김 ' || TO_CHAR(SYSDATE, 'HH24:MI:SS'));
END;
/

-- 5) 검증: 백업과 차이가 정확히 이동 값 / 첫 점이 새 범위 안 / 점 수 그대로. 하나라도 틀리면 되돌리고 멈춘다
--    백업과의 조인은 U8 조건 + 해시 조인 힌트: 없으면 IX_GDS_PLACED_LAYER 로 행마다 U8 전체를 훑는 Nested Loop 가 나온다.
DECLARE
    v_bad_box  NUMBER;
    v_bad_pt   NUMBER;
    v_bad_len  NUMBER;
    v_bad_mk   NUMBER;
    v_minx BINARY_DOUBLE; v_maxx BINARY_DOUBLE; v_miny BINARY_DOUBLE; v_maxy BINARY_DOUBLE;
BEGIN
    DBMS_APPLICATION_INFO.SET_ACTION('verify');
    SELECT /*+ USE_HASH(N B) */ COUNT(*) INTO v_bad_box
      FROM TQP_GDS_PLACED_EL N JOIN TQP_GDS_PLACED_EL_U8BAK B
        ON B.FACTORY = N.FACTORY AND B.DEVICE_ID = N.DEVICE_ID AND B.PLACED_ELEMENT_ID = N.PLACED_ELEMENT_ID
     WHERE N.FACTORY = 'SCK' AND N.DEVICE_ID = 'U8'
       AND (ABS((N.MIN_X - B.MIN_X) - (-11915d)) > 1e-6 OR ABS((N.MAX_Y - B.MAX_Y) - (-15026.5d)) > 1e-6);
    SELECT /*+ USE_HASH(N B) */ COUNT(*) INTO v_bad_len
      FROM TQP_GDS_PLACED_EL N JOIN TQP_GDS_PLACED_EL_U8BAK B
        ON B.FACTORY = N.FACTORY AND B.DEVICE_ID = N.DEVICE_ID AND B.PLACED_ELEMENT_ID = N.PLACED_ELEMENT_ID
     WHERE N.FACTORY = 'SCK' AND N.DEVICE_ID = 'U8'
       AND NVL(DBMS_LOB.GETLENGTH(N.POINTS_BIN), -1) <> NVL(DBMS_LOB.GETLENGTH(B.POINTS_BIN), -1);
    SELECT COUNT(*) INTO v_bad_pt
      FROM TQP_GDS_PLACED_EL
     WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' AND POINTS_BIN IS NOT NULL
       AND (UTL_RAW.CAST_TO_BINARY_DOUBLE(DBMS_LOB.SUBSTR(POINTS_BIN, 8, 1), 2) NOT BETWEEN MIN_X - 1e-6 AND MAX_X + 1e-6
         OR UTL_RAW.CAST_TO_BINARY_DOUBLE(DBMS_LOB.SUBSTR(POINTS_BIN, 8, 9), 2) NOT BETWEEN MIN_Y - 1e-6 AND MAX_Y + 1e-6);
    SELECT COUNT(*) INTO v_bad_mk
      FROM TQP_GDS_CHAIN_MEMBER N JOIN TQP_GDS_CHAIN_MEMBER_U8BAK B
        ON B.FACTORY = N.FACTORY AND B.DEVICE_ID = N.DEVICE_ID AND B.CHAIN_SEQ = N.CHAIN_SEQ AND B.VERSION_NO = N.VERSION_NO
       AND B.PLACED_ELEMENT_ID = N.PLACED_ELEMENT_ID
     WHERE B.MARKER_X IS NOT NULL AND (ABS((N.MARKER_X - B.MARKER_X) - (-11915d)) > 1e-6 OR ABS((N.MARKER_Y - B.MARKER_Y) - (-15026.5d)) > 1e-6);
    SELECT MIN(MIN_X), MAX(MAX_X), MIN(MIN_Y), MAX(MAX_Y) INTO v_minx, v_maxx, v_miny, v_maxy
      FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';
    DBMS_OUTPUT.PUT_LINE('검증: 범위 차이 틀림 ' || v_bad_box || ' / 점 목록 길이 틀림 ' || v_bad_len
        || ' / 첫 점 범위 밖 ' || v_bad_pt || ' / 표시점 틀림 ' || v_bad_mk);
    DBMS_OUTPUT.PUT_LINE('옮긴 뒤 범위: X ' || v_minx || ' ~ ' || v_maxx || ' / Y ' || v_miny || ' ~ ' || v_maxy);
    IF v_bad_box + v_bad_len + v_bad_pt + v_bad_mk > 0 THEN
        RAISE_APPLICATION_ERROR(-20004, '검증 실패 - 되돌립니다(백업 테이블은 남음).');
    END IF;
END;
/

COMMIT;
EXEC DBMS_APPLICATION_INFO.SET_MODULE(NULL, NULL)
PROMPT U8 GDS 원점 이동 완료. SEMDMS Service / DACrux 를 다시 시작하세요.
