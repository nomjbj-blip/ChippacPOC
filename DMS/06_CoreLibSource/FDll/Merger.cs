using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.TEST.DSL;

namespace FDll
{
    public class Merger
    {
        string m_Bin_Dynamic_when = "WHEN :testarea_GEC = 'F' THEN :testarea";
        string m_Bin_Dynamic_else = "ELSE :testarea";
        string m_BinGec_Dynamic_when = "WHEN :testarea_GEC = 'F' THEN 'F'";
        string m_BinGec_Dynamic_else = "ELSE 'T'";
        string m_VI_Dynamic_when = "WHEN VI_:testarea_GEC = 'F' THEN VI_:testarea";
        string m_VI_Dynamic_else = "ELSE 0";
        string m_VIGec_Dynamic_when = "WHEN VI_:testarea_GEC = 'F' THEN 'F'";
        string m_VIGec_Dynamic_else = "ELSE 'T'";

        string m_BIN_Dynamic_TEMP_TABLE = ":testarea   NUMBER(3)";
        string m_BIN_GEC_Dynamic_TEMP_TABLE = ":testarea_GEC   CHAR(1 BYTE)";
        string m_VI_Dynamic_TEMP_TABLE = "VI_:testarea NUMBER(3)";
        string m_VI_GEC_Dynamic_TEMP_TABLE = "VI_:testarea_GEC CHAR(1 BYTE)";

