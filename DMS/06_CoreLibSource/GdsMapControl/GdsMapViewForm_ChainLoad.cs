using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View 화면의 DB Chain 불러오기다. (설계: 문서/설계/02_GDS/2026-10-06_GDS_Map_화면역할_재정의_설계.md 4.3절)
    ///
    /// 처리 흐름 ([조회] 안에서 호출)
    ///  1) 도면을 그리기 전: DB Chain 목록 + Chain별 Layer / 규칙 / Member(+도형 Layer) / 편집 기록 읽기(ReadDbChains, 백그라운드)
    ///     - 상세는 Chain 번호 범위마다 한 번에 읽어 CHAIN_SEQ 로 나눈다(Chain 이 수만 개인 U8 대응, 2026-10-21)
    ///  2) Chain이 쓰는 Layer(탐색 Layer + Member 도형 Layer)가 조회 체크에 없으면 함께 조회할지 묻기(ConfirmChainLayers, 설계 결정 3)
    ///  3) 도면을 그린 뒤: DB 행을 화면 Chain(ChainDefinition)으로 복원(LoadChainsFromDb)
    ///     - Input / Output = 단자 후보 + 저장된 클릭 좌표, 수동 추가 / 제외 = 편집 기록
    ///     - 자동 탐색 결과 = TRACE Member로 만든 "열린 탐색 결과". 수동 편집 / 유사 묶음 코드가 탐색 결과를 그대로 쓰므로 고치지 않아도 된다.
    ///     - 지금 지도에 없는 도형(미조회 Layer)이 있으면 DbIncomplete로 표시하고 저장에서 제외한다(도형이 빠진 채 덮어쓰지 않도록).
    ///     - 저장 지문을 계산해 두어 바꾸지 않으면 "변경 없음"이 된다. DRAFT(저장 미완료)는 지문을 두지 않아 다시 저장 대상이 된다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>
        /// DB에서 읽은 Chain 1개의 원본 행.
        /// 상세 행은 Chain 번호 범위로 한 번에 읽은 표에서 이 Chain 의 행만 모은 것이다(행은 원래 표를 가리킨다) (2026-10-21).
        /// </summary>
        private sealed class ChainDbData
        {
            public DataRow Header;
            public List<DataRow> Layers = new List<DataRow>();
            public List<DataRow> Rules = new List<DataRow>();
            public List<DataRow> Members = new List<DataRow>();
            public List<DataRow> Edits = new List<DataRow>();

            /// <summary>이 Chain을 온전히 그리는 데 필요한 Layer 키(번호 + DataType): 탐색 Layer + Member 도형 Layer.</summary>
            public IEnumerable<int> NeededLayers
            {
                get
                {
                    return Layers.Select(row => GdsLayerKey.FromRow(row))
                        .Concat(Members.Select(row => GdsLayerKey.FromRow(row)));
                }
            }
        }

        /// <summary>
        /// Chain 상세를 한 번에 읽는 Chain 수(Chain 번호 범위 1개).
        /// U8 같은 형태 Chain(도형 10개)이면 포함 도형 약 5만 행, Binary 전송 약 4MB 다 (2026-10-21).
        /// </summary>
        private const int ChainDetailRangeSize = 5000;

        /// <summary>
        /// 불러온 Chain 이 이보다 많으면 지도 표시(체크)를 끈 채로 불러온다. 목록에는 모두 넣고, 체크 박스로 체크한 Chain 만 외곽선을 그린다.
        /// U8 같은 형태 Chain 27,342개를 모두 그리면 외곽선 정점이 약 3,200만 개(불러오기 약 15초 + 메모리 1.6GB 이상)였다 (2026-10-21 결정).
        /// 표시 여부는 저장 지문에 들어가지 않아 꺼도 "변경 없음" 그대로다.
        /// </summary>
        private const int ChainLoadVisibleLimit = 1000;

        /// <summary>
        /// DB Chain 목록과 Chain별 상세를 읽는다. 서비스 호출만 하므로 백그라운드 스레드에서 실행한다.
        /// progress: 화면 상태줄에 "n / 전체"를 보여 주기 위한 진행 보고(Chain이 많을 때 멈춘 것처럼 보이지 않게).
        /// 처리 흐름:
        ///  1) Chain 목록 1번 -> Chain 번호(CHAIN_SEQ) 순으로 정렬해 5,000개씩 범위를 나눈다
        ///  2) 범위마다 Layer / 규칙 / 포함 도형 / 편집 기록을 한 번씩 읽어 CHAIN_SEQ 로 각 Chain 에 나눠 담는다
        ///  3) 서버에 범위 조회가 없으면(이전 서버) Chain 마다 4번 읽는 예전 방식으로 바꾼다
        /// Chain 마다 4번 부르면 U8(27,342개)은 상세 읽기만 약 5분이 걸렸다 (2026-10-21, 설계: 문서/설계/02_GDS/2026-10-21_U8_Chain_View_조회속도_분석.md S1).
        /// </summary>
        private static List<ChainDbData> ReadDbChains(string factory, string deviceId, IProgress<string> progress)
        {
            var service = new DACrux.SEMDMS.RO.GdsChain();
            var result = new List<ChainDbData>();
            var bySeq = new Dictionary<long, ChainDbData>();
            DataTable list = service.GetChainList(factory, deviceId);
            foreach (DataRow row in list.Rows)
            {
                var data = new ChainDbData { Header = row };
                result.Add(data);
                bySeq[Convert.ToInt64(row["CHAIN_SEQ"], CultureInfo.InvariantCulture)] = data;
            }

            List<long> seqs = bySeq.Keys.OrderBy(seq => seq).ToList();
            bool rangeAvailable = false;
            try
            {
                for (int start = 0; start < seqs.Count; start += ChainDetailRangeSize)
                {
                    int end = Math.Min(start + ChainDetailRangeSize, seqs.Count) - 1;
                    string from = seqs[start].ToString(CultureInfo.InvariantCulture);
                    string to = seqs[end].ToString(CultureInfo.InvariantCulture);
                    progress.Report("저장된 Chain 조회 중 " + (end + 1) + " / " + seqs.Count);
                    SplitByChain(service.GetChainLayersRange(factory, deviceId, from, to), bySeq, data => data.Layers);
                    rangeAvailable = true;
                    SplitByChain(service.GetChainRulesRange(factory, deviceId, from, to), bySeq, data => data.Rules);
                    SplitByChain(service.GetChainMembersRange(factory, deviceId, from, to), bySeq, data => data.Members);
                    SplitByChain(service.GetChainEditsRange(factory, deviceId, from, to), bySeq, data => data.Edits);
                }
            }
            catch (Exception ex) when (!rangeAvailable && (ex is System.Runtime.Remoting.RemotingException || ex is MissingMethodException))
            {
                ReadDbChainDetailsOneByOne(service, factory, deviceId, result, progress);
            }
            return result;
        }

        /// <summary>범위 조회 표의 행을 CHAIN_SEQ 로 해당 Chain 에 나눠 담는다. 목록에 없는 Chain(목록 조회 뒤 새로 저장된 것)의 행은 건너뛴다.</summary>
        private static void SplitByChain(DataTable table, Dictionary<long, ChainDbData> bySeq, Func<ChainDbData, List<DataRow>> target)
        {
            foreach (DataRow row in table.Rows)
            {
                if (bySeq.TryGetValue(Convert.ToInt64(row["CHAIN_SEQ"], CultureInfo.InvariantCulture), out ChainDbData data))
                    target(data).Add(row);
            }
        }

        /// <summary>범위 조회가 없는 이전 서버: Chain 마다 Layer / 규칙 / 포함 도형 / 편집 기록을 따로 읽는다(예전 방식).</summary>
        private static void ReadDbChainDetailsOneByOne(DACrux.SEMDMS.RO.GdsChain service, string factory, string deviceId,
            List<ChainDbData> chains, IProgress<string> progress)
        {
            int index = 0;
            foreach (ChainDbData data in chains)
            {
                string seq = data.Header["CHAIN_SEQ"].ToString();
                progress.Report("저장된 Chain 조회 중 " + (++index) + " / " + chains.Count + " (" + data.Header["CHAIN_NAME"] + ")");
                data.Layers.AddRange(service.GetChainLayers(factory, deviceId, seq).Rows.Cast<DataRow>());
                data.Rules.AddRange(service.GetChainRules(factory, deviceId, seq).Rows.Cast<DataRow>());
                data.Members.AddRange(service.GetChainMembers(factory, deviceId, seq).Rows.Cast<DataRow>());
                data.Edits.AddRange(service.GetChainEdits(factory, deviceId, seq).Rows.Cast<DataRow>());
            }
        }

        /// <summary>
        /// Chain이 쓰는 Layer 중 조회 체크에 없는 Layer가 있으면 함께 조회할지 묻는다(설계 결정 3).
        /// [예]면 그 Layer를 목록에서 체크해 이번 조회에 포함하고 true. [아니오]면 그대로 진행한다(false).
        /// </summary>
        private bool ConfirmChainLayers(IEnumerable<int> neededLayers, string chainText)
        {
            var checkedIds = new HashSet<int>(chkLayerItems.CheckedItems.Cast<LayerDisplayItem>().Select(item => item.LayerKey));
            List<int> missing = neededLayers.Distinct().Where(id => !checkedIds.Contains(id) && _layerCounts.ContainsKey(id))
                .OrderBy(id => id).ToList();
            if (missing.Count == 0) return false;
            if (MessageBox.Show(this, chainText + "이(가) 사용하는 Layer " + String.Join(", ", missing.Select(GdsLayerKey.Format)) + "가 조회 대상에 없습니다.\r\n\r\n"
                    + "함께 조회하시겠습니까?\r\n[아니오]를 누르면 그 Layer의 Chain 도형이 지도에 나오지 않고, "
                    + "DB에서 불러오는 Chain은 도형 일부를 복원하지 못해 저장에서 제외됩니다.",
                    "Chain Layer 조회", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return false;
            _updatingLayerChecks = true;
            try
            {
                for (int i = 0; i < chkLayerItems.Items.Count; i++)
                    if (missing.Contains(((LayerDisplayItem)chkLayerItems.Items[i]).LayerKey))
                        chkLayerItems.SetItemChecked(i, true);
            }
            finally
            {
                _updatingLayerChecks = false;
            }
            return true;
        }

        /// <summary>같은 Map을 다시 조회할 때 화면 Chain이 쓰는 Layer(탐색 Layer + 불러온 Member 도형 Layer).</summary>
        private IEnumerable<int> MemoryChainLayers()
        {
            CaptureActiveChain();
            return _chains.SelectMany(chain => chain.LayerIds.Concat(chain.MemberLayerIds));
        }

        /// <summary>도면을 그린 뒤 DB 행을 화면 Chain으로 복원한다. 처리 내용은 클래스 설명 참고.</summary>
        private void LoadChainsFromDb(List<ChainDbData> data)
        {
            map.RebuildChainTraceItems();
            int maxOrdinal = 0;
            bool hideAll = data.Count > ChainLoadVisibleLimit;
            _switchingChain = true;
            // Chain 이 수만 개면 하나씩 넣을 때마다 목록을 다시 계산해 수십 초가 걸렸다. 넣는 동안 목록 그리기를 멈춘다 (2026-10-21)
            _chainList.BeginUpdate();
            try
            {
                foreach (ChainDbData item in data)
                {
                    ChainDefinition chain = CreateChainFromDb(item);
                    if (hideAll) chain.Visible = false;
                    _chains.Add(chain);
                    _chainList.Items.Add(chain, chain.Visible);
                    maxOrdinal = Math.Max(maxOrdinal, ParseChainCode(chain.Code));
                }
            }
            finally
            {
                _chainList.EndUpdate();
                _switchingChain = false;
            }
            // 새 Chain Code는 불러온 Chain의 최대 Code 다음부터 매긴다(같은 Code 덮어쓰기 방지).
            _nextChainOrdinal = Math.Max(_nextChainOrdinal, maxOrdinal);
            RefreshChainDisplay();
            SetChainListButtonsEnabled(true);
            // 불러온 Chain 수 / 일부 복원 못 한 Chain 수는 조회 결과 한 줄에 함께 보인다(btnSearch_Click, 2026-10-13).
        }

        /// <summary>DB Chain 1개를 화면 Chain으로 만든다. RebuildChainTraceItems 뒤에 호출한다.</summary>
        private ChainDefinition CreateChainFromDb(ChainDbData item)
        {
            DataRow h = item.Header;
            bool ready = h["VERSION_STATUS"].ToString() == "READY";
            decimal cellSize;
            if (!decimal.TryParse(h["CELL_SIZE"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out cellSize) || cellSize <= 0)
                cellSize = DefaultChainCellSize;
            var chain = new ChainDefinition
            {
                Id = Guid.NewGuid(),
                ChainSeq = h["CHAIN_SEQ"].ToString(),
                Code = h["CHAIN_CODE"].ToString(),
                Name = h["CHAIN_NAME"] is DBNull ? h["CHAIN_CODE"].ToString() : h["CHAIN_NAME"].ToString(),
                Color = h["COLOR_ARGB"] is DBNull ? Color.Gold : Color.FromArgb(ToInt(h["COLOR_ARGB"])),
                ApplyLayerRules = !(h["APPLY_LAYER_RULES"] is DBNull) && ToInt(h["APPLY_LAYER_RULES"]) == 1,
                CellSize = cellSize,
                // 탐색 Layer / 규칙은 Layer 키(번호 + DataType). DataType 열이 없는 이전 서버 응답은 0 (2026-10-20)
                LayerIds = new HashSet<int>(item.Layers.Select(row => GdsLayerKey.FromRow(row))),
                LayerRules = item.Rules.Select(row => new ChainLayerConnectionRule(
                    GdsLayerKey.FromRow(row, "FIRST_LAYER_ID", "FIRST_DATA_TYPE"), GdsLayerKey.FromRow(row, "SECOND_LAYER_ID", "SECOND_DATA_TYPE"),
                    Convert.ToDouble(row["TOLERANCE"], CultureInfo.InvariantCulture))).ToList()
            };

            var traceIds = new List<string>();
            var endpointIds = new List<string>();
            var missingLayers = new SortedSet<int>();
            int missing = 0;
            foreach (DataRow row in item.Members)
            {
                string id = row["PLACED_ELEMENT_ID"].ToString();
                string role = row["ROLE_CODE"].ToString();
                int layer = GdsLayerKey.FromRow(row);
                chain.MemberLayerIds.Add(layer);
                if (role == "INPUT" || role == "OUTPUT")
                {
                    ChainElementCandidate candidate = map.GetChainElementCandidate(id);
                    if (candidate == null) { missing++; missingLayers.Add(layer); continue; }
                    var pick = new ChainEndpointPick
                    {
                        Candidate = candidate,
                        // DB Marker는 저장 기준 좌표. 화면 보기 기준과 다르면 X 반전. GPoint 생성자 반올림을 피하려고 필드에 직접 넣는다.
                        Point = MarkerPoint(Convert.ToDouble(row["MARKER_X"], CultureInfo.InvariantCulture),
                            Convert.ToDouble(row["MARKER_Y"], CultureInfo.InvariantCulture))
                    };
                    (role == "INPUT" ? chain.Inputs : chain.Outputs)[id] = pick;
                    endpointIds.Add(id);
                }
                else if (row["INCLUSION_SOURCE"].ToString() == "TRACE")
                    traceIds.Add(id);
                else
                    chain.ManualAdded.Add(id);
            }
            foreach (DataRow row in item.Edits)
            {
                string id = row["PLACED_ELEMENT_ID"].ToString();
                string action = row["ACTION_CODE"].ToString();
                if (action == "ADD") chain.ManualAdded.Add(id);
                else if (action == "EXCLUDE") chain.ManualExcluded.Add(id);
            }

            IList<ChainTraceElement> traceElements = map.GetChainTraceElements(traceIds);
            IList<ChainTraceElement> endpointElements = map.GetChainTraceElements(endpointIds);
            if (traceElements.Count < traceIds.Count)
            {
                var found = new HashSet<string>(traceElements.Select(element => element.ElementKey), StringComparer.Ordinal);
                foreach (DataRow row in item.Members)
                    if (row["INCLUSION_SOURCE"].ToString() == "TRACE" && !found.Contains(row["PLACED_ELEMENT_ID"].ToString()))
                    {
                        missing++;
                        missingLayers.Add(GdsLayerKey.FromRow(row));
                    }
            }
            if (traceElements.Count > 0 || endpointElements.Count > 0)
            {
                // 저장된 자동 탐색 결과를 "열린 탐색 결과"로 복원한다(자동 후보 = 방문 도형 + 단자). 다시 [후보 경로 찾기]를 하면 새 결과로 바뀐다.
                chain.LastResult = new ChainTraceResult(false, new List<ChainTraceElement>(), traceElements,
                    "DB에서 불러온 경로", endpointElements: endpointElements, isOpenEnded: true);
                chain.TraceShown = true;
            }
            if (missing > 0)
                chain.DbIncomplete = "Layer " + String.Join(", ", missingLayers.Select(GdsLayerKey.Format)) + " 미조회로 도형 " + missing
                    + "개를 복원하지 못함";
            // 바꾸지 않으면 "변경 없음"이 되도록 지금 내용의 저장 지문을 기록한다. DRAFT는 다시 저장 대상이 되도록 비워 둔다.
            chain.SavedFingerprint = ready ? BuildChainSaveData(chain).Fingerprint : null;
            return chain;
        }

        /// <summary>저장 기준 Marker 좌표를 화면 좌표로(X는 보기 기준에 맞춰 반전).</summary>
        private GPoint MarkerPoint(double storedX, double y)
        {
            var point = new GPoint();
            point.X = ToDisplayX(storedX);
            point.Y = y;
            return point;
        }

        /// <summary>A~Z, AA... Code를 순번으로 바꾼다(GetChainCode의 역). 형식이 다르면 0.</summary>
        private static int ParseChainCode(string code)
        {
            int ordinal = 0;
            foreach (char ch in code ?? string.Empty)
            {
                if (ch < 'A' || ch > 'Z') return 0;
                ordinal = ordinal * 26 + (ch - 'A' + 1);
            }
            return ordinal;
        }

        private static int ToInt(object value)
        {
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }
    }
}
