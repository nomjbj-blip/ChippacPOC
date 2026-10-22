using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// Wafer Map 조회 화면(frmStandardMapView)이 DB 에 저장된 Map 을 화면 Map 컨트롤 기준으로 바꾸고, 저장값과 Die 행이 맞는지 확인한다.
    /// 입력: GET_MAPDEF 1행 (TQP_MAPDEF) + SELECT_USEMAP_ALLDIE 결과 (TQP_USEMAP 의 X / Y / USECODE).
    /// 화면 코드와 나눈 이유: DB 없이 같은 계산을 검사(Verification/StandardMapViewSmoke)하기 위해서다.
    ///
    /// 화면 Map 컨트롤은 저장 XY 방향과 관계없이 LeftBottom 번호로 그린다 (File Map Setup 과 같은 방식, 2026-10-13).
    /// 기존 WaferMap 은 LeftBottom 이 아닌 방향에서 마우스 위치 Die 판정과 AddDie 위치가 한 칸 어긋나기 때문이다 (MapCreationService 주석 참고).
    /// 1. Die 번호: 저장 번호 -> KlarfMapRule.FromDirection (Right 계열은 X, Top 계열은 Y 를 저장 Die 범위 안에서 뒤집음)
    /// 2. 원점 인덱스: 저장값은 KlarfMapRule.MapOriginIndex(방향) 결과이므로 그 식을 거꾸로 푼다 (ToLeftBottomOrigin)
    /// 두 계산이 같은 범위 합(DIE_INDEX_MIN + MAX)을 쓰므로, 기존 Map Define 으로 저장한 Map 도 저장 방향으로 그린 것과 같은 위치가 된다.
    /// </summary>
    internal sealed class StoredMapLayout
    {
        /// <summary>EDGE_SIZE 저장 단위는 mm 이고 기존 Map Define 입력칸 최대값이 5 mm 다 (2026-10-06 확인).</summary>
        private const double MaxEdgeMm = 5d;
        /// <summary>WAFER_SIZE 는 µm 로 저장한다 (300 mm = 300000). 이보다 작으면 mm 로 들어간 값으로 본다.</summary>
        private const double MinWaferMicron = 1000d;
        /// <summary>CHIP_SIZE 는 µm 로 저장한다. 10 µm 보다 작으면 mm 로 들어간 값으로 본다 (로컬 DB PX52: 3.34 x 3.12, 2026-10-13 확인).</summary>
        private const double MinDieMicron = 10d;

        private StoredMapLayout()
        {
            Dies = new List<StoredMapDie>();
            Checks = new List<StoredMapCheck>();
        }

        public string MapId { get; private set; }
        public string Factory { get; private set; }
        /// <summary>WAFER_SIZE (µm). 화면 Map 컨트롤에 그대로 넣는다.</summary>
        public double WaferSize { get; private set; }
        public double DieSizeX { get; private set; }
        public double DieSizeY { get; private set; }
        /// <summary>원점 (µm). GET_MAPDEF 가 Y 를 CHIP_SIZE_Y - ORIGIN_MICRO_Y 로 돌려주므로 화면 기준 값이다.</summary>
        public double OriginX { get; private set; }
        public double OriginY { get; private set; }
        /// <summary>EDGE_SIZE (mm, 저장값). 화면 Map 컨트롤에는 µm 로 바꿔 넣는다.</summary>
        public double EdgeSizeMm { get; private set; }
        public int NotchAngle { get; private set; }
        public Notch NotchType { get; private set; }
        public int StoredXYDirection { get; private set; }
        /// <summary>XY_DIRECTION 이 XYDirection 값(0 ~ 3)이 아니면 false. 이때는 Map 을 그리지 않는다.</summary>
        public bool IsDirectionValid { get; private set; }
        /// <summary>EDGE_SIZE 가 mm 저장 범위(0 ~ 5) 안인지. 밖이면 Map 에는 Edge 0 으로 그린다 (Wafer 반지름보다 크면 Map 컨트롤이 그리지 못함).</summary>
        public bool IsEdgeValid
        {
            get { return EdgeSizeMm >= 0 && EdgeSizeMm <= MaxEdgeMm; }
        }
        public XYDirection Direction { get; private set; }
        /// <summary>저장 DIE_INDEX_MIN / MAX 범위. 저장 번호를 뒤집는 기준이다 (원점 인덱스는 쓰지 않음).</summary>
        public KlarfMapGrid StoredGrid { get; private set; }
        public Point StoredOriginIndex { get; private set; }
        /// <summary>화면 Map 컨트롤(LeftBottom)에 넣을 원점 인덱스.</summary>
        public Point LeftBottomOriginIndex { get; private set; }
        public int FirstIndexX { get; private set; }
        public int FirstIndexY { get; private set; }
        public int StoredNetDie { get; private set; }
        public int ReferenceDieSetting { get; private set; }

        /// <summary>TQP_USEMAP 행. 저장 번호 / LeftBottom 번호 / USECODE.</summary>
        public List<StoredMapDie> Dies { get; private set; }
        public int UsedCount { get; private set; }
        public int RemovedCount { get; private set; }
        public int OtherCount { get; private set; }
        /// <summary>Die 표에 DIE_NUM 컬럼이 있는지 (GetDies 결과에는 있다). 없으면 DIE_NUM 확인 항목을 만들지 않는다.</summary>
        public bool HasStoredDieNum { get; private set; }
        /// <summary>LeftBottom 번호로 바꾼 Die 의 범위. Die 행이 없으면 저장 범위와 같다.</summary>
        public int LeftBottomMinX { get; private set; }
        public int LeftBottomMaxX { get; private set; }
        public int LeftBottomMinY { get; private set; }
        public int LeftBottomMaxY { get; private set; }

        /// <summary>데이터 확인 표 항목. Wafer 밖 사용 Die 는 Map 을 그린 뒤 AddOutsideWaferCheck 로 추가한다.</summary>
        public List<StoredMapCheck> Checks { get; private set; }

        /// <summary>다름 항목 수 (탭 머리글 표시용).</summary>
        public int DifferCount
        {
            get
            {
                int count = 0;
                foreach (StoredMapCheck check in Checks) if (check.Result == StoredMapCheckResult.Differ) count++;
                return count;
            }
        }

        /// <summary>
        /// 저장된 Map 정의 1행과 Die 표로 화면 기준 값과 확인 항목을 만든다.
        /// 흐름: 정의값 읽기 -> XY 방향 확인 -> 원점 인덱스 역산 -> Die 번호 변환 / 개수 / 범위 / 중복 -> 확인 항목 작성.
        /// </summary>
        /// <param name="mapDef">GET_MAPDEF 결과 행 (컬럼 이름은 쿼리와 같음)</param>
        /// <param name="dies">SELECT_USEMAP_ALLDIE 결과 (X / Y / USECODE). null 이면 0행으로 본다</param>
        public static StoredMapLayout Create(DataRow mapDef, DataTable dies)
        {
            if (mapDef == null) throw new ArgumentNullException("mapDef");
            StoredMapLayout layout = new StoredMapLayout();
            layout.MapId = Text(mapDef, "MAPID");
            layout.Factory = Text(mapDef, "FACTORY");
            layout.WaferSize = Number(mapDef, "WAFER_SIZE");
            layout.DieSizeX = Number(mapDef, "CHIP_SIZE_X");
            layout.DieSizeY = Number(mapDef, "CHIP_SIZE_Y");
            layout.OriginX = Number(mapDef, "ORIGIN_MICRO_X");
            layout.OriginY = Number(mapDef, "ORIGIN_MICRO_Y");
            layout.EdgeSizeMm = Number(mapDef, "EDGE_SIZE");
            layout.NotchAngle = Integer(mapDef, "ANGLE");
            layout.NotchType = Integer(mapDef, "NOTCH_TYPE") == 0 ? Notch.Notch : Notch.Flat;
            layout.FirstIndexX = Integer(mapDef, "FIRST_INDEX_X");
            layout.FirstIndexY = Integer(mapDef, "FIRST_INDEX_Y");
            layout.StoredNetDie = Integer(mapDef, "NETDIE");
            layout.ReferenceDieSetting = Integer(mapDef, "REFERENCEDIE_SETTING");
            layout.StoredOriginIndex = new Point(Integer(mapDef, "ORIGIN_INDEX_X"), Integer(mapDef, "ORIGIN_INDEX_Y"));
            layout.StoredGrid = new KlarfMapGrid(Integer(mapDef, "DIE_INDEX_MIN_X"), Integer(mapDef, "DIE_INDEX_MAX_X"),
                Integer(mapDef, "DIE_INDEX_MIN_Y"), Integer(mapDef, "DIE_INDEX_MAX_Y"), 0, 0, 0);

            layout.StoredXYDirection = Integer(mapDef, "XY_DIRECTION");
            layout.IsDirectionValid = Enum.IsDefined(typeof(XYDirection), layout.StoredXYDirection);
            layout.Direction = layout.IsDirectionValid ? (XYDirection)layout.StoredXYDirection : XYDirection.LeftBottom;
            layout.LeftBottomOriginIndex = ToLeftBottomOrigin(layout.Direction, layout.StoredOriginIndex, layout.StoredGrid);

            layout.ReadDies(dies);
            layout.NumberDies();
            layout.BuildChecks();
            return layout;
        }

        /// <summary>
        /// 저장 원점 인덱스(선택 방향 기준)를 LeftBottom 원점 인덱스로 되돌린다. KlarfMapRule.MapOriginIndex 를 거꾸로 푼 식이다.
        /// MapOriginIndex: LeftTop (sx - 1, sumY - sy) / RightTop (sumX - sx, sumY - sy) / RightBottom (sumX - sx, sy) / LeftBottom (sx, sy)
        /// 이 식은 AddDie 의 방향별 원점 모서리 차이(KlarfMapRule 주석)에서 나왔으므로, 같은 범위 합으로 Die 번호도 뒤집으면
        /// 저장 방향으로 AddDie 한 위치와 LeftBottom 으로 AddDie 한 위치가 같다.
        /// </summary>
        public static Point ToLeftBottomOrigin(XYDirection direction, Point storedOrigin, KlarfMapGrid grid)
        {
            int sumX = grid.MinX + grid.MaxX;
            int sumY = grid.MinY + grid.MaxY;
            switch (direction)
            {
                case XYDirection.LeftTop:
                    return new Point(storedOrigin.X + 1, sumY - storedOrigin.Y);
                case XYDirection.RightTop:
                    return new Point(sumX - storedOrigin.X, sumY - storedOrigin.Y);
                case XYDirection.RightBottom:
                    return new Point(sumX - storedOrigin.X, storedOrigin.Y);
                default:
                    return storedOrigin;
            }
        }

        /// <summary>
        /// Map 을 그린 뒤, 사용(1) Die 중 Wafer 안(반지름 - Edge, Notch / Flat 영역 밖)에 들어가지 않는 수를 확인 항목에 넣는다.
        /// 판정은 Map 컨트롤(StandardMapEditMap.IsInsideWafer)이 하므로 화면에서 센 값을 받는다. 0 이 아니면 작업자가 일부러 살렸을 수 있어 참고로 둔다.
        /// </summary>
        public void AddOutsideWaferCheck(int outsideUsedCount)
        {
            Checks.Add(new StoredMapCheck("Wafer 밖 사용 Die", "0 이어야 함", outsideUsedCount.ToString(CultureInfo.InvariantCulture),
                outsideUsedCount == 0 ? StoredMapCheckResult.Match : StoredMapCheckResult.Note,
                "Wafer 반지름 - Edge 밖이거나 Notch / Flat 영역에 걸린 사용 Die"));
        }

        /// <summary>
        /// Map 컨트롤이 저장값으로 Map 을 그리지 못했을 때(예외) 이유를 확인 항목에 남긴다. 화면은 Map 을 비우고 나머지 확인 결과는 그대로 보인다.
        /// </summary>
        public void AddDrawFailureCheck(string message)
        {
            Checks.Add(new StoredMapCheck("Map 그리기", "-", "실패", StoredMapCheckResult.Differ,
                "저장값으로 Map 을 그릴 수 없음: " + message));
        }

        /// <summary>
        /// Die 표를 읽어 LeftBottom 번호로 바꾸고 개수 / 범위 / 중복을 센다.
        /// LeftBottom 범위는 화면 Map 컨트롤의 DieMin / Max 로 쓴다 (File Map Setup Map 은 저장 범위와 같다).
        /// </summary>
        private void ReadDies(DataTable dies)
        {
            LeftBottomMinX = StoredGrid.MinX;
            LeftBottomMaxX = StoredGrid.MaxX;
            LeftBottomMinY = StoredGrid.MinY;
            LeftBottomMaxY = StoredGrid.MaxY;
            if (dies == null) return;
            HasStoredDieNum = dies.Columns.Contains("DIE_NUM");

            bool first = true;
            foreach (DataRow row in dies.Rows)
            {
                Point stored = new Point(Integer(row, "X"), Integer(row, "Y"));
                Point lb = KlarfMapRule.FromDirection(stored, Direction, StoredGrid);
                StoredMapDie die = new StoredMapDie(stored, lb, Integer(row, "USECODE"));
                if (HasStoredDieNum && !row.IsNull("DIE_NUM")) die.StoredDieNum = Integer(row, "DIE_NUM");
                Dies.Add(die);
                if (die.UseCode == 1) UsedCount++;
                else if (die.UseCode == -1) RemovedCount++;
                else OtherCount++;

                if (first)
                {
                    LeftBottomMinX = LeftBottomMaxX = lb.X;
                    LeftBottomMinY = LeftBottomMaxY = lb.Y;
                    first = false;
                }
                LeftBottomMinX = Math.Min(LeftBottomMinX, lb.X);
                LeftBottomMaxX = Math.Max(LeftBottomMaxX, lb.X);
                LeftBottomMinY = Math.Min(LeftBottomMinY, lb.Y);
                LeftBottomMaxY = Math.Max(LeftBottomMaxY, lb.Y);
            }
        }

        /// <summary>
        /// 데이터 확인 표 항목을 만든다 (설계 6절). File Map Setup 저장 규칙대로 들어간 Map 이면 모두 일치다.
        /// 다름 = 저장 규칙과 맞지 않아 확인이 필요한 값 / 참고 = 기존 Map Define 으로 만든 Map 이면 다를 수 있는 값.
        /// </summary>
        private void BuildChecks()
        {
            Checks.Add(new StoredMapCheck("Die 행 (TQP_USEMAP)", "1행 이상", Dies.Count.ToString(CultureInfo.InvariantCulture),
                Dies.Count > 0 ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "0행이면 Map 정의만 저장된 부분 실패일 수 있음"));

            Checks.Add(new StoredMapCheck("Net Die", StoredNetDie.ToString(CultureInfo.InvariantCulture), "USECODE 1 = " + UsedCount,
                StoredNetDie == UsedCount ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "TQP_MAPDEF.NETDIE 와 사용 Die 행 수"));

            int dieMinX = 0, dieMaxX = 0, dieMinY = 0, dieMaxY = 0;
            bool hasDie = Dies.Count > 0;
            if (hasDie)
            {
                dieMinX = dieMaxX = Dies[0].Stored.X;
                dieMinY = dieMaxY = Dies[0].Stored.Y;
                foreach (StoredMapDie die in Dies)
                {
                    dieMinX = Math.Min(dieMinX, die.Stored.X);
                    dieMaxX = Math.Max(dieMaxX, die.Stored.X);
                    dieMinY = Math.Min(dieMinY, die.Stored.Y);
                    dieMaxY = Math.Max(dieMaxY, die.Stored.Y);
                }
            }
            bool rangeMatch = hasDie && dieMinX == StoredGrid.MinX && dieMaxX == StoredGrid.MaxX && dieMinY == StoredGrid.MinY && dieMaxY == StoredGrid.MaxY;
            Checks.Add(new StoredMapCheck("Die 번호 범위", RangeText(StoredGrid.MinX, StoredGrid.MaxX, StoredGrid.MinY, StoredGrid.MaxY),
                hasDie ? RangeText(dieMinX, dieMaxX, dieMinY, dieMaxY) : "-",
                rangeMatch ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "DIE_INDEX_MIN / MAX 와 Die 행 번호 최소 / 최대 (저장 방향 번호)"));

            List<string> duplicates = FindDuplicates();
            Checks.Add(new StoredMapCheck("중복 Die", "없어야 함", duplicates.Count == 0 ? "0" : duplicates.Count + " (" + Join(duplicates, 5) + ")",
                duplicates.Count == 0 ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "같은 X, Y 행이 두 번 이상"));

            long width = (long)StoredGrid.MaxX - StoredGrid.MinX + 1;
            long height = (long)StoredGrid.MaxY - StoredGrid.MinY + 1;
            long gridCount = width > 0 && height > 0 ? width * height : 0;
            Checks.Add(new StoredMapCheck("격자 채움", string.Format(CultureInfo.InvariantCulture, "{0} x {1} = {2}", width, height, gridCount),
                Dies.Count.ToString(CultureInfo.InvariantCulture),
                gridCount == Dies.Count ? StoredMapCheckResult.Match : StoredMapCheckResult.Note,
                "File Map Setup 은 격자 전체를 저장한다 (Map Define 으로 만든 Map 은 일부만 있을 수 있음)"));

            Checks.Add(new StoredMapCheck("USECODE 구성", "1 / -1", UseCodeText(),
                OtherCount == 0 ? StoredMapCheckResult.Match : StoredMapCheckResult.Note,
                "File Map Setup 은 사용 1 / 제거 -1 만 저장한다"));

            Checks.Add(new StoredMapCheck("Edge 단위", EdgeSizeMm.ToString("0.###", CultureInfo.InvariantCulture), "0 ~ 5",
                IsEdgeValid ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "범위 밖이면 Map Define 에서 불러올 때 오류. 이 화면은 Edge 0 으로 그림"));

            Checks.Add(new StoredMapCheck("Wafer 크기 단위", WaferSize.ToString("#,0.###", CultureInfo.InvariantCulture), "1,000 이상",
                WaferSize >= MinWaferMicron ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "단위가 다르게 저장되면 Map 이 아주 작게 그려짐"));

            Checks.Add(new StoredMapCheck("Die 크기 단위",
                string.Format(CultureInfo.InvariantCulture, "{0:#,0.###} x {1:#,0.###}", DieSizeX, DieSizeY), "10 이상",
                DieSizeX >= MinDieMicron && DieSizeY >= MinDieMicron ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "단위가 다르게 저장되면 Die 가 점처럼 작게 그려져 마우스로 잡을 수 없음"));

            Checks.Add(new StoredMapCheck("XY 방향", StoredXYDirection.ToString(CultureInfo.InvariantCulture),
                IsDirectionValid ? Direction.ToString() : "알 수 없는 값",
                IsDirectionValid ? StoredMapCheckResult.Match : StoredMapCheckResult.Differ,
                "0 LeftTop / 1 LeftBottom / 2 RightBottom / 3 RightTop. 알 수 없는 값이면 Map 을 그리지 않음"));

            if (HasStoredDieNum)
            {
                // 저장 DIE_NUM 이 Direction 번호(DieNumberRule)와 같은지 사용 Die 기준으로 센다.
                // 현재 저장 코드(DSL TQP_USEMAP.CreateUseMap)는 행 순서(i + 1)로 넣으므로 보통 다르다. 오류가 아니라 참고로 둔다.
                int same = 0;
                foreach (StoredMapDie die in Dies)
                    if (die.UseCode == 1 && die.DirectionNumber.HasValue && die.StoredDieNum == die.DirectionNumber) same++;
                Checks.Add(new StoredMapCheck("DIE_NUM 순서", "저장 DIE_NUM",
                    string.Format(CultureInfo.InvariantCulture, "Direction 번호와 같음 {0} / {1}", same, UsedCount),
                    UsedCount > 0 && same == UsedCount ? StoredMapCheckResult.Match : StoredMapCheckResult.Note,
                    "Direction 번호 = 사용 Die 만, 시작 모서리부터 행 우선 (화면 계산). 저장 DIE_NUM 은 저장 순서"));
            }
        }

        /// <summary>
        /// 사용 Die 에 Direction 번호(DieNumberRule: 행 우선 / 사용 Die 만)를 매긴다.
        /// 저장 X / Y 가 곧 방향 번호(시작 모서리에서 1 부터 커짐)이므로 그대로 넘긴다. 같은 번호의 중복 행은 같은 번호를 받는다.
        /// </summary>
        private void NumberDies()
        {
            List<KeyValuePair<Point, int>> list = new List<KeyValuePair<Point, int>>(Dies.Count);
            foreach (StoredMapDie die in Dies) list.Add(new KeyValuePair<Point, int>(die.Stored, die.UseCode));
            Dictionary<Point, int> numbers = DieNumberRule.Number(list);
            foreach (StoredMapDie die in Dies)
            {
                int number;
                if (die.UseCode == 1 && numbers.TryGetValue(die.Stored, out number)) die.DirectionNumber = number;
            }
        }

        /// <summary>같은 저장 번호가 두 번 이상 나온 Die 를 "X,Y" 로 돌려준다.</summary>
        private List<string> FindDuplicates()
        {
            HashSet<Point> seen = new HashSet<Point>();
            HashSet<Point> reported = new HashSet<Point>();
            List<string> duplicates = new List<string>();
            foreach (StoredMapDie die in Dies)
            {
                if (seen.Add(die.Stored)) continue;
                if (reported.Add(die.Stored)) duplicates.Add(die.Stored.X + "," + die.Stored.Y);
            }
            return duplicates;
        }

        private string UseCodeText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "1: {0} / -1: {1} / 기타: {2}", UsedCount, RemovedCount, OtherCount);
            if (OtherCount > 0)
            {
                SortedDictionary<int, int> others = new SortedDictionary<int, int>();
                foreach (StoredMapDie die in Dies)
                {
                    if (die.UseCode == 1 || die.UseCode == -1) continue;
                    int count;
                    others.TryGetValue(die.UseCode, out count);
                    others[die.UseCode] = count + 1;
                }
                List<string> parts = new List<string>();
                foreach (KeyValuePair<int, int> pair in others) parts.Add(pair.Key + "=" + pair.Value);
                sb.Append(" (" + Join(parts, 5) + ")");
            }
            return sb.ToString();
        }

        private static string RangeText(int minX, int maxX, int minY, int maxY)
        {
            return string.Format(CultureInfo.InvariantCulture, "X {0}~{1} / Y {2}~{3}", minX, maxX, minY, maxY);
        }

        /// <summary>앞의 max 개만 보이고 나머지는 "외 n" 으로 줄인다 (표 칸이 넘치지 않게).</summary>
        private static string Join(List<string> values, int max)
        {
            if (values.Count <= max) return string.Join(" ", values.ToArray());
            return string.Join(" ", values.GetRange(0, max).ToArray()) + " 외 " + (values.Count - max);
        }

        /// <summary>Oracle NUMBER 는 decimal 로 온다. 없거나 비어 있으면 0 으로 본다.</summary>
        private static double Number(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row.IsNull(column)) return 0d;
            return System.Convert.ToDouble(row[column], CultureInfo.InvariantCulture);
        }

        private static int Integer(DataRow row, string column)
        {
            return (int)Math.Round(Number(row, column));
        }

        private static string Text(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row.IsNull(column)) return string.Empty;
            return System.Convert.ToString(row[column], CultureInfo.InvariantCulture).Trim();
        }
    }

    /// <summary>TQP_USEMAP 한 행. Stored = 저장 번호 (저장 XY 방향), LeftBottom = 화면 Map 컨트롤 번호.</summary>
    internal sealed class StoredMapDie
    {
        public StoredMapDie(Point stored, Point leftBottom, int useCode)
        {
            Stored = stored;
            LeftBottom = leftBottom;
            UseCode = useCode;
        }

        public Point Stored { get; private set; }
        public Point LeftBottom { get; private set; }
        public int UseCode { get; private set; }
        /// <summary>Direction 번호 (사용 Die 만, 아니면 null). 화면이 Die 위에 그리는 번호다.</summary>
        public int? DirectionNumber { get; set; }
        /// <summary>저장 TQP_USEMAP.DIE_NUM (없으면 null).</summary>
        public int? StoredDieNum { get; set; }
        /// <summary>Wafer 안 여부. Map 을 그린 뒤 화면이 채운다 (Map 컨트롤 판정).</summary>
        public bool? InsideWafer { get; set; }
    }

    /// <summary>데이터 확인 결과. 일치 = 저장 규칙과 맞음 / 다름 = 확인 필요 / 참고 = 기존 방식 Map 이면 다를 수 있음.</summary>
    internal enum StoredMapCheckResult
    {
        Match,
        Differ,
        Note
    }

    /// <summary>데이터 확인 표 한 줄 (항목 / 저장값 / Die 행 또는 기준 / 결과 / 설명).</summary>
    internal sealed class StoredMapCheck
    {
        public StoredMapCheck(string name, string storedValue, string dieValue, StoredMapCheckResult result, string description)
        {
            Name = name;
            StoredValue = storedValue;
            DieValue = dieValue;
            Result = result;
            Description = description;
        }

        public string Name { get; private set; }
        public string StoredValue { get; private set; }
        public string DieValue { get; private set; }
        public StoredMapCheckResult Result { get; private set; }
        public string Description { get; private set; }

        /// <summary>표에 보일 결과 글자.</summary>
        public string ResultText
        {
            get
            {
                switch (Result)
                {
                    case StoredMapCheckResult.Match: return "일치";
                    case StoredMapCheckResult.Differ: return "다름";
                    default: return "참고";
                }
            }
        }
    }
}
