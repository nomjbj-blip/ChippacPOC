using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 GDS Map 조회(읽기 전용) 서비스를 화면에 제공한다.</summary>
    public class GdsMapView
    {
        DACrux.SEMDMS.Interface.iGdsMapView m_OBJ;

        public GdsMapView()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iGdsMapView),
                        strUrl + "/DACrux.SEMDMS.BSL.GdsMapView.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iGdsMapView;
        }

        #region [TQP_GDS_MAP]

        public DataTable GetMapDeviceList()
        {
            try
            {
                return m_OBJ.GetMapDeviceList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapInfo(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetMapInfo(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_LAYER]

        public DataTable GetMapLayerList(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetMapLayerList(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_PLACED_EL]

        public DataTable GetPlacedElementPage(string strFactory, string strDeviceID, string strLayerID, string strAfterPlacedID, int iMaxRows)
        {
            try
            {
                return m_OBJ.GetPlacedElementPage(strFactory, strDeviceID, strLayerID, strAfterPlacedID, iMaxRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
