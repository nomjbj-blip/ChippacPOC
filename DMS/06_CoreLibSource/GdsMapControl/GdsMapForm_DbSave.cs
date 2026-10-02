using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map Setup 화면의 "DB 저장" 기능이다. (설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 7절 / 20절)
    /// 화면에 읽은 GDS 전체(LAYER / 원본 Grid 행 SOURCE / 화면 배치 도형 PLACED)를 DMS Service(RO GdsMapImport)로 나눠 보내고,
    /// 마지막에 서버가 DB 건수를 다시 세어 확정(READY)한다. DB 접속은 Service(Miracom.Middleware)만 사용한다.
    /// 2026-10-02 사용자 결정: 리비전 관리 없음. Map은 Factory + Device(제품)당 1개이고, 기존 데이터가 있으면 사용자가 삭제 후 재등록을 선택한다.
    ///
    /// 처리 흐름 (btnDbSave_Click)
    ///  1) 입력 / 도면 / AREF 확인 -> 2) 기존 데이터 확인(GetMapInfo). 있으면 내용을 보여 주고 "삭제 후 다시 등록?" 질문
    ///     -> 예: DeleteMap / 아니오: 저장하지 않음
    ///  3) 파일 SHA-256과 예상 건수 계산 -> 4) BeginMapImport -> 5) LAYER 1배치 -> 6) SOURCE 배치
    ///  7) PLACED 배치(화면 도형을 한 배치씩 추출) -> 8) CompleteMapImport
    ///  서비스 호출은 Task.Run으로 UI 스레드 밖에서 하고, 도형 추출만 UI 스레드에서 한 배치씩 한다.
    /// </summary>
    public partial class GdsMapForm
    {
        /// <summary>서비스 한 번에 보내는 행 수. 서버 최대 5,000건 이하로 둔다.</summary>
        private const int DbSaveBatchRows = 2000;

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
            foreach (Control control in new Control[] { lblDbFactory, txtDbFactory, lblDbDevice, txtDbDevice, btnDbSave, btnDbStop })
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
            btnDbSave.Enabled = !running;
            btnDbStop.Enabled = running;
            txtDbFactory.Enabled = txtDbDevice.Enabled = !running;
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
            string factory = txtDbFactory.Text.Trim();
            string deviceId = txtDbDevice.Text.Trim();
            if (factory.Length == 0 || deviceId.Length == 0)
            {
                MessageBox.Show(this, "Factory / Device를 입력하세요.", "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_dbSaveLibrary == null || map.Structure == null || String.IsNullOrEmpty(_dbSaveGdsPath))
            {
                MessageBox.Show(this, "먼저 GDS 파일을 읽어 화면에 표시하세요.", "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 현재 화면은 AREF를 펼치지 않는다. 저장하면 도형이 빠진 Map이 되므로 막는다(2026-10-02 사용자 결정).
            if (_dbSaveLibrary.Structures.Any(s => s.Layers.Any(l => l.Elements.Any(el => el is GdsARef))))
            {
                MessageBox.Show(this, "AREF(배열 참조)가 있는 도면은 아직 저장할 수 없습니다. 현재 화면이 AREF를 펼치지 않아 도형이 빠지기 때문입니다.",
                    "DB 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string user = Environment.UserName;
            _dbSaveCancel = new CancellationTokenSource();
            CancellationToken token = _dbSaveCancel.Token;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string jobSeq = null;
            SetDbSaveRunning(true);
            try
            {
                var service = new DACrux.SEMDMS.RO.GdsMapImport();
                GdsLibrary library = _dbSaveLibrary;

                // 2) 기존 데이터 확인. Device당 Map은 1개이므로 있으면 사용자가 삭제 후 재등록 여부를 정한다.
                SetDbSaveStatus("기존 데이터 확인 중", 0);
                string path = _dbSaveGdsPath;
                Task<string> hashTask = Task.Run(() => ComputeFileSha256(path));
                DataTable existing = await Task.Run(() => service.GetMapInfo(factory, deviceId));
                string sha256 = await hashTask;
                if (existing.Rows.Count > 0)
                {
                    if (!AskDeleteExistingMap(existing.Rows[0], factory, deviceId, sha256))
                    {
                        SetDbSaveStatus("DB 저장 취소 (기존 데이터 유지)", 0);
                        return;
                    }
                    SetDbSaveStatus("기존 데이터 삭제 중", 0);
                    await Task.Run(() => service.DeleteMap(factory, deviceId, user));
                }

                // 3) 예상 건수. 서버가 확정 시 DB 건수와 비교한다.
                long sourceCount = GdsMapSourceRowBuilder.CountSourceRows(library);
                long placedCount = 0, pointCount = 0;
                map.VisitPlacedElements(d => { placedCount++; pointCount += d.WorldPoints.Length; });

                // 4) 저장 시작
                string[] info = {
                    factory, deviceId, map.Structure.Name, Path.GetFileName(path), sha256,
                    library.UserUnit.ToString("G17", CultureInfo.InvariantCulture),
                    library.DatabaseUnit.ToString("G17", CultureInfo.InvariantCulture), user,
                    sourceCount.ToString(CultureInfo.InvariantCulture), placedCount.ToString(CultureInfo.InvariantCulture),
                    pointCount.ToString(CultureInfo.InvariantCulture) };
                DataTable job = await Task.Run(() => service.BeginMapImport(info));
                jobSeq = job.Rows[0]["IMPORT_JOB_SEQ"].ToString();
                long totalBatches = 1 + (sourceCount + DbSaveBatchRows - 1) / DbSaveBatchRows + (placedCount + DbSaveBatchRows - 1) / DbSaveBatchRows;
                long sentBatches = 0;

                // 5) LAYER: 화면 Layer 전체(PLACED FK 대상) + 원본에만 있는 Layer
                token.ThrowIfCancellationRequested();
                var colors = map.GetLayerDisplayItems().ToDictionary(item => item.LayerId, item => item.Color.ToArgb());
                foreach (int id in GdsMapSourceRowBuilder.CollectLayerIds(library))
                    if (!colors.ContainsKey(id)) colors[id] = System.Drawing.Color.Gray.ToArgb();
                string[,] layerRows = GdsMapSourceRowBuilder.BuildLayerRows(colors.OrderBy(pair => pair.Key).ToList());
                await Task.Run(() => service.CreateLayerBatch(jobSeq, 0, layerRows, user));
                SetDbSaveStatus("LAYER 저장 완료", ++sentBatches * 100 / totalBatches);

                // 6) SOURCE: 원본 Grid 행. 배치 단위로 만들어 메모리를 제한한다.
                long sourceNo = 0;
                foreach (string[,] rows in GdsMapSourceRowBuilder.BuildSourceBatches(library, DbSaveBatchRows))
                {
                    token.ThrowIfCancellationRequested();
                    long no = sourceNo++;
                    await Task.Run(() => service.CreateSourceBatch(jobSeq, no, rows, user));
                    SetDbSaveStatus("원본 행 저장 " + sourceNo + "배치", ++sentBatches * 100 / totalBatches);
                }

                // 7) PLACED: 화면 배치 도형. UI 스레드에서 한 배치만 복사하고 서버 응답을 기다린 뒤 다음 배치를 만든다.
                await map.VisitPlacedElementBatchesAsync(async batch =>
                {
                    long no = batch.BatchNo;
                    string[,] rows = await Task.Run(() => GdsMapPlacedRowBuilder.BuildRows(batch.Elements));
                    await Task.Run(() => service.CreatePlacedBatch(jobSeq, no, rows, user));
                    SetDbSaveStatus("도형 저장 " + (no + 1) + "배치", ++sentBatches * 100 / totalBatches);
                }, DbSaveBatchRows, 8L * 1024 * 1024, token);

                // 8) 확정: 서버가 DB 건수 / 배치 번호를 다시 확인한다.
                SetDbSaveStatus("저장 확정 중", 100);
                string finalSeq = jobSeq;
                DataRow result = (await Task.Run(() => service.CompleteMapImport(finalSeq, user))).Rows[0];
                bool ok = result["RESULT"].ToString() == "OK";
                string message = "Factory " + factory + " / Device " + deviceId + " / 작업 번호(IMPORT_JOB_SEQ) " + jobSeq + "\r\n"
                    + "원본 행 " + result["DB_SOURCE"] + " / 도형 " + result["DB_PLACED"] + " / 좌표 " + result["DB_POINT"] + "\r\n"
                    + "소요 " + watch.Elapsed.TotalSeconds.ToString("0.0") + "초\r\n" + result["MESSAGE"];
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
        /// 기존 데이터의 상태 / 등록 정보 / 건수 / 같은 파일 여부를 보여 주고 삭제 후 재등록할지 묻는다.
        /// Device당 Map은 1개라서 새로 등록하려면 기존 데이터(Chain 설정 포함)를 지워야 한다.
        /// </summary>
        private bool AskDeleteExistingMap(DataRow row, string factory, string deviceId, string sha256)
        {
            string status = row["MAP_STATUS"].ToString() == "READY" ? "저장 완료" : "저장 미완료(중단 / 실패)";
            bool sameFile = String.Equals(row["GDS_SHA256"].ToString().Trim(), sha256, StringComparison.OrdinalIgnoreCase);
            string text = "Factory " + factory + " / Device " + deviceId + "에 기존 GDS 데이터가 있습니다.\r\n\r\n"
                + "상태: " + status + "\r\n"
                + "파일: " + row["GDS_FILE_NAME"] + (sameFile ? " (지금 파일과 같은 파일)" : " (지금 파일과 다른 파일)") + "\r\n"
                + "등록: " + row["CREATE_TIME"] + " / " + row["CREATE_USER"] + "\r\n"
                + "건수: 원본 행 " + row["SOURCE_COUNT"] + " / 도형 " + row["PLACED_COUNT"] + " / 좌표 " + row["POINT_COUNT"] + "\r\n\r\n"
                + "기존 데이터를 삭제하고 다시 등록하시겠습니까?\r\n(이 Device의 Chain 설정도 함께 삭제됩니다)";
            return MessageBox.Show(this, text, "기존 데이터 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
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
        /// 중지한 저장 작업을 CANCELLED로 바꾸고 안내한다. 이어서 저장하는 기능은 없다(사용자 결정: 항상 삭제 후 재등록).
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

        /// <summary>GDS 원본 파일의 SHA-256(대문자 16진수 64자). 기존 데이터가 같은 파일인지 보여 주는 데 쓴다.</summary>
        private static string ComputeFileSha256(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024))
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(stream);
                var text = new StringBuilder(64);
                foreach (byte b in hash) text.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                return text.ToString();
            }
        }
    }
}
