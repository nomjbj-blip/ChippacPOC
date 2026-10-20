using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// POC 화면 버튼 / 배치 기준 검사 (2026-10-16).
/// 기준 문서: 문서/설계/06_환경_운영/2026-10-16_POC_화면_버튼_아이콘_레이아웃_기준.md
/// 실제 빌드된 화면을 화면 밖에서 띄워(Load / Activated 처리는 건너뜀) 실행 크기로 아래를 확인한다.
///  1) 보이는 버튼(Button / 버튼 모양 RadioButton / CheckBox)마다 16x16 아이콘이 있고, 그 그림이 버튼 아이콘 목록(BTN_*.png)에 있는지
///  2) 버튼 높이가 기준(28 ± 2)인지, 아이콘 + 글자가 버튼 폭 안에 들어가는지(글자 잘림)
///  3) 같은 부모 안에서 보이는 컨트롤끼리 겹치지 않는지, 부모 밖으로 나가지 않는지
/// 인자: [0] 버튼 아이콘 폴더(99_Resource/ByModule/POC/Button), [1] 검사 목록 파일(줄마다 "DLL 또는 EXE 경로|형식 이름"), [2] 결과 파일,
///       [3] 화면 그림 폴더(선택, 화면마다 <형식 이름>.png. OpenGL 도면 영역은 검게 나온다)
/// 결과: 화면별 표 + 마지막 줄 POC_BUTTON_LAYOUT=PASS / FAIL (n)
/// </summary>
internal static class PocButtonLayoutCheck
{
    private const int StandardHeight = 28;
    private const int HeightTolerance = 2;
    private static readonly Dictionary<string, string> IconKeys = new Dictionary<string, string>();
    private static readonly List<string> SearchDirs = new List<string>();
    private static int failCount;
    private static StringBuilder report = new StringBuilder();
    private static string imageDir;

    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); }
        catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        if (args.Length > 3) { imageDir = args[3]; Directory.CreateDirectory(imageDir); }
        AppDomain.CurrentDomain.AssemblyResolve += ResolveFromSearchDirs;
        foreach (string png in Directory.GetFiles(args[0], "BTN_*.png"))
        {
            using (var bmp = new Bitmap(png))
                IconKeys[PixelKey(bmp)] = Path.GetFileNameWithoutExtension(png).Substring(4);
        }
        Line("버튼 아이콘 목록: " + IconKeys.Count + " 개 (" + args[0] + ")");

        foreach (string raw in File.ReadAllLines(args[1], Encoding.UTF8))
        {
            string entry = raw.Trim();
            if (entry.Length == 0 || entry.StartsWith("#")) continue;
            string[] part = entry.Split('|');
            CheckForm(Path.GetFullPath(part[0]), part[1].Trim());
        }

        Line("");
        Line(failCount == 0 ? "POC_BUTTON_LAYOUT=PASS" : "POC_BUTTON_LAYOUT=FAIL (" + failCount + ")");
        File.WriteAllText(args[2], report.ToString(), new UTF8Encoding(true));
        return failCount == 0 ? 0 : 1;
    }

    /// <summary>화면 하나를 띄워 버튼과 배치를 검사한다. Load 처리(서버 조회 / 접속 이력)는 대신 만든 하위 형식에서 막는다.</summary>
    private static void CheckForm(string assemblyPath, string typeName)
    {
        Line("");
        Line("## " + typeName + "  (" + Path.GetFileName(assemblyPath) + ")");
        string dir = Path.GetDirectoryName(assemblyPath);
        if (!SearchDirs.Contains(dir)) SearchDirs.Add(dir);
        Type type = Assembly.LoadFrom(assemblyPath).GetType(typeName, true);
        Form form;
        try { form = CreateWithoutLoad(type); }
        catch (Exception ex) { Fail(typeName, "화면 생성 실패: " + (ex.InnerException ?? ex).Message); return; }

        using (form)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.WindowState = FormWindowState.Normal;
            form.Show();
            Application.DoEvents();
            Line(string.Format("실행 크기 {0}x{1} / 글꼴 {2} {3} / 설계 단위 {4} -> 실행 {5}",
                form.ClientSize.Width, form.ClientSize.Height, form.Font.Name, form.Font.SizeInPoints,
                form.AutoScaleDimensions, form.CurrentAutoScaleDimensions));
            Line("| 버튼 | 글자 | 위치 | 크기 | 아이콘 | 판정 |");
            Line("|---|---|---|---|---|---|");
            Walk(form, form, typeName);
            CheckOverlap(form, typeName);
            if (imageDir != null) SaveImage(form, Path.Combine(imageDir, typeName.Substring(typeName.LastIndexOf('.') + 1) + ".png"));
            form.Hide();
        }
    }

    private static void Walk(Control root, Control parent, string formName)
    {
        foreach (Control c in parent.Controls)
        {
            if (IsButton(c)) CheckButton(root, (ButtonBase)c, formName);
            if (c.Controls.Count > 0) Walk(root, c, formName);
        }
    }

    private static bool IsButton(Control c)
    {
        if (c is Button) return true;
        var rb = c as RadioButton;
        if (rb != null) return rb.Appearance == Appearance.Button;
        var cb = c as CheckBox;
        return cb != null && cb.Appearance == Appearance.Button;
    }

    /// <summary>
    /// 버튼 하나: 아이콘 유무 / 크기 / 목록 KEY, 높이, 폭(글자 + 아이콘 + 여백이 들어가는지)을 본다.
    /// 숨김 버튼(설계상 Visible=false)과 탭의 보이지 않는 페이지 버튼도 같은 기준으로 본다(탭을 바꾸면 보이므로).
    /// </summary>
    private static void CheckButton(Control root, ButtonBase b, string formName)
    {
        var problems = new List<string>();
        string key = "-";
        if (b.Image == null) problems.Add("아이콘 없음");
        else
        {
            if (b.Image.Width != 16 || b.Image.Height != 16) problems.Add("아이콘 " + b.Image.Width + "x" + b.Image.Height);
            if (!IconKeys.TryGetValue(PixelKey(b.Image), out key)) { key = "목록 밖"; problems.Add("목록 밖 아이콘"); }
            if (b.TextImageRelation != TextImageRelation.ImageBeforeText && b.Text.Trim().Length > 0)
                problems.Add("TextImageRelation=" + b.TextImageRelation);
        }
        if (Math.Abs(b.Height - StandardHeight) > HeightTolerance) problems.Add("높이 " + b.Height);
        int need = NeedWidth(b);
        if (need > b.Width) problems.Add("글자 잘림(필요 " + need + ")");
        Point p = root.PointToClient(b.PointToScreen(Point.Empty));
        string hidden = IsDesignVisible(b) ? "" : " (숨김)";
        Line(string.Format("| {0}{1} | {2} | {3},{4} | {5}x{6} | {7} | {8} |", b.Name, hidden, b.Text.Replace("\r\n", " "),
            p.X, p.Y, b.Width, b.Height, key, problems.Count == 0 ? "OK" : string.Join(" / ", problems.ToArray())));
        if (problems.Count > 0) failCount++;
    }

    /// <summary>아이콘(16) + 간격 4 + 글자 폭 + 좌우 여백(테두리 포함 12) 이 들어가야 하는 폭.</summary>
    private static int NeedWidth(ButtonBase b)
    {
        string text = b.Text.Trim();
        int textWidth = text.Length == 0 ? 0 : TextRenderer.MeasureText(text, b.Font).Width;
        int image = b.Image == null ? 0 : b.Image.Width + (text.Length == 0 ? 0 : 4);
        return textWidth + image + b.Padding.Horizontal + 12;
    }

    /// <summary>설계상 보이는 컨트롤인지(부모 쪽 Visible=false 포함). 탭 페이지는 선택 여부와 관계없이 보이는 것으로 본다.</summary>
    private static bool IsDesignVisible(Control c)
    {
        for (Control x = c; x != null; x = x.Parent)
        {
            if (x is TabPage) continue;
            if (!GetVisibleState(x)) return false;
        }
        return true;
    }

    private static bool GetVisibleState(Control c)
    {
        MethodInfo m = typeof(Control).GetMethod("GetState", BindingFlags.Instance | BindingFlags.NonPublic);
        return (bool)m.Invoke(c, new object[] { 2 });   // STATE_VISIBLE = 0x02 (부모가 숨어도 자기 Visible 값)
    }

    /// <summary>같은 부모 안의 보이는 컨트롤끼리 겹침 / 부모 밖으로 나감을 찾는다. Dock 된 컨트롤은 배치가 정해져 있어 뺀다.</summary>
    private static void CheckOverlap(Control parent, string formName)
    {
        var items = parent.Controls.Cast<Control>()
            .Where(c => c.Dock == DockStyle.None && IsDesignVisible(c) && !(c is TabPage) && !(parent is SplitterPanel && c is Splitter))
            .ToList();
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                Rectangle r = Rectangle.Intersect(items[i].Bounds, items[j].Bounds);
                if (r.Width > 1 && r.Height > 1)
                    Fail(formName, string.Format("겹침: {0}.{1} {2} / {3} {4}", parent.Name, items[i].Name, items[i].Bounds, items[j].Name, items[j].Bounds));
            }
            Rectangle area = parent.DisplayRectangle;
            Rectangle b = items[i].Bounds;
            bool scrolls = parent is ScrollableControl && ((ScrollableControl)parent).AutoScroll;
            if (!scrolls && !(parent is FlowLayoutPanel) && !(parent is TableLayoutPanel) && area.Width > 0 && area.Height > 0
                && (b.Right > area.Right + 1 || b.Bottom > area.Bottom + 1 || b.Left < area.Left - 1 || b.Top < area.Top - 1))
                Fail(formName, string.Format("부모 밖: {0}.{1} {2} (부모 {3})", parent.Name, items[i].Name, b, area));
        }
        foreach (Control c in parent.Controls)
            if (c.Controls.Count > 0 && IsDesignVisible(c) && !IsUserControlInside(c)) CheckOverlap(c, formName);
    }

    /// <summary>DataGridView / 콤보 / 숫자 입력 같은 기본 컨트롤 내부 자식은 검사하지 않는다.</summary>
    private static bool IsUserControlInside(Control c)
    {
        return c is DataGridView || c is ComboBox || c is NumericUpDown || c is PropertyGrid || c is TextBoxBase;
    }

    private static void SaveImage(Form form, string path)
    {
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp.Save(path, ImageFormat.Png);
        }
    }

    private static void Fail(string formName, string message)
    {
        failCount++;
        Line("- FAIL " + message);
    }

    private static void Line(string s)
    {
        Console.WriteLine(s);
        report.AppendLine(s);
    }

    /// <summary>
    /// 그림 비교 키 = 크기 + 픽셀 값(투명 픽셀은 색을 무시). resx 저장 / 읽기에서 같은 그림이면 같다.
    /// 그림을 다시 그려 복사하면 반투명 가장자리 픽셀 값이 바뀌므로 원래 Bitmap 의 픽셀을 그대로 읽는다.
    /// </summary>
    private static string PixelKey(Image image)
    {
        Bitmap bmp = image as Bitmap;
        bool copied = bmp == null;
        if (copied) bmp = new Bitmap(image);
        try
        {
            var sb = new StringBuilder(bmp.Width + "x" + bmp.Height + ":");
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    sb.Append(c.A == 0 ? "0" : c.ToArgb().ToString("X8")).Append(',');
                }
            return sb.ToString();
        }
        finally { if (copied) bmp.Dispose(); }
    }

    /// <summary>
    /// 화면 형식을 상속한 형식을 실행 중에 만들어 OnLoad / OnActivated / OnFormClosing 을 비운다(기존 Smoke 의 PreviewFixture 와 같은 방법).
    /// 기본 생성자가 없으면 매개변수 기본값(false / null / 0)으로 원래 형식을 만든다.
    /// </summary>
    private static Form CreateWithoutLoad(Type type)
    {
        ConstructorInfo ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        if (ctor == null || type.IsSealed)
        {
            ConstructorInfo any = type.GetConstructors().OrderBy(c => c.GetParameters().Length).First();
            object[] values = any.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();
            return (Form)any.Invoke(values);
        }
        var asmName = new AssemblyName("PocLayoutFixture_" + type.Name);
        AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
        ModuleBuilder mb = ab.DefineDynamicModule(asmName.Name);
        TypeBuilder tb = mb.DefineType(type.Name + "Fixture", TypeAttributes.Public | TypeAttributes.Class, type);
        ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
        ILGenerator il = cb.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, ctor);
        il.Emit(OpCodes.Ret);
        foreach (var pair in new[] { new { Name = "OnLoad", Arg = typeof(EventArgs) }, new { Name = "OnActivated", Arg = typeof(EventArgs) },
                                     new { Name = "OnFormClosing", Arg = typeof(FormClosingEventArgs) }, new { Name = "OnShown", Arg = typeof(EventArgs) } })
        {
            MethodInfo baseMethod = typeof(Form).GetMethod(pair.Name, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { pair.Arg }, null);
            MethodBuilder m = tb.DefineMethod(pair.Name, MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(void), new[] { pair.Arg });
            m.GetILGenerator().Emit(OpCodes.Ret);
            tb.DefineMethodOverride(m, baseMethod);
        }
        return (Form)Activator.CreateInstance(tb.CreateType());
    }

    private static Assembly ResolveFromSearchDirs(object sender, ResolveEventArgs e)
    {
        string name = new AssemblyName(e.Name).Name;
        foreach (string dir in SearchDirs)
            foreach (string ext in new[] { ".dll", ".exe" })
            {
                string path = Path.Combine(dir, name + ext);
                if (File.Exists(path)) return Assembly.LoadFrom(path);
            }
        return null;
    }
}
