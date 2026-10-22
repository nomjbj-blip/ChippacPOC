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
    /// 엔지니어가 KLARF 파일과 Defect 이미지를 골라 DMS DB(TQD_*)와 이미지 저장소에 올리는 화면이다. (F08)
    /// 왜: 기존 자동 적재(Inspection.DataService)는 설비 폴더 감시 서비스라 화면에서 바로 올릴 수 없고, 고객 샘플은 기존 규칙으로 저장되지 않는다.
    ///     저장된 데이터는 기존 Defect 조회 화면(왼쪽 Wafer 조회 -> Map Viewer / Gallery / Chart)에서 그대로 조회된다.
    /// 역할:
    /// 1. [파일 선택] / [폴더 선택]: 고르면 바로 기존 Parser 로 읽어 KLARF 요약 / Wafer 목록 / Map 미리보기 / KLARF Defect 목록(Grid)을 보여 준다.
    ///    DB 는 쓰지 않는다. (2026-10-08 요청: [분석] 버튼 제거, 처리 기록 제거, Defect Grid 추가)
    /// 2. [DB 저장]: RO.KlarfUpload 로 BeginUpload(서버 파싱 + 기존 등록 확인) -> 재등록 확인(D1) -> AddImage x N -> CommitUpload 를 호출한다.
    ///    저장 규칙(좌표 / 집계 / SetupID 128 / 이미지 FTP 상대 경로)은 서버 BSL KlarfUpload 한 곳에 있다.
    /// 결과는 Wafer 목록의 상태 / STEP_SEQ / 메시지 열과 완료 안내 창으로 보여 준다.
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-07_KLARF_Upload_DB적재_설계.md 6.5절
    /// </summary>
    public partial class frmKlarfUpload : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>DefectMap 기본 색 배열 크기(frmKlarfFileViewer 와 같은 제한).</summary>
        private const int MaxColorCode = 255;

        /// <summary>Wafer 외곽 띠 두께(um). 표시용이다(frmKlarfFileViewer 와 같은 값).</summary>
        private const double WaferEdgeMicron = 1200d;

        private static readonly Color[] Palette =
        {
            Color.FromArgb(235, 87, 87), Color.FromArgb(242, 153, 74), Color.FromArgb(47, 98, 235), Color.FromArgb(39, 174, 128),
            Color.FromArgb(155, 81, 224), Color.FromArgb(140, 86, 75), Color.FromArgb(227, 119, 194), Color.FromArgb(127, 127, 127),
            Color.FromArgb(188, 189, 34), Color.FromArgb(23, 190, 207), Color.FromArgb(0, 0, 0), Color.FromArgb(255, 215, 0)
        };

        private ParserKlarf parser;
        private string analyzedFile;
        private Dictionary<int, string> rawWaferIds = new Dictionary<int, string>();
        /// <summary>KLARF 가 참조하는 이미지 이름 중 이미지 폴더에서 찾은 파일 (이름 -> 전체 경로).</summary>
        private Dictionary<string, string> foundImages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>KLARF 가 참조하지만 이미지 폴더에 없는 파일 이름.</summary>
        private List<string> missingImages = new List<string>();
        private RO.KlarfUpload uploadService;

        /// <summary>화면 구성과 이벤트 연결은 InitializeComponent 에서 한다.</summary>
        public frmKlarfUpload()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            ClearView();
        }

        /// <summary>원격 호출 객체. 처음 저장할 때 만든다(파일만 읽을 때는 Service 연결이 필요 없음).</summary>
        private RO.KlarfUpload UploadService
        {
            get { return uploadService ?? (uploadService = new RO.KlarfUpload()); }
        }

        #region 파일 / 폴더 선택

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "KLARF 파일 선택";
                dialog.Filter = "모든 파일|*.*";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtFile.Text = dialog.FileName;
                txtImageDir.Text = ""; // 새 파일이면 이미지 폴더는 [폴더 선택]으로 다시 고른다(이전 파일의 폴더를 잘못 쓰지 않게)
                Analyze(); // 2026-10-08: [분석] 버튼 없이 파일을 고르면 바로 읽는다
            }
        }

        /// <summary>
        /// 이미지 폴더를 탐색기 형식 창(FolderPickerDialog)으로 고른다. 요청(2026-10-07): 옛 트리 형식 창은 폴더 찾기가 어렵다.
        /// 시작 폴더 = 지금 이미지 폴더, 없으면 KLARF 파일 폴더.
        /// </summary>
        private void btnImageDir_Click(object sender, EventArgs e)
        {
            string start = txtImageDir.Text.Trim();
            if (!Directory.Exists(start))
            {
                string file = txtFile.Text.Trim();
                start = file.Length > 0 && File.Exists(file) ? Path.GetDirectoryName(file) : "";
            }
            string selected = FolderPickerDialog.Show(this, "Defect 이미지 폴더 선택 (KLARF 의 TiffFileName 파일이 있는 폴더)", start);
            if (selected == null) return;
            txtImageDir.Text = selected;
            if (File.Exists(txtFile.Text.Trim())) Analyze(); // 파일이 있으면 바로 다시 읽어 이미지 찾음 수를 갱신한다
        }

        #endregion

        #region 파일 읽기 / 미리보기 (DB 저장 없음)

        /// <summary>
        /// 파일을 기존 Parser 로 읽어 미리보기를 만든다. 파일 / 이미지 폴더를 고를 때마다 호출된다.
        /// 순서: Parser 선택(FileAnalyzer) -> 오류 / Wafer / Pitch / SampleSize 검사 -> 원본 WaferID -> 지정한 이미지 폴더에서 찾기
        ///       -> 저장 값 기본값(같은 파일을 다시 읽을 때는 사용자가 고친 값을 유지) -> KLARF 요약 -> Wafer 목록 -> 첫 Wafer Map / Defect 목록.
        /// 저장 시에는 서버가 같은 Parser 로 다시 읽으므로, 여기 값은 확인용이다(Origin Index 는 서버가 DB 저장값을 우선 적용).
        /// 진행 표시(2026-10-14): 파일 읽기 / 원본 WaferID / 이미지 찾기는 화면 아래 busyBar 가 작업 스레드에서 돌리고 작업 이름을 보인다.
        /// </summary>
        private void Analyze()
        {
            string path = txtFile.Text.Trim();
            bool sameFile = analyzedFile != null && string.Equals(analyzedFile, path, StringComparison.OrdinalIgnoreCase);
            ClearView();
            if (path.Length == 0 || !File.Exists(path))
            {
                lblStatus.Text = string.Empty;
                return;
            }

            IDisposable scope = busyBar.Begin("KLARF 읽기");
            try
            {
                ParserKlarf loaded = busyBar.Run("파일 읽기", () =>
                {
                    FileAnalyzer analyzer = new FileAnalyzer(path);
                    return analyzer.FileType == FileType.KlarfFile_1_8 ? new ParserKlarf_18(path) : new ParserKlarf(path);
                });
                if (loaded.ErrorFlag) throw new FormatException(FirstLine(loaded.ErrorMessage));
                if (loaded.Wafers == null || loaded.Wafers.Count == 0)
                    throw new FormatException("Wafer 정보가 없습니다. WaferID / Slot / DieOrigin 항목을 확인하세요.");
                if (loaded.DiePitchX <= 0 || loaded.DiePitchY <= 0) throw new FormatException("DiePitch 값이 없습니다.");
                if (loaded.SampleSize <= 0) throw new FormatException("SampleSize (Wafer 크기) 값이 없습니다.");

                parser = loaded;
                analyzedFile = path;
                rawWaferIds = busyBar.Run("원본 WaferID 읽기", () => ReadRawWaferIds(path));
                ResolveImages(path);

                if (!sameFile || txtProduct.Text.Trim().Length == 0)
                {
                    txtProduct.Text = parser.DeviceID;
                    txtStepId.Text = parser.StepID;
                    txtEquip.Text = parser.Equip;
                }
                FillSummary();
                FillWaferGrid();
                if (dgvWafer.Rows.Count > 0) dgvWafer.Rows[0].Selected = true;
                ShowSelectedWafer();
                btnSave.Enabled = true;

                // 상태 줄은 결과만 짧게 보인다(사용법 안내 문구 없음, 2026-10-15).
                string imageNote = txtImageDir.Text.Length > 0 && missingImages.Count > 0 ? string.Format(" / 이미지 {0}개 없음", missingImages.Count) : "";
                lblStatus.Text = string.Format("읽기 완료 / Wafer {0}장{1}", parser.Wafers.Count, imageNote);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                ClearView();
                lblStatus.Text = "파일 읽기 실패";
                Notify(ex.Message, MessageBoxIcon.Warning);
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// KLARF 가 참조하는 이미지 이름을 모으고 [폴더 선택]으로 지정한 이미지 폴더에서 찾는다.
        /// 폴더를 지정하지 않으면 모두 "없음"으로 둔다(2026-10-07 화면 수정: 자동으로 폴더를 고르지 않음).
        /// </summary>
        private void ResolveImages(string klarfPath)
        {
            List<string> names = new List<string>();
            foreach (Wafer wafer in parser.Wafers)
                foreach (string name in GetImageNames(wafer))
                    if (!names.Contains(name, StringComparer.OrdinalIgnoreCase)) names.Add(name);

            string folder = txtImageDir.Text.Trim();

            foundImages.Clear();
            missingImages.Clear();
            // 폴더의 파일 확인(File.Exists)은 네트워크 폴더면 오래 걸릴 수 있어 작업 스레드에서 한다
            Dictionary<string, string> found = busyBar.Run("이미지 찾기 (" + names.Count + "개)", () =>
            {
                Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string name in names)
                {
                    string full = folder.Length > 0 ? Path.Combine(folder, name) : null;
                    if (full != null && File.Exists(full)) map[name] = full;
                }
                return map;
            });
            foreach (string name in names)
            {
                string full;
                if (found.TryGetValue(name, out full)) foundImages[name] = full;
                else missingImages.Add(name);
            }
        }

        /// <summary>Wafer 의 Defect 가 참조하는 이미지 파일 이름(중복 제거). DefectImageList 인덱서는 string 이라 GetDefectImage 로 꺼낸다.</summary>
        private static List<string> GetImageNames(Wafer wafer)
        {
            List<string> names = new List<string>();
            foreach (Defect d in wafer.DefectList)
            {
                if (d.Images == null) continue;
                for (int i = 0; i < d.Images.Count; i++)
                {
                    string name = d.Images.GetDefectImage(i).IMAGEPATH;
                    if (!string.IsNullOrEmpty(name))
                    {
                        name = Path.GetFileName(name);
                        if (!names.Contains(name, StringComparer.OrdinalIgnoreCase)) names.Add(name);
                    }
                }
            }
            return names;
        }

        /// <summary>Wafer 목록을 채운다. 행 Tag 에 Wafer 를 둔다. 상태 / STEP_SEQ 는 저장 단계에서 서버 결과로 바뀐다.</summary>
        private void FillWaferGrid()
        {
            dgvWafer.Rows.Clear();
            foreach (Wafer wafer in parser.Wafers)
            {
                List<string> names = GetImageNames(wafer);
                int found = names.Count(n => foundImages.ContainsKey(n));
                string rawId;
                int row = dgvWafer.Rows.Add(
                    rawWaferIds.TryGetValue(wafer.Slot, out rawId) ? rawId : "",
                    wafer.WaferID, wafer.Slot, wafer.DefectList.Count, CountSampleDies(wafer),
                    string.Format("{0} / {1}", found, names.Count), "읽음(미저장)", "", "");
                dgvWafer.Rows[row].Tag = wafer;
            }
            dgvWafer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); // 열 자동 너비 대신 다 채운 뒤 한 번 (2026-10-14)
            dgvWafer.ClearSelection();
        }

        private static int CountSampleDies(Wafer wafer)
        {
            HashSet<Point> dies = new HashSet<Point>();
            foreach (InspectionTest test in wafer.TestList)
                if (test.SampleTestPlan != null)
                    foreach (Point p in test.SampleTestPlan) dies.Add(p);
            return dies.Count;
        }

        /// <summary>Wafer 를 고르면 Map 과 Defect 목록(최대 수천 행)을 다시 그린다. 그동안 모래시계 커서 (2026-10-15).</summary>
        private void dgvWafer_SelectionChanged(object sender, EventArgs e)
        {
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                ShowSelectedWafer();
                FillDefectGrid();
            }
        }

        /// <summary>Wafer 목록에서 선택한 Wafer. 선택이 없으면 null.</summary>
        private Wafer SelectedWafer
        {
            get { return parser == null || dgvWafer.SelectedRows.Count == 0 ? null : dgvWafer.SelectedRows[0].Tag as Wafer; }
        }

        /// <summary>
        /// 요약 한 줄: Lot / Device / Step / 검사시각 / Wafer 크기 / Die Pitch / Wafer 수 / Defect 수 / 이미지 찾음 수.
        /// 처리 기록 창을 없앤 대신(2026-10-08) 파일에서 읽은 핵심 값을 한눈에 보이게 한다.
        /// </summary>
        private void FillSummary()
        {
            int defects = parser.Wafers.Cast<Wafer>().Sum(w => w.DefectList.Count);
            int images = foundImages.Count + missingImages.Count;
            lblSummary.Text = string.Format(CultureInfo.InvariantCulture,
                "Lot {0}   |   Device {1}   |   Step {2}   |   Wafer {3}장   |   Defect {4}   |   이미지 {5} / {6}",
                parser.LotID, parser.DeviceID, parser.StepID, parser.Wafers.Count, defects, foundImages.Count, images);
        }

        /// <summary>
        /// 선택한 Wafer 의 KLARF Defect 를 표로 보여 준다(2026-10-08 요청 "klarf data 를 grid 형식으로").
        /// Index 는 KLARF 원본 값(화면 Index - 보정량), X / Y 는 Wafer 중심 기준 um.
        /// 분류명은 KLARF 표준대로 ClassLookup 에서 CLASSNUMBER 로 찾는다 (2026-10-15).
        /// 속도(2026-10-14): Defect 1,122건 파일에서 행을 하나씩 넣으며 열 너비를 모든 행 기준으로 다시 계산해 4분 가까이 멈췄다.
        ///   열 자동 너비를 끄고(Designer) 행을 한 번에 넣은 뒤 보이는 행 기준으로 한 번만 너비를 맞춘다.
        /// </summary>
        private void FillDefectGrid()
        {
            dgvDefect.Rows.Clear();
            Wafer wafer = SelectedWafer;
            if (wafer == null)
            {
                grpDefect.Text = "KLARF Defect 목록";
                return;
            }

            int offsetX = (int)wafer.DieOriginX;
            int offsetY = (int)wafer.DieOriginY;
            List<DataGridViewRow> rows = new List<DataGridViewRow>(wafer.DefectList.Count);
            foreach (Defect d in wafer.DefectList)
            {
                string className;
                if (wafer.ClassLookup == null || !wafer.ClassLookup.TryGetValue(d.CLASSNUMBER, out className)) className = "";
                string image = FirstImageName(d);
                string imageState = image.Length == 0 ? "-" : foundImages.ContainsKey(image) ? "있음" : "없음";
                // 열 순서(Designer)를 바꿔도 값이 어긋나지 않게 열 이름으로 넣는다
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDefect);
                r.Cells[colDefectId.Index].Value = d.DEFECTID;
                r.Cells[colIndexX.Index].Value = d.XINDEX - offsetX;
                r.Cells[colIndexY.Index].Value = d.YINDEX - offsetY;
                r.Cells[colClassName.Index].Value = className.Trim();
                r.Cells[colImageFound.Index].Value = imageState;
                r.Cells[colX.Index].Value = d.X.ToString("#,0.###", CultureInfo.InvariantCulture);
                r.Cells[colY.Index].Value = d.Y.ToString("#,0.###", CultureInfo.InvariantCulture);
                r.Cells[colXRel.Index].Value = d.XREL.ToString("0.###", CultureInfo.InvariantCulture);
                r.Cells[colYRel.Index].Value = d.YREL.ToString("0.###", CultureInfo.InvariantCulture);
                r.Cells[colSize.Index].Value = d.DSIZE.ToString("0.###", CultureInfo.InvariantCulture);
                r.Cells[colTest.Index].Value = d.TEST;
                r.Cells[colClass.Index].Value = d.CLASSNUMBER;
                r.Cells[colRoughBin.Index].Value = d.ROUGHBINNUMBER;
                r.Cells[colFineBin.Index].Value = d.FINEBINNUMBER;
                r.Cells[colImageFile.Index].Value = image;
                r.Tag = d;
                if (imageState == "없음") r.Cells[colImageFound.Index].Style.ForeColor = Color.Firebrick;
                rows.Add(r);
            }
            dgvDefect.Rows.AddRange(rows.ToArray());
            dgvDefect.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvDefect.ClearSelection();
            grpDefect.Text = string.Format("KLARF Defect 목록 - {0} / {1}건", wafer.WaferID, wafer.DefectList.Count);
        }

        /// <summary>Defect 의 첫 이미지 파일 이름(없으면 빈 값).</summary>
        private static string FirstImageName(Defect d)
        {
            if (d.Images == null) return "";
            for (int i = 0; i < d.Images.Count; i++)
            {
                string name = d.Images.GetDefectImage(i).IMAGEPATH;
                if (!string.IsNullOrEmpty(name)) return Path.GetFileName(name);
            }
            return "";
        }

        /// <summary>
        /// 선택한 Wafer 를 기존 DefectMap 설정 순서대로 그린다(frmKlarfFileViewer.ShowWafer 와 같은 순서 / 좌표 규칙).
        /// 색상은 CLASSNUMBER 로 칠한다(저장 후 기존 화면의 Class 색 구분과 같음).
        /// </summary>
        private void ShowSelectedWafer()
        {
            if (parser == null || dgvWafer.SelectedRows.Count == 0) return;
            Wafer wafer = dgvWafer.SelectedRows[0].Tag as Wafer;
            if (wafer == null) return;

            int offsetX = (int)wafer.DieOriginX;
            int offsetY = (int)wafer.DieOriginY;
            m_dMap.DefectClear();
            m_dMap.Rotate(0);
            m_dMap.WaferSize = parser.SampleSize * 1000d;
            m_dMap.EdgeSize = Math.Min(WaferEdgeMicron, m_dMap.WaferSize / 100d); // WaferSize 다음에 넣어야 한다(먼저 넣으면 원호 크기 음수)
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
            m_dMap.WaferID = wafer.WaferID;
            m_dMap.DieCalculation(true);
            m_dMap.DrawDefects = "ALL";
            m_dMap.DieClear();

            HashSet<Point> dies = new HashSet<Point>();
            foreach (InspectionTest test in wafer.TestList)
            {
                if (test.SampleTestPlan == null) continue;
                foreach (Point point in test.SampleTestPlan)
                    if (dies.Add(point)) m_dMap.AddDie(new Die(point.X, point.Y, test.TestNo, 1));
            }
            foreach (Defect defect in wafer.DefectList)
                m_dMap.AddDefect(defect);

            m_dMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            ApplyColor(wafer);
        }

        /// <summary>
        /// CLASSNUMBER 코드별 색을 기존 DefectMap.TypeColor 로 넘긴다. 범위를 벗어난 코드가 있으면 색 없이 그린다.
        /// Map 아래 정보 글(Lot / Wafer / 분류별 건수)은 그리지 않는다(2026-10-14 화면 기능 최소화).
        /// </summary>
        private void ApplyColor(Wafer wafer)
        {
            SortedDictionary<int, int> counts = new SortedDictionary<int, int>();
            foreach (Defect d in wafer.DefectList)
            {
                int count;
                counts.TryGetValue(d.CLASSNUMBER, out count);
                counts[d.CLASSNUMBER] = count + 1;
            }

            DataTable table = new DataTable();
            table.Columns.Add("CLASSNUMBER", typeof(int));
            table.Columns.Add("COLOR", typeof(string));
            int index = 0;
            foreach (KeyValuePair<int, int> pair in counts)
            {
                if (pair.Key < 0 || pair.Key > MaxColorCode) continue;
                table.Rows.Add(pair.Key, ColorTranslator.ToHtml(Palette[index++ % Palette.Length]));
            }
            m_dMap.MapType = MAP_TYPE.CLASS;
            m_dMap.TypeColor = table;
            m_dMap.SetInfomation(new string[0]);
            m_dMap.Redraw();
        }

        #endregion

        #region DB 저장

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// 저장 순서(서비스 계약 iKlarfUpload):
        ///  1) BeginUpload: 서버가 같은 Parser 로 읽고 Wafer 별 저장 예정 값 / 기존 등록(EXISTS) / 입력 오류(INVALID)를 돌려준다.
        ///  2) INVALID 가 있으면 저장하지 않는다. EXISTS 가 있으면 "삭제 후 재등록 / 건너뜀 / 취소"를 묻는다(결정 D1).
        ///  3) 없는 이미지가 있으면 계속할지 묻는다(없는 이미지는 서버가 행을 만들지 않음).
        ///  4) 찾은 이미지 파일을 하나씩 AddImage -> CommitUpload. 결과를 Wafer 목록과 로그에 표시한다.
        /// 중간에 그만두거나 오류가 나면 CancelUpload 로 서버 작업 폴더를 지운다.
        /// 진행 표시(2026-10-14): 서버 확인 / 이미지 전송(n / 전체) / DB 저장은 화면 아래 busyBar 가 작업 스레드에서 돌린다(확인 창은 그 사이에 묻는다).
        /// </summary>
        private void Save()
        {
            if (parser == null || analyzedFile == null)
            {
                Notify("먼저 [파일 선택]으로 KLARF 파일을 고르세요.", MessageBoxIcon.Information);
                return;
            }
            if (!string.Equals(analyzedFile, txtFile.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                Notify("파일 경로가 바뀌었습니다. [파일 선택]으로 다시 고르세요.", MessageBoxIcon.Information);
                return;
            }

            string key = null;
            try
            {
                btnSave.Enabled = false;
                string[] info = new string[] {
                    DACrux.Base.GlobalVariable.Factory,
                    DACrux.Base.GlobalVariable.UserID,
                    Path.GetFileName(analyzedFile),
                    txtProduct.Text.Trim(),
                    txtStepId.Text.Trim(),
                    txtEquip.Text.Trim(),
                    "" }; // 6번 = 예약(빈 값). 분류는 서버가 KLARF 표준대로 저장 (2026-10-15)

                SetStatus("서버에서 파일 확인 중...");
                string file = analyzedFile;
                DataTable preview = busyBar.Run("서버에서 파일 확인", () => UploadService.BeginUpload(info, File.ReadAllBytes(file)));
                if (preview.Rows.Count == 0) throw new Exception("서버 확인 결과가 없습니다.");
                key = preview.Rows[0]["UPLOAD_KEY"].ToString();
                ApplyServerRows(preview);

                DataRow[] invalid = preview.Select("RESULT = 'INVALID'");
                if (invalid.Length > 0)
                {
                    CancelQuietly(key);
                    key = null;
                    Notify("저장할 수 없는 값이 있습니다.\r\n" + string.Join("\r\n", invalid.Select(r => r["WAFER_ID"] + ": " + r["MESSAGE"]).ToArray()), MessageBoxIcon.Warning);
                    SetStatus("저장 안 함 / 입력 값 확인");
                    return;
                }

                string overwrite = "N";
                DataRow[] exists = preview.Select("RESULT = 'EXISTS'");
                if (exists.Length > 0)
                {
                    DialogResult answer = Ask(string.Format(
                        "이미 등록된 Wafer 가 {0}장 있습니다.\r\n{1}\r\n\r\n예: 기존 데이터를 삭제하고 다시 등록\r\n아니요: 등록된 Wafer 는 건너뛰고 나머지만 저장\r\n취소: 저장하지 않음",
                        exists.Length, string.Join("\r\n", exists.Select(r => "  " + r["WAFER_ID"] + " (STEP_SEQ " + r["EXIST_STEP_SEQ"] + ")").ToArray())),
                        MessageBoxButtons.YesNoCancel);
                    if (answer == DialogResult.Cancel)
                    {
                        CancelQuietly(key);
                        key = null;
                        SetStatus("저장 취소");
                        return;
                    }
                    overwrite = answer == DialogResult.Yes ? "Y" : "N";
                }

                if (missingImages.Count > 0)
                {
                    DialogResult answer = Ask(string.Format("이미지 {0}개를 이미지 폴더에서 찾지 못했습니다.\r\n없는 이미지는 빼고 저장할까요?\r\n\r\n{1}",
                        missingImages.Count, string.Join("\r\n", missingImages.Take(10).ToArray())), MessageBoxButtons.YesNo);
                    if (answer != DialogResult.Yes)
                    {
                        CancelQuietly(key);
                        key = null;
                        SetStatus("저장 취소 / 이미지 폴더 확인");
                        return;
                    }
                }

                if (foundImages.Count > 0)
                {
                    SetStatus(string.Format("이미지 전송 중 ({0}개)", foundImages.Count));
                    string uploadKey = key;
                    List<KeyValuePair<string, string>> images = foundImages.ToList();
                    busyBar.RunSteps("이미지 전송", report =>
                    {
                        for (int i = 0; i < images.Count; i++)
                        {
                            UploadService.AddImage(uploadKey, images[i].Key, File.ReadAllBytes(images[i].Value));
                            report(i + 1, images.Count);
                        }
                        return images.Count;
                    });
                }

                SetStatus("DB 저장 중...");
                string commitKey = key;
                DataTable result = busyBar.Run("DB 저장", () => UploadService.CommitUpload(commitKey, new string[] { overwrite }));
                key = null; // Commit 은 성공 / 실패와 관계없이 서버 작업 폴더를 지운다
                ApplyServerRows(result);
                ReportResult(result);
            }
            catch (Exception ex)
            {
                if (key != null) CancelQuietly(key);
                SetStatus("저장 실패: " + FirstLine(ex.Message));
                Notify("저장 중 오류가 났습니다.\r\n" + FirstLine(ex.Message), MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = parser != null;
            }
        }

        /// <summary>
        /// 서버 결과(Begin / Commit)를 Slot 으로 Wafer 목록 행에 맞춰 상태 / STEP_SEQ / 메시지를 표시한다.
        /// STEP_SEQ / 메시지 열은 화면에서 숨기고 값만 채운다(2026-10-22). 메시지는 입력 오류 / 저장 결과 안내 창에 같이 나온다.
        /// </summary>
        private void ApplyServerRows(DataTable table)
        {
            foreach (DataRow r in table.Rows)
            {
                int slot = System.Convert.ToInt32(r["SLOT_ID"], CultureInfo.InvariantCulture); // DACrux.Base.Convert 와 이름이 겹쳐 System 을 붙인다
                foreach (DataGridViewRow row in dgvWafer.Rows)
                {
                    Wafer wafer = row.Tag as Wafer;
                    if (wafer == null || wafer.Slot != slot) continue;
                    row.Cells[colState.Index].Value = StateText(r["RESULT"].ToString());
                    object step = r["STEP_SEQ"] != DBNull.Value ? r["STEP_SEQ"] : r["EXIST_STEP_SEQ"];
                    row.Cells[colStepSeq.Index].Value = step == DBNull.Value ? "" : step.ToString();
                    row.Cells[colMessage.Index].Value = r["MESSAGE"].ToString();
                    if (table.Columns.Contains("SAVED_IMAGES") && r["SAVED_IMAGES"] != DBNull.Value)
                        row.Cells[colImages.Index].Value = string.Format("저장 {0}", r["SAVED_IMAGES"]);
                }
            }
            dgvWafer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private static string StateText(string result)
        {
            switch (result)
            {
                case "READY": return "신규";
                case "EXISTS": return "등록됨";
                case "INVALID": return "입력 오류";
                case "SAVED": return "저장";
                case "SKIPPED": return "건너뜀";
                case "ERROR": return "오류";
                default: return result;
            }
        }

        /// <summary>
        /// 저장 결과를 상태 표시줄과 완료 안내 창으로 알린다(Wafer 별 STEP_SEQ / 메시지는 이 안내 창에서 본다. 목록에서는 숨김).
        /// 기존 Defect 조회 화면에서 찾는 방법(Lot / 기간)도 함께 알려 준다.
        /// </summary>
        private void ReportResult(DataTable result)
        {
            int saved = result.Select("RESULT = 'SAVED'").Length;
            int skipped = result.Select("RESULT = 'SKIPPED'").Length;
            int error = result.Select("RESULT = 'ERROR'").Length;
            StringBuilder lines = new StringBuilder();
            foreach (DataRow r in result.Rows)
                lines.AppendFormat("  {0} : {1} / STEP_SEQ {2} / Defect {3} / 이미지 {4}{5}\r\n", r["WAFER_ID"], StateText(r["RESULT"].ToString()),
                    r["STEP_SEQ"], r["DEFECTS"], r["SAVED_IMAGES"], r["MESSAGE"].ToString().Length > 0 ? " (" + r["MESSAGE"] + ")" : "");

            string date = parser.ResultTimestamp.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string guide = string.Format("왼쪽 Wafer 조회에서 Lot = {0}, 기간에 {1} 를 넣어 조회하세요.", parser.LotID, date);
            SetStatus(string.Format("저장 {0} / 건너뜀 {1} / 오류 {2}", saved, skipped, error));
            Notify(string.Format("저장 {0} / 건너뜀 {1} / 오류 {2}\r\n\r\n{3}\r\n{4}", saved, skipped, error, lines, guide),
                error > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void CancelQuietly(string key)
        {
            try { UploadService.CancelUpload(key); }
            catch { /* 서버 작업 폴더는 하루 뒤 서버가 정리한다 */ }
        }

        #endregion

        #region 공통

        /// <summary>확인 창. 화면 검사(Smoke)는 이 함수를 바꿔 정해진 답을 쓴다.</summary>
        protected virtual DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, text, "KLARF 업로드", buttons, MessageBoxIcon.Question);
        }

        /// <summary>안내 창. 화면 검사(Smoke)는 이 함수를 바꿔 창을 띄우지 않는다.</summary>
        protected virtual void Notify(string text, MessageBoxIcon icon)
        {
            MessageBox.Show(this, text, "KLARF 업로드", MessageBoxButtons.OK, icon);
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        /// <summary>다른 파일을 읽기 전 / 실패 시 이전 결과가 남지 않게 비운다. 저장 값 입력란은 Analyze 가 다시 채운다.</summary>
        private void ClearView()
        {
            parser = null;
            analyzedFile = null;
            rawWaferIds = new Dictionary<int, string>();
            foundImages.Clear();
            missingImages.Clear();
            dgvWafer.Rows.Clear();
            dgvDefect.Rows.Clear();
            grpDefect.Text = "KLARF Defect 목록";
            lblSummary.Text = string.Empty;
            btnSave.Enabled = false;
            m_dMap.DefectClear();
            m_dMap.DieClear();
            m_dMap.SetInfomation(new string[0]);
            m_dMap.Redraw();
        }

        /// <summary>원본 WaferID(Parser 가 DMS 규칙으로 바꾸기 전 값). frmKlarfFileViewer.ReadRawWaferIds 와 같은 방식(KLARF 1.2 형식).</summary>
        private static Dictionary<int, string> ReadRawWaferIds(string path)
        {
            var result = new Dictionary<int, string>();
            foreach (Match match in Regex.Matches(File.ReadAllText(path), "WaferID\\s+\"?([^\";]*)\"?\\s*;\\s*Slot\\s+(\\d+)"))
            {
                int slot = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                if (!result.ContainsKey(slot)) result.Add(slot, match.Groups[1].Value.Trim());
            }
            return result;
        }

        private static string FirstLine(string message)
        {
            if (string.IsNullOrEmpty(message)) return "";
            int idx = message.IndexOfAny(new[] { '\r', '\n' });
            return idx > 0 ? message.Substring(0, idx) : message;
        }

        #endregion
    }
}
