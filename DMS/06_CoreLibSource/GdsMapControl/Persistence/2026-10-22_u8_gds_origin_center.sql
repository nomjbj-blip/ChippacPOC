-- U8 GDS 좌표 원점을 Chip(Die 칸) 가운데로 옮긴다 (2026-10-22)
-- 이유: U8 GDS(OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds)는 원점이 Die 왼쪽 아래(= KLARF 원점)라서,
--       원점을 Chip 가운데로 보는 연관 분석 변환(GDS = XREL - Pitch / 2)에서 Defect 가 Pitch 절반만큼 밀린다.
--       분석 로직은 그대로 두고 U8 의 저장 좌표만 U9 / OMM 과 같은 Chip 가운데 원점으로 옮긴다.
-- 옮기는 값: U8 KLARF(XD093.01.001) DiePitch 23,830 x 30,053 의 절반 -> X -11915, Y -15026.5
-- 바뀌는 곳: TQP_GDS_PLACED_EL (MIN_X / MIN_Y / MAX_X / MAX_Y / POINTS_BIN), TQP_GDS_CHAIN_MEMBER (MARKER_X / MARKER_Y)
-- 백업: TQP_GDS_PLACED_EL_U8BAK / TQP_GDS_CHAIN_MEMBER_U8BAK (이 스크립트가 만든다, 되돌리기 = 2026-10-22_u8_gds_origin_center_restore.sql)
-- 실행: GDS 테이블 소유 계정으로 sqlplus 에서 실행. 끝나면 SEMDMS Service / DACrux 를 다시 시작한다(서버 / 화면 도형 Cache).
-- 주의: GDS Map 설정 화면에서 U8 을 GDS 파일로 다시 저장하면 원래 좌표로 돌아간다(그때 이 스크립트를 다시 실행).
-- 두 번 실행 방지: 이미 옮겼으면(도형 범위 가운데가 0 근처) 또는 백업 테이블이 있으면 멈춘다.

WHENEVER SQLERROR EXIT ROLLBACK
SET SERVEROUTPUT ON
SET FEEDBACK OFF

-- 1) 확인: 대상 Map 이 있고, 아직 옮기지 않았고(도형 X 최소값 > 0), 점 목록이 2000 byte 이하
DECLARE
    v_cnt   NUMBER;
    v_minx  BINARY_DOUBLE;
    v_miny  BINARY_DOUBLE;
    v_len   NUMBER;
BEGIN
    SELECT COUNT(*), MIN(MIN_X), MIN(MIN_Y), MAX(DBMS_LOB.GETLENGTH(POINTS_BIN))
      INTO v_cnt, v_minx, v_miny, v_len
      FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';
    IF v_cnt = 0 THEN RAISE_APPLICATION_ERROR(-20001, 'SCK / U8 도형이 없습니다.'); END IF;
    IF v_minx <= 0 OR v_miny <= 0 THEN RAISE_APPLICATION_ERROR(-20002, '이미 옮긴 것 같습니다(도형 최소 X / Y <= 0).'); END IF;
    IF v_len > 2000 THEN RAISE_APPLICATION_ERROR(-20003, '점 목록이 2000 byte 를 넘는 도형이 있습니다: ' || v_len); END IF;
    DBMS_OUTPUT.PUT_LINE('대상 도형 ' || v_cnt || ' / 옮기기 전 최소 X ' || v_minx || ' / 최소 Y ' || v_miny);
END;
/

-- 2) 백업 (이미 있으면 여기서 멈춘다)
CREATE TABLE TQP_GDS_PLACED_EL_U8BAK AS
SELECT FACTORY, DEVICE_ID, PLACED_ELEMENT_ID, MIN_X, MIN_Y, MAX_X, MAX_Y, POINTS_BIN
  FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';
CREATE TABLE TQP_GDS_CHAIN_MEMBER_U8BAK AS
SELECT FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, PLACED_ELEMENT_ID, MARKER_X, MARKER_Y
  FROM TQP_GDS_CHAIN_MEMBER WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';

