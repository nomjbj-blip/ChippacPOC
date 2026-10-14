using System;
using System.Drawing;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS Map 조회 화면 레이아웃 동작(2026-10-12, 컨셉 A "도구 막대 한 줄 + 오른쪽 탭 패널").
    ///  - Map 정보 Label 은 제거했고 같은 문구를 Device 콤보 툴팁으로 보여 준다.
    ///  - Chain 작업 줄은 도면을 조회한 뒤에만, 유사 묶음 목록은 찾았거나 되돌릴 제외가 있을 때만 보인다.
    ///  - 도면 우클릭 메뉴(전체 보기)는 제대로 동작하지 않아 제거했다(2026-10-13).
    ///  - 상태 문구는 아래 상태 줄 한 곳에만 보인다. Chain 안내 줄 / Label 검색 결과 글자는 없앴다(2026-10-13).
    /// 버튼의 사용 가능 상태는 기존 UpdateChainSelectionButtons / SetChainButtonsEnabled 가 정하고, 끝에서 UpdateViewLayout 을 부른다.
    /// </summary>
    public partial class GdsMapViewForm
    {
        /// <summary>Map 정보 문구(파일 / 건수 / 저장 기준 / Sub Device / 보기 기준)를 Device 콤보 툴팁에 넣는다.</summary>
        private void SetMapInfo(string text)
        {
            toolTipView.SetToolTip(cboDevice, text ?? string.Empty);
        }

        /// <summary>
        /// Chain 작업 안내를 상태 줄에 보인다. 한 문장으로 지금 상태와 다음에 할 일만 쓴다(계산 / 확인용 세부 값은 쓰지 않는다).
        /// 저장 진행처럼 긴 작업 중에도 바로 보이도록 상태 줄을 다시 그린다.
        /// </summary>
        private void SetChainStatus(string text)
        {
            SetStatus(text ?? string.Empty);
            statusStripView.Refresh();
        }

        /// <summary>
        /// Chain 을 고르거나 다시 연결했을 때 그 Chain 의 상태로 다음 할 일을 안내한다.
        /// 순서: Layer 미조회 -> 저장 미완료 -> Input 없음 -> 후보 경로 없음 -> 편집 가능.
        /// </summary>
        private string ChainGuide(ChainDefinition chain)
        {
            if (chain == null) return "목록에서 Chain 을 선택하거나 [새 Chain]을 누르세요.";
            if (chain.DbIncomplete != null) return chain.Name + ": " + chain.DbIncomplete;
            if (chain.ChainSeq != null && chain.SavedFingerprint == null)
                return chain.Name + ": 저장이 끝나지 않은 Chain 입니다. [Chain 저장(DB)]으로 다시 저장하세요.";
            if (chain.Inputs.Count == 0) return chain.Name + ": [Input 지정]을 누르고 지도에서 Input 도형을 클릭하세요.";
            if (!chain.TraceShown) return chain.Name + ": [후보 경로 찾기]를 누르세요.";
            return chain.Name + " 선택";
        }

        /// <summary>
        /// 상태에 따라 보이는 영역을 맞춘다.
        /// Chain 작업 줄 = 도면이 있을 때. 유사 묶음 목록 = 찾는 중 / 찾은 묶음 / 되돌릴 제외가 있을 때(새로 보이면 Chain 탭으로 이동).
        /// Visible 은 값이 같으면 다시 배치하지 않으므로 자주 불러도 된다.
        /// </summary>
        private void UpdateViewLayout()
        {
            bool mapShown = map.Structure != null;
            tblChainBar.Visible = mapShown;
            // Chain Code(A, B, C) 대신 이름을 보인다. 긴 이름은 줄여서 작업 줄이 넘치지 않게 한다 (2026-10-12).
            lblChainBarTitle.Text = HasSelectedChain
                ? ShortChainName(_activeChain.Name) + " 편집"
                : "Chain 선택 필요";

            bool similar = _chainSimilarSearching || _chainSimilarGroups.Count > 0
                || (HasSelectedChain && _activeChain.LastSimilarUndo != null);
            if (similar && !chainSimilarListPanel.Visible) tabRight.SelectedTab = tabChain;
            chainSimilarListPanel.Visible = similar;
        }

        /// <summary>작업 줄 제목용 이름(28자 넘으면 줄임). 전체 이름은 Chain 목록에 보인다.</summary>
        private static string ShortChainName(string name)
        {
            name = name ?? string.Empty;
            return name.Length <= 28 ? name : name.Substring(0, 27) + "…";
        }

        /// <summary>[Map ▾] Map 관리 메뉴(Sub Device 설정 / 원점 표시 / Map 삭제)를 버튼 아래에 연다.</summary>
        private void btnMapMenu_Click(object sender, EventArgs e)
        {
            mnuMap.Show(btnMapMenu, new Point(0, btnMapMenu.Height));
        }
    }
}
