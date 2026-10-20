using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test Para 설정 화면이다. (2026-10-14, 같은 날 화면 재구성으로 E-Test Limit 설정 화면 기능을 합치고, 결과 파일 등록은 E-Test Para 등록 화면으로 분리)
    /// 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md (편집 / 저장), 문서/설계/04_ETest/2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md (화면 분리)
    /// 기준:
    ///  - 등록된 Device / 공정만 골라서 조회한다(콤보 목록 = GetEtParaProgramList, 직접 입력 없음). 등록은 E-Test Para 등록 화면(결과 파일)에서 한다.
    ///  - GDS Chain = Chain 1 의 GDS Setup Chain 이름. 사용자가 표에 직접 입력한다(오른쪽 GDS Setup Chain 목록에서 이름 복사). 자동 묶기 / 대표 Chain 없음.
    ///    오른쪽 목록은 위에서 고른 E-Test Device 이름의 GDS Device 로 조회한다(고르지 않음, 없으면 빈 목록, 2026-10-15).
    ///  - Spec(Lower / Upper / Enable)도 같은 표에서 고친다. [저장] 은 수정된 행만 서버로 보낸다.
    /// 처리 흐름:
    ///  1) Device / 공정 콤보를 고르면 바로 조회한다(저장 안 한 수정이 있으면 먼저 묻고, 아니요면 원래 값으로 되돌림).
    ///  2) GDS Chain / Lower / Upper / 비고는 더블클릭(F2)으로, 사용 / Enable 은 클릭으로 고친다.
    ///     행 Tag(ParaState)에 조회 당시 값을 두고 바뀐 칸은 노란색 + 행 머리글 '*'. [저장] -> SaveEtParameterSetup(수정 행만) -> 다시 판정 질문.
    ///  3) [Spec 표 붙여넣기] 는 고객 Spec 표(클립보드)를 공정별로 등록 + 저장한다. [선택 삭제] 는 Parameter 와 Spec 을 지운다.
    ///  4) 필터 콤보(2026-10-15): 표 위 측정 / 조건 / Chain 1 / GDS Chain / 사용 / Enable 콤보. 펼치면 (전체) + 그 열 값(건수)이 나오고, 고르면 그 값의 행만 보인다.
    ///     건수는 다른 콤보에서 고른 값을 통과한 행으로 센다(여러 콤보는 AND). 수정한 행은 필터와 상관없이 보인다.
    ///     같은 Device / 공정을 다시 조회(저장 뒤 포함)하면 고른 값을 유지하고, Device / 공정이 바뀌면 (전체)로 돌린다. [필터 해제] = 모두 (전체).
    /// 진행 표시: 서버 호출은 화면 아래 busyBar(ProgressStatusBar)가 별도 스레드에서 돌리고 작업 이름과 모래시계 커서를 보여 준다.
    ///  버튼 하나의 처리(서버 호출 + 다시 조회 + 표 채우기)는 busyBar.Begin 으로 묶고, 콤보 변경 중 짧은 조회는 RunHere(화면 스레드)로 한다.
    /// </summary>
    public partial class frmEtParaSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private static readonly Color DirtyColor = Color.LightYellow;
        private static readonly Color ErrorColor = Color.MistyRose;
        private const int MaxTextLength = 200;

        private RO.GdsEtResult paraService;
        private string loadedFactory = string.Empty;
        private string loadedDevice = string.Empty;
        private string loadedProgram = string.Empty;
        private string loadedGdsDevice = string.Empty;
        private bool filling;
        /// <summary>Factory 의 등록 Parameter 가 있는 Device + 공정 (콤보 목록)</summary>
        private DataTable programList;
        /// <summary>지금 GDS Device 의 Setup Chain 이름 (대소문자 무시)</summary>
        private readonly HashSet<string> gdsNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtParaSetup()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colPLower, colPUpper, colGUse })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn column in EditColumns())
                column.HeaderCell.Style.Font = new Font(dgvPara.Font, FontStyle.Bold);
            FillFilterCombos(); // 조회 전에도 (전체) 가 보이게
        }

        /// <summary>E-Test 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return paraService ?? (paraService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 와 Device / 공정 목록을 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadDeviceCombo();
        }

        #region [조건 / 조회]

        /// <summary>Factory 의 등록 Parameter 가 있는 Device + 공정을 읽어 콤보 목록을 채운다. 콤보 글자(지금 값)는 그대로 둔다.</summary>
        private void LoadDeviceCombo()
        {
            string factory = txtFactory.Text.Trim();
            programList = null;
            if (factory.Length == 0) return;
            try
            {
                programList = busyBar.RunHere("Device 목록", () => Service.GetEtParaProgramList(factory));
            }
            catch (Exception ex)
            {
                SetStatus("Device 목록 조회 실패(직접 입력 가능): " + Cause(ex).Message);
                return;
            }
            FillCombo(cboDevice, programList.Rows.Cast<DataRow>().Select(r => r["DEVICE_ID"].ToString()).Distinct(StringComparer.Ordinal), cboDevice.Text);
            FillProgramCombo(cboTestProgram.Text, false);
        }

        /// <summary>지금 Device 의 공정을 콤보 목록에 채운다. pickFirst 면 select 가 목록에 없을 때 첫 공정을 고른다(목록이 비면 입력값 유지).</summary>
        private void FillProgramCombo(string select, bool pickFirst)
        {
            string device = cboDevice.Text.Trim();
            List<string> programs = programList == null ? new List<string>() : programList.Rows.Cast<DataRow>()
                .Where(r => string.Equals(r["DEVICE_ID"].ToString(), device, StringComparison.OrdinalIgnoreCase))
                .Select(r => r["TEST_PROGRAM"].ToString()).ToList();
            string wanted = EtParameterNameRule.ToTestProgram(select);
            if (pickFirst && programs.Count > 0 && !programs.Contains(wanted, StringComparer.OrdinalIgnoreCase))
                wanted = programs[0];
            FillCombo(cboTestProgram, programs, wanted);
        }

        private void FillCombo(ComboBox combo, IEnumerable<string> items, string text)
        {
            bool wasFilling = filling;
            filling = true;
            try
            {
                combo.BeginUpdate();
                combo.Items.Clear();
                foreach (string item in items) combo.Items.Add(item);
                combo.EndUpdate();
                combo.Text = text ?? string.Empty;
            }
            finally
            {
                filling = wasFilling;
            }
        }

        private void txtFactory_Leave(object sender, EventArgs e)
        {
            LoadDeviceCombo();
        }

        /// <summary>Device 를 고르면 그 Device 의 공정 목록을 채우고 바로 조회한다. 저장 안 한 수정을 버리지 않으면 원래 값으로 되돌린다.</summary>
        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling || cboDevice.SelectedIndex < 0) return;
            if (!ConfirmDiscardParaChanges())
            {
                RestoreCombos();
                return;
            }
            FillProgramCombo(cboTestProgram.Text, true);
            Search();
        }

        private void cboTestProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling || cboTestProgram.SelectedIndex < 0) return;
            if (!ConfirmDiscardParaChanges())
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
            cboDevice.Text = loadedDevice;
            cboTestProgram.Text = loadedProgram;
            filling = wasFilling;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ConfirmDiscardParaChanges()) Search();
        }

        /// <summary>저장 안 된 수정이 있으면 버릴지 묻는다.</summary>
        private bool ConfirmDiscardParaChanges()
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.EndEdit();
            int dirty = DirtyParaRows().Count;
            return dirty == 0
                || Ask("저장하지 않은 수정 " + dirty + "행이 있습니다. 버리고 계속할까요?", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        /// 조회: 위 Factory / Device / 공정으로 GDS Device 와 Setup Chain 목록, 등록 Parameter 표를 다시 읽는다. 저장 안 된 수정 확인은 부르는 쪽에서 한다.
        /// </summary>
        private void Search()
        {
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            string program = EtParameterNameRule.ToTestProgram(cboTestProgram.Text);
            if (factory.Length == 0 || device.Length == 0 || program.Length == 0)
            {
                Notify("Factory / Device / 공정을 고르세요. Device 목록이 비어 있으면 E-Test Para 등록 화면에서 결과 파일로 먼저 등록하세요.", MessageBoxIcon.Warning);
                return;
            }
            bool deviceChanged = !string.Equals(device, loadedDevice, StringComparison.OrdinalIgnoreCase) || !string.Equals(factory, loadedFactory, StringComparison.Ordinal);
            // Device / 공정이 바뀌면 열 값이 달라 필터 콤보를 (전체)로 돌린다(같은 Device / 공정을 다시 조회할 때는 유지)
            if (deviceChanged || !string.Equals(program, loadedProgram, StringComparison.Ordinal))
                ResetFilterCombos();
            loadedFactory = factory;
            loadedDevice = device;
            loadedProgram = program;
            RestoreCombos();
            using (busyBar.Begin("조회"))
            {
                LoadGdsChains();
                LoadParameters();
            }
        }

        #endregion

        #region [조회 / 거르기]

        /// <summary>
        /// 등록 Parameter + GDS Chain + Spec + 값 통계를 읽어 표를 채운다.
        /// 각 행 Tag(ParaState)에 조회 당시 값을 넣어 수정 여부를 비교한다. 행이 많을 수 있어(헤더 전체 2,755개 등) 모아서 한 번에 넣는다.
        /// </summary>
        private void LoadParameters()
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.CancelEdit();
            dgvPara.CurrentCell = null;
            dgvPara.Rows.Clear();
            DataTable dt;
            string factory = loadedFactory, device = loadedDevice, program = loadedProgram;
            try
            {
                dt = busyBar.Run("등록 Parameter 조회", () => Service.GetEtParameterSetupList(factory, device, program));
            }
            catch (Exception ex)
            {
                UpdateParaInfo();
                Notify("등록 Parameter 조회 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                return;
            }
            int specCount = 0;
            filling = true;
            dgvPara.SuspendLayout();
            try
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string name = dr["ITEM_NAME"].ToString();
                    bool hasSpec = dr["HAS_SPEC"].ToString() == "Y";
                    bool use = dr["USE_FLAG"].ToString() != "N";
                    string gds = dr["GDS_CHAIN_NAME"].ToString();
                    string lower = Number(dr["LOWER_LIMIT"], "R");
                    string upper = Number(dr["UPPER_LIMIT"], "R");
                    bool enable = hasSpec && dr["ENABLE_FLAG"].ToString() == "Y";
                    string remark = dr["REMARK"].ToString();
                    if (hasSpec) specCount++;

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvPara, dr["HEADER_TEXT"], name, dr["MEAS_TYPE"],
                        ConditionText(dr["PARA_NAME"].ToString(), dr["MEAS_TYPE"].ToString(), dr["COND_VALUE"].ToString()),
                        dr["CHAIN_NAME"], dr["CHAIN_NAME2"], dr["UNIT"], use, gds, lower, upper, enable,
                        remark, dr["SPEC_TEXT"], SourceText(dr["REG_SOURCE"].ToString()),
                        dr["REG_FILE_NAME"], dr["UPDATE_USER"] + " " + FormatDate(dr["UPDATE_TIME"]));
                    row.Tag = new ParaState
                    {
                        Name = name, Use = use, Gds = gds, Lower = lower, Upper = upper, Enable = enable, Remark = remark,
                        HasSpec = hasSpec
                    };
                    PaintRow(row);
                    rows.Add(row);
                    busyBar.Pulse();
                }
                dgvPara.Rows.AddRange(rows.ToArray());
            }
            finally
            {
                dgvPara.ResumeLayout();
                filling = false;
            }
            FillFilterCombos();
            ApplyParaFilter();
            SetStatus(dt.Rows.Count == 0
                ? "등록 Parameter 없음 / Device " + device + " / 공정 " + program
                : "조회 완료 / Device " + device + " / 공정 " + program + " / 등록 Parameter " + dt.Rows.Count + " / Spec " + specCount + " / GDS Device " + loadedGdsDevice
                    + FilterText());
        }

        private void paraFilter_Changed(object sender, EventArgs e)
        {
            if (!filling) ApplyParaFilter();
        }

        /// <summary>
        /// 찾기(헤더 원문 / 저장 이름 / Chain / GDS Chain) / Spec 있는 것만 / GDS Chain 없는 것만 / 필터 콤보로 행을 숨긴다. 수정한 행은 항상 보인다.
        /// </summary>
        private void ApplyParaFilter()
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.EndEdit();
            string find = txtParaFind.Text.Trim();
            DataGridViewColumn[] findColumns = { colPHeader, colPName, colPChain1, colPChain2, colPGds };
            dgvPara.CurrentCell = null;
            foreach (DataGridViewRow row in dgvPara.Rows)
            {
                ParaState s = (ParaState)row.Tag;
                row.Visible = IsParaDirty(row)
                    || ((!chkSpecOnly.Checked || s.HasSpec)
                        && (!chkNoGdsOnly.Checked || CellText(row, colPGds).Length == 0)
                        && (find.Length == 0 || findColumns.Any(c => CellText(row, c).IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0))
                        && MatchesFilters(row, null));
            }
            btnClearFilter.Enabled = FilterSummary().Length > 0;
            RefreshGdsUse();
            UpdateParaInfo();
        }

        /// <summary>정보 줄: 등록 / 사용 / GDS Chain(목록에 없음) / Spec / 표시 / 수정 행.</summary>
        private void UpdateParaInfo()
        {
            int total = dgvPara.Rows.Count, use = 0, gds = 0, unknown = 0, spec = 0, visible = 0, dirty = 0;
            foreach (DataGridViewRow row in dgvPara.Rows)
            {
                ParaState s = (ParaState)row.Tag;
                if (Checked(row, colPUse)) use++;
                string name = CellText(row, colPGds);
                if (name.Length > 0)
                {
                    gds++;
                    if (!gdsNames.Contains(name)) unknown++;
                }
                if (s.HasSpec) spec++;
                if (row.Visible) visible++;
                if (IsParaDirty(row)) dirty++;
            }
            //lblParaInfo.Text = "등록 " + total + " / 사용 " + use + " / GDS Chain " + gds + " (목록에 없음 " + unknown + ") / Spec " + spec
            //    + " / 표시 " + visible + (dirty > 0 ? " / 수정 " + dirty + "행 (저장 전)" : string.Empty);
        }

        #endregion

        #region [필터 콤보]

        /// <summary>필터 콤보와 거르는 열 (표 위 왼쪽부터)</summary>
        private KeyValuePair<ComboBox, DataGridViewColumn>[] FilterCombos()
        {
            return new[]
            {
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltMeas, colPMeas),
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltCond, colPCond),
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltChain1, colPChain1),
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltGds, colPGds),
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltUse, colPUse),
                new KeyValuePair<ComboBox, DataGridViewColumn>(cboFltEnable, colPEnable)
            };
        }

        /// <summary>필터가 비교하는 칸 값: 사용 / Enable(체크 열) = Y / N, 나머지 = 칸 글자.</summary>
        private string FilterValue(DataGridViewRow row, DataGridViewColumn column)
        {
            if (column is DataGridViewCheckBoxColumn) return Checked(row, column) ? "Y" : "N";
            return CellText(row, column);
        }

        /// <summary>콤보에서 고른 값. (전체) 면 null, (빈 값) 이면 "".</summary>
        private static string FilterSelected(ComboBox combo)
        {
            FilterItem item = combo.SelectedItem as FilterItem;
            return item == null ? null : item.Value;
        }

        /// <summary>행이 필터 콤보에서 고른 값과 모두 맞는지. except 콤보는 빼고 본다(그 콤보 목록의 건수를 셀 때).</summary>
        private bool MatchesFilters(DataGridViewRow row, ComboBox except)
        {
            foreach (KeyValuePair<ComboBox, DataGridViewColumn> filter in FilterCombos())
            {
                if (filter.Key == except) continue;
                string value = FilterSelected(filter.Key);
                if (value != null && FilterValue(row, filter.Value) != value) return false;
            }
            return true;
        }

        /// <summary>모든 필터 콤보 목록을 지금 표로 다시 채운다(고른 값 유지).</summary>
        private void FillFilterCombos()
        {
            foreach (KeyValuePair<ComboBox, DataGridViewColumn> filter in FilterCombos())
                FillFilterCombo(filter.Key, filter.Value);
        }

        /// <summary>
        /// 필터 콤보 목록 = (전체) + 그 열 값(건수). 건수는 다른 콤보에서 고른 값을 통과한 행만 센다. 순서: 글자 순(대소문자 무시), (빈 값)은 끝.
        /// 고른 값이 지금 표에 없어도 고른 상태를 유지하려고 0건으로 넣는다.
        /// </summary>
        private void FillFilterCombo(ComboBox combo, DataGridViewColumn column)
        {
            string selected = FilterSelected(combo);
            Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.Ordinal);
            int total = 0;
            foreach (DataGridViewRow row in dgvPara.Rows)
            {
                if (!MatchesFilters(row, combo)) continue;
                string value = FilterValue(row, column);
                int n;
                counts.TryGetValue(value, out n);
                counts[value] = n + 1;
                total++;
            }
            if (selected != null && !counts.ContainsKey(selected)) counts[selected] = 0;
            List<FilterItem> items = new List<FilterItem> { new FilterItem(null, total) };
            items.AddRange(counts.OrderBy(p => p.Key.Length == 0 ? 1 : 0).ThenBy(p => p.Key, StringComparer.OrdinalIgnoreCase)
                .ThenBy(p => p.Key, StringComparer.Ordinal).Select(p => new FilterItem(p.Key, p.Value)));
            bool wasFilling = filling;
            filling = true;
            try
            {
                combo.BeginUpdate();
                combo.Items.Clear();
                combo.Items.AddRange(items.ToArray());
                combo.EndUpdate();
                combo.SelectedItem = items.First(i => i.Value == selected);
            }
            finally
            {
                filling = wasFilling;
            }
        }

        /// <summary>콤보를 펼칠 때 목록을 지금 표 값(수정 중인 값 포함)과 다른 콤보 선택으로 다시 센다.</summary>
        private void cboFlt_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            FillFilterCombo(combo, FilterCombos().First(f => f.Key == combo).Value);
        }

        /// <summary>필터 콤보에서 값을 고르면 행을 다시 거르고 상태 줄에 고른 값 / 표시 행을 보인다.</summary>
        private void cboFlt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filling) return;
            ApplyParaFilter();
            ShowFilterStatus();
        }

        /// <summary>[필터 해제]: 모든 필터 콤보를 (전체)로.</summary>
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            ResetFilterCombos();
            ApplyParaFilter();
            ShowFilterStatus();
        }

        /// <summary>모든 필터 콤보를 (전체)로 돌린다(행 거르기는 부르는 쪽에서).</summary>
        private void ResetFilterCombos()
        {
            bool wasFilling = filling;
            filling = true;
            try
            {
                foreach (KeyValuePair<ComboBox, DataGridViewColumn> filter in FilterCombos())
                    if (filter.Key.Items.Count > 0) filter.Key.SelectedIndex = 0;
            }
            finally
            {
                filling = wasFilling;
            }
        }

        /// <summary>고른 필터 값 (예: 조건=IFORCE=8.0MA, GDS Chain (Chain 1)=(빈 값)). 없으면 빈 문자열.</summary>
        private string FilterSummary()
        {
            return string.Join(", ", FilterCombos().Where(f => FilterSelected(f.Key) != null)
                .Select(f => f.Value.HeaderText + "=" + FilterItem.Display(FilterSelected(f.Key))).ToArray());
        }

        private void ShowFilterStatus()
        {
            string summary = FilterSummary();
            int visible = dgvPara.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            SetStatus((summary.Length > 0 ? "필터 " + summary : "필터 해제") + " / 표시 " + visible + " / 전체 " + dgvPara.Rows.Count);
        }

        /// <summary>조회 상태 줄에 붙이는 필터 문구. 고른 값이 없으면 빈 문자열.</summary>
        private string FilterText()
        {
            string summary = FilterSummary();
            if (summary.Length == 0) return string.Empty;
            int visible = dgvPara.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            return " / 필터 " + summary + " 표시 " + visible;
        }

        /// <summary>필터 콤보 한 줄: Value null = (전체), "" = (빈 값). 콤보 글자 = 값 (건수)</summary>
        private sealed class FilterItem
        {
            public readonly string Value;
            public readonly int Count;

            public FilterItem(string value, int count)
            {
                Value = value;
                Count = count;
            }

            public static string Display(string value)
            {
                return value == null ? "(전체)" : value.Length == 0 ? "(빈 값)" : value;
            }

            public override string ToString()
            {
                return Display(Value) + " (" + Count + ")";
            }
        }

        #endregion

        #region [편집 / 수정 표시]

        /// <summary>편집할 수 있는 열: 사용 / GDS Chain / Lower / Upper / Enable / 비고</summary>
        private DataGridViewColumn[] EditColumns()
        {
            return new DataGridViewColumn[] { colPUse, colPGds, colPLower, colPUpper, colPEnable, colPRemark };
        }

        /// <summary>더블클릭 / F2 로 편집하는 글자 열</summary>
        private bool IsTextEditColumn(int columnIndex)
        {
            return columnIndex == colPGds.Index || columnIndex == colPLower.Index || columnIndex == colPUpper.Index || columnIndex == colPRemark.Index;
        }

        private bool IsCheckColumn(int columnIndex)
        {
            return columnIndex == colPUse.Index || columnIndex == colPEnable.Index;
        }

        private void dgvPara_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !IsTextEditColumn(e.ColumnIndex)) return;
            dgvPara.CurrentCell = dgvPara.Rows[e.RowIndex].Cells[e.ColumnIndex];
            dgvPara.BeginEdit(true);
        }

        /// <summary>사용 / Enable 은 클릭할 때마다 바뀐다(열은 읽기 전용, 값은 코드에서 바꾼다).</summary>
        private void dgvPara_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !IsCheckColumn(e.ColumnIndex)) return;
            Toggle(dgvPara.Rows[e.RowIndex].Cells[e.ColumnIndex]);
        }

        /// <summary>F2 = 편집 시작, Space = 체크 바꾸기, Ctrl+V = 지금 칸에 클립보드 글자(첫 칸) 넣기.</summary>
        private void dgvPara_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewCell cell = dgvPara.CurrentCell;
            if (cell == null || dgvPara.IsCurrentCellInEditMode) return;
            if (e.KeyCode == Keys.F2 && IsTextEditColumn(cell.ColumnIndex))
                dgvPara.BeginEdit(true);
            else if (e.KeyCode == Keys.Space && !e.Control && IsCheckColumn(cell.ColumnIndex))
                Toggle(cell);
            else if (e.Control && e.KeyCode == Keys.V && IsTextEditColumn(cell.ColumnIndex))
                cell.Value = FirstCellText(ClipboardText());
            else
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>편집이 끝나면 앞뒤 공백을 지운다.</summary>
        private void dgvPara_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewCell cell = dgvPara.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string raw = Convert.ToString(cell.Value, CultureInfo.InvariantCulture);
            if (raw != raw.Trim()) cell.Value = raw.Trim();
        }

        /// <summary>
        /// 칸 값이 바뀌면 수정 표시를 다시 칠한다.
        /// Spec 이 없던 행에 Lower / Upper 를 처음 넣으면 Enable 을 켜고(한 번), Lower / Upper 를 모두 지우면 Enable 을 끈다(저장 때 Spec 삭제).
        /// </summary>
        private void dgvPara_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (filling || e.RowIndex < 0) return;
            DataGridViewRow row = dgvPara.Rows[e.RowIndex];
            ParaState s = row.Tag as ParaState;
            if (s == null) return;
            if (e.ColumnIndex == colPLower.Index || e.ColumnIndex == colPUpper.Index)
            {
                bool any = CellText(row, colPLower).Length > 0 || CellText(row, colPUpper).Length > 0;
                filling = true;
                try
                {
                    if (any && !s.HasSpec && !s.AutoEnabled && !Checked(row, colPEnable))
                    {
                        s.AutoEnabled = true;
                        row.Cells[colPEnable.Index].Value = true;
                    }
                    else if (!any && Checked(row, colPEnable))
                        row.Cells[colPEnable.Index].Value = false;
                }
                finally
                {
                    filling = false;
                }
            }
            PaintRow(row);
            if (e.ColumnIndex == colPGds.Index) RefreshGdsUse();
            UpdateParaInfo();
        }

        /// <summary>칸이 조회 당시 값과 다른지. Lower / Upper 는 숫자로 비교한다(720 과 720.0 은 같음).</summary>
        private bool Changed(DataGridViewRow row, DataGridViewColumn column)
        {
            ParaState s = (ParaState)row.Tag;
            if (column == colPUse) return Checked(row, colPUse) != s.Use;
            if (column == colPEnable) return Checked(row, colPEnable) != s.Enable;
            if (column == colPGds) return CellText(row, colPGds) != s.Gds;
            if (column == colPLower) return !SameNumber(CellText(row, colPLower), s.Lower);
            if (column == colPUpper) return !SameNumber(CellText(row, colPUpper), s.Upper);
            if (column == colPRemark) return CellText(row, colPRemark) != s.Remark;
            return false;
        }

        private bool IsParaDirty(DataGridViewRow row)
        {
            return row.Tag is ParaState && EditColumns().Any(c => Changed(row, c));
        }

        private bool IsSpecChanged(DataGridViewRow row)
        {
            return Changed(row, colPLower) || Changed(row, colPUpper) || Changed(row, colPEnable);
        }

        private List<DataGridViewRow> DirtyParaRows()
        {
            return dgvPara.Rows.Cast<DataGridViewRow>().Where(IsParaDirty).ToList();
        }

        /// <summary>
        /// 수정한 행의 입력 오류(서버 SaveEtParameterSetup 검사와 같은 기준). 없으면 빈 문자열. column = 붉게 칠할 칸.
        /// Spec 이 있던 행의 Lower / Upper 를 모두 지우면 Spec 삭제라 오류가 아니다(그때 Enable 은 자동으로 꺼진다).
        /// </summary>
        private string RowError(DataGridViewRow row, out DataGridViewColumn column)
        {
            column = colPLower;
            string lowerText = CellText(row, colPLower);
            string upperText = CellText(row, colPUpper);
            double lower = 0, upper = 0;
            if (lowerText.Length > 0 && !TryNumber(lowerText, out lower)) return "Lower 가 숫자가 아닙니다.";
            column = colPUpper;
            if (upperText.Length > 0 && !TryNumber(upperText, out upper)) return "Upper 가 숫자가 아닙니다.";
            column = colPLower;
            if (lowerText.Length > 0 && upperText.Length > 0 && lower > upper) return "Lower 가 Upper 보다 큽니다.";
            column = colPEnable;
            if (Checked(row, colPEnable) && lowerText.Length == 0 && upperText.Length == 0) return "Enable 인데 Lower / Upper 가 없습니다(둘 중 하나는 입력).";
            column = colPGds;
            if (CellText(row, colPGds).Length > MaxTextLength) return "GDS Chain 은 " + MaxTextLength + "자 이하입니다.";
            column = colPRemark;
            if (CellText(row, colPRemark).Length > MaxTextLength) return "비고는 " + MaxTextLength + "자 이하입니다.";
            column = null;
            return string.Empty;
        }

        /// <summary>바뀐 칸 노란색 / 오류 칸 붉은색 / 행 머리글 '*' / 사용 안 함 행 회색 글자 / 목록에 없는 GDS Chain 주황 글자.</summary>
        private void PaintRow(DataGridViewRow row)
        {
            bool dirty = false;
            foreach (DataGridViewColumn column in EditColumns())
            {
                bool changed = Changed(row, column);
                dirty |= changed;
                row.Cells[column.Index].Style.BackColor = changed ? DirtyColor : Color.Empty;
            }
            DataGridViewColumn errorColumn = null;
            string error = dirty ? RowError(row, out errorColumn) : string.Empty;
            if (errorColumn != null)
                row.Cells[errorColumn.Index].Style.BackColor = ErrorColor;
            row.Cells[colPLower.Index].ToolTipText = error;
            row.HeaderCell.Value = dirty ? "*" : null;
            row.HeaderCell.ToolTipText = error.Length > 0 ? error : dirty ? "수정됨 (저장 전)" : string.Empty;
            row.DefaultCellStyle.ForeColor = Checked(row, colPUse) ? Color.Empty : Color.Gray;
            PaintGds(row);
        }

        /// <summary>GDS Chain 이 지금 GDS Setup Chain 목록에 없으면 주황 글자(저장은 된다).</summary>
        private void PaintGds(DataGridViewRow row)
        {
            string gds = CellText(row, colPGds);
            bool unknown = gds.Length > 0 && !gdsNames.Contains(gds);
            row.Cells[colPGds.Index].Style.ForeColor = unknown ? Color.DarkOrange : Color.Empty;
            row.Cells[colPGds.Index].ToolTipText = unknown ? "GDS Setup Chain 목록에 없음" : string.Empty;
        }

        /// <summary>우클릭한 행이 선택에 없으면 그 행만 고른다. 드래그로 고른 여러 행 위에서 우클릭하면 선택을 그대로 둔다. (두 표 공용)</summary>
        private void grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (e.Button != MouseButtons.Right || e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            if (row.Selected || (e.ColumnIndex >= 0 && row.Cells[e.ColumnIndex].Selected)) return;
            if (grid.IsCurrentCellInEditMode) grid.EndEdit();
            grid.ClearSelection();
            DataGridViewColumn first = grid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            grid.CurrentCell = row.Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : first.Index];
            row.Selected = true;
        }

        #endregion

        #region [저장 / 삭제 / Spec 표 붙여넣기]

        /// <summary>
        /// 수정한 행만 저장한다(SaveEtParameterSetup: 사용 / 비고 / GDS Chain / Lower / Upper / Enable).
        /// Lower / Upper 를 모두 지운 행 중 Spec 이 있던 행은 Spec 삭제라 먼저 묻는다. Spec 을 바꿨으면 다시 판정할지 묻고 다시 조회한다.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.EndEdit();
            List<DataGridViewRow> rows = DirtyParaRows();
            if (rows.Count == 0)
            {
                Notify("저장할 수정이 없습니다. (GDS Chain / Lower / Upper / 비고 = 더블클릭, 사용 / Enable = 클릭)", MessageBoxIcon.Information);
                return;
            }
            if (!EnsureLoaded()) return;
            List<string> errors = new List<string>();
            foreach (DataGridViewRow row in rows)
            {
                DataGridViewColumn column;
                string error = RowError(row, out column);
                if (error.Length > 0) errors.Add(CellText(row, colPName) + ": " + error);
            }
            if (errors.Count > 0)
            {
                Notify("입력을 확인하세요(붉은 칸).\r\n\r\n" + string.Join("\r\n", errors.Take(15).ToArray()) + (errors.Count > 15 ? "\r\n..." : string.Empty), MessageBoxIcon.Warning);
                return;
            }
            List<DataGridViewRow> specDeletes = rows.Where(r => ((ParaState)r.Tag).HasSpec && CellText(r, colPLower).Length == 0 && CellText(r, colPUpper).Length == 0).ToList();
            if (specDeletes.Count > 0
                && Ask("Lower / Upper 를 모두 지운 Parameter " + specDeletes.Count + "개는 Spec 이 삭제됩니다(판정 안 함).\r\n\r\n"
                    + string.Join("\r\n", specDeletes.Take(10).Select(r => CellText(r, colPName)).ToArray()) + (specDeletes.Count > 10 ? "\r\n..." : string.Empty)
                    + "\r\n\r\n계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string[,] data = new string[rows.Count, 7];
            for (int i = 0; i < rows.Count; i++)
            {
                DataGridViewRow row = rows[i];
                string lower = CellText(row, colPLower), upper = CellText(row, colPUpper);
                data[i, 0] = ((ParaState)row.Tag).Name;
                data[i, 1] = Checked(row, colPUse) ? "Y" : "N";
                data[i, 2] = CellText(row, colPRemark);
                data[i, 3] = CellText(row, colPGds);
                data[i, 4] = lower;
                data[i, 5] = upper;
                data[i, 6] = Checked(row, colPEnable) && (lower.Length > 0 || upper.Length > 0) ? "Y" : "N";
            }
            bool specChanged = rows.Any(IsSpecChanged);
            string[] key = { loadedFactory, loadedDevice, loadedProgram };
            string user = User();
            using (busyBar.Begin("저장 (" + rows.Count + "행)"))
            {
                DataRow result;
                try
                {
                    result = busyBar.Run("서버 저장", () => Service.SaveEtParameterSetup(key, data, user)).Rows[0];
                }
                catch (Exception ex)
                {
                    SetStatus("저장 실패 / " + Cause(ex).Message);
                    Notify("저장하지 못했습니다(아무것도 저장하지 않음).\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
                    return;
                }
                string rejudge = specChanged ? AskRejudge(loadedFactory, loadedDevice, new[] { loadedProgram }) : string.Empty;
                LoadParameters();
                SetStatus("저장 완료 / 수정 " + result["UPDATED"] + "행 / Spec 저장 " + result["SPEC_SAVED"] + " / Spec 삭제 " + result["SPEC_DELETED"]
                    + " / Device " + loadedDevice + " / 공정 " + loadedProgram + rejudge);
            }
        }

        /// <summary>[등록 Parameter] 탭에서 고른 Parameter 를 삭제한다. Spec 도 같이 지운다. 측정값이 저장된 Parameter 는 서버가 거부한다.</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.EndEdit();
            List<DataGridViewRow> rows = SelectedRows(dgvPara);
            if (rows.Count == 0)
            {
                Notify("지울 Parameter 행을 고르세요(드래그 / Ctrl / Shift 로 여러 행).", MessageBoxIcon.Information);
                return;
            }
            if (!EnsureLoaded()) return;
            int specCount = rows.Count(r => ((ParaState)r.Tag).HasSpec);
            int dirty = DirtyParaRows().Count;
            string msg = "Parameter " + rows.Count + "개를 삭제합니다." + (specCount > 0 ? " Spec 이 있는 " + specCount + "개는 Spec 도 같이 지웁니다." : string.Empty)
                + "\r\n측정값이 저장된 Parameter 는 지울 수 없습니다(사용 안 함으로 바꾸세요)."
                + (dirty > 0 ? "\r\n\r\n저장하지 않은 수정 " + dirty + "행은 버려집니다." : string.Empty);
            if (Ask(msg, MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            string[] key = { loadedFactory, loadedDevice, loadedProgram };
            string[] names = rows.Select(r => ((ParaState)r.Tag).Name).ToArray();
            string user = User();
            using (busyBar.Begin("삭제 (" + names.Length + "개)"))
            {
                try
                {
                    int deleted = busyBar.Run("서버 삭제", () => Service.DeleteEtParameters(key, names, user));
                    LoadParameters();
                    SetStatus("삭제 " + deleted + "개");
                }
                catch (Exception ex)
                {
                    Notify("삭제 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 고객 Spec 표(클립보드)를 읽어 공정별로 저장한다.
        ///  1) EtSpecTable.Parse: 해석할 수 없는 행이 있으면 목록을 보여 주고 멈춘다. 헤더를 저장 이름으로 바꿀 수 없는 행도 멈춘다.
        ///  2) 저장 안 한 수정이 있으면 버릴지 묻는다. 공정마다 등록 Parameter 를 읽어 표에만 있는 Parameter 를 센다.
        ///     공정별 건수 / 새로 등록할 수 / 대상 Device 를 묻는다(표 Device 열은 무시하고 위 Device 로 저장).
        ///  3) 공정마다 RegisterEtParameters(출처 SPEC, 등록 안 된 것만) -> SaveEtLimit(MERGE, Enable = Y).
        ///     이미 있는 Spec 은 Lower / Upper / 단위 / Spec 원문을 덮어쓰고 비고는 지운다.
        ///  4) 다시 판정할지 묻고, Device / 공정 목록과 표를 다시 읽는다. 지금 공정이 표에 없으면 표의 첫 공정으로 바꾼다.
        /// </summary>
        private void btnPasteSpec_Click(object sender, EventArgs e)
        {
            if (dgvPara.IsCurrentCellInEditMode) dgvPara.EndEdit();
            string factory = txtFactory.Text.Trim();
            string device = cboDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("Factory / Device 를 먼저 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            List<EtSpecRow> rows = EtSpecTable.Parse(ClipboardText());
            if (rows.Count == 0)
            {
                Notify("클립보드에 Spec 표가 없습니다. Excel 에서 Device / Test Type / Chain name / Parameter / Spec / Unit 열을 복사한 뒤 누르세요.", MessageBoxIcon.Warning);
                return;
            }
            List<string> bad = rows.Where(r => r.Error != null).Select(r => r.LineNo + "줄: " + r.Error + " / " + r.SpecText).ToList();
            Dictionary<EtSpecRow, string> names = new Dictionary<EtSpecRow, string>();
            foreach (EtSpecRow row in rows.Where(r => r.Error == null))
            {
                EtParameterParts parts = EtParameterNameRule.Parse(row.HeaderText);
                if (parts.IsValid) names[row] = parts.ParameterName;
                else bad.Add(row.LineNo + "줄: " + parts.Error + " / " + row.HeaderText);
            }
            if (bad.Count > 0)
            {
                Notify("해석할 수 없는 행이 " + bad.Count + "개 있어 저장하지 않았습니다.\r\n\r\n"
                    + string.Join("\r\n", bad.Take(15).ToArray()) + (bad.Count > 15 ? "\r\n..." : string.Empty), MessageBoxIcon.Warning);
                SetStatus("Spec 표 확인 필요 / 오류 행 " + bad.Count);
                return;
            }
            if (!ConfirmDiscardParaChanges()) return;
            List<IGrouping<string, EtSpecRow>> groups = rows.GroupBy(r => EtParameterNameRule.ToTestProgram(r.TestProgram)).OrderBy(g => g.Key, StringComparer.Ordinal).ToList();
            Dictionary<string, List<EtSpecRow>> missing = new Dictionary<string, List<EtSpecRow>>(StringComparer.Ordinal);
            try
            {
                string[] programs = groups.Select(g => g.Key).ToArray();
                DataTable[] lists = busyBar.Run("등록 Parameter 확인", () => programs.Select(t => Service.GetEtParameterList(factory, device, t)).ToArray());
                for (int k = 0; k < groups.Count; k++)
                {
                    HashSet<string> exists = new HashSet<string>(lists[k].Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()), StringComparer.Ordinal);
                    missing[groups[k].Key] = groups[k].Where(r => !exists.Contains(names[r])).ToList();
                }
            }
            catch (Exception ex)
            {
                Notify("등록 Parameter 를 조회하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
                return;
            }
            string summary = string.Join(" / ", groups.Select(g => g.Key + " " + g.Count()).ToArray());
            int missingCount = missing.Values.Sum(l => l.Count);
            string tableDevices = string.Join(", ", rows.Select(r => r.Device).Where(d => d.Length > 0).Distinct().ToArray());
            if (Ask("Spec " + rows.Count + "건을 저장합니다.\r\n\r\n공정별: " + summary + "\r\nDevice: " + device
                + (tableDevices.Length > 0 && !string.Equals(tableDevices, device, StringComparison.OrdinalIgnoreCase) ? " (표의 Device " + tableDevices + " 대신)" : string.Empty)
                + (missingCount > 0 ? "\r\n\r\n등록되지 않은 Parameter " + missingCount + "개는 Spec 표 출처로 먼저 등록합니다("
                    + string.Join(" / ", missing.Where(m => m.Value.Count > 0).Select(m => m.Key + " " + m.Value.Count).ToArray()) + ")." : string.Empty)
                + "\r\n\r\n이미 있는 Spec 은 Lower / Upper / 단위 / Spec 원문을 덮어쓰고 Enable 을 켭니다. 계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string user = User();
            int registeredCount = 0;
            using (busyBar.Begin("Spec 표 저장 (" + rows.Count + "건)"))
            {
                try
                {
                    int groupNo = 0;
                    foreach (IGrouping<string, EtSpecRow> group in groups)
                    {
                        busyBar.Step(groupNo++, groups.Count);
                        List<EtSpecRow> add = missing[group.Key];
                        if (add.Count > 0)
                        {
                            string[,] reg = new string[add.Count, 4];
                            for (int i = 0; i < add.Count; i++)
                            {
                                reg[i, 0] = add[i].HeaderText;
                                reg[i, 1] = add[i].Unit;
                                reg[i, 2] = string.Empty;
                                reg[i, 3] = string.Empty;
                            }
                            string[] regKey = { factory, device, group.Key, "SPEC", string.Empty };
                            DataTable result = busyBar.Run(group.Key + " 등록", () => Service.RegisterEtParameters(regKey, reg, user));
                            registeredCount += Convert.ToInt32(result.Rows[0]["REGISTERED"], CultureInfo.InvariantCulture);
                        }
                        List<EtSpecRow> list = group.ToList();
                        string[,] data = new string[list.Count, 7];
                        for (int i = 0; i < list.Count; i++)
                        {
                            data[i, 0] = names[list[i]];
                            data[i, 1] = list[i].Lower;
                            data[i, 2] = list[i].Upper;
                            data[i, 3] = "Y";
                            data[i, 4] = list[i].Unit;
                            data[i, 5] = list[i].SpecText;
                            data[i, 6] = string.Empty;
                        }
                        string[] saveKey = { factory, device, group.Key };
                        busyBar.Run(group.Key + " Spec 저장", () => Service.SaveEtLimit(saveKey, data, user));
                    }
                    busyBar.Step(groups.Count, groups.Count);
                }
                catch (Exception ex)
                {
                    SetStatus("Spec 표 저장 실패 / " + Cause(ex).Message);
                    Notify("Spec 표를 저장하지 못했습니다. 앞 공정은 저장됐을 수 있습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
                    return;
                }
                string program = EtParameterNameRule.ToTestProgram(cboTestProgram.Text);
                if (program.Length == 0 || !groups.Any(g => g.Key == program))
                    program = groups[0].Key;
                string rejudge = AskRejudge(factory, device, groups.Select(g => g.Key));
                LoadDeviceCombo();
                filling = true;
                cboDevice.Text = device;
                cboTestProgram.Text = program;
                filling = false;
                Search();
                SetStatus("Spec 표 저장 완료 / " + summary + " / 새로 등록한 Parameter " + registeredCount + " / Device " + device + " / 지금 공정 " + program + rejudge);
            }
        }

        /// <summary>
        /// Spec 을 바꾼 뒤: 그 Device + 공정의 READY 업로드 파일이 있으면 지금 Spec 으로 다시 판정할지 묻는다(RejudgeEtFiles, Upload 재설계 9.6절).
        /// Spec 에 새로 넣은 Parameter 는 저장된 값이 없으면 판정되지 않는다(파일을 다시 업로드해야 한다). 반환: 상태 줄에 덧붙일 글(없으면 빈 값)
        /// </summary>
        private string AskRejudge(string factory, string device, IEnumerable<string> programs)
        {
            DataTable files;
            try
            {
                files = busyBar.Run("업로드 파일 확인", () => Service.GetEtFileList(factory, device));
            }
            catch (Exception ex)
            {
                return " / 업로드 파일 확인 실패: " + Cause(ex).Message;
            }
            List<string> done = new List<string>();
            foreach (string program in programs.Distinct(StringComparer.Ordinal))
            {
                int count = files.Rows.Cast<DataRow>().Count(r => r["TEST_PROGRAM"].ToString() == program && r["LOAD_STATUS"].ToString() == "READY");
                if (count == 0) continue;
                if (Ask("Device " + device + " / 공정 " + program + " 의 업로드 파일 " + count + "개를 바뀐 Spec 으로 다시 판정할까요?\r\n\r\n"
                    + "Spec 에 새로 넣은 Parameter 의 값은 파일을 다시 업로드해야 저장됩니다(등록 전에 올린 파일에는 값이 없음).", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    continue;
                try
                {
                    string[] key = { factory, device, program };
                    string user = User();
                    int judged = busyBar.Run("다시 판정 (" + program + " " + count + "개)", () => Service.RejudgeEtFiles(key, user));
                    done.Add(program + " " + judged + "개");
                }
                catch (Exception ex)
                {
                    done.Add(program + " 실패(" + Cause(ex).Message + ")");
                }
            }
            return done.Count == 0 ? string.Empty : " / 다시 판정: " + string.Join(", ", done.ToArray());
        }

        #endregion

        #region [GDS Setup Chain 목록 (오른쪽)]

        /// <summary>
        /// 위에서 고른 E-Test Device 이름(= GDS Device)의 Setup Chain 목록(ACTIVE + 현재 READY 버전)을 읽는다. 그 이름의 GDS 가 없으면 빈 목록 그대로.
        /// 사용 수는 RefreshGdsUse 가 채운다.
        /// </summary>
        private void LoadGdsChains()
        {
            gdsNames.Clear();
            dgvGds.Rows.Clear();
            string factory = loadedFactory;
            string gdsDevice = loadedDevice;
            loadedGdsDevice = gdsDevice;
            txtGdsDevice.Text = gdsDevice;
            if (factory.Length == 0 || gdsDevice.Length == 0) return;
            DataTable dt;
            try
            {
                dt = busyBar.RunHere("GDS Setup Chain 조회", () => Service.GetEtGdsChainList(factory, gdsDevice));
            }
            catch (Exception ex)
            {
                SetStatus("GDS Setup Chain 조회 실패: " + Cause(ex).Message);
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["CHAIN_NAME"].ToString();
                gdsNames.Add(name);
                dgvGds.Rows.Add(dr["CHAIN_CODE"], name, "0");
            }
            dgvGds.ClearSelection();
        }

        /// <summary>GDS Setup Chain 목록의 사용 수(지금 표에서 그 이름을 쓰는 Parameter 수, 대소문자 무시)와 안내 줄을 갱신한다.</summary>
        private void RefreshGdsUse()
        {
            Dictionary<string, int> use = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (DataGridViewRow row in dgvPara.Rows)
            {
                string name = CellText(row, colPGds);
                if (name.Length == 0) continue;
                int n;
                use.TryGetValue(name, out n);
                use[name] = n + 1;
            }
            foreach (DataGridViewRow row in dgvGds.Rows)
            {
                int n;
                use.TryGetValue(CellText(row, colGName), out n);
                row.Cells[colGUse.Index].Value = n.ToString(CultureInfo.InvariantCulture);
                row.DefaultCellStyle.ForeColor = n > 0 ? Color.Empty : Color.Gray;
            }
            lblGdsInfo.Text = loadedGdsDevice.Length == 0 ? "GDS Setup Chain" : "GDS Setup Chain " + dgvGds.Rows.Count + "개";
        }

        private void dgvGds_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) CopyGdsName(dgvGds.Rows[e.RowIndex]);
        }

        /// <summary>Ctrl+C 는 기본 복사(행 전체) 대신 Chain 이름만 복사한다.</summary>
        private void dgvGds_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control || (e.KeyCode != Keys.C && e.KeyCode != Keys.Insert)) return;
            DataGridViewRow row = SelectedRows(dgvGds).FirstOrDefault();
            if (row != null) CopyGdsName(row);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void mnuGdsCopy_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = SelectedRows(dgvGds).FirstOrDefault();
            if (row != null) CopyGdsName(row);
        }

        private void CopyGdsName(DataGridViewRow row)
        {
            string name = CellText(row, colGName);
            if (name.Length == 0) return;
            SetClipboardText(name);
            SetStatus("복사: " + name);
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

        private static bool Checked(DataGridViewRow row, DataGridViewColumn column)
        {
            object value = row.Cells[column.Index].Value;
            return value is bool && (bool)value;
        }

        private static void Toggle(DataGridViewCell cell)
        {
            cell.Value = !(cell.Value is bool && (bool)cell.Value);
        }

        /// <summary>클립보드 글자의 첫 칸(탭 / 줄바꿈 앞). Excel 에서 칸 하나를 복사해도 끝 줄바꿈이 붙는다.</summary>
        private static string FirstCellText(string text)
        {
            string value = text ?? string.Empty;
            int cut = value.IndexOfAny(new[] { '\t', '\r', '\n' });
            return (cut >= 0 ? value.Substring(0, cut) : value).Trim();
        }

        private static bool TryNumber(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>두 글자가 같거나 둘 다 같은 숫자인지.</summary>
        private static bool SameNumber(string a, string b)
        {
            double x, y;
            return a == b || (TryNumber(a, out x) && TryNumber(b, out y) && x == y);
        }

        /// <summary>DB 숫자(NULL 이면 빈 문자열)를 형식에 맞춰 문자열로 바꾼다.</summary>
        private static string Number(object value, string format)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>조건 표시: PARA_NAME 에서 측정 종류를 뺀 나머지 + '=' 조건 값 (예: IFORCE=8.0MA / SERIAL).</summary>
        private static string ConditionText(string paraName, string measType, string condValue)
        {
            string rest = paraName.Length > measType.Length && paraName.StartsWith(measType + "_", StringComparison.Ordinal)
                ? paraName.Substring(measType.Length + 1) : string.Empty;
            if (condValue.Length == 0) return rest;
            return rest.Length > 0 ? rest + "=" + condValue : condValue;
        }

        private static string SourceText(string source)
        {
            return source == "FILE" ? "결과 파일" : source == "SPEC" ? "Spec 표" : source;
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

        /// <summary>클립보드 텍스트. 화면 Smoke 검사는 Spec 표 파일 내용 / 정해진 글자를 돌려주도록 바꾼다.</summary>
        protected virtual string ClipboardText()
        {
            return Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
        }

        /// <summary>클립보드에 글자 넣기(GDS Setup Chain 이름 복사). 화면 Smoke 검사는 기록만 한다.</summary>
        protected virtual void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                SetStatus("클립보드에 복사하지 못했습니다: " + ex.Message);
            }
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

        /// <summary>등록 Parameter 행의 저장 이름 + 조회 당시 값(수정 비교용) + 표시 정보.</summary>
        private sealed class ParaState
        {
            public string Name;
            public bool Use;
            public string Gds;
            public string Lower;
            public string Upper;
            public bool Enable;
            public string Remark;
            public bool HasSpec;
            /// <summary>Spec 이 없던 행에 Limit 을 넣어 Enable 을 한 번 자동으로 켰음</summary>
            public bool AutoEnabled;
        }

        #endregion
    }
}
