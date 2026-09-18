using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;
using System.Drawing;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_COLORBYSIZE : Miracom.Middleware.QueryComponent
    {
        public TQD_COLORBYSIZE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_COLORBYSIZE.xml");
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

        public DataTable SelectColorListByDefectSize(string userId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_COLOR_BY_SIZE", null, new string[] { userId });

                //User 정보 기준으로 Data 가 없을 경우  Default User 기준으로 색상을 가져 온다.
                if (dt == null || dt.Rows.Count <= 0)
                    dt = this.GetDataTable("SELECT_COLOR_BY_SIZE", null, new string[] { "admin" });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        public DataTable SelectColorListByDefectSize01(string userId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_COLOR_BY_SIZE_01", null, new string[] { userId });

                //User 정보 기준으로 Data 가 없을 경우  Default User 기준으로 색상을 가져 온다.
                if (dt == null || dt.Rows.Count <= 0)
                    dt = this.GetDataTable("SELECT_COLOR_BY_SIZE_01", null, new string[] { "ADMIN" });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        public void InsertColorByDefectSize(string user, int[] sizeSeq, int[] sizeFrom, int[] sizeTo, System.Drawing.Color[] color)
        {
            try
            {
                string[,] param = new string[sizeSeq.Length, 5];
                for (int i = 0; i < sizeSeq.Length; i++)
                {
                    param[i, 0] = user;
                    param[i, 1] = string.Format("{0}", sizeSeq[i]);
                    param[i, 2] = string.Format("{0}", sizeFrom[i]);
                    param[i, 3] = string.Format("{0}", sizeTo[i]);
                    param[i, 4] = ColorTranslator.ToHtml(color[i]);
                }
                this.ExecuteMultiple("INSERT_COLOR_BY_SIZE", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteColorByDefectSize(string userId)
        {
            try
            {
                this.Execute("DELETE_COLOR_BY_SIZE", null, new string[] { userId });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public object ExistsDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            return ExecuteScalar(
                "EXISTS_DEFECT_SIZE_BY_COLOR",
                null,
                new string[] { userid, sizeSeq }
                );

        }

        public int InsertDefectSizeByColor(
            string userid,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            return this.ExecuteNonQuery(
                "INSERT_DEFECT_SIZE_BY_COLOR",
                null,
                new string[] { userid, sizeFrom, sizeTo, color }
                );
        }

        public int InsertDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            return this.ExecuteNonQuery(
                "INSERT_COLOR_BY_SIZE",
                null,
                new string[] { userid, sizeSeq, sizeFrom, sizeTo, color }
                );
        }

        public int UpdateDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            return this.ExecuteNonQuery(
                "UPDATE_DEFECT_SIZE_BY_COLOR",
                null,
                new string[] { userid, sizeSeq, sizeFrom, sizeTo, color }
                );
        }

        public int DeleteDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            return this.ExecuteNonQuery(
                "DELECT_DEFECT_SIZE_BY_COLOR",
                null,
                new string[] { userid, sizeSeq }
                );
        }

        public DataTable SelectColorByDefectSize(string userId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_SIZE_COLOR", null, new string[] { userId });

                //User 정보 기준으로 Data 가 없을 경우  Default User 기준으로 색상을 가져 온다.
                if (dt == null || dt.Rows.Count <= 0)
                    dt = this.GetDataTable("SELECT_COLOR_BY_SIZE", null, new string[] { "ADMIN" });

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }
    }
}
