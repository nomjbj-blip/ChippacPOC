using System;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// Defect Map Viewer(DPUCDefecMapViewer) 전용 Defect Map 이다 (2026-10-13).
    /// 만든 이유: 우클릭 메뉴를 작업에 쓰는 항목만 남기고 숨기기 위해서다. 그리기 / 확대 / 선택 동작은 기존 DACrux.Map.DefectMap 그대로다.
    /// 남기는 항목: Free Zoom / Fit Size / Move View Center / Defect Select / Die Select
    ///             + 화면이 생성 뒤 AddContextMenuItem 으로 붙인 항목(Center Cross Show / Hide 등).
    /// 방법: 기존 메뉴(WaferMap 의 protected ctxmWaferMap)는 수정하지 않고 메뉴가 뜰 때(Popup) 표시 여부만 바꾼다.
    ///   기존 Popup 처리기(WaferMap / DefectMap)는 기반 클래스 생성자에서 먼저 연결되므로 이 처리기가 마지막에 실행된다.
    ///   Defect Select / Die Select 메뉴 필드는 DefectMap 의 private 이라 메뉴 글자로 찾는다.
    ///   구분선은 보이는 항목 묶음 사이에만 1개씩 보인다.
    /// </summary>
    public class DefectViewerMap : DACrux.Map.DefectMap
    {
        /// <summary>기존 Map 이 만든 메뉴 항목 수. 이 위치부터는 화면이 붙인 항목이라 그대로 둔다.</summary>
        private readonly int m_iBaseMenuCount;

        public DefectViewerMap()
        {
            m_iBaseMenuCount = ctxmWaferMap.MenuItems.Count;
            ctxmWaferMap.Popup += ctxmWaferMap_ShowWorkItemsOnly;
        }

        /// <summary>메뉴가 뜨기 직전: 남길 항목만 보이고, 구분선은 보이는 항목 사이에만 보인다.</summary>
        private void ctxmWaferMap_ShowWorkItemsOnly(object sender, EventArgs e)
        {
            for (int i = 0; i < m_iBaseMenuCount; i++)
            {
                MenuItem item = ctxmWaferMap.MenuItems[i];
                item.Visible = item == mnuitemMAPMODE_FREEZOOM || item == mnuitemMAPMODE_FITSIZE || item == mnuitemMOUSEDRAGMODE_MOVE
                    || item.Text == "Defect Select" || item.Text == "Die Select";
            }

            // 구분선 정리: 앞에 보이는 항목이 있고 뒤에 보이는 항목이 이어질 때만 바로 앞 구분선 1개를 보인다.
            bool visibleBefore = false;
            MenuItem pendingSeparator = null;
            foreach (MenuItem item in ctxmWaferMap.MenuItems)
            {
                if (item.Text == "-")
                {
                    item.Visible = false;
                    if (visibleBefore) pendingSeparator = item;
                    continue;
                }
                if (!item.Visible) continue;
                if (pendingSeparator != null) pendingSeparator.Visible = true;
                pendingSeparator = null;
                visibleBefore = true;
            }
        }
    }
}
