using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// 실제 빌드된 KLARF 조회 화면을 화면 밖에서 생성해 파일 읽기 / 범례 / 목록 / 마우스 정보 / 그리기를 검사한다.
/// 인자: [0] DACrux.SEMDMS.ENGUI.dll, [1] KLARF 파일, [2] 결과 이미지(png), [3] 기대 Defect 수(선택), [4] 기대 범례 행 수(선택)
/// DB 저장과 공통 화면 접속 이력 호출은 하지 않는다.
/// </summary>
internal static class KlarfViewerFormSmoke
{
    [STAThread]
    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        Application.EnableVisualStyles();
        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type type = assembly.GetType("DACrux.SEMDMS.ENGUI.frmKlarfFileViewer", true);
        using (var form = new PreviewFixture())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.ShowInTaskbar = false;
            form.Show();
            Application.DoEvents();

            ((TextBox)Field(type, form, "txtFile")).Text = Path.GetFullPath(args[1]);
            type.GetMethod("btnReload_Click", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { null, EventArgs.Empty });
            Application.DoEvents();

            var status = (Label)Field(type, form, "lblStatus");
            var defects = (DataGridView)Field(type, form, "dgvDefect");
            var legend = (DataGridView)Field(type, form, "dgvLegend");
            var info = (TextBox)Field(type, form, "txtInfo");
            var map = (Control)Field(type, form, "m_dMap");
            Console.WriteLine("Status: " + status.Text);
            Console.WriteLine("Defect rows: " + defects.Rows.Count + " / Legend rows: " + legend.Rows.Count);
            if (!status.Text.StartsWith("읽기 완료")) throw new Exception("File was not loaded: " + status.Text);
            if (!(map is DACrux.Map.DefectMap)) throw new Exception("Wrong Map control");
            if (args.Length > 3 && defects.Rows.Count != int.Parse(args[3])) throw new Exception("Defect count mismatch");
            if (args.Length > 4 && legend.Rows.Count != int.Parse(args[4])) throw new Exception("Legend count mismatch");
            for (int i = 0; i < legend.Rows.Count; i++)
                Console.WriteLine("  Legend " + legend.Rows[i].Cells[1].Value + " " + legend.Rows[i].Cells[2].Value + " = " + legend.Rows[i].Cells[3].Value);
            Console.WriteLine(info.Text);

            // 첫 번째 Defect 의 화면 위치로 실제 OnMouseMove 를 보내 가까운 Defect / 검사 Die 표시를 확인한다.
            var mouse = (Label)Field(type, form, "lblMouse");
            var first = (DACrux.Base.Defect)defects.Rows[0].Tag;
            PointF p = (PointF)map.GetType().GetMethod("ToScreen").Invoke(map, new object[] { first.X, first.Y });
            MethodInfo move = map.GetType().GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
            move.Invoke(map, new object[] { new MouseEventArgs(MouseButtons.None, 0, (int)Math.Round(p.X), (int)Math.Round(p.Y), 0) });
            Console.WriteLine("Mouse at defect " + first.DEFECTID + " -> " + p + Environment.NewLine + mouse.Text);
            if (!mouse.Text.Contains("[검사 Die]") || !mouse.Text.Contains("가까운 Defect: ID "))
                throw new Exception("Mouse info did not reach label");
            int expectX = first.XINDEX - ((Point)map.GetType().GetProperty("IndexOffset").GetValue(map, null)).X;
            if (!mouse.Text.Contains("Die (KLARF Index): (" + expectX + ","))
                throw new Exception("Die index under mouse is not the defect die (" + expectX + ")");

            // 목록 선택 강조와 Fill 그리기가 예외 없이 동작해야 한다.
            defects.Rows[0].Selected = true;
            Application.DoEvents();
            using (var bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(args[2]);
            }
            var mapImage = (Bitmap)map.GetType().GetMethod("GetMapImage").Invoke(map, null);
            if (mapImage != null) mapImage.Save(Path.ChangeExtension(args[2], ".map.png"));
            form.Close();
        }
        Console.WriteLine("PASS: KLARF load / legend / defect list / mouse info / bitmap");
        return 0;
    }

    private static object Field(Type type, object instance, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
    }

    /// <summary>공통 베이스 화면의 접속 이력 기록만 생략한다. 실제 디자이너/컨트롤/파싱 코드는 그대로 실행한다.</summary>
    private sealed class PreviewFixture : DACrux.SEMDMS.ENGUI.frmKlarfFileViewer
    {
        protected override void OnLoad(EventArgs e) { }
        protected override void OnActivated(EventArgs e) { }
        protected override void OnFormClosing(FormClosingEventArgs e) { }
    }
}
