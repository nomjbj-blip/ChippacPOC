/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : iProbeAdmin.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.01.14
--  Description     : DACrux/SPC EDC DataSource Setup UI 
--  History         : Created by YSIM at 2013.01.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 * 2015-04-21 : YSLEE
    1. SelectProductInfo(string product) : 신규생성, 해당 Product가 존재하는 지 유무 체크
    2. UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT) : 신규생성, 현재 존재하는 Product 데이터 업데이트
    3. UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT) : 신규생성, 현재 존재하는 Product 삭제(실제로 DELETE_FLAG = 'Y'만 변경
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.TEST.DSL;

namespace DACrux.TEST.BSL
{
    public class ProbeAdmin : Miracom.Middleware.BaseComponent, DACrux.TEST.Interface.iProbeAdmin
    {
        public DataTable GetCustomerList()
        {
            TQP_CUSTOMER oCustomer = new TQP_CUSTOMER();
            return oCustomer.GetCustomerList();
        }

        public void CreateCustomer(string CustomerID
            , string Customer
            , string CustomerDesc
            , bool isDataService
            , int UserCount
            , string ExpireDate
            , string FtpSite
            , string FtpUser
            , string FtpPassword
            , string FtpPath
            , string SendTime
            , string AVIFormat
            , string EDSFormat
            , string InklessFormat
            , string RawDataFormat
            , string CustCode
            , string SprName
            , string MapRcvDir
            , string BackupDir
            , string ErrorDir
            , string LogDir)
        {
            TQP_CUSTOMER oCustomer = new TQP_CUSTOMER();
            oCustomer.CreateCustomer(CustomerID
                                    , Customer
                                    , CustomerDesc
                                    , isDataService
                                    , UserCount
                                    , ExpireDate.ToString()
                                    , FtpSite
                                    , FtpUser
                                    , FtpPassword
                                    , FtpPath
                                    , SendTime
                                    , AVIFormat
                                    , EDSFormat
                                    , InklessFormat
                                    , RawDataFormat
                                    , CustCode
                                    , SprName
                                    , MapRcvDir
                                    , BackupDir
                                    , ErrorDir
                                    , LogDir
                                    );
        }

        public void DeleteCustomer(string CustomerID)
        {
            TQP_CUSTOMER oCustomer = new TQP_CUSTOMER();
            oCustomer.DeleteCustomer(CustomerID);
        }

        public void UpdateCustomer(
            string Customer
            , string CustomerDesc
            , bool isDataService
            , int UserCount
            , string ExpireDate
            , string FtpSite
            , string FtpUser
            , string FtpPassword
            , string FtpPath
            , string SendTime
            , string AVIFormat
            , string EDSFormat
            , string InklessFormat
            , string RawDataFormat
            , string CustCode
            , string SprName
            , string MapRcvDir
            , string BackupDir
            , string ErrorDir
            , string LogDir
            , string CustomerID)
        {
            TQP_CUSTOMER oCustomer = new TQP_CUSTOMER();
            oCustomer.UpdateCustomer(Customer
                                    , CustomerDesc
                                    , isDataService
                                    , UserCount
                                    , ExpireDate.ToString()
                                    , FtpSite
                                    , FtpUser
                                    , FtpPassword
                                    , FtpPath
                                    , SendTime
                                    , AVIFormat
                                    , EDSFormat
                                    , InklessFormat
                                    , RawDataFormat
                                    , CustCode
                                    , SprName
                                    , MapRcvDir
                                    , BackupDir
                                    , ErrorDir
                                    , LogDir
                                    , CustomerID);
        }

        public DataTable GetMapIDList()
        {
            TQP_MAPDEF oMapdef = new TQP_MAPDEF();
            return oMapdef.GetMapIDList();
        }

        public DataTable GetMapIDAllList()
        {
            TQP_MAPDEF oMapdef = new TQP_MAPDEF();
            return oMapdef.GetMapIDAllList();
        }

        #region Shot Map Setup 관련

        public DataTable GetMapIDListAll()
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            return obj.GetMapIDListAll();
        }

        public DataTable GetMapIDListNotDelete()
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            return obj.GetMapIDListNotDelete();
        }

        public bool ExistsMapID(string factory, string mapID)
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            return obj.ExistsMapID(factory, mapID);
        }

        public void InsertMapData(Dictionary<string, string> dictionary, string userID)
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            obj.InsertMapData(dictionary, userID);
        }

        public void UpdateMapData(Dictionary<string, string> dictionary, string userID)
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            obj.UpdateMapData(dictionary, userID);
        }

        public void DeleteMapData(string factory, string mapID)
        {
            TQP_MAPDEF obj = new TQP_MAPDEF();
            obj.DeleteMapData(factory, mapID);
        }

        #endregion

        public void CreateAVITable(string PRODUCT)
        {
            TQP_PRODUCT oProduct = new TQP_PRODUCT();
            oProduct.CreateAVITable(PRODUCT);
        }

        public void SetDeviceDef(string FACILITY, string PRODUCT, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID)
        {
            TQP_PRODUCT oProduct = new TQP_PRODUCT();
            oProduct.SetDeviceDef(FACILITY, PRODUCT, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID);
        }

        public DataTable GetMapDef(string MapID)
        {
            TQP_MAPDEF oMapdef = new TQP_MAPDEF();
            return oMapdef.GetMapDef(MapID);
        }

        public DataTable GetDies(string MapID)
        {
            TQP_USEMAP oMapdef = new TQP_USEMAP();
            return oMapdef.GetDies(MapID);
        }

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
            TQP_MAPDEF oMapDef = new TQP_MAPDEF();
            oMapDef.SetMapDef(MAPID
                                , WAFER_SIZE
                                , CHIP_SIZE_X
                                , CHIP_SIZE_Y
                                , ORIGIN_MICRO_X
                                , ORIGIN_MICRO_Y
                                , ORIGIN_INDEX_X
                                , ORIGIN_INDEX_Y
                                , FIRST_MICRO_X
                                , FIRST_MICRO_Y
                                , FIRST_INDEX_X
                                , FIRST_INDEX_Y
                                , EDGE_SIZE
                                , ANGLE
                                , NETDIE
                                , NOTCH_TYPE
                                , ST_START
                                , ST_INTYPE
                                , ST_XCNT
                                , ST_YCNT
                                , ST_START_X
                                , ST_START_Y
                                , DIE_INDEX_MIN_X
                                , DIE_INDEX_MAX_X
                                , DIE_INDEX_MIN_Y
                                , DIE_INDEX_MAX_Y
                                , XY_DIRECTION
                                , REFERENCEDIE_SETTING);
        }
        public void CreateUseMap(string MapID, DataTable dtDies)
        {
            TQP_USEMAP oMapDef = new TQP_USEMAP();
            oMapDef.CreateUseMap(MapID, dtDies);
        }

        public void CreateUseMap(string strFactory, string MapID, DataTable dtDies)
        {
            TQP_USEMAP oMapDef = new TQP_USEMAP();
            oMapDef.CreateUseMap(strFactory, MapID, dtDies);
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
            TQP_MAPDEF oMapDef = new TQP_MAPDEF();
            oMapDef.UpdateMapDef(MAPID,
                                WAFER_SIZE,
                                CHIP_SIZE_X,
                                CHIP_SIZE_Y,
                                ORIGIN_MICRO_X,
                                ORIGIN_MICRO_Y,
                                ORIGIN_INDEX_X,
                                ORIGIN_INDEX_Y,
                                FIRST_MICRO_X,
                                FIRST_MICRO_Y,
                                FIRST_INDEX_X,
                                FIRST_INDEX_Y,
                                EDGE_SIZE,
                                ANGLE,
                                NETDIE,
                                NOTCH_TYPE,
                                ST_START,
                                ST_INTYPE,
                                ST_XCNT,
                                ST_YCNT,
                                ST_START_X,
                                ST_START_Y,
                                DIE_INDEX_MIN_X,
                                DIE_INDEX_MAX_X,
                                DIE_INDEX_MIN_Y,
                                DIE_INDEX_MAX_Y,
                                XY_DIRECTION);
        }

        public void DeleteUseMap(string MapID)
        {
            TQP_USEMAP oDSL = new TQP_USEMAP();
            oDSL.DeleteUseMap(MapID);
        }

        public DataTable GetProductList()
        {
            TQP_PRODUCT oDSL = new TQP_PRODUCT();
            return oDSL.GetProductList();
        }

        public DataTable GetProductList(
            string dtStart,
            string dtEnd,
            string[] testarea
            )
        {
            TQP_LOT oDsl = new TQP_LOT();
            return oDsl.GetProductList(
                dtStart,
                dtEnd,
                testarea
                );
        }

        public DataTable GetTestAreaList()
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.GetTestAreaList();
        }

        public DataTable GetTestAreaListNotPCM()
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.GetTestAreaListNotPCM();
        }

        public DataTable GetTestProgramList(
            string testarea,
            string device
            )
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.GetTestProgramList(
                testarea,
                device
                );
        }

        public DataTable GetTestProgramDuplication(
            string strFactory,
            string strProgram,
            string strTestarea,
            string strDevice
            )
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.GetTestProgramDuplication(strFactory, strProgram, strTestarea, strDevice);
        }

        public object CreateProgram(
            string strFactory,
            string strProgram,
            string strTestarea,
            string strDevice,
            string strUser
        )
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.CreateProgram(strFactory, strProgram, strTestarea, strDevice, strUser);
        }

        public DataTable GetTestProgramList(
            string[] testarea,
            string[] device
            )
        {
            TQP_PROGRAM oDsl = new TQP_PROGRAM();
            return oDsl.GetTestProgramList(
                testarea,
                device
                );
        }

        public DataTable GetTestProgramList(
            string dtStart,
            string dtEnd,
            string[] testarea,
            string[] device
            )
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetProgramList(dtStart, dtEnd, testarea, device);
        }

        public string ColorString(int iBinNumber)
        {
            return utilColor.ColorString(iBinNumber);
        }

        public object CreateCopyBin(string factory, string testarea, string sourceProgram, string targetProgram, string userId)
        {
            TQP_PROGRAM oProgram = new TQP_PROGRAM();
            TQP_BINDESC obj = new TQP_BINDESC();

            string device = String.Empty;
            DataTable dt = oProgram.GetProgramInfoByTestArea(targetProgram, testarea);
            // targetprogram 이 신규 프로그램인지 체크
            if (dt == null || dt.Rows.Count <= 0)
            {
                // 기존 프로그램에 대한 정보를 가져온다.
                dt = oProgram.GetProgramInfoByTestArea(sourceProgram, testarea);
                if (dt == null || dt.Rows.Count <= 0)
                    return null;

                device = dt.Rows[0]["DEVICE"].ToString();
                oProgram.MergeData(factory, targetProgram, testarea, device);
            }
            return obj.CreateCopyBin(sourceProgram, targetProgram);
        }

        public String[] GetProgramNotExistBin(string TestArea, string TargetProgram)
        {
            List<String> items = new List<string>();

            string program = String.Empty;
            if (TargetProgram.Contains('_'))
                program = String.Format("{0}%", TargetProgram.Substring(0, TargetProgram.IndexOf('_')));
            else program = String.Format("{0}%", TargetProgram);
            TQP_PROGRAM obj = new TQP_PROGRAM();
            DataTable dt = obj.GetProgramNotExistBin(TestArea, program);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (String.Equals(TargetProgram, row["PROGRAM"].ToString()))
                        continue;

                    items.Add(row["PROGRAM"].ToString());
                }
            }
            return items.ToArray();
        }

        public DataTable GetProgramInfo(string Program)
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            return oDSL.GetProgramInfo(Program);
        }

        public void CreateParaSpec(string[,] ParaInfo)
        {
            TQP_PARASPEC oDSL = new TQP_PARASPEC();
            oDSL.CreateParaSpec(ParaInfo);
        }

        public void CreateParaSpec(
            string factory,
            string program,
            string userid,
            DataTable paramDt
            )
        {
            TQP_PARASPEC oDsl = null;
            DataTable dt = null;
            String[,] param = null;
            int iRev = -1;

            oDsl = new TQP_PARASPEC();
            param = new String[paramDt.Rows.Count, 13];

            dt = oDsl.SelectProgramRevMaxValue(
                factory,
                program
                );

            if (!int.TryParse(dt.Rows[0]["MAX_VALUE"].ToString(), out iRev))
                iRev = 0;
            iRev += 1;

            for (int rowidx = 0; rowidx < paramDt.Rows.Count; rowidx++)
            {
                DataRow row = paramDt.Rows[rowidx];
                param[rowidx, 0] = factory;
                param[rowidx, 1] = row["PROGRAM"].ToString();
                param[rowidx, 2] = iRev.ToString();
                param[rowidx, 3] = row["PARAM_INDEX"].ToString();
                param[rowidx, 4] = row["PARAM_NAME"].ToString();
                param[rowidx, 5] = row["PARAM_TYPE"].ToString();
                param[rowidx, 6] = row["PARAM_DESC"].ToString();
                param[rowidx, 7] = userid;
                param[rowidx, 8] = row["TABLE_NAME"].ToString();
                param[rowidx, 9] = row["DECIMAL_PLACES"].ToString();
                param[rowidx, 10] = row["RUNTIME_DEFINED"].ToString();
                param[rowidx, 11] = row["UOM"].ToString();
                param[rowidx, 12] = String.Equals(row["USE_FLAG"].ToString(), bool.TrueString) ? "Y" : "N";
            }

            oDsl.CreateParaSpec(param);
        }

        public void CreateBins(string[,] BinInf)
        {
            TQP_BINDESC oDSL = new TQP_BINDESC();
            oDSL.CreateBins(BinInf);
        }

        public void DeleteProgramInfo(string PROGRAM)
        {
            TQP_PROGRAM oDSL = new TQP_PROGRAM();
            oDSL.DeleteProgramInfo(PROGRAM);
        }

        public void DeleteProgramBin(string PROGRAM)
        {
            TQP_BINDESC oDSL = new TQP_BINDESC();
            oDSL.DeleteProgramBin(PROGRAM);
        }

        public void DeleteProgramParaSpec(string Program)
        {
            TQP_PARASPEC oDSL = new TQP_PARASPEC();
            oDSL.DeleteProgramParaSpec(Program);
        }

        public DataTable GetBinListEditable(string Program)
        {
            TQP_BINDESC oDSL = new TQP_BINDESC();
            return oDSL.GetBinListEditable(Program);
        }

        public DataTable GetParaSpecList(
            string factory,
            string program
            )
        {
            TQP_PARASPEC oDsl = new TQP_PARASPEC();
            return oDsl.GetData(
                "GET_ITEM",
                null,
                new string[] { factory, program }
                );
        }

        public DataTable GetParaSpecList(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String testarea,
            String product,
            String program,
            String[] lots
            )
        {
            DataSet ds = null;
            DataTable dt = null;
            DataTable dtParam = null;
            DataRow[] drs = null;
            TQP_WAFER oWafer = null;
            TQP_PARASPEC oParaSpec = null;
            TD_TABLE oTable = null;

            Dictionary<string, string> dicAlias = null;
            List<string> lstTmp = null;
            String[] wafers = null;

            //--

            oWafer = new TQP_WAFER();
            oParaSpec = new TQP_PARASPEC();
            oTable = new TD_TABLE();
            ds = new DataSet();
            dicAlias = new Dictionary<string, string>();
            lstTmp = new List<string>();


            //--

            dt = oWafer.GetWaferInfo07(dtStart.ToString("yyyyMMdd"), dtEnd.ToString("yyyyMMdd"), new string[] { testarea }, product, program, lots);
            dt.TableName = "WAFER_INFO";
            ds.Tables.Add(dt);

            if (dt == null || dt.Rows.Count <= 0)
                return null;

            dt = dt.Select("[PROBE_CNT] = '0'").CopyToDataTable<DataRow>();
            wafers = new String[dt.Rows.Count];
            for (int ir = 0; ir < dt.Rows.Count; ir++)
            {
                wafers[ir] = dt.Rows[ir]["WAFER_SEQ"].ToString();
            }

            //--

            dt = dt.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
            foreach (DataRow r in dt.Rows)
            {
                dt = oParaSpec.GetData(
                    "SELECT_PARA_DATA",
                    new string[] { string.Format("'{0}'", string.Join("','", r["PROGRAM_REV"].ToString())) },
                    new string[] { factory, r["PROGRAM"].ToString() }
                    );

                if (dtParam == null)
                    dtParam = dt.Clone();
                dtParam.Merge(dt);
            }
            dtParam.AcceptChanges();
            dtParam.TableName = "PARAM_INFO";
            ds.Tables.Add(dtParam);

            //--

            dtParam = dtParam.DefaultView.ToTable(true, "TABLE_NAME");
            foreach (DataRow r in dtParam.Rows)
            {
                if (string.IsNullOrEmpty(r["TABLE_NAME"].ToString()))
                    continue;

                drs = ds.Tables["PARAM_INFO"].Select(String.Format("[TABLE_NAME] = '{0}'", r["TABLE_NAME"].ToString()));
                lstTmp.Clear();
                for (int ir = 0; ir < drs.Length; ir++)
                {
                    if (String.Equals(drs[ir]["PARAM_TYPE"].ToString(), "STRING")
                        || String.Equals(drs[ir]["PARAM_TYPE"].ToString(), "DATE"))
                        continue;

                    lstTmp.Add(String.Format("ROUND(MEDIAN({0}), 10) AS \"{0}\"", drs[ir]["PARAM_NAME"].ToString()));
                }
                dicAlias.Add(r["TABLE_NAME"].ToString(), String.Join(",", lstTmp.ToArray()));
            }

            //--

            lstTmp.Clear();

            dtParam = new DataTable();
            dtParam.Columns.Add(
                new DataColumn("PARAM_NAME", typeof(String))
                );

            foreach (KeyValuePair<string, string> pv in dicAlias)
            {
                dt = oTable.GetTestParameters(pv.Value, pv.Key, wafers);
                foreach (DataColumn c in dt.Columns)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[0][c.ColumnName].ToString()))
                    {
                        DataRow r = dtParam.NewRow();
                        r["PARAM_NAME"] = c.ColumnName;
                        dtParam.Rows.Add(r);
                    }
                }

            }
            return dtParam;
        }

        public DataTable GetParaSpecList(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String[] testarea,
            String product,
            String program,
            String[] lots
            )
        {
            DataSet ds = null;
            DataTable dt = null;
            DataTable dtParam = null;
            DataRow[] drs = null;
            TQP_WAFER oWafer = null;
            TQP_PARASPEC oParaSpec = null;
            TD_TABLE oTable = null;

            Dictionary<string, string> dicAlias = null;
            List<string> lstTmp = null;
            String[] wafers = null;

            //--

            oWafer = new TQP_WAFER();
            oParaSpec = new TQP_PARASPEC();
            oTable = new TD_TABLE();
            ds = new DataSet();
            dicAlias = new Dictionary<string, string>();
            lstTmp = new List<string>();


            //--

            //dt = oWafer.GetData(
            //    "SELECT_WAFER_INFO_07",
            //    new string[] { string.Format("'{0}'", string.Join("','", testarea)), string.Format("'{0}'", string.Join("','", lots)) },
            //    new string[] { dtStart.ToString("yyyyMMdd"), dtEnd.ToString("yyyyMMdd"), product, program }
            //    );
            dt = oWafer.GetWaferInfo07(dtStart.ToString("yyyyMMdd"), dtEnd.ToString("yyyyMMdd"), testarea, product, program, lots);
            dt.TableName = "WAFER_INFO";
            ds.Tables.Add(dt);

            if (dt == null || dt.Rows.Count <= 0)
                return null;

            //dt = dt.Select("[PROBE_CNT] = '0'").CopyToDataTable<DataRow>();
            wafers = new String[dt.Rows.Count];
            for (int ir = 0; ir < dt.Rows.Count; ir++)
            {
                wafers[ir] = dt.Rows[ir]["WAFER_SEQ"].ToString();
            }

            //--

            dt = dt.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
            foreach (DataRow r in dt.Rows)
            {
                dt = oParaSpec.GetData(
                    "SELECT_PARA_DATA",
                    new string[] { string.Format("'{0}'", string.Join("','", r["PROGRAM_REV"].ToString())) },
                    new string[] { factory, r["PROGRAM"].ToString() }
                    );

                if (dtParam == null)
                    dtParam = dt.Clone();
                dtParam.Merge(dt);
            }
            dtParam.AcceptChanges();
            dtParam.TableName = "PARAM_INFO";
            ds.Tables.Add(dtParam);

            //--

            dtParam = dtParam.DefaultView.ToTable(true, "TABLE_NAME");
            foreach (DataRow r in dtParam.Rows)
            {
                if (string.IsNullOrEmpty(r["TABLE_NAME"].ToString()) || string.Equals(r["TABLE_NAME"].ToString(), "TD_"))
                    continue;

                drs = ds.Tables["PARAM_INFO"].Select(String.Format("[TABLE_NAME] = '{0}'", r["TABLE_NAME"].ToString()));
                lstTmp.Clear();
                for (int ir = 0; ir < drs.Length; ir++)
                {
                    if (string.Equals(r["TABLE_NAME"].ToString(), "TD_"))
                        continue;

                    if (String.Equals(drs[ir]["PARAM_TYPE"].ToString(), "STRING")
                        || String.Equals(drs[ir]["PARAM_TYPE"].ToString(), "DATE"))
                        continue;

                    lstTmp.Add(String.Format("ROUND(MEDIAN({0}), 10) AS \"{0}\"", drs[ir]["PARAM_NAME"].ToString()));
                }
                dicAlias.Add(r["TABLE_NAME"].ToString(), String.Join(",", lstTmp.ToArray()));
            }

            //--

            lstTmp.Clear();

            dtParam = new DataTable();
            dtParam.Columns.Add(
                new DataColumn("PARAM_NAME", typeof(String))
                );

            foreach (KeyValuePair<string, string> pv in dicAlias)
            {
                dt = oTable.GetTestParameters(pv.Value, pv.Key, wafers);
                foreach (DataColumn c in dt.Columns)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[0][c.ColumnName].ToString()))
                    {
                        DataRow r = dtParam.NewRow();
                        r["PARAM_NAME"] = c.ColumnName;
                        dtParam.Rows.Add(r);
                    }
                }

            }
            dtParam.AcceptChanges();
            dtParam.DefaultView.Sort = "PARAM_NAME ASC";
            return dtParam.DefaultView.ToTable();
        }


        public DataTable GetParaSpecListEditable(
            string factory,
            string Program
            )
        {
            TQP_PARASPEC oDSL = null;

            try
            {
                oDSL = new TQP_PARASPEC();
                return oDSL.GetParaSpecListEditable(factory, Program);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramInfo(string PROGRAM, string TESTAREA, string PRODUCT, double TARGET_YIELD, string VERSION)
        {
            TQP_PROGRAM oDSL = null;

            try
            {
                oDSL = new TQP_PROGRAM();
                oDSL.UpdateProgramInfo(PROGRAM, TESTAREA, PRODUCT, TARGET_YIELD, VERSION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramParaSpec(string Program, string[,] ParaInfo)
        {
            TQP_PARASPEC oDSL = null;

            try
            {
                oDSL = new TQP_PARASPEC();
                oDSL.UpdateProgramParaSpec(Program, ParaInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUseFlag(string PROGRAM, string USE_FLAG)
        {
            TQP_PROGRAM oDSL = null;

            try
            {
                oDSL = new TQP_PROGRAM();
                oDSL.UpdateUseFlag(PROGRAM, USE_FLAG);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramBin(string Program, string[,] BinInf)
        {
            TQP_BINDESC oDSL = null;

            try
            {
                oDSL = new TQP_BINDESC();
                //oDSL.UpdateProgramBin(Program, BinInf);
                oDSL.UpdateProgramBin(Program, BinInf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateProgram(string PRODUCT,
                            string PROGRAM,
                            string TESTAREA,
                            double TARGET_YIELD,
                            string VERSION,
                            string ParaField,
                            string SiteCnt)
        {
            TQP_PROGRAM oDSL = null;

            try
            {
                oDSL = new TQP_PROGRAM();
                oDSL.CreateProgram(PRODUCT,
                            PROGRAM,
                            TESTAREA,
                            TARGET_YIELD,
                            VERSION,
                            ParaField,
                            SiteCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckDuplicateMapID(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetMapDef(MapID);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public DataTable SelectMaxWaferSeq()
        {
            DataTable dt = null;
            T_PRB oPrb = null;
            try
            {
                oPrb = new T_PRB();
                dt = oPrb.SelectMaxWaferSeq();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetInDies(string MapID)
        {
            TQP_USEMAP oDSL = null;
            try
            {
                oDSL = new TQP_USEMAP();
                return oDSL.GetInDies(MapID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteMapDef(string MapID, string UserID)
        {
            TQP_MAPDEF oMapDef = null;
            try
            {
                oMapDef = new TQP_MAPDEF();
                oMapDef.DeleteMapDef(MapID, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectProductInfo01(string product)
        {
            TQP_PRODUCT oDSL = null;
            try
            {
                oDSL = new TQP_PRODUCT();
                return oDSL.SelectProductInfo01(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT)
        {
            TQP_PRODUCT oDSL = null;
            try
            {
                oDSL = new TQP_PRODUCT();
                oDSL.UpdateProductInfo01(FACILITY, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID, PRODUCT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT)
        {
            TQP_PRODUCT oDSL = null;
            try
            {
                oDSL = new TQP_PRODUCT();
                oDSL.UpdateDeleteFlag(DELETE_FLAG, PRODUCT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateShot(string MapID, ref DataTable dtDies)
        {
            TQP_USEMAP oDSL = null;
            try
            {
                oDSL = new TQP_USEMAP();
                {
                    oDSL.UpdateShot(MapID, ref dtDies);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectShotOrigin(string mapid)
        {
            TQP_USEMAP oDSL = null;
            try
            {
                oDSL = new TQP_USEMAP();
                return oDSL.SelectShotOrigin(mapid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLotList(
            String lotid
            )
        {
            TQP_LOT oLot = null;
            try
            {
                oLot = new TQP_LOT();
                return oLot.GetData(
                    "SELECT_LOT_INFO_BY_LOT_ID",
                    null,
                    new string[] { lotid }
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLotList(
            DateTime dtStart,
            DateTime dtEnd,
            String testarea,
            String product,
            String program
            )
        {
            TQP_LOT olot = null;
            try
            {
                olot = new TQP_LOT();
                return olot.GetData(
                    "GET_LOT_LIST",
                    null,
                    new string[] { dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"), testarea, product, program }
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLotList(
            DateTime dtStart,
            DateTime dtEnd,
            String[] testarea,
            String product,
            String program
            )
        {
            TQP_LOT olot = new TQP_LOT();
            return olot.GetData(
                "GET_LOT_LIST_01",
                new string[] { string.Format("'{0}'", string.Join("','", testarea)) },
                new string[] { dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"), product, program }
                );
        }


        public bool ExistsBinInfo(string program, string bin)
        {
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            object obj = oBinDesc.ExistsBinInfo(program, bin);

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        public int InsertBinInfo(Dictionary<string, string> dicValues, string userid)
        {
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            return oBinDesc.InsertBinInfo(
                dicValues["PROGRAM"],
                dicValues["BIN"],
                dicValues["BIN_NAME"],
                dicValues["CHAR_BIN"],
                dicValues["HIGH_GEC"] == Boolean.TrueString ? "Y" : "N",
                dicValues["DISPLAY"] == Boolean.TrueString ? "Y" : "N",
                dicValues["UPPER_LIMIT_CNT"],
                dicValues["LOWER_LIMIT_CNT"],
                dicValues["COLOR"],
                dicValues["DESCRIPTION"],
                userid
                );
        }

        public int MergeBinInfo(List<String[]> values)
        {
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            int iResult = 0;
            foreach (String[] value in values)
            {
                iResult += oBinDesc.MergeBinInfo(value);
            }

            return iResult;
        }

        public int UpdateBinInfo(Dictionary<string, string> dicValues, string userid)
        {
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            return oBinDesc.UpadateBinInfo(
                dicValues["PROGRAM"],
                dicValues["BIN"],
                dicValues["BIN_NAME"],
                dicValues["CHAR_BIN"],
                dicValues["HIGH_GEC"] == Boolean.TrueString ? "Y" : "N",
                dicValues["DISPLAY"] == Boolean.TrueString ? "Y" : "N",
                dicValues["UPPER_LIMIT_CNT"],
                dicValues["LOWER_LIMIT_CNT"],
                dicValues["COLOR"],
                dicValues["DESCRIPTION"],
                userid
                );
        }

        public int DeleteBinInfo(string program, string bin, string userid)
        {
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            return oBinDesc.DeleteBinInfo(program, bin, userid);
        }
    }
}
