using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

/// <summary>
/// E-Test Bin 설정 화면(frmEtBinSetup)을 화면 밖에서 띄워 다음 흐름을 검사한다. (2026-10-22)
///  1) 구성: Factory / YMS Program 읽기 전용, Device / 공정 콤보는 목록에서만, 표 편집은 더블클릭 / 클릭, [저장] 굵은 글자, 열 머리글
///  2) Device 를 고르면 바로 조회: YMS Program / Bin 1 · 5 / Software Bin / Die / Pass / Fail / 파일 / 색 칸 배경
///  3) 편집: 이름(노란색) / 양품 클릭(상태 '파일 결과와 다름') / 색 창(#RRGGBB, 굵은 글자) / 읽기 칸은 편집 안 됨
///  4) [Bin 추가]: 같은 Bin 번호 / 설명 100바이트 넘음 -> 붉은 칸, 저장 막음 -> 바르게 고쳐 저장
///  5) [저장]: 양품이 바뀌면 수율 다시 계산 질문 -> 예 -> YMS 수율 83.33 -> 100, 다시 조회. 이름만 바꾸면 질문 없음
///  6) 저장 안 한 수정: 조회 질문 아니요 = 그대로 / 예 = 버림, 그 상태에서 정의 삭제는 막음
///  7) [선택 삭제]: 정의 삭제 확인 -> 표에서 빠짐, 새 행은 표에서만 뺌. 마지막에 양품을 되돌려 수율 83.33
/// Service(Remoting) 대신 RO.GdsEtResult(iGdsEtResult) 생성자로 BSL.GdsEtResult 를 같은 프로세스에 붙인다(화면 -> RO -> BSL -> 로컬 DB).
/// 시험 데이터: FACTORY=PROBE / Device PRBBINUI / 공정 TSVET / Slot 7. 시험 Map(TQP_USEMAP)은 Verify-EtBinSetup.ps1 이 만들고 지운다.
///   준비에서 고객 Spec 표 TSVET 37개를 등록하고 샘플을 업로드한다(YMS 반영: Bin 1 Pass 20 / Bin 5 Fail 4 -> 수율 83.33). keep 이 없으면 끝에 모두 지운다.
/// 인자: [0] E-Test xlsx, [1] 결과 이미지(png), [2] 고객 Spec 표(탭 구분 텍스트), [3] keep(선택)
/// </summary>
internal static class EtBinSetupFormSmoke
{
    private const string Factory = "PROBE";
    private const string Device = "PRBBINUI";
    private const string Slot = "7";
    private const string Lot = "RH-HS2615";
    private static readonly string Program = EtYmsRule.ToProgramName(Device, "TSVET");
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        bool keep = args.Length > 3 && args[3] == "keep";
        DACrux.Base.GlobalVariable.Factory = Factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";
        var bsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // ---- 0) 준비: 잔여 정리 -> TSVET 37개 등록 + Spec -> 샘플 업로드(시험 Map 있음 -> YMS 반영) ----
        Cleanup(bsl);
        string seq = Prepare(bsl, args[0], File.ReadAllText(args[2]));
        Check("준비: 업로드 -> YMS 반영 / 수율 83.33", Yield(seq) == 83.33, "파일 " + seq + " / Program " + Program + " / 수율 " + Yield(seq));

