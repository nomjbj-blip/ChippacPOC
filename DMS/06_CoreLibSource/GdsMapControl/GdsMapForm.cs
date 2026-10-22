using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map Setup(등록) 화면: GDS 파일 읽기 -> Layer 필터(도형 수 확인) -> Map DB 저장 / 원본 Grid 확인.
    /// 2026-10-06 결정: Chain 설정 / Chain DB 반영은 GDS Map View(GdsMapViewForm)에서 한다.
    /// </summary>
    public partial class GdsMapForm : Form
    {
		public static readonly string SAMPLE_FILE = @"D:\PROJECT\스태츠칩팩\Defect E-Test\sample\OMM all layer2\OMM all layer2.gds";
		private int _lastCoordinateUpdateTick;
		private bool _updatingLayerChecks;
		/// <summary>
		/// 오른쪽 Layer 목록 폭(px). Map(왼쪽 Tab)이 남는 폭을 모두 쓰도록 이 폭을 유지한다(2026-10-13).
		/// 처음 값은 Layer 패널 최소 폭(Panel2MinSize). 사용자가 분할선을 끌면 그 폭을 기억한다.
		/// </summary>
		private int _layerPanelWidth;
		private bool _keepingLayerPanel;
		private bool _hostedByDms;
		// Layer별 화면 배치 도형 수. 도면을 읽을 때(RefreshLayerList) 한 번 만들고 목록 표시 / 선택 합계에 쓴다.
		private Dictionary<int, int> _layerElementCounts = new Dictionary<int, int>();

		/// <summary>단독 실행에서 기존 GDS 테스트 화면 동작을 유지한다.</summary>
		public GdsMapForm()
        {
			InitializeComponent();
			GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
			// 분할선 이벤트는 InitializeComponent(자동 배율 조정 포함)가 끝난 뒤 연결한다.
			GDSContainer.SizeChanged += GDSContainer_SizeChanged;
			GDSContainer.SplitterMoved += GDSContainer_SplitterMoved;
			KeepLayerPanelWidth();
			InitializeElementGridFeatures();
			InitializeDbSaveState();
			statusStripMap.ShowItemToolTips = true;
        }

		/// <summary>
		/// DACrux 안에 표시할 때 사용하는 생성자다. 시험용 가상 결함을 그리지 않는다.
		/// 이전 직접 DB 저장 / 조회 버튼(GDS_ELEMENT)은 2026-10-02 삭제했다. DB 접속은 DMS Service(Miracom.Middleware) 경로만 사용한다.
		/// </summary>
		public GdsMapForm(bool hostedByDms) : this()
		{
			_hostedByDms = hostedByDms;
			// DB 저장은 DMS Service(RO) 연결 정보가 있는 DACrux 안에서만 가능하므로 호스트 모드에서만 보인다.
			ShowDbSaveControls(hostedByDms);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			map.OnlySelectSquareItems = true;
			KeepLayerPanelWidth();

			//button1.PerformClick();
		}

		/// <summary>
		/// Map(왼쪽 Tab)이 크게 보이도록 분할선을 Layer 목록 폭에 맞춘다(2026-10-13).
		/// SplitContainer 기본 동작은 자동 배율 조정 / DACrux 가 작은 크기로 먼저 붙였다가 키울 때 비율을 유지해서 Map 칸이 작게 남는다.
		/// 창이 Layer 폭 + Map 최소 폭보다 좁으면 SplitContainer 에 맡긴다.
		/// </summary>
		private void KeepLayerPanelWidth()
		{
			if (_layerPanelWidth <= 0) _layerPanelWidth = GDSContainer.Panel2MinSize;
			int distance = GDSContainer.ClientSize.Width - _layerPanelWidth - GDSContainer.SplitterWidth;
			if (distance < GDSContainer.Panel1MinSize || GDSContainer.SplitterDistance == distance) return;
			_keepingLayerPanel = true;
			try
			{
				GDSContainer.SplitterDistance = distance;
			}
			finally
			{
				_keepingLayerPanel = false;
			}
		}

		private void GDSContainer_SizeChanged(object sender, EventArgs e)
		{
			KeepLayerPanelWidth();
		}

		/// <summary>사용자가 분할선을 끌어 바꾼 Layer 폭은 다음 크기 변경에도 유지한다.</summary>
		private void GDSContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			if (_keepingLayerPanel || Control.MouseButtons != MouseButtons.Left) return;
			_layerPanelWidth = Math.Max(GDSContainer.Panel2MinSize, GDSContainer.Panel2.Width);
		}

		/// <summary>원점(0, 0) 십자가 표시 켜기 / 끄기. 2026-10-12 지도 우클릭 메뉴에서 화면으로 옮겼다.</summary>
		private void chkOriginCross_CheckedChanged(object sender, EventArgs e)
		{
			map.ShowOriginCross = chkOriginCross.Checked;
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
				// 최상위 Cell 목록을 채우고 펼친 도형이 가장 많은 Cell 부터 그린다(첫 Structure 가 하위 Cell 인 파일 대응) (2026-10-20)
				string topName = FillTopCellList(lib);
				GdsStructure str;
				if (topName == null || !lib.Structures.TryGetValue(topName, out str))
					throw new InvalidDataException("Structure 가 없는 GDS 파일입니다.");

				// 단독 시험에서만 가상 결함을 그려 DACrux 업무 화면의 실제 결함과 혼동하지 않게 한다.
				if (!_hostedByDms)
				{
					str.DefectList.Add(new Defect() { X = 0, Y = 0, Width = 15, Height = 20 });
					str.DefectList.Add(new Defect() { X = 1000, Y = 1500, Width = 30, Height = 20 });
				}
				SetMapStage("도면 구성 및 화면 반영 중", true);
				map.ShowStructure(lib, topName);
				// DB 저장(GdsMapForm_DbSave)이 원본 Grid 행과 파일 해시를 만들 수 있도록 현재 도면의 원본과 경로를 보관한다.
				_dbSaveLibrary = lib;
				_dbSaveGdsPath = path;
				BindElementList(lib);
				RefreshLayerList();
				EndMapLoad("완료 / Layer " + map.GetLayerDisplayItems().Count + "개");
			}
			catch (Exception ex)
			{
				EndMapLoad("파일 조회 실패");
				MessageBox.Show(this, ex.Message, "GDS 파일 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>작업 중 표시 (2026-10-15): 파일 / DB 조회와 DB 저장 중 상태 칸 "작업 중..." + 모래시계 커서. 처리 시간은 보이지 않는다.</summary>
		private BusyStatus MapBusy
		{
			get { return _mapBusy ?? (_mapBusy = new BusyStatus(this, lblMapElapsed)); }
		}
		private BusyStatus _mapBusy;

		/// <summary>
		/// 최상위 Cell 목록을 채우고 처음 그릴 Cell 이름을 돌려준다(펼친 도형이 많은 순서, 첫 항목 선택) (2026-10-20).
		/// 최상위 Cell 이 둘 이상일 때만 선택할 수 있다. 예: U8 = OMM_PFIC_PIC_PKG_A0(제품) / TOP(Package C4) / mark_cell.
		/// </summary>
		private string FillTopCellList(GdsLibrary lib)
		{
			cboTopCell.BeginUpdate();
			try
			{
				cboTopCell.Items.Clear();
				foreach (var top in lib.GetTopStructures())
					cboTopCell.Items.Add(new TopCellItem(top.Name, top.ElementCount));
				if (cboTopCell.Items.Count > 0) cboTopCell.SelectedIndex = 0;
			}
			finally
			{
				cboTopCell.EndUpdate();
			}
			cboTopCell.Enabled = cboTopCell.Items.Count > 1;
			var first = cboTopCell.SelectedItem as TopCellItem;
			return first != null ? first.Name : lib.GetDefaultTopStructureName();
		}

		/// <summary>다른 최상위 Cell 을 고르면 그 Cell 로 다시 펼친다. DB 저장 TOP_STRUCTURE 도 이 Cell 이 된다 (2026-10-20)</summary>
		private void cboTopCell_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var item = cboTopCell.SelectedItem as TopCellItem;
			if (item == null || _dbSaveLibrary == null || (map.Structure != null && map.Structure.Name == item.Name))
				return;

			BeginMapLoad("도면 구성 중 / " + item.Name, true);
			try
			{
				map.ShowStructure(_dbSaveLibrary, item.Name);
				RefreshLayerList();
				EndMapLoad("완료 / Cell " + item.Name + " / Layer " + map.GetLayerDisplayItems().Count + "개");
			}
			catch (Exception ex)
			{
				EndMapLoad("도면 구성 실패");
				MessageBox.Show(this, ex.Message, "Cell 변경", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>Cell 선택 목록 항목: 이름 (펼친 도형 수)</summary>
		private sealed class TopCellItem
		{
			public TopCellItem(string name, long elementCount)
			{
				Name = name;
				ElementCount = elementCount;
			}

			public string Name { get; private set; }
			public long ElementCount { get; private set; }

			public override string ToString()
			{
				return Name + " (" + ElementCount.ToString("N0") + ")";
			}
		}

		/// <summary>파일/DB 조회 중 조작을 막고 StatusStrip의 진행 상태를 초기화한다.</summary>
		private void BeginMapLoad(string stage, bool marquee)
		{
			MapBusy.Start();
			cboTopCell.Enabled = false;
			button1.Enabled = false;
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
			MapBusy.Stop();
			progressMapLoad.Visible = false;
			lblMapStatus.Text = message;
			button1.Enabled = true;
			cboTopCell.Enabled = cboTopCell.Items.Count > 1;
			btnLayerCheckAll.Enabled = chkLayerItems.Items.Count > 0;
			btnLayerCheckNone.Enabled = chkLayerItems.Items.Count > 0;
		}

		/// <summary>
		/// GDS에 파싱된 모든 Structure와 Element를 상세 열로 표시한다.
		/// 좌표 배열은 복사하지 않고 조회 전용 행이 원본 Element를 참조하여 대용량 메모리 증가를 줄인다.
		/// </summary>
		private void BindElementList(GdsLibrary lib)
		{
			int elementCount = lib.Structures.Sum(structure =>
				structure.Layers.Sum(layer => layer.Elements.Count) + structure.Layers.DuplicatedItems.Count);
			var rows = new List<GdsElementGridRow>(elementCount);
			int gridNumber = 1;
			foreach (var structure in lib.Structures)
			{
				int sourceOrdinal = 0;
				foreach (var layer in structure.Layers)
				{
					foreach (var element in layer.Elements)
					{
						// Flatten과 같은 순서로 번호를 매겨 원본 Grid 행에서 배치 도형을 역조회할 수 있게 한다.
						string sourceId = structure.Name + "/E" + sourceOrdinal++;
						rows.Add(new GdsElementGridRow(lib, structure, gridNumber++, element, false, sourceId));
					}
				}

				// 파서가 동일 키로 판단해 도면 목록에서 제외한 Element도 원본 확인을 위해 별도 행으로 표시한다.
				foreach (var duplicate in structure.Layers.DuplicatedItems)
					rows.Add(new GdsElementGridRow(lib, structure, gridNumber++, duplicate, true));
			}

			dataGrid.DataSource = rows;
			ConfigureElementGridColumns();
		}

		/// <summary>상세 열이 많아도 좌표와 TEXT를 확인하기 쉽도록 주요 열의 기본 폭과 숫자 형식을 설정한다.</summary>
		private void ConfigureElementGridColumns()
		{
			dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			foreach (DataGridViewColumn column in dataGrid.Columns)
			{
				column.Width = column.Name == "Text" ? 220 : column.Name == "StructureName" || column.Name == "ReferenceStructure" ? 150 : 95;
				if (column.ValueType == typeof(double) || column.ValueType == typeof(double?))
					column.DefaultCellStyle.Format = "0.#####";
			}

			// 원본 Element의 순번을 항상 가장 왼쪽에서 확인할 수 있게 고정한다.
			DataGridViewColumn numberColumn = dataGrid.Columns["GridNumber"];
			if (numberColumn != null)
			{
				numberColumn.HeaderText = "No";
				numberColumn.DisplayIndex = 0;
				numberColumn.Width = 65;
				numberColumn.Frozen = true;
			}
		}

		/// <summary>
		/// Element Grid에 Cell 단위 선택, Ctrl+C 복사, 우클릭 복사/Excel CSV 저장 기능을 연결한다.
		/// Designer 배치를 바꾸지 않고 Grid 자체 기능으로 제공하여 기존 화면 크기를 유지한다.
		/// </summary>
		private void InitializeElementGridFeatures()
		{
			dataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGrid.MultiSelect = true;
			dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGrid.KeyDown += DataGrid_KeyDown;
			dataGrid.CellMouseDown += DataGrid_CellMouseDown;

			var menu = new ContextMenuStrip();
			menu.Items.Add(new ToolStripMenuItem("선택 Cell 복사", null, (sender, args) => CopySelectedGridCells()));
			menu.Items.Add(new ToolStripSeparator());
			menu.Items.Add(new ToolStripMenuItem("Excel Export (CSV)", null, async (sender, args) => await ExportElementGridAsync()));
			dataGrid.ContextMenuStrip = menu;
		}

		/// <summary>Ctrl+C 입력 시 현재 선택된 Cell들을 행/열 형태를 유지한 탭 구분 문자열로 복사한다.</summary>
		private void DataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Control || e.KeyCode != Keys.C) return;
			CopySelectedGridCells();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}

		/// <summary>우클릭한 Cell이 선택되지 않았다면 해당 Cell을 먼저 선택하여 바로 복사할 수 있게 한다.</summary>
		private void DataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0) return;
			if (!dataGrid[e.ColumnIndex, e.RowIndex].Selected)
			{
				dataGrid.ClearSelection();
				dataGrid[e.ColumnIndex, e.RowIndex].Selected = true;
				dataGrid.CurrentCell = dataGrid[e.ColumnIndex, e.RowIndex];
			}
		}

		/// <summary>선택된 Cell 내용을 Windows Clipboard에 복사한다.</summary>
		private void CopySelectedGridCells()
		{
			if (dataGrid.GetCellCount(DataGridViewElementStates.Selected) == 0) return;
			DataObject data = dataGrid.GetClipboardContent();
			if (data != null)
				Clipboard.SetDataObject(data, true);
		}

		/// <summary>
		/// 현재 Element Grid 전체를 Excel에서 바로 열 수 있는 UTF-8 BOM CSV로 저장한다.
		/// UI에서는 저장 위치와 열 순서만 수집하고, 대용량 행 쓰기는 백그라운드에서 처리한다.
		/// </summary>
		private async Task ExportElementGridAsync()
		{
			var rows = dataGrid.DataSource as IEnumerable<GdsElementGridRow>;
			if (rows == null) return;

			string path;
			using (var dialog = new SaveFileDialog())
			{
				dialog.Filter = "Excel CSV (*.csv)|*.csv";
				dialog.DefaultExt = "csv";
				dialog.AddExtension = true;
				dialog.FileName = "GdsElements_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
				if (dialog.ShowDialog(this) != DialogResult.OK) return;
				path = dialog.FileName;
			}

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(GdsElementGridRow));
			var columns = dataGrid.Columns.Cast<DataGridViewColumn>()
				.Where(column => column.Visible && properties[column.DataPropertyName] != null)
				.OrderBy(column => column.DisplayIndex)
				.Select(column => new GridExportColumn(column.HeaderText, properties[column.DataPropertyName]))
				.ToArray();
			var exportRows = rows.ToList();

			dataGrid.Enabled = false;
			try
			{
				await Task.Run(() => WriteElementGridCsv(path, columns, exportRows));
				MessageBox.Show(this, "Excel CSV 저장이 완료되었습니다.\r\n" + path,
					"Excel Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Excel Export 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				dataGrid.Enabled = true;
			}
		}

		/// <summary>Grid 열 순서대로 Header와 모든 Element 값을 CSV 한 행씩 기록한다.</summary>
		private static void WriteElementGridCsv(string path, GridExportColumn[] columns, List<GdsElementGridRow> rows)
		{
			using (var writer = new StreamWriter(path, false, new UTF8Encoding(true)))
			{
				writer.WriteLine(String.Join(",", columns.Select(column => EscapeCsv(column.HeaderText))));
				foreach (GdsElementGridRow row in rows)
				{
					writer.WriteLine(String.Join(",", columns.Select(column =>
						EscapeCsv(Convert.ToString(column.Property.GetValue(row), CultureInfo.InvariantCulture)))));
				}
			}
		}

		/// <summary>쉼표, 큰따옴표, 줄바꿈이 있는 값도 Excel에서 한 Cell로 열리도록 CSV 규칙으로 감싼다.</summary>
		private static string EscapeCsv(string value)
		{
			value = value ?? String.Empty;
			return "\"" + value.Replace("\"", "\"\"") + "\"";
		}

		/// <summary>화면 Header와 원본 Row 속성을 연결하여 백그라운드 CSV 저장에 사용한다.</summary>
		private sealed class GridExportColumn
		{
			public string HeaderText { get; private set; }
			public PropertyDescriptor Property { get; private set; }

			public GridExportColumn(string headerText, PropertyDescriptor property)
			{
				HeaderText = headerText;
				Property = property;
			}
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

		/// <summary>Redraw 버튼으로 체크 상태를 적용한다. 참조 구조에만 있는 레이어도 포함한다.</summary>
		private void btnLayer_Click(object sender, EventArgs e)
		{
			ApplyLayerVisibility();
		}

		/// <summary>
		/// 개별 Layer 체크가 바뀌면 즉시 도면에 적용한다.
		/// ItemCheck는 목록 상태 변경 전에 발생하므로 이벤트의 새 상태를 사용한다.
		/// 체크는 행 왼쪽 체크 박스를 눌렀을 때만 바뀐다(2026-10-20, 목록 CheckOnCheckBoxOnly). 행 클릭 / 더블클릭 / Space 키는 행 선택만 한다.
		/// [전체 체크] / [전체 미체크]는 _updatingLayerChecks 로 먼저 빠져나가므로 그대로 동작한다.
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

		/// <summary>Layer 체크를 공통 지도 표시로 적용한다. 이미 찾은 Chain 경로와 Chain별 탐색 Layer는 유지한다.</summary>
		private void ApplyLayerVisibility(int changingIndex = -1, CheckState newState = CheckState.Unchecked)
		{
			if (map.Structure == null) return;
			var visibility = new Dictionary<int, bool>();
			for (int i = 0; i < chkLayerItems.Items.Count; i++)
			{
				var item = (LayerDisplayItem)chkLayerItems.Items[i];
				visibility[item.LayerKey] = i == changingIndex
					? newState == CheckState.Checked : chkLayerItems.GetItemChecked(i);
			}
			map.SetLayerVisibility(visibility);
			UpdateLayerSummary(visibility);
		}

		/// <summary>Layer 이름(번호/DataType) 뒤에 화면 배치 도형 수를 붙인다. 예: Layer 18/3 (12,345)</summary>
		private void ChkLayerItems_Format(object sender, ListControlConvertEventArgs e)
		{
			var item = e.ListItem as LayerDisplayItem;
			if (item == null) return;
			int count;
			e.Value = "Layer " + GdsLayerKey.Format(item.LayerKey) + " ("
				+ (_layerElementCounts.TryGetValue(item.LayerKey, out count) ? count.ToString("N0") : "-") + ")";
		}

		/// <summary>
		/// 목록 위에 선택 Layer 수 / 선택 도형 수 합계를 표시한다. DB 저장 시 PLACED 행 수와 같다.
		/// visibility가 null이면 목록의 현재 체크 상태를 사용한다(ItemCheck 중에는 새 상태를 넘겨받는다).
		/// </summary>
		private void UpdateLayerSummary(IDictionary<int, bool> visibility = null)
		{
			int layerCount = chkLayerItems.Items.Count, selectedLayers = 0;
			long total = 0, selected = 0;
			for (int i = 0; i < layerCount; i++)
			{
				int id = ((LayerDisplayItem)chkLayerItems.Items[i]).LayerKey;
				int count;
				_layerElementCounts.TryGetValue(id, out count);
				total += count;
				bool isChecked;
				if (visibility == null || !visibility.TryGetValue(id, out isChecked)) isChecked = chkLayerItems.GetItemChecked(i);
				if (!isChecked) continue;
				selectedLayers++;
				selected += count;
			}
			lblLayerSummary.Text = layerCount == 0 ? "Layer"
				: "선택 " + selectedLayers + " / " + layerCount + " Layer · 도형 " + selected.ToString("N0") + " / " + total.ToString("N0");
		}

		/// <summary>파일/DB 로드 후 실제 렌더링 레이어에서 목록과 색상 견본을 다시 만든다.</summary>
		private void RefreshLayerList()
		{
			_updatingLayerChecks = true;
			chkLayerItems.BeginUpdate();
			try
			{
				chkLayerItems.Items.Clear();
				_layerElementCounts = map.GetLayerElementCounts();
				foreach (var layer in map.GetLayerDisplayItems())
					chkLayerItems.Items.Add(layer, layer.Visible);
				if (chkLayerItems.Items.Count > 0) chkLayerItems.SelectedIndex = 0;
				btnLayerCheckAll.Enabled = chkLayerItems.Items.Count > 0;
				btnLayerCheckNone.Enabled = chkLayerItems.Items.Count > 0;
			}
			finally
			{
				chkLayerItems.EndUpdate();
				_updatingLayerChecks = false;
			}
			UpdateLayerSummary();
		}

	}
}
