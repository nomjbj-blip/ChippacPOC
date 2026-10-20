using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// E-Test 결과 1개 파일을 보여 주는 공통 Control (2026-10-15). 업로드 화면(저장 전)과 조회 화면(저장된 결과)이 같은 Control 을 쓴다.
    /// 설계: 문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md 3절
    ///  - 왼쪽 정보 표: 항목 / 값 (EtResultData.Info). EtResultData.EditableInfo 항목은 값 칸을 더블클릭해 고친다(옅은 파란 칸, 업로드 화면만).
    ///    고치면 InfoEdited 를 알린다(편집이 끝난 뒤) -> 쓰는 화면이 그 값으로 다시 분석해 ShowData 를 다시 부른다.
    ///  - 1단계 Die: DUT / X / Y / Map(X,Y) / 판정 / Fail 수 / '-' 수 / DUT Flag(참고). Fail Die 행은 빨간색. 여러 행을 고를 수 있다.
    ///  - 2단계 정의 Para 값: 한 행 = Die 1개 x Parameter 1개. 1단계에서 고른 Die 만 보인다(고르지 않으면 전체 Die).
    ///    거르기 = [Fail 만] / ['-' 만] / 찾기(Parameter · 헤더 원문 · Chain · GDS Chain 글자, 대소문자 무시). 판정 F 는 빨간색, '-' 는 회색.
    /// 데이터는 받기만 한다(판정 계산 / 서버 조회 없음). 쓰는 쪽: ShowData(EtResultData) / Clear().
    /// </summary>
    public partial class EtResultDataControl : UserControl
    {
        private EtResultData data;
        private DataView valueView;
        /// <summary>Die 표를 채우는 중(선택 변경 이벤트 무시)</summary>
        private bool filling;
        /// <summary>정보 표 편집 전 값 (바뀌었을 때만 알린다)</summary>
        private string editBefore;
        /// <summary>정보 표에서 고칠 수 있는 칸 배경</summary>
        private static readonly Color EditableBack = Color.FromArgb(232, 242, 255);

        /// <summary>정보 표의 고칠 수 있는 칸(EtResultData.EditableInfo)을 고쳤을 때. 편집이 끝난 뒤 화면 스레드에서 알린다.</summary>
        public event EventHandler<InfoEditedEventArgs> InfoEdited;

        public EtResultDataControl()
        {
            InitializeComponent();
            DACrux.SEMDMS.Control.GridBuffer.Enable(this); // 표 스크롤 잔상 방지
            foreach (DataGridViewColumn column in new DataGridViewColumn[] { colDDut, colDX, colDY, colDFail, colDNotTested, colVDut, colVValue, colVLower, colVUpper })
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colInfoName.DefaultCellStyle.ForeColor = Color.DimGray;
            colDFlag.DefaultCellStyle.ForeColor = Color.Gray;
            dgvValue.AutoGenerateColumns = false;
            Clear();
        }

        /// <summary>지금 보이는 데이터 (없으면 null)</summary>
        public EtResultData CurrentData
        {
            get { return data; }
        }

        /// <summary>표를 모두 비운다.</summary>
        public void Clear()
        {
            ShowData(null);
        }

        /// <summary>결과 데이터를 세 표에 채운다. null 이면 비운다. 거르기 조건(Fail 만 / '-' 만 / 찾기)은 그대로 둔다.</summary>
        public void ShowData(EtResultData value)
        {
            data = value;
            filling = true;
            try
            {
                dgvInfo.Rows.Clear();
                dgvDie.Rows.Clear();
                dgvValue.DataSource = null;
                valueView = null;
                lblInfoTitle.Text = value == null ? "결과 정보" : value.Title;
                lblDieTitle.Text = "1단계: Die";
                if (value == null)
                {
                    lblCount.Text = string.Empty;
                    return;
                }

                foreach (KeyValuePair<string, string> info in value.Info)
                {
                    DataGridViewRow row = dgvInfo.Rows[dgvInfo.Rows.Add(info.Key, info.Value)];
                    bool editable = value.EditableInfo.Contains(info.Key);
                    row.Cells[colInfoValue.Index].ReadOnly = !editable;
                    if (editable) row.Cells[colInfoValue.Index].Style.BackColor = EditableBack;
                }

                int failDie = 0;
                for (int i = 0; i < value.Dies.Count; i++)
                {
                    EtResultData.Die d = value.Dies[i];
                    int index = dgvDie.Rows.Add("#" + d.Dut, d.X, d.Y, d.Map, d.Judge,
                        d.FailCount.ToString(CultureInfo.InvariantCulture), d.NotTestedCount.ToString(CultureInfo.InvariantCulture), d.Flag);
                    DataGridViewRow row = dgvDie.Rows[index];
                    row.Tag = i;
                    if (d.Judge == "Fail")
                    {
                        failDie++;
                        row.DefaultCellStyle.BackColor = Color.MistyRose;
                        row.Cells[colDJudge.Index].Style.ForeColor = Color.Firebrick;
                    }
                }
                dgvDie.ClearSelection();
                lblDieTitle.Text = "1단계: Die " + value.Dies.Count + " (Fail " + failDie + ")";

                valueView = new DataView(BuildValueTable(value));
                dgvValue.DataSource = valueView;
            }
            finally
            {
                filling = false;
            }
            ApplyFilter();
        }

        /// <summary>2단계 표: 모든 Die x 모든 Parameter (보이는 행은 DataView 로 거른다)</summary>
        private static DataTable BuildValueTable(EtResultData value)
        {
            DataTable table = new DataTable("ET_RESULT_VALUE");
            table.Columns.Add("DIE_INDEX", typeof(int));
            table.Columns.Add("PARAM_INDEX", typeof(int));
            table.Columns.Add("STATUS", typeof(string));
            table.Columns.Add("JUDGE_FLAG", typeof(string));
            foreach (string name in new[] { "DUT", "XY", "NAME", "HEADER", "CHAIN", "GDS", "UNIT", "VALUE", "LOWER", "UPPER", "JUDGE", "REASON" })
                table.Columns.Add(name, typeof(string));
            table.BeginLoadData();
            for (int i = 0; i < value.Dies.Count; i++)
            {
                EtResultData.Die d = value.Dies[i];
                for (int m = 0; m < value.Params.Count; m++)
                {
                    EtResultData.Param p = value.Params[m];
                    EtResultData.Cell c = value.Cells[i, m];
                    string judge = c.Judge == EtJudgeRule.Fail ? "Fail" : c.Judge == EtJudgeRule.Pass ? "Pass" : string.Empty;
                    string reason = c.Judge == EtJudgeRule.Fail ? EtJudgeRule.FailReason(c.Status, c.Value, p.Lower, StripOff(p.Upper))
                        : c.Status == 'E' ? "빈 칸" : string.Empty;
                    table.Rows.Add(i, m, c.Status == '\0' ? string.Empty : c.Status.ToString(), c.Judge == '\0' ? string.Empty : c.Judge.ToString(),
                        "#" + d.Dut, "(" + d.X + "," + d.Y + ")", p.Name, p.Header, p.Chain, p.Gds, p.Unit, c.Value, p.Lower, p.Upper, judge, reason);
                }
            }
            table.EndLoadData();
            return table;
        }

        private static string StripOff(string upper)
        {
            const string off = " (Off)";
            return upper != null && upper.EndsWith(off, StringComparison.Ordinal) ? upper.Substring(0, upper.Length - off.Length) : upper;
        }

        /// <summary>고칠 수 있는 정보 칸을 더블클릭하면 편집을 시작한다.</summary>
        private void dgvInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colInfoValue.Index) return;
            DataGridViewCell cell = dgvInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.ReadOnly) return;
            editBefore = Convert.ToString(cell.Value, CultureInfo.InvariantCulture);
            dgvInfo.CurrentCell = cell;
            dgvInfo.BeginEdit(true);
        }

        /// <summary>
        /// 정보 칸 편집이 끝나면 값이 바뀌었을 때만 InfoEdited 를 알린다. 받는 쪽이 표를 다시 채우므로(ShowData)
        /// 편집 이벤트 안에서 부르면 재진입 오류가 나서 BeginInvoke 로 편집이 끝난 뒤에 부른다.
        /// </summary>
        private void dgvInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colInfoValue.Index) return;
            string name = Convert.ToString(dgvInfo.Rows[e.RowIndex].Cells[colInfoName.Index].Value, CultureInfo.InvariantCulture);
            string value = Convert.ToString(dgvInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, CultureInfo.InvariantCulture).Trim();
            if (value == editBefore || InfoEdited == null) return;
            BeginInvoke((MethodInvoker)delegate { InfoEdited(this, new InfoEditedEventArgs(name, value)); });
        }

        private void dgvDie_SelectionChanged(object sender, EventArgs e)
        {
            if (!filling) ApplyFilter();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        /// <summary>고른 Die / Fail 만 / '-' 만 / 찾기 글자로 2단계 표를 거른다(다시 만들지 않음).</summary>
        private void ApplyFilter()
        {
            if (valueView == null) return;
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
            {
                List<string> parts = new List<string>();
                List<int> dies = dgvDie.SelectedRows.Cast<DataGridViewRow>().Where(r => r.Tag is int).Select(r => (int)r.Tag).OrderBy(i => i).ToList();
                if (dies.Count > 0)
                    parts.Add("DIE_INDEX IN (" + string.Join(",", dies.Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray()) + ")");
                if (chkFailOnly.Checked)
                    parts.Add("JUDGE_FLAG = 'F'");
                if (chkNotTested.Checked)
                    parts.Add("STATUS = 'N'");
                string find = txtFind.Text.Trim();
                if (find.Length > 0)
                {
                    string like = EscapeLike(find);
                    parts.Add("(NAME LIKE '*" + like + "*' OR HEADER LIKE '*" + like + "*' OR CHAIN LIKE '*" + like + "*' OR GDS LIKE '*" + like + "*')");
                }
                valueView.RowFilter = string.Join(" AND ", parts.ToArray());
                lblValueTitle.Text = "2단계: 정의 Para 값" + (dies.Count == 0 ? " (전체 Die)" : dies.Count == 1 ? " (DUT " + data.Dies[dies[0]].Dut + ")" : " (Die " + dies.Count + "개)");
                lblCount.Text = "표시 " + valueView.Count.ToString("N0", CultureInfo.InvariantCulture) + " / 전체 " + valueView.Table.Rows.Count.ToString("N0", CultureInfo.InvariantCulture)
                    + " (Parameter " + data.Params.Count + " x Die " + data.Dies.Count + ")";
            }
        }

        /// <summary>DataView LIKE 특수 문자([ ] * %)와 작은따옴표를 처리한다. DataView LIKE 는 대소문자를 구분하지 않는다.</summary>
        private static string EscapeLike(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in text)
            {
                if (ch == '[' || ch == ']' || ch == '*' || ch == '%') sb.Append('[').Append(ch).Append(']');
                else if (ch == '\'') sb.Append("''");
                else sb.Append(ch);
            }
            return sb.ToString();
        }

        /// <summary>판정 F 행은 빨간색, '-' 값은 회색 가운데 정렬.</summary>
        private void dgvValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataRowView row = dgvValue.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;
            if (row["JUDGE_FLAG"].ToString() == "F")
            {
                e.CellStyle.BackColor = Color.MistyRose;
                if (e.ColumnIndex == colVValue.Index || e.ColumnIndex == colVJudge.Index || e.ColumnIndex == colVReason.Index)
                    e.CellStyle.ForeColor = Color.Firebrick;
            }
            if (e.ColumnIndex == colVValue.Index && row["STATUS"].ToString() == "N")
            {
                if (e.CellStyle.ForeColor != Color.Firebrick) e.CellStyle.ForeColor = Color.DimGray;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }

    /// <summary>정보 표 편집 결과: 항목 이름(EtResultData.InfoDevice 등)과 새 값(앞뒤 공백 제거)</summary>
    public sealed class InfoEditedEventArgs : EventArgs
    {
        public InfoEditedEventArgs(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}
