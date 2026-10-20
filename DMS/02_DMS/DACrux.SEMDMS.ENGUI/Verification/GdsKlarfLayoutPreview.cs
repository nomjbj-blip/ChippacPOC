using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// GDS x KLARF x E-Test 연관 분석 화면 레이아웃 시안 이미지를 만든다.
/// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8 / 15 / 16절
///
/// 실제 KLARF Wafer(로컬 DB, 조회만)와 현재 분석 서버(AnalyzeWafer)로 화면을 그리고,
/// 아직 서버가 만들지 않는 Ready 목록 / E-Test 판정 / 허용오차 표 / Chain 연결 / E-Test 값은 결과 표 계약(설계 10.2 / 16.4절) 모양의 예시 값으로 채운다.
/// 허용오차 표는 실제 서버를 허용오차별로 불러 센 값이다. 예시 값은 레이아웃 확인용이며 판정 결과가 아니다.
/// 인자: [0] 이미지 폴더, [1] STEP_SEQ, [2] Factory, [3] GDS Device
/// </summary>
internal static class GdsKlarfLayoutPreview
{
    private static readonly double[] TolSteps = { 0, 2, 5, 10, 20, 50 };

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int index);

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        string outDir = args[0];
        long stepSeq = long.Parse(args[1]);
        string factory = args[2];
        string device = args[3];
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "PREVIEW";

        var defectBsl = new DACrux.SEMDMS.BSL.DefectMapAnalysis();
        var viewBsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var chainBsl = new DACrux.SEMDMS.BSL.GdsChain();
        var analysisBsl = new DACrux.SEMDMS.BSL.GdsKlarfAnalysis();
        var etBsl = new DACrux.SEMDMS.BSL.GdsEtResult();

        // 실제 분석(Back / 5 µm) + 허용오차별 실제 수
        DataSet real = analysisBsl.AnalyzeWafer(factory, device, stepSeq, "BACK", 5, true, 0, 0, null, null);
        var tol = new DataTable("TOL_CURVE");
        tol.Columns.Add("TOL_UM", typeof(string));
        tol.Columns.Add("IN_CHAIN_DEFECT", typeof(string));
        tol.Columns.Add("HIT_DIE", typeof(string));
        foreach (double t in TolSteps)
        {
            DataSet r = analysisBsl.AnalyzeWafer(factory, device, stepSeq, "BACK", t, true, 0, 0, null, null);
            tol.Rows.Add(t.ToString(CultureInfo.InvariantCulture), r.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"].ToString(),
                r.Tables["DIE"].Rows.Cast<DataRow>().Count(d => Convert.ToInt32(d["IN_CHAIN_COUNT"]) > 0).ToString(CultureInfo.InvariantCulture));
        }
        DataRow info = real.Tables["ANL_INFO"].Rows[0];
        Console.WriteLine("실제 분석: " + info["WAFER_ID"] + " / " + info["STEP_ID"] + " / Defect " + info["DEFECT_COUNT"] + " / Chain 영역 " + info["IN_CHAIN_DEFECT_COUNT"]);
        DataSet sample = Augment(real, tol, stepSeq);

        Type typeA = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        int images = 0;
        using (var form = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis())
        {
            form.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(defectBsl);
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            form.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
            form.EtService = Ro<DACrux.SEMDMS.RO.GdsEtResult>(etBsl);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();

            var wafer = new DACrux.Base.DPWafer();
            wafer.StepSeq = stepSeq.ToString(CultureInfo.InvariantCulture);
            wafer.WaferID = info["WAFER_ID"].ToString();
            wafer.LotID = info["LOT_ID"].ToString();
            wafer.Product = info["PRODUCT"].ToString();
            form.DrawWafer(new[] { wafer });
            Pump(1500);
            if (form.GdsDevice != device) Console.WriteLine("참고: Product 로 찾은 GDS Device = " + form.GdsDevice + " (인자 " + device + ")");
            Application.DoEvents();

            // 1) 분석 전: Wafer 를 받으면 [KLARF] 기능 탭(Step 표 / Defect 영역 / 좌표 보정) (2026-10-19 기능 탭 배치)
            form.ShowReady(SampleReady(form.ReadyData, etBsl, factory, device));
            Pump(300);
            Save(form, Path.Combine(outDir, "A1_KLARF기준_분석전.png"));
            images++;

            // 2) 분석 결과: 처음 결과는 [종합 (Die)] 탭 (Map 범례는 2026-10-20 삭제, 색 구간은 [색 구간 설정] 팝업에서만)
            form.ShowAnalysisResult(sample);
            // 색 기준 콤보는 2026-10-16 숨김(기본 Chain Defect 수)
            Pump(300);
            Save(form, Path.Combine(outDir, "A2_결과_종합Die탭.png"));
            images++;
            // 2-1) [색 구간 설정] 팝업
            form.ShowBandPopup();
            Pump(300);
            Save((Form)Get(typeA, form, "_bandPopup"), Path.Combine(outDir, "A2b_색구간설정_팝업.png"));
            images++;
            ((Form)Get(typeA, form, "_bandPopup")).Hide();
            // 2-2) [GDS] 기능 탭: GDS Map / Chain 면 + Chain 결과
            form.ShowResultTab((TabPage)Get(typeA, form, "tabChain"));
            Pump(300);
            Save(form, Path.Combine(outDir, "A2c_결과_GDS_Chain탭.png"));
            images++;

            // 3) [E-Test] 기능 탭: E-Test Fail + 첫 행 선택
            form.ShowResultTab((TabPage)Get(typeA, form, "tabEtFail"));
            Pump(200);
            var dgvEtFail = (DataGridView)Get(typeA, form, "dgvEtFail");
            if (dgvEtFail.Rows.Count > 0)
                typeA.GetMethod("dgvEtFail_CellClick", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { dgvEtFail, new DataGridViewCellEventArgs(0, 0) });
            Pump(300);
            Save(form, Path.Combine(outDir, "A3_결과_ETestFail탭.png"));
            images++;

            // 4) 허용오차 / E-Test 연결 탭
            form.ShowResultTab((TabPage)Get(typeA, form, "tabTolerance"));
            Pump(300);
            Save(form, Path.Combine(outDir, "A4_결과_허용오차탭.png"));
            images++;
            form.ShowResultTab((TabPage)Get(typeA, form, "tabEtLink"));
            Pump(300);
            Save(form, Path.Combine(outDir, "A5_결과_ETest연결탭.png"));
            images++;

            // 5) [직접 찾기] 창 (E-Test, 실제 E-Test 목록 서비스)
            using (var dlg = new DACrux.SEMDMS.ENGUI.frmGdsKlarfSourceFind())
            {
                dlg.Text = "E-Test 파일 찾기";
                dlg.CompareText = "비교용 KLARF: Lot " + info["LOT_ID"] + " / Wafer " + info["WAFER_ID"] + " / " + info["STEP_ID"];
                dlg.ShowDevice = true;
                dlg.ShowProgram = true;
                dlg.SingleSelect = true;
                dlg.DeviceFilter = "OMM";
                dlg.StartPosition = FormStartPosition.Manual;
                dlg.Location = new Point(-32000, -32000);
                dlg.Show();
                DataTable list = null;
                try { list = etBsl.GetEtFileBrowseList(factory, "OMM", "", ""); } catch (Exception ex) { Console.WriteLine("E-Test 목록 조회 실패: " + ex.Message); }
                var toFind = typeA.GetMethod("ToEtFindTable", BindingFlags.NonPublic | BindingFlags.Static);
                var shown = (DataTable)toFind.Invoke(null, new object[] { list });
                if (shown.Rows.Count == 0)
                {
                    // 로컬 DB 에 E-Test 파일이 없으면 예시 행(레이아웃 확인용)
                    shown.Rows.Add("RH-HS2615-02", "RH-HS2615", "2", "TSVET", "OMM", "10-07-26 11:02", "24", "3", "10-14-26 09:12", "N", "0");
                    shown.Rows.Add("RH-HS2615-02", "RH-HS2615", "2", "COWET", "OMM", "10-09-26 16:20", "24", "1", "10-14-26 09:12", "Y", "0");
                    shown.Rows.Add("U9-HS2635-01", "U9-HS2635", "1", "TSVET", "OMM", "10-14-26 14:51", "73", "9", "10-15-26 14:51", "N", "0");
                }
                dlg.ShowList(shown);
                Pump(300);
                Save(dlg, Path.Combine(outDir, "A6_직접찾기_ETest.png"));
                images++;
            }
        }

        // 6) 화면 B: 역추적(E-Test Fail Chain) -> OpenGL 도면이라 화면에 띄워 캡처한다
        DataRow focus = sample.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().FirstOrDefault(r => r["CLASS4"].ToString() == "BOTH")
            ?? sample.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().FirstOrDefault(r => r["ET_STATUS"].ToString() == "F");
        if (focus != null)
        {
            int x = Convert.ToInt32(focus["XINDEX"]), y = Convert.ToInt32(focus["YINDEX"]);
            string seq = focus["CHAIN_SEQ"].ToString();
            using (var detail = new NexplantQMS.GdsMap.GdsDieAnalysisForm(true))
            {
                detail.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
                detail.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
                detail.StartPosition = FormStartPosition.Manual;
                detail.Location = new Point(30, 30);
                detail.Size = new Size(1400, 900);
                detail.ShowInTaskbar = false;
                detail.ShowErrorDialogs = false;
                detail.TopMost = true;
                detail.Show();
                detail.Activate();
                Application.DoEvents();
                System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
                // 기능 탭 [GDS / KLARF] [E-Test] [종합 (Die)] (2026-10-19, GDS / KLARF 합침 2026-10-20): 역추적으로 열면 [GDS / KLARF], Die 전체로 열면 [종합 (Die)]
                Wait(detail.ShowDieAsync(sample, x, y, seq), 600);
                Pump(1500);
                Capture(detail, Path.Combine(outDir, "B1_상세_역추적_GDS_KLARF탭.png"));
                images++;
                var tabMain = (TabControl)Get(typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm), detail, "tabMain");
                tabMain.SelectedTab = (TabPage)Get(typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm), detail, "tabEt");
                Pump(800);
                Capture(detail, Path.Combine(outDir, "B2_상세_역추적_ETest탭.png"));
                images++;
                Wait(detail.ShowDieAsync(sample, x, y, null), 60);
                Pump(1500);
                Capture(detail, Path.Combine(outDir, "B3_상세_Die전체_종합탭.png"));
                images++;
                tabMain.SelectedTab = (TabPage)Get(typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm), detail, "tabChainDefect");
                Pump(800);
                Capture(detail, Path.Combine(outDir, "B4_상세_Die전체_GDS_KLARF탭.png"));
                images++;
                Console.WriteLine("화면 B: Die (" + x + ", " + y + ") / Chain " + focus["CHAIN_CODE"] + " / 표시 Fail Chain " + detail.FailChainOverlayCount +" / 상태 " + detail.StatusText);
            }
        }

        Console.WriteLine("GDS_KLARF_LAYOUT_PREVIEW=PASS (이미지 " + images + "장)");
        return 0;
    }

    // ---------------------------------------------------------------- 예시 값 (레이아웃 확인용)

    /// <summary>준비 목록 예시: 고른 Step(실제) + 같은 Wafer 예시 Step 1개, E-Test = 실제 파일(있으면) + 예시 COWET, 점검 3줄.</summary>
    private static DataSet SampleReady(DataSet local, DACrux.SEMDMS.BSL.GdsEtResult etBsl, string factory, string device)
    {
        DataSet ds = DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis.NewReadySet();
        ds.Tables["WAFER_KEY"].Rows.Add("RH-HS2615-02", "LOT_CORE_SLOT", "N", "", factory);
        foreach (DataRow r in local.Tables["READY_GDS"].Rows) ds.Tables["READY_GDS"].ImportRow(r);
        if (ds.Tables["READY_GDS"].Rows.Count > 0)
        {
            ds.Tables["READY_GDS"].Rows[0]["CHAIN_LAST_SAVED"] = "2026-10-15 14:30";
            ds.Tables["READY_GDS"].Rows[0]["CACHE_HIT"] = "Y";
        }
        foreach (DataRow r in local.Tables["READY_KLARF"].Rows)
        {
            ds.Tables["READY_KLARF"].ImportRow(r);
            ds.Tables["READY_KLARF"].Rows[ds.Tables["READY_KLARF"].Rows.Count - 1]["PITCH_SOURCE"] = "KLARF";
        }
        ds.Tables["READY_KLARF"].Rows.Add("-1", "OMM_Hybrid_RDL_AOI (예시)", "PFIC_RH-HS2615_RDL", "10-04-26 18:40:12", "42", "76", "FRONT", "MAP_SETUP", "RULE", "Y");   // KLARF 검사 면 FS 고정 (2026-10-20)
        // E-Test 목록은 분석 GDS Device 와 같은 Device 파일만 (2026-10-20). 실제 파일이 없으면 예시 행, Step 콤보용으로 같은 Step 재측정 1개 + COWET 1개를 더한다
        DataRow file = null;
        try
        {
            DataTable list = etBsl.GetEtFileBrowseList(factory, device, "", "");
            file = list.Rows.Cast<DataRow>().FirstOrDefault();
        }
        catch (Exception ex) { Console.WriteLine("E-Test 목록 조회 실패: " + ex.Message); }
        if (file != null)
            ds.Tables["READY_ET"].Rows.Add(file["SRC_FILE_SEQ"].ToString(), file["TEST_PROGRAM"].ToString(), file["WAFER_ID"].ToString(), device,
                Text(file, "TEST_TIME"), Text(file, "DIE_COUNT"), Text(file, "FAIL_DIE_COUNT"), Text(file, "JUDGE_TIME"), Text(file, "SPEC_CHANGED"), "5", "2", "Y", "RULE", "Y");
        else
            ds.Tables["READY_ET"].Rows.Add("0", "TSVET", "RH-HS2615-02", device, "2026-10-07 11:02:00 (예시)", "76", "3", "2026-10-14 09:12:00", "N", "5", "2", "Y", "RULE", "Y");
        ds.Tables["READY_ET"].Rows.Add("-2", "COWET", "RH-HS2615-02", device, "2026-10-06 16:20:00 (예시)", "76", "1", "2026-10-14 09:12:00", "Y", "5", "2", "Y", "RULE", "N");
        ds.Tables["READY_ET"].Rows.Add("-3", "TSVET", "RH-HS2615-02", device, "2026-10-05 09:30:00 (예시)", "76", "4", "2026-10-14 09:12:00", "N", "5", "2", "Y", "RULE", "N");
        ds.Tables["READY_CHECK"].Rows.Add("ET_LINK", "주의", "E-Test Chain 이름 2개가 GDS 와 연결되지 않음 (DC_P9: GDS 에 없음 / BR_N3: 이름 중복)");
        ds.Tables["READY_CHECK"].Rows.Add("PITCH", "정보", "RDL_AOI Step 은 KLARF 원본 Pitch 가 없어 Map Setup Pitch 사용");
        ds.Tables["READY_CHECK"].Rows.Add("KEY_RULE", "정보", "Wafer Key 규칙 잠정 (KLARF Lot 핵심 + Slot) - 고객 협의 후 확정");
        return ds;
    }

    /// <summary>
    /// 실제 분석 결과에 통합 분석 결과 계약(설계 10.2절) 열을 예시 값으로 붙인다.
    /// E-Test 판정: Defect 가 걸린 (Die, Chain) 은 약 70% Fail, 그 밖은 드물게 Fail. 결정적(같은 입력이면 같은 값)이다.
    /// </summary>
    private static DataSet Augment(DataSet real, DataTable tolCurve, long stepSeq)
    {
        DataSet ds = real.Copy();
        DataTable die = ds.Tables["DIE"], chain = ds.Tables["CHAIN"], dc = ds.Tables["DIE_CHAIN"], defect = ds.Tables["DEFECT"];
        DataRow info = ds.Tables["ANL_INFO"].Rows[0];
        string stepId = info["STEP_ID"].ToString();
        AddCols(ds.Tables["ANL_INFO"], "WAFER_KEY", "STEP_SEQS", "ET_WAFER_ID", "ET_TEST_PROGRAM", "ET_SPEC_CHANGED", "PITCH_SOURCE");
        info["WAFER_KEY"] = "RH-HS2615-02";
        info["STEP_SEQS"] = stepSeq.ToString(CultureInfo.InvariantCulture);
        info["ET_WAFER_ID"] = "RH-HS2615-02";
        info["ET_TEST_PROGRAM"] = "TSVET";
        info["ET_SPEC_CHANGED"] = "N";
        info["PITCH_SOURCE"] = "KLARF";
        AddCols(die, "SYS_X", "SYS_Y", "ET_STATE", "ET_FAIL_CHAIN_COUNT", "BOTH_CHAIN_COUNT");
        AddCols(chain, "ET_LINK_STATUS", "ET_PARA_COUNT", "DIE_JUDGED", "DIE_DEFECT", "DIE_FAIL", "DIE_BOTH", "FAIL_RATE_DEFECT", "FAIL_RATE_CLEAN");
        AddCols(dc, "MIN_GAP", "NEAR_DEFECT_ID", "NEAR_STEP_ID", "ET_STATUS", "ET_PARA_COUNT", "ET_FAIL_COUNT", "ET_FIRST_FAIL_ITEM", "ET_FIRST_FAIL_VALUE", "CLASS4");
        AddCols(defect, "STEP_SEQ", "STEP_ID", "GAP", "NEAR_CHAIN_SEQ", "ROUGHBINNUMBER", "LARGE");

        foreach (DataRow d in defect.Rows)
        {
            d["STEP_SEQ"] = stepSeq.ToString(CultureInfo.InvariantCulture);
            d["STEP_ID"] = stepId;
            d["GAP"] = d["IN_CHAIN"].ToString() == "Y" ? "0.0" : "";
            d["ROUGHBINNUMBER"] = Convert.ToInt64(d["DEFECTID"]) % 2 == 0 ? "53" : "54";
            d["LARGE"] = Convert.ToDouble(d["XSIZE"]) >= 1000 || Convert.ToDouble(d["YSIZE"]) >= 1000 ? "Y" : "";
        }

        List<DataRow> chains = chain.Rows.Cast<DataRow>().ToList();
        var linked = new HashSet<string>(chains.Take(Math.Max(1, chains.Count - 1)).Select(c => c["CHAIN_SEQ"].ToString()));
        var etValue = new DataTable("ET_VALUE");
        foreach (string c in new[] { "XINDEX", "YINDEX", "CHAIN_SEQ", "ITEM_NAME", "HEADER_TEXT", "CHAIN_TEXT", "GDS_CHAIN_NAME", "MEAS_VALUE", "LOWER_LIMIT", "UPPER_LIMIT", "JUDGE_FLAG" })
            etValue.Columns.Add(c, typeof(string));

        foreach (DataRow d in die.Rows)
        {
            int x = Convert.ToInt32(d["XINDEX"]), y = Convert.ToInt32(d["YINDEX"]);
            bool tested = Hash(x, y, 0) % 19 != 0;
            d["SYS_X"] = x.ToString(CultureInfo.InvariantCulture);
            d["SYS_Y"] = y.ToString(CultureInfo.InvariantCulture);
            int fails = 0, both = 0;
            foreach (DataRow c in chains)
            {
                string seq = c["CHAIN_SEQ"].ToString();
                DataRow r = dc.Rows.Cast<DataRow>().FirstOrDefault(q => Convert.ToInt32(q["XINDEX"]) == x && Convert.ToInt32(q["YINDEX"]) == y && q["CHAIN_SEQ"].ToString() == seq);
                if (!linked.Contains(seq))
                {
                    if (r != null) { r["CLASS4"] = "NA"; r["MIN_GAP"] = "0.0"; }
                    continue;
                }
                int h = Hash(x, y, Convert.ToInt32(seq) + 1);
                // 예시: Defect 가 걸린 칸은 Fail(일치 분류가 보이도록), 나머지는 드물게 Fail
                bool fail = tested && (r != null ? Convert.ToInt32(r["DEFECT_COUNT"]) > 0 || h % 10 < 8 : h % 41 == 0);
                int para = 2 + h % 3;
                int failCount = fail ? 1 + h % para : 0;
                if (tested)
                    for (int i = 1; i <= para; i++)
                    {
                        bool f = fail && i <= failCount;
                        etValue.Rows.Add(x.ToString(CultureInfo.InvariantCulture), y.ToString(CultureInfo.InvariantCulture), seq,
                            "RES_" + c["CHAIN_NAME"] + "_" + i, "Resistance_iforce=1mA@" + c["CHAIN_NAME"] + (i == 2 ? ":" + c["CHAIN_NAME"] + "_R" : ""),
                            i == 2 ? c["CHAIN_NAME"] + " : " + c["CHAIN_NAME"] + "_R" : c["CHAIN_NAME"].ToString(), c["CHAIN_NAME"].ToString(),
                            f ? (h % 2 == 0 ? "9.9E+05" : "-") : (12 + (h + i * 7) % 15).ToString("0.0", CultureInfo.InvariantCulture), "5", "30", f ? "F" : "P");
                    }
                if (r == null && !fail) continue;
                if (r == null)
                {
                    r = dc.NewRow();
                    r["XINDEX"] = x;
                    r["YINDEX"] = y;
                    r["CHAIN_SEQ"] = c["CHAIN_SEQ"];
                    r["CHAIN_CODE"] = c["CHAIN_CODE"];
                    r["DEFECT_COUNT"] = 0;
                    r["DEFECT_IDS"] = "";
                    dc.Rows.Add(r);
                }
                int defects = Convert.ToInt32(r["DEFECT_COUNT"]);
                r["ET_STATUS"] = !tested ? "" : fail ? "F" : "P";
                r["ET_PARA_COUNT"] = tested ? para.ToString(CultureInfo.InvariantCulture) : "";
                r["ET_FAIL_COUNT"] = tested ? failCount.ToString(CultureInfo.InvariantCulture) : "";
                r["ET_FIRST_FAIL_ITEM"] = fail ? "RES_" + c["CHAIN_NAME"] + "_1" : "";
                r["ET_FIRST_FAIL_VALUE"] = fail ? (h % 2 == 0 ? "9.9E+05" : "-") : "";
                r["CLASS4"] = !tested ? "NA" : fail && defects > 0 ? "BOTH" : fail ? "FAIL" : defects > 0 ? "DEFECT" : "OK";
                r["MIN_GAP"] = defects > 0 ? "0.0" : h % 2 == 0 ? (6 + h % 40).ToString("0.0", CultureInfo.InvariantCulture) : "";
                r["NEAR_STEP_ID"] = r["MIN_GAP"].ToString().Length > 0 ? stepId : "";
                if (fail) fails++;
                if (fail && defects > 0) both++;
            }
            d["ET_STATE"] = !tested ? "" : fails > 0 ? "F" : "P";
            d["ET_FAIL_CHAIN_COUNT"] = tested ? fails.ToString(CultureInfo.InvariantCulture) : "";
            d["BOTH_CHAIN_COUNT"] = tested ? both.ToString(CultureInfo.InvariantCulture) : "";
        }
        ds.Tables.Add(etValue);

        foreach (DataRow c in chains)
        {
            string seq = c["CHAIN_SEQ"].ToString();
            bool isLinked = linked.Contains(seq);
            c["ET_LINK_STATUS"] = isLinked ? "LINKED" : "NONE";
            if (!isLinked) continue;
            List<DataRow> judged = die.Rows.Cast<DataRow>().Where(d => d["ET_STATE"].ToString().Length > 0).ToList();
            Func<DataRow, DataRow> cell = d => dc.Rows.Cast<DataRow>().FirstOrDefault(q => q["CHAIN_SEQ"].ToString() == seq
                && Convert.ToInt32(q["XINDEX"]) == Convert.ToInt32(d["XINDEX"]) && Convert.ToInt32(q["YINDEX"]) == Convert.ToInt32(d["YINDEX"]));
            int dd = judged.Count(d => { DataRow q = cell(d); return q != null && Convert.ToInt32(q["DEFECT_COUNT"]) > 0; });
            int fd = judged.Count(d => { DataRow q = cell(d); return q != null && q["ET_STATUS"].ToString() == "F"; });
            int bd = judged.Count(d => { DataRow q = cell(d); return q != null && q["CLASS4"].ToString() == "BOTH"; });
            c["ET_PARA_COUNT"] = "3";
            c["DIE_JUDGED"] = judged.Count.ToString(CultureInfo.InvariantCulture);
            c["DIE_DEFECT"] = dd.ToString(CultureInfo.InvariantCulture);
            c["DIE_FAIL"] = fd.ToString(CultureInfo.InvariantCulture);
            c["DIE_BOTH"] = bd.ToString(CultureInfo.InvariantCulture);
            c["FAIL_RATE_DEFECT"] = dd == 0 ? "" : (bd / (double)dd).ToString("0.###", CultureInfo.InvariantCulture);
            c["FAIL_RATE_CLEAN"] = judged.Count - dd == 0 ? "" : ((fd - bd) / (double)(judged.Count - dd)).ToString("0.###", CultureInfo.InvariantCulture);
        }

        tolCurve.TableName = "TOL_CURVE";
        ds.Tables.Add(tolCurve.Copy());

        var link = new DataTable("ET_LINK");
        foreach (string c in new[] { "GDS_CHAIN_KEY", "CHAIN_SEQ", "CHAIN_CODE", "STATUS", "PARA_COUNT", "CHAIN2_PARA_COUNT" }) link.Columns.Add(c, typeof(string));
        int n = 0;
        foreach (DataRow c in chains.Where(c => linked.Contains(c["CHAIN_SEQ"].ToString())))
            link.Rows.Add(c["CHAIN_NAME"].ToString().ToUpperInvariant(), c["CHAIN_SEQ"].ToString(), c["CHAIN_CODE"].ToString(), "LINKED", "3", n++ == 0 ? "1" : "0");
        link.Rows.Add("DC_P9", "", "", "NOT_IN_GDS", "2", "0");
        link.Rows.Add("BR_N3", "", "", "DUPLICATE", "1", "0");
        ds.Tables.Add(link);

        var defectChain = new DataTable("DEFECT_CHAIN");
        foreach (string c in new[] { "STEP_SEQ", "DEFECTID", "CHAIN_SEQ", "GAP", "NEAR_PLACED_ELEMENT_ID" }) defectChain.Columns.Add(c, typeof(string));
        if (ds.Tables.Contains("DEFECT_HIT"))
            foreach (var g in ds.Tables["DEFECT_HIT"].Rows.Cast<DataRow>().GroupBy(r => r["DEFECTID"] + "|" + r["CHAIN_SEQ"]))
            {
                DataRow first = g.First();
                defectChain.Rows.Add(stepSeq.ToString(CultureInfo.InvariantCulture), first["DEFECTID"].ToString(), first["CHAIN_SEQ"].ToString(), "0.0", first["PLACED_ELEMENT_ID"].ToString());
            }
        // 허용오차 밖 가까운 Defect 예시(역추적 목록 확인용): Chain 밖 Defect 를 같은 Die 의 첫 Chain 에 6 ~ 45 µm 로 붙인다
        foreach (DataRow d in defect.Rows.Cast<DataRow>().Where(r => r["IN_CHAIN"].ToString() != "Y"))
        {
            int h = Hash(Convert.ToInt32(d["XINDEX"]), Convert.ToInt32(d["YINDEX"]), Convert.ToInt32(Convert.ToInt64(d["DEFECTID"]) % 100000));
            DataRow c = chains[h % chains.Count];
            defectChain.Rows.Add(stepSeq.ToString(CultureInfo.InvariantCulture), d["DEFECTID"].ToString(), c["CHAIN_SEQ"].ToString(),
                (6 + h % 40).ToString("0.0", CultureInfo.InvariantCulture), "");
        }
        ds.Tables.Add(defectChain);
        return ds;
    }

    private static int Hash(int x, int y, int z)
    {
        unchecked { return ((x * 73856093) ^ (y * 19349663) ^ (z * 83492791)) & 0x7fffffff; }
    }

    private static void AddCols(DataTable t, params string[] names)
    {
        foreach (string n in names) if (!t.Columns.Contains(n)) t.Columns.Add(n, typeof(string));
    }

    private static string Text(DataRow r, string c)
    {
        return r.Table.Columns.Contains(c) && r[c] != DBNull.Value ? r[c].ToString() : "";
    }

    // ---------------------------------------------------------------- 화면 / 캡처 도우미

    private static void Save(Form form, string png)
    {
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(png, System.Drawing.Imaging.ImageFormat.Png);
        }
        Console.WriteLine("이미지: " + png);
    }

    /// <summary>
    /// OpenGL 도면은 DrawToBitmap 으로 그려지지 않아 화면에서 복사한다(창이 맨 위에 보이는 상태).
    /// 화면 배율(125 / 150%)이면 화면 복사는 실제 픽셀 좌표로 동작하므로 창 범위에 배율을 곱해 복사한다.
    /// </summary>
    private static void Capture(Form form, string png)
    {
        form.Activate();
        Application.DoEvents();
        double scale = ScreenScale();
        Rectangle r = form.Bounds;
        var physical = new Rectangle((int)Math.Round(r.X * scale), (int)Math.Round(r.Y * scale), (int)Math.Round(r.Width * scale), (int)Math.Round(r.Height * scale));
        using (var bmp = new Bitmap(physical.Width, physical.Height))
        {
            using (Graphics g = Graphics.FromImage(bmp)) g.CopyFromScreen(physical.Location, Point.Empty, physical.Size);
            bmp.Save(png, System.Drawing.Imaging.ImageFormat.Png);
        }
        Console.WriteLine("이미지: " + png + " (화면 배율 " + scale.ToString("0.##") + ")");
    }

    /// <summary>실제 화면 폭(DESKTOPHORZRES) / 프로그램이 보는 폭(HORZRES).</summary>
    private static double ScreenScale()
    {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
            IntPtr hdc = g.GetHdc();
            try
            {
                int logical = GetDeviceCaps(hdc, 8);      // HORZRES
                int physical = GetDeviceCaps(hdc, 118);   // DESKTOPHORZRES
                return logical > 0 ? Math.Max(1.0, physical / (double)logical) : 1.0;
            }
            finally { g.ReleaseHdc(hdc); }
        }
    }

    /// <summary>RO 생성자(원격 URL 읽기)를 거치지 않고 원격 연결 필드만 BSL 로 채운다.</summary>
    private static T Ro<T>(object bsl) where T : class
    {
        var ro = (T)FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return ro;
    }

    private static void Pump(int milliseconds)
    {
        var w = Stopwatch.StartNew();
        while (w.ElapsedMilliseconds < milliseconds) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    private static void Wait(Task task, int seconds)
    {
        var w = Stopwatch.StartNew();
        while (!task.IsCompleted)
        {
            if (w.Elapsed.TotalSeconds > seconds) throw new TimeoutException("화면 B 대기 시간 초과");
            Application.DoEvents();
            System.Threading.Thread.Sleep(20);
        }
        if (task.IsFaulted) throw task.Exception.InnerException;
    }

    private static object Get(Type type, object target, string name)
    {
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null) throw new MissingFieldException(type.Name, name);
        return f.GetValue(target);
    }
}