-- 3) 이동: POINTS_BIN v1 = 점마다 X, Y double little-endian(16 byte)
--    2026-10-28 갱신에 PK 인덱스 힌트 추가: 통계가 없는 DB 에서 IX_GDS_PLACED_LAYER 를 앞 두 컬럼으로만 타서 도형마다 U8 전체를 훑는 계획이 나와 끝나지 않았다.
DECLARE
    c_dx CONSTANT BINARY_DOUBLE := -11915d;
    c_dy CONSTANT BINARY_DOUBLE := -15026.5d;
    TYPE t_id  IS TABLE OF TQP_GDS_PLACED_EL.PLACED_ELEMENT_ID%TYPE;
    TYPE t_bin IS TABLE OF RAW(2000);
    v_ids  t_id;
    v_bins t_bin;
    v_rows PLS_INTEGER := 0;
    CURSOR c_el IS
        SELECT PLACED_ELEMENT_ID, DBMS_LOB.SUBSTR(POINTS_BIN, 2000, 1)
          FROM TQP_GDS_PLACED_EL WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8';

    FUNCTION shifted(p_bin RAW) RETURN RAW IS
        v_out RAW(2000);
        v_x   BINARY_DOUBLE;
        v_y   BINARY_DOUBLE;
    BEGIN
        IF p_bin IS NULL THEN RETURN NULL; END IF;
        FOR i IN 0 .. UTL_RAW.LENGTH(p_bin) / 16 - 1 LOOP
            v_x := UTL_RAW.CAST_TO_BINARY_DOUBLE(UTL_RAW.SUBSTR(p_bin, i * 16 + 1, 8), UTL_RAW.LITTLE_ENDIAN) + c_dx;
            v_y := UTL_RAW.CAST_TO_BINARY_DOUBLE(UTL_RAW.SUBSTR(p_bin, i * 16 + 9, 8), UTL_RAW.LITTLE_ENDIAN) + c_dy;
            v_out := UTL_RAW.CONCAT(v_out, UTL_RAW.CAST_FROM_BINARY_DOUBLE(v_x, UTL_RAW.LITTLE_ENDIAN),
                                           UTL_RAW.CAST_FROM_BINARY_DOUBLE(v_y, UTL_RAW.LITTLE_ENDIAN));
        END LOOP;
        RETURN v_out;
    END;
BEGIN
    OPEN c_el;
    LOOP
        FETCH c_el BULK COLLECT INTO v_ids, v_bins LIMIT 5000;
        EXIT WHEN v_ids.COUNT = 0;
        FOR i IN 1 .. v_bins.COUNT LOOP
            v_bins(i) := shifted(v_bins(i));
        END LOOP;
        FORALL i IN 1 .. v_ids.COUNT
            UPDATE /*+ INDEX(T PK_GDS_PLACED_EL) */ TQP_GDS_PLACED_EL T
               SET POINTS_BIN = v_bins(i),
                   MIN_X = MIN_X + c_dx, MAX_X = MAX_X + c_dx,
                   MIN_Y = MIN_Y + c_dy, MAX_Y = MAX_Y + c_dy
             WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' AND PLACED_ELEMENT_ID = v_ids(i);
        v_rows := v_rows + v_ids.COUNT;
    END LOOP;
    CLOSE c_el;

    UPDATE TQP_GDS_CHAIN_MEMBER
       SET MARKER_X = MARKER_X + c_dx, MARKER_Y = MARKER_Y + c_dy
     WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' AND MARKER_X IS NOT NULL;
    DBMS_OUTPUT.PUT_LINE('도형 ' || v_rows || ' / Chain 표시점 ' || SQL%ROWCOUNT || ' 옮김');
END;
/

-- 4) 검증: 백업과 차이가 정확히 이동 값 / 첫 점이 새 범위 안 / 점 수 그대로. 하나라도 틀리면 되돌리고 멈춘다
--    2026-10-28 백업과의 조인에 U8 조건 + 해시 조인 힌트 추가: 없으면 IX_GDS_PLACED_LAYER 로 행마다 U8 전체를 훑는 Nested Loop 가 나온다.
DECLARE
    v_bad_box  NUMBER;
    v_bad_pt   NUMBER;
    v_bad_len  NUMBER;
    v_bad_mk   NUMBER;
    v_minx BINARY_DOUBLE; v_maxx BINARY_DOUBLE; v_miny BINARY_DOUBLE; v_maxy BINARY_DOUBLE;
BEGIN
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
PROMPT U8 GDS 원점 이동 완료. SEMDMS Service / DACrux 를 다시 시작하세요.
