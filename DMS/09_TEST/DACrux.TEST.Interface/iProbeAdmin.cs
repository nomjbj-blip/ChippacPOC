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
using System.Text;
using System.Data;

namespace DACrux.TEST.Interface
{
    public interface iProbeAdmin
    {
        DataTable GetCustomerList();
        void UpdateCustomer(string Customer, string CustomerDesc, bool isDataService, int UserCount, string ExpireDate, string FtpSite
            , string FtpUser, string FtpPassword, string FtpPath, string SendTime, string AVIFormat, string EDSFormat, string InklessFormat, string RawDataFormat, string CustCode, string SprName, string MapRcvDir, string BackupDir, string ErrorDir, string LogDir, string CustomerID);
        void DeleteCustomer(string CustomerID);
        void CreateCustomer(string CustomerID, string Customer, string CustomerDesc, bool isDataService, int UserCount, string ExpireDate, string FtpSite
            , string FtpUser, string FtpPassword, string FtpPath, string SendTime, string AVIFormat, string EDSFormat, string InklessFormat, string RawDataFormat, string CustCode, string SprName, string MapRcvDir, string BackupDir, string ErrorDir, string LogDir);
        DataTable GetMapIDList();
        DataTable GetMapIDAllList();
        
        DataTable GetMapIDListAll();
        DataTable GetMapIDListNotDelete();
        bool ExistsMapID(string factory, string mapID);
        void InsertMapData(Dictionary<string, string> dictionary, string userID);
        void UpdateMapData(Dictionary<string, string> dictionary, string userID);
        void DeleteMapData(string factory, string mapID);

        void CreateAVITable(string PRODUCT);
        void SetDeviceDef(string FACILITY, string PRODUCT, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID);
        DataTable GetMapDef(string MapID);
        DataTable GetDies(string MapID);
        void SetMapDef(string MAPID
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
                        , int REFERENCEDIE_SETTING);
        void CreateUseMap(string MapID, DataTable dsDies);
        void CreateUseMap(string strFactory, string MapID, DataTable dtDies);
        void UpdateMapDef(string MAPID,
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
                            int XY_DIRECTION);
        string ColorString(int iBinNumber);
        DataTable GetProductList();
        DataTable GetProductList(string dtStart, string dtEnd, string[] testarea);
        DataTable GetTestAreaList();
        DataTable GetTestAreaListNotPCM();
        DataTable GetTestProgramList(string testarea, string device);
        DataTable GetTestProgramList(string[] testarea, string[] device);
        DataTable GetTestProgramList(string dtStart, string dtEnd, string[] testarea, string[] device);
        object CreateCopyBin(string factory, string testarea, string sourceProgram, string targetProgram, string userid);
        String[] GetProgramNotExistBin(string TestArea, string TargetProgram);
        DataTable GetProgramInfo(string Program);

        void CreateParaSpec(string[,] ParaInfo);
        void CreateParaSpec(string factory, string program, string userid, DataTable dt);
        void CreateBins(string[,] BinInf);

        object CreateProgram(string factory, string program, string testarea, string device, string userid);
        void CreateProgram(string PRODUCT,
                            string PROGRAM,
                            string TESTAREA,
                            double TARGET_YIELD,
                            string VERSION,
                            string ParaField,
                            string SiteCnt);
        void DeleteProgramInfo(string PROGRAM);
        void DeleteProgramBin(string PROGRAM);
        void DeleteProgramParaSpec(string PROGRAM);
        DataTable GetBinListEditable(string Program);

        DataTable GetParaSpecList(string factory, string program);
        DataTable GetParaSpecList(DateTime dtStart, DateTime dtEnd, string factory, string testarea, string product, string program, string[] lots);
        // Db Hitek 추가
        DataTable GetParaSpecList(DateTime dtStart, DateTime dtEnd, string factory, string[] testarea, string product, string program, string[] lots);
        DataTable GetParaSpecListEditable(string factory, string Program);

        void UpdateProgramInfo(string PROGRAM, string TESTAREA, string PRODUCT, double TARGET_YIELD, string VERSION);
        void UpdateUseFlag(string PROGRAM, string USE_FLAG);
        void UpdateProgramBin(string Program, string[,] BinInf);
        void UpdateProgramParaSpec(string Program, string[,] ParaInfo);
        bool CheckDuplicateMapID(string MapID);
        DataTable SelectMaxWaferSeq();
        DataTable GetInDies(string MapID);
        void DeleteMapDef(string MapID, string UserID);
        DataTable SelectProductInfo01(string product);
        void UpdateProductInfo01(string FACILITY, string CUSTOMER_ID, string CUSTOMER_NAME, string CUSTPROD, string MAPID, string PRODUCT);
        void UpdateDeleteFlag(string DELETE_FLAG, string PRODUCT);
        void UpdateShot(string MapID, ref DataTable dtDies);
        DataTable SelectShotOrigin(string mapid);

        DataTable GetLotList(string lotid);
        DataTable GetLotList(DateTime dtStart, DateTime dtEnd, String testarea, String product, String program);
        DataTable GetLotList(DateTime dtStart, DateTime dtEnd, string[] testarea, string product, string program);

        bool ExistsBinInfo(string program, string bin);
        int InsertBinInfo(Dictionary<string, string> dicValues, string userid);
        int MergeBinInfo(List<String[]> values);
        int UpdateBinInfo(Dictionary<string, string> dicValues, string userid);
        int DeleteBinInfo(string program, string bin, string userid);

    }
}
