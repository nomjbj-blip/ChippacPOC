using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// Die 상세 분석 화면(화면 B) 호스트. 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 7.3절,
    /// 여러 창 / 도면 재사용: 문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md 5절.
    /// 기존 frmGdsMapView 와 같은 방식으로 .NET 4.8 GDS EXE 를 실행 시 읽어 NexplantQMS.GdsMap.GdsDieAnalysisForm 을 자식 Form 으로 붙인다.
    /// 연관 분석 화면(frmGdsKlarfAnalysis)이 직접 만들어 연다(2026-10-12: ShowForm 은 ENGUI 를 다른 어셈블리 사본으로 읽어 중복 생성되므로 쓰지 않음),
    /// 매개변수 object[] { 분석 결과 DataSet, Die X, Die Y } 를 LoadParameter / ShowDie 로 넘긴다.
    /// 두 프로젝트가 같이 아는 형식(System.Data / System.Action / object)만 넘긴다.
    ///
    /// 여러 창 (2026-10-16)
    ///  1) 상세보기 창은 Die 마다 하나다. 연관 분석 화면이 FindWindow 로 찾고 없으면 새로 만든다(기존 창은 그대로 남는다).
    ///  2) GDS 상세 화면(도면 컨트롤 하나 약 2 GB, 만들기 약 4초)은 모든 창이 하나를 같이 쓴다(static).
    ///     창이 활성화되면(탭 전환 / 새 창) 가진 창의 보기 상태를 저장하고, 이 창 패널로 옮긴 뒤(핸들 유지) 이 창의 결과 / 보기 상태로 ShowDie 를 부른다.
    ///     도면 키가 같으면 도면은 다시 읽지 않는다. 공유 화면이 없는 창은 안내 문구를 보인다(탭 그룹을 나눠 같이 보일 때).
    ///  3) 연관 분석 화면이 열려 있거나(Preload 로 붙잡음) 상세보기 창이 하나라도 있으면 공유 화면을 둔다. 둘 다 없으면 버린다(메모리 반환).
    ///  4) [분석] 이 끝나면 연관 분석 화면이 Preload 로 도면 데이터를 미리 읽게 한다(백그라운드).
    /// </summary>
    public partial class frmGdsDieAnalysis : DACrux.Framework.Base.DACruxUXBasic01
    {
        private const string GdsAssemblyName = "NexplantQMS.GdsMap";
        private const string GdsFormTypeName = "NexplantQMS.GdsMap.GdsDieAnalysisForm";

        // 모든 상세보기 창이 같이 쓰는 GDS 상세 화면 / 그 화면을 가진 창 / 열린 창 / 공유 화면을 붙잡는 화면(연관 분석 화면)
        private static Type gdsFormType;
        private static Form sharedForm;
        private static MethodInfo showDie;
        private static MethodInfo captureView;
        private static frmGdsDieAnalysis owner;
        private static readonly List<frmGdsDieAnalysis> openWindows = new List<frmGdsDieAnalysis>();
        private static readonly HashSet<object> holders = new HashSet<object>();

        // 이 창의 값: Die 상세 결과 / 역추적 Chain / 다른 창으로 넘어갈 때 저장한 보기 상태
        private DataSet analysis;
        private string focusChainSeq;
        private object viewState;

        public frmGdsDieAnalysis()
        {
            InitializeComponent();
            openWindows.Add(this);
        }

        /// <summary>이 창을 연 화면(연관 분석 화면). 같은 화면의 같은 Die 창을 찾을 때 쓴다.</summary>
        public object Source { get; set; }

        /// <summary>연 화면의 분석 순번([분석] 마다 바뀜). 같으면 같은 분석 결과로 연 창이다.</summary>
        public int AnalysisVersion { get; set; }

        public int DieX { get; private set; }
        public int DieY { get; private set; }

        /// <summary>이 창의 Die 상세 결과(없으면 null).</summary>
        public DataSet Analysis { get { return analysis; } }

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
            get { return openWindows.Where(w => !w.IsDisposed).ToList(); }
        }

        /// <summary>source 화면이 연 (x, y) 창. 없으면 null.</summary>
        public static frmGdsDieAnalysis FindWindow(object source, int x, int y)
        {
            return openWindows.FirstOrDefault(w => !w.IsDisposed && !w.Disposing && ReferenceEquals(w.Source, source) && w.DieX == x && w.DieY == y);
        }

        /// <summary>
        /// 화면이 표시된 뒤 공유 GDS 상세 화면을 준비하고(처음이면 만든다) 이 창이 활성 창이면 가져온다.
        /// 전용 폴더(100_ClientBin\GdsMap)를 써서 DACrux DLL 과 GDS DLL 의 버전 충돌을 피한다(frmGdsMapView 와 같음).
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                using (DACrux.SEMDMS.Control.BusyCursor.Show(this)) // GDS EXE 읽기 + 화면 만들기 동안 모래시계 커서 (2026-10-15)
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

        /// <summary>창을 닫으면 공유 화면을 떼어 둔다(이 창과 함께 해제되지 않게). 다음에 활성화되는 창이 가져간다.</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Detach();
            base.OnFormClosed(e);
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

        /// <summary>
        /// 연관 분석 화면이 직접 부른다. 이 창의 결과 / Die / 역추적 Chain 을 바꾸고 저장한 보기 상태는 버린다(새 결과로 처음부터 표시).
        /// 탭 제목 = "상세보기 (X, Y)" (2026-10-12 요청 "Tab 상단에는 상세보기(좌표정보)", 설계 14.4절). 상세 화면은 이 Chip 1개만 분석한다.
        /// 공유 화면을 이 창이 가지고 있으면 바로 넘기고, 아니면 이 창이 활성화될 때 넘긴다.
        /// focusChainSeq = E-Test Fail 역추적 기준 Chain(없으면 null, 2026-10-16 통합 분석 설계 15.3절).
        /// </summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex, string focusChainSeq)
        {
            Text = TitleOf(xIndex, yIndex, false);
            this.analysis = analysis;
            DieX = xIndex;
            DieY = yIndex;
            this.focusChainSeq = focusChainSeq;
            viewState = null;
            if (owner == this && sharedForm != null && !sharedForm.IsDisposed) InvokeShowDie();
        }

        /// <summary>
        /// 연관 분석 화면 [분석] 이 끝나면 부른다(2026-10-16 설계 5.3 / 5.4절). holder(그 화면)가 열려 있는 동안 공유 화면을 둔다.
        /// 열린 상세보기 창이 없고 공유 화면이 다른 도면(Device / 보기 기준 / Chain)이면 먼저 버린다(메모리 반환).
        /// 그 다음 GDS EXE 의 PreloadMap 으로 도면 데이터를 백그라운드로 미리 읽는다. 실패해도 분석 화면은 그대로 둔다(상세보기를 열 때 다시 읽음).
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
                System.Diagnostics.Trace.TraceWarning("frmGdsDieAnalysis.Preload: {0}", ex);
            }
        }

        /// <summary>
        /// source 화면이 다시 분석했을 때(분석 순번 analysisVersion) 이전 결과로 남은 그 화면의 창은 탭 제목에 "(이전 분석)" 을 붙인다(2026-10-16).
        /// 창마다 분석 조건이 섞여도 구분되게 한다. 그 Die 를 다시 열면 새 결과로 바뀌고 제목도 돌아온다.
        /// </summary>
        public static void MarkOutdated(object source, int analysisVersion)
        {
            foreach (frmGdsDieAnalysis window in OpenWindows)
                if (ReferenceEquals(window.Source, source) && window.AnalysisVersion != analysisVersion && window.analysis != null)
                    window.Text = TitleOf(window.DieX, window.DieY, true);
        }

        /// <summary>탭 제목 = "상세보기 (X, Y)", 이전 분석 결과면 뒤에 " (이전 분석)".</summary>
        private static string TitleOf(int x, int y, bool outdated)
        {
            return String.Format("상세보기 ({0}, {1}){2}", x, y, outdated ? " (이전 분석)" : "");
        }

        /// <summary>연관 분석 화면을 닫을 때 부른다. 열린 상세보기 창도 없으면 공유 화면 / 미리 읽은 도면을 버린다.</summary>
        public static void Release(object holder)
        {
            holders.Remove(holder);
            if (holders.Count == 0 && OpenWindows.Count == 0) DisposeShared();
        }

        /// <summary>GDS EXE 의 상세 화면 형식. 이미 읽힌 어셈블리가 있으면 그것을 쓰고, 없으면 GdsMap 폴더에서 읽는다.</summary>
        private static Type GdsFormType()
        {
            if (gdsFormType != null) return gdsFormType;
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == GdsAssemblyName);
            if (assembly == null)
            {
                string assemblyPath = Path.Combine(Application.StartupPath, "GdsMap", "NexplantQMS.GdsMap.exe");
                if (!File.Exists(assemblyPath))
                    throw new FileNotFoundException("GDS Map 실행 파일을 찾을 수 없습니다.", assemblyPath);
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            gdsFormType = assembly.GetType(GdsFormTypeName, true);
            return gdsFormType;
        }

        /// <summary>공유 GDS 상세 화면이 없으면 만든다(패널에는 TakeShared 가 붙인다).</summary>
        private static void EnsureSharedForm()
        {
            if (sharedForm != null && !sharedForm.IsDisposed) return;
            Type formType = GdsFormType();
            var form = Activator.CreateInstance(formType, new object[] { true }) as Form;
            if (form == null)
                throw new InvalidOperationException("Die 상세 분석 화면을 Form 으로 만들 수 없습니다.");
            // 보기 상태를 받는 5인자 버전(2026-10-16) -> 역추적 기준 Chain 4인자 -> 이전 GDS EXE 의 3인자 순서로 찾는다
            showDie = formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int), typeof(string), typeof(object) })
                ?? formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int), typeof(string) })
                ?? formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) });
            if (showDie == null)
            {
                form.Dispose();
                throw new MissingMethodException(GdsFormTypeName, "ShowDie (GDS EXE 버전 확인 필요)");
            }
            captureView = formType.GetMethod("CaptureViewState", Type.EmptyTypes);
            PropertyInfo imageRequest = formType.GetProperty("DefectImageRequested");
            if (imageRequest != null)
                imageRequest.SetValue(form, new Action<long, long>(ShowOwnerDefectImages), null);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            sharedForm = form;
        }

        /// <summary>
        /// 공유 GDS 상세 화면을 이 창으로 옮긴다: 가진 창의 보기 상태 저장 -> 이 창 패널로 옮김(핸들 / 도면 유지, 2026-10-16 실험 확인)
        /// -> 이 창의 결과 / 보기 상태로 ShowDie. 이미 가지고 있거나 공유 화면 / 결과가 없으면 아무것도 하지 않는다.
        /// </summary>
        private void TakeShared()
        {
            if (IsDisposed || Disposing || analysis == null || sharedForm == null || sharedForm.IsDisposed) return;
            if (owner == this && sharedForm.Parent == pnlGdsMap) return;
            if (owner != null && owner != this) owner.GiveUpShared();
            owner = this;
            ShowPlaceholder(false);
            pnlGdsMap.Controls.Add(sharedForm);
            sharedForm.BringToFront();
            if (!sharedForm.Visible) sharedForm.Show();
            InvokeShowDie();
        }

        /// <summary>이 창의 보기 상태를 저장하고(도면을 읽는 중이면 전에 저장한 값 유지) 공유 화면을 뗀 뒤 안내 문구를 보인다.</summary>
        private void GiveUpShared()
        {
            if (owner != this) return;
            if (sharedForm != null && !sharedForm.IsDisposed)
            {
                object state = captureView == null ? null : captureView.Invoke(sharedForm, null);
                if (state != null) viewState = state;
                if (sharedForm.Parent == pnlGdsMap) pnlGdsMap.Controls.Remove(sharedForm);
            }
            owner = null;
            if (!IsDisposed && !Disposing) ShowPlaceholder(true);
        }

        private void InvokeShowDie()
        {
            int n = showDie.GetParameters().Length;
            object[] args = n == 5 ? new object[] { analysis, DieX, DieY, focusChainSeq, viewState }
                : n == 4 ? new object[] { analysis, DieX, DieY, focusChainSeq }
                : new object[] { analysis, DieX, DieY };
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

        /// <summary>공유 화면을 해제하고(도면 메모리 반환) GDS EXE 의 미리 읽기 캐시도 비운다.</summary>
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

        /// <summary>공유 화면의 이미지 보기 요청을 지금 공유 화면을 가진 창으로 넘긴다.</summary>
        private static void ShowOwnerDefectImages(long stepSeq, long defectId)
        {
            if (owner != null && !owner.IsDisposed) owner.ShowDefectImages(stepSeq, defectId);
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
    }
}
