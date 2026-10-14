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
    /// 왜: AOI(KLARF) / ET Map / GDS 를 같은 Die 격자와 같은 인덱스로 맞추기 위해서다. (결정 2026-10-06 "klarf 로 하자")
    /// 역할 / 흐름:
    /// 1. KlarfMapSource 로 파일을 읽는다. Die 인덱스는 DMS 가 KLARF 를 저장할 때와 같은 좌하단 (1,1) 기준이다.
    /// 2. [원형 생성] 에서 KlarfMapRule 로 Wafer 전체 격자를 구해 AddDie 로 넣고, Die 마다 원 안이면 1, 밖이면 -1 로 둔다.
    /// 3. 작업자는 화면의 편집 모드(보기 / 제거 / 복원)로 Map 을 클릭 / 드래그해 Die 를 지우거나 되살린다.
    ///    보조 버튼: [검사 Die 외 제거] (KLARF SampleTestPlan 기준) / [Wafer 밖 제거] (현재 Wafer / Edge / Notch 기준).
    ///    Map 의 Edit 메뉴는 숨겨 Mark / Skip / First Die 가 저장되지 않게 한다.
    /// 4. [새 Map 저장] 은 기존 ProbeAdmin 저장 경로(MapCreationService)를 그대로 쓴다. 단위는 µm, 제거 Die 는 USECODE = -1.
    ///    단 EDGE_SIZE 는 mm, ORIGIN_MICRO_Y 는 DieSizeY - OriginY 로 저장한다 (기존 데이터 / 조회 쿼리 규칙, MapCreationService.ToStoredValues).
    /// 5. Die 번호: 선택 XY 방향 시작 모서리부터 행 우선, 사용 Die 만 번호를 매겨 Map 위에 그린다 (DieNumberRule, 2026-10-13).
    ///    화면 표시용이며 저장 DIE_NUM 은 기존 저장 서비스(행 순서 i + 1)를 그대로 쓴다.
    /// 6. Die Pitch: 기본 = KLARF DiePitch (Die + Scribe). 작업자가 바꾸면 [원형 생성] 때 격자 / Die 크기에 쓰고 CHIP_SIZE_X / Y 로 저장한다 (2026-10-13).
    ///    KLARF Defect 위치(XREL / YREL)는 KLARF Pitch 칸 기준이라, 값이 다르면 정보창에 차이와 어긋나는 양을 표시한다.
    ///    Die 크기 / Scribe 폭은 입력받지 않는다. GDS 좌표 변환용 Scribe 오프셋은 연관 분석(F07) 설정에서 다룬다.
    /// 설계: 문서/2026-10-06_KLARF_Map_Setup_설계.md
    /// </summary>
    public partial class frmStandardMapSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private KlarfMapSource source;
        private KlarfMapWafer currentWafer;
        private KlarfMapGrid currentGrid;
        /// <summary>
        /// Map 번호 방향. 기본 LeftTop: BS top view 에서 고객 통합 Die# 1 번이 왼쪽 위에서 시작하므로 같은 모서리에 맞춘다 (2026-10-13).
        /// 작업자가 콤보에서 바꿀 수 있다. 형상은 그대로이고 번호만 바뀐다.
        /// </summary>
        private const XYDirection DefaultDirection = XYDirection.LeftTop;
        /// <summary>
        /// Die Pitch 입력 범위 = KLARF 값의 50% ~ 200% (2026-10-13).
        /// Pitch 조정은 측정 / Scribe 차이 보정용이다. 단위 착오(mm 입력 등)로 Die 수가 크게 늘어 화면이 멈추는 일을 막는다.
        /// </summary>
        private const double PitchMinRatio = 0.5d;
        private const double PitchMaxRatio = 2d;
        /// <summary>Pitch 입력칸 소수 자리(3) 기준으로 같다고 보는 차이 (µm).</summary>
        private const double PitchTolerance = 0.0005d;
        private XYDirection currentDirection = DefaultDirection;
        /// <summary>현재 Wafer 의 검사 Die (LeftBottom 인덱스).</summary>
        private readonly HashSet<Point> inspectedDies = new HashSet<Point>();
        /// <summary>동일 Die 에서는 변경 이벤트가 반복되지 않으므로 마지막 Die 를 유지한다.</summary>
        private Die hoverDie;
        private string savedMapId;
        /// <summary>원형 생성 후 작업자가 Die 를 바꿨는지. 다시 생성할 때 확인창을 띄운다.</summary>
        private bool dieEdited;
        /// <summary>코드에서 설정 컨트롤 값을 넣는 동안 변경 이벤트를 무시한다.</summary>
        private bool updatingSettings;
        /// <summary>마지막 정보 갱신 때 사용 / 제거 Die 수. 드래그 편집 후 바뀌었는지 비교한다.</summary>
        private int lastUsedCount;
        private int lastRemovedCount;

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
        /// KLARF 를 읽고 설정 초기값(Map ID / Wafer / 크기 / 방향 / 원점 / Die Pitch)을 채운 뒤 원형 Map 을 바로 만든다.
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
                cmbXYDirection.SelectedItem = DefaultDirection.ToString();
                // Flat Zone 위치 / 형태 초기값 = 파일 값 (OrientationMarkLocation / SampleOrientationMarkType) (2026-10-13)
                cmbFlatZone.SelectedItem = KlarfMapRule.AngleToFlatZone(loaded.NotchAngle);
                cmbMarkType.SelectedItem = loaded.IsFlat ? "Flat" : "Notch";
                numEdge.Value = 0;
                txtMapID.Text = loaded.DeviceID;
                ApplyWaferOrigin(loaded.Wafers[0]);
                SetPitchRange(numPitchX, loaded.PitchX);
                SetPitchRange(numPitchY, loaded.PitchY);
                ApplyKlarfPitch(loaded);
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

        /// <summary>Die Pitch 입력 범위를 KLARF 값 기준(PitchMinRatio ~ PitchMaxRatio)으로 정한다. 파일을 읽을 때만 부른다.</summary>
        private static void SetPitchRange(NumericUpDown box, double klarfPitch)
        {
            // NumericUpDown 은 Minimum 이 Maximum 보다 커지면 Maximum 을, 범위를 벗어난 Value 를 함께 맞춘다 (예외 없음).
            box.Minimum = (decimal)Math.Round(klarfPitch * PitchMinRatio, 3);
            box.Maximum = (decimal)Math.Round(klarfPitch * PitchMaxRatio, 3);
        }

        /// <summary>Die Pitch 입력칸을 KLARF DiePitch 로 채운다. 입력칸이 소수 3자리라 같은 자리로 맞춰 넣는다.</summary>
        private void ApplyKlarfPitch(KlarfMapSource klarf)
        {
            numPitchX.Value = (decimal)Math.Round(klarf.PitchX, 3);
            numPitchY.Value = (decimal)Math.Round(klarf.PitchY, 3);
        }

        /// <summary>
        /// 원형 생성에 쓸 Pitch (µm). 입력값이 KLARF 값과 같으면(소수 3자리 기준) 파일 값을 그대로 써서
        /// 반올림 때문에 Die 위치가 바뀌지 않게 한다.
        /// </summary>
        private static double InputPitch(NumericUpDown box, double klarfPitch)
        {
            double value = (double)box.Value;
            return PitchDiffers(value, klarfPitch) ? value : klarfPitch;
        }

        private static bool PitchDiffers(double pitch, double klarfPitch)
        {
            return Math.Abs(pitch - klarfPitch) >= PitchTolerance;
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
            btnRemoveNotInspected.Enabled = false;
            btnRemoveOutside.Enabled = false;
            btnPitchKlarf.Enabled = false;
            txtInfo.Clear();
            ClearDieCount();
            lblStatus.Text = "KLARF 파일을 선택하세요.";
            m_wMap.DieNumbers = null;
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

        /// <summary>Wafer 크기 / Edge / XY 방향 / Flat Zone / 원점 / Die Pitch 가 바뀌면 [원형 생성] 을 눌러 반영하도록 안내만 한다.</summary>
        private void MapSettingsChanged(object sender, EventArgs e)
        {
            if (updatingSettings || source == null) return;
            lblStatus.Text = "설정이 바뀌었습니다. [원형 생성] 을 눌러 반영하세요.";
        }

        /// <summary>
        /// [KLARF 값으로] Die Pitch 입력칸을 파일 DiePitch 로 되돌린다.
        /// 다른 설정과 같이 값만 바꾸고 Map 은 [원형 생성] 때 반영한다 (편집한 Die 를 바로 지우지 않기 위해서다).
        /// </summary>
        private void btnPitchKlarf_Click(object sender, EventArgs e)
        {
            if (source == null) return;
            updatingSettings = true;
            ApplyKlarfPitch(source);
            updatingSettings = false;
            bool applied = !PitchDiffers(m_wMap.DieSizeX, source.PitchX) && !PitchDiffers(m_wMap.DieSizeY, source.PitchY);
            lblStatus.Text = applied ? "Die Pitch 가 이미 KLARF 값입니다."
                : "Die Pitch 를 KLARF 값으로 되돌렸습니다. [원형 생성] 을 눌러 반영하세요.";
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

        /// <summary>
        /// Flat Zone 위치 / 형태 콤보를 Map 컨트롤에 넣는다 (2026-10-13).
        /// 기존 Map Define 의 Notch / Flat 선택 + 각도 입력, ITEK Standard Map Setup 의 Flat Zone(위치) 선택과 같은 역할이다.
        /// 각도 규칙은 Map 컨트롤 기준 (0 = 아래 / 90 = 왼쪽 / 180 = 위 / 270 = 오른쪽, KlarfMapRule.FlatZoneToAngle).
        /// 값이 비어 있으면 KLARF 파일 값을 쓴다. 저장 시 TQP_MAPDEF.ANGLE / NOTCH_TYPE 에 그대로 들어간다.
        /// </summary>
        private void ApplyFlatZone()
        {
            m_wMap.NotchAngle = cmbFlatZone.SelectedIndex >= 0 ? KlarfMapRule.FlatZoneToAngle(cmbFlatZone.Text) : source.NotchAngle;
            bool flat = cmbMarkType.SelectedIndex >= 0 ? cmbMarkType.Text == "Flat" : source.IsFlat;
            m_wMap.NotchType = flat ? Notch.Flat : Notch.Notch;
        }

        /// <summary>작업자가 Die 를 바꾼 뒤라면 다시 만들지 묻는다.</summary>
        private bool ConfirmRegenerate()
        {
            if (!dieEdited) return true;
            return AskYesNo("원형을 다시 만들면 Die 편집 내용이 초기화됩니다. 계속하시겠습니까?", "원형 생성");
        }

        /// <summary>
        /// 원형 Map 생성. 순서:
        /// 1. KlarfMapRule.CalculateGrid 로 선택한 Wafer 크기 / Die Pitch 의 전체 격자를 구한다 (LeftBottom, DMS 인덱스와 같은 기준).
        ///    Pitch 를 바꿔도 KLARF (0,0) Die 는 같은 인덱스(Parser 가 옮긴 양)에 둔다. 그래서 Defect 인덱스는 그대로 맞고 크기만 달라진다.
        /// 2. 화면 Map 은 항상 LeftBottom 번호로 그린다. 선택한 XY 방향은 좌표 표시와 저장(MapCreationService)에서만 번호로 바꾼다.
        ///    기존 WaferMap 은 LeftBottom 이 아닌 방향에서 마우스 위치 Die 판정과 Die 그리기가 한 칸 어긋나기 때문이다 (2026-10-13).
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
            double pitchX = InputPitch(numPitchX, source.PitchX);
            double pitchY = InputPitch(numPitchY, source.PitchY);

            currentGrid = KlarfMapRule.CalculateGrid(waferSize, pitchX, pitchY,
                originX, originY, currentWafer.ShiftX, currentWafer.ShiftY);
            Point originIndex = KlarfMapRule.MapOriginIndex(XYDirection.LeftBottom, currentGrid);

            inspectedDies.Clear();
            foreach (Point point in currentWafer.InspectedDies) inspectedDies.Add(point);

            m_wMap.DieClear();
            m_wMap.ViewAngle = 0;
            m_wMap.AngleOffSet = 0;
            m_wMap.WaferSize = waferSize;
            // Edge 는 Wafer 크기를 정한 뒤 넣는다 (KLARF File View 와 같은 순서).
            m_wMap.EdgeSize = (double)numEdge.Value;
            ApplyFlatZone();
            m_wMap.XYDirect = XYDirection.LeftBottom;
            m_wMap.DieSizeX = pitchX;
            m_wMap.DieSizeY = pitchY;
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
                    m_wMap.AddDie(new Die(x, y, 0, 0, 0, 0, 0, 1));
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
            btnRemoveNotInspected.Enabled = true;
            btnRemoveOutside.Enabled = true;
            btnPitchKlarf.Enabled = true;
            // 다시 만든 뒤에도 화면에서 고른 편집 모드를 유지한다.
            ApplyEditMode();
            m_wMap.Redraw();
            UpdateInfo();
        }

        /// <summary>클릭으로 Die 속성이 바뀌면 편집 표시를 하고 정보를 갱신한다. (Map 컨트롤은 클릭 때만 이 이벤트를 보낸다)</summary>
        private void m_wMap_OnChangeDieProperty(object sender, Die die)
        {
            if (source == null) return;
            dieEdited = true;
            UpdateInfo();
        }

        /// <summary>
        /// 드래그로 여러 Die 를 바꾸면 Map 컨트롤이 변경 이벤트를 보내지 않는다 (EditWaferMap.OnMouseMove).
        /// 그래서 마우스를 뗄 때 개수를 다시 세어, 바뀌었으면 편집 표시를 하고 정보 / 개수 줄을 갱신한다.
        /// </summary>
        private void m_wMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (source == null || currentGrid == null || !m_wMap.IsDieEditing) return;
            int used, removed;
            CountDies(out used, out removed);
            if (used != lastUsedCount || removed != lastRemovedCount) dieEdited = true;
            UpdateInfo();
        }

        #endregion

        #region Die 편집 (보기 / 제거 / 복원 + 보조 버튼)

        /// <summary>편집 모드 라디오가 바뀌면 Map 컨트롤 동작에 반영한다. 해제되는 라디오의 이벤트는 무시한다.</summary>
        private void EditModeChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio != null && !radio.Checked) return;
            ApplyEditMode();
            if (source != null)
                lblStatus.Text = rdoRemove.Checked ? "제거 모드: Map 에서 Die 를 클릭하거나 드래그하면 제거(-1)됩니다."
                    : rdoRestore.Checked ? "복원 모드: Map 에서 Die 를 클릭하거나 드래그하면 사용(1)으로 되살아납니다."
                    : "보기 모드: 클릭해도 Die 가 바뀌지 않습니다. 드래그하면 확대합니다.";
        }

        /// <summary>
        /// 라디오 상태를 Map 컨트롤에 넣는다. 보기 = 기존 기본 동작(Zoom), 제거 = DELETE(-1), 복원 = ADD(1).
        /// Map 의 Edit 메뉴는 Designer 에서 숨겼다 (EditMenuVisible = false). Mark / Skip / First Die 가 저장되는 일을 막기 위해서다.
        /// </summary>
        private void ApplyEditMode()
        {
            if (rdoRemove.Checked) m_wMap.SetDieEditMode(true, DACrux.Map.EditMode.DELETE);
            else if (rdoRestore.Checked) m_wMap.SetDieEditMode(true, DACrux.Map.EditMode.ADD);
            else m_wMap.SetDieEditMode(false, DACrux.Map.EditMode.ADD);
        }

        /// <summary>
        /// [검사 Die 외 제거] KLARF SampleTestPlan(검사 Die)에 없는 사용 Die 를 한 번에 제거(-1)한다.
        /// 확인창에 대상 개수를 먼저 보여 준다. 이후 제거 / 복원 모드로 다시 고칠 수 있다.
        /// </summary>
        private void btnRemoveNotInspected_Click(object sender, EventArgs e)
        {
            if (source == null || currentGrid == null) return;
            RemoveDies(die => !inspectedDies.Contains(ToLeftBottom(die)), "검사 Die 외 제거",
                "KLARF 검사 Die({0}개)에 없는 사용 Die {1}개를 제거합니다.");
        }

        /// <summary>
        /// [Wafer 밖 제거] 현재 Wafer / Edge / Notch 기준으로 원 밖에 걸친 사용 Die 를 한 번에 제거(-1)한다.
        /// 원형 생성 때 자동으로 제거되지만, 복원 모드로 원 밖 Die 를 살렸거나 Edge 를 다시 판정할 때 쓴다.
        /// Edge 값을 바꿨다면 [원형 생성] 없이도 이 버튼으로 새 Edge 를 적용할 수 있다.
        /// </summary>
        private void btnRemoveOutside_Click(object sender, EventArgs e)
        {
            if (source == null || currentGrid == null) return;
            // 입력칸의 Edge / Flat Zone 위치 / 형태를 바로 쓰고, Notch / Flat 영역은 그리기 중에 계산되므로 먼저 다시 그린다.
            m_wMap.EdgeSize = (double)numEdge.Value;
            ApplyFlatZone();
            m_wMap.Redraw();
            RemoveDies(die => !m_wMap.IsInsideWafer(die), "Wafer 밖 제거",
                "Wafer(Edge {2:#,0} µm 포함) 밖에 걸친 사용 Die {1}개를 제거합니다.");
        }

        /// <summary>
        /// 보조 버튼 공통: 조건에 맞는 사용 Die(DieProp = 1)를 세어 확인창을 띄우고, 예 이면 -1 로 바꾼다.
        /// 메시지 형식: {0} 검사 Die 수 / {1} 대상 수 / {2} Edge(µm).
        /// </summary>
        private void RemoveDies(Func<Die, bool> target, string title, string messageFormat)
        {
            List<Die> dies = new List<Die>();
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Die die = m_wMap.Dies[i];
                if (die.DieProp == 1 && target(die)) dies.Add(die);
            }
            if (dies.Count == 0)
            {
                ShowInformation("제거할 Die 가 없습니다.", title);
                return;
            }
            string message = string.Format(CultureInfo.InvariantCulture, messageFormat, inspectedDies.Count, dies.Count, (double)numEdge.Value);
            if (!AskYesNo(message + "\r\n\r\n계속하시겠습니까?", title))
                return;
            foreach (Die die in dies) die.DieProp = -1;
            dieEdited = true;
            m_wMap.Redraw();
            UpdateInfo();
            lblStatus.Text = title + ": " + dies.Count + "개 제거했습니다.";
        }

        /// <summary>
        /// 예 / 아니오 확인창. 원형 다시 생성 / 보조 버튼 확인에 쓴다.
        /// protected virtual 인 이유: 화면 밖 검사(Verification/KlarfMapFormSmoke)가 하위 클래스에서 "예" 로 바꿔 끝까지 실행하기 위해서다.
        /// </summary>
        protected virtual bool AskYesNo(string text, string title)
        {
            return MessageBox.Show(this, text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>정보 안내창. AskYesNo 와 같은 이유로 protected virtual 이다.</summary>
        protected virtual void ShowInformation(string text, string title)
        {
            MessageBox.Show(this, text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>사용(1) / 제거(-1) Die 수를 센다.</summary>
        private void CountDies(out int used, out int removed)
        {
            used = 0;
            removed = 0;
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                if (m_wMap.Dies[i].DieProp == 1) used++;
                else if (m_wMap.Dies[i].DieProp == -1) removed++;
            }
        }

        #endregion

        #region 정보 / 좌표 표시

        /// <summary>
        /// 저장될 값과 판단 근거를 보여준다. Map 값은 µm 이고 mm 를 함께 적는다.
        /// LeftBottom 이 아니면 DMS Defect 인덱스와 번호가 다르다는 것을 알린다.
        /// Map 위 개수 줄(lblDieCount)도 함께 갱신한다 (2026-10-07 요청: Net Die / 생성 Die 개수 표시).
        /// 원형 생성 후와 Edit 메뉴로 Die 속성이 바뀔 때마다 불린다.
        /// </summary>
        private void UpdateInfo()
        {
            if (source == null || currentGrid == null) { txtInfo.Clear(); ClearDieCount(); m_wMap.DieNumbers = null; return; }
            RefreshDieNumbers();

            int used = 0, removed = 0, other = 0, inspectedRemoved = 0;
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Die die = m_wMap.Dies[i];
                if (die.DieProp == 1) used++;
                else if (die.DieProp == -1) removed++;
                else other++;
                if (die.DieProp == -1 && inspectedDies.Contains(ToLeftBottom(die))) inspectedRemoved++;
            }
            lastUsedCount = used;
            lastRemovedCount = removed;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DEVICE: " + source.DeviceID);
            sb.AppendLine("LOT: " + source.LotID);
            sb.AppendLine(string.Format("WAFER: {0} (Slot {1})", currentWafer.WaferID, currentWafer.Slot));
            sb.AppendLine("SETUP / STEP: " + source.SetupID + " / " + source.StepID);
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Wafer: {0:#,0} µm (파일 {1} mm)", m_wMap.WaferSize, source.SampleSize));
            AppendPitchInfo(sb);
            sb.AppendLine(string.Format("Notch(파일): {0} / {1} ({2}°)", source.MarkType, source.MarkLocation, source.NotchAngle));
            sb.AppendLine(string.Format("Flat Zone(적용): {0} / {1} ({2}°)", m_wMap.NotchType, KlarfMapRule.AngleToFlatZone(m_wMap.NotchAngle), m_wMap.NotchAngle));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "원점(Center): {0:#,0.###}, {1:#,0.###} µm", m_wMap.OriginX, m_wMap.OriginY));
            MapDefStoredValues stored = MapCreationService.ToStoredValues(m_wMap, currentDirection, currentGrid);
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "저장값: EDGE {0:0.###} mm / ORIGIN_MICRO {1:#,0.###}, {2:#,0.###}",
                stored.EdgeSizeMm, stored.OriginMicroX, stored.OriginMicroY));
            sb.AppendLine(string.Format("KLARF (0,0) -> LeftBottom ({0}, {1}) / 출처: {2}", currentWafer.ShiftX, currentWafer.ShiftY,
                source.OriginFromDB ? "TQD_SETUP" : "계산기"));
            sb.AppendLine(string.Format("격자 (LeftBottom): X {0}~{1} / Y {2}~{3}", currentGrid.MinX, currentGrid.MaxX, currentGrid.MinY, currentGrid.MaxY));
            if (currentGrid.MinX < 1 || currentGrid.MinY < 1)
                sb.AppendLine("※ 0 이하 인덱스가 있습니다. KLARF (0,0) Die 인덱스를 유지하면서 격자가 왼쪽 / 아래로 늘어났습니다 (Pitch / 원점 확인).");
            sb.AppendLine(string.Format("XY 방향(저장): {0} / 원점 인덱스 {1}, {2}", currentDirection, stored.OriginIndexX, stored.OriginIndexY));
            if (currentDirection != XYDirection.LeftBottom)
                sb.AppendLine("※ DMS Defect 인덱스는 LeftBottom 기준이라 이 Map 번호와 다릅니다 (형상은 같음).");
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

        /// <summary>
        /// Die Pitch 줄: KLARF 값 / 적용 값(= 저장 CHIP_SIZE_X / Y). 다르면 차이와 Defect 위치가 어긋나는 양을 적는다 (2026-10-13).
        /// KLARF Defect 는 원본 인덱스 i 의 칸 안 위치(XREL)라 Map 위치는 i x 차이만큼 밀린다. 가장 먼 Die 끝(|i| 또는 |i + 1|)이 최대값이다.
        /// </summary>
        private void AppendPitchInfo(StringBuilder sb)
        {
            double pitchX = m_wMap.DieSizeX;
            double pitchY = m_wMap.DieSizeY;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Die Pitch(KLARF): {0:#,0.###} x {1:#,0.###} µm", source.PitchX, source.PitchY));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Die Pitch(적용 / 저장): {0:#,0.###} x {1:#,0.###} µm", pitchX, pitchY));
            if (!PitchDiffers(pitchX, source.PitchX) && !PitchDiffers(pitchY, source.PitchY)) return;

            double diffX = pitchX - source.PitchX;
            double diffY = pitchY - source.PitchY;
            int farX = Math.Max(Math.Abs(currentGrid.MinX - currentWafer.ShiftX), Math.Abs(currentGrid.MaxX - currentWafer.ShiftX + 1));
            int farY = Math.Max(Math.Abs(currentGrid.MinY - currentWafer.ShiftY), Math.Abs(currentGrid.MaxY - currentWafer.ShiftY + 1));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "※ KLARF Pitch 와 다름 (차이 X {0:+#,0.###;-#,0.###;0} / Y {1:+#,0.###;-#,0.###;0} µm). Defect 위치와 어긋날 수 있습니다 (격자 끝 최대 X {2:#,0.###} / Y {3:#,0.###} µm).",
                diffX, diffY, Math.Abs(diffX) * farX, Math.Abs(diffY) * farY));
        }

        /// <summary>
        /// 현재 사용 Die 와 생성 때의 XY 방향으로 Die 번호(DieNumberRule)를 다시 매긴다. 원형 생성 / 제거 / 복원 뒤 UpdateInfo 에서 불린다.
        /// 화면 Map 은 LeftBottom 번호라서 선택 방향 번호로 바꿔 순서를 정하고, Map 컨트롤에는 LeftBottom 번호를 키로 넘긴다.
        /// 번호가 바뀌었고 표시 중이면 다시 그린다 (그리기 전에 번호가 정해져야 Die 위에 나온다).
        /// </summary>
        private void RefreshDieNumbers()
        {
            List<KeyValuePair<Point, int>> dies = new List<KeyValuePair<Point, int>>(m_wMap.Dies.Count);
            Dictionary<Point, Point> toLeftBottom = new Dictionary<Point, Point>();
            for (int i = 0; i < m_wMap.Dies.Count; i++)
            {
                Point lb = ToLeftBottom(m_wMap.Dies[i]);
                Point number = KlarfMapRule.ToDirection(lb, currentDirection, currentGrid);
                dies.Add(new KeyValuePair<Point, int>(number, m_wMap.Dies[i].DieProp));
                toLeftBottom[number] = lb;
            }
            Dictionary<Point, int> numbers = new Dictionary<Point, int>();
            foreach (KeyValuePair<Point, int> pair in DieNumberRule.Number(dies)) numbers[toLeftBottom[pair.Key]] = pair.Value;

            bool changed = numbers.Count != m_wMap.DieNumbers.Count;
            if (!changed)
            {
                foreach (KeyValuePair<Point, int> pair in numbers)
                {
                    int before;
                    if (!m_wMap.DieNumbers.TryGetValue(pair.Key, out before) || before != pair.Value) { changed = true; break; }
                }
            }
            m_wMap.DieNumbers = numbers;
            m_wMap.ShowDieNumbers = chkDieNumber.Checked;
            if (changed && chkDieNumber.Checked) m_wMap.Redraw();
        }

        /// <summary>[Die 번호] 체크: Map 위 번호만 켜고 끈다 (번호는 UpdateInfo 에서 이미 매겼다).</summary>
        private void chkDieNumber_CheckedChanged(object sender, EventArgs e)
        {
            m_wMap.ShowDieNumbers = chkDieNumber.Checked;
            if (source != null) m_wMap.Redraw();
        }

        /// <summary>파일이 없거나 Map 을 비웠을 때 개수 줄을 초기 문구로 되돌린다.</summary>
        private void ClearDieCount()
        {
            lblDieCount.Text = "Net Die: - / 생성 Die: -";
        }

        /// <summary>화면 Map 의 Die 번호는 LeftBottom 인덱스 그대로다 (화면은 항상 LeftBottom 으로 그린다).</summary>
        private static Point ToLeftBottom(Die die)
        {
            return new Point(die.IndexX, die.IndexY);
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
            Point number = KlarfMapRule.ToDirection(lb, currentDirection, currentGrid);
            int dieNumber;
            string dieNumberText = m_wMap.DieNumbers.TryGetValue(lb, out dieNumber) ? dieNumber.ToString(CultureInfo.InvariantCulture) : "-";
            lblCoordinates.Text = string.Format(CultureInfo.InvariantCulture,
                "Die ({0}) X/Y: {1}, {2} / LeftBottom: {3}, {4} / KLARF 원본: {5}, {6} / 검사 Die: {7} / 상태: {8} / Die 번호: {9}\r\n마우스 Wafer 중심 기준: X={10:#,0.###}, Y={11:#,0.###} µm",
                currentDirection, number.X, number.Y, lb.X, lb.Y,
                lb.X - currentWafer.ShiftX, lb.Y - currentWafer.ShiftY,
                inspectedDies.Contains(lb) ? "예" : "아니오", hoverDie.DieProp, dieNumberText,
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
        ///    기존 Map Define 의 Save As 덮어쓰기와 같은 방식이다. (요청 2026-10-06 "재등록 시 Update")
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

                bool pitchChanged = PitchDiffers(m_wMap.DieSizeX, source.PitchX) || PitchDiffers(m_wMap.DieSizeY, source.PitchY);
                string summary = string.Format(CultureInfo.InvariantCulture,
                    "Map ID: {0}\r\n사용 Die: {1}개 / 전체 Die: {2}개\r\nWafer: {3:#,0} µm / XY: {4}\r\nDie Pitch: {5:#,0.###} x {6:#,0.###} µm{7}",
                    mapId, m_wMap.NetDie, m_wMap.Dies.Count, m_wMap.WaferSize, currentDirection,
                    m_wMap.DieSizeX, m_wMap.DieSizeY, pitchChanged ? " (KLARF 값과 다름)" : "");
                if (overwrite)
                {
                    if (MessageBox.Show(this, summary + "\r\n\r\n이미 있는 Map ID입니다. 기존 Map 정의와 Die 목록을 이 화면의 Map으로 덮어쓰시겠습니까?",
                            "Map 덮어쓰기", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                }
                else if (MessageBox.Show(this, summary + "\r\n\r\n새 Map으로 저장하시겠습니까?",
                        "신규 Map 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                writeStarted = true;
                if (overwrite) MapCreationService.Update(mapId, m_wMap, currentDirection, currentGrid);
                else MapCreationService.Create(mapId, m_wMap, currentDirection, currentGrid);
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
            return MapCreationService.CreateDieTable(m_wMap, currentDirection, currentGrid);
        }

        #endregion
    }
}
