using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_PROGRAM : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQP_PROGRAM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_PROGRAM.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

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

        #region OLD
        public DataTable GetTestAreaList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_AREALIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestAreaListNotPCM()
        {
            return this.GetDataTable("GET_AREALIST_NOT_PCM", null, null);
        }

        public DataTable GetTestProgramList(
            string testarea,
            string device
            )
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PROGRAMLIST", null, new string[] { testarea, device });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestProgramDuplication(
            string strFactory,
            string strProgram,
            string strTestarea,
            string strDevice
           )
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PGM_DUPLE", null, new string[] { strFactory, strProgram, strTestarea, strDevice });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateProgram(
            string strFactory,
            string strProgram,
            string strTestarea,
            string strDevice,
            string strUser
            )
        {
            return this.ExecuteNonQuery("INSERT_PROGRAM", null, new string[] { strFactory, strProgram, strTestarea, strDevice, strUser });
        }

        public void CreateProgram(
            string PRODUCT,
            string PROGRAM,
            string TESTAREA,
            double TARGET_YIELD,
            string VERSION,
            string ParaField)
        {
            if (VERSION.Length == 0) VERSION = "V1.0";
            //object oResult = null;

            try
            {
                this.Execute("CREATE_PROGRAM", null, new string[] { PROGRAM, VERSION, PRODUCT, TESTAREA, TARGET_YIELD.ToString() });

                if (TESTAREA == "PCM" || TESTAREA == "PARAMETRIC" || TESTAREA == "EPM" || TESTAREA == "DCPARA" || TESTAREA == "INLINE")
                {
                    this.ExecuteScalar("CREATE_PROGRAM_TABLE", new string[] { "T_PCM_" + PROGRAM.Replace("-", "_"), ParaField }, null);
                }
                else
                {
                    PROGRAM = PROGRAM.ToUpper().Replace("-", "_").Replace(".", "_").Replace(" ", "");

                    // Create Table ========================================================================
                    try
                    {
                        this.ExecuteScalar("CREATE_PROGRAM_TABLE", new string[] { "TD_" + PROGRAM, ParaField }, null);
                    }
                    catch (Exception) { }
                    //try
                    //{
                    //    this.ExecuteScalar("CREATE_INCUST_AOI_TABLE", new string[] { "T_INCUST_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }
                    //try
                    //{
                    //    this.ExecuteScalar("CREATE_INCUST_AOI_TABLE", new string[] { "T_AOI_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }
                    //try
                    //{
                    //    this.ExecuteScalar("CREATE_PNP_TABLE", new string[] { "T_PNP_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }

                    this.ExecuteScalar("CREATE_PROGRAM_TABLE_AVI", new string[] { "T_AVI_" + PROGRAM.Replace("-", "_") }, null);

                    // Create Index ==========================================================================
                    try
                    {
                        this.Execute("CREATE_IDX01", new string[] { "TD_" + PROGRAM + "_", "TD_" + PROGRAM }, null);
                    }
                    catch (Exception) { }
                    //try
                    //{
                    //    this.Execute("CREATE_IDX01", new string[] { "T_INCUST_" + PROGRAM + "_", "T_INCUST_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }
                    //try
                    //{
                    //    this.Execute("CREATE_IDX01", new string[] { "T_AOI_" + PROGRAM + "_", "T_AOI_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }
                    //try
                    //{
                    //    this.Execute("CREATE_IDX01", new string[] { "T_PNP_" + PROGRAM + "_", "T_PNP_" + PROGRAM }, null);
                    //}
                    //catch (Exception) { }

                    this.Execute("CREATE_AVI_IDX01", new string[] { "T_AVI_" + PROGRAM.Replace("-", "_") + "_", PROGRAM.Replace("-", "_") }, null);
                }

                //if (SiteCnt.Length == 0) SiteCnt = "null";
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("ORA-00955"))
                {
                    throw ex;
                }
            }
        }

        public void CreateProgram(
            string PRODUCT,
            string PROGRAM,
            string TESTAREA,
            double TARGET_YIELD,
            string VERSION,
            string ParaField,
            string SiteCnt
                )
        {
            if (VERSION.Length == 0) VERSION = "V1.0";
            object oResult = null;
            try
            {
                if (TESTAREA == "PROBE" || TESTAREA == "MULTIPROBE" || TESTAREA == "PT1" || TESTAREA == "PT2" || TESTAREA == "PT3"
                    || TESTAREA == "PRE" || TESTAREA == "POST" || TESTAREA == "CP")
                {
                    oResult = this.ExecuteScalar("CREATE_PROGRAM_TABLE", new string[] { "TD_" + PROGRAM.Replace("-", "_"), ParaField }, null);
                    this.Execute("CREATE_IDX01", new string[] { "PRB_" + PROGRAM.Replace("-", "_") + "_", "TD_" + PROGRAM.Replace("-", "_") }, null);
                    this.Execute("CREATE_IDX02", new string[] { "PRB_" + PROGRAM.Replace("-", "_") + "_", "TD_" + PROGRAM.Replace("-", "_") }, null);

                }
                else if (TESTAREA == "PCM" || TESTAREA == "PARAMETRIC" || TESTAREA == "EPM" || TESTAREA == "DCPARA" || TESTAREA == "INLINE")
                {
                    oResult = this.ExecuteScalar("CREATE_PROGRAM_TABLE", new string[] { "T_PCM_" + PROGRAM.Replace("-", "_"), ParaField }, null);
                }

                if (SiteCnt.Length == 0) SiteCnt = "null";

                this.Execute("CREATE_PROGRAM", null, new string[] { PROGRAM, VERSION, PRODUCT, TESTAREA, TARGET_YIELD.ToString(), SiteCnt });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetProgramInfo(string program)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PROGRAMINFO", null, new string[] { program });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProgramInfoByTestArea(string program, string testArea)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PROGRAMINFO_BY_TESTAREA", null, new string[] { program, testArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramInfo(string PROGRAM, string TESTAREA, string PRODUCT, double TARGET_YIELD, string VERSION)
        {
            if (VERSION.Length == 0) VERSION = "V1.0";
            try
            {
                this.Execute("UPDATE_PROGRAM", null, new string[] {TESTAREA,
																	   PRODUCT,
																	   TARGET_YIELD.ToString(),
																	   PROGRAM,
																	    VERSION
																   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramInfoDMS(string PROGRAM, string TESTAREA, string PRODUCT, double TARGET_YIELD, string VERSION, bool bDefaultDMS)
        {
            if (VERSION.Length == 0) VERSION = "V1.0";
            try
            {
                this.Execute("UPDATE_PROGRAM_DMS", null, new string[] {TESTAREA,
																	   PRODUCT,
																	   TARGET_YIELD.ToString(),
																	   PROGRAM,
																		VERSION,
                                                                        bDefaultDMS ? "Y":"N"
																   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProgramInfo(string PROGRAM)
        {
            try
            {
                this.Execute("DELETE_PROGRAM", null, new string[] { PROGRAM });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProgramInfoByTestArea(string PROGRAM, string TESTAREA)
        {
            try
            {
                this.Execute("DELETE_PROGRAM_BY_TESTAREA", null, new string[] { PROGRAM, TESTAREA });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUseFlag(string PROGRAM, string USE_FLAG)
        {
            //UPDATE_USEFLAG
            try
            {
                this.Execute("UPDATE_USEFLAG", null, new string[] { USE_FLAG, PROGRAM });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVIFailDie(string PROGRAM, string[] strValue)
        {
            try
            {
                this.Execute("UPDATE_VIFAIL", new string[] { string.Format("T_PRB_{0}", PROGRAM.Replace(".", "_").Replace("-", "_")) }, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetMapData(string PROGRAM, string PARA, string WaferSeq)
        {
            DataTable dt = null;
            try
            {
                //dt = this.GetDataTable("GET_PCM_MAPDATA", new string[] { utilParametric.REP(PARA), utilParametric.REP(PROGRAM) }, new string[] { WaferSeq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapData(string TableName, string[] WaferSeq, string[] BINS)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDATA_MULTI", new string[] {TableName
																		,string.Join(",",WaferSeq)
																		,string.Join(",",BINS)
																		,TableName
																		,WaferSeq[0]}, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFailBinMapData(string TableName, string[] WaferSeq, string[] BINS)
        {
            DataTable dt = null;
            try
            {

                //dt = this.GetDataTable("GET_FAIL_MAPDATA_MULTI", new string[] {PROGRAM.Replace("-","_").Replace(".","_")
                //                                                        ,string.Join(",",WaferSeq)
                //                                                        ,PROGRAM.Replace("-","_").Replace(".","_")
                //                                                        ,WaferSeq[0]}, null);

                dt = this.GetDataTable("GET_FAIL_MAPDATA_MULTI", new string[] {TableName
																		    ,string.Join(",",WaferSeq)
																		    ,TableName
																		    ,WaferSeq[0]}, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCumMapData(string PROGRAM, string[] WaferSeq, string[] BINS)
        {
            DataTable dt = null;
            try
            {

                dt = this.GetDataTable("GET_CUM_MAPDATA_MULTI", new string[] {PROGRAM.Replace("-","_").Replace(".","_")
																		,string.Join(",",WaferSeq)
																		,string.Join(",",BINS)}, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetParaList(string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                //dt = this.GetDataTable("GET_PARA_LIST", null, new string[] { utilParametric.REP(PROGRAM) });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductList(string TESTAREA)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PRODUCT_LIST", null, new string[] { TESTAREA });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProgramList(string TESTAREA, string PRODUCT)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_PROGRAM_LIST", null, new string[] { TESTAREA, PRODUCT });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistribution(string PROGRAM, string strTableName, string WaferSeq, string TestArea)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION", new string[] { strTableName }, new string[] { PROGRAM, WaferSeq, TestArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistributionByAll(string Product, string TestArea, string strTableName, string WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_BY_ALL", new string[] { strTableName }, new string[] { Product, TestArea, WaferSeq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistributionByAllBoth(string Dynamic_Query, string TableName, string[] WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_BY_ALL_BOTH", new string[] { Dynamic_Query, TableName, string.Join(",", WaferSeq) }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistributionForProbeVI(string Dynamic_Query)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_FOR_PROBE_VI", new string[] { Dynamic_Query }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistributionMultiWafer(string PROGRAM, string strTableName, string[] WaferSeq, string TestArea)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_MULTI_WAFER"
                    , new string[] { strTableName, string.Join(",", WaferSeq) }, new string[] { PROGRAM, TestArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistributionMultiWaferByAll(string Product, string TestArea, string strTableName, string[] WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_MULTI_WAFER_BY_ALL"
                    , new string[] { strTableName, string.Join(",", WaferSeq) }, new string[] { Product, TestArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-Use
        public DataTable GetMapData(string PROGRAM, long WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDATA", new string[] { PROGRAM.Replace("-", "_").Replace(".", "_") }, new string[] { WaferSeq.ToString() });
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

        public DataTable GetMapData(string program, long WaferSeq, string paraItem, int cutCnt)
        {
            return GetMapData(program, WaferSeq.ToString(), paraItem, cutCnt);
        }

        public DataTable GetMapData(string program, string WaferSeq, string paraItem, int cutCnt)
        {
            DataTable dt = null;
            try
            {
                string[] dynamic = new string[] {
													 paraItem
													 , paraItem
													 , paraItem
													 , program
													 , paraItem
													 , paraItem
													 , program
													 , paraItem
													 , paraItem
												 };
                string[] para = new string[] { WaferSeq, cutCnt.ToString(), program };

                dt = this.GetDataTable("GET_PARA_MAP_DATA", dynamic, para);
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

        public DataTable GetMapDataCnt(string program, long WaferSeq, string paraItem, int cutCnt)
        {
            return GetMapDataCnt(program, WaferSeq.ToString(), paraItem, cutCnt);
        }
        public DataTable GetMapDataCnt(string program, string WaferSeq, string paraItem, int cutCnt)
        {
            DataTable dt = null;
            try
            {
                string[] dynamic = new string[] {
													 paraItem
													 , paraItem
													 , paraItem
													 , program
													 , paraItem
													 , paraItem
													 , program
													 , paraItem
													 , paraItem
												 };
                string[] para = new string[] { WaferSeq, cutCnt.ToString(), program };

                dt = this.GetDataTable("GET_PARA_MAP_CNT", dynamic, para);
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

        public DataTable GetMapDataForDefectOverlay(string PROGRAM, string WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DEFECT_OVERLAY_MAPDATA", new string[] { PROGRAM.Replace("-", "_").Replace(".", "_") }, new string[] { WaferSeq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetBinDistribution(string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DESC", null, new string[] { PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestAreaList_Program()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_TESTAREA_LIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDefaultProgramList()
        {
            try
            {
                return this.GetDataTable("GET_DEFAULT_PROGAM_LIST", null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteWaferRawData(string TESTAREA, string Program, string WaferSeq)
        {
            string strTableName = string.Empty;
            try
            {
                switch (TESTAREA)
                {
                    case "AVI":
                        strTableName = string.Format("T_AVI_{0}", Program);
                        break;
                    case "MULTIPROBE":
                    case "AREA":
                    case "PT1":
                    case "PT2":
                        strTableName = string.Format("T_PRB_{0}", Program);
                        break;
                    case "PCM":
                    case "INLINE":
                    case "PARAMETRIC":
                        strTableName = string.Format("T_PCM_{0}", Program);
                        break;
                }
                this.Execute("DELETE_RAW_DATA", new string[] { strTableName }, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// add date: 2019.05.30
        /// UI: Setup by Parameter Spec 
        /// Testarea 별, Device에 대한 Program Infor
        /// </summary>
        /// <param name="testarea">String[]</param>
        /// <param name="device">String[]</param>
        /// <returns></returns>
        public DataTable GetTestProgramList(
            string[] testarea,
            string[] device
            )
        {
            StringBuilder sbQuery = new StringBuilder();
            if (testarea != null && testarea.Length > 0)
                sbQuery.AppendLine(String.Format("AND TESTAREA IN ('{0}')", string.Join("','", testarea)));

            if (device != null && device.Length > 0)
                sbQuery.AppendLine(String.Format("AND DEVICE IN ('{0}')", string.Join("','", device)));

            return this.GetDataTable(
                "GET_PROGRAMLIST_MULTI",
                new string[] { sbQuery.ToString() },
                null
                );
        }

        /// <summary>
        /// NEW=======================================================================
        /// </summary>
        /// <param name="strProgram"></param>
        /// <param name="WaferSeq"></param>
        /// <returns></returns>
        /// 

        public DataTable GetBinDistribution(string strProgram, long WaferSeq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION", new string[] { strProgram.Replace("-", "_").Replace(".", "_") }, new string[] { strProgram, WaferSeq.ToString() });
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

        public DataTable GetBinDistribution(string strProgram, long[] WaferSeqs)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BIN_DISTRIBUTION_MULTI_WAFER", new string[] {strProgram.Replace("-","_").Replace(".","_")
																							,string.Join(",",WaferSeqs)}, new string[] { strProgram });
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

        public DataTable GetMapData(string Program, long[] WaferSeq, string[] SelBin)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDATA_MULTI", new string[] {Program
																		,string.Join(",",WaferSeq)
																		,string.Join(",",SelBin)
																		,Program
																		,WaferSeq[0].ToString()}, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTableExist(string program)
        {
            DataTable dt = null;
            try
            {
                string sMap = @"TD_" + program.Replace("-", "_").ToString();
                dt = this.GetDataTable("TABLE_EXIST", null, new string[] { sMap });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 데이터가 존재하지 않는 경우 INSERT 합니다.
        /// </summary>
        public void MergeData(string factory, string program, string testarea, string device)
        {
            ExecuteNonQuery("MERGE_DATA", null, new string[] { factory, program, testarea, device });
        }

        public void MapDataIndexUpdate(string strTableName, string[,] strData)
        {
            this.ExecuteMultiple("UPDATE_DATA_TABLE_INDEX", new string[] { strTableName }, strData);
        }

        /// <summary>
        /// TESTAREA를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public DataTable GetConditionTestArea()
        {
            return GetDataTable("SELECT_TESTAREA", null, null);
        }

        public DataTable GetProgramNotExistBin(string TestArea, string program)
        {
            return GetDataTable("SELECT_PROGRAM_NOT_EXIST_BIN", null, new string[] { TestArea, program });
        }

    }
}
