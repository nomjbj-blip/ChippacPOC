using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPMATDEF : Miracom.Middleware.QueryComponent
    {
        public MES_MWIPMATDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_MWIPMATDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectMateriallist(string factory, string owner_code)
        {
            return this.GetDataTable("SELECT_MATERIAL_LIST", null, new string[] { factory, owner_code });
        }

        public DataTable SelectMateriallist(string factory, string prod_type, string prod_group)
        {
            string sDynQry = string.Empty;

            if (string.IsNullOrEmpty(prod_type) == false)
            {
                sDynQry += string.Format(" AND MAT_ID IN (");

                sDynQry += string.Format(" SELECT DISTINCT MAT_ID");
                sDynQry += string.Format(" FROM");
                sDynQry += string.Format(" (");
                sDynQry += string.Format(" SELECT MAT_ID, ( CASE WHEN ISNULL(TB.MAT_CMF_11,'') <> '' THEN TB.MAT_CMF_11 ELSE ");
                sDynQry += string.Format(" CASE WHEN ISNULL(TB.MAT_CMF_6,'') <> '' THEN TB.MAT_CMF_6 ELSE TB.MAT_GRP END END) AS MAT_GRP");
                sDynQry += string.Format(" FROM ( SELECT ");
                sDynQry += string.Format(" MAT_ID,");
                sDynQry += string.Format(" (SELECT  DISTINCT DATA_1 FROM MGCMTBLDAT ");
                sDynQry += string.Format(" WHERE 1 = 1 ");
                sDynQry += string.Format(" AND FACTORY = MAT.FACTORY");
                sDynQry += string.Format(" AND TABLE_NAME = 'C$MAT_GRP'");
                sDynQry += string.Format(" AND KEY_1 = MAT.MAT_TYPE");
                sDynQry += string.Format(" AND DATA_1 <> ' ') MAT_GRP,");
                sDynQry += string.Format(" MAT_CMF_6, MAT_CMF_11");
                sDynQry += string.Format(" FROM CWIPCTLDEF MAT");
                sDynQry += string.Format(" WHERE 1 = 1 ");
                sDynQry += string.Format(" AND MAT.FACTORY = '{0}'", factory);
                sDynQry += string.Format(" AND MAT.DELETE_FLAG <> 'Y'");
                sDynQry += string.Format("			) TB");
                sDynQry += string.Format(" ) MT");
                sDynQry += string.Format(" WHERE MT.MAT_GRP = '{0}'", prod_type);

                sDynQry += string.Format(" )");
            }

            if (string.IsNullOrEmpty(prod_group) == false)
            {
                sDynQry += string.Format(" AND MAT_DESC like '{0}%' ", prod_group);
            }

            return this.GetDataTable("SELECT_MAT_LIST_COND", new string[] { sDynQry }, new string[] { factory });
        }

        public DataTable SelectMaterialInfo(string factory, string mat_id)
        {
            if (mat_id.Trim() == string.Empty) mat_id = "ALL";
            return this.GetDataTable("SELECT_MAT_INFO", null, new string[] { factory, mat_id });
        }

        public DataTable SelectMaterialIdList(
            string factory
            )
        {
            return this.GetDataTable("SELECT_MAT_ID_LIST", null, new string[] { factory });
        }

        public DataTable SelectMaterialIdList(
            string factory,
            string searchMatId
            )
        {
            if (searchMatId == string.Empty)
            {
                return this.GetDataTable("SELECT_MAT_LIST_01", null, new string[] { factory });
            }

            // --
            return this.GetDataTable("SELECT_MAT_LIST_02", null, new string[] { factory, searchMatId });
        }


        public System.Data.DataTable SelectMaterialInfoDetail(
            string factory,
            string sMat_id
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MAT_INFO_DETAIL", null, new string[] { factory, sMat_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectMaterialInfoDetail(
            string factory
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MAT_INFO_DETAIL_RMS", null, new string[] { factory });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public System.Data.DataTable GetMatOper(string factory, string sMat_id, string oper)
        {
            try
            {
                return this.GetDataTable("SELECT_MAT_OPER", null, new string[] { factory, sMat_id, oper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
