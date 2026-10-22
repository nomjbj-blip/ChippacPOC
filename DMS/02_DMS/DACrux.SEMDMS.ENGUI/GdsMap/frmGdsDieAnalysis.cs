using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// Die 상세 분석 화면(화면 B) 호스트. 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 7.3절,
    /// 도면 재사용: 문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md 5절,
    /// Die 목록 / DLL 읽기: 문서/설계/05_연관분석/2026-10-22_Die상세_DLL로딩_Die목록_설계.md 4 · 5절.
    /// 오른쪽에 GDS 화면 DLL(GdsMapAssembly)의 NexplantQMS.GdsMap.GdsDieAnalysisForm 을 붙이고, 왼쪽에 Die 목록을 둔다.
    /// 연관 분석 화면(frmGdsKlarfAnalysis / frmGdsKlarfSimple)이 Open 으로 직접 연다(2026-10-12: ShowForm 은 ENGUI 를 다른 어셈블리 사본으로 읽어 중복 생성되므로 쓰지 않음).
    /// 두 프로젝트가 같이 아는 형식(System.Data / System.Action / object)만 리플렉션으로 넘긴다.
    ///
    /// 창 / Die 목록 (2026-10-22)
    ///  1) 상세보기 창은 하나다. 열려 있으면 분석 화면에서 다른 Die 를 더블클릭해도 새 창을 만들지 않고 이 창 목록에서 그 Die 를 고른다.
    ///  2) 목록 = 연 화면 Wafer 결과의 Die 중 Defect 가 있거나 E-Test Fail Chain 이 있는 Die(연 Die 는 항상 넣음). 순서는 서버 DIE 표 그대로(Chain 위 많은 순).
    ///  3) 목록에서 고르면(선택이 0.3초 멈추면) 창이 가진 요청 사본으로 그 Die 만 다시 계산(AnalyzeDie)해 오른쪽에 보인다.
    ///     본 Die 의 결과 / 보기 상태는 기억해 다시 고르면 서버를 부르지 않는다.
    ///  4) 연 화면이 다시 분석하면(설정을 바꾼 분석 / Wafer 바꿈) 목록을 새 결과로 바꾸고 보던 Die 를 새 요청으로 다시 계산한다(SourceAnalyzed).
    ///
    /// GDS 상세 화면(도면 컨트롤 하나 약 2 GB, 만들기 약 4초)은 static 하나를 둔다(2026-10-16).
    ///  - 창이 활성화되면 가진 창의 보기 상태를 저장하고, 이 창 패널로 옮긴 뒤(핸들 유지) 이 창의 결과 / 보기 상태로 ShowDie 를 부른다.
    ///  - 연관 분석 화면이 열려 있거나(Preload 로 붙잡음) 상세보기 창이 있으면 둔다. 둘 다 없으면 버린다(메모리 반환).
    ///  - [분석] 이 끝나면 연관 분석 화면이 Preload 로 도면 데이터를 미리 읽게 한다(백그라운드).
    /// </summary>
    public partial class frmGdsDieAnalysis : DACrux.Framework.Base.DACruxUXBasic01
    {
        private const string GdsFormTypeName = "NexplantQMS.GdsMap.GdsDieAnalysisForm";

        // 같이 쓰는 GDS 상세 화면 / 그 화면을 가진 창 / 열린 창 / 공유 화면을 붙잡는 화면(연관 분석 화면)
        private static Type gdsFormType;
        private static Form sharedForm;
        private static MethodInfo showDie;
        private static MethodInfo captureView;
        private static frmGdsDieAnalysis owner;
        private static readonly List<frmGdsDieAnalysis> openWindows = new List<frmGdsDieAnalysis>();
        private static readonly HashSet<object> holders = new HashSet<object>();

        // 목록 원본(연 화면 Wafer 결과, null 이면 연 Die 결과로 채움) / 그 Die 를 다시 계산할 요청 사본(null 이면 Wafer 결과를 그대로 씀) / 서버
        private DataSet wafer;
        private GdsKlarfAnalysisRequest request;
        private RO.GdsKlarfAnalysis service;
        // Die 별 상세 결과 / 역추적 Chain / 보기 상태, 지금 보이는 Die, 코드가 목록 선택을 바꾸는 중(선택 이벤트 무시)
        private readonly Dictionary<Point, DieView> views = new Dictionary<Point, DieView>();
        private Point? current;
        private bool listing;
        private static bool showErrorDialogs = true;

        public frmGdsDieAnalysis()
        {
            InitializeComponent();
            openWindows.Add(this);
        }

        /// <summary>이 창을 연 화면(연관 분석 화면). 그 화면이 다시 분석하면 이 창도 새 결과로 바꾼다.</summary>
        public object Source { get; private set; }

        /// <summary>연 화면의 분석 순번([분석] 마다 바뀜). 같으면 같은 분석 결과로 채운 창이다.</summary>
        public int AnalysisVersion { get; private set; }

        public int DieX { get { return current.HasValue ? current.Value.X : 0; } }
        public int DieY { get { return current.HasValue ? current.Value.Y : 0; } }

        /// <summary>지금 보이는 Die 의 상세 결과(없으면 null).</summary>
        public DataSet Analysis
        {
            get { DieView view = CurrentView; return view == null ? null : view.Data; }
        }

        /// <summary>왼쪽 목록의 Die (X, Y) 순서대로(Smoke 확인용).</summary>
        public IList<Point> ListedDies
        {
            get { return dgvDies.Rows.Cast<DataGridViewRow>().Select(r => (Point)r.Tag).ToList(); }
        }

        /// <summary>이 창이 서버에서 Die 를 계산한 횟수(Smoke 확인용).</summary>
        public int AnalyzeCount { get; private set; }

        /// <summary>목록 선택 / 동기화 중 오류를 메시지 창으로 알릴지(Smoke 는 끈다). 끄면 Trace 에만 남긴다.</summary>
        public static bool ShowErrorDialogs
        {
            get { return showErrorDialogs; }
            set { showErrorDialogs = value; }
        }

        /// <summary>지금 공유 GDS 상세 화면을 이 창이 가지고 있는지(Smoke 확인용).</summary>
        public bool HasSharedDetail
        {
            get { return owner == this && sharedForm != null && !sharedForm.IsDisposed && sharedForm.Parent == pnlGdsMap; }
        }

        /// <summary>공유 화면이 없을 때 안내 문구를 보이는지(Smoke 확인용).</summary>
        public bool ShowsPlaceholder { get { return lblShared.Visible; } }

        /// <summary>공유 GDS 상세 화면(없으면 null, Smoke 확인용).</summary>
        public static Form SharedDetailForm
        {
            get { return sharedForm != null && !sharedForm.IsDisposed ? sharedForm : null; }
        }

        /// <summary>열린 상세보기 창.</summary>
        public static IList<frmGdsDieAnalysis> OpenWindows
        {
            get { return openWindows.Where(w => !w.IsDisposed && !w.Disposing).ToList(); }
        }

        /// <summary>열린 상세보기 창(하나만 둔다). 없으면 null.</summary>
        public static frmGdsDieAnalysis Current
        {
            get { return OpenWindows.FirstOrDefault(); }
        }

        private DieView CurrentView
        {
            get { DieView view; return current.HasValue && views.TryGetValue(current.Value, out view) ? view : null; }
        }

        #region [열기 / 동기화]

        /// <summary>
        /// 연관 분석 화면이 Die 를 더블클릭했을 때 부른다(2026-10-22 설계 5.3절). 상세보기 창이 있으면 그 창, 없으면 새로 만든다(창은 하나).
        /// 연 화면 / 분석 순번 / 요청이 바뀌었으면 목록을 이 결과로 다시 만들고 기억한 Die 결과는 버린다. 그 Die 를 고르고 계산해 보인 뒤 창을 앞으로 가져온다.
        /// wafer = 연 화면 Wafer 결과(null 이면 연 Die 결과로 목록을 채움), request = 마지막 분석 요청(null 이면 wafer 를 그대로 상세 결과로 씀),
        /// focusChainSeq = E-Test Fail 역추적 기준 Chain(없으면 null, 있으면 기억한 결과가 있어도 다시 계산).
        /// 계산이 실패하면 예외를 그대로 던진다(새 창이었으면 만들지 않는다).
        /// </summary>
        public static frmGdsDieAnalysis Open(Form source, int version, DataSet wafer, GdsKlarfAnalysisRequest request, RO.GdsKlarfAnalysis service,
            int x, int y, string focusChainSeq)
        {
            frmGdsDieAnalysis window = Current;
            bool created = window == null;
            if (created)
            {
                window = new frmGdsDieAnalysis();
                if (source != null && source.MdiParent != null) window.MdiParent = source.MdiParent;
            }
            try
            {
                if (created || !ReferenceEquals(window.Source, source) || window.AnalysisVersion != version || !ReferenceEquals(window.request, request))
                    window.SetSource(source, version, wafer, request, service);
                window.SelectDie(new Point(x, y), focusChainSeq, focusChainSeq != null);
            }
            catch
            {
                if (created) window.Dispose();
                throw;
            }
            // 새 창은 Shown 에서 공유 화면을 가져와 보인다. 있던 창은 활성화될 때 가져온다(이미 가지고 있으면 SelectDie 가 보였다)
            if (created) window.Show();
            else window.Activate();
            return window;
        }

        /// <summary>
        /// 연관 분석 화면이 분석 결과를 반영할 때마다 부른다(2026-10-22 요청 "분석화면에서 설정을 바꿔서 분석을 하면 상세도 같이 refresh 되어야 한다. 서로 동기화").
        /// 상세보기 창이 그 화면에서 연 창이면 목록을 새 결과로 바꾸고 보던 Die 를 새 요청으로 다시 계산해 보인다(창을 앞으로 가져오지는 않는다).
        /// 다른 화면에서 연 창이거나 창이 없으면 아무것도 하지 않는다.
        /// </summary>
        public static void SourceAnalyzed(Form source, int version, DataSet wafer, GdsKlarfAnalysisRequest request, RO.GdsKlarfAnalysis service)
        {
            frmGdsDieAnalysis window = Current;
            if (window == null || !ReferenceEquals(window.Source, source) || window.AnalysisVersion == version) return;
            window.Resync(version, wafer, request, service);
        }

        /// <summary>
        /// 새 결과로 바꾼다. 보던 Die 가 새 결과에 있으면 그 Die, 없으면 목록 첫 Die 를 같은 역추적 Chain 없이 다시 계산한다(같은 Die 면 역추적 Chain 유지).
        /// 도면 키(Device / 보기 기준 / Chain)가 같으면 확대 위치 등 보기 상태를 이어 간다. 계산이 실패하면 창은 이전 결과로 두고 제목에 "(이전 분석)" 을 붙인다.
        /// </summary>
        private void Resync(int version, DataSet newWafer, GdsKlarfAnalysisRequest newRequest, RO.GdsKlarfAnalysis newService)
        {
            SaveCurrentView();
            DieView before = CurrentView;
            Point? pick = current.HasValue && FindDieRow(newWafer, current.Value) != null ? current : ListTargets(newWafer).Select(r => (Point?)PointOf(r)).FirstOrDefault();
            if (!pick.HasValue) pick = current;
            if (!pick.HasValue)
            {
                SetSource(Source, version, newWafer, newRequest, newService);
                return;
            }
            string focus = before != null && pick == current ? before.FocusChainSeq : null;
            DataSet data;
            try { data = LoadDie(newWafer, newRequest, newService, pick.Value, focus); }
            catch (Exception ex)
            {
                Report("상세보기 새 결과 계산 실패", ex);
                if (current.HasValue) Text = TitleOf(current.Value) + " (이전 분석)";
                return;
            }
            object state = before != null && pick == current && SameMap(before.Data, data) ? before.View : null;
            SetSource(Source, version, newWafer, newRequest, newService);
            ShowView(pick.Value, new DieView { Data = data, FocusChainSeq = focus, View = state });
        }

        /// <summary>목록 원본 / 요청 / 서버를 바꾸고 기억한 Die 결과를 버린 뒤 목록을 다시 채운다.</summary>
        private void SetSource(object source, int version, DataSet newWafer, GdsKlarfAnalysisRequest newRequest, RO.GdsKlarfAnalysis newService)
        {
            Source = source;
            AnalysisVersion = version;
            wafer = newWafer;
            request = newRequest;
            service = newService;
            views.Clear();
            FillList();
        }

        /// <summary>ShowForm 으로 열렸을 때 Activated 에서 불린다(DACruxUXBasic01). 매개변수: object[] { DataSet, int X, int Y (, string 역추적 Chain) }.</summary>
        public override void LoadParameter(object parameter)
        {
            var p = parameter as object[];
            if (p == null || p.Length < 3 || !(p[0] is DataSet)) return;
            ShowDie((DataSet)p[0], Convert.ToInt32(p[1]), Convert.ToInt32(p[2]), p.Length > 3 ? p[3] as string : null);
        }

        public void ShowDie(DataSet analysis, int xIndex, int yIndex)
        {
            ShowDie(analysis, xIndex, yIndex, null);
        }

        /// <summary>계산해 둔 Die 상세 결과를 그대로 보인다(서버를 부르지 않음). 목록에 그 Die 가 없으면 넣는다.</summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex, string focusChainSeq)
        {
            SaveCurrentView();
            ShowView(new Point(xIndex, yIndex), new DieView { Data = analysis, FocusChainSeq = focusChainSeq });
        }

        #endregion

        #region [Die 고르기]

        /// <summary>
        /// 그 Die 를 오른쪽에 보인다. 기억한 결과가 있고 다시 계산할 이유(역추적 Chain)가 없으면 서버를 부르지 않고 그 결과 / 보기 상태로 돌아간다.
        /// 계산이 실패하면 지금 보이는 Die 를 그대로 두고 예외를 던진다.
        /// </summary>
        private void SelectDie(Point p, string focusChainSeq, bool recompute)
        {
            DieView view;
            if (recompute || !views.TryGetValue(p, out view))
                view = new DieView { Data = LoadDie(wafer, request, service, p, focusChainSeq), FocusChainSeq = focusChainSeq };
            SaveCurrentView();
            ShowView(p, view);
        }

        /// <summary>Die 결과를 기억하고 지금 Die 로 정한 뒤(탭 제목 = "상세보기 (X, Y)") 목록 선택을 맞추고, 공유 화면을 가지고 있으면 바로 넘긴다.</summary>
        private void ShowView(Point p, DieView view)
        {
            views[p] = view;
            current = p;
            Text = TitleOf(p);
            EnsureListed(p, view.Data);
            MarkListRow(p);
            if (owner == this && sharedForm != null && !sharedForm.IsDisposed) InvokeShowDie();
        }

        /// <summary>그 Die 만 서버에서 계산한다(AnalyzeDie: 그 Die 의 Defect 전체 / Member 단위 걸림 / 역추적 Chain 거리 / E-Test 값). 요청이 없으면 Wafer 결과를 그대로 쓴다.</summary>
        private DataSet LoadDie(DataSet source, GdsKlarfAnalysisRequest dieRequest, RO.GdsKlarfAnalysis svc, Point p, string focusChainSeq)
        {
            if (dieRequest == null) return source;
            AnalyzeCount++;
            // 아직 보이지 않는 새 창이면 연 화면이 모래시계를 보이고 있다
            using (Visible ? DACrux.SEMDMS.Control.BusyCursor.Show(this) : null)
                return (svc ?? new RO.GdsKlarfAnalysis()).AnalyzeDie(dieRequest, p.X, p.Y, focusChainSeq);
        }

        /// <summary>목록 선택이 바뀌면 0.3초 뒤에 반영한다(위 / 아래 키로 빨리 넘기면 마지막 Die 만 계산).</summary>
        private void dgvDies_SelectionChanged(object sender, EventArgs e)
        {
            if (listing) return;
            tmrSelect.Stop();
            tmrSelect.Start();
        }

        /// <summary>목록에서 고른 Die 를 보인다(역추적 Chain 없이). 실패하면 알리고 목록 선택을 보이는 Die 로 되돌린다.</summary>
        private void tmrSelect_Tick(object sender, EventArgs e)
        {
            tmrSelect.Stop();
            if (dgvDies.SelectedRows.Count == 0) return;
            var p = (Point)dgvDies.SelectedRows[0].Tag;
            if (current.HasValue && p == current.Value) return;
            try { SelectDie(p, null, false); }
            catch (Exception ex)
            {
                Report("Die 상세 분석 실패", ex);
                if (current.HasValue) MarkListRow(current.Value);
            }
        }

        #endregion

        #region [Die 목록]

        /// <summary>목록을 지금 원본으로 다시 채운다. E-Test Fail 열은 E-Test 를 연결한 결과에만 보인다.</summary>
        private void FillList()
        {
            listing = true;
            try
            {
                dgvDies.Rows.Clear();
                DataTable die = wafer != null && wafer.Tables.Contains("DIE") ? wafer.Tables["DIE"] : null;
                colEtFail.Visible = die != null && die.Columns.Contains("ET_FAIL_CHAIN_COUNT") && die.Rows.Cast<DataRow>().Any(r => !r.IsNull("ET_FAIL_CHAIN_COUNT"));
                foreach (DataRow row in ListTargets(wafer)) AddRow(PointOf(row), row);
            }
            finally { listing = false; }
            lblListCount.Text = dgvDies.Rows.Count.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>목록 대상 Die: Defect 1개 이상 또는 E-Test Fail Chain 1개 이상(설계 L5), 서버 DIE 표 순서.</summary>
        private static IEnumerable<DataRow> ListTargets(DataSet source)
        {
            if (source == null || !source.Tables.Contains("DIE")) return Enumerable.Empty<DataRow>();
            return source.Tables["DIE"].Rows.Cast<DataRow>().Where(r => Int(r, "DEFECT_COUNT") > 0 || Int(r, "ET_FAIL_CHAIN_COUNT") > 0);
        }

        /// <summary>그 Die 가 목록에 없으면(Defect 0 Die 를 연 경우 / Wafer 결과 없이 연 경우) 끝에 넣는다. 값은 Wafer 결과, 없으면 Die 결과의 DIE 행.</summary>
        private void EnsureListed(Point p, DataSet dieData)
        {
            if (FindListRow(p) != null) return;
            DataRow row = FindDieRow(wafer, p) ?? FindDieRow(dieData, p);
            listing = true;
            try { AddRow(p, row); }
            finally { listing = false; }
            if (row != null && row.Table.Columns.Contains("ET_FAIL_CHAIN_COUNT") && !row.IsNull("ET_FAIL_CHAIN_COUNT")) colEtFail.Visible = true;
            lblListCount.Text = dgvDies.Rows.Count.ToString(CultureInfo.InvariantCulture);
        }

        private void AddRow(Point p, DataRow row)
        {
            int index = dgvDies.Rows.Add(dgvDies.Rows.Count + 1, p.X, p.Y,
                row == null ? (object)"" : Int(row, "DEFECT_COUNT"),
                row == null ? (object)"" : Int(row, "IN_CHAIN_COUNT"),
                row == null || !row.Table.Columns.Contains("ET_FAIL_CHAIN_COUNT") || row.IsNull("ET_FAIL_CHAIN_COUNT") ? (object)"" : Int(row, "ET_FAIL_CHAIN_COUNT"));
            dgvDies.Rows[index].Tag = p;
        }

        /// <summary>목록에서 그 Die 행을 고른 상태로 둔다(이벤트 없이, 보이게 스크롤).</summary>
        private void MarkListRow(Point p)
        {
            DataGridViewRow row = FindListRow(p);
            listing = true;
            try
            {
                dgvDies.ClearSelection();
                if (row == null) return;
                dgvDies.CurrentCell = row.Cells[colNo.Index];
                row.Selected = true;
            }
            finally { listing = false; }
        }

        private DataGridViewRow FindListRow(Point p)
        {
            return dgvDies.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Tag is Point && (Point)r.Tag == p);
        }

        private static DataRow FindDieRow(DataSet source, Point p)
        {
            if (source == null || !source.Tables.Contains("DIE")) return null;
            return source.Tables["DIE"].Rows.Cast<DataRow>().FirstOrDefault(r => PointOf(r) == p);
        }

        private static Point PointOf(DataRow row)
        {
            return new Point(Int(row, "XINDEX"), Int(row, "YINDEX"));
        }

        private static int Int(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && !row.IsNull(column) ? Convert.ToInt32(row[column], CultureInfo.InvariantCulture) : 0;
        }

        /// <summary>탭 제목 = "상세보기 (X, Y)" (2026-10-12 요청 "Tab 상단에는 상세보기(좌표정보)").</summary>
        private static string TitleOf(Point p)
        {
            return String.Format("상세보기 ({0}, {1})", p.X, p.Y);
        }

        #endregion

        #region [공유 GDS 상세 화면]

        /// <summary>
        /// 화면이 표시된 뒤 공유 GDS 상세 화면을 준비하고(처음이면 만든다) 이 창이 활성 창이면 가져온다.
        /// 전용 폴더(100_ClientBin\GdsMap)의 GDS DLL 을 써서 DACrux DLL 과 GDS DLL 이 섞이지 않게 한다(GdsMapAssembly).
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // 표가 처음 보일 때 첫 행을 고를 수 있어 지금 Die 로 되돌린다(그 선택으로 다른 Die 를 계산하지 않게)
            tmrSelect.Stop();
            if (current.HasValue) MarkListRow(current.Value);
            try
            {
                using (DACrux.SEMDMS.Control.BusyCursor.Show(this)) // GDS DLL 읽기 + 화면 만들기 동안 모래시계 커서 (2026-10-15)
                    EnsureSharedForm();
            }
            catch (Exception ex)
            {
                Exception cause = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                MessageBox.Show(this, cause.Message, "Die 상세 분석 화면 열기", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            if (MdiParent == null || MdiParent.ActiveMdiChild == this) TakeShared();
            else ShowPlaceholder(true);
        }

        /// <summary>MDI 탭 전환 / 새 창 표시로 이 창이 활성화되면 공유 화면을 가져온다.</summary>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            TakeShared();
        }

        /// <summary>창을 닫으면 공유 화면을 떼어 둔다(이 창과 함께 해제되지 않게). 붙잡은 화면도 없으면 해제한다.</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Detach();
            base.OnFormClosed(e);
        }

        /// <summary>
        /// 연관 분석 화면 [분석] 이 끝나면 부른다(2026-10-16 설계 5.3 / 5.4절). holder(그 화면)가 열려 있는 동안 공유 화면을 둔다.
        /// 열린 상세보기 창이 없고 공유 화면이 다른 도면(Device / 보기 기준 / Chain)이면 먼저 버린다(메모리 반환).
        /// 그 다음 GDS DLL 의 PreloadMap 으로 도면 데이터를 백그라운드로 미리 읽는다. 실패해도 분석 화면은 그대로 둔다(상세보기를 열 때 다시 읽음).
        /// </summary>
        public static void Preload(object holder, DataSet analysis)
        {
            if (holder != null) holders.Add(holder);
            try
            {
                Type formType = GdsFormType();
                MethodInfo keyOf = formType.GetMethod("MapKeyOf", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(DataSet) }, null);
                Form shared = SharedDetailForm;
                if (keyOf != null && shared != null && OpenWindows.Count == 0)
                {
                    PropertyInfo loaded = formType.GetProperty("LoadedMapKey");
                    string loadedKey = loaded == null ? null : loaded.GetValue(shared, null) as string;
                    if (loadedKey != null && loadedKey != keyOf.Invoke(null, new object[] { analysis }) as string) DisposeShared();
                }
                MethodInfo preload = formType.GetMethod("PreloadMap", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(DataSet) }, null);
                if (preload != null) preload.Invoke(null, new object[] { analysis });
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("frmGdsDieAnalysis.Preload: {0}", ex);
            }
        }

        /// <summary>연관 분석 화면을 닫을 때 부른다. 열린 상세보기 창도 없으면 공유 화면 / 미리 읽은 도면을 버린다.</summary>
        public static void Release(object holder)
        {
            holders.Remove(holder);
            if (holders.Count == 0 && OpenWindows.Count == 0) DisposeShared();
        }

        /// <summary>GDS DLL 의 상세 화면 형식(GdsMapAssembly: 이미 읽힌 어셈블리 또는 GdsMap 폴더 DLL).</summary>
        private static Type GdsFormType()
        {
            if (gdsFormType == null) gdsFormType = GdsMapAssembly.Load().GetType(GdsFormTypeName, true);
            return gdsFormType;
        }

        /// <summary>두 Die 결과의 도면 키가 같은지(GDS DLL MapKeyOf). 알 수 없으면 false.</summary>
        private static bool SameMap(DataSet a, DataSet b)
        {
            if (a == null || b == null || gdsFormType == null) return false;
            MethodInfo keyOf = gdsFormType.GetMethod("MapKeyOf", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(DataSet) }, null);
            if (keyOf == null) return false;
            string ka = keyOf.Invoke(null, new object[] { a }) as string;
            return ka != null && ka == keyOf.Invoke(null, new object[] { b }) as string;
        }

        /// <summary>공유 GDS 상세 화면이 없으면 만든다(패널에는 TakeShared 가 붙인다).</summary>
        private static void EnsureSharedForm()
        {
            if (sharedForm != null && !sharedForm.IsDisposed) return;
            Type formType = GdsFormType();
            var form = Activator.CreateInstance(formType, new object[] { true }) as Form;
            if (form == null)
                throw new InvalidOperationException("Die 상세 분석 화면을 Form 으로 만들 수 없습니다.");
            // 보기 상태를 받는 5인자 버전(2026-10-16) -> 역추적 기준 Chain 4인자 -> 이전 GDS 버전의 3인자 순서로 찾는다
            showDie = formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int), typeof(string), typeof(object) })
                ?? formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int), typeof(string) })
                ?? formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) });
            if (showDie == null)
            {
                form.Dispose();
                throw new MissingMethodException(GdsFormTypeName, "ShowDie (GDS DLL 버전 확인 필요)");
            }
            captureView = formType.GetMethod("CaptureViewState", Type.EmptyTypes);
            PropertyInfo imageRequest = formType.GetProperty("DefectImageRequested");
            if (imageRequest != null)
                imageRequest.SetValue(form, new Action<long, long>(ShowOwnerDefectImages), null);
            // 상세 화면 [다시 분석] (2026-10-22): 지금 공유 화면을 가진 창의 연 분석 화면이 다시 분석한다
            PropertyInfo reanalyzeRequest = formType.GetProperty("ReanalyzeRequested");
            if (reanalyzeRequest != null)
                reanalyzeRequest.SetValue(form, new Action(ReanalyzeOwnerSource), null);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            sharedForm = form;
        }

        /// <summary>
        /// 공유 GDS 상세 화면을 이 창으로 옮긴다: 가진 창의 보기 상태 저장 -> 이 창 패널로 옮김(핸들 / 도면 유지, 2026-10-16 실험 확인)
        /// -> 이 창의 지금 Die 결과 / 보기 상태로 ShowDie. 이미 가지고 있거나 공유 화면 / 결과가 없으면 아무것도 하지 않는다.
        /// </summary>
        private void TakeShared()
        {
            if (IsDisposed || Disposing || CurrentView == null || sharedForm == null || sharedForm.IsDisposed) return;
            if (owner == this && sharedForm.Parent == pnlGdsMap) return;
            if (owner != null && owner != this) owner.GiveUpShared();
            owner = this;
            ShowPlaceholder(false);
            pnlGdsMap.Controls.Add(sharedForm);
            sharedForm.BringToFront();
            if (!sharedForm.Visible) sharedForm.Show();
            InvokeShowDie();
        }

        /// <summary>지금 Die 의 보기 상태를 저장하고(도면을 읽는 중이면 전에 저장한 값 유지) 공유 화면을 뗀 뒤 안내 문구를 보인다.</summary>
        private void GiveUpShared()
        {
            if (owner != this) return;
            SaveCurrentView();
            if (sharedForm != null && !sharedForm.IsDisposed && sharedForm.Parent == pnlGdsMap) pnlGdsMap.Controls.Remove(sharedForm);
            owner = null;
            if (!IsDisposed && !Disposing) ShowPlaceholder(true);
        }

        /// <summary>공유 화면을 가지고 있으면 지금 Die 의 보기 상태(확대 위치 / 고른 행 / 체크)를 그 Die 에 저장한다.</summary>
        private void SaveCurrentView()
        {
            DieView view = CurrentView;
            if (view == null || owner != this || sharedForm == null || sharedForm.IsDisposed || captureView == null) return;
            object state = captureView.Invoke(sharedForm, null);
            if (state != null) view.View = state;
        }

        private void InvokeShowDie()
        {
            DieView view = CurrentView;
            if (view == null) return;
            int n = showDie.GetParameters().Length;
            object[] args = n == 5 ? new object[] { view.Data, DieX, DieY, view.FocusChainSeq, view.View }
                : n == 4 ? new object[] { view.Data, DieX, DieY, view.FocusChainSeq }
                : new object[] { view.Data, DieX, DieY };
            showDie.Invoke(sharedForm, args);
        }

        private void ShowPlaceholder(bool show)
        {
            lblShared.Visible = show;
            if (show) lblShared.BringToFront();
        }

        /// <summary>안내 문구를 누르면 이 창을 활성화해 공유 화면을 가져온다.</summary>
        private void lblShared_Click(object sender, EventArgs e)
        {
            Activate();
            TakeShared();
        }

        /// <summary>
        /// 이 창을 목록에서 빼고 공유 화면을 뗀다(닫기 / Dispose 에서 부름, 두 번 불러도 된다).
        /// 열린 창도 붙잡은 화면도 없으면 공유 화면 / 미리 읽은 도면을 버린다. 다른 상세보기 창이 활성 창이면 그 창이 바로 가져간다.
        /// </summary>
        private void Detach()
        {
            tmrSelect.Stop();
            openWindows.Remove(this);
            if (owner == this)
            {
                if (sharedForm != null && !sharedForm.IsDisposed && sharedForm.Parent == pnlGdsMap) pnlGdsMap.Controls.Remove(sharedForm);
                owner = null;
            }
            if (openWindows.Count == 0 && holders.Count == 0)
            {
                DisposeShared();
                return;
            }
            frmGdsDieAnalysis next = MdiParent == null ? null : MdiParent.ActiveMdiChild as frmGdsDieAnalysis;
            if (owner == null && next != null && next != this) next.TakeShared();
        }

        /// <summary>공유 화면을 해제하고(도면 메모리 반환) GDS DLL 의 미리 읽기 캐시도 비운다.</summary>
        private static void DisposeShared()
        {
            Form form = sharedForm;
            sharedForm = null;
            owner = null;
            if (form != null && !form.IsDisposed) form.Dispose();
            if (gdsFormType == null) return;
            MethodInfo clear = gdsFormType.GetMethod("ClearMapCache", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);
            if (clear != null) clear.Invoke(null, null);
        }

        #endregion

        #region [이미지 / 오류]

        /// <summary>공유 화면의 이미지 보기 요청을 지금 공유 화면을 가진 창으로 넘긴다.</summary>
        private static void ShowOwnerDefectImages(long stepSeq, long defectId)
        {
            if (owner != null && !owner.IsDisposed) owner.ShowDefectImages(stepSeq, defectId);
        }

        /// <summary>공유 화면 [다시 분석] 요청을 지금 공유 화면을 가진 창으로 넘긴다.</summary>
        private static void ReanalyzeOwnerSource()
        {
            if (owner != null && !owner.IsDisposed) owner.ReanalyzeSource();
        }

        /// <summary>
        /// 상세 화면 [다시 분석] (2026-10-22 요청 "상세보기 우측 상단에 다시 분석 버튼 하나 만들어서 조회"):
        /// 이 창을 연 분석 화면의 [다시 분석] / [분석] 과 같다(그 화면의 지금 설정으로 Wafer 다시 분석).
        /// 분석 화면이 결과를 반영하면 SourceAnalyzed 로 이 창 목록 / 보던 Die 가 새 결과로 바뀐다. 분석 화면이 닫혔으면 알린다.
        /// </summary>
        private void ReanalyzeSource()
        {
            var source = Source as IDetailSource;
            var form = Source as Form;
            if (source == null || form == null || form.IsDisposed)
            {
                Report("다시 분석", new InvalidOperationException("이 상세보기를 연 분석 화면이 닫혀 있어 다시 분석할 수 없습니다. 분석 화면에서 Die 를 다시 여세요."));
                return;
            }
            source.AnalyzeAgain();
        }

        /// <summary>상세보기를 여는 분석 화면(frmGdsKlarfAnalysis / frmGdsKlarfSimple). 상세 화면 [다시 분석] 이 그 화면의 분석을 다시 부른다 (2026-10-22).</summary>
        public interface IDetailSource
        {
            /// <summary>그 화면의 [다시 분석] / [분석] 과 같은 동작(지금 설정으로 Wafer 다시 분석, 결과 반영 뒤 SourceAnalyzed).</summary>
            void AnalyzeAgain();
        }

        /// <summary>
        /// GDS 상세 화면의 이미지 보기 요청. Defect Map Viewer 와 같은 경로(TQD_IMAGES -> 이미지 카드 DPUCDefectImageCard / FTP 다운로드)를 쓴다.
        /// 카드를 더블클릭하면 기존 ImagePopUp 이 열린다. 여기서는 Defect ID 만 알므로 카드 2줄째(Die / Class / 크기)는 비운다. (2026-10-20 카드 교체)
        /// </summary>
        private void ShowDefectImages(long stepSeq, long defectId)
        {
            DataTable paths;
            using (DACrux.SEMDMS.Control.BusyCursor.Show(this)) // 서버 이미지 경로 조회 동안 모래시계 커서 (2026-10-15)
                paths = new RO.DefectMapAnalysis().GetDefectImagePath01(new[] { stepSeq });
            var popup = new Form
            {
                Text = "Defect #" + defectId + " 이미지",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(720, 460),
                ShowInTaskbar = false
            };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            popup.Controls.Add(flow);
            foreach (DataRow row in paths.Rows)
            {
                if (Convert.ToInt64(row["DEFECTID"]) != defectId) continue;
                string path = String.Format("{0}/{1}", row["IMAGE_PATH"].ToString().Trim(), row["IMAGE_FILENAME"].ToString().Trim());
                flow.Controls.Add(new DACrux.SEMDMS.Control.DPUCDefectImageCard(defectId, path));
            }
            if (flow.Controls.Count == 0)
            {
                popup.Dispose();
                MessageBox.Show(this, "Defect #" + defectId + " 이미지가 없습니다(TQD_IMAGES).", "Defect 이미지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            popup.Show(this);
        }

        /// <summary>오류 알림: Trace 에 남기고, 오류 창을 켜 두었으면 메시지 창도 띄운다.</summary>
        private void Report(string title, Exception ex)
        {
            Exception cause = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
            Trace.TraceWarning("frmGdsDieAnalysis {0}: {1}", title, cause);
            if (ShowErrorDialogs) MessageBox.Show(this, cause.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        /// <summary>Die 하나의 상세 결과 / 역추적 Chain / 보기 상태(CaptureViewState 값, 내용은 보지 않음).</summary>
        private sealed class DieView
        {
            public DataSet Data;
            public string FocusChainSeq;
            public object View;
        }
    }
}
