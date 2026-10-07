using System;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// Chain 후보 연결에서 BOUNDARY의 실제 다각형과 폭이 있는 PATH가 닿거나 겹치는지 검사한다.
    /// Bounding Box는 이 검사 전에 주변 후보를 줄이는 용도로만 사용한다.
    /// </summary>
    public static class ChainGeometryOverlap
    {
        private const double Epsilon = 1e-9;

        /// <summary>
        /// 지도 클릭점이 BOUNDARY 내부 또는 PATH 폭 안에 있는지 검사한다.
        /// 화면 픽셀 오차를 월드 좌표로 변환한 tolerance만 경계에 추가한다.
        /// </summary>
        public static bool ContainsPoint(GPoint point, GPoint[] shape, bool isPath,
            double pathWidth, double tolerance)
        {
            if (shape == null || shape.Length == 0)
                return false;

            double limit = Math.Max(0, tolerance) + (isPath ? Math.Abs(pathWidth) / 2 : 0);
            if (!isPath && shape.Length >= 3 && PointInPolygon(point, shape))
                return true;

            int segmentCount = isPath ? Math.Max(1, shape.Length - 1) : shape.Length;
            for (int i = 0; i < segmentCount; i++)
            {
                GPoint end = shape[isPath ? Math.Min(i + 1, shape.Length - 1)
                    : (i + 1) % shape.Length];
                if (PointSegmentDistance(point, shape[i], end) <= limit + Epsilon)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 두 Element의 실제 점유 영역을 검사한다. 이전 Bounding Box 전용 Probe 데이터처럼
        /// 좌표 배열이 없는 경우에만 Bounds 겹침으로 처리한다.
        /// </summary>
        public static bool Intersects(ChainTraceElement first, ChainTraceElement second)
        {
            if (!first.Bounds.IntersectsWith(second.Bounds))
                return false;
            if (first.WorldPoints == null || second.WorldPoints == null)
                return true;

            bool firstPolygon = !string.Equals(first.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
            bool secondPolygon = !string.Equals(second.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
            if (firstPolygon && secondPolygon)
                return PolygonsIntersect(first.WorldPoints, second.WorldPoints);
            if (firstPolygon)
                return PolygonPathIntersect(first.WorldPoints, second.WorldPoints, second.PathWidth / 2);
            if (secondPolygon)
                return PolygonPathIntersect(second.WorldPoints, first.WorldPoints, first.PathWidth / 2);
            return PathsIntersect(first.WorldPoints, first.PathWidth / 2,
                second.WorldPoints, second.PathWidth / 2);
        }

        /// <summary>두 다각형의 변이 교차하거나 한쪽 꼭짓점이 다른 쪽 내부에 있으면 연결한다.</summary>
        private static bool PolygonsIntersect(GPoint[] first, GPoint[] second)
        {
            if (first.Length < 3 || second.Length < 3)
                return false;
            for (int i = 0; i < first.Length; i++)
                for (int j = 0; j < second.Length; j++)
                    if (SegmentsIntersect(first[i], first[(i + 1) % first.Length],
                        second[j], second[(j + 1) % second.Length]))
                        return true;
            return PointInPolygon(first[0], second) || PointInPolygon(second[0], first);
        }

        /// <summary>PATH 중심선과 Polygon 내부 또는 경계 사이의 거리가 PATH 반폭 이내인지 검사한다.</summary>
        private static bool PolygonPathIntersect(GPoint[] polygon, GPoint[] path, double radius)
        {
            if (polygon.Length < 3 || path.Length == 0)
                return false;
            for (int i = 0; i < path.Length; i++)
                if (PointInPolygon(path[i], polygon))
                    return true;

            for (int i = 0; i < Math.Max(1, path.Length - 1); i++)
            {
                GPoint start = path[i];
                GPoint end = path[Math.Min(i + 1, path.Length - 1)];
                for (int j = 0; j < polygon.Length; j++)
                    if (SegmentDistance(start, end, polygon[j],
                        polygon[(j + 1) % polygon.Length]) <= radius + Epsilon)
                        return true;
            }
            return false;
        }

        /// <summary>두 PATH의 선분 사이 최단 거리가 각 PATH 반폭의 합 이내인지 검사한다.</summary>
        private static bool PathsIntersect(GPoint[] first, double firstRadius,
            GPoint[] second, double secondRadius)
        {
            if (first.Length == 0 || second.Length == 0)
                return false;
            double limit = firstRadius + secondRadius + Epsilon;
            for (int i = 0; i < Math.Max(1, first.Length - 1); i++)
                for (int j = 0; j < Math.Max(1, second.Length - 1); j++)
                    if (SegmentDistance(first[i], first[Math.Min(i + 1, first.Length - 1)],
                        second[j], second[Math.Min(j + 1, second.Length - 1)]) <= limit)
                        return true;
            return false;
        }

        /// <summary>교차하는 선분은 거리 0, 그 외에는 네 끝점의 선분 거릿값 중 최소값을 반환한다.</summary>
        private static double SegmentDistance(GPoint a, GPoint b, GPoint c, GPoint d)
        {
            if (SegmentsIntersect(a, b, c, d))
                return 0;
            return Math.Min(Math.Min(PointSegmentDistance(a, c, d), PointSegmentDistance(b, c, d)),
                Math.Min(PointSegmentDistance(c, a, b), PointSegmentDistance(d, a, b)));
        }

        /// <summary>점에서 선분까지의 최단 거리를 투영 비율 0~1로 제한해 계산한다.</summary>
        private static double PointSegmentDistance(GPoint point, GPoint start, GPoint end)
        {
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double lengthSquared = dx * dx + dy * dy;
            double ratio = lengthSquared <= Epsilon ? 0 :
                Math.Max(0, Math.Min(1, ((point.X - start.X) * dx + (point.Y - start.Y) * dy) / lengthSquared));
            double gapX = point.X - start.X - ratio * dx;
            double gapY = point.Y - start.Y - ratio * dy;
            return Math.Sqrt(gapX * gapX + gapY * gapY);
        }

        /// <summary>선분 방향과 경계 포함 조건으로 두 선분의 교차 여부를 계산한다.</summary>
        private static bool SegmentsIntersect(GPoint a, GPoint b, GPoint c, GPoint d)
        {
            double abC = Cross(a, b, c);
            double abD = Cross(a, b, d);
            double cdA = Cross(c, d, a);
            double cdB = Cross(c, d, b);
            if (((abC > Epsilon && abD < -Epsilon) || (abC < -Epsilon && abD > Epsilon))
                && ((cdA > Epsilon && cdB < -Epsilon) || (cdA < -Epsilon && cdB > Epsilon)))
                return true;
            return (Math.Abs(abC) <= Epsilon && OnSegment(c, a, b))
                || (Math.Abs(abD) <= Epsilon && OnSegment(d, a, b))
                || (Math.Abs(cdA) <= Epsilon && OnSegment(a, c, d))
                || (Math.Abs(cdB) <= Epsilon && OnSegment(b, c, d));
        }

        /// <summary>점이 선분의 X/Y 범위에 포함되는지 검사한다.</summary>
        private static bool OnSegment(GPoint point, GPoint start, GPoint end)
        {
            return point.X >= Math.Min(start.X, end.X) - Epsilon
                && point.X <= Math.Max(start.X, end.X) + Epsilon
                && point.Y >= Math.Min(start.Y, end.Y) - Epsilon
                && point.Y <= Math.Max(start.Y, end.Y) + Epsilon;
        }

        /// <summary>다각형 경계는 포함하고 내부는 수평 광선의 교차 횟수로 판정한다.</summary>
        private static bool PointInPolygon(GPoint point, GPoint[] polygon)
        {
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (Math.Abs(Cross(polygon[j], polygon[i], point)) <= Epsilon
                    && OnSegment(point, polygon[j], polygon[i]))
                    return true;
                if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)
                    && point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y)
                        / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                    inside = !inside;
            }
            return inside;
        }

        private static double Cross(GPoint a, GPoint b, GPoint c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }
    }
}
