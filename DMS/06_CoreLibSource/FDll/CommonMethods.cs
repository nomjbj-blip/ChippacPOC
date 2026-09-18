using System;
using System.IO;
using System.Collections;
using System.Text;
using DACrux.TEST.DSL;
using DACrux.TEST.RO;
using System.Data;

namespace FDll
{
    public class CommonMethods
    {
        public DataTable GetMergeRule(string Customer, string Product, string ActTestArea)
        {
            T_TPS_MERGERULE oMergeRule = null;

            string Contents = null;
            DataTable dtMergeRule = null;

            try
            {
                Contents = string.Empty;
                oMergeRule = new T_TPS_MERGERULE();

                dtMergeRule = oMergeRule.SelectMergeRuleAtOnce(Customer, Product, ActTestArea);

                if (dtMergeRule == null || dtMergeRule.Rows.Count < 1)
                    throw new Exception("Merge Rule is undefinded");

                return dtMergeRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeRule(ref string Customer, ref string Product, ref string ActTestArea)
        {
            T_TPS_MERGERULE oMergeRule = null;

            string Contents = null;
            DataTable dtMergeRule = null;

            try
            {
                Contents = string.Empty;
                oMergeRule = new T_TPS_MERGERULE();

                dtMergeRule = oMergeRule.SelectMergeRuleAtOnce(Customer, Product, ActTestArea);

                if (dtMergeRule == null || dtMergeRule.Rows.Count < 1)
                    throw new Exception("Merge Rule is undefinded");

                Customer = dtMergeRule.Rows[0]["CUSTOMER"].ToString();
                Product = dtMergeRule.Rows[0]["PRODUCT"].ToString();
                ActTestArea = dtMergeRule.Rows[0]["ACT_TESTAREA"].ToString();

                return dtMergeRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeRuleByRuleName(string Customer, string Product, string RuleName)
        {
            T_TPS_MERGERULE oMergeRule = null;

            DataTable dtMergeRule = null;

            try
            {
                oMergeRule = new T_TPS_MERGERULE();

                dtMergeRule = oMergeRule.SelectMergeRuleByRuleName(Customer, Product, RuleName);
                
                if (dtMergeRule == null || dtMergeRule.Rows.Count < 1)
                    throw new Exception("Merge Rule is undefinded");

                return dtMergeRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeRuleByRuleName(ref string Customer, ref string Product, ref string RuleName)
        {
            T_TPS_MERGERULE oMergeRule = null;

            DataTable dtMergeRule = null;

            try
            {
                oMergeRule = new T_TPS_MERGERULE();

                dtMergeRule = oMergeRule.SelectMergeRuleByRuleName(Customer, Product, RuleName);

                if (dtMergeRule == null || dtMergeRule.Rows.Count < 1)
                    throw new Exception("Merge Rule is undefinded");

                Customer = dtMergeRule.Rows[0]["CUSTOMER"].ToString();
                Product = dtMergeRule.Rows[0]["PRODUCT"].ToString();
                RuleName = dtMergeRule.Rows[0]["RULE_NAME"].ToString();

                return dtMergeRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinByAllAtOnce(string Customer, string Product, string TestArea, string Format)
        {
            T_TPS_BINDESC oBindescDSL = null;

            DataTable dtBinDesc = null;

            try
            {
                oBindescDSL = new T_TPS_BINDESC();

                Customer = Customer.Length == 0 ? " " : Customer;
                Product = Product.Length == 0 ? " " : Product;
                TestArea = TestArea.Length == 0 ? " " : TestArea;
                Format = Format.Length == 0 ? " " : Format;

                return dtBinDesc = oBindescDSL.SelectBinByAllAtOnce(Customer, Product, TestArea, Format);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable SelectGecBinByAllAtOnce(string Customer, string Product, string TestArea, string Format)
        {
            T_TPS_BINDESC oDSL = null;

            try
            {
                oDSL = new T_TPS_BINDESC();

                Customer = Customer.Length == 0 ? " " : Customer;
                Product = Product.Length == 0 ? " " : Product;
                TestArea = TestArea.Length == 0 ? " " : TestArea;
                Format = Format.Length == 0 ? " " : Format;

                return oDSL.SelectGecBinByAllAtOnce(Customer, Product, TestArea, Format);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public DataTable SelectBinByAllAtOnce(ref string Customer, ref string Product, ref string TestArea, ref string Format)
        {
            T_TPS_BINDESC oBindescDSL = null;

            string Contents = null;
            DataTable dtBinDesc = null;

            try
            {
                Contents = string.Empty;
                oBindescDSL = new T_TPS_BINDESC();

                Customer = Customer.Length == 0 ? " " : Customer;
                Product = Product.Length == 0 ? " " : Product;
                TestArea = TestArea.Length == 0 ? " " : TestArea;
                Format = Format.Length == 0 ? " " : Format;

                dtBinDesc = oBindescDSL.SelectBinByAllAtOnce(Customer, Product, TestArea, Format);

                Customer = dtBinDesc.Rows[0]["CUSTOMER"].ToString();
                Product = dtBinDesc.Rows[0]["PRODUCT"].ToString();
                TestArea = dtBinDesc.Rows[0]["TESTAREA"].ToString();
                Format = dtBinDesc.Rows[0]["FORMAT"].ToString();

                return dtBinDesc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeBinDesc(string LotID, string WaferID, string Product, string ActTestArea, bool isFromMes)
        {
            T_TPS_LOT_SUM oDSLLotSum = null;
            T_TPS_WAFER_SUM oDSLWaferSum = null;
            MWIPLOTSTS oMwipLot = null;

            string strCustomer = string.Empty;
            string strProduct = string.Empty;
            string strFormat = string.Empty;

            try
            {
                oDSLLotSum = new T_TPS_LOT_SUM();
                oDSLWaferSum = new T_TPS_WAFER_SUM();

                // Get Customer
                DataTable dtLotSum = oDSLLotSum.SelectLotSumByLotID(LotID);
                foreach (DataRow dr in dtLotSum.Rows)
                {
                    strCustomer = dr["CUSTOMER"].ToString();
                    if (strCustomer.Length > 0)
                        break;
                }

                // Get Product From MES DB =====================================================
                
                if (!isFromMes) 
                {
                    oMwipLot = new MWIPLOTSTS();

                    // Select Product From MES (if not exist in MES, use DMS Product)
                    DataTable dt = oMwipLot.SelectProductByCusLotID(LotID);
                    // MES 에 없는 Lot 이라면 해당 Lot 의 Product 을 사용
                    if (dt == null || dt.Rows.Count < 1)
                        strProduct = dtLotSum.Rows[0]["PRODUCT"].ToString();
                    // MES 에 있는 Lot 이라면 MES의 Product 을 사용
                    else
                        strProduct = dt.Rows[0][0].ToString();
                }
                // From MES (Using MES PRODUCT)
                else
                {
                    // MES 에서 요청한 Call 이면 MES의 Product 을 사용
                    strProduct = Product;
                }

                // Merge Rule Select
                DataTable dtMergeRule = null;

                if (ActTestArea.Contains("MERGE"))
                    dtMergeRule = this.GetMergeRuleByRuleName(strCustomer, strProduct, ActTestArea);
                else
                    dtMergeRule = this.GetMergeRule(strCustomer, strProduct, ActTestArea);

                return GetMergeBinDesc(strCustomer, LotID, WaferID, strProduct, dtMergeRule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMergeBinDesc(string Customer, string LotID, string WaferID, string Product, DataTable dtMergeRule)
        {
            T_TPS_WAFER_SUM oDSLWaferSum = null;
            DataTable dtAllTestAreaBindesc = null;
            T_TPS_TESTAREA oDSLTestArea = null;

            string TestArea = string.Empty;
            string strFormat = string.Empty;
            string TestAreaGroup = string.Empty;

            try
            {
                oDSLWaferSum = new T_TPS_WAFER_SUM();
                dtAllTestAreaBindesc = new DataTable();
                oDSLTestArea = new T_TPS_TESTAREA();

                for (int i = 0; i < dtMergeRule.Rows.Count; i++)
                {
                    TestArea = dtMergeRule.Rows[i]["RULE_VALUE"].ToString();
                    TestAreaGroup = oDSLTestArea.SelectTestArea(TestArea).Rows[0]["AREA_DESC"].ToString();

                    // Format Search ====================================================================
                    if (TestAreaGroup == "AVI")
                    {
                        strFormat = string.Empty;
                    }
                    else
                    {
                        // Merge 와 관련된 모든 공정 들고옴 (Format을 위해 필요)
                        DataTable dtFormat = oDSLWaferSum.GetWaferInfo(TestArea, LotID, WaferID, true);
                        strFormat = dtFormat.Rows[0]["FORMAT"].ToString();
                    }
                    
                    // Get BinDesc ============================================================================
                    DataTable dtBinDesc = SelectBinByAllAtOnce(Customer, Product, TestArea, strFormat);

                    if (i == 0)
                    {
                        dtAllTestAreaBindesc = dtBinDesc.Clone();
                        // 중복 방지 Unique 조건 추가
                        dtAllTestAreaBindesc.Columns["BIN"].Unique = true;
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        // VI 의 경우 새로 검색해서 넣어준다.
                        if (j == 1)
                        {
                            // TestArea Group 알기
                            if (TestAreaGroup == "AVI")
                                TestArea = "VI_AVI";
                            else
                                TestArea = "VI_" + TestArea;

                            strFormat = string.Empty;
                            dtBinDesc = SelectBinByAllAtOnce(Customer, Product, TestArea, strFormat);
                        }
                        
                        // Row Import (Merge)
                        foreach (DataRow dr in dtBinDesc.Rows)
                        {
                            try
                            {
                                // Bin 중복이 발생하면 해당 Row는 SKIP (BIN : 1 중복 방지)
                                dtAllTestAreaBindesc.ImportRow(dr);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }

                dtAllTestAreaBindesc = dtAllTestAreaBindesc.Select("", " BIN ASC").CopyToDataTable<DataRow>();

                return dtAllTestAreaBindesc;
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        //private DataTable GetMergeBinDesc(string LotID, string WaferID, string[] 

        public bool GoldenCheck(DataTable dtParsingBin, string[,] arrBinData_Parsing, HeaderData hd)
        {
            ADMIN_DATA oAdmin = null;

            int iMinX_Parsing;
            int iMaxX_Parsing;
            int iMinY_Parsing;
            int iMaxY_Parsing;

            int iMinX_Golden;
            int iMaxX_Golden;
            int iMinY_Golden;
            int iMaxY_Golden;

            string[,] arrBinData_Golden;
            DataTable dtGoldenBin;

            try
            {
                oAdmin = new ADMIN_DATA();

                // Select Golden Map Bin Data
                dtGoldenBin = oAdmin.GetDies(hd.Device, true).Tables[0];

                iMinX_Parsing = Convert.ToInt32(dtParsingBin.Compute("MIN(X)", null).ToString());
                iMaxX_Parsing = Convert.ToInt32(dtParsingBin.Compute("MAX(X)", null).ToString());
                iMinY_Parsing = Convert.ToInt32(dtParsingBin.Compute("MIN(Y)", null).ToString());
                iMaxY_Parsing = Convert.ToInt32(dtParsingBin.Compute("MAX(Y)", null).ToString());

                iMinX_Golden = Convert.ToInt32(dtGoldenBin.Compute("MIN(X)", null).ToString());
                iMaxX_Golden = Convert.ToInt32(dtGoldenBin.Compute("MAX(X)", null).ToString());
                iMinY_Golden = Convert.ToInt32(dtGoldenBin.Compute("MIN(Y)", null).ToString());
                iMaxY_Golden = Convert.ToInt32(dtGoldenBin.Compute("MAX(Y)", null).ToString());

                // Compare Min Max
                if (iMinX_Parsing != iMinX_Golden || iMaxX_Parsing != iMaxX_Golden
                    || iMinY_Parsing != iMinY_Golden || iMaxY_Parsing != iMaxY_Golden) 
                    return false;

                // ConVert Data ro Array
                int x; int y;
                arrBinData_Golden = new string[iMaxX_Golden+1, iMaxY_Golden+1];
                foreach (DataRow dr in dtGoldenBin.Rows)
                {
                    x = Convert.ToInt32(dr["X"].ToString());
                    y = Convert.ToInt32(dr["Y"].ToString());
                    arrBinData_Golden[x, y] = "0";
                }

                // Comapre Exist
                for(int Y = iMinY_Golden; Y < iMaxY_Golden; Y++)
                {
                    for(int X = iMaxX_Golden; X < iMaxX_Golden; X++)
                    {
                        // Exist Check
                        if(string.IsNullOrEmpty(arrBinData_Parsing[X, Y]) && !string.IsNullOrEmpty(arrBinData_Golden[X, Y]))
                            return false;
                        else if (!string.IsNullOrEmpty(arrBinData_Parsing[X, Y]) && string.IsNullOrEmpty(arrBinData_Golden[X, Y]))
                            return false;
                    }
                }

                // Compare Success (Same)
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
