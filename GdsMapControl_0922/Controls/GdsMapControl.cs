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
		public double SceneInsertMs { get; internal set; }
		public double VertexBuildMs { get; internal set; }
		public double GpuUploadMs { get; internal set; }
		public double FirstDrawMs { get; internal set; }
		public int SceneItemCount { get; internal set; }
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
		private struct Vertex
		{
			public Vector2 Position;
			public Vector4 Color;
			public float IsSelected; // 1.0f = selected, 0.0f = normal

			public Vertex(Vector2 pos, Color col, bool selected = false)
			{
				Position = pos;
				Color = new Vector4(col.R / 255f, col.G / 255f, col.B / 255f, col.A / 255f);
				IsSelected = selected ? 1.0f : 0.0f;
			}
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

		private ContextMenuStrip _contextMenu;

		// OpenGL Resources
		private int _vao;
		private int _vbo;
		private int _shaderProgram;
		private int _uMatrixLoc;
		private bool _glInitialized;
		private List<Vertex> _gpuVertices = new List<Vertex>(50000000);
		private bool _gpuBufferReady;
		private Vertex[] _partialUploadChunk;
		private int _lastRenderProgressTick;
		private bool _firstFrameProgressPending;
		private bool _loadFramePending;
		private GdsMapLoadMetrics _loadMetrics;
		private long _sceneInsertTicks;

		// GLSL Shaders
		private const string VertexShaderCode = @"
			#version 330 core
			layout(location = 0) in vec2 aPos;
			layout(location = 1) in vec4 aColor;
			layout(location = 2) in float aSelected;

			uniform mat4 uMatrix;
			out vec4 vColor;

			void main() {
				gl_Position = uMatrix * vec4(aPos, 0.0, 1.0);
				if (aSelected > 0.5) {
					vColor = vec4(1.0, 0.92, 0.23, 1.0); // Highlight Yellow
				} else {
					vColor = aColor;
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

		public GdsMapControl()
		{
			InitializeContextMenu();
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

		public IEnumerable<GdsElement> GetSelectedElements()
		{
			return _selectedItems.Select(a => a.Source);
		}

		private void InitializeContextMenu()
		{
			_contextMenu = new ContextMenuStrip();
			var viewModeItem = new ToolStripMenuItem("보기 모드 (View)", null, (s, e) => Mode = ViewMode.View);
			var selectModeItem = new ToolStripMenuItem("선택 모드 (Select)", null, (s, e) => Mode = ViewMode.Select);

			_contextMenu.Items.Add(viewModeItem);
			_contextMenu.Items.Add(selectModeItem);
			_contextMenu.Items.Add(new ToolStripSeparator());
			_contextMenu.Items.Add(new ToolStripMenuItem("전체 보기 (Zoom to Fit)", null, (s, e) => ZoomToFit()));
			_contextMenu.Items.Add(new ToolStripMenuItem("선택 해제 (Clear Selection)", null, (s, e) =>
			{
				var changedItems = _selectedItems.ToArray();
				ClearSelectionInternal();
				UpdateSelectionVertices(changedItems);
				RaiseSelectionChanged();
				Invalidate();
			}));

			_contextMenu.Opening += (s, e) =>
			{
				viewModeItem.Checked = _mode == ViewMode.View;
				selectModeItem.Checked = _mode == ViewMode.Select;
			};
			ContextMenuStrip = _contextMenu;
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

			// Buffer allocations
			_vao = GL.GenVertexArray();
			_vbo = GL.GenBuffer();

			GL.BindVertexArray(_vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

			int stride = sizeof(float) * 7; // Vector2(2) + Vector4(4) + float(1)

			// Position Attribute
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
			GL.EnableVertexAttribArray(0);

			// Color Attribute
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);
			GL.EnableVertexAttribArray(1);

			// Selected Attribute
			GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, stride, sizeof(float) * 6);
			GL.EnableVertexAttribArray(2);

			_glInitialized = true;
			// 컨트롤 초기화 전에 도면을 받은 경우에도 한 번만 전체 정점을 업로드한다.
			if (Structure != null)
				UploadAllGpuVertices();
		}

		// ---- GDS Structure Loading & Triangulation --------------------------------------

		public void ShowStructure(GdsLibrary lib)
		{
			if (lib.Structures.Count > 0)
				ShowStructure(lib, lib.Structures[0].Name);
		}

		public void ShowStructure(GdsLibrary lib, string structureName)
		{
			_loadMetrics = new GdsMapLoadMetrics();
			_sceneInsertTicks = 0;
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
					FlattenStructure(lib, str, identity, new HashSet<string>(StringComparer.OrdinalIgnoreCase), 0, str.Name);
				ApplyInitialLayerColors();
				_loadMetrics.FlattenMs = ElapsedMilliseconds(flattenStart);
				_loadMetrics.SceneInsertMs = _sceneInsertTicks * 1000.0 / Stopwatch.Frequency;
				CalculateTextLabelDisplayAreas();
				// GDS TEXT에는 특정 도형 소유 관계가 없으므로 주변 도형 검색으로 표시 여부를 제한하지 않는다.
			}

			BuildGpuBuffers();
			RaiseSelectionChanged();
			// 첫 화면 측정은 새 도면에서만 수행한다. Layer 변경으로 인한 다시 그리기는 제외한다.
			_loadFramePending = true;
			ZoomToFit();
		}

		/// <summary>새 도면의 모든 Layer를 표시 여부와 무관하게 고정 VBO 위치로 구성한다.</summary>
		private void BuildGpuBuffers()
		{
			_gpuBufferReady = false;
			_gpuVertices.Clear();
			int totalItems = _layerList.Sum(layer => layer.Items.Count) + _defectList.Count;
			int processedItems = 0;
			ReportRenderProgress("GPU 버퍼 생성 중", 0, false, false, true);
			long vertexStart = Stopwatch.GetTimestamp();

			foreach (var layer in _layerList)
			{
				layer.VertexOffset = _gpuVertices.Count;
					Color color = layer.Color;

					foreach (var item in layer.Items)
					{
						item.FillVertexOffset = -1;
						item.FillVertexCount = 0;
						item.LineVertexOffset = -1;
						item.LineVertexCount = 0;
						if (item.Source is GdsPath filledPath && item.Width > 0 && item.WorldPoints.Length > 0)
						{
							// GDS PATH의 폭을 실제 면으로 만든다. 한 좌표만 가진 PATH도 패드로 표시한다.
							item.FillVertexOffset = _gpuVertices.Count;
							AddPathFillVertices(item, filledPath, color);
							item.FillVertexCount = _gpuVertices.Count - item.FillVertexOffset;
						}
						else if (item.Closed && item.WorldPoints.Length >= 3)
						{
							// ... (기존 삼각화 / 라인 추가 코드 유지) ...
							item.FillVertexOffset = _gpuVertices.Count;

							for (int i = 1; i < item.WorldPoints.Length - 1; i++)
							{
								_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[0].X, (float)item.WorldPoints[0].Y), Color.FromArgb(ColorAlpha, color), item.Selected));
								_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[i].X, (float)item.WorldPoints[i].Y), Color.FromArgb(ColorAlpha, color), item.Selected));
								_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[i + 1].X, (float)item.WorldPoints[i + 1].Y), Color.FromArgb(ColorAlpha, color), item.Selected));
							}
							item.FillVertexCount = _gpuVertices.Count - item.FillVertexOffset;

							item.LineVertexOffset = _gpuVertices.Count;
							for (int i = 0; i < item.WorldPoints.Length; i++)
							{
								var p1 = item.WorldPoints[i];
								var p2 = item.WorldPoints[(i + 1) % item.WorldPoints.Length];
								_gpuVertices.Add(new Vertex(new Vector2((float)p1.X, (float)p1.Y), color, item.Selected));
								_gpuVertices.Add(new Vertex(new Vector2((float)p2.X, (float)p2.Y), color, item.Selected));
							}
							item.LineVertexCount = _gpuVertices.Count - item.LineVertexOffset;
						}
						else if (item.WorldPoints.Length >= 2)
						{
							item.LineVertexOffset = _gpuVertices.Count;
							for (int i = 0; i < item.WorldPoints.Length - 1; i++)
							{
								var p1 = item.WorldPoints[i];
								var p2 = item.WorldPoints[i + 1];
								_gpuVertices.Add(new Vertex(new Vector2((float)p1.X, (float)p1.Y), color, item.Selected));
								_gpuVertices.Add(new Vertex(new Vector2((float)p2.X, (float)p2.Y), color, item.Selected));
							}
							item.LineVertexCount = _gpuVertices.Count - item.LineVertexOffset;
						}
						ReportBufferBuildProgress(++processedItems, totalItems);
					}
				layer.VertexCount = _gpuVertices.Count - layer.VertexOffset;
			}

			// defect
			foreach (var item in _defectList)
			{
				item.FillVertexOffset = _gpuVertices.Count;

				for (int i = 1; i < item.WorldPoints.Length - 1; i++)
				{
					_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[0].X, (float)item.WorldPoints[0].Y), Color.FromArgb(255, DefectColor)));
					_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[i].X, (float)item.WorldPoints[i].Y), Color.FromArgb(255, DefectColor)));
					_gpuVertices.Add(new Vertex(new Vector2((float)item.WorldPoints[i + 1].X, (float)item.WorldPoints[i + 1].Y), Color.FromArgb(255, DefectColor)));
				}

				item.FillVertexCount = _gpuVertices.Count - item.FillVertexOffset;
				item.LineVertexOffset = _gpuVertices.Count;

				//for (int i = 0; i < item.WorldPoints.Length; i++)
				//{
				//	var p1 = item.WorldPoints[i];
				//	var p2 = item.WorldPoints[(i + 1) % item.WorldPoints.Length];
				//	_gpuVertices.Add(new Vertex(new Vector2((float)p1.X, (float)p1.Y), DefectColor));
				//	_gpuVertices.Add(new Vertex(new Vector2((float)p2.X, (float)p2.Y), DefectColor));
				//}
				//item.LineVertexCount = _gpuVertices.Count - item.LineVertexOffset;
				ReportBufferBuildProgress(++processedItems, totalItems);
			}

			_loadMetrics.VertexBuildMs = ElapsedMilliseconds(vertexStart);
			_loadMetrics.VertexCount = _gpuVertices.Count;
			long uploadStart = Stopwatch.GetTimestamp();
			UploadAllGpuVertices();
			_loadMetrics.GpuUploadMs = ElapsedMilliseconds(uploadStart);
			ReportRenderProgress("GPU 버퍼 생성 완료", 100, false, false, true);
			// 다음 OnPaint에서 실제 DrawArrays 실행 진행률을 이어서 보고한다.
			_firstFrameProgressPending = true;
		}

		/// <summary>
		/// PATH 중심선과 폭으로 삼각형 면을 만든다. 1점 PATH는 원형/사각형 패드로,
		/// 여러 점은 각 선분의 사각형과 꺾이는 위치의 연결 삼각형으로 표시한다.
		/// </summary>
		private void AddPathFillVertices(GlSceneItem item, GdsPath path, Color layerColor)
		{
			float radius = (float)(item.Width / 2.0);
			if (radius <= 0) return;
			Color fillColor = Color.FromArgb(ColorAlpha, layerColor);
			GPoint[] points = item.WorldPoints;
			if (points.Length == 1)
			{
				AddPathPad(new Vector2((float)points[0].X, (float)points[0].Y), radius, path.PathType == 1, fillColor, item.Selected);
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
						AddPathTriangle(start, start + previousNormal * side, start + normal * side, fillColor, item.Selected);
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
				AddPathTriangle(drawStart + normal, drawStart - normal, drawEnd + normal, fillColor, item.Selected);
				AddPathTriangle(drawStart - normal, drawEnd - normal, drawEnd + normal, fillColor, item.Selected);
				previousDirection = direction;
				previousNormal = normal;
				lastPoint = end;
				hasSegment = true;
			}

			if (!hasSegment)
				AddPathPad(new Vector2((float)points[0].X, (float)points[0].Y), radius, path.PathType == 1, fillColor, item.Selected);
			else if (path.PathType == 1)
			{
				AddPathPad(firstPoint, radius, true, fillColor, item.Selected);
				AddPathPad(lastPoint, radius, true, fillColor, item.Selected);
			}
		}

		/// <summary>한 좌표의 PATH를 폭만큼 채운다. Round 타입은 원, 나머지는 정사각형으로 표시한다.</summary>
		private void AddPathPad(Vector2 center, float radius, bool round, Color color, bool selected)
		{
			if (!round)
			{
				var x = new Vector2(radius, 0);
				var y = new Vector2(0, radius);
				AddPathTriangle(center - x - y, center + x - y, center + x + y, color, selected);
				AddPathTriangle(center - x - y, center + x + y, center - x + y, color, selected);
				return;
			}
			const int segments = 16;
			for (int i = 0; i < segments; i++)
			{
				float angle1 = (float)(2.0 * Math.PI * i / segments);
				float angle2 = (float)(2.0 * Math.PI * (i + 1) / segments);
				var point1 = center + new Vector2((float)Math.Cos(angle1) * radius, (float)Math.Sin(angle1) * radius);
				var point2 = center + new Vector2((float)Math.Cos(angle2) * radius, (float)Math.Sin(angle2) * radius);
				AddPathTriangle(center, point1, point2, color, selected);
			}
		}

		/// <summary>PATH 채움 삼각형 하나를 기존 GPU 정점 목록에 추가한다.</summary>
		private void AddPathTriangle(Vector2 a, Vector2 b, Vector2 c, Color color, bool selected)
		{
			_gpuVertices.Add(new Vertex(a, color, selected));
			_gpuVertices.Add(new Vertex(b, color, selected));
			_gpuVertices.Add(new Vertex(c, color, selected));
		}

		/// <summary>새 도면에서만 정점 전체를 업로드한다. Layer/선택 변경은 부분 업로드를 사용한다.</summary>
		private void UploadAllGpuVertices()
		{
			if (!_glInitialized) return;
			ReportRenderProgress("GPU 업로드 중", 0, true, false, true);
			// 색상 선택 창을 닫은 뒤에도 이 컨트롤의 OpenGL 컨텍스트에 업로드한다.
			MakeCurrent();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

			if (_gpuVertices.Count == 0)
			{
				// 빈 버퍼 처리: 이전 데이터 지우기
				GL.BufferData(BufferTarget.ArrayBuffer, IntPtr.Zero, IntPtr.Zero, BufferUsageHint.DynamicDraw);
			}
			else
			{
				GL.BufferData(BufferTarget.ArrayBuffer, _gpuVertices.Count * sizeof(float) * 7, _gpuVertices.ToArray(), BufferUsageHint.DynamicDraw);
			}
			_gpuBufferReady = true;
			ReportRenderProgress("GPU 업로드 완료", 100, false, false, true);
		}

		/// <summary>Layer 색상만 CPU 정점에 반영하고 그 Layer의 연속 VBO 구간만 전송한다.</summary>
		private void UpdateLayerColorVertices(GlSceneLayer layer)
		{
			Color fillColor = Color.FromArgb(ColorAlpha, layer.Color);
			foreach (var item in layer.Items)
			{
				SetVertexColor(item.FillVertexOffset, item.FillVertexCount, fillColor);
				SetVertexColor(item.LineVertexOffset, item.LineVertexCount, layer.Color);
			}
			if (layer.VertexCount > 0)
				UploadVertexRanges(new List<VertexRange> { new VertexRange(layer.VertexOffset, layer.VertexCount) });
		}

		/// <summary>선택 상태가 실제로 바뀐 Item만 CPU 정점과 GPU 구간에 반영한다.</summary>
		private void UpdateSelectionVertices(IEnumerable<GlSceneItem> changedItems)
		{
			var ranges = new List<VertexRange>();
			foreach (var item in changedItems)
			{
				int count = item.FillVertexCount + item.LineVertexCount;
				if (count == 0) continue;
				int start = item.FillVertexCount > 0 ? item.FillVertexOffset : item.LineVertexOffset;
				if (start < 0) continue;
				float selected = item.Selected ? 1.0f : 0.0f;
				for (int i = start; i < start + count; i++)
				{
					var vertex = _gpuVertices[i];
					vertex.IsSelected = selected;
					_gpuVertices[i] = vertex;
				}
				ranges.Add(new VertexRange(start, count));
			}
			UploadVertexRanges(ranges);
		}

		/// <summary>채움/선의 기존 알파 값을 유지하면서 색상 필드만 바꾼다.</summary>
		private void SetVertexColor(int start, int count, Color color)
		{
			if (count == 0 || start < 0) return;
			var rgba = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
			for (int i = start; i < start + count; i++)
			{
				var vertex = _gpuVertices[i];
				vertex.Color = rgba;
				_gpuVertices[i] = vertex;
			}
		}

		/// <summary>인접 변경 구간을 합치고 고정 크기 배열로 나눠 전체 VBO 재전송을 피한다.</summary>
		private void UploadVertexRanges(List<VertexRange> ranges)
		{
			if (!_glInitialized || !_gpuBufferReady || ranges.Count == 0) return;
			ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
			MakeCurrent();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
			const int chunkVertices = 8192;
			const int stride = sizeof(float) * 7;
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
					_gpuVertices.CopyTo(position, _partialUploadChunk, 0, count);
					GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr((long)position * stride), count * stride, _partialUploadChunk);
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

		/// <summary>참조 구조를 재귀적으로 펼쳐 도형을 월드 좌표로 바꾸고 화면용 Layer에 모은다.</summary>
		private void FlattenStructure(GdsLibrary lib, GdsStructure str, System.Drawing.Drawing2D.Matrix parent, HashSet<string> stack, int depth, string structurePath)
		{
			if (depth > 64 || stack.Contains(str.Name)) 
				return;

			stack.Add(str.Name);

			foreach (var layer in str.Layers)
			{
				foreach (var e in layer.Elements)
				{
					if (e is GdsBoundary boundary)
					{
						var pts = TransformGeometryPoints(boundary.Points, e.Transform, parent);

						if (pts.Length >= 2)
							AddMeasuredSceneItem(new GlSceneItem(e, pts, true, 0));
						else
							_layerList.AddLayer(e.LayerID);
					}
					else if (e is GdsPath path)
					{
						var pts = TransformGeometryPoints(path.Points, e.Transform, parent);

						if (pts.Length >= 1)
							AddMeasuredSceneItem(new GlSceneItem(e, pts, false, Math.Abs(path.Width)));
						else
							_layerList.AddLayer(e.LayerID);
					}
					else if (e is GdsText text)
					{
						var pts = TransformTextPosition(text, parent);
						AddMeasuredSceneItem(new GlSceneItem(e, pts, false, 0, text.Text));
						AddTextLabel(text, pts, structurePath);
					}
					else if (e is GdsSRef sref && lib.Structures.TryGetValue(sref.StructureName, out var child))
					{
						using (var local = GdsGeometry.CreateTransform(e.Transform))
						using (var m = (System.Drawing.Drawing2D.Matrix)local.Clone())
						{
							m.Translate((float)sref.Origin.X, (float)sref.Origin.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
							using (var combined = (System.Drawing.Drawing2D.Matrix)parent.Clone())
							{
								combined.Multiply(m, System.Drawing.Drawing2D.MatrixOrder.Append);
								string childPath = structurePath + " > " + child.Name + " @ (" + sref.Origin.X + ", " + sref.Origin.Y + ")";
								FlattenStructure(lib, child, combined, stack, depth + 1, childPath);
							}
						}
					}
				}
			}

			// defect
			foreach (var defect in str.DefectList)
			{
				var pts = TransformPoints(defect.X, defect.Y, defect.Width, defect.Height, null, parent);
				_defectList.Add(new GlDefectItem(defect, pts));
			}

			stack.Remove(str.Name);
		}

		/// <summary>재귀 참조를 모두 펼친 뒤 Layer 색상을 한 번만 적용해 중복 순회를 없앤다.</summary>
		private void ApplyInitialLayerColors()
		{
			foreach (var layer in _layerList)
				layer.Color = GetLayerColor(layer.LayerID);
		}

		/// <summary>Flatten 내부에서 Layer 검색과 도형 등록이 차지하는 시간을 별도로 합산한다.</summary>
		private void AddMeasuredSceneItem(GlSceneItem item)
		{
			long start = Stopwatch.GetTimestamp();
			_layerList.AddSceneItem(item);
			_sceneInsertTicks += Stopwatch.GetTimestamp() - start;
			_loadMetrics.SceneItemCount++;
		}

		/// <summary>Stopwatch 원시 타임스탬프를 밀리초로 변환한다.</summary>
		private static double ElapsedMilliseconds(long start)
		{
			return (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;
		}

		/// <summary>
		/// GDS TEXT의 XY는 문자열 삽입점이므로 부모 구조의 배치 변환만 적용한다.
		/// TEXT 자체 MAG/ANGLE/Mirror는 삽입점을 중심으로 글자 모양에 적용되는 값이므로 좌표에는 적용하지 않는다.
		/// </summary>
		private static GPoint[] TransformTextPosition(GdsText text, System.Drawing.Drawing2D.Matrix parent)
		{
			return TransformPoints(new GPoint[] { text.Position }, null, parent);
		}

		/// <summary>회전/배율/반전이 없는 도형에는 동일한 좌표 결과를 내는 부모 변환만 적용한다.</summary>
		private static GPoint[] TransformGeometryPoints(GPoint[] points, GTransform transform, System.Drawing.Drawing2D.Matrix parent)
		{
			if (!transform.MirrorX && transform.Rotation == 0 && (transform.Magnification == 0 || transform.Magnification == 1))
				return TransformPoints(points, null, parent);

			using (var local = GdsGeometry.CreateTransform(transform))
				return TransformPoints(points, local, parent);
		}

		/// <summary>결함 사각형의 네 꼭짓점을 만들어 일반 도형과 같은 좌표 변환을 적용한다.</summary>
		private static GPoint[] TransformPoints(double x, double y, double width, double height, System.Drawing.Drawing2D.Matrix local, System.Drawing.Drawing2D.Matrix parent)
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

			return TransformPoints(points, local, parent);
		}

		/// <summary>도형 자체 변환이 있을 때만 적용한 뒤 부모 구조의 배치 변환을 적용한다.</summary>
		private static GPoint[] TransformPoints(GPoint[] src, System.Drawing.Drawing2D.Matrix local, System.Drawing.Drawing2D.Matrix parent)
		{
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

			if (_gpuVertices.Count > 0)
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
				GL.BindVertexArray(_vao);

				foreach (var layer in _layerList)
				{
					if (!layer.Visible)
						continue;

					foreach (var item in layer.Items)
					{
						if (item.FillVertexCount > 0)
							GL.DrawArrays(PrimitiveType.Triangles, item.FillVertexOffset, item.FillVertexCount);

						if (item.LineVertexCount > 0)
							GL.DrawArrays(PrimitiveType.Lines, item.LineVertexOffset, item.LineVertexCount);

						if (reportFirstFrame)
							ReportDrawProgress(++drawProcessed, drawTotal);
					}
				}

				foreach (var item in _defectList)
				{
					if (item.FillVertexCount > 0)
						GL.DrawArrays(PrimitiveType.Triangles, item.FillVertexOffset, item.FillVertexCount);

					if (item.LineVertexCount > 0)
						GL.DrawArrays(PrimitiveType.Lines, item.LineVertexOffset, item.LineVertexCount);

					if (reportFirstFrame)
						ReportDrawProgress(++drawProcessed, drawTotal);
				}
			}

			SwapBuffers();
			DrawTextLabels();
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
			
			_scale = Math.Min(sx, sy) * 3f;
			_scale = Clamp(_scale, MinScale, MaxScale);

			if (_scale <= 0 || double.IsNaN(_scale) || double.IsInfinity(_scale)) _scale = 1;
			SetTextLabelFitScale();

			// 월드 중심을 화면 중앙에 맞춤
			double cx = 0;// b.MinX + b.Width * 0.5;
			double cy = 0;// b.MinY + b.Height * 0.5;
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
					GL.DeleteBuffer(_vbo);
					GL.DeleteVertexArray(_vao);
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
