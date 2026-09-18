/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : TQP_PRODUCT.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.01.14
--  Description     : DACrux/SPC EDC DataSource Setup UI 
--  History         : Created by YSIM at 2013.01.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 * 2015-04-21 : YSLEE
    1. SelectProductInfo01(string product) : 신규생성, 해당 Product가 존재하는 지 유무 체크
    2. UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT) : 신규생성, 현재 존재하는 Product 데이터 업데이트
    3. UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT) : 신규생성, 현재 존재하는 Product 삭제(실제로 DELETE_FLAG = 'Y'만 변경
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_PRODUCT : Miracom.Middleware.QueryComponent
    {
        public TQP_PRODUCT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_PRODUCT.xml");
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

        public string GetMapID(string PRODUCT)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPID", null, new string[] { PRODUCT });
                return dt.Rows[0]["MAPID"].ToString(); ;
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

        public void CreateAVITable(string PRODUCT)
        {
            try
            {
                this.ExecuteScalar("CREATE_AVI_TABLE", new string[] { "T_AVI_" + PRODUCT }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetDeviceDef(string FACILITY, string PRODUCT, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID)
        {
            try
            {
                this.Execute("CREATE_PRODUCT", null, new string[] {FACILITY, PRODUCT, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PRODLIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectProductInfo(string mapid)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PRODUCT_INFO", null, new string[] { mapid });
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

        public DataTable SelectCustomerWithMapdef(string mapid)
        {
            try
            {
                return this.GetDataTable("SELECT_CUSTOMER_WITH_MAPDEF", null, new string[] { mapid });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectProductInfo01(string product)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PRODUCT_INFO_01", null, new string[] { product });
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

        public void UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT)
        {
            try
            {
                this.Execute("UPDATE_PRODUCT_INFO_01", null, new string[] { FACILITY, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID, PRODUCT });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT)
        {
            try
            {
                this.Execute("UPDATE_DELETE_FLAG", null, new string[] { DELETE_FLAG, PRODUCT });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 데이터가 존재하지 않는 경우 INSERT 합니다.
        /// </summary>
        public void MergeData(string factory, string product, string mapid)
        {
            ExecuteNonQuery("MERGE_DATA", null, new string[] { factory, product, mapid });
        }
    }
}
