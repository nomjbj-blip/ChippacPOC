using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
	/// <summary>StatusStrip에 전달할 도면 구성과 GPU 처리 진행 상태다.</summary>
	public sealed class GdsMapRenderProgressChangedEventArgs : EventArgs
	{
		public string Message { get; private set; }
		public int Percent { get; private set; }
		public bool IsMarquee { get; private set; }
		public bool IsCompleted { get; private set; }
		public GdsMapRenderProgressChangedEventArgs(string message, int percent, bool isMarquee, bool isCompleted)
		{
			Message = message;
			Percent = percent;
			IsMarquee = isMarquee;
			IsCompleted = isCompleted;
		}
	}

	/// <summary>첫 화면까지의 병목 구간을 비교하기 위한 시간과 처리량을 보관한다.</summary>
	public sealed class GdsMapLoadMetrics : EventArgs
	{
		public double FlattenMs { get; internal set; }
		public double CoordinateTransformMs { get; internal set; }
		public double SceneItemCreateMs { get; internal set; }
		public double SceneInsertMs { get; internal set; }
		public double SrefLookupMs { get; internal set; }
		public double SrefMatrixMs { get; internal set; }
		public double SrefPathMs { get; internal set; }
		public double TextRegisterMs { get; internal set; }
		public double VertexBuildMs { get; internal set; }
		public double GpuUploadMs { get; internal set; }
		public double FirstDrawMs { get; internal set; }
		public int SceneItemCount { get; internal set; }
		public int BoundaryCount { get; internal set; }
		public int PathCount { get; internal set; }
		public int TextCount { get; internal set; }
		public int SrefCount { get; internal set; }
		public long SourcePointCount { get; internal set; }
		public int ReusedPointArrayCount { get; internal set; }
		public long ReusedPointCount { get; internal set; }
		public int VertexCount { get; internal set; }
	}

	/// <summary>
	/// High-performance GDSII Viewer control based on OpenTK 4.x and OpenGL 4.0 Core Profile.
	/// Replaces GDI+ rendering pipeline with GPU VBO/VAO batching for extreme scale performance.
	/// </summary>
	public partial class GdsMapControl : GLControl
	{
		public enum ViewMode { View, Select }

		// ---- GPU Vertex Structure --------------------------------------------------------
		// 12바이트 정점: 위치(float 2) + 색(RGB 3바이트) + 선택 표시(1바이트, 255 = 선택). 채우기 / 외곽선 투명도는 uniform(uAlpha)로 준다.
		// 예전 28바이트(위치 + Vector4 색 + float 선택)는 U8 제품 Cell(도형 181만, 정점 약 2억 6천만)에서 배열 2GB 한도를 넘었다 (2026-10-20)
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
		private struct Vertex
		{
			public float X;
			public float Y;
			public uint Color; // R | G << 8 | B << 16 | 선택(0 / 255) << 24

			public Vertex(float x, float y, uint color)
			{
				X = x;
				Y = y;
				Color = color;
			}
		}

		private const int VertexStride = 12;
		private const uint SelectedMask = 0xFF000000u;
		// List<T> 배열 1개 2GB 한도보다 충분히 작게 막아, 넘으면 예외 대신 원인을 알려 준다 (Layer 1개 기준)
		private const int MaxLayerVertices = 120000000;
		private const int MaxLayerIndices = 400000000;

		/// <summary>RGB 색과 선택 표시를 정점 색 4바이트로 만든다.</summary>
		private static uint PackColor(Color color, bool selected)
		{
			return (uint)color.R | ((uint)color.G << 8) | ((uint)color.B << 16) | (selected ? SelectedMask : 0u);
		}

		/// <summary>
		/// Layer 하나의 GPU 버퍼 (2026-10-20). 다각형은 점 n 개만 정점으로 두고 채우기는 인덱스 3(n-2)개, 외곽선은 같은 정점으로
		/// LineLoop / LineStrip 을 MultiDrawArrays 로 한 번에 그린다. Layer 마다 VAO / VBO / EBO 를 따로 둬 배열 1개 한도와 무관하다.
		/// CPU 정점 목록은 색 / 선택 변경 부분 업로드에 쓰려고 남기고, 채우기 인덱스는 업로드 뒤 비운다.
		/// </summary>
		private sealed class LayerGpuBuffer
		{
			public readonly int LayerKey;
			public readonly List<Vertex> Vertices = new List<Vertex>();
			public List<int> FillIndices = new List<int>();
			public int FillIndexCount;
			public readonly List<int> LoopFirst = new List<int>();
			public readonly List<int> LoopCount = new List<int>();
			public readonly List<int> StripFirst = new List<int>();
			public readonly List<int> StripCount = new List<int>();
			public int[] LoopFirstArray = new int[0];
			public int[] LoopCountArray = new int[0];
			public int[] StripFirstArray = new int[0];
			public int[] StripCountArray = new int[0];
			public int Vao;
			public int Vbo;
			public int Ebo;
			public bool Uploaded;

			public LayerGpuBuffer(int layerKey)
			{
				LayerKey = layerKey;
			}

			/// <summary>점들을 정점으로 넣고 첫 정점 번호를 돌려준다.</summary>
			public int AddPoints(GPoint[] points, uint color)
			{
				CheckLimit(points.Length, 0);
				int start = Vertices.Count;
				for (int i = 0; i < points.Length; i++)
					Vertices.Add(new Vertex((float)points[i].X, (float)points[i].Y, color));
				return start;
			}

			/// <summary>start 부터 count 개 점을 첫 점 기준 부채꼴 삼각형으로 채운다(기존 채우기와 같은 방식).</summary>
			public void AddFan(int start, int count)
			{
				CheckLimit(0, 3 * (count - 2));
				for (int i = 1; i < count - 1; i++)
				{
					FillIndices.Add(start);
					FillIndices.Add(start + i);
					FillIndices.Add(start + i + 1);
				}
			}

			/// <summary>PATH 면의 삼각형 하나(정점 3개 + 인덱스 3개)를 넣는다.</summary>
			public void AddTriangle(Vector2 a, Vector2 b, Vector2 c, uint color)
			{
				CheckLimit(3, 3);
				int start = Vertices.Count;
				Vertices.Add(new Vertex(a.X, a.Y, color));
				Vertices.Add(new Vertex(b.X, b.Y, color));
				Vertices.Add(new Vertex(c.X, c.Y, color));
				FillIndices.Add(start);
				FillIndices.Add(start + 1);
				FillIndices.Add(start + 2);
			}

			/// <summary>그리기 목록을 MultiDrawArrays 용 배열로 확정한다.</summary>
			public void FinishDrawLists()
			{
				LoopFirstArray = LoopFirst.ToArray();
				LoopCountArray = LoopCount.ToArray();
				StripFirstArray = StripFirst.ToArray();
				StripCountArray = StripCount.ToArray();
				FillIndexCount = FillIndices.Count;
			}

			private void CheckLimit(int addVertices, int addIndices)
			{
				if ((long)Vertices.Count + addVertices > MaxLayerVertices || (long)FillIndices.Count + addIndices > MaxLayerIndices)
					throw new InvalidOperationException("Layer " + GdsLayerKey.Format(LayerKey) + " 의 도형이 너무 많아 화면에 그릴 수 없습니다(정점 "
						+ Vertices.Count.ToString("N0") + "개). Layer 를 나눠 저장하거나 담당자에게 문의하세요.");
			}
		}

		// Events
		public event EventHandler<PointF> MouseWorldPositionChanged;
		public event EventHandler SelectionChanged;
		public event EventHandler<ViewMode> ModeChanged;
		public event EventHandler<double> ZoomChanged;
		/// <summary>도면 구성, GPU 버퍼 생성, 최초 화면 그리기 단계를 화면에 전달한다.</summary>
		public event EventHandler<GdsMapRenderProgressChangedEventArgs> RenderProgressChanged;
		/// <summary>새 도면의 첫 화면이 그려진 뒤 구간별 측정 결과를 전달한다.</summary>
		public event EventHandler<GdsMapLoadMetrics> FirstFrameMeasured;

		// Colors & Palette
		private static readonly Color SelectionColor = Color.FromArgb(255, 235, 59);
		private static readonly Color DefectColor = Color.Red;

		private const double MinScale = 1e-6;
		private const double MaxScale = 1e9;
		private const float ClickDragThreshold = 4f;

		// Transform states
		private double _scale = 1.0;
		private Vector2 _offset = Vector2.Zero; // Camera translation in world unit

		[DefaultValue(typeof(ViewMode), "View")]
		private ViewMode _mode = ViewMode.View;

		private bool _showOriginCross = true;

		// OpenGL Resources
		private int _shaderProgram;
		private int _uMatrixLoc;
		private int _uAlphaLoc;
		private bool _glInitialized;
		// Layer 별 GPU 버퍼(Layer 키 -> 버퍼, GdsLayerKey)와 결함 버퍼 (2026-10-20)
		private readonly Dictionary<int, LayerGpuBuffer> _layerBuffers = new Dictionary<int, LayerGpuBuffer>();
		private LayerGpuBuffer _defectBuffer;
		private int[] _indexUploadChunk;
		private bool _gpuBufferReady;
		private Vertex[] _partialUploadChunk;
		private int _lastRenderProgressTick;
		private bool _firstFrameProgressPending;
		private bool _loadFramePending;
		private GdsMapLoadMetrics _loadMetrics;
		private long _coordinateTransformTicks;
		private long _sceneItemCreateTicks;
		private long _sceneInsertTicks;
		private long _srefLookupTicks;
		private long _srefMatrixTicks;
		private long _srefPathTicks;
		private long _textRegisterTicks;
		private int _lastFrameTimingLogTick;

		// GLSL Shaders
		// aColor: rgb = Layer 색, a = 선택(1). 선택 도형은 채우기 / 외곽선 모두 노란색 불투명, 나머지는 uAlpha(채우기 ColorAlpha, 외곽선 1) (2026-10-20)
		private const string VertexShaderCode = @"
			#version 330 core
			layout(location = 0) in vec2 aPos;
			layout(location = 1) in vec4 aColor;

			uniform mat4 uMatrix;
			uniform float uAlpha;
			out vec4 vColor;

			void main() {
				gl_Position = uMatrix * vec4(aPos, 0.0, 1.0);
				if (aColor.a > 0.5) {
					vColor = vec4(1.0, 0.92, 0.23, 1.0); // Highlight Yellow
				} else {
					vColor = vec4(aColor.rgb, uAlpha);
				}
			}
		";

		private const string FragmentShaderCode = @"
			#version 330 core
			in vec4 vColor;
			out vec4 FragColor;

			void main() {
				FragColor = vColor;
			}
		";

		/// <summary>
		/// 지도 우클릭 메뉴는 두지 않는다. 2026-10-12 요청로 단계적으로 뺐다.
		///  -: 보기 모드 / 선택 모드(선택 결과를 쓰는 화면 없음), 선택 해제(Chain 강조와 어긋남) 제거
		///  -: 전체 보기(Zoom to Fit) 제거, 원점 십자가 표시는 각 화면의 [원점 표시] 체크로 옮김(ShowOriginCross)
		/// 우클릭은 화면 쪽 메뉴(예: Chain 목록)와 겹치지 않게 지도에서 아무 동작도 하지 않는다.
		/// </summary>
		public GdsMapControl()
		{
		}

		public ViewMode Mode
		{
			get => _mode;
			set
			{
				if (_mode == value) return;
				_isPanning = false;
				_isDragTracking = false;
				_mode = value;
				Cursor = _mode == ViewMode.View ? Cursors.SizeAll : Cursors.Cross;
				ModeChanged?.Invoke(this, _mode);
				Invalidate();
			}
		}

		public double ZoomFactor => _scale;

		/// <summary>
		/// 월드 좌표 원점인 (0, 0)에 십자가를 표시할지 결정한다.
		/// 값이 바뀌면 현재 Zoom과 Offset을 사용해 화면 위치를 다시 계산하도록 다시 그린다.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowOriginCross
		{
			get => _showOriginCross;
			set
			{
				if (_showOriginCross == value) return;
				_showOriginCross = value;
				Invalidate();
			}
		}

		public IEnumerable<GdsElement> GetSelectedElements()
		{
			return _selectedItems.Select(a => a.Source);
		}

		// ---- OpenGL Initialization & Pipeline Setup --------------------------------------

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (DesignMode) return;

			GL.ClearColor(0.09f, 0.09f, 0.11f, 1.0f); // Dark background (#18181C)
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			// Shader compilation
			int vs = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vs, VertexShaderCode);
			GL.CompileShader(vs);

			int fs = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fs, FragmentShaderCode);
			GL.CompileShader(fs);

			_shaderProgram = GL.CreateProgram();
			GL.AttachShader(_shaderProgram, vs);
			GL.AttachShader(_shaderProgram, fs);
			GL.LinkProgram(_shaderProgram);

			GL.DeleteShader(vs);
			GL.DeleteShader(fs);

			_uMatrixLoc = GL.GetUniformLocation(_shaderProgram, "uMatrix");
			_uAlphaLoc = GL.GetUniformLocation(_shaderProgram, "uAlpha");
			// 정점 버퍼는 도면을 그릴 때 Layer 별로 만든다(UploadAllGpuVertices) (2026-10-20)
			InitializeChainOverlayGlResources();

			_glInitialized = true;
			// 컨트롤 초기화 전에 도면을 받은 경우에도 한 번만 전체 정점을 업로드한다.
			if (_layerBuffers.Count > 0 || _defectBuffer != null)
				UploadAllGpuVertices();
		}

		// ---- GDS Structure Loading & Triangulation --------------------------------------

		/// <summary>최상위 Cell(펼친 도형이 가장 많은 Cell)을 그린다. 첫 Structure 가 하위 Cell 인 파일도 KLayout 과 같은 Cell 로 연다 (2026-10-20)</summary>
		public void ShowStructure(GdsLibrary lib)
		{
			string name = lib == null ? null : lib.GetDefaultTopStructureName();
			if (name != null)
				ShowStructure(lib, name);
		}

		public void ShowStructure(GdsLibrary lib, string structureName)
		{
			// 진행 중인 Service 배치 추출이 새 도면의 도형을 이어 보내지 않도록 버전을 먼저 바꾼다.
			_mapContentVersion++;
			_loadMetrics = new GdsMapLoadMetrics();
			ResetFlattenMeasurements();
			_loadFramePending = false;
			if (lib != null && lib.Structures.TryGetValue(structureName, out var str))
			{
				ReportRenderProgress("도면 구조 생성 중", 0, true, false, true);
				Structure = str;

				// 파일을 다시 열 때 이전 레이어/선택이 새 목록과 섞이지 않도록 도면 상태를 비운다.
				ClearSelectionInternal();
				_layerList.Clear();
				_defectList.Clear();
				_textLabels.Clear();

				long flattenStart = Stopwatch.GetTimestamp();
				using (var identity = new System.Drawing.Drawing2D.Matrix())
					FlattenStructure(lib, str, identity, 1.0, new HashSet<string>(StringComparer.OrdinalIgnoreCase),
						0, str.Name, "R0");
				SortMeasuredSceneItems();
				ApplyInitialLayerColors();
				_loadMetrics.FlattenMs = ElapsedMilliseconds(flattenStart);
				CompleteFlattenMeasurements();
				CalculateTextLabelDisplayAreas();
				// GDS TEXT에는 특정 도형 소유 관계가 없으므로 주변 도형 검색으로 표시 여부를 제한하지 않는다.
			}

			BuildGpuBuffers();
			RaiseSelectionChanged();
			// 첫 화면 측정은 새 도면에서만 수행한다. Layer 변경으로 인한 다시 그리기는 제외한다.
			_loadFramePending = true;
			ZoomToFit();
		}

		/// <summary>
		/// 새 도면의 모든 Layer 를 표시 여부와 무관하게 Layer 별 GPU 버퍼로 구성한다 (2026-10-20 Layer 별 버퍼 / 12바이트 정점 / 인덱스 채우기).
		/// 처리 순서: 이전 버퍼 해제 -> Layer 마다 도형 정점 / 채우기 인덱스 / 외곽선 목록 생성 -> 결함 버퍼 -> 업로드.
		///  - 닫힌 도형: 점 n 개 + 부채꼴 인덱스 3(n-2) + 외곽선 LineLoop
		///  - 폭 있는 PATH: 면 삼각형(정점 3 + 인덱스 3씩), 외곽선 없음(기존과 같음)
		///  - 그 밖의 2점 이상: 외곽선 LineStrip
		/// </summary>
		private void BuildGpuBuffers()
		{
			_gpuBufferReady = false;
			ReleaseGpuBuffers();
			_layerBuffers.Clear();
			_defectBuffer = null;
			int totalItems = _layerList.Sum(layer => layer.Items.Count) + _defectList.Count;
			int processedItems = 0;
			ReportRenderProgress("GPU 버퍼 생성 중", 0, false, false, true);
			long vertexStart = Stopwatch.GetTimestamp();
			long vertexCount = 0;

			foreach (var layer in _layerList)
			{
				var buffer = new LayerGpuBuffer(layer.LayerKey);
				foreach (var item in layer.Items)
				{
					uint color = PackColor(layer.Color, item.Selected);
					item.VertexOffset = buffer.Vertices.Count;
					item.FillIndexOffset = buffer.FillIndices.Count;
					item.LineFirst = -1;
					item.LineCount = 0;
					item.LineLoop = false;
					if (item.Source is GdsPath filledPath && item.Width > 0 && item.WorldPoints.Length > 0)
					{
						// GDS PATH의 폭을 실제 면으로 만든다. 한 좌표만 가진 PATH도 패드로 표시한다.
						AddPathFillVertices(buffer, item, filledPath, color);
					}
					else if (item.Closed && item.WorldPoints.Length >= 3)
					{
						int start = buffer.AddPoints(item.WorldPoints, color);
						buffer.AddFan(start, item.WorldPoints.Length);
						item.LineFirst = start;
						item.LineCount = item.WorldPoints.Length;
						item.LineLoop = true;
						buffer.LoopFirst.Add(start);
						buffer.LoopCount.Add(item.WorldPoints.Length);
					}
					else if (item.WorldPoints.Length >= 2)
					{
						int start = buffer.AddPoints(item.WorldPoints, color);
						item.LineFirst = start;
						item.LineCount = item.WorldPoints.Length;
						buffer.StripFirst.Add(start);
						buffer.StripCount.Add(item.WorldPoints.Length);
					}
					item.VertexCount = buffer.Vertices.Count - item.VertexOffset;
					item.FillIndexCount = buffer.FillIndices.Count - item.FillIndexOffset;
					ReportBufferBuildProgress(++processedItems, totalItems);
				}
				buffer.FinishDrawLists();
				_layerBuffers[layer.LayerKey] = buffer;
				vertexCount += buffer.Vertices.Count;
			}

			// defect: 사각형 채우기만(외곽선 없음)
			if (_defectList.Count > 0)
			{
				_defectBuffer = new LayerGpuBuffer(-1);
				uint defectColor = PackColor(DefectColor, false);
				foreach (var item in _defectList)
				{
					item.VertexOffset = _defectBuffer.Vertices.Count;
					item.FillIndexOffset = _defectBuffer.FillIndices.Count;
					int start = _defectBuffer.AddPoints(item.WorldPoints, defectColor);
					_defectBuffer.AddFan(start, item.WorldPoints.Length);
					item.VertexCount = _defectBuffer.Vertices.Count - item.VertexOffset;
					item.FillIndexCount = _defectBuffer.FillIndices.Count - item.FillIndexOffset;
					ReportBufferBuildProgress(++processedItems, totalItems);
				}
				_defectBuffer.FinishDrawLists();
				vertexCount += _defectBuffer.Vertices.Count;
			}

			_loadMetrics.VertexBuildMs = ElapsedMilliseconds(vertexStart);
			_loadMetrics.VertexCount = (int)Math.Min(int.MaxValue, vertexCount);
			long uploadStart = Stopwatch.GetTimestamp();
			UploadAllGpuVertices();
			_loadMetrics.GpuUploadMs = ElapsedMilliseconds(uploadStart);
			ReportRenderProgress("GPU 버퍼 생성 완료", 100, false, false, true);
			// 다음 OnPaint에서 실제 그리기 진행률을 이어서 보고한다.
			_firstFrameProgressPending = true;
		}

		/// <summary>
		/// PATH 중심선과 폭으로 삼각형 면을 만든다. 1점 PATH는 원형/사각형 패드로,
		/// 여러 점은 각 선분의 사각형과 꺾이는 위치의 연결 삼각형으로 표시한다.
		/// </summary>
		private static void AddPathFillVertices(LayerGpuBuffer buffer, GlSceneItem item, GdsPath path, uint color)
		{
			float radius = (float)(item.Width / 2.0);
			if (radius <= 0) return;
			GPoint[] points = item.WorldPoints;
			if (points.Length == 1)
			{
				AddPathPad(buffer, new Vector2((float)points[0].X, (float)points[0].Y), radius, path.PathType == 1, color);
				return;
			}

			bool hasSegment = false;
			Vector2 previousDirection = Vector2.Zero;
			Vector2 previousNormal = Vector2.Zero;
			Vector2 firstPoint = Vector2.Zero;
			Vector2 lastPoint = Vector2.Zero;
			for (int i = 0; i < points.Length - 1; i++)
			{
				var start = new Vector2((float)points[i].X, (float)points[i].Y);
				var end = new Vector2((float)points[i + 1].X, (float)points[i + 1].Y);
				Vector2 direction = end - start;
				float length = direction.Length;
				if (length <= 0) continue;
				direction /= length;
				var normal = new Vector2(-direction.Y * radius, direction.X * radius);
				if (hasSegment)
				{
					float cross = previousDirection.X * direction.Y - previousDirection.Y * direction.X;
					if (Math.Abs(cross) > 0.000001f)
					{
						float side = cross > 0 ? -1f : 1f;
						buffer.AddTriangle(start, start + previousNormal * side, start + normal * side, color);
					}
				}
				else firstPoint = start;

				Vector2 drawStart = start;
				Vector2 drawEnd = end;
				if (path.PathType == 2)
				{
					if (!hasSegment) drawStart -= direction * radius;
					if (i == points.Length - 2) drawEnd += direction * radius;
				}
				buffer.AddTriangle(drawStart + normal, drawStart - normal, drawEnd + normal, color);
				buffer.AddTriangle(drawStart - normal, drawEnd - normal, drawEnd + normal, color);
				previousDirection = direction;
				previousNormal = normal;
				lastPoint = end;
				hasSegment = true;
			}

			if (!hasSegment)
				AddPathPad(buffer, new Vector2((float)points[0].X, (float)points[0].Y), radius, path.PathType == 1, color);
			else if (path.PathType == 1)
			{
				AddPathPad(buffer, firstPoint, radius, true, color);
				AddPathPad(buffer, lastPoint, radius, true, color);
			}
		}

		/// <summary>한 좌표의 PATH를 폭만큼 채운다. Round 타입은 원, 나머지는 정사각형으로 표시한다.</summary>
		private static void AddPathPad(LayerGpuBuffer buffer, Vector2 center, float radius, bool round, uint color)
		{
			if (!round)
			{
				var x = new Vector2(radius, 0);
				var y = new Vector2(0, radius);
				buffer.AddTriangle(center - x - y, center + x - y, center + x + y, color);
				buffer.AddTriangle(center - x - y, center + x + y, center - x + y, color);
				return;
			}
			const int segments = 16;
			for (int i = 0; i < segments; i++)
			{
				float angle1 = (float)(2.0 * Math.PI * i / segments);
				float angle2 = (float)(2.0 * Math.PI * (i + 1) / segments);
				var point1 = center + new Vector2((float)Math.Cos(angle1) * radius, (float)Math.Sin(angle1) * radius);
				var point2 = center + new Vector2((float)Math.Cos(angle2) * radius, (float)Math.Sin(angle2) * radius);
				buffer.AddTriangle(center, point1, point2, color);
			}
		}

		/// <summary>새 도면에서만 Layer 버퍼 전체를 업로드한다. Layer 색 / 선택 변경은 부분 업로드를 쓴다.</summary>
		private void UploadAllGpuVertices()
		{
			if (!_glInitialized) return;
			ReportRenderProgress("GPU 업로드 중", 0, true, false, true);
			// 색상 선택 창을 닫은 뒤에도 이 컨트롤의 OpenGL 컨텍스트에 업로드한다.
			MakeCurrent();
			foreach (var buffer in _layerBuffers.Values)
				UploadBuffer(buffer);
			if (_defectBuffer != null)
				UploadBuffer(_defectBuffer);
			GL.BindVertexArray(0);
			_gpuBufferReady = true;
			ReportRenderProgress("GPU 업로드 완료", 100, false, false, true);
		}

		/// <summary>
		/// Layer 버퍼 하나의 VAO / VBO / EBO 를 만들고 전송한다. ToArray 로 한 번 더 복사하면 메모리가 두 배로 들어
		/// 고정 크기 배열로 나눠 보낸다. 채우기 인덱스는 GPU 에만 있으면 되므로 전송 뒤 CPU 목록을 비운다.
		/// </summary>
		private void UploadBuffer(LayerGpuBuffer buffer)
		{
			const int chunkVertices = 8192;
			const int chunkIndices = 65536;
			if (buffer.Vao == 0)
			{
				buffer.Vao = GL.GenVertexArray();
				buffer.Vbo = GL.GenBuffer();
				buffer.Ebo = GL.GenBuffer();
			}
			GL.BindVertexArray(buffer.Vao);

			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr((long)buffer.Vertices.Count * VertexStride), IntPtr.Zero, BufferUsageHint.DynamicDraw);
			if (_partialUploadChunk == null)
				_partialUploadChunk = new Vertex[chunkVertices];
			for (int position = 0; position < buffer.Vertices.Count; position += chunkVertices)
			{
				int count = Math.Min(chunkVertices, buffer.Vertices.Count - position);
				buffer.Vertices.CopyTo(position, _partialUploadChunk, 0, count);
				GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr((long)position * VertexStride), count * VertexStride, _partialUploadChunk);
			}
			// 위치: float 2개 / 색: 부호 없는 바이트 4개를 0~1 로 정규화(rgb = 색, a = 선택)
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, VertexStride, 0);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, VertexStride, sizeof(float) * 2);
			GL.EnableVertexAttribArray(1);

			// EBO 연결은 VAO 에 기록된다.
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.Ebo);
			if (buffer.FillIndices != null)
			{
				GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr((long)buffer.FillIndices.Count * sizeof(int)), IntPtr.Zero, BufferUsageHint.StaticDraw);
				if (_indexUploadChunk == null)
					_indexUploadChunk = new int[chunkIndices];
				for (int position = 0; position < buffer.FillIndices.Count; position += chunkIndices)
				{
					int count = Math.Min(chunkIndices, buffer.FillIndices.Count - position);
					buffer.FillIndices.CopyTo(position, _indexUploadChunk, 0, count);
					GL.BufferSubData(BufferTarget.ElementArrayBuffer, new IntPtr((long)position * sizeof(int)), count * sizeof(int), _indexUploadChunk);
				}
				buffer.FillIndexCount = buffer.FillIndices.Count;
				buffer.FillIndices = null;
			}
			buffer.Uploaded = true;
		}

		/// <summary>이전 도면의 Layer / 결함 GPU 버퍼를 지운다. GL 컨텍스트가 없으면 CPU 목록만 버린다.</summary>
		private void ReleaseGpuBuffers()
		{
			if (!_glInitialized || IsDisposed)
				return;
			MakeCurrent();
			foreach (var buffer in _layerBuffers.Values)
				DeleteBufferObjects(buffer);
			if (_defectBuffer != null)
				DeleteBufferObjects(_defectBuffer);
		}

		private static void DeleteBufferObjects(LayerGpuBuffer buffer)
		{
			if (buffer.Vao == 0) return;
			GL.DeleteVertexArray(buffer.Vao);
			GL.DeleteBuffer(buffer.Vbo);
			GL.DeleteBuffer(buffer.Ebo);
			buffer.Vao = buffer.Vbo = buffer.Ebo = 0;
			buffer.Uploaded = false;
		}

		/// <summary>여러 Item의 부분 업로드 구간을 정렬하고 병합하는 데 사용한다.</summary>
		private struct VertexRange
		{
			public int Start;
			public int Count;

			public VertexRange(int start, int count)
			{
				Start = start;
				Count = count;
			}
		}

		/// <summary>Layer 색상만 CPU 정점에 반영하고(선택 표시는 유지) 그 Layer 버퍼만 다시 보낸다.</summary>
		private void UpdateLayerColorVertices(GlSceneLayer layer)
		{
			LayerGpuBuffer buffer;
			if (!_layerBuffers.TryGetValue(layer.LayerKey, out buffer) || buffer.Vertices.Count == 0)
				return;
			uint rgb = PackColor(layer.Color, false);
			for (int i = 0; i < buffer.Vertices.Count; i++)
			{
				var vertex = buffer.Vertices[i];
				vertex.Color = (vertex.Color & SelectedMask) | rgb;
				buffer.Vertices[i] = vertex;
			}
			UploadVertexRanges(buffer, new List<VertexRange> { new VertexRange(0, buffer.Vertices.Count) });
		}

		/// <summary>선택 상태가 실제로 바뀐 Item만 CPU 정점과 그 Layer 버퍼 구간에 반영한다.</summary>
		private void UpdateSelectionVertices(IEnumerable<GlSceneItem> changedItems)
		{
			var rangesByLayer = new Dictionary<int, List<VertexRange>>();
			foreach (var item in changedItems)
			{
				LayerGpuBuffer buffer;
				if (item.VertexCount == 0 || !_layerBuffers.TryGetValue(item.LayerKey, out buffer))
					continue;
				uint selected = item.Selected ? SelectedMask : 0u;
				for (int i = item.VertexOffset; i < item.VertexOffset + item.VertexCount; i++)
				{
					var vertex = buffer.Vertices[i];
					vertex.Color = (vertex.Color & ~SelectedMask) | selected;
					buffer.Vertices[i] = vertex;
				}
				List<VertexRange> ranges;
				if (!rangesByLayer.TryGetValue(item.LayerKey, out ranges))
					rangesByLayer[item.LayerKey] = ranges = new List<VertexRange>();
				ranges.Add(new VertexRange(item.VertexOffset, item.VertexCount));
			}
			foreach (var pair in rangesByLayer)
				UploadVertexRanges(_layerBuffers[pair.Key], pair.Value);
		}

		/// <summary>인접 변경 구간을 합치고 고정 크기 배열로 나눠 그 Layer VBO 의 해당 구간만 다시 보낸다.</summary>
		private void UploadVertexRanges(LayerGpuBuffer buffer, List<VertexRange> ranges)
		{
			if (!_glInitialized || !_gpuBufferReady || !buffer.Uploaded || ranges.Count == 0) return;
			ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
			MakeCurrent();
			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Vbo);
			const int chunkVertices = 8192;
			// 큰 배열을 색상/선택 변경마다 다시 할당하지 않도록 처음 사용 시 한 번만 만든다.
			if (_partialUploadChunk == null)
				_partialUploadChunk = new Vertex[chunkVertices];
			int start = ranges[0].Start;
			int end = start + ranges[0].Count;
			for (int i = 1; i <= ranges.Count; i++)
			{
				if (i < ranges.Count && ranges[i].Start <= end)
				{
					end = Math.Max(end, ranges[i].Start + ranges[i].Count);
					continue;
				}
				for (int position = start; position < end; position += chunkVertices)
				{
					int count = Math.Min(chunkVertices, end - position);
					buffer.Vertices.CopyTo(position, _partialUploadChunk, 0, count);
					GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr((long)position * VertexStride), count * VertexStride, _partialUploadChunk);
				}
				if (i < ranges.Count)
				{
					start = ranges[i].Start;
					end = start + ranges[i].Count;
				}
			}
		}

		/// <summary>Layer 변경 후에도 기존 화면 그리기 진행률을 유지한다.</summary>
		private void InvalidateWithDrawProgress()
		{
			_firstFrameProgressPending = true;
			Invalidate();
		}

		/// <summary>128개 단위와 120ms 제한을 함께 적용해 진행 상태 갱신 비용을 제한한다.</summary>
		private void ReportBufferBuildProgress(int processedItems, int totalItems)
		{
			if (processedItems % 128 != 0 && processedItems != totalItems) return;
			int percent = totalItems == 0 ? 100 : processedItems * 100 / totalItems;
			ReportRenderProgress("GPU 버퍼 생성 중 / " + processedItems + "개", percent, false, false, processedItems == totalItems);
		}

		/// <summary>상태 이벤트를 시간 제한해 렌더링 작업에 미치는 영향을 줄인다.</summary>
		private void ReportRenderProgress(string message, int percent, bool marquee, bool completed, bool force)
		{
			int now = Environment.TickCount;
			if (!force && unchecked(now - _lastRenderProgressTick) < 120) return;
			_lastRenderProgressTick = now;
			RenderProgressChanged?.Invoke(this, new GdsMapRenderProgressChangedEventArgs(message, percent, marquee, completed));
		}

		// ---- Flattening (Identical logic to GDS Geometry Engine) --------------------------

		/// <summary>
		/// 참조 구조를 재귀적으로 펼쳐 도형을 월드 좌표로 바꾸고 화면용 Layer에 모은다.
		/// parent는 이 구조의 점을 월드 좌표로 보내는 누적 배치 행렬, parentMagnification은 그 누적 배율이다.
		/// System.Drawing.Matrix는 점을 행벡터로 변환하므로 자식 배치 행렬은 부모 행렬 앞에 곱한다(자식 먼저, 부모 나중).
		/// </summary>
		private void FlattenStructure(GdsLibrary lib, GdsStructure str, System.Drawing.Drawing2D.Matrix parent, double parentMagnification,
			HashSet<string> stack, int depth, string structurePath, string instancePath)
		{
			if (depth > 64 || stack.Contains(str.Name)) 
				return;

			stack.Add(str.Name);
			bool parentIsIdentity = parent.IsIdentity;
			ParentTranslation translation = ParentTranslation.From(parent);
			int sourceOrdinal = 0;

			foreach (var layer in str.Layers)
			{
				foreach (var e in layer.Elements)
				{
					// 좌표가 같아도 서로 다른 원본/참조 인스턴스는 다른 배치 ID를 갖는다.
					int elementOrdinal = sourceOrdinal++;
					string placedId = instancePath + "/E" + elementOrdinal;
					string sourceId = str.Name + "/E" + elementOrdinal;
					if (e is GdsBoundary boundary)
					{
						_loadMetrics.BoundaryCount++;
						_loadMetrics.SourcePointCount += boundary.Points.Length;
						var pts = TransformMeasuredGeometryPoints(boundary.Points, e.Transform, parent, parentIsIdentity, translation);

						if (pts.Length >= 2)
							AddMeasuredSceneItem(CreateMeasuredSceneItem(e, pts, true, 0, null, placedId, sourceId));
						else
							_layerList.AddLayer(GdsLayerKey.Of(e));
					}
					else if (e is GdsPath path)
					{
						_loadMetrics.PathCount++;
						_loadMetrics.SourcePointCount += path.Points.Length;
						var pts = TransformMeasuredGeometryPoints(path.Points, e.Transform, parent, parentIsIdentity, translation);

						// SREF 배율을 PATH 폭에 반영한다. 음수 폭은 GDS 절대 폭이라 배율을 적용하지 않는다. (2026-10-15)
						double width = path.Width < 0 ? -path.Width : path.Width * parentMagnification;
						if (pts.Length >= 1)
							AddMeasuredSceneItem(CreateMeasuredSceneItem(e, pts, false, width, null, placedId, sourceId));
						else
							_layerList.AddLayer(GdsLayerKey.Of(e));
					}
					else if (e is GdsText text)
					{
						_loadMetrics.TextCount++;
						_loadMetrics.SourcePointCount++;
						long transformStart = Stopwatch.GetTimestamp();
						var pts = TransformTextPosition(text, parent, parentIsIdentity, translation);
						_coordinateTransformTicks += Stopwatch.GetTimestamp() - transformStart;
						AddMeasuredSceneItem(CreateMeasuredSceneItem(e, pts, false, 0, text.Text, placedId, sourceId));
						long textStart = Stopwatch.GetTimestamp();
						AddTextLabel(text, pts, structurePath);
						_textRegisterTicks += Stopwatch.GetTimestamp() - textStart;
					}
					else if (e is GdsSRef sref)
					{
						_loadMetrics.SrefCount++;
						long lookupStart = Stopwatch.GetTimestamp();
						bool found = lib.Structures.TryGetValue(sref.StructureName, out var child);
						_srefLookupTicks += Stopwatch.GetTimestamp() - lookupStart;
						if (!found)
							continue;

						long matrixStart = Stopwatch.GetTimestamp();
						using (var local = GdsGeometry.CreateTransform(e.Transform))
						using (var m = (System.Drawing.Drawing2D.Matrix)local.Clone())
						{
							m.Translate((float)sref.Origin.X, (float)sref.Origin.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
							using (var combined = (System.Drawing.Drawing2D.Matrix)parent.Clone())
							{
								// combined = m × parent: 자식 점에 이 SREF 배치(m)를 먼저, 부모 배치를 나중에 적용한다.
								// Append(parent × m)는 2단 이상 중첩에 회전 / 반전 / 배율이 섞이면 좌표가 틀어져 Prepend로 바꿈 (2026-10-15)
								combined.Multiply(m, System.Drawing.Drawing2D.MatrixOrder.Prepend);
								double magnification = e.Transform.Magnification == 0 ? 1 : Math.Abs(e.Transform.Magnification);
								_srefMatrixTicks += Stopwatch.GetTimestamp() - matrixStart;
								long pathStart = Stopwatch.GetTimestamp();
								string childPath = structurePath + " > " + child.Name + " @ (" + sref.Origin.X + ", " + sref.Origin.Y + ")";
								_srefPathTicks += Stopwatch.GetTimestamp() - pathStart;
								FlattenStructure(lib, child, combined, parentMagnification * magnification, stack, depth + 1, childPath,
									instancePath + "/R" + elementOrdinal);
							}
						}
					}
				}
			}

			// defect
			foreach (var defect in str.DefectList)
			{
				var pts = TransformPoints(defect.X, defect.Y, defect.Width, defect.Height, null, parent, parentIsIdentity);
				_defectList.Add(new GlDefectItem(defect, pts));
			}

			stack.Remove(str.Name);
		}

		/// <summary>재귀 참조를 모두 펼친 뒤 Layer 색상을 한 번만 적용해 중복 순회를 없앤다.</summary>
		private void ApplyInitialLayerColors()
		{
			foreach (var layer in _layerList)
				layer.Color = GetLayerColor(layer.LayerKey);
		}

		/// <summary>새 도면을 열 때 Flatten 내부 구간별 누적 시간을 초기화한다.</summary>
		private void ResetFlattenMeasurements()
		{
			_coordinateTransformTicks = 0;
			_sceneItemCreateTicks = 0;
			_sceneInsertTicks = 0;
			_srefLookupTicks = 0;
			_srefMatrixTicks = 0;
			_srefPathTicks = 0;
			_textRegisterTicks = 0;
		}

		/// <summary>Stopwatch 누적 Tick을 화면에서 비교할 수 있는 밀리초 값으로 확정한다.</summary>
		private void CompleteFlattenMeasurements()
		{
			_loadMetrics.CoordinateTransformMs = TicksToMilliseconds(_coordinateTransformTicks);
			_loadMetrics.SceneItemCreateMs = TicksToMilliseconds(_sceneItemCreateTicks);
			_loadMetrics.SceneInsertMs = TicksToMilliseconds(_sceneInsertTicks);
			_loadMetrics.SrefLookupMs = TicksToMilliseconds(_srefLookupTicks);
			_loadMetrics.SrefMatrixMs = TicksToMilliseconds(_srefMatrixTicks);
			_loadMetrics.SrefPathMs = TicksToMilliseconds(_srefPathTicks);
			_loadMetrics.TextRegisterMs = TicksToMilliseconds(_textRegisterTicks);
		}

		/// <summary>좌표 변환 시간만 누적하여 배열 생성과 Matrix 적용 비용을 확인한다.</summary>
		private GPoint[] TransformMeasuredGeometryPoints(GPoint[] points, GTransform transform, System.Drawing.Drawing2D.Matrix parent, bool parentIsIdentity,
			ParentTranslation translation)
		{
			long start = Stopwatch.GetTimestamp();
			GPoint[] result = TransformGeometryPoints(points, transform, parent, parentIsIdentity, translation);
			_coordinateTransformTicks += Stopwatch.GetTimestamp() - start;
			if (Object.ReferenceEquals(points, result))
			{
				_loadMetrics.ReusedPointArrayCount++;
				_loadMetrics.ReusedPointCount += points.Length;
			}
			return result;
		}

		/// <summary>GlSceneItem 생성자 안의 Bounds 계산을 Layer 등록 시간과 분리하여 측정한다.</summary>
		private GlSceneItem CreateMeasuredSceneItem(GdsElement source, GPoint[] points, bool closed,
			double width, string text = null, string placedElementId = null,
			string sourceElementId = null)
		{
			long start = Stopwatch.GetTimestamp();
			var item = new GlSceneItem(source, points, closed, width, text,
				placedElementId, sourceElementId);
			_sceneItemCreateTicks += Stopwatch.GetTimestamp() - start;
			return item;
		}

		/// <summary>Flatten 내부에서 Layer 검색과 도형 등록이 차지하는 시간을 별도로 합산한다.</summary>
		private void AddMeasuredSceneItem(GlSceneItem item)
		{
			long start = Stopwatch.GetTimestamp();
			// 뒤에 붙이고 펼치기가 끝나면 SortMeasuredSceneItems 로 한 번 정렬한다 (2026-10-20)
			_layerList.AppendSceneItem(item);
			_sceneInsertTicks += Stopwatch.GetTimestamp() - start;
			_loadMetrics.SceneItemCount++;
		}

		/// <summary>펼치기 / DB 행 읽기가 끝난 뒤 Layer 항목을 한 번 정렬한다. 시간은 Layer 삽입 시간에 더한다.</summary>
		private void SortMeasuredSceneItems()
		{
			long start = Stopwatch.GetTimestamp();
			_layerList.SortSceneItems();
			_sceneInsertTicks += Stopwatch.GetTimestamp() - start;
		}

		/// <summary>Stopwatch 원시 타임스탬프를 밀리초로 변환한다.</summary>
		private static double ElapsedMilliseconds(long start)
		{
			return (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;
		}

		/// <summary>누적된 Stopwatch Tick을 밀리초로 변환한다.</summary>
		private static double TicksToMilliseconds(long ticks)
		{
			return ticks * 1000.0 / Stopwatch.Frequency;
		}

		/// <summary>
		/// GDS TEXT의 XY는 문자열 삽입점이므로 부모 구조의 배치 변환만 적용한다.
		/// TEXT 자체 MAG/ANGLE/Mirror는 삽입점을 중심으로 글자 모양에 적용되는 값이므로 좌표에는 적용하지 않는다.
		/// </summary>
		private static GPoint[] TransformTextPosition(GdsText text, System.Drawing.Drawing2D.Matrix parent, bool parentIsIdentity, ParentTranslation translation)
		{
			var position = new GPoint[] { text.Position };
			return translation.IsTranslation && !parentIsIdentity ? TranslatePoints(position, translation)
				: TransformPoints(position, null, parent, parentIsIdentity);
		}

		/// <summary>회전/배율/반전이 없는 도형에는 동일한 좌표 결과를 내는 부모 변환만 적용한다.</summary>
		private static GPoint[] TransformGeometryPoints(GPoint[] points, GTransform transform, System.Drawing.Drawing2D.Matrix parent, bool parentIsIdentity,
			ParentTranslation translation)
		{
			if (!transform.MirrorX && transform.Rotation == 0 && (transform.Magnification == 0 || transform.Magnification == 1))
				return translation.IsTranslation && !parentIsIdentity ? TranslatePoints(points, translation)
					: TransformPoints(points, null, parent, parentIsIdentity);

			using (var local = GdsGeometry.CreateTransform(transform))
				return TransformPoints(points, local, parent, parentIsIdentity);
		}

		/// <summary>결함 사각형의 네 꼭짓점을 만들어 일반 도형과 같은 좌표 변환을 적용한다.</summary>
		private static GPoint[] TransformPoints(double x, double y, double width, double height, System.Drawing.Drawing2D.Matrix local, System.Drawing.Drawing2D.Matrix parent, bool parentIsIdentity)
		{
			double w = 0.5 * width;
			double h = 0.5 * height;

			var points = new GPoint[]
			{
				new GPoint(x + w, y + h), // top right
				new GPoint(x + w, y - h), // bottom right
				new GPoint(x - w, y - h), // bottom left
				new GPoint(x - w, y + h) // top left
			};

			return TransformPoints(points, local, parent, parentIsIdentity);
		}

		/// <summary>
		/// 부모 배치 행렬이 평행 이동뿐인지와 이동량 (2026-10-20). Structure 를 펼칠 때 한 번만 읽는다.
		/// U8 처럼 SREF 가 평행 이동뿐인 파일은 도형마다 GDI+ Matrix.TransformPoints 를 부르지 않고 직접 더한다(펼치기 시간 대부분이 이 호출이었음).
		/// </summary>
		private struct ParentTranslation
		{
			public bool IsTranslation;
			public float Dx;
			public float Dy;

			public static ParentTranslation From(System.Drawing.Drawing2D.Matrix matrix)
			{
				float[] e = matrix.Elements; // m11, m12, m21, m22, dx, dy
				return new ParentTranslation
				{
					IsTranslation = e[0] == 1f && e[1] == 0f && e[2] == 0f && e[3] == 1f,
					Dx = e[4],
					Dy = e[5]
				};
			}
		}

		/// <summary>
		/// 평행 이동만 적용한다. GDI+ Matrix.TransformPoints 와 비트 단위로 같은 결과를 낸다 (2026-10-20):
		/// GDI+ 는 float 좌표 + float 이동량의 정확한 합을 아래쪽(-무한대 방향)으로 내려 float 로 만든다(일반 float 덧셈과 동률 처리가 다름, 30만 회 비교로 확인).
		/// 두 float 의 합은 double 로 정확히 표현되므로 double 로 더한 뒤 float 값이 합보다 크면 한 칸 내린다.
		/// </summary>
		private static GPoint[] TranslatePoints(GPoint[] src, ParentTranslation translation)
		{
			var result = new GPoint[src.Length];
			for (int i = 0; i < src.Length; i++)
				result[i] = new GPoint(AddRoundDown((float)src[i].X, translation.Dx), AddRoundDown((float)src[i].Y, translation.Dy));
			return result;
		}

		/// <summary>두 float 의 정확한 합을 아래쪽으로 내린 float 값</summary>
		private static unsafe float AddRoundDown(float value, float offset)
		{
			double sum = (double)value + offset;
			float result = (float)sum;
			if (result > sum)
			{
				if (result == 0f) return -float.Epsilon;
				int bits = *(int*)&result;
				bits += result > 0f ? -1 : 1;
				result = *(float*)&bits;
			}
			return result;
		}

		/// <summary>도형 자체 변환이 있을 때만 적용한 뒤 부모 구조의 배치 변환을 적용한다.</summary>
		private static GPoint[] TransformPoints(GPoint[] src, System.Drawing.Drawing2D.Matrix local, System.Drawing.Drawing2D.Matrix parent, bool parentIsIdentity)
		{
			// 파싱이 끝난 원본 좌표는 이후 수정하지 않는다. 변환이 없으면 배열 생성과 복사를 생략한다.
			if (local == null && parentIsIdentity)
				return src;

			var a = new PointF[src.Length];

			for (int i = 0; i < src.Length; i++)
				a[i] = new PointF((float)src[i].X, (float)src[i].Y);

			if (local != null)
				local.TransformPoints(a);
			parent.TransformPoints(a);

			var r = new GPoint[a.Length];

			for (int i = 0; i < a.Length; i++)
				r[i] = new GPoint(a[i].X, a[i].Y);

			return r;
		}

		// ---- OpenGL Rendering -------------------------------------------------------------

		protected override void OnPaint(PaintEventArgs e)
		{
			if (DesignMode)
			{
				e.Graphics.Clear(SystemColors.Control);
				var text = nameof(GdsMapControl);
				var width = e.Graphics.MeasureString(text, Font).Width;
				e.Graphics.DrawString(nameof(GdsMapControl), Font, Brushes.Black, (Width - width) / 2f, Height / 2f, StringFormat.GenericDefault);
				return;
			}

			if (!_glInitialized) return;
			long frameStart = Stopwatch.GetTimestamp();
			long firstDrawStart = _loadFramePending ? Stopwatch.GetTimestamp() : 0;

			MakeCurrent();

			int drawW = Math.Max(1, ClientSize.Width);
			int drawH = Math.Max(1, ClientSize.Height);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			bool reportFirstFrame = _firstFrameProgressPending;
			int drawTotal = _layerList.Where(layer => layer.Visible).Sum(layer => layer.Items.Count) + _defectList.Count;
			int drawProcessed = 0;
			if (reportFirstFrame)
				ReportRenderProgress("화면 그리기 중", 0, false, false, true);

			long mapDrawEnd = frameStart;
			if (_gpuBufferReady)
			{
				GL.UseProgram(_shaderProgram);

				// Compute MVP Ortho Projection
				float halfW = (float)(drawW / (2.0 * _scale));
				float halfH = (float)(drawH / (2.0 * _scale));

				Matrix4 projection = Matrix4.CreateOrthographicOffCenter(
					_offset.X - halfW, _offset.X + halfW,
					_offset.Y - halfH, _offset.Y + halfH,
					-1.0f, 1.0f);

				GL.UniformMatrix4(_uMatrixLoc, false, ref projection);

				// Layer 마다 채우기 1번 + 외곽선 1~2번만 호출한다(도형마다 호출하면 U8 제품 Cell 은 프레임당 360만 번) (2026-10-20)
				foreach (var layer in _layerList)
				{
					if (!layer.Visible)
						continue;

					LayerGpuBuffer buffer;
					if (_layerBuffers.TryGetValue(layer.LayerKey, out buffer))
						DrawLayerBuffer(buffer, ColorAlpha / 255f);

					if (reportFirstFrame)
					{
						drawProcessed += layer.Items.Count;
						ReportDrawProgress(drawProcessed, drawTotal);
					}
				}

				// 뒤에 그린 Layer가 이미 선택된 Chain 객체를 덮지 않도록 선택 객체를 마지막에 다시 그린다.
				DrawSelectedItemsOnTop();

				// 결함은 불투명 빨간색으로 채운다.
				if (_defectBuffer != null && _defectList.Count > 0)
				{
					DrawLayerBuffer(_defectBuffer, 1f);
					if (reportFirstFrame)
					{
						drawProcessed += _defectList.Count;
						ReportDrawProgress(drawProcessed, drawTotal);
					}
				}
				GL.BindVertexArray(0);

				// Map과 같은 투영 행렬로 모든 Chain Element 외곽선을 그린다.
				mapDrawEnd = Stopwatch.GetTimestamp();
				DrawChainElementOverlaysGl(ref projection);
			}

			long chainDrawEnd = Stopwatch.GetTimestamp();
			SwapBuffers();
			long swapEnd = Stopwatch.GetTimestamp();
			DrawTextLabels();
			DrawOriginCross();
			DrawChainSetupOverlay();
			// KLARF Defect 표시(Die 상세 분석 화면). 표시할 Defect가 없으면 바로 끝난다.
			DrawDefectOverlay();
			ReportFrameTiming(frameStart, mapDrawEnd, chainDrawEnd, swapEnd);
			if (reportFirstFrame)
			{
				_firstFrameProgressPending = false;
				ReportRenderProgress("화면 그리기 완료", 100, false, true, true);
			}
			if (_loadFramePending)
			{
				_loadFramePending = false;
				_loadMetrics.FirstDrawMs = ElapsedMilliseconds(firstDrawStart);
				FirstFrameMeasured?.Invoke(this, _loadMetrics);
			}

			if (_isDragTracking)
			{
				var left = Math.Min(_rubberStartScreen.X, _rubberCurrentScreen.X);
				var top = Math.Min(_rubberStartScreen.Y, _rubberCurrentScreen.Y);
				var right = Math.Max(_rubberStartScreen.X, _rubberCurrentScreen.X);
				var bottom = Math.Max(_rubberStartScreen.Y, _rubberCurrentScreen.Y);
				var rect = RectangleF.FromLTRB(left, top, right, bottom);

				// CreateGraphics()로 GL 위에 GDI 그리기 (간단한 오버레이)
				using (var g = CreateGraphics())
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					// 약간의 반투명 채우기 (선택사항)
					using (var fill = new SolidBrush(Color.FromArgb(40, Color.DodgerBlue)))
						g.FillRectangle(fill, rect);
					// 점선 외곽선
					using (var pen = new Pen(Color.DodgerBlue, 1f) { DashStyle = DashStyle.Dash })
						g.DrawRectangle(pen, Rectangle.Round(rect));
				}
			}
		}

		/// <summary>
		/// 디버그 출력에 Map/Chain CPU 제출 시간과 버퍼 교체/GDI 시간을 1초 간격으로 기록한다.
		/// GL 명령은 비동기이므로 Map/Chain 수치는 GPU 실행 시간이 아닌 CPU 호출 시간이다.
		/// </summary>
		private void ReportFrameTiming(long frameStart, long mapDrawEnd, long chainDrawEnd, long swapEnd)
		{
			int now = Environment.TickCount;
			if (unchecked(now - _lastFrameTimingLogTick) < 1000) return;
			_lastFrameTimingLogTick = now;
			double mapSubmitMs = (mapDrawEnd - frameStart) * 1000.0 / Stopwatch.Frequency;
			double chainSubmitMs = (chainDrawEnd - mapDrawEnd) * 1000.0 / Stopwatch.Frequency;
			double swapMs = (swapEnd - chainDrawEnd) * 1000.0 / Stopwatch.Frequency;
			double gdiMs = ElapsedMilliseconds(swapEnd);
			Debug.WriteLine(string.Format(
				"GDS FRAME MapSubmit={0:F1}ms ChainSubmit={1:F1}ms Swap={2:F1}ms GDI={3:F1}ms ChainVertices={4} Ranges={5}",
				mapSubmitMs, chainSubmitMs, swapMs, gdiMs,
				_chainOverlayVertices.Count, _chainOverlayDrawRanges.Count));
		}

		/// <summary>
		/// 모든 GDS Layer를 그린 뒤 보이는 선택 Element만 다시 그려 선택 색상이 뒤쪽 Layer에 가리지 않게 한다.
		/// 선택 항목의 기존 GPU 정점을 재사용하므로 후보 계산과 Element 수는 바꾸지 않는다.
		/// </summary>
		private void DrawSelectedItemsOnTop()
		{
			if (_selectedItems.Count == 0)
				return;

			var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible)
				.Select(layer => layer.LayerKey));
			GL.Uniform1(_uAlphaLoc, 1f);
			int boundLayer = int.MinValue;
			foreach (var item in _selectedItems)
			{
				LayerGpuBuffer buffer;
				if (!visibleLayers.Contains(item.LayerKey) || !_layerBuffers.TryGetValue(item.LayerKey, out buffer) || !buffer.Uploaded)
					continue;
				if (boundLayer != item.LayerKey)
				{
					GL.BindVertexArray(buffer.Vao);
					boundLayer = item.LayerKey;
				}
				if (item.FillIndexCount > 0)
					GL.DrawElements(PrimitiveType.Triangles, item.FillIndexCount, DrawElementsType.UnsignedInt, new IntPtr((long)item.FillIndexOffset * sizeof(int)));
				if (item.LineCount > 0)
					GL.DrawArrays(item.LineLoop ? PrimitiveType.LineLoop : PrimitiveType.LineStrip, item.LineFirst, item.LineCount);
			}
		}

		/// <summary>Layer 버퍼 하나를 그린다: 채우기(인덱스 삼각형, 투명도 fillAlpha) -> 닫힌 외곽선(LineLoop) / 열린 선(LineStrip) 불투명.</summary>
		private void DrawLayerBuffer(LayerGpuBuffer buffer, float fillAlpha)
		{
			if (!buffer.Uploaded || buffer.Vertices.Count == 0)
				return;
			GL.BindVertexArray(buffer.Vao);
			if (buffer.FillIndexCount > 0)
			{
				GL.Uniform1(_uAlphaLoc, fillAlpha);
				GL.DrawElements(PrimitiveType.Triangles, buffer.FillIndexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
			}
			GL.Uniform1(_uAlphaLoc, 1f);
			if (buffer.LoopFirstArray.Length > 0)
				GL.MultiDrawArrays(PrimitiveType.LineLoop, buffer.LoopFirstArray, buffer.LoopCountArray, buffer.LoopFirstArray.Length);
			if (buffer.StripFirstArray.Length > 0)
				GL.MultiDrawArrays(PrimitiveType.LineStrip, buffer.StripFirstArray, buffer.StripCountArray, buffer.StripFirstArray.Length);
		}

		/// <summary>
		/// 월드 좌표 (0, 0)을 화면 좌표로 변환하여 얇은 초록색 십자가로 표시한다.
		/// 십자가 길이는 화면 픽셀 기준으로 고정해 Zoom 변경 중에도 원점 표식이 과도하게 커지지 않게 한다.
		/// </summary>
		private void DrawOriginCross()
		{
			if (!ShowOriginCross) return;

			PointF center = WorldToScreen(new GPoint(0, 0));
			if (center.X < 0 || center.Y < 0 || center.X > ClientSize.Width || center.Y > ClientSize.Height)
				return;

			const float armLength = 12f;
			using (Graphics graphics = CreateGraphics())
			using (var pen = new Pen(Color.LimeGreen, 1f))
			{
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.DrawLine(pen, center.X - armLength, center.Y, center.X + armLength, center.Y);
				graphics.DrawLine(pen, center.X, center.Y - armLength, center.X, center.Y + armLength);
			}
		}

		/// <summary>화면 그리기 중에는 128개 단위로만 진행률을 갱신한다.</summary>
		private void ReportDrawProgress(int drawProcessed, int drawTotal)
		{
			if (drawProcessed % 128 != 0 && drawProcessed != drawTotal) return;
			int percent = drawTotal == 0 ? 100 : drawProcessed * 100 / drawTotal;
			ReportRenderProgress("화면 그리기 중 / " + drawProcessed + "개", percent, false, false, drawProcessed == drawTotal);
		}

		// ---- Navigation & Interactive Math ----------------------------------------------

		public void ZoomToFit()
		{
			// 실제 GL 드로어블/클라이언트 크기 사용
			int drawW = Math.Max(1, ClientSize.Width);
			int drawH = Math.Max(1, ClientSize.Height);

			// 전체 경계 계산
			GBox b = GBox.Empty;

			foreach (var layer in _layerList)
			{
				if (!layer.Visible || layer.Bounds.IsEmpty)
					continue;

				b.Include(layer.Bounds);
			}

			if (b.IsEmpty)
			{
				_scale = 1;
				SetTextLabelFitScale();
				_offset = Vector2.Zero;
				Invalidate();
				ZoomChanged?.Invoke(this, _scale);

				return;
			}

			// 화면 여유(margin)를 양쪽(좌/우, 상/하)으로 적용
			const double margin = 10.0;
			double availW = Math.Max(1.0, drawW - margin * 2.0);
			double availH = Math.Max(1.0, drawH - margin * 2.0);

			// world -> pixel 스케일 계산 (pixels per world unit)
			double sx = availW / Math.Max(b.Width, 1e-9);
			double sy = availH / Math.Max(b.Height, 1e-9);

			// 원점이 도면 안에 있는 파일(U9 / OMM_TCB 등)은 지금까지처럼 원점 중심 + 3배로 연다.
			// 원점이 도면 밖이거나 경계에 있는 파일(U8: 0 ~ 23.7mm / 0 ~ 29.8mm)은 원점 중심이면 도면 모서리만 보여 도면 중심에 전체를 맞춘다 (2026-10-20)
			bool originInside = b.MinX < 0 && b.MaxX > 0 && b.MinY < 0 && b.MaxY > 0;
			_scale = Math.Min(sx, sy) * (originInside ? 3f : 1f);
			_scale = Clamp(_scale, MinScale, MaxScale);

			if (_scale <= 0 || double.IsNaN(_scale) || double.IsInfinity(_scale)) _scale = 1;
			SetTextLabelFitScale();

			double cx = originInside ? 0 : b.MinX + b.Width * 0.5;
			double cy = originInside ? 0 : b.MinY + b.Height * 0.5;
			_offset = new Vector2((float)cx, (float)cy);

			Invalidate();
			ZoomChanged?.Invoke(this, _scale);
		}

		private PointF WorldToScreen(GPoint p)
		{
			float x = (float)((p.X - _offset.X) * _scale + Width / 2.0);
			float y = (float)(Height / 2.0 - (p.Y - _offset.Y) * _scale);
			return new PointF(x, y);
		}

		private GPoint ScreenToWorld(PointF p)
		{
			double x = (p.X - Width / 2.0) / _scale + _offset.X;
			double y = (Height / 2.0 - p.Y) / _scale + _offset.Y;
			return new GPoint(x, y);
		}

		private void ZoomAt(PointF screenPt, double factor)
		{
			var worldBefore = ScreenToWorld(screenPt);
			double newScale = Clamp(_scale * factor, MinScale, MaxScale);
			if (Math.Abs(newScale - _scale) < double.Epsilon) return;

			_scale = newScale;
			var worldAfter = ScreenToWorld(screenPt);

			_offset.X += (float)(worldBefore.X - worldAfter.X);
			_offset.Y += (float)(worldBefore.Y - worldAfter.Y);

			Invalidate();
			ZoomChanged?.Invoke(this, _scale);
		}

		private void ClearSelectionInternal()
		{
			foreach (var item in _selectedItems)
				item.Selected = false;

			_selectedItems.Clear();
		}

		private void RaiseSelectionChanged()
		{
			SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!IsDisposed && _glInitialized)
				{
					MakeCurrent();
					ReleaseChainOverlayGlResources();
					ReleaseGpuBuffers();
					GL.DeleteProgram(_shaderProgram);
				}
			}

			base.Dispose(disposing);
		}

		public static double Clamp(double value, double min, double max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		public GdsStructure Structure { get; private set; }
		public int ColorAlpha { get; set; } = 110;
	}
}
