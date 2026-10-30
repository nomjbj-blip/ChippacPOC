-- U8 GDS 원점 이동 한 묶음 (2026-10-28). 2026-10-28_u8_gds_origin_center_resume.sql 에서 부른다.
-- 인자 1 = 묶음 번호 0 ~ 9 (ORA_HASH(PLACED_ELEMENT_ID, 9)). 백업 좌표 + 이동 값을 쓴다. COMMIT 은 부르는 쪽에서 한다.
-- POINTS_BIN v1 = 점마다 X, Y double little-endian(16 byte)
-- 갱신은 PK 인덱스 힌트를 쓴다: 통계가 없는 DB 에서 IX_GDS_PLACED_LAYER 를 앞 두 컬럼으로만 타서 도형마다 U8 전체를 훑는 계획이 나왔다.
DECLARE
    c_dx CONSTANT BINARY_DOUBLE := -11915d;
    c_dy CONSTANT BINARY_DOUBLE := -15026.5d;
    TYPE t_id    IS TABLE OF TQP_GDS_PLACED_EL_U8BAK.PLACED_ELEMENT_ID%TYPE;
    TYPE t_coord IS TABLE OF BINARY_DOUBLE;
    TYPE t_bin   IS TABLE OF RAW(2000);
    v_ids  t_id;
    v_minx t_coord;
    v_miny t_coord;
    v_maxx t_coord;
    v_maxy t_coord;
    v_bins t_bin;
    v_rows PLS_INTEGER := 0;
    v_upd  PLS_INTEGER := 0;
    CURSOR c_el IS
        SELECT PLACED_ELEMENT_ID, MIN_X, MIN_Y, MAX_X, MAX_Y, DBMS_LOB.SUBSTR(POINTS_BIN, 2000, 1)
          FROM TQP_GDS_PLACED_EL_U8BAK
         WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' AND ORA_HASH(PLACED_ELEMENT_ID, 9) = &1;

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
    DBMS_APPLICATION_INFO.SET_ACTION('part &1');
    OPEN c_el;
    LOOP
        FETCH c_el BULK COLLECT INTO v_ids, v_minx, v_miny, v_maxx, v_maxy, v_bins LIMIT 5000;
        EXIT WHEN v_ids.COUNT = 0;
        FOR i IN 1 .. v_bins.COUNT LOOP
            v_bins(i) := shifted(v_bins(i));
        END LOOP;
        FORALL i IN 1 .. v_ids.COUNT
            UPDATE /*+ INDEX(T PK_GDS_PLACED_EL) */ TQP_GDS_PLACED_EL T
               SET POINTS_BIN = v_bins(i),
                   MIN_X = v_minx(i) + c_dx, MAX_X = v_maxx(i) + c_dx,
                   MIN_Y = v_miny(i) + c_dy, MAX_Y = v_maxy(i) + c_dy
             WHERE FACTORY = 'SCK' AND DEVICE_ID = 'U8' AND PLACED_ELEMENT_ID = v_ids(i);
        v_upd := v_upd + SQL%ROWCOUNT;
        v_rows := v_rows + v_ids.COUNT;
    END LOOP;
    CLOSE c_el;
    IF v_upd <> v_rows THEN
        RAISE_APPLICATION_ERROR(-20005, '묶음 &1: 읽은 도형 ' || v_rows || ' / 바뀐 도형 ' || v_upd);
    END IF;
    DBMS_OUTPUT.PUT_LINE('묶음 &1: 도형 ' || v_rows || ' 옮김 ' || TO_CHAR(SYSDATE, 'HH24:MI:SS'));
END;
/
