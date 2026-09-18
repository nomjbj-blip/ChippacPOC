using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_CONFIG_USER : Miracom.Middleware.QueryComponent
    {
        public TQD_CONFIG_USER(
            )
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (String.IsNullOrEmpty(connectID))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_CONFIG_USER.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TQD_CONFIG_USER(
            string sConnectName,
            string sQueryID
            )
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings[sConnectName];
                if (String.IsNullOrEmpty(connectID))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                if (!sQueryID.Contains(".xml"))
                    sQueryID = String.Format("{0}.{1}", sQueryID, "xml");

                this.InitQueryComponent(connectID, sQueryID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Common Select]
        public DataTable GetData(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                return this.GetDataTable(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                this.GetDataTable(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                return this.ExecuteNonQuery(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(
            string sqlName,
            string[,] paras
            )
        {
            try
            {
                this.ExecuteMultiple(
                    sqlName,
                    null,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                this.GetDataTable(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                return this.ExecuteNonQuery(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                this.GetDataTable(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(
            string sqlName,
            string[] dynamic,
            string[] paras
            )
        {
            try
            {
                return this.ExecuteNonQuery(
                    sqlName,
                    dynamic,
                    paras
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Query ]

        public int InsertTqdConfigUser01(
            string[,] parameters
            )
        {
            return this.ExecuteMultipleEx(
                "INSERT_CONFIG_USER_01",
                null,
                parameters
                );
        }

        public int InsertTqdConfigUser01(
            string factory,
            string category,
            string name,
            string value,
            string type,
            string userid,
            string ordered,
            string comment
            )
        {
            return this.ExecuteNonQuery(
                "INSERT_CONFIG_USER_01",
                null,
                new string[] { factory, category, name, value, type, userid, ordered, userid, comment }
                );
        }

        public DataTable SelectTqdConfigUser01(
            string factory,
            string category,
            string userid
            )
        {
            return this.GetDataTable(
                "SELECT_CONFIG_USER_01",
                null,
                new string[] { factory, category, userid }
                );
        }

        public DataTable SelectTqdConfigUser02(
            string factory,
            string category,
            string userid
            )
        {
            return this.GetDataTable(
                "SELECT_CONFIG_USER_02",
                null,
                new string[] { factory, category, userid }
                );
        }

        public string SelectTqdConfigUser(
            string factory,
            string category,
            string name,
            string userid
            )
        {
            object obj = ExecuteScalar("SELECT_CONFIG_USER_04", null, new string[] { factory, category, name, userid });

            if (obj == null || obj == DBNull.Value)
                return null;

            return obj.ToString();
        }

        public int DelectTqdconfigUser01(
            string factory,
            string[] category,
            string userid
            )
        {
            return this.ExecuteNonQuery(
                "DELETE_CONFIG_USER_01",
                new string[] { String.Format("'{0}'", String.Join("','", category)) },
                new string[] { factory, userid }
                );
        }
        #endregion [ Query ]
    }
}
