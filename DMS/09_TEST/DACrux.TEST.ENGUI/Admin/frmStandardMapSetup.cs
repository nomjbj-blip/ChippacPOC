using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// File Map Setup 화면. KLARF 파일로 Wafer 원 형태의 표준 Map(TQP_MAPDEF / TQP_USEMAP)을 만든다.
    /// 왜: AOI(KLARF) / ET Map / GDS 를 같은 Die 격자와 같은 인덱스로 맞추기 위해서다. (사용자 결정 2026-10-06 "klarf 로 하자")
    /// 역할 / 흐름:
    /// 1. KlarfMapSource 로 파일을 읽는다. Die 인덱스는 DMS 가 KLARF 를 저장할 때와 같은 좌하단 (1,1) 기준이다.
    /// 2. [원형 생성] 에서 KlarfMapRule 로 Wafer 전체 격자를 구해 AddDie 로 넣고, Die 마다 원 안이면 1, 밖이면 -1 로 둔다.
    /// 3. 작업자는 Map 컨트롤 Edit 메뉴로 Die 를 지우거나 되살린다. (편집 모드 / 보조 버튼은 구현 3단계)
    /// 4. [새 Map 저장] 은 기존 ProbeAdmin 저장 경로(MapCreationService)를 그대로 쓴다. 단위는 µm, 제거 Die 는 USECODE = -1.
    ///    단 EDGE_SIZE 는 mm, ORIGIN_MICRO_Y 는 DieSizeY - OriginY 로 저장한다 (기존 데이터 / 조회 쿼리 규칙, MapCreationService.ToStoredValues).
    /// 설계: 문서/2026-10-06_KLARF_Map_Setup_설계.md
    /// </summary>
    public partial class frmStandardMapSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private KlarfMapSource source;
        private KlarfMapWafer currentWafer;
        private KlarfMapGrid currentGrid;
        private XYDirection currentDirection = XYDirection.LeftBottom;
        /// <summary>현재 Wafer 의 검사 Die (LeftBottom 인덱스).</summary>
        private readonly HashSet<Point> inspectedDies = new HashSet<Point>();
        /// <summary>동일 Die 에서는 변경 이벤트가 반복되지 않으므로 마지막 Die 를 유지한다.</summary>
        private Die hoverDie;
        private string savedMapId;
        /// <summary>원형 생성 후 작업자가 Die 를 바꿨는지. 다시 생성할 때 확인창을 띄운다.</summary>
        private bool dieEdited;
        /// <summary>코드에서 설정 컨트롤 값을 넣는 동안 변경 이벤트를 무시한다.</summary>
        private bool updatingSettings;

        /// <summary>화면 구성과 이벤트 연결은 InitializeComponent 에서 한 번에 초기화한다.</summary>
        public frmStandardMapSetup()
        {
            InitializeComponent();
        }

        #region 파일 읽기

        /// <summary>선택한 파일을 즉시 읽는다. 선택 취소 시 현재 상태를 유지한다.</summary>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "KLARF 파일 선택";
                dialog.Filter = "모든 파일|*.*";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtFile.Text = dialog.FileName;
                btnParse_Click(sender, e);
            }
        }

        /// <summary>파일 경로가 바뀌면 이전 미리보기와 저장 상태를 폐기해 다른 파일의 저장을 막는다.</summary>
        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            ClearMap();
        }

        /// <summary>
        /// KLARF 를 읽고 설정 초기값(Map ID / Wafer / 크기 / 방향 / 원점)을 채운 뒤 원형 Map 을 바로 만든다.
        /// 원점 조회 함수는 아직 null 이다 (TQD_SETUP 조회는 구현 4단계). 이때 옮긴 양은 WaferDieCalculator 값이며
        /// DMS 업로드 서비스도 TQD_SETUP 에 값이 없으면 같은 값을 쓴다.
        /// </summary>
        private void btnParse_Click(object sender, EventArgs e)
        {
            ClearMap();
            try
            {
                Cursor = Cursors.WaitCursor;
                KlarfMapSource loaded = KlarfMapSource.Load(txtFile.Text.Trim(), null);

                updatingSettings = true;
                source = loaded;
                cmbKlarfWafer.Items.Clear();
                foreach (KlarfMapWafer wafer in loaded.Wafers)
                    cmbKlarfWafer.Items.Add(string.Format("{0} (Slot {1})", wafer.WaferID, wafer.Slot));
                cmbKlarfWafer.Enabled = loaded.Wafers.Count > 1;
                cmbKlarfWafer.SelectedIndex = 0;
                SelectWaferSize(loaded.SampleSize);
                cmbXYDirection.SelectedItem = XYDirection.LeftBottom.ToString();
                numEdge.Value = 0;
                txtMapID.Text = loaded.DeviceID;
                ApplyWaferOrigin(loaded.Wafers[0]);
                updatingSettings = false;

                GenerateCircleMap();
                lblStatus.Text = "읽기 완료 / 원형 Map 을 만들었습니다. Edit Map 메뉴로 필요 없는 Die 를 지우세요.";
            }
            catch (Exception ex)
            {
                updatingSettings = false;
                ClearMap();
                lblStatus.Text = "파일 읽기 실패 / 파일 내용을 확인하세요.";
                MessageBox.Show(this, ex.Message, "KLARF 파일 확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>파일 SampleSize(mm) 를 Wafer 콤보에서 고른다. 목록에 없으면 항목을 추가한다.</summary>
        private void SelectWaferSize(int sampleSize)
        {
            string text = sampleSize.ToString(CultureInfo.InvariantCulture);
            int index = cmbWaferSize.Items.IndexOf(text);
            if (index < 0) index = cmbWaferSize.Items.Add(text);
            cmbWaferSize.SelectedIndex = index;
        }

        /// <summary>선택 Wafer 의 SampleCenterLocation(µm) 을 원점 입력칸에 넣는다.</summary>
        private void ApplyWaferOrigin(KlarfMapWafer wafer)
        {
            numOriginX.Value = ClampOrigin(numOriginX, wafer.CenterX);
            numOriginY.Value = ClampOrigin(numOriginY, wafer.CenterY);
        }

        private static decimal ClampOrigin(NumericUpDown box, double value)
        {
            return Math.Max(box.Minimum, Math.Min(box.Maximum, (decimal)value));
        }

        /// <summary>부분 실패나 파일 변경 후 이전 Map 이 저장되지 않도록 상태를 초기화한다.</summary>
        private void ClearMap()
        {
            ClearCoordinates();
            source = null;
            currentWafer = null;
            currentGrid = null;
            inspectedDies.Clear();
            hoverDie = null;
            savedMapId = null;
            dieEdited = false;
            btnSave.Enabled = false;
            btnOpenMap.Enabled = false;
            btnGenerate.Enabled = false;
            txtInfo.Clear();
            ClearDieCount();
            lblStatus.Text = "KLARF 파일을 선택하세요.";
            m_wMap.DieClear();
            m_wMap.Redraw();
        }

        #endregion

        #region 원형 생성

        /// <summary>파일 안의 다른 Wafer 를 고르면 그 Wafer 의 중심 / 검사 Die 로 다시 만든다.</summary>
        private void cmbKlarfWafer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updatingSettings || source == null || cmbKlarfWafer.SelectedIndex < 0) return;
            if (!ConfirmRegenerate())
            {
                updatingSettings = true;
                cmbKlarfWafer.SelectedIndex = source.Wafers.IndexOf(currentWafer);
                updatingSettings = false;
                return;
            }
            updatingSettings = true;
            ApplyWaferOrigin(source.Wafers[cmbKlarfWafer.SelectedIndex]);
            updatingSettings = false;
            GenerateCircleMap();
        }

        /// <summary>Wafer 크기 / Edge / XY 방향 / 원점이 바뀌면 [원형 생성] 을 눌러 반영하도록 안내만 한다.</summary>
        private void MapSettingsChanged(object sender, EventArgs e)
        {
            if (updatingSettings || source == null) return;
            lblStatus.Text = "설정이 바뀌었습니다. [원형 생성] 을 눌러 반영하세요.";
        }

        /// <summary>현재 설정으로 원형 Map 을 다시 만든다. 편집 내용이 있으면 먼저 확인한다.</summary>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (source == null || !ConfirmRegenerate()) return;
            try
            {
                GenerateCircleMap();
                lblStatus.Text = "원형 Map 을 다시 만들었습니다.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "원형 생성", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>작업자가 Die 를 바꾼 뒤라면 다시 만들지 묻는다.</summary>
        private bool ConfirmRegenerate()
        {
            if (!dieEdited) return true;
            return MessageBox.Show(this, "원형을 다시 만들면 Die 편집 내용이 초기화됩니다. 계속하시겠습니까?", "원형 생성",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// 원형 Map 생성. 순서:
        /// 1. KlarfMapRule.CalculateGrid 로 선택한 Wafer 크기의 전체 격자를 구한다 (LeftBottom, DMS 인덱스와 같은 기준).
        /// 2. 선택한 XY 방향에 맞춰 원점 인덱스(MapOriginIndex)와 Die 번호(ToDirection)를 바꾼다. 형상은 그대로다.
        /// 3. Map 설정 -> Redraw (Notch / Flat 영역 계산) -> DieClear -> 격자 Die 를 AddDie.
        ///    DieCalculation(true) 는 방향에 따라 AddDie 와 Die 위치가 한 칸 달라 쓰지 않는다.
        /// 4. Die 마다 IsInsideWafer 로 원 안 = 1, 밖 = -1 을 정하고 다시 그린다.
        /// </summary>
        private void GenerateCircleMap()
        {
            ClearCoordinates();
            currentWafer = source.Wafers[Math.Max(0, cmbKlarfWafer.SelectedIndex)];
            currentDirection = (XYDirection)Enum.Parse(typeof(XYDirection), cmbXYDirection.Text);
            double waferSize = double.Parse(cmbWaferSize.Text, CultureInfo.InvariantCulture) * 1000d;
            double originX = (double)numOriginX.Value;
            double originY = (double)numOriginY.Value;

            currentGrid = KlarfMapRule.CalculateGrid(waferSize, source.PitchX, source.PitchY,
                originX, originY, currentWafer.ShiftX, currentWafer.ShiftY);
            Point originIndex = KlarfMapRule.MapOriginIndex(currentDirection, currentGrid);

            inspectedDies.Clear();
            foreach (Point point in currentWafer.InspectedDies) inspectedDies.Add(point);

            m_wMap.DieClear();
            m_wMap.ViewAngle = 0;
            m_wMap.AngleOffSet = 0;
            m_wMap.WaferSize = waferSize;
            // Edge 는 Wafer 크기를 정한 뒤 넣는다 (KLARF File View 와 같은 순서).
            m_wMap.EdgeSize = (double)numEdge.Value;
            m_wMap.NotchType = source.IsFlat ? Notch.Flat : Notch.Notch;
            m_wMap.NotchAngle = source.NotchAngle;
            m_wMap.XYDirect = currentDirection;
            m_wMap.DieSizeX = source.PitchX;
            m_wMap.DieSizeY = source.PitchY;
            m_wMap.ReferenceDieSetting = 0;
            m_wMap.OriginIndexX = originIndex.X;
            m_wMap.OriginIndexY = originIndex.Y;
            m_wMap.OriginX = originX;
            m_wMap.OriginY = originY;
            m_wMap.DieMinX = currentGrid.MinX;
            m_wMap.DieMaxX = currentGrid.MaxX;
            m_wMap.DieMinY = currentGrid.MinY;
            m_wMap.DieMaxY = currentGrid.MaxY;
            m_wMap.FirstDieX = currentGrid.MinX;
            m_wMap.FirstDieY = currentGrid.MinY;
            m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            m_wMap.Redraw();

            for (int x = currentGrid.MinX; x <= currentGrid.MaxX; x++)
            {
                for (int y = currentGrid.MinY; y <= currentGrid.MaxY; y++)
                {
                    Point index = KlarfMapRule.ToDirection(new Point(x, y), currentDirection, currentGrid);
                    m_wMap.AddDie(new Die(index.X, index.Y, 0, 0, 0, 0, 0, 1));
                }
            }
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Die die = m_wMap.Dies[i];
                die.DieProp = m_wMap.IsInsideWafer(die) ? 1 : -1;
            }

            dieEdited = false;
            savedMapId = null;
            btnOpenMap.Enabled = false;
            btnGenerate.Enabled = true;
            btnSave.Enabled = true;
            m_wMap.Redraw();
            UpdateInfo();
        }

        /// <summary>Edit 메뉴로 Die 속성이 바뀌면 편집 표시를 하고 정보를 갱신한다.</summary>
        private void m_wMap_OnChangeDieProperty(object sender, Die die)
        {
            if (source == null) return;
            dieEdited = true;
            UpdateInfo();
        }

        #endregion

        #region 정보 / 좌표 표시

        /// <summary>
        /// 저장될 값과 판단 근거를 보여준다. Map 값은 µm 이고 mm 를 함께 적는다.
        /// LeftBottom 이 아니면 DMS Defect 인덱스와 번호가 다르다는 것을 알린다.
        /// Map 위 개수 줄(lblDieCount)도 함께 갱신한다 (2026-10-07 사용자 요청: Net Die / 생성 Die 개수 표시).
        /// 원형 생성 후와 Edit 메뉴로 Die 속성이 바뀔 때마다 불린다.
        /// </summary>
        private void UpdateInfo()
        {
            if (source == null || currentGrid == null) { txtInfo.Clear(); ClearDieCount(); return; }

            int used = 0, removed = 0, other = 0, inspectedRemoved = 0;
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Die die = m_wMap.Dies[i];
                if (die.DieProp == 1) used++;
                else if (die.DieProp == -1) removed++;
                else other++;
                if (die.DieProp == -1 && inspectedDies.Contains(ToLeftBottom(die))) inspectedRemoved++;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DEVICE: " + source.DeviceID);
            sb.AppendLine("LOT: " + source.LotID);
            sb.AppendLine(string.Format("WAFER: {0} (Slot {1})", currentWafer.WaferID, currentWafer.Slot));
            sb.AppendLine("SETUP / STEP: " + source.SetupID + " / " + source.StepID);
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Wafer: {0:#,0} µm (파일 {1} mm)", m_wMap.WaferSize, source.SampleSize));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Die Pitch: {0:#,0.###} x {1:#,0.###} µm", source.PitchX, source.PitchY));
            sb.AppendLine(string.Format("Notch: {0} / {1} ({2}°)", source.MarkType, source.MarkLocation, source.NotchAngle));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "원점(Center): {0:#,0.###}, {1:#,0.###} µm", m_wMap.OriginX, m_wMap.OriginY));
            MapDefStoredValues stored = MapCreationService.ToStoredValues(m_wMap);
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "저장값: EDGE {0:0.###} mm / ORIGIN_MICRO {1:#,0.###}, {2:#,0.###}",
                stored.EdgeSizeMm, stored.OriginMicroX, stored.OriginMicroY));
            sb.AppendLine(string.Format("KLARF (0,0) -> LeftBottom ({0}, {1}) / 출처: {2}", currentWafer.ShiftX, currentWafer.ShiftY,
                source.OriginFromDB ? "TQD_SETUP" : "계산기"));
            sb.AppendLine(string.Format("격자 (LeftBottom): X {0}~{1} / Y {2}~{3}", currentGrid.MinX, currentGrid.MaxX, currentGrid.MinY, currentGrid.MaxY));
            sb.AppendLine(string.Format("XY 방향: {0} / 원점 인덱스 {1}, {2}", currentDirection, m_wMap.OriginIndexX, m_wMap.OriginIndexY));
            if (currentDirection != XYDirection.LeftBottom)
                sb.AppendLine("※ LeftBottom 이 아니면 DMS Defect 인덱스와 번호가 다릅니다.");
            sb.AppendLine();
            sb.AppendLine(string.Format("사용 Die: {0} / 제거(-1): {1} / 기타: {2}", used, removed, other));
            sb.AppendLine(string.Format("검사 Die: {0} / 그중 제거됨: {1}", inspectedDies.Count, inspectedRemoved));
            sb.AppendLine("Defect: " + currentWafer.DefectCount);
            txtInfo.Text = sb.ToString();

            // Net Die = 저장 시 NETDIE 로 들어가는 값 (사용 Die = DieProp 1). 생성 Die = 격자 전체로 만든 Die 수 (TQP_USEMAP 저장 행 수).
            lblDieCount.Text = string.Format(CultureInfo.InvariantCulture,
                "Net Die: {0:#,0}   /   생성 Die: {1:#,0} (격자 {2} x {3})   /   제거(-1): {4:#,0}   /   기타: {5:#,0}   /   검사 Die {6:#,0} 중 사용 {7:#,0}",
                used, m_wMap.Dies.Count, currentGrid.MaxX - currentGrid.MinX + 1, currentGrid.MaxY - currentGrid.MinY + 1,
                removed, other, inspectedDies.Count, inspectedDies.Count - inspectedRemoved);
        }

        /// <summary>파일이 없거나 Map 을 비웠을 때 개수 줄을 초기 문구로 되돌린다.</summary>
        private void ClearDieCount()
        {
            lblDieCount.Text = "Net Die: - / 생성 Die: -";
        }

        /// <summary>Map 의 Die 번호를 LeftBottom 인덱스로 되돌린다.</summary>
        private Point ToLeftBottom(Die die)
        {
            return KlarfMapRule.FromDirection(new Point(die.IndexX, die.IndexY), currentDirection, currentGrid);
        }

        /// <summary>Map 컨트롤이 판정한 마우스 아래 Die 를 보관한다. 위치는 OnChangePosition 에서 표시한다.</summary>
        private void m_wMap_OnChangeCurrentDie(object sender, Die die)
        {
            if (source == null) return;
            hoverDie = die;
        }

        /// <summary>
        /// 마우스 위치의 Die 번호(선택 방향) / LeftBottom 인덱스(DMS) / KLARF 원본 인덱스 / Wafer 중심 기준 µm 를 보여준다.
        /// KLARF 원본 인덱스 = LeftBottom - 옮긴 양. 이후 GDS 좌표 변환에 쓰는 값이다.
        /// </summary>
        private void m_wMap_OnChangePosition(object sender, PointD point, PointD mapPoint)
        {
            if (source == null || currentGrid == null || object.ReferenceEquals(hoverDie, null)) return;
            Point lb = ToLeftBottom(hoverDie);
            lblCoordinates.Text = string.Format(CultureInfo.InvariantCulture,
                "Die ({0}) X/Y: {1}, {2} / LeftBottom: {3}, {4} / KLARF 원본: {5}, {6} / 검사 Die: {7} / 상태: {8}\r\n마우스 Wafer 중심 기준: X={9:#,0.###}, Y={10:#,0.###} µm",
                currentDirection, hoverDie.IndexX, hoverDie.IndexY, lb.X, lb.Y,
                lb.X - currentWafer.ShiftX, lb.Y - currentWafer.ShiftY,
                inspectedDies.Contains(lb) ? "예" : "아니오", hoverDie.DieProp,
                point.X, point.Y);
        }

        /// <summary>Die 가 없는 영역에서는 Map 컨트롤이 위치 이벤트를 보내지 않으므로 좌표를 먼저 지운다.</summary>
        private void m_wMap_MouseMove(object sender, MouseEventArgs e) { ClearCoordinates(); }

        /// <summary>Map 밖으로 이동하면 마지막 마우스 좌표를 지운다.</summary>
        private void m_wMap_MouseLeave(object sender, EventArgs e) { ClearCoordinates(); }

        private void ClearCoordinates()
        {
            lblCoordinates.Text = "Die X/Y: - / LeftBottom: - / KLARF 원본: -\r\n마우스 Wafer 중심 기준: -";
        }

        #endregion

        #region 저장

        /// <summary>
        /// 화면의 Map 을 저장한다. 기존 ProbeAdmin 저장 경로를 그대로 쓴다.
        /// 1. 같은 Map ID 가 없으면 신규 저장 (MapCreationService.Create: SetMapDef -> CreateUseMap).
        /// 2. 같은 Map ID 가 있으면 덮어쓸지 묻고 (기본 "아니오"), 예 이면 MapCreationService.Update (UpdateMapDef -> CreateUseMap).
        ///    기존 Map Define 의 Save As 덮어쓰기와 같은 방식이다. (사용자 요청 2026-10-06 "재등록 시 Update")
        /// 3. 같은 ID 가 삭제 표시(DELETE_FLAG = Y)된 Map 이면 덮어쓰지 않는다. UPDATE_MAP 은 DELETE_FLAG 를 바꾸지 않아
        ///    덮어써도 목록에 보이지 않는 Map 이 되기 때문이다.
        /// 두 원격 호출은 하나의 트랜잭션이 아니므로 중간 실패는 성공으로 표시하지 않는다.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (source == null || currentGrid == null) return;
            string mapId = txtMapID.Text.Trim();
            if (mapId.Length == 0)
            {
                MessageBox.Show(this, "Map ID를 입력하세요.");
                txtMapID.Focus();
                return;
            }
            bool writeStarted = false;
            bool overwrite = false;
            btnSave.Enabled = false;
            UseWaitCursor = true;
            try
            {
                DACrux.TEST.RO.ProbeAdmin admin = new DACrux.TEST.RO.ProbeAdmin();
                using (DataTable existing = admin.GetMapDef(mapId))
                {
                    if (existing != null && existing.Rows.Count > 0)
                    {
                        if (existing.Columns.Contains("DELETE_FLAG") && existing.Rows[0]["DELETE_FLAG"].ToString() == "Y")
                        {
                            MessageBox.Show(this, "삭제 표시된 Map ID입니다. 덮어쓰면 목록에 보이지 않으므로 다른 ID를 입력하세요.", "Map 저장");
                            return;
                        }
                        overwrite = true;
                    }
                }

                string summary = string.Format(CultureInfo.InvariantCulture,
                    "Map ID: {0}\r\n사용 Die: {1}개 / 전체 Die: {2}개\r\nWafer: {3:#,0} µm / XY: {4}",
                    mapId, m_wMap.NetDie, m_wMap.Dies.Count, m_wMap.WaferSize, currentDirection);
                if (overwrite)
                {
                    if (MessageBox.Show(this, summary + "\r\n\r\n이미 있는 Map ID입니다. 기존 Map 정의와 Die 목록을 이 화면의 Map으로 덮어쓰시겠습니까?",
                            "Map 덮어쓰기", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                }
                else if (MessageBox.Show(this, summary + "\r\n\r\n새 Map으로 저장하시겠습니까?",
                        "신규 Map 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                writeStarted = true;
                if (overwrite) MapCreationService.Update(mapId, m_wMap);
                else MapCreationService.Create(mapId, m_wMap);
                savedMapId = mapId;
                btnOpenMap.Enabled = true;
                lblStatus.Text = (overwrite ? "덮어쓰기 완료: " : "저장 완료: ") + mapId + " / 기존 Map Define 화면에서 조회할 수 있습니다.";
                MessageBox.Show(this, overwrite ? "Map을 덮어썼습니다." : "Map을 저장했습니다.", "Map 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string detail = writeStarted
                    ? "저장이 완료되지 않았습니다. Map 정의만 " + (overwrite ? "바뀌었을" : "저장되었을") + " 수 있으므로 같은 ID로 다시 저장하거나 기존 Map Define 화면에서 확인하세요.\r\n\r\n"
                    : "저장할 수 없습니다.\r\n\r\n";
                lblStatus.Text = "저장 실패: " + mapId;
                MessageBox.Show(this, detail + ex.Message, "Map 저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                btnSave.Enabled = source != null;
            }
        }

        /// <summary>방금 저장한 Map 을 기존 Map Define 화면에서 연다. 기존 화면의 코드는 변경하지 않는다.</summary>
        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(savedMapId)) return;
            frmSetupMap form = new frmSetupMap();
            form.StartPosition = FormStartPosition.CenterParent;
            // Show 에서 기존 화면의 Load 초기화를 마친 다음 저장 Map 을 불러온다.
            form.Show(this);
            form.LoadMap(savedMapId);
        }

        /// <summary>m_wMap 의 Die 목록에서 DB 저장용 X / Y / USECODE 를 만든다. (검사 프로그램에서도 사용)</summary>
        private DataTable CreateMapDieTable()
        {
            return MapCreationService.CreateDieTable(m_wMap);
        }

        #endregion
    }
}