        Type type = typeof(DACrux.SEMDMS.ENGUI.frmEtBinSetup);
        using (var form = new BinFixture())
        {
            ShowOffScreen(form);
            Set(type, form, "binService", new DACrux.SEMDMS.RO.GdsEtResult(bsl));
            var txtFactory = (TextBox)Get(type, form, "txtFactory");
            var cboDevice = (ComboBox)Get(type, form, "cboDevice");
            var cboProgram = (ComboBox)Get(type, form, "cboTestProgram");
            var txtYms = (TextBox)Get(type, form, "txtYmsProgram");
            var grid = (DataGridView)Get(type, form, "dgvBin");
            var btnSave = (Button)Get(type, form, "btnSave");
            var status = (Label)Get(type, form, "lblStatus");
            txtFactory.Text = Factory;

            // 1) 구성
            string headers = string.Join(",", grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).Select(c => c.HeaderText).ToArray());
            bool hidden = new[] { "colDie", "colPass", "colFail", "colFiles", "colState" }.All(n => !grid.Columns[n].Visible);
            Check("구성: Factory / YMS Program 읽기 전용, 콤보 목록만, 편집은 코드로, [저장] 굵게, Copy 없음, 보이는 열 머리글 / Die · Pass · Fail · 파일 · 상태 숨김(2026-10-22)",
                txtFactory.ReadOnly && txtYms.ReadOnly && cboDevice.DropDownStyle == ComboBoxStyle.DropDownList && cboProgram.DropDownStyle == ComboBoxStyle.DropDownList
                && grid.EditMode == DataGridViewEditMode.EditProgrammatically && btnSave.Font.Bold && !HasField(type, "btnCopy")
                && headers == "Bin,이름,양품,색,설명,Software Bin" && hidden && form.Text == "E-Test Bin 설정", headers);
            Invoke(type, form, "btnSearch_Click");
            Check("Device 없이 조회 -> 고르라는 안내", form.LastNotice.Contains("Factory / Device / 공정을 고르세요"), form.LastNotice);

            // 2) Device 목록 -> 시험 Device 고르기 = 바로 조회
            CallRet(type, form, "LoadPrograms");
            Check("Device 목록 = READY 파일이 있는 Device (" + Device + " 있음)", cboDevice.Items.Contains(Device), Items(cboDevice));
            if (cboDevice.Text == Device) CallRet(type, form, "Search"); // 첫 Device 가 시험 Device 면 콤보 값이 그대로라 화면 열기(OnLoad)와 같이 바로 조회
            else SelectItem(cboDevice, Device);
            DataGridViewRow b1 = BinRow(grid, "1"), b5 = BinRow(grid, "5");
            Check("Device 고르기 -> 조회: 공정 TSVET / YMS Program / Bin 1 · 5 / 분포 열 / 상태 빈 값",
                cboProgram.Text == "TSVET" && txtYms.Text == Program && grid.Rows.Count == 2 && Cell(b1, "colName") == "HW1 SW1" && Checked(b1, "colGood")
                && Cell(b1, "colSoft") == "1" && Cell(b1, "colDie") == "20" && Cell(b1, "colPass") == "20" && Cell(b1, "colFail") == "0" && Cell(b1, "colFiles") == "1"
                && Cell(b1, "colState") == "" && !Checked(b5, "colGood") && Cell(b5, "colSoft") == "5001" && Cell(b5, "colFail") == "4" && Cell(b5, "colState") == ""
                && status.Text.StartsWith("조회 완료", StringComparison.Ordinal), status.Text);
            Check("색 칸 배경 = 저장된 색 (#00C000 / #FFD700)", b1.Cells["colColor"].Style.BackColor == ColorTranslator.FromHtml("#00C000")
                && b5.Cells["colColor"].Style.BackColor == ColorTranslator.FromHtml("#FFD700"), Cell(b1, "colColor") + " / " + Cell(b5, "colColor"));

