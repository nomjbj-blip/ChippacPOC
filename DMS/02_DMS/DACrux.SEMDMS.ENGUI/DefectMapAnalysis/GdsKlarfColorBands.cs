using System;
using System.Collections.Generic;
using System.Drawing;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>색 구간 1개 (From 이상 To 이하, 경계 포함).</summary>
    public sealed class GdsKlarfColorBand
    {
        public int From;
        public int To;
        public Color Color;

        public bool Contains(int value)
        {
            return value >= From && value <= To;
        }
    }

    /// <summary>
    /// 연관 분석 Wafer Map 의 Die 색 규칙이다. 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4.4절 (결정 A1 / A2)
    ///   검사 계획 밖 Die      : 칠하지 않는다(기존 Map 표시 그대로)
    ///   Defect 없음           : 흰색 (기존 Defect Count Color 0 구간과 같음)
    ///   N &lt; 표시 기준       : 연회색 (Chain 밖 Defect 만 있는 Die 포함)
    ///   N &gt;= 표시 기준      : 구간 색 5단계 (기존 Defect Count Color 기본 색 순서: 연두 -> 노랑 -> 주황 -> 빨강 -> 자주)
    /// N = Die 안에서 Chain 영역에 걸린 Defect 수(Defect 중복 제거).
    /// </summary>
    public static class GdsKlarfColorBands
    {
        /// <summary>마지막 구간의 TO. 기존 Defect Count Color 의 마지막 구간처럼 큰 값으로 열어 둔다.</summary>
        public const int OpenEnd = 999;

        public static readonly Color NoDefectColor = Color.White;
        public static readonly Color BelowThresholdColor = Color.FromArgb(0xE0, 0xE0, 0xE0);

        /// <summary>DPUCDefecMapViewer.GetInitDefectConfig 기본 구간 색(1~5 구간째)과 같다.</summary>
        public static readonly Color[] Palette =
        {
            ColorTranslator.FromHtml("#DAF7A6"),
            ColorTranslator.FromHtml("#FFC300"),
            ColorTranslator.FromHtml("#FF5733"),
            ColorTranslator.FromHtml("#C70039"),
            ColorTranslator.FromHtml("#900C3F")
        };

        /// <summary>
        /// 자동 구간을 만든다. threshold = 표시 기준(1 이상), max = 이 Wafer 의 N 최대값.
        ///   max - threshold + 1 &lt;= 5 : 1개씩 한 구간, 마지막 구간 TO = 999
        ///   그보다 크면                : 폭 = ceil((max - threshold + 1) / 5) 로 5구간, 마지막 구간 TO = 999
        /// max 가 threshold 보다 작으면(걸린 Die 없음) threshold 1개 구간만 만든다.
        /// </summary>
        public static List<GdsKlarfColorBand> Auto(int threshold, int max)
        {
            if (threshold < 1) threshold = 1;
            var bands = new List<GdsKlarfColorBand>();
            int span = Math.Max(1, max - threshold + 1);
            int count = Math.Min(Palette.Length, span);
            int width = span <= Palette.Length ? 1 : (int)Math.Ceiling(span / (double)Palette.Length);
            for (int i = 0; i < count; i++)
            {
                int from = threshold + i * width;
                bands.Add(new GdsKlarfColorBand
                {
                    From = from,
                    To = i == count - 1 ? OpenEnd : from + width - 1,
                    Color = Palette[i]
                });
            }
            return bands;
        }

        /// <summary>
        /// Die 채움색. defectCount = Die 전체 Defect 수, chainCount = Chain 영역 Defect 수(N).
        /// 구간에 들지 않는 N(사용자가 구간을 고쳐 빈 곳이 생긴 경우)은 연회색으로 칠한다.
        /// </summary>
        public static Color DieColor(int defectCount, int chainCount, int threshold, IList<GdsKlarfColorBand> bands)
        {
            if (defectCount <= 0) return NoDefectColor;
            if (chainCount < threshold || chainCount <= 0) return BelowThresholdColor;
            foreach (GdsKlarfColorBand band in bands)
                if (band.Contains(chainCount)) return band.Color;
            return BelowThresholdColor;
        }
    }
}
