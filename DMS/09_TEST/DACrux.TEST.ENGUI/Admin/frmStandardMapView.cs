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
    /// 2. 목록에서 Map 을 고르면 GetMapDef + GetDies 로 정의 1행과 Die 행을 읽고 StoredMapLayout 으로 화면 기준 값과 확인 항목을 만든다.
    /// 3. Map 컨트롤은 LeftBottom 번호로 그린다 (StoredMapLayout 주석 참고). 좌표 표시는 저장 방향 번호와 LeftBottom 번호를 함께 보인다.
    /// 4. 오른쪽 탭: Map 정보 (TQP_MAPDEF 값) 하나만 둔다. Device 검색칸 / 데이터 확인 탭 / Die 목록 탭은 지웠다 (2026-10-15).
    ///    Map 을 그리지 못하면(XY 방향 값 오류 / 그리기 예외) 이유를 상태줄에 보인다.
    /// 5. Die 번호: 저장 XY 방향 기준으로 화면에서 계산해 Die 위에 그린다 (DieNumberRule, 2026-10-13).
    /// Factory 선택은 두지 않는다. 저장 쿼리(CREATE_MAP)가 FACTORY 를 넣지 않아 Factory 로 거르면 새로 만든 Map 이 빠질 수 있다.
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-13_Wafer_Map_조회화면_설계.md
    /// </summary>
    public partial class frmStandardMapView : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>마지막으로 조회한 Map 목록.</summary>
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
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            grdMapList.AutoGenerateColumns = false;
            ClearMap(string.Empty);
        }

        #region Map 목록

        /// <summary>화면을 열면 Map 목록을 한 번 읽는다.</summary>
        private void frmStandardMapView_Load(object sender, EventArgs e)
        {
            LoadMapList(false);
        }

        /// <summary>[조회]: 목록을 다시 읽는다. 보던 Map 이 목록에 남아 있으면 다시 골라 정의 / Die 를 새로 읽는다.</summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMapList(true);
        }

        /// <summary>
        /// 기존 서비스 ProbeAdmin.GetMapIDAllList (GET_MAPDEF_ALL: DELETE_FLAG 가 Y 가 아닌 Map 전체, MAPID 순) 를 부른다.
        /// 원격 호출이 실패하면 이전 목록 / Map 을 지워 다른 Map 정보가 남지 않게 한다.
        /// 진행 표시(2026-10-14): [조회] 는 작업 스레드에서(background = true), 화면 Load 중에는 화면 스레드에서 읽고 표시만 한다.
        /// </summary>
        private void LoadMapList(bool background)
        {
            string keepMapId = currentLayout == null ? null : currentLayout.MapId;
            try
            {
                Func<DataTable> query = () => new DACrux.TEST.RO.ProbeAdmin().GetMapIDAllList();
                ShowMapList(background ? busyBar.Run("Map 목록 조회", query) : busyBar.RunHere("Map 목록 조회", query), keepMapId);
            }
            catch (Exception ex)
            {
                ShowMapList(null, null);
                lblStatus.Text = "Map 목록 조회 실패";
                MessageBox.Show(this, ex.Message, "Map 목록 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                UpdateListCount();
                grdMapList.ClearSelection();
            }
            finally
            {
                bindingList = false;
            }

            if (!string.IsNullOrEmpty(keepMapId) && SelectMapRow(keepMapId)) return;
            ClearMap(mapList == null ? "Map 목록 없음" : string.Empty);
        }


        /// <summary>목록 건수를 보인다.</summary>
        private void UpdateListCount()
        {
            lblListCount.Text = string.Format(CultureInfo.InvariantCulture, "{0} 건", mapList == null ? 0 : mapList.Rows.Count);
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
        /// 표가 DataSource 를 묶은 뒤 첫 행을 자동 선택하므로 선택을 지운다.
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
        /// 목록 선택 변경 이벤트에서 불리므로 진행 표시줄은 화면 스레드 실행(RunHere)으로 표시만 한다.
        /// </summary>
        private void LoadSelectedMap()
        {
            string mapId = SelectedMapId();
            if (mapId == null) return;
            try
            {
                // 서버 조회부터 Map 그리기(수만 Die)까지 한 작업으로 묶어 모래시계를 유지한다 (2026-10-15)
                using (busyBar.Begin("Map 조회 (" + mapId + ")"))
                {
                    DataTable[] data = busyBar.RunHere("서버 조회", () =>
                    {
                        DACrux.TEST.RO.ProbeAdmin admin = new DACrux.TEST.RO.ProbeAdmin();
                        return new DataTable[] { admin.GetMapDef(mapId), admin.GetDies(mapId) };
                    });
                    using (DataTable mapDef = data[0])
                    using (DataTable dies = data[1])
                    {
                        ShowStoredMap(mapDef, dies);
                    }
                }
            }
            catch (Exception ex)
            {
                ClearMap("Map 조회 실패: " + mapId);
                MessageBox.Show(this, ex.Message, "Map 조회", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Map 표시

        /// <summary>
        /// 저장된 Map 정의 / Die 행을 화면에 보인다. 서비스 호출과 나눈 이유: 화면 밖 검사가 DataTable 을 직접 넣어 확인하기 위해서다.
        /// 흐름: StoredMapLayout 계산 -> Map 그리기(XY 방향 값이 올바를 때만) -> Map 정보 탭 / 개수 줄 / 상태줄 갱신.
        /// </summary>
        internal void ShowStoredMap(DataTable mapDef, DataTable dies)
        {
            if (mapDef == null || mapDef.Rows.Count == 0)
            {
                ClearMap("Map 정의(TQP_MAPDEF) 없음");
                return;
            }

            StoredMapLayout layout = StoredMapLayout.Create(mapDef.Rows[0], dies);
            ClearCoordinates();
            string drawError = null;
            if (layout.IsDirectionValid)
            {
                try
                {
                    DrawStoredMap(layout);
                }
                catch (Exception ex)
                {
                    // 저장값이 Map 컨트롤이 그릴 수 없는 값이어도 Map 정보는 보인다.
                    ResetMapControl();
                    drawError = "Map 그리기 실패 (" + ex.Message + ")";
                }
            }
            else
            {
                ResetMapControl();
                drawError = "XY 방향 값 오류 (" + layout.StoredXYDirection.ToString(CultureInfo.InvariantCulture) + ") - Map 을 그리지 않음";
            }

            currentLayout = layout;
            ShowInfo(layout);
            // XY 방향 / 원점 등 나머지 값은 [Map 정보] 탭에 있으므로 개수 줄은 한 줄에 들어가는 값만 보인다.
            lblDieCount.Text = string.Format(CultureInfo.InvariantCulture,
                "{0}  /  사용(1) {1:#,0}  /  제거(-1) {2:#,0}  /  기타 {3:#,0}  /  Die 행 {4:#,0}",
                layout.MapId, layout.UsedCount, layout.RemovedCount, layout.OtherCount, layout.Dies.Count);
            lblStatus.Text = "조회 완료: " + layout.MapId + (drawError == null ? string.Empty : " / " + drawError);
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
        /// 표는 보이게 될 때 첫 행을 자동 선택한다. Map 정보 표는 선택이 의미가 없으므로 보일 때 선택을 지운다.
        /// </summary>
        private void InfoGrid_VisibleChanged(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid.Visible) grid.ClearSelection();
        }

        /// <summary>Map / 탭 / 개수 줄을 비우고 상태줄에 이유를 적는다. 조회 실패 후 다른 Map 정보가 남지 않게 한다.</summary>
        private void ClearMap(string status)
        {
            currentLayout = null;
            hoverDie = null;
            ResetMapControl();
            grdInfo.Rows.Clear();
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
                "Die ({0}) X/Y: {1}, {2} / LeftBottom: {3}, {4} / USECODE: {5} ({6}) / Die 번호: {7}   |   마우스 X={8:#,0.###}, Y={9:#,0.###} µm",
                currentLayout.Direction, stored.X, stored.Y, lb.X, lb.Y, hoverDie.DieProp, UseCodeName(hoverDie.DieProp), numberText,
                point.X, point.Y);
        }

        /// <summary>Die 가 없는 영역에서는 Map 컨트롤이 위치 이벤트를 보내지 않으므로 좌표를 먼저 지운다.</summary>
        private void m_wMap_MouseMove(object sender, MouseEventArgs e) { ClearCoordinates(); }

        /// <summary>Map 밖으로 이동하면 마지막 마우스 좌표를 지운다.</summary>
        private void m_wMap_MouseLeave(object sender, EventArgs e) { ClearCoordinates(); }

        private void ClearCoordinates()
        {
            lblCoordinates.Text = string.Empty;
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

        #endregion
    }
}
