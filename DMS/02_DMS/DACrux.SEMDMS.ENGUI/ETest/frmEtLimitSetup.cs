using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test Spec 설정 화면이다.
    /// 설계: 문서/2026-10-07_ETest_Limit_설정_설계.md 12절(Spec) / 15절(Chain 묶기 / Setup Chain 으로 찾기)
    /// 기준 (2026-10-12):
    ///  - Spec 키 = Device + 공정(TSVET / COWET / FT) + Parameter(헤더 전체, 예: Resistance_iforce=8.0mA@TSVDC_TL_FH). 고객 Spec 표에 있는 Parameter 만 정의한다.
    ///  - 정의된 항목의 '-' 만 Fail / 헤더 끝 '#번호' 는 지운다 / 대문자로 저장 / Enable / Limit 한쪽만 가능(2026-10-07 결정 유지).
    ///  - Chain 은 대표 이름 없이 묶음으로만 관리하고, Setup Chain 으로 찾으면 묶인 Chain 의 Spec 등 정보를 모두 보여 준다.
    /// 처리 흐름:
    ///  1) Factory / Device / 공정을 고르고 [조회]: [Chain 묶기] 탭(E-Test Chain + 묶음 + Setup Chain)을 먼저 읽고, [Spec] 탭에 정의된 Parameter + 묶음 / Setup Chain + 통계 + Fail 수.
    ///  2) [Spec 표 붙여넣기]: Excel 에서 복사한 고객 Spec 표(Device / Test Type / Chain name / Parameter / Spec / Unit)를 읽어 공정별로 저장한다.
    ///     표의 Device 열은 무시하고 화면 Device 로 저장한다. 해석할 수 없는 행이 하나라도 있으면 저장하지 않는다.
    ///  3) [Spec] 탭에서 Lower / Upper / Enable / 비고를 고치고 [저장]. 바뀐 행은 노란색, 잘못된 값은 붉은색. Lower / Upper 를 모두 지우면 삭제할지 묻는다.
    ///  4) [Chain 묶기] 탭: Chain 을 고르고 [선택 Chain 묶기] (바로 저장), [FH / FL 짝 자동 묶기], [선택 묶음 해제].
    ///  5) [Setup Chain 으로 찾기] 탭: GDS Setup Chain 이름 -> 같은 이름 E-Test Chain(없으면 이름_FH / _FL) + 묶인 Chain 전부 -> 모든 공정의 Spec / 측정 항목 / 값 통계.
    /// Setup Chain 목록은 E-Test Device 의 GDS 대표 Device(Sub Device 연결) Chain, 연결이 없으면 Factory 전체 GDS Chain.
    /// Fail 수 = Enable 인 Spec 기준 범위 밖 값 + '-' 값 (READY 파일). [저장] 후 다시 조회할 때 갱신된다.
    /// </summary>
    public partial class frmEtLimitSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private static readonly Color DirtyColor = Color.LightYellow;
        private static readonly Color ErrorColor = Color.MistyRose;
        private static readonly Regex EndSuffixRegex = new Regex(@"_(FH|FL)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private RO.GdsEtResult limitService;
        private string loadedFactory = string.Empty;
        private string loadedDevice = string.Empty;
        private string loadedProgram = string.Empty;
        private bool filling;

        /// <summary>E-Test Chain -> 같은 묶음의 Chain 전부(자기 포함). 묶이지 않은 Chain 은 없다.</summary>
        private Dictionary<string, List<string>> groupMembers = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        /// <summary>Setup Chain(GDS Chain) 이름 목록</summary>
        private List<string> setupChains = new List<string>();

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtLimitSetup()
        {
            InitializeComponent();
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colValues, colNotTested, colMin, colMedian, colP99, colMax, colLower, colUpper, colFail,
                colChainItems, colChainGroupCount, colLkLower, colLkUpper, colLkFail, colLkFiles, colLkValues, colLkNotTested, colLkMin, colLkMax })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colLower, colUpper, colEnable, colRemark })
                column.HeaderCell.Style.Font = new Font(dgvLimit.Font, FontStyle.Bold);
            UpdateButtons();
        }

        /// <summary>Spec 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return limitService ?? (limitService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 와 Device 목록을 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadDeviceList();
        }

        #region [조회]

        /// <summary>측정 항목이 있는 Device 를 콤보에 채운다. 직접 입력도 된다.</summary>
        private void LoadDeviceList()
        {
            string current = cboDevice.Text;
            string factory = txtFactory.Text.Trim();
            if (factory.Length == 0) return;
            try
            {
                DataTable dt = Service.GetEtDeviceList(factory);
                cboDevice.BeginUpdate();
                cboDevice.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                    cboDevice.Items.Add(dr["DEVICE_ID"].ToString());
                cboDevice.EndUpdate();
            }
            catch (Exception ex)
            {
                SetStatus("Device 목록 조회 실패(직접 입력 가능): " + Cause(ex).Message);
            }
            cboDevice.Text = current;
        }

        /// <summary>Device 의 공정(업로드 파일 + Spec 이 있는 공정)을 콤보에 채운다. 직접 입력도 된다. 고를 공정이 없으면 첫 공정을 고른다.</summary>
        private void LoadTestProgramList(string select)
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0) return;
            try
            {
                DataTable dt = Service.GetEtTestProgramList(factory, device);
                filling = true;
                cboTestProgram.BeginUpdate();
                cboTestProgram.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                    cboTestProgram.Items.Add(dr["TEST_PROGRAM"].ToString());
                cboTestProgram.EndUpdate();
                string wanted = (select ?? string.Empty).Trim().ToUpperInvariant();
                if (wanted.Length == 0 && cboTestProgram.Items.Count > 0)
                    wanted = cboTestProgram.Items[0].ToString();
                cboTestProgram.Text = wanted;
            }
            catch (Exception ex)
            {
                SetStatus("공정 목록 조회 실패(직접 입력 가능): " + Cause(ex).Message);
            }
            finally
            {
                filling = false;
            }
        }

        private void txtFactory_Leave(object sender, EventArgs e)
        {
            LoadDeviceList();
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevice.SelectedIndex < 0) return;
            LoadTestProgramList(null);
            Search();
        }

        private void cboTestProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!filling && cboTestProgram.SelectedIndex >= 0) Search();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>저장 안 된 Spec 변경이 있으면 묻고 Chain / Spec 목록을 다시 조회한다.</summary>
        private void Search()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            string program = cboTestProgram.Text.Trim().ToUpperInvariant();
            if (factory.Length == 0 || device.Length == 0 || program.Length == 0)
            {
                Notify("Factory / Device / 공정을 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            if (DirtyRows().Count > 0
                && Ask("저장하지 않은 변경이 있습니다. 버리고 다시 조회할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            LoadAll(factory, device, program);
        }

        /// <summary>Chain 묶음을 먼저 읽고(Spec 탭의 묶음 / Setup Chain 열이 쓴다) Spec 을 읽는다.</summary>
        private void LoadAll(string factory, string device, string program)
        {
            if (!string.Equals(device, loadedDevice, StringComparison.Ordinal) || !string.Equals(factory, loadedFactory, StringComparison.Ordinal))
                ClearLookup();
            LoadChains(factory, device);
            LoadLimits(factory, device, program);
        }

        /// <summary>정의된 Parameter 의 Spec / 묶음 / Setup Chain / 통계를 표에 채운다. 각 행 Tag 에 조회 당시 값(원본)을 넣어 변경 여부를 비교한다.</summary>
        private void LoadLimits(string factory, string device, string program)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = Service.GetEtLimitList(factory, device, program);
                filling = true;
                dgvLimit.Rows.Clear();
                int notInData = 0, failItems = 0, noSetup = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string lower = Number(dr["LOWER_LIMIT"], "R");
                    string upper = Number(dr["UPPER_LIMIT"], "R");
                    bool enable = dr["ENABLE_FLAG"].ToString() == "Y";
                    string remark = dr["REMARK"].ToString();
                    bool inData = dr["IN_DATA"].ToString() == "Y";
                    string fail = Number(dr["FAIL_COUNT"], "N0");
                    int index = dgvLimit.Rows.Add(dr["ITEM_NAME"].ToString(), string.Empty, string.Empty, dr["UNIT"].ToString(), inData ? "Y" : "N",
                        Number(dr["VALUE_COUNT"], "N0"), Number(dr["NOT_TESTED_COUNT"], "N0"),
                        Number(dr["MIN_VALUE"], "G6"), Number(dr["MEDIAN_VALUE"], "G6"), Number(dr["P99_VALUE"], "G6"), Number(dr["MAX_VALUE"], "G6"),
                        lower, upper, enable, fail, dr["SPEC_TEXT"].ToString(), remark,
                        dr["UPDATE_USER"] + " " + FormatDate(dr["UPDATE_TIME"]));
                    DataGridViewRow row = dgvLimit.Rows[index];
                    row.Tag = new LimitState
                    {
                        Lower = lower, Upper = upper, Enable = enable, Remark = remark, Unit = dr["UNIT"].ToString(), SpecText = dr["SPEC_TEXT"].ToString(),
                        Chain1 = dr["CHAIN_NAME"].ToString(), Chain2 = dr["CHAIN_NAME2"].ToString()
                    };
                    if (!inData)
                    {
                        notInData++;
                        row.Cells[colItem.Index].Style.ForeColor = Color.Gray;
                        row.Cells[colItem.Index].ToolTipText = "이 공정의 업로드 파일(READY)에 없는 Parameter 입니다(헤더 이름 확인 또는 아직 업로드 전).";
                    }
                    if (fail != "0" && fail.Length > 0)
                    {
                        failItems++;
                        row.Cells[colFail.Index].Style.ForeColor = Color.Firebrick;
                    }
                }
                FillSpecChainColumns();
                foreach (DataGridViewRow row in dgvLimit.Rows)
                    if (CellText(row, colSetup).Length == 0) noSetup++;
                loadedFactory = factory;
                loadedDevice = device;
                loadedProgram = program;
                SetStatus(string.Format("조회 완료 / Factory {0} / Device {1} / 공정 {2} / Spec {3} / 데이터 없음 {4} / Fail 있는 Parameter {5} / Setup Chain 없음 {6}",
                    factory, device, program, dgvLimit.Rows.Count, notInData, failItems, noSetup));
            }
            catch (Exception ex)
            {
                SetStatus("조회 실패 / " + Cause(ex).Message);
                Notify("Spec 을 조회하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                filling = false;
                Cursor = Cursors.Default;
                UpdateButtons();
            }
        }

        /// <summary>
        /// Spec 탭의 묶음 / Setup Chain 열을 Chain 묶음 목록으로 다시 채운다(Spec 을 다시 읽지 않는다).
        ///  - 묶음: Chain 이 묶여 있으면 "A + B", Chain 이 2개인 항목(Leakage_Serial)은 "Chain1 쪽 : Chain2 쪽". 둘 다 안 묶였으면 빈 값.
        ///  - Setup Chain: 항목 Chain(과 묶인 Chain)에 연결되는 GDS Setup Chain 이름.
        /// </summary>
        private void FillSpecChainColumns()
        {
            bool old = filling;
            filling = true;
            foreach (DataGridViewRow row in dgvLimit.Rows)
            {
                LimitState s = row.Tag as LimitState;
                if (s == null) continue;
                List<string> chains = new List<string> { s.Chain1 };
                if (s.Chain2.Length > 0) chains.Add(s.Chain2);
                bool anyGroup = chains.Any(c => groupMembers.ContainsKey(c));
                row.Cells[colGroup.Index].Value = anyGroup ? string.Join(" : ", chains.Select(c => GroupText(c)).ToArray()) : string.Empty;
                row.Cells[colSetup.Index].Value = string.Join(", ", chains.SelectMany(c => SetupChainsFor(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
            }
            filling = old;
        }

        /// <summary>묶음 표시: 묶였으면 "A + B", 아니면 Chain 이름 그대로.</summary>
        private string GroupText(string chain)
        {
            List<string> members;
            return groupMembers.TryGetValue(chain, out members) ? string.Join(" + ", members.ToArray()) : chain;
        }

        /// <summary>
        /// E-Test Chain 에 연결되는 Setup Chain: Chain(묶였으면 묶음 Chain 전부) 중 하나가 Setup Chain 이름과 같거나 Setup Chain 이름 + _FH / _FL 이면 연결.
        /// 서버 GetEtChainMatch 와 같은 규칙이다(같은 이름이 있으면 _FH / _FL 은 보지 않는 우선순위는 화면 표시에서는 생략).
        /// </summary>
        private List<string> SetupChainsFor(string chain)
        {
            List<string> members;
            if (!groupMembers.TryGetValue(chain, out members)) members = new List<string> { chain };
            return setupChains.Where(s => members.Any(m => string.Equals(m, s, StringComparison.OrdinalIgnoreCase)
                || string.Equals(m, s + "_FH", StringComparison.OrdinalIgnoreCase)
                || string.Equals(m, s + "_FL", StringComparison.OrdinalIgnoreCase))).ToList();
        }

        #endregion

        #region [Spec 편집 표시]

        /// <summary>체크박스는 클릭 즉시 값이 반영되도록 바로 확정한다.</summary>
        private void dgvLimit_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLimit.IsCurrentCellDirty && dgvLimit.CurrentCell is DataGridViewCheckBoxCell)
                dgvLimit.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvLimit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (filling || e.RowIndex < 0) return;
            PaintRow(dgvLimit.Rows[e.RowIndex]);
            UpdateButtons();
        }

        /// <summary>바뀐 행은 노란색, 잘못된 값은 붉은색.</summary>
        private void PaintRow(DataGridViewRow row)
        {
            string error = RowError(row);
            Color back = error.Length > 0 ? ErrorColor : IsDirty(row) ? DirtyColor : Color.Empty;
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colLower, colUpper, colEnable, colRemark })
                row.Cells[column.Index].Style.BackColor = back;
            row.Cells[colLower.Index].ToolTipText = error;
        }

        private bool IsDirty(DataGridViewRow row)
        {
            LimitState s = (LimitState)row.Tag;
            return CellText(row, colLower) != s.Lower || CellText(row, colUpper) != s.Upper
                || CellChecked(row) != s.Enable || CellText(row, colRemark) != s.Remark;
        }

        /// <summary>행 입력 오류 문구. 없으면 빈 문자열. (두 칸 모두 빈 값은 "삭제" 라 오류가 아니다.)</summary>
        private string RowError(DataGridViewRow row)
        {
            string lowerText = CellText(row, colLower);
            string upperText = CellText(row, colUpper);
            double lower = 0, upper = 0;
            if (lowerText.Length > 0 && !TryNumber(lowerText, out lower)) return "Lower 가 숫자가 아닙니다.";
            if (upperText.Length > 0 && !TryNumber(upperText, out upper)) return "Upper 가 숫자가 아닙니다.";
            if (lowerText.Length > 0 && upperText.Length > 0 && lower > upper) return "Lower 가 Upper 보다 큽니다.";
            return string.Empty;
        }

        private List<DataGridViewRow> DirtyRows()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvLimit.Rows)
                if (row.Tag is LimitState && IsDirty(row)) rows.Add(row);
            return rows;
        }

        #endregion

        #region [Spec 저장 / 삭제 / 붙여넣기]

        /// <summary>
        /// 바뀐 행만 저장한다.
        ///  - Lower / Upper 가 하나라도 있는 행: SaveEtLimit (MERGE)
        ///  - Lower / Upper 를 모두 지운 행: 삭제할지 묻고 DeleteEtLimit
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvLimit.IsCurrentCellInEditMode) dgvLimit.EndEdit();
            List<DataGridViewRow> dirty = DirtyRows();
            if (dirty.Count == 0)
            {
                Notify("저장할 Spec 변경이 없습니다.", MessageBoxIcon.Information);
                return;
            }
            List<string> errors = new List<string>();
            List<DataGridViewRow> saves = new List<DataGridViewRow>();
            List<DataGridViewRow> deletes = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dirty)
            {
                string error = RowError(row);
                if (error.Length > 0) { errors.Add(CellText(row, colItem) + ": " + error); continue; }
                if (CellText(row, colLower).Length == 0 && CellText(row, colUpper).Length == 0) deletes.Add(row);
                else saves.Add(row);
            }
            if (errors.Count > 0)
            {
                Notify("입력을 확인하세요.\r\n\r\n" + string.Join("\r\n", errors.ToArray()), MessageBoxIcon.Warning);
                return;
            }
            if (deletes.Count > 0
                && Ask("Lower / Upper 를 모두 지운 Parameter " + deletes.Count + "개는 Spec 이 삭제됩니다(판정 안 함).\r\n\r\n"
                    + string.Join("\r\n", deletes.Take(10).Select(r => CellText(r, colItem)).ToArray()) + (deletes.Count > 10 ? "\r\n..." : string.Empty)
                    + "\r\n\r\n계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string user = User();
            try
            {
                Cursor = Cursors.WaitCursor;
                if (saves.Count > 0)
                {
                    string[,] rows = new string[saves.Count, 7];
                    for (int i = 0; i < saves.Count; i++)
                    {
                        LimitState s = (LimitState)saves[i].Tag;
                        rows[i, 0] = CellText(saves[i], colItem);
                        rows[i, 1] = CellText(saves[i], colLower);
                        rows[i, 2] = CellText(saves[i], colUpper);
                        rows[i, 3] = CellChecked(saves[i]) ? "Y" : "N";
                        rows[i, 4] = s.Unit;
                        rows[i, 5] = s.SpecText;
                        rows[i, 6] = CellText(saves[i], colRemark);
                    }
                    Service.SaveEtLimit(new string[] { loadedFactory, loadedDevice, loadedProgram }, rows, user);
                }
                foreach (DataGridViewRow row in deletes)
                    Service.DeleteEtLimit(new string[] { loadedFactory, loadedDevice, loadedProgram, CellText(row, colItem) }, user);
                LoadLimits(loadedFactory, loadedDevice, loadedProgram);
                SetStatus(string.Format("저장 완료 / 저장 {0} / 삭제 {1} / Device {2} / 공정 {3}", saves.Count, deletes.Count, loadedDevice, loadedProgram));
            }
            catch (Exception ex)
            {
                SetStatus("저장 실패 / " + Cause(ex).Message);
                Notify("Spec 을 저장하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>고른 Parameter(여러 행 가능)의 Spec 을 삭제한다.</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rows = SelectedRows(dgvLimit);
            if (rows.Count == 0)
            {
                Notify("삭제할 Parameter 를 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            if (Ask("Parameter " + rows.Count + "개의 Spec 을 삭제할까요? (판정 안 함으로 돌아갑니다)\r\n\r\n"
                + string.Join("\r\n", rows.Take(10).Select(r => CellText(r, colItem)).ToArray()) + (rows.Count > 10 ? "\r\n..." : string.Empty)
                + (DirtyRows().Count > 0 ? "\r\n\r\n저장하지 않은 다른 변경은 버려집니다." : string.Empty), MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                string user = User();
                foreach (DataGridViewRow row in rows)
                    Service.DeleteEtLimit(new string[] { loadedFactory, loadedDevice, loadedProgram, CellText(row, colItem) }, user);
                LoadLimits(loadedFactory, loadedDevice, loadedProgram);
                SetStatus("삭제 완료 / Parameter " + rows.Count + "개 / 공정 " + loadedProgram);
            }
            catch (Exception ex)
            {
                Notify("삭제하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 고객 Spec 표(클립보드)를 읽어 공정별로 저장한다.
        ///  1) EtSpecTable.Parse: 해석할 수 없는 행이 있으면 목록을 보여 주고 멈춘다.
        ///  2) 공정별 건수 / 대상 Device 를 묻는다(표 Device 열은 무시).
        ///  3) 공정마다 SaveEtLimit(MERGE, Enable = Y). 이미 있는 Parameter 는 Lower / Upper / 단위 / Spec 원문을 덮어쓰고 비고는 지운다.
        ///  4) 공정 목록을 다시 읽고, 지금 공정이 없으면 표의 첫 공정을 고른다.
        /// </summary>
        private void btnPaste_Click(object sender, EventArgs e)
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("Factory / Device 를 먼저 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            string text = ClipboardText();
            List<EtSpecRow> rows = EtSpecTable.Parse(text);
            if (rows.Count == 0)
            {
                Notify("클립보드에 Spec 표가 없습니다. Excel 에서 Device / Test Type / Chain name / Parameter / Spec / Unit 열을 복사한 뒤 누르세요.", MessageBoxIcon.Warning);
                return;
            }
            List<EtSpecRow> bad = rows.Where(r => r.Error != null).ToList();
            if (bad.Count > 0)
            {
                Notify("해석할 수 없는 행이 " + bad.Count + "개 있어 저장하지 않았습니다.\r\n\r\n"
                    + string.Join("\r\n", bad.Take(15).Select(r => r.LineNo + "줄: " + r.Error + " / " + r.SpecText).ToArray())
                    + (bad.Count > 15 ? "\r\n..." : string.Empty), MessageBoxIcon.Warning);
                SetStatus("Spec 표 확인 필요 / 오류 행 " + bad.Count);
                return;
            }
            List<IGrouping<string, EtSpecRow>> groups = rows.GroupBy(r => r.TestProgram).OrderBy(g => g.Key, StringComparer.Ordinal).ToList();
            string summary = string.Join(" / ", groups.Select(g => g.Key + " " + g.Count()).ToArray());
            string tableDevices = string.Join(", ", rows.Select(r => r.Device).Where(d => d.Length > 0).Distinct().ToArray());
            if (Ask("Spec " + rows.Count + "건을 저장합니다.\r\n\r\n공정별: " + summary + "\r\nDevice: " + device
                + (tableDevices.Length > 0 && !string.Equals(tableDevices, device, StringComparison.OrdinalIgnoreCase) ? " (표의 Device " + tableDevices + " 대신)" : string.Empty)
                + "\r\n\r\n이미 있는 Parameter 는 Lower / Upper / 단위 / Spec 원문을 덮어쓰고 Enable 을 켭니다. 계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            if (DirtyRows().Count > 0
                && Ask("저장하지 않은 Spec 변경은 버려집니다. 계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string user = User();
            try
            {
                Cursor = Cursors.WaitCursor;
                foreach (IGrouping<string, EtSpecRow> group in groups)
                {
                    List<EtSpecRow> list = group.ToList();
                    string[,] data = new string[list.Count, 7];
                    for (int i = 0; i < list.Count; i++)
                    {
                        data[i, 0] = list[i].ItemName;
                        data[i, 1] = list[i].Lower;
                        data[i, 2] = list[i].Upper;
                        data[i, 3] = "Y";
                        data[i, 4] = list[i].Unit;
                        data[i, 5] = list[i].SpecText;
                        data[i, 6] = string.Empty;
                    }
                    Service.SaveEtLimit(new string[] { factory, device, group.Key }, data, user);
                }
                string program = cboTestProgram.Text.Trim().ToUpperInvariant();
                if (program.Length == 0 || !groups.Any(g => g.Key == program))
                    program = groups[0].Key;
                LoadTestProgramList(program);
                LoadAll(factory, device, program);
                SetStatus("Spec 표 저장 완료 / " + summary + " / Device " + device + " / 지금 공정 " + program);
            }
            catch (Exception ex)
            {
                SetStatus("Spec 표 저장 실패 / " + Cause(ex).Message);
                Notify("Spec 표를 저장하지 못했습니다. 앞 공정은 저장됐을 수 있습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region [Chain 묶기]

        /// <summary>
        /// E-Test Chain 목록(측정 항목에서 추출) + 묶음을 채우고, Setup Chain 목록(찾기 탭 / Setup Chain 열)을 읽는다.
        /// groupMembers / setupChains 를 다시 만든다.
        /// </summary>
        private void LoadChains(string factory, string device)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string source;
                setupChains = LoadSetupChains(factory, device, out source) ?? new List<string>();
                DataTable dt = Service.GetEtChainGroupList(factory, device);
                groupMembers = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                foreach (IGrouping<string, DataRow> g in dt.Rows.Cast<DataRow>().Where(r => r["GROUP_SEQ"] != DBNull.Value).GroupBy(r => r["GROUP_SEQ"].ToString()))
                {
                    List<string> members = g.Select(r => r["CHAIN_NAME"].ToString()).OrderBy(x => x, StringComparer.Ordinal).ToList();
                    foreach (string m in members) groupMembers[m] = members;
                }

                filling = true;
                dgvChain.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string chain = dr["CHAIN_NAME"].ToString();
                    bool grouped = dr["GROUP_SEQ"] != DBNull.Value;
                    int index = dgvChain.Rows.Add(chain, Number(dr["ITEM_COUNT"], "N0"), grouped ? dr["GROUP_CHAINS"].ToString() : string.Empty,
                        grouped ? Number(dr["GROUP_COUNT"], "N0") : string.Empty, string.Join(", ", SetupChainsFor(chain).ToArray()), dr["REMARK"].ToString(),
                        grouped ? dr["UPDATE_USER"] + " " + FormatDate(dr["UPDATE_TIME"]) : string.Empty);
                    dgvChain.Rows[index].Tag = grouped ? dr["GROUP_SEQ"].ToString() : string.Empty;
                }
                ApplyChainFilter();
                int groupedChains = groupMembers.Count;
                int groups = groupMembers.Values.Distinct().Count();
                int linked = dgvChain.Rows.Cast<DataGridViewRow>().Count(r => CellText(r, colChainSetup).Length > 0);
                lblChainInfo.Text = string.Format("Chain {0} / 묶인 Chain {1} (묶음 {2}) / Setup Chain 연결 {3} / Setup Chain 목록: {4}",
                    dgvChain.Rows.Count, groupedChains, groups, linked, source);

                // [Setup Chain 으로 찾기] 탭 목록
                string current = cboSetupChain.Text;
                lstSetupChain.BeginUpdate();
                lstSetupChain.Items.Clear();
                cboSetupChain.Items.Clear();
                foreach (string name in setupChains)
                {
                    lstSetupChain.Items.Add(name);
                    cboSetupChain.Items.Add(name);
                }
                lstSetupChain.EndUpdate();
                cboSetupChain.Text = current;
            }
            catch (Exception ex)
            {
                lblChainInfo.Text = "Chain 조회 실패 / " + Cause(ex).Message;
            }
            finally
            {
                filling = false;
                Cursor = Cursors.Default;
                UpdateButtons();
            }
        }

        private void txtChainFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyChainFilter();
        }

        /// <summary>Chain 이름 / 묶음 / Setup Chain 에 필터 글자가 들어간 행만 보인다(대소문자 무시).</summary>
        private void ApplyChainFilter()
        {
            string filter = txtChainFilter.Text.Trim();
            dgvChain.CurrentCell = null;
            foreach (DataGridViewRow row in dgvChain.Rows)
                row.Visible = filter.Length == 0
                    || CellText(row, colChainName).IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
                    || CellText(row, colChainGroup).IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
                    || CellText(row, colChainSetup).IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void dgvChain_SelectionChanged(object sender, EventArgs e)
        {
            if (filling) return;
            List<DataGridViewRow> rows = SelectedRows(dgvChain);
            lblChainSelect.Text = "선택 " + rows.Count + "개" + (rows.Count > 0 && rows.Count <= 4 ? ": " + string.Join(", ", rows.Select(r => CellText(r, colChainName)).ToArray()) : string.Empty);
            UpdateButtons();
        }

        /// <summary>
        /// 고른 Chain 을 한 묶음으로 바로 저장한다. 대표 Chain 이름 없이 묶음 번호만 둔다 (2026-10-12).
        /// 이미 다른 묶음에 있는 Chain 은 새 묶음으로 옮긴다(1개만 남는 이전 묶음은 서버가 지운다). 2개가 아니면 묻는다.
        /// </summary>
        private void btnChainGroup_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rows = SelectedRows(dgvChain);
            if (rows.Count < 2)
            {
                Notify("묶을 Chain 을 2개 이상 고르세요(Ctrl / Shift).", MessageBoxIcon.Warning);
                return;
            }
            List<string> names = rows.Select(r => CellText(r, colChainName)).ToList();
            List<DataGridViewRow> moving = rows.Where(r => Convert.ToString(r.Tag).Length > 0).ToList();
            if (moving.Count > 0
                && Ask("이미 묶인 Chain " + moving.Count + "개는 기존 묶음에서 빠져 새 묶음으로 옮겨집니다(기존 묶음에 1개만 남으면 그 묶음은 풀립니다).\r\n\r\n"
                    + string.Join("\r\n", moving.Select(r => CellText(r, colChainName) + " (" + CellText(r, colChainGroup) + ")").ToArray())
                    + "\r\n\r\n계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            if (rows.Count != 2
                && Ask("보통 2개(_FH / _FL)를 묶습니다. Chain " + rows.Count + "개를 한 묶음으로 할까요?\r\n\r\n" + string.Join("\r\n", names.ToArray()), MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            string[,] data = new string[names.Count, 3];
            for (int i = 0; i < names.Count; i++)
            {
                data[i, 0] = "1";
                data[i, 1] = names[i];
                data[i, 2] = "수동 묶음";
            }
            SaveGroups(data, "묶음 완료 / Chain " + names.Count + "개: " + string.Join(" + ", names.ToArray()));
        }

        /// <summary>
        /// 아직 묶이지 않은 Chain 중 이름_FH 와 이름_FL 이 모두 있는 짝을 찾아 한 번에 묶는다(묻고 저장).
        /// </summary>
        private void btnChainAuto_Click(object sender, EventArgs e)
        {
            HashSet<string> all = new HashSet<string>(dgvChain.Rows.Cast<DataGridViewRow>().Select(r => CellText(r, colChainName)), StringComparer.Ordinal);
            List<string[]> pairs = new List<string[]>();
            foreach (string name in all.OrderBy(x => x, StringComparer.Ordinal))
            {
                if (!name.EndsWith("_FH", StringComparison.Ordinal)) continue;
                string baseName = name.Substring(0, name.Length - 3);
                string partner = baseName + "_FL";
                if (all.Contains(partner) && !groupMembers.ContainsKey(name) && !groupMembers.ContainsKey(partner))
                    pairs.Add(new[] { baseName, name, partner });
            }
            if (pairs.Count == 0)
            {
                Notify("묶을 FH / FL 짝이 없습니다(이미 묶였거나 짝이 없음).", MessageBoxIcon.Information);
                return;
            }
            if (Ask("아직 묶이지 않은 FH / FL 짝 " + pairs.Count + "쌍을 묶을까요?\r\n\r\n"
                + string.Join("\r\n", pairs.Take(10).Select(p => p[1] + " + " + p[2]).ToArray()) + (pairs.Count > 10 ? "\r\n..." : string.Empty), MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            string[,] data = new string[pairs.Count * 2, 3];
            for (int i = 0; i < pairs.Count; i++)
            {
                data[i * 2, 0] = pairs[i][0]; data[i * 2, 1] = pairs[i][1]; data[i * 2, 2] = "FH-FL 자동 묶음";
                data[i * 2 + 1, 0] = pairs[i][0]; data[i * 2 + 1, 1] = pairs[i][2]; data[i * 2 + 1, 2] = "FH-FL 자동 묶음";
            }
            SaveGroups(data, "FH / FL 자동 묶음 완료 / " + pairs.Count + "쌍");
        }

        /// <summary>CreateEtChainGroups 호출 후 Chain 목록과 Spec 탭 열을 다시 채운다.</summary>
        private void SaveGroups(string[,] data, string done)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Service.CreateEtChainGroups(new string[] { loadedFactory, loadedDevice }, data, User());
                LoadChains(loadedFactory, loadedDevice);
                FillSpecChainColumns();
                SetStatus(done);
            }
            catch (Exception ex)
            {
                SetStatus("묶음 저장 실패 / " + Cause(ex).Message);
                Notify("Chain 을 묶지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>고른 Chain 이 들어 있는 묶음을 푼다(묶음 전체).</summary>
        private void btnChainUngroup_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rows = SelectedRows(dgvChain).Where(r => Convert.ToString(r.Tag).Length > 0).ToList();
            List<DataGridViewRow> perGroup = rows.GroupBy(r => Convert.ToString(r.Tag)).Select(g => g.First()).ToList();
            if (perGroup.Count == 0)
            {
                Notify("묶인 Chain 을 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            if (Ask("묶음 " + perGroup.Count + "개를 풀까요?\r\n\r\n" + string.Join("\r\n", perGroup.Select(r => CellText(r, colChainGroup)).ToArray()), MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                Cursor = Cursors.WaitCursor;
                string user = User();
                foreach (DataGridViewRow row in perGroup)
                    Service.DeleteEtChainGroup(new string[] { loadedFactory, loadedDevice, CellText(row, colChainName) }, user);
                LoadChains(loadedFactory, loadedDevice);
                FillSpecChainColumns();
                SetStatus("묶음 해제 완료 / " + perGroup.Count + "개");
            }
            catch (Exception ex)
            {
                SetStatus("묶음 해제 실패 / " + Cause(ex).Message);
                Notify("묶음을 풀지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region [Setup Chain 으로 찾기]

        private void lstSetupChain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling || lstSetupChain.SelectedItem == null) return;
            cboSetupChain.Text = lstSetupChain.SelectedItem.ToString();
            Lookup();
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            Lookup();
        }

        private void cboSetupChain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            Lookup();
        }

        /// <summary>
        /// Setup Chain 이름으로 찾는다. 묶인 Chain 까지 넓혀 Limit 등 정보를 모두 보여 준다 (2026-10-12).
        ///  1) GetEtChainMatch: 같은 이름 E-Test Chain(없으면 이름_FH / _FL) + 묶인 Chain
        ///  2) GetEtChainSpecInfo: 그 Chain 들이 들어간 모든 공정의 Spec + Spec 없는 측정 항목 + 값 통계 / Fail 수
        /// </summary>
        private void Lookup()
        {
            string name = cboSetupChain.Text.Trim();
            if (loadedDevice.Length == 0)
            {
                Notify("먼저 Device / 공정을 고르고 [조회] 하세요.", MessageBoxIcon.Warning);
                return;
            }
            if (name.Length == 0)
            {
                Notify("Setup Chain 이름을 고르거나 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable match = Service.GetEtChainMatch(loadedFactory, loadedDevice, name);
                DataTable info = Service.GetEtChainSpecInfo(loadedFactory, loadedDevice, name);
                dgvLookup.Rows.Clear();
                foreach (DataRow dr in info.Rows)
                {
                    bool defined = dr["DEFINED"].ToString() == "Y";
                    string fail = defined ? Number(dr["FAIL_COUNT"], "N0") : string.Empty;
                    int index = dgvLookup.Rows.Add(dr["TEST_PROGRAM"].ToString(), dr["ITEM_NAME"].ToString(), dr["MATCH_CHAIN"].ToString(), defined ? "Y" : "N",
                        Number(dr["LOWER_LIMIT"], "R"), Number(dr["UPPER_LIMIT"], "R"), dr["UNIT"].ToString(), dr["ENABLE_FLAG"].ToString(), fail,
                        Number(dr["FILE_COUNT"], "N0"), Number(dr["VALUE_COUNT"], "N0"), Number(dr["NOT_TESTED_COUNT"], "N0"),
                        Number(dr["MIN_VALUE"], "G6"), Number(dr["MAX_VALUE"], "G6"), dr["SPEC_TEXT"].ToString());
                    DataGridViewRow row = dgvLookup.Rows[index];
                    if (!defined) row.DefaultCellStyle.ForeColor = Color.Gray;
                    if (fail.Length > 0 && fail != "0") row.Cells[colLkFail.Index].Style.ForeColor = Color.Firebrick;
                }
                if (match.Rows.Count == 0)
                {
                    lblLookupInfo.Text = "Setup Chain " + name + " -> 찾은 E-Test Chain 없음 (같은 이름 / " + name + "_FH / " + name + "_FL 모두 없음)";
                }
                else
                {
                    string chains = string.Join(", ", match.Rows.Cast<DataRow>().Select(r => r["CHAIN_NAME"] + " (" + MatchText(r["MATCH_TYPE"].ToString()) + ")").ToArray());
                    int defined = info.Rows.Cast<DataRow>().Count(r => r["DEFINED"].ToString() == "Y");
                    string programs = string.Join(", ", info.Rows.Cast<DataRow>().Where(r => r["DEFINED"].ToString() == "Y")
                        .Select(r => r["TEST_PROGRAM"].ToString()).Distinct().OrderBy(x => x, StringComparer.Ordinal).ToArray());
                    long failSum = info.Rows.Cast<DataRow>().Where(r => r["DEFINED"].ToString() == "Y").Sum(r => Convert.ToInt64(r["FAIL_COUNT"], CultureInfo.InvariantCulture));
                    lblLookupInfo.Text = "Setup Chain " + name + " -> E-Test Chain: " + chains + "\r\nSpec " + defined + "건" + (programs.Length > 0 ? " (" + programs + ")" : string.Empty)
                        + " / Spec 없는 측정 항목 " + (info.Rows.Count - defined) + "건 / Fail 합 " + failSum.ToString("N0", CultureInfo.InvariantCulture);
                }
                SetStatus("찾기 완료 / Setup Chain " + name + " / E-Test Chain " + match.Rows.Count + " / 정보 " + info.Rows.Count + "행");
            }
            catch (Exception ex)
            {
                SetStatus("찾기 실패 / " + Cause(ex).Message);
                Notify("Setup Chain 으로 찾지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string MatchText(string type)
        {
            switch (type)
            {
                case "EXACT": return "같은 이름";
                case "SUFFIX": return "이름 + _FH / _FL";
                case "GROUP": return "묶음";
                default: return type;
            }
        }

        /// <summary>Spec 이 있는 행을 더블클릭하면 [Spec] 탭에서 그 공정 / Parameter 로 이동한다.</summary>
        private void dgvLookup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvLookup.Rows[e.RowIndex];
            string program = CellText(row, colLkProgram);
            string item = CellText(row, colLkItem).ToUpperInvariant();
            if (program.Length == 0) return;
            if (program != loadedProgram)
            {
                if (DirtyRows().Count > 0 && Ask("저장하지 않은 Spec 변경은 버려집니다. 공정 " + program + " 으로 이동할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                filling = true;
                cboTestProgram.Text = program;
                filling = false;
                LoadLimits(loadedFactory, loadedDevice, program);
            }
            tabMain.SelectedTab = tabSpec;
            foreach (DataGridViewRow r in dgvLimit.Rows)
            {
                if (CellText(r, colItem) != item) continue;
                dgvLimit.ClearSelection();
                dgvLimit.CurrentCell = r.Cells[colItem.Index];
                r.Selected = true;
                break;
            }
        }

        private void ClearLookup()
        {
            dgvLookup.Rows.Clear();
            lblLookupInfo.Text = "Setup Chain 을 고르면 같은 이름 E-Test Chain(없으면 이름_FH / _FL)과 묶인 Chain 의 Spec / 측정 항목을 모두 보여 줍니다.";
        }

        #endregion

        #region [공통]

        /// <summary>조회 당시 Spec 값(변경 비교용) + 저장 때 그대로 넘길 단위 / Spec 원문 + 항목 Chain</summary>
        private sealed class LimitState
        {
            public string Lower;
            public string Upper;
            public bool Enable;
            public string Remark;
            public string Unit;
            public string SpecText;
            public string Chain1;
            public string Chain2;
        }

        private void UpdateButtons()
        {
            bool loaded = loadedDevice.Length > 0;
            btnSave.Enabled = dgvLimit.Rows.Count > 0;
            btnDelete.Enabled = dgvLimit.Rows.Count > 0;
            btnChainGroup.Enabled = loaded && dgvChain.Rows.Count > 0;
            btnChainAuto.Enabled = loaded && dgvChain.Rows.Count > 0;
            btnChainUngroup.Enabled = loaded && dgvChain.Rows.Count > 0;
            btnLookup.Enabled = loaded;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        /// <summary>선택된 행(보이는 행만). 셀 단위 선택이어도 그 셀의 행을 모은다.</summary>
        private static List<DataGridViewRow> SelectedRows(DataGridView grid)
        {
            HashSet<int> indexes = new HashSet<int>();
            foreach (DataGridViewCell cell in grid.SelectedCells) indexes.Add(cell.RowIndex);
            foreach (DataGridViewRow row in grid.SelectedRows) indexes.Add(row.Index);
            return indexes.Where(i => i >= 0).OrderBy(i => i).Select(i => grid.Rows[i]).Where(r => r.Visible).ToList();
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

        private bool CellChecked(DataGridViewRow row)
        {
            object value = row.Cells[colEnable.Index].Value;
            return value is bool && (bool)value;
        }

        private static bool TryNumber(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>DB 숫자(NULL 이면 빈 문자열)를 형식에 맞춰 문자열로 바꾼다.</summary>
        private static string Number(object value, string format)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            double number = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            return number.ToString(format, CultureInfo.InvariantCulture);
        }

        private static string FormatDate(object value)
        {
            return value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value);
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>
        /// Setup Chain 목록. E-Test Device(파일명 Device, 예: OMM) -> GDS 대표 Device(예: U9)는 GdsMapView.GetMapDeviceBySub(Sub 등록 -> 같은 이름 Map) 1행.
        /// 연결이 없거나 서비스가 아직 배포되지 않았으면 Factory 전체 GDS Chain. source 에 어느 쪽인지 적는다. 화면 Smoke 검사는 정해진 목록을 돌려주도록 바꾼다.
        /// </summary>
        protected virtual List<string> LoadSetupChains(string factory, string device, out string source)
        {
            string gdsDevice = string.Empty;
            try
            {
                DataTable map = new RO.GdsMapView().GetMapDeviceBySub(factory, device);
                if (map.Rows.Count > 0) gdsDevice = map.Rows[0]["DEVICE_ID"].ToString();
            }
            catch (Exception)
            {
                gdsDevice = string.Empty;
            }
            DataTable gds = Service.GetEtGdsChainList(factory);
            List<string> names = gds.Rows.Cast<DataRow>()
                .Where(r => gdsDevice.Length == 0 || string.Equals(r["DEVICE_ID"].ToString(), gdsDevice, StringComparison.OrdinalIgnoreCase))
                .Select(r => r["CHAIN_NAME"].ToString()).Distinct(StringComparer.Ordinal).ToList();
            source = gdsDevice.Length > 0
                ? "GDS Device " + gdsDevice + " 의 Setup Chain " + names.Count + "개"
                : "GDS Device 연결 없음 - Factory 전체 GDS Chain " + names.Count + "개";
            return names;
        }

        /// <summary>클립보드 텍스트. 화면 Smoke 검사는 Spec 표 파일 내용을 돌려주도록 바꾼다.</summary>
        protected virtual string ClipboardText()
        {
            return Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
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

        #endregion
    }
}
