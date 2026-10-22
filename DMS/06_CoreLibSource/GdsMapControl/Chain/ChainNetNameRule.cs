using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// U8 Chain 이름 / Code 규칙 (2026-10-20, GDS Map View U8 설계 D3 / D4).
    ///  - Code = 덩어리 안 번호 Label(Layer 50/3, 예 TSV_2162). Device 안에서 유일해 다시 만들 때 같은 TSV 를 알아본다.
    ///  - 이름 = Net Label(Layer 50/4, 예 VDD) + "_" + 번호(2162) = VDD_2162 (E-Test 항목 이름 '@' 뒤와 같은 형식).
    ///    Net Label 이 없으면(Map 에 50/4 를 저장하지 않음) Code.
    ///  - E-Test Net 200개 번호 = U8 TSV 번호 200 / 200 확인(2026-10-20, U8 E-Test 샘플).
    ///  - E-Test Net 목록 붙여넣기로 이름 / 범위를 정하던 기능은 뺐다 (2026-10-20).
    /// </summary>
    public sealed class ChainNetNameRule
    {
        private static readonly Regex NumberSuffix = new Regex(@"_(\d+)$", RegexOptions.CultureInvariant);

        public ChainNetNameRule()
        {
            NumberLabelLayerKey = GdsLayerKey.Make(50, 3);
            NetLabelLayerKey = GdsLayerKey.Make(50, 4);
        }

        /// <summary>번호 Label Layer 키(U8: 50/3 TSV 번호).</summary>
        public int NumberLabelLayerKey { get; set; }

        /// <summary>Net 이름 Label Layer 키(U8: 50/4).</summary>
        public int NetLabelLayerKey { get; set; }

        /// <summary>덩어리의 번호 Label 글자(Code). 없으면 null.</summary>
        public string CodeOf(ChainComponent component)
        {
            ChainComponentLabel label = component.Labels.FirstOrDefault(l => l.LayerKey == NumberLabelLayerKey);
            return label == null ? null : label.Text.Trim();
        }

        /// <summary>번호 Label 개수(같은 글자는 하나). 2개 이상이면 이웃 묶음과 붙은 덩어리다.</summary>
        public int NumberLabelCount(ChainComponent component)
        {
            return component.Labels.Where(l => l.LayerKey == NumberLabelLayerKey).Select(l => l.Text.Trim()).Distinct(StringComparer.Ordinal).Count();
        }

        /// <summary>Chain 이름: Net Label + 번호, 없으면 Code. Code 가 없으면 null.</summary>
        public string NameOf(ChainComponent component)
        {
            string code = CodeOf(component);
            if (code == null) return null;
            string number = NumberOf(code);
            ChainComponentLabel net = component.Labels.FirstOrDefault(l => l.LayerKey == NetLabelLayerKey && l.Text.Trim().Length > 0);
            if (net != null && number != null) return net.Text.Trim() + "_" + number;
            return code;
        }

        /// <summary>글자 끝의 "_숫자" 숫자(예 TSV_2162 -> 2162). 없으면 null.</summary>
        public static string NumberOf(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            Match m = NumberSuffix.Match(text.Trim());
            return m.Success ? m.Groups[1].Value : null;
        }
    }
}
