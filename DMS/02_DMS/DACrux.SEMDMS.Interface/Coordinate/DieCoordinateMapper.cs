using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace DACrux.SEMDMS.Interface
{
    // =====================================================================================================
    //  Die 좌표 변환 (2026-10-13)
    // -----------------------------------------------------------------------------------------------------
    //  목적
    //   E-Test 결과 파일 / KLARF(Back-side, Front-side) / 고객 통합 Die# 처럼 데이터마다 다른 Die 좌표를
    //   시스템 Die 좌표 하나로 바꿔 같은 Die 끼리 연결한다. Setup 화면 / 테이블 없이 Map 형상만으로 계산한다.
    //
    //  시스템 Die 좌표 (기준)
    //   File Map Setup 으로 만든 Map 의 TQP_USEMAP X / Y.
    //   Back-side top view, Notch 아래. 왼쪽 아래 Die 가 (MinX, MinY), X 는 오른쪽으로, Y 는 위로 커진다.
    //
    //  좌표 종류별 규칙 (MinX / MaxX / MinY / MaxY = 사용 Die 범위, (OX, OY) = KLARF 원점 Die 의 시스템 좌표)
    //   ETest      : E-Test (X, Y) (TSVET / COWET). X = 위에서 몇 번째 행, Y = 오른쪽에서 몇 번째 열 (0 부터).
    //                시스템 X = MaxX - y, 시스템 Y = MaxY - x
    //   KlarfBack  : KLARF XINDEX / YINDEX, Back-side top view 검사 (SiN / RDL / C4 Bump / G2DB AOI).
    //                시스템 X = x + OX, 시스템 Y = y + OY
    //   KlarfFront : KLARF XINDEX / YINDEX, Front-side top view 검사 (TBDB / uPAD AOI). Back-side 를 좌우로 뒤집은 좌표.
    //                시스템 X = (MinX + MaxX) - (x + OX), 시스템 Y = y + OY
    //   DieNumber  : 고객 통합 Die# (E-Test FT 파일의 Part ID). Back-side top view 왼쪽 위부터 행 우선으로 1, 2, 3 ...
    //                사용 Die 를 위 행부터(Y 큰 것부터), 같은 행은 왼쪽부터(X 작은 것부터) 센다. X = Die#, Y 는 쓰지 않는다.
    //
    //  U9 확인 값 (고객 대응표 76행과 모두 일치)
    //   Map OMM_Hybrid: 사용 Die 76개, X 1~12, Y 1~9 / KLARF 원점 (OX, OY) = (7, 5) = Die#39
    //   예) Die#39 = 시스템 (7, 5) = E-Test (4, 5) = KLARF Back (0, 0) = KLARF Front (-1, 0)
    //       Die#71 = 시스템 (9, 2) = E-Test (7, 3)
    //
    //  쓰는 쪽이 정할 것
    //   - 어떤 Map 을 쓸지 (Device -> Map ID).
    //   - KLARF 원점 (OX, OY): KLARF Index (0, 0) 인 Die 의 시스템 좌표. Map / TQD_SETUP 원점 값을 넘긴다.
    //   - KLARF 파일이 Back-side 인지 Front-side 인지 (파일의 OrientationInstructions 값은 고객 기준과 다를 수 있다).
    //
    //  걷어내기 / 재활용
    //   이 파일 하나에 모든 규칙이 있다. 다른 소스에 의존하지 않는다 (System / System.Data 만 사용).
    //   걷어낼 때는 이 파일과 csproj 의 Compile 1행을 지우고 호출하는 곳만 정리한다.
    //   다른 Device 의 E-Test 장비가 좌표를 다르게 매기면 ETest 규칙만 바꾸거나 DieCoordType 에 종류를 더한다.
    //
    //  사용 예
    //   DieCoordinateMapper map = DieCoordinateMapper.FromDieTable(dtUseMap, new DiePoint(7, 5));
    //   DiePoint sys;
    //   if (map.TryToSystem(DieCoordType.ETest, new DiePoint(7, 3), out sys)) { int dieNo = map.DieNumberOf(sys); }   // 71
    //   DiePoint front = map.Convert(DieCoordType.KlarfBack, new DiePoint(0, 0), DieCoordType.KlarfFront);         // (-1, 0)
    // =====================================================================================================

    /// <summary>Die 좌표 종류. 규칙은 DieCoordinateMapper 머리 주석 참고.</summary>
    public enum DieCoordType
    {
        /// <summary>시스템 Die 좌표 (File Map Setup Map 의 TQP_USEMAP X / Y)</summary>
        System = 0,
        /// <summary>E-Test 결과 파일 (X, Y) — TSVET / COWET</summary>
        ETest = 1,
        /// <summary>KLARF XINDEX / YINDEX — Back-side top view 검사</summary>
        KlarfBack = 2,
        /// <summary>KLARF XINDEX / YINDEX — Front-side top view 검사</summary>
        KlarfFront = 3,
        /// <summary>고객 통합 Die# (E-Test FT 의 Part ID). DiePoint.X = Die#, Y 는 0</summary>
        DieNumber = 4
    }

    /// <summary>Die 좌표 1개 (정수 Index).</summary>
    [Serializable]
    public struct DiePoint : IEquatable<DiePoint>
    {
        public DiePoint(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public bool Equals(DiePoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is DiePoint && Equals((DiePoint)obj);
        }

        public override int GetHashCode()
        {
            return unchecked(X * 397 ^ Y);
        }

        public override string ToString()
        {
            return "(" + X.ToString(CultureInfo.InvariantCulture) + ", " + Y.ToString(CultureInfo.InvariantCulture) + ")";
        }
    }

    /// <summary>
    /// Map 1개 기준의 Die 좌표 변환기. 만들 때 사용 Die 목록과 KLARF 원점만 받고, 이후 변환은 계산만 한다(DB 조회 없음).
    /// 서버(BSL)와 화면(ENGUI)이 같이 쓰도록 Interface 프로젝트에 둔다.
    /// </summary>
    [Serializable]
    public sealed class DieCoordinateMapper
    {
        /// <summary>TQP_USEMAP 의 사용 Die USECODE</summary>
        public const int UsedDieCode = 1;

        private readonly List<DiePoint> numberedDies;
        private readonly Dictionary<DiePoint, int> dieNumbers;

        /// <param name="usedDies">사용 Die 의 시스템 좌표 (TQP_USEMAP USECODE = 1 인 X / Y). 같은 좌표는 한 번만 센다.</param>
        /// <param name="klarfOrigin">KLARF Index (0, 0) 인 Die 의 시스템 좌표 (U9 = (7, 5))</param>
        public DieCoordinateMapper(IEnumerable<DiePoint> usedDies, DiePoint klarfOrigin)
        {
            if (usedDies == null)
                throw new ArgumentNullException("usedDies");

            HashSet<DiePoint> seen = new HashSet<DiePoint>();
            numberedDies = new List<DiePoint>();
            foreach (DiePoint die in usedDies)
                if (seen.Add(die))
                    numberedDies.Add(die);
            if (numberedDies.Count == 0)
                throw new ArgumentException("사용 Die 가 없습니다.", "usedDies");

            // Die# 순서: 위 행부터(Y 큰 것부터), 같은 행은 왼쪽부터(X 작은 것부터) = Back-side top view 왼쪽 위부터 행 우선
            numberedDies.Sort((a, b) => a.Y != b.Y ? b.Y.CompareTo(a.Y) : a.X.CompareTo(b.X));
            dieNumbers = new Dictionary<DiePoint, int>(numberedDies.Count);
            for (int i = 0; i < numberedDies.Count; i++)
                dieNumbers[numberedDies[i]] = i + 1;

            MinX = int.MaxValue;
            MaxX = int.MinValue;
            MinY = int.MaxValue;
            MaxY = int.MinValue;
            foreach (DiePoint die in numberedDies)
            {
                MinX = Math.Min(MinX, die.X);
                MaxX = Math.Max(MaxX, die.X);
                MinY = Math.Min(MinY, die.Y);
                MaxY = Math.Max(MaxY, die.Y);
            }
            KlarfOrigin = klarfOrigin;
        }

        /// <summary>
        /// TQP_USEMAP 조회 결과(열 X / Y / USECODE)로 만든다. USECODE 가 1 인 행만 사용 Die 로 본다.
        /// USECODE 열이 없으면 모든 행을 사용 Die 로 본다.
        /// </summary>
        public static DieCoordinateMapper FromDieTable(DataTable dies, DiePoint klarfOrigin)
        {
            if (dies == null)
                throw new ArgumentNullException("dies");
            bool hasUseCode = dies.Columns.Contains("USECODE");
            List<DiePoint> used = new List<DiePoint>();
            foreach (DataRow row in dies.Rows)
            {
                if (hasUseCode && System.Convert.ToInt32(row["USECODE"], CultureInfo.InvariantCulture) != UsedDieCode)
                    continue;
                used.Add(new DiePoint(System.Convert.ToInt32(row["X"], CultureInfo.InvariantCulture), System.Convert.ToInt32(row["Y"], CultureInfo.InvariantCulture)));
            }
            return new DieCoordinateMapper(used, klarfOrigin);
        }

        /// <summary>사용 Die 범위 (시스템 좌표)</summary>
        public int MinX { get; private set; }

        public int MaxX { get; private set; }

        public int MinY { get; private set; }

        public int MaxY { get; private set; }

        /// <summary>KLARF Index (0, 0) 인 Die 의 시스템 좌표</summary>
        public DiePoint KlarfOrigin { get; private set; }

        /// <summary>사용 Die 수 (= 마지막 Die#)</summary>
        public int DieCount
        {
            get { return numberedDies.Count; }
        }

        /// <summary>시스템 좌표가 사용 Die 인지</summary>
        public bool Contains(DiePoint system)
        {
            return dieNumbers.ContainsKey(system);
        }

        /// <summary>시스템 좌표의 Die#. 사용 Die 가 아니면 0.</summary>
        public int DieNumberOf(DiePoint system)
        {
            int number;
            return dieNumbers.TryGetValue(system, out number) ? number : 0;
        }

        /// <summary>
        /// 다른 종류 좌표 -> 시스템 좌표. 계산만 하므로 결과가 사용 Die 가 아닐 수 있다(확인은 Contains / TryToSystem).
        /// DieNumber 는 1 ~ DieCount 가 아니면 ArgumentOutOfRangeException.
        /// </summary>
        public DiePoint ToSystem(DieCoordType type, DiePoint value)
        {
            switch (type)
            {
                case DieCoordType.System:
                    return value;
                case DieCoordType.ETest:
                    return new DiePoint(MaxX - value.Y, MaxY - value.X);
                case DieCoordType.KlarfBack:
                    return new DiePoint(value.X + KlarfOrigin.X, value.Y + KlarfOrigin.Y);
                case DieCoordType.KlarfFront:
                    return new DiePoint(MinX + MaxX - (value.X + KlarfOrigin.X), value.Y + KlarfOrigin.Y);
                case DieCoordType.DieNumber:
                    if (value.X < 1 || value.X > numberedDies.Count)
                        throw new ArgumentOutOfRangeException("value", "Die# 는 1 ~ " + numberedDies.Count + " 이어야 합니다: " + value.X);
                    return numberedDies[value.X - 1];
                default:
                    throw new ArgumentOutOfRangeException("type", "알 수 없는 좌표 종류: " + type);
            }
        }

        /// <summary>다른 종류 좌표 -> 시스템 좌표. 결과가 사용 Die 일 때만 true (Die# 범위 밖도 false).</summary>
        public bool TryToSystem(DieCoordType type, DiePoint value, out DiePoint system)
        {
            system = default(DiePoint);
            if (type == DieCoordType.DieNumber && (value.X < 1 || value.X > numberedDies.Count))
                return false;
            system = ToSystem(type, value);
            return Contains(system);
        }

        /// <summary>
        /// 시스템 좌표 -> 다른 종류 좌표 (ToSystem 의 역변환). DieNumber 는 사용 Die 가 아니면 ArgumentOutOfRangeException.
        /// </summary>
        public DiePoint FromSystem(DieCoordType type, DiePoint system)
        {
            switch (type)
            {
                case DieCoordType.System:
                    return system;
                case DieCoordType.ETest:
                    return new DiePoint(MaxY - system.Y, MaxX - system.X);
                case DieCoordType.KlarfBack:
                    return new DiePoint(system.X - KlarfOrigin.X, system.Y - KlarfOrigin.Y);
                case DieCoordType.KlarfFront:
                    return new DiePoint(MinX + MaxX - system.X - KlarfOrigin.X, system.Y - KlarfOrigin.Y);
                case DieCoordType.DieNumber:
                    int number = DieNumberOf(system);
                    if (number == 0)
                        throw new ArgumentOutOfRangeException("system", "사용 Die 가 아닙니다: " + system);
                    return new DiePoint(number, 0);
                default:
                    throw new ArgumentOutOfRangeException("type", "알 수 없는 좌표 종류: " + type);
            }
        }

        /// <summary>한 종류 좌표 -> 다른 종류 좌표 (시스템 좌표를 거친다).</summary>
        public DiePoint Convert(DieCoordType from, DiePoint value, DieCoordType to)
        {
            return FromSystem(to, ToSystem(from, value));
        }

        /// <summary>좌표 종류의 변환식 설명 (화면 / 로그 표시용).</summary>
        public string Describe(DieCoordType type)
        {
            switch (type)
            {
                case DieCoordType.System:
                    return "시스템 좌표 (Map X / Y)";
                case DieCoordType.ETest:
                    return string.Format(CultureInfo.InvariantCulture, "E-Test (x, y) -> 시스템 ({0} - y, {1} - x)", MaxX, MaxY);
                case DieCoordType.KlarfBack:
                    return string.Format(CultureInfo.InvariantCulture, "KLARF Back (x, y) -> 시스템 (x + {0}, y + {1})", KlarfOrigin.X, KlarfOrigin.Y);
                case DieCoordType.KlarfFront:
                    return string.Format(CultureInfo.InvariantCulture, "KLARF Front (x, y) -> 시스템 ({0} - x, y + {1})", MinX + MaxX - KlarfOrigin.X, KlarfOrigin.Y);
                case DieCoordType.DieNumber:
                    return "Die# -> 사용 Die 를 위 행부터, 같은 행은 왼쪽부터 센 번호 (1 ~ " + numberedDies.Count + ")";
                default:
                    return type.ToString();
            }
        }
    }
}
