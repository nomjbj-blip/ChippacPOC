using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// GDS x KLARF 연관 분석 화면(화면 A, frmGdsKlarfAnalysis)과 Die 상세 화면(화면 B, NexplantQMS.GdsMap.GdsDieAnalysisForm)을
/// 화면 밖에서 실제로 띄워 Wafer 그리기 -> [분석] -> 구간 색 / 목록 -> Die 선택 -> 상세(도면 + Chain + Defect) 흐름을 검사한다.
/// Service(Remoting) 대신 RO 의 원격 연결 필드(m_OBJ)를 같은 프로세스 BSL 로 바꿔 화면 -> RO -> BSL -> DSL -> 로컬 DB 를 실제로 거친다. DB 는 조회만 한다.
/// 인자: [0] 화면 A 이미지(png), [1] STEP_SEQ, [2] Factory, [3] GDS Device
/// </summary>
internal static class GdsKlarfAnalysisFormSmoke
{
    private static int failures;
    private static int checks;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        string png = args[0];
        long stepSeq = long.Parse(args[1]);
        string factory = args[2];
        string device = args[3];
        DACrux.Base.GlobalVariable.Factory = factory;
        DACrux.Base.GlobalVariable.UserID = "SMOKE";

        var defectBsl = new DACrux.SEMDMS.BSL.DefectMapAnalysis();
        var viewBsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var chainBsl = new DACrux.SEMDMS.BSL.GdsChain();
        var analysisBsl = new DACrux.SEMDMS.BSL.GdsKlarfAnalysis();
        DataSet expected = analysisBsl.AnalyzeWafer(factory, device, stepSeq, "BACK", 5, true, 0, 0, null, null);
        DataRow expectedInfo = expected.Tables["ANL_INFO"].Rows[0];
        int expectedInChain = Convert.ToInt32(expectedInfo["IN_CHAIN_DEFECT_COUNT"]);
        DataRow topDie = expected.Tables["DIE"].Rows[0];
        int topX = Convert.ToInt32(topDie["XINDEX"]), topY = Convert.ToInt32(topDie["YINDEX"]), topN = Convert.ToInt32(topDie["IN_CHAIN_COUNT"]);
        Console.WriteLine("BSL 기준: Defect " + expectedInfo["DEFECT_COUNT"] + " / Chain 영역 " + expectedInChain + " / 첫 Die (" + topX + ", " + topY + ") N=" + topN);

        DataSet result;
        int dieChains = expected.Tables["DIE_CHAIN"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY);
        Type typeA = typeof(DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis);
        using (var form = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis())
        {
            form.DefectService = Ro<DACrux.SEMDMS.RO.DefectMapAnalysis>(defectBsl);
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            form.AnalysisService = new DACrux.SEMDMS.RO.GdsKlarfAnalysis(analysisBsl);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1400, 900);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();

            // ---- 1) Wafer 받기 (왼쪽 DMSearch [Search] 와 같은 입구) ----
            var wafer = new DACrux.Base.DPWafer();
            wafer.StepSeq = stepSeq.ToString();
            wafer.WaferID = expectedInfo["WAFER_ID"].ToString();
            wafer.LotID = expectedInfo["LOT_ID"].ToString();
            wafer.Product = expectedInfo["PRODUCT"].ToString();
            form.DrawWafer(new[] { wafer });
            Pump(1500);
            var map = (DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap)Get(typeA, form, "map");
            var cboDevice = (ComboBox)Get(typeA, form, "cboDevice");
            var btnChain = (Button)Get(typeA, form, "btnChain");
            var btnClass = (Button)Get(typeA, form, "btnClass");
            var status = (Label)Get(typeA, form, "lblStatus");
            Check("Wafer Map Defect 수", map.Defects.Count == Convert.ToInt32(expectedInfo["DEFECT_COUNT"]), map.Defects.Count.ToString());
            Check("Map Die 격자", map.Dies.Count > 0, "Die " + map.Dies.Count);
            if (!cboDevice.Items.Contains(device)) throw new InvalidOperationException("Device 목록에 " + device + " 없음");
            cboDevice.SelectedItem = device;
            Application.DoEvents();
            Check("GDS Device 선택 / Chain 메뉴(READY 전체)", (string)cboDevice.SelectedItem == device && btnChain.Text.StartsWith("Chain: 전체("), btnChain.Text);
            Check("Class 메뉴(전체)", btnClass.Text.StartsWith("Class: 전체("), btnClass.Text);
            Check("분석 전 안내", status.Text.Contains("[분석]"), status.Text);

