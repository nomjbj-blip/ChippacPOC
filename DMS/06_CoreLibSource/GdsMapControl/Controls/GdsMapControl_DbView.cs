using NexplantQMS.GdsMap.Gds;
using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS 파일 없이 DB에 저장된 배치 도형(PLACED)으로 도면을 그리는 경로다.
    /// 설계: 문서/설계/02_GDS/2026-10-02_GDS_Map_조회화면_설계.md 6절. 파일 경로(ShowStructure / Flatten)는 바꾸지 않는다.
    /// </summary>
    public partial class GdsMapControl
    {
        /// <summary>
        /// DB에서 읽은 배치 도형 전체로 현재 도면을 바꾼다. UI 스레드에서 한 번만 호출한다.
        /// 처리 순서 (ShowStructure와 같은 정리 순서):
        ///  1) Map 버전 증가 / 선택 / Layer / Defect / TEXT 라벨 비우기
        ///  2) 이름과 Layer만 가진 빈 GdsStructure를 Structure로 지정한다.
        ///     Layer 표시 / Chain 탐색 / 배치 추출 코드가 Structure != null을 검사하므로 그 코드를 고치지 않고 동작하게 하려는 목적이다.
        ///  3) 도형마다 SceneItem 생성 후 Layer에 추가, TEXT는 라벨 등록
        ///  4) Layer 색상 -> TEXT 라벨 영역 -> GPU 버퍼 1회 생성 -> 전체 보기
        /// layerColors: 저장 당시 화면 Layer 색상(TQP_GDS_LAYER.COLOR_ARGB). 키 = Layer 번호 + DataType(GdsLayerKey, 2026-10-20). 도형이 없는 Layer도 목록에 표시한다.
        /// </summary>
        public void ShowPlacedElements(string topStructure, IDictionary<int, Color> layerColors,
            IList<MapPlacedElementData> elements)
        {
            if (layerColors == null) throw new ArgumentNullException(nameof(layerColors));
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            string structureName = String.IsNullOrEmpty(topStructure) ? "DB" : topStructure;

            _mapContentVersion++;
            _loadMetrics = new GdsMapLoadMetrics();
            ResetFlattenMeasurements();
            _loadFramePending = false;
            ReportRenderProgress("DB 도면 구성 중", 0, true, false, true);

            ClearSelectionInternal();
            _layerList.Clear();
            _defectList.Clear();
            _textLabels.Clear();

            var structure = new GdsStructure { Name = structureName };
            // GdsLayerList는 BinarySearch를 쓰므로 번호 순서로 넣는다. 원본 Layer 는 번호 단위라 같은 번호의 키(DataType 만 다름)는 한 번만 넣는다.
            foreach (int layerKey in layerColors.Keys.OrderBy(id => id))
            {
                int layerNumber = GdsLayerKey.Layer(layerKey);
                if (structure.Layers.Count == 0 || structure.Layers[structure.Layers.Count - 1].LayerID != layerNumber)
                    structure.Layers.Add(new GdsLayer(layerNumber));
                _layerList.AddLayer(layerKey);
                _layerColor[layerKey] = Color.FromArgb(255, layerColors[layerKey]);
            }
            Structure = structure;

            long buildStart = Stopwatch.GetTimestamp();
            foreach (MapPlacedElementData data in elements)
            {
                GlSceneItem item = MapPlacedSceneItemFactory.Create(data);
                AddMeasuredSceneItem(item);
                _loadMetrics.SourcePointCount += data.WorldPoints.Length;
                if (item.Source is GdsBoundary) _loadMetrics.BoundaryCount++;
                else if (item.Source is GdsPath) _loadMetrics.PathCount++;
                else if (item.Source is GdsText text)
                {
                    _loadMetrics.TextCount++;
                    // DB에는 SREF 구조 경로가 없으므로 라벨 진단 경로는 최상위 이름으로 둔다.
                    AddTextLabel(text, data.WorldPoints, structureName);
                }
                // 저장 데이터에 Layer 목록에 없는 번호가 있어도 표시되도록 기본 색상 Layer를 만든다(AddSceneItem이 처리).
            }
            SortMeasuredSceneItems();
            ApplyInitialLayerColors();
            _loadMetrics.FlattenMs = ElapsedMilliseconds(buildStart);
            CompleteFlattenMeasurements();
            CalculateTextLabelDisplayAreas();

            BuildGpuBuffers();
            RaiseSelectionChanged();
            _loadFramePending = true;
            ZoomToFit();
        }

        /// <summary>
        /// 지금 보기(가운데 월드 좌표 / 배율 / 라벨 기준 배율). 도면 하나를 여러 상세보기 창이 나눠 쓸 때
        /// 창마다 보던 위치를 저장했다가 SetView 로 되돌린다 (2026-10-16, 문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md 5.2절).
        /// </summary>
        public void GetView(out double centerX, out double centerY, out double scale, out double labelScale)
        {
            centerX = _offset.X;
            centerY = _offset.Y;
            scale = _scale;
            labelScale = _labelFitScale;
        }

        /// <summary>GetView 로 저장한 보기로 되돌린다. 배율은 허용 범위로 자른다.</summary>
        public void SetView(double centerX, double centerY, double scale, double labelScale)
        {
            if (double.IsNaN(scale) || double.IsInfinity(scale) || scale <= 0) return;
            _scale = Clamp(scale, MinScale, MaxScale);
            _labelFitScale = labelScale > 0 && !double.IsNaN(labelScale) && !double.IsInfinity(labelScale) ? labelScale : _scale;
            _offset = new OpenTK.Vector2((float)centerX, (float)centerY);
            Invalidate();
            ZoomChanged?.Invoke(this, _scale);
        }
    }
}
