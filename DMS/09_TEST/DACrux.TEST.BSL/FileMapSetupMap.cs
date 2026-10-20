using System;
using System.Data;
using System.Globalization;
using DACrux.TEST.DSL;

namespace DACrux.TEST.BSL
{
    /// <summary>
    /// File Map Setup(TEST.ENGUI MapCreationService)으로 저장한 Map 을 YMS Map 화면에 DMS 화면(Wafer Map 조회 / KLARF 조회)과 같은 모양으로 넘긴다. (2026-10-14)
    /// 왜:
    ///  - YMS Map 화면은 XY 방향을 LeftBottom 으로 고정해 그린다. File Map Setup 은 원점 인덱스 / Die 번호를 선택한 XY 방향 기준으로 저장한다
    ///    (TEST.ENGUI KlarfMapRule.MapOriginIndex / ToDirection). DMS 화면은 저장값을 LeftBottom 으로 되돌려 그리는데(StoredMapLayout.ToLeftBottomOrigin /
    ///    KlarfMapRule.FromDirection) YMS 는 저장값을 그대로 써서 LeftTop Map 이 X 1칸 밀려 보였다.
    ///  - 측정하지 않은 Map Die 는 그리지 않는다(2026-10-14). Map 사용 Die 이면서 결과 파일에 있는 Die(TD 데이터)만 보인다.
    ///    Mark Die 는 Map 종류와 관계없이 TQP_MAPDEF_DIE(GET_MAPDEF_DIE) 그대로이고, File Map Setup Map 은 이 행이 없어 Mark Die 가 없다.
    /// 구분: TQP_MAPDEF_DIE 의 Die 행이 없고 TQP_USEMAP 행이 있는 Map 만 File Map Setup Map 으로 본다. 기존 Map Define 으로 만든 Map 은 그대로 둔다.
    /// 식은 TEST.ENGUI KlarfMapRule / StoredMapLayout 과 같다(서버는 ENGUI 를 참조하지 않아 여기 다시 둔다).
    /// XY 방향 값: 0 LeftTop / 1 LeftBottom / 2 RightBottom / 3 RightTop.
    /// </summary>
    public static class FileMapSetupMap
    {
        private const int LeftTop = 0;
        private const int RightBottom = 2;
        private const int RightTop = 3;

        /// <summary>File Map Setup 으로 저장한 Map 인지 (Mark Die 0 행 + Map Die 1 행 이상).</summary>
        public static bool Is(string mapId)
        {
            if (string.IsNullOrEmpty(mapId)) return false;
            DataTable dt = new TQP_MAPDEF().GetData("SELECT_MAP_DIE_SOURCE", null, new string[] { mapId, mapId });
            return dt != null && dt.Rows.Count > 0 && ToInt(dt.Rows[0]["MARK_DIE_COUNT"]) == 0 && ToInt(dt.Rows[0]["USE_DIE_COUNT"]) > 0;
        }

        /// <summary>
        /// Map 정의 행(GET_MAPDEF / GET_MAPDEF_MAP 결과)의 ORIGIN_INDEX_X / Y 를 LeftBottom 원점으로 바꾼다. File Map Setup Map 이 아니면 그대로 둔다.
        /// 행의 XY_DIRECTION 은 저장값이어야 한다 -> 부르는 쪽이 XY_DIRECTION 을 LeftBottom(1)으로 바꾸기 전에 부른다.
        /// </summary>
        public static void ToLeftBottomOrigin(DataRow mapDef, string mapId)
        {
            if (mapDef == null || !Is(mapId)) return;
            int direction = ToInt(mapDef["XY_DIRECTION"]);
            int x = ToInt(mapDef["ORIGIN_INDEX_X"]);
            int y = ToInt(mapDef["ORIGIN_INDEX_Y"]);
            int sumX = ToInt(mapDef["DIE_INDEX_MIN_X"]) + ToInt(mapDef["DIE_INDEX_MAX_X"]);
            int sumY = ToInt(mapDef["DIE_INDEX_MIN_Y"]) + ToInt(mapDef["DIE_INDEX_MAX_Y"]);
            switch (direction)
            {
                case LeftTop:
                    x = x + 1;
                    y = sumY - y;
                    break;
                case RightTop:
                    x = sumX - x;
                    y = sumY - y;
                    break;
                case RightBottom:
                    x = sumX - x;
                    break;
            }
            SetValue(mapDef, "ORIGIN_INDEX_X", x);
            SetValue(mapDef, "ORIGIN_INDEX_Y", y);
        }

        private static int ToInt(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            return (int)Math.Round(Convert.ToDouble(value, CultureInfo.InvariantCulture));
        }

        /// <summary>열 형식(DB 숫자는 decimal)에 맞춰 값을 넣는다.</summary>
        private static void SetValue(DataRow row, string column, object value)
        {
            if (!row.Table.Columns.Contains(column)) return;
            Type type = row.Table.Columns[column].DataType;
            row[column] = type == typeof(object) ? value : Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }
    }
}
