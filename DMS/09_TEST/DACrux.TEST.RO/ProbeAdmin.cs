/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : ProbeAdmin.cs
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
using System.Text;
using System.Data;

namespace DACrux.TEST.RO
{
    public class ProbeAdmin
    {
        DACrux.TEST.Interface.iProbeAdmin m_OBJ;
        public ProbeAdmin()
        {

            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iProbeAdmin)
                , strUrl + "/DACrux.TEST.BSL.ProbeAdmin.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iProbeAdmin;
        }

        public ProbeAdmin(string sServer, int nPort)
        {
            if (string.IsNullOrEmpty(sServer))
                sServer = "localhost";
            string strUrl = string.Format("tcp://{0}:{1}", sServer, nPort);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iProbeAdmin),
                strUrl + "/DACrux.TEST.BSL.ProbeAdmin.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iProbeAdmin;
        }

        public DataTable GetCustomerList()
        {
            return m_OBJ.GetCustomerList();
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
            m_OBJ.CreateCustomer(CustomerID
                                    , Customer
                                    , CustomerDesc
                                    , isDataService
                                    , UserCount
                                    , ExpireDate
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
            m_OBJ.DeleteCustomer(CustomerID);
        }

        public void UpdateCustomer(
            string Customer
            , bool isDataService
            , int UserCount
            , string ExpireDate
            , string CustomerDesc
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
            m_OBJ.UpdateCustomer(Customer
                                    , CustomerDesc
                                    , isDataService
                                    , UserCount
                                    , ExpireDate
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
            return m_OBJ.GetMapIDList();
        }

        public DataTable GetMapIDAllList()
        {
            return m_OBJ.GetMapIDAllList();
        }

        #region Map Setup 관련

        public DataTable GetMapIDListAll()
        {
            return m_OBJ.GetMapIDListAll();
        }

        public DataTable GetMapIDListNotDelete()
        {
            return m_OBJ.GetMapIDListNotDelete();
        }

        public bool ExistsMapID(string factory, string mapID)
        {
            return m_OBJ.ExistsMapID(factory, mapID);
        }

        public void InsertMapData(Dictionary<string, string> dictionary, string userID)
        {
            m_OBJ.InsertMapData(dictionary, userID);
        }

        public void UpdateMapData(Dictionary<string, string> dictionary, string userID)
        {
            m_OBJ.UpdateMapData(dictionary, userID);
        }

        public void DeleteMapData(string factory, string mapID)
        {
            m_OBJ.DeleteMapData(factory, mapID);
        }

        #endregion

        public void CreateAVITable(string PRODUCT)
        {
            m_OBJ.CreateAVITable(PRODUCT);
        }
        public void SetDeviceDef(string FACILITY, string PRODUCT, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID)
        {
            m_OBJ.SetDeviceDef(FACILITY, PRODUCT, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID);
        }

        public DataTable GetMapDef(string MapID)
        {
            DataTable dt = null;
            dt = m_OBJ.GetMapDef(MapID);
            return dt;
        }

        public DataTable GetDies(string MapID)
        {
            DataTable dt = null;
            dt = m_OBJ.GetDies(MapID);
            return dt;
        }

        public void SetMapDef(string MAPID
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
            m_OBJ.SetMapDef(MAPID
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
            m_OBJ.CreateUseMap(MapID, dtDies);
        }

        public void CreateUseMap(string strFactory, string MapID, DataTable dtDies)
        {
            m_OBJ.CreateUseMap(strFactory, MapID, dtDies);
        }

        public void UpdateMapDef(string MAPID,
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
            m_OBJ.UpdateMapDef(MAPID,
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

        public string ColorString(int iBinNumber)
        {
            return m_OBJ.ColorString(iBinNumber);
        }

        public DataTable GetProductList()
        {
            return m_OBJ.GetProductList();
        }

        public DataTable GetProductList(
            string dtStart,
            string dtEnd,
            string[] testarea
            )
        {
            return m_OBJ.GetProductList(
                dtStart,
                dtEnd,
                testarea
                );
        }

        public DataTable GetTestAreaList()
        {
            return m_OBJ.GetTestAreaList();
        }

        public DataTable GetTestAreaListNotPCM()
        {
            return m_OBJ.GetTestAreaListNotPCM();
        }

        public DataTable GetTestProgramList(
            string testarea,
            string device
            )
        {
            return m_OBJ.GetTestProgramList(testarea, device);
        }

        public DataTable GetTestProgramList(
            string[] testarea,
            string[] device
            )
        {
            return m_OBJ.GetTestProgramList(testarea, device);
        }


        public DataTable GetTestProgramList(string dtStart, string dtEnd, string[] testarea, string[] device)
        {
            return m_OBJ.GetTestProgramList(dtStart, dtEnd, testarea, device);
        }

        public DataTable GetProgramInfo(string Program)
        {
            return m_OBJ.GetProgramInfo(Program);
        }

        public void CreateParaSpec(string[,] ParaInfo)
        {
            m_OBJ.CreateParaSpec(ParaInfo);
        }

        public void CreateParaSpec(
            String factory,
            String program,
            String userid,
            DataTable dt
            )
        {
            m_OBJ.CreateParaSpec(factory, program, userid, dt);
        }

        public void CreateBins(string[,] BinInf)
        {
            m_OBJ.CreateBins(BinInf);
        }

        public object CreateProgram(
            string factory,
            string program,  
            string testarea, 
            string device, 
            string userid
            )
        {
            return m_OBJ.CreateProgram(factory, program, testarea, device, userid);
        }

        public void CreateProgram(string PRODUCT,
                            string PROGRAM,
                            string TESTAREA,
                            double TARGET_YIELD,
                            string VERSION,
                            string ParaField,
                            string SiteCnt)
        {
            m_OBJ.CreateProgram(PRODUCT,
                        PROGRAM,
                        TESTAREA,
                        TARGET_YIELD,
                        VERSION,
                        ParaField,
                        SiteCnt);
        }


        public void DeleteProgramBin(string PROGRAM)
        {
            m_OBJ.DeleteProgramBin(PROGRAM);
        }

        public void DeleteProgramParaSpec(string PROGRAM)
        {
            m_OBJ.DeleteProgramParaSpec(PROGRAM);
        }

        public DataTable GetBinListEditable(
            string Program
            )
        {
            return m_OBJ.GetBinListEditable(Program);
        }

        public DataTable GetParaSpecList(
            string factory,
            string program
            )
        {
            return m_OBJ.GetParaSpecList(
                factory,
                program
                );
        }

        public DataTable GetParaSpecList(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String testarea,
            String product,
            String program,
            String[] lots)
        {
            return m_OBJ.GetParaSpecList(
                dtStart,
                dtEnd,
                factory,
                testarea,
                product,
                program,
                lots
                );
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
            return m_OBJ.GetParaSpecList(
                dtStart,
                dtEnd,
                factory,
                testarea,
                product,
                program,
                lots
                );
        }

        public DataTable GetParaSpecListEditable(
            string factory,
            string Program
            )
        {
            return m_OBJ.GetParaSpecListEditable(factory, Program);
        }

        public void UpdateProgramInfo(string PROGRAM, string TESTAREA, string PRODUCT, double TARGET_YIELD, string VERSION)
        {
            m_OBJ.UpdateProgramInfo(PROGRAM, TESTAREA, PRODUCT, TARGET_YIELD, VERSION);
        }

        public void DeleteProgramInfo(string PROGRAM)
        {
            m_OBJ.DeleteProgramInfo(PROGRAM);
        }

        public void UpdateProgramBin(string Program, string[,] BinInf)
        {
            m_OBJ.UpdateProgramBin(Program, BinInf);
        }

        public void UpdateProgramParaSpec(string Program, string[,] ParaInfo)
        {
            m_OBJ.UpdateProgramParaSpec(Program, ParaInfo);
        }

        public void UpdateUseFlag(string PROGRAM, string USE_FLAG)
        {
            m_OBJ.UpdateUseFlag(PROGRAM, USE_FLAG);
        }

        public bool CheckDuplicateMapID(string MapID)
        {
            return m_OBJ.CheckDuplicateMapID(MapID);
        }

        public DataTable GetInDies(string MapID)
        {
            return m_OBJ.GetInDies(MapID);
        }

        public void DeleteMapDef(string MapID, string UserID)
        {
            m_OBJ.DeleteMapDef(MapID, UserID);
        }

        public DataTable SelectProductInfo01(string product)
        {
            return m_OBJ.SelectProductInfo01(product);
        }

        public void UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT)
        {
            m_OBJ.UpdateProductInfo01(FACILITY, CUSTOMER_ID, CUSTOMER_NAME, CUSTPROD, MAPID, PRODUCT);
        }

        public void UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT)
        {
            m_OBJ.UpdateDeleteFlag(DELETE_FLAG, PRODUCT);
        }

        public void UpdateShot(string MapID, ref DataTable dtDies)
        {
            m_OBJ.UpdateShot(MapID, ref dtDies);
        }

        public DataTable SelectShotOrigin(string mapid)
        {
            return m_OBJ.SelectShotOrigin(mapid);
        }

        public DataTable GetLotList(
            DateTime dtStart,
            DateTime dtEnd,
            String testarea,
            String product,
            String program
            )
        {
            return m_OBJ.GetLotList(
                dtStart,
                dtEnd,
                testarea,
                product,
                program
                );
        }

        public DataTable GetLotList(
            DateTime dtStart,
            DateTime dtEnd,
            String[] testarea,
            String product,
            String program
            )
        {
            return m_OBJ.GetLotList(
                dtStart,
                dtEnd,
                testarea,
                product,
                program
                );
        }

        public DataTable GetLotList(string lotid)
        {
            return m_OBJ.GetLotList(
                lotid
                );
        }

        public object CreateCopyBin(string factory, string testarea, string sourceProgram, string targetProgram, string userid)
        {
            return m_OBJ.CreateCopyBin(factory, testarea, sourceProgram, targetProgram, userid);
        }

        public String[] GetProgramNotExistBin(string testArea, string targetProgram)
        {
            return m_OBJ.GetProgramNotExistBin(testArea, targetProgram);
        }

        public bool ExistsBinInfo(string program, string bin)
        {
            return m_OBJ.ExistsBinInfo(program, bin);
        }

        public int InsertBinInfo(
            Dictionary<string, string> dicValues,
            string userid
            )
        {
            return m_OBJ.InsertBinInfo(dicValues, userid);
        }

        public int MergeBinInfo(
            List<String[]> values
            )
        {
            return m_OBJ.MergeBinInfo(values);
        }

        public int UpdateBinInfo(
            Dictionary<string, string> dicValues,
            string userid
            )
        {
            return m_OBJ.UpdateBinInfo(dicValues, userid);
        }

        public int DeleteBinInfo(string program, string bin, string userid)
        {
            return m_OBJ.DeleteBinInfo(program, bin, userid);
        }
    }
}
