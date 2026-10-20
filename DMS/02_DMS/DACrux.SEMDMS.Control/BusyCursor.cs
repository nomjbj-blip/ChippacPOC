using System;
using System.Drawing;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// 화면 작업 중 마우스 커서 (2026-10-15, POC 화면 공통).
    /// 목적: 버튼 / 목록 선택 같은 이벤트로 서버 조회나 그리기를 하는 동안 "작업 중"임을 커서로 보인다.
    /// 처리:
    ///  - Show: 입력을 막는 작업용 모래시계. Form.UseWaitCursor 만 켜면 Windows 가 다음 WM_SETCURSOR(마우스 이동)를 보낼 때까지
    ///    화살표가 남고, 화면 스레드에서 바로 실행하는 작업은 아예 바뀌지 않는다. 그래서 마우스가 그 화면 위에 있으면
    ///    Cursor.Current 도 바로 바꾼다(화면 스레드가 막혀도 유지된다).
    ///  - ShowBackground: 입력을 막지 않는 작업용 화살표 + 작은 모래시계(AppStarting). 작업 중이지만 다른 칸은 쓸 수 있다는 뜻.
    ///  - 반환 값을 Dispose 하면 원래 값으로 돌린다(using 으로 쓴다). UseWaitCursor 는 자식 컨트롤 커서(지도 Cross 등)를
    ///    보관했다가 되돌리므로 다른 커서를 망가뜨리지 않는다.
    ///  - 마우스가 다른 화면 / 메뉴 위에 있으면 Cursor.Current 는 건드리지 않는다(그쪽 커서가 바뀌어 보이지 않게).
    /// </summary>
    public static class BusyCursor
    {
        /// <summary>입력을 막는 작업 동안 모래시계를 보인다. owner 가 속한 화면 전체(자식 컨트롤 포함)에 적용된다.</summary>
        public static IDisposable Show(System.Windows.Forms.Control owner)
        {
            Form form = owner as Form ?? (owner == null ? null : owner.FindForm());
            if (form == null || form.IsDisposed) return new Scope(null, false, null);

            bool saved = form.UseWaitCursor;
            form.UseWaitCursor = true;
            if (IsPointerOver(form)) Cursor.Current = Cursors.WaitCursor;
            return new Scope(form, saved, null);
        }

        /// <summary>입력을 막지 않는 작업 동안 화살표 + 작은 모래시계(AppStarting)를 보인다.</summary>
        public static IDisposable ShowBackground(System.Windows.Forms.Control owner)
        {
            Form form = owner as Form ?? (owner == null ? null : owner.FindForm());
            if (form == null || form.IsDisposed) return new Scope(null, false, null);

            Cursor saved = form.Cursor;
            form.Cursor = Cursors.AppStarting;
            if (IsPointerOver(form) && !form.UseWaitCursor) Cursor.Current = Cursors.AppStarting;
            return new Scope(form, form.UseWaitCursor, saved);
        }

        /// <summary>마우스가 그 화면(창 영역) 위에 있는지. MDI 자식 / 호스트 안에 붙은 화면도 화면 좌표로 비교한다.</summary>
        private static bool IsPointerOver(Form form)
        {
            if (!form.IsHandleCreated || !form.Visible) return false;
            Rectangle area = form.RectangleToScreen(form.ClientRectangle);
            return area.Contains(Cursor.Position);
        }

        /// <summary>켠 커서를 되돌린다. 두 번 Dispose 해도 한 번만 되돌린다.</summary>
        private sealed class Scope : IDisposable
        {
            private Form form;
            private readonly bool savedWaitCursor;
            private readonly Cursor savedCursor;

            internal Scope(Form form, bool savedWaitCursor, Cursor savedCursor)
            {
                this.form = form;
                this.savedWaitCursor = savedWaitCursor;
                this.savedCursor = savedCursor;
            }

            public void Dispose()
            {
                Form target = form;
                form = null;
                if (target == null || target.IsDisposed) return;

                if (savedCursor != null)
                    target.Cursor = savedCursor;
                else
                    target.UseWaitCursor = savedWaitCursor;

                // 다음 마우스 이동 전까지 남아 있는 모래시계를 지금 바로 화면의 현재 커서로 돌린다.
                // (바깥에 아직 작업이 남아 있으면 그 커서: 모래시계 / AppStarting, 없으면 화면 기본 커서)
                if (IsPointerOver(target)) Cursor.Current = target.UseWaitCursor ? Cursors.WaitCursor : target.Cursor;
            }
        }
    }
}
