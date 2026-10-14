using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// Wafer Map 조회 화면. File Map Setup(frmStandardMapSetup) 등으로 저장된 Map(TQP_MAPDEF / TQP_USEMAP)을 읽기 전용으로 조회해
    /// Data 가 저장 규칙대로 들어갔는지 확인한다. 편집 / 삭제 / 저장 기능은 없다.
    /// 역할 / 흐름:
    /// 1. 화면을 열거나 [조회] 를 누르면 기존 ProbeAdmin.GetMapIDAllList (삭제 안 된 Map 전체) 를 읽어 왼쪽 목록에 보인다.
    ///    Device 검색칸은 다시 조회하지 않고 목록만 거른다 (Map ID 포함 검색 / 대소문자 무시).
    /// 2. 목록에서 Map 을 고르면 GetMapDef + GetDies 로 정의 1행과 Die 행을 읽고 StoredMapLayout 으로 화면 기준 값과 확인 항목을 만든다.
    /// 3. Map 컨트롤은 LeftBottom 번호로 그린다 (StoredMapLayout 주석 참고). 좌표 표시는 저장 방향 번호와 LeftBottom 번호를 함께 보인다.
    /// 4. 오른쪽 탭: Map 정보 (TQP_MAPDEF 값) / 데이터 확인 (저장값 vs Die 행) / Die 목록 (TQP_USEMAP 행).
    /// 5. Die 번호: 저장 XY 방향 기준으로 화면에서 계산해 Die 위에 그린다 (DieNumberRule, 2026-10-13). 저장 DIE_NUM 은 Die 목록에서 비교한다.
    /// Factory 선택은 두지 않는다. 저장 쿼리(CREATE_MAP)가 FACTORY 를 넣지 않아 Factory 로 거르면 새로 만든 Map 이 빠질 수 있다.
    /// 설계: 문서/2026-10-13_Wafer_Map_조회화면_설계.md
    /// </summary>
    public partial class frmStandardMapView : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>마지막으로 조회한 Map 목록 (검색은 이 표의 DataView 로 거른다).</summary>
        private DataTable mapList;
        /// <summary>지금 화면에 그린 Map. 없으면 null.</summary>
        private StoredMapLayout currentLayout;
        /// <summary>목록을 다시 묶는 동안 선택 변경 이벤트로 Map 을 읽지 않게 한다.</summary>
        private bool bindingList;
        /// <summary>동일 Die 에서는 변경 이벤트가 반복되지 않으므로 마지막 Die 를 유지한다.</summary>
        private Die hoverDie;

        /// <summary>화면 구성과 이벤트 연결은 InitializeComponent 에서 한다. 표의 자동 컬럼 생성만 여기서 끈다 (디자이너 속성 창에 없는 값).</summary>
        public frmStandardMapView()
        {
            InitializeComponent();
            grdMapList.AutoGenerateColumns = false;
            grdDies.AutoGenerateColumns = false;
            ClearMap("Device 를 선택하세요.");
        }

        #region Map 목록

        /// <summary>화면을 열면 Map 목록을 한 번 읽는다.</summary>
        private void frmStandardMapView_Load(object sender, EventArgs e)
        {
            LoadMapList();
        }

        /// <summary>[조회]: 목록을 다시 읽는다. 보던 Map 이 목록에 남아 있으면 다시 골라 정의 / Die 를 새로 읽는다.</summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMapList();
        }

        /// <summary>검색칸에서 Enter 를 누르면 목록을 거른다 (다시 조회하지 않음).</summary>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            ApplyFilter();
        }

        /// <summary>검색어가 바뀌면 바로 목록을 거른다.</summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        /// <summary>
        /// 기존 서비스 ProbeAdmin.GetMapIDAllList (GET_MAPDEF_ALL: DELETE_FLAG 가 Y 가 아닌 Map 전체, MAPID 순) 를 부른다.
        /// 원격 호출이 실패하면 이전 목록 / Map 을 지워 다른 Map 정보가 남지 않게 한다.
        /// </summary>
        private void LoadMapList()
        {
            string keepMapId = currentLayout == null ? null : currentLayout.MapId;
            try
            {
                Cursor = Cursors.WaitCursor;
                DACrux.TEST.RO.ProbeAdmin admin = new DACrux.TEST.RO.ProbeAdmin();
                ShowMapList(admin.GetMapIDAllList(), keepMapId);
            }
            catch (Exception ex)
            {
                ShowMapList(null, null);
                lblStatus.Text = "Map 목록 조회 실패";
                MessageBox.Show(this, ex.Message, "Map 목록 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 조회한 Map 목록을 표에 묶는다. 화면 표시용 컬럼(Wafer mm / XY 방향 이름)을 추가한다.
        /// keepMapId 가 목록에 있으면 그 행을 골라 다시 읽고, 없으면 Map 을 비운다.
        /// 서비스 호출과 나눈 이유: 화면 밖 검사가 DataTable 을 직접 넣어 확인하기 위해서다.
        /// </summary>
        internal void ShowMapList(DataTable maps, string keepMapId)
        {
            bindingList = true;
            try
            {
                DataTable old = mapList;
                mapList = maps;
                if (mapList != null)
                {
                    if (!mapList.Columns.Contains("WAFER_MM")) mapList.Columns.Add("WAFER_MM", typeof(string));
                    if (!mapList.Columns.Contains("XY_NAME")) mapList.Columns.Add("XY_NAME", typeof(string));
                    foreach (DataRow row in mapList.Rows)
                    {
                        row["WAFER_MM"] = WaferMmText(row);
                        row["XY_NAME"] = DirectionText(row);
                    }
                    grdMapList.DataSource = new DataView(mapList);
                }
                else
                {
                    grdMapList.DataSource = null;
                }
                // 표가 새 목록을 묶은 뒤에 이전 목록을 해제한다.
                if (old != null && !object.ReferenceEquals(old, mapList)) old.Dispose();
                ApplyFilterCore();
                grdMapList.ClearSelection();
            }
            finally
            {
                bindingList = false;
            }

            if (!string.IsNullOrEmpty(keepMapId) && SelectMapRow(keepMapId)) return;
            ClearMap(mapList == null ? "Map 목록이 없습니다." : "Device 를 선택하세요.");
        }

        /// <summary>검색어로 목록을 거르고 건수를 갱신한다. 보던 Map 이 걸러져도 Map 은 그대로 둔다.</summary>
        private void ApplyFilter()
        {
            bindingList = true;
            try { ApplyFilterCore(); }
            finally { bindingList = false; }
        }

        private void ApplyFilterCore()
        {
            DataView view = grdMapList.DataSource as DataView;
            if (view == null)
            {
                lblListCount.Text = "0 / 0 건";
                return;
            }
            string keyword = txtSearch.Text.Trim();
            // DataTable 의 문자열 비교는 기본이 대소문자 무시(CaseSensitive = false)다. LIKE 특수 문자는 [] 로 감싼다.
            view.RowFilter = keyword.Length == 0 ? string.Empty : "MAPID LIKE '%" + EscapeLike(keyword) + "%'";
            lblListCount.Text = string.Format(CultureInfo.InvariantCulture, "{0} / {1} 건", view.Count, view.Table.Rows.Count);
        }

        /// <summary>목록에서 Map ID 행을 골라 보이게 한다. 고르면 선택 변경 이벤트가 Map 을 읽는다.</summary>
        private bool SelectMapRow(string mapId)
        {
            foreach (DataGridViewRow row in grdMapList.Rows)
            {
                DataRowView item = row.DataBoundItem as DataRowView;
                if (item == null || !string.Equals(item["MAPID"].ToString(), mapId, StringComparison.Ordinal)) continue;
                grdMapList.CurrentCell = row.Cells[0];
                row.Selected = true;
                LoadSelectedMap();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 표가 DataSource 를 묶거나 다시 거른 뒤 첫 행을 자동 선택하므로 선택을 지운다.
        /// 작업자가 고른 Map 만 읽게 하기 위해서다 (자동 선택으로 원격 조회가 일어나지 않게).
        /// </summary>
        private void grdMapList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grdMapList.ClearSelection();
        }

        /// <summary>목록 선택이 바뀌면 그 Map 을 읽는다. 목록을 묶는 중이거나 같은 Map 이면 읽지 않는다 (다시 읽기는 [조회]).</summary>
        private void grdMapList_SelectionChanged(object sender, EventArgs e)
        {
            if (bindingList) return;
            string mapId = SelectedMapId();
            if (mapId == null) return;
            if (currentLayout != null && currentLayout.MapId == mapId) return;
            LoadSelectedMap();
        }

        private string SelectedMapId()
        {
            if (grdMapList.CurrentRow == null || !grdMapList.CurrentRow.Selected) return null;
            DataRowView item = grdMapList.CurrentRow.DataBoundItem as DataRowView;
            return item == null ? null : item["MAPID"].ToString();
        }

        /// <summary>
        /// 고른 Map 의 정의 1행(GetMapDef / GET_MAPDEF)과 Die 행(GetDies / SELECT_USEMAP_ALLDIE)을 읽어 보인다.
        /// 다른 화면이 그 사이 덮어썼을 수 있으므로 목록 값 대신 GET_MAPDEF 로 다시 읽는다.
        /// </summary>
        private void LoadSelectedMap()
        {
            string mapId = SelectedMapId();
            if (mapId == null) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                DACrux.TEST.RO.ProbeAdmin admin = new DACrux.TEST.RO.ProbeAdmin();
                using (DataTable mapDef = admin.GetMapDef(mapId))
                using (DataTable dies = admin.GetDies(mapId))
                {
                    ShowStoredMap(mapDef, dies);
                }
            }
            catch (Exception ex)
            {
                ClearMap("Map 조회 실패: " + mapId);
                MessageBox.Show(this, ex.Message, "Map 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Map 표시

        /// <summary>
        /// 저장된 Map 정의 / Die 행을 화면에 보인다. 서비스 호출과 나눈 이유: 화면 밖 검사가 DataTable 을 직접 넣어 확인하기 위해서다.
        /// 흐름: StoredMapLayout 계산 -> Map 그리기(XY 방향 값이 올바를 때만) -> Wafer 밖 사용 Die 확인 추가 -> 탭 3개 / 개수 줄 갱신.
        /// </summary>
        internal void ShowStoredMap(DataTable mapDef, DataTable dies)
        {
            if (mapDef == null || mapDef.Rows.Count == 0)
            {
                ClearMap("Map 정의(TQP_MAPDEF)가 없습니다. 목록을 다시 조회하세요.");
                return;
            }

            StoredMapLayout layout = StoredMapLayout.Create(mapDef.Rows[0], dies);
            ClearCoordinates();
            if (layout.IsDirectionValid)
            {
                bool drawn = false;
                try
                {
                    DrawStoredMap(layout);
                    drawn = true;
                }
                catch (Exception ex)
                {
                    // 저장값이 Map 컨트롤이 그릴 수 없는 값이어도 확인 표 / Die 목록은 보여야 한다 (이 화면의 목적이 저장값 확인이다).
                    ResetMapControl();
                    layout.AddDrawFailureCheck(ex.Message);
                }
                if (drawn) CheckInsideWafer(layout);
            }
            else
            {
                ResetMapControl();
            }

            currentLayout = layout;
            ShowInfo(layout);
            ShowChecks(layout);
            ShowDies(layout);
            // XY 방향 / 원점 등 나머지 값은 [Map 정보] 탭에 있으므로 개수 줄은 한 줄에 들어가는 값만 보인다.
            lblDieCount.Text = string.Format(CultureInfo.InvariantCulture,
                "{0}  /  사용(1) {1:#,0}  /  제거(-1) {2:#,0}  /  기타 {3:#,0}  /  Die 행 {4:#,0}",
                layout.MapId, layout.UsedCount, layout.RemovedCount, layout.OtherCount, layout.Dies.Count);
            int differ = layout.DifferCount;
            lblStatus.Text = differ == 0
                ? "조회 완료: " + layout.MapId + " / 데이터 확인 다름 0"
                : "조회 완료: " + layout.MapId + " / 데이터 확인 다름 " + differ + " - [데이터 확인] 탭을 보세요.";
        }

        /// <summary>
        /// 그린 Map 의 Die 마다 Wafer 안(반지름 - Edge, Notch / Flat 영역 밖)인지 Map 컨트롤 판정(IsInsideWafer)으로 정하고,
        /// 사용(1) 인데 Wafer 밖인 Die 수를 확인 항목에 넣는다. Map 컨트롤 Die 목록 순서에 기대지 않고 LeftBottom 번호로 찾는다.
        /// </summary>
        private void CheckInsideWafer(StoredMapLayout layout)
        {
            Dictionary<Point, bool> inside = new Dictionary<Point, bool>();
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Die die = m_wMap.Dies[i];
                inside[new Point(die.IndexX, die.IndexY)] = m_wMap.IsInsideWafer(die);
            }
            int outsideUsed = 0;
            foreach (StoredMapDie die in layout.Dies)
            {
                bool value;
                if (!inside.TryGetValue(die.LeftBottom, out value)) continue;
                die.InsideWafer = value;
                if (die.UseCode == 1 && !value) outsideUsed++;
            }
            layout.AddOutsideWaferCheck(outsideUsed);
        }

        /// <summary>
        /// File Map Setup 의 원형 생성(GenerateCircleMap)과 같은 순서로 Map 컨트롤 값을 넣고 Die 를 LeftBottom 번호로 AddDie 한다.
        /// - WaferSize: WAFER_SIZE 는 µm 저장값 그대로 / EdgeSize: EDGE_SIZE(mm) x 1000 = µm (File Map Setup 화면 기준)
        ///   (기존 Map Define 은 mm 값을 그대로 넣어 Edge 선이 거의 보이지 않는다. Die 위치 / USECODE 에는 영향 없음)
        /// - DieCalculation(true) 는 쓰지 않는다. AddDie 와 Die 위치 계산이 한 칸 다르기 때문이다 (StandardMapEditMap 주석).
        /// </summary>
        private void DrawStoredMap(StoredMapLayout layout)
        {
            m_wMap.DieClear();
            m_wMap.ViewAngle = 0;
            m_wMap.AngleOffSet = 0;
            m_wMap.WaferSize = layout.WaferSize;
            // Edge 는 Wafer 크기를 정한 뒤 넣는다 (File Map Setup 과 같은 순서). mm 범위 밖 값은 데이터 확인에서 다름으로 알리고 0 으로 그린다.
            m_wMap.EdgeSize = layout.IsEdgeValid ? layout.EdgeSizeMm * 1000d : 0d;
            m_wMap.NotchType = layout.NotchType;
            m_wMap.NotchAngle = layout.NotchAngle;
            m_wMap.XYDirect = XYDirection.LeftBottom;
            m_wMap.DieSizeX = layout.DieSizeX;
            m_wMap.DieSizeY = layout.DieSizeY;
            m_wMap.ReferenceDieSetting = layout.ReferenceDieSetting;
            m_wMap.OriginIndexX = layout.LeftBottomOriginIndex.X;
            m_wMap.OriginIndexY = layout.LeftBottomOriginIndex.Y;
            m_wMap.OriginX = layout.OriginX;
            m_wMap.OriginY = layout.OriginY;
            m_wMap.DieMinX = layout.LeftBottomMinX;
            m_wMap.DieMaxX = layout.LeftBottomMaxX;
            m_wMap.DieMinY = layout.LeftBottomMinY;
            m_wMap.DieMaxY = layout.LeftBottomMaxY;
            m_wMap.FirstDieX = layout.LeftBottomMinX;
            m_wMap.FirstDieY = layout.LeftBottomMinY;
            m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            // 보기 전용: 클릭해도 Die 가 바뀌지 않는다 (드래그 = 확대, 기존 기본 동작).
            m_wMap.SetDieEditMode(false, DACrux.Map.EditMode.ADD);
            m_wMap.DieNumbers = null;
            m_wMap.Redraw();

            // Die 번호는 Map 컨트롤 번호(LeftBottom)로 찾으므로 LeftBottom 을 키로 넘긴다.
            Dictionary<Point, int> numbers = new Dictionary<Point, int>();
            foreach (StoredMapDie die in layout.Dies)
            {
                m_wMap.AddDie(new Die(die.LeftBottom.X, die.LeftBottom.Y, 0, 0, 0, 0, 0, die.UseCode));
                if (die.DirectionNumber.HasValue) numbers[die.LeftBottom] = die.DirectionNumber.Value;
            }
            m_wMap.DieNumbers = numbers;
            m_wMap.ShowDieNumbers = chkDieNumber.Checked;
            m_wMap.Redraw();
        }

        /// <summary>Map 을 비우고 그릴 수 있는 기본값(300 mm / Edge 0)으로 되돌린다. 그리기 실패 / XY 방향 오류 / 조회 실패 때 쓴다.</summary>
        private void ResetMapControl()
        {
            m_wMap.DieNumbers = null;
            m_wMap.DieClear();
            m_wMap.EdgeSize = 0d;
            m_wMap.WaferSize = 300000d;
            m_wMap.DieSizeX = 1d;
            m_wMap.DieSizeY = 1d;
            m_wMap.OriginX = 0d;
            m_wMap.OriginY = 0d;
            m_wMap.Redraw();
        }

        /// <summary>[Map 정보] 탭: TQP_MAPDEF 한 행을 항목 / 값으로 보인다. 원점 Y 는 조회 쿼리가 되돌린 화면 기준 값이다.</summary>
        private void ShowInfo(StoredMapLayout layout)
        {
            DataRow row = null;
            grdInfo.Rows.Clear();
            AddInfo("Device (Map ID)", layout.MapId);
            AddInfo("Factory", Dash(layout.Factory));
            AddInfo("Wafer 크기", string.Format(CultureInfo.InvariantCulture, "{0:#,0.###} µm ({1:0.###} mm)", layout.WaferSize, layout.WaferSize / 1000d));
            AddInfo("Die 크기 X / Y", string.Format(CultureInfo.InvariantCulture, "{0:#,0.###} / {1:#,0.###} µm", layout.DieSizeX, layout.DieSizeY));
            AddInfo("Edge", string.Format(CultureInfo.InvariantCulture, "{0:0.###} mm", layout.EdgeSizeMm)
                + (layout.IsEdgeValid ? string.Empty : " (0 ~ 5 mm 밖 / Map 은 Edge 0 으로 그림)"));
            AddInfo("Notch", string.Format(CultureInfo.InvariantCulture, "{0} / {1}°", layout.NotchType, layout.NotchAngle));
            AddInfo("XY 방향", layout.IsDirectionValid
                ? string.Format(CultureInfo.InvariantCulture, "{0} ({1})", layout.Direction, layout.StoredXYDirection)
                : "알 수 없음 (" + layout.StoredXYDirection + ")");
            AddInfo("원점 X / Y (µm)", string.Format(CultureInfo.InvariantCulture, "{0:#,0.###} / {1:#,0.###}", layout.OriginX, layout.OriginY));
            AddInfo("원점 인덱스 (저장)", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", layout.StoredOriginIndex.X, layout.StoredOriginIndex.Y));
            AddInfo("원점 인덱스 (LeftBottom)", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", layout.LeftBottomOriginIndex.X, layout.LeftBottomOriginIndex.Y));
            AddInfo("First Index", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", layout.FirstIndexX, layout.FirstIndexY));
            AddInfo("Die 범위 (저장)", string.Format(CultureInfo.InvariantCulture, "X {0}~{1} / Y {2}~{3}",
                layout.StoredGrid.MinX, layout.StoredGrid.MaxX, layout.StoredGrid.MinY, layout.StoredGrid.MaxY));
            AddInfo("Net Die (저장)", layout.StoredNetDie.ToString(CultureInfo.InvariantCulture));
            AddInfo("Reference Die", layout.ReferenceDieSetting.ToString(CultureInfo.InvariantCulture));
            if (mapList != null)
            {
                DataRow[] found = mapList.Select("MAPID = '" + layout.MapId.Replace("'", "''") + "'");
                if (found.Length > 0) row = found[0];
            }
            // 등록 / 수정 정보는 GET_MAPDEF 와 목록 쿼리 모두에 있다. 저장 쿼리가 값을 넣지 않아 비어 있을 수 있다.
            AddInfo("등록", Dash(ListText(row, "INSERT_TIME")) + " / " + Dash(ListText(row, "INSERT_USER")));
            AddInfo("수정", Dash(ListText(row, "UPDATE_TIME")) + " / " + Dash(ListText(row, "UPDATE_USER")));
            grdInfo.ClearSelection();
        }

        private void AddInfo(string name, string value)
        {
            grdInfo.Rows.Add(name, value);
        }

        /// <summary>[Die 번호 표시] 체크: Map 위 번호만 켜고 끈다 (번호 계산은 조회할 때 이미 했다).</summary>
        private void chkDieNumber_CheckedChanged(object sender, EventArgs e)
        {
            m_wMap.ShowDieNumbers = chkDieNumber.Checked;
            if (currentLayout != null && currentLayout.IsDirectionValid) m_wMap.Redraw();
        }

        /// <summary>
        /// 숨은 탭의 표는 보이게 될 때 첫 행을 자동 선택한다. 정보 / 확인 표는 선택이 의미가 없으므로 보일 때 선택을 지운다.
        /// </summary>
        private void InfoGrid_VisibleChanged(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid.Visible) grid.ClearSelection();
        }

        /// <summary>[데이터 확인] 탭: 항목 / 저장값 / Die 행(기준) / 결과 / 설명. 다름은 빨간 글자, 참고는 회색.</summary>
        private void ShowChecks(StoredMapLayout layout)
        {
            grdCheck.Rows.Clear();
            foreach (StoredMapCheck check in layout.Checks)
            {
                int index = grdCheck.Rows.Add(check.Name, check.StoredValue, check.DieValue, check.ResultText, check.Description);
                DataGridViewCell cell = grdCheck.Rows[index].Cells[colCheckResult.Index];
                if (check.Result == StoredMapCheckResult.Differ)
                {
                    cell.Style.ForeColor = Color.Red;
                    cell.Style.Font = new Font(grdCheck.Font, FontStyle.Bold);
                }
                else if (check.Result == StoredMapCheckResult.Note)
                {
                    cell.Style.ForeColor = Color.DimGray;
                }
                else
                {
                    cell.Style.ForeColor = Color.Green;
                }
            }
            grdCheck.ClearSelection();
            int differ = layout.DifferCount;
            tabCheck.Text = differ == 0 ? "데이터 확인" : "데이터 확인 (다름 " + differ + ")";
        }

        /// <summary>
        /// [Die 목록] 탭: 저장 번호 / LeftBottom 번호 / USECODE / Wafer 안 / Die 번호 (Direction) / DIE_NUM (저장).
        /// 저장 번호 Y, X 순으로 정렬한다 (= Direction 행 우선 순서라 Die 번호가 1 부터 차례로 보인다).
        /// 기존 Map 중에는 Die 가 수만 개인 것도 있어 행을 하나씩 추가하지 않고 DataTable 로 묶는다.
        /// </summary>
        private void ShowDies(StoredMapLayout layout)
        {
            DataTable table = new DataTable();
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("LB_X", typeof(int));
            table.Columns.Add("LB_Y", typeof(int));
            table.Columns.Add("USECODE", typeof(int));
            table.Columns.Add("INSIDE", typeof(string));
            table.Columns.Add("DIE_NO_DIR", typeof(int));
            table.Columns.Add("DIE_NUM", typeof(int));
            StoredMapDie[] sorted = layout.Dies.ToArray();
            Array.Sort(sorted, CompareStoredDie);
            table.BeginLoadData();
            foreach (StoredMapDie die in sorted)
            {
                string inside = die.InsideWafer.HasValue ? (die.InsideWafer.Value ? "예" : "아니오") : "-";
                table.Rows.Add(die.Stored.X, die.Stored.Y, die.LeftBottom.X, die.LeftBottom.Y, die.UseCode, inside,
                    die.DirectionNumber.HasValue ? (object)die.DirectionNumber.Value : DBNull.Value,
                    die.StoredDieNum.HasValue ? (object)die.StoredDieNum.Value : DBNull.Value);
            }
            table.EndLoadData();
            SetDieTable(table);
            tabDies.Text = "Die 목록 (" + layout.Dies.Count.ToString("#,0", CultureInfo.InvariantCulture) + ")";
        }

        /// <summary>Die 목록 표의 DataSource 를 바꾸고 이전 DataTable 은 해제한다.</summary>
        private void SetDieTable(DataTable table)
        {
            DataTable old = grdDies.DataSource as DataTable;
            grdDies.DataSource = table;
            if (old != null) old.Dispose();
        }

        private static int CompareStoredDie(StoredMapDie a, StoredMapDie b)
        {
            int y = a.Stored.Y.CompareTo(b.Stored.Y);
            return y != 0 ? y : a.Stored.X.CompareTo(b.Stored.X);
        }

        /// <summary>Map / 탭 / 개수 줄을 비우고 상태줄에 이유를 적는다. 조회 실패 후 다른 Map 정보가 남지 않게 한다.</summary>
        private void ClearMap(string status)
        {
            currentLayout = null;
            hoverDie = null;
            ResetMapControl();
            grdInfo.Rows.Clear();
            grdCheck.Rows.Clear();
            SetDieTable(null);
            tabCheck.Text = "데이터 확인";
            tabDies.Text = "Die 목록";
            lblDieCount.Text = "Map: -";
            ClearCoordinates();
            lblStatus.Text = status;
        }

        #endregion

        #region 좌표 표시

        /// <summary>Map 컨트롤이 판정한 마우스 아래 Die 를 보관한다. 위치는 OnChangePosition 에서 표시한다.</summary>
        private void m_wMap_OnChangeCurrentDie(object sender, Die die)
        {
            if (currentLayout == null) return;
            hoverDie = die;
        }

        /// <summary>
        /// 마우스 위치 Die 의 저장 방향 번호 / LeftBottom 번호 / USECODE 와 Wafer 중심 기준 µm 를 보인다.
        /// 화면 Map 은 LeftBottom 이므로 저장 번호는 KlarfMapRule.ToDirection (저장 Die 범위 기준) 으로 다시 계산한다.
        /// </summary>
        private void m_wMap_OnChangePosition(object sender, PointD point, PointD mapPoint)
        {
            if (currentLayout == null || object.ReferenceEquals(hoverDie, null)) return;
            Point lb = new Point(hoverDie.IndexX, hoverDie.IndexY);
            Point stored = KlarfMapRule.ToDirection(lb, currentLayout.Direction, currentLayout.StoredGrid);
            int number;
            string numberText = m_wMap.DieNumbers.TryGetValue(lb, out number) ? number.ToString(CultureInfo.InvariantCulture) : "-";
            lblCoordinates.Text = string.Format(CultureInfo.InvariantCulture,
                "Die ({0}) X/Y: {1}, {2} / LeftBottom: {3}, {4} / USECODE: {5} ({6}) / Die 번호: {7}\r\n마우스 Wafer 중심 기준: X={8:#,0.###}, Y={9:#,0.###} µm",
                currentLayout.Direction, stored.X, stored.Y, lb.X, lb.Y, hoverDie.DieProp, UseCodeName(hoverDie.DieProp), numberText,
                point.X, point.Y);
        }

        /// <summary>Die 가 없는 영역에서는 Map 컨트롤이 위치 이벤트를 보내지 않으므로 좌표를 먼저 지운다.</summary>
        private void m_wMap_MouseMove(object sender, MouseEventArgs e) { ClearCoordinates(); }

        /// <summary>Map 밖으로 이동하면 마지막 마우스 좌표를 지운다.</summary>
        private void m_wMap_MouseLeave(object sender, EventArgs e) { ClearCoordinates(); }

        private void ClearCoordinates()
        {
            lblCoordinates.Text = "Die X/Y: - / LeftBottom: - / USECODE: - / Die 번호: -\r\n마우스 Wafer 중심 기준: -";
        }

        #endregion

        #region 표시 문자열

        private static string UseCodeName(int useCode)
        {
            if (useCode == 1) return "사용";
            if (useCode == -1) return "제거";
            return "기타";
        }

        /// <summary>목록 Wafer 칸: WAFER_SIZE(µm) / 1000. 값이 없으면 빈칸.</summary>
        private static string WaferMmText(DataRow row)
        {
            if (!row.Table.Columns.Contains("WAFER_SIZE") || row.IsNull("WAFER_SIZE")) return string.Empty;
            double micron = System.Convert.ToDouble(row["WAFER_SIZE"], CultureInfo.InvariantCulture);
            return (micron / 1000d).ToString("0.###", CultureInfo.InvariantCulture);
        }

        /// <summary>목록 XY 방향 칸: XYDirection 이름. 알 수 없는 값은 숫자 그대로.</summary>
        private static string DirectionText(DataRow row)
        {
            if (!row.Table.Columns.Contains("XY_DIRECTION") || row.IsNull("XY_DIRECTION")) return string.Empty;
            int value = System.Convert.ToInt32(row["XY_DIRECTION"], CultureInfo.InvariantCulture);
            return Enum.IsDefined(typeof(XYDirection), value) ? ((XYDirection)value).ToString() : value.ToString(CultureInfo.InvariantCulture);
        }

        private static string ListText(DataRow row, string column)
        {
            if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return string.Empty;
            return System.Convert.ToString(row[column], CultureInfo.InvariantCulture);
        }

        private static string Dash(string value)
        {
            return string.IsNullOrEmpty(value) ? "-" : value;
        }

        /// <summary>DataView.RowFilter 의 LIKE 값에서 특수 문자(* % [ ])는 [] 로 감싸고 작은따옴표는 두 번 쓴다.</summary>
        private static string EscapeLike(string value)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in value)
            {
                if (c == '*' || c == '%' || c == '[' || c == ']') sb.Append('[').Append(c).Append(']');
                else if (c == '\'') sb.Append("''");
                else sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion
    }
}
