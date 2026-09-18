using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;
using System.IO;

namespace DACrux.TEST.DSL
{
    public class TQP_USEMAP : Miracom.Middleware.QueryComponent
    {
        public TQP_USEMAP()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_USEMAP.xml");
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

        public DataTable GetDies(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_USEMAP_ALLDIE", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDies(string MapID, bool isUseDie)
        {
            if (!isUseDie) return GetDies(MapID);

            DataTable dt = null;
            try
            {
                return dt = this.GetDataTable("SELECT_USEMAP_USEDIE", null, new string[] { MapID });
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        
        public DataTable GetInDies(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_USEMAP_INDIE", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void CreateUseMap(string MapID, DataTable dtDies)
        {
            DataRow[] drs = dtDies.Select();
            string[,] strDatas = new string[drs.Length, 4];
            try
            {
                /// 기존의 Data를 지운다.
                DeleteUseMap(MapID);

                /// 새로운 Data를 넣는다.
                for (int i = 0; i < drs.Length; i++)
                {
                    strDatas[i, 0] = MapID;
                    strDatas[i, 1] = drs[i]["X"].ToString();
                    strDatas[i, 2] = drs[i]["Y"].ToString();
                    strDatas[i, 3] = drs[i]["USECODE"].ToString();
                }

                this.ExecuteMultiple("CREATE_USEMAP", strDatas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                strDatas = null;
            }
        }

        public void CreateUseMap(string strFactory, string MapID, DataTable dtDies)
        {
            DataRow[] drs = dtDies.Select();
            string[,] strDatas = new string[drs.Length, 6];
            try
            {
                /// 기존의 Data를 지운다.
                DeleteUseMap(MapID);

                /// 새로운 Data를 넣는다.
                for (int i = 0; i < drs.Length; i++)
                {
                    strDatas[i, 0] = strFactory;
                    strDatas[i, 1] = MapID;
                    strDatas[i, 2] = drs[i]["X"].ToString();
                    strDatas[i, 3] = drs[i]["Y"].ToString();
                    strDatas[i, 4] = drs[i]["USECODE"].ToString();
                    strDatas[i, 5] = (i+1).ToString();
                }

                this.ExecuteMultiple("CREATE_USEMAP_01", strDatas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                strDatas = null;
            }
        }

        public void DeleteUseMap(string MapID)
        {
            try
            {
                this.Execute("DELETE_USEMAP", null, new string[] { MapID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateShot(string MapID, ref DataTable dtDies)
        {
            DataRow[] drs = dtDies.Select();
            string[,] strDatas = new string[drs.Length, 8];
            try
            {
                /// 새로운 Data를 넣는다.
                for (int i = 0; i < drs.Length; i++)
                {
                    strDatas[i, 0] = drs[i]["SHOTID"].ToString(); ;
                    strDatas[i, 1] = drs[i]["SX"].ToString();
                    strDatas[i, 2] = drs[i]["SY"].ToString();
                    strDatas[i, 3] = drs[i]["DIESX"].ToString();
                    strDatas[i, 4] = drs[i]["DIESY"].ToString();
                    strDatas[i, 5] = MapID;
                    strDatas[i, 6] = drs[i]["X"].ToString();
                    strDatas[i, 7] = drs[i]["Y"].ToString();
                }

                this.ExecuteMultiple("UPDATE_SHOT", strDatas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                strDatas = null;
            }
        }

        public DataTable SelectShotOrigin(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_SHOT_ORIGIN", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
