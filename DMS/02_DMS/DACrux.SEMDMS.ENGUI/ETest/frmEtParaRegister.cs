using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test Para 등록 화면이다. (2026-10-14, E-Test Para 설정 화면의 [파일 헤더] 탭을 화면으로 분리)
    /// 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md, 등록 / 이름 변환 규칙은 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md 5절
    /// 기준:
    ///  - Factory 만 입력하고 Device / 공정 / 파일은 결과 파일에서 가져온다(읽기 전용).
    ///  - 이 화면은 DB 에 등록(RegisterEtParameters, 출처 FILE)만 한다. GDS Chain / Spec / 사용 / 삭제는 E-Test Para 설정 화면에서 한다.
    ///  - 저장 이름은 DB / 소스에서 그대로 쓰도록 바꾼 이름(EtParameterNameRule)이고 헤더 원문도 보관한다.
    /// 처리 흐름:
    ///  1) [결과 파일 열기]: xlsx 를 EtResultFileParser 로 읽고 파일명의 Device / 공정으로 등록 Parameter(GetEtParameterList)를 읽어 헤더마다 상태를 계산한다.
    ///     상태: 신규 / 등록됨 / 이름 충돌 / 단위 다름 / 오류 / 중복. 등록된 헤더는 GDS Chain / Spec 을 참고로 보인다.
    ///  2) 필요 없는 헤더 행은 드래그로 골라 우클릭 / Del 로 목록에서 뺀다(화면에서만, 같은 파일을 다시 열 때까지 유지).
    ///  3) 신규 헤더를 체크하고 [등록] -> 다시 읽어 상태를 바꾼다.
    ///  4) [Device 삭제](2026-10-22): 등록 Device 콤보에서 고른 Device 의 업로드 파일(측정값 / Die / YMS 반영 포함)과 모든 공정의 등록 Parameter / Spec 을 지운다.
    ///     확인 창에 공정별 Parameter / Spec / 업로드 파일 수를 보여 주고 [예]일 때만 지운다. GDS Map / Chain, KLARF 데이터는 지우지 않는다.
    /// 진행 표시: 서버 호출과 파일 읽기는 화면 아래 busyBar(ProgressStatusBar)가 별도 스레드에서 돌리고 작업 이름과 모래시계 커서를 보여 준다.
    /// </summary>
    public partial class frmEtParaRegister : DACrux.Framework.Base.DACruxUXBasic01
    {
        private static readonly Color DirtyColor = Color.LightYellow;
        private static readonly Color ErrorColor = Color.MistyRose;
        private static readonly Color DoneColor = Color.Gainsboro;
        private const string AllText = "(전체)";

        private RO.GdsEtResult paraService;
        private string loadedFactory = string.Empty;
        private bool filling;
        private EtResultFile currentFile;
        /// <summary>헤더 표에서 목록에서 뺀 행(currentFile.Items 위치). 화면에서만 숨기며 파일을 새로 열면 비운다.</summary>
        private readonly HashSet<int> removedItems = new HashSet<int>();
        private string fileInfoText = string.Empty;
        /// <summary>등록 Parameter: 저장 이름 -> (헤더 원문, 등록 단위, GDS Chain, Spec 요약)</summary>
        private readonly Dictionary<string, string[]> registered = new Dictionary<string, string[]>(StringComparer.Ordinal);

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtParaRegister()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            colHTestNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colHCheck.HeaderCell.Style.Font = new Font(dgvHeader.Font, FontStyle.Bold);
            // 행 머리글 = 결과 파일의 측정 열 순서. 오른쪽 / 가운데 정렬은 자릿수에 따라 위치가 달라져서 기본 왼쪽 정렬을 쓴다.
            dgvHeader.TopLeftHeaderCell.Value = "열";
            // 파일 이름 아래 규칙 = 파일을 열 때 나누는 규칙(EtResultFileParser.FileNameRule)과 같은 글자 (2026-10-21)
            lblFileRule.Text = EtResultFileParser.FileNameRule;
        }

        /// <summary>E-Test 서비스. 화면 Smoke 검사는 이 필드에 BSL 을 붙인 RO 를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return paraService ?? (paraService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 를 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            LoadDeviceList();
        }

        #region [파일 열기 / 상태]

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string path = ChooseFile();
            if (string.IsNullOrEmpty(path)) return;
            using (busyBar.Begin("결과 파일 열기"))
                OpenResultFile(path);
        }

        /// <summary>
        /// 결과 파일을 읽어 헤더를 등록 후보로 보여 준다.
        /// 처리 흐름: EtResultFileParser.Parse -> 파일명의 Device / 공정을 읽기 전용 칸에 표시 -> 등록 Parameter 조회 -> 헤더 분해 / 상태. 목록에서 뺀 행은 비운다.
        /// </summary>
        private void OpenResultFile(string path)
        {
            if (txtFactory.Text.Trim().Length == 0)
            {
                Notify("Factory 를 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            EtResultFile file;
            try
            {
                SetStatus("파일을 읽는 중: " + Path.GetFileName(path));
                file = busyBar.Run("파일 읽기", () => EtResultFileParser.Parse(path));
            }
            catch (Exception ex)
            {
                Notify("결과 파일을 읽지 못했습니다: " + Cause(ex).Message, MessageBoxIcon.Error);
                return;
            }
            string device = (file.FileDevice ?? string.Empty).Trim();
            string program = EtParameterNameRule.ToTestProgram(file.TestProgram);
            if (device.Length == 0 || program.Length == 0)
            {
                Notify("파일 이름에서 Device / 공정(Test Type)을 찾지 못했습니다: " + file.FileName, MessageBoxIcon.Warning);
                return;
            }
            currentFile = file;
            removedItems.Clear();
            txtDevice.Text = device;
            txtTestProgram.Text = program;
            txtFileName.Text = file.FileName;
            if (LoadRegistered())
                BuildHeaderRows();
            else
                dgvHeader.Rows.Clear();
            LoadDeviceList(device);
        }

        /// <summary>Factory 를 바꾸면 열린 파일의 등록 상태와 등록 Device 목록을 다시 읽는다.</summary>
        private void txtFactory_Leave(object sender, EventArgs e)
        {
            if (string.Equals(txtFactory.Text.Trim(), loadedFactory, StringComparison.Ordinal)) return;
            using (busyBar.Begin("등록 상태 다시 계산"))
            {
                if (currentFile != null && LoadRegistered())
                    BuildHeaderRows();
                LoadDeviceList();
            }
        }

        /// <summary>지금 Factory + 파일 Device + 공정의 등록 Parameter 를 읽어 상태 계산용 사전을 만든다. 실패하면 안내하고 false.</summary>
        private bool LoadRegistered()
        {
            registered.Clear();
            string factory = txtFactory.Text.Trim(), device = txtDevice.Text, program = txtTestProgram.Text;
            DataTable dt;
            try
            {
                dt = busyBar.Run("등록 Parameter 조회", () => Service.GetEtParameterList(factory, device, program));
            }
            catch (Exception ex)
            {
                Notify("등록 Parameter 조회 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                return false;
            }
            loadedFactory = factory;
            foreach (DataRow dr in dt.Rows)
            {
                bool hasSpec = dr["HAS_SPEC"].ToString() == "Y";
                string spec = hasSpec
                    ? Number(dr["LOWER_LIMIT"]) + " ~ " + Number(dr["UPPER_LIMIT"]) + (dr["ENABLE_FLAG"].ToString() == "Y" ? string.Empty : " (Off)")
                    : string.Empty;
                registered[dr["ITEM_NAME"].ToString()] = new string[] { dr["HEADER_TEXT"].ToString(), dr["UNIT"].ToString(), dr["GDS_CHAIN_NAME"].ToString(), spec };
            }
            return true;
        }

        /// <summary>헤더마다 분해하고 등록 여부 / 단위 / 이름 충돌 / GDS Chain / Spec 을 계산해 표를 채운다. 필터 콤보도 다시 만든다.</summary>
        private void BuildHeaderRows()
        {
            dgvHeader.Rows.Clear();
            if (currentFile == null || currentFile.Items == null) return;
            int count = currentFile.Items.GetLength(0);
            List<EtParameterParts> parts = new List<EtParameterParts>(count);
            Dictionary<string, int> nameCount = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < count; i++)
            {
                EtParameterParts p = EtParameterNameRule.Parse(currentFile.Items[i, 0]);
                parts.Add(p);
                if (p.IsValid)
                {
                    int n;
                    nameCount.TryGetValue(p.ParameterName, out n);
                    nameCount[p.ParameterName] = n + 1;
                }
            }

            int newCount = 0, doneCount = 0, badCount = 0;
            SortedSet<string> measures = new SortedSet<string>(StringComparer.Ordinal);
            SortedSet<string> types = new SortedSet<string>(StringComparer.Ordinal);
            filling = true;
            dgvHeader.SuspendLayout();
            try
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>(count);
                for (int i = 0; i < count; i++)
                {
                    EtParameterParts p = parts[i];
                    string unit = currentFile.Items[i, 5];
                    string status;
                    string message = p.Warning;
                    string gds = string.Empty, spec = string.Empty;
                    if (!p.IsValid)
                    {
                        status = "오류";
                        message = p.Error;
                    }
                    else if (nameCount[p.ParameterName] > 1)
                    {
                        status = "중복";
                        message = "파일 안에서 같은 저장 이름이 " + nameCount[p.ParameterName] + "번 나옵니다.";
                    }
                    else
                    {
                        string[] old;
                        if (!registered.TryGetValue(p.ParameterName, out old))
                            status = "신규";
                        else if (!string.Equals(old[0], p.HeaderText, StringComparison.OrdinalIgnoreCase))
                        {
                            status = "이름 충돌";
                            message = "다른 헤더가 같은 저장 이름으로 등록됨: " + old[0];
                        }
                        else
                        {
                            gds = old[2];
                            spec = old[3];
                            if (old[1].Length > 0 && unit.Length > 0 && !string.Equals(old[1], unit, StringComparison.Ordinal))
                            {
                                status = "단위 다름";
                                message = "등록 단위 " + old[1] + " / 파일 단위 " + unit;
                            }
                            else
                                status = "등록됨";
                        }
                    }
                    if (status == "신규") newCount++;
                    else if (status == "등록됨") doneCount++;
                    else badCount++;
                    if (p.MeasType.Length > 0) measures.Add(p.MeasType);
                    if (p.ChainType.Length > 0) types.Add(p.ChainType);

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvHeader, false, status, p.HeaderText, p.ParameterName, p.MeasType, p.Condition,
                        p.Chain1, p.ForceLeg1, p.Chain2, p.ForceLeg2, unit, currentFile.Items[i, 6], gds, spec, message);
                    row.HeaderCell.Value = currentFile.Items[i, 7];
                    row.Tag = new HeaderRow(i, p, status);
                    if (status == "등록됨")
                    {
                        row.DefaultCellStyle.BackColor = DoneColor;
                        row.Cells[colHCheck.Index].ReadOnly = true;
                    }
                    else if (status == "단위 다름")
                    {
                        row.DefaultCellStyle.BackColor = DirtyColor;
                        row.Cells[colHCheck.Index].ReadOnly = true;
                    }
                    else if (status != "신규")
                    {
                        row.DefaultCellStyle.BackColor = ErrorColor;
                        row.Cells[colHCheck.Index].ReadOnly = true;
                    }
                    rows.Add(row);
                    busyBar.Pulse();
                }
                dgvHeader.Rows.AddRange(rows.ToArray());
            }
            finally
            {
                dgvHeader.ResumeLayout();
                filling = false;
            }
            FillFilterCombo(cboMeasFilter, measures);
            FillFilterCombo(cboTypeFilter, types);
            fileInfoText = "헤더 " + count + " / 신규 " + newCount + " / 등록됨 " + doneCount + " / 등록 불가 " + badCount;
            ApplyHeaderFilter();
        }

        private void FillFilterCombo(ComboBox combo, IEnumerable<string> values)
        {
            bool wasFilling = filling;
            filling = true;
            try
            {
                combo.Items.Clear();
                combo.Items.Add(AllText);
                foreach (string value in values) combo.Items.Add(value);
                combo.SelectedIndex = 0;
            }
            finally
            {
                filling = wasFilling;
            }
        }

        #endregion

        #region [거르기 / 목록에서 빼기 / 체크]

        private void headerFilter_Changed(object sender, EventArgs e)
        {
            if (!filling) ApplyHeaderFilter();
        }

        /// <summary>목록에서 뺀 행과 측정 종류 / Chain 종류 / 글자 / 신규만 조건에 맞지 않는 헤더 행을 숨긴다(체크 상태는 유지).</summary>
        private void ApplyHeaderFilter()
        {
            string measure = cboMeasFilter.SelectedIndex > 0 ? cboMeasFilter.Text : string.Empty;
            string type = cboTypeFilter.SelectedIndex > 0 ? cboTypeFilter.Text : string.Empty;
            string find = txtFind.Text.Trim();
            int visible = 0;
            dgvHeader.CurrentCell = null;
            foreach (DataGridViewRow row in dgvHeader.Rows)
            {
                HeaderRow h = (HeaderRow)row.Tag;
                bool show = !removedItems.Contains(h.Index)
                    && (measure.Length == 0 || h.Parts.MeasType == measure)
                    && (type.Length == 0 || h.Parts.ChainType == type)
                    && (!chkNewOnly.Checked || h.Status == "신규")
                    && (find.Length == 0 || h.Parts.HeaderText.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0
                        || h.Parts.ParameterName.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0);
                row.Visible = show;
                if (show) visible++;
            }
            int listed = dgvHeader.Rows.Count - removedItems.Count;
            //lblFileInfo.Text = fileInfoText + (removedItems.Count > 0 ? " / 목록에서 뺌 " + removedItems.Count : string.Empty);
            SetStatus("헤더 " + listed + "개 중 " + visible + "개 표시 / 체크 " + CheckedHeaderRows().Count + "개");
        }

        /// <summary>우클릭한 행이 선택에 없으면 그 행만 고른다. 드래그로 고른 여러 행 위에서 우클릭하면 선택을 그대로 둔다.</summary>
        private void dgvHeader_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0) return;
            DataGridViewRow row = dgvHeader.Rows[e.RowIndex];
            if (row.Selected) return;
            dgvHeader.ClearSelection();
            row.Selected = true;
            dgvHeader.CurrentCell = row.Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : colHHeader.Index];
        }

        private void ctxHeader_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int selected = SelectedRows(dgvHeader).Count;
            mnuHeaderDelete.Text = "선택 행 목록에서 삭제 (" + selected + ")";
            mnuHeaderDelete.Enabled = selected > 0;
            mnuHeaderRestore.Text = "삭제한 행 모두 되살리기 (" + removedItems.Count + ")";
            mnuHeaderRestore.Enabled = removedItems.Count > 0;
            e.Cancel = currentFile == null;
        }

        private void dgvHeader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || dgvHeader.IsCurrentCellInEditMode) return;
            RemoveSelectedHeaderRows();
            e.Handled = true;
        }

        private void mnuHeaderDelete_Click(object sender, EventArgs e)
        {
            RemoveSelectedHeaderRows();
        }

        /// <summary>
        /// 고른 헤더 행을 등록 후보 목록에서 뺀다. DB 와 상관없이 화면에서만 숨기고 체크도 푼다.
        /// 표를 다시 만들지 않고 숨기기만 하므로 다른 행의 체크는 그대로다. 같은 파일을 보는 동안([등록] 뒤에도) 유지된다.
        /// </summary>
        private void RemoveSelectedHeaderRows()
        {
            List<DataGridViewRow> rows = SelectedRows(dgvHeader);
            if (rows.Count == 0) return;
            dgvHeader.EndEdit();
            foreach (DataGridViewRow row in rows)
            {
                removedItems.Add(((HeaderRow)row.Tag).Index);
                row.Cells[colHCheck.Index].Value = false;
            }
            dgvHeader.ClearSelection();
            ApplyHeaderFilter();
            SetStatus(lblStatus.Text + " / 목록에서 " + rows.Count + "행 뺌");
        }

        private void mnuHeaderRestore_Click(object sender, EventArgs e)
        {
            int count = removedItems.Count;
            removedItems.Clear();
            ApplyHeaderFilter();
            SetStatus(lblStatus.Text + " / " + count + "행 되살림");
        }

        private void btnCheckVisible_Click(object sender, EventArgs e)
        {
            dgvHeader.EndEdit();
            foreach (DataGridViewRow row in dgvHeader.Rows)
                if (row.Visible && ((HeaderRow)row.Tag).Status == "신규")
                    row.Cells[colHCheck.Index].Value = true;
            SetStatus("체크 " + CheckedHeaderRows().Count + "개");
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            dgvHeader.EndEdit();
            foreach (DataGridViewRow row in dgvHeader.Rows)
                row.Cells[colHCheck.Index].Value = false;
            SetStatus("체크 0개");
        }

        /// <summary>체크박스는 클릭 즉시 값이 반영되도록 바로 확정한다.</summary>
        private void dgvHeader_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvHeader.IsCurrentCellDirty && dgvHeader.CurrentCell is DataGridViewCheckBoxCell)
                dgvHeader.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private List<DataGridViewRow> CheckedHeaderRows()
        {
            return dgvHeader.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells[colHCheck.Index].Value is bool && (bool)r.Cells[colHCheck.Index].Value
                    && ((HeaderRow)r.Tag).Status == "신규" && !removedItems.Contains(((HeaderRow)r.Tag).Index)).ToList();
        }

        #endregion

        #region [등록]

        /// <summary>체크한 신규 헤더를 파일의 Device + 공정에 등록한다(출처 FILE + 파일 이름). 끝나면 다시 읽어 상태를 바꾼다.</summary>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            dgvHeader.EndEdit();
            List<DataGridViewRow> rows = CheckedHeaderRows();
            if (currentFile == null || rows.Count == 0)
            {
                Notify("등록할 신규 헤더를 체크하세요. (파일이 없으면 [결과 파일 열기])", MessageBoxIcon.Warning);
                return;
            }
            string factory = loadedFactory, device = txtDevice.Text, program = txtTestProgram.Text;
            if (Ask("Factory " + factory + " / " + device + " / " + program + " 에 Parameter " + rows.Count + "개를 등록합니다.", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            string[,] data = new string[rows.Count, 4];
            for (int i = 0; i < rows.Count; i++)
            {
                int k = ((HeaderRow)rows[i].Tag).Index;
                data[i, 0] = currentFile.Items[k, 0];
                data[i, 1] = currentFile.Items[k, 5];
                data[i, 2] = currentFile.Items[k, 6];
                data[i, 3] = currentFile.Items[k, 7];
            }
            string[] key = { factory, device, program, "FILE", currentFile.FileName };
            string user = User();
            using (busyBar.Begin("등록 (" + rows.Count + "개)"))
            {
                string done;
                try
                {
                    DataRow r = busyBar.Run("서버 저장", () => Service.RegisterEtParameters(key, data, user)).Rows[0];
                    done = "등록 " + r["REGISTERED"] + "개 / 이미 있어 건너뜀 " + r["SKIPPED"] + "개";
                }
                catch (Exception ex)
                {
                    Notify("등록 실패: " + Cause(ex).Message, MessageBoxIcon.Error);
                    return;
                }
                if (LoadRegistered())
                    BuildHeaderRows();
                LoadDeviceList(device);
                SetStatus(done);
            }
        }

        #endregion

        #region [Device 삭제]

        /// <summary>
        /// 등록 Device 콤보를 채운다: Factory 의 등록 Parameter 가 있는 Device 이름만(2026-10-22 공정 / Parameter 수 표시 뺌).
        /// select 가 있으면 그 Device 를, 없으면 지금 고른 Device 를 다시 고른다(목록에 없으면 고르지 않음).
        /// </summary>
        private void LoadDeviceList(string select = null)
        {
            string factory = txtFactory.Text.Trim();
            string wanted = select ?? SelectedDevice();
            List<string> devices = new List<string>();
            if (factory.Length > 0)
            {
                try
                {
                    DataTable dt = busyBar.RunHere("등록 Device 목록", () => Service.GetEtParaProgramList(factory));
                    devices = dt.Rows.Cast<DataRow>().Select(r => r["DEVICE_ID"].ToString()).Distinct(StringComparer.Ordinal)
                        .OrderBy(d => d, StringComparer.Ordinal).ToList();
                }
                catch (Exception ex)
                {
                    SetStatus("등록 Device 목록 조회 실패: " + Cause(ex).Message);
                }
            }
            cboRegDevice.BeginUpdate();
            cboRegDevice.Items.Clear();
            cboRegDevice.Items.AddRange(devices.Cast<object>().ToArray());
            cboRegDevice.EndUpdate();
            cboRegDevice.SelectedItem = devices.FirstOrDefault(d => string.Equals(d, wanted, StringComparison.OrdinalIgnoreCase));
            btnDeleteDevice.Enabled = devices.Count > 0;
        }

        /// <summary>콤보를 펼칠 때 목록을 다시 읽는다(다른 화면에서 등록 / 삭제한 것 반영).</summary>
        private void cboRegDevice_DropDown(object sender, EventArgs e)
        {
            LoadDeviceList();
        }

        private string SelectedDevice()
        {
            return cboRegDevice.SelectedItem as string ?? string.Empty;
        }

        /// <summary>
        /// [Device 삭제]: 고른 Device 의 업로드 파일(측정값 / Die / YMS 반영 포함)과 모든 공정의 등록 Parameter / Spec 을 지운다.
        /// 처리 흐름: 지울 내용 조회(공정별 Parameter / Spec 수, 업로드 파일) -> 확인(예 / 아니요) -> 업로드 파일마다 DeleteEtFile -> 공정마다 DeleteEtParameters.
        /// 서버가 측정값이 있는 Parameter 는 지우지 않으므로 파일을 먼저 지운다. 중간에 실패하면 거기서 멈추고 그때까지 지운 수를 알린다(다시 누르면 남은 것부터).
        /// GDS Map / Chain, KLARF 데이터는 지우지 않는다. 열어 둔 파일이 이 Device 면 등록 상태를 다시 계산한다(등록됨 -> 신규).
        /// </summary>
        private void btnDeleteDevice_Click(object sender, EventArgs e)
        {
            string factory = txtFactory.Text.Trim();
            string device = SelectedDevice();
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("지울 등록 Device 를 고르세요.", MessageBoxIcon.Warning);
                return;
            }

            DataTable files;
            List<string> programs;
            Dictionary<string, string[]> names = new Dictionary<string, string[]>(StringComparer.Ordinal);
            Dictionary<string, int> specs = new Dictionary<string, int>(StringComparer.Ordinal);
            try
            {
                files = busyBar.RunHere("업로드 파일 확인", () => Service.GetEtFileList(factory, device));
                programs = busyBar.RunHere("등록 공정 확인", () => Service.GetEtParaProgramList(factory)).Rows.Cast<DataRow>()
                    .Where(r => string.Equals(r["DEVICE_ID"].ToString(), device, StringComparison.Ordinal))
                    .Select(r => r["TEST_PROGRAM"].ToString()).ToList();
                foreach (string program in programs)
                {
                    DataTable dt = busyBar.RunHere(program + " Parameter 확인", () => Service.GetEtParameterList(factory, device, program));
                    names[program] = dt.Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()).ToArray();
                    specs[program] = dt.Rows.Cast<DataRow>().Count(r => r["HAS_SPEC"].ToString() == "Y");
                }
            }
            catch (Exception ex)
            {
                Notify("지울 내용을 조회하지 못했습니다: " + Cause(ex).Message, MessageBoxIcon.Error);
                return;
            }
            if (programs.Count == 0 && files.Rows.Count == 0)
            {
                Notify("Device " + device + " 에 등록 Parameter / 업로드 파일이 없습니다.", MessageBoxIcon.Information);
                LoadDeviceList();
                return;
            }

            Dictionary<string, int> fileCount = files.Rows.Cast<DataRow>().GroupBy(r => r["TEST_PROGRAM"].ToString(), StringComparer.Ordinal)
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
            List<string> lines = programs.Union(fileCount.Keys, StringComparer.Ordinal).OrderBy(p => p, StringComparer.Ordinal).Select(p =>
            {
                string[] list;
                int spec, count;
                names.TryGetValue(p, out list);
                specs.TryGetValue(p, out spec);
                fileCount.TryGetValue(p, out count);
                return "  " + p + ": Parameter " + (list == null ? 0 : list.Length) + " / Spec " + spec + " / 업로드 파일 " + count;
            }).ToList();
            if (Ask("Factory " + factory + " 의 Device " + device + " 를 삭제합니다.\r\n\r\n" + string.Join("\r\n", lines.ToArray())
                + "\r\n\r\n업로드 파일 " + files.Rows.Count + "개는 측정값 / Die / YMS 반영까지 같이 지웁니다. GDS Map / Chain, KLARF 데이터는 지우지 않습니다."
                + "\r\n되돌릴 수 없습니다. 계속할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string user = User();
            int fileDone = 0, paraDone = 0;
            using (busyBar.Begin("Device 삭제 (" + device + ")"))
            {
                try
                {
                    int step = 0, steps = files.Rows.Count + programs.Count;
                    foreach (DataRow f in files.Rows)
                    {
                        busyBar.Step(step++, steps);
                        string seq = f["SRC_FILE_SEQ"].ToString();
                        busyBar.Run("업로드 파일 삭제 (" + f["LOT_ID"] + " / Slot " + f["SLOT_NO"] + ")", () => Service.DeleteEtFile(seq, user));
                        fileDone++;
                    }
                    foreach (string program in programs)
                    {
                        busyBar.Step(step++, steps);
                        string[] list = names[program];
                        if (list.Length == 0) continue;
                        string[] key = { factory, device, program };
                        paraDone += busyBar.Run(program + " Parameter 삭제", () => Service.DeleteEtParameters(key, list, user));
                    }
                    busyBar.Step(steps, steps);
                }
                catch (Exception ex)
                {
                    string done = "업로드 파일 " + fileDone + " / Parameter " + paraDone;
                    SetStatus("Device " + device + " 삭제 중 멈춤 / 지운 것: " + done);
                    Notify("Device 삭제가 중간에 멈췄습니다. 지운 것: " + done + "\r\n다시 누르면 남은 것부터 지웁니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
                    LoadDeviceList();
                    RefreshOpenFile(device);
                    return;
                }
                LoadDeviceList();
                RefreshOpenFile(device);
                SetStatus("Device " + device + " 삭제 / 업로드 파일 " + fileDone + " / Parameter " + paraDone + " (공정 " + programs.Count + ")");
            }
        }

        /// <summary>열어 둔 파일이 지운 Device 면 등록 상태를 다시 계산한다(등록됨 -> 신규).</summary>
        private void RefreshOpenFile(string device)
        {
            if (currentFile == null || !string.Equals(txtDevice.Text, device, StringComparison.OrdinalIgnoreCase)) return;
            if (LoadRegistered())
                BuildHeaderRows();
        }

        #endregion

        #region [공통]

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

        private static string Number(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("G10", CultureInfo.InvariantCulture);
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>결과 파일 고르기. 화면 Smoke 검사는 샘플 파일 경로를 돌려주도록 바꾼다.</summary>
        protected virtual string ChooseFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "E-Test 결과 파일";
                dialog.Filter = "E-Test 결과 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*";
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileName : string.Empty;
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

        /// <summary>헤더 표 1행의 원본 위치 / 분해 결과 / 상태.</summary>
        private sealed class HeaderRow
        {
            public readonly int Index;
            public readonly EtParameterParts Parts;
            public readonly string Status;

            public HeaderRow(int index, EtParameterParts parts, string status)
            {
                Index = index;
                Parts = parts;
                Status = status;
            }
        }

        #endregion
    }
}
