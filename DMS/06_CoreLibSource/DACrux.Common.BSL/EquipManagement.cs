using System;
using System.Collections.Generic;
using System.Data;
using DACrux.Common.DSL;
using System.Reflection;

namespace DACrux.Common.BSL
{
    public class EquipManagement
        : Miracom.Middleware.BaseComponent, DACrux.Common.Interface.iEquipManagement
    {
        #region 조회 조건 관련 메서드

        /// <summary>
        /// Area 정보를 가져옵니다.
        /// </summary>
        public DataTable GetArea(string factory)
        {
            TQC_OPER obj = new TQC_OPER();
            return obj.GetArea(factory);
        }

        /// <summary>
        /// Oper 정보를 가져옵니다.
        /// </summary>
        public DataTable GetOper(string factory, string[] areaArr)
        {
            TQC_OPER obj = new TQC_OPER();
            return obj.GetOper(factory, areaArr);
        }

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        public DataTable GetEquipModel(string factory)
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            return obj.GetEquipModel(factory);
        }

        public DataTable GetEquipModel(
            string factory,
            string[] areas,
            string[] opers
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            return obj.GetEquipModel(
                factory,
                areas,
                opers
                );
        }

        /// <summary>
        /// Handler 클래스 명 배열을 가져옵니다.
        /// </summary>
        public string[] GetHanlderNames()
        {
            List<string> list = new List<string>();

            Type baseType = typeof(DACrux.Data.Handler.Handler);

            foreach (Type t in baseType.Module.GetTypes())
            {
                if (!t.IsAbstract && t.IsSubclassOf(baseType))
                    list.Add(t.Name);
            }

            list.Sort();
            return list.ToArray();
        }

        #endregion

        #region Equip Setup 관련 메서드