            // ---- 2) [분석] (Back side / 5 µm / 크기 반영 = 기본값) ----
            result = form.AnalyzeNow();
            DataRow info = result.Tables["ANL_INFO"].Rows[0];
            Check("분석 결과 = BSL 직접 호출", Convert.ToInt32(info["IN_CHAIN_DEFECT_COUNT"]) == expectedInChain && info["VIEW_SIDE"].ToString() == "BACK"
                && Convert.ToDouble(info["TOLERANCE_UM"]) == 5, "Chain 영역 " + info["IN_CHAIN_DEFECT_COUNT"] + " / " + info["VIEW_SIDE"]);
            Dictionary<Point, Color> fills = (Dictionary<Point, Color>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieFill");
            Dictionary<Point, int> labels = (Dictionary<Point, int>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieLabel");
            List<DACrux.SEMDMS.ENGUI.GdsKlarfColorBand> bands = form.ColorBands;
            Color topColor = DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.DieColor(Convert.ToInt32(topDie["DEFECT_COUNT"]), topN, 1, bands);
            Check("자동 구간 = Auto(1, 최대 N)", bands.Count == Math.Min(5, Math.Max(1, topN)) && bands[0].From == 1 && bands[bands.Count - 1].To == 999,
                string.Join(" / ", bands.Select(b => b.From + "-" + b.To).ToArray()));
            Check("Die 채움색 = 검사 Die 전체 / 숫자 = Defect Die", fills.Count == result.Tables["DIE"].Rows.Count && labels.Count == Convert.ToInt32(info["DEFECT_DIE_COUNT"]),
                "fill " + fills.Count + " / label " + labels.Count);
            Check("최다 Die 색 = 마지막 구간 색", fills[new Point(topX, topY)].ToArgb() == bands[bands.Count - 1].Color.ToArgb() && topColor.ToArgb() == bands[bands.Count - 1].Color.ToArgb(),
                ColorTranslator.ToHtml(fills[new Point(topX, topY)]));
            DataRow outOnly = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && Convert.ToInt32(r["IN_CHAIN_COUNT"]) == 0);
            DataRow empty = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) == 0);
            if (outOnly != null)
                Check("Chain 밖 Defect 만 있는 Die = 연회색", fills[DiePoint(outOnly)].ToArgb() == DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.BelowThresholdColor.ToArgb(), DiePoint(outOnly).ToString());
            if (empty != null)
                Check("Defect 없는 Die = 흰색", fills[DiePoint(empty)].ToArgb() == Color.White.ToArgb(), DiePoint(empty).ToString());
            var dgvBand = (DataGridView)Get(typeA, form, "dgvBand");
            var dgvDie = (DataGridView)Get(typeA, form, "dgvDie");
            int bandDies = 0;
            foreach (DataGridViewRow r in dgvBand.Rows) bandDies += Convert.ToInt32(r.Cells[3].Value);
            Check("구간 표 = 구간 + 고정 2행 / Die 수 합 = 결과 Die 수", dgvBand.Rows.Count == bands.Count + 2 && bandDies == result.Tables["DIE"].Rows.Count,
                "rows " + dgvBand.Rows.Count + " / dies " + bandDies);
            Check("Die 탭 = Defect Die, 첫 행 = Chain 영역 최다", dgvDie.Rows.Count == Convert.ToInt32(info["DEFECT_DIE_COUNT"])
                && Convert.ToInt32(dgvDie.Rows[0].Cells["Chain 영역"].Value) == topN, dgvDie.Rows.Count + " / " + dgvDie.Rows[0].Cells["Die"].Value);
            Check("상태줄 분석 기준", status.Text.Contains("Chain 영역 " + expectedInChain) && status.Text.Contains("BACK"), status.Text.Replace(Environment.NewLine, " | "));

            // ---- 3) Die 선택 -> Chain / Defect 탭 거르기 ----
            form.SelectDie(topX, topY);
            Application.DoEvents();
            var dgvChain = (DataGridView)Get(typeA, form, "dgvChain");
            var dgvDefect = (DataGridView)Get(typeA, form, "dgvDefect");
            Check("선택 Die 의 Chain 탭 = DIE_CHAIN", dgvChain.Rows.Count == dieChains, dgvChain.Rows.Count + " / " + dieChains);
            Check("선택 Die 의 Defect 탭(Chain 영역) = N", dgvDefect.Rows.Count == topN, dgvDefect.Rows.Count.ToString());
            Check("Map 선택 Die 빨간 테두리 목록", map.SelectedDies.Count == 1 && map.SelectedDies[0] == new Point(topX, topY), map.SelectedDies.Count.ToString());

            // ---- 3-1) 마우스가 Die 위를 지날 때(OnChangeCurrentDie) 상태줄. DACrux.Base.Die 의 == 연산자가 null 을 처리하지 않아 예외가 났던 경로 ----
            DACrux.Base.Die hoverDie = null;
            foreach (DACrux.Base.Die d in map.Dies) { if (d.IndexX == topX && d.IndexY == topY) { hoverDie = d; break; } }
            string hoverError = "";
            try { Invoke(typeA, form, "map_OnChangeCurrentDie", map, hoverDie); }
            catch (TargetInvocationException ex) { hoverError = ex.InnerException.GetType().Name + ": " + ex.InnerException.Message; }
            Check("마우스 Die 이동 -> 상태줄(예외 없음)", hoverError == "" && status.Text.Contains("Die (" + topX + ", " + topY + ") 전체"), hoverError == "" ? status.Text.Replace(Environment.NewLine, " | ") : hoverError);

            // ---- 3-2) 우클릭 메뉴(2026-10-12 요청, 2026-10-13 Center Cross 추가): Free Zoom / Fit Size / Defect Select / Die Select / Center Cross / Detail Analysis 만 ----
            var ctxm = (System.Windows.Forms.Menu)Get(typeof(DACrux.Map.WaferMap), map, "ctxmWaferMap");
            typeof(ContextMenu).GetMethod("OnPopup", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ctxm, new object[] { EventArgs.Empty });
            string menuText = string.Join(" / ", map.VisibleMenuItems().ToArray());
            Check("우클릭 메뉴 = Free Zoom / Fit Size / Defect Select / Die Select / - / Center Cross / - / Detail Analysis (선택 Chip 있으면 사용 가능)",
                menuText == "Free Zoom / Fit Size / Defect Select / Die Select / - / Center Cross / - / Detail Analysis" && map.DetailMenuEnabled, menuText + " / enabled=" + map.DetailMenuEnabled);

            // ---- 3-3) 우클릭하면 커서 아래 Chip 을 먼저 선택한다(메뉴는 끄고 확인: 실제 메뉴는 모달이라 자동 검사를 멈춘다) ----
            DataRow rightDie = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            if (rightDie != null)
            {
                Point rp = DiePoint(rightDie);
                DACrux.Base.Die target = null;
                foreach (DACrux.Base.Die d in map.Dies) { if (d.IndexX == rp.X && d.IndexY == rp.Y) { target = d; break; } }
                PointF sp = map.ToScreen(target.DieCood.X + target.DieCood.Width / 2d - map.OriginX, target.DieCood.Y + target.DieCood.Height / 2d - map.OriginY);
                map.PopupMenu = false;
                MethodInfo move = typeof(Control).GetMethod("OnMouseMove", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo down = typeof(Control).GetMethod("OnMouseDown", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo up = typeof(Control).GetMethod("OnMouseUp", BindingFlags.NonPublic | BindingFlags.Instance);
                move.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.None, 0, (int)sp.X, (int)sp.Y, 0) });
                down.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.Right, 1, (int)sp.X, (int)sp.Y, 0) });
                up.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.Right, 1, (int)sp.X, (int)sp.Y, 0) });
                map.PopupMenu = true;
                Application.DoEvents();
                Check("우클릭 -> 커서 아래 Chip 선택", map.SelectedDies.Count == 1 && map.SelectedDies[0] == rp, rp + " -> " + (map.SelectedDies.Count > 0 ? map.SelectedDies[0].ToString() : "없음"));
                form.SelectDie(topX, topY);
                Application.DoEvents();
            }

            // ---- 3-4) 상세 화면은 1개만(2026-10-12 사용자 확인 요청: 두 개가 뜨던 버그) / 탭 제목 = 상세보기 (X, Y) ----
            // MDI 부모 + 같은 화면 A 를 하나 더 만들어 OpenDetail 을 두 번 부른다. 상세 화면의 Shown(GDS EXE 로드)은 메시지를 돌리기 전에 닫아 실행하지 않는다.
            using (var mdi = new Form())
            {
                mdi.IsMdiContainer = true;
                mdi.StartPosition = FormStartPosition.Manual;
                mdi.Location = new Point(-32000, -32000);
                mdi.ShowInTaskbar = false;
                mdi.Show();
                var a2 = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis();
                a2.MdiParent = mdi;
                typeA.GetField("_result", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(a2, result);
                Invoke(typeA, a2, "OpenDetail", topX, topY);
                Form[] details1 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                string title1 = details1.Length > 0 ? details1[0].Text : "";
                Point second = rightDie != null ? DiePoint(rightDie) : new Point(topX, topY);
                Invoke(typeA, a2, "OpenDetail", second.X, second.Y);
                Form[] details2 = mdi.MdiChildren.Where(f => f is DACrux.SEMDMS.ENGUI.frmGdsDieAnalysis).ToArray();
                string title2 = details2.Length > 0 ? details2[0].Text : "";
                Check("상세 화면 1개만 / 탭 제목 상세보기(좌표) / 다른 Chip 은 같은 화면", details1.Length == 1 && details2.Length == 1 && details1[0] == details2[0]
                    && title1 == "상세보기 (" + topX + ", " + topY + ")" && title2 == "상세보기 (" + second.X + ", " + second.Y + ")",
                    details1.Length + " -> " + details2.Length + " / " + title1 + " -> " + title2);
                foreach (Form f in details2) f.Dispose();
                a2.Dispose();
            }
            Application.DoEvents();
            // ---- 4) 표시 기준 2 -> N=1 Die 는 연회색 ----
            var numThreshold = (NumericUpDown)Get(typeA, form, "numThreshold");
            numThreshold.Value = 2;
            Application.DoEvents();
            fills = (Dictionary<Point, Color>)Get(typeof(DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap), map, "_dieFill");
            DataRow one = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["IN_CHAIN_COUNT"]) == 1);
            if (one != null)
                Check("표시 기준 2: N=1 Die 연회색", fills[DiePoint(one)].ToArgb() == DACrux.SEMDMS.ENGUI.GdsKlarfColorBands.BelowThresholdColor.ToArgb(), DiePoint(one).ToString());
            Check("표시 기준 2: 구간 시작 2", form.ColorBands[0].From == 2, form.ColorBands[0].From.ToString());
            numThreshold.Value = 1;
            Application.DoEvents();

            // ---- 5) 사용자 구간: FROM/TO 수정 -> 자동 해제 ----
            // 실제 조작과 같게: TO 칸 편집 -> 다른 칸을 마우스로 누름(OnMouseDown -> SetCurrentCellAddressCore -> EndEdit -> CellEndEdit).
            // CurrentCell 속성으로 옮기면 처리 순서가 달라 재진입 예외가 재현되지 않는다.
            // 2026-10-12 사용자 실행 중 이 경로에서 Rows.Clear 재진입 예외가 났다(처리 함수 직접 호출로는 재현되지 않음).
            string bandError = "";
            try
            {
                dgvBand.Focus();
                dgvBand.CurrentCell = dgvBand.Rows[0].Cells[1];
                dgvBand.BeginEdit(true);
                if (dgvBand.EditingControl == null) throw new InvalidOperationException("편집 시작 실패");
                dgvBand.EditingControl.Text = "2";
                dgvBand.NotifyCurrentCellDirty(true);
                ClickCell(dgvBand, 0, 1);
                Pump(300);
            }
            catch (Exception ex) { bandError = ex.GetType().Name + ": " + ex.Message; }
            var chkAuto = (CheckBox)Get(typeA, form, "chkAutoBand");
            Check("구간 TO 칸 편집 후 다른 칸 이동 -> 사용자 구간(예외 없음, 자동 해제)", bandError == "" && !chkAuto.Checked && form.ColorBands[0].To == 2
                && System.Convert.ToString(dgvBand.Rows[0].Cells[1].Value) == "2", bandError == "" ? "auto=" + chkAuto.Checked + " to=" + form.ColorBands[0].To : bandError);
            bandError = "";
            try
            {
                dgvBand.CurrentCell = dgvBand.Rows[0].Cells[0];
                dgvBand.BeginEdit(true);
                dgvBand.EditingControl.Text = "abc";
                dgvBand.NotifyCurrentCellDirty(true);
                ClickCell(dgvBand, 1, 1);
                Pump(300);
            }
            catch (Exception ex) { bandError = ex.GetType().Name + ": " + ex.Message; }
            Check("구간 FROM 잘못된 값 -> 되돌림(예외 없음)", bandError == "" && System.Convert.ToString(dgvBand.Rows[0].Cells[0].Value) == form.ColorBands[0].From.ToString(),
                bandError == "" ? "from=" + dgvBand.Rows[0].Cells[0].Value : bandError);
            chkAuto.Checked = true;
            Application.DoEvents();

            // ---- 6) 조건 변경 -> 경고 -> 다시 분석 ----
            var numTol = (NumericUpDown)Get(typeA, form, "numTolerance");
            numTol.Value = 50;
            Application.DoEvents();
            Check("조건 변경 경고", status.Text.Contains("조건 변경됨"), "");
            DataSet result50 = form.AnalyzeNow();
            int in50 = Convert.ToInt32(result50.Tables["ANL_INFO"].Rows[0]["IN_CHAIN_DEFECT_COUNT"]);
            Check("허용오차 50 재분석 >= 5 µm", in50 >= expectedInChain && !status.Text.Contains("조건 변경됨"), expectedInChain + " -> " + in50);
            numTol.Value = 5;
            result = form.AnalyzeNow();
            form.SelectDie(topX, topY);
            Application.DoEvents();

            // ---- 7) 화면 이미지 ----
            using (var bmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                bmp.Save(png, System.Drawing.Imaging.ImageFormat.Png);
            }
            Console.WriteLine("화면 A 이미지: " + png);
        }

        // ---- 8) 화면 B 계약(ENGUI 호스트가 리플렉션으로 찾는 멤버) ----
        Type typeB = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
        Check("화면 B 계약: 생성자(bool) / ShowDie(DataSet,int,int) / DefectImageRequested(Action<long,long>)",
            typeB.GetConstructor(new[] { typeof(bool) }) != null && typeB.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) }) != null
            && typeB.GetProperty("DefectImageRequested") != null && typeB.GetProperty("DefectImageRequested").PropertyType == typeof(Action<long, long>), "");

        // ---- 9) 화면 B: 도면 + Chain + Defect ----
        using (var detail = new NexplantQMS.GdsMap.GdsDieAnalysisForm(true))
        {
            detail.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(viewBsl);
            detail.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(chainBsl);
            detail.StartPosition = FormStartPosition.Manual;
            detail.Location = new Point(-32000, -32000);
            detail.Size = new Size(1400, 900);
            detail.ShowInTaskbar = false;
            detail.ShowErrorDialogs = false;
            detail.Show();
            Application.DoEvents();
            // 콘솔 Smoke 는 Application.Run 이 없어 첫 화면을 닫은 뒤 동기화 문맥이 일반 SynchronizationContext 로 바뀐다(새 화면을 띄워도 그대로).
            // 그러면 await 뒤 처리가 다른 스레드에서 돌아 OpenGL MakeCurrent 가 실패(Error 170)하므로, DMS(메시지 루프 = WinForms 문맥)와 같게 화면을 띄운 뒤 지정한다.
            System.Threading.SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            var watch = Stopwatch.StartNew();
            Wait(detail.ShowDieAsync(result, topX, topY), 600);
            double loadSeconds = watch.Elapsed.TotalSeconds;
            Console.WriteLine("화면 B 첫 표시(도면 + Chain Member 조회): " + loadSeconds.ToString("0.0") + "초 / 상태: " + detail.StatusText);
            Check("화면 B 도면 키", detail.LoadedMapKey == factory + "|" + device + "|BACK|CHAIN", detail.LoadedMapKey);
            int dieDefects = result.Tables["DEFECT"].Rows.Cast<DataRow>().Count(r => Convert.ToInt32(r["XINDEX"]) == topX && Convert.ToInt32(r["YINDEX"]) == topY);
            Check("화면 B Defect 표시 = 선택 Die Defect", detail.DefectMarkerCount == dieDefects, detail.DefectMarkerCount + " / " + dieDefects);
            Check("화면 B Chain 표시(걸린 Chain)", detail.VisibleChainCount == Math.Max(1, dieChains) || (dieChains == 0 && detail.VisibleChainCount > 0), detail.VisibleChainCount + " / " + dieChains);

            // 반전 일치: 걸린 Member 도형(지도에 그린 보기 기준 좌표)이 Defect 사각형 ± 허용오차와 겹쳐야 한다
            Type typeB2 = typeof(NexplantQMS.GdsMap.GdsDieAnalysisForm);
            var gmap = (NexplantQMS.GdsMap.GdsMapControl)Get(typeB2, detail, "map");
            int pairs = 0, overlaps = 0;
            foreach (DataRow hit in result.Tables["DEFECT_HIT"].Rows)
            {
                DataRow d = result.Tables["DEFECT"].Select("DEFECTID = " + hit["DEFECTID"])[0];
                if (Convert.ToInt32(d["XINDEX"]) != topX || Convert.ToInt32(d["YINDEX"]) != topY) continue;
                pairs++;
                IList<NexplantQMS.GdsMap.Chain.ChainTraceElement> el = gmap.GetChainTraceElements(new[] { hit["PLACED_ELEMENT_ID"].ToString() });
                if (el.Count != 1) continue;
                double x = Convert.ToDouble(d["GDS_X"]), y = Convert.ToDouble(d["GDS_Y"]);
                double hw = Convert.ToDouble(d["XSIZE"]) / 2 + 5.001, hh = Convert.ToDouble(d["YSIZE"]) / 2 + 5.001;
                var b = el[0].Bounds;
                if (b.MinX <= x + hw && b.MaxX >= x - hw && b.MinY <= y + hh && b.MaxY >= y - hh) overlaps++;
            }
            Check("화면 B 반전 일치: 걸린 Member 도형 = Defect 위치(보기 기준)", pairs == 0 || overlaps == pairs, overlaps + " / " + pairs);

            // 상세 화면 = 선택 Chip 1개(2026-10-12 요청): Chip 이동 / Wafer 합산 컨트롤이 없고 위쪽에 Chip 좌표 정보가 나온다
            var lblDieInfo = (Label)Get(typeB2, detail, "lblDieInfo");
            Check("화면 B Chip 좌표 정보 / Chip 이동 컨트롤 없음", lblDieInfo.Text.StartsWith("Die (" + topX + ", " + topY + ")  Defect " + dieDefects)
                && typeB2.GetField("cboDie", BindingFlags.NonPublic | BindingFlags.Instance) == null && typeB2.GetField("rdoWafer", BindingFlags.NonPublic | BindingFlags.Instance) == null,
                lblDieInfo.Text);

            DataRow other = result.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["DEFECT_COUNT"]) > 0 && DiePoint(r) != new Point(topX, topY));
            if (other != null)
            {
                watch.Restart();
                Wait(detail.ShowDieAsync(result, DiePoint(other).X, DiePoint(other).Y), 60);
                Check("화면 B Die 변경 = 도면 다시 읽지 않음", watch.Elapsed.TotalSeconds < 5 && detail.DefectMarkerCount == Convert.ToInt32(other["DEFECT_COUNT"]),
                    watch.Elapsed.TotalSeconds.ToString("0.00") + "초 / Defect " + detail.DefectMarkerCount);
            }
            var dgvDefects = (DataGridView)Get(typeB2, detail, "dgvDefects");
            if (dgvDefects.Rows.Count > 0)
            {
                dgvDefects.Rows[0].Selected = true;
                Application.DoEvents();
                Check("화면 B Defect 행 선택 -> 상태줄", ((ToolStripStatusLabel)Get(typeB2, detail, "lblStatus")).Text.StartsWith("Defect #"),
                    ((ToolStripStatusLabel)Get(typeB2, detail, "lblStatus")).Text);
            }
        }

        Console.WriteLine();
        Console.WriteLine("GDS_KLARF_FORM_SMOKE=" + (failures == 0 ? "PASS" : "FAIL") + " (" + (checks - failures) + "/" + checks + ")");
        return failures == 0 ? 0 : 1;
    }

    /// <summary>RO 생성자(원격 URL 읽기)를 거치지 않고 원격 연결 필드만 BSL 로 채운다.</summary>
    private static T Ro<T>(object bsl) where T : class
    {
        var ro = (T)FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return ro;
    }

    private static Point DiePoint(DataRow r)
    {
        return new Point(Convert.ToInt32(r["XINDEX"]), Convert.ToInt32(r["YINDEX"]));
    }

    private static void Pump(int milliseconds)
    {
        var w = Stopwatch.StartNew();
        while (w.ElapsedMilliseconds < milliseconds) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    /// <summary>UI 스레드 await 가 이어지도록 메시지를 돌리며 기다린다.</summary>
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

    /// <summary>
    /// 칸 가운데를 마우스 왼쪽 버튼으로 누른다(DataGridView.OnMouseDown / OnMouseUp 을 직접 호출).
    /// 사용자 클릭과 같은 경로(OnCellMouseDown -> SetCurrentCellAddressCore(throughMouseClick))를 탄다.
    /// </summary>
    private static void ClickCell(DataGridView grid, int column, int row)
    {
        Rectangle r = grid.GetCellDisplayRectangle(column, row, false);
        if (r.IsEmpty) throw new InvalidOperationException("칸이 화면에 없음: " + column + ", " + row);
        var e = new MouseEventArgs(MouseButtons.Left, 1, r.X + r.Width / 2, r.Y + r.Height / 2, 0);
        MethodInfo down = typeof(Control).GetMethod("OnMouseDown", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo up = typeof(Control).GetMethod("OnMouseUp", BindingFlags.NonPublic | BindingFlags.Instance);
        try
        {
            down.Invoke(grid, new object[] { e });
            up.Invoke(grid, new object[] { e });
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    private static object Get(Type type, object target, string name)
    {
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null) throw new MissingFieldException(type.Name, name);
        return f.GetValue(target);
    }

    private static void Invoke(Type type, object target, string name, params object[] args)
    {
        type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, args);
    }

    private static void Check(string name, bool ok, string detail)
    {
        checks++;
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }
}
