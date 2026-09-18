using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DACrux.Common.RO
{
    public class EquipManagement
    {
        #region 멤버 변수

        DACrux.Common.Interface.iEquipManagement m_OBJ;
        
        #endregion

        #region 생성자

        public EquipManagement()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);

            m_OBJ = Activator.GetObject(typeof(DACrux.Common.Interface.iEquipManagement),
                        strUrl + "/DACrux.Common.BSL.EquipManagement.bin") as DACrux.Common.Interface.iEquipManagement;
        }

        #endregion

        #region 조회 조건 관련 메서드

        /// <summary>
        /// Area 정보를 가져옵니다.
        /// </summary>
        public DataTable GetArea(string factory)
        {
            return m_OBJ.GetArea(factory);
        }

        /// <summary>
        /// Oper 정보를 가져옵니다.
        /// </summary>
        public DataTable GetOper(string factory, string[] areaArr)
        {
            return m_OBJ.GetOper(factory, areaArr);
        }

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        public DataTable GetEquipModel(string factory)
        {
            return m_OBJ.GetEquipModel(factory);
        }

        public DataTable GetEquipModel(
            string factory, 
            string[] areas, 
            string[] opers
            )
        {
            return m_OBJ.GetEquipModel(
                factory,
                areas,
                opers
                );
        }

        public string[] GetHanlderNames()
        {
            return m_OBJ.GetHanlderNames();
        }
        #endregion

        #region Equip Setup 관련 메서드

        /// <summary>
        /// Equip 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetEquipList(string factory, string[] areaArr, string[] operArr, string[] equipModelArr)
        {
            return m_OBJ.GetEquipList(factory, areaArr, operArr, equipModelArr);
        }

        /// <summary>
        /// Equip 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        public bool ExistsEquipData(string factory, string equipID)
        {
            return m_OBJ.ExistsEquipData(factory, equipID);
        }

        /// <summary>
        /// Equip 데이터를 추가 합니다.
        /// </summary>
        public void InsertEquipData(Dictionary<string, string> dic, string userID)
        {
            m_OBJ.InsertEquipData(dic, userID);
        }

        /// <summary>
        /// Equip 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData(Dictionary<string, string> dic, string userID)
        {
            m_OBJ.UpdateEquipData(dic, userID);
        }

        /// <summary>
        /// Equip 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteEquipData(string factory, string equipID)
        {
            m_OBJ.DeleteEquipData(factory, equipID);
        }

        public DataTable GetEquipIDList(string factory, string oper)
        {
            return m_OBJ.GetEquipIDList(factory, oper);
        }

        #endregion

        #region Equip Model Setup 관련 메서드

        public DataTable GetEquipModel02(
            string factory
            )
        {
            return m_OBJ.GetEquipModel02(
                factory
                );
        }

        public bool ExistsEquipModelData(
            string factory,
            string equipModel
            )
        {
            return m_OBJ.ExistsEquipModelData(
                factory,
                equipModel
                );
        }

        public void InsertEquipModelData(
            Dictionary<string, string> dic, 
            string userid
            )
        {
            m_OBJ.InsertEquipModelData(
                dic, 
                userid
                );
        }

        public void UpdateEquipModelData(
            Dictionary<string, string> dic, 
            string userID
            )
        {
            m_OBJ.UpdateEquipModelData(
                dic, 
                userID
                );
        }

        public void DeleteEquipModelData(
            string factory, 
            string equipModel
            )
        {
            m_OBJ.DeleteEquipModelData(
                factory, 
                equipModel
                );
        }

        #endregion Equip Model Setup 관련 메서드

        #region Oper Setup 관련 메서드
        public DataTable GetOper02(
            string factory,
            string[] arrArea
            )
        {
            return m_OBJ.GetOperData02(
                factory,
                arrArea
                );
        }

        public bool ExistsOperData(
            string factory,
            string area,
            string oper
            )
        {
            return m_OBJ.ExistsOperData(
                factory,
                area,
                oper
                );
        }

        public void InsertOperData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            m_OBJ.InsertOperData(
                dic,
                userID
                );
        }

        public void UpdateOperData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            m_OBJ.UpdateOperData(
                dic,
                userID
                );
        }

        public void DeleteOperData(
            string factory,
            string area,
            string oper
            )
        {
            m_OBJ.DeleteOperData(
                factory,
                area,
                oper
                );
        }
        #endregion Oper Setup 관련 메서드


        #region MES Lot Status 정보를 가져온다.

        public DataTable SelectLotSTS(string strLotID)
        {
            return m_OBJ.SelectLotSTS(strLotID);
        }

        #endregion MES Lot Status 정보를 가져온다.
    }
}
