using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>Label 검색 일치 방식(2026-10-07 결정 2). 모두 대소문자 무시, 앞뒤 공백 제거.</summary>
    public enum GdsLabelMatchMode
    {
        /// <summary>값 전체가 같음(기본).</summary>
        Exact,
        /// <summary>입력으로 시작.</summary>
        Prefix,
        /// <summary>입력을 포함.</summary>
        Contains
    }

    /// <summary>Label 색인 한 행. 좌표는 DB 저장 기준(TQP_GDS_MAP.VIEW_SIDE) - 화면에 쓸 때 보기 기준으로 X 변환한다.</summary>
    public sealed class GdsLabelEntry
    {
        public string PlacedElementId { get; set; }
        public int LayerId { get; set; }
        public int? TextType { get; set; }
        public string Text { get; set; }
        public double StoredX { get; set; }
        public double Y { get; set; }
    }

    /// <summary>
    /// GDS Map View Label 검색 색인(설계: 문서/2026-10-07_GDS_Label_검색_설계.md 3 / 4절).
    /// [조회] 때 서비스 GetMapTextIndex로 Device의 TEXT 도형 전체(Layer 무관 = 속성 기준)를 한 번 받아 두고 메모리에서 찾는다.
    /// 화면과 분리해 둔 이유: Probe가 화면 없이 같은 검색 규칙을 확인하기 위해서다.
    /// </summary>
    public sealed class GdsLabelIndex
    {
        private readonly List<GdsLabelEntry> _entries;

        public GdsLabelIndex(IEnumerable<GdsLabelEntry> entries)
        {
            _entries = (entries ?? Enumerable.Empty<GdsLabelEntry>()).ToList();
        }

        public int Count { get { return _entries.Count; } }

        public IList<GdsLabelEntry> Entries { get { return _entries; } }

        /// <summary>서비스 결과(PLACED_ELEMENT_ID / LAYER_ID / TEXT_TYPE / TEXT_VALUE / MIN_X / MIN_Y)를 색인으로 바꾼다.</summary>
        public static GdsLabelIndex FromTable(DataTable table)
        {
            var entries = new List<GdsLabelEntry>(table == null ? 0 : table.Rows.Count);
            if (table != null)
                foreach (DataRow row in table.Rows)
                    entries.Add(new GdsLabelEntry
                    {
                        PlacedElementId = Convert.ToString(row["PLACED_ELEMENT_ID"], CultureInfo.InvariantCulture),
                        LayerId = Convert.ToInt32(row["LAYER_ID"], CultureInfo.InvariantCulture),
                        TextType = row["TEXT_TYPE"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["TEXT_TYPE"], CultureInfo.InvariantCulture),
                        Text = row["TEXT_VALUE"] == DBNull.Value ? string.Empty : Convert.ToString(row["TEXT_VALUE"], CultureInfo.InvariantCulture),
                        StoredX = Convert.ToDouble(row["MIN_X"], CultureInfo.InvariantCulture),
                        Y = Convert.ToDouble(row["MIN_Y"], CultureInfo.InvariantCulture)
                    });
            return new GdsLabelIndex(entries);
        }

        /// <summary>
        /// 입력과 일치하는 Label을 모두 찾는다(값 -> 위치 순 정렬). 입력이 비면 빈 목록.
        /// 정렬 이유: 같은 값의 쌍(예: 160µm 간격 2곳)이 목록에서 이웃하고, 실행마다 순서가 같도록.
        /// </summary>
        public List<GdsLabelEntry> Search(string text, GdsLabelMatchMode mode)
        {
            string key = (text ?? string.Empty).Trim();
            if (key.Length == 0) return new List<GdsLabelEntry>();
            Func<string, bool> match;
            switch (mode)
            {
                case GdsLabelMatchMode.Prefix:
                    match = value => value.StartsWith(key, StringComparison.OrdinalIgnoreCase);
                    break;
                case GdsLabelMatchMode.Contains:
                    match = value => value.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0;
                    break;
                default:
                    match = value => string.Equals(value, key, StringComparison.OrdinalIgnoreCase);
                    break;
            }
            return _entries.Where(e => match((e.Text ?? string.Empty).Trim()))
                .OrderBy(e => e.Text, StringComparer.OrdinalIgnoreCase)
                .ThenBy(e => e.Y).ThenBy(e => e.StoredX)
                .ToList();
        }
    }
}
