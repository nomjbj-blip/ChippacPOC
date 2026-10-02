using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using NexplantQMS.GdsMap.Chain;

namespace NexplantQMS.GdsMap
{
    /// <summary>Visible 체크된 다른 Chain의 지도 외곽선과 색을 전달하는 화면 전용 값이다.</summary>
    public sealed class ChainVisibleOverlay
    {
        public ChainVisibleOverlay(string code, Color color, IEnumerable<ChainTraceElement> elements)
        {
            Code = code;
            Color = color;
            Elements = elements.ToList().AsReadOnly();
        }

        public string Code { get; private set; }
        public Color Color { get; private set; }
        public IList<ChainTraceElement> Elements { get; private set; }
    }

    /// <summary>
    /// Chain 외곽선은 월드 좌표 GPU 버퍼에, 단자 Marker와 작업 영역은 화면 좌표 GDI+에 표시한다.
    /// 화면 이동/Zoom 중에는 Chain 정점 버퍼를 다시 만들지 않고 Map의 투영 행렬을 함께 쓴다.
    /// </summary>
    public partial class GdsMapControl
    {
        private GPoint? _chainOverlayInputPoint;
        private GPoint? _chainOverlayOutputPoint;
        private IList<GPoint> _chainOverlayInputPoints = new List<GPoint>();
        private IList<GPoint> _chainOverlayOutputPoints = new List<GPoint>();
        private IList<ChainTraceElement> _chainOverlayOverlaps = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainOverlayBranches = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainExampleElements = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainSimilarReady = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainSimilarProtected = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainSimilarAmbiguous = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainSimilarSelected = new List<ChainTraceElement>();
        private IList<ChainVisibleOverlay> _visibleChainOverlays = new List<ChainVisibleOverlay>();
        private readonly List<ChainOverlayVertex> _chainOverlayVertices = new List<ChainOverlayVertex>();
        private readonly List<ChainOverlayDrawRange> _chainOverlayDrawRanges = new List<ChainOverlayDrawRange>();
        private int _chainOverlayVao;
        private int _chainOverlayVbo;
        private int _chainOverlayShaderProgram;
        private int _chainOverlayMatrixLoc;
        private int _chainOverlayScaleLoc;
        private int _chainOverlayDashedLoc;
        private bool _chainOverlayGpuDirty;
        private GPoint? _chainOverlayWorkAreaFirstPoint;
        private GPoint? _chainOverlayWorkAreaSecondPoint;

        /// <summary>Map과 같은 정점 배열 배치를 쓰되 마지막 값에 분기 점선의 선분 내 거리를 보관한다.</summary>
        private struct ChainOverlayVertex
        {
            public Vector2 Position;
            public Vector4 Color;
            public float Distance;

            public ChainOverlayVertex(GPoint point, Color color, float distance)
            {
                Position = new Vector2((float)point.X, (float)point.Y);
                Color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                Distance = distance;
            }
        }

        /// <summary>같은 Layer의 Chain 선분을 한 번의 OpenGL 호출로 그리기 위한 버퍼 구간이다.</summary>
        private sealed class ChainOverlayDrawRange
        {
            public int LayerId;
            public int Start;
            public int Count;
            public PrimitiveType Primitive;
            public bool Dashed;
            public bool RespectLayerVisibility;
        }

        // 정점의 마지막 float는 선분 시작점부터의 월드 거리다. 분기 점선을 픽셀 길이로 유지한다.
        private const string ChainOverlayVertexShaderCode = @"
            #version 330 core
            layout(location = 0) in vec2 aPos;
            layout(location = 1) in vec4 aColor;
            layout(location = 2) in float aDistance;
            uniform mat4 uMatrix;
            out vec4 vColor;
            out float vDistance;
            void main() {
                gl_Position = uMatrix * vec4(aPos, 0.0, 1.0);
                vColor = aColor;
                vDistance = aDistance;
            }
        ";

        private const string ChainOverlayFragmentShaderCode = @"
            #version 330 core
            in vec4 vColor;
            in float vDistance;
            uniform float uScale;
            uniform int uDashed;
            out vec4 FragColor;
            void main() {
                if (uDashed == 1 && mod(vDistance * uScale, 12.0) >= 7.0)
                    discard;
                FragColor = vColor;
            }
        ";

