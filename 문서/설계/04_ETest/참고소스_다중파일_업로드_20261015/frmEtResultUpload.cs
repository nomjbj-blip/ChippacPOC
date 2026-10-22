using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// E-Test 결과 파일(xlsx) 업로드 화면이다. 2026-10-14 Upload 재설계로 다시 작성했다.
    /// 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9절 / 10절 (저장 대상 = E-Test Para 설정의 등록 Parameter)
    /// 처리 흐름:
    ///  1) [파일 추가] / [폴더 추가]: 대기 목록에 넣고 파일마다 순서대로 분석한다(BackgroundWorker).
    ///     분석 = 파일명 8칸(Device / 공정 / Lot / Lot-Wafer No -> Slot) -> 시트 -> 등록 Parameter 대조(EtUploadPlan) -> 미리보기 판정(EtJudgeRule)
    ///            -> Map 확인(Map ID = Device, 사용 Die 밖 Die 수) -> DB 의 같은 Wafer + 공정 파일 확인.
    ///     상태: 분석 중 / 준비 / 경고(누락 · 이름 충돌 · Map 밖 · 빈 칸 · 파일 경고) / 오류(읽기 실패 · 등록 Parameter 없음 · Slot · 목록 안 중복) / 중복(DB) / 저장됨 / 실패 / 건너뜀
    ///  2) 행을 고르면 아래 공통 결과 Control(EtResultDataControl, 조회 화면과 같은 모양)에 정보 / 1단계 Die / 2단계 정의 Para 값을 보여 준다.
    ///     Slot 은 대기 목록에서 고칠 수 있다.
    ///  3) [모두 저장] / [선택 저장]: 저장 전에 한 번 묻는다 — 파일마다 저장 Parameter x Die = 값 수와 합계를 보여 주고(등록 Parameter 사용 Y 만 저장),
    ///     중복이 있으면 예 = 덮어쓰기 / 아니요 = 건너뛰기. 나중에 등록한 Parameter 는 다시 업로드해야 보인다.
    ///     파일마다 BeginEtFile(21항목) -> Die -> 값(등록 Parameter 만, 10,000행씩) -> CompleteEtFile(판정 + Map 좌표 + READY). 실패하면 그 파일만 지운다.
    ///  4) 저장된 파일 목록 / 삭제 / Map 좌표 다시 계산 / 결과 보기는 E-Test 결과 조회 화면(frmEtResultView)에서 한다 (2026-10-15 Upload / View 분리).
    /// 진행 표시(2026-10-14): 화면 아래 busyBar(ProgressStatusBar). 분석은 입력을 막지 않고 "분석 중 · 파일 이름 (n / 전체)" 만 보이고,
    ///  저장은 파일 단위 막대 + 파일마다 서버 저장을 별도 스레드에서 한다.
    /// </summary>
    public partial class frmEtResultUpload : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>측정값 1회 전송 행 수 (서버 MaxBatchRows와 같음)</summary>
        private const int ValueBatchRows = 10000;

        private const string StPending = "분석 대기";
        private const string StAnalyzing = "분석 중";
        private const string StReady = "준비";
        private const string StWarning = "경고";
        private const string StError = "오류";
        private const string StDuplicate = "중복";
        private const string StSaved = "저장됨";
        private const string StFailed = "실패";
        private const string StSkipped = "건너뜀";

        private RO.GdsEtResult uploadService;
        private readonly List<QueueItem> queue = new List<QueueItem>();
        /// <summary>Factory|Device|공정 -> 등록 Parameter 목록 (분석마다 다시 읽지 않게, 목록을 비우면 지움)</summary>
        private readonly Dictionary<string, DataTable> registeredCache = new Dictionary<string, DataTable>(StringComparer.Ordinal);
        /// <summary>Factory|Device -> Map (없으면 null)</summary>
        private readonly Dictionary<string, DieCoordinateMapper> mapCache = new Dictionary<string, DieCoordinateMapper>(StringComparer.Ordinal);
        private BackgroundWorker analyzer;
        /// <summary>백그라운드 분석 진행 표시(입력은 막지 않음). 분석이 끝나면 닫는다.</summary>
        private IDisposable analysisScope;
        /// <summary>분석 진행 표시를 시작할 때 이미 분석이 끝나 있던 행 수(막대의 0 기준)</summary>
        private int analysisBase;
        private bool busy;
        private QueueItem shownItem;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtResultUpload()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colQSlot, colQDie, colQHeader, colQRegistered, colQSaved, colQMissing, colQFailDie, colQSeq })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colQSlot.HeaderCell.Style.Font = new Font(dgvQueue.Font, FontStyle.Bold);
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
        }

        #region [대기 목록 추가 / 분석]

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            string[] paths = ChooseFiles();
            if (paths != null && paths.Length > 0) AddFiles(paths);
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            string folder = ChooseFolder();
            if (string.IsNullOrEmpty(folder)) return;
            string[] paths = Directory.GetFiles(folder, "*.xlsx", SearchOption.TopDirectoryOnly)
                .Where(p => !Path.GetFileName(p).StartsWith("~$", StringComparison.Ordinal))
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToArray();
            if (paths.Length == 0)
            {
                Notify("폴더에 xlsx 파일이 없습니다: " + folder, MessageBoxIcon.Warning);
                return;
            }
            AddFiles(paths);
        }

        /// <summary>파일을 대기 목록 끝에 넣고 분석을 시작한다. 이미 목록에 있는 파일은 건너뛴다.</summary>
        private void AddFiles(IEnumerable<string> paths)
        {
            string factory = txtFactory.Text.Trim();
            if (factory.Length == 0)
            {
                Notify("Factory 를 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            int added = 0, same = 0;
            foreach (string path in paths)
            {
                if (queue.Any(q => string.Equals(q.Path, path, StringComparison.OrdinalIgnoreCase)))
                {
                    same++;
                    continue;
                }
                QueueItem item = new QueueItem { Path = path, Factory = factory };
                queue.Add(item);
                item.Row = dgvQueue.Rows[dgvQueue.Rows.Add()];
                item.Row.Tag = item;
                ShowRow(item);
                added++;
            }
            SetStatus("대기 목록에 " + added + "개 추가" + (same > 0 ? " / 중복 " + same + "개 건너뜀" : string.Empty) + " / 분석 중...");
            StartAnalysis();
        }

        /// <summary>분석을 백그라운드로 할지. 화면 Smoke 검사는 순서를 보장하려고 false 로 바꾼다.</summary>
        protected virtual bool AnalyzeInBackground
        {
            get { return true; }
        }

        private bool Analyzing
        {
            get { return analyzer != null && analyzer.IsBusy; }
        }

        /// <summary>분석 대기 행을 순서대로 분석한다. 백그라운드 작업이 이미 돌고 있으면 그 작업이 이어서 처리한다.</summary>
        private void StartAnalysis()
        {
            if (!AnalyzeInBackground)
            {
                using (busyBar.Begin("분석"))
                {
                    List<QueueItem> pending = queue.Where(q => q.Status == StPending).ToList();
                    for (int i = 0; i < pending.Count; i++)
                    {
                        QueueItem item = pending[i];
                        busyBar.Step(i, pending.Count);
                        busyBar.Note(Path.GetFileName(item.Path));
                        item.Status = StAnalyzing;
                        ShowRow(item);
                        Analyze(item);
                        AfterAnalyze(item);
                    }
                }
                AnalysisDone();
                return;
            }
            if (Analyzing) return;
            if (analysisScope == null)
            {
                analysisBase = queue.Count(q => q.Status != StPending && q.Status != StAnalyzing);
                analysisScope = busyBar.Begin("분석", false);
                StepAnalysis();
            }
            analyzer = new BackgroundWorker { WorkerReportsProgress = true };
            analyzer.DoWork += analyzer_DoWork;
            analyzer.ProgressChanged += analyzer_ProgressChanged;
            analyzer.RunWorkerCompleted += analyzer_RunWorkerCompleted;
            analyzer.RunWorkerAsync();
            UpdateButtons();
        }

        /// <summary>백그라운드: 화면 스레드에서 다음 분석 대기 행을 받아 분석하고, 결과는 ProgressChanged 로 화면에 반영한다.</summary>
        private void analyzer_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            while (true)
            {
                QueueItem next = null;
                try
                {
                    if (IsDisposed) return;
                    Invoke((MethodInvoker)delegate
                    {
                        next = queue.FirstOrDefault(q => q.Status == StPending);
                        if (next != null)
                        {
                            next.Status = StAnalyzing;
                            ShowRow(next);
                            busyBar.Note(Path.GetFileName(next.Path));
                        }
                    });
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (InvalidOperationException)
                {
                    return;
                }
                if (next == null) return;
                Analyze(next);
                worker.ReportProgress(0, next);
            }
        }

        private void analyzer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AfterAnalyze((QueueItem)e.UserState);
        }

        private void analyzer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                Notify("분석 중 오류가 났습니다.\r\n\r\n" + Cause(e.Error).Message, MessageBoxIcon.Error);
            AnalysisDone();
        }

        private void AnalysisDone()
        {
            if (analysisScope != null)
            {
                analysisScope.Dispose();
                analysisScope = null;
            }
            UpdateButtons();
            SetStatus("분석 완료 / " + QueueSummary());
        }

        /// <summary>
        /// 파일 1개 분석 (화면에 손대지 않는다, 백그라운드 스레드에서도 돈다).
        /// 파일 읽기 -> 등록 Parameter 대조 + 미리보기 판정 -> Map 확인 -> DB 의 같은 Wafer + 공정 파일 확인. 실패는 item.Error 에 남긴다.
        /// </summary>
        private void Analyze(QueueItem item)
        {
            item.Error = null;
            item.Plan = null;
            item.ExistingSeq = null;
            item.ExistingText = null;
            item.HasMap = false;
            item.MapText = null;
            item.OutOfMap = 0;
            try
            {
                item.File = EtResultFileParser.Parse(item.Path);
            }
            catch (Exception ex)
            {
                item.Error = "파일 읽기 실패: " + Cause(ex).Message.Replace("\r\n", " ");
                return;
            }
            EtResultFile f = item.File;
            item.Program = EtParameterNameRule.ToTestProgram(f.TestProgram);
            item.Slot = f.SlotNo;
            try
            {
                item.Plan = EtUploadPlan.Build(f, RegisteredFor(item.Factory, f.FileDevice, item.Program));
                if (item.Plan.RegisteredCount == 0)
                {
                    item.Error = "등록 Parameter 없음: E-Test Para 설정에서 Device " + f.FileDevice + " / 공정 " + item.Program + " 을 먼저 등록하세요";
                    return;
                }
                if (item.Plan.Matched.Count == 0)
                {
                    item.Error = "파일에 등록 Parameter 가 하나도 없음 (등록 " + item.Plan.RegisteredCount + " / 파일 헤더 " + f.ItemCount + ")";
                    return;
                }
                DieCoordinateMapper mapper = MapperFor(item.Factory, f.FileDevice);
                item.HasMap = mapper != null;
                item.MapText = new string[f.DieCount];
                for (int i = 0; i < f.DieCount; i++)
                {
                    if (mapper == null)
                    {
                        item.MapText[i] = string.Empty;
                        continue;
                    }
                    DiePoint system;
                    DiePoint etest = new DiePoint(int.Parse(f.Dies[i, 1], CultureInfo.InvariantCulture), int.Parse(f.Dies[i, 2], CultureInfo.InvariantCulture));
                    if (mapper.TryToSystem(DieCoordType.ETest, etest, out system))
                        item.MapText[i] = system.X.ToString(CultureInfo.InvariantCulture) + "," + system.Y.ToString(CultureInfo.InvariantCulture);
                    else
                    {
                        item.MapText[i] = "밖";
                        item.OutOfMap++;
                    }
                }
                CheckExisting(item);
            }
            catch (Exception ex)
            {
                item.Error = "분석 실패: " + Cause(ex).Message.Replace("\r\n", " ");
            }
        }

        /// <summary>그 Device + 공정의 등록 Parameter (GetEtParameterList, 캐시)</summary>
        private DataTable RegisteredFor(string factory, string device, string program)
        {
            string key = factory + "|" + device + "|" + program;
            lock (registeredCache)
            {
                DataTable dt;
                if (!registeredCache.TryGetValue(key, out dt))
                {
                    dt = Service.GetEtParameterList(factory, device, program);
                    registeredCache[key] = dt;
                }
                return dt;
            }
        }

        /// <summary>Map ID = Device 의 사용 Die 로 만든 좌표 변환기. Map 이 없으면 null (캐시)</summary>
        private DieCoordinateMapper MapperFor(string factory, string device)
        {
            string key = factory + "|" + device;
            lock (mapCache)
            {
                DieCoordinateMapper mapper;
                if (!mapCache.TryGetValue(key, out mapper))
                {
                    DataTable dies = Service.GetEtMapDieList(factory, device);
                    bool used = dies.Rows.Cast<DataRow>().Any(r => r["USECODE"] != DBNull.Value
                        && Convert.ToInt32(r["USECODE"], CultureInfo.InvariantCulture) == DieCoordinateMapper.UsedDieCode);
                    mapper = used ? DieCoordinateMapper.FromDieTable(dies, new DiePoint(0, 0)) : null;
                    mapCache[key] = mapper;
                }
                return mapper;
            }
        }

        /// <summary>DB 에 같은 Wafer(Lot + Slot) + 공정 파일이 있는지 본다(Slot 이 있을 때만).</summary>
        private void CheckExisting(QueueItem item)
        {
            item.ExistingSeq = null;
            item.ExistingText = null;
            if (item.File == null || item.Slot < 1 || item.Slot > 99) return;
            DataTable exist = Service.GetEtFileByKey(new string[] { item.Factory, item.File.LotId, item.Slot.ToString(CultureInfo.InvariantCulture), item.Program });
            if (exist.Rows.Count == 0) return;
            DataRow dr = exist.Rows[0];
            item.ExistingSeq = dr["SRC_FILE_SEQ"].ToString();
            item.ExistingText = "DB 에 같은 Wafer + 공정 파일 있음 (파일 번호 " + item.ExistingSeq + " / Device " + dr["DEVICE_ID"] + " / Test Date "
                + FormatTestTime(dr["TEST_TIME"].ToString()) + " / " + dr["LOAD_STATUS"] + ")";
        }

        /// <summary>분석 결과를 상태 / 행에 반영하고, 목록 안 중복을 다시 본다.</summary>
        private void AfterAnalyze(QueueItem item)
        {
            // 분석이 끝났으므로 '분석 중' 을 풀어 상태를 다시 정하게 한다(ComputeStatus)
            if (item.Status == StAnalyzing) item.Status = StReady;
            RefreshStatuses();
            StepAnalysis();
            if (shownItem == item) ShowDetail(item);
            else if (shownItem == null && dgvQueue.SelectedRows.Count == 1 && dgvQueue.SelectedRows[0].Tag == item) ShowDetail(item);
            UpdateButtons();
        }

        /// <summary>백그라운드 분석 진행 막대: 이번에 분석한 수 / (분석한 수 + 남은 수). 분석 중에 파일을 더 넣으면 전체 수가 늘어난다.</summary>
        private void StepAnalysis()
        {
            if (analysisScope == null) return;
            int remaining = queue.Count(q => q.Status == StPending || q.Status == StAnalyzing);
            int done = Math.Max(0, queue.Count - remaining - analysisBase);
            busyBar.Step(done, done + remaining);
        }

        /// <summary>분석이 끝난 행(저장됨 / 실패 / 건너뜀 제외)의 상태를 다시 정하고 행을 다시 그린다.</summary>
        private void RefreshStatuses()
        {
            foreach (QueueItem item in queue)
            {
                if (item.Status == StPending || item.Status == StAnalyzing || item.Status == StSaved || item.Status == StFailed || item.Status == StSkipped)
                {
                    ShowRow(item);
                    continue;
                }
                ComputeStatus(item);
                ShowRow(item);
            }
        }

        /// <summary>상태 / 메시지를 정한다. 오류 > 중복(DB) > 경고 > 준비.</summary>
        private void ComputeStatus(QueueItem item)
        {
            if (item.Error != null)
            {
                item.Status = StError;
                item.Message = item.Error;
                return;
            }
            if (item.Slot < 1 || item.Slot > 99)
            {
                item.Status = StError;
                item.Message = "Slot 을 입력하세요(1~99). Lot-Wafer No " + item.File.WaferId + " 에서 찾지 못했습니다.";
                return;
            }
            QueueItem twin = queue.TakeWhile(q => q != item).FirstOrDefault(q => q.File != null && q.Error == null && q.Status != StFailed && q.Status != StSkipped
                && q.Factory == item.Factory && q.Program == item.Program && q.WaferId == item.WaferId);
            if (twin != null)
            {
                item.Status = StError;
                item.Message = "목록 안 중복: 같은 WAFER_ID " + item.WaferId + " / 공정 " + item.Program + " 파일이 위에 있습니다 (" + Path.GetFileName(twin.Path) + ")";
                return;
            }
            EtUploadPlan plan = item.Plan;
            List<string> warnings = new List<string>();
            if (plan.Missing.Count > 0)
                warnings.Add("누락 " + plan.Missing.Count + " (예: " + string.Join(", ", plan.Missing.Take(2).ToArray()) + (plan.Missing.Count > 2 ? " ..." : string.Empty) + ")");
            if (plan.Conflicts.Count > 0)
                warnings.Add("이름 충돌 / 중복 헤더 " + plan.Conflicts.Count + " 건너뜀");
            if (item.OutOfMap > 0)
                warnings.Add("Map 밖 Die " + item.OutOfMap);
            if (plan.EmptyCount > 0)
                warnings.Add("빈 칸 값 " + plan.EmptyCount + " (판정 안 함)");
            // Slot 을 입력했으면 파서의 'Slot 기본값을 정하지 못함' 경고는 해결된 것이다
            warnings.AddRange(item.File.Warnings.Where(w => !w.StartsWith("Lot-Wafer No(", StringComparison.Ordinal)));
            List<string> notes = new List<string>();
            if (plan.Matched.Count > plan.JudgedCount)
                notes.Add("Spec 없음 · Enable N " + (plan.Matched.Count - plan.JudgedCount) + " (판정 안 함)");
            if (!item.HasMap)
                notes.Add("Map 없음 (File Map Setup 에 Map ID " + item.File.FileDevice + " 가 있으면 좌표 저장)");
            if (plan.Unused > 0)
                notes.Add("사용 안 함 " + plan.Unused + " 저장 안 함");

            item.Status = item.ExistingSeq != null ? StDuplicate : warnings.Count > 0 ? StWarning : StReady;
            List<string> all = new List<string>();
            if (item.ExistingText != null) all.Add(item.ExistingText);
            all.AddRange(warnings);
            all.AddRange(notes);
            item.Message = all.Count == 0 ? "저장할 수 있습니다." : string.Join(" / ", all.ToArray());
        }

        /// <summary>대기 목록 1행을 다시 그린다.</summary>
        private void ShowRow(QueueItem item)
        {
            DataGridViewRow row = item.Row;
            EtResultFile f = item.File;
            EtUploadPlan plan = item.Plan;
            row.Cells[colQFile.Index].Value = Path.GetFileName(item.Path);
            row.Cells[colQStatus.Index].Value = item.Status;
            row.Cells[colQDevice.Index].Value = f == null ? string.Empty : f.FileDevice;
            row.Cells[colQProgram.Index].Value = f == null ? string.Empty : item.Program;
            row.Cells[colQLot.Index].Value = f == null ? string.Empty : f.LotId;
            row.Cells[colQLotWafer.Index].Value = f == null ? string.Empty : f.WaferId;
            row.Cells[colQSlot.Index].Value = f == null || item.Slot == 0 ? string.Empty : item.Slot.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQWafer.Index].Value = item.WaferId;
            row.Cells[colQTestDate.Index].Value = f == null ? string.Empty : FormatTestTime(f.TestTime);
            row.Cells[colQDie.Index].Value = f == null ? string.Empty : f.DieCount.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQHeader.Index].Value = f == null ? string.Empty : f.ItemCount.ToString("N0", CultureInfo.InvariantCulture);
            row.Cells[colQRegistered.Index].Value = plan == null ? string.Empty : plan.RegisteredCount.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQSaved.Index].Value = plan == null ? string.Empty : plan.Matched.Count.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQMissing.Index].Value = plan == null ? string.Empty : plan.Missing.Count.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQFailDie.Index].Value = plan == null || item.Error != null ? string.Empty : plan.SpecFailDie.ToString(CultureInfo.InvariantCulture);
            row.Cells[colQMap.Index].Value = item.MapText == null ? string.Empty : !item.HasMap ? "없음" : item.OutOfMap > 0 ? "밖 " + item.OutOfMap : "있음";
            row.Cells[colQMessage.Index].Value = item.Message;
            row.Cells[colQSeq.Index].Value = item.SavedSeq;
            row.Cells[colQSlot.Index].ReadOnly = f == null || item.Status == StSaved || item.Status == StAnalyzing || busy;

            Color back = Color.Empty;
            if (item.Status == StError || item.Status == StFailed) back = Color.MistyRose;
            else if (item.Status == StWarning) back = Color.LightYellow;
            else if (item.Status == StSaved) back = Color.Gainsboro;
            row.DefaultCellStyle.BackColor = back;
            row.Cells[colQStatus.Index].Style.ForeColor = item.Status == StDuplicate ? Color.DarkOrange : item.Status == StError || item.Status == StFailed ? Color.Firebrick : Color.Empty;
            row.Cells[colQFailDie.Index].Style.ForeColor = plan != null && plan.SpecFailDie > 0 ? Color.Firebrick : Color.Empty;
            row.Cells[colQMissing.Index].Style.ForeColor = plan != null && plan.Missing.Count > 0 ? Color.DarkOrange : Color.Empty;
        }

        /// <summary>Slot 을 고치면 WAFER_ID / DB 중복 / 목록 안 중복을 다시 본다.</summary>
        private void dgvQueue_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colQSlot.Index) return;
            QueueItem item = dgvQueue.Rows[e.RowIndex].Tag as QueueItem;
            if (item == null || item.File == null) return;
            int slot;
            string text = Convert.ToString(dgvQueue.Rows[e.RowIndex].Cells[colQSlot.Index].Value, CultureInfo.InvariantCulture).Trim();
            item.Slot = int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out slot) && slot >= 1 && slot <= 99 ? slot : 0;
            if (item.Status == StFailed || item.Status == StSkipped) item.Status = StReady;
            try
            {
                // 서버 중복 확인: 표 편집 이벤트 도중이라 RunHere(화면 스레드, 진행 표시 + 모래시계) (2026-10-15)
                if (item.Error == null) busyBar.RunHere("중복 확인", () => { CheckExisting(item); return true; });
            }
            catch (Exception ex)
            {
                item.Error = "중복 확인 실패: " + Cause(ex).Message;
            }
            BeginInvoke((MethodInvoker)delegate
            {
                RefreshStatuses();
                if (shownItem == item) ShowDetail(item);
                UpdateButtons();
            });
        }

        /// <summary>Del: 고른 행을 대기 목록에서 뺀다(분석 중 / 저장 중이 아닐 때).</summary>
        private void dgvQueue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || dgvQueue.IsCurrentCellInEditMode || busy || Analyzing) return;
            List<QueueItem> items = SelectedItems();
            if (items.Count == 0) return;
            foreach (QueueItem item in items)
            {
                queue.Remove(item);
                dgvQueue.Rows.Remove(item.Row);
                if (shownItem == item) ShowDetail(null);
            }
            RefreshStatuses();
            UpdateButtons();
            SetStatus("대기 목록에서 " + items.Count + "개 뺌 / " + QueueSummary());
            e.Handled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (busy || Analyzing) return;
            queue.Clear();
            dgvQueue.Rows.Clear();
            lock (registeredCache) registeredCache.Clear();
            lock (mapCache) mapCache.Clear();
            ShowDetail(null);
            UpdateButtons();
            SetStatus("대기 목록을 비웠습니다.");
        }

        #endregion

        #region [선택 파일 결과]

        /// <summary>파일을 고르면 공통 결과 Control 에 정보 / Die / 정의 Para 값을 채운다. 그동안 모래시계 커서 (2026-10-15).</summary>
        private void dgvQueue_SelectionChanged(object sender, EventArgs e)
        {
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                List<QueueItem> items = SelectedItems();
                ShowDetail(items.Count == 1 ? items[0] : null);
                UpdateButtons();
            }
        }

        /// <summary>선택 파일의 분석 결과(저장 전)를 공통 결과 Control 에 보인다. null 이면 비운다.</summary>
        private void ShowDetail(QueueItem item)
        {
            shownItem = item;
            if (item == null || item.File == null)
            {
                resultView.Clear();
                return;
            }
            resultView.ShowData(EtResultData.FromUpload(item.File, item.Plan, item.Program, item.Slot, item.WaferId,
                item.MapText, item.HasMap, item.OutOfMap, item.Status, item.Message));
        }

        #endregion

        #region [저장]

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveItems(queue.Where(Savable).ToList());
        }

        private void btnSaveSelected_Click(object sender, EventArgs e)
        {
            SaveItems(SelectedItems().Where(Savable).ToList());
        }

        private static bool Savable(QueueItem item)
        {
            return item.Status == StReady || item.Status == StWarning || item.Status == StDuplicate;
        }

        /// <summary>
        /// 고른 행을 순서대로 저장한다. 저장 전에 한 번 묻는다: 중복(DB 에 같은 Wafer + 공정)이 있으면 예 = 덮어쓰기 / 아니요 = 건너뛰기 / 취소.
        /// 행마다 결과(저장됨 + 파일 번호 / 실패 + 메시지)를 표시한다.
        /// </summary>
        private void SaveItems(List<QueueItem> targets)
        {
            if (busy || Analyzing) return;
            if (targets.Count == 0)
            {
                Notify("저장할 파일이 없습니다. 상태가 준비 / 경고 / 중복인 행만 저장합니다.", MessageBoxIcon.Warning);
                return;
            }
            int duplicates = targets.Count(q => q.Status == StDuplicate);
            string text = SaveConfirmText(targets);
            bool overwrite = false;
            if (duplicates > 0)
            {
                DialogResult answer = Ask(text + "\r\n\r\nDB 에 같은 Wafer + 공정 파일이 이미 있는 " + duplicates + "개:\r\n"
                    + "예 = 기존 파일을 지우고 다시 저장(덮어쓰기)\r\n아니요 = 건너뛰기\r\n취소 = 저장하지 않음", MessageBoxButtons.YesNoCancel);
                if (answer == DialogResult.Cancel) return;
                overwrite = answer == DialogResult.Yes;
            }
            else if (Ask(text, MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            string user = User();
            int saved = 0, failed = 0, skipped = 0;
            using (busyBar.Begin("저장 (" + targets.Count + "개)"))
            {
                try
                {
                    SetBusy(true);
                    for (int t = 0; t < targets.Count; t++)
                    {
                        QueueItem item = targets[t];
                        busyBar.Step(t, targets.Count);
                        busyBar.Note(Path.GetFileName(item.Path));
                        if (item.Status == StDuplicate && !overwrite)
                        {
                            item.Status = StSkipped;
                            item.Message = "건너뜀: " + item.ExistingText;
                            skipped++;
                        }
                        else
                        {
                            SetStatus("저장 중 (" + (t + 1) + " / " + targets.Count + "): " + Path.GetFileName(item.Path));
                            QueueItem target = item;
                            if (busyBar.Run("서버 저장", () => SaveItem(target, user))) saved++;
                            else failed++;
                        }
                        ShowRow(item);
                        if (shownItem == item) ShowDetail(item);
                    }
                }
                finally
                {
                    SetBusy(false);
                    RefreshStatuses();
                }
            }
            string summary = "저장 " + saved + " / 실패 " + failed + " / 건너뜀 " + skipped + (saved > 0 ? "\r\n저장한 결과는 E-Test 결과 조회 화면에서 봅니다." : string.Empty);
            SetStatus("저장 끝 / " + summary + " / " + QueueSummary());
            Notify(summary + (failed > 0 ? "\r\n\r\n실패한 파일은 대기 목록 메시지를 확인하세요. 저장 도중 실패한 파일은 지웠습니다." : string.Empty),
                failed > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        /// <summary>
        /// 저장 확인 문구 (2026-10-15): 파일마다 공정 / 저장 Parameter x Die = 값 수 (10줄까지) + 합계, 등록되지 않은 헤더는 저장하지 않는다는 안내,
        /// 경고 / 오류 수. 숫자는 분석 결과(EtUploadPlan.Matched x Die)이고 서버 저장 건수와 다르면 저장이 MISMATCH 로 실패한다.
        /// </summary>
        private string SaveConfirmText(List<QueueItem> targets)
        {
            const int shown = 10;
            int warnings = targets.Count(q => q.Status == StWarning);
            int errors = queue.Count(q => q.Status == StError);
            long totalValues = 0, totalSkipped = 0;
            List<string> lines = new List<string>();
            foreach (QueueItem item in targets)
            {
                long values = item.Plan.ValueCount(item.File);
                totalValues += values;
                totalSkipped += item.Plan.Skipped + item.Plan.Unused;
                if (lines.Count < shown)
                    lines.Add("  " + Path.GetFileName(item.Path) + "  |  " + item.Program + "  |  Parameter " + item.Plan.Matched.Count
                        + " x Die " + item.File.DieCount + " = 값 " + values.ToString("N0", CultureInfo.InvariantCulture));
            }
            if (targets.Count > shown) lines.Add("  ... 외 " + (targets.Count - shown) + "개");
            return "E-Test 결과 파일 " + targets.Count + "개를 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.\r\n\r\n"
                + string.Join("\r\n", lines.ToArray()) + "\r\n"
                + "  합계: 값 " + totalValues.ToString("N0", CultureInfo.InvariantCulture) + "\r\n\r\n"
                + "등록되지 않은 헤더는 저장하지 않습니다 (" + totalSkipped.ToString("N0", CultureInfo.InvariantCulture) + "개). "
                + "나중에 등록한 Parameter 는 그 파일을 다시 업로드해야 보입니다."
                + (warnings > 0 ? "\r\n경고 " + warnings + "개 포함" : string.Empty)
                + (errors > 0 ? "\r\n오류 " + errors + "개는 저장하지 않습니다." : string.Empty);
        }

        /// <summary>
        /// 파일 1개 저장: (덮어쓰기면 기존 파일 삭제) -> BeginEtFile(21항목) -> Die -> 값(등록 Parameter 만) -> CompleteEtFile(판정 + Map 좌표 + READY).
        /// 실패하면 이번에 만든 파일을 지우고 false. 진행 표시줄의 작업 스레드에서 돈다(화면 컨트롤을 만지지 않는다).
        /// </summary>
        private bool SaveItem(QueueItem item, string user)
        {
            EtResultFile f = item.File;
            EtUploadPlan plan = item.Plan;
            string seq = null;
            try
            {
                if (item.ExistingSeq != null)
                    Service.DeleteEtFile(item.ExistingSeq, user);
                DataTable head = Service.BeginEtFile(new string[] {
                    item.Factory, f.FileDevice, f.FileName, f.LotId, item.Slot.ToString(CultureInfo.InvariantCulture), item.Program, f.TestTime, string.Empty, f.SheetName,
                    f.DieCount.ToString(CultureInfo.InvariantCulture), plan.Matched.Count.ToString(CultureInfo.InvariantCulture), user,
                    f.Customer, f.FileDevice, f.WaferType, f.WaferRev, f.WaferId, f.FabName,
                    f.ItemCount.ToString(CultureInfo.InvariantCulture), plan.RegisteredCount.ToString(CultureInfo.InvariantCulture), plan.Missing.Count.ToString(CultureInfo.InvariantCulture) });
                seq = head.Rows[0]["SRC_FILE_SEQ"].ToString();
                Service.CreateEtDieBatch(seq, f.Dies, user);
                foreach (string[,] batch in plan.BuildValueBatches(f, ValueBatchRows))
                    Service.CreateEtValueBatch(seq, batch, user);
                DataRow done = Service.CompleteEtFile(seq, user).Rows[0];
                if (done["RESULT"].ToString() != "OK")
                    throw new InvalidOperationException("저장 건수가 맞지 않습니다: " + done["MESSAGE"] + " (Die " + done["DB_DIE"] + " / " + done["EXPECTED_DIE"]
                        + ", 값 " + done["DB_VALUE"] + " / " + done["EXPECTED_VALUE"] + ")");
                item.SavedSeq = seq;
                item.ExistingSeq = null;
                item.Status = StSaved;
                string mapId = done["MAP_ID"].ToString();
                item.Message = "저장 완료 / WAFER_ID " + head.Rows[0]["WAFER_ID"] + " / Spec Fail Die " + done["SPEC_FAIL_DIE"] + " / 값 "
                    + Convert.ToInt64(done["DB_VALUE"], CultureInfo.InvariantCulture).ToString("N0", CultureInfo.InvariantCulture)
                    + (mapId.Length == 0 ? " / Map 없음" : " / Map " + mapId + " 좌표 Die " + done["MAP_DIE"]
                        + (Convert.ToInt64(done["OUT_OF_MAP"], CultureInfo.InvariantCulture) > 0 ? " (밖 " + done["OUT_OF_MAP"] + ")" : string.Empty));
                return true;
            }
            catch (Exception ex)
            {
                string cleanup = string.Empty;
                if (seq != null)
                {
                    try
                    {
                        Service.DeleteEtFile(seq, user);
                        cleanup = " (저장 도중 만든 파일 번호 " + seq + " 는 지움)";
                    }
                    catch (Exception cleanEx)
                    {
                        cleanup = " (파일 번호 " + seq + " 를 지우지 못함: " + Cause(cleanEx).Message + ")";
                    }
                }
                item.Status = StFailed;
                item.Message = "저장 실패: " + Cause(ex).Message.Replace("\r\n", " ") + cleanup;
                return false;
            }
        }

        #endregion

        #region [공통]

        /// <summary>대기 목록 1행 = 파일 1개</summary>
        private sealed class QueueItem
        {
            public string Path;
            public string Factory;
            public string Status = StPending;
            public string Message = string.Empty;
            public EtResultFile File;
            public EtUploadPlan Plan;
            public string Program = string.Empty;
            public int Slot;
            public bool HasMap;
            /// <summary>Die 별 시스템 Map 좌표 "x,y" / "밖" / 빈 값(Map 없음)</summary>
            public string[] MapText;
            public int OutOfMap;
            public string ExistingSeq;
            public string ExistingText;
            public string SavedSeq = string.Empty;
            public string Error;
            public DataGridViewRow Row;

            public string WaferId
            {
                get { return File == null || Slot < 1 ? string.Empty : File.LotId + "-" + Slot.ToString("00", CultureInfo.InvariantCulture); }
            }
        }

        private List<QueueItem> SelectedItems()
        {
            return dgvQueue.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => r.Tag as QueueItem).Where(q => q != null).ToList();
        }

        private string QueueSummary()
        {
            return "대기 목록 " + queue.Count + " (준비 " + queue.Count(q => q.Status == StReady) + " / 경고 " + queue.Count(q => q.Status == StWarning)
                + " / 중복 " + queue.Count(q => q.Status == StDuplicate) + " / 오류 " + queue.Count(q => q.Status == StError)
                + " / 저장됨 " + queue.Count(q => q.Status == StSaved) + " / 실패 " + queue.Count(q => q.Status == StFailed) + ")";
        }

        private void SetBusy(bool value)
        {
            busy = value;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool analyzing = Analyzing;
            btnAddFiles.Enabled = btnAddFolder.Enabled = !busy;
            txtFactory.Enabled = !busy;
            btnClear.Enabled = !busy && !analyzing && queue.Count > 0;
            btnSaveAll.Enabled = !busy && !analyzing && queue.Any(Savable);
            btnSaveSelected.Enabled = !busy && !analyzing && SelectedItems().Any(Savable);
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            lblStatus.Refresh();
        }

        private static string User()
        {
            string user = DACrux.Base.GlobalVariable.UserID;
            return string.IsNullOrEmpty(user) ? Environment.UserName : user;
        }

        private static string FormatTestTime(string time)
        {
            DateTime value;
            return DateTime.TryParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
                ? value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : time;
        }

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>파일 고르기(여러 개). 화면 Smoke 검사는 샘플 파일 경로를 돌려주도록 바꾼다.</summary>
        protected virtual string[] ChooseFiles()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "E-Test 결과 파일 선택 (여러 개 가능)";
                dialog.Filter = "E-Test 결과 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*";
                dialog.Multiselect = true;
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileNames : new string[0];
            }
        }

        /// <summary>폴더 고르기. 화면 Smoke 검사는 정해진 폴더를 돌려주도록 바꾼다.</summary>
        protected virtual string ChooseFolder()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "E-Test 결과 파일(xlsx)이 있는 폴더 (하위 폴더는 보지 않음)";
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.SelectedPath : string.Empty;
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

        #endregion
    }
}
