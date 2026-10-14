using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 결과 파일(xlsx)을 읽어 DMS DB(TQP_GDS_SRC_FILE / ET_ITEM / ET_DIE / ET_VALUE)에 저장하는 화면이다.
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절
    /// 처리 흐름:
    ///  1) [찾아보기]: 파일을 고르면 바로 분석한다(EtResultFileParser). 파일 정보(항목별 읽기 전용 칸) / Die 미리보기 / 경고를 보여 준다.
    ///  2) Factory: 로그인 Factory 기본. Device: 고르지 않고 파일명 2칸(Device)을 그대로 DEVICE_ID 로 쓴다(읽기 전용, 2026-10-12).
    ///  2-1) Slot No: 파일에 없어 사용자가 입력한다(파일명 Wafer '{Lot}-{숫자}'면 기본값). WAFER_ID = Lot + '-' + 2자리 Slot 을 미리 보여 준다.
    ///  3) [DB 저장]: 같은 Wafer(Factory + WAFER_ID)가 있으면 묻고 삭제 후 재등록 -> 헤더(LOADING) -> 측정 항목 -> Die -> 측정값(10,000행씩) -> 확정(READY)
    ///     Device 는 중복 허용(같은 Device 에 여러 Lot / Wafer).
    ///  4) 아래 목록: Device에 등록된 파일. [선택 파일 삭제]로 지운다(측정 항목은 남는다).
    /// 저장 도중 실패하면 헤더가 LOADING으로 남는다. 다시 저장하면 기존 파일로 보고 삭제 후 재등록을 묻는다.
    /// </summary>
    public partial class frmEtResultUpload : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>측정값 1회 전송 행 수 (서버 MaxBatchRows와 같음)</summary>
        private const int ValueBatchRows = 10000;

        private EtResultFile parsedFile;
        private RO.GdsEtResult uploadService;
        private bool busy;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtResultUpload()
        {
            InitializeComponent();
            UpdateButtons();
        }

        /// <summary>저장 서비스. 화면 Smoke 검사는 이 필드에 BSL을 붙인 RO를 넣는다.</summary>
        private RO.GdsEtResult Service
        {
            get { return uploadService ?? (uploadService = new RO.GdsEtResult()); }
        }

        /// <summary>화면이 처음 열릴 때 로그인 Factory 를 채운다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (txtFactory.Text.Trim().Length == 0)
                txtFactory.Text = DACrux.Base.GlobalVariable.Factory ?? string.Empty;
            ShowFileInfo(null);
        }

        #region [파일 분석]

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "E-Test 결과 파일 선택";
                dialog.Filter = "E-Test 결과 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*";
                if (txtFile.Text.Length > 0 && Directory.Exists(Path.GetDirectoryName(txtFile.Text)))
                    dialog.InitialDirectory = Path.GetDirectoryName(txtFile.Text);
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtFile.Text = dialog.FileName;
            }
            AnalyzeFile();
        }

        /// <summary>txtFile의 파일을 분석해 요약 / Die 미리보기를 표시한다. 실패하면 이전 분석 결과를 지운다.</summary>
        private void AnalyzeFile()
        {
            parsedFile = null;
            ShowFileInfo(null);
            dgvDie.DataSource = null;
            string path = txtFile.Text.Trim();
            if (path.Length == 0 || !File.Exists(path))
            {
                SetStatus("E-Test 결과 파일(xlsx)을 선택하세요.");
                UpdateButtons();
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                SetStatus("파일 분석 중...");
                parsedFile = EtResultFileParser.Parse(path);
                ShowFileInfo(parsedFile);
                dgvDie.DataSource = BuildDieTable(parsedFile);
                // Slot 번호는 파일에 없어 사용자가 입력한다. 파일명 Wafer에서 얻으면 기본값으로 넣는다 (2026-10-07).
                txtSlot.Text = parsedFile.SlotNo > 0 ? parsedFile.SlotNo.ToString(CultureInfo.InvariantCulture) : string.Empty;
                UpdateWaferPreview();
                LoadFileList();
                SetStatus(string.Format("분석 완료 / Device {0} / Die {1} / 측정 항목 {2} / 측정값 {3:N0}{4} / Factory / Slot No 를 확인한 뒤 [DB 저장]을 누르세요.",
                    parsedFile.FileDevice, parsedFile.DieCount, parsedFile.ItemCount, parsedFile.ValueCount,
                    parsedFile.Warnings.Count > 0 ? " / 경고 " + parsedFile.Warnings.Count + "건" : string.Empty));
            }
            catch (Exception ex)
            {
                SetStatus("파일 읽기 실패 / 파일 내용을 확인하세요.");
                Notify("E-Test 파일을 읽을 수 없습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
                UpdateWaferPreview();
                UpdateButtons();
            }
        }

        private void txtSlot_TextChanged(object sender, EventArgs e)
        {
            UpdateWaferPreview();
        }

        /// <summary>입력한 Slot 번호(1~99). 아니면 0.</summary>
        private int InputSlot()
        {
            int slot;
            return int.TryParse(txtSlot.Text.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out slot) && slot >= 1 && slot <= 99 ? slot : 0;
        }

        /// <summary>저장될 WAFER_ID(Lot + '-' + 2자리 Slot) 미리보기.</summary>
        private void UpdateWaferPreview()
        {
            int slot = InputSlot();
            if (parsedFile == null) lblWafer.Text = "WAFER_ID -";
            else if (slot == 0) lblWafer.Text = "WAFER_ID ? (Slot No 1~99 를 입력하세요)";
            else lblWafer.Text = "WAFER_ID " + parsedFile.LotId + "-" + slot.ToString("00", CultureInfo.InvariantCulture);
            lblWafer.ForeColor = parsedFile != null && slot == 0 ? System.Drawing.Color.Firebrick : System.Drawing.SystemColors.ControlText;
        }

        /// <summary>
        /// 파일 정보 칸을 채운다(문자열 대신 항목별 칸, 2026-10-12). null 이면 모두 비운다.
        /// 파일명 8칸 규칙: [Customer]_[Device]_[Wafer Type]_[Wafer Rev]_[Test Type]_[Test Date]_[Lot]_[Lot-Wafer No]$Fab$ (Device 는 위 Device 칸)
        /// </summary>
        private void ShowFileInfo(EtResultFile f)
        {
            txtDevice.Text = f == null ? string.Empty : f.FileDevice;
            txtInfoCustomer.Text = f == null ? string.Empty : f.Customer;
            txtInfoWaferType.Text = f == null ? string.Empty : f.WaferType;
            txtInfoWaferRev.Text = f == null ? string.Empty : f.WaferRev;
            txtInfoFab.Text = f == null ? string.Empty : (f.FabName.Length > 0 ? f.FabName : "(없음)");
            txtInfoTestType.Text = f == null ? string.Empty : f.TestProgram;
            txtInfoTestDate.Text = f == null ? string.Empty : FormatTestTime(f.TestTime);
            txtInfoLot.Text = f == null ? string.Empty : f.LotId;
            txtInfoLotWafer.Text = f == null ? string.Empty : f.WaferId;
            txtInfoDie.Text = f == null ? string.Empty : f.DieCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoPass.Text = f == null ? string.Empty : (f.DieCount - f.FailDieCount).ToString("N0", CultureInfo.InvariantCulture);
            txtInfoFail.Text = f == null ? string.Empty : f.FailDieCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoSheet.Text = f == null ? string.Empty : f.SheetName;
            txtInfoItems.Text = f == null ? string.Empty : f.ItemCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoValues.Text = f == null ? string.Empty : f.ValueCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoNotTested.Text = f == null ? string.Empty : f.NotTestedCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoEmpty.Text = f == null ? string.Empty : f.EmptyCount.ToString("N0", CultureInfo.InvariantCulture);
            txtInfoFail.ForeColor = f != null && f.FailDieCount > 0 ? System.Drawing.Color.Firebrick : System.Drawing.SystemColors.WindowText;
            txtWarning.Text = f == null ? string.Empty
                : f.Warnings.Count == 0 ? "경고 없음" : "경고 " + f.Warnings.Count + "건: " + string.Join("  /  ", f.Warnings.ToArray());
            txtWarning.ForeColor = f != null && f.Warnings.Count == 0 ? System.Drawing.SystemColors.GrayText : System.Drawing.Color.Firebrick;
        }

        /// <summary>Die 미리보기 표 (Die별 '-' 값 수 포함)</summary>
        private static DataTable BuildDieTable(EtResultFile f)
        {
            DataTable dt = new DataTable("ET_DIE");
            foreach (string name in new[] { "DUT", "X", "Y", "HW Bin", "SW Bin", "Pass/Fail", "Flag", "시험 시간(ms)", "'-' 값" })
                dt.Columns.Add(name, typeof(string));
            for (int i = 0; i < f.DieCount; i++)
            {
                int notTested = 0;
                for (int k = 0; k < f.ItemCount; k++)
                    if (f.Statuses[i, k] == 'N') notTested++;
                dt.Rows.Add("#" + f.Dies[i, 0], f.Dies[i, 1], f.Dies[i, 2], f.Dies[i, 3], f.Dies[i, 4],
                    f.Dies[i, 5] == "P" ? "Pass" : "Fail", f.Dies[i, 6], f.Dies[i, 7], notTested.ToString(CultureInfo.InvariantCulture));
            }
            return dt;
        }

        /// <summary>DB 날짜를 yyyy-MM-dd HH:mm:ss 로 표시한다(PC 지역 설정의 오전 / 오후 표시를 쓰지 않음).</summary>
        private static string FormatDate(object value)
        {
            return value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value);
        }

        private static string FormatTestTime(string time)
        {
            DateTime value;
            return DateTime.TryParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
                ? value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : time;
        }

        #endregion

        #region [Factory / Device / 파일 목록]

        private void txtFactory_Leave(object sender, EventArgs e)
        {
            LoadFileList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadFileList();
        }

        /// <summary>등록 파일 목록 열 제목 (열 이름은 DB 컬럼 그대로 두고 제목만 바꾼다)</summary>
        private static readonly Dictionary<string, string> FileListHeaders = new Dictionary<string, string>
        {
            { "SRC_FILE_SEQ", "파일 번호" }, { "FILE_NAME", "파일명" }, { "LOT_ID", "Lot" }, { "SLOT_NO", "Slot" },
            { "TEST_PROGRAM", "공정 (Test Type)" }, { "TEST_TIME", "Test Date" }, { "DIE_COUNT", "Die" }, { "ITEM_COUNT", "항목" },
            { "VALUE_COUNT", "측정값" }, { "LOAD_STATUS", "상태" }, { "CREATE_USER", "등록자" }, { "CREATE_TIME", "등록 시각" },
            { "CUSTOMER", "Customer" }, { "FILE_DEVICE", "파일 Device" }, { "WAFER_TYPE", "Wafer Type" }, { "WAFER_REV", "Wafer Rev" },
            { "FAB_NAME", "Fab" }, { "PASS_DIE_COUNT", "Pass Die" }, { "FAIL_DIE_COUNT", "Fail Die" }, { "NOT_TESTED_COUNT", "'-' 값" }
        };

        /// <summary>Factory / Device(파일명 Device)에 등록된 E-Test 파일 목록을 조회한다. 파일을 고르기 전에는 Device 가 없어 비어 있다.</summary>
        private void LoadFileList()
        {
            string factory = txtFactory.Text.Trim();
            string device = txtDevice.Text.Trim();
            if (factory.Length == 0 || device.Length == 0)
            {
                dgvFiles.DataSource = null;
                UpdateButtons();
                return;
            }
            try
            {
                dgvFiles.DataSource = Service.GetEtFileList(factory, device);
                foreach (DataGridViewColumn column in dgvFiles.Columns)
                {
                    string header;
                    if (FileListHeaders.TryGetValue(column.Name, out header)) column.HeaderText = header;
                }
                if (dgvFiles.Columns.Contains("CREATE_TIME"))
                    dgvFiles.Columns["CREATE_TIME"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            }
            catch (Exception ex)
            {
                dgvFiles.DataSource = null;
                SetStatus("등록 파일 목록 조회 실패: " + Cause(ex).Message);
            }
            UpdateButtons();
        }

        #endregion

        #region [DB 저장 / 삭제]

        /// <summary>
        /// DB 저장. 처리 흐름은 클래스 설명 3) 참고.
        /// 같은 Wafer 파일(Factory + WAFER_ID = Lot + 입력 Slot)이 있으면 삭제 후 재등록할지 묻는다.
        /// 기준 (2026-10-07): Device 는 중복 허용(같은 Device 에 여러 Lot / Wafer), 중복 방지는 WAFER_ID 기준, Slot 은 사용자 입력.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            EtResultFile f = parsedFile;
            string factory = txtFactory.Text.Trim();
            string device = f == null ? string.Empty : f.FileDevice; // 2026-10-12: 파일명 Device 를 그대로 DEVICE_ID 로 저장
            if (f == null)
            {
                Notify("먼저 E-Test 파일을 선택해 분석하세요.", MessageBoxIcon.Warning);
                return;
            }
            if (factory.Length == 0 || device.Length == 0)
            {
                Notify("Factory 를 입력하세요. (Device 는 파일명의 Device 칸을 그대로 씁니다)", MessageBoxIcon.Warning);
                return;
            }
            int slotNo = InputSlot();
            if (slotNo == 0)
            {
                Notify("Slot No 를 1~99 숫자로 입력하세요. WAFER_ID 는 Lot + '-' + 2자리 Slot 으로 만들어집니다.", MessageBoxIcon.Warning);
                txtSlot.Focus();
                return;
            }
            if (f.Warnings.Count > 0 && Ask("분석 경고가 " + f.Warnings.Count + "건 있습니다.\r\n\r\n" + string.Join("\r\n", f.Warnings.ToArray())
                + "\r\n\r\n그대로 저장할까요?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string user = DACrux.Base.GlobalVariable.UserID;
            if (string.IsNullOrEmpty(user)) user = Environment.UserName;
            string slot = slotNo.ToString(CultureInfo.InvariantCulture);
            string seq = null;
            try
            {
                Cursor = Cursors.WaitCursor;
                SetBusy(true);

                // 1) 같은 Wafer 파일 확인 (Factory + WAFER_ID, Device / 시험 일시는 보지 않음) -> 삭제 후 재등록
                // 키 = Factory + WAFER_ID + 공정(Test Type) (2026-10-12: TSVET -> COWET -> FT 는 같은 Wafer 라도 따로 저장)
                DataTable exist = Service.GetEtFileByKey(new string[] { factory, f.LotId, slot, f.TestProgram });
                if (exist.Rows.Count > 0)
                {
                    DataRow dr = exist.Rows[0];
                    string text = "같은 Wafer / 같은 공정의 E-Test 파일이 이미 등록되어 있습니다.\r\n\r\n"
                        + "WAFER_ID " + dr["WAFER_ID"] + " / 공정 " + dr["TEST_PROGRAM"] + " / Device " + dr["DEVICE_ID"] + " / 시험 일시 " + FormatTestTime(dr["TEST_TIME"].ToString()) + "\r\n"
                        + "새 파일: Device " + device + " / 시험 일시 " + FormatTestTime(f.TestTime) + "\r\n"
                        + "파일 번호 " + dr["SRC_FILE_SEQ"] + " / 상태 " + dr["LOAD_STATUS"] + " / 등록 " + dr["CREATE_USER"] + " " + FormatDate(dr["CREATE_TIME"]) + "\r\n\r\n"
                        + "기존 데이터를 삭제하고 다시 저장할까요?";
                    if (Ask(text, MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        SetStatus("저장 취소 / 기존 파일을 그대로 둡니다.");
                        return;
                    }
                    SetStatus("기존 파일 삭제 중...");
                    Service.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), user);
                }

                // 2) 헤더 (LOADING)
                DataTable head = Service.BeginEtFile(new string[] {
                    factory, device, f.FileName, f.LotId, slot, f.TestProgram, f.TestTime, string.Empty, f.SheetName,
                    f.DieCount.ToString(CultureInfo.InvariantCulture), f.ItemCount.ToString(CultureInfo.InvariantCulture), user,
                    f.Customer, f.FileDevice, f.WaferType, f.WaferRev, f.WaferId, f.FabName }); // 18항목 = 파일명 8칸 규칙 (PARSE_RULE_VER 2)
                seq = head.Rows[0]["SRC_FILE_SEQ"].ToString();

                // 3) 측정 항목 -> 이름별 번호
                SetStatus("측정 항목 등록 중...");
                DataTable items = Service.MergeEtItem(new string[] { factory, device }, f.Items, user);
                Dictionary<string, string> itemSeqByName = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (DataRow dr in items.Rows)
                    itemSeqByName[dr["ITEM_NAME"].ToString()] = dr["ET_ITEM_SEQ"].ToString();

                // 4) Die
                SetStatus("Die 저장 중...");
                Service.CreateEtDieBatch(seq, f.Dies, user);

                // 5) 측정값 (10,000행씩)
                List<string[,]> batches = EtResultFileParser.BuildValueBatches(f, itemSeqByName, ValueBatchRows);
                progressSave.Maximum = Math.Max(1, batches.Count);
                long sent = 0;
                for (int b = 0; b < batches.Count; b++)
                {
                    sent += Service.CreateEtValueBatch(seq, batches[b], user);
                    progressSave.Value = b + 1;
                    SetStatus(string.Format("측정값 저장 중... {0:N0} / {1:N0}", sent, f.ValueCount));
                }

                // 6) 확정
                DataRow done = Service.CompleteEtFile(seq, user).Rows[0];
                if (done["RESULT"].ToString() == "OK")
                {
                    SetStatus(string.Format("저장 완료 / 파일 번호 {0} / WAFER_ID {1} / Die {2} / 측정값 {3:N0} ('-' {4})",
                        seq, head.Rows[0]["WAFER_ID"], done["DB_DIE"], done["DB_VALUE"], done["NOT_TESTED"]));
                    Notify("저장이 완료되었습니다.\r\n\r\nWAFER_ID " + head.Rows[0]["WAFER_ID"] + " / 파일 번호 " + seq
                        + "\r\nDie " + done["DB_DIE"] + " (Fail " + done["FAIL_DIE"] + ") / 측정값 " + done["DB_VALUE"] + " ('-' " + done["NOT_TESTED"] + ")",
                        MessageBoxIcon.Information);
                }
                else
                {
                    SetStatus("저장 확인 불일치 / " + done["MESSAGE"]);
                    Notify("저장 건수가 맞지 않아 확정하지 못했습니다(파일 번호 " + seq + " 는 LOADING 상태).\r\n\r\n" + done["MESSAGE"]
                        + "\r\nDie " + done["DB_DIE"] + " / " + done["EXPECTED_DIE"] + ", 측정값 " + done["DB_VALUE"] + " / " + done["EXPECTED_VALUE"]
                        + "\r\n\r\n다시 저장하면 삭제 후 재등록할 수 있습니다.", MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                SetStatus("저장 실패 / " + Cause(ex).Message);
                Notify("저장하지 못했습니다.\r\n\r\n" + Cause(ex).Message
                    + (seq == null ? string.Empty : "\r\n\r\n파일 번호 " + seq + " 는 LOADING 상태로 남습니다. 다시 저장하면 삭제 후 재등록할 수 있습니다."),
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                SetBusy(false);
                LoadFileList();
            }
        }

        /// <summary>등록 파일 목록을 더블클릭하면 그 파일의 업로드 데이터 조회 창을 연다(설계 11절). 창은 여러 개 띄울 수 있다.</summary>
        private void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (busy || e.RowIndex < 0 || e.RowIndex >= dgvFiles.Rows.Count) return;
            DataRowView rowView = dgvFiles.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null) return;
            ShowResultView(new frmEtResultView(Service, rowView.Row["SRC_FILE_SEQ"].ToString()));
        }

        /// <summary>조회 창 표시. 화면 Smoke 검사는 창을 화면 밖에 띄우도록 바꾼다.</summary>
        protected virtual void ShowResultView(Form view)
        {
            view.Show(this);
        }

        /// <summary>목록에서 고른 파일을 삭제한다(Die / 측정값 포함, 측정 항목은 남음).</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvFiles.CurrentRow == null || dgvFiles.CurrentRow.DataBoundItem == null)
            {
                Notify("삭제할 파일을 목록에서 고르세요.", MessageBoxIcon.Warning);
                return;
            }
            DataRow dr = ((DataRowView)dgvFiles.CurrentRow.DataBoundItem).Row;
            string seq = dr["SRC_FILE_SEQ"].ToString();
            if (Ask("E-Test 파일을 삭제할까요?\r\n\r\nWAFER_ID " + dr["WAFER_ID"] + " / 시험 일시 " + FormatTestTime(dr["TEST_TIME"].ToString())
                + " / 파일 번호 " + seq + "\r\nDie / 측정값도 함께 삭제됩니다.", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                Cursor = Cursors.WaitCursor;
                string user = DACrux.Base.GlobalVariable.UserID;
                if (string.IsNullOrEmpty(user)) user = Environment.UserName;
                int values = Service.DeleteEtFile(seq, user);
                SetStatus("삭제 완료 / 파일 번호 " + seq + " / 측정값 " + values.ToString("N0", CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Notify("삭제하지 못했습니다.\r\n\r\n" + Cause(ex).Message, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                LoadFileList();
            }
        }

        #endregion

        #region [공통]

        private void SetBusy(bool value)
        {
            busy = value;
            if (!value) progressSave.Value = 0;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnBrowse.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            txtFactory.Enabled = txtSlot.Enabled = !busy;
            btnSave.Enabled = !busy && parsedFile != null;
            btnDelete.Enabled = !busy && dgvFiles.Rows.Count > 0;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
            progressSave.Refresh();
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
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
