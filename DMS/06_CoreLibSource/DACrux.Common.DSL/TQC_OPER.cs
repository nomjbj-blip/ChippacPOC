using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.DSL
{
    public class TQC_OPER : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_OPER()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_OPER.xml");
        }

        #endregion

        /// <summary>
        /// Area 정보를 가져옵니다.
        /// </summary>
        public DataTable GetArea(string factory)
        {
            return GetDataTable("SELECT_AREA_01", null, new string[] { factory });
        }

        /// <summary>
        /// Oper 정보를 가져옵니다.
        /// </summary>
        public DataTable GetOper(string factory, string[] areaArr)
        {
            StringBuilder sb = new StringBuilder();

            if (areaArr != null && areaArr.Length > 0)
                sb.AppendLine(String.Format("AND AREA IN ('{0}')", String.Join("','", areaArr)));

            return GetDataTable(
                "SELECT_OPER_01", 
                new string[] { sb.ToString() }, 
                new string[] { factory } );
        }


        /// <summary>
        /// Oper 정보를 가져옵니다.
        /// </summary>
        public DataTable GetOper02(
            string factory, 
            string[] arrArea
            )
        {
            StringBuilder sb = new StringBuilder();
            if (arrArea != null && arrArea.Length > 0)
                sb.AppendLine(String.Format("AND AREA IN ('{0}')", String.Join("','", arrArea)));

            return GetDataTable(
                "SELECT_OPER_02",
                new string[] { sb.ToString() },
                new string[] { factory }
                );
        }

        public bool ExistsOperData(
            string factory,
            string area,
            string oper
            )
        {
            object obj = ExecuteScalar(
                "EXISTS_OPER_DATA",
                null,
                new string[] { factory, area, oper });
            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        public void InsertOperData01(
            Dictionary<string, string> dic,
            string userID
            )
        {
            ExecuteNonQuery(
                "INSER_OPER_01",
                null,
                new string[] {
                    dic["FACTORY"],
                    dic["AREA"],
                    dic["OPER"],
                    dic["OPER_DESC"],
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

        public void UpdateOperData01(
            Dictionary<string, string> dic,
            string userID
            )
        {
            ExecuteNonQuery(
                "UPDATE_OPER_01",
                null,
                new string[] {
                    dic["FACTORY"],
                    dic["AREA"],
                    dic["OPER"],
                    dic["OPER_DESC"],
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

        public void DeleteOperData01(
            string factory,
            string area,
            string oper
            )
        {
            ExecuteNonQuery(
                "DELETE_OPER_01",
                null,
                new string[] {
                    factory,
                    area,
                    oper
                });
        }

    }
}
