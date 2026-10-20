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
    /// E-Test 결과 파일(xlsx) 업로드 화면이다. 2026-10-14 Upload 재설계로 다시 작성했고, 2026-10-15 파일 하나씩 올리는 화면으로 바꿨다.
    /// 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9절 / 10절 (저장 대상 = E-Test Para 설정의 등록 Parameter),
    ///       문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md 12절 (단일 파일). 여러 파일 버전은 문서/설계/04_ETest/참고소스_다중파일_업로드_20261015 에 있다.
    /// 처리 흐름:
    ///  1) [파일 열기]: 파일 하나를 분석한다(진행 표시줄 작업 스레드).
    ///     분석 = 파일명 8칸(Device / 공정 / Lot / Lot-Wafer No -> Slot) -> 시트 -> 등록 Parameter 대조(EtUploadPlan) -> 미리보기 판정(EtJudgeRule)
    ///            -> Map 확인(Map ID = Device 필수, 없으면 오류 / 사용 Die 밖 Die 수) -> DB 의 같은 Wafer + 공정 파일 확인.
    ///     상태: 준비 / 경고(누락 · 이름 충돌 · Map 밖 · 빈 칸 · 파일 경고) / 오류(읽기 실패 · 등록 Parameter 없음 · Map 없음 · Slot) / 중복(DB) / 저장됨 / 실패.
    ///     [다시 읽기]: 같은 파일을 다시 분석한다(등록 Parameter / Spec / Map 을 바꾼 뒤).
    ///  2) 결과는 아래 공통 결과 Control(EtResultDataControl, 조회 화면과 같은 모양)에 정보 / 1단계 Die / 2단계 정의 Para 값으로 보인다.
    ///     정보 표의 Device / 공정 / Lot / Lot-Wafer No 는 더블클릭해 고친다(파일명이 틀렸을 때, 고치면 다시 분석). Slot = Lot-Wafer No 끝 '-숫자'.
    ///     위 Slot 칸(txtSlot)은 2026-10-15 화면에서 숨겼다(Visible = false). 코드는 남아 있어 보이게 하면 Enter / 칸 이동 때 WAFER_ID / DB 중복을 다시 본다.
    ///  3) [저장]: 저장 전에 한 번 묻는다 — 저장 Parameter x Die = 값 수(등록 Parameter 사용 Y 만 저장). 중복이면 예 = 덮어쓰기 / 아니요 = 저장 안 함.
    ///     BeginEtFile(21항목) -> Die -> 값(등록 Parameter 만, 10,000행씩) -> CompleteEtFile(판정 + Map 좌표 + READY). 실패하면 만든 파일을 지운다.
    ///     나중에 등록한 Parameter 는 이 파일을 다시 업로드해야 보인다.
    ///  4) 저장된 파일 목록 / 삭제 / 결과 보기는 E-Test 결과 조회 화면(frmEtResultView)에서 한다 (2026-10-15 Upload / View 분리).
    ///     Map 을 나중에 만들거나 고쳤으면 그 파일을 다시 업로드한다([Map 좌표 다시 계산] 기능은 2026-10-15 삭제).
    /// </summary>
    public partial class frmEtResultUpload : DACrux.Framework.Base.DACruxUXBasic01
    {
        /// <summary>측정값 1회 전송 행 수 (서버 MaxBatchRows와 같음)</summary>
        private const int ValueBatchRows = 10000;

        private const string StReady = "준비";
        private const string StWarning = "경고";
        private const string StError = "오류";
        private const string StDuplicate = "중복";
        private const string StSaved = "저장됨";
        private const string StFailed = "실패";

        private RO.GdsEtResult uploadService;
        /// <summary>지금 연 파일 (없으면 null)</summary>
        private UploadItem current;
        private bool busy;

        /// <summary>메뉴 로더가 요구하는 기본 생성자.</summary>
        public frmEtResultUpload()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지 (2026-10-15)
            lblState.Font = new Font(Font, FontStyle.Bold);
            resultView.InfoEdited += resultView_InfoEdited;
            ShowItem(null);
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

        #region [파일 열기 / 분석]

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string path = ChooseFile();
            if (!string.IsNullOrEmpty(path)) OpenFile(path);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (current != null) OpenFile(current.Path);
        }

        /// <summary>파일 하나를 분석해 지금 파일로 보인다. 앞에 연 파일은 버린다(저장했으면 DB 에 남음).</summary>
        private void OpenFile(string path)
        {
            if (busy) return;
            string factory = txtFactory.Text.Trim();
            if (factory.Length == 0)
            {
                Notify("Factory 를 입력하세요.", MessageBoxIcon.Warning);
                return;
            }
            UploadItem item = new UploadItem { Path = path, Factory = factory };
            txtFile.Text = path;
            SetBusy(true);
            try
            {
                busyBar.Run("분석", () => { Analyze(item); return true; });
            }
            catch (Exception ex)
            {
                item.Error = "분석 실패: " + Cause(ex).Message.Replace("\r\n", " ");
            }
            finally
            {
                SetBusy(false);
            }
            current = item;
            ComputeStatus(item);
            ShowItem(item);
            SetStatus("분석 완료 / " + Path.GetFileName(path) + " / " + item.Status);
        }

        /// <summary>
        /// 파일 1개 분석 (화면에 손대지 않는다, 진행 표시줄 작업 스레드에서 돈다).
        /// 파일 읽기 -> Slot 기본값(파일명 Lot-Wafer No) -> AnalyzeFile. 실패는 item.Error 에 남긴다.
        /// </summary>
        private void Analyze(UploadItem item)
        {
            try
            {
                item.File = EtResultFileParser.Parse(item.Path);
            }
            catch (Exception ex)
            {
                item.Error = "파일 읽기 실패: " + Cause(ex).Message.Replace("\r\n", " ");
                return;
            }
            item.Slot = item.File.SlotNo;
            AnalyzeFile(item);
        }

        /// <summary>
        /// 읽어 둔 파일(item.File 의 Device / 공정 / Lot / Lot-Wafer No, 정보 표에서 고친 값 포함)로 다시 본다. 파일은 다시 읽지 않는다.
        /// 등록 Parameter 대조 + 미리보기 판정 -> Map 확인(필수) -> DB 의 같은 Wafer + 공정 파일 확인. 실패는 item.Error 에 남긴다.
        /// </summary>
        private void AnalyzeFile(UploadItem item)
        {
            item.Error = null;
            item.Plan = null;
            item.MapText = null;
            item.OutOfMap = 0;
            item.ExistingSeq = null;
            item.ExistingText = null;
            EtResultFile f = item.File;
            try
            {
                item.Program = EtParameterNameRule.ToTestProgram(f.TestProgram);
                item.Plan = EtUploadPlan.Build(f, Service.GetEtParameterList(item.Factory, f.FileDevice, item.Program));
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
                // Map 은 필수 (2026-10-15): 없으면 저장하지 않는다(좌표 없는 파일은 Map / YMS 에 그릴 수 없음)
                DieCoordinateMapper mapper = LoadMapper(item.Factory, f.FileDevice);
                if (mapper == null)
                {
                    item.Error = "Map 없음: File Map Setup 에 Map ID " + f.FileDevice + " (사용 Die 가 있는 Map) 을 먼저 만드세요";
                    return;
                }
                item.MapText = new string[f.DieCount];
                for (int i = 0; i < f.DieCount; i++)
                {
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

        /// <summary>Map ID = Device 의 사용 Die 로 만든 좌표 변환기. Map 이 없으면 null</summary>
        private DieCoordinateMapper LoadMapper(string factory, string device)
        {
            DataTable dies = Service.GetEtMapDieList(factory, device);
            bool used = dies.Rows.Cast<DataRow>().Any(r => r["USECODE"] != DBNull.Value
                && Convert.ToInt32(r["USECODE"], CultureInfo.InvariantCulture) == DieCoordinateMapper.UsedDieCode);
            return used ? DieCoordinateMapper.FromDieTable(dies, new DiePoint(0, 0)) : null;
        }

        /// <summary>DB 에 같은 Wafer(Lot + Slot) + 공정 파일이 있는지 본다(Slot 이 있을 때만).</summary>
        private void CheckExisting(UploadItem item)
        {
            item.ExistingSeq = null;
            item.ExistingText = null;
            if (item.File == null || item.Slot < 1 || item.Slot > 99) return;
            DataTable exist = Service.GetEtFileByKey(new string[] { item.Factory, item.File.LotId, item.Slot.ToString(CultureInfo.InvariantCulture), item.Program });
            if (exist.Rows.Count == 0) return;
            DataRow dr = exist.Rows[0];
            item.ExistingSeq = dr["SRC_FILE_SEQ"].ToString();
            item.ExistingText = "DB 에 같은 Wafer + 공정 파일 있음 (파일 번호 " + item.ExistingSeq + " / Device " + dr["DEVICE_ID"] + " / Test Date "
                + EtResultData.FormatTestTime(dr["TEST_TIME"].ToString()) + " / " + dr["LOAD_STATUS"] + ")";
        }

        /// <summary>상태 / 메시지를 정한다. 오류 > 중복(DB) > 경고 > 준비. 저장됨 / 실패는 그대로 둔다.</summary>
        private void ComputeStatus(UploadItem item)
        {
            if (item.Status == StSaved || item.Status == StFailed) return;
            if (item.Error != null)
            {
                item.Status = StError;
                item.Message = item.Error;
                return;
            }
            if (item.Slot < 1 || item.Slot > 99)
            {
                item.Status = StError;
                item.Message = "Slot 없음: Lot-Wafer No " + item.File.WaferId + " 끝에 '-숫자(1~99)' 가 없습니다. 정보 표의 Lot-Wafer No 를 더블클릭해 고치세요.";
                return;
            }
            EtUploadPlan plan = item.Plan;
            List<string> warnings = new List<string>();
            if (plan.Missing.Count > 0)
                warnings.Add("누락 " + plan.Missing.Count + " (예: " + string.Join(", ", plan.Missing.Take(2).ToArray()) + (plan.Missing.Count > 2 ? " ..." : string.Empty) + ")");
            if (plan.Conflicts.Count > 0)
                warnings.Add("이름 충돌 / 중복 헤더 " + plan.Conflicts.Count + " 건너뜀");
            // 등록 단위와 다른 Parameter 는 저장하지 않는다 (2026-10-16)
            if (plan.UnitMismatches.Count > 0)
                warnings.Add("단위 다름 " + plan.UnitMismatches.Count + " 건너뜀 (예: " + plan.UnitMismatches[0] + ")");
            if (item.OutOfMap > 0)
                warnings.Add("Map 밖 Die " + item.OutOfMap);
            if (plan.EmptyCount > 0)
                warnings.Add("빈 칸 값 " + plan.EmptyCount + " (판정 안 함)");
            // Slot 을 입력했으면 파서의 'Slot 기본값을 정하지 못함' 경고는 해결된 것이다
            warnings.AddRange(item.File.Warnings.Where(w => !w.StartsWith("Lot-Wafer No(", StringComparison.Ordinal)));
            List<string> notes = new List<string>();
            if (plan.Matched.Count > plan.JudgedCount)
                notes.Add("Spec 없음 · Enable N " + (plan.Matched.Count - plan.JudgedCount) + " (판정 안 함)");
            if (plan.Unused > 0)
                notes.Add("사용 안 함 " + plan.Unused + " 저장 안 함");

            item.Status = item.ExistingSeq != null ? StDuplicate : warnings.Count > 0 ? StWarning : StReady;
            List<string> all = new List<string>();
            if (item.ExistingText != null) all.Add(item.ExistingText);
            all.AddRange(warnings);
            all.AddRange(notes);
            item.Message = all.Count == 0 ? "저장할 수 있습니다." : string.Join(" / ", all.ToArray());
        }

        /// <summary>지금 파일의 상태 줄 / Slot 칸 / 결과 Control 을 다시 그린다. null 이면 비운다.</summary>
        private void ShowItem(UploadItem item)
        {
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                if (item == null)
                {
                    txtFile.Text = string.Empty;
                    txtSlot.Text = string.Empty;
                    lblState.Text = string.Empty;
                    lblMessage.Text = "[파일 열기] 로 E-Test 결과 파일(xlsx) 하나를 고르세요. Device / 공정 / Lot / Slot 은 파일명에서 읽고, 등록 Parameter(사용 Y)만 저장합니다.";
                    lblMessage.ForeColor = Color.DimGray;
                    resultView.Clear();
                    UpdateButtons();
                    return;
                }
                txtSlot.Text = item.File == null || item.Slot == 0 ? string.Empty : item.Slot.ToString(CultureInfo.InvariantCulture);
                lblState.Text = item.Status;
                lblState.ForeColor = item.Status == StError || item.Status == StFailed ? Color.Firebrick
                    : item.Status == StWarning || item.Status == StDuplicate ? Color.DarkOrange
                    : item.Status == StSaved ? Color.SteelBlue : Color.ForestGreen;
                lblMessage.Text = item.Message;
                lblMessage.ForeColor = SystemColors.ControlText;
                if (item.File == null) resultView.Clear();
                else resultView.ShowData(EtResultData.FromUpload(item.File, item.Plan, item.Program, item.Slot, item.WaferId,
                    item.MapText, item.Status, item.SavedSeq, item.Status != StSaved));
                UpdateButtons();
            }
        }

        /// <summary>
        /// 정보 표에서 Device / 공정 / Lot / Lot-Wafer No 를 더블클릭해 고치면(파일명이 틀렸을 때) 그 값으로 다시 분석한다(2026-10-15).
        /// 파일은 다시 읽지 않는다. Lot-Wafer No 를 고치면 Slot 도 그 끝 '-숫자' 로 다시 정한다. [다시 읽기] 는 파일명 값으로 돌아간다.
        /// 저장에는 고친 값이 들어간다(DEVICE_ID / TEST_PROGRAM / LOT_ID / LOT_WAFER_NO / WAFER_ID). 파일명(FILE_NAME)은 원래 이름 그대로.
        /// </summary>
        private void resultView_InfoEdited(object sender, InfoEditedEventArgs e)
        {
            UploadItem item = current;
            if (item == null || item.File == null || item.Status == StSaved || busy) return;
            if (e.Value.Length == 0)
            {
                Notify(e.Name + " 은(는) 비울 수 없습니다.", MessageBoxIcon.Warning);
                ShowItem(item);
                return;
            }
            EtResultFile f = item.File;
            switch (e.Name)
            {
                case EtResultData.InfoDevice:
                    f.FileDevice = e.Value;
                    break;
                case EtResultData.InfoProgram:
                    f.TestProgram = e.Value;
                    break;
                case EtResultData.InfoLot:
                    f.LotId = e.Value;
                    break;
                case EtResultData.InfoWaferNo:
                    f.WaferId = e.Value;
                    f.SlotNo = EtResultFileParser.SlotFromWaferNo(e.Value);
                    item.Slot = f.SlotNo;
                    break;
                default:
                    return;
            }
            item.Status = StReady;   // 실패 상태였어도 다시 정한다
            SetBusy(true);
            try
            {
                busyBar.Run("다시 분석", () => { AnalyzeFile(item); return true; });
            }
            catch (Exception ex)
            {
                item.Error = "분석 실패: " + Cause(ex).Message.Replace("\r\n", " ");
            }
            finally
            {
                SetBusy(false);
            }
            ComputeStatus(item);
            ShowItem(item);
            SetStatus(e.Name + " = " + e.Value + " 로 다시 분석 / " + item.Status);
        }

        private void txtSlot_Leave(object sender, EventArgs e)
        {
            ApplySlot();
        }

        private void txtSlot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            ApplySlot();
        }

        /// <summary>Slot 칸을 바꿨으면 WAFER_ID / DB 중복 / 상태를 다시 본다.</summary>
        private void ApplySlot()
        {
            UploadItem item = current;
            if (item == null || item.File == null || item.Status == StSaved || busy) return;
            int parsed;
            int slot = int.TryParse(txtSlot.Text.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out parsed) && parsed >= 1 && parsed <= 99 ? parsed : 0;
            if (slot == item.Slot) return;
            item.Slot = slot;
            if (item.Status == StFailed) item.Status = StReady;
            if (item.Error == null)
            {
                try
                {
                    // 서버 중복 확인: 입력 이벤트 도중이라 RunHere(화면 스레드, 진행 표시 + 모래시계)
                    busyBar.RunHere("중복 확인", () => { CheckExisting(item); return true; });
                }
                catch (Exception ex)
                {
                    item.Error = "중복 확인 실패: " + Cause(ex).Message;
                }
            }
            ComputeStatus(item);
            ShowItem(item);
        }

        #endregion

        #region [저장]

        private static bool Savable(UploadItem item)
        {
            return item != null && (item.Status == StReady || item.Status == StWarning || item.Status == StDuplicate);
        }

        /// <summary>
        /// 지금 파일을 저장한다. 저장 전에 한 번 묻는다(저장 Parameter x Die = 값 수). 중복(DB 에 같은 Wafer + 공정)이면 예 = 덮어쓰기 / 아니요 = 저장 안 함.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Slot 을 고친 뒤 Enter 없이 바로 [저장] 을 눌러도 반영한다
            ApplySlot();
            UploadItem item = current;
            if (item == null || busy) return;
            if (!Savable(item))
            {
                Notify("저장할 수 없습니다: " + item.Status + "\r\n\r\n" + item.Message, MessageBoxIcon.Warning);
                return;
            }
            string text = SaveConfirmText(item);
            if (item.Status == StDuplicate)
            {
                if (Ask(text + "\r\n\r\n" + item.ExistingText + "\r\n예 = 기존 파일을 지우고 다시 저장(덮어쓰기) / 아니요 = 저장하지 않음", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            else if (Ask(text, MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            string user = User();
            bool saved;
            SetBusy(true);
            try
            {
                saved = busyBar.Run("저장", () => SaveItem(item, user));
            }
            finally
            {
                SetBusy(false);
            }
            ShowItem(item);
            SetStatus((saved ? "저장 완료 / " : "저장 실패 / ") + Path.GetFileName(item.Path));
            Notify(saved ? item.Message + "\r\n\r\n저장한 결과는 E-Test 결과 조회 화면에서 봅니다." : item.Message, saved ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 저장 확인 문구 (2026-10-15): 파일 / WAFER_ID / 공정 / 저장 Parameter x Die = 값 수, 등록되지 않은 헤더는 저장하지 않는다는 안내, 경고.
        /// 숫자는 분석 결과(EtUploadPlan.Matched x Die)이고 서버 저장 건수와 다르면 저장이 MISMATCH 로 실패한다.
        /// </summary>
        private static string SaveConfirmText(UploadItem item)
        {
            return "E-Test 결과 파일을 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.\r\n\r\n"
                + "  파일: " + Path.GetFileName(item.Path) + "\r\n"
                + "  WAFER_ID " + item.WaferId + " / 공정 " + item.Program + "\r\n"
                + "  저장: Parameter " + item.Plan.Matched.Count + " x Die " + item.File.DieCount + " = 값 "
                + item.Plan.ValueCount(item.File).ToString("N0", CultureInfo.InvariantCulture) + "\r\n\r\n"
                + "등록되지 않은 헤더는 저장하지 않습니다 (" + (item.Plan.Skipped + item.Plan.Unused).ToString("N0", CultureInfo.InvariantCulture) + "개). "
                + "나중에 등록한 Parameter 는 이 파일을 다시 업로드해야 보입니다."
                + (item.Status == StWarning ? "\r\n\r\n경고: " + item.Message : string.Empty);
        }

        /// <summary>
        /// 파일 1개 저장: (덮어쓰기면 기존 파일 삭제) -> BeginEtFile(21항목) -> Die -> 값(등록 Parameter 만) -> CompleteEtFile(판정 + Map 좌표 + READY).
        /// 실패하면 이번에 만든 파일을 지우고 false. 진행 표시줄의 작업 스레드에서 돈다(화면 컨트롤을 만지지 않는다).
        /// </summary>
        private bool SaveItem(UploadItem item, string user)
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
                item.ExistingText = null;
                item.Status = StSaved;
                string mapId = done["MAP_ID"].ToString();
                item.Message = "저장 완료 / 파일 번호 " + seq + " / WAFER_ID " + head.Rows[0]["WAFER_ID"] + " / Spec Fail Die " + done["SPEC_FAIL_DIE"] + " / 값 "
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

        /// <summary>지금 연 파일 1개의 분석 / 저장 상태</summary>
        private sealed class UploadItem
        {
            public string Path;
            public string Factory;
            public string Status = StReady;
            public string Message = string.Empty;
            public EtResultFile File;
            public EtUploadPlan Plan;
            public string Program = string.Empty;
            public int Slot;
            /// <summary>Die 별 시스템 Map 좌표 "x,y" / "밖" (Map 확인 전이면 null)</summary>
            public string[] MapText;
            public int OutOfMap;
            public string ExistingSeq;
            public string ExistingText;
            public string SavedSeq = string.Empty;
            public string Error;

            public string WaferId
            {
                get { return File == null || Slot < 1 ? string.Empty : File.LotId + "-" + Slot.ToString("00", CultureInfo.InvariantCulture); }
            }
        }

        private void SetBusy(bool value)
        {
            busy = value;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnOpen.Enabled = !busy;
            btnReload.Enabled = !busy && current != null;
            btnSave.Enabled = !busy && Savable(current);
            txtFactory.Enabled = !busy;
            txtSlot.ReadOnly = busy || current == null || current.File == null || current.Status == StSaved;
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

        /// <summary>원격 호출 / 리플렉션 예외의 실제 원인</summary>
        private static Exception Cause(Exception ex)
        {
            while (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        /// <summary>파일 고르기(하나). 화면 Smoke 검사는 샘플 파일 경로를 돌려주도록 바꾼다.</summary>
        protected virtual string ChooseFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "E-Test 결과 파일 선택";
                dialog.Filter = "E-Test 결과 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*";
                dialog.Multiselect = false;
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

        #endregion
    }
}
