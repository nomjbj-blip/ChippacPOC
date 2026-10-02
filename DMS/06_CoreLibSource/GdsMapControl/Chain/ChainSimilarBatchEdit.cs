using System;
using System.Collections.Generic;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>한 번의 일괄 제외에서 실제 바뀐 수동 편집 Key와 이전 상태를 보관한다.</summary>
    public sealed class ChainSimilarBatchChange
    {
        private readonly HashSet<string> _changed;
        private readonly HashSet<string> _addedBefore;
        private readonly HashSet<string> _excludedBefore;

        internal ChainSimilarBatchChange(HashSet<string> changed, HashSet<string> addedBefore,
            HashSet<string> excludedBefore)
        {
            _changed = changed;
            _addedBefore = addedBefore;
            _excludedBefore = excludedBefore;
        }

        public int ChangedCount { get { return _changed.Count; } }

        /// <summary>이번 작업에서 바뀐 Key만 원래 수동 추가/제외 상태로 복원한다.</summary>
        public void Restore(HashSet<string> manualAdded, HashSet<string> manualExcluded)
        {
            if (manualAdded == null) throw new ArgumentNullException(nameof(manualAdded));
            if (manualExcluded == null) throw new ArgumentNullException(nameof(manualExcluded));
            foreach (string key in _changed)
            {
                if (_addedBefore.Contains(key)) manualAdded.Add(key);
                else manualAdded.Remove(key);
                if (_excludedBefore.Contains(key)) manualExcluded.Add(key);
                else manualExcluded.Remove(key);
            }
        }
    }

    /// <summary>검색 결과의 배치 Element를 현재 Chain의 수동 편집 집합에만 반영한다.</summary>
    public static class ChainSimilarBatchEdit
    {
        /// <summary>자동 후보는 제외 집합에 넣고, 수동 추가 도형은 추가 집합에서 뺀다.</summary>
        public static ChainSimilarBatchChange Apply(IEnumerable<string> removeKeys,
            ISet<string> automaticKeys, HashSet<string> manualAdded,
            HashSet<string> manualExcluded)
        {
            if (removeKeys == null) throw new ArgumentNullException(nameof(removeKeys));
            if (automaticKeys == null) throw new ArgumentNullException(nameof(automaticKeys));
            if (manualAdded == null) throw new ArgumentNullException(nameof(manualAdded));
            if (manualExcluded == null) throw new ArgumentNullException(nameof(manualExcluded));
            var changed = new HashSet<string>(StringComparer.Ordinal);
            var addedBefore = new HashSet<string>(StringComparer.Ordinal);
            var excludedBefore = new HashSet<string>(StringComparer.Ordinal);
            foreach (string key in removeKeys.Distinct(StringComparer.Ordinal))
            {
                bool wasAdded = manualAdded.Contains(key);
                bool wasExcluded = manualExcluded.Contains(key);
                bool willExclude = automaticKeys.Contains(key);
                if (!wasAdded && wasExcluded == willExclude) continue;
                changed.Add(key);
                if (wasAdded) addedBefore.Add(key);
                if (wasExcluded) excludedBefore.Add(key);
                manualAdded.Remove(key);
                if (willExclude) manualExcluded.Add(key);
                else manualExcluded.Remove(key);
            }
            return new ChainSimilarBatchChange(changed, addedBefore, excludedBefore);
        }
    }
}
