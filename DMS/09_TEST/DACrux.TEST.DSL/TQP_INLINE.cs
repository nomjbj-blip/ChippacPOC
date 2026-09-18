using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_INLINE : Miracom.Middleware.QueryComponent
    {
        public TQP_INLINE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_INLINE.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Common Select]
        public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(string sqlName, string[,] paras)
        {
            try
            {
                this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public DataTable GetLotList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            // 孽府积己
            string strQuery = string.Empty;
            string strField = string.Empty;
            string strTable = string.Empty;
            string strCondition = string.Empty;
            string strOrder = string.Empty;

            strField = "Select distinct " + string.Join(", ", strFields) + " ";
            strField = strField.Replace("Fac", "'AFB1' Fac");

            strTable = "From MFD_EDC0 ";
            strCondition = string.Format("Where DTTM between TO_DATE('{0}','YYYY-MM-DD') and TO_DATE('{1}','YYYY-MM-DD') ", StartTime, EndTime);

            if (strWhere != null && strWhere.Length > 0)
            {
                for (int i = 0; i < strWhere.Length; i++)
                {
                    strCondition += " And " + strWhere[i];
                }
                strCondition += " ";
            }

            strOrder = "Order by " + strSort + " ";

            strQuery = strField + strTable + strCondition + strOrder;

            // 孽府角青
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_INLINE_CONDITION", new string[] { strQuery }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetInlineLot(string strProd, string strLpt, string strOpn)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_INLINE_LOT", null, new string[] { strProd, strLpt, strOpn });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetInlinePara(string strProd, string strLpt, string strOpn)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_INLINE_PARA", null, new string[] { strProd, strLpt, strOpn });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetParaSpec(string strProd, string strLpt, string strOpn, string strPara)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_INLINE_SPEC", null, new string[] { strProd, strLpt, strOpn, strPara });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetInlineSiteData(string strProd, string strLpt, string strOpn, string strPara, string[] strLot)
        {
            DataTable dt = null;
            string Lot = "'" + String.Join("', '", strLot) + "'";

            try
            {
                dt = this.GetDataTable("GET_INLINE_SITEDATA", new string[] { Lot }, new string[] { strProd, strLpt, strOpn, strPara });
                if (dt.Rows.Count < 1) return null;

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable GetInlineData(string strProd, string strLpt, string strOpn, string[] strLot, string[] strPara)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("GET_INLINE_DATA", null, new string[] { strProd, strLpt, strOpn });
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }
    }
}
