using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// 화면 아래 진행 표시줄 (2026-10-14, POC 화면 공통).
    /// 목적: 서버 조회 / 저장 / 삭제나 파일 처리처럼 오래 걸리는 작업 동안 화면이 "동작 중"인지 "멈췄는지" 알 수 있게 한다.
    /// 쓰는 법:
    ///  - Run: 작업을 별도 스레드에서 돌리고, 끝날 때까지 화면 메시지를 계속 처리하며 기다린다. 부르는 쪽에서는 작업이 끝난 뒤 돌아오는
    ///    보통 메서드라 기존 처리 순서(확인 창 -> 저장 -> 다시 조회)와 자동 검사를 그대로 쓴다. 작업 안에서는 화면 컨트롤을 만지지 않는다.
    ///  - RunSteps: Run 과 같고, 작업이 report(처리 수, 전체 수)를 부르면 막대가 그 비율로 찬다(작업 스레드에서 불러도 됨).
    ///  - RunHere: 콤보 / 입력 칸 / 표 선택 변경처럼 이벤트 처리 도중이라 메시지를 처리하면 안 되는 곳. 화면 스레드에서 바로 실행하고 표시만 한다.
    ///  - Begin: 여러 Run 과 표 채우기를 한 작업으로 묶는다(안쪽 Run 이름은 작업 이름 뒤에 붙는다).
    ///    Begin(이름, false) 는 입력을 막지 않는다(예: 업로드 화면의 백그라운드 분석 중에도 결과 행을 볼 수 있게).
    ///    Step(처리 수, 전체 수)로 막대를 채우고, Note 로 지금 처리 대상(파일 이름 등)을 보이고,
    ///    화면 스레드에서 오래 도는 반복문에서는 Pulse() 로 표시만 갱신한다.
    /// 작업 중: 막대(전체 수를 모르면 계속 움직임) + 작업 이름(0.2초마다 처리 수 갱신), 이 화면의 마우스 / 키 입력은 막고(다른 화면은 사용 가능)
    ///          모래시계 커서(BusyCursor, 마우스를 움직이지 않아도 바로 보임), 화면 닫기는 끝날 때까지 미룬다.
    ///          입력을 막지 않는 작업만 있으면 화살표 + 작은 모래시계(AppStarting).
    ///          끝나면 "작업 완료" 또는 "실패" 만 남긴다. 처리 시간 / 끝난 시각은 보이지 않는다 (2026-10-15).
    ///          막대는 작업 중에만 보이고, 대기 / 끝난 뒤에는 숨긴다 (2026-10-19).
    /// 겹친 작업: 진행 중인 작업을 목록으로 관리한다. 표시 이름은 가장 먼저 시작한 작업, 입력 차단은 막는 작업이 하나라도 있으면 유지한다.
    /// </summary>
    [ToolboxItem(true)]
    public partial class ProgressStatusBar : UserControl
    {
        private const int WM_KEYFIRST = 0x0100;
        private const int WM_KEYLAST = 0x0109;
        private const int WM_MOUSEFIRST = 0x0200;
        private const int WM_MOUSELAST = 0x020E;

        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly List<BusyTask> tasks = new List<BusyTask>();
        private string note = string.Empty;
        private int stepDone;
        private int stepTotal;
        private volatile int reportDone;
        private volatile int reportTotal;
        private bool failed;
        private long lastPulse;
        private InputBlocker blocker;
        private IDisposable waitCursor;
        private IDisposable backgroundCursor;
        private Form closingForm;

        public ProgressStatusBar()
        {
            InitializeComponent();
            RunInBackground = true;
            LastResult = string.Empty;
            Disposed += (sender, e) =>
            {
                tasks.Clear();
                UpdateBlock();
                if (closingForm != null) closingForm.FormClosing -= HostFormClosing;
            };
        }

        /// <summary>false 면 Run 도 화면 스레드에서 바로 실행한다(표시는 같다). 기본 true.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunInBackground { get; set; }

        /// <summary>작업 중인지.</summary>
        [Browsable(false)]
        public bool IsBusy
        {
            get { return tasks.Count > 0; }
        }

        /// <summary>마지막으로 끝난 작업 문구 (예: "삭제 (2개) 완료"). 자동 검사용.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LastResult { get; private set; }

        /// <summary>지금 표시 중인 문구.</summary>
        [Browsable(false)]
        public string DisplayText
        {
            get { return lblBusy.Text; }
        }

        #region [실행]

        /// <summary>작업을 별도 스레드에서 실행하고 끝날 때까지 기다린다(그동안 화면 메시지 처리). 작업의 예외는 그대로 다시 던진다.</summary>
        public T Run<T>(string name, Func<T> work)
        {
            if (work == null) throw new ArgumentNullException("work");
            return RunSteps(name, report => work());
        }

        /// <summary>반환 값이 없는 작업용 Run.</summary>
        public void Run(string name, Action work)
        {
            if (work == null) throw new ArgumentNullException("work");
            RunSteps<object>(name, report =>
            {
                work();
                return null;
            });
        }

        /// <summary>Run 과 같다. work 가 report(처리 수, 전체 수)를 부르면 막대가 그 비율로 찬다.</summary>
        public T RunSteps<T>(string name, Func<Action<int, int>, T> work)
        {
            if (work == null) throw new ArgumentNullException("work");
            BusyTask task = StartTask(name, true);
            bool ok = false;
            try
            {
                Action<int, int> report = (done, total) =>
                {
                    reportTotal = total;
                    reportDone = done;
                };
                T result = RunInBackground && IsHandleCreated ? WaitFor(() => work(report)) : work(report);
                ok = true;
                return result;
            }
            finally
            {
                EndTask(task, ok);
            }
        }

        /// <summary>화면 스레드에서 바로 실행하고 표시만 한다(이벤트 처리 도중이라 메시지를 처리하면 안 되는 곳).</summary>
        public T RunHere<T>(string name, Func<T> work)
        {
            if (work == null) throw new ArgumentNullException("work");
            BusyTask task = StartTask(name, true);
            bool ok = false;
            try
            {
                T result = work();
                ok = true;
                return result;
            }
            finally
            {
                EndTask(task, ok);
            }
        }

        /// <summary>여러 단계를 한 작업으로 묶는다(입력 막음). 반환 값을 Dispose 하면 끝난다(using 으로 쓴다).</summary>
        public IDisposable Begin(string name)
        {
            return Begin(name, true);
        }

        /// <summary>여러 단계를 한 작업으로 묶는다. blockInput = false 면 표시만 하고 입력은 막지 않는다.</summary>
        public IDisposable Begin(string name, bool blockInput)
        {
            return new Scope(this, StartTask(name, blockInput));
        }

        /// <summary>묶은 작업의 처리 수 / 전체 수. 화면 스레드에서 부른다. 전체 수가 0 이면 계속 움직이는 막대로 돌아간다.</summary>
        public void Step(int done, int total)
        {
            if (tasks.Count == 0) return;
            stepDone = Math.Max(0, done);
            stepTotal = Math.Max(0, total);
            Render();
            PaintNow();
        }

        /// <summary>지금 처리 대상(파일 이름 등)을 작업 이름 뒤에 보인다. 화면 스레드에서 부른다.</summary>
        public void Note(string text)
        {
            if (tasks.Count == 0) return;
            note = text ?? string.Empty;
            Render();
            PaintNow();
        }

        /// <summary>화면 스레드에서 오래 도는 반복문(표 채우기 등) 안에서 부른다. 0.15초마다 한 번만 표시를 갱신한다.</summary>
        public void Pulse()
        {
            if (tasks.Count == 0) return;
            long now = clock.ElapsedMilliseconds;
            if (now - lastPulse < 150) return;
            lastPulse = now;
            Render();
            PaintNow();
        }

        #endregion

        #region [내부]

        private T WaitFor<T>(Func<T> work)
        {
            T result = default(T);
            Exception error = null;
            using (ManualResetEvent finished = new ManualResetEvent(false))
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        result = work();
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                    finally
                    {
                        finished.Set();
                    }
                });
                thread.IsBackground = true;
                thread.Name = "ProgressStatusBar";
                thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                thread.Start();
                while (!finished.WaitOne(30))
                    Application.DoEvents();
            }
            if (error != null) throw error;
            return result;
        }

        private BusyTask StartTask(string name, bool block)
        {
            BusyTask task = new BusyTask(string.IsNullOrEmpty(name) ? "작업" : name, block);
            reportDone = 0;
            reportTotal = 0;
            if (tasks.Count == 0)
            {
                HookForm();
                failed = false;
                stepDone = 0;
                stepTotal = 0;
                note = string.Empty;
                lastPulse = 0;
                lblBusy.ForeColor = SystemColors.ControlText;
                progress.Value = 0;
                progress.Visible = true;
                timer.Start();
            }
            tasks.Add(task);
            UpdateBlock();
            Render();
            PaintNow();
            return task;
        }

        private void EndTask(BusyTask task, bool ok)
        {
            if (!tasks.Contains(task)) return;
            if (!ok) failed = true;
            reportDone = 0;
            reportTotal = 0;
            BusyTask first = tasks[0];
            tasks.Remove(task);
            UpdateBlock();
            if (tasks.Count > 0)
            {
                Render();
                return;
            }
            timer.Stop();
            // 작업이 끝나면 막대는 숨긴다. 100% 로 채운 막대를 남기지 않는다 (2026-10-19)
            HideProgress();
            // 끝났는지 / 실패했는지만 남긴다. 처리 시간 / 끝난 시각은 보이지 않는다 (2026-10-15).
            LastResult = first.Name + (!failed ? " 완료" : ok ? " 끝 (오류 있음)" : " 실패");
            lblBusy.Text = LastResult;
            lblBusy.ForeColor = failed ? Color.Firebrick : Color.DimGray;
            PaintNow();
        }

        private void Render()
        {
            if (tasks.Count == 0) return;
            BusyTask first = tasks[0];
            int done = stepTotal > 0 ? stepDone : reportDone;
            int total = stepTotal > 0 ? stepTotal : reportTotal;
            string text = first.Name + " 중...";
            if (tasks.Count > 1) text += " / " + tasks[tasks.Count - 1].Name;
            if (note.Length > 0) text += " / " + note;
            if (total > 0)
                text += " (" + done.ToString("N0", CultureInfo.InvariantCulture) + " / " + total.ToString("N0", CultureInfo.InvariantCulture) + ")";
            lblBusy.Text = text;
            if (total > 0)
            {
                if (progress.Style != ProgressBarStyle.Continuous) progress.Style = ProgressBarStyle.Continuous;
                progress.Maximum = total;
                progress.Value = Math.Min(Math.Max(done, 0), total);
            }
            else if (progress.Style != ProgressBarStyle.Marquee)
            {
                progress.Style = ProgressBarStyle.Marquee;
            }
        }

        /// <summary>막대를 숨기고 다음 작업을 위해 빈 상태(움직이지 않는 막대, 값 0)로 되돌린다 (2026-10-19)</summary>
        private void HideProgress()
        {
            progress.Visible = false;
            progress.Style = ProgressBarStyle.Continuous;
            progress.Value = 0;
        }

        private void PaintNow()
        {
            if (!IsHandleCreated) return;
            lblBusy.Update();
            progress.Update();
        }

        /// <summary>
        /// 입력을 막는 작업이 있으면 이 화면(자식 컨트롤 포함)으로 가는 마우스 / 키 입력을 막고 모래시계 커서를 바로 보인다.
        /// 입력을 막지 않는 작업만 남으면 화살표 + 작은 모래시계(AppStarting)로 바꾸고, 작업이 없으면 원래 커서로 돌린다 (2026-10-15).
        /// </summary>
        private void UpdateBlock()
        {
            bool block = tasks.Any(t => t.Block);
            if (block)
            {
                SetBackgroundCursor(false);
                if (blocker != null) return;
                Form form = FindForm();
                if (form == null) return;
                waitCursor = BusyCursor.Show(form);
                blocker = new InputBlocker(form);
                Application.AddMessageFilter(blocker);
                return;
            }
            if (blocker != null)
            {
                Application.RemoveMessageFilter(blocker);
                blocker = null;
            }
            if (waitCursor != null)
            {
                waitCursor.Dispose();
                waitCursor = null;
            }
            SetBackgroundCursor(tasks.Count > 0);
        }

        /// <summary>입력을 막지 않는 작업 중 커서(AppStarting)를 켜거나 끈다.</summary>
        private void SetBackgroundCursor(bool on)
        {
            if (on && backgroundCursor == null)
            {
                Form form = FindForm();
                if (form != null) backgroundCursor = BusyCursor.ShowBackground(form);
            }
            else if (!on && backgroundCursor != null)
            {
                backgroundCursor.Dispose();
                backgroundCursor = null;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            HookForm();
        }

        /// <summary>화면 닫기를 작업이 끝날 때까지 미루려고 화면의 FormClosing 에 붙는다.</summary>
        private void HookForm()
        {
            Form form = FindForm();
            if (form == closingForm) return;
            if (closingForm != null) closingForm.FormClosing -= HostFormClosing;
            closingForm = form;
            if (form != null) form.FormClosing += HostFormClosing;
        }

        /// <summary>입력을 막는 작업 중에는 화면을 닫지 않는다(Windows 종료 / 프로그램 종료 요청은 그대로).</summary>
        private void HostFormClosing(object sender, FormClosingEventArgs e)
        {
            if (blocker == null || e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing
                || e.CloseReason == CloseReason.ApplicationExitCall)
                return;
            e.Cancel = true;
            lblBusy.Text = tasks[0].Name + " 중 / 끝난 뒤 닫을 수 있습니다";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private sealed class BusyTask
        {
            internal readonly string Name;
            internal readonly bool Block;

            internal BusyTask(string name, bool block)
            {
                Name = name;
                Block = block;
            }
        }

        private sealed class Scope : IDisposable
        {
            private ProgressStatusBar owner;
            private readonly BusyTask task;

            internal Scope(ProgressStatusBar owner, BusyTask task)
            {
                this.owner = owner;
                this.task = task;
            }

            public void Dispose()
            {
                if (owner == null) return;
                owner.EndTask(task, true);
                owner = null;
            }
        }

        /// <summary>작업 중인 화면으로 가는 키 / 마우스 메시지를 버린다(다른 화면과 창 제목 표시줄은 그대로).</summary>
        private sealed class InputBlocker : IMessageFilter
        {
            private readonly Form form;

            internal InputBlocker(Form form)
            {
                this.form = form;
            }

            public bool PreFilterMessage(ref Message m)
            {
                bool input = (m.Msg >= WM_KEYFIRST && m.Msg <= WM_KEYLAST) || (m.Msg >= WM_MOUSEFIRST && m.Msg <= WM_MOUSELAST);
                if (!input || form.IsDisposed) return false;
                System.Windows.Forms.Control target = System.Windows.Forms.Control.FromChildHandle(m.HWnd);
                return target != null && (target == form || form.Contains(target));
            }
        }

        #endregion
    }
}
