using System;
using System.Data;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// File Map Setup(KLARF 기준) 화면이 신규 Map 을 저장할 때 쓰는 진입점이다. (기존 frmSetupMap 은 자체 저장 코드를 쓴다)
    /// 화면의 m_wMap 설정과 Die 목록을 기존 ProbeAdmin 서비스(SetMapDef -> CreateUseMap)에 전달한다.
    /// 저장 전에 기존 TQP_MAPDEF 데이터 규칙에 맞춰 값을 바꾼다 (ToStoredValues 참고).
    /// </summary>
    internal static class MapCreationService
    {
        /// <summary>
        /// 기존 frmSetupMap.CreateMap 의 인수와 호출 순서를 유지한다.
        /// Map 정의를 SetMapDef 로 저장한 뒤 Die 목록을 CreateUseMap 으로 저장한다.
        /// 두 원격 호출은 하나의 트랜잭션이 아니므로 실패 예외는 호출 화면으로 전달한다.
        /// 중복 확인과 사용자 확인은 화면에서 한다.
        /// </summary>
        public static void Create(string mapId, DACrux.Map.EditWaferMap map)
        {
            if (map == null) throw new ArgumentNullException("map");
            MapDefStoredValues stored = ToStoredValues(map);
            var admin = new DACrux.TEST.RO.ProbeAdmin();
            using (DataTable dies = CreateDieTable(map))
            {
                admin.SetMapDef(mapId,
                    map.WaferSize, map.DieSizeX, map.DieSizeY,
                    stored.OriginMicroX, stored.OriginMicroY, map.OriginIndexX, map.OriginIndexY,
                    map.FirstDieX * map.DieSizeX, map.FirstDieY * map.DieSizeY,
                    map.FirstDieX, map.FirstDieY, stored.EdgeSizeMm, map.NotchAngle,
                    map.NetDie, map.NotchType == DACrux.Base.Notch.Notch ? 0 : 1,
                    0, 0, 0, 0, 0, 0,
                    map.DieMinX, map.DieMaxX, map.DieMinY, map.DieMaxY,
                    (int)map.XYDirect, map.ReferenceDieSetting);
                admin.CreateUseMap(DACrux.Base.GlobalVariable.Factory, mapId, dies);
            }
        }

        /// <summary>
        /// 이미 있는 Map ID 를 화면 Map 으로 덮어쓴다. 기존 Map Define 의 Save As 덮어쓰기(frmSetupMap.UpdateMap)와 같은 서비스를 쓴다.
        /// 1. UpdateMapDef (UPDATE_MAP): TQP_MAPDEF 행의 값을 바꾼다. DELETE_FLAG / REFERENCEDIE_SETTING / INSERT_* 는 바꾸지 않는다.
        /// 2. CreateUseMap: 기존 TQP_USEMAP 행을 지우고 화면 Die 를 다시 넣는다.
        /// 저장값 규칙(Edge mm / Y 원점)과 FIRST_MICRO 계산은 Create 와 같게 해서, 신규 / 덮어쓰기 결과 행이 같은 값이 되게 한다.
        /// (frmSetupMap.UpdateMap 은 FIRST_MICRO 를 (FirstDie - 1) * DieSize 로 넣어 신규와 다르다. 이 화면은 신규 기준으로 맞춘다)
        /// 두 원격 호출은 하나의 트랜잭션이 아니므로 실패 예외는 호출 화면으로 전달한다.
        /// </summary>
        public static void Update(string mapId, DACrux.Map.EditWaferMap map)
        {
            if (map == null) throw new ArgumentNullException("map");
            MapDefStoredValues stored = ToStoredValues(map);
            var admin = new DACrux.TEST.RO.ProbeAdmin();
            using (DataTable dies = CreateDieTable(map))
            {
                admin.UpdateMapDef(mapId,
                    map.WaferSize, map.DieSizeX, map.DieSizeY,
                    stored.OriginMicroX, stored.OriginMicroY, map.OriginIndexX, map.OriginIndexY,
                    map.FirstDieX * map.DieSizeX, map.FirstDieY * map.DieSizeY,
                    map.FirstDieX, map.FirstDieY, stored.EdgeSizeMm, map.NotchAngle,
                    map.NetDie, map.NotchType == DACrux.Base.Notch.Notch ? 0 : 1,
                    0, 0, 0, 0, 0, 0,
                    map.DieMinX, map.DieMaxX, map.DieMinY, map.DieMaxY,
                    (int)map.XYDirect);
                admin.CreateUseMap(DACrux.Base.GlobalVariable.Factory, mapId, dies);
            }
        }

        /// <summary>
        /// 화면(Map 컨트롤) 값을 TQP_MAPDEF 저장값으로 바꾼다.
        /// 왜 (2026-10-06 사용자 확인 / 기존 데이터 403건 기준):
        /// 1. EDGE_SIZE 는 mm 로 저장되어 있다 (기존 2.5 ~ 5). Map Define 의 Edge 입력칸도 최대 5 (mm, 소수 1자리) 라서
        ///    µm 를 그대로 저장하면 Map Define 에서 불러올 때 범위 오류가 난다. 화면 Map 컨트롤은 µm 이므로 1000 으로 나눈다.
        /// 2. 조회 쿼리 (GET_MAPDEF / SELECT_MAPINFO / GET_MAPDEF_DIE_INFO) 는 Y 원점을 CHIP_SIZE_Y - ORIGIN_MICRO_Y 로 돌려준다.
        ///    다시 읽었을 때 화면과 같은 원점이 되도록 ORIGIN_MICRO_Y = DieSizeY - OriginY 로 저장한다. X 는 그대로다.
        /// </summary>
        public static MapDefStoredValues ToStoredValues(DACrux.Map.EditWaferMap map)
        {
            if (map == null) throw new ArgumentNullException("map");
            MapDefStoredValues stored = new MapDefStoredValues();
            stored.EdgeSizeMm = map.EdgeSize / 1000d;
            stored.OriginMicroX = map.OriginX;
            stored.OriginMicroY = map.DieSizeY - map.OriginY;
            return stored;
        }

        /// <summary>
        /// 컨트롤의 Die 좌표와 사용 속성을 기존 TQP_USEMAP 저장 형식으로 변환한다.
        /// 사용(1) / 제거(-1) 등 DieProp 을 그대로 USECODE 로 옮기며 원본 Bin 판정은 추가하지 않는다.
        /// 반환된 DataTable 은 호출자가 Dispose 한다.
        /// </summary>
        public static DataTable CreateDieTable(DACrux.Map.EditWaferMap map)
        {
            if (map == null) throw new ArgumentNullException("map");
            var table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("USECODE", typeof(int));
            for (int index = 0; index < map.Dies.Count; index++)
            {
                DACrux.Base.Die die = map.Dies[index];
                table.Rows.Add(die.IndexX, die.IndexY, die.DieProp);
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
    }
}
