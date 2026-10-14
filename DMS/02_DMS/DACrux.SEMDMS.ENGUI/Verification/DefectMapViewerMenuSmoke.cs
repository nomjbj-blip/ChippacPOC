using System;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// Defect Map Viewer 컨트롤(DPUCDefecMapViewer) 우클릭 메뉴 [Center Cross Show / Hide] 검사 (2026-10-13).
/// 컨트롤만 만들고 화면에 띄우지 않는다 (Load 이벤트의 기준정보 / DB 조회를 하지 않기 위해서다).
/// 확인 항목:
/// 1. 기존 메뉴 항목은 그대로이고 맨 뒤에 "-" / "Center Cross Show / Hide" 2개만 늘었다
/// 2. 처음 CenterMark = 끔 (InitializeComponent 값 그대로, Debug 빌드의 켬은 Draw 에서 정한다)
/// 3. 메뉴를 누를 때마다 CenterMark 가 바뀌고, 작업자가 바꿨다는 표시(m_bCenterCrossChanged)가 남는다
///    (Draw 의 Debug 기본값 처리는 Wafer / DB 가 있어야 실행되므로 이 검사에서는 표시 값만 본다)
/// 4. 메뉴가 뜰 때(Popup) Free Zoom / Fit Size / Move View Center / Defect Select / Die Select / Center Cross 만 보인다 (2026-10-13 메뉴 정리)
/// 사용: DefectMapViewerMenuSmoke.exe <TEMP 빌드 DACrux.SEMDMS.Control.dll>
/// 출력 마지막 줄: DEFECT_MAP_VIEWER_MENU_SMOKE=PASS / FAIL
/// </summary>
internal static class DefectMapViewerMenuSmoke
{
    private static int failures;

    [STAThread]
    private static int Main(string[] args)
    {
        try { Run(args); }
        catch (Exception ex) { Console.WriteLine("ERROR " + ex); failures++; }
        Console.WriteLine(failures == 0 ? "DEFECT_MAP_VIEWER_MENU_SMOKE=PASS" : "DEFECT_MAP_VIEWER_MENU_SMOKE=FAIL (" + failures + ")");
        return failures == 0 ? 0 : 1;
    }

    private static void Run(string[] args)
    {
        Assembly assembly = Assembly.LoadFrom(args[0]);
        Type type = assembly.GetType("DACrux.SEMDMS.Control.DPUCDefecMapViewer", true);
        using (Control viewer = (Control)Activator.CreateInstance(type))
        {
            object map = type.GetField("map", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(viewer);
            ContextMenu menu = Menu(map);
            int count = menu.MenuItems.Count;
            Console.WriteLine("메뉴 항목 수: " + count + " / 마지막 2개: " + menu.MenuItems[count - 2].Text + " | " + menu.MenuItems[count - 1].Text);
            Check("맨 뒤 = 구분선 / Center Cross Show / Hide", menu.MenuItems[count - 2].Text == "-" && menu.MenuItems[count - 1].Text == "Center Cross Show / Hide");

            // 같은 Core 의 DefectMap 을 새로 만들어 기존 메뉴 수와 비교한다 (기존 항목은 바뀌지 않고 2개만 늘어야 함).
            // 화면 Map 은 전용 상속 클래스(DefectViewerMap, 2026-10-13)라 비교 대상은 기반 DACrux.Map.DefectMap 이다.
            Type defectMapType = map.GetType().BaseType;
            Check("화면 Map = DefectViewerMap (기반 DefectMap)", map.GetType().Name == "DefectViewerMap" && defectMapType.FullName == "DACrux.Map.DefectMap");
            using (Control plain = (Control)Activator.CreateInstance(defectMapType))
            {
                ContextMenu plainMenu = Menu(plain);
                bool same = plainMenu.MenuItems.Count + 2 == count;
                for (int i = 0; same && i < plainMenu.MenuItems.Count; i++) same = plainMenu.MenuItems[i].Text == menu.MenuItems[i].Text;
                Check("기존 메뉴 항목 " + plainMenu.MenuItems.Count + "개 그대로 + 2개", same);
            }

            PropertyInfo centerMark = defectMapType.GetProperty("CenterMark");
            FieldInfo changed = type.GetField("m_bCenterCrossChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            Check("처음 십자선 끔 / 바꾼 표시 없음", !(bool)centerMark.GetValue(map, null) && !(bool)changed.GetValue(viewer));
            menu.MenuItems[count - 1].PerformClick();
            Check("메뉴 1번 -> 십자선 켬 / 바꾼 표시", (bool)centerMark.GetValue(map, null) && (bool)changed.GetValue(viewer));
            menu.MenuItems[count - 1].PerformClick();
            Check("메뉴 2번 -> 십자선 끔", !(bool)centerMark.GetValue(map, null));

            // 메뉴 정리 (2026-10-13): 메뉴가 뜰 때(Popup) 작업 항목만 보인다. 화면에 띄우지 않으므로 Popup 이벤트를 직접 부른다.
            typeof(Menu).Assembly.GetType("System.Windows.Forms.ContextMenu")
                .GetMethod("OnPopup", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(menu, new object[] { EventArgs.Empty });
            string shown = "";
            foreach (MenuItem item in menu.MenuItems)
                if (item.Visible) shown += (shown.Length > 0 ? " | " : "") + item.Text;
            Console.WriteLine("보이는 메뉴: " + shown);
            Check("보이는 메뉴 = Free Zoom / Fit Size / Move View Center / - / Defect Select / Die Select / - / Center Cross",
                shown == "Free Zoom | Fit Size | Move View Center | - | Defect Select | Die Select | - | Center Cross Show / Hide");
            int hidden = 0;
            foreach (MenuItem item in menu.MenuItems) if (!item.Visible) hidden++;
            Check("숨긴 항목 " + hidden + "개 (메뉴 자체는 그대로, 항목 수 " + menu.MenuItems.Count + ")", hidden > 0 && menu.MenuItems.Count == count);
        }
    }

    private static ContextMenu Menu(object map)
    {
        Type waferMap = map.GetType();
        while (waferMap != null && waferMap.Name != "WaferMap") waferMap = waferMap.BaseType;
        return (ContextMenu)waferMap.GetField("ctxmWaferMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(map);
    }

    private static void Check(string name, bool ok)
    {
        Console.WriteLine((ok ? "PASS " : "FAIL ") + name);
        if (!ok) failures++;
    }
}
