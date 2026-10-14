using NexplantQMS.GdsMap.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map View Label 검색 (2026-10-07, 설계: 문서/2026-10-07_GDS_Label_검색_설계.md).
    /// Label 색인을 [조회] 때 한 번 받아 메모리 검색 / 결과 목록 + 첫 건 이동 + [전체 보기] / 결과 중심 300µm.
    /// 일치 방식은 "포함" 하나(대소문자 무시)로 고정하고 일치 방식 콤보를 없앴다. 검색 시간 표시도 뺐다 (2026-10-12).
    /// 검색 결과 수 / 안내는 상태 줄 한 곳에만 보인다. 검색 칸 옆 결과 글자는 없앴다 (2026-10-13).
    /// Label = 도형 종류 TEXT인 배치 도형 전체(Layer 무관). 그래서 Label Layer(지금은 775)가 바뀌거나 조회하지 않아도 찾는다.
    ///
    /// 처리 흐름
    ///  [조회](다른 Map) -> GetMapTextIndex -> 도면 그리기 성공 시 색인 교체 -> 검색 입력 활성화
    ///  [검색] / Enter -> 색인 검색 -> 결과 목록(최대 1,000) -> 첫 건 위치로 이동
    ///  결과 선택 -> 그 위치로 이동 / [전체 보기] -> 결과 전체 범위
    ///  보기 기준 전환 -> 목록 좌표를 새 기준 X로 다시 표시 / Device 변경 -> 색인 / 결과 비움
    /// 지도에 검색 표시(과녁 / Label 글자)는 그리지 않고 위치 이동만 한다(소스 단순화, 2026-10-07).
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>목록에 보여 줄 최대 결과 수. 그보다 많으면 "외 n건"만 알린다.</summary>
        private const int LabelResultLimit = 1000;
        /// <summary>결과로 이동할 때 보이는 범위(결과 중심 사각형 한 변, µm). 설계 결정 4.</summary>
        private const double LabelFocusSize = 300.0;

        /// <summary>지금 도면 Map의 Label 색인. null이면 아직 받지 않았거나 받지 못함.</summary>
        private GdsLabelIndex _labelIndex;
        private string _labelIndexError;
        /// <summary>마지막 검색의 전체 결과(목록에는 앞 LabelResultLimit건).</summary>
        private List<GdsLabelEntry> _labelResults = new List<GdsLabelEntry>();
        private bool _updatingLabelResults;

        /// <summary>Label 일치 방식: 포함(대소문자 무시, 앞뒤 공백 제거). GdsLabelIndex.Search 참고.</summary>
        private const GdsLabelMatchMode LabelMatchMode = GdsLabelMatchMode.Contains;

        /// <summary>Designer 생성 뒤 초기 상태(도면 전에는 검색 비활성).</summary>
        private void InitializeLabelSearchState()
        {
            SetLabelSearchEnabled(false);
        }

        /// <summary>서비스에서 Label 색인을 받는다. 백그라운드 스레드에서 실행한다.</summary>
        private static GdsLabelIndex ReadLabelIndex(string factory, string deviceId)
        {
            var service = new DACrux.SEMDMS.RO.GdsMapView();
            using (DataTable table = service.GetMapTextIndex(factory, deviceId))
                return GdsLabelIndex.FromTable(table);
        }

        /// <summary>
        /// 새 Map을 그린 뒤 색인을 바꾼다. 색인을 받지 못해도 도면은 그대로 쓰고 검색만 막는다.
        /// 색인 건수 / 실패 사유는 미리 보이지 않고, 검색할 때 상태 줄로 알린다(RunLabelSearch).
        /// </summary>
        private void ReplaceLabelIndex(GdsLabelIndex index, string error)
        {
            ClearLabelResults();
            _labelIndex = index;
            _labelIndexError = error;
            SetLabelSearchEnabled(true);
        }

        /// <summary>Device가 바뀌면 이전 Map의 색인 / 결과를 버린다.</summary>
        private void ClearLabelIndex()
        {
            ClearLabelResults();
            _labelIndex = null;
            _labelIndexError = null;
            SetLabelSearchEnabled(false);
        }

        private void ClearLabelResults()
        {
            _labelResults = new List<GdsLabelEntry>();
            _updatingLabelResults = true;
            cboLabelResults.Items.Clear();
            _updatingLabelResults = false;
            btnLabelAll.Enabled = false;
        }

        /// <summary>검색 입력은 도면(색인)이 있을 때만 쓸 수 있다. 조회 중에는 SetLoading이 함께 막는다.</summary>
        private void SetLabelSearchEnabled(bool enabled)
        {
            bool ready = enabled && _labelIndex != null;
            txtLabel.Enabled = btnLabelSearch.Enabled = ready;
            cboLabelResults.Enabled = ready && _labelResults.Count > 0;
            btnLabelAll.Enabled = ready && _labelResults.Count > 1;
        }

        private void btnLabelSearch_Click(object sender, EventArgs e)
        {
            RunLabelSearch();
        }

        private void txtLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = e.SuppressKeyPress = true;
            if (btnLabelSearch.Enabled) RunLabelSearch();
        }

        /// <summary>
        /// 입력한 Label을 색인에서 찾아 목록에 보여 주고 첫 건으로 이동한다.
        /// 0건이면 이유(Map에 TEXT 없음 / 일치 방식)를 상태줄에 안내한다.
        /// </summary>
        private void RunLabelSearch()
        {
            if (_labelIndex == null)
            {
                SetStatus(_labelIndexError == null ? "[조회]로 도면을 먼저 불러오세요." : "Label 색인이 없어 검색할 수 없습니다: " + _labelIndexError);
                return;
            }
            string text = txtLabel.Text.Trim();
            if (text.Length == 0)
            {
                MessageBox.Show(this, "찾을 Label을 입력하세요. 예: C4DC_212_71", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLabel.Focus();
                return;
            }
            List<GdsLabelEntry> results = _labelIndex.Search(text, LabelMatchMode);

            ClearLabelResults();
            _labelResults = results;
            if (results.Count == 0)
            {
                SetLabelSearchEnabled(true);
                SetStatus(_labelIndex.Count == 0
                    ? "이 Map에는 TEXT(Label)가 없습니다. Setup에서 Label Layer를 포함해 저장했는지 확인하세요."
                    : "'" + text + "'이(가) 들어 있는 Label이 없습니다.");
                return;
            }

            _updatingLabelResults = true;
            cboLabelResults.BeginUpdate();
            for (int i = 0; i < results.Count && i < LabelResultLimit; i++)
                cboLabelResults.Items.Add(DescribeLabelResult(i));
            cboLabelResults.EndUpdate();
            _updatingLabelResults = false;
            SetLabelSearchEnabled(true);

            SelectLabelResult(0);
            SetStatus("'" + text + "' " + results.Count.ToString("N0") + "건"
                + (results.Count > LabelResultLimit ? "(목록에는 앞 " + LabelResultLimit.ToString("N0") + "건)" : "")
                + ". 첫 번째 위치로 이동했습니다." + UnloadedLabelLayerNote(results));
        }

        /// <summary>결과 Layer 중 도면에 올리지 않은 Layer가 있으면 안내 문구(위치로는 이동하지만 지도에 글자가 없다).</summary>
        private string UnloadedLabelLayerNote(IEnumerable<GdsLabelEntry> results)
        {
            var missing = results.Select(r => r.LayerId).Distinct().Where(l => !_loadedLayers.Contains(l)).OrderBy(l => l).ToList();
            return missing.Count == 0 ? ""
                : " Label 글자를 보려면 Layer " + string.Join(", ", missing) + " 을(를) 체크하고 [조회]를 누르세요.";
        }

        /// <summary>목록 문구: 값 / Layer / 화면 좌표(지금 보기 기준).</summary>
        private string DescribeLabelResult(int index)
        {
            GdsLabelEntry entry = _labelResults[index];
            return (index + 1).ToString(CultureInfo.InvariantCulture) + ". " + entry.Text + "  |  Layer " + entry.LayerId
                + "  |  (" + ToDisplayX(entry.StoredX).ToString("0.###", CultureInfo.InvariantCulture)
                + ", " + entry.Y.ToString("0.###", CultureInfo.InvariantCulture) + ")";
        }

        private GPoint LabelDisplayPoint(GdsLabelEntry entry)
        {
            return new GPoint(ToDisplayX(entry.StoredX), entry.Y);
        }

        private void cboLabelResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingLabelResults || cboLabelResults.SelectedIndex < 0) return;
            FocusLabel(cboLabelResults.SelectedIndex);
        }

        /// <summary>목록에서 index를 선택한다(선택 변경 이벤트가 이동을 한다).</summary>
        private void SelectLabelResult(int index)
        {
            if (index < 0 || index >= cboLabelResults.Items.Count) return;
            if (cboLabelResults.SelectedIndex == index)
                cboLabelResults_SelectedIndexChanged(cboLabelResults, EventArgs.Empty);
            else
                cboLabelResults.SelectedIndex = index;
        }

        private void FocusLabel(int index)
        {
            GPoint p = LabelDisplayPoint(_labelResults[index]);
            double half = LabelFocusSize / 2;
            FocusArea(new GBox(p.X - half, p.Y - half, p.X + half, p.Y + half));
        }

        /// <summary>
        /// area가 화면에 맞게 보이도록 이동 / 확대한다. 기존 FocusChainBounds(받은 범위의 4배를 보여 줌)를 그대로 쓰려고
        /// 같은 중심의 1/4 크기 범위를 넘긴다(GdsMapControl 수정 없음).
        /// </summary>
        private void FocusArea(GBox area)
        {
            double cx = area.MinX + area.Width / 2, cy = area.MinY + area.Height / 2;
            double hw = area.Width / 8, hh = area.Height / 8;
            map.FocusChainBounds(new GBox(cx - hw, cy - hh, cx + hw, cy + hh));
        }

        /// <summary>[전체 보기]: 목록 결과 전체가 한 화면에 들어오도록(가장자리 결과도 300µm 여유).</summary>
        private void btnLabelAll_Click(object sender, EventArgs e)
        {
            int count = Math.Min(_labelResults.Count, LabelResultLimit);
            if (count == 0) return;
            double half = LabelFocusSize / 2;
            GBox area = GBox.Empty;
            foreach (GdsLabelEntry entry in _labelResults.Take(count))
            {
                GPoint p = LabelDisplayPoint(entry);
                area.Include(p.X - half, p.Y - half);
                area.Include(p.X + half, p.Y + half);
            }
            FocusArea(area);
            SetStatus("검색 결과 " + count.ToString("N0") + "건 전체 보기");
        }

        /// <summary>보기 기준을 바꾼 뒤: 같은 결과를 새 기준 X로 목록에 다시 표시한다(색인은 저장 기준이라 그대로).</summary>
        private void RefreshLabelResultsForSide()
        {
            if (_labelResults.Count == 0) return;
            int selected = cboLabelResults.SelectedIndex;
            _updatingLabelResults = true;
            cboLabelResults.BeginUpdate();
            for (int i = 0; i < cboLabelResults.Items.Count; i++)
                cboLabelResults.Items[i] = DescribeLabelResult(i);
            cboLabelResults.SelectedIndex = selected;
            cboLabelResults.EndUpdate();
            _updatingLabelResults = false;
        }
    }
}
