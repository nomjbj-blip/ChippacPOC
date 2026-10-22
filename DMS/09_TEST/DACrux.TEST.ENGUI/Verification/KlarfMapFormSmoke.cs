using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// File Map Setup(KLARF 기준) 구현 2단계 화면 검사. 실제 빌드된 폼과 Map 컨트롤을 화면 밖에서 만들어 확인한다.
/// DB 저장은 호출하지 않는다. 사용: KlarfMapFormSmoke.exe <TEST.ENGUI dll> <KLARF 파일> <출력 png>
/// 확인 항목:
/// 1. 파일 읽기 후 원형 Map 이 만들어지고 격자 전체 Die 가 들어가는가 (샘플 300 mm: 12 x 9 = 108)
/// 2. XY 방향 4종 모두 Die 좌하단(DieCood)이 KLARF 물리 위치 (LB번호 - 옮긴 양) x Pitch 와 같은가
/// 3. 방향을 바꿔도 사용 Die 수가 같은가 / 검사 Die 26개가 모두 사용 Die 인가
/// 4. 사용 Die 가 원(반지름) 안에 완전히 들어가는가 / Edge 를 키우면 사용 Die 가 줄어드는가
/// 5. 저장용 DataTable (X / Y / USECODE) 이 Die 수와 같고 USECODE 가 1 / -1 인가
/// 6. 좌표 표시에 KLARF 원본 인덱스가 나오는가 / 파일 경로가 바뀌면 저장이 막히는가
/// 출력 마지막 줄: KLARF_FORM_SMOKE=PASS / FAIL
/// </summary>
internal static class KlarfMapFormSmoke
{
    private const double PitchX = 23834d;
    private const double PitchY = 30060.1d;
    private const int ShiftX = 7;
    private const int ShiftY = 5;
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { Run(args); }
        catch (Exception ex) { Console.WriteLine("ERROR " + ex); failures++; }
        Console.WriteLine(failures == 0 ? "KLARF_FORM_SMOKE=PASS" : "KLARF_FORM_SMOKE=FAIL (" + failures + ")");
        return failures == 0 ? 0 : 1;
    }

    private static void Run(string[] args)
    {
        Application.EnableVisualStyles();
        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type type = assembly.GetType("DACrux.TEST.ENGUI.frmStandardMapSetup", true);
        using (PreviewFixture form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();

            TextBox path = (TextBox)Field(type, form, "txtFile");
            path.Text = Path.GetFullPath(args[1]);
            Invoke(type, form, "btnParse_Click");

            Control map = (Control)Field(type, form, "m_wMap");
            Button save = (Button)Field(type, form, "btnSave");
            ComboBox direction = (ComboBox)Field(type, form, "cmbXYDirection");
            ComboBox wafer = (ComboBox)Field(type, form, "cmbWaferSize");
            NumericUpDown edge = (NumericUpDown)Field(type, form, "numEdge");
            Check("Map 컨트롤 = StandardMapEditMap", map.GetType().Name == "StandardMapEditMap");
            Check("저장 버튼 활성", save.Enabled);
            Check("기본 XY = LeftTop", direction.Text == "LeftTop");
            Check("기본 Wafer = 300", wafer.Text == "300");
            Check("격자 Die 108 (12 x 9)", Dies(map).Count == 108);
            Check("WaferSize 300000 µm", (double)Prop(map, "WaferSize") == 300000d);

            // 버튼 아이콘 (2026-10-08): resx 의 공용 아이콘이 로드되는가 (16 x 16)
            foreach (string b in new[] { "btnBrowse", "btnParse", "btnGenerate", "btnSave", "btnOpenMap", "btnPitchKlarf" })
            {
                Button button = (Button)Field(type, form, b);
                Check(b + " 아이콘 16x16", button.Image != null && button.Image.Width == 16 && button.Image.Height == 16);
            }

            // 우클릭 메뉴 (2026-10-13): Free Zoom / Fit Size / Move View Center + Center Cross 만 (Edit Map / Rotate Notch / Zone / Shot 등 숨김)
            string menu = ShownMenu(map);
            Console.WriteLine("우클릭 메뉴: " + menu);
            Check("우클릭 메뉴 = Free Zoom / Fit Size / Move View Center / - / Center Cross", menu == "Free Zoom / Fit Size / Move View Center / - / Center Cross");
            Check("십자선 기본 켬 (Designer CenterMark)", (bool)Prop(map, "CenterMark"));

            // Map 위 개수 줄 (2026-10-07): Net Die = 사용 Die = 저장 NETDIE, 생성 Die = 격자 전체 Die 수
            Label dieCount = (Label)Field(type, form, "lblDieCount");
            Console.WriteLine("개수 줄: " + dieCount.Text);
            Check("개수 줄 Net Die 79 / 생성 Die 108 (격자 12 x 9)",
                dieCount.Text.Contains("Net Die: 79") && dieCount.Text.Contains("생성 Die: 108 (격자 12 x 9)") && dieCount.Text.Contains("검사 Die 26 중 사용 26"));
            Check("개수 줄 Net Die = 저장 NETDIE (m_wMap.NetDie)", (int)Prop(map, "NetDie") == CountUsed(map) && CountUsed(map) == 79);

            int lbUsed = -1;
            foreach (string dir in new[] { "LeftBottom", "LeftTop", "RightBottom", "RightTop" })
            {
                direction.SelectedItem = dir;
                Invoke(type, form, "btnGenerate_Click");
                int used = CheckGeometry(type, form, map, dir);
                CheckHoverHitTest(form, map, dir, true);
                if (lbUsed < 0) lbUsed = used;
                Check(dir + " 사용 Die 수 같음 (" + used + ")", used == lbUsed);
                if (dir == "LeftBottom") SaveImages(form, map, args[2]);
            }

            direction.SelectedItem = "LeftBottom";
            Invoke(type, form, "btnGenerate_Click");
            int circle = CountInsideCircle(map, 150000d);
            Console.WriteLine("사용 Die " + lbUsed + " / 원 판정만(Notch 제외 안 함) " + circle);
            Check("사용 Die <= 원 안 Die", lbUsed <= circle && lbUsed > 0);

            using (System.Data.DataTable table = (System.Data.DataTable)type.GetMethod("CreateMapDieTable", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, null))
            {
                Check("저장 행 = Die 108", table.Rows.Count == 108);
                Check("USECODE 1 / -1 만", table.Select("USECODE <> 1 AND USECODE <> -1").Length == 0);
                Check("USECODE 1 = 사용 Die", table.Select("USECODE = 1").Length == lbUsed);
            }

            CheckCoordinateLabel(type, form, map);
            CheckStoredValues(assembly, type, form, map, edge);
            CheckEditModes(type, form, map, edge);
            CheckFlatZone(type, form, map);
            CheckPitch(assembly, type, form, map);

            edge.Value = 5000;
            Invoke(type, form, "btnGenerate_Click");
            int edgeUsed = CountUsed(map);
            Console.WriteLine("Edge 5000 µm 사용 Die " + edgeUsed);
            Check("Edge 5000 이면 사용 Die 감소", edgeUsed < lbUsed);
            CheckCustomerLayout(type, form, map, edge, direction);
            edge.Value = 0;

            wafer.SelectedItem = "200";
            Invoke(type, form, "btnGenerate_Click");
            TextBox info = (TextBox)Field(type, form, "txtInfo");
            Console.WriteLine("Wafer 200 mm: Die " + Dies(map).Count + " / 사용 " + CountUsed(map));
            Check("Wafer 200 정보에 검사 Die 제거 수 표시", info.Text.Contains("그중 제거됨"));

            path.Text += ".different";
            Check("경로 변경 시 저장 비활성", !save.Enabled);
            Check("경로 변경 시 개수 줄 초기화", ((Label)Field(type, form, "lblDieCount")).Text.StartsWith("Net Die: -"));
            form.Close();
        }
    }

    /// <summary>
    /// 화면 Map 은 XY 방향과 관계없이 LeftBottom 으로 그린다 (2026-10-13). 확인:
    /// 1. Map 컨트롤 XYDirect = LeftBottom, 모든 Die 의 좌하단(DieCood) = KLARF 물리 위치, 검사 Die 26개 모두 사용
    /// 2. 저장 행(CreateMapDieTable)은 선택 방향 번호다: 선택 방향 -> LeftBottom 으로 되돌리면 화면 Die 와 번호 / USECODE 가 같다
    ///    (Right 는 X, Top 은 Y 를 격자 범위에서 뒤집음. 화면 Rule 과 별도로 여기서 다시 계산)
    /// </summary>
    private static int CheckGeometry(Type type, Form form, Control map, string dir)
    {
        int minX = (int)Prop(map, "DieMinX"), maxX = (int)Prop(map, "DieMaxX");
        int minY = (int)Prop(map, "DieMinY"), maxY = (int)Prop(map, "DieMaxY");
        Check(dir + " 화면 Map 은 LeftBottom", Prop(map, "XYDirect").ToString() == "LeftBottom");
        System.Collections.Generic.Dictionary<string, int> screen = new System.Collections.Generic.Dictionary<string, int>();
        int bad = 0, used = 0, inspectedUsed = 0;
        foreach (object die in Dies(map))
        {
            int lbx = (int)GetField(die, "IndexX"), lby = (int)GetField(die, "IndexY");
            screen[lbx + "," + lby] = (int)GetField(die, "DieProp");
            object cood = GetField(die, "DieCood");
            double x = (double)cood.GetType().GetField("X").GetValue(cood);
            double y = (double)cood.GetType().GetField("Y").GetValue(cood);
            if (Math.Abs(x - (lbx - ShiftX) * PitchX) > 1e-6 || Math.Abs(y - (lby - ShiftY) * PitchY) > 1e-6) bad++;
            if ((int)GetField(die, "DieProp") == 1)
            {
                used++;
                if (IsSampleInspected(lbx, lby)) inspectedUsed++;
            }
        }
        Console.WriteLine(string.Format("{0}: 원점 인덱스 {1},{2} / 위치 불일치 {3} / 사용 {4} / 검사 Die 중 사용 {5}",
            dir, Prop(map, "OriginIndexX"), Prop(map, "OriginIndexY"), bad, used, inspectedUsed));
        Check(dir + " Die 위치 = KLARF 물리 위치", bad == 0);
        Check(dir + " 검사 Die 26 모두 사용", inspectedUsed == 26);

        int rowMismatch = 0, rows = 0;
        using (System.Data.DataTable table = (System.Data.DataTable)type.GetMethod("CreateMapDieTable", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, null))
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                rows++;
                int x = (int)row["X"], y = (int)row["Y"];
                int lbx = dir.StartsWith("Right") ? minX + maxX - x : x;
                int lby = dir.EndsWith("Top") ? minY + maxY - y : y;
                int prop;
                if (!screen.TryGetValue(lbx + "," + lby, out prop) || prop != (int)row["USECODE"]) rowMismatch++;
            }
        }
        Check(dir + " 저장 행 = 선택 방향 번호 (되돌리면 화면 Die 와 같음)", rows == screen.Count && rowMismatch == 0);
        return used;
    }

    /// <summary>
    /// 마우스 위치 -> Die 판정(hit-test) 확인 (2026-10-13, "최상단 라인 Die 정보가 안 나온다" 보고).
    /// 모든 Die 의 그려진 중심(DieCood - Origin + 반 칸)을 Map 의 GetViewPoint 로 픽셀로 바꿔 OnMouseMove 를 흉내 내고,
    /// Map 이 잡은 번호(m_iCurrentX / Y)가 그 Die 번호와 같은지 센다. 불일치 / 못 잡은 Die 수와 행(Y)별 결과를 출력한다.
    /// </summary>
    private static void CheckHoverHitTest(Form form, Control map, string dir, bool asCheck)
    {
        form.PerformLayout();
        Application.DoEvents();
        map.GetType().GetMethod("Redraw").Invoke(map, null);
        Type waferMap = map.GetType();
        while (waferMap != null && waferMap.Name != "WaferMap") waferMap = waferMap.BaseType;
        MethodInfo view = waferMap.GetMethod("GetViewPoint", BindingFlags.Instance | BindingFlags.NonPublic);
        MethodInfo move = waferMap.GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
        FieldInfo curX = waferMap.GetField("m_iCurrentX", BindingFlags.Instance | BindingFlags.NonPublic);
        FieldInfo curY = waferMap.GetField("m_iCurrentY", BindingFlags.Instance | BindingFlags.NonPublic);
        double ox = (double)Prop(map, "OriginX"), oy = (double)Prop(map, "OriginY");
        int ok = 0, wrong = 0, miss = 0;
        System.Collections.Generic.SortedDictionary<int, string> rowResult = new System.Collections.Generic.SortedDictionary<int, string>();
        foreach (object die in Dies(map))
        {
            if ((int)GetField(die, "DieProp") != 1) continue;
            int ix = (int)GetField(die, "IndexX"), iy = (int)GetField(die, "IndexY");
            object cood = GetField(die, "DieCood");
            double x = (double)cood.GetType().GetField("X").GetValue(cood) - ox + PitchX / 2;
            double y = (double)cood.GetType().GetField("Y").GetValue(cood) - oy + PitchY / 2;
            PointF p = (PointF)view.Invoke(map, new object[] { x, y });
            curX.SetValue(map, -1); curY.SetValue(map, -1);
            move.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.None, 0, (int)Math.Round(p.X), (int)Math.Round(p.Y), 0) });
            int hx = (int)curX.GetValue(map), hy = (int)curY.GetValue(map);
            string mark;
            if (hx == ix && hy == iy) { ok++; mark = "O"; }
            else if (hx == -1) { miss++; mark = "-"; }
            else { wrong++; mark = "X(" + hx + "," + hy + ")"; }
            string prev;
            rowResult.TryGetValue(iy, out prev);
            rowResult[iy] = (prev == null ? "" : prev + " ") + ix + ":" + mark;
        }
        Console.WriteLine(string.Format("{0} hit-test: 일치 {1} / 다른 Die {2} / 못 잡음 {3}", dir, ok, wrong, miss));
        if (wrong + miss > 0)
            foreach (var row in rowResult) Console.WriteLine("   Y=" + row.Key + " : " + row.Value);
        // asCheck = false 는 기존 Map Define 화면 참고 출력이다 (기존 WaferMap 동작이라 합격 기준에 넣지 않음).
        if (asCheck) Check(dir + " 마우스 위치 Die = 그려진 Die", wrong == 0 && miss == 0);
    }

    /// <summary>샘플 SINF RowData 의 숫자 칸 (LeftBottom = 열 + 1, 9 - 행). Probe 1단계에서 KLARF 검사 Die 와 같음을 확인했다.</summary>
    private static bool IsSampleInspected(int x, int y)
    {
        string[] rows =
        {
            "___ ___ 025 000 000 025 025 025 ___ ___",
            "___ ___ ___ ___ ___ ___ ___ ___ ___ ___",
            "025 ___ ___ ___ 025 025 ___ ___ ___ 025",
            "025 ___ ___ 025 025 025 025 ___ ___ 025",
            "025 ___ ___ ___ 025 025 ___ ___ ___ 025",
            "___ ___ ___ ___ ___ ___ ___ ___ ___ ___",
            "___ ___ 025 025 025 025 025 025 ___ ___",
        };
        int row = 9 - y, col = x - 1;
        if (row < 1 || row > rows.Length || col < 1 || col > 10) return false;
        string[] cells = rows[row - 1].Split(' ');
        return !cells[col - 1].StartsWith("_");
    }

    /// <summary>Notch 영역은 빼지 않고 Die 네 모서리가 반지름 안인지만 센다 (비교 기준).</summary>
    private static int CountInsideCircle(Control map, double radius)
    {
        double ox = (double)Prop(map, "OriginX"), oy = (double)Prop(map, "OriginY");
        int count = 0;
        foreach (object die in Dies(map))
        {
            object cood = GetField(die, "DieCood");
            double x = (double)cood.GetType().GetField("X").GetValue(cood) - ox;
            double y = (double)cood.GetType().GetField("Y").GetValue(cood) - oy;
            bool inside = true;
            foreach (double cx in new[] { x, x + PitchX })
                foreach (double cy in new[] { y, y + PitchY })
                    inside &= Math.Sqrt(cx * cx + cy * cy) <= radius;
            if (inside) count++;
        }
        return count;
    }

    /// <summary>
    /// 고객 U9 Wafer Map (BS top view, 76 Die) 과 같은 배치가 나오는지 확인한다 (2026-10-13).
    /// 고객표의 BS KLARF 좌표 행 범위: Y 4 ~ -4 행마다 X 시작 ~ 끝. KLARF 원본 = LeftBottom - (7, 5).
    /// Edge 3000 µm 로 원형 생성하면 사용 Die 가 이 76개와 정확히 같아야 한다. LeftTop 도 사용 Die 수가 같아야 한다.
    /// </summary>
    private static void CheckCustomerLayout(Type type, Form form, Control map, NumericUpDown edge, ComboBox direction)
    {
        int[,] rows = { { 4, -2, 1 }, { 3, -4, 3 }, { 2, -5, 4 }, { 1, -5, 4 }, { 0, -6, 5 },
                        { -1, -5, 4 }, { -2, -5, 4 }, { -3, -4, 3 }, { -4, -2, 1 } };
        HashSet<string> expected = new HashSet<string>();
        for (int r = 0; r < rows.GetLength(0); r++)
            for (int x = rows[r, 1]; x <= rows[r, 2]; x++) expected.Add(x + "," + rows[r, 0]);

        direction.SelectedItem = "LeftBottom";
        edge.Value = 3000;
        Invoke(type, form, "btnGenerate_Click");
        HashSet<string> used = new HashSet<string>();
        foreach (object die in Dies(map))
            if ((int)GetField(die, "DieProp") == 1)
                used.Add(((int)GetField(die, "IndexX") - 7) + "," + ((int)GetField(die, "IndexY") - 5));
        Console.WriteLine("Edge 3000 µm 사용 Die " + used.Count + " / 고객 Map 76");
        Check("Edge 3000 사용 Die = 고객 U9 Map 76개 배치", used.SetEquals(expected));

        direction.SelectedItem = "LeftTop";
        Invoke(type, form, "btnGenerate_Click");
        Check("Edge 3000 LeftTop 사용 Die 76", CountUsed(map) == 76);
        direction.SelectedItem = "LeftBottom";
    }

    private static int CountUsed(Control map)
    {
        int used = 0;
        foreach (object die in Dies(map)) if ((int)GetField(die, "DieProp") == 1) used++;
        return used;
    }

    /// <summary>KLARF 원본 (0,0) Die = LeftBottom (7,5) 를 좌표 표시 이벤트에 넣어 라벨 내용을 확인한다.</summary>
    private static void CheckCoordinateLabel(Type type, Form form, Control map)
    {
        object target = null;
        foreach (object die in Dies(map))
            if ((int)GetField(die, "IndexX") == ShiftX && (int)GetField(die, "IndexY") == ShiftY) target = die;
        Check("LeftBottom (7,5) Die 존재", target != null);
        if (target == null) return;
        type.GetMethod("m_wMap_OnChangeCurrentDie", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { map, target });
        Type pointD = target.GetType().Assembly.GetType("DACrux.Base.PointD", true);
        object pt = Activator.CreateInstance(pointD);
        type.GetMethod("m_wMap_OnChangePosition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { map, pt, pt });
        string text = ((Label)Field(type, form, "lblCoordinates")).Text;
        Console.WriteLine("좌표 표시: " + text.Replace("\r\n", " | "));
        Check("좌표 표시에 KLARF 원본 0, 0", text.Contains("KLARF 원본: 0, 0") && text.Contains("검사 Die: 예"));
    }

    /// <summary>
    /// 저장값 변환 (MapCreationService.ToStoredValues) 확인. DB 는 호출하지 않는다.
    /// - Edge 입력칸 최대 5000 µm (Map Define 의 Edge 입력칸 최대 5 mm 와 맞춤)
    /// - Edge 200 µm -> EDGE_SIZE 0.2 mm / ORIGIN_MICRO_X = OriginX / ORIGIN_MICRO_Y = DieSizeY - OriginY
    /// - 조회 쿼리 규칙 (CHIP_SIZE_Y - ORIGIN_MICRO_Y) 으로 되돌리면 화면 OriginY 와 같다
    /// - XY 방향 4종 모두: 저장값(원점 인덱스 / XY_DIRECTION / 방향 번호 Die)을 기존 Map Define 으로 불러오면
    ///   화면 Map 과 같은 위치에 그려진다 (2026-10-13)
    /// </summary>
    private static void CheckStoredValues(Assembly assembly, Type formType, Form form, Control map, NumericUpDown edge)
    {
        Check("Edge 입력 최대 5000 µm", edge.Maximum == 5000m);
        ComboBox direction = (ComboBox)Field(formType, form, "cmbXYDirection");
        string before = direction.Text;
        edge.Value = 200;
        bool first = true;
        foreach (string dir in new[] { "LeftBottom", "LeftTop", "RightBottom", "RightTop" })
        {
            direction.SelectedItem = dir;
            Invoke(formType, form, "btnGenerate_Click");
            object stored = StoredValues(assembly, formType, form, map);
            double edgeMm = (double)Prop(stored, "EdgeSizeMm");
            double ox = (double)Prop(stored, "OriginMicroX");
            double oy = (double)Prop(stored, "OriginMicroY");
            if (first)
            {
                double mapOriginX = (double)Prop(map, "OriginX"), mapOriginY = (double)Prop(map, "OriginY");
                Console.WriteLine(string.Format("저장값: EDGE {0} mm / ORIGIN_MICRO {1}, {2}", edgeMm, ox, oy));
                Check("EDGE_SIZE 0.2 mm", Math.Abs(edgeMm - 0.2) < 1e-9);
                Check("ORIGIN_MICRO_X = -29", Math.Abs(ox - mapOriginX) < 1e-9 && Math.Abs(ox + 29) < 1e-9);
                Check("ORIGIN_MICRO_Y = 14869.1", Math.Abs(oy - 14869.1) < 1e-6);
                Check("조회 규칙으로 되돌린 Y = 화면 OriginY", Math.Abs((PitchY - oy) - mapOriginY) < 1e-6);
            }
            Check(dir + " 저장 XY_DIRECTION", (int)Prop(stored, "XYDirection") == (int)Enum.Parse(Prop(map, "XYDirect").GetType(), dir));
            CheckMapDefineReadBack(assembly, formType, form, map, stored, dir, first);
            first = false;
        }
        edge.Value = 0;
        direction.SelectedItem = before;
        Invoke(formType, form, "btnGenerate_Click");
    }

    /// <summary>화면의 현재 XY 방향 / 격자로 MapCreationService.ToStoredValues 를 부른다 (화면이 저장할 때와 같은 값).</summary>
    private static object StoredValues(Assembly assembly, Type formType, Form form, Control map)
    {
        Type service = assembly.GetType("DACrux.TEST.ENGUI.MapCreationService", true);
        object direction = Field(formType, form, "currentDirection");
        object grid = Field(formType, form, "currentGrid");
        return service.GetMethod("ToStoredValues", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { map, direction, grid });
    }

    /// <summary>
    /// 기존 Map Define 화면(frmSetupMap)의 불러오기 함수 DrawLoadingMap 에 GET_MAPDEF 결과와 같은 모양의 DataTable 을 넣는다.
    /// GET_MAPDEF 는 ORIGIN_MICRO_Y 를 CHIP_SIZE_Y - 값 으로 돌려주므로 그 계산도 여기서 똑같이 한다. 숫자 컬럼은 Oracle NUMBER 처럼 decimal 이다.
    /// 원점 인덱스 / XY_DIRECTION / Die 번호는 저장값(선택 방향)을 쓴다.
    /// 확인: 불러오기 중 예외가 없는가 (Edge 범위 오류 재발 방지) / Map Define 의 모든 Die 를 LeftBottom 으로 되돌렸을 때
    ///       위치와 USECODE 가 File Map Setup 화면과 같은가. 참고로 Map Define 자체의 마우스 판정 결과도 출력한다.
    /// frmSetupMap 코드는 바꾸지 않고, 접속 이력 / 목록 조회(Load 이벤트)만 막은 하위 클래스로 실행한다.
    /// </summary>
    private static void CheckMapDefineReadBack(Assembly assembly, Type formType, Form form, Control map, object stored, string dir, bool reproduceOldError)
    {
        int minX = (int)Prop(map, "DieMinX"), maxX = (int)Prop(map, "DieMaxX");
        int minY = (int)Prop(map, "DieMinY"), maxY = (int)Prop(map, "DieMaxY");
        System.Data.DataTable def = new System.Data.DataTable();
        string[] cols = { "WAFER_SIZE", "ANGLE", "NOTCH_TYPE", "EDGE_SIZE", "CHIP_SIZE_X", "CHIP_SIZE_Y", "ORIGIN_MICRO_X", "ORIGIN_MICRO_Y",
            "DIE_INDEX_MIN_X", "DIE_INDEX_MAX_X", "DIE_INDEX_MIN_Y", "DIE_INDEX_MAX_Y", "ORIGIN_INDEX_X", "ORIGIN_INDEX_Y",
            "FIRST_INDEX_X", "FIRST_INDEX_Y", "XY_DIRECTION", "REFERENCEDIE_SETTING" };
        foreach (string c in cols) def.Columns.Add(c, typeof(decimal));
        System.Data.DataRow r = def.NewRow();
        r["WAFER_SIZE"] = (decimal)(double)Prop(map, "WaferSize");
        r["ANGLE"] = (decimal)(int)Prop(map, "NotchAngle");
        r["NOTCH_TYPE"] = Prop(map, "NotchType").ToString() == "Notch" ? 0m : 1m;
        r["EDGE_SIZE"] = (decimal)(double)Prop(stored, "EdgeSizeMm");
        // CHIP_SIZE = 화면 Map 의 Die 크기 (저장 서비스가 map.DieSizeX / Y 를 그대로 넣는다. Pitch 를 바꾸면 그 값)
        decimal chipX = (decimal)(double)Prop(map, "DieSizeX");
        decimal chipY = (decimal)(double)Prop(map, "DieSizeY");
        r["CHIP_SIZE_X"] = chipX;
        r["CHIP_SIZE_Y"] = chipY;
        r["ORIGIN_MICRO_X"] = (decimal)(double)Prop(stored, "OriginMicroX");
        r["ORIGIN_MICRO_Y"] = chipY - (decimal)(double)Prop(stored, "OriginMicroY");
        r["DIE_INDEX_MIN_X"] = (decimal)minX;
        r["DIE_INDEX_MAX_X"] = (decimal)maxX;
        r["DIE_INDEX_MIN_Y"] = (decimal)minY;
        r["DIE_INDEX_MAX_Y"] = (decimal)maxY;
        r["ORIGIN_INDEX_X"] = (decimal)(int)Prop(stored, "OriginIndexX");
        r["ORIGIN_INDEX_Y"] = (decimal)(int)Prop(stored, "OriginIndexY");
        r["FIRST_INDEX_X"] = (decimal)(int)Prop(map, "FirstDieX");
        r["FIRST_INDEX_Y"] = (decimal)(int)Prop(map, "FirstDieY");
        r["XY_DIRECTION"] = (decimal)(int)Prop(stored, "XYDirection");
        r["REFERENCEDIE_SETTING"] = 0m;
        def.Rows.Add(r);

        System.Data.DataTable dies = (System.Data.DataTable)formType.GetMethod("CreateMapDieTable", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, null);
        Type setupType = assembly.GetType("DACrux.TEST.ENGUI.frmSetupMap", true);
        using (SetupMapFixture setup = new SetupMapFixture())
        {
            setup.StartPosition = FormStartPosition.Manual;
            setup.Location = new Point(-32000, -32000);
            setup.ShowInTaskbar = false;
            setup.Show();
            Application.DoEvents();
            MethodInfo draw = setupType.GetMethod("DrawLoadingMap", BindingFlags.Instance | BindingFlags.NonPublic);

            if (reproduceOldError)
            {
                // 원인 재현: 수정 전 저장값(EDGE_SIZE 200, µm 그대로)이면 Map Define 불러오기가 실패해야 한다.
                System.Data.DataTable oldDef = def.Copy();
                oldDef.Rows[0]["EDGE_SIZE"] = 200m;
                string oldError = null;
                try { draw.Invoke(setup, new object[] { oldDef, dies }); }
                catch (TargetInvocationException ex) { oldError = ex.InnerException == null ? ex.Message : ex.InnerException.GetType().Name; }
                Console.WriteLine("수정 전 값(EDGE 200) 불러오기: " + (oldError ?? "예외 없음"));
                Check("수정 전 값은 Map Define 에서 실패 (원인 재현)", oldError != null);
            }

            string error = null;
            try { draw.Invoke(setup, new object[] { def, dies }); }
            catch (TargetInvocationException ex) { error = ex.InnerException == null ? ex.Message : ex.InnerException.Message; }
            Check(dir + " Map Define 불러오기 예외 없음" + (error == null ? "" : " (" + error + ")"), error == null);
            if (error != null) return;

            Control loaded = (Control)setupType.GetField("m_wMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(setup);
            System.Collections.Generic.Dictionary<string, string> expected = new System.Collections.Generic.Dictionary<string, string>();
            foreach (object die in Dies(map)) expected[Key(die)] = Pos(die);
            int mismatch = 0;
            foreach (object die in Dies(loaded))
            {
                int x = (int)GetField(die, "IndexX"), y = (int)GetField(die, "IndexY");
                int lbx = dir.StartsWith("Right") ? minX + maxX - x : x;
                int lby = dir.Contains("Top") ? minY + maxY - y : y;
                string key = lbx + "," + lby + "," + GetField(die, "DieProp");
                string pos;
                if (!expected.TryGetValue(key, out pos) || pos != Pos(die)) mismatch++;
            }
            Console.WriteLine(string.Format("{0} Map Define 불러오기: Die {1} / 불일치 {2} / XY {3} / OriginY {4}",
                dir, Dies(loaded).Count, mismatch, Prop(loaded, "XYDirect"), Prop(loaded, "OriginY")));
            Check(dir + " Map Define Die 수 같음", Dies(loaded).Count == Dies(map).Count);
            Check(dir + " Map Define Die 위치 / USECODE 같음", mismatch == 0);
            CheckHoverHitTest(setup, loaded, dir + " (기존 Map Define 참고)", false);
            setup.Close();
        }
    }

    /// <summary>
    /// 3단계 (2026-10-08) 편집 모드 / 보조 버튼 확인. 확인창은 PreviewFixture 가 "예" 로 답한다.
    /// - 기본 보기 = 클릭해도 편집 안 됨 (IsDieEditing false), Edit 메뉴 숨김
    /// - 제거 = DELETE + 편집 상태, 복원 = ADD + 편집 상태, 보기로 돌아오면 편집 해제
    /// - [검사 Die 외 제거]: 사용 79 -> 26 (검사 Die 만 남음), 개수 줄 / 편집 표시 갱신
    /// - [Wafer 밖 제거]: 원 밖 Die 를 강제로 살린 뒤 실행하면 원 안 79 로 돌아온다 / Edge 5000 µm 로 실행하면 76
    /// 끝나면 [원형 생성] 으로 원래 상태(79)로 되돌린다.
    /// </summary>
    private static void CheckEditModes(Type type, Form form, Control map, NumericUpDown edge)
    {
        RadioButton view = (RadioButton)Field(type, form, "rdoView");
        RadioButton remove = (RadioButton)Field(type, form, "rdoRemove");
        RadioButton restore = (RadioButton)Field(type, form, "rdoRestore");
        Label dieCount = (Label)Field(type, form, "lblDieCount");
        Check("기본 = 보기 / 편집 안 됨", view.Checked && !(bool)Prop(map, "IsDieEditing"));
        Check("Edit 메뉴 숨김", !(bool)Prop(map, "EditMenuVisible"));
        remove.Checked = true;
        Check("제거 = DELETE + 편집", (bool)Prop(map, "IsDieEditing") && Prop(map, "EditMethod").ToString() == "DELETE");
        restore.Checked = true;
        Check("복원 = ADD + 편집", (bool)Prop(map, "IsDieEditing") && Prop(map, "EditMethod").ToString() == "ADD");
        view.Checked = true;
        Check("보기로 돌아오면 편집 해제", !(bool)Prop(map, "IsDieEditing"));

        Invoke(type, form, "btnRemoveNotInspected_Click");
        Console.WriteLine("검사 Die 외 제거 후: " + dieCount.Text);
        Check("검사 Die 외 제거 -> 사용 26", CountUsed(map) == 26 && dieCount.Text.Contains("Net Die: 26") && dieCount.Text.Contains("검사 Die 26 중 사용 26"));
        Check("검사 Die 외 제거 -> 편집 표시", (bool)Field(type, form, "dieEdited"));

        // 복원 모드로 원 밖 Die 를 살린 상황을 흉내 낸다 (모든 Die = 1)
        foreach (object die in Dies(map)) die.GetType().GetField("DieProp").SetValue(die, 1);
        Invoke(type, form, "btnRemoveOutside_Click");
        Check("Wafer 밖 제거 -> 원 안 79", CountUsed(map) == 79);
        edge.Value = 5000;
        Invoke(type, form, "btnRemoveOutside_Click");
        Check("Edge 5000 으로 Wafer 밖 제거 -> 76", CountUsed(map) == 76);
        edge.Value = 0;
        Invoke(type, form, "btnGenerate_Click");
        Check("원형 다시 생성 -> 79 / 편집 표시 해제", CountUsed(map) == 79 && !(bool)Field(type, form, "dieEdited"));
    }

    /// <summary>
    /// Flat Zone 위치 / 형태 설정 확인 (2026-10-13).
    /// - 파일 초기값: 샘플 KLARF NOTCH / DOWN -> 형태 Notch / 위치 Bottom
    /// - 형태 Flat 으로 위치 4종 원형 생성: NotchAngle 이 Bottom 0 / Left 90 / Top 180 / Right 270,
    ///   Map 이 계산한 Flat 영역(m_rectNotchArea, 원 안 판정 UseDie 가 쓰는 영역)의 중심이 선택한 쪽(아래 / 왼쪽 / 위 / 오른쪽)에 있다.
    /// - 샘플(300 mm)은 Flat(30° 현, 중심에서 약 144.9 mm)까지 닿는 Die 가 없어서 사용 Die 수는 그대로다 (가장 바깥 Die 끝 135.4 mm).
    ///   그래서 Die 감소 대신 Flat 영역 위치로 확인하고, 줄어든 Die 수는 출력만 한다.
    /// 끝나면 파일 값(Notch / Bottom)으로 다시 생성한다.
    /// </summary>
    private static void CheckFlatZone(Type type, Form form, Control map)
    {
        ComboBox zone = (ComboBox)Field(type, form, "cmbFlatZone");
        ComboBox mark = (ComboBox)Field(type, form, "cmbMarkType");
        Check("Flat Zone 초기값 = 파일 (Bottom / Notch)", zone.Text == "Bottom" && mark.Text == "Notch");
        Invoke(type, form, "btnGenerate_Click");
        System.Collections.Generic.HashSet<string> notchUsed = new System.Collections.Generic.HashSet<string>();
        foreach (object die in Dies(map))
            if ((int)GetField(die, "DieProp") == 1) notchUsed.Add(GetField(die, "IndexX") + "," + GetField(die, "IndexY"));

        double ox = (double)Prop(map, "OriginX"), oy = (double)Prop(map, "OriginY");
        mark.SelectedItem = "Flat";
        foreach (string z in new[] { "Bottom", "Left", "Top", "Right" })
        {
            zone.SelectedItem = z;
            Invoke(type, form, "btnGenerate_Click");
            int angle = (int)Prop(map, "NotchAngle");
            double sumX = 0, sumY = 0;
            int lost = 0;
            foreach (object die in Dies(map))
            {
                string key = GetField(die, "IndexX") + "," + GetField(die, "IndexY");
                if ((int)GetField(die, "DieProp") == 1 || !notchUsed.Contains(key)) continue;
                object cood = GetField(die, "DieCood");
                sumX += (double)cood.GetType().GetField("X").GetValue(cood) - ox + PitchX / 2;
                sumY += (double)cood.GetType().GetField("Y").GetValue(cood) - oy + PitchY / 2;
                lost++;
            }
            // Flat 영역: X = 왼쪽, Y = 위쪽 (Map 좌표), Width / Height. 중심 = (X + W/2, Y - H/2)
            Type waferMap = map.GetType();
            while (waferMap != null && waferMap.Name != "WaferMap") waferMap = waferMap.BaseType;
            RectangleF area = (RectangleF)waferMap.GetField("m_rectNotchArea", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(map);
            double ax = area.X + area.Width / 2, ay = area.Y - area.Height / 2;
            bool side = z == "Bottom" ? ay < 0 && Math.Abs(ay) > Math.Abs(ax)
                : z == "Top" ? ay > 0 && Math.Abs(ay) > Math.Abs(ax)
                : z == "Left" ? ax < 0 && Math.Abs(ax) > Math.Abs(ay)
                : ax > 0 && Math.Abs(ax) > Math.Abs(ay);
            Console.WriteLine(string.Format("Flat {0}: Angle {1} / Flat 영역 중심 ({2:0}, {3:0}) µm / 사용 {4} / Notch 대비 줄어든 Die {5}",
                z, angle, ax, ay, CountUsed(map), lost));
            int expectedAngle = z == "Left" ? 90 : z == "Top" ? 180 : z == "Right" ? 270 : 0;
            Check("Flat " + z + " 각도 " + expectedAngle, angle == expectedAngle && Prop(map, "NotchType").ToString() == "Flat");
            Check("Flat " + z + " 영역이 " + z + " 쪽", side);
        }
        mark.SelectedItem = "Notch";
        zone.SelectedItem = "Bottom";
        Invoke(type, form, "btnGenerate_Click");
        Check("Notch / Bottom 으로 되돌림 -> 79", CountUsed(map) == 79);
    }

    /// <summary>
    /// Die Pitch 설정 확인 (2026-10-13).
    /// - 초기값 = KLARF DiePitch (23,834 x 30,060.1), 입력 범위 = KLARF 값의 50% ~ 200%, 정보창에 차이 경고 없음
    /// - 23,830 x 30,064 로 원형 생성: Map Die 크기(= 저장 CHIP_SIZE) 반영, 격자 / KLARF (0,0) Die 인덱스 유지,
    ///   모든 Die 좌하단 = (LB번호 - 옮긴 양) x 새 Pitch, ORIGIN_MICRO_Y = 새 Pitch Y - OriginY,
    ///   정보창 경고 (차이 X -4 / Y +3.9, 격자 끝 최대 X 24 (6칸) / Y 19.5 (5칸)), Map Define 불러오기 위치 같음
    /// - 단위 착오 입력(23.834)은 최소값(50%)으로 맞춰지고, 0 이하 인덱스가 생기면 정보창에 알린다
    /// - [KLARF 값으로]: 입력칸만 되돌리고 [원형 생성] 후 원래 Map (Die 크기 = 파일 값, 사용 79) 으로 돌아온다
    /// </summary>
    private static void CheckPitch(Assembly assembly, Type type, Form form, Control map)
    {
        NumericUpDown px = (NumericUpDown)Field(type, form, "numPitchX");
        NumericUpDown py = (NumericUpDown)Field(type, form, "numPitchY");
        Button reset = (Button)Field(type, form, "btnPitchKlarf");
        TextBox info = (TextBox)Field(type, form, "txtInfo");
        Label status = (Label)Field(type, form, "lblStatus");

        Check("Pitch 초기값 = KLARF", px.Value == 23834m && py.Value == 30060.1m);
        Check("Pitch 범위 = KLARF 50% ~ 200%", px.Minimum == 11917m && px.Maximum == 47668m && py.Minimum == 15030.05m && py.Maximum == 60120.2m);
        Check("Pitch 그대로면 Die 크기 = 파일 값", (double)Prop(map, "DieSizeX") == PitchX && (double)Prop(map, "DieSizeY") == PitchY);
        Check("Pitch 그대로면 경고 없음", info.Text.Contains("Die Pitch(KLARF): 23,834 x 30,060.1") && !info.Text.Contains("KLARF Pitch 와 다름"));
        Check("[KLARF 값으로] 활성", reset.Enabled);

        const double newX = 23830d, newY = 30064d;
        px.Value = (decimal)newX;
        py.Value = (decimal)newY;
        Check("Pitch 변경 -> 원형 생성 전 표시", status.Text.Contains("원형 생성 전"));
        Check("Pitch 변경만으로는 Map 그대로", (double)Prop(map, "DieSizeX") == PitchX);
        Invoke(type, form, "btnGenerate_Click");
        Check("변경 Pitch -> Die 크기 (저장 CHIP_SIZE)", (double)Prop(map, "DieSizeX") == newX && (double)Prop(map, "DieSizeY") == newY);
        Check("변경 Pitch -> 격자 108 / 원점 인덱스 (7,5) 유지",
            Dies(map).Count == 108 && (int)Prop(map, "OriginIndexX") == ShiftX && (int)Prop(map, "OriginIndexY") == ShiftY);
        int bad = 0;
        foreach (object die in Dies(map))
        {
            int lbx = (int)GetField(die, "IndexX"), lby = (int)GetField(die, "IndexY");
            object cood = GetField(die, "DieCood");
            double x = (double)cood.GetType().GetField("X").GetValue(cood);
            double y = (double)cood.GetType().GetField("Y").GetValue(cood);
            if (Math.Abs(x - (lbx - ShiftX) * newX) > 1e-6 || Math.Abs(y - (lby - ShiftY) * newY) > 1e-6) bad++;
        }
        Check("변경 Pitch -> Die 좌하단 = (LB - 옮긴 양) x 새 Pitch", bad == 0);
        object stored = StoredValues(assembly, type, form, map);
        Check("변경 Pitch -> ORIGIN_MICRO_Y = 새 Pitch Y - OriginY",
            Math.Abs((double)Prop(stored, "OriginMicroY") - (newY - (double)Prop(map, "OriginY"))) < 1e-6);
        string warning = Array.Find(info.Lines, l => l.Contains("KLARF Pitch 와 다름")) ?? "";
        Console.WriteLine("Pitch 경고: " + warning);
        Check("변경 Pitch -> 정보창 경고 / 차이 / 격자 끝 최대",
            warning.Contains("차이 X -4 / Y +3.9 /") && warning.EndsWith("최대 X 24 / Y 19.5") && info.Text.Contains("Die Pitch(적용 / 저장): 23,830 x 30,064"));
        Console.WriteLine("Pitch 변경 사용 Die " + CountUsed(map) + " / " + ((Label)Field(type, form, "lblDieCount")).Text);
        string direction = ((ComboBox)Field(type, form, "cmbXYDirection")).Text;
        CheckMapDefineReadBack(assembly, type, form, map, stored, direction + " (Pitch 변경)", false);

        // 단위 착오: mm 로 입력하면 최소값(KLARF 50%)으로 맞춰진다 (사용자가 입력칸에 쓴 것과 같게 Text + ValidateEditText)
        px.Text = "23.834";
        typeof(NumericUpDown).GetMethod("ValidateEditText", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(px, null);
        Check("Pitch 23.834 입력 -> 최소값 11,917", px.Value == 11917m);
        Invoke(type, form, "btnGenerate_Click");
        object grid = Field(type, form, "currentGrid");
        int minX = (int)Prop(grid, "MinX"), minY = (int)Prop(grid, "MinY");
        Console.WriteLine(string.Format("Pitch 11,917 x 30,064: 격자 X {0}~{1} / Die {2} / 사용 {3}", minX, Prop(grid, "MaxX"), Dies(map).Count, CountUsed(map)));
        object origin = null;
        foreach (object die in Dies(map))
            if ((int)GetField(die, "IndexX") == ShiftX && (int)GetField(die, "IndexY") == ShiftY) origin = die;
        Check("최소 Pitch -> KLARF (0,0) Die 위치 (0,0) 유지", origin != null && Pos(origin) == "0.000,0.000");
        Check("0 이하 인덱스 안내 = 격자 최소값과 맞음", info.Text.Contains("0 이하 인덱스") == (minX < 1 || minY < 1));

        Invoke(type, form, "btnPitchKlarf_Click");
        Check("[KLARF 값으로] -> 입력칸 = KLARF / Map 은 그대로 / 안내", px.Value == 23834m && py.Value == 30060.1m
            && (double)Prop(map, "DieSizeX") == 11917d && status.Text.Contains("되돌렸습니다"));
        Invoke(type, form, "btnGenerate_Click");
        Check("되돌린 뒤 원형 생성 -> 파일 Pitch / 108 / 사용 79 / 경고 없음",
            (double)Prop(map, "DieSizeX") == PitchX && (double)Prop(map, "DieSizeY") == PitchY && Dies(map).Count == 108
            && CountUsed(map) == 79 && !info.Text.Contains("KLARF Pitch 와 다름"));
    }

    /// <summary>Die 번호 + USECODE (비교 키).</summary>
    private static string Key(object die)
    {
        return GetField(die, "IndexX") + "," + GetField(die, "IndexY") + "," + GetField(die, "DieProp");
    }

    /// <summary>Die 좌하단 좌표를 소수 3자리 문자열로 (비교 값).</summary>
    private static string Pos(object die)
    {
        object cood = GetField(die, "DieCood");
        double x = (double)cood.GetType().GetField("X").GetValue(cood);
        double y = (double)cood.GetType().GetField("Y").GetValue(cood);
        return x.ToString("0.000") + "," + y.ToString("0.000");
    }

    /// <summary>기존 Map Define 화면에서 Load 이벤트(Map 목록 원격 조회 / 초기화)와 접속 이력만 막는다.</summary>
    private sealed class SetupMapFixture : DACrux.TEST.ENGUI.frmSetupMap
    {
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }

    private static void SaveImages(Form form, Control map, string png)
    {
        form.PerformLayout();
        Application.DoEvents();
        map.GetType().GetMethod("Redraw").Invoke(map, null);
        Bitmap image = (Bitmap)map.GetType().GetMethod("GetMapImage").Invoke(map, null);
        if (image != null) image.Save(Path.ChangeExtension(png, ".map.png"));
        using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
            bitmap.Save(png);
        }
        Console.WriteLine("이미지: " + png);
    }

    /// <summary>우클릭 메뉴가 뜰 때(Popup)와 같은 처리를 실행하고 보이는 항목 Text 를 " / " 로 잇는다.</summary>
    private static string ShownMenu(Control map)
    {
        Type waferMap = map.GetType();
        while (waferMap != null && waferMap.Name != "WaferMap") waferMap = waferMap.BaseType;
        ContextMenu menu = (ContextMenu)waferMap.GetField("ctxmWaferMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(map);
        typeof(ContextMenu).GetMethod("OnPopup", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(menu, new object[] { EventArgs.Empty });
        List<string> shown = new List<string>();
        foreach (MenuItem item in menu.MenuItems)
            if (item.Visible) shown.Add(item.Text);
        return string.Join(" / ", shown.ToArray());
    }

    private static IList Dies(Control map) { return (IList)map.GetType().GetProperty("Dies").GetValue(map, null); }
    private static object Prop(object target, string name) { return target.GetType().GetProperty(name).GetValue(target, null); }
    private static object GetField(object target, string name) { return target.GetType().GetField(name).GetValue(target); }

    private static object Field(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    private static void Invoke(Type type, object form, string method)
    {
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { null, EventArgs.Empty });
    }

    private static void Check(string name, bool ok)
    {
        Console.WriteLine((ok ? "PASS " : "FAIL ") + name);
        if (!ok) failures++;
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록만 생략한다. 실제 Designer / 컨트롤 / 생성 코드는 그대로 실행한다.</summary>
    private sealed class PreviewFixture : DACrux.TEST.ENGUI.frmStandardMapSetup
    {
        /// <summary>확인창은 "예" 로 답하고 내용만 출력한다 (화면 밖 검사가 멈추지 않게).</summary>
        protected override bool AskYesNo(string text, string title) { Console.WriteLine("[확인창 예] " + title + ": " + text.Replace("\r\n", " ")); return true; }
        protected override void ShowInformation(string text, string title) { Console.WriteLine("[안내창] " + title + ": " + text); }
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }
}
