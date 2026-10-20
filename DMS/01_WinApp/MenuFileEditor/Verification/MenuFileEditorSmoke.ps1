# 메뉴 파일 편집기 Smoke (2026-10-15)
# 목적: MenuFileEditor 가 DACruxV5\MenuFile.mnu 를 형식 그대로 읽고 쓰는지, 그리고 순서 / 부모 / Key / 추가 / 복제 / 삭제 / 값 변경을
#       여러 번 해도 왼쪽 트리가 비거나 메뉴가 빠지지 않는지 확인한다.
#       - 메뉴 파일은 %TEMP%\chippac\menueditorsmoke 에 복사해서 쓰고 원본은 바꾸지 않는다.
#       - 화면은 화면 밖에 띄운다. 속성 창 값 변경은 속성 창 항목(GridEntry)의 글자 입력 경로로 넣는다.
#       - 예상하지 못한 메시지 창이 뜨면 닫고 FAIL 로 남긴다(검사가 멈추지 않게).
#       - 마지막으로 연 파일 기록(%LOCALAPPDATA%\MenuFileEditor\last_path.txt)은 검사 뒤 원래대로 돌린다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File MenuFileEditorSmoke.ps1 -EditorExe <빌드한 MenuFileEditor.exe>
param(
    [Parameter(Mandatory = $true)][string]$EditorExe,
    [string]$MenuFile = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($MenuFile -eq '') { $MenuFile = Join-Path $dms '01_WinApp\DACruxV5\MenuFile.mnu' }
$work = Join-Path $env:TEMP 'chippac\menueditorsmoke'
if (Test-Path $work) { Remove-Item $work -Recurse -Force }
New-Item -ItemType Directory -Force (Join-Path $work 'bin') | Out-Null
# Add-Type 참조는 .dll 이름만 받으므로 실행 파일을 .dll 이름으로 복사해서 쓴다.
$exe = Join-Path $work 'bin\MenuFileEditor.dll'
Copy-Item $EditorExe $exe -Force
$workFile = Join-Path $work 'MenuFile.mnu'
Copy-Item $MenuFile $workFile -Force
[void][Reflection.Assembly]::LoadFrom($exe)
Add-Type -AssemblyName System.Windows.Forms, System.Drawing, System.Data, System.Xml

$helper = @'
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MenuFileEditor;

public static class MenuEditorSmoke
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    static readonly List<string> results = new List<string>();
    static readonly List<string> dialogs = new List<string>();
    static MenuEditorForm form;
    static IntPtr formHandle;
    static int uiThread;
    static volatile bool watching;

    delegate bool EnumProc(IntPtr hwnd, IntPtr lParam);
    [DllImport("user32.dll")] static extern bool EnumThreadWindows(int threadId, EnumProc callback, IntPtr lParam);
    [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr hwnd);
    [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hwnd, int index);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int max);
    [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("kernel32.dll")] static extern int GetCurrentThreadId();

    static readonly EnumProc closeDialog = CloseDialog;

    /// <summary>화면 스레드의 제목 있는 창(메인 화면 제외)을 메시지 창으로 기록하고 닫는다.</summary>
    static bool CloseDialog(IntPtr h, IntPtr l)
    {
        if (h == formHandle || !IsWindowVisible(h)) return true;
        if ((GetWindowLong(h, -16) & 0x00C00000) != 0x00C00000) return true;
        var sb = new StringBuilder(256);
        GetWindowText(h, sb, 256);
        lock (dialogs) dialogs.Add(sb.ToString());
        PostMessage(h, 0x0111, (IntPtr)7, IntPtr.Zero);
        PostMessage(h, 0x0111, (IntPtr)1, IntPtr.Zero);
        PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero);
        return true;
    }

    static void Watch()
    {
        while (watching)
        {
            Thread.Sleep(300);
            EnumThreadWindows(uiThread, closeDialog, IntPtr.Zero);
        }
    }

    static int DialogCount() { lock (dialogs) return dialogs.Count; }

    static string TakeDialogs(int from)
    {
        lock (dialogs)
        {
            string text = string.Join(" | ", dialogs.Skip(from).ToArray());
            dialogs.RemoveRange(from, dialogs.Count - from);
            return text;
        }
    }

    static object F(string name) { return typeof(MenuEditorForm).GetField(name, NP).GetValue(form); }

    static object Call(string name, params object[] args)
    {
        MethodInfo m = typeof(MenuEditorForm).GetMethod(name, NP | BindingFlags.Public);
        try { return m.Invoke(form, args); }
        catch (TargetInvocationException ex) { throw ex.InnerException; }
    }

    static void Pump() { for (int i = 0; i < 4; i++) Application.DoEvents(); }
    static void Check(string name, bool ok, string detail) { results.Add((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail); }

    static MenuDocument Doc { get { return form.Document; } }
    static TreeView Tree { get { return (TreeView)F("treeMenu"); } }
    static PropertyGrid Grid { get { return (PropertyGrid)F("propertyGrid"); } }
    static Dictionary<DataRow, TreeNode> Map { get { return (Dictionary<DataRow, TreeNode>)F("nodeMap"); } }
    static TreeNode RootNode { get { return (TreeNode)F("rootNode"); } }
    static TreeNode OrphanNode { get { return (TreeNode)F("orphanNode"); } }
    static DataRow Selected { get { TreeNode n = Tree.SelectedNode; return n == null ? null : n.Tag as DataRow; } }
    static MenuItemView View { get { return Grid.SelectedObject as MenuItemView; } }
    static string K(DataRow r) { return MenuDocument.Key(r); }
    static string P(DataRow r) { return MenuDocument.Parent(r); }
    static void Select(DataRow row) { Call("SelectRow", row); Pump(); }

    static void Flatten(TreeNode node, List<TreeNode> list)
    {
        list.Add(node);
        foreach (TreeNode child in node.Nodes) Flatten(child, list);
    }

    static List<TreeNode> AllNodes()
    {
        var list = new List<TreeNode>();
        foreach (TreeNode n in Tree.Nodes) Flatten(n, list);
        return list;
    }

    static bool Consecutive(string parent)
    {
        List<DataRow> c = Doc.Children(parent);
        for (int i = 0; i < c.Count; i++) if (MenuDocument.Order(c[i]) != i) return false;
        return true;
    }

    /// <summary>
    /// 트리 무결성: 최상위 노드 있음 / 메뉴 노드 수 = 문서 메뉴 수 / 노드 부모 = 문서 부모 / 형제 순서 = 문서 순서 /
    /// 지운 행이 남지 않음 / 선택 노드 있음 / 속성 창이 선택 메뉴를 보임.
    /// </summary>
    static string Integrity()
    {
        if (Tree.Nodes.Count == 0) return "트리 비었음";
        TreeNode root = RootNode, orphan = OrphanNode;
        if (root == null || root.TreeView != Tree) return "최상위 노드 없음";
        List<TreeNode> all = AllNodes();
        int rowNodes = all.Count(n => n.Tag is DataRow);
        if (rowNodes != Doc.Count) return "노드 " + rowNodes + " / 메뉴 " + Doc.Count;
        if (Doc.Orphans().Count > 0 && orphan == null) return "부모 없음 노드 없음";
        foreach (TreeNode n in all)
        {
            var row = n.Tag as DataRow;
            if (row == null) continue;
            if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) return "지운 행이 트리에 남음";
            if (n.Parent == root) { if (P(row) != MenuDocument.Root) return "최상위 아래 부모 다름: " + K(row); }
            else if (n.Parent != orphan)
            {
                var parentRow = n.Parent == null ? null : n.Parent.Tag as DataRow;
                if (parentRow == null || K(parentRow) != P(row)) return "부모 다름: " + K(row);
            }
            List<DataRow> expect = Doc.Children(K(row));
            List<DataRow> actual = n.Nodes.Cast<TreeNode>().Select(c => c.Tag as DataRow).ToList();
            if (!expect.SequenceEqual(actual)) return "하위 순서 다름: " + K(row);
        }
        if (!Doc.Children(MenuDocument.Root).SequenceEqual(root.Nodes.Cast<TreeNode>().Select(c => c.Tag as DataRow))) return "최상위 순서 다름";
        if (Tree.SelectedNode == null) return "선택 없음";
        DataRow selected = Selected;
        MenuItemView view = View;
        if (selected == null ? view != null : (view == null || view.Row != selected)) return "속성 창이 선택 메뉴와 다름";
        return "OK";
    }

    static GridItem FindItem(GridItem item, string label)
    {
        if (item.GridItemType == GridItemType.Property && item.Label == label) return item;
        foreach (GridItem child in item.GridItems)
        {
            GridItem found = FindItem(child, label);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>속성 창 항목에 글자를 넣는다(사용자가 칸에 입력하고 Enter 를 누른 것과 같은 경로).</summary>
    static void SetViaGrid(string label, string text)
    {
        SetViaGrid(label, text, true);
    }

    /// <param name="pump">false 면 메시지 처리를 하지 않는다(예약된 트리 다시 만들기 전 상태를 만들 때).</param>
    static void SetViaGrid(string label, string text, bool pump)
    {
        GridItem item = Grid.SelectedGridItem;
        if (item == null) throw new InvalidOperationException("속성 창 선택 항목 없음");
        while (item.Parent != null) item = item.Parent;
        GridItem target = FindItem(item, label);
        if (target == null) throw new InvalidOperationException("속성 없음: " + label);
        MethodInfo m = target.GetType().GetMethod("SetPropertyTextValue", NP | BindingFlags.Public, null, new[] { typeof(string) }, null);
        if (m == null) throw new InvalidOperationException("SetPropertyTextValue 없음");
        try { m.Invoke(target, new object[] { text }); }
        catch (TargetInvocationException ex) { throw ex.InnerException; }
        if (pump) Pump();
    }

    static string MakeImage(string dir)
    {
        string path = Path.Combine(dir, "icon_source.png");
        using (var bitmap = new Bitmap(64, 48, PixelFormat.Format32bppArgb))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.FillEllipse(Brushes.SteelBlue, 4, 4, 56, 40);
            }
            bitmap.Save(path, ImageFormat.Png);
        }
        return path;
    }

    public static List<string> Run(string original, string workFile, string workDir)
    {
        byte[] originalBytes = File.ReadAllBytes(original);
        MenuDocument raw = MenuDocument.Load(workFile);
        byte[] round = raw.Serialize();
        // 기준 1: 기존 메뉴 편집 도구 저장 방식(DataTable.WriteXml(파일, WriteSchema))과 바이트 단위로 같아야 한다.
        string legacyPath = Path.Combine(Path.GetDirectoryName(workFile), "legacy_write.mnu");
        var legacy = new DataSet();
        legacy.ReadXml(original, XmlReadMode.ReadSchema);
        legacy.Tables["Table1"].WriteXml(legacyPath, XmlWriteMode.WriteSchema);
        byte[] legacyBytes = File.ReadAllBytes(legacyPath);
        Check("수정 없이 저장 = 기존 편집 도구 저장 결과와 바이트 단위로 같음", round.SequenceEqual(legacyBytes), legacyBytes.Length + " / " + round.Length + " byte");
        // 기준 2: 원본 파일과는 태그 사이 공백(손으로 고친 줄) 외에는 같아야 한다.
        string originalText = Encoding.UTF8.GetString(originalBytes), roundText = Encoding.UTF8.GetString(round);
        string[] ol = originalText.Split(new[] { "\r\n" }, StringSplitOptions.None), rl = roundText.Split(new[] { "\r\n" }, StringSplitOptions.None);
        int lineDiff = ol.Except(rl).Count();
        Check("수정 없이 저장 = 원본과 태그 사이 공백 외 차이 없음", Regex.Replace(originalText, @">\s+<", "><") == Regex.Replace(roundText, @">\s+<", "><"),
            "공백만 다른 줄 " + lineDiff + "개");
        List<MenuProblem> baseProblems = raw.Validate(null);
        int baseErrors = baseProblems.Count(p => p.IsError);
        int baseLevelWarn = baseProblems.Count(p => !p.IsError && p.Message.StartsWith("Menu Level"));
        Check("원본 메뉴 파일 검증 오류 0", baseErrors == 0, "메뉴 " + raw.Count + " / 오류 " + baseErrors + " / 경고 " + (baseProblems.Count - baseErrors));

        uiThread = GetCurrentThreadId();
        form = new MenuEditorForm(workFile);
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-32000, -32000);
        form.Show();
        formHandle = form.Handle;
        watching = true;
        var watcher = new Thread(Watch);
        watcher.IsBackground = true;
        watcher.Start();
        try
        {
            Pump();
            Steps(workFile, workDir, baseLevelWarn);
        }
        catch (Exception ex)
        {
            Check("검사 중 예외 없음", false, ex.GetType().Name + ": " + ex.Message + " @ " + ex.StackTrace);
        }
        finally
        {
            form.Dispose();
            Pump();
            watching = false;
        }
        lock (dialogs) Check("예상하지 못한 메시지 창 없음", dialogs.Count == 0, dialogs.Count == 0 ? "0" : string.Join(" | ", dialogs.ToArray()));
        return results;
    }

    static void Steps(string workFile, string workDir, int baseLevelWarn)
    {
        // 1. 처음 열기
        bool allExpanded = AllNodes().Where(n => n.Nodes.Count > 0).All(n => n.IsExpanded);
        Check("처음 열면 모든 메뉴가 트리에 보임(모두 펼침)", Doc != null && Integrity() == "OK" && allExpanded,
            "메뉴 " + (Doc == null ? 0 : Doc.Count) + " / " + Integrity() + " / 모두 펼침 " + allExpanded);

        // 1-1. 창 크기: 최소 크기 / 분할선을 끝까지 / 최소화 -> 복원에서 오류 없이 찾기 줄 버튼이 겹치지 않음
        var split = (SplitContainer)F("splitMain");
        var txtSearch = (TextBox)F("txtSearch");
        var btnFind = (Button)F("btnFind");
        Size normalSize = form.Size;
        form.Size = form.MinimumSize;
        Pump();
        split.SplitterDistance = 0;
        Pump();
        bool narrowOk = split.SplitterDistance >= split.Panel1MinSize && btnFind.Left >= txtSearch.Right && txtSearch.Width > 0;
        string narrow = "분할 " + split.SplitterDistance + " / 찾기 칸 " + txtSearch.Left + "~" + txtSearch.Right + " / 찾기 버튼 " + btnFind.Left;
        form.WindowState = FormWindowState.Minimized;
        Pump();
        form.WindowState = FormWindowState.Normal;
        Pump();
        form.Size = normalSize;
        split.SplitterDistance = 640;
        Pump();
        Check("최소 크기 / 분할선 끝까지 / 최소화 -> 복원: 찾기 줄 겹치지 않음 / 트리 유지", narrowOk && Integrity() == "OK" && split.SplitterDistance == 640, narrow + " / " + Integrity());

        // 2. 순서 이동(위 / 아래) + 펼침 상태 / 위쪽 노드 / 속성 창 객체 유지
        DataRow parentRow = Doc.Rows.First(r => Doc.Children(K(r)).Count >= 3);
        string parentKey = K(parentRow);
        List<DataRow> c = Doc.Children(parentKey);
        List<string> originalOrder = c.Select(K).ToList();
        var ancestors = new HashSet<string>();
        for (string k = parentKey; k != MenuDocument.Root && k.Length > 0; k = P(Doc.Find(k))) ancestors.Add(k);
        DataRow collapsedRow = Doc.Children(MenuDocument.Root).First(r => Doc.Children(K(r)).Count > 0 && !ancestors.Contains(K(r)));
        Map[collapsedRow].Collapse();
        Select(c[1]);
        var visible = new List<TreeNode>();
        for (TreeNode n = Tree.Nodes[0]; n != null; n = n.NextVisibleNode) visible.Add(n);
        int vi = visible.IndexOf(Map[c[1]]);
        Tree.TopNode = visible[Math.Max(0, vi - 3)];
        Pump();
        object topTag = Tree.TopNode.Tag;
        MenuItemView viewBefore = View;
        bool upEnabled = ((Button)F("btnMoveUp")).Enabled;
        Call("MoveSelected", -1);
        Pump();
        List<DataRow> after = Doc.Children(parentKey);
        Check("위로 이동: 형제 순서 / MNU_ORDER 0..n-1 / 선택 유지", upEnabled && after[0] == c[1] && after[1] == c[0] && Consecutive(parentKey) && Selected == c[1] && Integrity() == "OK",
            parentKey + " : " + string.Join(",", after.Take(3).Select(K).ToArray()) + " / " + Integrity());
        Check("트리를 다시 만들어도 접은 노드 / 펼친 노드 / 위쪽 노드 유지", !Map[collapsedRow].IsExpanded && Map[parentRow].IsExpanded && Tree.TopNode != null && Equals(Tree.TopNode.Tag, topTag),
            "접은 " + K(collapsedRow) + " = " + !Map[collapsedRow].IsExpanded + " / 위쪽 노드 같음 = " + (Tree.TopNode != null && Equals(Tree.TopNode.Tag, topTag)));
        Check("구조가 바뀌어도 속성 창 객체를 바꾸지 않음", View == viewBefore && View != null, "같은 객체 = " + (View == viewBefore));
        bool upDisabled = !((Button)F("btnMoveUp")).Enabled;
        Call("MoveSelected", -1);
        Pump();
        Check("맨 위 메뉴는 위로 이동 안 됨(버튼 비활성 / 순서 그대로)", upDisabled && Doc.Children(parentKey)[0] == c[1], "버튼 비활성 = " + upDisabled);
        Call("MoveSelected", 1);
        Pump();
        Check("아래로 이동: 원래 순서로 돌아옴", Doc.Children(parentKey).Select(K).SequenceEqual(originalOrder) && Integrity() == "OK", string.Join(",", Doc.Children(parentKey).Take(3).Select(K).ToArray()));

        // 3. 값 변경은 해당 노드만 갱신(트리 다시 만들지 않음)
        DataRow v = c[0];
        Select(v);
        TreeNode nodeBefore = Map[v];
        SetViaGrid("Caption 1", "Smoke Caption");
        Check("Caption 변경: 같은 노드 글자만 갱신 / 변경 표시", ReferenceEquals(Map[v], nodeBefore) && nodeBefore.Text == "[" + K(v) + "] Smoke Caption" && Doc.IsDirty && form.Text.EndsWith("*"),
            nodeBefore.Text + " / 제목 " + form.Text);
        SetViaGrid("Visible", "False");
        bool gray = Map[v].ForeColor == Color.Gray && MenuDocument.Text(v, "VISIBLE") == "False";
        SetViaGrid("Visible", "True");
        Check("Visible = False 면 회색 / True 로 돌리면 원래 색", gray && Map[v].ForeColor != Color.Gray && MenuDocument.Text(v, "VISIBLE") == "True" && ReferenceEquals(Map[v], nodeBefore), "회색 = " + gray);

        // 4. 부모 변경(속성 창) / 순환 거부
        DataRow target = Doc.Children(MenuDocument.Root).First(r => Doc.Children(K(r)).Count > 0 && K(r) != parentKey);
        HashSet<string> targetTree = Doc.SubtreeKeys(target);
        DataRow leaf = Doc.Rows.First(r => Doc.Children(K(r)).Count == 0 && P(r) != MenuDocument.Root && !targetTree.Contains(K(r)));
        string oldParent = P(leaf);
        Select(leaf);
        viewBefore = View;
        SetViaGrid("Popup(Parent) Menu", K(target));
        Check("부모 변경: 새 부모 마지막 / Level 갱신 / 예전 형제 순서 정리 / 선택 유지",
            P(leaf) == K(target) && Doc.Children(K(target)).Last() == leaf && Convert.ToInt32(leaf["MNU_LEVEL"]) == Doc.Depth(leaf) && Consecutive(oldParent)
            && Map[leaf].Parent == Map[target] && Selected == leaf && View == viewBefore && Integrity() == "OK",
            K(leaf) + " : " + oldParent + " -> " + P(leaf) + " / Level " + leaf["MNU_LEVEL"] + " / " + Integrity());
        Select(target);
        string cycleError = "";
        try { View.PopupMenu = K(leaf); }
        catch (ArgumentException ex) { cycleError = ex.Message; }
        Pump();
        bool notCandidate = !Doc.ParentCandidates(target).Contains(K(leaf)) && !Doc.ParentCandidates(target).Contains(K(target));
        Check("자기 하위 아래로 부모 변경 거부(순환 방지) / 후보 목록에서 제외", cycleError.Length > 0 && P(target) == MenuDocument.Root && notCandidate && Integrity() == "OK", cycleError);

        // 5. Key 변경(속성 창) / 하위 Parent 같이 변경 / 잘못된 Key 거부
        string oldKey = K(target);
        List<DataRow> kids = Doc.Children(oldKey);
        bool idFollow = MenuDocument.Text(target, "MENU_ID") == oldKey;
        SetViaGrid("Menu Key", "MNU_SMOKE_RENAMED");
        Check("Key 변경: 하위 메뉴 부모도 같이 변경 / 트리 유지",
            K(target) == "MNU_SMOKE_RENAMED" && kids.All(k => P(k) == "MNU_SMOKE_RENAMED") && Doc.Find(oldKey) == null && Map[target].Nodes.Count == kids.Count
            && Selected == target && (!idFollow || MenuDocument.Text(target, "MENU_ID") == "MNU_SMOKE_RENAMED") && Integrity() == "OK",
            oldKey + " -> " + K(target) + " / 하위 " + kids.Count + " / " + Integrity());
        var rejected = new List<string>();
        foreach (string bad in new[] { K(c[0]), "A B", "ROOT", "" })
        {
            try { View.MenuKey = bad; }
            catch (ArgumentException) { rejected.Add(bad.Length == 0 ? "(빈 값)" : bad); }
        }
        Pump();
        Check("중복 / 공백 / ROOT / 빈 Key 거부", rejected.Count == 4 && K(target) == "MNU_SMOKE_RENAMED", string.Join(", ", rejected.ToArray()));

        // 속성 창 값이 반영된 직후(트리 다시 만들기 전) 다른 메뉴를 누른 경우: 누른 메뉴가 선택된 채로 남아야 한다.
        Select(leaf);
        string leafKeyBefore = K(leaf);
        SetViaGrid("Menu Key", leafKeyBefore + "_X", false);
        Call("SelectRow", c[1]);
        Pump();
        Check("값 반영 직후 다른 메뉴를 누르면 누른 메뉴 선택 유지", K(leaf) == leafKeyBefore + "_X" && Selected == c[1] && View != null && View.Row == c[1] && Integrity() == "OK",
            "선택 " + (Selected == null ? "null" : K(Selected)) + " / " + Integrity());

        // 6. 추가 / 하위 추가 / 복제 / 삭제
        Select(leaf);
        int count0 = Doc.Count;
        Call("AddMenu", false);
        Pump();
        DataRow added = Selected;
        List<DataRow> sib = Doc.Children(P(leaf));
        Check("같은 수준 추가: 선택 메뉴 바로 아래 / 새 메뉴 선택",
            Doc.Count == count0 + 1 && added != null && added != leaf && P(added) == P(leaf) && sib.IndexOf(added) == sib.IndexOf(leaf) + 1 && K(added).StartsWith("MNU_NEW")
            && Convert.ToInt32(added["MNU_LEVEL"]) == Doc.Depth(added) && Integrity() == "OK",
            (added == null ? "null" : K(added)) + " / " + Integrity());
        Call("AddMenu", true);
        Pump();
        DataRow child = Selected;
        Check("하위 추가: 선택 메뉴 아래 마지막 / 부모 노드 펼침",
            Doc.Count == count0 + 2 && child != null && P(child) == K(added) && Map[added].IsExpanded && Convert.ToInt32(child["MNU_LEVEL"]) == Doc.Depth(added) + 1 && Integrity() == "OK",
            (child == null ? "null" : K(child)) + " / " + Integrity());
        Select(leaf);
        Call("btnDuplicate_Click", null, EventArgs.Empty);
        Pump();
        DataRow copy = Selected;
        bool sameValues = copy != null && MenuDocument.ColumnNames.Where(col => col != "MENU_KEY" && col != "MENU_ID" && col != "MNU_ORDER")
            .All(col => MenuDocument.Text(copy, col) == MenuDocument.Text(leaf, col));
        sib = Doc.Children(P(leaf));
        Check("복제: Key_COPY / 값 같음 / 원본 바로 아래",
            copy != null && K(copy) == K(leaf) + "_COPY" && sameValues && sib.IndexOf(copy) == sib.IndexOf(leaf) + 1 && Integrity() == "OK",
            (copy == null ? "null" : K(copy)) + " / " + Integrity());
        sib = Doc.Children(P(added));
        DataRow expectedNext = sib.IndexOf(added) + 1 < sib.Count ? sib[sib.IndexOf(added) + 1] : sib[sib.IndexOf(added) - 1];
        Select(added);
        int count1 = Doc.Count;
        string addedKey = K(added), childKey = K(child);
        Call("DeleteSelected");
        Pump();
        Check("삭제: 하위까지 삭제 / 다음 형제 선택 / 형제 순서 정리",
            Doc.Count == count1 - 2 && Doc.Find(addedKey) == null && Doc.Find(childKey) == null && Selected == expectedNext && Consecutive(P(expectedNext)) && Integrity() == "OK",
            "메뉴 " + count1 + " -> " + Doc.Count + " / 선택 " + (Selected == null ? "null" : K(Selected)) + " / " + Integrity());

        // 7. 검증: 잘못된 값 / 부모 없음 / 순환 / 아이콘 / 저장 막기
        string enableBefore = MenuDocument.Text(v, "ENABLE");
        v["ENABLE"] = "Yes";
        bool enableError = Doc.Validate(null).Any(p => p.IsError && p.Key == K(v) && p.Message.Contains("ENABLE"));
        string blockedPath = Path.Combine(workDir, "blocked.mnu");
        bool blocked = false;
        try { Doc.Save(blockedPath); }
        catch (InvalidOperationException) { blocked = true; }
        v["ENABLE"] = enableBefore;
        Check("True / False 가 아닌 값은 오류 / 오류가 있으면 저장 안 함", enableError && blocked && !File.Exists(blockedPath), "오류 = " + enableError + " / 저장 막음 = " + blocked);

        string leafParent = P(leaf);
        leaf["POPUP_MENU"] = "NO_SUCH_PARENT";
        Call("BuildTree", null, false);
        bool orphanShown = OrphanNode != null && Map.ContainsKey(leaf) && Map[leaf].Parent == OrphanNode && AllNodes().Count(n => n.Tag is DataRow) == Doc.Count;
        bool parentError = Doc.Validate(null).Any(p => p.IsError && p.Key == K(leaf) && p.Message.Contains("NO_SUCH_PARENT"));
        leaf["POPUP_MENU"] = leafParent;
        Call("BuildTree", null, false);
        Check("부모 없는 메뉴도 '(부모 없음)' 아래에 보임 / 오류로 표시 / 고치면 원래 자리", orphanShown && parentError && OrphanNode == null && Integrity() == "OK",
            "보임 = " + orphanShown + " / 오류 = " + parentError + " / " + Integrity());

        DataRow loopChild = Doc.Children(K(target)).First();
        target["POPUP_MENU"] = K(loopChild);
        Call("BuildTree", null, false);
        int orphanRows = OrphanNode == null ? 0 : AllNodes().Count(n => n.Tag is DataRow && IsUnder(n, OrphanNode));
        bool loopShown = orphanRows == Doc.SubtreeKeys(target).Count && AllNodes().Count(n => n.Tag is DataRow) == Doc.Count;
        bool loopError = Doc.Validate(null).Any(p => p.IsError && p.Message.Contains("순환"));
        target["POPUP_MENU"] = MenuDocument.Root;
        Call("BuildTree", null, false);
        Check("순환 부모도 트리에서 빠지지 않음 / 오류로 표시", loopShown && loopError && OrphanNode == null && Integrity() == "OK",
            "부모 없음 아래 " + orphanRows + "개 / 오류 = " + loopError + " / " + Integrity());

        string icon16Before = MenuDocument.Text(v, "ICON16");
        v["ICON16"] = "ZZ 00";
        bool iconError = Doc.Validate(null).Any(p => p.IsError && p.Key == K(v) && p.Message.StartsWith("ICON16"));
        v["ICON16"] = icon16Before;
        Check("그림으로 읽을 수 없는 아이콘 값은 오류", iconError, "오류 = " + iconError);

        Call("btnValidate_Click", null, EventArgs.Empty);
        Pump();
        var problemsPanel = (Panel)F("pnlProblems");
        var problemList = (ListBox)F("lstProblems");
        var problemLabel = (Label)F("lblProblems");
        Check("[검증] 버튼: 결과 목록 표시 / 오류 0", problemsPanel.Visible && problemLabel.Text.StartsWith("검증 결과") && Doc.Validate(null).Count(p => p.IsError) == 0,
            problemLabel.Text + " / 목록 " + problemList.Items.Count);

        // 8. 아이콘: 그림 파일 -> 32 / 16 PNG 문자열 -> 속성 반영 / 지우기
        string png = MakeImage(workDir);
        string hex32, hex16;
        using (var f = new IconEditForm("", 32)) { f.LoadImageFile(png); hex32 = f.HexValue; }
        using (var f = new IconEditForm("", 16)) { f.LoadImageFile(png); hex16 = f.HexValue; }
        Size s32 = Size.Empty, s16 = Size.Empty;
        using (Image i32 = MenuDocument.HexToImage(hex32)) if (i32 != null) s32 = i32.Size;
        using (Image i16 = MenuDocument.HexToImage(hex16)) if (i16 != null) s16 = i16.Size;
        Select(v);
        View.Icon32 = hex32;
        View.Icon16 = hex16;
        Pump();
        string shown = Convert.ToString(new IconTextConverter().ConvertTo(null, null, MenuDocument.Text(v, "ICON32"), typeof(string)));
        Check("그림 파일 -> 아이콘 32 x 32 / 16 x 16 PNG 로 바꿔 넣음", hex32.StartsWith("89 50 4E 47") && s32 == new Size(32, 32) && s16 == new Size(16, 16)
            && MenuDocument.Text(v, "ICON32") == hex32 && MenuDocument.Text(v, "ICON16") == hex16 && shown.StartsWith("32 x 32"), s32 + " / " + s16 + " / " + shown);
        string cleared;
        using (var f = new IconEditForm(hex32, 32))
        {
            typeof(IconEditForm).GetMethod("btnClear_Click", NP).Invoke(f, new object[] { null, EventArgs.Empty });
            cleared = f.HexValue;
        }
        Check("아이콘 편집 창 [지우기] = 빈 값", cleared == "", "'" + cleared + "'");

        // 9. 찾기
        ((TextBox)F("txtSearch")).Text = "MNU_SMOKE_RENAMED";
        Select(leaf);
        Call("FindNext");
        Pump();
        bool found = Selected == target;
        ((TextBox)F("txtSearch")).Text = "zz_not_found_zz";
        int d0 = DialogCount();
        Call("FindNext");
        Pump();
        string notFound = TakeDialogs(d0);
        Check("찾기: Key 로 찾아 선택 / 없으면 안내 후 선택 유지", found && notFound.Length > 0 && Selected == target, "찾음 = " + found + " / 안내 창 = " + notFound);

        // 10. 무작위 반복 변경 300회: 매번 트리 무결성 / 펼친 노드 유지 확인
        var rnd = new Random(20261015);
        string stressFail = null;
        int ops = 0;
        var opCounts = new int[11];
        for (int i = 0; i < 300 && stressFail == null; i++)
        {
            List<DataRow> rows = Doc.Rows.ToList();
            DataRow r = rows[rnd.Next(rows.Count)];
            int op = rnd.Next(11);
            HashSet<DataRow> expandedBefore = new HashSet<DataRow>(Map.Where(p => p.Value.IsExpanded).Select(p => p.Key));
            string label = "";
            try
            {
                Select(r);
                switch (op)
                {
                    case 0: label = "위로 " + K(r); Call("MoveSelected", -1); break;
                    case 1: label = "아래로 " + K(r); Call("MoveSelected", 1); break;
                    case 2:
                        List<string> candidates = Doc.ParentCandidates(r);
                        string np = candidates[rnd.Next(candidates.Count)];
                        label = "부모 " + K(r) + " -> " + np;
                        SetViaGrid("Popup(Parent) Menu", np);
                        break;
                    case 3: label = "같은 수준 추가 " + K(r); Call("AddMenu", false); break;
                    case 4: label = "하위 추가 " + K(r); Call("AddMenu", true); break;
                    case 5: label = "복제 " + K(r); Call("btnDuplicate_Click", null, EventArgs.Empty); break;
                    case 6:
                        if (Doc.Count > 80 && Doc.SubtreeKeys(r).Count <= 3) { label = "삭제 " + K(r); Call("DeleteSelected"); }
                        else { label = "삭제 생략"; }
                        break;
                    case 7: label = "Key " + K(r); SetViaGrid("Menu Key", "MNU_R" + i); break;
                    case 8: label = "Caption " + K(r); SetViaGrid("Caption 1", "C" + i); break;
                    case 9: label = "Visible " + K(r); SetViaGrid("Visible", MenuDocument.IsTrue(r, "VISIBLE") ? "False" : "True"); break;
                    default:
                        label = "펼침 전환 " + K(r);
                        TreeNode tn = Map[r];
                        if (tn.IsExpanded) tn.Collapse(); else tn.Expand();
                        break;
                }
                Pump();
            }
            catch (Exception ex)
            {
                stressFail = i + " " + label + " 예외 " + ex.GetType().Name + ": " + ex.Message;
                break;
            }
            ops++;
            opCounts[op]++;
            string integrity = Integrity();
            if (integrity != "OK") { stressFail = i + " " + label + " : " + integrity; break; }
            if (op != 10)
            {
                // 하위가 모두 다른 곳으로 옮겨져 자식이 없어진 노드는 펼칠 수 없으므로 제외한다.
                DataRow lost = expandedBefore.FirstOrDefault(x => x.RowState != DataRowState.Detached && x.RowState != DataRowState.Deleted && Map.ContainsKey(x)
                    && Map[x].Nodes.Count > 0 && !Map[x].IsExpanded);
                if (lost != null) { stressFail = i + " " + label + " : 펼친 노드가 접힘 " + K(lost); break; }
            }
            if ((op == 0 || op == 1 || op == 2 || op == 7 || op == 8 || op == 9) && Selected != r) { stressFail = i + " " + label + " : 선택이 바뀜"; break; }
        }
        List<MenuProblem> stressProblems = Doc.Validate(null);
        int levelWarn = stressProblems.Count(p => !p.IsError && p.Message.StartsWith("Menu Level"));
        Check("무작위 변경 300회 동안 트리 무결성 / 선택 / 펼침 유지", stressFail == null, stressFail ?? ("수행 " + ops + "회 / 메뉴 " + Doc.Count + " / 동작별 " + string.Join(",", opCounts.Select(x => x.ToString()).ToArray())));
        Check("무작위 변경 뒤 검증 오류 0 / Level 경고 늘지 않음", stressProblems.Count(p => p.IsError) == 0 && levelWarn <= baseLevelWarn,
            "오류 " + stressProblems.Count(p => p.IsError) + " / Level 경고 " + baseLevelWarn + " -> " + levelWarn
            + (stressProblems.Any(p => p.IsError) ? " / " + stressProblems.First(p => p.IsError) : ""));

        // 11. 저장(백업 / 형식) -> 다시 읽기 같음
        byte[] beforeSave = File.ReadAllBytes(workFile);
        string backupDir = Path.Combine(Path.GetTempPath(), @"MenuFileEditor\backup");
        // File.Copy 는 수정 시각을 원본 그대로 두므로, 저장 전후 백업 폴더 목록을 비교해 새로 생긴 파일을 찾는다.
        var backupsBefore = new HashSet<string>(Directory.Exists(backupDir) ? Directory.GetFiles(backupDir) : new string[0], StringComparer.OrdinalIgnoreCase);
        int d1 = DialogCount();
        bool saved = (bool)Call("SaveTo", workFile);
        Pump();
        string saveDialog = TakeDialogs(d1);
        List<FileInfo> created = new DirectoryInfo(backupDir).GetFiles("MenuFile_*.mnu").Where(x => !backupsBefore.Contains(x.FullName)).ToList();
        FileInfo newest = created.Count == 1 ? created[0] : null;
        bool backupOk = newest != null && File.ReadAllBytes(newest.FullName).SequenceEqual(beforeSave);
        Check("저장: 저장 전 파일 백업 / 변경 표시 지움", saved && saveDialog.Length > 0 && backupOk && !Doc.IsDirty && !form.Text.EndsWith("*"),
            "저장 = " + saved + " / 백업 = " + (newest == null ? "없음" : newest.Name) + " / 제목 " + form.Text);
        byte[] savedBytes = File.ReadAllBytes(workFile);
        MenuDocument reloaded = MenuDocument.Load(workFile);
        List<DataRow> a = Doc.Rows.ToList(), b = reloaded.Rows.ToList();
        bool same = a.Count == b.Count && Enumerable.Range(0, a.Count).All(i => MenuDocument.ColumnNames.All(col => MenuDocument.Text(a[i], col) == MenuDocument.Text(b[i], col)));
        Check("저장한 파일을 다시 읽으면 화면 값과 같음 / 다시 저장해도 같은 바이트", same && reloaded.Serialize().SequenceEqual(savedBytes), "메뉴 " + a.Count + " / " + b.Count);
        string head = Encoding.UTF8.GetString(savedBytes, 0, 40);
        bool noBom = !(savedBytes[0] == 0xEF && savedBytes[1] == 0xBB);
        bool crlf = Encoding.UTF8.GetString(savedBytes).Contains("\r\n");
        bool endSame = (originalEndsWithNewline(savedBytes) == originalEndsWithNewline(File.ReadAllBytes(Path.Combine(workDir, "MenuFile.original.mnu"))));
        var ds = new DataSet();
        ds.ReadXml(workFile);
        Check("파일 형식: BOM 없음 / standalone 선언 / CRLF / DataSet.ReadXml(DACrux 방식)로 읽힘",
            noBom && head.StartsWith("<?xml version=\"1.0\" standalone=\"yes\"?>") && crlf && endSame && ds.Tables.Contains("Table1") && ds.Tables["Table1"].Rows.Count == Doc.Count,
            "BOM 없음 = " + noBom + " / CRLF = " + crlf + " / Table1 " + (ds.Tables.Contains("Table1") ? ds.Tables["Table1"].Rows.Count : -1));

        // 12. 다른 곳에서 파일이 바뀐 뒤 저장하면 확인(아니요 = 저장 안 함)
        Select(v);
        SetViaGrid("Caption 2", "외부 변경 확인");
        File.SetLastWriteTimeUtc(workFile, DateTime.UtcNow.AddMinutes(5));
        byte[] external = File.ReadAllBytes(workFile);
        int d2 = DialogCount();
        bool overwritten = (bool)Call("SaveTo", workFile);
        Pump();
        string externalDialog = TakeDialogs(d2);
        Check("열고 난 뒤 파일이 바뀌었으면 덮어쓸지 확인(아니요면 저장 안 함)", !overwritten && externalDialog.Length > 0 && File.ReadAllBytes(workFile).SequenceEqual(external) && Doc.IsDirty,
            "확인 창 = " + externalDialog);

        // 13. 다시 열기 / 없는 파일 열기 실패 시 기존 트리 유지
        Call("OpenFile", workFile);
        Pump();
        bool reopened = !Doc.IsDirty && Integrity() == "OK" && Doc.Count == a.Count;
        MenuDocument current = Doc;
        int d3 = DialogCount();
        bool opened = (bool)Call("OpenFile", Path.Combine(workDir, "no_such_file.mnu"));
        Pump();
        string openDialog = TakeDialogs(d3);
        Check("다시 열기 / 없는 파일을 열면 안내 후 지금 트리 그대로", reopened && !opened && openDialog.Length > 0 && Doc == current && Integrity() == "OK",
            "다시 열기 = " + reopened + " / 안내 창 = " + openDialog + " / " + Integrity());
    }

    static bool originalEndsWithNewline(byte[] bytes)
    {
        return bytes.Length > 0 && bytes[bytes.Length - 1] == (byte)'\n';
    }

    static bool IsUnder(TreeNode node, TreeNode ancestor)
    {
        for (TreeNode n = node.Parent; n != null; n = n.Parent) if (n == ancestor) return true;
        return false;
    }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($exe, 'System.Windows.Forms', 'System.Drawing', 'System.Data', 'System.Xml', 'System.Core')

Copy-Item $MenuFile (Join-Path $work 'MenuFile.original.mnu') -Force
$lastPath = Join-Path $env:LOCALAPPDATA 'MenuFileEditor\last_path.txt'
$lastBackup = $null
if (Test-Path $lastPath) { $lastBackup = [IO.File]::ReadAllBytes($lastPath) }
try {
    $results = [MenuEditorSmoke]::Run($MenuFile, $workFile, $work)
}
finally {
    if ($null -ne $lastBackup) { [IO.File]::WriteAllBytes($lastPath, $lastBackup) }
    elseif (Test-Path $lastPath) { Remove-Item $lastPath -Force }
}
foreach ($line in $results) { Write-Host $line }
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "MENU_FILE_EDITOR_SMOKE=PASS ($($results.Count))" } else { Write-Host "MENU_FILE_EDITOR_SMOKE=FAIL ($fail)"; exit 1 }
