using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MenuFileEditor
{
    /// <summary>
    /// DACrux 메뉴 파일(MenuFile.mnu) 편집 화면 (2026-10-15).
    /// 목적: 메뉴 순서 / 부모 / 표시 옵션 / 캡션 / 아이콘 / 화면 연결 값을 트리와 속성 창으로 고치고 같은 형식으로 저장한다.
    /// 트리 유지 원칙(기존 편집 도구에서 왼쪽 트리가 사라지던 문제 방지):
    ///  - 데이터는 MenuDocument 하나만 쓰고, 트리 노드 Tag 에는 DataRow 를 그대로 둔다(Key 가 바뀌어도 같은 노드).
    ///  - 값만 바뀌면 해당 노드 글자 / 색만 고친다(트리 전체를 다시 만들지 않음).
    ///  - 부모 / 순서 / Key / 추가 / 삭제처럼 모양이 바뀌면 트리를 다시 만들되, 펼침 상태 / 선택 / 위쪽 위치를 그대로 되살린다.
    ///    속성 창에서 고친 경우에는 속성 창 처리가 끝난 뒤(BeginInvoke) 다시 만든다.
    ///  - 다시 만들기는 BeginUpdate / try / finally EndUpdate 로 감싸 중간에 오류가 나도 트리가 멈추거나 비지 않는다.
    ///  - 부모가 없거나 순환하는 메뉴도 "(부모 없음)" 아래에 보여서 조회에서 빠지지 않는다.
    /// </summary>
    public partial class MenuEditorForm : Form
    {
        private const string AppTitle = "Menu File Editor";

        private readonly Dictionary<DataRow, TreeNode> nodeMap = new Dictionary<DataRow, TreeNode>();
        private readonly string startPath;
        private MenuDocument document;
        private TreeNode rootNode;
        private TreeNode orphanNode;
        private DateTime loadedWriteTimeUtc;
        private bool rebuildPending;
        private bool rebuilding;
        private DataRow pendingSelect;

        public MenuEditorForm()
            : this(null)
        {
        }

        /// <param name="path">처음 열 메뉴 파일(없으면 마지막으로 연 파일 -> DACruxV5\MenuFile.mnu 순서로 찾음)</param>
        public MenuEditorForm(string path)
        {
            InitializeComponent();
            startPath = path;
            UpdateButtons();
            UpdateStatus();
        }

        /// <summary>지금 열린 문서(없으면 null).</summary>
        public MenuDocument Document
        {
            get { return document; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            string path = FindStartPath(startPath);
            if (path == null)
            {
                txtPath.Focus();
                return;
            }
            txtPath.Text = path;
            OpenFile(path);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == Keys.F3)
            {
                FindNext();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CommitGridEdit();
            if (!e.Cancel && !ConfirmDiscard()) e.Cancel = true;
            base.OnFormClosing(e);
        }

        #region [파일 열기 / 저장]

        /// <summary>
        /// 메뉴 파일을 연다. 실패하면 오류 창을 띄우고 지금 보던 문서와 트리는 그대로 둔다.
        /// </summary>
        public bool OpenFile(string path)
        {
            MenuDocument loaded;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                loaded = MenuDocument.Load(path);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "메뉴 파일을 읽을 수 없습니다.\r\n" + path + "\r\n\r\n" + ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (document != null) document.Changed -= Document_Changed;
            document = loaded;
            document.Changed += Document_Changed;
            loadedWriteTimeUtc = File.GetLastWriteTimeUtc(path);
            txtPath.Text = path;
            propertyGrid.SelectedObject = null;
            lstProblems.Items.Clear();
            pnlProblems.Visible = false;
            rebuildPending = false;
            pendingSelect = null;

            BuildTree(null, true);
            RememberPath(path);
            UpdateStatus();
            UpdateButtons();
            Cursor.Current = Cursors.Default;
            return true;
        }

        /// <summary>
        /// 저장한다. 검증 오류가 있으면 저장하지 않고 검증 결과를 보인다.
        /// 연 뒤에 다른 곳에서 파일이 바뀌었으면 덮어쓸지 묻는다.
        /// </summary>
        private bool SaveTo(string path)
        {
            if (document == null) return false;
            CommitGridEdit();
            RebuildIfPending();

            if (string.Equals(path, document.FilePath, StringComparison.OrdinalIgnoreCase) && File.Exists(path)
                && File.GetLastWriteTimeUtc(path) != loadedWriteTimeUtc)
            {
                DialogResult answer = MessageBox.Show(this, "파일을 연 뒤에 다른 곳에서 파일이 바뀌었습니다.\r\n지금 내용으로 덮어쓸까요?", AppTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (answer != DialogResult.Yes) return false;
            }

            List<MenuProblem> problems = document.Validate(ModuleFolders(path));
            if (problems.Any(p => p.IsError))
            {
                ShowProblems(problems);
                MessageBox.Show(this, "오류가 있어 저장하지 않았습니다. 아래 검증 결과를 확인해 주세요.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string backup;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                backup = document.Save(path);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "저장하지 못했습니다.\r\n" + path + "\r\n\r\n" + ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Cursor.Current = Cursors.Default;

            loadedWriteTimeUtc = File.GetLastWriteTimeUtc(path);
            txtPath.Text = path;
            if (rootNode != null) rootNode.Text = Path.GetFileName(path);
            RememberPath(path);
            UpdateStatus();
            if (pnlProblems.Visible) ShowProblems(problems);
            MessageBox.Show(this, "저장했습니다." + (backup == null ? string.Empty : "\r\n백업: " + backup), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        /// <summary>저장하지 않은 변경이 있으면 저장 / 버리기 / 취소를 묻는다. 계속해도 되면 true.</summary>
        private bool ConfirmDiscard()
        {
            if (document == null || !document.IsDirty) return true;
            DialogResult answer = MessageBox.Show(this, "저장하지 않은 변경이 있습니다. 저장할까요?", AppTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (answer == DialogResult.Cancel) return false;
            if (answer == DialogResult.No) return true;
            return SaveTo(document.FilePath);
        }

        /// <summary>속성 창에서 입력 중인 값이 있으면 확정한다(포커스를 트리로 옮기면 속성 창이 값을 반영한다).</summary>
        private void CommitGridEdit()
        {
            if (propertyGrid.ContainsFocus) treeMenu.Focus();
        }

        /// <summary>
        /// 처음 열 파일: 실행 인자 -> 마지막으로 연 파일 -> 실행 폴더에서 위로 올라가며 찾은 DACruxV5\MenuFile.mnu -> 실행 폴더의 MenuFile.mnu.
        /// </summary>
        private static string FindStartPath(string requested)
        {
            if (!string.IsNullOrEmpty(requested)) return Path.GetFullPath(requested);

            string last = ReadLastPath();
            if (last != null && File.Exists(last)) return last;

            string dir = Application.StartupPath;
            for (int i = 0; i < 6 && !string.IsNullOrEmpty(dir); i++)
            {
                foreach (string candidate in new[] { Path.Combine(dir, @"DACruxV5\MenuFile.mnu"), Path.Combine(dir, @"01_WinApp\DACruxV5\MenuFile.mnu") })
                    if (File.Exists(candidate)) return Path.GetFullPath(candidate);
                dir = Path.GetDirectoryName(dir);
            }
            string local = Path.Combine(Application.StartupPath, "MenuFile.mnu");
            return File.Exists(local) ? local : null;
        }

        private static string LastPathFile
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"MenuFileEditor\last_path.txt"); }
        }

        private static string ReadLastPath()
        {
            try
            {
                return File.Exists(LastPathFile) ? File.ReadAllText(LastPathFile).Trim() : null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        private static void RememberPath(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LastPathFile));
                File.WriteAllText(LastPathFile, path);
            }
            catch (IOException)
            {
                // 마지막 경로 기억은 편의 기능이라 실패해도 무시한다.
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        /// <summary>
        /// Module DLL 을 찾을 폴더: 메뉴 파일 폴더 / ..\..\100_ClientBin(DACruxV5 원본 위치 기준) / ..\100_ClientBin.
        /// DACrux DLL 이 하나도 없는 폴더는 빼서, 임시 폴더에서 열었을 때 모든 메뉴가 경고로 나오지 않게 한다.
        /// </summary>
        private static List<string> ModuleFolders(string menuPath)
        {
            var folders = new List<string>();
            if (string.IsNullOrEmpty(menuPath)) return folders;
            string dir = Path.GetDirectoryName(Path.GetFullPath(menuPath));
            foreach (string candidate in new[] { dir, Path.Combine(dir, @"..\..\100_ClientBin"), Path.Combine(dir, @"..\100_ClientBin") })
            {
                try
                {
                    string full = Path.GetFullPath(candidate);
                    if (Directory.Exists(full) && Directory.GetFiles(full, "DACrux*.dll").Length > 0 && !folders.Contains(full, StringComparer.OrdinalIgnoreCase))
                        folders.Add(full);
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
            return folders;
        }

        #endregion

        #region [트리]

        /// <summary>트리에서 선택한 메뉴 행(최상위 / "부모 없음" 노드면 null).</summary>
        private DataRow SelectedRow
        {
            get
            {
                TreeNode node = treeMenu.SelectedNode;
                DataRow row = node == null ? null : node.Tag as DataRow;
                return IsAlive(row) ? row : null;
            }
        }

        private static bool IsAlive(DataRow row)
        {
            return row != null && row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached;
        }

        /// <summary>
        /// 트리를 문서 내용으로 다시 만든다.
        /// 처음 열 때(first)는 모두 펼치고, 그 밖에는 펼침 상태 / 위쪽 노드 / 선택을 되살린다(select 가 있으면 그 행을 선택).
        /// </summary>
        private void BuildTree(DataRow select, bool first)
        {
            if (document == null) return;
            var expanded = new HashSet<DataRow>();
            bool rootExpanded = true;
            bool orphanExpanded = true;
            DataRow topRow = null;
            bool topIsRoot = false;
            if (!first)
            {
                foreach (KeyValuePair<DataRow, TreeNode> pair in nodeMap)
                    if (pair.Value.IsExpanded) expanded.Add(pair.Key);
                if (rootNode != null) rootExpanded = rootNode.IsExpanded;
                if (orphanNode != null) orphanExpanded = orphanNode.IsExpanded;
                TreeNode top = treeMenu.TopNode;
                if (top != null)
                {
                    topRow = top.Tag as DataRow;
                    topIsRoot = top == rootNode;
                }
                if (select == null) select = SelectedRow;
            }

            rebuilding = true;
            treeMenu.BeginUpdate();
            try
            {
                treeMenu.Nodes.Clear();
                nodeMap.Clear();
                rootNode = new TreeNode(Path.GetFileName(document.FilePath));
                treeMenu.Nodes.Add(rootNode);
                AddChildren(rootNode, MenuDocument.Root);

                orphanNode = null;
                List<DataRow> orphans = document.Orphans();
                if (orphans.Count > 0)
                {
                    orphanNode = new TreeNode("(부모 없음) " + orphans.Count.ToString(CultureInfo.InvariantCulture) + "개") { ForeColor = Color.Firebrick };
                    treeMenu.Nodes.Add(orphanNode);
                    var orphanKeys = new HashSet<string>(orphans.Select(MenuDocument.Key), StringComparer.Ordinal);
                    // 부모가 아예 없는 행을 먼저 붙이고(하위 포함), 순환으로 남은 행은 그다음에 붙인다.
                    foreach (DataRow row in orphans.Where(r => !orphanKeys.Contains(MenuDocument.Parent(r))).Concat(orphans))
                    {
                        if (nodeMap.ContainsKey(row)) continue;
                        TreeNode node = CreateNode(row);
                        orphanNode.Nodes.Add(node);
                        AddChildren(node, MenuDocument.Key(row));
                    }
                }

                if (first)
                {
                    treeMenu.ExpandAll();
                }
                else
                {
                    if (rootExpanded) rootNode.Expand();
                    if (orphanNode != null && orphanExpanded) orphanNode.Expand();
                    foreach (DataRow row in expanded)
                    {
                        TreeNode node;
                        if (nodeMap.TryGetValue(row, out node)) node.Expand();
                    }
                }

                TreeNode topNode;
                if (topRow != null && nodeMap.TryGetValue(topRow, out topNode)) treeMenu.TopNode = topNode;
                else if (topIsRoot || first) treeMenu.TopNode = rootNode;

                TreeNode selectNode;
                if (select != null && nodeMap.TryGetValue(select, out selectNode))
                {
                    treeMenu.SelectedNode = selectNode;
                    selectNode.EnsureVisible();
                }
                else
                {
                    treeMenu.SelectedNode = rootNode;
                }
            }
            finally
            {
                treeMenu.EndUpdate();
                rebuilding = false;
            }
            ShowRow(SelectedRow);
            UpdateButtons();
        }

        /// <summary>parentKey 의 하위 행을 형제 순서대로 붙인다. 이미 붙은 행(중복 Key / 순환)은 건너뛴다.</summary>
        private void AddChildren(TreeNode parentNode, string parentKey)
        {
            if (string.IsNullOrEmpty(parentKey)) return;
            foreach (DataRow child in document.Children(parentKey))
            {
                if (nodeMap.ContainsKey(child)) continue;
                TreeNode node = CreateNode(child);
                parentNode.Nodes.Add(node);
                AddChildren(node, MenuDocument.Key(child));
            }
        }

        private TreeNode CreateNode(DataRow row)
        {
            var node = new TreeNode { Tag = row };
            ApplyNodeStyle(node, row);
            nodeMap[row] = node;
            return node;
        }

        /// <summary>노드 글자 "[MENU_KEY] Caption 1", 보이지 않는 메뉴(VISIBLE = False)는 회색.</summary>
        private static void ApplyNodeStyle(TreeNode node, DataRow row)
        {
            string caption = MenuDocument.Text(row, "CAPTION001");
            string text = "[" + MenuDocument.Key(row) + "]" + (caption.Length == 0 ? string.Empty : " " + caption);
            if (node.Text != text) node.Text = text;
            Color color = MenuDocument.IsTrue(row, "VISIBLE") ? SystemColors.WindowText : Color.Gray;
            if (node.ForeColor != color) node.ForeColor = color;
        }

        /// <summary>
        /// 문서 변경 알림. 값만 바뀌면 그 노드만 고치고, 모양이 바뀌면 트리 다시 만들기를 한 번만 예약한다
        /// (속성 창 값 반영 중에 트리 / 속성 창을 바꾸지 않도록 BeginInvoke 로 뒤로 미룸).
        /// </summary>
        private void Document_Changed(object sender, MenuChangedEventArgs e)
        {
            if (e.Structure)
            {
                if (!rebuildPending)
                {
                    rebuildPending = true;
                    if (IsHandleCreated) BeginInvoke(new MethodInvoker(RebuildIfPending));
                }
            }
            else if (e.Row != null)
            {
                TreeNode node;
                if (nodeMap.TryGetValue(e.Row, out node)) ApplyNodeStyle(node, e.Row);
            }
            UpdateStatus();
        }

        /// <summary>
        /// 예약된 트리 다시 만들기가 있으면 지금 한다(버튼 동작 직후에도 부른다).
        /// 선택은 지금 선택한 메뉴를 우선한다(속성 창 값이 반영된 직후 다른 메뉴를 눌렀으면 그 메뉴 유지).
        /// 선택한 메뉴를 지운 경우에만 미리 정한 다음 메뉴(pendingSelect)를 고른다.
        /// </summary>
        private void RebuildIfPending()
        {
            if (!rebuildPending || IsDisposed) return;
            rebuildPending = false;
            DataRow select = SelectedRow ?? (IsAlive(pendingSelect) ? pendingSelect : null);
            pendingSelect = null;
            BuildTree(select, false);
        }

        /// <summary>
        /// 속성 창에 row 를 보인다. 이미 같은 행이면 객체를 바꾸지 않고 다시 그리기만 한다(편집 중인 칸 / 펼침 상태 유지).
        /// </summary>
        private void ShowRow(DataRow row)
        {
            var current = propertyGrid.SelectedObject as MenuItemView;
            if (row == null)
            {
                if (current != null) propertyGrid.SelectedObject = null;
                return;
            }
            if (current != null && current.Row == row && current.Document == document)
                propertyGrid.Refresh();
            else
                propertyGrid.SelectedObject = new MenuItemView(document, row);
        }

        private void SelectRow(DataRow row)
        {
            TreeNode node;
            if (row == null || !nodeMap.TryGetValue(row, out node)) return;
            treeMenu.SelectedNode = node;
            node.EnsureVisible();
        }

        private void treeMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (rebuilding) return;
            ShowRow(SelectedRow);
            UpdateButtons();
        }

        private void treeMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 오른쪽 클릭한 노드를 선택해서 오른쪽 메뉴가 그 노드에 동작하게 한다.
            if (e.Button == MouseButtons.Right && e.Node != null) treeMenu.SelectedNode = e.Node;
        }

        private void treeMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                MoveSelected(e.KeyCode == Keys.Up ? -1 : 1);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Delete && !e.Control && !e.Alt && !e.Shift)
            {
                btnDelete_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ctxTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (document == null)
            {
                e.Cancel = true;
                return;
            }
            DataRow row = SelectedRow;
            bool onRoot = treeMenu.SelectedNode == rootNode;
            mnuAddSibling.Enabled = row != null;
            mnuAddChild.Enabled = row != null || onRoot;
            mnuDuplicate.Enabled = row != null;
            mnuDelete.Enabled = row != null;
            mnuMoveUp.Enabled = btnMoveUp.Enabled;
            mnuMoveDown.Enabled = btnMoveDown.Enabled;
        }

        #endregion

        #region [메뉴 추가 / 이동 / 삭제]

        /// <summary>선택 메뉴를 형제 안에서 위(-1) / 아래(+1)로 옮긴다.</summary>
        private void MoveSelected(int direction)
        {
            DataRow row = SelectedRow;
            if (row == null) return;
            CommitGridEdit();
            if (document.Move(row, direction)) RebuildIfPending();
        }

        /// <summary>같은 수준 추가 = 선택 메뉴 바로 아래, 하위 추가 = 선택 메뉴(최상위 노드면 ROOT)의 마지막.</summary>
        private void AddMenu(bool child)
        {
            if (document == null) return;
            CommitGridEdit();
            DataRow row = SelectedRow;
            DataRow added;
            if (row == null)
            {
                if (treeMenu.SelectedNode != rootNode && treeMenu.SelectedNode != null) return;
                added = document.Add(MenuDocument.Root, null);
            }
            else if (child)
            {
                added = document.Add(MenuDocument.Key(row), null);
            }
            else
            {
                added = document.Add(MenuDocument.Parent(row), row);
            }
            RebuildIfPending();
            SelectRow(added);
        }

        /// <summary>선택 메뉴와 하위를 지운다. 다음 형제 -> 이전 형제 -> 부모 순서로 다음 선택을 정한다.</summary>
        private void DeleteSelected()
        {
            DataRow row = SelectedRow;
            if (row == null) return;
            List<DataRow> siblings = document.Children(MenuDocument.Parent(row));
            int index = siblings.IndexOf(row);
            DataRow next = index >= 0 && index + 1 < siblings.Count ? siblings[index + 1]
                : index > 0 ? siblings[index - 1]
                : document.Find(MenuDocument.Parent(row));

            propertyGrid.SelectedObject = null;
            document.Delete(row);
            pendingSelect = IsAlive(next) ? next : null;
            RebuildIfPending();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            MoveSelected(-1);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            MoveSelected(1);
        }

        private void btnAddSibling_Click(object sender, EventArgs e)
        {
            AddMenu(false);
        }

        private void btnAddChild_Click(object sender, EventArgs e)
        {
            AddMenu(true);
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            DataRow row = SelectedRow;
            if (row == null) return;
            CommitGridEdit();
            DataRow copy = document.Duplicate(row);
            RebuildIfPending();
            SelectRow(copy);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = SelectedRow;
            if (row == null) return;
            int count = document.SubtreeKeys(row).Count;
            string message = "[" + MenuDocument.Key(row) + "] 메뉴를 삭제할까요?"
                + (count > 1 ? "\r\n하위 메뉴 " + (count - 1).ToString(CultureInfo.InvariantCulture) + "개도 함께 삭제됩니다." : string.Empty);
            if (MessageBox.Show(this, message, AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            DeleteSelected();
        }

        #endregion

        #region [찾기 / 펼치기]

        /// <summary>지금 선택 다음 노드부터 Key / Caption 에 찾는 글자가 들어 있는 메뉴를 찾는다(끝까지 가면 처음부터).</summary>
        private void FindNext()
        {
            string text = txtSearch.Text.Trim();
            if (document == null || text.Length == 0) return;
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode node in treeMenu.Nodes) Flatten(node, nodes);
            int start = treeMenu.SelectedNode == null ? -1 : nodes.IndexOf(treeMenu.SelectedNode);
            for (int i = 1; i <= nodes.Count; i++)
            {
                TreeNode node = nodes[(start + i + nodes.Count) % nodes.Count];
                var row = node.Tag as DataRow;
                if (!IsAlive(row) || !Matches(row, text)) continue;
                treeMenu.SelectedNode = node;
                node.EnsureVisible();
                return;
            }
            MessageBox.Show(this, "'" + text + "' 을(를) 찾지 못했습니다.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void Flatten(TreeNode node, List<TreeNode> list)
        {
            list.Add(node);
            foreach (TreeNode child in node.Nodes) Flatten(child, list);
        }

        private static bool Matches(DataRow row, string text)
        {
            foreach (string column in new[] { "MENU_KEY", "CAPTION001", "CAPTION002", "CAPTION003", "ASSEMBLY" })
                if (MenuDocument.Text(row, column).IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            FindNext();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            TreeNode selected = treeMenu.SelectedNode;
            treeMenu.BeginUpdate();
            try
            {
                treeMenu.ExpandAll();
                if (selected != null) selected.EnsureVisible();
            }
            finally
            {
                treeMenu.EndUpdate();
            }
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            treeMenu.BeginUpdate();
            try
            {
                treeMenu.CollapseAll();
                if (rootNode != null) rootNode.Expand();
                if (treeMenu.SelectedNode != null) treeMenu.SelectedNode.EnsureVisible();
            }
            finally
            {
                treeMenu.EndUpdate();
            }
        }

        #endregion

        #region [검증]

        private void ShowProblems(List<MenuProblem> problems)
        {
            lstProblems.BeginUpdate();
            try
            {
                lstProblems.Items.Clear();
                foreach (MenuProblem problem in problems.OrderByDescending(p => p.IsError))
                    lstProblems.Items.Add(problem);
            }
            finally
            {
                lstProblems.EndUpdate();
            }
            int errors = problems.Count(p => p.IsError);
            lblProblems.Text = problems.Count == 0 ? "검증 결과: 문제 없음"
                : string.Format(CultureInfo.InvariantCulture, "검증 결과: 오류 {0}건 / 경고 {1}건", errors, problems.Count - errors);
            lblProblems.ForeColor = errors > 0 ? Color.Firebrick : SystemColors.ControlText;
            pnlProblems.Visible = true;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (document == null) return;
            CommitGridEdit();
            RebuildIfPending();
            ShowProblems(document.Validate(ModuleFolders(document.FilePath)));
        }

        private void lstProblems_DoubleClick(object sender, EventArgs e)
        {
            var problem = lstProblems.SelectedItem as MenuProblem;
            if (problem == null || document == null) return;
            DataRow row = document.Find(problem.Key);
            if (row != null) SelectRow(row);
        }

        private void btnCloseProblems_Click(object sender, EventArgs e)
        {
            pnlProblems.Visible = false;
        }

        #endregion

        #region [상단 / 하단 버튼]

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            btnReload_Click(sender, EventArgs.Empty);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "메뉴 파일 (*.mnu)|*.mnu|XML 파일 (*.xml)|*.xml|모든 파일 (*.*)|*.*";
                string current = txtPath.Text.Trim();
                if (current.Length > 0 && File.Exists(current))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(current);
                    dialog.FileName = Path.GetFileName(current);
                }
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                CommitGridEdit();
                if (!ConfirmDiscard()) return;
                OpenFile(dialog.FileName);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim().Trim('"');
            if (path.Length == 0) return;
            CommitGridEdit();
            if (!ConfirmDiscard()) return;
            OpenFile(path);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (document == null) return;
            SaveTo(document.FilePath);
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (document == null) return;
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "메뉴 파일 (*.mnu)|*.mnu|XML 파일 (*.xml)|*.xml|모든 파일 (*.*)|*.*";
                dialog.InitialDirectory = Path.GetDirectoryName(document.FilePath);
                dialog.FileName = Path.GetFileName(document.FilePath);
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                SaveTo(dialog.FileName);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdateButtons();
        }

        #endregion

        #region [상태 표시]

        private void UpdateButtons()
        {
            bool hasDocument = document != null;
            DataRow row = SelectedRow;
            bool canUp = false, canDown = false;
            if (row != null)
            {
                List<DataRow> siblings = document.Children(MenuDocument.Parent(row));
                int index = siblings.IndexOf(row);
                canUp = index > 0;
                canDown = index >= 0 && index < siblings.Count - 1;
            }
            btnMoveUp.Enabled = canUp;
            btnMoveDown.Enabled = canDown;
            btnAddSibling.Enabled = row != null;
            btnAddChild.Enabled = row != null || (hasDocument && treeMenu.SelectedNode == rootNode);
            btnDuplicate.Enabled = row != null;
            btnDelete.Enabled = row != null;
            btnValidate.Enabled = hasDocument;
            btnSave.Enabled = hasDocument;
            btnSaveAs.Enabled = hasDocument;
            btnFind.Enabled = hasDocument;
            btnExpandAll.Enabled = hasDocument;
            btnCollapseAll.Enabled = hasDocument;
        }

        private void UpdateStatus()
        {
            if (document == null)
            {
                Text = AppTitle;
                lblStatusFile.Text = string.Empty;
                lblStatusCount.Text = string.Empty;
                lblStatusDirty.Text = string.Empty;
                return;
            }
            Text = AppTitle + " - " + Path.GetFileName(document.FilePath) + (document.IsDirty ? " *" : string.Empty);
            lblStatusFile.Text = document.FilePath;
            lblStatusCount.Text = "메뉴 " + document.Count.ToString(CultureInfo.InvariantCulture) + "개";
            lblStatusDirty.Text = document.IsDirty ? "변경됨" : string.Empty;
        }

        #endregion
    }
}
