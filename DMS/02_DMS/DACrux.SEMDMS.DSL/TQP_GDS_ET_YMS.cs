using System;
using System.Configuration;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    /// <summary>
    /// TQP_GDS_ET_YMS의 Oracle XML 쿼리를 호출하는 DB 접근 클래스다. (2026-10-14)
    /// E-Test 결과를 YMS 테이블에 넣을 때 기존 YMS 쿼리로는 할 수 없는 TQP_PARASPEC 보정(설명 / 단위 / LSL / USL / 형식)과
    /// E-Test 파일 -> YMS 반영용 읽기 / 반영 상태 기록을 담당한다. YMS 적재 / 조회 / 삭제는 TEST BSL / DSL 의 기존 함수와 쿼리를 쓴다.
    /// </summary>
    public class TQP_GDS_ET_YMS : Miracom.Middleware.QueryComponent
    {
        public TQP_GDS_ET_YMS()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_GDS_ET_YMS.xml");
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

        public void UpdateExecuteMultiple(string sqlName, string[,] paras)
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
    }
}
