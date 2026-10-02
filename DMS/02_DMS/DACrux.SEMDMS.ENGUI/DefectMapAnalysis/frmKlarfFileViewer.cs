using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Data.Parser;
using DACrux.Data.Parser.Klarf;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// KLARF 파일 하나를 DB 없이 바로 읽어 Wafer / 검사 Die / Defect 를 보여주는 조회 화면이다.
    /// 왜: AOI KLARF 좌표를 GDS Chain 과 맞추기 전에, 파일의 Die / Defect 좌표가 어떻게 놓이는지 눈으로 확인하기 위해 만들었다.
    /// 역할:
    /// 1. 파싱은 기존 DMS 와 같은 ParserKlarf / ParserKlarf_18 (FileAnalyzer 로 버전 판별) 을 사용한다.
    /// 2. 그리기는 기존 DACrux.Map.DefectMap 을 상속한 KlarfDefectMap 을 사용한다 (확대/이동/메뉴 기존 그대로).
    /// 3. Map 설정 순서는 기존 Defect Map 화면 (frmDefectPatternSearchNew.DrawWafer) 과 같다.
    ///    WaferSize -> DieSize -> OriginIndex / Origin -> DieCalculation(true) -> DieClear -> AddDie -> AddDefect
    /// 4. 마우스 위치의 Wafer 좌표 / KLARF 좌표 / Die Index / Die 안 상대 위치 / 가까운 Defect 를 아래에 표시한다.
    /// DB 저장과 Service 호출은 하지 않는다.
    /// </summary>
    public partial class frmKlarfFileViewer : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>DefectMap 기본 색 배열 크기. 코드가 이 범위를 넘으면 기존 Map 이 그릴 때 오류가 나므로 미리 막는다.</summary>
        private const int MaxColorCode = 255;

        /// <summary>마우스에서 이 거리(px) 안에 있는 Defect 를 "가까운 Defect" 로 표시한다.</summary>
        private const float NearDefectPixel = 8f;

        /// <summary>Wafer 외곽선이 보이도록 그리는 바깥 띠 두께(um). 측정/판정에는 쓰지 않는 표시용 값이다.</summary>
        private const double WaferEdgeMicron = 1200d;

        /// <summary>범례 / Defect 색상. 코드 오름차순으로 순서대로 배정한다.</summary>
        private static readonly Color[] Palette =
        {
            Color.FromArgb(235, 87, 87), Color.FromArgb(242, 153, 74), Color.FromArgb(47, 98, 235), Color.FromArgb(39, 174, 128),
            Color.FromArgb(155, 81, 224), Color.FromArgb(140, 86, 75), Color.FromArgb(227, 119, 194), Color.FromArgb(127, 127, 127),
            Color.FromArgb(188, 189, 34), Color.FromArgb(23, 190, 207), Color.FromArgb(0, 0, 0), Color.FromArgb(255, 215, 0)
        };

        private ParserKlarf parser;
        private Wafer currentWafer;
        /// <summary>현재 Wafer 의 파일 원본 WaferID. 기존 Parser 의 WaferID 는 DMS 규칙(Lot-Slot)으로 바뀐 값이다.</summary>
        private string currentRawWaferId;
        /// <summary>검사(Sample) Die Index 목록. 화면 Index 기준이며 마우스 위치가 검사 Die 인지 판정할 때 사용한다.</summary>
        private HashSet<Point> sampleDies = new HashSet<Point>();
        /// <summary>코드에서 Wafer 콤보를 채울 때 SelectedIndexChanged 재진입을 막는다.</summary>
        private bool updatingWafer;

        /// <summary>화면 구성과 이벤트 연결은 InitializeComponent 에서 한 번에 초기화한다.</summary>
        public frmKlarfFileViewer()
        {
            InitializeComponent();
            cmbColorBy.SelectedIndex = 1;
            ClearView();
        }

        #region 파일 읽기

        /// <summary>파일을 선택하면 바로 읽는다. 선택 취소 시 현재 화면을 유지한다.</summary>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "KLARF 파일 선택";
                dialog.Filter = "KLARF 파일|*.000;*.001;*.klarf;*.klaf;*.kla;*.krf;*.sinf;*.txt|모든 파일|*.*";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtFile.Text = dialog.FileName;
                LoadFile();
            }
        }

        /// <summary>입력한 경로의 파일을 다시 읽는다.</summary>
        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        /// <summary>
        /// 기존 HandlerInsp.CreateParser 와 같이 FileAnalyzer 로 KLARF 1.8 여부를 판별해 Parser 를 고른다.
        /// Parser 는 예외를 밖으로 던지지 않고 ErrorFlag / ErrorMessage 로 실패를 알려주므로 둘 다 확인한다.
        /// WaferID 가 없거나 Slot 이 0 인 Wafer 는 기존 Parser 가 건너뛰므로 Wafer 0건도 실패로 본다.
        /// </summary>
        private void LoadFile()
        {
            ClearView();
            string path = txtFile.Text.Trim();
            if (path.Length == 0 || !File.Exists(path))
            {
                lblStatus.Text = "KLARF 파일을 선택하세요.";
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                FileAnalyzer analyzer = new FileAnalyzer(path);
                ParserKlarf loaded = analyzer.FileType == FileType.KlarfFile_1_8 ? new ParserKlarf_18(path) : new ParserKlarf(path);
                if (loaded.ErrorFlag) throw new FormatException(loaded.ErrorMessage);
                if (loaded.Wafers == null || loaded.Wafers.Count == 0)
                    throw new FormatException("Wafer 정보가 없습니다. WaferID / Slot / DieOrigin 항목을 확인하세요.");
                if (loaded.DiePitchX <= 0 || loaded.DiePitchY <= 0)
                    throw new FormatException("DiePitch 값이 없습니다.");
                if (loaded.SampleSize <= 0)
                    throw new FormatException("SampleSize (Wafer 크기) 값이 없습니다.");

                parser = loaded;
                Dictionary<int, string> rawIds = ReadRawWaferIds(path);
                updatingWafer = true;
                foreach (Wafer wafer in parser.Wafers)
                {
                    string rawId;
                    cmbWafer.Items.Add(new WaferItem(wafer, rawIds.TryGetValue(wafer.Slot, out rawId) ? rawId : wafer.WaferID));
                }
                updatingWafer = false;
                cmbWafer.Enabled = cmbWafer.Items.Count > 1;
                cmbWafer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ClearView();
                lblStatus.Text = "파일 읽기 실패 / 파일 내용을 확인하세요.";
                MessageBox.Show(this, ex.Message, "KLARF 파일 확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                updatingWafer = false;
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 기존 Parser 는 WaferID 를 DMS 규칙(Lot-Slot)으로 바꾸므로, 화면에 파일 원본 값도 보여주기 위해 따로 읽는다.
        /// KLARF 1.2 의 "WaferID "..."; Slot n;" 순서를 찾아 Slot -> 원본 WaferID 로 만든다. 못 찾으면 빈 사전을 돌려준다.
        /// </summary>
        private static Dictionary<int, string> ReadRawWaferIds(string path)
        {
            var result = new Dictionary<int, string>();
            string text = File.ReadAllText(path);
            foreach (Match match in Regex.Matches(text, "WaferID\\s+\"?([^\";]*)\"?\\s*;\\s*Slot\\s+(\\d+)"))
            {
                int slot = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                if (!result.ContainsKey(slot)) result.Add(slot, match.Groups[1].Value.Trim());
            }
            return result;
        }

        /// <summary>한 파일에 Wafer 가 여러 장이면 선택한 Wafer 를 다시 그린다.</summary>
        private void cmbWafer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updatingWafer) return;
            WaferItem item = cmbWafer.SelectedItem as WaferItem;
            if (item != null) ShowWafer(item);
        }

        /// <summary>다른 파일을 읽기 전/실패 시 이전 Wafer 가 남지 않도록 화면을 비운다.</summary>
        private void ClearView()
        {
            parser = null;
            currentWafer = null;
            currentRawWaferId = null;
            sampleDies.Clear();
            updatingWafer = true;
            cmbWafer.Items.Clear();
            updatingWafer = false;
            cmbWafer.Enabled = false;
            txtInfo.Clear();
            dgvLegend.Rows.Clear();
            dgvDefect.Rows.Clear();
            ClearMouseInfo();
            m_dMap.ShowDieIndexLabel = chkDieLabel.Checked;
            m_dMap.DefectClear();
            m_dMap.DieClear();
            m_dMap.SetInfomation(new string[0]);
            m_dMap.Redraw();
        }

        #endregion

        #region Wafer 그리기

        /// <summary>
        /// 선택한 Wafer 를 기존 Defect Map 설정 순서대로 그린다.
        /// 좌표 관계 (ParserKlarf 기준, 단위 um):
        ///   Defect.X = (원본 XINDEX * DiePitchX + XREL) - SampleCenterLocationX  -> Wafer 중심 기준 좌표
        ///   화면 Index = 원본 Index + Wafer.DieOrigin (Parser 가 좌하단 기준 양수 Index 로 옮긴 값)
        /// 그래서 Map 의 OriginIndex = Wafer.DieOrigin, Origin = SampleCenterLocation 으로 두면 Die 와 Defect 가 겹친다.
        /// </summary>
        private void ShowWafer(WaferItem item)
        {
            Wafer wafer = item.Wafer;
            currentWafer = wafer;
            currentRawWaferId = item.RawWaferId;
            int offsetX = (int)wafer.DieOriginX;
            int offsetY = (int)wafer.DieOriginY;

            m_dMap.DefectClear();
            m_dMap.Rotate(0);
            m_dMap.WaferSize = parser.SampleSize * 1000d;
            // 외곽 띠(Edge)는 Wafer 크기를 정한 뒤에 넣어야 한다. 먼저 넣으면 기존 Map 의 원호 크기가 음수가 되어 그리기 오류가 난다.
            m_dMap.EdgeSize = Math.Min(WaferEdgeMicron, m_dMap.WaferSize / 100d);
            m_dMap.NotchType = Notch.Notch;
            m_dMap.NotchAngle = parser.GetAngle();
            m_dMap.AngleOffSet = 0;
            m_dMap.XYDirect = XYDirection.LeftBottom;
            m_dMap.DieSizeX = parser.DiePitchX;
            m_dMap.DieSizeY = parser.DiePitchY;
            m_dMap.OriginIndexX = offsetX;
            m_dMap.OriginIndexY = offsetY;
            m_dMap.OriginX = wafer.SampleCenterLocationX;
            m_dMap.OriginY = wafer.SampleCenterLocationY;
            m_dMap.IndexOffset = new Point(offsetX, offsetY);
            m_dMap.WaferID = currentRawWaferId;
            m_dMap.DieCalculation(true);
            m_dMap.DrawDefects = "ALL";
            m_dMap.DieClear();

            // 검사 Die = 모든 InspectionTest 의 SampleTestPlan 합집합. 중복 Index 는 한 번만 추가한다.
            sampleDies.Clear();
            foreach (InspectionTest test in wafer.TestList)
            {
                if (test.SampleTestPlan == null) continue;
                foreach (Point point in test.SampleTestPlan)
                {
                    if (!sampleDies.Add(point)) continue;
                    m_dMap.AddDie(new Die(point.X, point.Y, test.TestNo, 1));
                }
            }

            foreach (Defect defect in wafer.DefectList)
                m_dMap.AddDefect(defect);

            m_dMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            ApplyColorBy();
            FillInfo();
            FillDefectGrid();
            ClearMouseInfo();
            lblStatus.Text = string.Format("읽기 완료 / Wafer {0} / 검사 Die {1}개 / Defect {2}개", currentRawWaferId, sampleDies.Count, wafer.DefectList.Count);
        }

        /// <summary>색상 기준(Class / FineBin / RoughBin / Cluster)을 바꾸면 범례와 Map 색을 다시 만든다.</summary>
        private void cmbColorBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentWafer != null) ApplyColorBy();
        }

        /// <summary>Die 라벨 표시 여부를 바꾸고 다시 그린다.</summary>
        private void chkDieLabel_CheckedChanged(object sender, EventArgs e)
        {
            m_dMap.ShowDieIndexLabel = chkDieLabel.Checked;
            m_dMap.Redraw();
        }

        /// <summary>현재 색상 기준의 MAP_TYPE. 콤보 순서와 같다.</summary>
        private MAP_TYPE SelectedMapType
        {
            get
            {
                switch (cmbColorBy.SelectedIndex)
                {
                    case 0: return MAP_TYPE.CLASS;
                    case 2: return MAP_TYPE.ROUGHBIN;
                    case 3: return MAP_TYPE.CLUSTER;
                    default: return MAP_TYPE.FINEBIN;
                }
            }
        }

        /// <summary>Defect 에서 현재 색상 기준의 코드 값을 꺼낸다.</summary>
        private int GetCode(Defect defect)
        {
            switch (SelectedMapType)
            {
                case MAP_TYPE.CLASS: return defect.CLASSNUMBER;
                case MAP_TYPE.ROUGHBIN: return defect.ROUGHBINNUMBER;
                case MAP_TYPE.CLUSTER: return defect.CLUSTERNUMBER;
                default: return defect.FINEBINNUMBER;
            }
        }

        /// <summary>코드 이름. Class 는 파일의 ClassLookup 이름을 쓰고 나머지는 "기준명 코드" 로 표시한다.</summary>
        private string GetCodeName(int code)
        {
            string name;
            if (SelectedMapType == MAP_TYPE.CLASS && currentWafer.ClassLookup != null && currentWafer.ClassLookup.TryGetValue(code, out name))
                return name;
            return cmbColorBy.Text + " " + code.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 코드별 개수를 세어 범례를 만들고, 기존 DefectMap.TypeColor (CLASSNUMBER / COLOR 표) 로 색을 넘긴다.
        /// TypeColor 는 표에 없는 코드를 검정으로 두므로, 파일에 있는 코드만 Palette 색을 배정한다.
        /// </summary>
        private void ApplyColorBy()
        {
            var counts = new SortedDictionary<int, int>();
            foreach (Defect defect in currentWafer.DefectList)
            {
                int code = GetCode(defect);
                int count;
                counts.TryGetValue(code, out count);
                counts[code] = count + 1;
            }

            if (counts.Keys.Any(code => code < 0 || code > MaxColorCode))
            {
                MessageBox.Show(this, string.Format("{0} 값이 0~{1} 범위를 벗어나 Class 기준으로 표시합니다.", cmbColorBy.Text, MaxColorCode),
                    "색상 기준", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbColorBy.SelectedIndex = 0; // 다시 ApplyColorBy 가 호출된다.
                return;
            }

            var table = new DataTable();
            table.Columns.Add("CLASSNUMBER", typeof(int));
            table.Columns.Add("COLOR", typeof(string));
            dgvLegend.Rows.Clear();
            int index = 0;
            foreach (KeyValuePair<int, int> pair in counts)
            {
                Color color = Palette[index++ % Palette.Length];
                table.Rows.Add(pair.Key, ColorTranslator.ToHtml(color));
                int row = dgvLegend.Rows.Add("", pair.Key, GetCodeName(pair.Key), pair.Value);
                dgvLegend.Rows[row].Cells[colLegendColor.Index].Style.BackColor = color;
                dgvLegend.Rows[row].Cells[colLegendColor.Index].Style.SelectionBackColor = color;
            }

            m_dMap.MapType = SelectedMapType;
            m_dMap.TypeColor = table;
            m_dMap.SetInfomation(new[] { string.Format("{0} / {1} / Defect {2}", parser.LotID, currentRawWaferId, currentWafer.DefectList.Count) });
            m_dMap.Redraw();
        }

        /// <summary>오른쪽 정보창에 파일 Header 와 Defect 가 많은 Die 순위를 표시한다. Die Index 는 KLARF 원본 기준이다.</summary>
        private void FillInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("파일: " + Path.GetFileName(txtFile.Text.Trim()));
            sb.AppendLine("Lot ID: " + parser.LotID);
            sb.AppendLine(string.Format("Wafer ID: {0} (Slot {1})", currentRawWaferId, currentWafer.Slot));
            sb.AppendLine("DMS Wafer ID: " + currentWafer.WaferID);
            sb.AppendLine("Device ID: " + parser.DeviceID);
            sb.AppendLine("Step ID: " + parser.StepID);
            sb.AppendLine("Setup ID: " + parser.SetupID);
            sb.AppendLine(string.Format("설비: {0} / {1} / {2}", parser.Maker, parser.Model, parser.Equip));
            sb.AppendLine("Result Time: " + parser.ResultTimestamp.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(string.Format("Wafer 크기: {0} mm / Notch: {1}", parser.SampleSize, parser.OrientationMarkLocation));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Die Pitch: {0:#,0.###} x {1:#,0.###} um", parser.DiePitchX, parser.DiePitchY));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Sample Center: {0:#,0.###}, {1:#,0.###} um", currentWafer.SampleCenterLocationX, currentWafer.SampleCenterLocationY));
            sb.AppendLine(string.Format("화면 Index 보정: X+{0}, Y+{1}", m_dMap.IndexOffset.X, m_dMap.IndexOffset.Y));
            sb.AppendLine(string.Format("검사 Die: {0}개 / Defect: {1}개", sampleDies.Count, currentWafer.DefectList.Count));
            sb.AppendLine();
            sb.AppendLine("Defect 많은 Die (KLARF Index)");
            var top = currentWafer.DefectList
                .GroupBy(d => new Point(d.XINDEX, d.YINDEX))
                .OrderByDescending(g => g.Count()).ThenBy(g => g.Key.X).ThenBy(g => g.Key.Y)
                .Take(6);
            foreach (var group in top)
                sb.AppendLine(string.Format("  ({0}, {1}): {2}", group.Key.X - m_dMap.IndexOffset.X, group.Key.Y - m_dMap.IndexOffset.Y, group.Count()));
            txtInfo.Text = sb.ToString();
        }

        /// <summary>Defect 목록을 채운다. 행 Tag 에 Defect 를 두어 선택 시 Map 에 강조한다.</summary>
        private void FillDefectGrid()
        {
            dgvDefect.Rows.Clear();
            foreach (Defect d in currentWafer.DefectList)
            {
                int row = dgvDefect.Rows.Add(d.DEFECTID, d.XINDEX - m_dMap.IndexOffset.X, d.YINDEX - m_dMap.IndexOffset.Y,
                    d.XREL.ToString("0.###", CultureInfo.InvariantCulture), d.YREL.ToString("0.###", CultureInfo.InvariantCulture),
                    d.CLASSNUMBER, d.FINEBINNUMBER, d.ROUGHBINNUMBER, d.DSIZE.ToString("0.###", CultureInfo.InvariantCulture));
                dgvDefect.Rows[row].Tag = d;
            }
            dgvDefect.ClearSelection();
        }

        /// <summary>목록에서 고른 Defect 를 기존 DefectMap 선택 강조(파란 원)로 표시한다.</summary>
        private void dgvDefect_SelectionChanged(object sender, EventArgs e)
        {
            if (currentWafer == null) return;
            m_dMap.SelectedDefect.Clear();
            foreach (DataGridViewRow row in dgvDefect.SelectedRows)
            {
                Defect defect = row.Tag as Defect;
                if (defect != null) m_dMap.SelectedDefect.Add(defect);
            }
            m_dMap.Redraw();
        }

        #endregion

        #region 마우스 위치 정보

        /// <summary>
        /// 마우스 위치를 Die / Defect 정보로 바꿔 아래 표시줄에 보여준다.
        /// x, y 는 Wafer 중심 기준 um 이고 x + OriginX 가 KLARF 좌표계(원본 Index 0 Die 좌하단 기준)이다.
        ///   원본 Die Index = floor((x + OriginX) / DiePitchX)  /  Die 안 위치(XREL) = 나머지
        /// </summary>
        private void m_dMap_MousePositionChanged(object sender, KlarfMousePositionEventArgs e)
        {
            if (currentWafer == null) return;

            double klarfX = e.X + m_dMap.OriginX;
            double klarfY = e.Y + m_dMap.OriginY;
            int dieX = (int)Math.Floor(klarfX / parser.DiePitchX);
            int dieY = (int)Math.Floor(klarfY / parser.DiePitchY);
            double relX = klarfX - dieX * parser.DiePitchX;
            double relY = klarfY - dieY * parser.DiePitchY;
            int mapX = dieX + m_dMap.IndexOffset.X;
            int mapY = dieY + m_dMap.IndexOffset.Y;
            bool onWafer = e.X * e.X + e.Y * e.Y <= Math.Pow(m_dMap.WaferSize / 2d, 2);
            int dieDefects = currentWafer.DefectList.Count(d => d.XINDEX == mapX && d.YINDEX == mapY);

            lblMouse.Text = string.Format(CultureInfo.InvariantCulture,
                "Wafer 좌표 (중심 기준): X={0:#,0.0}, Y={1:#,0.0} um   |   KLARF 좌표: X={2:#,0.0}, Y={3:#,0.0} um   |   중심 거리 {4:#,0.0} um{5}\r\n" +
                "Die (KLARF Index): ({6}, {7})  {8}   |   Die 안 위치 XREL={9:#,0.0}, YREL={10:#,0.0} um   |   Die Defect {11}개\r\n" +
                "가까운 Defect: {12}",
                e.X, e.Y, klarfX, klarfY, Math.Sqrt(e.X * e.X + e.Y * e.Y), onWafer ? "" : " (Wafer 밖)",
                dieX, dieY, sampleDies.Contains(new Point(mapX, mapY)) ? "[검사 Die]" : "[미검사]",
                relX, relY, dieDefects, DescribeNearDefect(e.Screen));
        }

        /// <summary>마우스가 Map 밖으로 나가면 좌표를 지운다.</summary>
        private void m_dMap_MousePositionCleared(object sender, EventArgs e)
        {
            ClearMouseInfo();
        }

        private void ClearMouseInfo()
        {
            lblMouse.Text = "Wafer 좌표: -\r\nDie (KLARF Index): -\r\n가까운 Defect: -";
        }

        /// <summary>화면에서 NearDefectPixel 안에 있는 가장 가까운 보이는 Defect 를 설명 문자열로 만든다.</summary>
        private string DescribeNearDefect(Point screen)
        {
            Defect nearest = null;
            double best = NearDefectPixel * NearDefectPixel;
            foreach (Defect defect in m_dMap.Defects)
            {
                if (!defect.Visible) continue;
                PointF p = m_dMap.ToScreen(defect.X, defect.Y);
                double dist = (p.X - screen.X) * (p.X - screen.X) + (p.Y - screen.Y) * (p.Y - screen.Y);
                if (dist <= best)
                {
                    best = dist;
                    nearest = defect;
                }
            }
            if (nearest == null) return "-";

            string className;
            if (currentWafer.ClassLookup == null || !currentWafer.ClassLookup.TryGetValue(nearest.CLASSNUMBER, out className))
                className = "-";
            return string.Format(CultureInfo.InvariantCulture,
                "ID {0} / Class {1} ({2}) / FineBin {3} / RoughBin {4} / XREL={5:0.0}, YREL={6:0.0} / Size {7:0.###} / Image {8}",
                nearest.DEFECTID, nearest.CLASSNUMBER, className, nearest.FINEBINNUMBER, nearest.ROUGHBINNUMBER,
                nearest.XREL, nearest.YREL, nearest.DSIZE, nearest.IMAGECOUNT);
        }

        #endregion

        /// <summary>Wafer 콤보 표시용. 파일 원본 WaferID 와 Slot 을 함께 보여준다.</summary>
        private sealed class WaferItem
        {
            public WaferItem(Wafer wafer, string rawWaferId)
            {
                Wafer = wafer;
                RawWaferId = rawWaferId;
            }
            public Wafer Wafer { get; private set; }
            /// <summary>파일에 적힌 WaferID 원본 값.</summary>
            public string RawWaferId { get; private set; }
            public override string ToString()
            {
                return string.Format("{0} (Slot {1})", RawWaferId, Wafer.Slot);
            }
        }
    }
}
