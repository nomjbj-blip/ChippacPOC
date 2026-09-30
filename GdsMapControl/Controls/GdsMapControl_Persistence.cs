using NexplantQMS.GdsMap.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 현재 Map의 배치 도형을 DB 저장 계층에 한 건씩 전달한다.
    /// 대형 GDS의 좌표 배열 전체를 다시 복사하지 않기 위해 동기 방문 방식으로 분리한다.
    /// </summary>
    public partial class GdsMapControl
    {
        private int _mapContentVersion;

        /// <summary>
        /// 화면을 이루는 모든 Boundary/Path/Text를 Layer 표시 여부와 무관하게 방문한다.
        /// Chain에 포함되지 않은 도형도 저장해야 DB만으로 Map을 다시 그릴 수 있다.
        /// 호출자는 UI 스레드에서 실행하고 콜백 안에서 각 건의 좌표를 즉시 직렬화한다.
        /// </summary>
        public int VisitPlacedElements(Action<MapPlacedElementData> visitor)
        {
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));
            if (Structure == null) throw new InvalidOperationException("Map을 먼저 조회해야 합니다.");

            int count = 0;
            foreach (GlSceneItem item in AllItems)
            {
                visitor(CreatePersistenceData(item));
                count++;
            }
            return count;
        }

        /// <summary>
        /// Map을 조금씩 읽어 배치 하나만 복사한 뒤 비동기 소비자에게 넘긴다.
        /// 소비자가 Service 응답을 기다리는 동안 UI 메시지를 처리하고, 도면 교체 시 즉시 중단한다.
        /// </summary>
        public async Task<int> VisitPlacedElementBatchesAsync(
            Func<MapPlacedElementBatch, Task> consumeBatchAsync,
            int maxElements = 2000, long maxBytes = 8L * 1024 * 1024,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (consumeBatchAsync == null) throw new ArgumentNullException(nameof(consumeBatchAsync));
            if (InvokeRequired) throw new InvalidOperationException("Map 배치 추출은 UI 스레드에서 시작해야 합니다.");
            if (Structure == null) throw new InvalidOperationException("Map을 먼저 조회해야 합니다.");

            int version = _mapContentVersion;
            var builder = new MapPlacedElementBatchBuilder(maxElements, maxBytes);
            int batchNo = 0;
            int total = 0;
            foreach (GlSceneItem item in AllItems)
            {
                cancellationToken.ThrowIfCancellationRequested();
                EnsureMapVersion(version);
                MapPlacedElementData data = CreatePersistenceData(item);
                if (!builder.TryAdd(data))
                {
                    await consumeBatchAsync(builder.TakeBatch(batchNo++));
                    await Task.Yield();
                    cancellationToken.ThrowIfCancellationRequested();
                    EnsureMapVersion(version);
                    if (!builder.TryAdd(data))
                        throw new InvalidOperationException("빈 배치에 도형을 추가할 수 없습니다.");
                }
                total++;
            }
            if (!builder.IsEmpty)
            {
                cancellationToken.ThrowIfCancellationRequested();
                EnsureMapVersion(version);
                await consumeBatchAsync(builder.TakeBatch(batchNo));
                await Task.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                EnsureMapVersion(version);
            }
            return total;
        }

        /// <summary>도면을 다시 열었다면 이전 개정본의 배치를 계속 보내지 않게 한다.</summary>
        private void EnsureMapVersion(int version)
        {
            if (_mapContentVersion != version)
                throw new InvalidOperationException("배치 추출 중 Map이 바뀌었습니다. 새 도면에서 다시 시작하세요.");
        }

        /// <summary>동기 방문과 비동기 배치가 같은 필드를 추출하도록 한 곳에서 모델을 만든다.</summary>
        private static MapPlacedElementData CreatePersistenceData(GlSceneItem item)
        {
            if (String.IsNullOrEmpty(item.PlacedElementId)
                || String.IsNullOrEmpty(item.SourceElementId))
                throw new InvalidOperationException("배치 Element ID가 없는 도형이 있습니다.");
            if (item.PlacedElementId.Length > 1024 || item.SourceElementId.Length > 512)
                throw new InvalidOperationException("DB 컬럼 길이를 넘는 Element ID가 있습니다.");
            if (item.WorldPoints == null || item.WorldPoints.Length == 0)
                throw new InvalidOperationException("좌표가 없는 배치 도형은 저장할 수 없습니다.");

            var text = item.Source as GdsText;
            return new MapPlacedElementData
            {
                PlacedElementId = item.PlacedElementId,
                SourceElementId = item.SourceElementId,
                LayerId = item.LayerID,
                DataType = item.Source.DataType,
                ElementType = item.Source is GdsBoundary ? "BOUNDARY"
                    : item.Source is GdsPath ? "PATH" : "TEXT",
                Closed = item.Closed,
                PathWidth = item.Width,
                Bounds = item.WorldBounds,
                Text = item.Text,
                TextType = text == null ? (int?)null : text.TextType,
                TextFont = text == null ? (int?)null : text.FontNumber,
                TextHorizontal = text == null ? null : text.HorizontalPresentation.ToString(),
                TextVertical = text == null ? null : text.VerticalPresentation.ToString(),
                WorldPoints = item.WorldPoints
            };
        }
    }
}
