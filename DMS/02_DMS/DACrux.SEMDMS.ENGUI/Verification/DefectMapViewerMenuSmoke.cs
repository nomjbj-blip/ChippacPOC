using System;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// Defect Map Viewer 컨트롤(DPUCDefecMapViewer) 우클릭 메뉴 [Center Cross Show / Hide] 검사 (2026-10-13).
/// 컨트롤만 만들고 화면에 띄우지 않는다 (Load 이벤트의 기준정보 / DB 조회를 하지 않기 위해서다).
/// 확인 항목:
/// 1. 기존 메뉴 항목은 그대로이고 맨 뒤에 "-" / "Center Cross Show / Hide" 2개만 늘었다
/// 2. 처음 CenterMark = 끔 (InitializeComponent 값 그대로. 2026-10-16 부터 Debug 빌드도 Draw 에서 켜지 않는다)
/// 3. 메뉴를 누를 때마다 CenterMark 가 바뀐다
///    (Draw 는 Wafer / DB 가 있어야 실행되므로 이 검사에서는 Draw 뒤 값을 보지 않는다. 대신 Debug 켬 처리용 필드가 없어졌는지 본다)
/// 4. 메뉴가 뜰 때(Popup) Free Zoom / Fit Size / Move View Center / Defect Select / Die Select / Center Cross 만 보인다 (2026-10-13 메뉴 정리)
/// 5. Reclassify / RD 탭은 닫혀 있고 Shot 선 / 원점(중심) Die 테두리는 기본 꺼짐 (2026-10-16)
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
            // Debug 빌드에서 Draw 가 십자선을 켜던 처리(작업자 변경 표시 필드 포함)는 2026-10-16 에 없앴다. 이 검사는 Debug 빌드 DLL 로 돈다.
            Check("Debug 기본 켬 처리 없음 (m_bCenterCrossChanged 필드 없음)", type.GetField("m_bCenterCrossChanged", BindingFlags.Instance | BindingFlags.NonPublic) == null);
            Check("처음 십자선 끔", !(bool)centerMark.GetValue(map, null));
            menu.MenuItems[count - 1].PerformClick();
            Check("메뉴 1번 -> 십자선 켬", (bool)centerMark.GetValue(map, null));
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

            // 탭 정리 (2026-10-16): Reclassify(RC) / RD 탭은 닫혀 있고 나머지 4개는 열려 있다. Shot 선은 기본 꺼짐.
            object dockManager = type.GetField("ultraDockManager", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(viewer);
            object panes = dockManager.GetType().GetProperty("ControlPanes").GetValue(dockManager, null);
            PropertyInfo byKey = panes.GetType().GetProperty("Item", new Type[] { typeof(string) });
            string closedKeys = "";
            foreach (string key in new string[] { "DD", "IL", "RA", "IM", "RC", "RD" })
            {
                object pane = byKey.GetValue(panes, new object[] { key });
                if ((bool)pane.GetType().GetProperty("Closed").GetValue(pane, null)) closedKeys += (closedKeys.Length > 0 ? "," : "") + key;
            }
            Console.WriteLine("닫힌 탭: " + closedKeys);
            Check("닫힌 탭 = RC(Reclassify) / RD", closedKeys == "RC,RD");
            Check("Shot 선 기본 꺼짐 (VisibleShot = false)", !(bool)defectMapType.GetProperty("VisibleShot").GetValue(map, null));
            // 원점(중심) Die 빨간 테두리 기본 끔 (2026-10-16)
            Check("원점 Die 테두리 기본 끔 (DrawOriginDie = false)", !(bool)defectMapType.GetProperty("DrawOriginDie").GetValue(map, null));
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
