using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View 화면의 "Chain 저장(DB)" 기능이다. (설계: 문서/2026-10-06_GDS_Chain_DB저장_설계.md 10.1절, 문서/2026-10-06_GDS_Map_화면역할_재정의_설계.md 4.4절)
    /// 버전 이력 없음(덮어쓰기) / 변경된 Chain 전체 저장 / 삭제는 실제 삭제 / DMS 호스트 모드에서만 사용 (2026-10-06).
    /// Setup 화면에서 이 화면으로 옮겼다 (2026-10-06). 화면 도형이 DB 도형 그대로라서 GDS 파일 SHA-256 / DB Map Layer 범위 검사는 하지 않는다.
    ///
    /// 처리 흐름 (BtnChainDbSave_Click)
    ///  1) 조회한 Map / 편집 모드 확인 -> 2) DB Map이 아직 READY인지 확인(다른 사용자가 삭제 / 재등록했을 수 있음)
    ///  -> 3) Chain마다 저장 행(Layer / 규칙 / Member / 편집 기록) 생성 + 검사(Input 1개 / Output ≤ 1개 / 포함 도형 있음)
    ///  -> 4) 변경 여부(마지막 저장 지문과 비교) / 삭제 대기 Chain을 보여 주고 확인.
    ///     새 Chain인데 DB에 같은 Code가 있으면(불러온 뒤 다른 사용자가 저장) 덮어쓰지 않고 그 Chain만 제외한다.
    ///  -> 5) 삭제 대기 Chain 삭제 -> Chain마다 BeginChainSave -> Member / 편집 배치 -> CompleteChainSave
    /// 서비스 호출은 Task.Run으로 UI 스레드 밖에서 한다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>서비스 한 번에 보내는 Chain Member / 편집 행 수. 서버 최대 10,000건과 같다.</summary>
        private const int ChainDbBatchRows = 10000;

        /// <summary>화면에서 지웠지만 아직 DB에서 지우지 않은 저장된 Chain. 다음 Chain 저장 때 실제 삭제한다.</summary>
        private readonly List<ChainDefinition> _deletedDbChains = new List<ChainDefinition>();
        private bool _chainDbSaving;

        /// <summary>Chain 하나를 DB에 보낼 행과 검사 결과. 저장 직전 화면 상태를 복사해 저장 도중 편집과 섞이지 않게 한다.</summary>
        private sealed class ChainSaveData
        {
            public ChainDefinition Chain;
            public string[,] Layers;
            public string[,] Rules;
            public List<string[]> Members = new List<string[]>();
            public List<string[]> Edits = new List<string[]>();
            public string Fingerprint;
            public List<string> Problems = new List<string>();
            public bool Changed;
        }

        /// <summary>DB 저장은 DMS Service 연결 정보가 있는 DACrux 안에서만 가능하므로 호스트 모드에서만 버튼을 보인다.</summary>
        private void InitializeChainDbState(bool hostedByDms)
        {
            _chainDbEnabled = hostedByDms;
            _btnChainDbSave.Visible = hostedByDms;
        }

        /// <summary>DMS Service로 Chain을 저장 / 불러올 수 있는지(DACrux 호스트 모드). 단독 실행에서는 DB Chain을 읽지 않는다.</summary>
        private bool _chainDbEnabled;

        /// <summary>
        /// 저장하지 않은 Chain 변경 수: 새 Chain + 마지막 저장 내용과 지문이 다른 Chain + DB 삭제 대기 Chain.
        /// 지문은 저장 때와 같은 계산(BuildChainSaveData)을 써서 "저장하면 보낼 것"이 있는지와 같게 판단한다.
        /// </summary>
        private int CountUnsavedChains()
        {
            CaptureActiveChain();
            return _deletedDbChains.Count + _chains.Count(chain => chain.ChainSeq == null
                || !String.Equals(BuildChainSaveData(chain).Fingerprint, chain.SavedFingerprint, StringComparison.Ordinal));
        }

        /// <summary>저장하지 않은 Chain 변경이 있으면 action(예: "다른 Device를 선택")을 계속할지 묻는다. 없으면 바로 true.</summary>
        private bool ConfirmDiscardUnsavedChains(string action)
        {
            if (_chainDbSaving) return false;
            int unsaved = CountUnsavedChains();
            if (unsaved == 0) return true;
            return MessageBox.Show(this, "저장하지 않은 Chain 변경이 " + unsaved + "건 있습니다(새 Chain / 수정 / 삭제 대기).\r\n\r\n"
                    + action + "하면 변경 내용이 사라집니다. 계속하시겠습니까?\r\n(저장하려면 [아니오] 후 [Chain 저장(DB)])",
                "저장하지 않은 Chain", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>Chain 저장 버튼. 처리 흐름은 클래스 설명 참고.</summary>
        private async void BtnChainDbSave_Click(object sender, EventArgs e)
        {
            if (_chainDbSaving) return;
            MapItem target = _shownMap;
            if (target == null || map.Structure == null)
            {
                MessageBox.Show(this, "먼저 Device를 선택하고 [조회]로 도면을 표시하세요.", "Chain 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string factory = target.Factory;
            string deviceId = target.DeviceId;
            if (_chainPointCaptureMode != ChainPointCaptureMode.None || _chainEditMode != ChainEditMode.None || _chainTraceInProgress)
            {
                MessageBox.Show(this, "Input / Output 지정이나 수동 편집을 마친 뒤 저장하세요.", "Chain 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CaptureActiveChain();
            if (_chains.Count == 0 && _deletedDbChains.Count == 0)
            {
                MessageBox.Show(this, "저장할 Chain이 없습니다.", "Chain 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string user = Environment.UserName;
            SetChainDbSaving(true);
            try
            {
                // 2) 조회한 뒤 다른 사용자가 Map을 삭제 / 재등록했으면 도형 ID가 달라질 수 있으므로 아직 READY인지 확인한다.
                SetChainDbStatus("DB Map 확인 중");
                var view = new DACrux.SEMDMS.RO.GdsMapView();
                var chainService = new DACrux.SEMDMS.RO.GdsChain();
                DataTable mapInfo = await Task.Run(() => view.GetMapInfo(factory, deviceId));
                if (mapInfo.Rows.Count == 0 || mapInfo.Rows[0]["MAP_STATUS"].ToString() != "READY")
                {
                    MessageBox.Show(this, "Factory " + factory + " / Device " + deviceId + "의 GDS Map이 없거나 저장 완료 상태가 아닙니다.\r\n"
                        + "다른 사용자가 Map을 삭제 / 재등록했을 수 있습니다. Device를 다시 선택해 조회하세요.",
                        "Chain 저장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataTable dbChains = await Task.Run(() => chainService.GetChainList(factory, deviceId));
                var dbChainByCode = dbChains.Rows.Cast<DataRow>()
                    .ToDictionary(row => row["CHAIN_CODE"].ToString(), row => row["CHAIN_SEQ"].ToString(), StringComparer.Ordinal);

                // 3) 저장 행 생성과 검사
                SetChainDbStatus("Chain 저장 내용 확인 중");
                List<ChainSaveData> data = _chains.Select(BuildChainSaveData).ToList();
                foreach (ChainSaveData item in data)
                {
                    item.Changed = item.Chain.ChainSeq == null
                        || !String.Equals(item.Fingerprint, item.Chain.SavedFingerprint, StringComparison.Ordinal);
                    // [조회] 때 DB Chain을 불러오므로, 새 Chain의 Code가 DB에 있으면 그 뒤 다른 사용자가 같은 Code로 저장한 것이다.
                    // 남의 Chain을 덮어쓰지 않도록 이 Chain만 저장에서 제외하고 안내한다.
                    if (item.Chain.ChainSeq == null && dbChainByCode.ContainsKey(item.Chain.Code))
                        item.Problems.Add("DB에 같은 Code의 Chain이 이미 있음(다른 사용자가 저장했을 수 있음). Device를 다시 조회해 불러온 뒤 편집하세요");
                }
                // 4) 확인창
                List<ChainSaveData> invalid = data.Where(item => item.Changed && item.Problems.Count > 0).ToList();
                List<ChainSaveData> toSave = data.Where(item => item.Changed && item.Problems.Count == 0).ToList();
                if (!AskChainDbSave(toSave, invalid, data.Count(item => !item.Changed)))
                    return;

                // 5) 삭제 -> 저장
                var failures = new List<string>();
                int deleted = 0, saved = 0, done = 0, total = _deletedDbChains.Count + toSave.Count;
                foreach (ChainDefinition removed in _deletedDbChains.ToList())
                {
                    SetChainDbStatus("Chain 삭제 " + removed.Name + " (" + (++done) + "/" + total + ")");
                    string seq = removed.ChainSeq;
                    await Task.Run(() => chainService.DeleteChain(factory, deviceId, seq, user));
                    _deletedDbChains.Remove(removed);
                    deleted++;
                }
                foreach (ChainSaveData item in toSave)
                {
                    SetChainDbStatus("Chain 저장 " + item.Chain.Name + " (" + (++done) + "/" + total + ")");
                    string result = await SaveOneChain(chainService, item, factory, deviceId, user);
                    if (result == null) saved++;
                    else failures.Add(item.Chain.Name + " : " + result);
                }
                _chainList.Invalidate();
                string message = "저장 " + saved + "개 / 삭제 " + deleted + "개 / 변경 없음 " + data.Count(item => !item.Changed) + "개"
                    + (invalid.Count > 0 ? " / 제외 " + invalid.Count + "개" : "")
                    + (failures.Count > 0 ? "\r\n\r\n실패:\r\n" + String.Join("\r\n", failures)
                        + "\r\n\r\n실패한 Chain은 DB에 '저장 미완료'로 남습니다. 다시 저장하면 처음부터 저장합니다." : "");
                SetChainDbStatus("Chain 저장 완료 / " + message.Split('\r')[0]);
                MessageBox.Show(this, "Factory " + factory + " / Device " + deviceId + "\r\n" + message,
                    failures.Count == 0 ? "Chain 저장 완료" : "Chain 저장 일부 실패", MessageBoxButtons.OK,
                    failures.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                SetChainDbStatus("Chain 저장 실패");
                MessageBox.Show(this, ex.Message, "Chain 저장 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetChainDbSaving(false);
            }
        }

        /// <summary>
        /// Chain 하나의 저장 행을 만든다. 포함 도형은 화면 표시와 같은 규칙(GetChainKeys)으로 계산한다.
        ///  - 역할: Input = INPUT, Output = OUTPUT, 나머지 = MEMBER
        ///  - 출처: Input / Output = ENDPOINT, 자동 탐색 결과 = TRACE, 그 외(수동 추가) = MANUAL
        ///  - 편집 기록: 수동 추가 = ADD, 수동 제외 = EXCLUDE (번호는 ADD -> EXCLUDE 순서로 1부터)
        /// </summary>
        private ChainSaveData BuildChainSaveData(ChainDefinition chain)
        {
            var data = new ChainSaveData { Chain = chain };
            if (chain.Inputs.Count != 1) data.Problems.Add("Input " + chain.Inputs.Count + "개(1개여야 함)");
            if (chain.Outputs.Count > 1) data.Problems.Add("Output " + chain.Outputs.Count + "개(0~1개여야 함)");
            if (chain.Inputs.Keys.Any(key => chain.Outputs.ContainsKey(key))) data.Problems.Add("같은 도형이 Input과 Output");

            var automatic = new HashSet<string>(StringComparer.Ordinal);
            if (chain.LastResult != null && chain.TraceShown)
            {
                IEnumerable<ChainTraceElement> elements = chain.LastResult.IsConnected
                    ? chain.LastResult.CandidateElements
                    : chain.LastResult.VisitedElements.Concat(chain.LastResult.EndpointElements);
                automatic.UnionWith(elements.Select(element => element.ElementKey));
            }
            foreach (string key in GetChainKeys(chain).OrderBy(key => key, StringComparer.Ordinal))
            {
                ChainEndpointPick pick;
                if (chain.Inputs.TryGetValue(key, out pick))
                    data.Members.Add(new[] { key, "INPUT", "ENDPOINT", Coordinate(ToStoredX(pick.Point.X)), Coordinate(pick.Point.Y) });
                else if (chain.Outputs.TryGetValue(key, out pick))
                    data.Members.Add(new[] { key, "OUTPUT", "ENDPOINT", Coordinate(ToStoredX(pick.Point.X)), Coordinate(pick.Point.Y) });
                else
                    data.Members.Add(new[] { key, "MEMBER", automatic.Contains(key) ? "TRACE" : "MANUAL", "", "" });
            }
            int editSeq = 0;
            foreach (string key in chain.ManualAdded.OrderBy(key => key, StringComparer.Ordinal))
                data.Edits.Add(new[] { (++editSeq).ToString(CultureInfo.InvariantCulture), key, "ADD", "" });
            foreach (string key in chain.ManualExcluded.OrderBy(key => key, StringComparer.Ordinal))
                data.Edits.Add(new[] { (++editSeq).ToString(CultureInfo.InvariantCulture), key, "EXCLUDE", "" });
            if (data.Members.Count == 0) data.Problems.Add("포함 도형 없음");
            // 필요한 Layer를 조회하지 않고 불러온 Chain은 일부 도형이 빠져 있으므로 덮어쓰지 않는다.
            if (chain.DbIncomplete != null) data.Problems.Add(chain.DbIncomplete);

            List<int> layers = chain.LayerIds.OrderBy(id => id).ToList();
            data.Layers = new string[layers.Count, 1];
            for (int i = 0; i < layers.Count; i++) data.Layers[i, 0] = layers[i].ToString(CultureInfo.InvariantCulture);
            if (layers.Count == 0) data.Problems.Add("탐색 Layer 없음");
            data.Rules = new string[chain.LayerRules.Count, 3];
            for (int i = 0; i < chain.LayerRules.Count; i++)
            {
                data.Rules[i, 0] = chain.LayerRules[i].FirstLayerId.ToString(CultureInfo.InvariantCulture);
                data.Rules[i, 1] = chain.LayerRules[i].SecondLayerId.ToString(CultureInfo.InvariantCulture);
                data.Rules[i, 2] = Coordinate(chain.LayerRules[i].Tolerance);
            }
            data.Fingerprint = ChainFingerprint(chain, data);
            return data;
        }

        /// <summary>저장 / 제외 / 변경 없음 / 삭제 대상을 보여 주고 계속할지 묻는다.</summary>
        private bool AskChainDbSave(List<ChainSaveData> toSave, List<ChainSaveData> invalid, int unchanged)
        {
            if (toSave.Count == 0 && _deletedDbChains.Count == 0)
            {
                MessageBox.Show(this, (invalid.Count > 0
                        ? "저장할 수 있는 Chain이 없습니다.\r\n\r\n" + FormatInvalidChains(invalid)
                        : "변경된 Chain이 없습니다. (변경 없음 " + unchanged + "개)"),
                    "Chain 저장", MessageBoxButtons.OK, invalid.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                return false;
            }
            var text = new StringBuilder();
            text.Append("저장: ").Append(toSave.Count).Append("개");
            if (toSave.Count > 0)
                text.Append(" (").Append(String.Join(", ", toSave.Select(item => item.Chain.Name + " 도형 " + item.Members.Count.ToString("N0")))).Append(")");
            text.Append("\r\n변경 없음: ").Append(unchanged).Append("개");
            if (_deletedDbChains.Count > 0)
                text.Append("\r\nDB에서 삭제: ").Append(String.Join(", ", _deletedDbChains.Select(chain => chain.Name)));
            if (invalid.Count > 0)
                text.Append("\r\n\r\n아래 Chain은 저장하지 않습니다.\r\n").Append(FormatInvalidChains(invalid));
            text.Append("\r\n\r\nChain을 저장하시겠습니까?");
            return MessageBox.Show(this, text.ToString(), "Chain 저장 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes;
        }

        private static string FormatInvalidChains(IEnumerable<ChainSaveData> invalid)
        {
            // Code(A, B, C)는 화면에 보이지 않는다. 이름으로 안내한다 (2026-10-12).
            return String.Join("\r\n", invalid.Select(item => item.Chain.Name + " : " + String.Join(", ", item.Problems)));
        }

        /// <summary>
        /// Chain 1개를 저장한다: BeginChainSave(헤더 / Layer / 규칙, 기존 내용 삭제) -> Member / 편집 배치 -> CompleteChainSave.
        /// 성공하면 Chain 번호와 저장 지문을 화면 Chain에 기록하고 null을, 실패하면 이유를 돌려준다.
        /// </summary>
        private async Task<string> SaveOneChain(DACrux.SEMDMS.RO.GdsChain service, ChainSaveData item,
            string factory, string deviceId, string user)
        {
            ChainDefinition chain = item.Chain;
            string[] info = {
                factory, deviceId, chain.ChainSeq ?? "", chain.Code, chain.Name,
                chain.Color.ToArgb().ToString(CultureInfo.InvariantCulture), chain.ApplyLayerRules ? "1" : "0",
                chain.CellSize.ToString(CultureInfo.InvariantCulture), user };
            try
            {
                DataTable begin = await Task.Run(() => service.BeginChainSave(info, item.Layers, item.Rules));
                string seq = begin.Rows[0]["CHAIN_SEQ"].ToString();
                string[] key = { factory, deviceId, seq };
                foreach (string[,] rows in ChainBatches(item.Members, 5))
                    await Task.Run(() => service.CreateChainMemberBatch(key, rows, user));
                foreach (string[,] rows in ChainBatches(item.Edits, 4))
                    await Task.Run(() => service.CreateChainEditBatch(key, rows, user));
                string[] completeKey = { factory, deviceId, seq,
                    item.Members.Count.ToString(CultureInfo.InvariantCulture), item.Edits.Count.ToString(CultureInfo.InvariantCulture) };
                DataRow result = (await Task.Run(() => service.CompleteChainSave(completeKey, user))).Rows[0];
                chain.ChainSeq = seq;
                if (result["RESULT"].ToString() != "OK")
                    return result["MESSAGE"].ToString();
                chain.SavedFingerprint = item.Fingerprint;
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>행 목록을 서비스 배치(string[,]) 단위로 나눈다.</summary>
        private static IEnumerable<string[,]> ChainBatches(List<string[]> rows, int columns)
        {
            for (int start = 0; start < rows.Count; start += ChainDbBatchRows)
            {
                int count = Math.Min(ChainDbBatchRows, rows.Count - start);
                var batch = new string[count, columns];
                for (int i = 0; i < count; i++)
                    for (int c = 0; c < columns; c++)
                        batch[i, c] = rows[start + i][c];
                yield return batch;
            }
        }

        /// <summary>
        /// 저장 내용의 지문(SHA-256). 마지막 저장 때 지문과 같으면 "변경 없음"으로 보고 다시 보내지 않는다.
        /// 이름 / 색상 / 탐색 옵션 / Layer / 규칙 / Member / 편집 기록을 모두 포함한다.
        /// </summary>
        private static string ChainFingerprint(ChainDefinition chain, ChainSaveData data)
        {
            var text = new StringBuilder();
            text.Append(chain.Code).Append('\u001F').Append(chain.Name).Append('\u001F').Append(chain.Color.ToArgb())
                .Append('\u001F').Append(chain.ApplyLayerRules).Append('\u001F').Append(chain.CellSize.ToString(CultureInfo.InvariantCulture));
            AppendRows(text, data.Layers);
            AppendRows(text, data.Rules);
            foreach (string[] row in data.Members) text.Append('\u001E').Append(String.Join("\u001F", row));
            foreach (string[] row in data.Edits) text.Append('\u001E').Append(String.Join("\u001F", row));
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text.ToString()))).Replace("-", "");
        }

        private static void AppendRows(StringBuilder text, string[,] rows)
        {
            text.Append('\u001D');
            for (int i = 0; i < rows.GetLength(0); i++)
            {
                text.Append('\u001E');
                for (int c = 0; c < rows.GetLength(1); c++) text.Append(rows[i, c]).Append('\u001F');
            }
        }

        /// <summary>좌표 / 허용 오차 double을 비트 손실 없는 G17 문자열로 바꾼다(서버 TO_BINARY_DOUBLE).</summary>
        private static string Coordinate(double value)
        {
            return value.ToString("G17", CultureInfo.InvariantCulture);
        }

        /// <summary>저장 중에는 Chain 편집 / 목록 / 도면 열기를 막는다. 저장 도중 Chain 내용이 바뀌지 않게 하기 위함이다.</summary>
        private void SetChainDbSaving(bool saving)
        {
            _chainDbSaving = saving;
            btnSearch.Enabled = cboDevice.Enabled = cboViewSide.Enabled = !saving;
            if (saving)
            {
                SetChainButtonsEnabled(false);
                SetChainListButtonsEnabled(false);
            }
            else
            {
                SetChainButtonsEnabled(map.Structure != null);
                SetChainListButtonsEnabled(map.Structure != null);
                UpdateChainSelectionButtons();
            }
        }

        private void SetChainDbStatus(string text)
        {
            SetChainStatus(text);
        }
    }
}
