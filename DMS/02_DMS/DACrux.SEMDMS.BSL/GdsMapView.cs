using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS Map 조회 화면의 읽기 전용 업무 검증과 DSL 호출을 담당한다.
    /// 설계: 문서/설계/02_GDS/2026-10-02_GDS_Map_조회화면_설계.md 4~5절 / 15절.
    /// 2026-10-02 리비전 관리 제거로 조회 키는 Factory + Device다(Device당 Map 1개).
    /// 저장 중이거나 실패한 Map(DRAFT)은 도형 일부만 있을 수 있으므로 READY Map만 조회한다.
    /// </summary>
    public class GdsMapView : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsMapView
    {
        /// <summary>
        /// 한 번에 넘기는 DataTable 크기 상한. 좌표 BLOB이 들어 있어 행 수가 크면 Remoting 메시지가 수십 MB가 된다.
        /// 기본 페이지 크기는 화면에서 정하고 서버는 상한만 막는다.
        /// </summary>
        private const int MaxPageRows = 50000;

        public GdsMapView()
        {
        }

        #region [TQP_GDS_MAP]

        /// <summary>READY Map이 있는 Factory / Device 목록을 조회한다.</summary>
        public DataTable GetMapDeviceList()
        {
            DACrux.SEMDMS.DSL.TQP_GDS_MAP oTQP_GDS_MAP = new DACrux.SEMDMS.DSL.TQP_GDS_MAP();
            return oTQP_GDS_MAP.GetData("SELECT_MAP_DEVICE_LIST", null, null);
        }

        /// <summary>Device의 Map 헤더 / 저장 작업 건수를 조회한다(저장 서비스와 같은 SELECT_MAP_INFO). 없으면 0행이다.</summary>
        public DataTable GetMapInfo(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_MAP oTQP_GDS_MAP = new DACrux.SEMDMS.DSL.TQP_GDS_MAP();
            return oTQP_GDS_MAP.GetData("SELECT_MAP_INFO", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        #endregion

        #region [TQP_GDS_LAYER]

        /// <summary>
        /// READY Map인지 확인한 뒤 Layer 목록과 Layer별 도형 수 / 좌표 수를 조회한다.
        /// Layer 는 번호 + DataType 단위다(KLayout 18/0, 18/3 처럼 나뉨). 결과에 DATA_TYPE 컬럼이 더 있다 (2026-10-20).
        /// </summary>
        public DataTable GetMapLayerList(string strFactory, string strDeviceID)
        {
            string[] key = RequireReadyMap(strFactory, strDeviceID);
            DACrux.SEMDMS.DSL.TQP_GDS_LAYER oTQP_GDS_LAYER = new DACrux.SEMDMS.DSL.TQP_GDS_LAYER();
            return oTQP_GDS_LAYER.GetData("SELECT_LAYER_WITH_COUNT_DT", null, new string[] { key[0], key[1], key[0], key[1] });
        }

        #endregion

        #region [TQP_GDS_PLACED_EL]

        /// <summary>
        /// Layer 배치 도형 한 페이지를 조회한다.
        /// 처리 흐름: 입력 검증 -> READY Map 확인 -> 첫 페이지 / 다음 페이지 쿼리 선택 -> PLACED_ELEMENT_ID 순서 최대 iMaxRows건.
        /// 다음 페이지는 이전 페이지 마지막 ID보다 큰 키부터 읽는다(OFFSET 방식은 뒤 페이지로 갈수록 느려짐).
        /// strLayerID: "18/3"(Layer 번호 / DataType)이면 그 DataType 만, "18"(번호만)이면 번호 전체를 읽는다(기존 호출 호환) (2026-10-20).
        /// </summary>
        public DataTable GetPlacedElementPage(string strFactory, string strDeviceID, string strLayerID, string strAfterPlacedID, int iMaxRows)
        {
            if (iMaxRows < 1 || iMaxRows > MaxPageRows)
                throw new ArgumentException("페이지 행 수는 1 ~ " + MaxPageRows + " 사이여야 합니다: " + iMaxRows, "iMaxRows");
            string strDataType;
            string strLayer = RequiredLayerId(strLayerID, out strDataType);
            string[] key = RequireReadyMap(strFactory, strDeviceID);
            string strMaxRows = iMaxRows.ToString(CultureInfo.InvariantCulture);

            DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL oTQP_GDS_PLACED_EL = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL();
            DataTable dt;
            if (string.IsNullOrEmpty(strAfterPlacedID))
            {
                if (strDataType == null)
                    dt = oTQP_GDS_PLACED_EL.GetData("SELECT_PLACED_EL_PAGE_FIRST", null,
                        new string[] { key[0], key[1], strLayer, strMaxRows });
                else
                    dt = oTQP_GDS_PLACED_EL.GetData("SELECT_PLACED_EL_PAGE_FIRST_DT", null,
                        new string[] { key[0], key[1], strLayer, strDataType, strMaxRows });
            }
            else
            {
                if (Encoding.UTF8.GetByteCount(strAfterPlacedID) > 1024)
                    throw new ArgumentException("이전 페이지 마지막 ID가 1024바이트를 넘습니다.", "strAfterPlacedID");
                if (strDataType == null)
                    dt = oTQP_GDS_PLACED_EL.GetData("SELECT_PLACED_EL_PAGE", null,
                        new string[] { key[0], key[1], strLayer, strAfterPlacedID, strMaxRows });
                else
                    dt = oTQP_GDS_PLACED_EL.GetData("SELECT_PLACED_EL_PAGE_DT", null,
                        new string[] { key[0], key[1], strLayer, strDataType, strAfterPlacedID, strMaxRows });
            }

            // 좌표 BLOB이 있어 기본 XML 직렬화는 Base64 문자열로 커진다.
            // 2026-10-02 측정(10,000행): XML 7.7MB / 194ms -> Binary 2.9MB / 28ms. 받는 쪽 코드는 바꿀 필요 없다.
            dt.RemotingFormat = SerializationFormat.Binary;
            return dt;
        }

        /// <summary>
        /// Label 검색 색인(2026-10-07). 처리 흐름: READY Map 확인 -> TEXT 도형 전체(ID / Layer / TEXTTYPE / 값 / 위치) 조회.
        /// Layer가 바뀌어도 찾을 수 있도록 Layer 조건 없이 도형 종류(TEXT)로만 찾는다. 샘플 기준 약 3만 행.
        /// </summary>
        public DataTable GetMapTextIndex(string strFactory, string strDeviceID)
        {
            string[] key = RequireReadyMap(strFactory, strDeviceID);
            DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL oTQP_GDS_PLACED_EL = new DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL();
            DataTable dt = oTQP_GDS_PLACED_EL.GetData("SELECT_PLACED_TEXT_INDEX", null, new string[] { key[0], key[1] });
            dt.RemotingFormat = SerializationFormat.Binary;
            return dt;
        }

        #endregion

        #region [공통 검증]

        /// <summary>Factory / Device 형식을 확인하고 READY Map이 아니면 오류를 낸다. 정리한 [Factory, Device]를 반환한다.</summary>
        private string[] RequireReadyMap(string strFactory, string strDeviceID)
        {
            DataTable dt = null;
            try
            {
                dt = GetMapInfo(strFactory, strDeviceID);
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("GDS Map이 없습니다: " + strFactory + " / " + strDeviceID);
                string strStatus = dt.Rows[0]["MAP_STATUS"].ToString();
                if (!string.Equals(strStatus, "READY", StringComparison.Ordinal))
                    throw new InvalidOperationException("저장이 끝난(READY) Map만 조회할 수 있습니다. 현재 상태: " + strStatus);
                return new string[] { dt.Rows[0]["FACTORY"].ToString(), dt.Rows[0]["DEVICE_ID"].ToString() };
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>
        /// GDS Layer 값을 확인한다. "18"(번호만) 또는 "18/3"(번호 / DataType 0 ~ 65535) 형식이다.
        /// 번호를 반환하고 DataType 은 strDataType 으로 준다(번호만이면 null) (2026-10-20).
        /// </summary>
        private static string RequiredLayerId(string strValue, out string strDataType)
        {
            strDataType = null;
            string strText = strValue == null ? string.Empty : strValue.Trim();
            int iSlash = strText.IndexOf('/');
            string strLayerText = iSlash < 0 ? strText : strText.Substring(0, iSlash).Trim();
            int iValue;
            if (!int.TryParse(strLayerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out iValue) || iValue < 0)
                throw new ArgumentException("Layer 번호는 0 이상의 정수여야 합니다: " + strValue, "strLayerID");
            if (iSlash >= 0)
            {
                int iDataType;
                if (!int.TryParse(strText.Substring(iSlash + 1).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out iDataType)
                    || iDataType < 0 || iDataType > 65535)
                    throw new ArgumentException("Layer DataType 은 0 ~ 65535 정수여야 합니다: " + strValue, "strLayerID");
                strDataType = iDataType.ToString(CultureInfo.InvariantCulture);
            }
            return iValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Oracle 컬럼 바이트 길이에 맞는 필수 문자열인지 확인한다.</summary>
        private static string Required(string strValue, string strName, int iMaxBytes)
        {
            string strResult = strValue == null ? string.Empty : strValue.Trim();
            if (strResult.Length == 0)
                throw new ArgumentException(strName + " 값이 필요합니다.", strName);
            if (Encoding.UTF8.GetByteCount(strResult) > iMaxBytes)
                throw new ArgumentException(strName + "는 " + iMaxBytes + "바이트 이하여야 합니다.", strName);
            return strResult;
        }

        #endregion
    }
}
