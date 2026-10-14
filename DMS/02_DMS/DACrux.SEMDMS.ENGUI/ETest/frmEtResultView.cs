using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// 업로드된 E-Test 파일 1건의 저장 데이터를 보여 주는 조회 창이다. 업로드 화면의 등록 파일 목록을 더블클릭하면 열린다.
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 11절
    /// 처리 흐름:
    ///  1) 파일 정보 / Die 목록 / Device 측정 항목 / 측정값 전체를 한 번 조회한다(서버 GdsEtResult).
    ///  2) 측정값을 행 = 측정 항목, 열 = Die 표로 펼친다. 항목이 2,755개라 반대로 펼치면 열이 너무 많아진다.
    ///  3) 거르기(Para / Chain 포함 / '-' 있는 항목만)는 다시 조회하지 않고 DataView RowFilter로 한다.
    /// 값은 저장값 그대로(R 형식) 보여 준다. "-" 셀은 회색이다(Fail 판정은 Spec 정의 항목만, 2026-10-12). Fail Die(DUT Flag) 열 머리글은 붉은색이다.
    /// Limit 범위 밖 값 색은 아직 표시하지 않는다(Limit 설정 화면 연동 단계).
    /// </summary>
    public partial class frmEtResultView : Form
    {
        private const string AllPara = "(전체)";
        private const string DieColumnPrefix = "D_";

        private readonly RO.GdsEtResult service;
        private readonly string srcFileSeq;
        private DataView valueView;

        /// <summary>디자이너용 기본 생성자.</summary>
        public frmEtResultView()
        {
            InitializeComponent();
        }

        /// <summary>조회할 파일 번호와 서비스(업로드 화면의 RO)를 받는다.</summary>
        public frmEtResultView(RO.GdsEtResult service, string srcFileSeq)
            : this()
        {
            this.service = service;
            this.srcFileSeq = srcFileSeq;
        }

        /// <summary>창이 열릴 때 데이터를 조회한다. 실패하면 알리고 창을 닫는다.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (service == null) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                LoadData();
            }
            catch (Exception ex)
            {
                Exception cause = ex is System.Reflection.TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                MessageBox.Show(this, "E-Test 데이터를 조회하지 못했습니다.\r\n\r\n" + cause.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BeginInvoke(new MethodInvoker(Close));
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #region [조회 / 표 만들기]

        /// <summary>파일 / Die / 항목 / 측정값을 조회해 두 표를 채운다.</summary>
        private void LoadData()
        {
            DataTable file = service.GetEtFile(srcFileSeq);
            if (file.Rows.Count == 0)
                throw new InvalidOperationException("파일 번호 " + srcFileSeq + " 가 없습니다(삭제되었을 수 있습니다).");
            DataRow f = file.Rows[0];
            DataTable dies = service.GetEtDieList(srcFileSeq);
            DataTable items = service.GetEtItemList(f["FACTORY"].ToString(), f["DEVICE_ID"].ToString());
            DataTable values = service.GetEtValueList(srcFileSeq);

            // Die 열 순서 (DUT 번호 순) / Fail 여부
            Dictionary<string, int> dieIndex = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < dies.Rows.Count; i++)
                dieIndex[dies.Rows[i]["DUT_NO"].ToString()] = i;

            // 측정값 표: 항목 정보 열 + Die 열
            DataTable table = new DataTable("ET_VALUE");
            table.Columns.Add("ET_ITEM_SEQ", typeof(long));
            table.Columns.Add("DISPLAY_ORDER", typeof(long));
            table.Columns.Add("PARA_KEY", typeof(string));
            table.Columns.Add("NOT_TESTED", typeof(int));
            table.Columns.Add("PARA_NAME", typeof(string));
            table.Columns.Add("COND_VALUE", typeof(string));
            table.Columns.Add("CHAIN_NAME", typeof(string));
            table.Columns.Add("CHAIN_NAME2", typeof(string));
            table.Columns.Add("UNIT", typeof(string));
            table.Columns.Add("TEST_NO", typeof(string));
            foreach (DataRow d in dies.Rows)
                table.Columns.Add(DieColumnPrefix + d["DUT_NO"], typeof(string));

            Dictionary<string, DataRow> itemRows = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            foreach (DataRow it in items.Rows)
            {
                DataRow r = table.NewRow();
                r["ET_ITEM_SEQ"] = Convert.ToInt64(it["ET_ITEM_SEQ"]);
                r["DISPLAY_ORDER"] = it["DISPLAY_ORDER"] == DBNull.Value ? long.MaxValue : Convert.ToInt64(it["DISPLAY_ORDER"]);
                r["PARA_NAME"] = it["PARA_NAME"].ToString();
                r["COND_VALUE"] = it["COND_VALUE"].ToString();
                r["PARA_KEY"] = ParaKey(it["PARA_NAME"].ToString(), it["COND_VALUE"].ToString());
                r["CHAIN_NAME"] = it["CHAIN_NAME"].ToString();
                r["CHAIN_NAME2"] = it["CHAIN_NAME2"].ToString();
                r["UNIT"] = it["UNIT"].ToString();
                r["TEST_NO"] = it["TEST_NO"].ToString();
                r["NOT_TESTED"] = 0;
                itemRows[it["ET_ITEM_SEQ"].ToString()] = r;
            }

            // 측정값을 펼친다. 이 파일에 값이 있는 항목만 표에 넣는다(항목 마스터는 Device 단위라 다른 파일 항목도 있을 수 있음).
            HashSet<DataRow> used = new HashSet<DataRow>();
            int[] dieNotTested = new int[dies.Rows.Count];
            long notTestedTotal = 0;
            foreach (DataRow v in values.Rows)
            {
                DataRow r;
                if (!itemRows.TryGetValue(v["ET_ITEM_SEQ"].ToString(), out r)) continue;
                string dut = v["DUT_NO"].ToString();
                string status = v["VALUE_STATUS"].ToString();
                string text;
                if (status == "V") text = Convert.ToDouble(v["MEAS_VALUE"], CultureInfo.InvariantCulture).ToString("R", CultureInfo.InvariantCulture);
                else if (status == "N")
                {
                    text = "-";
                    r["NOT_TESTED"] = (int)r["NOT_TESTED"] + 1;
                    int di;
                    if (dieIndex.TryGetValue(dut, out di)) dieNotTested[di]++;
                    notTestedTotal++;
                }
                else text = string.Empty;
                if (table.Columns.Contains(DieColumnPrefix + dut)) r[DieColumnPrefix + dut] = text;
                used.Add(r);
            }
            List<DataRow> ordered = new List<DataRow>(used);
            ordered.Sort((a, b) => ((long)a["DISPLAY_ORDER"]).CompareTo((long)b["DISPLAY_ORDER"]));
            foreach (DataRow r in ordered) table.Rows.Add(r);

            // 파일 정보 줄
            Text = "E-Test 업로드 데이터 조회 - " + f["WAFER_ID"] + " (" + FormatTestTime(f["TEST_TIME"].ToString()) + ")"
                + (f["LOAD_STATUS"].ToString() == "READY" ? string.Empty : " [" + f["LOAD_STATUS"] + "]");
            // 2026-10-12: 파일명 8칸 규칙 값(Customer / 파일 Device / Wafer Type / Rev / Fab)과 Pass / Fail Die 수를 함께 보여 준다(이전 규칙 파일은 DDL 이 채움).
            lblInfo.Text = "WAFER_ID " + f["WAFER_ID"] + "  /  Lot " + f["LOT_ID"] + "  /  Slot " + f["SLOT_NO"] + "  /  Lot-Wafer No " + f["LOT_WAFER_NO"]
                + "  /  Factory " + f["FACTORY"] + "  /  Device " + f["DEVICE_ID"] + "  /  면 " + f["VIEW_SIDE"] + "\r\n"
                + "Customer " + f["CUSTOMER"] + "  /  파일 Device " + f["FILE_DEVICE"] + "  /  Wafer Type " + f["WAFER_TYPE"] + "  /  Wafer Rev " + f["WAFER_REV"]
                + "  /  공정 " + f["TEST_PROGRAM"] + "  /  Test Date " + FormatTestTime(f["TEST_TIME"].ToString())
                + "  /  Fab " + (f["FAB_NAME"].ToString().Length > 0 ? f["FAB_NAME"].ToString() : "(없음)") + "\r\n"
                + "Die " + dies.Rows.Count + " (Pass " + f["PASS_DIE_COUNT"] + " / Fail " + f["FAIL_DIE_COUNT"] + ")  /  측정 항목 " + table.Rows.Count
                + "  /  측정값 " + values.Rows.Count.ToString("N0", CultureInfo.InvariantCulture)
                + " ('-' " + notTestedTotal + ")  /  상태 " + f["LOAD_STATUS"] + "  /  파일 번호 " + f["SRC_FILE_SEQ"]
                + "  /  등록 " + f["CREATE_USER"] + " " + FormatDate(f["CREATE_TIME"]) + "  /  파일 " + f["FILE_NAME"];

            BindDieGrid(dies, dieNotTested);
            BindValueGrid(table, dies);
            FillParaList(table);
        }

        /// <summary>왼쪽 Die 목록 (Die별 '-' 값 수 포함)</summary>
        private void BindDieGrid(DataTable dies, int[] dieNotTested)
        {
            DataTable dt = new DataTable("ET_DIE_VIEW");
            foreach (string name in new[] { "DUT", "X", "Y", "HW Bin", "SW Bin", "P/F", "Flag", "'-' 값" })
                dt.Columns.Add(name, typeof(string));
            for (int i = 0; i < dies.Rows.Count; i++)
            {
                DataRow d = dies.Rows[i];
                dt.Rows.Add("#" + d["DUT_NO"], d["DIE_X"].ToString(), d["DIE_Y"].ToString(), d["HW_BIN"].ToString(), d["SW_BIN"].ToString(),
                    d["PASS_FLAG"].ToString() == "F" ? "Fail" : "Pass", d["FLAG_CODE"].ToString(), dieNotTested[i].ToString(CultureInfo.InvariantCulture));
            }
            dgvDie.DataSource = dt;
            foreach (DataGridViewRow row in dgvDie.Rows)
                if (Convert.ToString(row.Cells["P/F"].Value) == "Fail") row.DefaultCellStyle.ForeColor = Color.Firebrick;
        }

        /// <summary>오른쪽 측정값 표. 항목 정보 열은 고정하고 Die 열 머리글은 "#DUT (X,Y)", Fail Die는 붉은색.</summary>
        private void BindValueGrid(DataTable table, DataTable dies)
        {
            valueView = new DataView(table);
            dgvValue.AutoGenerateColumns = true;
            dgvValue.DataSource = valueView;
            foreach (string hidden in new[] { "ET_ITEM_SEQ", "DISPLAY_ORDER", "PARA_KEY", "NOT_TESTED" })
                dgvValue.Columns[hidden].Visible = false;
            SetHeader("PARA_NAME", "Para", 150);
            SetHeader("COND_VALUE", "조건", 60);
            SetHeader("CHAIN_NAME", "Chain", 140);
            SetHeader("CHAIN_NAME2", "Chain2", 120);
            SetHeader("UNIT", "단위", 45);
            SetHeader("TEST_NO", "Test No", 55);
            dgvValue.Columns["TEST_NO"].Frozen = true;
            foreach (DataRow d in dies.Rows)
            {
                DataGridViewColumn column = dgvValue.Columns[DieColumnPrefix + d["DUT_NO"]];
                column.HeaderText = "#" + d["DUT_NO"] + " (" + d["DIE_X"] + "," + d["DIE_Y"] + ")";
                column.Width = 82;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (d["PASS_FLAG"].ToString() == "F")
                {
                    column.HeaderCell.Style.ForeColor = Color.White;
                    column.HeaderCell.Style.BackColor = Color.IndianRed;
                    column.ToolTipText = "Fail Die (" + d["FLAG_CODE"] + ")";
                }
            }
        }

        private void SetHeader(string name, string header, int width)
        {
            DataGridViewColumn column = dgvValue.Columns[name];
            column.HeaderText = header;
            column.Width = width;
        }

        /// <summary>Para 거르기 목록: (전체) + 표에 있는 Para[=조건] (항목 순서대로)</summary>
        private void FillParaList(DataTable table)
        {
            cboPara.BeginUpdate();
            cboPara.Items.Clear();
            cboPara.Items.Add(AllPara);
            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (DataRow r in table.Rows)
                if (seen.Add(r["PARA_KEY"].ToString())) cboPara.Items.Add(r["PARA_KEY"].ToString());
            cboPara.EndUpdate();
            cboPara.SelectedIndex = 0;
            ApplyFilter();
        }

        private static string ParaKey(string para, string cond)
        {
            return cond.Length == 0 ? para : para + "=" + cond;
        }

        /// <summary>DB 날짜를 yyyy-MM-dd HH:mm:ss 로 표시한다(PC 지역 설정의 오전 / 오후 표시를 쓰지 않음).</summary>
        private static string FormatDate(object value)
        {
            return value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value);
        }

        private static string FormatTestTime(string time)
        {
            DateTime value;
            return DateTime.TryParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
                ? value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : time;
        }

        #endregion

        #region [거르기 / 이동 / 표시]

        private void Filter_Changed(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        /// <summary>Para / Chain 포함 / '-' 있는 항목만 조건을 RowFilter로 건다.</summary>
        private void ApplyFilter()
        {
            if (valueView == null) return;
            List<string> parts = new List<string>();
            string para = cboPara.SelectedItem as string;
            if (!string.IsNullOrEmpty(para) && para != AllPara)
                parts.Add("PARA_KEY = '" + para.Replace("'", "''") + "'");
            string chain = txtChain.Text.Trim();
            if (chain.Length > 0)
            {
                string like = EscapeLike(chain);
                parts.Add("(CHAIN_NAME LIKE '*" + like + "*' OR CHAIN_NAME2 LIKE '*" + like + "*')");
            }
            if (chkNotTested.Checked)
                parts.Add("NOT_TESTED > 0");
            valueView.RowFilter = string.Join(" AND ", parts.ToArray());
            lblCount.Text = "표시 " + valueView.Count.ToString("N0", CultureInfo.InvariantCulture) + " / "
                + valueView.Table.Rows.Count.ToString("N0", CultureInfo.InvariantCulture) + " 항목";
        }

        /// <summary>DataView LIKE 특수 문자([ ] * %)와 작은따옴표를 처리한다. DataView LIKE는 대소문자를 구분하지 않는다(CaseSensitive 기본 false).</summary>
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

        /// <summary>Die를 더블클릭하면 측정값 표가 그 Die 열로 이동한다.</summary>
        private void dgvDie_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string dut = Convert.ToString(dgvDie.Rows[e.RowIndex].Cells["DUT"].Value).TrimStart('#');
            DataGridViewColumn column = dgvValue.Columns[DieColumnPrefix + dut];
            if (column == null) return;
            try
            {
                dgvValue.FirstDisplayedScrollingColumnIndex = column.Index;
                if (dgvValue.Rows.Count > 0)
                    dgvValue.CurrentCell = dgvValue.Rows[Math.Max(0, dgvValue.FirstDisplayedScrollingRowIndex)].Cells[column.Index];
            }
            catch (InvalidOperationException)
            {
                // 표가 아직 그려지지 않은 경우 이동하지 않는다
            }
        }

        /// <summary>"-" 셀은 회색 가운데 정렬. Fail 여부는 Spec 정의 항목만 판정하므로(정의된 항목의 '-' 만 Fail, 2026-10-12) 여기서는 색으로 Fail 을 표시하지 않는다.</summary>
        private void dgvValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (!dgvValue.Columns[e.ColumnIndex].Name.StartsWith(DieColumnPrefix, StringComparison.Ordinal)) return;
            if (Convert.ToString(e.Value) == "-")
            {
                e.CellStyle.ForeColor = Color.DimGray;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        #endregion
    }
}
