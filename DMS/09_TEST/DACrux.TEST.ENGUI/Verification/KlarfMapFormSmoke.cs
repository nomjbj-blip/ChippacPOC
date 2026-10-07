using System;
using System.Collections;
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
            Check("기본 XY = LeftBottom", direction.Text == "LeftBottom");
            Check("기본 Wafer = 300", wafer.Text == "300");
            Check("격자 Die 108 (12 x 9)", Dies(map).Count == 108);
            Check("WaferSize 300000 µm", (double)Prop(map, "WaferSize") == 300000d);

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
                int used = CheckGeometry(map, dir);
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

            edge.Value = 5000;
            Invoke(type, form, "btnGenerate_Click");
            int edgeUsed = CountUsed(map);
            Console.WriteLine("Edge 5000 µm 사용 Die " + edgeUsed);
            Check("Edge 5000 이면 사용 Die 감소", edgeUsed < lbUsed);
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
    /// 모든 Die 의 좌하단(DieCood)이 KLARF 물리 위치와 같은지 확인하고 사용 Die 수를 돌려준다.
    /// LeftBottom 번호는 화면 Rule 과 별도로 여기서 다시 계산한다 (Right 는 X, Top 은 Y 를 격자 범위에서 뒤집음).
    /// 검사 Die 26개(샘플 SINF 위치를 LeftBottom 으로 옮긴 값)가 모두 사용 Die 인지도 본다.
    /// </summary>
    private static int CheckGeometry(Control map, string dir)
    {
        int minX = (int)Prop(map, "DieMinX"), maxX = (int)Prop(map, "DieMaxX");
        int minY = (int)Prop(map, "DieMinY"), maxY = (int)Prop(map, "DieMaxY");
        int bad = 0, used = 0, inspectedUsed = 0;
        foreach (object die in Dies(map))
        {
            int ix = (int)GetField(die, "IndexX"), iy = (int)GetField(die, "IndexY");
            int lbx = dir.StartsWith("Right") ? minX + maxX - ix : ix;
            int lby = dir.EndsWith("Top") ? minY + maxY - iy : iy;
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
        return used;
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
    /// </summary>
    private static void CheckStoredValues(Assembly assembly, Type formType, Form form, Control map, NumericUpDown edge)
    {
        Check("Edge 입력 최대 5000 µm", edge.Maximum == 5000m);
        edge.Value = 200;
        Invoke(formType, form, "btnGenerate_Click");
        Type service = assembly.GetType("DACrux.TEST.ENGUI.MapCreationService", true);
        object stored = service.GetMethod("ToStoredValues", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { map });
        double edgeMm = (double)Prop(stored, "EdgeSizeMm");
        double ox = (double)Prop(stored, "OriginMicroX");
        double oy = (double)Prop(stored, "OriginMicroY");
        double mapOriginX = (double)Prop(map, "OriginX"), mapOriginY = (double)Prop(map, "OriginY");
        Console.WriteLine(string.Format("저장값: EDGE {0} mm / ORIGIN_MICRO {1}, {2}", edgeMm, ox, oy));
        Check("EDGE_SIZE 0.2 mm", Math.Abs(edgeMm - 0.2) < 1e-9);
        Check("ORIGIN_MICRO_X = -29", Math.Abs(ox - mapOriginX) < 1e-9 && Math.Abs(ox + 29) < 1e-9);
        Check("ORIGIN_MICRO_Y = 14869.1", Math.Abs(oy - 14869.1) < 1e-6);
        Check("조회 규칙으로 되돌린 Y = 화면 OriginY", Math.Abs((PitchY - oy) - mapOriginY) < 1e-6);
        CheckMapDefineReadBack(assembly, formType, form, map, edgeMm, ox, oy);
        edge.Value = 0;
        Invoke(formType, form, "btnGenerate_Click");
    }

    /// <summary>
    /// 기존 Map Define 화면(frmSetupMap)의 불러오기 함수 DrawLoadingMap 에 GET_MAPDEF 결과와 같은 모양의 DataTable 을 넣는다.
    /// GET_MAPDEF 는 ORIGIN_MICRO_Y 를 CHIP_SIZE_Y - 값 으로 돌려주므로 그 계산도 여기서 똑같이 한다. 숫자 컬럼은 Oracle NUMBER 처럼 decimal 이다.
    /// 확인: 불러오기 중 예외가 없는가 (Edge 범위 오류 재발 방지) / 모든 Die 위치와 USECODE 가 File Map Setup 과 같은가.
    /// frmSetupMap 코드는 바꾸지 않고, 접속 이력 / 목록 조회(Load 이벤트)만 막은 하위 클래스로 실행한다.
    /// </summary>
    private static void CheckMapDefineReadBack(Assembly assembly, Type formType, Form form, Control map, double edgeMm, double originMicroX, double originMicroY)
    {
        System.Data.DataTable def = new System.Data.DataTable();
        string[] cols = { "WAFER_SIZE", "ANGLE", "NOTCH_TYPE", "EDGE_SIZE", "CHIP_SIZE_X", "CHIP_SIZE_Y", "ORIGIN_MICRO_X", "ORIGIN_MICRO_Y",
            "DIE_INDEX_MIN_X", "DIE_INDEX_MAX_X", "DIE_INDEX_MIN_Y", "DIE_INDEX_MAX_Y", "ORIGIN_INDEX_X", "ORIGIN_INDEX_Y",
            "FIRST_INDEX_X", "FIRST_INDEX_Y", "XY_DIRECTION", "REFERENCEDIE_SETTING" };
        foreach (string c in cols) def.Columns.Add(c, typeof(decimal));
        System.Data.DataRow r = def.NewRow();
        r["WAFER_SIZE"] = (decimal)(double)Prop(map, "WaferSize");
        r["ANGLE"] = (decimal)(int)Prop(map, "NotchAngle");
        r["NOTCH_TYPE"] = Prop(map, "NotchType").ToString() == "Notch" ? 0m : 1m;
        r["EDGE_SIZE"] = (decimal)edgeMm;
        r["CHIP_SIZE_X"] = (decimal)PitchX;
        r["CHIP_SIZE_Y"] = (decimal)PitchY;
        r["ORIGIN_MICRO_X"] = (decimal)originMicroX;
        r["ORIGIN_MICRO_Y"] = (decimal)PitchY - (decimal)originMicroY;
        r["DIE_INDEX_MIN_X"] = (decimal)(int)Prop(map, "DieMinX");
        r["DIE_INDEX_MAX_X"] = (decimal)(int)Prop(map, "DieMaxX");
        r["DIE_INDEX_MIN_Y"] = (decimal)(int)Prop(map, "DieMinY");
        r["DIE_INDEX_MAX_Y"] = (decimal)(int)Prop(map, "DieMaxY");
        r["ORIGIN_INDEX_X"] = (decimal)(int)Prop(map, "OriginIndexX");
        r["ORIGIN_INDEX_Y"] = (decimal)(int)Prop(map, "OriginIndexY");
        r["FIRST_INDEX_X"] = (decimal)(int)Prop(map, "FirstDieX");
        r["FIRST_INDEX_Y"] = (decimal)(int)Prop(map, "FirstDieY");
        r["XY_DIRECTION"] = (decimal)(int)Prop(map, "XYDirect");
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

            // 원인 재현: 수정 전 저장값(EDGE_SIZE 200, µm 그대로)이면 Map Define 불러오기가 실패해야 한다.
            System.Data.DataTable oldDef = def.Copy();
            oldDef.Rows[0]["EDGE_SIZE"] = 200m;
            string oldError = null;
            try { draw.Invoke(setup, new object[] { oldDef, dies }); }
            catch (TargetInvocationException ex) { oldError = ex.InnerException == null ? ex.Message : ex.InnerException.GetType().Name; }
            Console.WriteLine("수정 전 값(EDGE 200) 불러오기: " + (oldError ?? "예외 없음"));
            Check("수정 전 값은 Map Define 에서 실패 (원인 재현)", oldError != null);

            string error = null;
            try { draw.Invoke(setup, new object[] { def, dies }); }
            catch (TargetInvocationException ex) { error = ex.InnerException == null ? ex.Message : ex.InnerException.Message; }
            Check("Map Define 불러오기 예외 없음" + (error == null ? "" : " (" + error + ")"), error == null);
            if (error != null) return;

            Control loaded = (Control)setupType.GetField("m_wMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(setup);
            System.Collections.Generic.Dictionary<string, string> expected = new System.Collections.Generic.Dictionary<string, string>();
            foreach (object die in Dies(map)) expected[Key(die)] = Pos(die);
            int mismatch = 0;
            foreach (object die in Dies(loaded))
            {
                string pos;
                if (!expected.TryGetValue(Key(die), out pos) || pos != Pos(die)) mismatch++;
            }
            Console.WriteLine(string.Format("Map Define 불러오기: Die {0} / 불일치 {1} / OriginY {2}", Dies(loaded).Count, mismatch, Prop(loaded, "OriginY")));
            Check("Map Define Die 수 같음", Dies(loaded).Count == Dies(map).Count);
            Check("Map Define Die 위치 / USECODE 같음", mismatch == 0);
            setup.Close();
        }
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
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }
}