            // 3) 편집
            InvokeCell(type, form, "dgvBin_CellDoubleClick", grid, b1.Index, grid.Columns["colDie"].Index);
            bool dieEdit = grid.IsCurrentCellInEditMode;
            InvokeCell(type, form, "dgvBin_CellDoubleClick", grid, b1.Index, grid.Columns["colBin"].Index);
            bool binEdit = grid.IsCurrentCellInEditMode;
            Check("읽기 칸(Die / 기존 행 Bin) 더블클릭 -> 편집 안 됨", !dieEdit && !binEdit, "Die " + dieEdit + " / Bin " + binEdit);
            string oldDesc5 = Cell(b5, "colDesc");
            EditCell(type, form, grid, b5, "colName", "FAIL FIVE");
            InvokeCell(type, form, "dgvBin_CellContentClick", grid, b5.Index, grid.Columns["colGood"].Index);
            form.NextColor = Color.FromArgb(0x12, 0x34, 0x56);
            InvokeCell(type, form, "dgvBin_CellDoubleClick", grid, b5.Index, grid.Columns["colColor"].Index);
            Check("Bin 5 수정: 이름 노란색 / 양품 체크 -> '파일 결과와 다름' / 색 창(지금 색에서 시작) -> #123456 굵은 글자 / 행 머리글 *",
                Cell(b5, "colName") == "FAIL FIVE" && b5.Cells["colName"].Style.BackColor == Color.LightYellow && Checked(b5, "colGood")
                && Cell(b5, "colState") == "파일 결과와 다름" && Cell(b5, "colColor") == "#123456" && b5.Cells["colColor"].Style.Font != null
                && b5.Cells["colColor"].Style.Font.Bold && Convert.ToString(b5.HeaderCell.Value) == "*" && form.LastAskedColor == ColorTranslator.FromHtml("#FFD700"),
                Cell(b5, "colState") + " / " + Cell(b5, "colColor") + " / 색 창 시작 " + ColorTranslator.ToHtml(form.LastAskedColor));
            form.NextColor = Color.Empty;
            SelectOnly(grid, b5.Cells["colColor"]);
            Key(type, form, "dgvBin_KeyDown", Keys.F2);
            Check("색 칸 F2 -> 색 창 / 취소하면 그대로", form.ColorAsks == 2 && Cell(b5, "colColor") == "#123456", "색 창 " + form.ColorAsks);

            // 4) [Bin 추가]: 같은 번호 / 설명 길이 오류 -> 저장 막음
            Invoke(type, form, "btnAdd_Click");
            DataGridViewRow nr = grid.Rows[grid.Rows.Count - 1];
            bool editingBin = grid.IsCurrentCellInEditMode && grid.CurrentCell.ColumnIndex == grid.Columns["colBin"].Index;
            grid.EditingControl.Text = "5";
            grid.EndEdit();
            Application.DoEvents();
            string dupState = Cell(nr, "colState");
            bool dupRed = nr.Cells["colBin"].Style.BackColor == Color.MistyRose;
            Invoke(type, form, "btnSave_Click");
            Check("Bin 추가: 새 행 Bin 칸 편집 시작 / 5 입력 -> '이미 있습니다' 붉은 칸 / 저장 막음(DB 그대로)",
                editingBin && dupState.Contains("이미 있습니다") && dupRed && form.LastNotice.Contains("입력을 확인하세요") && DbBin("5")["BIN_NAME"].ToString() == "HW5 SW5001",
                dupState + " / " + form.LastNotice.Split('\r')[0]);
            EditCell(type, form, grid, nr, "colBin", "07");
            EditCell(type, form, grid, nr, "colName", "MANUAL 7");
            EditCell(type, form, grid, nr, "colDesc", new string('가', 34));
            string longState = Cell(nr, "colState");
            bool longRed = nr.Cells["colDesc"].Style.BackColor == Color.MistyRose;
            EditCell(type, form, grid, nr, "colDesc", "수동 Bin");
            Check("설명 한글 34자(102바이트) -> 붉은 칸 / 고치면 '새 Bin'", longState.Contains("100바이트") && longRed && Cell(nr, "colState") == "새 Bin", longState);

