using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// Wafer Map 조회 화면(frmStandardMapView) 검사. 실제 빌드된 폼과 Map 컨트롤을 화면 밖에서 만들어 확인한다. DB / 원격 서비스는 호출하지 않는다.
/// 사용: StandardMapViewSmoke.exe <TEST.ENGUI dll> <KLARF 파일> <출력 png>
/// 확인 항목:
/// 1. File Map Setup 화면이 XY 방향 4종으로 만든 저장값(GET_MAPDEF 모양 + TQP_USEMAP 행)을 조회 화면에 넣으면
///    조회 화면 Die 위치 / 번호(LeftBottom) / USECODE 가 File Map Setup 화면 Map 과 같은가, 화면 Map 은 LeftBottom 인가
/// 2. 조회 화면 마우스 위치 Die 판정이 4방향 모두 그려진 Die 와 같은가 (LeftTop 최상단 행 포함)
/// 3. 같은 저장값을 기존 Map Define(frmSetupMap.DrawLoadingMap)으로 그린 위치와 같은가
/// 4. File Map Setup 으로 저장한 값은 데이터 확인 항목이 모두 일치인가 / 좌표 표시에 저장 방향 번호와 LeftBottom 번호가 나오는가
/// 5. 기존 Map Define 방식 Map (격자 일부 / USECODE 0, 3 / 임의 원점 인덱스) 도 4방향 모두 기존 화면과 같은 위치인가
/// 6. 일부러 틀린 값(NETDIE / Die 0행 / 중복 / Edge µm / Wafer mm / XY 방향 7)은 해당 항목만 다름인가
/// 7. Map 목록: 표시 컬럼 / 건수 / 검색(대소문자 무시, 특수 문자) / 자동 선택 없음
/// 8. (인수 4 = DB 데이터 폴더, StandardMapViewDbProbe.ps1) 실제 DB 조회 결과로 목록 / Map 표시 / 마우스 판정 확인, 데이터 확인 결과 출력
/// 9. Die 번호 (2026-10-13): 방향 4종 모두 조회 화면 / File Map Setup 화면 번호 = 시험 코드가 따로 계산한 번호 (시작 모서리부터 행 우선, 사용 Die 만),
///    번호를 켜면 Map 그림이 바뀌는가, 좌표 / Die 목록에 번호가 나오는가, 저장 DIE_NUM 비교 항목 (행 순서 = 참고 / Direction 번호 = 일치)
/// 출력 마지막 줄: STANDARD_MAP_VIEW_SMOKE=PASS / FAIL
/// </summary>
internal static class StandardMapViewSmoke
{
    private static readonly string[] Directions = { "LeftBottom", "LeftTop", "RightBottom", "RightTop" };
    private static int failures;
    /// <summary>화면 이미지 저장 경로 (인수 3). 확인 탭 / 목록 이미지는 같은 폴더에 이름만 바꿔 저장한다.</summary>
    private static string imagePath;

    [STAThread]
    private static int Main(string[] args)
    {
        try { Run(args); }
        catch (Exception ex) { Console.WriteLine("ERROR " + ex); failures++; }
        Console.WriteLine(failures == 0 ? "STANDARD_MAP_VIEW_SMOKE=PASS" : "STANDARD_MAP_VIEW_SMOKE=FAIL (" + failures + ")");
        return failures == 0 ? 0 : 1;
    }

