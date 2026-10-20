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
    ///  2) Chain이 쓰는 Layer(탐색 Layer + Member 도형 Layer)가 조회 체크에 없으면 함께 조회할지 묻기(ConfirmChainLayers, 설계 결정 3)
    ///  3) 도면을 그린 뒤: DB 행을 화면 Chain(ChainDefinition)으로 복원(LoadChainsFromDb)
    ///     - Input / Output = 단자 후보 + 저장된 클릭 좌표, 수동 추가 / 제외 = 편집 기록
    ///     - 자동 탐색 결과 = TRACE Member로 만든 "열린 탐색 결과". 수동 편집 / 유사 묶음 코드가 탐색 결과를 그대로 쓰므로 고치지 않아도 된다.
    ///     - 지금 지도에 없는 도형(미조회 Layer)이 있으면 DbIncomplete로 표시하고 저장에서 제외한다(도형이 빠진 채 덮어쓰지 않도록).
    ///     - 저장 지문을 계산해 두어 바꾸지 않으면 "변경 없음"이 된다. DRAFT(저장 미완료)는 지문을 두지 않아 다시 저장 대상이 된다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>DB에서 읽은 Chain 1개의 원본 행.</summary>
        private sealed class ChainDbData
        {
            public DataRow Header;
            public DataTable Layers;
            public DataTable Rules;
            public DataTable Members;
            public DataTable Edits;

            /// <summary>이 Chain을 온전히 그리는 데 필요한 Layer 키(번호 + DataType): 탐색 Layer + Member 도형 Layer.</summary>
            public IEnumerable<int> NeededLayers
            {
                get
                {
                    return Layers.Rows.Cast<DataRow>().Select(row => GdsLayerKey.FromRow(row))
                        .Concat(Members.Rows.Cast<DataRow>().Select(row => GdsLayerKey.FromRow(row)));
                }
            }
        }

        /// <summary>
        /// DB Chain 목록과 Chain별 상세를 읽는다. 서비스 호출만 하므로 백그라운드 스레드에서 실행한다.
        /// progress: 화면 상태줄에 "n / 전체 Code"를 보여 주기 위한 진행 보고(Chain이 많을 때 멈춘 것처럼 보이지 않게).
        /// </summary>
        private static List<ChainDbData> ReadDbChains(string factory, string deviceId, IProgress<string> progress)
        {
            var service = new DACrux.SEMDMS.RO.GdsChain();
            var result = new List<ChainDbData>();
            DataTable list = service.GetChainList(factory, deviceId);
            int index = 0;
            foreach (DataRow row in list.Rows)
            {
                string seq = row["CHAIN_SEQ"].ToString();
                progress.Report("저장된 Chain 조회 중 " + (++index) + " / " + list.Rows.Count + " (" + row["CHAIN_NAME"] + ")");
                result.Add(new ChainDbData
                {
                    Header = row,
                    Layers = service.GetChainLayers(factory, deviceId, seq),
                    Rules = service.GetChainRules(factory, deviceId, seq),
                    Members = service.GetChainMembers(factory, deviceId, seq),
                    Edits = service.GetChainEdits(factory, deviceId, seq)
                });
            }
            return result;
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
            _switchingChain = true;
            try
            {
                foreach (ChainDbData item in data)
                {
                    ChainDefinition chain = CreateChainFromDb(item);
                    _chains.Add(chain);
                    _chainList.Items.Add(chain, chain.Visible);
                    maxOrdinal = Math.Max(maxOrdinal, ParseChainCode(chain.Code));
                }
            }
            finally
            {
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
                LayerIds = new HashSet<int>(item.Layers.Rows.Cast<DataRow>().Select(row => GdsLayerKey.FromRow(row))),
                LayerRules = item.Rules.Rows.Cast<DataRow>().Select(row => new ChainLayerConnectionRule(
                    GdsLayerKey.FromRow(row, "FIRST_LAYER_ID", "FIRST_DATA_TYPE"), GdsLayerKey.FromRow(row, "SECOND_LAYER_ID", "SECOND_DATA_TYPE"),
                    Convert.ToDouble(row["TOLERANCE"], CultureInfo.InvariantCulture))).ToList()
            };

            var traceIds = new List<string>();
            var endpointIds = new List<string>();
            var missingLayers = new SortedSet<int>();
            int missing = 0;
            foreach (DataRow row in item.Members.Rows)
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
            foreach (DataRow row in item.Edits.Rows)
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
                foreach (DataRow row in item.Members.Rows)
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
