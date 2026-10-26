using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;

namespace DACruxV5
{
    /// <summary>
    /// 데이터 생성 / 수정 작업 가능 기간을 서버에 물어 Main 메뉴에 반영한다. (2026-10-23)
    /// - 기간이 지나면 POC 메뉴 묶음 Setup / Data Upload 와 그 아래 화면을 비활성으로 바꾸고 제목에 안내를 붙인다.
    /// - 판정은 서버(DB 일시)가 한다. 클라이언트 PC 시간은 쓰지 않는다.
    ///   실제 저장 차단은 서버 저장 함수가 하므로, 여기서 서버 호출이 실패하면 메뉴를 그대로 둔다.
    /// - 서버가 꺼져 있어도 시작이 늦어지지 않게 Main 이 보인 뒤 작업 스레드에서 호출한다.
    /// 설계: 문서/설계/06_환경_운영/2026-10-14_POC_License_설계.md 8절
    /// </summary>
    static class MenuWorkRule
    {
        /// <summary>기간이 지나면 막을 메뉴 묶음(MenuFile.mnu MENU_KEY). 묶음 아래 화면도 함께 막는다.</summary>
        static readonly string[] WorkMenuKeys = { "MNU_POC_SETUP", "MNU_POC_UPLOAD" };

        /// <summary>마지막 날까지 이 일수 이하로 남으면 제목에 남은 일수를 보인다.</summary>
        const int NoticeDays = 14;

        /// <summary>Main 화면이 보이면 작업 기간을 한 번 확인하도록 연결한다.</summary>
        public static void Attach(Form mainForm)
        {
            mainForm.Shown += (s, e) => ThreadPool.QueueUserWorkItem(_ => LoadState(mainForm));
        }

        /// <summary>작업 스레드: 서버에서 기간 상태를 받아 UI 스레드로 넘긴다. 실패하면 아무것도 바꾸지 않는다.</summary>
        static void LoadState(Form mainForm)
        {
            DataTable state;
            try
            {
                state = new DACrux.SEMDMS.RO.GdsMapView().GetWorkState();
            }
            catch
            {
                // 서버 꺼짐 / 이전 버전 서버: 메뉴를 바꾸지 않는다. 저장은 서버 저장 함수가 막는다.
                return;
            }
            if (state == null || state.Rows.Count == 0)
                return;

            DataRow row = state.Rows[0];
            try
            {
                if (!mainForm.IsDisposed && mainForm.IsHandleCreated)
                    mainForm.BeginInvoke(new Action(() => Apply(mainForm, row)));
            }
            catch (InvalidOperationException)
            {
                // 그 사이 Main 화면이 닫힌 경우
            }
        }

        /// <summary>UI 스레드: 기간이 지났으면 메뉴를 막고, 기한이 가까우면 남은 일수를 제목에 보인다.</summary>
        static void Apply(Form mainForm, DataRow row)
        {
            if (mainForm.IsDisposed)
                return;

            bool open = row["WORK_OPEN"].ToString() == "Y";
            int daysLeft = Convert.ToInt32(row["DAYS_LEFT"]);
            string lastDay = row["LAST_DAY"].ToString();

            if (!open)
            {
                DisableWorkMenus(mainForm);
                mainForm.Text += "  [데이터 등록 / 수정 기간 종료 - 조회 전용]";
            }
            else if (daysLeft <= NoticeDays)
            {
                mainForm.Text += string.Format("  [데이터 등록 / 수정 기한 {0}, {1}일 남음]", lastDay, daysLeft);
            }
        }

        /// <summary>
        /// 메뉴 묶음과 그 아래 화면 Tool 을 비활성으로 바꾼다. SharedProps 는 같은 Tool 의 모든 위치(메뉴 / 바로가기 / 리본)에 함께 적용된다.
        /// 메뉴 파일에서 ENABLE 이 이미 False 인 메뉴는 켜지 않는다(막는 쪽으로만 바꾼다).
        /// </summary>
        static void DisableWorkMenus(Form mainForm)
        {
            UltraToolbarsManager utb = UltraToolbarsManager.FromForm(mainForm);
            if (utb == null)
                return;

            foreach (string key in WorkMenuKeys)
            {
                int index = utb.Tools.IndexOf(key);
                if (index < 0)
                    continue;

                ToolBase group = utb.Tools[index];
                group.SharedProps.Enabled = false;

                PopupMenuTool popup = group as PopupMenuTool;
                if (popup == null)
                    continue;
                foreach (ToolBase child in popup.Tools)
                    child.SharedProps.Enabled = false;
            }
        }
    }
}
