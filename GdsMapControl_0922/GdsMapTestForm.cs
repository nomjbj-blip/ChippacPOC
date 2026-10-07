using NexplantQMS.GdsMap.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>GDS 파일 읽기와 레이어 표시/색상 변경을 뷰어에 전달하는 화면이다.</summary>
    public partial class GdsMapTestForm : Form
    {
		public static readonly string SAMPLE_FILE = @"D:\PROJECT\스태츠칩팩\Defect E-Test\sample\OMM all layer2\OMM all layer2.gds";
		private readonly Stopwatch _mapLoadWatch = new Stopwatch();
		private readonly Stopwatch _firstFrameWatch = new Stopwatch();
		private GdsMapLoadMetrics _lastLoadMetrics;
		private bool _mapLoadSucceeded;
		private int _lastCoordinateUpdateTick;
		private bool _updatingLayerChecks;
        public GdsMapTestForm()
        {
			InitializeComponent();
			statusStripMap.ShowItemToolTips = true;
			map.RenderProgressChanged += Map_RenderProgressChanged;
			map.FirstFrameMeasured += Map_FirstFrameMeasured;
			map.MouseWorldPositionChanged += Map_MouseWorldPositionChanged;
			chkLayerItems.ItemCheck += ChkLayerItems_ItemCheck;
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			map.OnlySelectSquareItems = true;

			//button1.PerformClick();
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			string path;
			// C# 7.3 환경에서도 파일 선택 창 자원을 자동 해제하기 위해 using 블록을 사용한다.
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = "GDS files (*.gds)|*.gds|All files (*.*)|*.*";
				if (dlg.ShowDialog() != DialogResult.OK) return;
				path = dlg.FileName;
			}

			BeginMapLoad("파일 읽기 준비 중", false);
			try
			{
				var reader = new GdsReader { LengthUnit = LengthUnit.Micrometer };
				reader.ProgressChanged += Reader_ProgressChanged;
				var lib = await Task.Run(() => reader.Read(path));
				reader.ProgressChanged -= Reader_ProgressChanged;
				var str = lib.Structures.First();

				// POC 화면에 표시할 테스트 결함이다.
				str.DefectList.Add(new Defect() { X = 0, Y = 0, Width = 15, Height = 20 });
				str.DefectList.Add(new Defect() { X = 1000, Y = 1500, Width = 30, Height = 20 });
				SetMapStage("도면 구성 및 화면 반영 중", true);
				map.ShowStructure(lib);
				BindElementList(str);
				RefreshLayerList();
				EndMapLoad("완료 / Layer " + map.GetLayerDisplayItems().Count + "개 / " + _mapLoadWatch.Elapsed.TotalSeconds.ToString("0.0") + "초");
			}
			catch (Exception ex)
			{
				EndMapLoad("파일 조회 실패");
				MessageBox.Show(this, ex.Message, "GDS 파일 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (map.Structure == null)
				return;

			// 테스트로 Element만 저장
			foreach (var layer in map.Structure.Layers)
				OracleSaveUtil.SaveElements(layer.Elements);
		}

		private async void btnLoad_Click(object sender, EventArgs e)
		{
			BeginMapLoad("DB 조회 중", true);
			try
			{
				var elements = await Task.Run(() => OracleLoadUtil.LoadElements(2, "TOP"));
				var lib = new GdsLibrary();
				var str = new GdsStructure { Name = "TOP" };
				foreach (var el in elements) str.Layers.AddElement(el);
				lib.Structures.Add(str);
				SetMapStage("DB 도면 구성 및 화면 반영 중", true);
				map.ShowStructure(lib);
				BindElementList(str);
				RefreshLayerList();
				EndMapLoad("완료 / " + elements.Count + "건 / " + _mapLoadWatch.Elapsed.TotalSeconds.ToString("0.0") + "초");
			}
			catch (Exception ex)
			{
				EndMapLoad("DB 조회 실패");
				MessageBox.Show(this, ex.Message, "DB 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>파일/DB 조회 중 조작을 막고 StatusStrip의 진행 상태를 초기화한다.</summary>
		private void BeginMapLoad(string stage, bool marquee)
		{
			_mapLoadWatch.Restart();
			_firstFrameWatch.Restart();
			_lastLoadMetrics = null;
			_mapLoadSucceeded = false;
			button1.Enabled = false;
			btnLoad.Enabled = false;
			btnSave.Enabled = false;
			btnLayer.Enabled = false;
			btnLayerCheckAll.Enabled = false;
			btnLayerCheckNone.Enabled = false;
			SetMapStage(stage, marquee);
		}

		/// <summary>현재 단계만 갱신한다. 파일 읽기는 백그라운드에서 발생하므로 UI 스레드로 전달한다.</summary>
		private void Reader_ProgressChanged(object sender, GdsReadProgressChangedEventArgs e)
		{
			if (IsDisposed || !IsHandleCreated) return;
			BeginInvoke(new Action(() =>
			{
				progressMapLoad.Style = ProgressBarStyle.Continuous;
				progressMapLoad.Visible = true;
				progressMapLoad.Value = Math.Max(progressMapLoad.Minimum, Math.Min(progressMapLoad.Maximum, e.Percent));
				lblMapStatus.Text = "파일 읽기 " + e.Percent + "% / " + FormatBytes(e.BytesRead) + " / " + FormatBytes(e.TotalBytes);
			}));
		}

		/// <summary>진행률을 알 수 없는 DB/도면 구성 단계에는 Marquee를 표시한다.</summary>
		private void SetMapStage(string stage, bool marquee)
		{
			progressMapLoad.Visible = true;
			progressMapLoad.Style = marquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
			if (!marquee) progressMapLoad.Value = 0;
			lblMapStatus.Text = stage;
			statusStripMap.Refresh();
		}

		/// <summary>조회가 끝나면 버튼과 StatusStrip을 정상 상태로 되돌린다.</summary>
		private void EndMapLoad(string message)
		{
			_mapLoadWatch.Stop();
			_mapLoadSucceeded = message.StartsWith("완료", StringComparison.Ordinal);
			if (!_mapLoadSucceeded) _firstFrameWatch.Stop();
			progressMapLoad.Visible = false;
			lblMapStatus.Text = _mapLoadSucceeded && _lastLoadMetrics != null ? FormatLoadMetrics(_lastLoadMetrics) : message;
			button1.Enabled = true;
			btnLoad.Enabled = true;
			btnSave.Enabled = true;
			btnLayer.Enabled = chkLayerItems.Items.Count > 0;
			btnLayerCheckAll.Enabled = chkLayerItems.Items.Count > 0;
			btnLayerCheckNone.Enabled = chkLayerItems.Items.Count > 0;
		}

		/// <summary>도형 목록 바인딩은 파일/DB 조회 흐름에서 공통으로 사용한다.</summary>
		private void BindElementList(GdsStructure str)
		{
			dataGridView1.DataSource = str.Layers.SelectMany(s => s.Elements).Select(element => new
			{
				element.LayerID,
				element.ElementName,
				element.Bounds,
				etc = element is GdsText ? ((GdsText)element).Text : element is GdsPath ? ((GdsPath)element).Width.ToString() : String.Empty
			}).ToList();
		}

		/// <summary>도면 구성과 GPU 처리 단계의 진행 상태를 StatusStrip에 표시한다.</summary>
		private void Map_RenderProgressChanged(object sender, GdsMapRenderProgressChangedEventArgs e)
		{
			lblMapStatus.Text = e.Message;
			progressMapLoad.Visible = !e.IsCompleted;
			if (e.IsCompleted)
			{
				statusStripMap.Refresh();
				return;
			}
			progressMapLoad.Style = e.IsMarquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
			if (!e.IsMarquee)
				progressMapLoad.Value = Math.Max(progressMapLoad.Minimum, Math.Min(progressMapLoad.Maximum, e.Percent));
			statusStripMap.Refresh();
		}

		/// <summary>첫 화면 완료 후 Flatten, 정점 생성, GPU 업로드의 실제 시간을 StatusStrip에 표시한다.</summary>
		private void Map_FirstFrameMeasured(object sender, GdsMapLoadMetrics metrics)
		{
			_firstFrameWatch.Stop();
			_lastLoadMetrics = metrics;
			if (_mapLoadSucceeded)
				lblMapStatus.Text = FormatLoadMetrics(metrics);
			lblMapStatus.ToolTipText = "Flatten " + metrics.FlattenMs.ToString("0") + "ms (도형 등록 " + metrics.SceneInsertMs.ToString("0") + "ms) / 정점 생성 " + metrics.VertexBuildMs.ToString("0") + "ms / GPU 업로드 " + metrics.GpuUploadMs.ToString("0") + "ms / Draw " + metrics.FirstDrawMs.ToString("0") + "ms / 도형 " + metrics.SceneItemCount.ToString("N0") + "개 / 정점 " + metrics.VertexCount.ToString("N0") + "개";
		}

		/// <summary>병목 비교에 필요한 시간만 짧게 표시하고 처리량은 마우스를 올리면 확인하게 한다.</summary>
		private string FormatLoadMetrics(GdsMapLoadMetrics metrics)
		{
			return "첫 화면 " + _firstFrameWatch.Elapsed.TotalSeconds.ToString("0.0") + "초 / Flatten " + metrics.FlattenMs.ToString("0") + "ms / 정점 " + metrics.VertexBuildMs.ToString("0") + "ms / 업로드 " + metrics.GpuUploadMs.ToString("0") + "ms";
		}

		private static string FormatBytes(long bytes)
		{
			return bytes < 1024 * 1024 ? (bytes / 1024.0).ToString("0.0") + " KB" : (bytes / 1024.0 / 1024.0).ToString("0.0") + " MB";
		}

		/// <summary>마우스 월드 좌표를 최대 약 30fps로 제한하여 StatusStrip에 표시한다.</summary>
		private void Map_MouseWorldPositionChanged(object sender, PointF position)
		{
			int now = Environment.TickCount;
			if (unchecked(now - _lastCoordinateUpdateTick) < 33) return;
			_lastCoordinateUpdateTick = now;
			lblMapCoordinate.Text = "X: " + position.X.ToString("0.###") + " / Y: " + position.Y.ToString("0.###");
		}

		/// <summary>Redraw 버튼으로 체크 상태를 적용한다. 참조 구조에만 있는 레이어도 포함한다.</summary>
		private void btnLayer_Click(object sender, EventArgs e)
		{
			ApplyLayerVisibility();
		}

		/// <summary>
		/// 개별 Layer 체크가 바뀌면 즉시 도면에 적용한다.
		/// ItemCheck는 목록 상태 변경 전에 발생하므로 이벤트의 새 상태를 사용한다.
		/// </summary>
		private void ChkLayerItems_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (_updatingLayerChecks) return;
			ApplyLayerVisibility(e.Index, e.NewValue);
		}

		/// <summary>모든 Layer를 체크한 뒤 한 번만 Map 표시 상태를 적용한다.</summary>
		private void btnLayerCheckAll_Click(object sender, EventArgs e)
		{
			SetAllLayerChecked(true);
		}

		/// <summary>모든 Layer를 미체크한 뒤 한 번만 Map 표시 상태를 적용한다.</summary>
		private void btnLayerCheckNone_Click(object sender, EventArgs e)
		{
			SetAllLayerChecked(false);
		}

		/// <summary>
		/// 목록 상태를 일괄 변경한다. 반복 중에는 항목 단위 화면 갱신을 막고 끝난 뒤 한 번만 다시 그린다.
		/// </summary>
		private void SetAllLayerChecked(bool isChecked)
		{
			if (map.Structure == null || chkLayerItems.Items.Count == 0) return;
			_updatingLayerChecks = true;
			chkLayerItems.BeginUpdate();
			try
			{
				for (int i = 0; i < chkLayerItems.Items.Count; i++)
					chkLayerItems.SetItemChecked(i, isChecked);
			}
			finally
			{
				chkLayerItems.EndUpdate();
				_updatingLayerChecks = false;
			}
			ApplyLayerVisibility();
		}

		/// <summary>현재 목록의 체크 상태를 모아 Map에 한 번만 전달한다. 변경 중인 항목은 새 상태를 우선 사용한다.</summary>
		private void ApplyLayerVisibility(int changingIndex = -1, CheckState newState = CheckState.Unchecked)
		{
			if (map.Structure == null) return;
			var visibility = new Dictionary<int, bool>();
			for (int i = 0; i < chkLayerItems.Items.Count; i++)
			{
				var item = (LayerDisplayItem)chkLayerItems.Items[i];
				visibility[item.LayerId] = i == changingIndex
					? newState == CheckState.Checked : chkLayerItems.GetItemChecked(i);
			}
			map.SetLayerVisibility(visibility);
		}

		/// <summary>파일/DB 로드 후 실제 렌더링 레이어에서 목록과 색상 견본을 다시 만든다.</summary>
		private void RefreshLayerList()
		{
			_updatingLayerChecks = true;
			chkLayerItems.BeginUpdate();
			try
			{
				chkLayerItems.Items.Clear();
				foreach (var layer in map.GetLayerDisplayItems())
					chkLayerItems.Items.Add(layer, layer.Visible);
				if (chkLayerItems.Items.Count > 0) chkLayerItems.SelectedIndex = 0;
				btnLayer.Enabled = chkLayerItems.Items.Count > 0;
				btnLayerCheckAll.Enabled = chkLayerItems.Items.Count > 0;
				btnLayerCheckNone.Enabled = chkLayerItems.Items.Count > 0;
			}
			finally
			{
				chkLayerItems.EndUpdate();
				_updatingLayerChecks = false;
			}
		}

	}
}
