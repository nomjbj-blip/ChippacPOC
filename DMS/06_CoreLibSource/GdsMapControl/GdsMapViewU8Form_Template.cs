using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View U8 예시 지정 (2026-10-20, 설계 5절 1번).
    /// [예시 지정] 후 도형 하나를 클릭하면 체크된 Layer 에서 그 도형과 닿는 도형 덩어리 전체를 예시로 정한다.
    /// U9 처럼 Input / Output / 경로를 고르지 않고 "덩어리 한 번 클릭" 이다. 예시는 자주색 외곽선으로 보인다.
    ///
    /// 처리 흐름
    ///  [조회] 끝 -> 화면 도형(TEXT 제외)으로 도형 색인(_elementByKey)
    ///  클릭 -> 클릭점의 도형 후보(GetChainElementCandidates, 체크된 Layer) -> 덩어리 계산기(체크된 Layer, 처음 한 번 만들고 재사용)
    ///       -> ExtractFrom -> Label 붙이기 -> 예시 형태(ChainShapeTemplate) + 이름 미리보기
    /// </summary>
    public partial class GdsMapViewU8Form
    {
        /// <summary>끌기(화면 이동)와 클릭을 나누는 거리(픽셀).</summary>
        private const int ClickSlop = 4;

        /// <summary>화면 도형(TEXT 제외, 화면 좌표). 덩어리 계산 / 강조 / 저장된 Chain 표시에 쓴다.</summary>
        private Dictionary<string, ChainTraceElement> _elementByKey = new Dictionary<string, ChainTraceElement>(StringComparer.Ordinal);
        /// <summary>도형 색인이 바뀔 때마다 증가. 늦게 끝난 계산을 버릴 때 비교한다.</summary>
        private int _elementGeneration;
        private ChainComponentExtractor _extractor;
        private string _extractorLayers;

        private bool _templateSelecting;
        private ChainShapeTemplate _template;
        private ChainComponent _templateComponent;
        /// <summary>예시를 지정할 때 체크된 도형 Layer. 같은 형태 찾기 / Chain 탐색 Layer 로 쓴다.</summary>
        private List<int> _templateLayers = new List<int>();
        private Point _mouseDownPoint;

        private void InitializeTemplateState()
        {
            lblTemplate.Text = "예시: 없음";
        }

        /// <summary>[조회] / 보기 기준 전환 뒤 화면 도형으로 도형 색인을 다시 만든다.</summary>
        private void BuildElementIndex()
        {
            _elementGeneration++;
            _extractor = null;
            _extractorLayers = null;
            var index = new Dictionary<string, ChainTraceElement>(StringComparer.Ordinal);
            if (_shownItems != null)
                foreach (MapPlacedElementData item in _shownItems)
                {
                    if (string.Equals(item.ElementType, "TEXT", StringComparison.OrdinalIgnoreCase)) continue;
                    if (item.WorldPoints == null || item.WorldPoints.Length == 0 || string.IsNullOrEmpty(item.PlacedElementId)) continue;
                    index[item.PlacedElementId] = new ChainTraceElement(item.PlacedElementId, item.LayerId, item.Bounds,
                        item.ElementType, item.WorldPoints, item.PathWidth, item.DataType);
                }
            _elementByKey = index;
            UpdateU8Buttons();
        }

        private void ClearElementIndex()
        {
            _elementGeneration++;
            _extractor = null;
            _extractorLayers = null;
            _elementByKey = new Dictionary<string, ChainTraceElement>(StringComparer.Ordinal);
        }

        /// <summary>
        /// 체크된 Layer 의 덩어리 계산기. Layer 조합이 같으면 다시 만들지 않는다(U8 33만 도형 격자 구성 약 1초).
        /// 만드는 동안 도면이 바뀌면 null.
        /// </summary>
        private async Task<ChainComponentExtractor> GetExtractorAsync(IList<int> layers)
        {
            string key = string.Join(",", layers.OrderBy(l => l));
            if (_extractor != null && _extractorLayers == key) return _extractor;
            int generation = _elementGeneration;
            var layerSet = new HashSet<int>(layers);
            List<ChainTraceElement> elements = _elementByKey.Values.Where(e => layerSet.Contains(e.LayerKey)).ToList();
            ChainComponentExtractor extractor;
            using (ViewBusy.Busy())
                extractor = await Task.Run(() => new ChainComponentExtractor(elements, ChainComponentExtractor.DefaultCellSize));
            if (generation != _elementGeneration) return null;
            _extractor = extractor;
            _extractorLayers = key;
            return extractor;
        }

        /// <summary>체크되어 있고 도면에 올린 Layer 중 도형(TEXT 제외)이 있는 Layer.</summary>
        private List<int> CheckedShapeLayers()
        {
            HashSet<int> checkedLayers = CheckedLoadedLayers();
            var shapeLayers = new HashSet<int>(_elementByKey.Values.Select(e => e.LayerKey));
            return checkedLayers.Where(shapeLayers.Contains).OrderBy(l => l).ToList();
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            SetTemplateSelecting(!_templateSelecting);
            if (!_templateSelecting) SetStatus("예시 지정을 마쳤습니다.");
        }

        /// <summary>예시 지정(클릭) 모드 켜기 / 끄기. 켜면 지도 커서가 십자가 된다.</summary>
        private void SetTemplateSelecting(bool selecting)
        {
            _templateSelecting = selecting && _shownMap != null;
            map.Cursor = _templateSelecting ? Cursors.Cross : Cursors.SizeAll;
            if (_templateSelecting)
            {
                map.Mode = GdsMapControl.ViewMode.View;
                SetStatus("예시 지정: 같은 형태로 찾을 묶음의 도형 하나를 클릭하세요. 체크된 Layer 의 닿는 도형이 모두 예시가 됩니다. (Esc 끝)");
            }
            UpdateU8Buttons();
        }

        private void map_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPoint = e.Location;
        }

        /// <summary>예시 지정 중 왼쪽 클릭(끌기 아님)이면 그 위치의 덩어리를 예시로 정한다.</summary>
        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !_templateSelecting) return;
            if (Math.Abs(e.X - _mouseDownPoint.X) > ClickSlop || Math.Abs(e.Y - _mouseDownPoint.Y) > ClickSlop) return;
            SelectTemplateAt(e.Location);
        }

        /// <summary>
        /// 클릭한 도형의 덩어리를 예시로 정한다. 클릭점의 도형이 여러 개여도 서로 겹쳐 있어 같은 덩어리이므로 가장 작은 도형에서 시작한다.
        /// </summary>
        private async void SelectTemplateAt(Point screenPoint)
        {
            List<int> layers = CheckedShapeLayers();
            if (layers.Count == 0)
            {
                SetStatus("체크된 도형 Layer 가 없습니다. Layer 탭에서 찾을 도형 Layer 를 체크하세요.");
                return;
            }
            var layerSet = new HashSet<int>(layers);
            ChainElementCandidate candidate = map.GetChainElementCandidates(screenPoint).FirstOrDefault(c => layerSet.Contains(c.LayerKey));
            if (candidate == null)
            {
                SetStatus("클릭한 위치에 체크된 Layer 의 도형이 없습니다.");
                return;
            }
            try
            {
                ChainComponentExtractor extractor = await GetExtractorAsync(layers);
                if (extractor == null || !_templateSelecting) return;
                ChainComponent component = extractor.ExtractFrom(candidate.ElementKey);
                if (component == null)
                {
                    SetStatus("클릭한 도형을 찾지 못했습니다. 다시 [조회] 하세요.");
                    return;
                }
                extractor.AttachLabels(new[] { component }, NamingLabels(_rule));
                ClearExtractResults();
                _templateComponent = component;
                _template = new ChainShapeTemplate(component.Elements);
                _templateLayers = layers;
                map.SetChainExampleElements(component.Elements);
                string name = _rule.NameOf(component);
                lblTemplate.Text = "예시: " + (name ?? "Label 없음") + " / 도형 " + component.Elements.Count.ToString("N0");
                toolTipView.SetToolTip(lblTemplate, "구성: " + _template.Description
                    + "\r\nLayer: " + string.Join(", ", layers.Select(GdsLayerKey.Format))
                    + "\r\nLabel: " + (component.Labels.Count == 0 ? "없음" : string.Join(", ", component.Labels.Select(l => l.Text))));
                SetTemplateSelecting(false);
                int numberLabels = _rule.NumberLabelCount(component);
                SetStatus("예시: " + (name ?? "Label 없음") + " / 도형 " + component.Elements.Count.ToString("N0")
                    + " (Layer " + string.Join(", ", layers.Select(GdsLayerKey.Format)) + ")"
                    + (numberLabels == 1 ? " - [같은 형태 찾기]를 누르세요."
                        : numberLabels == 0 ? " - 번호 Label(" + GdsLayerKey.Format(_rule.NumberLabelLayerKey) + ")이 없는 묶음입니다. Chain Code 를 만들 수 없습니다."
                        : " - 번호 Label 이 " + numberLabels + "개입니다. 이웃 묶음과 붙어 있는지 확인하세요."));
            }
            catch (Exception ex)
            {
                ShowError("예시 지정", ex);
            }
        }

        /// <summary>예시를 지운다(도면 / Device / 보기 기준이 바뀔 때).</summary>
        private void ClearTemplate()
        {
            _template = null;
            _templateComponent = null;
            _templateLayers = new List<int>();
            _templateSelecting = false;
            map.Cursor = Cursors.SizeAll;
            if (map.Structure != null) map.SetChainExampleElements(null);
            lblTemplate.Text = "예시: 없음";
            toolTipView.SetToolTip(lblTemplate, string.Empty);
            UpdateU8Buttons();
        }
    }
}
