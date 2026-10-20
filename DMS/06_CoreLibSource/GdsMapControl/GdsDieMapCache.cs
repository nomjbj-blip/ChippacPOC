using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Die 상세(화면 B) 도면 데이터 공유 캐시 (2026-10-16, 문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md 5.3 / 10절).
    /// 연관 분석 화면 [분석] 이 끝나면 미리 읽고(GdsDieAnalysisForm.PreloadMap), 상세 화면은 같은 키면 DB 를 부르지 않는다.
    ///  - 키 2개까지 둔다(최근에 쓴 것 우선). 재분석으로 도면 키가 달라진 창(Chain 면 / Chain 거르기 / Device)끼리 오가도
    ///    DB 를 다시 읽지 않고 도면만 다시 만든다(약 9초 -> 약 4초). 키 하나에 도형 데이터 약 150 MB.
    ///  - 읽는 중이면 같은 작업을 기다린다(두 번 읽지 않음). 실패한 작업은 다음 요청 때 다시 읽는다.
    ///  - 도형 목록은 보기 기준으로 맞춘(반전한) 것 하나만 둔다. 도면(GdsMapControl)이 좌표 배열을 같이 쓰므로
    ///    지금 도면의 키는 캐시가 더 차지하는 메모리가 거의 없다(2026-10-16 측정 0 MB).
    /// </summary>
    public static class GdsDieMapCache
    {
        /// <summary>캐시에 두는 키 수.</summary>
        public const int Slots = 2;

        private static readonly object Gate = new object();
        // 앞이 최근에 쓴 것
        private static readonly List<KeyValuePair<string, Task<GdsDieMapData>>> _entries = new List<KeyValuePair<string, Task<GdsDieMapData>>>();
        private static int _loadCount;

        /// <summary>DB 에서 실제로 읽기 시작한 횟수(Smoke 확인용).</summary>
        public static int LoadCount { get { return Volatile.Read(ref _loadCount); } }

        /// <summary>최근에 쓴 캐시 키(없으면 null).</summary>
        public static string Key
        {
            get { lock (Gate) return _entries.Count == 0 ? null : _entries[0].Key; }
        }

        /// <summary>캐시 키 전부(최근 순).</summary>
        public static string[] Keys
        {
            get { lock (Gate) return _entries.Select(e => e.Key).ToArray(); }
        }

        /// <summary>최근에 쓴 키의 작업(읽는 중이거나 끝난 것, 없으면 null). Smoke 가 미리 읽기 끝을 기다릴 때 쓴다.</summary>
        public static Task Pending
        {
            get { lock (Gate) return _entries.Count == 0 ? null : _entries[0].Value; }
        }

        /// <summary>
        /// 키가 같고 실패하지 않은 작업이 있으면 그것을 돌려주고(최근으로 올림), 없으면 load 를 백그라운드로 시작한다.
        /// 칸이 넘치면 가장 오래 안 쓴 키를 버린다. load 는 화면 컨트롤을 만지지 않아야 한다.
        /// </summary>
        internal static Task<GdsDieMapData> GetOrLoad(string key, Func<GdsDieMapData> load)
        {
            lock (Gate)
            {
                int i = _entries.FindIndex(e => e.Key == key);
                if (i >= 0)
                {
                    KeyValuePair<string, Task<GdsDieMapData>> hit = _entries[i];
                    _entries.RemoveAt(i);
                    if (!hit.Value.IsFaulted && !hit.Value.IsCanceled)
                    {
                        _entries.Insert(0, hit);
                        return hit.Value;
                    }
                }
                Task<GdsDieMapData> task = Task.Run(() =>
                {
                    Interlocked.Increment(ref _loadCount);
                    return load();
                });
                // 미리 읽기는 기다리는 쪽이 없을 수 있어 실패를 여기서 남긴다(상세 화면을 열 때 다시 읽는다)
                task.ContinueWith(t => System.Diagnostics.Trace.TraceWarning("GdsDieMapCache {0}: {1}", key, t.Exception.InnerException),
                    TaskContinuationOptions.OnlyOnFaulted);
                _entries.Insert(0, new KeyValuePair<string, Task<GdsDieMapData>>(key, task));
                while (_entries.Count > Slots) _entries.RemoveAt(_entries.Count - 1);
                return task;
            }
        }

        /// <summary>캐시를 비운다(연관 분석 화면 / 상세보기 창을 모두 닫았을 때). 읽는 중인 작업은 끝나도 버려진다.</summary>
        public static void Clear()
        {
            lock (Gate) _entries.Clear();
        }
    }

    /// <summary>DB 에서 읽은 상세 화면 도면 묶음. 읽은 뒤에는 바꾸지 않는다(여러 화면 / 캐시가 같이 쓴다).</summary>
    internal sealed class GdsDieMapData
    {
        public string TopStructure;
        public string StoredSide;
        public readonly Dictionary<int, Color> Colors = new Dictionary<int, Color>();
        /// <summary>보기 기준으로 맞춘 도형(저장 기준과 다르면 좌우 반전한 사본).</summary>
        public List<MapPlacedElementData> Items = new List<MapPlacedElementData>();
        public readonly Dictionary<long, IList<string>> Members = new Dictionary<long, IList<string>>();
    }
}
