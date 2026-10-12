using System;
using System.Collections.Generic;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// GDS Front / Back side 좌표 변환을 한 곳에 모은다. (설계: 문서/2026-10-07_GDS_FrontBack_전환_설계.md 4절 /)
    /// GDS 원본은 Front side. Back side = GDS 원점 기준 좌우 반전: X' = -X, Y' = Y.
    /// DB(PLACED / Chain Marker)는 저장 기준(TQP_GDS_MAP.VIEW_SIDE) 좌표 그대로 두고, 보기 / 분석 기준이 다를 때만 이 변환을 쓴다.
    /// X 부호만 바꾸므로(GPoint 생성자의 소수 5자리 반올림을 다시 거치지 않음) 두 번 바꾸면 비트 단위로 원본과 같다.
    /// 원본 배열은 바꾸지 않고 항상 새 배열 / 새 객체를 돌려준다.
    /// </summary>
    public static class GdsSideTransform
    {
        public const string Front = "FRONT";
        public const string Back = "BACK";

        /// <summary>저장 기준과 보기 기준이 다르면 좌우 반전이 필요하다. 빈 값은 FRONT로 본다.</summary>
        public static bool NeedsMirror(string storedSide, string viewSide)
        {
            return !String.Equals(Normalize(storedSide), Normalize(viewSide), StringComparison.Ordinal);
        }

        /// <summary>BACK 외의 값(빈 값 포함)은 FRONT.</summary>
        public static string Normalize(string side)
        {
            return String.Equals(side == null ? null : side.Trim(), Back, StringComparison.OrdinalIgnoreCase) ? Back : Front;
        }

        /// <summary>화면 문구.</summary>
        public static string Text(string side)
        {
            return Normalize(side) == Back ? "Back side(좌우 반전)" : "Front side(원본)";
        }

        public static double MirrorX(double x)
        {
            return -x;
        }

        public static GPoint MirrorPoint(GPoint point)
        {
            point.X = -point.X;
            return point;
        }

        /// <summary>좌우 반전 사본(새 배열).</summary>
        public static GPoint[] MirrorPoints(GPoint[] points)
        {
            var result = new GPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
                result[i] = MirrorPoint(points[i]);
            return result;
        }

        /// <summary>좌우 반전한 영역: X 범위가 (-MaxX ~ -MinX)로 바뀐다.</summary>
        public static GBox MirrorBounds(GBox bounds)
        {
            return new GBox(-bounds.MaxX, bounds.MinY, -bounds.MinX, bounds.MaxY);
        }

        /// <summary>배치 도형 한 건의 좌우 반전 사본(좌표 / Bounds). TEXT는 위치(WorldPoints[0])만 반전하고 정렬 / 글자는 그대로.</summary>
        public static MapPlacedElementData MirrorData(MapPlacedElementData data)
        {
            var copy = data.CloneForBatch();
            copy.WorldPoints = MirrorPoints(data.WorldPoints);
            copy.Bounds = MirrorBounds(data.Bounds);
            return copy;
        }

        /// <summary>배치 도형 목록 전체의 좌우 반전 사본. 샘플 60만 도형 기준 반전 계산 약 0.2초(2026-10-07 측정).</summary>
        public static List<MapPlacedElementData> MirrorAll(IList<MapPlacedElementData> items)
        {
            var result = new List<MapPlacedElementData>(items.Count);
            foreach (MapPlacedElementData item in items)
                result.Add(MirrorData(item));
            return result;
        }
    }
}
