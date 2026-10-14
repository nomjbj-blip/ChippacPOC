using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map Setup 화면의 "DB 저장" 기능이다. (설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 7절 / 20절)
    /// 화면에 읽은 GDS 중 Layer 목록에서 체크된(화면에 그려진) Layer의 LAYER / 화면 배치 도형 PLACED를
    /// DMS Service(RO GdsMapImport)로 나눠 보내고,
    /// 마지막에 서버가 DB 건수를 다시 세어 확정(READY)한다. DB 접속은 Service(Miracom.Middleware)만 사용한다.
    /// 2026-10-02 결정: 리비전 관리 없음. Map은 Factory + Device(제품)당 1개이고, 기존 데이터가 있으면 사용자가 삭제 후 재등록을 선택한다.
    ///
    /// 2026-10-06 결정(설계 21절): 데이터 양을 줄이기 위해 체크된 Layer만 저장한다.
    /// 2026-10-12: 원본 Grid 행(TQP_GDS_SOURCE_EL)은 조회하는 곳이 없어 저장하지 않는다(테이블 제거).
    ///
    /// 처리 흐름 (btnDbSave_Click)
    ///  1) 입력 / 도면 / AREF 확인 -> 1-1) 저장 Layer 스냅샷. 일부만 체크되었으면 제외 Layer를 보여 주고 확인
    ///  -> 2) 기존 데이터 확인(GetMapInfo). 있으면 내용을 보여 주고 "삭제 후 다시 등록?" 질문
    ///     -> 예: DeleteMap / 아니오: 저장하지 않음
    ///  3) 예상 건수 계산 (파일 SHA-256 은 2026-10-06 부터 계산하지 않음) -> 4) BeginMapImport -> 5) LAYER 1배치
    ///  6) PLACED 배치(화면 도형을 한 배치씩 추출) -> 7) CompleteMapImport
    ///  서비스 호출은 Task.Run으로 UI 스레드 밖에서 하고, 도형 추출만 UI 스레드에서 한 배치씩 한다.
    /// </summary>
    public partial class GdsMapForm
    {
        /// <summary>서비스 한 번에 보내는 행 수. 서버 최대 10,000건 이하로 둔다. 호출 횟수(Remoting 왕복 + 배치 기록)를 줄이기 위해 2,000에서 10,000으로 올렸다(2026-10-06).</summary>
        private const int DbSaveBatchRows = 10000;

        private GdsLibrary _dbSaveLibrary;
        private string _dbSaveGdsPath;
        private CancellationTokenSource _dbSaveCancel;

        /// <summary>단독 실행에서는 DMS Service 연결 정보가 없으므로 저장 UI를 숨긴다. 호스트 모드면 ShowDbSaveControls(true)로 다시 보인다.</summary>
        private void InitializeDbSaveState()
        {
            ShowDbSaveControls(false);
        }

        /// <summary>DB 저장 입력 / 버튼의 표시 여부를 바꾼다.</summary>
        private void ShowDbSaveControls(bool visible)
        {
            foreach (Control control in new Control[] { lblDbDevice, txtDbDevice, btnDbSave, btnDbStop })
                control.Visible = visible;
            btnDbStop.Enabled = false;
        }

        /// <summary>저장 중에는 도면을 바꾸는 조작을 막는다. 저장 도중 도면이 바뀌면 배치 ID가 달라지기 때문이다.</summary>
        private void SetDbSaveRunning(bool running)
        {
            button1.Enabled = !running;
            btnLayer.Enabled = !running;
            btnLayerCheckAll.Enabled = !running;
            btnLayerCheckNone.Enabled = !running;
            chkLayerItems.Enabled = !running;
            btnDbSave.Enabled = !running;
            btnDbStop.Enabled = running;
            txtDbDevice.Enabled = !running;
            progressMapLoad.Visible = running;
            if (running)
            {
                progressMapLoad.Style = ProgressBarStyle.Continuous;
                progressMapLoad.Value = 0;
            }
        }

        /// <summary>DB 저장 버튼. 처리 흐름은 클래스 설명 참고.</summary>
        private async void btnDbSave_Click(object sender, EventArgs e)
        {
            // Factory: 화면 입력 대신 DACrux 로그인 Factory(DACrux.Base.GlobalVariable.Factory) (2026-10-07 요청)
            string factory = DmsHostContext.Factory;
            string deviceId = txtDbDevice.Text.Trim();
            if (factory.Length == 0)
            {
                MessageBox.Show(this, "Factory 정보가 없습니다. DACrux에 로그인한 상태에서 메뉴로 여세요.", "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (deviceId.Length == 0)
            {
                MessageBox.Show(this, "Device를 입력하세요.", "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_dbSaveLibrary == null || map.Structure == null || String.IsNullOrEmpty(_dbSaveGdsPath))
            {
                MessageBox.Show(this, "먼저 GDS 파일을 읽어 화면에 표시하세요.", "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 현재 화면은 AREF를 펼치지 않는다. 저장하면 도형이 빠진 Map이 되므로 막는다(2026-10-02 결정).
            if (_dbSaveLibrary.Structures.Any(s => s.Layers.Any(l => l.Elements.Any(el => el is GdsARef))))
            {
                MessageBox.Show(this, "AREF(배열 참조)가 있는 도면은 아직 저장할 수 없습니다. 현재 화면이 AREF를 펼치지 않아 도형이 빠지기 때문입니다.",
                    "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1-1) 저장 Layer: 지금 체크된 Layer를 복사해 저장 도중 체크가 바뀌어도 예상 건수와 실제 전송이 같게 한다.
            var layerItems = map.GetLayerDisplayItems();
            var saveLayerIds = new HashSet<int>(layerItems.Where(item => item.Visible).Select(item => item.LayerId));
            if (saveLayerIds.Count == 0)
            {
                MessageBox.Show(this, "저장할 Layer가 없습니다. Layer 목록에서 저장할 Layer를 체크하세요.", "DB 저장",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (saveLayerIds.Count < layerItems.Count && !AskSaveSelectedLayers(layerItems, saveLayerIds))
                return;

            // 저장 기준(2026-10-07): 불러온 GDS 원본(Front side) 좌표 그대로 저장한다. Front / Back 보기는 View 화면에서 로직으로 전환한다.

            string user = Environment.UserName;
            _dbSaveCancel = new CancellationTokenSource();
            CancellationToken token = _dbSaveCancel.Token;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // 구간별 시간: 어디서 시간이 걸리는지 완료 창에서 바로 보기 위한 측정(2026-10-06 요청)
            var timing = new DbSaveTiming();
            var phase = System.Diagnostics.Stopwatch.StartNew();
            string jobSeq = null;
            SetDbSaveRunning(true);
            try
            {
                var service = new DACrux.SEMDMS.RO.GdsMapImport();
                GdsLibrary library = _dbSaveLibrary;

                // 2) 기존 데이터 확인. Device당 Map은 1개이므로 있으면 사용자가 삭제 후 재등록 여부를 정한다.
                SetDbSaveStatus("기존 데이터 확인 중", 0);
                string path = _dbSaveGdsPath;
                // 원본 파일 SHA-256 은 계산하지 않는다 (2026-10-06 요청). 재등록 확인창의 같은 파일 표시에만 쓰였다.
                DataTable existing = await Task.Run(() => service.GetMapInfo(factory, deviceId));
                timing.CheckMs = phase.ElapsedMilliseconds;
                if (existing.Rows.Count > 0)
                {
                    phase.Restart();
                    bool delete = AskDeleteExistingMap(existing.Rows[0], factory, deviceId);
                    timing.AskMs = phase.ElapsedMilliseconds;
                    if (!delete)
                    {
                        SetDbSaveStatus("DB 저장 취소 (기존 데이터 유지)", 0);
                        return;
                    }
                    SetDbSaveStatus("기존 데이터 삭제 중", 0);
                    phase.Restart();
                    await Task.Run(() => service.DeleteMap(factory, deviceId, user));
                    timing.DeleteMs = phase.ElapsedMilliseconds;
                }

                // 3) 예상 건수. 서버가 확정 시 DB 건수와 비교한다.
                phase.Restart();
                long placedCount = 0, pointCount = 0;
                map.VisitPlacedElements(saveLayerIds, d => { placedCount++; pointCount += d.WorldPoints.Length; });

                // 4) 저장 시작. 5번째(예전 GDS SHA-256 자리)와 9번째(예전 원본 행 수 자리) 빈 값은 서버가 무시한다.
                string[] info = {
                    factory, deviceId, map.Structure.Name, Path.GetFileName(path), string.Empty,
                    library.UserUnit.ToString("G17", CultureInfo.InvariantCulture),
                    library.DatabaseUnit.ToString("G17", CultureInfo.InvariantCulture), user,
                    string.Empty, placedCount.ToString(CultureInfo.InvariantCulture),
                    pointCount.ToString(CultureInfo.InvariantCulture), "FRONT" };
                DataTable job = await Task.Run(() => service.BeginMapImport(info));
                jobSeq = job.Rows[0]["IMPORT_JOB_SEQ"].ToString();
                timing.BeginMs = phase.ElapsedMilliseconds;
                long totalBatches = 1 + (placedCount + DbSaveBatchRows - 1) / DbSaveBatchRows;
                long sentBatches = 0;

                // 5) LAYER: 저장 Layer만(PLACED FK 대상).
                token.ThrowIfCancellationRequested();
                phase.Restart();
                var colors = layerItems.Where(item => saveLayerIds.Contains(item.LayerId))
                    .ToDictionary(item => item.LayerId, item => item.Color.ToArgb());
                string[,] layerRows = GdsMapLayerRowBuilder.BuildLayerRows(colors.OrderBy(pair => pair.Key).ToList());
                await Task.Run(() => service.CreateLayerBatch(jobSeq, 0, layerRows, user));
                timing.LayerMs = phase.ElapsedMilliseconds;
                SetDbSaveStatus("LAYER 저장 완료", ++sentBatches * 100 / totalBatches);

                // 6) PLACED: 저장 Layer의 화면 배치 도형. UI 스레드에서 한 배치만 복사하고 서버 응답을 기다린 뒤 다음 배치를 만든다.
                //    전체 - 행 생성 - Service 호출 = UI 스레드 배치 복사 / 대기 시간
                var build = new System.Diagnostics.Stopwatch();
                var call = new System.Diagnostics.Stopwatch();
                long placedBatches = 0;
                phase.Restart();
                await map.VisitPlacedElementBatchesAsync(saveLayerIds, async batch =>
                {
                    long no = batch.BatchNo;
                    build.Start();
                    string[,] rows = await Task.Run(() => GdsMapPlacedRowBuilder.BuildRows(batch.Elements));
                    build.Stop();
                    call.Start();
                    await Task.Run(() => service.CreatePlacedBatch(jobSeq, no, rows, user));
                    call.Stop();
                    placedBatches++;
                    SetDbSaveStatus("도형 저장 " + (no + 1) + "배치", ++sentBatches * 100 / totalBatches);
                }, DbSaveBatchRows, 8L * 1024 * 1024, token);
                timing.PlacedMs = phase.ElapsedMilliseconds;
                timing.PlacedBuildMs = build.ElapsedMilliseconds;
                timing.PlacedCallMs = call.ElapsedMilliseconds;
                timing.PlacedBatches = placedBatches;

                // 7) 확정: 서버가 DB 건수를 다시 확인한다.
                SetDbSaveStatus("저장 확정 중", 100);
                string finalSeq = jobSeq;
                phase.Restart();
                DataRow result = (await Task.Run(() => service.CompleteMapImport(finalSeq, user))).Rows[0];
                timing.CompleteMs = phase.ElapsedMilliseconds;
                bool ok = result["RESULT"].ToString() == "OK";
                string message = "Factory " + factory + " / Device " + deviceId + " / 작업 번호(IMPORT_JOB_SEQ) " + jobSeq + "\r\n"
                    + "저장 Layer " + saveLayerIds.Count + " / " + layerItems.Count + "개\r\n"
                    + "도형 " + result["DB_PLACED"] + " / 좌표 " + result["DB_POINT"] + "\r\n"
                    + "소요 " + watch.Elapsed.TotalSeconds.ToString("0.0") + "초 (삭제 확인 창 대기 " + Sec(timing.AskMs) + "초 포함)\r\n"
                    + result["MESSAGE"] + "\r\n\r\n" + timing.ToText();
                SetDbSaveStatus(ok ? "DB 저장 완료 / " + factory + " / " + deviceId : "DB 저장 확인 필요 / " + result["MESSAGE"], 100);
                MessageBox.Show(this, message, ok ? "DB 저장 완료" : "DB 저장 건수 불일치", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (OperationCanceledException)
            {
                SetDbSaveStatus("DB 저장 중지", 0);
                await CancelStoppedImport(jobSeq, user);
            }
            catch (Exception ex)
            {
                SetDbSaveStatus("DB 저장 실패", 0);
                MessageBox.Show(this, ex.Message + (jobSeq == null ? "" : "\r\n\r\n저장된 일부 데이터는 남아 있습니다. 다시 저장하면 기존 데이터 삭제 여부를 묻습니다."),
                    "DB 저장 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dbSaveCancel.Dispose();
                _dbSaveCancel = null;
                SetDbSaveRunning(false);
            }
        }

        /// <summary>
        /// 기존 데이터의 상태 / 등록 정보 / 건수 / 파일 이름을 보여 주고 삭제 후 재등록할지 묻는다.
        /// Device당 Map은 1개라서 새로 등록하려면 기존 데이터(Chain 설정 포함)를 지워야 한다.
        /// 2026-10-06: 파일 SHA-256 계산을 빼서 "같은 파일 / 다른 파일" 표시는 하지 않는다. 같은 도면인지는 파일 이름 / 등록 시각으로 판단한다.
        /// </summary>
        private bool AskDeleteExistingMap(DataRow row, string factory, string deviceId)
        {
            string status = row["MAP_STATUS"].ToString() == "READY" ? "저장 완료" : "저장 미완료(중단 / 실패)";
            string text = "Factory " + factory + " / Device " + deviceId + "에 기존 GDS 데이터가 있습니다.\r\n\r\n"
                + "상태: " + status + "\r\n"
                + "파일: " + row["GDS_FILE_NAME"] + "\r\n"
                + "등록: " + row["CREATE_TIME"] + " / " + row["CREATE_USER"] + "\r\n"
                + "건수: 도형 " + row["PLACED_COUNT"] + " / 좌표 " + row["POINT_COUNT"] + "\r\n\r\n"
                + "기존 데이터를 삭제하고 다시 등록하시겠습니까?\r\n(이 Device의 Chain 설정도 함께 삭제됩니다)";
            return MessageBox.Show(this, text, "기존 데이터 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>
        /// 일부 Layer만 체크된 경우 저장 범위와 제외 Layer를 보여 주고 계속할지 묻는다(설계 21절).
        /// 제외 Layer는 조회 / Chain에서 쓸 수 없고, 다시 쓰려면 삭제 후 재등록해야 하므로 기본 버튼은 아니오다.
        /// </summary>
        private bool AskSaveSelectedLayers(IList<LayerDisplayItem> layerItems, ISet<int> saveLayerIds)
        {
            const int maxShown = 30;
            List<int> excluded = layerItems.Select(item => item.LayerId).Where(id => !saveLayerIds.Contains(id)).OrderBy(id => id).ToList();
            string excludedText = String.Join(", ", excluded.Take(maxShown)) + (excluded.Count > maxShown ? " 외 " + (excluded.Count - maxShown) + "개" : "");
            string text = "전체 " + layerItems.Count + "개 Layer 중 체크된 " + saveLayerIds.Count + "개 Layer만 저장합니다.\r\n\r\n"
                + "저장: " + String.Join(", ", saveLayerIds.OrderBy(id => id)) + "\r\n"
                + "제외: " + excludedText + "\r\n\r\n"
                + "제외한 Layer는 조회 화면과 Chain에서 사용할 수 없습니다.\r\n"
                + "나중에 필요하면 삭제 후 다시 등록해야 하며, 이때 Chain 설정도 함께 삭제됩니다.\r\n\r\n"
                + "체크된 Layer만 저장하시겠습니까?";
            return MessageBox.Show(this, text, "저장 Layer 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>저장 중지 버튼. 현재 배치가 끝난 뒤 멈춘다.</summary>
        private void btnDbStop_Click(object sender, EventArgs e)
        {
            if (_dbSaveCancel != null) _dbSaveCancel.Cancel();
            btnDbStop.Enabled = false;
            SetDbSaveStatus("현재 배치가 끝나면 중지합니다", progressMapLoad.Value);
        }

        /// <summary>
        /// 중지한 저장 작업을 CANCELLED로 바꾸고 안내한다. 이어서 저장하는 기능은 없다(결정: 항상 삭제 후 재등록).
        /// 저장된 일부 데이터는 남으며, 다음 저장 때 기존 데이터로 표시되어 삭제 여부를 묻는다.
        /// </summary>
        private async Task CancelStoppedImport(string jobSeq, string user)
        {
            if (jobSeq != null)
            {
                try
                {
                    var service = new DACrux.SEMDMS.RO.GdsMapImport();
                    await Task.Run(() => service.CancelImportJob(jobSeq, user));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "저장 작업 취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show(this, "저장을 중지했습니다." + (jobSeq == null ? "" : " (작업 번호 " + jobSeq + ")")
                + "\r\n다시 저장하면 지금까지 저장된 데이터를 삭제하고 처음부터 등록합니다.",
                "DB 저장 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>StatusStrip의 진행률과 문구를 바꾼다.</summary>
        private void SetDbSaveStatus(string text, long percent)
        {
            lblMapStatus.Text = text;
            progressMapLoad.Value = (int)Math.Max(progressMapLoad.Minimum, Math.Min(progressMapLoad.Maximum, percent));
            statusStripMap.Refresh();
        }

        /// <summary>밀리초를 "0.0" 초 문자열로 바꾼다.</summary>
        private static string Sec(long ms)
        {
            return (ms / 1000.0).ToString("0.0", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DB 저장 구간별 시간(밀리초). 완료 창에 표시해 어디서 시간이 걸리는지 바로 보게 한다.
        /// Service 호출 시간 = Remoting 직렬화 / 전송 + 서버 BSL + DB Insert(DB 대기 포함).
        /// </summary>
        private sealed class DbSaveTiming
        {
            public long CheckMs, AskMs, DeleteMs, BeginMs, LayerMs;
            public long PlacedMs, PlacedBuildMs, PlacedCallMs, PlacedBatches;
            public long CompleteMs;

            public string ToText()
            {
                return "[구간별 시간]\r\n"
                    + "기존 데이터 확인 " + Sec(CheckMs) + "초 / 기존 데이터 삭제 " + Sec(DeleteMs) + "초\r\n"
                    + "건수 계산 + 저장 시작 " + Sec(BeginMs) + "초 / LAYER " + Sec(LayerMs) + "초\r\n"
                    + "도형 " + Sec(PlacedMs) + "초 (" + PlacedBatches + "배치 / Service 호출 " + Sec(PlacedCallMs)
                    + "초 / 행 생성 " + Sec(PlacedBuildMs) + "초 / 배치 복사 " + Sec(PlacedMs - PlacedCallMs - PlacedBuildMs) + "초)\r\n"
                    + "저장 확정(DB 건수 확인) " + Sec(CompleteMs) + "초";
            }
        }
    }
}
