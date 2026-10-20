using System;
using System.Drawing;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 작업 중 표시 (2026-10-15). 예전 경과 시간 칸(ElapsedStatusTicker)을 대신한다.
    /// 목적: 파일 읽기 / DB 조회 / 저장처럼 오래 걸리는 작업 동안 "작업 중"임을 상태 칸과 마우스 커서로 보인다. 처리 시간은 보이지 않는다.
    /// 처리:
    ///  - Start / Stop: 진행 막대와 같이 쓰는 긴 작업. 상태 칸에 "작업 중..." 을 보이고 끝나면 숨긴다.
    ///  - Busy(): using 으로 감싸는 작업(콤보 선택 조회 / 계산 등). 상태 칸은 그대로 두고 커서만 바꾼다.
    ///  - 둘 중 하나라도 진행 중이면 화면 전체를 모래시계 커서로 한다(UseWaitCursor). UseWaitCursor 만 켜면 마우스를 움직일 때까지
    ///    화살표가 남고 화면 스레드가 막히면 바뀌지 않으므로, 마우스가 화면 위에 있으면 Cursor.Current 도 바로 바꾼다.
    ///  - 지도 조작 커서(Cross / SizeAll)는 UseWaitCursor 가 보관했다가 되돌리므로 그대로 남는다.
    /// DACrux 쪽 공통 컨트롤(DACrux.SEMDMS.Control.BusyCursor)과 같은 방식이다. GdsMap 은 DLL 버전 충돌을 피하려고 그 프로젝트를 참조하지 않는다.
    /// </summary>
    internal sealed class BusyStatus
    {
        private readonly Form form;
        private readonly ToolStripStatusLabel label;
        private bool started;
        private int scopes;
        private bool applied;
        private bool savedWaitCursor;

        /// <param name="form">모래시계를 보일 화면</param>
        /// <param name="label">"작업 중..." 을 보일 상태 칸. null 이면 커서만 바꾼다.</param>
        internal BusyStatus(Form form, ToolStripStatusLabel label)
        {
            this.form = form;
            this.label = label;
        }

        /// <summary>작업 중인지(자동 검사용).</summary>
        internal bool IsBusy
        {
            get { return started || scopes > 0; }
        }

        /// <summary>긴 작업 시작: 상태 칸 "작업 중..." + 모래시계. 이미 시작했으면 그대로 둔다.</summary>
        internal void Start()
        {
            started = true;
            if (label != null && !label.IsDisposed)
            {
                label.Text = "작업 중...";
                label.Visible = true;
            }
            Apply();
        }

        /// <summary>긴 작업 끝: 상태 칸을 숨기고 커서를 되돌린다. 시작하지 않았으면 아무것도 하지 않는다.</summary>
        internal void Stop()
        {
            if (!started) return;
            started = false;
            if (label != null && !label.IsDisposed)
            {
                label.Visible = false;
                label.Text = string.Empty;
            }
            Apply();
        }

        /// <summary>using 으로 감싸는 동안 모래시계를 보인다(겹쳐 써도 된다).</summary>
        internal IDisposable Busy()
        {
            scopes++;
            Apply();
            return new Scope(this);
        }

        /// <summary>작업이 하나라도 있으면 모래시계를 켜고, 모두 끝나면 원래 커서로 돌린다.</summary>
        private void Apply()
        {
            if (form == null || form.IsDisposed) return;
            bool on = IsBusy;
            if (on && !applied)
            {
                applied = true;
                savedWaitCursor = form.UseWaitCursor;
                form.UseWaitCursor = true;
                if (IsPointerOver()) Cursor.Current = Cursors.WaitCursor;
            }
            else if (!on && applied)
            {
                applied = false;
                form.UseWaitCursor = savedWaitCursor;
                if (IsPointerOver()) Cursor.Current = form.UseWaitCursor ? Cursors.WaitCursor : form.Cursor;
            }
        }

        /// <summary>마우스가 이 화면(DACrux 화면 안에 붙은 경우 포함) 위에 있는지.</summary>
        private bool IsPointerOver()
        {
            if (!form.IsHandleCreated || !form.Visible) return false;
            Rectangle area = form.RectangleToScreen(form.ClientRectangle);
            return area.Contains(Cursor.Position);
        }

        private sealed class Scope : IDisposable
        {
            private BusyStatus owner;

            internal Scope(BusyStatus owner)
            {
                this.owner = owner;
            }

            public void Dispose()
            {
                if (owner == null) return;
                owner.scopes--;
                owner.Apply();
                owner = null;
            }
        }
    }
}
