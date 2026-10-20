using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return VisitPlacedElements(null, visitor);
        }

        /// <summary>
        /// layerIds에 있는 Layer의 배치 도형만 방문한다. null이면 전체를 방문한다.
        /// 화면 Layer 필터 기준 저장(설계 21절)에서 예상 건수를 셀 때 사용한다.
        /// </summary>
        public int VisitPlacedElements(ISet<int> layerIds, Action<MapPlacedElementData> visitor)
        {
            return VisitPlacedElements(layerIds, false, visitor);
        }

        /// <summary>
        /// layerIds Layer의 배치 도형을 방문한다. mirrorX = true면 Back side 저장용으로 X 좌표를 GDS 원점 기준 좌우 반전(X' = -X)한
        /// 사본을 넘긴다(설계: 문서/설계/02_GDS/2026-10-06_GDS_반전_BackSide_설계.md 8절). 화면 도형과 원본 좌표는 바꾸지 않는다.
        /// </summary>
        public int VisitPlacedElements(ISet<int> layerIds, bool mirrorX, Action<MapPlacedElementData> visitor)
        {
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));
            if (Structure == null) throw new InvalidOperationException("Map을 먼저 조회해야 합니다.");

            int count = 0;
            foreach (GlSceneItem item in ItemsOnLayers(layerIds))
            {
                visitor(CreatePersistenceData(item, mirrorX));
                count++;
            }
            return count;
        }

        /// <summary>
        /// Map을 조금씩 읽어 배치 하나만 복사한 뒤 비동기 소비자에게 넘긴다.
        /// 소비자가 Service 응답을 기다리는 동안 UI 메시지를 처리하고, 도면 교체 시 즉시 중단한다.
        /// </summary>
        public Task<int> VisitPlacedElementBatchesAsync(
            Func<MapPlacedElementBatch, Task> consumeBatchAsync,
            int maxElements = 2000, long maxBytes = 8L * 1024 * 1024,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return VisitPlacedElementBatchesAsync(null, consumeBatchAsync, maxElements, maxBytes, cancellationToken);
        }

        /// <summary>
        /// layerIds에 있는 Layer의 배치 도형만 배치로 넘긴다. null이면 전체를 넘긴다.
        /// 호출자는 저장 시작 시 복사한 Layer 집합을 넘겨 저장 도중 체크가 바뀌어도 예상 건수와 같게 한다.
        /// </summary>
        public Task<int> VisitPlacedElementBatchesAsync(ISet<int> layerIds,
            Func<MapPlacedElementBatch, Task> consumeBatchAsync,
            int maxElements = 2000, long maxBytes = 8L * 1024 * 1024,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return VisitPlacedElementBatchesAsync(layerIds, false, consumeBatchAsync, maxElements, maxBytes, cancellationToken);
        }

        /// <summary>
        /// layerIds Layer의 배치 도형을 배치로 넘긴다. mirrorX = true면 Back side 저장용 좌우 반전 사본(X' = -X)을 넘긴다.
        /// 예상 건수 / 좌표 수는 반전과 무관하다(도형 수 / 점 수 동일).
        /// </summary>
        public async Task<int> VisitPlacedElementBatchesAsync(ISet<int> layerIds, bool mirrorX,
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
            foreach (GlSceneItem item in ItemsOnLayers(layerIds))
            {
                cancellationToken.ThrowIfCancellationRequested();
                EnsureMapVersion(version);
                MapPlacedElementData data = CreatePersistenceData(item, mirrorX);
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

        /// <summary>
        /// 배치 도형 ID별 Layer 키(번호 + DataType, GdsLayerKey)를 돌려준다. 현재 도면에 없는 ID는 결과에 넣지 않는다 (2026-10-20 키로 변경).
        /// Chain DB 저장 전 검사(Member / 편집 도형이 DB Map에 저장된 Layer 안인지, 지금 도면에 있는지)에 사용한다.
        /// Chain 탐색 캐시는 탐색한 도형만 가지므로 전체 화면 도형에서 찾는다.
        /// </summary>
        public Dictionary<string, int> GetPlacedElementLayers(IEnumerable<string> placedElementIds)
        {
            var wanted = new HashSet<string>(placedElementIds, StringComparer.Ordinal);
            var result = new Dictionary<string, int>(StringComparer.Ordinal);
            if (wanted.Count == 0) return result;
            foreach (GlSceneItem item in AllItems)
            {
                if (item.PlacedElementId == null || !wanted.Contains(item.PlacedElementId)) continue;
                result[item.PlacedElementId] = item.LayerKey;
                if (result.Count == wanted.Count) break;
            }
            return result;
        }

        /// <summary>Layer 키(번호 + DataType) 조건에 맞는 화면 도형. 조건이 null이면 전체다.</summary>
        private IEnumerable<GlSceneItem> ItemsOnLayers(ISet<int> layerIds)
        {
            return layerIds == null ? AllItems
                : _layerList.Where(layer => layerIds.Contains(layer.LayerKey)).SelectMany(layer => layer.Items);
        }

        /// <summary>도면을 다시 열었다면 이전 개정본의 배치를 계속 보내지 않게 한다.</summary>
        private void EnsureMapVersion(int version)
        {
            if (_mapContentVersion != version)
                throw new InvalidOperationException("배치 추출 중 Map이 바뀌었습니다. 새 도면에서 다시 시작하세요.");
        }

        /// <summary>
        /// 동기 방문과 비동기 배치가 같은 필드를 추출하도록 한 곳에서 모델을 만든다.
        /// mirrorX: Back side 저장용 좌우 반전. 좌표는 새 배열에 X 부호만 바꿔 넣는다(GPoint 생성자의 소수 5자리 반올림을 다시 거치지 않아
        /// 두 번 반전하면 비트 단위로 원본과 같다). 화면 도형의 WorldPoints는 원본 GDS 배열을 공유할 수 있으므로 절대 직접 바꾸지 않는다.
        /// Bounds는 (-MaxX, MinY, -MinX, MaxY). TEXT는 위치(WorldPoints[0])만 반전하고 정렬 / 글자 모양은 그대로 둔다.
        /// </summary>
        private static MapPlacedElementData CreatePersistenceData(GlSceneItem item, bool mirrorX)
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
                // DB DATA_TYPE = 화면 Layer 키의 DataType(TEXT 는 TEXTTYPE, KLayout 과 같음) (2026-10-20)
                DataType = GdsLayerKey.DataType(item.LayerKey),
                ElementType = item.Source is GdsBoundary ? "BOUNDARY"
                    : item.Source is GdsPath ? "PATH" : "TEXT",
                Closed = item.Closed,
                PathWidth = item.Width,
                PathType = (item.Source as GdsPath)?.PathType ?? 0,
                Bounds = mirrorX ? MirrorBounds(item.WorldBounds) : item.WorldBounds,
                Text = item.Text,
                TextType = text == null ? (int?)null : text.TextType,
                TextFont = text == null ? (int?)null : text.FontNumber,
                TextHorizontal = text == null ? null : text.HorizontalPresentation.ToString(),
                TextVertical = text == null ? null : text.VerticalPresentation.ToString(),
                WorldPoints = mirrorX ? MirrorPoints(item.WorldPoints) : item.WorldPoints
            };
        }

        /// <summary>GDS 원점 기준 좌우 반전(X' = -X) 사본. 변환식은 GdsSideTransform 한 곳에 둔다.</summary>
        internal static GPoint[] MirrorPoints(GPoint[] points)
        {
            return GdsSideTransform.MirrorPoints(points);
        }

        /// <summary>좌우 반전한 영역: X 범위가 (-MaxX ~ -MinX)로 바뀐다.</summary>
        internal static GBox MirrorBounds(GBox bounds)
        {
            return GdsSideTransform.MirrorBounds(bounds);
        }
    }
}
