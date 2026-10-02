using System;
using System.Configuration;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    /// <summary>
    /// TQP_GDS_PLACED_EL의 Oracle XML 쿼리를 호출하는 DB 접근 클래스다.
    /// GDS 화면 배치 도형(POINTS_BIN BLOB) 저장 / 조회를 담당한다. 작은 도형은 다건, 큰 도형은 한 건씩 저장한다.
    /// </summary>
    public class TQP_GDS_PLACED_EL : Miracom.Middleware.QueryComponent
    {
        public TQP_GDS_PLACED_EL()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_GDS_PLACED_EL.xml");
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

        /// <summary>
        /// BLOB(byte[])이 들어가는 한 건을 object 파라미터로 실행한다.
        /// 기존 InsertDataNonQuery / InsertExecuteMultiple은 string만 받으므로 2,000바이트를 넘는 BLOB은 이 경로만 가능하다.
        /// 이 테이블 XML은 logging="FALSE"여야 한다(Middleware SQL 로그가 byte[]를 string으로 바꾸다 실패).
        /// </summary>
        public void InsertDataObject(string sqlName, object[] paras)
        {
            try
            {
                this.Execute1(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
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

        #region [Common Update]
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
    }
}