        public DataSet MergeByQuery(
            string LotID, string WaferID, string Customer, string Product
            , string ActTestArea
            , string[] arrMergeTestArea
            , string[] arrMergeTestAreaDesc
            , bool isView)
        {
            #region ' Variable ' 
            T_PRB oPrb = null;
            T_TPS_LOT oLot = null;
            T_TPS_LOT_SUM oLotSum = null;
            T_TPS_WAFER_SUM oWaferSum = null;
            T_TPS_BINDESC oBinDesc = null;
            T_TPS_MERGERULE oMergeRule = null;
            T_TPS_TEMP_MERGE oDoMerge = null;
            CommonMethods commonMethod = null;

            string strWaferSeq;
            string TestArea;

            string MergeID;

            string Bin_Dynamic = string.Empty;
            string BinGec_Dynamic = string.Empty;
            string VI_Dynamic = string.Empty;
            string VI_Dynamic_View = string.Empty;
            string VIGec_Dynamic = string.Empty;
            
            string TableName = string.Empty;
            string TableName1 = string.Empty;
            string TableName2 = string.Empty;

            string TempTableName = string.Empty;

            DataTable dtBinData = null;
            DataSet dsResult = null;

            bool isDrop = false;

            #endregion ------------------------------

            try
            {
                /// [[ Merge 하는 순서 ]]
                /// 
                /// 1. 각 정보들을 얻어낸다. (Customer, Product 등등)
                /// 
                /// 2. Merge Rule을 얻어낸다 (Merge 에 쓰일 TestArea, Order)
                /// 
                /// 3. 각각의 Table 에 흩어져 있는 Data들을 Temp Table 로 모은다 (Insert : Copy)
                /// 
                /// 4. Merge Rule 의 우선순위에 따라 Query를 구성하고 Select 한다 (Merge)

                oPrb = new T_PRB();
                oWaferSum = new T_TPS_WAFER_SUM();
                oMergeRule = new T_TPS_MERGERULE();
                oLot = new T_TPS_LOT();
                oLotSum = new T_TPS_LOT_SUM();
                oBinDesc = new T_TPS_BINDESC();
                oDoMerge = new T_TPS_TEMP_MERGE();
                dsResult = new DataSet();
                commonMethod = new CommonMethods();
                
                MergeID = WaferID;

                DateTime dtNotDate = DateTime.Now;

                TempTableName = WaferID.Replace('.','_').Replace('-','_').Replace(" ","") + dtNotDate.Millisecond.ToString();

                oDoMerge.DeleteData(MergeID);

                // Table Create
                string contents = string.Empty;
                for (int i = 0; i < arrMergeTestArea.Length; i++)
                {
                    string BIN_Dynamic_TEMP_TABLE = m_BIN_Dynamic_TEMP_TABLE;
                    string BIN_GEC_Dynamic_TEMP_TABLE = m_BIN_GEC_Dynamic_TEMP_TABLE;
                    string VI_Dynamic_TEMP_TABLE = m_VI_Dynamic_TEMP_TABLE;
                    string VI_GEC_Dynamic_TEMP_TABLE = m_VI_GEC_Dynamic_TEMP_TABLE;
                        
                    contents += BIN_Dynamic_TEMP_TABLE.Replace(":testarea", arrMergeTestArea[i]) + ",\n";
                    contents += BIN_GEC_Dynamic_TEMP_TABLE.Replace(":testarea", arrMergeTestArea[i]) + ",\n";
                    contents += VI_Dynamic_TEMP_TABLE.Replace(":testarea", arrMergeTestArea[i]) + ",\n";
                    contents += VI_GEC_Dynamic_TEMP_TABLE.Replace(":testarea", arrMergeTestArea[i]) + ",\n";
                }

                oDoMerge.CreateTempTable(TempTableName, contents.Substring(0,contents.Length - 2));


                // 대상 TestArea 의 Row Data 들 Input
                for (int i = 0; i < arrMergeTestArea.Length; i++)
                {
                    string Bin_Dynamic_when = m_Bin_Dynamic_when;
                    string Bin_Dynamic_else = m_Bin_Dynamic_else;
                    string BinGec_Dynamic_when = m_BinGec_Dynamic_when;
                    string BinGec_Dynamic_else = m_BinGec_Dynamic_else;
                    string VI_Dynamic_when = m_VI_Dynamic_when;
                    string VI_Dynamic_else = m_VI_Dynamic_else;
                    string VIGec_Dynamic_when = m_VIGec_Dynamic_when;
                    string VIGec_Dynamic_else = m_VIGec_Dynamic_else;

                    TestArea = arrMergeTestArea[i];
                    string TestAreaGroup = arrMergeTestAreaDesc[i];

                    // [ INSET ] =================================================

                    // Exist ??
                    DataTable dt = oDoMerge.SelectIS(TempTableName, MergeID);

                    // AVI
                    if (TestAreaGroup == "AVI")
                    {
                        string WaferIDForAVI = string.Empty;
                        if (WaferID.Length > 2)
                        {
                            WaferIDForAVI = WaferID.Substring(WaferID.Length - 2, 2);
                        }

                        if (dt == null || dt.Rows.Count < 1)
                            oDoMerge.InsertDataAVI(TempTableName, TestArea, LotID, WaferID.Substring(WaferID.Length - 2, 2), TestArea);
                        else
                            oDoMerge.UpdateDataAVI(TempTableName, TestArea, LotID, WaferID.Substring(WaferID.Length - 2, 2), TestArea);
                    }

                    // VI PASS
                    else if (TestAreaGroup.Contains("VI")) { }

                    // INCUST, PROBETEST, AOI, 
                    else
                    {
                        string strTable = string.Empty;

                        // WaferSeq (With out PRODUCT)
                        DataSet dsWaferSum = oWaferSum.GetWaferInfoWithoutProduct(TestArea, LotID, WaferID);
                        if (dsWaferSum.Tables[0] == null || dsWaferSum.Tables[0].Rows.Count < 1)
                            throw new Exception("TestArea is not exist. Check the rule and Data");

                        strWaferSeq = dsWaferSum.Tables[0].Rows[0]["WAFER_SEQ"].ToString();
                        

                        // Table Name
                        if (TestArea == "PROBETEST") strTable = "T_PRB_" + Product.ToUpper().Replace('-', '_').Replace(" ", "");
                        else strTable = "T_" + TestArea;

                        if (dt == null || dt.Rows.Count < 1)
                            oDoMerge.InsertDataNotAVI(TempTableName, TestArea, strTable, strWaferSeq);
                        else
                            oDoMerge.UpdateDataNotAVI(TempTableName, TestArea, strTable, strWaferSeq);
                    }

                    // [ Make Dynamic Query ] ==================================================================

                    Bin_Dynamic_when = Bin_Dynamic_when.Replace(":testarea", TestArea);
                    Bin_Dynamic_else = Bin_Dynamic_else.Replace(":testarea", TestArea);
                    BinGec_Dynamic_when = BinGec_Dynamic_when.Replace(":testarea", TestArea);
                    BinGec_Dynamic_else = BinGec_Dynamic_else.Replace(":testarea", TestArea);
                    VI_Dynamic_when = VI_Dynamic_when.Replace(":testarea", TestArea);
                    VI_Dynamic_else = VI_Dynamic_else.Replace(":testarea", TestArea);
                    VIGec_Dynamic_when = VIGec_Dynamic_when.Replace(":testarea", TestArea);
                    VIGec_Dynamic_else = VIGec_Dynamic_else.Replace(":testarea", TestArea);

                    if (i + 1 < arrMergeTestArea.Length)
                    {
                        if (isView)
                        {
                            Bin_Dynamic += (Bin_Dynamic_when + "\n\t\t");
                            BinGec_Dynamic += (BinGec_Dynamic_when + "\n\t\t");
                            VI_Dynamic += (VI_Dynamic_when + "\n\t\t");
                            VI_Dynamic_View += (VI_Dynamic_when + "\n\t\t");
                            VIGec_Dynamic += (VIGec_Dynamic_when + "\n\t\t");
                        }
                        else
                        {
                            Bin_Dynamic += (Bin_Dynamic_when + "\n\t\t" + VI_Dynamic_when + "\n\t\t");
                            BinGec_Dynamic += (BinGec_Dynamic_when + "\n\t\t" + VIGec_Dynamic_when + "\n\t\t");
                            VI_Dynamic += (VI_Dynamic_when + "\n\t\t");
                            VI_Dynamic_View += (VI_Dynamic_when + "\n\t\t");
                            VIGec_Dynamic += (VIGec_Dynamic_when + "\n\t\t");
                        }
                        
                    }
                    else
                    {
                        if (isView)
                        {
                            Bin_Dynamic += (Bin_Dynamic_when + "\n" + Bin_Dynamic_else);
                            BinGec_Dynamic += (BinGec_Dynamic_when + "\n" + BinGec_Dynamic_else);
                            VI_Dynamic += (VI_Dynamic_when + "\n" + VI_Dynamic_else);
                            VI_Dynamic_View += (VI_Dynamic_when + "\n");
                            VIGec_Dynamic += (VIGec_Dynamic_when + "\n" + VIGec_Dynamic_else);
                        }

                        else
                        {
                            Bin_Dynamic += (Bin_Dynamic_when + "\n" + VI_Dynamic_when + "\n" + Bin_Dynamic_else);
                            BinGec_Dynamic += (BinGec_Dynamic_when + "\n" + VIGec_Dynamic_when + "\n" + BinGec_Dynamic_else);
                            VI_Dynamic += (VI_Dynamic_when + "\n" + VI_Dynamic_else);
                            VI_Dynamic_View += (VI_Dynamic_when + "\n");
                            VIGec_Dynamic += (VIGec_Dynamic_when + "\n" + VIGec_Dynamic_else);
                        }
                    }
                }

                // [ Merge : return result ] =======================================================================
                
                // === Merge ===
                if (!isView)
                {
                    dtBinData = oDoMerge.SelectMerge(TempTableName, Bin_Dynamic, BinGec_Dynamic, VI_Dynamic, VIGec_Dynamic, MergeID);
                    dtBinData.TableName = "BINDATA";
                    dsResult.Tables.Add(dtBinData);

                    //SUM Data
                    DataTable dtSumReturn = GetSumData(dtBinData);
                    dtSumReturn.TableName = "SUMDATA";
                    dsResult.Tables.Add(dtSumReturn);
                }
                // === Merge For View ====
                else
                {
                    string VITargetTestArea = "PROBETEST";

                    DataTable dtWaferSum = oWaferSum.SelectWaferSumInfo(LotID, WaferID, VITargetTestArea);
                    string WaferSeq = dtWaferSum.Rows[0]["WAFER_SEQ"].ToString();

                    T_TPS_TESTAREA oTestArea = new T_TPS_TESTAREA();
                    DataTable dtTestArea = oTestArea.SelectTestArea(VITargetTestArea);

                    // NOT PRB TABLE : Table Name
                    string VITableName = dtTestArea.Rows[0]["SAVE_TABLE"].ToString();
                    
                    // PRB Table : PRODUCT NAME Attach Require
                    if (VITableName.Contains("%"))
                        VITableName = VITableName.Replace("%", Product.Replace("-", "_").Replace(".", "_"));

                    // Merge Foe Vi 는 기존 Table의 VI 값을 같이 Merge
                    dtBinData = oDoMerge.SelectMergeForView(TempTableName, VI_Dynamic_View, Bin_Dynamic, MergeID, WaferSeq, VITableName);
                    dsResult.Tables.Add(dtBinData);
                }

                // Temp Table Drop
                oDoMerge.DropTempTable(TempTableName);
                isDrop = true;

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // [ DROP TABLE ] =======================================================================================
                if(!isDrop)
                    oDoMerge.DropTempTable(TempTableName);
            }
        }
              

