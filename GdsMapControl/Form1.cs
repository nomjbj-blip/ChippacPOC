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
    public partial class Form1 : Form
    {
		public static readonly string SAMPLE_FILE = @"D:\PROJECT\스태츠칩팩\Defect E-Test\sample\OMM all layer2\OMM all layer2.gds";
		private readonly Stopwatch _mapLoadWatch = new Stopwatch();
		private int _lastCoordinateUpdateTick;
        public Form1()
        {
			InitializeComponent();
			InitializeLayerColorMenu();
			map.RenderProgressChanged += Map_RenderProgressChanged;
			map.MouseWorldPositionChanged += Map_MouseWorldPositionChanged;
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
			button1.Enabled = false;
			btnLoad.Enabled = false;
			btnSave.Enabled = false;
			btnLayer.Enabled = false;
			btnLayerColor.Enabled = false;
			btnLayerCheckAll.Enabled = false;
			btnLayerCheckNone.Enabled = false;
			btnLayerLabelDiagnostic.Enabled = false;
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
			progressMapLoad.Visible = false;
			lblMapStatus.Text = message;
			button1.Enabled = true;
			btnLoad.Enabled = true;
			btnSave.Enabled = true;
			btnLayer.Enabled = true;
			btnLayerColor.Enabled = checkedListBox1.SelectedItem is LayerDisplayItem;
			btnLayerCheckAll.Enabled = checkedListBox1.Items.Count > 0;
			btnLayerCheckNone.Enabled = checkedListBox1.Items.Count > 0;
			btnLayerLabelDiagnostic.Enabled = map.Structure != null;
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

		/// <summary>Layer 775 TEXT의 원본 좌표와 변환 후 좌표를 표로 조회한다. 표시 위치는 변경하지 않는다.</summary>
		private void btnLayerLabelDiagnostic_Click(object sender, EventArgs e)
		{
			var diagnostics = map.GetLayer775LabelDiagnostics();
			dataGridView1.DataSource = diagnostics;
			lblMapStatus.Text = "Layer 775 Label 진단 / " + diagnostics.Count + "건 / 좌표 보정 없음";
		}

		/// <summary>Redraw 버튼으로 체크 상태를 적용한다. 참조 구조에만 있는 레이어도 포함한다.</summary>
		private void btnLayer_Click(object sender, EventArgs e)
		{
			ApplyLayerVisibility();
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
			if (map.Structure == null || checkedListBox1.Items.Count == 0) return;
			checkedListBox1.BeginUpdate();
			try
			{
				for (int i = 0; i < checkedListBox1.Items.Count; i++)
					checkedListBox1.SetItemChecked(i, isChecked);
			}
			finally
			{
				checkedListBox1.EndUpdate();
			}
			ApplyLayerVisibility();
		}

		/// <summary>현재 목록의 체크 상태를 모아 Map에 한 번만 전달한다.</summary>
		private void ApplyLayerVisibility()
		{
			if (map.Structure == null) return;
			var visibility = new Dictionary<int, bool>();
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				var item = (LayerDisplayItem)checkedListBox1.Items[i];
				visibility[item.LayerId] = checkedListBox1.GetItemChecked(i);
			}
			map.SetLayerVisibility(visibility);
		}

		/// <summary>우클릭/키보드 메뉴와 버튼을 동일한 색상 변경 처리에 연결한다.</summary>
		private void InitializeLayerColorMenu()
		{
			var menu = new ContextMenuStrip(components);
			menu.Items.Add("색상 변경...", null, ChangeSelectedLayerColor);
			menu.Items.Add("기본 색상으로 복원", null, ResetSelectedLayerColor);
			menu.Opening += (s, e) => e.Cancel = !(checkedListBox1.SelectedItem is LayerDisplayItem);
			checkedListBox1.ContextMenuStrip = menu;
			checkedListBox1.SelectedIndexChanged += (s, e) =>
				btnLayerColor.Enabled = checkedListBox1.SelectedItem is LayerDisplayItem;
			var tip = new ToolTip(components);
			tip.SetToolTip(checkedListBox1, "체크 후 Redraw: 표시/숨김\n우클릭: 색상 변경/기본 색상 복원");
		}

		/// <summary>파일/DB 로드 후 실제 렌더링 레이어에서 목록과 색상 견본을 다시 만든다.</summary>
		private void RefreshLayerList()
		{
			checkedListBox1.BeginUpdate();
			try
			{
				checkedListBox1.Items.Clear();
				foreach (var layer in map.GetLayerDisplayItems())
					checkedListBox1.Items.Add(layer, layer.Visible);
				if (checkedListBox1.Items.Count > 0) checkedListBox1.SelectedIndex = 0;
				btnLayerColor.Enabled = checkedListBox1.SelectedItem is LayerDisplayItem;
				btnLayerCheckAll.Enabled = checkedListBox1.Items.Count > 0;
				btnLayerCheckNone.Enabled = checkedListBox1.Items.Count > 0;
				btnLayerLabelDiagnostic.Enabled = map.Structure != null;
			}
			finally
			{
				checkedListBox1.EndUpdate();
			}
		}

		/// <summary>현재 색상으로 선택 창을 열고 확인한 경우에만 도면과 색상 견본을 갱신한다.</summary>
		private void ChangeSelectedLayerColor(object sender, EventArgs e)
		{
			var item = checkedListBox1.SelectedItem as LayerDisplayItem;
			if (item == null) return;
			using (var dialog = new ColorDialog { Color = item.Color, FullOpen = true })
			{
				if (dialog.ShowDialog(this) != DialogResult.OK) return;
				ApplyLayerColor(item, dialog.Color);
			}
		}

		/// <summary>선택 레이어 하나를 번호에 맞는 기본 팔레트 색상으로 되돌린다.</summary>
		private void ResetSelectedLayerColor(object sender, EventArgs e)
		{
			var item = checkedListBox1.SelectedItem as LayerDisplayItem;
			if (item != null) ApplyLayerColor(item, map.GetDefaultLayerColor(item.LayerId));
		}

		/// <summary>도면 적용이 성공하면 해당 항목만 갱신하여 아직 적용하지 않은 체크 상태도 유지한다.</summary>
		private void ApplyLayerColor(LayerDisplayItem item, Color color)
		{
			if (!map.SetLayerColor(item.LayerId, color)) return;
			item.Color = map.GetLayerColor(item.LayerId);
			checkedListBox1.Invalidate();
		}
	}
}
