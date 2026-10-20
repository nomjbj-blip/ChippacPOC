using System;
using System.Collections.Generic;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// 연관 분석에 쓰는 Chain Member 1개의 실제 모양(저장 기준 µm).
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.2 / 4.3절
    ///  - BOUNDARY: 닫힌 다각형(마지막 중복 점을 뺀 꼭짓점). 오목 다각형도 그대로 쓴다.
    ///  - PATH: 선분마다 폭 W 사각형 + 끝 처리(PATH_TYPE) + 꺾인 곳 Miter 조각 + 둥근 끝 원(Type 1)
    ///  - Min / Max 는 위 조각을 모두 감싸는 범위(후보 거르기용). DB 의 MIN_X ~ MAX_Y 보다 정확하다.
    /// </summary>
    public sealed class GdsKlarfShape
    {
        public long ChainSeq;
        public string PlacedElementId;
        public int LayerId;
        /// <summary>GDS DataType. Layer 는 번호 + DataType 단위다 (2026-10-20).</summary>
        public int DataType;
        public string RoleCode;
        public bool IsPath;
        public double HalfWidth;
        public int PathType;
        public double MinX, MinY, MaxX, MaxY;

        /// <summary>BOUNDARY 다각형 1개 또는 PATH 조각(볼록 사각형 / 삼각형) 여러 개. 각 조각 = { xs, ys }. Cache 공유 값이라 읽기만 한다.</summary>
        public readonly List<double[][]> Pieces = new List<double[][]>();

        /// <summary>둥근 끝(Type 1) / 점 1개 원: { cx, cy }, 반지름 = HalfWidth. 읽기만 한다.</summary>
        public readonly List<double[]> Circles = new List<double[]>();

        /// <summary>폭 0 PATH 의 중심선(선분 거리로 판정). 읽기만 한다.</summary>
        public double[] LineXs, LineYs;
    }

    /// <summary>
    /// Defect 사각형과 Chain Member 모양 사이 거리(Gap, µm)를 구한다. 겹치거나 닿으면 0.
    /// 화면 쪽 Chain/ChainGeometryOverlap(.NET 4.8, GdsMap)의 점-선분 거리 / 선분 교차 / 다각형 포함 계산을
    /// 서버(.NET 4.0)에서 쓰도록 double 배열로 옮겼다(4.0 BSL 은 4.8 어셈블리를 참조할 수 없다).
    ///
    /// PATH 모양 규칙 (GDSII PATH, 설계 4.3절)
    ///  - Type 0: 끝 연장 없음 / Type 2: 양 끝 W/2 연장 / Type 4: 연장 값을 저장하지 않으므로 Type 2 와 같게 / Type 1: Type 0 + 양 끝 원
    ///  - 꺾인 곳: 두 선분 사각형의 바깥쪽 틈을 Miter 조각(꺾임 점 / 바깥 모서리 2개 / Miter 점)으로 메운다.
    ///    Miter 점이 꺾임 점에서 2W 보다 멀면(약 151도 넘게 꺾임) 바깥 모서리 2개를 잇는 삼각형으로 자른다.
    ///  - 점 1개: 한 변 W 정사각형(Type 1 은 원), 폭 0: 중심선
    /// </summary>
    public static class GdsKlarfShapeGeometry
    {
        private const double Epsilon = 1e-9;

        /// <summary>
        /// 저장된 도형 1개로 판정용 모양을 만든다.
        /// points = 저장 기준 좌표(x0, y0, x1, y1, ...). BOUNDARY 는 닫는 중복 점을 빼고, PATH 는 연속 중복 점을 뺀다.
        /// </summary>
        public static GdsKlarfShape Create(long chainSeq, string placedId, int layerId, string roleCode,
            string elementType, double pathWidth, int pathType, double[] xs, double[] ys)
        {
            if (xs == null || ys == null || xs.Length != ys.Length || xs.Length == 0)
                throw new ArgumentException("도형 좌표가 없습니다: " + placedId);
            var shape = new GdsKlarfShape
            {
                ChainSeq = chainSeq,
                PlacedElementId = placedId,
                LayerId = layerId,
                RoleCode = roleCode,
                IsPath = String.Equals(elementType, "PATH", StringComparison.OrdinalIgnoreCase),
                HalfWidth = Math.Abs(pathWidth) / 2,
                PathType = pathType
            };
            if (shape.IsPath) BuildPath(shape, xs, ys);
            else BuildPolygon(shape, xs, ys);
            UpdateBounds(shape);
            return shape;
        }

        private static void BuildPolygon(GdsKlarfShape shape, double[] xs, double[] ys)
        {
            int n = xs.Length;
            if (n > 1 && Same(xs[0], ys[0], xs[n - 1], ys[n - 1])) n--;   // BOUNDARY 는 마지막 점 = 첫 점
            var px = new double[n];
            var py = new double[n];
            Array.Copy(xs, px, n);
            Array.Copy(ys, py, n);
            shape.Pieces.Add(new[] { px, py });
        }

        private static void BuildPath(GdsKlarfShape shape, double[] xs, double[] ys)
        {
            var px = new List<double>();
            var py = new List<double>();
            for (int i = 0; i < xs.Length; i++)
                if (px.Count == 0 || !Same(px[px.Count - 1], py[py.Count - 1], xs[i], ys[i]))
                {
                    px.Add(xs[i]);
                    py.Add(ys[i]);
                }
            double r = shape.HalfWidth;
            int n = px.Count;
            bool round = shape.PathType == 1;
            if (r <= Epsilon)
            {
                shape.LineXs = px.ToArray();
                shape.LineYs = py.ToArray();
                return;
            }
            if (n == 1)
            {
                if (round) shape.Circles.Add(new[] { px[0], py[0] });
                else shape.Pieces.Add(new[] { new[] { px[0] - r, px[0] + r, px[0] + r, px[0] - r }, new[] { py[0] - r, py[0] - r, py[0] + r, py[0] + r } });
                return;
            }
            double ext = shape.PathType == 2 || shape.PathType == 4 ? r : 0;
            for (int i = 0; i < n - 1; i++)
            {
                double dx = px[i + 1] - px[i], dy = py[i + 1] - py[i];
                double len = Math.Sqrt(dx * dx + dy * dy);
                double ux = dx / len, uy = dy / len;
                double nx = -uy * r, ny = ux * r;
                double sx = px[i] - (i == 0 ? ext * ux : 0), sy = py[i] - (i == 0 ? ext * uy : 0);
                double ex = px[i + 1] + (i == n - 2 ? ext * ux : 0), ey = py[i + 1] + (i == n - 2 ? ext * uy : 0);
                shape.Pieces.Add(new[] { new[] { sx + nx, ex + nx, ex - nx, sx - nx }, new[] { sy + ny, ey + ny, ey - ny, sy - ny } });
            }
            for (int i = 1; i < n - 1; i++)
                AddJoin(shape, px[i - 1], py[i - 1], px[i], py[i], px[i + 1], py[i + 1], r);
            if (round)
            {
                shape.Circles.Add(new[] { px[0], py[0] });
                shape.Circles.Add(new[] { px[n - 1], py[n - 1] });
            }
        }

        /// <summary>꺾인 곳 바깥쪽 틈을 메우는 Miter 조각(Miter 길이 > 2W 면 삼각형).</summary>
        private static void AddJoin(GdsKlarfShape shape, double ax, double ay, double vx, double vy, double bx, double by, double r)
        {
            double l1 = Math.Sqrt((vx - ax) * (vx - ax) + (vy - ay) * (vy - ay));
            double l2 = Math.Sqrt((bx - vx) * (bx - vx) + (by - vy) * (by - vy));
            double u1x = (vx - ax) / l1, u1y = (vy - ay) / l1, u2x = (bx - vx) / l2, u2y = (by - vy) / l2;
            double cross = u1x * u2y - u1y * u2x;
            if (Math.Abs(cross) < 1e-12) return;                    // 곧은 선(조각끼리 이미 맞닿음)
            double side = cross > 0 ? -1 : 1;                         // 왼쪽으로 꺾이면 바깥쪽은 오른쪽(-법선)
            double n1x = -u1y, n1y = u1x, n2x = -u2y, n2y = u2x;
            double c1x = vx + side * r * n1x, c1y = vy + side * r * n1y;
            double c2x = vx + side * r * n2x, c2y = vy + side * r * n2y;
            double bisX = n1x + n2x, bisY = n1y + n2y;
            double bisLen = Math.Sqrt(bisX * bisX + bisY * bisY);
            double cosHalf = bisLen / 2;                              // 두 법선 사이 각의 절반의 cos
            if (cosHalf < 0.25 || bisLen < Epsilon)                   // Miter 길이 r / cosHalf > 4r = 2W
            {
                shape.Pieces.Add(new[] { new[] { vx, c1x, c2x }, new[] { vy, c1y, c2y } });
                return;
            }
            double miter = r / cosHalf;
            double mx = vx + side * bisX / bisLen * miter, my = vy + side * bisY / bisLen * miter;
            shape.Pieces.Add(new[] { new[] { vx, c1x, mx, c2x }, new[] { vy, c1y, my, c2y } });
        }

        private static void UpdateBounds(GdsKlarfShape s)
        {
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            foreach (double[][] p in s.Pieces)
                for (int i = 0; i < p[0].Length; i++)
                {
                    minX = Math.Min(minX, p[0][i]); maxX = Math.Max(maxX, p[0][i]);
                    minY = Math.Min(minY, p[1][i]); maxY = Math.Max(maxY, p[1][i]);
                }
            foreach (double[] c in s.Circles)
            {
                minX = Math.Min(minX, c[0] - s.HalfWidth); maxX = Math.Max(maxX, c[0] + s.HalfWidth);
                minY = Math.Min(minY, c[1] - s.HalfWidth); maxY = Math.Max(maxY, c[1] + s.HalfWidth);
            }
            if (s.LineXs != null)
                for (int i = 0; i < s.LineXs.Length; i++)
                {
                    minX = Math.Min(minX, s.LineXs[i]); maxX = Math.Max(maxX, s.LineXs[i]);
                    minY = Math.Min(minY, s.LineYs[i]); maxY = Math.Max(maxY, s.LineYs[i]);
                }
            s.MinX = minX; s.MinY = minY; s.MaxX = maxX; s.MaxY = maxY;
        }

        /// <summary>
        /// Defect 사각형 [x1, x2] x [y1, y2] 와 모양 사이 가장 가까운 거리(µm). 겹치거나 닿으면 0.
        /// 모양 조각마다 거리를 구해 최솟값을 쓴다.
        /// </summary>
        public static double Gap(GdsKlarfShape shape, double x1, double y1, double x2, double y2)
        {
            double best = double.MaxValue;
            foreach (double[][] p in shape.Pieces)
            {
                best = Math.Min(best, PolygonGap(p[0], p[1], x1, y1, x2, y2));
                if (best <= 0) return 0;
            }
            foreach (double[] c in shape.Circles)
            {
                best = Math.Min(best, Math.Max(0, PointRectDistance(c[0], c[1], x1, y1, x2, y2) - shape.HalfWidth));
                if (best <= 0) return 0;
            }
            if (shape.LineXs != null)
            {
                if (shape.LineXs.Length == 1) best = Math.Min(best, PointRectDistance(shape.LineXs[0], shape.LineYs[0], x1, y1, x2, y2));
                for (int i = 0; i < shape.LineXs.Length - 1; i++)
                    best = Math.Min(best, SegmentRectDistance(shape.LineXs[i], shape.LineYs[i], shape.LineXs[i + 1], shape.LineYs[i + 1], x1, y1, x2, y2));
            }
            return best <= Epsilon ? 0 : best;
        }

        /// <summary>
        /// 사각형과 다각형(오목 가능) 사이 거리.
        ///  1) 다각형 꼭짓점이 사각형 안 -> 0  2) 변이 사각형과 만남 -> 0  3) 사각형 중심이 다각형 안 -> 0
        ///  4) 그 밖 = 변마다 선분-사각형 거리의 최솟값
        /// </summary>
        public static double PolygonGap(double[] xs, double[] ys, double x1, double y1, double x2, double y2)
        {
            int n = xs.Length;
            if (n == 1) return PointRectDistance(xs[0], ys[0], x1, y1, x2, y2);
            for (int i = 0; i < n; i++)
                if (xs[i] >= x1 - Epsilon && xs[i] <= x2 + Epsilon && ys[i] >= y1 - Epsilon && ys[i] <= y2 + Epsilon) return 0;
            double best = double.MaxValue;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                if (n == 2 && j == 0) break;
                double d = SegmentRectDistance(xs[i], ys[i], xs[j], ys[j], x1, y1, x2, y2);
                if (d <= Epsilon) return 0;
                best = Math.Min(best, d);
            }
            if (n >= 3 && PointInPolygon((x1 + x2) / 2, (y1 + y2) / 2, xs, ys)) return 0;
            return best;
        }

        /// <summary>선분과 사각형 사이 거리. 만나면 0, 아니면 양 끝점-사각형 / 사각형 꼭짓점-선분 거리 중 최솟값.</summary>
        public static double SegmentRectDistance(double ax, double ay, double bx, double by, double x1, double y1, double x2, double y2)
        {
            if (SegmentHitsRect(ax, ay, bx, by, x1, y1, x2, y2)) return 0;
            double d = Math.Min(PointRectDistance(ax, ay, x1, y1, x2, y2), PointRectDistance(bx, by, x1, y1, x2, y2));
            d = Math.Min(d, PointSegmentDistance(x1, y1, ax, ay, bx, by));
            d = Math.Min(d, PointSegmentDistance(x2, y1, ax, ay, bx, by));
            d = Math.Min(d, PointSegmentDistance(x2, y2, ax, ay, bx, by));
            d = Math.Min(d, PointSegmentDistance(x1, y2, ax, ay, bx, by));
            return d;
        }

        /// <summary>선분이 사각형(경계 포함)과 만나는지: Liang-Barsky 자르기.</summary>
        public static bool SegmentHitsRect(double ax, double ay, double bx, double by, double x1, double y1, double x2, double y2)
        {
            double t0 = 0, t1 = 1, dx = bx - ax, dy = by - ay;
            return Clip(-dx, ax - x1, ref t0, ref t1) && Clip(dx, x2 - ax, ref t0, ref t1)
                && Clip(-dy, ay - y1, ref t0, ref t1) && Clip(dy, y2 - ay, ref t0, ref t1);
        }

        private static bool Clip(double p, double q, ref double t0, ref double t1)
        {
            if (Math.Abs(p) < Epsilon) return q >= -Epsilon;
            double r = q / p;
            if (p < 0)
            {
                if (r > t1 + Epsilon) return false;
                if (r > t0) t0 = r;
            }
            else
            {
                if (r < t0 - Epsilon) return false;
                if (r < t1) t1 = r;
            }
            return true;
        }

        public static double PointRectDistance(double px, double py, double x1, double y1, double x2, double y2)
        {
            double dx = Math.Max(Math.Max(x1 - px, 0), px - x2);
            double dy = Math.Max(Math.Max(y1 - py, 0), py - y2);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>점-선분 거리(투영 비율을 0 ~ 1 로 제한). ChainGeometryOverlap.PointSegmentDistance 와 같은 식.</summary>
        public static double PointSegmentDistance(double px, double py, double ax, double ay, double bx, double by)
        {
            double dx = bx - ax, dy = by - ay;
            double len2 = dx * dx + dy * dy;
            double t = len2 <= Epsilon ? 0 : ((px - ax) * dx + (py - ay) * dy) / len2;
            t = Math.Max(0, Math.Min(1, t));
            double cx = ax + t * dx - px, cy = ay + t * dy - py;
            return Math.Sqrt(cx * cx + cy * cy);
        }

        /// <summary>수평 광선 짝홀 규칙. ChainGeometryOverlap.PointInPolygon 과 같은 식(경계는 거리 계산이 먼저 0 으로 잡는다).</summary>
        public static bool PointInPolygon(double px, double py, double[] xs, double[] ys)
        {
            bool inside = false;
            for (int i = 0, j = xs.Length - 1; i < xs.Length; j = i++)
                if ((ys[i] > py) != (ys[j] > py) && px < (xs[j] - xs[i]) * (py - ys[i]) / (ys[j] - ys[i]) + xs[i])
                    inside = !inside;
            return inside;
        }

        private static bool Same(double ax, double ay, double bx, double by)
        {
            return Math.Abs(ax - bx) < Epsilon && Math.Abs(ay - by) < Epsilon;
        }
    }
}
