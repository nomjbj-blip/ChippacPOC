using System;
using System.Data;
using System.Drawing;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// File Map Setup(KLARF 기준) 화면이 신규 Map 을 저장할 때 쓰는 진입점이다. (기존 frmSetupMap 은 자체 저장 코드를 쓴다)
    /// 화면의 m_wMap 설정과 Die 목록을 기존 ProbeAdmin 서비스(SetMapDef -> CreateUseMap)에 전달한다.
    /// 저장 전에 기존 TQP_MAPDEF 데이터 규칙에 맞춰 값을 바꾼다 (ToStoredValues 참고).
    ///
    /// 화면 Map 컨트롤은 항상 LeftBottom 번호로 그리고, 작업자가 고른 XY 방향은 저장할 때 번호로 바꾼다 (2026-10-13).
    /// 이유: 기존 WaferMap 은 LeftBottom 이 아닌 방향에서 마우스 위치 Die 판정(OnMouseMove)과 Die 그리기(AddDie)가
    /// 한 칸 어긋난다 (LeftTop 은 최상단 행이 잡히지 않고 2번째 행에서 최상단 Die 가 잡힘, 클릭 편집도 옆 Die 에 적용됨).
    /// LeftBottom 은 두 계산이 같아서, 화면은 LeftBottom 으로 두고 저장값만 선택 방향으로 만든다. 저장 결과는 같다.
    /// </summary>
    internal static class MapCreationService
    {
        /// <summary>
        /// 기존 frmSetupMap.CreateMap 의 인수와 호출 순서를 유지한다.
        /// Map 정의를 SetMapDef 로 저장한 뒤 Die 목록을 CreateUseMap 으로 저장한다.
        /// 두 원격 호출은 하나의 트랜잭션이 아니므로 실패 예외는 호출 화면으로 전달한다.
        /// 중복 확인과 저장 확인창은 화면에서 한다.
        /// </summary>
        /// <param name="map">LeftBottom 번호로 그린 화면 Map</param>
        /// <param name="direction">저장할 XY 방향</param>
        /// <param name="grid">Wafer 격자 (번호 변환 범위)</param>
        public static void Create(string mapId, DACrux.Map.EditWaferMap map, XYDirection direction, KlarfMapGrid grid)
        {
            MapDefStoredValues stored = ToStoredValues(map, direction, grid);
            var admin = new DACrux.TEST.RO.ProbeAdmin();
            using (DataTable dies = CreateDieTable(map, direction, grid))
            {
                admin.SetMapDef(mapId,
                    map.WaferSize, map.DieSizeX, map.DieSizeY,
                    stored.OriginMicroX, stored.OriginMicroY, stored.OriginIndexX, stored.OriginIndexY,
                    map.FirstDieX * map.DieSizeX, map.FirstDieY * map.DieSizeY,
                    map.FirstDieX, map.FirstDieY, stored.EdgeSizeMm, map.NotchAngle,
                    map.NetDie, map.NotchType == DACrux.Base.Notch.Notch ? 0 : 1,
                    0, 0, 0, 0, 0, 0,
                    map.DieMinX, map.DieMaxX, map.DieMinY, map.DieMaxY,
                    stored.XYDirection, map.ReferenceDieSetting);
                admin.CreateUseMap(DACrux.Base.GlobalVariable.Factory, mapId, dies);
            }
        }

        /// <summary>
        /// 이미 있는 Map ID 를 화면 Map 으로 덮어쓴다. 기존 Map Define 의 Save As 덮어쓰기(frmSetupMap.UpdateMap)와 같은 서비스를 쓴다.
        /// 1. UpdateMapDef (UPDATE_MAP): TQP_MAPDEF 행의 값을 바꾼다. DELETE_FLAG / REFERENCEDIE_SETTING / INSERT_* 는 바꾸지 않는다.
        /// 2. CreateUseMap: 기존 TQP_USEMAP 행을 지우고 화면 Die 를 다시 넣는다.
        /// 저장값 규칙(Edge mm / Y 원점 / XY 방향 번호)과 FIRST_MICRO 계산은 Create 와 같게 해서, 신규 / 덮어쓰기 결과 행이 같은 값이 되게 한다.
        /// (frmSetupMap.UpdateMap 은 FIRST_MICRO 를 (FirstDie - 1) * DieSize 로 넣어 신규와 다르다. 이 화면은 신규 기준으로 맞춘다)
        /// 두 원격 호출은 하나의 트랜잭션이 아니므로 실패 예외는 호출 화면으로 전달한다.
        /// </summary>
        public static void Update(string mapId, DACrux.Map.EditWaferMap map, XYDirection direction, KlarfMapGrid grid)
        {
            MapDefStoredValues stored = ToStoredValues(map, direction, grid);
            var admin = new DACrux.TEST.RO.ProbeAdmin();
            using (DataTable dies = CreateDieTable(map, direction, grid))
            {
                admin.UpdateMapDef(mapId,
                    map.WaferSize, map.DieSizeX, map.DieSizeY,
                    stored.OriginMicroX, stored.OriginMicroY, stored.OriginIndexX, stored.OriginIndexY,
                    map.FirstDieX * map.DieSizeX, map.FirstDieY * map.DieSizeY,
                    map.FirstDieX, map.FirstDieY, stored.EdgeSizeMm, map.NotchAngle,
                    map.NetDie, map.NotchType == DACrux.Base.Notch.Notch ? 0 : 1,
                    0, 0, 0, 0, 0, 0,
                    map.DieMinX, map.DieMaxX, map.DieMinY, map.DieMaxY,
                    stored.XYDirection);
                admin.CreateUseMap(DACrux.Base.GlobalVariable.Factory, mapId, dies);
            }
        }

        /// <summary>
        /// 화면(Map 컨트롤) 값을 TQP_MAPDEF 저장값으로 바꾼다.
        /// 왜 (2026-10-06 확인 / 기존 데이터 403건 기준):
        /// 1. EDGE_SIZE 는 mm 로 저장되어 있다 (기존 2.5 ~ 5). Map Define 의 Edge 입력칸도 최대 5 (mm, 소수 1자리) 라서
        ///    µm 를 그대로 저장하면 Map Define 에서 불러올 때 범위 오류가 난다. 화면 Map 컨트롤은 µm 이므로 1000 으로 나눈다.
        /// 2. 조회 쿼리 (GET_MAPDEF / SELECT_MAPINFO / GET_MAPDEF_DIE_INFO) 는 Y 원점을 CHIP_SIZE_Y - ORIGIN_MICRO_Y 로 돌려준다.
        ///    다시 읽었을 때 화면과 같은 원점이 되도록 ORIGIN_MICRO_Y = DieSizeY - OriginY 로 저장한다. X 는 그대로다.
        /// 3. ORIGIN_INDEX / XY_DIRECTION 은 선택한 XY 방향 기준으로 만든다 (KlarfMapRule.MapOriginIndex). (2026-10-13)
        ///    화면 Map 은 LeftBottom 이므로, 기존 Map Define 이 이 방향으로 AddDie 해도 같은 위치에 그려지는 원점 인덱스다.
        /// </summary>
        public static MapDefStoredValues ToStoredValues(DACrux.Map.EditWaferMap map, XYDirection direction, KlarfMapGrid grid)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (grid == null) throw new ArgumentNullException("grid");
            Point originIndex = KlarfMapRule.MapOriginIndex(direction, grid);
            MapDefStoredValues stored = new MapDefStoredValues();
            stored.EdgeSizeMm = map.EdgeSize / 1000d;
            stored.OriginMicroX = map.OriginX;
            stored.OriginMicroY = map.DieSizeY - map.OriginY;
            stored.OriginIndexX = originIndex.X;
            stored.OriginIndexY = originIndex.Y;
            stored.XYDirection = (int)direction;
            return stored;
        }

        /// <summary>
        /// 컨트롤의 Die 좌표와 사용 속성을 기존 TQP_USEMAP 저장 형식으로 변환한다.
        /// Die 번호는 LeftBottom -> 선택한 XY 방향으로 바꾼다 (KlarfMapRule.ToDirection, 형상은 같고 번호만 바뀜).
        /// 사용(1) / 제거(-1) 등 DieProp 을 그대로 USECODE 로 옮기며 원본 Bin 판정은 추가하지 않는다.
        /// 반환된 DataTable 은 호출자가 Dispose 한다.
        /// </summary>
        public static DataTable CreateDieTable(DACrux.Map.EditWaferMap map, XYDirection direction, KlarfMapGrid grid)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (grid == null) throw new ArgumentNullException("grid");
            var table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("USECODE", typeof(int));
            for (int index = 0; index < map.Dies.Count; index++)
            {
                DACrux.Base.Die die = map.Dies[index];
                Point number = KlarfMapRule.ToDirection(new Point(die.IndexX, die.IndexY), direction, grid);
                table.Rows.Add(number.X, number.Y, die.DieProp);
            }
            return table;
        }
    }

    /// <summary>TQP_MAPDEF 에 넣을 때 화면 값과 단위 / 기준이 다른 항목. 나머지는 Map 컨트롤 값을 그대로 쓴다.</summary>
    internal sealed class MapDefStoredValues
    {
        /// <summary>EDGE_SIZE (mm).</summary>
        public double EdgeSizeMm { get; set; }

        /// <summary>ORIGIN_MICRO_X (µm) = 화면 OriginX.</summary>
        public double OriginMicroX { get; set; }

        /// <summary>ORIGIN_MICRO_Y (µm) = DieSizeY - 화면 OriginY. 조회 쿼리가 다시 DieSizeY - 값 으로 되돌린다.</summary>
        public double OriginMicroY { get; set; }

        /// <summary>ORIGIN_INDEX_X / Y = 선택한 XY 방향 기준 원점 인덱스.</summary>
        public int OriginIndexX { get; set; }
        public int OriginIndexY { get; set; }

        /// <summary>XY_DIRECTION = 선택한 XY 방향 (XYDirection 값).</summary>
        public int XYDirection { get; set; }
    }
}
