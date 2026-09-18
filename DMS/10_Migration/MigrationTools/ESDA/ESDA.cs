using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;

namespace MigrationTools.ESDA
{
    public class ESDA
    {
        private System.Data.OracleClient.OracleConnection connSourceDb = null;
        private const int RK = 1;

        //private readonly string m_ESDA_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.144.251.226)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=DMS)));User ID=DMSMGR;Password=DMSMGR;";
        //private readonly string m_ANAMDB_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.144.251.226)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=DMS)));User ID=DMSMGR;Password=DMSMGR;";
        //private readonly string m_IDB_ANAMIDB_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.144.251.226)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=DMS)));User ID=DMSMGR;Password=DMSMGR;";

        private readonly string m_ANAMDB_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.147.13.13)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=anamdb)));User ID=T_ESDA;Password=T_ESDA;";
        private readonly string m_IDB_ANAMIDB_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= 10.147.13.20)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=ANAMIDB)));User ID=IDBMGRREAD;Password=IDBMGRREAD;";
        
        //Migraion 시 FTP Download 속도 관련 Issue 가 있기 때문에 File 들을 미리 Download 받아 놓는다.
        private readonly string m_TempFilePath = @"D:\FAB1MIG";

        public void fnFTPRecipe(string strDevice, ref MapInfo.MapInfo oInfo)
        {
            MigrationTools.ESDA.HFtpClient oFTP = null;
            string strLocalPathName = string.Empty;
            string strLocalFullPath = string.Empty;
            string strFTPPath = "/dms/dms/DRV";
            string strFTPFileName = string.Empty;
            string strFTPFullPath = string.Empty;

            FileInfo oRecipeFile = null;
            DirectoryInfo oDirectory = null;

            StreamReader sr = null;

            try
            {
                strFTPFileName = string.Format("{0}.drv", strDevice.ToLower());

                //FTP 속도 문제 때문에 미리 Download 받고 없으면 FTP 로 Download 한다.
                strLocalFullPath = System.IO.Path.Combine(m_TempFilePath, strFTPFileName);
                oRecipeFile = new FileInfo(strLocalFullPath);
                if (!oRecipeFile.Exists)
                {
                    oFTP = new MigrationTools.ESDA.HFtpClient("10.147.13.16", 21, "dms", "dms_0930", false, true);
                    bool IsFtpConnect = oFTP.LoginTest();
                    if (IsFtpConnect == false)
                        throw new Exception("FTP에 접속할 수 없습니다.");

                    strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "TEMP");

                    oDirectory = new DirectoryInfo(strLocalPathName);
                    if (!oDirectory.Exists)
                        oDirectory.Create();

                    strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFTPFileName);
                    oRecipeFile = new FileInfo(strLocalFullPath);
                    if (!oRecipeFile.Exists)
                    {

                        strFTPFullPath = System.IO.Path.Combine(strFTPPath, strFTPFileName).Replace(@"\", "/");
                        oFTP.SetCurrentDir(strFTPPath);
                        oFTP.Down(strFTPFullPath, strLocalFullPath);

                        oRecipeFile = new FileInfo(strLocalFullPath);
                        if (!oRecipeFile.Exists)
                            throw new Exception("Recipe File을 Download 하였으나 존재 하지 않습니다.");
                    }
                }

                using (sr = new System.IO.StreamReader(oRecipeFile.FullName, System.Text.Encoding.Default))
                {
                    string line = string.Empty;
                    string[] sPara = null;
                    string strTemp = string.Empty;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.ToUpper().StartsWith("DEVICE"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            oInfo.Device = sPara[1].Trim();
                        }
                        else if (line.ToUpper().StartsWith("MAX_ROWS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oInfo.Max_rows) == false)
                                oInfo.Max_rows = 0;
                        }
                        else if (line.ToUpper().StartsWith("MAX_COLS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oInfo.Max_cols) == false)
                                oInfo.Max_cols = 0;
                        }
                        else if (line.ToUpper().StartsWith("X_PERIOD"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.X_period) == false)
                                oInfo.X_period = 0;
                        }
                        else if (line.ToUpper().StartsWith("Y_PERIOD"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.Y_period) == false)
                                oInfo.Y_period = 0;
                        }
                        else if (line.ToUpper().StartsWith("X_OS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.X_os) == false)
                                oInfo.X_os = 0;
                        }
                        else if (line.ToUpper().StartsWith("Y_OS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.Y_os) == false)
                                oInfo.Y_os = 0;
                        }
                        else if (line.ToUpper().StartsWith("STREET_WIDTH"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.Street_width) == false)
                                oInfo.Street_width = 0;
                        }
                        else if (line.ToUpper().StartsWith("STREET_HEIGHT"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (decimal.TryParse(strTemp, out oInfo.Street_height) == false)
                                oInfo.Street_height = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }

                //if(oRecipeFile != null && oRecipeFile.Exists)
                //    oRecipeFile.Delete();
            }

        }

        public void fnFTPMap(string strDevice, ref MapInfo.Map oMap)
        {
            MigrationTools.ESDA.HFtpClient oFTP = null;
            string strLocalPathName = string.Empty;
            string strLocalFullPath = string.Empty;
            string strFTPPath = "/%2f/opt/esda/config";  // 기본 Directory 가 아니라 상위 부터 할경우 /%2f 추가...!!!!
            string strFTPFileName = string.Empty;
            string strFTPFullPath = string.Empty;

            bool bDieList = false;

            int iIndex = 0;

            FileInfo oMapFile = null;

            StreamReader sr = null;

            try
            {
                strFTPFileName = string.Format("{0}.map", strDevice);

                //FTP 속도 문제 때문에 미리 Download 받고 없으면 FTP 로 Download 한다.
                strLocalFullPath = System.IO.Path.Combine(m_TempFilePath, strFTPFileName);
                oMapFile = new FileInfo(strLocalFullPath);
                if (!oMapFile.Exists)
                {
                    oFTP = new MigrationTools.ESDA.HFtpClient("10.147.13.14", 21, "anam1", "anam1", false, true);
                    bool IsFtpConnect = oFTP.LoginTest();
                    if (IsFtpConnect == false)
                        throw new Exception("FTP에 접속할 수 없습니다.");

                    strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "TEMP");
                    DirectoryInfo oDirectory = new DirectoryInfo(strLocalPathName);
                    if (!oDirectory.Exists)
                        oDirectory.Create();

                    strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFTPFileName);

                    oMapFile = new FileInfo(strLocalFullPath);
                    if (!oMapFile.Exists)
                    {
                        strFTPFullPath = System.IO.Path.Combine(strFTPPath, strFTPFileName).Replace(@"\", "/");
                        oFTP.SetCurrentDir(strFTPPath);
                        oFTP.Down(strFTPFullPath, strLocalFullPath);

                        oMapFile = new FileInfo(strLocalFullPath);
                        if (!oMapFile.Exists)
                            throw new Exception("Map File을 Download 하였으나 존재 하지 않습니다.");
                    }
                }

                using (sr = new System.IO.StreamReader(oMapFile.FullName, System.Text.Encoding.Default))
                {
                    string line = string.Empty;
                    string[] sPara = null;
                    string strTemp = string.Empty;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (bDieList)
                        {
                            sPara = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            MapInfo.DIEINFO_TAG oDie = new MapInfo.DIEINFO_TAG();

                            strTemp = sPara[0].Trim();
                            if (int.TryParse(strTemp, out oDie.DX) == false)
                                oDie.DX = 0;
    
                            strTemp = sPara[1].Trim();
                            if (int.TryParse(strTemp, out oDie.DY) == false)
                                oDie.DY = 0;

                            strTemp = sPara[2].Trim();
                            if (int.TryParse(strTemp, out oDie.Index) == false)
                                oDie.Index = 0;

                            oMap.TestDieInfo[iIndex] = oDie;
                            iIndex++;

                        }
                        else if (line.ToUpper().StartsWith("NUM_TAGVALUE_PAIRS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oMap.Num_tagvalue_pairs) == false)
                                oMap.Num_tagvalue_pairs = 0;
                        }
                        else if (line.ToUpper().StartsWith("NUM_COLUMNS"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oMap.Num_columns) == false)
                                oMap.Num_columns = 0;
                        }
                        else if (line.ToUpper().StartsWith("MAX_X"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oMap.Max_x) == false)
                                oMap.Max_x = 0;
                        }
                        else if (line.ToUpper().StartsWith("MAX_Y"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oMap.Max_y) == false)
                                oMap.Max_y = 0;
                        }
                        else if (line.ToUpper().StartsWith("NUM_DIE"))
                        {
                            sPara = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            strTemp = sPara[1].Trim();

                            if (int.TryParse(strTemp, out oMap.Num_die) == false)
                                oMap.Num_die = 0;

                            oMap.TestDieInfo = new MapInfo.DIEINFO_TAG[oMap.Num_die];

                            bDieList = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }

                ////if (oMapFile != null && oMapFile.Exists)
                ////    oMapFile.Delete();
            }

        }

        public void fnGetMapSetup(string strDevice, ref MapInfo.MAP_SETUP oMapsetup)
        {
            DataTable dtMapSetup = null;

            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.AppendLine(string.Format("SELECT * FROM IDBMGR.MAP_SETUP WHERE DEVICE = '{0}' OR  TW_DEVICE = '{0}' OR  DM_DEVICE = '{0}'", strDevice));

                dtMapSetup = fnTableSelect(m_IDB_ANAMIDB_ConnectionString, sQuery.ToString());
                if (dtMapSetup != null && dtMapSetup.Rows.Count > 0)
                {
                    oMapsetup.NOTCH_TYPE = dtMapSetup.Rows[0]["NOTCH_TYPE"].ToString();

                    if (int.TryParse(dtMapSetup.Rows[0]["TEST_ANGLE"].ToString(), out oMapsetup.TEST_ANGLE) == false)
                        oMapsetup.TEST_ANGLE = 0;

                    if (int.TryParse(dtMapSetup.Rows[0]["WAFER_SIZE"].ToString(), out oMapsetup.WAFER_SIZE) == false)
                        oMapsetup.WAFER_SIZE = 0;

                    if (decimal.TryParse(dtMapSetup.Rows[0]["ORIGIN_X"].ToString(), out oMapsetup.ORIGIN_X) == false)
                        oMapsetup.ORIGIN_X = 0;

                    if (decimal.TryParse(dtMapSetup.Rows[0]["ORIGIN_Y"].ToString(), out oMapsetup.ORIGIN_Y) == false)
                        oMapsetup.ORIGIN_Y = 0;

                    if (int.TryParse(dtMapSetup.Rows[0]["TEST_X"].ToString(), out oMapsetup.TEST_X) == false)
                        oMapsetup.TEST_X = 0;

                    if (int.TryParse(dtMapSetup.Rows[0]["TEST_Y"].ToString(), out oMapsetup.TEST_Y) == false)
                        oMapsetup.TEST_Y = 0;
                }
                else
                    throw new Exception("MAP_SETUP 에 정의된 정보가 없습니다.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnGetDefectList(string strLevel_seq, ref MapInfo.DEFECT_INFO oDefectList)
        {
            DataTable dtDefectList = null;
            DataTable dtDefectGroup= null;

            StringBuilder sQuery = new StringBuilder();

            MapInfo.DEFECT oDefect;

            try
            {
                sQuery.AppendLine("SELECT MOD(A.DIE, 256) AS DIE_X, ");
                sQuery.AppendLine("TRUNC(A.DIE / 256) AS DIE_Y, ");
                sQuery.AppendLine("A.*, B.IMAGE_FILE_NAME, B.THUMB_FILE_NAME ");
                sQuery.AppendLine("FROM T_ESDA_IDATA A, T_ESDA_IMAGES B ");
                sQuery.AppendLine("WHERE 1 = 1  ");
                sQuery.AppendLine(string.Format("AND A.LEVEL_SEQ = {0} ", strLevel_seq));
                sQuery.AppendLine("AND A.LEVEL_SEQ = B.LEVEL_SEQ(+) ");
                sQuery.AppendLine("AND A.DATA_SEQ = B.DATA_SEQ(+) ");
                sQuery.AppendLine("ORDER BY A.DIE ");

                dtDefectList = fnTableSelect(m_ANAMDB_ConnectionString, sQuery.ToString());
                if (dtDefectList != null && dtDefectList.Rows.Count > 0)
                {
                    oDefectList.DefectList = new MapInfo.DEFECT[dtDefectList.Rows.Count];
                    for (int ir = 0; ir < dtDefectList.Rows.Count; ir++)
                    {
                        oDefect = new MapInfo.DEFECT();

                        oDefect.Index = ir + 1;

                        if (decimal.TryParse(dtDefectList.Rows[ir]["DEFECT_SIZE"].ToString(), out oDefect.DefectSize) == false)
                            oDefect.DefectSize = 0;

                        oDefect.DefectSize = oDefect.DefectSize * (decimal)RK;

                        if (int.TryParse(dtDefectList.Rows[ir]["DIE_X"].ToString(), out oDefect.DIE_X) == false)
                            oDefect.DIE_X = 0;

                        if (int.TryParse(dtDefectList.Rows[ir]["DIE_Y"].ToString(), out oDefect.DIE_Y) == false)
                            oDefect.DIE_Y = 0;

                        if (decimal.TryParse(dtDefectList.Rows[ir]["XSIZE"].ToString(), out oDefect.Xsize) == false)
                            oDefect.Xsize = 0;

                        oDefect.Xsize = oDefect.Xsize * (decimal)RK;

                        if (decimal.TryParse(dtDefectList.Rows[ir]["YSIZE"].ToString(), out oDefect.Ysize) == false)
                            oDefect.Ysize = 0;

                        oDefect.Ysize = oDefect.Ysize * (decimal)RK;

                        if (decimal.TryParse(dtDefectList.Rows[ir]["XMICRON"].ToString(), out oDefect.Xmicron) == false)
                            oDefect.Xmicron = 0;

                        oDefect.Xmicron = oDefect.Xmicron * (decimal)RK;

                        if (decimal.TryParse(dtDefectList.Rows[ir]["YMICRON"].ToString(), out oDefect.Ymicron) == false)
                            oDefect.Ymicron = 0;

                        oDefect.Ymicron = oDefect.Ymicron * (decimal)RK;

                        oDefect.Image_file_name = dtDefectList.Rows[ir]["IMAGE_FILE_NAME"].ToString();
                        oDefect.Thumb_file_name = dtDefectList.Rows[ir]["THUMB_FILE_NAME"].ToString();
                        oDefect.Newdefect = dtDefectList.Rows[ir]["NEW_DEFECT"].ToString();
                        oDefect.First_level = dtDefectList.Rows[ir]["FIRST_LEVEL"].ToString();

                        if (int.TryParse(dtDefectList.Rows[ir]["DEFECT_CATEGORY"].ToString(), out oDefect.Defect_category) == false)
                            oDefect.Defect_category = 0;

                        if (int.TryParse(dtDefectList.Rows[ir]["ADC"].ToString(), out oDefect.ADC) == false)
                            oDefect.ADC = 0;

                        if (int.TryParse(dtDefectList.Rows[ir]["DEFECT_CLUSTER"].ToString(), out oDefect.Defect_cluster) == false)
                            oDefect.Defect_cluster = 0;

                        if (int.TryParse(dtDefectList.Rows[ir]["REV"].ToString(), out oDefect.REV) == false)
                            oDefect.REV = 0;

                        if (int.TryParse(dtDefectList.Rows[ir]["ROUGHBIN"].ToString(), out oDefect.Roughbin) == false)
                            oDefect.Roughbin = 0;


                        oDefectList.DefectList[ir] = oDefect;
                    }
                }

                dtDefectGroup = dtDefectList.DefaultView.ToTable(true, new string[] { "DIE_X", "DIE_Y" });

                oDefectList.DefectCount = dtDefectList.Rows.Count;
                oDefectList.DefectDieCount = dtDefectGroup.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public FileInfo fnFTPImage(string strFTPDirctory, DateTime dtTime, string strDevice, string strLotID, int iWaferNo)
        {
            MigrationTools.ESDA.HFtpClient oFTP = null;
            string strLocalPathName = string.Empty;
            string strLocalFullPath = string.Empty;
            string strFTPPath = "/%2f/images";
            string strFTPFileName = string.Empty;
            string strFTPFullPath = string.Empty;

            FileInfo oRecipeFile = null;
            DirectoryInfo oDirectory = null;

            StreamReader sr = null;

            string[] ArrPath = null;

            try
            {
                ArrPath = strFTPDirctory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                strFTPFileName = ArrPath[ArrPath.Length - 1].Trim();

                oFTP = new MigrationTools.ESDA.HFtpClient("10.147.13.14", 21, "anam1", "anam1", false, true);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");


                //Path 는 년/월/일/Device/Lot/WaferNo
                strLocalPathName = System.IO.Path.Combine(@"D:\DEFECTIMAGE",
                    string.Format("{0:0000}", dtTime.Year), string.Format("{0:00}", dtTime.Month), string.Format("{0:00}", dtTime.Day),
                    strDevice, strLotID,  string.Format("{0:00}", iWaferNo));

                oDirectory = new DirectoryInfo(strLocalPathName);
                if (!oDirectory.Exists)
                    oDirectory.Create();

                strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFTPFileName);

                strFTPFullPath = System.IO.Path.Combine(strFTPPath, strFTPDirctory).Replace(@"\", "/");
                //oFTP.SetCurrentDir(strFTPPath);
                oFTP.Down(strFTPFullPath, strLocalFullPath);

                oRecipeFile = new FileInfo(strLocalFullPath);
                if (!oRecipeFile.Exists)
                    throw new Exception("Defect Image File을 Download 하였으나 존재 하지 않습니다.");

                return oRecipeFile;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }
            }

        }

        public FileInfo fnFTPImageDownload(string strFTPDirctory)
        {
            MigrationTools.ESDA.HFtpClient oFTP = null;
            string strLocalFullPath = string.Empty;
            string strFTPPath = "/%2f/images";
            string strFTPFullPath = string.Empty;

            FileInfo oRecipeFile = null;
            DirectoryInfo oDirectory = null;

            StreamReader sr = null;

            try
            {
                string[] ArrImage = strFTPDirctory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                oFTP = new MigrationTools.ESDA.HFtpClient("10.147.13.14", 21, "anam1", "anam1", false, true);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");

                strLocalFullPath = System.IO.Path.Combine(@"D:\DEFECTIMAGE", strFTPDirctory.Replace("/", @"\"));

                oDirectory = new DirectoryInfo(strLocalFullPath.Replace(ArrImage[ArrImage.Length - 1].ToString(), ""));
                if (!oDirectory.Exists)
                    oDirectory.Create();

                strFTPFullPath = System.IO.Path.Combine(strFTPPath, strFTPDirctory).Replace(@"\", "/");
                oFTP.Down(strFTPFullPath, strLocalFullPath);

                oRecipeFile = new FileInfo(strLocalFullPath);
                if (!oRecipeFile.Exists)
                    throw new Exception("Defect Image File을 Download 하였으나 존재 하지 않습니다.");

                return oRecipeFile;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }
            }

        }


        public DataTable fnTableSelect(string strConnectInfo, string strQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                connSourceDb = new System.Data.OracleClient.OracleConnection();
                connSourceDb.ConnectionString = strConnectInfo;
                connSourceDb.Open();

                using (System.Data.OracleClient.OracleCommand oCommand = connSourceDb.CreateCommand())
                {
                    oCommand.CommandText = strQuery;
                    oCommand.CommandTimeout = 60;
                    oCommand.CommandType = CommandType.Text;

                    System.Data.OracleClient.OracleDataAdapter oAdapter = new System.Data.OracleClient.OracleDataAdapter(oCommand);
                    oAdapter.Fill(dt);
                    oAdapter.Dispose();
                    oCommand.Dispose();
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connSourceDb != null)
                {
                   connSourceDb.Close();
                   connSourceDb.Dispose();
                   connSourceDb = null;
                }
            }
        }
    }
}