            // 5) [저장] -> 수율 다시 계산 질문 -> 예
            int dirty = Dirty(type, form);
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            DataGridViewRow b5a = BinRow(grid, "5"), b7 = BinRow(grid, "7");
            DataRow d5 = DbBin("5"), d7 = DbBin("7");
            double yield100 = Yield(seq);
            Check("저장: 수정 2행 -> 수율 질문 예 -> 100 / Bin 5 = FAIL FIVE · 양품 · #123456 · 설명 그대로 / Bin 7 = 파일에 없음 / 다시 조회 뒤 수정 0",
                dirty == 2 && form.LastQuestion.Contains("YMS 수율을 다시 계산") && yield100 == 100 && Cell(b5a, "colName") == "FAIL FIVE" && Checked(b5a, "colGood")
                && d5["HIGH_GEC"].ToString() == "Y" && d5["COLOR"].ToString() == "#123456" && d5["DESCRIPTION"].ToString() == oldDesc5
                && d7["BIN_NAME"].ToString() == "MANUAL 7" && d7["DESCRIPTION"].ToString() == "수동 Bin" && Cell(b7, "colState") == "파일에 없음"
                && status.Text.StartsWith("저장 완료", StringComparison.Ordinal) && status.Text.Contains("수율 다시 계산 1개") && Dirty(type, form) == 0,
                "수정 " + dirty + " / 수율 " + yield100 + " / " + status.Text);
            int questions = form.Questions.Count;
            EditCell(type, form, grid, b7, "colName", "MANUAL SEVEN");
            Invoke(type, form, "btnSave_Click");
            Check("이름만 바꾸면 수율 질문 없음", form.Questions.Count == questions && DbBin("7")["BIN_NAME"].ToString() == "MANUAL SEVEN", status.Text);

            // 6) 저장 안 한 수정: 조회 질문 / 정의 삭제 막기
            EditCell(type, form, grid, BinRow(grid, "1"), "colName", "X1");
            form.Answers.Enqueue(DialogResult.No);
            Invoke(type, form, "btnSearch_Click");
            bool kept = Cell(BinRow(grid, "1"), "colName") == "X1";
            SelectOnly(grid, BinRow(grid, "7").Cells["colName"]);
            Invoke(type, form, "btnDelete_Click");
            string blockNotice = form.LastNotice;
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSearch_Click");
            Check("수정 중 조회 아니요 = 그대로 / 다른 행 정의 삭제는 '먼저 저장' 으로 막음 / 조회 예 = 버림",
                kept && blockNotice.Contains("먼저 [저장]") && DbBin("7") != null && Cell(BinRow(grid, "1"), "colName") == "HW1 SW1", blockNotice);

            // 7) [선택 삭제] + 되돌림
            SelectOnly(grid, BinRow(grid, "7").Cells["colName"]);
            form.Answers.Enqueue(DialogResult.OK);
            Invoke(type, form, "btnDelete_Click");
            Check("Bin 7 삭제: 확인 -> 정의 삭제 / 표에서 빠짐", form.LastQuestion.Contains("Bin 7 정의를 지웁니다") && FindRow(grid, "7") == null && DbBin("7") == null, status.Text);
            InvokeCell(type, form, "dgvBin_CellContentClick", grid, BinRow(grid, "5").Index, grid.Columns["colGood"].Index);
            form.Answers.Enqueue(DialogResult.Yes);
            Invoke(type, form, "btnSave_Click");
            Check("Bin 5 양품 해제 -> 수율 다시 계산 83.33", Yield(seq) == 83.33 && !Checked(BinRow(grid, "5"), "colGood") && DbBin("5")["HIGH_GEC"].ToString() == "N", status.Text);
            int before = grid.Rows.Count;
            Invoke(type, form, "btnAdd_Click");
            grid.CancelEdit();
            grid.EndEdit();
            SelectOnly(grid, grid.Rows[grid.Rows.Count - 1].Cells["colName"]);
            Invoke(type, form, "btnDelete_Click");
            Check("새 행 [선택 삭제] -> 표에서만 뺌", grid.Rows.Count == before && status.Text.StartsWith("표에서 1행을 뺐습니다", StringComparison.Ordinal), status.Text);

