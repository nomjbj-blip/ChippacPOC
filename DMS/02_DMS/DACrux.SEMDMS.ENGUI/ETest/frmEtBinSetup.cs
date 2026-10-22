using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test Bin 설정 화면이다. (2026-10-22, TEST 화면 frmSetupBin 을 POC 용으로 새로 만든 것. frmSetupBin 은 그대로 둔다)
    /// 설계: 문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md
    /// 기준:
    ///  - TestArea / Program 대신 E-Test Device / 공정으로 고른다. YMS Program = Device_공정(업로드와 같은 규칙, 읽기 전용 칸).
    ///  - 행 = 그 YMS Program 의 Bin 정의(TQP_BINDESC) + 업로드 파일 Die 에 나온 Hardware Bin. 0 ~ 150 고정 행은 없다.
    ///  - 고치는 값은 이름 / 양품 / 색 / 설명뿐이다. Software Bin / Die / Pass / Fail / 파일 / 상태는 양품을 정하는 근거로 보이기만 한다.
    ///    자동으로 바꾸는 값은 없다. Map 기호 / 표시 / Limit 수는 서버가 그대로 둔다.
    /// 처리 흐름:
    ///  1) Device / 공정 콤보를 고르면 바로 조회한다(저장 안 한 수정이 있으면 먼저 묻고, 아니요면 콤보를 원래 값으로 되돌림).
    ///  2) 이름 / 설명 = 더블클릭(F2), 양품 = 클릭(Space), 색 = 더블클릭(F2) -> 색 창. 바뀐 칸 노란색(색 칸은 굵은 글자) + 행 머리글 '*', 틀린 칸 붉은색.
    ///  3) [Bin 추가] = 표 끝 새 행(Bin 번호 직접 입력). [선택 삭제] = 고른 행의 Bin 정의 삭제(새 행 / 정의 없는 행은 표에서만 뺌).
    ///  4) [저장] = 바뀐 행 + 새 행만 SaveEtBins. 양품이 바뀌었고 YMS 반영 파일이 있으면 수율 다시 계산(RejudgeEtFiles)을 묻는다.
    /// 진행 표시: 서버 호출은 화면 아래 busyBar(ProgressStatusBar)로 묶는다. 저장 / 삭제 / 수율 다시 계산은 Run(별도 스레드),
    ///  콤보 변경에서도 부르는 목록 / Bin 조회는 RunHere(화면 스레드)다.
    /// </summary>
    public partial class frmEtBinSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private static readonly Color DirtyColor = Color.LightYellow;
        private static readonly Color ErrorColor = Color.MistyRose;
        private static readonly Regex ColorPattern = new Regex("^#[0-9A-F]{6}$", RegexOptions.CultureInvariant);

        private RO.GdsEtResult binService;
        private string loadedFactory = string.Empty;
        private string loadedDevice = string.Empty;
        private string loadedProgram = string.Empty;
        private bool filling;
        /// <summary>Factory 의 READY 파일이 있는 Device + 공정 (콤보 목록, YMS_FILE_COUNT / YMS_PROGRAM 포함)</summary>
        private DataTable programList;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtBinSetup()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colBin, colDie, colPass, colFail, colFiles })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colName, colGood, colColor, colDesc })
                column.HeaderCell.Style.Font = new Font(dgvBin.Font, FontStyle.Bold);
        }

        /// <summary>E-Test 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return binService ?? (binService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 의 Device / 공정 목록을 채우고 첫 Device / 공정을 조회한다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadPrograms();
            if (cboDevice.Text.Length > 0 && cboTestProgram.Text.Length > 0)
                Search();
        }

        #region [조건 / 조회]

        /// <summary>READY 파일이 있는 Device + 공정을 읽어 콤보 목록을 채운다. 고른 Device 가 목록에 없으면 첫 Device 를 고른다.</summary>
        private void LoadPrograms()
        {
            string factory = txtFactory.Text.Trim();
            programList = null;
            if (factory.Length == 0) return;
            try
            {
                programList = busyBar.RunHere("Device 목록", () => Service.GetEtBinProgramList(factory));
            }
            catch (Exception ex)
            {
                SetStatus("Device 목록 조회 실패: " + Cause(ex).Message);
                return;
            }
            List<string> devices = programList.Rows.Cast<DataRow>().Select(r => r["DEVICE_ID"].ToString()).Distinct(StringComparer.Ordinal).ToList();
            string device = devices.Contains(cboDevice.Text, StringComparer.Ordinal) ? cboDevice.Text : devices.FirstOrDefault();
            FillCombo(cboDevice, devices, device);
            FillProgramCombo(cboTestProgram.Text);
            if (devices.Count == 0)
                SetStatus("업로드된 E-Test 파일이 없습니다. E-Test Result Upload 화면에서 먼저 올리세요.");
        }

        /// <summary>지금 Device 의 공정을 콤보 목록에 채운다. select 가 목록에 없으면 첫 공정을 고른다.</summary>
        private void FillProgramCombo(string select)
        {
            string device = cboDevice.Text;
            List<string> programs = programList == null ? new List<string>() : programList.Rows.Cast<DataRow>()
                .Where(r => string.Equals(r["DEVICE_ID"].ToString(), device, StringComparison.Ordinal))
                .Select(r => r["TEST_PROGRAM"].ToString()).ToList();
            FillCombo(cboTestProgram, programs, programs.Contains(select, StringComparer.Ordinal) ? select : programs.FirstOrDefault());
        }

        private void FillCombo(ComboBox combo, IEnumerable<string> items, string select)
        {
            bool wasFilling = filling;
            filling = true;
            try
            {
                combo.BeginUpdate();
                combo.Items.Clear();
                foreach (string item in items) combo.Items.Add(item);
                combo.EndUpdate();
                combo.SelectedIndex = select == null ? -1 : combo.Items.IndexOf(select);
            }
            finally
            {
                filling = wasFilling;
            }
        }

        /// <summary>Device 를 고르면 그 Device 의 공정 목록을 채우고 바로 조회한다. 저장 안 한 수정을 버리지 않으면 원래 값으로 되돌린다.</summary>
        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling || cboDevice.SelectedIndex < 0) return;
            if (!ConfirmDiscardChanges())
            {
                RestoreCombos();
                return;
            }
            FillProgramCombo(cboTestProgram.Text);
            Search();
        }

        private void cboTestProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling || cboTestProgram.SelectedIndex < 0) return;
            if (!ConfirmDiscardChanges())
            {
                RestoreCombos();
                return;
            }
            Search();
        }

        private void RestoreCombos()
        {
            bool wasFilling = filling;
            filling = true;
            cboDevice.SelectedIndex = cboDevice.Items.IndexOf(loadedDevice);
            filling = wasFilling;
            FillProgramCombo(loadedProgram);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ConfirmDiscardChanges()) Search();
        }

        /// <summary>저장 안 된 수정이 있으면 버릴지 묻는다.</summary>
        private bool ConfirmDiscardChanges()
        {
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.EndEdit();
            int dirty = DirtyRows().Count;
            return dirty == 0
                || Ask("저장하지 않은 수정 " + dirty + "행이 있습니다. 버리고 계속할까요?", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>조회: 위 Factory / Device / 공정의 Bin 표를 다시 읽는다. 저장 안 된 수정 확인은 부르는 쪽에서 한다.</summary>
        private void Search()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text;
            string program = cboTestProgram.Text;
            if (factory.Length == 0 || device.Length == 0 || program.Length == 0)
            {
                Notify("Factory / Device / 공정을 고르세요. 목록이 비어 있으면 E-Test Result Upload 화면에서 결과 파일을 먼저 올리세요.", MessageBoxIcon.Warning);
                return;
            }
            loadedFactory = factory;
            loadedDevice = device;
            loadedProgram = program;
            txtYmsProgram.Text = EtYmsRule.ToProgramName(device, EtYmsRule.ToTestArea(program));
            using (busyBar.Begin("조회"))
            {
                LoadBins();
            }
        }

        /// <summary>
        /// Bin 정의 + 파일 Bin 분포를 읽어 표를 채운다. 각 행 Tag(BinState)에 조회 당시 값을 넣어 수정 여부를 비교한다.
        /// YMS 에 넣을 수 없는 Bin(Hardware Bin 없음 / 0 ~ 99 밖)은 보이기만 하고 고칠 수 없다.
        /// </summary>
        private void LoadBins()
        {
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.CancelEdit();
            dgvBin.CurrentCell = null;
            dgvBin.Rows.Clear();
            string[] key = { loadedFactory, loadedDevice, loadedProgram };
            DataTable dt;
            try
            {
                dt = busyBar.RunHere("Bin 조회", () => Service.GetEtBinList(key)); // 콤보 변경 이벤트에서도 부르므로 화면 스레드
            }
            catch (Exception ex)
            {
                SetStatus("Bin 조회 실패 / " + Cause(ex).Message);
                Notify("Bin 조회 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                return;
            }
            filling = true;
            dgvBin.SuspendLayout();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BinState s = new BinState
                    {
                        Bin = dr["BIN"].ToString(),
                        HasDef = dr["HAS_DEF"].ToString() == "Y",
                        InRange = dr["IN_RANGE"].ToString() == "Y",
                        Name = dr["BIN_NAME"].ToString(),
                        Good = dr["HIGH_GEC"].ToString() == "Y",
                        Color = dr["COLOR"].ToString(),
                        Description = dr["DESCRIPTION"].ToString(),
                        Die = Convert.ToInt64(dr["DIE_COUNT"], CultureInfo.InvariantCulture),
                        Pass = Convert.ToInt64(dr["PASS_COUNT"], CultureInfo.InvariantCulture),
                        Fail = Convert.ToInt64(dr["FAIL_COUNT"], CultureInfo.InvariantCulture)
                    };
                    if (dr["YMS_PROGRAM"].ToString().Length > 0) txtYmsProgram.Text = dr["YMS_PROGRAM"].ToString();
                    AddRow(s, dr["SW_BINS"].ToString(), Convert.ToInt64(dr["FILE_COUNT"], CultureInfo.InvariantCulture));
                }
            }
            finally
            {
                dgvBin.ResumeLayout();
                filling = false;
            }
            dgvBin.ClearSelection();
            int good = dt.Rows.Cast<DataRow>().Count(r => r["HIGH_GEC"].ToString() == "Y");
            long die = dt.Rows.Cast<DataRow>().Sum(r => Convert.ToInt64(r["DIE_COUNT"], CultureInfo.InvariantCulture));
            SetStatus("조회 완료 / Device " + loadedDevice + " / 공정 " + loadedProgram + " / YMS Program " + txtYmsProgram.Text
                + " / Bin " + dt.Rows.Count + " / 양품 Bin " + good + " / 파일 Die " + die + YmsFileText());
        }

        /// <summary>표에 한 행을 넣는다. 새 행이면 Bin 칸만 비워 둔다.</summary>
        private DataGridViewRow AddRow(BinState s, string softBins, long files)
        {
            int index = dgvBin.Rows.Add(s.Bin, s.Name, s.Good, s.Color, s.Description, softBins,
                s.IsNew ? string.Empty : s.Die.ToString(CultureInfo.InvariantCulture),
                s.IsNew ? string.Empty : s.Pass.ToString(CultureInfo.InvariantCulture),
                s.IsNew ? string.Empty : s.Fail.ToString(CultureInfo.InvariantCulture),
                s.IsNew ? string.Empty : files.ToString(CultureInfo.InvariantCulture), string.Empty);
            DataGridViewRow row = dgvBin.Rows[index];
            row.Tag = s;
            if (!s.InRange)
            {
                row.DefaultCellStyle.ForeColor = Color.Gray;
                row.ReadOnly = true;
            }
            PaintRow(row);
            return row;
        }

        /// <summary>상태 줄에 붙일 YMS 반영 파일 수</summary>
        private string YmsFileText()
        {
            int count = YmsFileCount();
            return count > 0 ? " / YMS 반영 파일 " + count : " / YMS 반영 파일 없음";
        }

        private int YmsFileCount()
        {
            if (programList == null) return 0;
            DataRow dr = programList.Rows.Cast<DataRow>().FirstOrDefault(r => r["DEVICE_ID"].ToString() == loadedDevice && r["TEST_PROGRAM"].ToString() == loadedProgram);
            return dr == null ? 0 : Convert.ToInt32(dr["YMS_FILE_COUNT"], CultureInfo.InvariantCulture);
        }

        #endregion

        #region [편집]

        private DataGridViewColumn[] EditColumns()
        {
            return new DataGridViewColumn[] { colBin, colName, colGood, colColor, colDesc };
        }

        /// <summary>이 칸을 고칠 수 있는지: YMS 에 넣을 수 있는 Bin 의 이름 / 양품 / 색 / 설명, Bin 번호는 새 행만.</summary>
        private bool CanEdit(DataGridViewCell cell)
        {
            BinState s = cell.OwningRow.Tag as BinState;
            if (s == null || !s.InRange) return false;
            if (cell.ColumnIndex == colBin.Index) return s.IsNew;
            return cell.ColumnIndex == colName.Index || cell.ColumnIndex == colDesc.Index || cell.ColumnIndex == colGood.Index || cell.ColumnIndex == colColor.Index;
        }

        private bool IsTextColumn(int columnIndex)
        {
            return columnIndex == colBin.Index || columnIndex == colName.Index || columnIndex == colDesc.Index;
        }

        private void dgvBin_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewCell cell = dgvBin.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!CanEdit(cell)) return;
            if (e.ColumnIndex == colColor.Index)
                PickColor(cell.OwningRow);
            else if (IsTextColumn(e.ColumnIndex))
            {
                dgvBin.CurrentCell = cell;
                dgvBin.BeginEdit(true);
            }
        }

        /// <summary>양품은 클릭할 때마다 바뀐다(열은 읽기 전용, 값은 코드에서 바꾼다).</summary>
        private void dgvBin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colGood.Index) return;
            DataGridViewCell cell = dgvBin.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (CanEdit(cell)) Toggle(cell);
        }

        /// <summary>F2 = 글자 편집 / 색 고르기(색 칸), Space = 양품 바꾸기. (Enter 는 표가 다음 행 이동으로 먼저 쓴다)</summary>
        private void dgvBin_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewCell cell = dgvBin.CurrentCell;
            if (cell == null || dgvBin.IsCurrentCellInEditMode) return;
            if (e.KeyCode == Keys.F2 && IsTextColumn(cell.ColumnIndex) && CanEdit(cell))
                dgvBin.BeginEdit(true);
            else if (e.KeyCode == Keys.Space && !e.Control && cell.ColumnIndex == colGood.Index && CanEdit(cell))
                Toggle(cell);
            else if (e.KeyCode == Keys.F2 && cell.ColumnIndex == colColor.Index && CanEdit(cell))
                PickColor(cell.OwningRow);
            else
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void dgvBin_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewCell cell = dgvBin.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string raw = Convert.ToString(cell.Value, CultureInfo.InvariantCulture);
            if (raw != raw.Trim()) cell.Value = raw.Trim();
        }

        /// <summary>칸 값이 바뀌면 수정 표시 / 상태 글자를 다시 칠한다.</summary>
        private void dgvBin_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (filling || e.RowIndex < 0 || e.ColumnIndex == colState.Index) return; // 상태 칸은 PaintRow 가 쓴다
            PaintRow(dgvBin.Rows[e.RowIndex]);
        }

        /// <summary>우클릭한 행이 선택에 없으면 그 행만 고른다.</summary>
        private void dgvBin_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0) return;
            DataGridViewRow row = dgvBin.Rows[e.RowIndex];
            if (row.Selected || (e.ColumnIndex >= 0 && row.Cells[e.ColumnIndex].Selected)) return;
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.EndEdit();
            dgvBin.ClearSelection();
            dgvBin.CurrentCell = row.Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : colBin.Index];
            row.Selected = true;
        }

        /// <summary>색 창을 지금 색에서 열고, 고른 색을 #RRGGBB 로 넣는다.</summary>
        private void PickColor(DataGridViewRow row)
        {
            Color current = ParseColor(CellText(row, colColor));
            Color picked = AskColor(current.IsEmpty ? Color.White : current);
            if (picked.IsEmpty) return;
            row.Cells[colColor.Index].Value = string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", picked.R, picked.G, picked.B);
        }

        /// <summary>칸이 조회 당시 값과 다른지. 새 행은 모든 칸이 바뀐 것으로 본다.</summary>
        private bool Changed(DataGridViewRow row, DataGridViewColumn column)
        {
            BinState s = (BinState)row.Tag;
            if (s.IsNew) return true;
            if (column == colName) return CellText(row, colName) != s.Name;
            if (column == colGood) return Checked(row, colGood) != s.Good;
            if (column == colColor) return !string.Equals(CellText(row, colColor), s.Color, StringComparison.OrdinalIgnoreCase);
            if (column == colDesc) return CellText(row, colDesc) != s.Description;
            return false;
        }

        private bool IsDirty(DataGridViewRow row)
        {
            BinState s = row.Tag as BinState;
            return s != null && s.InRange && EditColumns().Any(c => Changed(row, c));
        }

        private List<DataGridViewRow> DirtyRows()
        {
            return dgvBin.Rows.Cast<DataGridViewRow>().Where(IsDirty).ToList();
        }

        /// <summary>
        /// 수정한 행의 입력 오류(서버 SaveEtBins 검사와 같은 기준). 없으면 빈 문자열. column = 붉게 칠할 칸.
        /// 이름 30바이트 / 설명 100바이트는 DB 컬럼 길이(VARCHAR2 BYTE)라 한글은 1자 = 3바이트로 센다.
        /// </summary>
        private string RowError(DataGridViewRow row, out DataGridViewColumn column)
        {
            column = colBin;
            BinState s = (BinState)row.Tag;
            if (s.IsNew)
            {
                int bin;
                string text = CellText(row, colBin);
                if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out bin) || bin < 0 || bin > EtYmsRule.MaxBin)
                    return "Bin 은 0 ~ " + EtYmsRule.MaxBin + " 정수로 입력하세요.";
                string normalized = bin.ToString(CultureInfo.InvariantCulture);
                if (dgvBin.Rows.Cast<DataGridViewRow>().Any(r => r.Index != row.Index && BinNumber(r) == normalized))
                    return "Bin " + normalized + " 은 표에 이미 있습니다.";
            }
            column = colName;
            if (Encoding.UTF8.GetByteCount(CellText(row, colName)) > EtYmsRule.MaxBinNameLength)
                return "이름은 " + EtYmsRule.MaxBinNameLength + "바이트 이하입니다(한글 1자 = 3바이트).";
            column = colColor;
            string color = CellText(row, colColor).ToUpperInvariant();
            if (color.Length > 0 && !ColorPattern.IsMatch(color))
                return "색은 #RRGGBB 형식입니다(더블클릭으로 고르기).";
            column = colDesc;
            if (Encoding.UTF8.GetByteCount(CellText(row, colDesc)) > EtYmsRule.MaxBinDescriptionLength)
                return "설명은 " + EtYmsRule.MaxBinDescriptionLength + "바이트 이하입니다(한글 1자 = 3바이트).";
            column = null;
            return string.Empty;
        }

        /// <summary>
        /// 바뀐 칸 노란색 / 오류 칸 붉은색 / 행 머리글 '*' / 색 칸 = 그 색(바뀌면 굵은 글자) / 상태 글자.
        /// 상태는 지금 칸 값(양품 체크)으로 다시 계산한다.
        /// </summary>
        private void PaintRow(DataGridViewRow row)
        {
            BinState s = (BinState)row.Tag;
            bool dirty = false;
            if (s.InRange)
            {
                foreach (DataGridViewColumn column in EditColumns())
                {
                    bool changed = column != colBin && Changed(row, column);
                    dirty |= changed;
                    if (column != colColor)
                        row.Cells[column.Index].Style.BackColor = changed && !s.IsNew ? DirtyColor : Color.Empty;
                }
                dirty |= s.IsNew;
            }
            DataGridViewColumn errorColumn = null;
            string error = dirty ? RowError(row, out errorColumn) : string.Empty;
            PaintColor(row, !s.IsNew && Changed(row, colColor));
            if (errorColumn != null)
                row.Cells[errorColumn.Index].Style.BackColor = ErrorColor;
            row.HeaderCell.Value = dirty ? "*" : null;
            row.HeaderCell.ToolTipText = error.Length > 0 ? error : dirty ? "수정됨 (저장 전)" : string.Empty;
            row.Cells[colState.Index].Value = error.Length > 0 ? error : StateText(row);
            row.Cells[colState.Index].Style.ForeColor = error.Length > 0 ? Color.Firebrick : Color.Empty;
        }

        /// <summary>색 칸 배경을 그 색으로(선택해도 같은 색), 글자는 배경에 따라 흰색 / 검정.</summary>
        private void PaintColor(DataGridViewRow row, bool changed)
        {
            DataGridViewCell cell = row.Cells[colColor.Index];
            Color color = ParseColor(CellText(row, colColor));
            cell.Style.BackColor = color;
            cell.Style.SelectionBackColor = color;
            Color text = color.IsEmpty ? Color.Empty : (color.R * 299 + color.G * 587 + color.B * 114) / 1000 < 128 ? Color.White : Color.Black;
            cell.Style.ForeColor = text;
            cell.Style.SelectionForeColor = text;
            cell.Style.Font = changed ? new Font(dgvBin.Font, FontStyle.Bold) : null;
        }

        /// <summary>상태 글자: YMS 에 못 넣음 / 새 Bin / 정의 없음 / 파일에 없음 / 파일 결과와 다름 / 빈 값</summary>
        private string StateText(DataGridViewRow row)
        {
            BinState s = (BinState)row.Tag;
            if (!s.InRange) return "YMS 에 못 넣음";
            if (s.IsNew) return "새 Bin";
            if (!s.HasDef) return "정의 없음";
            if (s.Die == 0) return "파일에 없음";
            bool good = Checked(row, colGood);
            if ((good && s.Fail > 0) || (!good && s.Fail == 0 && s.Pass > 0)) return "파일 결과와 다름";
            return string.Empty;
        }

        #endregion

        #region [추가 / 삭제 / 저장]

        /// <summary>표 끝에 새 Bin 행을 넣고 Bin 번호 편집을 시작한다.</summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!EnsureLoaded()) return;
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.EndEdit();
            DataGridViewRow row = AddRow(new BinState { IsNew = true, InRange = true, Bin = string.Empty, Name = string.Empty, Color = string.Empty, Description = string.Empty }, string.Empty, 0);
            dgvBin.ClearSelection();
            dgvBin.CurrentCell = row.Cells[colBin.Index];
            dgvBin.BeginEdit(true);
            SetStatus("새 Bin: 번호(0 ~ " + EtYmsRule.MaxBin + ")와 이름 / 양품 / 색 / 설명을 넣고 [저장] 하세요.");
        }

        /// <summary>
        /// 고른 행 중 정의가 있는 행의 Bin 정의를 지운다(DeleteEtBins). 새 행 / 정의 없는 행은 표에서만 뺀다.
        /// 정의를 지우면 표를 다시 읽으므로 고른 행 밖에 저장 안 한 수정이 있으면 먼저 저장하라고 막는다.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.EndEdit();
            List<DataGridViewRow> rows = SelectedRows();
            if (rows.Count == 0)
            {
                Notify("지울 Bin 행을 고르세요(드래그 / Ctrl / Shift 로 여러 행).", MessageBoxIcon.Information);
                return;
            }
            if (!EnsureLoaded()) return;
            List<DataGridViewRow> defRows = rows.Where(r => ((BinState)r.Tag).HasDef && !((BinState)r.Tag).IsNew).ToList();
            List<DataGridViewRow> localRows = rows.Except(defRows).ToList();
            if (defRows.Count == 0)
            {
                foreach (DataGridViewRow row in localRows) dgvBin.Rows.Remove(row);
                SetStatus("표에서 " + localRows.Count + "행을 뺐습니다(정의가 없어 DB 는 그대로).");
                return;
            }
            int dirtyOthers = DirtyRows().Except(rows).Count();
            if (dirtyOthers > 0)
            {
                Notify("저장하지 않은 수정 " + dirtyOthers + "행이 있습니다. 먼저 [저장] 하거나 다시 조회한 뒤 지우세요.", MessageBoxIcon.Warning);
                return;
            }
            string[] bins = defRows.Select(r => ((BinState)r.Tag).Bin).ToArray();
            int inFile = defRows.Count(r => ((BinState)r.Tag).Die > 0);
            string msg = "Bin " + string.Join(", ", bins) + " 정의를 지웁니다."
                + (inFile > 0 ? "\r\n파일에 있는 Bin 은 다음 YMS 반영 때 기본값으로 다시 만들어집니다." : string.Empty);
            if (Ask(msg, MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            string[] key = { loadedFactory, loadedDevice, loadedProgram };
            string user = User();
            using (busyBar.Begin("삭제 (" + bins.Length + "개)"))
            {
                int deleted;
                try
                {
                    deleted = busyBar.Run("서버 삭제", () => Service.DeleteEtBins(key, bins, user));
                }
                catch (Exception ex)
                {
                    Notify("삭제 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                    return;
                }
                LoadBins();
                SetStatus("삭제 " + deleted + "개 / Bin " + string.Join(", ", bins) + YmsFileText());
            }
        }

        /// <summary>
        /// 바뀐 행 + 새 행만 저장한다(SaveEtBins: Bin / 이름 / 양품 / 색 / 설명).
        /// 양품이 바뀌었고 YMS 반영 파일이 있으면 수율 다시 계산을 묻고(RejudgeEtFiles = 판정 + YMS 다시 반영), 표를 다시 읽는다.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvBin.IsCurrentCellInEditMode) dgvBin.EndEdit();
            List<DataGridViewRow> rows = DirtyRows();
            if (rows.Count == 0)
            {
                Notify("저장할 수정이 없습니다. (이름 / 색 / 설명 = 더블클릭, 양품 = 클릭)", MessageBoxIcon.Information);
                return;
            }
            if (!EnsureLoaded()) return;
            List<string> errors = new List<string>();
            foreach (DataGridViewRow row in rows)
            {
                DataGridViewColumn column;
                string error = RowError(row, out column);
                if (error.Length > 0) errors.Add("Bin " + CellText(row, colBin) + ": " + error);
            }
            if (errors.Count > 0)
            {
                Notify("입력을 확인하세요(붉은 칸).\r\n\r\n" + string.Join("\r\n", errors.Take(15).ToArray()) + (errors.Count > 15 ? "\r\n..." : string.Empty), MessageBoxIcon.Warning);
                return;
            }

            string[,] data = new string[rows.Count, 5];
            for (int i = 0; i < rows.Count; i++)
            {
                data[i, 0] = BinNumber(rows[i]);
                data[i, 1] = CellText(rows[i], colName);
                data[i, 2] = Checked(rows[i], colGood) ? "Y" : "N";
                data[i, 3] = CellText(rows[i], colColor).ToUpperInvariant();
                data[i, 4] = CellText(rows[i], colDesc);
            }
            string[] key = { loadedFactory, loadedDevice, loadedProgram };
            string user = User();
            using (busyBar.Begin("저장 (" + rows.Count + "행)"))
            {
                DataRow result;
                try
                {
                    result = busyBar.Run("서버 저장", () => Service.SaveEtBins(key, data, user)).Rows[0];
                }
                catch (Exception ex)
                {
                    SetStatus("저장 실패 / " + Cause(ex).Message);
                    Notify("저장하지 못했습니다(아무것도 저장하지 않음).\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
                    return;
                }
                string recalc = string.Empty;
                int ymsFiles = Convert.ToInt32(result["YMS_FILE_COUNT"], CultureInfo.InvariantCulture);
                if (result["GOOD_CHANGED"].ToString() == "Y" && ymsFiles > 0
                    && Ask("양품 Bin 이 바뀌었습니다. YMS 수율을 다시 계산할까요? (파일 " + ymsFiles + "개)\r\n\r\n"
                        + "예 = 이 Device / 공정의 업로드 파일을 다시 판정하고 YMS 에 다시 반영합니다.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        int judged = busyBar.Run("수율 다시 계산 (" + ymsFiles + "개)", () => Service.RejudgeEtFiles(key, user));
                        recalc = " / 수율 다시 계산 " + judged + "개";
                    }
                    catch (Exception ex)
                    {
                        recalc = " / 수율 다시 계산 실패: " + Cause(ex).Message;
                    }
                }
                LoadPrograms();
                LoadBins();
                SetStatus("저장 완료 / " + result["SAVED_COUNT"] + "행 (새 Bin " + result["NEW_COUNT"] + ") / Device " + loadedDevice + " / 공정 " + loadedProgram + recalc);
            }
        }

        #endregion

        #region [공통]

        private bool EnsureLoaded()
        {
            if (loadedFactory.Length == 0 || loadedDevice.Length == 0 || loadedProgram.Length == 0)
            {
                Notify("먼저 Device / 공정을 골라 조회하세요.", MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        /// <summary>선택된 행. 셀 단위 선택이어도 그 셀의 행을 모은다.</summary>
        private List<DataGridViewRow> SelectedRows()
        {
            HashSet<int> indexes = new HashSet<int>();
            foreach (DataGridViewCell cell in dgvBin.SelectedCells) indexes.Add(cell.RowIndex);
            foreach (DataGridViewRow row in dgvBin.SelectedRows) indexes.Add(row.Index);
            return indexes.Where(i => i >= 0).OrderBy(i => i).Select(i => dgvBin.Rows[i]).ToList();
        }

        /// <summary>행의 Bin 번호: 새 행은 입력 칸(앞 0 없앰), 나머지는 조회 값</summary>
        private string BinNumber(DataGridViewRow row)
        {
            BinState s = (BinState)row.Tag;
            if (!s.IsNew) return s.Bin;
            int bin;
            string text = CellText(row, colBin);
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out bin) ? bin.ToString(CultureInfo.InvariantCulture) : text;
        }

        private static string User()
        {
            string user = DACrux.Base.GlobalVariable.UserID;
            return string.IsNullOrEmpty(user) ? Environment.UserName : user;
        }

        private static string CellText(DataGridViewRow row, DataGridViewColumn column)
        {
            return Convert.ToString(row.Cells[column.Index].Value, CultureInfo.InvariantCulture).Trim();
        }

        private static bool Checked(DataGridViewRow row, DataGridViewColumn column)
        {
            object value = row.Cells[column.Index].Value;
            return value is bool && (bool)value;
        }

        private static void Toggle(DataGridViewCell cell)
        {
            cell.Value = !(cell.Value is bool && (bool)cell.Value);
        }

        /// <summary>#RRGGBB 를 색으로. 형식이 아니면 Color.Empty</summary>
        private static Color ParseColor(string text)
        {
            if (!ColorPattern.IsMatch((text ?? string.Empty).ToUpperInvariant())) return Color.Empty;
            return Color.FromArgb(int.Parse(text.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture) | unchecked((int)0xFF000000));
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>색 창. 고르지 않으면 Color.Empty. 화면 Smoke 검사는 정해진 색을 돌려주도록 바꾼다.</summary>
        protected virtual Color AskColor(Color current)
        {
            using (ColorDialog dialog = new ColorDialog { Color = current, FullOpen = true })
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Color : Color.Empty;
        }

        /// <summary>확인창. 화면 Smoke 검사는 정해진 답을 돌려주도록 바꾼다.</summary>
        protected virtual DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, text, Text, buttons, MessageBoxIcon.Question);
        }

        /// <summary>안내창. 화면 Smoke 검사는 창을 띄우지 않고 기록만 한다.</summary>
        protected virtual void Notify(string text, MessageBoxIcon icon)
        {
            MessageBox.Show(this, text, Text, MessageBoxButtons.OK, icon);
        }

        /// <summary>Bin 행의 조회 당시 값(수정 비교용)과 파일 분포.</summary>
        private sealed class BinState
        {
            public string Bin;
            /// <summary>[Bin 추가] 로 넣은 행 (Bin 번호 입력, 저장 전)</summary>
            public bool IsNew;
            /// <summary>Bin 정의(TQP_BINDESC)가 있음</summary>
            public bool HasDef;
            /// <summary>YMS 에 넣을 수 있는 Bin (0 ~ 99)</summary>
            public bool InRange;
            public string Name;
            public bool Good;
            public string Color;
            public string Description;
            public long Die;
            public long Pass;
            public long Fail;
        }

        #endregion
    }
}
