using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MenuFileEditor
{
    /// <summary>
    /// DACrux 메뉴 파일(MenuFile.mnu) 한 개를 다루는 문서 클래스 (2026-10-15).
    /// 목적: 화면(트리 / 속성 창)과 파일 처리 로직을 나눠, 화면 없이도 같은 동작을 검사할 수 있게 한다.
    /// 파일 형식: DataTable.WriteXml(파일, WriteSchema) 결과(XML 선언 standalone / NewDataSet / 인라인 스키마(MainDataTable = Table1) / Table1 행,
    ///   UTF-8 BOM 없음, CRLF, 2칸 들여쓰기). 기존 메뉴 편집 도구와 같은 방식이고, DACruxMain.GetMenu 가 DataSet.ReadXml 로 그대로 읽는다.
    ///   아무것도 바꾸지 않고 저장하면 기존 도구로 저장한 것과 바이트 단위로 같다(손으로 고친 줄의 태그 사이 공백만 정리됨, 자동 검사 항목).
    /// 메뉴 구조: POPUP_MENU = 부모 MENU_KEY (최상위는 "ROOT"). 형제 순서 = MNU_ORDER 오름차순(같으면 파일 행 순서).
    ///   DACrux 의 MenuHelper.InitMenu 는 최상위를 MNU_ORDER 위치에 끼워 넣고, 하위는 (POPUP_MENU, MNU_ORDER) 순서대로 붙인다.
    /// 변경 원칙: 사용자가 바꾼 값과 그에 딸린 값(하위 Parent, 형제 순서, Level)만 바꾸고 파일의 행 순서는 바꾸지 않는다.
    /// </summary>
    public sealed class MenuDocument
    {
        /// <summary>최상위 메뉴의 POPUP_MENU 값.</summary>
        public const string Root = "ROOT";

        /// <summary>Table1 의 열(파일 스키마 순서). 문자열 열 외에 MNU_ORDER / MNU_LEVEL 은 정수.</summary>
        public static readonly string[] ColumnNames =
        {
            "MENU_KEY", "MENU_ID", "POPUP_MENU", "STOCK_TOOLBAR", "CAPTION001", "CAPTION002", "CAPTION003", "CAPTION004", "CAPTION005",
            "TOOL_TIP", "ICON32", "ICON16", "FORM_TYPE", "MODULE", "ASSEMBLY", "HELP_URL", "MNU_ORDER", "MNU_LEVEL", "FUNCTION_CODE",
            "RIBBON_TAB", "RIBBON_GROUP", "RESV_01", "RESV_02", "RESV_03", "RESV_04", "RESV_05", "ENABLE", "VISIBLE", "IS_START", "MULTI"
        };

        /// <summary>True / False 문자열을 쓰는 열.</summary>
        public static readonly string[] BoolColumns = { "ENABLE", "VISIBLE", "IS_START", "MULTI" };

        /// <summary>DACrux 프레임워크가 직접 처리해서 MODULE / ASSEMBLY 가 없어도 되는 메뉴(로그오프 / 종료 / 창 / 도움말).</summary>
        private static readonly string[] FrameworkKeys = { "MNU_FILE_LOGOUT", "MNU_FILE_EXIT", "MNU_WINDOWS_TAB_VIEW", "MNU_WINDOWS_MDI_WINDOWS_LIST", "MNU_HELP_ABOUT_DACRUX", "MNU_HELP_MANUAL" };

        private readonly DataTable table;

        private MenuDocument(DataTable table, string path)
        {
            this.table = table;
            FilePath = path;
        }

        /// <summary>읽은(또는 마지막으로 저장한) 파일 경로.</summary>
        public string FilePath { get; private set; }

        /// <summary>저장하지 않은 변경이 있는지.</summary>
        public bool IsDirty { get; private set; }

        /// <summary>값이 바뀌었을 때(화면이 해당 노드만 다시 그린다).</summary>
        public event EventHandler<MenuChangedEventArgs> Changed;

        /// <summary>모든 행(파일 순서).</summary>
        public IEnumerable<DataRow> Rows
        {
            get { return table.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted); }
        }

        public int Count
        {
            get { return Rows.Count(); }
        }

        #region [읽기 / 저장]

        /// <summary>
        /// 메뉴 파일을 읽는다. 스키마가 들어 있으면 그대로 쓰고, 빠진 열은 문자열 열로 더한다(값은 빈 문자열).
        /// 읽기만 하고 값은 바꾸지 않는다(순서 / Level 정리도 하지 않음).
        /// </summary>
        public static MenuDocument Load(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new FileNotFoundException("메뉴 파일이 없습니다.", path);

            var ds = new DataSet();
            ds.ReadXml(path, XmlReadMode.ReadSchema);
            if (ds.Tables.Count == 0)
                throw new InvalidDataException("메뉴 파일에 Table1 이 없습니다: " + path);
            DataTable t = ds.Tables.Contains("Table1") ? ds.Tables["Table1"] : ds.Tables[0];
            foreach (string name in ColumnNames)
            {
                if (t.Columns.Contains(name)) continue;
                DataColumn column = t.Columns.Add(name, name == "MNU_ORDER" || name == "MNU_LEVEL" ? typeof(int) : typeof(string));
                foreach (DataRow row in t.Rows) row[column] = column.DataType == typeof(int) ? (object)0 : string.Empty;
            }
            t.AcceptChanges();
            return new MenuDocument(t, path);
        }

        /// <summary>
        /// 파일 내용(바이트). DataTable.WriteXml(파일 경로) 과 같은 순서로 쓴다:
        /// UTF-8(BOM 없음, encoding 속성 없음) / XML 선언 standalone="yes" / 2칸 들여쓰기 / CRLF / 끝 줄바꿈 없음.
        /// DataSet.WriteXml 로 쓰면 스키마의 MainDataTable 속성이 빠지므로 DataTable 로 쓴다.
        /// </summary>
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                // XmlTextWriter 에 인코딩을 넘기지 않으면 UTF-8(BOM 없음)로 쓰고 선언에 encoding 속성을 넣지 않는다(원본 파일과 같음).
                var writer = new XmlTextWriter(stream, null) { Formatting = Formatting.Indented, Indentation = 2, IndentChar = ' ' };
                writer.WriteStartDocument(true);
                table.WriteXml(writer, XmlWriteMode.WriteSchema);
                writer.WriteEndDocument();
                writer.Flush();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 파일로 저장한다. 순서: 검증 오류가 있으면 저장하지 않음 -> 기존 파일을 백업 폴더에 복사 -> 같은 폴더의 임시 파일에 쓴 뒤 바꿔치기.
        /// 중간에 실패해도 원래 파일이 반쯤 쓰인 상태로 남지 않는다.
        /// </summary>
        /// <returns>백업 파일 경로(기존 파일이 없었으면 null)</returns>
        public string Save(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("저장할 경로가 없습니다.");
            List<MenuProblem> errors = Validate(null).Where(p => p.IsError).ToList();
            if (errors.Count > 0)
                throw new InvalidOperationException("오류 " + errors.Count + "건이 있어 저장하지 않았습니다.\r\n" + string.Join("\r\n", errors.Take(10).Select(p => p.ToString()).ToArray()));

            byte[] bytes = Serialize();
            string backup = null;
            if (File.Exists(path))
            {
                backup = BackupFile(path);
            }
            string temp = path + ".saving";
            File.WriteAllBytes(temp, bytes);
            if (File.Exists(path))
            {
                File.Copy(temp, path, true);
                File.Delete(temp);
            }
            else
            {
                File.Move(temp, path);
            }
            FilePath = path;
            table.AcceptChanges();
            IsDirty = false;
            return backup;
        }

        /// <summary>저장 전 원본을 %TEMP%\MenuFileEditor\backup 에 날짜 붙여 복사한다. 최근 30개만 남긴다.</summary>
        private static string BackupFile(string path)
        {
            string dir = Path.Combine(Path.GetTempPath(), "MenuFileEditor", "backup");
            Directory.CreateDirectory(dir);
            string name = Path.GetFileNameWithoutExtension(path) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture) + Path.GetExtension(path);
            string target = Path.Combine(dir, name);
            File.Copy(path, target, true);
            try
            {
                foreach (FileInfo old in new DirectoryInfo(dir).GetFiles().OrderByDescending(f => f.CreationTimeUtc).Skip(30))
                    old.Delete();
            }
            catch (IOException)
            {
                // 오래된 백업 정리 실패는 저장에 영향을 주지 않는다.
            }
            return target;
        }

        #endregion

        #region [조회]

        public static string Text(DataRow row, string column)
        {
            object value = row[column];
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public static string Key(DataRow row)
        {
            return Text(row, "MENU_KEY");
        }

        public static string Parent(DataRow row)
        {
            return Text(row, "POPUP_MENU");
        }

        public static int Order(DataRow row)
        {
            object value = row["MNU_ORDER"];
            if (value == null || value == DBNull.Value) return int.MaxValue;
            int order;
            return int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out order) ? order : int.MaxValue;
        }

        public static bool IsTrue(DataRow row, string column)
        {
            return string.Equals(Text(row, column), "True", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>MENU_KEY 로 행을 찾는다(대소문자 구분). 없으면 null.</summary>
        public DataRow Find(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return Rows.FirstOrDefault(r => Key(r) == key);
        }

        /// <summary>parentKey 의 자식 행(형제 순서). parentKey = ROOT 면 최상위.</summary>
        public List<DataRow> Children(string parentKey)
        {
            List<DataRow> all = Rows.ToList();
            return all.Where(r => Parent(r) == parentKey)
                .Select(r => new { Row = r, Index = all.IndexOf(r) })
                .OrderBy(x => Order(x.Row)).ThenBy(x => x.Index)
                .Select(x => x.Row).ToList();
        }

        /// <summary>
        /// 트리에 붙지 못하는 행(부모가 없거나 순환). 화면은 이 행들을 "부모 없음" 아래에 모아 보여 준다(조회에서 빠지지 않게).
        /// </summary>
        public List<DataRow> Orphans()
        {
            var reachable = new HashSet<DataRow>();
            var stack = new Stack<string>();
            stack.Push(Root);
            var visitedKeys = new HashSet<string>(StringComparer.Ordinal);
            while (stack.Count > 0)
            {
                string parent = stack.Pop();
                if (!visitedKeys.Add(parent)) continue;
                foreach (DataRow child in Children(parent))
                {
                    if (!reachable.Add(child)) continue;
                    string key = Key(child);
                    if (key.Length > 0) stack.Push(key);
                }
            }
            return Rows.Where(r => !reachable.Contains(r)).ToList();
        }

        /// <summary>row 의 깊이(최상위 = 1). 부모를 따라가다 끊기거나 순환이면 그때까지의 깊이.</summary>
        public int Depth(DataRow row)
        {
            int depth = 1;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            string parent = Parent(row);
            while (parent != Root && parent.Length > 0 && seen.Add(parent))
            {
                DataRow p = Find(parent);
                if (p == null) break;
                depth++;
                parent = Parent(p);
            }
            return depth;
        }

        /// <summary>row 와 그 아래 모든 행의 MENU_KEY.</summary>
        public HashSet<string> SubtreeKeys(DataRow row)
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);
            var stack = new Stack<string>();
            stack.Push(Key(row));
            while (stack.Count > 0)
            {
                string key = stack.Pop();
                if (!keys.Add(key)) continue;
                foreach (DataRow child in Children(key))
                    stack.Push(Key(child));
            }
            return keys;
        }

        /// <summary>row 의 부모로 고를 수 있는 값: ROOT + 자기 자신 / 하위가 아닌 모든 MENU_KEY.</summary>
        public List<string> ParentCandidates(DataRow row)
        {
            HashSet<string> blocked = row == null ? new HashSet<string>() : SubtreeKeys(row);
            var list = new List<string> { Root };
            list.AddRange(Rows.Select(Key).Where(k => k.Length > 0 && !blocked.Contains(k)).Distinct().OrderBy(k => k, StringComparer.Ordinal));
            return list;
        }

        #endregion

        #region [변경]

        /// <summary>
        /// 열 하나를 바꾼다. MENU_KEY / POPUP_MENU / MNU_ORDER / MNU_LEVEL 은 전용 함수(RenameKey / ChangeParent / Move)로만 바꾼다.
        /// 값이 같으면 아무것도 하지 않는다.
        /// </summary>
        public void SetValue(DataRow row, string column, object value)
        {
            if (column == "MENU_KEY" || column == "POPUP_MENU" || column == "MNU_ORDER" || column == "MNU_LEVEL")
                throw new InvalidOperationException(column + " 은 전용 기능으로만 바꿉니다.");
            if (!table.Columns.Contains(column)) throw new ArgumentException("없는 열: " + column);
            object newValue = value ?? string.Empty;
            if (BoolColumns.Contains(column))
                newValue = Convert.ToBoolean(newValue, CultureInfo.InvariantCulture) ? "True" : "False";
            if (Equals(Text(row, column), Convert.ToString(newValue, CultureInfo.InvariantCulture))) return;
            row[column] = newValue;
            MarkChanged(row, false);
        }

        /// <summary>
        /// MENU_KEY 를 바꾼다. 하위 메뉴의 POPUP_MENU 도 같이 바꾸고, MENU_ID 가 예전 Key 와 같으면 MENU_ID 도 바꾼다.
        /// 빈 값 / 공백 포함 / 이미 있는 Key / ROOT 는 거부한다.
        /// </summary>
        public void RenameKey(DataRow row, string newKey)
        {
            newKey = (newKey ?? string.Empty).Trim();
            string oldKey = Key(row);
            if (newKey == oldKey) return;
            if (newKey.Length == 0) throw new ArgumentException("Menu Key 는 비울 수 없습니다.");
            if (newKey.Any(char.IsWhiteSpace)) throw new ArgumentException("Menu Key 에 공백을 넣을 수 없습니다.");
            if (newKey == Root) throw new ArgumentException("ROOT 는 Menu Key 로 쓸 수 없습니다.");
            if (Find(newKey) != null) throw new ArgumentException("이미 있는 Menu Key 입니다: " + newKey);

            foreach (DataRow child in Rows.Where(r => Parent(r) == oldKey).ToList())
                child["POPUP_MENU"] = newKey;
            if (Text(row, "MENU_ID") == oldKey) row["MENU_ID"] = newKey;
            row["MENU_KEY"] = newKey;
            MarkChanged(row, true);
        }

        /// <summary>
        /// 부모를 바꾼다. 자기 자신 / 하위 메뉴 아래로는 옮길 수 없다(순환 방지). 새 부모의 마지막 순서로 붙고,
        /// 예전 형제와 새 형제의 순서를 0부터 다시 매기며, 옮긴 메뉴와 그 하위의 MNU_LEVEL 을 깊이에 맞춘다.
        /// </summary>
        public void ChangeParent(DataRow row, string newParent)
        {
            newParent = (newParent ?? string.Empty).Trim();
            if (newParent.Length == 0) newParent = Root;
            string oldParent = Parent(row);
            if (newParent == oldParent) return;
            if (newParent != Root)
            {
                if (Find(newParent) == null) throw new ArgumentException("없는 부모 메뉴입니다: " + newParent);
                if (SubtreeKeys(row).Contains(newParent)) throw new ArgumentException("자기 자신이나 하위 메뉴 아래로는 옮길 수 없습니다.");
            }
            List<DataRow> newSiblings = Children(newParent);
            row["POPUP_MENU"] = newParent;
            newSiblings.Add(row);
            Renumber(newSiblings);
            Renumber(Children(oldParent));
            UpdateLevels(row);
            MarkChanged(row, true);
        }

        /// <summary>형제 안에서 한 칸 위(-1) / 아래(+1)로 옮긴다. 옮길 수 없으면 false.</summary>
        public bool Move(DataRow row, int direction)
        {
            List<DataRow> siblings = Children(Parent(row));
            int index = siblings.IndexOf(row);
            int target = index + Math.Sign(direction);
            if (index < 0 || target < 0 || target >= siblings.Count) return false;
            siblings.RemoveAt(index);
            siblings.Insert(target, row);
            Renumber(siblings);
            MarkChanged(row, true);
            return true;
        }

        /// <summary>형제의 MNU_ORDER 를 지금 순서대로 0, 1, 2 ... 로 다시 매긴다(같은 값 / 빈 값 정리).</summary>
        private static void Renumber(IList<DataRow> siblings)
        {
            for (int i = 0; i < siblings.Count; i++)
                if (Order(siblings[i]) != i) siblings[i]["MNU_ORDER"] = i;
        }

        /// <summary>row 와 하위의 MNU_LEVEL 을 깊이로 맞춘다.</summary>
        private void UpdateLevels(DataRow row)
        {
            foreach (string key in SubtreeKeys(row))
            {
                DataRow r = Find(key);
                if (r == null) continue;
                int depth = Depth(r);
                object current = r["MNU_LEVEL"];
                if (current == DBNull.Value || Convert.ToInt32(current, CultureInfo.InvariantCulture) != depth) r["MNU_LEVEL"] = depth;
            }
        }

        /// <summary>
        /// 새 메뉴를 만든다. parentKey 아래 afterRow 바로 다음 순서(afterRow 가 null 이면 마지막)에 넣는다.
        /// 기본값: Key = MNU_NEW_n, 보이기 / 사용 = True, FORM_TYPE = BASIC_FORM, 나머지 빈 값. 파일에는 맨 뒤 행으로 붙는다.
        /// </summary>
        public DataRow Add(string parentKey, DataRow afterRow)
        {
            parentKey = string.IsNullOrEmpty(parentKey) ? Root : parentKey;
            if (parentKey != Root && Find(parentKey) == null) throw new ArgumentException("없는 부모 메뉴입니다: " + parentKey);
            DataRow row = table.NewRow();
            foreach (DataColumn column in table.Columns)
                row[column] = column.DataType == typeof(int) ? (object)0 : string.Empty;
            string key = NewKey("MNU_NEW");
            row["MENU_KEY"] = key;
            row["MENU_ID"] = key;
            row["POPUP_MENU"] = parentKey;
            row["CAPTION001"] = "New Menu";
            row["FORM_TYPE"] = "BASIC_FORM";
            row["ENABLE"] = "True";
            row["VISIBLE"] = "True";
            row["IS_START"] = "False";
            row["MULTI"] = "False";
            InsertAt(row, parentKey, afterRow);
            return row;
        }

        /// <summary>source 를 복제해 바로 아래에 넣는다(하위 메뉴는 복제하지 않음). Key 는 원래 Key + "_COPY"(겹치면 번호).</summary>
        public DataRow Duplicate(DataRow source)
        {
            DataRow row = table.NewRow();
            row.ItemArray = (object[])source.ItemArray.Clone();
            string key = NewKey(Key(source) + "_COPY");
            row["MENU_KEY"] = key;
            if (Text(source, "MENU_ID") == Key(source)) row["MENU_ID"] = key;
            InsertAt(row, Parent(source), source);
            return row;
        }

        private void InsertAt(DataRow row, string parentKey, DataRow afterRow)
        {
            List<DataRow> siblings = Children(parentKey);
            table.Rows.Add(row);
            int index = afterRow == null ? siblings.Count : siblings.IndexOf(afterRow) + 1;
            if (index <= 0 || index > siblings.Count) index = siblings.Count;
            siblings.Insert(index, row);
            Renumber(siblings);
            row["MNU_LEVEL"] = Depth(row);
            MarkChanged(row, true);
        }

        /// <summary>prefix 로 시작하는 쓰지 않은 Key. prefix 자체가 비어 있으면 그대로, 아니면 _2, _3 ...</summary>
        private string NewKey(string prefix)
        {
            if (Find(prefix) == null) return prefix;
            for (int i = 2; ; i++)
            {
                string key = prefix + "_" + i.ToString(CultureInfo.InvariantCulture);
                if (Find(key) == null) return key;
            }
        }

        /// <summary>row 와 하위 메뉴를 모두 지운다. 남은 형제 순서를 다시 매긴다. 지운 행 수를 돌려준다.</summary>
        public int Delete(DataRow row)
        {
            string parent = Parent(row);
            List<DataRow> targets = SubtreeKeys(row).Select(Find).Where(r => r != null).ToList();
            if (!targets.Contains(row)) targets.Add(row);
            foreach (DataRow target in targets)
                table.Rows.Remove(target);
            Renumber(Children(parent));
            MarkChanged(null, true);
            return targets.Count;
        }

        private void MarkChanged(DataRow row, bool structure)
        {
            IsDirty = true;
            EventHandler<MenuChangedEventArgs> handler = Changed;
            if (handler != null) handler(this, new MenuChangedEventArgs(row, structure));
        }

        #endregion

        #region [검증]

        /// <summary>
        /// 파일을 DACrux 가 문제없이 읽을 수 있는지 확인한다.
        /// 오류(저장 막음): Key 비었음 / 중복 / 부모 없음 / 순환 / True / False 열 값 / 아이콘 그림으로 못 읽음.
        /// 경고: 형제 순서 중복 / Level 과 깊이 다름 / 화면 메뉴인데 MODULE / ASSEMBLY 비었음 / MODULE DLL 을 못 찾음.
        /// </summary>
        /// <param name="moduleFolders">MODULE DLL 을 찾을 폴더(없으면 DLL 확인 생략)</param>
        public List<MenuProblem> Validate(IEnumerable<string> moduleFolders)
        {
            var problems = new List<MenuProblem>();
            List<DataRow> rows = Rows.ToList();
            var keyCounts = rows.GroupBy(Key).ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
            List<string> folders = moduleFolders == null ? new List<string>() : moduleFolders.Where(Directory.Exists).ToList();

            foreach (DataRow row in rows)
            {
                string key = Key(row);
                if (key.Length == 0) problems.Add(new MenuProblem(true, key, "Menu Key 가 비었습니다."));
                else if (keyCounts[key] > 1) problems.Add(new MenuProblem(true, key, "Menu Key 가 " + keyCounts[key] + "번 있습니다."));

                string parent = Parent(row);
                if (parent.Length == 0) problems.Add(new MenuProblem(true, key, "Popup(Parent) Menu 가 비었습니다."));
                else if (parent != Root && !keyCounts.ContainsKey(parent)) problems.Add(new MenuProblem(true, key, "부모 메뉴가 없습니다: " + parent));

                foreach (string column in BoolColumns)
                {
                    string value = Text(row, column);
                    if (value != "True" && value != "False") problems.Add(new MenuProblem(true, key, column + " 값은 True / False 여야 합니다: '" + value + "'"));
                }

                foreach (string column in new[] { "ICON32", "ICON16" })
                {
                    string hex = Text(row, column);
                    if (hex.Trim().Length > 0 && HexToImage(hex) == null) problems.Add(new MenuProblem(true, key, column + " 를 그림으로 읽을 수 없습니다."));
                }

                bool hasChildren = key.Length > 0 && rows.Any(r => Parent(r) == key);
                string formType = Text(row, "FORM_TYPE");
                if (!hasChildren && parent != Root && (formType == "BASIC_FORM" || formType == "MODAL_FORM") && !FrameworkKeys.Contains(key)
                    && (Text(row, "MODULE").Length == 0 || Text(row, "ASSEMBLY").Length == 0))
                    problems.Add(new MenuProblem(false, key, "화면 메뉴인데 Module / Assembly 가 비었습니다(눌러도 아무것도 열리지 않음)."));

                string module = Text(row, "MODULE").Trim();
                if (module.Length > 0 && folders.Count > 0 && !folders.Any(f => File.Exists(Path.Combine(f, module))))
                    problems.Add(new MenuProblem(false, key, "Module 파일을 찾지 못했습니다: " + module));

                int depth = Depth(row);
                object level = row["MNU_LEVEL"];
                if (level != DBNull.Value && Convert.ToInt32(level, CultureInfo.InvariantCulture) != depth)
                    problems.Add(new MenuProblem(false, key, "Menu Level " + level + " 이 실제 깊이 " + depth + " 와 다릅니다."));
            }

            foreach (DataRow row in Orphans())
                if (Parent(row).Length > 0 && (Parent(row) == Root || keyCounts.ContainsKey(Parent(row))))
                    problems.Add(new MenuProblem(true, Key(row), "부모를 따라가면 순환합니다(자기 하위가 부모)."));

            foreach (var group in rows.GroupBy(r => Parent(r) + "|" + Order(r)).Where(g => g.Count() > 1))
                problems.Add(new MenuProblem(false, Key(group.First()), "같은 부모(" + Parent(group.First()) + ") 안에 Menu Order " + Order(group.First())
                    + " 가 " + group.Count() + "개 있습니다: " + string.Join(", ", group.Select(Key).ToArray())));
            return problems;
        }

        #endregion

        #region [아이콘]

        /// <summary>DACrux 아이콘 문자열("89 50 4E ..." 공백 구분 16진수)을 그림으로 바꾼다. 못 읽으면 null.</summary>
        public static Image HexToImage(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Trim().Length == 0) return null;
            try
            {
                byte[] bytes = HexToBytes(hex);
                using (var stream = new MemoryStream(bytes))
                using (Image image = Image.FromStream(stream))
                    return new Bitmap(image);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
        }

        /// <summary>DACrux.Base.Convert.Convert16ToByte 와 같은 규칙(공백으로 나눠 16진수 해석).</summary>
        public static byte[] HexToBytes(string hex)
        {
            string[] parts = hex.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var bytes = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                bytes[i] = (byte)int.Parse(parts[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return bytes;
        }

        /// <summary>바이트를 DACrux 아이콘 문자열("89 50 4E ...", 대문자 / 공백 구분)로 바꾼다.</summary>
        public static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 3);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(bytes[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        /// <summary>그림을 size x size PNG 로 줄여(비율 유지, 가운데 정렬, 투명 배경) 아이콘 문자열로 바꾼다.</summary>
        public static string ImageToHex(Image source, int size)
        {
            using (var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    float scale = Math.Min((float)size / source.Width, (float)size / source.Height);
                    float w = source.Width * scale, h = source.Height * scale;
                    g.DrawImage(source, (size - w) / 2f, (size - h) / 2f, w, h);
                }
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return BytesToHex(stream.ToArray());
                }
            }
        }

        #endregion
    }

    /// <summary>문서 변경 알림. Structure = 트리 모양(부모 / 순서 / Key / 추가 / 삭제)이 바뀌었는지.</summary>
    public sealed class MenuChangedEventArgs : EventArgs
    {
        public MenuChangedEventArgs(DataRow row, bool structure)
        {
            Row = row;
            Structure = structure;
        }

        public DataRow Row { get; private set; }
        public bool Structure { get; private set; }
    }

    /// <summary>검증 결과 한 건. IsError 면 저장을 막는다.</summary>
    public sealed class MenuProblem
    {
        public MenuProblem(bool isError, string key, string message)
        {
            IsError = isError;
            Key = key ?? string.Empty;
            Message = message;
        }

        public bool IsError { get; private set; }
        public string Key { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
        {
            return (IsError ? "[오류] " : "[경고] ") + (Key.Length == 0 ? "(Key 없음)" : Key) + " : " + Message;
        }
    }
}