        /// <summary>Input 월드 좌표를 저장하고 이전 후보 경로를 해제한 뒤 화면을 다시 그린다.</summary>
        public void SetChainInputPoint(GPoint point)
        {
            _chainOverlayInputPoint = point;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>Output 월드 좌표를 저장하고 이전 후보 경로를 해제한 뒤 화면을 다시 그린다.</summary>
        public void SetChainOutputPoint(GPoint point)
        {
            _chainOverlayOutputPoint = point;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>
        /// 마우스로 고른 여러 Input/Output의 표시 위치를 한 번에 갱신한다.
        /// 선택 중에는 확정 경로 데이터를 지우지 않고 표시만 바꾼다.
        /// </summary>
        public void SetChainEndpointPoints(IEnumerable<GPoint> inputPoints, IEnumerable<GPoint> outputPoints)
        {
            _chainOverlayInputPoint = null;
            _chainOverlayOutputPoint = null;
            _chainOverlayInputPoints = (inputPoints ?? Enumerable.Empty<GPoint>()).ToList();
            _chainOverlayOutputPoints = (outputPoints ?? Enumerable.Empty<GPoint>()).ToList();
            Invalidate();
        }

        /// <summary>경로와 직접 겹친 도형 및 그중 분기 검토 대상을 지도 외곽선 표시용으로 보관한다.</summary>
        private void SetChainCandidateOverlay(ChainTraceResult result, ISet<string> includedKeys)
        {
            _chainOverlayOverlaps = result.OverlappingElements
                .Where(element => includedKeys.Contains(element.ElementKey)).ToList();
            _chainOverlayBranches = result.BranchCandidates
                .Where(element => includedKeys.Contains(element.ElementKey)).ToList();
            BuildChainOverlayVertices();
        }

        /// <summary>단자나 Layer 조건 변경 시 이전 후보의 구분 외곽선을 제거한다.</summary>
        private void ClearChainCandidateOverlay()
        {
            if (_chainOverlayOverlaps.Count == 0 && _chainOverlayBranches.Count == 0)
                return;
            _chainOverlayOverlaps = new List<ChainTraceElement>();
            _chainOverlayBranches = new List<ChainTraceElement>();
            BuildChainOverlayVertices();
        }

        /// <summary>Visible Chain이 변경될 때만 화면 외곽선 정점을 다시 구성하고 GPU 업로드를 예약한다.</summary>
        public void SetVisibleChainOverlays(IEnumerable<ChainVisibleOverlay> overlays)
        {
            _visibleChainOverlays = (overlays ?? Enumerable.Empty<ChainVisibleOverlay>()).ToList();
            BuildChainOverlayVertices();
            Invalidate();
        }

        /// <summary>현재 Chain에서 엔지니어가 고른 예시 도형만 자주색 외곽선으로 강조한다.</summary>
        public void SetChainExampleElements(IEnumerable<ChainTraceElement> elements)
        {
            _chainExampleElements = (elements ?? Enumerable.Empty<ChainTraceElement>()).ToList();
            BuildChainOverlayVertices();
            Invalidate();
        }

        /// <summary>현재 Chain 안에서 찾은 묶음을 일반/단자 보호/검토 필요 색으로 미리 보여 준다.</summary>
        public void SetChainSimilarPreview(IEnumerable<ChainTraceElement> ready,
            IEnumerable<ChainTraceElement> protectedElements,
            IEnumerable<ChainTraceElement> ambiguous)
        {
            _chainSimilarReady = (ready ?? Enumerable.Empty<ChainTraceElement>()).ToList();
            _chainSimilarProtected = (protectedElements ?? Enumerable.Empty<ChainTraceElement>()).ToList();
            _chainSimilarAmbiguous = (ambiguous ?? Enumerable.Empty<ChainTraceElement>()).ToList();
            BuildChainOverlayVertices();
            Invalidate();
        }

        /// <summary>결과 목록의 현재 행을 흰색 외곽선으로 표시해 다른 검색 결과와 구별한다.</summary>
        public void SetChainSimilarSelected(IEnumerable<ChainTraceElement> elements)
        {
            _chainSimilarSelected = (elements ?? Enumerable.Empty<ChainTraceElement>()).ToList();
            BuildChainOverlayVertices();
            Invalidate();
        }

        /// <summary>
        /// Map과 동일한 월드 좌표를 Chain 색 선분으로 바꾼다.
        /// Chain/Layer별로 모아 두어 화면 이동 시 Element마다 DrawArrays를 호출하지 않는다.
        /// </summary>
        private void BuildChainOverlayVertices()
        {
            _chainOverlayVertices.Clear();
            _chainOverlayDrawRanges.Clear();
            foreach (ChainVisibleOverlay chain in _visibleChainOverlays)
                AppendChainOverlayElements(chain.Elements, chain.Color, false, true);
            // 겹침/분기는 현재 후보의 상태 표시이므로 Chain Visible과 독립적으로 그린다.
            AppendChainOverlayElements(_chainOverlayOverlaps, Color.DeepSkyBlue, false, false);
            AppendChainOverlayElements(_chainOverlayBranches, Color.OrangeRed, true, false);
            AppendChainOverlayElements(_chainSimilarReady, Color.LimeGreen, false, true);
            AppendChainOverlayElements(_chainSimilarProtected, Color.Orange, false, true);
            AppendChainOverlayElements(_chainSimilarAmbiguous, Color.Red, false, true);
            AppendChainOverlayElements(_chainSimilarSelected, Color.White, false, true);
            AppendChainOverlayElements(_chainExampleElements, Color.Magenta, false, true);
            _chainOverlayGpuDirty = true;
        }

        /// <summary>후보/Visible Chain 도형을 공통 선분 형식으로 만들고 Layer별 호출 범위를 기록한다.</summary>
        private void AppendChainOverlayElements(IEnumerable<ChainTraceElement> elements, Color color,
            bool dashed, bool respectLayerVisibility)
        {
            foreach (var layerGroup in elements.GroupBy(element => element.LayerId))
            {
                int lineStart = _chainOverlayVertices.Count;
                foreach (ChainTraceElement element in layerGroup)
                {
                    GPoint[] points = element.WorldPoints;
                    if (points == null || points.Length < 2) continue;
                    bool closed = !string.Equals(element.ElementType, "PATH", StringComparison.OrdinalIgnoreCase)
                        && points.Length >= 3;
                    int segmentCount = closed ? points.Length : points.Length - 1;
                    for (int i = 0; i < segmentCount; i++)
                    {
                        GPoint first = points[i];
                        GPoint second = points[(i + 1) % points.Length];
                        double dx = second.X - first.X;
                        double dy = second.Y - first.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                        _chainOverlayVertices.Add(new ChainOverlayVertex(first, color, 0f));
                        _chainOverlayVertices.Add(new ChainOverlayVertex(second, color, distance));
                    }
                }
                AddChainOverlayRange(layerGroup.Key, lineStart, PrimitiveType.Lines,
                    dashed, respectLayerVisibility);

                int pointStart = _chainOverlayVertices.Count;
                foreach (ChainTraceElement element in layerGroup)
                {
                    GPoint[] points = element.WorldPoints;
                    if (points == null || points.Length != 1) continue;
                    _chainOverlayVertices.Add(new ChainOverlayVertex(points[0], color, 0f));
                }
                AddChainOverlayRange(layerGroup.Key, pointStart, PrimitiveType.Points,
                    false, respectLayerVisibility);
            }
        }

        /// <summary>정점이 있는 Chain/Layer 구간만 그리기 목록에 추가한다.</summary>
        private void AddChainOverlayRange(int layerId, int start, PrimitiveType primitive,
            bool dashed, bool respectLayerVisibility)
        {
            int count = _chainOverlayVertices.Count - start;
            if (count > 0)
                _chainOverlayDrawRanges.Add(new ChainOverlayDrawRange
                {
                    LayerId = layerId, Start = start, Count = count, Primitive = primitive,
                    Dashed = dashed, RespectLayerVisibility = respectLayerVisibility
                });
        }

        /// <summary>Map의 좌표 변환을 받는 Chain 전용 셰이더와 정점 배열을 준비한다.</summary>
        private void InitializeChainOverlayGlResources()
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, ChainOverlayVertexShaderCode);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int shaderStatus);
            if (shaderStatus == 0)
                throw new InvalidOperationException("Chain Vertex Shader 오류: " + GL.GetShaderInfoLog(vertexShader));
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, ChainOverlayFragmentShaderCode);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out shaderStatus);
            if (shaderStatus == 0)
                throw new InvalidOperationException("Chain Fragment Shader 오류: " + GL.GetShaderInfoLog(fragmentShader));
            _chainOverlayShaderProgram = GL.CreateProgram();
            GL.AttachShader(_chainOverlayShaderProgram, vertexShader);
            GL.AttachShader(_chainOverlayShaderProgram, fragmentShader);
            GL.LinkProgram(_chainOverlayShaderProgram);
            GL.GetProgram(_chainOverlayShaderProgram, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus == 0)
                throw new InvalidOperationException("Chain Shader 연결 오류: " + GL.GetProgramInfoLog(_chainOverlayShaderProgram));
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            _chainOverlayMatrixLoc = GL.GetUniformLocation(_chainOverlayShaderProgram, "uMatrix");
            _chainOverlayScaleLoc = GL.GetUniformLocation(_chainOverlayShaderProgram, "uScale");
            _chainOverlayDashedLoc = GL.GetUniformLocation(_chainOverlayShaderProgram, "uDashed");
            _chainOverlayVao = GL.GenVertexArray();
            _chainOverlayVbo = GL.GenBuffer();
            GL.BindVertexArray(_chainOverlayVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _chainOverlayVbo);
            int stride = sizeof(float) * 7;
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, stride, sizeof(float) * 6);
            GL.EnableVertexAttribArray(2);
            _chainOverlayGpuDirty = true;
        }

        /// <summary>변경된 도형만 업로드하고 Chain/후보를 Map 투영 행렬로 한 번에 그린다.</summary>
        private void DrawChainElementOverlaysGl(ref Matrix4 projection)
        {
            if (_chainOverlayGpuDirty)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _chainOverlayVbo);
                ChainOverlayVertex[] vertices = _chainOverlayVertices.ToArray();
                if (vertices.Length == 0)
                    GL.BufferData(BufferTarget.ArrayBuffer, IntPtr.Zero, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                else
                    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 7,
                        vertices, BufferUsageHint.DynamicDraw);
                _chainOverlayGpuDirty = false;
            }
            if (_chainOverlayDrawRanges.Count == 0) return;

            var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible).Select(layer => layer.LayerID));
            GL.UseProgram(_chainOverlayShaderProgram);
            GL.UniformMatrix4(_chainOverlayMatrixLoc, false, ref projection);
            GL.Uniform1(_chainOverlayScaleLoc, (float)_scale);
            GL.BindVertexArray(_chainOverlayVao);
            GL.LineWidth(2.5f);
            GL.PointSize(6f);
            foreach (ChainOverlayDrawRange range in _chainOverlayDrawRanges)
                if (!range.RespectLayerVisibility || visibleLayers.Contains(range.LayerId))
                {
                    GL.Uniform1(_chainOverlayDashedLoc, range.Dashed ? 1 : 0);
                    GL.DrawArrays(range.Primitive, range.Start, range.Count);
                }
            GL.LineWidth(1f);
            GL.PointSize(1f);
        }

        /// <summary>Map Control이 종료될 때 Chain 전용 GPU 자원도 같은 OpenGL 컨텍스트에서 해제한다.</summary>
        private void ReleaseChainOverlayGlResources()
        {
            if (_chainOverlayVbo != 0) GL.DeleteBuffer(_chainOverlayVbo);
            if (_chainOverlayVao != 0) GL.DeleteVertexArray(_chainOverlayVao);
            if (_chainOverlayShaderProgram != 0) GL.DeleteProgram(_chainOverlayShaderProgram);
            _chainOverlayVbo = 0;
            _chainOverlayVao = 0;
            _chainOverlayShaderProgram = 0;
        }

        /// <summary>
        /// 작업 영역의 두 모서리를 저장한다. 두 번째 모서리가 없으면 첫 모서리 위치만 표시한다.
        /// 영역이 바뀌면 기존 후보 경로는 더 이상 유효하지 않으므로 강조를 해제한다.
        /// </summary>
        public void SetChainWorkAreaPoints(GPoint? firstPoint, GPoint? secondPoint)
        {
            _chainOverlayWorkAreaFirstPoint = firstPoint;
            _chainOverlayWorkAreaSecondPoint = secondPoint;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>새 GDS 조회 시 이전 도면에 설정한 Chain Overlay와 후보 경로를 모두 제거한다.</summary>
        public void ResetChainSetupOverlay()
        {
            _lastChainTraceItems.Clear();
            _visibleChainOverlays.Clear();
            _chainExampleElements.Clear();
            _chainSimilarReady.Clear();
            _chainSimilarProtected.Clear();
            _chainSimilarAmbiguous.Clear();
            _chainSimilarSelected.Clear();
            _chainOverlayVertices.Clear();
            _chainOverlayDrawRanges.Clear();
            _chainOverlayGpuDirty = true;
            _chainOverlayInputPoint = null;
            _chainOverlayOutputPoint = null;
            _chainOverlayInputPoints.Clear();
            _chainOverlayOutputPoints.Clear();
            _chainOverlayWorkAreaFirstPoint = null;
            _chainOverlayWorkAreaSecondPoint = null;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>
        /// OpenGL 도형과 GDS TEXT가 그려진 뒤 Chain 설정 상태를 가장 위에 표시한다.
        /// Marker는 확대 배율과 관계없이 일정한 픽셀 크기로 표시하여 축소 화면에서도 찾을 수 있게 한다.
        /// </summary>
        private void DrawChainSetupOverlay()
        {
            if (!_chainOverlayInputPoint.HasValue
                && !_chainOverlayOutputPoint.HasValue
                && _chainOverlayInputPoints.Count == 0
                && _chainOverlayOutputPoints.Count == 0
                && !_chainOverlayWorkAreaFirstPoint.HasValue)
                return;

            using (Graphics graphics = CreateGraphics())
            using (var markerFont = new Font("Segoe UI", 9f, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                DrawChainWorkArea(graphics);
                // Element 외곽선은 모두 SwapBuffers 전에 OpenGL로 그린다.
                if (_chainOverlayInputPoint.HasValue)
                    DrawChainPointMarker(graphics, _chainOverlayInputPoint.Value, "IN", Color.LimeGreen, markerFont);
                if (_chainOverlayOutputPoint.HasValue)
                    DrawChainPointMarker(graphics, _chainOverlayOutputPoint.Value, "OUT", Color.OrangeRed, markerFont);
                foreach (GPoint point in _chainOverlayInputPoints)
                    DrawChainPointMarker(graphics, point, "IN", Color.LimeGreen, markerFont);
                foreach (GPoint point in _chainOverlayOutputPoints)
                    DrawChainPointMarker(graphics, point, "OUT", Color.OrangeRed, markerFont);
            }
        }

        /// <summary>두 모서리가 있으면 작업 영역 사각형을 그리고, 하나만 있으면 지정된 모서리만 표시한다.</summary>
        private void DrawChainWorkArea(Graphics graphics)
        {
            if (!_chainOverlayWorkAreaFirstPoint.HasValue)
                return;

            PointF first = WorldToScreen(_chainOverlayWorkAreaFirstPoint.Value);
            if (!_chainOverlayWorkAreaSecondPoint.HasValue)
            {
                DrawWorkAreaCorner(graphics, first);
                return;
            }

            PointF second = WorldToScreen(_chainOverlayWorkAreaSecondPoint.Value);
            var rectangle = RectangleF.FromLTRB(
                Math.Min(first.X, second.X),
                Math.Min(first.Y, second.Y),
                Math.Max(first.X, second.X),
                Math.Max(first.Y, second.Y));

            using (var fill = new SolidBrush(Color.FromArgb(24, Color.DeepSkyBlue)))
            using (var pen = new Pen(Color.DeepSkyBlue, 2f) { DashStyle = DashStyle.Dash })
            {
                graphics.FillRectangle(fill, rectangle);
                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }

            DrawWorkAreaCorner(graphics, first);
            DrawWorkAreaCorner(graphics, second);
        }

        /// <summary>작업 영역 모서리를 확대 상태에서도 확인할 수 있는 작은 사각형으로 표시한다.</summary>
        private static void DrawWorkAreaCorner(Graphics graphics, PointF point)
        {
            const float size = 8f;
            using (var fill = new SolidBrush(Color.DeepSkyBlue))
            using (var pen = new Pen(Color.White, 1f))
            {
                var rectangle = new RectangleF(point.X - size / 2f, point.Y - size / 2f, size, size);
                graphics.FillRectangle(fill, rectangle);
                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        /// <summary>Input과 Output을 색상 원, 흰색 외곽선, 짧은 문자로 표시한다.</summary>
        private void DrawChainPointMarker(Graphics graphics, GPoint worldPoint, string text, Color color, Font font)
        {
            PointF screen = WorldToScreen(worldPoint);
            const float radius = 13f;
            var circle = new RectangleF(screen.X - radius, screen.Y - radius, radius * 2f, radius * 2f);

            using (var fill = new SolidBrush(Color.FromArgb(220, color)))
            using (var border = new Pen(Color.White, 2f))
            using (var textBrush = new SolidBrush(Color.White))
            {
                graphics.FillEllipse(fill, circle);
                graphics.DrawEllipse(border, circle);
                SizeF textSize = graphics.MeasureString(text, font);
                graphics.DrawString(
                    text,
                    font,
                    textBrush,
                    screen.X - textSize.Width / 2f,
                    screen.Y - textSize.Height / 2f);
            }
        }
    }
}
