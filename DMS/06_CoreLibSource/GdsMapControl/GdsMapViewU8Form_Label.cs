using NexplantQMS.GdsMap.Chain;
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
    /// GDS Map View U8 Label 검색 (2026-10-20). 기존 GDS Map View 와 같은 규칙(포함, 대소문자 무시)이다.
    /// 색인은 [조회] 때 한 번 받아 두고, 같은 형태 찾기에서 덩어리 이름(TSV 번호 / Net Label)을 붙일 때도 쓴다.
    /// </summary>
    public partial class GdsMapViewU8Form
    {
        private const int LabelResultLimit = 1000;
        /// <summary>결과로 이동할 때 보이는 범위(µm).</summary>
        private const double LabelFocusSize = 300.0;

        private GdsLabelIndex _labelIndex;
        private string _labelIndexError;
        private List<GdsLabelEntry> _labelResults = new List<GdsLabelEntry>();
        private bool _updatingLabelResults;

        private void InitializeLabelSearchState()
        {
            SetLabelSearchEnabled(false);
        }

        /// <summary>서비스에서 Label 색인을 받는다(백그라운드).</summary>
        private static GdsLabelIndex ReadLabelIndex(DACrux.SEMDMS.RO.GdsMapView service, string factory, string deviceId)
        {
            using (DataTable table = service.GetMapTextIndex(factory, deviceId))
                return GdsLabelIndex.FromTable(table);
        }

        private void ReplaceLabelIndex(GdsLabelIndex index, string error)
        {
            ClearLabelResults();
            _labelIndex = index;
            _labelIndexError = error;
            SetLabelSearchEnabled(true);
        }

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

        /// <summary>입력한 Label 을 색인에서 찾아 목록에 보여 주고 첫 건으로 이동한다.</summary>
        private void RunLabelSearch()
        {
            if (_labelIndex == null)
            {
                SetStatus(_labelIndexError == null ? "조회된 도면 없음" : "Label 색인 없음: " + _labelIndexError);
                return;
            }
            string text = txtLabel.Text.Trim();
            if (text.Length == 0)
            {
                MessageBox.Show(this, "찾을 Label을 입력하세요. 예: TSV_2162", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLabel.Focus();
                return;
            }
            List<GdsLabelEntry> results = _labelIndex.Search(text, GdsLabelMatchMode.Contains);
            ClearLabelResults();
            _labelResults = results;
            if (results.Count == 0)
            {
                SetLabelSearchEnabled(true);
                SetStatus(_labelIndex.Count == 0 ? "이 Map에는 TEXT(Label)가 없습니다." : "'" + text + "'이(가) 들어 있는 Label이 없습니다.");
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
                + (results.Count > LabelResultLimit ? "(목록에는 앞 " + LabelResultLimit.ToString("N0") + "건)" : ""));
        }

        private string DescribeLabelResult(int index)
        {
            GdsLabelEntry entry = _labelResults[index];
            return (index + 1).ToString(CultureInfo.InvariantCulture) + ". " + entry.Text + "  |  Layer " + GdsLayerKey.Format(GdsLayerKey.Make(entry.LayerId, entry.TextType ?? 0))
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
            GPoint p = LabelDisplayPoint(_labelResults[cboLabelResults.SelectedIndex]);
            double half = LabelFocusSize / 2;
            FocusArea(new GBox(p.X - half, p.Y - half, p.X + half, p.Y + half));
        }

        private void SelectLabelResult(int index)
        {
            if (index < 0 || index >= cboLabelResults.Items.Count) return;
            if (cboLabelResults.SelectedIndex == index)
                cboLabelResults_SelectedIndexChanged(cboLabelResults, EventArgs.Empty);
            else
                cboLabelResults.SelectedIndex = index;
        }

        /// <summary>area 가 화면에 맞게 보이도록 이동 / 확대한다(FocusChainBounds 는 받은 범위의 4배를 보이므로 1/4 크기로 넘긴다).</summary>
        private void FocusArea(GBox area)
        {
            double cx = area.MinX + area.Width / 2, cy = area.MinY + area.Height / 2;
            double hw = area.Width / 8, hh = area.Height / 8;
            map.FocusChainBounds(new GBox(cx - hw, cy - hh, cx + hw, cy + hh));
        }

        /// <summary>[전체 보기]: 목록 결과 전체가 한 화면에 들어오도록.</summary>
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

        /// <summary>보기 기준을 바꾼 뒤 같은 결과를 새 기준 X 로 다시 표시한다.</summary>
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

        /// <summary>
        /// 덩어리에 붙일 Label(화면 좌표). 번호 Label / Net Label Layer 만 고른다.
        /// Label 은 도면에 올리지 않은 Layer 여도 색인에 있으므로 이름 / Code 를 붙일 수 있다.
        /// </summary>
        private List<ChainComponentLabel> NamingLabels(ChainNetNameRule rule)
        {
            var result = new List<ChainComponentLabel>();
            if (_labelIndex == null) return result;
            foreach (GdsLabelEntry entry in _labelIndex.Entries)
            {
                int key = GdsLayerKey.Make(entry.LayerId, entry.TextType ?? 0);
                if (key != rule.NumberLabelLayerKey && key != rule.NetLabelLayerKey) continue;
                result.Add(new ChainComponentLabel(entry.Text, key, LabelDisplayPoint(entry)));
            }
            return result;
        }
    }
}
