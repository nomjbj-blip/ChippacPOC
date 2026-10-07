using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>실제 빌드된 폼을 화면 밖에서 생성해 Parsing/레이아웃/경로 변경 상태를 검사한다. DB 저장은 호출하지 않는다.</summary>
internal static class StandardMapFormSmoke
{
    /// <summary>샘플을 실제 폼 이벤트에 전달하고 DrawToBitmap으로 검토용 이미지를 만든다.</summary>
    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex.Message); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type type = assembly.GetType("DACrux.TEST.ENGUI.frmStandardMapSetup", true);
        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            var path = (TextBox)Field(type, form, "txtFile");
            path.Text = Path.GetFullPath(args[1]);
            type.GetMethod("btnParse_Click", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { null, EventArgs.Empty });
            var info = (TextBox)Field(type, form, "txtInfo");
            var save = (Button)Field(type, form, "btnSave");
            var preview = (Control)Field(type, form, "m_wMap");
            if (!(preview is DACrux.Map.EditWaferMap)) throw new Exception("Wrong Map control");
            if (!info.Text.Contains("26개") || !save.Enabled) throw new Exception("Parsing did not reach form");
            using (var dies = (System.Data.DataTable)type.GetMethod("CreateMapDieTable", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, null))
            {
                if (dies.Rows.Count != 26 || dies.Select("USECODE <> 1").Length != 0 || dies.Select("Y = 2 OR Y = 6").Length != 0
                    || dies.Select("X = 4 AND Y = 1").Length != 1 || dies.Select("X = 1 AND Y = 1").Length != 0)
                    throw new Exception("m_wMap storage shape failed");
            }
            var wafer = (ComboBox)Field(type, form, "cmbWaferSize");
            wafer.SelectedItem = "300";
            if ((double)preview.GetType().GetProperty("WaferSize").GetValue(preview, null) != 300D)
                throw new Exception("Designer selection event was not connected");
            // Wafer 크기에 비례해 칩 크기가 바뀌어야 한다 (Wafer 맞춤). 가장 먼 칩 모서리는 반지름 안에 있어야 한다.
            double die300 = Prop(preview, "DieSizeX");
            wafer.SelectedItem = "200";
            double die200 = Prop(preview, "DieSizeX");
            if (Math.Abs(die300 / die200 - 1.5) > 1e-9) throw new Exception("Die size did not follow wafer size: " + die300 + " / " + die200);
            CheckInsideWafer(preview, 200D);
            wafer.SelectedItem = "300";
            CheckInsideWafer(preview, 300D);
            if (!(bool)preview.GetType().GetProperty("PopupMenu").GetValue(preview, null)) throw new Exception("Context menu disabled");
            if (preview.GetType().GetProperty("XYDirect").GetValue(preview, null).ToString() != "LeftTop"
                || (bool)preview.GetType().GetProperty("DrawOriginDie").GetValue(preview, null)
                || !(bool)preview.GetType().GetProperty("CenterMark").GetValue(preview, null)
                || (int)preview.GetType().GetProperty("OriginIndexX").GetValue(preview, null) != 1
                || (int)preview.GetType().GetProperty("OriginIndexY").GetValue(preview, null) != 1
                || (int)preview.GetType().GetProperty("SelectedDiesCount").GetValue(preview, null) != 1)
                throw new Exception("LeftTop / file reference / center die setup failed");
            form.PerformLayout();
            Application.DoEvents();
            if (preview.Width < 400 || preview.Left < 300) throw new Exception("Preview dock layout failed");
            preview.GetType().GetMethod("Redraw").Invoke(preview, null);
            CheckHover(type, form, preview);
            var mapImage = (Bitmap)preview.GetType().GetMethod("GetMapImage").Invoke(preview, null);
            Console.WriteLine("Map image: " + (mapImage == null ? "NULL" : mapImage.Size.ToString()));
            if (mapImage == null || !HasRedCenterMark(mapImage)) throw new Exception("Center mark was not rendered");
            if (mapImage != null) mapImage.Save(Path.ChangeExtension(args[2], ".map.png"));
            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[2]);
            }
            form.Size = form.MinimumSize;
            form.PerformLayout();
            Application.DoEvents();
            if (preview.Width < 400 || preview.Height < 400) throw new Exception("Minimum-size layout failed");
            path.Text += ".different";
            if (save.Enabled || info.Text.Length != 0) throw new Exception("Stale map remained enabled");
            form.Close();
        }
        Console.WriteLine("PASS: real form parsing / default and minimum layout / stale-path reset / bitmap");
        return 0;
    }

    /// <summary>Map 컨트롤의 double 속성 값을 읽는다.</summary>
    private static double Prop(object target, string name)
    {
        return (double)target.GetType().GetProperty(name).GetValue(target, null);
    }

    /// <summary>Map 이미지의 가운데 부분에 기존 중심 다이의 빨간 표시가 그려졌는지 확인한다.</summary>
    private static bool HasRedCenterMark(Bitmap image)
    {
        for (int y = image.Height / 3; y < image.Height * 2 / 3; y++)
            for (int x = image.Width / 3; x < image.Width * 2 / 3; x++)
            {
                Color color = image.GetPixel(x, y);
                if (color.R > 180 && color.G < 100 && color.B < 100) return true;
            }
        return false;
    }

    /// <summary>실제 Map의 마우스 판정과 Designer 이벤트 연결을 통해 다이 좌표 표시를 검사한다.</summary>
    private static void CheckHover(Type formType, Form form, Control preview)
    {
        var label = (Label)Field(formType, form, "lblCoordinates");
        DACrux.Base.PointD waferPoint = new DACrux.Base.PointD();
        DACrux.Base.PointD mapPoint = new DACrux.Base.PointD();
        var map = (DACrux.Map.EditWaferMap)preview;
        map.OnChangePosition += delegate(object sender, DACrux.Base.PointD wafer, DACrux.Base.PointD origin)
        {
            waferPoint = wafer;
            mapPoint = origin;
        };
        MethodInfo move = preview.GetType().GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
        Point hit = Point.Empty;
        bool found = false;
        for (int y = preview.Height / 2; y < preview.Height && !found; y += 40)
            for (int x = preview.Width / 2; x < preview.Width && !found; x += 40)
            {
                move.Invoke(preview, new object[] { new MouseEventArgs(MouseButtons.None, 0, x, y, 0) });
                if (!label.Text.StartsWith("다이 X/Y: -")) { hit = new Point(x, y); found = true; }
            }
        if (!found || !label.Text.Contains("마우스 Wafer 중심 기준:") || !label.Text.Contains("중심 다이: 5, 4"))
            throw new Exception("Hover coordinates did not reach Designer label: " + label.Text + " / die=" + (Field(formType, form, "hoverDie") == null ? "null" : "set"));
        if (!label.Text.Contains("X=" + waferPoint.X.ToString("0.#####", CultureInfo.InvariantCulture))
            || !label.Text.Contains("X=" + mapPoint.X.ToString("0.#####", CultureInfo.InvariantCulture)))
            throw new Exception("Displayed position differs from Map control event");
        string first = label.Text;
        move.Invoke(preview, new object[] { new MouseEventArgs(MouseButtons.None, 0, hit.X + 1, hit.Y + 1, 0) });
        if (label.Text == first || label.Text.StartsWith("다이 X/Y: -"))
            throw new Exception("Mouse position did not update within the same die");
        move.Invoke(preview, new object[] { new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) });
        if (!label.Text.StartsWith("다이 X/Y: -")) throw new Exception("Empty location retained last coordinates");
        Console.WriteLine("PASS: actual Map mouse position / center die / empty-area clear");
    }

    /// <summary>모든 칩의 모서리가 Wafer 반지름 안에 있고 가장 먼 모서리가 반지름의 90% 이상인지 확인한다.</summary>
    private static void CheckInsideWafer(Control preview, double waferSize)
    {
        double sx = Prop(preview, "DieSizeX"), sy = Prop(preview, "DieSizeY");
        double ox = Prop(preview, "OriginX"), oy = Prop(preview, "OriginY");
        int refX = (int)preview.GetType().GetProperty("OriginIndexX").GetValue(preview, null);
        int refY = (int)preview.GetType().GetProperty("OriginIndexY").GetValue(preview, null);
        var dies = (System.Collections.IList)preview.GetType().GetProperty("Dies").GetValue(preview, null);
        double max = 0;
        foreach (object die in dies)
        {
            int ix = (int)die.GetType().GetField("IndexX").GetValue(die);
            int iy = (int)die.GetType().GetField("IndexY").GetValue(die);
            double l = (ix - refX - 1) * sx - ox, b = (refY - iy) * sy - oy;
            double fx = Math.Max(Math.Abs(l), Math.Abs(l + sx)), fy = Math.Max(Math.Abs(b), Math.Abs(b + sy));
            max = Math.Max(max, Math.Sqrt(fx * fx + fy * fy));
        }
        if (max > waferSize / 2 || max < waferSize / 2 * 0.9) throw new Exception("Dies not fitted to " + waferSize + " wafer: " + max);
    }

    /// <summary>디자이너 필드를 검사하기 위한 접근이며 제품 코드의 공개 API를 늘리지 않는다.</summary>
    private static object Field(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록만 생략한다. 실제 디자이너/컨트롤/Parsing 코드는 그대로 실행한다.</summary>
    private sealed class PreviewFixture : DACrux.TEST.ENGUI.frmStandardMapSetup
    {
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }
}
