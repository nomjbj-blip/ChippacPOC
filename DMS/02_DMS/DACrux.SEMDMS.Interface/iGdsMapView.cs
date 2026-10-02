using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// DB에 저장된 GDS Map을 조회하는 읽기 전용 원격 호출 계약이다.
    /// 설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 4절 / 15절.
    /// 2026-10-02 사용자 결정(CLAUDE-017)으로 리비전 관리가 없어져 Map은 Factory + Device(제품)당 1개다. 조회 키도 Factory + Device다.
    /// 저장(iGdsMapImport)과 분리해 조회 화면은 DB를 바꾸지 않는다. 저장이 끝난(READY) Map만 조회한다.
    /// </summary>
    public interface iGdsMapView
    {
        #region [TQP_GDS_MAP]

        /// <summary>READY Map이 있는 Factory / Device 목록. 열: FACTORY / DEVICE_ID</summary>
        DataTable GetMapDeviceList();

        /// <summary>
        /// Device의 Map 헤더 1행(없으면 0행). 저장 서비스와 같은 SELECT_MAP_INFO 결과다.
        /// 열: FACTORY / DEVICE_ID / TOP_STRUCTURE / GDS_FILE_NAME / GDS_SHA256 / MAP_STATUS / CREATE_USER / CREATE_TIME /
        ///     IMPORT_JOB_SEQ / JOB_STATUS / SOURCE_COUNT / PLACED_COUNT / POINT_COUNT
        /// </summary>
        DataTable GetMapInfo(string strFactory, string strDeviceID);

        #endregion

        #region [TQP_GDS_LAYER]

        /// <summary>READY Map의 Layer 목록. 열: LAYER_ID / COLOR_ARGB / DISPLAY_NAME / PLACED_COUNT / POINT_COUNT</summary>
        DataTable GetMapLayerList(string strFactory, string strDeviceID);

        #endregion

        #region [TQP_GDS_PLACED_EL]

        /// <summary>
        /// READY Map의 한 Layer 배치 도형을 PLACED_ELEMENT_ID 순서로 최대 iMaxRows건 조회한다.
        /// strAfterPlacedID: 이전 페이지의 마지막 PLACED_ELEMENT_ID (첫 페이지는 빈 값). 반환 행 수가 iMaxRows보다 작으면 마지막 페이지다.
        /// 열: PLACED_ELEMENT_ID / SOURCE_ELEMENT_ID / LAYER_ID / DATA_TYPE / ELEMENT_TYPE / CLOSED_FLAG / PATH_WIDTH / PATH_TYPE /
        ///     MIN_X / MIN_Y / MAX_X / MAX_Y / POINT_COUNT / POINTS_FORMAT_VERSION / POINTS_BIN(byte[], X/Y double LE) /
        ///     TEXT_VALUE / TEXT_TYPE / TEXT_FONT / TEXT_HORIZONTAL / TEXT_VERTICAL
        /// </summary>
        DataTable GetPlacedElementPage(string strFactory, string strDeviceID, string strLayerID, string strAfterPlacedID, int iMaxRows);

        #endregion
    }
}
