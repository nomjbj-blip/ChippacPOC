using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.DSL
{
    public class TQC_EQUIP_MODEL : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_EQUIP_MODEL()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_EQUIP_MODEL.xml");
        }

        #endregion

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        public DataTable GetEquipModel(string factory)
        {
            return GetDataTable("SELECT_EQUIP_MODEL_01", null, new string[] { factory });
        }

        /// <summary>
        /// Equip Model 정보를 가져옵니다.
        /// </summary>
        public DataTable GetEquipModel02(
            string factory
            )
        {
            return GetDataTable(
                "SELECT_EQUIP_MODEL_02",
                null,
                new string[] { 
                    factory
                });
        }

        //--

        public bool ExistEquipModelData(
            string factory,
            string equipModel
            )
        {
            object obj = ExecuteScalar("EXISTS_EQUIP_MODEL_DATA", null, new string[] { factory, equipModel });

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        public void InsertEquipModelData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            ExecuteNonQuery(
                "INSERT_EQUIP_MODEL_01",
                null,
                new string[] {
                    dic["FACTORY"],
                    dic["EQUIP_MODEL"],
                    dic["EQUIP_MODEL_DESC"],
                    dic["CMF_FIELD01"],
                    dic["CMF_FIELD02"],
                    dic["CMF_FIELD03"],
                    dic["CMF_FIELD04"],
                    dic["CMF_FIELD05"],
                    dic["CMF_FIELD06"],
                    dic["CMF_FIELD07"],
                    dic["CMF_FIELD08"],
                    dic["CMF_FIELD09"],
                    dic["CMF_FIELD10"],
                    userID
                });
        }

        public void UpdateEquipModelData(
            Dictionary<string, string> dic,
            string userID
            )
        {
            ExecuteNonQuery(
                "UPDATE_EQUIP_MODEL_01",
                null,
                new string[] {
                    dic["FACTORY"],
                    dic["EQUIP_MODEL"],
                    dic["EQUIP_MODEL_DESC"],
                    dic["CMF_FIELD01"],
                    dic["CMF_FIELD02"],
                    dic["CMF_FIELD03"],
                    dic["CMF_FIELD04"],
                    dic["CMF_FIELD05"],
                    dic["CMF_FIELD06"],
                    dic["CMF_FIELD07"],
                    dic["CMF_FIELD08"],
                    dic["CMF_FIELD09"],
                    dic["CMF_FIELD10"],
                    userID
                });
        }

        public DataTable GetEquipModel(string factory, string[] areas, string[] opers)
        {
            return GetDataTable(
                "SELECT_EQUIP_MODEL_03",
                new string[] { string.Format("'{0}'", string.Join("','", areas)), string.Format("'{0}'", string.Join("','", opers)) },
                new string[] { factory }
                );
        }

        public void DeleteEquipModelData(
            string factory,
            string equipModel
            )
        {
            ExecuteNonQuery(
                "DELETE_EQUIP_MODEL_01",
                null,
                new string[] {
                    factory,
                    equipModel
                });
        }
    }
}
