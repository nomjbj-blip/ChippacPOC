using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.Interface
{
    public interface iEquipManagement
    {
        /// <summary>
        /// Area 정보를 가져옵니다.
        /// </summary>
        DataTable GetArea(string factory);

        /// <summary>
        /// Oper 정보를 가져옵니다.
        /// </summary>
        DataTable GetOper(string factory, string[] areaArr);

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        DataTable GetEquipModel(string factory);

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        DataTable GetEquipModel(string factory, string[] areas, string[] opers);

        /// <summary>
        /// Equip Handler 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        string[] GetHanlderNames();

        //--

        #region Equipment Model Setup
        /// <summary>
        /// Equip Model 데이터 조회
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        DataTable GetEquipModel02(string factory);

        /// <summary>
        /// Equip Model 데이터가 존재하는지 확인
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="equipModel"></param>
        bool ExistsEquipModelData(string factory, string equipModel);

        /// <summary>
        /// Equip Model 데이터를 추가합니다.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="userid"></param>
        void InsertEquipModelData(Dictionary<string, string> dic, string userid);

        /// <summary>
        /// Equip Model 데이터를 업데이트 합니다.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="userID"></param>
        void UpdateEquipModelData(Dictionary<string, string> dic, string userID);

        /// <summary>
        /// Equip Model 데이터를 삭제합니다.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="equipModel"></param>
        void DeleteEquipModelData(string factory, string equipModel);
        #endregion Equipment Model Setup

        //--

        #region Equipment Setup
        /// <summary>
        /// Equip 리스트를 가져옵니다.
        /// </summary>
        DataTable GetEquipList(string factory, string[] areaArr, string[] operArr, string[] equipModelArr);

        /// <summary>
        /// Equip 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        bool ExistsEquipData(string factory, string equipID);

        /// <summary>
        /// Equip 데이터를 추가 합니다.
        /// </summary>
        void InsertEquipData(Dictionary<string, string> dic, string userID);

        /// <summary>
        /// Equip 데이터를 업데이트 합니다.
        /// </summary>
        void UpdateEquipData(Dictionary<string, string> dic, string userID);

        /// <summary>
        /// Equip 데이터를 삭제 합니다.
        /// </summary>
        void DeleteEquipData(string factory, string equipID);

        DataTable GetEquipIDList(string factory, string oper);

        #endregion Equipment Setup

        //--

        #region Oper Setup

        /// <summary>
        /// Oper List 를 가져옵니다.
        /// </summary>
        DataTable GetOperData02(string factory, string[] arrArea);

        /// <summary>
        /// Oper 데이터가 존재하는지 가져옵니다.
        /// </summary>
        bool ExistsOperData(string factory, string area, string oper);

        /// <summary>
        /// Oper 데이터를 추가합니다
        /// </summary>
        void InsertOperData(Dictionary<string, string> dic, string userID);

        /// <summary>
        /// Oper 데이터를 갱신합니다
        /// </summary>
        void UpdateOperData(Dictionary<string, string> dic, string userID);

        /// <summary>
        /// Oper 데이터를 삭제합니다
        /// </summary>
        void DeleteOperData(string factory, string area, string oper);

        #endregion Oper Setup


        #region MES Lot Status 정보를 가져온다.

        DataTable SelectLotSTS(string strLotID);

        #endregion MES Lot Status 정보를 가져온다.
    }
}
