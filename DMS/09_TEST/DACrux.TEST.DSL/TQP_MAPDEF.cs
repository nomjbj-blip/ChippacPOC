using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_MAPDEF : Miracom.Middleware.QueryComponent
    {
        public TQP_MAPDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_MAPDEF.xml");
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

        public void SetMapDef(
        string MAPID
        , double WAFER_SIZE
        , double CHIP_SIZE_X
        , double CHIP_SIZE_Y
        , double ORIGIN_MICRO_X
        , double ORIGIN_MICRO_Y
        , int ORIGIN_INDEX_X
        , int ORIGIN_INDEX_Y
        , double FIRST_MICRO_X
        , double FIRST_MICRO_Y
        , int FIRST_INDEX_X
        , int FIRST_INDEX_Y
        , double EDGE_SIZE
        , int ANGLE
        , int NETDIE
        , int NOTCH_TYPE
        , int ST_START
        , int ST_INTYPE
        , int ST_XCNT
        , int ST_YCNT
        , int ST_START_X
        , int ST_START_Y
        , int DIE_INDEX_MIN_X
        , int DIE_INDEX_MAX_X
        , int DIE_INDEX_MIN_Y
        , int DIE_INDEX_MAX_Y
        , int XY_DIRECTION
        , int REFERENCEDIE_SETTING)
        {
            try
            {
                this.Execute("CREATE_MAP", null, new string[] {MAPID.ToString()
																   , WAFER_SIZE.ToString()
																   , CHIP_SIZE_X.ToString()
																   , CHIP_SIZE_Y.ToString()
																   , ORIGIN_MICRO_X.ToString()
																   , ORIGIN_MICRO_Y.ToString()
																   , ORIGIN_INDEX_X.ToString()
																   , ORIGIN_INDEX_Y.ToString()
																   , FIRST_MICRO_X.ToString()
																   , FIRST_MICRO_Y.ToString()
																   , FIRST_INDEX_X.ToString()
																   , FIRST_INDEX_Y.ToString()
																   , EDGE_SIZE.ToString()
																   , ANGLE.ToString()
																   , NETDIE.ToString()
																   , NOTCH_TYPE.ToString()
																   , ST_START.ToString()
																   , ST_INTYPE.ToString()
																   , ST_XCNT.ToString()
																   , ST_YCNT.ToString()
																   , ST_START_X.ToString()
																   , ST_START_Y.ToString()
																   , DIE_INDEX_MIN_X.ToString()
																   , DIE_INDEX_MAX_X.ToString()
																   , DIE_INDEX_MIN_Y.ToString()
																   , DIE_INDEX_MAX_Y.ToString()
																   , XY_DIRECTION.ToString()
																   , REFERENCEDIE_SETTING.ToString()});

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDef(
            string MapID
            )
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDEF", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDefDie(
            string mapid
            )
        {
            return this.GetDataTable(
                "GET_MAPDEF_DIE",
                null,
                new string[] { mapid }
                );
        }

        public DataTable GetMapDefByLotID(string LotID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDEF_BY_LOTID", null, new string[] { LotID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetMapIDList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPIDLIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMapDef(
            string MAPID,
            double WAFER_SIZE,
            double CHIP_SIZE_X,
            double CHIP_SIZE_Y,
            double ORIGIN_MICRO_X,
            double ORIGIN_MICRO_Y,
            int ORIGIN_INDEX_X,
            int ORIGIN_INDEX_Y,
            double FIRST_MICRO_X,
            double FIRST_MICRO_Y,
            int FIRST_INDEX_X,
            int FIRST_INDEX_Y,
            double EDGE_SIZE,
            int ANGLE,
            int NETDIE,
            int NOTCH_TYPE,
            int ST_START,
            int ST_INTYPE,
            int ST_XCNT,
            int ST_YCNT,
            int ST_START_X,
            int ST_START_Y,
            int DIE_INDEX_MIN_X,
            int DIE_INDEX_MAX_X,
            int DIE_INDEX_MIN_Y,
            int DIE_INDEX_MAX_Y,
            int XY_DIRECTION)
        {
            try
            {
                this.Execute("UPDATE_MAP", null, new string[] {WAFER_SIZE.ToString(),
																CHIP_SIZE_X.ToString(),
																CHIP_SIZE_Y.ToString(), 
																ORIGIN_MICRO_X.ToString(), 
																ORIGIN_MICRO_Y.ToString(), 
																ORIGIN_INDEX_X.ToString(), 
																ORIGIN_INDEX_Y.ToString(), 
																FIRST_MICRO_X.ToString(), 
																FIRST_MICRO_Y.ToString(), 
																FIRST_INDEX_X.ToString(), 
																FIRST_INDEX_Y.ToString(), 
																EDGE_SIZE.ToString(), 
																ANGLE.ToString(), 
																NETDIE.ToString(), 
																NOTCH_TYPE.ToString(), 
																ST_START.ToString(), 
																ST_INTYPE.ToString(), 
																ST_XCNT.ToString(), 
																ST_YCNT.ToString(), 
																ST_START_X.ToString(), 
																ST_START_Y.ToString(), 
																DIE_INDEX_MIN_X.ToString(), 
																DIE_INDEX_MAX_X.ToString(), 
																DIE_INDEX_MIN_Y.ToString(), 
																DIE_INDEX_MAX_Y.ToString(),
															    XY_DIRECTION.ToString(),
																MAPID});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMapDef(string MapID, string UserID)
        {
            try
            {
                this.Execute("DELETE_MAP", null, new string[] { MapID, UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapDef_EDS(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDEF", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapIDList_EDS()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPIDLIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapIDAllList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDEF_ALL", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapIDListAll()
        {
            return GetDataTable("GET_MAPIDLIST_ALL", null, null);
        }

        public DataTable GetMapIDListNotDelete()
        {
            return GetDataTable("GET_MAPIDLIST_USE", null, null);
        }

         public DataTable GetMapData(string strDevice)
        {
            return GetDataTable("SELECT_MAP_DATA", null, new string[] { strDevice });
        }
        

        public bool ExistsMapID(string factory, string mapID)
        {
            object val = ExecuteScalar("EXISTS_MAPID", null, new string[] { factory, mapID });

            if (val == null)
                return false;

            return Int32.Parse(val.ToString()) > 0;
        }

        public void InsertMapData(Dictionary<string, string> dic, string userID)
        {
            ExecuteNonQuery("INSERT_USER_MAP", null,
                new string[] 
                {
                    dic["FACTORY"],
                    dic["MAPID"],
                    dic["WAFER_SIZE"],
                    dic["CHIP_SIZE_X"],
                    dic["CHIP_SIZE_Y"],
                    dic["ORIGIN_MICRO_X"],
                    dic["ORIGIN_MICRO_Y"],
                    dic["ORIGIN_INDEX_X"],
                    dic["ORIGIN_INDEX_Y"],
                    dic["EDGE_SIZE"],
                    dic["DIE_INDEX_MIN_X"],
                    dic["DIE_INDEX_MAX_X"],
                    dic["DIE_INDEX_MIN_Y"],
                    dic["DIE_INDEX_MAX_Y"],
                    userID
                });
        }

        public void UpdateMapData(Dictionary<string, string> dic, string userID)
        {
            ExecuteNonQuery("UPDATE_USER_MAP", null,
                new string[] 
                {
                    dic["FACTORY"],
                    dic["MAPID"],
                    dic["WAFER_SIZE"],
                    dic["CHIP_SIZE_X"],
                    dic["CHIP_SIZE_Y"],
                    dic["ORIGIN_MICRO_X"],
                    dic["ORIGIN_MICRO_Y"],
                    dic["ORIGIN_INDEX_X"],
                    dic["ORIGIN_INDEX_Y"],
                    dic["EDGE_SIZE"],
                    dic["DIE_INDEX_MIN_X"],
                    dic["DIE_INDEX_MAX_X"],
                    dic["DIE_INDEX_MIN_Y"],
                    dic["DIE_INDEX_MAX_Y"],
                    userID
                });
        }

        public void DeleteMapData(string factory, string mapID)
        {
            ExecuteNonQuery("DELETE_USER_MAP", null, new string[] { factory, mapID });
        }

        #region Map Parsing 사용 ---------------------------------------------------

        public DataTable SelectNetDie(string MapId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_NETDIE", null, new string[] { MapId });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        #endregion------------------------------------------------------------
    }
}