        /// <summary>
        /// Equip 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetEquipList(string factory, string[] areaArr, string[] operArr, string[] equipModelArr)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            return obj.GetEquipList(factory, areaArr, operArr, equipModelArr);
        }

        /// <summary>
        /// Equip 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        public bool ExistsEquipData(string factory, string equipID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            return obj.ExistsEquipData(factory, equipID);
        }

        /// <summary>
        /// Equip 데이터를 추가 합니다.
        /// </summary>
        public void InsertEquipData(Dictionary<string, string> dic, string userID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            obj.InsertEquipData(dic, userID);
        }

        /// <summary>
        /// Equip 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData(Dictionary<string, string> dic, string userID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            obj.UpdateEquipData(dic, userID);
        }

        /// <summary>
        /// Equip 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteEquipData(string factory, string equipID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            obj.DeleteEquipData(factory, equipID);
        }

        /// <summary>
        /// EquipInfo 정보를 가져옵니다.
        /// </summary>
        public DACrux.Data.Handler.EquipInfo GetEquipInfo(string factory, string equipID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            DataTable dt = obj.GetEquipInfo(factory, equipID);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            DACrux.Data.Handler.EquipInfo info = new Data.Handler.EquipInfo();
            info.Factory = dt.Rows[0]["FACTORY"].ToString();
            info.EquipID = dt.Rows[0]["EQUIP_ID"].ToString();
            info.Handler = dt.Rows[0]["HANDLER"].ToString();
            info.EquipModel = dt.Rows[0]["EQUIP_MODEL"].ToString();
            info.Area = dt.Rows[0]["AREA"].ToString();
            info.Oper = dt.Rows[0]["OPER"].ToString();

            info.Grp01 = dt.Rows[0]["EQUIP_GRP_1"].ToString(); // DM : 설비 별 추가 복사 경로 2019.10.24 Taihi,Kim.
            info.Grp02 = dt.Rows[0]["EQUIP_GRP_2"].ToString();
            info.Grp03 = dt.Rows[0]["EQUIP_GRP_3"].ToString();
            info.Grp04 = dt.Rows[0]["EQUIP_GRP_4"].ToString();
            info.Grp05 = dt.Rows[0]["EQUIP_GRP_5"].ToString();
            info.Grp06 = dt.Rows[0]["EQUIP_GRP_6"].ToString();
            info.Grp07 = dt.Rows[0]["EQUIP_GRP_7"].ToString();
            info.Grp08 = dt.Rows[0]["EQUIP_GRP_8"].ToString();
            info.Grp09 = dt.Rows[0]["EQUIP_GRP_9"].ToString();

            info.Sts01 = dt.Rows[0]["EQUIP_STS_1"].ToString();
            info.Sts02 = dt.Rows[0]["EQUIP_STS_2"].ToString();
            info.Sts03 = dt.Rows[0]["EQUIP_STS_3"].ToString();
            info.Sts04 = dt.Rows[0]["EQUIP_STS_4"].ToString();
            info.Sts05 = dt.Rows[0]["EQUIP_STS_5"].ToString();
            info.Sts06 = dt.Rows[0]["EQUIP_STS_6"].ToString();
            info.Sts07 = dt.Rows[0]["EQUIP_STS_7"].ToString();
            info.Sts08 = dt.Rows[0]["EQUIP_STS_8"].ToString();
            info.Sts09 = dt.Rows[0]["EQUIP_STS_9"].ToString();

            info.Cmf01 = dt.Rows[0]["EQUIP_CMF_1"].ToString(); // FAB1 DM 데이터 회전각도(예.180), PCM PROBE CNT 증가 여부
            info.Cmf02 = dt.Rows[0]["EQUIP_CMF_2"].ToString(); // FAB1 DM Review폴더명(예.AIT,KLA)
            info.Cmf03 = dt.Rows[0]["EQUIP_CMF_3"].ToString();
            info.Cmf04 = dt.Rows[0]["EQUIP_CMF_4"].ToString();
            info.Cmf05 = dt.Rows[0]["EQUIP_CMF_5"].ToString();
            info.Cmf06 = dt.Rows[0]["EQUIP_CMF_6"].ToString();
            info.Cmf07 = dt.Rows[0]["EQUIP_CMF_7"].ToString(); // REVIEW 파일을 LOTEND 때 생성할지 여부 (N: 파싱때 마다, Y : LOTEND 파일이 올라올때마다)
            info.Cmf08 = dt.Rows[0]["EQUIP_CMF_8"].ToString();
            info.Cmf09 = dt.Rows[0]["EQUIP_CMF_9"].ToString();

            return info;
        }

        public DataTable GetEquipIDList(string factory, string oper)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            return obj.GetEquipIDList(factory, oper);
        }

        #endregion

        #region Equip Model Setup 관련 메서드

        public DataTable GetEquipModel02(
            string factory
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            return obj.GetEquipModel02(
                factory
                );
        }

        public bool ExistsEquipModelData(
            string factory,
            string equipModel
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            return obj.ExistEquipModelData(
                factory,
                equipModel
                );
        }

        public void InsertEquipModelData(
            Dictionary<string, string> dic,
            string userid
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            obj.InsertEquipModelData(
                dic,
                userid
                );
        }

        public void UpdateEquipModelData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            obj.UpdateEquipModelData(
                dic,
                userID
                );
        }

        public void DeleteEquipModelData(
            string factory,
            string equipModel
            )
        {
            TQC_EQUIP_MODEL obj = new TQC_EQUIP_MODEL();
            obj.DeleteEquipModelData(
                factory,
                equipModel
                );
        }
        #endregion  Equip Model Setup 관련 메서드

        #region Oper Setup 관련 메서드
        public DataTable GetOperData02(
            string factory,
            string[] arrArea
            )
        {
            TQC_OPER obj = new TQC_OPER();
            return obj.GetOper02(
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
            TQC_OPER obj = new TQC_OPER();
            return obj.ExistsOperData(
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
            TQC_OPER obj = new TQC_OPER();
            obj.InsertOperData01(
                dic,
                userID
                );
        }

        public void UpdateOperData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            TQC_OPER obj = new TQC_OPER();
            obj.UpdateOperData01(
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
            TQC_OPER obj = new TQC_OPER();
            obj.DeleteOperData01(
                factory,
                area,
                oper
                );
        }

        #endregion Oper Setup 관련 메서드


        #region MES Lot Status 정보를 가져온다.

        public DataTable SelectLotSTS(string strLotID)
        {
            TQC_LOT_STS obj = new TQC_LOT_STS();
            return obj.SelectLotSTS(strLotID);
        }

        public DateTime SelectSysdate()
        {
            string strSysdate = string.Empty;
            DateTime dtTime;
            TQC_LOT_STS oTQC_LOT_STS = new TQC_LOT_STS();
            strSysdate = oTQC_LOT_STS.SelectSysDate().Rows[0][0].ToString();
            DateTime.TryParseExact(strSysdate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dtTime);
            return dtTime;
        }

        #endregion MES Lot Status 정보를 가져온다.
    }
}
