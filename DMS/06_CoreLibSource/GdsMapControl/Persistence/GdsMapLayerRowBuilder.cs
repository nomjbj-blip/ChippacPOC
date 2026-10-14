using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// GDS Layer 정보를 DB 저장 서비스(iGdsMapImport.CreateLayerBatch)에 보낼 행으로 바꾼다.
    /// 설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 6.1절.
    /// 원본 행 테이블(TQP_GDS_SOURCE_EL) 제거로 원본 행 생성 부분을 빼고 Layer 행만 남겼다(2026-10-12).
    /// </summary>
    public static class GdsMapLayerRowBuilder
    {
        /// <summary>Layer 배치 행(LAYER_ID / COLOR_ARGB / DISPLAY_NAME)을 만든다. 배치 도형 FK 대상이므로 저장할 Layer를 모두 넘긴다.</summary>
        public static string[,] BuildLayerRows(IList<KeyValuePair<int, int>> layerColors)
        {
            if (layerColors == null) throw new ArgumentNullException(nameof(layerColors));
            var rows = new string[layerColors.Count, 3];
            for (int i = 0; i < layerColors.Count; i++)
            {
                rows[i, 0] = layerColors[i].Key.ToString(CultureInfo.InvariantCulture);
                rows[i, 1] = layerColors[i].Value.ToString(CultureInfo.InvariantCulture);
                rows[i, 2] = null;
            }
            return rows;
        }

        /// <summary>원본 Element 전체에서 Layer ID 목록을 모은다. 화면 없이 저장할 때(Probe) 사용한다.</summary>
        public static List<int> CollectLayerIds(GdsLibrary library)
        {
            var ids = new SortedSet<int>();
            foreach (var structure in library.Structures)
                foreach (var layer in structure.Layers)
                    ids.Add(layer.LayerID);
            return ids.ToList();
        }
    }
}
