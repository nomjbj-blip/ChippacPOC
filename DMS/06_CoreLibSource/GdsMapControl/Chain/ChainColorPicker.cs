using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// 새 Chain에 이미 쓰고 있는 Chain 색과 최대한 다른 색을 고른다. (2026-10-08 요청)
    /// 이전에는 고정 8색을 생성 순서대로 돌려 써서, DB에서 불러온 Chain 색이나 비슷한 계열(LimeGreen / Cyan / DeepSkyBlue)이 겹쳤다.
    ///
    /// 처리 방식
    ///  - 후보: 색상환 24개(15도 간격) x 밝기 / 채도 3단계. 어두운 배경(#18181C)에서 보이도록 너무 어두운 색은 만들지 않는다.
    ///    흰색 / 흰색에 가까운 색은 뺀다(2026-10-12 요청: 흰색은 유사 묶음 찾기 결과 표시 색이라 Chain 색과 겹친다).
    ///  - 거리: 사람 눈의 차이에 가까운 CIE Lab 거리(ΔE76)를 쓴다. RGB 거리는 초록 계열 차이를 과대평가해 비슷한 초록이 다르게 계산된다.
    ///  - 점수: 기존 Chain 색과의 최소 거리가 1순위다. 지도 Layer 색과의 거리는 거의 같은 후보 사이에서만 영향을 주도록 작은 가중치로 더한다.
    /// </summary>
    public static class ChainColorPicker
    {
        /// <summary>Layer 색 거리는 이 값까지만 반영한다. 충분히 떨어지면 더 멀어도 점수에 이득이 없게 해 Chain 간 구분을 우선한다.</summary>
        private const double LayerDistanceCap = 40.0;
        private const double LayerWeight = 0.3;

        /// <summary>
        /// Chain 자동 색에서 빼는 색: 유사 묶음 찾기 결과 표시 색(흰색, GdsMapControl_ChainOverlay 의 ChainSimilarFoundColor 와 같은 색).
        /// 이 색과 ΔE MinReservedDistance 미만인 후보도 뺀다.
        /// </summary>
        public static readonly Color SimilarPreviewColor = Color.White;
        private const double MinReservedDistance = 25.0;

        private static readonly Color[] Candidates = BuildCandidates();

        /// <summary>색 선택 창(ColorDialog) 사용자 정의 색으로 보여 줄 대표 16색(구분이 잘 되는 순서).</summary>
        public static readonly Color[] SuggestedColors =
        {
            Color.FromArgb(255, 215, 0), Color.FromArgb(0, 191, 255), Color.FromArgb(255, 0, 255), Color.FromArgb(255, 69, 0),
            Color.FromArgb(0, 255, 0), Color.FromArgb(240, 128, 128), Color.FromArgb(200, 120, 255), Color.FromArgb(255, 140, 0),
            Color.FromArgb(0, 255, 255), Color.FromArgb(255, 105, 180), Color.FromArgb(173, 255, 47), Color.FromArgb(30, 144, 255),
            Color.FromArgb(220, 20, 60), Color.FromArgb(255, 250, 120), Color.FromArgb(127, 255, 212), Color.FromArgb(186, 85, 211)
        };

        /// <summary>
        /// 사용 중인 Chain 색과 가장 먼 후보를 반환한다.
        /// usedChainColors: 화면의 모든 Chain 색(DB에서 불러온 Chain 포함). avoidLayerColors: 지도 Layer 색(없으면 null).
        /// 같은 입력이면 항상 같은 색을 고른다(후보 순서 고정).
        /// </summary>
        public static Color Pick(IEnumerable<Color> usedChainColors, IEnumerable<Color> avoidLayerColors)
        {
            double[][] used = (usedChainColors ?? Enumerable.Empty<Color>()).Select(ToLab).ToArray();
            double[][] layers = (avoidLayerColors ?? Enumerable.Empty<Color>()).Select(ToLab).ToArray();
            Color best = Candidates[0];
            double bestScore = double.MinValue;
            foreach (Color candidate in Candidates)
            {
                double[] lab = ToLab(candidate);
                // Chain이 없으면 1순위 점수는 모두 같으므로 Layer 거리만으로 고른다.
                double chainDistance = used.Length == 0 ? 0 : used.Min(other => Distance(lab, other));
                double layerDistance = layers.Length == 0 ? LayerDistanceCap
                    : Math.Min(LayerDistanceCap, layers.Min(other => Distance(lab, other)));
                double score = chainDistance + LayerWeight * layerDistance;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }
            return best;
        }

        /// <summary>유사 묶음 표시 색(흰색)과 구분이 어려운 색인지. 사용자가 직접 고른 색을 안내할 때 쓴다.</summary>
        public static bool IsNearSimilarPreviewColor(Color color)
        {
            return Difference(color, SimilarPreviewColor) < MinReservedDistance;
        }

        /// <summary>두 색의 CIE Lab 거리(ΔE76). 약 10 이하면 화면에서 같은 계열로 보이기 쉽다.</summary>
        public static double Difference(Color first, Color second)
        {
            return Distance(ToLab(first), ToLab(second));
        }

        /// <summary>
        /// HSV 색상환 24개 x (진한 / 연한 / 조금 어두운) 3단계. 흰색 계열(유사 묶음 표시 색 근처)은 넣지 않는다.
        /// 지도 배경이 어두워(#18181C) 밝기(Lab L)가 MinLightness 미만인 색(예: 순수 파랑 #0000FF, L 약 32)은 후보에서 뺀다.
        /// </summary>
        private static Color[] BuildCandidates()
        {
            var list = new List<Color>();
            var levels = new[] { new[] { 1.0, 1.0 }, new[] { 0.55, 1.0 }, new[] { 1.0, 0.75 } };
            foreach (double[] level in levels)
                for (int hue = 0; hue < 360; hue += 15)
                {
                    Color color = FromHsv(hue, level[0], level[1]);
                    if (ToLab(color)[0] >= MinLightness && !IsNearSimilarPreviewColor(color))
                        list.Add(color);
                }
            return list.ToArray();
        }

        /// <summary>후보 색의 최소 밝기(Lab L, 0~100). 어두운 배경에서 Chain 외곽선이 보이도록 한다.</summary>
        private const double MinLightness = 50.0;

        private static Color FromHsv(double hue, double saturation, double value)
        {
            double c = value * saturation;
            double x = c * (1 - Math.Abs(hue / 60.0 % 2 - 1));
            double m = value - c;
            double r, g, b;
            if (hue < 60) { r = c; g = x; b = 0; }
            else if (hue < 120) { r = x; g = c; b = 0; }
            else if (hue < 180) { r = 0; g = c; b = x; }
            else if (hue < 240) { r = 0; g = x; b = c; }
            else if (hue < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }
            return Color.FromArgb(255, (int)Math.Round((r + m) * 255), (int)Math.Round((g + m) * 255), (int)Math.Round((b + m) * 255));
        }

        private static double Distance(double[] a, double[] b)
        {
            double l = a[0] - b[0], u = a[1] - b[1], v = a[2] - b[2];
            return Math.Sqrt(l * l + u * u + v * v);
        }

        /// <summary>sRGB(D65) -> CIE Lab. 투명도는 무시한다.</summary>
        private static double[] ToLab(Color color)
        {
            double r = Linear(color.R / 255.0), g = Linear(color.G / 255.0), b = Linear(color.B / 255.0);
            double x = (r * 0.4124 + g * 0.3576 + b * 0.1805) / 0.95047;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = (r * 0.0193 + g * 0.1192 + b * 0.9505) / 1.08883;
            x = LabF(x); y = LabF(y); z = LabF(z);
            return new[] { 116 * y - 16, 500 * (x - y), 200 * (y - z) };
        }

        private static double Linear(double channel)
        {
            return channel <= 0.04045 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
        }

        private static double LabF(double t)
        {
            return t > 0.008856 ? Math.Pow(t, 1.0 / 3.0) : 7.787 * t + 16.0 / 116.0;
        }
    }
}
