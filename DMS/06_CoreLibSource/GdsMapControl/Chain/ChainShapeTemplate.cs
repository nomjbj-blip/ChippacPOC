using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>예시와 비교한 결과.</summary>
    public enum ChainShapeMatch
    {
        /// <summary>구성 / 배치 모두 같음(평행 이동).</summary>
        Match,
        /// <summary>회전 / 좌우 대칭하면 같음.</summary>
        MatchTransformed,
        /// <summary>도형 구성은 같지만 배치가 다름.</summary>
        SamePartsDifferentLayout,
        /// <summary>도형이 더 많음(이웃 묶음과 닿음 등).</summary>
        MoreElements,
        /// <summary>도형이 더 적음(연결 / 끝점 없음 등).</summary>
        FewerElements,
        /// <summary>도형 수는 같지만 구성이 다름(크기 / 모양이 다른 도형).</summary>
        DifferentParts
    }

    /// <summary>
    /// 예시 덩어리(연결된 도형 묶음)의 형태 지문. 다른 덩어리가 같은 형태인지 비교한다 (2026-10-20, GDS Map View U8).
    ///  - 구성: Layer 키 / 도형 종류 / 꼭짓점 수(닫힌 점 중복 제외) / 폭 x 높이(SizeStep µm 묶음)의 목록이 같은지.
    ///    1 µm 반올림은 114.5 같은 경계 값에서 같은 도형이 갈려 5 µm 로 묶는다.
    ///  - 배치: 예시에서 가장 드문 도형을 기준점으로 모든 도형 중심의 상대 위치가 Tolerance(µm) 안인지.
    ///    회전 0 / 90 / 180 / 270 x 좌우 대칭 8 방향까지 본다(90 / 270 은 폭 / 높이가 바뀐다).
    /// 설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 5절.
    /// </summary>
    public sealed class ChainShapeTemplate
    {
        private sealed class Part
        {
            public string Key;
            public double Dx;
            public double Dy;
        }

        private readonly double _sizeStep;
        private readonly double _tolerance;
        private readonly List<string> _keys;
        private readonly string _anchorKey;
        private readonly List<Part> _parts;

        public ChainShapeTemplate(IList<ChainTraceElement> example, double sizeStep = 5, double tolerance = 0.5)
        {
            if (example == null || example.Count == 0) throw new ArgumentException("예시 도형이 없습니다.", nameof(example));
            if (sizeStep <= 0) throw new ArgumentOutOfRangeException(nameof(sizeStep));
            _sizeStep = sizeStep;
            _tolerance = tolerance;
            _keys = example.Select(e => Key(e, false)).OrderBy(k => k, StringComparer.Ordinal).ToList();
            _anchorKey = _keys.GroupBy(k => k).OrderBy(g => g.Count()).ThenBy(g => g.Key, StringComparer.Ordinal).First().Key;
            GPoint anchor;
            Anchor(example, _anchorKey, out anchor);
            _parts = Parts(example, anchor);
            Description = string.Join(" + ", _keys.GroupBy(k => k).Select(g => g.Count() + " x [" + g.Key + "]").ToArray());
        }

        public int ElementCount { get { return _keys.Count; } }

        /// <summary>구성 요약(예: 2 x [850/0 BOUNDARY 64점 55x55] + ...).</summary>
        public string Description { get; private set; }

        /// <summary>덩어리를 예시와 비교한다. transform: 일치한 방향(0 = 그대로, 1~7 = 회전 / 대칭), 일치하지 않으면 -1.</summary>
        public ChainShapeMatch Compare(IList<ChainTraceElement> candidate, out int transform)
        {
            transform = -1;
            if (candidate.Count > _keys.Count) return ChainShapeMatch.MoreElements;
            if (candidate.Count < _keys.Count) return ChainShapeMatch.FewerElements;
            List<string> keys = candidate.Select(e => Key(e, false)).OrderBy(k => k, StringComparer.Ordinal).ToList();
            bool sameParts = keys.SequenceEqual(_keys);
            for (int t = 0; t < 8; t++)
            {
                bool swap = (t & 1) == 1;
                List<string> expect = swap ? _keys.Select(SwapKey).OrderBy(k => k, StringComparer.Ordinal).ToList() : _keys;
                if (!keys.SequenceEqual(expect)) continue;
                GPoint anchor;
                if (!Anchor(candidate, swap ? SwapKey(_anchorKey) : _anchorKey, out anchor)) continue;
                if (!SameLayout(Parts(candidate, anchor), t, swap)) continue;
                transform = t;
                return t == 0 ? ChainShapeMatch.Match : ChainShapeMatch.MatchTransformed;
            }
            return sameParts ? ChainShapeMatch.SamePartsDifferentLayout : ChainShapeMatch.DifferentParts;
        }

        /// <summary>화면 / 목록 표시 글자.</summary>
        public static string Text(ChainShapeMatch match)
        {
            switch (match)
            {
                case ChainShapeMatch.Match: return "일치";
                case ChainShapeMatch.MatchTransformed: return "회전·대칭 일치";
                case ChainShapeMatch.SamePartsDifferentLayout: return "배치 다름";
                case ChainShapeMatch.MoreElements: return "도형 더 많음";
                case ChainShapeMatch.FewerElements: return "도형 더 적음";
                default: return "구성 다름";
            }
        }

        private bool SameLayout(List<Part> parts, int t, bool swap)
        {
            var used = new bool[parts.Count];
            foreach (Part tp in _parts)
            {
                double ex, ey;
                Transform(t, tp.Dx, tp.Dy, out ex, out ey);
                string key = swap ? SwapKey(tp.Key) : tp.Key;
                int hit = -1;
                for (int k = 0; k < parts.Count; k++)
                    if (!used[k] && parts[k].Key == key && Math.Abs(parts[k].Dx - ex) <= _tolerance && Math.Abs(parts[k].Dy - ey) <= _tolerance)
                    {
                        hit = k;
                        break;
                    }
                if (hit < 0) return false;
                used[hit] = true;
            }
            return true;
        }

        /// <summary>회전 0 / 90 / 180 / 270(t &amp; 3) x 좌우 대칭(t &amp; 4).</summary>
        private static void Transform(int t, double x, double y, out double ox, out double oy)
        {
            double rx = (t & 4) != 0 ? -x : x, ry = y;
            switch (t & 3)
            {
                case 0: ox = rx; oy = ry; break;
                case 1: ox = -ry; oy = rx; break;
                case 2: ox = -rx; oy = -ry; break;
                default: ox = ry; oy = -rx; break;
            }
        }

        private bool Anchor(IList<ChainTraceElement> elements, string key, out GPoint anchor)
        {
            foreach (ChainTraceElement e in elements)
                if (Key(e, false) == key)
                {
                    anchor = Center(e);
                    return true;
                }
            anchor = new GPoint(0, 0);
            return false;
        }

        private List<Part> Parts(IList<ChainTraceElement> elements, GPoint anchor)
        {
            return elements.Select(e =>
            {
                GPoint c = Center(e);
                return new Part { Key = Key(e, false), Dx = c.X - anchor.X, Dy = c.Y - anchor.Y };
            }).ToList();
        }

        private static GPoint Center(ChainTraceElement e)
        {
            return new GPoint((e.Bounds.MinX + e.Bounds.MaxX) / 2, (e.Bounds.MinY + e.Bounds.MaxY) / 2);
        }

        private string Key(ChainTraceElement e, bool swap)
        {
            int count = e.WorldPoints == null ? 0 : e.WorldPoints.Length;
            bool closed = !string.Equals(e.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
            if (closed && count > 1 && Math.Abs(e.WorldPoints[0].X - e.WorldPoints[count - 1].X) < 1e-6
                && Math.Abs(e.WorldPoints[0].Y - e.WorldPoints[count - 1].Y) < 1e-6)
                count--;
            double w = Step(e.Bounds.MaxX - e.Bounds.MinX), h = Step(e.Bounds.MaxY - e.Bounds.MinY);
            if (swap) { double s = w; w = h; h = s; }
            return GdsLayerKey.Format(e.LayerKey) + " " + e.ElementType.ToUpperInvariant() + " " + count + "점 "
                + w.ToString(CultureInfo.InvariantCulture) + "x" + h.ToString(CultureInfo.InvariantCulture);
        }

        private double Step(double value)
        {
            return Math.Round(value / _sizeStep, MidpointRounding.AwayFromZero) * _sizeStep;
        }

        /// <summary>구성 키의 폭 / 높이를 바꾼다(90 / 270 도 회전 비교용).</summary>
        private static string SwapKey(string key)
        {
            int space = key.LastIndexOf(' ');
            string[] wh = key.Substring(space + 1).Split('x');
            return key.Substring(0, space + 1) + wh[1] + "x" + wh[0];
        }
    }
}