            SaveImage(form, args[1]);
            form.Close();
        }

        if (!keep) Cleanup(bsl);
        else Console.WriteLine("KEEP: " + Factory + " / " + Device + " 파일 번호 " + seq + " / YMS Program " + Program);

        if (failures == 0) { Console.WriteLine("ET_BIN_SETUP_FORM_SMOKE=PASS"); return 0; }
        Console.WriteLine("ET_BIN_SETUP_FORM_SMOKE=FAIL (" + failures + ")");
        return 1;
    }

    /// <summary>고객 Spec 표 TSVET 를 시험 Device 로 등록 + Spec 저장하고, 업로드 화면과 같은 순서로 샘플을 저장한다(Slot 7). 반환: 파일 번호</summary>
    private static string Prepare(DACrux.SEMDMS.BSL.GdsEtResult bsl, string path, string specText)
    {
        var spec = DACrux.SEMDMS.ENGUI.EtSpecTable.Parse(specText).Where(r => r.TestProgram == "TSVET").ToList();
        string[] key = { Factory, Device, "TSVET" };
        bsl.RegisterEtParameters(new[] { Factory, Device, "TSVET", "SPEC", "" }, Rows(spec.Select(r => new[] { r.HeaderText, r.Unit, "", "" })), "SMOKE");
        bsl.SaveEtLimit(key, Rows(spec.Select(r => new[] { r.HeaderText, r.Lower, r.Upper, "Y", r.Unit, r.SpecText, "" })), "SMOKE");
        var f = DACrux.SEMDMS.ENGUI.EtResultFileParser.Parse(path);
        var plan = DACrux.SEMDMS.ENGUI.EtUploadPlan.Build(f, bsl.GetEtParameterList(Factory, Device, "TSVET"));
        string seq = bsl.BeginEtFile(new[] { Factory, Device, f.FileName, f.LotId, Slot, "TSVET", f.TestTime, "", f.SheetName,
            f.DieCount.ToString(CultureInfo.InvariantCulture), plan.Matched.Count.ToString(CultureInfo.InvariantCulture), "SMOKE",
            f.Customer, f.FileDevice, f.WaferType, f.WaferRev, f.WaferId, f.FabName,
            f.ItemCount.ToString(CultureInfo.InvariantCulture), plan.RegisteredCount.ToString(CultureInfo.InvariantCulture), plan.Missing.Count.ToString(CultureInfo.InvariantCulture) })
            .Rows[0]["SRC_FILE_SEQ"].ToString();
        bsl.CreateEtDieBatch(seq, f.Dies, "SMOKE");
        foreach (var batch in plan.BuildValueBatches(f, 10000)) bsl.CreateEtValueBatch(seq, batch, "SMOKE");
        DataRow done = bsl.CompleteEtFile(seq, "SMOKE").Rows[0];
        if (done["RESULT"].ToString() != "OK" || done["YMS_STATUS"].ToString() != "DONE")
            throw new InvalidOperationException("준비 업로드 실패: " + done["RESULT"] + " / YMS " + done["YMS_STATUS"] + " " + done["YMS_MESSAGE"] + " (시험 Map 확인)");
        return seq;
    }

    /// <summary>시험 파일(Slot 7, YMS 반영도 같이) -> 남은 YMS Wafer / Lot -> 등록 Parameter(Spec 같이) -> TD 테이블 / Bin 정의 / YMS Program / Product 를 지운다.</summary>
    private static void Cleanup(DACrux.SEMDMS.BSL.GdsEtResult bsl)
    {
        foreach (DataRow dr in bsl.GetEtFileByKey(new[] { Factory, Lot, Slot }).Rows)
            bsl.DeleteEtFile(dr["SRC_FILE_SEQ"].ToString(), "SMOKE");
        var lotDsl = new DACrux.TEST.DSL.TQP_LOT();
        string lot = lotDsl.GetLotSeq(Factory, "TSVET", Lot, Program);
        if (!string.IsNullOrEmpty(lot))
        {
            var pub = new DACrux.SEMDMS.BSL.EtYmsPublisher();
            foreach (DataRow dr in new DACrux.TEST.DSL.TQP_WAFER().GetData("SELECT_WAFER_BY_LOT", null, new[] { lot, Program }).Rows)
                pub.DeleteWafer(dr["WAFER_SEQ"].ToString());
            new DACrux.TEST.DSL.TQP_LOT_SUM().DeleteLot(lot);
            lotDsl.DeleteLot(lot);
        }
        string[] names = bsl.GetEtParameterList(Factory, Device, "TSVET").Rows.Cast<DataRow>().Select(r => r["ITEM_NAME"].ToString()).ToArray();
        if (names.Length > 0) bsl.DeleteEtParameters(new[] { Factory, Device, "TSVET" }, names, "SMOKE");
        var manager = new DACrux.TEST.BSL.TestDataManager();
        foreach (string table in new DACrux.TEST.DSL.TQP_DATA_TABLES().GetTableNames(Factory, Program) ?? new string[0])
            if (!string.IsNullOrEmpty(table)) manager.CheckTableEmpty(table);
        new DACrux.TEST.DSL.TQP_BINDESC().DeleteDataNonQuery("DELETE_PROGRAM_BIN_ALL", null, new[] { Program });
        new DACrux.TEST.DSL.TQP_PROGRAM().DeleteDataNonQuery("DELETE_PROGRAM", null, new[] { Program });
        new DACrux.TEST.DSL.TQP_PRODUCT().UpdateDeleteFlag("Y", Device);
    }

    /// <summary>파일의 YMS Wafer 수율 (반영 안 됐으면 -1)</summary>
    private static double Yield(string seq)
    {
        DataTable state = new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS().GetData("SELECT_ET_YMS_FILE", null, new[] { seq });
        string wafer = state.Rows.Count == 0 ? string.Empty : state.Rows[0]["YMS_WAFER_SEQ"].ToString();
        if (wafer.Length == 0) return -1;
        DataTable sum = new DACrux.SEMDMS.BSL.EtYmsPublisher().GetWaferSummary(wafer);
        return sum.Rows.Count == 0 ? -1 : Convert.ToDouble(sum.Rows[0]["YIELD"], CultureInfo.InvariantCulture);
    }

    /// <summary>DB 의 Bin 정의 1행 (없으면 null)</summary>
    private static DataRow DbBin(string bin)
    {
        return new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS().GetData("SELECT_ET_BIN_DEF", null, new[] { Program }).Rows.Cast<DataRow>()
            .FirstOrDefault(r => Convert.ToString(r["BIN"], CultureInfo.InvariantCulture) == bin);
    }

    private static string[,] Rows(IEnumerable<string[]> rows)
    {
        List<string[]> list = rows.ToList();
        int cols = list.Count == 0 ? 0 : list[0].Length;
        string[,] a = new string[list.Count, cols];
        for (int r = 0; r < list.Count; r++)
            for (int c = 0; c < cols; c++)
                a[r, c] = list[r][c];
        return a;
    }

    private static void ShowOffScreen(Form form)
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-32000, -32000);
        form.ShowInTaskbar = false;
        form.Show();
        Application.DoEvents();
    }

    private static void SaveImage(Form form, string path)
    {
        form.Size = new Size(1120, 520);
        Application.DoEvents();
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(path);
        }
    }

    private static bool HasField(Type type, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) != null;
    }

    /// <summary>저장 안 한 수정 행 수 (화면 DirtyRows)</summary>
    private static int Dirty(Type type, object form)
    {
        return ((List<DataGridViewRow>)CallRet(type, form, "DirtyRows")).Count;
    }

    private static void SelectItem(ComboBox combo, string value)
    {
        for (int i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i].ToString() != value) continue;
            combo.SelectedIndex = i;
            Application.DoEvents();
            return;
        }
        throw new InvalidOperationException("콤보 항목 없음: " + value + " / " + Items(combo));
    }

    private static string Items(ComboBox combo)
    {
        return string.Join(",", combo.Items.Cast<object>().Select(x => x.ToString()).ToArray());
    }

    private static DataGridViewRow FindRow(DataGridView grid, string bin)
    {
        return grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Cell(r, "colBin") == bin);
    }

    private static DataGridViewRow BinRow(DataGridView grid, string bin)
    {
        DataGridViewRow row = FindRow(grid, bin);
        if (row == null) throw new InvalidOperationException("Bin 행 없음: " + bin);
        return row;
    }

    private static string Cell(DataGridViewRow row, string column)
    {
        return row == null ? "(행 없음)" : Convert.ToString(row.Cells[column].Value, CultureInfo.InvariantCulture);
    }

    private static bool Checked(DataGridViewRow row, string column)
    {
        object v = row.Cells[column].Value;
        return v is bool && (bool)v;
    }

    private static void Check(string name, bool ok, string detail)
    {
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static object Get(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    private static void Set(Type type, object instance, string name, object value)
    {
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, value);
    }

    private static void Invoke(Type type, object instance, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { null, EventArgs.Empty });
        Application.DoEvents();
    }

    private static void InvokeCell(Type type, object instance, string method, DataGridView grid, int row, int column)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { grid, new DataGridViewCellEventArgs(column, row) });
        Application.DoEvents();
    }

    /// <summary>다른 선택을 풀고 칸 하나만 지금 칸 + 선택으로 둔다(사용자가 그 칸을 클릭한 것과 같음).</summary>
    private static void SelectOnly(DataGridView grid, DataGridViewCell cell)
    {
        grid.ClearSelection();
        grid.CurrentCell = cell;
        cell.Selected = true;
    }

    private static void Key(Type type, object instance, string method, Keys keys)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, new object[] { null, new KeyEventArgs(keys) });
        Application.DoEvents();
    }

    /// <summary>칸을 더블클릭해 편집을 시작하고 글자를 넣은 뒤 편집을 끝낸다(사용자 입력과 같은 경로).</summary>
    private static void EditCell(Type type, object instance, DataGridView grid, DataGridViewRow row, string column, string text)
    {
        InvokeCell(type, instance, "dgvBin_CellDoubleClick", grid, row.Index, grid.Columns[column].Index);
        if (!grid.IsCurrentCellInEditMode) throw new InvalidOperationException("편집이 시작되지 않음: " + column);
        grid.EditingControl.Text = text;
        grid.EndEdit();
        Application.DoEvents();
    }

    private static object CallRet(Type type, object instance, string method)
    {
        object result = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(instance, null);
        Application.DoEvents();
        return result;
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록과 확인 / 안내 / 색 창만 바꾼다.</summary>
    private sealed class BinFixture : DACrux.SEMDMS.ENGUI.frmEtBinSetup
    {
        public readonly Queue<DialogResult> Answers = new Queue<DialogResult>();
        public readonly List<string> Questions = new List<string>();
        public string LastQuestion = "";
        public string LastNotice = "";
        public Color NextColor = Color.Empty;
        public Color LastAskedColor = Color.Empty;
        public int ColorAsks;

        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }

        protected override Color AskColor(Color current)
        {
            ColorAsks++;
            LastAskedColor = current;
            Console.WriteLine("  [색 창] " + ColorTranslator.ToHtml(current) + " -> " + (NextColor.IsEmpty ? "취소" : ColorTranslator.ToHtml(NextColor)));
            return NextColor;
        }

        protected override DialogResult Ask(string text, MessageBoxButtons buttons)
        {
            LastQuestion = text;
            Questions.Add(text);
            DialogResult answer = Answers.Count > 0 ? Answers.Dequeue() : DialogResult.Yes;
            Console.WriteLine("  [질문] " + text.Split('\r')[0] + " -> " + answer);
            return answer;
        }

        protected override void Notify(string text, MessageBoxIcon icon)
        {
            LastNotice = text;
            Console.WriteLine("  [안내] " + text.Replace("\r\n", " / "));
        }
    }
}
