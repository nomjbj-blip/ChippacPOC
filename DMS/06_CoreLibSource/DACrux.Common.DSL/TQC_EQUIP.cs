using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.DSL
{
    public class TQC_EQUIP : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_EQUIP()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_EQUIP.xml");
        }

        #endregion

        /// <summary>
        /// Equip 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetEquipList(string factory, string[] areaArr, string[] operArr, string[] equipModelArr)
        {
            StringBuilder sb = new StringBuilder();

            if (areaArr != null && areaArr.Length > 0)
                sb.AppendLine(String.Format("AND AREA IN ('{0}')", String.Join("','", areaArr)));

            if (operArr != null && operArr.Length > 0)
                sb.AppendLine(String.Format("AND OPER IN ('{0}')", String.Join("','", operArr)));

            if (equipModelArr != null && equipModelArr.Length > 0)
                sb.AppendLine(String.Format("AND EQUIP_MODEL IN ('{0}')", String.Join("','", equipModelArr)));

            return GetDataTable("SELECT_EQUIP_LIST", new string[] { sb.ToString() }, new string[] { factory });
        }

        /// <summary>
        /// Equip 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        public bool ExistsEquipData(string factory, string equipID)
        {
            object obj = ExecuteScalar("EXISTS_EQUIP_DATA", null, new string[] { factory, equipID });

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        /// <summary>
        /// Equip 데이터를 추가 합니다.
        /// </summary>
        public void InsertEquipData(Dictionary<string, string> dic, string userID)
        {
            ExecuteNonQuery("INSERT_DATA", null, new string[]
            {
                dic["FACTORY"],
                dic["EQUIP_ID"],
                dic["HANDLER"],
                dic["EQUIP_MODEL"],
                dic["AREA"],
                dic["OPER"],
                userID,
                dic["QTY_1"],
                dic["QTY_2"],
                dic["QTY_3"],
                dic["QTY_4"],
                dic["QTY_5"],
                dic["QTY_6"],
                dic["QTY_7"],
                dic["QTY_8"],
                dic["QTY_9"],
                dic["EQUIP_GRP_1"],
                dic["EQUIP_GRP_2"],
                dic["EQUIP_GRP_3"],
                dic["EQUIP_GRP_4"],
                dic["EQUIP_GRP_5"],
                dic["EQUIP_GRP_6"],
                dic["EQUIP_GRP_7"],
                dic["EQUIP_GRP_8"],
                dic["EQUIP_GRP_9"],
                dic["EQUIP_STS_1"],
                dic["EQUIP_STS_2"],
                dic["EQUIP_STS_3"],
                dic["EQUIP_STS_4"],
                dic["EQUIP_STS_5"],
                dic["EQUIP_STS_6"],
                dic["EQUIP_STS_7"],
                dic["EQUIP_STS_8"],
                dic["EQUIP_STS_9"],
                dic["EQUIP_CMF_1"],
                dic["EQUIP_CMF_2"],
                dic["EQUIP_CMF_3"],
                dic["EQUIP_CMF_4"],
                dic["EQUIP_CMF_5"],
                dic["EQUIP_CMF_6"],
                dic["EQUIP_CMF_7"],
                dic["EQUIP_CMF_8"],
                dic["EQUIP_CMF_9"]
            });
        }

        /// <summary>
        /// Equip 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData(Dictionary<string, string> dic, string userID)
        {
            ExecuteNonQuery("UPDATE_DATA", null, new string[]
            {
                dic["FACTORY"],
                dic["EQUIP_ID"],
                dic["HANDLER"],
                dic["EQUIP_MODEL"],
                dic["AREA"],
                dic["OPER"],
                userID,
                dic["QTY_1"],
                dic["QTY_2"],
                dic["QTY_3"],
                dic["QTY_4"],
                dic["QTY_5"],
                dic["QTY_6"],
                dic["QTY_7"],
                dic["QTY_8"],
                dic["QTY_9"],
                dic["EQUIP_GRP_1"],
                dic["EQUIP_GRP_2"],
                dic["EQUIP_GRP_3"],
                dic["EQUIP_GRP_4"],
                dic["EQUIP_GRP_5"],
                dic["EQUIP_GRP_6"],
                dic["EQUIP_GRP_7"],
                dic["EQUIP_GRP_8"],
                dic["EQUIP_GRP_9"],
                dic["EQUIP_STS_1"],
                dic["EQUIP_STS_2"],
                dic["EQUIP_STS_3"],
                dic["EQUIP_STS_4"],
                dic["EQUIP_STS_5"],
                dic["EQUIP_STS_6"],
                dic["EQUIP_STS_7"],
                dic["EQUIP_STS_8"],
                dic["EQUIP_STS_9"],
                dic["EQUIP_CMF_1"],
                dic["EQUIP_CMF_2"],
                dic["EQUIP_CMF_3"],
                dic["EQUIP_CMF_4"],
                dic["EQUIP_CMF_5"],
                dic["EQUIP_CMF_6"],
                dic["EQUIP_CMF_7"],
                dic["EQUIP_CMF_8"],
                dic["EQUIP_CMF_9"]
            });
        }

        /// <summary>
        /// 설비 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData01(string factory, string equipID, string equip_sts_5, string equip_sts_6)
        {
            Execute("UPDATE_DATA_01", null, new string[] { factory, equipID, equip_sts_5, equip_sts_6 });
        }

        /// <summary>
        /// 설비 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData02(string factory, string equipID, 
            string equip_sts_1, string equip_sts_3, string equip_sts_4)
        {
            ExecuteNonQuery("UPDATE_DATA_02", null, new string[] { factory, equipID, equip_sts_1, equip_sts_3, equip_sts_4 });
        }

        /// <summary>
        /// 설비 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData02(string[,] arr)
        {
            ExecuteMultiple("UPDATE_DATA_02", arr);
        }

        /// <summary>
        /// 설비 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData03(string factory, string equipID,
            string equip_sts_7, string equip_sts_8, string equip_sts_9)
        {
            ExecuteNonQuery("UPDATE_DATA_03", null, new string[] { factory, equipID, equip_sts_7, equip_sts_8, equip_sts_9 });
        }


        /// <summary>
        /// 설비 데이터를 업데이트 합니다.
        /// </summary>
        public void UpdateEquipData04(
            string factory, 
            string equipID,
            string equip_sts_8)
        {
            ExecuteNonQuery("UPDATE_DATA_04", null, new string[] { factory, equipID, equip_sts_8 });
        }

        /// <summary>
        /// PCM 설비 상태값을 가져옵니다.
        /// </summary>
        public DataTable GetPcmEquipStatus(string factory)
        {
            return GetDataTable("SELECT_PCM_EQUIP_STATUS", null, new string[] { factory });
        }

        /// <summary>
        /// Equip 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteEquipData(string factory, string equipID)
        {
            ExecuteNonQuery("DELETE_DATA", null, new string[] { factory, equipID });
        }

        public DataTable GetEquipInfo(string factory, string equipID)
        {
            return GetDataTable("SELECT_EQUIP_INFO", null, new string[] { factory, equipID });
        }

        public DataTable GetEquipIDList(string factory, string oper)
        {
            return GetDataTable("SELECT_EQUIP_ID", null, new string[] { factory, oper });
        }
    }
}