    private static void Run(string[] args)
    {
        Application.EnableVisualStyles();
        imagePath = args[2];
        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type setupType = assembly.GetType("DACrux.TEST.ENGUI.frmStandardMapSetup", true);
        Type viewType = assembly.GetType("DACrux.TEST.ENGUI.frmStandardMapView", true);
        using (SetupFixture setup = new SetupFixture())
        using (ViewFixture view = new ViewFixture())
        {
            ShowOffScreen(setup);
            ShowOffScreen(view);

            ((TextBox)Field(setupType, setup, "txtFile")).Text = Path.GetFullPath(args[1]);
            Invoke(setupType, setup, "btnParse_Click");
            Control setupMap = (Control)Field(setupType, setup, "m_wMap");
            Control viewMap = (Control)Field(viewType, view, "m_wMap");
            ComboBox direction = (ComboBox)Field(setupType, setup, "cmbXYDirection");

            Check("조회 화면 Map 컨트롤 = StandardMapEditMap", viewMap.GetType().Name == "StandardMapEditMap");
            // 우클릭 메뉴 (2026-10-13): Free Zoom / Fit Size / Move View Center + Center Cross 만
            string menu = ShownMenu(viewMap);
            Console.WriteLine("우클릭 메뉴: " + menu);
            Check("조회 화면 우클릭 메뉴 = Free Zoom / Fit Size / Move View Center / - / Center Cross", menu == "Free Zoom / Fit Size / Move View Center / - / Center Cross");
            CheckCenterCrossMenu("조회 화면", viewMap);
            CheckCenterCrossMenu("File Map Setup", setupMap);
            Button search = (Button)Field(viewType, view, "btnSearch");
            Check("조회 버튼 아이콘 16x16", search.Image != null && search.Image.Width == 16 && search.Image.Height == 16);
            Check("오른쪽 탭 3개", ((TabControl)Field(viewType, view, "tabRight")).TabPages.Count == 3);
            Check("처음에는 Map 없음", Dies(viewMap).Count == 0 && Field(viewType, view, "currentLayout") == null);

            DataTable leftBottomDef = null, leftBottomDies = null;
            foreach (string dir in Directions)
            {
                direction.SelectedItem = dir;
                Invoke(setupType, setup, "btnGenerate_Click");
                DataTable def = BuildSetupMapDef(assembly, setupType, setup, setupMap, "SMOKE_" + dir);
                DataTable dies = (DataTable)setupType.GetMethod("CreateMapDieTable", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(setup, null);
                ShowStoredMap(viewType, view, def, dies);

                Check(dir + " 화면 Map = LeftBottom", Prop(viewMap, "XYDirect").ToString() == "LeftBottom");
                CompareDies(dir + " 조회 화면 = File Map Setup 화면", setupMap, viewMap, null, null);
                CheckHoverHitTest(view, viewMap, dir);
                CheckAllMatch(viewType, view, dir);
                CheckCoordinateLabel(viewType, view, viewMap, def, dir);
                CheckDieNumbers(viewType, view, viewMap, setupMap, dir);
                CheckTabs(viewType, view, dies.Rows.Count, dir);
                CheckLegacyDraw(assembly, def, dies, viewMap, dir + " 조회 화면 = 기존 Map Define");
                if (dir == "LeftTop") SaveImages(view, viewMap, args[2]);
                if (dir == "LeftBottom") { leftBottomDef = def; leftBottomDies = dies; }
            }

            CheckDieNumberPaint(viewType, view, viewMap);
            CheckStoredDieNum(viewType, view, leftBottomDef, leftBottomDies, setupMap);
            CheckLegacyStyleMaps(assembly, viewType, view, viewMap);
            CheckErrorCases(viewType, view, viewMap, leftBottomDef, leftBottomDies);
            CheckMapList(viewType, view, viewMap);
            if (args.Length > 3) CheckDbData(viewType, view, viewMap, args[3]);
            view.Close();
            setup.Close();
        }
    }

    /// <summary>
    /// File Map Setup 화면이 저장할 값(MapCreationService.Create 인수)을 GET_MAPDEF 조회 결과 모양으로 만든다.
    /// GET_MAPDEF 는 ORIGIN_MICRO_Y 를 CHIP_SIZE_Y - 값 으로 돌려주므로 같은 계산을 한다. 숫자 컬럼은 Oracle NUMBER 처럼 decimal 이다.
    /// </summary>
    private static DataTable BuildSetupMapDef(Assembly assembly, Type setupType, Form setup, Control map, string mapId)
    {
        Type service = assembly.GetType("DACrux.TEST.ENGUI.MapCreationService", true);
        object stored = service.GetMethod("ToStoredValues", BindingFlags.Static | BindingFlags.Public)
            .Invoke(null, new object[] { map, Field(setupType, setup, "currentDirection"), Field(setupType, setup, "currentGrid") });
        double dieY = (double)Prop(map, "DieSizeY");
        DataTable def = NewMapDef();
        DataRow r = def.NewRow();
        r["MAPID"] = mapId;
        r["FACTORY"] = DBNull.Value;
        r["WAFER_SIZE"] = (decimal)(double)Prop(map, "WaferSize");
        r["CHIP_SIZE_X"] = (decimal)(double)Prop(map, "DieSizeX");
        r["CHIP_SIZE_Y"] = (decimal)dieY;
        r["ORIGIN_MICRO_X"] = (decimal)(double)Prop(stored, "OriginMicroX");
        r["ORIGIN_MICRO_Y"] = (decimal)dieY - (decimal)(double)Prop(stored, "OriginMicroY");
        r["ORIGIN_INDEX_X"] = (decimal)(int)Prop(stored, "OriginIndexX");
        r["ORIGIN_INDEX_Y"] = (decimal)(int)Prop(stored, "OriginIndexY");
        r["FIRST_INDEX_X"] = (decimal)(int)Prop(map, "FirstDieX");
        r["FIRST_INDEX_Y"] = (decimal)(int)Prop(map, "FirstDieY");
        r["EDGE_SIZE"] = (decimal)(double)Prop(stored, "EdgeSizeMm");
        r["ANGLE"] = (decimal)(int)Prop(map, "NotchAngle");
        r["NETDIE"] = (decimal)(int)Prop(map, "NetDie");
        r["NOTCH_TYPE"] = Prop(map, "NotchType").ToString() == "Notch" ? 0m : 1m;
        r["DIE_INDEX_MIN_X"] = (decimal)(int)Prop(map, "DieMinX");
        r["DIE_INDEX_MAX_X"] = (decimal)(int)Prop(map, "DieMaxX");
        r["DIE_INDEX_MIN_Y"] = (decimal)(int)Prop(map, "DieMinY");
        r["DIE_INDEX_MAX_Y"] = (decimal)(int)Prop(map, "DieMaxY");
        r["XY_DIRECTION"] = (decimal)(int)Prop(stored, "XYDirection");
        r["REFERENCEDIE_SETTING"] = 0m;
        r["DELETE_FLAG"] = "N";
        def.Rows.Add(r);
        return def;
    }

    private static DataTable NewMapDef()
    {
        DataTable def = new DataTable();
        def.Columns.Add("FACTORY", typeof(string));
        def.Columns.Add("MAPID", typeof(string));
        foreach (string c in new[] { "WAFER_SIZE", "CHIP_SIZE_X", "CHIP_SIZE_Y", "ORIGIN_MICRO_X", "ORIGIN_MICRO_Y", "ORIGIN_INDEX_X", "ORIGIN_INDEX_Y",
            "FIRST_MICRO_X", "FIRST_MICRO_Y", "FIRST_INDEX_X", "FIRST_INDEX_Y", "EDGE_SIZE", "ANGLE", "NETDIE", "NETSHOT", "NOTCH_TYPE",
            "DIE_INDEX_MIN_X", "DIE_INDEX_MAX_X", "DIE_INDEX_MIN_Y", "DIE_INDEX_MAX_Y", "XY_DIRECTION", "REFERENCEDIE_SETTING" })
            def.Columns.Add(c, typeof(decimal));
        foreach (string c in new[] { "DELETE_FLAG", "INSERT_TIME", "INSERT_USER", "UPDATE_TIME", "UPDATE_USER" })
            def.Columns.Add(c, typeof(string));
        return def;
    }

    private static void ShowStoredMap(Type viewType, Form view, DataTable def, DataTable dies)
    {
        viewType.GetMethod("ShowStoredMap", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, new object[] { def, dies });
    }

    /// <summary>
    /// 두 Map 컨트롤의 Die 를 (LeftBottom 번호, USECODE) -> 좌하단 좌표로 비교한다.
    /// 기준 Map 이 저장 방향 번호로 그려졌으면 (min / max 와 방향을 주면) LeftBottom 으로 뒤집어 비교한다.
    /// </summary>
    private static void CompareDies(string name, Control expectedMap, Control viewMap, string storedDir, int[] range)
    {
        Dictionary<string, string> expected = new Dictionary<string, string>();
        foreach (object die in Dies(expectedMap))
        {
            int x = (int)GetField(die, "IndexX"), y = (int)GetField(die, "IndexY");
            if (storedDir != null)
            {
                if (storedDir.StartsWith("Right")) x = range[0] + range[1] - x;
                if (storedDir.EndsWith("Top")) y = range[2] + range[3] - y;
            }
            expected[x + "," + y + "," + GetField(die, "DieProp")] = Pos(die);
        }
        int mismatch = 0;
        foreach (object die in Dies(viewMap))
        {
            string pos;
            if (!expected.TryGetValue(Key(die), out pos) || pos != Pos(die)) mismatch++;
        }
        Console.WriteLine(string.Format("{0}: Die {1} / {2} / 불일치 {3}", name, Dies(viewMap).Count, Dies(expectedMap).Count, mismatch));
        Check(name + " Die 수 / 위치 / USECODE", Dies(viewMap).Count == Dies(expectedMap).Count && Dies(viewMap).Count > 0 && mismatch == 0);
    }

    /// <summary>
    /// 마우스 위치 -> Die 판정 확인 (KlarfMapFormSmoke 와 같은 방식). 사용 Die 의 그려진 중심을 픽셀로 바꿔 OnMouseMove 를 흉내 내고
    /// Map 이 잡은 번호(m_iCurrentX / Y)가 그 Die 번호와 같은지 센다.
    /// </summary>
    private static void CheckHoverHitTest(Form form, Control map, string dir)
    {
        CheckHoverHitTest(form, map, dir, false);
    }

    /// <summary>insideOnly = true 면 Wafer 안(IsInsideWafer) 사용 Die 만 본다. 실제 DB Map 에는 Wafer 밖 사용 Die 가 있을 수 있어서다.</summary>
    private static void CheckHoverHitTest(Form form, Control map, string dir, bool insideOnly)
    {
        MethodInfo insideWafer = map.GetType().GetMethod("IsInsideWafer");
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
        double px = (double)Prop(map, "DieSizeX"), py = (double)Prop(map, "DieSizeY");
        int ok = 0, wrong = 0, miss = 0;
        foreach (object die in Dies(map))
        {
            if ((int)GetField(die, "DieProp") != 1) continue;
            if (insideOnly && !(bool)insideWafer.Invoke(map, new object[] { die })) continue;
            int ix = (int)GetField(die, "IndexX"), iy = (int)GetField(die, "IndexY");
            object cood = GetField(die, "DieCood");
            double x = (double)cood.GetType().GetField("X").GetValue(cood) - ox + px / 2;
            double y = (double)cood.GetType().GetField("Y").GetValue(cood) - oy + py / 2;
            PointF p = (PointF)view.Invoke(map, new object[] { x, y });
            curX.SetValue(map, -1); curY.SetValue(map, -1);
            move.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.None, 0, (int)Math.Round(p.X), (int)Math.Round(p.Y), 0) });
            int hx = (int)curX.GetValue(map), hy = (int)curY.GetValue(map);
            if (hx == ix && hy == iy) ok++;
            else if (hx == -1) miss++;
            else wrong++;
        }
        Console.WriteLine(string.Format("{0} hit-test: 일치 {1} / 다른 Die {2} / 못 잡음 {3}", dir, ok, wrong, miss));
        Check(dir + " 마우스 위치 Die = 그려진 Die", ok > 0 && wrong == 0 && miss == 0);
    }

    /// <summary>File Map Setup 저장값은 데이터 확인 항목이 모두 일치여야 한다 (Wafer 밖 사용 Die 포함 11항목).</summary>
    private static void CheckAllMatch(Type viewType, Form view, string dir)
    {
        object layout = Field(viewType, view, "currentLayout");
        IList checks = (IList)Prop(layout, "Checks");
        List<string> notMatch = new List<string>();
        foreach (object check in checks)
            if (Prop(check, "ResultText").ToString() != "일치") notMatch.Add(Prop(check, "Name") + "=" + Prop(check, "ResultText") + "(" + Prop(check, "StoredValue") + " / " + Prop(check, "DieValue") + ")");
        if (notMatch.Count > 0) Console.WriteLine("   " + string.Join(" | ", notMatch.ToArray()));
        Check(dir + " 데이터 확인 11항목 모두 일치", checks.Count == 11 && notMatch.Count == 0);
        Label status = (Label)Field(viewType, view, "lblStatus");
        Check(dir + " 상태줄 다름 0", status.Text.Contains("다름 0"));
    }

    /// <summary>좌표 표시: 사용 Die 하나를 좌표 이벤트에 넣어 저장 방향 번호(시험 코드에서 따로 계산) / LeftBottom 번호가 나오는지 본다.</summary>
    private static void CheckCoordinateLabel(Type viewType, Form view, Control map, DataTable def, string dir)
    {
        object target = null;
        foreach (object die in Dies(map)) if ((int)GetField(die, "DieProp") == 1) { target = die; break; }
        if (target == null) { Check(dir + " 좌표 표시용 사용 Die", false); return; }
        int lbx = (int)GetField(target, "IndexX"), lby = (int)GetField(target, "IndexY");
        int minX = (int)(decimal)def.Rows[0]["DIE_INDEX_MIN_X"], maxX = (int)(decimal)def.Rows[0]["DIE_INDEX_MAX_X"];
        int minY = (int)(decimal)def.Rows[0]["DIE_INDEX_MIN_Y"], maxY = (int)(decimal)def.Rows[0]["DIE_INDEX_MAX_Y"];
        int sx = dir.StartsWith("Right") ? minX + maxX - lbx : lbx;
        int sy = dir.EndsWith("Top") ? minY + maxY - lby : lby;
        viewType.GetMethod("m_wMap_OnChangeCurrentDie", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, new object[] { map, target });
        Type pointD = target.GetType().Assembly.GetType("DACrux.Base.PointD", true);
        object pt = Activator.CreateInstance(pointD);
        viewType.GetMethod("m_wMap_OnChangePosition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, new object[] { map, pt, pt });
        string text = ((Label)Field(viewType, view, "lblCoordinates")).Text;
        int number = ExpectedNumbers(viewMap: map, dir: dir)[new Point(lbx, lby)];
        string expected = string.Format("Die ({0}) X/Y: {1}, {2} / LeftBottom: {3}, {4} / USECODE: 1 (사용) / Die 번호: {5}", dir, sx, sy, lbx, lby, number);
        if (dir == "LeftTop") Console.WriteLine("좌표 표시: " + text.Replace("\r\n", " | "));
        Check(dir + " 좌표 표시 = 저장 번호 / LeftBottom 번호", text.Contains(expected));
    }

    /// <summary>
    /// 시험 코드가 따로 계산한 Direction 번호. Map 컨트롤 Die (LeftBottom 번호) 중 사용 Die 를 시작 모서리부터 행 우선으로 센다.
    /// LeftTop: 위 행(LB Y 큰 쪽)부터 왼쪽부터 / LeftBottom: 아래 행부터 왼쪽부터 / RightTop: 위 행부터 오른쪽부터 / RightBottom: 아래 행부터 오른쪽부터.
    /// </summary>
    private static Dictionary<Point, int> ExpectedNumbers(Control viewMap, string dir)
    {
        bool top = dir.EndsWith("Top"), right = dir.StartsWith("Right");
        List<Point> used = new List<Point>();
        foreach (object die in Dies(viewMap))
            if ((int)GetField(die, "DieProp") == 1) used.Add(new Point((int)GetField(die, "IndexX"), (int)GetField(die, "IndexY")));
        used.Sort(delegate (Point a, Point b)
        {
            int ay = top ? -a.Y : a.Y, by = top ? -b.Y : b.Y;
            if (ay != by) return ay.CompareTo(by);
            int ax = right ? -a.X : a.X, bx = right ? -b.X : b.X;
            return ax.CompareTo(bx);
        });
        Dictionary<Point, int> numbers = new Dictionary<Point, int>();
        for (int i = 0; i < used.Count; i++) numbers[used[i]] = i + 1;
        return numbers;
    }

    /// <summary>조회 화면 / File Map Setup 화면 Map 컨트롤의 DieNumbers 가 시험 계산과 같은지, 1번 위치가 시작 모서리인지 본다.</summary>
    private static void CheckDieNumbers(Type viewType, Form view, Control viewMap, Control setupMap, string dir)
    {
        Dictionary<Point, int> expected = ExpectedNumbers(viewMap, dir);
        foreach (KeyValuePair<string, Control> target in new[] { new KeyValuePair<string, Control>("조회 화면", viewMap), new KeyValuePair<string, Control>("File Map Setup", setupMap) })
        {
            Dictionary<Point, int> actual = (Dictionary<Point, int>)Prop(target.Value, "DieNumbers");
            int mismatch = 0;
            foreach (KeyValuePair<Point, int> pair in expected)
            {
                int value;
                if (!actual.TryGetValue(pair.Key, out value) || value != pair.Value) mismatch++;
            }
            Check(dir + " " + target.Key + " Die 번호 = 시작 모서리부터 행 우선 / 사용 Die " + expected.Count + "개",
                expected.Count > 0 && actual.Count == expected.Count && mismatch == 0 && (bool)Prop(target.Value, "ShowDieNumbers"));
        }
        // 1번 Die 가 시작 모서리 행에 있는지 (LB 번호 기준 범위로 따로 확인)
        Point first = Point.Empty;
        foreach (KeyValuePair<Point, int> pair in expected) if (pair.Value == 1) first = pair.Key;
        int topY = int.MinValue, bottomY = int.MaxValue;
        foreach (Point p in expected.Keys) { topY = Math.Max(topY, p.Y); bottomY = Math.Min(bottomY, p.Y); }
        int rowEdgeX = dir.StartsWith("Right") ? int.MinValue : int.MaxValue;
        int rowY = dir.EndsWith("Top") ? topY : bottomY;
        foreach (Point p in expected.Keys)
            if (p.Y == rowY) rowEdgeX = dir.StartsWith("Right") ? Math.Max(rowEdgeX, p.X) : Math.Min(rowEdgeX, p.X);
        Console.WriteLine(string.Format("{0} Die 번호 1 = LeftBottom ({1}, {2}) / 마지막 {3}", dir, first.X, first.Y, expected.Count));
        Check(dir + " Die 번호 1 = 시작 모서리 행의 끝 Die", first.Y == rowY && first.X == rowEdgeX);

        DataGridView dies = (DataGridView)Field(viewType, view, "grdDies");
        int numbered = 0;
        foreach (DataGridViewRow row in dies.Rows) if (row.Cells["colDieNumber"].Value != null && row.Cells["colDieNumber"].Value != DBNull.Value) numbered++;
        Check(dir + " Die 목록 Die 번호 칸 = 사용 Die 수", numbered == expected.Count);
    }

    /// <summary>번호를 켜고 끈 Map 그림을 비교한다 (번호를 덧그리면 픽셀이 바뀌어야 한다). 체크를 끄면 Map 컨트롤 표시도 꺼진다.</summary>
    private static void CheckDieNumberPaint(Type viewType, Form view, Control viewMap)
    {
        CheckBox number = (CheckBox)Field(viewType, view, "chkDieNumber");
        Check("Die 번호 표시 기본 켬", number.Checked);
        Bitmap on = (Bitmap)viewMap.GetType().GetMethod("GetMapImage").Invoke(viewMap, null);
        on = new Bitmap(on);
        number.Checked = false;
        Bitmap off = new Bitmap((Bitmap)viewMap.GetType().GetMethod("GetMapImage").Invoke(viewMap, null));
        int diff = 0;
        for (int x = 0; x < Math.Min(on.Width, off.Width); x++)
            for (int y = 0; y < Math.Min(on.Height, off.Height); y++)
                if (on.GetPixel(x, y) != off.GetPixel(x, y)) diff++;
        Console.WriteLine("Die 번호 켬 / 끔 그림 차이 픽셀: " + diff);
        Check("Die 번호를 끄면 Map 그림에서 번호가 빠짐", diff > 0 && !(bool)Prop(viewMap, "ShowDieNumbers"));
        number.Checked = true;
        Check("Die 번호 다시 켬", (bool)Prop(viewMap, "ShowDieNumbers"));
        on.Dispose();
        off.Dispose();
    }

    /// <summary>
    /// 저장 DIE_NUM 비교 항목. 실제 저장 서비스(DSL CreateUseMap)처럼 행 순서(i + 1)로 넣으면 참고,
    /// Direction 번호(사용 Die) 로 넣으면 일치여야 한다. DIE_NUM 컬럼이 없으면(앞 검사들) 항목이 없다.
    /// </summary>
    private static void CheckStoredDieNum(Type viewType, Form view, DataTable def, DataTable dies, Control setupMap)
    {
        DataTable saved = dies.Copy();
        saved.Columns.Add("DIE_NUM", typeof(decimal));
        for (int i = 0; i < saved.Rows.Count; i++) saved.Rows[i]["DIE_NUM"] = (decimal)(i + 1);
        ShowStoredMap(viewType, view, def, saved);
        Dictionary<string, string> results = Results(Field(viewType, view, "currentLayout"));
        Console.WriteLine("저장 순서 DIE_NUM: " + (results.ContainsKey("DIE_NUM 순서") ? results["DIE_NUM 순서"] : "항목 없음"));
        Check("저장 순서(i + 1) DIE_NUM -> DIE_NUM 순서 참고 (12항목)", results.Count == 12 && results["DIE_NUM 순서"] == "참고");

        // LeftBottom 저장값: 방향 번호 = LB 번호이므로 시험 계산 번호를 그대로 넣는다.
        Control viewMap = (Control)Field(viewType, view, "m_wMap");
        Dictionary<Point, int> expected = ExpectedNumbers(viewMap, "LeftBottom");
        DataTable direction = dies.Copy();
        direction.Columns.Add("DIE_NUM", typeof(decimal));
        int next = expected.Count;
        foreach (DataRow row in direction.Rows)
        {
            int value;
            Point key = new Point(Convert.ToInt32(row["X"]), Convert.ToInt32(row["Y"]));
            row["DIE_NUM"] = (decimal)(expected.TryGetValue(key, out value) && Convert.ToInt32(row["USECODE"]) == 1 ? value : ++next);
        }
        ShowStoredMap(viewType, view, def, direction);
        results = Results(Field(viewType, view, "currentLayout"));
        Check("Direction 번호로 저장한 DIE_NUM -> DIE_NUM 순서 일치", results["DIE_NUM 순서"] == "일치");
        ShowStoredMap(viewType, view, def, dies);
    }

    /// <summary>탭 내용: Map 정보 행 / 데이터 확인 행 / Die 목록 행 수와 탭 제목.</summary>
    private static void CheckTabs(Type viewType, Form view, int dieRows, string dir)
    {
        DataGridView info = (DataGridView)Field(viewType, view, "grdInfo");
        DataGridView check = (DataGridView)Field(viewType, view, "grdCheck");
        DataGridView dies = (DataGridView)Field(viewType, view, "grdDies");
        TabPage dieTab = (TabPage)Field(viewType, view, "tabDies");
        bool hasDirection = false;
        foreach (DataGridViewRow row in info.Rows)
            if (Convert.ToString(row.Cells[0].Value) == "XY 방향" && Convert.ToString(row.Cells[1].Value).StartsWith(dir)) hasDirection = true;
        Check(dir + " Map 정보 16행 / XY 방향 표시", info.Rows.Count == 16 && hasDirection);
        Check(dir + " 데이터 확인 11행", check.Rows.Count == 11);
        Check(dir + " Die 목록 행 = 저장 행 (" + dieRows + ")", dies.Rows.Count == dieRows && dieTab.Text.Contains(dieRows.ToString()));
    }

    /// <summary>
    /// 같은 저장값을 기존 Map Define 화면의 불러오기(frmSetupMap.DrawLoadingMap)로 그리고, 조회 화면 Die 와 위치를 비교한다.
    /// 기존 화면은 저장 방향 번호로 그리므로 LeftBottom 으로 뒤집어 비교한다. frmSetupMap 코드는 바꾸지 않는다.
    /// </summary>
    private static void CheckLegacyDraw(Assembly assembly, DataTable def, DataTable dies, Control viewMap, string name)
    {
        Type setupMapType = assembly.GetType("DACrux.TEST.ENGUI.frmSetupMap", true);
        using (SetupMapFixture legacy = new SetupMapFixture())
        {
            ShowOffScreen(legacy);
            string error = null;
            try { setupMapType.GetMethod("DrawLoadingMap", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(legacy, new object[] { def, dies }); }
            catch (TargetInvocationException ex) { error = ex.InnerException == null ? ex.Message : ex.InnerException.Message; }
            Check(name + " 기존 화면 불러오기 예외 없음" + (error == null ? "" : " (" + error + ")"), error == null);
            if (error == null)
            {
                Control loaded = (Control)setupMapType.GetField("m_wMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(legacy);
                DataRow r = def.Rows[0];
                int[] range = { (int)(decimal)r["DIE_INDEX_MIN_X"], (int)(decimal)r["DIE_INDEX_MAX_X"], (int)(decimal)r["DIE_INDEX_MIN_Y"], (int)(decimal)r["DIE_INDEX_MAX_Y"] };
                string dir = Enum.GetName(Prop(loaded, "XYDirect").GetType(), (int)(decimal)r["XY_DIRECTION"]);
                CompareDies(name, loaded, viewMap, dir, range);
            }
            legacy.Close();
        }
    }

    /// <summary>
    /// 기존 Map Define 방식처럼 격자 일부만 / USECODE 1, 0, 3 / 원점 인덱스가 격자 가운데인 Map 을 4방향으로 만들어
    /// 기존 화면과 같은 위치인지 / 마우스 판정 / 확인 결과(다름 0, 격자 채움과 USECODE 는 참고)를 본다.
    /// </summary>
    private static void CheckLegacyStyleMaps(Assembly assembly, Type viewType, Form view, Control viewMap)
    {
        for (int d = 0; d < Directions.Length; d++)
        {
            string dir = Directions[d];
            DataTable def = NewMapDef();
            DataRow r = def.NewRow();
            r["MAPID"] = "LEGACY_" + dir;
            r["FACTORY"] = "SCK";
            r["WAFER_SIZE"] = 200000m;
            r["CHIP_SIZE_X"] = 9000m;
            r["CHIP_SIZE_Y"] = 11000m;
            r["ORIGIN_MICRO_X"] = 1500m;
            r["ORIGIN_MICRO_Y"] = 2500m;
            r["ORIGIN_INDEX_X"] = 10m;
            r["ORIGIN_INDEX_Y"] = 8m;
            r["FIRST_INDEX_X"] = 1m;
            r["FIRST_INDEX_Y"] = 1m;
            r["EDGE_SIZE"] = 3m;
            r["ANGLE"] = 0m;
            r["NOTCH_TYPE"] = 0m;
            r["DIE_INDEX_MIN_X"] = 1m;
            r["DIE_INDEX_MAX_X"] = 20m;
            r["DIE_INDEX_MIN_Y"] = 1m;
            r["DIE_INDEX_MAX_Y"] = 16m;
            r["XY_DIRECTION"] = (decimal)(int)Enum.Parse(Prop(viewMap, "XYDirect").GetType(), dir);
            r["REFERENCEDIE_SETTING"] = 0m;
            DataTable dies = new DataTable();
            dies.Columns.Add("MAPID", typeof(string));
            dies.Columns.Add("X", typeof(decimal));
            dies.Columns.Add("Y", typeof(decimal));
            dies.Columns.Add("USECODE", typeof(decimal));
            int used = 0;
            for (int x = 1; x <= 20; x++)
                for (int y = 1; y <= 16; y++)
                {
                    if ((x + y) % 7 == 0) continue;
                    int code = (x * y) % 5 != 0 ? 1 : (x % 2 == 0 ? 0 : 3);
                    if (code == 1) used++;
                    dies.Rows.Add("LEGACY_" + dir, (decimal)x, (decimal)y, (decimal)code);
                }
            r["NETDIE"] = (decimal)used;
            def.Rows.Add(r);

            ShowStoredMap(viewType, view, def, dies);
            CheckHoverHitTest(view, viewMap, "기존 방식 " + dir);
            CheckLegacyDraw(assembly, def, dies, viewMap, "기존 방식 " + dir + " 조회 화면 = 기존 Map Define");
            object layout = Field(viewType, view, "currentLayout");
            Dictionary<string, string> results = Results(layout);
            Check("기존 방식 " + dir + " 다름 0 / 격자 채움 참고 / USECODE 참고",
                (int)Prop(layout, "DifferCount") == 0 && results["격자 채움"] == "참고" && results["USECODE 구성"] == "참고");
        }
    }

    /// <summary>LeftBottom 저장값을 일부러 바꿔 해당 항목만 다름이 되는지 본다. XY 방향 값이 틀리면 Map 을 그리지 않는다.</summary>
    private static void CheckErrorCases(Type viewType, Form view, Control viewMap, DataTable def, DataTable dies)
    {
        ExpectDiffer(viewType, view, "NETDIE + 1", Modify(def, "NETDIE", (decimal)def.Rows[0]["NETDIE"] + 1), dies, new[] { "Net Die" });
        TabControl tabs = (TabControl)Field(viewType, view, "tabRight");
        tabs.SelectedTab = (TabPage)Field(viewType, view, "tabCheck");
        SaveFormImage(view, Path.ChangeExtension(imagePath, ".check.png"));
        tabs.SelectedTab = (TabPage)Field(viewType, view, "tabDies");
        SaveFormImage(view, Path.ChangeExtension(imagePath, ".dies.png"));
        tabs.SelectedTab = (TabPage)Field(viewType, view, "tabInfo");

        DataTable empty = dies.Clone();
        ExpectDiffer(viewType, view, "Die 0행", def, empty, new[] { "Die 행 (TQP_USEMAP)", "Net Die", "Die 번호 범위" });

        DataTable duplicated = dies.Copy();
        DataRow removed = null;
        foreach (DataRow row in duplicated.Rows) if (Convert.ToInt32(row["USECODE"]) == -1) { removed = row; break; }
        duplicated.Rows.Add(removed.ItemArray);
        ExpectDiffer(viewType, view, "중복 행", def, duplicated, new[] { "중복 Die" });

        ExpectDiffer(viewType, view, "Edge µm 저장", Modify(def, "EDGE_SIZE", 3000m), dies, new[] { "Edge 단위 (mm)" });
        ExpectDiffer(viewType, view, "Wafer mm 저장", Modify(def, "WAFER_SIZE", 300m), dies, new[] { "Wafer 크기 단위 (µm)" });
        ExpectDiffer(viewType, view, "Die 크기 mm 저장", Modify(Modify(def, "CHIP_SIZE_X", 3.34m), "CHIP_SIZE_Y", 3.12m), dies, new[] { "Die 크기 단위 (µm)" });

        ExpectDiffer(viewType, view, "XY 방향 7", Modify(def, "XY_DIRECTION", 7m), dies, new[] { "XY 방향" });
        Check("XY 방향 7 이면 Map 을 그리지 않음", Dies(viewMap).Count == 0);
        DataGridView grid = (DataGridView)Field(viewType, view, "grdCheck");
        bool red = false;
        foreach (DataGridViewRow row in grid.Rows)
            if (Convert.ToString(row.Cells[3].Value) == "다름" && row.Cells[3].Style.ForeColor == Color.Red) red = true;
        Check("다름 결과는 빨간 글자", red);

        ShowStoredMap(viewType, view, def, dies);
        Check("정상 값으로 되돌리면 다름 0", (int)Prop(Field(viewType, view, "currentLayout"), "DifferCount") == 0
            && ((TabPage)Field(viewType, view, "tabCheck")).Text == "데이터 확인");
    }

    private static void ExpectDiffer(Type viewType, Form view, string name, DataTable def, DataTable dies, string[] expected)
    {
        ShowStoredMap(viewType, view, def, dies);
        object layout = Field(viewType, view, "currentLayout");
        List<string> differ = new List<string>();
        foreach (KeyValuePair<string, string> pair in Results(layout)) if (pair.Value == "다름") differ.Add(pair.Key);
        differ.Sort();
        List<string> want = new List<string>(expected);
        want.Sort();
        string tab = ((TabPage)Field(viewType, view, "tabCheck")).Text;
        Console.WriteLine(string.Format("{0}: 다름 = {1} / 탭 = {2}", name, string.Join(", ", differ.ToArray()), tab));
        Check(name + " -> 다름 항목 " + string.Join(", ", expected), string.Join("|", differ.ToArray()) == string.Join("|", want.ToArray())
            && tab == "데이터 확인 (다름 " + expected.Length + ")");
    }

    private static DataTable Modify(DataTable def, string column, decimal value)
    {
        DataTable copy = def.Copy();
        copy.Rows[0][column] = value;
        return copy;
    }

    private static Dictionary<string, string> Results(object layout)
    {
        Dictionary<string, string> results = new Dictionary<string, string>();
        foreach (object check in (IList)Prop(layout, "Checks")) results[Prop(check, "Name").ToString()] = Prop(check, "ResultText").ToString();
        return results;
    }

    /// <summary>
    /// Map 목록: GET_MAPDEF_ALL 모양 표를 넣어 표시 컬럼 / 건수 / 검색을 본다. 목록을 묶으면 보던 Map 은 지우고 자동 선택하지 않는다
    /// (자동 선택되면 원격 조회가 일어나므로 이 검사에서 예외가 난다).
    /// </summary>
    private static void CheckMapList(Type viewType, Form view, Control viewMap)
    {
        DataTable list = new DataTable();
        list.Columns.Add("FACTORY", typeof(string));
        list.Columns.Add("MAPID", typeof(string));
        list.Columns.Add("WAFER_SIZE", typeof(decimal));
        list.Columns.Add("NETDIE", typeof(decimal));
        list.Columns.Add("XY_DIRECTION", typeof(decimal));
        list.Columns.Add("INSERT_TIME", typeof(string));
        list.Rows.Add("SCK", "U9_HYBRID", 300000m, 76m, 0m, DBNull.Value);
        list.Rows.Add("SCK", "u9_omm", 300000m, 79m, 1m, "2026/10/13 10:00:00");
        list.Rows.Add(DBNull.Value, "OMM[1]_TEST%", 200000m, 10m, 2m, DBNull.Value);
        list.Rows.Add("SCK", "ZZZ", 300000m, 5m, 9m, DBNull.Value);

        viewType.GetMethod("ShowMapList", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, new object[] { list, null });
        Application.DoEvents();
        DataGridView grid = (DataGridView)Field(viewType, view, "grdMapList");
        Label count = (Label)Field(viewType, view, "lblListCount");
        TextBox search = (TextBox)Field(viewType, view, "txtSearch");
        Check("목록 4행 / 건수 4 / 4", grid.Rows.Count == 4 && count.Text == "4 / 4 건");
        Check("목록 컬럼 Device / Wafer mm / Net Die / XY 이름",
            Convert.ToString(grid.Rows[0].Cells[0].Value) == "U9_HYBRID" && Convert.ToString(grid.Rows[0].Cells[1].Value) == "300"
            && Convert.ToString(grid.Rows[0].Cells[2].Value) == "76" && Convert.ToString(grid.Rows[0].Cells[3].Value) == "LeftTop"
            && Convert.ToString(grid.Rows[2].Cells[3].Value) == "RightBottom" && Convert.ToString(grid.Rows[3].Cells[3].Value) == "9");
        Check("목록을 묶으면 Map 비움 / 자동 선택 없음", Field(viewType, view, "currentLayout") == null && Dies(viewMap).Count == 0
            && grid.SelectedRows.Count == 0);

        string[,] cases = { { "u9", "2" }, { "U9_H", "1" }, { "omm[1", "1" }, { "%", "1" }, { "'", "0" }, { "*", "0" }, { "", "4" } };
        for (int i = 0; i < cases.GetLength(0); i++)
        {
            search.Text = cases[i, 0];
            Check("검색 '" + cases[i, 0] + "' -> " + cases[i, 1] + "건", grid.Rows.Count == int.Parse(cases[i, 1]) && count.Text == cases[i, 1] + " / 4 건");
        }
        Check("검색 후에도 자동 선택 없음", grid.SelectedRows.Count == 0 && Field(viewType, view, "currentLayout") == null);
        SaveFormImage(view, Path.ChangeExtension(imagePath, ".list.png"));
    }

    /// <summary>
    /// DB 확인 (StandardMapViewDbProbe.ps1 이 저장한 GET_MAPDEF_ALL / GET_MAPDEF / SELECT_USEMAP_ALLDIE 결과, Oracle 형식 그대로).
    /// 합격 기준: 목록 건수 = 목록 행 수 / Map 마다 예외 없이 표시 / Wafer 안 사용 Die 의 마우스 판정 모두 일치.
    /// 데이터 확인 결과(다름 / 참고)는 실제 데이터 상태이므로 합격 기준에 넣지 않고 출력만 한다.
    /// </summary>
    private static void CheckDbData(Type viewType, Form view, Control viewMap, string dir)
    {
        DataTable list = ReadXml(Path.Combine(dir, "list.xml"));
        viewType.GetMethod("ShowMapList", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, new object[] { list, null });
        Application.DoEvents();
        Label count = (Label)Field(viewType, view, "lblListCount");
        Console.WriteLine(string.Format("DB 목록 컬럼 형식: WAFER_SIZE={0} / NETDIE={1} / XY_DIRECTION={2}",
            TypeName(list, "WAFER_SIZE"), TypeName(list, "NETDIE"), TypeName(list, "XY_DIRECTION")));
        Check("DB 목록 건수 " + list.Rows.Count, count.Text == list.Rows.Count + " / " + list.Rows.Count + " 건");

        string[] files = Directory.GetFiles(dir, "map*_def.xml");
        Array.Sort(files, StringComparer.Ordinal);
        foreach (string file in files)
        {
            DataTable def = ReadXml(file);
            DataTable dies = ReadXml(file.Replace("_def.xml", "_dies.xml"));
            string mapId = def.Rows.Count > 0 ? Convert.ToString(def.Rows[0]["MAPID"]) : Path.GetFileName(file);
            string error = null;
            try { ShowStoredMap(viewType, view, def, dies); }
            catch (TargetInvocationException ex) { error = ex.InnerException == null ? ex.Message : ex.InnerException.Message; }
            Check("DB " + mapId + " 표시 예외 없음" + (error == null ? "" : " (" + error + ")"), error == null);
            if (error != null) continue;

            object layout = Field(viewType, view, "currentLayout");
            List<string> differ = new List<string>(), note = new List<string>();
            foreach (object check in (IList)Prop(layout, "Checks"))
            {
                string result = Prop(check, "ResultText").ToString();
                string text = Prop(check, "Name") + "(" + Prop(check, "StoredValue") + " / " + Prop(check, "DieValue") + ")";
                if (result == "다름") differ.Add(text);
                else if (result == "참고") note.Add(text);
            }
            Console.WriteLine(string.Format("DB {0}: Die 행 {1} / 사용 {2} / XY {3} / 다름 {4} / 참고 {5}",
                mapId, ((IList)Prop(layout, "Dies")).Count, Prop(layout, "UsedCount"), Prop(layout, "Direction"), differ.Count, note.Count));
            foreach (string text in differ) Console.WriteLine("   다름: " + text);
            foreach (string text in note) Console.WriteLine("   참고: " + text);
            // Die 크기가 mm 로 저장된 Map 은 Die 가 1 픽셀보다 작아 마우스로 잡을 수 없으므로 판정하지 않는다 (데이터 확인에서 다름으로 표시됨).
            if (Results(layout)["Die 크기 단위 (µm)"] == "다름") Console.WriteLine("DB " + mapId + " hit-test: 생략 (Die 크기 단위 다름)");
            else if (Dies(viewMap).Count > 0) CheckHoverHitTest(view, viewMap, "DB " + mapId, true);
        }
    }

    private static DataTable ReadXml(string path)
    {
        DataTable table = new DataTable();
        table.ReadXml(path);
        return table;
    }

    private static string TypeName(DataTable table, string column)
    {
        return table.Columns.Contains(column) ? table.Columns[column].DataType.Name : "-";
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

    private static void SaveFormImage(Form form, string png)
    {
        form.PerformLayout();
        Application.DoEvents();
        using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
            bitmap.Save(png);
        }
        Console.WriteLine("이미지: " + png);
    }

    private static void ShowOffScreen(Form form)
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-32000, -32000);
        form.ShowInTaskbar = false;
        form.Show();
        Application.DoEvents();
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

    /// <summary>
    /// 우클릭 [Center Cross] (2026-10-13): 기본 켬(Designer) / 메뉴 체크 = 현재 값 / 누르면 끔 -> 다시 누르면 켬.
    /// 끝나면 처음 상태(켬)로 돌아간다.
    /// </summary>
    private static void CheckCenterCrossMenu(string name, Control map)
    {
        Type waferMap = map.GetType();
        while (waferMap != null && waferMap.Name != "WaferMap") waferMap = waferMap.BaseType;
        ContextMenu menu = (ContextMenu)waferMap.GetField("ctxmWaferMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(map);
        MenuItem cross = null;
        foreach (MenuItem item in menu.MenuItems) if (item.Text == "Center Cross") cross = item;
        ShownMenu(map);
        Check(name + " 십자선 기본 켬 / 메뉴 체크", cross != null && (bool)Prop(map, "CenterMark") && cross.Checked);
        if (cross == null) return;
        cross.PerformClick();
        ShownMenu(map);
        Check(name + " [Center Cross] 누르면 끔 / 체크 해제", !(bool)Prop(map, "CenterMark") && !cross.Checked);
        cross.PerformClick();
        ShownMenu(map);
        Check(name + " 다시 누르면 켬", (bool)Prop(map, "CenterMark") && cross.Checked);
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

    /// <summary>조회 화면: Load 이벤트(Map 목록 원격 조회)와 접속 이력만 막는다. Designer / 컨트롤 / 표시 코드는 그대로 실행한다.</summary>
    private sealed class ViewFixture : DACrux.TEST.ENGUI.frmStandardMapView
    {
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }

    /// <summary>File Map Setup 화면: 확인창은 "예" 로 답하고 접속 이력만 막는다 (KlarfMapFormSmoke 와 같은 방식).</summary>
    private sealed class SetupFixture : DACrux.TEST.ENGUI.frmStandardMapSetup
    {
        protected override bool AskYesNo(string text, string title) { return true; }
        protected override void ShowInformation(string text, string title) { }
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }

    /// <summary>기존 Map Define 화면: Load 이벤트(Map 목록 원격 조회 / 초기화)와 접속 이력만 막는다.</summary>
    private sealed class SetupMapFixture : DACrux.TEST.ENGUI.frmSetupMap
    {
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }
}