        public DataTable GetSumData(DataTable dtBinData)
        {
            DataTable dtReturn;


            int[] arrBinData = new int[256];
            int VICnt = 0;
            int GoodCnt = 0;
            int FailCnt = 0;

            int iBin = 0;
            char BinGec; 
            int iVI = 0;
            char VIGec;
            try
            {
                dtReturn = new DataTable();
                for (int i = 1; i <= 255; i++)
                {
                    dtReturn.Columns.Add(string.Format("{0}", i.ToString()), typeof(int));
                }
                dtReturn.Columns.Add("VISUALINSP", typeof(int));
                dtReturn.Columns.Add("GEC", typeof(int));
                dtReturn.Columns.Add("LOSS_DIE", typeof(int));
                DataRow dr = dtReturn.NewRow();
                dtReturn.Rows.Add(dr);

                for(int i = 0; i< dtBinData.Rows.Count; i++)
                {
                    iBin = Convert.ToInt32(dtBinData.Rows[i][2].ToString());
                    BinGec = Convert.ToChar(dtBinData.Rows[i][3].ToString());
                    iVI = Convert.ToInt32(dtBinData.Rows[i][4].ToString());
                    VIGec = Convert.ToChar(dtBinData.Rows[i][5].ToString());

                    arrBinData[iBin]++;

                    if (BinGec == 'T')
                    {
                        if (VIGec == 'T')
                        {
                            GoodCnt++;
                        }
                        else
                        {
                            VICnt++;
                            FailCnt++;
                        }
                    }
                    else
                        FailCnt++;
                }

                for (int i = 0; i < 255; i++)
                {
                    dtReturn.Rows[0][i] = arrBinData[i+1];
                }
                dtReturn.Rows[0]["VISUALINSP"] = VICnt;
                dtReturn.Rows[0]["GEC"] = GoodCnt;
                dtReturn.Rows[0]["LOSS_DIE"] = FailCnt;

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string ConvertToList(string[] TestArea)
        {
            string strWaferID = string.Empty;

            try
            {
                if (TestArea[0].Contains("'"))
                    strWaferID = string.Join("','", TestArea);
                else
                    strWaferID = "'" + string.Join("','", TestArea) + "'";

                return strWaferID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


