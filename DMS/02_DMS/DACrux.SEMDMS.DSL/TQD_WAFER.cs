using System;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    /// <Summary>
    /// 
    /// <b>■KLARF File Handling을 위한 Process Class</b><br>
    ///  
    /// - 작  성  자 : 미라콤 임영신<br>
    /// - 최초작성일 : 2004년 07월 07일<br>
    /// - 최종수정자 : 임영신<br>
    /// - 최종수정일 : 2004년 07월 07일<br>
    /// - 주요변경로그<br>
    /// 2004.07.07 생성<br>
    /// </Summary>
    /// <Remarks>없음</Remarks>

    public class TQD_WAFER : Miracom.Middleware.QueryComponent
    {
        public TQD_WAFER()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_WAFER.xml");
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

        public void Create(
            string[] WaferInfo
            )
        {
            try
            {
                //업무 로직 구현
                this.Execute("CREATE_WAFER", null, WaferInfo);

            }
            catch (Exception ex)
            {
                //예외처리
                throw this.ProcessErr(ex);
            }
        }

        //--

        public long GetWaferSeq(
            string[] WaferInfo
            )
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_WAFER_SEQ", null, new string[2] { WaferInfo[0], WaferInfo[1] });
                if (dt.Rows.Count == 0) return -1;
                return long.Parse(dt.Rows[0]["WAFER_SEQ"].ToString());
            }
            catch (Exception ex)
            {
                //예외처리
                throw this.ProcessErr(ex);
            }

        }

        //--

        public DataTable GetWaferInfo(
            string factory,
            string lotid
            )
        {
            return this.GetDataTable(
                "SELECT_WAFER_INFO_02",
                null,
                new string[] { lotid }
                );
        }

        public DataTable GetWaferSeq(string strLotSeq, string strWaferID)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_SEQ", null, new string[] { strLotSeq, strWaferID });
            }
            catch (Exception ex)
            {
                //예외처리
                throw this.ProcessErr(ex);
            }

        }

        public void UpdateWaferMaint(string strLotSeq, string strLotID, string strStepSeq)
        {
            this.Execute("UPDATE_MAINT_DATA", new string[] { strStepSeq }, new string[] { strLotSeq, strLotID });
        }
    }
}
