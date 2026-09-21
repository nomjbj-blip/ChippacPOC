using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		// Events
		public event EventHandler<PointF> MouseWorldPositionChanged;
		public event EventHandler SelectionChanged;
		public event EventHandler<ViewMode> ModeChanged;
		public event EventHandler<double> ZoomChanged;
		/// <summary>도면 구성, GPU 버퍼 생성, 최초 화면 그리기 단계를 화면에 전달한다.</summary>
		public event EventHandler<GdsMapRenderProgressChangedEventArgs> RenderProgressChanged;

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
		private int _lastRenderProgressTick;
		private bool _firstFrameProgressPending;

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
				ClearSelectionInternal();
				UpdateGpuBuffers();
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
		}

		// ---- GDS Structure Loading & Triangulation --------------------------------------

		public void ShowStructure(GdsLibrary lib)
		{
			if (lib.Structures.Count > 0)
				ShowStructure(lib, lib.Structures[0].Name);
		}

		public void ShowStructure(GdsLibrary lib, string structureName)
		{
			if (lib != null && lib.Structures.TryGetValue(structureName, out var str))
			{
				ReportRenderProgress("도면 구조 생성 중", 0, true, false, true);
				Structure = str;

				// 파일을 다시 열 때 이전 레이어/선택이 새 목록과 섞이지 않도록 도면 상태를 비운다.
				ClearSelectionInternal();
				_layerList.Clear();
				_defectList.Clear();
				_layer775Labels.Clear();

				using (var identity = new System.Drawing.Drawing2D.Matrix())
					FlattenStructure(lib, str, identity, new HashSet<string>(StringComparer.OrdinalIgnoreCase), 0, str.Name);
				CalculateLayer775LabelDisplayAreas();
				// GDS TEXT에는 특정 도형 소유 관계가 없으므로 주변 도형 검색으로 표시 여부를 제한하지 않는다.
			}

			BuildGpuBuffers();
			RaiseSelectionChanged();
			ZoomToFit();
		}

		private void BuildGpuBuffers()
		{
			_gpuVertices.Clear();
			int totalItems = _layerList.Sum(layer => layer.Items.Count) + _defectList.Count;
			int processedItems = 0;
			ReportRenderProgress("GPU 버퍼 생성 중", 0, false, false, true);

			foreach (var layer in _layerList)
			{
				if (layer.Visible)
				{
					Color color = layer.Color;

					foreach (var item in layer.Items)
					{
						if (item.Closed && item.WorldPoints.Length >= 3)
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

				}
				else
				{
					foreach (var item in layer.Items)
					{
						item.FillVertexOffset = -1;
						item.FillVertexCount = 0;
						item.LineVertexOffset = -1;
						item.LineVertexCount = 0;
						ReportBufferBuildProgress(++processedItems, totalItems);
					}
				}
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

			UpdateGpuBuffers();
			ReportRenderProgress("GPU 버퍼 생성 완료", 100, false, false, true);
			// 다음 OnPaint에서 실제 DrawArrays 실행 진행률을 이어서 보고한다.
			_firstFrameProgressPending = true;
		}

		private void UpdateGpuBuffers()
		{
			if (!_glInitialized) return;
			ReportRenderProgress("GPU 업로드 중", 0, true, false, true);
			// 색상 선택 창을 닫은 뒤에도 이 컨트롤의 OpenGL 컨텍스트에 업로드한다.
			MakeCurrent();

			// Sync selections to GPU Vertex Array
			foreach (var layer in _layerList)
			{
				if (!layer.Visible)
					continue;

				foreach (var item in layer.Items)
				{
					// 필터로 스킵된 항목은 건너뜀
					if ((item.FillVertexOffset < 0 && item.LineVertexOffset < 0)) continue;

					float sel = item.Selected ? 1.0f : 0.0f;
					int totalCount = item.FillVertexCount + item.LineVertexCount;
					int start = item.FillVertexCount > 0 ? item.FillVertexOffset : item.LineVertexOffset;

					for (int i = start; i < start + totalCount; i++)
					{
						var v = _gpuVertices[i];
						v.IsSelected = sel;
						_gpuVertices[i] = v;
					}
				}
			}

			foreach (var item in _defectList)
			{
				float sel = 0.0f;
				int totalCount = item.FillVertexCount + item.LineVertexCount;
				int start = item.FillVertexCount > 0 ? item.FillVertexOffset : item.LineVertexOffset;

				for (int i = start; i < start + totalCount; i++)
				{
					var v = _gpuVertices[i];
					v.IsSelected = sel;
					_gpuVertices[i] = v;
				}
			}

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
			ReportRenderProgress("GPU 업로드 완료", 100, false, false, true);
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

		private void FlattenStructure(GdsLibrary lib, GdsStructure str, System.Drawing.Drawing2D.Matrix parent, HashSet<string> stack, int depth, string structurePath)
		{
			if (depth > 64 || stack.Contains(str.Name)) 
				return;

			stack.Add(str.Name);

			foreach (var layer in str.Layers)
			{
				foreach (var e in layer.Elements)
				{
					using (var local = GdsGeometry.CreateTransform(e.Transform))
					{
						if (e is GdsBoundary boundary)
						{
							var pts = TransformPoints(boundary.Points, local, parent);

							if (pts.Length >= 2)
								_layerList.AddSceneItem(new GlSceneItem(e, pts, true, 0));
							else
								_layerList.AddLayer(e.LayerID);
						}
						else if (e is GdsPath path)
						{
							var pts = TransformPoints(path.Points, local, parent);

							if (pts.Length >= 2)
								_layerList.AddSceneItem(new GlSceneItem(e, pts, false, Math.Abs(path.Width)));
							else
								_layerList.AddLayer(e.LayerID);
						}
						else if (e is GdsText text)
						{
							var pts = TransformTextPosition(text, parent);
							_layerList.AddSceneItem(new GlSceneItem(e, pts, false, 0, text.Text));
							AddLayer775Label(text, pts, structurePath);
						}
						else if (e is GdsSRef sref && lib.Structures.TryGetValue(sref.StructureName, out var child))
						{
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
			}

			// defect
			foreach (var defect in str.DefectList)
			{
				using (var local = GdsGeometry.CreateTransform(GTransform.Identity))
				{
					var pts = TransformPoints(defect.X, defect.Y, defect.Width, defect.Height, local, parent);
					_defectList.Add(new GlDefectItem(defect, pts));
				}
			}

			// Set Layer Color
			foreach (var layer in _layerList)
			{
				layer.Color = GetLayerColor(layer.LayerID);
			}

			stack.Remove(str.Name);
		}

		/// <summary>
		/// GDS TEXT의 XY는 문자열 삽입점이므로 부모 구조의 배치 변환만 적용한다.
		/// TEXT 자체 MAG/ANGLE/Mirror는 삽입점을 중심으로 글자 모양에 적용되는 값이므로 좌표에는 적용하지 않는다.
		/// </summary>
		private static GPoint[] TransformTextPosition(GdsText text, System.Drawing.Drawing2D.Matrix parent)
		{
			using (var identity = new System.Drawing.Drawing2D.Matrix())
			{
				return TransformPoints(new GPoint[] { text.Position }, identity, parent);
			}
		}

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

		private static GPoint[] TransformPoints(GPoint[] src, System.Drawing.Drawing2D.Matrix local, System.Drawing.Drawing2D.Matrix parent)
		{
			var a = new PointF[src.Length];

			for (int i = 0; i < src.Length; i++)
				a[i] = new PointF((float)src[i].X, (float)src[i].Y);

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

			MakeCurrent();

			int drawW = Math.Max(1, ClientSize.Width);
			int drawH = Math.Max(1, ClientSize.Height);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			bool reportFirstFrame = _firstFrameProgressPending;
			int drawTotal = _layerList.Where(layer => layer.Visible).Sum(layer => layer.Items.Count) + _defectList.Count;
			int drawProcessed = 0;
			if (reportFirstFrame)
				ReportRenderProgress("초기 화면 그리기 중", 0, false, false, true);

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
			DrawLayer775Labels();
			if (reportFirstFrame)
			{
				_firstFrameProgressPending = false;
				ReportRenderProgress("초기 화면 그리기 완료", 100, false, true, true);
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

		/// <summary>최초 화면 그리기 중에는 128개 단위로만 진행률을 갱신한다.</summary>
		private void ReportDrawProgress(int drawProcessed, int drawTotal)
		{
			if (drawProcessed % 128 != 0 && drawProcessed != drawTotal) return;
			int percent = drawTotal == 0 ? 100 : drawProcessed * 100 / drawTotal;
			ReportRenderProgress("초기 화면 그리기 중 / " + drawProcessed + "개", percent, false, false, drawProcessed == drawTotal);
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
