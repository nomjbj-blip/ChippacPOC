using System;
using System.Drawing;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// KLARF 로 만드는 Map 의 Die 격자 범위와 원점 인덱스.
    /// 인덱스는 모두 LeftBottom 기준(좌하단 첫 Die = 1,1)이며 DMS 가 KLARF Defect 를 저장할 때 쓰는 인덱스와 같다.
    /// </summary>
    internal sealed class KlarfMapGrid
    {
        public KlarfMapGrid(int minX, int maxX, int minY, int maxY, int originIndexX, int originIndexY, int calculatorDieCount)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            OriginIndexX = originIndexX;
            OriginIndexY = originIndexY;
            CalculatorDieCount = calculatorDieCount;
        }

        public int MinX { get; private set; }
        public int MaxX { get; private set; }
        public int MinY { get; private set; }
        public int MaxY { get; private set; }

        /// <summary>KLARF 원본 (0,0) Die 의 Map 인덱스. 원본 인덱스 + 이 값 = Map 인덱스.</summary>
        public int OriginIndexX { get; private set; }
        public int OriginIndexY { get; private set; }

        /// <summary>WaferDieCalculator 가 Wafer 안에 들어간다고 본 Die 수. 정보 표시용이다.</summary>
        public int CalculatorDieCount { get; private set; }

        /// <summary>격자 범위 안의 인덱스인지 확인한다.</summary>
        public bool Contains(Point index)
        {
            return index.X >= MinX && index.X <= MaxX && index.Y >= MinY && index.Y <= MaxY;
        }
    }

    /// <summary>
    /// KLARF 기준 Map Setup 의 계산 Rule 만 모은 클래스다. 화면 코드와 분리해 Probe 로 따로 검증한다.
    /// 왜: GDS / KLARF / ET Map 을 같은 Die 격자로 맞추려면 DMS 가 KLARF 를 저장할 때 쓰는 인덱스 규칙을 그대로 따라야 한다.
    /// 근거: DACrux.Data.Parser Klarf/WaferList.UpdateDieIndex 는 원본 인덱스(가운데 Die = 0,0)에
    ///       TQD_SETUP.DIE_ORIGIN 또는 WaferDieCalculator.OriginIndex 를 더해 좌하단 (1,1) 기준으로 옮긴다.
    /// 설계: 문서/2026-10-06_KLARF_Map_Setup_설계.md 2~3절
    /// </summary>
    internal static class KlarfMapRule
    {
        /// <summary>
        /// Wafer 전체 Die 격자 범위를 구한다.
        /// 처리 흐름:
        /// 1. Parser 와 같은 입력(Wafer 크기 / Pitch / SampleCenterLocation, 여백 0)으로 WaferDieCalculator 를 실행한다.
        /// 2. 계산기 격자는 계산기 원점 기준이다. Parser 가 실제로 옮긴 양(shift)이 DB 값이라 계산기 원점과 다르면
        ///    그 차이만큼 격자를 옮겨 Parser 인덱스와 물리 위치가 맞게 한다. (DB 값이 없으면 차이는 0)
        /// Edge 여백은 격자에 넣지 않는다. 격자가 바뀌면 DMS 인덱스와 어긋나기 때문이며, Edge 는 원형 판정에서만 쓴다.
        /// </summary>
        /// <param name="waferSize">Wafer 지름 (µm)</param>
        /// <param name="pitchX">Die Pitch X (µm)</param>
        /// <param name="pitchY">Die Pitch Y (µm)</param>
        /// <param name="centerX">SampleCenterLocation X (µm)</param>
        /// <param name="centerY">SampleCenterLocation Y (µm)</param>
        /// <param name="shiftX">Parser 가 원본 인덱스에 더한 X 값 (Wafer.DieOriginX)</param>
        /// <param name="shiftY">Parser 가 원본 인덱스에 더한 Y 값 (Wafer.DieOriginY)</param>
        public static KlarfMapGrid CalculateGrid(double waferSize, double pitchX, double pitchY,
            double centerX, double centerY, int shiftX, int shiftY)
        {
            if (waferSize <= 0 || pitchX <= 0 || pitchY <= 0)
                throw new ArgumentException("Wafer 크기와 Die Pitch 는 0 보다 커야 합니다.");

            WaferDieCalculator calc = new WaferDieCalculator();
            calc.WaferSize = waferSize;
            calc.DiePitchX = pitchX;
            calc.DiePitchY = pitchY;
            calc.OriginX = centerX;
            calc.OriginY = centerY;
            calc.Calculate();

            // Die 가 Wafer 반지름보다 크면 계산기는 격자 없이 원점 0 만 돌려준다.
            if (calc.RowArrayList.Count == 0 || calc.IndexXMax < 1)
                throw new InvalidOperationException("Wafer 안에 들어가는 Die 가 없습니다. SampleSize / DiePitch 를 확인하세요.");

            int moveX = shiftX - calc.OriginIndexX;
            int moveY = shiftY - calc.OriginIndexY;
            return new KlarfMapGrid(calc.IndexXMin + moveX, calc.IndexXMax + moveX,
                calc.IndexYMin + moveY, calc.IndexYMax + moveY,
                shiftX, shiftY, calc.TotalDieCount);
        }

        /// <summary>
        /// LeftBottom 인덱스를 선택한 XY 방향의 번호로 바꾼다. 형상(Die 의 물리 위치)은 그대로 두고 번호만 바꾼다.
        /// Right 계열은 X 를, Top 계열은 Y 를 격자 범위 안에서 뒤집는다. 같은 식을 한 번 더 적용하면 원래 값이 된다.
        /// </summary>
        public static Point ToDirection(Point leftBottomIndex, XYDirection direction, KlarfMapGrid grid)
        {
            int x = leftBottomIndex.X;
            int y = leftBottomIndex.Y;
            if (direction == XYDirection.RightBottom || direction == XYDirection.RightTop)
                x = grid.MinX + grid.MaxX - x;
            if (direction == XYDirection.LeftTop || direction == XYDirection.RightTop)
                y = grid.MinY + grid.MaxY - y;
            return new Point(x, y);
        }

        /// <summary>Flat Zone 위치 콤보 항목 (화면 표시 순서).</summary>
        public static readonly string[] FlatZoneNames = { "Bottom", "Top", "Left", "Right" };

        /// <summary>
        /// Flat Zone 위치 -> Map 컨트롤 NotchAngle (2026-10-13).
        /// 기존 WaferMap.UseDie / Notch 영역 계산 기준: 0 = 아래, 90 = 왼쪽, 180 = 위, 270 = 오른쪽.
        /// KLARF OrientationMarkLocation (DOWN 0 / LEFT 90 / UP 180 / RIGHT 270) 과 같고, TQP_MAPDEF.ANGLE 에 그대로 저장된다.
        /// </summary>
        public static int FlatZoneToAngle(string flatZone)
        {
            switch (flatZone)
            {
                case "Left": return 90;
                case "Top": return 180;
                case "Right": return 270;
                default: return 0;
            }
        }

        /// <summary>NotchAngle -> Flat Zone 위치. 0 / 90 / 180 / 270 이 아니면 Bottom.</summary>
        public static string AngleToFlatZone(int angle)
        {
            switch (((angle % 360) + 360) % 360)
            {
                case 90: return "Left";
                case 180: return "Top";
                case 270: return "Right";
                default: return "Bottom";
            }
        }

        /// <summary>선택한 XY 방향의 번호를 LeftBottom 인덱스로 되돌린다. (ToDirection 과 같은 계산)</summary>
        public static Point FromDirection(Point directionIndex, XYDirection direction, KlarfMapGrid grid)
        {
            return ToDirection(directionIndex, direction, grid);
        }

        /// <summary>
        /// Map 컨트롤에 넣을 OriginIndexX / Y 를 XY 방향별로 구한다.
        /// 왜: 기존 WaferMap.AddDie 는 방향마다 원점 Die 를 놓는 모서리가 다르다 (06_CoreLibSource 수정 금지라 그대로 맞춘다).
        ///   AddDie 가 계산하는 Die 좌하단 (원점 기준, s = 원점 인덱스, i = Die 번호, p = Pitch)
        ///     LeftBottom  : X = (i - s) p     / Y = (i - s) p
        ///     LeftTop     : X = (i - s - 1) p / Y = (s - i) p
        ///     RightTop    : X = (s - i) p     / Y = (s - i) p
        ///     RightBottom : X = (s - i) p     / Y = (i - s) p
        ///   목표는 KLARF 원본 (0,0) Die 의 좌하단이 원점이 되는 것: X = (LB번호 - ShiftX) p, Y = (LB번호 - ShiftY) p.
        ///   번호는 ToDirection 으로 뒤집으므로 위 식을 풀면 아래 값이 된다.
        /// 실제 컨트롤 결과는 Verification/KlarfMapFormSmoke 에서 방향 4종 모두 Die 좌표로 확인한다.
        /// </summary>
        public static Point MapOriginIndex(XYDirection direction, KlarfMapGrid grid)
        {
            int sx = grid.OriginIndexX;
            int sy = grid.OriginIndexY;
            int sumX = grid.MinX + grid.MaxX;
            int sumY = grid.MinY + grid.MaxY;
            switch (direction)
            {
                case XYDirection.LeftTop:
                    return new Point(sx - 1, sumY - sy);
                case XYDirection.RightTop:
                    return new Point(sumX - sx, sumY - sy);
                case XYDirection.RightBottom:
                    return new Point(sumX - sx, sy);
                default:
                    return new Point(sx, sy);
            }
        }
    }
}
